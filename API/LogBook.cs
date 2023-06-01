using Azure;
using Azure.Data.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static API.CallTracker;

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
        public string VisitorId { get; set; }
        public string Url { get; set; }
        public string TimeStampClient { get; set; }
        public string TimeStampServer { get; set; }
        public string Branch { get; set; }
        public string Source { get; set; }
        public string Data { get; set; }
        public string Error { get; set; }

        public static LogBookEntity FromXml(XElement newVisitorXml)
        {
            var x = new LogBookEntity()
            {
                VisitorId = newVisitorXml.Element("VisitorId")?.Value ?? "",
                UserId = newVisitorXml.Element("UserId")?.Value ?? "",
                TimeStampClient = newVisitorXml.Element("TimeStamp")?.Value ?? "",
                TimeStampServer = newVisitorXml.Element("TimeStampServer")?.Value ?? "",
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
