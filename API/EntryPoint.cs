using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Xml.Linq;
using Azure.Storage.Blobs;
using Genso.Astrology.Library;
using Genso.Astrology.Library.Compatibility;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;

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
        private const string ContainerName = "vedastro-site-data";


        /// <summary>
        /// Default success message sent to caller
        /// </summary>
        private static OkObjectResult PassMessage(string msg = "") => new(new XElement("Root", new XElement("Status", "Pass"), new XElement("Payload", msg)).ToString());
        private static OkObjectResult PassMessage(XElement msgXml) => new(new XElement("Root", new XElement("Status", "Pass"), new XElement("Payload", msgXml)).ToString());
        private static OkObjectResult FailMessage(string msg = "") => new(new XElement("Root", new XElement("Status", "Fail"), new XElement("Payload", msg)).ToString());
        private static OkObjectResult FailMessage(XElement msgXml) => new(new XElement("Root", new XElement("Status", "Fail"), new XElement("Payload", msgXml)).ToString());




        //▄▀█ █▀█ █   █▀▀ █░█ █▄░█ █▀▀ ▀█▀ █ █▀█ █▄░█ █▀
        //█▀█ █▀▀ █   █▀░ █▄█ █░▀█ █▄▄ ░█░ █ █▄█ █░▀█ ▄█


        [FunctionName("getmatchreport")]
        public static async Task<IActionResult> GetMatchReport(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestMessage incomingRequest,
            [Blob(PersonListXml, FileAccess.Read)] Stream personListRead)
        {
            string responseMessage;

            try
            {
                //get name of male & female
                var rootXml = APITools.ExtractDataFromRequest(incomingRequest);
                var maleHash = int.Parse(rootXml.Element("MaleHash").Value);
                var femaleHash = int.Parse(rootXml.Element("FemaleHash").Value);


                //get list of all people
                var personList = new Data(personListRead);

                //generate compatibility report
                CompatibilityReport compatibilityReport = APITools.GetCompatibilityReport(maleHash, femaleHash, personList);
                responseMessage = compatibilityReport.ToXml().ToString();
            }
            catch (Exception e)
            {
                //log error
                await Log.Error(e, incomingRequest);
                //format error nicely to show user
                return APITools.FormatErrorReply(e);
            }


            var okObjectResult = new OkObjectResult(responseMessage);
            return okObjectResult;
        }

        [FunctionName("addperson")]
        public static async Task<IActionResult> AddPerson(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestMessage incomingRequest)
        {

            try
            {
                //get new person data out of incoming request
                //note: inside new person xml already contains user id
                var newPersonXml = APITools.ExtractDataFromRequest(incomingRequest);

                //add new person to main list
                await APITools.AddXElementToXDocument(newPersonXml, "PersonList.xml", ContainerName);

                return PassMessage();

            }
            catch (Exception e)
            {
                //log error
                await Log.Error(e, incomingRequest);

                //format error nicely to show user
                return APITools.FormatErrorReply(e);
            }

        }

        [FunctionName("addmessage")]
        public static async Task<IActionResult> AddMessage(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestMessage incomingRequest,
            [Blob(MessageListXml, FileAccess.ReadWrite)] BlobClient messageListClient)
        {

            try
            {
                //get new message data out of incoming request
                //note: inside new person xml already contains user id
                var newMessageXml = APITools.ExtractDataFromRequest(incomingRequest);

                //add new message to main list
                var messageListXml = APITools.AddXElementToXDocument(messageListClient, newMessageXml);

                //upload modified list to storage
                await APITools.OverwriteBlobData(messageListClient, messageListXml);

                return PassMessage();

            }
            catch (Exception e)
            {
                //log error
                await Log.Error(e, incomingRequest);

                //format error nicely to show user
                return APITools.FormatErrorReply(e);
            }
        }

        [FunctionName("addtask")]
        public static async Task<IActionResult> AddTask(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestMessage incomingRequest,
            [Blob(TaskListXml, FileAccess.ReadWrite)] BlobClient taskListClient)
        {
            try
            {
                //get new task data out of incoming request 
                var newTaskXml = APITools.ExtractDataFromRequest(incomingRequest);

                //add new task to main list
                var taskListXml = APITools.AddXElementToXDocument(taskListClient, newTaskXml);

                //upload modified list to storage
                await APITools.OverwriteBlobData(taskListClient, taskListXml);

                return PassMessage();

            }
            catch (Exception e)
            {
                //log error
                await Log.Error(e, incomingRequest);

                //format error nicely to show user
                return APITools.FormatErrorReply(e);
            }

        }

        [FunctionName("addvisitor")]
        public static async Task<IActionResult> AddVisitor(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestMessage incomingRequest,
            [Blob(VisitorLogXml, FileAccess.ReadWrite)] BlobClient visitorLogClient)
        {
            try
            {
                //get new visitor data out of incoming request 
                var newVisitorXml = APITools.ExtractDataFromRequest(incomingRequest);

                //add new visitor to main list
                var taskListXml = APITools.AddXElementToXDocument(visitorLogClient, newVisitorXml);

                //upload modified list to storage
                await APITools.OverwriteBlobData(visitorLogClient, taskListXml);

                return PassMessage();

            }
            catch (Exception e)
            {
                //log error
                await Log.Error(e, incomingRequest);

                //format error nicely to show user
                return APITools.FormatErrorReply(e);
            }

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
                //log error
                await Log.Error(e, incomingRequest);

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
                //var uniqueVisitorList = from visitorXml in visitorLogXml.Root?.Elements()
                //                        where
                //                            //note: location tag only exists for new visitor log,
                //                            //so use that to get unique list
                //                            visitorXml.Element("Location") != null
                //                        select visitorXml;

                //send list to caller
                responseMessage = visitorLogXml.ToString();

            }
            catch (Exception e)
            {
                //log error
                await Log.Error(e, incomingRequest);

                //format error nicely to show user
                return APITools.FormatErrorReply(e);
            }


            var okObjectResult = new OkObjectResult(responseMessage);

            return okObjectResult;
        }

        /// <summary>
        /// Function for debugging purposes
        /// </summary>
        [FunctionName("getipaddress")]
        public static async Task<IActionResult> GetIpAddress(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]
            HttpRequestMessage incomingRequest)
        {
            return PassMessage(incomingRequest.GetCallerIp().ToString());
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
                //log error
                await Log.Error(e, incomingRequest);

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
                var foundPerson = await APITools.FindPersonByHash(personListXml, originalHash);

                //send person to caller
                responseMessage = new XElement("Root", foundPerson).ToString();

            }
            catch (Exception e)
            {
                //log error
                await Log.Error(e, incomingRequest);

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
        [FunctionName("getpersoneventsreport")]
        public static async Task<IActionResult> GetPersonEventsReport(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestMessage incomingRequest,
            [Blob(PersonListXml, FileAccess.ReadWrite)] BlobClient personListClient)
        {

            try
            {

                //get dasa report for sending
                var dasaReportSvg = await GetEventReportSvgForIncomingRequest(incomingRequest, personListClient);

                //send image back to caller
                var x = StreamToByteArray(dasaReportSvg);
                return new FileContentResult(x, "image/svg+xml");

            }
            catch (Exception e)
            {
                //log error
                await Log.Error(e, incomingRequest);

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
                //log error
                await Log.Error(e, incomingRequest);

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

            try
            {
                //get unedited hash & updated person details from incoming request
                var requestData = APITools.ExtractDataFromRequest(incomingRequest);
                var originalHash = int.Parse(requestData?.Element("PersonHash").Value);
                var updatedPersonXml = requestData?.Element("Person");

                //get the person record that needs to be updated
                var personListXml = APITools.BlobClientToXml(personListClient);
                var personToUpdate = await APITools.FindPersonByHash(personListXml, originalHash);

                //delete the previous person record,
                //and insert updated record in the same place
                personToUpdate.ReplaceWith(updatedPersonXml);

                //upload modified list to storage
                await APITools.OverwriteBlobData(personListClient, personListXml);

                return PassMessage();

            }
            catch (Exception e)
            {
                //log error
                await Log.Error(e, incomingRequest);

                //format error nicely to show user
                return APITools.FormatErrorReply(e);
            }

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

            try
            {
                //get unedited hash & updated person details from incoming request
                var requestData = APITools.ExtractDataFromRequest(incomingRequest);
                var originalHash = int.Parse(requestData.Value);

                //get the person record that needs to be deleted
                var personListXml = APITools.BlobClientToXml(personListClient);
                var personToDelete = await APITools.FindPersonByHash(personListXml, originalHash);

                //delete the person record,
                personToDelete.Remove();

                //upload modified list to storage
                await APITools.OverwriteBlobData(personListClient, personListXml);

                return PassMessage();

            }
            catch (Exception e)
            {
                //log error
                await Log.Error(e, incomingRequest);

                //format error nicely to show user
                return APITools.FormatErrorReply(e);
            }
        }

        [FunctionName("deletevisitorbyuserid")]
        public static async Task<IActionResult> DeleteVisitorByUserId(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestMessage incomingRequest,
            [Blob(VisitorLogXml, FileAccess.ReadWrite)] BlobClient visitorLogClient)
        {

            try
            {
                //get unedited hash & updated person details from incoming request
                var userIdXml = APITools.ExtractDataFromRequest(incomingRequest);
                var userId = userIdXml.Value;

                //get all visitor elements that needs to be deleted
                var visitorListXml = APITools.BlobClientToXml(visitorLogClient);
                var visitorLogsToDelete = visitorListXml.Root?.Elements().Where(x => x.Element("UserId")?.Value == userId).ToList();

                //delete each record
                foreach (var visitorXml in visitorLogsToDelete)
                {
                    visitorXml.Remove();
                }

                //upload modified list to storage
                await APITools.OverwriteBlobData(visitorLogClient, visitorListXml);

                return PassMessage();

            }
            catch (Exception e)
            {
                //log error
                await Log.Error(e, incomingRequest);
                //format error nicely to show user
                return APITools.FormatErrorReply(e);
            }

        }

        [FunctionName("deletevisitorbyvisitorid")]
        public static async Task<IActionResult> DeleteVisitorByVisitorId(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestMessage incomingRequest,
            [Blob(VisitorLogXml, FileAccess.ReadWrite)] BlobClient visitorLogClient)
        {

            try
            {
                //get unedited hash & updated person details from incoming request
                var visitorIdXml = APITools.ExtractDataFromRequest(incomingRequest);
                var visitorId = visitorIdXml.Value;

                //get all visitor elements that needs to be deleted
                var visitorListXml = APITools.BlobClientToXml(visitorLogClient);
                var visitorLogsToDelete = (from xml in visitorListXml.Root?.Elements()
                                           where xml.Element("VisitorId")?.Value == visitorId
                                           select xml).ToList();

                //delete each record
                foreach (var visitorXml in visitorLogsToDelete)
                {
                    visitorXml.Remove();
                }

                //upload modified list to storage
                await APITools.OverwriteBlobData(visitorLogClient, visitorListXml);

                return PassMessage();

            }
            catch (Exception e)
            {
                //log error
                await Log.Error(e, incomingRequest);
                //format error nicely to show user
                return APITools.FormatErrorReply(e);
            }

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
                //log error
                await Log.Error(e, incomingRequest);
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
                //log error
                await Log.Error(e, incomingRequest);
                //format error nicely to show user
                return APITools.FormatErrorReply(e);
            }


            var okObjectResult = new OkObjectResult(responseMessage);

            return okObjectResult;
        }

        [FunctionName("SignInGoogle")]
        public static async Task<IActionResult> SignInGoogle([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestMessage incomingRequest)
        {

            //get ID Token/JWT from received request
            var tokenXml = APITools.ExtractDataFromRequest(incomingRequest);
            var jwtToken = tokenXml.Value;

            try
            {
                //validate the the token & get data to id the user
                var validPayload = await GoogleJsonWebSignature.ValidateAsync(jwtToken);
                var userId = validPayload.Subject; //Unique Google User ID
                var userName = validPayload.Name;
                var userEmail = validPayload.Email;

                //use the email to get the user's record (or make new one if don't exist)
                var userData = await Storage.GetUserData(userId, userName, userEmail);

                //todo add login to users log (browser, location, time)
                //todo maybe better in client
                //userData.AddLoginLog();
                //save updated user data

                //send user data as xml in with pass status
                //so that client can generate hash and use it
                return PassMessage(userData.ToXml());
            }
            //if any failure, reply as in valid login & log the event
            catch (Exception e)
            {
                await Log.Error(e, incomingRequest);
                return FailMessage("Login Failed");
            }
        }

        [FunctionName("SignInFacebook")]
        public static async Task<IActionResult> SignInFacebook(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestMessage incomingRequest)
        {

            try
            {
                //get accessToken from received request
                var tokenXml = APITools.ExtractDataFromRequest(incomingRequest);
                var accessToken = tokenXml.Value;

                //validate the the token & get user data
                var url = $"https://graph.facebook.com/me/?fields=id,name,email&access_token={accessToken}";
                var reply = await GetRequest(url);
                var jsonText = await reply.Content.ReadAsStringAsync();
                var json = JsonNode.Parse(jsonText);
                var userId = json["id"].ToString();
                var userName = json["name"].ToString();
                var userEmail = json["email"].ToString();

                //use the email to get the user's record (or make new one if don't exist)
                var userData = await Storage.GetUserData(userId, userName, userEmail);

                //send user data as xml in with pass status
                //so that client can generate hash and use it
                return PassMessage(userData.ToXml());
            }
            //if any failure, reply as in valid login & log the event
            catch (Exception e)
            {
                await Log.Error(e, incomingRequest);
                return FailMessage("Login Failed");
            }
        }


        //█▀█ █▀█ █ █░█ ▄▀█ ▀█▀ █▀▀   █▀▄▀█ █▀▀ ▀█▀ █░█ █▀█ █▀▄ █▀
        //█▀▀ █▀▄ █ ▀▄▀ █▀█ ░█░ ██▄   █░▀░█ ██▄ ░█░ █▀█ █▄█ █▄▀ ▄█


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


        private static async Task<HttpResponseMessage> GetRequest(string receiverAddress)
        {
            //prepare the data to be sent
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, receiverAddress);

            //get the data sender
            using var client = new HttpClient();

            //tell sender to wait for complete reply before exiting
            var waitForContent = HttpCompletionOption.ResponseContentRead;

            //send the data on its way
            var response = await client.SendAsync(httpRequestMessage, waitForContent);

            //return the raw reply to caller
            return response;
        }

        /// <summary>
        /// processes incoming xml request and outputs events svg report
        /// </summary>
        private static async Task<Stream> GetEventReportSvgForIncomingRequest(HttpRequestMessage req, BlobClient personListClient)
        {
            //get all the data needed out of the incoming request
            var rootXml = APITools.ExtractDataFromRequest(req);
            var personHash = int.Parse(rootXml.Element("PersonHash").Value);
            var eventTagListXml = rootXml.Element("EventTagList");
            var eventTags = EventTagExtensions.FromXmlList(eventTagListXml);
            var startTimeXml = rootXml.Element("StartTime").Elements().First();
            var startTime = Time.FromXml(startTimeXml);
            var endTimeXml = rootXml.Element("EndTime").Elements().First();
            var endTime = Time.FromXml(endTimeXml);
            var daysPerPixel = double.Parse(rootXml.Element("DaysPerPixel").Value);


            //get the person instance by hash
            var foundPerson = await APITools.GetPersonFromHash(personHash, personListClient);

            //from person get svg report
            var eventsReportSvgString = GenerateMainEventsReportSvg(foundPerson, startTime, endTime, daysPerPixel, eventTags);

            //convert svg string to stream for sending
            //todo check if using really needed here
            var stream = GenerateStreamFromString(eventsReportSvgString);

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
            var foundPerson = await APITools.GetPersonFromHash(personHash, personListClient);


        TryAgain:
            //use svg from cache if it exists else,
            //make new one save it in cache & send that
            var cacheFound = GetCachedDasaReportSvg(foundPerson, cachedReportsClient, out var cachedReportsXml);
            if (cacheFound) { return cachedReportsXml; }


            //if control reaches here, then no cache
            //generate new report (compute)
            //await RefreshDasaReportCache(foundPerson, cachedReportsClient);

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


        /// <summary>
        /// Massive method that generates dasa report in SVG
        /// </summary>
        private static string GenerateMainEventsReportSvg(Person inputPerson, Time startTime, Time endTime, double daysPerPixel, List<EventTag> inputedEventTags)
        {
            // One precision value for generating all dasa components,
            // because misalignment occurs if use different precision
            // note: precision = time slice count, each slice = 1 pixel (zoom level)
            double eventsPrecisionHours = Tools.DaysToHours(daysPerPixel);

            //generate time slice only once for all rows
            var timeSlices = EventManager.GetTimeListFromRange(startTime, endTime, eventsPrecisionHours);


            var compiledRow = GenerateRowsSvg(inputPerson, daysPerPixel, startTime, endTime, timeSlices, eventsPrecisionHours, inputedEventTags, out int totalHeight);

            //the height for all lines, cursor, now & life events line
            //place below the last row
            //var totalHeight = 240; //hard set for now
            var padding = 2; //space between rows
            var lineHeight = totalHeight + padding;

            //add in the cursor line (moves with cursor via JS)
            //note: template cursor line is duplicated to dynamically generate legend box
            compiledRow += GetTimeCursorLine(lineHeight);


            //get now line position
            var nowLinePosition = GetLinePosition(timeSlices, startTime.StdTimeNowAtOffset);
            compiledRow += GetNowLine(lineHeight, nowLinePosition);

            //wait!, add in life events also
            //use offset of input time, this makes sure life event lines
            //are placed on event chart correctly, since event chart is based on input offset
            var inputOffset = startTime.GetStdDateTimeOffset().Offset;
            compiledRow += GetLifeEventLinesSvg(inputPerson, lineHeight, inputOffset);

            //compile the final svg
            //save a copy of the number of time slices used to calculate the svg total width later
            var dasaSvgWidth = timeSlices.Count;
            var svgTotalHeight = totalHeight + 30;//add space for life event icon
            //add border around svg element
            compiledRow += $"<rect width=\"{dasaSvgWidth}\" height=\"{svgTotalHeight}\" style=\"stroke-width: 2; fill: none; paint-order: stroke; stroke:#333;\"></rect>";
            var finalSvg = WrapSvgElements(compiledRow, dasaSvgWidth, svgTotalHeight); //little wiggle room

            return finalSvg;



            //█░░ █▀█ █▀▀ ▄▀█ █░░   █▀▀ █░█ █▄░█ █▀▀ ▀█▀ █ █▀█ █▄░█ █▀
            //█▄▄ █▄█ █▄▄ █▀█ █▄▄   █▀░ █▄█ █░▀█ █▄▄ ░█░ █ █▄█ █░▀█ ▄█


            //gets person's life events as lines for the dasa chart
            string GetLifeEventLinesSvg(Person person, int lineHeight, TimeSpan inputOffset)
            {
                var compiledLines = "";

                foreach (var lifeEvent in person.LifeEventList)
                {

                    //get start time of life event and find the position of it in slices (same as now line)
                    //so that this life event line can be placed exactly on the report where it happened
                    var lifeEvtTime = DateTimeOffset.ParseExact(lifeEvent.StartTime, Time.DateTimeFormat, null);
                    var startTimeInputOffset = lifeEvtTime.ToOffset(inputOffset); //change to input offset, to match chart
                    var positionX = GetLinePosition(timeSlices, startTimeInputOffset);

                    //if line is not in report time range, don't generate it
                    if (positionX == 0) { continue; }

                    //put together icon + line + event data
                    compiledLines += GenerateLifeEventLine(lifeEvent, lineHeight, lifeEvtTime, positionX);
                }


                //wrap in a group so that can be hidden/shown as needed
                var wrapperGroup = $"<g id=\"LifeEventLinesHolder\">{compiledLines}</g>";

                return wrapperGroup;


            }

        }

        //todo need to unify event color
        public static string GetColor(string nature)
        {
            switch (nature)
            {
                case "Good": return "#42f706";
                case "Neutral": return "grey";
                //case "Bad": return "#a80000";
                case "Bad": return "#ff5656";
                default: throw new Exception($"Invalid Nature: {nature}");
            }
        }

        /// <summary>
        /// Generates individual life event line svg
        /// </summary>
        public static string GenerateLifeEventLine(LifeEvent lifeEvent, int lineHeight, DateTimeOffset lifeEvtTime,
            int positionX)
        {

            //note: this is the icon below the life event line to magnify it
            var iconWidth = 12;
            var iconX = $"-{iconWidth}"; //use negative to move center under line
            var iconSvg = $@"
                                    <g transform=""matrix(2, 0, 0, 2, {iconX}, {lineHeight})"">
                                        <rect style=""fill:{GetColor(lifeEvent.Nature)}; stroke:black; stroke-width: 0.5px;"" x=""0"" y=""0"" width=""{iconWidth}"" height=""9.95"" rx=""2.5"" ry=""2.5""/>
                                        <path d=""M 7.823 5.279 L 6.601 5.279 C 6.377 5.279 6.194 5.456 6.194 5.671 L 6.194 6.846 C 6.194 7.062 6.377 7.238 6.601 7.238 L 7.823 7.238 C 8.046 7.238 8.229 7.062 8.229 6.846 L 8.229 5.671 C 8.229 5.456 8.046 5.279 7.823 5.279 Z M 7.823 1.364 L 7.823 1.756 L 4.566 1.756 L 4.566 1.364 C 4.566 1.148 4.383 0.973 4.158 0.973 C 3.934 0.973 3.751 1.148 3.751 1.364 L 3.751 1.756 L 3.345 1.756 C 2.892 1.756 2.534 2.108 2.534 2.539 L 2.53 8.021 C 2.53 8.454 2.895 8.804 3.345 8.804 L 9.044 8.804 C 9.492 8.804 9.857 8.452 9.857 8.021 L 9.857 2.539 C 9.857 2.108 9.492 1.756 9.044 1.756 L 8.636 1.756 L 8.636 1.364 C 8.636 1.148 8.453 0.973 8.229 0.973 C 8.006 0.973 7.823 1.148 7.823 1.364 Z M 8.636 8.021 L 3.751 8.021 C 3.528 8.021 3.345 7.845 3.345 7.629 L 3.345 3.714 L 9.044 3.714 L 9.044 7.629 C 9.044 7.845 8.86 8.021 8.636 8.021 Z"" />
                                    </g>";

            //put together icon + line + event data
            var lifeEventLine = $"<g" +
                             $" eventName=\"{lifeEvent.Name}\" " +
                             $" class=\"LifeEventLines\" " + //atm used for tooltip logic
                             $" stdTime=\"{lifeEvtTime:dd/MM/yyyy}\" " + //show only date
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

            return lifeEventLine;
        }

        /// <summary>
        /// add in the cursor line (moves with cursor via JS)
        /// note: template cursor line is duplicated to dynamically generate legend box
        /// </summary>
        public static string GetTimeCursorLine(int lineHeight)
        {
            return $@"
                        <g id=""CursorLine"" x=""0"" y=""0"">
		                    <rect width=""2"" height=""{lineHeight}"" style=""fill:#000000;"" x=""0"" y=""0""></rect>
		                    <g id=""CursorLineLegendTemplate"" transform=""matrix(1, 0, 0, 1, 10, 26)"" style=""display:none;"">
                                <rect style=""fill: blue; opacity: 0.80;"" x=""-1"" y=""0"" width=""160"" height=""15"" rx=""2"" ry=""2""></rect>
			                    <text style=""fill:#FFFFFF; font-size:11px; font-weight:400; white-space: pre;"" x=""14"" y=""11"">Template</text>
                                <circle cx=""6.82"" cy=""7.573"" r=""4.907"" fill=""red""></circle>
                            </g>
                            <g id=""CursorLineLegendDescriptionHolder"" style=""display:none;"">
			                    <rect style=""fill: blue; opacity: 0.8;"" x=""170"" y=""11.244"" width=""150"" height=""{lineHeight-10}"" rx=""2"" ry=""2""></rect>
                                <g id=""CursorLineLegendDescription""></g>
		                    </g>
                       </g>
                        ";
        }
        public static string GetNowLine(int lineHeight, int nowLinePosition)
        {
            return $@"
                       <g id=""NowVerticalLine"" x=""0"" y=""0"" transform=""matrix(1, 0, 0, 1, {nowLinePosition}, 0)"">
		                    <rect width=""2"" height=""{lineHeight}"" style="" fill:blue; stroke-width:0.5px; stroke:#000;""></rect>
		                    <g transform=""matrix(2, 0, 0, 2, -12, 188)"">
			                    <rect style=""fill:blue; stroke:black; stroke-width: 0.5px;"" x=""0"" y=""0"" width=""12"" height=""9.95"" rx=""2.5"" ry=""2.5""></rect>
			                    <text style=""fill: rgb(255, 255, 255); font-size: 4.1px; white-space: pre;"" x=""1.135"" y=""6.367"">NOW</text>
		                    </g>
	                    </g>
                        ";
        }

        /// <summary>
        /// gets line position given a date
        /// finds most closest time slice, else return 0 means none found
        /// note:
        /// - make sure offset in time list and input time matches
        /// - tries to get nearest day first, then tries month to nearest year
        /// </summary>
        public static int GetLinePosition(List<Time> timeSliceList, DateTimeOffset inputTime)
        {
            var nowHour = inputTime.Hour;
            var nowDay = inputTime.Day;
            var nowYear = inputTime.Year;
            var nowMonth = inputTime.Month;

            //if nearest hour is possible then end here
            var nearestHour = GetNearestHour();
            if (nearestHour != 0) { return nearestHour; }

            //else try get nearest day
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
                //go through the list and find where the slice is closest to now
                var slicePosition = 0;
                foreach (var time in timeSliceList)
                {

                    //if same year and same month then send this slice position
                    //as the correct one
                    var sameDay = time.GetStdDate() == nowDay;
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

            int GetNearestHour()
            {
                //go through the list and find where the slice is closest to now
                var slicePosition = 0;
                foreach (var time in timeSliceList)
                {

                    //if same year and same month then send this slice position
                    //as the correct one
                    var sameHour = time.GetStdDateTimeOffset().Hour == nowHour;
                    var sameDay = time.GetStdDate() == nowDay;
                    var sameYear = time.GetStdYear() == nowYear;
                    var sameMonth = time.GetStdMonth() == nowMonth;
                    if (sameDay && sameHour && sameMonth && sameYear)
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
        public static string WrapSvgElements(string combinedSvgString, int svgWidth, int svgTotalHeight)
        {

            var svgBackgroundColor = "#f0f9ff";

            //create the final svg that will be displayed
            var svgTotalWidth = svgWidth + 10; //add little for wiggle room
            var svgBody = $"<svg id=\"DasaViewHolder\"" +
                          //$" width=\"100%\"" +
                          //$" height=\"100%\"" +
                          $" style=\"" +
                          //note: if width & height not hard set, parent div clips it
                          $"width:{svgTotalWidth}px;" +
                          $"height:{svgTotalHeight}px;" +
                          $"background:{svgBackgroundColor};" +
                          $"\" " +
                          $"xmlns=\"http://www.w3.org/2000/svg\">" +
                          $"{combinedSvgString}</svg>";

            return svgBody;
        }

        /// <summary>
        /// Generates the event & header part of the dasa report
        /// </summary>
        private static string GenerateRowsSvg(Person inputPerson, double daysPerPixel, Time startTime,
            Time endTime, List<Time> timeSlices, double precisionInHours, List<EventTag> inputedEventTags, out int totalHeight)
        {

            //px width & height of each slice of time
            //used when generating dasa rows
            //note: changes needed only here
            const int widthPerSlice = 1;

            //STEP 1 : GENERATE TIME HEADER ROWS
            var compiledRow = GenerateTimeHeaderRow(timeSlices, daysPerPixel, widthPerSlice, out int headerY);


            //STEP 2 : GENERATE EVENT ROWS
            compiledRow += GenerateEventRows(
                precisionInHours,
                startTime,
                endTime,
                inputPerson,
                headerY,
                timeSlices,
                daysPerPixel,
                inputedEventTags,
                out totalHeight);


            return compiledRow;


        }

        private static string GenerateTimeHeaderRow(List<Time> timeSlices, double daysPerPixel, int _widthPerSlice, out int headerY)
        {
            var dasaSvgWidth = 0; //will be filled when calling row generator
            var compiledRow = "";

            var beginYear = timeSlices[0].GetStdYear();
            var endYear = timeSlices.Last().GetStdYear();
            var difYears = endYear - beginYear;

            //header rows are dynamically generated as needed, hence the extra logic below
            var headerGenerator = new List<Func<List<Time>, int, int, int, string>>();
            var showYearRow = daysPerPixel <= 15;
            if (difYears >= 10 && !showYearRow) { headerGenerator.Add(GenerateDecadeRowSvg); }
            if (difYears is >= 5 and < 10) { headerGenerator.Add(Generate5YearRowSvg); }
            if (showYearRow) { headerGenerator.Add(GenerateYearRowSvg); }
            if (daysPerPixel <= 1.3) { headerGenerator.Add(GenerateMonthRowSvg); }
            if (daysPerPixel <= 0.07) { headerGenerator.Add(GenerateDateRowSvg); }


            var padding = 2;//space between rows
            headerY = 0;
            int headerHeight = 11;
            foreach (var generator in headerGenerator)
            {
                compiledRow += generator(timeSlices, headerY, 0, headerHeight);

                //update for next generator
                headerY = headerY + headerHeight + padding;
            }

            return compiledRow;


            string GenerateYearRowSvg(List<Time> timeSlices, int yAxis, int xAxis, int rowHeight)
            {

                //generate the row for each time slice
                var rowHtml = "";
                var previousYear = 0; //start 0 for first draw
                var yearBoxWidthCount = 0;
                int rectWidth = 0;
                int childAxisX = 0;

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
                const int decade = 10;

                var beginYear = timeSlices[0].GetStdYear();
                var endYear = beginYear + decade; //decade


                foreach (var slice in timeSlices)
                {

                    //only generate new year box when year changes or at
                    //end of time slices to draw the last year box
                    var lastTimeSlice = timeSlices.IndexOf(slice) == timeSlices.Count - 1;
                    var yearChanged = previousYear != slice.GetStdYear();
                    if (yearChanged || lastTimeSlice)
                    {
                        //is this slice end year & last month (month for accuracy, otherwise border at jan not december)
                        //todo begging of box is not beginning of year, possible solution month
                        //var isLastMonth = slice.GetStdMonth() is 10 or 11 or 12; //use oct & nov in-case december is not generated at low precision 
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
                            endYear = beginYear + decade;

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

            string GenerateDateRowSvg(List<Time> timeSlices, int yAxis, int xAxis, int rowHeight)
            {

                //generate the row for each time slice
                var rowHtml = "";
                var previousDate = 0; //start 0 for first draw
                var dateBoxWidthCount = 0;
                int rectWidth = 0;
                int childAxisX = 0;
                //int rowHeight = 11;

                foreach (var slice in timeSlices)
                {

                    //only generate new date box when date changes or at
                    //end of time slices to draw the last date box
                    var lastTimeSlice = timeSlices.IndexOf(slice) == timeSlices.Count - 1;
                    var dateChanged = previousDate != slice.GetStdDate();
                    if (dateChanged || lastTimeSlice)
                    {
                        //and it is in the beginning
                        if (previousDate == 0)
                        {
                            dateBoxWidthCount = 0; //reset width
                        }
                        else
                        {
                            //generate previous date data first before resetting
                            childAxisX += rectWidth; //use previous rect width to position this
                            rectWidth = dateBoxWidthCount * _widthPerSlice; //calculate new rect width
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
                                       $"{previousDate}" + //previous date generate at begin of new date
                                       $"</text>" +
                                       $"</g>";


                            //add to final return
                            rowHtml += rect;

                            //reset width
                            dateBoxWidthCount = 0;

                        }
                    }
                    //year same as before
                    else
                    {
                        //update width only, position is same
                        //as when created the year box
                        //yearBoxWidthCount *= _widthPerSlice;

                    }

                    //update previous date for next slice
                    previousDate = slice.GetStdDate();

                    dateBoxWidthCount++;


                }

                //wrap all the rects inside a svg so they can me moved together
                //svg tag here acts as group, svg nesting
                rowHtml = $"<g transform=\"matrix(1, 0, 0, 1, {xAxis}, {yAxis})\">{rowHtml}</g>";

                return rowHtml;

            }

        }

        private static async Task<string> Generate2(double eventsPrecision, Time startTime, Time endTime,
            Person inputPerson, int headerY, List<Time> timeSlices, double daysPerPixel)
        {
            const int _widthPerSlice = 1;

            var eventDataList = await APITools.GetEventDataList();


            //rows are dynamically generated as needed, hence the extra logic below
            //list of rows to generate
            var listX = new List<Tuple<string, List<Event>>>();

            //1 GENERATE DATA FOR EVENT ROWS
            //based on logic add each row
            var dasaEventList = APITools.CalculateEvents(eventsPrecision, startTime, endTime, inputPerson.GetBirthLocation(), inputPerson, EventTag.Dasa, eventDataList);
            listX.Add(new Tuple<string, List<Event>>("Dasa", dasaEventList));

            var bhuktiEventList = APITools.CalculateEvents(eventsPrecision, startTime, endTime, inputPerson.GetBirthLocation(), inputPerson, EventTag.Bhukti, eventDataList);
            listX.Add(new Tuple<string, List<Event>>("Bhukti", bhuktiEventList));

            var antaramEventList = APITools.CalculateEvents(eventsPrecision, startTime, endTime, inputPerson.GetBirthLocation(), inputPerson, EventTag.Antaram, eventDataList);
            listX.Add(new Tuple<string, List<Event>>("Antaram", antaramEventList));

            var gocharaEventList = APITools.CalculateEvents(eventsPrecision, startTime, endTime, inputPerson.GetBirthLocation(), inputPerson, EventTag.Gochara, eventDataList);
            listX.Add(new Tuple<string, List<Event>>("Gochara", gocharaEventList));

            //only show tara chandra when zoomed in
            if (daysPerPixel <= 0.08)
            {
                var tarabalaEventList = APITools.CalculateEvents(eventsPrecision, startTime, endTime, inputPerson.GetBirthLocation(), inputPerson, EventTag.Tarabala, eventDataList);
                listX.Add(new Tuple<string, List<Event>>("Tarabala", tarabalaEventList));

                var chandrabalaEventList = APITools.CalculateEvents(eventsPrecision, startTime, endTime, inputPerson.GetBirthLocation(), inputPerson, EventTag.Chandrabala, eventDataList);
                listX.Add(new Tuple<string, List<Event>>("Chandrabala", chandrabalaEventList));
            }

            var dasaSpecialList = APITools.CalculateEvents(eventsPrecision, startTime, endTime, inputPerson.GetBirthLocation(), inputPerson, EventTag.DasaSpecialRules, eventDataList);
            listX.Add(new Tuple<string, List<Event>>("DasaSpecialRules", dasaSpecialList));



            //2 STACK & GENERATED ROWS FROM ABOVE DATA
            var padding = 2;//space between rows
            var compiledRow = "";

            int yAxis = headerY;
            foreach (var tuple in listX)
            {
                //generate svg for each row & add to final row
                compiledRow += GenerateMultipleRowSvg(tuple.Item2, timeSlices, yAxis, 0, out int finalHeight, tuple.Item1);
                //set y axis (horizontal) for next row
                yAxis = yAxis + finalHeight + padding;
            }

            //3 ADD IN GOCHARA
            //future passed to caller to draw line
            //var totalHeight = yAxis + gocharaHeight;
            var totalHeight = yAxis;



            return compiledRow;

            //█░░ █▀█ █▀▀ ▄▀█ █░░   █▀▀ █░█ █▄░█ █▀▀ ▀█▀ █ █▀█ █▄░█ █▀
            //█▄▄ █▄█ █▄▄ █▀█ █▄▄   █▀░ █▄█ █░▀█ █▄▄ ░█░ █ █▄█ █░▀█ ▄█



            string GenerateSingleEventRowSvg(List<Event> eventList, List<Time> timeSlices, int yAxis, int xAxis, int rowHeight, string eventType)
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
                    var foundEvent = foundEventList.Any() ? foundEventList[0] : Event.Empty; //if no event exist use empty event as filler todo maybe filler not needed

                    //if current event is different than event has changed, so draw a black line
                    var isNewEvent = prevEventName != foundEvent.Name;
                    var borderRoomAvailable = daysPerPixel <= 10; //above 10px/day borders look ugly
                    var color = isNewEvent && borderRoomAvailable ? "black" : GetEventColor(foundEvent?.Nature);
                    prevEventName = foundEvent.Name;

                    //only if event is not empty
                    if (foundEvent.Name != EventName.EmptyEvent)
                    {
                        //generate and add to row
                        //the hard coded attribute names used here are used in App.js
                        var rect = $"<rect " +
                                   $"type=\"{eventType}\" " +
                                   $"eventname=\"{foundEvent?.FormattedName}\" " +
                                   $"age=\"{inputPerson.GetAge(slice)}\" " +
                                   $"stdtime=\"{slice.GetStdDateTimeOffset():dd/MM/yyyy}\" " + //show only date
                                   $"x=\"{horizontalPosition}\" " +
                                   $"width=\"{_widthPerSlice}\" " +
                                   $"height=\"{rowHeight}\" " +
                                   $"fill=\"{color}\" />";

                        rowHtml += rect;
                    }

                    //set position for next element
                    horizontalPosition += _widthPerSlice;

                }

                //wrap all the rects inside a svg so they can me moved together
                //svg tag here acts as group, svg nesting
                rowHtml = $"<g transform=\"matrix(1, 0, 0, 1, {xAxis}, {yAxis})\">{rowHtml}</g>";


                return rowHtml;
            }

            //height not known until generated
            //returns the final dynamic height of this gochara row
            string GenerateGocharaSvg(List<Event> eventList, List<Time> timeSlices, int yAxis, int xAxis, out int gocharaHeight, string eventType)
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
                                   $"type=\"{eventType}\" " +
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

            //height not known until generated
            //returns the final dynamic height of this gochara row
            string GenerateMultipleRowSvg(List<Event> eventList, List<Time> timeSlices, int yAxis, int xAxis, out int finalHeight, string eventType)
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
                                   $"type=\"{eventType}\" " +
                                   $"eventname=\"{foundEvent?.FormattedName}\" " +
                                   $"age=\"{inputPerson.GetAge(slice)}\" " +
                                   $"stdtime=\"{slice.GetStdDateTimeOffset():dd/MM/yyyy}\" " + //show only date
                                   $"x=\"{horizontalPosition}\" " +
                                   $"y=\"{yAxis + verticalPosition}\" " + //y axis placed here instead of parent group, so that auto legend can use the y axis
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
                //rowHtml = $"<g transform=\"matrix(1, 0, 0, 1, {xAxis}, {yAxis})\">{rowHtml}</g>";
                rowHtml = $"<g transform=\"matrix(1, 0, 0, 1, {xAxis}, 0)\">{rowHtml}</g>";

                //send height of tallest time slice aka the
                //final height of this gochara row to caller
                finalHeight = highestTimeSlice;

                return rowHtml;
            }

            //todo need to unify event color
            // Get dasa color based on nature & number of events
            string GetEventColor(EventNature? eventNature)
            {
                var colorId = "blue";

                //set color id based on nature
                switch (eventNature)
                {
                    case EventNature.Good:
                        colorId = "green";
                        break;
                    case EventNature.Neutral:
                        colorId = "grey";
                        break;
                    case EventNature.Bad:
                        colorId = "red";
                        break;
                }

                return colorId;
            }
        }

        /// <summary>
        /// Generate rows based on inputed events
        /// </summary>
        private static string GenerateEventRows(double eventsPrecision, Time startTime, Time endTime,
            Person inputPerson, int headerY, List<Time> timeSlices, double daysPerPixel, List<EventTag> inputedEventTags, out int totalHeight)
        {
            const int _widthPerSlice = 1;

            var eventDataList = APITools.GetEventDataList().Result;

            //rows are dynamically generated as needed, hence the extra logic below
            //list of rows to generate
            var listX = new List<Tuple<string, List<Event>>>();

            //1 GENERATE DATA FOR EVENT ROWS
            //based on logic add each row

            foreach (var eventTag in inputedEventTags)
            {
                var tempEventList = APITools.CalculateEvents(eventsPrecision, startTime, endTime, inputPerson.GetBirthLocation(), inputPerson, eventTag, eventDataList);
                listX.Add(new Tuple<string, List<Event>>(eventTag.ToString(), tempEventList));

            }


            //2 STACK & GENERATED ROWS FROM ABOVE DATA
            var padding = 2;//space between rows
            var compiledRow = "";

            int yAxis = headerY;
            foreach (var tuple in listX)
            {
                //generate svg for each row & add to final row
                compiledRow += GenerateMultipleRowSvg(tuple.Item2, timeSlices, yAxis, 0, out int finalHeight, tuple.Item1);
                //set y axis (horizontal) for next row
                yAxis = yAxis + finalHeight + padding;
            }

            //3 ADD IN GOCHARA
            //future passed to caller to draw line
            //var totalHeight = yAxis + gocharaHeight;
            totalHeight = yAxis;



            return compiledRow;

            //█░░ █▀█ █▀▀ ▄▀█ █░░   █▀▀ █░█ █▄░█ █▀▀ ▀█▀ █ █▀█ █▄░█ █▀
            //█▄▄ █▄█ █▄▄ █▀█ █▄▄   █▀░ █▄█ █░▀█ █▄▄ ░█░ █ █▄█ █░▀█ ▄█



            string GenerateSingleEventRowSvg(List<Event> eventList, List<Time> timeSlices, int yAxis, int xAxis, int rowHeight, string eventType)
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
                    var foundEvent = foundEventList.Any() ? foundEventList[0] : Event.Empty; //if no event exist use empty event as filler todo maybe filler not needed

                    //if current event is different than event has changed, so draw a black line
                    var isNewEvent = prevEventName != foundEvent.Name;
                    var borderRoomAvailable = daysPerPixel <= 10; //above 10px/day borders look ugly
                    var color = isNewEvent && borderRoomAvailable ? "black" : GetEventColor(foundEvent?.Nature);
                    prevEventName = foundEvent.Name;

                    //only if event is not empty
                    if (foundEvent.Name != EventName.EmptyEvent)
                    {
                        //generate and add to row
                        //the hard coded attribute names used here are used in App.js
                        var rect = $"<rect " +
                                   $"type=\"{eventType}\" " +
                                   $"eventname=\"{foundEvent?.FormattedName}\" " +
                                   $"age=\"{inputPerson.GetAge(slice)}\" " +
                                   $"stdtime=\"{slice.GetStdDateTimeOffset():dd/MM/yyyy}\" " + //show only date
                                   $"x=\"{horizontalPosition}\" " +
                                   $"width=\"{_widthPerSlice}\" " +
                                   $"height=\"{rowHeight}\" " +
                                   $"fill=\"{color}\" />";

                        rowHtml += rect;
                    }

                    //set position for next element
                    horizontalPosition += _widthPerSlice;

                }

                //wrap all the rects inside a svg so they can me moved together
                //svg tag here acts as group, svg nesting
                rowHtml = $"<g transform=\"matrix(1, 0, 0, 1, {xAxis}, {yAxis})\">{rowHtml}</g>";


                return rowHtml;
            }

            //height not known until generated
            //returns the final dynamic height of this gochara row
            string GenerateGocharaSvg(List<Event> eventList, List<Time> timeSlices, int yAxis, int xAxis, out int gocharaHeight, string eventType)
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
                                   $"type=\"{eventType}\" " +
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

            //height not known until generated
            //returns the final dynamic height of this event row
            string GenerateMultipleRowSvg(List<Event> eventList, List<Time> timeSlices, int yAxis, int xAxis, out int finalHeight, string eventType)
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
                    //get events that occurred at this time slice
                    var foundEventList = eventList.FindAll(tempEvent => tempEvent.IsOccurredAtTime(slice));

                    //generate rect for each event & stack from top to bottom
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
                                   $"type=\"{eventType}\" " +
                                   $"eventname=\"{foundEvent?.FormattedName}\" " +
                                   $"age=\"{inputPerson.GetAge(slice)}\" " +
                                   $"stdtime=\"{slice.GetStdDateTimeOffset():HH:mm dd/MM/yyyy}\" " + //show only date
                                   $"x=\"{horizontalPosition}\" " +
                                   $"y=\"{yAxis + verticalPosition}\" " + //y axis placed here instead of parent group, so that auto legend can use the y axis
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

                //wrap all the rects inside a svg so they can be moved together
                //note: use group instead of svg because editing capabilities
                //rowHtml = $"<g transform=\"matrix(1, 0, 0, 1, {xAxis}, {yAxis})\">{rowHtml}</g>";
                rowHtml = $"<g transform=\"matrix(1, 0, 0, 1, {xAxis}, 0)\">{rowHtml}</g>";

                //send height of tallest time slice aka the
                //final height of this gochara row to caller
                finalHeight = highestTimeSlice;

                return rowHtml;
            }

            //todo need to unify event color
            // Get dasa color based on nature & number of events
            string GetEventColor(EventNature? eventNature)
            {
                var colorId = "blue";

                //set color id based on nature
                switch (eventNature)
                {
                    case EventNature.Good:
                        colorId = "green";
                        break;
                    case EventNature.Neutral:
                        colorId = "grey";
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
