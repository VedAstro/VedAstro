using Azure.Data.Tables;
using Azure;
using System;

namespace VedAstro.Library
{
    /// <summary>
    /// Represents the data in 1 row of person share list table
    /// represents shared profiles a user has access to made by others
    /// </summary>
    public class PersonShareRow : ITableEntity
    {
        /// <summary>
        /// ID of person receiving share and and the person ID of the shared profile
        /// </summary>
        public PersonShareRow(string ownerId, string sharedPersonId)
        {
            PartitionKey = ownerId;
            RowKey = sharedPersonId;
        }

        /// <summary>
        /// Owner ID of the user receiving the shares
        /// </summary>
        public string PartitionKey { get; set; }
        
        /// <summary>
        /// Person ID of the profile shared to this owner
        /// </summary>
        public string RowKey { get; set; }

        /// <summary>
        /// Time of change
        /// </summary>
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }

    }

}
