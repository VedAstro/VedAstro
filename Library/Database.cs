using System;
using System.Collections.Generic;
using Genso.Astrology.Library;
using Genso.Framework;

namespace Genso.Astrology.Library
{
    /// <summary>
    /// Class to handle DB logic, database is the XML file
    /// </summary>
    public static class Database
    {
        //get the event data list in a structed form xml file
        private static Data EventDataList = new Data("data\\EventDataList.xml");


        /// <summary>
        /// Gets a list of all event data from XML file
        /// </summary>
        public static List<EventData> GetEventDataListFromDatabase()
        {
            //create a place to store the list
            List<EventData> eventDataList = new List<EventData>();

            //get all the raw event data into a list
            var rawEventDataList = EventDataList.getAllRecords();

            //parse each raw event data in list
            foreach (var eventData in rawEventDataList)
            {
                //extract the individual data out & convert it to the correct type
                var id = Int32.Parse(eventData.Element("Id").Value);
                var nameString = eventData.Element("Name").Value;
                Enum.TryParse(nameString, out EventName name);
                var natureString = eventData.Element("Nature").Value;
                Enum.TryParse(natureString, out EventNature nature);
                var description = eventData.Element("Description").Value;
                var tagString = eventData.Element("Tag").Value;
                var tagList = getEventTags(tagString);
                var calculatorMethod = General.GetEventCalculatorMethod(name);

                //place the data into an event data structure
                var eventX = new EventData(id, name, nature, description, tagList, calculatorMethod);

                //add it to the return list
                eventDataList.Add(eventX);
            }


            //return the list to caller
            return eventDataList;

            //Gets a list of tags in string form & changes it a structed list of tags
            List<EventTag> getEventTags(string rawTags)
            {
                //create a place to store the parsed tags
                var returnTags = new List<EventTag>();

                //split the string by comma "," (tag seperator)
                var splittedRawTags = rawTags.Split(',');

                //parse each raw tag
                foreach (var rawTag in splittedRawTags)
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

        }
    }
}