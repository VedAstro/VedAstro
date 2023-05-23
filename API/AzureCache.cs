using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Azure;
using Azure.Data.Tables;
using Azure.Data.Tables.Models;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

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

            //else continue to look in table

            Pageable<TableEntity> queryResultsFilter = tableClient.Query<TableEntity>(filter: $"PartitionKey eq '{callerId}'");

            //if more exist there will be something found
            return queryResultsFilter.Any();
        }


        public static async Task<string> GetLarge(string callerId)
        {
            BlobClient blobClient = blobContainerClient.GetBlobClient(callerId);

            var data = await APITools.BlobClientToString(blobClient);
            return data;
        }

        public static string Get(string callerId)
        {
            Pageable<TableEntity> queryResultsFilter = tableClient.Query<TableEntity>(filter: $"PartitionKey eq '{callerId}'");

            //if more exist there will be something found
            var cacheRow = queryResultsFilter.FirstOrDefault();
            var cachedData = cacheRow?.GetString("Value") ?? "EMPTY!";

            return cachedData;
        }

        public static void Add(string callerId, string value)
        {

            //create the data
            var tableEntity = new TableEntity(callerId, "")
            {
                { "Value", value }
            };

            // add to cloud store
            tableClient.AddEntity(tableEntity);

        }
        public static void AddLarge(string callerId, string value)
        {

            BlobClient blobClient = blobContainerClient.GetBlobClient(callerId);


            var content = Encoding.UTF8.GetBytes(value);
            using (var ms = new MemoryStream(content))
            {
                blobClient.Upload(ms);
            }

            blobClient.SetAccessTier(AccessTier.Hot);


        }
    }
}
