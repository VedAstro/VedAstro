using System.Xml.Linq;
using Genso.Astrology.Library.Compatibility;

namespace BlazorApp.Client
{

    /// <summary>
    /// Encapsulates all thing to do with server (API)
    /// </summary>
    public static class ServerManager
    {
        /// <summary>
        /// Gets CompatibilityReport from API server
        /// </summary>
        public static async Task<CompatibilityReport?> GetCompatibilityReport(string male, string female)
        {
            //prepare request to API server
            var url = $"https://vedastroapi.azurewebsites.net/api/compatibility?male={male}&female={female}";

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








    }
}
