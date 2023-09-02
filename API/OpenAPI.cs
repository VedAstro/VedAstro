using System.Net.Mime;
using VedAstro.Library;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Newtonsoft.Json.Linq;
using System.Reflection;


namespace API
{
    public static class OpenAPI
    {
        //  CALC                          TIME
        //Karana/Location/Singapore/Time/23:59/31/12/2000/+08:00
        private const string Route1 = "Calculate/{calculatorName}/{*fullParamString}"; //* that captures the rest of the URL path


        /// <summary>
        /// Main Open API method to handle all calls
        /// /.../Calculator/DistanceBetweenPlanets/PlanetName/Sun/PlanetName/Moon/Location/Singapore/Time/23:59/31/12/2000/+08:00
        /// </summary>
        [Function(nameof(Calculate))]
        public static async Task<HttpResponseData> Calculate([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = Route1)]
            HttpRequestData incomingRequest,
            string calculatorName,
            string fullParamString
            )
        {
            try
            {
                //1 : GET INPUT DATA
                var calculator = Tools.MethodNameToMethodInfo(calculatorName); //get calculator name
                var parameterTypes = calculator.GetParameters().Select(p => p.ParameterType).ToList();
				
                //get inputed parameters
				var parsedParamList = await ParseUrlParameters(fullParamString, parameterTypes);


				//2 : CUSTOM AYANAMSA
				//DON'T LIMIT FEATURES JUST BECAUSE NOT LOGGED IN
				//as such no API KEY field here, direct to Ayanamsa
				//if this is ayanamsa, then take it out to be used later...
				//var isAyanamsa = parsedParam is Ayanamsa;
				//if (isAyanamsa) { userAyanamsa = parsedParam; }//value to be used later...
				var userAyanamsa = Ayanamsa.Raman; //default
				var isAyanamsa = fullParamString.Contains(nameof(Ayanamsa));
                if (isAyanamsa)
                {
                    userAyanamsa = await Tools.EnumFromUrl(fullParamString);
                }
                AstronomicalCalculator.YearOfCoincidence = (int)userAyanamsa;


                //3 : EXECUTE COMMAND
                var rawPlanetData = calculator?.Invoke(null, parsedParamList.ToArray()); ;

                //4 : CONVERT TO JSON
                var payloadJson = Tools.AnyToJSON(calculatorName, rawPlanetData); //use calculator name as key

                //5 : SEND DATA
                return APITools.PassMessageJson(payloadJson, incomingRequest);
            }

            //if any failure, show error in payload
            catch (Exception e)
            {
                await APILogger.Error(e, incomingRequest);
                return APITools.FailMessageJson(e.Message, incomingRequest);
            }

        }

        /// <summary>
        /// Backup function to catch invalid calls, say gracefully fails
        /// </summary>
        [Function(nameof(Catch404))]
		public static async Task<HttpResponseData> Catch404([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "{*Catch404}")]
			HttpRequestData incomingRequest,
			string Catch404
		)
		{
			var message = "Invalid or Outdated Call, please rebuild API URL at vedastro.org/APIBuilder";
	        return APITools.FailMessageJson(message, incomingRequest);
		}


		/// <summary>
		/// Reads URL data to instances  
		/// </summary>
		public static async Task<List<dynamic>> ParseUrlParameters(string fullParamString, List<Type> parameterTypes)
		{
			//Based on the calculator method we prepare to cut the string into parameters as text

			//place to store ready params
			var parsedParamList = new List<dynamic>(); //exact number as specified (performance)
			//cut the string based on parameter type
			foreach (var parameterType in parameterTypes)
			{
				//get inches to cut based on Type of cloth (ask the cloth)
				var nameOfField = nameof(IFromUrl.OpenAPILength);
				FieldInfo fieldInfo = parameterType.GetField(nameOfField, BindingFlags.Public | BindingFlags.Static);
				//note: enums can't set this, so default to 2 /{EnumName}/{EnumAsString}
				var cutCount = (int)(fieldInfo?.GetValue(null) ?? 2);

				//cut out the string that contains data of the parameter (URL version of Time, PlanetName, etc.)
				var extractedUrl = Tools.CutOutString(fullParamString, cutCount);

				//keep unused param string for next parsing
				fullParamString = Tools.CutRemoveString(fullParamString, cutCount);

				//convert URL to understandable data (magic!)
				var nameOfMethod = nameof(IFromUrl.FromUrl);
				var parsedParamInstance = parameterType.GetMethod(nameOfMethod, BindingFlags.Public | BindingFlags.Static);
				//if not found then probably Enum, so use special Enum converter
				if (parsedParamInstance == null)
				{
					//var enumExtensions = $"VedAstro.Library.{parameterType.Name}Extensions, VedAstro.Library, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null";
					parsedParamInstance = typeof(Tools).GetMethod(nameof(Tools.EnumFromUrl), BindingFlags.Public | BindingFlags.Static);
				}

				//execute param parser
				Task task = (Task)parsedParamInstance.Invoke(null, new object[] { extractedUrl }); //pass in extracted URL
				await task; //getting person and location is async, so all same 
				dynamic parsedParam = task.GetType().GetProperty("Result").GetValue(task, null);

				//ACT 5:
				//add to main list IF NOT AYANAMSA (used later for main final execution)
				parsedParamList.Add(parsedParam);
			}

			return parsedParamList;
		}


        //TODO MARKED FOR DELETION  
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
