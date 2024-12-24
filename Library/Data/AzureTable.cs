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

        // TABLE NAMES
        public const string APIAbuseListName = "APIAbuseList";
        public const string PersonListName = "PersonList";
        public const string UserDataListName = "UserDataList";
        public const string PersonShareListName = "PersonShareList";
        public const string PersonListRecycleBinName = "PersonListRecycleBin";
        public const string LifeEventListName = "LifeEventList";

        // CREDENTIALS AND SERVICE CLIENT
        private static readonly TableSharedKeyCredential Credentials = new TableSharedKeyCredential(AccountName, StorageAccountKey);

        private static readonly Uri ServiceUri = new Uri($"https://{AccountName}.table.core.windows.net");

        private static readonly TableServiceClient TableServiceClient = new TableServiceClient(ServiceUri, Credentials);

        // TABLE CLIENTS
        public static readonly TableClient PersonList = TableServiceClient.GetTableClient(PersonListName);

        public static readonly TableClient UserDataList = TableServiceClient.GetTableClient(UserDataListName);

        public static readonly TableClient APIAbuseList = TableServiceClient.GetTableClient(APIAbuseListName);

        public static readonly TableClient PersonListRecycleBin = TableServiceClient.GetTableClient(PersonListRecycleBinName);

        public static readonly TableClient LifeEventList = TableServiceClient.GetTableClient(LifeEventListName);

        /// <summary>
        /// Allows multiple users to share one person profile with read & write privileges.
        /// Shared people will also appear in the drop-down list.
        /// </summary>
        public static readonly TableClient PersonShareList = TableServiceClient.GetTableClient(PersonShareListName);
    }
}
