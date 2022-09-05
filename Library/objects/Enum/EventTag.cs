

using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Genso.Astrology.Library
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
        Agriculture,
        General,
        Personal,
        RulingConstellation,
        HairNailCutting,
        Medical,
        Debug,
        Marriage,
        Astronomical,
        BuyingSelling,
        Gochara,
        Dasa,
        Horoscope,
        Building,
        Bhukti,
        Antaram,
        DasaSpecialRules,
        Tarabala,
        Chandrabala,
        Travel
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
        /// Note root element is "EventTag"
        /// </summary>
        public static XElement ToXml(this EventTag _eventTag)
        {
            var holder = new XElement("EventTag", _eventTag.ToString());

            return holder;
        }



    }
}