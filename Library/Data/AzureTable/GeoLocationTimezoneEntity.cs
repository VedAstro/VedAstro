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
public class GeoLocationTimezoneEntity : ITableEntity
{
    public static GeoLocationTimezoneEntity Empty = new GeoLocationTimezoneEntity();

    /// <summary>
    /// latitude & longitude in google search friendly format
    /// EXP: -3.9571599,103.8723379 
    /// </summary>
    public string PartitionKey { get; set; }

    /// <summary>
    /// time at place date time format no timezone
    /// </summary>
    public string RowKey { get; set; }


    /// <summary>
    /// final timezone with combined DST comes in from API
    /// </summary>
    public string TimezoneText { get; set; }

    /// <summary>
    /// hash that links to metadata (not shatter or wax)
    /// </summary>
    public string MetadataHash { get; set; }


    /// <summary>
    /// mandatory
    /// </summary>
    public DateTimeOffset? Timestamp { get; set; }

    /// <summary>
    /// mandatory
    /// </summary>
    public ETag ETag { get; set; }

}