using System;
using Genso.Astrology.Library;


namespace Muhurtha.Desktop
{
    class Program
    {
        static void Main(string[] args)
        {

            Start();

            System.Console.ReadLine();
        }

        public static void Start()
        {
            var startStdTime = DateTimeOffset.ParseExact("00:00 19/03/2021 +08:00", Time.GetDateTimeFormat(), null);
            var endStdTime = DateTimeOffset.ParseExact("23:59 20/03/2021 +08:00", Time.GetDateTimeFormat(), null);
            var geoLocation = new GeoLocation("Ipoh", 101.0901, 4.5975);


            //get list of event data to check for event
            var eventDataList = DatabaseManager.GetEventDataList();

            //filter IN event data list
            var filteredEventDataList = eventDataList.FindAll(eventData =>
            {
                //single tag filter
                //var filter1 = eventData.GetName() == EventName.SuryaSankramana || eventData.GetName() == EventName.Sunset || eventData.GetName() == EventName.Midday;
                //var filter1 = eventData.GetName() == EventName.Papashadvargas;
                //var filter1 = eventData.GetName().ToString().Contains("Suns");
                var filter1 = eventData.GetEventTags().Contains(EventTag.Personal);

                return filter1;
            });

            var muhurthaTimePeriod = General.GetNewMuhurthaTimePeriod(startStdTime, endStdTime, geoLocation, personSindhu, TimePreset.Minute1, filteredEventDataList);

            //debug print data
            System.Console.WriteLine($"Number of events: {muhurthaTimePeriod.GetEventList().Count}");
            System.Console.WriteLine($"Cache use count: {CacheManager.CacheUseCount}");
            System.Console.WriteLine($"Cache not use count: {CacheManager.CacheNotUseCount}");

            //2.5 SPLIT THE EVENTS
            System.Console.WriteLine("Splitting events");
            var splittedEvents = General.SplitEventsByDay(muhurthaTimePeriod.GetEventList());


            //3. UPLOAD TO CALENDAR

            foreach (var e in splittedEvents)
            {
                System.Console.WriteLine(e.ToString());
            }

            System.Console.WriteLine("UNSPLITTED");

            foreach (var e in muhurthaTimePeriod.GetEventList())
            {
                System.Console.WriteLine(e.ToString());
            }

            System.Console.WriteLine("Add events to Google calender? (y/n)");

            var input = System.Console.ReadLine();

            if (string.Equals(input, "y", StringComparison.OrdinalIgnoreCase))
            {
                //add events to google calender
                Calendar.AddEventsToGoogleCalender(splittedEvents, Calendar.Sindhu);
                //Logic.General.AddEventsToiCloudCalender(temp.GetEventList());

            }

            System.Console.ReadLine();

        }

    }
}
