using Azure;
using Azure.Data.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VedAstro.Library
{
    public class UserDataListEntity : ITableEntity
    {
        /// <summary>
        /// Email (since more unique)
        /// </summary>
        public string PartitionKey { get; set; }

        /// <summary>
        /// leave null not needed
        /// </summary>
        public string RowKey { get; set; }

        /// <summary>
        /// Time of change
        /// </summary>
        public DateTimeOffset? Timestamp { get; set; }

        public ETag ETag { get; set; }

        //CUSTOM DATA
        public string Name { get; set; }

        /// <summary>
        /// Id given by Google or Facebook
        /// </summary>
        public string Id { get; set; }

    }
}
