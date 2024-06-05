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
public class IpAddressGeoLocationMetadataEntity : ITableEntity
{
    public static IpAddressGeoLocationMetadataEntity Empty = new IpAddressGeoLocationMetadataEntity();

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
    /// mandatory
    /// </summary>
    public DateTimeOffset? Timestamp { get; set; }

    /// <summary>
    /// mandatory
    /// </summary>
    public ETag ETag { get; set; }

    public string AsnName { get; set; }
    public string TimezoneName { get; set; }
    public string TimezoneOffset { get; set; }
    public string IsProxy { get; set; }
    public string IsDatacenter { get; set; }
    public string IsAnonymous { get; set; }
    public string IsKnownAttacker { get; set; }
    public string IsKnownAbuser { get; set; }
    public string IsThreat { get; set; }
    public string IsBogon { get; set; }


    public string CalculateCombinedHash()
    {
        // Convert all non-null and non-empty string properties into a single string
        var propertyValues = new StringBuilder()
            .Append(this.AsnName ?? "")
            .Append(this.TimezoneName ?? "")
            .Append(this.TimezoneOffset ?? "")
            .Append(this.IsProxy ?? "")
            .Append(this.IsDatacenter ?? "")
            .Append(this.IsAnonymous ?? "")
            .Append(this.IsKnownAttacker ?? "")
            .Append(this.IsKnownAbuser ?? "")
            .Append(this.IsThreat ?? "")
            .Append(this.IsBogon ?? "");

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