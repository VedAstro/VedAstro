
using Newtonsoft.Json.Linq;
using SwissEphNet;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static VedAstro.Library.PlanetName;
using Exception = System.Exception;
using System.Text;
using ExCSS;
using ScottPlot.Drawing.Colormaps;
using Azure;
// Note: The Azure OpenAI client library for .NET is in preview.
// Install the .NET library via NuGet: dotnet add package Azure.AI.OpenAI --version 1.0.0-beta.5
using Azure;
using Azure.AI.OpenAI;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Web;
using System.IO;
using MimeDetective.Storage.Xml.v2;
using System.Net;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Azure.Storage.Blobs;
using System.Net.Mime;

namespace VedAstro.Library
{

    //█▀▄▀█ █▀▀ ▀▀█▀▀ █░░█ █▀▀█ █▀▀▄ 　 ▀▀█▀▀ █▀▀█ 　 ▀▀█▀▀ █░░█ █▀▀ 　 █▀▄▀█ █▀▀█ █▀▀▄ █▀▀▄ █▀▀ █▀▀ █▀▀ 
    //█░▀░█ █▀▀ ░░█░░ █▀▀█ █░░█ █░░█ 　 ░░█░░ █░░█ 　 ░░█░░ █▀▀█ █▀▀ 　 █░▀░█ █▄▄█ █░░█ █░░█ █▀▀ ▀▀█ ▀▀█ 
    //▀░░░▀ ▀▀▀ ░░▀░░ ▀░░▀ ▀▀▀▀ ▀▀▀░ 　 ░░▀░░ ▀▀▀▀ 　 ░░▀░░ ▀░░▀ ▀▀▀ 　 ▀░░░▀ ▀░░▀ ▀▀▀░ ▀░░▀ ▀▀▀ ▀▀▀ ▀▀▀ 

    //█▀▀█ █▀▀█ █▀▀▄ █▀▀ █▀▀█ 　 ▀▀█▀▀ █▀▀█ 　 ▀▀█▀▀ █░░█ █▀▀ 　 █▀▀ █░░█ █▀▀█ █▀▀█ █▀▀ 
    //█░░█ █▄▄▀ █░░█ █▀▀ █▄▄▀ 　 ░░█░░ █░░█ 　 ░░█░░ █▀▀█ █▀▀ 　 █░░ █▀▀█ █▄▄█ █░░█ ▀▀█ 
    //▀▀▀▀ ▀░▀▀ ▀▀▀░ ▀▀▀ ▀░▀▀ 　 ░░▀░░ ▀▀▀▀ 　 ░░▀░░ ▀░░▀ ▀▀▀ 　 ▀▀▀ ▀░░▀ ▀░░▀ ▀▀▀▀ ▀▀▀

    /// <summary>
    /// Collection of astronomical calculator functions
    /// Note : Many of the functions here use cacheing machanism
    /// </summary>
    public partial class Calculate
    {

        #region SETTINGS

        /// <summary>
        /// updated when calls come in, then when sub calls go out, then are this address is used
        /// </summary>
        public static string CurrentServerAddress;


        /// <summary>
        /// Defaults to RAMAN, but can be set before calling any funcs,
        /// NOTE: remember not to change mid instance, because "GetAyanamsa" & others are cached per instance
        /// </summary>
        public static int Ayanamsa { get; set; } = (int)Library.Ayanamsa.LAHIRI;

        /// <summary>
        /// Number of days in a year. Used for dasa related calculations.
        /// much debate on this number. Tests prove Raman's 360 is accurate.
        /// 365.25 is used by 3rd party astrology software like LoKPA
        /// default to 365.25 for the sake of DEAR CP JOIS 🙈
        /// 365.2564 True Sidereal Solar Year
        /// </summary>
        public static double SolarYearTimeSpan { get; set; } = 365.35;

        /// <summary>
        /// If set true, will not include gochara that was obstructed by "Vedhanka Point" calculation
        /// Enabled by default, recommend only disabled for research & debugging.
        /// Vedhanka needed for accuracy, recommended leave true
        /// </summary>
        public static bool UseVedhankaInGochara { get; set; } = true;

        /// <summary>
        /// Defaults to mean Rahu & Ketu positions for a more even value,
        /// set to false to use true node.
        /// Correlates to Swiss Ephemeris, SE_TRUE_NODE & SE_MEAN_NODE
        /// </summary>
        public static bool UseMeanRahuKetu { get; set; } = true;


        #endregion


        //----------------------------------------CORE CODE---------------------------------------------

        #region PERSON

        /// <summary>
        /// Add new person to DB,
        /// returns ID of newly created person so caller can get use it
        /// http://localhost:7071/api/Calculate/AddPerson/OwnerId/xxx/Location/Singapore/Time/00:00/24/06/2024/+08:00/PersonName/James%20Brown/Gender/Male/Notes/%7Brodden:%22AA%22%7D
        /// </summary>
        public static async Task<string> AddPerson(string ownerId, Time birthTime, string personName, Gender gender, string notes = "", bool failIfDuplicate = false)
        {
            //special ID made for human brains 🧠
            var brandNewHumanReadyId = await PersonManagerTools.GeneratePersonId(ownerId, personName, birthTime.StdYearText, failIfDuplicate);

            //make new person
            var newPerson = new Person(ownerId, brandNewHumanReadyId, personName, birthTime, gender, notes);

            //possible old cache of person with same id lived, so clear cache if any
            //delete data related to person (NOT USER, PERSON PROFILE)
            await AzureCache.DeleteCacheRelatedToPerson(newPerson);

            //creates record if no exist, update if already there
            AzureTable.PersonList.UpsertEntity(newPerson.ToAzureRow());

            //return ID of newly created person so caller can get use it
            return newPerson.Id;

        }

        public static async Task<string> UpdatePerson(string ownerId, string personId, Time birthTime, string personName, Gender gender, string notes = "")
        {
            //pack the data
            var personParsed = new Person(ownerId, personId, personName, birthTime, gender, notes);

            //delete data related to person (NOT USER, PERSON PROFILE)
            await AzureCache.DeleteCacheRelatedToPerson(personParsed);

            //person updated based on Person ID which is immutable
            await AzureTable.PersonList?.UpsertEntityAsync(personParsed.ToAzureRow());

            return "Updated!";

        }

        /// <summary>
        /// Deletes a person's record, uses hash to identify person
        /// Note : user id is not checked here because Person hash
        /// can't even be generated by client side if you don't have access.
        /// Theoretically anybody who gets the hash of the person,
        /// can delete the record by calling this API
        /// </summary>
        public static async Task<string> DeletePerson(string ownerId, string personId)
        {
            //# get full person copy to place in recycle bin
            //query the database
            var foundCalls = AzureTable.PersonList?.Query<PersonListEntity>(row => row.PartitionKey == ownerId && row.RowKey == personId);
            //make into readable format
            var personAzureRow = foundCalls?.FirstOrDefault();
            var personToDelete = Person.FromAzureRow(personAzureRow);

            //# delete data related to person (NOT USER, PERSON PROFILE)
            await AzureCache.DeleteCacheRelatedToPerson(personToDelete);

            //# add deleted person to recycle bin
            await AzureTable.PersonListRecycleBin.UpsertEntityAsync(personAzureRow);

            //# do final delete from MAIN DATABASE
            await AzureTable.PersonList.DeleteEntityAsync(ownerId, personId);

            return "Updated!";

        }

        /// <summary>
        /// Gets person list
        /// </summary>
        public static async Task<JArray> GetPersonList(string ownerId)
        {

            var foundCalls = AzureTable.PersonList.Query<PersonListEntity>(call => call.PartitionKey == ownerId);

            //add each to return list
            var personJsonList = new JArray();
            foreach (var call in foundCalls) { personJsonList.Add(Person.FromAzureRow(call).ToJson()); }

            //send to caller
            return personJsonList;

        }

        public static async Task<Person> GetPerson(string ownerId, string personId)
        {
            //get person from database matching user & owner ID (also checks shared list)
            var foundPerson = Tools.GetPersonById(personId, ownerId);

            //send person to caller
            return foundPerson;
        }

        /// <summary>
        /// Intelligible gets a person's image
        /// </summary>
        public static async Task<byte[]> GetPersonImage(string personId)
        {

            //start with backup person if all fails
            var personToImage = Person.Empty;
            BlobClient imageFile = null;

            try
            {
                //OPTION 1
                //check directly if custom uploaded image exist, if got end here
                var imageFound = await Tools.IsCustomPersonImageExist(personId);

                if (imageFound)
                {
                    imageFile = Tools.GetPersonImage(personId);
                    using var stream = new MemoryStream();
                    await imageFile.DownloadToAsync(stream);
                    stream.Position = 0;
                    var byteArray = stream.ToArray();
                    return byteArray;
                }

                //OPTION 2 : GET AZURE SEARCHED IMAGED
                else
                {
                    //get the person record by ID
                    personToImage = Tools.GetPersonById(personId);
                    byte[] foundImage = await Tools.GetSearchImage(personToImage); //gets most probable fitting person image

                    //save copy of image under profile, so future calls don't spend BING search quota
                    await Tools.SaveNewPersonImage(personToImage.Id, foundImage);

                    //return gotten image as is
                    return foundImage;

                }

            }

            //OPTION 3 : USE ANONYMOUS IMAGE
            //used only when bing and saved records fail
            catch (Exception e)
            {

                //get default male or female image
                imageFile = personToImage.Gender == Gender.Male ? Tools.GetPersonImage("male") : Tools.GetPersonImage("female");

                //save copy of image under profile, so future calls don't spend BING search quota
                await Tools.SaveNewPersonImage(personToImage.Id, imageFile);

                //send person image to caller
                using var stream = new MemoryStream();
                await imageFile.DownloadToAsync(stream);
                stream.Position = 0;
                var byteArray = stream.ToArray();
                return byteArray;
            }


        }


        #endregion

        #region MAINTAINANCE

        /// <summary>
        /// Special debug function
        /// </summary>
        public static string BouncBackInputPlanet(PlanetName planetName, Time time) => planetName.ToString();

        /// <summary>
        /// Basic bounce back data to confirm validity or ML table needs
        /// </summary>
        public static GeoLocation BouncBackInputGeoLocation(Time time) => time.GetGeoLocation();

        /// <summary>
        /// Basic bounce back data to confirm validity or ML table needs
        /// </summary>
        public static string BouncBackInputTime(Time time) => time.ToString();

        /// <summary>
        /// Returns list of all API calls for fun, why not
        /// </summary>
        /// <returns></returns>
        public static JArray List()
        {
            var allApiCalculatorsMethodInfo = Tools.GetAllApiCalculatorsMethodInfo();

            var returnList = new JArray();
            foreach (var openApiCalc in allApiCalculatorsMethodInfo)
            {
                //get special signature to find the correct description from list
                var signature = openApiCalc.GetMethodSignature();
                returnList.Add(signature);
            }

            return returnList;
        }

        #endregion

        #region GEO LOCATION


        /// <summary>
        /// Given an address will convert to it's geo location equivelant
        /// http://localhost:7071/api/Calculate/AddressToGeoLocation/Address/Gaithersburg
        /// </summary>
        /// <param name="address">can be any location name or coordinates like -3.9571599,103.8723379</param>
        /// <returns></returns>
        public static GeoLocation AddressToGeoLocation(string address)
        {
            //CACHE MECHANISM
            return CacheManager.GetCache<GeoLocation>(new CacheKey(nameof(AddressToGeoLocation), address), _AddressToGeoLocation);

            //UNDERLYING FUNCTION
            GeoLocation _AddressToGeoLocation()
            {
                //inject api key from parent
                var locationProvider = new Location();

                //do calculation using API and cache inteligently
                var returnVal = locationProvider.AddressToGeoLocation(address).Result;

                return returnVal;
            }
        }

        public static async Task<List<GeoLocation>> SearchLocation(string address)
        {
            //CACHE MECHANISM
            return await CacheManager.GetCache(new CacheKey(nameof(SearchLocation), address), async () => await _SearchLocation(address));

            //UNDERLYING FUNCTION
            async Task<List<GeoLocation>> _SearchLocation(string address)
            {

                //return all searches with less than 4 chars as pre name typing search
                if (address.Length < 4) { return new List<GeoLocation>(); }

                //inject api key from parent
                var locationProvider = new Location();

                //do calculation using API and cache inteligently
                var returnVal = await locationProvider.SearchAddressToGeoLocation(address);

                return returnVal;
            }

        }

        /// <summary>
        /// Given coordinates will convert to it's geo location equivelant
        /// http://localhost:7071/api/Calculate/CoordinatesToGeoLocation/Latitude/35.6764/Longitude/139.6500
        /// </summary>
        public static async Task<GeoLocation> CoordinatesToGeoLocation(string latitude, string longitude)
        {
            //CACHE MECHANISM
            return await CacheManager.GetCache(new CacheKey(nameof(CoordinatesToGeoLocation), latitude, longitude), async () => await _CoordinatesToGeoLocation(latitude, longitude));

            //UNDERLYING FUNCTION
            async Task<GeoLocation> _CoordinatesToGeoLocation(string latitude, string longitude)
            {
                //inject api key from parent
                var locationProvider = new Location();

                //do calculation using API and cache inteligently
                var returnVal = await locationProvider.CoordinatesToGeoLocation(latitude, longitude);

                return returnVal;
            }
        }

        /// <summary>
        /// ...../api/Calculate/GeoLocationToTimezone/Location/Chennai,TamilNadu,India/Time/23:37/07/08/1990/+01:00
        /// </summary>
        public static async Task<string> GeoLocationToTimezone(GeoLocation geoLocation, DateTimeOffset timeAtLocation)
        {
            //CACHE MECHANISM
            return await CacheManager.GetCache(new CacheKey(nameof(GeoLocationToTimezone), geoLocation, timeAtLocation), async () => await _GeoLocationToTimezone(geoLocation, timeAtLocation));

            //UNDERLYING FUNCTION
            async Task<string> _GeoLocationToTimezone(GeoLocation geoLocation, DateTimeOffset timeAtLocation)
            {
                //inject api key from parent
                var locationProvider = new Location();

                //do calculation using API and cache inteligently
                var returnVal = await locationProvider.GeoLocationToTimezone(geoLocation, timeAtLocation);

                return returnVal;
            }

        }

        /// <summary>
        /// ...../api/Calculate/IpAddressToGeoLocation/IpAddress/180.89.33.89
        /// </summary>
        public static async Task<GeoLocation> IpAddressToGeoLocation(string ipAddress)
        {
            //CACHE MECHANISM
            return await CacheManager.GetCache(new CacheKey(nameof(IpAddressToGeoLocation), ipAddress), async () => await _IpAddressToGeoLocation(ipAddress));

            //UNDERLYING FUNCTION
            async Task<GeoLocation> _IpAddressToGeoLocation(string ipAddress)
            {

                //inject api key from parent
                var locationProvider = new Location();

                //do calculation using API and cache inteligently
                var returnVal = await locationProvider.IpAddressToGeoLocation(ipAddress);

                return returnVal;
            }
        }



        #endregion

        #region EVENTS

        /// <summary>
        /// Gets all events occuring at given time. Basically a slice from "Events Chart"
        /// Can be used by LLM to interprate final prediction
        /// </summary>
        /// <param name="birthTime"></param>
        /// <param name="checkTime"></param>
        /// <param name="eventTagList"></param>
        public static List<Event> EventsAtTime(Time birthTime, Time checkTime, List<EventTag> eventTagList)
        {
            // TEMP hack to place time in Person (wrapped) 
            var johnDoe = new Person("", birthTime, Gender.Empty);

            var xx = EventManager.CalculateEvents(1, checkTime, checkTime, johnDoe, eventTagList);

            return xx;
        }

        /// <summary>
        /// Given a birth time, current time and event name, gets the event data occuring at current time
        /// Easy way to check if Gochara is occuring at given time, with start and end time calculated
        /// Precision hard set to 1 hour TODO
        /// </summary>
        /// <param name="birthTime">birth time of native</param>
        /// <param name="checkTime">time to base calculation on</param>
        public static Event EventStartEndTime(Time birthTime, Time checkTime, EventName nameOfEvent)
        {
            //from event name, get full event data
            EventData eventData = EventDataListStatic.Rows.Where(x => x.Name == nameOfEvent).FirstOrDefault();

            //TODO should be changeable for fine events
            var precisionInHours = 1;

            //check if event is occuring
            //NOTE: hack to enter birth time with existing code
            var birthTimeWrapped = new Person("", birthTime, Gender.Male);
            var isOccuringAtCheckTime = EventManager.ConvertToEventSlice(checkTime, eventData, birthTimeWrapped)?.IsOccuring ?? false; //not found default to false

            //if occuring, start scanning for start & end times
            if (isOccuringAtCheckTime)
            {
                //scan for start time given event data
                var eventStartTime = Calculate.EventStartTime(birthTime, checkTime, eventData, precisionInHours);

                //scan for end time given event data
                var eventEndTime = Calculate.EventEndTime(birthTime, checkTime, eventData, precisionInHours);

                //TODO: temp
                var tags = EventManager.GetTagsByEventName(eventData.Name);
                var finalEvent = new Event(eventData.Name, eventData.Nature, eventData.Description, eventData.SpecializedSummary, eventStartTime, eventEndTime, tags);

                return finalEvent;
            }

            //if not occuring, let user know with empty event
            else { return Event.Empty; }

        }

        public static Time EventStartTime(Time birthTime, Time checkTime, EventData eventData, int precisionInHours)
        {
            //NOTE: hack to enter birth time with existing code
            var birthTimeWrapped = new Person("", birthTime, Gender.Male);

            //check time will be used as possible start time
            var possibleStartTime = checkTime;
            var previousPossibleStartTime = possibleStartTime;

            //start as not found
            var isFound = false;
            while (!isFound) //run while not found
            {
                //check possible start time if event occurring (heavy computation)
                var updatedEventData = EventManager.ConvertToEventSlice(possibleStartTime, eventData, birthTimeWrapped);

                //if occuring than continue to next, start time not found
                if (updatedEventData != null && updatedEventData.IsOccuring)
                {
                    //save a copy of possible time, to be used when we go too far
                    previousPossibleStartTime = possibleStartTime;

                    //decrement entered time, to check next possible start time in the past
                    possibleStartTime = possibleStartTime.SubtractHours(precisionInHours);
                }
                //start time found!, event has stopped occuring (too far)
                else
                {
                    //return possible start time as confirmed!
                    possibleStartTime = previousPossibleStartTime;
                    isFound = true; //stop looking
                }
            }

            //if control reaches here than start time found
            return possibleStartTime;

        }

        public static Time EventEndTime(Time birthTime, Time checkTime, EventData eventData, int precisionInHours)
        {
            //NOTE: hack to enter birth time with existing code
            var birthTimeWrapped = new Person("", birthTime, Gender.Male);

            //check time will be used as possible end time
            var possibleEndTime = checkTime;
            var previousPossibleEndTime = possibleEndTime;

            //end as not found
            var isFound = false;
            while (!isFound) //run while not found
            {
                //check possible end time if event occurring (heavy computation)
                var updatedEventData = EventManager.ConvertToEventSlice(possibleEndTime, eventData, birthTimeWrapped);

                //if occuring than continue to next, end time not found
                if (updatedEventData != null && updatedEventData.IsOccuring)
                {
                    //save a copy of possible time, to be used when we go too far
                    previousPossibleEndTime = possibleEndTime;

                    //increment possible end time, to check next possible end time in the future
                    possibleEndTime = possibleEndTime.AddHours(precisionInHours);
                }
                //end time found!, event has stopped occuring (too far)
                else
                {
                    //return possible end time as confirmed!
                    possibleEndTime = previousPossibleEndTime;
                    isFound = true; //stop looking
                }
            }

            //if control reaches here than end time found
            return possibleEndTime;

        }


        #endregion

        #region MATCH CHECK KUTA SCORE

        /// <summary>
        /// Get full kuta match data for 2 horoscopes
        /// </summary>
        public static MatchReport CompatibilityMatch(Time maleBirthTime, Time femaleBirthTime)
        {
            //get 1st and 2nd only for now (todo support more)
            var male = new Person("", maleBirthTime, Gender.Male);
            var female = new Person("", femaleBirthTime, Gender.Female);

            //generate compatibility report
            var compatibilityReport = MatchReportFactory.GetNewMatchReport(male, female, "101");


            return compatibilityReport;
        }

        public static async Task<string> BirthTimeLocationAutoAIFill(string personFullName)
        {
            //get birth time as compatible text
            var birthTime = await BirthTimeAutoAIFill(personFullName);

            //get birth location as compatible text, without comma
            var birthLocation = await BirthLocationAutoAIFill(personFullName);

            //get birth location as compatible text, without comma
            var marriagePartnerName = await MarriagePartnerNameAutoAIFill(personFullName);

            //get partner data
            var marriagePartnerBirthTime = await BirthTimeAutoAIFill(marriagePartnerName);
            var marriagePartnerBirthLocation = await BirthLocationAutoAIFill(marriagePartnerName);

            //get marriage data
            var marriageTags = await MarriageTagsAutoAIFill(personFullName, marriagePartnerName);

            return $"{marriageTags},{personFullName},{birthTime},{birthLocation},{marriagePartnerName},{marriagePartnerBirthTime},{marriagePartnerBirthLocation}";
        }

        /// <summary>
        /// Given a famous person name will auto find birth time using LLM AI
        /// </summary>
        public static async Task<string> BirthTimeAutoAIFill(string personFullName)
        {
            string anyScaleAPIKey = Environment.GetEnvironmentVariable("AnyScaleAPIKey");

            using (var client = new HttpClient())
            {
                var requestBodyObject = new
                {
                    model = "meta-llama/Meta-Llama-3-70B-Instruct",
                    messages = new List<object>
                    {
                        new { role = "system", content = "given person name output birth time as {HH:mm DD/MM/YYYY zzz}" },
                        new { role = "user", content = "Monroe, Marilyn" },
                        new { role = "assistant", content = "09:30 01/06/1926 -08:00" },
                        new { role = "user", content = personFullName }
                    }
                };

                string requestBody = JsonConvert.SerializeObject(requestBodyObject);

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", anyScaleAPIKey);
                client.BaseAddress = new Uri("https://api.endpoints.anyscale.com/v1/chat/completions");

                var content = new StringContent(requestBody);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                HttpResponseMessage response = await client.PostAsync("", content);

                string responseContent = await response.Content.ReadAsStringAsync();
                var fullResponse = JObject.Parse(responseContent);
                var message = fullResponse["choices"][0]["message"]["content"].Value<string>();

                return message;
            }
        }

        public static async Task<string> MarriageTagsAutoAIFill(string personA, string personB)
        {
            string anyScaleAPIKey = Environment.GetEnvironmentVariable("AnyScaleAPIKey");

            using (var client = new HttpClient())
            {
                var requestBodyObject = new
                {
                    model = "meta-llama/Meta-Llama-3-70B-Instruct",
                    messages = new List<object>
                    {
                        new { role = "system", content = "given married couple name output marriage duration in years" },
                        new { role = "user", content = "Brad Pitt & Angelina Jolie" },
                        new { role = "assistant", content = "#2Years" },
                        new { role = "user", content = "Napoleon Bonaparte & Joséphine" },
                        new { role = "assistant", content = "#14Years" },
                        new { role = "user", content = "Dax Shepard & Kristen Bell" },
                        new { role = "assistant", content = "#StillMarried" },
                        new { role = "user", content = $"{personA} & {personB}" }
                    }
                };

                string requestBody = JsonConvert.SerializeObject(requestBodyObject);

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", anyScaleAPIKey);
                client.BaseAddress = new Uri("https://api.endpoints.anyscale.com/v1/chat/completions");

                var content = new StringContent(requestBody);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                HttpResponseMessage response = await client.PostAsync("", content);

                string responseContent = await response.Content.ReadAsStringAsync();
                var fullResponse = JObject.Parse(responseContent);
                var message = fullResponse["choices"][0]["message"]["content"].Value<string>();

                return message;
            }
        }

        /// <summary>
        /// Given a famous person name will auto find birth location using LLM AI
        /// </summary>
        public static async Task<string> BirthLocationAutoAIFill(string personFullName)
        {
            string anyScaleAPIKey = Environment.GetEnvironmentVariable("AnyScaleAPIKey");

            using (var client = new HttpClient())
            {
                var requestBodyObject = new
                {
                    model = "meta-llama/Meta-Llama-3-70B-Instruct",
                    messages = new List<object>
                    {
                        new { role = "system", content = "given person name output birth location as {city state country}" },
                        new { role = "user", content = "Monroe, Marilyn" },
                        new { role = "assistant", content = "Los Angeles California USA" },
                        new { role = "user", content = personFullName }
                    }
                };

                string requestBody = JsonConvert.SerializeObject(requestBodyObject);

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", anyScaleAPIKey);
                client.BaseAddress = new Uri("https://api.endpoints.anyscale.com/v1/chat/completions");

                var content = new StringContent(requestBody);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                HttpResponseMessage response = await client.PostAsync("", content);

                string responseContent = await response.Content.ReadAsStringAsync();
                var fullResponse = JObject.Parse(responseContent);
                var message = fullResponse["choices"][0]["message"]["content"].Value<string>(); ;

                return message;
            }
        }

        /// <summary>
        /// Given a famous person name will auto find marriage partner using LLM AI
        /// </summary>
        public static async Task<string> MarriagePartnerNameAutoAIFill(string personFullName)
        {
            string anyScaleAPIKey = Environment.GetEnvironmentVariable("AnyScaleAPIKey");

            using (var client = new HttpClient())
            {
                var requestBodyObject = new
                {
                    model = "meta-llama/Meta-Llama-3-70B-Instruct",
                    messages = new List<object>
                    {
                        new { role = "system", content = "given person name output first marriage partner name" },
                        new { role = "user", content = "Monroe, Marilyn" },
                        new { role = "assistant", content = "James Dougherty" },
                        new { role = "user", content = personFullName }
                    }
                };

                string requestBody = JsonConvert.SerializeObject(requestBodyObject);

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", anyScaleAPIKey);
                client.BaseAddress = new Uri("https://api.endpoints.anyscale.com/v1/chat/completions");

                var content = new StringContent(requestBody);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                HttpResponseMessage response = await client.PostAsync("", content);

                string responseContent = await response.Content.ReadAsStringAsync();
                var fullResponse = JObject.Parse(responseContent);
                var message = fullResponse["choices"][0]["message"]["content"].Value<string>(); ;

                return message;
            }
        }

        public class Movie
        {
            public string Title { get; set; }
            public string Director { get; set; }
            // Add other properties as needed
        }

        public class SearchResults
        {
            public Movie[] Value { get; set; }
        }

        public static async Task<JObject> GetData(string searchQuery)
        {
            string subscriptionKey = Secrets.Get("BING_IMAGE_SEARCH");

            //string searchQuery = $"movies directed by {director}";
            string bingAPIEndpoint = "https://api.bing.microsoft.com/v7.0/entities";

            using (var client = new HttpClient())
            {
                var uri = new Uri($"{bingAPIEndpoint}?q={Uri.EscapeDataString(searchQuery)}&count=100&mkt=en-US");
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);

                var response = await client.GetAsync(uri);

                string jsonResponse = await response.Content.ReadAsStringAsync();

                JObject jobj = JObject.Parse(jsonResponse);

                return jobj;
            }


            throw new NotImplementedException();
        }


        #endregion

        #region CHAT API & MACHINE LEARNING

        /// <summary>
        /// Ask questions to AI astrologer about life horoscope predictions
        /// </summary>
        /// <param name="birthTime">time of person hprtson horoscope to check</param>
        /// <param name="userQuestion">question related horoscope</param>
        /// <param name="chatSession"></param>
        /// <returns></returns>
        public static async Task<JObject> HoroscopeChat(Time birthTime, string userQuestion, string userId, string sessionId = "")
        {
            return await ChatAPI.SendMessageHoroscope(birthTime, userQuestion, sessionId, userId);
        }

        public static async Task HoroscopeChat2(Time birthTime, string userQuestion, string userId, string sessionId = "")
        {
            //await ChatAPI.CreatePresetQuestionEmbeddings_CohereEmbed();

            await ChatAPI.LLMSearchAPICall_CohereEmbed(userQuestion);
            //var foundQuestions = await ChatAPI.FindPresetQuestionEmbeddings_CohereEmbed(userQuestion);

            //throw new NotImplementedException();
            //return foundQuestions.Select(jv => jv.ToJson()).ToList();



            //return await ChatAPI.SendMessageHoroscope(birthTime, userQuestion, sessionId, userId);

        }

        public static async Task<JObject> HoroscopeChatFeedback(string answerHash, int feedbackScore)
        {
            return await ChatAPI.HoroscopeChatFeedback(answerHash, feedbackScore);
        }

        public static async Task<JObject> HoroscopeFollowUpChat(Time birthTime, string followUpQuestion, string primaryAnswerHash, string userId,
            string sessionId)
        {
            return await ChatAPI.SendMessageHoroscopeFollowUp(birthTime, followUpQuestion, primaryAnswerHash, userId, sessionId);
        }

        /// <summary>
        /// Ask questions to AI astrologer about life horoscope predictions
        /// </summary>
        /// <param name="birthTime">time of person hprtson horoscope to check</param>
        /// <param name="userQuestion">question related horoscope</param>
        /// <param name="chatSession"></param>
        /// <returns></returns>
        public static async Task<JObject> MatchChat(Time maleBirthTime, Time femaleBirthTime, string userQuestion, string chatSession = "")
        {
            return await ChatAPI.SendMessageMatch(maleBirthTime, femaleBirthTime, userQuestion, chatSession);
        }

        /// <summary>
        /// Searches all horoscopes predictions with LLM
        /// </summary>
        public static async Task<List<HoroscopePrediction>> HoroscopeLLMSearch(Time birthTime, string textInput)
        {
            //make http call to python api server
            var timeUrl = birthTime.ToUrl();
            var callUrl = $"https://vedastrocontainer.delightfulground-a2445e4b.westus2.azurecontainerapps.io/HoroscopeLLMSearch";
            var jsonString = $@"{{""query"":""{textInput}"",
                                ""birth_time"":""{timeUrl}"",
                                ""llm_model_name"":""sentence-transformers/all-MiniLM-L6-v2"",
                                ""search_type"" : ""similarity""
                            }}";

            //result is an array of found
            var rawReply = await Tools.MakePostRequest(callUrl, jsonString);

            //convert to nice nice format
            var finalList = new List<HoroscopePrediction>();
            foreach (var eachPrediction in rawReply)
            {
                finalList.Add(HoroscopePrediction.FromLLMJson(eachPrediction));
            }

            return finalList;
        }

        /// <summary>
        /// Given a start time & end time and space in hours between.
        /// Will generate massive CSV tables for ML & Data Science
        /// Will contain 3 columns, "Name","Time","Location"
        /// this can then be fed into ML Table Generator to make datasets worthy of HuggingFace
        /// </summary>
        public static string GenerateTimeListCSV(Time startTime, Time endTime, double hoursBetween)
        {
            //make slices to fill list
            var timeSlices = Time.GetTimeListFromRange(startTime, endTime, hoursBetween);

            //generate CSV string from above time slices
            StringBuilder csv = new StringBuilder();
            csv.AppendLine("Name,Time,Location");

            for (var index = 0; index < timeSlices.Count; index++)
            {
                var time = timeSlices[index];
                var locationNameCSVSafe = time.GetGeoLocation().Name().Replace(",", ""); //remove comma, since CSV reserved
                csv.AppendLine($"row{index},{time.GetStdDateTimeOffsetText()},{locationNameCSVSafe}");
            }

            return csv.ToString();
        }


        #endregion

        //the introduction of certain sensitive points
        // or sahams signifying events is a novel aspect ofTajaka. 
        //There are special
        // positions or points signifying important events in life.
        // While a Bhava or house comprehends a number of
        // events, a Saham or sensitive point relates to only one
        // particular event. 

        #region SAHAMS

        //public static Angle PunyaSahamLongitude(Time birthTime)
        //{
        //    //# 1 Punya Fortune/good deeds Moon – Sun + Lagna
        //    var moonLong = Calculate.PlanetNirayanaLongitude(Moon, birthTime).TotalDegrees; //A
        //    var sunLong = Calculate.PlanetNirayanaLongitude(Sun, birthTime).TotalDegrees; //B
        //    var lagnaLong = Calculate.HouseLongitude(HouseName.House1, birthTime).GetMiddleLongitude().TotalDegrees; //C

        //    //Each saham has a formula that looks like A - B + C. What
        //    //this means is that we take the longitudes of A, B and C and find (A - B + C).
        //    //This is equivalent to finding how far A is from B and then taking the same distance from C.
        //    var punyaSahamLong = (moonLong - sunLong) + lagnaLong;

        //    //if night birth
        //    var isNightBirth = Calculate.IsNightBirth(birthTime);
        //    if (isNightBirth) { punyaSahamLong = (sunLong - moonLong) + lagnaLong; }

        //    // However, if C is not between B and A (i.e. we start
        //    // from B and go zodiacal till we meet A, and we do not find C on the way),
        //    // then we add 30° to the value evaluated above.
        //    if (!IsSahamCBetweenBToA(moonLong, sunLong, lagnaLong)) { punyaSahamLong += 30; }

        //    //expunge 360 degrees
        //    var final = Angle.FromDegrees(punyaSahamLong).Expunge360();

        //    return final;
        //}

        public static Angle PunyaSahamLongitude(Time birthTime)
        {
            //STEP 1:
            //in order to find the Punya Saham deduct
            //the Sun's longitude from the Moon's (if the year
            //commences during daytime or vice versa if during the
            //night) and add the ascendant
            var moonLong = Calculate.PlanetNirayanaLongitude(Moon, birthTime).TotalDegrees; //A
            var sunLong = Calculate.PlanetNirayanaLongitude(Sun, birthTime).TotalDegrees; //B
            var tajakaBirth = Calculate.TajikaDateForYear(birthTime, 205);

            return Angle.Degrees180;

        }

        public static Angle VidyaSahamLongitude(Time standardHoroscope)
        {
            throw new NotImplementedException();
        }

        public static Angle YasasSahamLongitude(Time standardHoroscope)
        {
            throw new NotImplementedException();
        }

        public static Angle MitraSahamLongitude(Time standardHoroscope)
        {
            throw new NotImplementedException();
        }

        public static Angle MahatmyaSahamLongitude(Time standardHoroscope)
        {
            throw new NotImplementedException();
        }

        public static Angle AshaSahamLongitude(Time standardHoroscope)
        {
            throw new NotImplementedException();
        }

        public static Angle SamarthaSahamLongitude(Time standardHoroscope)
        {
            throw new NotImplementedException();
        }

        public static Angle BhratruSahamLongitude(Time standardHoroscope)
        {
            throw new NotImplementedException();
        }

        public static Angle GauravaSahamLongitude(Time standardHoroscope)
        {
            throw new NotImplementedException();
        }

        public static Angle PitruSahamLongitude(Time standardHoroscope)
        {
            throw new NotImplementedException();
        }

        public static Angle RajyaSahamLongitude(Time standardHoroscope)
        {
            throw new NotImplementedException();
        }

        public static Angle MatruSahamLongitude(Time standardHoroscope)
        {
            throw new NotImplementedException();
        }

        public static Angle PutraSahamLongitude(Time standardHoroscope)
        {
            throw new NotImplementedException();
        }

        public static Angle JeevastambaSahamLongitude(Time standardHoroscope)
        {
            throw new NotImplementedException();
        }

        public static Angle KarmaSahamLongitude(Time standardHoroscope)
        {
            throw new NotImplementedException();
        }

        public static Angle RogaSahamLongitude(Time standardHoroscope)
        {
            throw new NotImplementedException();
        }

        public static Angle KalaSahamLongitude(Time standardHoroscope)
        {
            throw new NotImplementedException();
        }

        public static Angle ShashtrasthanaSahamLongitude(Time standardHoroscope)
        {
            throw new NotImplementedException();
        }

        public static Angle BandhuSahamLongitude(Time standardHoroscope)
        {
            throw new NotImplementedException();
        }

        public static Angle MrityuSahamLongitude(Time standardHoroscope)
        {
            throw new NotImplementedException();
        }

        public static Angle ParadeshSahamLongitude(Time standardHoroscope)
        {
            throw new NotImplementedException();
        }

        public static Angle ArthastambaSahamLongitude(Time standardHoroscope)
        {
            throw new NotImplementedException();
        }

        public static Angle ParameshthisthanaSahamLongitude(Time standardHoroscope)
        {
            throw new NotImplementedException();
        }

        public static Angle VanijSahamLongitude(Time standardHoroscope)
        {
            throw new NotImplementedException();
        }

        public static Angle KaryasiddhiSahamLongitude(Time standardHoroscope)
        {
            throw new NotImplementedException();
        }

        public static Angle VivahaSahamLongitude(Time standardHoroscope)
        {
            throw new NotImplementedException();
        }

        public static Angle SanthanasthanaSahamLongitude(Time standardHoroscope)
        {
            throw new NotImplementedException();
        }

        public static Angle SradhdhasthanamSahamLongitude(Time standardHoroscope)
        {
            throw new NotImplementedException();
        }

        public static Angle PreethistambhaSahamLongitude(Time standardHoroscope)
        {
            throw new NotImplementedException();
        }

        public static Angle JadysthanamSahamLongitude(Time standardHoroscope)
        {
            throw new NotImplementedException();
        }

        public static Angle VyanjansthanaSahamLongitude(Time standardHoroscope)
        {
            throw new NotImplementedException();
        }

        public static Angle SathruSahamLongitude(Time standardHoroscope)
        {
            throw new NotImplementedException();
        }

        public static Angle JaladoshamSahamLongitude(Time standardHoroscope)
        {
            throw new NotImplementedException();
        }

        public static Angle BandhanasthanaSahamLongitude(Time standardHoroscope)
        {
            throw new NotImplementedException();
        }

        public static Angle ApamrithyusthanamSahamLongitude(Time standardHoroscope)
        {
            throw new NotImplementedException();
        }

        public static Angle LabhesthanamSahamLongitude(Time standardHoroscope)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This function checks if the astrological sign (Rasi) of a point C is between
        /// the astrological signs of points B and A.
        /// The astrological sign is calculated by dividing the longitude by 30.
        /// The function returns true if C’s Rasi is between B’s and A’s Rasi, and false otherwise.
        /// </summary>
        private static bool IsSahamCBetweenBToA(double aLong, double bLong, double cLong)
        {
            // Convert the longitude values to Rasi (Hindu Astrological Sign) by dividing by 30
            int aRasi = (int)(aLong / 30);
            int bRasi = (int)(bLong / 30);
            int cRasi = (int)(cLong / 30);

            // Initialize a flag to check if C's Rasi is found between B's and A's Rasi
            bool cRasiFound = false;

            // Loop from B's Rasi to B's Rasi + 11 (covering all 12 Rasis in a circular manner)
            for (int n = bRasi; n < bRasi + 11; n++)
            {
                // Calculate the next Rasi in a circular manner (0 to 11)
                int nextN = (n + 1) % 12;

                // If the next Rasi is C's Rasi, set the flag to True and break the loop
                if (nextN == cRasi)
                {
                    cRasiFound = true;
                    break;
                }
                // If the next Rasi is A's Rasi before finding C's Rasi, break the loop
                else if (nextN == aRasi)
                {
                    break;
                }
            }

            // Return the flag which indicates if C's Rasi is between B's and A's Rasi
            return cRasiFound;
        }


        #endregion

        #region VARGAS OR SUBTLE DIVISIONS

        /// <summary>
        /// Calculates the divisional longitude of a planet in a D-chart (divisional chart) in Vedic Astrology.
        /// written by AI & Human 
        /// </summary>
        /// <param name="planetName">The name of the planet.</param>
        /// <param name="inputTime">The time for which the calculation is to be made.</param>
        /// <param name="divisionalNo">The number of the D-chart (e.g., 2, 3, 4, 7, 9, etc.).</param>
        /// <returns>The divisional longitude of the planet in the specified D-chart.</returns>
        public static Angle PlanetDivisionalLongitude(PlanetName planetName, Time inputTime, int divisionalNo)
        {
            // Step 1: Get the Nirayana (sidereal) longitude of the planet at the given time.
            var planet_degrees = Calculate.PlanetNirayanaLongitude(planetName, inputTime).TotalDegrees;

            // Multiply the planet's longitude by the D-chart number to get the raw divisional longitude.
            var total_degrees = planet_degrees * divisionalNo;

            // Step 2: Normalize the raw divisional longitude to the range [0, 60) degrees.
            // This is done by subtracting 60 (the number of degrees in a zodiac sign) until the result is less than 60.
            while (total_degrees >= 60)
            {
                total_degrees -= 60;
            }

            // The remaining value is the longitude of the planet in the D-chart.
            var finalLong = Angle.FromDegrees(total_degrees);
            return finalLong;
        }

        public static Angle DivisionalLongitude(double totalDegrees, int divisionalNo)
        {
            // Step 1: Get the Nirayana (sidereal) longitude of the planet at the given time.
            //var planet_degrees = Calculate.PlanetNirayanaLongitude(planetName, inputTime).TotalDegrees;

            // Multiply the planet's longitude by the D-chart number to get the raw divisional longitude.
            var total_degrees = totalDegrees * divisionalNo;

            // Step 2: Normalize the raw divisional longitude to the range [0, 60) degrees.
            // This is done by subtracting 60 (the number of degrees in a zodiac sign) until the result is less than 60.
            while (total_degrees >= 60)
            {
                total_degrees -= 60;
            }

            // The remaining value is the longitude of the planet in the D-chart.
            var finalLong = Angle.FromDegrees(total_degrees);
            return finalLong;
        }


        //------------ D1 ------------
        /// <summary>
        /// Get zodiac sign planet is in.
        /// D1
        /// </summary>
        public static ZodiacSign PlanetZodiacSign(PlanetName planetName, Time time)
        {
            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey(nameof(PlanetZodiacSign), planetName, time, Ayanamsa), _planetZodiacSign);

            //UNDERLYING FUNCTION
            ZodiacSign _planetZodiacSign()
            {
                //get longitude of planet
                var longitudeOfPlanet = PlanetNirayanaLongitude(planetName, time);

                //get sign planet is in
                var signPlanetIsIn = ZodiacSignAtLongitude(longitudeOfPlanet);

                //return
                return signPlanetIsIn;

            }

        }


        //------------ D2 ------------

        /// <summary>
        /// Gets hora zodiac sign of a planet
        /// D2
        /// </summary>
        public static ZodiacSign PlanetHoraSigns(PlanetName planetName, Time time) => Calculate.DrekkanaSignName(Calculate.PlanetZodiacSign(planetName, time));

        /// <summary>
        /// D2 chart
        /// </summary>
        public static ZodiacSign HoraSignName(ZodiacSign zodiacSign) => Vargas.VargasCoreCalculator(zodiacSign, Vargas.HoraTable[zodiacSign.GetSignName()], 2);

        /// <summary>
        /// given a longitude will return hora sign at that
        /// </summary>
        public static ZodiacSign HoraSignAtLongitude(Angle longitude) => HoraSignName(ZodiacSignAtLongitude(longitude));

        /// <summary>
        /// Gets the zodiac sign at middle longitude of the house with degrees data
        /// </summary>
        public static ZodiacSign HouseHoraSign(HouseName houseNumber, Time time)
        {
            //get all houses
            var allHouses = AllHouseMiddleLongitudes(time);

            //get the house specified 
            var specifiedHouse = allHouses.Find(house => house.GetHouseName() == houseNumber);

            //get sign of the specified house
            var middleLongitude = specifiedHouse.GetMiddleLongitude();
            var houseSign = HoraSignAtLongitude(middleLongitude);

            //return the name of house sign
            return houseSign;
        }


        //------------ D3 ------------
        /// <summary>
        /// Gets the Drekkana sign the planet is in
        /// D3
        /// </summary>
        public static ZodiacSign PlanetDrekkanaSign(PlanetName planetName, Time time) => Calculate.DrekkanaSignName(Calculate.PlanetZodiacSign(planetName, time));

        /// <summary>
        /// Given a zodiac sign will convert to drekkana
        /// D3
        /// </summary>
        public static ZodiacSign DrekkanaSignName(ZodiacSign zodiacSign) => Vargas.VargasCoreCalculator(zodiacSign, Vargas.DrekkanaTable[zodiacSign.GetSignName()], 3);


        //------------ D4 ------------

        /// <summary>
        /// D4 chart
        /// </summary>
        public static ZodiacSign PlanetChaturthamshaSign(PlanetName planetName, Time time) => Calculate.ChaturthamshaSignName(Calculate.PlanetZodiacSign(planetName, time));

        /// <summary>
        /// D4 chart
        /// </summary>
        public static ZodiacSign ChaturthamshaSignName(ZodiacSign zodiacSign) => Vargas.VargasCoreCalculator(zodiacSign, Vargas.ChaturthamshaTable[zodiacSign.GetSignName()], 4);




        /// <summary>
        /// Gets list of all planets and the zodiac signs they are in
        /// </summary>
        public static Dictionary<PlanetName, ZodiacSign> AllPlanetSigns(Time time) => All9Planets.ToDictionary(planet => planet, planet => new ZodiacSign(PlanetZodiacSign(planet, time).GetSignName(), Angle.Zero));
        public static Dictionary<PlanetName, ZodiacSign> AllPlanetHoraSign(Time time) => All9Planets.ToDictionary(planet => planet, planet => PlanetHoraSigns(planet, time));
        public static Dictionary<PlanetName, ZodiacSign> AllPlanetDrekkanaSign(Time time) => All9Planets.ToDictionary(planet => planet, planet => PlanetDrekkanaSign(planet, time));
        public static Dictionary<PlanetName, ZodiacSign> AllPlanetChaturthamsaSign(Time time) => All9Planets.ToDictionary(planet => planet, planet => PlanetChaturthamshaSign(planet, time));
        public static Dictionary<PlanetName, ZodiacSign> AllPlanetPanchamsaSign(Time time) => All9Planets.ToDictionary(planet => planet, planet => PlanetPanchamsaSign(planet, time));


        #endregion

        #region TAJIKA

        /// <summary>
        /// Gets a given planet's Tajika Longitude
        /// </summary>
        /// <param name="scanYear">4 digit year number</param>
        public static Angle PlanetTajikaLongitude(PlanetName planetName, Time birthTime, int scanYear)
        {
            //based on birth sun sign find next date with exact sign for given year
            var possibleTajika = Calculate.TajikaDateForYear(birthTime, scanYear);

            //once found, use that date to get niryana longitude for asked for planet
            var tajikaLongitude = Calculate.PlanetNirayanaLongitude(planetName, possibleTajika);

            return tajikaLongitude;
        }

        /// <summary>
        /// Gets a given planet's Tajika constellation
        /// </summary>
        /// <param name="scanYear">4 digit year number</param>
        public static Constellation PlanetTajikaConstellation(PlanetName planetName, Time birthTime, int scanYear)
        {
            //get position of planet in longitude
            var planetLongitude = PlanetTajikaLongitude(planetName, birthTime, scanYear);

            //return the constellation behind the planet
            return ConstellationAtLongitude(planetLongitude);

        }

        /// <summary>
        /// Gets a given planet's Tajika zodiac sign
        /// </summary>
        /// <param name="scanYear">4 digit year number</param>
        public static ZodiacSign PlanetTajikaZodiacSign(PlanetName planetName, Time birthTime, int scanYear)
        {
            //get position of planet in longitude
            var planetLongitude = PlanetTajikaLongitude(planetName, birthTime, scanYear);

            //return the constellation behind the planet
            return ZodiacSignAtLongitude(planetLongitude);
        }

        /// <summary>
        /// Annual or Progressed Horoscope
        /// The annual
        /// or progressed horoscope (sidereal solar return according to Western astrology) is cast the same way as the
        /// birth horoscope. The time of the commencement of
        /// the anniversary, known as Varsharambha, is said to
        /// begin at the exact moment when the Sun comes to
        /// the same position he was in at the time of birth. In
        /// other words the individual's New Year begins when
        /// the Sun comes back to the same point he heJd at the·
        /// time of birth. 
        /// Given a birth time and scan year, will return exact time for tajika chart
        /// The tājika system attempts to predict in detail the likely happenings in one year of
        /// an individual's life. The system goes to such details as to predict events even on a
        /// day-by-day basis or even half-a-day. On account of this,
        /// this system is also called the varṣaphala system.
        /// </summary>
        /// <param name="scanYear">4 digit year number</param>
        public static Time TajikaDateForYear2(Time birthTime, int scanYear)
        {

            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey(nameof(TajikaDateForYear2), birthTime, scanYear, Ayanamsa), _tajikaDateForYear);


            //UNDERLYING FUNCTION

            Time _tajikaDateForYear()
            {
                //get position of sun on birth
                var sunBirthSign = SunSign(birthTime);

                //scan to find next time sun will be in same sign as birth for given year
                //(not overall longitude only same sign and degree in sign)
                var tajikaDateFound = false;

                //NOTE: to speed up computation time, only start scan 5 days before birth date
                //      this assumes that all tajika dates will only occure +/-5 days from birthday
                var birthDateYear = new Time($"00:00 {birthTime.StdDateText()}/{birthTime.StdMonthText()}/{scanYear} {birthTime.StdTimezoneText}", birthTime.GetGeoLocation());
                var possibleTajika = birthDateYear.SubtractHours(Tools.DaysToHours(5));

                while (!tajikaDateFound)
                {
                    //get sun sign at possible date
                    var possibleSunSign = Calculate.SunSign(possibleTajika);

                    //if found
                    var nameMatch = sunBirthSign.GetSignName() == possibleSunSign.GetSignName();
                    var degreesInSign = sunBirthSign.GetDegreesInSign().TotalDegrees;
                    var inSign = possibleSunSign.GetDegreesInSign().TotalDegrees;
                    var tolerance = 0.008; // Tolerance in degrees
                    var degreesMatch = Math.Abs(degreesInSign - inSign) <= tolerance;

                    //date found, can stop looking
                    if (nameMatch && degreesMatch)
                    {
                        tajikaDateFound = true;
                    }

                    //not found, keep looking
                    else
                    {
                        //NOTE : The sun moves across the zodiac at a rate of approximately 0.3 hours per minute.
                        //as such to be optimal we scan every 0.3 hours, to achive "DMS" minute level accuracy match
                        possibleTajika = possibleTajika.AddHours(0.3);
                    }

                }

                //possible date confirmed as correct date
                return possibleTajika;
            }

        }


        /// <summary>
        /// Annual or Progressed Horoscope
        /// The annual
        /// or progressed horoscope (sidereal solar return according to Western astrology) is cast the same way as the
        /// birth horoscope. The time of the commencement of
        /// the anniversary, known as Varsharambha, is said to
        /// begin at the exact moment when the Sun comes to
        /// the same position he was in at the time of birth. In
        /// other words the individual's New Year begins when
        /// the Sun comes back to the same point he heJd at the·
        /// time of birth. 
        /// Calculated based on method in BV Raman book "Varshaphala" 
        /// </summary>
        /// <param name="scanYear">4 digit year number</param>
        public static Time TajikaDateForYear(Time birthTime, int scanYear)
        {
            //This method of calculating the
            // Varshaphal horoscope (also called sidereal solar return
            // chart) is based on the modern value of the duration of
            // the sidereal year, viz., 365.256374 days or roughJy
            // 365 days, 6 hours, 9 minutes and 12 seconds differing from. the Hindu sidereal year by ~.5 vighatis or
            // 3 minutes and 24 seconds. A study· of a number of
            // annual charts for over 30 years has convinced me
            // that the moden_:1 value of the sidereal year would yield
            // better results. H

            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey(nameof(TajikaDateForYear), birthTime, scanYear, Ayanamsa), _tajikaDateForYear);


            //UNDERLYING FUNCTION

            Time _tajikaDateForYear()
            {

                //Below data was called out from pg 22 of Varshaphala-Hindu Progressed Horoscope - BV. RAMAN
                var records = new Dictionary<int, Dictionary<string, int>>()
                {
                    { 1, new Dictionary<string, int>(){ {"Days", 1}, {"Hrs", 6}, {"Mts", 9}, {"Secs", 12} }},
                    { 2, new Dictionary<string, int>(){ {"Days", 2}, {"Hrs", 12}, {"Mts", 18}, {"Secs", 18} }},
                    { 3, new Dictionary<string, int>(){ {"Days", 3}, {"Hrs", 18}, {"Mts", 27}, {"Secs", 30} }},
                    { 4, new Dictionary<string, int>(){ {"Days", 5}, {"Hrs", 0}, {"Mts", 36}, {"Secs", 36} }},
                    { 5, new Dictionary<string, int>(){ {"Days", 6}, {"Hrs", 6}, {"Mts", 45}, {"Secs", 48} }},
                    { 6, new Dictionary<string, int>(){ {"Days", 0}, {"Hrs", 12}, {"Mts", 55}, {"Secs", 0} }},
                    { 7, new Dictionary<string, int>(){ {"Days", 1}, {"Hrs", 19}, {"Mts", 4}, { "Secs", 6} }},
                    { 8, new Dictionary<string, int>(){ {"Days", 3}, {"Hrs", 1}, {"Mts", 13}, { "Secs", 18} }},
                    { 9, new Dictionary<string, int>(){ {"Days", 4}, {"Hrs", 7}, {"Mts", 22}, { "Secs", 30} }},
                    { 10, new Dictionary<string, int>(){ {"Days", 5}, {"Hrs", 13}, {"Mts", 31}, { "Secs", 36} }},
                    { 20, new Dictionary<string, int>(){ {"Days", 4}, {"Hrs", 13}, {"Mts", 3}, { "Secs", 12} }},
                    { 30, new Dictionary<string, int>(){ {"Days", 2}, {"Hrs", 16}, {"Mts", 34}, { "Secs", 54} }},
                    { 40, new Dictionary<string, int>(){ {"Days", 1}, {"Hrs", 6}, {"Mts", 6}, { "Secs", 30} }},
                    { 50, new Dictionary<string, int>(){ {"Days", 6}, {"Hrs", 19}, {"Mts", 38}, { "Secs", 6} }},
                    { 60, new Dictionary<string, int>(){ {"Days", 5}, {"Hrs", 9}, {"Mts", 9}, { "Secs", 42} }},
                    { 70, new Dictionary<string, int>(){ {"Days", 3}, {"Hrs", 22}, {"Mts", 41}, { "Secs", 24} }},
                    { 80, new Dictionary<string, int>(){ {"Days", 2}, {"Hrs", 12}, {"Mts", 13}, { "Secs", 00} }},
                    { 90, new Dictionary<string, int>(){ {"Days", 1}, {"Hrs", 1}, { "Mts", 44 }, { "Secs", 36} }},
                    { 100, new Dictionary<string, int>(){ {"Days", 6}, {"Hrs", 15}, { "Mts", 13 }, { "Secs", 12} }}
                };

                throw new Exception();
            }

        }


        #endregion

        #region TRANSITS

        public static HouseName TransitHouseFromLagna(PlanetName transitPlanet, Time checkTime, Time birthTime)
        {
            //Note the Lagna Rashi.
            var lagnaRasi = HouseZodiacSign(HouseName.House1, birthTime);

            //Choose the planet transit result for which predictions to be made.

            //Note the transit position of the Moon with reference to
            //Natal Moon(Janma Rashi) when the chosen planet enters a new sign.
            var transitRasi = PlanetZodiacSign(transitPlanet, checkTime);
            var count = Calculate.CountFromSignToSign(lagnaRasi.GetSignName(), transitRasi.GetSignName());

            return (HouseName)count;

        }

        public static HouseName TransitHouseFromNavamsaLagna(PlanetName transitPlanet, Time checkTime, Time birthTime)
        {
            //Note the Lagna Rashi.
            var navamsaLagnaRasi = Calculate.HouseNavamsaSign(HouseName.House1, birthTime);

            //Choose the planet transit result for which predictions to be made.

            //Note the transit position of the Moon with reference to
            //Natal Moon(Janma Rashi) when the chosen planet enters a new sign.
            var transitRasi = PlanetZodiacSign(transitPlanet, checkTime);
            var count = Calculate.CountFromSignToSign(navamsaLagnaRasi, transitRasi.GetSignName());

            return (HouseName)count;

        }

        public static HouseName TransitHouseFromMoon(PlanetName transitPlanet, Time checkTime, Time birthTime)
        {
            //Note the Janma Rashi.
            var janmaRasi = PlanetZodiacSign(Moon, birthTime);

            //Choose the planet transit result for which predictions to be made.
            //Note the transit position of the Moon with reference to
            //Natal Moon(Janma Rashi) when the chosen planet enters a new sign.
            var transitRasi = PlanetZodiacSign(transitPlanet, checkTime);
            var count = Calculate.CountFromSignToSign(janmaRasi.GetSignName(), transitRasi.GetSignName());

            return (HouseName)count;
        }

        public static HouseName TransitHouseFromNavamsaMoon(PlanetName transitPlanet, Time checkTime, Time birthTime)
        {
            //Note the Janma Rashi.
            var janmaRasi = Calculate.PlanetNavamsaSign(Moon, birthTime);

            //Choose the planet transit result for which predictions to be made.
            //Note the transit position of the Moon with reference to
            //Natal Moon(Janma Rashi) when the chosen planet enters a new sign.
            var transitRasi = PlanetZodiacSign(transitPlanet, checkTime);
            var count = Calculate.CountFromSignToSign(janmaRasi, transitRasi.GetSignName());

            return (HouseName)count;
        }

        public static string Murthi(PlanetName transitPlanet, Time checkTime, Time birthTime)
        {
            return "";

            //if moon retun no murthi
            if (transitPlanet == Moon) { return ""; }

            //Note the Janma Rashi.
            var janmaRasi = PlanetZodiacSign(Moon, birthTime);

            //Choose the planet transit result for which predictions to be made.

            //Note the transit position of the Moon with reference to
            //Natal Moon(Janma Rashi) when the chosen planet enters a new sign.
            var transitRasi = PlanetZodiacSign(transitPlanet, checkTime);
            var count = Calculate.CountFromSignToSign(janmaRasi.GetSignName(), transitRasi.GetSignName());

            //Name the Moorti as follows:- •

            //If the transit Moon is in 1st, 6th or 11th from Natal Moon – Swarna(Golden) Moorti.
            //If it is in 2nd, 5th or 9th – Rajata(Silver) Moorti.
            //If it is in 3rd, 7th or 10th – Tamra(Copper) Moorti.
            //If it is in 4th, 8th or 12th – Loha(Iron) Moorti.

            throw new Exception("");

        }

        #endregion

        #region PANCHA PAKSHI

        /// <summary>
        /// In each of the main activities, the other four activities also occur as
        /// abstract sub-activity for short duration of time gaps covering the complete
        /// duration of the main activity, the period being 2 hrs. 24 min
        /// for Pancha Pakshi
        /// </summary>
        public static BirdActivity AbstractActivity(Time checkTime)
        {
            //start counting from start of current Yama
            var yamaStartTime = BirthYama(checkTime).YamaStartTime;

            //based on day or night birth start checking
            if (IsDayBirth(checkTime))
            {
                //total minutes is 2h 24min
                var daySubTimings = new Dictionary<BirdActivity, double>()
                {
                    {BirdActivity.Eating, 30},
                    {BirdActivity.Walking, 36},
                    {BirdActivity.Ruling, 48},
                    {BirdActivity.Sleeping, 18},
                    {BirdActivity.Dying, 12}
                };

                //find which sub activity given time falls under
                foreach (var timing in daySubTimings)
                {
                    //calculate end time for this yama
                    var subYamaSpanMin = timing.Value;
                    var yamaEndTime = yamaStartTime.AddHours(Tools.MinutesToHours(subYamaSpanMin));

                    //if birth time is in this sub-yama, found! end here.
                    //(start time must be smaller and time must be bigger)
                    if (yamaStartTime < checkTime && yamaEndTime > checkTime)
                    {
                        return timing.Key; //bird name
                    }

                    //since not found
                    //keep looking, end of this yama begins next
                    yamaStartTime = yamaEndTime;
                }
            }
            //night birth
            else
            {
                //total minutes is 2h 24min
                var nightSubTimings = new Dictionary<BirdActivity, double>()
                {
                    {BirdActivity.Eating, 30},
                    {BirdActivity.Ruling, 24},
                    {BirdActivity.Dying, 36},
                    {BirdActivity.Walking, 30},
                    {BirdActivity.Sleeping, 24}
                };

                //find which sub activity given time falls under this sub yama
                foreach (var timing in nightSubTimings)
                {
                    //calculate end time for this yama
                    var subYamaSpanMin = timing.Value;
                    var yamaEndTime = yamaStartTime.AddHours(Tools.MinutesToHours(subYamaSpanMin));

                    //if birth time is in this sub-yama, found! end here.
                    //(start time must be smaller and time must be bigger)
                    if (yamaStartTime < checkTime && yamaEndTime > checkTime)
                    {
                        return timing.Key; //bird name
                    }

                    //since not found
                    //keep looking, end of this yama begins next
                    yamaStartTime = yamaEndTime;
                }
            }


            throw new Exception("END OF LINE!");
        }

        /// <summary>
        /// Each bird performs these five activities during each day
        /// and in night over the week days and during waxing and
        /// waning Moon cycles during the 5 YAMAS in day and 5
        /// YAMAS in night in a stipulated order
        /// for Pancha Pakshi
        /// </summary>
        public static BirdActivity MainActivity(Time birthTime, Time checkTime)
        {

            // Determine the bird's type and its current main and abstract activities.
            var birthBird = PanchaPakshiBirthBird(birthTime);
            var timeOfDay = IsDayBirth(checkTime) ? PanchaPakshi.TimeOfDay.Day : PanchaPakshi.TimeOfDay.Night;
            var dayOfWeek = DayOfWeek(checkTime);
            var yamaNumber = BirthYama(checkTime).YamaCount;

            // Retrieve the strength of the bird's abstract activity from the pre-initialized dictionary.
            var mainActivity = PanchaPakshi.TableData[timeOfDay][dayOfWeek][yamaNumber][birthBird];
            return mainActivity;

        }

        /// <summary>
        /// These 5 elemental vibrations act in 5 gradations offaculties for stipulated time
        /// intervals called (YAMAS) consisting of
        /// 2 hrs. 24 mits. each (6 Ghatikas each) over the 5 YAMAS in
        /// the day and 5 YAMAS in the night, thus spread over evenly in
        /// 24 hours.
        /// </summary>
        public static BirthYama BirthYama(Time inputTime)
        {
            //get the vedic day start time for given input time (aka sunrise)
            var dayStartVedic = Calculate.VedicDayStartTime(inputTime);

            //get start of vedic day and start checking 1 yama range at a time
            var isFound = false;
            var yamaCount = 1;
            var yamaStartTime = dayStartVedic;
            while (!isFound)
            {
                //calculate yama end time based on yama count (2h 24min = 2.4h)
                //var minFromStart = 2.4 * yamaCount;
                var yamaEndTime = yamaStartTime.AddHours(2.4); // Changed this line

                //if birth time is in this yama, found! end here.
                //(start time must be smaller or equal and end time must be bigger or equal)
                if (yamaStartTime <= inputTime && yamaEndTime >= inputTime)
                {
                    //if above 5, restart Yama count for night cycle
                    if (yamaCount > 5) { yamaCount = yamaCount - 5; }

                    return new BirthYama(yamaCount, yamaStartTime, yamaEndTime);
                }

                //keep looking, end of this yama begins next
                yamaStartTime = yamaEndTime;

                yamaCount++;
            }

            throw new Exception("END OF LINE!");

        }

        /// <summary>
        /// Given a time, it will find out the start time of for that vedic day
        /// If time is before sunrise, the previous day
        /// </summary>
        public static Time VedicDayStartTime(Time inputTime)
        {
            var sunrise = Calculate.SunriseTime(inputTime);

            //#PREVIOUS DAY
            //time should be before sunrise (sunrise time will be bigger)
            if (sunrise > inputTime)
            {
                var previousDay = inputTime.SubtractHours(23); //todo proper method
                var yamaStartTime = Calculate.SunriseTime(previousDay);
                return yamaStartTime;
            }
            //else return sunrise as is
            else
            {
                return sunrise;
            }


        }



        /// <summary>
        /// yama works out to 2 hrs. 24 mts. of our modern time.
        /// It is to be noted that the beginning of the day is
        /// reckoned from Sun rise to Sun set in Hindu system. Similarly
        /// night is reckoned from Sun set to Sun rise on the following
        /// day, thus consisting of 24 hours for one day.
        /// The timings of the five Yamas are the same during day
        /// and night
        /// for Pancha Pakshi
        /// </summary>


        /// <summary>
        /// Calculates the strength of a bird's "Abstract" activity (sub activity) based on its birth time.
        /// for pancha pakshi bird
        /// </summary>
        /// <param name="birthTime">The bird's birth time :D</param>
        /// <returns>The strength of the bird's activity.</returns>
        public static double AbstractActivityStrength(Time birthTime, Time checkTime)
        {
            // Determine the bird's type and its current main and abstract activities.
            var birthBird = PanchaPakshiBirthBird(birthTime);
            var mainActivity = MainActivity(birthTime, checkTime);
            var abstractActivity = AbstractActivity(checkTime);

            // Retrieve the strength of the bird's abstract activity from the pre-initialized dictionary.
            return PanchaPakshi.AbstractActivityStrengthTable[birthBird][mainActivity][abstractActivity];
        }

        /// <summary>
        /// Gets "birth bird" for a birth time.
        /// Sidhas have personified the elements as birds identifying each element under
        /// which an individual is born, when these elements are all functioning differentially
        /// during each time gap. These 5 elemental vibrations are personified as PAKSHIS or BIRDS and the
        /// gradations of their faculities are named as 5 activities.
        /// This bird is called his birth Stellar Lunar bird.
        /// </summary>
        public static BirdName PanchaPakshiBirthBird(Time birthTime)
        {
            //get rulling constellation
            var rullingConst = Calculate.MoonConstellation(birthTime);
            var rullingConstNumber = (int)rullingConst.GetConstellationName();

            //based on waxing or waning assign bird accordingly
            var isWaxing = Calculate.IsWaxingMoon(birthTime);
            if (isWaxing)
            {
                switch (rullingConstNumber)
                {
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                    case 5:
                        return BirdName.Vulture;
                    case 6:
                    case 7:
                    case 8:
                    case 9:
                    case 10:
                    case 11:
                        return BirdName.Owl;
                    case 12:
                    case 13:
                    case 14:
                    case 15:
                    case 16:
                        return BirdName.Crow;
                    case 17:
                    case 18:
                    case 19:
                    case 20:
                    case 21:
                        return BirdName.Cock;
                    case 22:
                    case 23:
                    case 24:
                    case 25:
                    case 26:
                    case 27:
                        return BirdName.Peacock;
                }
            }
            //else must be wanning
            else
            {
                switch (rullingConstNumber)
                {
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                    case 5:
                        return BirdName.Peacock;
                    case 6:
                    case 7:
                    case 8:
                    case 9:
                    case 10:
                    case 11:
                        return BirdName.Cock;
                    case 12:
                    case 13:
                    case 14:
                    case 15:
                    case 16:
                        return BirdName.Crow;
                    case 17:
                    case 18:
                    case 19:
                    case 20:
                    case 21:
                        return BirdName.Owl;
                    case 22:
                    case 23:
                    case 24:
                    case 25:
                    case 26:
                    case 27:
                        return BirdName.Vulture;
                }
            }

            throw new Exception("END OF LINE!");
        }

        /// <summary>
        /// Ancients have evolved a method of identifying the birth bird of
        /// other individuals by recognising the first
        /// vowel sound that shoots out while uttering the name of such
        /// individual. Here, we have to be
        /// very careful in identifying the first vowel sound (and not the
        /// first vowel letter) ofthe other man's name. In this system, the
        /// vowels referred to are ofthe Dravidian Origin TAMIL and do
        /// not indicate the English vowel sounds. This should always be
        /// borne in mind.
        /// It should
        /// be remembered that the eleven vowels of Dravidian Tamil
        /// language are distributed among the 5 birds. These vowels and
        /// consonants which contain them are to be identified from the
        /// first sound of the name. Virtually, these eleven vowel sounds
        /// are to be equated and sounded by the five English vowels A, E,
        /// I, O and U. In this language "U" is uttered as "V + U = VU",
        /// to project the Dravidian sound. Except the sound "I", all
        /// other sounds have short and long vowels.
        ///
        /// From what has been explained so far, it can be understood
        /// that for the same name, the birds are different during bright
        /// half and dark halfperiods of Moon where we do not know the
        /// birth data of the other person and for such persons only we
        /// should use this system
        /// </summary>
        /// <param name="name">a popular name and known by that name only</param>
        public static BirdName PanchaPakshiBirthBirdFromName(string name)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Given a time will return true if it is on
        /// "Waxing moon" or "Shukla Paksha" or "Bright half"
        /// </summary>
        public static bool IsWaxingMoon(Time birthTime)
        {
            var lunarDay = LunarDay(birthTime);

            return lunarDay.GetMoonPhase() == MoonPhase.BrightHalf;
        }

        /// <summary>
        /// Given a time will return true if it is on
        /// "Waning moon" or "Krishna Paksha" or "Dark half"
        /// </summary>
        public static bool IsWaningMoon(Time birthTime)
        {
            var lunarDay = LunarDay(birthTime);

            return lunarDay.GetMoonPhase() == MoonPhase.DarkHalf;
        }

        /// <summary>
        /// Given a name will extract out the 1st vowel sound.
        /// Used to get Pancha Pakshi bird when birth date not known
        /// </summary>
        public static string FirstVowelSound(string word)
        {
            HashSet<char> Vowels = new HashSet<char>("aeiouAEIOU");
            Dictionary<string, string> ConsecutiveVowelMap = new Dictionary<string, string>()
            {
                { "AI", "I" },
                { "AE", "A" },
                { "AO", "A" },
                { "AU", "A" },
                { "EZ", "EA" },
                { "JA", "EA" },
                { "PE", "EA" },
                { "ES", "EA" },
                { "EI", "E" },
                { "MI", "E" },
                { "EA", "E" },
                { "EO", "E" },
                { "EU", "E" },
                { "IA", "I" },
                { "IE", "I" },
                { "IO", "I" },
                { "IU", "I" },
                { "OA", "O" },
                { "OE", "O" },
                { "OI", "O" },
                { "OU", "OW" },
                { "OP", "O" },
                { "UA", "U" },
                { "UE", "U" },
                { "UI", "U" },
                { "UO", "U" }
            };


            // Remove non-letter characters from the input word
            var cleanedWord = new string(word.Where(Char.IsLetter).ToArray());

            // Split the cleaned word into syllables
            var syllables = Syllables(cleanedWord);

            for (int i = 0; i < syllables.Count; i++)
            {
                var syllableToCheck = syllables[i];

                // If the syllable starts with a vowel or contains consecutive vowels at its end and beginning, handle it accordingly
                if (IsVowel(syllableToCheck) || (i < syllables.Count - 1 && IsVowel(syllableToCheck[^1].ToString()) && IsVowel(syllables[i + 1][0].ToString())))
                {
                    // Use the lookup table if there are two consecutive vowels at the end and beginning of adjacent syllables
                    if (i < syllables.Count - 1 && IsVowel(syllableToCheck[^1].ToString()) && IsVowel(syllables[i + 1][0].ToString()))
                    {
                        var firstVowelSound = syllableToCheck[^1].ToString() + syllables[i + 1][0];

                        if (ConsecutiveVowelMap.TryGetValue(firstVowelSound.ToUpper(), out string mappedValue2))
                        {
                            return mappedValue2;
                        }

                        return firstVowelSound;
                    }

                    // Use the lookup table for syllables that only contain vowels
                    if (IsVowel(syllableToCheck) && ConsecutiveVowelMap.TryGetValue(syllableToCheck.ToUpper(), out string mappedValue))
                    {
                        return mappedValue;
                    }

                    // Return the last vowel sound if it's not followed by another vowel in the next syllable
                    if (i < syllables.Count - 1 && IsVowel(syllableToCheck[^1].ToString()) && !IsVowel(syllables[i + 1][0].ToString()))
                    {
                        return syllableToCheck[^1].ToString();
                    }

                    // Otherwise, just return the syllable itself
                    return syllableToCheck;
                }
            }

            return "";


            List<string> Syllables(string word)
            {
                var vowels = new HashSet<char> { 'a', 'e', 'i', 'o', 'u' };
                var syllables = new List<string>();
                var currentSyllable = new StringBuilder();

                for (int i = 0; i < word.Length; i++)
                {
                    currentSyllable.Append(word[i]);
                    if (vowels.Contains(char.ToLower(word[i])))
                    {
                        syllables.Add(currentSyllable.ToString());
                        currentSyllable.Clear();

                        if (i < word.Length - 1 && !vowels.Contains(char.ToLower(word[i + 1])))
                        {
                            currentSyllable.Append(word[i + 1]);
                            i++;
                        }
                    }
                }

                if (currentSyllable.Length > 0)
                {
                    syllables.Add(currentSyllable.ToString());
                }

                // Combine single character vowel syllables
                for (int i = 0; i < syllables.Count - 1; i++)
                {
                    if (syllables[i].Length == 1 && syllables[i + 1].Length == 1 && vowels.Contains(char.ToLower(syllables[i][0])) && vowels.Contains(char.ToLower(syllables[i + 1][0])))
                    {
                        syllables[i] += syllables[i + 1];
                        syllables.RemoveAt(i + 1);
                        i--;
                    }
                }

                return syllables;
            }

            bool IsVowel(string syllable)
            {
                return syllable.Any(c => "aeiou".Contains(char.ToLower(c)));
            }

        }



        #endregion

        #region PANCHANGA

        /// <summary>
        /// It’s used to determine auspicious times and rituals.
        /// It includes multiple attributes such as,
        /// Tithi (lunar day),
        /// Lunar Month
        /// Vara (weekday),
        /// Nakshatra (constellation),
        /// Yoga (luni-solar day) and Karana (half of a Tithi).
        /// Disha Shool
        /// </summary>
        public static PanchangaTable PanchangaTable(Time inputTime)
        {
            //Ayanamsa
            var ayanamsaDegree = Calculate.AyanamsaDegree(inputTime).DegreesMinutesSecondsText;

            //Tithi (lunar day)
            var tithi = Calculate.LunarDay(inputTime);

            //lunar month
            var lunarMonth = Calculate.LunarMonth(inputTime);

            //Vara (weekday)
            var weekDay = Calculate.DayOfWeek(inputTime);

            //Nakshatra (constellation)
            var constellation = Calculate.MoonConstellation(inputTime);

            //Yoga (luni-solar day) 
            var yoga = Calculate.NithyaYoga(inputTime);

            //Karana (half of a Tithi)
            var karana = Calculate.Karana(inputTime);

            //Hora Lord
            var horaLord = Calculate.LordOfHoraFromTime(inputTime);

            //Disha Shool
            var dishaShool = Calculate.DishaShool(inputTime);

            //Sunrise
            var sunrise = Calculate.SunriseTime(inputTime);

            //Sunset
            var sunset = Calculate.SunsetTime(inputTime);

            //Ishta Kaala
            var ishtaKaala = Calculate.IshtaKaala(inputTime);

            return new PanchangaTable(ayanamsaDegree, tithi, lunarMonth, weekDay, constellation, yoga, karana, horaLord, dishaShool, sunrise, sunset, ishtaKaala);
        }

        /// <summary>
        /// Here are the following Disha shool days and the directions that are considered as
        /// inauspicious or Disha shool. Check the Disha Shool chart to find the inauspicious direction to travel 
        /// </summary>
        public static string DishaShool(Time inputTime)
        {
            //vedic day
            var vedicWeekDay = Calculate.DayOfWeek(inputTime);

            switch (vedicWeekDay)
            {
                case Library.DayOfWeek.Monday: return "East";
                case Library.DayOfWeek.Tuesday: return "North";
                case Library.DayOfWeek.Wednesday: return "North";
                case Library.DayOfWeek.Thursday: return "South";
                case Library.DayOfWeek.Friday: return "West";
                case Library.DayOfWeek.Saturday: return "East";
                case Library.DayOfWeek.Sunday: return "West";
            }

            throw new Exception("END OF LINE!");
        }

        /// <summary>
        /// Also know as Chandramana or Hindu Month.
        /// Each Hindu month begins with the New Moon.
        /// These lunar months go by special names. The name of a lunar month is
        /// decided by the rasi in which Sun-Moon conjunction takes place.
        /// These names come from the constellation that Moon is most likely to
        /// occupy on the full Moon day.
        /// Names are Chaitra, Vaisaakha, Jyeshtha, Aashaadha, Sraavana etc...
        /// </summary>
        public static LunarMonth LunarMonth(Time inputTime, bool ignoreLeapMonth = false)
        {
            //TODO JAN 2024
            //needs further validation, the month before
            //Adhika is also shown as Adhika
            //most test cases pass, but some closser to change dates fail

            //based on vedic start of day (sunrise time)
            //scan and get dates when new moon last occured
            var sunriseTime = Calculate.SunriseTime(inputTime);
            var lastNewMoonRaw = Calculate.PreviousNewMoon(sunriseTime); //for this moon month
            var nextNewMoon = Calculate.NextNewMoon(sunriseTime); //for next moon month

            //get sign as number
            var thisMonthSign = (int)Calculate.MoonSignName(lastNewMoonRaw);
            var nextMonthSign = (int)Calculate.MoonSignName(nextNewMoon);

            //detect leap month if 2 months are same name
            var isLeapMonth = (thisMonthSign == nextMonthSign);

            //increment 1 to convert from rasi to solar month number
            var monthNumber = thisMonthSign + 1;

            //if exceed 12 than loop back to 1
            if (monthNumber > 12) { monthNumber = monthNumber % 12; }

            //verify if really leap month (rescursive)
            //NOTE: this was added later as hack (remove if needed)
            if (isLeapMonth && !ignoreLeapMonth)
            {
                var ccc = Calculate.NextNewMoon(nextNewMoon.AddHours(24));
                var nextNewMoonxx = Calculate.LunarMonth(ccc, true); //NOTE:turn off recursive
                var possibleLeapMonth = ((LunarMonth)monthNumber).ToString();

                //checks if month name is in the next months name (Jyeshtha -> JyeshthaAdhika)
                var vvv = nextNewMoonxx.ToString().Contains(possibleLeapMonth);
                if (!vvv)
                {
                    isLeapMonth = false;
                }
            }

            //based on month number (NOT sign number or constellation)
            //set the name of the lunar month also based on if leap month
            var monthName = Library.LunarMonth.Empty;
            switch (monthNumber)
            {
                case 1: monthName = isLeapMonth ? Library.LunarMonth.ChaitraAdhika : Library.LunarMonth.Chaitra; break;
                case 2: monthName = isLeapMonth ? Library.LunarMonth.VaisaakhaAdhika : Library.LunarMonth.Vaisaakha; break;
                case 3: monthName = isLeapMonth ? Library.LunarMonth.JyeshthaAdhika : Library.LunarMonth.Jyeshtha; break;
                case 4: monthName = isLeapMonth ? Library.LunarMonth.AashaadhaAdhika : Library.LunarMonth.Aashaadha; break;
                case 5: monthName = isLeapMonth ? Library.LunarMonth.SraavanaAdhika : Library.LunarMonth.Sraavana; break;
                case 6: monthName = isLeapMonth ? Library.LunarMonth.BhaadrapadaAdhika : Library.LunarMonth.Bhaadrapada; break;
                case 7: monthName = isLeapMonth ? Library.LunarMonth.AaswayujaAdhika : Library.LunarMonth.Aaswayuja; break;
                case 8: monthName = isLeapMonth ? Library.LunarMonth.KaarteekaAdhika : Library.LunarMonth.Kaarteeka; break;
                case 9: monthName = isLeapMonth ? Library.LunarMonth.MaargasiraAdhika : Library.LunarMonth.Maargasira; break;
                case 10: monthName = isLeapMonth ? Library.LunarMonth.PushyaAdhika : Library.LunarMonth.Pushya; break;
                case 11: monthName = isLeapMonth ? Library.LunarMonth.MaaghaAdhika : Library.LunarMonth.Maagha; break;
                case 12: monthName = isLeapMonth ? Library.LunarMonth.PhaalgunaAdhika : Library.LunarMonth.Phaalguna; break;
            }

            return monthName;
        }

        /// <summary>
        /// Gets next future New Moon date, when tithi will be 1.
        /// Uses conjunctions angle to calculate with accuracy of ~30min
        /// Includes start time in scan
        /// </summary>
        public static Time NextNewMoon(Time inputTime)
        {
            //scan till find
            //start with input time
            var newMoonTime = inputTime;
            while (true)
            {
                //if conjunction, than new moon dectected
                var conjunctAngle = SunMoonConjunctionAngle(newMoonTime);

                //When Sun and Moon are at the same longitude, a new lunar month of 30 tithis starts
                //which conjunction 0 degrees
                if (conjunctAngle.TotalDegrees < 1)
                {
                    return newMoonTime;
                }

                //go foward in time since did not find 0 degree conjunction
                newMoonTime = newMoonTime.AddHours(0.5);
            }

            return newMoonTime;
        }

        /// <summary>
        /// Gets last occured New Moon date, when tithi will be 1.
        /// Uses conjunctions angle to calculate with accuracy of ~30min
        /// Includes start time in scan
        /// </summary>
        public static Time PreviousNewMoon(Time inputTime)
        {
            //scan till find
            //start with input time
            var newMoonTime = inputTime;
            while (true)
            {
                //if conjunction, than new moon dectected
                var conjunctAngle = SunMoonConjunctionAngle(newMoonTime);

                //When Sun and Moon are at the same longitude, a new lunar month of 30 tithis starts
                //which conjunction 0 degrees
                if (conjunctAngle.TotalDegrees < 1)
                {
                    return newMoonTime;
                }

                //go backward in time since did not find 0 degree conjunction
                newMoonTime = newMoonTime.SubtractHours(0.5);
            }
        }

        /// <summary>
        /// Gets the distance in degrees between Sun & Moon at a given time
        /// Used to calculate lunar months.
        /// </summary>
        public static Angle SunMoonConjunctionAngle(Time ccc)
        {
            //longitudes of the sun & moon
            Angle sunLong = PlanetNirayanaLongitude(Sun, ccc);
            Angle moonLong = PlanetNirayanaLongitude(Moon, ccc);

            //get non negative difference, expunge 360 if needed
            var cleanedDifference = moonLong.GetDifference(sunLong).Expunge360();

            return cleanedDifference;
        }

        #endregion

        #region NUMEROLOGY

        /// <summary>
        /// Numerology
        /// Your birth number denotes your ruling power; the structure of
        /// the body and the character depend on that number.
        /// The birth number denotes a person’s status and desires.
        /// let us take it as 17-10-1931. Number 17 becomes 1+7 = 8.
        /// So 8 is your Birth number.
        /// </summary>
        public static int BirthNumber(Time birthTime)
        {
            //get STD birth date in month (1-31)
            var birthDate = birthTime.StdDate();

            //sum the single digits to get birth number till number is 9 or lower (single digit)
            while (birthDate > 9)
            {
                birthDate = birthDate.ToString().Select(x => x - '0').Sum();
            }

            return birthDate;
        }

        /// <summary>
        /// Numerology
        /// The events that occur in your life, your relationship with others, your future and the
        /// end of your life, are all denoted by your destiny number.
        /// The destiny number denotes to what extent a person will come up in life as well
        /// as it determines his fate.
        /// </summary>
        public static int DestinyNumber(Time birthTime)
        {
            //EXP :17-10-1931
            //The sum total of your date of birth, month and year is your Destiny
            //number. So, if all the numbers are added up, i.e. 1 + 7 + 1 + 0 + 1 +
            //9 + 3 + 1 = 23 ; then 2 + 3 = 5 is the answer. 

            //to count the number, 1st convert to string without any space or characters
            var combinedNumberText = birthTime.StdDateMonthYearText.Replace("/", "");

            //add together all the numbers
            var destinyNumber = combinedNumberText.Select(x => x - '0').Sum();

            //add together total till get number 9 or less
            //sum the single digits to get birth number till number is 9 or lower (single digit)
            while (destinyNumber > 9)
            {
                destinyNumber = destinyNumber.ToString().Select(x => x - '0').Sum();
            }

            return destinyNumber;

        }

        /// <summary>
        /// The numerical values given to the alphabets
        /// are based on the “Chaldean System”
        /// Numbers (values) denote the wave length of the
        /// sound and impact of letters.
        /// The powers of the nine planets in twelve star signs at different
        /// times are indicated in 108 numbers.
        /// </summary>
        public static int NameNumber(string fullName)
        {
            var alphabetScoreList = new Dictionary<char, int>
            {
                { 'a', 1 },
                { 'b', 2 },
                { 'c', 3 },
                { 'd', 4 },
                { 'e', 5 },
                { 'f', 8 },
                { 'g', 3 },
                { 'h', 5 },
                { 'i', 1 },
                { 'j', 1 },
                { 'k', 2 },
                { 'l', 3 },
                { 'm', 4 },
                { 'n', 5 },
                { 'o', 7 },
                { 'p', 8 },
                { 'q', 1 },
                { 'r', 2 },
                { 's', 3 },
                { 't', 4 },
                { 'u', 6 },
                { 'v', 6 },
                { 'w', 6 },
                { 'x', 5 },
                { 'y', 1 },
                { 'z', 7 }
            };

            //when letter alone, different score, due to pronounce change
            var initialScoreList = new Dictionary<char, int>
            {
                { 'a', 1 },
                { 'b', 2 },
                { 'c', 3 },
                { 'd', 4 },
                { 'e', 5 },
                { 'f', 8 },
                { 'g', 3 },
                { 'h', 5 },
                { 'i', 10 }, //I, J and Y becomes 10
                { 'j', 10 },
                { 'k', 20 }, //K becomes 20
                { 'l', 30 }, //L and S becomes 30
                { 'm', 40 }, //M becomes 40
                { 'n', 50 }, //N becomes 50
                { 'o', 70 }, //O becomes 70
                { 'p', 80 }, //P becomes 80
                { 'q', 100 }, //Q becomes 100
                { 'r', 200 }, //R becomes 200 
                { 's', 30 }, //L and S becomes 30
                { 't', 400 }, //Tbecomes 400
                { 'u', 6 },
                { 'v', 6 },
                { 'w', 6 },
                { 'x', 5 },
                { 'y', 10 }, //I, J and Y becomes 10
                { 'z', 7 }
            };

            //NOTE:
            // calculations must be
            // made only on the basis of the spelling of the name along with the
            // initials. Letters denoting respect or status like Mr., etc., and degrees
            // and suffixes to names have no value. However, names starting with
            // title of Dr. (Doctor) should be taken into account.

            //remove dots "Dr." --> "Dr"
            fullName = fullName.Replace(".", "");

            //remove numbers & fancy characters
            fullName = Regex.Replace(fullName, "[^a-zA-Z]", "");

            //make lower case for matching
            fullName = fullName.ToLower();

            //split by space into name pieces, first name, last name, initial (order not important)
            var splittedName = fullName.Split(" ");

            //add together number from each name piece
            var totalScore = 0;
            foreach (var namePiece in splittedName)
            {
                //if piece is only 1 alphabet, than it is an initial (different scoring system)
                var isInitial = namePiece.Length == 1;

                //add special score for initial
                if (isInitial) { totalScore += initialScoreList[namePiece[0]]; }

                //add together points for each alphabet in name piece
                else
                {
                    foreach (var alphabet in namePiece)
                    {
                        totalScore += alphabetScoreList[alphabet];
                    }
                }
            }

            //return final added score for full name
            return totalScore;

        }

        /// <summary>
        /// Shows numerology prediction for given name. At first the name number is calculated
        /// based on “Chaldean System”, then prediction is matched with
        /// translation from Mantra Sutras.
        /// </summary>
        public static string NameNumberPrediction(string fullName)
        {
            //get name number
            var nameNumber = NameNumber(fullName);

            //dictionary of all possible predictions 
            var nameNumberPredictions = new Dictionary<int, string>
            {
                //1 for SUN
                { 10, "This number when comes as a name indicates the sound or resonance of the primal force. This is depicted in the ancient texts as a snake enmeshed within a wheel. Those named under this number will be dignified and popular. Confidence and patience coexist in their lives but their fortunes will change frequently. It is like a revolving wheel, with ups and downs frequently. They must be honest in all their activities and they are bound to gain popularity. They will lead happy lives since there will be no paucity of funds" },
                { 19, "Ancient books on this subject attribute mastery over the Three Worlds to this Number and as such, these people will be the focus of attention wherever they are. This number indicates the Rising Sun. This also has been described as the “Prince of the Celestial World” in ancient Indian texts and as an ‘‘Ideal Lover’’ in Egyptian scriptures. The sun becomes brighter as the day lengthens and so also these people progress as their age advances. Position, status, happiness, success and wealth will be gradually on the rise. Being well-disciplined, they will look young and will be very active even in their advanced age. They must be honest even in matters related to sensual pleasures.\r" },
                { 28, "Those with this name number do progress and get all the comforts during the early part of life but they frequently face struggles or difficulties in all their endeavours in life. They may have to start their lives again and again always afresh, many times over. Although they progress very fast in their lives, they finally lose everything due to the cruel stroke of fate. One of the examples of such people born under this number is General MacArthur, a fine soldier who deserved more of honour and recognition, but was deprived of his position and career by President Truman of USA! Those coming under this number may incur unexpected losses due to their friends and relatives. Money lent by them rarely comes back. As such, this number can only be regarded as an unlucky one, since all the hardearned money may be lost unexpectedly." },
                { 37, "This is a very lucky number. It will lift even an ordinary person to the most prominent positions in life. It brings success in love and the patronage of the elite. These people will have good friends from both sexes. They will be greatly favoured by men if they are women and vice versa. As a result, their lives will improve greatly. People will come forward to invest their capital with such people. Accumulation of money and wealth will be easy through various means. They will have an active interest in the fine arts and in all probability, lead comfortable and luxurious lives. They will be renowned for their pleasing manners and countenance. Some of them will be philanderers because of their casual attitude towards opposite sex. This number, which gives unexpected success, is a desirable one. (Note: If people who occupy very high positions have their names under this number, it may bring them unnecessary problems. This number will bring good fortune to ordinary people. These people should remain satisfied when they attain a certain position in life and should not be too ambitious. This number, will bring fortune automatically, but will lose its power when one becomes too greedy.)\r" },
                { 46, "This has been described as the “Crowned Head” in the ancient texts. It means that when prudence, intelligence and knowledge are used wisely, it will bring the crown of life. Whatever may be the business, this number will help one to reach the pinnacle of success and is capable of raising even the most ordinary person to the position of a ruler. Wealth and status will go up with the advancement of age. People belonging to this number should be honest in all walks of life." },
                { 55, "This number predicts that both creation and destruction can be done by a single power. This will bring victory over enemies. Before entering the battlefield, Greek soldiers were ordered to wear a talisman marked with number 55 around their necks. This number is the epitome of will-power and intuition. People under this number will astonish others by their knowledge and win them over. They are acknowledged as scholars. Wisdom and intelligence will be as bright as lightning. If not used in a proper way, this may destroy them. Knowledge in various subjects could be acquired by those born under this number.\r" },
                { 64, "This number will create equal number of friends and foes. Opposition will be experienced in life. This gives extraordinary will power, intelligence and knowledge. This will bestow fame by enabling them to do things that are considered impossible. This ensures high position in the Government. At times, this will give such a high position that everyone will pay respect and hold these people in high esteem and awe. Their words would cast a powerful influence." },
                { 73, "This name number strengthens mental faculties and bestows fame, wealth and power. People having this number will aspire to lead comfortable lives and will accomplish their desires. Support from the people of authority will be available and material possessions will be in plenty. If they are not honest, they will lose their fame. If they are the spiritual types, they will lead a peaceful and comfortable life with pure hearts and noble thoughts.\r" },
                { 82, "This is one of the most powerful numbers and it can elevate even an ordinary person to the status of a ruler. Those having this number in their names are duty-conscious. With unceasing efforts, they will dominate the scene in any field they are placed in. They would own lands, gold mines and precious gems. They are lovers of high-bred horses and will attain the pinnacle of fame by making a fortune either in horse races, car races or in similar sports or business. They create unnecessary problems in their love matters and will be over-adamant in nature. Their eyes have magnetic powers. If the power of this number is properly understood and practiced, no physical or mental feat is impossible to perform.\r" },
                { 91, "This indicates strong determination and profitable journeys. They also undertake many journeys for trade or otherwise and will do all things with great vigour. Maritime trade using boats and ships will bring them plenty of wealth. They can attain success in breathing exercises like meditation or concentration. Comfortable living awaits them" },
                { 100, "Even though this number is capable of giving success in all efforts, it will not offer many opportunities. There will be plenty of money. This number implies a long and comfortable life, without any major achievements." },
                //2 for MOON
                { 11, "Those having 11 as their name number will come up in life by their sheer faith in God. They will proft by various means very easily. They may be riddled with unforeseen problems and dangers, as if to test their faith. Sometimes, they tend to meddle with matters that do not concern them. They are liable to be let down by their family and friends. If they have faith in God, they will definitely attain great heights in life. If they lack faith, they are bound to face a lot of dangers." },
                { 20, "This spiritual number represents a drumbeat heralding triumph or victory. People having this Name number work for liberation and social reforms. They are capable of providing relief to the masses from grief and struggles. The world will admire them. When they work with personal motives, they are extremely selfish and highly destructive. The 20 number people will excel in medical practice using toxic medicines and deal with poisonous drugs. These people possess the ability to awaken the sleeping masses and lead them to very great achievements. When they go out of their way to satisfy their selfish needs, delay and utter failure will be imminent. (HITLER, born on the 20th, spurred Germany into war and faced a humiliating defeat. He is a very good example of this Name number. He was represented by the battle drum that goaded the people to follow a selfish motive which led to destruction)" },
                { 29, "Those under this Name number often find it necessary to go to court to settle disputes. They will experience all sorts of problems in their families and will generally be let down by family and friends. Those who praised them yesterday may curse them today. These people live a life of mental agony and sorrow with their life partner. They get into deep troubles with the opposite sex. Any remedial measures taken to come out of these troubles may result in considerable delay and huge loss of money. The personal life will be full of ups and downs. Family life consists of events similar to the feats in a circus! Unless the name number is corrected properly, these people will encounter these problems orever." },
                { 38, "The people under this name number will be honest, peaceloving and gentle. When it is a name number of a person or a business, it will earn the help of the influential. This Name number can bring great success. People under this number will make rapid development and earn fame and wealth even from very humble beginnings. At times, they will face a lot of dangers and get cheated by bad people, resulting in various dificulties. Even their deaths will be sudden, rather unexpected and unusual.\r" },
                { 47, "Those who come up very fast in life can be seen amongst the people having this name number. They will be very much concerned about their own progress and will work out plans to achieve the same and will not rest until they reach their goal. As for as money matters are concerned, they will be very lucky and can be considered as very fortunate people. Many people in this number tend to lose their eyesight. Even the best of treatment may be in vain and they suffer very much. For those who have the habit of hunting, it would be better for them to abstain from hunting and eating flesh of any kind.\r" },
                { 56, "This number is full of wonders. Though this number tends to bring fortune and fame, it is the one that is used by those practising various forms of occultism and divination. This number can free a person from all ties and can break bondage of any kind. Locks would open. Even the animals inside the cage would find their way out. As too much of an explanation would be dangerous, I do not wish to pursue this subject any further. (As explained earlier, the number 29 represents powers of the body and mind, whereas Number 56 gives magical powers). These people will lose their wealth and fame all of a sudden." },
                { 65, "This number denotes divine grace and progress in spiritual life. It will earn the help of wealthy and powerful patrons. Marital life will be blissful. Persons under this Name number may be sometimes injured in accidents and may have cuts or bruises on their bodies.\r" },
                { 74, "These people have great affnity towards their religion. They run short of money often. They will introduce social and religious reforms and spread their principles. However, this is not a desirable number. This Name number is best suited only for hermits and priests and is not favourable to others. These people always remain worried about something or the other.\r" },
                { 83, "This Name Number bestows prestigious posts, which will earn the respect and adoration of many. They will achieve a life of splendour and authority. These people will be successful." },
                { 92, "This number signifies gold, silver, land, wealth and possession of yogic power. If people having this Name number can carefully practise the art of yogic breathing, they may even acquire the power of Astral projection (defying gravity) or Kechari Mu-dra (defying diseases and death)." },
                { 101, "Those under this Name number will have greater help from governments or the people of authority (than from their own efforts). There will be lots of obstacles in their business. Slump in business will be common. This cannot be considered a lucky number." },
                //3 for JUPITER
                { 3, "This name number denotes hard work, intelligence, success and a comfortable life. They will be highly educated and will gradually progress in life." },
                { 12, "These people naturally possess the ability to attract people by their power of speech. They sacrifice their lives for the welfare and happiness of others by shouldering their burdens too.\r" },
                { 21, "These people are self-centered and concerned about their own happiness and matters profitable to them. With great determination, they rise steadily in life and reach the pinnacle of success. Their tactful behaviour helps them to solve all their problems. They struggle hard in their early days but achieve success and happiness as they grow up. They will attain and retain good positions permanently in their lives." },
                { 30, "These people tend to live in a world of fantasy. They are wise thinkers. They like to do what they feel is right. At times, just for their own satisfaction they get involved in certain difficult tasks, without expecting any returns. They have less interest in making money. They know their minds and conquer the same easily. They gain mystic powers through mind control and related mental exercises.\r" },
                { 39, "These people are very sincere and hardworking. Invariably, the name and fame that are rightfully due to them will be enjoyed by others. They work unceasingly for the welfare of others. They are not as healthy as the other Number 3 people. At some stage in their lives they are prone to some kind of skin diseases." },
                { 48, "They will be more interested in religious matters, but face opposition in matters that involve the society at large. They will do a lot of work for public welfare, and create problems for themselves while attempting to do things beyond their capacity. Fate is against them most of the time.\r" },
                { 57, "This number gives victory or success in the beginning, but brings about gradual downfall and loss of interest in the end. Life which progresses at a very swift pace will grind to a halt all of a sudden. People named under this number will achieve great heights from humble beginnings but will later revert to their original positions.\r" },
                { 66, "This number denotes dynamism and oratorical skills, perfection in the fine arts, patronage from the government authorities and also a comfortable life.\r" },
                { 75, "All of a sudden they attain great fame. They will make good friends very soon. Unexpectedly, they become very popular. Fame and comforts will come in search of them. They become good poets and writers." },
                { 84, "Early days will be full of struggles and worries. They earn enemies unnecessarily. Travelling benefits them. They do not get rewards commensurate to their efforts. They improve themselves to some extent spiritually. Though generally lacking in enthusiasm at first, they can go to extremes, if need be! If the influence of their birth date is favourable, they can be great achievers.\r" },
                { 93, "These people are capable of doing marvelous things. They improve their worldly knowledge and are lucky to have their desires fulfilled. They excel in the field of histrionics through which they attain more fame. They earn through many business pursuits and lead very dignified lives.\r" },
                { 102, "This number signifies success at first ,followed by struggles and confusion. These people cannot be called lucky." },
                //4 for RAHU
                { 4, "As this name number will be only for very short names, it signifies a person or a thing that is popular. It does not bring luck as one might deserve. They will have needless fears, sickness and opposition. They can be well informed and worldlywise, but still they would work only as subordinates to others" },
                { 13, "People in the western counties regard this number as unlucky and ominous. They do not stay in rooms or houses having this number and some hotels even do not have rooms with this number. Unexpected events of sorrowful nature occur frequently. Men of this number have bitter experiences and face a lot of difficulties because of women. Though these people do manage to come up in life materially, they would still lead lives full of struggles. This is not a desirable name number. This number gives only severe grief, if birth and destiny numbers are also unlucky.\r" },
                { 22, "The characteristics of those born on the 22nd hold equally good for this Name number also. This number instigates base feelings and emotions. They are drawn towards gambling, drinking, speculation and other vices and will readily indulge in them. They may move towards self-destruction at a great speed and are generally surrounded by wicked and fraudulent people. They invariably earn a bad reputation. They dislike the counsel of others. If the influence of their date of birth is favourable, they can be successful. Otherwise, they have to struggle to prevent total failure. Those with selfish motives invariably urge these people to devious ways in order to further their own interests. They are good administrators and can meet any problem or difficulty with courage. Dangerous circumstances are foreseen. They often face humiliations." },
                { 31, "These people do not care for profit or loss but want only the freedom to do what they desire. Whatever may be the gain involved, they would not like to indulge in anything against their wish. They evince interest in astrology, philosophy and related sciences. These people do not care about what others do or say about them. They only wish to succeed and are never keen on the monetary benefits they gain from such successes. Having succeeded, they sometimes even forgo their due profits. By the 31st year they lose all their material possession and savings. They regain them only by the age of 37. Unexpected happenings will bring about major changes in life. Even their death will be sudden and abrupt. However, when death draws nearer, they somehow become intuitive and sense it well in advance. If their birth number is 1 of any month, this number helps them to achieve great positions in their official career.\r" },
                { 40, "Those under this number earn good friends, who will be of immense help to them in gaining jobs and positions of distinction. It provides accumulation of fine jewellery and wealth. It also brings fame and prosperity. Yet it is their negative qualities that are noticed by others. They can perform any work without any fear. Eventually, their lives will turn out to be fruitless and in vain. They will lose all their money. They will blame the society for not recognizing their services or help. Lot of problems will come up in their lives and the end will be pathetic.\r" },
                { 49, "This name number brings abundant riches. Their fame will spread far and wide and their achievements will be the envy of others. They lead highly eventful lives and travel a lot. Wonderful experiences, permanent prosperity, excellent properties and sudden fortunes will come to them. Accidents can also happen suddenly. If the birth number is a fortunate one, they lead happy lives. If not, they can end up being hated by others in the society and life will end in a tragic manner. This number kindles the power of imagination." },
                { 58, "This number gives outstanding popularity and the power to captivate others. They are great achievers. Life’s progress will be swift. They are pious and orthodox and are great reformers, though attached to religion. If their birth number is 4 or 8 of any month, they will hold positions of great responsibility and fame. They may be sometimes forced to carry out certain things against their wish. Outwardly, these people appear to be very lucky but they also have a lot of unwanted fears within. If the names of those born under other birth numbers also come under this number, life will slowly take a turn for the worse and they will lose their reputation. They may become selfish and may have to undergo a lot of difficulties during their lifetime." },
                { 67, "These people are exemplary artists (they may be artistes who perform) and work with great determination and vigour. They are patronized by power barons and they reveal noble ideas. (Men should control passions and lust for women.) Love, affection and grace make these people endearing. They can never achieve anything if they are selfish. This number, which helps to attract and conquer others, does not help non-artists." },
                { 76, "Those having this name number lose all their worldly possessions at some point of time. They are very popular. They will be successful in philanthropic deeds. Surprisingly, they make money in new ways. Income or material gains come through unexpected means. Their last years are generally spent in solitude, doing nothing but eating and sleeping.\r" },
                { 85, "This name number signifies those who come up the hard way. They not only overcome all afflictions, but help in solving others’ problems too. They reveal new ideas about religion and nature. They shine well in the field of medicine. They generally attain a position of distinction and honour." },
                { 94, "These people execute lots of good services for the sake of mankind in general. They bring reforms in society. Comfort and fame will come and go in their lives. Their fame and good work will generally be remembered even after their demise. This is a fortunate name number." },
                { 103, "This name number is also favourable. There will be improvement in material success initially, followed by a change in business. They will face a lot of competition. Later years will be pleasant and comfortable." },
                //5 for MERCURY
                { 5, "This number gives the power to charm people, exude dynamism and to lead a luxurious life enjoying fame and prominence. They spend money lavishly. These people should cultivate perseverance and concentration of mind.\r" },
                { 14, "This number is suitable for trade. Those having this Name number are always surrounded by a lot of people and things. They are successful in various trades and will meet a lot of friends. They may have strange problems and may face disappointments by trusting others. They may also face risk from thunder, lightning, water and fire. They undertake frequent travels. These people are advised to be careful while travelling in fast-moving vehicles. If the product in which they trade also comes under number 14, it will have excellent public patronage. The matters concerning love and marriage must be considered and reconsidered many times before a decision is taken. If not, these people may marry in haste and repent at leisure. This number can be called a very lucky number." },
                { 23, "This number is the luckiest of all Mercury (5) numbers. These people find success in all their endeavours. All their plans will succeed. They can achieve things which others won’t even imagine. Their accomplishments will astonish those around them. In spite of being such lucky people, if they do not strive hard, they will end up leading ordinary lives. Since they succeed in all their efforts, they will earn the patronage, respect, honour and favour of people in very high positions. Hence, these people are advised to keep up high standards and work out plans to achieve their goals. If not, they might end up leading lives of luxury and pomp devoid of any personal accomplishment. The positive types among them are the most-soughtafter type executives in governments or in private enterprises. The negative types devote time to sleep and daydreaming.\r" },
                { 32, "This number can attract a variety of people. They have a mass appeal and they come out with unique ideas and techniques even without prior experience. This potent and forceful number can make anyone a prominent person. If they lead their life following their intuition, life will be wonderful. If they listen to the advice of others, failures may recur one after another. This number is said to be the epitome of wisdom and intuition. They have aboveaverage intelligence and a witty manner of speaking. They will become geniuses. Ups and downs will be common in their lives. They will attain high positions in life and will be youthful in appearance even in old age.\r" },
                { 41, "This number denotes the qualities of charming and controlling. They are renowned achievers and they have high ideals. They are keen about their development and will be world-famous. When they become heady with success, they get into things or matters which are beyond their capability. The failures that could result from such situations will be cleverly hidden from the public scrutiny. They are found to lead successful lives." },
                { 50, "They are very intelligent people and analyse everything thoroughly. They excel in education. Some people will shine as good teachers. Some others use their intelligence to make money. They are lucky after the age of 50. Their life span will improve and they live longer.\r" },
                { 59, "Similar to persons belonging to number 50, these people are also research-minded. Their writings are full of humour and they would shine as “Humour Kings” among writers. They would become rich by writing and get excellent public support. Their aim will be to earn money. They will enjoy permanent fortunes. They may suffer from nervous diseases including paralysis. Hence, it is necessary for them to have good habits and keep themselves healthy.\r" },
                { 68, "This number is lucky to a certain extent. However, life that starts quite pleasantly may suddenly grind to a halt. They will get involved in schemes that they cannot execute and will be badly hurt. Their greed will spoil their career and life. The fortunes that came through an unforeseen stroke of luck may soon disappear. Hence, this number is not quite fortunate." },
                { 77, "This number denotes sincere effort, selfconfidence and hard work. Support from others brings in profits, fame and honour. Life will be very enchanting. They reap full benefits of this number only if they repose faith in god. They get chances to travel aboard.\r" },
                { 86, "This number denotes those who come up in a gradual manner and the hard way. They get what they deserve. They earn the favour and help of rich people. With the help so received, they will lead comfortable and happy lives. They will have good savings and lead happy lives.\r" },
                { 95, "This number signifies a disciplined life combined with daring events and honour. They are successful in trade and achieve distinction. By trading in a variety of new things, they amass wealth. They are excellent orators and will become popular in their line of business.\r" },
                { 104, "This number will bring success in life followed by unexpected changes. Though they can be good achievers, they can only earn fame and not money. In other words, they become popular but material success may be a far cry" },
                //6 for VENUS
                { 6, "This number signifies a peaceful life, a satisfied and contented mind and a good standard of living. Being a single digit, it does not have much power.\r" },
                { 15, "The determination to succeed in all the plans and earn money and the quest for achieving one’s ends are signifed by this number. Lust, revenge and malice may push the people of this Name number to a vile state of mind. They may become selfish gradually. A charming appearance and forceful speech will help them in achieving their interests. Though this name number is not one conducive to leading a virtuous life, it is one that is ideally suited for purely material success. An alluring personality, excellence in fine arts and a witty nature yield positive results in any activity that can generate huge profits. Many people come forward to help them in need. Life will be luxurious. If their birth number is also favourable, these people will attain fame, wealth and distinction in all aspects of life" },
                { 24, "Those named under this number 24 will receive many favours from the government. This number helps them to reach very high positions in their careers easily. They will marry those who are much higher in status and wealth. If the name number comes under 24, these people can be found progressing very fast in uniformed service like the Defense, Para-military forces and the Police. Even if they join as the lowest rank in any field, this number will help them to rise to very high position by its powerful vibrations." },
                { 33, "This number signifies simultaneous growth in divine grace and prosperity. Those with this name number with or without their knowledge attain spiritual enlightenment surprisingly. Along with divine grace, they are blessed with abundant wealth and properties like granaries, mills etc. They will have many luxurious things at their disposal. They may have everlasting wealth.\r" },
                { 42, "Those named under this number, even if they are poor at the beginning of their lives, will attain a very prominent rank or position in their careers. They may be greedy at times. They will have thrifty bent of mind and are smart in saving money. They hesitate to part with money even for their own comforts in life. Strength of mind and grace will flourish.\r" },
                { 51, "This is the most powerful of all numbers under six. It signifies sudden progress. Those who were just commoners yesterday will become popular and prominent to-day. Unusual circumstances will bring about an unexpected ascent in rank and status. Their body and mind will be bubbling with energy and become uncontrollable. They will be frequently lost in thought, or will become emotionally active and put in untiring efforts in their work. These people who are active in body and mind cannot sleep peacefully. They become restless like a caged lion. To be precise, these people will be ruled by an extra-ordinary energy or power of both body and mind. As there is a possibility of these people making enemies who could threaten their lives, it is advisable for them not to hurt others’ sentiments or feelings. This number is considered to be fortunate, as it signifes the accumulation of abundant wealth." },
                { 60, "This number signifies peace, prosperity, appreciation of fine arts, a balanced state of mind and wisdom. They are skilled conversationalists who can put forth very logical arguments. Their family life will be happy and idealistic. This is quite a fortunate number.\r" },
                { 69, "The person named under this number will be like an uncrowned king in any business they are involved in. They overtake others and retain their position safely by their own efforts. They are majestic in appearance, very prosperous and will achieve awe-inspiring status. Spurred by emotions, they are known to spend money lavishly for their self-satisfaction. They possess majesty and will lead extremely comfortable and luxurious lives. They are incomparable when it comes to charming others with their tact and speech.\r" },
                { 78, "They are the most righteous type among all the Number 6 people. They have a great leaning towards their religion and sometimes follow orthodox beliefs. They can become good poets and can bring the listeners under their spell. They are very generous and are fond of social service. They earn or inherit large sums of money very easily. But, if they are not careful, they could lose all their possessions except the Divine grace. Some of them attain success in occult practices and will be respected by one and all in society" },
                { 87, "This number can give mystic powers. Money will be earned by devious and illegal means. In case of a “negative swing”, this number makes people steal at midnight and helps them to charm snakes and tame animals. If birth and destiny numbers are not positive, this number could be related to criminals and bad people. So, the less we discuss, the better." },
                { 96, "This can give a combination of prosperity and higher education. All desires will be fulfilled. They can excel in the fine arts easily. Women will be charmed easily by these people. This is a fortunate number." },
                { 105, "This number can give fortunes, satisfactory environment, great fame and accumulation of wealth. It will beget good progeny.\r" },
                //7 for KETU
                { 7, "This name number represents high principles and virtuous qualities, which may flourish with divine grace. Unexpected changes will take place. Efforts will not produce the desired results." },
                { 16, "This number signifies speedy progress and a sudden downfall. Ancient texts depict this number by a picture showing the shattering of a tall tower and a king’s head with his crown falling from its top. This truth was proved when Japan (16) fell prey to the nuclear bombs. The Japanese Emperor was considered to be God-incarnate and his people would not even look at him from an elevated place. Unfortunately, the Americans bombed Japan stripping the Emperor of his status and the consequences are well known even today. If your name number is 16, it is better to change it to some other lucky number. This number induces new imaginative thoughts which will be refected in the writings of the person.\r" },
                { 25, "As this number gives good results in the end, it can be considered a good number. These people will undergo many trials in life. Every step in life will present problems and difficulties. The victory gained over such problems will give them selfconfidence, spiritual growth and the support of those around them. They are worldly-wise, known for clarity of thought and hence their actions will be well-planned. These people will establish ideals and standards for themselves and will adhere to them at all costs. Just as gold when cleansed of all impurities becomes shiny and precious, the lives of these people will end with respect and honour after many trials. (Note the life of Mahatma Gandhi who used to sign as M.K.Gandhi).\r" },
                { 34, "In a way, this number can also be called lucky. This number will openly display the best qualities and capabilities of these persons in an attractive manner. It improves their stature but cannot be considered as fortunate. If the birth number is also a favourable one, they can earn enormous wealth quite easily. If not, earning money itself will become a problem. There will be some problem in their family life. Most of the men will either be addicted to women or wine. Their minds easily succumb to sensuous pleasures. This is the most fearful aspect of this number. (Be cautious about it)! Hence, they are advised to change this Name number to a more fortunate one.\r" },
                { 43, "This is a strange number. Their whole life will be revolutionary. Whatever profession in which they are involved, this number produces new enemies. They have the tendency to resign from their jobs often. They will be constantly bringing out extreme ideas. They have extraordinary powers of imagination, speaking and writing as in the case of the other name numbers under 7. Their desires will be fulfilled at the end. This number, which is regarded as somewhat unlucky, indicates trials, great obstacles and revolutionary changes. They are sure to succeed in their ideals. (It is unlucky in the sense that they do not enjoy the luxuries or comforts of a peaceful life. Even their success will not yield them any personal gains). Their shrewdness increases with age, but they will encounter more criticism than praise for their capabilities and intelligence." },
                { 52, "This number also signifies some type of revolutionary qualities. If the birth number is favourable it could bring world renown. They readily offer a solution to any problem and can charm many. If they are on the spiritual path, they can attain great powers and immense popularity. All their desires will be fulfilled. They can bring about a new era in the lives of others. (This power can also be found in number 25 to a certain extent). Their end will come abruptly, leaving their work unaccomplished. Although their personal life will be fraught with problems, they are sure to be famous and favoured by all.\r" },
                { 61, "These people will quit a comfortable life and try new avenues according to their wishes. Successes and failures come in succession. If they can take care of their health, their later years will be quite fruitful helping them win prestigious posts. Though they may seem to be leading happy lives, in reality, they will be unhappy in their family lives. They spend much time in making new efforts and will achieve victory. They will earn great fame.\r" },
                { 70, "People represented by this number are of extreme nature. Their comfortable life gets disturbed by circumstances. There are frequent disappointments, failures and problems. But the later years will be fruitful, successful and filled with blessings. This number does not possess much power. If their destiny number is favourable, these people will be happy during their final years. If not, their problems may drown them in misery" },
                { 79, "People of this number tend to suffer very badly in the beginning of their lives due to many difficulties. Later, these people will rise quickly by their cleverness and sheer will power. They will settle down very comfortably and will succeed in their endeavours. They will have popular support, a comfortable life and will achieve enduring success. They become very fortunate with the passage of time and also become great personalities.\r" },
                { 88, "This number gives spiritual progress. They are generous and compassionate. They are affectionate to all creatures and will become popular" },
                { 97, "This number gives proficiency in the scriptures and fine arts. It also gives eminence in spiritual career. They will be successful in all their efforts and will be prosperous due to their astounding achievements in chosen fields." },
                { 106, "There will be drastic changes in life. They will experience many problems during their middle age. Their later years will be comfortable. They get into big troubles that cannot be solved easily. This number is not a lucky one. Greed for worldly things will supersede the interest in seeking divine grace.\r" },
                //8 for SATURN
                { 8, "This Name number gives you great success in spiritual life. If they have no control over pleasures, success will delayed. After a big struggle they may succeed. They will have to face unexpected dangers and difficult circumstances in life.\r" },
                { 17, "This name number gives demonic qualities while pursuing the goals. It brings many problems and trials. However, they will persistently struggle without giving up. Failures will prompt them to struggle more actively. In the end they will be successful, and they get permanent prosperity and great fame. Some of them risk their lives to attain their goals and so achieve progress. The world can never forget them. This number can give mystic powers also." },
                { 26, "This number denotes poverty in old age and fruitless efforts. Those who have this name number undergo great losses due to friends and partners. Circumstances lead them to failure and confusion. This number reduces one’s span of life and earns enemies who may even go to the extent of murdering them. Those with Name numbers 26 at frst begin their life with great principles and later change their minds and end up only in pursuit of money and status, (Based on their general qualities of number 8, they will be a little more fortunate in their later years)." },
                { 35, "This number outwardly seems to be fortunate but the person will suffer losses because of friends and associates. These people will become very rich and popular but later lose all their money. Unexpected accidents may happen. This number helps in earning money through illegal means. Today’s friends will become tomorrow’s foes. They are very fickle- minded. Expenses will mount. This number would create severe incurable pain in the stomach. (For those with heart problems, this Name number will be a curative factor). Those with this name number must be very careful in their large business endeavours." },
                { 44, "This number helps in earning money easily. Industries involving many people, like cinema theatres, printing presses, coal and iron mining, painting, making of furniture and sports goods, and organizing contests will help them earn a good income. Hiring out vehicles like buses, trucks and cars will also be rewarding. They can also run banks. One day everything may come to a halt. Only the owner will enjoy the profits in a proprietary concern. There is danger from fire and collapse of building. This number indicates that they may have to spend some time in prison. Their minds will go astray towards bad ways. Their lives may be comfortable outside prison. Generally, either they suffer from some disease or spend some time in prison, especially when the birth and destiny numbers are also not harmonious.\r" },
                { 53, "They experience success and failure in the beginning of life itself. As they grow older, their lives will become steadier and they will become well-known. Though they are intelligent, they are bound to get into problems beyond their control. These people, who can convert failure into success, will perform good deeds and earn prestige and popularity. (This is an unstable number. Only if the birth number is favourable, will it bring desirable effects). They will be lucky in their old age.\r" },
                { 62, "Generally it will give great fame, victories and a comfortable life. At times, great dangers and failures alternatively affect them. It could bring about serious enmity. It also causes misunderstanding among relatives. Family life will not be pleasant. Intellectual faculties will improve. These people can charm everyone easily. This name number helps in charming enemies too!" },
                { 71, "This number which brings about obstacles initially will later shower prosperity. They will be good counsellors to others because of their intelligence. This number may be considered fortunate." },
                { 80, "This number has strange mystic powers, but may lead to grave dangers if the date of birth is not favourable. Research in theology will be successful. Nature will change its course and help them. Though their lives are full of dangers and anxieties, they will be comfortable. Miracles will happen. It is a fortunate number." },
                { 89, "This number, which also signifies benefits, brings problems initially. They have a helping tendency and they will acquire great riches like land, houses and jewellery. Women are attracted to them easily. Society will respect the women of this Name number. They have a combination of beauty and wealth. They lead fearless lives with the help of their great power of speech and action. Initially, fire accidents may occur in their lives." },
                { 98, "Like the people under Number 71, these people are also intelligent. But their lives are filled with worries and desires. Though they are intelligent, they may not benefit from that quality. Difficulties and chronic diseases may affect them.\r" },
                { 107, "This number will bring fame and success. If they are men, they will have problems due to women and if they are women, it will be from men. Even if they attain wealth, life will not be comfortable. However, they will be famous and influential.\r" },
                //9 for MARS
                { 9, "If the name comes under this number, it signifies wisdom and capability. It also denotes travel, struggles against odd situations and victory in the end. When they finally succeed, they will have a long life of luxury." },
                { 18, "This name number, which signifies the decline of divinity, will bring in problems, procrastination in all matters, deviousness, and dangerous enemies. Their selfishness may induce them to indulge in antisocial activities. They follow evil ways consciously and become highly selfish. Life devoid of peace and rest will be the order of the day. This number signifies growth of personal desires at the cost of virtue. Desires will come to an end. Divine grace will get destroyed. (Hindu Epics record that Mahabharat war was fought for 18 days and 18 divisions fought the war in which the Pandavas had to kill their own elders and gurus in the battle. Although considered holy by the Hindus, it is the eighteen-chaptered Bhagavat Gita that spurred Arjuna into the war). This number, which denotes jealousy, malice and dangers due to fire and weapons, is not considered desirable." },
                { 27, "This number signifies a clear mind and intelligence, unceasing hard work, accumulation of wealth, all round influence and positions of prominence and high rank. Especially in uniformed services like police, army, etc., they will rise very high in their ranks. This is as fortunate as number 24, which has been explained earlier in this book. They will be respected and treated as the best in their profession or service. They like to do social service and will be involved also in matters that would benefit them. This is a very fortunate number that brings spirituality and magical powers." },
                { 36, "This number can raise even poor people to an enviable status and make them live in mansions. Only when these people go away from their place of birth to distant regions, do they attain success. They will travel extensively and occupy high positions. This number, though it appears very fortunate, will cause problems within the family. They may be surrounded by disloyal people.\r" },
                { 45, "This is a lucky Name number. Even those who struggle at lower levels will be raised to a higher status and positions. They are good conversationalists and will be found in gatherings that involve entertaining people. These hard-working people will earn outstanding places in their career. They will achieve their goals at any cost. People would wonder if their life was a big show. Even though they may have nagging problems, they will retain their smile and will never allow anyone to know their problems. This number, which assures a comfortable life, fame and wealth, is a desirable one. Diseases will also be cured." },
                { 54, "This number will give success step by step. Failures can also happen. They may begin their lives with prestige, reputation and prosperity. Stubbornness and thoughtless decisions will make them lose their name and fame. Greed is their worst enemy that could make them lose all their wealth if they are not careful. In the fag end of their lives, they may achieve success. Their life will be without freedom and they will be under the control of others.\r" },
                { 63, "This is also a lucky number. However, if this comes as a Name number, it will lead one into wrong ways. The ancient texts describe this number as one related to thieves; so the less said, the better!\r" },
                { 72, "This is the best of all numbers under 9. Although these people struggle in their early years, they later enjoy life with all comforts. A mind devoid of doubt will be filled with joy. The wealth acquired by these people will remain intact in their family for many future generations. Money keeps on coming continuously, without fail. (Businessmen should take note of this advantageous number). It will also bring repute. This number signifies permanent wealth." },
                { 81, "This number signifies a fortunate life. This will give development, good position and wealth. If these people are not careful, their luck could change for the worse. They will have opportunities to become teachers." },
                { 90, "As this number derives its full power from number 9, its people will go to any extent to get their desires fulfilled. Victory will be certain. They will become very wealthy and famous. For those who are interested in spiritual pursuits this number is not desirable.\r" },
                { 99, "This Name number will lure its native to devious ways. Success will come along with enmity. This number signifies being attacked by enemies, and hence it is not a good number. (However they will be blessed with education, wealth and prosperity).\r" },
                { 108, "This number can give high positions and success. Everything will happen according to their desire. As this number induces its people to make good efforts resulting in success, it is a very lucky number" },
            };

            //based on name number get predictions
            //check if exists first
            var predictionExist = nameNumberPredictions.ContainsKey(nameNumber);

            //only if number exist
            if (predictionExist) { return nameNumberPredictions[nameNumber]; }

            //let caller know fail
            return $"NO Prediction for name number {nameNumber}";
        }

        #endregion

        #region AVASTA

        /// <summary>
        /// Gets all the Avastas for a planet, Lajjita, Garvita, Kshudita, etc...
        /// </summary>
        /// <param name="time">time to base calculation on</param>
        public static List<Avasta> PlanetAvasta(PlanetName planetName, Time time)
        {
            var finalList = new Avasta?[6]; //total 6 avasta

            //add in each avasta that matches
            finalList[0] = IsPlanetInLajjitaAvasta(planetName, time) ? Avasta.LajjitaShamed : null;
            finalList[1] = IsPlanetInGarvitaAvasta(planetName, time) ? Avasta.GarvitaProud : null;
            finalList[2] = IsPlanetInKshuditaAvasta(planetName, time) ? Avasta.KshuditaStarved : null;
            finalList[3] = IsPlanetInTrashitaAvasta(planetName, time) ? Avasta.TrishitaThirst : null;
            finalList[4] = IsPlanetInMuditaAvasta(planetName, time) ? Avasta.MuditaDelighted : null;
            finalList[5] = IsPlanetInKshobhitaAvasta(planetName, time) ? Avasta.KshobitaAgitated : null;

            // Convert array to List<Avasta> and remove nulls
            var resultList = finalList.OfType<Avasta>().ToList();
            return resultList;

        }

        /// <summary>
        /// Lajjita / humiliated : Planet in the 5th house in conjunction with rahu or ketu, Saturn or mars.
        /// </summary>
        /// <param name="time">time to base calculation on</param>
        public static bool IsPlanetInLajjitaAvasta(PlanetName planetName, Time time)
        {
            //check if input planet is in 5th
            var isPlanetIn5thHouse = IsPlanetInHouse(planetName, HouseName.House5, time);

            //check if any negative planets is in 5th (conjunct)
            var planetNames = new List<PlanetName>() { Rahu, Ketu, Saturn, Mars };
            var rahuKetuSaturnMarsIn5th = IsAllPlanetInHouse(planetNames, HouseName.House5, time);

            //check if all conditions are met Lajjita
            var isLajjita = isPlanetIn5thHouse && rahuKetuSaturnMarsIn5th;

            return isLajjita;

        }

        /// <summary>
        /// Garvita, proud : Planet in exaltation sign or moolatrikona zone, happiness and gains
        /// </summary>
        /// <param name="time">time to base calculation on</param>
        public static bool IsPlanetInGarvitaAvasta(PlanetName planetName, Time time)
        {
            //Planet in exaltation sign
            var planetExalted = IsPlanetExalted(planetName, time);

            //moolatrikona zone
            var planetInMoolatrikona = IsPlanetInMoolatrikona(planetName, time);

            //check if all conditions are met for Garvita
            var isGarvita = planetExalted || planetInMoolatrikona;

            return isGarvita;
        }

        /// <summary>
        /// Kshudita, hungry : Planet in enemy’s sign or conjoined with enemy or aspected by enemy, Grief
        /// </summary>
        public static bool IsPlanetInKshuditaAvasta(PlanetName planetName, Time time)
        {
            //Planet in enemy’s sign 
            var planetExalted = IsPlanetInEnemyHouse(planetName, time);

            //conjoined with enemy (same house)
            var conjunctWithMalefic = IsPlanetConjunctWithEnemyPlanets(planetName, time);

            //aspected by enemy
            var aspectedByMalefic = IsPlanetAspectedByEnemyPlanets(planetName, time);

            //check if all conditions are met for Kshudita
            var isKshudita = planetExalted || conjunctWithMalefic || aspectedByMalefic;

            return isKshudita;
        }

        /// <summary>
        /// Trashita, thirsty – Planet in a watery sign, aspected by a enemy and is without the aspect of benefic Planets
        /// 
        /// The Planet who being conjoined or aspected by a Malefic or his enemy Planet is situated,
        /// without the aspect of a benefic Planet, in the 4th House is Trashita.
        /// 
        /// Another version
        /// 
        /// If the Planet is situated in a watery sign, is aspected by an enemy Planet and
        /// is without the aspect of benefic Planets he is called Trashita.
        ///
        /// --------
        /// "A planet in a Water Sign and aspected by an enemy planet,
        /// with no auspiscious Graha aspecting is said to be Trishita Avastha/Thirsty State".
        /// 
        /// This state is in effect whenever a planet is in a Water Sign and it gets
        /// aspected by an enemy planet. But if, a Gentle Planet (Mercury/Venus/Moon) aspects here,
        /// it strengthens the planet in Water Sign. This Avastha is only for the aspecting enemy
        /// planet that will cause Trishita/Thirst. This state shows that a planet in a watery
        /// Rasi can still be productive even when aspected by enemies, though it will not be happy.
        /// As the name “Thirsty State” implies, it indicates the lack of emotional fulfillment that a planet experiences.
        /// </summary>
        public static bool IsPlanetInTrashitaAvasta(PlanetName planetName, Time time)
        {
            //Planet in a watery sign
            var planetInWater = IsPlanetInWaterySign(planetName, time);

            //aspected by an enemy
            var aspectedByEnemy = IsPlanetAspectedByEnemyPlanets(planetName, time);

            //no benefic planet aspect
            var noBeneficAspect = false == IsPlanetAspectedByBeneficPlanets(planetName, time);

            //check if all conditions are met for Trashita
            var isTrashita = planetInWater && aspectedByEnemy && noBeneficAspect;

            return isTrashita;
        }

        /// <summary>
        /// The Planet who is in his friend’s sign, is in conjunction with Jupiter,
        /// and is together with or is aspected by a friendly Planet is called Mudita
        /// 
        /// Mudita, sated, happy – Planet in a friend’s sign or aspected by a friend and conjoined with Jupiter, Gains
        ///
        /// If a planet is in a friend’s sign or joined with a friend or aspected by a friend,
        /// or that joined with Jupiter is called Mudita Avastha/Delighted State
        ///
        /// It is clear from explanation itself that a planet will feel delighted when it
        /// is in friendly sign or friendly planet conjuncts/aspects or it is joined by the
        /// biggest benefic planet Jupiter. We can understand planet’s delight in such cases. 
        /// 
        /// Planet in friendly sign - A planet in a friendly sign is productive,
        /// and the stronger that friend planet, the more productive it will be. 
        /// </summary>
        public static bool IsPlanetInMuditaAvasta(PlanetName planetName, Time time)
        {
            //Planet who is in his friend’s sign
            var isInFriendly = IsPlanetInFriendHouse(planetName, time);

            //is in conjunction with Jupiter
            var isConjunctJupiter = IsPlanetConjunctWithPlanet(planetName, Jupiter, time);

            //is together with or is aspected by a friendly (conjunct or aspect)
            var isConjunctWithFriendly = IsPlanetConjunctWithFriendPlanets(planetName, time);
            var isAspectedByFriendly = IsPlanetAspectedByFriendPlanets(planetName, time);
            var accosiatedWithFriendly = isConjunctWithFriendly || isAspectedByFriendly;

            //check if all conditions are met for Mudita
            var isMudita = isInFriendly || isConjunctJupiter || accosiatedWithFriendly;

            return isMudita;
        }

        /// <summary>
        /// If a planet is conjunct by Sun or it is aspected by Enemy Malefic Planets then
        /// it should always be known as Kshobhita Avastha/Agitated State
        /// 
        /// Kshobhita, guilty, repentant – Planet in conjunction with sun and aspected by malefics and an enemy. Penury
        /// </summary>
        public static bool IsPlanetInKshobhitaAvasta(PlanetName planetName, Time time)
        {
            //Planet in conjunction with sun 
            var conjunctWithSun = IsPlanetConjunctWithPlanet(planetName, Sun, time);

            //aspected by an enemy or malefic
            var isAspectedByEnemy = false == IsPlanetAspectedByEnemyPlanets(planetName, time);
            var isAspectedByMalefics = false == IsPlanetAspectedByMaleficPlanets(planetName, time);
            var accosiatedWithBadPlanets = isAspectedByEnemy || isAspectedByMalefics;

            //check if all conditions are met for Kshobhita
            var isKshobhita = conjunctWithSun && accosiatedWithBadPlanets;

            return isKshobhita;
        }

        #endregion

        #region PLANET TRANSITS

        public static List<Tuple<Time, Time, ZodiacName, PlanetName>> PlanetSignTransit(Time startTime, Time endTime, PlanetName planetName)
        {
            //make slices to scan
            var accuracyInHours = 0.05; // 3 minute
            var timeSlices = Time.GetTimeListFromRange(startTime, endTime, accuracyInHours);

            //prepare place to store data
            var returnList = new List<Tuple<Time, Time, ZodiacName, PlanetName>>();

            //get the start sign
            var startZodiacSign = Calculate.PlanetZodiacSign(planetName, startTime);
            var previousZodiacName = startZodiacSign.GetSignName();
            var startTimeSlice = timeSlices[0]; //set start slice for 1st change
            foreach (var timeSlice in timeSlices)
            {
                var tempZodiacName = Calculate.PlanetZodiacSign(planetName, timeSlice).GetSignName();

                //if constellation changes mark the time as start for one and end for another
                if (tempZodiacName != previousZodiacName)
                {
                    //add previous, with current slice as end time
                    returnList.Add(new Tuple<Time, Time, ZodiacName, PlanetName>(startTimeSlice, timeSlice, previousZodiacName, planetName));

                    //save current slice as start for next
                    startTimeSlice = timeSlice;
                }

                //update value for next check
                previousZodiacName = tempZodiacName;
            }

            return returnList;

        }

        /// <summary>
        /// Gets all the constellation start time for a given planet
        /// Set to an accuracy of 1 minute
        /// </summary>
        public static List<Tuple<Time, ConstellationName, ZodiacSign>> GetConstellationTransitStartTime(Time startTime, Time endTime, PlanetName planetName)
        {
            //make slices to scan
            var accuracyInHours = 0.05; // 3 minute
            var timeSlices = Time.GetTimeListFromRange(startTime, endTime, accuracyInHours);

            var returnList = new List<Tuple<Time, ConstellationName, ZodiacSign>>();

            var startConstellation = Calculate.PlanetConstellation(planetName, startTime);
            var previousConstellation = startConstellation.GetConstellationName();

            foreach (var timeSlice in timeSlices)
            {
                //if constellation changes mark the time
                var tempConstellationName = Calculate.PlanetConstellation(planetName, timeSlice).GetConstellationName();

                //CPJ Added for Planet's Zodiac Sign
                var planetLongitude = Calculate.PlanetNirayanaLongitude(planetName, timeSlice);
                var planetZodiacSign = Calculate.ZodiacSignAtLongitude(planetLongitude);
                //-------

                if (tempConstellationName != previousConstellation)
                {
                    returnList.Add(new Tuple<Time, ConstellationName, ZodiacSign>(timeSlice, tempConstellationName, planetZodiacSign));
                }

                //update value for next check
                previousConstellation = tempConstellationName;
            }

            return returnList;
        }

        #endregion

        #region ALL DATA

        /// <summary>
        /// Niryana Constellation of all 9 planets
        /// </summary>
        public static Dictionary<PlanetName, Constellation> AllPlanetConstellation(Time time) => All9Planets.ToDictionary(planet => planet, planet => PlanetConstellation(planet, time));

        /// <summary>
        /// Gets all possible calculations for a given Time
        /// </summary>
        /// <param name="time">can be birth or query time</param>
        public static List<APIFunctionResult> AllTimeData(Time time)
        {
            //exclude this method from getting included in "Find" and Execute below
            MethodBase method = MethodBase.GetCurrentMethod();
            MethodInfo methodToExclude = method as MethodInfo;

            //do calculation
            var raw = AutoCalculator.FindAndExecuteFunctions(methodToExclude, time);

            return raw;
        }

        /// <summary>
        /// Gets all possible calculations for a Planet at a given Time
        /// </summary>
        /// <param name="time">can be birth or query time</param>
        public static List<APIFunctionResult> AllPlanetData(PlanetName planetName, Time time)
        {
            //exclude this method from getting included in "Find" and Execute below
            MethodBase method = MethodBase.GetCurrentMethod();
            MethodInfo methodToExclude = method as MethodInfo;

            //do calculation
            var raw = AutoCalculator.FindAndExecuteFunctions(methodToExclude, planetName, time);

            return raw;
        }

        /// <summary>
        /// All possible calculations for a House at a given Time
        /// </summary>
        /// <param name="time">can be birth or query time</param>
        public static List<APIFunctionResult> AllHouseData(HouseName houseName, Time time)
        {
            //exclude this method from getting included in "Find" and Execute below
            MethodBase method = MethodBase.GetCurrentMethod();
            MethodInfo methodToExclude = method as MethodInfo;

            //do calculation
            var raw = AutoCalculator.FindAndExecuteFunctions(methodToExclude, houseName, time);

            return raw;
        }

        /// <summary>
        /// All possible calculations for a Planet and House at a given Time
        /// </summary>
        /// <param name="time">can be birth or query time</param>
        public static List<APIFunctionResult> AllPlanetHouseData(PlanetName planetName, HouseName houseName, Time time)
        {
            //exclude this method from getting included in "Find" and Execute below
            MethodBase method = MethodBase.GetCurrentMethod();
            MethodInfo methodToExclude = method as MethodInfo;

            //do calculation
            var raw = AutoCalculator.FindAndExecuteFunctions(methodToExclude, planetName, houseName, time);

            return raw;
        }

        /// <summary>
        /// All possible calculations for a Zodiac Sign at a given Time
        /// </summary>
        /// <param name="time">can be birth or query time</param>
        public static List<APIFunctionResult> AllZodiacSignData(ZodiacName zodiacName, Time time)
        {
            //exclude this method from getting included in "Find" and Execute below
            MethodBase method = MethodBase.GetCurrentMethod();
            MethodInfo methodToExclude = method as MethodInfo;

            //do calculation
            var raw = AutoCalculator.FindAndExecuteFunctions(methodToExclude, zodiacName, time);

            return raw;
        }

        #endregion

        #region GENERAL

        /// <summary>
        /// As the name suggests, Ghataka chakra is seen for any kind of injuries,
        /// may it be Physical or Mental. The injuries can be inflicted at an inopportune
        /// moment or by an inimical person. Both of these can be seen from the Ghataka Chakra.
        /// For the first instance, the inopportune time can be seen from the horoscope of the
        /// moment of occurance of the event and for the latter case, the same can be seen from
        /// the horoscope of the person inflicting pain and injury.
        /// </summary>
        public static List<string> GhatakaChakra(Time time, Time birthTime)
        {
            //below table of all possible combinations
            var GhatakaChakraTable = new Dictionary<ZodiacName, GhatakaRow>()
            {
                { ZodiacName.Aries, new GhatakaRow(ZodiacName.Aries, LunarDayGroup.Nanda, VedAstro.Library.DayOfWeek.Sunday, ConstellationName.Makha, ZodiacName.Aries, ZodiacName.Libra)},
                { ZodiacName.Taurus, new GhatakaRow(ZodiacName.Virgo, LunarDayGroup.Purna, VedAstro.Library.DayOfWeek.Saturday, ConstellationName.Hasta, ZodiacName.Taurus, ZodiacName.Scorpio)},
                { ZodiacName.Gemini, new GhatakaRow(ZodiacName.Aquarius,LunarDayGroup.Bhadra , VedAstro.Library.DayOfWeek.Monday , ConstellationName.Swathi ,ZodiacName.Cancer,ZodiacName.Capricorn)},
                { ZodiacName.Cancer, new GhatakaRow(ZodiacName.Leo,LunarDayGroup.Bhadra ,VedAstro.Library.DayOfWeek.Wednesday ,ConstellationName.Anuradha,ZodiacName.Libra,ZodiacName.Aries)},
                { ZodiacName.Leo, new GhatakaRow(ZodiacName.Capricorn,LunarDayGroup.Jaya ,VedAstro.Library.DayOfWeek.Saturday ,ConstellationName.Moola,ZodiacName.Capricorn,ZodiacName.Cancer)},
                { ZodiacName.Virgo, new GhatakaRow(ZodiacName.Gemini,LunarDayGroup.Purna ,VedAstro.Library.DayOfWeek.Saturday ,ConstellationName.Sravana,ZodiacName.Pisces,ZodiacName.Virgo)},
                { ZodiacName.Libra, new GhatakaRow(ZodiacName.Sagittarius,LunarDayGroup.Rikta,VedAstro.Library.DayOfWeek.Thursday,ConstellationName.Satabhisha,ZodiacName.Virgo,ZodiacName.Pisces)},
                { ZodiacName.Scorpio, new GhatakaRow(ZodiacName.Taurus,LunarDayGroup.Nanda,VedAstro.Library.DayOfWeek.Friday,ConstellationName.Revathi,ZodiacName.Taurus,ZodiacName.Scorpio)},
                { ZodiacName.Sagittarius, new GhatakaRow(ZodiacName.Pisces,LunarDayGroup.Jaya,VedAstro.Library.DayOfWeek.Friday,ConstellationName.Aswini,ZodiacName.Sagittarius,ZodiacName.Gemini)},
                { ZodiacName.Capricorn, new GhatakaRow(ZodiacName.Leo,LunarDayGroup.Rikta,VedAstro.Library.DayOfWeek.Tuesday,ConstellationName.Rohini,ZodiacName.Aquarius,ZodiacName.Leo)},
                { ZodiacName.Aquarius, new GhatakaRow(ZodiacName.Sagittarius,LunarDayGroup.Jaya,VedAstro.Library.DayOfWeek.Thursday,ConstellationName.Aridra,ZodiacName.Gemini,ZodiacName.Sagittarius)},
                { ZodiacName.Pisces, new GhatakaRow(ZodiacName.Aquarius,LunarDayGroup.Purna,VedAstro.Library.DayOfWeek.Thursday,ConstellationName.Aslesha,ZodiacName.Leo,ZodiacName.Aquarius)}
            };

            //get janma rasi
            var janmaRasi = Calculate.MoonSignName(birthTime);

            //get data points that could make this happen at check time
            ZodiacName moonSign = Calculate.MoonSignName(time);
            LunarDayGroup tithiGroup = Calculate.LunarDay(time).GetLunarDayGroup();
            DayOfWeek vedicDay = Calculate.DayOfWeek(time);
            ConstellationName moonConstellation = Calculate.MoonConstellation(time).GetConstellationName();
            ZodiacName lagna = Calculate.HouseZodiacSign(HouseName.House1, time).GetSignName();

            //add any to list, can occur more than 1
            var foundGhataka = new List<string>();

            //if any one of the above data points match with any one GhatakRow then add to list
            //can be more than 1 added
            if (GhatakaChakraTable.TryGetValue(janmaRasi, out GhatakaRow ghatakaRow))
            {
                //check each and add
                if (ghatakaRow.MoonSign == moonSign) { foundGhataka.Add(nameof(ghatakaRow.MoonSign)); }
                if (ghatakaRow.TithiGroup == tithiGroup) { foundGhataka.Add(nameof(ghatakaRow.TithiGroup)); }
                if (ghatakaRow.WeekDay == vedicDay) { foundGhataka.Add(nameof(ghatakaRow.WeekDay)); }
                if (ghatakaRow.MoonConstellation == moonConstellation) { foundGhataka.Add(nameof(ghatakaRow.MoonConstellation)); }
                if (ghatakaRow.LagnaSameSex == lagna) { foundGhataka.Add(nameof(ghatakaRow.LagnaSameSex)); }
            }

            //return list
            return foundGhataka;
        }

        /// <summary>
        /// Gets total hours in a vedic day, that is duration from sunset to sunrise
        /// NOTE: does not account if birth time is outside sunrise & sunset range
        /// </summary>
        public static double DayDurationHours(Time time)
        {
            var sunset = Calculate.SunsetTime(time);
            var sunrise = Calculate.SunriseTime(time);

            var totalHours = sunset.Subtract(sunrise).TotalHours;
            return totalHours;
        }

        /// <summary>
        /// A day starts at the time of sunrise and ends at the time of sunset. A
        /// night starts at the time of sunset and ends at the time of next day's sunrise.
        /// </summary>
        public static bool IsNightBirth(Time birthTime)
        {
            //get sunset time for that day
            var sunset = Calculate.SunsetTime(birthTime);

            //get next day sunrise time
            var nextDay = birthTime.AddHours(23);
            var sunriseNextDay = Calculate.SunriseTime(nextDay);

            //check if given birth time is within this time frame
            var xx = birthTime >= sunset;
            var cc = birthTime <= sunriseNextDay;

            //if so then night birth!
            return xx && cc;
        }

        /// <summary>
        /// A day starts at the time of sunrise and ends at the time of sunset. A
        /// night starts at the time of sunset and ends at the time of next day's sunrise.
        /// </summary>
        public static bool IsDayBirth(Time birthTime)
        {
            //get sunrise time for that day
            var sunrise = Calculate.SunriseTime(birthTime);

            //get day sunset time
            var sunset = Calculate.SunsetTime(birthTime);

            //check if given birth time is within this time frame
            var xx = birthTime >= sunrise;
            var cc = birthTime <= sunset;

            //if so then day birth!
            return xx && cc;
        }

        /// <summary>
        /// Easyly import Jaganath Hora (.jhd) files into VedAstro.
        /// Yeah! Competition drives growth!
        /// </summary>
        public static Person ParseJHDFiles(string personName, string rawTextData)
        {
            // Split the raw text data into an array of strings
            var lines = rawTextData.Trim().Split('\n');

            // Extract the date and time parts
            var hoursTotalDecimal = double.Parse(lines[3]);
            // Extract the whole number part for hours
            var hours = (int)hoursTotalDecimal;
            // Get the fractional part and convert it to minutes
            double fractionalPart = hoursTotalDecimal - hours;
            var minutes = (int)Math.Round(fractionalPart * 100);

            //extract out the date parts
            var month = int.Parse(lines[0]);
            var day = int.Parse(lines[1]);
            var year = int.Parse(lines[2]);
            var timeZoneSpan = ConvertRawTimezoneToTimeSpan(lines[4]);

            // Format the date and time text
            DateTimeOffset parsedStdTime = new DateTimeOffset(year, month, day, hours, minutes, 0, 0, timeZoneSpan);
            var dateTimeText = parsedStdTime.ToString(Time.DateTimeFormat);

            // Extract the location and coordinates
            var locationRaw = $"{lines[12].Trim()}, {lines[13].Trim()}";//remove trailing white spaces
            var locationName = Regex.Replace(locationRaw, "[^a-zA-Z0-9 ,]", "");
            var rawLongitude = lines[5];
            var longitude = ConvertRawLongitude(rawLongitude);
            var latitude = double.Parse(lines[6]);
            var parsedLocation = new GeoLocation(locationName, longitude, latitude);

            var birthTime = new Time(parsedStdTime, parsedLocation);

            //extract gender
            var genderRaw = int.Parse(lines[17].Trim());
            var parsedGender = genderRaw == 1 ? Gender.Male : Gender.Female; //female is 2

            //combine all into 1 person
            var person = new Person(personName, birthTime, parsedGender);

            return person;

            // Converts input to a TimeSpan representing UTC offset.
            // If input is “-5.300000/r” it converts to "+05:30"
            // but if “5.300000/r” it converts to "-05:30"
            static TimeSpan ConvertRawTimezoneToTimeSpan(string input)
            {
                // Remove the "/r" from the end of the string
                string cleanedInput = input.Replace("/r", "");

                // Split the string into hours and minutes
                string[] timeParts = cleanedInput.Split('.');

                // Convert the string parts to integers
                int hours = int.Parse(timeParts[0]);
                int minutes = int.Parse(timeParts[1].Substring(0, 2)); // Get the first two digits of the decimal part

                // Reverse the sign of the hours
                hours = -hours;

                // Convert the hours and minutes to a TimeSpan
                TimeSpan timeSpan = new TimeSpan(hours, minutes, 0);

                return timeSpan;
            }

            //Converts a raw longitude string to a double and changes its sign.
            //EXP: "-77.350000\r" to 77.35, "108.350000\r" to -108.35
            static double ConvertRawLongitude(string rawLongitude)
            {
                // Trim the string to remove leading and trailing white spaces
                string trimmedLongitude = rawLongitude.Trim();

                // Try to parse the string to a double
                if (double.TryParse(trimmedLongitude, out double longitude))
                {
                    // If the longitude is negative, make it positive. If it's positive, make it negative.
                    longitude = longitude < 0 ? Math.Abs(longitude) : -Math.Abs(longitude);
                    return longitude;
                }
                else
                {
                    throw new FormatException("Invalid format for longitude.");
                }
            }

        }



        /// <summary>
        /// Gets all houses owned by a planet at a given time 
        /// </summary>
        public static List<HouseName> HousesOwnedByPlanet(PlanetName inputPlanet, Time time)
        {
            //Given a planet, return Zodiac Signs owned by it Ex. Ju, returns Sag an Pis
            var signsOwned = Calculate.ZodiacSignsOwnedByPlanet(inputPlanet);

            //given a Zodiac Sign, return, House Number (or Cusp number as its actually called)
            var houseList = new List<HouseName>();


            //get signs of all houses
            var houseSigns = Calculate.AllHouseSign(time);


            foreach (var zodiacName in signsOwned)
            {

                //get all houses that have inputed zodiac name
                List<HouseName> matchingHouses = houseSigns
                    .Where(pair => pair.Value.GetSignName() == zodiacName)
                    .Select(pair => pair.Key)
                    .ToList();

                //add to return list
                houseList.AddRange(matchingHouses);
            }


            //Next present these houses as Owned by a planet.
            return houseList;
        }

        /// <summary>
        /// Given a sign name and time will get the house that it is in, based on middle longitude.
        /// </summary>
        public static HouseName HouseFromSignName(ZodiacName zodiacName, Time inputTime)
        {
            //get signs for all houses
            //TODO cache down
            var houses = Calculate.AllHouseSign(inputTime);

            //pick out and return only for input sign
            HouseName houseName = houses.Where(e => e.Value.GetSignName() == zodiacName).Select(e => e.Key).FirstOrDefault();

            return houseName;

        }

        /// <summary>
        /// All horoscope predictions as Alpaca Template ready for LoRA training in JSON
        /// </summary>
        public static async Task<List<JObject>> HoroscopePredictionAlpacaTemplateLoRA(Time birthTime)
        {
            var returnList = new List<JObject>();
            foreach (var horoscopeData in HoroscopeDataListStatic.Rows)
            {
                JObject jObject = new JObject
                {
                    ["instruction"] = horoscopeData.Name.ToString(),
                    ["input"] = "",
                    ["output"] = horoscopeData.Description
                };

                returnList.Add(jObject);
            }

            return returnList;
        }

        /// <summary>
        /// Given a birth time will calculate all predictions that match for given birth time.
        /// Default includes all predictions, ie: Yoga, Planets in Sign, AshtakavargaYoga
        /// Can be filtered.
        /// </summary>
        /// <param name="filterTag">Set to only show certain types of predictions</param>
        public static List<HoroscopePrediction> HoroscopePredictions(Time birthTime, EventTag filterTag = EventTag.Empty)
        {
            //calculate predictions for current person
            var predictionList = Tools.GetHoroscopePrediction(birthTime, filterTag);

            return predictionList;
        }

        /// <summary>
        /// Given a birth time will calculate all prediction name's that match for given birth time
        /// example : "Moon House 8", "10th Lord in 8th House"
        /// note : used by AI Chat, when talking to Astro tuned LLM server
        /// </summary>
        public static List<string> HoroscopePredictionNames(Time birthTime)
        {
            //calculate predictions for current person
            var predictionList = Tools.GetHoroscopePrediction(birthTime);

            //take out only name
            var namesOnly = predictionList.Select(x => x.Name.ToString()).ToList();

            return namesOnly;
        }

        /// <summary>
        /// Given a standard time (LMT) and location will get Local mean time
        /// </summary>
        public static string LocalMeanTime(Time time) => time.GetLmtDateTimeOffsetText();

        /// <summary>
        /// Given a standard time (STD) and location will get local standard time based on location
        /// Offset auto set by Google Offset API 
        /// </summary>
        public static string LocalStandardTime(Time time) => time.GetStdDateTimeOffsetText();

        /// <summary>
        /// Calculate Lord of Star (Constellation) given Constellation. Returns Star Lord Name
        /// </summary>
        public static PlanetName LordOfConstellation(ConstellationName constellation)
        {
            switch (constellation)
            {
                case ConstellationName.Aswini:
                case ConstellationName.Makha:
                case ConstellationName.Moola:
                    return Ketu;
                    break;
                case ConstellationName.Bharani:
                case ConstellationName.Pubba:
                case ConstellationName.Poorvashada:
                    return Venus;
                case ConstellationName.Krithika:
                case ConstellationName.Uttara:
                case ConstellationName.Uttarashada:
                    //case ConstellationName.Abhijit:
                    return VedAstro.Library.PlanetName.Sun;
                case ConstellationName.Rohini:
                case ConstellationName.Hasta:
                case ConstellationName.Sravana:
                    return VedAstro.Library.PlanetName.Moon;
                case ConstellationName.Mrigasira:
                case ConstellationName.Chitta:
                case ConstellationName.Dhanishta:
                    return VedAstro.Library.PlanetName.Mars;
                case ConstellationName.Aridra:
                case ConstellationName.Swathi:
                case ConstellationName.Satabhisha:
                    return VedAstro.Library.PlanetName.Rahu;
                case ConstellationName.Punarvasu:
                case ConstellationName.Vishhaka:
                case ConstellationName.Poorvabhadra:
                    return VedAstro.Library.PlanetName.Jupiter;
                case ConstellationName.Pushyami:
                case ConstellationName.Anuradha:
                case ConstellationName.Uttarabhadra:
                    return VedAstro.Library.PlanetName.Saturn;
                case ConstellationName.Aslesha:
                case ConstellationName.Jyesta:
                case ConstellationName.Revathi:
                    return VedAstro.Library.PlanetName.Mercury;
                default:
                    return VedAstro.Library.PlanetName.Empty;
            }

            throw new Exception("End of Line");

        }



        /// <summary>
        /// Calculate Fortuna Point for a given birth time & place. Returns Sign Number from Lagna
        /// for KP system a fast-moving point which can differentiate between two early births as twins.
        /// </summary>
        public static int FortunaPoint(ZodiacName ascZodiacSignName, Time time)
        {
            //Fortune Point is calculated as Asc Degrees + Moon Degrees - Sun Degrees
            var a1 = Calculate.AllHouseMiddleLongitudes(time)[0].GetBeginLongitude().TotalDegrees;

            //Find Lagna, Moon and Sun longitude degree
            var _asc_Degrees = Calculate.AllHouseMiddleLongitudes(time)[0].GetMiddleLongitude().TotalDegrees;
            var _moonDegrees = Calculate.PlanetNirayanaLongitude(PlanetName.Moon, time).TotalDegrees;
            var _sunDegrees = Calculate.PlanetNirayanaLongitude(PlanetName.Sun, time).TotalDegrees;

            //fortuna point is the point that is same distance from Ascendant
            //as Moon is from Sun
            var _fortunaPointDegrees = 0.00;

            /*
            //if its a day chart


            if (_sunDegrees >= 180.000 && _sunDegrees < 360.000) 
            {
                _fortunaPointDegrees = _asc_Degrees + _moonDegrees - _sunDegrees;
            }

            else
            {
                if (_sunDegrees >= 0.000 && _sunDegrees < 180.000)
                {
                    _fortunaPointDegrees = _asc_Degrees + _sunDegrees - _moonDegrees;
                }
            }
            */

            //first let's compute how far the Moon is from Sun
            var _moon_sun_distance = _moonDegrees - _sunDegrees;

            if (_moon_sun_distance < 0) //moon is behind sun
            {
                _moon_sun_distance = _moon_sun_distance + 360;
            }

            //now lets compute the Fortuna point 
            _fortunaPointDegrees = _asc_Degrees + _moon_sun_distance;

            if (_fortunaPointDegrees >= 360)
            {
                _fortunaPointDegrees = _fortunaPointDegrees - 360;
            }


            //convert Degrees to Angle
            var _angleAtFortunaPointDegrees = VedAstro.Library.Angle.FromDegrees(_fortunaPointDegrees);

            //find zodiacSignAtFP Longitude
            var _zodiacSignAtFP = Calculate.ZodiacSignAtLongitude(_angleAtFortunaPointDegrees).GetSignName();

            // var houseNo = Calculate.HouseFromSignName(_zodiacSignAtFP, time);


            //find how many signs the FP is from Lagna
            var _signCount = Calculate.CountFromSignToSign(ascZodiacSignName, _zodiacSignAtFP);
            return _signCount;
        }

        /// <summary>
        /// Calculate Destiny Point for a given birth time & place. Returns Sign Number from Lagna
        /// </summary>
        public static int DestinyPoint(Time time, ZodiacName ascZodiacSignName)
        {
            //destiny point is calculated as follows
            //Difference between Moon and Rahu longitude, Difference divided by 2, the result added to Rahu longitude

            var rahuDegrees = Calculate.PlanetNirayanaLongitude(PlanetName.Rahu, time).TotalDegrees;
            var moonDegrees = Calculate.PlanetNirayanaLongitude(PlanetName.Moon, time).TotalDegrees;

            var diff = moonDegrees - rahuDegrees;

            // if diff is negative, that means Moon is ahead of Rahu, then add 360 to the number. 
            if (diff < 0)
            {
                diff = diff + 360;
            }

            var mid_point = diff / 2;

            // Add mid_point to Rahu degrees
            var destinyPointDegrees = rahuDegrees + mid_point;

            if (destinyPointDegrees >= 360)
            {
                destinyPointDegrees = destinyPointDegrees - 360;
            }

            var angleAtDestinyPointDegrees = VedAstro.Library.Angle.FromDegrees(destinyPointDegrees);
            var zodiacSignAtDP = Calculate.ZodiacSignAtLongitude(angleAtDestinyPointDegrees).GetSignName();
            var signCount = Calculate.CountFromSignToSign(ascZodiacSignName, zodiacSignAtDP);

            return signCount;
        }

        /// <summary>
        /// Given a person will give yoni kuta animal with sex
        /// </summary>
        public static string YoniKutaAnimalFromPerson(Person person)
        {
            var finalPrediction = "";

            var birthConst = Calculate.MoonConstellation(person.BirthTime);
            var animal = Calculate.YoniKutaAnimalFromConstellation(birthConst.GetConstellationName());

            finalPrediction += animal.ToString();

            return finalPrediction;
        }

        /// <summary>
        /// Given a constellation will give animal with sex, used for yoni kuta calculations
        /// and body appearance prediction
        /// </summary>
        public static ConstellationAnimal YoniKutaAnimalFromConstellation(ConstellationName sign)
        {
            switch (sign)
            {
                //Horse
                case ConstellationName.Aswini:
                    return new ConstellationAnimal("Male", AnimalName.Horse);
                case ConstellationName.Satabhisha:
                    return new ConstellationAnimal("Female", AnimalName.Horse);

                //Elephant
                case ConstellationName.Bharani:
                    return new ConstellationAnimal("Male", AnimalName.Elephant);
                case ConstellationName.Revathi:
                    return new ConstellationAnimal("Female", AnimalName.Elephant);

                //Sheep
                case ConstellationName.Pushyami:
                    return new ConstellationAnimal("Male", AnimalName.Sheep);
                case ConstellationName.Krithika:
                    return new ConstellationAnimal("Female", AnimalName.Sheep);

                //Serpent
                case ConstellationName.Rohini:
                    return new ConstellationAnimal("Male", AnimalName.Serpent);
                case ConstellationName.Mrigasira:
                    return new ConstellationAnimal("Female", AnimalName.Serpent);

                //Dog
                case ConstellationName.Moola:
                    return new ConstellationAnimal("Male", AnimalName.Dog);
                case ConstellationName.Aridra:
                    return new ConstellationAnimal("Female", AnimalName.Dog);

                //Cat
                case ConstellationName.Aslesha:
                    return new ConstellationAnimal("Male", AnimalName.Cat);
                case ConstellationName.Punarvasu:
                    return new ConstellationAnimal("Female", AnimalName.Cat);

                //Rat
                case ConstellationName.Makha:
                    return new ConstellationAnimal("Male", AnimalName.Rat);
                case ConstellationName.Pubba:
                    return new ConstellationAnimal("Female", AnimalName.Rat);

                //Cow
                case ConstellationName.Uttara:
                    return new ConstellationAnimal("Male", AnimalName.Cow);
                case ConstellationName.Uttarabhadra:
                    return new ConstellationAnimal("Female", AnimalName.Cow);

                //Buffalo
                case ConstellationName.Swathi:
                    return new ConstellationAnimal("Male", AnimalName.Buffalo);
                case ConstellationName.Hasta:
                    return new ConstellationAnimal("Female", AnimalName.Buffalo);

                //Tiger
                case ConstellationName.Vishhaka:
                    return new ConstellationAnimal("Male", AnimalName.Tiger);
                case ConstellationName.Chitta:
                    return new ConstellationAnimal("Female", AnimalName.Tiger);

                //Hare
                case ConstellationName.Jyesta:
                    return new ConstellationAnimal("Male", AnimalName.Hare);
                case ConstellationName.Anuradha:
                    return new ConstellationAnimal("Female", AnimalName.Hare);

                //Monkey
                case ConstellationName.Poorvashada:
                    return new ConstellationAnimal("Male", AnimalName.Monkey);
                case ConstellationName.Sravana:
                    return new ConstellationAnimal("Female", AnimalName.Monkey);

                //Lion
                case ConstellationName.Poorvabhadra:
                    return new ConstellationAnimal("Male", AnimalName.Lion);
                case ConstellationName.Dhanishta:
                    return new ConstellationAnimal("Female", AnimalName.Lion);

                //Mongoose
                case ConstellationName.Uttarashada:
                    return new ConstellationAnimal("Male", AnimalName.Mongoose);

                default: throw new Exception("Yoni Kuta Animal Not Found!");
            }
        }

        /// <summary>
        /// Get sky chart as animated GIF. URL can be used like a image source link
        /// </summary>
        public static async Task<byte[]> SkyChartGIF(Time time) => await SkyChartFactory.GenerateChartGif(time, 750, 230);

        /// <summary>
        /// Get sky chart at a given time. SVG image file. URL can be used like a image source link
        /// </summary>
        public static async Task<string> SkyChart(Time time) => await SkyChartFactory.GenerateChart(time, 750, 230);


        /// <summary>
        /// Get sky chart at a given time. SVG image file. URL can be used like a image source link
        /// </summary>
        public static string SouthIndianChart(Time time, ChartType chartType = ChartType.Rasi)
        {
            var svgString = (new SouthChartFactory(time, 1000, 1000, chartType)).SVGChart;

            return svgString;
        }

        /// <summary>
        /// Get sky chart at a given time. SVG image file. URL can be used like a image source link
        /// </summary>
        public static string NorthIndianChart(Time time)
        {
            var svgString = NorthChartFactory.GenerateChart(time, 1000, 1000);

            return svgString;
        }

        /// <summary>
        /// Checks if a planet is in a Watery or aqua sign
        /// </summary>
        public static bool IsPlanetInWaterySign(PlanetName planetName, Time time)
        {
            //get sign planet is in
            var planetSign = PlanetZodiacSign(planetName, time);

            //check if sign is watery
            var isWater = IsWaterSign(planetSign.GetSignName());

            return isWater;
        }

        /// <summary>
        /// Strength to judge the exact quantity of effect planet gives in a house
        /// 
        /// Use of Residential Strength --This will
        /// enable us to judge the exact quantity of effect that
        /// a pJanet in a Bhava gives, which may find expression
        /// during its Dasa.Its application and usefulness
        /// will be explained on a subsequent occasion.
        /// This effect will materialize during his Dasa or
        /// Bhukti. This is only a general statement standing
        /// to be modified or qualified in the light of other
        /// important factors such as, the strength or the weakness
        /// of the planets aspecting the Bhavas, the
        /// strength of the Bhava itself and the disposition
        /// of planets towards particular signs, the yogakarakas
        /// and such other factors.
        /// For instance, in the Standard Horoscope Jupiter
        /// gives 0.48 units of the total effects of the 6th Bhava.
        /// </summary>
        public static double ResidentialStrength(PlanetName planetName, Time time)
        {
            return 0;

            //todo from PG15 of Bhava and Graha Balas
            throw new NotImplementedException("");
        }

        /// <summary>
        /// Converts time back to longitude, it is the reverse of GetLocalTimeOffset in Time
        /// Exp :  5h. 10m. 20s. E. Long. to 77° 35' E. Long
        /// </summary>
        public static Angle TimeToLongitude(TimeSpan time)
        {
            //TODO function is a candidate for caching
            //degrees is equivalent to hours
            var totalDegrees = time.TotalHours * 15;

            return Angle.FromDegrees(totalDegrees);
        }

        //NORMAL FUNCTIONS
        //FUNCTIONS THAT CALL OTHER FUNCTIONS IN THIS CLASS

        /// <summary>
        /// Gets the ephemris time that is consumed by Swiss Ephemeris
        /// Converts normal time to Ephemeris time shown as a number
        /// </summary>
        public static double TimeToEphemerisTime(Time time)
        {

            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey(nameof(TimeToEphemerisTime), time, Ayanamsa), _timeToEphemerisTime);


            //UNDERLYING FUNCTION
            double _timeToEphemerisTime()
            {
                SwissEph ephemeris = new();

                //set GREGORIAN CALENDAR
                int gregflag = SwissEph.SE_GREG_CAL;

                //get LMT at UTC (+0:00)
                DateTimeOffset utcDate = LmtToUtc(time);

                //extract details of time
                int year = utcDate.Year;
                int month = utcDate.Month;
                int day = utcDate.Day;
                double hour = (utcDate.TimeOfDay).TotalHours;


                double jul_day_UT;
                double jul_day_ET;

                //do conversion to ephemris time
                jul_day_UT = ephemeris.swe_julday(year, month, day, hour, gregflag); //time to Julian Day
                jul_day_ET = jul_day_UT + ephemeris.swe_deltat(jul_day_UT); //Julian Day to ET

                return jul_day_ET;
            }

        }

        /// <summary>
        /// Gets Moon's position or day in lunar calendar
        /// </summary>
        public static LunarDay LunarDay(Time time)
        {
            //get position of sun & moon
            Angle sunLong = PlanetNirayanaLongitude(Sun, time);
            Angle moonLong = PlanetNirayanaLongitude(Moon, time);

            double rawLunarDate;

            if (moonLong.TotalDegrees > sunLong.TotalDegrees)
            {
                rawLunarDate = (moonLong - sunLong).TotalDegrees / 12.0;
            }
            else
            {
                rawLunarDate = ((moonLong + Angle.Degrees360) - sunLong).TotalDegrees / 12.0;
            }

            //round number to next whole number (ceiling)
            int roundedLunarDateNumber = (int)Math.Ceiling(rawLunarDate);

            //use lunar date number to initialize a lunar day instance
            var lunarDay = new LunarDay(roundedLunarDateNumber);

            //return lunar day to caller
            return lunarDay;


        }

        /// <summary>
        /// Gets name of Constellation behind the moon at a given time
        /// </summary>
        public static Constellation MoonConstellation(Time time) => PlanetConstellation(Moon, time);

        /// <summary>
        /// Gets the constellation behind a planet at a given time
        /// </summary>
        public static Constellation PlanetConstellation(PlanetName planet, Time time)
        {
            //get position of planet in longitude
            var planetLongitude = PlanetNirayanaLongitude(planet, time);

            //return the constellation behind the planet
            return ConstellationAtLongitude(planetLongitude);
        }

        /// <summary>
        /// Tarabala or birth ruling constellation strength, used for personal muhurtha
        /// </summary>
        public static Tarabala Tarabala(Time time, Person person)
        {
            int dayRulingConstellationNumber = MoonConstellation(time).GetConstellationNumber();

            int birthRulingConstellationNumber = MoonConstellation(person.BirthTime).GetConstellationNumber();

            int counter = 0;

            int cycle;


            //Need to count from birthRulingConstellationNumber to dayRulingConstellationNumber
            //todo upgrade to "ConstellationCounter", double check validity first
            //If birthRulingConstellationNumber is more than dayRulingConstellationNumber
            if (birthRulingConstellationNumber > dayRulingConstellationNumber)
            {
                //count birthRulingConstellationNumber to last constellation (27)
                int countToLastConstellation = (27 - birthRulingConstellationNumber) + 1; //plus 1 to count it self

                //add dayRulingConstellationNumber to countToLastConstellation(difference)
                counter = dayRulingConstellationNumber + countToLastConstellation;
            }
            else if (birthRulingConstellationNumber == dayRulingConstellationNumber)
            {
                counter = 1;
            }
            else if (birthRulingConstellationNumber < dayRulingConstellationNumber)
            {
                //If dayRulingConstellationNumber is more than or equal to birthRulingConstellationNumber
                counter = (dayRulingConstellationNumber - birthRulingConstellationNumber) + 1; //plus 1 to count it self
            }

            //change to double for division and then round up
            cycle = (int)Math.Ceiling(((double)counter / 9.0));


            //divide the number by 9 if divisible. Otherwise
            //keep it as it is.
            if (counter > 9)
            {
                //get modulos of counter
                counter = counter % 9;
                if (counter == 0)
                    counter = 9;
            }


            //initialize new tarabala from tarabala number & cycle
            var returnValue = new Tarabala(counter, cycle);

            return returnValue;
        }

        /// <summary>
        /// Chandrabala or lunar strength, used for personal muhurtha
        ///
        /// Reference:
        /// Chandrabala. - As we have already said above, the consideration of the
        /// Moon and his position are of much importance in Muhurtha. To be at its
        /// best, the Moon should not occupy in the election chart, a position that
        /// happens to represent the 6th, 8th or 12th from the person's Janma Rasi.
        /// </summary>
        public static int Chandrabala(Time time, Person person)
        {
            //TODO Needs to be updated with count sign from sign for better consistency
            //     also possible to leave it as is for better decoupling since this is working fine

            //initialize chandrabala number as 0
            int chandrabalaNumber = 0;

            //get zodiac name & convert to its number
            var dayMoonSignNumber = (int)MoonSignName(time);
            var birthMoonSignNumber = (int)MoonSignName(person.BirthTime);


            //Need to count from birthMoonSign to dayMoonSign

            //If birthMoonSign is more than dayMoonSign
            if (birthMoonSignNumber > dayMoonSignNumber)
            {
                //count birthMoonSign to last zodiac (12)
                int countToLastZodiac = (12 - birthMoonSignNumber) + 1; //plus 1 to count it self

                //add dayMoonSign to countToLastZodiac
                chandrabalaNumber = dayMoonSignNumber + countToLastZodiac;

            }
            else if (birthMoonSignNumber == dayMoonSignNumber)
            {
                chandrabalaNumber = 1;
            }
            else if (birthMoonSignNumber < dayMoonSignNumber)
            {
                //If dayMoonSign is more than or equal to birthMoonSign
                chandrabalaNumber = (dayMoonSignNumber - birthMoonSignNumber) + 1; //plus 1 to count it self
            }

            return chandrabalaNumber;

        }

        /// <summary>
        /// Zodiac sign behind the Moon at given time
        /// </summary>
        public static ZodiacName MoonSignName(Time time)
        {
            //get zodiac sign behind the moon
            var moonSign = PlanetZodiacSign(Moon, time);

            //return name of zodiac sign
            return moonSign.GetSignName();
        }

        /// <summary>
        /// Zodiac sign at the Lagna/Ascendant at given time
        /// </summary>
        public static ZodiacName LagnaSignName(Time time)
        {
            //get zodiac sign behind the Lagna/Ascendant
            var lagnaSign = HouseSignName(HouseName.House1, time);

            //return name of zodiac sign
            return lagnaSign;
        }

        /// <summary>
        /// Also know as Panchanga Yoga
        /// Nithya Yoga = (Longitude of Sun + Longitude of Moon) / 13°20' (or 800')
        /// </summary>
        public static NithyaYoga NithyaYoga(Time time)
        {
            //Nithya Yoga = (Longitude of Sun + Longitude of Moon) / 13°20' (or 800')

            //get position of sun & moon in longitude
            Angle sunLongitude = PlanetNirayanaLongitude(Sun, time);
            Angle moonLongitude = PlanetNirayanaLongitude(Moon, time);

            var jointLongitudeInMinutes = (sunLongitude + moonLongitude).Expunge360().TotalMinutes;

            //get joint motion in longitude of the Sun and the Moon
            //var jointLongitudeInMinutes = sunLongitude.TotalMinutes + moonLongitude.TotalMinutes;



            //get unrounded nithya yoga number by
            //dividing joint longitude by 800'
            var rawNithyaYogaNumber = jointLongitudeInMinutes / 800;

            //round to ceiling to get whole number
            var nithyaYogaNumber = Math.Ceiling(rawNithyaYogaNumber);

            //convert nithya yoga number to type
            var nithyaYoga = VedAstro.Library.NithyaYoga.FromNumber(nithyaYogaNumber);

            //return to caller

            return nithyaYoga;
        }

        /// <summary>
        /// Used for muhurtha of auspicious activities, part of Panchang like Tithi, Nakshatra, Yoga, etc.
        /// Each tithi is divided into 2 karanas. There are 11 karanas: (1) Bava, (2)
        /// Balava, (3) Kaulava, (4) Taitula, (5) Garija, (6) Vanija, (7) Vishti, (8) Sakuna,
        /// (9) Chatushpada, (10) Naga, and, (11) Kimstughna. The first 7 karanas
        /// repeat 8 times starting from the 2nd half of the first lunar day of a month.
        /// The last 4 karanas come just once in a month, starting from the 2nd half of
        /// the 29th lunar day and ending at the 1st half of the first lunar day.
        /// </summary>
        public static Karana Karana(Time time)
        {
            //declare karana as empty first
            Karana? karanaToReturn = null;

            //get position of sun & moon
            Angle sunLong = PlanetNirayanaLongitude(Sun, time);
            Angle moonLong = PlanetNirayanaLongitude(Moon, time);

            //get raw lunar date
            double rawlunarDate;

            if (moonLong.TotalDegrees > sunLong.TotalDegrees)
            {
                rawlunarDate = (moonLong - sunLong).TotalDegrees / 12.0;
            }
            else
            {
                rawlunarDate = ((moonLong + Angle.Degrees360) - sunLong).TotalDegrees / 12.0;
            }

            //round number to next whole number (ceiling)
            int roundedLunarDateNumber = (int)Math.Ceiling(rawlunarDate);

            //get lunar day already traversed
            var lunarDayAlreadyTraversed = rawlunarDate - Math.Floor(rawlunarDate);

            switch (roundedLunarDateNumber)
            {
                //based on lunar date get karana
                case 1:
                    karanaToReturn = lunarDayAlreadyTraversed <= 0.5 ? Library.Karana.Kimstughna : Library.Karana.Bava;
                    break;
                case 23:
                case 16:
                case 9:
                case 2:
                    karanaToReturn = lunarDayAlreadyTraversed <= 0.5 ? Library.Karana.Balava : Library.Karana.Kaulava;
                    break;
                case 24:
                case 17:
                case 10:
                case 3:
                    karanaToReturn = lunarDayAlreadyTraversed <= 0.5 ? Library.Karana.Taitula : Library.Karana.Garija;
                    break;
                case 25:
                case 18:
                case 11:
                case 4:
                    karanaToReturn = lunarDayAlreadyTraversed <= 0.5 ? Library.Karana.Vanija : Library.Karana.Visti;
                    break;
                case 26:
                case 19:
                case 12:
                case 5:
                    karanaToReturn = lunarDayAlreadyTraversed <= 0.5 ? Library.Karana.Bava : Library.Karana.Balava;
                    break;
                case 27:
                case 20:
                case 13:
                case 6:
                    karanaToReturn = lunarDayAlreadyTraversed <= 0.5 ? Library.Karana.Kaulava : Library.Karana.Taitula;
                    break;
                case 28:
                case 21:
                case 14:
                case 7:
                    karanaToReturn = lunarDayAlreadyTraversed <= 0.5 ? Library.Karana.Garija : Library.Karana.Vanija;
                    break;
                case 22:
                case 15:
                case 8:
                    karanaToReturn = lunarDayAlreadyTraversed <= 0.5 ? Library.Karana.Visti : Library.Karana.Bava;
                    break;
                case 29:
                    karanaToReturn = lunarDayAlreadyTraversed <= 0.5 ? Library.Karana.Visti : Library.Karana.Sakuna;
                    break;
                case 30:
                    karanaToReturn = lunarDayAlreadyTraversed <= 0.5 ? Library.Karana.Chatushpada : Library.Karana.Naga;
                    break;

            }

            //if karana not found throw error
            if (karanaToReturn == null)
            {
                throw new Exception("Karana could not be found!");
            }

            return (Karana)karanaToReturn;
        }

        /// <summary>
        /// Zodiac sign behind the Sun at a time
        /// </summary>
        public static ZodiacSign SunSign(Time time)
        {
            //get zodiac sign behind the sun
            var sunSign = PlanetZodiacSign(Sun, time);

            //return zodiac sign behind sun
            return sunSign;
        }

        ///<summary>
        ///Find time when Sun was in 0.001 degrees
        ///in current sign (just entered sign)
        ///</summary>
        public static Time TimeSunEnteredCurrentSign(Time time)
        {

            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey(nameof(TimeSunEnteredCurrentSign), time, Ayanamsa), _timeSunEnteredCurrentSign);


            //UNDERLYING FUNCTION
            Time _timeSunEnteredCurrentSign()
            {

                //set the maximum accuracy used to calculate time sun will enter the sign
                //once this limit is hit, the previously calculated time will be returned
                double AccuracyLimit = TimePreset.Minute3;

                //set time decrement accuracy at 96 hours (4 days) at first
                double timeDecrementAccuracy = 96;

                //set input time as possible entered time at first
                var possibleEnteredTime = time;
                var previousPossibleEnteredTime = time;


                //get current sun sign
                var currentSunSign = SunSign(time);

                //if entered time not yet found
                while (true) //breaks when found
                {
                    //get the sign at possible entered time
                    var possibleSunSign = SunSign(possibleEnteredTime);

                    //if possible sign name is same as current sign name, then check if sun is about to enter sign
                    var signNameIsSame = possibleSunSign.GetSignName() == currentSunSign.GetSignName();
                    if (signNameIsSame)
                    {
                        //if sun sign is less than 0.001 degrees, entered time found
                        if (possibleSunSign.GetDegreesInSign().TotalDegrees < 0.001) { break; }

                        //else sun not yet torward the start of the sign, so decrement time
                        else
                        {
                            //back up possible entered time before changing
                            previousPossibleEnteredTime = possibleEnteredTime;

                            //decrement entered time, to check next possible time
                            possibleEnteredTime = possibleEnteredTime.SubtractHours(timeDecrementAccuracy);
                        }
                    }
                    //else sun sign is not same, went to far
                    else
                    {
                        //return possible entered time to previous time
                        possibleEnteredTime = previousPossibleEnteredTime;

                        //if accuracy limit is hit, then use previous time as answer, stop looking
                        if (timeDecrementAccuracy <= AccuracyLimit) { break; }

                        //decrease time decrement accuracy by half
                        timeDecrementAccuracy = timeDecrementAccuracy / 2;

                    }
                }

                //return possible entered time
                return possibleEnteredTime;

            }
        }

        ///<summary>
        ///Find time when Sun was in 29 degrees
        ///in current sign (just about to leave sign)
        ///
        /// Note:
        /// -2 possible ways leaving time is calculated
        ///     1. degrees Sun is in sign is more than 29.999 degrees (very very close to leaving sign)
        ///     2. accuracy limit is hit
        ///</summary>
        public static Time TimeSunLeavesCurrentSign(Time time)
        {

            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey(nameof(TimeSunLeavesCurrentSign), time, Ayanamsa), _getTimeSunLeavesCurrentSign);


            //UNDERLYING FUNCTION
            Time _getTimeSunLeavesCurrentSign()
            {

                //set the maximum accuracy used to calculate time sun will leave the sign
                //once this limit is hit, the previously calculated time will be returned
                double AccuracyLimit = TimePreset.Minute3;

                //set time increment accuracy at 96 hours (4 days) at first
                double timeIncrementAccuracy = 96;

                //set input time as possible leaving time at first
                var possibleLeavingTime = time;
                var previousPossibleLeavingTime = time;

                //get current sun sign
                var currentSunSign = SunSign(time);

                //find leaving time
                while (true)
                {
                    //get the sign at possible leaving time
                    var possibleSunSign = SunSign(possibleLeavingTime);

                    //if possible sign name is same as current sign name, then check if sun is about to leave sign
                    var signNameIsSame = possibleSunSign.GetSignName() == currentSunSign.GetSignName();
                    if (signNameIsSame)
                    {
                        //if sun sign is more than 29.9 degrees, leaving time found
                        if (possibleSunSign.GetDegreesInSign().TotalDegrees > 29.999) { break; }

                        //else sun not yet torward the end of the sign, so increment time
                        else
                        {
                            //back up possible leaving time before changing
                            previousPossibleLeavingTime = possibleLeavingTime;

                            //increment leaving time, to check next possible time
                            possibleLeavingTime = possibleLeavingTime.AddHours(timeIncrementAccuracy);
                        }
                    }
                    //else sun sign is not same, went to far, go back a little in time
                    else
                    {
                        //restore possible leaving time to previous time
                        possibleLeavingTime = previousPossibleLeavingTime;

                        //if accuracy limit is hit, then use previous time as answer, stop looking
                        if (timeIncrementAccuracy <= AccuracyLimit) { break; }

                        //decrease time increment accuracy by half
                        timeIncrementAccuracy = timeIncrementAccuracy / 2;

                    }
                }

                //return possible leaving time
                return possibleLeavingTime;

            }

        }

        /// <summary>
        /// special function localized to allow caching
        /// note: there is another version that does caching
        /// </summary>
        public static double TimeToJulianDay(Time time)
        {
            //get lmt time
            var lmtDateTime = time.GetLmtDateTimeOffset();

            //Converts LMT to UTC (GMT)
            DateTimeOffset utcDateTime = lmtDateTime.ToUniversalTime();

            SwissEph swissEph = new SwissEph();

            double jul_day_UT;
            jul_day_UT = swissEph.swe_julday(utcDateTime.Year, utcDateTime.Month, utcDateTime.Day,
                utcDateTime.TimeOfDay.TotalHours, SwissEph.SE_GREG_CAL);
            return jul_day_UT;

        }

        /// <summary>
        /// Gives the middle longitude of all houses at a give time
        /// </summary>
        public static List<House> AllHouseMiddleLongitudes(Time time)
        {
            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey(nameof(AllHouseMiddleLongitudes), time, Ayanamsa), _getHouses);


            //UNDERLYING FUNCTION
            List<House> _getHouses()
            {
                //declare house longitudes
                Angle house1BeginLongitude, house1MiddleLongitude, house1EndLongitude;
                Angle house2BeginLongitude, house2MiddleLongitude, house2EndLongitude;
                Angle house3BeginLongitude, house3MiddleLongitude, house3EndLongitude;
                Angle house4BeginLongitude, house4MiddleLongitude, house4EndLongitude;
                Angle house5BeginLongitude, house5MiddleLongitude, house5EndLongitude;
                Angle house6BeginLongitude, house6MiddleLongitude, house6EndLongitude;
                Angle house7BeginLongitude, house7MiddleLongitude, house7EndLongitude;
                Angle house8BeginLongitude, house8MiddleLongitude, house8EndLongitude;
                Angle house9BeginLongitude, house9MiddleLongitude, house9EndLongitude;
                Angle house10BeginLongitude, house10MiddleLongitude, house10EndLongitude;
                Angle house11BeginLongitude, house11MiddleLongitude, house11EndLongitude;
                Angle house12BeginLongitude, house12MiddleLongitude, house12EndLongitude;


                //1.Get middle longitudes of angular houses

                //1.1 get House 1 & 10

                //Get western 1 & 10 house longitudes
                var cusps = GetHouse1And10Longitudes(time);

                //Get Sayana Long. of cusp of ascend.
                var sayanaCuspOfHouse1 = Angle.FromDegrees(cusps[1]);

                //Get Sayana Long. of cusp of tenth-house
                var sayanaCuspOfHouse10 = Angle.FromDegrees(cusps[10]);

                //Deduct from these two, the Ayanamsa to get the Nirayana longitudes
                // of Udaya Lagna (Ascendant) and the Madhya Lagna (Upper Meridian)
                var ayanamsa = AyanamsaDegree(time);

                var udayaLagna = sayanaCuspOfHouse1 - ayanamsa;
                var madhyaLagna = sayanaCuspOfHouse10 - ayanamsa;

                //Add 180° to each of these two, to get the Nirayana Asta Lagna (Western Horizon)
                //and the Pathala Lagna (Lower Meridian)
                var astaLagna = udayaLagna + Angle.Degrees180;
                var pathalaLagna = madhyaLagna + Angle.Degrees180;

                //if longitude is more than 360°, expunge 360°
                astaLagna = astaLagna.Expunge360();
                pathalaLagna = pathalaLagna.Expunge360();

                //assign angular house middle longitudes, houses 1,4,7,10
                house1MiddleLongitude = udayaLagna + Angle.FromDegrees(15);
                house4MiddleLongitude = pathalaLagna + Angle.FromDegrees(15);
                house7MiddleLongitude = astaLagna + Angle.FromDegrees(15);
                house10MiddleLongitude = madhyaLagna + Angle.FromDegrees(15);

                //2.0 Get middle longitudes of non-angular houses
                //2.1 Calculate arcs
                Angle arcA, arcB, arcC, arcD;

                //calculate Arc A
                if (house4MiddleLongitude < house1MiddleLongitude)
                {
                    arcA = ((house4MiddleLongitude + Angle.Degrees360) - house1MiddleLongitude);
                }
                else
                {
                    arcA = (house4MiddleLongitude - house1MiddleLongitude);
                }

                //calculate Arc B
                if (house7MiddleLongitude < house4MiddleLongitude)
                {
                    arcB = ((house7MiddleLongitude + Angle.Degrees360) - house4MiddleLongitude);
                }
                else
                {
                    arcB = (house7MiddleLongitude - house4MiddleLongitude);
                }

                //calculate Arc C
                if (house10MiddleLongitude < house7MiddleLongitude)
                {
                    arcC = ((house10MiddleLongitude + Angle.Degrees360) - house7MiddleLongitude);
                }
                else
                {
                    arcC = (house10MiddleLongitude - house7MiddleLongitude);
                }

                //calculate Arc D
                if (house1MiddleLongitude < house10MiddleLongitude)
                {
                    arcD = ((house1MiddleLongitude + Angle.Degrees360) - house10MiddleLongitude);
                }
                else
                {
                    arcD = (house1MiddleLongitude - house10MiddleLongitude);
                }

                //2.2 Trisect each arc
                //Cacl House 2 & 3
                house2MiddleLongitude = house1MiddleLongitude + arcA.Divide(3);
                house2MiddleLongitude = house2MiddleLongitude.Expunge360();
                house3MiddleLongitude = house2MiddleLongitude + arcA.Divide(3);
                house3MiddleLongitude = house3MiddleLongitude.Expunge360();

                //Cacl House 5 & 6
                house5MiddleLongitude = house4MiddleLongitude + arcB.Divide(3);
                house5MiddleLongitude = house5MiddleLongitude.Expunge360();
                house6MiddleLongitude = house5MiddleLongitude + arcB.Divide(3);
                house6MiddleLongitude = house6MiddleLongitude.Expunge360();

                //Cacl House 8 & 9
                house8MiddleLongitude = house7MiddleLongitude + arcC.Divide(3);
                house8MiddleLongitude = house8MiddleLongitude.Expunge360();
                house9MiddleLongitude = house8MiddleLongitude + arcC.Divide(3);
                house9MiddleLongitude = house9MiddleLongitude.Expunge360();

                //Cacl House 11 & 12
                house11MiddleLongitude = house10MiddleLongitude + arcD.Divide(3);
                house11MiddleLongitude = house11MiddleLongitude.Expunge360();
                house12MiddleLongitude = house11MiddleLongitude + arcD.Divide(3);
                house12MiddleLongitude = house12MiddleLongitude.Expunge360();

                //3.0 Calculate house begin & end longitudes

                house1EndLongitude = house2BeginLongitude = HouseJunctionPoint(house1MiddleLongitude, house2MiddleLongitude);
                house2EndLongitude = house3BeginLongitude = HouseJunctionPoint(house2MiddleLongitude, house3MiddleLongitude);
                house3EndLongitude = house4BeginLongitude = HouseJunctionPoint(house3MiddleLongitude, house4MiddleLongitude);
                house4EndLongitude = house5BeginLongitude = HouseJunctionPoint(house4MiddleLongitude, house5MiddleLongitude);
                house5EndLongitude = house6BeginLongitude = HouseJunctionPoint(house5MiddleLongitude, house6MiddleLongitude);
                house6EndLongitude = house7BeginLongitude = HouseJunctionPoint(house6MiddleLongitude, house7MiddleLongitude);
                house7EndLongitude = house8BeginLongitude = HouseJunctionPoint(house7MiddleLongitude, house8MiddleLongitude);
                house8EndLongitude = house9BeginLongitude = HouseJunctionPoint(house8MiddleLongitude, house9MiddleLongitude);
                house9EndLongitude = house10BeginLongitude = HouseJunctionPoint(house9MiddleLongitude, house10MiddleLongitude);
                house10EndLongitude = house11BeginLongitude = HouseJunctionPoint(house10MiddleLongitude, house11MiddleLongitude);
                house11EndLongitude = house12BeginLongitude = HouseJunctionPoint(house11MiddleLongitude, house12MiddleLongitude);
                house12EndLongitude = house1BeginLongitude = HouseJunctionPoint(house12MiddleLongitude, house1MiddleLongitude);

                //4.0 Initialize houses into list
                var houseList = new List<House>();

                houseList.Add(new House(HouseName.House1, house1BeginLongitude, house1MiddleLongitude, house1EndLongitude));
                houseList.Add(new House(HouseName.House2, house2BeginLongitude, house2MiddleLongitude, house2EndLongitude));
                houseList.Add(new House(HouseName.House3, house3BeginLongitude, house3MiddleLongitude, house3EndLongitude));
                houseList.Add(new House(HouseName.House4, house4BeginLongitude, house4MiddleLongitude, house4EndLongitude));
                houseList.Add(new House(HouseName.House5, house5BeginLongitude, house5MiddleLongitude, house5EndLongitude));
                houseList.Add(new House(HouseName.House6, house6BeginLongitude, house6MiddleLongitude, house6EndLongitude));
                houseList.Add(new House(HouseName.House7, house7BeginLongitude, house7MiddleLongitude, house7EndLongitude));
                houseList.Add(new House(HouseName.House8, house8BeginLongitude, house8MiddleLongitude, house8EndLongitude));
                houseList.Add(new House(HouseName.House9, house9BeginLongitude, house9MiddleLongitude, house9EndLongitude));
                houseList.Add(new House(HouseName.House10, house10BeginLongitude, house10MiddleLongitude, house10EndLongitude));
                houseList.Add(new House(HouseName.House11, house11BeginLongitude, house11MiddleLongitude, house11EndLongitude));
                houseList.Add(new House(HouseName.House12, house12BeginLongitude, house12MiddleLongitude, house12EndLongitude));


                return houseList;

            }

        }

        /// <summary>
        /// Convert LMT to Julian Days used in Swiss Ephemeris
        /// </summary>
        public static double ConvertLmtToJulian(Time time)
        {

            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey(nameof(ConvertLmtToJulian), time, Ayanamsa), _convertLmtToJulian);


            //UNDERLYING FUNCTION
            double _convertLmtToJulian()
            {
                //get lmt time
                DateTimeOffset lmtDateTime = time.GetLmtDateTimeOffset();

                //split lmt time to pieces
                int year = lmtDateTime.Year;
                int month = lmtDateTime.Month;
                int day = lmtDateTime.Day;
                double hour = (lmtDateTime.TimeOfDay).TotalHours;

                //set calender type
                int gregflag = SwissEph.SE_GREG_CAL; //GREGORIAN CALENDAR

                //declare output variables
                double localMeanTimeInJulian_UT;

                //initialize ephemeris
                SwissEph ephemeris = new SwissEph();

                //get lmt in julian day in Universal Time (UT)
                localMeanTimeInJulian_UT = ephemeris.swe_julday(year, month, day, hour, gregflag);//time to Julian Day

                return localMeanTimeInJulian_UT;

            }

        }

        /// <summary>
        /// Gets all the planets that are in conjunction with the inputed planet
        ///
        /// Note:
        /// 1.The planet inputed is not included in return list
        /// 
        /// 2. Theory behind conjunction
        /// Conjunction :-Two heavenly bodies in the same longitude.
        ///
        /// "The effect of an aspect is felt even if the planets are not
        /// exactly in the mutual distances mentioned above. Therefore
        /// a so-called orb of aspect, and this varies in each aspect is allowed."
        /// The orbs of aspects are :
        /// Conjunction = 8° degrees
        ///
        /// -Planets can be in same sign but not conjunct :
        /// "There are also other variations
        /// of aspects brought about by two planets remaining in the
        /// same sign and not in conjunction but another planet occupying
        /// a trine in respect of the two."
        /// </summary>
        public static List<PlanetName> PlanetsInConjuction(PlanetName inputPlanet, Time time)
        {
            //set 8° degrees as max space around planet where conjunction occurs
            var conjunctionOrbMax = new Angle(8, 0, 0);

            //get longitude of inputed planet
            var inputedPlanet = PlanetNirayanaLongitude(inputPlanet, time);

            //get all planet longitudes
            List<PlanetLongitude> allPlanetLongitudeList = AllPlanetLongitude(time);

            //a place to store conjunct planets 
            var conjunctPlanets = new List<PlanetName>();

            //loop through each planet
            foreach (var planet in allPlanetLongitudeList)
            {
                //skip the inputed planet
                if (planet.GetPlanetName() == inputPlanet) { continue; }

                //get the space between the planet in longitude
                var spaceBetween = DistanceBetweenPlanets(inputedPlanet, planet.GetPlanetLongitude());

                //if space between is from 0° to 8°, then it is conjunct
                if (spaceBetween >= Angle.Zero && spaceBetween <= conjunctionOrbMax)
                {
                    conjunctPlanets.Add(planet.GetPlanetName());
                }

            }

            //return list
            return conjunctPlanets;
        }

        /// <summary>
        /// Gets longitudinal space between 2 planets
        /// Note :
        /// - Longitude of planet after 360 is 0 degrees,
        ///   when calculating difference this needs to be accounted for.
        /// - Calculation in Nirayana longitudes
        /// - Calculates longitudes for you
        /// </summary>
        public static Angle DistanceBetweenPlanets(PlanetName planet1, PlanetName planet2, Time time)
        {
            var planet1Longitude = PlanetNirayanaLongitude(planet1, time);
            var planet2Longitude = PlanetNirayanaLongitude(planet2, time);

            var distanceBetweenPlanets = planetDistance(planet1Longitude.TotalDegrees, planet2Longitude.TotalDegrees);

            return Angle.FromDegrees(distanceBetweenPlanets);



            //---------------FUNCTION---------------


            double planetDistance(double len1, double len2)
            {
                double d = red_deg(Math.Abs(len2 - len1));

                if (d > 180) return (360 - d);

                return d;
            }

            //Reduces a given double value modulo 360.
            //The return value is between 0 and 360.
            double red_deg(double input) => a_red(input, 360);

            //Reduces a given double value x modulo the double a(should be positive).
            //The return value is between 0 and a.
            double a_red(double x, double a) => (x - Math.Floor(x / a) * a);

        }

        /// <summary>
        /// Gets longitudinal space between 2 planets
        /// Note :
        /// - Longitude of planet after 360 is 0 degrees,
        ///   when calculating difference this needs to be accounted for
        /// - Expects you to calculate longitude
        /// </summary>
        public static Angle DistanceBetweenPlanets(Angle planet1, Angle planet2)
        {

            var distanceBetweenPlanets = planetDistance(planet1.TotalDegrees, planet2.TotalDegrees);

            return Angle.FromDegrees(distanceBetweenPlanets);



            //---------------FUNCTION---------------


            double planetDistance(double len1, double len2)
            {
                double d = red_deg(Math.Abs(len2 - len1));

                if (d > 180) return (360 - d);

                return d;
            }

            //Reduces a given double value modulo 360.
            //The return value is between 0 and 360.
            double red_deg(double input) => a_red(input, 360);

            //Reduces a given double value x modulo the double a(should be positive).
            //The return value is between 0 and a.
            double a_red(double x, double a) => (x - Math.Floor(x / a) * a);

        }

        /// <summary>
        /// Gets list of all planets that's in a house/bhava at a given time
        /// based on house longitudes and not sign
        /// </summary>
        public static List<PlanetName> PlanetsInHouse(HouseName houseNumber, Time time)
        {
            //declare empty list of planets
            var listOfPlanetInHouse = new List<PlanetName>();

            //get all houses
            var houseList = AllHouseMiddleLongitudes(time);

            //find house that matches input house number
            var house = houseList.Find(h => h.GetHouseName() == houseNumber);

            //get all planet longitudes
            List<PlanetLongitude> allPlanetLongitudeList = AllPlanetLongitude(time);

            //loop through each planet in house
            foreach (var planet in allPlanetLongitudeList)
            {
                //check if planet is in house
                bool planetIsInHouse = house.IsLongitudeInHouseRange(planet.GetPlanetLongitude());

                if (planetIsInHouse)
                {
                    //add to list if planet is in house
                    listOfPlanetInHouse.Add(planet.GetPlanetName());
                }
            }

            //return list
            return listOfPlanetInHouse;
        }



        /// <summary>
        /// Gets list of all planets that's in a house at a given time
        /// based on sign the house and planet is in and not house longitudes
        /// </summary>
        public static List<PlanetName> PlanetsInHouseBasedOnSign(HouseName houseNumber, Time time)
        {
            //get house sign
            var houseSign = Calculate.HouseSignName(houseNumber, time);

            //get all planets in sign
            var planetsInSign = Calculate.PlanetsInSign(houseSign, time);

            //return list
            return planetsInSign;
        }

        /// <summary>
        /// Gets list of all planets that's in a sign at a given time
        /// </summary>
        public static List<PlanetName> PlanetsInSign(ZodiacName signName, Time time)
        {
            var returnList = new List<PlanetName>();

            //check each planet if in sign
            foreach (var planet in All9Planets)
            {
                var planetSign = PlanetZodiacSign(planet, time);

                //if correct sign add to return list
                if (planetSign.GetSignName() == signName) { returnList.Add(planet); }
            }

            return returnList;
        }




        /// <summary>
        /// Gets the Nirayana longitude of all 9 planets
        /// </summary>
        public static List<PlanetLongitude> AllPlanetLongitude(Time time)
        {
            //get longitudes of all planets
            var sunLongitude = PlanetNirayanaLongitude(Sun, time);
            var sun = new PlanetLongitude(Sun, sunLongitude);

            var moonLongitude = PlanetNirayanaLongitude(Moon, time);
            var moon = new PlanetLongitude(Moon, moonLongitude);

            var marsLongitude = PlanetNirayanaLongitude(Mars, time);
            var mars = new PlanetLongitude(Mars, marsLongitude);

            var mercuryLongitude = PlanetNirayanaLongitude(Mercury, time);
            var mercury = new PlanetLongitude(Mercury, mercuryLongitude);

            var jupiterLongitude = PlanetNirayanaLongitude(Jupiter, time);
            var jupiter = new PlanetLongitude(Jupiter, jupiterLongitude);

            var venusLongitude = PlanetNirayanaLongitude(Venus, time);
            var venus = new PlanetLongitude(Venus, venusLongitude);

            var saturnLongitude = PlanetNirayanaLongitude(Saturn, time);
            var saturn = new PlanetLongitude(Saturn, saturnLongitude);

            var rahuLongitude = PlanetNirayanaLongitude(Rahu, time);
            var rahu = new PlanetLongitude(Rahu, rahuLongitude);

            var ketuLongitude = PlanetNirayanaLongitude(Ketu, time);
            var ketu = new PlanetLongitude(Ketu, ketuLongitude);


            //add longitudes to list
            var allPlanetLongitudeList = new List<PlanetLongitude>
            {
                sun, moon, mars, mercury, jupiter, venus, saturn, ketu, rahu
            };


            //return list;
            return allPlanetLongitudeList;
        }

        /// <summary>
        /// Gets longitude positions of all planets Sayana / Fixed zodiac 
        /// </summary>
        public static List<PlanetLongitude> AllPlanetFixedLongitude(Time time)
        {
            //get longitudes of all planets
            var sunLongitude = PlanetSayanaLongitude(Sun, time);
            var sun = new PlanetLongitude(Sun, sunLongitude);

            var moonLongitude = PlanetSayanaLongitude(Moon, time);
            var moon = new PlanetLongitude(Moon, moonLongitude);

            var marsLongitude = PlanetSayanaLongitude(Mars, time);
            var mars = new PlanetLongitude(Mars, marsLongitude);

            var mercuryLongitude = PlanetSayanaLongitude(Mercury, time);
            var mercury = new PlanetLongitude(Mercury, mercuryLongitude);

            var jupiterLongitude = PlanetSayanaLongitude(Jupiter, time);
            var jupiter = new PlanetLongitude(Jupiter, jupiterLongitude);

            var venusLongitude = PlanetSayanaLongitude(Venus, time);
            var venus = new PlanetLongitude(Venus, venusLongitude);

            var saturnLongitude = PlanetSayanaLongitude(Saturn, time);
            var saturn = new PlanetLongitude(Saturn, saturnLongitude);

            var rahuLongitude = PlanetSayanaLongitude(Rahu, time);
            var rahu = new PlanetLongitude(Rahu, rahuLongitude);

            var ketuLongitude = PlanetSayanaLongitude(Ketu, time);
            var ketu = new PlanetLongitude(Ketu, ketuLongitude);


            //add longitudes to list
            var allPlanetLongitudeList = new List<PlanetLongitude>
            {
                sun, moon, mars, mercury, jupiter, venus, saturn, ketu, rahu
            };


            //return list;
            return allPlanetLongitudeList;
        }

        /// <summary>
        /// Gets the House number a given planet is in at a time
        /// </summary>
        public static HouseName HousePlanetOccupies(PlanetName planetName, Time time)
        {
            //get the planets longitude
            var planetLongitude = PlanetNirayanaLongitude(planetName, time);

            //get all houses
            var houseList = AllHouseMiddleLongitudes(time);

            //loop through all houses
            foreach (var house in houseList)
            {
                //check if planet is in house's range
                var planetIsInHouse = house.IsLongitudeInHouseRange(planetLongitude);

                //if planet is in house
                if (planetIsInHouse)
                {
                    //return house's number
                    return house.GetHouseName();
                }
            }

            //if planet not found in any house, raise error
            throw new Exception("Planet not in any house, error!");

        }



        /// <summary>
        /// List of all planets and the houses they are located in at a given time based on zodiac sign.
        /// </summary>
        public static Dictionary<PlanetName, HouseName> HouseAllPlanetOccupiesBasedOnSign(Time time)
        {
            //get all the signs of the planets
            var planetSigns = Calculate.AllPlanetSigns(time);

            //get all signs of houses at middle longitude 
            var houseSigns = Calculate.AllHouseSign(time);

            //make new list combined data
            var returnList = new Dictionary<PlanetName, HouseName>();
            foreach (var planet in PlanetName.All9Planets)
            {
                var planetSign = planetSigns[planet];

                var foundHouse = houseSigns.Where(yy => yy.Value.GetSignName() == planetSign.GetSignName()).FirstOrDefault();

                returnList.Add(planet, foundHouse.Key);
            }

            return returnList;
        }

        /// <summary>
        /// List of all planets and the houses they are located in at a given time
        /// </summary>
        public static Dictionary<PlanetName, HouseName> HouseAllPlanetOccupies(Time time)
        {
            var returnList = new Dictionary<PlanetName, HouseName>();

            foreach (var planet in PlanetName.All9Planets)
            {
                var houseIsIn = HousePlanetOccupies(planet, time);
                returnList.Add(planet, houseIsIn);
            }

            return returnList;
        }

        /// <summary>
        /// Gets lord of given house at given time
        /// The lord is the Graha (planet) in whose Rasi (sign) the Bhavamadhya falls
        /// </summary>
        public static PlanetName LordOfHouse(HouseName houseNumber, Time time)
        {
            //get sign name based on house number
            var houseSignName = HouseSignName(houseNumber, time);

            //get the lord of the house sign
            var lordOfHouseSign = LordOfZodiacSign(houseSignName);

            return lordOfHouseSign;
        }



        /// <summary>
        /// Gets the lord of zodiac sign planet is in, aka "Planet Sign Lord"
        /// </summary>
        public static PlanetName PlanetLordOfZodiacSign(PlanetName inputPlanet, Time time)
        {
            // Calculate the Nirayana longitude (sidereal longitude in Vedic astrology) 
            // of the current planet at the birth time.
            var nirayanaDegrees = PlanetNirayanaLongitude(inputPlanet, time);

            var zodiacSign = ZodiacSignAtLongitude(nirayanaDegrees);

            return LordOfZodiacSign(zodiacSign.GetSignName());
        }

        /// <summary>
        /// Gets the lord of constellation planet is in, aka "Planet Star Lord"
        /// </summary>
        public static PlanetName PlanetLordOfConstellation(PlanetName inputPlanet, Time time)
        {
            // Calculate the Nirayana longitude (sidereal longitude in Vedic astrology) 
            // of the current planet at the birth time.
            var nirayanaDegrees = PlanetNirayanaLongitude(inputPlanet, time);

            // The value is the lord of the constellation at the planet's longitude
            var value = LordOfConstellation(ConstellationAtLongitude(nirayanaDegrees).GetConstellationName());

            // Add the key-value pair to the dictionary
            return value;
        }

        /// <summary>
        /// The lord of a bhava is
        /// the Graha (planet) in whose Rasi (sign) the Bhavamadhya falls
        /// List overload to GetLordOfHouse (above method)
        /// </summary>
        public static List<PlanetName> LordOfHouseList(List<HouseName> houseList, Time time)
        {
            var returnList = new List<PlanetName>();
            foreach (var house in houseList)
            {
                var tempLord = LordOfHouse(house, time);
                returnList.Add(tempLord);
            }

            return returnList;
        }

        /// <summary>
        /// Checks if the inputed sign was the sign of the house during the inputed time
        /// </summary>
        public static bool IsHouseSignName(HouseName house, ZodiacName sign, Time time) => HouseSignName(house, time) == sign;

        /// <summary>
        /// Gets only the the zodiac sign name at middle longitude of the house.
        /// </summary>
        public static ZodiacName HouseSignName(HouseName houseNumber, Time time)
        {
            //if empty return aries, can't give empty because no Empty for ZodiacName
            if (houseNumber == HouseName.Empty) { return ZodiacName.Aries; }

            //get full sign data
            var zodiacSign = Calculate.HouseZodiacSign(houseNumber, time);

            //only return name
            return zodiacSign.GetSignName();
        }

        /// <summary>
        /// Gets the zodiac sign at middle longitude of the house with degrees data
        /// </summary>
        public static ZodiacSign HouseZodiacSign(HouseName houseNumber, Time time)
        {
            //get all houses
            var allHouses = AllHouseMiddleLongitudes(time);

            //get the house specified 
            var specifiedHouse = allHouses.Find(house => house.GetHouseName() == houseNumber);

            //get sign of the specified house
            var middleLongitude = specifiedHouse.GetMiddleLongitude();
            var houseSign = ZodiacSignAtLongitude(middleLongitude);

            //return the name of house sign
            return houseSign;
        }


        /// <summary>
        /// Gets the zodiac sign at middle longitude of the house.
        /// </summary>
        public static Dictionary<HouseName, ZodiacSign> AllHouseSign(Time time)
        {
            //get all houses
            var allHouses = new Dictionary<HouseName, ZodiacSign>();

            //get for all houses
            foreach (var house in Library.House.AllHouses)
            {
                var calcHouseSign = Calculate.HouseZodiacSign(house, time);
                allHouses.Add(house, calcHouseSign);
            }

            return allHouses;
        }

        /// <summary>
        /// For all houses.
        /// Calculate Lord of Star (Constellation) given Constellation. Returns Star Lord Name
        /// </summary>
        public static Dictionary<HouseName, PlanetName> AllHouseConstellationLord(Time time)
        {
            //get all houses
            var allHouses = new Dictionary<HouseName, PlanetName>();

            //get for all houses
            foreach (var house in VedAstro.Library.House.AllHouses)
            {
                // get constellation house is in middle longitude
                var houseConste = Calculate.HouseConstellation(house, time);

                //get lord based on constellation
                var calcResult = Calculate.LordOfConstellation(houseConste.GetConstellationName());
                allHouses.Add(house, calcResult);
            }

            return allHouses;
        }



        /// <summary>
        /// Gets the constellation at middle longitude of the house.
        /// </summary>
        public static Constellation HouseConstellation(HouseName houseNumber, Time time)
        {
            //get all houses
            var allHouses = AllHouseMiddleLongitudes(time);

            //get the house specified 
            var specifiedHouse = allHouses.Find(house => house.GetHouseName() == houseNumber);

            //get sign of the specified house
            //Note :
            //When the middle longitude has just entered a new sign,
            //rounding the longitude shows better accuracy.
            //Example, with middle longitude 90.4694, becomes Cancer (0°28'9"),
            //but predictive results points to Gemini (30°0'0"), so rounding is implemented
            var middleLongitude = specifiedHouse.GetMiddleLongitude();
            var houseConstellation = ConstellationAtLongitude(middleLongitude);

            //return the name of house sign
            return houseConstellation;
        }

        /// <summary>
        /// Gets the zodiac sign at middle longitude of the house.
        /// </summary>
        public static Dictionary<HouseName, List<PlanetName>> AllHousePlanetsInHouseBasedOnSign(Time time)
        {
            //get all houses
            var allHouses = new Dictionary<HouseName, List<PlanetName>>();

            //get for all houses
            foreach (var house in VedAstro.Library.House.AllHouses)
            {
                var calcHouseSign = Calculate.PlanetsInHouseBasedOnSign(house, time);
                allHouses.Add(house, calcHouseSign);
            }

            return allHouses;
        }

        /// <summary>
        /// Gets Navamsa sign given a longitude
        /// </summary>
        public static ZodiacName NavamsaSignNameFromLongitude(Angle longitude)
        {
            //1.0 Get ordinary zodiac sign name
            //get ordinary zodiac sign
            var ordinarySign = ZodiacSignAtLongitude(longitude);

            //get name of ordinary sign
            var ordinarySignName = ordinarySign.GetSignName();

            //2.0 Get first navamsa sign
            ZodiacName firstNavamsa;

            switch (ordinarySignName)
            {
                //Aries, Leo, Sagittarius - from Aries.
                case ZodiacName.Aries:
                case ZodiacName.Leo:
                case ZodiacName.Sagittarius:
                    firstNavamsa = ZodiacName.Aries;
                    break;
                //Taurus, Capricorn, Virgo - from Capricorn.
                case ZodiacName.Taurus:
                case ZodiacName.Capricorn:
                case ZodiacName.Virgo:
                    firstNavamsa = ZodiacName.Capricorn;
                    break;
                //Gemini, Libra, Aquarius - from Libra.
                case ZodiacName.Gemini:
                case ZodiacName.Libra:
                case ZodiacName.Aquarius:
                    firstNavamsa = ZodiacName.Libra;
                    break;
                //Cancer, Scorpio, Pisces - from Cancer.
                case ZodiacName.Cancer:
                case ZodiacName.Scorpio:
                case ZodiacName.Pisces:
                    firstNavamsa = ZodiacName.Cancer;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            //3.0 Get the number of the navamsa currently in
            //get degrees in ordinary sign
            var degreesInOrdinarySign = ordinarySign.GetDegreesInSign();

            //declare length of a navamsa in the ecliptic arc
            const double navamsaLenghtInDegrees = 3.333333333;

            //divide total degrees in current sign to get raw navamsa number
            var rawNavamsaNumber = degreesInOrdinarySign.TotalDegrees / navamsaLenghtInDegrees;

            //round the raw number to get current navamsa number
            var navamsaNumber = (int)Math.Ceiling(rawNavamsaNumber);

            //4.0 Get navamsa sign
            //count from first navamsa sign
            ZodiacName signAtNavamsa = SignCountedFromInputSign(firstNavamsa, navamsaNumber);

            return signAtNavamsa;

        }

        /// <summary>
        /// Exp : Get 4th sign from Cancer
        /// </summary>
        public static ZodiacName SignCountedFromInputSign(ZodiacName inputSign, int countToNextSign)
        {
            //assign counted to same as starting sign at first
            ZodiacName signCountedTo = inputSign;

            //get the next sign the same number as the count to next sign
            for (int i = 1; i < countToNextSign; i++)
            {
                //get the next zodiac sign from the current counted to sign
                signCountedTo = NextZodiacSign(signCountedTo);
            }

            return signCountedTo;

        }

        /// <summary>
        /// Exp : Get 4th sign from Moon
        /// </summary>
        public static ZodiacName SignCountedFromPlanetSign(int countToNextSign, PlanetName startPlanet, Time inputTime)
        {
            var planetSignName = PlanetZodiacSign(startPlanet, inputTime).GetSignName();
            return SignCountedFromInputSign(planetSignName, countToNextSign);
        }

        /// <summary>
        /// Exp : Get 4th sign from Saturn
        /// </summary>
        public static ZodiacName SignCountedFromPlanetSign(int countToNextSign, Time inputTime, PlanetName startPlanet)
        {
            var planetSignName = PlanetZodiacSign(startPlanet, inputTime).GetSignName();
            return SignCountedFromInputSign(planetSignName, countToNextSign);
        }

        /// <summary>
        /// Exp : Get 4th sign from Lagna/Ascendant
        /// </summary>
        public static ZodiacName SignCountedFromLagnaSign(int countToNextSign, Time inputTime) => SignCountedFromInputSign(LagnaSignName(inputTime), countToNextSign);

        /// <summary>
        /// Exp : Get 4th house from 5th house (input house)
        /// </summary>
        public static int HouseCountedFromInputHouse(int inputHouseNumber, int countToNextHouse)
        {
            //assign count to same as starting house at first
            int houseCountedTo = inputHouseNumber;

            //get the next house the same number as the count to next house
            for (int i = 1; i < countToNextHouse; i++)
            {
                //get the next house number from the current counted to house
                houseCountedTo = NextHouseNumber(houseCountedTo);
            }

            return houseCountedTo;

        }


        /// <summary>
        /// Checks if a given planet is in a given sign at a given time
        /// </summary>
        public static bool IsPlanetInSign(PlanetName planetName, ZodiacName signInput, Time time)
        {
            var currentSign = PlanetZodiacSign(planetName, time).GetSignName();

            //check if sign match
            return currentSign == signInput;
        }

        /// <summary>
        /// Get Navamsa sign of planet at a given time
        /// </summary>
        public static ZodiacName PlanetNavamsaSign(PlanetName planetName, Time time)
        {
            //get planets longitude
            var planetLongitude = PlanetNirayanaLongitude(planetName, time);

            //get navamsa sign at longitude
            var navamsaSignOfPlanet = NavamsaSignNameFromLongitude(planetLongitude);

            return navamsaSignOfPlanet;
        }

        /// <summary>
        /// Gives a list of all zodiac signs a specified planet is aspecting
        /// 
        /// All their location with a quarter sight, the 5th and the
        /// 9th houses with a half sight, the 4th and the 8th houses
        /// with three-quarters of a sight and the 7th house with
        /// a full sight.
        /// </summary>
        public static List<ZodiacName> SignsPlanetIsAspecting(PlanetName planetName, Time time)
        {
            //create empty list of signs
            var planetSignList = new List<ZodiacName>();

            //get zodiac sign name which the planet is currently in
            var planetSignName = PlanetZodiacSign(planetName, time).GetSignName();

            // Saturn powerfully aspects the 3rd and the 10th houses
            if (planetName == Saturn)
            {
                //get signs planet is aspecting
                var sign3FromSaturn = SignCountedFromInputSign(planetSignName, 3);
                var sign10FromSaturn = SignCountedFromInputSign(planetSignName, 10);

                //add signs to return list
                planetSignList.Add(sign3FromSaturn);
                planetSignList.Add(sign10FromSaturn);

            }

            // Mandi or Gulika is said to aspect 2nd, 7th
            // and 12th Houses from the sign of occupation. 
            if (planetName == Maandi)
            {
                //get signs planet is aspecting
                var sign2ndFromMaandi = SignCountedFromInputSign(planetSignName, 2);
                var sign7thFromMaandi = SignCountedFromInputSign(planetSignName, 7);
                var sign12thFromMaandi = SignCountedFromInputSign(planetSignName, 12);

                //add signs to return list
                planetSignList.Add(sign2ndFromMaandi);
                planetSignList.Add(sign7thFromMaandi);
                planetSignList.Add(sign12thFromMaandi);
            }

            // Mandi or Gulika is said to aspect 2nd, 7th
            // and 12th Houses from the sign of occupation. 
            if (planetName == Gulika)
            {
                //get signs planet is aspecting
                var sign2ndFromGulika = SignCountedFromInputSign(planetSignName, 2);
                var sign7thFromGulika = SignCountedFromInputSign(planetSignName, 7);
                var sign12thFromGulika = SignCountedFromInputSign(planetSignName, 12);

                //add signs to return list
                planetSignList.Add(sign2ndFromGulika);
                planetSignList.Add(sign7thFromGulika);
                planetSignList.Add(sign12thFromGulika);
            }

            // Jupiter the 5th and the 9th houses
            if (planetName == Jupiter)
            {
                //get signs planet is aspecting
                var sign5FromJupiter = SignCountedFromInputSign(planetSignName, 5);
                var sign9FromJupiter = SignCountedFromInputSign(planetSignName, 9);

                //add signs to return list
                planetSignList.Add(sign5FromJupiter);
                planetSignList.Add(sign9FromJupiter);

            }

            // Mars, the 4th and the 8th houses
            if (planetName == Mars)
            {
                //get signs planet is aspecting
                var sign4FromMars = SignCountedFromInputSign(planetSignName, 4);
                var sign8FromMars = SignCountedFromInputSign(planetSignName, 8);

                //add signs to return list
                planetSignList.Add(sign4FromMars);
                planetSignList.Add(sign8FromMars);

            }

            //All planets aspect 7th house

            //get signs planet is aspecting
            var sign7FromPlanet = SignCountedFromInputSign(planetSignName, 7);

            //add signs to return list
            planetSignList.Add(sign7FromPlanet);


            return planetSignList;

        }

        /// <summary>
        /// Get navamsa sign of house (mid point)
        /// TODO: Checking for correctness needed
        /// </summary>
        public static ZodiacName HouseNavamsaSign(HouseName house, Time time)
        {
            //if empty return Aries
            if (house == HouseName.Empty) { return ZodiacName.Aries; }

            //get all houses
            var allHouseList = AllHouseMiddleLongitudes(time);

            //get house mid longitude
            var houseMiddleLongitude = allHouseList.Find(hs => hs.GetHouseName() == house).GetMiddleLongitude();

            //get navamsa house sign at house mid longitude
            var navamsaSign = NavamsaSignNameFromLongitude(houseMiddleLongitude);

            return navamsaSign;
        }

        /// <summary>
        /// Get Thrimsamsa sign of house (mid point)
        /// </summary>
        public static ZodiacName PlanetThrimsamsaSign(PlanetName planetName, Time time)
        {
            //get sign planet is in
            var planetSign = PlanetZodiacSign(planetName, time);

            //get planet sign name
            var planetSignName = planetSign.GetSignName();

            //get degrees in sign 
            var degreesInSign = planetSign.GetDegreesInSign().TotalDegrees;

            //declare const number for Thrimsamsa calculation
            const double maxThrimsamsaDegrees = 1; // 30/1
            const double maxSignDegrees = 30.0;

            //get rough Thrimsamsa number
            double roughThrimsamsaNumber = (degreesInSign % maxSignDegrees) / maxThrimsamsaDegrees;

            //get rounded saptamsa number
            var thrimsamsaNumber = (int)Math.Ceiling(roughThrimsamsaNumber);

            //if planet is in odd sign
            if (IsOddSign(planetSignName))
            {
                //1,2,3,4,5 - Mars
                if (thrimsamsaNumber >= 0 && thrimsamsaNumber <= 5)
                {
                    //Aries and Scorpio are ruled by Mars
                    return ZodiacName.Scorpio;
                }
                //6,7,8,9,10 - saturn
                if (thrimsamsaNumber >= 6 && thrimsamsaNumber <= 10)
                {
                    //Capricorn and Aquarius by Saturn.
                    return ZodiacName.Capricorn;

                }
                //11,12,13,14,15,16,17,18 - jupiter
                if (thrimsamsaNumber >= 11 && thrimsamsaNumber <= 18)
                {
                    //Sagittarius and Pisces by Jupiter
                    return ZodiacName.Sagittarius;

                }
                //19,20,21,22,23,24,25 - mercury
                if (thrimsamsaNumber >= 19 && thrimsamsaNumber <= 25)
                {
                    //Gemini and Virgo by Mercury
                    return ZodiacName.Gemini;
                }
                //26,27,28,29,30 - venus
                if (thrimsamsaNumber >= 26 && thrimsamsaNumber <= 30)
                {
                    //Taurus and Libra by Venus;
                    return ZodiacName.Taurus;
                }

            }

            //if planet is in even sign
            if (IsEvenSign(planetSignName))
            {
                //1,2,3,4,5 - venus
                if (thrimsamsaNumber >= 0 && thrimsamsaNumber <= 5)
                {
                    //Taurus and Libra by Venus;
                    return ZodiacName.Taurus;
                }
                //6,7,8,9,10,11,12 - mercury
                if (thrimsamsaNumber >= 6 && thrimsamsaNumber <= 12)
                {
                    //Gemini and Virgo by Mercury
                    return ZodiacName.Gemini;
                }
                //13,14,15,16,17,18,19,20 - jupiter
                if (thrimsamsaNumber >= 13 && thrimsamsaNumber <= 20)
                {
                    //Sagittarius and Pisces by Jupiter
                    return ZodiacName.Sagittarius;

                }
                //21,22,23,24,25 - saturn
                if (thrimsamsaNumber >= 21 && thrimsamsaNumber <= 25)
                {
                    //Capricorn and Aquarius by Saturn.
                    return ZodiacName.Capricorn;

                }
                //26,27,28,29,30 - Mars
                if (thrimsamsaNumber >= 26 && thrimsamsaNumber <= 30)
                {
                    //Aries and Scorpio are ruled by Mars
                    return ZodiacName.Scorpio;
                }

            }

            throw new Exception("Thrimsamsa not found, error!");
        }

        /// <summary>
        /// When a sign is divided into 12 equal parts each is called a dwadasamsa and measures 2.5 degrees.
        /// The Bhachakra can thus he said to contain 12x12=144 Dwadasamsas. The lords of the 12
        /// Dwadasamsas in a sign are the lords of the 12 signs from it, i.e.,
        /// the lord of the first Dwadasamsa in Mesha is Kuja, that of the second Sukra and so on.
        /// </summary>
        public static ZodiacName PlanetDwadasamsaSign(PlanetName planetName, Time time)
        {
            //get sign planet is in
            var planetSign = PlanetZodiacSign(planetName, time);

            //get planet sign name
            var planetSignName = planetSign.GetSignName();

            //get degrees in sign 
            var degreesInSign = planetSign.GetDegreesInSign().TotalDegrees;

            //declare const number for Dwadasamsa calculation
            const double maxDwadasamsaDegrees = 2.5; // 30/12
            const double maxSignDegrees = 30.0;

            //get rough Dwadasamsa number
            double roughDwadasamsaNumber = (degreesInSign % maxSignDegrees) / maxDwadasamsaDegrees;

            //get rounded Dwadasamsa number
            var dwadasamsaNumber = (int)Math.Ceiling(roughDwadasamsaNumber);

            //get Dwadasamsa sign from counting with Dwadasamsa number
            var dwadasamsaSign = SignCountedFromInputSign(planetSignName, dwadasamsaNumber);

            return dwadasamsaSign;
        }

        /// <summary>
        /// sign is divided into 7 equal parts each is called a Saptamsa and measures 4.28 degrees
        /// </summary>
        public static ZodiacName PlanetSaptamsaSign(PlanetName planetName, Time time)
        {
            //get sign planet is in
            var planetSign = PlanetZodiacSign(planetName, time);

            //get planet sign name
            var planetSignName = planetSign.GetSignName();

            //get degrees in sign 
            var degreesInSign = planetSign.GetDegreesInSign().TotalDegrees;

            //declare const number for saptamsa calculation
            const double maxSaptamsaDegrees = 4.285714285714286; // 30/7
            const double maxSignDegrees = 30.0;

            //get rough saptamsa number
            double roughSaptamsaNumber = (degreesInSign % maxSignDegrees) / maxSaptamsaDegrees;

            //get rounded saptamsa number
            var saptamsaNumber = (int)Math.Ceiling(roughSaptamsaNumber);

            //2.0 Get even or odd sign

            //if planet is in odd sign
            if (IsOddSign(planetSignName))
            {
                //convert saptamsa number to zodiac name
                return SignCountedFromInputSign(planetSignName, saptamsaNumber);
            }

            //if planet is in even sign
            if (IsEvenSign(planetSignName))
            {
                var countToNextSign = saptamsaNumber + 6;
                return SignCountedFromInputSign(planetSignName, countToNextSign);
            }


            throw new Exception("Saptamsa not found, error!");
        }

        public static ZodiacSign PlanetPanchamsaSign(PlanetName planetName, Time time) => Calculate.PanchamsaSignName(Calculate.PlanetZodiacSign(planetName, time));

        public static ZodiacSign PanchamsaSignName(ZodiacSign zodiacSign)
        {
            //TODO
            return new ZodiacSign();

            throw new Exception("END OF LINE!");
        }

        /// <summary>
        /// Similar to Exaltation but covers a range not just a point
        /// Moolathrikonas, these are positions similar to exaltation.
        /// NOTE:
        /// - No moolatrikone for Rahu & Ketu, no error will be raised
        /// </summary>
        public static bool IsPlanetInMoolatrikona(PlanetName planetName, Time time)
        {
            //get sign planet is in
            var planetSign = PlanetZodiacSign(planetName, time);

            //Sun's Moola Thrikona is Leo (0°-20°);
            if (planetName == Sun)
            {
                if (planetSign.GetSignName() == ZodiacName.Leo)
                {
                    var degreesInSign = planetSign.GetDegreesInSign().TotalDegrees;

                    if (degreesInSign >= 0 && degreesInSign <= 20)
                    {
                        return true;
                    }
                }
            }

            //Moon-Taurus (4°-30°);
            if (planetName == Moon)
            {
                if (planetSign.GetSignName() == ZodiacName.Taurus)
                {
                    var degreesInSign = planetSign.GetDegreesInSign().TotalDegrees;

                    if (degreesInSign >= 4 && degreesInSign <= 30)
                    {
                        return true;
                    }
                }
            }

            //Mercury-Virgo (16°-20°);
            if (planetName == Mercury)
            {
                if (planetSign.GetSignName() == ZodiacName.Virgo)
                {
                    var degreesInSign = planetSign.GetDegreesInSign().TotalDegrees;

                    if (degreesInSign >= 16 && degreesInSign <= 20)
                    {
                        return true;
                    }
                }
            }

            //Jupiter-Sagittarius (0°-13°);
            if (planetName == Jupiter)
            {
                if (planetSign.GetSignName() == ZodiacName.Sagittarius)
                {
                    var degreesInSign = planetSign.GetDegreesInSign().TotalDegrees;

                    if (degreesInSign >= 0 && degreesInSign <= 13)
                    {
                        return true;
                    }
                }
            }

            // Mars-Aries (0°-18°);
            if (planetName == Mars)
            {
                if (planetSign.GetSignName() == ZodiacName.Aries)
                {
                    var degreesInSign = planetSign.GetDegreesInSign().TotalDegrees;

                    if (degreesInSign >= 0 && degreesInSign <= 18)
                    {
                        return true;
                    }
                }
            }

            // Venus-Libra (0°-10°)
            if (planetName == Venus)
            {
                if (planetSign.GetSignName() == ZodiacName.Libra)
                {
                    var degreesInSign = planetSign.GetDegreesInSign().TotalDegrees;

                    if (degreesInSign >= 0 && degreesInSign <= 10)
                    {
                        return true;
                    }
                }
            }

            // Saturn-Aquarius (0°-20°).
            if (planetName == Saturn)
            {
                if (planetSign.GetSignName() == ZodiacName.Aquarius)
                {
                    var degreesInSign = planetSign.GetDegreesInSign().TotalDegrees;

                    if (degreesInSign >= 0 && degreesInSign <= 20)
                    {
                        return true;
                    }
                }
            }

            //if no above conditions met, moolatrikonas not happening 
            return false;
        }

        /// <summary>
        /// Gets a planet's relationship to a sign, based on the relation to the lord
        /// Note :
        /// - Moolatrikona, Debilited & Exalted is not calculated heres
        /// - Rahu & ketu not accounted for
        /// </summary>
        public static PlanetToSignRelationship PlanetRelationshipWithSign(PlanetName planetName, ZodiacName zodiacSignName, Time time)
        {

            //no calculation for rahu and ketu here
            var isRahu = planetName.Name == PlanetNameEnum.Rahu;
            var isKetu = planetName.Name == PlanetNameEnum.Ketu;
            var isRahuKetu = isRahu || isKetu;
            if (isRahuKetu) { return PlanetToSignRelationship.Empty; }


            //types of relationship
            //Swavarga - own varga
            //Samavarga - neutral's varga
            //Mitravarga - friendly varga
            //Adhi Mitravarga - Intimate friend varga
            //Satruvarga - enemy's varga
            //Adhi Satruvarga - Bitter enemy varga


            //Get lord of zodiac sign
            var lordOfSign = LordOfZodiacSign(zodiacSignName);

            //if lord of sign is same as input planet
            if (planetName == lordOfSign)
            {
                //return own varga, swavarga
                return PlanetToSignRelationship.OwnVarga;
            }

            //else, get relationship between input planet and lord of sign
            PlanetToPlanetRelationship relationshipToLordOfSign = PlanetCombinedRelationshipWithPlanet(planetName, lordOfSign, time);

            //return relation ship with sign based on relationship with lord of sign
            switch (relationshipToLordOfSign)
            {
                case PlanetToPlanetRelationship.BestFriend:
                    return PlanetToSignRelationship.BestFriendVarga;
                case PlanetToPlanetRelationship.Friend:
                    return PlanetToSignRelationship.FriendVarga;
                case PlanetToPlanetRelationship.BitterEnemy:
                    return PlanetToSignRelationship.BitterEnemyVarga;
                case PlanetToPlanetRelationship.Enemy:
                    return PlanetToSignRelationship.EnemyVarga;
                case PlanetToPlanetRelationship.Neutral:
                    return PlanetToSignRelationship.NeutralVarga;
                default:
                    return PlanetToSignRelationship.Empty;
            }

        }

        /// <summary>
        /// strengths of planets, mix the temporary relations and the permanent
        /// 
        /// In order to find the strengths of planets we have
        /// to mix the temporary relations and the permanent
        /// relations. Thus a temporary enemy plus a permanent
        /// or natural enemy becomes a bitter enemy.
        /// </summary>
        public static PlanetToPlanetRelationship PlanetCombinedRelationshipWithPlanet(PlanetName mainPlanet, PlanetName secondaryPlanet, Time time)
        {

            //no calculation for rahu and ketu here
            var isRahu = mainPlanet.Name == PlanetNameEnum.Rahu;
            var isKetu = mainPlanet.Name == PlanetNameEnum.Ketu;
            var isRahu2 = secondaryPlanet.Name == PlanetNameEnum.Rahu;
            var isKetu2 = secondaryPlanet.Name == PlanetNameEnum.Ketu;
            var isRahuKetu = isRahu || isKetu || isRahu2 || isKetu2;
            if (isRahuKetu) { return PlanetToPlanetRelationship.Empty; }


            //if main planet & secondary planet is same, then it is own plant (same planet), end here
            if (mainPlanet == secondaryPlanet) { return PlanetToPlanetRelationship.SamePlanet; }

            //get planet's permanent relationship
            PlanetToPlanetRelationship planetPermanentRelationship = PlanetPermanentRelationshipWithPlanet(mainPlanet, secondaryPlanet);

            //get planet's temporary relationship
            PlanetToPlanetRelationship planetTemporaryRelationship = PlanetTemporaryRelationshipWithPlanet(mainPlanet, secondaryPlanet, time);

            //Tatkalika Mitra + Naisargika Mitra = Adhi Mitras
            if (planetTemporaryRelationship == PlanetToPlanetRelationship.Friend && planetPermanentRelationship == PlanetToPlanetRelationship.Friend)
            {
                //they both become intimate friends (Adhi Mitras).
                return PlanetToPlanetRelationship.BestFriend;
            }

            //Tatkalika Mitra + Naisargika Satru = Sama
            if (planetTemporaryRelationship == PlanetToPlanetRelationship.Friend && planetPermanentRelationship == PlanetToPlanetRelationship.Enemy)
            {
                return PlanetToPlanetRelationship.Neutral;
            }

            //Tatkalika Mitra + Naisargika Sama = Mitra
            if (planetTemporaryRelationship == PlanetToPlanetRelationship.Friend && planetPermanentRelationship == PlanetToPlanetRelationship.Neutral)
            {
                return PlanetToPlanetRelationship.Friend;
            }

            //Tatkalika Satru + Naisargika Satru = Adhi Satru
            if (planetTemporaryRelationship == PlanetToPlanetRelationship.Enemy && planetPermanentRelationship == PlanetToPlanetRelationship.Enemy)
            {
                return PlanetToPlanetRelationship.BitterEnemy;
            }

            //Tatkalika Satru + Naisargika Mitra = Sama
            if (planetTemporaryRelationship == PlanetToPlanetRelationship.Enemy && planetPermanentRelationship == PlanetToPlanetRelationship.Friend)
            {
                return PlanetToPlanetRelationship.Neutral;
            }

            //Tatkalika Satru + Naisargika Sama = Satru
            if (planetTemporaryRelationship == PlanetToPlanetRelationship.Enemy && planetPermanentRelationship == PlanetToPlanetRelationship.Neutral)
            {
                return PlanetToPlanetRelationship.Enemy;
            }


            return PlanetToPlanetRelationship.Empty;
            throw new Exception("Combined planet relationship not found, error!");
        }

        /// <summary>
        /// Relation between the planet and the lord of the sign of the house
        /// 
        /// Gets a planets relationship with a house,
        /// Based on the relation between the planet and the lord of the sign of the house
        /// Note : needs verification if this is correct
        /// </summary>
        public static PlanetToSignRelationship PlanetRelationshipWithHouse(HouseName house, PlanetName planet, Time time)
        {
            //get sign the house is in
            var houseSign = HouseSignName(house, time);

            //get the planet's relationship with the sign
            var relationship = PlanetRelationshipWithSign(planet, houseSign, time);

            return relationship;
        }

        /// <summary>
        /// Planets found in the certain signs from any other planet becomes temporary friends
        /// 
        /// Temporary Friendship
        /// Planets found in the 2nd, 3rd, 4th, 10th, 11th
        /// and 12th signs from any other planet becomes the
        /// latter's temporary friends. The others are its enemies.
        /// </summary>
        public static PlanetToPlanetRelationship PlanetTemporaryRelationshipWithPlanet(PlanetName mainPlanet, PlanetName secondaryPlanet, Time time)
        {
            //if main planet & secondary planet is same, then it is own plant (same planet), end here
            if (mainPlanet == secondaryPlanet) { return PlanetToPlanetRelationship.SamePlanet; }


            //1.0 get planet's friends
            var friendlyPlanetList = PlanetTemporaryFriendList(mainPlanet, time);

            //check if planet is found in friend list
            var planetFoundInFriendList = friendlyPlanetList.Contains(secondaryPlanet);

            //if found in friend list
            if (planetFoundInFriendList)
            {
                //return relationship as friend
                return PlanetToPlanetRelationship.Friend;
            }

            //if planet is not a friend then it is an enemy
            //return relationship as enemy
            return PlanetToPlanetRelationship.Enemy;
        }

        /// <summary>
        /// Gets all the planets in a sign
        /// </summary>
        public static List<PlanetName> PlanetInSign(ZodiacName signName, Time time)
        {
            //get all planets locations in signs
            var sunSignName = PlanetZodiacSign(Sun, time).GetSignName();
            var moonSignName = PlanetZodiacSign(Moon, time).GetSignName();
            var marsSignName = PlanetZodiacSign(Mars, time).GetSignName();
            var mercurySignName = PlanetZodiacSign(Mercury, time).GetSignName();
            var jupiterSignName = PlanetZodiacSign(Jupiter, time).GetSignName();
            var venusSignName = PlanetZodiacSign(Venus, time).GetSignName();
            var saturnSignName = PlanetZodiacSign(Saturn, time).GetSignName();
            var rahuSignName = PlanetZodiacSign(Rahu, time).GetSignName();
            var ketuSignName = PlanetZodiacSign(Ketu, time).GetSignName();


            //create empty list of planet names to return
            var planetFoundInSign = new List<PlanetName>();

            //if planet is in same sign as input sign add planet to list
            if (sunSignName == signName)
            {
                planetFoundInSign.Add(Sun);
            }
            if (moonSignName == signName)
            {
                planetFoundInSign.Add(Moon);
            }
            if (marsSignName == signName)
            {
                planetFoundInSign.Add(Mars);
            }
            if (mercurySignName == signName)
            {
                planetFoundInSign.Add(Mercury);
            }
            if (jupiterSignName == signName)
            {
                planetFoundInSign.Add(Jupiter);
            }
            if (venusSignName == signName)
            {
                planetFoundInSign.Add(Venus);
            }
            if (saturnSignName == signName)
            {
                planetFoundInSign.Add(Saturn);
            }
            if (rahuSignName == signName)
            {
                planetFoundInSign.Add(Rahu);
            }
            if (ketuSignName == signName)
            {
                planetFoundInSign.Add(Ketu);
            }


            return planetFoundInSign;
        }

        /// <summary>
        /// Get list of Temporary (Tatkalika) Friend for a planet
        /// 
        /// The planets in -the 2nd, 3rd, 4th, 10th, 11th and
        /// 12th signs from any other planet becomes his
        /// (Tatkalika) friend.
        /// </summary>
        public static List<PlanetName> PlanetTemporaryFriendList(PlanetName planetName, Time time)
        {
            //get sign planet is currently in
            var planetSignName = PlanetZodiacSign(planetName, time).GetSignName();

            //Get signs of friends of main planet
            //get planets in 2nd
            var sign2FromMainPlanet = SignCountedFromInputSign(planetSignName, 2);
            //get planets in 3rd
            var sign3FromMainPlanet = SignCountedFromInputSign(planetSignName, 3);
            //get planets in 4th
            var sign4FromMainPlanet = SignCountedFromInputSign(planetSignName, 4);
            //get planets in 10th
            var sign10FromMainPlanet = SignCountedFromInputSign(planetSignName, 10);
            //get planets in 11th
            var sign11FromMainPlanet = SignCountedFromInputSign(planetSignName, 11);
            //get planets in 12th
            var sign12FromMainPlanet = SignCountedFromInputSign(planetSignName, 12);

            //add houses of friendly planets to a list
            var signsOfFriendlyPlanet = new List<ZodiacName>(){sign2FromMainPlanet, sign3FromMainPlanet, sign4FromMainPlanet,
                sign10FromMainPlanet, sign11FromMainPlanet, sign12FromMainPlanet};

            //declare list of friendly planets
            var friendlyPlanetList = new List<PlanetName>();

            //loop through the signs and fill the friendly planet list
            foreach (var sign in signsOfFriendlyPlanet)
            {
                //get the planets in the current sign
                var friendlyPlanetsInThisSign = PlanetInSign(sign, time);

                //add the planets in to the list
                friendlyPlanetList.AddRange(friendlyPlanetsInThisSign);
            }

            //remove rahu & ketu from list
            friendlyPlanetList.Remove(Rahu);
            friendlyPlanetList.Remove(Ketu);


            return friendlyPlanetList;

        }

        /// <summary>
        /// Greenwich Apparent In Julian Days
        /// </summary>
        public static double GreenwichApparentInJulianDays(Time time)
        {
            //convert lmt to julian days, in universal time (UT)
            var localMeanTimeInJulian_UT = GreenwichLmtInJulianDays(time);

            //get longitude of location
            double longitude = time.GetGeoLocation().Longitude();

            //delcare output variables
            double localApparentTimeInJulian;
            string errorString = "";

            //convert lmt to local apparent time (LAT)
            using SwissEph ephemeris = new();
            ephemeris.swe_lmt_to_lat(localMeanTimeInJulian_UT, longitude, out localApparentTimeInJulian, ref errorString);


            return localApparentTimeInJulian;
        }

        /// <summary>
        /// Shows local apparent time from Swiss Eph
        /// </summary>
        public static DateTime LocalApparentTime(Time time)
        {
            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey(nameof(LocalApparentTime), time, Ayanamsa), _getLocalApparentTime);

            //UNDERLYING FUNCTION
            DateTime _getLocalApparentTime()
            {
                //convert lmt to julian days, in universal time (UT)
                var localMeanTimeInJulian_UT = ConvertLmtToJulian(time);

                //get longitude of location
                double longitude = time.GetGeoLocation().Longitude();

                //delcare output variables
                double localApparentTimeInJulian;
                string errorString = null;

                //initialize ephemeris
                SwissEph ephemeris = new SwissEph();

                //convert lmt to local apparent time (LAT)
                ephemeris.swe_lmt_to_lat(localMeanTimeInJulian_UT, longitude, out localApparentTimeInJulian, ref errorString);

                var localApparentTime = ConvertJulianTimeToNormalTime(localApparentTimeInJulian);

                return localApparentTime;

            }

        }

        /// <summary>
        /// House start middle and end longitudes
        /// </summary>
        public static House HouseLongitude(HouseName houseNumber, Time time)
        {
            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey(nameof(HouseLongitude), houseNumber, time, Ayanamsa), _getHouse);

            //UNDERLYING FUNCTION
            House _getHouse()
            {
                //get all house list
                var allHouses = AllHouseMiddleLongitudes(time);

                //get required house from list
                var requiredHouse = allHouses.Find(h => h.GetHouseName() == houseNumber);

                return requiredHouse;

            }

        }

        /// <summary>
        /// Gets Panchaka at a given time
        /// </summary>
        public static PanchakaName Panchaka(Time time)
        {
            //If the remainder is 1 (mrityu panchakam), it
            // indicates danger; if 2 (agni panchakam), risk from fire; if 4 (raja
            // panchakam), bad results; if 6 (chora panchakam), evil happenings and if
            // 8 (roga panchakam), disease. If the remainder is 3, 5, 7 or zero then it is
            // good.

            //get the number of the lunar day (from the 1st of the month),
            var lunarDateNumber = LunarDay(time).GetLunarDateNumber();

            //get the number of the constellation (from Aswini)
            var rullingConstellationNumber = MoonConstellation(time).GetConstellationNumber();

            //Number of weekday
            var weekdayNumber = (int)DayOfWeek(time);

            //Number of zodiacal sign, number of the Lagna (from Aries).
            var risingSignNumber = (int)HouseSignName(HouseName.House1, time);

            //add all to get total
            double total = lunarDateNumber + rullingConstellationNumber + weekdayNumber + risingSignNumber;

            //get modulos of 9 to get panchaka number (Remainder From Division)
            var panchakaNumber = total % 9.0;

            //convert panchakam number to name
            switch (panchakaNumber)
            {
                //1 (mrityu panchakam)
                case 1:
                    return PanchakaName.Mrityu;
                //2 (agni panchakam)
                case 2:
                    return PanchakaName.Agni;
                //4 (raja panchakam)
                case 4:
                    return PanchakaName.Raja;
                //6 (chora panchakam)
                case 6:
                    return PanchakaName.Chora;
                //8 (roga panchakam)
                case 8:
                    return PanchakaName.Roga;
                //If the remainder is 3, 5, 7 or 0 then it is good (shubha)
                case 3:
                case 5:
                case 7:
                case 0:
                    return PanchakaName.Shubha;
            }

            //if panchaka number did not match above, throw error
            throw new Exception("Panchaka not found, error!");


        }

        /// <summary>
        /// Planet lord that governs a weekday
        /// </summary>
        public static PlanetName LordOfWeekday(Time time)
        {
            //Sunday Sun
            //Monday Moon
            //Tuesday Mars
            //Wednesday Mercury
            //Thursday Jupiter
            //Friday Venus
            //Saturday Saturn

            //get the weekday
            var weekday = DayOfWeek(time);

            //based on weekday return the planet lord
            switch (weekday)
            {
                case Library.DayOfWeek.Sunday: return Sun;
                case Library.DayOfWeek.Monday: return Moon;
                case Library.DayOfWeek.Tuesday: return Mars;
                case Library.DayOfWeek.Wednesday: return Mercury;
                case Library.DayOfWeek.Thursday: return Jupiter;
                case Library.DayOfWeek.Friday: return Venus;
                case Library.DayOfWeek.Saturday: return Saturn;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Planet lord that governs a weekday
        /// </summary>
        public static PlanetName LordOfWeekday(DayOfWeek weekday)
        {
            //Sunday Sun
            //Monday Moon
            //Tuesday Mars
            //Wednesday Mercury
            //Thursday Jupiter
            //Friday Venus
            //Saturday Saturn

            //based on weekday return the planet lord
            switch (weekday)
            {
                case Library.DayOfWeek.Sunday: return Sun;
                case Library.DayOfWeek.Monday: return Moon;
                case Library.DayOfWeek.Tuesday: return Mars;
                case Library.DayOfWeek.Wednesday: return Mercury;
                case Library.DayOfWeek.Thursday: return Jupiter;
                case Library.DayOfWeek.Friday: return Venus;
                case Library.DayOfWeek.Saturday: return Saturn;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Convert Local Mean Time (LMT) to Standard Time (STD)
        /// </summary>
        public static DateTimeOffset LmtToStd(DateTimeOffset lmtDateTime, TimeSpan stdOffset)
        {
            //set lmt to offset
            //var tempTime = new DateTimeOffset(lmtDateTime);

            return lmtDateTime.ToOffset(stdOffset);
        }


        /// <summary>
        /// Birth Time In Ghatis
        /// Also know as "Suryodayadi Jananakala Ghatikah".
        /// It is customary among the Hindus to mention
        /// the time of birth as "Suryodayadi Jananakala
        /// Ghatikaha ", i.e., the number of ghatis passed
        /// from sunrise up to the moment of birth.
        /// sunrise to sunrise, and consists of 60 Ghatis
        /// </summary>
        public static Angle IshtaKaala(Time birthTime)
        {
            //check if sunrise before or after
            var isBefore = Calculate.IsBeforeSunrise(birthTime);

            //if birthTime is before sunrise then use previous day sunrise
            TimeSpan timeDifference;
            if (isBefore)
            {
                var preSunrise = Calculate.SunriseTime(birthTime.SubtractHours(23));
                timeDifference = birthTime.Subtract(preSunrise); //sunrise day before
            }
            else
            {
                var sunrise = Calculate.SunriseTime(birthTime);
                timeDifference = birthTime.Subtract(sunrise); ;
            }

            //(Birth Time - Sunrise) x 2.5 = Suryodayadi Jananakala Ghatikaha. 
            var differenceHours = timeDifference.TotalHours;
            var ghatis = differenceHours * 2.5;

            //return round(ghatis to 2 decimal places)
            var xx = VedicTime.FromTotalGhatis(ghatis);
            return Angle.FromDegrees(ghatis);
        }

        /// <summary>
        /// Given a time, will check if it occured before or after sunrise for that given day.
        /// Returns true if given time is before sunrise
        /// </summary>
        public static bool IsBeforeSunrise(Time birthTime)
        {
            //get sunrise for that day
            var sunrise = Calculate.SunriseTime(birthTime);

            //if time is before than it must be smalller
            var isBefore = birthTime < sunrise;

            return isBefore;
        }

        /// <summary>
        /// A hora is equal to 1/24th part of
        /// a day. The Hindu day begins with sunrise and continues till
        /// next sunrise. The first hora on any day will be the
        /// first hour after sunrise and the last hora, the hour
        /// before sunrise the next day.
        /// </summary>
        public static int HoraAtBirth(Time time)
        {
            TimeSpan hours;

            var birthTime = time.GetLmtDateTimeOffset();
            var sunriseTime = SunriseTime(time).GetLmtDateTimeOffset();

            //if birth time is after sunrise, then sunrise time is correct 
            if (birthTime >= sunriseTime)
            {
                //get hours (hora) passed since sunrise (start of day)
                hours = birthTime.Subtract(sunriseTime);
            }
            //else birth has occured before sunrise on that day,
            //so have to use sunrise of the previous day
            else
            {
                //get sunrise of the previous day
                var previousDay = new Time(time.GetLmtDateTimeOffset().DateTime.AddDays(-1), time.GetStdDateTimeOffset().Offset, time.GetGeoLocation());
                sunriseTime = SunriseTime(previousDay).GetLmtDateTimeOffset();

                //get hours (hora) passed since sunrise (start of day)
                hours = birthTime.Subtract(sunriseTime);

            }

            //round hours to highest possible (ceiling)
            var hora = Math.Ceiling(hours.TotalHours);

            //if birth time is exactly as sunrise time hora will be zero here, meaning 1st hora
            if (hora == 0) { hora = 1; }

            //ensure hora is within 1 to 24
            if (hora > 24) { hora = 24; }

            return (int)hora;

        }

        /// <summary>
        /// get sunrise time for that day at that place
        /// </summary>
        public static Time SunriseTime(Time time)
        {

            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey(nameof(SunriseTime), time, Ayanamsa), _getSunriseTime);


            //UNDERLYING FUNCTION
            Time _getSunriseTime()
            {
                //1. Calculate sunrise time

                //prepare data to do calculation
                const int iflag = SwissEph.SEFLG_SWIEPH | SwissEph.SEFLG_SPEED | SwissEph.SEFLG_SIDEREAL;
                const int srflag = SwissEph.SE_BIT_NO_REFRACTION | SwissEph.SE_BIT_DISC_CENTER; //disk is at center of horizon
                var options = SwissEph.SE_CALC_RISE | srflag; //set for sunrise
                var planet = SwissEph.SE_SUN;

                double[] geopos = new Double[3] { time.GetGeoLocation().Longitude(), time.GetGeoLocation().Latitude(), 0 };
                double riseTimeRaw = 0;

                var errorMsg = "";
                const double atpress = 0.0; //pressure
                const double attemp = 0.0;  //temperature

                //create a new time at 12 am on the same day, as calculator searches for sunrise after the inputed time
                var oriLmt = time.GetLmtDateTimeOffset();
                var lmtAt12Am = new DateTime(oriLmt.Year, oriLmt.Month, oriLmt.Day, 0, 0, 0);
                var timeAt12Am = new Time(lmtAt12Am, time.GetStdDateTimeOffset().Offset, time.GetGeoLocation());


                //get LMT at Greenwich in Julian days
                var julianLmtUtcTime = GreenwichLmtInJulianDays(timeAt12Am);

                //do calculation for sunrise time
                using SwissEph ephemeris = new();
                int ret = ephemeris.swe_rise_trans(julianLmtUtcTime, planet, "", iflag, options, geopos, atpress, attemp, ref riseTimeRaw, ref errorMsg);


                //2. Convert raw sun rise time (julian lmt utc) to normal time (std)

                //julian days back to normal time (greenwich)
                var sunriseLmtAtGreenwich = GreenwichTimeFromJulianDays(riseTimeRaw);

                //return sunrise time at orginal location to caller
                var stdOriginal = sunriseLmtAtGreenwich.ToOffset(time.GetStdDateTimeOffset().Offset);
                var sunriseTime = new Time(stdOriginal, time.GetGeoLocation());
                return sunriseTime;

            }

        }

        /// <summary>
        /// Get actual sunset time for that day at that place
        /// </summary>
        public static Time SunsetTime(Time time)
        {

            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey(nameof(SunsetTime), time, Ayanamsa), _getSunsetTime);


            //UNDERLYING FUNCTION
            Time _getSunsetTime()
            {
                //1. Calculate sunset time

                //prepare data to do calculation
                const int iflag = SwissEph.SEFLG_SWIEPH | SwissEph.SEFLG_SPEED | SwissEph.SEFLG_SIDEREAL;
                const int srflag = SwissEph.SE_BIT_NO_REFRACTION | SwissEph.SE_BIT_DISC_CENTER; //disk is at center of horizon
                var options = SwissEph.SE_CALC_SET | srflag; //set for sunset
                var planet = SwissEph.SE_SUN;

                double[] geopos = new Double[3] { time.GetGeoLocation().Longitude(), time.GetGeoLocation().Latitude(), 0 };
                double setTimeRaw = 0;

                var errorMsg = "";
                const double atpress = 0.0; //pressure
                const double attemp = 0.0;  //temperature


                //create a new time at 12 am on the same day, as calculator searches for sunrise after the inputed time
                var oriLmt = time.GetLmtDateTimeOffset();
                var lmtAt12Am = new DateTime(oriLmt.Year, oriLmt.Month, oriLmt.Day, 0, 0, 0);
                var timeAt12Am = new Time(lmtAt12Am, time.GetStdDateTimeOffset().Offset, time.GetGeoLocation());

                //get LMT at Greenwich in Julian days
                var julianLmtUtcTime = GreenwichLmtInJulianDays(timeAt12Am);

                //do calculation for sunset time
                using SwissEph ephemeris = new();
                int ret = ephemeris.swe_rise_trans(julianLmtUtcTime, planet, "", iflag, options, geopos, atpress, attemp, ref setTimeRaw, ref errorMsg);


                //2. Convert raw sun set time (julian lmt utc) to normal time (std)

                //julian days back to normal time (greenwich)
                var sunriseLmtAtGreenwich = GreenwichTimeFromJulianDays(setTimeRaw);

                //return sunset time at orginal location to caller
                var stdOriginal = sunriseLmtAtGreenwich.ToOffset(time.GetStdDateTimeOffset().Offset);
                var sunsetTime = new Time(stdOriginal, time.GetGeoLocation());
                return sunsetTime;

            }

        }

        /// <summary>
        /// Get actual noon time for that day at that place
        /// Returned in apparent time (DateTime)
        /// Note:
        /// This is marked when the centre of the Sun is exactly* on the
        /// meridian of the place. The apparent noon is
        /// almost the same for all places.
        /// *Center of disk is not actually used for now (future implementation)
        /// </summary>
        public static DateTime NoonTime(Time time)
        {
            //get apparent time
            var localApparentTime = LocalApparentTime(time);
            var apparentNoon = new DateTime(localApparentTime.Year, localApparentTime.Month, localApparentTime.Day, 12, 0, 0);

            return apparentNoon;
        }

        /// <summary>
        /// Checks if planet A is in good aspect to planet B
        ///
        /// Note:
        /// A is transmitter, B is receiver
        /// 
        /// An aspect is good or bad according to the relation
        /// between the aspecting and the aspected body
        /// </summary>
        public static bool IsPlanetInGoodAspectToPlanet(PlanetName receivingAspect, PlanetName transmitingAspect, Time time)
        {
            //check if transmitting planet is aspecting receiving planet
            var isAspecting = IsPlanetAspectedByPlanet(receivingAspect, transmitingAspect, time);

            //if not aspecting at all, end here as not occuring
            if (!isAspecting) { return false; }

            //check if it is a good aspect
            var aspectNature = PlanetCombinedRelationshipWithPlanet(receivingAspect, transmitingAspect, time);
            var isGood = aspectNature == PlanetToPlanetRelationship.BestFriend ||
                         aspectNature == PlanetToPlanetRelationship.Friend;

            //if is aspecting and it is good, then occuring (true)
            return isAspecting && isGood;

        }

        /// <summary>
        ///Checks if a planet is in good aspect to a house
        ///
        /// Note:
        /// An aspect is good or bad according to the relation
        /// between the planet and lord of the house sign
        /// </summary>
        public static bool IsPlanetInGoodAspectToHouse(HouseName receivingAspect, PlanetName transmitingAspect, Time time)
        {
            //check if transmiting planet is aspecting receiving planet
            var isAspecting = IsHouseAspectedByPlanet(receivingAspect, transmitingAspect, time);

            //if not aspecting at all, end here as not occuring
            if (!isAspecting) { return false; }

            //check if it is a good aspect
            var aspectNature = PlanetRelationshipWithHouse(receivingAspect, transmitingAspect, time);

            var isGood = aspectNature == PlanetToSignRelationship.OwnVarga || //Swavarga - own varga
                         aspectNature == PlanetToSignRelationship.FriendVarga || //Mitravarga - friendly varga
                         aspectNature == PlanetToSignRelationship.BestFriendVarga; //Adhi Mitravarga - Intimate friend varga


            //if is aspecting and it is good, then occuring (true)
            return isAspecting && isGood;

        }

        /// <summary>
        /// To determine if sthana bala is indicating good position or bad position
        /// a neutral point is set, anything above is good & below is bad
        ///
        /// Note:
        /// Neutral point is derived from all possible sthana bala values across
        /// 25 years (2000-2025), with 1 hour granularity
        ///
        /// Formula used = ((max-min)/2)+min
        /// max = hightest possible value
        /// min = lowest possible value
        /// </summary>
        public static double PlanetSthanaBalaNeutralPoint(PlanetName planet)
        {
            //no calculation for rahu and ketu here
            var isRahu = planet.Name == PlanetNameEnum.Rahu;
            var isKetu = planet.Name == PlanetNameEnum.Ketu;
            var isRahuKetu = isRahu || isKetu;
            if (isRahuKetu) { return 0; }


            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey(nameof(PlanetSthanaBalaNeutralPoint), planet, Ayanamsa), _getPlanetSthanaBalaNeutralPoint);


            double _getPlanetSthanaBalaNeutralPoint()
            {
                int max = 0, min = 0;

                if (planet == Saturn) { max = 297; min = 59; }
                if (planet == Mars) { max = 362; min = 60; }
                if (planet == Jupiter) { max = 296; min = 77; }
                if (planet == Mercury) { max = 295; min = 47; }
                if (planet == Venus) { max = 284; min = 60; }
                if (planet == Sun) { max = 327; min = 52; }
                if (planet == Moon) { max = 311; min = 54; }

                //calculate neutral point
                var neutralPoint = ((max - min) / 2) + min;

                if (neutralPoint <= 0) { throw new Exception("Planet does not have neutral point!"); }

                return neutralPoint;
            }
        }

        /// <summary>
        /// Checks if a planet is in a Trikona house (trines)(1,5,9)
        /// Equals to "Is Jupiter in Trine from Lagna"
        /// </summary>
        public static bool IsPlanetInTrikona(PlanetName planet, Time time)
        {
            //get current planet house
            var planetHouse = HousePlanetOccupies(planet, time);

            //check if planet is in Trine
            var isPlanetInTrine = planetHouse == HouseName.House1 ||
                                  planetHouse == HouseName.House5 ||
                                  planetHouse == HouseName.House9;

            return isPlanetInTrine;
        }

        /// <summary>
        /// Checks if a planet is in a kendra house (1,4,7,10)
        /// Equals to "Is Jupiter in Kendra from Lagna"
        /// </summary>
        public static bool IsPlanetInKendra(PlanetName planet, Time time)
        {
            //The 4th, the 7th and the 10th are the Kendras
            var planetHouse = HousePlanetOccupies(planet, time);

            //check if planet is in kendra
            var isPlanetInKendra = planetHouse == HouseName.House1 || planetHouse == HouseName.House4 || planetHouse == HouseName.House7 || planetHouse == HouseName.House10;

            return isPlanetInKendra;
        }

        /// <summary>
        /// Checks if a planet is in a Upachayas (3rd, 6th, 10th, and 11th)
        /// </summary>
        public static bool IsPlanetInUpachaya(PlanetName planet, Time time)
        {
            //get current house
            var planetHouse = HousePlanetOccupies(planet, time);

            //check if planet is in 3rd, 6th, 10th, or 11th
            var isPlanetInUpachayas = planetHouse == HouseName.House3 ||
                                      planetHouse == HouseName.House6 ||
                                      planetHouse == HouseName.House10 ||
                                      planetHouse == HouseName.House11;

            return isPlanetInUpachayas;
        }

        /// <summary>
        /// Checks if any 1 given planet is in a kendra house (1,4,7,10)
        /// Equals to "Is Jupiter or Venus in Kendra from Lagna"
        /// </summary>
        public static bool IsPlanetInKendra(PlanetName[] planetList, Time time)
        {
            //default to false
            var isFound = false;

            //if any planet is found, stop loop and return true
            foreach (var planet in planetList)
            {
                var isKendra = IsPlanetInKendra(planet, time);
                if (isKendra) { return true; }
            }

            return isFound;
        }

        /// <summary>
        /// Checks if a planet is in a kendra house (1,4,7,10) from another planet. Exp : Is Jupiter is in a kendra from the Moon
        /// </summary>
        public static bool IsPlanetInKendraFromPlanet(PlanetName kendraFrom, PlanetName kendraTo, Time time)
        {
            //get the number of signs between planets
            var count = SignDistanceFromPlanetToPlanet(kendraTo, kendraFrom, time);

            //check if number is a kendra number
            var isKendra = count == 1 ||
                           count == 4 ||
                           count == 7 ||
                           count == 10;

            return isKendra;
        }

        /// <summary>
        /// Counts number of sign between 2 planets.
        /// </summary>
        public static int SignDistanceFromPlanetToPlanet(PlanetName startPlanet, PlanetName endPlanet, Time time)
        {
            //get position of "kendra to" planet
            var startSign = PlanetZodiacSign(startPlanet, time);

            //get position of "kendra from" planet
            var endSign = PlanetZodiacSign(endPlanet, time);

            //count distance between signs
            var count = CountFromSignToSign(startSign.GetSignName(), endSign.GetSignName());

            return count;
        }

        /// <summary>
        /// Checks if the lord of a house is in the specified house.
        /// Example question : Is Lord of 1st house in 2nd house?
        /// </summary>
        public static bool IsHouseLordInHouse(HouseName lordHouse, HouseName occupiedHouse, Time time)
        {
            //get the house lord
            var houseLord = LordOfHouse(lordHouse, time);

            //get house the lord is in
            var houseIsIn = HousePlanetOccupies(houseLord, time);

            //if it matches then occuring
            return houseIsIn == occupiedHouse;
        }

        /// <summary>
        /// Checks if a planet is conjuct with an evil/malefic planet
        /// </summary>
        public static bool IsPlanetConjunctWithMaleficPlanets(PlanetName planetName, Time time)
        {
            //get all the planets conjuct with inputed planet
            var planetsInConjunct = PlanetsInConjuction(planetName, time);

            //get all evil planets
            var evilPlanets = MaleficPlanetList(time);

            //check if any conjunct planet is an evil one
            var evilFound = planetsInConjunct.FindAll(planet => evilPlanets.Contains(planet)).Any();
            return evilFound;

        }

        /// <summary>
        /// Checks if a planet is conjunct with an enemy planet by combined relationship
        /// </summary>
        public static bool IsPlanetConjunctWithEnemyPlanets(PlanetName inputPlanet, Time time)
        {
            //get all the planets conjunct with inputed planet
            var planetsInConjunct = PlanetsInConjuction(inputPlanet, time);

            //check if any conjunct planet is an enemy
            foreach (var planet in planetsInConjunct)
            {
                //get relationship of the 2 planets
                var aspectNature = PlanetCombinedRelationshipWithPlanet(inputPlanet, planet, time);
                var isEnemy = aspectNature == PlanetToPlanetRelationship.Enemy ||
                              aspectNature == PlanetToPlanetRelationship.BitterEnemy;

                //if enemy than end here as true
                if (isEnemy) { return true; }

            }

            //if control reaches here than no enemy planet found
            return false;

        }

        /// <summary>
        /// Checks if a planet is conjunct with an Friend planet by combined relationship
        /// </summary>
        public static bool IsPlanetConjunctWithFriendPlanets(PlanetName inputPlanet, Time time)
        {
            //get all the planets conjunct with inputed planet
            var planetsInConjunct = PlanetsInConjuction(inputPlanet, time);

            //check if any conjunct planet is an Friend
            foreach (var planet in planetsInConjunct)
            {
                //get relationship of the 2 planets
                var conjunctNature = PlanetCombinedRelationshipWithPlanet(inputPlanet, planet, time);
                var isFriend = conjunctNature == PlanetToPlanetRelationship.Friend ||
                               conjunctNature == PlanetToPlanetRelationship.BestFriend;

                //if enemy than end here as true
                if (isFriend) { return true; }

            }

            //if control reaches here than no enemy planet found
            return false;

        }

        /// <summary>
        /// Checks if any evil/malefic planets are in a house
        /// Note : Planet to house relationship not account for
        /// TODO Account for planet to sign relationship, find reference
        /// </summary>
        public static bool IsMaleficPlanetInHouse(HouseName houseNumber, Time time)
        {
            //get all the planets in the house
            var planetsInHouse = PlanetsInHouse(houseNumber, time);

            //get all evil planets
            var evilPlanets = MaleficPlanetList(time);

            //check if any planet in house is an evil one
            var evilFound = planetsInHouse.FindAll(planet => evilPlanets.Contains(planet)).Any();

            return evilFound;

        }

        /// <summary>
        /// Checks if any good/benefic planets are in a house
        /// Note : Planet to house relationship not account for
        /// TODO Account for planet to sign relationship, find reference
        /// </summary>
        public static bool IsBeneficPlanetInHouse(HouseName houseNumber, Time time)
        {
            //get all the planets in the house
            var planetsInHouse = PlanetsInHouse(houseNumber, time);

            //get all good planets
            var goodPlanets = BeneficPlanetList(time);

            //check if any planet in house is an good one
            var goodFound = planetsInHouse.FindAll(planet => goodPlanets.Contains(planet)).Any();

            return goodFound;

        }

        /// <summary>
        /// Checks if any good/benefic planets are in kendra houses house
        /// </summary>
        public static bool IsBeneficsInKendra(Time time)
        {
            //get all good planets
            var goodPlanets = BeneficPlanetList(time);

            //check if any one of the good planets is in a kendra
            foreach (var planet in goodPlanets)
            {
                var isInKendra = IsPlanetInKendra(planet, time);

                //if planet found in kendra, end here as true (found)
                if (isInKendra) { return true; }
            }

            //if control reaches here than no benefic in kendra found, return false
            return false;

        }

        /// <summary>
        /// Checks if all malefics are in places in Upachayas.
        /// Malefic planets are those that are generally considered to bring challenges or difficulties.
        /// The Upachayas are the 3rd, 6th, 10th, and 11th houses.
        /// These houses are known as the houses of growth and expansion.
        /// When malefic planets are in these houses, they can drive ambition and personal growth.
        /// </summary>
        public static bool IsAllMaleficsInUpachayas(Time time)
        {
            //get all bad planets
            var badPlanets = MaleficPlanetList(time);

            //all planets must be in
            foreach (var planet in badPlanets)
            {
                var isInUpachaya = IsPlanetInUpachaya(planet, time);

                //if not in, then end as not occuring
                if (!isInUpachaya) { return false; }
            }

            //if control reaches true
            return true;

        }


        /// <summary>
        /// Checks if any evil/malefic planets are in a sign
        /// </summary>
        public static bool IsMaleficPlanetInSign(ZodiacName sign, Time time)
        {
            //get all the planets in the sign
            var planetsInSign = PlanetInSign(sign, time);

            //get all evil planets
            var evilPlanets = MaleficPlanetList(time);

            //check if any planet in sign is an evil one
            var evilFound = planetsInSign.FindAll(planet => evilPlanets.Contains(planet)).Any();

            return evilFound;
        }

        /// <summary>
        /// Gets list of evil/malefic planets in a sign
        /// </summary>
        public static List<PlanetName> MaleficPlanetListInSign(ZodiacName sign, Time time)
        {
            //get all the planets in the sign
            var planetsInSign = PlanetInSign(sign, time);

            //get all evil planets
            var evilPlanets = MaleficPlanetList(time);

            //get evil planets in sign
            var evilFound = planetsInSign.FindAll(planet => evilPlanets.Contains(planet));

            return evilFound;
        }

        /// <summary>
        /// Checks if any good/benefic planets are in a sign
        /// </summary>
        public static bool IsBeneficPlanetInSign(ZodiacName sign, Time time)
        {
            //get all the planets in the sign
            var planetsInSign = PlanetInSign(sign, time);

            //get all good planets
            var goodPlanets = BeneficPlanetList(time);

            //check if any planet in sign is an good one
            var goodFound = planetsInSign.FindAll(planet => goodPlanets.Contains(planet)).Any();

            return goodFound;
        }

        /// <summary>
        /// Gets any good/benefic planets in a sign
        /// </summary>
        public static List<PlanetName> BeneficPlanetListInSign(ZodiacName sign, Time time)
        {
            //get all the planets in the sign
            var planetsInSign = PlanetInSign(sign, time);

            //get all good planets
            var goodPlanets = BeneficPlanetList(time);

            //gets all good planets in this sign
            var goodFound = planetsInSign.FindAll(planet => goodPlanets.Contains(planet));

            return goodFound;
        }

        /// <summary>
        /// Checks if any evil/malefic planet is transmitting aspect to a house
        /// Note: This does NOT account for bad aspects, where relationship with house lord is checked
        /// TODO relationship aspect should be added get reference for it firsts
        /// </summary>
        public static bool IsMaleficPlanetAspectHouse(HouseName house, Time time)
        {
            //get all evil planets
            var evilPlanets = MaleficPlanetList(time);

            //check if any evil planet is aspecting the inputed house
            var evilFound = evilPlanets.FindAll(evilPlanet => IsHouseAspectedByPlanet(house, evilPlanet, time)).Any();

            return evilFound;

        }

        /// <summary>
        /// Checks if any good/benefic planet is transmitting aspect to a house
        /// Note: This does NOT account for good aspects, where relationship with house lord is checked
        /// TODO relationship aspect should be added get reference for it firsts
        /// </summary>
        public static bool IsBeneficPlanetAspectHouse(HouseName house, Time time)
        {
            //get all good planets
            var goodPlanets = BeneficPlanetList(time);

            //check if any good planet is aspecting the inputed house
            var goodFound = goodPlanets.FindAll(goodPlanet => IsHouseAspectedByPlanet(house, goodPlanet, time)).Any();

            return goodFound;

        }

        /// <summary>
        /// Checks if a planet is receiving aspects from an evil planet
        /// </summary>
        public static bool IsPlanetAspectedByMaleficPlanets(PlanetName lord, Time time)
        {
            //get list of evil planets
            var evilPlanets = MaleficPlanetList(time);

            //check if any of the evil planets is aspecting inputed planet
            var evilAspectFound = evilPlanets.FindAll(evilPlanet =>
                IsPlanetAspectedByPlanet(lord, evilPlanet, time)).Any();
            return evilAspectFound;

        }

        /// <summary>
        /// Checks if a planet is receiving aspects from an benefic planet
        /// </summary>
        public static bool IsPlanetAspectedByBeneficPlanets(PlanetName lord, Time time)
        {
            //get list of benefic planets
            var goodPlanets = BeneficPlanetList(time);

            //check if any of the benefic planets is aspecting inputed planet
            var goodAspectFound = goodPlanets.FindAll(goodPlanet =>
                IsPlanetAspectedByPlanet(lord, goodPlanet, time)).Any();

            return goodAspectFound;

        }

        /// <summary>
        /// Checks if a planet is receiving aspects from an enemy planet based on combined relationship
        /// </summary>
        public static bool IsPlanetAspectedByEnemyPlanets(PlanetName inputPlanet, Time time)
        {
            //get all the planets aspecting inputed planet
            var planetsAspecting = PlanetsAspectingPlanet(inputPlanet, time);

            //check if any aspecting planet is an enemy
            foreach (var planet in planetsAspecting)
            {
                //get relationship of the 2 planets
                var aspectNature = PlanetCombinedRelationshipWithPlanet(inputPlanet, planet, time);
                var isEnemy = aspectNature == PlanetToPlanetRelationship.Enemy ||
                              aspectNature == PlanetToPlanetRelationship.BitterEnemy;

                //if enemy than end here as true
                if (isEnemy) { return true; }

            }

            //if control reaches here than no enemy planet found
            return false;


        }

        /// <summary>
        /// Checks if a planet is receiving aspects from a Friend planet based on combined relationship
        /// </summary>
        public static bool IsPlanetAspectedByFriendPlanets(PlanetName inputPlanet, Time time)
        {
            //get all the planets aspecting inputed planet
            var planetsAspecting = PlanetsAspectingPlanet(inputPlanet, time);

            //check if any aspecting planet is an Friend
            foreach (var planet in planetsAspecting)
            {
                //get relationship of the 2 planets
                var aspectNature = PlanetCombinedRelationshipWithPlanet(inputPlanet, planet, time);
                var isFriend = aspectNature == PlanetToPlanetRelationship.Friend ||
                               aspectNature == PlanetToPlanetRelationship.BestFriend;

                //if Friend than end here as true
                if (isFriend) { return true; }

            }

            //if control reaches here than no Friend planet found
            return false;


        }

        /// <summary>
        /// Gets the Arudha Lagna Sign 
        /// 
        /// Reference Note:
        /// Arudha Lagna and planetary dispositions in reference to it have a strong bearing on the
        /// financial status of the person. In my own humble experience, Arudha Lagna should be given
        /// as much importance as the usual Janma Lagna. Arudha Lagna is the sign arrived at by counting
        /// as many signs from lord of Lagna as lord of Lagna is removed from Lagna.
        /// Thus if Aquarius is ascendant and its lord Saturn is in the 4th (Taurus)
        /// then the 4th from Taurus, viz., Leo becomes Arudha Lagna.
        /// </summary>
        public static ZodiacName ArudhaLagnaSign(Time time)
        {
            //get janma lagna
            var janmaLagna = HouseSignName(HouseName.House1, time);

            //get sign lord of janma lagna is in
            var lagnaLord = LordOfHouse(HouseName.House1, time);
            var lagnaLordSign = PlanetZodiacSign(lagnaLord, time).GetSignName();

            //count the signs from janma to the sign the lord is in
            var janmaToLordCount = CountFromSignToSign(janmaLagna, lagnaLordSign);

            //use the above count to find arudha sign from lord's sign
            var arudhaSign = SignCountedFromInputSign(lagnaLordSign, janmaToLordCount);

            return arudhaSign;
        }

        /// <summary>
        /// Counts from start sign to end sign
        /// Example : Aquarius to Taurus is 4
        /// </summary>
        public static int CountFromSignToSign(ZodiacName startSign, ZodiacName endSign)
        {
            int count = 0;

            //get zodiac name & convert to its number equivalent
            var startSignNumber = (int)startSign;
            var endSignNumber = (int)endSign;

            //if start sign is more than end sign (meaning lower in the list)
            if (startSignNumber > endSignNumber)
            {
                //minus with 12, as though counting to the end
                int countToLastZodiac = (12 - startSignNumber) + 1; //plus 1 to count it self

                count = endSignNumber + countToLastZodiac;
            }
            else if (startSignNumber == endSignNumber)
            {
                count = 1;
            }
            //if start sign is lesser than end sign (meaning higher in the list)
            //we can minus like normal, and just add 1 to count it self
            else if (startSignNumber < endSignNumber)
            {
                count = (endSignNumber - startSignNumber) + 1; //plus 1 to count it self
            }

            return count;
        }

        /// <summary>
        /// Counts from start Constellation to end Constellation
        /// Example : Aquarius to Taurus is 4
        /// </summary>
        public static int CountFromConstellationToConstellation(Constellation start, Constellation end)
        {

            //get the number equivalent of the constellation
            int endConstellationNumber = end.GetConstellationNumber();

            int startConstellationNumber = start.GetConstellationNumber();

            int counter = 0;


            //Need to count from birthRulingConstellationNumber to dayRulingConstellationNumber

            //if start is more than end (meaning lower in the list)
            if (startConstellationNumber > endConstellationNumber)
            {
                //count from start to last constellation (27)
                int countToLastConstellation = (27 - startConstellationNumber) + 1; //plus 1 to count it self

                //to previous count add end constellation number
                counter = endConstellationNumber + countToLastConstellation;
            }
            else if (startConstellationNumber == endConstellationNumber)
            {
                counter = 1;
            }
            else if (startConstellationNumber < endConstellationNumber)
            {
                //if start sign is lesser than end sign (meaning higher in the list)
                //we can minus like normal, and just add 1 to count it self
                counter = (endConstellationNumber - startConstellationNumber) + 1; //plus 1 to count it self
            }


            return counter;
        }

        /// <summary>
        /// Checks if a planet is in a given house at a specified time 
        /// </summary>
        /// <param name="houseNumber">house number to check</param>
        public static bool IsPlanetInHouse(PlanetName planet, HouseName houseNumber, Time time)
        {
            return HousePlanetOccupies(planet, time) == houseNumber;
        }

        /// <summary>
        /// Checks if a planet is in a given house at a specified time, using KP method
        /// </summary>
        /// <param name="cusps">can be both Horary & Kundali house cusps (point between houses)</param>
        /// <param name="planetNirayanaDegrees"></param>
        /// <param name="house"></param>
        /// <returns></returns>
        public static bool IsPlanetInHouseKP(Dictionary<HouseName, Angle> cusps, Angle planetNirayanaDegrees, HouseName house)
        {
            //get house number
            var houseNumber = (int)house;
            // Check if houseNumber is within the bounds of the array
            if (houseNumber >= 0 && houseNumber < cusps.Count)
            {
                //check if cusp longitude is smaller than next cusp longitude
                if (houseNumber + 1 < cusps.Count && cusps[(HouseName)houseNumber + 1] > cusps[(HouseName)houseNumber])
                {
                    return (planetNirayanaDegrees.TotalDegrees >= cusps[(HouseName)houseNumber].TotalDegrees) &&
                           //this means that the planet falls in between these house cusps
                           (planetNirayanaDegrees.TotalDegrees <= cusps[(HouseName)houseNumber + 1].TotalDegrees);
                }
                //if next cusp start long is smaller than current cusp we are rotating through 360 deg
                else if (houseNumber + 1 < cusps.Count)
                {
                    return (planetNirayanaDegrees.TotalDegrees >= cusps[(HouseName)houseNumber].TotalDegrees) ||
                           (planetNirayanaDegrees.TotalDegrees <= cusps[(HouseName)houseNumber + 1].TotalDegrees);
                }
                // If houseNumber is the last index in the cusps array
                else
                {
                    return planetNirayanaDegrees.TotalDegrees >= cusps[(HouseName)houseNumber].TotalDegrees;
                }
            }
            return false;
        }

        /// <summary>
        /// Checks if a planet is in a given house at a specified time 
        /// </summary>
        public static bool IsAllPlanetInHouse(List<PlanetName> planetList, HouseName houseNumber, Time time)
        {
            //calculate each planet, even if 1 planet is out, then return as false
            foreach (var planetName in planetList)
            {
                var tempVal = IsPlanetInHouse(planetName, houseNumber, time);
                if (tempVal == false) { return false; }
            }

            //if control reaches here than all planets in house
            return true;
        }

        /// <summary>
        /// Checks if any planet in list is at a given house at a specified time 
        /// </summary>
        public static bool IsAnyPlanetInHouse(List<PlanetName> planetList, HouseName houseNumber, Time time)
        {
            //calculate each planet, even if 1 planet is out, then return as false
            foreach (var planetName in planetList)
            {
                var tempVal = IsPlanetInHouse(planetName, houseNumber, time);
                if (tempVal == true) { return true; }
            }

            // if control reaches here then no planet is in house
            return false;
        }

        /// <summary>
        /// Checks if a planet is in a longitude where it's in Debilitated
        /// Note : Rahu & ketu accounted for
        /// </summary>
        public static bool IsPlanetDebilitated(PlanetName planet, Time time)
        {
            //get planet location
            var planetLongitude = PlanetNirayanaLongitude(planet, time);

            //convert planet longitude to zodiac sign
            var planetZodiac = ZodiacSignAtLongitude(planetLongitude);

            //get the longitude where planet is Debilited
            var point = PlanetDebilitationPoint(planet);

            //check if planet is in Debilitation sign
            var sameSign = planetZodiac.GetSignName() == point.GetSignName();

            //check only degree ignore minutes & seconds
            var sameDegree = planetZodiac.GetDegreesInSign().Degrees == point.GetDegreesInSign().Degrees;
            var planetIsDebilitated = sameSign && sameDegree;

            return planetIsDebilitated;
        }

        /// <summary>
        /// Checks if a planet is in a longitude where it's in Exaltation
        ///
        /// NOTE:
        /// -   Rahu & ketu accounted for
        /// 
        /// -   Exaltation
        ///     Each planet is held to be exalted when it is
        ///     in a particular sign. The power to do good when in
        ///     exaltation is greater than when in its own sign.
        ///     Throughout the sign ascribed,
        ///     the planet is exalted but in a particular degree
        ///     its exaltation is at the maximum level.
        /// </summary>
        public static bool IsPlanetExalted(PlanetName planet, Time time)
        {
            //get planet location
            var planetLongitude = PlanetNirayanaLongitude(planet, time);

            //convert planet longitude to zodiac sign
            var planetZodiac = ZodiacSignAtLongitude(planetLongitude);

            //get the longitude where planet is Exaltation
            var point = PlanetExaltationPoint(planet);

            //check if planet is in Exaltation sign
            var sameSign = planetZodiac.GetSignName() == point.GetSignName();

            //check only degree ignore minutes & seconds
            var sameDegree = planetZodiac.GetDegreesInSign().Degrees == point.GetDegreesInSign().Degrees;
            var planetIsExaltation = sameSign && sameDegree;

            return planetIsExaltation;
        }

        /// <summary>
        /// Checks if the moon is FULL, moon day 15 at given time
        /// </summary>
        public static bool IsFullMoon(Time time)
        {
            //get the lunar date number
            int lunarDayNumber = LunarDay(time).GetLunarDayNumber();

            //if day 15, it is full moon
            return lunarDayNumber == 15;
        }

        /// <summary>
        /// Checks if the moon is New, moon day 1 at given time
        /// </summary>
        public static bool IsNewMoon(Time time)
        {
            //get the lunar date number
            int lunarDayNumber = LunarDay(time).GetLunarDayNumber();

            //if day 1, it is new moon
            return lunarDayNumber == 1 || lunarDayNumber == 0;
        }

        /// <summary>
        /// Check if it is a Water / Aquatic sign
        /// Water Signs: this category include Cancer, Scorpio and Pisces.
        /// </summary>
        public static bool IsWaterSign(ZodiacName moonSign) => moonSign is ZodiacName.Cancer or ZodiacName.Scorpio or ZodiacName.Pisces;

        /// <summary>
        /// Check if it is a Fire sign
        /// Fire Signs: this sign encloses Aries, Leo and Sagittarius.
        /// </summary>
        public static bool IsFireSign(ZodiacName moonSign) => moonSign is ZodiacName.Aries or ZodiacName.Leo or ZodiacName.Sagittarius;

        /// <summary>
        /// Check if it is a Earth sign
        /// Earth Signs: it contains Taurus, Virgo and Capricorn.
        /// </summary>
        public static bool IsEarthSign(ZodiacName moonSign) => moonSign is ZodiacName.Taurus or ZodiacName.Virgo or ZodiacName.Capricorn;

        /// <summary>
        /// Check if it is a Air / Windy sign
        /// Air Signs: this sign include Gemini, Libra and Aquarius.
        /// </summary>
        public static bool IsAirSign(ZodiacName moonSign) => moonSign is ZodiacName.Gemini or ZodiacName.Libra or ZodiacName.Aquarius;

        /// <summary>
        /// WARNING! MARKED FOR DELETION : ERONEOUS RESULTS NOT SUITED FOR INTENDED PURPOSE
        /// METHOD NOT VERIFIED
        /// This methods perpose is to define the final good or bad
        /// nature of planet in antaram.
        ///
        /// For now only data from chapter "Key-planets for Each Sign"
        /// If this proves to be inacurate, add more checks in this method.
        /// - bindu points
        /// 
        /// Similar to method GetDasaInfoForAscendant
        /// Data from pg 80 of Key-planets for Each Sign in Hindu Predictive Astrology
        /// TODO meant to determine nature of antram
        /// </summary>
        public static EventNature PlanetAntaramNature(Person person, PlanetName planet)
        {
            //todo account for rahu & ketu
            //rahu & ketu not sure for now, just return neutral
            if (planet == Rahu || planet == Ketu) { return EventNature.Neutral; }

            //get nature from person's lagna
            var planetNature = GetNatureFromLagna();

            //if nature is neutral then use nature of relation to current house
            //assumed that bad relation to sign is bad planet (todo upgrade to bindu points)
            //note: generaly speaking a neutral planet shloud not exist, either good or bad
            if (planetNature == EventNature.Neutral)
            {
                var _planetCurrentHouse = HousePlanetOccupies(planet, person.BirthTime);

                var _currentHouseRelation = PlanetRelationshipWithHouse(_planetCurrentHouse, planet, person.BirthTime);

                switch (_currentHouseRelation)
                {
                    case PlanetToSignRelationship.BestFriendVarga:
                    case PlanetToSignRelationship.FriendVarga:
                    case PlanetToSignRelationship.OwnVarga:
                    case PlanetToSignRelationship.Moolatrikona:
                        return EventNature.Good;
                    case PlanetToSignRelationship.NeutralVarga:
                        return EventNature.Neutral;
                    case PlanetToSignRelationship.EnemyVarga:
                    case PlanetToSignRelationship.BitterEnemyVarga:
                        return EventNature.Bad;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            //else return nature from lagna
            return planetNature;


            //LOCAL FUNCTIONS

            EventNature GetNatureFromLagna()
            {
                var personLagna = HouseSignName(HouseName.House1, person.BirthTime);

                //get list of good and bad planets for a lagna
                dynamic planetData = GetPlanetData(personLagna);
                List<PlanetName> goodPlanets = planetData.Good;
                List<PlanetName> badPlanets = planetData.Bad;

                //check good planets first
                if (goodPlanets.Contains(planet))
                {
                    return EventNature.Good;
                }

                //check bad planets next
                if (badPlanets.Contains(planet))
                {
                    return EventNature.Bad;
                }

                //if control reaches here, then planet not
                //listed as good or bad, so just say neutral
                return EventNature.Neutral;
            }

            // data from chapter "Key-planets for Each Sign"
            object GetPlanetData(ZodiacName lagna)
            {
                List<PlanetName> good = null;
                List<PlanetName> bad = null;

                switch (lagna)
                {
                    //Aries - Saturn, Mercury and Venus are ill-disposed.
                    // Jupiter and the Sun are auspicious. The mere combination
                    // of Jupiler and Saturn produces no beneficial results. Jupiter
                    // is the Yogakaraka or the planet producing success. If Venus
                    // becomes a maraka, he will not kill the native but planets like
                    // Saturn will bring about death to the person.
                    case ZodiacName.Aries:
                        good = new List<PlanetName>() { Jupiter, Sun };
                        bad = new List<PlanetName>() { Saturn, Mercury, Venus };
                        break;
                    //Taurus - Saturn is the most auspicious and powerful
                    // planet. Jupiter, Venus and the Moon are evil planets. Saturn
                    // alone produces Rajayoga. The native will be killed in the
                    // periods and sub-periods of Jupiter, Venus and the Moon if
                    // they get death-inflicting powers.
                    case ZodiacName.Taurus:
                        good = new List<PlanetName>() { Saturn };
                        bad = new List<PlanetName>() { Jupiter, Venus, Moon };
                        break;
                    //Gemini - Mars, Jupiter and the Sun are evil. Venus alone
                    // is most beneficial and in conjunction with Saturn in good signs
                    // produces and excellent career of much fame. Combination
                    // of Saturn and Jupiter produces similar results as in Aries.
                    // Venus and Mercury, when well associated, cause Rajayoga.
                    // The Moon will not kill the person even though possessed of
                    // death-inflicting powers.
                    case ZodiacName.Gemini:
                        good = new List<PlanetName>() { Venus };
                        bad = new List<PlanetName>() { Mars, Jupiter, Sun };
                        break;
                    //Cancer - Venus and Mercury are evil. Jupiter and Mars
                    // give beneficial results. Mars is the Rajayogakaraka
                    // (conferor of name and fame). The combination of Mars and Jupiter
                    // also causes Rajayoga (combination for political success). The
                    // Sun does not kill the person although possessed of maraka
                    // powers. Venus and other inauspicious planets kill the native.
                    // Mars in combination with the Moon or Jupiter in favourable
                    // houses especially the 1st, the 5th, the 9th and the 10th
                    // produces much reputation.
                    case ZodiacName.Cancer:
                        good = new List<PlanetName>() { Jupiter, Mars };
                        bad = new List<PlanetName>() { Venus, Mercury };
                        break;
                    //Leo - Mars is the most auspicious and favourable planet.
                    // The combination of Venus and Jupiter does not cause Rajayoga
                    // but the conjunction of Jupiter and Mars in favourable
                    // houses produce Rajayoga. Saturn, Venus and Mercury are
                    // evil. Saturn does not kill the native when he has the maraka
                    // power but Mercury and other evil planets inflict death when
                    // they get maraka powers.
                    case ZodiacName.Leo:
                        good = new List<PlanetName>() { Mars };
                        bad = new List<PlanetName>() { Saturn, Venus, Mercury };
                        break;
                    //Virgo - Venus alone is the most powerful. Mercury and
                    // Venus when combined together cause Rajayoga. Mars and
                    // the Moon are evil. The Sun does not kill the native even if
                    // be becomes a maraka but Venus, the Moon and Jupiter will
                    // inflict death when they are possessed of death-infticting power.
                    case ZodiacName.Virgo:
                        good = new List<PlanetName>() { Venus };
                        bad = new List<PlanetName>() { Mars, Moon };
                        break;
                    // Libra - Saturn alone causes Rajayoga. Jupiter, the Sun
                    // and Mars are inauspicious. Mercury and Saturn produce good.
                    // The conjunction of the Moon and Mercury produces Rajayoga.
                    // Mars himself will not kill the person. Jupiter, Venus
                    // and Mars when possessed of maraka powers certainly kill the
                    // nalive.
                    case ZodiacName.Libra:
                        good = new List<PlanetName>() { Saturn, Mercury };
                        bad = new List<PlanetName>() { Jupiter, Sun, Mars };
                        break;
                    //Scorpio - Jupiter is beneficial. The Sun and the Moon
                    // produce Rajayoga. Mercury and Venus are evil. Jupiter,
                    // even if be becomes a maraka, does not inflict death. Mercury
                    // and other evil planets, when they get death-inlflicting powers,
                    // do not certainly spare the native.
                    case ZodiacName.Scorpio:
                        good = new List<PlanetName>() { Jupiter };
                        bad = new List<PlanetName>() { Mercury, Venus };
                        break;
                    //Sagittarius - Mars is the best planet and in conjunction
                    // with Jupiter, produces much good. The Sun and Mars also
                    // produce good. Venus is evil. When the Sun and Mars
                    // combine together they produce Rajayoga. Saturn does not
                    // bring about death even when he is a maraka. But Venus
                    // causes death when be gets jurisdiction as a maraka planet.
                    case ZodiacName.Sagittarius:
                        good = new List<PlanetName>() { Mars };
                        bad = new List<PlanetName>() { Venus };
                        break;
                    //Capricorn - Venus is the most powerful planet and in
                    // conjunction with Mercury produces Rajayoga. Mars, Jupiter
                    // and the Moon are evil.
                    case ZodiacName.Capricorn:
                        good = new List<PlanetName>() { Venus };
                        bad = new List<PlanetName>() { Mars, Jupiter, Moon };
                        break;
                    //Aquarius - Venus alone is auspicious. The combination of
                    // Venus and Mars causes Rajayoga. Jupiter and the Moon are
                    // evil.
                    case ZodiacName.Aquarius:
                        good = new List<PlanetName>() { Venus };
                        bad = new List<PlanetName>() { Jupiter, Moon };
                        break;
                    //Pisces - The Moon and Mars are auspicious. Mars is
                    // most powerful. Mars with the Moon or Jupiter causes Rajayoga.
                    // Saturn, Venus, the Sun and Mercury are evil. Mars
                    // himself does not kill the person even if he is a maraka.
                    case ZodiacName.Pisces:
                        good = new List<PlanetName>() { Moon, Mars };
                        bad = new List<PlanetName>() { Saturn, Venus, Sun, Mercury };
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }


                return new { Good = good, Bad = bad };

            }
        }

        /// <summary>
        /// Soumyas
        /// Source : Astrology for beginners pg 30
        /// </summary>
        public static bool IsPlanetBeneficToLagna(PlanetName planetName, ZodiacName lagna)
        {
            switch (lagna)
            {
                case ZodiacName.Aries:
                    return planetName == Sun || planetName == Mars || planetName == Jupiter;
                case ZodiacName.Taurus:
                    return planetName == Sun || planetName == Mars
                                             || planetName == Mercury || planetName == Saturn;
                case ZodiacName.Gemini:
                    return planetName == Venus || planetName == Saturn;
                case ZodiacName.Cancer:
                    return planetName == Mars || planetName == Jupiter;
                case ZodiacName.Leo:
                    return planetName == Sun || planetName == Mars;
                case ZodiacName.Virgo:
                    return planetName == Venus;
                case ZodiacName.Libra:
                    return planetName == Mercury || planetName == Venus || planetName == Saturn;
                case ZodiacName.Scorpio:
                    return planetName == Jupiter || planetName == Sun || planetName == Moon;
                case ZodiacName.Sagittarius:
                    return planetName == Sun || planetName == Mars;
                case ZodiacName.Capricorn:
                    return planetName == Mercury || planetName == Venus || planetName == Saturn;
                case ZodiacName.Aquarius:
                    return planetName == Venus || planetName == Mars
                                               || planetName == Sun || planetName == Saturn;
                case ZodiacName.Pisces:
                    return planetName == Mars || planetName == Moon;
            }

            //control should not come here
            throw new InvalidOperationException();
        }

        /// <summary>
        /// Kruras (Malefics)
        /// Source : Astrology for beginners pg 30
        /// </summary>
        public static bool IsPlanetMaleficToLagna(PlanetName planetName, ZodiacName lagna)
        {
            switch (lagna)
            {
                case ZodiacName.Aries:
                    return planetName == Venus || planetName == Mercury || planetName == Saturn;
                case ZodiacName.Taurus:
                    return planetName == Moon || planetName == Jupiter || planetName == Venus;
                case ZodiacName.Gemini:
                    return planetName == Sun || planetName == Mars || planetName == Jupiter;
                case ZodiacName.Cancer:
                    return planetName == Mercury || planetName == Venus || planetName == Saturn;
                case ZodiacName.Leo:
                    return planetName == Mercury || planetName == Venus || planetName == Saturn;
                case ZodiacName.Virgo:
                    return planetName == Mars || planetName == Moon || planetName == Jupiter;
                case ZodiacName.Libra:
                    return planetName == Sun || planetName == Moon || planetName == Jupiter;
                case ZodiacName.Scorpio:
                    return planetName == Mercury || planetName == Saturn;
                case ZodiacName.Sagittarius:
                    return planetName == Saturn || planetName == Venus || planetName == Mercury;
                case ZodiacName.Capricorn:
                    return planetName == Moon || planetName == Mars || planetName == Jupiter;
                case ZodiacName.Aquarius:
                    return planetName == Jupiter || planetName == Moon;
                case ZodiacName.Pisces:
                    return planetName == Sun || planetName == Mercury
                                             || planetName == Venus || planetName == Saturn;
            }

            //control should not come here
            throw new InvalidOperationException();
        }

        /// <summary>
        /// Yogakaraka (Planets indicating prosperity)
        /// Source : Astrology for beginners pg 30
        /// </summary>
        public static bool IsPlanetYogakarakaToLagna(PlanetName planetName, ZodiacName lagna)
        {
            //handle empty
            return false;

            switch (lagna)
            {
                case ZodiacName.Aries:
                    return planetName == Sun;
                case ZodiacName.Taurus:
                    return planetName == Saturn;
                case ZodiacName.Gemini:
                    return planetName == Venus || planetName == Saturn;
                case ZodiacName.Cancer:
                    return planetName == Mars;
                case ZodiacName.Leo:
                    return planetName == Mars;
                case ZodiacName.Virgo:
                    return planetName == Mercury || planetName == Venus;
                case ZodiacName.Libra:
                    return planetName == Moon || planetName == Mercury || planetName == Saturn;
                case ZodiacName.Scorpio:
                    return planetName == Sun || planetName == Moon;
                case ZodiacName.Sagittarius:
                    return planetName == Sun || planetName == Mars;
                case ZodiacName.Capricorn:
                    return planetName == Mercury || planetName == Venus;
                case ZodiacName.Aquarius:
                    return planetName == Mars || planetName == Venus;
                case ZodiacName.Pisces:
                    return planetName == Mars || planetName == Jupiter || planetName == Moon;
            }

            //control should not come here
            throw new InvalidOperationException();
        }

        /// <summary>
        /// Yogakaraka (Planets indicating prosperity)
        /// Source : Astrology for beginners pg 30
        /// </summary>
        public static bool IsPlanetMarakaToLagna(PlanetName planetName, ZodiacName lagna)
        {
            switch (lagna)
            {
                case ZodiacName.Aries:
                    return planetName == Mercury || planetName == Saturn;
                case ZodiacName.Taurus:
                    return planetName == Jupiter || planetName == Venus;
                case ZodiacName.Gemini:
                    return planetName == Mars || planetName == Jupiter;
                case ZodiacName.Cancer:
                    return planetName == Mercury || planetName == Venus;
                case ZodiacName.Leo:
                    return planetName == Mercury || planetName == Venus;
                case ZodiacName.Virgo:
                    return planetName == Mars || planetName == Jupiter;
                case ZodiacName.Libra:
                    return planetName == Jupiter;
                case ZodiacName.Scorpio:
                    return planetName == Mercury || planetName == Venus || planetName == Saturn;
                case ZodiacName.Sagittarius:
                    return planetName == Venus || planetName == Saturn;
                case ZodiacName.Capricorn:
                    return planetName == Mars || planetName == Jupiter;
                case ZodiacName.Aquarius:
                    return planetName == Mars;
                case ZodiacName.Pisces:
                    return planetName == Mercury || planetName == Venus || planetName == Saturn;
            }

            //control should not come here
            throw new InvalidOperationException();
        }

        /// <summary>
        /// Checks if planet is placed in own house
        /// meaning house sign owned by planet
        /// note: rahu and ketu return false always
        /// </summary>
        public static bool IsPlanetInOwnHouse(PlanetName planetName, Time time)
        {
            //find out if planet is rahu or ketu, because not all calculations supported
            var isRahuKetu = planetName == Rahu || planetName == Ketu;

            //get current house
            var _planetCurrentHouse = HousePlanetOccupies(planetName, time);

            //relationship with current house
            var _currentHouseRelation = isRahuKetu ? 0 : PlanetRelationshipWithHouse(_planetCurrentHouse, planetName, time);

            //relation should be own
            if (_currentHouseRelation == PlanetToSignRelationship.OwnVarga)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// True if a planet is in a house sign owned by an enemy. Rahu and Ketu is false always
        /// </summary>
        public static bool IsPlanetInEnemyHouse(PlanetName planetName, Time time)
        {
            //find out if planet is rahu or ketu, because not all calculations supported
            var isRahuKetu = planetName == Rahu || planetName == Ketu;

            //get current house
            var _planetCurrentHouse = HousePlanetOccupies(planetName, time);

            //relationship with current house
            var _currentHouseRelation = isRahuKetu ? 0 : PlanetRelationshipWithHouse(_planetCurrentHouse, planetName, time);

            //relation should be own
            var isEnemy = _currentHouseRelation == PlanetToSignRelationship.EnemyVarga;
            var isSuperEnemy = _currentHouseRelation == PlanetToSignRelationship.BitterEnemyVarga;
            if (isEnemy || isSuperEnemy)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// True if a planet is in a house sign owned by a friend. Rahu and Ketu is false always
        /// </summary>
        public static bool IsPlanetInFriendHouse(PlanetName planetName, Time time)
        {
            //find out if planet is rahu or ketu, because not all calculations supported
            var isRahuKetu = planetName == Rahu || planetName == Ketu;

            //get current house
            var _planetCurrentHouse = HousePlanetOccupies(planetName, time);

            //relationship with current house
            var _currentHouseRelation = isRahuKetu ? 0 : PlanetRelationshipWithHouse(_planetCurrentHouse, planetName, time);

            //relation should be own
            var isEnemy = _currentHouseRelation == PlanetToSignRelationship.EnemyVarga;
            var isSuperEnemy = _currentHouseRelation == PlanetToSignRelationship.BitterEnemyVarga;
            if (isEnemy || isSuperEnemy)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Get planet's Longitude, Latitude, DistanceAU, SpeedLongitude, SpeedLatitude...
        /// Swiss Ephemeris "swe_calc" wrapper for open API 
        /// </summary>
        public static dynamic SwissEphemeris(PlanetName planetName, Time time)
        {
            //convert planet name, compatible with Swiss Eph
            int swissPlanet = Tools.VedAstroToSwissEph(planetName);

            //do the calculation
            var sweCalcResults = Tools.ephemeris_swe_calc(time, swissPlanet);

            return sweCalcResults;
        }

        /// <summary>
        /// For all planets including Pluto, Neptune, Uranus
        /// Get planet's Longitude, Latitude, DistanceAU, SpeedLongitude, SpeedLatitude...
        /// Uses Swiss Ephemeris directly to get values
        /// </summary>
        public static List<dynamic> SwissEphemerisAll(Time time)
        {
            //for all planets
            var _12Planets = new List<int>
            {
                SwissEph.SE_SUN, SwissEph.SE_MOON, SwissEph.SE_MERCURY, SwissEph.SE_MARS,
                SwissEph.SE_VENUS, SwissEph.SE_JUPITER, SwissEph.SE_SATURN,
                SwissEph.SE_URANUS, SwissEph.SE_NEPTUNE, SwissEph.SE_PLUTO,
                //rahu & ketu
                SwissEph.SE_TRUE_NODE, SwissEph.SE_OSCU_APOG,
            };

            //put all data for all planets in 1 big list
            var bigList = new List<dynamic>();
            foreach (var planet in _12Planets)
            {
                var temp = Tools.ephemeris_swe_calc(time, planet);
                bigList.Add(temp);
            }

            return bigList;
        }

        /// <summary>
        /// Checks if a planet is same house (not nessarly conjunct) with the lord of a certain house
        /// Example : Is Sun joined with lord of 9th?
        /// </summary>
        public static bool IsPlanetSameHouseWithHouseLord(int houseNumber, PlanetName planet, Time birthTime)
        {
            //get house of the lord in question
            var houseLord = LordOfHouse((HouseName)houseNumber, birthTime);
            var houseLordHouse = HousePlanetOccupies(houseLord, birthTime);

            //get house of input planet
            var inputPlanetHouse = HousePlanetOccupies(planet, birthTime);

            //check if both are in same house
            if (inputPlanetHouse == houseLordHouse)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        /// <summary>
        /// Based on Shadvarga get nature of house for a person,
        /// nature in number form to for easy calculation into summary
        /// good = 1, bad = -1, neutral = 0
        /// specially made method for life chart summary
        /// </summary>
        public static int HouseNatureScore(HouseName inputHouse, Time personBirthTime)
        {
            //if no house then no score
            if (inputHouse == HouseName.Empty)
            {
                return 0;
            }

            //get house score
            var houseStrength = HouseStrength(inputHouse, personBirthTime).ToDouble();

            //based on score determine final nature
            switch (houseStrength)
            {
                //positive
                case > 550: return 2; //extra for power
                case >= 450: return 1;

                //negative
                case < 250: return -3; //if below is even worse
                case < 350: return -2; //if below is even worse
                case < 450: return -1;
                default:
                    throw new Exception("No Strength Power defined!");
            }
        }

        /// <summary>
        /// Experimental Code
        /// </summary>
        public static double HouseNatureScoreMK4(Time personBirthTime, HouseName inputHouse)
        {
            //if no house then no score
            if (inputHouse == HouseName.Empty)
            {
                return 0;
            }

            //get house score
            var houseStrength = HouseStrength(inputHouse, personBirthTime).ToDouble();

            //weakest planet gives lowest score -2
            //strongest planet gives highest score 2
            //get range
            var highestHouseScore = HouseStrength(AllHousesOrderedByStrength(personBirthTime)[0], personBirthTime).ToDouble();
            var lowestHouseScore = HouseStrength(AllHousesOrderedByStrength(personBirthTime)[11], personBirthTime).ToDouble();

            var rangeBasedScore = houseStrength.Remap(lowestHouseScore, highestHouseScore, -3, 3);


            return rangeBasedScore;
        }

        /// <summary>
        /// Experimental Code, stand back!
        /// </summary>
        public static double PlanetNatureScoreMK4(Time personBirthTime, PlanetName inputPlanet)
        {
            //if no house then no score
            if (inputPlanet == PlanetName.Empty) { return 0; }

            //get house score
            //var planetStrength = GetPlanetShadbalaPinda(inputPlanet, personBirthTime).ToDouble();

            //weakest planet gives lowest score -2
            //strongest planet gives highest score 2
            //get range
            //var highestPlanetScore = GetPlanetShadbalaPinda(GetAllPlanetOrderedByStrength(personBirthTime)[0], personBirthTime).ToDouble();
            //var weakestPlanet = GetAllPlanetOrderedByStrength(personBirthTime)[8];
            //var lowestPlanetScore = GetPlanetShadbalaPinda(weakestPlanet, personBirthTime).ToDouble();

            //find accurate planet strength relative to others
            //if above limit than strong else weak below 0
            var isBenefic = IsPlanetStrongInShadbala(inputPlanet, personBirthTime);
            //var rangeBasedScore = 0.0;

            var x = isBenefic ? 1 : -1;

            return x;

            //if (isBenefic) //positive number
            //{
            //     rangeBasedScore = planetStrength.Remap(lowestPlanetScore, highestPlanetScore, 0, 2);

            //}
            //else // 0 or below
            //{
            //     rangeBasedScore = planetStrength.Remap(lowestPlanetScore, highestPlanetScore, -2, 0);
            //}

            //return rangeBasedScore;
        }

        /// <summary>
        /// Based on Shadvarga get nature of planet for a person,
        /// nature in number form to for easy calculation into summary
        /// good = 1, bad = -1, neutral = 0
        /// specially made method for life chart summary
        /// </summary>
        public static int PlanetNatureScore(Time personBirthTime, PlanetName inputPlanet)
        {
            //get house score
            var planetStrength = PlanetShadbalaPinda(inputPlanet, personBirthTime).ToDouble();


            //based on score determine final nature
            switch (planetStrength)
            {
                //positive
                case > 550: return 2; //extra for power
                case >= 450: return 1;

                //negative
                case < 250: return -3; //if below is even worse
                case < 350: return -2; //if below is even worse
                case < 450: return -1;
                default:
                    throw new Exception("No Strength Power defined!");
            }
        }

        /// <summary>
        /// Get a person's varna or color (character)
        /// A person's varna can be observed in real life
        /// </summary>
        public static Varna BirthVarna(Time birthTime)
        {
            //get ruling sign
            var ruleSign = PlanetZodiacSign(Moon, birthTime).GetSignName();

            //get grade
            var maleGrade = GetGrade(ruleSign);

            return maleGrade;

            //higher grade is higher class
            Varna GetGrade(ZodiacName sign)
            {
                switch (sign)
                {   //Pisces, Scorpio and Cancer represent the highest development - Brahmin 
                    case ZodiacName.Pisces:
                    case ZodiacName.Scorpio:
                    case ZodiacName.Cancer:
                        return Varna.BrahminScholar;

                    //Leo, Sagittarius and Libra indicate the second grade - or Kshatriya;
                    case ZodiacName.Leo:
                    case ZodiacName.Sagittarius:
                    case ZodiacName.Libra:
                        return Varna.KshatriyaWarrior;

                    //Aries, Gemini and Aquarius suggest the third or the Vaisya;
                    case ZodiacName.Aries:
                    case ZodiacName.Gemini:
                    case ZodiacName.Aquarius:
                        return Varna.VaisyaWorkmen;

                    //while Taurus, Virgo and Capricorn indicate the last grade, viz., Sudra
                    case ZodiacName.Taurus:
                    case ZodiacName.Virgo:
                    case ZodiacName.Capricorn:
                        return Varna.SudraServant;

                    default: throw new Exception("");
                }
            }


        }

        /// <summary>
        /// Used for judging dasa good or bad, Bala book pg 110
        /// if planet has more Ishta than good = +1
        /// else if more Kashta than bad = -1
        /// </summary>
        public static double PlanetIshtaKashtaScore(PlanetName planet, Time birthTime)
        {
            var ishtaScore = PlanetIshtaScore(planet, birthTime);

            var kashtaScore = PlanetKashtaScore(planet, birthTime);

            //if more than good, else bad
            var ishtaMore = ishtaScore > kashtaScore;

            return ishtaMore ? 1 : -1;
        }


        /// <summary>
        /// Used for judging dasa good or bad, Bala book pg 110
        /// output range -5 to 5
        /// </summary>
        public static double PlanetIshtaKashtaScoreDegree(PlanetName planet, Time birthTime)
        {
            //get both scores of good and bad
            var ishtaScore = PlanetIshtaScore(planet: planet, birthTime: birthTime);
            var kashtaScore = PlanetKashtaScore(planet: planet, birthTime: birthTime);

            //final nature of event
            var ishtaMore = ishtaScore > kashtaScore;

            //NOTE: ASTRO THEORY
            //caculate the difference between Good and Bad scores in percentage
            //so the more difference there is the greater the Good or Bad
            //if the difference is very small, than it makes sense that they
            //should cancel each other.
            var baseVal = ishtaMore ? ishtaScore : kashtaScore;
            var difference = Math.Abs(value: ishtaScore - kashtaScore);
            var ratio = difference / baseVal;
            var percentage = ratio * 100;

            var finalVal = 0.0;
            if (ishtaMore)
            {
                //remap the
                finalVal = percentage.Remap(fromMin: 0, fromMax: 100, toMin: 0, toMax: 4);
            }
            else
            {
                //remap the
                finalVal = percentage.Remap(fromMin: 0, fromMax: 100, toMin: -4, toMax: 0);
            }

            return Math.Round(finalVal, 3);
        }

        /// <summary>
        /// Experimental Code, stand back!
        /// Kashta Phala (Bad Strength) of a Planet
        /// </summary>

        public static double PlanetKashtaScore(PlanetName planet, Time birthTime)
        {
            //The Ochcha Bala (exaltation strength) of a planet
            //is multiplied by its Chesta Bala(motional strength)
            //and then the square root of the product extracted.
            var ochchaBala = PlanetOchchaBala(planet, birthTime).ToDouble();
            var chestaBala = PlanetChestaBala(planet, birthTime, true).ToDouble();
            var product = (60 - ochchaBala) * (60 - chestaBala);

            //Square root of the product extracted.
            //the result would represent the Kashta Phala.
            var ishtaScore = Math.Sqrt(product);

            return ishtaScore;

        }

        /// <summary>
        /// Ishta Phala (Good Strength) of a Planet
        /// </summary>
        public static double PlanetIshtaScore(PlanetName planet, Time birthTime)
        {
            //The Ochcha Bala (exaltation strength) of a planet
            //is multiplied by its Chesta Bala(motional strength)
            //and then the square root of the product extracted.
            var ochchaBala = PlanetOchchaBala(planet, birthTime).ToDouble();
            var chestaBala = PlanetChestaBala(planet, birthTime, true).ToDouble();
            var product = ochchaBala * chestaBala;

            //Square root of the product extracted.
            //the result would represent the Ishta Phala.
            var ishtaScore = Math.Sqrt(product);

            return ishtaScore;
        }

        /// <summary>
        /// Gets all planets in certain sign from the moon. Exp: get planets 3rd from the moon
        /// </summary>
        public static List<PlanetName> AllPlanetsSignsFromPlanet(int signsFromMoon, PlanetName startPlanet, Time birthTime)
        {
            //get the sign to check
            var moonNthSign = SignCountedFromPlanetSign(signsFromMoon, startPlanet, birthTime);

            //get all the planets in the sign
            var planetsIn = PlanetsInSign(moonNthSign, birthTime);

            return planetsIn;
        }

        /// <summary>
        /// Gets all planets in certain sign from the Lagna/Ascendant. Exp: get planets 3rd from the Lagna/Ascendant
        /// </summary>
        public static List<PlanetName> AllPlanetsInASignFromLagna(int signsFromLagna, Time birthTime)
        {
            //get the sign to check
            var lagnaNthSign = SignCountedFromLagnaSign(signsFromLagna, birthTime);

            //get all the planets in the sign
            var planetsIn = PlanetsInSign(lagnaNthSign, birthTime);

            return planetsIn;
        }

        /// <summary>
        /// Gets all planets in certain sign from the moon, given list of signs. Exp: get planets 3rd from the moon
        /// </summary>
        public static List<PlanetName> AllPlanetsSignsFromPlanet(int[] signsFromList, PlanetName startPlanet, Time birthTime)
        {
            var returnList = new List<PlanetName>();

            foreach (var sigsFrom in signsFromList)
            {
                //get all planets in given number (house) from moon
                var temp = AllPlanetsSignsFromPlanet(sigsFrom, startPlanet, birthTime);
                returnList.AddRange(temp);
            }

            //remove duplicates
            return returnList.Distinct().ToList();

        }

        /// <summary>
        /// Gets all planets in certain sign from a given planet, given list of signs. Exp: get planets 3rd from the Jupiter
        /// </summary>
        public static List<PlanetName> AllPlanetsSignsFromPlanet(int[] signsFromList, Time birthTime, PlanetName startPlanet)
        {
            var returnList = new List<PlanetName>();

            foreach (var sigsFrom in signsFromList)
            {
                //get all planets in given number (house) from moon
                var temp = AllPlanetsSignsFromPlanet(sigsFrom, birthTime, startPlanet);
                returnList.AddRange(temp);
            }

            //remove duplicates
            return returnList.Distinct().ToList();

        }

        /// <summary>
        /// Gets all planets in certain sign from the planet. Exp: get planets 3rd from the Jupiter
        /// </summary>
        public static List<PlanetName> AllPlanetsSignsFromPlanet(int signsFromMoon, Time birthTime, PlanetName startPlanet)
        {
            //get the sign to check
            var moonNthSign = SignCountedFromPlanetSign(signsFromMoon, birthTime, startPlanet);

            //get all the planets in the sign
            var planetsIn = PlanetsInSign(moonNthSign, birthTime);

            return planetsIn;
        }

        /// <summary>
        /// Gets all planets in certain sign from the Lagna/Ascendant, given list of signs. Exp: get planets 3rd from the Lagna/Ascendant
        /// </summary>
        public static List<PlanetName> AllPlanetsInSignsFromLagna(int[] signsFromList, Time birthTime)
        {
            var returnList = new List<PlanetName>();

            foreach (var sigsFrom in signsFromList)
            {
                //get all planets in given number (house) from moon
                var temp = AllPlanetsInASignFromLagna(sigsFrom, birthTime);
                returnList.AddRange(temp);
            }

            //remove duplicates
            return returnList.Distinct().ToList();

        }

        /// <summary>
        /// Checks if a given list of planets are found in any inputed signs from another planet
        /// Exp: Is Sun or Moon in 6 or 7th from Moon
        /// </summary>
        public static bool IsPlanetsInSignsFromPlanet(int[] signsFromList, PlanetName[] planetList, PlanetName startPlanet, Time birthTime)
        {
            //get all planets in given list of signs from planet
            var planetsFromPlanet = AllPlanetsSignsFromPlanet(signsFromList, birthTime, startPlanet);

            var isOccuring = false; //default to false

            //if planet is found will be set by checks below and retured as occuring
            foreach (var planet in planetsFromPlanet)
            {
                //check given list if contains planets 
                var isFound = planetList.Contains(planet);
                if (isFound)
                {
                    isOccuring = true;
                    break; //stop looking
                }
            }

            return isOccuring;
        }


        /// <summary>
        /// Checks if a given list of planets are found in any inputed signs from Lagna/Ascendant
        /// Exp: Is Sun or Moon in 6 or 7th from Lagna
        /// </summary>
        public static bool IsPlanetsInSignsFromLagna(int[] signsFromList, PlanetName[] planetList, Time birthTime)
        {
            //get all planets in given list of signs from Lagna
            var planetsFromLagna = AllPlanetsInSignsFromLagna(signsFromList, birthTime);

            var isOccuring = false; //default to false

            //if planet is found will be set by checks below and retured as occuring
            foreach (var planet in planetsFromLagna)
            {
                //check given list if contains planets 
                var isFound = planetList.Contains(planet);
                if (isFound)
                {
                    isOccuring = true;
                    break; //stop looking
                }
            }

            return isOccuring;
        }

        /// <summary>
        /// Checks if benefics are found in any inputed signs from moon
        /// Exp: Is benefics in 6 & 7th from moon
        /// </summary>
        public static bool IsBeneficsInSignsFromPlanet(int[] signsFromList, PlanetName startPlanet, Time birthTime)
        {
            //get all planets that are standard benefics at given time
            var beneficList = BeneficPlanetList(birthTime).ToArray();

            //get all planets in given list of signs from moon
            var isOccuring = IsPlanetsInSignsFromPlanet(signsFromList, beneficList, startPlanet, birthTime);

            return isOccuring;
        }

        /// <summary>
        /// Checks if benefics are found in any inputed signs from Lagna/Ascendant
        /// Exp: Is benefics in 6 & 7th from moon
        /// </summary>
        public static bool IsBeneficsInSignsFromLagna(int[] signsFromList, Time birthTime)
        {
            //get all planets that are standard benefics at given time
            var beneficList = BeneficPlanetList(birthTime).ToArray();

            //get all planets in given list of signs from lagna
            var isOccuring = IsPlanetsInSignsFromLagna(signsFromList, beneficList, birthTime);

            return isOccuring;
        }

        #endregion

        #region UPAGRAHA

        /// <summary>
        /// Dhuma Sun' s longitude + 133°20’
        /// </summary>
        public static Angle DhumaLongitude(Time time)
        {
            //get sun long
            var sunLong = Calculate.PlanetNirayanaLongitude(Sun, time);

            //add 133°20’
            var _133 = new Angle(133, 20, 0);
            var total = _133 + sunLong;

            return total.Expunge360();
        }

        /// <summary>
        /// 360°-Dhuma's longitude
        /// </summary>
        public static Angle VyatipaataLongitude(Time time)
        {
            //get needed longitude
            var dhumaLong = Calculate.DhumaLongitude(time);

            //calculate final
            var total = Angle.Degrees360 - dhumaLong;

            return total.Expunge360();
        }

        /// <summary>
        /// Vyatipaata's longitude + 180°
        /// </summary>
        public static Angle PariveshaLongitude(Time time)
        {
            //get needed longitude
            var longitude = Calculate.VyatipaataLongitude(time);

            //calculate final
            var total = longitude + Angle.Degrees180;

            return total.Expunge360();
        }

        /// <summary>
        /// 360° - Parivesha's longitude
        /// </summary>
        public static Angle IndrachaapaLongitude(Time time)
        {
            //get needed longitude
            var longitude = Calculate.PariveshaLongitude(time);

            //calculate final
            var total = Angle.Degrees360 - longitude;

            return total.Expunge360();
        }

        /// <summary>
        /// Indrachaapa's longitude + 16°40'
        /// </summary>
        public static Angle UpaketuLongitude(Time time)
        {
            //get needed longitude
            var longitude = Calculate.IndrachaapaLongitude(time);

            //calculate final
            var _1640 = new Angle(16, 40, 0);
            var total = longitude + _1640;

            return total.Expunge360();
        }

        /// <summary>
        /// Kaala rises at the middle of Sun's part. In other words,
        /// we find the time at the middle of Sun's part
        /// and find lagna rising then. That gives Kaala's longitude.
        /// </summary>
        public static Angle KaalaLongitude(Time time) => UpagrahaLongitude(time, PlanetNameEnum.Sun, "middle");

        /// <summary>
        /// Mrityu rises at the middle of Mars's part.
        /// </summary>
        public static Angle MrityuLongitude(Time time) => UpagrahaLongitude(time, PlanetNameEnum.Mars, "middle");

        /// <summary>
        /// Artha Praharaka rises at the middle of Mercury's part. 
        /// </summary>
        public static Angle ArthaprahaaraLongitude(Time time) => UpagrahaLongitude(time, PlanetNameEnum.Mercury, "middle");

        /// <summary>
        /// Yama ghantaka rises at the middle of Jupiter's part
        /// </summary>
        public static Angle YamaghantakaLongitude(Time time) => UpagrahaLongitude(time, PlanetNameEnum.Jupiter, "middle");

        /// <summary>
        /// Gulika rises at the middle of Saturn's part. 
        /// </summary>
        public static Angle GulikaLongitude(Time time) => UpagrahaLongitude(time, PlanetNameEnum.Saturn, "begin");

        /// <summary>
        /// Maandi rises at the beginning of Saturn's part.
        /// </summary>
        public static Angle MaandiLongitude(Time time) => UpagrahaLongitude(time, PlanetNameEnum.Saturn, "middle");

        /// <summary>
        /// Calculates longitudes for the non sun based Upagrahas (sub-planets)
        /// </summary>
        public static Angle UpagrahaLongitude(Time time, PlanetNameEnum relatedPlanet, string upagrahaPart)
        {
            // Once we divide the day/night of birth into 8 equal parts and identify the
            // ruling planets of the 8 parts, we can find the longitudes of Kaala etc upagrahas
            var partNumber = UpagrahaPartNumber(time, relatedPlanet); //since Kaala->Sun

            //ascertain if day birth or night birth
            var isDayBirth = Calculate.IsDayBirth(time);

            var adjustedPartNumber = partNumber - 1; //decrement part number to calculate start time of part interested in

            //calculated duration of day based on sunrise and sunset
            var dayDuration = Calculate.DayDurationHours(time);
            //since there are 8 parts, hours per part is roughly ~1.5
            var hoursPerPart = dayDuration / 8.0;

            //place to store all longitudes for house 1 (lagna)
            House lagnaLongitudes;

            //# Based on night or day birth calculate the
            //longitude based on lagna position at given part number

            //day birth
            if (isDayBirth)
            {
                //get time the part starts after sunrise before sunset
                var hoursAfterSunrise = adjustedPartNumber * hoursPerPart;

                //calculate start time based on sunrise
                var sunrise = Calculate.SunriseTime(time);
                var partStartTime = sunrise.AddHours(hoursAfterSunrise);

                //calculate middle point in time of part (~1.5/2 = ~0.75 hours)
                var hoursPerHalfPart = hoursPerPart / 2;
                var partMiddleTime = partStartTime.AddHours(hoursPerHalfPart);

                //get lagna longitude at this middle time, which is the sub planet's long
                //NOTE ASSUMPITION: only possible values "middle" or "begin"
                var selectedPart = upagrahaPart == "middle" ? partMiddleTime :
                    upagrahaPart == "begin" ? partStartTime : throw new Exception("END OF LINE!");
                var allHouseMiddleLongitudes = Calculate.AllHouseMiddleLongitudes(selectedPart);
                lagnaLongitudes = allHouseMiddleLongitudes.Where(x => x.GetHouseName() == HouseName.House1).First();
            }
            //nigth birth
            else
            {
                //get time the part starts after sunset before sunrise next day
                var hoursAfterSunset = adjustedPartNumber * hoursPerPart;

                //calculate start time based on sunrise
                var sunset = Calculate.SunsetTime(time);
                var partStartTime = sunset.AddHours(hoursAfterSunset);

                //calculate middle point in time of part (~1.5/2 = ~0.75 hours)
                var hoursPerHalfPart = hoursPerPart / 2;
                var partMiddleTime = partStartTime.AddHours(hoursPerHalfPart);

                //get lagna longitude at this middle time, which is the sub planet's long
                //NOTE ASSUMPITION: only possible values "middle" or "begin"
                var selectedPart = upagrahaPart == "middle" ? partMiddleTime :
                    upagrahaPart == "begin" ? partStartTime : throw new Exception("END OF LINE!");
                var allHouseMiddleLongitudes = Calculate.AllHouseMiddleLongitudes(selectedPart);
                lagnaLongitudes = allHouseMiddleLongitudes.Where(x => x.GetHouseName() == HouseName.House1).First();
            }

            return lagnaLongitudes.GetMiddleLongitude();

        }

        /// <summary>
        /// Depending on whether one is born during the day or the night, we divide the
        /// length of the day/night into 8 equal parts. Each part is assigned a planet.
        /// Given a planet and time the part number will be returned.
        /// Each part is 12/8 = 1.5 hours.
        /// </summary>
        public static int UpagrahaPartNumber(Time inputTime, PlanetNameEnum inputPlanet)
        {
            //based on night or day birth get the number accoridngly
            var isDayBirth = Calculate.IsDayBirth(inputTime);

            if (isDayBirth)
            {
                return UpagrahaPartNumberDayBirth(inputTime, inputPlanet);
            }
            else
            {
                return UpagrahaPartNumberNightBirth(inputTime, inputPlanet);
            }


            //------------------LOCAL FUNCS-------------------------

            int UpagrahaPartNumberNightBirth(Time inputTime, PlanetNameEnum inputPlanet)
            {
                //get weekday
                var weekday = Calculate.DayOfWeek(inputTime);

                //based on weekday and planet name return part number
                //NOTE: table data from 
                Dictionary<DayOfWeek, Dictionary<PlanetNameEnum, int>> nightRulers = new Dictionary<DayOfWeek, Dictionary<PlanetNameEnum, int>>
            {
                { Library.DayOfWeek.Sunday, new Dictionary<PlanetNameEnum, int>
                    { { PlanetNameEnum.Jupiter, 1 }, { PlanetNameEnum.Venus, 2 }, { PlanetNameEnum.Saturn, 3 }, { PlanetNameEnum.Empty, 4 }, { PlanetNameEnum.Sun, 5 }, { PlanetNameEnum.Moon, 6 }, { PlanetNameEnum.Mars ,7 }, { PlanetNameEnum.Mercury ,8 } }
                },
                { Library.DayOfWeek.Monday, new Dictionary<PlanetNameEnum, int>
                    { { PlanetNameEnum.Venus, 1 }, { PlanetNameEnum.Saturn, 2 }, { PlanetNameEnum.Empty, 3 }, { PlanetNameEnum.Sun, 4 }, { PlanetNameEnum.Moon, 5 }, { PlanetNameEnum.Mars, 6 }, { PlanetNameEnum.Mercury, 7 }, { PlanetNameEnum.Jupiter ,8 } }
                },
                { Library.DayOfWeek.Tuesday, new Dictionary<PlanetNameEnum, int>
                    { { PlanetNameEnum.Saturn, 1 }, { PlanetNameEnum.Empty, 2 }, { PlanetNameEnum.Sun, 3 }, { PlanetNameEnum.Moon, 4 }, { PlanetNameEnum.Mars, 5 }, { PlanetNameEnum.Mercury, 6 }, { PlanetNameEnum.Jupiter, 7 }, { PlanetNameEnum.Venus ,8 } }
                },
                { Library.DayOfWeek.Wednesday, new Dictionary<PlanetNameEnum, int>
                    { { PlanetNameEnum.Sun, 1 }, { PlanetNameEnum.Moon, 2 }, { PlanetNameEnum.Mars, 3 }, { PlanetNameEnum.Mercury, 4 }, { PlanetNameEnum.Jupiter, 5 }, { PlanetNameEnum.Venus, 6 }, { PlanetNameEnum.Saturn, 7 }, { PlanetNameEnum.Empty ,8} }
                },
                { Library.DayOfWeek.Thursday, new Dictionary<PlanetNameEnum, int>
                    { { PlanetNameEnum.Moon, 1 }, { PlanetNameEnum.Mars, 2 }, { PlanetNameEnum.Mercury, 3 }, { PlanetNameEnum.Jupiter, 4 }, { PlanetNameEnum.Venus, 5 }, { PlanetNameEnum.Saturn, 6 }, { PlanetNameEnum.Empty, 7 }, { PlanetNameEnum.Sun ,8 } }
                },
                { Library.DayOfWeek.Friday, new Dictionary<PlanetNameEnum, int>
                    { { PlanetNameEnum.Mars, 1 }, { PlanetNameEnum.Mercury, 2 }, { PlanetNameEnum.Jupiter, 3 }, { PlanetNameEnum.Venus, 4 }, { PlanetNameEnum.Saturn, 5 }, { PlanetNameEnum.Empty, 6 }, { PlanetNameEnum.Sun, 7 }, { PlanetNameEnum.Moon ,8 } }
                },
                { Library.DayOfWeek.Saturday, new Dictionary<PlanetNameEnum, int>
                    { { PlanetNameEnum.Mercury, 1 }, { PlanetNameEnum.Jupiter, 2 }, { PlanetNameEnum.Venus, 3 }, { PlanetNameEnum.Saturn, 4 }, { PlanetNameEnum.Empty, 5 }, { PlanetNameEnum.Sun, 6 }, { PlanetNameEnum.Moon, 7 }, { PlanetNameEnum.Mars ,8 } }
                },

            };

                if (nightRulers.TryGetValue(weekday, out var planetParts))
                {
                    if (planetParts.TryGetValue(inputPlanet, out var partNumber))
                    {
                        return partNumber;
                    }
                    throw new Exception("Invalid planet name");
                }

                throw new Exception("Invalid day of week");
            }


            int UpagrahaPartNumberDayBirth(Time inputTime, PlanetNameEnum inputPlanet)
            {
                //get weekday
                var weekday = Calculate.DayOfWeek(inputTime);

                //based on weekday and planet name return part number
                //NOTE: table data from 
                Dictionary<DayOfWeek, Dictionary<PlanetNameEnum, int>> dayRulers = new Dictionary<DayOfWeek, Dictionary<PlanetNameEnum, int>>
            {
                { Library.DayOfWeek.Sunday, new Dictionary<PlanetNameEnum, int>
                    { { PlanetNameEnum.Sun, 1 }, { PlanetNameEnum.Moon, 2 }, { PlanetNameEnum.Mars ,3 }, { PlanetNameEnum.Mercury ,4 }, { PlanetNameEnum.Jupiter ,5 }, { PlanetNameEnum.Venus ,6 }, { PlanetNameEnum.Saturn ,7 }, { PlanetNameEnum.Empty ,8 } }
                },
                { Library.DayOfWeek.Monday, new Dictionary<PlanetNameEnum, int>
                    { { PlanetNameEnum.Moon, 1 }, { PlanetNameEnum.Mars ,2 }, { PlanetNameEnum.Mercury ,3 }, { PlanetNameEnum.Jupiter ,4},  {PlanetNameEnum.Venus ,5},  {PlanetNameEnum.Saturn ,6},  {PlanetNameEnum.Empty ,7}, { PlanetNameEnum.Sun ,8 } }
                },
                { Library.DayOfWeek.Tuesday, new Dictionary<PlanetNameEnum, int>
                    { { PlanetNameEnum.Mars, 1 }, { PlanetNameEnum.Mercury ,2 }, { PlanetNameEnum.Jupiter ,3 }, { PlanetNameEnum.Venus ,4},  {PlanetNameEnum.Saturn ,5},  {PlanetNameEnum.Empty ,6},  {PlanetNameEnum.Sun ,7}, { PlanetNameEnum.Moon ,8 } }
                },
                { Library.DayOfWeek.Wednesday, new Dictionary<PlanetNameEnum, int>
                    { { PlanetNameEnum.Mercury, 1 }, { PlanetNameEnum.Jupiter ,2 }, { PlanetNameEnum.Venus ,3 }, { PlanetNameEnum.Saturn ,4},  {PlanetNameEnum.Empty ,5},  {PlanetNameEnum.Sun ,6},  {PlanetNameEnum.Moon ,7}, { PlanetNameEnum.Mars ,8 } }
                },
                { Library.DayOfWeek.Thursday, new Dictionary<PlanetNameEnum, int>
                    { { PlanetNameEnum.Jupiter, 1 }, { PlanetNameEnum.Venus ,2 }, { PlanetNameEnum.Saturn ,3 }, { PlanetNameEnum.Empty ,4},  {PlanetNameEnum.Sun ,5},  {PlanetNameEnum.Moon ,6},  {PlanetNameEnum.Mars ,7}, { PlanetNameEnum.Mercury ,8 } }
                },
                { Library.DayOfWeek.Friday, new Dictionary<PlanetNameEnum, int>
                    { { PlanetNameEnum.Venus, 1 }, { PlanetNameEnum.Saturn ,2 }, { PlanetNameEnum.Empty ,3 }, { PlanetNameEnum.Sun ,4},  {PlanetNameEnum.Moon ,5},  {PlanetNameEnum.Mars ,6},  {PlanetNameEnum.Mercury ,7}, { PlanetNameEnum.Jupiter ,8 } }
                },
                { Library.DayOfWeek.Saturday, new Dictionary<PlanetNameEnum, int>
                    { { PlanetNameEnum.Saturn, 1 }, { PlanetNameEnum.Empty ,2 }, { PlanetNameEnum.Sun ,3 }, { PlanetNameEnum.Moon ,4},  {PlanetNameEnum.Mars ,5},  {PlanetNameEnum.Mercury ,6},  {PlanetNameEnum.Jupiter ,7}, { PlanetNameEnum.Venus ,8 } }
                },
            };


                if (dayRulers.TryGetValue(weekday, out var planetParts))
                {
                    if (planetParts.TryGetValue(inputPlanet, out var partNumber))
                    {
                        return partNumber;
                    }
                    throw new Exception("Invalid planet name");
                }

                throw new Exception("Invalid day of week");
            }

        }

        /// <summary>
        /// Given a planet name will tell if it is an Upagraha planet
        /// </summary>
        public static bool IsUpagraha(PlanetName planet)
        {
            var planetName = planet.Name;
            switch (planetName)
            {
                case PlanetNameEnum.Dhuma:
                case PlanetNameEnum.Vyatipaata:
                case PlanetNameEnum.Parivesha:
                case PlanetNameEnum.Indrachaapa:
                case PlanetNameEnum.Upaketu:
                case PlanetNameEnum.Kaala:
                case PlanetNameEnum.Mrityu:
                case PlanetNameEnum.Arthaprahaara:
                case PlanetNameEnum.Yamaghantaka:
                case PlanetNameEnum.Gulika:
                case PlanetNameEnum.Maandi:
                    return true;
            }

            //if control reaches here than must be normal planet
            return false;
        }

        #endregion

        #region CACHED FUNCTIONS
        //NOTE : These are functions that don't call other functions from this class
        //       Only functions that don't call other cached functions are allowed to be cached
        //       otherwise, it's erroneous in parallel

        /// <summary>
        /// Gets nutation from Swiss Ephemeris
        public static double Nutation(Time time)
        {
            SwissEph swissEph = new SwissEph();
            double[] x = new double[6];
            string serr = "";

            var julDayUt = Calculate.TimeToJulianDay(time);

            swissEph.swe_calc(julDayUt, SwissEph.SE_ECL_NUT, 0, x, ref serr);
            return x[2]; //See SWISS EPH docs and confirm array location - is it 1 or 2??
        }

        /// <summary>
        /// This method is used to convert the tropical ascendant to the ARMC (Ascendant Right Meridian Circle).
        /// It first calculates the right ascension and declination using the provided tropical ascendant and
        /// obliquity of the ecliptic. Then, it calculates the oblique ascension by subtracting a value derived
        /// from the declination and geographic latitude from the right ascension. Finally, it calculates the ARMC
        /// based on the value of the tropical ascendant and the oblique ascension.
        /// </summary>
        public static double AscendantDegreesToARMC(double ascendant, double obliquityOfEcliptic, double geographicLatitude, Time time)
        {
            //NEEDS UPDATE CP

            // The main method is taken from a post by K S Upendra on Group.IO in 2019
            // Calculate the right ascension using the formula:
            // atan(cos(obliquityOfEcliptic) * tan(tropicalAscendant))
            double rightAscension = 4.98;

            // Calculate the declination using the formula:
            // asin(sin(obliquityOfEcliptic) * sin(tropicalAscendant))
            double declination = 6.64;

            // Calculate the oblique ascension by subtracting the result of the following formula from the right ascension:
            // asin(tan(declination) * tan(geographicLatitude))
            double obliqueAscension = rightAscension -
                                      (Math.Asin(Math.Tan(declination * Math.PI / 180) *
                                                 Math.Tan(geographicLatitude * Math.PI / 180)) * 180 / Math.PI);
            // Initialize the armc variable
            double armc = 0;
            // Depending on the value of the tropical ascendant, calculate the armc using the formula:
            // armc = 270 + obliqueAscension or armc = 90 + obliqueAscension
            if (ascendant >= 0 && ascendant < 90)
            {
                armc = 270 + obliqueAscension;
            }
            else if (ascendant >= 90 && ascendant < 180)
            {
                armc = 90 + obliqueAscension;
            }
            else if (ascendant >= 180 && ascendant < 270)
            {
                armc = 90 + obliqueAscension;
            }
            else if (ascendant >= 270 && ascendant < 360)
            {
                armc = 270 + obliqueAscension;
            }
            // Return the calculated armc value
            return armc;
        }

        /// <summary>
        /// The distance between the Hindu First Point and the Vernal Equinox, measured at an epoch, is known as the Ayanamsa
        /// in Varahamihira's time, the summer solistice coincided with the first degree of Cancer,
        /// and the winter solistice with the first degree of Capricorn, whereas at one time the summer solistice coincided with the
        /// middle of the Aslesha
        /// </summary>
        public static Angle AyanamsaDegree(Time time)
        {

            //it has been observed and proved mathematically, that each year at the time when the Sun reaches his
            //equinoctial point of Aries 0° when throughout the earth, the day and night are equal in length,
            //the position of the earth in reference to some fixed star is nearly 50.333 of space farther west
            //than the earth was at the same equinoctial moment of the previous year.


            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey(nameof(AyanamsaDegree), time, Ayanamsa), _getAyanamsaDegree);


            //UNDERLYING FUNCTION
            Angle _getAyanamsaDegree()
            {
                //This would request sidereal positions calculated using the Swiss Ephemeris.
                int iflag = SwissEph.SEFLG_SIDEREAL;
                //int iflag = SwissEph.SEFLG_NONUT;
                double jul_day_ET;
                SwissEph ephemeris = new SwissEph();

                // Convert DOB to ET
                jul_day_ET = TimeToEphemerisTime(time);

                //set ayanamsa
                ephemeris.swe_set_sid_mode(Ayanamsa, 0, 0);



                //var ayanamsaDegree = ephemeris.swe_get_ayanamsa(jul_day_ET);
                //return Angle.FromDegrees(ayanamsaDegree);

                //USE this newer method in Swiss Eph intrduced in Ver 2.0. See Swiss Eph for Documentation
                //CPJ Add/Change Nov 22 2023 because Ayanamsa not precise compared to other software products
                //this provides higher precision Ayanamsa
                string serr = "";
                double daya;
                var ayanamsaDegree = ephemeris.swe_get_ayanamsa_ex(jul_day_ET, iflag, out daya, ref serr);

                return Angle.FromDegrees(daya);

            }

        }

        /// <summary>
        /// Get fixed longitude used in western systems, connects SwissEph Library with VedAstro
        /// NOTE This method connects SwissEph Library with VedAstro Library
        /// </summary>
        public static Angle PlanetSayanaLongitude(PlanetName planetName, Time time)
        {

            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey(nameof(PlanetSayanaLongitude), time, planetName, Ayanamsa), _getPlanetSayanaLongitude);


            //UNDERLYING FUNCTION

            Angle _getPlanetSayanaLongitude()
            {
                //Converts LMT to UTC (GMT)
                //DateTimeOffset utcDate = lmtDateTime.ToUniversalTime();

                int iflag = SwissEph.SEFLG_SWIEPH;  //+ SwissEph.SEFLG_SPEED;
                double[] results = new double[6];
                string err_msg = "";
                SwissEph ephemeris = new SwissEph();

                // Convert DOB to ET
                double jul_day_ET = TimeToEphemerisTime(time);

                //convert planet name, compatible with Swiss Eph
                int swissPlanet = Tools.VedAstroToSwissEph(planetName);

                //Get planet long
                int ret_flag = ephemeris.swe_calc(jul_day_ET, swissPlanet, iflag, results, ref err_msg);



                //data in results at index 0 is longitude
                var planetSayanaLongitude = Angle.FromDegrees(results[0]);

                //if ketu add 180 to rahu
                if (planetName == Ketu)
                {
                    var x = planetSayanaLongitude + Angle.Degrees180;
                    planetSayanaLongitude = x.Expunge360();
                }

                return planetSayanaLongitude;

            }


        }

        /// <summary>
        /// Planet longitude that has been corrected with Ayanamsa
        /// Gets planet longitude used vedic astrology
        /// Nirayana Longitude = Sayana Longitude corrected to Ayanamsa
        /// Number from 0 to 360, represent the degrees in the zodiac as viewed from earth
        /// Note: Since Nirayana is corrected, in actuality 0 degrees will start at Taurus not Aries
        /// </summary>
        public static Angle PlanetNirayanaLongitude(PlanetName planetName, Time time)
        {

            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey(nameof(PlanetNirayanaLongitude), time, planetName, Ayanamsa), _getPlanetNirayanaLongitude);


            //UNDERLYING FUNCTION

            Angle _getPlanetNirayanaLongitude()
            {
                //if Upagrahas hadle seperately
                if (Calculate.IsUpagraha(planetName))
                {
                    //calculate upagraha
                    switch (planetName.Name)
                    {
                        case PlanetNameEnum.Dhuma: return Calculate.DhumaLongitude(time);
                        case PlanetNameEnum.Vyatipaata: return Calculate.VyatipaataLongitude(time);
                        case PlanetNameEnum.Parivesha: return Calculate.PariveshaLongitude(time);
                        case PlanetNameEnum.Indrachaapa: return Calculate.IndrachaapaLongitude(time);
                        case PlanetNameEnum.Upaketu: return Calculate.UpaketuLongitude(time);
                        case PlanetNameEnum.Kaala: return Calculate.KaalaLongitude(time);
                        case PlanetNameEnum.Mrityu: return Calculate.MrityuLongitude(time);
                        case PlanetNameEnum.Arthaprahaara: return Calculate.ArthaprahaaraLongitude(time);
                        case PlanetNameEnum.Yamaghantaka: return Calculate.YamaghantakaLongitude(time);
                        case PlanetNameEnum.Gulika: return Calculate.GulikaLongitude(time);
                        case PlanetNameEnum.Maandi: return Calculate.MaandiLongitude(time);
                    }
                }


                //This would request sidereal positions calculated using the Swiss Ephemeris.
                int iflag = SwissEph.SEFLG_SIDEREAL | SwissEph.SEFLG_SWIEPH; // SEFLG_SIDEREAL | ; //+ SwissEph.SEFLG_SPEED;
                double[] results = new double[6];
                string err_msg = "";
                double jul_day_ET;
                SwissEph ephemeris = new SwissEph();

                // Convert DOB to ET
                jul_day_ET = TimeToEphemerisTime(time);

                //convert planet name, compatible with Swiss Eph
                int swissPlanet = Tools.VedAstroToSwissEph(planetName);

                //NOTE Ayanamsa needs to be set before caling calc
                ephemeris.swe_set_sid_mode(Ayanamsa, 0, 0);

                //do calculation
                int ret_flag = ephemeris.swe_calc(jul_day_ET, swissPlanet, iflag, results, ref err_msg);

                //data in results at index 0 is longitude
                var planetSayanaLongitude = Angle.FromDegrees(results[0]);

                //if ketu add 180 to rahu
                if (planetName == Ketu)
                {
                    var x = planetSayanaLongitude + Angle.Degrees180;
                    planetSayanaLongitude = x.Expunge360();
                }

                return planetSayanaLongitude;

            }


        }

        /// <summary>
        /// find time of next lunar eclipse UTC time
        /// </summary>
        public static DateTime NextLunarEclipse(Time time)
        {

            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey(nameof(NextLunarEclipse), time, Ayanamsa), _getNextLunarEclipse);


            //UNDERLYING FUNCTION

            DateTime _getNextLunarEclipse()
            {
                int iflag = SwissEph.SEFLG_SWIEPH;  //+ SwissEph.SEFLG_SPEED;
                double[] results = new double[10];
                string err_msg = "";
                SwissEph ephemeris = new SwissEph();

                // Convert DOB to ET
                var jul_day_ET = Calculate.ConvertLmtToJulian(time);

                //Get planet long
                var eclipseType = 0; /* eclipse type wanted: SE_ECL_TOTAL etc. or 0, if any eclipse type */
                var backward = false; /* TRUE, if backward search */
                int ret_flag = ephemeris.swe_lun_eclipse_when(jul_day_ET, iflag, eclipseType, results, backward, ref err_msg);

                //get raw results out
                var eclipseMaxTime = results[0]; //time of maximum eclipse (Julian day number)

                //convert to UTC Time
                var utcTime = Calculate.ConvertJulianTimeToNormalTime(eclipseMaxTime);

                return utcTime;

            }


        }

        /// <summary>
        /// finds the next solar eclipse globally UTC time
        /// </summary>
        public static DateTime NextSolarEclipse(Time time)
        {

            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey(nameof(NextSolarEclipse), time, Ayanamsa), _getNextSolarEclipse);


            //UNDERLYING FUNCTION

            DateTime _getNextSolarEclipse()
            {
                int iflag = SwissEph.SEFLG_SWIEPH;  //+ SwissEph.SEFLG_SPEED;
                double[] results = new double[10];
                string err_msg = "";
                double jul_day_ET;
                SwissEph ephemeris = new SwissEph();

                // Convert DOB to ET
                jul_day_ET = Calculate.ConvertLmtToJulian(time);

                //Get planet long
                var eclipseType = 0; /* eclipse type wanted: SE_ECL_TOTAL etc. or 0, if any eclipse type */
                var backward = false; /* TRUE, if backward search */
                int ret_flag = ephemeris.swe_sol_eclipse_when_glob(jul_day_ET, iflag, eclipseType, results, backward, ref err_msg);

                //get raw results out
                var eclipseMaxTime = results[0]; //time of maximum eclipse (Julian day number)

                //convert to UTC Time
                var utcTime = Calculate.ConvertJulianTimeToNormalTime(eclipseMaxTime);

                return utcTime;

            }


        }

        /// <summary>
        /// Get fixed longitude used in western systems aka Sayana longitude
        /// NOTE This method connects SwissEph Library with VedAstro Library
        /// </summary>
        public static Angle PlanetEphemerisLongitude(PlanetName planetName, Time time)
        {

            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey(nameof(PlanetEphemerisLongitude), time, planetName, Ayanamsa), _getPlanetSayanaLongitude);


            //UNDERLYING FUNCTION

            Angle _getPlanetSayanaLongitude()
            {
                //Converts LMT to UTC (GMT)
                //DateTimeOffset utcDate = lmtDateTime.ToUniversalTime();

                int iflag = SwissEph.SEFLG_SWIEPH;  //+ SwissEph.SEFLG_SPEED;
                double[] results = new double[6];
                string err_msg = "";
                double jul_day_ET;
                SwissEph ephemeris = new SwissEph();

                // Convert DOB to ET
                jul_day_ET = TimeToEphemerisTime(time);

                //convert planet name, compatible with Swiss Eph
                int swissPlanet = Tools.VedAstroToSwissEph(planetName);

                //Get planet long
                int ret_flag = ephemeris.swe_calc(jul_day_ET, swissPlanet, iflag, results, ref err_msg);

                //data in results at index 0 is longitude
                var planetSayanaLongitude = Angle.FromDegrees(results[0]);

                //if ketu add 180 to rahu
                if (planetName == Library.PlanetName.Ketu)
                {
                    var x = planetSayanaLongitude + Angle.Degrees180;
                    planetSayanaLongitude = x.Expunge360();
                }

                return planetSayanaLongitude;
            }


        }

        /// <summary>
        /// Gets Swiss Ephemeris longitude for a planet
        /// </summary>
        public static Angle PlanetSayanaLatitude(PlanetName planetName, Time time)
        {

            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey(nameof(PlanetSayanaLatitude), time, planetName, Ayanamsa), _getPlanetSayanaLatitude);


            //UNDERLYING FUNCTION

            Angle _getPlanetSayanaLatitude()
            {
                //Converts LMT to UTC (GMT)
                //DateTimeOffset utcDate = lmtDateTime.ToUniversalTime();

                int planet = 0;
                int iflag = SwissEph.SEFLG_SWIEPH;  //+ SwissEph.SEFLG_SPEED;
                double[] results = new double[6];
                string err_msg = "";
                double jul_day_ET;
                SwissEph ephemeris = new SwissEph();

                // Convert DOB to ET
                jul_day_ET = TimeToEphemerisTime(time);


                //Convert PlanetName to SE_PLANET type
                if (planetName == Library.PlanetName.Sun)
                    planet = SwissEph.SE_SUN;
                else if (planetName == Library.PlanetName.Moon)
                {
                    planet = SwissEph.SE_MOON;
                }
                else if (planetName == Library.PlanetName.Mars)
                {
                    planet = SwissEph.SE_MARS;
                }
                else if (planetName == Library.PlanetName.Mercury)
                {
                    planet = SwissEph.SE_MERCURY;
                }
                else if (planetName == Library.PlanetName.Jupiter)
                {
                    planet = SwissEph.SE_JUPITER;
                }
                else if (planetName == Library.PlanetName.Venus)
                {
                    planet = SwissEph.SE_VENUS;
                }
                else if (planetName == Library.PlanetName.Saturn)
                {
                    planet = SwissEph.SE_SATURN;
                }
                else if (planetName == Library.PlanetName.Rahu)
                {
                    planet = SwissEph.SE_MEAN_NODE;
                }
                else if (planetName == Library.PlanetName.Ketu)
                {
                    planet = SwissEph.SE_MEAN_NODE;
                }

                //Get planet long
                int ret_flag = ephemeris.swe_calc(jul_day_ET, planet, iflag, results, ref err_msg);

                //data in results at index 1 is latitude
                return Angle.FromDegrees(results[1]);

            }


        }

        /// <summary>
        /// Speed of planet from Swiss eph
        /// </summary>
        public static double PlanetSpeed(PlanetName planetName, Time time)
        {
            //Converts LMT to UTC (GMT)
            //DateTimeOffset utcDate = lmtDateTime.ToUniversalTime();

            int planet = 0;
            int iflag = SwissEph.SEFLG_SWIEPH | SwissEph.SEFLG_SPEED;
            double[] results = new double[6];
            string err_msg = "";
            double jul_day_ET;
            SwissEph ephemeris = new SwissEph();

            // Convert DOB to ET
            jul_day_ET = TimeToEphemerisTime(time);


            //Convert PlanetName to SE_PLANET type
            if (planetName == Library.PlanetName.Sun)
                planet = SwissEph.SE_SUN;
            else if (planetName == Library.PlanetName.Moon)
            {
                planet = SwissEph.SE_MOON;
            }
            else if (planetName == Library.PlanetName.Mars)
            {
                planet = SwissEph.SE_MARS;
            }
            else if (planetName == Library.PlanetName.Mercury)
            {
                planet = SwissEph.SE_MERCURY;
            }
            else if (planetName == Library.PlanetName.Jupiter)
            {
                planet = SwissEph.SE_JUPITER;
            }
            else if (planetName == Library.PlanetName.Venus)
            {
                planet = SwissEph.SE_VENUS;
            }
            else if (planetName == Library.PlanetName.Saturn)
            {
                planet = SwissEph.SE_SATURN;
            }
            else if (planetName == Library.PlanetName.Rahu)
            {
                planet = SwissEph.SE_MEAN_NODE;
            }
            else if (planetName == Library.PlanetName.Ketu)
            {
                planet = SwissEph.SE_MEAN_NODE;
            }

            //Get planet long
            int ret_flag = ephemeris.swe_calc(jul_day_ET, planet, iflag, results, ref err_msg);

            //data in results at index 3 is speed in right ascension (deg/day)
            return results[3];
        }

        /// <summary>
        /// Converts Planet Longitude to Constellation equivelant
        /// Gets info about the constellation at a given longitude, ie. Constellation Name,
        /// Quarter, Degrees in constellation, etc.
        /// </summary>
        public static Constellation ConstellationAtLongitude(Angle planetLongitude)
        {

            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey(nameof(ConstellationAtLongitude), planetLongitude, Ayanamsa), _constellationAtLongitude);


            //UNDERLYING FUNCTION
            Constellation _constellationAtLongitude()
            {
                if (planetLongitude == null) { return Library.Constellation.Empty; }

                //if planet longitude is negative means, it before aries at 0, starts back at 360 pieces
                if (planetLongitude.TotalDegrees < 0)
                {
                    planetLongitude = Angle.FromDegrees(360.0 + planetLongitude.TotalDegrees); //use plus because number is already negative
                }

                //get planet's longitude in minutes
                var planetLongitudeInMinutes = planetLongitude.TotalMinutes;

                //The ecliptic is divided into 27 constellations
                //of 13° 20' (800') each. Hence divide 800
                var roughConstellationNumber = planetLongitudeInMinutes / 800.0;

                //get constellation number (rounds up)
                var constellationNumber = (int)Math.Ceiling(roughConstellationNumber);

                //if constellation number = 0, then its 1 - CPJ Added to handle 0 degree longitude items
                if (constellationNumber == 0) { constellationNumber = 1; }

                //calculate quarter from remainder
                int quarter;

                var remainder = roughConstellationNumber - Math.Floor(roughConstellationNumber);

                //CPJ Amnded Code - March 13, 2024 - changed the upper limit not to be <= 0.25 but only < 0.25. 
                //This returns the Pada correctly. Try edge case Long 270 degrees. It is the start of U-Ashada Pada 2.
                //The equal to value is the lower limit of each case below
                if (remainder >= 0 && remainder < 0.25) quarter = 1;
                else if (remainder >= 0.25 && remainder < 0.5) quarter = 2;
                else if (remainder >= 0.5 && remainder < 0.75) quarter = 3;
                else if (remainder >= 0.75 && remainder <= 1) quarter = 4;
                else quarter = 0;

                //calculate "degrees in constellation" from the remainder
                var minutesInConstellation = remainder * 800.0;
                var degreesInConstellation = new Angle(0, minutesInConstellation, 0);

                var constellation = new Constellation();
                //put together all the info of this point in the constellation
                //CPJ Added Code Change - March 13, 2024 - to fix an error with edge cases - example 266.666667Long results in remainder = 0.
                //CPJ - When remainder = 0, new Constellation should return next Constellation Pada 1. Hence the if-else code change
                if (minutesInConstellation == 0)
                {
                    constellation = new Constellation((constellationNumber + 1), quarter, degreesInConstellation);
                }
                else
                {

                    constellation = new Constellation(constellationNumber, quarter, degreesInConstellation);
                }

                //return constellation value
                return constellation;
            }

        }


        /// <summary>
        /// Converts Planet Longitude to Zodiac Sign equivalent
        /// </summary>
        public static ZodiacSign ZodiacSignAtLongitude(Angle longitude)
        {

            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey(nameof(ZodiacSignAtLongitude), longitude, Ayanamsa), _zodiacSignAtLongitude);


            //UNDERLYING FUNCTION
            ZodiacSign _zodiacSignAtLongitude()
            {
                //max degrees of each sign
                const double maxDegreesInSign = 30.0;

                // Adjust longitude to be within 0-360 range
                double adjustedLongitude = longitude.TotalDegrees;
                while (adjustedLongitude < 0)
                {
                    adjustedLongitude += 360.0;
                }
                //get rough zodiac number
                double roughZodiacNumber = (adjustedLongitude % 360.0) / maxDegreesInSign;

                //Calculate degrees in zodiac sign
                //get remainder from rough zodiac number
                var roughZodiacNumberRemainder = roughZodiacNumber - Math.Truncate(roughZodiacNumber);

                //convert remainder to degrees in current sign
                var degreesInSignRaw = roughZodiacNumberRemainder * maxDegreesInSign;

                //round number (too high accuracy causes equality mismtach because of minute difference)
                var degreesInSign = Math.Round(degreesInSignRaw, 7);

                //Get name of zodiac sign
                //round to ceiling to get integer zodiac number
                var zodiacNumber = (int)Math.Ceiling(roughZodiacNumber);
                if (adjustedLongitude == 0.00) { zodiacNumber = 1; }

                //convert zodiac number to zodiac name
                var calculatedZodiac = (ZodiacName)zodiacNumber;

                //return new instance of planet sign
                var degreesAngle = Angle.FromDegrees(Math.Abs(degreesInSign)); //make always positive

                var zodiacSignAtLongitude = new ZodiacSign(calculatedZodiac, degreesAngle);
                return zodiacSignAtLongitude;
            }


        }

        /// <summary>
        /// Converts Zodiac Sign to Planet Longitude equivalent
        /// </summary>
        public static Angle LongitudeAtZodiacSign(ZodiacSign zodiacSign)
        {

            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey(nameof(LongitudeAtZodiacSign), zodiacSign, Ayanamsa), _getLongitudeAtZodiacSign);


            //UNDERLYING FUNCTION
            Angle _getLongitudeAtZodiacSign()
            {
                //convert zodic name to its number equivelant in order
                var zodiacNumber = (int)zodiacSign.GetSignName();

                //calculate planet longitude to sign just before
                var zodiacBefore = zodiacNumber - 1;
                var maxDegreesInSign = 30.0;
                var longtiudeToBefore = Angle.FromDegrees(maxDegreesInSign * zodiacBefore);

                //add planet longitude from sign just before with
                //degrees already traversed in current sign
                var totalLongitude = longtiudeToBefore + zodiacSign.GetDegreesInSign();

                return totalLongitude;
            }


        }

        /// <summary>
        /// Get Vedic Day Of Week
        /// The Hindu day begins with sunrise and continues till
        /// next sunrise.The first hora on any day will be the
        /// first hour after sunrise and the last hora, the hour
        /// before sunrise the next day.
        /// </summary>
        public static DayOfWeek DayOfWeek(Time time)
        {
            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey(nameof(DayOfWeek), time, Ayanamsa), _getDayOfWeek);


            //UNDERLYING FUNCTION
            DayOfWeek _getDayOfWeek()
            {
                // The Hindu day begins with sunrise and continues till
                // next sunrise. The first hora on any day will be the
                // first hour after sunrise and the last hora, the hour
                // before sunrise the next day.

                //TODO NEEDS VERIFICATION

                var sunRise = Calculate.SunriseTime(time);

                // If the current time is after today's sunrise and before tomorrow's sunrise,
                // then it is still considered today.
                var tomorrowSunrise = Calculate.SunriseTime(time.AddHours(24)).GetLmtDateTimeOffset();
                var todaySunrise = sunRise.GetLmtDateTimeOffset();
                var lmtTime = time.GetLmtDateTimeOffset();

                if (lmtTime >= todaySunrise && lmtTime < tomorrowSunrise)
                {
                    //get week day name in string
                    var dayOfWeekNameInString = lmtTime.DayOfWeek.ToString();

                    //convert string to day of week type
                    Enum.TryParse(dayOfWeekNameInString, out DayOfWeek dayOfWeek);

                    return dayOfWeek;
                }
                else
                {
                    //get week day name in string
                    var dayOfWeekNameInString = lmtTime.AddDays(-1).DayOfWeek.ToString();

                    //convert string to day of week type
                    Enum.TryParse(dayOfWeekNameInString, out DayOfWeek dayOfWeek);

                    // If the current time is before today's sunrise, then it is considered the previous day.
                    return dayOfWeek;
                }
            }


        }

        /// <summary>
        /// Gets hora lord based on hora number & week day
        /// </summary>
        public static PlanetName LordOfHoraFromWeekday(int hora, DayOfWeek day)
        {
            switch (day)
            {
                case Library.DayOfWeek.Sunday:
                    switch (hora)
                    {
                        case 1: return Library.PlanetName.Sun;
                        case 2: return Library.PlanetName.Venus;
                        case 3: return Library.PlanetName.Mercury;
                        case 4: return Library.PlanetName.Moon;
                        case 5: return Library.PlanetName.Saturn;
                        case 6: return Library.PlanetName.Jupiter;
                        case 7: return Library.PlanetName.Mars;
                        case 8: return Library.PlanetName.Sun;
                        case 9: return Library.PlanetName.Venus;
                        case 10: return Library.PlanetName.Mercury;
                        case 11: return Library.PlanetName.Moon;
                        case 12: return Library.PlanetName.Saturn;
                        case 13: return Library.PlanetName.Jupiter;
                        case 14: return Library.PlanetName.Mars;
                        case 15: return Library.PlanetName.Sun;
                        case 16: return Library.PlanetName.Venus;
                        case 17: return Library.PlanetName.Mercury;
                        case 18: return Library.PlanetName.Moon;
                        case 19: return Library.PlanetName.Saturn;
                        case 20: return Library.PlanetName.Jupiter;
                        case 21: return Library.PlanetName.Mars;
                        case 22: return Library.PlanetName.Sun;
                        case 23: return Library.PlanetName.Venus;
                        case 24: return Library.PlanetName.Mercury;
                    }
                    break;
                case Library.DayOfWeek.Monday:
                    switch (hora)
                    {
                        case 1: return Library.PlanetName.Moon;
                        case 2: return Library.PlanetName.Saturn;
                        case 3: return Library.PlanetName.Jupiter;
                        case 4: return Library.PlanetName.Mars;
                        case 5: return Library.PlanetName.Sun;
                        case 6: return Library.PlanetName.Venus;
                        case 7: return Library.PlanetName.Mercury;
                        case 8: return Library.PlanetName.Moon;
                        case 9: return Library.PlanetName.Saturn;
                        case 10: return Library.PlanetName.Jupiter;
                        case 11: return Library.PlanetName.Mars;
                        case 12: return Library.PlanetName.Sun;
                        case 13: return Library.PlanetName.Venus;
                        case 14: return Library.PlanetName.Mercury;
                        case 15: return Library.PlanetName.Moon;
                        case 16: return Library.PlanetName.Saturn;
                        case 17: return Library.PlanetName.Jupiter;
                        case 18: return Library.PlanetName.Mars;
                        case 19: return Library.PlanetName.Sun;
                        case 20: return Library.PlanetName.Venus;
                        case 21: return Library.PlanetName.Mercury;
                        case 22: return Library.PlanetName.Moon;
                        case 23: return Library.PlanetName.Saturn;
                        case 24: return Library.PlanetName.Jupiter;
                    }
                    break;
                case Library.DayOfWeek.Tuesday:
                    switch (hora)
                    {
                        case 1: return Library.PlanetName.Mars;
                        case 2: return Library.PlanetName.Sun;
                        case 3: return Library.PlanetName.Venus;
                        case 4: return Library.PlanetName.Mercury;
                        case 5: return Library.PlanetName.Moon;
                        case 6: return Library.PlanetName.Saturn;
                        case 7: return Library.PlanetName.Jupiter;
                        case 8: return Library.PlanetName.Mars;
                        case 9: return Library.PlanetName.Sun;
                        case 10: return Library.PlanetName.Venus;
                        case 11: return Library.PlanetName.Mercury;
                        case 12: return Library.PlanetName.Moon;
                        case 13: return Library.PlanetName.Saturn;
                        case 14: return Library.PlanetName.Jupiter;
                        case 15: return Library.PlanetName.Mars;
                        case 16: return Library.PlanetName.Sun;
                        case 17: return Library.PlanetName.Venus;
                        case 18: return Library.PlanetName.Mercury;
                        case 19: return Library.PlanetName.Moon;
                        case 20: return Library.PlanetName.Saturn;
                        case 21: return Library.PlanetName.Jupiter;
                        case 22: return Library.PlanetName.Mars;
                        case 23: return Library.PlanetName.Sun;
                        case 24: return Library.PlanetName.Venus;
                    }
                    break;
                case Library.DayOfWeek.Wednesday:
                    switch (hora)
                    {
                        case 1: return Library.PlanetName.Mercury;
                        case 2: return Library.PlanetName.Moon;
                        case 3: return Library.PlanetName.Saturn;
                        case 4: return Library.PlanetName.Jupiter;
                        case 5: return Library.PlanetName.Mars;
                        case 6: return Library.PlanetName.Sun;
                        case 7: return Library.PlanetName.Venus;
                        case 8: return Library.PlanetName.Mercury;
                        case 9: return Library.PlanetName.Moon;
                        case 10: return Library.PlanetName.Saturn;
                        case 11: return Library.PlanetName.Jupiter;
                        case 12: return Library.PlanetName.Mars;
                        case 13: return Library.PlanetName.Sun;
                        case 14: return Library.PlanetName.Venus;
                        case 15: return Library.PlanetName.Mercury;
                        case 16: return Library.PlanetName.Moon;
                        case 17: return Library.PlanetName.Saturn;
                        case 18: return Library.PlanetName.Jupiter;
                        case 19: return Library.PlanetName.Mars;
                        case 20: return Library.PlanetName.Sun;
                        case 21: return Library.PlanetName.Venus;
                        case 22: return Library.PlanetName.Mercury;
                        case 23: return Library.PlanetName.Moon;
                        case 24: return Library.PlanetName.Saturn;
                    }
                    break;
                case Library.DayOfWeek.Thursday:
                    switch (hora)
                    {
                        case 1: return Library.PlanetName.Jupiter;
                        case 2: return Library.PlanetName.Mars;
                        case 3: return Library.PlanetName.Sun;
                        case 4: return Library.PlanetName.Venus;
                        case 5: return Library.PlanetName.Mercury;
                        case 6: return Library.PlanetName.Moon;
                        case 7: return Library.PlanetName.Saturn;
                        case 8: return Library.PlanetName.Jupiter;
                        case 9: return Library.PlanetName.Mars;
                        case 10: return Library.PlanetName.Sun;
                        case 11: return Library.PlanetName.Venus;
                        case 12: return Library.PlanetName.Mercury;
                        case 13: return Library.PlanetName.Moon;
                        case 14: return Library.PlanetName.Saturn;
                        case 15: return Library.PlanetName.Jupiter;
                        case 16: return Library.PlanetName.Mars;
                        case 17: return Library.PlanetName.Sun;
                        case 18: return Library.PlanetName.Venus;
                        case 19: return Library.PlanetName.Mercury;
                        case 20: return Library.PlanetName.Moon;
                        case 21: return Library.PlanetName.Saturn;
                        case 22: return Library.PlanetName.Jupiter;
                        case 23: return Library.PlanetName.Mars;
                        case 24: return Library.PlanetName.Sun;
                    }
                    break;
                case Library.DayOfWeek.Friday:
                    switch (hora)
                    {
                        case 1: return Library.PlanetName.Venus;
                        case 2: return Library.PlanetName.Mercury;
                        case 3: return Library.PlanetName.Moon;
                        case 4: return Library.PlanetName.Saturn;
                        case 5: return Library.PlanetName.Jupiter;
                        case 6: return Library.PlanetName.Mars;
                        case 7: return Library.PlanetName.Sun;
                        case 8: return Library.PlanetName.Venus;
                        case 9: return Library.PlanetName.Mercury;
                        case 10: return Library.PlanetName.Moon;
                        case 11: return Library.PlanetName.Saturn;
                        case 12: return Library.PlanetName.Jupiter;
                        case 13: return Library.PlanetName.Mars;
                        case 14: return Library.PlanetName.Sun;
                        case 15: return Library.PlanetName.Venus;
                        case 16: return Library.PlanetName.Mercury;
                        case 17: return Library.PlanetName.Moon;
                        case 18: return Library.PlanetName.Saturn;
                        case 19: return Library.PlanetName.Jupiter;
                        case 20: return Library.PlanetName.Mars;
                        case 21: return Library.PlanetName.Sun;
                        case 22: return Library.PlanetName.Venus;
                        case 23: return Library.PlanetName.Mercury;
                        case 24: return Library.PlanetName.Moon;
                    }
                    break;
                case Library.DayOfWeek.Saturday:
                    switch (hora)
                    {
                        case 1: return Library.PlanetName.Saturn;
                        case 2: return Library.PlanetName.Jupiter;
                        case 3: return Library.PlanetName.Mars;
                        case 4: return Library.PlanetName.Sun;
                        case 5: return Library.PlanetName.Venus;
                        case 6: return Library.PlanetName.Mercury;
                        case 7: return Library.PlanetName.Moon;
                        case 8: return Library.PlanetName.Saturn;
                        case 9: return Library.PlanetName.Jupiter;
                        case 10: return Library.PlanetName.Mars;
                        case 11: return Library.PlanetName.Sun;
                        case 12: return Library.PlanetName.Venus;
                        case 13: return Library.PlanetName.Mercury;
                        case 14: return Library.PlanetName.Moon;
                        case 15: return Library.PlanetName.Saturn;
                        case 16: return Library.PlanetName.Jupiter;
                        case 17: return Library.PlanetName.Mars;
                        case 18: return Library.PlanetName.Sun;
                        case 19: return Library.PlanetName.Venus;
                        case 20: return Library.PlanetName.Mercury;
                        case 21: return Library.PlanetName.Moon;
                        case 22: return Library.PlanetName.Saturn;
                        case 23: return Library.PlanetName.Jupiter;
                        case 24: return Library.PlanetName.Mars;
                    }
                    break;
            }

            throw new Exception("Did not find hora, something wrong!");

        }


        /// <summary>
        /// Each day starts at sunrise and ends at next day's sunrise. This period is
        /// divided into 24 equal parts and they are called horas. A hora is almost equal
        /// to an hour. These horas are ruled by different planets. The lords of hora
        /// come in the order of decreasing speed with respect to earth: Saturn, Jupiter,
        /// Mars, Sun, Venus, Mercury and Moon. After Moon, we go back to Saturn
        /// and repeat the 7 planets.
        /// </summary>
        public static PlanetName LordOfHoraFromTime(Time time)
        {
            //first ascertain the weekday of birth
            var birthWeekday = Calculate.DayOfWeek(time);

            //ascertain the number of hours elapsed from sunrise to birth
            //This shows the number of horas passed.
            var hora = Calculate.HoraAtBirth(time);

            //get lord of hora (hour)
            var lord = Calculate.LordOfHoraFromWeekday(hora, birthWeekday);

            return lord;
        }

        /// <summary>
        /// Gets the junction point (sandhi) between 2 consecutive
        /// houses, where one house begins and the other ends.
        /// </summary>
        public static Angle HouseJunctionPoint(Angle previousHouse, Angle nextHouse)
        {
            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey(nameof(HouseJunctionPoint), previousHouse, nextHouse, Ayanamsa), _getHouseJunctionPoint);


            //UNDERLYING FUNCTION
            Angle _getHouseJunctionPoint()
            {
                //Add the longitudes of two consecutive Bhavas (house)
                //and divide the sum by 2. The result represents sandhi (junction point of houses).

                //get sum of house longitudes
                var longitudeSum = previousHouse + nextHouse;

                Angle junctionPoint;

                //if next house longitude is lower than previous house longitude
                //next house is after 360 degrees
                if (nextHouse < previousHouse)
                {
                    //add 360 to longitude sum
                    longitudeSum = longitudeSum + Angle.Degrees360;

                    //divide sum by 2 to get junction point
                    junctionPoint = longitudeSum.Divide(2);

                    //correct junction point by subtracting 360
                    junctionPoint = junctionPoint - Angle.Degrees360;
                }
                else
                {
                    //divide sum by 2 to get junction point
                    junctionPoint = longitudeSum.Divide(2);
                }

                //return junction point
                return junctionPoint;

            }

        }

        /// <summary>
        /// Gets planet which is the lord of a given sign
        /// </summary>
        public static PlanetName LordOfZodiacSign(ZodiacName signName)
        {
            //handle empty
            if (signName == Library.ZodiacName.Empty) { return Library.PlanetName.Empty; }

            switch (signName)
            {
                //Aries and Scorpio are ruled by Mars;
                case ZodiacName.Aries:
                case ZodiacName.Scorpio:
                    return Library.PlanetName.Mars;

                //Taurus and Libra by Venus;
                case ZodiacName.Taurus:
                case ZodiacName.Libra:
                    return Library.PlanetName.Venus;

                //Gemini and Virgo by Mercury;
                case ZodiacName.Gemini:
                case ZodiacName.Virgo:
                    return Library.PlanetName.Mercury;

                //Cancer by the Moon;
                case ZodiacName.Cancer:
                    return Library.PlanetName.Moon;

                //Leo by the Sun ;
                case ZodiacName.Leo:
                    return Library.PlanetName.Sun;

                //Sagittarius and Pisces by Jupiter
                case ZodiacName.Sagittarius:
                case ZodiacName.Pisces:
                    return Library.PlanetName.Jupiter;

                //Capricorn and Aquarius by Saturn.
                case ZodiacName.Capricorn:
                case ZodiacName.Aquarius:
                    return Library.PlanetName.Saturn;
                default:
                    throw new Exception("Lord of sign not found, error!");
            }
        }

        /// <summary>
        /// Given a planet name will return list of signs that the planet rules
        /// </summary>
        public static List<ZodiacName> ZodiacSignsOwnedByPlanet(PlanetName planetName)
        {
            List<ZodiacName> zodiacNames = new List<ZodiacName>();
            switch (planetName.Name)
            {
                case PlanetNameEnum.Mars:
                    zodiacNames.Add(ZodiacName.Aries);
                    zodiacNames.Add(ZodiacName.Scorpio);
                    break;
                case PlanetNameEnum.Venus:
                    zodiacNames.Add(ZodiacName.Taurus);
                    zodiacNames.Add(ZodiacName.Libra);
                    break;
                case PlanetNameEnum.Mercury:
                    zodiacNames.Add(ZodiacName.Gemini);
                    zodiacNames.Add(ZodiacName.Virgo);
                    break;
                case PlanetNameEnum.Moon:
                    zodiacNames.Add(ZodiacName.Cancer);
                    break;
                case PlanetNameEnum.Sun:
                    zodiacNames.Add(ZodiacName.Leo);
                    break;
                case PlanetNameEnum.Jupiter:
                    zodiacNames.Add(ZodiacName.Sagittarius);
                    zodiacNames.Add(ZodiacName.Pisces);
                    break;
                case PlanetNameEnum.Saturn:
                    zodiacNames.Add(ZodiacName.Capricorn);
                    zodiacNames.Add(ZodiacName.Aquarius);
                    break;
                case PlanetNameEnum.Dhuma:
                    zodiacNames.Add(ZodiacName.Capricorn);
                    break;
                case PlanetNameEnum.Vyatipaata:
                    zodiacNames.Add(ZodiacName.Gemini);
                    break;
                case PlanetNameEnum.Parivesha:
                    zodiacNames.Add(ZodiacName.Sagittarius);
                    break;
                case PlanetNameEnum.Indrachaapa:
                    zodiacNames.Add(ZodiacName.Cancer);
                    break;
                case PlanetNameEnum.Upaketu:
                    zodiacNames.Add(ZodiacName.Cancer);
                    break;
                case PlanetNameEnum.Gulika:
                    zodiacNames.Add(ZodiacName.Aquarius);
                    break;
                case PlanetNameEnum.Yamaghantaka:
                    zodiacNames.Add(ZodiacName.Sagittarius);
                    break;
                case PlanetNameEnum.Arthaprahaara:
                    zodiacNames.Add(ZodiacName.Gemini);
                    break;
                case PlanetNameEnum.Kaala:
                    zodiacNames.Add(ZodiacName.Capricorn);
                    break;
                case PlanetNameEnum.Mrityu:
                    zodiacNames.Add(ZodiacName.Scorpio);
                    break;
                default:
                    zodiacNames.Add(ZodiacName.Empty);
                    break;
            }
            return zodiacNames;
        }

        /// <summary>
        /// Gets next zodiac sign after input sign
        /// </summary>
        public static ZodiacName NextZodiacSign(ZodiacName inputSign)
        {

            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey(nameof(NextZodiacSign), inputSign, Ayanamsa), _getNextZodiacSign);


            //UNDERLYING FUNCTION
            ZodiacName _getNextZodiacSign()
            {
                //get number of of input zodiac
                int inputSignNumber = (int)inputSign;

                int nextSignNumber;

                //after pieces (12) is Aries (1)
                if (inputSignNumber == 12)
                {
                    nextSignNumber = 1;
                }
                else
                {
                    //else next sign is input sign plus 1
                    nextSignNumber = inputSignNumber + 1;
                }

                //convert next sign number to its zodiac name
                var nextSignName = (ZodiacName)nextSignNumber;

                return nextSignName;

            }
        }

        /// <summary>
        /// Gets next house number after input house number, goes to  1 after 12
        /// </summary>
        public static int NextHouseNumber(int inputHouseNumber)
        {

            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey(nameof(NextHouseNumber), inputHouseNumber, Ayanamsa), _getNextHouseNumber);


            //UNDERLYING FUNCTION
            int _getNextHouseNumber()
            {
                int nextHouseNumber;

                //if input house number is 12
                if (inputHouseNumber == 12)
                {
                    //next house number is 1
                    nextHouseNumber = 1;

                }
                else
                {
                    //else next house number is input number + 1
                    nextHouseNumber = inputHouseNumber + 1;
                }


                return nextHouseNumber;

            }

        }

        /// <summary>
        /// Gets the exact longitude where planet is Exalted/Exaltation
        /// Exaltation
        /// Each planet is held to be exalted when it is
        /// in a particular sign. The power to do good when in
        /// exaltation is greater than when in its own sign.
        /// Throughout the sign ascribed, the planet is exalted
        /// but in a particular degree its exaltation is at the maximum level.
        /// 
        /// NOTE:
        /// - For Upagrahas no exact degree for exaltation the whole
        /// sign is counted as such exalatiotn set at degree 1
        /// 
        /// - Rahu & ketu have exaltation points ref : Astroloy for Beginners pg. 12
        /// </summary>
        public static ZodiacSign PlanetExaltationPoint(PlanetName planetName)
        {

            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey(nameof(PlanetExaltationPoint), planetName, Ayanamsa), _getPlanetExaltationPoint);


            //UNDERLYING FUNCTION
            ZodiacSign _getPlanetExaltationPoint()
            {
                //Sun in the 10th degree of Aries;
                if (planetName == Library.PlanetName.Sun)
                {
                    return new ZodiacSign(ZodiacName.Aries, Angle.FromDegrees(10));
                }

                // Moon 3rd of Taurus;
                else if (planetName == Library.PlanetName.Moon)
                {
                    return new ZodiacSign(ZodiacName.Taurus, Angle.FromDegrees(3));
                }

                // Mars 28th of Capricorn ;
                else if (planetName == Library.PlanetName.Mars)
                {
                    return new ZodiacSign(ZodiacName.Capricorn, Angle.FromDegrees(28));
                }

                // Mercury 15th of Virgo;
                else if (planetName == Library.PlanetName.Mercury)
                {
                    return new ZodiacSign(ZodiacName.Virgo, Angle.FromDegrees(15));
                }

                // Jupiter 5th of Cancer;
                else if (planetName == Library.PlanetName.Jupiter)
                {
                    return new ZodiacSign(ZodiacName.Cancer, Angle.FromDegrees(5));
                }

                // Venus 27th of Pisces and
                else if (planetName == Library.PlanetName.Venus)
                {
                    return new ZodiacSign(ZodiacName.Pisces, Angle.FromDegrees(27));
                }

                // Saturn 20th of Libra.
                else if (planetName == Library.PlanetName.Saturn)
                {
                    return new ZodiacSign(ZodiacName.Libra, Angle.FromDegrees(20));
                }

                // Rahu 20th of Taurus.
                else if (planetName == Library.PlanetName.Rahu)
                {
                    return new ZodiacSign(ZodiacName.Taurus, Angle.FromDegrees(20));
                }

                // Ketu 20th of Scorpio.
                else if (planetName == Library.PlanetName.Ketu)
                {
                    return new ZodiacSign(ZodiacName.Scorpio, Angle.FromDegrees(20));
                }

                //NOTE: Upagrahas exalatation whole sign, artificial set degree 1
                else if (planetName == Library.PlanetName.Dhuma)
                {
                    return new ZodiacSign(ZodiacName.Leo, Angle.FromDegrees(1));
                }
                else if (planetName == Library.PlanetName.Vyatipaata)
                {
                    return new ZodiacSign(ZodiacName.Scorpio, Angle.FromDegrees(1));
                }
                else if (planetName == Library.PlanetName.Parivesha)
                {
                    return new ZodiacSign(ZodiacName.Gemini, Angle.FromDegrees(1));
                }
                else if (planetName == Library.PlanetName.Indrachaapa)
                {
                    return new ZodiacSign(ZodiacName.Sagittarius, Angle.FromDegrees(1));
                }
                else if (planetName == Library.PlanetName.Upaketu)
                {
                    return new ZodiacSign(ZodiacName.Aquarius, Angle.FromDegrees(1));
                }

                throw new Exception("Planet exaltation point not found, error!");

            }

        }

        /// <summary>
        /// Gets the exact sign longitude where planet is Debilitated/Debility
        /// TODO method needs testing!
        /// Note:
        /// - Rahu & ketu have debilitation points ref : Astroloy for Beginners pg. 12
        /// - "planet to sign relationship" is the whole sign, this is just a point
        /// - The 7th house or the 180th degree from the place of exaltation is the
        ///   place of debilitation or fall. The Sun is debilitated-
        ///   in the 10th degree of Libra, the Moon 3rd
        ///   of Scorpio and so on.
        /// - For Upagrahas no exact degree for exaltation the whole
        ///   sign is counted as such exalatiotn set at degree 1
        /// - The debilitation or depression points are found
        ///   by adding 180° to the maximum points given above.
        ///   While in a state of fall, planets give results contrary
        ///   to those when in exaltation. ref : Astroloy for Beginners pg. 11
        /// </summary>
        public static ZodiacSign PlanetDebilitationPoint(PlanetName planetName)
        {

            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey(nameof(PlanetDebilitationPoint), planetName, Ayanamsa), _getPlanetDebilitationPoint);


            //UNDERLYING FUNCTION
            ZodiacSign _getPlanetDebilitationPoint()
            {
                //The 7th house or the
                // 180th degree from the place of exaltation is the
                // place of debilitation or fall. The Sun is debilitated-
                // in the 10th degree of Libra, the Moon 3rd
                // of Scorpio and so on.

                //Sun in the 10th degree of Libra;
                if (planetName == Library.PlanetName.Sun)
                {
                    return new ZodiacSign(ZodiacName.Libra, Angle.FromDegrees(10));
                }

                // Moon 0 of Scorpio
                else if (planetName == Library.PlanetName.Moon)
                {
                    //TODO check if 0 degrees exist
                    return new ZodiacSign(ZodiacName.Scorpio, Angle.FromDegrees(0));
                }

                // Mars 28th of Cancer ;
                else if (planetName == Library.PlanetName.Mars)
                {
                    return new ZodiacSign(ZodiacName.Cancer, Angle.FromDegrees(28));
                }

                // Mercury 15th of Pisces;
                else if (planetName == Library.PlanetName.Mercury)
                {
                    return new ZodiacSign(ZodiacName.Pisces, Angle.FromDegrees(15));
                }

                // Jupiter 5th of Capricorn;
                else if (planetName == Library.PlanetName.Jupiter)
                {
                    return new ZodiacSign(ZodiacName.Capricorn, Angle.FromDegrees(5));
                }

                // Venus 27th of Virgo and
                else if (planetName == Library.PlanetName.Venus)
                {
                    return new ZodiacSign(ZodiacName.Virgo, Angle.FromDegrees(27));
                }

                // Saturn 20th of Aries.
                else if (planetName == Library.PlanetName.Saturn)
                {
                    return new ZodiacSign(ZodiacName.Aries, Angle.FromDegrees(20));
                }

                // Rahu 20th of Scorpio.
                else if (planetName == Library.PlanetName.Rahu)
                {
                    return new ZodiacSign(ZodiacName.Scorpio, Angle.FromDegrees(20));
                }

                // Ketu 20th of Taurus.
                else if (planetName == Library.PlanetName.Ketu)
                {
                    return new ZodiacSign(ZodiacName.Taurus, Angle.FromDegrees(20));
                }

                //NOTE: Upagrahas Debilitation whole sign, artificial set degree 1
                else if (planetName == Library.PlanetName.Dhuma)
                {
                    return new ZodiacSign(ZodiacName.Aquarius, Angle.FromDegrees(1));
                }
                else if (planetName == Library.PlanetName.Vyatipaata)
                {
                    return new ZodiacSign(ZodiacName.Taurus, Angle.FromDegrees(1));
                }
                else if (planetName == Library.PlanetName.Parivesha)
                {
                    return new ZodiacSign(ZodiacName.Sagittarius, Angle.FromDegrees(1));
                }
                else if (planetName == Library.PlanetName.Indrachaapa)
                {
                    return new ZodiacSign(ZodiacName.Gemini, Angle.FromDegrees(1));
                }
                else if (planetName == Library.PlanetName.Upaketu)
                {
                    return new ZodiacSign(ZodiacName.Leo, Angle.FromDegrees(1));
                }


                throw new Exception("Planet debilitation point not found, error!");

            }


        }


        #region SIGN GROUP CALULATORS

        /// <summary>
        /// Returns true if zodiac sign is an Even sign,  Yugma Rasis
        /// </summary>
        public static bool IsEvenSign(ZodiacName planetSignName)
        {

            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey(nameof(IsEvenSign), planetSignName, Ayanamsa), _isEvenSign);


            //UNDERLYING FUNCTION
            bool _isEvenSign()
            {
                if (planetSignName == ZodiacName.Taurus || planetSignName == ZodiacName.Cancer || planetSignName == ZodiacName.Virgo ||
                    planetSignName == ZodiacName.Scorpio || planetSignName == ZodiacName.Capricorn || planetSignName == ZodiacName.Pisces)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }

        }

        /// <summary>
        /// Returns true if zodiac sign is an Odd sign, Oja Rasis
        /// </summary>
        public static bool IsOddSign(ZodiacName planetSignName)
        {
            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey(nameof(IsOddSign), planetSignName, Ayanamsa), _isOddSign);


            //UNDERLYING FUNCTION
            bool _isOddSign()
            {
                if (planetSignName == ZodiacName.Aries || planetSignName == ZodiacName.Gemini || planetSignName == ZodiacName.Leo ||
                    planetSignName == ZodiacName.Libra || planetSignName == ZodiacName.Sagittarius || planetSignName == ZodiacName.Aquarius)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }


        }

        /// <summary>
        /// Fixed signs- Taurus, Leo, Scropio, Aquarius.
        /// </summary>
        public static bool IsFixedSign(ZodiacName sunSign)
        {
            switch (sunSign)
            {
                case ZodiacName.Taurus:
                case ZodiacName.Leo:
                case ZodiacName.Scorpio:
                case ZodiacName.Aquarius:
                    return true;
                default:
                    return false;
            }

        }

        /// <summary>
        /// Movable signs- Aries, Cancer, Libra, Capricorn.
        /// </summary>
        public static bool IsMovableSign(ZodiacName sunSign)
        {
            switch (sunSign)
            {
                case ZodiacName.Aries:
                case ZodiacName.Cancer:
                case ZodiacName.Libra:
                case ZodiacName.Capricorn:
                    return true;
                default:
                    return false;
            }

        }

        /// <summary>
        /// Common signs- Gemini, Virgo, Sagitarius, Pisces.
        /// </summary>
        public static bool IsCommonSign(ZodiacName sunSign)
        {
            switch (sunSign)
            {
                case ZodiacName.Gemini:
                case ZodiacName.Virgo:
                case ZodiacName.Sagittarius:
                case ZodiacName.Pisces:
                    return true;
                default:
                    return false;
            }

        }


        #endregion

        /// <summary>
        /// Gets a planets permenant relationship.
        /// Based on : Hindu Predictive Astrology, pg. 21
        /// Note:
        /// - Rahu & Ketu are not mentioned in any permenant relatioship by Raman.
        ///   But some websites do mention this. As such Raman's take is taken as final.
        ///   Since there's so far no explanation by Raman on Rahu & Ketu permenant relation it
        ///   is assumed that such relationship is not needed and to make them up for conveniece sake
        ///   could result in wrong prediction down the line.
        ///   But temporary relationship are mentioned by Raman for Rahu & Ketu, so explicitly use
        ///   Temperary relationship where needed.
        /// </summary>
        public static PlanetToPlanetRelationship PlanetPermanentRelationshipWithPlanet(PlanetName mainPlanet, PlanetName secondaryPlanet)
        {

            //no calculation for rahu and ketu here
            var isRahu = mainPlanet.Name == Library.PlanetName.PlanetNameEnum.Rahu;
            var isKetu = mainPlanet.Name == Library.PlanetName.PlanetNameEnum.Ketu;
            var isRahu2 = secondaryPlanet.Name == Library.PlanetName.PlanetNameEnum.Rahu;
            var isKetu2 = secondaryPlanet.Name == Library.PlanetName.PlanetNameEnum.Ketu;
            var isRahuKetu = isRahu || isKetu || isRahu2 || isKetu2;
            if (isRahuKetu) { return PlanetToPlanetRelationship.Empty; }



            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey(nameof(PlanetPermanentRelationshipWithPlanet), mainPlanet, secondaryPlanet, Ayanamsa), _getPlanetPermanentRelationshipWithPlanet);


            //UNDERLYING FUNCTION
            PlanetToPlanetRelationship _getPlanetPermanentRelationshipWithPlanet()
            {
                //if main planet & secondary planet is same, then it is own plant (same planet), end here
                if (mainPlanet == secondaryPlanet) { return PlanetToPlanetRelationship.SamePlanet; }


                bool planetInEnemies = false;
                bool planetInNeutrals = false;
                bool planetInFriends = false;


                //if main planet is sun
                if (mainPlanet == Library.PlanetName.Sun)
                {
                    //List planets friends, neutrals & enemies
                    var sunFriends = new List<PlanetName>() { Library.PlanetName.Moon, Library.PlanetName.Mars, Library.PlanetName.Jupiter };
                    var sunNeutrals = new List<PlanetName>() { Library.PlanetName.Mercury };
                    var sunEnemies = new List<PlanetName>() { Library.PlanetName.Venus, Library.PlanetName.Saturn };

                    //check if planet is found in any of the lists
                    planetInFriends = sunFriends.Contains(secondaryPlanet);
                    planetInNeutrals = sunNeutrals.Contains(secondaryPlanet);
                    planetInEnemies = sunEnemies.Contains(secondaryPlanet);
                }

                //if main planet is moon
                if (mainPlanet == Library.PlanetName.Moon)
                {
                    //List planets friends, neutrals & enemies
                    var moonFriends = new List<PlanetName>() { Library.PlanetName.Sun, Library.PlanetName.Mercury };
                    var moonNeutrals = new List<PlanetName>() { Library.PlanetName.Mars, Library.PlanetName.Jupiter, Library.PlanetName.Venus, Library.PlanetName.Saturn };
                    var moonEnemies = new List<PlanetName>() { };

                    //check if planet is found in any of the lists
                    planetInFriends = moonFriends.Contains(secondaryPlanet);
                    planetInNeutrals = moonNeutrals.Contains(secondaryPlanet);
                    planetInEnemies = moonEnemies.Contains(secondaryPlanet);

                }

                //if main planet is mars
                if (mainPlanet == Library.PlanetName.Mars)
                {
                    //List planets friends, neutrals & enemies
                    var marsFriends = new List<PlanetName>() { Library.PlanetName.Sun, Library.PlanetName.Moon, Library.PlanetName.Jupiter };
                    var marsNeutrals = new List<PlanetName>() { Library.PlanetName.Venus, Library.PlanetName.Saturn };
                    var marsEnemies = new List<PlanetName>() { Library.PlanetName.Mercury };

                    //check if planet is found in any of the lists
                    planetInFriends = marsFriends.Contains(secondaryPlanet);
                    planetInNeutrals = marsNeutrals.Contains(secondaryPlanet);
                    planetInEnemies = marsEnemies.Contains(secondaryPlanet);

                }

                //if main planet is mercury
                if (mainPlanet == Library.PlanetName.Mercury)
                {
                    //List planets friends, neutrals & enemies
                    var mercuryFriends = new List<PlanetName>() { Library.PlanetName.Sun, Library.PlanetName.Venus };
                    var mercuryNeutrals = new List<PlanetName>() { Library.PlanetName.Mars, Library.PlanetName.Jupiter, Library.PlanetName.Saturn };
                    var mercuryEnemies = new List<PlanetName>() { Library.PlanetName.Moon };

                    //check if planet is found in any of the lists
                    planetInFriends = mercuryFriends.Contains(secondaryPlanet);
                    planetInNeutrals = mercuryNeutrals.Contains(secondaryPlanet);
                    planetInEnemies = mercuryEnemies.Contains(secondaryPlanet);

                }

                //if main planet is jupiter
                if (mainPlanet == Library.PlanetName.Jupiter)
                {
                    //List planets friends, neutrals & enemies
                    var jupiterFriends = new List<PlanetName>() { Library.PlanetName.Sun, Library.PlanetName.Moon, Library.PlanetName.Mars };
                    var jupiterNeutrals = new List<PlanetName>() { Library.PlanetName.Saturn };
                    var jupiterEnemies = new List<PlanetName>() { Library.PlanetName.Mercury, Library.PlanetName.Venus };

                    //check if planet is found in any of the lists
                    planetInFriends = jupiterFriends.Contains(secondaryPlanet);
                    planetInNeutrals = jupiterNeutrals.Contains(secondaryPlanet);
                    planetInEnemies = jupiterEnemies.Contains(secondaryPlanet);

                }

                //if main planet is venus
                if (mainPlanet == Library.PlanetName.Venus)
                {
                    //List planets friends, neutrals & enemies
                    var venusFriends = new List<PlanetName>() { Library.PlanetName.Mercury, Library.PlanetName.Saturn };
                    var venusNeutrals = new List<PlanetName>() { Library.PlanetName.Mars, Library.PlanetName.Jupiter };
                    var venusEnemies = new List<PlanetName>() { Library.PlanetName.Sun, Library.PlanetName.Moon };

                    //check if planet is found in any of the lists
                    planetInFriends = venusFriends.Contains(secondaryPlanet);
                    planetInNeutrals = venusNeutrals.Contains(secondaryPlanet);
                    planetInEnemies = venusEnemies.Contains(secondaryPlanet);

                }

                //if main planet is saturn
                if (mainPlanet == Library.PlanetName.Saturn)
                {
                    //List planets friends, neutrals & enemies
                    var saturnFriends = new List<PlanetName>() { Library.PlanetName.Mercury, Library.PlanetName.Venus };
                    var saturnNeutrals = new List<PlanetName>() { Library.PlanetName.Jupiter };
                    var saturnEnemies = new List<PlanetName>() { Library.PlanetName.Sun, Library.PlanetName.Moon, Library.PlanetName.Mars };

                    //check if planet is found in any of the lists
                    planetInFriends = saturnFriends.Contains(secondaryPlanet);
                    planetInNeutrals = saturnNeutrals.Contains(secondaryPlanet);
                    planetInEnemies = saturnEnemies.Contains(secondaryPlanet);

                }

                //for Rahu & Ketu special exception
                if (mainPlanet == Library.PlanetName.Rahu || mainPlanet == Library.PlanetName.Ketu)
                {
                    throw new Exception("No Permenant Relation for Rahu and Ketu, use Temporary Relation!");
                }




                //return planet relationship based on where planet is found
                if (planetInFriends)
                {
                    return PlanetToPlanetRelationship.Friend;
                }
                if (planetInNeutrals)
                {
                    return PlanetToPlanetRelationship.Neutral;
                }
                if (planetInEnemies)
                {
                    return PlanetToPlanetRelationship.Enemy;
                }

                return PlanetToPlanetRelationship.Empty;
                throw new Exception("planet permanent relationship not found, error!");

            }

        }

        /// <summary>
        /// Converts julian time to normal time, normal time can be lmt, lat, utc
        /// </summary>
        public static DateTime ConvertJulianTimeToNormalTime(double julianTime)
        {

            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey(nameof(ConvertJulianTimeToNormalTime), julianTime, Ayanamsa), _convertJulianTimeToNormalTime);


            //UNDERLYING FUNCTION
            DateTime _convertJulianTimeToNormalTime()
            {
                //initialize ephemeris
                SwissEph ephemeris = new SwissEph();

                //set calender type
                int gregflag = SwissEph.SE_GREG_CAL; //GREGORIAN CALENDAR

                //julian time to normal time
                int year = 0, month = 0, day = 0, hour = 0, min = 0;
                double sec = 0;

                // convert julian time to normal time
                ephemeris.swe_jdut1_to_utc(julianTime, gregflag, ref year, ref month, ref day, ref hour, ref min, ref sec);

                //put pieces of time into one type
                var normalUtcTime = new DateTime(year, month, day, hour, min, (int)sec);

                return normalUtcTime;

            }


        }

        /// <summary>
        /// Gets Greenwich time in normal format from Julian days at Greenwich
        /// Note : Inputed time is Julian days at greenwich, callers reponsibility to make sure 
        /// </summary>
        public static DateTimeOffset GreenwichTimeFromJulianDays(double julianTime)
        {

            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey(nameof(GreenwichTimeFromJulianDays), julianTime, Ayanamsa), _convertJulianTimeToNormalTime);


            //UNDERLYING FUNCTION
            DateTimeOffset _convertJulianTimeToNormalTime()
            {
                //initialize ephemeris
                SwissEph ephemeris = new();

                //set calender type
                int gregflag = SwissEph.SE_GREG_CAL; //GREGORIAN CALENDAR

                //prepare a place to receive the time in normal format 
                int year = 0, month = 0, day = 0, hour = 0, min = 0;
                double sec = 0;

                //convert julian time to normal time
                ephemeris.swe_jdut1_to_utc(julianTime, gregflag, ref year, ref month, ref day, ref hour, ref min, ref sec);

                //put pieces of time into one type
                var normalUtcTime = new DateTime(year, month, day, hour, min, (int)sec);

                //set the correct offset (Greenwich = UTC = +0:00)
                var offsetTime = new DateTimeOffset(normalUtcTime, new TimeSpan(0, 0, 0));

                return offsetTime;
            }


        }

        /// <summary>
        /// Gets Local mean time (LMT) at Greenwich (UTC) in Julian days based on the inputed time
        /// </summary>
        public static double GreenwichLmtInJulianDays(Time time)
        {

            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey(nameof(GreenwichLmtInJulianDays), time, Ayanamsa), _getGreenwichLmtInJulianDays);


            //UNDERLYING FUNCTION
            double _getGreenwichLmtInJulianDays()
            {
                //get LMT time at Greenwich (UTC)
                DateTimeOffset lmtDateTime = time.GetLmtDateTimeOffset().ToUniversalTime();

                //split lmt time to pieces
                int year = lmtDateTime.Year;
                int month = lmtDateTime.Month;
                int day = lmtDateTime.Day;
                double hour = (lmtDateTime.TimeOfDay).TotalHours;

                //set calender type
                int gregflag = SwissEph.SE_GREG_CAL; //GREGORIAN CALENDAR

                //declare output variables
                double localMeanTimeInJulian_UT;

                //initialize ephemeris
                SwissEph ephemeris = new();

                //get lmt in julian day in Universal Time (UT)
                localMeanTimeInJulian_UT = ephemeris.swe_julday(year, month, day, hour, gregflag);//time to Julian Day

                return localMeanTimeInJulian_UT;

            }

        }

        /// <summary>
        /// Gets the longitude of house 1 and house 10
        /// using Swiss Epehemris swe_houses
        /// </summary>
        public static double[] GetHouse1And10Longitudes(Time time)
        {
            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey(nameof(GetHouse1And10Longitudes), time, Ayanamsa), _getHouse1And10Longitudes);


            //UNDERLYING FUNCTION
            double[] _getHouse1And10Longitudes()
            {
                //get location at place of time
                var location = time.GetGeoLocation();

                //Convert DOB to Julian Day
                var jul_day_UT = TimeToJulianDay(time);

                SwissEph swissEph = new SwissEph();

                double[] cusps = new double[13];

                //we have to supply ascmc to make the function run
                double[] ascmc = new double[10];

                //set ayanamsa
                swissEph.swe_set_sid_mode(Ayanamsa, 0, 0);

                //NOTE:
                //if you use P which is Placidus there is a high chances you will get unequal houses from the SwissEph library itself...
                // you have to use V - 'V'Vehlow equal (Asc. in middle of house 1)
                swissEph.swe_houses(jul_day_UT, location.Latitude(), location.Longitude(), 'V', cusps, ascmc);

                //we only return cusps, cause that is what is used for now
                return cusps;
            }

        }


        /// <summary>
        /// Converts Local Mean Time (LMT) to Universal Time (UTC)
        /// </summary>
        public static DateTimeOffset LmtToUtc(Time time)
        {
            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey(nameof(LmtToUtc), time, Ayanamsa), _lmtToUtc);


            //UNDERLYING FUNCTION
            DateTimeOffset _lmtToUtc()
            {
                return time.GetLmtDateTimeOffset().ToUniversalTime();
            }
        }

        #endregion

        #region ASHTAKVARGA

        /// <summary>
        /// When the benefic points contributed by each planet in Bhinnashtakavargas
        /// different signs are added, we get a Sarvashtakavarga.
        /// A total of 337 benefic points are contributed, by the seven planets,
        /// to various houses in relation to seven planets and the lagna.
        /// </summary>
        public static Sarvashtakavarga SarvashtakavargaChart(Time birthTime)
        {
            return Ashtakavarga.SarvashtakavargaChart2(birthTime);
        }

        /// <summary>
        /// Seven different charts are thus possible for the seven different
        /// planets. These are called as Bhinnashtakavargas. The position
        /// of each planet in the natal chart is of primary consideration. 
        /// </summary>
        public static Bhinnashtakavarga BhinnashtakavargaChart(Time birthTime)
        {
            var returnVal = new Bhinnashtakavarga();

            foreach (var planet in PlanetName.All7Planets)
            {
                //use prastaraka to calculate bhinnashtaka and add it compiled list
                var planetPrastaraka = Ashtakavarga.PlanetPrastaraka(planet, birthTime);
                returnVal[planet] = planetPrastaraka.BhinnashtakaRow();
            }

            return returnVal;
        }

        /// <summary>
        /// Give a planet and sign and ashtakvarga bindu can be calculated
        ///
        /// EXP : In the Sun's own Ashtakvarga, there are 5 bindus in Aries
        /// 
        /// NOTE ON USE: Ashtakvarga System pg.128 
        /// For example, in the Standard Horoscope,
        /// the Sun's transit of Aries (3rd from Moon) should
        /// prove favorable. In the Sun's own Ashtakvarga,
        /// there are 5 bindus in Aries. Therefore the
        /// good effects produced should be to the extent
        /// of 62%. The Sun's transit of Capricorn
        /// (12th from the Moon) should prove adverse.
        /// Capricorn, has no bindus.Therefore the evil results
        /// to be produced by this transit are to the brim.
        /// </summary>
        public static int PlanetAshtakvargaBindu(PlanetName planet, ZodiacName signToCheck, Time time)
        {
            //calculates ashtakvarga for given planet 
            var bhinnashtakavargaChart = Ashtakavarga.BhinnashtakavargaChartForPlanet(planet, time);

            //get bindu score for given sign
            var bindu = bhinnashtakavargaChart[signToCheck];

            return bindu;
        }

        /// <summary>
        /// Kakshyas for daily use : The concept of Kakshyas can be
        /// employed for daily use. The method of this application is simple.
        /// Prepare the Prastaraka charts for the seven planets. Then find
        /// out the longitudes of each of the seven planets on a given day.
        /// In the Prastaraka of the Sun, see if the transiting Sun is passing
        /// through a Kakshya with a benefic point. For the Moon's transit,
        /// consider the Prastaraka of the Moon. See for all the planets.
        /// When several planets are transiting the Kakshyas where the natal
        /// planets have contributed benefic points, that day is auspicious.
        /// When several planets transit the Kakshyas where there are no
        /// benefic points, it is adverse time for the native
        /// 
        /// The Concept of Kakshya
        /// The Prastaraka charts for different planets can be represented
        /// in a different manner to make use of the concept of Kakshyas.
        /// Each rashi or sign is divided into eight equal parts or Kakshyas
        /// The Prastaraka chart for each planet can thus be readjusted
        /// to bring in the concept of the Kakshyas.
        /// A planet is considered to be productive ofbenefic
        /// results when it transits a Kakshya where there is a benefic point
        /// </summary>
        public static GocharaKakshas GocharaKakshas(Time checkTime, Time birthTime)
        {
            //first is column of name planets
            var column1 = PlanetName.All7Planets;

            //2nd column is signs the planet is currently in
            var column2 = new Dictionary<PlanetName, ZodiacSign>();
            //add in each planet and the sign it is in at check time
            foreach (var planetName in column1) { column2.Add(planetName, Calculate.PlanetZodiacSign(planetName, checkTime)); }


            //3rd column is planet which is ruling the current planet
            //based on current zodiac sign determine the ruling planet
            var column3 = new Dictionary<PlanetName, string>();
            foreach (var currentZodiacSign in column2) { column3.Add(currentZodiacSign.Key, GetKakshyaLord(currentZodiacSign.Value)); }

            //NOTE : where current time is linked to birth time
            //4th column, score of 1 or 0 from Prastaraka 
            var column4 = new Dictionary<PlanetName, int>();
            foreach (var mainPlanet in column1) //can be acendant
            {
                var planetPrastaraka = Ashtakavarga.PlanetPrastaraka(mainPlanet, birthTime);
                //narrow down to planet which ruling current planet
                var rullingPlanet = column3[mainPlanet]; //includes Ascendant
                var allSigns = planetPrastaraka[rullingPlanet];

                //get specific score at current transiting sign
                var checkTimeSign = column2[mainPlanet];
                var score = allSigns[checkTimeSign.GetSignName()];
                column4.Add(mainPlanet, score);
            }

            //5th column add points from Prastaraka
            var column5 = new Dictionary<PlanetName, int>();
            foreach (var mainPlanet in column1) //can be acendant
            {
                var planetPrastaraka = Ashtakavarga.PlanetPrastaraka(mainPlanet, birthTime);
                //get score of compiled Prastaraka for all signs
                var bhinnashtakaRow = planetPrastaraka.BhinnashtakaRow();

                //get specific score at current transiting sign
                var checkTimeSign = column2[mainPlanet];
                var score = bhinnashtakaRow[checkTimeSign.GetSignName()];
                column5.Add(mainPlanet, score);
            }

            //6th column add points from Sarvashtaka 
            var column6 = new Dictionary<PlanetName, int>();
            foreach (var mainPlanet in column1) //can be acendant
            {
                //get Sarvashtaka for all signs
                var allSigns = SarvashtakavargaChart(birthTime);

                //get specific score at current transiting sign
                var checkTimeSign = column2[mainPlanet];

                //get column with added points
                var score = allSigns.SarvashtakavargaRow[checkTimeSign.GetSignName()];

                column6.Add(mainPlanet, score);
            }

            //pack the data to be oupted various formats even JPEG! yeah!
            var finalData = new GocharaKakshas(column1, column2, column3, column4, column5, column6);
            return finalData;

            //based on table data
            string GetKakshyaLord(ZodiacSign inputZodiacSign)
            {
                var degreeInSign = inputZodiacSign.GetDegreesInSign().TotalDegrees;

                if (degreeInSign >= 0 && degreeInSign <= 3.75) { return "Saturn"; }
                else if (degreeInSign > 3.75 && degreeInSign <= 7.5) { return "Jupiter"; }
                else if (degreeInSign > 7.5 && degreeInSign <= 11.25) { return "Mars"; }
                else if (degreeInSign > 11.25 && degreeInSign <= 15.00) { return "Sun"; }
                else if (degreeInSign > 15.00 && degreeInSign <= 18.75) { return "Venus"; }
                else if (degreeInSign > 18.75 && degreeInSign <= 22.5) { return "Mercury"; }
                else if (degreeInSign > 22.5 && degreeInSign <= 26.25) { return "Moon"; }
                else if (degreeInSign > 26.25 && degreeInSign <= 30.0) { return "Ascendant"; }

                throw new Exception("END OF LINE");
            }
        }


        #endregion

        #region GOCHARA

        /// <summary>
        /// Gets the Gochara House number which is the count from birth Moon sign (janma rasi)
        /// to the sign the planet is at the current time. Gochara == Transits
        /// </summary>
        public static int GocharaZodiacSignCountFromMoon(Time birthTime, Time currentTime, PlanetName planet)
        {
            //get moon sign at birth (janma rasi)
            var janmaSign = Calculate.MoonSignName(birthTime);

            //get planet sign at input time
            var planetSign = Calculate.PlanetZodiacSign(planet, currentTime).GetSignName();

            //count from janma to sign planet is in
            var count = Calculate.CountFromSignToSign(janmaSign, planetSign);

            return count;
        }

        /// <summary>
        /// Check if there is an obstruction to a given Gochara, obstructing house/point (Vedhanka)
        /// </summary>
        public static bool IsGocharaObstructed(PlanetName planet, int gocharaHouse, Time birthTime, Time currentTime)
        {
            //get the obstructing house/point (Vedhanka) for the inputed Gochara house
            var vedhanka = Vedhanka(planet, gocharaHouse);

            //if vedhanka is 0, then end here as no obstruction
            if (vedhanka == 0) { return false; }

            //get all the planets transiting (gochara) in this obstruction point/house (vedhanka)
            var planetList = PlanetsInGocharaHouse(birthTime, currentTime, gocharaHouse);

            //remove the exception planets
            //No Vedha occurs between the Sun and Saturn, and the Moon and Mercury.
            if (planet == Library.PlanetName.Sun || planet == Library.PlanetName.Saturn)
            {
                planetList.Remove(Library.PlanetName.Sun);
                planetList.Remove(Library.PlanetName.Saturn);
            }
            if (planet == Library.PlanetName.Moon || planet == Mercury)
            {
                planetList.Remove(Library.PlanetName.Moon);
                planetList.Remove(Library.PlanetName.Mercury);
            }

            //now if any planet is found in the list, than obstruction is present
            return planetList.Any();

        }

        /// <summary>
        /// Gets all the planets in a given Gochara House
        /// 
        /// Note : Gochara House number is the count from birth Moon sign (janma rasi)
        /// to the sign the planet is at the current time. Gochara == Transits
        /// </summary>
        public static List<PlanetName> PlanetsInGocharaHouse(Time birthTime, Time currentTime, int gocharaHouse)
        {
            //get the gochara house for every planet at current time
            var gocharaSun = GocharaZodiacSignCountFromMoon(birthTime, currentTime, Library.PlanetName.Sun);
            var gocharaMoon = GocharaZodiacSignCountFromMoon(birthTime, currentTime, Library.PlanetName.Moon);
            var gocharaMars = GocharaZodiacSignCountFromMoon(birthTime, currentTime, Library.PlanetName.Mars);
            var gocharaMercury = GocharaZodiacSignCountFromMoon(birthTime, currentTime, Library.PlanetName.Mercury);
            var gocharaJupiter = GocharaZodiacSignCountFromMoon(birthTime, currentTime, Library.PlanetName.Jupiter);
            var gocharaVenus = GocharaZodiacSignCountFromMoon(birthTime, currentTime, Library.PlanetName.Venus);
            var gocharaSaturn = GocharaZodiacSignCountFromMoon(birthTime, currentTime, Library.PlanetName.Saturn);

            //add every planet name to return list that matches input Gochara house number
            var planetList = new List<PlanetName>();
            if (gocharaSun == gocharaHouse) { planetList.Add(Library.PlanetName.Sun); }
            if (gocharaMoon == gocharaHouse) { planetList.Add(Library.PlanetName.Moon); }
            if (gocharaMars == gocharaHouse) { planetList.Add(Library.PlanetName.Mars); }
            if (gocharaMercury == gocharaHouse) { planetList.Add(Library.PlanetName.Mercury); }
            if (gocharaJupiter == gocharaHouse) { planetList.Add(Library.PlanetName.Jupiter); }
            if (gocharaVenus == gocharaHouse) { planetList.Add(Library.PlanetName.Venus); }
            if (gocharaSaturn == gocharaHouse) { planetList.Add(Library.PlanetName.Saturn); }

            return planetList;
        }

        /// <summary>
        /// Gets the Vedhanka (point of obstruction), used for Gohchara calculations.
        /// The data returned comes from a fixed table. 
        /// NOTE: - Planet exceptions are not accounted for here.
        ///       - Return 0 when no obstruction point exists 
        /// Reference : Hindu Predictive Astrology pg. 257
        /// </summary>
        public static int Vedhanka(PlanetName planet, int house)
        {
            //filter based on planet
            if (planet == Library.PlanetName.Sun)
            {
                //good
                if (house == 11) { return 5; }
                if (house == 3) { return 9; }
                if (house == 10) { return 4; }
                if (house == 6) { return 12; }
                //bad
                if (house == 5) { return 11; }
                if (house == 9) { return 3; }
                if (house == 4) { return 10; }
                if (house == 12) { return 6; }
            }

            if (planet == Library.PlanetName.Moon)
            {
                //good
                if (house == 7) { return 2; }
                if (house == 1) { return 5; }
                if (house == 6) { return 12; }
                if (house == 11) { return 8; }
                if (house == 10) { return 4; }
                if (house == 3) { return 9; }
                //bad
                if (house == 2) { return 7; }
                if (house == 5) { return 1; }
                if (house == 12) { return 6; }
                if (house == 8) { return 11; }
                if (house == 4) { return 10; }
                if (house == 9) { return 3; }

            }

            if (planet == Library.PlanetName.Mars)
            {
                //good
                if (house == 3) { return 12; }
                if (house == 11) { return 5; }
                if (house == 6) { return 9; }
                //bad
                if (house == 12) { return 3; }
                if (house == 5) { return 11; }
                if (house == 9) { return 6; }
            }

            if (planet == Library.PlanetName.Mercury)
            {
                //good
                if (house == 2) { return 5; }
                if (house == 4) { return 3; }
                if (house == 6) { return 9; }
                if (house == 8) { return 1; }
                if (house == 10) { return 7; }
                if (house == 11) { return 12; }

                //bad
                if (house == 5) { return 2; }
                if (house == 3) { return 4; }
                if (house == 9) { return 6; }
                if (house == 1) { return 8; }
                if (house == 7) { return 10; }
                if (house == 12) { return 11; }
            }

            if (planet == Library.PlanetName.Jupiter)
            {
                //good
                if (house == 2) { return 12; }
                if (house == 11) { return 8; }
                if (house == 9) { return 10; }
                if (house == 5) { return 4; }
                if (house == 7) { return 3; }

                //bad
                if (house == 12) { return 2; }
                if (house == 8) { return 11; }
                if (house == 10) { return 9; }
                if (house == 4) { return 5; }
                if (house == 3) { return 7; }

            }

            if (planet == Library.PlanetName.Venus)
            {
                //good
                if (house == 1) { return 8; }
                if (house == 2) { return 7; }
                if (house == 3) { return 1; }
                if (house == 4) { return 10; }
                if (house == 5) { return 9; }
                if (house == 8) { return 5; }
                if (house == 9) { return 11; }
                if (house == 11) { return 6; }
                if (house == 12) { return 3; }

                //bad
                if (house == 8) { return 1; }
                if (house == 7) { return 2; }
                if (house == 1) { return 3; }
                if (house == 10) { return 4; }
                if (house == 9) { return 5; }
                if (house == 5) { return 8; }
                if (house == 11) { return 9; }
                if (house == 6) { return 11; }
                if (house == 3) { return 12; }

            }

            if (planet == Library.PlanetName.Saturn)
            {
                //good
                if (house == 3) { return 12; }
                if (house == 11) { return 5; }
                if (house == 6) { return 9; }

                //bad
                if (house == 12) { return 3; }
                if (house == 5) { return 11; }
                if (house == 9) { return 6; }

            }
            //copy of saturn & mars
            if (planet == Library.PlanetName.Rahu)
            {
                //good
                if (house == 3) { return 12; }
                if (house == 11) { return 5; }
                if (house == 6) { return 9; }

                //bad
                if (house == 12) { return 3; }
                if (house == 5) { return 11; }
                if (house == 9) { return 6; }

            }
            if (planet == Library.PlanetName.Ketu)
            {
                //good
                if (house == 3) { return 12; }
                if (house == 11) { return 5; }
                if (house == 6) { return 9; }

                //bad
                if (house == 12) { return 3; }
                if (house == 5) { return 11; }
                if (house == 9) { return 6; }

            }





            //if no condition above met, then there is no obstruction point
            return 0;
        }

        /// <summary>
        /// Is SunGocharaInHouse1
        /// Checks if a Gochara is occuring for a planet in a given house without any obstructions at a given time
        /// Note : Basically a wrapper method for Gochra event calculations
        /// </summary>
        public static bool IsGocharaOccurring(Time birthTime, Time time, PlanetName planet, int gocharaHouse)
        {
            //check if planet is in the specified gochara house
            var planetGocharaMatch = Calculate.GocharaZodiacSignCountFromMoon(birthTime, time, planet) == gocharaHouse;

            //NOTE: only use Vedha point by default, but allow disable if needed (LONG LEVER DESIGN)
            bool obstructionNotFound = true; //default to true, so if disabled Vedha point will still work
            if (Calculate.UseVedhankaInGochara)
            {
                //check if there is any planet obstructing this transit prediction via Vedhasthana
                obstructionNotFound = !Calculate.IsGocharaObstructed(planet, gocharaHouse, birthTime, time);
            }

            //occuring if all conditions met
            var occuring = planetGocharaMatch && obstructionNotFound;

            return occuring;
        }

        /// <summary>
        /// Checks if a given planet's with given number of bindu is transiting now (Gochara)
        /// </summary>
        public static bool IsPlanetGocharaBindu(Time birthTime, Time nowTime, PlanetName planet, int bindu)
        {
            //house the planet is transiting now
            var gocharaSignCount = Calculate.GocharaZodiacSignCountFromMoon(birthTime, nowTime, planet);

            //check if there is any planet obstructing this transit prediction via Vedhasthana
            var obstructionFound = Calculate.IsGocharaObstructed(planet, gocharaSignCount, birthTime, nowTime);

            //if obstructed end here
            if (obstructionFound) { return false; }

            //gochara ongoing, get sign of house to get planet's bindu score for said transit
            var gocharaSign = HouseSignName((HouseName)gocharaSignCount, nowTime);

            //get planet's current bindu
            var planetBindu = Calculate.PlanetAshtakvargaBindu(planet, gocharaSign, nowTime);

            //occuring if bindu is match
            var occuring = planetBindu == bindu;

            return occuring;
        }


        #endregion

        #region DASA

        /// <summary>
        /// Given a start time and end time and birth time will calculate all dasa periods
        /// in nice JSON table format
        /// You can also set how many levels of dasa you want to calculate, default is 4
        /// 7 Levels : Dasa > Bhukti > Antaram > Sukshma > Prana > Avi Prana > Viprana
        /// </summary>
        /// <param name="levels">range 1 to 7,coresponds to bhukti, antaram, etc..., lower is faster</param>
        /// <param name="scanYears">time span to calculate, defaults 100 years, average life</param>
        /// <param name="precisionHours"> defaults to 21 days, higher is faster
        /// set how accurately the start & end time of each event is calculated
        /// exp: setting 1 hour, means given in a time range of 100 years, it will be checked 876600 times 
        /// </param>
        public static JObject DasaForLife(Time birthTime, int levels = 3, int precisionHours = 100, int scanYears = 100)
        {
            //TODO NOTE:
            //precisionHours limits the levels that can be calculated (because of 0 filtering)

            //based on scan years, set start & end time
            Time startTime = birthTime;
            Time endTime = birthTime.AddYears(scanYears);

            //set what dasa levels to include based on input level
            var tagList = new List<EventTag>();
            for (int i = 1; i <= levels; i++)
            {
                tagList.Add((EventTag)Enum.Parse(typeof(EventTag), $"PD{i}"));
            }

            // TEMP hack to place time in Person (wrapped) 
            var johnDoe = new Person("", birthTime, Gender.Empty);

            //do calculation (heavy computation)
            List<Event> eventList = EventManager.CalculateEvents(precisionHours,
                                                                        startTime,
                                                                        endTime,
                                                                        johnDoe,
                                                                        tagList);

            //convert to Dasa Events for special Dasa related formating
            var dasaEventList = eventList.Select(e => new DasaEvent(e)).ToList();

            //format raw events into nested JSON format
            var dasaEvents1 = VimshottariDasa.GetDasaJson(dasaEventList, 1);

            return dasaEvents1;

        }

        /// <summary>
        /// Calculates dasa for a specific time frame
        /// </summary>
        /// <param name="startTime">start of time range to show dasa</param>
        /// <param name="endTime">end of time range to show dasa</param>
        /// <param name="levels">range 1 to 7,coresponds to bhukti, antaram, etc..., lower is faster</param>
        /// <param name="precisionHours"> defaults to 21 days, higher is faster
        /// set how accurately the start & end time of each event is calculated
        /// exp: setting 1 hour, means given in a time range of 100 years, it will be checked 876600 times 
        /// </param>
        /// <returns></returns>
        public static JObject DasaAtRange(Time birthTime, Time startTime, Time endTime, int levels = 3, int precisionHours = 100)
        {
            //TODO NOTE:
            //precisionHours limits the levels that can be calculated (because of 0 filtering)

            //based on scan years, set start & end time

            //set what dasa levels to include based on input level
            var tagList = new List<EventTag>();
            for (int i = 1; i <= levels; i++)
            {
                tagList.Add((EventTag)Enum.Parse(typeof(EventTag), $"PD{i}"));
            }

            // TEMP hack to place time in Person (wrapped) 
            var johnDoe = new Person("", birthTime, Gender.Empty);

            //do calculation (heavy computation)
            List<Event> eventList = EventManager.CalculateEvents(precisionHours,
                                                                        startTime,
                                                                        endTime,
                                                                        johnDoe,
                                                                        tagList);

            //convert to Dasa Events for special Dasa related formating
            var dasaEventList = eventList.Select(e => new DasaEvent(e)).ToList();

            //format raw events into nested JSON format
            var dasaEvents1 = VimshottariDasa.GetDasaJson(dasaEventList, 1);

            return dasaEvents1;
        }

        public static JObject DasaAtTime(Time birthTime, Time checkTime, int levels = 3)
        {
            //TODO NOTE:
            //precisionHours limits the levels that can be calculated (because of 0 filtering)
            //based on scan years, set start & end time

            //set what dasa levels to include based on input level
            var tagList = new List<EventTag>();
            for (int i = 1; i <= levels; i++)
            {
                tagList.Add((EventTag)Enum.Parse(typeof(EventTag), $"PD{i}"));
            }

            // TEMP hack to place time in Person (wrapped) 
            var johnDoe = new Person("", birthTime, Gender.Empty);

            //do calculation (heavy computation)
            List<Event> eventList = EventManager.CalculateEvents(1,
                                                                        checkTime,
                                                                        checkTime,
                                                                        johnDoe,
                                                                        tagList, false);

            //convert to Dasa Events for special Dasa related formating
            var dasaEventList = eventList.Select(e => new DasaEvent(e)).ToList();

            //format raw events into nested JSON format
            var dasaEvents1 = VimshottariDasa.GetDasaJson(dasaEventList, 1);

            return dasaEvents1;

        }

        public static JObject DasaForNow(Time birthTime, int levels = 3)
        {
            //TODO NOTE:
            //precisionHours limits the levels that can be calculated (because of 0 filtering)
            //based on scan years, set start & end time

            //set what dasa levels to include based on input level
            var tagList = new List<EventTag>();
            for (int i = 1; i <= levels; i++)
            {
                tagList.Add((EventTag)Enum.Parse(typeof(EventTag), $"PD{i}"));
            }

            // TEMP hack to place time in Person (wrapped) 
            var johnDoe = new Person("", birthTime, Gender.Empty);

            //get now time using birth location
            var nowTime = Time.NowSystem(birthTime.GetGeoLocation());

            //do calculation (heavy computation)
            List<Event> eventList = EventManager.CalculateEvents(1,
                                                                        nowTime,
                                                                        nowTime,
                                                                        johnDoe,
                                                                        tagList, false);

            //convert to Dasa Events for special Dasa related formating
            var dasaEventList = eventList.Select(e => new DasaEvent(e)).ToList();

            //format raw events into nested JSON format
            var dasaEvents1 = VimshottariDasa.GetDasaJson(dasaEventList, 1);

            return dasaEvents1;

        }

        #endregion

        #region PLANET BENEFIC & MALEFIC

        /// <summary>
        /// Whenever an affiiction by way of a malefic occupying
        /// a certain house or joining with a certain planet
        /// is suggested, by implication an aspect is also meant,
        /// though an affliction caused by aspect.is comparatively less malevolent
        ///
        /// Note:
        /// TODO presently not 100% sure, if what is meant by "affliction" is solely only limited to
        /// aspects & conjunction with bad planets. Or
        /// -Located in enemy sign an affliction?
        /// -Low shadbala an affliction?
        /// -Low drikbala an affliction?
        ///
        /// 
        /// At present, malefic aspects & conjunctions are used
        /// becasue it seems based on texts that this is correct.
        /// 
        /// But it seems mercury in enemny sign or position in a house should also play a role.
        /// 
        /// There must be a corelation between shadbala or drikbala to aspects & conjucntion
        /// A more precise way of mesurement it could be via the bala method.
        /// Needs testing for sure, to find out what bala values determine an afflicted mercury
        ///
        /// </summary>
        /// TODO POSSIBLE RENAME TO is IsMercuryMalefic
        public static bool IsMercuryAfflicted(Time time)
        {
            //for now only malefic conjunctions are considered
            return IsMercuryMalefic(time);

        }

        /// <summary>
        /// Check if Mercury is malefic (true), returns false if benefic 
        /// 
        ///
        /// References:
        /// 
        /// "Mercury, by nature, is called sournya or good. And if he is in
        /// conjunction with the Sun, Saturn, Mars, Rahu or Ketu, he will
        /// be a malefic. His conjunction with beneficial planets like Full
        /// Moon, Jupiter or Venus will classify him as a benefic. Benefic
        /// means a good and malefic means an evil planet."
        /// -TODO Does malefic moon make it malefic? (atm malefic moon makes it malefic)
        ///
        /// "Though in the earlier pages Mercury is defined either as a subba
        /// (benefic) or papa (malefic) according to its association is with a benefic or
        /// malefic, Mercury for purposes of calculating Drisbtibala of Bbavas is to
        /// be deemed as a full benefic. This is in accord with the injunctions of
        /// classical writers (Gurugnabbyam tu yuktasya poomamekam tu yojayet).
        /// "
        ///11. Benefics and Malefics. Among these, Sūrya, Śani, Mangal, decreasing Candr, Rahu and
        /// Ketu (the ascending and the descending nodes of Candr) are malefics, while the rest are
        /// benefics. Budh, however, is a malefic, if he joins a malefic. 
        /// 
        /// Note:
        /// ATM malefic planets override benefic
        /// TODO not sure if malefic planet overrides benefic if both are conjunct
        /// </summary>
        public static bool IsMercuryMalefic(Time time)
        {
            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey(nameof(IsMercuryMalefic), time, Ayanamsa), _isMercuryMalefic);


            //UNDERLYING FUNCTION
            bool _isMercuryMalefic()
            {
                //if mercury is already with malefics,then not checking if conjunct with benefic (not 100% sure)
                if (conjunctWithMalefic()) { return true; }

                //if conjunct with benefic, then it is benefic
                if (conjunctWithBenefic()) { return false; }

                //if not conjunct with any planet, should be benefic
                //NOTE : Mercury, by nature, is called sournya or good.
                return false; // false means not malefic


                //------------FUNCTIONS-------------

                bool conjunctWithMalefic()
                {
                    //list the planets that will make mercury malefic
                    var evilPlanetNameList = new List<PlanetName>() { Library.PlanetName.Sun, Library.PlanetName.Saturn, Library.PlanetName.Mars, Library.PlanetName.Rahu, Library.PlanetName.Ketu };

                    //if moon is malefic, add to malefic list
                    if (!IsMoonBenefic(time)) { evilPlanetNameList.Add(Library.PlanetName.Moon); }

                    //get all planets in conjunction with mercury
                    var planetsConjunct = Calculate.PlanetsInConjuction(Library.PlanetName.Mercury, time);

                    //mark evil planet not in conjunct at first
                    bool evilPlanetFoundInConjunct = false;

                    //check if evil planets are in conjunct
                    foreach (var planetName in evilPlanetNameList)
                    {
                        evilPlanetFoundInConjunct = planetsConjunct.Contains(planetName);

                        //if one evil planet is found, break loop, stop looking
                        if (evilPlanetFoundInConjunct) { break; }
                    }

                    //return flag of evil planets found in conjunct
                    return evilPlanetFoundInConjunct;

                }

                bool conjunctWithBenefic()
                {
                    var beneficPlanetNameList = new List<PlanetName>() { Library.PlanetName.Jupiter, Library.PlanetName.Venus };

                    //if moon is benefic, add to benefic list
                    if (IsMoonBenefic(time)) { beneficPlanetNameList.Add(Library.PlanetName.Moon); }

                    //get all planets in conjunction with mercury
                    var planetsConjunct = Calculate.PlanetsInConjuction(Library.PlanetName.Mercury, time);

                    //mark benefic planet not in conjunct at first
                    bool beneficPlanetFoundInConjunct = false;

                    //check if benefic planets are in conjunct
                    foreach (var planetName in beneficPlanetNameList)
                    {
                        beneficPlanetFoundInConjunct = planetsConjunct.Contains(planetName);

                        //if one good planet is found, break loop, stop looking
                        if (beneficPlanetFoundInConjunct) { break; }
                    }

                    //return flag of benefic planets found in conjunct
                    return beneficPlanetFoundInConjunct;

                }

            }


        }

        /// <summary>
        /// Moon is a benefic from the 8th day of the bright half of the lunar month
        /// to the 8th day of the dark half of the lunar month
        /// and a malefic in the rest of the days.
        /// 
        /// Returns true if benefic & false if malefic
        /// </summary>
        public static bool IsMoonBenefic(Time time)
        {
            //get the lunar date number
            int lunarDateNumber = Calculate.LunarDay(time).GetLunarDateNumber();

            //8th day of the bright half = 8th lunar date
            //8th day of the dark half  = 23rd lunar date
            if (lunarDateNumber >= 8 && lunarDateNumber <= 23)
            {
                return true;
            }
            else
            {
                return false;
            }

        }


        /// <summary>
        /// Checks if a given planet is benefic
        /// </summary>
        public static bool IsPlanetBenefic(PlanetName planetName, Time time)
        {
            //get list of benefic planets
            var beneficPlanetList = BeneficPlanetList(time);

            //check if input planet is in the list
            var planetIsBenefic = beneficPlanetList.Contains(planetName);

            return planetIsBenefic;
        }

        /// <summary>
        /// Gets all planets that are benefics at a given time, since moon & mercury changes
        /// Benefics, on the other hand, tend to do good ; but
        /// sometimes they also become capable of doing harm.
        /// </summary>
        public static List<PlanetName> BeneficPlanetList(Time time)
        {
            //Add permanent good planets to list first
            var listOfGoodPlanets = new List<PlanetName>() { PlanetName.Jupiter, PlanetName.Venus };

            //check if moon is benefic
            var moonIsBenefic = IsMoonBenefic(time);

            //if moon is benefic add to benefic list
            if (moonIsBenefic) { listOfGoodPlanets.Add(PlanetName.Moon); }

            //check if mercury is good
            var mercuryIsBenefic = IsMercuryMalefic(time) == false;

            //if mercury is benefic add to benefic list
            if (mercuryIsBenefic) { listOfGoodPlanets.Add(PlanetName.Mercury); }

            return listOfGoodPlanets;
        }

        /// <summary>
        /// Checks if a given planet is Malefic
        /// </summary>
        public static bool IsPlanetMalefic(PlanetName planetName, Time time)
        {
            //get list of malefic planets
            var maleficPlanetList = MaleficPlanetList(time);

            //check if input planet is in the list
            var planetIsMalefic = maleficPlanetList.Contains(planetName);

            return planetIsMalefic;
        }

        /// <summary>
        /// Gets list of permanent malefic planets,
        /// for moon & mercury it is based on changing factors
        ///
        /// Malefics are always inclined to do harm, but under certain
        /// conditions, the intensity of the mischief is tempered.
        /// </summary>
        public static List<PlanetName> MaleficPlanetList(Time time)
        {
            //Add permanent evil planets to list first
            var listOfEvilPlanets = new List<PlanetName>() { Library.PlanetName.Sun, Library.PlanetName.Saturn, Library.PlanetName.Mars, Library.PlanetName.Rahu, Library.PlanetName.Ketu };

            //check if moon is evil
            var moonIsEvil = IsMoonBenefic(time) == false;

            //if moon is evil add to evil list
            if (moonIsEvil)
            {
                listOfEvilPlanets.Add(Library.PlanetName.Moon);
            }

            //check if mercury is evil
            var mercuryIsEvil = IsMercuryMalefic(time);
            //if mercury is evil add to evil list
            if (mercuryIsEvil)
            {
                listOfEvilPlanets.Add(Library.PlanetName.Mercury);
            }

            return listOfEvilPlanets;
        }

        /// <summary>
        /// Gets all planets the inputed planet is transmitting aspect to
        /// </summary>
        public static List<PlanetName> PlanetsInAspect(PlanetName inputPlanet, Time time)
        {
            //get signs planet is aspecting
            var signAspecting = Calculate.SignsPlanetIsAspecting(inputPlanet, time);

            //get all the planets located in these signs
            var planetsAspected = new List<PlanetName>();
            foreach (var zodiacSign in signAspecting)
            {
                var planetInSign = Calculate.PlanetInSign(zodiacSign, time);

                //add to list
                planetsAspected.AddRange(planetInSign);
            }

            //return these planets as aspected by input planet
            return planetsAspected;
        }

        /// <summary>
        /// Calculate aspect angle between 2 planets
        /// </summary>
        public static double PlanetAspectDegree(PlanetName receiver, PlanetName trasmitter, Time time)
        {
            //Finding Drishti Kendra or Aspect Angle
            var planetNirayanaLongitude = Calculate.PlanetNirayanaLongitude(receiver, time).TotalDegrees;
            var nirayanaLongitude = Calculate.PlanetNirayanaLongitude(trasmitter, time).TotalDegrees;
            var dk = planetNirayanaLongitude - nirayanaLongitude;

            if (dk < 0) { dk += 360; }

            //get special aspect if any
            var vdrishti = FindViseshaDrishti(dk, trasmitter);

            var final = FindDrishtiValue(dk) + vdrishti;

            return final;

        }

        /// <summary>
        /// Gets all planets the transmitting aspect to inputed planet
        /// </summary>
        public static List<PlanetName> PlanetsAspectingPlanet(PlanetName receivingAspect, Time time)
        {
            //check if all planets is aspecting inputed planet
            var aspectFound = Library.PlanetName.All9Planets.FindAll(transmitPlanet => IsPlanetAspectedByPlanet(receivingAspect, transmitPlanet, time));

            return aspectFound;
        }

        /// <summary>
        /// Gets houses aspected by the inputed planet
        /// </summary>
        public static List<HouseName> HousesInAspect(PlanetName planet, Time time)
        {
            //get signs planet is aspecting
            var signAspecting = Calculate.SignsPlanetIsAspecting(planet, time);

            //get all the houses located in these signs
            var housesAspected = new List<HouseName>();
            foreach (var house in Library.House.AllHouses)
            {
                //get sign of house
                var houseSign = Calculate.HouseSignName(house, time);

                //add house to list if sign is aspected by planet
                if (signAspecting.Contains(houseSign)) { housesAspected.Add(house); }
            }

            //return the houses aspected by input planet
            return housesAspected;

        }

        /// <summary>
        /// Gets all planets aspecting inputed house
        /// </summary>
        public static List<PlanetName> PlanetsAspectingHouse(HouseName inputHouse, Time time)
        {
            //create empty list
            var returnPlanetList = new List<PlanetName>();

            //check each planet if aspecting house
            foreach (var planet in Library.PlanetName.All9Planets)
            {
                //get houses
                var housesInAspect = HousesInAspect(planet, time);

                //check if any house is a match
                var houseMatch = housesInAspect.FindAll(house => house == inputHouse).Any();
                if (houseMatch)
                {
                    returnPlanetList.Add(planet);
                }
            }


            return returnPlanetList;
        }

        /// <summary>
        /// Checks if the a planet is aspected by another planet
        /// </summary>
        public static bool IsPlanetAspectedByPlanet(PlanetName receiveingAspect, PlanetName transmitingAspect, Time time)
        {
            //get planets aspected by transmiting planet
            var planetsInAspect = Calculate.PlanetsInAspect(transmitingAspect, time);

            //if receiving planet is in list of currently aspected
            return planetsInAspect.Contains(receiveingAspect);

        }

        /// <summary>
        /// Checks if a house is aspected by a planet
        /// </summary>
        public static bool IsHouseAspectedByPlanet(HouseName receiveingAspect, PlanetName transmitingAspect, Time time)
        {
            //get houses aspected by transmiting planet
            var houseInAspect = Calculate.HousesInAspect(transmitingAspect, time);

            //if receiving house is in list of currently aspected
            return houseInAspect.Contains(receiveingAspect);

        }

        /// <summary>
        /// Checks if the a planet is conjunct with another planet
        ///
        /// Note:
        /// Both planets A & B are checked if they are in conjunct with each other,
        /// performance might be effected mildly, but errors in conjunction calculation would be caught here.
        /// Can be removed once, conjunction calculator is confirmed accurate.
        /// </summary>
        public static bool IsPlanetConjunctWithPlanet(PlanetName planetA, PlanetName planetB, Time time)
        {
            //get planets in conjunt for A & B
            var planetAConjunct = Calculate.PlanetsInConjuction(planetA, time);
            var planetBConjunct = Calculate.PlanetsInConjuction(planetB, time);

            //check that A conjuncts B and B conjuncts A 
            var conjunctFound = planetAConjunct.Contains(planetB) && planetBConjunct.Contains(planetA);

            //TODO clean or clear
            //erro check, can be removed upon corectness confirmation
            //if (planetAConjunct.Contains(planetB) != planetBConjunct.Contains(planetA))
            //{
            //    throw new Exception("Conjunct planet not uniform!");
            //}

            return conjunctFound;
        }

        #endregion

        #region PLANET AND HOUSE STRENGHT

        /// <summary>
        /// convert the planets strength into a value over hundred with max & min set by strongest & weakest planet
        /// </summary>
        public static double PlanetPowerPercentage(PlanetName inputPlanet, Time time)
        {
            //get all planet strength for given time (horoscope)
            var allPlanets = Calculate.AllPlanetStrength(time);

            //get the power of the planet inputed
            var planetPwr = allPlanets.FirstOrDefault(x => x.Item2 == inputPlanet).Item1;

            //get min & max
            var min = allPlanets.Min(x => x.Item1); //weakest planet
            var max = allPlanets.Max(x => x.Item1); //strongest planet

            //convert the planets strength into a value over hundred with max & min set by strongest & weakest planet
            //returns as percentage over 100%
            var factor = planetPwr.Remap(min, max, 0, 100);

            //planet power below 60% filtered out
            //factor = factor < 60 ? 0 : factor;

            return factor;
        }


        /// <summary>
        /// Returns an array of all planets sorted by strenght,
        /// 0 index being strongest to 8 index being weakest
        ///
        /// Note:
        /// Significance of being Powerful.-Among
        /// the several planets associated with a bhava, that,
        /// which has the greatest Sbadbala, influences the
        /// bhava most.
        /// </summary>
        public static List<PlanetName> AllPlanetOrderedByStrength(Time time)
        {

            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey(nameof(AllPlanetOrderedByStrength), time, Ayanamsa), _getAllPlanetOrderedByStrength);


            //UNDERLYING FUNCTION
            List<PlanetName> _getAllPlanetOrderedByStrength()
            {
                var planetStrenghtList = new Dictionary<PlanetName, double>();

                //create a list with planet names & its strength (unsorted)
                foreach (var planet in Library.PlanetName.All9Planets)
                {
                    //get planet strength in rupas
                    var strength = PlanetShadbalaPinda(planet, time).ToDouble();

                    //place in list with planet name
                    planetStrenghtList.Add(planet, strength);
                }


                //sort that list from strongest planet to weakest planet
                var sortedList = planetStrenghtList.OrderByDescending(item => item.Value);
                var nameOnlyList = sortedList.Select(x => x.Key).ToList();

                return nameOnlyList;

                /*--------------FUNCTIONS----------------*/
            }
        }

        /// <summary>
        /// Significance of being Powerful.-Among
        /// the several planets associated with a bhava, that,
        /// which has the greatest Sbadbala, influences the
        /// bhava most.
        /// Powerful Planets.-Ravi is befd to be
        /// powerful when his Shadbala Pinda is 5 or more
        /// rupas. Chandra becomes strong when his Shadbala
        /// Pinda is 6 or more rupas. Kuja becomes powerful
        /// when bis Shadbala Pinda does not fall short of
        /// 5 rupas.Budha becomes potent by having his
        /// Sbadbala Pinda as 7 rupas; Guru, Sukra and Sani
        /// become thoroughly powerful if their Shadbala
        /// Pindas are 6.5, 5.5 and 5 rupas or more respectively.
        /// </summary>
        public static bool IsPlanetStrongInShadbala(PlanetName planet, Time time)
        {

            var limit = 0.0;
            if (planet == Sun) { limit = 5; }
            else if (planet == Library.PlanetName.Moon) { limit = 6; }
            else if (planet == Library.PlanetName.Mars) { limit = 5; }
            else if (planet == Library.PlanetName.Mercury) { limit = 7; }
            else if (planet == Library.PlanetName.Jupiter) { limit = 6.5; }
            else if (planet == Library.PlanetName.Venus) { limit = 5.5; }
            else if (planet == Library.PlanetName.Saturn) { limit = 5; }
            //todo rahu and ketu added later on based on saturn and mars
            else if (planet == Library.PlanetName.Rahu) { limit = 5; }
            else if (planet == Library.PlanetName.Ketu) { limit = 5; }

            //divide strength by minimum limit of power (based on planet)
            //if above limit than benefic, else return false
            var shadbalaRupa = Calculate.PlanetShadbalaPinda(planet, time);
            var rupa = Math.Round(shadbalaRupa.ToRupa(), 1);
            var strengthAfterLimit = rupa / limit;

            //if 1 or above is positive, below 1 is below limit
            var isBenefic = strengthAfterLimit >= 1.1;

            return isBenefic;
        }

        /// <summary>
        /// sets benefic if above 450 score
        /// </summary>
        public static bool IsHouseBeneficInShadbala(HouseName house, Time birthTime, double threshold)
        {
            //get house strength
            var strength = HouseStrength(house, birthTime).ToDouble();

            //if above 450 then good
            var isBenefic = strength > threshold;
            return isBenefic;
        }

        /// <summary>
        /// Gets  strength (shadbala) of all 9 planets
        /// </summary>
        public static List<(double, PlanetName)> AllPlanetStrength(Time time)
        {
            var planetStrenghtList = new List<(double, PlanetName)>();

            //create a list with planet names & its strength (unsorted)
            foreach (var planet in Library.PlanetName.All9Planets)
            {
                //get planet strength in rupas
                var strength = PlanetShadbalaPinda(planet, time).ToDouble();

                //place in list with planet name
                planetStrenghtList.Add((strength, planet));

            }

            return planetStrenghtList;
        }

        /// <summary>
        /// Returns an array of all houses sorted by strength,
        /// 0 index being strongest to 11 index being weakest
        /// </summary>
        public static HouseName[] AllHousesOrderedByStrength(Time time)
        {

            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey(nameof(AllHousesOrderedByStrength), time, Ayanamsa), _getAllHousesOrderedByStrength);


            //UNDERLYING FUNCTION
            HouseName[] _getAllHousesOrderedByStrength()
            {
                var houseStrengthList = new Dictionary<double, HouseName>();

                //create a list with planet names & its strength (unsorted)
                foreach (var house in Library.House.AllHouses)
                {
                    //get house strength
                    var strength = HouseStrength(house, time).ToRupa();

                    //place in list with house number
                    houseStrengthList[strength] = house;

                }


                //sort that list from strongest house to weakest house
                var keysSorted = houseStrengthList.Keys.ToList();
                keysSorted.Sort();

                var sortedArray = new HouseName[12];
                var count = 11;
                foreach (var key in keysSorted)
                {
                    sortedArray[count] = houseStrengthList[key];
                    count--;
                }

                return sortedArray;
            }

        }

        /// <summary>
        /// THE FINAL TOTAL STRENGTH
        /// Shadbala :the six sources of strength and weakness the planets
        /// The importance of and the part played by the Shadbalas,
        /// in the science of horoscopy, are manifold
        ///
        /// In order to obtain the total strength of
        /// the Shadbala Pinda of each planet, we have to add
        /// together its Sthana Bala, Dik Bala, Kala Bala.
        /// 'Chesta Bala and Naisargika Bala. And the Graha's
        /// Drik Bala must be added to or subtracted from the
        /// above sum according as it is positive or negative.
        /// The result obtained is the Shadbala Pinda of the
        /// planet in Shashtiamsas.
        ///
        /// Note: Rahu & Ketu supported, via house lord
        /// </summary>
        public static Shashtiamsa PlanetShadbalaPinda(PlanetName planetName, Time time)
        {
            try
            {
                //return 0 if null planet
                if (planetName == null) { return Shashtiamsa.Zero; }

                //CACHE MECHANISM
                return CacheManager.GetCache(new CacheKey(nameof(PlanetShadbalaPinda), planetName, time, Ayanamsa), _getPlanetShadbalaPinda);


                //UNDERLYING FUNCTION
                Shashtiamsa _getPlanetShadbalaPinda()
                {

                    //if planet name is rahu or ketu then replace with house lord's strength
                    if (planetName == Rahu || planetName == Ketu)
                    {
                        var houseLord = LordOfHousePlanetIsIn(planetName, time);
                        planetName = houseLord;
                    }

                    //Sthana bala (Positional Strength)
                    var sthanaBala = PlanetSthanaBala(planetName, time);

                    //Get dik bala (Directional Strength)
                    var digBala = PlanetDigBala(planetName, time);

                    //Get kala bala (Temporal Strength)
                    var kalaBala = PlanetKalaBala(planetName, time);

                    //Get Chesta bala (Motional Strength)
                    var chestaBala = PlanetChestaBala(planetName, time);

                    //Get Naisargika bala (Natural Strength)
                    var naisargikaBala = PlanetNaisargikaBala(planetName, time);

                    //Get Drik/drug bala (Aspect Strength)
                    var drikBala = PlanetDrikBala(planetName, time);


                    //Get total Shadbala Pinda
                    var total = sthanaBala + digBala + kalaBala + chestaBala + naisargikaBala + drikBala;

                    //round it 2 decimal places
                    var roundedTotal = new Shashtiamsa(Math.Round(total.ToDouble(), 2));

                    return roundedTotal;
                }
            }
            catch (Exception e)
            {
                //print the error and for server guys
                Console.WriteLine(e);

                //continue without a word
                return Shashtiamsa.Zero;
            }

        }

        /// <summary>
        /// get total combined strength of the inputed planet
        /// input birth time to get strength in horoscope
        /// note: an alias method to GetPlanetShadbalaPinda ("strength" is easier to remember)
        /// </summary>
        public static Shashtiamsa PlanetStrength(PlanetName planetName, Time time) => PlanetShadbalaPinda(planetName, time);

        /// <summary>
        /// Gets the lord of the house the inputed planet is in
        /// </summary>
        private static PlanetName LordOfHousePlanetIsIn(PlanetName planetName, Time time)
        {
            var currentHouse = Calculate.HousePlanetOccupies(planetName, time);
            var houseLord = Calculate.LordOfHouse((HouseName)currentHouse, time);

            return houseLord;
        }

        /// <summary>
        /// Aspect strength
        ///
        /// This strength is gained by the virtue of the aspect
        /// (Graha Dristi) of different planets on other planet.
        /// The aspect of benefics is considered to be strength and
        /// the aspect of malefics is considered to be weaknesses.
        ///
        /// 
        /// Drik Bala.-This means aspect strength.
        /// The Drik Bala of a Gqaha is one-fourth of the
        /// Drishti Pinda on it. It is positive or negative
        /// according as the Drishti Pinda is positive or
        /// negative.
        ///
        /// 
        /// See the formula given on page 85. There is
        /// special aspect for Jupiter, ,Mars and Saturn on the
        /// 5th and 9th, 4th and 8th and 3rd and 10th signs
        /// respectively.
        /// </summary>
        public static Shashtiamsa PlanetDrikBala(PlanetName planetName, Time time)
        {
            //no calculation for rahu and ketu here
            var isRahu = planetName.Name == Library.PlanetName.PlanetNameEnum.Rahu;
            var isKetu = planetName.Name == Library.PlanetName.PlanetNameEnum.Ketu;
            var isRahuKetu = isRahu || isKetu;
            if (isRahuKetu) { return Shashtiamsa.Zero; }

            double dk;
            var drishti = new Dictionary<string, double>();
            double vdrishti;
            var sp = new Dictionary<PlanetName, int>();


            foreach (var p in Library.PlanetName.All7Planets)
            {
                if (Calculate.IsPlanetBenefic(p, time))
                {
                    sp[p] = 1;
                }
                else
                {
                    sp[p] = -1;
                }

            }


            foreach (var i in Library.PlanetName.All7Planets)
            {
                foreach (var j in Library.PlanetName.All7Planets)
                {
                    //Finding Drishti Kendra or Aspect Angle
                    var planetNirayanaLongitude = Calculate.PlanetNirayanaLongitude(j, time).TotalDegrees;
                    var nirayanaLongitude = Calculate.PlanetNirayanaLongitude(i, time).TotalDegrees;
                    dk = planetNirayanaLongitude - nirayanaLongitude;

                    if (dk < 0) { dk += 360; }

                    //get special aspect if any
                    vdrishti = FindViseshaDrishti(dk, i);

                    drishti[i.ToString() + j.ToString()] = FindDrishtiValue(dk) + vdrishti;

                }
            }

            double bala = 0;

            var DrikBala = new Dictionary<PlanetName, double>();

            foreach (var i in Library.PlanetName.All7Planets)
            {
                bala = 0;

                foreach (var j in All7Planets)
                {
                    bala = bala + (sp[j] * drishti[j.ToString() + i.ToString()]);

                }

                DrikBala[i] = bala / 4;
            }



            return new Shashtiamsa(DrikBala[planetName]);



        }

        /// <summary>
        /// Get special aspect if any of Kuja, Guru and Sani
        /// </summary>
        public static double FindViseshaDrishti(double dk, PlanetName p)
        {
            double vdrishti = 0;

            if (p == Library.PlanetName.Saturn)
            {
                if (((dk >= 60) && (dk <= 90)) || ((dk >= 270) && (dk <= 300)))
                {
                    vdrishti = 45;
                }

            }
            else if (p == Library.PlanetName.Jupiter)
            {

                if (((dk >= 120) && (dk <= 150))
                    || ((dk >= 240) && (dk <= 270)))
                {
                    vdrishti = 30;
                }

            }
            else if (p == Library.PlanetName.Mars)
            {
                if (((dk >= 90) && (dk <= 120)) || ((dk >= 210) && (dk <= 240)))
                {
                    vdrishti = 15;
                }

            }
            else
            {
                vdrishti = 0;
            }


            return vdrishti;

        }

        public static double FindDrishtiValue(double dk)
        {

            double drishti = 0;

            if ((dk >= 30.0) && (dk <= 60))
            {
                drishti = (dk - 30) / 2;
            }
            else if ((dk > 60.0) && (dk <= 90))
            {
                drishti = (dk - 60) + 15;
            }
            else if ((dk > 90.0) && (dk <= 120))
            {
                drishti = ((120 - dk) / 2) + 30;
            }
            else if ((dk > 120.0) && (dk <= 150))
            {
                drishti = (150 - dk);
            }
            else if ((dk > 150.0) && (dk <= 180))
            {
                drishti = (dk - 150) * 2;
            }
            else if ((dk > 180.0) && (dk <= 300))
            {
                drishti = (300 - dk) / 2;
            }

            return drishti;

        }

        /// <summary>
        /// Nalsargika Bala.-This is the natural
        /// strength that each Graha possesses. The value
        /// assigned to each depends upon its luminosity.
        /// Ravi, the brightest of all planets, has the greatest
        /// Naisargika strength while Sani, the darkest, has
        /// the least Naisargika Bala.
        ///
        /// This is the natural or inherent strength of a planet.
        /// </summary>
        public static Shashtiamsa PlanetNaisargikaBala(PlanetName planetName, Time time)
        {
            //no calculation for rahu and ketu here
            var isRahu = planetName.Name == Library.PlanetName.PlanetNameEnum.Rahu;
            var isKetu = planetName.Name == Library.PlanetName.PlanetNameEnum.Ketu;
            var isRahuKetu = isRahu || isKetu;
            if (isRahuKetu) { return Shashtiamsa.Zero; }


            if (planetName == Library.PlanetName.Sun) { return new Shashtiamsa(60); }
            else if (planetName == Library.PlanetName.Moon) { return new Shashtiamsa(51.43); }
            else if (planetName == Library.PlanetName.Venus) { return new Shashtiamsa(42.85); }
            else if (planetName == Library.PlanetName.Jupiter) { return new Shashtiamsa(34.28); }
            else if (planetName == Library.PlanetName.Mercury) { return new Shashtiamsa(25.70); }
            else if (planetName == Library.PlanetName.Mars) { return new Shashtiamsa(17.14); }
            else if (planetName == Library.PlanetName.Saturn) { return new Shashtiamsa(8.57); }

            throw new Exception("Planet not specified!");
        }

        /// <summary>
        /// NOTE: sun, moon get score for ISHTA/KESHA calculation only when specified for IshataKashta
        /// MOTIONAL STRENGTH
        /// Chesta here means Vakra Chesta or act of retrogression. Each planet, except the Sun and the Moon,
        /// and shadowy planets get into the state of Vakra or retrogression
        /// when its distance from the Sun exceeds a particular limit.
        /// And the strength or potency due to the planet on account of the arc of the retrogression is
        /// termed as Chesta Bala
        ///
        /// Deduct from the Seeghrocbcha, half the sum
        /// of the True and Mean Longitudes of planets and
        /// divide the difference by 3. The quotient is the
        /// Chestabala.
        /// Max 60, meaning Retrograde/Vakra
        /// When the distance of any one planet from
        /// the Sun exceeds a particular limit, it becomes
        /// retrograde, i.e., when the planet goes from
        /// perihelion (the point in a planet's orbit nearest
        /// to the Sun) to aphelion (the part of a planet's
        /// oroit most distant from the Sun) as it recedes
        /// from the Sun, it gradually loses the power
        /// of the Sun's gravitation and consequently, 
        /// </summary>
        public static Shashtiamsa PlanetChestaBala(PlanetName planetName, Time time, bool useSpecialSunMoon = false)
        {
            //if include Sun/Moon specified, then use special function (used for Ishta/Kashta score)
            if (planetName == Sun && useSpecialSunMoon) { return SunChestaBala(time); }
            if (planetName == Moon && useSpecialSunMoon) { return MoonChestaBala(time); }

            //the Sun,Moon,Rahu and Ketu does not not retrograde, so 0 chesta bala
            if (planetName == Sun || planetName == Moon || planetName == Rahu || planetName == Ketu) { return Shashtiamsa.Zero; }

            //get the interval between birth date and the date of the epoch (1900)
            //verified standard horoscope = 6862.579
            //NOTE: dates before 1900 give negative values
            var interval = EpochInterval(time);

            //get the mean/average longitudes of all planets
            var madhya = Madhya(interval, time);

            //get the apogee of all planets (apogee=earth, aphelion=sun)
            //aphelion (the part of a planet's orbit most distant from the Sun) 
            var seegh = GetSeeghrochcha(madhya, interval, time);


            //calculate chesta kendra, also called Seeghra kendra
            var planetLongitude = Calculate.PlanetNirayanaLongitude(planetName, time).TotalDegrees;
            //This is the Arc of retrogression.
            var planetAphelion = seegh[planetName]; //fixed most distant point from sun
            var planetMeanCircle = madhya[planetName]; //planet average distant point from sun (CIRCLE ORBIT)
                                                       //Chesta kendra = 180 degrees = Retrograde
                                                       //Because the orbits are elliptical
                                                       //and not circular, equations are applied to the mean positions to get the true longitudes.
            var trueLongitude = ((planetMeanCircle + planetLongitude) / 2.0);
            //distance from stationary point, if less than 0 than add 360 
            var chestaKendra = (planetAphelion - trueLongitude);


            //If the Chesta kendra exceeds 180° (maximum retrograde), subtract it from 360, otherwise
            //keep it as it is. The remainder represents the reduced Chesta kendra
            //NOTE: done to reduce value of direct motion, only value relative to retro motion
            if (chestaKendra < 360.00)
            {
                chestaKendra = chestaKendra + 360;
            }
            chestaKendra = chestaKendra % 360;
            if (chestaKendra > 180.00) { chestaKendra = 360 - chestaKendra; }


            //The Chesta Bala is zero when the Chesta kendra is also zero. When it is
            //180° the Chesta Bala is 60 Shashtiamsas. In intermediate position, the
            //Bala is found by proportion (devide by 3)
            var chestaBala = (chestaKendra / 3.00);

            return new Shashtiamsa(chestaBala);



            //------------------------FUNCTIONS--------------


            //Seeghrochcha is the aphelion of the planet
            //It is required to find the Chesta kendra.
            //NOTE:aphelion (the part of a planet's orbit most distant from the Sun)
            Dictionary<PlanetName, double> GetSeeghrochcha(Dictionary<PlanetName, double> mean, double epochToBirthDays, Time time1)
            {
                int _birthYear = time1.GetLmtDateTimeOffset().Year;
                var seegh = new Dictionary<PlanetName, double>();
                double correction;

                //The mean longitude of the Sun will be the Seeghrochcha of Kuja, Guru and Sani.
                seegh[Library.PlanetName.Mars] = seegh[Library.PlanetName.Jupiter] = seegh[Library.PlanetName.Saturn] = mean[Library.PlanetName.Sun];

                correction = 6.670 + (0.00133 * (_birthYear - 1900));
                double changeDuringIntervalMercury = (epochToBirthDays * 4.092385);
                const double aphelionAtEpochMercury = 164.00; // The Seeghrochcha of Budha at the epoch
                double mercuryAphelion = changeDuringIntervalMercury < 0 ? aphelionAtEpochMercury - changeDuringIntervalMercury : aphelionAtEpochMercury + changeDuringIntervalMercury;
                mercuryAphelion -= correction; //further correction of +6.670-0133
                seegh[Library.PlanetName.Mercury] = (mercuryAphelion + correction) % 360;

                correction = 5 + (0.0001 * (_birthYear - 1900));
                double changeDuringIntervalVenus = (epochToBirthDays * 1.602159);
                const double aphelionAtEpochVenus = 328.51; // The Seeghrochcha of Budha at the epoch
                double venusAphelion = changeDuringIntervalVenus < 0 ? aphelionAtEpochVenus - changeDuringIntervalVenus : aphelionAtEpochVenus + changeDuringIntervalVenus;
                venusAphelion -= correction; //diminish the sum by 5 + 0.001*t (where t = birth year - 1900)
                seegh[Library.PlanetName.Venus] = venusAphelion % 360;

                return seegh;
            }

        }

        /// <summary>
        /// special function to get chesta score for Ishta/Kashta score
        /// Bala book pg. 108
        ///
        /// Sun has no Chesta kendra or Chesta bala
        /// as he never gets into retrogression. But still a
        /// method is prescribed to find his Chesla Bala which
        /// is necessary to ascertain the lshta and Kashta·
        /// Phalas. 
        /// </summary>
        public static Shashtiamsa SunChestaBala(Time inputTime)
        {
            //Add 90° to Sun's Sayana longitude.
            var sunSayana = Calculate.PlanetSayanaLatitude(Sun, inputTime);

            //The result is Sun's Chesta kendra
            var chestaKendra = (sunSayana + Angle.Degrees90).TotalDegrees;

            //if chesta kendra execeeds 180 substract from 360
            if (chestaKendra > 180) { chestaKendra = 360 - chestaKendra; }

            //dividing this by 3 we get his Chesta bala in Shashtiamsa
            var chestaBala = chestaKendra / 3.0;

            return new Shashtiamsa(chestaBala);
        }

        /// <summary>
        /// special function to get chesta score for Ishta/Kashta score
        /// Bala book pg. 108
        /// </summary>
        public static Shashtiamsa MoonChestaBala(Time inputTime)
        {

            //Subtract the Sun's longitude from that of the Moon and the 
            //latter's Chesta Kendra is obtained.
            var sunNirayana = Calculate.PlanetNirayanaLongitude(Sun, inputTime);
            var moonNirayana = Calculate.PlanetNirayanaLongitude(Moon, inputTime);
            var chestaKendra = moonNirayana.GetDifference(sunNirayana).TotalDegrees;

            //if chesta kendra execeeds 180 substract from 360
            if (chestaKendra > 180) { chestaKendra = 360 - chestaKendra; }

            //dividing this by 3 we get his Chesta bala. in Shashtiamsa
            var chestaBala = chestaKendra / 3.0;

            return new Shashtiamsa(chestaBala);
        }

        /// <summary>
        /// The mean position of a planet is the position which it would have attained at a uniform
        /// rate of motion and the corrections to be applied in respect of the eccentricity of the orbit are not considered
        /// </summary>
        public static Dictionary<PlanetName, double> Madhya(double epochToBirthDays, Time time1)
        {
            int _birthYear = time1.GetLmtDateTimeOffset().Year;

            var madhya = new Dictionary<PlanetName, double>();

            //calculate chesta kendra, also called Seeghra kendra

            //SUN 
            //Start from the epoch. Calculate the time of interval from the epoch to the day of birth
            //and multiply the same by the daily motion of the planet, and the change during the interval is obtained.
            var sunEpochMean = 257.4568; //epoch the mean position
            double changeDuringIntervalSun = (epochToBirthDays * 0.9855931);

            //This change being added to or subtracted from the mean position at the
            //time of epoch as the date is posterior or anterior to the epoch day, the mean position is arrived at.
            double meanPositionSun = changeDuringIntervalSun < 0 ? sunEpochMean - changeDuringIntervalSun : sunEpochMean + changeDuringIntervalSun;
            meanPositionSun = meanPositionSun % 360; //expunge
            madhya[Library.PlanetName.Sun] = meanPositionSun;

            //Mean Longitudes of -Inferior Planets.-The mean longitudes of Budba and Sukra are the same as that of the Sun.
            //same for venus & mercury because closer to sun than earth it self
            madhya[Library.PlanetName.Mercury] = madhya[Library.PlanetName.Venus] = madhya[Library.PlanetName.Sun];

            //MARS
            var marsEpochMean = 270.22;
            double changeDuringIntervalMars = (epochToBirthDays * 0.5240218);
            double meanPositionMars = changeDuringIntervalMars < 0 ? marsEpochMean - changeDuringIntervalMars : marsEpochMean + changeDuringIntervalMars;
            meanPositionMars = meanPositionMars % 360; //expunge
            madhya[Library.PlanetName.Mars] = meanPositionMars;

            //JUPITER
            var jupiterEpochMean = 220.04;
            double changeDuringIntervalJupiter = (epochToBirthDays * 0.08310024);
            double meanPositionJupiter = changeDuringIntervalJupiter < 0 ? jupiterEpochMean - changeDuringIntervalJupiter : jupiterEpochMean + changeDuringIntervalJupiter;
            var correction1 = 3.33 + (0.0067 * (_birthYear - 1900));
            meanPositionJupiter -= correction1; //deduct from the total 3.33 + 0.0067*t (where t=birth year-1900).
            meanPositionJupiter %= 360; //expunge
            madhya[Library.PlanetName.Jupiter] = meanPositionJupiter;

            //SATURN
            var saturnEpochMean = 220.04;
            double changeDuringIntervalSaturn = (epochToBirthDays * 0.03333857);
            double meanPositionSaturn = changeDuringIntervalSaturn < 0 ? saturnEpochMean - changeDuringIntervalSaturn : saturnEpochMean + changeDuringIntervalSaturn;
            var correction2 = 5 + (0.001 * (_birthYear - 1900));
            meanPositionSaturn += correction2; //add 5°+0.001*t (where t = birth year - 1900)
            meanPositionSaturn %= 360; //expunge
            madhya[Library.PlanetName.Saturn] = meanPositionSaturn;

            //raise alarm if negative, since that is clearly an error, no negative mean longitude
            if (madhya.Any(x => x.Value < 0)) { throw new Exception("Madya/Mean can't be negative!"); }

            return madhya;
        }

        /// <summary>
        /// Get interval from the epoch to the birth date in days
        /// The result represents the interval from the epoch to the birth date.
        /// </summary>
        public static double EpochInterval(Time time1)
        {
            //Determine the interval between birth date and the date of the epoch thus.

            int birthYear = time1.GetLmtDateTimeOffset().Year;
            int birthMonth = time1.GetLmtDateTimeOffset().Month;
            int birthDate = time1.GetLmtDateTimeOffset().Day;

            //month ends in days
            int[] monthEnds = { 0, 31, 59, 90, 120, 151, 181, 212, 243, 273, 304, 334, 365 };

            //Deduct 1900 from the Christian Era. The difference will be past
            //years when positive and coming years when negative.
            int yrdiff = birthYear - 1900;

            //Multiply the same by 365 and to the product add the intervening bi-sextile days.
            var epochDays = ((yrdiff * 365) + (yrdiff / 4) + monthEnds[birthMonth - 1]) - 1 + birthDate;


            int hour = time1.GetLmtDateTimeOffset().Hour;
            int minute = time1.GetLmtDateTimeOffset().Minute;
            double offsetHours = time1.GetLmtDateTimeOffset().Offset.TotalHours;
            double utime = new TimeSpan(hour, minute, 0).TotalHours + ((5 + (double)(4.00 / 60.00)) - offsetHours);

            //The result represents the interval from the epoch to the birth date.
            double interval = epochDays + (double)(utime / 24.00);
            interval = Math.Round(interval, 3);//round to 3 places decimal

            return interval;
        }

        /// <summary>
        /// Gets the planets motion name, can be Retrograde, Direct, Stationary
        /// a name version of Chesta Bala
        /// </summary>
        public static PlanetMotion PlanetMotionName(PlanetName planetName, Time time)
        {
            return PlanetMotion.Direct; //RETURN DUMMY DATA

            ////sun, moon, rahu & ketu don' have retrograde so always direct
            //if (planetName == Library.PlanetName.Sun || planetName == Library.PlanetName.Moon || planetName == Library.PlanetName.Rahu || planetName == Library.PlanetName.Ketu) { return PlanetMotion.Direct; }

            ////get chestaBala
            //var chestaBala = Calculate.PlanetChestaBala(planetName, time).ToDouble();

            ////based on chesta bala assign name to it
            ////Chesta kendra = 180 degrees = Retrograde
            //switch (chestaBala)
            //{
            //    case <= 60 and > 45: return PlanetMotion.Retrograde;
            //    case <= 45 and > 15: return PlanetMotion.Direct;
            //    case <= 15 and >= 0: return PlanetMotion.Stationary;
            //    default:
            //        throw new Exception($"Error in GetPlanetMotionName : {chestaBala}");
            //}

            throw new NotImplementedException();
        }

        ///// <summary>
        ///// </summary>
        //public static PlanetMotion PlanetMotionName2(PlanetName planetName, Time time)
        //{
        //    //Brihat Parashara Hora Shatra > Ch.27 Shl.21-23
        //    //
        //    //Motions of Grahas (Mangal to Shani): Eight kinds of motions are attributed to grahas.
        //    //These are Vakra (retrogression),
        //    //Anuvakr (entering the previous rashi in retrograde motion),
        //    //Vikal (devoid of motion or in stationary position),
        //    //Mand (somewhat slower motion than usual),
        //    //Mandatar (slower than the previous mentioned motion),
        //    //Sama (somewhat increasing in motion as against Mand),
        //    //Chara (faster than Sama) and
        //    //Atichara (entering next rashi in accelerated motion).
        //    //The strengths allotted due to such 8 motions are: 60, 30, 15, 30, 15, 7.5, 45, and 30.

        //    //There is an easy method to find out Gati or speed of Mars, Jupiter & Saturn which are beyond Earth with respect to Sun (outer planets).
        //    // Whenever these planets are transmitting 2nd or 1st or 12th sign from Sun, these planets will be in - Atichara
        //    // In 3rd - Sama
        //    // In 4th - Chara
        //    // In 5th - Manda & Mandatara
        //    // In 6th - Vikala
        //    // In 7th & 8th - Vakra
        //    // In 9th - Vikala & Forward (Manda)
        //    // In 10th - Sama
        //    // In 11 th -Chara
        //    // (Source - Bhaavartha Ratnakara-Last Chapters-Translated by B. V. Raman ji)
        //    //

        //    //Ancient Siddhaanta and Phalit classics mention eight types of speeds (Gati) of planets. All these eight types apply to Pancha-taaraa planets only : Mercury, Venus, Mars, Jupiter and Saturn. Rahu and Ketu are always retrograde. Sun and Moon are never retrograde.
        //    // 
        //    // The eight types of speeds are as follows :-
        //    // 
        //    // Vakra (Faster Retrograde)
        //    // Anuvakra (Slower Retrograde)
        //    // Kutila (complicated and very slow retrograde, sometimes relapsing into non-retro)
        //    // Mandatara (slowest forward motion)
        //    // Manda (slow forward motion)
        //    // Sama (normal forward motion)
        //    // Sheeghra (fast forward)
        //    // Sheeghratara (very fast forward)
        //    // These eight speeds according to their numbers are shown in the picture below, which is GEOCENTRIC epicycloidal motion of any Pancha-taara planet. In heliocentric model, there is no such differentiation and speed is always "sama". In Geocentric system too, speed of Sun or Moon is always "sama".

        //    //sun, moon, rahu & ketu don't have retrograde so always direct
        //    if (planetName == Library.PlanetName.Sun || planetName == Library.PlanetName.Moon || planetName == Library.PlanetName.Rahu || planetName == Library.PlanetName.Ketu) { return PlanetMotion.Direct; }

        //    //get chestaBala
        //    var chestaBala = Calculate.PlanetChestaBala(planetName, time).ToDouble();

        //    //based on chesta bala assign name to it
        //    //Chesta kendra = 180 degrees = Retrograde
        //    switch (chestaBala)
        //    {
        //        case <= 60 and > 30: return PlanetMotion.Retrograde;
        //        case <= 30 and > 15: return PlanetMotion.Direct;
        //        default:
        //            throw new Exception($"Error in GetPlanetMotionName : {chestaBala}");
        //    }

        //}


        /// <summary>
        /// A retrograde planet moves in the reverse direction and, instead of
        /// increasing, its longitude decreases as the time elapses. Rahu and Ketu often
        /// move in retrograde direction only. Other planets, except the Sun and the
        /// Moon, are subject to retrogression from time to time.
        /// </summary>
        public static bool IsPlanetRetrograde(PlanetName planetName, Time time)
        {
            bool retro = false;
            //if planet is Sun or Moon than default retrograde is off
            if (planetName.Name == PlanetNameEnum.Sun || planetName.Name == PlanetNameEnum.Moon) { return false; }

            //if planet is Rahu or Ketu than default retrograde is always on
            if (planetName.Name == PlanetNameEnum.Rahu || planetName.Name == PlanetNameEnum.Ketu) { return false; } //RahuKetu never go retro. Thier retro is direct.

            //get longitude of planet at given time
            var checkTimeLong = PlanetNirayanaLongitude(planetName, time);

            //get longitude of planet next day
            var nextDay = time.AddHours(24);
            var nextDayLong = PlanetNirayanaLongitude(planetName, nextDay);

            var dayplus2 = time.AddHours(48);
            var dayplus2Long = PlanetNirayanaLongitude(planetName, dayplus2);

            var dayplus3 = time.AddHours(72);
            var dayplus3Long = PlanetNirayanaLongitude(planetName, dayplus3);
            //Console.WriteLine("Long: {0} {1} {2} {3}", dayplus3Long.TotalDegrees, dayplus2Long.TotalDegrees, nextDayLong.TotalDegrees, checkTimeLong.TotalDegrees);

            if (nextDayLong <= checkTimeLong)
            {
                //check if the next day long is less than checktimelong because its crossing over 0.00 - this is not a retro condition
                if ((checkTimeLong.TotalDegrees >= 355.00 && checkTimeLong.TotalDegrees <= 360.00) && (nextDayLong.TotalDegrees >= 0.00 && nextDayLong.TotalDegrees <= 5.00))
                {
                    retro = false;
                }
                else
                {
                    retro = true;
                }
            }
            if (nextDayLong >= checkTimeLong)
            {
                //check if the next day long is more than checktimelong because its reverse crossing over 0.00 - this is a retro condition
                if ((checkTimeLong.TotalDegrees >= 0.00 && checkTimeLong.TotalDegrees <= 5.00) && (nextDayLong.TotalDegrees >= 355.00 && nextDayLong.TotalDegrees <= 0.00))
                {
                    retro = true;

                }
                else
                {
                    retro = false;

                }
            }
            return retro;
        }


        /// <summary>
        /// Combustion of planets: Planets when too close to the Sun become
        /// invisible and are labelled as combust. A combust planet loses its strength
        /// and tends to behave adversely according to predictive astrology. Aryabhata
        /// has the following to say about combustion:
        /// ‘When the Moon has no latitude (i.e., when it is at zero degree of
        /// latitude) it is visible when situated at a distance of 12 degrees from the Sun.
        /// Venus is visible when 9 degrees distant from the Sun. The other planets
        /// taken in the order of decreasing sizes (viz., Jupiter, Mercury, Saturn and
        /// Mars) are visible when they are 9 degrees increased by twos (i.e., when they
        /// are 11, 13, 15 and 17 degrees) distant from the Sun.’
        /// The degrees as mentioned above are generally taken as the limits within
        /// which the respective planets are said to be combust.
        /// </summary>
        /// <returns></returns>
        public static bool IsPlanetCombust()
        {
            //todo
            return false;
        }

        /// <summary>
        /// circulation time of the objects in years, used by cheshta bala calculation
        /// </summary>
        public static double PlanetCirculationTime(PlanetName planetName)
        {

            if (planetName == Library.PlanetName.Sun) { return 1.0; }
            if (planetName == Library.PlanetName.Moon) { return .082; }
            if (planetName == Library.PlanetName.Mars) { return 1.88; }
            if (planetName == Library.PlanetName.Mercury) { return .24; }
            if (planetName == Library.PlanetName.Jupiter) { return 11.86; }
            if (planetName == Library.PlanetName.Venus) { return .62; }
            if (planetName == Library.PlanetName.Saturn) { return 29.46; }

            throw new Exception("Planet circulation time not defined!");

        }

        /// <summary>
        /// Sapthavargajabala: This is the strength of a
        /// planet due to its residence in the seven sub-divisions
        /// according to its relation with the dispositor.
        ///
        /// Saptavargaja bala means the strength a
        /// planet gets by virtue of its disposition in a friendly,
        /// neutral or inimical Rasi, Hora, Drekkana, Sapthamsa,
        /// Navamsa, Dwadasamsa and Thrimsamsa.
        /// </summary>
        public static Shashtiamsa PlanetSaptavargajaBala(PlanetName planetName, Time time)
        {
            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey(nameof(PlanetSaptavargajaBala), planetName, time, Ayanamsa), _getPlanetSaptavargajaBala);


            //UNDERLYING FUNCTION
            Shashtiamsa _getPlanetSaptavargajaBala()
            {
                //declare total value
                double totalSaptavargajaBalaInShashtiamsa = 0;

                //declare empty list of planet sign relationships
                var planetSignRelationshipList = new List<PlanetToSignRelationship>();


                //get planet rasi Moolatrikona.
                var planetInMoolatrikona = Calculate.IsPlanetInMoolatrikona(planetName, time);

                //if planet is in moolatrikona
                if (planetInMoolatrikona)
                {
                    //add the relationship to the list
                    planetSignRelationshipList.Add(PlanetToSignRelationship.Moolatrikona);
                }
                else
                //else get planet's normal relationship with rasi
                {
                    //get planet rasi
                    var planetRasi = Calculate.PlanetZodiacSign(planetName, time).GetSignName();
                    var rasiSignRelationship = Calculate.PlanetRelationshipWithSign(planetName, planetRasi, time);

                    //add planet rasi relationship to list
                    planetSignRelationshipList.Add(rasiSignRelationship);
                }

                //get planet hora
                var planetHora = Calculate.PlanetHoraSigns(planetName, time).GetSignName();
                var horaSignRelationship = Calculate.PlanetRelationshipWithSign(planetName, planetHora, time);
                //add planet hora relationship to list
                planetSignRelationshipList.Add(horaSignRelationship);


                //get planet drekkana
                var planetDrekkana = Calculate.PlanetDrekkanaSign(planetName, time).GetSignName();
                var drekkanaSignRelationship = Calculate.PlanetRelationshipWithSign(planetName, planetDrekkana, time);
                //add planet drekkana relationship to list
                planetSignRelationshipList.Add(drekkanaSignRelationship);


                //get planet saptamsa
                var planetSaptamsa = Calculate.PlanetSaptamsaSign(planetName, time);
                var saptamsaSignRelationship = Calculate.PlanetRelationshipWithSign(planetName, planetSaptamsa, time);
                //add planet saptamsa relationship to list
                planetSignRelationshipList.Add(saptamsaSignRelationship);


                //get planet navamsa
                var planetNavamsa = Calculate.PlanetNavamsaSign(planetName, time);
                var navamsaSignRelationship = Calculate.PlanetRelationshipWithSign(planetName, planetNavamsa, time);
                //add planet navamsa relationship to list
                planetSignRelationshipList.Add(navamsaSignRelationship);


                //get planet dwadasamsa
                var planetDwadasamsa = Calculate.PlanetDwadasamsaSign(planetName, time);
                var dwadasamsaSignRelationship = Calculate.PlanetRelationshipWithSign(planetName, planetDwadasamsa, time);
                //add planet dwadasamsa relationship to list
                planetSignRelationshipList.Add(dwadasamsaSignRelationship);


                //get planet thrimsamsa
                var planetThrimsamsa = Calculate.PlanetThrimsamsaSign(planetName, time);
                var thrimsamsaSignRelationship = Calculate.PlanetRelationshipWithSign(planetName, planetThrimsamsa, time);
                //add planet thrimsamsa relationship to list
                planetSignRelationshipList.Add(thrimsamsaSignRelationship);


                //calculate total Saptavargaja Bala

                //loop through all the relationship
                foreach (var planetToSignRelationship in planetSignRelationshipList)
                {
                    //add relationship point accordingly

                    //A planet in its Moolatrikona is assigned a value of 45 Shashtiamsas;
                    if (planetToSignRelationship == PlanetToSignRelationship.Moolatrikona)
                    {
                        totalSaptavargajaBalaInShashtiamsa = totalSaptavargajaBalaInShashtiamsa + 45;
                    }

                    //in Swavarga 30 Shashtiamsas;
                    if (planetToSignRelationship == PlanetToSignRelationship.OwnVarga)
                    {
                        totalSaptavargajaBalaInShashtiamsa = totalSaptavargajaBalaInShashtiamsa + 30;
                    }

                    //in Adhi Mitravarga 22.5 Shashtiamsas;
                    if (planetToSignRelationship == PlanetToSignRelationship.BestFriendVarga)
                    {
                        totalSaptavargajaBalaInShashtiamsa = totalSaptavargajaBalaInShashtiamsa + 22.5;
                    }

                    //in Mitravarga 15 · Shashtiamsas;
                    if (planetToSignRelationship == PlanetToSignRelationship.FriendVarga)
                    {
                        totalSaptavargajaBalaInShashtiamsa = totalSaptavargajaBalaInShashtiamsa + 15;
                    }

                    //in Samavarga 7.5 Shashtiamsas ~
                    if (planetToSignRelationship == PlanetToSignRelationship.NeutralVarga)
                    {
                        totalSaptavargajaBalaInShashtiamsa = totalSaptavargajaBalaInShashtiamsa + 7.5;
                    }

                    //in Satruvarga 3.75 Shashtiamsas;
                    if (planetToSignRelationship == PlanetToSignRelationship.EnemyVarga)
                    {
                        totalSaptavargajaBalaInShashtiamsa = totalSaptavargajaBalaInShashtiamsa + 3.75;
                    }

                    //in Adhi Satruvarga 1.875 Shashtiamsas.
                    if (planetToSignRelationship == PlanetToSignRelationship.BitterEnemyVarga)
                    {
                        totalSaptavargajaBalaInShashtiamsa = totalSaptavargajaBalaInShashtiamsa + 1.875;
                    }

                }


                return new Shashtiamsa(totalSaptavargajaBalaInShashtiamsa);

            }

        }

        /// <summary>
        /// residence of the planet and as such a certain degree of strength or weakness attends on it
        /// 
        /// Positonal strength
        /// 
        /// A planet occupies a
        /// certain sign in a Rasi and friendly, neutrai or
        /// inimical varga~. It is either exalted or debilitated·
        /// lt ocupies its Moolathrikona or it has its own
        /// varga. All these states refer to the position or
        /// residence of the planet and as such a certain degree
        /// of strength or weakness attends on it. This strength
        /// or potency is known as the Sthanabala.
        /// 
        /// 
        /// 1.Uccha Bala:
        /// Uccha means exaltation. When a planet is placed in its highest exaltation point,
        /// it is of full strength and when it is in its deepest debilitation point, it is devoid of any strength.
        /// When in between the strength is calculated proportionately dependent on the distance these planets are
        /// placed from the highest exaltation or deepest debilitation point.
        ///
        /// 2.Sapta Vargiya Bala:
        /// Rashi, Hora, Drekkana, Saptamsha, Navamsha, Dwadasamsha and Trimsamsha constitute the Sapta Varga.
        /// The strength of the planets in these seven divisional charts based on their placements in Mulatrikona,
        /// own sign, friendly sign etc. constitute the Sapta vargiya bala.
        /// 
        /// 3.Oja-Yugma Rashi-Amsha Bala:
        /// Oja means odd signs and Yugma means even signs. Thus, as the name imply, this strength is derived from
        /// a planet’s placement in the odd or even signs in the Rashi and Navamsha.
        /// 
        /// 4.Kendradi Bala:
        /// The name itself implies how to compute this strength. A planet in a Kendra (1-4-7-10) gets full strength,
        /// while one in Panapara (2-5-8-11) gets half and the one in Apoklimas (12-3-6-9) gets quarter strength.
        ///
        /// 5.Drekkana Bala:
        /// Due to placement in first, second, or third Drekkana of a sign, male, female and hermaphrodite planets respectively,
        /// get a quarter strength according to placements in the first, second and third Drekkana.
        /// </summary>
        public static Shashtiamsa PlanetSthanaBala(PlanetName planetName, Time time)
        {
            //no calculation for rahu and ketu here
            var isRahu = planetName.Name == Library.PlanetName.PlanetNameEnum.Rahu;
            var isKetu = planetName.Name == Library.PlanetName.PlanetNameEnum.Ketu;
            var isRahuKetu = isRahu || isKetu;
            if (isRahuKetu) { return Shashtiamsa.Zero; }

            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey(nameof(PlanetSthanaBala), planetName, time, Ayanamsa), _getPlanetSthanaBala);


            //UNDERLYING FUNCTION
            Shashtiamsa _getPlanetSthanaBala()
            {
                //Get Ochcha Bala (exaltation strength)
                var ochchaBala = PlanetOchchaBala(planetName, time);

                //Get Saptavargaja Bala
                var saptavargajaBala = PlanetSaptavargajaBala(planetName, time);

                //Get Ojayugmarasyamsa Bala
                var ojayugmarasymsaBala = PlanetOjayugmarasyamsaBala(planetName, time);

                //Get Kendra Bala
                var kendraBala = PlanetKendraBala(planetName, time);

                //Drekkana Bala
                var drekkanaBala = PlanetDrekkanaBala(planetName, time);

                //Total Sthana Bala
                var totalSthanaBala = ochchaBala + saptavargajaBala + ojayugmarasymsaBala + kendraBala + drekkanaBala;

                return totalSthanaBala;

            }

        }

        /// <summary>
        /// Drekkanabala: The Sun, Jupiter and Mars
        /// in the lst ; Saturn and Mercury in the 2nd ; and
        /// the Moon and Venus in the last Drekkana, get full
        /// strength of 60 shashtiamsas.
        /// </summary>
        public static Shashtiamsa PlanetDrekkanaBala(PlanetName planetName, Time time)
        {
            //get sign planet is in
            var planetSign = Calculate.PlanetZodiacSign(planetName, time);

            //get degrees in sign 
            var degreesInSign = planetSign.GetDegreesInSign().TotalDegrees;

            //if male planet -Ravi, Guru and Kuja.
            if (planetName == Library.PlanetName.Sun || planetName == Library.PlanetName.Jupiter || planetName == Library.PlanetName.Mars)
            {
                //if planet is in 1st drekkana
                if (degreesInSign >= 0 && degreesInSign <= 10.0)
                {
                    //return 15 bala
                    return new Shashtiamsa(15);
                }

            }

            //if Hermaphrodite Planets.-Sani and Budba
            if (planetName == Library.PlanetName.Saturn || planetName == Library.PlanetName.Mercury)
            {
                //if planet is in 2nd drekkana
                if (degreesInSign > 10 && degreesInSign <= 20.0)
                {
                    //return 15 bala
                    return new Shashtiamsa(15);
                }

            }

            //if Female Planets.-Chandra and Sukra
            if (planetName == Library.PlanetName.Moon || planetName == Library.PlanetName.Venus)
            {
                //if planet is in 3rd drekkana
                if (degreesInSign > 20 && degreesInSign <= 30.0)
                {
                    //return 15 bala
                    return new Shashtiamsa(15);
                }
            }

            //if none above conditions met return 0 bala
            return new Shashtiamsa(0);
        }

        /// <summary>
        /// Kendrtzbala: Planets in Kendras get 60
        /// shashtiamsas; in Panapara 30, and in Apoklima 15.
        /// </summary>
        public static Shashtiamsa PlanetKendraBala(PlanetName planetName, Time time)
        {
            //get number of the sign planet is in
            var planetSignNumber = (int)Calculate.PlanetZodiacSign(planetName, time).GetSignName();

            //A planet in a kendra sign  gets 60 Shashtiamsas as its strength ;
            //Quadrants.-Kendras-1 (Ar, 4, 7 and 10.
            if (planetSignNumber == 1 || planetSignNumber == 4 || planetSignNumber == 7 || planetSignNumber == 10)
            {
                return new Shashtiamsa(60);
            }

            //in a Panapara sign 30 Shashtiamsas;
            //-Panaparas-2, 5, 8 and 11.
            if (planetSignNumber == 2 || planetSignNumber == 5 || planetSignNumber == 8 || planetSignNumber == 11)
            {
                return new Shashtiamsa(30);
            }


            //and in an Apoklima sign 15 Shashtiamsas.
            //Apoklimas-3, 6, 9 and 12 {9th being a trikona must be omitted).
            if (planetSignNumber == 3 || planetSignNumber == 6 || planetSignNumber == 9 || planetSignNumber == 12)
            {
                return new Shashtiamsa(15);
            }


            throw new Exception("Kendra Bala not found, error");
        }

        /// <summary>
        /// Ojayugmarasyamsa: In odd Rasi and Navamsa,
        /// the Sun, Mars, Jupiter, Mercury and Saturn
        /// get strength and the rest in even signs
        /// </summary>
        public static Shashtiamsa PlanetOjayugmarasyamsaBala(PlanetName planetName, Time time)
        {
            //get planet rasi sign
            var planetRasiSign = Calculate.PlanetZodiacSign(planetName, time).GetSignName();

            //get planet navamsa sign
            var planetNavamsaSign = Calculate.PlanetNavamsaSign(planetName, time);

            //declare total Ojayugmarasyamsa Bala as 0 first
            double totalOjayugmarasyamsaBalaInShashtiamsas = 0;

            //if planet is the moon or venus
            if (planetName == Library.PlanetName.Moon || planetName == Library.PlanetName.Venus)
            {
                //if rasi sign is an even sign
                if (Calculate.IsEvenSign(planetRasiSign))
                {
                    //add 15 Shashtiamsas
                    totalOjayugmarasyamsaBalaInShashtiamsas += 15;
                }

                //if navamsa sign is an even sign
                if (Calculate.IsEvenSign(planetNavamsaSign))
                {
                    //add 15 Shashtiamsas
                    totalOjayugmarasyamsaBalaInShashtiamsas += 15;
                }

            }
            //if planet is Sun, Mars, Jupiter, Mercury and Saturn
            else if (planetName == Library.PlanetName.Sun || planetName == Library.PlanetName.Mars ||
                     planetName == Library.PlanetName.Jupiter || planetName == Library.PlanetName.Mercury || planetName == Library.PlanetName.Saturn)
            {
                //if rasi sign is an odd sign
                if (Calculate.IsOddSign(planetRasiSign))
                {
                    //add 15 Shashtiamsas
                    totalOjayugmarasyamsaBalaInShashtiamsas += 15;
                }

                //if navamsa sign is an odd sign
                if (Calculate.IsOddSign(planetNavamsaSign))
                {
                    //add 15 Shashtiamsas
                    totalOjayugmarasyamsaBalaInShashtiamsas += 15;
                }

            }

            return new Shashtiamsa(totalOjayugmarasyamsaBalaInShashtiamsas);
        }

        /// <summary>
        /// Gets a planet's Kala Bala or Temporal strength
        /// </summary>
        public static Shashtiamsa PlanetKalaBala(PlanetName planetName, Time time)
        {
            //no calculation for rahu and ketu here
            var isRahu = planetName.Name == Library.PlanetName.PlanetNameEnum.Rahu;
            var isKetu = planetName.Name == Library.PlanetName.PlanetNameEnum.Ketu;
            var isRahuKetu = isRahu || isKetu;
            if (isRahuKetu) { return Shashtiamsa.Zero; }



            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey(nameof(PlanetKalaBala), planetName, time, Ayanamsa), _getPlanetKalaBala);


            //UNDERLYING FUNCTION
            Shashtiamsa _getPlanetKalaBala()
            {
                //place to store pre kala bala values
                var kalaBalaList = new Dictionary<PlanetName, Shashtiamsa>();

                //Yuddha Bala requires all planet's pre kala bala
                //so calculate pre kala bala for all planets first
                foreach (var planet in Library.PlanetName.All7Planets)
                {
                    //calculate pre kala bala
                    var preKalaBala = GetPreKalaBala(planet, time);

                    //store in a list sorted by planet name, to be accessed later
                    kalaBalaList.Add(planet, preKalaBala);
                }

                //calculate Yuddha Bala
                var yuddhaBala = PlanetYuddhaBala(planetName, kalaBalaList, time);

                //Total Kala Bala
                var total = kalaBalaList[planetName] + yuddhaBala;

                return total;

                //---------------FUNCTIONS--------------
                Shashtiamsa GetPreKalaBala(PlanetName planetName, Time time)
                {
                    //Nathonnatha Bala
                    var nathonnathaBala = PlanetNathonnathaBala(planetName, time);

                    //Paksha Bala
                    var pakshaBala = PlanetPakshaBala(planetName, time);

                    //Tribhaga Bala
                    var tribhagaBala = PlanetTribhagaBala(planetName, time);

                    //Abda Bala
                    var abdaBala = PlanetAbdaBala(planetName, time);

                    //Masa Bala
                    var masaBala = PlanetMasaBala(planetName, time);

                    //Vara Bala
                    var varaBala = PlanetVaraBala(planetName, time);

                    //Hora Bala
                    var horaBala = PlanetHoraBala(planetName, time);

                    //Ayana Bala
                    var ayanaBala = PlanetAyanaBala(planetName, time);

                    //combine all the kala bala calculated so far, and return the value
                    var preKalaBala = nathonnathaBala + pakshaBala + tribhagaBala + abdaBala + masaBala + varaBala + horaBala +
                                      ayanaBala;

                    return preKalaBala;
                }
            }

        }

        /// <summary>
        /// Two planets are said to be in Yuddha or fight when they are in
        /// conjunction and the distance between them is less than one degree.
        /// TODO Not fully tested
        ///
        /// Yuddhabala : All planets excepting the Sun
        /// and the Moon enter into war when two planets are
        /// in the same degree. The pJanet having the lesser
        /// longitude is the winner. Find out the sum total of
        /// the SthanabaJa, Kalabala and Digbala of these two'
        /// planets. Difference between the two, divided by
        /// the difference of their diameters of its disc, gives
        /// the Yuddhabala. Add this to the victorious planet
        /// and dedu_ct it from the vanquished.
        /// </summary>
        public static Shashtiamsa PlanetYuddhaBala(PlanetName inputedPlanet, Dictionary<PlanetName, Shashtiamsa> preKalaBalaValues, Time time)
        {
            //All the planets excepting Sun and Moon may enter into war (Yuddha)
            if (inputedPlanet == Library.PlanetName.Moon || inputedPlanet == Library.PlanetName.Sun) { return Shashtiamsa.Zero; }


            //place to store winner & loser balas
            var yudhdhabala = new Dictionary<PlanetName, Shashtiamsa>();


            //get all planets that are conjunct with inputed planet
            var conjunctPlanetList = Calculate.PlanetsInConjuction(inputedPlanet, time);

            //remove rahu & kethu if present, they are not included in Yuddha Bala calculations
            conjunctPlanetList.RemoveAll(pl => pl == Library.PlanetName.Rahu || pl == Library.PlanetName.Ketu);


            foreach (var checkingPlanet in conjunctPlanetList)
            {

                //All the planets excepting Sun and Moon may enter into war (Yuddha)
                //no need to calculate Yuddha, move to next planet, if sun or moon
                if (checkingPlanet == Library.PlanetName.Moon || checkingPlanet == Library.PlanetName.Sun) { continue; }


                //get distance between conjunct planet & inputed planet
                var inputedPlanetLong = Calculate.PlanetNirayanaLongitude(inputedPlanet, time);
                var checkingPlanetLong = Calculate.PlanetNirayanaLongitude(checkingPlanet, time);
                var distance = Calculate.DistanceBetweenPlanets(inputedPlanetLong, checkingPlanetLong);


                //if the distance between them is less than one degree
                if (distance < Angle.FromDegrees(1))
                {
                    PlanetName winnerPlanet = null;
                    PlanetName losserPlanet = null;

                    //The conquering planet is the one whose longitude is less.
                    if (inputedPlanetLong < checkingPlanetLong) { winnerPlanet = inputedPlanet; losserPlanet = checkingPlanet; } //inputed planet won
                    else if (inputedPlanetLong > checkingPlanetLong) { winnerPlanet = checkingPlanet; losserPlanet = inputedPlanet; } //checking planet won
                    else if (inputedPlanetLong == checkingPlanetLong)
                    {
                        //unlikely chance, log error & set inputed planet as winner (random)
                        LogManager.Error($"Planets same longitude! Not expected, random result used!");
                        winnerPlanet = inputedPlanet; losserPlanet = checkingPlanet;
                    }

                    //When two planets are in war, get the sum of the various Balas, viv., Sthanabala, the
                    // Dikbala and the Kalabala (up to Horabala) described hitherto of the fighting planets. Find out the
                    // difference between these two sums.
                    var shadbaladiff = Math.Abs(preKalaBalaValues[inputedPlanet].ToDouble() - preKalaBalaValues[checkingPlanet].ToDouble());


                    //Divide shadbala difference by the difference between the diameters of the discs of the two fighting planets
                    var diameterDifference = PlanetDiscDiameter(inputedPlanet).GetDifference(PlanetDiscDiameter(checkingPlanet));


                    //And the resulting quotient which is the Yuddhabala (Shashtiamsa) must be added to the total of the Kalabala (detailed
                    // hitherto) of the victorious planet and must be subtracted from the total Kalabala of the vanquished planet.
                    var shadbala = diameterDifference.TotalDegrees / shadbaladiff;

                    yudhdhabala[winnerPlanet] = new Shashtiamsa(shadbala);
                    yudhdhabala[losserPlanet] = new Shashtiamsa(-shadbala);

                }


            }


            //return yudhdhabala if it was calculated else, return 0 
            var found = yudhdhabala.TryGetValue(inputedPlanet, out var bala);
            return found ? bala : Shashtiamsa.Zero;




            //-----------FUNCTIONS----------------


        }

        /// <summary>
        /// Bimba Parimanas -This means the diameters of the discs of the planets.
        /// </summary>
        static Angle PlanetDiscDiameter(PlanetName planet)
        {
            if (planet == Library.PlanetName.Mars) { return new Angle(0, 9, 4); }
            if (planet == Library.PlanetName.Mercury) { return new Angle(0, 6, 6); }
            if (planet == Library.PlanetName.Jupiter) { return new Angle(0, 190, 4); }
            if (planet == Library.PlanetName.Venus) { return new Angle(0, 16, 6); }
            if (planet == Library.PlanetName.Saturn) { return new Angle(0, 158, 0); }

            //control should not come here, report error
            throw new Exception("Disc diameter now found!");
        }

        /// <summary>
        /// Ayanabala : All planets get 30 shasbtiamsas
        /// at the equator. For the Sun, Jupiter, Mars
        /// and Venus add proportionately when they are in
        /// northern course and for the Moon and Saturn when
        /// in southern course. Deduct proportionately when
        /// they are in the opposite direction. Unit of strength
        /// is 60 shashtiamsas.
        ///
        /// 
        /// TODO some values for calculation with standard hooscope out of whack,
        /// it seems small differences in longitude seem magnified at final value,
        /// not 100% sure, need further testing for confirmation, but final values seem ok so far
        /// </summary>
        public static Shashtiamsa PlanetAyanaBala(PlanetName planetName, Time time)
        {

            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey(nameof(PlanetAyanaBala), planetName, time, Ayanamsa), _getPlanetAyanaBala);


            //UNDERLYING FUNCTION
            Shashtiamsa _getPlanetAyanaBala()
            {
                double ayanaBala = 0;

                //get plant kranti (negative south, positive north)
                var kranti = PlanetDeclination(planetName, time);

                //prepare values for calculation of ayanabala
                var x = Angle.FromDegrees(24);
                var isNorthDeclination = kranti < 0 ? false : true;

                //get declination without negative (absolute value), easier for calculation
                var absKranti = Math.Abs((double)kranti);

                //In case of Sukra, Ravi, Kuja and Guru their north declinations are
                //additive and south declinations are subtractive
                if (planetName == Library.PlanetName.Venus || planetName == Library.PlanetName.Sun || planetName == Library.PlanetName.Mars || planetName == Library.PlanetName.Jupiter)
                {
                    //additive
                    if (isNorthDeclination) { ayanaBala = ((24 + absKranti) / 48) * 60; }

                    //subtractive
                    else { ayanaBala = ((24 - absKranti) / 48) * 60; }

                    //And double the Ayanabala in the case of the Sun
                    if (planetName == Library.PlanetName.Sun) { ayanaBala = ayanaBala * 2; }

                }
                //In case of Sani and Chandra, their south declinations are additive while their
                //north declinations are subtractive.
                else if (planetName == Library.PlanetName.Saturn || planetName == Library.PlanetName.Moon)
                {
                    //additive
                    if (!isNorthDeclination) { ayanaBala = ((24 + absKranti) / 48) * 60; }

                    //subtractive
                    else { ayanaBala = ((24 - absKranti) / 48) * 60; }
                }
                //For Budha the declination, north or south, is always additive.
                else if (planetName == Library.PlanetName.Mercury)
                {
                    ayanaBala = ((24 + absKranti) / 48) * 60;
                }


                return new Shashtiamsa(ayanaBala);

            }


        }

        /// <summary>
        /// A heavenly body moves northwards the equator for sometime and
        /// then gets southwards. This angular distance from
        /// the equinoctial or celestial equator is Kranti or the
        /// declination.
        ///
        /// Declinations are reckoned plus or minus according as the planet
        /// is situated in the northern or southern celestial hemisphere
        /// </summary>
        public static double PlanetDeclination(PlanetName planetName, Time time)
        {

            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey(nameof(PlanetDeclination), planetName, time, Ayanamsa), _getPlanetDeclination);


            //UNDERLYING FUNCTION
            double _getPlanetDeclination()
            {
                //for degree to radian conversion
                const double DEG2RAD = 0.0174532925199433;

                var eps = EclipticObliquity(time);

                var tlen = Calculate.PlanetSayanaLongitude(planetName, time);
                var lat = Calculate.PlanetSayanaLatitude(planetName, time);

                //if kranti (declination), is a negative number, it means south, else north of equator
                var kranti = lat.TotalDegrees + eps * Math.Sin(DEG2RAD * tlen.TotalDegrees);

                return kranti;
            }

        }

        /// <summary>
        /// true obliquity of the Ecliptic (includes nutation)
        /// </summary>
        public static double EclipticObliquity(Time time)
        {

            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey(nameof(EclipticObliquity), time, Ayanamsa), _getPlanetEps);


            //UNDERLYING FUNCTION
            double _getPlanetEps()
            {
                double eps;

                string err = "";
                double[] x = new double[6];

                SwissEph ephemeris = new SwissEph();

                // Convert DOB to ET
                var jul_day_ET = Calculate.TimeToEphemerisTime(time);

                //ephemeris.swe_calc(jul_day_ET, SwissEph.SE_ECL_NUT, 0, x, ref err);

                ephemeris.swe_calc(jul_day_ET, SwissEph.SE_ECL_NUT, 0, x, ref err);

                eps = x[0];

                return eps;
            }

        }

        /// <summary>
        /// Hora Bala AKA Horadhipathi Bala
        /// </summary>
        public static Shashtiamsa PlanetHoraBala(PlanetName planetName, Time time)
        {

            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey(nameof(PlanetHoraBala), planetName, time, Ayanamsa), _getPlanetHoraBala);


            //UNDERLYING FUNCTION
            Shashtiamsa _getPlanetHoraBala()
            {
                //first ascertain the weekday of birth
                var birthWeekday = Calculate.DayOfWeek(time);

                //ascertain the number of hours elapsed from sunrise to birth
                //This shows the number of horas passed.
                var hora = Calculate.HoraAtBirth(time);

                //get lord of hora (hour)
                var lord = Calculate.LordOfHoraFromWeekday(hora, birthWeekday);

                //planet inputed is lord of hora, then 60 shashtiamsas
                if (lord == planetName)
                {
                    return new Shashtiamsa(60);
                }
                else
                {
                    return Shashtiamsa.Zero;
                }

            }



        }

        /// <summary>
        /// The planet who is the king of
        /// the year of birth is assigned a value of 15 Shashtiamsas
        /// as his Abdabala.
        /// </summary>
        public static Shashtiamsa PlanetAbdaBala(PlanetName planetName, Time time)
        {
            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey(nameof(PlanetAbdaBala), planetName, time, Ayanamsa), _getPlanetAbdaBala);


            //UNDERLYING FUNCTION
            Shashtiamsa _getPlanetAbdaBala()
            {
                //calculate year lord
                dynamic yearAndMonthLord = YearAndMonthLord(time);
                PlanetName yearLord = yearAndMonthLord.YearLord;

                //if inputed planet is year lord than 15 Shashtiamsas
                if (yearLord == planetName) { return new Shashtiamsa(15); }

                //not year lord, 0 Shashtiamsas
                return Shashtiamsa.Zero;
            }


        }

        /// <summary>
        /// Gets a planet's masa bala
        /// the lord of the month of birth is assigned a value of 30 Shashtiamsas as his Masabala
        /// </summary>
        public static Shashtiamsa PlanetMasaBala(PlanetName planetName, Time time)
        {
            //The planet who is the lord of
            //the month of birth is assigned a value of 30 Shashtiamsas
            //as his Masabala.

            //calculate month lord
            dynamic yearAndMonthLord = YearAndMonthLord(time);
            PlanetName monthLord = yearAndMonthLord.MonthLord;

            //if inputed planet is month lord than 30 Shashtiamsas
            if (monthLord == planetName) { return new Shashtiamsa(30); }

            //not month lord, 0 Shashtiamsas
            return Shashtiamsa.Zero;
        }

        public static Shashtiamsa PlanetVaraBala(PlanetName planetName, Time time)
        {
            //The planet who is the lord of
            //the day of birth is assigned a value of 45 Shashtiamsas
            //as his Varabala.

            //calculate day lord
            PlanetName dayLord = Calculate.LordOfWeekday(time);

            //if inputed planet is day lord than 45 Shashtiamsas
            if (dayLord == planetName) { return new Shashtiamsa(45); }

            //not day lord, 0 Shashtiamsas
            return Shashtiamsa.Zero;

        }

        /// <summary>
        /// Gets year & month lord at inputed time
        /// </summary>
        public static object YearAndMonthLord(Time time)
        {
            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey(nameof(YearAndMonthLord), time, Ayanamsa), _getYearAndMonthLord);


            //UNDERLYING FUNCTION
            object _getYearAndMonthLord()
            {
                //set default
                var yearLord = Library.PlanetName.Sun;
                var monthLord = Library.PlanetName.Sun;

                //initialize ephemeris
                using SwissEph ephemeris = new SwissEph();

                double ut_arghana = ephemeris.swe_julday(1827, 5, 2, 0, SwissEph.SE_GREG_CAL);
                double ut_noon = Calculate.GreenwichLmtInJulianDays(time);

                double diff = ut_noon - ut_arghana;
                if (diff >= 0)
                {
                    double quo = Math.Floor(diff / 360.0);
                    diff -= quo * 360.0;
                }
                else
                {
                    double pdiff = -diff;
                    double quo = Math.Ceiling(pdiff / 360.0);
                    diff += quo * 360.0;
                }

                double diff_year = diff;
                while (diff > 30.0) diff -= 30.0;
                double diff_month = diff;
                while (diff > 7) diff -= 7.0;

                var yearLordRaw = ephemeris.swe_day_of_week(ut_noon - diff_year);
                var monthLordRaw = ephemeris.swe_day_of_week(ut_noon - diff_month);

                //parse raw weekday
                var yearWeekday = swissEphWeekDayToMuhurthaDay(yearLordRaw);
                var monthWeekday = swissEphWeekDayToMuhurthaDay(monthLordRaw);


                //Abdadbipat : the planet that rules over the weekday on which the year begins (hindu year)
                yearLord = Calculate.LordOfWeekday(yearWeekday);

                //Masadhipath : The planet that rules the weekday of the commencement of the month of the birth
                monthLord = Calculate.LordOfWeekday(monthWeekday);

                //package year & month lord together & return
                return new { YearLord = yearLord, MonthLord = monthLord };


                //---------------------FUNCTION--------------------

                //converts swiss epehmeris weekday numbering to muhurtha weekday numbering
                DayOfWeek swissEphWeekDayToMuhurthaDay(int dayNumber)
                {
                    switch (dayNumber)
                    {
                        case 0: return Library.DayOfWeek.Monday;
                        case 1: return Library.DayOfWeek.Tuesday;
                        case 2: return Library.DayOfWeek.Wednesday;
                        case 3: return Library.DayOfWeek.Thursday;
                        case 4: return Library.DayOfWeek.Friday;
                        case 5: return Library.DayOfWeek.Saturday;
                        case 6: return Library.DayOfWeek.Sunday;
                        default: throw new Exception("Invalid day number!");
                    }
                }

            }

        }

        /// <summary>
        /// Thribhagabala : Mercury, the Sun and
        /// Saturn get 60 shashtiamsas each, during the lst,
        /// 2nd and 3rd one-third positions of the day, respectively.
        /// The Moon, Venus and Mars govern the lst, 2nd and 3rd one-third portion of the night
        /// respectively. Jupiter is always strong and gets 60
        /// shashtiamsas of strength.
        /// </summary>
        public static Shashtiamsa PlanetTribhagaBala(PlanetName planetName, Time time)
        {
            PlanetName ret = Library.PlanetName.Jupiter;

            var sunsetTime = Calculate.SunsetTime(time);

            if (IsDayBirth(time))
            {
                //find out which part of the day birth took place

                var sunriseTime = Calculate.SunriseTime(time);

                //substraction should always return a positive number, since sunset is always after sunrise
                double lengthHours = (sunsetTime.Subtract(sunriseTime).TotalHours) / 3;
                double offset = time.Subtract(sunriseTime).TotalHours;
                int part = (int)(Math.Floor(offset / lengthHours));
                switch (part)
                {
                    case 0: ret = Library.PlanetName.Mercury; break;
                    case 1: ret = Library.PlanetName.Sun; break;
                    case 2: ret = Library.PlanetName.Saturn; break;
                }
            }
            else
            {
                //get sunrise time at on next day to get duration of the night
                var nextDayTime = time.AddHours(24);
                var nextDaySunrise = Calculate.SunriseTime(nextDayTime);

                double lengthHours = (nextDaySunrise.Subtract(sunsetTime).TotalHours) / 3;
                double offset = time.Subtract(sunsetTime).TotalHours;
                int part = (int)(Math.Floor(offset / lengthHours));
                switch (part)
                {
                    case 0: ret = Library.PlanetName.Moon; break;
                    case 1: ret = Library.PlanetName.Venus; break;
                    case 2: ret = Library.PlanetName.Mars; break;
                }
            }

            //Always assign a value of 60 Shashtiamsas
            //to Guru irrespective of whether birth is during
            //night or during day.
            if (planetName == Library.PlanetName.Jupiter || planetName == ret) { return new Shashtiamsa(60); }

            return new Shashtiamsa(0);
        }

        /// <summary>
        /// Oochchabala : The distance between the
        /// planet's longitude and its debilitation point, divided
        /// by 3, gives its exaltation strength or oochchabaJa.
        /// </summary>
        public static Shashtiamsa PlanetOchchaBala(PlanetName planetName, Time time)
        {
            //1.0 Get Planet longitude
            var planetLongitude = Calculate.PlanetNirayanaLongitude(planetName, time);

            //2.0 Get planet debilitation point
            var planetDebilitationPoint = Calculate.PlanetDebilitationPoint(planetName);
            //convert to planet longitude
            var debilitationLongitude = LongitudeAtZodiacSign(planetDebilitationPoint);

            //3.0 Get difference between planet longitude & debilitation point
            //var difference = planetLongitude.GetDifference(planetDebilitationPoint); //todo need checking
            var difference = DistanceBetweenPlanets(planetLongitude, debilitationLongitude);

            //4.0 If difference is more than 180 degrees
            if (difference.TotalDegrees > 180)
            {
                //get the difference of it with 360 degrees
                //difference = difference.GetDifference(Angle.Degrees360);
                difference = Calculate.DistanceBetweenPlanets(difference, Angle.Degrees360);

            }

            //5.0 Divide difference with 3 to get ochchabala
            var ochchabalaInShashtiamsa = difference.TotalDegrees / 3;

            //return value in shashtiamsa type
            return new Shashtiamsa(ochchabalaInShashtiamsa);
        }

        /// <summary>
        /// Pakshabala : When the Moon is waxing,
        /// take the distance from the Sun to the Moon and
        /// divide it by 3. The quotient is the Pakshabala.
        /// When the Moon is waning, take the distance from
        /// the Moon to the· Sun, and divide it by 3 for assessing
        /// Pakshabala. Moon, Jupiter, Venus and
        /// Mercury are strong in Sukla Paksba and the others
        /// in Krishna Paksha.
        ///
        /// Note: Mercury is benefic or malefic based on planets conjunct with it
        /// </summary>
        public static Shashtiamsa PlanetPakshaBala(PlanetName planetName, Time time)
        {
            double pakshaBala = 0;

            //get moon phase
            var moonPhase = Calculate.LunarDay(time).GetMoonPhase();

            var sunLongitude = Calculate.PlanetNirayanaLongitude(Library.PlanetName.Sun, time);
            var moonLongitude = Calculate.PlanetNirayanaLongitude(Library.PlanetName.Moon, time);

            //var differenceBetweenMoonSun = moonLongitude.GetDifference(sunLongitude);
            var differenceBetweenMoonSun = Calculate.DistanceBetweenPlanets(moonLongitude, sunLongitude);

            //When Moon's Long.-Sun's Long. exceeds 180, deduct it from 360°
            if (differenceBetweenMoonSun.TotalDegrees > 180)
            {
                differenceBetweenMoonSun = Calculate.DistanceBetweenPlanets(differenceBetweenMoonSun, Angle.Degrees360);
            }

            double pakshaBalaOfSubhas = 0;

            //get paksha Bala Of Subhas
            if (moonPhase == MoonPhase.BrightHalf)
            {
                //If birth has occurred during Sukla Paksha subtract the Sun's longitude from that of the Moon· Divide the
                // balance by 3. The result represents the Paksha Bala of Subhas.
                pakshaBalaOfSubhas = differenceBetweenMoonSun.TotalDegrees / 3.0;
            }
            else if (moonPhase == MoonPhase.DarkHalf)
            {
                //Subtract the remainder again from 360 degrees and divide the balance divide 3
                var totalDegrees = Calculate.DistanceBetweenPlanets(differenceBetweenMoonSun, Angle.Degrees360).TotalDegrees;
                pakshaBalaOfSubhas = totalDegrees / 3.0;
            }

            //60 Shashtiamsas diminished by paksha Bala Of Subhas gives the Paksha Bala of Papas
            var pakshaBalaOfPapas = 60.0 - pakshaBalaOfSubhas;

            //if planet is malefic
            var planetIsMalefic = Calculate.IsPlanetMalefic(planetName, time);
            var planesIsBenefic = Calculate.IsPlanetBenefic(planetName, time);

            if (planesIsBenefic == true && planetIsMalefic == false)
            {
                pakshaBala = pakshaBalaOfSubhas;
            }

            if (planesIsBenefic == false && planetIsMalefic == true)
            {
                pakshaBala = pakshaBalaOfPapas;
            }

            //Chandra's Paksha Bala is always to be doubled
            if (planetName == Library.PlanetName.Moon)
            {
                pakshaBala = pakshaBala * 2.0;
            }

            //if paksha bala is 0
            if (pakshaBala == 0)
            {
                //raise error
                throw new Exception("Paksha bala not found, error!");
            }

            return new Shashtiamsa(pakshaBala);
        }

        /// <summary>
        /// Nathonnathabala: Midnight to midday,
        /// the Sun, Jupiter and Venus gain strength proportionately
        /// till they get maximum at zenith. The other
        /// planets, except Mercury. a,re gaining strength from
        /// midday to midnight proportionately. In the same
        /// way, Mercury is always strong and gets 60 shashtiamsas.
        /// </summary>
        public static Shashtiamsa PlanetNathonnathaBala(PlanetName planetName, Time time)
        {

            //no calculation for rahu and ketu here
            var isRahu = planetName.Name == Library.PlanetName.PlanetNameEnum.Rahu;
            var isKetu = planetName.Name == Library.PlanetName.PlanetNameEnum.Ketu;
            var isRahuKetu = isRahu || isKetu;
            if (isRahuKetu) { return Shashtiamsa.Zero; }


            //get local apparent time
            var localApparentTime = Calculate.LocalApparentTime(time);

            //convert birth time (reckoned from midnight) into degrees at 15 degrees per hour
            var hour = localApparentTime.Hour;
            var minuteInHours = localApparentTime.Minute / 60.0;
            var secondInHours = localApparentTime.Second / 3600.0;
            //total hours
            var totalTimeInHours = hour + minuteInHours + secondInHours;

            //convert hours to degrees
            const double degreesPerHour = 15;
            var birthTimeInDegrees = totalTimeInHours * degreesPerHour;

            //if birth time in degrees exceeds 180 degrees subtract it from 360
            if (birthTimeInDegrees > 180)
            {
                birthTimeInDegrees = 360 - birthTimeInDegrees;
            }

            if (planetName == Library.PlanetName.Sun || planetName == Library.PlanetName.Jupiter || planetName == Library.PlanetName.Venus)
            {
                var divBala = birthTimeInDegrees / 3;

                return new Shashtiamsa(divBala);
            }

            if (planetName == Library.PlanetName.Saturn || planetName == Library.PlanetName.Moon || planetName == Library.PlanetName.Mars)
            {
                var ratriBala = (180 - birthTimeInDegrees) / 3;

                return new Shashtiamsa(ratriBala);
            }

            if (planetName == Library.PlanetName.Mercury)
            {
                //Budha has always a Divaratri Bala of 60 Shashtiamsas
                return new Shashtiamsa(60);

            }

            throw new Exception("Planet Nathonnatha Bala not found, error!");
        }

        /// <summary>
        /// Gets Dig Bala of a planet.
        /// Jupiter and Mercury are strong in Lagna (Ascendant),
        /// the Sun and Mars in the 10th, Saturn in
        /// the 7th and the Moon and Venus in the 4th. The
        /// opposite houses are weak , points. Divide the
        /// distance between the longitude of the planet and
        /// its depression point by 3. Quotient is the strength.
        /// </summary>
        public static Shashtiamsa PlanetDigBala(PlanetName planetName, Time time)
        {
            try
            {
                //no calculation for rahu and ketu here
                var isRahu = planetName.Name == PlanetNameEnum.Rahu;
                var isKetu = planetName.Name == PlanetNameEnum.Ketu;
                var isRahuKetu = isRahu || isKetu;
                if (isRahuKetu) { return Shashtiamsa.Zero; }


                //get planet longitude
                var planetLongitude = PlanetNirayanaLongitude(planetName, time);

                //
                Angle powerlessPointLongitude = null;
                House powerlessHouse;


                //subtract the longitude of the 4th house from the longitudes of the Sun and Mars.
                if (planetName == Sun || planetName == Mars)
                {
                    powerlessHouse = HouseLongitude(HouseName.House4, time);
                    powerlessPointLongitude = powerlessHouse.GetMiddleLongitude();
                }

                //Subtract the 7th house, from Jupiter and Mercury.
                if (planetName == Jupiter || planetName == Mercury)
                {
                    powerlessHouse = HouseLongitude(HouseName.House7, time);
                    powerlessPointLongitude = powerlessHouse.GetMiddleLongitude();
                }

                //Subtracc the 10th house from Venus and the Moon
                if (planetName == Venus || planetName == Moon)
                {
                    powerlessHouse = HouseLongitude(HouseName.House10, time);
                    powerlessPointLongitude = powerlessHouse.GetMiddleLongitude();
                }

                //from Saturn, the ascendant.
                if (planetName == Saturn)
                {
                    powerlessHouse = HouseLongitude(HouseName.House1, time);
                    powerlessPointLongitude = powerlessHouse.GetMiddleLongitude();
                }

                //get Digbala arc
                //Digbala arc= planet's long. - its powerless cardinal point.
                //var digBalaArc = planetLongitude.GetDifference(powerlessPointLongitude);
                var xxx = powerlessPointLongitude.TotalDegrees == null ? Angle.Zero : powerlessPointLongitude;
                var digBalaArc = DistanceBetweenPlanets(planetLongitude, xxx);

                //If difference is more than 180° 
                if (digBalaArc > Angle.Degrees180)
                {
                    //subtract it from 360 degrees.
                    //digBalaArc = digBalaArc.GetDifference(Angle.Degrees360);
                    digBalaArc = DistanceBetweenPlanets(digBalaArc, Angle.Degrees360);
                }

                //The Digbala arc of a ptanet, divided by 3, gives the Digbala
                var digBala = digBalaArc.TotalDegrees / 3;



                return new Shashtiamsa(digBala);
            }
            catch (Exception e)
            {
                //print the error and for server guys
                Console.WriteLine(e);

                //continue without a word
                return Shashtiamsa.Zero;
            }

        }

        /// <summary>
        /// Bhava Bala.-Bhava means house and
        /// Bala means strength. Bhava Bala is the potency or
        /// strength of the house or bhava or signification. We
        /// have already seen that there are 12 bhavas which
        /// comprehend all human events. Each bhava signifies
        /// or indicates certain events or functions. For
        /// instance, the first bhava represents Thanu or body,
        /// the appearance of the individual, his complexion,
        /// his disposition, his stature, etc.
        ///
        /// If it attains certain strength, the native will enjoy the indications of
        /// the bhava fully, otherwise he will not sufficiently
        /// enjoy them. The strength of a bhava is composed
        /// of three factors, viz., (1) Bhavadhipathi Bala,
        /// (2) Bhava Digbala, (3) Bhava Drishti Bala.
        /// </summary>
        public static Shashtiamsa HouseStrength(HouseName inputHouse, Time time)
        {

            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey(nameof(HouseStrength), inputHouse, time, Ayanamsa), _getBhavabala);


            //UNDERLYING FUNCTION
            Shashtiamsa _getBhavabala()
            {
                //get all the sub-strengths into a list 
                var subStrengthList = new List<HouseSubStrength>();

                subStrengthList.Add(BhavaAdhipathiBala(time));
                subStrengthList.Add(BhavaDigBala(time));
                subStrengthList.Add(BhavaDrishtiBala(time));

                var totalBhavaBala = new Dictionary<HouseName, double>();

                foreach (var houseToTotal in Library.House.AllHouses)
                {
                    //to get the total strength of the a house, we combine 3 types sub-strengths
                    double total = 0;
                    foreach (var subStrength in subStrengthList) { total += subStrength.Power[houseToTotal]; }
                    totalBhavaBala[houseToTotal] = total;
                }

                return new Shashtiamsa(totalBhavaBala[inputHouse]);

            }

        }

        /// <summary>
        /// House received aspect strength
        /// 
        /// Bhavadrishti Bala.-Each bhava in a
        /// horoscope remains aspected by certain planets.
        /// Sometimes the aspect cast on a bhava will be positive
        /// and sometimes it will be negative according
        /// as it is aspected by benefics or malefics.
        /// For all 12 houses
        /// </summary>
        public static HouseSubStrength BhavaDrishtiBala(Time time)
        {

            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey(nameof(BhavaDrishtiBala), time, Ayanamsa), _calcBhavaDrishtiBala);


            //UNDERLYING FUNCTION
            HouseSubStrength _calcBhavaDrishtiBala()
            {
                double vdrishti;

                //assign initial negative or positive value based on benefic or malefic planet
                var sp = goodAndBad();


                var drishti = GetDrishtiKendra(time);


                double bala = 0;

                var BhavaDrishtiBala = new Dictionary<HouseName, double>();

                foreach (var house in Library.House.AllHouses)
                {

                    bala = 0;

                    foreach (var planet in Library.PlanetName.All7Planets)
                    {

                        bala += (sp[planet] * drishti[planet.ToString() + house.ToString()]);

                    }

                    BhavaDrishtiBala[house] = bala;
                }


                var newHouseResult = new HouseSubStrength(BhavaDrishtiBala, "BhavaDrishtiBala");

                return newHouseResult;



                //------------------LOCAL FUNCTIONS---------------------

                Dictionary<PlanetName, int> goodAndBad()
                {

                    var _sp = new Dictionary<PlanetName, int>();

                    //assign initial negative or positive value based on benefic or malefic planet
                    foreach (var p in Library.PlanetName.All7Planets)
                    {
                        //Though in the earlier pages Mercury is defined either as a subba
                        //(benefic) or papa (malefic) according to its association is with a benefic or
                        //malefic, Mercury for purposes of calculating Drisbtibala of Bbavas is to
                        //be deemed as a full benefic. This is in accord with the injunctions of
                        //classical writers (Gurugnabbyam tu yuktasya poomamekam tu yojayet).

                        if (p == Library.PlanetName.Mercury)
                        {
                            _sp[p] = 1;
                            continue;
                        }

                        if (Calculate.IsPlanetBenefic(p, time))
                        {
                            _sp[p] = 1;
                        }
                        else
                        {
                            _sp[p] = -1;
                        }
                    }

                    return _sp;
                }

                Dictionary<string, double> GetDrishtiKendra(Time time1)
                {

                    //planet & house no. is used key
                    var _drishti = new Dictionary<string, double>();

                    double drishtiKendra;

                    foreach (var planet in Library.PlanetName.All7Planets)
                    {
                        foreach (var houseNo in Library.House.AllHouses)
                        {
                            //house is considered as a Drusya Graha (aspected body)
                            var houseMid = Calculate.HouseLongitude(houseNo, time1).GetMiddleLongitude();
                            var plantLong = Calculate.PlanetNirayanaLongitude(planet, time1);

                            //Subtract the longitude of the Drishti (aspecting)
                            // planet from that of the Drusya (aspected) Bhava
                            // Madhya. The Drishti Kendra is obtained.
                            drishtiKendra = (houseMid - plantLong).TotalDegrees;

                            //In finding the Drishti Kendra always add 360° to the longitude of the
                            //Drusya (Bhava Madhya) when it is less than the longitude of the
                            //Drishta (aspecting Graha).
                            if (drishtiKendra < 0) { drishtiKendra += 360; }

                            //get special aspect if any
                            vdrishti = FindViseshaDrishti(drishtiKendra, planet);

                            if ((planet == Library.PlanetName.Mercury) || (planet == Library.PlanetName.Jupiter))
                            {
                                //take the Drishti values of Guru and Budha on the Bhava Madhya as they are
                                _drishti[planet.ToString() + (houseNo).ToString()] = FindDrishtiValue(drishtiKendra) + vdrishti;
                            }
                            else
                            {
                                //take a fourth of the aspect value of other Grahas over the Bhava Madhya
                                _drishti[planet.ToString() + (houseNo).ToString()] = (FindDrishtiValue(drishtiKendra) + vdrishti) / 4.00;
                            }
                        }
                    }


                    return _drishti;
                }
            }

        }

        /// <summary>
        /// House strength from different types of signs
        /// 
        /// Bhava Digbala.-This is the strength
        /// acquired by the different bhavas falling in the
        /// different groups or types of signs.
        /// For all 12 houses
        /// </summary>
        public static HouseSubStrength BhavaDigBala(Time time)
        {

            var BhavaDigBala = new Dictionary<HouseName, double>();

            int dig = 0;

            //for every house
            foreach (var houseNumber in Library.House.AllHouses)
            {
                //a particular bhava acquires strength by its mid-point
                //falling in a particular kind of sign.

                //so get mid point of house
                var mid = Calculate.HouseLongitude(houseNumber, time).GetMiddleLongitude().TotalDegrees;
                var houseSign = Calculate.HouseSignName(houseNumber, time);

                //Therefore first find the number of a given Bhava Madhya and subtract
                // it from 1, if the given Bhava Madhya is situated
                // in Vrischika
                if ((mid >= 210.00)
                    && (mid <= 240.00))
                {
                    dig = 1 - (int)houseNumber;
                }
                //Subtract it from 4, if the given Bhava
                // Madhya is situated in Mesha, Vrishabha, Simha,
                // first half of Makara or last half of Dhanus.
                else if (((mid >= 0.00) && (mid <= 60.00))
                         || ((mid >= 120.00) && (mid <= 150.00))
                         || ((mid >= 255.00) && (mid <= 285.00)))
                {
                    dig = 4 - (int)houseNumber;
                }
                //Subtract it from 7 if in Mithuna, Thula, Kumbha, Kanya or
                // first half of Dhanus
                else if (((mid >= 60.00) && (mid <= 90.00))
                         || ((mid >= 150.00) && (mid <= 210.00))
                         || ((mid >= 300.00) && (mid <= 330.00))
                         || ((mid >= 240.00) && (mid <= 255.00)))
                {
                    dig = 7 - (int)houseNumber;
                }
                //and lastly from 1O if in Kataka, Meena and last half of Makara.
                else if (((mid >= 90.00) && (mid <= 120.00))
                         || ((mid >= 330.00) && (mid <= 360.00))
                         || ((mid >= 285.00) && (mid <= 300.00)))
                {
                    dig = 10 - (int)houseNumber;
                }


                //If the difference exceeds 6, subtract it from 12, otherwise
                //take it as it is and multiply this difference by 1O.
                //And you will get Bhava digbala of the particular bhava.

                if (dig < 0)
                {
                    dig = dig + 12;
                }

                if (dig > 6)
                {
                    dig = 12 - dig;
                }

                //store digbala value in return list with house number
                BhavaDigBala[houseNumber] = (double)dig * 10;

            }


            var newHouseResult = new HouseSubStrength(BhavaDigBala, "BhavaDigBala");

            return newHouseResult;

        }

        /// <summary>
        /// Bhavadhipatbi Bala: This is the potency
        /// of the lord of the bhava.
        /// For all 12 houses
        /// </summary>
        public static HouseSubStrength BhavaAdhipathiBala(Time time)
        {
            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey(nameof(BhavaAdhipathiBala), time, Ayanamsa), _calcBhavaAdhipathiBala);


            //UNDERLYING FUNCTION
            HouseSubStrength _calcBhavaAdhipathiBala()
            {
                PlanetName houseLord;

                var BhavaAdhipathiBala = new Dictionary<HouseName, double>();

                foreach (var house in Library.House.AllHouses)
                {

                    //get current house lord
                    houseLord = Calculate.LordOfHouse(house, time);

                    //The Shadbala Pinda (aggregate of the Shadbalas) of the lord of the
                    //bhava constitutes its Bhavadhipathi Bala.
                    //get Shadbala Pinda of lord (total strength) in shashtiamsas
                    BhavaAdhipathiBala[house] = PlanetShadbalaPinda(houseLord, time).ToDouble();

                }

                var newHouseResult = new HouseSubStrength(BhavaAdhipathiBala, "BhavaAdhipathiBala");

                return newHouseResult;

            }

        }

        /// <summary>
        /// 0 index is strongest
        /// </summary>
        public static List<PlanetName> BeneficPlanetListByShadbala(Time personBirthTime, int threshold)
        {

            //get all planets
            //var allPlanetByStrenght = AstronomicalCalculator.GetAllPlanetOrderedByStrength(personBirthTime);

            //take top 3 as needed planets
            var returnList = new List<PlanetName>();
            var yyy = Calculate.AllPlanetStrength(personBirthTime);
            foreach (var planet in yyy)
            {
                if (planet.Item1 > threshold)
                {
                    returnList.Add(planet.Item2);
                }
            }
            return returnList;
        }

        public static List<PlanetName> BeneficPlanetListByShadbala(Time personBirthTime)
        {

            //get all planets
            var allPlanetByStrenght = Calculate.AllPlanetOrderedByStrength(personBirthTime);

            //take top 3 as needed planets
            var returnList = new List<PlanetName>();
            returnList.Add(allPlanetByStrenght[0]);
            //returnList.Add(allPlanetByStrenght[1]);
            //returnList.Add(allPlanetByStrenght[2]);

            return returnList;
        }

        /// <summary>
        /// 0 index is strongest
        /// </summary>
        public static List<HouseName> BeneficHouseListByShadbala(Time personBirthTime, int threshold)
        {
            var returnList = new List<HouseName>();

            //create a list with planet names & its strength (unsorted)
            foreach (var house in Library.House.AllHouses)
            {
                //get house strength
                var strength = HouseStrength(house, personBirthTime).ToDouble();

                if (strength > threshold)
                {
                    returnList.Add(house);
                }


            }

            return returnList;


        }

        public static List<HouseName> BeneficHouseListByShadbala(Time personBirthTime)
        {
            //get all planets
            var allPlanetByStrenght = Calculate.AllHousesOrderedByStrength(personBirthTime);

            //take top 3 as needed planets
            var returnList = new List<HouseName>();
            returnList.Add(allPlanetByStrenght[0]);
            //returnList.Add(allPlanetByStrenght[1]);
            //returnList.Add(allPlanetByStrenght[2]);

            return returnList;


        }

        public static List<PlanetName> MaleficPlanetListByShadbala(Time personBirthTime, int threshold)
        {

            var returnList = new List<PlanetName>();
            var yyy = Calculate.AllPlanetStrength(personBirthTime);
            foreach (var planet in yyy)
            {
                if (planet.Item1 < threshold)
                {
                    returnList.Add(planet.Item2);
                }
            }
            return returnList;
        }

        /// <summary>
        /// 0 index is most malefic
        /// </summary>
        public static List<PlanetName> MaleficPlanetListByShadbala(Time personBirthTime)
        {

            //get all planets
            var allPlanetByStrenght = Calculate.AllPlanetOrderedByStrength(personBirthTime);

            //take last 3 as needed planets
            var returnList = new List<PlanetName>();
            returnList.Add(allPlanetByStrenght[^1]);
            //returnList.Add(allPlanetByStrenght[^2]);
            //returnList.Add(allPlanetByStrenght[^3]);

            return returnList;

        }

        /// <summary>
        /// 0 index is most malefic
        /// </summary>
        public static List<HouseName> MaleficHouseListByShadbala(Time personBirthTime, int threshold)
        {
            var returnList = new List<HouseName>();

            //create a list with planet names & its strength (unsorted)
            foreach (var house in Library.House.AllHouses)
            {
                //get house strength
                var strength = HouseStrength(house, personBirthTime).ToDouble();

                if (strength < threshold)
                {
                    returnList.Add(house);
                }


            }

            return returnList;
        }

        public static List<HouseName> MaleficHouseListByShadbala(Time personBirthTime)
        {

            //get all planets
            var allPlanetByStrenght = Calculate.AllHousesOrderedByStrength(personBirthTime);

            //take last 3 as needed planets
            var returnList = new List<HouseName>();
            returnList.Add(allPlanetByStrenght[^1]);
            //returnList.Add(allPlanetByStrenght[^2]);
            //returnList.Add(allPlanetByStrenght[^3]);

            return returnList;

        }

        #endregion

        #region TAGS STATIC

        /// <summary>
        /// keywords or tag related to a house
        /// </summary>
        public static string GetHouseTags(HouseName house)
        {
            switch (house)
            {
                case HouseName.House1: return "beginning of life, childhood, health, environment, personality, the physical body and character";
                case HouseName.House2: return "family, face, right eye, food, wealth, literary gift, and manner and source of death, self-acquisition and optimism";
                case HouseName.House3: return "brothers and sisters, intelligence, cousins and other immediate relations";
                case HouseName.House4: return "peace of mind, home life, mother, conveyances, house property, landed and ancestral properties, education and neck and shoulders";
                case HouseName.House5: return "children, grandfather, intelligence, emotions and fame";
                case HouseName.House6: return "debts, diseases, enemies, miseries, sorrows, illness and disappointments";
                case HouseName.House7: return "wife, husband, marriage, urinary organs, marital happiness, sexual diseases, business partner, diplomacy, talent, energies and general happiness";
                case HouseName.House8: return "longevity, legacies and gifts and unearned wealth, cause of death, disgrace, degradation and details pertaining to death";
                case HouseName.House9: return "father, righteousness, preceptor, grandchildren, intuition, religion, sympathy, fame, charities, leadership, journeys and communications with spirits";
                case HouseName.House10: return "occupation, profession, temporal honours, foreign travels, self-respect, knowledge and dignity and means of livelihood";
                case HouseName.House11: return "means of gains, elder brother and freedom from misery";
                case HouseName.House12: return "losses, expenditure, waste, extravagance, sympathy, divine knowledge, Moksha and the state after death";
                default: throw new Exception("House details not found!");
            }
        }

        /// <summary>
        /// Given a zodiac sign, will return astro keywords related to sign
        /// These details would be highly useful in the delineation of
        /// character and mental disposition
        /// Source:Hindu Predictive Astrology pg.16
        /// </summary>
        public static string GetSignTags(ZodiacName zodiacName)
        {
            switch (zodiacName)
            {
                case ZodiacName.Aries:
                    return @"movable, odd, masculine, cruel, fiery, of short ascension, rising by hinder part, powerful during the night";
                case ZodiacName.Taurus:
                    return @"fixed, even, feminine, mild,earthy, fruitful, of short ascension, rising by hinder part";
                case ZodiacName.Gemini:
                    return @"common, odd, masculine, cruel, airy, barren, of short ascension, rising by the head.";
                case ZodiacName.Cancer:
                    return @"even, movable, feminine, mild, watery, of long ascension, rising by the hinder part and fruitful.";
                case ZodiacName.Leo:
                    return @"fixed, odd, masculine, cruel, fiery, of long ascension, barren, rising by the head.";
                case ZodiacName.Virgo:
                    return @"common, even, feminine, mild, earthy, of long ascension, rising by the head.";
                case ZodiacName.Libra:
                    return @"movable, odd, masculine, cruel, airy, of long ascension, rising by the head.";
                case ZodiacName.Scorpio:
                    return @"fixed, even, feminine, mild, watery, of long ascension, rising by the head.";
                case ZodiacName.Sagittarius:
                    return @"common, odd, masculine, cruel, fiery, of long ascension, rising by the hinder part.";
                case ZodiacName.Capricorn:
                    return @"movable, even, feminine, mild, earthy, of long ascension, rising by hinder part";
                case ZodiacName.Aquarius:
                    return @"fixed, odd, masculine, cruel, fruitful, airy, of short ascension, rising by the head.";
                case ZodiacName.Pisces:
                    return @"common, feminine, water, even, mild, of short ascension, rising by head and hinder part.";
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static string GetPlanetTags(List<PlanetName> planetList) => planetList.Aggregate("", (current, planet) => current + GetPlanetTags(planet));

        /// <summary>
        /// Get keywords related to a planet.
        /// </summary>
        public static string GetPlanetTags(PlanetName lordOfHouse)
        {
            switch (lordOfHouse.Name)
            {
                case PlanetName.PlanetNameEnum.Sun:
                    return "Father, masculine, malefic, copper colour, philosophical tendency, royal, ego, sons, patrimony, self reliance, political power, windy and bilious temperament, month, places of worship, money-lenders, goldsmith, bones, fires, coronation chambers, doctoring capacity";
                case PlanetName.PlanetNameEnum.Moon:
                    return "Mother, feminine, mind, benefic when waxing, malefic when waning, white colour, women, sea-men, pearls, gems, water, fishermen, stubbornness, romances, bath-rooms, blood, popularity, human responsibilities";
                case PlanetName.PlanetNameEnum.Mars:
                    return "Brothers, masculine, blood-red colour, malefic, refined taste, base metals, vegetation, rotten things, orators, ambassadors, military activities, commerce, aerial journeys, weaving, public speakers.";
                case PlanetName.PlanetNameEnum.Mercury:
                    return "Profession, benefic if well associated, hermaphrodite, green colour, mercantile activity, public speakers, cold nervous, intelligence";
                case PlanetName.PlanetNameEnum.Jupiter:
                    return "Children, masculine, benefic, bright yellow colour, devotion, truthfulness, religious fervour, philosophical wisdom, corpulence";
                case PlanetName.PlanetNameEnum.Venus:
                    return "Wife, feminine, benefic, mixture of all colours, love affairs, sensual pleasure, family bliss, harems of ill-fame, vitality";
                case PlanetName.PlanetNameEnum.Saturn:
                    return "Longevity, malefic, hermaphrodite, dark colour, stubbornness, impetuosity, demoralisation, windy diseases, despondency, gambling";
                case PlanetName.PlanetNameEnum.Rahu:
                    return "Maternal relations, malefic, feminine, renunciation, corruption, epidemics";
                case PlanetName.PlanetNameEnum.Ketu:
                    return "Paternal relations, Hermaphrodite, malefic, religious, sectarian principles, pride, selfishness, occultism, mendicancy";
                default:
                    return "";
            }
        }

        /// <summary>
        /// Source: Hindu Predictive Astrology pg.17
        /// </summary>
        public static string GetHouseType(HouseName houseNumber)
        {
            //Quadrants (kendras) are l, 4, 7 and 10.
            //Trines(Trikonas) are 5 and 9.
            //Cadent houses (Panaparas) are 2, 5, 8 and 11
            //Succeedent houses (Apoklimas) are 3, 6, 9 and 12 (9th being a trikona must be omitted)
            //Upachayas are 3, 6, 10 and 11.

            var returnString = "";

            switch (houseNumber)
            {
                //Quadrants (kendras) are l, 4, 7 and 10.
                case HouseName.House1:
                case HouseName.House4:
                case HouseName.House7:
                case HouseName.House10:
                    returnString += @"Quadrants (kendras)";
                    break;
                //Trines(Trikonas) are 5 and 9.
                case HouseName.House5:
                case HouseName.House9:
                    returnString += @"Trines (Trikonas)";
                    break;
            }

            switch (houseNumber)
            {
                //Cadent (Panaparas) are 2, 5, 8 and 11
                case HouseName.House2:
                case HouseName.House5:
                case HouseName.House8:
                case HouseName.House11:
                    returnString += @"Cadent (Panaparas)";
                    break;
                //Succeedent (Apoklimas) are 3, 6, 9 and 12 (9th being a trikona must be omitted)
                case HouseName.House3:
                case HouseName.House6:
                case HouseName.House9:
                case HouseName.House12:
                    returnString += @"Succeedent (Apoklimas)";
                    break;
            }

            switch (houseNumber)
            {
                //Upachayas are 3, 6, 10 and 11.
                case HouseName.House3:
                case HouseName.House6:
                case HouseName.House10:
                case HouseName.House11:
                    returnString += @"Upachayas";
                    break;

            }

            return returnString;
        }

        /// <summary>
        /// Get general planetary info for person's dasa (hardcoded table)
        /// It is intended to be used to interpret dasa predictions
        /// as such should be displayed next to dasa chart.
        /// This method is direct translation from the book.
        /// Similar to method GetPlanetDasaNature
        /// Data from pg 80 of Key-planets for Each Sign in Hindu Predictive Astrology
        /// </summary>
        public static string GetDasaInfoForAscendant(ZodiacName ascendantName)
        {
            //As soon as tbc Dasas and Bhuktis are determined, the next
            //step would be to find out the good and evil planets for each
            //ascendant so that in applying the principles to decipher the
            //future history of man, the student may be able to carefully
            //analyse the intensilty or good or evil combinations and proceed
            //further with his predictions when applying the results of
            //Dasas and other combinations.

            switch (ascendantName)
            {
                case ZodiacName.Aries:
                    return @"
                        Aries - Saturn, Mercury and Venus are ill-disposed.
                        Jupiter and the Sun are auspicious. The mere combination
                        of Jupiler and Saturn produces no beneficial results. Jupiter
                        is the Yogakaraka or the planet producing success. If Venus
                        becomes a maraka, he will not kill the native but planets like
                        Saturn will bring about death to the person.
                        ";
                case ZodiacName.Taurus:
                    return @"
                        Taurus - Saturn is the most auspicious and powerful
                        planet. Jupiter, Venus and the Moon are evil planets. Saturn
                        alone produces Rajayoga. The native will be killed in the
                        periods and sub-periods of Jupiter, Venus and the Moon if
                        they get death-inflicting powers.
                        ";
                case ZodiacName.Gemini:
                    return @"
                        Gemini - Mars, Jupiter and the Sun are evil. Venus alone
                        is most beneficial and in conjunction with Saturn in good signs
                        produces and excellent career of much fame. Combination
                        of Saturn and Jupiter produces similar results as in Aries.
                        Venus and Mercury, when well associated, cause Rajayoga.
                        The Moon will not kill the person even though possessed of
                        death-inflicting powers.
                        ";
                case ZodiacName.Cancer:
                    return @"
                        Cancer - Venus and Mercury are evil. Jupiter and Mars
                        give beneficial results. Mars is the Rajayogakaraka
                        (conferor of name and fame). The combination of Mars and Jupiter
                        also causes Rajayoga (combination for political success). The
                        Sun does not kill the person although possessed of maraka
                        powers. Venus and other inauspicious planets kill the native.
                        Mars in combination with the Moon or Jupiter in favourable
                        houses especially the 1st, the 5th, the 9th and the 10th
                        produces much reputation.
                        ";
                case ZodiacName.Leo:
                    return @"
                        Leo - Mars is the most auspicious and favourable planet.
                        The combination of Venus and Jupiter does not cause Rajayoga
                        but the conjunction of Jupiter and Mars in favourable
                        houses produce Rajayoga. Saturn, Venus and Mercury are
                        evil. Saturn does not kill the native when he has the maraka
                        power but Mercury and other evil planets inflict death when
                        they get maraka powers.
                        ";
                case ZodiacName.Virgo:
                    return @"
                        Virgo - Venus alone is the most powerful. Mercury and
                        Venus when combined together cause Rajayoga. Mars and
                        the Moon are evil. The Sun does not kill the native even if
                        be becomes a maraka but Venus, the Moon and Jupiter will
                        inflict death when they are possessed of death-infticting power.
                        ";
                case ZodiacName.Libra:
                    return @"
                        Libra - Saturn alone causes Rajayoga. Jupiter, the Sun
                        and Mars are inauspicious. Mercury and Saturn produce good.
                        The conjunction of the Moon and Mercury produces Rajayoga.
                        Mars himself will not kill the person. Jupiter, Venus
                        and Mars when possessed of maraka powers certainly kill the
                        nalive.
                        ";
                case ZodiacName.Scorpio:
                    return @"
                        Scorpio - Jupiter is beneficial. The Sun and the Moon
                        produce Rajayoga. Mercury and Venus are evil. Jupiter,
                        even if be becomes a maraka, does not inflict death. Mercury
                        and other evil planets, when they get death-inlflicting powers,
                        do not certainly spare the native.
                        ";
                case ZodiacName.Sagittarius:
                    return @"
                        Sagittarius - Mars is the best planet and in conjunction
                        with Jupiter, produces much good. The Sun and Mars also
                        produce good. Venus is evil. When the Sun and Mars
                        combine together they produce Rajayoga. Saturn does not
                        bring about death even when he is a maraka. But Venus
                        causes death when be gets jurisdiction as a maraka planet.
                        ";
                case ZodiacName.Capricorn:
                    return @"
                        Capricorn - Venus is the most powerful planet and in
                        conjunction with Mercury produces Rajayoga. Mars, Jupiter
                        and the Moon are evil.
                        ";
                case ZodiacName.Aquarius:
                    return @"
                        Aquarius - Venus alone is auspicious. The combination of
                        Venus and Mars causes Rajayoga. Jupiter and the Moon are
                        evil.
                        ";
                case ZodiacName.Pisces:
                    return @"
                        Pisces - The Moon and Mars are auspicious. Mars is
                        most powerful. Mars with the Moon or Jupiter causes Rajayoga.
                        Saturn, Venus, the Sun and Mercury are evil. Mars
                        himself does not kill the person even if he is a maraka.
                        ";
                default:
                    throw new ArgumentOutOfRangeException(nameof(ascendantName), ascendantName, null);
            }

        }

        #endregion

        //--------------------------------------------------------------------------------------------



        /// <summary>
        /// Given a list of planets will pick out the strongest planet based on Shadbala
        /// </summary>
        public static PlanetName PickOutStrongestPlanet(List<PlanetName> relatedPlanets, Time birthTime)
        {
            //if only 1 planet than no need to check
            if (relatedPlanets.Count == 1) { return relatedPlanets[0]; }

            //calculare strength for all given planets
            var powerList = new Dictionary<PlanetName, double>();
            foreach (var relatedPlanet in relatedPlanets)
            {
                var strength = Calculate.PlanetStrength(relatedPlanet, birthTime);
                powerList.Add(relatedPlanet, strength.ToDouble());
            }

            //pickout highest value
            var strongest = powerList.Aggregate((l, r) => l.Value > r.Value ? l : r);

            //return strongest planet name
            return strongest.Key;
        }

        /// <summary>
        /// Gets the characteristic of signs
        /// </summary>
        public static SignProperties SignProperties(ZodiacName inputSign) => new SignProperties(inputSign);


    }
}

