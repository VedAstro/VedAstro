using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Genso.Astrology.Library;

namespace Horoscope.Desktop
{
    /// <summary>
    /// Class to encapsulate access to data files
    /// Manager to handle getting from & saving to database (XML files on disk)
    /// TODO NOTE : Another similar class exist in Genso.Astrology.Library, needs to be checked for duplication
    /// </summary>
    public static class DatabaseManager
    {


        /// <summary>
        /// Gets a list of all prediction data from EventDataList file.
        /// Note: element names used here correspond to the ones found in the XML file
        ///       if change here, than change in XML as well
        /// </summary>
        public static List<EventData> GetPredictionDataList(string filePath)
        {
            //get the event data list in a structured form xml file
            Data eventDataListFile = new Data(filePath);

            //create a place to store the list
            List<EventData> eventDataList = new List<EventData>();

            //get all the raw event data into a list
            var rawEventDataList = eventDataListFile.GetAllRecords();

            //parse each raw event data in list
            foreach (var eventDataXml in rawEventDataList)
            {
                //add it to the return list
                eventDataList.Add(EventData.FromXml(eventDataXml));
            }


            //return the list to caller
            return eventDataList;
            
        }

        /// <summary>
        /// Gets a list of all persons from database
        /// Note: element names used here correspond to the ones found in the XML file
        ///       if change here, than change in XML as well
        /// </summary>
        public static List<Person> GetPersonList(string filePath)
        {
            //get the person list file
            Data personListFile = new Data(filePath);

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
