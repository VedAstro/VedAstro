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
        [Function(nameof(SavedChartNameList))]
        public static async Task<HttpResponseData> SavedChartNameList(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = $"{nameof(SavedChartNameList)}/{{personId}}")] HttpRequestData req, string personId)
        {
            try
            {
                //get all in cache
                var allSavedCharts = AzureCache.ListBlobs(personId);

                var eventDataList = new JArray();
                foreach (var chart in allSavedCharts)
                {
                    EventsChart parsedChart = await VedAstro.Library.EventsChart.FromCacheName(chart.Name);

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
        /// Main func to generate event charts used by site, via awesome built in cache mechanism
        /// </summary>
        [Function(nameof(EventsChart))]
        public static async Task<HttpResponseData> EventsChart(
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
                var chartSpecsOnly = await VedAstro.Library.EventsChart.FromUrl(settingsUrl);

                //a hash to id the chart's specs (caching)
                var chartId = chartSpecsOnly.GetEventsChartSignature();

                //PREPARE THE CALL
                Func<string> generateChart = () =>
                {
                    var chartSvg = EventsChartFactory.GenerateEventsChartSvg(chartSpecsOnly);
                    return chartSvg;
                };

                //NOTE USING CHART ID INSTEAD OF CALLER ID, FOR CACHE SHARING BETWEEN ALL WHO COME
                Func<Task<BlobClient>> cacheExecuteTask = () => AzureCache.ExecuteAndSaveToCache(generateChart, chartId);


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
        [Function(nameof(EventsChartNoCache))]
        public static async Task<HttpResponseData> EventsChartNoCache(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = RouteGetEventsChartNoCache)] HttpRequestData incomingRequest,
            string userId, string visitorId, string settingsUrl)
        {

            try
            {
                //SET ayanamsa to RAMAN
                Calculate.Ayanamsa = (int)Ayanamsa.RAMAN;

                //check if the specs given is correct and readable
                //this is partially filled chart with no generated svg content only specs
                var chartSpecsOnly = await VedAstro.Library.EventsChart.FromUrl(settingsUrl);

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
                var chartSpecsOnly = await VedAstro.Library.EventsChart.FromJson(requestJson);

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




        //█▀█ █▀█ █ █░█ ▄▀█ ▀█▀ █▀▀   █▀▄▀█ █▀▀ ▀█▀ █░█ █▀█ █▀▄ █▀
        //█▀▀ █▀▄ █ ▀▄▀ █▀█ ░█░ ██▄   █░▀░█ ██▄ ░█░ █▀█ █▄█ █▄▀ ▄█


    }


}
