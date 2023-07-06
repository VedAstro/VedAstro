using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using System.Xml.Linq;
using VedAstro.Library;
using System.Net.Mime;
using System.Net;
using Azure.Storage.Blobs;
using System;
using System.Linq;
using Microsoft.Bing.ImageSearch;
using Microsoft.Bing.ImageSearch.Models;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Routing;
using Person = VedAstro.Library.Person;

namespace API
{
    /// <summary>
    /// Group of API calls related to finding birth based on dictionary attack on time and other methods
    /// </summary>
    public static class BirthTimeFinderAPI
    {

        //CENTRAL FOR ROUTES
        private const string RouteFindBirthTimeAnimal = "FindBirthTimeAnimal/PersonId/{personId}";
        private const string startTime = "{hhmmStart}/{dateStart}/{monthStart}/{yearStart}/{offsetStart}";
        private const string endTime = "{hhmmEnd}/{dateEnd}/{monthEnd}/{yearEnd}/{offsetEnd}";
        private const string RouteFindBirthTimeRisingSign = $"FindBirthTimeRisingSign/Location/{{locationName}}/StartTime/{startTime}/EndTime/{endTime}";
        private const string RouteFindBirthTimeRisingSignPerson = "FindBirthTimeRisingSign/PersonId/{personId}";

        [Function(nameof(FindBirthTimeAnimal))]
        public static async Task<HttpResponseData> FindBirthTimeAnimal([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = RouteFindBirthTimeAnimal)] HttpRequestData incomingRequest, string personId)
        {

            try
            {
                //get person record
                var foundPerson = await APITools.GetPersonById(personId);
                
                //get list of possible birth time slice in the current birth day
                var timeSlices = Get24TimeSlices(foundPerson);

                //get predictions for each slice and place in out going list  
                var compiledObj = new JObject();
                foreach (var timeSlice in timeSlices)
                {
                    //replace original birth time
                    var personAdjusted = foundPerson.ChangeBirthTime(timeSlice);
                    
                    //get the animal prediction for possible birth time
                    var newBirthConstellation = AstronomicalCalculator.GetMoonConstellation(personAdjusted.BirthTime).GetConstellationName();
                    var animal = AstronomicalCalculator.GetAnimal(newBirthConstellation);

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
                await APILogger.Error(e);
                var response = incomingRequest.CreateResponse(HttpStatusCode.OK);
                response.Headers.Add("Call-Status", "Fail"); //caller checks this
                response.Headers.Add("Access-Control-Expose-Headers", "Call-Status"); //needed by silly browser to read call-status
                return response;
            }

        }

        [Function(nameof(FindBirthTimeDasa))]
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
                var dasaEventTags = new List<EventTag> { EventTag.PD1, EventTag.PD2, EventTag.PD3, EventTag.PD4, EventTag.PD5, EventTag.PD6, EventTag.PD7, EventTag.PD8, EventTag.Gochara };
                //PRECISION
                //calculate based on max screen width,
                //done for fast calculation only for needed viewability
                var timeRange = new TimeRange(startTime, endTime);
                var daysPerPixel = EventsChart.GetDayPerPixel(timeRange, 800);


                var combinedSvg = "";
                var chartYPosition = 30; //start with top padding
                var leftPadding = 10;
                foreach (var possibleTime in possibleTimeList)
                {
                    //replace original birth time
                    var personAdjusted = person.ChangeBirthTime(possibleTime);
                    var newChart = await EventsChartAPI.GenerateNewChart(personAdjusted, timeRange, daysPerPixel, dasaEventTags);
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

        /// <summary>
        /// Finds in same same day of birth, for quick & easy search
        /// </summary>
        [Function(nameof(FindBirthTimeRisingSignEasy))]
        public static async Task<HttpResponseData> FindBirthTimeRisingSignEasy([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = RouteFindBirthTimeRisingSignPerson)] HttpRequestData incomingRequest, string personId)
        {
            try
            {
                //get person record
                var foundPerson = await APITools.GetPersonById(personId);

                //get list of possible birth time slice in the current birth day
                var timeSlices = Get24TimeSlices(foundPerson);

                //get predictions for each slice and place in out going list  
                var compiledObj = new JObject();
                foreach (var timeSlice in timeSlices)
                {
                    //get all predictions for person
                    var allPredictions = await Tools.GetHoroscopePrediction(timeSlice, APITools.HoroscopeDataListFile);
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
                await APILogger.Error(e, incomingRequest);
                //format error nicely to show user
                return APITools.FailMessage(e, incomingRequest);
            }
        }


        [Function(nameof(FindBirthTimeRisingSign))]
        public static async Task<HttpResponseData> FindBirthTimeRisingSign([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = RouteFindBirthTimeRisingSign)] HttpRequestData incomingRequest,
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
                var startTime = await APITools.ParseTime(locationName, hhmmStart, dateStart, monthStart, yearStart, offsetStart);
                var endTime = await APITools.ParseTime(locationName, hhmmEnd, dateEnd, monthEnd, yearEnd, offsetEnd);


                //get list of possible birth time slice in the current birth day
                var timeSlices = Time.GetTimeListFromRange(startTime, endTime, 1);

                //get predictions for each slice and place in out going list  
                var compiledObj = new JObject();
                foreach (var timeSlice in timeSlices)
                {

                    //get all predictions for person
                    var allPredictions = await Tools.GetHoroscopePrediction(timeSlice, APITools.HoroscopeDataListFile);
                    
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
                await APILogger.Error(e, incomingRequest);
                //format error nicely to show user
                return APITools.FailMessage(e, incomingRequest);
            }
        }




        //█▀█ █▀█ █ █░█ ▄▀█ ▀█▀ █▀▀   █▀▄▀█ █▀▀ ▀█▀ █░█ █▀█ █▀▄ █▀
        //█▀▀ █▀▄ █ ▀▄▀ █▀█ ░█░ ██▄   █░▀░█ ██▄ ░█░ █▀█ █▄█ █▄▀ ▄█

        /// <summary>
        /// split a person's day into 24 slices of possible birth times
        /// </summary>
        private static List<Time> Get24TimeSlices(Person person)
        {
            //start of day till end of day
            var dayStart = new Time($"00:00 {person.BirthDateMonthYear} {person.BirthTimeZone}", person.GetBirthLocation());
            var dayEnd = new Time($"23:59 {person.BirthDateMonthYear} {person.BirthTimeZone}", person.GetBirthLocation());

            var finalList = Time.GetTimeListFromRange(dayStart, dayEnd, 1);

            return finalList;
        }




    }
}
