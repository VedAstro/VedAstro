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
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

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
                dynamic names = await APITools.ExtractNames(req);

                //get list of all people
                var personList = new Data(personListRead);

                //generate compatibility report
                CompatibilityReport compatibilityReport = APITools.GetCompatibilityReport(names.Male, names.Female, personList);
                responseMessage = compatibilityReport.ToXml().ToString();
            }
            catch (Exception e)
            {
                //format error nicely to show user
                responseMessage = APITools.FormatErrorReply(e);
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
                var newPersonXml = APITools.ExtractDataFromRequest(incomingRequest);

                //add new person to main list
                var personListXml = APITools.AddXElementToXDocument(personListClient, newPersonXml);

                //upload modified list to storage
                await APITools.OverwriteBlobData(personListClient, personListXml);

                responseMessage = new XElement("Status", "Success").ToString();

            }
            catch (Exception e)
            {
                //format error nicely to show user
                responseMessage = APITools.FormatErrorReply(e);
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
                var personListXml = APITools.BlobClientToXml(personListClient);

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
                responseMessage = APITools.FormatErrorReply(e);
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
                var personListXml = APITools.BlobClientToXml(personListClient);

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
                responseMessage = APITools.FormatErrorReply(e);
            }


            var okObjectResult = new OkObjectResult(responseMessage);

            return okObjectResult;
        }

        //[FunctionName("getpeoplelist")]
        //public static async Task<IActionResult> GetPeopleList(
        //    [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestMessage incomingRequest,
        //    [Blob("vedastro-site-data/PersonList.xml", FileAccess.ReadWrite)] BlobClient personListClient)
        //{
        //    var responseMessage = "";

        //    try
        //    {

        //        //get person list from storage
        //        var personListXml = APITools.BlobClientToXml(personListClient);

        //        //get only female ppl into a list
        //        var maleList = from person in personListXml.Root?.Elements()
        //            where person.Element("Gender")?.Value == "Female"
        //            select person;

        //        //send female list to caller
        //        responseMessage = new XElement("Root", maleList).ToString();

        //    }
        //    catch (Exception e)
        //    {
        //        //format error nicely to show user
        //        responseMessage = APITools.FormatErrorReply(e);
        //    }


        //    var okObjectResult = new OkObjectResult(responseMessage);

        //    return okObjectResult;
        //}

        [FunctionName("getevents")]
        public static async Task<IActionResult> GetEvents(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestMessage incomingRequest,
            [Blob("vedastro-site-data/EventDataList.xml", FileAccess.ReadWrite)] BlobClient eventDataListClient)
        {
            var responseMessage = "";

            try
            {

                //get person list from storage
                var eventDataListXml = APITools.BlobClientToXml(eventDataListClient);

                //get data needed to generate events
                var requestData = APITools.ExtractDataFromRequest(incomingRequest);

                //parse it
                var person = Person.FromXml(requestData.Element("Person"));
                var startTime = Time.FromXml(requestData.Element("StartTime").Element("Time"));
                var endTime = Time.FromXml(requestData.Element("EndTime").Element("Time"));
                var location = GeoLocation.FromXml(requestData.Element("Location"));
                var tag =  Tools.XmlToAnyType<EventTag>(requestData.Element(typeof(EventTag).FullName));
                var precision = Tools.XmlToAnyType<double>(requestData.Element(typeof(double).FullName));

                //calculate events from the data received
                var events = CalculateEvents(startTime, endTime, location, person, tag, precision, eventDataListXml);

                //convert events to XML for sending
                var rootXml = new XElement("Root");
                foreach (var _event in events)
                {
                    rootXml.Add(_event.ToXml());
                }

                responseMessage = rootXml.ToString();

            }
            catch (Exception e)
            {
                //format error nicely to show user
                responseMessage = APITools.FormatErrorReply(e);
            }


            var okObjectResult = new OkObjectResult(responseMessage);

            return okObjectResult;
        }


        public static List<Event> CalculateEvents(Time startTime, Time endTime, GeoLocation location, Person person, EventTag tag, double precision, XDocument dataEventdatalistXml)
        {

            //parse each raw event data in list
            var eventDataList = new List<EventData>();
            foreach (var eventData in dataEventdatalistXml.Root.Elements())
            {
                //add it to the return list
                eventDataList.Add(EventData.ToXml(eventData));
            }

            //get all event data/types which has the inputed tag (FILTER)
            var eventDataListFiltered = DatabaseManager.GetEventDataListByTag(tag, eventDataList);

            //TODO enable when can
            ////debug to measure event calculation time
            //var watch = Stopwatch.StartNew();

            //start calculating events
            var eventList = EventManager.GetEventsInTimePeriod(startTime.GetStdDateTimeOffset(), endTime.GetStdDateTimeOffset(), location, person, precision, eventDataListFiltered);

            //watch.Stop();
            //LogManager.Debug($"Events computed in: { watch.Elapsed.TotalSeconds}s");

            return eventList;
        }

    }
}
