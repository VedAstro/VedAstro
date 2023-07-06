using Azure;
using Azure.Data.Tables;
using System.Xml.Linq;
using VedAstro.Library;

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

    /// <summary>
    /// represents 1 record in the log book
    /// </summary>
    public class LogBookEntity : ITableEntity
    {
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
        public string UserId { get; set; }
        public string Url { get; set; }
        public DateTimeOffset TimeStampClient { get; set; }
        public DateTimeOffset TimeStampServer { get; set; }
        public string Branch { get; set; }
        public string Source { get; set; }
        public string Data { get; set; }
        public string Error { get; set; }

        public static LogBookEntity FromXml(XElement newVisitorXml)
        {
            DateTimeOffset timeStampClient;
            DateTimeOffset timeStampServer;
            try
            {
                var timeStampClientRaw = newVisitorXml.Element("TimeStamp")?.Value ?? "";
                timeStampClient = DateTimeOffset.ParseExact(timeStampClientRaw, Time.DateTimeFormatSeconds, null);

                var timeStampServerRaw = newVisitorXml.Element("TimeStampServer")?.Value ?? "";
                timeStampServer = DateTimeOffset.ParseExact(timeStampServerRaw, Time.DateTimeFormatSeconds, null);

            }
            //if fail log about that then!
            catch (Exception e)
            {
                Console.WriteLine(e.Message); //todo
                timeStampServer = DateTimeOffset.UtcNow;
                timeStampClient = DateTimeOffset.UtcNow;
            }

            var x = new LogBookEntity()
            {
                //all records to 1 visitor is under 1 Partition (hence use as partition key) 
                PartitionKey = newVisitorXml.Element("VisitorId")?.Value ?? "NO VISITOR BADGE",
                RowKey = Tools.GenerateId(), //uniquely represents each record
                UserId = newVisitorXml.Element("UserId")?.Value ?? "",
                TimeStampClient = timeStampClient,
                TimeStampServer = timeStampServer,
                Url = newVisitorXml.Element("Url")?.Value ?? "",
                Branch = newVisitorXml.Element("Branch")?.Value ?? "",
                Source = newVisitorXml.Element("Source")?.Value ?? "",
                Data = newVisitorXml.Element("Data")?.Value ?? "",
                Error = newVisitorXml.Element("Error")?.Value ?? "",

            };

            return x;

        }

    }

}
