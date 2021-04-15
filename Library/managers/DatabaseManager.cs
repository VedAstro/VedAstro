using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Genso.Astrology.Library;
using Genso.Framework;

namespace Genso.Astrology.Library
{
    /// <summary>
    /// Manager to handle getting from & saving to database (XML files on disk)
    /// </summary>
    public static class DatabaseManager
    {


        /// <summary>
        /// Gets a list of all event data from database
        /// Note: element names used here corespond to the ones found in the XML file
        ///       if change here, than change in XML as well
        /// </summary>
        public static List<EventData> GetEventDataList(string filePath)
        {
            //get the event data list in a structed form xml file
            Data eventDataListFile = new Data(filePath);

            //create a place to store the list
            List<EventData> eventDataList = new List<EventData>();

            //get all the raw event data into a list
            var rawEventDataList = eventDataListFile.getAllRecords();

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
                //todo needs to be moved to a better place
                var calculatorMethod = EventManager.GetEventCalculatorMethod(name);

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

        /// <summary>
        /// Gets all event data/types that match the inputed tag
        /// </summary>
        public static List<EventData> GetEventDataListByTag(EventTag tag, string filePath)
        {
            //get all event data/types
            var eventDataList = DatabaseManager.GetEventDataList(filePath);

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
        public static List<Person> GetPersonList(string filePath)
        {
            //get the person list file
            Data personListFile = new Data(filePath);

            //create a place to store the list
            var eventDataList = new List<Person>();

            //get all the raw person data into a list
            var rawPersonList = personListFile.getAllRecords();

            //parse each raw person data in list
            foreach (var personXml in rawPersonList)
            {
                //extract the individual data out & convert it to the correct type
                var nameString = personXml.Element("Name").Value;
                var birthTime = getBirthTime(personXml.Element("BirthTime"));

                //place the data into an event data structure
                var person = new Person(nameString, birthTime);

                //add it to the return list
                eventDataList.Add(person);
            }


            //return the list to caller
            return eventDataList;

            //--------------FUNCTIONS
            //converts xml reprisentation of birth time to object instance of it
            Time getBirthTime(XElement birthTimeXml)
            {
                //extract the individual data out & convert it to the correct type
                var birthDateTimeRaw = birthTimeXml.Element("Time").Value;
                var birthDateTime = DateTimeOffset.ParseExact(birthDateTimeRaw, Time.GetDateTimeFormat(), null);

                //extract geolocation
                var locationHolder = birthTimeXml.Element("Location");
                var locationName = locationHolder.Element("Name").Value;
                var longitude = double.Parse(locationHolder.Element("Longitude").Value);
                var latitide = double.Parse(locationHolder.Element("Latitude").Value);
                var birthLocation = new GeoLocation(locationName, longitude, latitide);

                //return the extracted data
                return new Time(birthDateTime, birthLocation);
            }

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
                new GeoLocation("Teluk", 101.0901, 4.5975)
            };

            return list;
        }

    }
}