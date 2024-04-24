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
public class AddressGeoLocationEntity : ITableEntity
{
    public static AddressGeoLocationEntity Empty = new AddressGeoLocationEntity();

    /// <summary>
    /// full formatted named
    /// EXP: Tokyo Japan 
    /// </summary>
    public string PartitionKey { get; set; }

    /// <summary>
    /// cleaned named entered by user
    /// EXP: Japan
    /// </summary>
    public string RowKey { get; set; }


    public double Longitude { get; set; }

    public double Latitude { get; set; }


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
            if (string.IsNullOrEmpty(PartitionKey)) { return GeoLocation.Empty; }

            return new GeoLocation(PartitionKey, Longitude, Latitude);
        }
        catch (Exception e)
        {
            return GeoLocation.Empty;
        }
    }
}