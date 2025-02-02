using VedAstro.Library;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using System.Net;
using System.IO;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace API
{
    /// <summary>
    /// All API calls with no home are here, send them somewhere you think is good
    /// </summary>
    public class GeneralAPI
    {

        /// <summary>
        /// When browser visit API, they ask for FavIcon, so yeah redirect favicon from website
        /// </summary>
        [Function(nameof(FavIcon))]
        public static async Task<HttpResponseData> FavIcon([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "favicon.ico")] HttpRequestData incomingRequest)
        {
            //use same fav icon from website
            string url = URL.WebStable+"/images/favicon.ico";

            //send to caller
            using (var client = new HttpClient())
            {
                var bytes = await client.GetByteArrayAsync(url);
                var response = incomingRequest.CreateResponse(HttpStatusCode.OK);
                
                //copy caller data from original caller if any, so calls are traceable
                //CurrentCallerData.AddOriginalCallerHeadersIfAny(response);

                response.Headers.Add("Content-Type", "image/x-icon");
                await response.Body.WriteAsync(bytes, 0, bytes.Length);
                return response;
            }
        }

        /// <summary>
        /// API Home page
        /// </summary>
        [Function(nameof(Home))]
        public static async Task<HttpResponseData> Home([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "Home")] HttpRequestData incomingRequest)
        {

            ApiStatistic.Log(incomingRequest); //logger

            //get chart special API home page and send that to caller
            var apiHomePageTxt = await Tools.GetStringFileHttp(URL.WebStable + "/data/APIHomePage.html");

            return APITools.SendTextToCaller(apiHomePageTxt, incomingRequest);
        }

        private static readonly HttpClient httpClient = new HttpClient();

        /// <summary>
        /// Gets hash of VedAstro.js file located in direct azure storage
        /// </summary>
        [Function(nameof(GetVedAstroJSHash))]
        public static async Task<HttpResponseData> GetVedAstroJSHash(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "GetVedAstroJSHash")] HttpRequestData incomingRequest,
            FunctionContext executionContext)
        {
            //direct link to JS file without CDN
            string fileUrl = "https://vedastrowebsitestorage.z5.web.core.windows.net/js/VedAstro.js";
            string vedAstroJSHash;

            // Fetch the file from Azure Storage
            using (var response = await httpClient.GetAsync(fileUrl))
            {
                response.EnsureSuccessStatusCode();
                using (var stream = await response.Content.ReadAsStreamAsync())
                {
                    // Generate the hash
                    using (var sha256 = SHA256.Create())
                    {
                        var hash = sha256.ComputeHash(stream);
                        vedAstroJSHash = BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                    }
                }
            }

            return APITools.PassMessageJson(vedAstroJSHash, incomingRequest);
        }


        /// <summary>
        /// Backup function to catch invalid calls, say gracefully fails
        /// NOTE: "z" in name needed to make as last API call, else will be called all the time
        /// </summary>
        [Function(nameof(zCatch404))]
        public static async Task<HttpResponseData> zCatch404([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "{*Catch404}")]
            HttpRequestData incomingRequest,
            string Catch404
        )
        {
            //0 : LOG CALL
            //log ip address, call time and URL,  used later for throttle limit
            ApiStatistic.Log(incomingRequest); //logger

            // Control API overload, even this if hit hard can COST Money via CDN
            //await APITools.AutoControlOpenAPIOverload(callLog);

            var message = "Invalid or Outdated Call, please rebuild API URL at vedastro.org/APIBuilder.html";
            return APITools.FailMessageJson(message, incomingRequest);
        }


    }
}
