using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using VedAstro.Library;
using System.Net;
using Microsoft.Bing.ImageSearch;
using Microsoft.Bing.ImageSearch.Models;
using Newtonsoft.Json.Linq;
using Azure.Data.Tables;

namespace API
{
    /// <summary>
    /// Group of API calls related to user's API subscription
    /// </summary>
    public static class SubscriptionAPI
    {

        /// <summary>
        /// scans through dates and rebuilds maps cache table
        /// </summary>
        [Function(nameof(RegisterSubscription))]
        public static async Task<HttpResponseData> RegisterSubscription([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "RegisterSubscription/OwnerId/{ownerId}/APIKey/{apiKey}/")] HttpRequestData incomingRequest, string ownerId, string apiKey)
        {

            try
            {
                // Search for existing record by OwnerId (PartitionKey)
                var tableClient = AzureTable.UserDataList;
                var filter = $"PartitionKey eq '{ownerId}'";
                var query = tableClient.Query<UserDataListEntity>(filter: filter);
                var existingRecord = query.FirstOrDefault();

                if (existingRecord != null)
                {
                    // Update API key if record exists
                    existingRecord.APIKey = apiKey;
                    await tableClient.UpsertEntityAsync(existingRecord);
                }
                else
                {
                    // Return error if no record found
                    return APITools.FailMessageJson("No record found for specified OwnerId", incomingRequest);
                }

                return APITools.PassMessageJson("API key updated successfully", incomingRequest);
            }
            catch (Exception e)
            {
                APILogger.Error(e, incomingRequest);
                return APITools.FailMessageJson(e.Message, incomingRequest);
            }
        }

    }
}