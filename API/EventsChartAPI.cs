using System.Xml.Linq;
using VedAstro.Library;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Azure.Storage.Blobs;
using System.Net;
using static SuperConvert.Enums;
using System.Collections;

namespace API
{



    public static class EventsChartAPI
    {

        private static Dictionary<double, string> _cacheList;

        //CENTRAL FOR ROUTES
        private const string RouteGetEventsChartNoCache = "GetEventsChartNoCache/UserId/{userId}/VisitorId/{visitorId}";
        private const string RouteGetEventsChart = "GetEventsChart/UserId/{userId}/VisitorId/{visitorId}";
        private const string SendGetEventsChart = "SendEventsChart/Email/{email}";



        //▄▀█ █▀█ █   █▀▀ █░█ █▄░█ █▀▀ ▀█▀ █ █▀█ █▄░█ █▀
        //█▀█ █▀▀ █   █▀░ █▄█ █░▀█ █▄▄ ░█░ █ █▄█ █░▀█ ▄█


        /// <summary>
        /// Main func to generate event charts used by site, via awesome built in cache mechanism
        /// </summary>
        [Function(nameof(GetEventsChart))]
        public static async Task<HttpResponseData> GetEventsChart(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = RouteGetEventsChart)] HttpRequestData incomingRequest,
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
                    var foundPerson = await Tools.GetPersonById(chartSpecsOnly.PersonId);
                    var chartSvg = await EventsChartManager.GenerateEventsChart(
                        foundPerson,
                        chartSpecsOnly.TimeRange,
                        chartSpecsOnly.DaysPerPixel,
                        chartSpecsOnly.EventTagList,
                        chartSpecsOnly.Options);
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
                //data comes out of caller, basic spec on how the chart should be
                var requestJson = await APITools.ExtractDataFromRequestJson(incomingRequest);

                //check if the specs given is correct and readable
                //this is partially filled chart with no generated svg content only specs
                var chartSpecsOnly = await EventsChart.FromJsonSpecOnly(requestJson);

                //a hash to id the chart's specs (caching)
                var chartId = chartSpecsOnly.GetEventsChartSignature();

                //PREPARE THE CALL
                var foundPerson = await Tools.GetPersonById(chartSpecsOnly.PersonId);
                var chartSvg = await EventsChartManager.GenerateEventsChart(
                    foundPerson,
                    chartSpecsOnly.TimeRange,
                    chartSpecsOnly.DaysPerPixel,
                    chartSpecsOnly.EventTagList,
                    chartSpecsOnly.Options);
                //return chartSvg;

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
                await APILogger.Error(e);
                var response = incomingRequest.CreateResponse(HttpStatusCode.OK);
                response.Headers.Add("Call-Status", "Fail"); //caller checks this
                response.Headers.Add("Access-Control-Expose-Headers", "Call-Status"); //needed by silly browser to read call-status
                return response;
            }
        }


        /// <summary>
        /// SPECIAL DEBUG version to generate life chart without cache for R & D purposes
        /// </summary>
        [Function(nameof(GetEventsChartNoCache))]
        public static async Task<HttpResponseData> GetEventsChartNoCache(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = RouteGetEventsChartNoCache)] HttpRequestData incomingRequest,
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
                var foundPerson = await Tools.GetPersonById(chartSpecsOnly.PersonId);
                var chartSvg = await EventsChartManager.GenerateEventsChart(
                    foundPerson,
                    chartSpecsOnly.TimeRange,
                    chartSpecsOnly.DaysPerPixel,
                    chartSpecsOnly.EventTagList,
                    chartSpecsOnly.Options);
                //return chartSvg;


                //send image back to caller
                return APITools.SendSvgToCaller(chartSvg, incomingRequest);

            }
            catch (Exception e)
            {
                //log it
                await APILogger.Error(e);
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
                var personName = (await Tools.FindPersonXMLById(personId)).Name;
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
                var personName = (await Tools.GetPersonById(chart.PersonId)).Name;
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
                var savedChartXmlDoc = await Tools.GetXmlFileFromAzureStorage(APITools.SavedEventsChartListFile, Tools.BlobContainerName);

                //extract out the name & id
                var rootXml = new XElement("Root");

                //make a name & id only xml list of saved charts
                //note: done so to not download what is not needed
                var savedChartXmlList = savedChartXmlDoc?.Root?.Elements().ToList() ?? new List<XElement>(); //empty list so no failure
                foreach (var chartXml in savedChartXmlList)
                {
                    var parsedChart = EventsChart.FromXml(chartXml);
                    var person = await Tools.GetPersonById(parsedChart.PersonId);
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
                var savedChartListClient = await Tools.GetBlobClientAzure(APITools.SavedEventsChartListFile, Tools.BlobContainerName);
                var savedChartListXml = await Tools.GetXmlFileFromAzureStorage(APITools.SavedEventsChartListFile, Tools.BlobContainerName);
                await Tools.OverwriteBlobData(savedChartListClient, savedChartListXml);

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
            var algorithmFuncsList = new List<AlgorithmFuncs>() { EventsChartManager.Algorithm.GetGeneralScore };
            var summaryOptions = new ChartOptions(algorithmFuncsList);


            //minus 7% from width for side padding
            //so that chart not squeezed to side of page
            maxWidth = maxWidth - (int)(maxWidth * 0.07); ;

            //EXTRACT & PREPARE DATA

            //get the person instance by id
            var foundPerson = await Tools.GetPersonById(personId);

            //TIME
            var timeRange = EventChartTools.AutoCalculateTimeRange(foundPerson, timePreset, timezone);

            //EVENTS
            var eventTags = GetSelectedEventTypesEasy(eventPreset);

            //PRECISION
            //calculate based on max screen width,
            //done for fast calculation only for needed viewability
            var daysPerPixel = GetDayPerPixel(timeRange, maxWidth);

            return await Tools.GenerateNewChart(foundPerson, timeRange, daysPerPixel, eventTags, summaryOptions);
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
