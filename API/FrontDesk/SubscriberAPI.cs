using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker;
using Newtonsoft.Json.Linq;
using ScottPlot;
using System.Drawing;
using System.Linq.Expressions;
using System.Net.Mime;
using Azure.Data.Tables;
using VedAstro.Library;

namespace API
{

    /// <summary>
    /// View table data via API
    /// </summary>
    public static class SubscriberAPI
    {
        /// <summary>
        /// Gets person list
        /// </summary>
        [Function(nameof(GetPrivateServerList))]
        public static async Task<HttpResponseData> GetPrivateServerList(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = $"{nameof(GetPrivateServerList)}/UserId/{{userId}}")] HttpRequestData req,
            string userId)
        {
            try
            {


                var foundCalls = AzureTable.PersonList.Query<PersonListEntity>(call => call.PartitionKey == userId);

                //add each to return list
                var personJsonList = new JArray();
                foreach (var call in foundCalls) { personJsonList.Add(Person.FromAzureRow(call).ToJson()); }

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

        [Function(nameof(GetApiMeterList))]
        public static async Task<HttpResponseData> GetApiMeterList(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = $"{nameof(GetApiMeterList)}/UserId/{{userId}}")] HttpRequestData req,
            string userId)
        {
            try
            {


                //get all IP address linked to account user's id
                List<string> ipAddressList = GetIpAddressByUserId(userId);

                //sum all the api counts for given month for each found ip address
                var finalCount = 0;
                foreach (var ipAddress in ipAddressList)
                {
                    //get all counts for ip address in a given month
                    object monthNumber = null;
                    int countsForIp = GetCountsByIpAddressAndMonth(ipAddress, monthNumber);

                    //add together
                    finalCount += countsForIp;
                }


                var foundCalls = AzureTable.PersonList.Query<PersonListEntity>(call => call.PartitionKey == userId);

                //add each to return list
                var personJsonList = new JArray();
                foreach (var call in foundCalls) { personJsonList.Add(Person.FromAzureRow(call).ToJson()); }

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

        private static int GetCountsByIpAddressAndMonth(string ipAddress, object monthNumber)
        {
            throw new NotImplementedException();
        }

        private static List<string> GetIpAddressByUserId(string userId)
        {
                throw new NotImplementedException();
        }
    }
}
