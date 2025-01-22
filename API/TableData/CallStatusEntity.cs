using Azure.Data.Tables;
using Azure;

namespace API
{

    /// <summary>
    /// To track long-running calls like EventsChart
    /// </summary>
    public class CallStatusEntity : ITableEntity
    {
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
        public bool IsRunning { get; set; }
    }
}
