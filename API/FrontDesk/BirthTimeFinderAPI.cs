using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using VedAstro.Library;
using System.Net;
using Newtonsoft.Json.Linq;
using Time = VedAstro.Library.Time;



//█░█░█ █░█ █▀▀ █▄░█   ▀█▀ █▀▀ █▀▀ █░█ █▄░█ █▀█ █░░ █▀█ █▀▀ █▄█   █▀▀ ▄▀█ █▄░█ ▀ ▀█▀   █▄▀ █▀▀ █▀▀ █▀█   █░█ █▀█
//▀▄▀▄▀ █▀█ ██▄ █░▀█   ░█░ ██▄ █▄▄ █▀█ █░▀█ █▄█ █▄▄ █▄█ █▄█ ░█░   █▄▄ █▀█ █░▀█ ░ ░█░   █░█ ██▄ ██▄ █▀▀   █▄█ █▀▀

//█░█░█ █ ▀█▀ █░█   █▄█ █▀█ █░█ █▀█   █ █▀▄ █▀▀ ▄▀█ █▀ ░   ▀█▀ █░█ █▀▀ █▄░█   █▄█ █▀█ █░█   █▄▀ █▄░█ █▀█ █░█░█
//▀▄▀▄▀ █ ░█░ █▀█   ░█░ █▄█ █▄█ █▀▄   █ █▄▀ ██▄ █▀█ ▄█ █   ░█░ █▀█ ██▄ █░▀█   ░█░ █▄█ █▄█   █░█ █░▀█ █▄█ ▀▄▀▄▀

//█▀▄ █▀█ █ █▄░█ █▀▀   █▀ ▀█▀ █░█ █▀▀ █▀▀
//█▄▀ █▄█ █ █░▀█ █▄█   ▄█ ░█░ █▄█ █▀░ █▀░
//06/07/2023

namespace API
{
    /// <summary>
    /// Group of API calls related to finding birth based on dictionary attack on time and other methods
    /// </summary>
    public static class BirthTimeFinderAPI
    {

        //CENTRAL FOR ROUTES
        private const string startTime = "{hhmmStart}/{dateStart}/{monthStart}/{yearStart}/{offsetStart}";
        private const string endTime = "{hhmmEnd}/{dateEnd}/{monthEnd}/{yearEnd}/{offsetEnd}";

        private const string FindBirthTime_Animal_Person = "FindBirthTime/Animal/PersonId/{personId}";
        private const string FindBirthTime_Animal_Time = "FindBirthTime/Animal/Location/{locationName}/StartTime/{startTime}/EndTime/{endTime}"; //todo

        private const string FindBirthTime_RisingSign_Person = "FindBirthTime/RisingSign/PersonId/{personId}";
        private const string FindBirthTime_RisingSign_Time = $"FindBirthTime/RisingSign/Location/{{locationName}}/StartTime/{startTime}/EndTime/{endTime}";

        private const string FindBirthTime_HouseStrength_Person = "FindBirthTime/HouseStrength/PersonId/{personId}";
        private const string FindBirthTime_EventsChart_Person = "FindBirthTime/EventsChart/PersonId/{personId}";



        //--------------------------------------------------------



        [Function(nameof(FindBirthTimeAnimal))]
        public static async Task<HttpResponseData> FindBirthTimeAnimal([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = FindBirthTime_Animal_Person)] HttpRequestData incomingRequest, string personId)
        {

            try
            {
                //get person record
                var foundPerson = Tools.GetPersonById(personId);

                //get list of possible birth time slice in the current birth day
                var timeSlices = Tools.GetTimeSlicesOnBirthDay(foundPerson, 1);

                //get predictions for each slice and place in out going list  
                var compiledObj = new JObject();
                foreach (var timeSlice in timeSlices)
                {
                    //replace original birth time
                    var personAdjusted = foundPerson.ChangeBirthTime(timeSlice);

                    //get the animal prediction for possible birth time
                    var newBirthConstellation = Calculate.MoonConstellation(personAdjusted.BirthTime).GetConstellationName();
                    var animal = Calculate.YoniKutaAnimalFromConstellation(newBirthConstellation);

                    //nicely packed
                    var named = new JProperty(timeSlice.ToString(), animal.ToString());
                    compiledObj.Add(named);

                }


                //send image back to caller
                return APITools.PassMessageJson(compiledObj, incomingRequest);

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

        [Function(nameof(FindBirthTimeEventsChartPerson))]
        public static async Task<HttpResponseData> FindBirthTimeEventsChartPerson([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = FindBirthTime_EventsChart_Person)] HttpRequestData incomingRequest, string personId)
        {

            try
            {
                //get person specified by caller
                var foundPerson = Tools.GetPersonById(personId);

                //generate the needed charts
                //var chartList = new List<EventsChart>();
                var eventTags = new List<EventTag> { EventTag.PD1, EventTag.PD2, EventTag.PD3, EventTag.PD4, EventTag.PD5, EventTag.Gochara };
                var algorithmFuncsList = new List<AlgorithmFuncs>() { Algorithm.General };
                var summaryOptions = new ChartOptions(algorithmFuncsList);

                //time range is preset to full life 100 years from birth
                var start = foundPerson.BirthTime;
                var end = foundPerson.BirthTime.AddYears(100);
                var timeRange = new TimeRange(start, end);

                //calculate based on max screen width,
                var daysPerPixel = EventsChart.GetDayPerPixel(timeRange, 1500);

                //get list of possible birth time slice in the current birth day
                var possibleTimeList = Tools.GetTimeSlicesOnBirthDay(foundPerson, 1);

                var combinedSvg = "";
                var chartYPosition = 30; //start with top padding
                var leftPadding = 10;
                foreach (var possibleTime in possibleTimeList)
                {
                    //replace original birth time
                    var personAdjusted = foundPerson.ChangeBirthTime(possibleTime);
                    var newChart = EventsChartFactory.GenerateEventsChart(personAdjusted, timeRange, daysPerPixel, eventTags, summaryOptions);
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
                    chartYPosition += 390;
                }

                //put all charts in 1 big container
                var finalSvg = EventsChartFactory.WrapSvgElements(
                    svgClass: "MultipleDasa",
                    combinedSvgString: combinedSvg,
                    svgWidth: 800,
                    svgTotalHeight: chartYPosition,
                    randomId: Tools.GenerateId(),
                    svgBackgroundColor: "#757575"); //grey easy on the eyes

                //send image back to caller
                return APITools.SendSvgToCaller(finalSvg, incomingRequest);
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
        /// Finds in same same day of birth, for quick & easy search
        /// </summary>
        [Function(nameof(FindBirthTimeRisingSignPerson))]
        public static async Task<HttpResponseData> FindBirthTimeRisingSignPerson([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = FindBirthTime_RisingSign_Person)] HttpRequestData incomingRequest, string personId)
        {
            try
            {
                //get person record
                var foundPerson = Tools.GetPersonById(personId);

                //get list of possible birth time slice in the current birth day
                var timeSlices = Tools.GetTimeSlicesOnBirthDay(foundPerson, 1);

                //get predictions for each slice and place in out going list  
                var compiledObj = new JObject();
                foreach (var timeSlice in timeSlices)
                {
                    //get all predictions for person
                    var allPredictions = Tools.GetHoroscopePrediction(timeSlice);
                    //select only rising sign
                    var selected = allPredictions.Where(x => x.FormattedName.Contains("Rising")).FirstOrDefault();

                    //nicely packed
                    var named = new JProperty(timeSlice.ToString(), selected.ToString());
                    compiledObj.Add(named);

                }


                //send image back to caller
                return APITools.PassMessageJson(compiledObj, incomingRequest);


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
        /// Finds in same same day of birth, for quick & easy search
        /// </summary>
        [Function(nameof(FindBirthTimeHouseStrengthPerson))]
        public static async Task<HttpResponseData> FindBirthTimeHouseStrengthPerson([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = FindBirthTime_HouseStrength_Person)] HttpRequestData incomingRequest, string personId)
        {
            try
            {
                //get person record
                var foundPerson = Tools.GetPersonById(personId);

                //get list of possible birth time slice in the current birth day
                var timeSlices = Tools.GetTimeSlicesOnBirthDay(foundPerson, 1);

                //get predictions for each slice and place in out going list  
                var compiledObj = new JObject();
                foreach (var timeSlice in timeSlices)
                {

                    //compile all house strengths into a nice presentable string
                    var finalString = "";
                    foreach (var house in House.AllHouses)
                    {
                        //get house strength
                        var strength = Calculate.HouseStrength(house, timeSlice).ToDouble(2);

                        //add to compiled string
                        var thisHouse = $"{house} {strength},";
                        finalString += thisHouse;

                    }


                    //nicely packed with TIME next to variable data
                    var named = new JProperty(timeSlice.ToString(), finalString);
                    compiledObj.Add(named);

                }


                //send image back to caller
                return APITools.PassMessageJson(compiledObj, incomingRequest);


            }
            catch (Exception e)
            {
                //log error
                APILogger.Error(e, incomingRequest);
                //format error nicely to show user
                return APITools.FailMessage(e, incomingRequest);
            }
        }


        [Function(nameof(FindBirthTimeRisingSignTime))]
        public static async Task<HttpResponseData> FindBirthTimeRisingSignTime([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = FindBirthTime_RisingSign_Time)] HttpRequestData incomingRequest,
            string locationName,
            string hhmmStart,
            string dateStart,
            string monthStart,
            string yearStart,
            string offsetStart,
            string hhmmEnd,
            string dateEnd,
            string monthEnd,
            string yearEnd,
            string offsetEnd
            )
        {
            try
            {

                //parse time range from caller (possible to fail)
                var startTime = await Tools.ParseTime(locationName, hhmmStart, dateStart, monthStart, yearStart);
                var endTime = await Tools.ParseTime(locationName, hhmmEnd, dateEnd, monthEnd, yearEnd);

                //get list of possible birth time slice in the current birth day
                var timeSlices = Time.GetTimeListFromRange(startTime, endTime, 1);

                //get predictions for each slice and place in out going list  
                var compiledObj = new JObject();
                foreach (var timeSlice in timeSlices)
                {

                    //get all predictions for person
                    var allPredictions = Tools.GetHoroscopePrediction(timeSlice);

                    //select only rising sign
                    var selected = allPredictions.Where(x => x.FormattedName.Contains("Rising")).FirstOrDefault();

                    //nicely packed
                    var named = new JProperty(timeSlice.ToString(), selected.ToString());
                    compiledObj.Add(named);

                }


                //send image back to caller
                return APITools.PassMessageJson(compiledObj, incomingRequest);


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



    }
}
