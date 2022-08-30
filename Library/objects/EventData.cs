using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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
            Description = description;
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
            var description = getDescription(eventData.Element("Description")?.Value); //with proper formatting
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
            string getDescription(string rawDescription)
            {
                //remove new line
                //var cleaned1 = rawDescription.Replace("\n", "").Replace("\r", "");

                //remove double spaces
                //RegexOptions options = RegexOptions.None;
                //Regex regex = new Regex("[ ]{3,}", options);
                //var cleaned2 = regex.Replace(cleaned1, " ");
                var cleaned = Regex.Replace(rawDescription, @"\s+", " ");

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

        
    }
}