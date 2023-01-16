using System.Diagnostics;
using System.Xml.Linq;
using Genso.Astrology.Library;
using Microsoft.JSInterop;

namespace Website
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
                var visitorXml = await GetVisitorDataXml(AppData.JsRuntime);

                //send to server for storage
                await SendLogToServer(visitorXml, AppData.JsRuntime);

            }
            catch (Exception e)
            {
                //if fail exit silently, not priority
                Console.WriteLine($"BLZ: LogVisitor: Failed! \n{e.Message}\n{e.StackTrace}");
            }

        }


        public static async Task LogError(XElement errorDataXml)
        {
           await LogError(errorDataXml.ToString());
        }

        /// <summary>
        /// Log error when there is no exception data
        /// used when #blazor-error-ui is shown
        /// </summary>
        public static async Task LogError(string errorMsg)
        {

            //if running code locally, end here
            //since in local errors will show in console
            //and also not to clog server's error log
#if DEBUG
            Console.WriteLine("BLZ: LogError: DEBUG mode, skipped logging to server");
            Console.WriteLine($"PAGE NAME:{await AppData.CurrentUrlJS}\nERROR MESSAGE:{errorMsg}");
            return;
#endif
            //place error data into visitor tag
            //this is done because visitor data might hold clues to error
            var visitorXml = new XElement("Visitor");
            var userId = new XElement("UserId", AppData.CurrentUser.Id);
            var visitorId = new XElement("VisitorId", AppData.VisitorId);
            var urlXml = new XElement("Url", await AppData.CurrentUrlJS);
            var errorXml = new XElement("Error", new XElement("Message", errorMsg));
            visitorXml.Add(userId, visitorId, errorXml, urlXml, WebsiteTools.TimeStampSystemXml);

            //send to server for storage
            SendLogToServer(visitorXml, AppData.JsRuntime);

            Console.WriteLine("BLZ: LogError: An unexpected error occurred and was logged.");

        }

        /// <summary>
        /// Makes a log of the exception in API server
        /// </summary>
        public static async Task LogError(Exception exception, string extraInfo = "")
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
            var userId = new XElement("UserId", AppData.CurrentUser?.Id);
            var visitorId = new XElement("VisitorId", AppData.VisitorId);
            var dataXml = new XElement("Data", extraInfo);
            var urlXml = new XElement("Url", await AppData.CurrentUrlJS);
            visitorXml.Add(userId, visitorId, errorXml, urlXml, dataXml, WebsiteTools.TimeStampSystemXml);

            //send to server for storage
            await SendLogToServer(visitorXml, AppData.JsRuntime);

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
            Console.WriteLine("BLZ: LogClick: DEBUG mode, skipped logging to server");
            return;
#endif

            await LogData(AppData.JsRuntime, $"Button Text:{buttonText}");

        }

        /// <summary>
        /// Logs an alert shown to user
        /// </summary>
        public static async Task LogAlert(IJSRuntime jsRuntime, dynamic alertData)
        {
            //if running code locally, end here
            //since in local errors will show in console
            //and also not to clog server's error log
#if DEBUG
            Console.WriteLine("BLZ: LogAlert: DEBUG mode, skipped logging to server");
            return;
#endif
            //all alerts except loading box, visitor list popup (use of html instead of title)
            try
            {
                //todo loading box is not logged, because of over logging (possible fix, wrapper class to handle )
                var alertMessage = ((dynamic)alertData)?.title ?? "";
                await LogData(AppData.JsRuntime, $"Alert Message:{alertMessage}");
            }
            catch (Exception)
            {
                // only visitor list page & loading box uses "html" and not "title",
                // so if can't get it skip it
            }


        }

        /// <summary>
        /// Simple method to log general data to API
        /// note: will not run debug
        /// </summary>
        public static async Task LogData(IJSRuntime jsRuntime, string data)
        {
            //if running code locally, end here
            //since in local errors will show in console
            //and also not to clog server's error log
#if DEBUG
            Console.WriteLine($"BLZ:LogData:DEBUG mode:skipped logging:{data}");
            return;
#endif

            //get basic visitor data
            var visitorXml = await GetVisitorDataXml(AppData.JsRuntime);

            //add in button click data
            visitorXml.Add(new XElement("Data", data));

            //send to server for storage
            await SendLogToServer(visitorXml, AppData.JsRuntime);
        }




        //█▀█ █▀█ █ █░█ ▄▀█ ▀█▀ █▀▀   █▀▄▀█ █▀▀ ▀█▀ █░█ █▀█ █▀▄ █▀
        //█▀▀ █▀▄ █ ▀▄▀ █▀█ ░█░ ██▄   █░▀░█ ██▄ ░█░ █▀█ █▄█ █▄▀ ▄█

        private static async Task<XElement> GetVisitorDataXml(IJSRuntime jsRuntime)
        {
            //get url user is on
            var urlString = await AppData.JsRuntime.GetCurrentUrl();
            //place url in xml
            var urlXml = new XElement("Url", urlString);
            var userIdXml = new XElement("UserId", AppData.CurrentUser?.Id);


            //based on visitor type create the right record data to log
            //this is done to minimize excessive logging
            var visitorXml = AppData.IsNewVisitor
                ? await NewVisitor(userIdXml, urlXml, AppData.JsRuntime)
                : OldVisitor(userIdXml, urlXml);


            return visitorXml;

        }

        //all possible details are logged
        private static async Task<XElement> NewVisitor(XElement userIdXml, XElement urlXml, IJSRuntime jsRuntime)
        {
            //get visitor data & format it nicely for storage
            var browserDataXml = await AppData.JsRuntime.InvokeAsyncJson("getVisitorData", "BrowserData");
            var screenDataXml = await AppData.JsRuntime.InvokeAsyncJson("getScreenData", "ScreenData");
            var originUrlXml = new XElement("OriginUrl", await AppData.OriginUrl);
            var visitorIdXml = new XElement("VisitorId", AppData.VisitorId);
            var resultLocation = await ServerManager.ReadFromServerXmlReply(ServerManager.GetGeoLocation, null, "Location");
            var locationXml = resultLocation.Payload;
            var visitorElement = new XElement("Visitor");
            visitorElement.Add(userIdXml, visitorIdXml, urlXml, WebsiteTools.TimeStampSystemXml, WebsiteTools.TimeStampServerXml, locationXml, browserDataXml, screenDataXml, originUrlXml);

            //mark new visitor as already logged for first time
            AppData.IsNewVisitor = false;

            return visitorElement;
        }

        //only needed details are logged
        private static XElement OldVisitor(XElement userIdXml, XElement urlXml)
        {

            //get visitor data & format it nicely for storage
            var visitorElement = new XElement("Visitor");
            //todo origin url does not change, for now not needed for old visitor updates
            //var originUrlXml = new XElement("OriginUrl", AppData.OriginUrl);
            var visitorIdXml = new XElement("VisitorId", AppData.VisitorId); //use id generated above
            visitorElement.Add(userIdXml, visitorIdXml, urlXml, WebsiteTools.TimeStampSystemXml, WebsiteTools.TimeStampServerXml);

            return visitorElement;
        }

        /// <summary>
        /// Given the Visitor xml element, it will send it to API for safe keeping
        /// </summary>
        private static async Task SendLogToServer(XElement visitorElement, IJSRuntime jsRuntime)
        {
            try
            {
                //send using worker JS
                await AppData.JsRuntime.InvokeAsync<string>("window.LogThread.postMessage", visitorElement.ToString());

                //send to API for save keeping
                //note:js runtime passed as null, so no internet checking done
                //var result = await ServerManager.WriteToServerXmlReply(ServerManager.AddVisitorApi, visitorElement, null);

                //check result, display error if needed
                //if (!result.IsPass) { Console.WriteLine($"BLZ: ERROR: Add Visitor Api\n{result.Payload.Value}"); }

            }
            catch (Exception e)
            {
                //not important if fail, keep quite
                Console.WriteLine("BLZ: SendLogToServer Silent Fail");
            }


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
                new XElement("MethodName", methodName)
            );


            return newRecord;
        }

    }
}
