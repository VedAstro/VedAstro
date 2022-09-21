using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Genso.Astrology.Library
{

    /// <summary>
    /// Simple class to encapsulate a HoroscopePrediction (data)
    /// Note : EventData + Time = HoroscopePrediction
    /// </summary>
    public class HoroscopePrediction : IHasName, IToXml
    {
        //FIELDS
        private readonly EventName _name;
        private readonly string _description;
        private readonly string _info;


        //CTOR
        public HoroscopePrediction(EventName name, string description, string info)
        {
            //initialize fields
            _name = name;
            _description = description;
            _info = info;
        }



        //PROPERTIES
        //Note: Created mainly for ease of use with WPF binding
        public EventName Name => _name;
        public string Description => _description;
        public string Info => _info;
        public string FormattedName => Format.FormatName(this);




        //PUBLIC METHODS
        public EventName GetName() => _name;

        /// <summary>
        /// Note: Root element must be named HoroscopePrediction
        /// </summary>
        public static HoroscopePrediction FromXml(XElement predictionXml)
        {
            var eventName = Enum.Parse<EventName>(predictionXml.Element("Name")?.Value ?? "EmptyEvent");
            var description = predictionXml.Element("Description")?.Value ?? "Empty Description";
            var info = predictionXml.Element("Info")?.Value ?? "Empty Info";

            var parsed = new HoroscopePrediction(eventName, description, info);

            return parsed;
        }

        /// <summary>
        /// Converts a list of prediction xml 
        /// Note: Root element must be named Root with children as HoroscopePrediction
        /// </summary>
        public static List<HoroscopePrediction> FromXmlList(XElement rootXml)
        {
            var returnList = new List<HoroscopePrediction>();

            foreach (var predictionXml in rootXml.Elements())
            {
                returnList.Add(HoroscopePrediction.FromXml(predictionXml));
            }

            return returnList;
        }

        /// <summary>
        /// Note root element is "Time"
        /// </summary>
        public XElement ToXml()
        {
            var predictionHolder = new XElement("HoroscopePrediction");
            var nameXml = new XElement("Name", this.Name.ToString());
            var descriptionXml = new XElement("Description", this.Description);
            var infoXml = new XElement("Info", this.Info);

            predictionHolder.Add(nameXml, descriptionXml, infoXml);

            return predictionHolder;
        }


        /// <summary>
        /// The root element is expected to be name of Type
        /// Note: Special method done to implement IToXml
        /// </summary>
        public dynamic FromXml<T>(XElement xml) where T : IToXml => FromXml(xml);


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

            if (value.GetType() == typeof(HoroscopePrediction))
            {
                //cast to type
                var parsedValue = (HoroscopePrediction)value;

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

            return hash1 + hash2;
        }

        public override string ToString()
        {
            return $"{FormattedName} - {Description} - {Info}";
        }



        /// <summary>
        /// Searches all text in prediction for input
        /// </summary>
        public bool Contains(string searchText)
        {
            //place all text together
            var compiledText = $"{FormattedName} {Description} {Info}";

            //do the searching
            string pattern = @"\b" + Regex.Escape(searchText) + @"\b"; //searches only words
            var searchResult = Regex.Match(compiledText, pattern, RegexOptions.IgnoreCase).Success;
            return searchResult;

        }
    }
}