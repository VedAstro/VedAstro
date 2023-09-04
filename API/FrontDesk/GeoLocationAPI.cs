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
using Newtonsoft.Json.Linq;

namespace API
{
    /// <summary>
    /// Wrapper API for Google API with backup built in cache
    /// NOTE: all Google API code should only be here
    /// </summary>
    public class GeoLocationAPI
    {

        private const string AddressToGeoLocationRoute = "AddressToGeoLocation/{address}";
        private const string CoordinatesToGeoLocationRoute = "CoordinatesToGeoLocation/Latitude/{latitude}/Longitude/{longitude}";
        private const string GeoLocationToTimezoneRoute = "GeoLocationToTimezone/{*timeUrl}";

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

        [Function(nameof(AddressToGeoLocation))]
        public static async Task<HttpResponseData> AddressToGeoLocation([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = AddressToGeoLocationRoute)]
            HttpRequestData incomingRequest,
            string address
        )
        {
            //0 : LOG CALL
            //log ip address, call time and URL
            var call = APILogger.OpenApiCall(incomingRequest);

            //1 : CALCULATE
            var parsedGeoLocation = await AddressToGeoLocation_VedAstro(address);
            //use google only if no cache in VedAstro
            if (parsedGeoLocation.Name() == GeoLocation.Empty.Name())
            {
                parsedGeoLocation = await AddressToGeoLocation_Google(address);
                //add new data to cache, for future speed up
                AddToCache(parsedGeoLocation);
            }


            //2 : SEND TO CALLER
            return APITools.PassMessage((XElement)parsedGeoLocation.ToXml(), incomingRequest);
        }

        [Function(nameof(CoordinatesToGeoLocation))]
        public static async Task<HttpResponseData> CoordinatesToGeoLocation([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = CoordinatesToGeoLocationRoute)]
            HttpRequestData incomingRequest,
            string latitude, string longitude
        )
        {
            //0 : LOG CALL
            //log ip address, call time and URL
            var call = APILogger.OpenApiCall(incomingRequest);

            //1 : CALCULATE
            var parsedGeoLocation = await CoordinatesToGeoLocation_Vedastro(longitude, latitude);
            //use google only if no cache in VedAstro
            if (parsedGeoLocation.Name() == GeoLocation.Empty.Name())
            {
                parsedGeoLocation = await CoordinatesToGeoLocation_Google(longitude, latitude);
                //add new data to cache, for future speed up
                AddToCache(parsedGeoLocation);
            }


            //2 : SEND TO CALLER
            return APITools.PassMessage((XElement)parsedGeoLocation.ToXml(), incomingRequest);
        }

        [Function(nameof(GeoLocationToTimezone))]
        public static async Task<HttpResponseData> GeoLocationToTimezone([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = GeoLocationToTimezoneRoute)]
            HttpRequestData incomingRequest,
            string timeUrl
        )
        {
            //0 : LOG CALL
            //log ip address, call time and URL
            var call = APILogger.OpenApiCall(incomingRequest);

            //1 : GET DATA OUT TODO NEEDS CHECKING
            Time x = await Time.FromUrl(timeUrl);
            var timeAtLocation = x.GetStdDateTimeOffset();
            var geoLocation = x.GetGeoLocation();

            //2 : CALCULATE
            var timezoneStr = await GeoLocationToTimezone_Vedastro(geoLocation, timeAtLocation);
            //use google only if no cache in VedAstro
            if (string.IsNullOrEmpty(timezoneStr))
            {
                timezoneStr = await GeoLocationToTimezone_Google(geoLocation, timeAtLocation);
                //add new data to cache, for future speed up
                AddToCache(geoLocation, timeAtLocation.Ticks.ToString(),timezoneStr);
            }


            //3 : SEND TO CALLER
            return APITools.PassMessage(timezoneStr, incomingRequest);
        }



        //---------------------------------------------------------------
        private static async Task<string> GeoLocationToTimezone_Vedastro(GeoLocation geoLocation, DateTimeOffset timeAtLocation)
        {

            //time that is linked to timezone
            var timezoneTime = timeAtLocation.Ticks.ToString();

            //do direct search in table
            //NOTE: don't search by location name, because geo location name might be empty
            var linqEntities =
                tableClient.Query<GeoLocationCacheEntity>(
                    call => call.RowKey == timezoneTime //match time for timezone
                                                    && call.Latitude == geoLocation.Latitude()
                                                    && call.Longitude == geoLocation.Longitude());

            //ro
            var foundRaw = linqEntities?.FirstOrDefault()?.Timezone ?? "";

            return foundRaw;

        }

        private static async Task<string> GeoLocationToTimezone_Google(GeoLocation geoLocation, DateTimeOffset timeAtLocation)
        {
            var returnResult = new WebResult<string>();

            //use timestamp to account for historic timezone changes
            var locationTimeUnix = timeAtLocation.ToUnixTimeSeconds();
            var longitude = geoLocation.Longitude();
            var latitude = geoLocation.Latitude();

            //create the request url for Google API 
            var apiKey = Secrets.GoogleAPIKey;
            var url = string.Format($@"https://maps.googleapis.com/maps/api/timezone/xml?location={latitude},{longitude}&timestamp={locationTimeUnix}&key={apiKey}");

            //get raw location data from GoogleAPI
            var apiResult = await Tools.ReadFromServerXmlReply(url);

            //if result from API is a failure then use system timezone
            //this is clearly an error, as such log it
            TimeSpan offsetMinutes;
            if (apiResult.IsPass) //all well
            {
                //get the raw data from google
                var timeZoneResponseXml = apiResult.Payload;

                //try parse Google API's payload
                var isParsed = Tools.TryParseGoogleTimeZoneResponse(timeZoneResponseXml, out offsetMinutes);
                if (!isParsed) { goto Fail; } //not parsed end here

                //convert to string exp: +08:00
                var parsedOffsetString = Tools.TimeSpanToUTCTimezoneString(offsetMinutes);

                //place data inside capsule
                returnResult.Payload = parsedOffsetString;
                returnResult.IsPass = true;

                return returnResult.Payload;
            }

        Fail:
            //mark as fail & use possibly inaccurate backup timezone (client browser's timezone)
            returnResult.IsPass = false;
            offsetMinutes = Tools.GetSystemTimezone();
            returnResult.Payload = Tools.TimeSpanToUTCTimezoneString(offsetMinutes);
            return returnResult.Payload;

        }

        /// <summary>
        /// Will add new cache to Geo Location Cache
        /// </summary>
        private static void AddToCache(GeoLocation parsedGeoLocation, string dateTimeTimezoneTicks = "", string timezone = "")
        {
            //package the data
            GeoLocationCacheEntity customerEntity = new GeoLocationCacheEntity()
            {
                PartitionKey = parsedGeoLocation.Name(),
                RowKey = dateTimeTimezoneTicks,
                Timezone = timezone,
                Latitude = parsedGeoLocation.Latitude(),
                Longitude = parsedGeoLocation.Longitude()
            };

            //creates record if no exist, update if already there
            tableClient.UpsertEntity(customerEntity);
        }

        /// <summary>
        /// Gets coordinates from Google API
        /// </summary>
        private static async Task<GeoLocation> CoordinatesToGeoLocation_Google(string longitude, string latitude)
        {
            var apiKey = Secrets.GoogleAPIKey;
            var urlReverse = $"https://maps.googleapis.com/maps/api/geocode/json?latlng={latitude},{longitude}&key={apiKey}";
            var resultString2 = await Tools.WriteServer(HttpMethod.Post, urlReverse);

            var resultsJson = resultString2["results"][0];

            var locationNameLong = resultsJson["formatted_address"].Value<string>();
            var splitted = locationNameLong.Split(',');
            
            //keep only the last parts, country, state...
            var newLocationName = $"{splitted[splitted.Length - 3]},{splitted[splitted.Length - 2]},{splitted[splitted.Length - 1]}";

            var fromIpAddress = new GeoLocation(newLocationName, double.Parse(longitude), double.Parse(latitude));

            return fromIpAddress;
        }

        /// <summary>
        /// Will return empty Geo Location if no cache
        /// </summary>
        private static async Task<GeoLocation> AddressToGeoLocation_VedAstro(string address)
        {
            //do direct search for address in name field 
            Pageable<GeoLocationCacheEntity> linqEntities = tableClient.Query<GeoLocationCacheEntity>(call => call.PartitionKey == address);

            //if old call found check if running else default false
            //NOTE : important return empty, because used to detect later if empty
            var foundRaw = linqEntities?.FirstOrDefault()?.ToGeoLocation() ?? GeoLocation.Empty;

            return foundRaw;

        }

        /// <summary>
        /// Will return empty Geo Location if no cache
        /// </summary>
        private static async Task<GeoLocation> CoordinatesToGeoLocation_Vedastro(string longitude, string latitude)
        {
            //do direct search for address in name field 
            var linqEntities = 
                tableClient.Query<GeoLocationCacheEntity>(
                    call => call.Longitude == double.Parse(longitude) && call.Latitude == double.Parse(latitude));

            //if old call found check if running else default false
            //NOTE : important return empty, because used to detect later if empty
            var foundRaw = linqEntities?.FirstOrDefault()?.ToGeoLocation() ?? GeoLocation.Empty;

            return foundRaw;

        }

        private static async Task<GeoLocation> AddressToGeoLocation_Google(string address)
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
