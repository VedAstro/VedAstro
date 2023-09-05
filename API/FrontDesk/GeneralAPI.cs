using VedAstro.Library;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Azure.Data.Tables;
using Azure;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace API
{
    /// <summary>
    /// All API calls with no home are here, send them somewhere you think is good
    /// </summary>
    public class GeneralAPI
    {

        [Function("gethoroscope")]
        public static async Task<HttpResponseData> GetHoroscope([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestData incomingRequest)
        {

            try
            {
                //get person from request
                var rootXml = await APITools.ExtractDataFromRequestXml(incomingRequest);
                var personId = rootXml.Value;

                var person = await Tools.GetPersonById(personId);

                //calculate predictions for current person
                var predictionList = await Tools.GetHoroscopePrediction(person.BirthTime, APITools.HoroscopeDataListFile);

                var sortedList = SortPredictionData(predictionList);

                //convert list to xml string in root elm
                return APITools.PassMessage(Tools.AnyTypeToXmlList(sortedList), incomingRequest);

            }
            catch (Exception e)
            {
                //log error
                APILogger.Error(e, incomingRequest);
                //format error nicely to show user
                return APITools.FailMessage(e, incomingRequest);
            }



            List<HoroscopePrediction> SortPredictionData(List<HoroscopePrediction> horoscopePredictions)
            {
                //put rising sign at top
                horoscopePredictions.MoveToBeginning((horPre) => horPre.FormattedName.ToLower().Contains("rising"));

                //todo followed by planet in sign prediction ordered by planet strength 

                return horoscopePredictions;
            }

        }


        /// <summary>
        /// When browser visit API, they ask for FavIcon, so yeah redirect favicon from website
        /// </summary>
        [Function(nameof(FavIcon))]
        public static async Task<HttpResponseData> FavIcon([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "favicon.ico")] HttpRequestData incomingRequest)
        {
            //use same fav icon from website
            string url = "https://vedastro.org/images/favicon.ico";

            //send to caller
            using (var client = new HttpClient())
            {
                var bytes = await client.GetByteArrayAsync(url);
                var response = incomingRequest.CreateResponse(HttpStatusCode.OK);
                response.Headers.Add("Content-Type", "image/x-icon");
                await response.Body.WriteAsync(bytes, 0, bytes.Length);
                return response;
            }
        }

        /// <summary>
        /// Gets log from last 30 days, and groups by IP and all time count
        /// http://localhost:7071/api/OpenAPILog/
        /// </summary>
        [Function(nameof(OpenAPILog))]
        public static HttpResponseData OpenAPILog([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "OpenAPILog")] HttpRequestData incomingRequest)
        {
            var tableClient = APILogger.LogBookClient;

            //get all IP address records in the last 30 days
            DateTimeOffset aMomentAgo = DateTimeOffset.UtcNow.AddDays(-30);
            var allEntities = tableClient.Query<OpenAPILogBookEntity>(call => call.Timestamp >= aMomentAgo).ToList();

            //get only unique addresses from last 30 days
            List<string> distinctIpAddressList = allEntities.Select(e => e.PartitionKey).Distinct().ToList();

            //create new presentation list
            var presentationList = new Dictionary<string,int>();
            foreach (var callerAddress in distinctIpAddressList)
            {
                //get all calls from full time period
                var foundCalls = tableClient.Query<OpenAPILogBookEntity>(call => call.PartitionKey == callerAddress);
                presentationList[callerAddress] = foundCalls.Count();
            }

            //sort by most called to least
            var sortedList = presentationList.OrderByDescending(x => x.Value); //order by call count

            //4 : CONVERT TO JSON
            JArray jsonArray = new JArray(sortedList.Select(x => new JObject { { x.Key, x.Value } }));

            //5 : SEND DATA
            return APITools.PassMessageJson(jsonArray, incomingRequest);

        }

        /// <summary>
        /// Shows all rows for a given IP
        /// http://localhost:7071/api/OpenAPILog/IP/212.35.2.245
        /// </summary>
        [Function(nameof(OpenAPILog_IP))]
        public static HttpResponseData OpenAPILog_IP([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "OpenAPILog/IP/{ipAddress}")] HttpRequestData incomingRequest,
            string ipAddress)
        {
            var tableClient = APILogger.LogBookClient;

            //all for IP
            var foundCalls = tableClient.Query<OpenAPILogBookEntity>(call => call.PartitionKey == ipAddress);

            //create new presentation list
            var presentationList = new Dictionary<DateTimeOffset, string>();
            foreach (var callLog in foundCalls)
            {
                //format into understandable time format
                var x = callLog?.Timestamp ?? DateTimeOffset.MinValue;
                var serverTime = x.ToOffset(TimeSpan.FromHours(8)).ToString(Time.DateTimeFormatSeconds);
                var rowData = $"{serverTime} | {callLog.PartitionKey} | {callLog.URL}";
                presentationList[x] = rowData;
            }

            //sort by most called to least
            var sortedList = presentationList.OrderBy(x => x.Value); //order by time

            //4 : CONVERT TO JSON
            JArray jsonArray = new JArray(sortedList.Select(x => x.Value));

            //5 : SEND DATA
            return APITools.PassMessageJson(jsonArray, incomingRequest);

        }



    }
}
