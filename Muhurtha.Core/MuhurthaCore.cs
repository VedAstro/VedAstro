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

            //get all event data/types which has the inputed tag
            var eventDataList = DatabaseManager.GetEventDataListByTag(tag, dataEventdatalistXml);

            //send thread canceler into calculator, so that calculator can be stopped halfway if needed
            EventManager.threadCanceler = threadCanceler;

            //debug to measure event calculation time
            var watch = Stopwatch.StartNew();

            //start calculating events
            var eventList = EventManager.GetEventsInTimePeriod(startStdTime, endStdTime, location, person, TimePreset.Minute3, eventDataList);

            watch.Stop();
            LogManager.Debug($"Events computed in: { watch.Elapsed.TotalSeconds}s");

            //fire event to let others know event calculation is done
            EventCalculationCompleted.Invoke();

            return eventList;
        }

        /// <summary>
        /// Finds a time when a few events occur at the same time.
        /// </summary>
        public static List<Event> FindCombinedEvents(string startTime, string endTime, GeoLocation location, Person person, List<EventData> eventsToFind)
        {
            //todo move it into Time object
            var startStdTime = DateTimeOffset.ParseExact(startTime, Time.GetDateTimeFormat(), null);
            var endStdTime = DateTimeOffset.ParseExact(endTime, Time.GetDateTimeFormat(), null);

            //debug to measure event calculation time
            var watch = Stopwatch.StartNew();

            //start calculating events
            var eventList = EventManager.FindCombinedEventsInTimePeriod(startStdTime, endStdTime, location, person, TimePreset.Minute3, eventsToFind);

            watch.Stop();
            LogManager.Debug($"Events found in: { watch.Elapsed.TotalSeconds}s");

            //fire event to let others know event calculation is done
            EventCalculationCompleted.Invoke();

            return eventList;
        }


        public static void SendEventsToCalendar(List<Event> events, Calendar calendarName, CalendarAccount google, bool splitEvents, bool enableReminders, string customEventName)
        {
            //split events by day if checked
            if (splitEvents) { events = EventManager.SplitEventsByDay(events); }

            //pass thread canceler, so that sending can be stopped if needed
            CalendarManager.threadCanceler = threadCanceler;

            //start sending
            CalendarManager.AddEventsToCalenderGoogle(events, calendarName.Id, enableReminders, customEventName);

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
