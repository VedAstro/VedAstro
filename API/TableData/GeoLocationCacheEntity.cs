﻿using Azure;
using Azure.Data.Tables;
using VedAstro.Library;

namespace API;

/// <summary>
/// Represents 1 row in Geo Location Cache used in API
/// </summary>
public class GeoLocationCacheEntity : ITableEntity
{
    /// <summary>
    /// Name of the location
    /// </summary>
    public string PartitionKey { get; set; }
    
    /// <summary>
    /// date time offset in ticks seconds
    /// </summary>
    public string RowKey { get; set; }
    public DateTimeOffset? Timestamp { get; set; }
    
    /// <summary>
    /// Timezone linked to date time offset in RowKey
    /// </summary>
    public string Timezone { get; set; }
    public ETag ETag { get; set; }
    public double Longitude { get; set; }
    public double Latitude { get; set; }

    public GeoLocation ToGeoLocation() => new(PartitionKey, Longitude, Latitude);
}