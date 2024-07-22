using System;
using System.Linq;
using Azure;
using Azure.Data.Tables;
using Newtonsoft.Json.Linq;

namespace VedAstro.Library
{

    /// <summary>
    /// Represents the data in 1 row of OpenAPIErrorBook
    /// </summary>
    public class PersonNameEmbeddingsEntity : ITableEntity
    {
        public string PartitionKey { get; set; }

        public string RowKey { get; set; } = ""; //set empty to stop error
        
        public string Embeddings { get; set; }
        public string PersonId { get; set; }


        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }


        public double[] GetEmbeddingsArray()
        {
            var docEmbedding = JArray.Parse(this.Embeddings);
            var newQueryEmbedsgg = docEmbedding.Select(jv => (double)jv).ToArray();

            return newQueryEmbedsgg;
        }

    }
}