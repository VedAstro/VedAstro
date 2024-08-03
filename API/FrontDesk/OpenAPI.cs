using VedAstro.Library;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Newtonsoft.Json.Linq;
using System.Reflection;
using Azure;
using ScottPlot.Palettes;


namespace API
{
    public static class OpenAPI
    {

        //.../Calculate/Karana/Location/Singapore/Time/23:59/31/12/2000/+08:00
        private const string CalculateRoute = $"{nameof(Calculate)}/{{calculatorName}}/{{*fullParamString}}"; //* that captures the rest of the URL path
        private const string ListRoute = $"{nameof(ListCalls)}"; 

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
        public static async Task<HttpResponseData> Calculate([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = CalculateRoute)] HttpRequestData incomingRequest,
        string calculatorName,
        string fullParamString
        )
        {
            try
            {
                //make caller data global to all children calling HTTP
                CurrentCallerData.originalHttpRequest = incomingRequest;

                //0 : SET LOCAL HOST : used when making same API server sub calls  "http://localhost:7071/api"
                VedAstro.Library.Calculate.CurrentServerAddress = incomingRequest.ExtractHostAddress()+"/api";

                //0 : LOG CALL : used later for throttle limit
                ApiStatistic.Log(incomingRequest); //logger

                //1 : extract out custom format else empty string (removed from url)
                var format = ParseAndGetFormat(fullParamString);

                //process call smartly
                var rawProcessedData = await HandleOpenAPICalls(calculatorName, fullParamString);

                // Control API overload
                //await APITools.AutoControlOpenAPIOverload(callLog);

                // Send data to the caller
                // Some calculators return SVG, so need to send to caller directly without "Payload" wrapper
                switch (calculatorName)
                {
                    // Handle SVG string
                    case nameof(VedAstro.Library.Calculate.SkyChart):
                    case nameof(VedAstro.Library.Calculate.SouthIndianChart):
                    case nameof(VedAstro.Library.Calculate.NorthIndianChart):
                        //send direct as raw SVG image
                        return Tools.SendFileToCaller(System.Text.Encoding.UTF8.GetBytes((string)rawProcessedData), incomingRequest, "image/svg+xml");
                    // CSV string
                    case nameof(VedAstro.Library.Calculate.GenerateTimeListCSV):
                        //send direct as raw CSV file
                        return Tools.SendFileToCaller(System.Text.Encoding.UTF8.GetBytes((string)rawProcessedData), incomingRequest, "text/csv");
                    default:
                        return APITools.SendAnyToCaller(format, calculatorName, rawProcessedData, incomingRequest);
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
            // List of keywords
            //NOTE: the type name in front "/All" is needed to detect properly
            //      also needed is the backslash at the end, without which it could detect methods instead
            var allCallKeywords = new[] { "PlanetName/All/", "PlanetName/All9WithUpagrahas/", "HouseName/All/" };

            // Check if any keyword is present in the fullParamString
            var isAllCall = allCallKeywords.Any(call => fullParamString?.Contains(call) ?? false); //default to false

            // Find the keyword that was used
            var usedKeyword = allCallKeywords.FirstOrDefault(call => fullParamString?.Contains(call) ?? false); //default to false

            dynamic rawPlanetData;

            // If it's an 'All' call
            if (isAllCall)
            {
                // Generate new list of URL with the Planet name or house name changed from All to Sun, House1
                var broken = usedKeyword.Split('/'); //here we remove the front part because not needed
                var callList = GenerateCallList(fullParamString, broken[1]);

                // Make new calculation for all planets or houses 
                rawPlanetData = ProcessAllCalls(calculatorName, callList);
            }
            // If it's a single/normal call
            else
            {
                rawPlanetData = await SingleAPICallData(calculatorName, fullParamString);
            }

            return rawPlanetData;
        }

        /// <summary>
        /// generate new list of URL with the Planet name or house name changed from All to Sun, House1
        /// </summary>
        private static Dictionary<dynamic, string> GenerateCallList(string fullParamString, string allUrlKeyWord)
        {
            var splitParamString = fullParamString.Split('/');
            var allTypeNameLocation = Array.IndexOf(splitParamString, allUrlKeyWord) - 1;
            var typeName = splitParamString[allTypeNameLocation];
            var callList = new Dictionary<dynamic, string>();

            //based on planet or house process accordingly
            //todo future add Zodiac sign, constellation,....
            switch (typeName)
            {
                case nameof(PlanetName):
                    if (fullParamString.Contains("/All9WithUpagrahas/"))
                    {
                        foreach (var planet in PlanetName.All9WithUpagrahas)
                        {
                            var newUrl = fullParamString.Replace(allUrlKeyWord, $"{planet}");
                            callList.Add(planet, newUrl);
                        }
                    }
                    else
                    {
                        foreach (var planet in PlanetName.All9Planets)
                        {
                            var newUrl = fullParamString.Replace(allUrlKeyWord, $"{planet}");
                            callList.Add(planet, newUrl);
                        }
                    }
                    break;
                case nameof(HouseName):
                    foreach (var house in House.AllHouses)
                    {
                        var newUrl = fullParamString.Replace(allUrlKeyWord, $"{house}");
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
        private static Dictionary<string, dynamic> ProcessAllCalls(string calculatorName, Dictionary<dynamic, string> callList)
        {
            var rawPlanetData = new Dictionary<string, dynamic>();
            foreach (var callUrl in callList)
            {
                var variedDataName = callUrl.Key.ToString();
                var variedURL = callUrl.Value;

                //do heavy compute to get raw data
                var rawData = SingleAPICallData(calculatorName, variedURL).Result;

                //get method info of the called method and get it's
                var ccc = Tools.MethodNameToMethodInfo(calculatorName, new[] { typeof(Calculate) });
                var returnType = ccc.ReturnType;

                // Cast rawData to its underlying type here
                var castedData = Convert.ChangeType(rawData, returnType); // Use Convert.ChangeType

                rawPlanetData.Add(variedDataName, castedData);
            }
            return rawPlanetData;
        }


        //private static async Task<JArray> ProcessAllCalls(string calculatorName, Dictionary<dynamic, string> callList)
        //{
        //    var rawPlanetData = new JArray();
        //    //TODO can be made PARALLEL for speed, but disordered data
        //    foreach (var callUrl in callList)
        //    {
        //        var variedDataName = callUrl.Key.ToString();
        //        var variedURL = callUrl.Value;

        //        //do heavy compute to get raw data
        //        var rawData = await SingleAPICallData(calculatorName, variedURL);

        //        var jProperty = Tools.AnyToJSON(variedDataName, rawData);
        //        JObject obj = new JObject();
        //        obj.Add(jProperty);
        //        rawPlanetData.Add(obj);
        //    }
        //    return rawPlanetData;
        //}


        /// <summary>
        /// Main method that handles all API calls, be it SINGLE or ALL
        /// </summary>
        private static async Task<dynamic> SingleAPICallData(string calculatorName, string fullParamString)
        {
            //1 : GET INPUT DATA
            var calculator =
                Tools.MethodNameToMethodInfo(calculatorName,
                    new[] { typeof(Calculate), /*typeof(CalculateKP)*/ }); //get calculator name

            //2 : CUSTOM AYANAMSA (removes ayanamsa once read)
            fullParamString = ParseAndSetAyanamsa(fullParamString);

            //3: GET INPUTED PARAMETERS
            var rawOut = await ParseUrlParameters(fullParamString, calculator);
            var parsedParamList = rawOut.Item1;
            var remainderParamString = rawOut.Item2; //remainder of chopped string


            //4 : EXECUTE COMMAND
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
            ApiStatistic.Log(incomingRequest); //logger

            // Control API overload, even this if hit hard can COST Money via CDN
            //await APITools.AutoControlOpenAPIOverload(callLog);

            var message = "Invalid or Outdated Call, please rebuild API URL at vedastro.org/APIBuilder";
            return APITools.FailMessageJson(message, incomingRequest);
        }



        //-----------------------------------------------PRIVATE-----------------------------------------------


        /// <summary>
        /// takes out Ayanamsa from URL and returns remainder of URL
        /// allows /Ayanamsa/Raman to be used anywhere in URL
        /// </summary>
        public static string ParseAndSetAyanamsa(string? fullParamString)
        {
            //if url contains word "ayanamsa" than process it
            var isCustomAyanamsa = fullParamString?.Contains(nameof(Ayanamsa)) ?? false;  //default to false
            if (isCustomAyanamsa)
            {
                //scan URL and take out ayanamsa and set it
                var splitParamString = fullParamString?.Split('/') ?? Array.Empty<string>();
                var ayanamsaLocation = Array.IndexOf(splitParamString, nameof(Ayanamsa));
                var ayanamsaUrl = $"/{splitParamString[ayanamsaLocation]}/{splitParamString[ayanamsaLocation + 1]}";

                //set ayanamsa
                VedAstro.Library.Calculate.Ayanamsa = (int)Tools.EnumFromUrl(ayanamsaUrl);

                //remove ayanamsa from URL
                fullParamString = fullParamString?.Replace(ayanamsaUrl, "");

                return fullParamString ?? "";
            }

            //if no ayanamsa, then return as is
            else
            {
                return fullParamString ?? "";
            }
        }

        /// <summary>
        /// To detect if api caller wants image instead
        /// </summary>
        public static string ParseAndGetFormat(string fullParamString)
        {
            try
            {
                //if url contains word "ayanamsa" than process it
                var isCustomFormat = fullParamString.Contains("Format/");
                if (isCustomFormat)
                {
                    //scan URL and take out ayanamsa and set it
                    var splitParamString = fullParamString.Split('/');
                    var formatLocation = Array.IndexOf(splitParamString, "Format");
                    var formatValue = splitParamString[formatLocation + 1];

                    return formatValue;
                }

                //if no ayanamsa, then return as is
                else
                {
                    return "";
                }

            }
            catch (Exception e)
            {
                return "";
            }
        }


        /// <summary>
        /// Reads URL data to instances
        /// returns parsed list and remained of chopped up URL for enum processing
        /// </summary>
        private static async Task<(List<dynamic>, string)> ParseUrlParameters(string fullParamString, MethodInfo calculator)
        {
            //Based on the calculator method we prepare to cut the string into parameters as text

            //STAGE 1 : HANDLE COMPULSORY PARAMETERS (has to be in sequence)
            //get all compulsory parameters needed for the calculator to execute
            var allNeededParams = calculator.GetParameters().Where(p => !p.HasDefaultValue).Select(p => p.ParameterType).ToList();

            //place to store ready params
            var parsedInputParamList = new List<dynamic>(); //exact number as specified (performance)
                                                            //cut the string based on parameter type
                                                            //process 1 parameter at a time from start
            foreach (var parameterType in allNeededParams)
            {
                var parsedParam = await ParseUrlParameterByType(parameterType);

                //add to main list
                parsedInputParamList.Add(parsedParam);
            }


            //STAGE 2 : HANDLE OPTIONAL PARAMETERS (can be in any order)
            var optionalParsedInputParamList = new Dictionary<string, dynamic>();

            //only continue if param string still has data
            if (!IsNoURLDataLeft(fullParamString))
            {
                //break down the optional params
                //NOTE: all optional params are expected to be only in 1 format "ParamName/ParamValue"
                var brokenParams = fullParamString.Split('/');

                //time to parse for each optional param
                //NOTE: this allows wrong optional params to be ignored, for only what is needed is checked
                //get all optional parameters
                var allOptionalParams = calculator.GetParameters().Where(p => p.HasDefaultValue).Select(p => p).ToList();
                foreach (var optionalParam in allOptionalParams)
                {
                    var lookingForName = optionalParam.Name.ToLower();

                    //get index of param name that matches
                    var foundIndex = brokenParams.ToList().FindIndex(p => p.ToLower() == lookingForName);

                    //if none found, continue to next
                    if (foundIndex == -1) { continue; }

                    //get value of param name that matches 
                    var valueToParse = brokenParams[foundIndex + 1]; //always next, hence plus 1

                    //get the correct parser (can be for double, int or string)
                    var parser = GetParser(optionalParam.ParameterType);

                    //parse the data
                    var parsedData = parser(valueToParse);

                    //add to list
                    optionalParsedInputParamList.Add(optionalParam.Name, parsedData);
                }
            }

            //get all params to check if any optional params left
            var allParams = calculator.GetParameters().Select(p => p).ToList();

            //rebuild the params order
            foreach (var param in allParams)
            {
                //if mandatory param, skip it since already processed & placed in order
                if (!param.IsOptional) { continue; }

                //since optional param, check if any was inputed
                var foundOptionalParam = FindOptionalParam(param);

                var isInputed = foundOptionalParam != null;
                if (isInputed) { parsedInputParamList.Add(foundOptionalParam); }

                //else if none was inputed, then use default value
                else { parsedInputParamList.Add(param.DefaultValue); }

            }


            //inputed params is complete, send as is
            return (parsedInputParamList, fullParamString);


            //-------LOCAL FUNCTIONS-------

            //find optional param from parsed list (based on name)
            dynamic FindOptionalParam(ParameterInfo parameterInfo)
            {
                //get param name to compare
                var neededParamName = parameterInfo.Name.ToLower();

                //check if 1 name is found in other and vice versa
                foreach (var parsedParam in optionalParsedInputParamList)
                {
                    var isFound = parsedParam.Key.ToLower() == neededParamName;

                    //once found, return it
                    if (isFound) { return parsedParam.Value; }
                }

                //if control comes here, then no match found
                return null;
            }

            //parse data in URL and modifies the full URL string
            //note: placed inside to use same fullParamString 
            async Task<dynamic> ParseUrlParameterByType(Type parameterType)
            {
                //get inches to cut based on Type of cloth (ask the "cloth" aka type)
                var cutCount = OpenAPI.GetInchesToCutClothBasedOnType(parameterType);

                //cut out the string that contains data of the parameter (URL version of Time, PlanetName, etc.)
                var extractedUrl = Tools.CutOutString(fullParamString, cutCount);

                //keep unused param string for next parsing
                fullParamString = Tools.CutRemoveString(fullParamString, cutCount);

                //convert URL to understandable data (magic!)
                var nameOfMethod = nameof(IFromUrl.FromUrl);
                var parsedParamInstance = parameterType.GetMethod(nameOfMethod);

                //if not found then probably special types, like Enum & string, so use special Enum converter
                dynamic parsedParam;
                if (parsedParamInstance == null)
                {
                    try
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
                            //parsedParamInstance = typeof(Tools).GetMethod(nameof(Tools.EnumFromUrl), BindingFlags.Public | BindingFlags.Static);

                            //execute param parser
                            parsedParam = Tools.EnumFromUrl(extractedUrl, parameterType); //pass in extracted URL
                        }
                        //ENUM
                        else if (parameterType == typeof(int))
                        {
                            //get the parser
                            parsedParamInstance = typeof(Tools).GetMethod(nameof(Tools.IntFromUrl), BindingFlags.Public | BindingFlags.Static);

                            //execute param parser
                            parsedParam = parsedParamInstance.Invoke(null, new object[] { extractedUrl }); //pass in extracted URL
                        }
                        //DOUBLE
                        else if (parameterType == typeof(double))
                        {
                            //get the parser
                            parsedParamInstance = typeof(Tools).GetMethod(nameof(Tools.DoubleFromUrl), BindingFlags.Public | BindingFlags.Static);

                            //execute param parser
                            parsedParam = parsedParamInstance.Invoke(null, new object[] { extractedUrl }); //pass in extracted URL
                        }

                        //DATE TIME OFFSET
                        //NOTE: cut count set in 
                        else if (parameterType == typeof(DateTimeOffset))
                        {
                            //get the parser
                            parsedParamInstance = typeof(Tools).GetMethod(nameof(Tools.DateTimeOffsetFromUrl), BindingFlags.Public | BindingFlags.Static);

                            //execute param parser
                            parsedParam = parsedParamInstance.Invoke(null, new object[] { extractedUrl }); //pass in extracted URL
                        }

                        //UNPREPARED TYPES
                        else
                            throw new Exception($"Type URL Parser not implemented! {parameterType.Name}");

                    }
                    catch (Exception e)
                    {
                        throw new Exception($"Error when parsing the params you gave sweetheart! Try to regenerate latest URL from vedastro.org/APIBuilder");
                    }
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

            //checks if URL is empty or has only "//"
            bool IsNoURLDataLeft(string s) => s == "//" || string.IsNullOrEmpty(s);
        }

        /// <summary>
        /// Based on cloth like "Time", "PlanetName", "Double"
        /// </summary>
        private static int GetInchesToCutClothBasedOnType(Type parameterType)
        {
            //get inches to cut based on Type of cloth (ask the "cloth" aka type)
            var nameOfField = nameof(IFromUrl.OpenAPILength);
            FieldInfo fieldInfo = parameterType.GetField(nameOfField, BindingFlags.Public | BindingFlags.Static);

            //if cut count not found in type, then must be Native cloth, so special handle them
            if (fieldInfo == null)
            {
                //      0    1    2  3   4    5
                //.../Time/14:02/09/11/1977/+00:00
                if (parameterType == typeof(DateTimeOffset)) { return 6; }
            }

            //if the "cloth" does not speak, then we assume!
            //note: enums can't set this, so default to 2 /{EnumName}/{EnumAsString}
            var cutCount = (int)(fieldInfo?.GetValue(null) ?? 2);

            return cutCount;
        }


        /// <summary>
        /// Given a type, will return a parser for it
        /// </summary>
        private static Func<string, object> GetParser(Type type)
        {
            //handle string
            if (type == typeof(string))
            {
                return (input) => input;
            }
            //handle enum
            else if (type.IsEnum)
            {
                return (input) => Enum.Parse(type, input, true);
            }
            //handle int
            else if (type == typeof(int))
            {
                return (input) => int.Parse(input);
            }
            //handle double
            else if (type == typeof(double))
            {
                return (input) => double.Parse(input);
            }
            //handle double
            else if (type == typeof(bool))
            {
                return (input) => bool.Parse(input.ToLower());
            }
            else
            {
                throw new Exception($"Type Parser not implemented! {type.Name}");
            }
        }




    }
}
