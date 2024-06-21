using System;
using System.Collections.Generic;
using Azure;
using Azure.Data.Tables;
using Newtonsoft.Json.Linq;
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
public class SearchAddressGeoLocationEntity : ITableEntity
{
    public static SearchAddressGeoLocationEntity Empty = new SearchAddressGeoLocationEntity();

    /// <summary>
    /// cleaned text entered by user
    /// </summary>
    public string PartitionKey { get; set; }

    /// <summary>
    /// empty
    /// </summary>
    public string RowKey { get; set; }
    
    /// <summary>
    /// List of GeoLocation in JSON string format
    /// </summary>
    public string Results { get; set; }



    /// <summary>
    /// mandatory
    /// </summary>
    public DateTimeOffset? Timestamp { get; set; }

    /// <summary>
    /// mandatory
    /// </summary>
    public ETag ETag { get; set; }


    public List<GeoLocation> ToGeoLocationList()
    {
        try
        {
            //if empty name then fail
            if (string.IsNullOrEmpty(PartitionKey)) { return new List<GeoLocation>(); }


            //parse string into jobject
            var parsedListJson = JArray.Parse(Results);

            var returnList = new List<GeoLocation>();
            //convert each jobject list into geo location
            foreach (var geoLocationJson in parsedListJson)
            {
                returnList.Add(GeoLocation.FromJson(geoLocationJson));
            }

            return returnList;
        }
        catch (Exception e)
        {
            return new List<GeoLocation>();
        }
    }
}