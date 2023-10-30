using Azure.Data.Tables;
using Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VedAstro.Library
{
    public class APIAbuseRow : ITableEntity
    {
        /// <summary>
        /// caller IP address
        /// </summary>
        public string PartitionKey { get; set; }

        /// <summary>
        /// Random ID to count number of exceeds
        /// </summary>
        public string RowKey { get; set; } = Tools.GenerateId();

        /// <summary>
        /// Time of exceed
        /// </summary>
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }

    }

}
