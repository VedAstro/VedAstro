using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
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
                //todo proper log
                Console.WriteLine(e.Message);
                return "";
            }

            throw new Exception("END OF LINE!");

        }


        //}
        public static async Task<BlobClient?> AddLarge<T>(string callerId, T value, string mimeType = "")
        {
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
    }
}
