using System;
using Azure;
using Azure.Data.Tables;
using System.Security.Cryptography;
using System.Text;
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
public class GeoLocationTimezoneMetadataEntity : ITableEntity
{
    public static GeoLocationTimezoneMetadataEntity Empty = new GeoLocationTimezoneMetadataEntity();

    /// <summary>
    /// hash that identifies that represents the data,
    /// hash is used at query time to check for match
    /// </summary>
    public string PartitionKey { get; set; }

    /// <summary>
    /// leave empty not needed
    /// </summary>
    public string RowKey { get; set; }


    /// <summary>
    /// final timezone with combined DST
    /// </summary>
    public string TimezoneText { get; set; }

    /// <summary>
    /// STD offset always on
    /// </summary>
    public string StandardOffset { get; set; }

    /// <summary>
    /// daylight timezone only when timezone
    /// </summary>
    public string DaylightSavings { get; set; }

    /// <summary>
    /// ISO CODE
    /// </summary>
    public string Tag { get; set; }

    /// <summary>
    /// extra data to verify if DST exists
    /// </summary>
    public string Standard_Name { get; set; }

    /// <summary>
    /// extra data to verify if DST exists
    /// </summary>
    public string Daylight_Name { get; set; }

    /// <summary>
    /// ISO name, MS API calls it ID
    /// </summary>
    public string ISO_Name { get; set; }



    /// <summary>
    /// mandatory
    /// </summary>
    public DateTimeOffset? Timestamp { get; set; }

    /// <summary>
    /// mandatory
    /// </summary>
    public ETag ETag { get; set; }



    public string CalculateCombinedHash()
    {
        // Convert all non-null and non-empty string properties into a single string
        var propertyValues = new StringBuilder()
            .Append(this.TimezoneText ?? "")
            .Append(this.StandardOffset ?? "")
            .Append(this.DaylightSavings ?? "")
            .Append(this.Tag ?? "")
            .Append(this.Standard_Name ?? "")
            .Append(this.Daylight_Name ?? "")
            .Append(this.ISO_Name ?? "");

        // Create an MD5 hash of the concatenated property values
        using (var md5 = MD5.Create())
        {
            byte[] inputBytes = Encoding.ASCII.GetBytes(propertyValues.ToString());
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            // Format the hash as a hexadecimal string
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
        }
    }
}