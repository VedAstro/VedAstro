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
        private static readonly TableServiceClient tableServiceClient;
        private static string tableName = "FunctionCache";
        private static readonly TableClient tableClient;
        private static readonly BlobContainerClient blobContainerClient;
        private const string blobContainerName = "cache";

        static AzureCache()
        {
            //storageexplorer://v=1&accountid=%2Fsubscriptions%2Ff453f954-107f-4b85-b1d3-744b013dfd5d%2FresourceGroups%2FVedAstro%2Fproviders%2FMicrosoft.Storage%2FstorageAccounts%2Fvedastroapistorage&subscriptionid=f453f954-107f-4b85-b1d3-744b013dfd5d&resourcetype=Azure.Table&resourcename=FunctionCache
            var storageUri = "https://vedastroapistorage.table.core.windows.net/FunctionCache";
            string accountName = "vedastroapistorage";
            string storageAccountKey = "kquBbAE8QKhe/Iyny4F0we3Yx6Y/zJv7yZgrERuFq7nRoEa53o6rb50W88II+PsSEP7RzYYNQizDHPOlt0kwxw==";

            //save reference for late use
            tableServiceClient = new TableServiceClient(new Uri(storageUri), new TableSharedKeyCredential(accountName, storageAccountKey));
            tableClient = tableServiceClient.GetTableClient(tableName);

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
            if (isExists)
            {
                return true;
            }

            return IsExistTable(callerId);
        }

        /// <summary>
        /// checks only in table
        /// </summary>
        private static bool IsExistTable(string callerId)
        {
            //else continue to look in table

            Pageable<TableEntity> queryResultsFilter = tableClient.Query<TableEntity>(filter: $"PartitionKey eq '{callerId}'");

            //if more exist there will be something found
            return queryResultsFilter.Any();

        }


        //public static async Task<string> GetLarge(string callerId)
        //{
        //    BlobClient blobClient = blobContainerClient.GetBlobClient(callerId);

        //    var data = await APITools.BlobClientToString(blobClient);
        //    return data;
        //}
        public static async Task<dynamic> GetLarge<T>(string callerId)
        {

            try
            {
                //check if table or blob
                var dataInTable = IsExistTable(callerId);
                if (dataInTable)
                {
                    var yyy = GetDataFromTable(callerId);
                    return yyy;
                }
                else
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

            }
            catch (Exception e)
            {
                //todo proper log
                Console.WriteLine(e.Message);
                return "";
            }

            throw new Exception("END OF LINE!");

        }

        public static object GetDataFromTable(string callerId)
        {
            var queryResultsFilter = tableClient.Query<TableEntity>(filter: $"PartitionKey eq '{callerId}'");

            //if more exist there will be something found
            var cacheRow = queryResultsFilter.FirstOrDefault();
            var cachedData = cacheRow?["Value"] ?? "EMPTY!";

            return cachedData;
        }

        public static async Task AddDataToTable(string callerId, string value)
        {
            //create the data
            var tableEntity = new TableEntity(callerId, "")
            {
                { "Value", value }
            };

            //check if exist
            var xx = await tableClient.GetEntityIfExistsAsync<TableEntity>(callerId, "");

            //not yet born
            if (xx == null)
            {
                // add to cloud store
                await tableClient.AddEntityAsync(tableEntity);
            }
            //update existing
            else
            {
                await tableClient.UpdateEntityAsync(tableEntity, ETag.All, TableUpdateMode.Replace);
            }

        }
        //public static void AddLarge(string callerId, string value)
        //{
        //    BlobClient blobClient = blobContainerClient.GetBlobClient(callerId);

        //    var xx = new BinaryData(value, JsonSerializerOptions.Default);
        //    blobClient.Upload(xx);

        //    blobClient.SetAccessTier(AccessTier.Hot);


        //}
        public static async Task<BlobClient?> AddLarge<T>(string callerId, T value, string mimeType = "")
        {
            var blobClient = blobContainerClient.GetBlobClient(callerId);

            if (typeof(T) == typeof(string))
            {
                var stringToSave = value as string ?? string.Empty;

                //based on string length place at in table or blob
                if (stringToSave.Length < 200)
                {
                    await AddDataToTable(callerId, stringToSave);
                    return null;
                }
                //if text is long then off it goes to BLOB (slow but big)
                else
                {
                    //NOTE:set UTF 8 so when taking out will go fine
                    var content = Encoding.UTF8.GetBytes(stringToSave);
                    using var ms = new MemoryStream(content);
                    await blobClient.UploadAsync(ms, overwrite: true);

                }

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

            //if did not delete blob then most likely table data
            if (!result.Value)
            {
                await tableClient.DeleteEntityAsync(callerId, "");
            }

        }
    }
}
