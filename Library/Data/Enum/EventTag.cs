using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace VedAstro.Library
{
    /// <summary>
    /// List of Tags added to event data for categorizing, calendar marking & etc.
    /// Note :
    /// -This is the main list, all add & remove is done here
    /// -Edits here also reflects in GUI
    /// -Multiple tags can be used by 1 event, separated by comma in in the Tag element
    /// </summary>
    public enum EventTag
    {
        Empty = 0,
        //todo made visible to public via api
        Agriculture,
        General,
        Personal,
        Yoga, //todo not yet implemented 
        RulingConstellation,
        HairNailCutting,
        Medical,
        Debug,
        Marriage,
        Astronomical,
        BuyingSelling,
        Building,
        Studies,
        Horoscope,
        Tarabala,
        Chandrabala,
        Travel,
        RisingSign, //used in birth time finder todo might not be needed
        GocharaSummary, //todo not yet implemented 

        AshtakvargaGochara, //gochara based on bindu
        Gochara,

        //DASA STUFF
        Dasa, //main category include below sub category todo marked for deletion replaced by PD...

        /// <summary>
        /// Dasa, main planet period
        /// </summary>
        PD1,
        PD2,
        PD3,
        PD4,
        PD5,
        PD6,
        PD7,
        PD8,

        DasaSpecialRules,
        Upagraha,
        PanchaPakshi,
        YamaNumber, //special to verify underlying astro events for pancha pakshi
        GhatakaChakra,

    }

    public static class EventTagExtensions
    {
        /// <summary>
        /// Note: Root element must be named EventTag
        /// </summary>
        public static EventTag FromXml(XElement eventTagXml)
        {
            //converts string to enum instance
            Enum.TryParse(eventTagXml.Value, out EventTag eventTag);

            return eventTag;
        }

        public static EventTag FromJson(JToken eventTagJson)
        {
            //converts string to enum instance
            Enum.TryParse(eventTagJson.Value<string>(), out EventTag eventTag);

            return eventTag;
        }

        /// <summary>
        /// Note: Root element must be named EventTagList
        /// </summary>
        public static List<EventTag> FromXmlList(XElement eventTagListXml)
        {
            var returnList = new List<EventTag>();
            foreach (var eventTagXml in eventTagListXml.Elements())
            {
                returnList.Add(EventTagExtensions.FromXml(eventTagXml));
            }
            return returnList;
        }

        public static List<EventTag> FromJsonList(JToken eventTagJsonList)
        {
            var returnList = new List<EventTag>();
            foreach (var eventTagJson in eventTagJsonList)
            {
                returnList.Add(EventTagExtensions.FromJson(eventTagJson));
            }
            return returnList;
        }

        /// <summary>
        /// Note: Root element must be named EventTagList
        /// </summary>
        public static XElement ToXmlList(List<EventTag> eventTagList)
        {
            var eventTagListXml = new XElement("EventTagList");

            foreach (var eventTag in eventTagList)
            {
                eventTagListXml.Add(eventTag.ToXml());
            }

            return eventTagListXml;
        }

        /// <summary>
        /// Note: Root element must be named EventTagList
        /// </summary>
        public static JArray ToJsonList(List<EventTag> eventTagList)
        {
            var jsonList = new JArray();


            foreach (var eventTag in eventTagList)
            {
                jsonList.Add(eventTag.ToString());
            }

            return jsonList;
        }

        /// <summary>
        /// Note root element is "EventTag"
        /// </summary>
        public static XElement ToXml(this EventTag _eventTag)
        {
            var holder = new XElement("EventTag", _eventTag.ToString());

            return holder;
        }
        public static JObject ToJson(this EventTag _eventTag)
        {
            var holder = new JObject(_eventTag.ToString());

            return holder;
        }

        /// <summary>
        /// Get all in a list for looping
        /// </summary>
        public static List<EventTag> GetAll()
        {
            List<EventTag> enumList = Enum.GetValues(typeof(EventTag)).Cast<EventTag>().ToList();
            return enumList;
        }


        /// <summary>
        /// Given a string separated by comma, parse to list
        /// EXP : PD1,PD2,PD3,PD4,PD5,AshtakvargaGochara,Gochara
        /// </summary>
        public static List<EventTag> FromString(string eventTagString)
        {
            List<EventTag> enumList = eventTagString.Split(',')
                .Select(s => (EventTag)Enum.Parse(typeof(EventTag), s))
                .ToList();

            return enumList;
        }
    }
}