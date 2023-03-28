using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Xml.Linq;
using VedAstro.Library;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

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
        private static Dictionary<double, string> _cacheList;



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
        [Function("geteventschart")]
        public static async Task<HttpResponseData> GetEventsChart([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestData incomingRequest)
        {
            try
            {
                //get dasa report for sending
                var chart = await GetEventReportSvgForIncomingRequest(incomingRequest, false);

                return SendSvgToCaller(chart.ContentSvg, incomingRequest);

                //marked for deletion
                ////convert svg string to stream for sending
                //var stream = GenerateStreamFromString(chart.ContentSvg);
                //var x = StreamToByteArray(stream);
                //return new FileContentResult(x, "image/svg+xml");

            }
            catch (Exception e)
            {
                //log error
                await APILogger.Error(e, incomingRequest);

                //format error nicely to show user
                return APITools.FailMessage(e, incomingRequest);
            }

        }


        [Function("findbirthtimedasa")]
        public static async Task<HttpResponseData> FindBirthTimeDasa([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestData incomingRequest)
        {
            try
            {
                //get data needed to generate multiple charts
                //accurate life event
                var lifeEvent = new LifeEvent();
                lifeEvent.StartTime = "00:00 01/01/2014";
                lifeEvent.Location = "Malaysia";
                //view little ahead & forward of event
                var eventTime = await lifeEvent.GetDateTimeOffsetAsync();
                var startTime = new Time(eventTime.AddMonths(-1), await lifeEvent.GetGeoLocation());
                var endTime = new Time(eventTime.AddMonths(1), await lifeEvent.GetGeoLocation());

                //person
                var person = await APITools.GetPersonById("54041d1ffb494a79997f7987ecfcf08b5");
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
                var finalSvg = EventsChartManager.WrapSvgElements(combinedSvg, 800, chartYPosition, Tools.GenerateId());


                //send image back to caller
                return SendSvgToCaller(finalSvg, incomingRequest);
            }
            catch (Exception e)
            {
                //log error
                await APILogger.Error(e, incomingRequest);

                //format error nicely to show user
                return APITools.FailMessage(e, incomingRequest);
            }
        }

        [Function("findbirthtimerisingsign")]
        public static async Task<HttpResponseData> FindBirthTimeRisingSign([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestData incomingRequest)
        {
            try
            {
                //get person
                var person = await APITools.GetPersonById("05abf671c8bf4e4badc07a086c067f26");

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
                var responseMessage = Tools.AnyTypeToXmlList(predictionList).ToString();

                return APITools.PassMessage(responseMessage, incomingRequest);
            }
            catch (Exception e)
            {
                //log error
                await APILogger.Error(e, incomingRequest);
                //format error nicely to show user
                return APITools.FailMessage(e, incomingRequest);
            }



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
        /// This API is called by EventsChart.js when viewing chart view lite viewer
        /// </summary>
        [Function("geteventscharteasy")]
        public static async Task<HttpResponseData> GetEventsChartEasy([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestData incomingRequest)
        {
            try
            {
                //TODO caching
                //TODO get saved chart if any
                //var rootXml = APITools.ExtractDataFromRequest(req);

                //get dasa report for sending
                var chart = await GetEventReportSvgForIncomingRequestEasy(incomingRequest);

                //send image back to caller
                return SendSvgToCaller(chart.ContentSvg, incomingRequest);
            }
            catch (Exception e)
            {
                //log error
                await APILogger.Error(e, incomingRequest);

                //format error nicely to show user
                return APITools.FailMessage(e, incomingRequest);
            }
        }


        /// <summary>
        /// Gets chart from saved list, needs to be given id to find
        /// </summary>
        [Function("getsavedeventschart")]
        public static async Task<HttpResponseData> GetSavedEventsChart([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestData incomingRequest)
        {

            try
            {
                //get id that will be used find the person
                var requestData = await APITools.ExtractDataFromRequest(incomingRequest);
                var chartId = requestData.Value;

                //get the saved chart record by id
                var savedChartListXml = await APITools.GetXmlFileFromAzureStorage(APITools.SavedChartListFile, APITools.BlobContainerName);
                var foundChartXml = await APITools.FindChartById(savedChartListXml, chartId);
                var chart = Chart.FromXml(foundChartXml);

                //send image back to caller
                return SendSvgToCaller(chart.ContentSvg, incomingRequest);

            }
            catch (Exception e)
            {
                //log error
                await APILogger.Error(e, incomingRequest);

                //format error nicely to show user
                return APITools.FailMessage(e, incomingRequest);
            }

        }


        /// <summary>
        /// Special function to access Saved Chart directly without website
        /// </summary>
        [Function("savedchart")]
        public static async Task<HttpResponseData> GetSavedEventsChartDirect([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "savedchart/{chartId}")]
            HttpRequestData incomingRequest, string chartId)
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
                var htmlTemplate = await APITools.GetStringFileHttp(APITools.Url.EventsChartViewerHtml);

                //insert person name into page, to show ready page faster
                //TODO NEEDS TO BE UPDATED
                var personName = (await APITools.GetPersonById(chart.PersonId)).Name;
                var jsVariables = $@"window.PersonName = ""{personName}"";";
                jsVariables += $@"window.ChartType = ""{"Muhurtha"}"";";
                jsVariables += $@"window.PersonId = ""{chart.PersonId}"";";
                htmlTemplate = htmlTemplate.Replace("/*INSERT-JS-VAR-HERE*/", jsVariables);

                //insert SVG into html place holder page
                htmlTemplate = htmlTemplate.Replace("<!--INSERT SVG-->", svgString);

                return APITools.SendHtmlToCaller(htmlTemplate, incomingRequest);

            }
            catch (Exception e)
            {
                //log error
                await APILogger.Error(e, incomingRequest);

                //format error nicely to show user
                return APITools.FailMessage(e, incomingRequest);
            }

        }


        /// <summary>
        /// Returns EventsChartViewer.html with injected variables
        /// Special function to generate new Events Chart directly without website
        /// Does not do anything to generate the chart, that is done by
        /// geteventscharteasy API called by the JS in HTML
        /// </summary>
        [Function("chart")]
        public static async Task<HttpResponseData> GetEventsChartDirect([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "chart/{personId}/{eventPreset}/{timePreset}")]
            HttpRequestData incomingRequest, string personId, string eventPreset, string timePreset)
        {
            try
            {
                //get chart index.html and send that to caller
                var eventsChartViewerHtml = await APITools.GetStringFileHttp(APITools.Url.EventsChartViewerHtml);

                //insert person name into page, to show ready page faster
                var personName = (await APITools.GetPersonById(personId)).Name;
                var jsVariables = $@"window.PersonName = ""{personName}"";";
                jsVariables += $@"window.ChartType = ""{"Muhurtha"}"";";
                jsVariables += $@"window.PersonId = ""{personId}"";";
                var finalHtml = eventsChartViewerHtml.Replace("/*INSERT-JS-VAR-HERE*/", jsVariables);

                return APITools.SendHtmlToCaller(finalHtml, incomingRequest);

            }
            catch (Exception e)
            {
                //log error
                await APILogger.Error(e, incomingRequest);

                //format error nicely to show user
                return APITools.FailMessage(e, incomingRequest);
            }

        }


        /// <summary>
        /// Given a chart id it will return the person id for that chart
        /// </summary>
        [Function("getpersonidfromsavedchartid")]
        public static async Task<HttpResponseData> GetPersonIdFromSavedChartId([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestData incomingRequest)
        {

            try
            {
                //get id that will be used find the person
                var requestData = await APITools.ExtractDataFromRequest(incomingRequest);
                var chartId = requestData.Value;

                //get the saved chart record by id
                var savedChartListXml = await APITools.GetXmlFileFromAzureStorage(APITools.SavedChartListFile, APITools.BlobContainerName);
                var foundChartXml = await APITools.FindChartById(savedChartListXml, chartId);
                var chart = Chart.FromXml(foundChartXml);

                //extract out the person id from chart and send it to caller
                var personIdXml = new XElement("PersonId", chart.PersonId);

                return APITools.PassMessage(personIdXml, incomingRequest);

            }
            catch (Exception e)
            {
                //log error
                await APILogger.Error(e, incomingRequest);

                //format error nicely to show user
                return APITools.FailMessage(Tools.ExceptionToXml(e), incomingRequest);
            }

        }


        /// <summary>
        /// Saves the chart in Azure Storage
        /// </summary>
        [Function("SaveEventsChart")]
        public static async Task<HttpResponseData> SaveEventsChart([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestData incomingRequest)
        {
            try
            {
                //generate report
                var chart = await GetEventReportSvgForIncomingRequest(incomingRequest, false);

                //save chart into storage
                //note: do not wait to speed things up, beware! failure will go undetected on client
                APITools.AddXElementToXDocumentAzure(chart.ToXml(), APITools.SavedChartListFile, APITools.BlobContainerName);

                //let caller know all good
                return APITools.PassMessage(incomingRequest);

            }
            catch (Exception e)
            {
                //log error
                await APILogger.Error(e, incomingRequest);

                //format error nicely to show user
                return APITools.FailMessage(e, incomingRequest);
            }


        }


        /// <summary>
        /// make a name & id only xml list of saved charts
        /// note: done so to not download what is not needed
        ///
        /// client will use data from here to get the actual saved chart
        /// </summary>
        [Function("getsavedchartnamelist")]
        public static async Task<HttpResponseData> GetSavedChartNameList([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestData incomingRequest)
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
                    var person = await APITools.GetPersonById(parsedChart.PersonId);
                    var chartNameXml = new XElement("ChartName",
                        new XElement("Name",
                            parsedChart.GetFormattedName(person.Name)),
                        new XElement("ChartId",
                            parsedChart.ChartId));

                    //add to final xml
                    rootXml.Add(chartNameXml);
                }


                return APITools.PassMessage(rootXml, incomingRequest);
            }
            catch (Exception e)
            {
                //log error
                await APILogger.Error(e, incomingRequest);

                //format error nicely to show user
                return APITools.FailMessage(e, incomingRequest);
            }


        }


        [Function("deletesavedchart")]
        public static async Task<HttpResponseData> DeleteSavedChart([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestData incomingRequest)
        {

            try
            {
                //get unedited hash & updated person details from incoming request
                var requestData = await APITools.ExtractDataFromRequest(incomingRequest);
                var chartId = requestData.Value;


                //get the person record that needs to be deleted
                var savedChartListXml = await APITools.GetXmlFileFromAzureStorage(APITools.SavedChartListFile, APITools.BlobContainerName);
                var chartToDelete = await APITools.FindChartById(savedChartListXml, chartId);

                //delete the chart record,
                chartToDelete.Remove();

                //upload modified list to storage
                var savedChartListClient = await APITools.GetBlobClientAzure(APITools.SavedChartListFile, APITools.BlobContainerName);
                await APITools.OverwriteBlobData(savedChartListClient, savedChartListXml);

                return APITools.PassMessage(incomingRequest);
            }
            catch (Exception e)
            {
                //log error
                await APILogger.Error(e, incomingRequest);

                //format error nicely to show user
                return APITools.FailMessage(e, incomingRequest);
            }
        }





        //█▀█ █▀█ █ █░█ ▄▀█ ▀█▀ █▀▀   █▀▄▀█ █▀▀ ▀█▀ █░█ █▀█ █▀▄ █▀
        //█▀▀ █▀▄ █ ▀▄▀ █▀█ ░█░ ██▄   █░▀░█ ██▄ ░█░ █▀█ █▄█ █▄▀ ▄█

        private static HttpResponseData SendSvgToCaller(string chartContentSvg, HttpRequestData incomingRequest)
        {
            //send image back to caller
            var response = incomingRequest.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "image/svg+xml");
            //place in response body
            response.WriteString(chartContentSvg);
            return response;
        }


        /// <summary>
        /// processes incoming xml request and outputs events svg report
        /// </summary>
        private static async Task<Chart> GetEventReportSvgForIncomingRequest(HttpRequestData req, bool cacheEnable)
        {
            //get all the data needed out of the incoming request
            var rootXml = await APITools.ExtractDataFromRequest(req);
            var personId = rootXml.Element("PersonId")?.Value;
            var eventTagListXml = rootXml.Element("EventTagList");
            var eventTags = EventTagExtensions.FromXmlList(eventTagListXml);
            var startTimeXml = rootXml.Element("StartTime")?.Elements().First();
            var startTime = Time.FromXml(startTimeXml);
            var endTimeXml = rootXml.Element("EndTime")?.Elements().First();
            var endTime = Time.FromXml(endTimeXml);
            var daysPerPixel = double.Parse(rootXml.Element("DaysPerPixel")?.Value ?? "0");

            //get the person instance by id
            var foundPerson = await APITools.GetPersonById(personId);

            Chart chart;

            if (cacheEnable)
            {
                //todo needs testing
                //based on mode set by caller use cache
                chart = await GetChartCached(foundPerson, startTime, endTime, daysPerPixel, eventTags);
            }
            else
            {
                //based on mode set by caller use cache
                chart = await GetChart(foundPerson, startTime, endTime, daysPerPixel, eventTags);
            }

            return chart;
        }

        private static async Task<Chart> GetEventReportSvgForIncomingRequestEasy(HttpRequestData req)
        {
            //get all the data needed out of the incoming request
            var rootXml = await APITools.ExtractDataFromRequest(req);
            var personId = rootXml.Element("PersonId")?.Value;
            var timePreset = rootXml.Element("TimePreset")?.Value;
            var eventPreset = rootXml.Element("EventPreset")?.Value;
            var maxWidth = int.Parse(rootXml.Element("MaxWidth")?.Value); //px
            //client's timezone, not based on birth
            var timezone = Tools.StringToTimezone(rootXml.Element("Timezone")?.Value);//should be "+08:00"

            //hard set max width to 1000px so that no forever calculation created
            maxWidth = maxWidth > 1000 ? 1000 : maxWidth;

            //minus 7% from width for side padding
            //so that chart not squeezed to side of page
            maxWidth = maxWidth - (int)(maxWidth * 0.07); ;

            //EXTRACT & PREPARE DATA

            //get the person instance by id
            var foundPerson = await APITools.GetPersonById(personId);

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

        private static async Task<Chart> GetChartCached(Person foundPerson, Time startTime, Time endTime,
            double daysPerPixel, List<EventTag> eventTags)
        {

            //convert events into 1 signature
            var eventTagsHash = 0;
            foreach (var eventTag in eventTags) { eventTagsHash += eventTag.GetHashCode(); }

            //convert input data into a signature
            var dataSignature = foundPerson.Hash + startTime.GetHashCode() + endTime.GetHashCode() + daysPerPixel + eventTagsHash;

            //use cache if exist else use new one
            var result = _cacheList.TryGetValue(dataSignature, out var cachedChartXml);

            if (string.IsNullOrEmpty(cachedChartXml))
            {
                //a new chart is born
                var newChart = await GetChart(foundPerson, startTime, endTime, daysPerPixel, eventTags);

                //add new chart into cache for future use
                _cacheList[dataSignature] = newChart.ToXml().ToString();

                return newChart;
            }

            //if available use cached chart
            var oldChart = Chart.FromXml(XElement.Parse(cachedChartXml ?? "<Empty/>"));
            return oldChart;

        }

        private static async Task<Chart> GetChart(Person foundPerson, Time startTime, Time endTime, double daysPerPixel, List<EventTag> eventTags)
        {
            //from person get svg report
            var eventsChartSvgString = await EventsChartManager.GenerateEventsChart(foundPerson, startTime, endTime, daysPerPixel, eventTags);

            //a new chart is born
            var newChartId = Tools.GenerateId();
            var eventTagListXml = EventTagExtensions.ToXmlList(eventTags);
            var startTimeXml = startTime.ToXml();
            var endTimeXml = endTime.ToXml();
            var newChart = new Chart(newChartId, eventsChartSvgString, foundPerson.Id, eventTagListXml, startTimeXml, endTimeXml, daysPerPixel);

            return newChart;
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

            //split into pieces if any
            var presetList = eventPreset.Split(',');
            foreach (var preset in presetList)
            {
                switch (preset)
                {
                    //only general is customized
                    case "Summary":
                        returnList.Add(EventTag.GocharaSummary);
                        returnList.Add(EventTag.General);
                        returnList.Add(EventTag.Personal);
                        returnList.Add(EventTag.RulingConstellation);
                        break;

                    //others are converted as is
                    default:
                        returnList.Add(Enum.Parse<EventTag>(preset));
                        break;

                }
            }

            return returnList;
        }

        /// <summary>
        /// Given a time preset in string like "Today"
        /// a precise start and end time will be returned
        /// used in dasa/muhurtha easy calculator
        /// </summary>
        private static object AutoCalculateTimeRange(string timePresetRaw, GeoLocation birthLocation, TimeSpan timezone)
        {

            Time start, end;
            //use the inputed user's timezone
            DateTimeOffset now = DateTimeOffset.Now.ToOffset(timezone);
            var today = now.ToString("dd/MM/yyyy zzz");

            var yesterday = now.AddDays(-1).ToString("dd/MM/yyyy zzz");
            var timePreset = timePresetRaw.ToLower(); //so that all cases are accepted

            //assume input is "3days", number + date type
            //so split by number
            var split = Tools.SplitAlpha(timePreset);
            var result = int.TryParse(split[0], out int number);
            number = number < 1 ? 1 : number; //min 1, so user can in put just, "year" and except 1 year
            //if no number, than data type in 1st place
            var dateType = result ? split[1] : split[0];

            int days;
            double hoursToAdd;
            string _2MonthsAgo;
            var timeNow = Time.Now(birthLocation);
            switch (dateType)
            {
                case "hour":
                case "hours":
                    var startHour = now.AddHours(-1); //back 1 hour
                    var endHour = now.AddHours(number); //front by input
                    start = new Time(startHour, birthLocation);
                    end = new Time(endHour, birthLocation);
                    return new { start, end };
                case "today":
                case "day":
                case "days":
                    start = new Time($"00:00 {today}", birthLocation);
                    end = timeNow.AddHours(Tools.DaysToHours(number));
                    return new { start, end };
                case "week":
                case "weeks":
                    days = number * 7;
                    hoursToAdd = Tools.DaysToHours(days);
                    start = new Time($"00:00 {yesterday}", birthLocation);
                    end = timeNow.AddHours(hoursToAdd);
                    return new { start, end };
                case "month":
                case "months":
                    days = number * 30;
                    hoursToAdd = Tools.DaysToHours(days);
                    var _1WeekAgo = now.AddDays(-7).ToString("dd/MM/yyyy zzz");
                    start = new Time($"00:00 {_1WeekAgo}", birthLocation);
                    end = timeNow.AddHours(hoursToAdd);
                    return new { start, end };
                case "year":
                case "years":
                    days = number * 365;
                    hoursToAdd = Tools.DaysToHours(days);
                    _2MonthsAgo = now.AddDays(-60).ToString("dd/MM/yyyy zzz");
                    start = new Time($"00:00 {_2MonthsAgo}", birthLocation);
                    end = timeNow.AddHours(hoursToAdd);
                    return new { start, end };
                case "decades":
                case "decade":
                    days = number * 3652;
                    hoursToAdd = Tools.DaysToHours(days);
                    _2MonthsAgo = now.AddDays(-60).ToString("dd/MM/yyyy zzz");
                    start = new Time($"00:00 {_2MonthsAgo}", birthLocation);
                    end = timeNow.AddHours(hoursToAdd);
                    return new { start, end };
                default:
                    return new { start = Time.Empty, end = Time.Empty };

            }
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
