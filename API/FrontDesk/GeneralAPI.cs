using VedAstro.Library;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using System.Net;
using System.Net.Http;

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
            string url = "https://vedastro.org/images/favicon.ico";

            //send to caller
            using (var client = new HttpClient())
            {
                var bytes = await client.GetByteArrayAsync(url);
                var response = incomingRequest.CreateResponse(HttpStatusCode.OK);
                
                //copy caller data from original caller if any, so calls are traceable
                CurrentCallerData.AddOriginalCallerHeadersIfAny(response);

                response.Headers.Add("Content-Type", "image/x-icon");
                await response.Body.WriteAsync(bytes, 0, bytes.Length);
                return response;
            }
        }




    }
}
