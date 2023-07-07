using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using System.Xml.Linq;
using VedAstro.Library;
using System.Net.Mime;
using System.Net;
using Azure.Storage.Blobs;
using Microsoft.Bing.ImageSearch;
using Microsoft.Bing.ImageSearch.Models;
using Newtonsoft.Json.Linq;

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
        /// API Home page
        /// </summary>
        [Function(nameof(SearchImage))]
        public static async Task<HttpResponseData> SearchImage([HttpTrigger(AuthorizationLevel.Anonymous, "get",
            Route = "SearchImage/Keywords/{keywords}")] HttpRequestData incomingRequest,
            string keywords)
        {
            //IMPORTANT: replace this variable with your Cognitive Services subscription key
            string subscriptionKey = Secrets.BING_IMAGE_SEARCH;
            //stores the image results returned by Bing
            Images imageResults = null;

            var client = new ImageSearchClient(new ApiKeyServiceClientCredentials(subscriptionKey));

            // make the search request to the Bing Image API, and get the results"
            imageResults = await client.Images.SearchAsync(query: keywords); //search query

            //get only jpeg images for ease of handling down the road
            var jpegOnly = imageResults.Value.Where(x => x.EncodingFormat == "jpeg");


            var possibleImages = new JArray();
            foreach (var image in jpegOnly)
            {            
                //pack data nicely
                var temp = new JObject();
                temp["Name"] = image.Name; //keywords to image
                temp["URL"] = image.ThumbnailUrl; //show as number

                //add to main list
                possibleImages.Add(temp);
            }

            return APITools.PassMessageJson(possibleImages, incomingRequest);

        }




        /// <summary>
        /// designed to be called directly, getting ANY and ALL needed data in one simple GET call
        /// </summary>
        [Function(nameof(GetCallData))]
        public static async Task<HttpResponseData> GetCallData(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "GetCallData/CallerId/{callerId}/Format/{formatName}")]
            HttpRequestData req,
            string callerId, string formatName)
        {

            if (formatName.ToLower() == "json")
            {
                string jsonText = await AzureCache.GetData<string>(callerId);

                var response = req.CreateResponse(HttpStatusCode.OK);
                response.Headers.Add("Content-Type", MediaTypeNames.Application.Json);

                //place in response body
                await response.WriteStringAsync(jsonText);

                return response;
            }

            else if (formatName.ToLower() == "gif")
            {
                //for images get and send direct with as less operations as possible
                var fileBlobClient = await AzureCache.GetData<BlobClient>(callerId);

                return APITools.SendFileToCaller(fileBlobClient, req, MediaTypeNames.Image.Gif);
            }
            else if (formatName.ToLower() == "svg")
            {
                //for images get and send direct with as less operations as possible
                var fileBlobClient = await AzureCache.GetData<BlobClient>(callerId);

                return APITools.SendFileToCaller(fileBlobClient, req, "image/svg+xml");
            }

            throw new Exception("END OF THE LINE");
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
                var visitorLogDocument = await Tools.GetXmlFileFromAzureStorage(APITools.VisitorLogFile, Tools.BlobContainerName);

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