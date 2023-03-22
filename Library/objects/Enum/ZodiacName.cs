using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace VedAstro.Library
{
    [Serializable()]
    public enum ZodiacName
    {
        Aries = 1,
        Taurus = 2,
        Gemini = 3,
        Cancer = 4,
        Leo = 5,
        Virgo = 6,
        Libra = 7,
        Scorpio = 8,
        Sagittarius = 9,
        Capricornus = 10,
        Aquarius = 11,
        Pisces = 12
    }

    public static class ZodiacNameExtensions
    {

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
    }

}