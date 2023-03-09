using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Components.WebAssembly.Http;

namespace Genso.Astrology.Library
{

    /// <summary>
    /// Simple logger to log events, errors directly in side library
    /// WebLogger & ApiLogger can't access the level of detail this logger gets
    /// </summary>
    public static class LibLogger
    {

        private static readonly XElement SourceXml = new XElement("Source", "LibLogger");


        /// <summary>
        /// Makes a log of the exception in API server
        /// </summary>
        public static async Task Error(Exception exception, string extraInfo = "")
        {

            //if running code locally, end here
            //since in local errors will show in console
            //and also not to clog server's error log
            //todo check if below debug code is needed
            //#if DEBUG
            //            Console.WriteLine("BLZ: LogError: DEBUG mode, skipped logging to server");
            //            Console.WriteLine($"{exception.Message}\n{exception.StackTrace}");
            //            return;
            //#endif

            //convert exception into nice xml
            var errorXml = Tools.ExtractDataFromException(exception);

            //place error data into visitor tag
            //this is done because visitor data might hold clues to error
            var visitorXml = new XElement("Visitor");
            var dataXml = new XElement("Data", extraInfo);
            visitorXml.Add(Tools.BranchXml, SourceXml, errorXml, dataXml, Tools.TimeStampSystemXml, Tools.TimeStampServerXml);

            //send to server for storage
            await SendLogToServer(visitorXml);

            Console.WriteLine("LibLogger: An unexpected error occurred and was logged.");
        }

        public static async Task Error(XElement errorXml, string extraInfo = "")
        {
            //place error data into visitor tag
            //this is done because visitor data might hold clues to error
            var visitorXml = new XElement("Visitor");
            var dataXml = new XElement("Data", extraInfo);
            visitorXml.Add(Tools.BranchXml, SourceXml, errorXml, dataXml, Tools.TimeStampSystemXml, Tools.TimeStampServerXml);

            //send to server for storage
            await SendLogToServer(visitorXml);
        }

        public static async Task Error(string errorMessage)
        {
            //place error data into visitor tag
            //this is done because visitor data might hold clues to error
            var visitorXml = new XElement("Visitor");
            var dataXml = new XElement("Error", errorMessage);
            visitorXml.Add(Tools.BranchXml, SourceXml, dataXml, Tools.TimeStampSystemXml, Tools.TimeStampServerXml);

            //send to server for storage
            await SendLogToServer(visitorXml);

        }


        /// <summary>
        /// Makes a debug log entry
        /// </summary>
        public static async Task Debug(string message = "")
        {
            //place error data into visitor tag
            //this is done because visitor data might hold clues to error
            var visitorXml = new XElement("Visitor");
            var dataXml = new XElement("Data", message);
            visitorXml.Add(Tools.BranchXml, SourceXml, dataXml, Tools.TimeStampSystemXml, Tools.TimeStampServerXml);

            //send to server for storage
            await SendLogToServer(visitorXml);

            Console.WriteLine("LibLogger: An unexpected error occurred and was logged.");
        }

        /// <summary>
        /// Given the Visitor xml element, it will send it to API for safe keeping
        /// </summary>
        private static async Task SendLogToServer(XElement visitorElement)
        {
            try
            {
                //send to API for save keeping
                //note:js runtime passed as null, so no internet checking done
                var result = await WriteToServerXmlReply(URL.AddVisitorApi_Stable, visitorElement);

                //check result, display error if needed
                if (!result.IsPass) { Console.WriteLine($"BLZ: ERROR: Add Visitor Api\n{result.Payload.Value}"); }

            }
            catch (Exception e)
            {
                //not important if fail, keep quite
                Console.WriteLine("BLZ: SendLogToServer Silent Fail");
            }


        }

        public static async Task<WebResult<XElement>> WriteToServerXmlReply(string apiUrl, XElement xmlData)
        {
            WebResult<XElement> returnVal;


            string rawMessage = "";
            var statusCode = "";

            try
            {
                //prepare the data to be sent
                var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, apiUrl);
                httpRequestMessage.SetBrowserRequestMode(BrowserRequestMode.NoCors); //NO CORS!

                httpRequestMessage.Content = Tools.XmLtoHttpContent(xmlData);

                //get the data sender
                using var client = new HttpClient();

                //tell sender to wait for complete reply before exiting
                var waitForContent = HttpCompletionOption.ResponseContentRead;

                //send the data on its way (wait forever no timeout)
                client.Timeout = new TimeSpan(0, 0, 0, 0, Timeout.Infinite);
                var response = await client.SendAsync(httpRequestMessage, waitForContent);
                statusCode = response?.StatusCode.ToString();

                //extract the content of the reply data
                var readAsStringAsync = await response?.Content.ReadAsStringAsync();
                rawMessage = readAsStringAsync ?? "";

                //problems might occur when parsing
                //try to parse as XML
                var writeToServerXmlReply = XElement.Parse(rawMessage);
                returnVal = WebResult<XElement>.FromXml(writeToServerXmlReply);

            }

            //note: failure here could be for several very likely reasons,
            //so it is important to properly check and handled here for best UX
            //- server unexpected failure
            //- server unreachable
            catch (Exception e)
            {
                //todo clean up

                returnVal = new WebResult<XElement>(false, new XElement("Root", $"Error from WriteToServerXmlReply()\n{statusCode}\n{rawMessage}"));

                //throw new ApiCommunicationFailed($"Error from WriteToServerXmlReply()\n{statusCode}\n{rawMessage}", e);
            }

            //if fail log it
            //if (!returnVal.IsPass) { WebLogger.Error(returnVal.Payload); }

            return returnVal;
        }

    }
}
