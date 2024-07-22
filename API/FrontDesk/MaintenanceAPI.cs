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
        /// scans through dates and rebuilds maps cache table
        /// </summary>
        [Function(nameof(RebuildGeoLocationTimezoneTable))]
        public static async Task<HttpResponseData> RebuildGeoLocationTimezoneTable([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "RebuildGeoLocationTimezoneTable")] HttpRequestData incomingRequest)
        {

            Tools.PasswordProtect("123"); //password is Spotty


            //get chart special API home page and send that to caller
            var APIHomePageTxt = "";

            return APITools.SendTextToCaller(APIHomePageTxt, incomingRequest);
        }

        /// <summary>
        /// API Home page
        /// </summary>
        [Function(nameof(Home))]
        public static async Task<HttpResponseData> Home([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "Home")] HttpRequestData incomingRequest)
        {
            
            ApiStatistic.Log(incomingRequest); //logger

            //get chart special API home page and send that to caller
            var apiHomePageTxt = await Tools.GetStringFileHttp(APITools.Url.APIHomePageTxt);

            return APITools.SendTextToCaller(apiHomePageTxt, incomingRequest);
        }

        /// <summary>
        /// wrapper place to run 1 time debug code
        /// </summary>
        [Function(nameof(DEBUG))]
        public static async Task<HttpResponseData> DEBUG([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "DEBUG")] HttpRequestData incomingRequest)
        {
            throw new NotImplementedException();
            //      //get latest all match reports
            //      var personListXml = await Tools.GetXmlFileFromAzureStorage(Tools.PersonListFile, Tools.BlobContainerName);

            //      //list of person XMLs
            //      var personXmlList = personListXml?.Root?.Elements() ?? new List<XElement>();

            //      var personList = Person.FromXml(personXmlList);

            //      var xxx = ConvertListToCsv(personList);

            //return APITools.SendTextToCaller(xxx, incomingRequest);

            // string ConvertListToCsv(List<Person> people)
            //{
            //	var csv = new StringBuilder();
            //	// Add headers
            //	csv.AppendLine("Name,Gender,BirthDate,BirthLocation");
            //	foreach (var person in people)
            //	{
            //		var localNameClean = person.GetBirthLocation().Name().Replace(",", "");
            //		csv.AppendLine($"{person.Name.Truncate(5," ")},{person.Gender},{person.BirthTimeString},{localNameClean}");
            //	}
            //	return csv.ToString();
            //}

        }

        /// <summary>
        /// Searches for image and gives URL
        /// </summary>
        [Function(nameof(SearchImage))]
        public static async Task<HttpResponseData> SearchImage([HttpTrigger(AuthorizationLevel.Anonymous, "get",
            Route = "SearchImage/Keywords/{keywords}")] HttpRequestData incomingRequest,
            string keywords)
        {
            ApiStatistic.Log(incomingRequest); //logger

            //IMPORTANT: replace this variable with your Cognitive Services subscription key
            string subscriptionKey = Secrets.Get("BING_IMAGE_SEARCH");
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
            HttpRequestData incomingRequest,
            string callerId, string formatName)
        {
            ApiStatistic.Log(incomingRequest); //logger

            if (formatName.ToLower() == "json")
            {
                string jsonText = await AzureCache.GetData<string>(callerId);

                var response = incomingRequest.CreateResponse(HttpStatusCode.OK);
                response.Headers.Add("Content-Type", MediaTypeNames.Application.Json);

                //place in response body
                await response.WriteStringAsync(jsonText);

                return response;
            }

            else if (formatName.ToLower() == "gif")
            {
                //for images get and send direct with as less operations as possible
                var fileBlobClient = await AzureCache.GetData<BlobClient>(callerId);

                return Tools.SendFileToCaller(fileBlobClient, incomingRequest, MediaTypeNames.Image.Gif);
            }
            else if (formatName.ToLower() == "svg")
            {
                //for images get and send direct with as less operations as possible
                var fileBlobClient = await AzureCache.GetData<BlobClient>(callerId);

                return Tools.SendFileToCaller(fileBlobClient, incomingRequest, "image/svg+xml");
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
        [Function("getipaddress")]
        public static HttpResponseData GetIpAddress([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequestData incomingRequest)
        {
            ApiStatistic.Log(incomingRequest); //logger

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
            ApiStatistic.Log(incomingRequest); //logger


            var response = incomingRequest.CreateResponse(HttpStatusCode.OK);

            //place in response body
            response.WriteString(ThisAssembly.CommitNumber);

            return response;

            //try
            //{
            //    var holder = new XElement("Root");
            //    var versionNumberXml = new XElement("Version", ThisAssembly.Version);
            //    holder.Add(versionNumberXml, Tools.TimeStampServerXml);

            //    return APITools.PassMessage(holder, incomingRequest);

            //}
            //catch (Exception e)
            //{
            //    //log it
            //    APILogger.Error(e);

            //    //let user know
            //    return APITools.FailMessageJson(e, incomingRequest);
            //}


        }

        [Function("Stats")]
        public static async Task<HttpResponseData> GetStats([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequestData incomingRequest)
        {
            ApiStatistic.Log(incomingRequest); //logger


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
            ApiStatistic.Log(incomingRequest); //logger

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