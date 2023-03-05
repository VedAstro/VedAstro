
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Reflection;

namespace Genso.Astrology.Library
{
    /// <summary>
    /// All logic dealing with creating events is here
    /// </summary>
    public static class EventManager
    {
        /** FIELDS **/

        //used for canceling calculation halfway
        public static CancellationToken threadCanceler;



        /** PUBLIC METHODS **/

        /// <summary>
        /// Get list of events occuring in a time period for all the
        /// inputed event types aka "event data"
        /// Note : Cancellation token caught here
        /// </summary>
        public static List<Event> GetEventsInTimePeriod(DateTimeOffset startStdTime, DateTimeOffset endStdTime, GeoLocation geoLocation, Person person, double precisionInHours, List<EventData> eventDataList)
        {

            //get data to instantiate muhurtha time period
            //get start & end times
            var startTime = new Time(startStdTime, geoLocation);
            var endTime = new Time(endStdTime, geoLocation);


            //initialize empty list of event to return
            List<Event> eventList = new();

            //split time into slices based on precision
            var timeList = GetTimeListFromRange(startTime, endTime, precisionInHours);

            var sync = new object();//to lock thread access to list

            //var count = 0;
            try
            {
                Parallel.ForEach(eventDataList, (eventData) =>
                {
                    //get list of occuring events for a single event type
                    var eventListForThisEvent = GetListOfEventsByEventDataParallel(eventData, person, timeList);
                    //TODO Progress show code WIP
                    //count++;
                    //double percentDone = ((double)count / (double)eventDataList.Count) * 100.0;
                    //debug print
                    //LogManager.Debug($"\r Completed : {percentDone}%");

                    //adding to list needs to be synced for thread safety
                    lock (sync)
                    {
                        //add events to main list of event
                        eventList.AddRange(eventListForThisEvent);

                    }
                });

            }
            //catches only exceptions that idicates that user canceled the calculation (caller lost interest in the result)
            //since the error is not thrown here, we use InnerException
            catch (Exception e) when (e.InnerException.GetType() == typeof(OperationCanceledException))
            {
                //log it
                LogManager.Debug("User canceled event calculation halfway!");
                //return empty list
                return new List<Event>();
            }


            //return calculated event list
            return eventList;
        }

        /// <summary>
        /// Note : the list of events to find is treated as 1 event here
        /// </summary>
        public static List<Event> FindCombinedEventsInTimePeriod(DateTimeOffset startStdTime, DateTimeOffset endStdTime, GeoLocation geoLocation, Person person, double precisionInHours, List<EventData> eventDataList)
        {

            //get data to instantiate
            //get start & end times
            var startTime = new Time(startStdTime, geoLocation);
            var endTime = new Time(endStdTime, geoLocation);


            //split time into slices based on precision
            List<Time> timeList = GetTimeListFromRange(startTime, endTime, precisionInHours);

            //takes time slices and check if all the events is occurig for each time slice,
            //the list of events is treated as 1 event here 
            var precalculatedList = GetCalculatedTimeEventOccuringList(eventDataList);

            //create a new combined event data type (used when extracting events)
            var combinedEvent = CreateNewEventData(eventDataList);


            //go through time slices and identify start & end of events
            //and place them in a event list
            var eventList = ExtractEventsFromTimeSlices(timeList, precalculatedList, combinedEvent);

            return eventList;



            //----------------------FUNCTIONS--------------------------

            //takes time list and checks if event is occurig for each time slice,
            //and returns it in dictionary list, done in parallel
            ConcurrentDictionary<Time, bool> GetCalculatedTimeEventOccuringList(List<EventData> combinedEvents)
            {
                //time is "key", and "is occuring" is "value"
                var returnList = new ConcurrentDictionary<Time, bool>();

                Parallel.ForEach(timeList, (time, state) =>
                {
                    //check if all events are occuring in this time slice
                    var isEventOccuring = isAllEventsOccuring(time, person, combinedEvents);

                    //add to the dictionary
                    returnList[time] = isEventOccuring;

                });

                return returnList;
            }

            //check if all events are occuring for a time slice
            bool isAllEventsOccuring(Time timeSlice, Person person, List<EventData> combinedEvents)
            {
                //makes sure all the events are occuring for this time slice
                //else false is returned even if 1 event is not occuring
                foreach (var _event in combinedEvents)
                {
                    var isEventOccuring = _event.IsEventOccuring(timeSlice, person);
                    if (isEventOccuring == false) { return false; }
                }

                //if control reaches here, then all events are occuring
                return true;
            }

            //creates a new event data type for the particular custom search
            EventData CreateNewEventData(List<EventData> eventDatas)
            {
                //if only looking for 1 event, then no need to create a new merged event
                //just use that event data
                if (eventDatas.Count == 1) { return eventDatas[0]; }

                //but if more than 1, create a merged/combined type
                var eventName = EventName.CombinedEvent;
                var description = "Combined event of:\n";
                eventDatas.ForEach(eventData => description += $"-{eventData.Name}\n"); //name of each event is added to description
                var combinedEvent =
                    new EventData(eventName, EventNature.Neutral, description, null,
                        null); //no need tag & calculator since only used visualy
                return combinedEvent;
            }
        }

        /// <summary>
        /// Get a list of events in a time period for a single event type aka "event data"
        /// Decision on when event starts & ends is also done here
        /// Note :
        /// 1.thread cancellation checked & thrown here (caught by caller)
        /// 2.method is operated in parallel threaded way (inside) for performance gains
        /// </summary>
        private static List<Event> GetListOfEventsByEventDataParallel(EventData eventData, Person person, List<Time> timeList)
        {
            //caculate if event occured for each time slice and store it in a dictionary
            var precalculatedList = GetCalculatedTimeEventOccuringList();

            //go through time slices and identify start & end of events
            //place them in a event list
            var eventList = ExtractEventsFromTimeSlices(timeList, precalculatedList, eventData);

            return eventList;


            //----------------------LOCAL FUNCTIONS--------------------------

            //takes time list and checks if event is occurig for each time slice,
            //and returns it in dictionary list, done in parallel
            ConcurrentDictionary<Time, bool> GetCalculatedTimeEventOccuringList()
            {
                //time is "key", and is occuring is "value"
                var returnList = new ConcurrentDictionary<Time, bool>();

                Parallel.ForEach(timeList, (time, state) =>
                {
                    //get if event occuring at the inputed time
                    var isEventOccuring = eventData.IsEventOccuring(time, person);

                    //add to the dictionary
                    returnList[time] = isEventOccuring;

                });

                return returnList;
            }

        }

        //go through time slices and identify start & end of events
        //place them in a event list and return it
        private static List<Event> ExtractEventsFromTimeSlices(List<Time> timeList, ConcurrentDictionary<Time, bool> precalculatedList, EventData eventData)
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
                //check if user has canceled calculation halfway
                threadCanceler.ThrowIfCancellationRequested();

                //check if event is occuring at this time slice (precalculated list)
                var eventIsOccuringNow = precalculatedList[time];

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
                    var newEvent = new Event(eventData.Name,
                        eventData.Nature,
                        eventData.Description,
                        eventStartTime,
                        eventEndTime);

                    //if event is duration is 0 minute, raise alarm
                    if (newEvent.GetDurationMinutes() <= 0)
                    {
                        LogManager.Debug("Event duration is 0!");
                    }


                    eventList.Add(newEvent);

                    //set flag
                    eventOccuredInPreviousTime = false;
                }

                //if event is occuring now & it is the last time
                if (eventIsOccuringNow == true & time == lastInstanceOfTime)
                {
                    //add current event to list
                    var newEvent2 = new Event(eventData.Name,
                        eventData.Nature,
                        eventData.Description,
                        eventStartTime,
                        eventEndTime);

                    //if event is duration is 0 minute, raise alarm
                    if (newEvent2.GetDurationMinutes() <= 0)
                    {
                        LogManager.Debug("Event duration is 0!");
                    }

                    eventList.Add(newEvent2);
                }
            }

            return eventList;
        }


        /// <summary>
        /// MARKED FOR ARHIVEING, if not used long archive it
        /// Get a list of events in a time period for a single event type aka "event data"
        /// Decision on when event starts & ends is also done here
        /// Note :
        /// 1.thread cancelation checked & thrown here (caught by caller)
        /// 2.method is operated in sequencial way, it is anloder version, marked for archiving!
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
                //check if user has canceled calculation halfway
                threadCanceler.ThrowIfCancellationRequested();

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
                    var newEvent = new Event(eventData.Name,
                        eventData.Nature,
                        eventData.Description,
                        eventStartTime,
                        eventEndTime);

                    //if event is duration is 0 minute, raise alarm
                    if (newEvent.GetDurationMinutes() <= 0) { LogManager.Debug("Event duration is 0!"); }


                    eventList.Add(newEvent);

                    //set flag
                    eventOccuredInPreviousTime = false;
                }

                //if event is occuring now & it is the last time
                if (eventIsOccuringNow == true & time == lastInstanceOfTime)
                {
                    //add current event to list
                    var newEvent2 = new Event(eventData.Name, eventData.Nature, eventData.Description,
                        eventStartTime,
                        eventEndTime);

                    //if event is duration is 0 minute, raise alarm
                    if (newEvent2.GetDurationMinutes() <= 0) { LogManager.Debug("Event duration is 0!"); }

                    eventList.Add(newEvent2);
                }
            }

            return eventList;
        }

        /// <summary>
        /// Slices time range into pieces by inputed hours
        /// Given a start time and end time, it will add precision hours to start time until reaching end time.
        /// Note: number of slices returned != precision hours
        /// </summary>
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
        public static EventCalculatorDelegate GetEventCalculatorMethod(EventName inputEventName)
        {
            //get all event calculator methods
            var eventCalculatorList = typeof(EventCalculatorMethods).GetMethods();

            EventCalculatorDelegate emptyCalculator = null;

            //loop through all calculators
            foreach (var eventCalculator in eventCalculatorList)
            {

                //try to get attribute attached to the calculator method
                var eventCalculatorAttribute = (EventCalculatorAttribute)Attribute.GetCustomAttribute(eventCalculator, typeof(EventCalculatorAttribute));

                //if attribute not found, go to next method (private function)
                if (eventCalculatorAttribute == null) { continue; }

                //store empty event to be used if error
                if (eventCalculatorAttribute.GetEventName() == EventName.EmptyEvent)
                {
                    emptyCalculator = (EventCalculatorDelegate)Delegate.CreateDelegate(typeof(EventCalculatorDelegate), eventCalculator);
                }

                //if attribute name matches input event name
                if (eventCalculatorAttribute.GetEventName() == inputEventName)
                {
                    //convert calculator reference to a delegate instance
                    var eventCalculatorDelegate = (EventCalculatorDelegate)Delegate.CreateDelegate(typeof(EventCalculatorDelegate), eventCalculator);

                    //return calculator delegate
                    return eventCalculatorDelegate;
                }
            }


            //if calculator method not found,
            //to make old code run with updated eventdatalist.xml file, an empty calculator return no event is attached as default
            return emptyCalculator;
            //todo log if this happens hack
            throw new Exception("Calculator method not found!");

        }

        private static bool Predicate(MethodInfo arg)
        {
            var eventCalculatorAttribute = (EventCalculatorAttribute)Attribute.GetCustomAttribute(arg,
                typeof(EventCalculatorAttribute));

            var eventName = eventCalculatorAttribute.GetEventName();

            return eventName == EventName.EmptyEvent;

        }

        /// <summary>
        /// Gets the method that does the caculations for an event based on the events name
        /// </summary>
        public static HoroscopeCalculatorDelegate GetHoroscopeCalculatorMethod(EventName inputEventName)
        {
            //get all event calculator methods
            var horoscopeCalculatorList = typeof(HoroscopeCalculatorMethods).GetMethods();

            //loop through all calculators
            foreach (var horoscopeCalculator in horoscopeCalculatorList)
            {
                //try to get attribute attached to the calculator method
                var horoscopeCalculatorAttribute = (EventCalculatorAttribute)Attribute.GetCustomAttribute(horoscopeCalculator,
                    typeof(EventCalculatorAttribute));

                //if attribute not found
                if (horoscopeCalculatorAttribute == null)
                {   //go to next method
                    continue;
                }

                //if attribute name matches input event name
                if (horoscopeCalculatorAttribute.GetEventName() == inputEventName)
                {
                    //convert calculator reference to a delegate instance
                    var horoscopeCalculatorDelegate = (HoroscopeCalculatorDelegate)Delegate.CreateDelegate(typeof(HoroscopeCalculatorDelegate), horoscopeCalculator);

                    //return calculator delegate
                    return horoscopeCalculatorDelegate;
                }
            }


            //if calculator method not found, raise error
            throw new Exception("Calculator method not found!");

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
            var filteredEvents = EventManager.FilterOutShortEvents(returnList, 0);


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


//ARCHIVE CODE
//public static MuhurthaTimePeriod GetNewMuhurthaTimePeriod(DateTimeOffset startStdTime, DateTimeOffset endStdTime, GeoLocation geoLocation, Person person, double precisionInHours, List<EventData> eventDataList)
//{



//    //get list of events
//    var eventList = GetEventsInTimePeriod(startTime, endTime, person, precisionInHours, eventDataList);

//    //initialize new muhurtha time period
//    var muhurthaTimePeriod = new MuhurthaTimePeriod(startTime, endTime, person, eventList);

//    //return 
//    return muhurthaTimePeriod;
//}