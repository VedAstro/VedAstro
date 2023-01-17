using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;
using Genso.Astrology.Library;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;

namespace API
{

    public struct SumData
    {
        public double NatureScore = 0;
        public List<PlanetName> Planet;
        public Time BirthTime;

        public SumData()
        {
            Planet = null;
        }
    }

    public static class EventsChartAPI
    {
        //▄▀█ █▀█ █   █▀▀ █░█ █▄░█ █▀▀ ▀█▀ █ █▀█ █▄░█ █▀
        //█▀█ █▀▀ █   █▀░ █▄█ █░▀█ █▄▄ ░█░ █ █▄█ █░▀█ ▄█


        /// <summary>
        /// Generates a new SVG dasa report given a person id
        /// Exp call:
        /// <Root>
        //      <PersonId>374117709</PersonId>
        //      <StartTime>
        //          <Time>
        //              <StdTime>00:00 01/01/1994 +08:00</StdTime>
        //              <Location>
        //                  <Name>Teluk Intan</Name>
        //                  <Longitude>101.0206</Longitude>
        //                  <Latitude>4.0224</Latitude>
        //              </Location>
        //          </Time>
        //  </StartTime>
        //  <EndTime>
        //      <Time>
        //          <StdTime>11:59 31/12/2024 +08:00</StdTime>
        //          <Location>
        //              <Name>Teluk Intan</Name>
        //              <Longitude>101.0206</Longitude>
        //              <Latitude>4.0224</Latitude>
        //          </Location>
        //      </Time>
        //  </EndTime>
        //  <DaysPerPixel>11</DaysPerPixel>
        //</Root>
        /// </summary>
        [FunctionName("geteventschart")]
        public static async Task<IActionResult> GetEventsChart(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestMessage incomingRequest)
        {
            try
            {
                //get dasa report for sending
                var chart = await GetEventReportSvgForIncomingRequest(incomingRequest);

                //send image back to caller
                //convert svg string to stream for sending
                var stream = GenerateStreamFromString(chart.ContentSvg);
                var x = StreamToByteArray(stream);
                return new FileContentResult(x, "image/svg+xml");

            }
            catch (Exception e)
            {
                //log error
                await ApiLogger.Error(e, incomingRequest);

                //format error nicely to show user
                return APITools.FailMessage(e);
            }

        }

        [FunctionName("findbirthtimedasa")]
        public static async Task<IActionResult> FindBirthTimeDasa(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestMessage incomingRequest)
        {
            try
            {
                //get data needed to generate multiple charts
                //accurate life event
                var lifeEvent = new LifeEvent();
                lifeEvent.StartTime = "00:00 01/01/2014";
                lifeEvent.Location = "Malaysia";
                //view little ahead & forward of event
                var eventTime = await lifeEvent.GetDateTimeOffset();
                var startTime = new Time(eventTime.AddMonths(-1), await lifeEvent.GetGeoLocation());
                var endTime = new Time(eventTime.AddMonths(1), await lifeEvent.GetGeoLocation());

                //person
                var person = await APITools.GetPersonFromId("54041d1ffb494a79997f7987ecfcf08b5");
                //zoom level
                //possible birth times
                var timeSkip = 1;// 1 hour
                var possibleTimeList = new List<Time>();
                Time previousTime = person.BirthTime;//start with birth time
                for (int i = 0; i < 3; i++) //5 possibilities
                {
                    //save to be added upon later
                    previousTime = previousTime.AddHours(timeSkip);
                    possibleTimeList.Add(previousTime);
                }

                //generate the needed charts
                var chartList = new List<Chart>();
                var dasaEventTags = new List<EventTag> { EventTag.Dasa, EventTag.Bhukti, EventTag.Antaram, EventTag.Gochara, EventTag.DasaSpecialRules };
                //PRECISION
                //calculate based on max screen width,
                //done for fast calculation only for needed viewability
                var daysPerPixel = GetDayPerPixel(startTime, endTime, 800);


                var combinedSvg = "";
                var chartYPosition = 30; //start with top padding
                var leftPadding = 10;
                foreach (var possibleTime in possibleTimeList)
                {
                    //replace original birth time
                    var personAdjusted = person.ChangeBirthTime(possibleTime);
                    var newChart = await GetChart(personAdjusted, startTime, endTime, daysPerPixel, dasaEventTags);
                    var adjustedBirth = personAdjusted.BirthTimeString;

                    //place in group with time above the chart
                    var wrappedChart = $@"
                            <g transform=""matrix(1, 0, 0, 1, {leftPadding}, {chartYPosition})"">
                                <text style=""font-size: 16px; white-space: pre-wrap;"" x=""2"" y=""-6.727"">{adjustedBirth}</text>
                                {newChart.ContentSvg}
                              </g>
                            ";

                    //combine charts together
                    combinedSvg += wrappedChart;

                    //next chart goes below this one
                    //todo get actual chart height for dynamic stacking
                    chartYPosition += 270;
                }

                //put all charts in 1 big container
                var finalSvg = WrapSvgElements(combinedSvg, 800, chartYPosition);


                //send image back to caller
                //convert svg string to stream for sending
                var stream = GenerateStreamFromString(finalSvg);
                var x = StreamToByteArray(stream);
                return new FileContentResult(x, "image/svg+xml");
            }
            catch (Exception e)
            {
                //log error
                await ApiLogger.Error(e, incomingRequest);

                //format error nicely to show user
                return APITools.FailMessage(e);
            }
        }

        [FunctionName("findbirthtimerisingsign")]
        public static async Task<IActionResult> FindBirthTimeRisingSign(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestMessage incomingRequest)
        {
            string responseMessage;

            try
            {
                //get person
                var person = await APITools.GetPersonFromId("05abf671c8bf4e4badc07a086c067f26");
                //var person = await APITools.GetPersonFromId("54041d1ffb494a79997f7987ecfcf08b5");

                //get all birth time with different rising sign
                var possibleTimeList = await GetTimeList(person);

                //calculate predictions
                List<HoroscopePrediction> predictionList = new List<HoroscopePrediction>();
                foreach (var timeSlice in possibleTimeList)
                {
                    //replace original birth time
                    var personAdjusted = person.ChangeBirthTime(timeSlice);

                    var x = await APITools.GetPrediction(personAdjusted);
                }

                //convert list to xml string in root elm
                responseMessage = Tools.AnyTypeToXmlList(predictionList).ToString();
            }
            catch (Exception e)
            {
                //log error
                await ApiLogger.Error(e, incomingRequest);
                //format error nicely to show user
                return APITools.FailMessage(e);
            }

            var okObjectResult = new OkObjectResult(responseMessage);
            return okObjectResult;

            //LOCAL FUNCTIONS
            async Task<List<Time>> GetTimeList(Person person)
            {

                //start of day till end of day
                var dayStart = new Time($"00:00 {person.BirthDateMonthYear} {person.BirthTimeZone}", person.GetBirthLocation());
                var dayEnd = new Time($"23:59 {person.BirthDateMonthYear} {person.BirthTimeZone}", person.GetBirthLocation());

                var returnList = new List<Time>();
                var timeSkip = 1;// 1 hour
                var reachedDayEnd = false;
                var checkTime = dayStart; //from the start of the day
                var previousSign = AstronomicalCalculator.GetHouseSignName(1, checkTime);
                //add 1st sign at start of day to list
                returnList.Add(checkTime);

                //scan through the day & add mark each time new sign rises
                //stop looking when reached end of day
                while (!reachedDayEnd)
                {
                    //if different from previous sign, then add time slice to list
                    var checkSign = AstronomicalCalculator.GetHouseSignName(1, checkTime);
                    if (checkSign != previousSign) { returnList.Add(checkTime); }

                    //update previous sign
                    previousSign = checkSign;

                    //goto to next time slice
                    checkTime = checkTime.AddHours(timeSkip);

                    //if next time slice goes to next day, then end it here
                    if (checkTime > dayEnd) { reachedDayEnd = true; }
                }

                return returnList;

            }
        }


        /// <summary>
        /// Special function to make Light Viewer(EventsChartViewer.html) work
        /// </summary>
        [FunctionName("geteventscharteasy")]
        public static async Task<IActionResult> GetEventsChartEasy(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestMessage incomingRequest)
        {
            try
            {
                //TODO caching
                //TODO get saved chart if any
                //var rootXml = APITools.ExtractDataFromRequest(req);

                //get dasa report for sending
                var chart = await GetEventReportSvgForIncomingRequestEasy(incomingRequest);

                //send image back to caller
                //convert svg string to stream for sending
                var stream = GenerateStreamFromString(chart.ContentSvg);
                var x = StreamToByteArray(stream);
                return new FileContentResult(x, "image/svg+xml");
            }
            catch (Exception e)
            {
                //log error
                await ApiLogger.Error(e, incomingRequest);

                //format error nicely to show user
                return APITools.FailMessage(e);
            }
        }


        /// <summary>
        /// Gets chart from saved list, needs to be given id to find
        /// </summary>
        [FunctionName("getsavedeventschart")]
        public static async Task<IActionResult> GetSavedEventsChart(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestMessage incomingRequest)
        {

            try
            {
                //get id that will be used find the person
                var requestData = APITools.ExtractDataFromRequest(incomingRequest);
                var chartId = requestData.Value;

                //get the saved chart record by id
                var savedChartListXml = await APITools.GetXmlFileFromAzureStorage(APITools.SavedChartListFile, APITools.BlobContainerName);
                var foundChartXml = await APITools.FindChartById(savedChartListXml, chartId);
                var chart = Chart.FromXml(foundChartXml);

                //send image back to caller
                //convert svg string to stream for sending
                var stream = GenerateStreamFromString(chart.ContentSvg);
                var x = StreamToByteArray(stream);
                return new FileContentResult(x, "image/svg+xml");

            }
            catch (Exception e)
            {
                //log error
                await ApiLogger.Error(e, incomingRequest);

                //format error nicely to show user
                return APITools.FailMessage(e);
            }

        }


        /// <summary>
        /// Special function to access Saved Chart directly without website
        /// </summary>
        [FunctionName("savedchart")]
        public static async Task<IActionResult> GetSavedEventsChartDirect([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "savedchart/{chartId}")]
            HttpRequestMessage incomingRequest, string chartId)
        {
            try
            {

                //get the saved chart record by id
                var savedChartListXml = await APITools.GetXmlFileFromAzureStorage(APITools.SavedChartListFile, APITools.BlobContainerName);
                var foundChartXml = await APITools.FindChartById(savedChartListXml, chartId);
                var chart = Chart.FromXml(foundChartXml);
                var svgString = chart.ContentSvg;

                //get chart index.html and send that to caller
                //get data list from Static Website storage
                var htmlTemplate = await APITools.GetStringFileHttp(APITools.UrlEventsChartViewerHtml);

                //insert person name into page, to show ready page faster
                var personName = (await APITools.GetPersonFromId(chart.PersonId)).Name;
                var jsVariables = $@"window.PersonName = ""{personName}"";";
                jsVariables += $@"window.ChartType = ""{"Muhurtha"}"";";
                jsVariables += $@"window.PersonId = ""{chart.PersonId}"";";
                htmlTemplate = htmlTemplate.Replace("/*INSERT-JS-VAR-HERE*/", jsVariables);

                //insert SVG into html place holder page
                htmlTemplate = htmlTemplate.Replace("<!--INSERT SVG-->", svgString);

                return new ContentResult { Content = htmlTemplate, ContentType = "text/html" };


            }
            catch (Exception e)
            {
                //log error
                await ApiLogger.Error(e, incomingRequest);

                //format error nicely to show user
                return APITools.FailMessage(e);
            }

        }


        /// <summary>
        /// Returns EventsChartViewer.html with injected variables
        /// Special function to generate new Events Chart directly without website
        /// Does not do anything to generate the chart, that is done by
        /// geteventscharteasy API called by the JS in HTML
        /// </summary>
        [FunctionName("chart")]
        public static async Task<IActionResult> GetEventsChartDirect([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "chart/{personId}/{eventPreset}/{timePreset}")]
            HttpRequestMessage incomingRequest, string personId, string eventPreset, string timePreset)
        {
            try
            {
                //get chart index.html and send that to caller
                var eventsChartViewerHtml = await APITools.GetStringFileHttp(APITools.UrlEventsChartViewerHtml);


                //insert person name into page, to show ready page faster
                var personName = (await APITools.GetPersonFromId(personId)).Name;
                var jsVariables = $@"window.PersonName = ""{personName}"";";
                jsVariables += $@"window.ChartType = ""{"Muhurtha"}"";";
                jsVariables += $@"window.PersonId = ""{personId}"";";
                var finalHtml = eventsChartViewerHtml.Replace("/*INSERT-JS-VAR-HERE*/", jsVariables);


                return new ContentResult { Content = finalHtml, ContentType = "text/html" };

            }
            catch (Exception e)
            {
                //log error
                await ApiLogger.Error(e, incomingRequest);

                //format error nicely to show user
                return APITools.FailMessage(e);
            }

        }


        /// <summary>
        /// Given a chart id it will return the person id for that chart
        /// </summary>
        [FunctionName("getpersonidfromsavedchartid")]
        public static async Task<IActionResult> GetPersonIdFromSavedChartId(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestMessage incomingRequest)
        {

            try
            {
                //get id that will be used find the person
                var requestData = APITools.ExtractDataFromRequest(incomingRequest);
                var chartId = requestData.Value;

                //get the saved chart record by id
                var savedChartListXml = await APITools.GetXmlFileFromAzureStorage(APITools.SavedChartListFile, APITools.BlobContainerName);
                var foundChartXml = await APITools.FindChartById(savedChartListXml, chartId);
                var chart = Chart.FromXml(foundChartXml);

                //extract out the person id from chart and send it to caller
                var personIdXml = new XElement("PersonId", chart.PersonId);

                return APITools.PassMessage(personIdXml);

            }
            catch (Exception e)
            {
                //log error
                await ApiLogger.Error(e, incomingRequest);

                //format error nicely to show user
                return APITools.FailMessage(Tools.ExceptionToXml(e));
            }

        }


        /// <summary>
        /// Saves the chart in Azure Storage
        /// </summary>
        [FunctionName("SaveEventsChart")]
        public static async Task<IActionResult> SaveEventsChart(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestMessage incomingRequest)
        {
            try
            {
                //generate report
                var chart = await GetEventReportSvgForIncomingRequest(incomingRequest);

                //save chart into storage
                //note: do not wait to speed things up, beware! failure will go undetected on client
                APITools.AddXElementToXDocumentAzure(chart.ToXml(), APITools.SavedChartListFile, APITools.BlobContainerName);

                //let caller know all good
                return APITools.PassMessage();

            }
            catch (Exception e)
            {
                //log error
                await ApiLogger.Error(e, incomingRequest);

                //format error nicely to show user
                return APITools.FailMessage(e);
            }


        }

        /// <summary>
        /// make a name & id only xml list of saved charts
        /// note: done so to not download what is not needed
        ///
        /// client will use data from here to get the actual saved chart
        /// </summary>
        [FunctionName("getsavedchartnamelist")]
        public static async Task<IActionResult> GetSavedChartNameList(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestMessage incomingRequest)
        {
            try
            {

                //get ful saved chart list
                var savedChartXmlDoc = await APITools.GetXmlFileFromAzureStorage(APITools.SavedChartListFile, APITools.BlobContainerName);

                //extract out the name & id
                var rootXml = new XElement("Root");

                //make a name & id only xml list of saved charts
                //note: done so to not download what is not needed
                var savedChartXmlList = savedChartXmlDoc?.Root?.Elements().ToList() ?? new List<XElement>(); //empty list so no failure
                foreach (var chartXml in savedChartXmlList)
                {
                    var parsedChart = Chart.FromXml(chartXml);
                    var person = await APITools.GetPersonFromId(parsedChart.PersonId);
                    var chartNameXml = new XElement("ChartName",
                        new XElement("Name",
                            parsedChart.GetFormattedName(person.Name)),
                        new XElement("ChartId",
                            parsedChart.ChartId));

                    //add to final xml
                    rootXml.Add(chartNameXml);
                }


                return APITools.PassMessage(rootXml);
            }
            catch (Exception e)
            {
                //log error
                await ApiLogger.Error(e, incomingRequest);

                //format error nicely to show user
                return APITools.FailMessage(e);
            }


        }


        [FunctionName("deletesavedchart")]
        public static async Task<IActionResult> DeleteSavedChart(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestMessage incomingRequest)
        {

            try
            {
                //get unedited hash & updated person details from incoming request
                var requestData = APITools.ExtractDataFromRequest(incomingRequest);
                var chartId = requestData.Value;


                //get the person record that needs to be deleted
                var savedChartListXml = await APITools.GetXmlFileFromAzureStorage(APITools.SavedChartListFile, APITools.BlobContainerName);
                var chartToDelete = await APITools.FindChartById(savedChartListXml, chartId);

                //delete the chart record,
                chartToDelete.Remove();

                //upload modified list to storage
                var savedChartListClient = await APITools.GetBlobClientAzure(APITools.SavedChartListFile, APITools.BlobContainerName);
                await APITools.OverwriteBlobData(savedChartListClient, savedChartListXml);

                return APITools.PassMessage();
            }
            catch (Exception e)
            {
                //log error
                await ApiLogger.Error(e, incomingRequest);

                //format error nicely to show user
                return APITools.FailMessage(e);
            }
        }





        //█▀█ █▀█ █ █░█ ▄▀█ ▀█▀ █▀▀   █▀▄▀█ █▀▀ ▀█▀ █░█ █▀█ █▀▄ █▀
        //█▀▀ █▀▄ █ ▀▄▀ █▀█ ░█░ ██▄   █░▀░█ ██▄ ░█░ █▀█ █▄█ █▄▀ ▄█


        /// <summary>
        /// processes incoming xml request and outputs events svg report
        /// </summary>
        private static async Task<Chart> GetEventReportSvgForIncomingRequest(HttpRequestMessage req)
        {
            //get all the data needed out of the incoming request
            var rootXml = APITools.ExtractDataFromRequest(req);
            var personId = rootXml.Element("PersonId")?.Value;
            var eventTagListXml = rootXml.Element("EventTagList");
            var eventTags = EventTagExtensions.FromXmlList(eventTagListXml);
            var startTimeXml = rootXml.Element("StartTime")?.Elements().First();
            var startTime = Time.FromXml(startTimeXml);
            var endTimeXml = rootXml.Element("EndTime")?.Elements().First();
            var endTime = Time.FromXml(endTimeXml);
            var daysPerPixel = double.Parse(rootXml.Element("DaysPerPixel")?.Value ?? "0");


            //get the person instance by id
            var foundPerson = await APITools.GetPersonFromId(personId);

            return await GetChart(foundPerson, startTime, endTime, daysPerPixel, eventTags);
        }

        private static async Task<Chart> GetEventReportSvgForIncomingRequestEasy(HttpRequestMessage req)
        {
            //get all the data needed out of the incoming request
            var rootXml = APITools.ExtractDataFromRequest(req);
            var personId = rootXml.Element("PersonId")?.Value;
            var timePreset = rootXml.Element("TimePreset")?.Value;
            var eventPreset = rootXml.Element("EventPreset")?.Value;
            var maxWidth = int.Parse(rootXml.Element("MaxWidth")?.Value); //px
            //client's timezone, not based on birth
            var timezone = Tools.StringToTimezone(rootXml.Element("Timezone")?.Value);//should be "+08:00"

            //hard set max width to 1000px so that no forever calculation created
            maxWidth = maxWidth > 1000 ? 1000 : maxWidth;

            //minus 7% for side padding
            maxWidth = maxWidth - (int)(maxWidth * 0.07); ;


            //EXTRACT & PREPARE DATA

            //get the person instance by id
            var foundPerson = await APITools.GetPersonFromId(personId);

            //TIME
            dynamic x = AutoCalculateTimeRange(timePreset, foundPerson.GetBirthLocation(), timezone);
            Time startTime = x.start;
            Time endTime = x.end;

            //EVENTS
            var eventTags = GetSelectedEventTypesEasy(eventPreset);

            //PRECISION
            //calculate based on max screen width,
            //done for fast calculation only for needed viewability
            var daysPerPixel = GetDayPerPixel(startTime, endTime, maxWidth);


            return await GetChart(foundPerson, startTime, endTime, daysPerPixel, eventTags);
        }

        private static async Task<Chart> GetChart(Person foundPerson, Time startTime, Time endTime, double daysPerPixel, List<EventTag> eventTags)
        {
            //from person get svg report
            var eventsChartSvgString = await GenerateMainEventsChartSvg(foundPerson, startTime, endTime, daysPerPixel, eventTags);

            //a new chart is born
            var newChartId = Tools.GenerateId();
            var eventTagListXml = EventTagExtensions.ToXmlList(eventTags);
            var startTimeXml = startTime.ToXml();
            var endTimeXml = endTime.ToXml();

            var eventReportSvgForIncomingRequest = new Chart(newChartId, eventsChartSvgString, foundPerson.Id, eventTagListXml, startTimeXml, endTimeXml, daysPerPixel);

            return eventReportSvgForIncomingRequest;
        }

        /// <summary>
        /// calculates the precision of the events to fit inside 1000px width
        /// </summary>
        private static double GetDayPerPixel(Time start, Time end, int maxWidth)
        {
            //const int maxWidth = 1000; //px

            var daysBetween = end.Subtract(start).TotalDays;
            var daysPerPixel = Math.Round(daysBetween / maxWidth, 3); //small val = higher precision
                                                                      //var daysPerPixel = Math.Round(yearsBetween * 0.4, 3); //small val = higher precision
                                                                      //daysPerPixel = daysPerPixel < 1 ? 1 : daysPerPixel; // minimum 1 day per px

            //if final value is 0 then recalculate without rounding
            daysPerPixel = daysPerPixel <= 0 ? daysBetween / maxWidth : daysPerPixel;

            return daysPerPixel;
        }

        /// <summary>
        /// Parses event preset, string to list
        /// </summary>
        private static List<EventTag> GetSelectedEventTypesEasy(string eventPreset)
        {
            var returnList = new List<EventTag>();

            //if string contains comma "," then we read each seperately
            if (eventPreset.Contains(','))
            {
                var split = eventPreset.Split(',');
                foreach (var item in split)
                {
                    //are converted as is
                    returnList.Add(Enum.Parse<EventTag>(item));
                }
            }
            //else hard coded preset below
            else
            {
                switch (eventPreset)
                {
                    //only general is customized
                    case "Summary":
                        returnList.Add(EventTag.GocharaSummary);
                        returnList.Add(EventTag.General);
                        returnList.Add(EventTag.Personal);
                        returnList.Add(EventTag.RulingConstellation);
                        break;
                    //case "Gochara":
                    //    returnList.Add(EventTag.Personal);
                    //    returnList.Add(EventTag.Gochara);
                    //    break;
                    //case "Travel":
                    //    returnList.Add(EventTag.Personal);
                    //    returnList.Add(EventTag.General);
                    //    returnList.Add(EventTag.Travel);
                    //    break;

                    //others are converted as is
                    default:
                        returnList.Add(Enum.Parse<EventTag>(eventPreset));
                        break;

                }
            }


            return returnList;
        }

        /// <summary>
        /// Given a time preset in string like "Today"
        /// a precisise start and end time will be returned
        /// used in dasa/muhurtha easy calculator
        /// </summary>
        private static object AutoCalculateTimeRange(string timePreset, GeoLocation birthLocation, TimeSpan timezone)
        {

            Time start, end;
            //use the inputed user's timezone
            DateTimeOffset now = DateTimeOffset.Now.ToOffset(timezone);
            var today = now.ToString("dd/MM/yyyy zzz");

            var yesterday = now.AddDays(-1).ToString("dd/MM/yyyy zzz");
            switch (timePreset)
            {

                case "Hour":
                    var startHour = now.AddHours(-1);//back 1 hour
                    var endHour = now.AddHours(1);//front 1 hour
                    start = new Time(startHour, birthLocation);
                    end = new Time(endHour, birthLocation);
                    return new { start, end };
                case "Today":
                case "Day":
                    start = new Time($"00:00 {today}", birthLocation);
                    end = new Time($"23:59 {today}", birthLocation);
                    return new { start, end };
                case "Week":
                    start = new Time($"00:00 {yesterday}", birthLocation);
                    var weekEnd = now.AddDays(7).ToString("dd/MM/yyyy zzz");
                    end = new Time($"23:59 {weekEnd}", birthLocation);
                    return new { start, end };
                case "Month":
                    var _1WeekAgo = now.AddDays(-7).ToString("dd/MM/yyyy zzz");
                    start = new Time($"00:00 {_1WeekAgo}", birthLocation);
                    var monthEnd = now.AddDays(30).ToString("dd/MM/yyyy zzz");
                    end = new Time($"23:59 {monthEnd}", birthLocation);
                    return new { start, end };
                case "3Months":
                    var _2WeekAgo = now.AddDays(-14).ToString("dd/MM/yyyy zzz");
                    start = new Time($"00:00 {_2WeekAgo}", birthLocation);
                    var _3monthEnd = now.AddDays(90).ToString("dd/MM/yyyy zzz");
                    end = new Time($"23:59 {_3monthEnd}", birthLocation);
                    return new { start, end };
                case "6Months":
                    var _1MonthsAgo = now.AddDays(-30).ToString("dd/MM/yyyy zzz");
                    start = new Time($"00:00 {_1MonthsAgo}", birthLocation);
                    var _6monthEnd = now.AddDays(180).ToString("dd/MM/yyyy zzz");
                    end = new Time($"23:59 {_6monthEnd}", birthLocation);
                    return new { start, end };
                case "Year":
                    var _2MonthsAgo = now.AddDays(-60).ToString("dd/MM/yyyy zzz");
                    start = new Time($"00:00 {_2MonthsAgo}", birthLocation);
                    var yearEnd = now.AddDays(365).ToString("dd/MM/yyyy zzz");
                    end = new Time($"23:59 {yearEnd}", birthLocation);
                    return new { start, end };
                case "5Years":
                    var _6MonthsAgo = now.AddDays(-180).ToString("dd/MM/yyyy zzz");
                    start = new Time($"00:00 {_6MonthsAgo}", birthLocation);
                    var _5yearEnd = now.AddDays((365 * 5)).ToString("dd/MM/yyyy zzz");
                    end = new Time($"23:59 {_5yearEnd}", birthLocation);
                    return new { start, end };
                case "10Years":
                case "Decade":
                    var _1YearAgo = now.AddDays(-365).ToString("dd/MM/yyyy zzz");
                    start = new Time($"00:00 {_1YearAgo}", birthLocation);
                    var decadeEnd = now.AddDays((365 * 10)).ToString("dd/MM/yyyy zzz");
                    end = new Time($"23:59 {decadeEnd}", birthLocation);
                    return new { start, end };
                default:
                    return new { start = Time.Empty, end = Time.Empty };
            }
        }

        /// <summary>
        /// Massive method that generates dasa report in SVG
        /// </summary>
        private static async Task<string> GenerateMainEventsChartSvg(Person inputPerson, Time startTime, Time endTime, double daysPerPixel, List<EventTag> inputedEventTags)
        {
            //1 CALCULATE DATA
            // One precision value for generating all dasa components,
            // because misalignment occurs if use different precision
            // note: precision = time slice count, each slice = 1 pixel (zoom level)
            double eventsPrecisionHours = Tools.DaysToHours(daysPerPixel);

            //generate time slice only once for all rows
            var timeSlices = EventManager.GetTimeListFromRange(startTime, endTime, eventsPrecisionHours);


            //2 MAKE EVENT ROWS
            var compiledRow = GenerateRowsSvg(inputPerson, daysPerPixel, startTime, endTime, timeSlices, eventsPrecisionHours, inputedEventTags, out int totalHeight);


            //3 MAKE NOW LINE
            //the height for all lines, cursor, now & life events line
            //place below the last row
            var padding = 2; //space between rows
            var lineHeight = totalHeight + padding;

            //get now line position
            var clientNowTime = startTime.StdTimeNowAtOffset;//now time at client
            var nowLinePosition = GetLinePosition(timeSlices, clientNowTime);
            var nowLineHeight = lineHeight + 6; //space between icon & last row
            compiledRow += GetNowLine(nowLineHeight, nowLinePosition);

            //5 WRAP IN GROUP
            //place all content except border & cursor time legend inside group for padding
            const int contentPadding = 2;
            compiledRow = $"<g class=\"EventChartContent\" transform=\"matrix(1, 0, 0, 1, {contentPadding}, {contentPadding})\">{compiledRow}</g>";


            //6 MAKE CURSOR LINE
            //add in the cursor line (moves with cursor via JS)
            //note: - template cursor line is duplicated to dynamically generate legend box
            //      - placed last so that show on top of others
            var svgTotalHeight = totalHeight + 110;//add space for life event icon
            compiledRow += GetTimeCursorLine(svgTotalHeight);

            //4 MAKE LIFE EVENTS
            //wait!, add in life events also
            //use offset of input time, this makes sure life event lines
            //are placed on event chart correctly, since event chart is based on input offset
            var lifeEventHeight = lineHeight + 6; //space between icon & last row
            var inputOffset = startTime.GetStdDateTimeOffset().Offset; //timezone the chart will be in
            compiledRow += await GetLifeEventLinesSvg(inputPerson, lifeEventHeight, inputOffset, timeSlices);


            //7 ADD BORDER
            //save a copy of the number of time slices used to calculate the svg total width later
            var dasaSvgWidth = timeSlices.Count;
            //add border around svg element
            //note:compensate for padding, makes border fit nicely around content
            var borderWidth = dasaSvgWidth + contentPadding;
            var roundedBorder = 3;
            compiledRow += $"<rect class=\"EventChartBorder\" rx=\"{roundedBorder}\" width=\"{borderWidth}\" height=\"{svgTotalHeight}\" style=\"stroke-width: 1; fill: none; paint-order: stroke; stroke:#333;\"></rect>";


            //8 DONE!
            //put all stuff in final SVG tag
            var finalSvg = WrapSvgElements(compiledRow, dasaSvgWidth, svgTotalHeight); //little wiggle room
            return finalSvg;



            //█░░ █▀█ █▀▀ ▄▀█ █░░   █▀▀ █░█ █▄░█ █▀▀ ▀█▀ █ █▀█ █▄░█ █▀
            //█▄▄ █▄█ █▄▄ █▀█ █▄▄   █▀░ █▄█ █░▀█ █▄▄ ░█░ █ █▄█ █░▀█ ▄█


        }

        /// <summary>
        /// Get color based on nature
        /// </summary>
        private static string GetColor(EventNature? eventNature) => GetColor(eventNature.ToString());

        /// <summary>
        /// Get color based on nature
        /// </summary>
        private static string GetColor(string nature)
        {
            switch (nature)
            {
                case "Good": return "#00FF00"; //green
                case "Neutral": return "#D3D3D3"; //grey
                case "Bad": return "#FF0000"; //red
                default: throw new Exception($"Invalid Nature: {nature}");
            }
        }

        /// <summary>
        /// convert value coming in, to a percentage based on min & max
        /// this will make setting color based on value easier & accurate
        /// note, this will cause color to be relative to only what is visible in chart atm!
        /// </summary>
        public static string GetSummaryColor(double totalNature, double minValue, double maxValue)
        {
            string colorHex;
            if (totalNature >= 0) //good
            {
                //note: higher number is lighter color lower is darker
                var color255 = (int)totalNature.Remap(0, maxValue, 0, 255);
                if (color255 <= 10) { return "#FFFFFF"; } //if very close to 0, return greenish white color (lighter)
                if (color255 >= 220) { return "#00DD00"; } //if very close to 255, return darker green (DD) to highlight

                //this is done to invert the color,
                //so that higher score leads to darker green
                color255 = 255 - color255;

                colorHex = $"{color255:X}";//convert from 255 to FF
                var summaryColor = $"#{colorHex}FF{colorHex}";
                return summaryColor; //green
            }
            else //bad
            {
                //note: colors here are rendered opposite compared to good
                //because lower number here is worse, as such needs to be dark red.
                var color255 = (int)totalNature.Remap(minValue, 0, 0, 255);
                if (color255 <= 10) { return "#DD0000"; } //if very close to 255, return darker red (DD) to highlight
                if (color255 >= 220) { return "#FFFFFF"; } //if very close to 0, return white color
                colorHex = $"{color255:X}";//convert from 255 to FF
                var summaryColor = $"#FF{colorHex}{colorHex}";
                return summaryColor; //red
            }

        }

        /// <summary>
        /// Generates individual life event line svg
        /// Incrementing line height also increments icon position below
        /// </summary>
        private static string GenerateLifeEventLine(LifeEvent lifeEvent, int lineHeight, DateTimeOffset lifeEvtTime, int positionX)
        {
            //shorten the event name if too long & add ellipsis at end
            //else text goes out side box
            var formattedEventName = ShortenName(lifeEvent.Name);

            //based on length of event name make the background
            //mainly done to shorten background of short names (saving space)
            var backgroundWidth = GetTextWidthPx(formattedEventName);

            int iconYAxis = lineHeight; //start icon at end of line
            var iconXAxis = $"-{backgroundWidth / 2}"; //use negative to move center under line
            var nameTextHeight = 15;
            var iconSvg = $@"
                                <rect class=""vertical-line"" fill=""#1E1EEA"" width=""2"" height=""{lineHeight}""></rect>
                                <!-- EVENT ICON LABEL -->
                                <g transform=""translate({iconXAxis},{iconYAxis})"">
                                    <!-- NAME -->
                                    <g class=""name-label"" >
                                        <g transform=""translate(15,0)"">
						                    <rect class=""background"" x=""0"" y=""0"" style=""fill: blue; opacity: 0.8;"" width=""{backgroundWidth}"" height=""{nameTextHeight}"" rx=""2"" ry=""2"" />
						                    <text class=""event-name"" x=""3"" y=""11"" style=""fill:#FFFFFF; font-family:'Calibri'; font-size:12px;"">{formattedEventName}</text>
					                    </g>
				                        <g class=""nature-icon"" transform=""translate(8,8)"">
                                            <rect class=""background"" fill=""{GetColor(lifeEvent.Nature)}"" x=""-8"" y=""-8"" width=""15"" height=""15"" rx=""0"" ry=""0""/>
					                        <path d=""M2-0.2H0.7C0.5-0.2,0.3,0,0.3,0.2v1.3c0,0.2,0.2,0.4,0.4,0.4H2c0.2,0,0.4-0.2,0.4-0.4V0.2C2.5,0,2.3-0.2,2-0.2z M2-4.5v0.4 h-3.6v-0.4c0-0.2-0.2-0.4-0.4-0.4c-0.2,0-0.4,0.2-0.4,0.4v0.4h-0.4c-0.5,0-0.9,0.4-0.9,0.9v6.1c0,0.5,0.4,0.9,0.9,0.9h6.3 c0.5,0,0.9-0.4,0.9-0.9v-6.1c0-0.5-0.4-0.9-0.9-0.9H3.1v-0.4c0-0.2-0.2-0.4-0.4-0.4S2-4.8,2-4.5z M2.9,2.9h-5.4 c-0.2,0-0.4-0.2-0.4-0.4v-4.4h6.3v4.4C3.4,2.7,3.2,2.9,2.9,2.9z""/>
				                        </g>
			                        
</g>
                                    <!-- DESCRIPTION -->
		                            <g class=""description-label"" style=""display:none;"" transform=""translate(0,{20})"">
                                        <rect class=""background"" style=""fill: blue; opacity: 0.8;"" width=""{backgroundWidth}"" height=""50"" rx=""2"" ry=""2""/>
                                        <text x=""4.849"" y=""-1.77"">
                                            {StringToSvgTextBox(lifeEvent.Description)}
			                            </text>
		                            </g>
                                </g>
                                ";

            //put together icon + line + event data
            var lifeEventLine = $@"<g eventName=""{lifeEvent.Name}"" class=""LifeEventLines"" stdTime=""{lifeEvtTime:dd/MM/yyyy}"" 
                              transform=""translate({positionX}, 0)"">{iconSvg}</g>";

            return lifeEventLine;

            //LOCAL FUNC
            string ShortenName(string rawInput)
            {
                //if no changes needed return as is (default)
                var returnVal = rawInput;
                const int nameCharLimit = 12; //any number of char above this limit will be replaced with  ellipsis  "..."

                //if input is above limit
                //replace extra chars with ...
                var isAbove = rawInput.Length > nameCharLimit;
                if (isAbove)
                {
                    //cut the extra chars out
                    returnVal = returnVal.Substring(0, nameCharLimit);

                    //add ellipsis 
                    returnVal += "...";
                }

                return returnVal;
            }

            //gets the exact width of a text based on Font size & type
            //used to generate nicely fitting background for text
            double GetTextWidthPx(string textInput)
            {
                //TODO handle max & min
                //set max & min width background
                //const int maxWidth = 70;
                //backgroundWidth = backgroundWidth > maxWidth ? maxWidth : backgroundWidth;
                //const int minWidth = 30;
                //backgroundWidth = backgroundWidth > minWidth ? minWidth : backgroundWidth;


                SizeF size;
                using (var graphics = Graphics.FromHwnd(IntPtr.Zero))
                {
                    size = graphics.MeasureString(formattedEventName, new Font("Calibri", 12, FontStyle.Regular, GraphicsUnit.Pixel));
                }
                var widthPx = Math.Round(size.Width);

                return widthPx;
            }
        }

        /// <summary>
        /// Input sting to get nicely wrapped text in SVG
        /// Note: this is only an invisible structured text, other styling have to be added separately
        /// </summary>
        /// <returns></returns>
        private static string StringToSvgTextBox(string inputStr)
        {

            const int characterLimit = 21;
            //chop string to pieces, to wrap text nicely (new line)
            var splitStringRaw = Tools.SplitByCharCount(inputStr, characterLimit);

            //convert raw string to be HTML safe (handles special chars like &,!,%)
            var splitString = splitStringRaw.Select(rawStr => HttpUtility.HtmlEncode(rawStr)).ToList();

            //make a new holder for each line
            var compiledSvgLines = "";
            const int lineHeight = 12;
            var nextYAxis = 0; //start with 0, at top
            foreach (var strLine in splitString)
            {
                var newLineSvg = $@"<tspan x=""0"" y=""{nextYAxis}"" fill=""#FFFFFF"" font-family=""Calibri"" font-size=""10"">{strLine}</tspan>";

                //add together with other lines
                compiledSvgLines += newLineSvg;

                //set next line to come underneath this (increment y axis)
                nextYAxis += lineHeight;
            }

            //var finalTextSvg = $@"{compiledSvgLines}";

            return compiledSvgLines;
        }

        /// <summary>
        /// Returns a person's life events in SVG group to be placed inside events chart
        /// gets person's life events as lines for the events chart
        /// </summary>
        private static async Task<string> GetLifeEventLinesSvg(Person person, int lineHeight, TimeSpan inputOffset, List<Time> timeSlices)
        {

            var compiledLines = "";

            //sort by earliest to latest event
            var lifeEventList = person.LifeEventList;
            lifeEventList.Sort((x, y) => x.CompareTo(y));


            var previousPositionX = 0; //to keep track of crowding
            var incrementRate = 25; //for overcrowded jump
            var adjustedLineHeight = lineHeight; //keep copy for resetting after overcrowded jum
                                                 //count for previous events crowded in a row 
            var prevCrowdedCount = 0;
            var previousMovedDown = false;
            foreach (var lifeEvent in lifeEventList)
            {
                //this is placed here so that if fail
                try
                {
                    //get timezone at place event happened
                    var lifeEvtTime = await lifeEvent.GetDateTimeOffset();//time at the place of event with correct standard timezone
                    var startTimeInputOffset = lifeEvtTime.ToOffset(inputOffset); //change to output offset, to match chart
                    var positionX = GetLinePosition(timeSlices, startTimeInputOffset);

                    //if line is not in report time range, don't generate it
                    if (positionX == 0) { continue; }

                    //check if overcrowded, move icon down if so
                    var isCrowded = IsEventIconSpaceCrowded(previousPositionX, positionX);
                    if (isCrowded)
                    {
                        //set icon position lower than previous
                        //if crowded back to back then move down accordingly
                        //todo can move up as well if next item is not going to crowd
                        prevCrowdedCount++;
                        //if previous move down, the this move up
                        if (previousMovedDown)
                        {
                            adjustedLineHeight = lineHeight;
                            previousMovedDown = false; //set as moved up, so next can go down
                                                       //reset previous back to back crowd count
                            prevCrowdedCount = 0;
                        }
                        //move down
                        else
                        {
                            adjustedLineHeight += (incrementRate * prevCrowdedCount);
                            previousMovedDown = true; //set as moved down, so next can go up
                        }
                    }
                    else
                    {
                        //reset previous back to back crowd count
                        prevCrowdedCount = 0;
                        //reset previous label crowded movement
                        previousMovedDown = false;
                    }

                    //put together icon + line + event data
                    compiledLines += GenerateLifeEventLine(lifeEvent, adjustedLineHeight, lifeEvtTime, positionX);

                    //reset line height for next 
                    if (isCrowded) { adjustedLineHeight = lineHeight; }

                    //update previous position
                    previousPositionX = positionX;

                }
                catch (Exception e)
                {
                    //if fail log it and go on with next life event despite failure
                    await ApiLogger.Error(e, null);
                    continue;
                }
            }


            //wrap in a group so that can be hidden/shown as needed
            //add transform matrix to adjust for border shift
            const int contentPadding = 2;
            var wrapperGroup = $"<g id=\"LifeEventLinesHolder\" transform=\"matrix(1, 0, 0, 1, {contentPadding}, {contentPadding})\">{compiledLines}</g>";

            return wrapperGroup;

            //-------------------------

            //check if current event icon position will block previous life event icon
            //expects next event to be chronologically next event
            bool IsEventIconSpaceCrowded(int previousX, int currentX)
            {
                //if previous 0, then obviously not crowded
                if (previousX <= 0) { return false; }

                //space smaller than this is set crowded
                const int minSpaceBetween = 110;//px

                //previous X axis should be lower than current
                //difference shows space between these 2 icons
                var difference = currentX - previousX;
                var isOverLapping = difference < minSpaceBetween;
                return isOverLapping;
            }
        }

        /// <summary>
        /// add in the cursor line (moves with cursor via JS)
        /// note: template cursor line is duplicated to dynamically generate legend box
        /// </summary>
        private static string GetTimeCursorLine(int lineHeight)
        {
            return $@"
                        <g id=""CursorLine"" x=""0"" y=""0"">
                            <!--place where asset icons are stored-->
                            <g id=""CursorLineLegendIcons"" style=""display:none;"">
                                <circle id=""CursorLineBadIcon"" cx=""6.82"" cy=""7.573"" r=""4.907"" fill=""red""></circle>
                                <circle id=""CursorLineGoodIcon"" cx=""6.82"" cy=""7.573"" r=""4.907"" fill=""green""></circle>
                                <g id=""CursorLineClockIcon"" fill=""#fff"" transform=""matrix(0.5, 0, 0, 0.5, 2, 3)"" width=""12"" height=""12"">
				                    <path d=""M10 0a10 10 0 1 0 10 10A10 10 0 0 0 10 0zm2.5 14.5L9 11V4h2v6l3 3z""/>
			                    </g>
                                <path id=""CursorLineSumIcon"" transform=""matrix(0.045, 0, 0, 0.045, -14, -4)"" fill=""#fff"" d=""M437 122c-15 2-26 5-38 10-38 16-67 51-75 91-4 17-4 36 0 54 10 48 47 86 95 98 11 2 15 3 30 3s19-1 30-3c48-12 86-50 96-98 3-18 3-37 0-54-10-47-48-86-95-98-10-2-16-3-29-3h-14zm66 59c2 2 3 3 4 6s1 17 0 20c-2 7-11 9-15 2-1-2-1-3-1-7v-5h-37-37s8 11 18 23l21 25c1 2 1 5 1 7-1 1-10 13-21 26l-19 24c0 1 13 1 37 1h37v-5c0-6 1-9 5-11 5-2 11 1 11 8 1 1 1 6 1 10-1 7-1 8-2 10s-3 4-7 4h-52-50l-2-1c-4-3-5-7-3-11 0 0 11-14 24-29l22-28-22-28c-13-16-24-29-24-30-2-3-1-7 2-9 2-3 2-3 55-3h51l3 1z"" stroke=""none"" fill-rule=""nonzero""/>
                            </g>

                            <g id=""IBeam"">
			                    <rect width=""20"" height=""2"" style=""fill:black;"" x=""-9"" y=""0""></rect>
			                    <rect width=""2"" height=""{lineHeight}"" style=""fill:#000000;"" x=""0"" y=""0""></rect>
			                    <rect width=""20"" height=""2"" style=""fill:black;"" x=""-9"" y=""{lineHeight - 2}""></rect>
		                    </g>
		                    <g id=""CursorLineLegendTemplate"" transform=""matrix(1, 0, 0, 1, 10, 26)"" style=""display:none;"">
                                <rect style=""fill: blue; opacity: 0.80;"" x=""-1"" y=""0"" width=""160"" height=""15"" rx=""2"" ry=""2""></rect>
			                    <text style=""fill:#FFFFFF; font-size:11px; font-weight:400; white-space: pre;"" x=""14"" y=""11"">Template</text>
                                <!--icon set dynamic by JS-->
                                <use xlink:href=""""></use>                                
                            </g>
                            <!--place where dynamic event names are placed by JS-->
                            <g id=""CursorLineLegendHolder"" transform=""matrix(1, 0, 0, 1, 0, 4)""></g>
                            <g id=""CursorLineLegendDescriptionHolder"" transform=""matrix(1, 0, 0, 1, 0, 0)"" style=""display:none;"">
			                    <rect id=""CursorLineLegendDescriptionBackground"" style=""fill:#003e99;"" x=""170"" y=""11.244"" width=""150"" height=""0"" rx=""2"" ry=""2""></rect>
                                <g id=""CursorLineLegendDescription""></g>
		                    </g>
                       </g>
                        ";
        }

        private static string GetNowLine(int lineHeight, int nowLinePosition)
        {
            return $@"
                       <g id=""NowVerticalLine"" x=""0"" y=""0"" transform=""matrix(1, 0, 0, 1, {nowLinePosition}, 0)"">
		                    <rect width=""2"" height=""{lineHeight}"" style="" fill:blue; stroke-width:0.5px; stroke:#000;""></rect>
		                    <g transform=""matrix(2, 0, 0, 2, -12, {lineHeight})"">
			                    <rect style=""fill:blue; stroke:black; stroke-width: 0.5px;"" x=""0"" y=""0"" width=""12"" height=""9.95"" rx=""2.5"" ry=""2.5""></rect>
			                    <text style=""fill: rgb(255, 255, 255); font-size: 4.1px; white-space: pre;"" x=""1.135"" y=""6.367"">NOW</text>
		                    </g>
	                    </g>
                        ";
        }

        /// <summary>
        /// gets line position given a date
        /// finds most closest time slice, else return 0 means none found
        /// note:
        /// - make sure offset in time list and input time matches
        /// - tries to get nearest day first, then tries month to nearest year
        /// </summary>
        private static int GetLinePosition(List<Time> timeSliceList, DateTimeOffset inputTime)
        {
            //input timezone must be converted output timezone (timeSliceList)
            var outputTimezone = timeSliceList[0].GetStdDateTimeOffset().Offset;
            //translate event timezone to match chart timezone
            inputTime = inputTime.ToOffset(outputTimezone);

#if DEBUG
            if (outputTimezone.Ticks != inputTime.Offset.Ticks) { Console.WriteLine($"Life Event Offset Translated:{outputTimezone}->{inputTime.Offset}"); }
#endif

            var nowHour = inputTime.Hour;
            var nowDay = inputTime.Day;
            var nowYear = inputTime.Year;
            var nowMonth = inputTime.Month;

            //if nearest hour is possible then end here
            var nearestHour = GetNearestHour();
            if (nearestHour != 0) { return nearestHour; }

            //else try get nearest day
            var nearestDay = GetNearestDay();
            if (nearestDay != 0) { return nearestDay; }

            //else try get nearest month
            var nearestMonth = GetNearestMonth();
            if (nearestMonth != 0) { return nearestMonth; }

            //else try get nearest year
            var nearestYear = GetNearestYear();
            if (nearestYear != 0) { return nearestYear; }


            //if control reaches here then now time not found in time slices
            //this is possible when viewing old charts as such set now line to 0
            return 0;

            int GetNearestMonth()
            {
                //go through the list and find where the slice is closest to now
                var slicePosition = 0;
                foreach (var time in timeSliceList)
                {

                    //if same year and same month then send this slice position
                    //as the correct one
                    var sameYear = time.GetStdYear() == nowYear;
                    var sameMonth = time.GetStdMonth() == nowMonth;
                    if (sameMonth && sameYear)
                    {
                        return slicePosition;
                    }

                    //move to next slice position
                    slicePosition++;
                }

                return 0;
            }

            int GetNearestDay()
            {
                //go through the list and find where the slice is closest to now
                var slicePosition = 0;
                foreach (var time in timeSliceList)
                {

                    //if same year and same month then send this slice position
                    //as the correct one
                    var sameDay = time.GetStdDate() == nowDay;
                    var sameYear = time.GetStdYear() == nowYear;
                    var sameMonth = time.GetStdMonth() == nowMonth;
                    if (sameMonth && sameYear && sameDay)
                    {
                        return slicePosition;
                    }

                    //move to next slice position
                    slicePosition++;
                }

                return 0;
            }

            int GetNearestHour()
            {
                //go through the list and find where the slice is closest to now
                var slicePosition = 0;
                foreach (var time in timeSliceList)
                {

                    //if same year and same month then send this slice position
                    //as the correct one
                    var sameHour = time.GetStdDateTimeOffset().Hour == nowHour;
                    var sameDay = time.GetStdDate() == nowDay;
                    var sameYear = time.GetStdYear() == nowYear;
                    var sameMonth = time.GetStdMonth() == nowMonth;
                    if (sameDay && sameHour && sameMonth && sameYear)
                    {
                        return slicePosition;
                    }

                    //move to next slice position
                    slicePosition++;
                }

                return 0;
            }

            int GetNearestYear()
            {

                //go through the list and find where the slice is closest to now
                var slicePosition = 0;
                foreach (var time in timeSliceList)
                {

                    //if same year and same month then send this slice position
                    //as the correct one
                    var sameYear = time.GetStdYear() == nowYear;
                    if (sameYear)
                    {
                        return slicePosition;
                    }

                    //move to next slice position
                    slicePosition++;
                }

                return 0;
            }
        }

        //wraps a list of svg elements inside 1 main svg element
        //if width not set defaults to 1000px, and height to 1000px
        private static string WrapSvgElements(string combinedSvgString, int svgWidth, int svgTotalHeight)
        {

            var svgBackgroundColor = "#f0f9ff";

            //create the final svg that will be displayed
            var svgTotalWidth = svgWidth + 10; //add little for wiggle room
            var svgBody = $"<svg id=\"EventChartHolder\"" +
                          //$" width=\"100%\"" +
                          //$" height=\"100%\"" +
                          $" style=\"" +
                          //note: if width & height not hard set, parent div clips it
                          $"width:{svgTotalWidth}px;" +
                          $"height:{svgTotalHeight}px;" +
                          $"background:{svgBackgroundColor};" +
                          $"\" " +//end of style tag
                          $"xmlns=\"http://www.w3.org/2000/svg\" " +
                          $"xmlns:xlink=\"http://www.w3.org/1999/xlink\">" + //much needed for use tags to work
                                                                             //$"<title>{chartTitle}</title>" + //title visible in browser when open direct
                          $"{combinedSvgString}</svg>";

            return svgBody;
        }

        /// <summary>
        /// Generates the event & header part of the dasa report
        /// </summary>
        private static string GenerateRowsSvg(Person inputPerson, double daysPerPixel, Time startTime,
            Time endTime, List<Time> timeSlices, double precisionInHours, List<EventTag> inputedEventTags, out int totalHeight)
        {

            //px width & height of each slice of time
            //used when generating dasa rows
            //note: changes needed only here
            const int widthPerSlice = 1;

            //STEP 1 : GENERATE TIME HEADER ROWS
            var compiledRow = GenerateTimeHeaderRow(timeSlices, daysPerPixel, widthPerSlice, out int headerY);


            //STEP 2 : GENERATE EVENT ROWS
            compiledRow += GenerateEventRows(
                precisionInHours,
                startTime,
                endTime,
                inputPerson,
                headerY,
                timeSlices,
                daysPerPixel,
                inputedEventTags,
                out totalHeight);

            return compiledRow;

        }

        private static string GenerateTimeHeaderRow(List<Time> timeSlices, double daysPerPixel, int _widthPerSlice, out int headerY)
        {
            var dasaSvgWidth = 0; //will be filled when calling row generator
            var compiledRow = "";

            var beginYear = timeSlices[0].GetStdYear();
            var endYear = timeSlices.Last().GetStdYear();
            var difYears = endYear - beginYear;

            //header rows are dynamically generated as needed, hence the extra logic below
            var headerGenerator = new List<Func<List<Time>, int, int, int, string>>();
            var showYearRow = daysPerPixel <= 15;
            if (difYears >= 10 && !showYearRow) { headerGenerator.Add(GenerateDecadeRowSvg); }
            if (difYears is >= 5 and < 10) { headerGenerator.Add(Generate5YearRowSvg); }
            if (showYearRow) { headerGenerator.Add(GenerateYearRowSvg); }
            if (daysPerPixel <= 1.3) { headerGenerator.Add(GenerateMonthRowSvg); }
            if (daysPerPixel <= 0.07) { headerGenerator.Add(GenerateDateRowSvg); }
            if (daysPerPixel <= 0.001) { headerGenerator.Add(GenerateHourRowSvg); }

            var padding = 2;//space between rows
            headerY = 0;
            int headerHeight = 11;
            foreach (var generator in headerGenerator)
            {
                compiledRow += generator(timeSlices, headerY, 0, headerHeight);

                //update for next generator
                headerY = headerY + headerHeight + padding;
            }

            return compiledRow;


            string GenerateYearRowSvg(List<Time> timeSlices, int yAxis, int xAxis, int rowHeight)
            {

                //generate the row for each time slice
                var rowHtml = "";
                var previousYear = 0; //start 0 for first draw
                var yearBoxWidthCount = 0;
                int rectWidth = 0;
                int childAxisX = 0;

                foreach (var slice in timeSlices)
                {

                    //only generate new year box when year changes or at
                    //end of time slices to draw the last year box
                    var lastTimeSlice = timeSlices.IndexOf(slice) == timeSlices.Count - 1;
                    var yearChanged = previousYear != slice.GetStdYear();
                    if (yearChanged || lastTimeSlice)
                    {
                        //and it is in the beginning
                        if (previousYear == 0)
                        {
                            yearBoxWidthCount = 0; //reset width
                        }
                        else
                        {
                            //generate previous year data first before resetting
                            childAxisX += rectWidth; //use previous rect width to position this
                            rectWidth = yearBoxWidthCount * _widthPerSlice; //calculate new rect width
                            var textX = rectWidth / 2; //center of box divide 2
                            var rect = $"<g transform=\"matrix(1, 0, 0, 1, {childAxisX}, 0)\">" + //y is 0 because already set in parent group
                                                $"<rect " +
                                                    $"fill=\"#0d6efd\" x=\"0\" y=\"0\" width=\"{rectWidth}\" height=\"{rowHeight}\" " + $" style=\"paint-order: stroke; stroke: rgb(255, 255, 255); stroke-opacity: 1; stroke-linejoin: round;\"/>" +
                                                    $"<text x=\"{textX}\" y=\"{9}\" width=\"{rectWidth}\" fill=\"white\"" +
                                                        $" style=\"fill: rgb(255, 255, 255);" +
                                                        $" font-size: 10px;" +
                                                        $" font-weight: 700;" +
                                                        $" text-anchor: middle;" +
                                                        $" white-space: pre;\"" +
                                                        //$" transform=\"matrix(0.966483, 0, 0, 0.879956, 2, -6.779947)\"" +
                                                        $">" +
                                                        $"{previousYear}" + //previous year generate at begin of new year
                                                    $"</text>" +
                                             $"</g>";


                            //add to final return
                            rowHtml += rect;

                            //reset width
                            yearBoxWidthCount = 0;

                        }
                    }
                    //year same as before
                    else
                    {
                        //update width only, position is same
                        //as when created the year box
                        //yearBoxWidthCount *= _widthPerSlice;

                    }

                    //update previous year for next slice
                    previousYear = slice.GetStdYear();

                    yearBoxWidthCount++;


                }

                //wrap all the rects inside a svg so they can me moved together
                //svg tag here acts as group, svg nesting
                rowHtml = $"<g transform=\"matrix(1, 0, 0, 1, {xAxis}, {yAxis})\">{rowHtml}</g>";

                return rowHtml;
            }

            string GenerateDecadeRowSvg(List<Time> timeSlices, int yAxis, int xAxis, int rowHeight)
            {

                //generate the row for each time slice
                var rowHtml = "";
                var previousYear = 0; //start 0 for first draw
                var yearBoxWidthCount = 0;
                int rectWidth = 0;
                int childAxisX = 0;
                const int decade = 10;

                var beginYear = timeSlices[0].GetStdYear();
                var endYear = beginYear + decade; //decade


                foreach (var slice in timeSlices)
                {

                    //only generate new year box when year changes or at
                    //end of time slices to draw the last year box
                    var lastTimeSlice = timeSlices.IndexOf(slice) == timeSlices.Count - 1;
                    var yearChanged = previousYear != slice.GetStdYear();
                    if (yearChanged || lastTimeSlice)
                    {
                        //is this slice end year & last month (month for accuracy, otherwise border at jan not december)
                        //todo begging of box is not beginning of year, possible solution month
                        //var isLastMonth = slice.GetStdMonth() is 10 or 11 or 12; //use oct & nov in-case december is not generated at low precision 
                        var isEndYear = endYear == slice.GetStdYear();
                        if (isEndYear)
                        {
                            //generate previous year data first before resetting
                            childAxisX += rectWidth; //use previous rect width to position this
                            rectWidth = yearBoxWidthCount * _widthPerSlice; //calculate new rect width
                            var textX = rectWidth / 2; //center of box divide 2
                            var rect = $"<g transform=\"matrix(1, 0, 0, 1, {childAxisX}, 0)\">" + //y is 0 because already set in parent group
                                       $"<rect " +
                                       $"fill=\"#0d6efd\" x=\"0\" y=\"0\" width=\"{rectWidth}\" height=\"{rowHeight}\" " + $" style=\"paint-order: stroke; stroke: rgb(255, 255, 255); stroke-opacity: 1; stroke-linejoin: round;\"/>" +
                                       $"<text x=\"{textX}\" y=\"{9}\" width=\"{rectWidth}\" fill=\"white\"" +
                                       $" style=\"fill: rgb(255, 255, 255);" +
                                       $" font-size: 10px;" +
                                       $" font-weight: 700;" +
                                       $" text-anchor: middle;" +
                                       $" white-space: pre;\"" +
                                       //$" transform=\"matrix(0.966483, 0, 0, 0.879956, 2, -6.779947)\"" +
                                       $">" +
                                       $"{beginYear} - {endYear}" + //previous year generate at begin of new year
                                       $"</text>" +
                                       $"</g>";


                            //add to final return
                            rowHtml += rect;

                            //reset width
                            yearBoxWidthCount = 0;

                            //set new begin & end
                            beginYear = endYear + 1;
                            endYear = beginYear + decade;

                        }

                    }

                    //update previous year for next slice
                    previousYear = slice.GetStdYear();

                    yearBoxWidthCount++;

                }

                //wrap all the rects inside a svg so they can me moved together
                //svg tag here acts as group, svg nesting
                rowHtml = $"<g transform=\"matrix(1, 0, 0, 1, {xAxis}, {yAxis})\">{rowHtml}</g>";

                return rowHtml;
            }

            string Generate5YearRowSvg(List<Time> timeSlices, int yAxis, int xAxis, int rowHeight)
            {

                //generate the row for each time slice
                var rowHtml = "";
                var previousYear = 0; //start 0 for first draw
                var yearBoxWidthCount = 0;
                int rectWidth = 0;
                int childAxisX = 0;
                //int rowHeight = 11;

                const int yearRange = 5;

                var beginYear = timeSlices[0].GetStdYear();
                var endYear = beginYear + yearRange;


                foreach (var slice in timeSlices)
                {

                    //only generate new year box when year changes or at
                    //end of time slices to draw the last year box
                    var lastTimeSlice = timeSlices.IndexOf(slice) == timeSlices.Count - 1;
                    var yearChanged = previousYear != slice.GetStdYear();
                    if (yearChanged || lastTimeSlice)
                    {
                        //is this slice end year
                        var isEndYear = endYear == slice.GetStdYear();
                        if (isEndYear)
                        {
                            //generate previous year data first before resetting
                            childAxisX += rectWidth; //use previous rect width to position this
                            rectWidth = yearBoxWidthCount * _widthPerSlice; //calculate new rect width
                            var textX = rectWidth / 2; //center of box divide 2
                            var rect = $"<g transform=\"matrix(1, 0, 0, 1, {childAxisX}, 0)\">" + //y is 0 because already set in parent group
                                       $"<rect " +
                                       $"fill=\"#0d6efd\" x=\"0\" y=\"0\" width=\"{rectWidth}\" height=\"{rowHeight}\" " + $" style=\"paint-order: stroke; stroke: rgb(255, 255, 255); stroke-opacity: 1; stroke-linejoin: round;\"/>" +
                                       $"<text x=\"{textX}\" y=\"{9}\" width=\"{rectWidth}\" fill=\"white\"" +
                                       $" style=\"fill: rgb(255, 255, 255);" +
                                       $" font-size: 10px;" +
                                       $" font-weight: 700;" +
                                       $" text-anchor: middle;" +
                                       $" white-space: pre;\"" +
                                       //$" transform=\"matrix(0.966483, 0, 0, 0.879956, 2, -6.779947)\"" +
                                       $">" +
                                       $"{beginYear} - {endYear}" + //previous year generate at begin of new year
                                       $"</text>" +
                                       $"</g>";


                            //add to final return
                            rowHtml += rect;

                            //reset width
                            yearBoxWidthCount = 0;

                            //set new begin & end
                            beginYear = endYear + 1;
                            endYear = beginYear + yearRange;

                        }

                    }

                    //update previous year for next slice
                    previousYear = slice.GetStdYear();

                    yearBoxWidthCount++;

                }

                //wrap all the rects inside a svg so they can me moved together
                //svg tag here acts as group, svg nesting
                rowHtml = $"<g transform=\"matrix(1, 0, 0, 1, {xAxis}, {yAxis})\">{rowHtml}</g>";

                return rowHtml;
            }

            string GenerateMonthRowSvg(List<Time> timeSlices, int yAxis, int xAxis, int rowHeight)
            {

                //generate the row for each time slice
                var rowHtml = "";
                var previousMonth = 0; //start 0 for first draw
                var yearBoxWidthCount = 0;
                int rectWidth = 0;
                int childAxisX = 0;
                //int rowHeight = 11;

                foreach (var slice in timeSlices)
                {

                    //only generate new year box when year changes or at
                    //end of time slices to draw the last year box
                    var lastTimeSlice = timeSlices.IndexOf(slice) == timeSlices.Count - 1;
                    var monthChanged = previousMonth != slice.GetStdMonth();
                    if (monthChanged || lastTimeSlice)
                    {
                        //and it is in the beginning
                        if (previousMonth == 0)
                        {
                            yearBoxWidthCount = 0; //reset width
                        }
                        else
                        {
                            //generate previous month data first before resetting
                            childAxisX += rectWidth; //use previous rect width to position this
                            rectWidth = yearBoxWidthCount * _widthPerSlice; //calculate new rect width
                            var textX = rectWidth / 2; //center of box divide 2
                            var rect = $"<g transform=\"matrix(1, 0, 0, 1, {childAxisX}, 0)\">" + //y is 0 because already set in parent group
                                       $"<rect " +
                                       $"fill=\"#0d6efd\" x=\"0\" y=\"0\" width=\"{rectWidth}\" height=\"{rowHeight}\" " + $" style=\"paint-order: stroke; stroke: rgb(255, 255, 255); stroke-opacity: 1; stroke-linejoin: round;\"/>" +
                                       $"<text x=\"{textX}\" y=\"{9}\" width=\"{rectWidth}\" fill=\"white\"" +
                                       $" style=\"fill: rgb(255, 255, 255);" +
                                       $" font-size: 10px;" +
                                       $" font-weight: 700;" +
                                       $" text-anchor: middle;" +
                                       $" white-space: pre;\"" +
                                       //$" transform=\"matrix(0.966483, 0, 0, 0.879956, 2, -6.779947)\"" +
                                       $">" +
                                       $"{GetMonthName(previousMonth)}" + //previous year generate at begin of new year
                                       $"</text>" +
                                       $"</g>";


                            //add to final return
                            rowHtml += rect;

                            //reset width
                            yearBoxWidthCount = 0;

                        }
                    }
                    //year same as before
                    else
                    {
                        //update width only, position is same
                        //as when created the year box
                        //yearBoxWidthCount *= _widthPerSlice;

                    }

                    //update previous month for next slice
                    previousMonth = slice.GetStdMonth();

                    yearBoxWidthCount++;


                }

                //wrap all the rects inside a svg so they can me moved together
                //svg tag here acts as group, svg nesting
                rowHtml = $"<g transform=\"matrix(1, 0, 0, 1, {xAxis}, {yAxis})\">{rowHtml}</g>";

                return rowHtml;

                string GetMonthName(int monthNum)
                {
                    switch (monthNum)
                    {
                        case 1: return "JAN";
                        case 2: return "FEB";
                        case 3: return "MAR";
                        case 4: return "APR";
                        case 5: return "MAY";
                        case 6: return "JUN";
                        case 7: return "JUL";
                        case 8: return "AUG";
                        case 9: return "SEP";
                        case 10: return "OCT";
                        case 11: return "NOV";
                        case 12: return "DEC";
                        default: throw new Exception($"Invalid Month: {monthNum}");
                    }
                }
            }

            string GenerateDateRowSvg(List<Time> timeSlices, int yAxis, int xAxis, int rowHeight)
            {

                //generate the row for each time slice
                var rowHtml = "";
                var previousDate = 0; //start 0 for first draw
                var dateBoxWidthCount = 0;
                int rectWidth = 0;
                int childAxisX = 0;
                //int rowHeight = 11;

                foreach (var slice in timeSlices)
                {

                    //only generate new date box when date changes or at
                    //end of time slices to draw the last date box
                    var lastTimeSlice = timeSlices.IndexOf(slice) == timeSlices.Count - 1;
                    var dateChanged = previousDate != slice.GetStdDate();
                    if (dateChanged || lastTimeSlice)
                    {
                        //and it is in the beginning
                        if (previousDate == 0)
                        {
                            dateBoxWidthCount = 0; //reset width
                        }
                        else
                        {
                            //generate previous date data first before resetting
                            childAxisX += rectWidth; //use previous rect width to position this
                            rectWidth = dateBoxWidthCount * _widthPerSlice; //calculate new rect width
                            var textX = rectWidth / 2; //center of box divide 2
                            var rect = $"<g transform=\"matrix(1, 0, 0, 1, {childAxisX}, 0)\">" + //y is 0 because already set in parent group
                                       $"<rect " +
                                       $"fill=\"#0d6efd\" x=\"0\" y=\"0\" width=\"{rectWidth}\" height=\"{rowHeight}\" " + $" style=\"paint-order: stroke; stroke: rgb(255, 255, 255); stroke-opacity: 1; stroke-linejoin: round;\"/>" +
                                       $"<text x=\"{textX}\" y=\"{9}\" width=\"{rectWidth}\" fill=\"white\"" +
                                       $" style=\"fill: rgb(255, 255, 255);" +
                                       $" font-size: 10px;" +
                                       $" font-weight: 700;" +
                                       $" text-anchor: middle;" +
                                       $" white-space: pre;\"" +
                                       //$" transform=\"matrix(0.966483, 0, 0, 0.879956, 2, -6.779947)\"" +
                                       $">" +
                                       $"{previousDate}" + //previous date generate at begin of new date
                                       $"</text>" +
                                       $"</g>";


                            //add to final return
                            rowHtml += rect;

                            //reset width
                            dateBoxWidthCount = 0;

                        }
                    }

                    //update previous date for next slice
                    previousDate = slice.GetStdDate();

                    dateBoxWidthCount++;

                }

                //wrap all the rects inside a svg so they can me moved together
                //svg tag here acts as group, svg nesting
                rowHtml = $"<g transform=\"matrix(1, 0, 0, 1, {xAxis}, {yAxis})\">{rowHtml}</g>";

                return rowHtml;

            }

            string GenerateHourRowSvg(List<Time> timeSlices, int yAxis, int xAxis, int rowHeight)
            {

                //generate the row for each time slice
                var rowHtml = "";
                var previousHour = -1; //so that hour 0 is counted
                var hourBoxWidthCount = 0;
                int rectWidth = 0;
                int childAxisX = 0;
                //int rowHeight = 11;

                foreach (var slice in timeSlices)
                {

                    //only generate new date box when hour changes or at
                    //end of time slices to draw the last hour box
                    var isLastTimeSlice = timeSlices.IndexOf(slice) == timeSlices.Count - 1;
                    var hourChanged = previousHour != slice.GetStdHour();
                    if (hourChanged || isLastTimeSlice)
                    {
                        //and it is in the beginning
                        if (previousHour == -1)
                        {
                            hourBoxWidthCount = 0; //reset width
                        }
                        else
                        {
                            //generate previous hour data first before resetting
                            childAxisX += rectWidth; //use previous rect width to position this
                            rectWidth = hourBoxWidthCount * _widthPerSlice; //calculate new rect width
                            var textX = rectWidth / 2; //center of box divide 2
                            var rect = $"<g transform=\"matrix(1, 0, 0, 1, {childAxisX}, 0)\">" + //y is 0 because already set in parent group
                                       $"<rect " +
                                       $"fill=\"#0d6efd\" x=\"0\" y=\"0\" width=\"{rectWidth}\" height=\"{rowHeight}\" " + $" style=\"paint-order: stroke; stroke: rgb(255, 255, 255); stroke-opacity: 1; stroke-linejoin: round;\"/>" +
                                       $"<text x=\"{textX}\" y=\"{9}\" width=\"{rectWidth}\" fill=\"white\"" +
                                       $" style=\"fill: rgb(255, 255, 255);" +
                                       $" font-size: 10px;" +
                                       $" font-weight: 700;" +
                                       $" text-anchor: middle;" +
                                       $" white-space: pre;\"" +
                                       //$" transform=\"matrix(0.966483, 0, 0, 0.879956, 2, -6.779947)\"" +
                                       $">" +
                                       $"{previousHour}" + //previous hour generate at begin of new hour
                                       $"</text>" +
                                       $"</g>";


                            //add to final return
                            rowHtml += rect;

                            //reset width
                            hourBoxWidthCount = 0;

                        }
                    }

                    //update previous hour for next slice
                    previousHour = slice.GetStdHour();

                    hourBoxWidthCount++;
                }

                //wrap all the rects inside a svg so they can me moved together
                //svg tag here acts as group, svg nesting
                rowHtml = $"<g transform=\"matrix(1, 0, 0, 1, {xAxis}, {yAxis})\">{rowHtml}</g>";

                return rowHtml;

            }

        }

        /// <summary>
        /// Generate rows based of inputed events
        /// </summary>
        private static string GenerateEventRows(double eventsPrecision, Time startTime, Time endTime,
            Person inputPerson, int headerY, List<Time> timeSlices, double daysPerPixel, List<EventTag> inputedEventTags, out int totalHeight)
        {
            //1 GENERATE DATA FOR EVENT ROWS
            const int widthPerSlice = 1;
            const int singleRowHeight = 15;
            var eventDataList = APITools.GetEventDataList().Result;

            //rows are dynamically generated as needed, hence the extra logic below
            //list of rows to generate
            var unsortedEventList = new List<Event>();

            //calculate events for each tag
            foreach (var eventTag in inputedEventTags)
            {
                var tempEventList = APITools.CalculateEvents(eventsPrecision, startTime, endTime, inputPerson.GetBirthLocation(), inputPerson, eventTag, eventDataList);
                unsortedEventList.AddRange(tempEventList);
            }

            //sort event by duration, so that events are ordered nicely in chart
            //todo events are breaking up between rows
            var eventList = unsortedEventList.OrderByDescending(x => x.Duration).ToList();


            //2 STACK & GENERATED ROWS FROM ABOVE DATA
            var padding = 1;//space between rows
            var compiledRow = "";

            //note: summary data is filled when generating rows
            var summaryRowData = new Dictionary<int, SumData>();//x axis, total nature score, planet name
            int yAxis = headerY;
            //generate svg for each row & add to final row
            compiledRow += GenerateMultipleRowSvg(eventList, timeSlices, yAxis, 0, out int finalHeight);
            //set y axis (horizontal) for next row
            yAxis = yAxis + finalHeight + padding;

            //4 GENERATE SUMMARY ROW
            //min & max used to calculate color later
            var maxValue = summaryRowData.Values.Max(x => x.NatureScore);
            var minValue = summaryRowData.Values.Min(x => x.NatureScore);
            compiledRow += GenerateSummaryRow(yAxis);
            yAxis += 15;//add in height of summary row

            //3 LET CALLER KNOW FINAL HEIGHT
            totalHeight = yAxis;

            return compiledRow;



            //█░░ █▀█ █▀▀ ▄▀█ █░░   █▀▀ █░█ █▄░█ █▀▀ ▀█▀ █ █▀█ █▄░█ █▀
            //█▄▄ █▄█ █▄▄ █▀█ █▄▄   █▀░ █▄█ █░▀█ █▄▄ ░█░ █ █▄█ █░▀█ ▄█

            string GenerateSummaryRow(int yAxis)
            {
#if DEBUG
                Console.WriteLine("GenerateSummaryRow");
                Console.WriteLine("MAX" + maxValue);
                Console.WriteLine("MIN" + minValue);
#endif

                var rowHtml = "";
                //STEP 1 : generate color summary
                var colorRow = "";
                foreach (var summarySlice in summaryRowData)
                {
                    int xAxis = summarySlice.Key;
                    //total nature score is sum of negative & positive 1s of all events
                    //that occurred at this point in time, possible negative number
                    //exp: -4 bad + 5 good = 1 total nature score
                    double totalNatureScore = summarySlice.Value.NatureScore;

                    var rect = $"<rect " +
                               $"x=\"{xAxis}\" " +
                               $"y=\"{yAxis}\" " + //y axis placed here instead of parent group, so that auto legend can use the y axis
                               $"width=\"{widthPerSlice}\" " +
                               $"height=\"{singleRowHeight}\" " +
                               $"fill=\"{GetSummaryColor(totalNatureScore, minValue, maxValue)}\" />";

                    //add rect to row
                    colorRow += rect;
                }

                rowHtml += $"<g id=\"ColorRow\">{colorRow}</g>";


                //STEP 2 : generate graph summary
                var barChartRow = "";
                foreach (var summarySlice in summaryRowData)
                {
                    int xAxis = summarySlice.Key;
                    double totalNatureScore = summarySlice.Value.NatureScore; //possible negative
                    var barHeight = (int)totalNatureScore.Remap(minValue, maxValue, 0, 30);
                    var rect = $"<rect " +
                               $"x=\"{xAxis}\" " +
                               $"y=\"{yAxis}\" " + //y axis placed here instead of parent group, so that auto legend can use the y axis
                               $"width=\"{widthPerSlice}\" " +
                               $"height=\"{barHeight}\" " +
                               $"fill=\"black\" />";

                    //add rect to row
                    barChartRow += rect;
                }

                //note: chart is flipped 180, to start bar from bottom to top
                //default hidden
                rowHtml += $"<g id=\"BarChartRow\" transform=\"matrix(1, 0, 0, 1, 0, 20)\">{barChartRow}</g>";


                //STEP 3 : generate color summary SMART
                var colorRowSmart = "";
                foreach (var summarySlice in summaryRowData)
                {
                    int xAxis = summarySlice.Key;
                    //total nature score is sum of negative & positive 1s of all events
                    //that occurred at this point in time, possible negative number
                    //exp: -4 bad + 5 good = 1 total nature score
                    double totalNatureScore = summarySlice.Value.NatureScore;
                    var planetPowerFactor = GetPlanetPowerFactor(summarySlice.Value.Planet, summarySlice.Value.BirthTime);
                    var smartNatureScore = totalNatureScore * planetPowerFactor;
                    var rect = $"<rect " +
                               $"x=\"{xAxis}\" " +
                               $"y=\"{yAxis}\" " + //y axis placed here instead of parent group, so that auto legend can use the y axis
                               $"width=\"{widthPerSlice}\" " +
                               $"height=\"{singleRowHeight}\" " +
                               $"fill=\"{GetSummaryColor(smartNatureScore, -100, 100)}\" />";

                    //add rect to row
                    colorRowSmart += rect;
                }
                //note: chart is flipped 180, to start bar from bottom to top
                //default hidden
                rowHtml += $"<g id=\"BarChartRowSmart\" transform=\"matrix(1, 0, 0, 1, 0, 43)\">{colorRowSmart}</g>";


                //STEP 4 : final wrapper
                //add in "Summary" label above row
                float aboveRow = yAxis - singleRowHeight - padding;
                rowHtml += $@"
                    <g id=""SummaryLabel"" transform=""matrix(1, 0, 0, 1, 0, {aboveRow})"">
				        <rect style=""fill: blue; opacity: 0.80;"" x=""-1"" y=""0"" width=""68"" height=""15"" rx=""2"" ry=""2""/>
				        <text style=""fill:#FFFFFF; font-size:11px; font-weight:400;"" x=""16"" y=""11"">Summary</text>
				        <path transform=""matrix(0.045, 0, 0, 0.045, -14, -4)"" fill=""#fff"" d=""M437 122c-15 2-26 5-38 10-38 16-67 51-75 91-4 17-4 36 0 54 10 48 47 86 95 98 11 2 15 3 30 3s19-1 30-3c48-12 86-50 96-98 3-18 3-37 0-54-10-47-48-86-95-98-10-2-16-3-29-3h-14zm66 59c2 2 3 3 4 6s1 17 0 20c-2 7-11 9-15 2-1-2-1-3-1-7v-5h-37-37s8 11 18 23l21 25c1 2 1 5 1 7-1 1-10 13-21 26l-19 24c0 1 13 1 37 1h37v-5c0-6 1-9 5-11 5-2 11 1 11 8 1 1 1 6 1 10-1 7-1 8-2 10s-3 4-7 4h-52-50l-2-1c-4-3-5-7-3-11 0 0 11-14 24-29l22-28-22-28c-13-16-24-29-24-30-2-3-1-7 2-9 2-3 2-3 55-3h51l3 1z"" stroke=""none"" fill-rule=""nonzero""/>
			        </g>";

                //return compiled rects as 1 row in a group for easy debugging & edits
                rowHtml = $"<g id=\"SummaryRow\">{rowHtml}</g>";
                return rowHtml;
            }



            //height not known until generated
            //returns the final dynamic height of this event row
            string GenerateMultipleRowSvg(List<Event> eventList, List<Time> timeSlices, int yAxis, int xAxis, out int finalHeight)
            {
                //generate the row for each time slice
                var rowHtml = "";
                var horizontalPosition = 0; //distance from left
                var verticalPosition = 0; //distance from top

                //height of each row
                var spaceBetweenRow = 1;

                //used to determine final height
                var highestTimeSlice = 0;
                var multipleEventCount = 0;

                //start as empty event
                var prevEventList = new Dictionary<int, EventName>();

                //generate 1px (rect) per time slice (horizontal)
                foreach (var slice in timeSlices)
                {
                    //get events that occurred at this time slice
                    var foundEventList = eventList.FindAll(tempEvent => tempEvent.IsOccurredAtTime(slice));

                    //generate rect for each event & stack from top to bottom (vertical)
                    foreach (var foundEvent in foundEventList)
                    {
                        //if current event is different than event has changed, so draw a black line
                        int finalYAxis = yAxis + verticalPosition;
                        var prevExist = prevEventList.TryGetValue(finalYAxis, out var prevEventName);
                        var isNewEvent = prevExist && (prevEventName != foundEvent.Name);
                        var color = isNewEvent ? "black" : GetColor(foundEvent?.Nature);

                        //save current event to previous, to draw border later
                        //border ONLY for top 3 rows (long duration events), lower row borders block color
                        if (finalYAxis <= 29)
                        {
                            prevEventList[finalYAxis] = foundEvent.Name;
                        }

                        //var color = GetColor(foundEvent?.Nature);

                        //generate and add to row
                        //the hard coded attribute names used here are used in App.js
                        var rect = $"<rect " +
                                   $"eventname=\"{foundEvent?.FormattedName}\" " +
                                   $"age=\"{inputPerson.GetAge(slice)}\" " +
                                   $"stdtime=\"{slice.GetStdDateTimeOffset().ToString(Time.DateTimeFormat)}\" " +
                                   $"x=\"{horizontalPosition}\" " +
                                   $"y=\"{finalYAxis}\" " + //y axis placed here instead of parent group, so that auto legend can use the y axis
                                   $"width=\"{widthPerSlice}\" " +
                                   $"height=\"{singleRowHeight}\" " +
                                   $"fill=\"{color}\" />";

                        //add rect to return list
                        rowHtml += rect;

                        //every time a rect is added, we keep track of it in a list to generate the summary row at last
                        //based on event nature minus or plus 1
                        double natureScore = 0;
                        switch (foundEvent?.Nature)
                        {
                            case EventNature.Good:
                                natureScore = 1;
                                break;
                            case EventNature.Bad:
                                natureScore = -1;
                                break;
                        }

                        //compile nature score for making summary row later (defaults to 0)
                        var previousNatureScoreSum = (summaryRowData.ContainsKey(horizontalPosition) ? summaryRowData[horizontalPosition].NatureScore : 0);

                        var x = new SumData
                        {
                            BirthTime = inputPerson.BirthTime,
                            Planet = Tools.GetPlanetFromName(foundEvent.FormattedName),
                            NatureScore = natureScore + previousNatureScoreSum //combine current with previous
                        };
                        summaryRowData[horizontalPosition] = x;


                        //increment vertical position for next
                        //element to be placed beneath this one
                        verticalPosition += singleRowHeight + spaceBetweenRow;

                        multipleEventCount++; //include this in count
                    }

                    //set position for next element in time slice
                    horizontalPosition += widthPerSlice;

                    //reset vertical position for next time slice
                    verticalPosition = 0;

                    //safe only the highest row (last row in to be added) used for calculating final height
                    var multipleRowH = (multipleEventCount * (singleRowHeight + spaceBetweenRow)) - spaceBetweenRow; //minus 1 to compensate for last row
                    var thisSliceHeight = multipleEventCount > 1 ? multipleRowH : singleRowHeight; //different height calc for multiple & single row
                    highestTimeSlice = thisSliceHeight > highestTimeSlice ? thisSliceHeight : highestTimeSlice;
                    multipleEventCount = 0; //reset

                }

                //wrap all the rects inside a svg so they can be moved together
                //note: use group instead of svg because editing capabilities
                rowHtml = $"<g class=\"EventListHolder\" transform=\"matrix(1, 0, 0, 1, {xAxis}, 0)\">{rowHtml}</g>";

                //send height of tallest time slice aka the
                //final height of this gochara row to caller
                finalHeight = highestTimeSlice;

                return rowHtml;
            }

        }

        /// <summary>
        /// Given a list planet names only power factor of strongest planet is returned
        /// convert the planets strength into a value over hundred with max & min set by strongest & weakest planet
        /// </summary>
        public static double GetPlanetPowerFactor(PlanetName valuePlanet, Time time)
        {
            //get all planet strength for given time (horoscope)
            var list = AstronomicalCalculator.GetAllPlanetStrength(time);
            //get the power of the planet inputed
            var planetPwr = list.FirstOrDefault(x => x.Item2 == valuePlanet).Item1;

            //get min & max
            var min = list.Min(x => x.Item1); //weakest planet
            var max = list.Max(x => x.Item1); //strongest planet

            //convert the planets strength into a value over hundred with max & min set by strongest & weakest planet
            //returns as percentage over 100%
            var factor = planetPwr.Remap(min, max, 0, 100);

            //planet power below 70% filtered out
            factor = factor < 70 ? 0 : factor;

            return factor;
        }

        /// <summary>
        /// Given planet list, will return factor for strongest only
        /// </summary>
        public static double GetPlanetPowerFactor(List<PlanetName> inputPlanetList, Time time)
        {
            //if no input planets return 0 power,
            //this makes events with no planets not appear in smart sum row
            //muhurtha events also are stopped here
            if (!inputPlanetList.Any()) { return 0; }

            //get all power factors into a list
            var listx = new List<double>();
            foreach (var planet in inputPlanetList)
            {
                var x = GetPlanetPowerFactor(planet, time);
                listx.Add(x);
            }

            //todo can experiment with average as well,
            //strongest was choose because, it is assumed that strong bhukti can override dasa
            //take the strongest
            var strongest = listx.Max();

            return strongest;

        }

        private static byte[] StreamToByteArray(Stream input)
        {
            //reset stream position
            input.Position = 0;
            MemoryStream ms = new MemoryStream();
            input.CopyTo(ms);
            return ms.ToArray();
        }

        private static Stream GenerateStreamFromString(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }


}
