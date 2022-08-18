using System.Diagnostics;
using System.Xml.Linq;
using Genso.Astrology.Library;
using Microsoft.JSInterop;

namespace Website.Managers
{

    /// <summary>
    /// A specialized log manager for website
    /// </summary>
    public static class WebsiteLogManager
    {

        /// <summary>
        /// Tries to ID the user, and sends a log of the visit to API server
        /// Called from MainLayout everytime page is loaded
        /// Note:
        /// - Does not log any url with localhost
        /// - if fail will exit silently
        /// </summary>
        public static async Task LogVisitor(IJSRuntime jsRuntime)
        {
            try
            {

                //if running code locally, end here
                //since in local errors will show in console
                //and also not to clog server's error log
#if DEBUG
                Console.WriteLine("BLZ: LogVisitor: DEBUG mode, skipped logging to server");
                return;
#endif

                //get all visitor data
                var visitorXml = await GetVisitorDataXml(jsRuntime);

                //send to server for storage
                await SendLogToServer(visitorXml);

            }
            catch (Exception e)
            {
                //if fail exit silently, not priority
                Console.WriteLine($"BLZ: LogVisitor: Failed! \n{e.Message}\n{e.StackTrace}");
            }

        }


        /// <summary>
        /// Makes a log of the exception in API server
        /// </summary>
        public static async Task LogError(IJSRuntime jsRuntime, Exception exception)
        {
            //if running code locally, end here
            //since in local errors will show in console
            //and also not to clog server's error log
#if DEBUG
            Console.WriteLine("BLZ: LogError: DEBUG mode, skipped logging to server");
            Console.WriteLine($"{exception.Message}\n{exception.StackTrace}");
            return;
#endif

            //get all visitor data
            var visitorXml = await GetVisitorDataXml(jsRuntime);

            //convert exception into nice xml
            var errorXml = ExtractDataFromException(exception);

            //place error data into visitor tag
            //this is done because visitor data might hold clues to error
            visitorXml.Add(errorXml);

            //send to server for storage
            await SendLogToServer(visitorXml);

        }

        /// <summary>
        /// Makes a log of the exception in API server
        /// This version doesn't use user data, only exception data is sent to server
        /// Because no access to JS runtime from here, todo user data can be moved to blazor side
        /// then full logging can be done
        /// </summary>
        public static async Task LogError(Exception exception)
        {

            //if running code locally, end here
            //since in local errors will show in console
            //and also not to clog server's error log
#if DEBUG
            Console.WriteLine("BLZ: LogError: DEBUG mode, skipped logging to server");
            Console.WriteLine($"{exception.Message}\n{exception.StackTrace}");
            return;
#endif

            //get all visitor data
            //var visitorXml = await GetVisitorDataXml(jsRuntime);

            //convert exception into nice xml
            var errorXml = ExtractDataFromException(exception);

            //place error data into visitor tag
            //this is done because visitor data might hold clues to error
            var visitorXml = new XElement("Visitor");
            var userId = new XElement("UserId", AppData.CurrentUser);
            var visitorId = new XElement("VisitorId", AppData.VisitorId);
            visitorXml.Add(userId, visitorId, errorXml);

            //send to server for storage
            await SendLogToServer(visitorXml);

            Console.WriteLine("BLZ: LogError: An unexpected error occurred and was logged.");
        }

        /// <summary>
        /// Logs a button click
        /// </summary>
        public static async Task LogClick(IJSRuntime jsRuntime, string? buttonText)
        {
            //if running code locally, end here
            //since in local errors will show in console
            //and also not to clog server's error log
#if DEBUG
            Console.WriteLine("BLZ: LogVisitor: DEBUG mode, skipped logging to server");
            return;
#endif

            //get basic visitor data
            var visitorXml = await GetVisitorDataXml(jsRuntime);

            //add in button click data
            visitorXml.Add(new XElement("ButtonText", buttonText));

            //send to server for storage
            await SendLogToServer(visitorXml);

        }



        //█▀█ █▀█ █ █░█ ▄▀█ ▀█▀ █▀▀   █▀▄▀█ █▀▀ ▀█▀ █░█ █▀█ █▀▄ █▀
        //█▀▀ █▀▄ █ ▀▄▀ █▀█ ░█░ ██▄   █░▀░█ ██▄ ░█░ █▀█ █▄█ █▄▀ ▄█

        private static async Task<XElement> GetVisitorDataXml(IJSRuntime jsRuntime)
        {
            //get url user is on
            var urlString = await jsRuntime.InvokeAsync<string>("getUrl");
            //place url in xml
            var urlXml = new XElement("Url", urlString);
            var userIdXml = new XElement("UserId", AppData.CurrentUser?.Id);

            //find out if new visitor just arriving or old one browsing
            var visitorId = await jsRuntime.GetProperty("VisitorId"); //local storage
            var isNewVisitor = visitorId is null or "";

            //based on visitor type create the right record data to log
            //this is done to minimize excessive logging
            var visitorXml = isNewVisitor
                ? await NewVisitor(userIdXml, urlXml, jsRuntime)
                : await OldVisitor(userIdXml, urlXml, visitorId);


            return visitorXml;

        }

        //all possible details are logged
        private static async Task<XElement> NewVisitor(XElement userIdXml, XElement urlXml, IJSRuntime jsRuntime)
        {
            //since new visitor generate new id & store it
            var visitorId = Tools.GenerateId();
            AppData.VisitorId = visitorId; //app memory for this session use
            await jsRuntime.SetProperty("VisitorId", visitorId); //local storage, so can id user on return

            //get visitor data & format it nicely for storage
            var browserDataXml = await jsRuntime.InvokeAsyncJson("getVisitorData", "BrowserData");
            var screenDataXml = await jsRuntime.InvokeAsyncJson("getScreenData", "ScreenData");
            var originUrlXml = new XElement("OriginUrl", await jsRuntime.InvokeAsync<string>("getOriginUrl"));
            var timeStampXml = new XElement("TimeStamp", Tools.GetNowSystemTimeSecondsText());
            var visitorIdXml = new XElement("VisitorId", visitorId);
            var locationXml = await ServerManager.ReadFromServerXmlReply(ServerManager.GetGeoLocation, "Location");
            var visitorElement = new XElement("Visitor");
            visitorElement.Add(userIdXml, visitorIdXml, urlXml, timeStampXml, locationXml, browserDataXml, screenDataXml, originUrlXml);


            return visitorElement;
        }

        //only needed details are logged
        private static async Task<XElement> OldVisitor(XElement userIdXml, XElement urlXml, string visitorId)
        {

            //get visitor data & format it nicely for storage
            var visitorElement = new XElement("Visitor");
            var timeStampXml = new XElement("TimeStamp", Tools.GetNowSystemTimeSecondsText());
            var visitorIdXml = new XElement("VisitorId", visitorId); //use id generated above
            visitorElement.Add(userIdXml, visitorIdXml, urlXml, timeStampXml);

            return visitorElement;
        }

        /// <summary>
        /// Given the Visitor xml element, it will send it to API for safe keeping
        /// </summary>
        private static async Task SendLogToServer(XElement visitorElement)
        {
            //send to API for save keeping
            var result = await ServerManager.WriteToServerXmlReply(ServerManager.AddVisitorApi, visitorElement);

            //check result, display error if needed
            if (result.Value != "Pass") { Console.WriteLine($"BLZ: ERROR: Add Visitor Api\n{result.Value}"); }

        }

        /// <summary>
        /// Extracts data from an Exception puts it in a nice XML
        /// </summary>
        private static XElement ExtractDataFromException(Exception e)
        {
            //place to store the exception data
            string fileName;
            string methodName;
            int line;
            int columnNumber;
            string message;
            string source;

            //get the exception that started it all
            var originalException = e.GetBaseException();

            //extract the data from the error
            StackTrace st = new StackTrace(e, true);

            //Get the first stack frame
            StackFrame frame = st.GetFrame(st.FrameCount - 1);

            //Get the file name
            fileName = frame.GetFileName();

            //Get the method name
            methodName = frame.GetMethod().Name;

            //Get the line number from the stack frame
            line = frame.GetFileLineNumber();

            //Get the column number
            columnNumber = frame.GetFileColumnNumber();

            message = originalException.ToString();

            source = originalException.Source;


            //put together the new error record
            var newRecord = new XElement("Error",
                new XElement("Message", message),
                new XElement("Source", source),
                new XElement("FileName", fileName),
                new XElement("SourceLineNumber", line),
                new XElement("SourceColNumber", columnNumber),
                new XElement("MethodName", methodName),
                new XElement("Time", Tools.GetNowSystemTimeSecondsText())
            );


            return newRecord;
        }


        


    }
}
