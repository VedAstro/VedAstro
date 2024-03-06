using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;

namespace VedAstro.Library
{

    /// <summary>
    /// Data structure to encapsulate an event before it's calculated
    /// In other words an object instance of the event data as stored in file
    /// </summary>
    public struct EventData : IToXml, IToJson
    {
        /** FIELDS **/

        public static EventData Empty = new();

        private string _description = "";


        /** CTOR **/
        public EventData(EventName name, EventNature nature, SpecializedSummary specializedNature, string description, List<EventTag> eventTags, EventCalculatorDelegate eventCalculator)
        {
            Name = name;
            Nature = nature;
            SpecializedSummary = specializedNature;
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
        public string FormattedName => Format.FormatName(this.Name);

        public EventNature Nature { get; private set; }

        public SpecializedSummary SpecializedSummary { get; private set; }

        public EventCalculatorDelegate EventCalculator { get; private set; }

        /// <summary>
        /// Auto encoding to HTML safe on get & set
        /// </summary>
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

        /// <summary>
        /// Converts XML to Instance
        /// </summary>
        public static EventData FromXml(XElement eventData)
        {
            // Check if eventData is null
            if (eventData == null)
            {
                throw new ArgumentNullException(nameof(eventData));
            }
            // Try to parse EventName and EventNature from XML, use Empty as default
            Enum.TryParse(eventData.Element("Name")?.Value ?? "Empty", out EventName name);
            Enum.TryParse(eventData.Element("Nature")?.Value ?? "Empty", out EventNature nature);
            var specializedNature = SpecializedSummary.Empty; //NOTE: specialized is pumped in static generator using llm data, hence here empty 
            // Clean the description text
            var rawDescription = eventData.Element("Description")?.Value ?? "Empty";
            var description = CleanText(rawDescription);
            // Get the list of tags, split by comma and parse each tag
            var tagString = eventData.Element("Tag")?.Value;
            var tagList = GetEventTags(tagString);
            // Get the calculator method for the event
            var calculatorMethod = EventManager.GetEventCalculatorMethod(name);
            // Create and return the EventData object
            var eventX = new EventData(name, nature, specializedNature, description, tagList, calculatorMethod);
            return eventX;
        }

        /// <summary>
        /// Handles nulls nicely
        /// </summary>
        public static List<EventTag> GetEventTags(string rawTags)
        {
            if (string.IsNullOrEmpty(rawTags))
            {
                return new List<EventTag>();
            }
            // Split the string by comma and parse each tag
            var returnTags = rawTags.Split(',')
                                    .Select(rawTag =>
                                    {
                                        if (!Enum.TryParse(rawTag, out EventTag eventTag))
                                        {
                                            throw new Exception($"Event tag '{rawTag}' not found!");
                                        }
                                        return eventTag;
                                    })
                                    .ToList();
            return returnTags;
        }

        private static string CleanText(string text)
        {
            // Remove all special characters and decode HTML
            var cleaned = Regex.Replace(text, @"\s+", " ");
            cleaned = HttpUtility.HtmlDecode(cleaned);
            return cleaned;
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
        /// Given a json list of eventData will convert to instance
        /// used for transferring between server & client
        /// </summary>
        public static List<EventData> FromJsonList(JToken eventDataList)
        {
            //if null empty list please
            if (eventDataList == null) { return new List<EventData>(); }

            var returnList = new List<EventData>();

            foreach (var eventDataJson in eventDataList)
            {
                returnList.Add(EventData.FromJson(eventDataJson));
            }

            return returnList;
        }

        JObject IToJson.ToJson() => (JObject)this.ToJson();

        public JObject ToJson()
        {
            // Check if eventData is null
            if (this == null) { return new JObject(); }

            // Create a new JObject
            var json = new JObject();

            // Convert EventName and EventNature to string and add to JObject
            json["Name"] = this.Name.ToString();
            json["Nature"] = this.Nature.ToString();

            // Convert SpecializedSummary to JObject and add to JObject
            json["SpecializedSummary"] = this.SpecializedSummary.ToJson();

            // Add the description text to JObject
            json["Description"] = this.Description;

            // Convert the list of tags to a comma-separated string and add to JObject
            var tagString = string.Join(",", this.EventTags);
            json["Tag"] = tagString;

            // Convert the calculator method to string (if possible) and add to JObject
            // Note: This assumes that the calculator method can be represented as a string. 
            // If it can't, you might need to remove this line or handle it differently.
            json["CalculatorMethod"] = this.EventCalculator.Method.Name;

            // Return the JObject
            return json;
        }

        public static EventData FromJson(JToken eventData)
        {
            // Check if eventData is null
            if (eventData == null) { return EventData.Empty; }

            // Try to parse EventName and EventNature from JSON, use Empty as default
            Enum.TryParse(eventData["Name"]?.Value<string>() ?? "Empty", out EventName name);
            Enum.TryParse(eventData["Nature"]?.Value<string>() ?? "Empty", out EventNature nature);

            // Extract SpecializedSummary from JSON
            var specializedNature = SpecializedSummary.FromJson(eventData["SpecializedSummary"] as JObject);

            // Clean the description text
            var rawDescription = eventData["Description"]?.Value<string>() ?? "Empty";
            var description = CleanText(rawDescription);

            // Get the list of tags, split by comma and parse each tag
            var tagString = eventData["Tag"]?.Value<string>();
            var tagList = GetEventTags(tagString);

            // Get the calculator method for the event
            var calculatorMethod = EventManager.GetEventCalculatorMethod(name);

            // Create and return the EventData object
            var eventX = new EventData(name, nature, specializedNature, description, tagList, calculatorMethod);
            return eventX;
        }

        /// <summary>
        /// Given a parsed list of EventData will convert to JSON
        /// </summary>
        public static JArray ListToJson(List<EventData> eventDataList)
        {
            var returnValue = new JArray();
            foreach (var eventData in eventDataList)
            {
                returnValue.Add(eventData.ToJson());
            }

            return returnValue;
        }

    }
}