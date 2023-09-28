using Azure.Data.Tables;
using Azure;

namespace API
{

	public class AnalyticsEntity : ITableEntity
	{
		public string PartitionKey { get; set; }
		public string RowKey { get; set; }
		public string URL { get; set; }
		public DateTimeOffset? Timestamp { get; set; }
		public ETag ETag { get; set; }
	}

}
