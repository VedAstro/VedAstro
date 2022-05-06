using Genso.Astrology.Library;
using System.Text;
using System.Text.Json.Nodes;
using System.Xml.Linq;
using Newtonsoft.Json;

namespace Website
{

    /// <summary>
    /// Encapsulates all thing to do with server (API)
    /// </summary>
    public static class ServerManager
    {

        public const string AddPersonApi = "https://vedastroapi.azurewebsites.net/api/addperson";
        public const string AddMessageApi = "https://vedastroapi.azurewebsites.net/api/addmessage";
        public const string DeletePersonApi = "https://vedastroapi.azurewebsites.net/api/deleteperson";
        public const string AddTaskApi = "https://vedastroapi.azurewebsites.net/api/addtask";
        public const string AddVisitorApi = "https://vedastroapi.azurewebsites.net/api/addvisitor";
        public const string GetMaleListApi = "https://vedastroapi.azurewebsites.net/api/getmalelist";
        public const string GetPersonListApi = "https://vedastroapi.azurewebsites.net/api/getpersonlist";
        public const string GetPersonApi = "https://vedastroapi.azurewebsites.net/api/getperson";
        public const string UpdatePersonApi = "https://vedastroapi.azurewebsites.net/api/updateperson";
        public const string GetTaskListApi = "https://vedastroapi.azurewebsites.net/api/gettasklist";
        public const string GetFemaleListApi = "https://vedastroapi.azurewebsites.net/api/getfemalelist";
        public const string GetMatchReportApi = "https://vedastroapi.azurewebsites.net/api/getmatchreport";
        public const string GetEventsApi = "https://vedastroapi.azurewebsites.net/api/getevents";
        public const string GetGeoLocation = "https://get.geojs.io/v1/ip/geo.json";
        public const string GoogleGeoLocationApiKey = "AIzaSyDVrV2b91dJpdeWMmMAwU92j2ZEyO8uOqg"; //marked for deletetion
        public const string GoogleGeoLocationApiKey2 = "AIzaSyDqBWCqzU1BJenneravNabDUGIHotMBsgE";
        /// <summary>
        /// link to js file used for google sign in function
        /// </summary>
        public const string GoogleSignInJs = "https://apis.google.com/js/platform.js"; 


        //PUBLIC METHODS

        /// <summary>
        /// Calls a URL and returns the content of the result as XML
        /// Even if content is returned as JSON, it is converted to XML
        /// Note: if JSON auto adds "Root" as first element, unless specified
        /// for XML data root element name is ignored
        /// </summary>
        public static async Task<XElement> ReadFromServer(string apiUrl, string rootElementName = "Root")
        {
            //send request to API server
            var result = await RequestServer(apiUrl);

            //parse data reply
            var rawMessage = result.Content.ReadAsStringAsync().Result;

            //raw message can be JSON or XML
            //try parse as XML if fail then as JSON
            try { return XElement.Parse(rawMessage);}
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
        /// Send xml as string to server and returns xml as response
        /// Note: xml is not checked here, just converted
        /// </summary>
        public static async Task<XElement> WriteToServer(string apiUrl, XElement xmlData)
        {
            //prepare the data to be sent
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, apiUrl);

            httpRequestMessage.Content = XmLtoStringContent(xmlData);

            //get the data sender
            using var client = new HttpClient();

            //tell sender to wait for complete reply before exiting
            var waitForContent = HttpCompletionOption.ResponseContentRead;

            //send the data on its way
            var response = await client.SendAsync(httpRequestMessage, waitForContent);

            //extract the content of the reply data
            var rawMessage = response.Content.ReadAsStringAsync().Result;

            //problems might occur when parsing
            //try to parse as XML
            try { return XElement.Parse(rawMessage); }

            //else raise alarm and show raw message in debugger
            catch (Exception) { Console.WriteLine($"Raw Unparseable Data:\n{rawMessage}"); throw; }

        }




        //PRIVATE METHODS
        /// <summary>
        /// Packages the data into ready form for the HTTP client to use in final sending stage
        /// </summary>
        private static StringContent XmLtoStringContent(XElement data)
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
