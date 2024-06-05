using Microsoft.Azure.Functions.Worker.Http;
using System.Xml.Linq;
using VedAstro.Library;

namespace API
{
    /// <summary>
    /// A collection of general tools used by API
    /// </summary>
    public static partial class APITools
    {

        public static async Task<XElement> ExtractDataFromRequestXml(HttpRequestData request)
        {
            //get xml string from caller
            var xmlString = (await request?.ReadAsStringAsync()) ?? "<Empty/>";

            //parse xml string
            var xml = XElement.Parse(xmlString);

            return xml;
        }

        /// <summary>
        /// Extracts data from HTTP request and turn it into a XML
        /// </summary>
        public static async Task<XElement> RequestToXml(HttpRequestData requestData)
        {
            var requestXml = new XElement("Request");

            //STAGE 1 : HEADERS
            var headersRaw = requestData?.Headers.ToList();
            var headerXml = new XElement("HeaderList");
            foreach (var keyValuePair in headersRaw)
            {
                var headData = string.Join(",", keyValuePair.Value);
                var headName = keyValuePair.Key;
                headerXml.Add(new XElement(headName, headData));
            }

            //add headers separately last, because generated separately
            requestXml.Add(headerXml);

            //STAGE 2 : OTHER DATA
            var propList = new Dictionary<string, string>()
            {
                ["Method"] = requestData.Method,
                ["Url"] = requestData.Url.ToString(),
                ["IP"] = requestData?.GetCallerIp().ToString() ?? "no ip!",
                ["Body"] = await requestData?.ReadAsStringAsync() ?? "Empty",
            };

            foreach (var property in propList)
            {
                var tempXml = new XElement(property.Key, property.Value);
                requestXml.Add(tempXml);
            }

            return requestXml;
        }

    }
}
