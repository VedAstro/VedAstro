using System.Net.Mime;
using VedAstro.Library;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Newtonsoft.Json.Linq;


namespace API
{
    public static class OpenAPI
    {
        private const string Route1 = "Location/{locationName}/Time/{hhmmStr}/{dateStr}/{monthStr}/{yearStr}/{offsetStr}/{celestialBodyType}/{celestialBodyName}/{propertyName}";
        private const string Route2 = "Location/{locationName}/Time/{hhmmStr}/{dateStr}/{monthStr}/{yearStr}/{offsetStr}/{celestialBodyType}/{celestialBodyName}";
        private const string Route3 = "Location/{locationName}/Time/{hhmmStr}/{dateStr}/{monthStr}/{yearStr}/{offsetStr}/{celestialBodyType}";


        /// <summary>
        /// https://api.vedastro.org/Location/Singapore/Time/23:59/31/12/2000/+08:00/Planet/Sun/Sign/
        /// </summary>
        [Function(nameof(Income1))]
        public static async Task<HttpResponseData> Income1([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = Route1)]
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

            //parse time range from caller (possible to fail)
            var parsedTime = await Tools.ParseTime(locationName, hhmmStr, dateStr, monthStr, yearStr, offsetStr);

            //send to sorter
            return await FrontDeskSorter(celestialBodyType, celestialBodyName, propertyName, parsedTime, incomingRequest);
        }

        [Function(nameof(Income2))]
        public static async Task<HttpResponseData> Income2([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = Route2)]
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

            //parse time range from caller (possible to fail)
            var parsedTime = await Tools.ParseTime(locationName, hhmmStr, dateStr, monthStr, yearStr, offsetStr);

            //send to sorter (no property, set null)
            return await FrontDeskSorter(celestialBodyType, celestialBodyName, null, parsedTime, incomingRequest);
        }

        [Function(nameof(Income3))]
        public static async Task<HttpResponseData> Income3([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = Route3)]
            HttpRequestData incomingRequest,
            string locationName,
            string hhmmStr,
            string dateStr,
            string monthStr,
            string yearStr,
            string offsetStr,
            string celestialBodyType)
        {
            //log the call
            APILogger.Visitor(incomingRequest);

            //parse time range from caller (possible to fail)
            var parsedTime = await Tools.ParseTime(locationName, hhmmStr, dateStr, monthStr, yearStr, offsetStr);

            //send to sorter (no property, set null)
            return await FrontDeskSorter(celestialBodyType, "", null, parsedTime, incomingRequest);
        }



        private static async Task<HttpResponseData> FrontDeskSorter(string celestialBodyType, string celestialBodyName, string propertyName, Time parsedTime,
            HttpRequestData incomingRequest)
        {

            var individualPropertySelected = !string.IsNullOrEmpty(propertyName);


            //all planet body calls
            if (celestialBodyType.ToLower() == "planet")
            {

                //if all planets
                if (celestialBodyName.ToLower() == "all")
                {

                    //compile together all the data
                    var compiledObj = new JObject();
                    var compiledAry = new JArray();
                    var isArray = true;
                    foreach (var planet in PlanetName.All9Planets)
                    {
                        var planetName = planet.Name.ToString();
                        var planetData = GetPlanetDataJSON(planet, propertyName, parsedTime);


                        //JSON format used for all planet data and selected data is different
                        //as such when building for all planets needs to be done properly to match
                        var dataType = planetData.GetType();
                        if (typeof(JObject) == dataType)
                        {
                            isArray = true; //hack for below
                            var xxx = new JObject();
                            xxx[planetName] = planetData;
                            compiledAry.Add(xxx);
                        }
                        else
                        {
                            isArray = false; //hack for below

                            //nicely packed
                            var wrapped = new JObject(planetData);
                            var named = new JProperty(planetName, wrapped);
                            compiledObj.Add(named);
                        }
                    }

                    //for ALL property
                    if (isArray) { return APITools.PassMessageJson(compiledAry, incomingRequest); }

                    //for selected property
                    else { return APITools.PassMessageJson(compiledObj, incomingRequest); }

                }


                //users selects 1 particular planet
                else
                {
                    var planetName = PlanetName.Parse(celestialBodyName);
                    var result = GetPlanetDataJSON(planetName, propertyName, parsedTime);
                    return APITools.PassMessageJson(result, incomingRequest);
                }

            }

            //all house body calls
            if (celestialBodyType.ToLower() == "house")
            {

                //if all house
                if (celestialBodyName.ToLower() == "all")
                {

                    //compile together all the data
                    var compiledObj = new JObject();
                    var compiledAry = new JArray();
                    var isArray = true;
                    foreach (var house in House.AllHouses)
                    {
                        var houseData = GetHouseDataJSON(house, propertyName, parsedTime);

                        //JSON format used for all planet data and selected data is different
                        //as such when building for all planets needs to be done properly to match
                        var dataType = houseData.GetType();
                        if (typeof(JObject) == dataType)
                        {
                            isArray = true; //hack for below
                            var xxx = new JObject();
                            xxx[house.ToString()] = houseData;
                            compiledAry.Add(xxx);
                        }
                        else
                        {
                            isArray = false; //hack for below

                            //nicely packed
                            var wrapped = new JObject(houseData);
                            var named = new JProperty(house.ToString(), wrapped);
                            compiledObj.Add(named);
                        }


                    }

                    //for ALL property
                    if (isArray) { return APITools.PassMessageJson(compiledAry, incomingRequest); }

                    //for selected property
                    else { return APITools.PassMessageJson(compiledObj, incomingRequest); }

                }

                //users selects 1 particular house
                else
                {
                    //get the planet data needed
                    Enum.TryParse<HouseName>(celestialBodyName, out var houseName);

                    var result = GetHouseDataJSON(houseName, propertyName, parsedTime);
                    return APITools.PassMessageJson(result, incomingRequest);
                }

                //-------------


            }



            //------------------------------------------------------

            //sky chart
            var skyChartWidth = 750.0;
            var skyChartHeight = 230.0;

            //NOTE: each frame is cached
            if (celestialBodyType.ToLower() == "skychart")
            {
                //create unique id based on params to recognize future calls (caching)
                var callerId = $"{parsedTime.GetHashCode()}{skyChartWidth}{skyChartHeight}";

                Func<Task<string>> generateChart = () => SkyChartManager.GenerateChart(parsedTime, skyChartWidth, skyChartHeight);

                var chart = await APITools.CacheExecuteTask(generateChart, callerId);

                return APITools.SendSvgToCaller(chart, incomingRequest);

            }

            if (celestialBodyType.ToLower() == "skychartgif")
            {
                //create unique id based on params to recognize future calls (caching)
                var callerId = $"{parsedTime.GetHashCode()}{skyChartWidth}{skyChartHeight}GIF";

                //squeeze the Sky Juice!
                var chartTask = () => SkyChartManager.GenerateChartGif(parsedTime, skyChartWidth, skyChartHeight);

                //get chart if in cache, else make and save in cache
                var chartGif = await APITools.CacheExecuteTaskOpenAPI(chartTask, callerId, MediaTypeNames.Image.Gif);



                return APITools.SendFileToCaller(chartGif, incomingRequest, MediaTypeNames.Image.Gif);

            }


            return APITools.FailMessage("End Of ThE Line", incomingRequest);

            //-----------------------------


        }

        private static JToken GetPlanetDataJSON(PlanetName planetName, string propertyName, Time parsedTime)
        {
            var individualPropertySelected = !string.IsNullOrEmpty(propertyName);

            //allows to dynamically select property that would other wise come together in list below
            if (individualPropertySelected)
            {
                //find > execute > wrap to JSON matching function property
                var result = AutoCalculator.ExecuteFunctionsJSON(propertyName, planetName, parsedTime); ;


                return result;
            }
            //else get all PROPS
            else
            {
                //all planet related data
                //get all calculators that can accept a planet name and time
                var xxx = AutoCalculator.FindAndExecuteFunctionsJSON(Category.All, planetName, parsedTime); ;

                //send the payload on it's merry way
                return xxx;
            }

        }

        private static JToken GetHouseDataJSON(HouseName houseName, string methodName, Time parsedTime)
        {
            var individualPropertySelected = !string.IsNullOrEmpty(methodName);

            //allows to dynamically select property that would other wise come together in list below
            if (individualPropertySelected)
            {
                //find > execute > wrap to JSON matching function property
                var result = AutoCalculator.ExecuteFunctionsJSON(methodName, houseName, parsedTime);

                return result;
            }
            //else get all
            else
            {
                //all house related data
                //get all calculators that can accept a house name and time
                var houseTimeCalcs = AutoCalculator.FindAndExecuteFunctionsJSON(Category.All, houseName, parsedTime);

                //send the payload on it's mary way
                return houseTimeCalcs;
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




    }
}
