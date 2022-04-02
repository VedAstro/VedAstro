using Genso.Astrology.Library;
using System.Text;
using System.Xml.Linq;

namespace Website
{

    /// <summary>
    /// Encapsulates all thing to do with server (API)
    /// </summary>
    public static class ServerManager
    {

        public const string AddPersonAPI = "https://vedastroapi.azurewebsites.net/api/addperson";
        public const string GetMaleListAPI = "https://vedastroapi.azurewebsites.net/api/getmalelist";
        public const string GetPersonListAPI = "https://vedastroapi.azurewebsites.net/api/getpersonlist";
        public const string GetFemaleListAPI = "https://vedastroapi.azurewebsites.net/api/getfemalelist";
        public const string GetMatchReportAPI = "https://vedastroapi.azurewebsites.net/api/getmatchreport";
        public const string GetEventsAPI = "https://vedastroapi.azurewebsites.net/api/getevents";
        public const string GoogleGeoLocationAPIKey = "AIzaSyDVrV2b91dJpdeWMmMAwU92j2ZEyO8uOqg";


        //PUBLIC METHODS

        /// <summary>
        /// Calls a URL and returns the content of the result as XML
        /// NOTE: Content is assumed to be XML here
        /// </summary>
        public static async Task<XElement> ReadFromServer(string apiUrl)
        {
            //send request to API server
            var result = await RequestServer(apiUrl);

            //parse data reply
            var rawMessage = result.Content.ReadAsStringAsync().Result;
            var parsedElement = XElement.Parse(rawMessage);

            return parsedElement;


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



            //return the raw reply to caller
            return XElement.Parse(rawMessage);

        }



        /// <summary>
        /// Gets Muhurtha events from API
        /// TODO NEED TO MOVE to component that uses it
        /// </summary>
        public static async Task<List<Event>> GetEvents(Time startTime, Time endTime, GeoLocation location, Person person, EventTag tag, double precision)
        {
            //prepare data to send to API
            var root = new XElement("Root");

            root.Add(
                new XElement("StartTime", startTime.ToXml()),
                new XElement("EndTime", endTime.ToXml()),
                location.ToXml(),
                person.ToXml(),
                Genso.Astrology.Library.Tools.AnyTypeToXml(tag),
                Genso.Astrology.Library.Tools.AnyTypeToXml(precision));

            //send to api and get results
            var resultsRaw = await ServerManager.WriteToServer(ServerManager.GetEventsAPI, root);

            //parse raw results
            List<Event> resultsParsed = Event.FromXml(resultsRaw);

            //send to caller
            return resultsParsed;

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
