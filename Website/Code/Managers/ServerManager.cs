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

        public const string UpdatePersonApi = "https://vedastroapi.azurewebsites.net/api/updateperson";
        public const string GetTaskListApi = "https://vedastroapi.azurewebsites.net/api/gettasklist";
        public const string GetVisitorList = "https://vedastroapi.azurewebsites.net/api/getvisitorlist";
        public const string GetMessageList = "https://vedastroapi.azurewebsites.net/api/getmessagelist";
        public const string GetFemaleListApi = "https://vedastroapi.azurewebsites.net/api/getfemalelist";
        public const string GetMatchReportApi = "https://vedastroapi.azurewebsites.net/api/getmatchreport";
        public const string GetPersonDasaReport = "https://vedastroapi.azurewebsites.net/api/getpersondasareport";
        public const string GetPersonEventsReport = "https://vedastroapi.azurewebsites.net/api/getpersoneventsreport";
        public const string GetPersonDasaReportCached = "https://vedastroapi.azurewebsites.net/api/getpersondasareportcached";
        public const string GetPersonDasaReportLocal = "http://localhost:7071/api/getpersondasareport";
        public const string GetEventsApi = "https://vedastroapi.azurewebsites.net/api/getevents";
        public const string GetGeoLocation = "https://get.geojs.io/v1/ip/geo.json";
        public const string GoogleGeoLocationApiKey = "AIzaSyDqBWCqzU1BJenneravNabDUGIHotMBsgE";
        /// <summary>
        /// link to js file used for google sign in function
        /// </summary>
        public const string GoogleSignInJs = "https://accounts.google.com/gsi/client";
        public const string SignInGoogle = "https://vedastroapi.azurewebsites.net/api/SignInGoogle";
        public const string SignInFacebook = "https://vedastroapi.azurewebsites.net/api/SignInFacebook";
        public const string Paypal = "https://www.paypal.com/sdk/js?client-id=sb&enable-funding=venmo&currency=USD";


        //PUBLIC METHODS

        /// <summary>
        /// Calls a URL and returns the content of the result as XML
        /// Even if content is returned as JSON, it is converted to XML
        /// Note: if JSON auto adds "Root" as first element, unless specified
        /// for XML data root element name is ignored
        /// </summary>
        public static async Task<XElement> ReadFromServerXmlReply(string apiUrl, IJSRuntime? jsRuntime, string rootElementName = "Root")
        {
            //if js runtime available & browser offline show error
            jsRuntime?.CheckInternet();

            //send request to API server
            var result = await RequestServer(apiUrl);

            //parse data reply
            var rawMessage = result.Content.ReadAsStringAsync().Result;

            //raw message can be JSON or XML
            //try parse as XML if fail then as JSON
            try { return XElement.Parse(rawMessage); }
            catch (Exception)
            {
                //try to parse data as JSON
                try
                {
                    var rawXml = JsonConvert.DeserializeXmlNode(rawMessage, rootElementName);
                    return XElement.Parse(rawXml.InnerXml);
                }
                //unparseable data, let user know
                catch (Exception e)
                {
                    Console.WriteLine(rawMessage);
                    throw;
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
            //if js runtime available & browser offline show error
            jsRuntime?.CheckInternet();

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

            return rawMessage;
        }

        /// <summary>
        /// Send xml as string to server and returns xml as response
        /// Note: xml is not checked here, just converted
        /// NOTEl: No timeout! Will wait forever
        /// </summary>
        public static async Task<XElement> WriteToServerXmlReply(string apiUrl, XElement xmlData, IJSRuntime? jsRuntime)
        {
            //if js runtime available & browser offline show error
            jsRuntime?.CheckInternet();

            string rawMessage = "";
            HttpResponseMessage response = null;

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
                response = await client.SendAsync(httpRequestMessage, waitForContent);

                //extract the content of the reply data
                rawMessage = response.Content.ReadAsStringAsync().Result;
                
                //problems might occur when parsing
                //try to parse as XML
                return XElement.Parse(rawMessage);
            }

            //note: failure here could be for several very likely reasons,
            //so it is important to properly check and handled here for best UX
            //- server unexpected failure
            //- server unreachable
            catch (Exception e)
            {
                //log error, don't await to reduce lag
                WebsiteLogManager.LogError(e, $"Error from WriteToServerXmlReply()\n{response?.StatusCode}");

                //if possible show error to user & reload page
                if (jsRuntime is not null)
                {
                    //failure here can't be recovered, so best choice is to refresh page to home
                    await jsRuntime.ShowAlert("warning", AlertText.SorryNeedRefreshToHome, true, 0);
                    await jsRuntime.LoadPage(PageRoute.Home);
                }

                //throw exception to stop execution
                //and let user be reloaded to home via alert
                throw;
            }

        }


        /// <summary>
        /// Checks if Status is Pass or Fail in Root xml
        /// </summary>
        public static bool IsReplyPass(XElement rootXml) => rootXml.Element("Status")?.Value == "Pass";

        /// <summary>
        /// Gets first child element in "Payload" element
        /// </summary>
        public static XElement? GetPayload(XElement rootXml) => rootXml.Element("Payload")?.Elements()?.FirstOrDefault();


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
