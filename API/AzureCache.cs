using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Azure;
using Azure.Data.Tables;
using Azure.Data.Tables.Models;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using iText.Commons.Bouncycastle.Cert.Ocsp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker.Http;
using VedAstro.Library;

namespace API
{
    /// <summary>
    /// cache manager for storing in Azure Tables
    /// </summary>
    public static class AzureCache
    {
        private static readonly BlobContainerClient blobContainerClient;
        private const string blobContainerName = "cache";

        static AzureCache()
        {


            //get the connection string stored separately (for security reasons)
            //note: dark art secrets are in local.settings.json
            var storageConnectionString = Environment.GetEnvironmentVariable("API_STORAGE"); //vedastro-api-data

            //get image from storage
            blobContainerClient = new BlobContainerClient(storageConnectionString, blobContainerName);


        }


        public static async Task<bool> IsExist(string callerId)
        {

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
                    var data = await APITools.BlobClientToString(blobClient);
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
                await APILogger.Error(e); //log it
                return "";
            }

            throw new Exception("END OF LINE!");

        }

        //}
        public static async Task<BlobClient?> Add<T>(string callerId, T value, string mimeType = "")
        {

#if DEBUG
            Console.WriteLine($"SAVING NEW DATA TO CACHE: {callerId}");
#endif


            var blobClient = blobContainerClient.GetBlobClient(callerId);

            if (typeof(T) == typeof(string))
            {
                var stringToSave = value as string ?? string.Empty;

                //NOTE:set UTF 8 so when taking out will go fine
                var content = Encoding.UTF8.GetBytes(stringToSave);
                using var ms = new MemoryStream(content);
                await blobClient.UploadAsync(ms, overwrite: true);

            }
            else if (typeof(T) == typeof(byte[]))
            {
                var vv = value as byte[];
                using var ms = new MemoryStream(vv, false);
                await blobClient.UploadAsync(ms, overwrite: true);

                //var xx = new BinaryData(value, JsonSerializerOptions.Default);
                //blobClient.Upload(xx);
            }

            //if specified
            if (!(string.IsNullOrEmpty(mimeType)))
            {
                //auto correct content type from wrongly set "octet/stream"
                var blobHttpHeaders = new BlobHttpHeaders { ContentType = mimeType };
                await blobClient.SetHttpHeadersAsync(blobHttpHeaders);
            }

            //set as hot since file should be living for long
            //note : can be changed to cool once in stable production,
            //where cache is expected to live long
            await blobClient.SetAccessTierAsync(AccessTier.Hot);

            return blobClient;

        }

        public static async Task Delete(string callerId)
        {
            var blobClient = blobContainerClient.GetBlobClient(callerId);

            var result = await blobClient.DeleteIfExistsAsync();

            //if result unexpected raise alarm
            if (result?.Value == false)
            {
                Console.WriteLine($"WARNING! FILE DID NOT EXIST : {callerId}");
            }
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
            //check if call already made and is running
            //call is made again (polling), don't disturb running
            var isRunning = CallTracker.IsRunning(callerInfo.CallerId);

#if DEBUG
            var status = isRunning ? "RUNNING" : "DONE";
            Console.WriteLine($"CALL IS {status}");
#endif

            //already running end here for quick reply
            if (isRunning)
            {
                var response = httpRequestData.CreateResponse(HttpStatusCode.OK);
                response.Headers.Add("Call-Status", "Running"); //caller checks this
                return response;
            }
            //start new call
            else
            {

                //if task not running next check cache
                var gotCache = await AzureCache.IsExist(callerInfo.CallerId);
                if (gotCache)
                {
                    BlobClient chartBlobClient = await AzureCache.GetData<BlobClient>(callerInfo.CallerId);

#if DEBUG
                    var xxx = chartBlobClient.GetProperties().Value.ContentLength;
                    Console.WriteLine($"USING CACHE : {callerInfo.CallerId} SIZE:{xxx}");
#endif

                    var httpResponseData = APITools.SendPassHeaderToCaller(chartBlobClient, httpRequestData, MediaTypeNames.Application.Json);
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
                    //so on next call will retreive from cache
                    cacheExecuteTask3.Invoke();


#if DEBUG
                    Console.WriteLine($"BUSY NOW COME BACK LATER : {callerInfo.CallerId}");
#endif

                    var response = httpRequestData.CreateResponse(HttpStatusCode.OK);
                    response.Headers.Add("Call-Status", "Running"); //caller checks this
                    return response;

                }



            }
        }
    }
}
