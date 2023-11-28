using VedAstro.Library;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Newtonsoft.Json.Linq;
using System.Reflection;
using Azure;


namespace API
{
    public static class OpenAPI
    {
        //.../Calculate/Karana/Location/Singapore/Time/23:59/31/12/2000/+08:00
        private const string CalculateRoute = $"{nameof(Calculate)}/{{calculatorName}}/{{*fullParamString}}"; //* that captures the rest of the URL path
        private const string ListRoute = $"{nameof(ListCalls)}"; //* that captures the rest of the URL path

        [Function(nameof(ListCalls))]
        public static async Task<HttpResponseData> ListCalls(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = ListRoute)] HttpRequestData incomingRequest
        )
        {
            var apiCallListJson = OpenAPIMetadata.FromMethodInfoList();

            return APITools.PassMessageJson(apiCallListJson, incomingRequest);
        }

        /// <summary>
        /// Main Open API method to handle all calls
        /// /.../Calculator/DistanceBetweenPlanets/PlanetName/Sun/PlanetName/Moon/Location/Singapore/Time/23:59/31/12/2000/+08:00
        /// </summary>
        [Function(nameof(Calculate))]
        public static async Task<HttpResponseData> Calculate([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = CalculateRoute)]
            HttpRequestData incomingRequest,
        string calculatorName,
        string fullParamString
        )
        {
            try
            {
                //0 : LOG CALL : used later for throttle limit
                //var callLog = await APILogger.Visit(incomingRequest);

                //process call smartly
                var rawPlanetData = await HandleOpenAPICalls(calculatorName, fullParamString);

                // Control API overload
                //await APITools.AutoControlOpenAPIOverload(callLog);

                // Send data to the caller
                // Some calculators return SVG & binary data, so need to send to caller directly
                switch (calculatorName)
                {
                    // Handle SVG string
                    case nameof(VedAstro.Library.Calculate.SkyChart):
                    case nameof(VedAstro.Library.Calculate.SouthIndianChart):
                    case nameof(VedAstro.Library.Calculate.NorthIndianChart):
                        return APITools.SendFileToCaller(System.Text.Encoding.UTF8.GetBytes((string)rawPlanetData), incomingRequest, "image/svg+xml");
                    default:
                        return APITools.SendAnyToCaller(calculatorName, rawPlanetData, incomingRequest);
                }
            }

            //if any failure, show error in payload
            catch (Exception e)
            {
                APILogger.Error(e, incomingRequest);
                return APITools.FailMessageJson(e.Message, incomingRequest);
            }

        }



        /// <summary>
        /// This code is handling API calls for either all planets or houses or for individual ones.
        /// It first checks if the call is for all planets or houses,
        /// if yes then it creates a list of calls for each planet or house and then makes the API call for each one,
        /// combines the data and adds it to rawPlanetData.
        /// If the call is for an individual planet or house, it directly makes the API call and adds
        /// the data to rawPlanetData. Finally, it sends the data to the caller.
        /// </summary>
        private static async Task<dynamic> HandleOpenAPICalls(string calculatorName, string fullParamString)
        {
            const string allKeyword = "All";
            var allCalls = new[] { "PlanetName/All/", "HouseName/All/" };
            var isAllCall = allCalls.Any(call => fullParamString.Contains(call));
            dynamic rawPlanetData;
            if (isAllCall)
            {
                //generate new list of URL with the Planet name or house name changed from All to Sun, House1
                var callList = GenerateCallList(fullParamString);

                //make new calculation for all planets or houses 
                rawPlanetData = await ProcessAllCalls(calculatorName, callList);
            }
            else
            {
                rawPlanetData = await SingleAPICallData(calculatorName, fullParamString);
            }

            return rawPlanetData;
        }

        /// <summary>
        /// generate new list of URL with the Planet name or house name changed from All to Sun, House1
        /// </summary>
        private static Dictionary<dynamic, string> GenerateCallList(string fullParamString)
        {
            var splitParamString = fullParamString.Split('/');
            var allTypeNameLocation = Array.IndexOf(splitParamString, "All") - 1;
            var typeName = splitParamString[allTypeNameLocation];
            var callList = new Dictionary<dynamic, string>();
            switch (typeName)
            {
                case nameof(PlanetName):
                    foreach (var planet in PlanetName.All9Planets)
                    {
                        var newUrl = fullParamString.Replace("/All/", $"/{planet}/");
                        callList.Add(planet, newUrl);
                    }
                    break;
                case nameof(HouseName):
                    foreach (var house in House.AllHouses)
                    {
                        var newUrl = fullParamString.Replace("/All/", $"/{house}/");
                        callList.Add(house, newUrl);
                    }
                    break;
                default:
                    throw new Exception($"Support for All with type {typeName} not built!");
            }
            return callList;
        }

        /// <summary>
        /// Make call one by one for URL provided and return combined results
        /// </summary>
        private static async Task<JArray> ProcessAllCalls(string calculatorName, Dictionary<dynamic, string> callList)
        {
            var rawPlanetData = new JArray();
            //TODO can be made PARALLEL for speed, but disordered data
            foreach (var callUrl in callList)
            {
                var variedDataName = callUrl.Key.ToString();
                var variedURL = callUrl.Value;
                var rawData = await SingleAPICallData(calculatorName, variedURL);
                var jProperty = Tools.AnyToJSON(variedDataName, rawData);
                JObject obj = new JObject();
                obj.Add(jProperty);
                rawPlanetData.Add(obj);
            }
            return rawPlanetData;
        }



        private static async Task<object?> SingleAPICallData(string calculatorName, string fullParamString)
        {
            //1 : GET INPUT DATA
            var calculator =
                Tools.MethodNameToMethodInfo(calculatorName,
                    new[] { typeof(Calculate), typeof(CalculateKP) }); //get calculator name

            //2 : CUSTOM AYANAMSA (removes ayanamsa once read)
            fullParamString = ParseAndSetAyanamsa(fullParamString);

            //get inputed parameters
            var rawOut = await ParseUrlParameters(fullParamString, calculator);
            var parsedParamList = rawOut.Item1;
            var remainderParamString = rawOut.Item2; //remainder of chopped string


            //3 : EXECUTE COMMAND
            object rawPlanetData;
            var isAsyncReturn1 = calculator.ReturnType.IsGenericType && calculator.ReturnType.GetGenericTypeDefinition() == typeof(Task<>);
            var isAsyncReturn2 = Tools.IsMethodReturnAsync(calculator);
            //when calculator return an async result
            if (isAsyncReturn1 || isAsyncReturn2)
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

            return rawPlanetData;
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
            //log ip address, call time and URL,  used later for throttle limit
            var callLog = await APILogger.Visit(incomingRequest);

            // Control API overload, even this if hit hard can COST Money via CDN
            await APITools.AutoControlOpenAPIOverload(callLog);

            var message = "Invalid or Outdated Call, please rebuild API URL at vedastro.org/APIBuilder";
            return APITools.FailMessageJson(message, incomingRequest);
        }



        //-----------------------------------------------PRIVATE-----------------------------------------------


        /// <summary>
        /// takes out Ayanamsa from URL and returns remainder of URL
        /// allows /Ayanamsa/Raman to be used anywhere in URL
        /// </summary>
        private static string ParseAndSetAyanamsa(string fullParamString)
        {
            //if url contains word "ayanamsa" than process it
            var isCustomAyanamsa = fullParamString.Contains(nameof(Ayanamsa));
            if (isCustomAyanamsa)
            {
                //scan URL and take out ayanamsa and set it
                var splitParamString = fullParamString.Split('/');
                var ayanamsaLocation = Array.IndexOf(splitParamString, nameof(Ayanamsa));
                var ayanamsaUrl = $"/{splitParamString[ayanamsaLocation]}/{splitParamString[ayanamsaLocation + 1]}";

                //set ayanamsa
                VedAstro.Library.Calculate.Ayanamsa = (int)Tools.EnumFromUrl(ayanamsaUrl);

                //remove ayanamsa from URL
                fullParamString = fullParamString.Replace(ayanamsaUrl, "");

                return fullParamString;
            }

            //if no ayanamsa, then return as is
            else
            {
                return fullParamString;
            }
        }


        /// <summary>
        /// Reads URL data to instances
        /// returns parsed list and remained of chopped up URL for enum processing
        /// </summary>
        private static async Task<(List<dynamic>, string)> ParseUrlParameters(string fullParamString, MethodInfo calculator)
        {
            //Based on the calculator method we prepare to cut the string into parameters as text
            var allNeededParams = calculator.GetParameters().Select(p => p.ParameterType).ToList();

            //place to store ready params
            var parsedInputParamList = new List<dynamic>(); //exact number as specified (performance)
                                                            //cut the string based on parameter type

            //process 1 parameter at a time from start
            foreach (var parameterType in allNeededParams)
            {
                //check if paramUrlString is empty, possible that last param is optional or invalid call
                if (fullParamString == "//" || string.IsNullOrEmpty(fullParamString)) { goto EndCall; }

                var parsedParam = await ParseUrlParameterByType(parameterType);

                //add to main list
                parsedInputParamList.Add(parsedParam);
            }



        //control comes here once all inputed from URL has been parsed
        EndCall:

            //add in optional missing parameters, since needed for final execution (only if missing save time)
            if (parsedInputParamList.Count < allNeededParams.Count)
            {
                var parameters = calculator.GetParameters();
                var args = new object[parameters.Length];
                for (int i = 0; i < parameters.Length; i++)
                {
                    //if parameter is provided, use it
                    if (i < parsedInputParamList.Count)
                    {
                        args[i] = parsedInputParamList[i];
                    }
                    //if no val provided but has default value, use that
                    else if (parameters[i].HasDefaultValue)
                    {
                        args[i] = parameters[i].DefaultValue;
                    }
                    //let caller know failed
                    else
                    {
                        throw new ArgumentException($"Parameter {parameters[i].Name} has no default value and no value was provided.");
                    }
                }


                // send full list with missing params
                return (args.ToList(), fullParamString);
            }
            
            //inputed params is complete, send as is
            else { return (parsedInputParamList, fullParamString); }



            //note: placed inside to use same fullParamString 
            async Task<dynamic> ParseUrlParameterByType(Type parameterType)
            {
                //get inches to cut based on Type of cloth (ask the cloth aka type)
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

                //if not found then probably special types, like Enum & string, so use special Enum converter
                dynamic parsedParam;
                if (parsedParamInstance == null)
                {
                    //STRING
                    if (parameterType == typeof(string))
                    {
                        parsedParamInstance = typeof(Tools).GetMethod(nameof(Tools.StringFromUrl), BindingFlags.Public | BindingFlags.Static);

                        //execute param parser
                        parsedParam = parsedParamInstance.Invoke(null, new object[] { extractedUrl }); //pass in extracted URL

                    }
                    //ENUM
                    else if (parameterType.IsEnum)
                    {
                        parsedParamInstance = typeof(Tools).GetMethod(nameof(Tools.EnumFromUrl), BindingFlags.Public | BindingFlags.Static);

                        //execute param parser
                        parsedParam = parsedParamInstance.Invoke(null, new object[] { extractedUrl }); //pass in extracted URL
                    }
                    //ENUM
                    else if (parameterType == typeof(int))
                    {
                        parsedParamInstance = typeof(Tools).GetMethod(nameof(Tools.IntFromUrl), BindingFlags.Public | BindingFlags.Static);

                        //execute param parser
                        parsedParam = parsedParamInstance.Invoke(null, new object[] { extractedUrl }); //pass in extracted URL
                    }

                    //UNPREPARED TYPES
                    else
                        throw new Exception($"Type URL Parser not implemented! {parameterType.Name}");
                }
                //if not NULL process as normal
                else
                {
                    //execute param parser
                    Task task = (Task)parsedParamInstance.Invoke(null, new object[] { extractedUrl }); //pass in extracted URL
                    await task; //getting person and location is async, so all same 
                    parsedParam = task.GetType().GetProperty("Result").GetValue(task, null);
                }

                return parsedParam;
            }

        }




    }
}
