using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace VedAstro.Library
{
    /// <summary>
    /// note EMPTY not allowed here because, used to generate list of 12 for looping
    /// </summary>
    [Serializable()]
    public enum ZodiacName
    {
        Empty = 0,
        Aries = 1,
        Taurus = 2,
        Gemini = 3,
        Cancer = 4,
        Leo = 5,
        Virgo = 6,
        Libra = 7,
        Scorpio = 8,
        Sagittarius = 9,
        Capricorn = 10,
        Aquarius = 11,
        Pisces = 12
    }

    /// <summary>
    /// Follow naming convention "{Enum}Extensions" (depended by Open API)
    /// </summary>
    public static class ZodiacNameExtensions
    {

        /// <summary>
        /// All 12 zodiac signs in order
        /// </summary>
        public static List<ZodiacName> AllZodiacSigns = Enum.GetValues(typeof(ZodiacName))
                                                            .Cast<ZodiacName>()
                                                            .Where(z => z != ZodiacName.Empty) //remove empty
                                                            .ToList();

        /// <summary>
        /// Note: Root element must be named ZodiacName
        /// </summary>
        public static ZodiacName FromXml(XElement zodiacNameXml)
        {
            //converts string to enum instance
            Enum.TryParse(zodiacNameXml.Value, out ZodiacName eventTag);

            return eventTag;
        }

        /// <summary>
        /// Note: Root element must be named EventTagList
        /// </summary>
        public static List<ZodiacName> FromXmlList(XElement zodiacNameListXml)
        {
            var returnList = new List<ZodiacName>();
            foreach (var zodiacNameXml in zodiacNameListXml.Elements())
            {
                returnList.Add(ZodiacNameExtensions.FromXml(zodiacNameXml));
            }
            return returnList;
        }

        /// <summary>
        /// Note: Root element must be named ZodiacNameList
        /// </summary>
        public static XElement ToXmlList(List<ZodiacName> zodiacNameList)
        {
            var zodiacNameListXml = new XElement("ZodiacNameList");

            foreach (var zodiacName in zodiacNameList)
            {
                zodiacNameListXml.Add(zodiacName.ToXml());
            }

            return zodiacNameListXml;
        }

        /// <summary>
        /// Note root element is "ZodiacName"
        /// </summary>
        public static XElement ToXml(this ZodiacName _eventTag)
        {
            var holder = new XElement("ZodiacName", _eventTag.ToString());

            return holder;
        }

        /// <summary>
        /// Gets all zodiac signs in order in an dictionary with a  value,
        /// used for making astavarga charts
        /// </summary>
        public static Dictionary<ZodiacName, T> AllZodiacSignsDictionary<T>(T defaultValue)
        {
            var returnVal = new Dictionary<ZodiacName, T>();

            foreach (var sign in ZodiacNameExtensions.AllZodiacSigns)
            {
                returnVal.Add(sign, defaultValue);
            }

            return returnVal;
        }

        #region JSON SUPPORT


        /// <summary>
        /// Given a json list of person will convert to instance
        /// used for transferring between server & client
        /// </summary>
        public static List<ZodiacName> FromJsonList(JToken personList)
        {
            //if null empty list please
            if (personList == null) { return new List<ZodiacName>(); }

            var returnList = new List<ZodiacName>();

            foreach (var personJson in personList)
            {
                returnList.Add(ZodiacNameExtensions.FromJson(personJson));
            }

            return returnList;
        }

        public static JArray ToJsonList(List<ZodiacName> zodiacNameList)
        {
            var jsonList = new JArray();

            foreach (var eventTag in zodiacNameList)
            {
                jsonList.Add(eventTag.ToString());
            }

            return jsonList;
        }


        public static ZodiacName FromJson(JToken planetInput)
        {
            //if null return empty, end here
            if (planetInput == null) { return ZodiacName.Empty; }

            try
            {
                var valueStr = planetInput.Value<string>();
                var parsedEnum = (ZodiacName)Enum.Parse(typeof(ZodiacName), valueStr);

                return parsedEnum;
            }
            catch (Exception e)
            {
                LibLogger.Debug($"Failed to parse:\n{planetInput.ToString()}");

                return ZodiacName.Empty;
            }

        }

        #endregion


    }

}