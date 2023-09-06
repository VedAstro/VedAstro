using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker;
using Newtonsoft.Json.Linq;
using ScottPlot;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Azure;
using Azure.Data.Tables;
using VedAstro.Library;
using Azure.Data.Tables.Models;
using System.Collections;

namespace API
{

	/// <summary>
	/// View table data via API
	/// </summary>
	public static class TableAPI
	{

		private const string Route1 = "Analytics/{*tableName}"; //* that captures the rest of the URL path

		/// <summary>
		/// Gets log from last 30 days, and groups by IP and all time count
		/// http://localhost:7071/api/OpenAPILog/
		/// </summary>
		[Function(nameof(OpenAPILog))]
		public static HttpResponseData OpenAPILog([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = Route1)] HttpRequestData incomingRequest,
			string tableName)
		{
			//user inputs table name and user does not input table name
			if (string.IsNullOrEmpty(tableName)) //no input, show directory
			{
				var nameList = GetAllTableNames();

				//4 : CONVERT TO JSON
				var jsonArray = new JArray(nameList); //IP address | Call Count

				//5 : SEND DATA
				return APITools.PassMessageJson(jsonArray, incomingRequest);

			}
			//has input, process table
			else
			{
				//from table name get table client
				TableClient logBookClient = GetTableClientFromTableName(tableName);

				//get all IP address records in the last 30 days
				var aMomentAgo = DateTimeOffset.UtcNow.AddDays(-30);
				var allEntities = logBookClient.Query<ITableEntity>(call => call.Timestamp >= aMomentAgo).ToList();

				//get only unique addresses from last 30 days
				List<string> distinctIpAddressList = allEntities.Select(e => e.PartitionKey).Distinct().ToList();

				//create new presentation list
				var presentationList = new Dictionary<string, int>();
				foreach (var callerAddress in distinctIpAddressList)
				{
					//get all calls from full time period
					var foundCalls = logBookClient.Query<ITableEntity>(call => call.PartitionKey == callerAddress);
					presentationList[callerAddress] = foundCalls.Count();
				}

				//sort by most called to least
				var sortedList = presentationList.OrderByDescending(x => x.Value); //order by call count

				//4 : CONVERT TO JSON
				var jsonArray = new JArray(sortedList.Select(x => $"IP: {x.Key} | Count: {x.Value}")); //IP address | Call Count

				//5 : SEND DATA
				return APITools.PassMessageJson(jsonArray, incomingRequest);
			}

			

		}
		/// <summary>
		/// Gets the names of all the tables in azure storage
		/// </summary>
		public static List<string> GetAllTableNames()
		{
			string connectionString = Secrets.API_STORAGE; // Replace with your storage account's connection string
			var serviceClient = new TableServiceClient(connectionString);
			var tableResponses =  serviceClient.Query();
			var tableNames = new List<string>();
			foreach (var table in tableResponses)
			{
				tableNames.Add(table.Name);
			}
			return tableNames;
		}

		private static TableClient GetTableClientFromTableName(string tableName)
		{
			//prepare call stuff
			var tableUlr = $"https://vedastroapistorage.table.core.windows.net/{tableName}";
			string accountName = "vedastroapistorage";
			string storageAccountKey = Secrets.VedAstroApiStorageKey;

			//get connection
			var _tableServiceClient = new TableServiceClient(new Uri(tableUlr), new TableSharedKeyCredential(accountName, storageAccountKey));
			var client = _tableServiceClient.GetTableClient(tableName);

			return client;
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
		public static HttpResponseData OpenAPILog_Chart([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "OpenAPILog/Chart/SliceDays/{sliceDays}/DaysAgo/{daysAgo}")] HttpRequestData incomingRequest,
			string daysAgo, string sliceDays)
		{

			//1. GET TIME RANGE
			var daysAgoParsed = double.Parse(daysAgo); //how far back to get records
			var sliceDaysParsed = double.Parse(sliceDays); //days each bar chart represents

			//make list of time ranges to count, exp: look back 5 days from now
			var startTime = DateTimeOffset.UtcNow;
			var endTime = DateTimeOffset.UtcNow.AddDays(-daysAgoParsed);

			//list of start and end times for each bar
			var countTimeRangeList = new List<(DateTimeOffset Start, DateTimeOffset End)>();
			var cursorEnd = endTime; //start at end and slowly increment till start time
			while (cursorEnd < startTime)
			{
				//time range that will be used to query for count of a bar data
				var _endTime = cursorEnd.AddDays(sliceDaysParsed);

				//note: here switch Start as smaller coming first, than end time as larger after
				//works for searching like normal event in log book
				var countRange = (Start: cursorEnd, End: _endTime);

				//data starts from end time at 0 index
				countTimeRangeList.Add(countRange); //add to list

				//move cursor for next
				cursorEnd = _endTime;
			}

			//2. CONVERT RANGE TO DATA
			var xxList = new List<Tuple<double, int>>(); //date and count
			foreach (var time in countTimeRangeList)
			{
				//search within start and end time, all call within range
				var startX = time.Start;
				var endX = time.End;
				Expression<Func<OpenAPILogBookEntity, bool>> expression = call => call.Timestamp >= startX && call.Timestamp <= endX;
				var foundCalls = APILogger.LogBookClient.Query<OpenAPILogBookEntity>(expression);

				//date range and call count
				var daysAgoX = DateTimeOffset.UtcNow - startX; //time from now in days
				xxList.Add(new Tuple<double, int>((int)daysAgoX.TotalDays, foundCalls.Count()));
			}

			//use fancy lib to convert data to bar chart
			var byteArray = CreateBarChart(xxList);

			//send file as JPEG image to caller
			return APITools.SendFileToCaller(byteArray, incomingRequest, MediaTypeNames.Image.Jpeg);

		}



		//public static byte[] CreateBarChart(Dictionary<string, int> data)
		//{
		//	var plt = new Plot(720, 500);
		//	// Convert the dictionary to arrays for the bar chart
		//	string[] daysSince = data.Keys.ToArray();
		//	double[] values = data.Values.Select(x => (double)x).ToArray();

		//	var barPlot = plt.AddBar(values);
		//	barPlot.ShowValuesAboveBars = true;
		//	plt.XTicks(values, daysSince); // Set the X-axis ticks to be the categories

		//	plt.Title("API Usage Chart");
		//	plt.YLabel("Number of Calls");
		//	plt.XLabel("Days Since");

		//	//convert plot into JPEG image
		//	Bitmap bmp = plt.GetBitmap();
		//	using var stream = new MemoryStream();
		//	bmp.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
		//	return stream.ToArray();
		//}

		public static byte[] CreateBarChart(List<Tuple<double, int>> data)
		{
			var plt = new Plot(720, 500);
			// Convert the dictionary to arrays for the bar chart
			double[] daysSince = data.Select(t => t.Item1).ToArray();
			double[] values = data.Select(t => (double)t.Item2).ToArray();

			plt.PlotBar(daysSince, values, showValues: true);
			plt.Title("API Usage Chart");
			plt.YLabel("Number of Calls");
			plt.XLabel("Days Since");

			//convert plot into JPEG image
			Bitmap bmp = plt.GetBitmap();
			using var stream = new MemoryStream();
			bmp.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
			return stream.ToArray();
		}

	}
}
