using System.Net;
using System.Net.Mime;
using System.Text;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Azure.Functions.Worker.Http;
using VedAstro.Library;

namespace API
{
    /// <summary>
    /// cache manager for storing in Azure blobs as cache
    /// </summary>
    public static class AzureCache
    {
        private static readonly BlobContainerClient blobContainerClient;
        private const string blobContainerName = "cache";

        static AzureCache()
        {
            //get the connection string stored separately (for security reasons)
            //note: dark art secrets are in local.settings.json
            var storageConnectionString = Secrets.Get("API_STORAGE");

            //get image from storage
            blobContainerClient = new BlobContainerClient(storageConnectionString, blobContainerName);

        }

        public static List<BlobItem> ListBlobs(string searchKeyword)
        {
            var blobNames = new List<BlobItem>();

            var blobItems = blobContainerClient.GetBlobs(prefix: searchKeyword).ToList();

            //foreach (var blobItem in blobItems)
            //{
            //    if (blobItem.Name.Contains(searchKeyword))
            //    {
            //        blobNames.Add(blobItem);
            //    }
            //}

            return blobItems;
        }

        public static async Task<bool> IsExist(string callerId)
        {
            //#if DEBUG
            //            Console.WriteLine("CACHE TURNED OFF IN DEBUG MODE!!");
            //            return false;
            //#endif

            BlobClient blobClient = blobContainerClient.GetBlobClient(callerId);

            bool isExists = await blobClient.ExistsAsync(CancellationToken.None);

            //if found in blob then end here
            return isExists;
        }

        //public static async Task<string> GetLarge(string callerId)
        //{
        //    BlobClient blobClient = blobContainerClient.GetBlobClient(callerId);

        //    var data = await APITools.BlobClientToString(blobClient);
        //    return data;
        //}
        public static async Task<dynamic> GetData<T>(string callerId)
        {

            try
            {
                BlobClient blobClient = blobContainerClient.GetBlobClient(callerId);


                if (typeof(T) == typeof(string))
                {
                    var data = await Tools.BlobClientToString(blobClient);
                    return data;

                }
                else if (typeof(T) == typeof(byte[]))
                {
                    using var ms = new MemoryStream();
                    await blobClient.DownloadToAsync(ms);
                    return ms.ToArray();
                }
                else if (typeof(T) == typeof(BlobClient))
                {

                    return blobClient;
                }


            }
            catch (Exception e)
            {
                APILogger.Error(e); //log it
                return "";
            }

            throw new Exception("END OF LINE!");

        }

        /// <summary>
        /// Given a any data type, will add to Cache container, with specified name, mimetype is optional
        /// </summary>
        public static async Task<BlobClient?> Add<T>(string fileName, T value, string mimeType = "", Dictionary<string, string> metadata = null)
        {

#if DEBUG
            Console.WriteLine($"SAVING NEW DATA TO CACHE: {fileName}");
#endif


            var blobClient = blobContainerClient.GetBlobClient(fileName);

            if (typeof(T) == typeof(string))
            {
                var stringToSave = value as string ?? string.Empty;

                //NOTE:set UTF 8 so when taking out will go fine
                var content = Encoding.UTF8.GetBytes(stringToSave);
                using var ms = new MemoryStream(content);

                var blobUploadOptions = new BlobUploadOptions();
                blobUploadOptions.AccessTier = AccessTier.Cool; //save money!

                //note no overwrite needed because specifying BlobUploadOptions, is auto overwrite
                //TODO metadata option for future
                var inputMetaData = metadata ?? new Dictionary<string, string>();
                await blobClient.UploadAsync(ms, metadata: inputMetaData, accessTier: AccessTier.Cool);

            }
            else if (typeof(T) == typeof(byte[]))
            {
                var byteArrayData = value as byte[];
                using var ms = new MemoryStream(byteArrayData, false);

                var blobUploadOptions = new BlobUploadOptions();
                blobUploadOptions.AccessTier = AccessTier.Cool; //save money!

                //note no override needed because specifying BlobUploadOptions, is auto override
                await blobClient.UploadAsync(ms, options: blobUploadOptions);

                //var xx = new BinaryData(value, JsonSerializerOptions.Default);
                //blobClient.UploadAsync(ms, options: blobUploadOptions);
            }

            //if specified
            if (!(string.IsNullOrEmpty(mimeType)))
            {
                //auto correct content type from wrongly set "octet/stream"
                var blobHttpHeaders = new BlobHttpHeaders { ContentType = mimeType };
                await blobClient.SetHttpHeadersAsync(blobHttpHeaders);
            }

            //set as COOL since file should be living for long
            //where cache is expected to live long
            await blobClient.SetAccessTierAsync(AccessTier.Cool);

            return blobClient;

        }

        public static async Task Delete(string callerId)
        {
            var blobClient = blobContainerClient.GetBlobClient(callerId);

            var result = await blobClient.DeleteIfExistsAsync();

            //if result unexpected raise alarm
            if (result?.Value == false) { APILogger.Error($"WARNING! FILE DID NOT EXIST : {callerId}"); }
        }

        /// <summary>
        /// If got data use that, else do calculations and give that
        /// Also acts as polling URL, client only has to refresh to poll
        /// response will auto change to full data file when needed
        /// NOTE : HEADERS USED TO MARK STATUS PASS OR FAIL
        /// </summary>
        public static async Task<HttpResponseData> CacheExecute(Func<Task<BlobClient>> cacheExecuteTask3,
            CallerInfo callerInfo, HttpRequestData httpRequestData)
        {

            //1: CHECK IF RUNNING
            //check if call already made and is running
            //call is made again (polling), don't disturb running
            var isRunning = CallTracker.IsRunning(callerInfo.CallerId);

#if DEBUG
            var status = isRunning ? "ATM RUNNING" : "NOT RUNNING / NON EXIST";
            Console.WriteLine($"CALL IS {status}");
#endif

            //already running end here for quick reply
            if (isRunning)
            {
                var response = httpRequestData.CreateResponse(HttpStatusCode.OK);
                response.Headers.Add("Call-Status", "Running"); //caller checks this
                response.Headers.Add("Access-Control-Expose-Headers", "Call-Status"); //needed by silly browser to read call-status
                return response;
            }
            //start new call
            else
            {
                //2: CHECK IF CACHED
                //if task not running next check cache
                var gotCache = await AzureCache.IsExist(callerInfo.CallerId);
                if (gotCache)
                {
                    BlobClient chartBlobClient = await AzureCache.GetData<BlobClient>(callerInfo.CallerId);

#if DEBUG
                    var xxx = chartBlobClient.GetProperties().Value.ContentLength;
                    Console.WriteLine($"USING CACHE : {callerInfo.CallerId} SIZE:{xxx}");
#endif

                    var httpResponseData = Tools.SendPassHeaderToCaller(chartBlobClient, httpRequestData, MediaTypeNames.Application.Json);
                    return httpResponseData;

                }
                //if no cache only now start task
                else
                {

#if DEBUG
                    Console.WriteLine($"NO CACHE! RUNNING COMPUTE : {callerInfo.CallerId}");
#endif
                    //no waiting
                    //will execute and save the data to cache,
                    //so on next call will retrieve from cache
                    cacheExecuteTask3.Invoke();


#if DEBUG
                    Console.WriteLine($"BUSY NOW COME BACK LATER : {callerInfo.CallerId}");
#endif

                    var response = httpRequestData.CreateResponse(HttpStatusCode.OK);
                    response.Headers.Add("Call-Status", "Running"); //caller checks this
                    response.Headers.Add("Access-Control-Expose-Headers", "Call-Status"); //needed by silly browser to read call-status
                    return response;

                }



            }
        }

        /// <summary>
        /// Relies on cache names having prefix person ID to be detected and deleted
        /// Exp: Travis1985-EventsChart-20010202...
        /// </summary>
        public static async Task DeleteCacheRelatedToPerson(Person newPerson)
        {
            //if empty id, end here
            if (Person.Empty.Equals(newPerson)) { return; }

            //person is placed infront if that cache belongs to that person
            //as such get all cache such way and delete
            var foundCaches = blobContainerClient.GetBlobs(BlobTraits.All, BlobStates.None, newPerson.Id);

            //delete all cache
            foreach (var cache in foundCaches)
            {
                await blobContainerClient.DeleteBlobIfExistsAsync(cache.Name, DeleteSnapshotsOption.None);
            }
        }

        /// <summary>
        /// Relies on cache names having prefix person ID to be detected and deleted
        /// Exp: Travis1985-EventsChart-20010202...
        /// </summary>
        public static async Task DeleteCacheRelatedToPerson(string personId)
        {
            //if empty id, end here
            if (personId == "Empty") { return; }

            //person is placed in front if that cache belongs to that person
            //as such get all cache such way and delete
            var foundCaches = blobContainerClient.GetBlobs(BlobTraits.All, BlobStates.None, personId);

            //delete all cache
            foreach (var cache in foundCaches)
            {
                await blobContainerClient.DeleteBlobIfExistsAsync(cache.Name, DeleteSnapshotsOption.None);
            }
        }
    }
}
