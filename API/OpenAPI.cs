using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using VedAstro.Library;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using SwissEphNet;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.Json.Nodes;
using Newtonsoft.Json.Linq;


namespace API
{
    public static class OpenAPI
    {


        /// <summary>
        /// https://api.vedastro.org/Location/Singapore/Time/23:59/31/12/2000/+08:00/Planet/Sun/Sign/
        /// </summary>
        [Function(nameof(OneProperty))]
        public static async Task<HttpResponseData> OneProperty([HttpTrigger(AuthorizationLevel.Anonymous,
                "get",
                Route =
                    "Location/{locationName}/Time/{hhmmStr}/{dateStr}/{monthStr}/{yearStr}/{offsetStr}/Planet/{planetNameStr}/{propertyName}")]
            HttpRequestData incomingRequest,
            string locationName,
            string hhmmStr,
            string dateStr,
            string monthStr,
            string yearStr,
            string offsetStr,
            string planetNameStr,
            string propertyName)
        {
            //log the call
            await APILogger.Visitor(incomingRequest);


            PlanetName planetName;
            var planetNameResult = PlanetName.TryParse(planetNameStr, out planetName);
            var geoLocationResult = await Tools.AddressToGeoLocation(locationName);
            var geoLocation = geoLocationResult.Payload;

            //check result 1st before parsing
            if (!planetNameResult || !geoLocationResult.IsPass) { return APITools.FailMessage("Please check your input, it failed to parse.", incomingRequest); }

            //clean time text
            var timeStr = $"{hhmmStr} {dateStr}/{monthStr}/{yearStr} {offsetStr}";
            var parsedTime = new Time(timeStr, geoLocation);


            //SWISS EPH

            if (propertyName == "SwissEphemeris")
            {
                var result = SwissEphWrapper(parsedTime, planetName);

                JObject jsonResult = JObject.FromObject(result);

                return APITools.PassMessageJson(jsonResult, incomingRequest);

            }


            //ALL PLANET DATA

            //get all calculators that can work with the inputed data
            var calculatorClass = typeof(AstronomicalCalculator);
            var calculators1 = from calculatorInfo in calculatorClass.GetMethods()
                               let parameter = calculatorInfo.GetParameters()
                               where parameter.Length == 2 //only 2 params
                                     && parameter[0].ParameterType == typeof(PlanetName)  //planet name
                                     && parameter[1].ParameterType == typeof(Time)        //birth time
                               select calculatorInfo;

            //second possible order, technically should be aligned todo
            var calculators3 = from calculatorInfo in calculatorClass.GetMethods()
                               let parameter = calculatorInfo.GetParameters()
                               where parameter.Length == 2 //only 2 params
                                     && parameter[0].ParameterType == typeof(Time)  //birth time
                                     && parameter[1].ParameterType == typeof(PlanetName)        //planet name
                               select calculatorInfo;

            //these are for calculators with static tag data
            var calculators2 = from calculatorInfo in calculatorClass.GetMethods()
                               let parameter = calculatorInfo.GetParameters()
                               where parameter.Length == 1 //only 2 params
                                     && parameter[0].ParameterType == typeof(PlanetName)  //planet name
                               select calculatorInfo;

            //place the data from all possible methods nicely in JSON
            var rootPayloadJson = new JObject();
            object[] param1 = new object[] { planetName, parsedTime };
            foreach (var methodInfo in calculators1) { addMethodInfoToJson(methodInfo, param1); }

            object[] param3 = new object[] { parsedTime, planetName };
            foreach (var methodInfo in calculators3) { addMethodInfoToJson(methodInfo, param3); }

            object[] param2 = new object[] { planetName };
            foreach (var methodInfo in calculators2) { addMethodInfoToJson(methodInfo, param2); }

            //send the payload on it's mary way
            return APITools.PassMessageJson(rootPayloadJson, incomingRequest);



            //-----------------------------

            void addMethodInfoToJson(MethodInfo methodInfo1, object[] param)
            {
                //try to get special API name for the calculator, possible not to exist
                var properApiName = methodInfo1?.GetCustomAttributes(true)?.OfType<APIAttribute>()?.FirstOrDefault()?.Name ?? "";
                //default name in-case no special
                var defaultName = methodInfo1.Name;

                //choose which name is available, prefer special
                var name = string.IsNullOrEmpty(properApiName) ? defaultName : properApiName;

                string output = "";

                //likely to fail during call, as such just ignore and move along
                try
                {
                    output = methodInfo1?.Invoke(null, param)?.ToString() ?? "";
                }
                catch (Exception e)
                {
                    output = e.Message;
                }

                //if nothing in output then, something went wrong
                output = string.IsNullOrEmpty(output) ? "#ERROR" : output;

                //save it nicely in json format
                rootPayloadJson[name] = output;
            }


        }

        private static JObject GetSignDataJson(PlanetName planetName, Time parsedTime)
        {
            var planetSign = AstronomicalCalculator.GetPlanetRasiSign(planetName, parsedTime);
            var rootJson = new JObject();
            rootJson["Name"] = planetSign.GetSignName().ToString();
            rootJson["DegreesInSign"] = planetSign.GetDegreesInSign().TotalDegrees;

            return rootJson;
        }


        /// <summary>
        /// Here comes calls that with 2 level properties like planet strength
        /// https://api.vedastro.org/Location/Singapore/Time/23:59/31/12/2000/+08:00/Planet/Sun/Strength/Temporal
        /// </summary>
        [Function(nameof(TwoProperty))]
        public static async Task<HttpResponseData> TwoProperty([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "Location/{locationName}/Time/{hhmmStr}/{dateStr}/{monthStr}/{yearStr}/{offsetStr}/Planet/{planetNameStr}/{propertyName1}/{propertyName2}")] HttpRequestData incomingRequest, string locationName, string hhmmStr, string dateStr, string monthStr, string yearStr, string offsetStr, string planetNameStr, string propertyName1, string propertyName2)
        {
            //log the call
            await APILogger.Visitor(incomingRequest);

            PlanetName planetName;
            var planetNameResult = PlanetName.TryParse(planetNameStr, out planetName);
            var geoLocationResult = await Tools.AddressToGeoLocation(locationName);
            var geoLocation = geoLocationResult.Payload;

            //check result 1st before parsing
            if (!planetNameResult || !geoLocationResult.IsPass) { return APITools.FailMessage("Please check your input, it failed to parse. Check the caps/spelling/parameter order!", incomingRequest); }

            //clean time text
            var timeStr = $"{hhmmStr} {dateStr}/{monthStr}/{yearStr} {offsetStr}";
            var parsedTime = new Time(timeStr, geoLocation);

            //based on property call the method
            var returnVal = "";
            switch (propertyName1)
            {

                //LONGITUDE
                case "Longitude":
                    {
                        switch (propertyName2)
                        {
                            case "Fixed":
                            case "Nirayana": returnVal = AstronomicalCalculator.GetPlanetNirayanaLongitude(parsedTime, planetName).ToString(); break;
                            case "Movable":
                            case "Sayana": returnVal = AstronomicalCalculator.GetPlanetSayanaLongitude(parsedTime, planetName).ToString(); break;
                        }
                        break;
                    }
                case "Strength":
                    {
                        switch (propertyName2)
                        {
                            case "Total":
                            case "ShadbalaPinda": returnVal = AstronomicalCalculator.GetPlanetShadbalaPinda(planetName, parsedTime).ToString(); break;

                            case "Positional":
                            case "Sthana": returnVal = AstronomicalCalculator.GetPlanetSthanaBala(planetName, parsedTime).ToString(); break;

                            case "Directional":
                            case "Dig": returnVal = AstronomicalCalculator.GetPlanetDigBala(planetName, parsedTime).ToString(); break;

                            case "Temporal":
                            case "Kala": returnVal = AstronomicalCalculator.GetPlanetKalaBala(planetName, parsedTime).ToString(); break;

                            case "Motional":
                            case "Chesta": returnVal = AstronomicalCalculator.GetPlanetChestaBala(planetName, parsedTime).ToString(); break;

                            case "Natural":
                            case "Naisargika": returnVal = AstronomicalCalculator.GetPlanetNaisargikaBala(planetName, parsedTime).ToString(); break;

                            case "Aspect":
                            case "Drik": returnVal = AstronomicalCalculator.GetPlanetDrikBala(planetName, parsedTime).ToString(); break;
                        }
                        break;
                    }

            }


            return APITools.PassMessageJson(returnVal, incomingRequest);
        }




        private static dynamic SwissEphWrapper(Time time, PlanetName planetName)
        {
            //Converts LMT to UTC (GMT)
            //DateTimeOffset utcDate = lmtDateTime.ToUniversalTime();

            int iflag = 2;//SwissEph.SEFLG_SWIEPH;  //+ SwissEph.SEFLG_SPEED;
            double[] results = new double[6];
            string err_msg = "";
            double jul_day_ET;
            SwissEph ephemeris = new SwissEph();

            // Convert DOB to ET
            jul_day_ET = AstronomicalCalculator.TimeToEphemerisTime(time);

            //convert planet name, compatible with Swiss Eph
            int swissPlanet = Tools.VedAstroToSwissEph(planetName);

            //Get planet long
            int ret_flag = ephemeris.swe_calc(jul_day_ET, swissPlanet, iflag, results, ref err_msg);

            //data in results at index 0 is longitude
            var sweCalcResults = new
            {
                Longitude = results[0],
                Latitude = results[1],
                DistanceAU = results[2],
                SpeedLongitude = results[3],
                SpeedLatitude = results[4],
                SpeedDistance = results[5]
            };

            return sweCalcResults;
        }

    }
}
