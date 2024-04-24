using Azure.Data.Tables;
using Azure;
using System;

namespace VedAstro.Library
{
    /// <summary>
    /// Represents the data in 1 row of person list table
    /// </summary>
    public class PersonRow : ITableEntity
    {
        //NEEDED BY TABLE
        public string PartitionKey { get; set; }
        
        /// <summary>
        /// Time of creation
        /// </summary>
        public string RowKey { get; set; }

        /// <summary>
        /// Time of change
        /// </summary>
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }

        //CUSTOM DATA
        public string Name { get; set; }
        public string BirthTime { get; set; }
        public string Gender { get; set; }
        public string Notes { get; set; }

        /// <summary>
        /// Full clone for easy modification
        /// </summary>
        public PersonRow Clone()
        {
            return new PersonRow()
            {
                PartitionKey = PartitionKey,
                RowKey = RowKey,
                Timestamp = Timestamp,
                ETag = ETag,
                Name = Name,
                BirthTime = BirthTime,
                Gender = Gender,
                Notes = Notes
            };
        }
    }

}
