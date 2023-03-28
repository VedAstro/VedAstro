using System.Threading.Tasks;
using System.Xml.Linq;
using VedAstro.Library;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

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
