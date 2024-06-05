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
public class IpAddressGeoLocationEntity : ITableEntity
{
    public static IpAddressGeoLocationEntity Empty = new IpAddressGeoLocationEntity();

    /// <summary>
    /// Ip Address
    /// </summary>
    public string PartitionKey { get; set; }

    /// <summary>
    /// empty
    /// </summary>
    public string RowKey { get; set; }

    public string LocationName { get; set; }

    public double Longitude { get; set; }

    public double Latitude { get; set; }


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


    public GeoLocation ToGeoLocation()
    {
        try
        {
            //if empty name then fail
            if (string.IsNullOrEmpty(this.LocationName)) { return GeoLocation.Empty; }

            return new GeoLocation(this.LocationName, this.Longitude, this.Latitude);

        }
        //if any fails then empty
        catch (Exception e)
        {
            return GeoLocation.Empty;
        }
    }


}