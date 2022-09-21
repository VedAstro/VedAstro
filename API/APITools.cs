using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using Azure.Storage.Blobs;
using Genso.Astrology.Library;
using Genso.Astrology.Library.Compatibility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace API
{
    /// <summary>
    /// A collection of general tools used by API
    /// </summary>
    public static class APITools
    {
        //used in muhurtha
        private const string UrlEventDataListXml = "https://www.vedastro.org/data/EventDataList.xml";

        //used in horoscope
        private const string UrlPredictionDataListXml = "https://www.vedastro.org/data/PredictionDataList.xml";


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
        }

        //todo use new method below for shorter code & consistency
        /// <summary>
        /// Adds an XML element to XML document in blob form
        /// </summary>
        public static XDocument AddXElementToXDocument(BlobClient xDocuBlobClient, XElement newElement)
        {
            //get person list from storage
            var personListXml = BlobClientToXml(xDocuBlobClient);

            //add new person to list
            personListXml.Root.Add(newElement);

            return personListXml;
        }

        /// <summary>
        /// Adds an XML element to XML document in by file & container name
        /// and saves files directly to blob store
        /// </summary>
        public static async Task AddXElementToXDocument(XElement dataXml, string fileName, string containerName)
        {
            //get user data list file (UserDataList.xml) Azure storage
            var fileClient = await GetFileFromContainer(fileName, containerName);

            //add new log to main list
            var updatedListXml = AddXElementToXDocument(fileClient, dataXml);

            //upload modified list to storage
            await OverwriteBlobData(fileClient, updatedListXml);

        }



        /// <summary>
        /// Extracts data coming in from API caller
        /// And parses it as XML
        /// Note : Data is assumed to be XML in string form
        /// </summary>
        public static XElement ExtractDataFromRequest(HttpRequestMessage request)
        {
            //get xml string from caller
            var xmlString = RequestToXmlString(request);

            //parse xml string
            //todo an exception check here might be needed
            var xml = XElement.Parse(xmlString);

            return xml;
        }

        /// <summary>
        /// Converts a blob client of a file to an XML document
        /// </summary>
        public static XDocument BlobClientToXml(BlobClient blobClient)
        {
            //parse string as xml doc
            var document = XDocument.Load(blobClient.Download().Value.Content);

            return document;
        }

        public static Stream GenerateStreamFromString(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        /// <summary>
        /// Simply converts incoming request to raw string
        /// No parsing is done here
        /// note: null request return empty string
        /// </summary>
        public static string RequestToXmlString(HttpRequestMessage rawData) => rawData?.Content?.ReadAsStringAsync().Result ?? "";

        /// <summary>
        /// Extracts names from the query URL
        /// </summary>
        public static async Task<object> ExtractMaleFemaleHash(HttpRequest request)
        {
            string maleHash = request.Query["male"];
            string femaleHash = request.Query["female"];

            string requestBody = await new StreamReader(request.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            maleHash = maleHash ?? data?.male;
            femaleHash = femaleHash ?? data?.female;

            //convert to int
            return new { Male = int.Parse(maleHash), Female = int.Parse(femaleHash) };

        }

        public static CompatibilityReport GetCompatibilityReport(int maleHash, int femaleHash, Data personList)
        {
            //get all the people
            var peopleList = DatabaseManager.GetPersonList(personList);

            //filter out the male and female ones we want
            var male = peopleList.Find(person => person.Hash == maleHash);
            var female = peopleList.Find(person => person.Hash == femaleHash);

            return MatchCalculator.GetCompatibilityReport(male, female);
        }

        /// <summary>
        /// Gets the error in a nice string to send to user
        /// todo receiver using this data expect XML, so when receiving such data as below will raise parse alarm
        /// todo so when changing to XML, look for receivers and update their error catching mechanism
        /// </summary> 
        public static OkObjectResult FormatErrorReply(Exception e)
        {
            var responseMessage = "";

            responseMessage += $"#Message#\n{e.Message}\n";
            responseMessage += $"#Data#\n{e.Data}\n";
            responseMessage += $"#InnerException#\n{e.InnerException}\n";
            responseMessage += $"#Source#\n{e.Source}\n";
            responseMessage += $"#StackTrace#\n{e.StackTrace}\n";
            responseMessage += $"#StackTrace#\n{e.TargetSite}\n";

            return new OkObjectResult(responseMessage);
        }

        /// <summary>
        /// Given list of person, it will find the person by hash
        /// and return the XML version of the person.
        /// Note:- Person's hash is computed on the fly to reduce coupling
        ///      - If any error occur, it will return empty person tag
        /// </summary>
        public static async Task<XElement> FindPersonByHash(XDocument personListXml, int originalHash)
        {
            try
            {
                return personListXml.Root.Elements()
                    .Where(delegate (XElement personXml)
                    {   //use hash as id to find the person's record
                        var thisHash = Person.FromXml(personXml).GetHashCode();
                        return thisHash == originalHash;
                    }).First();
            }
            catch (Exception e)
            {
                //if fail log it and return empty xelement
                await Log.Error(e, null);
                return new XElement("Person");
            }
        }


        public static async Task<BlobClient> GetFileFromContainer(string fileName, string blobContainerName)
        {
            //get the connection string stored separately (for security reasons)
            var storageConnectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");

            //get image from storage
            var blobContainerClient = new BlobContainerClient(storageConnectionString, blobContainerName);
            var fileBlobClient = blobContainerClient.GetBlobClient(fileName);

            return fileBlobClient;

            //var returnStream = new MemoryStream();
            //await fileBlobClient.DownloadToAsync(returnStream);

            //return returnStream;
        }


        public static XElement FindVisitorById(XDocument visitorListXml, string visitorId)
        {
            try
            {
                var uniqueVisitorList = from visitorXml in visitorListXml.Root?.Elements()
                                        where visitorXml.Element("VisitorId")?.Value == visitorId
                                        select visitorXml;

                return uniqueVisitorList.FirstOrDefault();
            }
            catch (Exception e)
            {
                //if fail log it and return empty xelement
                //todo log failure
                return new XElement("Visitor");
            }
        }

        /// <summary>
        /// Find all person's xml element by user id
        /// </summary>
        public static IEnumerable<XElement> FindPersonByUserId(XDocument personListXml, string userId)
        {
            var foundPersonListXml = from person in personListXml.Root?.Elements()
                                     where
                                         person.Element("UserId")?.Value == userId
                                     select person;

            return foundPersonListXml;
        }


        public static async Task<Person> GetPersonFromHash(int personHash, BlobClient personListClient)
        {
            var personListXml = APITools.BlobClientToXml(personListClient);
            var foundPersonXml = await APITools.FindPersonByHash(personListXml, personHash);
            var foundPerson = Person.FromXml(foundPersonXml);

            return foundPerson;
        }

        /// <summary>
        /// Get parsed EventDataList.xml from wwwroot file / static site
        /// Note: Event data list needed to calculate events
        /// </summary>
        public static async Task<List<EventData>> GetEventDataList()
        {
            //get data list from Static Website storage
            var eventDataListXml = await GetXmlFile(APITools.UrlEventDataListXml);

            //parse each raw event data in list
            var eventDataList = new List<EventData>();
            foreach (var eventDataXml in eventDataListXml)
            {
                //add it to the return list
                eventDataList.Add(EventData.FromXml(eventDataXml));
            }

            return eventDataList;

        }

        /// <summary>
        /// Get parsed PredictionDataList.xml from wwwroot file / static site
        /// </summary>
        public static async Task<List<EventData>> GetPredictionDataList()
        {
            //get data list from Static Website storage
            var predictionDataListXml = await GetXmlFile(APITools.UrlPredictionDataListXml);

            //parse each raw event data in list
            var predictionDataList = new List<EventData>();
            foreach (var predictionDataXml in predictionDataListXml)
            {
                //add it to the return list
                predictionDataList.Add(EventData.FromXml(predictionDataXml));
            }

            return predictionDataList;

        }


        /// <summary>
        /// Gets XML file from any URL and parses it into xelement list
        /// </summary>
        public static async Task<List<XElement>> GetXmlFile(string url)
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


        public static List<Event> CalculateEvents(double eventsPrecision, Time startTime, Time endTime, GeoLocation geoLocation, Person person, EventTag tag, List<EventData> eventDataList)
        {

            //get all event data/types which has the inputed tag (FILTER)
            var eventDataListFiltered = DatabaseManager.GetEventDataListByTag(tag, eventDataList);


            //start calculating events
            var eventList = EventManager.GetEventsInTimePeriod(startTime.GetStdDateTimeOffset(), endTime.GetStdDateTimeOffset(), geoLocation, person, eventsPrecision, eventDataListFiltered);


            //sort the list by time before sending view
            var orderByAscResult = from dasaEvent in eventList
                                   orderby dasaEvent.StartTime.GetStdDateTimeOffset()
                                   select dasaEvent;


            //send sorted events to view
            return orderByAscResult.ToList();

        }

        /// <summary>
        /// Gets XML file from Azure blob storage
        /// </summary>
        public static async Task<XDocument> GetXmlFileFromAzureStorage(string fileName, string blobContainerName)
        {
            var fileClient = await APITools.GetFileFromContainer(fileName, blobContainerName);
            var xmlFile = APITools.BlobClientToXml(fileClient);

            return xmlFile;
        }


        public static async Task<List<HoroscopePrediction>> GetPrediction(Person person)
        {
            //note: modified to use birth time as start & end time
            var startStdTime = person.BirthTime;
            var endStdTime = person.BirthTime;

            var location = person.GetBirthLocation();

            //get list of prediction event data to check for event (file from wwwroot)
            var eventDataList = await GetPredictionDataList();

            //start calculating predictions
            var predictionList = GetListOfPredictionInTimePeriod(startStdTime, endStdTime, location, person, TimePreset.Minute1, eventDataList);


            return predictionList;
        }

        /// <summary>
        /// Get list of predictions occurring in a time period for all the
        /// inputed prediction types aka "prediction data"
        /// </summary>
        public static List<HoroscopePrediction> GetListOfPredictionInTimePeriod(Time startStdTime, Time endStdTime, GeoLocation geoLocation, Person person, double precisionInHours, List<EventData> eventDataList)
        {
            //get data to instantiate muhurtha time period
            //get start & end times

            //initialize empty list of event to return
            List<HoroscopePrediction> eventList = new();

            //split time into slices based on precision
            var timeList = GetTimeListFromRange(startStdTime, endStdTime, precisionInHours);

            try
            {
                foreach (var eventData in eventDataList)
                {
                    //get list of occuring events for a single event type
                    var eventListForThisEvent = GetPredictionListByEventData(eventData, person, timeList);
                    //add events to main list of event
                    eventList.AddRange(eventListForThisEvent);
                }

            }
            //catches only exceptions that indicates that user canceled the calculation (caller lost interest in the result)
            catch (Exception e) when (e.InnerException.GetType() == typeof(OperationCanceledException))
            {
                //return empty list
                return new List<HoroscopePrediction>();
            }


            //return calculated event list
            return eventList;
        }

        public static List<Time> GetTimeListFromRange(Time startTime, Time endTime, double precisionInHours)
        {
            //declare return value
            var timeList = new List<Time>();

            //create list
            for (var day = startTime; day.GetStdDateTimeOffset() <= endTime.GetStdDateTimeOffset(); day = day.AddHours(precisionInHours))
            {
                timeList.Add(day);
            }

            //return value
            return timeList;
        }

        /// <summary>
        /// Get a list of events in a time period for a single event type aka "event data"
        /// Decision on when event starts & ends is also done here
        /// Event Data + Time = HoroscopePrediction
        /// </summary>
        private static List<HoroscopePrediction> GetPredictionListByEventData(EventData eventData, Person person, List<Time> timeList)
        {
            //declare empty event list to fill
            var eventList = new List<HoroscopePrediction>();

            //set previous time as false for first time instance
            var eventOccuredInPreviousTime = false;

            //declare start & end times
            Time eventStartTime = new Time();
            Time eventEndTime = new Time();
            var lastInstanceOfTime = timeList.Last();

            //loop through time list 
            //note: loop must be done in sequential order, to detect correct start & end time
            foreach (var time in timeList)
            {
                //debug print
                //Console.Write($"\r Checking time:{time} : {eventData.GetName()}");

                //get flag of event occuring now
                var eventIsOccuringNow = eventData.IsEventOccuring(time, person);

                //if event is occuring now & not in previous time
                if (eventIsOccuringNow == true & eventOccuredInPreviousTime == false)
                {
                    //save new start & end time
                    eventStartTime = time;
                    eventEndTime = time;
                    //update flag
                    eventOccuredInPreviousTime = true;
                }
                //if event is occuring now & in previous time
                else if (eventIsOccuringNow == true & eventOccuredInPreviousTime == true)
                {
                    //update end time only
                    eventEndTime = time;
                    //update flag
                    eventOccuredInPreviousTime = true;
                }
                //if event is not occuring now but occurred before
                else if (eventIsOccuringNow == false & eventOccuredInPreviousTime == true)
                {
                    //add previous event to list
                    var newEvent = new HoroscopePrediction(eventData.GetName(),
                        eventData.GetDescription(),
                        eventData.GetStrength());

                    eventList.Add(newEvent);

                    //set flag
                    eventOccuredInPreviousTime = false;
                }

                //if event is occuring now & it is the last time
                if (eventIsOccuringNow == true & time == lastInstanceOfTime)
                {
                    //add current event to list
                    var newEvent2 = new HoroscopePrediction(eventData.GetName(),
                        eventData.GetDescription(),
                        eventData.GetStrength());


                    eventList.Add(newEvent2);
                }
            }

            return eventList;
        }



    }
}
