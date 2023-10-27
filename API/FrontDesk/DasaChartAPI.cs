using VedAstro.Library;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Azure.Storage.Blobs;
using System.Net;

namespace API
{

    public static class DasaChartAPI
    {

        private static Dictionary<double, string> _cacheList;

        //CENTRAL FOR ROUTES
        private const string RouteGetDasaChartNoCache = "DasaChartNoCache/{*settingsUrl}";
        private const string RouteGetDasaChart = "DasaChart/{*settingsUrl}";
        private const string SendGetDasaChart = "SendDasaChart/Email/{email}";



        //▄▀█ █▀█ █   █▀▀ █░█ █▄░█ █▀▀ ▀█▀ █ █▀█ █▄░█ █▀
        //█▀█ █▀▀ █   █▀░ █▄█ █░▀█ █▄▄ ░█░ █ █▄█ █░▀█ ▄█


        /// <summary>
        /// Main func to generate event charts used by site, via awesome built in cache mechanism
        /// </summary>
        [Function(nameof(GetDasaChart))]
        public static async Task<HttpResponseData> GetDasaChart(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = RouteGetDasaChart)] HttpRequestData incomingRequest,
             string settingsUrl)
        {

            try
            {

                //get basic spec on how to make chart
                //check if the specs given is correct and readable
                //this is partially filled chart with no generated svg content only specs
                var chartSpecsOnly = await  DasaChart.FromUrl(settingsUrl);

                //a hash to id the chart's specs (caching)
                var chartId = chartSpecsOnly.GetDasaChartSignature();

                //PREPARE THE CALL
                Func<Task<string>> generateChart = async () =>
                {
                    var chartSvg = await DasaChartManager.GenerateDasaChartSvg(chartSpecsOnly);
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
        [Function(nameof(GetDasaChartNoCache))]
        public static async Task<HttpResponseData> GetDasaChartNoCache(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = RouteGetDasaChartNoCache)] HttpRequestData incomingRequest,
            string userId, string visitorId, string settingsUrl)
        {

            try
            {
                //check if the specs given is correct and readable
                //this is partially filled chart with no generated svg content only specs
                var chartSpecsOnly = await DasaChart.FromUrl(settingsUrl);

                //PREPARE THE CALL
                var chartSvg = await DasaChartManager.GenerateDasaChartSvg(chartSpecsOnly);

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
        [Function(nameof(SendDasaChartToEmail))]
        public static async Task<HttpResponseData> SendDasaChartToEmail(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = SendGetDasaChart)] HttpRequestData incomingRequest, string receiverEmail)
        {

            try
            {
                //data comes out of caller, basic spec on how the chart should be
                var requestJson = await APITools.ExtractDataFromRequestJson(incomingRequest);

                //check if the specs given is correct and readable
                //this is partially filled chart with no generated svg content only specs
                var chartSpecsOnly = await DasaChart.FromJson(requestJson);

                //PREPARE THE CALL
                var foundPerson = Tools.GetPersonById(chartSpecsOnly.Person.Id);
                var chartSvg = await DasaChartManager.GenerateDasaChartSvg(chartSpecsOnly);

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
