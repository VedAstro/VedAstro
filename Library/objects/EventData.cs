using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml.Linq;

namespace VedAstro.Library
{

    /// <summary>
    /// Data structure to encapsulate an event before it's calculated
    /// In other words an object instance of the event data as stored in file
    /// </summary>
    public struct EventData : IHasName, IToXml
    {
        /** FIELDS **/


        private string _description = "";


        /** CTOR **/
        public EventData(EventName name, EventNature nature, string description, List<EventTag> eventTags, EventCalculatorDelegate eventCalculator)
        {
            Name = name;
            Nature = nature;
            EventCalculator = eventCalculator;
            Description = description;
            EventTags = eventTags;
        }


        /** PROPERTIES **/
        //mainly created for access from WPF binding
        public EventName Name { get; private set; } = EventName.Empty;
        
        /// <summary>
        /// Gets human readable Event Name, removes camel case
        /// </summary>
        public string FormattedName => Format.FormatName(this);

        public EventNature Nature { get; private set; }
        
        public EventCalculatorDelegate EventCalculator { get; private set; }
        
        public string Description
        {
            get => HttpUtility.HtmlDecode(_description);
            set => _description = HttpUtility.HtmlEncode(value);
        }

        /// <summary>
        /// Contains data about planets, houses, and signs related to a calculation
        /// Note: filled when IsEventOccuring is called
        /// </summary>
        public RelatedBody RelatedBody { get; set; } = new RelatedBody(); //default empty
        
        public List<EventTag> EventTags { get; }


        /** PUBLIC METHODS **/


       



        /// <summary>
        /// Converts XML to Instance
        /// </summary>
        public static EventData FromXml(XElement eventData)
        {
            //extract the individual data out & convert it to the correct type
            //NOTE : some XML elements like nature & description is filled
            //       by code, so is empty  tag like <Nature/>

            //set defaults, so that wont fail when hit empty tags
            EventName name = EventName.Empty;
            EventNature nature = EventNature.Empty;

            var nameString = eventData.Element("Name")?.Value ?? "Empty";
            Enum.TryParse(nameString, out name);
            var natureString = eventData.Element("Nature")?.Value ?? "Empty";
            Enum.TryParse(natureString, out nature);
            var rawDescription = eventData.Element("Description")?.Value ?? "Empty";
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
            //so that the description wraps nicely when rendered also HTML Decode
            string CleanText(string text)
            {
                //remove all special characters
                var cleaned = Regex.Replace(text, @"\s+", " ");

                //decode special HTML character, exp: &,'
                cleaned = HttpUtility.HtmlDecode(cleaned);

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

        public XElement ToXml()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The root element is expected to be Person
        /// Note: Special method done to implement IToXml
        /// </summary>
        public dynamic FromXml<T>(XElement xml) where T : IToXml => FromXml(xml);

    }
}