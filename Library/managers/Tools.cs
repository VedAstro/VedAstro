using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using Microsoft.JSInterop;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using static System.Net.WebRequestMethods;
using static Genso.Astrology.Library.PlanetName;

namespace Genso.Astrology.Library
{
    /// <summary>
    /// A collection of general functions that don't have a home yet, so they live here for now.
    /// You're allowed to move them somewhere you see fit, not copy, move!
    /// </summary>
    public static class Tools
    {

        /// <summary>
        /// "H1N1" -> ["H", "1", "N", "1"]
        /// "H" -> ["H"]
        /// "GH1N12" -> ["GH", "1", "N", "12"]
        /// "OS234" -> ["OS", "234"]
        /// </summary>
        public static List<string> SplitAlpha(string input)
        {
            var words = new List<string> { string.Empty };
            for (var i = 0; i < input.Length; i++)
            {
                words[words.Count - 1] += input[i];
                if (i + 1 < input.Length && char.IsLetter(input[i]) != char.IsLetter(input[i + 1]))
                {
                    words.Add(string.Empty);
                }
            }
            return words;
        }
        /// <summary>
        /// Converts xml element instance to string properly
        /// </summary>
        public static string XmlToString(XElement xml)
        {
            //remove all formatting, for clean xml as string
            return xml.ToString(SaveOptions.DisableFormatting);
        }

        /// <summary>
        /// Gets XML file from any URL and parses it into xelement list
        /// </summary>
        public static async Task<List<XElement>> GetXmlFileHttp(string url)
        {
            //get the data sender
            using var client = new HttpClient();

            //load xml event data files before hand to be used quickly later for search
            //get main horoscope prediction file (located in wwwroot)
            var fileStream = await client.GetStreamAsync(url);

            //parse raw file to xml doc
            var document = XDocument.Load(fileStream);

            //get all records in document
            return document.Root.Elements().ToList();
        }


        /// <summary>
        /// Converts any type to XML, it will use Type's own ToXml() converter if available
        /// else ToString is called and placed inside element with Type's full name
        /// Note, used to transfer data via internet Client to API Server
        /// Example:
        /// <TypeName>
        ///     DataValue
        /// </TypeName>
        /// </summary>
        public static XElement AnyTypeToXml<T>(T value)
        {
            //check if type has own ToXml method
            //use the Type's own converter if available
            if (value is IToXml hasToXml)
            {
                var betterXml = hasToXml.ToXml();
                return betterXml;
            }

            //gets enum value as string to place inside XML
            //note: value can be null hence ?, fails quietly
            var enumValueStr = value?.ToString();

            //get the name of the Enum
            //Note: This is the name that will be used
            //later to instantiate the class from string
            var typeName = typeof(T).FullName;

            return new XElement(typeName, enumValueStr);
        }

        /// <summary>
        /// Converts any type that implements IToXml to XML, it will use Type's own ToXml() converter
        /// Note, used to transfer data via internet Client to API Server
        /// Placed inside "Root" xml
        /// Default name for root element is Root
        /// </summary>
        public static XElement AnyTypeToXmlList<T>(List<T> xmlList, string rootElementName = "Root") where T : IToXml
        {
            var rootXml = new XElement(rootElementName);
            foreach (var xmlItem in xmlList)
            {
                rootXml.Add(AnyTypeToXml(xmlItem));
            }
            return rootXml;
        }

        /// <summary>
        /// Simple override for XML, to skip parsing to type before sorting
        /// </summary>
        public static XElement AnyTypeToXmlList(List<XElement> xmlList, string rootElementName = "Root")
        {
            var rootXml = new XElement(rootElementName);
            foreach (var xmlItem in xmlList)
            {
                rootXml.Add(xmlItem);
            }
            return rootXml;
        }

        /// <summary>
        /// Given the URL of a standard VedAstro XML file, like "http://...PersonList.xml",
        /// will convert to the specified type and return in nice list, with time to be home for dinner
        /// </summary>
        public static async Task<List<T>> ConvertXmlListFileToInstanceList<T>(string httpUrl) where T : IToXml, new()
        {
            //get data list from Static Website storage
            //note : done so that any updates to that live file will be instantly reflected in API results
            var eventDataListXml = await Tools.GetXmlFileHttp(httpUrl);

            //parse each raw event data in list
            var eventDataList = new List<T>();
            foreach (var eventDataXml in eventDataListXml)
            {
                //add it to the return list
                var x = new T();
                eventDataList.Add(x.FromXml<T>(eventDataXml));
            }

            return eventDataList;

        }


        /// <summary>
        /// Converts given exception data to XML
        /// </summary>
        public static XElement ExceptionToXml(Exception e)
        {

            var responseMessage = new XElement("Exception");

            responseMessage.Add($"#Message#\n{e.Message}\n");
            responseMessage.Add($"#Data#\n{e.Data}\n");
            responseMessage.Add($"#InnerException#\n{e.InnerException}\n");
            responseMessage.Add($"#Source#\n{e.Source}\n");
            responseMessage.Add($"#Source#\n{e.Source}\n");
            responseMessage.Add($"#StackTrace#\n{e.StackTrace}\n");
            responseMessage.Add($"#StackTrace#\n{e.TargetSite}\n");

            return responseMessage;
        }

        /// <summary>
        /// - Type is a value typ
        /// - Enum
        /// </summary>
        public static dynamic XmlToAnyType<T>(XElement xml) // where T : //IToXml, new()
        {
            //get the name of the Enum
            var typeNameFullName = typeof(T).FullName;
            var typeNameShortName = typeof(T).FullName;

#if DEBUG
            Console.WriteLine(xml.ToString());
#endif

            //type name inside XML
            var xmlElementName = xml?.Name;

            //get the value for parsing later
            var rawVal = xml.Value;


            //make sure the XML enclosing type has the same name
            //check both full class name, and short class name
            var isSameName = xmlElementName == typeNameFullName || xmlElementName == typeof(T).GetShortTypeName();

            //if not same name raise error
            if (!isSameName)
            {
                throw new Exception($"Can't parse XML {xmlElementName} to {typeNameFullName}");
            }

            //implements ToXml()
            var typeImplementsToXml = typeof(T).GetInterfaces().Any(x =>
                x.IsGenericType &&
                x.GetGenericTypeDefinition() == typeof(IToXml));

            //type has owm ToXml method
            if (typeImplementsToXml)
            {
                dynamic inputTypeInstance = GetInstance(typeof(T).FullName);

                return inputTypeInstance.FromXml(xml);

            }

            //if type is an Enum process differently
            if (typeof(T).IsEnum)
            {
                var parsedEnum = (T)Enum.Parse(typeof(T), rawVal);

                return parsedEnum;
            }

            //else it is a value type
            if (typeof(T) == typeof(string))
            {
                return rawVal;
            }

            if (typeof(T) == typeof(double))
            {
                return Double.Parse(rawVal);
            }

            if (typeof(T) == typeof(int))
            {
                return int.Parse(rawVal);
            }

            //raise error since converter not implemented
            throw new NotImplementedException($"XML converter for {typeNameFullName}, not implemented!");
        }

        /// <summary>
        /// Gets only the name of the Class, without assembly
        /// </summary>
        public static string GetShortTypeName(this Type type)
        {
            var sb = new StringBuilder();
            var name = type.Name;
            if (!type.IsGenericType) return name;
            sb.Append(name.Substring(0, name.IndexOf('`')));
            sb.Append("<");
            sb.Append(string.Join(", ", type.GetGenericArguments()
                .Select(t => t.GetShortTypeName())));
            sb.Append(">");
            return sb.ToString();
        }

        public static bool Implements<I>(this Type type, I @interface) where I : class
        {
            if (((@interface as Type) == null) || !(@interface as Type).IsInterface)
                throw new ArgumentException("Only interfaces can be 'implemented'.");

            return (@interface as Type).IsAssignableFrom(type);
        }

        /// <summary>
        /// For converting value types, String, Double, etc.
        /// </summary>
        //public static dynamic XmlToValueType<T>(XElement xml) 
        //{
        //    //get the name of the Enum
        //    var typeName = nameof(T);


        //    //raise error since not XML type and Input type mismatch
        //    throw new Exception($"Can't parse XML to {typeName}");
        //}


        /// <summary>
        /// Gets an instance of Class from string name
        /// </summary>
        public static object GetInstance(string strFullyQualifiedName)
        {
            Type type = Type.GetType(strFullyQualifiedName);
            if (type != null)
                return Activator.CreateInstance(type);
            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                type = asm.GetType(strFullyQualifiedName);
                if (type != null)
                    return Activator.CreateInstance(type);
            }

            return null;
        }



        /// <summary>
        /// Converts days to hours
        /// </summary>
        /// <returns></returns>
        public static double DaysToHours(double days) => days * 24.0;

        public static double MinutesToHours(double minutes) => minutes / 60.0;

        public static double MinutesToYears(double minutes) => minutes / 525600.0;

        public static double MinutesToDays(double minutes) => minutes / 1440.0;

        /// <summary>
        /// Given a date it will count the days to the end of that year
        /// </summary>
        public static double GetDaysToNextYear(Time getBirthDateTime)
        {
            //get start of next year
            var standardTime = getBirthDateTime.GetStdDateTimeOffset();
            var nextYear = standardTime.Year + 1;
            var startOfNextYear = new DateTimeOffset(nextYear, 1, 1, 0, 0, 0, 0, standardTime.Offset);

            //calculate difference of days between 2 dates
            var diffDays = (startOfNextYear - standardTime).TotalDays;

            return diffDays;
        }

        /// <summary>
        /// Gets the time now in the system in text form
        /// formatted with standard style (HH:mm dd/MM/yyyy zzz) 
        /// </summary>
        public static string GetNowSystemTimeText() => DateTimeOffset.Now.ToString(Time.DateTimeFormat);
        /// <summary>
        /// Gets the time now in the system in text form with seconds (HH:mm:ss dd/MM/yyyy zzz) 
        /// </summary>
        public static string GetNowSystemTimeSecondsText() => DateTimeOffset.Now.ToString(Time.DateTimeFormatSeconds);

        /// <summary>
        /// Gets the time now in the Server (+8:00) in text form with seconds (HH:mm:ss dd/MM/yyyy zzz) 
        /// </summary>
        public static string GetNowServerTimeSecondsText() => DateTimeOffset.Now.ToOffset(TimeSpan.FromHours(8)).ToString(Time.DateTimeFormatSeconds);

        /// <summary>
        /// Custom hash generator for Strings. Returns consistent/deterministic values
        /// If null returns 0
        /// Note: MD5 (System.Security.Cryptography) not used because not supported in Blazor WASM
        /// </summary>
        public static int GetStringHashCode(string stringToHash)
        {
            if (stringToHash == null)
            {
                return 0;
            }

            unchecked
            {
                int hash1 = (5381 << 16) + 5381;
                int hash2 = hash1;

                for (int i = 0; i < stringToHash.Length; i += 2)
                {
                    hash1 = ((hash1 << 5) + hash1) ^ stringToHash[i];
                    if (i == stringToHash.Length - 1)
                        break;
                    hash2 = ((hash2 << 5) + hash2) ^ stringToHash[i + 1];
                }

                return hash1 + (hash2 * 1566083941);
            }


            //MD5 md5Hasher = MD5.Create();
            //var hashedByte = md5Hasher.ComputeHash(Encoding.UTF8.GetBytes(stringToHash));
            //return BitConverter.ToInt32(hashedByte, 0);

        }

        /// <summary>
        /// Gets random unique ID
        /// </summary>
        public static string GenerateId() => Guid.NewGuid().ToString("N");


        /// <summary>
        /// Converts any list to comma separated string
        /// Note: calls ToString();
        /// </summary>
        public static string ListToString<T>(List<T> list)
        {
            var combinedNames = "";
            foreach (var item in list)
            {
                combinedNames += item.ToString() + ", ";
            }

            return combinedNames;
        }


        //█▀▀ █░█ ▀▀█▀▀ █▀▀ █▀▀▄ █▀▀ ░▀░ █▀▀█ █▀▀▄ 　 █▀▄▀█ █▀▀ ▀▀█▀▀ █░░█ █▀▀█ █▀▀▄ █▀▀ 
        //█▀▀ ▄▀▄ ░░█░░ █▀▀ █░░█ ▀▀█ ▀█▀ █░░█ █░░█ 　 █░▀░█ █▀▀ ░░█░░ █▀▀█ █░░█ █░░█ ▀▀█ 
        //▀▀▀ ▀░▀ ░░▀░░ ▀▀▀ ▀░░▀ ▀▀▀ ▀▀▀ ▀▀▀▀ ▀░░▀ 　 ▀░░░▀ ▀▀▀ ░░▀░░ ▀░░▀ ▀▀▀▀ ▀▀▀░ ▀▀▀


        /// <summary>
        /// Find the first offset in the string that might contain the characters
        /// in `needle`, in any order. Returns -1 if not found.
        /// <para>This function can return false positives</para>
        /// </summary>
        public static bool FindCluster(this string haystack, string needle)
        {
            if (haystack == null) return false;
            if (needle == null) return false;

            if (haystack.Length < needle.Length) return false;

            long sum = needle.ToCharArray().Sum(c => c);
            long rolling = haystack.ToCharArray().Take(needle.Length).Sum(c => c);

            var idx = 0;
            var head = needle.Length;
            while (rolling != sum)
            {
                if (head >= haystack.Length) return false;
                rolling -= haystack[idx];
                rolling += haystack[head];
                head++;
                idx++;
            }

            return true;
        }

        /// <summary>
        /// Remap from 1 range to another
        /// </summary>
        public static float Remap(this float from, float fromMin, float fromMax, float toMin, float toMax)
        {
            var fromAbs = from - fromMin;
            var fromMaxAbs = fromMax - fromMin;

            var normal = fromAbs / fromMaxAbs;

            var toMaxAbs = toMax - toMin;
            var toAbs = toMaxAbs * normal;

            var to = toAbs + toMin;

            return to;
        }

        /// <summary>
        /// Remap from 1 range to another
        /// </summary>
        public static double Remap(this double from, double fromMin, double fromMax, double toMin, double toMax)
        {
            var fromAbs = from - fromMin;
            var fromMaxAbs = fromMax - fromMin;

            var normal = fromAbs / fromMaxAbs;

            var toMaxAbs = toMax - toMin;
            var toAbs = toMaxAbs * normal;

            var to = toAbs + toMin;

            return to;
        }


        public static string StreamToString(Stream stream)
        {
            StreamReader reader = new StreamReader(stream);
            string text = reader.ReadToEnd();

            return text;
        }


        /// <summary>
        /// Converts a timezone (+08:00) in string form to parsed timespan 
        /// </summary>
        public static TimeSpan StringToTimezone(string timezoneRaw)
        {
            return DateTimeOffset.ParseExact(timezoneRaw, "zzz", CultureInfo.InvariantCulture).Offset;
        }

        /// <summary>
        /// Returns system timezone offset as TimeSpan
        /// </summary>
        public static string GetSystemTimezoneStr() => DateTimeOffset.Now.ToString("zzz");

        /// <summary>
        /// Returns system timezone offset as TimeSpan
        /// </summary>
        public static TimeSpan GetSystemTimezone() => DateTimeOffset.Now.Offset;

        public static async Task<WebResult<GeoLocation>> AddressToGeoLocation(string address)
        {
            //create the request url for Google API
            var apiKey = "AIzaSyDqBWCqzU1BJenneravNabDUGIHotMBsgE";
            var url = $"https://maps.googleapis.com/maps/api/geocode/xml?key={apiKey}&address={Uri.EscapeDataString(address)}&sensor=false";

            //get location data from GoogleAPI
            var webResult = await ReadFromServerXmlReply(url);

            //if fail to make call, end here
            if (!webResult.IsPass) { return new WebResult<GeoLocation>(false, GeoLocation.Empty); }

            //if success, get the reply data out
            var geocodeResponseXml = webResult.Payload;
            var resultXml = geocodeResponseXml.Element("result");
            var statusXml = geocodeResponseXml.Element("status");

            //DEBUG
            //Console.WriteLine(geocodeResponseXml.ToString());

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

        /// <summary>
        /// gets the name of the place given th coordinates, uses Google API
        /// </summary>
        public static async Task<GeoLocation> CoordinateToGeoLocation(double longitude, double latitude, string apiKey)
        {
            //create the request url for Google API
            var url = string.Format($"https://maps.googleapis.com/maps/api/geocode/xml?latlng={latitude},{longitude}&key={apiKey}");

            //get location data from GoogleAPI
            var webResult = await ReadFromServerXmlReply(url);
            var rawReplyXml = webResult.Payload;

            //extract out the longitude & latitude
            var locationData = new XDocument(rawReplyXml);
            var localityResult = locationData.Element("GeocodeResponse")?.Elements("result").FirstOrDefault(result => result.Element("type")?.Value == "locality");
            var locationName = localityResult?.Element("formatted_address")?.Value;


            return new GeoLocation(locationName, longitude, latitude);

        }

        /// <summary>
        /// Given a location & time, will use Google Timezone API
        /// to get accurate time zone that was/is used
        /// Must input valid geo location 
        /// NOTE:
        /// - offset of timeAtLocation not important
        /// - googleGeoLocationApiKey needed to work
        /// </summary>
        public static async Task<TimeSpan> GetTimezoneOffset(string locationName, DateTimeOffset timeAtLocation, string apiKey)
        {
            //get geo location first then call underlying method
            var geoLocation = await GeoLocation.FromName(locationName);
            return Tools.StringToTimezone(await GetTimezoneOffsetApi(geoLocation, timeAtLocation, apiKey));
        }
        public static async Task<string> GetTimezoneOffsetString(string locationName, DateTime timeAtLocation, string apiKey)
        {
            //get geo location first then call underlying method
            var geoLocation = await GeoLocation.FromName(locationName);
            return await GetTimezoneOffsetApi(geoLocation, timeAtLocation, apiKey);
        }
        public static async Task<string> GetTimezoneOffsetString(string location, string dateTime)
        {
            //get timezone from Google API
            var lifeEvtTimeNoTimezone = DateTime.ParseExact(dateTime, Time.DateTimeFormatNoTimezone, null);
            var timezone = await Tools.GetTimezoneOffsetString(location, lifeEvtTimeNoTimezone, "AIzaSyDqBWCqzU1BJenneravNabDUGIHotMBsgE");

            return timezone;

            //get start time of life event and find the position of it in slices (same as now line)
            //so that this life event line can be placed exactly on the report where it happened
            //var lifeEvtTimeStr = $"{dateTime} {timezone}"; //add offset 0 only for parsing, not used by API to get timezone
            //var lifeEvtTime = DateTimeOffset.ParseExact(lifeEvtTimeStr, Time.DateTimeFormat, null);

            //return lifeEvtTime;
        }


        /// <summary>
        /// Given a location & time, will use Google Timezone API
        /// to get accurate time zone that was/is used, if Google fail,
        /// then auto default to system timezone
        /// NOTE:
        /// - sometimes unexpected failure to call google by some clients only
        /// - offset of timeAtLocation not important
        /// - googleGeoLocationApiKey needed to work
        /// </summary>
        public static async Task<WebResult<string>> GetTimezoneOffsetApi(GeoLocation geoLocation, DateTimeOffset timeAtLocation, string apiKey)
        {
            var returnResult = new WebResult<string>();

            //use timestamp to account for historic timezone changes
            var locationTimeUnix = timeAtLocation.ToUnixTimeSeconds();
            var longitude = geoLocation.GetLongitude();
            var latitude = geoLocation.GetLatitude();

            //create the request url for Google API 
            //todo get the API key string stored separately (for security reasons)
            var url = string.Format($@"https://maps.googleapis.com/maps/api/timezone/xml?location={latitude},{longitude}&timestamp={locationTimeUnix}&key={apiKey}");

            //get raw location data from GoogleAPI
            var apiResult = await ReadFromServerXmlReply(url);

            //if result from API is a failure then use system timezone
            //this is clearly an error, as such log it
            TimeSpan offsetMinutes;
            if (apiResult.IsPass) //all well
            {
                //get the raw data from google
                var timeZoneResponseXml = apiResult.Payload;

                //try parse Google API's payload
                var isParsed = TryParseGoogleTimeZoneResponse(timeZoneResponseXml, out offsetMinutes);
                if (!isParsed) { goto Fail; } //not parsed end here

                //convert to string exp: +08:00
                var parsedOffsetString = Tools.TimeSpanToUTCTimezoneString(offsetMinutes);

                //place data inside capsule
                returnResult.Payload = parsedOffsetString;
                returnResult.IsPass = true;
                return returnResult;
            }

        Fail:
            //mark as fail & use possibly inaccurate backup timezone (client browser's timezone)
            returnResult.IsPass = false;
            offsetMinutes = Tools.GetSystemTimezone();
            returnResult.Payload = Tools.TimeSpanToUTCTimezoneString(offsetMinutes);
            return returnResult;

        }

        /// <summary>
        /// Given a timespan instance converts to string timezone +08:00
        /// </summary>
        private static string TimeSpanToUTCTimezoneString(TimeSpan offsetMinutes)
        {
            var x = DateTimeOffset.UtcNow.ToOffset(offsetMinutes).ToString("zzz");
            return x;
        }

        /// <summary>
        /// When using google api to get timezone data, the API returns a reply in XML similar to one below
        /// This function parses this raw XML data from google to TimeSpan data we need
        /// It also checks for other failures like wrong location name
        /// Failing when parsing this TimeZoneResponse XML has occurred enough times, for its own method
        /// </summary>
        public static bool TryParseGoogleTimeZoneResponse(XElement timeZoneResponseXml, out TimeSpan offsetMinutes)
        {
            //<?xml version="1.0" encoding="UTF-8"?>
            //<TimeZoneResponse>
            //    <status>INVALID_REQUEST </ status >
            //    < error_message > Invalid request.Invalid 'location' parameter.</ error_message >
            //</ TimeZoneResponse >

            //extract out the data from google's reply timezone offset
            var status = timeZoneResponseXml?.Element("status")?.Value ?? "";
            var failed = status.Contains("INVALID_REQUEST");

            //try process data if did NOT fail so far
            if (!failed)
            {
                double offsetSeconds;

                //get raw data from XML
                var rawOffsetData = timeZoneResponseXml?.Element("raw_offset")?.Value;

                //at times google api returns no valid data, but call is replied as normal
                //so check for that here, if fail end here
                if (string.IsNullOrEmpty(rawOffsetData)) { goto Fail; }

                //try to parse what ever value there is, should be number
                else
                {
                    var isNumber = double.TryParse(rawOffsetData, out offsetSeconds);
                    if (!isNumber) { goto Fail; } //if not number end here
                }

                //offset needs to be "whole" minutes, else fail
                //purposely hard cast to int to remove not whole minutes
                var notWhole = TimeSpan.FromSeconds(offsetSeconds).TotalMinutes;
                offsetMinutes = TimeSpan.FromMinutes((int)Math.Round(notWhole)); //set

                //let caller know valid data
                return true;
            }

        //if fail let caller know something went wrong & set to 0s
        Fail:
            LibLogger.Error(timeZoneResponseXml);
            offsetMinutes = TimeSpan.Zero;
            return false;


        }

        /// <summary>
        /// Calls a URL and returns the content of the result as XML
        /// Even if content is returned as JSON, it is converted to XML
        /// Note:
        /// - if JSON auto adds "Root" as first element, unless specified
        /// for XML data root element name is ignored
        /// </summary>
        public static async Task<WebResult<XElement>> ReadFromServerXmlReply(string apiUrl, string rootElementName = "Root")
        {
            var returnResult = new WebResult<XElement>();
            string rawMessage = "";

            try
            {
                //send request to API server
                var result = await RequestServer(apiUrl);

                //parse data reply
                rawMessage = result.Content.ReadAsStringAsync().Result;

                //raw message can be JSON or XML
                //try parse as XML if fail then as JSON
                var readFromServerXmlReply = XElement.Parse(rawMessage);
                returnResult.Payload = readFromServerXmlReply;
                returnResult.IsPass = true; //pass

            }
            catch (Exception)
            {
                //try to parse data as JSON
                try
                {
                    var rawXml = JsonConvert.DeserializeXmlNode(rawMessage, rootElementName);
                    var readFromServerXmlReply = XElement.Parse(rawXml?.InnerXml ?? "<Empty/>");

                    returnResult.Payload = readFromServerXmlReply;
                    returnResult.IsPass = true; //pass

                }
                //unparseable data, let user know
                catch (Exception)
                {
                    //todo log it
                    var logData = $"ReadFromServerXmlReply()\n{rawMessage}";

                    returnResult.IsPass = false; //fail
                }
            }

            //send the prepared result caller
            return returnResult;


            //--------------------
            // FUNCTIONS

            async Task<HttpResponseMessage> RequestServer(string receiverAddress)
            {
                //prepare the data to be sent
                var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, receiverAddress);
                httpRequestMessage.SetBrowserRequestMode(BrowserRequestMode.NoCors); //NO CORS!

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
        /// Given a list of strings will return one by random
        /// Used to make dynamic user error & info messages
        /// </summary>
        public static string RandomSelect(string[] msgList)
        {
            // Create a Random object  
            Random rand = new Random();

            // Generate a random index less than the size of the array.  
            int randomIndexNumber = rand.Next(msgList.Length);

            //return random text from list to caller
            return msgList[randomIndexNumber];
        }

        /// <summary>
        /// Split string by character count
        /// </summary>
        public static IEnumerable<string> SplitByCharCount(string str, int maxChunkSize)
        {
            for (int i = 0; i < str.Length; i += maxChunkSize)
                yield return str.Substring(i, Math.Min(maxChunkSize, str.Length - i));
        }

        /// <summary>
        /// Inputed event name has be space separated
        /// </summary>
        public static List<PlanetName> GetPlanetFromName(string eventName)
        {
            var returnList = new List<PlanetName>();

            //lower case it
            var lowerCased = eventName.ToLower();

            //split into words
            var splited = lowerCased.Split(' ');

            //check if any be parsed into planet name
            foreach (var word in splited)
            {
                var result = PlanetName.TryParse(word, out var planetParsed);
                if (result)
                {
                    //add list if parsed
                    returnList.Add(planetParsed);
                }
            }


            //return list to caller
            return returnList;
        }

        /// <summary>
        /// Packages the data into ready form for the HTTP client to use in final sending stage
        /// </summary>
        public static StringContent XmLtoHttpContent(XElement data)
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

        /// <summary>
        /// Extracts data from an Exception puts it in a nice XML
        /// </summary>
        public static XElement ExtractDataFromException(Exception e)
        {
            //place to store the exception data
            string fileName;
            string methodName;
            int line;
            int columnNumber;
            string message;
            string source;

            //get the exception that started it all
            var originalException = e.GetBaseException();

            //extract the data from the error
            StackTrace st = new StackTrace(e, true);

            //Get the first stack frame
            StackFrame frame = st.GetFrame(st.FrameCount - 1);

            //Get the file name
            fileName = frame?.GetFileName();

            //Get the method name
            methodName = frame.GetMethod()?.Name;

            //Get the line number from the stack frame
            line = frame.GetFileLineNumber();

            //Get the column number
            columnNumber = frame.GetFileColumnNumber();

            message = originalException.ToString();

            source = originalException.Source;
            //todo include inner exception data
            var stackTrace = originalException.StackTrace;


            //put together the new error record
            var newRecord = new XElement("Error",
                new XElement("Message", message),
                new XElement("Source", source),
                new XElement("FileName", fileName),
                new XElement("SourceLineNumber", line),
                new XElement("SourceColNumber", columnNumber),
                new XElement("MethodName", methodName),
                new XElement("MethodName", methodName)
            );


            return newRecord;
        }

        /// <summary>
        /// Gets now time with seconds in wrapped in xml element
        /// used for logging
        /// </summary>
        public static XElement TimeStampSystemXml => new("TimeStamp", Tools.GetNowSystemTimeSecondsText());

        /// <summary>
        /// Gets now time at server location (+8:00) with seconds in wrapped in xml element
        /// used for logging
        /// </summary>
        public static XElement TimeStampServerXml => new("TimeStampServer", Tools.GetNowServerTimeSecondsText());

        /// <summary>
        /// Has to be loaded when app loads, obviously since that is when branch manifest it read
        /// since this is only used by loggers
        /// </summary>
        public static XElement BranchXml = new XElement("Branch", "not yet loaded, patience");

        /// <summary>
        /// Gets now time in UTC +8:00
        /// Because server time is uncertain, all change to UTC8
        /// </summary>
        public static string GetNow()
        {
            //create utc 8
            var utc8 = new TimeSpan(8, 0, 0);
            //get now time in utc 0
            var nowTime = DateTimeOffset.Now.ToUniversalTime();
            //convert time utc 0 to utc 8
            var utc8Time = nowTime.ToOffset(utc8);

            //return converted time to caller
            return utc8Time.ToString(Time.DateTimeFormatSeconds);
        }


        /// <summary>
        /// Removes all invalid characters for an person name
        /// used to clean name field user input
        /// allowed chars : periods (.) and hyphens (-), space ( )
        /// SRC:https://learn.microsoft.com/en-us/dotnet/standard/base-types/how-to-strip-invalid-characters-from-a-string
        /// </summary>
        public static string CleanNameText(string nameInput)
        {
            // Replace invalid characters with empty strings.
            try
            {
                var cleanText = Regex.Replace(nameInput, @"[^\w\.\s*-]", "", RegexOptions.None, TimeSpan.FromSeconds(2));
                return cleanText;
            }
            // If we timeout when replacing invalid characters,
            // we should return Empty.
            catch (RegexMatchTimeoutException)
            {
                return string.Empty;
            }
        }

    }

}
