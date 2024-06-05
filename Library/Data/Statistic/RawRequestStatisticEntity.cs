using System;
using Azure;
using Azure.Data.Tables;
using VedAstro.Library;
using System;
using System.ComponentModel;
using System.Security.Cryptography;
using System.Text;

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

/// <summary>
/// Represents raw statistics of an HTTP request.
/// </summary>
[Serializable]
public class RawRequestStatisticEntity : ITableEntity
{
    /// <summary>
    /// Gets or sets the requested URL.
    /// Sample value: "/sample/path"
    /// </summary>
    public string PartitionKey { get; set; }

    /// <summary>
    /// Gets or sets an empty string.
    /// Sample value: ""
    /// </summary>
    public string RowKey { get; set; }

    // ALL POSSIBLE HTTP REQUEST HEADERS

    #region Headers

    /// <summary>
    /// Gets or sets the 'Accept' header value
    /// Describes the media types acceptable for the response
    /// Example: application/json
    /// </summary>
    [Description("Accept")]
    public string Accept { get; set; }

    /// <summary>
    /// Gets or sets the 'Accept-Charset' header value
    /// Indicates the charsets that the browser accepts
    /// Example: UTF-8,ISO-8859-1
    /// </summary>
    [Description("Accept-Charset")]
    public string AcceptCharset { get; set; }

    /// <summary>
    /// Gets or sets the 'Accept-Encoding' header value
    /// Indicates the encoding schemes that the browser understands
    /// Example: gzip,deflate,br
    /// </summary>
    [Description("Accept-Encoding")]
    public string AcceptEncoding { get; set; }

    /// <summary>
    /// Gets or sets the 'Accept-Language' header value
    /// Defines the language(s) preferred by the user agent
    /// Example: en-us,en;q=0.5
    /// </summary>
    [Description("Accept-Language")]
    public string AcceptLanguage { get; set; }

    /// <summary>
    /// Gets or sets the 'Authorization' header value
    /// Contains the credential details necessary to authenticate the request
    /// Example: Basic YWRtaW46cGFzc3dvcmQ=
    /// </summary>
    [Description("Authorization")]
    public string Authorization { get; set; }

    /// <summary>
    /// Gets or sets the 'Cache-Control' header value
    /// Manages caching behaviors between sender and receiver
    /// Example: no-store
    /// </summary>
    [Description("Cache-Control")]
    public string CacheControl { get; set; }

    /// <summary>
    /// Gets or sets the 'Connection' header value
    /// Determines persistence of TCP connection
    /// Example: Keep-Alive
    /// </summary>
    [Description("Connection")]
    public string Connection { get; set; }

    /// <summary>
    /// Gets or sets the 'Cookie' header value
    /// Sends cookie(s) previously obtained from the server
    /// Example: session_id=1234567890; _ga=GA1.2.1325446114.1528792981
    /// </summary>
    [Description("Cookie")]
    public string Cookie { get; set; }

    /// <summary>
    /// Gets or sets the 'Content-Length' header value
    /// Communicates the size of the content being transmitted
    /// Example: 1234
    /// </summary>
    [Description("Content-Length")]
    public string ContentLength { get; set; }

    /// <summary>
    /// Gets or sets the 'Content-MD5' header value
    /// Supplies an MD5 digest of the message body
    /// Example: qbWDFdpPiOGBkAbMEEwJBA==
    /// </summary>
    [Description("Content-MD5")]
    public string ContentMD5 { get; set; }

    /// <summary>
    /// Gets or sets the 'Content-Type' header value
    /// Declares the format of the body content
    /// Example: text/plain;charset=utf-8
    /// </summary>
    [Description("Content-Type")]
    public string ContentType { get; set; }

    /// <summary>
    /// Gets or sets the 'Date' header value
    /// Conveys the date and time when the message was originated
    /// Example: Fri, 07 Jun 2019 09:01:22 GMT
    /// </summary>
    [Description("Date")]
    public string Date { get; set; }

    /// <summary>
    /// Gets or sets the 'Expect' header value
    /// Instructs the server what expectations the client has
    /// Example: 100-Continue
    /// </summary>
    [Description("Expect")]
    public string Expect { get; set; }

    /// <summary>
    /// Gets or sets the 'From' header value
    /// Indicates the email address of the user making the request
    /// Example: user@domain.com
    /// </summary>
    [Description("From")]
    public string From { get; set; }

    /// <summary>
    /// Gets or sets the 'Host' header value
    /// Identifies the target host and optional port number
    /// Example: www.contoso.com:80
    /// </summary>
    [Description("Host")]
    public string Host { get; set; }

    /// <summary>
    /// Gets or sets the 'If-Match' header value
    /// Enables conditional deletion based on ETags
    /// Example: &quot;"etag1","etag2&quot;
    /// </summary>
    [Description("If-Match")]
    public string IfMatch { get; set; }

    /// <summary>
    /// Gets or sets the 'If-Modified-Since' header value
    /// Returns the resource if it has changed since given date
    /// Example: Sat, 1 Jan 2000 00:00:00 GMT
    /// </summary>
    [Description("If-Modified-Since")]
    public string IfModifiedSince { get; set; }

    /// <summary>
    /// Gets or sets the 'If-None-Match' header value
    /// Prevents accidental overwriting due to preconditions failure
    /// Example: *,*
    /// </summary>
    [Description("If-None-Match")]
    public string IfNoneMatch { get; set; }

    /// <summary>
    /// Gets or sets the 'If-Range' header value
    /// Limits the scope of a partial retrieval
    /// Example: etag
    /// </summary>
    [Description("If-Range")]
    public string IfRange { get; set; }

    /// <summary>
    /// Gets or sets the 'If-Unmodified-Since' header value
    /// Only applies changes made before this time
    /// Example: Thu, 1 Apr 1993 05:25:07 GMT
    /// </summary>
    [Description("If-Unmodified-Since")]
    public string IfUnmodifiedSince { get; set; }

    /// <summary>
    /// Gets or sets the 'Max-Forwards' header value
    /// Restricts how many intermediaries may forward requests
    /// Example: 10
    /// </summary>
    [Description("Max-Forwards")]
    public string MaxForwards { get; set; }

    /// <summary>
    /// Gets or sets the 'Pragma' header value
    /// Carries implementation-specific directives
    /// Example: no-cache
    /// </summary>
    [Description("Pragma")]
    public string Pragma { get; set; }

    /// <summary>
    /// Gets or sets the 'Proxy-Authorization' header value
    /// Sends proxy authentication credentials
    /// Example: BASIC dXNlcm5hbWU6cGFzc3dvcmQ=
    /// </summary>
    [Description("Proxy-Authorization")]
    public string ProxyAuthorization { get; set; }

    /// <summary>
    /// Gets or sets the 'Range' header value
    /// Retrieves part of a document identified by its URI
    /// Example: bytes=0-99
    /// </summary>
    [Description("Range")]
    public string Range { get; set; }

    /// <summary>
    /// Gets or sets the 'Referer' header value
    /// Shares the referrer URL with the destination site
    /// Example: https://www.example.com/index.html
    /// </summary>
    [Description("Referer")]
    public string Referer { get; set; }

    /// <summary>
    /// Gets or sets the 'TE' header value
    /// Lists supported extension techniques
    /// Example: trailers,deflate
    /// </summary>
    [Description("TE")]
    public string TE { get; set; }

    /// <summary>
    /// Gets or sets the 'Upgrade' header value
    /// Expresses willingness to upgrade communication
    /// Example: h2,http/1.1
    /// </summary>
    [Description("Upgrade")]
    public string Upgrade { get; set; }

    /// <summary>
    /// Gets or sets the 'User-Agent' header value
    /// Reports the operating system, software vendor, etc.
    /// Example: Mozilla/5.0 (Windows NT 10.0; WOW64; Trident/7.0; AS; rv:11.0)
    /// like Gecko
    /// </summary>
    [Description("User-Agent")]
    public string UserAgent { get; set; }

    /// <summary>
    /// Gets or sets the 'Via' header value
    /// Routes messages via intermediary servers
    /// Example: 1.0 fred, 1.1 example.com (Apache/2.2.22)
    /// </summary>
    [Description("Via")]
    public string Via { get; set; }

    /// <summary>
    /// Gets or sets the 'Warning' header value
    /// Holds extra warning info regarding the status of the response
    /// Example: 199 Miscellaneous warning http://www.wisdomandwonder.com/
    /// </summary>
    [Description("Warning")]
    public string Warning { get; set; }

    /// <summary>
    /// Gets or sets the 'Sec-Fetch-Referer' header value
    /// Provides the Referer URL of the request
    /// Example: "https://www.example.com"
    /// </summary>
    [Description("sec-fetch-referer")]
    public string SecFetchReferer { get; set; }

    /// <summary>
    /// Gets or sets the 'Sec-Fetch-Origin' header value
    /// Provides the origin of the request
    /// Example: "https://www.example.com"
    /// </summary>
    [Description("sec-fetch-origin")]
    public string SecFetchOrigin { get; set; }

    /// <summary>
    /// Gets or sets the 'Sec-Fetch-Dest' header value
    /// Provides the request's destination according to Fetch specification
    /// Example: "document"
    /// </summary>
    [Description("sec-fetch-dest")]
    public string SecFetchDest { get; set; }

    /// <summary>
    /// Gets or sets the 'Sec-Fetch-Mode' header value
    /// Provides the request's mode according to Fetch specification
    /// Example: "navigate"
    /// </summary>
    [Description("sec-fetch-mode")]
    public string SecFetchMode { get; set; }

    /// <summary>
    /// Gets or sets the 'Sec-Fetch-Site' header value
    /// Provides the relationship between the request initiator and the destination, according to Fetch specification
    /// Example: "same-origin"
    /// </summary>
    [Description("sec-fetch-site")]
    public string SecFetchSite { get; set; }

    /// <summary>
    /// Gets or sets the 'Sec-Fetch-User' header value
    /// Indicates whether or not a navigation request was triggered by a user activation
    /// Example: "?1"
    /// </summary>
    [Description("sec-fetch-user")]
    public string SecFetchUser { get; set; }


    /// <summary>
    /// Gets or sets the 'Sec-CH-UA-Platform' header value
    /// Provides platform information according to Chromium standards
    /// Example: Windows
    /// </summary>
    [Description("sec-ch-ua-platform")]
    public string SecChUaPlatform { get; set; }

    /// <summary>
    /// Gets or sets the 'Sec-CH-UA' header value
    /// Provides browser information according to Chromium standards
    /// Example: " Not A;Brand";v="99", "Chromium";v="90"
    /// </summary>
    [Description("sec-ch-ua")]
    public string SecChUa { get; set; }

    /// <summary>
    /// Gets or sets the 'Sec-CH-UA-Mobile' header value
    /// Indicates whether the browser is on a mobile device or not
    /// Example: ?0
    /// </summary>
    [Description("sec-ch-ua-mobile")]
    public string SecChUaMobile { get; set; }

    /// <summary>
    /// Gets or sets the 'Sec-CH-UA-Full-Version' header value
    /// Provides full version information of the browser
    /// Example: "90.0.4430.93"
    /// </summary>
    [Description("sec-ch-ua-full-version")]
    public string SecChUaFullVersion { get; set; }

    /// <summary>
    /// Gets or sets the 'Sec-CH-UA-Arch' header value
    /// Provides architecture information of the browser
    /// Example: "x86"
    /// </summary>
    [Description("sec-ch-ua-arch")]
    public string SecChUaArch { get; set; }

    /// <summary>
    /// Gets or sets the 'Sec-CH-UA-Model' header value
    /// Provides model information of the device
    /// Example: ""
    /// </summary>
    [Description("sec-ch-ua-model")]
    public string SecChUaModel { get; set; }

    /// <summary>
    /// Gets or sets the 'Sec-CH-UA-Platform-Version' header value
    /// Provides platform version information of the browser
    /// Example: "10.0"
    /// </summary>
    [Description("sec-ch-ua-platform-version")]
    public string SecChUaPlatformVersion { get; set; }


    /// <summary>
    /// Gets or sets the 'X-Azure-ClientIP' header value
    /// Custom header to store client IP address
    /// </summary>
    [Description("X-Azure-ClientIP")]
    public string XAzureClientIP { get; set; }

    /// <summary>
    /// Gets or sets the 'X-Forwarded-For' header value
    /// Standard header used by proxies and load balancers to identify the originating IP address
    /// </summary>
    [Description("X-Forwarded-For")]
    public string XForwardedFor { get; set; }

    /// <summary>
    /// Gets or sets the 'X-Forwarded-Host' header value
    /// Standard header used by proxies and load balancers to indicate original host requested by the client
    /// </summary>
    [Description("X-Forwarded-Host")]
    public string XForwardedHost { get; set; }

    /// <summary>
    /// Gets or sets the 'X-Forwarded-Proto' header value
    /// Standard header used by proxies and load balancers to indicate the protocol (http or https) used by the client
    /// </summary>
    [Description("X-Forwarded-Proto")]
    public string XForwardedProto { get; set; }

    /// <summary>
    /// Gets or sets the 'X-Real-IP' header value
    /// Nginx reverse proxy uses this header to pass real client IP address
    /// </summary>
    [Description("X-Real-IP")]
    public string XRealIP { get; set; }


    #endregion

    /// <summary>
    /// Gets or sets a required field in Azure Cosmos DB representing the last updated timestamp of the entity. It's automatically managed by Azure Cosmos DB.
    /// </summary>
    public DateTimeOffset? Timestamp { get; set; }

    /// <summary>
    /// Gets or sets a required field in Azure Cosmos DB representing a unique identifier for the version of the entity. It's automatically managed by Azure Cosmos DB.
    /// </summary>
    public ETag ETag { get; set; }



    public string CalculateCombinedHash()
    {
        var propertyValues = new StringBuilder();

        // Get all properties of the current instance
        var properties = this.GetType().GetProperties();

        foreach (var property in properties)
        {
            // Check if the property has a Description attribute
            var hasDescriptionAttribute = Attribute.IsDefined(property, typeof(DescriptionAttribute));

            if (hasDescriptionAttribute)
            {
                // Get the property value
                var value = property.GetValue(this) as string;

                // Append the value to the StringBuilder
                propertyValues.Append(value ?? "");
            }
        }

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