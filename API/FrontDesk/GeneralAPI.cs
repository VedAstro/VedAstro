using VedAstro.Library;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Azure.Data.Tables;
using Azure;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Linq.Expressions;
using ScottPlot;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Mime;

namespace API
{
	/// <summary>
	/// All API calls with no home are here, send them somewhere you think is good
	/// </summary>
	public class GeneralAPI
	{

		[Function("gethoroscope")]
		public static async Task<HttpResponseData> GetHoroscope([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestData incomingRequest)
		{

			try
			{
				//get person from request
				var rootXml = await APITools.ExtractDataFromRequestXml(incomingRequest);
				var personId = rootXml.Value;

				var person = await VedAstro.Library.Tools.GetPersonById(personId);

				//calculate predictions for current person
				var predictionList = await VedAstro.Library.Tools.GetHoroscopePrediction(person.BirthTime, APITools.HoroscopeDataListFile);

				var sortedList = SortPredictionData(predictionList);

				//convert list to xml string in root elm
				return APITools.PassMessage(VedAstro.Library.Tools.AnyTypeToXmlList(sortedList), incomingRequest);

			}
			catch (Exception e)
			{
				//log error
				APILogger.Error(e, incomingRequest);
				//format error nicely to show user
				return APITools.FailMessage(e, incomingRequest);
			}



			List<HoroscopePrediction> SortPredictionData(List<HoroscopePrediction> horoscopePredictions)
			{
				//put rising sign at top
				horoscopePredictions.MoveToBeginning((horPre) => horPre.FormattedName.ToLower().Contains("rising"));

				//todo followed by planet in sign prediction ordered by planet strength 

				return horoscopePredictions;
			}

		}


		/// <summary>
		/// When browser visit API, they ask for FavIcon, so yeah redirect favicon from website
		/// </summary>
		[Function(nameof(FavIcon))]
		public static async Task<HttpResponseData> FavIcon([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "favicon.ico")] HttpRequestData incomingRequest)
		{
			//use same fav icon from website
			string url = "https://vedastro.org/images/favicon.ico";

			//send to caller
			using (var client = new HttpClient())
			{
				var bytes = await client.GetByteArrayAsync(url);
				var response = incomingRequest.CreateResponse(HttpStatusCode.OK);
				response.Headers.Add("Content-Type", "image/x-icon");
				await response.Body.WriteAsync(bytes, 0, bytes.Length);
				return response;
			}
		}

		/// <summary>
		/// Gets log from last 30 days, and groups by IP and all time count
		/// http://localhost:7071/api/OpenAPILog/
		/// </summary>
		[Function(nameof(OpenAPILog))]
		public static HttpResponseData OpenAPILog([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "OpenAPILog")] HttpRequestData incomingRequest)
		{
			//get all IP address records in the last 30 days
			var aMomentAgo = DateTimeOffset.UtcNow.AddDays(-30);
			var allEntities = APILogger.LogBookClient.Query<OpenAPILogBookEntity>(call => call.Timestamp >= aMomentAgo).ToList();

			//get only unique addresses from last 30 days
			List<string> distinctIpAddressList = allEntities.Select(e => e.PartitionKey).Distinct().ToList();

			//create new presentation list
			var presentationList = new Dictionary<string, int>();
			foreach (var callerAddress in distinctIpAddressList)
			{
				//get all calls from full time period
				var foundCalls = APILogger.LogBookClient.Query<OpenAPILogBookEntity>(call => call.PartitionKey == callerAddress);
				presentationList[callerAddress] = foundCalls.Count();
			}

			//sort by most called to least
			var sortedList = presentationList.OrderByDescending(x => x.Value); //order by call count

			//4 : CONVERT TO JSON
			var jsonArray = new JArray(sortedList.Select(x => $"IP: {x.Key} | Count: {x.Value}")); //IP address | Call Count

			//5 : SEND DATA
			return APITools.PassMessageJson(jsonArray, incomingRequest);

		}

		/// <summary>
		/// Shows all rows for a given IP
		/// http://localhost:7071/api/OpenAPILog/IP/212.35.2.245
		/// </summary>
		[Function(nameof(OpenAPILog_IP))]
		public static HttpResponseData OpenAPILog_IP([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "OpenAPILog/IP/{ipAddress}")] HttpRequestData incomingRequest,
			string ipAddress)
		{
			//all for IP
			var foundCalls = APILogger.LogBookClient.Query<OpenAPILogBookEntity>(call => call.PartitionKey == ipAddress);

			//create new presentation list
			var presentationList = new Dictionary<DateTimeOffset, string>();
			foreach (var callLog in foundCalls)
			{
				//format into understandable time format
				var timestamp = callLog?.Timestamp ?? DateTimeOffset.MinValue;
				var serverTime = timestamp.ToOffset(TimeSpan.FromHours(8)).ToString(Time.DateTimeFormatSeconds); //convert to +08:00 time

				//combined string
				var rowData = $"{serverTime} | {ipAddress} | {callLog.URL}";
				presentationList[timestamp] = rowData;
			}

			//sort by most called to least
			var sortedList = presentationList.OrderBy(x => x.Value); //order by time

			//4 : CONVERT TO JSON
			var jsonArray = new JArray(sortedList.Select(x => x.Value));

			//5 : SEND DATA
			return APITools.PassMessageJson(jsonArray, incomingRequest);

		}


		/// <summary>
		/// Shows all rows for a given IP
		/// http://localhost:7071/api/OpenAPILog/Chart
		/// </summary>
		[Function(nameof(OpenAPILog_Chart))]
		public static HttpResponseData OpenAPILog_Chart([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "OpenAPILog/Chart")] HttpRequestData incomingRequest,
			string ipAddress)
		{

			//1. GET TIME RANGE
			var timeGapHour = 5;

			//make list of time ranges to count
			var startTime = DateTimeOffset.UtcNow;
			var endTime = DateTimeOffset.UtcNow.AddDays(-5); //60 days 

			//list of start and end times for each bar
			var countTimeRangeList = new List<(DateTimeOffset Start, DateTimeOffset End)>();
			var cursorEnd = endTime; //start at end and slowly increment till start time
			while (cursorEnd < startTime)
			{
				//time range that will be used to query for count of a bar data
				var _endTime = cursorEnd.AddHours(timeGapHour);

				//note: here switch Start as smaller coming first, than end time as larger after
				//works for searching like normal event in log book
				var countRange = (Start: cursorEnd, End: _endTime );
				
				//data starts from end time at 0 index
				countTimeRangeList.Add(countRange); //add to list

				//move cursor for next
				cursorEnd = _endTime;
			}

			//2. CONVERT RANGE TO DATA
			var xxList = new Dictionary<double, int>();
			foreach (var time in countTimeRangeList)
			{
				//search within start and end time, all call within range
				var startX = time.Start;
				var endX = time.End;
				Expression<Func<OpenAPILogBookEntity, bool>> expression = call => call.Timestamp >= startX && call.Timestamp <= endX;
				var foundCalls = APILogger.LogBookClient.Query<OpenAPILogBookEntity>(expression);

				//date range and call count
				xxList.Add(countTimeRangeList.IndexOf(time), foundCalls.Count());
			}

			//use fancy lib to convert data to bar chart
			var byteArray = CreateBarChart(xxList);

			//send file as JPEG image to caller
			return APITools.SendFileToCaller(byteArray, incomingRequest, MediaTypeNames.Image.Jpeg);

		}





		public static byte[] CreateBarChart(Dictionary<double, int> data)
		{
			var plt = new Plot(600, 400);
			// Convert the dictionary to arrays for the bar chart
			var categories = data.Keys.ToArray();
			double[] values = data.Values.Select(x => (double)x).ToArray();
			plt.PlotBar(categories, values);
			plt.Title("My Bar Chart");
			plt.YLabel("Values");
			plt.XLabel("Categories");
			// Get the plot as a Bitmap
			Bitmap bmp = plt.GetBitmap();
			// Convert the Bitmap to a byte array
			using var stream = new MemoryStream();
			bmp.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
			return stream.ToArray();
		}

	}
}
