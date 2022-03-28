using System.Text;
using System.Xml.Linq;
using Genso.Astrology.Library;
using Genso.Astrology.Library.Compatibility;

namespace Website
{

    /// <summary>
    /// Encapsulates all thing to do with server (API)
    /// </summary>
    public static class ServerManager
    {

        public static string AddPersonAPI = "https://vedastroapi.azurewebsites.net/api/addperson";

        /// <summary>
        /// Gets CompatibilityReport from API server
        /// </summary>
        public static async Task<CompatibilityReport?> GetCompatibilityReport(string male, string female)
        {
            //prepare request to API server
            var url = $"https://vedastroapi.azurewebsites.net/api/match?male={male}&female={female}";

            //send request to API server
            var result = await RequestServer(url);

            //parse data reply
            var rawMessage = result.Content.ReadAsStringAsync().Result;
            var parsedElement = XElement.Parse(rawMessage);
            var report = CompatibilityReport.FromXml(parsedElement);

            //return parsed data to caller
            return report;
        }

        private static async Task<HttpResponseMessage> RequestServer(string receiverAddress)
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

        /// <summary>
        /// Send xml as string to server and returns xml as response
        /// </summary>
        public static async Task<XElement> SendXmlToServer(string apiAddress, XElement xmlData)
        {
            //prepare the data to be sent
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, apiAddress);

            httpRequestMessage.Content = XMLtoStringContent(xmlData);

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
        /// Packages the data into ready form for the HTTP client to use in final sending stage
        /// </summary>
        public static StringContent XMLtoStringContent(XElement _data)
        {
            //gets the main XML data as a string
            var data = xmlToString(_data);

            //specify the data encoding
            var encoding = Encoding.UTF8;

            //specify the type of the data sent
            //plain text, stops auto formatting
            var mediaType = "plain/text";

            //return packaged data to caller
            return new StringContent(data, encoding, mediaType);
        }

        /// <summary>
        /// Converts xml element instance to string properly
        /// TODO needs to be a separate class utils
        /// </summary>
        public static string xmlToString(XElement xml)
        {
            //remove all formatting, for clean xml as string
            return xml.ToString(SaveOptions.DisableFormatting);
        }


    }
}
