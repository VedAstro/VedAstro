
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VedAstro.Library
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


        //we use direct storage URL for fast access & solid
        private const string AzureStorage = "vedastrowebsitestorage.z5.web.core.windows.net";

        //used in muhurtha, dasa, etc... events
        //public const string UrlEventDataListXml = $"https://{AzureStorage}/data/EventDataList.xml";



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

            //place to store all generated events
            List<Event> eventList = new();

            //split time into slices based on precision
            var timeList = Time.GetTimeListFromRange(startTime, endTime, precisionInHours);

            foreach (var eventData in eventDataList)
            {
                //get list of occuring events for a single event data type
                var eventListForThisEvent = GetListOfEventsForEventData(eventData, person, timeList);

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
                var eventSlices = ConvertToEventSlice(timeList, eventData, person);

                //go through time slices and identify start & end of events, place them in a event list
                var finalList = EventSlicesToEvents(eventSlices);

                return finalList;
            }

        }

        //takes time list and checks if event is occuring for each time slice,
        //and returns it in dictionary list, done in parallel
        public static EventSlice[] ConvertToEventSlice(List<Time> timeList, EventData eventData, Person person, bool useParallel = true)
        {
            EventSlice[] returnList = new EventSlice[timeList.Count];

            if (useParallel)
            {
                Parallel.For(0, timeList.Count, i =>
                {
                    //event data list is recreated because the data
                    //inside is possibly changed when calculators are run (dynamic yeah!)
                    var timeTemp = timeList[i];

                    //get if event occurring at the inputed time (heavy computation)
                    var updatedEventData = ConvertToEventSlice(timeTemp, eventData, person);

                    //note: fills null if not occuring
                    returnList[i] = updatedEventData;
                });
            }
            else
            {
                for (int i = 0; i < timeList.Count; i++)
                {
                    var timeTemp = timeList[i];
                    var updatedEventData = ConvertToEventSlice(timeTemp, eventData, person);
                    returnList[i] = updatedEventData;
                }
            }

            return returnList;
        }


        /// <summary>
        /// Does the heavy computation to know if event is occuring.
        /// Note also same reason this is run separately only when needed & not run during startup
        /// , and it also allows the code to be managed easily for parallel computation
        /// combines event data and calculator result 
        /// </summary>
        public static EventSlice? ConvertToEventSlice(Time time, EventData eventData, Person person)
        {
            //if event is Empty, then not occuring
            if (eventData == EventData.Empty) { return EventSlice.Empty; }

            //do calculation for this event to get prediction data
            var predictionData = eventData.EventCalculator(time, person);

            if (predictionData.Occuring)
            {
                //extract the data out
                //store planets, houses & signs related to result
                //TODO related body data needs to pumped into slice
                var RelatedBody = predictionData.RelatedBody;

                //override event nature from xml if specified
                var nature = predictionData.NatureOverride == EventNature.Empty ? eventData.Nature : predictionData.NatureOverride;

                //override description if specified
                var isDescriptionOverride = !string.IsNullOrEmpty(predictionData.DescriptionOverride); //if true override, else false
                var description = isDescriptionOverride ? predictionData.DescriptionOverride : eventData.Description;

                if (nature == EventNature.Empty)
                {
                    throw new Exception("Something wrong");
                }

                //send caller the updated event data, since override possibility
                return new EventSlice(eventData.Name, nature, description, eventData.SpecializedSummary, time, predictionData.Occuring);

            }

            return null;
        }

        /// <summary>
        /// Scans array of event times, and creates begin and end based events
        /// Note: each array is one 1 event type only
        /// </summary>
        public static List<Event> EventSlicesToEvents(EventSlice[] eventSliceList)
        {
            //convert event slices to array of occuring and not occuring (easy to detect range)
            bool[] isOccuringList = Array.ConvertAll(eventSliceList, evtSlice => evtSlice != null);//not null is true in list

            //converts array into start & end ranges (all possible cases tested!)
            var ranges = EventManager.GetTrueRanges(isOccuringList);

            //each range detected becomes an event
            var returnList = ranges.Select(RangeToEvent).ToList();

            //send final compiled list to caller
            return returnList;


            //------------------LOCAL FUNCS

            //given a range converts it into an event
            Event RangeToEvent((int Start, int End) range)
            {
                //note: using start for all event Name, Nature, Description (expected to be same)
                var startSlice = eventSliceList[range.Start];
                var startTime = startSlice.Time;
                var endTime = eventSliceList[range.End].Time;

                //NOTE: this should be the only place event is created

                //TODO : temp hack to add in tags into events for easy sorting later

                var tags = GetTagsByEventName(startSlice.Name);

                var convertedEvent = new Event(startSlice.Name, startSlice.Nature, startSlice.Description, startSlice.SpecializedSummary, startTime, endTime, tags);

                return convertedEvent;
            }

        }

        /// <summary>
        /// Gets the method that does the calculations for an event based on the events name
        /// </summary>
        public static EventCalculatorDelegate GetEventCalculatorMethod(EventName inputEventName)
        {
            //get all event calculator methods
            var eventCalculatorList = typeof(EventCalculatorMethods).GetMethods();


            //loop through all calculators
            foreach (var eventCalculator in eventCalculatorList)
            {
                //try to get attribute attached to the calculator method
                var eventCalculatorAttribute = (EventCalculatorAttribute)Attribute.GetCustomAttribute(eventCalculator, typeof(EventCalculatorAttribute));

                //if attribute not found, go to next method (private function not meant for events)
                if (eventCalculatorAttribute == null) { continue; }

                //if attribute name matches input event name
                if (eventCalculatorAttribute.EventName == inputEventName)
                {
                    //convert calculator reference to a delegate instance
                    var eventCalculatorDelegate = (EventCalculatorDelegate)Delegate.CreateDelegate(typeof(EventCalculatorDelegate), eventCalculator);

                    //return calculator delegate
                    return eventCalculatorDelegate;
                }
            }

            //control should not reach here
            //if calculator method not found,
            //to make old code run with updated eventdatalist.xml file, an empty calculator return no event is attached as default
            throw new Exception($"Calculator method not found! : {inputEventName}");

        }

        /// <summary>
        /// Gets the method that does the calculations for an event based on the events name
        /// </summary>
        public static HoroscopeCalculatorDelegate GetHoroscopeCalculatorMethod(HoroscopeName inputEventName)
        {
            //get all event calculator methods
            var horoscopeCalculatorList = typeof(CalculateHoroscope).GetMethods();

            //loop through all calculators
            foreach (var horoscopeCalculator in horoscopeCalculatorList)
            {
                //try to get attribute attached to the calculator method
                var horoscopeCalculatorAttribute = (HoroscopeCalculatorAttribute)Attribute.GetCustomAttribute(horoscopeCalculator,
                    typeof(HoroscopeCalculatorAttribute));

                //if attribute not found
                if (horoscopeCalculatorAttribute == null)
                {   //go to next method
                    continue;
                }

                //if attribute name matches input event name
                if (horoscopeCalculatorAttribute.HoroscopeName == inputEventName)
                {
                    //convert calculator reference to a delegate instance
                    var horoscopeCalculatorDelegate = (HoroscopeCalculatorDelegate)Delegate.CreateDelegate(typeof(HoroscopeCalculatorDelegate), horoscopeCalculator);

                    //return calculator delegate
                    return horoscopeCalculatorDelegate;
                }
            }


            //if control reaches here than failure
            //if calculator method not found, raise error
            throw new Exception($"Calculator method not found! : {inputEventName.ToString()}");

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
                var specializedSummary = _event.SpecializedSummary;
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
                    //todo temp
                    var tags2 = EventManager.GetTagsByEventName(name);
                    newEvent = new Event(name, eventNature, description, specializedSummary, newStartTime, newEndTime, tags2);

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
                //todo temp
                var tags = EventManager.GetTagsByEventName(name);
                newEvent = new Event(name, eventNature, description, specializedSummary, newStartTime, endTime, tags); //use the original end time

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
        /// Gets all event data/types that match the inputed tag
        /// NOTE: file is cached, so may not be latest
        /// </summary>
        public static List<EventData> GetEventDataListByTag(EventTag tag)
        {

            //filter IN event data list by tag
            var filteredEventDataList = EventDataListStatic.Rows.FindAll(eventData =>
            {
                var filter1 = eventData.EventTags.Contains(tag);

                return filter1;
            });

            return filteredEventDataList;
        }

        /// <summary>
        /// Gets all tags for a given event name
        /// </summary>
        public static List<EventTag> GetTagsByEventName(EventName eventName)
        {

            //filter IN event data list by tag
            var tags = EventDataListStatic.Rows.Find(eventData => eventData.Name == eventName).EventTags;

            return tags;
        }

        /// <summary>
        /// Calculates events for given person, tags, time period
        /// Parallel already built in
        /// </summary>
        public static List<Event> CalculateEvents(double eventsPrecision, Time startTime, Time endTime, Person inputPerson, List<EventTag> inputedEventTags, bool filter0Duration = true)
        {

            //reset, if called in the same instance
            EventManager.EventList = new List<Event>();

            //calculate events for each tag
            //NOTE: parallel speed > 143s > 40s
            var sync = new object();//to lock thread access to list
            Parallel.ForEach(inputedEventTags, (eventTag, state) =>
            {
                //get all events for a specific tag
                var tempEventList = CalculateEventsForTag(eventsPrecision, startTime, endTime, inputPerson.GetBirthLocation(), inputPerson, eventTag, filter0Duration);

                //adding to list needs to be synced for thread safety
                lock (sync) { EventList.AddRange(tempEventList); }

            });

            return EventManager.EventList;

        }

        public static List<Event> CalculateEventsForTag(double eventsPrecision, Time startTime, Time endTime, GeoLocation geoLocation, Person person, EventTag tag, bool filter0Duration = true)
        {
            // Get all event data/types which has the inputed tag (FILTER)
            var eventDataListFiltered = EventManager.GetEventDataListByTag(tag);
            // Start calculating events
            var eventList = EventManager.GetEventsInTimePeriod(startTime.GetStdDateTimeOffset(), endTime.GetStdDateTimeOffset(), geoLocation, person, eventsPrecision, eventDataListFiltered);
            // If filter0Duration is true, remove all events with 0 duration
            if (filter0Duration)
            {
                eventList = eventList.Where(dasaEvent => dasaEvent.DurationMin > 0).ToList();
            }
            // Sort the list by time before sending view
            var sortedEventList = eventList.OrderBy(dasaEvent => dasaEvent.StartTime.GetStdDateTimeOffset()).ToList();
            // Send sorted events to view
            return sortedEventList;
        }


        /// <summary>
        /// takes an array of booleans and returns a list of tuples,
        /// where each tuple represents the start and end indices of a range of true values
        /// Note: written by AI
        /// </summary>
        public static List<(int Start, int End)> GetTrueRanges(bool[] array)
        {
            var ranges = new List<(int Start, int End)>();
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i])
                {
                    int start = i;
                    while (i < array.Length && array[i])
                    {
                        i++;
                    }
                    int end = i - 1;
                    ranges.Add((start, end));
                }
            }
            return ranges;
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