using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Azure.Data.Tables;

namespace AzureTableStorageDownloader
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string connectionString = "your_connection_string_here"; // Replace with your actual connection string
            string folderPath = "saved-tables"; // Folder to save CSV files

            var storageAccount = CloudStorageAccount.Parse(connectionString);
            var tableClient = storageAccount.CreateCloudTableClient();

            var tableList = tableClient.ListTables();
            foreach (var table in tableList)
            {
                var entities = await GetEntitiesAsync(table);
                await SaveEntitiesAsCsvAsync(table.Name, entities, folderPath);
            }

            Console.WriteLine("Data downloaded and saved as CSV files.");
        }

        static async Task<IEnumerable<MyEntity>> GetEntitiesAsync(CloudTable table)
        {
            var query = new TableQuery<MyEntity>(); // Replace MyEntity with your entity type
            var entities = await table.ExecuteQueryAsync(query);
            return entities;
        }

        static async Task SaveEntitiesAsCsvAsync(string tableName, IEnumerable<MyEntity> entities, string folderPath)
        {
            Directory.CreateDirectory(folderPath);
            string filePath = Path.Combine(folderPath, $"{tableName}.csv");

            using (var writer = new StreamWriter(filePath))
            {
                foreach (var entity in entities)
                {
                    await writer.WriteLineAsync($"{entity.PartitionKey},{entity.RowKey},{entity.SomeProperty}");
                    // Adjust the properties you want to include in the CSV
                }
            }
        }
    }

    // Define your entity class
    class MyEntity : ITableEntity
    {
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public string ETag { get; set; }
        public string SomeProperty { get; set; }
        // Add other properties as needed
    }
}
