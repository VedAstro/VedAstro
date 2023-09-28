//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Threading;
//using Google.Apis.Auth.OAuth2;
//using Google.Apis.Calendar.v3;
//using Google.Apis.Calendar.v3.Data;
//using Google.Apis.Services;
//using Google.Apis.Util.Store;

//namespace VedAstro.Library
//{
//    public static class CalendarManager
//    {


//        /** FIELDS **/

//        //used for canceling sending halfway
//        public static CancellationToken threadCanceler;


//        /** CONST FIELDS **/



//        /** GOOGLE **/
//        /// <summary>
//        /// Gets access to google calendar for a particular account
//        /// If account data is not present, login screen will popup in browser
//        /// </summary>
//        public static CalendarService GetAccountAPIAccessGoogle()
//        {
//            // If modifying these scopes, delete your previously saved credentials
//            // at ~/.credentials/calendar-dotnet-quickstart.json
//            string[] Scopes = {  CalendarService.Scope.Calendar };
//            string ApplicationName = "MuhurthaCalendarAPI";

//            UserCredential credential;

//            using (var stream = new FileStream("google_credentials.json", FileMode.Open, FileAccess.Read))
//            {
//                // The file token.json stores the user's access and refresh tokens, and is created
//                // automatically when the authorization flow completes for the first time.
//                string credPath = "token.json";

//                //opens login page to get authentication
//                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
//                    GoogleClientSecrets.Load(stream).Secrets,
//                    Scopes,
//                    "user",
//                    CancellationToken.None,
//                    new FileDataStore(credPath, true)).Result;

//                //debug print
//                LibLogger.Debug("Credential file saved to: " + credPath);
//            }


//            // Create Google Calendar API service.
//            var service = new CalendarService(new BaseClientService.Initializer()
//            {
//                HttpClientInitializer = credential,
//                ApplicationName = ApplicationName,
//            });


//            return service;
//        }

//        /// <summary>
//        /// Adds a list of event to google calendar
//        /// Note: Cancellation token is raised & caught here
//        /// </summary>
//        public static void AddEventsToCalenderGoogle(List<Event> eventToAddList, String calendarId, bool enableReminders, string customEventName)
//        {
//            var addedEventCount = 0;

//            // get the Google Calendar API service, for connecting to calendar
//            var service = GetAccountAPIAccessGoogle();

//            try
//            {
//                //loop through each event in list & add to calendar
//                foreach (var eventToAdd in eventToAddList)
//                {
//                    //check if user has canceled sending events halfway
//                    threadCanceler.ThrowIfCancellationRequested();

//                    //log progress
//                    LibLogger.Debug($"{addedEventCount} of {eventToAddList.Count} Events added to Google Calendar");

//                    //increment progress counter
//                    addedEventCount++;

//                    //add an event to calendar
//                    AddEventToCalendar(eventToAdd, service, enableReminders, customEventName);

//                }

//            }
//            //catches only exceptions that idicates that user canceled the sending (caller lost interest in the result)
//            catch (Exception e) when (e.GetType() == typeof(OperationCanceledException))
//            {
//                //log the event and end here
//                LibLogger.Debug($"User canceled sending events to google calendar: {addedEventCount} events already sent!");
//                return;
//            }


//            LibLogger.Debug($"All {eventToAddList.Count} events added to Google calender");


//            //----------------FUNCTIONS-----------------------

//            void AddEventToCalendar(Event eventToAdd, CalendarService service, bool enableReminders, string customEventName)
//            {

//                //if a custom event name is specified use that instead (empty string not specified)
//                var eventName = customEventName == "" ? Format.FormatName(eventToAdd) : customEventName;

//                //create the event in google's calendar type
//                var newEvent = new Google.Apis.Calendar.v3.Data.Event()
//                {
//                    Summary = eventName,
//                    //Location = "",
//                    Description = eventToAdd.GetDescription() + "\nDuration:" + TimeSpan.FromMinutes(eventToAdd.GetDurationMinutes()).ToString(),
//                    Start = new EventDateTime()
//                    {
//                        DateTime = eventToAdd.GetStartTime().GetStdDateTimeOffset().DateTime,
//                        TimeZone = "Asia/Kuala_Lumpur",
//                    },
//                    End = new EventDateTime()
//                    {
//                        DateTime = eventToAdd.GetEndTime().GetStdDateTimeOffset().DateTime,
//                        TimeZone = "Asia/Kuala_Lumpur",
//                    },
//                    ColorId = GetEventColorGoogle(eventToAdd)
//                };

//                //if reminders is enabled added it to the event
//                if (enableReminders)
//                {
//                    newEvent.Reminders = new Google.Apis.Calendar.v3.Data.Event.RemindersData()
//                    {
//                        UseDefault = false,
//                        Overrides = new EventReminder[]
//                        {
//                            new EventReminder() {Method = "popup", Minutes = 24 * 60}, //1 day before
//                            new EventReminder() {Method = "popup", Minutes = 60}, //1 hour before
//                            new EventReminder() {Method = "popup", Minutes = 1} //1 minute before
//                        }
//                    };
//                }

//                //insert the event into google calander service
//                EventsResource.InsertRequest request = service.Events.Insert(newEvent, calendarId);
//                var createdEvent = request.Execute();

//            }

//        }

//        /// <summary>
//        /// Customizable function to delete accidentally created events
//        /// Example date : "00:00 01/01/2021 +08:00"
//        /// </summary>
//        public static void DeleteEventsGoogle(string calendarId, string searchText, string startDate)
//        {
//            // get the Google Calendar API service, for connecting to calendar
//            var service = GetAccountAPIAccessGoogle();

//            EventsResource.ListRequest eventRequest = service.Events.List(calendarId);

//            //specify variables to find the events to delete
//            eventRequest.Q = searchText;
//            //all events after this time
//            eventRequest.TimeMin = DateTimeOffset.ParseExact(startDate, Time.DateTimeFormat, null).DateTime;

//            //execute request
//            var result = eventRequest.Execute();

//            //loop through the events, and delete
//            foreach (var googleEvent in result.Items)
//            {
//                // Delete an event
//                service.Events.Delete(calendarId, googleEvent.Id).Execute();
//            }

//            //debug event
//            LibLogger.Debug($"{result.Items.Count} events deleted!");

//        }

//        /// <summary>
//        /// Gets the right color id for the event (google calendar) based on nature of the event
//        /// </summary>
//        /// <returns></returns>
//        private static string GetEventColorGoogle(Event enEvent)
//        {
//            var colorId = "";

//            //1 blue

//            //2 green

//            //3 purple

//            //4 red

//            //5 yellow

//            //6 orange

//            //7 turquoise

//            //8 gray

//            //9 bold blue

//            //10 bold green

//            //11 bold red

//            //set color id based on nature
//            switch (enEvent.GetNature())
//            {
//                case EventNature.Good:
//                    colorId = "2";
//                    break;
//                case EventNature.Neutral:
//                    colorId = "1";
//                    break;
//                case EventNature.Bad:
//                    colorId = "4";
//                    break;
//            }

//            return colorId;
//        }

//        /// <summary>
//        /// Gets all calendars in the google account
//        /// </summary>
//        public static List<Calendar> GetCalendarListGoogle()
//        {
//            //get the Google Calendar API service, for connecting to calendar
//            var service = GetAccountAPIAccessGoogle();

//            //specify to get the calendar list
//            var eventRequest = service.CalendarList.List();

//            //execute request (possible delay)
//            CalendarList result = eventRequest.Execute();

//            //parse the raw calendars into a usable struture
//            var calendarList = new List<Calendar>();
//            foreach (CalendarListEntry rawCalendar in result.Items)
//            {
//                var calendar = new Calendar { Id = rawCalendar.Id, Name = rawCalendar.Summary };
//                calendarList.Add(calendar);
//            }

//            return calendarList;
//        }
//    }
//}