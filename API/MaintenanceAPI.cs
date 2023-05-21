using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.DurableTask.Client;
using System.Xml.Linq;
using VedAstro.Library;

namespace API
{
    /// <summary>
    /// Group of API calls related to maintaining the data integrity of the system.
    /// </summary>
    public static class MaintenanceAPI
    {
        /// <summary>
        /// API Home page
        /// </summary>
        [Function(nameof(Home))]
        public static async Task<HttpResponseData> Home([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "Home")] HttpRequestData incomingRequest)
        {
            //get chart special API home page and send that to caller
            var APIHomePageTxt = await APITools.GetStringFileHttp(APITools.Url.APIHomePageTxt);

            return APITools.SendTextToCaller(APIHomePageTxt, incomingRequest);
        }


        /// <summary>
        /// Call here after calling prepare chart or anything with ID
        /// </summary>
        [Function(nameof(GetCallStatus))]
        public static async Task<HttpResponseData> GetCallStatus(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "GetCallStatus/{callerId}")]
            HttpRequestData incomingRequest,
            [DurableClient] DurableTaskClient client,
            string callerId
        )
        {
            try
            {
                //try to get cached version, if not available then make new one
                var result = await client.GetInstanceAsync(callerId, false, CancellationToken.None);
                var statusMessage = result?.RuntimeStatus.ToString() ?? "No Exist";

                return APITools.PassMessageJson(statusMessage, incomingRequest);

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
        /// Clears all cache, run manually when code is updated and new data is needed
        /// </summary>
        [Function(nameof(ClearCache))]
        public static async Task<HttpResponseData> ClearCache(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]
            HttpRequestData req,
            [DurableClient] DurableTaskClient client)
        {
            var purgeInstancesFilter = new PurgeInstancesFilter(DateTimeOffset.MinValue); //get all
            var result = await client.PurgeAllInstancesAsync(purgeInstancesFilter, CancellationToken.None);

            var message = $"CLEARED COUNT:{result.PurgedInstanceCount}";

            return APITools.PassMessageJson(message, req);
        }


        /// <summary>
        /// to allow client to send match report and other files to email via a single call
        /// </summary>
        [Function(nameof(SendFileToEmail))]
        public static async Task<HttpResponseData> SendFileToEmail([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "Send/{fileName}/{fileFormat}/{receiverEmail}")] HttpRequestData incomingRequest,
            string fileName, string fileFormat, string receiverEmail)
        {
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
        [Function("getipaddress")]
        public static HttpResponseData GetIpAddress([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequestData incomingRequest)
        {

            try
            {
                return APITools.PassMessage(incomingRequest?.GetCallerIp()?.ToString() ?? "no ip", incomingRequest);
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


            try
            {
                var holder = new XElement("Root");
                var versionNumberXml = new XElement("Version", ThisAssembly.Version);
                holder.Add(versionNumberXml, Tools.TimeStampServerXml);

                return APITools.PassMessage(holder, incomingRequest);

            }
            catch (Exception e)
            {
                //log it
                APILogger.Error(e);

                //let user know
                return APITools.FailMessageJson(e, incomingRequest);
            }


        }


        [Function("Stats")]
        public static async Task<HttpResponseData> GetStats([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequestData incomingRequest)
        {


            try
            {
                //get visitor log from storage
                var visitorLogDocument = await APITools.GetXmlFileFromAzureStorage(APITools.VisitorLogFile, APITools.BlobContainerName);

                //unique visitors
                var uniqueList = APITools.GetOnlineVisitors(visitorLogDocument);

                //convert list to nice string before sending to caller
                var x = uniqueList.ToList();
                var visitorLogXmlString = Tools.ListToString(x);
                return APITools.PassMessage(visitorLogXmlString, incomingRequest);
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
        /// Build to be pinged by Render server for live build, but can be used by any for checking health
        /// </summary>
        [Function("health")]
        public static HttpResponseData GetHealth([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequestData incomingRequest)
        {

            try
            {
                //so long as respond as OK 200, then will pass (Render server)
                //todo real health check please
                //check if all data files are accessible

                var holder = new XElement("Root");
                var versionNumberXml = new XElement("Version", ThisAssembly.Version);
                holder.Add(versionNumberXml, Tools.TimeStampServerXml);

                return APITools.PassMessage(holder, incomingRequest);

            }
            catch (Exception e)
            {
                //log it
                APILogger.Error(e);

                //let user know
                return APITools.FailMessageJson(e, incomingRequest);
            }

        }
    }
}