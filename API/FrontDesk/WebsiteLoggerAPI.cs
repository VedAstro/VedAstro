using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker;
using VedAstro.Library;
using Azure.Data.Tables;
using Azure;

namespace API
{
    public class WebsiteLoggerAPI
    {

        /// <summary>
        /// Logs errors from website
        /// </summary>
        [Function(nameof(LogError))]
        public static async Task LogError([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "LogError")] HttpRequestData incomingRequest)
        {
            // Read error details from request body
            var errorDetails = await incomingRequest.ReadFromJsonAsync<ErrorDetails>();

            // Save error details to Azure Table Storage
            var entity = new WebsiteErrorLogEntity
            {
                PartitionKey = errorDetails.UserId,
                RowKey = errorDetails.LocalTime,
                ErrorMessage = errorDetails.Message,
                StackTrace = errorDetails.Stack,
                Url = errorDetails.Url,
                UserAgent = errorDetails.UserAgent,
                Timestamp = DateTime.UtcNow
            };
            await AzureTable.WebsiteErrorLog.AddEntityAsync(entity);
        }

        /// <summary>
        /// Logs general info from the website for debug purposes
        /// </summary>
        [Function(nameof(LogDebug))]
        public static async Task LogDebug(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "LogDebug")] HttpRequestData incomingRequest)
        {
            // Read debug details from request body
            var debugDetails = await incomingRequest.ReadFromJsonAsync<DebugDetails>();

            // Save debug details to Azure Table Storage
            var entity = new WebsiteDebugLogEntity
            {
                PartitionKey = debugDetails.UserId,
                RowKey = debugDetails.LocalTime,
                Message = debugDetails.Message,
                Url = debugDetails.Url,
                UserAgent = debugDetails.UserAgent,
                Timestamp = DateTime.UtcNow
            };
            await AzureTable.WebsiteDebugLog.AddEntityAsync(entity);
        }
    }



    // New class for DebugDetails
    public class DebugDetails
    {
        public string UserId { get; set; }
        public string LocalTime { get; set; }

        public string Url { get; set; }
        public string Message { get; set; }

        public string UserAgent { get; set; }
    }

    // New class for WebsiteDebugLogEntity
    public class WebsiteDebugLogEntity : ITableEntity
    {
        // Needed by Table
        public string PartitionKey { get; set; }

        /// <summary>
        /// Local Time
        /// </summary>
        public string RowKey { get; set; }

        /// <summary>
        /// Time of change
        /// </summary>
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
        public string Url { get; set; }
        public string UserAgent { get; set; }

        public string Message { get; set; }
    }


    public class ErrorDetails
    {
        public string UserId { get; set; }
        public string LocalTime { get; set; }
        public string Url { get; set; }
        public string Message { get; set; }
        public string Stack { get; set; }
        public string UserAgent { get; set; }
    }

    public class WebsiteErrorLogEntity : ITableEntity
    {
        //NEEDED BY TABLE
        public string PartitionKey { get; set; }

        /// <summary>
        /// Client's Local Time
        /// </summary>
        public string RowKey { get; set; }

        /// <summary>
        /// Time of change
        /// </summary>
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
        public string Url { get; set; }
        public string UserAgent { get; set; }
        public string ErrorMessage { get; set; }
        public string StackTrace { get; set; }
    }

}
