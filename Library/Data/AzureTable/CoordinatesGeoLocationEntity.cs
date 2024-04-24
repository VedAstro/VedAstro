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
public class CoordinatesGeoLocationEntity : ITableEntity
{
    public static CoordinatesGeoLocationEntity Empty = new CoordinatesGeoLocationEntity();

    /// <summary>
    /// Latitude (placed here for fast indexing, known by caller)
    /// </summary>
    public string PartitionKey { get; set; }

    /// <summary>
    /// Longitude (placed here for fast indexing, known by caller)
    /// </summary>
    public string RowKey { get; set; }

    /// <summary>
    /// Formal location name
    /// </summary>
    public string Name { get; set; }


    /// <summary>
    /// moroccan hash that links to metadata (not shatter or wax)
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

    public GeoLocation ToGeoLocation()
    {
        try
        {
            //if empty name then fail
            if (string.IsNullOrEmpty(this.Name)) { return GeoLocation.Empty; }

            //convert from string to numbers
            var latitude = double.Parse(this.PartitionKey);
            var longitude = double.Parse(this.RowKey);

            return new GeoLocation(this.Name, longitude, latitude);

        }
        //if any fails then empty
        catch (Exception e)
        {
            return GeoLocation.Empty;
        }
    }
}