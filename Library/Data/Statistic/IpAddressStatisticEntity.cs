using System;
using Azure;
using Azure.Data.Tables;
using VedAstro.Library;

namespace VedAstro.Library;

/// <summary>
/// Represents 1 row in Geo Location Timezone table used in API
/// used to store world timezone data
/// Facts:
/// 1 decimal place: 11.1 km
/// 2 decimal places: 1.11 km
/// 3 decimal places: 111 m
/// 4 decimal places: 11.1 m
/// 5 decimal places: 1.11 m
/// 6 decimal places: 0.111 m
/// </summary>
public class IpAddressStatisticEntity : ITableEntity
{
    public static IpAddressStatisticEntity Empty = new IpAddressStatisticEntity();

    /// <summary>
    /// Ip Address
    /// </summary>
    public string PartitionKey { get; set; }

    /// <summary>
    /// empty
    /// </summary>
    public string RowKey { get; set; }


    public double CallsPerSecond { get; set; }
    public DateTimeOffset? PerSecondTimestamp { get; set; }

    public double CallsPerMinute { get; set; }
    public DateTimeOffset? PerMinuteTimestamp { get; set; }

    public double CallsPerHour { get; set; }
    public DateTimeOffset? PerHourTimestamp { get; set; }

    public double CallsPerDay { get; set; }
    public DateTimeOffset? PerDayTimestamp { get; set; }

    public double CallsPerMonth { get; set; }
    public DateTimeOffset? PerMonthTimestamp { get; set; }


    /// <summary>
    /// mandatory
    /// </summary>
    public DateTimeOffset? Timestamp { get; set; }

    /// <summary>
    /// mandatory
    /// </summary>
    public ETag ETag { get; set; }


}