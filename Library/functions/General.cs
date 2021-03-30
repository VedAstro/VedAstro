
using System;
using System.Collections.Generic;
using System.Linq;
using Genso.Astrology.Library;
using Event = Genso.Astrology.Library.Event;
using System.Collections;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

namespace Genso.Astrology.Library
{
    public static class General
    {
        //used for canceling calculation halfway
        public static CancellationToken threadCanceler;


        /// <summary>
        /// Get list of events occurig in a time periode for all the
        /// inputed event types aka "event data"
        /// </summary>
        public static List<Event> GetListOfEventsInTimePeriod(Time startTime, Time endTime, Person person, double precisionInHours, List<EventData> eventDataList)
        {
            //initialize empty list of event to return
            List<Event> eventList = new();

            //split time into slices based on precision
            List<Time> timeList = GetTimeListFromRange(startTime, endTime, precisionInHours);

            var sync = new object();//to lock thread access to list

            //var count = 0;
            try
            {
                Parallel.ForEach(eventDataList, (eventData) =>
                {
                    //get list of occuring events for a sigle event type
                    var eventListForThisEvent = GetListOfEventsByEventData(eventData, person, timeList);
                    //TODO Progress show code WIP
                    //count++;
                    //double percentDone = ((double)count / (double)eventDataList.Count) * 100.0;
                    //debug print
                    //Console.Write($"\r Completed : {percentDone}%");

                    //adding to list needs to be synced for thread safety
                    lock (sync)
                    {
                        //add events to main list of event
                        eventList.AddRange(eventListForThisEvent);

                    }
                });

            }
            //catches only exceptions that idicates that user canceled the calculation (caller lost interest in the result)
            catch (Exception e) when (e.InnerException.GetType() == typeof(OperationCanceledException))
            {
                //return empty list
                return new List<Event>();
            }


            //return calculated event list
            return eventList;
        }

        /// <summary>
        /// Get a list of events in a time period for a single event type aka "event data"
        /// Decision on when event starts & ends is also done here
        /// </summary>
        private static List<Event> GetListOfEventsByEventData(EventData eventData, Person person, List<Time> timeList)
        {
            //declare empty event list to fill
            var eventList = new List<Event>();

            //set previous time as false for first time instance
            var eventOccuredInPreviousTime = false;

            //declare start & end times
            Time eventStartTime = new Time();
            Time eventEndTime = new Time();
            var lastInstanceOfTime = timeList.Last();

            //loop through time list 
            //note: loop must be done in sequencial order, to detect correct start & end time
            foreach (var time in timeList)
            {
                // used for canceling calculation by outside thread (user cancel halfway)
                // this will throw the appropriate exception
                threadCanceler.ThrowIfCancellationRequested();

                //debug print
                Console.Write($"\r Checking time:{time} : {eventData.GetName()}");

                //get flag of event occuring now
                var eventIsOccuringNow = eventData.IsEventOccuring(time, person);

                //if event is occuring now & not in previous time
                if (eventIsOccuringNow == true & eventOccuredInPreviousTime == false)
                {
                    //save new start & end time
                    eventStartTime = time;
                    eventEndTime = time;
                    //update flag
                    eventOccuredInPreviousTime = true;
                }
                //if event is occuring now & in previous time
                else if (eventIsOccuringNow == true & eventOccuredInPreviousTime == true)
                {
                    //update end time only
                    eventEndTime = time;
                    //update flag
                    eventOccuredInPreviousTime = true;
                }
                //if event is not occuring now but occured before
                else if (eventIsOccuringNow == false & eventOccuredInPreviousTime == true)
                {
                    //add previous event to list
                    var newEvent = new Event(eventData.GetName(),
                        eventData.GetNature(),
                        eventData.GetDescription(),
                        eventStartTime,
                        eventEndTime);

                    //if event is duration is 0 minute, raise alarm
                    //if (newEvent.GetDurationMinutes() <= 0) { throw new Exception("Event duration is 0!"); }
                    if (newEvent.GetDurationMinutes() <= 0) { Console.WriteLine("Event duration is 0!"); }


                    eventList.Add(newEvent);

                    //set flag
                    eventOccuredInPreviousTime = false;
                }

                //if event is occuring now & it is the last time
                if (eventIsOccuringNow == true & time == lastInstanceOfTime)
                {
                    //add current event to list
                    var newEvent2 = new Event(eventData.GetName(),
                        eventData.GetNature(),
                        eventData.GetDescription(),
                        eventStartTime,
                        eventEndTime);

                    //if event is duration is 0 minute, raise alarm
                    if (newEvent2.GetDurationMinutes() <= 0) { Console.WriteLine("Event duration is 0!"); }

                    eventList.Add(newEvent2);
                }
            }

            return eventList;
        }

        public static List<Time> GetTimeListFromRange(Time startTime, Time endTime, double precisionInHours)
        {
            //declare return value
            var timeList = new List<Time>();

            //create list
            for (var day = startTime; day.GetStdDateTimeOffset() <= endTime.GetStdDateTimeOffset(); day = day.AddHours(precisionInHours))
            {
                timeList.Add(day);
            }

            //return value
            return timeList;
        }

        /// <summary>
        /// Gets the method that does the caculations for an event based on the events name
        /// </summary>
        public static EventCalculator GetEventCalculatorMethod(EventName inputEventName)
        {
            //get all event calculator methods
            var eventCalculatorList = typeof(EventCalculatorMethods).GetMethods();

            //loop through all calculators
            foreach (var eventCalculator in eventCalculatorList)
            {
                //try to get attribute attached to the calculator method
                var eventCalculatorAttribute = (EventCalculatorAttribute)Attribute.GetCustomAttribute(eventCalculator,
                    typeof(EventCalculatorAttribute));

                //if attribute not found
                if (eventCalculatorAttribute == null)
                {   //go to next method
                    continue;
                }

                //if attribute name matches input event name
                if (eventCalculatorAttribute.GetEventName() == inputEventName)
                {
                    //convert calculator reference to a delegate instance
                    var eventCalculatorDelegate = (EventCalculator)Delegate.CreateDelegate(typeof(EventCalculator), eventCalculator);

                    //return calculator delegate
                    return eventCalculatorDelegate;
                }
            }


            //if calculator method not found, raise error
            throw new Exception("Calculator method not found!");

        }

        public static MuhurthaTimePeriod GetNewMuhurthaTimePeriod(DateTimeOffset startStdTime, DateTimeOffset endStdTime, GeoLocation geoLocation, Person person, double precisionInHours, List<EventData> eventDataList)
        {

            //get data to instantiate muhurtha time period
            //get start & end times
            var startTime = new Time(startStdTime, geoLocation);
            var endTime = new Time(endStdTime, geoLocation);


            //get list of events
            var eventList = GetListOfEventsInTimePeriod(startTime, endTime, person, precisionInHours, eventDataList);

            //initialize new muhurtha time period
            var muhurthaTimePeriod = new MuhurthaTimePeriod(startTime, endTime, person, eventList);

            //return 
            return muhurthaTimePeriod;
        }

        /// <summary>
        /// Splits events that span across 2 days or more
        /// </summary>
        public static List<Event> SplitEventsByDay(List<Event> events)
        {
            //clone the list
            var returnList = new List<Event>(events);

            foreach (var _event in events)
            {
                //check if event spans more than 1 day
                bool spanMore = IsEventSpanMoreThan1Day(_event);

                //if event spans more, 
                if (spanMore)
                {   //split it into pieces
                    var splittedEvents = SplitEvent(_event);
                    //remove the unsplitted event from the list
                    returnList.Remove(_event);
                    //add the new splitted events into the return list
                    returnList.AddRange(splittedEvents);
                }
            }


            //remove 0 minutes events
            //Note:during splitting 0 minute events are sometimes created it is removed here
            var filteredEvents = General.FilterOutShortEvents(returnList, 0);


            //return list to caller 
            return filteredEvents;


            //------------------------METHODS-----------------------------

            ///splits an event into pieces by day
            ///call this function, with events that are confirmed to span more than 1 day
            List<Event> SplitEvent(Event _event)
            {
                var splitedEventList = new List<Event>();

                //extract the details of the event that wont't be changed
                //and will be used to make the new splitted events
                var name = _event.GetName();
                var eventNature = _event.GetNature();
                var description = _event.GetDescription();
                var geoLocation = _event.GetStartTime().GetGeoLocation();
                var stdOffset = _event.GetStartTime().GetStdDateTimeOffset().Offset;

                var startStdTime = _event.GetStartTime().GetStdDateTimeOffset();
                var startDate = startStdTime.Day;

                var endTime = _event.GetEndTime();
                var endDate = endTime.GetStdDateTimeOffset().Day;

                //temp variables to store event data during creation
                Event newEvent;
                Time newStartTime;
                Time newEndTime;
                DateTimeOffset newStdTime;

                //so long as start date & end date does not match continue loop
                while (startDate != endDate)
                {
                    //create a new end time on the end of start day
                    newStdTime = new DateTimeOffset(
                        startStdTime.Year,
                        startStdTime.Month,
                        startStdTime.Day, 23, 59, 59, stdOffset);//set to the last minute of the day
                    newEndTime = new Time(newStdTime, geoLocation);
                    newStartTime = new Time(startStdTime, geoLocation);

                    //create a new event
                    newEvent = new Event(name, eventNature,
                        description, newStartTime, newEndTime);

                    //place the event inside the return list
                    splitedEventList.Add(newEvent);

                    //set the start time value for the next event with the end time of this one
                    startStdTime = newStdTime.AddSeconds(1);//increment by 1 second to become the next day

                    //update the start date for checking
                    startDate = startStdTime.Day;
                }

                //create the last splitted event

                //create a new start time based on what was set before
                newStartTime = new Time(startStdTime, geoLocation);

                //create a new event
                newEvent = new Event(name, eventNature,
                    description, newStartTime, endTime); //use the original end time

                //place the event inside the return list
                splitedEventList.Add(newEvent);

                //return the list to the caller
                return splitedEventList;
            }


            /// checks if an event spans more than 1 day
            /// event starts on a day & ends on another day return true
            /// Note : Event can be less then 24 hours and still span more than 1 day
            bool IsEventSpanMoreThan1Day(Event _event)
            {
                //gets the start date number of the month for the event
                var startDay = _event.GetStartTime().GetStdDateTimeOffset().Day;

                //gets the end date number of the month for the event
                var endDay = _event.GetEndTime().GetStdDateTimeOffset().Day;

                //if the date number does not match, return event spans more (true)
                if (startDay != endDay)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }


        // EXTENSION FUNCTIONS TO GET KEYS OUT OF MEMORY CACHE (USED IN ASTRONOMICAL FUNCTION CACHING)

        private static readonly Func<MemoryCache, object> GetEntriesCollection = Delegate.CreateDelegate(
            typeof(Func<MemoryCache, object>),
            typeof(MemoryCache).GetProperty("EntriesCollection", BindingFlags.NonPublic | BindingFlags.Instance).GetGetMethod(true),
            throwOnBindFailure: true) as Func<MemoryCache, object>;


        public static IEnumerable GetKeys(this IMemoryCache memoryCache) =>
            ((IDictionary)GetEntriesCollection((MemoryCache)memoryCache)).Keys;

        public static IEnumerable<T> GetKeys<T>(this IMemoryCache memoryCache) =>
            GetKeys(memoryCache).OfType<T>();

        /// <summary>
        /// Removes events that are shorter or equal minimum minutes specified
        /// </summary>
        public static List<Event> FilterOutShortEvents(List<Event> splittedEvents, int minimumMinutes)
        {
            //make a hard copy of the event list
            var returnList = new List<Event>(splittedEvents);

            //find events in list that are too short
            var foundList = from x in splittedEvents where x.GetDurationMinutes() <= minimumMinutes select x;

            //remove found events from return list
            foreach (var _event in splittedEvents)
            {
                //if this is the event to be deleted
                if (foundList.Contains(_event))
                {
                    //remove it
                    returnList.Remove(_event);
                }

            }

            //return the cleaned list to caller
            return returnList;

        }
    }
}
