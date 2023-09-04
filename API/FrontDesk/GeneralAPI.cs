using VedAstro.Library;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

namespace API
{
    /// <summary>
    /// All API calls with no home are here, send them somewhere you think is good
    /// </summary>
    public class GeneralAPI
    {

        [Function("gethoroscope")]
        public static async Task<HttpResponseData> GetHoroscope([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestData incomingRequest)
        {

            try
            {
                //get person from request
                var rootXml = await APITools.ExtractDataFromRequestXml(incomingRequest);
                var personId = rootXml.Value;

                var person = await Tools.GetPersonById(personId);

                //calculate predictions for current person
                var predictionList = await Tools.GetHoroscopePrediction(person.BirthTime, APITools.HoroscopeDataListFile);

                var sortedList = SortPredictionData(predictionList);

                //convert list to xml string in root elm
                return APITools.PassMessage(Tools.AnyTypeToXmlList(sortedList), incomingRequest);

            }
            catch (Exception e)
            {
                //log error
                await APILogger.Error(e, incomingRequest);
                //format error nicely to show user
                return APITools.FailMessage(e, incomingRequest);
            }



            List<HoroscopePrediction> SortPredictionData(List<HoroscopePrediction> horoscopePredictions)
            {
                //put rising sign at top
                horoscopePredictions.MoveToBeginning((horPre) => horPre.FormattedName.ToLower().Contains("rising"));

                //todo followed by planet in sign prediction ordered by planet strength 

                return horoscopePredictions;
            }

        }



    }
}
