using System;
using System.Xml.Linq;

namespace Genso.Astrology.Library
{
    /// <summary>
    /// Simple data type to contain a person's details
    /// </summary>
    public struct Person : IToXml
    {
        private readonly string _name;
        private readonly Gender _gender;
        private readonly Time _birthTime;

        //CTOR
        public Person(string name, Time birthTime, Gender gender)
        {
            _name = name;
            _birthTime = birthTime;
            _gender = gender;
        }

        
        
        //PUBLIC PROPERTIES
        public Time GetBirthDateTime() => _birthTime;
        /// <summary>
        /// Get the place of birth
        /// Note: uses the location stored in birth "Time"
        /// </summary>
        public GeoLocation GetBirthLocation() => _birthTime.GetGeoLocation();
        public string GetName() => _name;
        public Gender GetGender() => _gender;



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
            var returnString = $"{this._name}";

            //return string to caller
            return returnString;
        }

        /// <summary>
        /// Name & Birth Time are used to generate Hash
        /// </summary>
        public override int GetHashCode()
        {
            //get hash of all the fields & combine them
            var hash1 = _name?.GetHashCode() ?? 0;
            var hash2 = _birthTime.GetHashCode();

            return hash1 + hash2;
        }



        //METHODS
        public XElement ToXml()
        {
            var person = new XElement("Person");
            var name = new XElement("Name", this.GetName());
            var gender = new XElement("Gender", this.GetGender().ToString());
            var birthTime = new XElement("BirthTime", this.GetBirthDateTime().ToXml());

            person.Add(name, gender, birthTime);

            return person;
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

            var parsedPerson = new Person(name, time, gender);

            return parsedPerson;
        }

        /// <summary>
        /// Gets STD birth year for person
        /// </summary>
        public int GetBirthYear() => this.GetBirthDateTime().GetStdDateTimeOffset().Year;
    }
}