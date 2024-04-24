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
        public string Body { get; set; }
        public string Header1 { get; set; }
        public string Header2 { get; set; }
        public string Header3 { get; set; }
        public string Header4 { get; set; }
        public string Header5 { get; set; }
        public string Header6 { get; set; }
        public string Header7 { get; set; }
        public string Header8 { get; set; }
        public string Header9 { get; set; }
        public string Header10 { get; set; }
        public string Header11 { get; set; }
        public string Header12 { get; set; }
        public string Header13 { get; set; }
        public string Header14 { get; set; }
        public string Header15 { get; set; }


    }
}
