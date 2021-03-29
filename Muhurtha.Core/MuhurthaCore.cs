using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Genso.Astrology.Library;

namespace Genso.Astrology.Muhurtha.Core
{
    /// <summary>
    /// The logic part of the Muhurtha program,
    /// all access to muhurtha from GUI comes here
    /// Note : GUI does not & should not call any other class but MuhurthaCore
    /// </summary>
    public static class MuhurthaCore
    {
        public static List<Person> GetAllPeopleList() => DatabaseManager.GetPersonList("data\\PersonList.xml");

        public static List<EventTag> GetAllTagList()
        {
            //get all "Event Tag" values into an array
            var array = (EventTag[])Enum.GetValues(typeof(EventTag));

            //convert to list & return to caller
            return new List<EventTag>(array);
        }

        public static List<GeoLocation> GetAllLocationList() => DatabaseManager.GetLocationList("data\\LocationList.xml");

        public static List<Event> GetEvents(string startTime, string endTime, GeoLocation location, Person person, EventTag tag)
        {
            var startStdTime = DateTimeOffset.ParseExact(startTime, Time.GetDateTimeFormat(), null);
            var endStdTime = DateTimeOffset.ParseExact(endTime, Time.GetDateTimeFormat(), null);

            //TODO NEEDS TO BE MOVED TO A BETTER PLACE
            //----------------
            //get list of event data to check for event
            var eventDataList = DatabaseManager.GetEventDataList("data\\EventDataList.xml");

            //filter IN event data list
            var filteredEventDataList = eventDataList.FindAll(eventData =>
            {
                //single tag filter
                //var filter1 = eventData.GetName() == EventName.SuryaSankramana || eventData.GetName() == EventName.Sunset || eventData.GetName() == EventName.Midday;
                //var filter1 = eventData.GetName() == EventName.Papashadvargas;
                //var filter1 = eventData.GetName().ToString().Contains("Suns");
                var filter1 = eventData.GetEventTags().Contains(tag);

                return filter1;
            });
            //-------------------

            var x = General.GetNewMuhurthaTimePeriod(startStdTime, endStdTime, location, person, TimePreset.Minute3, filteredEventDataList);

            return x.GetEventList();
        }
    }
}
