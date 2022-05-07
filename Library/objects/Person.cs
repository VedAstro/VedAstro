using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;

namespace Genso.Astrology.Library
{
    /// <summary>
    /// Simple data type to contain a person's details
    /// NOTE: switch to class if needed
    /// </summary>
    public struct Person : IToXml
    {
        //DATA FIELDS
        public string Name { get; set; }
        /// <summary>
        /// User ID is used by website. Shows owner of person's profile
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// Misc. notes about the person
        /// </summary>
        public string Notes { get; set; }
        public Gender Gender { get; set; }
        public Time BirthTime { get; set; }
        /// <summary>
        /// List of events that mark important moments in a persons life
        /// This is used later by calculators like Dasa to show against astrological predictions
        /// </summary>
        public List<LifeEvent> LifeEventList { get; set; }



        //CTOR
        public Person(string name, Time birthTime, Gender gender, string userId = "101", string notes = "", List<LifeEvent> lifeEventList = null)
        {
            Name = name;
            BirthTime = birthTime;
            Gender = gender;
            UserId = userId;
            Notes = notes;
            LifeEventList = lifeEventList;
        }



        //PUBLIC PROPERTIES

        /// <summary>
        /// Todo deprecated use BirthTime
        /// </summary>
        public Time GetBirthDateTime() => BirthTime;

        /// <summary>
        /// Get the place of birth
        /// Note: uses the location stored in birth "Time"
        /// </summary>
        public GeoLocation GetBirthLocation() => BirthTime.GetGeoLocation();

        /// <summary>
        /// Todo deprecated use Name
        /// </summary>
        public string GetName() => Name;

        /// <summary>
        /// Todo deprecated use Gender
        /// </summary>
        public Gender GetGender() => Gender;

        /// <summary>
        /// Gets STD birth year for person
        /// </summary>
        public int BirthYear => this.GetBirthDateTime().GetStdDateTimeOffset().Year;

        /// <summary>
        /// Gets this person's age at the inputed time (using year from STD time)
        /// </summary>
        public int GetAge(Time time) => time.GetStdDateTimeOffset().Year - this.BirthYear;

        /// <summary>
        /// Used by tabulator JS, when person is converted to json
        /// </summary>
        public string GenderString => Gender.ToString();

        /// <summary>
        /// Used by tabulator JS, when person is converted to json
        /// </summary>
        public int Hash => this.GetHashCode();

        /// <summary>
        /// Used by tabulator JS, when person is converted to json
        /// </summary>
        public string BirthTimeString => this.GetBirthDateTime().GetStdDateTimeOffsetText();



        //OVERRIDES METHODS
        public override bool Equals(object value)
        {

            if (value.GetType() == typeof(Person))
            {
                //cast to type
                var parsedValue = (Person)value;

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
            var returnString = $"{this.Name}";

            //return string to caller
            return returnString;
        }

        /// <summary>
        /// Name & Birth Time are used to generate Hash
        /// </summary>
        public override int GetHashCode()
        {
            //get hash of all the fields & combine them
            var hash1 = Tools.GetHashCode(this.Name);
            var hash2 = BirthTime.GetHashCode();

            //take out negative before returning
            return Math.Abs(hash1 + hash2);
        }





        //METHODS
        public XElement ToXml()
        {
            var person = new XElement("Person");
            var name = new XElement("Name", this.GetName());
            var gender = new XElement("Gender", this.GetGender().ToString());
            var birthTime = new XElement("BirthTime", this.GetBirthDateTime().ToXml());
            var userId = new XElement("UserId", this.UserId);
            var lifeEventListXml = getLifeEventListXml(LifeEventList);

            person.Add(name, gender, birthTime, userId, lifeEventListXml);

            return person;

            //----------LOCAL FUNCTIONS ---------------
            XElement getLifeEventListXml(List<LifeEvent> lifeList)
            {
                //create the empty list holder xml
                var returnXml = new XElement("LifeEventList");

                //convert to xml and add to holder xml
                foreach (var lifeEvent in lifeList)
                {
                    var lifeXml = lifeEvent.ToXml();
                    returnXml.Add(lifeXml);
                }

                return returnXml;
            }
        }

        /// <summary>
        /// The root element is expected to be Person
        /// Note: Special method done to implement IToXml
        /// </summary>
        public dynamic FromXml<T>(XElement xml) where T : IToXml => FromXml(xml);

        /// <summary>
        /// The root element is expected to be Person
        /// </summary>
        public static Person FromXml(XElement personXml)
        {
            var name = personXml.Element("Name")?.Value;
            var time = Time.FromXml(personXml.Element("BirthTime")?.Element("Time"));
            var gender = Enum.Parse<Gender>(personXml.Element("Gender")?.Value);
            var userId = personXml.Element("UserId")?.Value;
            var lifeEventList = getLifeEventListFromXml();

            var parsedPerson = new Person(name, time, gender, userId, "",lifeEventList);

            return parsedPerson;

            //-------------------LOCAL FUNCTION--------
            List<LifeEvent> getLifeEventListFromXml()
            {
                var lifeEventListXml = personXml.Element("LifeEventList")?.Elements();
                var returnList = new List<LifeEvent>();
                foreach (var lifeEventXml in lifeEventListXml)
                {
                    returnList.Add(LifeEvent.FromXml(lifeEventXml));
                }

                return returnList;
            }
        }

    }
}