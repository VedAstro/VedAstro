using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using Azure.Storage.Blobs;
using Genso.Astrology.Library;
using Genso.Astrology.Library.Compatibility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace API
{
    public static class EntryPoint
    {

        //█░█ ▄▀█ █▀█ █▀▄   █▀▄ ▄▀█ ▀█▀ ▄▀█
        //█▀█ █▀█ █▀▄ █▄▀   █▄▀ █▀█ ░█░ █▀█


        //hard coded links to files stored in storage
        private const string PersonListXml = "vedastro-site-data/PersonList.xml";
        private const string MessageListXml = "vedastro-site-data/MessageList.xml";
        private const string TaskListXml = "vedastro-site-data/TaskList.xml";
        private const string VisitorLogXml = "vedastro-site-data/VisitorLog.xml";
        /// <summary>
        /// Default success message sent to caller
        /// </summary>
        private static string PassMessageXml = new XElement("Status", "Pass").ToString();




        //▄▀█ █▀█ █   █▀▀ █░█ █▄░█ █▀▀ ▀█▀ █ █▀█ █▄░█ █▀
        //█▀█ █▀▀ █   █▀░ █▄█ █░▀█ █▄▄ ░█░ █ █▄█ █░▀█ ▄█


        [FunctionName("getmatchreport")]
        public static async Task<IActionResult> Match(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            [Blob(PersonListXml, FileAccess.Read)] Stream personListRead,
            ILogger log)
        {
            string responseMessage;

            try
            {
                //get name of male & female
                dynamic names = await APITools.ExtractNames(req);

                //get list of all people
                var personList = new Data(personListRead);

                //generate compatibility report
                CompatibilityReport compatibilityReport = APITools.GetCompatibilityReport(names.Male, names.Female, personList);
                responseMessage = compatibilityReport.ToXml().ToString();
            }
            catch (Exception e)
            {
                //format error nicely to show user
                responseMessage = APITools.FormatErrorReply(e);
            }


            var okObjectResult = new OkObjectResult(responseMessage);
            //okObjectResult.ContentTypes.Add("text/html");
            return okObjectResult;
        }

        [FunctionName("addperson")]
        public static async Task<IActionResult> AddPerson(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestMessage incomingRequest,
            [Blob(PersonListXml, FileAccess.ReadWrite)] BlobClient personListClient)
        {
            var responseMessage = "";

            try
            {
                //get new person data out of incoming request
                //note: inside new person xml already contains user id
                var newPersonXml = APITools.ExtractDataFromRequest(incomingRequest);

                //add new person to main list
                var personListXml = APITools.AddXElementToXDocument(personListClient, newPersonXml);

                //upload modified list to storage
                await APITools.OverwriteBlobData(personListClient, personListXml);

                responseMessage = PassMessageXml;

            }
            catch (Exception e)
            {
                //format error nicely to show user
                responseMessage = APITools.FormatErrorReply(e);
            }


            var okObjectResult = new OkObjectResult(responseMessage);

            return okObjectResult;
        }

        [FunctionName("addmessage")]
        public static async Task<IActionResult> AddMessage(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestMessage incomingRequest,
            [Blob(MessageListXml, FileAccess.ReadWrite)] BlobClient messageListClient)
        {
            var responseMessage = "";

            try
            {
                //get new message data out of incoming request
                //note: inside new person xml already contains user id
                var newMessageXml = APITools.ExtractDataFromRequest(incomingRequest);

                //add new message to main list
                var messageListXml = APITools.AddXElementToXDocument(messageListClient, newMessageXml);

                //upload modified list to storage
                await APITools.OverwriteBlobData(messageListClient, messageListXml);

                responseMessage = PassMessageXml;

            }
            catch (Exception e)
            {
                //format error nicely to show user
                responseMessage = APITools.FormatErrorReply(e);
            }


            var okObjectResult = new OkObjectResult(responseMessage);

            return okObjectResult;
        }

        [FunctionName("addtask")]
        public static async Task<IActionResult> AddTask(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestMessage incomingRequest,
            [Blob(TaskListXml, FileAccess.ReadWrite)] BlobClient taskListClient)
        {
            var responseMessage = "";

            try
            {
                //get new task data out of incoming request 
                var newTaskXml = APITools.ExtractDataFromRequest(incomingRequest);

                //add new task to main list
                var taskListXml = APITools.AddXElementToXDocument(taskListClient, newTaskXml);

                //upload modified list to storage
                await APITools.OverwriteBlobData(taskListClient, taskListXml);

                responseMessage = PassMessageXml;

            }
            catch (Exception e)
            {
                //format error nicely to show user
                responseMessage = APITools.FormatErrorReply(e);
            }


            var okObjectResult = new OkObjectResult(responseMessage);

            return okObjectResult;
        }

        [FunctionName("addvisitor")]
        public static async Task<IActionResult> AddVisitor(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestMessage incomingRequest,
            [Blob(VisitorLogXml, FileAccess.ReadWrite)] BlobClient visitorLogClient)
        {
            var responseMessage = "";

            try
            {
                //get new visitor data out of incoming request 
                var newVisitorXml = APITools.ExtractDataFromRequest(incomingRequest);

                //add new visitor to main list
                var taskListXml = APITools.AddXElementToXDocument(visitorLogClient, newVisitorXml);

                //upload modified list to storage
                await APITools.OverwriteBlobData(visitorLogClient, taskListXml);

                responseMessage = PassMessageXml;

            }
            catch (Exception e)
            {
                //format error nicely to show user
                responseMessage = APITools.FormatErrorReply(e);
            }


            var okObjectResult = new OkObjectResult(responseMessage);

            return okObjectResult;
        }

        [FunctionName("getmalelist")]
        public static async Task<IActionResult> GetMaleList(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestMessage incomingRequest,
            [Blob(PersonListXml, FileAccess.ReadWrite)] BlobClient personListClient)
        {
            var responseMessage = "";

            try
            {

                //get user id
                var userId = APITools.ExtractDataFromRequest(incomingRequest).Value;

                //get person list from storage
                var personListXml = APITools.BlobClientToXml(personListClient);

                //get only male ppl into a list & matching user id
                var maleList = from person in personListXml.Root?.Elements()
                               where
                                   person.Element("Gender")?.Value == "Male" &&
                                   person.Element("UserId")?.Value == userId
                               select person;

                //send male list to caller
                responseMessage = new XElement("Root", maleList).ToString();

            }
            catch (Exception e)
            {
                //format error nicely to show user
                responseMessage = APITools.FormatErrorReply(e);
            }


            var okObjectResult = new OkObjectResult(responseMessage);

            return okObjectResult;
        }

        [FunctionName("getfemalelist")]
        public static async Task<IActionResult> GetFemaleList(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestMessage incomingRequest,
            [Blob(PersonListXml, FileAccess.ReadWrite)] BlobClient personListClient)
        {
            var responseMessage = "";

            try
            {
                //get user id
                var userId = APITools.ExtractDataFromRequest(incomingRequest).Value;

                //get person list from storage
                var personListXml = APITools.BlobClientToXml(personListClient);

                //get only female ppl into a list
                var maleList = from person in personListXml.Root?.Elements()
                               where
                                   person.Element("Gender")?.Value == "Female"
                                   &&
                                   person.Element("UserId")?.Value == userId
                               select person;

                //send female list to caller
                responseMessage = new XElement("Root", maleList).ToString();

            }
            catch (Exception e)
            {
                //format error nicely to show user
                responseMessage = APITools.FormatErrorReply(e);
            }


            var okObjectResult = new OkObjectResult(responseMessage);

            return okObjectResult;
        }

        /// <summary>
        /// Gets person all details from only hash
        /// </summary>
        [FunctionName("getperson")]
        public static async Task<IActionResult> GetPerson(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestMessage incomingRequest,
            [Blob(PersonListXml, FileAccess.ReadWrite)] BlobClient personListClient)
        {
            var responseMessage = "";

            try
            {
                //get hash that will be used find the person
                var requestData = APITools.ExtractDataFromRequest(incomingRequest);
                var originalHash = int.Parse(requestData.Value);

                //get the person record by hash
                var personListXml = APITools.BlobClientToXml(personListClient);
                var foundPerson = APITools.FindPersonByHash(personListXml, originalHash);

                //send person to caller
                responseMessage = new XElement("Root", foundPerson).ToString();

            }
            catch (Exception e)
            {
                //format error nicely to show user
                responseMessage = APITools.FormatErrorReply(e);
            }


            var okObjectResult = new OkObjectResult(responseMessage);

            return okObjectResult;
        }

        /// <summary>
        /// Generates a new SVG dasa report given a person hash
        /// </summary>
        [FunctionName("getpersondasareport")]
        public static async Task<IActionResult> GetPersonDasaReport(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestMessage incomingRequest,
            [Blob(PersonListXml, FileAccess.ReadWrite)] BlobClient personListClient)
        {
            var responseMessage = "";

            try
            {
                //get dasa report for sending
                var dasaReportSvg = await GetDasaReportSvgForIncomingRequest(incomingRequest);

                //send image back to caller
                var x = streamToByteArray(dasaReportSvg);
                return new FileContentResult(x, "image/svg+xml");

            }
            catch (Exception e)
            {
                //format error nicely to show user
                responseMessage = APITools.FormatErrorReply(e);
            }


            var okObjectResult = new OkObjectResult(responseMessage);

            return okObjectResult;

            //

            async Task<Stream> GetDasaReportSvgForIncomingRequest(HttpRequestMessage req)
            {
                //get hash that will be used find the person
                var requestData = APITools.ExtractDataFromRequest(req);
                var originalHash = int.Parse(requestData.Value);

                //get the person instance by hash
                var personListXml = APITools.BlobClientToXml(personListClient);
                var foundPersonXml = APITools.FindPersonByHash(personListXml, originalHash);
                var foundPerson = Person.FromXml(foundPersonXml);

                //from person get svg report
                var dasaReportSvgString = await GetDasaReportSvgFromApi(foundPerson);

                //convert svg string to stream for sending
                //todo check if using really needed here
                var stream = GenerateStreamFromString(dasaReportSvgString);

                return stream;
            }

        }

        /// <summary>
        /// Updates a person's record, uses hash to identify person to overwrite
        /// </summary>
        [FunctionName("updateperson")]
        public static async Task<IActionResult> UpdatePerson(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestMessage incomingRequest,
            [Blob(PersonListXml, FileAccess.ReadWrite)] BlobClient personListClient)
        {
            var responseMessage = "";

            try
            {
                //get unedited hash & updated person details from incoming request
                var requestData = APITools.ExtractDataFromRequest(incomingRequest);
                var originalHash = int.Parse(requestData?.Element("PersonHash").Value);
                var updatedPersonXml = requestData?.Element("Person");

                //get the person record that needs to be updated
                var personListXml = APITools.BlobClientToXml(personListClient);
                var personToUpdate = APITools.FindPersonByHash(personListXml, originalHash);

                //delete the previous person record,
                //and insert updated record in the same place
                personToUpdate.ReplaceWith(updatedPersonXml);

                //upload modified list to storage
                await APITools.OverwriteBlobData(personListClient, personListXml);

                responseMessage = PassMessageXml;

            }
            catch (Exception e)
            {
                //format error nicely to show user
                responseMessage = APITools.FormatErrorReply(e);
            }


            var okObjectResult = new OkObjectResult(responseMessage);

            return okObjectResult;
        }

        /// <summary>
        /// Deletes a person's record, uses hash to identify person
        /// Note : user id is not checked here because Person hash
        /// can't even be generated by client side if you don't have access.
        /// Theoretically anybody who gets the hash of the person,
        /// can delete the record by calling this API
        /// </summary>
        [FunctionName("deleteperson")]
        public static async Task<IActionResult> DeletePerson(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestMessage incomingRequest,
            [Blob(PersonListXml, FileAccess.ReadWrite)] BlobClient personListClient)
        {
            var responseMessage = "";

            try
            {
                //get unedited hash & updated person details from incoming request
                var requestData = APITools.ExtractDataFromRequest(incomingRequest);
                var originalHash = int.Parse(requestData.Value);

                //get the person record that needs to be deleted
                var personListXml = APITools.BlobClientToXml(personListClient);
                var personToDelete = APITools.FindPersonByHash(personListXml, originalHash);

                //delete the person record,
                personToDelete.Remove();

                //upload modified list to storage
                await APITools.OverwriteBlobData(personListClient, personListXml);

                responseMessage = PassMessageXml;

            }
            catch (Exception e)
            {
                //format error nicely to show user
                responseMessage = APITools.FormatErrorReply(e);
            }


            var okObjectResult = new OkObjectResult(responseMessage);

            return okObjectResult;
        }

        [FunctionName("getpersonlist")]
        public static async Task<IActionResult> GetPersonList(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestMessage incomingRequest,
            [Blob(PersonListXml, FileAccess.ReadWrite)] BlobClient personListClient)
        {
            var responseMessage = "";

            try
            {
                //get user id
                var userId = APITools.ExtractDataFromRequest(incomingRequest).Value;

                //get all person list from storage
                var personListXml = APITools.BlobClientToXml(personListClient);

                //filter out person by user id
                var filteredList = APITools.FindPersonByUserId(personListXml, userId);

                //send filtered list to caller
                responseMessage = new XElement("Root", filteredList).ToString();

            }
            catch (Exception e)
            {
                //format error nicely to show user
                responseMessage = APITools.FormatErrorReply(e);
            }


            var okObjectResult = new OkObjectResult(responseMessage);

            return okObjectResult;
        }

        [FunctionName("gettasklist")]
        public static async Task<IActionResult> GetTaskList(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestMessage incomingRequest,
            [Blob(TaskListXml, FileAccess.ReadWrite)] BlobClient taskListClient)
        {
            var responseMessage = "";

            try
            {
                //get task list from storage
                var taskListXml = APITools.BlobClientToXml(taskListClient);

                //send task list to caller
                responseMessage = taskListXml.ToString();

            }
            catch (Exception e)
            {
                //format error nicely to show user
                responseMessage = APITools.FormatErrorReply(e);
            }


            var okObjectResult = new OkObjectResult(responseMessage);

            return okObjectResult;
        }

        [FunctionName("getevents")]
        public static async Task<IActionResult> GetEvents(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestMessage incomingRequest,
            [Blob("vedastro-site-data/EventDataList.xml", FileAccess.ReadWrite)] BlobClient eventDataListClient)
        {
            var responseMessage = "";

            try
            {

                //get person list from storage
                var eventDataListXml = APITools.BlobClientToXml(eventDataListClient);

                //get data needed to generate events
                var requestData = APITools.ExtractDataFromRequest(incomingRequest);

                //parse it
                var person = Person.FromXml(requestData.Element("Person"));
                var startTime = Time.FromXml(requestData.Element("StartTime").Element("Time"));
                var endTime = Time.FromXml(requestData.Element("EndTime").Element("Time"));
                var location = GeoLocation.FromXml(requestData.Element("Location"));
                var tag = Tools.XmlToAnyType<EventTag>(requestData.Element(typeof(EventTag).FullName));
                var precision = Tools.XmlToAnyType<double>(requestData.Element(typeof(double).FullName));

                //calculate events from the data received
                var events = CalculateEvents(startTime, endTime, location, person, tag, precision, eventDataListXml);

                //convert events to XML for sending
                var rootXml = new XElement("Root");
                foreach (var _event in events)
                {
                    rootXml.Add(_event.ToXml());
                }

                responseMessage = rootXml.ToString();

            }
            catch (Exception e)
            {
                //format error nicely to show user
                responseMessage = APITools.FormatErrorReply(e);
            }


            var okObjectResult = new OkObjectResult(responseMessage);

            return okObjectResult;
        }





        //█▀█ █▀█ █ █░█ ▄▀█ ▀█▀ █▀▀   █▀▄▀█ █▀▀ ▀█▀ █░█ █▀█ █▀▄ █▀
        //█▀▀ █▀▄ █ ▀▄▀ █▀█ ░█░ ██▄   █░▀░█ ██▄ ░█░ █▀█ █▄█ █▄▀ ▄█


        public static List<Event> CalculateEvents(Time startTime, Time endTime, GeoLocation location, Person person, EventTag tag, double precision, XDocument dataEventdatalistXml)
        {

            //parse each raw event data in list
            var eventDataList = new List<EventData>();
            foreach (var eventData in dataEventdatalistXml.Root.Elements())
            {
                //add it to the return list
                eventDataList.Add(EventData.ToXml(eventData));
            }

            //get all event data/types which has the inputed tag (FILTER)
            var eventDataListFiltered = DatabaseManager.GetEventDataListByTag(tag, eventDataList);

            //TODO event generation time logging enable when can
            ////debug to measure event calculation time
            //var watch = Stopwatch.StartNew();

            //start calculating events
            var eventList = EventManager.GetEventsInTimePeriod(startTime.GetStdDateTimeOffset(), endTime.GetStdDateTimeOffset(), location, person, precision, eventDataListFiltered);

            //watch.Stop();
            //LogManager.Debug($"Events computed in: { watch.Elapsed.TotalSeconds}s");

            return eventList;
        }

        private static async Task<string> GetDasaReportSvgFromApi(Person inputPerson)
        {

            //px width & height of each slice of time
            //used when generating dasa rows
            //note: changes needed only here
            int _widthPerSlice = 1;
            int _heightPerSlice = 35;

            // One precision value for generating all dasa components,
            // because misalignment occurs if use different precision
            double _eventsPrecision = Tools.DaysToHours(14);

            double _timeSlicePrecision = _eventsPrecision;


            //prep data
            var startTime = inputPerson.GetBirthDateTime(); //start time is birth time
            var endTime = startTime.AddYears(120); //end time is 120 years from birth (dasa cycle)

            //use the inputed data to get events from API
            //note: below methods access the data internally
            var dasaEventList = await GetDasaEvents(_eventsPrecision, startTime, endTime, inputPerson);
            var bhuktiEventList = await GetBhuktiEvents(_eventsPrecision, startTime, endTime, inputPerson);
            var antaramEventList = await GetAntaramEvents(_eventsPrecision, startTime, endTime, inputPerson);

            //generate rows and pump them into the page
            var dasaSvgWidth = 0; //will be filled when calling row generator
            var compiledRow = "";
            
            //generate time slice only once for all rows
            var timeSlices = GetTimeSlices();

            //save a copy of the number of time slices used to calculate the svg total width later
            dasaSvgWidth = timeSlices.Count;

            compiledRow += await GenerateYearRowSvg(dasaEventList, timeSlices, _eventsPrecision, 0);
            compiledRow += await GenerateRowSvg(dasaEventList, timeSlices, _eventsPrecision, 12);
            compiledRow += await GenerateRowSvg(bhuktiEventList, timeSlices, _eventsPrecision, 12 + (37 * 1));
            compiledRow += await GenerateRowSvg(antaramEventList, timeSlices, _eventsPrecision, 12 + (37 * 2));

            //add in the cursor line
            compiledRow += $"<rect id=\"CursorLine\" width=\"2.3506315\" height=\"124.60775\" style=\"fill:#000000;\" x=\"0\" y=\"0\" />";

            //get now line position
            var nowLinePosition = GetLinePosition(timeSlices, DateTimeOffset.Now);
            compiledRow += $"<rect id=\"NowVerticalLine\" width=\"2.3506315\" height=\"124.60775\" style=\"fill:blue;\" x=\"0\" y=\"0\" transform=\"matrix(1, 0, 0, 1, {nowLinePosition}, 0)\" />";

            //wait!, add in life events also
            compiledRow += GetLifeEventLinesSvg(inputPerson);

            //compile the final svg
            var finalSvg = WrapSvgElements(compiledRow, dasaSvgWidth, (_heightPerSlice * 3) + 10); //little wiggle room

            return finalSvg;



            //█░░ █▀█ █▀▀ ▄▀█ █░░   █▀▀ █░█ █▄░█ █▀▀ ▀█▀ █ █▀█ █▄░█ █▀
            //█▄▄ █▄█ █▄▄ █▀█ █▄▄   █▀░ █▄█ █░▀█ █▄▄ ░█░ █ █▄█ █░▀█ ▄█

            
            //gets person's life events as lines for the dasa chart
            string GetLifeEventLinesSvg(Person person)
            {
                var compiledLines = "";

                foreach (var lifeEvent in person.LifeEventList)
                {

                    //get start time of life event and find the position of it in slices (same as now line)
                    //so that this life event line can be placed exactly on the report where it happened
                    var startTime = DateTimeOffset.ParseExact(lifeEvent.StartTime, Time.GetDateTimeFormat(), null);
                    var position = GetLinePosition(timeSlices, startTime);
                    compiledLines += $"<rect" +
                                     $"eventName=\"{lifeEvent.Name}\" " +
                                     $"age=\"{inputPerson.GetAge(startTime.Year)}\" " +
                                     $"stdTime=\"{startTime:dd/MM/yyyy}\" " + //show only date
                                     $" width=\"2.3506315\"" +
                                     $" height=\"124.60775\"" +
                                     $" style=\"fill:blue;\"" +
                                     $" x=\"0\"" +
                                     $" y=\"0\" " +
                                     $"transform=\"matrix(1, 0, 0, 1, {position}, 0)\" />";

                    //var rect = $"<rect " +
                    //           $"eventName=\"{foundEvent?.FormattedName}\" " +
                    //           $"age=\"{inputPerson.GetAge(slice)}\" " +
                    //           $"stdTime=\"{slice.GetStdDateTimeOffset():dd/MM/yyyy}\" " + //show only date
                    //           $"x=\"{horizontalPosition}\" " +
                    //           $"width=\"{_widthPerSlice}\" " +
                    //           $"height=\"{_heightPerSlice}\" " +
                    //           $"fill=\"{color}\" />";


                }


                return compiledLines;
            }


            //gets line position given a date
            //finds most closest time slice, else return 0 means none found
            int GetLinePosition(List<Time> timeSliceList, DateTimeOffset inputTime)
            {
                var nowYear = inputTime.Year;
                var nowMonth = inputTime.Month;

                //go through the list and find where the slice is closest to now
                var slicePosition = 0;
                foreach (var time in timeSliceList)
                {

                    //if same year and same month then send this slice position
                    //as the correct one
                    var sameYear = time.GetStdYear() == nowYear;
                    var sameMonth= time.GetStdMonth() == nowMonth;
                    if (sameMonth && sameYear)
                    {
                        return slicePosition;
                    }

                    //move to next slice position
                    slicePosition++;
                }

                //if control reaches here then now time not found in time slices
                //this is possible when viewing old charts as such set now line to 0
                return 0;

            }

            async Task<string> GenerateRowSvg(List<Event> eventList, List<Time> timeSlices, double precisionHours, int yAxis)
            {



                //generate the row for each time slice
                var rowHtml = "";
                var horizontalPosition = 0; //distance from left
                var prevEventName = EventName.EmptyEvent;
                foreach (var slice in timeSlices)
                {
                    //get event that occurred at this time slice
                    //if more than 1 event raise alarm
                    var foundEventList = eventList.FindAll(tempEvent => tempEvent.IsOccurredAtTime(slice));
                    if (foundEventList.Count > 1) throw new Exception("Only 1 event in 1 time slice!");
                    var foundEvent = foundEventList[0];

                    //if current event is different than event has changed, so draw a black line
                    var isNewEvent = prevEventName != foundEvent.Name;
                    var color = isNewEvent ? "black" : GetEventColor(foundEvent?.Nature);
                    prevEventName = foundEvent.Name;

                    //generate and add to row
                    //the hard coded attribute names used here are used in App.js
                    var rect = $"<rect " +
                               $"eventName=\"{foundEvent?.FormattedName}\" " +
                               $"age=\"{inputPerson.GetAge(slice)}\" " +
                               $"stdTime=\"{slice.GetStdDateTimeOffset():dd/MM/yyyy}\" " + //show only date
                               $"x=\"{horizontalPosition}\" " +
                               $"width=\"{_widthPerSlice}\" " +
                               $"height=\"{_heightPerSlice}\" " +
                               $"fill=\"{color}\" />";

                    //set position for next element
                    horizontalPosition += _widthPerSlice;

                    rowHtml += rect;

                }

                //wrap all the rects inside a svg so they can me moved together
                //svg tag here acts as group, svg nesting
                rowHtml = $"<svg y=\"{yAxis}\" >{rowHtml}</svg>";

                return rowHtml;
            }

            async Task<string> GenerateYearRowSvg(List<Event> eventList, List<Time> timeSlices, double precisionHours, int yAxis)
            {


                //generate the row for each time slice
                var rowHtml = "";
                var previousYear = 0;
                var yearBoxWidthCount = 0;
                int rectWidth = 0;
                int newX = 0;
                foreach (var slice in timeSlices)
                {

                    //only generate new year box when year changes
                    var yearChanged = previousYear != slice.GetStdYear();

                    //if year changed
                    if (yearChanged)
                    {
                        //and it is in the beginning
                        if (previousYear == 0)
                        {
                            yearBoxWidthCount = 0; //reset width
                        }
                        else
                        {
                            //generate previous year data first before resetting
                            newX += rectWidth; //use previous rect width to position this
                            rectWidth = yearBoxWidthCount * _widthPerSlice; //calculate new rect width

                            var rect = $"<g x=\"{newX}\" y=\"{20}\" transform=\"matrix(1, 0, 0, 1, {newX}, 0)\">" +
                                                $"<rect " +
                                                    $"fill=\"#0d6efd\" x=\"0\" y=\"0\" width=\"{rectWidth}\" height=\"{11}\" rx=\"0\" ry=\"0\"" + $"style=\"paint-order: stroke; stroke: rgb(255, 255, 255); stroke-opacity: 1; stroke-linejoin: round;\"/>" +
                                                    $"<text x=\"0\" y=\"18.034\" fill=\"white\" " +
                                                        $"style=\"fill: rgb(255, 255, 255); font-size: 10.2278px; font-weight: 700; line-height: 36.3655px; white-space: pre;\"" +
                                                        $"transform=\"matrix(0.966483, 0, 0, 0.879956, 2, -6.779947)\"" +
                                                        $"x=\"0\" y=\"18.034\" bx:origin=\"0.511627 0.5\">" +
                                                        $"{previousYear}" + //previous year generate at begin of new year
                                                    $"</text>" +
                                             $"</g>";

                            //<g x="3" y="20" transform="matrix(1, 0, 0, 1, 3, 0)">
                            //    <rect fill="#0d6efd" width="27" height="11" style="paint-order: stroke; stroke: rgb(255, 255, 255); stroke-opacity: 1; stroke-linejoin: round;"/>
                            //    <text style="fill: rgb(255, 255, 255); font-size: 10.2278px; font-weight: 700; line-height: 36.3655px; white-space: pre;" transform="matrix(0.966483, 0, 0, 0.879956, 2, -6.779947)" x="0" y="18.034" bx:origin="0.511627 0.5">1943</text>
                            //</g>



                            //add to final return
                            rowHtml += rect;

                            //reset width
                            yearBoxWidthCount = 0;

                        }
                    }
                    //year same as before
                    else
                    {
                        //update width only, position is same
                        //as when created the year box
                        //yearBoxWidthCount *= _widthPerSlice;

                    }

                    //update previous year for next slice
                    previousYear = slice.GetStdYear();

                    yearBoxWidthCount++;


                }

                //wrap all the rects inside a svg so they can me moved together
                //svg tag here acts as group, svg nesting
                //rowHtml = $"<svg y=\"{yAxis}\" >{rowHtml}</svg>";

                return rowHtml;
            }

            // Get dasa color based on nature & number of events
            string GetEventColor(EventNature? eventNature)
            {
                var colorId = "gray";

                if (eventNature == null) { return colorId; }

                //set color id based on nature
                switch (eventNature)
                {
                    case EventNature.Good:
                        colorId = "green";
                        break;
                    case EventNature.Neutral:
                        colorId = "";
                        break;
                    case EventNature.Bad:
                        colorId = "red";
                        break;
                }

                return colorId;
            }

            //wraps a list of svg elements inside 1 main svg element
            //if width not set defaults to 1000px, and height to 1000px
            string WrapSvgElements(string combinedSvgString, int svgWidth = 1000, int svgTotalHeight = 1000)
            {

                //create the final svg that will be displayed
                var svgTotalWidth = svgWidth + 10; //add little for wiggle room
                var svgBody = $"<svg id=\"DasaViewHolder\" " +
                              $"style=\"" +
                              $"width:{svgTotalWidth}px;" +
                              $"height:{svgTotalHeight}px;" +
                              $"\" " +
                              $"xmlns=\"http://www.w3.org/2000/svg\">" +
                              $"{combinedSvgString}</svg>";

                return svgBody;
            }

            //generates time slices for dasa
            List<Time> GetTimeSlices()
            {
                //get time slices used to get events
                var startTime = inputPerson.GetBirthDateTime(); //start time is birth time
                var endTime = startTime.AddYears(120); //end time is 120 years from birth (dasa cycle)
                List<Time> timeSlices = EventManager.GetTimeListFromRange(startTime, endTime, _timeSlicePrecision);

                return timeSlices;
            }

        }

        /// <summary>
        /// Gets Dasa events from API
        /// </summary>
        public static async Task<List<Event>?> GetDasaEvents(double _eventsPrecision, Time startTime, Time endTime, Person person)
            => await EventsByTag(EventTag.Dasa, _eventsPrecision, startTime, endTime, person);

        /// <summary>
        /// Gets Bhukti events from API
        /// </summary>
        public static async Task<List<Event>?> GetBhuktiEvents(double _eventsPrecision, Time startTime, Time endTime, Person person)
            => await EventsByTag(EventTag.Bhukti, _eventsPrecision, startTime, endTime, person);

        /// <summary>
        /// Gets Antaram events from API
        /// </summary>
        public static async Task<List<Event>?> GetAntaramEvents(double _eventsPrecision, Time startTime, Time endTime, Person person)
            => await EventsByTag(EventTag.Antaram, _eventsPrecision, startTime, endTime, person);

        /// <summary>
        /// gets events from server filtered by event tag
        /// </summary>
        public static async Task<List<Event>?> EventsByTag(EventTag tag, double precisionHours, Time startTime, Time endTime, Person person)
        {

            //get events from API server
            var dasaEventsUnsorted =
                await GetEventsFromApi(
                    startTime,
                    endTime,
                    //birth location always as current place,
                    //since place does not matter for Dasa
                    person.GetBirthLocation(),
                    person,
                    tag,
                    precisionHours);


            //sort the list by time before sending view
            var orderByAscResult = from dasaEvent in dasaEventsUnsorted
                                   orderby dasaEvent.StartTime.GetStdDateTimeOffset()
                                   select dasaEvent;


            //send sorted events to view
            return orderByAscResult.ToList();
        }

        /// <summary>
        /// Gets Muhurtha events from API
        /// </summary>
        public static async Task<List<Event>> GetEventsFromApi(Time startTime, Time endTime, GeoLocation location, Person person, EventTag tag, double precisionHours)
        {
            //prepare data to send to API
            var root = new XElement("Root");

            root.Add(
                new XElement("StartTime", startTime.ToXml()),
                new XElement("EndTime", endTime.ToXml()),
                location.ToXml(),
                person.ToXml(),
                Tools.AnyTypeToXml(tag),
                Tools.AnyTypeToXml(precisionHours));

            //get person list from storage
            var eventDataListClient = await GetFileFromContainer("EventDataList.xml", "vedastro-site-data");
            var eventDataListXml = APITools.BlobClientToXml(eventDataListClient);


            //calculate events from the data received
            var events = CalculateEvents(startTime, endTime, location, person, tag, precisionHours, eventDataListXml);

            return events;

            ////send to api and get results
            //var resultsRaw = await ServerManager.WriteToServer(ServerManager.GetEventsApi, root);


            ////parse raw results
            //List<Event> resultsParsed = Event.FromXml(resultsRaw);

            ////send to caller
            //return resultsParsed;
        }

        public static byte[] streamToByteArray(Stream input)
        {
            //reset stream position
            input.Position = 0;
            MemoryStream ms = new MemoryStream();
            input.CopyTo(ms);
            return ms.ToArray();
        }

        public static Stream GenerateStreamFromString(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }



        //▄▀█ ▀█ █░█ █▀█ █▀▀   █▀ ▀█▀ █▀█ █▀█ ▄▀█ █▀▀ █▀▀   █▀▀ █░█ █▄░█ █▀▀ ▀█▀ █ █▀█ █▄░█ █▀
        //█▀█ █▄ █▄█ █▀▄ ██▄   ▄█ ░█░ █▄█ █▀▄ █▀█ █▄█ ██▄   █▀░ █▄█ █░▀█ █▄▄ ░█░ █ █▄█ █░▀█ ▄█


        private static async Task<BlobClient> GetFileFromContainer(string fileName, string blobContainerName)
        {
            //get the connection string stored separately (for security reasons)
            var storageConnectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");

            //get image from storage
            var blobContainerClient = new BlobContainerClient(storageConnectionString, blobContainerName);
            var fileBlobClient = blobContainerClient.GetBlobClient(fileName);

            return fileBlobClient;
            
            //var returnStream = new MemoryStream();
            //await fileBlobClient.DownloadToAsync(returnStream);

            //return returnStream;
        }


    }
}
