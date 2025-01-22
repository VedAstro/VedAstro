using VedAstro.Library;
using Microsoft.Azure.Functions.Worker.Http;

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


    static APILogger()
    {
        try
        {
            LogBookClient = APITools.GetTableClientFromTableName(OpenApiLogBook);
            ErrorBookClient = APITools.GetTableClientFromTableName(OpenApiErrorBook);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

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
            ErrorBookClient.UpsertEntity(errorLog);

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


}