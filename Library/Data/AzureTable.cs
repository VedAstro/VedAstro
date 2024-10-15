using System;
using Azure.Data.Tables;

namespace VedAstro.Library
{

    /// <summary>
    /// static storage for connection access to Azure tables
    /// </summary>
    public static class AzureTable
    {
        //STORAGE SETTINGS
        public static readonly string AccountName = Secrets.Get("CentralStorageAccountName");
        private static readonly string StorageAccountKey = Secrets.Get("CentralStorageKey");

        //TABLE NAMES
        public const string APIAbuseListName = "APIAbuseList";
        public const string PersonListName = "PersonList";
        public const string UserDataListName = "UserDataList";
        public const string PersonShareListName = "PersonShareList";
        public const string PersonListRecycleBinName = "PersonListRecycleBin";
        public const string LifeEventListName = "LifeEventList";

        //URL
        public static readonly string APIAbuseListUri = $"https://{AccountName}.table.core.windows.net/{APIAbuseListName}";
        public static readonly string PersonListUri = $"https://{AccountName}.table.core.windows.net/{PersonListName}";
        public static readonly string UserDataListUri = $"https://{AccountName}.table.core.windows.net/{UserDataListName}";
        public static readonly string PersonShareListUri = $"https://{AccountName}.table.core.windows.net/{PersonShareListName}";
        public static readonly string PersonListRecycleBinUri = $"https://{AccountName}.table.core.windows.net/{PersonListRecycleBinName}";
        public static readonly string LifeEventListUri = $"https://{AccountName}.table.core.windows.net/{LifeEventListName}";

        //TABLE
        public static readonly TableClient? PersonList = (new TableServiceClient(new Uri(PersonListUri), new TableSharedKeyCredential(AccountName, StorageAccountKey))).GetTableClient(PersonListName);
        public static readonly TableClient? UserDataList = (new TableServiceClient(new Uri(UserDataListUri), new TableSharedKeyCredential(AccountName, StorageAccountKey))).GetTableClient(UserDataListUri);
        public static readonly TableClient? APIAbuseList = (new TableServiceClient(new Uri(APIAbuseListUri), new TableSharedKeyCredential(AccountName, StorageAccountKey))).GetTableClient(APIAbuseListName);
        public static readonly TableClient? PersonListRecycleBin = (new TableServiceClient(new Uri(PersonListRecycleBinUri), new TableSharedKeyCredential(AccountName, StorageAccountKey))).GetTableClient(PersonListRecycleBinName);
        public static readonly TableClient? LifeEventList = (new TableServiceClient(new Uri(LifeEventListUri), new TableSharedKeyCredential(AccountName, StorageAccountKey))).GetTableClient(LifeEventListName);
        
        /// <summary>
        /// Allows multiple users to share 1 person profile, with read & write privileges, with 1 original creator user
        /// People in share list will also appear in drop down list
        /// </summary>
        public static readonly TableClient? PersonShareList = (new TableServiceClient(new Uri(PersonShareListUri), new TableSharedKeyCredential(AccountName, StorageAccountKey))).GetTableClient(PersonShareListName);

    }
}
