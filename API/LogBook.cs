using Azure.Data.Tables;

namespace API
{
    /// <summary>
    /// Specialized class to log data to table in AZ store
    /// </summary>
    public static class LogBook
    {
        private static readonly TableServiceClient tableServiceClient;
        private static string tableName = "LogBook";
        private static readonly TableClient tableClient;

        static LogBook()
        {
            //todo cleanup
            var storageUri = $"https://vedastroapistorage.table.core.windows.net/{tableName}";
            string accountName = "vedastroapistorage";
            string storageAccountKey = "kquBbAE8QKhe/Iyny4F0we3Yx6Y/zJv7yZgrERuFq7nRoEa53o6rb50W88II+PsSEP7RzYYNQizDHPOlt0kwxw==";

            //save reference for late use
            tableServiceClient = new TableServiceClient(new Uri(storageUri), new TableSharedKeyCredential(accountName, storageAccountKey));
            tableClient = tableServiceClient.GetTableClient(tableName);

        }

        /// <summary>
        /// Marks the call as running
        /// </summary>
        public static void Add(LogBookEntity newLogRecord)
        {
            

            //creates record if no exist, update if already there
            tableClient.UpsertEntity(newLogRecord);

        }


    }

}
