using System.Threading.Tasks;
using System.Xml.Linq;
using Azure.Communication.Email;
using VedAstro.Library;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Newtonsoft.Json.Linq;
using Azure;
using System.Net.Http;

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
        public static async Task<HttpResponseData> Home([HttpTrigger(AuthorizationLevel.Anonymous, "get", "put", "delete", "post", "head", "trace", "patch", "connect", "options", Route = "Home")] HttpRequestData incomingRequest)
        {

            //get chart special API home page and send that to caller
            var APIHomePageTxt = await APITools.GetStringFileHttp(APITools.Url.APIHomePageTxt);

            return APITools.SendTextToCaller(APIHomePageTxt, incomingRequest);

        }


        /// <summary>
        /// to allow client to send match report and other files to email via a single call 
        /// </summary>
        [Function(nameof(SendFileToEmail))]
        public static async Task<HttpResponseData> SendFileToEmail([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "Send/{fileName}/{fileFormat}/{receiverEmail}")] HttpRequestData incomingRequest,
            string fileName,string fileFormat,string receiverEmail)
        {

            try
            {
                //log the call todo log causes errors in reading body, maybe read first
                //APILogger.Visitor(incomingRequest);

                //extract file from request
                var rawFileBytes = incomingRequest.Body;

                //using Azure Email Sender, send file to given email
                APITools.SendEmail(fileName, fileFormat, receiverEmail, rawFileBytes);

                JToken jsonReply = JToken.Parse($"'Email sent to -> {receiverEmail}'"); //empty default

                return APITools.PassMessageJson(jsonReply, incomingRequest);

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
            return APITools.PassMessage(incomingRequest?.GetCallerIp()?.ToString() ?? "no ip", incomingRequest);
        }


        [Function("version")]
        public static HttpResponseData GetVersion([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequestData incomingRequest)
        {
            var holder = new XElement("Root");
            var versionNumberXml = new XElement("Version", ThisAssembly.Version);
            holder.Add(versionNumberXml, Tools.TimeStampServerXml);

            return APITools.PassMessage(holder, incomingRequest);
        }

        /// <summary>
        /// Build to be pinged by Render server for live build, but can be used by any for checking health
        /// </summary>
        [Function("health")]
        public static HttpResponseData GetHealth([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequestData incomingRequest)
        {
            //so long as respond as OK 200, then will pass
            //todo real health check please
            //check if all data files are accessible

            var holder = new XElement("Root");
            var versionNumberXml = new XElement("Version", ThisAssembly.Version);
            holder.Add(versionNumberXml, Tools.TimeStampServerXml);

            return APITools.PassMessage(holder, incomingRequest);
        }

    }
}
