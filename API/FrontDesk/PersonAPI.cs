using System.Net.Mime;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using VedAstro.Library;
using Azure.Storage.Blobs;
using Person = VedAstro.Library.Person;
using Azure.Data.Tables;
using Newtonsoft.Json.Linq;
using System.Dynamic;
using System.Text;
using Newtonsoft.Json;

namespace API
{
    /// <summary>
    /// API Functions related to Person Profiles
    /// </summary>
    public class PersonAPI
    {
        


        /// <summary>
        /// Gets person list
        /// TODO MARKED FOR OBLIVION
        /// </summary>
        [Function(nameof(VerifyPersonList))]
        public static async Task<HttpResponseData> VerifyPersonList(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = $"{nameof(VerifyPersonList)}/OwnerId/{{ownerId}}/LocalHash/{{localPersonListHash}}")] HttpRequestData req,
            string ownerId, string localPersonListHash)
        {
            try
            {
                //get data from DB
                var foundCalls = AzureTable.PersonList.Query<PersonListEntity>(call => call.PartitionKey == ownerId);

                //get person list from DB data
                var foundPersonList = new List<Person>();
                foreach (var call in foundCalls) { foundPersonList.Add(Person.FromAzureRow(call)); }
                var personJsonList = Person.ListToJson(foundPersonList); //convert to json

                //NOTE: set formatting none needed so can create matching hash with client
                var jsonString = personJsonList.ToString(Formatting.None);

                //calculate latest hash
                var latestPersonListHash = Tools.GetStringHashCode(jsonString).ToString();

                //check if match with inputed hash
                var isMatch = latestPersonListHash.Equals(localPersonListHash);

                //send to caller
                return APITools.PassMessageJson(isMatch.ToString(), req);
            }

            //if any failure, show error in payload
            catch (Exception e)
            {
                APILogger.Error(e, req);
                return APITools.FailMessageJson(e.Message, req);
            }
        }

        /// <summary>
        /// Gets person list with swap function
        /// TODO MARKED FOR OBLIVION
        /// </summary>
        [Function(nameof(GetPersonListSwap))]
        public static async Task<HttpResponseData> GetPersonListSwap(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = $"{nameof(GetPersonListSwap)}/OwnerId/{{ownerId}}/VisitorId/{{visitorId}}")] HttpRequestData req,
            string ownerId, string visitorId)
        {
            try
            {
                //STAGE 1 : swap visitor ID with user ID if any (data follows user when log in)
                await APITools.SwapUserId(ownerId, visitorId);

                //STAGE 2 : continue as normal to get person list
                var foundPersons = AzureTable.PersonList.Query<PersonListEntity>(call => call.PartitionKey == ownerId);

                //add each to return list
                var personJsonList = new JArray();
                foreach (var call in foundPersons) { personJsonList.Add(Person.FromAzureRow(call).ToJson()); }

                //send to caller
                return APITools.PassMessageJson(personJsonList, req);
            }

            //if any failure, show error in payload
            catch (Exception e)
            {
                APILogger.Error(e, req);
                return APITools.FailMessageJson(e.Message, req);
            }
        }



        /// <summary>
        /// TODO MARKED FOR OBLIVION
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [Function(nameof(UpsertLifeEvent))]
        public static async Task<HttpResponseData> UpsertLifeEvent(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = nameof(UpsertLifeEvent))] HttpRequestData req)
        {

            try
            {
                //get data out of call
                var rootJson = await APITools.ExtractDataFromRequestJson(req);

                //read the life event that "literally" just came under the oceans
                var parsedLifeEvent = LifeEvent.FromJson(rootJson);

                //delete data related to person (NOT USER, PERSON PROFILE)
                await AzureCache.DeleteCacheRelatedToPerson(parsedLifeEvent.PersonId);

                //upsert to database
                var tableRow = parsedLifeEvent.ToAzureRow();
                await AzureTable.LifeEventList?.UpsertEntityAsync(tableRow);

                return APITools.PassMessageJson(req);
            }
            catch (Exception e)
            {
                //log error
                APILogger.Error(e, req);

                //format error nicely to show user
                return APITools.FailMessageJson(e, req);
            }

        }


        
        [Function(nameof(DeleteLifeEvent))]
        public static async Task<HttpResponseData> DeleteLifeEvent(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "DeleteLifeEvent/PersonId/{personId}/LifeEventId/{lifeEventId}")] HttpRequestData req,
            string personId, string lifeEventId)
        {
            try
            {

                //# delete data related to person (NOT USER, PERSON PROFILE)
                await AzureCache.DeleteCacheRelatedToPerson(personId);

                //# add deleted person to recycle bin
                //await AzureTable.PersonListRecycleBin.UpsertEntityAsync(personAzureRow);

                //# do final delete from MAIN DATABASE
                await AzureTable.LifeEventList.DeleteEntityAsync(personId, lifeEventId);

                return APITools.PassMessageJson(req);

            }
            catch (Exception e)
            {
                //log error
                APILogger.Error(e, req);

                //format error nicely to show user
                return APITools.FailMessageJson(e, req);
            }
        }


    }
}
