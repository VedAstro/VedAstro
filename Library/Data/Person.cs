using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;

namespace VedAstro.Library
{
    /// <summary>
    /// Simple data type to contain a person's details
    /// NOTE: try to maintain as struct, for unmutable data
    /// </summary>
    public struct Person : IToXml, IFromUrl, IToJson
    {

        /// <summary>
        /// The number of pieces the URL version of this instance needs to be cut for processing
        /// used to segregate out the combined URL with other data
        /// EXP -> /Person/JesusHChrist0000/ == 2 PIECES
        /// </summary>
        public static int OpenAPILength = 2;


        private static string DefaultUserId = "101";

        /// <summary>
        /// Empty instance for null use cases
        /// All internal properties are initialized with empty values
        /// so use that to detect
        /// </summary>
        public static Person Empty = new Person(DefaultUserId, "0", "Empty", Time.Empty, Gender.Empty, "Empty");

        private string _notes;

        //DATA FIELDS
        /// <summary>
        /// Represents permanent identity to this record, generated only once during creation
        /// made of human readable ID made of person name and birth year
        /// should be camel case, would look nicer
        /// </summary>
        public string Id { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// User ID is used by website. Multiple supported, Shows owner of person's profile
        /// </summary>
        public string OwnerId { get; set; }

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
        public Person(string ownerId, string id, string name, Time birthTime, Gender gender, string notes = "",
            List<LifeEvent> lifeEventList = null)
        {
            Id = id;
            Name = name;
            BirthTime = birthTime;
            Gender = gender;
            OwnerId = ownerId;
            Notes = notes;
            LifeEventList = lifeEventList ?? new List<LifeEvent>(); //no nulls in list
        }

        /// <summary>
        /// shortcut method to make person when database not needed
        /// made for easy 3rd party lib code 
        /// </summary>
        public Person(string name, Time birthTime, Gender gender)
        {
            Id = Tools.GenerateId();
            Name = name;
            BirthTime = birthTime;
            Gender = gender;
            OwnerId = DefaultUserId;
            Notes = "";
            LifeEventList = new List<LifeEvent>(); //empty list if not specified
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
        public string BirthHourMinute =>
            this.BirthTime.GetStdDateTimeOffset().ToString("HH:mm"); //note "HH" is 24H format vs "hh" is 12H format 

        /// <summary>
        /// Gets STD birth Date Month Year for person
        /// exp: 31/12/1999
        /// </summary>
        public string BirthDateMonthYear =>
            BirthTime.GetStdDateTimeOffset().ToString("dd/MM/yyyy"); //note "MM" is month, not "mm"

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

        /// <summary>
        /// Gets now time at birth location of person
        /// </summary>
        public Time TimeNowAtBirthLocation
        {
            get
            {
                var temp = new Time(DateTimeOffset.UtcNow.ToOffset(this.BirthTime.GetStdDateTimeOffset().Offset), this.GetBirthLocation());
                return temp;
            }
        }

        /// <summary>
        /// image name is ID with .jpg at back
        /// </summary>
        public string ImageName => $"{this.Id}.jpg";

        /// <summary>
        /// Format name with birth year for easy identification
        /// used for showing to  in website Steve Jobs - 1995
        /// </summary>
        public string DisplayName => $"{Name} - {BirthYear}";

        /// <summary>
        /// Name with no space, used for file names, SteveJobs
        /// </summary>
        public string NameWithNoSpace => $"{Name.Replace(" ", "")}";


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

        /// <summary>
        /// Gets age at now time at current time's locations
        /// </summary>
        /// <returns></returns>
        public int GetAge() => GetAge(Time.NowSystem(this.GetBirthLocation()));



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
            var hash3 = Tools.GetStringHashCode(this.OwnerId);

            //take out negative before returning
            return Math.Abs(hash1 + hash2 + hash3);
        }


        //METHODS

        /// <summary>
        /// Given an XML list of persons record from storage
        /// converts to json
        /// TODO CHANGE TO SELECT BASED METHOD
        /// </summary>
        /// <param name="personXmlList"></param>
        /// <returns></returns>
        public static JArray XmlListToJsonList(List<XElement> personXmlList)
        {
            //parse to person first (don't care about speed we are on durable API yeah!)
            var personList = Person.FromXml(personXmlList);
            JArray returnArray = new JArray();
            foreach (var person in personList)
            {

                //compile into an JSON array
                returnArray.Add(person.ToJson());
            }


            return returnArray;
        }

        /// <summary>
        /// Converts life event list to JSON on the fly
        /// </summary>
        public JArray LifeEventListJson
        {
            get
            {
                JArray array = new JArray();
                foreach (var lifeEvent in LifeEventList)
                {
                    array.Add(lifeEvent.ToJson());
                }
                return array;
            }
        }

        /// <summary>
        /// Returns STD birth time without hour and minute (for modification by caller)
        /// EXP: 23/12/2000 +02:00
        /// </summary>
        public string BirthDateMonthYearOffset => $"{this.BirthDateMonthYear} {this.BirthTimeZone}";

        public XElement ToXml()
        {
            var person = new XElement("Person");
            var name = new XElement("Name", this.Name);
            var id = new XElement("PersonId", this.Id);
            var notes = new XElement("Notes", this.Notes);
            var gender = new XElement("Gender", this.Gender.ToString());
            var birthTime = new XElement("BirthTime", this.BirthTime.ToXml());
            var ownerId = new XElement("OwnerId", this.OwnerId);
            var lifeEventListXml = getLifeEventListXml(LifeEventList);

            person.Add(id, name, gender, birthTime, ownerId, lifeEventListXml, notes);

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
        public static Person FromXml(XElement? personXml)
        {
            //if null return empty, end here
            if (personXml == null) { return Person.Empty; }

            //if no id place 0 find the error
            //note:
            //  - do not just generate a new ID, it should only be generated in one place for easy maintenance
            //  - some times can use to clean data, but once done change back
            var id = personXml.Element("PersonId")?.Value ?? "0";
            var name = personXml.Element("Name")?.Value;
            var notes = personXml.Element("Notes")?.Value;
            var time = Time.FromXml(personXml.Element("BirthTime")?.Element("Time"));
            var gender = Enum.Parse<Gender>(personXml.Element("Gender")?.Value);
            var ownerId = personXml.Element("OwnerId")?.Value;

            var parsedPerson = new Person(ownerId, id, name, time, gender, notes);

            return parsedPerson;

            //-------------------LOCAL FUNCTION--------
        }

        /// <summary>
        /// Parses a list of person's
        /// </summary>
        /// <param name="personXmlList"></param>
        /// <returns></returns>
        public static List<Person> FromXml(IEnumerable<XElement> personXmlList) => personXmlList.Select(personXml => Person.FromXml(personXml)).ToList();

        /// <summary>
        /// Given Time instance in URL form will convert to instance
        /// /Person/JesusHChrist0000/
        /// </summary>
        public static async Task<dynamic> FromUrl(string url)
        {
            // INPUT -> "/Person/JesusHChrist0000/"
            string[] parts = url.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

            var person = Tools.GetPersonById(parts[1]);

            return person;
        }

        /// <summary>
        /// Returns a new instance person with modified birth time
        /// everything else including ID stays the same
        /// </summary>
        public Person ChangeBirthTime(Time newBirthTime)
        {
            //make a copy of person details except birth time
            var newPerson = new Person(OwnerId, this.Id, Name, newBirthTime, Gender, Notes, LifeEventList);
            return newPerson;
        }

        /// <summary>
        /// This makes owner ID as primary key and person id as 
        /// </summary>
        /// <returns></returns>
        public PersonListEntity ToAzureRow()
        {
            //make the cache row to be added
            var newRow = new PersonListEntity()
            {
                //can have many IP as partition key
                PartitionKey = this.OwnerId,
                RowKey = this.Id,
                Name = this.Name,
                BirthTime = this.BirthTime.ToJson().ToString(),
                Gender = this.Gender.ToString(),
                Notes = Notes,
            };

            return newRow;
        }

        /// <summary>
        /// Brings back Owner ID from Primary key & 
        /// </summary>
        public static Person FromAzureRow(PersonListEntity rowData)
        {
            //parse the person only
            var birthTime = Time.FromJson(JToken.Parse(rowData.BirthTime));
            var rowDataGender = Enum.Parse<Gender>(rowData.Gender);
            var personId = rowData.RowKey;
            var newPerson = new Person(rowData.PartitionKey, personId, rowData.Name, birthTime, rowDataGender);

            //get person life event list (partition key = person id)
            var lifeEvents = AzureTable.LifeEventList.Query<LifeEventRow>(call => call.PartitionKey == personId);

            //convert to list
            var personJsonList = lifeEvents.Select(call => LifeEvent.FromAzureRow(call)).ToList();

            //add to person data
            newPerson.LifeEventList = personJsonList;

            return newPerson;
        }

        #region JSON SUPPORT


        JObject IToJson.ToJson() => (JObject)this.ToJson();

        public JToken ToJson()
        {
            var temp = new JObject();
            temp["PersonId"] = this.Id;
            temp["Name"] = this.Name;
            temp["Notes"] = this.Notes;
            temp["BirthTime"] = this.BirthTime.ToJson();
            temp["Gender"] = this.GenderString;
            temp["OwnerId"] = this.OwnerId;
            temp["LifeEventList"] = this.LifeEventListJson; //json array

            return temp;
        }

        /// <summary>
        /// Given a json list of person will convert to instance
        /// used for transferring between server & client
        /// </summary>
        public static List<Person> FromJsonList(JToken personList)
        {
            //if null empty list please
            if (personList == null) { return new List<Person>(); }

            var returnList = new List<Person>();

            foreach (var personJson in personList)
            {
                returnList.Add(Person.FromJson(personJson));
            }

            return returnList;
        }

        public static Person FromJson(JToken personInput)
        {
            //if null return empty, end here
            if (personInput == null) { return Person.Empty; }

            try
            {
                var personId = personInput["PersonId"].Value<string>();
                var name = personInput["Name"].Value<string>();
                var notes = personInput["Notes"].Value<string>();
                var time = Time.FromJson(personInput["BirthTime"]);
                var gender = Enum.Parse<Gender>(personInput["Gender"].Value<string>());
                var ownerId = personInput["OwnerId"].Value<string>();

                //note person ID injected into each life event
                var lifeEventList = LifeEvent.FromJsonList(personInput["LifeEventList"]);

                var parsedPerson = new Person(ownerId, personId, name, time, gender, notes, lifeEventList);

                return parsedPerson;
            }
            catch (Exception e)
            {

                #region DEBUG
                Console.WriteLine(e);
                #endregion

                LibLogger.Debug($"Failed to parse Person:\n{personInput.ToString()}");

                return Person.Empty;
            }

        }

        #endregion

        /// <summary>
        /// Given a parsed list of person will convert to JSON
        /// </summary>
        public static JArray ListToJson(List<Person> personList)
        {
            var returnValue = new JArray();
            foreach (var person in personList)
            {
                returnValue.Add(person.ToJson());
            }

            return returnValue;
        }
    }
}