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
            //get user data list file  (UserDataList.xml) Azure storage
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


        public static async Task<Person> GetPersonFromHash(int personHash,BlobClient personListClient)
        {
            var personListXml = APITools.BlobClientToXml(personListClient);
            var foundPersonXml = await APITools.FindPersonByHash(personListXml, personHash);
            var foundPerson = Person.FromXml(foundPersonXml);

            return foundPerson;
        }

        /// <summary>
        /// Get parsed EventDataList from Azure storage
        /// Note: Event data list needed to calculate events
        /// </summary>
        public static async Task<List<EventData>> GetEventDataList()
        {
            //get data list from Azure storage
            //TODO NEEDS TO BE CHANGED TO WWW ROOT VERSION OF THE FILE
            var eventDataListXml = await GetXmlFileFromAzureStorage("EventDataList.xml", "vedastro-site-data");

            //parse each raw event data in list
            var eventDataList = new List<EventData>();
            foreach (var eventData in eventDataListXml.Root.Elements())
            {
                //add it to the return list
                eventDataList.Add(EventData.ToXml(eventData));
            }

            return eventDataList;
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


    }
}
