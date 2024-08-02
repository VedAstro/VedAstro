using Azure;
using Azure.Data.Tables;
using VedAstro.Library;

namespace API;

public static class CallTracker
{

    private static readonly TableServiceClient tableServiceClient;
    private static string tableName = "CallTracker";
    private static readonly TableClient tableClient;

    static CallTracker()
    {
        //todo cleanup
        var storageUri = $"https://vedastroapistorage.table.core.windows.net/{tableName}";
        string accountName = "vedastroapistorage";
        string storageAccountKey = Secrets.Get("VedAstroApiStorageKey");

        //save reference for late use
        tableServiceClient = new TableServiceClient(new Uri(storageUri), new TableSharedKeyCredential(accountName, storageAccountKey));
        tableClient = tableServiceClient.GetTableClient(tableName);

    }

    public static bool IsRunning(string callerId)
    {

        try
        {
            Pageable<CallStatusEntity> linqEntities = tableClient.Query<CallStatusEntity>(call => call.PartitionKey == callerId);

            //if old call found check if running else default false
            var found = linqEntities?.FirstOrDefault();
            var foundIsRunning = found?.IsRunning ?? false;

            return foundIsRunning;
        }
        catch (Exception e)
        {
            APILogger.Error(e); //log it

#if DEBUG
            Console.WriteLine($"FAILURE!!! : {e.Message} /n {e.StackTrace}");
#endif
            return false;
        }

    }

    /// <summary>
    /// Marks the call as running
    /// </summary>
    public static void CallStart(string callerId)
    {
        //set the call as running
        CallStatusEntity customerEntity = new CallStatusEntity()
        {
            PartitionKey = callerId,
            RowKey = "",
            IsRunning = true
        };

        //creates record if no exist, update if already there
        tableClient.UpsertEntity(customerEntity);

    }

    /// <summary>
    /// Marks the call as not running
    /// </summary>
    public static void CallEnd(string callerId)
    {
        //set the call as running
        CallStatusEntity customerEntity = new()
        {
            PartitionKey = callerId,
            RowKey = "",
            IsRunning = false //mark as done
        };

        //creates record if no exist, update if already there
        tableClient.UpsertEntity(customerEntity);
    }

}