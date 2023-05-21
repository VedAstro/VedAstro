using System.Xml.Linq;
using VedAstro.Library;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.DurableTask.Client;
using Microsoft.DurableTask;
using System.Text.Json;
using Newtonsoft.Json.Linq;
using Time = VedAstro.Library.Time;

namespace API
{

    public struct SumData
    {
        /// <summary>
        /// the final score to use when generating colors,
        /// voting should be done already
        /// </summary>
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

                return APITools.SendSvgToCaller(chart.ContentSvg, incomingRequest);

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



        //[Function(nameof(GetPersonListAsync))]
        //public static async Task<HttpResponseData> GetPersonListAsync(
        //    [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "GetPersonListAsync/UserId/{userId}/VisitorId/{visitorId}")] HttpRequestData req,
        //    [DurableClient] DurableTaskClient client,
        //    string userId, string visitorId)
        //{


        [Function(nameof(GetEventsChartAsync))]
        public static async Task<HttpResponseData> GetEventsChartAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]
            HttpRequestData incomingRequest,
            [DurableClient] DurableTaskClient client
            )
        {
            try
            {
                //get data out
                var requestJson = await APITools.ExtractDataFromRequestJson(incomingRequest);

                var requestChart = await Chart.GetDataParsed(requestJson);

                var chartId = requestChart.GetEventsChartSignature();

                //start processing
                var options = new StartOrchestrationOptions(chartId); //set caller id so can callback
                var jsonText = requestJson.ToString();

                var instanceId = await client.ScheduleNewOrchestrationInstanceAsync(nameof(_GetEventsChartAsync), jsonText, options, CancellationToken.None); //should match caller ID


                //get dasa report for sending
                //var chart = await GetEventReportSvgForIncomingRequest(incomingRequest, false);

                //give user the url to query for status and data
                //note : todo this is hack to get polling URL via RESPONSE creator, should be able to create directly
                //var x = client.CreateCheckStatusResponse(incomingRequest, sign.ToString());
                //var pollingUrl = APITools.GetHeaderValue(x, "Location");

                //send polling URL to caller as Passed payload, client should know what todo
                return APITools.PassMessageJson(instanceId, incomingRequest);

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
        /// Call here after calling prepare chart
        /// </summary>
        [Function(nameof(GetEventsChartResult))]
        public static async Task<HttpResponseData> GetEventsChartResult(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "GetEventsChartResult/{chartId}")]
            HttpRequestData incomingRequest,
            [DurableClient] DurableTaskClient client,
            string chartId
            )
        {
            try
            {
                //try to get already calculated chart
                var result = await client.GetInstanceAsync(chartId, false, CancellationToken.None);
                if (result?.RuntimeStatus == OrchestrationRuntimeStatus.Completed)
                {
                    //note : todo hack to get polling URL via RESPONSE header, should be a better way
                    var x = client.CreateCheckStatusResponse(incomingRequest, chartId);
                    var pollingUrl = APITools.GetHeaderValue(x, "Location");

                    //call polling URL
                    //call status endpoint and get oversized output data
                    using var httpClient = new HttpClient();
                    var taskStatusResult = await httpClient.GetStringAsync(pollingUrl);
                    var parsedResult = JObject.Parse(taskStatusResult);
                    var largeSvgString = parsedResult["output"].Value<string>();

                    //send to caller as SVG image
                    return APITools.SendSvgToCaller(largeSvgString, incomingRequest);
                }


                return APITools.FailMessageJson("No Record Call Found", incomingRequest);

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
        /// The underlying async func that actually gets the list
        /// </summary>
        [Function(nameof(_GetEventsChartAsync))]
        public static async Task<string> _GetEventsChartAsync([OrchestrationTrigger]
            TaskOrchestrationContext context)
        {
            var requestData = context.GetInput<string>();

            var swapStatus = await context.CallActivityAsync<string>(nameof(GetChartAsync), requestData); //note swap done before get list

            return swapStatus;
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
                return APITools.SendSvgToCaller(chart.ContentSvg, incomingRequest);
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
                var personName = (await APITools.FindPersonXMLById(personId)).Name;
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
        /// Saves the chart in Azure Storage
        /// </summary>
        [Function(nameof(SaveEventsChart))]
        public static async Task<HttpResponseData> SaveEventsChart([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestData incomingRequest)
        {
            try
            {
                //generate report
                var chart = await GetEventReportSvgForIncomingRequest(incomingRequest, false);

                //save chart into storage
                //note: do not wait to speed things up, beware! failure will go undetected on client
                APITools.AddXElementToXDocumentAzure(chart.ToXml(), APITools.SavedEventsChartListFile, APITools.BlobContainerName);

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
        /// Special function to access Saved Chart directly without website
        /// </summary>
        [Function("savedchart")]
        public static async Task<HttpResponseData> GetSavedEventsChartDirect([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "savedchart/{chartId}")]
            HttpRequestData incomingRequest, string chartId)
        {
            try
            {

                //get the saved chart record by id
                var foundChartXml = await APITools.FindSavedEventsChartXMLById(chartId);
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
        /// Gets chart from saved list, needs to be given id to find
        /// </summary>
        [Function("getsavedeventschart")]
        public static async Task<HttpResponseData> GetSavedEventsChart([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestData incomingRequest)
        {

            try
            {
                //get id that will be used find the person
                var requestData = await APITools.ExtractDataFromRequestXml(incomingRequest);
                var chartId = requestData.Value;

                //get the saved chart record by id
                var foundChartXml = await APITools.FindSavedEventsChartXMLById(chartId);
                var chart = Chart.FromXml(foundChartXml);

                //send image back to caller
                return APITools.SendSvgToCaller(chart.ContentSvg, incomingRequest);

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
                var requestData = await APITools.ExtractDataFromRequestXml(incomingRequest);
                var chartId = requestData.Value;

                //get the saved chart record by id
                var foundChartXml = await APITools.FindSavedEventsChartXMLById(chartId);
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
                var savedChartXmlDoc = await APITools.GetXmlFileFromAzureStorage(APITools.SavedEventsChartListFile, APITools.BlobContainerName);

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
                var requestData = await APITools.ExtractDataFromRequestXml(incomingRequest);
                var chartId = requestData.Value;


                //get the person record that needs to be deleted
                var chartToDelete = await APITools.FindSavedEventsChartXMLById(chartId);

                //delete the chart record,
                chartToDelete.Remove();

                //upload modified list to storage
                //todo methodify or already exist
                var savedChartListClient = await APITools.GetBlobClientAzure(APITools.SavedEventsChartListFile, APITools.BlobContainerName);
                var savedChartListXml = await APITools.GetXmlFileFromAzureStorage(APITools.SavedEventsChartListFile, APITools.BlobContainerName);
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
                    var newChart = await GenerateNewChart(personAdjusted, startTime, endTime, daysPerPixel, dasaEventTags);
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
                var finalSvg = EventsChartManager.WrapSvgElements("MultipleDasa", combinedSvg, 800, chartYPosition, Tools.GenerateId());


                //send image back to caller
                return APITools.SendSvgToCaller(finalSvg, incomingRequest);
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

                    var x = await Tools.GetHoroscopePrediction(personAdjusted, APITools.HoroscopeDataListFile);
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




        //█▀█ █▀█ █ █░█ ▄▀█ ▀█▀ █▀▀   █▀▄▀█ █▀▀ ▀█▀ █░█ █▀█ █▀▄ █▀
        //█▀▀ █▀▄ █ ▀▄▀ █▀█ ░█░ ██▄   █░▀░█ ██▄ ░█░ █▀█ █▄█ █▄▀ ▄█


        /// <summary>
        /// processes incoming xml request and outputs events svg report
        /// </summary>
        private static async Task<Chart> GetEventReportSvgForIncomingRequest(HttpRequestData req, bool cacheEnable)
        {
            //get all the data needed out of the incoming request
            var rootXml = await APITools.ExtractDataFromRequestXml(req);
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

            Chart chart = Chart.Empty;

            if (cacheEnable)
            {
                //todo needs testing
                //based on mode set by caller use cache
                //chart = await GetChartCached(foundPerson, startTime, endTime, daysPerPixel, eventTags);
            }
            else
            {
                //based on mode set by caller use cache
                chart = await GenerateNewChart(foundPerson, startTime, endTime, daysPerPixel, eventTags);
            }

            return chart;
        }

        private static async Task<Chart> GetEventReportSvgForIncomingRequestEasy(HttpRequestData req)
        {
            //get all the data needed out of the incoming request
            var rootXml = await APITools.ExtractDataFromRequestXml(req);
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


            return await GenerateNewChart(foundPerson, startTime, endTime, daysPerPixel, eventTags);
        }


        ///// <summary>
        ///// Cache stored in running memory
        ///// </summary>
        //private static async Task<Chart> GetChartCached(Person foundPerson, Time startTime, Time endTime,
        //    double daysPerPixel, List<EventTag> eventTags)
        //{
        //    //create a unique signature to identify all future calls that is exactly alike
        //    var dataSignature = GetEventsChartSignature(foundPerson, startTime,endTime,daysPerPixel, eventTags);

        //    //use cache if exist else use new one
        //    var result = _cacheList.TryGetValue(dataSignature, out var cachedChartXml);

        //    if (string.IsNullOrEmpty(cachedChartXml))
        //    {
        //        //a new chart is born
        //        var newChart = await GenerateNewChart(foundPerson, startTime, endTime, daysPerPixel, eventTags);

        //        //add new chart into cache for future use
        //        _cacheList[dataSignature] = newChart.ToXml().ToString();

        //        return newChart;
        //    }

        //    //if available use cached chart
        //    var oldChart = Chart.FromXml(XElement.Parse(cachedChartXml ?? "<Empty/>"));
        //    return oldChart;

        //}


        [Function(nameof(GetChartAsync))]
        public static async Task<string> GetChartAsync([ActivityTrigger] string input)
        {

            var parsed = JObject.Parse(input);

            var requestChart = await Chart.GetDataParsed(parsed);

            //from person get svg report
            var foundPerson = await APITools.GetPersonById(requestChart.PersonId);

            var eventsChartSvgString = await EventsChartManager.GenerateEventsChart(foundPerson, requestChart.StartTime,
                requestChart.EndTime, requestChart.DaysPerPixel, requestChart.EventTagList);

            return eventsChartSvgString;
        }

        private static async Task<Chart> GenerateNewChart(Person foundPerson, Time startTime, Time endTime, double daysPerPixel, List<EventTag> eventTags)
        {
            //from person get svg report
            var eventsChartSvgString = await EventsChartManager.GenerateEventsChart(foundPerson, startTime, endTime, daysPerPixel, eventTags);

            //a new chart is born
            var newChartId = Tools.GenerateId();
            var startTimeXml = startTime.ToXml();
            var endTimeXml = endTime.ToXml();
            var newChart = new Chart(newChartId, eventsChartSvgString, foundPerson.Id, startTimeXml, endTimeXml, daysPerPixel, eventTags);

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
