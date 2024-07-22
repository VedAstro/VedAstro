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
    public class MarriageInfoDatasetEntity : ITableEntity
    {
        //NEEDED BY TABLE
        public string PartitionKey { get; set; }

        /// <summary>
        /// Time of creation
        /// </summary>
        public string RowKey { get; set; } = "";

        public string Info { get; set; }


        public void SetMarriages(List<JToken> marriages)
        {
            var jsonParsed = this?.InfoJson() ?? new JObject();
            jsonParsed["marriages"] = new JArray(marriages);
            this.Info = jsonParsed.ToString();
        }


        public List<JToken> GetMarriagesWhereRoddenAA()
        {
            var allMarriages = GetMarriages();

            var filtered = allMarriages.Where(x =>
            {
                var personId = x["PersonId"]?.Value<string>();
                //if NOT null add to list, meaning person was found in valid dataset
                return !string.IsNullOrEmpty(personId); 
            });

            return filtered.ToList();
        }
        public List<JToken> GetMarriages()
        {
            //1 : convert to json
            var jsonParsed = this?.InfoJson();
            var returnList = new List<JToken>();
            if (jsonParsed != null)
            {
                var marriagesArray = jsonParsed["marriages"];
                foreach (var marriagesJson in marriagesArray)
                {
                    returnList.Add(marriagesJson);
                }
            }

            return returnList;
        }

        public JObject? InfoJson()
        {
            try
            {
                return JObject.Parse(this.Info);
            }
            catch (Exception e)
            {
                return null;
            }

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


        /// <summary>
        /// Time of change
        /// </summary>
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
    }

}
