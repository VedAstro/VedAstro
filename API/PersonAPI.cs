using System.Net;
using System.Text.Json;
using System.Xml.Linq;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.DurableTask;
using Microsoft.DurableTask.Client;
using VedAstro.Library;

namespace API
{
    /// <summary>
    /// API Functions related to Person Profiles
    /// </summary>
    public class PersonAPI
    {

        /// <summary>
        /// Gets all person profiles owned by User ID & Visitor ID
        /// </summary>
        [Function(nameof(Account))]
        public static async Task<HttpResponseData> Account(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "Account/UserID/{userId}/VisitorID/{visitorId}")] HttpRequestData incomingRequest,
            string userId,
            string visitorId)
        {
        //used when visitor id person were moved to user id, shouldn't happen all the time, obviously adds to the lag (needs speed testing) 
        TryAgain:

            try
            {

                //get all person list from storage
                var personListXml = await APITools.GetXmlFileFromAzureStorage(APITools.PersonListFile, APITools.BlobContainerName);

                //filter out person by user id
                var userIdPersonList = Tools.FindXmlByUserId(personListXml, userId);

                //filter out person by visitor id
                var visitorIdPersonList = Tools.FindXmlByUserId(personListXml, visitorId);

                //before sending to user, clean the data
                //if user made profile while logged out then logs in, transfer the profiles created with visitor id to the new user id
                //if this is not done, then when user loses the visitor ID, they also loose access to the person profile
                var loggedIn = userId != "101" && !(string.IsNullOrEmpty(userId));//already logged in if true
                var visitorProfileExists = visitorIdPersonList.Any();
                if (loggedIn && visitorProfileExists)
                {
                    //transfer to user id
                    foreach (var person in visitorIdPersonList)
                    {
                        //edit data direct to for speed
                        person.Element("UserId").Value = userId;

                        //save to main list
                        await APITools.UpdatePersonRecord(person);
                    }

                    //after the transfer, restart the call as though new, so that user only gets the correct list at all times (though this might be little slow)
                    goto TryAgain;
                }

                //STAGE 4 : combine and remove duplicates
                if (visitorIdPersonList.Any()) { userIdPersonList.AddRange(visitorIdPersonList); }
                List<XElement> personListNoDupes = userIdPersonList.Distinct().ToList();

                var x = Tools.ListToJson(personListNoDupes);

                //send filtered list to caller
                return APITools.PassMessageJson(x, incomingRequest);

            }
            catch (Exception e)
            {
                //log error
                await APILogger.Error(e, incomingRequest);
                //format error nicely to show user
                return APITools.FailMessageJson(e, incomingRequest);
            }


        }


        /// <summary>
        /// Generate a human readable Person ID
        /// 
        /// </summary>
        [Function("GetNewPersonId")]
        public static async Task<HttpResponseData> GetNewPersonId([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestData incomingRequest)
        {

            try
            {
                //STAGE 1 : GET DATA OUT
                //data out of request
                var rootXml = await APITools.ExtractDataFromRequestXml(incomingRequest);
                var name = rootXml.Element("Name")?.Value;
                var birthYear = rootXml.Element("BirthYear")?.Value;

                //special ID made for human brains
                var brandNewHumanReadyID = await APITools.GeneratePersonId(name, birthYear);

                return APITools.PassMessage(brandNewHumanReadyID, incomingRequest);

            }
            catch (Exception e)
            {
                //log error
                await APILogger.Error(e, incomingRequest);

                //format error nicely to show user
                return APITools.FailMessage(e, incomingRequest);
            }

        }

        [Function("addperson")]
        public static async Task<HttpResponseData> AddPerson([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestData incomingRequest)
        {

            try
            {
                //get new person data out of incoming request
                //note: inside new person xml already contains user id
                var newPersonXml = await APITools.ExtractDataFromRequestXml(incomingRequest);

                //add new person to main list
                await APITools.AddXElementToXDocumentAzure(newPersonXml, APITools.PersonListFile, APITools.BlobContainerName);

                return APITools.PassMessage(incomingRequest);

            }
            catch (Exception e)
            {
                //log error
                await APILogger.Error(e, incomingRequest);

                //format error nicely to show user
                return APITools.FailMessage(e, incomingRequest);
            }

        }


        /// <summary>
        /// Gets person all details from only hash
        /// </summary>
        [Function("getperson")]
        public static async Task<HttpResponseData> GetPerson([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestData incomingRequest)
        {

            try
            {
                //get hash that will be used find the person
                var requestData = await APITools.ExtractDataFromRequestXml(incomingRequest);
                var personId = requestData.Value;

                //get the person record by ID
                var foundPerson = await APITools.FindPersonXMLById(personId);

                //send person to caller
                return APITools.PassMessage(foundPerson, incomingRequest);

            }
            catch (Exception e)
            {
                //log error
                await APILogger.Error(e, incomingRequest);

                //let caller know fail, include exception info for easy debugging
                return APITools.FailMessage(e, incomingRequest);
            }


        }

        /// <summary>
        /// Updates a person's record, uses hash to identify person to overwrite
        /// </summary>
        [Function("updateperson")]
        public static async Task<HttpResponseData> UpdatePerson(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestData incomingRequest)
        {

            try
            {
                //get unedited hash & updated person details from incoming request
                var updatedPersonXml = await APITools.ExtractDataFromRequestXml(incomingRequest);

                //save a copy of the original person record in recycle bin, just in-case accidental update
                var personId = Person.FromXml(updatedPersonXml).Id;//does not change
                var originalPerson = await APITools.GetPersonById(personId);
                await APITools.AddXElementToXDocumentAzure(originalPerson.ToXml(), APITools.RecycleBinFile, APITools.BlobContainerName);

                //directly updates and saves new person record to main list (does all the work, sleep easy)
                await APITools.UpdatePersonRecord(updatedPersonXml);

                //all is good baby
                return APITools.PassMessage(incomingRequest);

            }
            catch (Exception e)
            {
                //log error
                await APILogger.Error(e, incomingRequest);

                //format error nicely to show user
                return APITools.FailMessage(e, incomingRequest);
            }

        }

        /// <summary>
        /// Deletes a person's record, uses hash to identify person
        /// Note : user id is not checked here because Person hash
        /// can't even be generated by client side if you don't have access.
        /// Theoretically anybody who gets the hash of the person,
        /// can delete the record by calling this API
        /// </summary>
        [Function(nameof(DeletePerson))]
        public static async Task<HttpResponseData> DeletePerson([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestData incomingRequest)
        {

            try
            {
                //get unedited hash & updated person details from incoming request
                var requestData = await APITools.ExtractDataFromRequestXml(incomingRequest);
                var personId = requestData.Value;

                //get the person record that needs to be deleted
                var personToDelete = await APITools.FindPersonXMLById(personId);

                //add deleted person to recycle bin 
                await APITools.AddXElementToXDocumentAzure(personToDelete, APITools.RecycleBinFile, APITools.BlobContainerName);

                //delete the person record,
                await APITools.DeleteXElementFromXDocumentAzure(personToDelete, APITools.PersonListFile, APITools.BlobContainerName);

                return APITools.PassMessage(incomingRequest);

            }
            catch (Exception e)
            {
                //log error
                await APILogger.Error(e, incomingRequest);

                //format error nicely to show user
                return APITools.FailMessage(e, incomingRequest);
            }
        }

        /// <summary>
        /// Gets all person profiles owned by User ID & Visitor ID
        /// </summary>
        [Function(nameof(GetPersonList))]
        public static async Task<HttpResponseData> GetPersonList([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestData incomingRequest)
        {
            AppInstance.IncomingRequest = incomingRequest;
            var personListFileUrl = APITools.PersonListFile;

            try
            {

                //STAGE 1 : GET DATA OUT
                //data out of request
                var rootXml = await APITools.ExtractDataFromRequestXml(incomingRequest);
                var userId = rootXml.Element("UserId")?.Value;
                var visitorId = rootXml.Element("VisitorId")?.Value;

                //STAGE 2 : SWAP DATA
                //swap visitor ID with user ID if any (data follows user when log in)
                await APITools.SwapUserId(visitorId, userId, personListFileUrl);

                //STAGE 3 : FILTER 
                //get latest all match reports
                var personListXml = await APITools.GetXmlFileFromAzureStorage(personListFileUrl, APITools.BlobContainerName);
                //filter out record by user id
                var userIdList = Tools.FindXmlByUserId(personListXml, userId);

                //STAGE 4 : SEND XML todo JSON 
                //convert list to xml
                var xmlPayload = Tools.AnyTypeToXmlList(userIdList);

                //send filtered list to caller
                return APITools.PassMessage(xmlPayload, incomingRequest);

            }
            catch (Exception e)
            {
                //log error
                await APILogger.Error(e, incomingRequest);
                //format error nicely to show user
                return APITools.FailMessage(e, incomingRequest);
            }

        }


        //------------- ASYNC FUNCTIONS----------------------------------------------

        [Function(nameof(AddPersonAsync))]
        public static async Task<HttpResponseData> AddPersonAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "AddPersonAsync/UserId/{userId}/VisitorId/{visitorId}")] HttpRequestData req,
            [DurableClient] DurableTaskClient client,
            string userId, string visitorId)
        {
            //STAGE 1 : GET DATA OUT
            var parsedRequest = new ParsedRequest(userId, visitorId);

            //get caller ID, so can use back any data if available
            string callerId = APITools.GetCallerId(parsedRequest);

            //adding new person needs make sure all cache is cleared if any
            //NOTE: this call kill the entire webhook url to 404
            var purgeResult = await client.PurgeInstanceAsync(callerId);
            var successPurge = purgeResult.PurgedInstanceCount > 0; //if purged will be 1

            //get new person data out of incoming request
            //note: inside new person xml already contains user id
            var personJson = await APITools.ExtractDataFromRequestJson(req);
            var newPerson = Person.FromJson(personJson);

            //add new person to main list
            await APITools.AddXElementToXDocumentAzure(newPerson.ToXml(), APITools.PersonListFile, APITools.BlobContainerName);

            return APITools.PassMessageJson("", req);

        }


        [Function(nameof(GetPersonListAsync))]
        public static async Task<HttpResponseData> GetPersonListAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "GetPersonListAsync/UserId/{userId}/VisitorId/{visitorId}")] HttpRequestData req,
            [DurableClient] DurableTaskClient client,
            string userId, string visitorId)
        {
            //STAGE 1 : GET DATA OUT
            var parsedRequest = new ParsedRequest(userId, visitorId);

            //get caller ID, so can use back any data if available
            string callerId = APITools.GetCallerId(parsedRequest);

            //check if new call or old call 
            var result = await client.GetInstanceAsync(callerId, CancellationToken.None);
            var needsRestart = result?.RuntimeStatus == OrchestrationRuntimeStatus.Failed ||
                               result?.RuntimeStatus == OrchestrationRuntimeStatus.Terminated||
                               result?.RuntimeStatus == OrchestrationRuntimeStatus.Suspended; 
            var isNewCall = result == null; //no old calls found will null
            if (isNewCall || needsRestart)
            {
                //start processing
                var options = new StartOrchestrationOptions(callerId); //set caller id so can callback
                var instanceId = await client.ScheduleNewOrchestrationInstanceAsync(nameof(_getPersonListAsync), parsedRequest, options, CancellationToken.None); //should match caller ID
            }

            //give user the url to query for status and data
            //note : todo this is hack to get polling URL via RESPONSE creator, should be able to create directly
            var x = client.CreateCheckStatusResponse(req, callerId);
            var pollingUrl = APITools.GetHeaderValue(x, "Location");

            //send URL direct to caller
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteStringAsync(pollingUrl);
            return response;

        }


        [Function(nameof(_getPersonListAsync))]
        public static async Task<JsonDocument> _getPersonListAsync([OrchestrationTrigger]
            TaskOrchestrationContext context)
        {
            var requestData = context.GetInput<ParsedRequest>();

            var swapStatus = await context.CallActivityAsync<bool>(nameof(SwapData), requestData); //note swap done before get list
            var personList = await context.CallActivityAsync<JsonDocument>(nameof(FilterData), requestData);

            return personList;
        }

        [Function(nameof(SwapData))]
        public static async Task<bool> SwapData([ActivityTrigger] ParsedRequest swapOptions, FunctionContext executionContext)
        {

            //STAGE 2 : SWAP DATA
            //swap visitor ID with user ID if any (data follows user when log in)
            bool didSwap = await APITools.SwapUserId(swapOptions.VisitorId, swapOptions.UserId, APITools.PersonListFile);

            return didSwap;
        }

        [Function(nameof(FilterData))]
        public static async Task<JsonDocument> FilterData([ActivityTrigger] ParsedRequest swapOptions, FunctionContext executionContext)
        {

            //get latest all match reports
            var personListXml = await APITools.GetXmlFileFromAzureStorage(APITools.PersonListFile, APITools.BlobContainerName);

            //filter out record by user id
            var userIdList = Tools.FindXmlByUserId(personListXml, swapOptions.UserId);

            //convert raw XML to Person Json
            var personListJson = Person.XmlListToJsonList(userIdList);

            //convert to type accepted by durable
            var jsonText = personListJson.ToString();
            var personListJsonDurable = JsonDocument.Parse(jsonText); //done for compatibility with durable

            return personListJsonDurable;

        }


    }
}
