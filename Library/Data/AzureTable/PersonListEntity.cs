using Azure.Data.Tables;
using Azure;
using System;
using Newtonsoft.Json.Linq;

namespace VedAstro.Library
{
    /// <summary>
    /// Represents the data in 1 row of person list table
    /// </summary>
    public class PersonListEntity : ITableEntity
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
        public PersonListEntity Clone()
        {
            return new PersonListEntity()
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


        /// <summary>
        /// parse and extract birth year on the fly
        /// </summary>
        /// <returns></returns>
        public string GetBirthYear()
        {
            var xxx = JObject.Parse(this.BirthTime);
            var stdTimeString = xxx["StdTime"].Value<string>();
            //break & take out center part dd/mm/year
            var ccc = stdTimeString.Split(" ");
            var ddMMYYYY = ccc[1];

            return ddMMYYYY;
        }

        public Time ToBirthTime()
        {
            var birthTime = Time.FromJson(JObject.Parse(this.BirthTime));
            return birthTime;
        }

        public bool IsMale()
        {
            return this.Gender.ToLower() == "male";
        }
    }

}
