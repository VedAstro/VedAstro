using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml.Linq;


//█▄█ █▀█ █░█ ▀ █▀█ █▀▀   ▄▀█ █░░ █░█░█ ▄▀█ █▄█ █▀   █░█░█ █ ▀█▀ █░█   █▄█ █▀█ █░█ █▀█   █▀ █░█░█ █▀▀ █▀▀ ▀█▀
//░█░ █▄█ █▄█ ░ █▀▄ ██▄   █▀█ █▄▄ ▀▄▀▄▀ █▀█ ░█░ ▄█   ▀▄▀▄▀ █ ░█░ █▀█   ░█░ █▄█ █▄█ █▀▄   ▄█ ▀▄▀▄▀ ██▄ ██▄ ░█░

//█░█ █▀▀ ▄▀█ █▀█ ▀█▀ ░   ░░█ █░█ █▀ ▀█▀   █▀▀ ▄▀█ █▄░█ ▀ ▀█▀   █▀ █▀▀ █▀▀   █ ▀█▀
//█▀█ ██▄ █▀█ █▀▄ ░█░ █   █▄█ █▄█ ▄█ ░█░   █▄▄ █▀█ █░▀█ ░ ░█░   ▄█ ██▄ ██▄   █ ░█░


namespace VedAstro.Library
{
    /// <summary>
    /// Represents a period of time "Event" with start, end time and data related
    /// </summary>
    public class Event : IToXml, IToJson
    {
        /// <summary>
        /// Returns an Empty Time instance meant to be used as null/void filler
        /// for debugging and generating empty dasa svg lines
        /// </summary>
        public static Event Empty = new Event(EventName.Empty, EventNature.Empty, "", SpecializedSummary.Empty,  Time.Empty, Time.Empty, new List<EventTag>());

        //FIELDS
        private readonly EventName _name;
        private readonly string _description;
        private readonly EventNature _nature;
        private readonly Time _startTime;
        private readonly Time _endTime;
        private readonly List<EventTag> _eventTags;


        //CTOR
        public Event(EventName name, EventNature nature, string description, SpecializedSummary specializedSummary, Time startTime, Time endTime, List<EventTag> eventTags)
        {
            //initialize fields
            _name = name;
            _nature = nature;
            _description = HttpUtility.HtmlEncode(description); //HTML character safe
            SpecializedSummary = specializedSummary;
            _startTime = startTime;
            _endTime = endTime;
            _eventTags = eventTags;
        }


        //PROPERTIES
        //Note: Created mainly for ease of use with WPF binding
        public EventName Name => _name;
        public string FormattedName => Format.FormatName(this.Name);
        public string Description => HttpUtility.HtmlDecode(_description);
        public EventNature Nature => _nature;
        public Time StartTime => _startTime;
        public Time EndTime => _endTime;
        public List<EventTag> EventTags => _eventTags;
        public SpecializedSummary SpecializedSummary { get; private set; }


        /// <summary>
        /// total duration of event in minutes
        /// </summary>
        public double DurationMin => GetDurationMinutes();

        /// <summary>
        /// total duration of event in Hours
        /// </summary>
        public double DurationHour => GetDurationHours();

        /// <summary>
        /// Score linked to the nature of the event,
        /// calculated & inject separately using Algorithms like Ishta Kashta Score
        /// </summary>
        public double NatureScore { get; set; }


        //PUBLIC METHODS
        public EventName GetName() => _name;

        public EventNature GetNature() => _nature;

        public Time GetStartTime() => _startTime;

        public string GetDescription() => _description;

        public Time GetEndTime() => _endTime;


        //PRIVATE METHODS
        private static string SplitCamelCase(string str)
        {
            return Regex.Replace(
                Regex.Replace(
                    str,
                    @"(\P{Ll})(\P{Ll}\p{Ll})",
                    "$1 $2"
                ),
                @"(\p{Ll})(\P{Ll})",
                "$1 $2"
            );
        }


        //METHOD OVERRIDES
        public override bool Equals(object value)
        {

            if (value != null && value.GetType() == typeof(Event))
            {
                //cast to type
                var parsedValue = (Event)value;

                //check equality
                bool returnValue = (this.GetHashCode() == parsedValue.GetHashCode());

                return returnValue;
            }
            else
            {
                //Return false if value is null
                return false;
            }


        }

        public override int GetHashCode()
        {
            //get hash of all the fields & combine them
            var hash1 = _name.GetHashCode();
            var hash2 = Tools.GetStringHashCode(_description);
            var hash3 = _nature.GetHashCode();
            var hash4 = _startTime.GetHashCode();
            var hash5 = _endTime.GetHashCode();

            return hash1 + hash2 + hash3 + hash4 + hash5;
        }

        public override string ToString()
        {
            return $"{GetName()} - {_nature} - {_startTime} - {_endTime} - {GetDurationMinutes()}";
        }

        /// <summary>
        /// Gets the duration of the event from start to end time
        /// </summary>
        public double GetDurationMinutes()
        {
            var difference = GetEndTime().GetStdDateTimeOffset() - GetStartTime().GetStdDateTimeOffset();

            return difference.TotalMinutes;
        }

        /// <summary>
        /// Gets the duration of the event from start to end time
        /// </summary>
        public double GetDurationHours()
        {
            var difference = GetEndTime().GetStdDateTimeOffset() - GetStartTime().GetStdDateTimeOffset();

            return difference.TotalHours;
        }

        /// <summary>
        /// The root element is expected to be Person
        /// Note: Special method done to implement IToXml
        /// </summary>
        public dynamic FromXml<T>(XElement xml) where T : IToXml => FromXml(xml);

        /// <summary>
        /// Convert an XML representation of Event to an Event instance
        /// Accepts both 1 XML Event and a list of XML events placed in 1 "Root" element
        /// But always returns a list
        /// </summary>
        public static List<Event> FromXml(XElement resultsRaw)
        {
            var returnList = new List<Event>();

            //first find out if it's 1 or list
            var firstElementName = resultsRaw.Name.LocalName;
            var isList = firstElementName == "Root";

            if (isList)
            {
                //parse as list
                foreach (var eventXml in resultsRaw.Elements())
                {
                    returnList.Add(XmlToEvent(eventXml));
                }

            }
            //else it is only 1 event xml
            else
            {
                returnList.Add(XmlToEvent(resultsRaw));
            }


            return returnList;


            //FUNCTIONS

            //convert 1 event xml to instance
            Event XmlToEvent(XElement eventXml)
            {

                var nameXml = eventXml.Element("Name")?.Element(typeof(EventName).FullName);
                var name = Enum.Parse<EventName>(nameXml.Value);

                var natureXml = eventXml.Element("Nature")?.Element(typeof(EventNature).FullName);
                var nature = Enum.Parse<EventNature>(natureXml.Value);

                var descriptionXml = eventXml.Element("Description")?.Element(typeof(String).FullName);
                var description = descriptionXml.Value;

                var startTimeXml = eventXml.Element("StartTime").Element("Time");
                var startTime = Time.FromXml(startTimeXml);

                var endTimeXml = eventXml.Element("EndTime").Element("Time");
                var endTime = Time.FromXml(endTimeXml);

                // Get the list of tags, split by comma and parse each tag
                var tagString = eventXml.Element("Tag")?.Value;
                var tagList = EventData.GetEventTags(tagString);

                //note: specialized summary only pumped in at static tables with LLM preprocessing
                //      so when coming from XML file touched by user, there should not be any specialized summary
                var specializedSummary = SpecializedSummary.Empty;
                var parsedPerson = new Event(name, nature, description, specializedSummary,  startTime, endTime, tagList);

                return parsedPerson;

            }
        }

        /// <summary>
        /// converts the current instance of Event to its XML version
        /// </summary>
        public XElement ToXml()
        {
            var eventXml = new XElement("Event");
            var nameXml = new XElement("Name", Tools.AnyTypeToXml(this.Name));
            var natureXml = new XElement("Nature", Tools.AnyTypeToXml(this.Nature));
            var descriptionXml = new XElement("Description", Tools.AnyTypeToXml(this.Description));
            var startTimeXml = new XElement("StartTime", Tools.AnyTypeToXml(this.StartTime));
            var endTimeXml = new XElement("EndTime", Tools.AnyTypeToXml(this.EndTime));

            eventXml.Add(nameXml, descriptionXml, natureXml, startTimeXml, endTimeXml);

            return eventXml;

        }

        /// <summary>
        /// Checks if this event occurred at the inputed time
        /// </summary>
        public bool IsOccurredAtTime(Time inputTime)
        {
            //input time is after (more than) event start time
            var afterStart = inputTime >= this.StartTime;

            //input time is before (less than) event end time
            var beforeStart = inputTime <= this.EndTime;

            //time occurred only if both conditions met
            var result = afterStart && beforeStart;
            return result;

        }

        /// <summary>
        /// gets all planets that this event is influenced by
        /// extracted from name
        /// </summary>
        public List<PlanetName> GetRelatedPlanet()
        {
            //every time planet is mentioned add to list
            var eventName = this.Name.ToString(); //take without spacing
            var returnList = new List<PlanetName>();
            foreach (var planetName in PlanetName.All9Planets)
            {
                var found = eventName.Contains(planetName.Name.ToString(), StringComparison.OrdinalIgnoreCase);

                //if contains planet name, add to return list
                if (found) { returnList.Add(planetName); }
            }

            //remove duplicates
            returnList = new List<PlanetName>(returnList.Distinct());

            //return to caller
            return returnList;
        }

        /// <summary>
        /// gets all planets that this event is influenced by
        /// extracted from name, if no house in name, then returns empty list
        /// </summary>
        public List<HouseName> GetRelatedHouse()
        {

            var eventName = this.Name.ToString().ToLower(); //take without spacing

            List<int> numbers = new List<int>();
            // Use regex to find matches
            MatchCollection matches = Regex.Matches(eventName, @"house(\d+)");
            // Loop through matches
            foreach (Match match in matches)
            {
                // Try to parse the number after "house"
                if (int.TryParse(match.Groups[1].Value, out int number))
                {
                    numbers.Add(number);
                }
            }

            //convert to standard names
            var houseList = numbers.Select(x => (HouseName)x).ToList();

            //remove duplicates
            houseList = new List<HouseName>(houseList.Distinct());

            //return to caller
            return houseList;
        }



        #region JSON SUPPORT

        JObject IToJson.ToJson() => (JObject)this.ToJson();

        public JToken ToJson()
        {
            var temp = new JObject();
            temp["Name"] = this.Name.ToString();
            temp["Nature"] = this.Name.ToString();
            temp["Description"] = this.Name.ToString();
            temp["StartTime"] = this.StartTime.ToJson();
            temp["EndTime"] = this.EndTime.ToJson();

            return temp;
        }

        /// <summary>
        /// Given a json list of person will convert to instance
        /// used for transferring between server & client
        /// </summary>
        public static List<Event> FromJsonList(JToken personList)
        {
            //if null empty list please
            if (personList == null) { return new List<Event>(); }

            var returnList = new List<Event>();

            foreach (var personJson in personList)
            {
                returnList.Add(Event.FromJson(personJson));
            }

            return returnList;
        }

        public static JArray ToJsonList(List<Event> eventList)
        {
            var jsonList = new JArray();

            foreach (var eventInstance in eventList)
            {
                jsonList.Add(eventInstance.ToJson());
            }

            return jsonList;
        }

        public static Event FromJson(JToken planetInput)
        {
            //if null return empty, end here
            if (planetInput == null) { return Event.Empty; }

            try
            {
                var nameStr = planetInput["Name"].Value<string>();
                var name = (EventName)Enum.Parse(typeof(EventName), nameStr);

                var natureStr = planetInput["Nature"].Value<string>();
                var nature = (EventNature)Enum.Parse(typeof(EventNature), natureStr);

                var description = planetInput["Description"].Value<string>();
                
                var startTime = Time.FromJson(planetInput["StartTime"]);
                var endTime = Time.FromJson(planetInput["EndTime"]);
                var specializedSummary = SpecializedSummary.FromJson(planetInput["SpecializedSummary"]);

                // Get the list of tags, split by comma and parse each tag
                var tagString = planetInput["Tag"]?.Value<string>();
                var tagList = EventData.GetEventTags(tagString);

                var parsedHoroscope = new Event(name, nature, description, specializedSummary, startTime, endTime, tagList);

                return parsedHoroscope;
            }
            catch (Exception e)
            {
                LibLogger.Debug($"Failed to parse:\n{planetInput.ToString()}");

                return Event.Empty;
            }

        }

        #endregion


    }
}