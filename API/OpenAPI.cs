using VedAstro.Library;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using System.Collections;
using Newtonsoft.Json.Linq;
using Type = System.Type;


namespace API
{
    public static class OpenAPI
    {


        /// <summary>
        /// https://api.vedastro.org/Location/Singapore/Time/23:59/31/12/2000/+08:00/Planet/Sun/Sign/
        /// </summary>
        [Function(nameof(Income1))]
        public static async Task<HttpResponseData> Income1([HttpTrigger(AuthorizationLevel.Anonymous,
                "get",
                Route = "Location/{locationName}/Time/{hhmmStr}/{dateStr}/{monthStr}/{yearStr}/{offsetStr}/{celestialBodyType}/{celestialBodyName}/{propertyName}")]
            HttpRequestData incomingRequest,
            string locationName,
            string hhmmStr,
            string dateStr,
            string monthStr,
            string yearStr,
            string offsetStr,
            string celestialBodyType,
            string celestialBodyName,
            string propertyName)
        {
            //log the call
            APILogger.Visitor(incomingRequest);


            WebResult<GeoLocation>? geoLocationResult = await Tools.AddressToGeoLocation(locationName);
            var geoLocation = geoLocationResult.Payload;

            //clean time text
            var timeStr = $"{hhmmStr} {dateStr}/{monthStr}/{yearStr} {offsetStr}";
            var parsedTime = new Time(timeStr, geoLocation);


            //send to sorter
            return await FrontDeskSorter(celestialBodyType, celestialBodyName, propertyName, parsedTime, geoLocationResult, incomingRequest);
        }

        [Function(nameof(Income2))]
        public static async Task<HttpResponseData> Income2([HttpTrigger(AuthorizationLevel.Anonymous,
                "get",
                Route = "Location/{locationName}/Time/{hhmmStr}/{dateStr}/{monthStr}/{yearStr}/{offsetStr}/{celestialBodyType}/{celestialBodyName}")]
            HttpRequestData incomingRequest,
            string locationName,
            string hhmmStr,
            string dateStr,
            string monthStr,
            string yearStr,
            string offsetStr,
            string celestialBodyType,
            string celestialBodyName)
        {
            //log the call
            APILogger.Visitor(incomingRequest);


            WebResult<GeoLocation>? geoLocationResult = await Tools.AddressToGeoLocation(locationName);
            var geoLocation = geoLocationResult.Payload;

            //clean time text
            var timeStr = $"{hhmmStr} {dateStr}/{monthStr}/{yearStr} {offsetStr}";
            var parsedTime = new Time(timeStr, geoLocation);

            //send to sorter (no property, set null)
            return await FrontDeskSorter(celestialBodyType, celestialBodyName, null, parsedTime, geoLocationResult, incomingRequest);
        }



        private static async Task<HttpResponseData> FrontDeskSorter(string celestialBodyType, string celestialBodyName, string propertyName, Time parsedTime, WebResult<GeoLocation>? geoLocationResult,
            HttpRequestData incomingRequest)
        {

            var individualPropertySelected = !string.IsNullOrEmpty(propertyName);

            //all planet body calls
            if (celestialBodyType.ToLower() == "planet")
            {
                //get the planet data needed
                var planetNameResult = PlanetName.TryParse(celestialBodyName, out var planetName);
                //check result 1st before parsing
                if (!planetNameResult || !geoLocationResult.IsPass) { return APITools.FailMessage("Invalid Planet Name", incomingRequest); }

                //allows to dynamically select property that would other wise come together in list below
                if (individualPropertySelected)
                {
                    //find > execute > wrap to JSON matching function property
                    var result = ExecuteCalculatorByApiName<PlanetName, Time>(propertyName, planetName, parsedTime);

                    return APITools.PassMessageJson(result, incomingRequest);
                }
                //else get all
                else
                {
                    //all planet related data
                    //get all calculators that can accept a planet name and time
                    var planetTimeCalcs = Tools.GetCalcsResultsByParam<PlanetName, Time>(planetName, parsedTime);

                    //send the payload on it's mary way
                    return APITools.PassMessageJson(planetTimeCalcs, incomingRequest);
                }

            }

            //all house body calls
            if (celestialBodyType.ToLower() == "house")
            {
                //get the house data needed
                var houseName = HouseNameExtensions.FromString(celestialBodyName);

                //check result 1st before parsing
                if ((houseName == null) || !geoLocationResult.IsPass) { return APITools.FailMessage("Invalid House Name", incomingRequest); }

                //allows to dynamically select property that would other wise come together in list below
                if (individualPropertySelected)
                {
                    //find > execute > wrap to JSON matching function property
                    var result = ExecuteCalculatorByApiName<HouseName, Time>(propertyName, (HouseName)houseName, parsedTime);

                    return APITools.PassMessageJson(result, incomingRequest);
                }
                //else get all
                else
                {
                    //all house related data
                    //get all calculators that can accept a house name and time
                    var houseTimeCalcs = Tools.GetCalcsResultsByParam<HouseName, Time>((HouseName)houseName, parsedTime);

                    //send the payload on it's mary way
                    return APITools.PassMessageJson(houseTimeCalcs, incomingRequest);
                }

            }




            return APITools.FailMessage("End Of ThE Line", incomingRequest);

            //-----------------------------


        }


        /// <summary>
        /// Given an API name, will find the calc and try to call and wrap it in JSON
        /// </summary>
        public static JObject ExecuteCalculatorByApiName<T1, T2>(string methodName, T1 param1, T2 param2)
        {
            var calculatorClass = typeof(AstronomicalCalculator);
            var foundMethod = calculatorClass.GetMethods().Where(x => Tools.GetAPISpecialName(x) == methodName).FirstOrDefault();

            //place the data from all possible methods nicely in JSON
            var rootPayloadJson = new JObject(); //each call below adds to this root

            //if method not found, possible outdated API call link, end call here
            if (foundMethod == null)
            {
                //let caller know that method not found
                var msg = $"Call not found, make sure API link is latest version : {methodName} ";
                var parsed = JToken.Parse($"'{msg}'");
                rootPayloadJson[""] = parsed;
                return rootPayloadJson;
            }


            //get methods 1st param
            var param1Type = foundMethod.GetParameters()[0].ParameterType;
            object[] paramOrder1 = new object[] { param1, param2 };
            object[] paramOrder2 = new object[] { param2, param1 };

            //if first param match type, then use that
            var finalParamOrder = param1Type == param1.GetType() ? paramOrder1 : paramOrder2;

#if DEBUG
            //print out which order is used more, helps to clean code
            Console.WriteLine(param1Type == param1.GetType() ? "paramOrder1" : "paramOrder2");
#endif

            //based on what type it is we process accordingly, converts better to JSON
            var rawResult = foundMethod?.Invoke(null, finalParamOrder);

            //PROCESS LIST DIFFERENTLY
            if (rawResult is IList iList)
            {
                //convert list to comma separated string
                var parsedList = iList.Cast<object>().ToList();
                var stringComma = Tools.ListToString(parsedList);

                rootPayloadJson[methodName] = stringComma;
            }
            //normal conversion via to string
            else
            {
                rootPayloadJson[methodName] = rawResult?.ToString() ?? "";
            }


            return rootPayloadJson;
        }



        private static JObject GetSignDataJson(PlanetName planetName, Time parsedTime)
        {
            var planetSign = AstronomicalCalculator.GetPlanetRasiSign(planetName, parsedTime);
            var rootJson = new JObject();
            rootJson["Name"] = planetSign.GetSignName().ToString();
            rootJson["DegreesInSign"] = planetSign.GetDegreesInSign().TotalDegrees;

            return rootJson;
        }




    }
}
