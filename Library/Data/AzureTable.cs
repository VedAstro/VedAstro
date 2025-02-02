using System;
using Azure.Data.Tables;

namespace VedAstro.Library
{
    /// <summary>
    /// Static storage for connection access to Azure tables
    /// </summary>
    public static class AzureTable
    {
        // STORAGE SETTINGS
        public static readonly string AccountName = Secrets.Get("CentralStorageAccountName");
        private static readonly string StorageAccountKey = Secrets.Get("CentralStorageKey");


        // CREDENTIALS AND SERVICE CLIENT
        private static readonly TableSharedKeyCredential Credentials = new TableSharedKeyCredential(AccountName, StorageAccountKey);

        private static readonly Uri ServiceUri = new Uri($"https://{AccountName}.table.core.windows.net");

        private static readonly TableServiceClient TableServiceClient = new TableServiceClient(ServiceUri, Credentials);

        // TABLE CLIENTS
        public static readonly TableClient PersonList = TableServiceClient.GetTableClient("PersonList");

        public static readonly TableClient SubscriberCallRecords = TableServiceClient.GetTableClient("SubscriberCallRecords");
        
        public static readonly TableClient AnonymousIpCallRecords = TableServiceClient.GetTableClient("AnonymousIpCallRecords");

        public static readonly TableClient UserDataList = TableServiceClient.GetTableClient("UserDataList");

        public static readonly TableClient LifeEventList = TableServiceClient.GetTableClient("LifeEventList");
        
        public static readonly TableClient OpenAPIErrorBook = TableServiceClient.GetTableClient("OpenAPIErrorBook");
        
        public static readonly TableClient CallTracker = TableServiceClient.GetTableClient("CallTracker");

        public static readonly TableClient WebsiteErrorLog = TableServiceClient.GetTableClient("WebsiteErrorLog");

        public static readonly TableClient WebsiteDebugLog = TableServiceClient.GetTableClient("WebsiteDebugLog");

        /// <summary>
        /// Allows multiple users to share one person profile with read & write privileges.
        /// Shared people will also appear in the drop-down list.
        /// </summary>
        public static readonly TableClient PersonShareList = TableServiceClient.GetTableClient("PersonShareList");
    }
}
