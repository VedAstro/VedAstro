using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml.Linq;

namespace VedAstro.Library
{

    /// <summary>
    /// FOR HOROSCOPE EVENTS ONLY
    /// Data structure to encapsulate an event before it's calculated
    /// In other words an object instance of the event data as stored in file
    /// </summary>
    public struct HoroscopeData 
    {
        /** FIELDS **/


        private string _description = "";


        /** CTOR **/
        public HoroscopeData(HoroscopeName name, EventNature nature, string description, List<EventTag> eventTags, HoroscopeCalculatorDelegate horoscopeCalculator)
        {
            Name = name;
            Nature = nature;
            HoroscopeCalculator = horoscopeCalculator;
            Description = description;
            EventTags = eventTags;
        }


        /** PROPERTIES **/
        //mainly created for access from WPF binding
        public HoroscopeName Name { get; private set; } = HoroscopeName.Empty;
        
        /// <summary>
        /// Gets human readable Event Name, removes camel case
        /// </summary>
        public string FormattedName => Format.FormatName(this);
        
        public EventNature Nature { get; private set; }
        
        public HoroscopeCalculatorDelegate HoroscopeCalculator { get; private set; }
        
        public string Description
        {
            get => HttpUtility.HtmlDecode(_description);
            set => _description = HttpUtility.HtmlEncode(value);
        }

        /// <summary>
        /// Contains data about planets, houses, and signs related to a calculation
        /// Note: filled when IsEventOccuring is called
        /// </summary>
        public RelatedBody RelatedBody { get; set; } = new RelatedBody();
        
        public List<EventTag> EventTags { get; }


        /** PUBLIC METHODS **/


        /// <summary>
        /// Calculator Results are made here
        /// Once run RelatedBody becomes available
        /// </summary>
        public bool IsEventOccuring(Time time)
        {
            //do calculation for this event to get prediction data
            var predictionData = this.HoroscopeCalculator(time);

            //extract the data out and store it for later use
            //is prediction occuring
            bool isEventOccuring = predictionData.Occuring;

            //store planets, houses & signs related to result
            RelatedBody = predictionData.RelatedBody;

            //override event nature from xml if specified
            Nature = predictionData.NatureOverride == EventNature.Empty ? Nature : predictionData.NatureOverride;

            //override description if specified
            var isDescriptionOverride = !string.IsNullOrEmpty(predictionData.DescriptionOverride); //if true override, else false
            Description = isDescriptionOverride ? predictionData.DescriptionOverride : Description;

            //let caller know if occuring
            return isEventOccuring;
        }



        /// <summary>
        /// Converts XML to Instance
        /// </summary>
        public static HoroscopeData FromXml(XElement horoscopeData)
        {
            //extract the individual data out & convert it to the correct type
            var nameString = horoscopeData.Element("Name")?.Value;
            Enum.TryParse(nameString, out HoroscopeName name);
            var natureString = horoscopeData.Element("Nature")?.Value;
            Enum.TryParse(natureString, out EventNature nature);
            var rawDescription = horoscopeData.Element("Description")?.Value;
            var description = CleanText(rawDescription); //with proper formatting
            var tagString = horoscopeData.Element("Tag")?.Value;
            var tagList = getEventTags(tagString); //multiple tags are possible ',' separated
            var calculatorMethod = EventManager.GetHoroscopeCalculatorMethod(name);

            //place the data into an event data structure
            var eventX = new HoroscopeData(name, nature, description, tagList, calculatorMethod);

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

            if (value.GetType() == typeof(HoroscopeData))
            {
                //cast to type
                var parsedValue = (HoroscopeData)value;

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

        public static bool operator ==(HoroscopeData left, HoroscopeData right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(HoroscopeData left, HoroscopeData right)
        {
            return !(left == right);
        }


        /// <summary>
        /// Converts an XML list of Event Data to instance List
        /// </summary>
        public static List<HoroscopeData> FromXmlList(List<XElement> horoscopeDataListXml)
        {
            //create a place to store the list
            List<HoroscopeData> horoscopeDataList = new List<HoroscopeData>();

            //parse each raw event data in list
            foreach (var horoscopeDataXml in horoscopeDataListXml)
            {

                //add it to the return list
                horoscopeDataList.Add(HoroscopeData.FromXml(horoscopeDataXml));
            }

            //return the list to caller
            return horoscopeDataList;
        }
    }
}