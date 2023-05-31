using System.Xml.Linq;
using VedAstro.Library;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Time = VedAstro.Library.Time;
using Azure.Storage.Blobs;
using System.Net;

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



        [Function(nameof(GetEventsChart))]
        public static async Task<HttpResponseData> GetEventsChart(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "GetEventsChart/UserId/{userId}/VisitorId/{visitorId}")] HttpRequestData incomingRequest,
            string userId, string visitorId)
        {

            try
            {
                //data comes out of caller, basic spec on how the chart should be
                var requestJson = await APITools.ExtractDataFromRequestJson(incomingRequest);

                //check if the specs given is correct and readable
                //this is partially filled chart with no generated svg content only specs
                var chartSpecsOnly = await EventsChart.FromJsonSpecOnly(requestJson);

                //a hash to id the chart's specs (caching)
                var chartId = chartSpecsOnly.GetEventsChartSignature();

                //PREPARE THE CALL
                Func<Task<string>> generateChart = async () =>
                {
                    var foundPerson = await APITools.GetPersonById(chartSpecsOnly.PersonId);
                    var chartSvg = await EventsChartManager.GenerateEventsChart(
                        foundPerson,
                        chartSpecsOnly.TimeRange,
                        chartSpecsOnly.DaysPerPixel,
                        chartSpecsOnly.EventTagList);
                    return chartSvg;
                };

                //NOTE USING CHART ID INSTEAD OF CALLER ID, FOR CACHE SHARING BETWEEN ALL WHO COME
                Func<Task<BlobClient>> cacheExecuteTask = () => APITools.ExecuteAndSaveToCache(generateChart, chartId);


                //CACHE MECHANISM
                var callerInfo = new CallerInfo(visitorId, userId);//get who or what is calling
                callerInfo.CallerId = chartId;//NOTE OVERRIDE CALLER ID TO CHART FOR CACHE SHARING
                var httpResponseData = await AzureCache.CacheExecute(cacheExecuteTask, callerInfo, incomingRequest);

                return httpResponseData;

            }
            catch (Exception e)
            {
                //log it
                await APILogger.Error(e);
                var response = incomingRequest.CreateResponse(HttpStatusCode.OK);
                response.Headers.Add("Call-Status", "Fail"); //caller checks this
                return response;
            }

        }




        //---------------------------------------


        /// <summary>
        /// Special function to make Light Viewer(EventsChartViewer.html) work
        /// This API is called by EventsChart.js when viewing chart view lite viewer
        /// </summary>
        [Function("geteventscharteasy")]
        public static async Task<HttpResponseData> GetEventsChartEasy([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestData incomingRequest)
        {
            try
            {

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
                throw new NotImplementedException("NOT YET DONE BRO!!");

                ////generate report
                //var chart = await GetEventReportSvgForIncomingRequest(incomingRequest, false);

                ////save chart into storage
                ////note: do not wait to speed things up, beware! failure will go undetected on client
                //APITools.AddXElementToXDocumentAzure(chart.ToXml(), APITools.SavedEventsChartListFile, APITools.BlobContainerName);

                ////let caller know all good
                //return APITools.PassMessage(incomingRequest);

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
                var chart = EventsChart.FromXml(foundChartXml);
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
                var chart = EventsChart.FromXml(foundChartXml);

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
                var chart = EventsChart.FromXml(foundChartXml);

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
                    var parsedChart = EventsChart.FromXml(chartXml);
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
                var chartList = new List<EventsChart>();
                var dasaEventTags = new List<EventTag> { EventTag.Dasa, EventTag.Bhukti, EventTag.Antaram, EventTag.Gochara, EventTag.DasaSpecialRules };
                //PRECISION
                //calculate based on max screen width,
                //done for fast calculation only for needed viewability
                var timeRange = new TimeRange(startTime, endTime);
                var daysPerPixel = GetDayPerPixel(timeRange, 800);


                var combinedSvg = "";
                var chartYPosition = 30; //start with top padding
                var leftPadding = 10;
                foreach (var possibleTime in possibleTimeList)
                {
                    //replace original birth time
                    var personAdjusted = person.ChangeBirthTime(possibleTime);
                    var newChart = await GenerateNewChart(personAdjusted, timeRange, daysPerPixel, dasaEventTags);
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


        ///// <summary>
        ///// processes incoming xml request and outputs events svg report
        ///// </summary>
        //private static async Task<EventsChart> GetEventReportSvgForIncomingRequest(HttpRequestData req, bool cacheEnable)
        //{
        //    //get all the data needed out of the incoming request
        //    var rootXml = await APITools.ExtractDataFromRequestXml(req);
        //    var personId = rootXml.Element("PersonId")?.Value;
        //    var eventTagListXml = rootXml.Element("EventTagList");
        //    var eventTags = EventTagExtensions.FromXmlList(eventTagListXml);
        //    var startTimeXml = rootXml.Element("StartTime")?.Elements().First();
        //    var startTime = Time.FromXml(startTimeXml);
        //    var endTimeXml = rootXml.Element("EndTime")?.Elements().First();
        //    var endTime = Time.FromXml(endTimeXml);
        //    var daysPerPixel = double.Parse(rootXml.Element("DaysPerPixel")?.Value ?? "0");

        //    //get the person instance by id
        //    var foundPerson = await APITools.GetPersonById(personId);

        //    EventsChart chart = EventsChart.Empty;

        //    if (cacheEnable)
        //    {
        //        //todo needs testing
        //        //based on mode set by caller use cache
        //        //chart = await GetChartCached(foundPerson, startTime, endTime, daysPerPixel, eventTags);
        //    }
        //    else
        //    {
        //        //based on mode set by caller use cache
        //        chart = await GenerateNewChart(foundPerson, timeRange, daysPerPixel, eventTags);
        //    }

        //    return chart;
        //}

        private static async Task<EventsChart> GetEventReportSvgForIncomingRequestEasy(HttpRequestData req)
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
            var timeRange = EventChartTools.AutoCalculateTimeRange(foundPerson, timePreset, timezone);

            //EVENTS
            var eventTags = GetSelectedEventTypesEasy(eventPreset);

            //PRECISION
            //calculate based on max screen width,
            //done for fast calculation only for needed viewability
            var daysPerPixel = GetDayPerPixel(timeRange, maxWidth);


            return await GenerateNewChart(foundPerson, timeRange, daysPerPixel, eventTags);
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


        private static async Task<EventsChart> GenerateNewChart(Person foundPerson, TimeRange timeRange, double daysPerPixel, List<EventTag> eventTags)
        {
            //from person get svg report
            var eventsChartSvgString = await EventsChartManager.GenerateEventsChart(foundPerson, timeRange, daysPerPixel, eventTags);

            //a new chart is born
            var newChartId = Tools.GenerateId();
            var newChart = new EventsChart(newChartId, eventsChartSvgString, foundPerson.Id, timeRange, daysPerPixel, eventTags);

            return newChart;
        }

        /// <summary>
        /// calculates the precision of the events to fit inside 1000px width
        /// </summary>
        private static double GetDayPerPixel(TimeRange timeRange, int maxWidth)
        {
            //const int maxWidth = 1000; //px
            var daysPerPixel = Math.Round(timeRange.daysBetween / maxWidth, 3); //small val = higher precision
                                                                                //var daysPerPixel = Math.Round(yearsBetween * 0.4, 3); //small val = higher precision
                                                                                //daysPerPixel = daysPerPixel < 1 ? 1 : daysPerPixel; // minimum 1 day per px

            //if final value is 0 then recalculate without rounding
            daysPerPixel = daysPerPixel <= 0 ? timeRange.daysBetween / maxWidth : daysPerPixel;

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
