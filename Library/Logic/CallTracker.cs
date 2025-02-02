using System;
using System.Linq;
using Azure;
using Azure.Data.Tables;
using VedAstro.Library;

namespace VedAstro.Library;

public static class CallTracker
{

    public static bool IsRunning(string callerId)
    {

        try
        {
            Pageable<CallStatusEntity> linqEntities = AzureTable.CallTracker.Query<CallStatusEntity>(call => call.PartitionKey == callerId);

            //if old call found check if running else default false
            var found = linqEntities?.FirstOrDefault();
            var foundIsRunning = found?.IsRunning ?? false;

            return foundIsRunning;
        }
        catch (Exception e)
        {
            //APILogger.Error(e); //log it

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
        AzureTable.CallTracker.UpsertEntity(customerEntity);

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
        AzureTable.CallTracker.UpsertEntity(customerEntity);
    }

    /// <summary>
    /// clear the record for the call
    /// </summary>
    public static void DeleteCall(string callerId)
    {
        // Query for the entity to be deleted
        Pageable<CallStatusEntity> linqEntities = AzureTable.CallTracker.Query<CallStatusEntity>(call => call.PartitionKey == callerId);

        var entityToDelete = linqEntities?.FirstOrDefault();

        if (entityToDelete != null)
        {
            // Delete the entity
            AzureTable.CallTracker.DeleteEntity(entityToDelete.PartitionKey, entityToDelete.RowKey);
        }

    }

}