using System;
using System.Collections.Generic;

namespace Genso.Astrology.Library
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
            //get the event data list in a structed form xml file
            Data eventDataListFile = new Data(filePath);

            //create a place to store the list
            List<EventData> eventDataList = new List<EventData>();

            //get all the raw event data into a list
            var rawEventDataList = eventDataListFile.GetAllRecords();

            //parse each raw event data in list
            foreach (var eventData in rawEventDataList)
            {
                //add it to the return list
                eventDataList.Add(EventData.ToXml(eventData));
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
                var filter1 = eventData.GetEventTags().Contains(tag);

                return filter1;
            });

            return filteredEventDataList;
        }
        
        
        /// <summary>
        /// Gets a list of all persons from database
        /// Note: element names used here corespond to the ones found in the XML file
        ///       if change here, than change in XML as well
        /// </summary>
        public static List<Person> GetPersonList(Data personListFile)
        {
            //create a place to store the list
            var eventDataList = new List<Person>();

            //get all the raw person data into a list
            var rawPersonList = personListFile.GetAllRecords();

            //parse each raw person data in list
            foreach (var personXml in rawPersonList)
            {
                //add it to the return list
                eventDataList.Add(Person.FromXml(personXml));
            }


            //return the list to caller
            return eventDataList;

        }

        //overload for above method
        public static List<Person> GetPersonList(string filePath)
        {
            //get the person list file
            Data personListFile = new Data(filePath);

            return GetPersonList(personListFile);
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