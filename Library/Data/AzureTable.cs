using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Data.Tables;

namespace VedAstro.Library
{

    /// <summary>
    /// static storage for connection access to Azure tables
    /// </summary>
    public static class AzureTable
    {
        public const string AccountName = "vedastrocentralstorage";
        private static readonly string StorageAccountKey = Secrets.VedAstroCentralStorageKey;

        public const string PersonListName = "PersonList";
        public const string PersonShareListName = "PersonShareList";
        public const string PersonListRecycleBinName = "PersonListRecycleBin";
        public const string LifeEventListName = "LifeEventList";
        public const string PersonListUri = $"https://vedastrocentralstorage.table.core.windows.net/{PersonListName}";
        public const string PersonShareListUri = $"https://vedastrocentralstorage.table.core.windows.net/{PersonShareListName}";
        public const string PersonListRecycleBinUri = $"https://vedastrocentralstorage.table.core.windows.net/{PersonListRecycleBinName}";
        public const string LifeEventListUri = $"https://vedastrocentralstorage.table.core.windows.net/{LifeEventListName}";
        public static readonly TableClient? PersonList = (new TableServiceClient(new Uri(PersonListUri), new TableSharedKeyCredential(AccountName, StorageAccountKey))).GetTableClient(PersonListName);
        public static readonly TableClient? PersonListRecycleBin = (new TableServiceClient(new Uri(PersonListRecycleBinUri), new TableSharedKeyCredential(AccountName, StorageAccountKey))).GetTableClient(PersonListRecycleBinName);
        public static readonly TableClient? LifeEventListTable = (new TableServiceClient(new Uri(LifeEventListUri), new TableSharedKeyCredential(AccountName, StorageAccountKey))).GetTableClient(LifeEventListName);
        public static readonly TableClient? PersonShareListTable = (new TableServiceClient(new Uri(PersonShareListUri), new TableSharedKeyCredential(AccountName, StorageAccountKey))).GetTableClient(PersonShareListName);

    }
}
