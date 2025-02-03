using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Azure.Data.Tables;

namespace VedAstro.Library
{
    /// <summary>
    /// Simple class to hold user data
    /// </summary>
    public class UserData
    {        
        /// <summary>
        /// Guest users default account
        /// Empty instance of User with id 101
        /// </summary>
        public static UserData Guest = new UserData("101", "Guest", "guest@example.com");


        public UserData(string id = "", string name = "", string email = "", string apiKey = "", string stripeCustomerID = "")
        {
            Id = id;
            Name = name;
            Email = email;
            APIKey = apiKey;
            StripeCustomerID = stripeCustomerID;
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string APIKey { get; set; }

        public string StripeCustomerID { get; set; }

        /// <summary>
        /// Split the given name by space, and take the first name as first name
        /// </summary>
        public string FirstName => this.Name.Split(" ")[0];


        public JToken ToJson()
        {
            var temp = new JObject();
            temp["Name"] = this.Name;
            temp["Id"] = this.Id;
            temp["Email"] = this.Email;
            temp["APIKey"] = this.APIKey;
            temp["StripeCustomerID"] = this.StripeCustomerID;
            return temp;
        }


        //OVERRIDES METHODS
        public override bool Equals(object value)
        {

            if (value.GetType() == typeof(UserData))
            {
                //cast to type
                var parsedValue = (UserData)value;

                //check equality
                bool returnValue = (this.GetHashCode() == parsedValue.GetHashCode());

                return returnValue;
            }
            else
            {
                //return false if value is null
                return false;
            }
        }

        public override string ToString()
        {
            //prepare string
            var returnString = $"{this.Name}|{this.Email}";

            //return string to caller
            return returnString;
        }

        /// <summary>
        /// ID & Email are used to generate Hash
        /// since name could change not used
        /// </summary>
        public override int GetHashCode()
        {
            //get hash of all the fields & combine them
            var hash1 = Tools.GetStringHashCode(this.Id);
            var hash2 = Tools.GetStringHashCode(this.Email);

            //take out negative before returning
            return Math.Abs(hash1 + hash2);
        }

        /// <summary>
        /// This makes email as primary key
        /// </summary>
        public UserDataListEntity ToAzureRow()
        {
            //make the cache row to be added
            var newRow = new UserDataListEntity()
            {
                PartitionKey = this.Id,
                RowKey = this.Email,
                Name = this.Name,
                APIKey = this.APIKey,
                StripeCustomerID = this.StripeCustomerID,
            };

            return newRow;
        }

    }
}