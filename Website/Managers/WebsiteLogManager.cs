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
                Console.WriteLine("BLZ: LogError: DEBUG mode, skipped logging to server");
                return;
#endif

                //if URL is localhost ignore & end here
                //todo below code might not be needed if above works fine
                var urlString = await jsRuntime.InvokeAsync<string>("getUrl");
                if (urlString.Contains("localhost")) { return; }

                //get all visitor data
                var visitorXml = await GetVisitorDataXml(jsRuntime);

                //send to server for storage
                await SendLogToServer(visitorXml);

            }
            catch (Exception)
            {
                //if fail exit silently, not priority
                Console.WriteLine("BLZ: LogVisitor: Failed!");
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
            return;
#endif

            //if URL is localhost ignore & end here
            //todo below code might not be needed if above works fine
            var urlString = await jsRuntime.InvokeAsync<string>("getUrl");
            if (urlString.Contains("localhost")) { return; }

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
            return;
#endif

            //if URL is localhost ignore & end here
            //var urlString = await jsRuntime.InvokeAsync<string>("getUrl");
            //if (urlString.Contains("localhost")) { return; }

            //get all visitor data
            //var visitorXml = await GetVisitorDataXml(jsRuntime);

            //convert exception into nice xml
            var errorXml = ExtractDataFromException(exception);

            //place error data into visitor tag
            //this is done because visitor data might hold clues to error
            var visitorXml = new XElement("Visitor");
            visitorXml.Add(errorXml);

            //send to server for storage
            await SendLogToServer(visitorXml);

            Console.WriteLine("BLZ: LogError: An unexpected error occurred and was logged.");
        }



        //█▀█ █▀█ █ █░█ ▄▀█ ▀█▀ █▀▀   █▀▄▀█ █▀▀ ▀█▀ █░█ █▀█ █▀▄ █▀
        //█▀▀ █▀▄ █ ▀▄▀ █▀█ ░█░ ██▄   █░▀░█ ██▄ ░█░ █▀█ █▄█ █▄▀ ▄█


        private static async Task<XElement> GetVisitorDataXml(IJSRuntime jsRuntime)
        {
            //get url user is on
            var urlString = await jsRuntime.InvokeAsync<string>("getUrl");
            //place url in xml
            var urlXml = new XElement("Url", urlString);
            var userIdXml = new XElement("UserId", await WebsiteTools.GetUserIdAsync(jsRuntime));

            //find out if new visitor just arriving or old one browsing
            var uniqueId = await GetVisitorIdFromCookie();
            var isNewVisitor = uniqueId is null or "";

            //based on visitor type create the right record data to log
            //this is done to minimize excessive logging
            var visitorXml = isNewVisitor
                ? await NewVisitor(userIdXml, urlXml, jsRuntime)
                : await OldVisitor(userIdXml, urlXml, uniqueId, jsRuntime);


            return visitorXml;


            //----------------LOCAL FUNCTIONS

            //returns null if no id found
            async Task<string> GetVisitorIdFromCookie() => await jsRuntime.InvokeAsync<string>("getCookiesWrapper", "uniqueId");

        }

        //all possible details are logged
        private static async Task<XElement> NewVisitor(XElement userIdXml, XElement urlXml, IJSRuntime jsRuntime)
        {

            //get visitor data & format it nicely for storage
            var browserDataXml = await jsRuntime.InvokeAsyncJson("getVisitorData", "BrowserData");
            var timeStampXml = new XElement("TimeStamp", Tools.GetNowSystemTimeText());
            var visitorId = Tools.GenerateId();
            var uniqueIdXml = new XElement("UniqueId", visitorId);
            var locationXml = await ServerManager.ReadFromServer(ServerManager.GetGeoLocation, "Location");
            var visitorElement = new XElement("Visitor");
            visitorElement.Add(userIdXml, uniqueIdXml, urlXml, timeStampXml, locationXml, browserDataXml);


            //mark visitor with id inside cookie
            //so can id user on return
            await SetNewVisitorIdInCookie(visitorId);


            return visitorElement;

            //------------LOCAL FUNCTIONS--------------

            async Task SetNewVisitorIdInCookie(string id) => await jsRuntime.InvokeVoidAsync("setCookiesWrapper", "uniqueId", id);

        }

        //only needed details are logged
        private static async Task<XElement> OldVisitor(XElement userIdXml, XElement urlXml, string uniqueId, IJSRuntime jsRuntime)
        {

            //get visitor data & format it nicely for storage
            var visitorElement = new XElement("Visitor");
            var timeStampXml = new XElement("TimeStamp", Tools.GetNowSystemTimeText());
            var uniqueIdXml = new XElement("UniqueId", uniqueId); //use id generated above
            visitorElement.Add(userIdXml, uniqueIdXml, urlXml, timeStampXml);

            return visitorElement;
        }

        /// <summary>
        /// Given the Visitor xml element, it will send it to API for safe keeping
        /// </summary>
        private static async Task SendLogToServer(XElement visitorElement)
        {
            //send to API for save keeping
            var result = await ServerManager.WriteToServer(ServerManager.AddVisitorApi, visitorElement);

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
                new XElement("Time", GetNow())
            );


            return newRecord;
        }


        /// <summary>
        /// Gets now time in UTC +8:00
        /// </summary>
        /// <returns></returns>
        private static DateTimeOffset GetNow()
        {
            //create utc 8
            var utc8 = new TimeSpan(8, 0, 0);
            //get now time in utc 0
            var nowTime = DateTimeOffset.Now.ToUniversalTime();
            //convert time utc 0 to utc 8
            var utc8Time = nowTime.ToOffset(utc8);

            //return converted time to caller
            return utc8Time;
        }


    }
}
