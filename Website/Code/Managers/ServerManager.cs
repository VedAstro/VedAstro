using System.Net;
using Genso.Astrology.Library;
using System.Text;
using System.Xml.Linq;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Newtonsoft.Json;

namespace Website
{

    /// <summary>
    /// Encapsulates all thing to do with server (API)
    /// </summary>
    public static class ServerManager
    {

        public const string AddPersonApi = "https://vedastroapi.azurewebsites.net/api/addperson";
        public const string GetHoroscope = "https://vedastroapi.azurewebsites.net/api/gethoroscope";
        public const string AddLifeEventApi = "https://vedastroapi.azurewebsites.net/api/addlifeevent";
        public const string AddMessageApi = "https://vedastroapi.azurewebsites.net/api/addmessage";
        public const string DeletePersonApi = "https://vedastroapi.azurewebsites.net/api/deleteperson";
        public const string DeleteVisitorByUserId = "https://vedastroapi.azurewebsites.net/api/deletevisitorbyuserid";
        public const string DeleteVisitorByVisitorId = "https://vedastroapi.azurewebsites.net/api/deletevisitorbyvisitorid";
        public const string AddTaskApi = "https://vedastroapi.azurewebsites.net/api/addtask";
        public const string AddVisitorApi = "https://vedastroapi.azurewebsites.net/api/addvisitor";

        public const string GetMaleListApi = "https://vedastroapi.azurewebsites.net/api/getmalelist";
        public const string GetPersonListApi = "https://vedastroapi.azurewebsites.net/api/getpersonlist";
        public const string GetPersonApi = "https://vedastroapi.azurewebsites.net/api/getperson";
        public const string GetPersonIdFromSavedChartHash = "https://vedastroapi.azurewebsites.net/api/getpersonidfromsavedcharthash";

        public const string UpdatePersonApi = "https://vedastroapi.azurewebsites.net/api/updateperson";
        public const string GetTaskListApi = "https://vedastroapi.azurewebsites.net/api/gettasklist";
        public const string GetVisitorList = "https://vedastroapi.azurewebsites.net/api/getvisitorlist";
        public const string GetMessageList = "https://vedastroapi.azurewebsites.net/api/getmessagelist";
        public const string GetFemaleListApi = "https://vedastroapi.azurewebsites.net/api/getfemalelist";
        public const string GetMatchReportApi = "https://vedastroapi.azurewebsites.net/api/getmatchreport";
        public const string GetPersonEventsReport = "https://vedastroapi.azurewebsites.net/api/getpersoneventsreport";
        public const string GetSavedPersonEventsReport = "https://vedastroapi.azurewebsites.net/api/getsavedpersoneventsreport";
        public const string GetSavedChartIdList = "https://vedastroapi.azurewebsites.net/api/getsavedchartnamelist";
        public const string SavePersonEventsReport = "https://vedastroapi.azurewebsites.net/api/savepersoneventsreport";
        public const string GetPersonDasaReportCached = "https://vedastroapi.azurewebsites.net/api/getpersondasareportcached";
        public const string GetPersonDasaReportLocal = "http://localhost:7071/api/getpersondasareport";
        public const string GetEventsApi = "https://vedastroapi.azurewebsites.net/api/getevents";
        public const string GetGeoLocation = "https://get.geojs.io/v1/ip/geo.json";
        //TODO HIDE API
        public const string GoogleGeoLocationApiKey = "AIzaSyDqBWCqzU1BJenneravNabDUGIHotMBsgE";
        /// <summary>
        /// link to js file used for google sign in function
        /// </summary>
        public const string GoogleSignInJs = "https://accounts.google.com/gsi/client";
        public const string SignInGoogle = "https://vedastroapi.azurewebsites.net/api/SignInGoogle";
        public const string SignInFacebook = "https://vedastroapi.azurewebsites.net/api/SignInFacebook";
        public const string Paypal = "https://www.paypal.com/sdk/js?client-id=sb&enable-funding=venmo&currency=USD";
        
        /// <summary>
        /// Keep track of calls waiting in line,
        /// to by pass if one call holds the que
        /// </summary>
        private static int _waitingInLineCount = 0;

        /// <summary>
        /// Shows if any active connection is on, used to enforce 1 call at a time
        /// </summary>
        public static bool IsBusy = false;

        //PUBLIC METHODS

        /// <summary>
        /// Calls a URL and returns the content of the result as XML
        /// Even if content is returned as JSON, it is converted to XML
        /// Note: if JSON auto adds "Root" as first element, unless specified
        /// for XML data root element name is ignored
        /// </summary>
        public static async Task<XElement> ReadFromServerXmlReply(string apiUrl, IJSRuntime? jsRuntime, string rootElementName = "Root")
        {
            await IfBusyPleaseHold(apiUrl);

            //set busy
            IsBusy = true;


            //if js runtime available & browser offline show error
            jsRuntime?.CheckInternet();
            string rawMessage = "";

            try
            {
                //send request to API server
                var result = await RequestServer(apiUrl);

                //parse data reply
                rawMessage = result.Content.ReadAsStringAsync().Result;

                //raw message can be JSON or XML
                //try parse as XML if fail then as JSON
                var readFromServerXmlReply = XElement.Parse(rawMessage);

                //set free
                IsBusy = false;

                return readFromServerXmlReply;
            }
            catch (Exception)
            {
                //try to parse data as JSON
                try
                {
                    var rawXml = JsonConvert.DeserializeXmlNode(rawMessage, rootElementName);
                    var readFromServerXmlReply = XElement.Parse(rawXml.InnerXml);

                    //set free
                    IsBusy = false;

                    return readFromServerXmlReply;
                }
                //unparseable data, let user know
                catch (Exception e)
                {
                    //set free
                    IsBusy = false;

                    throw new ApiCommunicationFailed($"ReadFromServerXmlReply()\n{rawMessage}", e);
                }
            }



            // FUNCTIONS

            async Task<HttpResponseMessage> RequestServer(string receiverAddress)
            {
                //prepare the data to be sent
                var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, receiverAddress);

                //get the data sender
                using var client = new HttpClient();

                //tell sender to wait for complete reply before exiting
                var waitForContent = HttpCompletionOption.ResponseContentRead;

                //send the data on its way
                var response = await client.SendAsync(httpRequestMessage, waitForContent);

                //return the raw reply to caller
                return response;
            }
        }

        /// <summary>
        /// Send xml as string to server and returns stream as response
        /// </summary>
        public static async Task<Stream> WriteToServerStreamReply(string apiUrl, XElement xmlData, IJSRuntime? jsRuntime)
        {
            await IfBusyPleaseHold(apiUrl);

            //set busy
            IsBusy = true;

            //if js runtime available & browser offline show error
            jsRuntime?.CheckInternet();

            try
            {
                //prepare the data to be sent
                var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, apiUrl);

                httpRequestMessage.Content = XmLtoHttpContent(xmlData);

                //get the data sender
                using var client = new HttpClient();

                //tell sender to wait for complete reply before exiting
                var waitForContent = HttpCompletionOption.ResponseContentRead;

                //send the data on its way
                var response = await client.SendAsync(httpRequestMessage, waitForContent);

                //extract the content of the reply data
                var rawMessage = response.Content.ReadAsStreamAsync().Result;

                //set free
                IsBusy = false;

                return rawMessage;

            }
            //rethrow specialized exception to be handled by caller
            catch (Exception e)
            {
                //set free
                IsBusy = false;

                throw new ApiCommunicationFailed($"WriteToServerStreamReply()", e);
            }

        }

        /// <summary>
        /// Send xml as string to server and returns xml as response
        /// Note: xml is not checked here, just converted
        /// NOTEl: No timeout! Will wait forever
        /// </summary>
        public static async Task<XElement> WriteToServerXmlReply(string apiUrl, XElement xmlData, IJSRuntime? jsRuntime)
        {
            await IfBusyPleaseHold(apiUrl);

            //set busy
            IsBusy = true;

            //if js runtime available & browser offline show error
            jsRuntime?.CheckInternet();

            string rawMessage = "";
            var statusCode = "";

            try
            {
                //prepare the data to be sent
                var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, apiUrl);

                httpRequestMessage.Content = XmLtoHttpContent(xmlData);

                //get the data sender
                using var client = new HttpClient();

                //tell sender to wait for complete reply before exiting
                var waitForContent = HttpCompletionOption.ResponseContentRead;

                //send the data on its way (wait forever no timeout)
                client.Timeout = new TimeSpan(0, 0, 0, 0, Timeout.Infinite);
                var response = await client.SendAsync(httpRequestMessage, waitForContent);
                statusCode = response?.StatusCode.ToString();

                //extract the content of the reply data
                //todo await instead of result testing needed
                rawMessage = response?.Content.ReadAsStringAsync().Result ?? "";

                //problems might occur when parsing
                //try to parse as XML
                var writeToServerXmlReply = XElement.Parse(rawMessage);

                //set free
                IsBusy = false;

                return writeToServerXmlReply;
            }

            //note: failure here could be for several very likely reasons,
            //so it is important to properly check and handled here for best UX
            //- server unexpected failure
            //- server unreachable
            catch (Exception e)
            {
                //set free
                IsBusy = false;

                throw new ApiCommunicationFailed($"Error from WriteToServerXmlReply()\n{statusCode}\n{rawMessage}", e);
            }

        }


        /// <summary>
        /// Holds the control until line is clear
        /// enforces 1 call at a time
        /// check every 200ms
        /// </summary>
        /// <returns></returns>
        public static async Task IfBusyPleaseHold(string caller = "")
        {
            //note: experimentation has shown that long wait time causes serious lag
            //as que piles up, so many checks very fast seems to work perfectly so far

            //if waiting too long, move on
            while (IsBusy && _waitingInLineCount < 10)
            {
                Console.WriteLine($"BLZ:Waiting in line for call:{caller}");
                await Task.Delay(100);
                _waitingInLineCount++; //increment  count
            }

            //reset
            _waitingInLineCount = 0;
        }


        //PRIVATE METHODS
        /// <summary>
        /// Packages the data into ready form for the HTTP client to use in final sending stage
        /// </summary>
        private static StringContent XmLtoHttpContent(XElement data)
        {
            //gets the main XML data as a string
            var dataString = Tools.XmlToString(data);

            //specify the data encoding
            var encoding = Encoding.UTF8;

            //specify the type of the data sent
            //plain text, stops auto formatting
            var mediaType = "plain/text";

            //return packaged data to caller
            return new StringContent(dataString, encoding, mediaType);
        }



    }
}
