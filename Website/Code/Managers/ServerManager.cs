using Genso.Astrology.Library;
using System.Xml.Linq;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Components.WebAssembly.Http;

namespace Website
{

    /// <summary>
    /// Encapsulates all thing to do with server (API)
    /// </summary>
    public static class ServerManager
    {

        //TODO HIDE API
        public const string GoogleGeoLocationApiKey = "AIzaSyDqBWCqzU1BJenneravNabDUGIHotMBsgE";


        //PUBLIC METHODS

        /// <summary>
        /// Calls a URL and returns the content of the result as XML
        /// Even if content is returned as JSON, it is converted to XML
        /// Note: if JSON auto adds "Root" as first element, unless specified
        /// for XML data root element name is ignored
        /// </summary>
        public static async Task<WebResult<XElement>> ReadFromServerXmlReply(string apiUrl, IJSRuntime? jsRuntime, string rootElementName = "Root")
        {
            //if js runtime available & browser offline show error
            jsRuntime?.CheckInternet();

            //send request to API server
            var result = await RequestServer(apiUrl);

            //get raw reply from the server response
            var rawMessage = await result.Content?.ReadAsStringAsync() ?? "";

            //only good reply from server is accepted, anything else is marked invalid
            //stops invalid replies from being passed as valid
            if (!result.IsSuccessStatusCode) { return new WebResult<XElement>(false, new("RawErrorData", rawMessage)); }

            //tries to parse the raw data received into XML or JSON
            //if all fail will return raw data with fail status
            var parsed = ParseData(rawMessage);


            return parsed;


            //----------------------------------------------------------
            // FUNCTIONS

            WebResult<XElement> ParseData(string inputRawString)
            {
                var exceptionList = new List<Exception>();

                try
                {
                    //OPTION 1 : xml with VedAstro standard reply
                    var parsedXml = XElement.Parse(inputRawString);
                    var returnVal = WebResult<XElement>.FromXml(parsedXml);
                    return returnVal;
                }
                catch (Exception e1)
                {
                    try
                    {
                        //OPTION 2 : xml 3rd party reply (google)
                        var parsedXml = XElement.Parse(inputRawString);
                        return new WebResult<XElement>(true, parsedXml);
                    }
                    catch (Exception e2) { exceptionList.Add(e2); }

                    try
                    {
                        //OPTION 3 : json 3rd party reply
                        var parsedJson = JsonConvert.DeserializeXmlNode(inputRawString, "LocationData");
                        var wrappedXml = XElement.Parse(parsedJson.InnerXml); //expected to fail if not right
                        return new WebResult<XElement>(true, wrappedXml);
                    }
                    catch (Exception e3) { exceptionList.Add(e3); } //if fail just void print

                    exceptionList.Add(e1);

                    //send all exception data to server
                    foreach (var exception in exceptionList) { WebLogger.Error(exception, inputRawString); }

                    //if control reaches here all has failed
                    return new WebResult<XElement>(false, new XElement("Failed"));
                }
            }

            async Task<HttpResponseMessage> RequestServer(string receiverAddress)
            {
                //prepare the data to be sent
                var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, receiverAddress);
                httpRequestMessage.SetBrowserRequestMode(BrowserRequestMode.NoCors); //NO CORS!

                //tell sender to wait for complete reply before exiting
                var waitForContent = HttpCompletionOption.ResponseContentRead;

                //send the data on its way
                var response = await AppData.HttpClient.SendAsync(httpRequestMessage, waitForContent);

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

            try
            {
                //prepare the data to be sent
                var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, apiUrl);
                httpRequestMessage.SetBrowserRequestMode(BrowserRequestMode.NoCors); //NO CORS!

                httpRequestMessage.Content = Tools.XmLtoHttpContent(xmlData);

                //get the data sender
                //using var client = new HttpClient();

                //tell sender to wait for complete reply before exiting
                var waitForContent = HttpCompletionOption.ResponseContentRead;

                //send the data on its way
                var response = await AppData.HttpClient.SendAsync(httpRequestMessage, waitForContent);

                //extract the content of the reply data
                var rawMessage = response.Content.ReadAsStreamAsync().Result;

                return rawMessage;

            }
            //rethrow specialized exception to be handled by caller
            catch (Exception e) { throw new ApiCommunicationFailed($"WriteToServerStreamReply()", e); }

        }

        /// <summary>
        /// TODO DEPRECATED MARKED FOR DELETION
        /// Send xml as string to server and returns xml as response
        /// Note:
        /// - on failure payload will contain error info
        /// - xml is not checked here, just converted
        /// - No timeout! Will wait forever
        /// - failure is logged here
        /// </summary>
        public static async Task<WebResult<XElement>> WriteToServerXmlReplyDotNet(string apiUrl, XElement xmlData, IJSRuntime? jsRuntime)
        {

            WebResult<XElement> returnVal;

            //if js runtime available & browser offline show error
            jsRuntime?.CheckInternet();

            string rawMessage = "";
            var statusCode = "";

            try
            {
                //prepare the data to be sent
                var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, apiUrl);
                httpRequestMessage.SetBrowserRequestMode(BrowserRequestMode.NoCors); //NO CORS!
                httpRequestMessage.Content = Tools.XmLtoHttpContent(xmlData);

                //tell sender to wait for complete reply before exiting
                var waitForContent = HttpCompletionOption.ResponseContentRead;

                //send the data on its way (wait forever no timeout)
                //client.Timeout = new TimeSpan(0, 0, 0, 0, Timeout.Infinite);
                var response = await AppData.HttpClient.SendAsync(httpRequestMessage, waitForContent);
                statusCode = response?.StatusCode.ToString();

                //extract the content of the reply data
                rawMessage = await response?.Content.ReadAsStringAsync() ?? "";

                //problems might occur when parsing
                //try to parse as XML
                var writeToServerXmlReply = XElement.Parse(rawMessage);
                returnVal = WebResult<XElement>.FromXml(writeToServerXmlReply);

            }
            //if internet failure let caller know immediately
            catch (HttpRequestException e)
            {
#if DEBUG
                Console.WriteLine(e.Message);
#endif
                throw new NoInternetError();
            }

            //note: other failure here could be for several very likely reasons,
            //so it is important to properly check and handled here for best UX
            //- server unexpected failure
            //- server unreachable
            catch (Exception)
            {
                returnVal = new WebResult<XElement>(false, new XElement("Root", $"Error from WriteToServerXmlReply()\n{statusCode}\n{rawMessage}"));
            }

            //if fail log it
            if (!returnVal.IsPass) { WebLogger.Error(returnVal.Payload); }

            return returnVal;
        }

        /// <summary>
        /// HTTP Post via JS interop
        /// </summary>
        public static async Task<WebResult<XElement>> WriteToServerXmlReply(string apiUrl, XElement xmlData)
        {
            //ACT 1:
            //send data to URL, using JS for speed
            var receivedData = await WebsiteTools.Post(apiUrl, xmlData);

            //ACT 2:
            //check raw data 
            //todo handle error better
            if (string.IsNullOrEmpty(receivedData)) { throw new ApiCommunicationFailed(); }

            //ACT 2:
            //return data as XML
            //problems might occur when parsing
            //try to parse as XML
            var writeToServerXmlReply = XElement.Parse(receivedData);
            var returnVal = WebResult<XElement>.FromXml(writeToServerXmlReply);

            //ACT 3:
            return returnVal;
        }


        ///// <summary>
        ///// Holds the control until line is clear
        ///// enforces 1 call at a time
        ///// check every 200ms
        ///// </summary>
        ///// <returns></returns>
        //public static async Task IfBusyPleaseHold(string caller = "")
        //{
        //    //note: experimentation has shown that long wait time causes serious lag
        //    //as que piles up, so many checks very fast seems to work perfectly so far

        //    //if waiting too long, move on
        //    while (IsBusy && _waitingInLineCount < 10)
        //    {
        //        Console.WriteLine($"BLZ:Waiting in line for call:{caller}");
        //        await Task.Delay(100);
        //        _waitingInLineCount++; //increment  count
        //    }

        //    //reset
        //    _waitingInLineCount = 0;
        //}


        //PRIVATE METHODS



    }
}
