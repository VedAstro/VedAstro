using System.Text;
using Azure;
using Azure.Data.Tables;
using VedAstro.Library;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.AspNetCore.Http;
using System.Reflection;

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
        string accountName = Secrets.Get("CentralStorageAccountName");
        string storageAccountKey = Secrets.Get("CentralStorageKey");
        var logBookUri = $"https://{accountName}.table.core.windows.net/{OpenApiLogBook}";
        var errorBookUri = $"https://{accountName}.table.core.windows.net/{OpenApiErrorBook}";

        //get connection & load tables
        var tableServiceClient = new TableServiceClient(new Uri(logBookUri), new TableSharedKeyCredential(accountName, storageAccountKey));
        LogBookClient = tableServiceClient.GetTableClient(OpenApiLogBook);

        tableServiceClient = new TableServiceClient(new Uri(errorBookUri), new TableSharedKeyCredential(accountName, storageAccountKey));
        ErrorBookClient = tableServiceClient.GetTableClient(errorBookUri);

    }


    /// <summary>
    /// Adds error log to OpenAPIErrorBook
    /// </summary>
    public static void Error(Exception exception, HttpRequestData req = null)
    {
        try
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
        catch (Exception deeperException)
        {
            //NOTE: to error on error loop, we quietly console
            //out here without stopping execution
            Console.WriteLine(exception.Message);
            Console.WriteLine(deeperException.Message);
        }

        
    }



    /// <summary>
    /// Adds error log to OpenAPIErrorBook
    /// </summary>
    public static void Error(string message)
    {
        try
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
        catch (Exception e)
        {
            Console.WriteLine("ERROR LOGGING FAILED!");
        }

    }

    /// <summary>
    /// Logs API requests. It extracts the IP address, URL, and body data from the request, and then logs this information.
    /// For binary data in the request body, it adds the data to a cache location. (note can't store binary above 64KB in table)
    /// </summary>
    //public static async Task<OpenAPILogBookEntity> Visit(HttpRequestData httpRequestData)
    //{
    //    //adds log to DB and returns saved data for checking
    //    var openApiLogBookEntity = await OpenApiLogBookEntity();

    //    return openApiLogBookEntity;

    //    async Task<OpenAPILogBookEntity> OpenApiLogBookEntity()
    //    {
    //        //var get ip address & URL and save it for future use
    //        APILogger.IpAddress = httpRequestData?.GetCallerIp()?.ToString() ?? "no ip";
    //        APILogger.URL = httpRequestData?.Url.ToString() ?? "no URL";

    //        //# extract out the body data (POST/GET/...)
    //        var streamReader = new StreamReader(httpRequestData?.Body);
    //        var payload = await streamReader?.ReadToEndAsync();
    //        var bodyData = "no Body";
    //        var generateRowKey = Tools.GenerateId(); // random so each call is logged without conflict

    //        // Check if the payload is a valid string or binary
    //        if (payload != null)
    //        {
    //            bool isBinary = payload.Any(ch => char.IsControl(ch) && ch != '\r' && ch != '\n');
    //            bodyData = isBinary ? $"Binary data stored in cache : {generateRowKey}" : payload; //link to cache
    //            if (isBinary)
    //            {
    //                var chartBytes = Encoding.UTF8.GetBytes(payload); // Convert the payload to bytes
    //                var mimeType =
    //                    "application/octet-stream"; // This is a general MIME type for binary data. Don't waste time detecting file type, just logging.
    //                //add Binary file to cache location (debugging & reference purposes)
    //                var cacheBinaryBody = $"BinaryBody_{IpAddress}_{generateRowKey}";
    //                await AzureCache.Add(cacheBinaryBody, chartBytes, mimeType);
    //            }
    //        }

    //        //make the cache row to be added
    //        var customerEntity = new OpenAPILogBookEntity()
    //        {
    //            //can have many IP as partition key
    //            PartitionKey = IpAddress,
    //            RowKey = generateRowKey,
    //            URL = URL,
    //            Body = bodyData,
    //            Timestamp = DateTimeOffset.UtcNow //utc used later to check for overload control
    //        };

    //        var requestHeaderList = httpRequestData.Headers.ToDictionary(x => x.Key, x => x.Value, StringComparer.Ordinal);

    //        for (int i = 0; i < requestHeaderList.Count; i++)
    //        {
    //            string propertyName = $"Header{i}"; //can go up to 15 exp: Header2
    //            var xxx = requestHeaderList.ElementAt(i);
    //            string newValue = Tools.ListToString(xxx.Value.ToList());

    //            PropertyInfo propertyInfo = customerEntity?.GetType()?.GetProperty(propertyName);
    //            propertyInfo?.SetValue(customerEntity, newValue);
    //        }

    //        //NOTE: control here, ramming here can raise TABLE STORAGE prices too
    //        await APITools.AutoControlOpenAPIOverload(customerEntity); //slows down if needed

    //        //creates record if no exist, update if already there
    //        LogBookClient.UpsertEntity(customerEntity);
    //        return customerEntity;
    //    }
    //}



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


    public static int GetAbuseCountWithinLastTimeperiod(string ipAddress, double timeMinute)
    {
        //get all IP address records in the last specified time period
        var aMomentAgo = DateTimeOffset.UtcNow.AddMinutes(-timeMinute);
        var linqEntities = AzureTable.APIAbuseList.Query<APIAbuseRow>(apiAbs => apiAbs.PartitionKey == ipAddress && apiAbs.Timestamp >= aMomentAgo);

        //return the number of last calls found
        return linqEntities.Count();
    }
}