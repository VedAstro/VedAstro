using System;
using System.Collections.Generic;
using System.IO;

namespace VedAstro.Library
{
    /// <summary>
    /// Manager to handle getting from & saving to database (XML files on disk)
    /// TODO NOTE : Another similar class exist in Horoscope.Desktop, needs to be checked for duplication
    /// </summary>
    public static class DatabaseManager
    {
        /// <summary>
        /// Gets a list of all event data from database
        /// Note: element names used here correspond to the ones found in the XML file
        ///       if change here, than change in XML as well
        /// </summary>
        public static List<EventData> GetEventDataList(string filePath)
        {
            //get the event data list in a structured form xml file
            Data eventDataListFile = new(filePath);

            //create a place to store the list
            var eventDataList = new List<EventData>();

            //get all the raw event data into a list
            var rawEventDataList = eventDataListFile.GetAllRecords();

            //parse each raw event data in list
            foreach (var eventData in rawEventDataList)
            {
                //add it to the return list
                eventDataList.Add(EventData.FromXml(eventData));
            }


            //return the list to caller
            return eventDataList;

        }

        /// <summary>
        /// Converts XML file from stream to List
        /// </summary>
        public static List<EventData> GetEventDataList(Stream eventDataListFileStream)
        {
            //get the event data list in a structured form xml file
            Data eventDataList = new Data(eventDataListFileStream);

            return GetEventDataList(eventDataList);
        }
        /// <summary>
        /// Gets a list of all prediction data from EventDataList file.
        /// Note: element names used here correspond to the ones found in the XML file
        ///       if change here, than change in XML as well
        /// </summary>
        public static List<EventData> GetEventDataList(Data eventDataListFile)
        {
            //create a place to store the list
            List<EventData> eventDataList = new List<EventData>();

            //get all the raw event data into a list
            var rawEventDataList = eventDataListFile.GetAllRecords();

            //parse each raw event data in list
            foreach (var eventData in rawEventDataList)
            {
                
                //add it to the return list
                eventDataList.Add(EventData.FromXml(eventData));
            }


            //return the list to caller
            return eventDataList;

        }

        /// <summary>
        /// Gets all event data/types that match the inputed tag
        /// </summary>
        public static List<EventData> GetEventDataListByTag(EventTag tag, string filePath)
        {
            //get all event data/types
            var eventDataList = DatabaseManager.GetEventDataList(filePath);

            return GetEventDataListByTag(tag, eventDataList);

        }

        /// <summary>
        /// Gets all event data/types that match the inputed tag
        /// </summary>
        public static List<EventData> GetEventDataListByTag(EventTag tag, List<EventData> eventDataList)
        {
            //get all event data/types
            //var eventDataList = DatabaseManager.GetEventDataList(filePath);

            //filter IN event data list by tag
            var filteredEventDataList = eventDataList.FindAll(eventData =>
            {
                //single tag filter
                //var filter1 = eventData.GetName() == EventName.SuryaSankramana || eventData.GetName() == EventName.Sunset || eventData.GetName() == EventName.Midday;
                //var filter1 = eventData.GetName() == EventName.Papashadvargas;
                //var filter1 = eventData.GetName().ToString().Contains("Suns");
                var filter1 = eventData.EventTags.Contains(tag);

                return filter1;
            });

            return filteredEventDataList;
        }


        //DEMO METHOD
        public static void SavePersonList(List<Person> personList, string filePath)
        {
            throw new NotImplementedException();
        }

        public static List<GeoLocation> GetLocationList(string dataLocationlistXml)
        {
            //todo dummy location list needs proper location list
            var list = new List<GeoLocation>()
            {
                new GeoLocation("Ipoh", 101.0901, 4.5975),
                new GeoLocation("Kuala", 101.0901, 4.5975),
                new GeoLocation("Teluk", 101.0901, 4.5975),
                new GeoLocation("Mangaluru", 74.8625, 12.9172)

            };

            return list;
        }

    }
}