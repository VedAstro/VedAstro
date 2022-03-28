using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using Azure.Storage.Blobs;
using Genso.Astrology.Library;
using Genso.Astrology.Library.Compatibility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace API
{
    public static class EntryPoint
    {

        [FunctionName("match")]
        public static async Task<IActionResult> Match(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            [Blob("vedastro-site-data/PersonList.xml", FileAccess.Read)] Stream personListRead,
            ILogger log)
        {
            string responseMessage;

            try
            {
                //get name of male & female
                dynamic names = await ExtractNames(req);
                
                //get list of all people
                var personList = new Data(personListRead);

                //generate compatibility report
                CompatibilityReport compatibilityReport = GetCompatibilityReport(names.Male, names.Female, personList);
                responseMessage = compatibilityReport.ToXML().ToString();
            }
            catch (Exception e)
            {
                responseMessage = $"Message\n{e.Message}\n";
                responseMessage += $"Data\n{e.Data}\n";
                responseMessage += $"InnerException\n{e.InnerException}\n";
                responseMessage += $"Source\n{e.Source}\n";
                responseMessage += $"StackTrace\n{e.StackTrace}\n";
                responseMessage += $"StackTrace\n{e.TargetSite}\n";
            }


            var okObjectResult = new OkObjectResult(responseMessage);
            //okObjectResult.ContentTypes.Add("text/html");
            return okObjectResult;
        }

        [FunctionName("addperson")]
        public static async Task<IActionResult> AddPerson(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestMessage req,
            [Blob("vedastro-site-data/PersonList.xml", FileAccess.ReadWrite)]BlobClient personListClient)
        {
            string responseMessage = "";

            var newPersonXml = ExtractXmlFromRequest(req);

            try
            {

                //get person list from storage
                var personListXml = GetXmlFile(personListClient);

                //add new person to list
                personListXml.Root.Add(newPersonXml);

                //upload modified list to storage
                var updatedPersonListStream = GenerateStreamFromString(personListXml.ToString());
                await personListClient.UploadAsync(updatedPersonListStream, overwrite: true );

            }
            catch (Exception e)
            {
                responseMessage += $"#Message#\n{e.Message}\n";
                responseMessage += $"#Data#\n{e.Data}\n";
                responseMessage += $"#InnerException#\n{e.InnerException}\n";
                responseMessage += $"#Source#\n{e.Source}\n";
                responseMessage += $"#StackTrace#\n{e.StackTrace}\n";
                responseMessage += $"#StackTrace#\n{e.TargetSite}\n";
            }


            var okObjectResult = new OkObjectResult(responseMessage);

            return okObjectResult;
        }

        static XElement ExtractXmlFromRequest(HttpRequestMessage request)
        {
            //get xml string from caller
            var xmlString = RequestToXmlString(request);

            //parse xml string
            var xml = XElement.Parse(xmlString);

            return xml;
        }

        static XDocument GetXmlFile(BlobClient blobClient)
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
        /// When receiving
        /// Extracts the raw data coming from the server, and extract what is needed
        ///  here original data is overwritten
        /// </summary>
        public static string RequestToXmlString(HttpRequestMessage rawData)
        {
            //get request body
            return rawData.Content.ReadAsStringAsync().Result;

            

            //extract needed data from reply
            //_status = rawRequest.StatusCode;

            //_message = parseMessage(rawRequest);

        }


        /// <summary>
        /// Extracts names from the query URL
        /// </summary>
        private static async Task<object> ExtractNames(HttpRequest request)
        {
            string male = request.Query["male"];
            string female = request.Query["female"];

            string requestBody = await new StreamReader(request.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            male = male ?? data?.male;
            female = female ?? data?.female;

            return new { Male = male, Female = female };

        }

        static CompatibilityReport GetCompatibilityReport(string maleName, string femaleName, Data personList)
        {
            //get all the people
            var peopleList = DatabaseManager.GetPersonList(personList);

            //filter out the male and female ones we want
            var male = peopleList.Find(person => person.GetName() == maleName);
            var female = peopleList.Find(person => person.GetName() == femaleName);

            return MatchCalculator.GetCompatibilityReport(male, female);
        }


    }
}
