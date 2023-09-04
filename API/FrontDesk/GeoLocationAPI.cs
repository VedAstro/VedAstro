using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Azure.Data.Tables;
using Azure;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using VedAstro.Library;
using static API.CallTracker;
using System.Xml.Linq;

namespace API
{
    /// <summary>
    /// Wrapper API for Google API with backup built in cache
    /// NOTE: all Google API code should only be here
    /// </summary>
    public class GeoLocationAPI
    {

        private const string AddressToGeoLocationRoute = "AddressToGeoLocation/{address}";

        private static readonly TableServiceClient tableServiceClient;
        private static string tableName = "GeoLocationCache";
        private static readonly TableClient tableClient;

        /// <summary>
        /// init Table access
        /// </summary>
        static GeoLocationAPI()
        {
            //todo cleanup
            var storageUri = $"https://vedastroapistorage.table.core.windows.net/{tableName}";
            string accountName = "vedastroapistorage";
            string storageAccountKey = Secrets.VedAstroApiStorageKey;

            //save reference for late use
            tableServiceClient = new TableServiceClient(new Uri(storageUri), new TableSharedKeyCredential(accountName, storageAccountKey));
            tableClient = tableServiceClient.GetTableClient(tableName);

        }
        /// <summary>
        /// Backup function to catch invalid calls, say gracefully fails
        /// NOTE: "z" in name needed to make as last API call, else will be called all the time
        /// </summary>
        [Function(nameof(AddressToGeoLocation))]
        public static async Task<HttpResponseData> AddressToGeoLocation([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = AddressToGeoLocationRoute)]
            HttpRequestData incomingRequest,
            string address
        )
        {
            //0 : LOG CALL
            //log ip address, call time and URL
            var call = APILogger.OpenApiCall(incomingRequest);

            //1 : CALCULATE
            var parsedGeoLocation = await UseVedAstroAPICache(address);
            //use google only if no cache in VedAstro
            if (parsedGeoLocation.Name() == GeoLocation.Empty.Name()) { parsedGeoLocation = await CallGoogleAPI(address); }


            //2 : SEND TO CALLER
            return APITools.PassMessage((XElement)parsedGeoLocation.ToXml(), incomingRequest);
        }


        /// <summary>
        /// Will return empty Geo Location if no cache
        /// </summary>
        private static async Task<GeoLocation> UseVedAstroAPICache(string address)
        {
            //do direct search for address in name field 
            Pageable<GeoLocationCacheEntity> linqEntities = tableClient.Query<GeoLocationCacheEntity>(call => call.PartitionKey == address);

            //if old call found check if running else default false
            //NOTE : important return empty, because used to detect later if empty
            var foundRaw = linqEntities?.FirstOrDefault()?.ToGeoLocation() ?? GeoLocation.Empty;

            return foundRaw;

        }

        private static async Task<GeoLocation> CallGoogleAPI(string address)
        {
            //if null or empty turn back as nothing
            if (string.IsNullOrEmpty(address)) { return new WebResult<GeoLocation>(false, GeoLocation.Empty); }

            //create the request url for Google API
            var apiKey = Secrets.GoogleAPIKey;
            var url = $"https://maps.googleapis.com/maps/api/geocode/xml?key={apiKey}&address={Uri.EscapeDataString(address)}&sensor=false";

            //get location data from GoogleAPI
            var webResult = await Tools.ReadFromServerXmlReply(url);

            //if fail to make call, end here
            if (!webResult.IsPass) { return new WebResult<GeoLocation>(false, GeoLocation.Empty); }

            //if success, get the reply data out
            var geocodeResponseXml = webResult.Payload;
            var resultXml = geocodeResponseXml.Element("result");
            var statusXml = geocodeResponseXml.Element("status");

#if DEBUG
            //DEBUG
            Console.WriteLine(geocodeResponseXml.ToString());
#endif

            //check the data, if location was NOT found by google API, end here
            if (statusXml == null || statusXml.Value == "ZERO_RESULTS") { return new WebResult<GeoLocation>(false, GeoLocation.Empty); }

            //if success, extract out the longitude & latitude
            var locationElement = resultXml?.Element("geometry")?.Element("location");
            var lat = double.Parse(locationElement?.Element("lat")?.Value ?? "0");
            var lng = double.Parse(locationElement?.Element("lng")?.Value ?? "0");

            //round coordinates to 3 decimal places
            lat = Math.Round(lat, 3);
            lng = Math.Round(lng, 3);

            //get full name with country & state
            var fullName = resultXml?.Element("formatted_address")?.Value;

            //return to caller pass
            return new WebResult<GeoLocation>(true, new GeoLocation(fullName, lng, lat));
        }
    }
}
