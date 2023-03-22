using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace VedAstro.Library
{
    /// <summary>
    /// Simple data type to contain a person's details
    /// NOTE: try to maintain as struct, for unmutable data
    /// </summary>
    public struct Person : IToXml
    {
        private static string[] DefaultUserId = new[] { "101" };

        /// <summary>
        /// Empty instance for null use cases
        /// All internal properties are initialized with empty values
        /// so use that to detect
        /// </summary>
        public static Person Empty = new Person("0", "Empty", Time.Now(GeoLocation.Empty), Gender.Empty, DefaultUserId, "Empty", new List<LifeEvent>());

        private string _notes;

        //DATA FIELDS
        /// <summary>
        /// Represents permanent identity to this record, generated only once during creation
        /// made of random alphanumeric string 
        /// </summary>
        public string Id { get; set; }

        public string Name { get; set; }
        /// <summary>
        /// User ID is used by website. Multiple supported, Shows owner of person's profile
        /// </summary>
        public string[] UserId { get; set; }

        /// <summary>
        /// Comma sperated string of Person's all user ID
        /// </summary>
        public string UserIdString => string.Join(",", UserId);


        /// <summary>
        /// Misc. notes about the person
        /// </summary>
        public string Notes
        {
            get => HttpUtility.HtmlDecode(_notes);
            set => _notes = HttpUtility.HtmlEncode(value);
        }

        public Gender Gender { get; set; }
        public Time BirthTime { get; set; }

        /// <summary>
        /// List of events that mark important moments in a persons life
        /// This is used later by calculators like Dasa to show against astrological predictions
        /// </summary>
        public List<LifeEvent> LifeEventList { get; set; } = new List<LifeEvent>(); //default empty list to stop null errors



        //CTOR
        public Person(string id, string name, Time birthTime, Gender gender, string[] userId, string notes = "", List<LifeEvent> lifeEventList = null)
        {
            Id = id;
            Name = name;
            BirthTime = birthTime;
            Gender = gender;
            UserId = userId.Any() ? userId : DefaultUserId;
            Notes = notes;
            LifeEventList = lifeEventList ?? new List<LifeEvent>(); //empty list if not specified
        }


        /// <summary>
        /// Gets STD birth year for person
        /// </summary>
        public int BirthYear => this.BirthTime.GetStdDateTimeOffset().Year;

        /// <summary>
        /// Gets STD birth time zone for person
        /// exp : +08:00
        /// </summary>
        public string BirthTimeZone => this.BirthTime.GetStdDateTimeOffset().ToString("zzz");

        /// <summary>
        /// Gets STD birth hour minute for person (24H format)
        /// exp: 15:30
        /// </summary>
        public string BirthHourMinute => this.BirthTime.GetStdDateTimeOffset().ToString("HH:mm");//note "HH" is 24H format vs "hh" is 12H format 

        /// <summary>
        /// Gets STD birth Date Month Year for person
        /// exp: 31/12/1999
        /// </summary>
        public string BirthDateMonthYear => BirthTime.GetStdDateTimeOffset().ToString("dd/MM/yyyy");//note "MM" is month, not "mm"


        /// <summary>
        /// Used by tabulator JS, when person is converted to json
        /// </summary>
        public string GenderString => Gender.ToString();

        /// <summary>
        /// Used by tabulator JS, when person is converted to json
        /// </summary>
        public int Hash => this.GetHashCode();

        /// <summary>
        /// Returns STD birth time in string HH:mm dd/MM/yyyy zzz
        /// Used by tabulator JS, when person is converted to json
        /// </summary>
        public string BirthTimeString => this.BirthTime.GetStdDateTimeOffsetText();

        /// <summary>
        /// Gets now time at birth location of person (STD time)
        /// </summary>
        public DateTimeOffset StdTimeNowAtBirthLocation => DateTimeOffset.Now.ToOffset(this.BirthTime.GetStdDateTimeOffset().Offset);





        //PUBLIC PROPERTIES

        /// <summary>
        /// Get the place of birth
        /// Note: uses the location stored in birth "Time"
        /// </summary>
        public GeoLocation GetBirthLocation() => BirthTime.GetGeoLocation();




        /// <summary>
        /// Gets this person's age at the inputed time (using year from STD time)
        /// </summary>
        public int GetAge(Time time) => time.GetStdDateTimeOffset().Year - this.BirthYear;

        /// <summary>
        /// Gets this person's age at the inputed std year (using year from STD time)
        /// </summary>
        public int GetAge(int year) => year - this.BirthYear;



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
            var hash1 = Tools.GetStringHashCode(this.Name);
            var hash2 = BirthTime.GetHashCode();
            var hash3 = Tools.GetStringHashCode(this.UserIdString);

            //take out negative before returning
            return Math.Abs(hash1 + hash2 + hash3);
        }





        //METHODS
        public XElement ToXml()
        {
            var person = new XElement("Person");
            var name = new XElement("Name", this.Name);
            var id = new XElement("PersonId", this.Id);
            var notes = new XElement("Notes", this.Notes);
            var gender = new XElement("Gender", this.Gender.ToString());
            var birthTime = new XElement("BirthTime", this.BirthTime.ToXml());
            var userId = new XElement("UserId", this.UserIdString);
            var lifeEventListXml = getLifeEventListXml(LifeEventList);

            person.Add(id, name, gender, birthTime, userId, lifeEventListXml, notes);

            return person;

            //----------LOCAL FUNCTIONS ---------------
            XElement getLifeEventListXml(List<LifeEvent> lifeList)
            {
                //create the empty list holder xml
                var returnXml = new XElement("LifeEventList");

                //if null just return empty list
                if (lifeList is null) { return returnXml; }

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
            var id = personXml.Element("PersonId")?.Value ?? Tools.GenerateId(); //if no id generate new
            var notes = personXml.Element("Notes")?.Value;
            var time = Time.FromXml(personXml.Element("BirthTime")?.Element("Time"));
            var gender = Enum.Parse<Gender>(personXml.Element("Gender")?.Value);
            var userIdRaw = personXml.Element("UserId")?.Value ?? "";
            //clean, remove white space & new line if any
            userIdRaw = userIdRaw.Replace("\n", "");
            userIdRaw = userIdRaw.Replace(" ", "");

            var userId = userIdRaw.Split(',');//split by comma
            var lifeEventList = getLifeEventListFromXml();

            var parsedPerson = new Person(id, name, time, gender, userId, notes, lifeEventList);

            return parsedPerson;

            //-------------------LOCAL FUNCTION--------
            List<LifeEvent> getLifeEventListFromXml()
            {
                //Try to get data from xml
                //there is a possibility that xml doesn't exist
                try
                {
                    var lifeEventListXml = personXml.Element("LifeEventList")?.Elements();
                    var returnList = new List<LifeEvent>();

                    //is null when no life events exist for user, so to avoid exception this is
                    if (lifeEventListXml != null)
                    {
                        foreach (var lifeEventXml in lifeEventListXml)
                        {
                            returnList.Add(LifeEvent.FromXml(lifeEventXml));
                        }
                    }

                    return returnList;

                }
                //if fail, probably xml doesn't exist so just send empty list
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    Console.WriteLine("No Valid Life Events Found! Empty list used!");//debug
                    return new List<LifeEvent>();
                }
            }
        }

        /// <summary>
        /// Parses a list of person's
        /// </summary>
        /// <param name="personXmlList"></param>
        /// <returns></returns>
        public static List<Person> FromXml(IEnumerable<XElement> personXmlList)
        {
            return personXmlList.Select(personXml => Person.FromXml(personXml)).ToList();
        }

        /// <summary>
        /// Replaces life event list and returns new Person with modified val
        /// note:
        /// - done so because it's a struct
        /// - maintains all other person props
        /// </summary>
        public Person SetNewLifeEvents(List<LifeEvent> updatedLifeEventList) => new Person(Id, Name, BirthTime, Gender, UserId, Notes, updatedLifeEventList);

        /// <summary>
        /// Returns a new instance person with modified birth time
        /// everything else including ID stays the same
        /// </summary>
        public Person ChangeBirthTime(Time newBirthTime)
        {
            //make a copy of person details except birth time
            var newPerson = new Person(this.Id, Name, newBirthTime, Gender, UserId, Notes, LifeEventList);
            return newPerson;
        }
    }
}