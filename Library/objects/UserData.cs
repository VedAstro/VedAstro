using System;
using System.Collections.Generic;
using System.Xml.Linq;

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
        public static UserData Guest = new UserData("101", "Guest", "guest@example.com", "", "", "");

        private string _name;


        public UserData(string id = "", string name = "", string email = "", string familyName = "", string locale = "", string picture = "")
        {
            Id = id;
            Name = name;
            Email = email;
            FamilyName = familyName;
            Locale = locale;
            Picture = picture;
            LocationList = new List<GeoLocation>();
        }

        public string Id { get; set; }

        public string Name
        {
            get => _name;
            set => _name = value == "Vignes" ? "James Brown" : value;
        }

        public List<GeoLocation> LocationList { get; set; }
        public string Email { get; set; }
        public string Locale { get; set; }
        public string FamilyName { get; set; }
        public string Picture { get; set; }

        /// <summary>
        /// Split the given name by space, and take the first name as first name
        /// </summary>
        public string FirstName => this.Name.Split(" ")[0];


        /// <summary>
        /// Converts XML to instance, root element is UserData
        /// </summary>
        public static UserData? FromXml(XElement userDataXml)
        {
            var userData = new UserData(
                id: userDataXml.Element("Id")?.Value,
                name: userDataXml.Element("Name")?.Value,
                email: userDataXml.Element("Email")?.Value,
                familyName: userDataXml.Element("FamilyName")?.Value,
                locale: userDataXml.Element("Locale")?.Value,
                picture: userDataXml.Element("Picture")?.Value);

            return userData;
        }

        /// <summary>
        /// Converts to XML, root element is UserData
        /// </summary>
        public XElement ToXml()
        {

            var userDataXml = new XElement("UserData");
            var nameXml = new XElement("Name", this.Name);
            var idXml = new XElement("Id", this.Id);
            var emailXml = new XElement("Email", this.Email);
            var familyNameXml = new XElement("FamilyName", this.FamilyName);
            var localeXml = new XElement("Locale", this.Locale);
            var pictureXml = new XElement("Picture", this.Picture);

            userDataXml.Add(idXml, nameXml, emailXml, familyNameXml, pictureXml, localeXml);

            return userDataXml;
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



    }
}