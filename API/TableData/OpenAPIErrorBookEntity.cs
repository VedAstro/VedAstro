using Azure;
using Azure.Data.Tables;

namespace API
{

	/// <summary>
	/// Represents the data in 1 row of OpenAPIErrorBook
	/// </summary>
	public class OpenAPIErrorBookEntity : ITableEntity
    {
		/// <summary>
		/// IP Address
		/// </summary>
        public string PartitionKey { get; set; }
        
		/// <summary>
		/// UTC Call Time in Ticks
		/// </summary>
		public string RowKey { get; set; }

		/// <summary>
		/// Branch beta/stable version to ID code fault
		/// </summary>
        public string Branch { get; set; }

		/// <summary>
		/// URL that was called
		/// </summary>
        public string URL { get; set; }

		/// <summary>
		/// Compiled error message info
		/// </summary>
        public string Message { get; set; }

		public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
	}
}
