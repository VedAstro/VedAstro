using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using VedAstro.Library;
using Azure.Storage.Blobs;

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
        [Function(nameof(GetNewPersonId))]
        public static async Task<HttpResponseData> GetNewPersonId([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "GetNewPersonId/Name/{personName}/BirthYear/{birthYear}")] HttpRequestData incomingRequest,
            string personName, string birthYear)
        {

            try
            {
                //special ID made for human brains
                var brandNewHumanReadyId = await APITools.GeneratePersonId(personName, birthYear);

                return APITools.PassMessageJson(brandNewHumanReadyId, incomingRequest);

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
        /// Gets person list
        /// </summary>
        [Function(nameof(GetPersonList))]
        public static async Task<HttpResponseData> GetPersonList(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "GetPersonList/UserId/{userId}/VisitorId/{visitorId}")] HttpRequestData req,
            string userId, string visitorId)
        {

            try
            {
                //GET THE DATA FROM CALLER
                var parsedRequest = new CallerInfo(visitorId, userId);

                //PREPARE THE CALL
                Func<Task<BlobClient>> getPersonList = () => APITools.ExecuteAndSaveToCache(() => _getPersonList(parsedRequest), parsedRequest.CallerId);

                //CACHE MECHANISM
                var httpResponseData = await AzureCache.CacheExecute(getPersonList, parsedRequest, req);

                return httpResponseData;

            }
            catch (Exception e)
            {
                //log it
                await APILogger.Error(e);
                var response = req.CreateResponse(HttpStatusCode.OK);
                response.Headers.Add("Call-Status", "Fail"); //caller checks this
                return response;
            }


            //--------------------------------
            async Task<string> _getPersonList(CallerInfo callerInfo)
            {

                //STAGE 2 : SWAP DATA
                //swap visitor ID with user ID if any (data follows user when log in)
                if (!callerInfo.Both101) //only swap if needed
                {
                    bool didSwap = await APITools.SwapUserId(callerInfo, APITools.PersonListFile);
                }

                //get latest all match reports
                var personListXml = await APITools.GetXmlFileFromAzureStorage(APITools.PersonListFile, APITools.BlobContainerName);

                //filter out record by caller id, which will be visitor id when user not logged in
                var personListByCallerIdXml = Tools.FindXmlByUserId(personListXml, callerInfo.CallerId);

                //sort a to z by name for ease of user (done here for speed vs client)
                var sortedList = personListByCallerIdXml.OrderBy(personXml => personXml.Element("Name")?.Value ?? "").ToList();

                //convert raw XML to Person Json
                var personListJson = Person.XmlListToJsonList(sortedList);

                //convert to type .NET's JSON accepted by durable
                var jsonText = personListJson.ToString();

                return jsonText;

            }

        }


        /// <summary>
        /// Gets person all details from only hash
        /// </summary>
        [Function(nameof(GetPerson))]
        public static async Task<HttpResponseData> GetPerson(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "GetPerson/PersonId/{personId}")] HttpRequestData req,
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
            string userId, string visitorId)
        {
            //STAGE 1 : GET DATA OUT
            var parsedRequest = new CallerInfo(visitorId,userId);

            //adding new person needs make sure all cache is cleared if any
            await AzureCache.Delete(parsedRequest.CallerId);

            //STAGE 2 : NOW WE WORK
            //get new person data out of incoming request
            //note: inside new person xml already contains user id
            var personJson = await APITools.ExtractDataFromRequestJson(req);
            var newPerson = Person.FromJson(personJson);

            //possible old cache of person with same id lived, so clear cache if any
            //delete data related to person (NOT USER, PERSON PROFILE)
            await AzureCache.DeleteStuffRelatedToPerson(newPerson);

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
            string userId, string visitorId)
        {
            //STAGE 1 : GET DATA OUT
            var parsedRequest = new CallerInfo(visitorId, userId);

            await AzureCache.Delete(parsedRequest.CallerId);

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
                    //delete data related to person (NOT USER, PERSON PROFILE)
                    await AzureCache.DeleteStuffRelatedToPerson(updatedPerson);

                    //save a copy of the original person record in recycle bin, just in-case accidental update
                    var originalPerson = await APITools.GetPersonById(updatedPerson.Id);
                    await APITools.AddXElementToXDocumentAzure(originalPerson.ToXml(), APITools.RecycleBinFile, APITools.BlobContainerName);

                    //directly updates and saves new person record to main list (does all the work, sleep easy)
                    await APITools.UpdatePersonRecord(updatedPerson);

                    //all is good baby
                    return APITools.PassMessageJson(req);

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
                return APITools.FailMessageJson(e, req);
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
            string userId, string visitorId, string personId)
        {
            //STAGE 1 : GET DATA OUT
            var parsedRequest = new CallerInfo(visitorId, userId);

            await AzureCache.Delete(parsedRequest.CallerId);

            //STAGE 2 : NOW WE WORK
            try
            {
                //get the person record that needs to be deleted
                var personToDelete = await APITools.FindPersonXMLById(personId);
                var personParsed = Person.FromXml(personToDelete);


                //only owner can delete so make sure here
                var isOwner = Tools.IsUserIdMatch(personParsed.UserIdString, parsedRequest.CallerId);
                if (isOwner)
                {
                    //delete data related to person (NOT USER, PERSON PROFILE)
                    await AzureCache.DeleteStuffRelatedToPerson(personParsed);

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







    }
}
