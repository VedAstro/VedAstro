using Azure.Data.Tables;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VedAstro.Library;
using System.Linq.Expressions;
using Azure.Data.Tables;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net;
using Microsoft.Azure.Functions.Worker.Http;
using ScottPlot.Palettes;
using System.Reflection;
using System.ComponentModel;

namespace API
{
    public class ApiStatistic
    {
        /// <summary>
        /// sample holder type when doing interop
        /// </summary>
        public record GeoLocationRawAPI(dynamic MainRow, dynamic MetadataRow);

        private readonly TableServiceClient ipAddressServiceClient;
        private readonly TableServiceClient requestUrlStatisticServiceClient;
        private readonly TableServiceClient rawRequestStatisticServiceClient;
        private readonly TableServiceClient ipAddressMetadataServiceClient;

        private readonly TableClient ipAddressStatisticTableClient;
        private readonly TableClient requestUrlStatisticTableClient;
        private readonly TableClient rawRequestStatisticTableClient;
        private readonly TableClient ipAddressStatisticMetadataTableClient;




        /// <summary>
        /// init Table access
        /// </summary>
        public ApiStatistic()
        {
            string accountName = "centralapistorage"; //indic heritage 
                                                      //string accountName = "vedastroapistorage"; //vedastro 

            
            //# RAW REQUEST : (use only when needed, costly🤑)
            //------------------------------------
            //Initialize address table 
            string tableNameRawRequestStatistic = "RawRequestStatistic";
            var storageUriRawRequestStatistic = $"https://{accountName}.table.core.windows.net/{tableNameRawRequestStatistic}";
            //save reference for late use
            rawRequestStatisticServiceClient = new TableServiceClient(new Uri(storageUriRawRequestStatistic), new TableSharedKeyCredential(accountName, Secrets.AzureGeoLocationStorageKey));
            rawRequestStatisticTableClient = rawRequestStatisticServiceClient.GetTableClient(tableNameRawRequestStatistic);


            //# REQUEST URL
            //------------------------------------
            //Initialize address table 
            string tableNameRequestUrlStatistic = "RequestUrlStatistic";
            var storageUriRequestUrlStatistic = $"https://{accountName}.table.core.windows.net/{tableNameRequestUrlStatistic}";
            //save reference for late use
            requestUrlStatisticServiceClient = new TableServiceClient(new Uri(storageUriRequestUrlStatistic), new TableSharedKeyCredential(accountName, Secrets.AzureGeoLocationStorageKey));
            requestUrlStatisticTableClient = requestUrlStatisticServiceClient.GetTableClient(tableNameRequestUrlStatistic);

            
            //# IP ADDRESS
            //------------------------------------
            //Initialize address table 
            string tableNameIpAddressStatistic = "IpAddressStatistic";
            var storageUriIpAddressStatistic = $"https://{accountName}.table.core.windows.net/{tableNameIpAddressStatistic}";
            //save reference for late use
            ipAddressServiceClient = new TableServiceClient(new Uri(storageUriIpAddressStatistic), new TableSharedKeyCredential(accountName, Secrets.AzureGeoLocationStorageKey));
            ipAddressStatisticTableClient = ipAddressServiceClient.GetTableClient(tableNameIpAddressStatistic);

            //Initialize address metadata table 
            string tableNameIpAddressMetadata = "IpAddressGeoLocationMetadata";
            var storageUriIpAddressMetadata = $"https://{accountName}.table.core.windows.net/{tableNameIpAddressMetadata}";

            //save reference for late use
            ipAddressMetadataServiceClient = new TableServiceClient(new Uri(storageUriIpAddressMetadata), new TableSharedKeyCredential(accountName, Secrets.AzureGeoLocationStorageKey));
            ipAddressStatisticMetadataTableClient = ipAddressMetadataServiceClient.GetTableClient(tableNameIpAddressMetadata);

        }

        //-------------------------------------


        /// <summary>
        /// Logs IP to for statistics
        /// </summary>
        public void LogIpAddress(HttpRequestData incomingRequest)
        {

            //# get ip address out
            var ipAddress = incomingRequest?.GetCallerIp()?.ToString() ?? "0.0.0.0";

            //# check if ip address already exist
            //make a search for ip address stored under row key
            Expression<Func<IpAddressStatisticEntity, bool>> expression = call => call.PartitionKey == ipAddress;

            //execute search
            var recordFound = ipAddressStatisticTableClient.Query(expression).FirstOrDefault();

            //# if existed, update call count
            var isExist = recordFound != null;
            if (isExist)
            {
                //update row
                recordFound.CallCount = ++recordFound.CallCount; //increment call count
                ipAddressStatisticTableClient.UpsertEntity(recordFound);
            }

            //# if not exist, make new log
            else
            {
                var newRow = new IpAddressStatisticEntity();
                newRow.PartitionKey = Tools.CleanAzureTableKey(ipAddress);
                newRow.RowKey = "0";
                newRow.CallCount = 1;
                ipAddressStatisticTableClient.AddEntity(newRow);
            }
        }


        public void LogRequestUrl(HttpRequestData incomingRequest)
        {

            //# get request URL
            var requestUrl = incomingRequest?.Url.ToString() ?? "no URL";

            //# check if URL already exist
            //make a search for ip address stored under row key
            var cleanAzureTableKey = Tools.CleanAzureTableKey(requestUrl, "|");
            Expression<Func<RequestUrlStatisticEntity, bool>> expression = call => call.PartitionKey == cleanAzureTableKey;

            //execute search
            var recordFound = requestUrlStatisticTableClient.Query(expression).FirstOrDefault();

            //# if existed, update call count
            var isExist = recordFound != null;
            if (isExist)
            {
                //update row
                recordFound.CallCount = ++recordFound.CallCount; //increment call count
                requestUrlStatisticTableClient.UpsertEntity(recordFound);
            }

            //# if not exist, make new log
            else
            {
                var newRow = new RequestUrlStatisticEntity();
                
                newRow.PartitionKey = cleanAzureTableKey;
                newRow.RowKey = "0";
                newRow.CallCount = 1;
                requestUrlStatisticTableClient.AddEntity(newRow);
            }
        }


        /// <summary>
        /// Makes raw full header log of what ever that comes in
        /// TODO control with config
        /// NOTE: high cost carefully use
        /// </summary>
        public void LogRawRequest(HttpRequestData incomingRequest)
        {
            //step 1: extract needed data from request
            var newRow = new RawRequestStatisticEntity();

            //convert to list
            var requestHeaderList = incomingRequest.Headers.ToDictionary(x => x.Key, x => x.Value, StringComparer.Ordinal);

            for (int i = 0; i < requestHeaderList.Count; i++)
            {
                var currentHeader = requestHeaderList.ElementAt(i);
                var currentHeaderKey = currentHeader.Key;
                string currentValue = Tools.ListToString(currentHeader.Value.ToList());

                //debug print
                //Console.WriteLine($"{currentHeaderKey}:{currentValue}");

                //match with correct header based on attribute and fill in the value
                // Get all properties of the current instance
                var properties = newRow.GetType().GetProperties();
                foreach (var property in properties)
                {
                    var attribute = (DescriptionAttribute)property.GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault();
                    if (attribute?.Description.Equals(currentHeaderKey, StringComparison.OrdinalIgnoreCase)?? false)
                    {
                        property.SetValue(newRow, currentValue);
                        break;
                    }
                }
            }

            //step 2: generate hash to identify the data
            newRow.PartitionKey = newRow.CalculateCombinedHash();
            newRow.RowKey = Tools.GenerateId(); //unique

            //step 3: add entry to database
            rawRequestStatisticTableClient.AddEntity(newRow);
        }






        //----------------------------------PRIVATE FUNCS-----------------------------


        private void AddToIpAddressMetadataTable(IpAddressGeoLocationMetadataEntity newRow)
        {
            // If an identical entity exists, the call to
            // AddEntity would cause a duplicate entry error.
            // Therefore, before calling AddEntity,
            // ensure that the entity does not already exist
            try
            {
                ipAddressStatisticMetadataTableClient.AddEntity(newRow);
            }
            catch (Exception e)
            {
                //NOTE:
                //here we expect multiple adds of same metadata row,
                //so we do Add instead of Upsert to save money,
                //and if fail because can't add then proceed as normal
#if DEBUG
                Console.WriteLine("Metadata already exist...continuing");
#endif
            }

        }

        private void AddToIpAddressTable(IpAddressGeoLocationEntity newRow)
        {
            // If an identical entity exists, the call to
            // AddEntity would cause a duplicate entry error.
            // Therefore, before calling AddEntity,
            // ensure that the entity does not already exist
            try
            {
                ipAddressStatisticTableClient.AddEntity(newRow);
            }
            catch (Exception e)
            {
                var errorMessage = $"Can't add duplicate IpAddressGeoLocation row : PartKey:{newRow.PartitionKey}, RowKey:{newRow.RowKey}";

#if DEBUG
                Console.WriteLine(errorMessage);
#else
                LibLogger.Error(errorMessage);
#endif
                //this is critical and should not propagate!

                throw new Exception(errorMessage);
            }
        }




        //--------------- VEDASTRO -----------------
        //note async is needed to maintain interop

        private async Task<GeoLocationRawAPI> IpAddressToGeoLocation_VedAstro(string ipAddress)
        {

            //make a search for ip address stored under row key
            Expression<Func<IpAddressGeoLocationEntity, bool>> expression = call => call.PartitionKey == ipAddress;

            //execute search
            var recordFound = ipAddressStatisticTableClient.Query(expression).FirstOrDefault();

            //if old call found check if running else default false
            //NOTE : important return empty, because used to detect later if empty
            var foundRaw = recordFound ?? IpAddressGeoLocationEntity.Empty;

            //we don't supply metadata cause not needed, as separate query
            return new GeoLocationRawAPI(foundRaw, null);
        }







        //--------------- AZURE -----------------

        private static async Task<GeoLocationRawAPI> IpAddressToGeoLocation_Azure(string ipAddress)
        {
            throw new NotImplementedException();

            //TODO
            //            return new WebResult<GeoLocation>(false, GeoLocation.Empty);

            //            if (string.IsNullOrEmpty(ipAddress)) { return new WebResult<GeoLocation>(false, GeoLocation.Empty); }

            //            var apiKey = Secrets.AzureMapsAPIKey;
            //            //var url = $"https://atlas.microsoft.com/ip/reverse/json?api-version=1.0&subscription-key={apiKey}&ipAddress={ipAddress}";
            //            //var apiKey = "<YOUR_API_KEY>";
            //            var baseUrl = @"https://atlas.microsoft.com/ip/reverse/json?api-version=1.0&subscription-key=";
            //            var regionSpecificEndpoint = "WestEurope"; // Change according to your subscription region
            //            var url = $"{baseUrl}{apiKey}&ipAddress={Uri.EscapeDataString(ipAddress)}&region={regionSpecificEndpoint}";

            //            var webResult = await VedAstro.Library.Tools.ReadFromServerJsonReply(url);

            //            if (!webResult.IsPass) { return new WebResult<GeoLocation>(false, GeoLocation.Empty); }

            //            var reverseIPResponseJson = webResult.Payload;

            //#if DEBUG
            //            Console.WriteLine(reverseIPResponseJson.ToString());
            //#endif

            //            //if (reverseIPResponseJson == null || !reverseIPResponseJson.ContainsKey("record")) { return new WebResult<GeoLocation>(false, GeoLocation.Empty); }

            //            var record = reverseIPResponseJson["record"];

            //            if (record == null || !(record is JArray array) || array.Count < 1 || !(array[0] is JObject firstRecord)) { return new WebResult<GeoLocation>(false, GeoLocation.Empty); }

            //            JToken positionToken;
            //            if (!firstRecord.TryGetValue("position", out positionToken)) { return new WebResult<GeoLocation>(false, GeoLocation.Empty); };

            //            JObject positionObj = (JObject)positionToken;

            //            double lat;
            //            double lon;

            //            if (!double.TryParse(positionObj["lat"].Value<string>(), NumberStyles.Float, CultureInfo.InvariantCulture, out lat)) { return new WebResult<GeoLocation>(false, GeoLocation.Empty); }
            //            if (!double.TryParse(positionObj["lon"].Value<string>(), NumberStyles.Float, CultureInfo.InvariantCulture, out lon)) { return new WebResult<GeoLocation>(false, GeoLocation.Empty); }

            //            lat = Math.Round(lat, 3);
            //            lon = Math.Round(lon, 3);

            //            string fullName = "";
            //            if (firstRecord.TryGetValue("address", out JToken addressTok))
            //            {
            //                JObject addressObj = (JObject)addressTok;
            //                if (addressObj.TryGetValue("freeformAddress", StringComparison.OrdinalIgnoreCase, out JToken addrTextTok))
            //                {
            //                    fullName = addrTextTok.Value<string>();
            //                }
            //            }

            //            if (string.IsNullOrWhiteSpace(fullName))
            //            {
            //                if (firstRecord.TryGetValue("locality", out JToken localityTok))
            //                {
            //                    fullName = localityTok.Value<string>();
            //                }
            //            }

            //            if (string.IsNullOrWhiteSpace(fullName))
            //            {
            //                if (firstRecord.TryGetValue("administrativeArea5", out JToken area5Tok))
            //                {
            //                    fullName = area5Tok.Value<string>();
            //                }
            //            }

            //            if (string.IsNullOrWhiteSpace(fullName))
            //            {
            //                if (firstRecord.TryGetValue("administrativeArea3", out JToken area3Tok))
            //                {
            //                    fullName = area3Tok.Value<string>();
            //                }
            //            }

            //            if (string.IsNullOrWhiteSpace(fullName))
            //            {
            //                if (firstRecord.TryGetValue("countrySubdivision", out JToken subDivTok))
            //                {
            //                    fullName = subDivTok.Value<string>();
            //                }
            //            }

            //            if (string.IsNullOrWhiteSpace(fullName))
            //            {
            //                if (firstRecord.TryGetValue("countryCode", out JToken countryCodeTok))
            //                {
            //                    fullName = countryCodeTok.Value<string>();
            //                }
            //            }

            //            if (string.IsNullOrWhiteSpace(fullName))
            //            {
            //                return new WebResult<GeoLocation>(false, GeoLocation.Empty);
            //            }

            //            return new WebResult<GeoLocation>(true, new GeoLocation(fullName, lon, lat));
        }

        private static async Task<GeoLocationRawAPI> AddressToGeoLocation_Azure(string userInputAddress)
        {
            var returnResult = new WebResult<GeoLocationRawAPI>();

            //create the request url for Azure Maps API
            var apiKey = Secrets.AzureMapsAPIKey;
            var url = $"https://atlas.microsoft.com/search/address/json?api-version=1.0&subscription-key={apiKey}&query={Uri.EscapeDataString(userInputAddress)}";

            //get location data from Azure Maps API
            var apiResult = await Tools.ReadFromServerJsonReply(url);

            // If result from API is a failure, use the system time zone as fallback
            if (apiResult.IsPass) // All well
            {
                // Parse Azure API's payload
                var outData = TryParseAzureAddressResponse(apiResult.Payload, userInputAddress);
                bool isParsed = outData.IsParsed;
                if (isParsed)
                {
                    // Convert to string (example: +08:00)
                    returnResult.Payload = new GeoLocationRawAPI(outData.MainRow, null);
                    returnResult.IsPass = true;
                }
                else
                {
                    // Mark as fail & return empty for fail detection
                    returnResult.IsPass = false;
                    returnResult.Payload = new GeoLocationRawAPI(AddressGeoLocationEntity.Empty, null);
                }
            }
            else
            {
                // Mark as fail & return empty for fail detection
                returnResult.IsPass = false;
                returnResult.Payload = new GeoLocationRawAPI(GeoLocationTimezoneEntity.Empty, GeoLocationTimezoneMetadataEntity.Empty);
            }

            return returnResult;

        }

        private static async Task<GeoLocationRawAPI> GeoLocationToTimezone_Azure(GeoLocation geoLocation, DateTimeOffset timeAtLocation)
        {
            var returnResult = new WebResult<GeoLocationRawAPI>();

            // Convert the DateTimeOffset to ISO 8601 format ("yyyy-MM-ddTHH:mm:ssZ")
            var locationTimeIso8601 = timeAtLocation.ToString("O", System.Globalization.CultureInfo.InvariantCulture);

            // Create the request URL for Azure Maps API
            var apiKey = Secrets.AzureMapsAPIKey;
            var url = $@"https://atlas.microsoft.com/timezone/byCoordinates/json?api-version=1.0&subscription-key={apiKey}&query={geoLocation.Latitude()},{geoLocation.Longitude()}&timestamp={Uri.EscapeDataString(locationTimeIso8601)}";

            // Get raw location data from Azure Maps API
            var apiResult = await Tools.ReadFromServerJsonReply(url);

            // If result from API is a failure, use the system time zone as fallback
            if (apiResult.IsPass) // All well
            {
                // Parse Azure API's payload
                var outData = TryParseAzureTimeZoneResponse(timeAtLocation, geoLocation, apiResult.Payload);
                bool isParsed = outData.IsParsed;
                if (isParsed)
                {
                    // Convert to string (example: +08:00)
                    returnResult.Payload = new GeoLocationRawAPI(outData.MainRow, outData.MetadataRow);
                    returnResult.IsPass = true;
                }
                else
                {
                    // Mark as fail & return empty for fail detection
                    returnResult.IsPass = false;
                    returnResult.Payload = new GeoLocationRawAPI(GeoLocationTimezoneEntity.Empty, GeoLocationTimezoneMetadataEntity.Empty);
                }
            }
            else
            {
                // Mark as fail & return empty for fail detection
                returnResult.IsPass = false;
                returnResult.Payload = new GeoLocationRawAPI(GeoLocationTimezoneEntity.Empty, GeoLocationTimezoneMetadataEntity.Empty);
            }

            return returnResult;
        }

        private static dynamic TryParseAzureTimeZoneResponse(DateTimeOffset timeAtLocation, GeoLocation geoLocation, JToken timeZoneResponseJson)
        {
            try
            {
                //STEP 1: validate the data
                var timeZonesArray = timeZoneResponseJson["TimeZones"] as JArray;
                if (timeZonesArray == null || !timeZonesArray.HasValues)
                    throw new ArgumentException($"Invalid or missing 'TimeZones' property in Azure timezone response.");

                var firstTimeZoneObject = timeZonesArray[0] as JObject;
                if (firstTimeZoneObject == null)
                    throw new ArgumentException($"Invalid or missing timezone object in Azure timezone response.");

                var referenceTimeObject = firstTimeZoneObject["ReferenceTime"] as JObject;
                if (referenceTimeObject == null)
                    throw new ArgumentException($"Invalid or missing 'ReferenceTime' object in Azure timezone response.");

                //STEP 2: get data out 
                var standardOffsetString = referenceTimeObject["StandardOffset"].Value<string>();
                var stdOffsetMinutes = TimeSpan.Parse(standardOffsetString);

                //difference in daylight savings
                //when no daylight saving will return 0
                var daylightSavingsString = referenceTimeObject["DaylightSavings"].Value<string>();
                var daylightSavingsOffsetMinutes = TimeSpan.Parse(daylightSavingsString);

                //add standard never changing offset to daylight savings to get final accurate timezone
                var finalOffsetMinutes = stdOffsetMinutes + daylightSavingsOffsetMinutes;
                var timezoneText = VedAstro.Library.Tools.TimeSpanToUTCTimezoneString(finalOffsetMinutes);

                //STEP 3: CONVERT FORMAT (AZURE -> VEDASTRO CACHE DB)
                //NOTE: from Azure's response, 2 table row data types is created,
                //      timezone data and metadata (stored separately to save duplicate writes)

                //# TYPE 1 : ONLY TIMEZONE BY COORDINATES
                var timezoneRow = new GeoLocationTimezoneEntity();
                timezoneRow.PartitionKey = geoLocation.GetPartitionKey();

                //NOTE :reduce accuracy to days so time is removed (this only writes, another checks)
                //      done to reduce cache clogging, so might miss offset by hours but not days
                //      !!DO NOT lower accuracy below time as needed for Western daylight saving changes!! 
                DateTimeOffset roundedTime = new DateTimeOffset(timeAtLocation.Year, timeAtLocation.Month, timeAtLocation.Day, 0, 0, 0, timeAtLocation.Offset);
                timezoneRow.TimezoneText = timezoneText;
                timezoneRow.RowKey = roundedTime.ToRowKey();

                //# TYPE 2 : METADATA FOR TIMEZONE
                //fill the timezone metadata row details
                var timezoneMetadataRow = new GeoLocationTimezoneMetadataEntity();
                timezoneMetadataRow.StandardOffset = Tools.TimeSpanToUTCTimezoneString(stdOffsetMinutes);
                timezoneMetadataRow.DaylightSavings = Tools.TimeSpanToUTCTimezoneString(daylightSavingsOffsetMinutes);
                timezoneMetadataRow.Tag = (referenceTimeObject["Tag"] ?? "").Value<string>() ?? string.Empty;
                timezoneMetadataRow.Standard_Name = (firstTimeZoneObject["Names"]?["Standard"] ?? "").Value<string>() ?? string.Empty;
                timezoneMetadataRow.Daylight_Name = (firstTimeZoneObject["Names"]?["Daylight"] ?? "").Value<string>() ?? string.Empty;
                timezoneMetadataRow.ISO_Name = (firstTimeZoneObject["Id"] ?? "").Value<string>() ?? string.Empty;
                timezoneMetadataRow.RowKey = "0";//DateTimeOffset.Now.ToOffset(TimeSpan.FromHours(8)).ToRowKey(); //to know when was created

                //NOTE: linking is done last, because hash is based on data, and it needs to be filled 1st
                timezoneMetadataRow.PartitionKey = timezoneMetadataRow.CalculateCombinedHash();
                timezoneRow.MetadataHash = timezoneMetadataRow.PartitionKey; //link the timezone to it's metadata

                return new { IsParsed = true, MainRow = timezoneRow, MetadataRow = timezoneMetadataRow };
            }
            catch
            {
                //if fail return empty and fail
                return new { IsParsed = false, MainRow = GeoLocationTimezoneEntity.Empty, MetadataRow = GeoLocationTimezoneMetadataEntity.Empty };
            }
        }

        private static dynamic TryParseAzureAddressResponse(JToken geocodeResponseJson, string userInputAddress)
        {
            try
            {
                var rawAzureReply = geocodeResponseJson["results"][0];

                //check the data, if location was NOT found by Azure Maps API, end here
                if (rawAzureReply == null || rawAzureReply["type"].Value<string>() != "Geography") { return null; }

                //if success, extract out the longitude & latitude
                var locationElement = rawAzureReply["position"];
                var lat = double.Parse(locationElement["lat"].Value<string>() ?? "0");
                var lng = double.Parse(locationElement["lon"].Value<string>() ?? "0");

                //round coordinates to 3 decimal places
                lat = Math.Round(lat, 3);
                lng = Math.Round(lng, 3);

                //get full name with country & state
                var freeformAddress = rawAzureReply["address"]["freeformAddress"].Value<string>();
                var country = rawAzureReply["address"]["country"].Value<string>();
                var fullName = $"{freeformAddress}, {country}";


                //# MAIN ROW
                var mainRow = new AddressGeoLocationEntity();
                mainRow.Latitude = lat;
                mainRow.Longitude = lng;
                mainRow.PartitionKey = fullName;
                mainRow.RowKey = Tools.CleanAzureTableKey(userInputAddress); //todo verify clean procedure


                return new { IsParsed = true, MainRow = mainRow };

            }
            catch
            {
                //if fail return empty and fail
                return new { IsParsed = false, TimezoneRow = AddressGeoLocationEntity.Empty };
            }
        }

        private static dynamic TryParseGoogleAddressResponse(JToken geocodeResponseJson, string userInputAddress)
        {
            try
            {
                var resultJson = geocodeResponseJson["results"];
                var statusJson = geocodeResponseJson["status"];

#if DEBUG
                //DEBUG
                Console.WriteLine(geocodeResponseJson.ToString());
#endif

                //check the data, if location was NOT found by google API, end here
                //TODO log error properly
                var statusMsg = statusJson.Value<string>();
                //if (statusXml == null || statusMsg == "ZERO_RESULTS" || statusMsg == "REQUEST_DENIED") { return new WebResult<GeoLocation>(false, GeoLocation.Empty); }

                //if success, extract out the longitude & latitude
                var locationData = resultJson[0]; //select first result
                var locationElement = locationData["geometry"]["location"];
                var lat = locationElement["lat"].Value<double>();
                var lng = locationElement["lng"].Value<double>();


                //round coordinates to 3 decimal places
                lat = Math.Round(lat, 3);
                lng = Math.Round(lng, 3);

                //get full name with country & state
                var fullName = locationData["formatted_address"].Value<string>();


                //# MAIN ROW
                var mainRow = new AddressGeoLocationEntity();
                mainRow.Latitude = lat;
                mainRow.Longitude = lng;
                mainRow.PartitionKey = Tools.CleanAzureTableKey(fullName);
                mainRow.RowKey = Tools.CleanAzureTableKey(userInputAddress);


                return new { IsParsed = true, MainRow = mainRow };

            }
            catch
            {
                //if fail return empty and fail
                return new { IsParsed = false, TimezoneRow = AddressGeoLocationEntity.Empty };
            }
        }

        private static dynamic TryParseGoogleCoordinatesResponse(JToken geocodeResponseJson, double longitude, double latitude)
        {
            try
            {
                //get address data out
                //NOTE: multiple locations that match are given, only 1 first is taken
                var resultsJson = geocodeResponseJson["results"][0];
                var locationNameLong = resultsJson["formatted_address"].Value<string>();
                var splitted = locationNameLong.Split(',');

                //NOTE: front part sometimes contain, street address, not needed
                //keep only the last parts, country, state... EXP : Ipoh, Perak, Malaysia
                var fullName = $"{splitted[splitted.Length - 3]},{splitted[splitted.Length - 2]},{splitted[splitted.Length - 1]}";


                //# MAIN ROW
                var mainRow = new CoordinatesGeoLocationEntity();
                mainRow.PartitionKey = latitude.ToString();
                mainRow.RowKey = longitude.ToString();
                mainRow.Name = fullName.Trim(); //remove leading & trailing white space if any

                return new { IsParsed = true, MainRow = mainRow };

            }
            catch
            {
                //if fail return empty and fail
                return new { IsParsed = false, MainRow = CoordinatesGeoLocationEntity.Empty };
            }
        }




        //--------------- IP DATA -----------------


        private static async Task<GeoLocationRawAPI> IpAddressToGeoLocation_IpData(string ipAddress)
        {
            const string baseUrl = "https://api.ipdata.co";
            var apiKey = Secrets.IpDataAPIKey;
            var requestUri = $"{baseUrl}/{ipAddress}?api-key={apiKey}";

            //get location data from Azure Maps API
            var apiResult = await Tools.ReadFromServerJsonReply(requestUri);

            //based on reply from API pack data for caller
            var returnResult = new WebResult<GeoLocationRawAPI>();
            if (apiResult.IsPass) // ALL WELL
            {
                // Parse Azure API's payload
                var outData = TryParseIpDataCoordinatesResponse(apiResult.Payload, ipAddress);
                bool isParsed = outData.IsParsed;
                if (isParsed)
                {
                    // Convert to string (example: +08:00)
                    returnResult.Payload = new GeoLocationRawAPI(outData.MainRow, outData.MetadataRow);
                    returnResult.IsPass = true;
                }
                else
                {
                    // Mark as fail & return empty for fail detection
                    returnResult.IsPass = false;
                    returnResult.Payload = new GeoLocationRawAPI(IpAddressGeoLocationEntity.Empty, IpAddressGeoLocationMetadataEntity.Empty);
                }
            }
            else // FAIL
            {
                // Mark as fail & return empty for fail detection
                returnResult.IsPass = false;
                returnResult.Payload = new GeoLocationRawAPI(IpAddressGeoLocationEntity.Empty, IpAddressGeoLocationMetadataEntity.Empty);
            }

            return returnResult;

        }

        private static dynamic TryParseIpDataCoordinatesResponse(JToken geocodeResponseJson, string ipAddress)
        {
            try
            {

                //if success, extract out the longitude & latitude
                var latitude = double.Parse(geocodeResponseJson?["latitude"]?.Value<string>() ?? "0");
                var longitude = double.Parse(geocodeResponseJson?["longitude"]?.Value<string>() ?? "0");

                //round coordinates to 3 decimal places
                latitude = Math.Round(latitude, 3);
                longitude = Math.Round(longitude, 3);

                //get full name with country & state
                var region = geocodeResponseJson["region"].Value<string>();
                var country = geocodeResponseJson["country_name"].Value<string>();
                var fullName = $"{region}, {country}";

                //meta data
                var asn_name = geocodeResponseJson["asn"]["name"].Value<string>();
                var timezone_name = geocodeResponseJson["time_zone"]["name"].Value<string>();
                var timezone_offset = geocodeResponseJson["time_zone"]["offset"].Value<string>();
                var is_proxy = geocodeResponseJson["threat"]["is_proxy"].Value<string>();
                var is_datacenter = geocodeResponseJson["threat"]["is_datacenter"].Value<string>();
                var is_anonymous = geocodeResponseJson["threat"]["is_anonymous"].Value<string>();
                var is_known_attacker = geocodeResponseJson["threat"]["is_known_attacker"].Value<string>();
                var is_known_abuser = geocodeResponseJson["threat"]["is_known_abuser"].Value<string>();
                var is_threat = geocodeResponseJson["threat"]["is_threat"].Value<string>();
                var is_bogon = geocodeResponseJson["threat"]["is_bogon"].Value<string>();


                //# MAIN ROW
                var mainRow = new IpAddressGeoLocationEntity();
                mainRow.PartitionKey = ipAddress;
                mainRow.RowKey = "";
                mainRow.Latitude = latitude;
                mainRow.Longitude = longitude;
                mainRow.LocationName = fullName.Trim(); //remove leading & trailing white space if any


                //# METADATA
                //fill the metadata row details
                var metadataRow = new IpAddressGeoLocationMetadataEntity();
                metadataRow.AsnName = asn_name;
                metadataRow.TimezoneName = timezone_name;
                metadataRow.TimezoneOffset = timezone_offset;
                metadataRow.IsProxy = is_proxy;
                metadataRow.IsDatacenter = is_datacenter;
                metadataRow.IsAnonymous = is_anonymous;
                metadataRow.IsKnownAttacker = is_known_attacker;
                metadataRow.IsKnownAbuser = is_known_abuser;
                metadataRow.IsThreat = is_threat;
                metadataRow.IsBogon = is_bogon;

                metadataRow.RowKey = "0";//DateTimeOffset.Now.ToOffset(TimeSpan.FromHours(8)).ToRowKey(); //to know when was created

                //NOTE: linking is done last, because hash is based on data, and it needs to be filled 1st
                metadataRow.PartitionKey = metadataRow.CalculateCombinedHash();
                mainRow.MetadataHash = metadataRow.PartitionKey; //link the main row to it's metadata

                return new { IsParsed = true, MainRow = mainRow, MetadataRow = metadataRow };

            }
            catch
            {
                //if fail return empty and fail
                return new { IsParsed = false, MainRow = IpAddressGeoLocationEntity.Empty, MetadataRow = IpAddressGeoLocationMetadataEntity.Empty };
            }
        }

    }

}
