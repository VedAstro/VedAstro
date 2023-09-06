using System.Xml.Linq;
using Azure;
using Azure.Data.Tables;
using VedAstro.Library;
using Microsoft.Azure.Functions.Worker.Http;
using System;

namespace API;

/// <summary>
/// Custom simple logger for API, auto log to Azure Data Table
/// </summary>
public static class APILogger
{
    /// <summary>
    /// Table client used for API LogBook
    /// </summary>
    public static readonly TableClient LogBookClient;
    public static readonly TableClient ErrorBookClient;
    private const string OpenApiLogBook = "OpenAPILogBook";
    private const string OpenApiErrorBook = "OpenAPIErrorBook"; //place to store errors, should be cleaned regularly

    /// <summary>
    /// ip address set when visit log is made
    /// </summary>
    public static string IpAddress = "NOT SET";

    /// <summary>
    /// URL set when visit log is made
    /// </summary>
    public static string URL = "NOT SET";

    private static TableClient openApiErrorBookClient = APITools.GetTableClientFromTableName("OpenAPIErrorBook");


	static APILogger()
    {
	    var logBookUri = $"https://vedastroapistorage.table.core.windows.net/{OpenApiLogBook}";
	    var errorBookUri = $"https://vedastroapistorage.table.core.windows.net/{OpenApiErrorBook}";
	    string accountName = "vedastroapistorage";
	    string storageAccountKey = Secrets.VedAstroApiStorageKey;

		//get connection & load tables
		var _tableServiceClient = new TableServiceClient(new Uri(logBookUri), new TableSharedKeyCredential(accountName, storageAccountKey));
		LogBookClient = _tableServiceClient.GetTableClient(OpenApiLogBook);

		_tableServiceClient = new TableServiceClient(new Uri(errorBookUri), new TableSharedKeyCredential(accountName, storageAccountKey));
		ErrorBookClient = _tableServiceClient.GetTableClient(errorBookUri);

    }


	/// <summary>
	/// Adds error log to OpenAPIErrorBook
	/// </summary>
	public static void Error(Exception exception, HttpRequestData req = null)
	{
        //summarize exception data
        var exceptionData = Tools.ExceptionToJSON(exception).ToString(); //JSON string

        var errorLog = new OpenAPIErrorBookEntity()
        {
	        PartitionKey = IpAddress,
	        RowKey = DateTimeOffset.UtcNow.Ticks.ToString(),
	        Branch = ThisAssembly.Version,
	        URL = URL,
	        Message = exceptionData
        };

		//creates record if no exist, update if already there
		openApiErrorBookClient.UpsertEntity(errorLog);
	}



	/// <summary>
	/// Adds error log to OpenAPIErrorBook
	/// </summary>
	public static void Error(string message)
    {
		var errorLog = new OpenAPIErrorBookEntity()
		{
			PartitionKey = IpAddress,
			RowKey = DateTimeOffset.UtcNow.Ticks.ToString(),
			Branch = ThisAssembly.Version,
			URL = URL,
			Message = message
		};

		//creates record if no exist, update if already there
		ErrorBookClient.UpsertEntity(errorLog);

	}

	/// <summary>
	/// Adds a row to open api log book, with ip address & call url
	/// </summary>
	public static OpenAPILogBookEntity Visit(HttpRequestData httpRequestData)
	{
		//var get ip address & URL and save it for future use
		APILogger.IpAddress = httpRequestData?.GetCallerIp()?.ToString() ?? "no ip";
		APILogger.URL = httpRequestData?.Url.ToString() ?? "no URL";

		//set the call as running
		var customerEntity = new OpenAPILogBookEntity()
		{
			//can have many IP as partition key
			PartitionKey = IpAddress,
			RowKey = Tools.GenerateId(),
			URL = URL,
			Timestamp = DateTimeOffset.UtcNow //utc used later to check for overload control
		};

		//creates record if no exist, update if already there
		LogBookClient.UpsertEntity(customerEntity);

		return customerEntity;
	}


	//BELOW METHODS ARE FOR QUERYING DATA OUT

	/// <summary>
	/// from LogBookClient
	/// Given an IP address, will return number of calls made in the last specified time period
	/// </summary>
	public static int GetAllCallsWithinLastTimeperiod(string ipAddress, double timeMinute)
    {
		//get all IP address records in the last specified time period
		DateTimeOffset aMomentAgo = DateTimeOffset.UtcNow.AddMinutes(-timeMinute);
		Pageable<OpenAPILogBookEntity> linqEntities = LogBookClient.Query<OpenAPILogBookEntity>(call => call.PartitionKey == ipAddress && call.Timestamp >= aMomentAgo);

        //return the number of last calls found
		return linqEntities.Count();
    }
}