using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace VedAstro.Library
{

    public enum HouseName
    {
        //Note: maintain int relation for code that is still using int
        Empty = 0,
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



    /// <summary>
    /// Follow naming convention "{Enum}Extensions" (depended by Open API)
    /// </summary>
    public static class HouseNameExtensions
    {

        public static string ToString(this HouseName inputHouse)
        {
            return $"House {(int)inputHouse}";
        }

        /// <summary>
        /// If fail will return null
        /// </summary>
        public static HouseName? FromString(string inputHouseName)
        {
            var houseName = (HouseName)Enum.Parse(typeof(HouseName), inputHouseName, true);

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

        /// <summary>
        /// /HouseName/1
        /// </summary>
        public static async Task<dynamic> FromUrl(string url)
        {
            string[] parts = url.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

            var houseNumber = int.Parse(parts[1]);
            var parsedAngle = (HouseName)houseNumber;

            return parsedAngle;
        }

        #region JSON SUPPORT

        
        /// <summary>
        /// Given a json list of person will convert to instance
        /// used for transferring between server & client
        /// </summary>
        public static List<HouseName> FromJsonList(JToken personList)
        {
            //if null empty list please
            if (personList == null) { return new List<HouseName>(); }

            var returnList = new List<HouseName>();

            foreach (var personJson in personList)
            {
                returnList.Add(HouseNameExtensions.FromJson(personJson));
            }

            return returnList;
        }

        public static JArray ToJsonList(List<HouseName> houseNameList)
        {
            var jsonList = new JArray();

            foreach (var eventTag in houseNameList)
            {
                jsonList.Add(eventTag.ToString());
            }

            return jsonList;
        }


        public static HouseName FromJson(JToken planetInput)
        {
            //if null return empty, end here
            if (planetInput == null) { return HouseName.Empty; }

            try
            {
                var valueStr = planetInput.Value<string>();
                var parsedEnum = (HouseName)Enum.Parse(typeof(HouseName), valueStr);

                return parsedEnum;
            }
            catch (Exception e)
            {
                LibLogger.Debug($"Failed to parse:\n{planetInput.ToString()}");
                return HouseName.Empty;
            }

        }

        #endregion


    }

}
