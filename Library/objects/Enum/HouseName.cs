using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace VedAstro.Library
{

    public enum HouseName
    {
        //Note: maintain int relation for code that is still using int
        House1 = 1,
        House2 = 2,
        House3 = 3,
        House4 = 4,
        House5 = 5,
        House6 = 6,
        House7 = 7,
        House8 = 8,
        House9 = 9,
        House10 = 10,
        House11 = 11,
        House12 = 12,
    }

    public static class HouseNameExtensions
    {

        /// <summary>
        /// If fail will return null
        /// </summary>
        public static HouseName? FromString(string inputHouseName)
        {
            var houseName = (HouseName)Enum.Parse(typeof(HouseName), inputHouseName,true);

            return houseName;
        }

        /// <summary>
        /// Note: Root element must be named HouseName
        /// </summary>
        public static HouseName FromXml(XElement houseNameXml)
        {
            //converts string to enum instance
            Enum.TryParse(houseNameXml.Value, out HouseName eventTag);

            return eventTag;
        }

        /// <summary>
        /// Note: Root element must be named EventTagList
        /// </summary>
        public static List<HouseName> FromXmlList(XElement houseNameListXml)
        {
            var returnList = new List<HouseName>();
            foreach (var houseNameXml in houseNameListXml.Elements())
            {
                returnList.Add(HouseNameExtensions.FromXml(houseNameXml));
            }
            return returnList;
        }

        /// <summary>
        /// Note: Root element must be named HouseNameList
        /// </summary>
        public static XElement ToXmlList(List<HouseName> houseNameList)
        {
            var houseNameListXml = new XElement("HouseNameList");

            foreach (var houseName in houseNameList)
            {
                houseNameListXml.Add(houseName.ToXml());
            }

            return houseNameListXml;
        }

        /// <summary>
        /// Note root element is "HouseName"
        /// </summary>
        public static XElement ToXml(this HouseName _eventTag)
        {
            var holder = new XElement("HouseName", _eventTag.ToString());

            return holder;
        }
    }

}
