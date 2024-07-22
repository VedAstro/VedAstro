using Azure.Data.Tables;
using Azure;
using System;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace VedAstro.Library
{
    /// <summary>
    /// Represents the data in 1 row of person list table
    /// </summary>
    public class BodyInfoDatasetEntity : ITableEntity
    {
        //NEEDED BY TABLE
        public string PartitionKey { get; set; }

        public string RowKey { get; set; } = "";
        public string Info { get; set; }



        /// <summary>
        /// Time of change
        /// </summary>
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }

        public JObject InfoJson()
        {
            return JObject.Parse(this.Info);
        }

        public bool IsJson()
        {
            try
            {
                var obj = JToken.Parse(this.Info);
                return true; // JSON is valid
            }
            catch (JsonReaderException ex)
            {
                return false; // JSON is invalid
            }
        }
    }

}
