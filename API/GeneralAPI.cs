using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Genso.Astrology.Library;
using Genso.Astrology.Library.Compatibility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;

namespace API
{
    /// <summary>
    /// All API calls with no home are here, send them somewhere you think is good
    /// </summary>
    public class GeneralAPI
    {
        /// <summary>
        /// Function for debugging purposes
        /// Call to see if return correct IP
        /// </summary>
        [FunctionName("getipaddress")]
        public static async Task<IActionResult> GetIpAddress([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequestMessage incomingRequest)
        {
            return APITools.PassMessage(incomingRequest?.GetCallerIp()?.ToString() ?? "no ip");
        }

        [FunctionName("getmatchreport")]
        public static async Task<IActionResult> GetMatchReport([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestMessage incomingRequest)
        {

            try
            {
                //get name of male & female
                var rootXml = APITools.ExtractDataFromRequest(incomingRequest);
                var maleId = rootXml.Element("MaleId")?.Value;
                var femaleId = rootXml.Element("FemaleId")?.Value;

                //generate compatibility report
                var compatibilityReport = await APITools.GetCompatibilityReport(maleId, femaleId);
                return APITools.PassMessage(compatibilityReport.ToXml());
            }
            catch (Exception e)
            {
                //log error
                await Log.Error(e, incomingRequest);
                //format error nicely to show user
                return APITools.FailMessage(e);
            }

        }

        [FunctionName("gethoroscope")]
        public static async Task<IActionResult> GetHoroscope([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestMessage incomingRequest)
        {

            try
            {
                //get person from request
                var rootXml = APITools.ExtractDataFromRequest(incomingRequest);
                var personId = rootXml.Value;

                var person = await APITools.GetPersonFromId(personId);

                //calculate predictions for current person
                var predictionList = await APITools.GetPrediction(person);

                //convert list to xml string in root elm
                return APITools.PassMessage(Tools.AnyTypeToXmlList(predictionList));

            }
            catch (Exception e)
            {
                //log error
                await Log.Error(e, incomingRequest);
                //format error nicely to show user
                return APITools.FailMessage(e);
            }

        }



    }
}
