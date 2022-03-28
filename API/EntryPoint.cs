using System;
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
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace API
{
    public static class EntryPoint
    {

        [FunctionName("getmatchreport")]
        public static async Task<IActionResult> Match(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            [Blob("vedastro-site-data/PersonList.xml", FileAccess.Read)] Stream personListRead,
            ILogger log)
        {
            string responseMessage;

            try
            {
                //get name of male & female
                dynamic names = await Tools.ExtractNames(req);

                //get list of all people
                var personList = new Data(personListRead);

                //generate compatibility report
                CompatibilityReport compatibilityReport = Tools.GetCompatibilityReport(names.Male, names.Female, personList);
                responseMessage = compatibilityReport.ToXML().ToString();
            }
            catch (Exception e)
            {
                //format error nicely to show user
                responseMessage = Tools.FormatErrorReply(e);
            }


            var okObjectResult = new OkObjectResult(responseMessage);
            //okObjectResult.ContentTypes.Add("text/html");
            return okObjectResult;
        }

        [FunctionName("addperson")]
        public static async Task<IActionResult> AddPerson(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestMessage incomingRequest,
            [Blob("vedastro-site-data/PersonList.xml", FileAccess.ReadWrite)] BlobClient personListClient)
        {
            var responseMessage = "";

            try
            {
                //get new person data out of incoming request 
                var newPersonXml = Tools.ExtractDataFromRequest(incomingRequest);

                //add new person to main list
                var personListXml = Tools.AddXElementToXDocument(personListClient, newPersonXml);

                //upload modified list to storage
                await Tools.OverwriteBlobData(personListClient, personListXml);

            }
            catch (Exception e)
            {
                //format error nicely to show user
                responseMessage = Tools.FormatErrorReply(e);
            }


            var okObjectResult = new OkObjectResult(responseMessage);

            return okObjectResult;
        }

        [FunctionName("getmalelist")]
        public static async Task<IActionResult> GetMaleList(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestMessage incomingRequest,
            [Blob("vedastro-site-data/PersonList.xml", FileAccess.ReadWrite)] BlobClient personListClient)
        {
            var responseMessage = "";

            try
            {

                //get person list from storage
                var personListXml = Tools.BlobClientToXml(personListClient);

                //get only male ppl into a list
                var maleList = from person in personListXml.Root?.Elements()
                    where person.Element("Gender")?.Value == "Male"
                    select person;

                //send male list to caller
                responseMessage = new XElement("Root", maleList).ToString();

            }
            catch (Exception e)
            {
                //format error nicely to show user
                responseMessage = Tools.FormatErrorReply(e);
            }


            var okObjectResult = new OkObjectResult(responseMessage);

            return okObjectResult;
        }

        [FunctionName("getfemalelist")]
        public static async Task<IActionResult> GetFemaleList(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestMessage incomingRequest,
            [Blob("vedastro-site-data/PersonList.xml", FileAccess.ReadWrite)] BlobClient personListClient)
        {
            var responseMessage = "";

            try
            {

                //get person list from storage
                var personListXml = Tools.BlobClientToXml(personListClient);

                //get only female ppl into a list
                var maleList = from person in personListXml.Root?.Elements()
                    where person.Element("Gender")?.Value == "Female"
                    select person;

                //send female list to caller
                responseMessage = new XElement("Root", maleList).ToString();

            }
            catch (Exception e)
            {
                //format error nicely to show user
                responseMessage = Tools.FormatErrorReply(e);
            }


            var okObjectResult = new OkObjectResult(responseMessage);

            return okObjectResult;
        }


    }
}
