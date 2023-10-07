using System.Linq.Expressions;
using Azure.Data.Tables;
using Azure;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using VedAstro.Library;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;
using Google.Protobuf.WellKnownTypes;
using Azure.Core;
using Azure.Storage.Blobs;
using System.Net;

namespace API
{
    public class MLTableAPI
    {

        /// <summary>
        /// Generates Time List from an excel file uploaded by user to be parsed and returned as Time list
        /// </summary>
        [Function(nameof(GetMLTimeListFromExcel))]
        public static async Task<HttpResponseData> GetMLTimeListFromExcel(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = nameof(GetMLTimeListFromExcel))]
            HttpRequestData incomingRequest)
        {
            try
            {
                //0 : LOG CALL
                //log ip address, call time and URL
                var call = await APILogger.Visit(incomingRequest);

                //1 : GET DATA OUT 
                var excelBinary = incomingRequest.Body;
                excelBinary.Position = 0;
                var foundRawTimeList = await Tools.ExtractTimeColumnFromExcel(excelBinary);
                var foundGeoLocationList = await Tools.ExtractLocationColumnFromExcel(excelBinary);

                //3 : COMBINE DATA
                var returnList = foundRawTimeList.Select(dateTimeOffset => new Time(dateTimeOffset, foundGeoLocationList[foundRawTimeList.IndexOf(dateTimeOffset)])).ToList();

                //convert raw XML to Person Json
                var personListJson = Tools.ListToJson(returnList);

                return APITools.PassMessageJson(personListJson, incomingRequest);

            }

            //if any failure, show error in payload
            catch (Exception e)
            {
                APILogger.Error(e, incomingRequest);
                return APITools.FailMessageJson(e.Message, incomingRequest);
            }

        }

        /// <summary>
        /// Generates an instance of ML Table and is sent back wrapped in JSON form
        /// </summary>
        [Function(nameof(GenerateMLTable))]
        public static async Task<HttpResponseData> GenerateMLTable(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = nameof(GenerateMLTable))]
            HttpRequestData incomingRequest)
        {
            //0 : LOG CALL
            //log ip address, call time and URL
            var call = APILogger.Visit(incomingRequest);

            //1 : PREPARE PARAMS DATA
            //prepare url to call
            //get data out of call
            var rootJson = await APITools.ExtractDataFromRequestJson(incomingRequest);

            throw new NotImplementedException();

            //var url = $"{_api.URL.GetPersonList}/UserId/{_api.UserId}/VisitorId/{_api.VisitorID}";
            //CachedPersonList = await _api.GetList(url, Person.FromJsonList);


            ////2 : GENERATE TABLE (HEAVY COMPUTE 🚀) 
            //var timeSlices = new List<Time>();
            //var openApiMetadatas = new List<OpenAPIMetadata>();
            //var newMLTable = MLTable.FromData(timeSlices, openApiMetadatas);


            ////3 : SEND TO CALLER (HTML)
            //var jsonMLTable = newMLTable.ToJson();
            //return APITools.PassMessageJson(jsonMLTable, incomingRequest);


        }



        //----------------------------------PRIVATE FUNCS-----------------------------


    }
}
