using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Genso.Astrology.Library;

namespace Horoscope.Desktop
{
    public static class HoroscopeCore
    {
        /** FIELDS **/

        //used for canceling calculation halfway
        public static CancellationToken threadCanceler;

        /** CONST FIELDS **/
        //TODO Note ; missing files at these location fail without proper catch
        private const string PredictionFilePath = "data\\PredictionDataList.xml";
        private const string PersonFilePath = "data\\PersonList.xml";



        /** EVENTS **/
        //fired when event calculator completes
        public static event Action EventCalculationCompleted;


        /** PUBLIC METHODS **/
        public static List<Person> GetAllPeopleList() => DatabaseManager.GetPersonList(PersonFilePath);

        public static List<Prediction> GetPrediction(Person person, EventTag tag)
        {
            //note: modified to use birth time as start & end time
            var startStdTime = person.GetBirthDateTime().GetStdDateTimeOffset();
            var endStdTime = person.GetBirthDateTime().GetStdDateTimeOffset();

            var location = person.GetBirthLocation();

            //get list of event data to check for event
            var eventDataList = DatabaseManager.GetPredictionDataList(PredictionFilePath);

            //pass thread canceler General, so that methods inside can be stopped if needed
            General.threadCanceler = threadCanceler;

            //start calculating predictions
            var predictionList = GetListOfPredictionInTimePeriod(startStdTime, endStdTime, location, person, TimePreset.Minute1, eventDataList);

            //fire event to let others know event calculation is done
            EventCalculationCompleted.Invoke();

            return predictionList;
        }

        /// <summary>
        /// Get list of predictions occurrig in a time periode for all the
        /// inputed prediction types aka "prediction data"
        /// </summary>
        public static List<Prediction> GetListOfPredictionInTimePeriod(DateTimeOffset startStdTime, DateTimeOffset endStdTime, GeoLocation geoLocation, Person person, double precisionInHours, List<EventData> eventDataList)
        {
            //get data to instantiate muhurtha time period
            //get start & end times
            var startTime = new Time(startStdTime, geoLocation);
            var endTime = new Time(endStdTime, geoLocation);


            //initialize empty list of event to return
            List<Prediction> eventList = new();

            //split time into slices based on precision
            List<Time> timeList = GetTimeListFromRange(startTime, endTime, precisionInHours);

            var sync = new object();//to lock thread access to list

            //var count = 0;
            try
            {
                Parallel.ForEach(eventDataList, (eventData) =>
                {
                    //get list of occuring events for a sigle event type
                    var eventListForThisEvent = GetPredictionListByEventData(eventData, person, timeList);
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
                return new List<Prediction>();
            }


            //return calculated event list
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
        /// Get a list of events in a time period for a single event type aka "event data"
        /// Decision on when event starts & ends is also done here
        /// </summary>
        private static List<Prediction> GetPredictionListByEventData(EventData eventData, Person person, List<Time> timeList)
        {
            //declare empty event list to fill
            var eventList = new List<Prediction>();

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
                    var newEvent = new Prediction(eventData.GetName(),
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
                    var newEvent2 = new Prediction(eventData.GetName(),
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

    }
}
