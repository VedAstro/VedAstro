using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
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
        //█░█ ▄▀█ █▀█ █▀▄   █▀▄ ▄▀█ ▀█▀ ▄▀█
        //█▀█ █▀█ █▀▄ █▄▀   █▄▀ █▀█ ░█░ █▀█


        //hard coded links to files stored in storage
        //public const string ApiDataStorageContainer = "vedastro-site-data";


        //NAMES OF FILES IN AZURE STORAGE FOR ACCESS
        public const string LiveChartHtml = "LiveChart.html";
        public const string PersonListFile = "PersonList.xml";
        public const string VisitorLogFile = "VisitorLog.xml";
        public const string TaskListFile = "TaskList.xml";
        public const string MessageListFile = "MessageList.xml";
        public const string SavedChartListFile = "SavedChartList.xml";
        public const string RecycleBinFile = "RecycleBin.xml";
        public const string UserDataListXml = "UserDataList.xml";
        public const string BlobContainerName = "vedastro-site-data";

        //we use direct storage URL for fast access & solid
        public const string AzureStorage = "vedastrowebsitestorage.z5.web.core.windows.net";


        //used in horoscope prediction
        public const string UrlHoroscopeDataListXml = $"https://{AzureStorage}/data/HoroscopeDataList.xml";

        public const string UrlEventsChartViewerHtml = $"https://{AzureStorage}/data/EventsChartViewer.html";

        /// <summary>
        /// Toolbar.svg used in when rendering events chart
        /// </summary>
        public const string ToolbarSvgAzure = $"https://{AzureStorage}/svg/Toolbar.svg";

        /// <summary>
        /// Default success message sent to caller
        /// - .ToString() to make xml indented
        /// </summary>
        public static OkObjectResult PassMessage(string msg = "") => new(new XElement("Root", new XElement("Status", "Pass"), new XElement("Payload", msg)).ToString());
        public static OkObjectResult PassMessage(XElement payloadXml) => new(new XElement("Root", new XElement("Status", "Pass"), new XElement("Payload", payloadXml)).ToString());
        public static OkObjectResult FailMessage(string msg = "") => new(new XElement("Root", new XElement("Status", "Fail"), new XElement("Payload", msg)).ToString());
        public static OkObjectResult FailMessage(XElement payloadXml) => new(new XElement("Root", new XElement("Status", "Fail"), new XElement("Payload", payloadXml)).ToString());
        public static OkObjectResult FailMessage(Exception payloadException) => FailMessage(Tools.ExceptionToXml(payloadException));

        public static List<HoroscopeData> SavedHoroscopeDataList { get; set; } = null; //null used for checking empty


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

        /// <summary>
        /// Adds an XML element to XML document in blob form
        /// </summary>
        public static async Task<XDocument> AddXElementToXDocument(BlobClient xDocuBlobClient, XElement newElement)
        {
            //get person list from storage
            var xDocument = await BlobClientToXmlDoc(xDocuBlobClient);

            //add new person to list
            xDocument.Root.Add(newElement);

            return xDocument;
        }

        /// <summary>
        /// Adds an XML element to XML document in by file & container name
        /// and saves files directly to Azure blob store
        /// </summary>
        public static async Task AddXElementToXDocumentAzure(XElement dataXml, string fileName, string containerName)
        {
            //get user data list file (UserDataList.xml) Azure storage
            var fileClient = await GetBlobClientAzure(fileName, containerName);

            //add new log to main list
            var updatedListXml = await AddXElementToXDocument(fileClient, dataXml);

            //upload modified list to storage
            await OverwriteBlobData(fileClient, updatedListXml);

        }

        /// <summary>
        /// Deletes an XML element from an XML document in by file & container name
        /// and saves files directly to Azure blob store
        /// </summary>
        public static async Task DeleteXElementFromXDocumentAzure(XElement dataXmlToDelete, string fileName, string containerName)
        {
            //access to file
            var fileClient = await GetBlobClientAzure(fileName, containerName);
            //get xml file
            var xmlDocFile = await BlobClientToXmlDoc(fileClient);

            //check if record to delete exists
            //if not found, raise alarm
            var xmlRecordList = xmlDocFile.Root.Elements();
            var personToDelete = Person.FromXml(dataXmlToDelete);
            var foundRecords = xmlRecordList.Where(x => Person.FromXml(x).Id == personToDelete.Id);
            if (!foundRecords.Any()) { throw new Exception("Could not find XML record to delete in main list!"); }

            //continue with delete
            foundRecords.First().Remove();

            //upload modified list to storage
            await OverwriteBlobData(fileClient, xmlDocFile);

        }

        /// <summary>
        /// Saves XML file direct to Azure storage
        /// </summary>
        public static async Task SaveXDocumentToAzure(XDocument dataXml, string fileName, string containerName)
        {
            //get file client for file
            var fileClient = await GetBlobClientAzure(fileName, containerName);

            //upload modified list to storage
            await OverwriteBlobData(fileClient, dataXml);

        }

        /// <summary>
        /// Extracts data coming in from API caller
        /// And parses it as XML
        /// Note : Data is assumed to be XML in string form
        /// </summary>
        public static XElement ExtractDataFromRequest(HttpRequestMessage request)
        {
            //get xml string from caller
            var xmlString = RequestToString(request);

            //parse xml string
            //todo an exception check here might be needed, json data might come here
            var xml = XElement.Parse(xmlString);

            return xml;
        }

        /// <summary>
        /// Converts a blob client of a file to an XML document
        /// </summary>
        public static async Task<XDocument> BlobClientToXmlDoc(BlobClient blobClient)
        {
            try
            {
                var xmlFileString = await DownloadToText(blobClient);

                XDocument document = XDocument.Parse(xmlFileString);
                //var document = XDocument.Load(xmlFileString);

                return document;

            }
            catch (Exception e)
            {
                //todo log the error here
                Console.WriteLine(e);
                throw new Exception($"Azure Storage Failure : {blobClient.Name}");
            }


            //Console.WriteLine(blobClient.Name);
            //Console.WriteLine(blobClient.AccountName);
            //Console.WriteLine(blobClient.BlobContainerName);
            //Console.WriteLine(blobClient.Uri);
            //Console.WriteLine(blobClient.CanGenerateSasUri);

            //if does not exist raise alarm
            if (!await blobClient.ExistsAsync())
            {
                Console.WriteLine("NO FILE!");
            }

            //parse string as xml doc
            //var valueContent = blobClient.Download().Value.Content;
            //Console.WriteLine("Text:"+Tools.StreamToString(valueContent));

        }

        /// <summary>
        /// Converts a blob client of a file to string
        /// </summary>
        public static async Task<string> BlobClientToString(BlobClient blobClient)
        {
            try
            {
                var xmlFileString = await DownloadToText(blobClient);

                return xmlFileString;

            }
            catch (Exception e)
            {
                //todo log the error here
                Console.WriteLine(e);
                throw new Exception($"Azure Storage Failure : {blobClient.Name}");
            }


            //Console.WriteLine(blobClient.Name);
            //Console.WriteLine(blobClient.AccountName);
            //Console.WriteLine(blobClient.BlobContainerName);
            //Console.WriteLine(blobClient.Uri);
            //Console.WriteLine(blobClient.CanGenerateSasUri);

            //if does not exist raise alarm
            if (!await blobClient.ExistsAsync())
            {
                Console.WriteLine("NO FILE!");
            }

            //parse string as xml doc
            //var valueContent = blobClient.Download().Value.Content;
            //Console.WriteLine("Text:"+Tools.StreamToString(valueContent));

        }

        /// <summary>
        /// Method from Azure Website
        /// </summary>
        public static async Task<string> DownloadToText(BlobClient blobClient)
        {
            BlobDownloadResult downloadResult = await blobClient.DownloadContentAsync();
            string downloadedData = downloadResult.Content.ToString();
            //Console.WriteLine("Downloaded data:", downloadedData);
            return downloadedData;
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
        public static string RequestToString(HttpRequestMessage rawData) => rawData?.Content?.ReadAsStringAsync().Result ?? "";

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





        public static async Task<XElement> FindChartById(XDocument savedChartListXml, string inputChartId)
        {
            try
            {
                var chartXmlList = savedChartListXml.Root.Elements();

                //Console.WriteLine(savedChartListXml.ToString());

                return chartXmlList.Where(delegate (XElement chartXml)
                {   //use chart id to find chart record
                    var thisId = Chart.FromXml(chartXml).ChartId;
                    return thisId == inputChartId;
                }).FirstOrDefault(Chart.Empty.ToXml());


            }
            catch (Exception e)
            {
                //if fail log it and return empty xelement
                await APILogger.Error(e, null);
                return new XElement("Chart");
            }
        }

        /// <summary>
        /// Gets file blob client from azure storage by name
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="blobContainerName"></param>
        /// <returns></returns>
        public static async Task<BlobClient> GetBlobClientAzure(string fileName, string blobContainerName)
        {
            //get the connection string stored separately (for security reasons)
            //note: dark art secrets are in local.settings.json
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
        /// Given a id will return parsed person from main list
        /// Returns empty person if, no person found
        /// </summary>
        public static async Task<Person> GetPersonFromId(string personId)
        {
            var personListXml = await GetPersonListFile();
            var foundPersonXml = await FindPersonById(personListXml, personId);

            if (foundPersonXml == null) { return Person.Empty; }

            var foundPerson = Person.FromXml(foundPersonXml);

            return foundPerson;
        }

        /// <summary>
        /// Find all person's xml element owned by User/Visitor ID
        /// Note:
        /// - multiple comma seperated UserId in Person profile
        /// - 1 person profile can have multiple user id (shared)
        /// - document is inputed instead for wide compatibilty
        /// </summary>
        public static List<XElement> FindPersonByUserId(XDocument personListXml, string userId)
        {
            var returnList = new List<XElement>();

            //add all person profiles that have the given user ID
            var allPersonList = personListXml.Root?.Elements();
            foreach (var personXml in allPersonList)
            {
                var allOwnerId = personXml.Element("UserId")?.Value ?? "";
                //check if inputed ID is found in list
                if (allOwnerId.Contains(userId)) { returnList.Add(personXml); }
            }

            return returnList;
        }


        /// <summary>
        /// Will look for a person in a given list
        /// returns null if no person found
        /// </summary>
        public static async Task<XElement?> FindPersonById(XDocument personListXmlDoc, string personId)
        {
            try
            {
                //list of person XMLs
                var personXmlList = personListXmlDoc.Root?.Elements();

                //do the finding
                var foundPerson = personXmlList?.Where(delegate (XElement personXml)
                {   //use id to find the person's record
                    var thisId = Person.FromXml(personXml).Id;
                    return thisId == personId;
                }).First();

                return foundPerson;
            }
            catch (Exception e)
            {
                //if fail log it and return empty xelement
                await APILogger.Error(e, null);
                return null;
            }
        }

        /// <summary>
        /// Gets user data, if user does
        /// not exist makes a new one & returns that
        /// Note :
        /// - email is used to find user, not hash or id (unique)
        /// - Uses UserDataList.xml
        /// </summary>
        public static async Task<UserData> GetUserData(string id, string name, string email)
        {
            //get user data list file (UserDataList.xml) Azure storage
            var userDataListXml = await GetXmlFileFromAzureStorage(UserDataListXml, BlobContainerName);

            //look for user with matching email
            var foundUserXml = userDataListXml.Root?.Elements()
                .Where(userDataXml => userDataXml.Element("Email")?.Value == email)?
                .FirstOrDefault();

            //if user found, initialize xml and send that
            if (foundUserXml != null) { return UserData.FromXml(foundUserXml); }

            //if no user found, make new user and send that
            else
            {
                //create new user from google's data
                var newUser = new UserData(id, name, email);

                //add new user xml to main list
                await AddXElementToXDocumentAzure(newUser.ToXml(), UserDataListXml, BlobContainerName);

                //return newly created user to caller
                return newUser;
            }

        }

        /// <summary>
        /// Given a user data it will find mathcing user email and replace the existing UserData with inputed
        /// Note :
        /// - Uses UserDataList.xml
        /// </summary>
        public static async Task<UserData> UpdateUserData(string id, string name, string email)
        {
            //get user data list file (UserDataList.xml) Azure storage
            var userDataListXml = await GetXmlFileFromAzureStorage(UserDataListXml, BlobContainerName);

            //look for user with matching email
            var foundUserXml = userDataListXml.Root?.Elements()
                .Where(userDataXml => userDataXml.Element("Email")?.Value == email)?
                .FirstOrDefault();

            //if user found, initialize xml and send that
            if (foundUserXml != null) { return UserData.FromXml(foundUserXml); }

            //if no user found, make new user and send that
            else
            {
                //create new user from google's data
                var newUser = new UserData(id, name, email);

                //add new user xml to main list
                await AddXElementToXDocumentAzure(newUser.ToXml(), UserDataListXml, BlobContainerName);

                //return newly created user to caller
                return newUser;
            }

        }

        /// <summary>
        /// Gets main person list xml doc file 
        /// </summary>
        /// <returns></returns>
        private static async Task<XDocument> GetPersonListFile() => await GetXmlFileFromAzureStorage("PersonList.xml", "vedastro-site-data");

        /// <summary>
        /// Get parsed HoroscopeDataList.xml from wwwroot file / static site
        /// Note: auto caching is used
        /// </summary>
        public static async Task<List<HoroscopeData>> GetHoroscopeDataList()
        {
            //if prediction list already loaded use that instead
            if (SavedHoroscopeDataList != null) { return SavedHoroscopeDataList; }

            //get data list from Static Website storage
            var horoscopeDataListXml = await Tools.GetXmlFileHttp(UrlHoroscopeDataListXml);

            //parse each raw event data in list
            var horoscopeDataList = new List<HoroscopeData>();
            foreach (var predictionDataXml in horoscopeDataListXml)
            {
                //add it to the return list
                horoscopeDataList.Add(HoroscopeData.FromXml(predictionDataXml));
            }

            //make a copy to be used later if needed (speed improve)
            SavedHoroscopeDataList = horoscopeDataList;

            return horoscopeDataList;

        }


        /// <summary>
        /// Gets any file at given url as string
        /// </summary>
        public static async Task<string> GetStringFileHttp(string url)
        {
            //get the data sender
            using var client = new HttpClient();

            //load xml event data files before hand to be used quickly later for search
            //get main horoscope prediction file (located in wwwroot)
            var fileString = await client.GetStringAsync(url);

            return fileString;
        }


        /// <summary>
        /// Gets XML file from Azure blob storage
        /// </summary>
        public static async Task<XDocument> GetXmlFileFromAzureStorage(string fileName, string blobContainerName)
        {
            var fileClient = await GetBlobClientAzure(fileName, blobContainerName);
            var xmlFile = await BlobClientToXmlDoc(fileClient);

            return xmlFile;
        }

        /// <summary>
        /// Gets any file from Azure blob storage in string form
        /// </summary>
        public static async Task<string> GetStringFileFromAzureStorage(string fileName, string blobContainerName)
        {
            var fileClient = await GetBlobClientAzure(fileName, blobContainerName);
            var xmlFile = await BlobClientToString(fileClient);

            return xmlFile;
        }

        /// <summary>
        /// Gets all horoscope predictions for a person
        /// </summary>
        public static async Task<List<HoroscopePrediction>> GetPrediction(Person person)
        {

            //get list of horoscope data (file from wwwroot)
            var horoscopeDataList = await GetHoroscopeDataList();

            //start calculating predictions (mix with time by person's birth date)
            var predictionList = calculate(person, horoscopeDataList);


            return predictionList;

            /// <summary>
            /// Get list of predictions occurring in a time period for all the
            /// inputed prediction types aka "prediction data"
            /// </summary>
            List<HoroscopePrediction> calculate(Person person, List<HoroscopeData> horoscopeDataList)
            {
                //get data to instantiate muhurtha time period
                //get start & end times

                //initialize empty list of event to return
                List<HoroscopePrediction> horoscopeList = new();

                try
                {
                    foreach (var horoscopeData in horoscopeDataList)
                    {
                        //only add if occuring
                        var isOccuring = horoscopeData.IsEventOccuring(person.BirthTime, person);
                        if (isOccuring)
                        {
                            var newHoroscopePrediction = new HoroscopePrediction(horoscopeData.Name, horoscopeData.Description, horoscopeData.RelatedBody);
                            //add events to main list of event
                            horoscopeList.Add(newHoroscopePrediction);
                        }
                    }

                }
                //catches only exceptions that indicates that user canceled the calculation (caller lost interest in the result)
                catch (Exception)
                {
                    //return empty list
                    return new List<HoroscopePrediction>();
                }


                //return calculated event list
                return horoscopeList;
            }
        }



        /// <summary>
        /// If start time and end time is same then will only return 1 time in list
        /// </summary>
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

        public static async Task<HttpResponseMessage> GetRequest(string receiverAddress)
        {
            //prepare the data to be sent
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, receiverAddress);

            //get the data sender
            using var client = new HttpClient();

            //tell sender to wait for complete reply before exiting
            var waitForContent = HttpCompletionOption.ResponseContentRead;

            //send the data on its way
            var response = await client.SendAsync(httpRequestMessage, waitForContent);

            //return the raw reply to caller
            return response;
        }

        public static async Task UpdatePersonRecord(XElement updatedPersonXml)
        {
            var updatedPerson = Person.FromXml(updatedPersonXml);

            //get the person record that needs to be updated
            var personListXmlDoc = await APITools.GetXmlFileFromAzureStorage(APITools.PersonListFile, APITools.BlobContainerName);
            var personToUpdate = await APITools.FindPersonById(personListXmlDoc, updatedPerson.Id);

            //delete the previous person record,
            //and insert updated record in the same place
            personToUpdate.ReplaceWith(updatedPersonXml);

            //upload modified list file to storage
            await APITools.SaveXDocumentToAzure(personListXmlDoc, APITools.PersonListFile, APITools.BlobContainerName);
        }

        public static async Task<List<Person>> GetAllPersonList()
        {
            //get all person list from storage
            var personListXml = await APITools.GetXmlFileFromAzureStorage(APITools.PersonListFile, APITools.BlobContainerName);
            var allPersonList = personListXml.Root?.Elements();

            var returnList = Person.FromXml(allPersonList);

            return returnList;
        }
    }
}
