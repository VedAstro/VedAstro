using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml.Linq;

namespace Genso.Astrology.Library
{

    /// <summary>
    /// Data structure to encapsulate an event before it's calculated
    /// In other words an object instance of the event data as stored in file
    /// </summary>
    public struct EventData : IHasName
    {
        /** FIELDS **/


        private readonly EventCalculator _eventCalculator;


        /** CTOR **/
        public EventData(EventName name, EventNature nature, string description, List<EventTag> eventTags, EventCalculator eventCalculator)
        {
            Name = name;
            Nature = nature;
            Description = HttpUtility.HtmlEncode(description); //HTML character safe
            Strength = "";//strength is only gotten after calculator is run
            EventTags = eventTags;
            _eventCalculator = eventCalculator;
        }


        /** PROPERTIES **/
        //mainly created for access from WPF binding
        public EventName Name { get; }
        public string FormattedName => Format.FormatName(this);
        public EventNature Nature { get; private set; }
        public string Description { get; }
        public string Strength { get; set; }
        public List<EventTag> EventTags { get; }



        /** PUBLIC METHODS **/
        public bool IsEventOccuring(Time time, Person person)
        {
            //do calculation for this event to get prediction data
            var predictionData = _eventCalculator(time, person);

            //extract the data out and store it for later use
            //is prediction occuring
            bool isEventOccuring = predictionData.Occuring;

            //prediction strength
            Strength = predictionData.Info;

            //override even nature from xml if specified
            Nature = predictionData.NatureOverride == EventNature.Empty ? Nature : predictionData.NatureOverride;

            //let caller know if occuring
            return isEventOccuring;
        }

        public EventName GetName() => Name;

        public EventNature GetNature() => Nature;

        public string GetDescription() => Description;

        public string GetStrength() => Strength;
        
        public List<EventTag> GetEventTags() => EventTags;



        /// <summary>
        /// Converts XML to Instance
        /// </summary>
        public static EventData FromXml(XElement eventData)
        {
            //extract the individual data out & convert it to the correct type
            var nameString = eventData.Element("Name")?.Value;
            Enum.TryParse(nameString, out EventName name);
            var natureString = eventData.Element("Nature")?.Value;
            Enum.TryParse(natureString, out EventNature nature);
            var rawDescription = eventData.Element("Description")?.Value;
            var description = CleanText(rawDescription); //with proper formatting
            var tagString = eventData.Element("Tag")?.Value;
            var tagList = getEventTags(tagString); //multiple tags are possible ',' separated
            var calculatorMethod = EventManager.GetEventCalculatorMethod(name);

            //place the data into an event data structure
            var eventX = new EventData(name, nature, description, tagList, calculatorMethod);

            return eventX;


            //Gets a list of tags in string form & changes it a structured list of tags
            //Multiple tags can be used by 1 event, separated by comma in in the Tag element
            List<EventTag> getEventTags(string rawTags)
            {
                //create a place to store the parsed tags
                var returnTags = new List<EventTag>();

                //split the string by comma "," (tag separator)
                var splitedRawTags = rawTags.Split(',');

                //parse each raw tag
                foreach (var rawTag in splitedRawTags)
                {
                    //parse
                    var result = Enum.TryParse(rawTag, out EventTag eventTag);
                    //raise error if could not parse
                    if (!result) throw new Exception("Event tag not found!");

                    //add the parsed tag to the return list
                    returnTags.Add(eventTag);
                }

                return returnTags;
            }

            //little function to format the description coming from the file
            //so that the description wraps nicely when rendered
            string CleanText(string text)
            { 
                //remove all special characters
                var cleaned = Regex.Replace(text, @"\s+", " ");

                //HTML character safe
                cleaned = HttpUtility.HtmlEncode(cleaned); 

                return cleaned;
            }


        }

        /// <summary>
        /// Searches all text in prediction for input
        /// </summary>
        public bool Contains(string searchText)
        {
            //place all text together
            var compiledText = $"{FormattedName} {Description} {Nature} {string.Join(",", EventTags)}";

            //do the searching
            string pattern = @"\b" + Regex.Escape(searchText) + @"\b"; //searches only words
            var searchResult = Regex.Match(compiledText, pattern, RegexOptions.IgnoreCase).Success;
            return searchResult;

        }



        /** METHOD OVERRIDES **/
        public override bool Equals(object value)
        {

            if (value.GetType() == typeof(EventData))
            {
                //cast to type
                var parsedValue = (EventData)value;

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
            var hash1 = Name.GetHashCode();
            var hash2 = Nature.GetHashCode();
            var hash3 = Tools.GetStringHashCode(Description);

            return hash1 + hash2 + hash3;
        }

        public override string ToString()
        {
            return $"{Name} - {Nature} - {Description}";
        }

        public static bool operator ==(EventData left, EventData right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(EventData left, EventData right)
        {
            return !(left == right);
        }


        /// <summary>
        /// Converts an XML list of Event Data to instance List
        /// </summary>
        public static List<EventData> FromXmlList(List<XElement> eventDataListXml)
        {
            //create a place to store the list
            List<EventData> eventDataList = new List<EventData>();

            //parse each raw event data in list
            foreach (var eventDataXml in eventDataListXml)
            {

                //add it to the return list
                eventDataList.Add(EventData.FromXml(eventDataXml));
            }

            //return the list to caller
            return eventDataList;
        }
    }
}