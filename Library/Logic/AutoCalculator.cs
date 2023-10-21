using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json.Linq;

namespace VedAstro.Library
{

	public static class AutoCalculator
	{

		/// <summary>
		/// based on number and type of params all available methods are taken and 
		/// </summary>
		public static List<APIFunctionResult> FindAndExecuteFunctions(MethodInfo callerToExclude = null, params object[] paramInput)
		{
			//GET ALL CALCS
			var foundCalcs = FindCalcs(callerToExclude, paramInput);

			//STAGE 2: EXECUTE
			var returnList = ExecuteCals(foundCalcs, paramInput);

			//STAGE 3: HALLELUJAH
			return returnList;

		}


		/// <summary>
		/// Given a method info of already found calcs will execute it
		/// </summary>
		public static JToken ExecuteFunctionsJSON(MethodInfo methodName, params object[] paramInput)
		{
			var list = new List<MethodInfo> { methodName };

			return ExecuteFunctionsJSON(list, paramInput);
		}

		/// <summary>
		/// Given a list of already found calcs will execute it
		/// </summary>
		public static JToken ExecuteFunctionsJSON(List<MethodInfo> methodInfoList, params object[] paramInput)
		{
			//get results from executed calcs in list
			var returnList = ExecuteCals(methodInfoList, paramInput);

			//package nicely into JSON
			var parsed = APIFunctionResult.ToJsonList(returnList);

			return parsed;
		}

		public static JToken FindAndExecuteFunctionsJSON(MethodInfo callerToExclude = null , params object[] paramInput)
		{
			var raw = FindAndExecuteFunctions(callerToExclude, paramInput);

			var parsed = APIFunctionResult.ToJsonList(raw);

			return parsed;
		}


		public static List<APIFunctionResult> ExecuteCals(IEnumerable<MethodInfo> foundCalcs, params object[] paramInput)
		{
			var returnList = new List<APIFunctionResult>();
			foreach (var calc in foundCalcs)
			{
				//get the params ordered nicely to match methods
				var paramOrder = OrderParamToMatch(calc, paramInput);

				object rawResult;
				try //likely to fail, don't let one stop the train
				{
					rawResult = calc?.Invoke(null, paramOrder);
				}
				catch (Exception e)
				{
					rawResult = e; //pass in failure to make easy detect by caller
				}

				//get correct name for this method, API friendly
				var apiSpecialName = calc.Name;

				//add to main list
				var temp = new APIFunctionResult(apiSpecialName, rawResult);
				returnList.Add(temp);

			}


			return returnList;
		}

		/// <summary>
		/// given a category will find those method's info only
		/// Param input only used to get the Types names , data inside not used
		/// </summary>
		public static IEnumerable<MethodInfo> FindCalcs(MethodInfo callerToExclude = null, params object[] paramInput)
		{
			//STAGE 1: FIND
			//get the data needed to 
			var typeList = paramInput.Select(param => param.GetType()).ToList();

			//get all calculators that can work with the inputed data
			//note: does not account for param order, only count and type
			var calculatorClass = typeof(Calculate);
			var matchedCalculators =
				from calculatorInfo in calculatorClass.GetMethods()
				let parameter = calculatorInfo.GetParameters()
				where ParamListMatch(parameter, typeList) && calculatorInfo != callerToExclude
                select calculatorInfo;

			//sort alphabetically so easier to eye data point
			var aToZOrder = matchedCalculators.OrderBy(method => method.Name).ToList();


#if DEBUG
			Console.WriteLine($"FOUND CALCS :{aToZOrder.Count()}");
#endif

			return aToZOrder;

		}




		//------------------------
		
		/// <summary>
		/// Here we know the params exist, but assume that the order is out of whack
		/// so we rebuild the param order again, this will allow any and all param
		/// signatures to be accepted without a hiccup
		/// </summary>
		private static object[] OrderParamToMatch(MethodInfo calc, object[] paramInput)
		{
			var expectedParamList = calc.GetParameters();
			var returnList = new object[expectedParamList.Length]; //make same size

			//make into modifiable list so that can remove after assign
			var paramNotYetAssign = paramInput.ToList();

			for (int position = 0; position < expectedParamList.Length; position++)
			{
				//get type expected at this position
				var needType = expectedParamList[position].ParameterType;

				//find param with matching type from input list
				var foundParam = paramNotYetAssign.First(param => param.GetType() == needType);

				//place in the expected position
				returnList[position] = foundParam;

				//remove assigned param from input list,
				//this will allow methods with multiple same param types
				//this stops from accidentally reusing the same param
				//even if the param order is screwed for same param type, it should be decteable by caller
				//but theoretically it should be fine since we remove it anyway
				paramNotYetAssign.Remove(foundParam);
			}

			return returnList;
		}

		/// <summary>
		/// does not account for param order, only count and type
		/// used for finding the right method to based only on param types
		/// </summary>
		private static bool ParamListMatch(ParameterInfo[] inputParamList, List<Type> neededTypeList)
		{
			//when called both should have same number of params for sure, else no match obviously
			if (inputParamList.Length != neededTypeList.Count) { return false; }

			//convert to type list on the fly
			var inputTypeList = inputParamList.Select(p => p.ParameterType).ToList();

			//check if every param needed is accounted for
			var isFound = true; //assume all is ok at start
			foreach (var neededType in neededTypeList)
			{
				//all must be found else fail
				var found = inputTypeList.Contains(neededType);
				if (!found) { isFound = false; break; }
			}

			return isFound;
		}

		


		//----------------------------------------


		//        /// <summary>
		//        /// Given a reference to astro calculator method,
		//        /// will return it's API friendly name
		//        /// </summary>


		//        /// <summary>
		//        /// Get all methods that is available to time and planet param
		//        /// this is the lis that will appear on the fly in API Builder dropdown
		//        /// </summary>
		//        /// <returns></returns>
		//        public static IEnumerable<APICallData> GetPlanetApiCallList<T1, T2>()
		//        {
		//            //get all the same methods gotten by Open api func
		//            var calcList = GetCalculatorListByParam<T1, T2>();

		//            var finalList = new List<APICallData>();

		//            //make final list with API description
		//            //get nice API calc name, shown in builder dropdown
		//            foreach (var calc in calcList)
		//            {
		//                finalList.Add(new APICallData(GetAPISpecialName(calc), GetAPICallDescByName("")));
		//            }

		//            return finalList;
		//        }

		//        //todo needs improvement
		//        private static string GetAPICallDescByName(string calcName)
		//        {
		//            //temp
		//            return "temp no data, implement pls";
		//        }

		//        /// <summary>
		//        /// Gets calculators by param type and count
		//        /// Gets all calculated data in nice JSON with matching param signature
		//        /// used to create a dynamic API call list
		//        /// </summary>
		//        public static List<APIFunctionResult> ExecuteCalculatorByParam<T1, T2>(T1 inputedPram1, T2 inputedPram2)
		//        {
		//            //get reference to all the calculators that can be used with the inputed param types
		//            var finalList = GetCalculatorListByParam<T1, T2>();

		//            //sort alphabetically so easier to eye data point
		//            var aToZOrder = finalList.OrderBy(method => Tools.GetAPISpecialName(method)).ToList();


		//            //place the data from all possible methods nicely in JSON
		//            var rootPayloadJson = new JObject(); //each call below adds to this root
		//            object[] paramList = new object[] { inputedPram1, inputedPram2 };
		//            foreach (var methodInfo in aToZOrder)
		//            {
		//                var resultParse1 = ExecuteAPICalculator(methodInfo, paramList);
		//                //done to get JSON formatting right
		//                var resultParse2 = JToken.FromObject(resultParse1); //jprop needs to be wrapped in JToken
		//                rootPayloadJson.Add(resultParse2);
		//            }

		//            return rootPayloadJson;

		//        }

		//        public static List<APIFunctionResult> ExecuteCalculatorByParam<T1>(T1 inputedPram1)
		//        {
		//            //get reference to all the calculators that can be used with the inputed param types
		//            var finalList = GetCalculatorListByParam<T1>();

		//            //sort alphabetically so easier to eye data point
		//            var aToZOrder = finalList.OrderBy(method => Tools.GetAPISpecialName(method)).ToList();

		//            //place the data from all possible methods nicely in JSON
		//            var returnList = new List<APIFunctionResult>(); //data name and raw data
		//            object[] paramList = new object[] { inputedPram1 };
		//            foreach (var methodInfo in aToZOrder)
		//            {
		//                var resultParse1 = ExecuteAPICalculator(methodInfo, paramList);
		//                returnList.Add(resultParse1);
		//            }

		//            return returnList;

		//        }


		//        /// <summary>
		//        /// Given an API name, will find the calc and try to call and wrap it in JSON
		//        /// </summary>
		//        public static JProperty ExecuteCalculatorByApiName<T1, T2>(string methodName, T1 param1, T2 param2)
		//        {
		//            var calculatorClass = typeof(AstronomicalCalculator);
		//            var foundMethod = calculatorClass.GetMethods().Where(x => Tools.GetAPISpecialName(x) == methodName).FirstOrDefault();

		//            //place the data from all possible methods nicely in JSON
		//            var rootPayloadJson = new JObject(); //each call below adds to this root

		//            //if method not found, possible outdated API call link, end call here
		//            if (foundMethod == null)
		//            {
		//                //let caller know that method not found
		//                var msg = $"Call not found, make sure API link is latest version : {methodName} ";
		//                return new JProperty(methodName, $"ERROR:{msg}");
		//            }

		//            //pass to main function
		//            return ExecuteCalculatorByApiName(foundMethod, param1, param2);
		//        }

		//        /// <summary>
		//        /// Given an API name, will find the calc and try to call and wrap it in JSON
		//        /// </summary>
		//        public static APIFunctionResult ExecuteCalculatorByApiName<T1, T2>(MethodInfo foundMethod, T1 param1, T2 param2)
		//        {
		//            //get methods 1st param
		//            var param1Type = foundMethod.GetParameters()[0].ParameterType;
		//            object[] paramOrder1 = new object[] { param1, param2 };
		//            object[] paramOrder2 = new object[] { param2, param1 };

		//            //if first param match type, then use that
		//            var finalParamOrder = param1Type == param1.GetType() ? paramOrder1 : paramOrder2;

		//#if DEBUG
		//            //print out which order is used more, helps to clean code
		//            Console.WriteLine(param1Type == param1.GetType() ? "paramOrder1" : "paramOrder2");
		//#endif

		//            //based on what type it is we process accordingly, converts better to JSON
		//            var rawResult = foundMethod?.Invoke(null, finalParamOrder);

		//            //get correct name for this method, API friendly
		//            var apiSpecialName = Tools.GetAPISpecialName(foundMethod);

		//            var returnData = new APIFunctionResult(apiSpecialName, rawResult);

		//            return returnData;

		//        }


		//        public static APIFunctionResult ExecuteCalculatorByApiName<T1>(MethodInfo foundMethod, T1 param1)
		//        {
		//            //get methods 1st param
		//            var param1Type = foundMethod.GetParameters()[0].ParameterType;
		//            object[] paramOrder1 = new object[] { param1 };


		//            //based on what type it is we process accordingly, converts better to JSON
		//            var rawResult = foundMethod?.Invoke(null, paramOrder1);

		//            //get correct name for this method, API friendly
		//            var apiSpecialName = Tools.GetAPISpecialName(foundMethod);

		//            var returnData = new APIFunctionResult(apiSpecialName, rawResult);

		//            return returnData;
		//        }

		//        /// <summary>
		//        /// Executes all calculators for API based on input param type only
		//        /// Wraps return data in JSON
		//        /// </summary>
		//        public static APIFunctionResult ExecuteAPICalculator(MethodInfo methodInfo1, object[] param)
		//        {

		//            //likely to fail during call, as such just ignore and move along
		//            try
		//            {
		//                APIFunctionResult outputResult;
		//                //execute based on param count
		//                if (param.Length == 1)
		//                {
		//                    outputResult = ExecuteCalculatorByApiName(methodInfo1, param[0]);
		//                }
		//                else if (param.Length == 2)
		//                {
		//                    outputResult = ExecuteCalculatorByApiName(methodInfo1, param[0], param[1]);
		//                }
		//                else
		//                {
		//                    //if not filled than not accounted for
		//                    throw new Exception("END OF THE LINE!");
		//                }


		//                return outputResult;
		//            }
		//            catch (Exception e)
		//            {
		//                try
		//                {
		//#if DEBUG
		//                    Console.WriteLine($"Trying again in reverse! {methodInfo1.Name}:\n{e.Message}\n{e.StackTrace}");
		//#endif
		//                    //try again in reverse
		//                    if (param.Length == 2)
		//                    {
		//                        var outputResult3 = ExecuteCalculatorByApiName(methodInfo1, param[1], param[0]);
		//                        return outputResult3;
		//                    }

		//                    var jsonPacked = new JProperty(methodInfo1.Name, $"ERROR: {e.Message}");
		//                    return jsonPacked;

		//                }
		//                //if fail put error in data for easy detection
		//                catch (Exception e2)
		//                {
		//                    //save it nicely in json format
		//                    var jsonPacked = new JProperty(methodInfo1.Name, $"ERROR: {e2.Message}");
		//                    return jsonPacked;
		//                }
		//            }
		//        }

		//        /// <summary>
		//        /// Gets all methods in Astronomical calculator that has the pram types inputed
		//        /// Note : also gets when order is reversed
		//        /// </summary>
		//        public static IEnumerable<MethodInfo> GetCalculatorListByParam<T1, T2>()
		//        {
		//            var inputedParamType1 = typeof(T1);
		//            var inputedParamType2 = typeof(T2);

		//            //get all calculators that can work with the inputed data
		//            var calculatorClass = typeof(AstronomicalCalculator);

		//            var finalList = new List<MethodInfo>();

		//            var calculators1 = from calculatorInfo in calculatorClass.GetMethods()
		//                               let parameter = calculatorInfo.GetParameters()
		//                               where parameter.Length == 2 //only 2 params
		//                                     && parameter[0].ParameterType == inputedParamType1
		//                                     && parameter[1].ParameterType == inputedParamType2
		//                               select calculatorInfo;

		//            finalList.AddRange(calculators1);

		//            //reverse order
		//            //second possible order, technically should be aligned todo
		//            var calculators2 = from calculatorInfo in calculatorClass.GetMethods()
		//                               let parameter = calculatorInfo.GetParameters()
		//                               where parameter.Length == 2 //only 2 params
		//                                     && parameter[0].ParameterType == inputedParamType2
		//                                     && parameter[1].ParameterType == inputedParamType1
		//                               select calculatorInfo;

		//            finalList.AddRange(calculators2);

		//#if true
		//            //PRINT DEBUG DATA
		//            Console.WriteLine($"Calculators Type 1 : {calculators1?.Count()}");
		//            Console.WriteLine($"Calculators Type 2 : {calculators2?.Count()}");
		//#endif

		//            return finalList;
		//        }


		//        public static IEnumerable<MethodInfo> GetCalculatorListByParam<T1>()
		//        {
		//            var inputedParamType1 = typeof(T1);

		//            //get all calculators that can work with the inputed data
		//            var calculatorClass = typeof(AstronomicalCalculator);

		//            var finalList = new List<MethodInfo>();

		//            var calculators1 = from calculatorInfo in calculatorClass.GetMethods()
		//                               let parameter = calculatorInfo.GetParameters()
		//                               where parameter.Length == 1 //only 2 params
		//                                     && parameter[0].ParameterType == inputedParamType1
		//                               select calculatorInfo;

		//            finalList.AddRange(calculators1);


		//#if true
		//            //PRINT DEBUG DATA
		//            Console.WriteLine($"Calculators with 1 param : {calculators1?.Count()}");
		//#endif

		//            return finalList;
		//        }


	}
}
