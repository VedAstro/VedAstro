using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.JSInterop;

//A LIE IS JUST A GREAT STORY THAT SOMEONE RUINED WITH THE TRUTH -- Barnabus Stinson

namespace VedAstro.Library
{

    /// <summary>
    /// Encapsulates all thing to do with server (API)
    /// </summary>
    public static class ServerManager
    {

        //PUBLIC METHODS

       

        /// <summary>
        /// Send xml as string to server and returns stream as response
        /// </summary>
        public static async Task<Stream> WriteToServerStreamReply(string apiUrl, XElement xmlData, IJSRuntime? jsRuntime, HttpClient httpClient)
        {

            try
            {
                //prepare the data to be sent
                var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, apiUrl);

                httpRequestMessage.Content = Tools.XmLtoHttpContent(xmlData);

                //tell sender to wait for complete reply before exiting
                var waitForContent = HttpCompletionOption.ResponseContentRead;

                //send the data on its way
                var response = await httpClient.SendAsync(httpRequestMessage, waitForContent);

                //extract the content of the reply data
                var rawMessage = response.Content.ReadAsStreamAsync().Result;

                return rawMessage;

            }
            //rethrow specialized exception to be handled by caller
            catch (Exception e) { throw new ApiCommunicationFailed($"WriteToServerStreamReply()", e); }

        }


        /// <summary>
        /// HTTP Post via JS interop
        /// </summary>
        public static async Task<WebResult<XElement>> WriteToServerXmlReply(string apiUrl, XElement xmlData, int timeout = 60)
        {


        TryAgain:

            //ACT 1:
            //send data to URL, using JS for reliability & speed
            //also if call does not respond in time, we replay the call over & over
            string receivedData;
            try { receivedData = await Tools.TaskWithTimeoutAndException(Post(apiUrl, xmlData.ToString(SaveOptions.DisableFormatting)), TimeSpan.FromSeconds(timeout)); }

            //if fail replay and log it
            catch (Exception e)
            {
                var debugInfo = $"Call to \"{apiUrl}\" timeout at : {timeout}s";

                LibLogger.Debug(debugInfo);
#if DEBUG
                Console.WriteLine(debugInfo);
#endif
                goto TryAgain;
            }

            //ACT 2:
            //check raw data 
            if (string.IsNullOrEmpty(receivedData))
            {
                //log it
                await LibLogger.Debug($"BLZ > Call returned empty\n To:{apiUrl} with payload:\n{xmlData}");

                //send failed empty data to caller, it should know what to do with it
                return new WebResult<XElement>(false, new XElement("CallEmptyError"));
            }

            //ACT 3:
            //return data as XML
            //problems might occur when parsing
            //try to parse as XML
            var writeToServerXmlReply = XElement.Parse(receivedData);
            var returnVal = WebResult<XElement>.FromXml(writeToServerXmlReply);

            //ACT 4:
            return returnVal;
        }


        public static async Task<string> Post(string apiUrl, string data)
        {
            using (var httpClient = new HttpClient())
            {
                var content = new StringContent(data, Encoding.UTF8, "application/xml");
                var httpRequestMessage = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri(apiUrl),
                    Content = content
                };


                HttpResponseMessage response = await httpClient.SendAsync(httpRequestMessage);
                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    return result;
                }
                else
                {
                    throw new Exception($"Error in Post method: {response.StatusCode}");
                }
            }
        }



    }
}
