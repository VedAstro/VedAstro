using Azure.Data.Tables;
using Azure;
using System.Xml.Linq;
using VedAstro.Library;

namespace API
{
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
