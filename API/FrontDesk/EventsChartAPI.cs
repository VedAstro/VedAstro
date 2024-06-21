using VedAstro.Library;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Azure.Storage.Blobs;
using System.Net;
using Newtonsoft.Json.Linq;

namespace API
{

    public static class EventsChartAPI
    {

        private static Dictionary<double, string> _cacheList;

        //CENTRAL FOR ROUTES
        private const string RouteGetEventsChartNoCache = "EventsChartNoCache/{*settingsUrl}";
        private const string RouteGetEventsChart = "EventsChart/{*settingsUrl}";
        private const string SendGetEventsChart = "SendEventsChart/Email/{email}";



        //▄▀█ █▀█ █   █▀▀ █░█ █▄░█ █▀▀ ▀█▀ █ █▀█ █▄░█ █▀
        //█▀█ █▀▀ █   █▀░ █▄█ █░▀█ █▄▄ ░█░ █ █▄█ █░▀█ ▄█

        /// <summary>
        /// Gets all saved/cached chart names for a person,
        /// NOTE:
        /// user than selects the chart, with data,
        /// when called via generate will auto get the cached chart
        /// </summary>
        [Function(nameof(GetSavedChartNameList))]
        public static async Task<HttpResponseData> GetSavedChartNameList(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = $"{nameof(GetSavedChartNameList)}/{{personId}}")] HttpRequestData req, string personId)
        {
            try
            {
                //get all in cache
                var allSavedCharts = AzureCache.ListBlobs(personId);

                var eventDataList = new JArray();
                foreach (var chart in allSavedCharts)
                {
                    EventsChart parsedChart = await EventsChart.FromCacheName(chart.Name);

                    eventDataList.Add(parsedChart.ToJson());
                }

                return APITools.PassMessageJson(eventDataList, req);
            }

            //if any failure, show error in payload
            catch (Exception e)
            {
                APILogger.Error(e, req);
                return APITools.FailMessageJson(e.Message, req);
            }
        }

        /// <summary>
        /// Gets all entries in EventDataList.xml as parsed JSON
        /// NOTE:
        /// used by good time finder
        /// </summary>
        [Function(nameof(GetEventDataList))]
        public static async Task<HttpResponseData> GetEventDataList(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = $"{nameof(GetEventDataList)}")] HttpRequestData req)
        {
            try
            {
                //get raw xml file
                //package nicely into JSON
                var eventDataList = new JArray();
                foreach (var eventData in EventDataListStatic.Rows)
                {
                    eventDataList.Add(eventData.ToJson());
                }


                return APITools.PassMessageJson(eventDataList, req);
            }

            //if any failure, show error in payload
            catch (Exception e)
            {
                APILogger.Error(e, req);
                return APITools.FailMessageJson(e.Message, req);
            }
        }


        /// <summary>
        /// Main func to generate event charts used by site, via awesome built in cache mechanism
        /// </summary>
        [Function(nameof(GetEventsChart))]
        public static async Task<HttpResponseData> GetEventsChart(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = RouteGetEventsChart)] HttpRequestData incomingRequest,
             string settingsUrl)
        {

            try
            {
                //1 : CUSTOM AYANAMSA (removes ayanamsa once read)
                settingsUrl = OpenAPI.ParseAndSetAyanamsa(settingsUrl);


                //get basic spec on how to make chart
                //check if the specs given is correct and readable
                //this is partially filled chart with no generated svg content only specs
                var chartSpecsOnly = await EventsChart.FromUrl(settingsUrl);

                //a hash to id the chart's specs (caching)
                var chartId = chartSpecsOnly.GetEventsChartSignature();

                //PREPARE THE CALL
                Func<string> generateChart = () =>
                {
                    var chartSvg = EventsChartFactory.GenerateEventsChartSvg(chartSpecsOnly);
                    return chartSvg;
                };

                //NOTE USING CHART ID INSTEAD OF CALLER ID, FOR CACHE SHARING BETWEEN ALL WHO COME
                Func<Task<BlobClient>> cacheExecuteTask = () => APITools.ExecuteAndSaveToCache(generateChart, chartId);


                //CACHE MECHANISM
                var callerInfo = new CallerInfo("101", "101");//disabled because no space to squeeze in URL
                callerInfo.CallerId = chartId;//NOTE OVERRIDE CALLER ID TO CHART FOR CACHE SHARING
                HttpResponseData httpResponseData = await AzureCache.CacheExecute(cacheExecuteTask, callerInfo, incomingRequest);

                // mark as SVG to be viewed direct in browser, and remove old if already set
                httpResponseData.Headers.Remove("Content-Type");
                httpResponseData.Headers.Add("Content-Type", "image/svg+xml");
                httpResponseData.Headers.Add("Access-Control-Allow-Origin", "*"); //for CORS

                return httpResponseData;

            }
            catch (Exception e)
            {
                //log it
                APILogger.Error(e);
                var response = incomingRequest.CreateResponse(HttpStatusCode.OK);
                response.Headers.Add("Call-Status", "Fail"); //caller checks this
                response.Headers.Add("Access-Control-Expose-Headers", "Call-Status"); //needed by silly browser to read call-status
                response.Headers.Add("Access-Control-Allow-Origin", "*"); //for CORS

                return response;
            }

        }


        /// <summary>
        /// SPECIAL DEBUG version to generate life chart without cache for R & D purposes
        /// </summary>
        [Function(nameof(GetEventsChartNoCache))]
        public static async Task<HttpResponseData> GetEventsChartNoCache(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = RouteGetEventsChartNoCache)] HttpRequestData incomingRequest,
            string userId, string visitorId, string settingsUrl)
        {

            try
            {
                //SET ayanamsa to RAMAN
                Calculate.Ayanamsa = (int)Ayanamsa.RAMAN;

                //check if the specs given is correct and readable
                //this is partially filled chart with no generated svg content only specs
                var chartSpecsOnly = await EventsChart.FromUrl(settingsUrl);

                //PREPARE THE CALL
                var chartSvg = EventsChartFactory.GenerateEventsChartSvg(chartSpecsOnly);

                //send image back to caller
                return APITools.SendSvgToCaller(chartSvg, incomingRequest);

            }
            catch (Exception e)
            {
                //log it
                APILogger.Error(e);
                var response = incomingRequest.CreateResponse(HttpStatusCode.OK);
                response.Headers.Add("Call-Status", "Fail"); //caller checks this
                response.Headers.Add("Access-Control-Expose-Headers", "Call-Status"); //needed by silly browser to read call-status
                return response;
            }

        }


        /// <summary>
        /// creates an event chart and send it to email
        /// calculates new does not use cache
        /// </summary>
        [Function(nameof(SendEventsChartToEmail))]
        public static async Task<HttpResponseData> SendEventsChartToEmail(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = SendGetEventsChart)] HttpRequestData incomingRequest, string receiverEmail)
        {

            try
            {
                //SET ayanamsa to RAMAN
                Calculate.Ayanamsa = (int)Ayanamsa.RAMAN;

                //data comes out of caller, basic spec on how the chart should be
                var requestJson = await APITools.ExtractDataFromRequestJson(incomingRequest);

                //check if the specs given is correct and readable
                //this is partially filled chart with no generated svg content only specs
                var chartSpecsOnly = await EventsChart.FromJson(requestJson);

                //PREPARE THE CALL
                var foundPerson = Tools.GetPersonById(chartSpecsOnly.Person.Id);
                var chartSvg = EventsChartFactory.GenerateEventsChartSvg(chartSpecsOnly);

                //string to binary
                byte[] rawFileBytes = System.Text.Encoding.UTF8.GetBytes(chartSvg); //SVG uses UTF-8
                MemoryStream stream = new MemoryStream(rawFileBytes);

                //using Azure Email Sender, send file to given email
                var fileName = $"Chart-{foundPerson.Name}";
                APITools.SendEmail(fileName, "svg", receiverEmail, stream);

                return APITools.PassMessageJson("Email sent success", incomingRequest);


            }
            catch (Exception e)
            {
                //log it
                APILogger.Error(e);
                var response = incomingRequest.CreateResponse(HttpStatusCode.OK);
                response.Headers.Add("Call-Status", "Fail"); //caller checks this
                response.Headers.Add("Access-Control-Expose-Headers", "Call-Status"); //needed by silly browser to read call-status
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
                APILogger.Error(e, incomingRequest);

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
                var eventsChartViewerHtml = await Tools.GetStringFileHttp(APITools.Url.EventsChartViewerHtml);

                //insert person name into page, to show ready page faster
                var personName = Tools.GetPersonById(personId).Name;
                var jsVariables = $@"window.PersonName = ""{personName}"";";
                jsVariables += $@"window.ChartType = ""{"Muhurtha"}"";";
                jsVariables += $@"window.PersonId = ""{personId}"";";
                var finalHtml = eventsChartViewerHtml.Replace("/*INSERT-JS-VAR-HERE*/", jsVariables);

                return APITools.SendHtmlToCaller(finalHtml, incomingRequest);

            }
            catch (Exception e)
            {
                //log error
                APILogger.Error(e, incomingRequest);

                //format error nicely to show user
                return APITools.FailMessage(e, incomingRequest);
            }

        }





        //█▀█ █▀█ █ █░█ ▄▀█ ▀█▀ █▀▀   █▀▄▀█ █▀▀ ▀█▀ █░█ █▀█ █▀▄ █▀
        //█▀▀ █▀▄ █ ▀▄▀ █▀█ ░█░ ██▄   █░▀░█ ██▄ ░█░ █▀█ █▄█ █▄▀ ▄█



        private static async Task<EventsChart> GetEventReportSvgForIncomingRequestEasy(HttpRequestData req)
        {
            //ACT 1 : SET VARIABLES

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

            //todo needs to be pumped in from input
            var algorithmFuncsList = new List<AlgorithmFuncs>() { Algorithm.General };
            var summaryOptions = new ChartOptions(algorithmFuncsList);

            //minus 7% from width for side padding
            //so that chart not squeezed to side of page
            maxWidth = maxWidth - (int)(maxWidth * 0.07); ;

            //EXTRACT & PREPARE DATA

            //get the person instance by id
            var foundPerson = Tools.GetPersonById(personId);

            //TIME
            var timeRange = EventChartTools.AutoCalculateTimeRange(foundPerson, timePreset, timezone);

            //EVENTS
            var eventTags = GetSelectedEventTypesEasy(eventPreset);

            //PRECISION
            //calculate based on max screen width,
            //done for fast calculation only for needed viewability
            var daysPerPixel = GetDayPerPixel(timeRange, maxWidth);

            return EventsChartFactory.GenerateEventsChart(foundPerson, timeRange, daysPerPixel, eventTags, summaryOptions);
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
