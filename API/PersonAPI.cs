using System.Text.Json;
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
        /// Generate a human readable Person ID
        /// 
        /// </summary>
        [Function("GetNewPersonId")]
        public static async Task<HttpResponseData> GetNewPersonId([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "GetNewPersonId/Name/{personName}/BirthYear/{birthYear}")] HttpRequestData incomingRequest,
            string personName, string birthYear)
        {

            try
            {
                //special ID made for human brains
                var brandNewHumanReadyID = await APITools.GeneratePersonId(personName, birthYear);

                return APITools.PassMessageJson(brandNewHumanReadyID, incomingRequest);

            }
            catch (Exception e)
            {
                //log error
                await APILogger.Error(e, incomingRequest);

                //format error nicely to show user
                return APITools.FailMessage(e, incomingRequest);
            }

        }






        //░█████╗░░██████╗██╗░░░██╗███╗░░██╗░█████╗░
        //██╔══██╗██╔════╝╚██╗░██╔╝████╗░██║██╔══██╗
        //███████║╚█████╗░░╚████╔╝░██╔██╗██║██║░░╚═╝
        //██╔══██║░╚═══██╗░░╚██╔╝░░██║╚████║██║░░██╗
        //██║░░██║██████╔╝░░░██║░░░██║░╚███║╚█████╔╝
        //╚═╝░░╚═╝╚═════╝░░░░╚═╝░░░╚═╝░░╚══╝░╚════╝░
        //POWERED BY AZURE DURABLE FUNCTIONS
        //-------------------------------------------------------------------------------------------

        /// <summary>
        /// Gets person all details from only hash
        /// </summary>
        [Function(nameof(GetPerson))]
        public static async Task<HttpResponseData> GetPerson(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "GetPerson/PersonId/{personId}")] HttpRequestData req,
            [DurableClient] DurableTaskClient client,
            string personId)
        {

            try
            {
                //get the person record by ID
                var foundPersonXml = await APITools.FindPersonXMLById(personId);

                var personToReturn = Person.FromXml(foundPersonXml);
                
                //send person to caller
                return APITools.PassMessageJson(personToReturn.ToJson(), req);

            }
            catch (Exception e)
            {
                //log error
                await APILogger.Error(e, req);

                //let caller know fail, include exception info for easy debugging
                return APITools.FailMessageJson(e, req);
            }


        }

        [Function(nameof(AddPerson))]
        public static async Task<HttpResponseData> AddPerson(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "AddPerson/UserId/{userId}/VisitorId/{visitorId}")] HttpRequestData req,
            [DurableClient] DurableTaskClient client,
            string userId, string visitorId)
        {
            //STAGE 1 : GET DATA OUT
            var parsedRequest = new ParsedRequest(userId, visitorId);


            //adding new person needs make sure all cache is cleared if any
            //NOTE: only choice here is "Purge", "Terminate" does not work, will cause webhook url to 404
            var purgeResult = await client.PurgeInstanceAsync(parsedRequest.CallerId);
            var successPurge = purgeResult.PurgedInstanceCount > 0; //if purged will be 1


            //STAGE 2 : NOW WE WORK
            //get new person data out of incoming request
            //note: inside new person xml already contains user id
            var personJson = await APITools.ExtractDataFromRequestJson(req);
            var newPerson = Person.FromJson(personJson);

            //add new person to main list
            await APITools.AddXElementToXDocumentAzure(newPerson.ToXml(), APITools.PersonListFile, APITools.BlobContainerName);

            return APITools.PassMessageJson(req);

        }


        /// <summary>
        /// Updates a person's record, uses hash to identify person to overwrite
        /// </summary>
        [Function(nameof(UpdatePerson))]
        public static async Task<HttpResponseData> UpdatePerson(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "UpdatePerson/UserId/{userId}/VisitorId/{visitorId}")] HttpRequestData req,
            [DurableClient] DurableTaskClient client,
            string userId, string visitorId)
        {
            //STAGE 1 : GET DATA OUT
            var parsedRequest = new ParsedRequest(userId, visitorId);


            //adding new person needs make sure all cache is cleared if any
            //NOTE: only choice here is "Purge", "Terminate" does not work, will cause webhook url to 404
            var purgeResult = await client.PurgeInstanceAsync(parsedRequest.CallerId);
            var successPurge = purgeResult.PurgedInstanceCount > 0; //if purged will be 1


            //STAGE 2 : NOW WE WORK
            try
            {
                //get new person data out of incoming request
                //note: inside new person xml already contains user id
                var personJson = await APITools.ExtractDataFromRequestJson(req);
                var updatedPerson = Person.FromJson(personJson);


                //only owner can delete so make sure here
                var isOwner = Tools.IsUserIdMatch(updatedPerson.UserIdString, parsedRequest.CallerId);
                if (isOwner)
                {
                    //save a copy of the original person record in recycle bin, just in-case accidental update
                    var originalPerson = await APITools.GetPersonById(updatedPerson.Id);
                    await APITools.AddXElementToXDocumentAzure(originalPerson.ToXml(), APITools.RecycleBinFile, APITools.BlobContainerName);

                    //directly updates and saves new person record to main list (does all the work, sleep easy)
                    await APITools.UpdatePersonRecord(updatedPerson);

                    //all is good baby
                    return APITools.PassMessage(req);

                }
                else
                {
                    return APITools.FailMessageJson("Only owner can update, you are not.", req);
                }


            }
            catch (Exception e)
            {
                //log error
                await APILogger.Error(e, req);

                //format error nicely to show user
                return APITools.FailMessage(e, req);
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
        public static async Task<HttpResponseData> DeletePerson(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "DeletePerson/UserId/{userId}/VisitorId/{visitorId}/PersonId/{personId}")] HttpRequestData req,
            [DurableClient] DurableTaskClient client,
            string userId, string visitorId, string personId)
        {
            //STAGE 1 : GET DATA OUT
            var parsedRequest = new ParsedRequest(userId, visitorId);


            //adding new person needs make sure all cache is cleared if any
            //NOTE: only choice here is "Purge", "Terminate" does not work, will cause webhook url to 404
            var purgeResult = await client.PurgeInstanceAsync(parsedRequest.CallerId);
            var successPurge = purgeResult.PurgedInstanceCount > 0; //if purged will be 1


            //STAGE 2 : NOW WE WORK
            try
            {
                //get the person record that needs to be deleted
                var personToDelete = await APITools.FindPersonXMLById(personId);

                //only owner can delete so make sure here
                var personParsed = Person.FromXml(personToDelete);
                var isOwner = Tools.IsUserIdMatch(personParsed.UserIdString, parsedRequest.CallerId);
                if (isOwner)
                {
                    //add deleted person to recycle bin 
                    await APITools.AddXElementToXDocumentAzure(personToDelete, APITools.RecycleBinFile, APITools.BlobContainerName);

                    //delete the person record,
                    await APITools.DeleteXElementFromXDocumentAzure(personToDelete, APITools.PersonListFile, APITools.BlobContainerName);

                    return APITools.PassMessageJson(req);
                }
                else
                {
                    return APITools.FailMessageJson("Only owner can delete, you are not.", req);
                }

            }
            catch (Exception e)
            {
                //log error
                await APILogger.Error(e, req);

                //format error nicely to show user
                return APITools.FailMessage(e, req);
            }
        }




        //-------------------TODO CLEAN




        [Function(nameof(GetPersonListAsync))]
        public static async Task<HttpResponseData> GetPersonListAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "GetPersonListAsync/UserId/{userId}/VisitorId/{visitorId}")] HttpRequestData req,
            [DurableClient] DurableTaskClient client,
            string userId, string visitorId)
        {
            //STAGE 1 : GET DATA OUT
            var parsedRequest = new ParsedRequest(userId, visitorId);


            //check if new call or old call 
            var result = await client.GetInstanceAsync(parsedRequest.CallerId, CancellationToken.None);
            var needsRestart = result?.RuntimeStatus == OrchestrationRuntimeStatus.Failed ||
                               result?.RuntimeStatus == OrchestrationRuntimeStatus.Terminated ||
                               result?.RuntimeStatus == OrchestrationRuntimeStatus.Suspended;
            var isNewCall = result == null; //no old calls found will null


            //STAGE 2 : PROCESS
            //NOTE: only recalculate if non existent or corrupt
            if (isNewCall || needsRestart)
            {
                //if already exist than we must kill it first completely of the face of the earth aka PURGE
                //NOTE: this should help make sure brand new instance is created after without error effecting restart
                var purgeResult = await client.PurgeInstanceAsync(parsedRequest.CallerId);
                var successPurge = purgeResult.PurgedInstanceCount > 0; //if purged will be 1

                //start processing
                var options = new StartOrchestrationOptions(parsedRequest.CallerId); //set caller id so can callback
                var instanceId = await client.ScheduleNewOrchestrationInstanceAsync(nameof(_getPersonListAsync), parsedRequest, options, CancellationToken.None); //should match caller ID
            }

            //give user the url to query for status and data
            //note : todo this is hack to get polling URL via RESPONSE creator, should be able to create directly
            var x = client.CreateCheckStatusResponse(req, parsedRequest.CallerId);
            var pollingUrl = APITools.GetHeaderValue(x, "Location");

            //send polling URL to caller as Passed payload, client should know what todo
            return APITools.PassMessageJson(pollingUrl, req);

        }


        /// <summary>
        /// The underlying async func that actually gets the list
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
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
        public static async Task<JsonDocument> FilterData([ActivityTrigger] ParsedRequest requestData, FunctionContext executionContext)
        {

            //get latest all match reports
            var personListXml = await APITools.GetXmlFileFromAzureStorage(APITools.PersonListFile, APITools.BlobContainerName);

            //filter out record by caller id, which will be visitor id when user not logged in
            var personListByCallerIdXml = Tools.FindXmlByUserId(personListXml, requestData.CallerId);

            //sort a to z by name for ease of user (done here for speed vs client)
            var sortedList = personListByCallerIdXml.OrderBy(personXml => personXml.Element("Name")?.Value ?? "").ToList();

            //convert raw XML to Person Json
            var personListJson = Person.XmlListToJsonList(sortedList);

            //convert to type .NET's JSON accepted by durable
            var jsonText = personListJson.ToString();
            var personListJsonDurable = JsonDocument.Parse(jsonText); //done for compatibility with durable

            return personListJsonDurable;

        }


    }
}
