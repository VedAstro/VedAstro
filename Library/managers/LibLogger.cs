using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace VedAstro.Library
{

    /// <summary>
    /// Simple logger to log events, errors directly in side library
    /// WebLogger & ApiLogger can't access the level of detail this logger gets
    /// </summary>
    public static class LibLogger
    {
        /// <summary>
        /// Has to be loaded when app loads, obviously since that is when branch manifest it read
        /// since this is only used by loggers
        /// </summary>
        private static XElement BranchXml = new XElement("Branch", "todo set lib logger branch");

        private static readonly XElement SourceXml = new XElement("Source", "LibLogger");

        
        /// <summary>
        /// Makes a log of the exception in API server
        /// </summary>
        public static async Task Error(Exception exception, string extraInfo = "")
        {

            //if running code locally, end here
            //since in local errors will show in console
            //and also not to clog server's error log
#if DEBUG
            Console.WriteLine("BLZ: LogError: DEBUG mode, skipped logging to server");
            Console.WriteLine($"{exception.Message}\n{exception.StackTrace}");
            return;
#endif

            //convert exception into nice xml
            var errorXml = Tools.ExtractDataFromException(exception);

            //place error data into visitor tag
            //this is done because visitor data might hold clues to error
            var visitorXml = new XElement("Visitor");
            var dataXml = new XElement("Data", extraInfo);
            visitorXml.Add(BranchXml, SourceXml, errorXml, dataXml, Tools.TimeStampSystemXml, Tools.TimeStampServerXml);

            //send to server for storage
            await SendLogToServer(visitorXml);

            Console.WriteLine("LibLogger: An unexpected error occurred and was logged.");
        }

        public static async Task Error(XElement errorXml, string extraInfo = "")
        {
            //place error data into visitor tag
            //this is done because visitor data might hold "Blue's Clues"
            //to why it all went down hill, hence easier to fix
            //so take the time & data to log
            var visitorXml = new XElement("Visitor");
            var dataXml = new XElement("Data", extraInfo);
            visitorXml.Add(BranchXml, SourceXml, errorXml, dataXml, Tools.TimeStampSystemXml, Tools.TimeStampServerXml);

            //send to server for storage
            await SendLogToServer(visitorXml);
        }

        public static async Task Error(string errorMessage)
        {
            //place error data into visitor tag
            //this is done because visitor data might hold clues to error
            var visitorXml = new XElement("Visitor");
            var dataXml = new XElement("Error", errorMessage);
            visitorXml.Add(BranchXml, SourceXml, dataXml, Tools.TimeStampSystemXml, Tools.TimeStampServerXml);

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
            visitorXml.Add(BranchXml, SourceXml, dataXml, Tools.TimeStampSystemXml, Tools.TimeStampServerXml);

            //send to server for storage
            await SendLogToServer(visitorXml);

            Console.WriteLine($"LibLogger > Debug > Unexpected Computation > {message}");
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
                var result = await WriteToServerXmlReply(URL.AddVisitorApiStable, visitorElement);

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
                rawMessage = response?.Content.ReadAsStringAsync().Result ?? "";

                //problems might occur when parsing
                //try to parse as XML
                var writeToServerXmlReply = XElement.Parse(rawMessage);
                returnVal = WebResult<XElement>.FromXml(writeToServerXmlReply);

            }

            //note: failure here could be for several very likely reasons,
            //so it is important to properly check and handled here for best UX
            //- server unexpected failure
            //- server unreachable
            //catch the exception, and return a nice & clean fail result 
            catch (Exception) { returnVal = new WebResult<XElement>(false, new XElement("Root", $"Error from WriteToServerXmlReply()\n{statusCode}\n{rawMessage}")); }

            //if fail log it properly, hence await (should not happen all the time)
            if (!returnVal.IsPass) { await LibLogger.Error(returnVal.Payload); }

            return returnVal;
        }

    }
}
