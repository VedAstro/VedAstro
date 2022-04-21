using System;
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
        public Gender Gender { get; set; }
        public Time BirthTime { get; set; }
        


        //CTOR
        public Person(string name, Time birthTime, Gender gender)
        {
            Name = name;
            BirthTime = birthTime;
            Gender = gender;
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

        public string GenderString => Gender.ToString();

        /// <summary>
        /// Gets STD birth time as string
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
            var hash1 = Name?.GetHashCode() ?? 0;
            var hash2 = BirthTime.GetHashCode();

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

    }
}