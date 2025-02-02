using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using VedAstro.Library;
using System.Net;
using Azure.Data.Tables;


namespace API
{
    /// <summary>
    /// Group of API calls related to maintaining the data integrity of the system.
    /// </summary>
    public static class MaintenanceAPI
    {


        /// <summary>
        /// scans through dates and rebuilds maps cache table
        /// </summary>
        [Function(nameof(ResetOpenAPIErrorBook))]
        public static async Task<HttpResponseData> ResetOpenAPIErrorBook([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "ResetOpenAPIErrorBook/Password/{password}")] HttpRequestData incomingRequest, string password)
        {

            // Only allow call if using admin password (throws exception if pass fails)
            Tools.PasswordProtect(password);

            // Get the client for the OpenAPIErrorBook table
            var openApiErrorBookClient = AzureTable.OpenAPIErrorBook;

            // Create a query to retrieve all entities in the table
            var query = openApiErrorBookClient.Query<TableEntity>();

            // Iterate through all entities and delete them
            foreach (var entity in query)
            {
                await openApiErrorBookClient.DeleteEntityAsync(entity.PartitionKey, entity.RowKey);
            }

            // Return a success message
            return APITools.PassMessageJson(incomingRequest);

        }

        /// <summary>
        /// to allow client to send match report and other files to email via a single call
        /// </summary>
        [Function(nameof(SendFileToEmail))]
        public static async Task<HttpResponseData> SendFileToEmail([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "Send/{fileName}/{fileFormat}/{receiverEmail}")] HttpRequestData incomingRequest,
            string fileName, string fileFormat, string receiverEmail)
        {
            ApiStatistic.Log(incomingRequest); //logger


            try
            {
                //log the call todo log causes errors in reading body, maybe read first
                //APILogger.Visitor(incomingRequest);

                //extract file from request
                var rawFileBytes = incomingRequest.Body;

                //if no file received, end here
                if (rawFileBytes.Length <= 0) { return APITools.FailMessage("No File Received!", incomingRequest); }

                //using Azure Email Sender, send file to given email
                APITools.SendEmail(fileName, fileFormat, receiverEmail, rawFileBytes);

                return APITools.PassMessageJson("Email sent success", incomingRequest);
            }
            catch (Exception e)
            {
                //log it
                APILogger.Error(e);

                //let user know
                return APITools.FailMessageJson(e, incomingRequest);
            }
        }

        /// <summary>
        /// Function for debugging purposes
        /// Call to see if return correct IP
        /// </summary>
        [Function(nameof(GetIpAddress))]
        public static HttpResponseData GetIpAddress([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequestData incomingRequest)
        {
            ApiStatistic.Log(incomingRequest); //logger

            try
            {
                return APITools.PassMessageJson(incomingRequest?.GetCallerIp()?.ToString() ?? "no ip", incomingRequest);
            }
            catch (Exception e)
            {
                //log it
                APILogger.Error(e);

                //let user know
                return APITools.FailMessageJson(e, incomingRequest);
            }


        }

        [Function("version")]
        public static HttpResponseData GetVersion([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequestData incomingRequest)
        {
            ApiStatistic.Log(incomingRequest); //logger


            var response = incomingRequest.CreateResponse(HttpStatusCode.OK);

            //place in response body
            response.WriteString(ThisAssembly.CommitNumber);

            return response;

        }



    }
}