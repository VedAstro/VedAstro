using Azure.Data.Tables;
using Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace VedAstro.Library
{
    /// <summary>
    /// Represents the data in 1 row of person list table
    /// </summary>
    public class MarriageTrainingDatasetEntity : ITableEntity
    {
        //NEEDED BY TABLE
        public string PartitionKey { get; set; }

        public string RowKey { get; set; } = "";

        public string Outcome { get; set; }
        public string MarriageDate { get; set; }
        public string ChildrenData { get; set; }
        public string DivorceData { get; set; }
        public string DivorceDate { get; set; }
        public string MalePersonId { get; set; }
        public string FemalePersonId { get; set; }
        public string Embeddings { get; set; }
        public double KutaScore { get; set; }
        public double MarriageDuration { get; set; }


        


        /// <summary>
        /// Time of change
        /// </summary>
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }

        public double[] GetEmbeddingsArray()
        {
            var ccc = Tools.ConvertStringToDoubleArray(Embeddings);
            return ccc;
        }


    }

}
