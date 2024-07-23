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
        public const string AccountName = "vedastrocentralstorage";
        public const string AccountName_Indic = "centralapistorage";
        private static readonly string StorageAccountKey = Secrets.Get("VedAstroCentralStorageKey");
        private static readonly string StorageAccountKey_Indic = Secrets.Get("IndicCentralStorageKey");

        //TABLE NAMES
        public const string APIAbuseListName = "APIAbuseList";
        public const string PersonListName = "PersonList";
        public const string PersonShareListName = "PersonShareList";
        public const string PersonListRecycleBinName = "PersonListRecycleBin";
        public const string LifeEventListName = "LifeEventList";
        
        //URL
        public const string APIAbuseListUri = $"https://vedastrocentralstorage.table.core.windows.net/{APIAbuseListName}";
        public const string PersonListUri = $"https://vedastrocentralstorage.table.core.windows.net/{PersonListName}";
        public const string PersonListUri_Indic = $"https://centralapistorage.table.core.windows.net/{PersonListName}";
        public const string PersonShareListUri = $"https://vedastrocentralstorage.table.core.windows.net/{PersonShareListName}";
        public const string PersonListRecycleBinUri = $"https://vedastrocentralstorage.table.core.windows.net/{PersonListRecycleBinName}";
        public const string LifeEventListUri = $"https://vedastrocentralstorage.table.core.windows.net/{LifeEventListName}";
        public const string LifeEventListUri_Indic = $"https://centralapistorage.table.core.windows.net/{LifeEventListName}";
        //TABLE
        public static readonly TableClient? PersonList = (new TableServiceClient(new Uri(PersonListUri), new TableSharedKeyCredential(AccountName, StorageAccountKey))).GetTableClient(PersonListName);
        //public static readonly TableClient? PersonList_Indic = (new TableServiceClient(new Uri(PersonListUri_Indic), new TableSharedKeyCredential(AccountName_Indic, StorageAccountKey_Indic))).GetTableClient(PersonListName);
        public static readonly TableClient? APIAbuseList = (new TableServiceClient(new Uri(APIAbuseListUri), new TableSharedKeyCredential(AccountName, StorageAccountKey))).GetTableClient(APIAbuseListName);
        public static readonly TableClient? PersonListRecycleBin = (new TableServiceClient(new Uri(PersonListRecycleBinUri), new TableSharedKeyCredential(AccountName, StorageAccountKey))).GetTableClient(PersonListRecycleBinName);
        public static readonly TableClient? LifeEventList = (new TableServiceClient(new Uri(LifeEventListUri), new TableSharedKeyCredential(AccountName, StorageAccountKey))).GetTableClient(LifeEventListName);
        //public static readonly TableClient? LifeEventList_Indic = (new TableServiceClient(new Uri(LifeEventListUri_Indic), new TableSharedKeyCredential(AccountName_Indic, StorageAccountKey_Indic))).GetTableClient(LifeEventListName);
        public static readonly TableClient? PersonShareList = (new TableServiceClient(new Uri(PersonShareListUri), new TableSharedKeyCredential(AccountName, StorageAccountKey))).GetTableClient(PersonShareListName);

    }
}
