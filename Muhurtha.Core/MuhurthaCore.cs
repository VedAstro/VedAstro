using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Genso.Astrology.Library;
using Genso.Astrology.Library.objects.Enum;
using Genso.Framework;

namespace Genso.Astrology.Muhurtha.Core
{
    /// <summary>
    /// The logic part of the Muhurtha program,
    /// all access to muhurtha from GUI comes here
    /// Note : GUI does not & should not call any other class but MuhurthaCore
    /// </summary>
    public static class MuhurthaCore
    {
        /** FIELDS **/

        //used for canceling calculation halfway
        public static CancellationToken threadCanceler;


        /** CONST FIELDS **/

        private const string dataPersonlistXml = "data\\PersonList.xml";
        private const string dataLocationlistXml = "data\\LocationList.xml";
        private const string dataEventdatalistXml = "data\\EventDataList.xml";



        /** EVENTS **/
        //fired when event calculator completes
        public static event Action EventCalculationCompleted;
        public static event Action SendingEventsCompleted;



        /** PUBLIC METHODS **/

        public static List<Person> GetAllPeopleList()
        {
            return DatabaseManager.GetPersonList(dataPersonlistXml);
        }

        public static List<EventTag> GetAllTagList()
        {
            //get all "Event Tag" values into an array
            var array = (EventTag[])Enum.GetValues(typeof(EventTag));

            //convert to list & return to caller
            return new List<EventTag>(array);
        }

        public static List<GeoLocation> GetAllLocationList() => DatabaseManager.GetLocationList(dataLocationlistXml);

        public static List<Event> GetEvents(string startTime, string endTime, GeoLocation location, Person person, EventTag tag)
        {
            var startStdTime = DateTimeOffset.ParseExact(startTime, Time.GetDateTimeFormat(), null);
            var endStdTime = DateTimeOffset.ParseExact(endTime, Time.GetDateTimeFormat(), null);

            //TODO NEEDS TO BE MOVED TO A BETTER PLACE
            //----------------
            //get list of event data to check for event
            var eventDataList = DatabaseManager.GetEventDataList(dataEventdatalistXml);

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

            //pass thread canceler General, so that methods inside can be stopped if needed
            EventManager.threadCanceler = threadCanceler;

            //debug measure execution time
            var watch = Stopwatch.StartNew();

            //start calculating events
            var eventList = EventManager.GetEventsInTimePeriod(startStdTime, endStdTime, location, person, TimePreset.Minute3, filteredEventDataList);

            watch.Stop();
            LogManager.Debug($"Events computed in: { watch.Elapsed.TotalSeconds}s");

            //fire event to let others know event calculation is done
            EventCalculationCompleted.Invoke();

            return eventList;
        }

        public static void SendEventsToCalendar(List<Event> events, Calendar calendarName, CalendarAccount google, bool splitEvents)
        {
            //split events by day
            var splittedEvents = EventManager.SplitEventsByDay(events);

            //pass thread canceler, so that sending can be stopped if needed
            CalendarManager.threadCanceler = threadCanceler;

            //start sending
            CalendarManager.AddEventsToCalenderGoogle(splittedEvents, calendarName.Id);

            //fire event to let others know event sending is done
            SendingEventsCompleted.Invoke();

        }

        public static List<Calendar> GetCalendarsForAccount(CalendarAccount selectedAccount)
        {
            //create empty calendar list
            var calendarList = new List<Calendar>();

            //based on calendar account use the right calendar list getter
            switch (selectedAccount)
            {
                case CalendarAccount.Google:
                    calendarList = CalendarManager.GetCalendarListGoogle();
                    break;
                case CalendarAccount.iCloud:
                    LogManager.Error("iCloud not supported atm!");
                    break;
                case CalendarAccount.Outlook:
                    LogManager.Error("Outlook not supported atm!");
                    break;

            }

            //return possibly filed list to caller
            return calendarList;
        }

        public static List<CalendarAccount> GetAllCalendarAccounts()
        {
            //get all "CalendarAccount" values into an array
            var array = (CalendarAccount[])Enum.GetValues(typeof(CalendarAccount));

            //convert to list & return to caller
            return new List<CalendarAccount>(array);

        }

        public static List<EventData> GetAllEventDataList() => DatabaseManager.GetEventDataList(dataEventdatalistXml);
    }
}
