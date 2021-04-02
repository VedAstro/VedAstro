using System;
using System.Collections.Generic;
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



        /** EVENTS **/
        //fired when event calculator completes
        public static event Action EventCalculationCompleted;
        public static event Action SendingEventsCompleted;



        /** PUBLIC METHODS **/

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

            //pass thread canceler General, so that methods inside can be stopped if needed
            General.threadCanceler = threadCanceler;

            //start calculating events
            var muhurthaTimePeriod = General.GetNewMuhurthaTimePeriod(startStdTime, endStdTime, location, person, TimePreset.Minute3, filteredEventDataList);

            //fire event to let others know event calculation is done
            EventCalculationCompleted.Invoke();

            return muhurthaTimePeriod.GetEventList();
        }

        public static void SendEventsToCalendar(List<Event> events, Calendar calendarName, CalendarAccount google, bool splitEvents)
        {
            //split events by day
            var splittedEvents = General.SplitEventsByDay(events);

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
    }
}
