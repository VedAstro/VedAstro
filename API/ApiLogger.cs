using VedAstro.Library;
using Microsoft.Azure.Functions.Worker.Http;

namespace API;

/// <summary>
/// Custom simple logger for API, auto log to Azure Data Table
/// </summary>
public static class APILogger
{

    /// <summary>
    /// Adds error log to OpenAPIErrorBook
    /// </summary>
    public static void Error(Exception exception, HttpRequestData incomingRequest = null)
    {
        try
        {
            //summarize exception data
            var exceptionData = Tools.ExceptionToJSON(exception).ToString(); //JSON string

            var errorLog = new OpenAPIErrorBookEntity()
            {
                PartitionKey = incomingRequest?.GetCallerIp()?.ToString() ?? "0.0.0.0",
                RowKey = DateTimeOffset.UtcNow.Ticks.ToString(),
                Branch = ThisAssembly.Version,
                URL = incomingRequest?.Url.ToString() ?? "no URL",
                Message = exceptionData
            };

            //creates record if no exist, update if already there
            AzureTable.OpenAPIErrorBook.UpsertEntity(errorLog);

        }
        catch (Exception deeperException)
        {
            //NOTE: to error on error loop, we quietly console
            //out here without stopping execution
            Console.WriteLine(exception.Message);
            Console.WriteLine(deeperException.Message);
        }

        
    }

}