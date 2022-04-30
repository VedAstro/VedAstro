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

        public const string AddPersonAPI = "https://vedastroapi.azurewebsites.net/api/addperson";
        public const string DeletePersonAPI = "https://vedastroapi.azurewebsites.net/api/deleteperson";
        public const string AddTaskAPI = "https://vedastroapi.azurewebsites.net/api/addtask";
        public const string AddVisitorAPI = "https://vedastroapi.azurewebsites.net/api/addvisitor";
        public const string GetMaleListAPI = "https://vedastroapi.azurewebsites.net/api/getmalelist";
        public const string GetPersonListAPI = "https://vedastroapi.azurewebsites.net/api/getpersonlist";
        public const string GetPersonAPI = "https://vedastroapi.azurewebsites.net/api/getperson";
        public const string UpdatePersonAPI = "https://vedastroapi.azurewebsites.net/api/updateperson";
        public const string GetTaskListAPI = "https://vedastroapi.azurewebsites.net/api/gettasklist";
        public const string GetFemaleListAPI = "https://vedastroapi.azurewebsites.net/api/getfemalelist";
        public const string GetMatchReportAPI = "https://vedastroapi.azurewebsites.net/api/getmatchreport";
        public const string GetEventsAPI = "https://vedastroapi.azurewebsites.net/api/getevents";
        public const string GetGeoLocation = "https://get.geojs.io/v1/ip/geo.json";
        public const string GoogleGeoLocationAPIKey = "AIzaSyDVrV2b91dJpdeWMmMAwU92j2ZEyO8uOqg";


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
                //data must be json string
                var rawXml = JsonConvert.DeserializeXmlNode(rawMessage, rootElementName);
                return XElement.Parse(rawXml.InnerXml);
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
        private static StringContent XmLtoStringContent(XElement _data)
        {
            //gets the main XML data as a string
            var data = Genso.Astrology.Library.Tools.XmlToString(_data);

            //specify the data encoding
            var encoding = Encoding.UTF8;

            //specify the type of the data sent
            //plain text, stops auto formatting
            var mediaType = "plain/text";

            //return packaged data to caller
            return new StringContent(data, encoding, mediaType);
        }
    }
}
