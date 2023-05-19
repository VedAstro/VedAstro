using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Azure.Functions.Worker.Http;
using System.Xml.Linq;

namespace API
{
    /// <summary>
    /// A collection of general tools used by API
    /// </summary>
    public static partial class APITools
    {
        /// <summary>
        /// Overwrites new XML data to a blob file
        /// </summary>
        public static async Task OverwriteBlobData(BlobClient blobClient, XDocument newData)
        {
            //convert xml data to string
            var dataString = newData.ToString();

            //convert xml string to stream
            var dataStream = GenerateStreamFromString(dataString);

            //upload stream to blob
            await blobClient.UploadAsync(dataStream, overwrite: true);

            //auto correct content type from wrongly set "octet/stream"
            var blobHttpHeaders = new BlobHttpHeaders { ContentType = "text/xml" };
            await blobClient.SetHttpHeadersAsync(blobHttpHeaders);
        }

        public static async Task<XElement> ExtractDataFromRequestXml(HttpRequestData request)
        {
            //get xml string from caller
            var xmlString = (await request?.ReadAsStringAsync()) ?? "<Empty/>";

            //parse xml string
            var xml = XElement.Parse(xmlString);

            return xml;
        }
        /// <summary>
        /// Adds an XML element to XML document in blob form
        /// </summary>
        public static async Task<XDocument> AddXElementToXDocument(BlobClient xDocuBlobClient, XElement newElement)
        {
            //get person list from storage
            var xDocument = await DownloadToXDoc(xDocuBlobClient);

            //add new person to list
            xDocument.Root.Add(newElement);

            return xDocument;
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
