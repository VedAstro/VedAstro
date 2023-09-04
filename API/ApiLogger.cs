using System.Xml.Linq;
using Azure.Data.Tables;
using VedAstro.Library;
using Microsoft.Azure.Functions.Worker.Http;

namespace API;

/// <summary>
/// Custom simple logger for API, auto log to AppLog.xml file
/// </summary>
public static class APILogger
{
    private const string AppLogXml = "AppLog.xml";
    private const string VisitorLogXml = "VisitorLog.xml";
    private const string ContainerName = "vedastro-site-data";

    private static readonly XElement SourceXml = new("Source", "APILogger");
    private static XElement BranchXml = new XElement("Branch", ThisAssembly.Version);

    private static readonly TableClient tableClient;
    private static readonly TableServiceClient tableServiceClient;
    private static string tableName = "OpenAPILogBook";

	static APILogger()
    {
	    //todo cleanup
	    var storageUri = $"https://vedastroapistorage.table.core.windows.net/{tableName}";
	    string accountName = "vedastroapistorage";
	    string storageAccountKey = Secrets.VedAstroApiStorageKey;

	    //save reference for late use
	    tableServiceClient = new TableServiceClient(new Uri(storageUri), new TableSharedKeyCredential(accountName, storageAccountKey));
	    tableClient = tableServiceClient.GetTableClient(tableName);

    }


	//PUBLIC FUNCTIONS

	/// <summary>
	/// Logs an error directly to AppLog.xml
	/// note: request can be null
	/// </summary>
	public static async Task Error(Exception exception, HttpRequestData req = null)
    {

        //add error data to main app log file
        var visitorXml = new XElement("Visitor");

        //get data out of exception
        var errorXml = Tools.ExtractDataFromException(exception);
        visitorXml.Add(errorXml);

        //get data out of request (if specified)
        if (req != null)
        {
            var requestDataXml = await APITools.RequestToXml(req);
            visitorXml.Add(requestDataXml);
        }

        //stamp it!
        visitorXml.Add(BranchXml, SourceXml);
        visitorXml.Add(Tools.TimeStampSystemXml);
        visitorXml.Add(Tools.TimeStampServerXml);

        await Tools.AddXElementToXDocumentAzure(visitorXml, AppLogXml, ContainerName);

    }

    public static async Task Error(string message)
    {
        var visitorXml = new XElement("Visitor");

        visitorXml.Add(BranchXml, SourceXml);
        visitorXml.Add(new XElement("Error"), message);
        visitorXml.Add(Tools.TimeStampSystemXml);
        visitorXml.Add(Tools.TimeStampServerXml);

        //add error data to main app log file
        await Tools.AddXElementToXDocumentAzure(visitorXml, VisitorLogXml, ContainerName);

    }

    public static async Task Visitor(HttpRequestData req)
    {
        var visitorXml = new XElement("Visitor");

        visitorXml.Add(BranchXml, SourceXml);
        visitorXml.Add(await APITools.RequestToXml(req)); //contains IP
        visitorXml.Add(Tools.TimeStampSystemXml);
        visitorXml.Add(Tools.TimeStampServerXml);

        //add error data to main app log file
        await Tools.AddXElementToXDocumentAzure(visitorXml, VisitorLogXml, ContainerName);

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




    //PRIVATE FUNCTIONS


    /// <summary>
    /// Adds a row to open api log book, with ip address & call url
    /// </summary>
    public static OpenAPILogBookEntity OpenApiCall(HttpRequestData httpRequestData)
    {
        //var get ip address
        var ip = httpRequestData?.GetCallerIp()?.ToString() ?? "no ip";
        var url = httpRequestData?.Url.ToString() ?? "no URL";

		//set the call as running
		OpenAPILogBookEntity customerEntity = new OpenAPILogBookEntity()
        {
            //can have many IP as partition key
	        PartitionKey = ip,
	        RowKey = Tools.GenerateId(),
	        URL = url
        };

        //creates record if no exist, update if already there
        tableClient.UpsertEntity(customerEntity);

        return customerEntity;
	}
}