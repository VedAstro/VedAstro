using Azure.Data.Tables;
using Azure;
using System;

namespace VedAstro.Library
{
    public class UserDataListEntity : ITableEntity
    {
        /// <summary>
        /// Id given by Google or Facebook (owner ID)
        /// </summary>
        public string PartitionKey { get; set; }

        /// <summary>
        /// email registered with FB or google
        /// </summary>
        public string RowKey { get; set; }

        /// <summary>
        /// registered name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// subscription API key (only generated if subscribed)
        /// </summary>
        public string APIKey { get; set; }
        
        /// <summary>
        /// ID used by Stripe for billing API
        /// </summary>
        public string StripeCustomerID { get; set; }

        /// <summary>
        /// Time of change
        /// </summary>
        public DateTimeOffset? Timestamp { get; set; }

        public ETag ETag { get; set; }

    }
}
