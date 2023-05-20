using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml.Linq;

namespace VedAstro.Library
{
    public class Event : IHasName, IToXml
    {
        /// <summary>
        /// Returns an Empty Time instance meant to be used as null/void filler
        /// for debugging and generating empty dasa svg lines
        /// </summary>
        public static Event Empty = new Event(EventName.Empty, EventNature.Empty, "", Time.Empty, Time.Empty);

        //FIELDS
        private readonly EventName _name;
        private readonly string _description;
        private readonly EventNature _nature;
        private readonly Time _startTime;
        private readonly Time _endTime;


        //CTOR
        public Event(EventName name, EventNature nature, string description, Time startTime, Time endTime)
        {
            //initialize fields
            _name = name;
            _nature = nature;
            _description = HttpUtility.HtmlEncode(description); //HTML character safe
            _startTime = startTime;
            _endTime = endTime;
        }


        //PROPERTIES
        //Note: Created mainly for ease of use with WPF binding
        public EventName Name => _name;
        public string FormattedName => Format.FormatName(this);
        public string Description => HttpUtility.HtmlDecode(_description);
        public EventNature Nature => _nature;
        public Time StartTime => _startTime;
        public Time EndTime => _endTime;
        public double Duration => GetDurationMinutes();



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
                var name =  Enum.Parse<EventName>(nameXml.Value);

                var natureXml = eventXml.Element("Nature")?.Element(typeof(EventNature).FullName);
                var nature = Enum.Parse<EventNature>(natureXml.Value);

                var descriptionXml = eventXml.Element("Description")?.Element(typeof(String).FullName);
                var description = descriptionXml.Value;
                
                var startTimeXml = eventXml.Element("StartTime").Element("Time");
                var startTime = Time.FromXml(startTimeXml);

                var endTimeXml = eventXml.Element("EndTime").Element("Time");
                var endTime = Time.FromXml(endTimeXml);


                var parsedPerson = new Event( name,  nature,  description,  startTime,  endTime);

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
            var eventName = this.FormattedName.ToLower(); //take formatted name with spacing
            var returnList = new List<PlanetName>();
            foreach (var planetName in PlanetName.All9Planets)
            {
                var found = eventName.Contains(planetName.ToString().ToLower());
                if (found)
                {
                    returnList.Add(planetName);
                }
            }

            //remove duplicates
            returnList = new List<PlanetName>(returnList.Distinct());

            //return to caller
            return returnList;
        }

        /// <summary>
        /// gets all planets that this event is influenced by
        /// extracted from name
        /// </summary>
        public List<HouseName> GetRelatedHouse()
        {
            //every time planet is mentioned add to list
            var eventName = this.Name.ToString().ToLower(); //take without spacing
            var returnList = new List<HouseName>();
            foreach (var houseName in House.AllHouses)
            {
                var found = eventName.Contains(houseName.ToString().ToLower());
                if (found)
                {
                    returnList.Add(houseName);
                }
            }

            //remove duplicates
            returnList = new List<HouseName>(returnList.Distinct());

            //return to caller
            return returnList;
        }


    }
}