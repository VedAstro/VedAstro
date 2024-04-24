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
public class GeoLocationCoordinatesEntity : ITableEntity
{
    public static GeoLocationCoordinatesEntity Empty = new GeoLocationCoordinatesEntity();

    /// <summary>
    /// longitude at 2 decimal accuracy (1.11 km)
    /// NOTE: longitude portion is bigger because more longitudes than latitude in real case use
    /// </summary>
    public string PartitionKey { get; set; }

    /// <summary>
    /// NOTE: keep empty as decided for not needed
    /// </summary>
    public string RowKey { get; set; }

    public string Standard_Name { get; set; }

    public double Longitude { get; set; }

    public double Latitude { get; set; }


    /// <summary>
    /// mandatory
    /// </summary>
    public DateTimeOffset? Timestamp { get; set; }

    /// <summary>
    /// mandatory
    /// </summary>
    public ETag ETag { get; set; }

}