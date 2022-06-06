using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
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
        private const string CachedDasaReportXml = "vedastro-site-data/CachedDasaReport.xml";
        private const string MessageListXml = "vedastro-site-data/MessageList.xml";
        private const string TaskListXml = "vedastro-site-data/TaskList.xml";
        private const string VisitorLogXml = "vedastro-site-data/VisitorLog.xml";
        /// <summary>
        /// Default success message sent to caller
        /// </summary>
        private static readonly OkObjectResult PassMessage = new(new XElement("Status", "Pass").ToString());




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
                return APITools.FormatErrorReply(e);
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

                return PassMessage;

            }
            catch (Exception e)
            {
                //format error nicely to show user
                return APITools.FormatErrorReply(e);
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

                return PassMessage;

            }
            catch (Exception e)
            {
                //format error nicely to show user
                return APITools.FormatErrorReply(e);
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

                return PassMessage;

            }
            catch (Exception e)
            {
                //format error nicely to show user
                return APITools.FormatErrorReply(e);
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

                return PassMessage;

            }
            catch (Exception e)
            {
                //format error nicely to show user
                return APITools.FormatErrorReply(e);
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
                return APITools.FormatErrorReply(e);
            }


            var okObjectResult = new OkObjectResult(responseMessage);

            return okObjectResult;
        }

        /// <summary>
        /// Gets all the unique visitors to the site
        /// </summary>
        [FunctionName("getvisitorlist")]
        public static async Task<IActionResult> GetVisitorList(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestMessage incomingRequest,
            [Blob(VisitorLogXml, FileAccess.ReadWrite)] BlobClient visitorLogClient)
        {
            var responseMessage = "";

            try
            {

                //get user id
                var userId = APITools.ExtractDataFromRequest(incomingRequest).Value;

                //get visitor log from storage
                var visitorLogXml = APITools.BlobClientToXml(visitorLogClient);

                //get all unique visitor elements only
                var uniqueVisitorList = from visitorXml in visitorLogXml.Root?.Elements()
                                        where
                                            //note: location tag only exists for new visitor log,
                                            //so use that to get unique list
                                            visitorXml.Element("Location") != null
                                        select visitorXml;

                //send list to caller
                responseMessage = new XElement("Root", uniqueVisitorList).ToString();

            }
            catch (Exception e)
            {
                //format error nicely to show user
                return APITools.FormatErrorReply(e);
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
                var femaleList = from person in personListXml.Root?.Elements()
                                 where
                                     person.Element("Gender")?.Value == "Female"
                                     &&
                                     person.Element("UserId")?.Value == userId
                                 select person;

                //send female list to caller
                responseMessage = new XElement("Root", femaleList).ToString();

            }
            catch (Exception e)
            {
                //format error nicely to show user
                return APITools.FormatErrorReply(e);
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
                return APITools.FormatErrorReply(e);
            }


            var okObjectResult = new OkObjectResult(responseMessage);

            return okObjectResult;
        }


        /// <summary>
        /// Generates a new SVG dasa report given a person hash
        /// Exp call:
        /// <Root>
        //      <PersonHash>374117709</PersonHash>
        //      <StartTime>
        //          <Time>
        //              <StdTime>00:00 01/01/1994 +08:00</StdTime>
        //              <Location>
        //                  <Name>Teluk Intan</Name>
        //                  <Longitude>101.0206</Longitude>
        //                  <Latitude>4.0224</Latitude>
        //              </Location>
        //          </Time>
        //  </StartTime>
        //  <EndTime>
        //      <Time>
        //          <StdTime>11:59 31/12/2024 +08:00</StdTime>
        //          <Location>
        //              <Name>Teluk Intan</Name>
        //              <Longitude>101.0206</Longitude>
        //              <Latitude>4.0224</Latitude>
        //          </Location>
        //      </Time>
        //  </EndTime>
        //  <DaysPerPixel>11</DaysPerPixel>
        //</Root>
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
                var dasaReportSvg = await GetDasaReportSvgForIncomingRequest(incomingRequest, personListClient);

                //send image back to caller
                var x = StreamToByteArray(dasaReportSvg);
                return new FileContentResult(x, "image/svg+xml");

            }
            catch (Exception e)
            {
                //format error nicely to show user
                return APITools.FormatErrorReply(e);
            }


            var okObjectResult = new OkObjectResult(responseMessage);

            return okObjectResult;

            //

        }

        /// <summary>
        /// Generates a new SVG dasa report given a person hash
        /// PRIVATE METHOD TO DEBUG AND TEST
        /// Exp call: <PersonHash>374117709</PersonHash
        /// </summary>
        [FunctionName("refreshdasareportcache")]
        public static async Task<IActionResult> RefreshDasaReportCacheApi(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestMessage incomingRequest,
            [Blob(PersonListXml, FileAccess.ReadWrite)] BlobClient personListClient,
            [Blob(CachedDasaReportXml, FileAccess.ReadWrite)] BlobClient cachedReportsClient
            )
        {
            var responseMessage = "";

            try
            {
                var personHashXml = APITools.ExtractDataFromRequest(incomingRequest);
                var personHash = int.Parse(personHashXml.Value);

                await RefreshDasaReportCache(APITools.GetPersonFromHash(personHash, personListClient), cachedReportsClient);

                return PassMessage;

            }
            catch (Exception e)
            {
                //format error nicely to show user
                return APITools.FormatErrorReply(e);
            }

        }

        /// <summary>
        /// Generates a new SVG dasa report given a person hash (cached)
        /// Call normally as method above
        /// NOTE:
        /// no specific time used standard 120 years only
        /// each cache is 120 years at a specific resolution (days per pixel)
        /// It is done so that cache is not clogged with individual time slice and resolution
        /// </summary>
        [FunctionName("getpersondasareportcached")]
        public static async Task<IActionResult> GetPersonDasaReportCached(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestMessage incomingRequest,
            [Blob(PersonListXml, FileAccess.ReadWrite)] BlobClient personListClient,
            [Blob(CachedDasaReportXml, FileAccess.ReadWrite)] BlobClient cachedDasaReportXmlClient)
        {
            var responseMessage = "";

            try
            {
                //get dasa report for sending
                var dasaReportSvg = await GetDasaReportSvgForIncomingRequestCached(incomingRequest, personListClient, cachedDasaReportXmlClient);

                return new OkObjectResult(dasaReportSvg.ToString());

            }
            catch (Exception e)
            {
                //format error nicely to show user
                return APITools.FormatErrorReply(e);
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

                return PassMessage;

            }
            catch (Exception e)
            {
                //format error nicely to show user
                return APITools.FormatErrorReply(e);
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

                return PassMessage;

            }
            catch (Exception e)
            {
                //format error nicely to show user
                return APITools.FormatErrorReply(e);
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
        [FunctionName("deletevisitor")]
        public static async Task<IActionResult> DeleteVisitor(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestMessage incomingRequest,
            [Blob(VisitorLogXml, FileAccess.ReadWrite)] BlobClient visitorLogClient)
        {
            var responseMessage = "";

            try
            {
                //get unedited hash & updated person details from incoming request
                var requestData = APITools.ExtractDataFromRequest(incomingRequest);
                var visitorId = requestData.Value;

                //get the person record that needs to be deleted
                var visitorLogXml = APITools.BlobClientToXml(visitorLogClient);
                var visitorToDelete = APITools.FindVisitorById(visitorLogXml, visitorId);

                //delete the person record,
                visitorToDelete.Remove();

                //upload modified list to storage
                await APITools.OverwriteBlobData(visitorLogClient, visitorLogXml);

                return PassMessage;

            }
            catch (Exception e)
            {
                //format error nicely to show user
                return APITools.FormatErrorReply(e);
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
                return APITools.FormatErrorReply(e);
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
                return APITools.FormatErrorReply(e);
            }


            var okObjectResult = new OkObjectResult(responseMessage);

            return okObjectResult;
        }

        



        //█▀█ █▀█ █ █░█ ▄▀█ ▀█▀ █▀▀   █▀▄▀█ █▀▀ ▀█▀ █░█ █▀█ █▀▄ █▀
        //█▀▀ █▀▄ █ ▀▄▀ █▀█ ░█░ ██▄   █░▀░█ ██▄ ░█░ █▀█ █▄█ █▄▀ ▄█

        /// <summary>
        /// Generate ready made cache resolutions and saves it
        /// Note: will only return after cache has been saved
        /// </summary>
        private static async Task RefreshDasaReportCache(Person person, BlobClient cachedReportsClient)
        {
            //existing cache has to be cleared first here
            await ClearExistingCacheForUser(person.Hash, cachedReportsClient);

            //if control reaches here, then no cache
            //generate new report (compute)
            var startTime = person.BirthTime;
            var endTime = startTime.AddYears(120);

            var daysPerPixel = new List<double> { 44, 22, 2 };


            Parallel.ForEach(daysPerPixel, async dayPx =>
            {
                var dasaReportSvgString = await GenerateDasaReportSvg(person, startTime, endTime, dayPx);
                //save it in cache for future calls
                //note : needs to await, because caller expects it reports saved on call end
                await SetCachedDasaReportSvg(person.Hash, dayPx, cachedReportsClient, dasaReportSvgString);
            });

            //foreach (var dayPx in daysPerPixel)
            //{
            //    var dasaReportSvgString = await GenerateDasaReportSvg(person, startTime, endTime, dayPx);
            //    //save it in cache for future calls
            //    //note : needs to await, because caller expects it reports saved on call end
            //    await SetCachedDasaReportSvg(person.Hash, dayPx, cachedReportsClient, dasaReportSvgString);
            //}


        }

        private static async Task ClearExistingCacheForUser(int personHash, BlobClient cachedReportsClient)
        {
            var cachedReportXml = APITools.BlobClientToXml(cachedReportsClient);

            //get only the report specified
            var foundList = from report in cachedReportXml.Root?.Elements()
                            where
                                report.Element("PersonHash")?.Value == personHash.ToString()
                            select report;


            //delete each resolution if any
            foreach (var reportXml in foundList)
            {
                reportXml.Remove();
            }

            //upload modified list to storage
            await APITools.OverwriteBlobData(cachedReportsClient, cachedReportXml);

        }

        private static async Task<Stream> GetDasaReportSvgForIncomingRequest(HttpRequestMessage req, BlobClient personListClient)
        {
            //get all the data needed out of the incoming request
            var rootXml = APITools.ExtractDataFromRequest(req);
            var personHash = int.Parse(rootXml.Element("PersonHash").Value);
            var startTimeXml = rootXml.Element("StartTime").Elements().First();
            var startTime = Time.FromXml(startTimeXml);
            var endTimeXml = rootXml.Element("EndTime").Elements().First();
            var endTime = Time.FromXml(endTimeXml);
            var daysPerPixel = double.Parse(rootXml.Element("DaysPerPixel").Value);


            //get the person instance by hash
            var foundPerson = APITools.GetPersonFromHash(personHash, personListClient);

            //from person get svg report
            var dasaReportSvgString = await GenerateDasaReportSvg(foundPerson, startTime, endTime, daysPerPixel);

            //convert svg string to stream for sending
            //todo check if using really needed here
            var stream = GenerateStreamFromString(dasaReportSvgString);

            return stream;
        }

        /// <summary>
        /// note: start time & end time hard set 120 years here
        /// </summary>
        private static async Task<XElement> GetDasaReportSvgForIncomingRequestCached(HttpRequestMessage req, BlobClient personListClient, BlobClient cachedReportsClient)
        {
            //get all the data needed out of the incoming request
            var personHashXml = APITools.ExtractDataFromRequest(req);
            var personHash = int.Parse(personHashXml.Value);

            //get the person instance by hash
            var foundPerson = APITools.GetPersonFromHash(personHash, personListClient);


        TryAgain:
            //use svg from cache if it exists else,
            //make new one save it in cache & send that
            var cacheFound = GetCachedDasaReportSvg(foundPerson, cachedReportsClient, out var cachedReportsXml);
            if (cacheFound) { return cachedReportsXml; }


            //if control reaches here, then no cache
            //generate new report (compute)
            await RefreshDasaReportCache(foundPerson, cachedReportsClient);

            //try again
            goto TryAgain;

        }
        
        /// <summary>
        /// Returns false if no cache found
        /// </summary>
        private static bool GetCachedDasaReportSvg(Person inputPerson, BlobClient cachedReportsClient, out XElement cachedReports)
        {
            //get cached reports from storage
            var reportListXml = APITools.BlobClientToXml(cachedReportsClient);

            //get only the report specified
            var foundList = from report in reportListXml.Root?.Elements()
                            where
                                report.Element("PersonHash")?.Value == inputPerson.Hash.ToString()
                            select report;

            cachedReports = new XElement("Root", foundList);

            //only allowed 1 cache
            //if (foundList.Count() > 1) { throw new Exception("Duplicate Dasa Report Cache!"); }

            //if none found return empty str
            //if (!foundList.Any()) { return ""; }

            //return true if found cache, else false
            return foundList.Any();
        }
        

        private static async Task SetCachedDasaReportSvg(int personHash, double daysPerPixel, BlobClient cachedReportsClient, string svgString)
        {

            var personHashXml = new XElement("PersonHash", personHash);
            var daysPerPixelXml = new XElement("DaysPerPixel", daysPerPixel);
            var svgReportXml = new XElement("SvgReport", svgString);
            var reportXml = new XElement("Report", personHashXml, daysPerPixelXml, svgReportXml);

            //add new visitor to main list
            var taskListXml = APITools.AddXElementToXDocument(cachedReportsClient, reportXml);

            //upload modified list to storage
            await APITools.OverwriteBlobData(cachedReportsClient, taskListXml);

        }


        private static async Task<string> GenerateDasaReportSvg(Person inputPerson, Time startTime, Time endTime, double daysPerPixel)
        {
            // One precision value for generating all dasa components,
            // because misalignment occurs if use different precision
            // note: precision = time slice count, each slice = 1 pixel (zoom level)
            // 120 years @ 14 day/px
            // 1 year @ 1 day/px 
            double eventsPrecisionHours = Tools.DaysToHours(daysPerPixel);
            double _timeSlicePrecision = eventsPrecisionHours;


            //generate time slice only once for all rows
            var timeSlices = EventManager.GetTimeListFromRange(startTime, endTime, _timeSlicePrecision);


            var compiledRow = await GenerateRowsSvg(inputPerson, daysPerPixel, startTime, endTime, timeSlices, eventsPrecisionHours);


            //the height for all lines, cursor, now & life events line
            //place below the last row
            var totalHeight = 197; //hard set for now
            var padding = 2;//space between rows
            var lineHeight = totalHeight + padding;

            //add in the cursor line
            compiledRow += $"<rect id=\"CursorLine\" width=\"2\" height=\"{lineHeight}\" style=\"fill:#000000;\" x=\"0\" y=\"0\" />";

            //get now line position
            var nowLinePosition = GetLinePosition(timeSlices, DateTimeOffset.Now);
            compiledRow += $"<rect id=\"NowVerticalLine\" width=\"2\" height=\"{lineHeight}\" style=\"fill:blue;\" x=\"0\" y=\"0\" transform=\"matrix(1, 0, 0, 1, {nowLinePosition}, 0)\" />";

            //wait!, add in life events also
            compiledRow += GetLifeEventLinesSvg(inputPerson, lineHeight);

            //compile the final svg
            //save a copy of the number of time slices used to calculate the svg total width later
            var dasaSvgWidth = timeSlices.Count;
            var finalSvg = WrapSvgElements(compiledRow, dasaSvgWidth); //little wiggle room

            return finalSvg;



            //█░░ █▀█ █▀▀ ▄▀█ █░░   █▀▀ █░█ █▄░█ █▀▀ ▀█▀ █ █▀█ █▄░█ █▀
            //█▄▄ █▄█ █▄▄ █▀█ █▄▄   █▀░ █▄█ █░▀█ █▄▄ ░█░ █ █▄█ █░▀█ ▄█


            //gets person's life events as lines for the dasa chart
            string GetLifeEventLinesSvg(Person person, int lineHeight)
            {
                var compiledLines = "";

                foreach (var lifeEvent in person.LifeEventList)
                {

                    //get start time of life event and find the position of it in slices (same as now line)
                    //so that this life event line can be placed exactly on the report where it happened
                    var startTime = DateTimeOffset.ParseExact(lifeEvent.StartTime, Time.GetDateTimeFormat(), null);
                    var positionX = GetLinePosition(timeSlices, startTime);

                    //note: this is the icon below the life event line to magnify it
                    var iconWidth = 12;
                    var iconX = $"-{iconWidth}"; //use negative to move center under line
                    var iconSvg = $@"
                                    <g transform=""matrix(2, 0, 0, 2, {iconX}, {lineHeight})"">
                                        <rect style=""fill:{GetColor(lifeEvent.Nature)}; stroke:black; stroke-width: 0.5px;"" x=""0"" y=""0"" width=""{iconWidth}"" height=""9.95"" rx=""2.5"" ry=""2.5""/>
                                        <path d=""M 7.823 5.279 L 6.601 5.279 C 6.377 5.279 6.194 5.456 6.194 5.671 L 6.194 6.846 C 6.194 7.062 6.377 7.238 6.601 7.238 L 7.823 7.238 C 8.046 7.238 8.229 7.062 8.229 6.846 L 8.229 5.671 C 8.229 5.456 8.046 5.279 7.823 5.279 Z M 7.823 1.364 L 7.823 1.756 L 4.566 1.756 L 4.566 1.364 C 4.566 1.148 4.383 0.973 4.158 0.973 C 3.934 0.973 3.751 1.148 3.751 1.364 L 3.751 1.756 L 3.345 1.756 C 2.892 1.756 2.534 2.108 2.534 2.539 L 2.53 8.021 C 2.53 8.454 2.895 8.804 3.345 8.804 L 9.044 8.804 C 9.492 8.804 9.857 8.452 9.857 8.021 L 9.857 2.539 C 9.857 2.108 9.492 1.756 9.044 1.756 L 8.636 1.756 L 8.636 1.364 C 8.636 1.148 8.453 0.973 8.229 0.973 C 8.006 0.973 7.823 1.148 7.823 1.364 Z M 8.636 8.021 L 3.751 8.021 C 3.528 8.021 3.345 7.845 3.345 7.629 L 3.345 3.714 L 9.044 3.714 L 9.044 7.629 C 9.044 7.845 8.86 8.021 8.636 8.021 Z"" />
                                    </g>";

                    //put together icon + line + event data
                    compiledLines += $"<g" +
                                     $" eventName=\"{lifeEvent.Name}\" " +
                                     $" class=\"LifeEventLines\" " + //atm used for tooltip logic
                                     $" stdTime=\"{startTime:dd/MM/yyyy}\" " + //show only date
                                     $" transform=\"matrix(1, 0, 0, 1, {positionX}, 0)\"" +
                                    $" x=\"0\" y=\"0\" >" +
                                        $"<rect" +
                                        $" width=\"2\"" +
                                        $" height=\"{lineHeight}\"" +
                                        $" style=\"" +
                                        $" fill:{GetColor(lifeEvent.Nature)};" +
                                        //border
                                        $" stroke-width:0.5px;" +
                                        $" stroke:#000;" +
                                        $"\"" +
                                        $" />"
                                         + iconSvg +
                                    "</g>";

                    //stroke-width: 0.6px; fill: rgb(168, 0, 0); fill - rule: nonzero; stroke: rgb(0, 0, 0);
                }


                return compiledLines;

                string GetColor(string nature)
                {
                    switch (nature)
                    {
                        case "Good": return "#42f706";
                        //case "Neutral": return "";
                        //case "Bad": return "#a80000";
                        case "Bad": return "#ff5656";
                        default: throw new Exception($"Invalid Nature: {nature}");
                    }
                }
            }

            //gets line position given a date
            //finds most closest time slice, else return 0 means none found
            //note: tries to get nearest day first, then tries month to nearest year
            int GetLinePosition(List<Time> timeSliceList, DateTimeOffset inputTime)
            {
                //if nearest day is possible then end here
                var nearestDay = GetNearestDay();
                if (nearestDay != 0) { return nearestDay; }

                //else try get nearest month
                var nearestMonth = GetNearestMonth();
                if (nearestMonth != 0) { return nearestMonth; }

                //else try get nearest year
                var nearestYear = GetNearestYear();
                if (nearestYear != 0) { return nearestYear; }


                //if control reaches here then now time not found in time slices
                //this is possible when viewing old charts as such set now line to 0
                return 0;

                int GetNearestMonth()
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
                        var sameMonth = time.GetStdMonth() == nowMonth;
                        if (sameMonth && sameYear)
                        {
                            return slicePosition;
                        }

                        //move to next slice position
                        slicePosition++;
                    }

                    return 0;
                }
                int GetNearestDay()
                {
                    var nowDay = inputTime.Day;
                    var nowYear = inputTime.Year;
                    var nowMonth = inputTime.Month;

                    //go through the list and find where the slice is closest to now
                    var slicePosition = 0;
                    foreach (var time in timeSliceList)
                    {

                        //if same year and same month then send this slice position
                        //as the correct one
                        var sameDay = time.GetStdDay() == nowDay;
                        var sameYear = time.GetStdYear() == nowYear;
                        var sameMonth = time.GetStdMonth() == nowMonth;
                        if (sameMonth && sameYear && sameDay)
                        {
                            return slicePosition;
                        }

                        //move to next slice position
                        slicePosition++;
                    }

                    return 0;
                }
                int GetNearestYear()
                {
                    var nowYear = inputTime.Year;

                    //go through the list and find where the slice is closest to now
                    var slicePosition = 0;
                    foreach (var time in timeSliceList)
                    {

                        //if same year and same month then send this slice position
                        //as the correct one
                        var sameYear = time.GetStdYear() == nowYear;
                        if (sameYear)
                        {
                            return slicePosition;
                        }

                        //move to next slice position
                        slicePosition++;
                    }

                    return 0;
                }
            }


            //wraps a list of svg elements inside 1 main svg element
            //if width not set defaults to 1000px, and height to 1000px
            //height is set auto because hard to determine
            string WrapSvgElements(string combinedSvgString, int svgWidth = 1000, int svgTotalHeight = 1000)
            {

                var svgBackgroundColor = "#f0f9ff";

                //create the final svg that will be displayed
                var svgTotalWidth = svgWidth + 10; //add little for wiggle room
                var svgBody = $"<svg id=\"DasaViewHolder\"" +
                              $" width=\"100%\"" +
                              $" height=\"100%\"" +
                              $" style=\"" +
                              $"width:{svgTotalWidth}px;" + //note: if width not hard set, parent div clips it
                                                            //$"height:{svgTotalHeight}px;" +
                              $"background:{svgBackgroundColor};" +
                              $"\" " +
                              $"xmlns=\"http://www.w3.org/2000/svg\">" +
                              $"{combinedSvgString}</svg>";

                return svgBody;
            }


        }


        private static async Task<string> GenerateRowsSvg(Person inputPerson, double daysPerPixel, Time startTime, Time endTime, List<Time> timeSlices, double eventsPrecision)
        {
            //px width & height of each slice of time
            //used when generating dasa rows
            //note: changes needed only here
            int _widthPerSlice = 1;
            int _heightPerSlice = 20;
            //var lineHeight = 170;

            //use the inputed data to get events from API
            //note: below methods access the data internally
            var eventDataList = await APITools.GetEventDataList();
            
            var dasaEventList = APITools.CalculateEvents(eventsPrecision, startTime, endTime, inputPerson.GetBirthLocation(), inputPerson, EventTag.Dasa, eventDataList);
            var bhuktiEventList = APITools.CalculateEvents(eventsPrecision, startTime, endTime, inputPerson.GetBirthLocation(), inputPerson, EventTag.Bhukti, eventDataList);
            var antaramEventList = APITools.CalculateEvents(eventsPrecision, startTime, endTime, inputPerson.GetBirthLocation(), inputPerson, EventTag.Antaram, eventDataList);
            var gocharaEventList = APITools.CalculateEvents(eventsPrecision, startTime, endTime, inputPerson.GetBirthLocation(), inputPerson, EventTag.Gochara, eventDataList);


            //generate rows and pump them final svg string
            var dasaSvgWidth = 0; //will be filled when calling row generator
            var compiledRow = "";


            var beginYear = timeSlices[0].GetStdYear();
            var endYear = timeSlices.Last().GetStdYear();
            var difYears = endYear - beginYear;

            var headerGenerator = new List<Func<List<Time>, int, int, int, string>>();
            if (difYears >= 10) { headerGenerator.Add(GenerateDecadeRowSvg); }
            if (difYears is >= 5 and < 10) { headerGenerator.Add(Generate5YearRowSvg); }
            if (daysPerPixel <= 15) { headerGenerator.Add(GenerateYearRowSvg); }
            if (daysPerPixel <= 1.3) { headerGenerator.Add(GenerateMonthRowSvg); }


            var padding = 2;//space between rows
            int headerY = 0, headerHeight = 11;
            foreach (var generator in headerGenerator)
            {
                compiledRow += generator(timeSlices, headerY, 0, headerHeight);

                //update for next generator
                headerY = headerY + headerHeight + padding;
            }


            int dasaY = headerY;
            compiledRow += GenerateDasaRowSvg(dasaEventList, timeSlices, dasaY, 0, _heightPerSlice);
            int bhuktiY = dasaY + _heightPerSlice + padding;
            compiledRow += GenerateBhuktiRowSvg(bhuktiEventList, timeSlices, bhuktiY, 0, _heightPerSlice);
            int antaramY = bhuktiY + _heightPerSlice + padding;
            compiledRow += GenerateAntaramRowSvg(antaramEventList, timeSlices, antaramY, 0, _heightPerSlice);
            int gocharaY = antaramY + _heightPerSlice + padding;
            compiledRow += GenerateGocharaSvg(gocharaEventList, timeSlices, eventsPrecision, gocharaY, 0, out int gocharaHeight);

            //future passed to caller to draw line
            var totalHeight = gocharaY + gocharaHeight;



            return compiledRow;



            
            //█░░ █▀█ █▀▀ ▄▀█ █░░   █▀▀ █░█ █▄░█ █▀▀ ▀█▀ █ █▀█ █▄░█ █▀
            //█▄▄ █▄█ █▄▄ █▀█ █▄▄   █▀░ █▄█ █░▀█ █▄▄ ░█░ █ █▄█ █░▀█ ▄█



            string GenerateAntaramRowSvg(List<Event> eventList, List<Time> timeSlices, int yAxis, int xAxis, int rowHeight)
            {
                //generate the row for each time slice
                var rowHtml = "";
                var horizontalPosition = 0; //distance from left
                var prevEventName = EventName.EmptyEvent;

                //generate 1px (rect) per time slice
                foreach (var slice in timeSlices)
                {
                    //get event that occurred at this time slice
                    //if more than 1 event raise alarm, since 1px (rect) is equal to 1 event at a time 
                    var foundEventList = eventList.FindAll(tempEvent => tempEvent.IsOccurredAtTime(slice));
                    if (foundEventList.Count > 1) throw new Exception("Only 1 event in 1 time slice!");
                    var foundEvent = foundEventList[0];

                    //if current event is different than event has changed, so draw a black line
                    var isNewEvent = prevEventName != foundEvent.Name;
                    var borderRoomAvailable = daysPerPixel <= 10; //above 10px/day borders look ugly
                    var color = isNewEvent && borderRoomAvailable ? "black" : GetEventColor(foundEvent?.Nature);
                    prevEventName = foundEvent.Name;

                    //generate and add to row
                    //the hard coded attribute names used here are used in App.js
                    var rect = $"<rect " +
                               $"type=\"Antaram\" " +
                               $"eventname=\"{foundEvent?.FormattedName}\" " +
                               $"age=\"{inputPerson.GetAge(slice)}\" " +
                               $"stdtime=\"{slice.GetStdDateTimeOffset():dd/MM/yyyy}\" " + //show only date
                               $"x=\"{horizontalPosition}\" " +
                               $"width=\"{_widthPerSlice}\" " +
                               $"height=\"{rowHeight}\" " +
                               $"fill=\"{color}\" />";

                    //set position for next element
                    horizontalPosition += _widthPerSlice;

                    rowHtml += rect;

                }

                //wrap all the rects inside a svg so they can me moved together
                //svg tag here acts as group, svg nesting
                rowHtml = $"<g transform=\"matrix(1, 0, 0, 1, {xAxis}, {yAxis})\">{rowHtml}</g>";


                return rowHtml;
            }
            string GenerateDasaRowSvg(List<Event> eventList, List<Time> timeSlices, int yAxis, int xAxis, int rowHeight)
            {
                //generate the row for each time slice
                var rowHtml = "";
                var horizontalPosition = 0; //distance from left
                var prevEventName = EventName.EmptyEvent;

                //generate 1px (rect) per time slice
                foreach (var slice in timeSlices)
                {
                    //get event that occurred at this time slice
                    //if more than 1 event raise alarm, since 1px (rect) is equal to 1 event at a time 
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
                               $"type=\"Dasa\" " +
                               $"eventname=\"{foundEvent?.FormattedName}\" " +
                               $"age=\"{inputPerson.GetAge(slice)}\" " +
                               $"stdtime=\"{slice.GetStdDateTimeOffset():dd/MM/yyyy}\" " + //show only date
                               $"x=\"{horizontalPosition}\" " +
                               $"width=\"{_widthPerSlice}\" " +
                               $"height=\"{rowHeight}\" " +
                               $"fill=\"{color}\" />";

                    //set position for next element
                    horizontalPosition += _widthPerSlice;

                    rowHtml += rect;

                }

                //wrap all the rects inside a svg so they can me moved together
                //svg tag here acts as group, svg nesting
                rowHtml = $"<g transform=\"matrix(1, 0, 0, 1, {xAxis}, {yAxis})\">{rowHtml}</g>";


                return rowHtml;
            }
            string GenerateBhuktiRowSvg(List<Event> eventList, List<Time> timeSlices, int yAxis, int xAxis, int rowHeight)
            {
                //generate the row for each time slice
                var rowHtml = "";
                var horizontalPosition = 0; //distance from left
                var prevEventName = EventName.EmptyEvent;

                //generate 1px (rect) per time slice
                foreach (var slice in timeSlices)
                {
                    //get event that occurred at this time slice
                    //if more than 1 event raise alarm, since 1px (rect) is equal to 1 event at a time 
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
                               $"type=\"Bhukti\" " +
                               $"eventname=\"{foundEvent?.FormattedName}\" " +
                               $"age=\"{inputPerson.GetAge(slice)}\" " +
                               $"stdtime=\"{slice.GetStdDateTimeOffset():dd/MM/yyyy}\" " + //show only date
                               $"x=\"{horizontalPosition}\" " +
                               $"width=\"{_widthPerSlice}\" " +
                               $"height=\"{rowHeight}\" " +
                               $"fill=\"{color}\" />";

                    //set position for next element
                    horizontalPosition += _widthPerSlice;

                    rowHtml += rect;

                }

                //wrap all the rects inside a svg so they can me moved together
                //svg tag here acts as group, svg nesting
                rowHtml = $"<g transform=\"matrix(1, 0, 0, 1, {xAxis}, {yAxis})\">{rowHtml}</g>";


                return rowHtml;
            }

            //height not known until generated
            //returns the final dynamic height of this gochara row
            string GenerateGocharaSvg(List<Event> eventList, List<Time> timeSlices, double precisionHours, int yAxis, int xAxis, out int gocharaHeight)
            {
                //generate the row for each time slice
                var rowHtml = "";
                var horizontalPosition = 0; //distance from left
                var verticalPosition = 0; //distance from top
                var prevEventName = EventName.EmptyEvent;

                //height of each row
                var rowHeight = 15;

                //used to determine final height
                var highestTimeSlice = 0;
                var multipleEventCount = 0;

                //generate 1px (rect) per time slice
                foreach (var slice in timeSlices)
                {
                    //get event that occurred at this time slice
                    //if more than 1 event raise alarm, since 1px (rect) is equal to 1 event at a time 
                    var foundEventList = eventList.FindAll(tempEvent => tempEvent.IsOccurredAtTime(slice));
                    //if (foundEventList.Count > 1) throw new Exception("Only 1 event in 1 time slice!");
                    //var foundEvent = foundEventList[0];


                    foreach (var foundEvent in foundEventList)
                    {
                        ////if current event is different than event has changed, so draw a black line
                        //var isNewEvent = prevEventName != foundEvent.Name;
                        //var color = isNewEvent ? "black" : GetEventColor(foundEvent?.Nature);
                        //prevEventName = foundEvent.Name;
                        var color = GetEventColor(foundEvent?.Nature);

                        //generate and add to row
                        //the hard coded attribute names used here are used in App.js
                        var rect = $"<rect " +
                                   $"type=\"Gochara\" " +
                                   $"eventname=\"{foundEvent?.FormattedName}\" " +
                                   $"age=\"{inputPerson.GetAge(slice)}\" " +
                                   $"stdtime=\"{slice.GetStdDateTimeOffset():dd/MM/yyyy}\" " + //show only date
                                   $"x=\"{horizontalPosition}\" " +
                                   $"y=\"{verticalPosition}\" " +
                                   $"width=\"{_widthPerSlice}\" " +
                                   $"height=\"{rowHeight}\" " +
                                   $"fill=\"{color}\" />";

                        rowHtml += rect;

                        //increment vertical position for next
                        //element to be placed beneath this one
                        var spaceBetweenRow = 1;
                        verticalPosition += rowHeight + spaceBetweenRow;

                        multipleEventCount++; //include this in count
                    }

                    //set position for next element in time slice
                    horizontalPosition += _widthPerSlice;

                    //reset vertical position for next time slice
                    verticalPosition = 0;

                    //safe only the highest row
                    var thisSliceHeight = multipleEventCount * rowHeight;
                    highestTimeSlice = thisSliceHeight > highestTimeSlice ? thisSliceHeight : highestTimeSlice;
                    multipleEventCount = 0; //reset

                }

                //wrap all the rects inside a svg so they can me moved together
                //note: use group instead of svg because editing capabilities
                rowHtml = $"<g transform=\"matrix(1, 0, 0, 1, {xAxis}, {yAxis})\">{rowHtml}</g>";

                //send height of tallest time slice aka the
                //final height of this gochara row to caller
                gocharaHeight = highestTimeSlice;

                return rowHtml;
            }

            string GenerateYearRowSvg(List<Time> timeSlices, int yAxis, int xAxis, int rowHeight)
            {

                //generate the row for each time slice
                var rowHtml = "";
                var previousYear = 0; //start 0 for first draw
                var yearBoxWidthCount = 0;
                int rectWidth = 0;
                int childAxisX = 0;
                //int rowHeight = 11;

                foreach (var slice in timeSlices)
                {

                    //only generate new year box when year changes or at
                    //end of time slices to draw the last year box
                    var lastTimeSlice = timeSlices.IndexOf(slice) == timeSlices.Count - 1;
                    var yearChanged = previousYear != slice.GetStdYear();
                    if (yearChanged || lastTimeSlice)
                    {
                        //and it is in the beginning
                        if (previousYear == 0)
                        {
                            yearBoxWidthCount = 0; //reset width
                        }
                        else
                        {
                            //generate previous year data first before resetting
                            childAxisX += rectWidth; //use previous rect width to position this
                            rectWidth = yearBoxWidthCount * _widthPerSlice; //calculate new rect width
                            var textX = rectWidth / 2; //center of box divide 2
                            var rect = $"<g transform=\"matrix(1, 0, 0, 1, {childAxisX}, 0)\">" + //y is 0 because already set in parent group
                                                $"<rect " +
                                                    $"fill=\"#0d6efd\" x=\"0\" y=\"0\" width=\"{rectWidth}\" height=\"{rowHeight}\" " + $" style=\"paint-order: stroke; stroke: rgb(255, 255, 255); stroke-opacity: 1; stroke-linejoin: round;\"/>" +
                                                    $"<text x=\"{textX}\" y=\"{9}\" width=\"{rectWidth}\" fill=\"white\"" +
                                                        $" style=\"fill: rgb(255, 255, 255);" +
                                                        $" font-size: 10px;" +
                                                        $" font-weight: 700;" +
                                                        $" text-anchor: middle;" +
                                                        $" white-space: pre;\"" +
                                                        //$" transform=\"matrix(0.966483, 0, 0, 0.879956, 2, -6.779947)\"" +
                                                        $">" +
                                                        $"{previousYear}" + //previous year generate at begin of new year
                                                    $"</text>" +
                                             $"</g>";


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
                rowHtml = $"<g transform=\"matrix(1, 0, 0, 1, {xAxis}, {yAxis})\">{rowHtml}</g>";

                return rowHtml;
            }

            string GenerateDecadeRowSvg(List<Time> timeSlices, int yAxis, int xAxis, int rowHeight)
            {

                //generate the row for each time slice
                var rowHtml = "";
                var previousYear = 0; //start 0 for first draw
                var yearBoxWidthCount = 0;
                int rectWidth = 0;
                int childAxisX = 0;
                //int rowHeight = 11;

                var beginYear = timeSlices[0].GetStdYear();
                var endYear = beginYear + 10; //decade


                foreach (var slice in timeSlices)
                {

                    //only generate new year box when year changes or at
                    //end of time slices to draw the last year box
                    var lastTimeSlice = timeSlices.IndexOf(slice) == timeSlices.Count - 1;
                    var yearChanged = previousYear != slice.GetStdYear();
                    if (yearChanged || lastTimeSlice)
                    {
                        //is this slice end year
                        var isEndYear = endYear == slice.GetStdYear();
                        if (isEndYear)
                        {
                            //generate previous year data first before resetting
                            childAxisX += rectWidth; //use previous rect width to position this
                            rectWidth = yearBoxWidthCount * _widthPerSlice; //calculate new rect width
                            var textX = rectWidth / 2; //center of box divide 2
                            var rect = $"<g transform=\"matrix(1, 0, 0, 1, {childAxisX}, 0)\">" + //y is 0 because already set in parent group
                                       $"<rect " +
                                       $"fill=\"#0d6efd\" x=\"0\" y=\"0\" width=\"{rectWidth}\" height=\"{rowHeight}\" " + $" style=\"paint-order: stroke; stroke: rgb(255, 255, 255); stroke-opacity: 1; stroke-linejoin: round;\"/>" +
                                       $"<text x=\"{textX}\" y=\"{9}\" width=\"{rectWidth}\" fill=\"white\"" +
                                       $" style=\"fill: rgb(255, 255, 255);" +
                                       $" font-size: 10px;" +
                                       $" font-weight: 700;" +
                                       $" text-anchor: middle;" +
                                       $" white-space: pre;\"" +
                                       //$" transform=\"matrix(0.966483, 0, 0, 0.879956, 2, -6.779947)\"" +
                                       $">" +
                                       $"{beginYear} - {endYear}" + //previous year generate at begin of new year
                                       $"</text>" +
                                       $"</g>";


                            //add to final return
                            rowHtml += rect;

                            //reset width
                            yearBoxWidthCount = 0;

                            //set new begin & end
                            beginYear = endYear + 1;
                            endYear = beginYear + 10;

                        }

                    }

                    //update previous year for next slice
                    previousYear = slice.GetStdYear();

                    yearBoxWidthCount++;

                }

                //wrap all the rects inside a svg so they can me moved together
                //svg tag here acts as group, svg nesting
                rowHtml = $"<g transform=\"matrix(1, 0, 0, 1, {xAxis}, {yAxis})\">{rowHtml}</g>";

                return rowHtml;
            }

            string Generate5YearRowSvg(List<Time> timeSlices, int yAxis, int xAxis, int rowHeight)
            {

                //generate the row for each time slice
                var rowHtml = "";
                var previousYear = 0; //start 0 for first draw
                var yearBoxWidthCount = 0;
                int rectWidth = 0;
                int childAxisX = 0;
                //int rowHeight = 11;

                const int yearRange = 5;

                var beginYear = timeSlices[0].GetStdYear();
                var endYear = beginYear + yearRange;


                foreach (var slice in timeSlices)
                {

                    //only generate new year box when year changes or at
                    //end of time slices to draw the last year box
                    var lastTimeSlice = timeSlices.IndexOf(slice) == timeSlices.Count - 1;
                    var yearChanged = previousYear != slice.GetStdYear();
                    if (yearChanged || lastTimeSlice)
                    {
                        //is this slice end year
                        var isEndYear = endYear == slice.GetStdYear();
                        if (isEndYear)
                        {
                            //generate previous year data first before resetting
                            childAxisX += rectWidth; //use previous rect width to position this
                            rectWidth = yearBoxWidthCount * _widthPerSlice; //calculate new rect width
                            var textX = rectWidth / 2; //center of box divide 2
                            var rect = $"<g transform=\"matrix(1, 0, 0, 1, {childAxisX}, 0)\">" + //y is 0 because already set in parent group
                                       $"<rect " +
                                       $"fill=\"#0d6efd\" x=\"0\" y=\"0\" width=\"{rectWidth}\" height=\"{rowHeight}\" " + $" style=\"paint-order: stroke; stroke: rgb(255, 255, 255); stroke-opacity: 1; stroke-linejoin: round;\"/>" +
                                       $"<text x=\"{textX}\" y=\"{9}\" width=\"{rectWidth}\" fill=\"white\"" +
                                       $" style=\"fill: rgb(255, 255, 255);" +
                                       $" font-size: 10px;" +
                                       $" font-weight: 700;" +
                                       $" text-anchor: middle;" +
                                       $" white-space: pre;\"" +
                                       //$" transform=\"matrix(0.966483, 0, 0, 0.879956, 2, -6.779947)\"" +
                                       $">" +
                                       $"{beginYear} - {endYear}" + //previous year generate at begin of new year
                                       $"</text>" +
                                       $"</g>";


                            //add to final return
                            rowHtml += rect;

                            //reset width
                            yearBoxWidthCount = 0;

                            //set new begin & end
                            beginYear = endYear + 1;
                            endYear = beginYear + yearRange;

                        }

                    }

                    //update previous year for next slice
                    previousYear = slice.GetStdYear();

                    yearBoxWidthCount++;

                }

                //wrap all the rects inside a svg so they can me moved together
                //svg tag here acts as group, svg nesting
                rowHtml = $"<g transform=\"matrix(1, 0, 0, 1, {xAxis}, {yAxis})\">{rowHtml}</g>";

                return rowHtml;
            }

            string GenerateMonthRowSvg(List<Time> timeSlices, int yAxis, int xAxis, int rowHeight)
            {

                //generate the row for each time slice
                var rowHtml = "";
                var previousMonth = 0; //start 0 for first draw
                var yearBoxWidthCount = 0;
                int rectWidth = 0;
                int childAxisX = 0;
                //int rowHeight = 11;

                foreach (var slice in timeSlices)
                {

                    //only generate new year box when year changes or at
                    //end of time slices to draw the last year box
                    var lastTimeSlice = timeSlices.IndexOf(slice) == timeSlices.Count - 1;
                    var monthChanged = previousMonth != slice.GetStdMonth();
                    if (monthChanged || lastTimeSlice)
                    {
                        //and it is in the beginning
                        if (previousMonth == 0)
                        {
                            yearBoxWidthCount = 0; //reset width
                        }
                        else
                        {
                            //generate previous month data first before resetting
                            childAxisX += rectWidth; //use previous rect width to position this
                            rectWidth = yearBoxWidthCount * _widthPerSlice; //calculate new rect width
                            var textX = rectWidth / 2; //center of box divide 2
                            var rect = $"<g transform=\"matrix(1, 0, 0, 1, {childAxisX}, 0)\">" + //y is 0 because already set in parent group
                                       $"<rect " +
                                       $"fill=\"#0d6efd\" x=\"0\" y=\"0\" width=\"{rectWidth}\" height=\"{rowHeight}\" " + $" style=\"paint-order: stroke; stroke: rgb(255, 255, 255); stroke-opacity: 1; stroke-linejoin: round;\"/>" +
                                       $"<text x=\"{textX}\" y=\"{9}\" width=\"{rectWidth}\" fill=\"white\"" +
                                       $" style=\"fill: rgb(255, 255, 255);" +
                                       $" font-size: 10px;" +
                                       $" font-weight: 700;" +
                                       $" text-anchor: middle;" +
                                       $" white-space: pre;\"" +
                                       //$" transform=\"matrix(0.966483, 0, 0, 0.879956, 2, -6.779947)\"" +
                                       $">" +
                                       $"{GetMonthName(previousMonth)}" + //previous year generate at begin of new year
                                       $"</text>" +
                                       $"</g>";


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

                    //update previous month for next slice
                    previousMonth = slice.GetStdMonth();

                    yearBoxWidthCount++;


                }

                //wrap all the rects inside a svg so they can me moved together
                //svg tag here acts as group, svg nesting
                rowHtml = $"<g transform=\"matrix(1, 0, 0, 1, {xAxis}, {yAxis})\">{rowHtml}</g>";

                return rowHtml;

                string GetMonthName(int monthNum)
                {
                    switch (monthNum)
                    {
                        case 1: return "JAN";
                        case 2: return "FEB";
                        case 3: return "MAR";
                        case 4: return "APR";
                        case 5: return "MAY";
                        case 6: return "JUN";
                        case 7: return "JUL";
                        case 8: return "AUG";
                        case 9: return "SEP";
                        case 10: return "OCT";
                        case 11: return "NOV";
                        case 12: return "DEC";
                        default: throw new Exception($"Invalid Month: {monthNum}");
                    }
                }
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

        }


        private static byte[] StreamToByteArray(Stream input)
        {
            //reset stream position
            input.Position = 0;
            MemoryStream ms = new MemoryStream();
            input.CopyTo(ms);
            return ms.ToArray();
        }

        private static Stream GenerateStreamFromString(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }



    }
}
