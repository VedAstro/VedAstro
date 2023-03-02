
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;
using System.Timers;

namespace Genso.Astrology.Library
{
    /// <summary>
    /// All logic dealing with creating events is here
    /// NOTE: Parallel already built in
    /// </summary>
    public static class EventManager
    {
        /** FIELDS **/


        /// <summary>
        /// Placed here to reduce overhead when being accessed by different methods in this class
        /// </summary>
        private static List<Event> EventList { get; set; } = new List<Event>();
        private static List<EventData> EventDataList { get; set; }

        //we use direct storage URL for fast access & solid
        private const string AzureStorage = "vedastrowebsitestorage.z5.web.core.windows.net";

        //used in muhurtha, dasa, etc... events
        private const string UrlEventDataListXml = $"https://{AzureStorage}/data/EventDataList.xml";


        /// <summary>
        /// Every time events is being generated ((data+calculator)+time=event),
        /// the count goes up, auto reset on new task 
        /// </summary>
        public static int EventsGeneratorProgressCounter { get; set; }



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

            EventsGeneratorProgressCounter = 0;// progress counter reset

            foreach (var eventData in eventDataList)
            {
                //get list of occuring events for a single event type
                var eventListForThisEvent = GetListOfEventsForEventData(eventData, person, timeList);

                //increment counters, for anybody listening for progress
                EventsGeneratorProgressCounter++;

                //add events to main list of event
                eventList.AddRange(eventListForThisEvent);
            }

            //return calculated event list
            return eventList;


            /// <summary>
            /// Get a list of events in a time period for a single event type aka "event data"
            /// timelist 
            /// Note :
            /// 2.method is operated in parallel threaded way (inside) for performance gains
            /// </summary>
            List<Event> GetListOfEventsForEventData(EventData eventData, Person person, List<Time> timeList)
            {
                //calculate if event occurred for each time slice and store it in a dictionary
                //NOTE:
                var eventSlices = ConvertToEventSlice(timeList, eventData, person);

                //go through time slices and identify start & end of events
                //place them in a event list
                var eventList = ExtractEventsFromTimeSlices(timeList, eventSlices);

                return eventList;



            }

        }

        //takes time list and checks if event is occuring for each time slice,
        //and returns it in dictionary list, done in parallel
        public static List<EventSlice> ConvertToEventSlice(List<Time> timeList, EventData eventData,
            Person person)
        {
            //event data list is recreated because the data
            //inside is possibly changed when calculators are run (dynamic yeah!)
            //var returnList = new ConcurrentBag<EventSlice>();
            EventSlice[] returnList = new EventSlice[timeList.Count];

            //PARALLEL CODE
            //Parallel.ForEach(timeList, (time, state) =>
            //{
            //    //get if event occuring at the inputed time (heavy computation)
            //    var updatedEventData = ConvertToEventSlice(time, _eventData, _person);

            //    //only add if occuring
            //    if (updatedEventData != null) { returnList.Add((EventSlice)updatedEventData); }

            //});

            Parallel.For(0, timeList.Count, i =>
            {
                var timeTemp = timeList[i];
                //get if event occuring at the inputed time (heavy computation)
                var updatedEventData = ConvertToEventSlice(timeTemp, eventData, person);

                //note: fills null if not occuring
                returnList[i] = updatedEventData;
            });

            return returnList.ToList();
        }


        /// <summary>
        /// Does the heavy computation to know if event is occuring.
        /// Note also same reason this is run separately only when needed & not run during startup
        /// and it also allows the code to be managed easily for parallel computation
        /// combines event data and calculator result 
        /// </summary>
        public static EventSlice? ConvertToEventSlice(Time time, EventData _eventData, Person person)
        {
            //do calculation for this event to get prediction data
            var predictionData = _eventData.EventCalculator(time, person);

            if (predictionData.Occuring)
            {
                //extract the data out
                //store planets, houses & signs related to result
                //todo related body data needs to pumped into slice
                var RelatedBody = predictionData.RelatedBody;

                //override event nature from xml if specified
                var nature = predictionData.NatureOverride == EventNature.Empty ? _eventData.Nature : predictionData.NatureOverride;

                //override description if specified
                var isDescriptionOverride = !string.IsNullOrEmpty(predictionData.DescriptionOverride); //if true override, else false
                var description = isDescriptionOverride ? predictionData.DescriptionOverride : _eventData.Description;

                if (nature == EventNature.Empty || string.IsNullOrEmpty(description))
                {
                    throw new Exception("Something wrong");
                }

                //send caller the updated event data, since override possibility
                return new EventSlice(_eventData.Name, nature, description, time, predictionData.Occuring);

            }

            return null;
        }

        private static List<Event> ExtractEventsFromTimeSlices(List<Time> timeList, List<EventSlice> eventSliceList)
        {
            //before becoming events

            //Makes the events as it scans them
            var returnList = new List<Event>();
            var startI = 0;
            var endI = 0;
            var inBetween = false; //in between start & end event, start as though null before
            var lastIndex = eventSliceList.Count;
            for (int i = 0; i < lastIndex; i++)
            {
                //if null move to next
                var val = eventSliceList[i];
                var isNull = val == null;
                if (isNull)
                {
                    //marks the end of the event, make it & save it
                    if (inBetween)
                    {
                        inBetween = false;//halt
                        var previousSliceI = --i;//last slice in event

                        //create the event
                        var startSlice = eventSliceList[startI];
                        var startTime = startSlice.Time;
                        var endTime = eventSliceList[previousSliceI].Time;//aka endI
                        
                        //note: only details from start slice is used, meaning if other slices have
                        //different nature it is ignored, and should not have it either
                        var newEvent = new Event(startSlice.Name, startSlice.Nature, startSlice.Description, startTime, endTime);
                        returnList.Add(newEvent); //add to list
                    }

                    //nothing to see go to next
                    continue;
                }

                //if new event after null, set start
                if (!isNull && !inBetween) { startI = i; inBetween = true; }


            }

            return returnList;

        }

        //go through time slices and identify start & end of events
        //place them in a event list and return it
        //new feature feb 2023 : allows each event data to be modified by calculator, so 1 event name can
        //have different descriptions or natures based on time & person (truly dynamic) 
        private static List<Event> ExtractEventsFromTimeSlicesOld(List<Time> timeList, List<EventSlice> eventSliceList)
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
            //note: loop must be done in sequential order, to detect correct start & end time
            foreach (var time in timeList)
            {

                //check if event is occuring at this time slice (pre-calculated list)
                var eventSlice = eventSliceList.Where(x => x.Time == time).FirstOrDefault();
                var eventIsOccuringNow = eventSlice.IsOccuring;

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
                //if event is not occuring now but occurred before
                else if (eventIsOccuringNow == false & eventOccuredInPreviousTime == true)
                {
                    //add previous event to list
                    var newEvent = new Event(eventSlice.Name,
                        eventSlice.Nature,
                        eventSlice.Description,
                        eventStartTime,
                        eventEndTime);

                    //if event is duration is 0 minute, log it
                    if (newEvent.GetDurationMinutes() <= 0)
                    {
                        //LibLogger.Debug("Event duration is 0!");
                    }


                    eventList.Add(newEvent);

                    //set flag
                    eventOccuredInPreviousTime = false;
                }

                //if event is occuring now & it is the last time
                if (eventIsOccuringNow == true & time == lastInstanceOfTime)
                {
                    //add current event to list
                    var newEvent2 = new Event(eventSlice.Name,
                        eventSlice.Nature,
                        eventSlice.Description,
                        eventStartTime,
                        eventEndTime);

                    //if event is duration is 0 minute, raise alarm
                    if (newEvent2.GetDurationMinutes() <= 0)
                    {
                        //LibLogger.Debug("Event duration is 0!");
                    }

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


        /// <summary>
        /// Get parsed EventDataList.xml from wwwroot file / static site
        /// Note: Event data list needed to calculate events
        /// TODO MOVE TO TOOLS!!!!!
        /// </summary>
        public static async Task<List<T>> GetEventDataList<T>(string httpUrl) where T : IToXml, new()
        {
            //get data list from Static Website storage
            //note : done so that any updates to that live file will be instantly reflected in API results
            var eventDataListXml = await Tools.GetXmlFileHttp(httpUrl);

            //parse each raw event data in list
            var eventDataList = new List<T>();
            foreach (var eventDataXml in eventDataListXml)
            {
                //add it to the return list
                var x = new T();
                eventDataList.Add(x.FromXml<T>(eventDataXml));
            }

            return eventDataList;

        }



        /// <summary>
        /// Gets all event data/types that match the inputed tag
        /// </summary>
        public static List<EventData> GetEventDataListByTag(EventTag tag)
        {

            //filter IN event data list by tag
            var filteredEventDataList = EventDataList.FindAll(eventData =>
            {
                var filter1 = eventData.EventTags.Contains(tag);

                return filter1;
            });

            return filteredEventDataList;
        }

        /// <summary>
        /// Parallel already built in
        /// </summary>
        public static List<Event> CalculateEvents(double eventsPrecision, Time startTime, Time endTime, GeoLocation getBirthLocation, Person inputPerson, List<EventTag> inputedEventTags)
        {
            //load data list
            //used later by calculate events
            EventDataList = GetEventDataList<EventData>(UrlEventDataListXml).Result;

            var sync = new object();//to lock thread access to list


            //set based on 

            //calculate events for each tag
            //NOTE: parallel speed > 143s > 40s
            Parallel.ForEach(inputedEventTags, (eventTag, state) =>
            {
                var tempEventList = _CalculateEvents(eventsPrecision, startTime, endTime, inputPerson.GetBirthLocation(), inputPerson, eventTag);


                //adding to list needs to be synced for thread safety
                lock (sync)
                {
                    //adding to list needs to be synced for thread safety
                    EventList.AddRange(tempEventList);
                }


            });

            return EventManager.EventList;

        }

        public static List<Event> _CalculateEvents(double eventsPrecision, Time startTime, Time endTime, GeoLocation geoLocation, Person person, EventTag tag)
        {

            //get all event data/types which has the inputed tag (FILTER)
            var eventDataListFiltered = EventManager.GetEventDataListByTag(tag);

            //start calculating events
            var eventList = EventManager.GetEventsInTimePeriod(startTime.GetStdDateTimeOffset(), endTime.GetStdDateTimeOffset(), geoLocation, person, eventsPrecision, eventDataListFiltered);

            //sort the list by time before sending view
            var orderByAscResult = from dasaEvent in eventList
                                   orderby dasaEvent.StartTime.GetStdDateTimeOffset()
                                   select dasaEvent;


            //send sorted events to view
            return orderByAscResult.ToList();

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