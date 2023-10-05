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
                //0 : LOG CALL : used later for throttle limit
                var callLog = await APILogger.Visit(incomingRequest);

                //1 : GET INPUT DATA
                var calculator = Tools.MethodNameToMethodInfo(calculatorName); //get calculator name
                var parameterTypes = calculator.GetParameters().Select(p => p.ParameterType).ToList();
				
                //get inputed parameters
				var rawOut = await ParseUrlParameters(fullParamString, parameterTypes);
                var parsedParamList = rawOut.Item1;
                var remainderParamString = rawOut.Item2; //remainder of chopped string

                //2 : CUSTOM AYANAMSA
                //DON'T LIMIT FEATURES JUST BECAUSE NOT LOGGED IN
                //as such no API KEY field here, direct to Ayanamsa
                //if this is ayanamsa, then take it out to be used later...
                var userAyanamsa = Ayanamsa.Raman; //default
				var isCustomAyanamsa = fullParamString.Contains(nameof(Ayanamsa));
                if (isCustomAyanamsa)
                {
                    userAyanamsa = await Tools.EnumFromUrl(remainderParamString);
                }
                VedAstro.Library.Calculate.YearOfCoincidence = (int)userAyanamsa;


				//3 : EXECUTE COMMAND
				object rawPlanetData;
                //when calculator return an async result
				if (calculator.ReturnType.IsGenericType && calculator.ReturnType.GetGenericTypeDefinition() == typeof(Task<>))
				{
					dynamic task = calculator.Invoke(null, parsedParamList.ToArray());
					await task;
					rawPlanetData = task.Result;
				}
				//when calculator return a normal result
				else
				{
					rawPlanetData = calculator?.Invoke(null, parsedParamList.ToArray());
				}

				//4 : OVERLOAD LIMIT
				await APITools.AutoControlOpenAPIOverload(callLog);

                //5 : SEND DATA TO CALLER
                return APITools.SendAnyToCaller(calculatorName, rawPlanetData,incomingRequest);

			}

			//if any failure, show error in payload
			catch (Exception e)
            {
                APILogger.Error(e, incomingRequest);
                return APITools.FailMessageJson(e.Message, incomingRequest);
            }

        }

        /// <summary>
        /// Backup function to catch invalid calls, say gracefully fails
        /// NOTE: "z" in name needed to make as last API call, else will be called all the time
        /// </summary>
        [Function(nameof(zCatch404))]
        public static async Task<HttpResponseData> zCatch404([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "{*Catch404}")]
            HttpRequestData incomingRequest,
            string Catch404
        )
        {
            //0 : LOG CALL
            //log ip address, call time and URL
            var call = APILogger.Visit(incomingRequest);

            var message = "Invalid or Outdated Call, please rebuild API URL at vedastro.org/APIBuilder";
            return APITools.FailMessageJson(message, incomingRequest);
        }



        //-----------------------------------------------PRIVATE-----------------------------------------------

        /// <summary>
        /// Reads URL data to instances
        /// returns parsed list and remained of chopped up URL for enum processing
        /// </summary>
        private static async Task<(List<dynamic>, string)> ParseUrlParameters(string fullParamString, List<Type> parameterTypes)
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
				if (parsedParamInstance == null) { parsedParamInstance = typeof(Tools).GetMethod(nameof(Tools.EnumFromUrl), BindingFlags.Public | BindingFlags.Static); }

				//execute param parser
				Task task = (Task)parsedParamInstance.Invoke(null, new object[] { extractedUrl }); //pass in extracted URL
				await task; //getting person and location is async, so all same 
				dynamic parsedParam = task.GetType().GetProperty("Result").GetValue(task, null);

				//ACT 5:
				//add to main list IF NOT AYANAMSA (used later for main final execution)
				parsedParamList.Add(parsedParam);
			}

			return (parsedParamList, fullParamString);
		}

        private static JToken GetPlanetDataJSON(PlanetName planetName, string propertyName, Time parsedTime, MethodInfo callerToExclude = null)
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
                var xxx = AutoCalculator.FindAndExecuteFunctionsJSON(callerToExclude, planetName, parsedTime); ;

                //send the payload on it's merry way
                return xxx;
            }

        }

        private static JToken GetHouseDataJSON(HouseName houseName, string methodName, Time parsedTime, MethodInfo callerToExclude = null)
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
                var houseTimeCalcs = AutoCalculator.FindAndExecuteFunctionsJSON(callerToExclude, houseName, parsedTime);

                //send the payload on it's mary way
                return houseTimeCalcs;
            }


        }

        private static JObject GetSignDataJson(PlanetName planetName, Time parsedTime)
        {
            var planetSign = VedAstro.Library.Calculate.PlanetSignName(planetName, parsedTime);
            var rootJson = new JObject();
            rootJson["Name"] = planetSign.GetSignName().ToString();
            rootJson["DegreesInSign"] = planetSign.GetDegreesInSign().TotalDegrees;

            return rootJson;
        }

    }
}
