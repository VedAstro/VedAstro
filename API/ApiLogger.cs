using System.Xml.Linq;
using Azure;
using Azure.Data.Tables;
using VedAstro.Library;
using Microsoft.Azure.Functions.Worker.Http;
using System;

namespace API;

/// <summary>
/// Custom simple logger for API, auto log to AppLog.xml file
/// </summary>
public static class APILogger
{
    private const string VisitorLogXml = "VisitorLog.xml";
    private const string ContainerName = "vedastro-site-data";

    private static readonly XElement SourceXml = new("Source", "APILogger");
    private static XElement BranchXml = new XElement("Branch", ThisAssembly.Version);

    /// <summary>
    /// Table client used for API LogBook
    /// </summary>
    public static readonly TableClient LogBookClient;
    public static readonly TableClient ErrorBookClient;
    private static readonly TableServiceClient tableServiceClient;
    private const string OpenAPILogBook = "OpenAPILogBook";
    private const string OpenAPIErrorBook = "OpenAPIErrorBook"; //place to store errors, should be cleaned regularly

    /// <summary>
    /// ip address set when visit log is made
    /// </summary>
    public static string IpAddress = "NOT SET";

    /// <summary>
    /// URL set when visit log is made
    /// </summary>
    public static string URL = "NOT SET";


	static APILogger()
    {
	    var storageUri = $"https://vedastroapistorage.table.core.windows.net/{OpenAPILogBook}";
	    string accountName = "vedastroapistorage";
	    string storageAccountKey = Secrets.VedAstroApiStorageKey;

        //get connection
	    tableServiceClient = new TableServiceClient(new Uri(storageUri), new TableSharedKeyCredential(accountName, storageAccountKey));
	    
	    //load tables
	    LogBookClient = tableServiceClient.GetTableClient(OpenAPILogBook);
	    ErrorBookClient = tableServiceClient.GetTableClient(OpenAPIErrorBook);

    }


	//PUBLIC FUNCTIONS


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
        LogBookClient.UpsertEntity(errorLog);
	}


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
		LogBookClient.UpsertEntity(errorLog);

	}


    public static async Task Data(string textData, HttpRequestData req = null)
    {

        var visitorXml = new XElement("Visitor");
        visitorXml.Add(BranchXml, SourceXml);
        visitorXml.Add(new XElement("Data"), textData);
        if (req != null) { visitorXml.Add(await APITools.RequestToXml(req)); } //only add if specified
        visitorXml.Add(Tools.TimeStampSystemXml);
        visitorXml.Add(Tools.TimeStampServerXml);

        //add error data to main app log file
        await Tools.AddXElementToXDocumentAzure(visitorXml, VisitorLogXml, ContainerName);

    }

	// NEW AZURE TABLE FUNCTIONS
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




	//PRIVATE FUNCTIONS


	/// <summary>
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