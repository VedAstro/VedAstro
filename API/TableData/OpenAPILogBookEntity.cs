using Azure;
using Azure.Data.Tables;

namespace API
{

	/// <summary>
	/// Represents the data in 1 row of OpenAPILogBook
	/// </summary>
	public class OpenAPILogBookEntity : ITableEntity
    {
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public string URL { get; set; }
		public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
	}
}
