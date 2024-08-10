using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using HtmlAgilityPack;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Logging;
using MimeDetective.Storage.Xml.v2;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OfficeOpenXml;
using Svg;
using SwissEphNet;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Microsoft.Bing.ImageSearch;
using Microsoft.Bing.ImageSearch.Models;
using MimeDetective.Definitions;
using Formatting = Newtonsoft.Json.Formatting;
using XmlSerializer = System.Xml.Serialization.XmlSerializer;
using System.Reflection.Metadata;


//█ ▀ █▀▄▀█   █▄░█ █▀█ ▀█▀   ▄▀█ █▄░█   █ █▄░█ █▀▀ █░█   ▀█▀ █▀█ █▀█   █▀▀ ▄▀█ █▀█
//█ ░ █░▀░█   █░▀█ █▄█ ░█░   █▀█ █░▀█   █ █░▀█ █▄▄ █▀█   ░█░ █▄█ █▄█   █▀░ █▀█ █▀▄

//█▀█ █▀█   ▄▀█   █▀ █▀▀ █▀▀ █▀█ █▄░█ █▀▄   ▀█▀ █▀█ █▀█   █░░ ▄▀█ ▀█▀ █▀▀ ░  
//█▄█ █▀▄   █▀█   ▄█ ██▄ █▄▄ █▄█ █░▀█ █▄▀   ░█░ █▄█ █▄█   █▄▄ █▀█ ░█░ ██▄ █  

//█ ▀ █▀▄▀█   █▀▀ ▀▄▀ ▄▀█ █▀▀ ▀█▀ █░░ █▄█   █░█░█ █░█ █▀▀ █▀█ █▀▀  
//█ ░ █░▀░█   ██▄ █░█ █▀█ █▄▄ ░█░ █▄▄ ░█░   ▀▄▀▄▀ █▀█ ██▄ █▀▄ ██▄  

//█ ▀ █▀▄▀█   █▀ █░█ █▀█ █▀█ █▀█ █▀ █▀▀ █▀▄   ▀█▀ █▀█   █▄▄ █▀▀   ▄▀█ █░░ █░█░█ ▄▀█ █▄█ █▀ ░
//█ ░ █░▀░█   ▄█ █▄█ █▀▀ █▀▀ █▄█ ▄█ ██▄ █▄▀   ░█░ █▄█   █▄█ ██▄   █▀█ █▄▄ ▀▄▀▄▀ █▀█ ░█░ ▄█ █

//█░█ █▀▀ █▀█ █▀▀   ▄▀█ █▄░█ █▀▄   █▄░█ █▀█ █░█░█  
//█▀█ ██▄ █▀▄ ██▄   █▀█ █░▀█ █▄▀   █░▀█ █▄█ ▀▄▀▄▀  

//█░█░█ █ ▀█▀ █░█   █▀▄▀█ █▄█   █▀ █░█░█ █▀▀ █▀▀ ▀█▀ █░█ █▀▀ ▄▀█ █▀█ ▀█▀ ░
//▀▄▀▄▀ █ ░█░ █▀█   █░▀░█ ░█░   ▄█ ▀▄▀▄▀ ██▄ ██▄ ░█░ █▀█ ██▄ █▀█ █▀▄ ░█░ ▄


namespace VedAstro.Library
{
    /// <summary>
    /// A collection of general functions that don't have a home yet, so they live here for now.
    /// You're allowed to move them somewhere you see fit, not copy, move!! remember dear :-)
    /// </summary>
    public static class Tools
    {

        public const string BlobContainerName = "vedastro-site-data";


        /// <summary>
        /// Searches for person's image on BING and return one most probable as result
        /// note uses thumbnail version for speed and data save
        /// </summary>
        public static async Task<byte[]> GetSearchImage(Person personToImage)
        {
            Console.WriteLine("NOT IMPLEMENTED!");
            return new[] { byte.MinValue };

            ////IMPORTANT: replace this variable with your Cognitive Services subscription key
            //string subscriptionKey = Secrets.Get("BING_IMAGE_SEARCH");
            ////stores the image results returned by Bing
            //Default.FileTypes.Images imageResults = null;

            //var client = new ImageSearchClient(new ApiKeyServiceClientCredentials(subscriptionKey));

            ////make search query based on person's details
            //var keywords = personToImage.DisplayName; //todo maybe location can help

            //// make the search request to the Bing Image API, and get the results
            //imageResults = await client.Images.SearchAsync(query: keywords); //search query


            ////pick out the images that seems most suited
            //var handPickedApples = imageResults.Value.Where(delegate (ImageObject x)
            //{
            //    var isJpeg = x.EncodingFormat == "jpeg";//get only jpeg images for ease of handling down the road
            //    var isCorrectShape = x.Width < x.Height; //rectangle image to fit site style better
            //    return isJpeg && isCorrectShape;
            //});


            ////get 1st image in list as data
            //var topImageUrl = handPickedApples.First().ThumbnailUrl;
            //var imageBytes = await Tools.GetFileHttp(topImageUrl);

            ////return to caller
            //return imageBytes;
        }

        public static HttpResponseData SendFileToCaller(byte[] gif, HttpRequestData incomingRequest, string mimeType)
        {
            //send image back to caller
            var response = incomingRequest.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", mimeType);
            //place in response body
            response.WriteBytes(gif);
            return response;
        }


        /// <summary>
        /// SPECIAL METHOD made to allow files straight from blob to be sent to caller
        /// as fast as possible
        /// </summary>
        public static HttpResponseData SendFileToCaller(BlobClient fileBlobClient, HttpRequestData incomingRequest, string mimeType)
        {
            //send image back to caller
            var response = incomingRequest.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", mimeType);

            //place in response body
            //NOTE: very important to pass as stream to make work
            //      if convert to byte array will not work!
            //      needs to be direct stream to response
            response.Body = fileBlobClient.OpenRead();
            return response;
        }


        /// <summary>
        /// Given an image in byte form, will save it as Person profile image in correct place with ID as file name
        /// </summary>
        public static async Task SaveNewPersonImage(string personId, byte[] imageBytes)
        {

            var blobContainerName = "vedastro-site-data";

            //get the connection string stored separately (for security reasons)
            //note: dark art secrets are in local.settings.json
            var storageConnectionString = Secrets.Get("API_STORAGE"); //place where is image is stored

            //get image from storage
            var blobContainerClient = new BlobContainerClient(storageConnectionString, blobContainerName);

            //get access to file
            var imageFile = $"images/person/{personId}.jpg";
            var fileBlobClient = blobContainerClient.GetBlobClient(imageFile);

            using var ms = new MemoryStream(imageBytes);
            var blobUploadOptions = new BlobUploadOptions();
            blobUploadOptions.AccessTier = AccessTier.Cool; //save money!

            //note no override needed because specifying BlobUploadOptions, is auto override
            await fileBlobClient.UploadAsync(content: ms, options: blobUploadOptions);

        }

        /// <summary>
        /// Given an image in blob form, will save it as Person profile image in correct place with ID as file name
        /// </summary>
        public static async Task SaveNewPersonImage(string personId, BlobClient blobToUpload)
        {

            var blobContainerName = "$web";

            //get the connection string stored separately (for security reasons)
            //note: dark art secrets are in local.settings.json
            var storageConnectionString = Secrets.Get("WEB_STORAGE"); //place where is image is stored

            //get image from storage
            var blobContainerClient = new BlobContainerClient(storageConnectionString, blobContainerName);

            //place to save new image
            var imageFile = $"images/person/{personId}.jpg";
            var oldImageToReplace = blobContainerClient.GetBlobClient(imageFile);

            // assume that if the following doesn't throw an exception, then it is successful.
            CopyFromUriOperation operation = await oldImageToReplace.StartCopyFromUriAsync(blobToUpload.Uri, null, AccessTier.Cool);
            await operation.WaitForCompletionAsync();
        }

        /// <summary>
        /// gets image already stored in Images/Person as blobclient based on image name, without file format
        /// </summary>
        public static BlobClient GetPersonImage(string personId)
        {

            var blobContainerName = "vedastro-site-data";

            //get the connection string stored separately (for security reasons)
            //note: dark art secrets are in local.settings.json
            var storageConnectionString = Secrets.Get("API_STORAGE"); //place where is image is stored

            //get image from storage
            var blobContainerClient = new BlobContainerClient(storageConnectionString, blobContainerName);

            //get access to file
            var imageFile = $"images/person/{personId}.jpg";
            var fileBlobClient = blobContainerClient.GetBlobClient(imageFile);

            return fileBlobClient;


        }


        ///// <summary>
        ///// Gets any file from Azure blob storage in string form
        ///// </summary>
        //public static async Task<string> GetStringFileFromAzureStorage(string fileName, string blobContainerName)
        //{
        //    var fileClient = await Tools.GetBlobClientAzure(fileName, blobContainerName);
        //    var xmlFile = await BlobClientToString(fileClient);

        //    return xmlFile;
        //}



        /// <summary>
        /// Saves XML file direct to Azure storage
        /// </summary>
        public static async Task SaveXDocumentToAzure(XDocument dataXml, string fileName, string containerName)
        {
            //get file client for file
            var fileClient = await Tools.GetBlobClientAzure(fileName, containerName);

            //upload modified list to storage
            await Tools.OverwriteBlobData(fileClient, dataXml);
        }



        /// <summary>
        /// check if a person's profile image already exist on server
        /// </summary>
        public static async Task<bool> IsCustomPersonImageExist(string personId)
        {

            var blobContainerName = "vedastro-site-data";

            //get the connection string stored separately (for security reasons)
            //note: dark art secrets are in local.settings.json
            var storageConnectionString = Secrets.Get("API_STORAGE"); //place where is image is stored

            //get image from storage
            var blobContainerClient = new BlobContainerClient(storageConnectionString, blobContainerName);

            //get access to file
            var imageFile = $"images/person/{personId}.jpg";
            var fileBlobClient = blobContainerClient.GetBlobClient(imageFile);

            //do the actual code 
            var isCustomPersonImageExist = await fileBlobClient.ExistsAsync();

            var x = isCustomPersonImageExist.Value;
            return x;

        }

        public static bool IsSecureConnection(this HttpRequestData incomingRequest)
        {
            // Check X-Forwarded-Proto header for requests coming via a proxy
            //if (incomingRequest.Headers.ContainsKey("X-Forwarded-Proto"))
            //{
            //    var protoValues = incomingRequest.Headers["X-Forwarded-Proto"].ToString().Split(",");
            //    foreach (var proto in protoValues)
            //    {
            //        if ("https".Equals(proto.Trim(), StringComparison.OrdinalIgnoreCase))
            //            return true;
            //    }
            //}

            // Fall back to checking URL for direct requests
            if (incomingRequest.Url.Port == 443 || incomingRequest.Url.Scheme.Equals("https", StringComparison.OrdinalIgnoreCase))
                return true;

            return false;
        }

        public static string ExtractHostAddress(this HttpRequestData incomingRequest)
        {

            const string SCHEME_HTTP = "http";
            const string SCHEME_HTTPS = "https";

            var scheme = SCHEME_HTTP;
            if (incomingRequest.IsSecureConnection())
                scheme = SCHEME_HTTPS;

            var requestHeaderList = incomingRequest.Headers.ToDictionary(x => x.Key, x => x.Value, StringComparer.Ordinal);
            requestHeaderList.TryGetValue("Host", out var hostValues);
            var host = hostValues?.FirstOrDefault() ?? "no host";

            return $"{scheme}://{host}";

            ////from incoming request instance extract full host address like  "http://localhost:7071/api"
            //var requestHeaderList = incomingRequest.Headers.ToDictionary(x => x.Key, x => x.Value, StringComparer.Ordinal);
            //requestHeaderList.TryGetValue("Host", out var hostValues);
            //var host = hostValues?.FirstOrDefault() ?? "no host";
            //return host;
        }

        public static HttpResponseData SendPassHeaderToCaller(BlobClient fileBlobClient, HttpRequestData req, string mimeType)
        {
            //send image back to caller
            //response = incomingRequest.CreateResponse(HttpStatusCode.OK);
            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Call-Status", "Pass"); //lets caller know data is in payload
            response.Headers.Add("Access-Control-Expose-Headers", "Call-Status"); //needed by silly browser to read call-status
            response.Headers.Add("Content-Type", mimeType);

            //place in response body
            //NOTE: very important to pass as stream to make work
            //      if convert to byte array will not work!
            //      needs to be direct stream to response
            response.Body = fileBlobClient.OpenRead();
            return response;
        }

        public static async Task<HttpResponseData> SendPassHeaderToCaller(string dataToSend, HttpRequestData req, string mimeType)
        {
            //send image back to caller
            //response = incomingRequest.CreateResponse(HttpStatusCode.OK);
            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Call-Status", "Pass"); //lets caller know data is in payload
            response.Headers.Add("Access-Control-Expose-Headers", "Call-Status"); //needed by silly browser to read call-status
            response.Headers.Add("Content-Type", mimeType);

            //place in response body
            //NOTE: very important to pass as stream to make work
            //      if convert to byte array will not work!
            //      needs to be direct stream to response
            await response.WriteStringAsync(dataToSend);

            return response;
        }


        public static string GetCallerId(string userId, string visitorId)
        {
            var IsLoggedIn = userId != "101";
            if (IsLoggedIn)
            {
                return userId;
            }
            //if user NOT logged in then take his visitor ID as caller id
            else
            {
                return visitorId;
            }

        }


        /// <summary>
        /// Gets public IP address of client sending the http request
        /// </summary>
        public static IPAddress GetCallerIp(this HttpRequestData req)
        {
            var headerDictionary = req.Headers.ToDictionary(x => x.Key, x => x.Value, StringComparer.Ordinal);

            //debug print
            //foreach (var item in headerDictionary) { Console.WriteLine($"Key: {item.Key}, Value: {Tools.ListToString<string>(item.Value.ToList())}"); }

            var key = "x-forwarded-for";
            var key2 = "x-azure-clientip";
            IPAddress? ipAddress = null;

            if (headerDictionary.ContainsKey(key) || headerDictionary.ContainsKey(key2))
            {
                var headerValues = headerDictionary[key];
                var ipn = headerValues?.FirstOrDefault()?.Split(new char[] { ',' }).FirstOrDefault()
                    ?.Split(new char[] { ':' }).FirstOrDefault();
                var key1ParseResult = IPAddress.TryParse(ipn, out ipAddress);

                //if key 1 fail , try key 2
                if (!key1ParseResult)
                {
                    headerValues = headerDictionary[key];
                    ipn = headerValues?.FirstOrDefault()?.Split(new char[] { ',' }).FirstOrDefault()
                        ?.Split(new char[] { ':' }).FirstOrDefault();
                    key1ParseResult = IPAddress.TryParse(ipn, out ipAddress);
                }
            }

            return ipAddress ?? IPAddress.None;
        }

        /// <summary>
        /// Method from Azure Website
        /// </summary>
        public static async Task<string> DownloadToText(BlobClient blobClient)
        {
            var isFileExist = (await blobClient.ExistsAsync()).Value;

            if (isFileExist)
            {
                BlobDownloadResult downloadResult = await blobClient.DownloadContentAsync();
                string downloadedData = downloadResult.Content.ToString();

                return downloadedData;
            }
            else
            {
                //will be logged by caller
                throw new Exception($"No File in Cloud : {blobClient.Name}");
            }

        }


        /// <summary>
        /// Converts a blob client of a file to string
        /// </summary>
        public static async Task<string> BlobClientToString(BlobClient blobClient)
        {
            try
            {
                var xmlFileString = await DownloadToText(blobClient);

                return xmlFileString;
            }
            catch (Exception e)
            {
                //APILogger.Error(e); //log it
                throw new Exception($"Azure Storage Failure : {blobClient.Name}");
            }

            //Console.WriteLine(blobClient.Name);
            //Console.WriteLine(blobClient.AccountName);
            //Console.WriteLine(blobClient.BlobContainerName);
            //Console.WriteLine(blobClient.Uri);
            //Console.WriteLine(blobClient.CanGenerateSasUri);

            //if does not exist raise alarm
            if (!await blobClient.ExistsAsync())
            {
                Console.WriteLine("NO FILE!");
            }

            //parse string as xml doc
            //var valueContent = blobClient.Download().Value.Content;
            //Console.WriteLine("Text:"+Tools.StreamToString(valueContent));
        }


        public static IPAddress GetCallerIp(this HttpRequestMessage request)
        {
            IPAddress result = null;
            if (request.Headers.TryGetValues("X-Forwarded-For", out IEnumerable<string> values))
            {
                var ipn = values.FirstOrDefault().Split(new char[] { ',' }).FirstOrDefault().Split(new char[] { ':' })
                    .FirstOrDefault();
                IPAddress.TryParse(ipn, out result);
            }

            return result;
        }

        /// <summary>
        /// Subtracts the specified number of hours from the given DateTimeOffset value.
        /// </summary>
        /// <param name="value">The original DateTimeOffset value.</param>
        /// <param name="hours">The number of hours to subtract.</param>
        /// <returns>A new DateTimeOffset value with the specified hours subtracted.</returns>
        public static DateTimeOffset RemoveHours(this DateTimeOffset value, double hours)
        {
            return value.AddHours(-hours);
        }

        /// <summary>
        /// Make clone of stream
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static Stream Clone(this Stream stream)
        {
            long originalPosition = stream.Position;
            MemoryStream ms = new MemoryStream();
            stream.CopyTo(ms);
            ms.Seek(originalPosition, SeekOrigin.Begin);
            return ms;
        }

        public static async Task<T> WithCancellation<T>(this Task<T> task, CancellationToken cancellationToken)
        {
            var tcs = new TaskCompletionSource<bool>();
            using (cancellationToken.Register(s => ((TaskCompletionSource<bool>)s).TrySetResult(true), tcs))
            {
                if (task != await Task.WhenAny(task, tcs.Task))
                    throw new OperationCanceledException(cancellationToken);
            }
            return await task;
        }

        /// <summary>
        /// public static string ToJson(this MethodInfo methodInfo)
        /// </summary>
        /// <param name="methodInfo"></param>
        /// <returns></returns>
        public static JObject ToJson(this MethodInfo methodInfo)
        {
            var jObject = new JObject
            {
                ["Name"] = methodInfo.Name,
                ["DeclaringType"] = methodInfo.DeclaringType?.FullName,
                ["Parameters"] = new JArray(methodInfo.GetParameters().Select(p => new JObject
                {
                    ["Name"] = p.Name,
                    ["ParameterType"] = p.ParameterType.FullName
                }))
            };
            return jObject;
        }

        // public static MethodInfo FromJson(this MethodInfo methodInfo, JObject jsonInput)
        public static MethodInfo MethodInfoFromJson(JObject jObject)
        {
            var methodName = (string)jObject["Name"];
            var declaringType = Type.GetType((string)jObject["DeclaringType"]);
            var parameters = ((JArray)jObject["Parameters"]).Select(jt => Type.GetType((string)jt["ParameterType"])).ToArray();
            var methodInfo = declaringType?.GetMethod(methodName, parameters);
            return methodInfo;
        }


        /// <summary>
        /// Converts a list of TableRow aka a full ML Data Table into CSV string 
        /// </summary>
        public static string ListToCSV(List<MLTableRow> tableRowList)
        {
            // If the list is null or empty, return an empty string.
            if (tableRowList == null || !tableRowList.Any()) { return string.Empty; }

            // Initialize a StringBuilder to build the CSV string.
            var csv = new StringBuilder();

            // Get the column names from the first row in the list.
            // The column names are the MLTableName properties of the DataColumns.
            var columnNames = tableRowList[0].DataColumns.Select(result => result.MLTableName("NOTHH!"));

            // Add the column headers to the CSV string.
            // The headers are the column names joined by commas, with "Time" as the first column.
            csv.AppendLine($"Time,{string.Join(",", columnNames)}");

            // Iterate over each row in the list.
            foreach (var row in tableRowList)
            {
                // Quote the Time value and add it as the first column of the row.
                var timeColumnData = Tools.QuoteValue(row.Time);
                // Initialize a list for the row values and add the time column data.
                var rowValues = new List<string> { timeColumnData };
                // Quote the values of the other columns and add them to the row.
                rowValues.AddRange(row.DataColumns.Select(d => Tools.QuoteValue(d.ResultAsString())));
                // Join the row values by commas to form a CSV row, and add it to the CSV string.
                csv.AppendLine(string.Join(",", rowValues));
            }
            // Convert the StringBuilder to a string and return it.
            return csv.ToString();
        }

        public static byte[] ListToExcel(List<MLTableRow> tableRowList)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet1");
                // If the list is null or empty, return an empty Excel file.
                if (tableRowList == null || !tableRowList.Any())
                {
                    return package.GetAsByteArray();
                }
                // Get the column names from the first row in the list.
                var columnNames = tableRowList[0].DataColumns.Select(result => result.MLTableName("NOT YET"));
                // Add the column headers to the Excel file.
                var headerRow = new List<string> { "Time" };
                headerRow.AddRange(columnNames);
                worksheet.Cells["A1"].LoadFromArrays(new List<object[]> { headerRow.Cast<object>().ToArray() });
                // Iterate over each row in the list.
                for (int i = 0; i < tableRowList.Count; i++)
                {
                    var row = tableRowList[i];
                    var excelRow = new List<string> { row.Time.ToString() };
                    excelRow.AddRange(row.DataColumns.Select(d => d.ResultAsString()));
                    worksheet.Cells[i + 2, 1].LoadFromArrays(new List<object[]> { excelRow.Cast<object>().ToArray() });
                }
                return package.GetAsByteArray();
            }
        }


        /// <summary>
        /// Given a JSON version of a Table will convert to HTML table in string
        /// </summary>
        public static string ConvertJsonToHtmlTable(JToken jObject)
        {
            var data = jObject.ToObject<List<Dictionary<string, string>>>();
            var sb = new StringBuilder();
            sb.Append("<table>");
            // Add header row
            sb.Append("<tr>");
            foreach (var key in data[0].Keys)
            {
                sb.AppendFormat("<th>{0}</th>", key);
            }
            sb.Append("</tr>");
            // Add data rows
            foreach (var row in data)
            {
                sb.Append("<tr>");
                foreach (var value in row.Values)
                {
                    sb.AppendFormat("<td>{0}</td>", value);
                }
                sb.Append("</tr>");
            }
            sb.Append("</table>");
            return sb.ToString();
        }


        /// <summary>
        /// Given a HTML table in string will convert to JSON version
        /// </summary>
        public static JObject ConvertHtmlTableToJson(string htmlTable)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(htmlTable);
            var table = doc.DocumentNode.SelectSingleNode("//table");
            var rows = table.SelectNodes("tr").Skip(1); // Skip header row
            var header = table.SelectSingleNode("tr"); // Header row
            var data = new List<Dictionary<string, string>>();
            foreach (var row in rows)
            {
                var rowData = new Dictionary<string, string>();
                var cells = row.SelectNodes("td");
                for (int i = 0; i < cells.Count; i++)
                {
                    var cellText = cells[i].InnerText;
                    var headerText = header.SelectNodes("th")[i].InnerText;
                    rowData.Add(headerText, cellText);
                }
                data.Add(rowData);
            }
            return JObject.FromObject(data);
        }


        /// <summary>
        /// Given any data will try to print it as data readable json
        /// Note: made for beautiful code use in python 
        /// </summary>
        public static void Print(dynamic input)
        {
            JToken jsonData = Tools.AnyToJSON("", input);
            var stringJson = jsonData.ToString();
            Console.WriteLine(stringJson);
        }

        //gets the exact width of a text based on Font size & type
        //used to generate nicely fitting background for text
        public static double GetTextWidthPx(string textInput)
        {
            //TODO handle max & min
            //set max & min width background
            //const int maxWidth = 70;
            //backgroundWidth = backgroundWidth > maxWidth ? maxWidth : backgroundWidth;
            //const int minWidth = 30;
            //backgroundWidth = backgroundWidth > minWidth ? minWidth : backgroundWidth;


            SizeF size;
            using (var graphics = Graphics.FromHwnd(IntPtr.Zero))
            {
                size = graphics.MeasureString(textInput,
                    new Font("Calibri", 12, FontStyle.Regular, GraphicsUnit.Pixel));
            }

            var widthPx = Math.Round(size.Width);

            return widthPx;
        }


        /// <summary>
        /// Gets any file at given WWW url will return as string
        /// used for SVG
        /// </summary>
        public static async Task<string> GetStringFileHttp(string url)
        {
            //get the data sender
            using var client = new HttpClient();

            client.Timeout = Timeout.InfiniteTimeSpan;

            //load xml event data files beforehand to be used quickly later for search
            //get main horoscope prediction file (located in wwwroot)
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);

            HttpResponseMessage response = await client.SendAsync(request, CancellationToken.None);
            var fileString = await response.Content.ReadAsStringAsync();
            return fileString;
        }

        /// <summary>
        /// Gets any file at given WWW url will return as bytes
        /// </summary>
        public static async Task<byte[]> GetFileHttp(string url)
        {
            using var client = new HttpClient();
            client.Timeout = Timeout.InfiniteTimeSpan;

            // Create HttpRequestMessage
            var request = new HttpRequestMessage(HttpMethod.Get, url);

            // Send the request and get the response
            using var response = await client.SendAsync(request, CancellationToken.None);

            // Ensure the request was successful
            response.EnsureSuccessStatusCode();

            // Read the content as byte array
            var fileBytes = await response.Content.ReadAsByteArrayAsync();

            return fileBytes;
        }


        /// <summary>
        /// Gets a SVG icon file direct from Illustrator, removes not needed
        /// attributes and makes it ready to be injected into another SVG
        /// no file return nothing
        /// </summary>
        public static async Task<string> GetSvgIconHttp(string svgFileUrl, double width, double height)
        {

            //get raw icon as SVG (if exist)
            var svgIconString = await Tools.GetStringFileHttp(svgFileUrl);
            if (!string.IsNullOrEmpty(svgIconString))
            {
                //remove XML file header

                var parsedIcon = Svg.SvgDocument.FromSvg<Svg.SvgDocument>(svgIconString);

                //set custom width & height
                parsedIcon.Height = (SvgUnit)height;
                parsedIcon.Width = (SvgUnit)width;
                //parsedIcon.ViewBox = new SvgViewBox(0, 0, (float)width, (float)height);

                var final = parsedIcon.GetXML();

                //<?xml version="1.0" encoding="utf-8"?>
                final = final.Replace("<?xml version=\"1.0\" encoding=\"utf-8\"?>", "");
                //<!DOCTYPE svg PUBLIC "-//W3C//DTD SVG 1.1//EN" "http://www.w3.org/Graphics/SVG/1.1/DTD/svg11.dtd">
                final = final.Replace("<!DOCTYPE svg PUBLIC \"-//W3C//DTD SVG 1.1//EN\" \"http://www.w3.org/Graphics/SVG/1.1/DTD/svg11.dtd\">", "");

                return final;
            }

            //if control reaches here than no file
            return "";

        }

        public static Image Svg2Png(string svg, int width, int height)
        {

            byte[] png_bytes;
            string png_base64;
            byte[] byte_array;
            Stream stream;
            SvgDocument svg_document;
            Bitmap bitmap;
            string base64_string;


            //convert svg string to byte array
            //NOTE : proper encoding needed else will shown funny values when render
            byte_array = Encoding.UTF8.GetBytes(svg);

            //convert byte array to stream
            stream = new MemoryStream(byte_array);

            Svg.SvgDocument.EnsureSystemIsGdiPlusCapable();


            //generate svg doc from stream
            svg_document = SvgDocument.Open<Svg.SvgDocument>(stream);

            //convert svg doc to bitmap with specified width & height
            bitmap = svg_document.Draw(width, height);

            return bitmap;

            //png_bytes = ImageToByte2(bitmap);

            //base64_string = Convert.ToBase64String(png_bytes, 0, png_bytes.Length);

            //png_base64 = "data:image/png;base64," + base64_string;

            //return png_bytes;

        }

        public static byte[] ImageToByte2(Image img)
        {
            using (var stream = new MemoryStream())
            {
                img.Save(stream, ImageFormat.Png);
                return stream.ToArray();
            }
        }

        /// <summary>
        /// Extension method to get null when out of range, instead of exception
        /// </summary>
        public static T GetValueOrNull<T>(this List<T> list, int index)
        {
            if (index >= 0 && index < list.Count)
            {
                return list[index];
            }
            else
            {
                return default(T); // Returns null for reference types and zero for numeric value types
            }
        }

        /// <summary>
        /// gets last day of any month at any time
        /// input : 01/1981
        /// </summary>
        public static int GetLastDay(string monthYearText)
        {
            //split month and year
            string[] splited = monthYearText.Split('/');
            var month = int.Parse(splited[0]);
            var year = int.Parse(splited[1]);

            int daysInMonth = DateTime.DaysInMonth(year: year, month: month);
            return daysInMonth;

        }

        /// <summary>
        /// Converts raw call from API via URL to parsed Time
        /// Note: Timezone/Offset auto get from API based on location
        /// </summary>
        public static async Task<Time> ParseTime(string locationName,
            string hhmmStr,
            string dateStr,
            string monthStr,
            string yearStr)
        {
            //NOTE: location name can be "coordinates" or "real location names" 
            //      detect here which is which based on pattern
            //  EXP: 4°32'55.1"S,101°04'57.0"E
            //      -3.9571599,103.8723379
            string coordinatesPattern = @"^-?\d+(\.\d+)?,\-?\d+(\.\d+)?$"; ; //ask AI to explain

            bool isCoordinates = Regex.IsMatch(locationName, coordinatesPattern);
            GeoLocation geoLocation;
            if (isCoordinates)
            {
                //NOTE : must be sample : "-3.9571599,103.8723379"
                var splitted = locationName.Split(',');
                double latitude = Convert.ToDouble(splitted[0]);
                double longitude = Convert.ToDouble(splitted[1]);

                //NOTE: we preserve lat and long into name as well, since technically
                //      no location name has a coordinate but a radius. So in that sense,
                //      lat and long as location name is valid, though the data seem little duplicated
                geoLocation = new GeoLocation(locationName, longitude, latitude);
            }
            else
            {
                //get coordinates for location (API)
                WebResult<GeoLocation>? geoLocationResult = await Tools.AddressToGeoLocation(locationName);
                geoLocation = geoLocationResult.Payload;
            }


            //compile the time string into standard format
            //NOTE : offset hard set to UTC 0, because not used, only format filler (will be overriden later)
            var timeStr = $"{hhmmStr} {dateStr}/{monthStr}/{yearStr} +00:00";

            //get standard UTC Timezone offset at location at time (API) 
            var isPass = Time.TryParseStd(timeStr, out var parsedInputTime);

            //get timezone as text
            var timezoneSTDOffsetResult = await Tools.GetTimezoneOffsetApi(geoLocation, parsedInputTime);
            var timeZone = timezoneSTDOffsetResult.Payload;

            //if failed to get timezone raise alarm
            if (string.IsNullOrEmpty(timeZone))
            {
                throw new Exception("Timezone failed to get from API!");
            }

            //create time with perfect coordinates & time zone
            var correctTimeString = $"{hhmmStr} {dateStr}/{monthStr}/{yearStr} {timeZone}";
            var parsedTime = new Time(correctTimeString, geoLocation);

            return parsedTime;
        }

        /// <summary>
        /// Adds an XML element to XML document in by file & container name
        /// and saves files directly to Azure blob store
        /// </summary>
        public static async Task AddXElementToXDocumentAzure(XElement dataXml, string fileName, string containerName)
        {
            //get user data list file (UserDataList.xml) Azure storage
            var fileClient = await Tools.GetBlobClientAzure(fileName, containerName);

            //add new log to main list
            var updatedListXml = await AddXElementToXDocument(fileClient, dataXml);

            //upload modified list to storage
            await OverwriteBlobData(fileClient, updatedListXml);
        }

        public static void AddXElementToXDocumentAzure(XElement dataXml, ref XDocument xDocument)
        {

            //add new log to main list
            AddXElementToXDocument(dataXml, ref xDocument);

        }

        /// <summary>
        /// Overwrites new XML data to a blob file
        /// </summary>
        public static async Task OverwriteBlobData(BlobClient blobClient, XDocument newData)
        {
            //convert xml data to string
            var dataString = newData.ToString();

            //convert xml string to stream
            var dataStream = GenerateStreamFromString(dataString);

            var blobUploadOptions = new BlobUploadOptions();
            blobUploadOptions.AccessTier = AccessTier.Cool; //save money!

            //upload stream to blob
            //note: no override needed because specifying BlobUploadOptions, is auto override
            await blobClient.UploadAsync(dataStream, options: blobUploadOptions);

            //auto correct content type from wrongly set "octet/stream"
            var blobHttpHeaders = new BlobHttpHeaders { ContentType = "text/xml" };
            await blobClient.SetHttpHeadersAsync(blobHttpHeaders);
        }

        public static Stream GenerateStreamFromString(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        /// <summary>
        /// Adds an XML element to XML document in blob form
        /// </summary>
        public static async Task<XDocument> AddXElementToXDocument(BlobClient xDocuBlobClient, XElement newElement)
        {
            //get person list from storage
            var xDocument = await Tools.DownloadToXDoc(xDocuBlobClient);

            //add new person to list
            xDocument.Root.Add(newElement);

            return xDocument;
        }

        public static void AddXElementToXDocument(XElement newElement, ref XDocument xDocument)
        {
            //add new person to list
            xDocument.Root.Add(newElement);
        }

        /// <summary>
        /// Deletes an XML element from an XML document in by file & container name
        /// and saves files directly to Azure blob store
        /// </summary>
        public static async Task DeleteXElementFromXDocumentAzure(XElement dataXmlToDelete, string fileName, string containerName)
        {
            //access to file
            var fileClient = await Tools.GetBlobClientAzure(fileName, containerName);

            //get xml file
            var xmlDocFile = await Tools.DownloadToXDoc(fileClient);

            //check if record to delete exists
            //if not found, raise alarm
            var xmlRecordList = xmlDocFile.Root.Elements();
            var personToDelete = Person.FromXml(dataXmlToDelete);
            var foundRecords = xmlRecordList.Where(x => Person.FromXml(x).Id == personToDelete.Id);
            if (!foundRecords.Any()) { throw new Exception("Could not find XML record to delete in main list!"); }

            //continue with delete
            foundRecords.First().Remove();

            //upload modified list to storage
            await OverwriteBlobData(fileClient, xmlDocFile);
        }

        /// <summary>
        /// deletes by using same reference to limit unnecessary calls to storage
        /// used in maintenance scripts
        /// deletes by ID not HASH
        /// </summary>
        public static void DeleteXElementFromXDocumentAzure(XElement dataXmlToDelete, ref XDocument xmlDocFile)
        {

            //check if record to delete exists
            //if not found, raise alarm
            var xmlRecordList = xmlDocFile.Root.Elements();
            var personToDelete = Person.FromXml(dataXmlToDelete);
            var foundRecords = xmlRecordList.Where(x => Person.FromXml(x).Id == personToDelete.Id);
            if (!foundRecords.Any()) { throw new Exception("Could not find XML record to delete in main list!"); }

            //continue with delete
            foundRecords.First().Remove();

        }

        /// <summary>
        /// used for finding uncertain time in certain birth day
        /// split a person's day into precision based slices of possible birth times
        /// </summary>
        public static List<Time> GetTimeSlicesOnBirthDay(Person person, double precisionInHours)
        {
            //start of day till end of day
            var dayStart = new Time($"00:00 {person.BirthDateMonthYear} {person.BirthTimeZone}", person.GetBirthLocation());
            var dayEnd = new Time($"23:59 {person.BirthDateMonthYear} {person.BirthTimeZone}", person.GetBirthLocation());

            var finalList = Time.GetTimeListFromRange(dayStart, dayEnd, precisionInHours);

            return finalList;
        }

        public static async Task<T> DelayedResultTask<T>(TimeSpan delay, Func<T> fallbackMaker)
        {
            await Task.Delay(delay);
            return fallbackMaker();
        }

        public static async Task<T> DelayedTimeoutExceptionTask<T>(TimeSpan delay)
        {
            await Task.Delay(delay);
            throw new TimeoutException();
        }

        public static async Task<T> TaskWithTimeoutAndException<T>(Task<T> task, TimeSpan timeout)
        {
            //two task are fired at once, real task and countdown
            return await await Task.WhenAny(task, DelayedTimeoutExceptionTask<T>(timeout));
        }


        /// <summary>
        /// Gets all horoscope predictions for a person
        /// include all when filter not specified
        /// </summary>
        public static List<HoroscopePrediction> GetHoroscopePrediction(Time birthTime, EventTag filterTag = EventTag.Empty)
        {
            //get list of horoscope data (static cs data in memory)
            var horoscopeDataList = HoroscopeDataListStatic.Rows;

            //filter what to show if any specified
            if (filterTag != EventTag.Empty)
            {
                horoscopeDataList = horoscopeDataList.Where(horoData => horoData.EventTags.Contains(filterTag)).ToList();
            }

            //start calculating predictions (mix with time by person's birthdate)
            var predictionList = CalculatePredictions(birthTime, horoscopeDataList);

            //place important predictions at the top
            //var sorted = SortPredictionData(predictionList);

            return predictionList;

            /// <summary>
            /// Get list of predictions occurring in a time period for all the
            /// inputed prediction types aka "prediction data"
            /// </summary>
            List<HoroscopePrediction> CalculatePredictions(Time birthTime, List<HoroscopeData> horoscopeDataList)
            {
                //get data to instantiate muhurtha time period
                //get start & end times

                //initialize empty list of event to return
                List<HoroscopePrediction> horoscopeList = new();

                try
                {
                    foreach (var horoscopeData in horoscopeDataList)
                    {
                        //only add if occuring
                        var isOccuring = horoscopeData.IsEventOccuring(birthTime);
                        if (isOccuring)
                        {
                            var newHoroscopePrediction = new HoroscopePrediction(horoscopeData.Name, horoscopeData.Description, horoscopeData.RelatedBody);
                            //add events to main list of event
                            horoscopeList.Add(newHoroscopePrediction);
                        }
                    }
                }
                //catches only exceptions that indicates that user canceled the calculation (caller lost interest in the result)
                catch (Exception)
                {
                    //return empty list
                    return new List<HoroscopePrediction>();
                }

                //return calculated event list
                return horoscopeList;
            }

            //TODO MARKED FOR OBLIVION
            //pushes "rising sign " to top
            List<HoroscopePrediction> SortPredictionData(List<HoroscopePrediction> horoscopePredictions)
            {
                //todo followed by planet in sign prediction ordered by planet strength 

                // Check if the list is null or empty
                if (horoscopePredictions == null || !horoscopePredictions.Any())
                {
                    throw new ArgumentException("The list of horoscope predictions cannot be null or empty.");
                }

                // Find the index of the rising sign
                int risingSignIndex = horoscopePredictions.FindIndex(horPre => horPre.FormattedName.ToLower().Contains("rising"));

                // Check if the rising sign was found
                if (risingSignIndex == -1)
                {
                    throw new Exception("The rising sign was not found in the list of horoscope predictions.");
                }

                // Move the rising sign to the beginning of the list
                HoroscopePrediction risingSign = horoscopePredictions[risingSignIndex];
                horoscopePredictions.RemoveAt(risingSignIndex);
                horoscopePredictions.Insert(0, risingSign);

                return horoscopePredictions;
            }

        }


        /// <summary>
        /// "H1N1" -> ["H", "1", "N", "1"]
        /// "H" -> ["H"]
        /// "GH1N12" -> ["GH", "1", "N", "12"]
        /// "OS234" -> ["OS", "234"]
        /// </summary>
        public static List<string> SplitAlpha(string input)
        {
            var words = new List<string> { string.Empty };
            for (var i = 0; i < input.Length; i++)
            {
                words[words.Count - 1] += input[i];
                if (i + 1 < input.Length && char.IsLetter(input[i]) != char.IsLetter(input[i + 1]))
                {
                    words.Add(string.Empty);
                }
            }
            return words;
        }

        /// <summary>
        /// Converts xml element instance to string properly
        /// </summary>
        public static string XmlToString(XElement xml)
        {
            //remove all formatting, for clean xml as string
            return xml.ToString(SaveOptions.DisableFormatting);
        }


        /// <summary>
        /// Gets XML file from any URL and parses it into xelement list
        /// </summary>
        /// <param name="url">The URL of the XML file</param>
        /// <param name="httpClient">An optional HttpClient instance to use for the request</param>
        /// <returns>A list of XElement objects representing the elements in the XML file</returns>
        public static async Task<List<XElement>> GetXmlFile(string url, HttpClient httpClient = null)
        {
            // If no HttpClient is provided, create a new one
            if (httpClient == null)
            {
                httpClient = new HttpClient();
            }

            // Create HttpRequestMessage
            var request = new HttpRequestMessage(HttpMethod.Get, url);

            // Send the request and get the response stream
            var response = await httpClient.SendAsync(request);
            var fileStream = await response.Content.ReadAsStreamAsync();

            // Parse raw file to xml doc
            var document = XDocument.Load(fileStream);

            // Get all records in document
            return document.Root.Elements().ToList();
        }


        /// <summary>
        /// Converts any type to XML, it will use Type's own ToXml() converter if available
        /// else ToString is called and placed inside element with Type's full name
        /// Note, used to transfer data via internet Client to API Server
        /// Example:
        /// <TypeName>
        ///     DataValue
        /// </TypeName>
        /// </summary>
        public static XElement AnyTypeToXml<T>(T value)
        {
            //check if type has own ToXml method
            //use the Type's own converter if available
            if (value is IToXml hasToXml)
            {
                var betterXml = hasToXml.ToXml();
                return betterXml;
            }

            //gets enum value as string to place inside XML
            //note: value can be null hence ?, fails quietly
            var enumValueStr = value?.ToString();

            //get the name of the Enum
            //Note: This is the name that will be used
            //later to instantiate the class from string
            var typeName = typeof(T).FullName;

            return new XElement(typeName, enumValueStr);
        }

        /// <summary>
        /// will convert inputed type to xelement via .net serializer
        /// </summary>
        public static XElement AnyTypeToXElement(object o)
        {
            var doc = new XDocument();
            using (XmlWriter writer = doc.CreateWriter())
            {
                XmlSerializer serializer = new XmlSerializer(o.GetType());
                serializer.Serialize(writer, o);
            }

            return doc.Root;
        }

        /// <summary>
        /// Converts any type that implements IToXml to XML, it will use Type's own ToXml() converter
        /// Note, used to transfer data via internet Client to API Server
        /// Placed inside "Root" xml
        /// Default name for root element is Root
        /// </summary>
        public static XElement AnyTypeToXmlList<T>(List<T> xmlList, string rootElementName = "Root") where T : IToXml
        {
            var rootXml = new XElement(rootElementName);
            foreach (var xmlItem in xmlList)
            {
                rootXml.Add(AnyTypeToXml(xmlItem));
            }
            return rootXml;
        }

        /// <summary>
        /// Simple override for XML, to skip parsing to type before sorting
        /// </summary>
        public static XElement AnyTypeToXmlList(List<XElement> xmlList, string rootElementName = "Root")
        {
            var rootXml = new XElement(rootElementName);
            foreach (var xmlItem in xmlList)
            {
                rootXml.Add(xmlItem);
            }
            return rootXml;
        }

        /// <summary>
        /// Given the URL of a standard VedAstro XML file, like "http://...PersonList.xml",
        /// will convert to the specified type and return in nice list, with time to be home for dinner
        /// </summary>
        public static async Task<List<T>> ConvertXmlListFileToInstanceList<T>(string httpUrl) where T : IToXml, new()
        {
            //get data list from Static Website storage
            //note : done so that any updates to that live file will be instantly reflected in API results
            var eventDataListXml = await Tools.GetXmlFile(httpUrl);

            var finalInstance = await ConvertXmlListFileToInstanceList<T>(eventDataListXml);

            return finalInstance;
        }

        /// <summary>
        /// Given an XML file will auto convert it to an instance using ToXml() method
        /// </summary>
        public static async Task<List<T>> ConvertXmlListFileToInstanceList<T>(List<XElement> eventDataListXml) where T : IToXml, new()
        {
            //parse each raw event data in list
            var eventDataList = new List<T>();
            foreach (var eventDataXml in eventDataListXml)
            {
                //add it to the return list
                var x = new T();
                eventDataList.Add(x.FromXml<T>(eventDataXml));
            }

            return eventDataList;

        }



        /// <summary>
        /// Extracts data from an Exception puts it in a nice XML
        /// </summary>
        public static XElement ExceptionToXML(Exception e)
        {
            //place to store the exception data
            string fileName;
            string methodName;
            int line;
            int columnNumber;
            string message;
            string source;

            //get the exception that started it all
            var originalException = e.GetBaseException();

            //extract the data from the error
            StackTrace st = new StackTrace(e, true);

            //Get the first stack frame
            StackFrame frame = st.GetFrame(st.FrameCount - 1);

            //Get the file name
            fileName = frame?.GetFileName();

            //Get the method name
            methodName = frame.GetMethod()?.Name;

            //Get the line number from the stack frame
            line = frame.GetFileLineNumber();

            //Get the column number
            columnNumber = frame.GetFileColumnNumber();

            message = originalException.ToString();

            source = originalException.Source;
            //todo include inner exception data
            var stackTrace = originalException.StackTrace;


            //put together the new error record
            var newRecord = new XElement("Error",
                new XElement("Message", message),
                new XElement("Source", source),
                new XElement("FileName", fileName),
                new XElement("SourceLineNumber", line),
                new XElement("SourceColNumber", columnNumber),
                new XElement("MethodName", methodName),
                new XElement("MethodName", methodName)
            );


            return newRecord;
        }

        /// <summary>
        /// - Type is a value typ
        /// - Enum
        /// </summary>
        public static dynamic XmlToAnyType<T>(XElement xml) // where T : //IToXml, new()
        {
            //get the name of the Enum
            var typeNameFullName = typeof(T).FullName;
            var typeNameShortName = typeof(T).FullName;

#if DEBUG
            Console.WriteLine(xml.ToString());
#endif

            //type name inside XML
            var xmlElementName = xml?.Name;

            //get the value for parsing later
            var rawVal = xml.Value;


            //make sure the XML enclosing type has the same name
            //check both full class name, and short class name
            var isSameName = xmlElementName == typeNameFullName || xmlElementName == typeof(T).GetShortTypeName();

            //if not same name raise error
            if (!isSameName)
            {
                throw new Exception($"Can't parse XML {xmlElementName} to {typeNameFullName}");
            }

            //implements ToXml()
            var typeImplementsToXml = typeof(T).GetInterfaces().Any(x =>
                x.IsGenericType &&
                x.GetGenericTypeDefinition() == typeof(IToXml));

            //type has owm ToXml method
            if (typeImplementsToXml)
            {
                dynamic inputTypeInstance = GetInstance(typeof(T).FullName);

                return inputTypeInstance.FromXml(xml);

            }

            //if type is an Enum process differently
            if (typeof(T).IsEnum)
            {
                var parsedEnum = (T)Enum.Parse(typeof(T), rawVal);

                return parsedEnum;
            }

            //else it is a value type
            if (typeof(T) == typeof(string))
            {
                return rawVal;
            }

            if (typeof(T) == typeof(double))
            {
                return Double.Parse(rawVal);
            }

            if (typeof(T) == typeof(int))
            {
                return int.Parse(rawVal);
            }

            //raise error since converter not implemented
            throw new NotImplementedException($"XML converter for {typeNameFullName}, not implemented!");
        }

        /// <summary>
        /// Gets only the name of the Class, without assembly
        /// </summary>
        public static string GetShortTypeName(this Type type)
        {
            var sb = new StringBuilder();
            var name = type.Name;
            if (!type.IsGenericType) return name;
            sb.Append(name.Substring(0, name.IndexOf('`')));
            sb.Append("<");
            sb.Append(string.Join(", ", type.GetGenericArguments()
                .Select(t => t.GetShortTypeName())));
            sb.Append(">");
            return sb.ToString();
        }

        public static bool Implements<I>(this Type type, I @interface) where I : class
        {
            if (((@interface as Type) == null) || !(@interface as Type).IsInterface)
                throw new ArgumentException("Only interfaces can be 'implemented'.");

            return (@interface as Type).IsAssignableFrom(type);
        }

        /// <summary>
        /// For converting value types, String, Double, etc.
        /// </summary>
        //public static dynamic XmlToValueType<T>(XElement xml) 
        //{
        //    //get the name of the Enum
        //    var typeName = nameof(T);


        //    //raise error since not XML type and Input type mismatch
        //    throw new Exception($"Can't parse XML to {typeName}");
        //}


        /// <summary>
        /// Gets an instance of Class from string name
        /// </summary>
        public static object GetInstance(string strFullyQualifiedName)
        {
            Type type = Type.GetType(strFullyQualifiedName);
            if (type != null)
                return Activator.CreateInstance(type);
            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                type = asm.GetType(strFullyQualifiedName);
                if (type != null)
                    return Activator.CreateInstance(type);
            }

            return null;
        }


        /// <summary>
        /// Converts days to hours
        /// </summary>
        /// <returns></returns>
        public static double DaysToHours(double days) => days * 24.0;

        public static double WeeksToHours(double weeks) => weeks * 7.0 * 24.0;
        public static double MonthsToHours(double months) => months * 30.44 * 24.0; // Approximate average number of days in a month
        public static double YearsToHours(double years) => years * 365.25 * 24.0; // Approximate average number of days in a year (accounting for leap years)
        public static double DecadesToHours(double decades) => decades * 10.0 * 365.25 * 24.0;


        public static double MinutesToHours(double minutes) => minutes / 60.0;

        public static double MinutesToYears(double minutes) => minutes / 525600.0;

        public static double MinutesToDays(double minutes) => minutes / 1440.0;

        /// <summary>
        /// Given a date it will count the days to the end of that year
        /// </summary>
        public static double GetDaysToNextYear(Time getBirthDateTime)
        {
            //get start of next year
            var standardTime = getBirthDateTime.GetStdDateTimeOffset();
            var nextYear = standardTime.Year + 1;
            var startOfNextYear = new DateTimeOffset(nextYear, 1, 1, 0, 0, 0, 0, standardTime.Offset);

            //calculate difference of days between 2 dates
            var diffDays = (startOfNextYear - standardTime).TotalDays;

            return diffDays;
        }

        /// <summary>
        /// Gets the time now in the system in text form
        /// formatted with standard style (HH:mm dd/MM/yyyy zzz) 
        /// </summary>
        public static string GetNowSystemTimeText() => DateTimeOffset.Now.ToString(Time.DateTimeFormat);

        /// <summary>
        /// Gets the time now in the system in text form with seconds (HH:mm:ss dd/MM/yyyy zzz) 
        /// </summary>
        public static string GetNowSystemTimeSecondsText() => DateTimeOffset.Now.ToString(Time.DateTimeFormatSeconds);

        /// <summary>
        /// Gets the time now in the Server (+8:00) in text form with seconds (HH:mm:ss dd/MM/yyyy zzz) 
        /// </summary>
        public static string GetNowServerTimeSecondsText() => DateTimeOffset.Now.ToOffset(TimeSpan.FromHours(8)).ToString(Time.DateTimeFormatSeconds);

        /// <summary>
        /// Custom hash generator for Strings. Returns consistent/deterministic values
        /// If null returns 0
        /// Note: MD5 (System.Security.Cryptography) not used because not supported in Blazor WASM
        /// </summary>
        public static int GetStringHashCode(string stringToHash)
        {
            if (stringToHash == null)
            {
                return 0;
            }

            unchecked
            {
                int hash1 = (5381 << 16) + 5381;
                int hash2 = hash1;

                for (int i = 0; i < stringToHash.Length; i += 2)
                {
                    hash1 = ((hash1 << 5) + hash1) ^ stringToHash[i];
                    if (i == stringToHash.Length - 1)
                        break;
                    hash2 = ((hash2 << 5) + hash2) ^ stringToHash[i + 1];
                }

                return hash1 + (hash2 * 1566083941);
            }

        }

        /// <summary>
        /// Custom hash generator for Strings. Returns consistent/deterministic values
        /// If null returns 0
        /// </summary>
        /// <param name="stringToHash"></param>
        /// <param name="truncateLength">The number of characters to truncate the hash to. Default is 32.</param>
        /// <returns></returns>
        public static string GetStringHashCodeMD5(string stringToHash, int truncateLength = 32)
        {
            if (stringToHash == null)
            {
                return "0";
            }

            using (MD5 md5Hasher = MD5.Create())
            {
                var hashedBytes = md5Hasher.ComputeHash(Encoding.UTF8.GetBytes(stringToHash));
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashedBytes.Length; i++)
                {
                    sb.Append(hashedBytes[i].ToString("x2"));
                }
                string hash = sb.ToString();
                return hash.Length > truncateLength ? hash.Substring(0, truncateLength) : hash;
            }
        }

        /// <summary>
        /// Gets random unique ID
        /// </summary>
        /// <param name="truncate">Optional parameter to truncate the generated ID</param>
        public static string GenerateId(int? truncate = null)
        {
            var id = Guid.NewGuid().ToString("N");
            return truncate.HasValue && truncate.Value < id.Length ? id.Substring(0, truncate.Value) : id;
        }


        /// <summary>
        /// Converts any list to comma separated string
        /// Note: calls ToString();
        /// </summary>
        public static string ListToString<T>(List<T> list, string separator = ",")
        {
            var combinedNames = "";

            for (int i = 0; i < list.Count; i++)
            {
                //when last in row, don't add comma
                var isLastItem = i == (list.Count - 1);
                var ending = isLastItem ? "" : $"{separator} ";

                //combine to together based on type
                var item = list[i];

                combinedNames += item.ToString() + ending;

                //if (item is IToJson iToJson)
                //{
                //    //todo can wrap into jobject if needed
                //    combinedNames += iToJson.ToJson() + ending;
                //}
                //else
                //{
                //    combinedNames += item.ToString() + ending;
                //}

            }

            return combinedNames;
        }







        //█▀▀ █░█ ▀▀█▀▀ █▀▀ █▀▀▄ █▀▀ ░▀░ █▀▀█ █▀▀▄ 　 █▀▄▀█ █▀▀ ▀▀█▀▀ █░░█ █▀▀█ █▀▀▄ █▀▀ 
        //█▀▀ ▄▀▄ ░░█░░ █▀▀ █░░█ ▀▀█ ▀█▀ █░░█ █░░█ 　 █░▀░█ █▀▀ ░░█░░ █▀▀█ █░░█ █░░█ ▀▀█ 
        //▀▀▀ ▀░▀ ░░▀░░ ▀▀▀ ▀░░▀ ▀▀▀ ▀▀▀ ▀▀▀▀ ▀░░▀ 　 ▀░░░▀ ▀▀▀ ░░▀░░ ▀░░▀ ▀▀▀▀ ▀▀▀░ ▀▀▀


        public static string? Truncate(this string? value, int maxLength, string truncationSuffix = "…")
        {
            return value?.Length > maxLength
                ? value.Substring(0, maxLength) + truncationSuffix
                : value;
        }

        /// <summary>
        /// Find the first offset in the string that might contain the characters
        /// in `needle`, in any order. Returns -1 if not found.
        /// <para>This function can return false positives</para>
        /// </summary>
        public static bool FindCluster(this string haystack, string needle)
        {
            if (haystack == null) return false;
            if (needle == null) return false;

            if (haystack.Length < needle.Length) return false;

            long sum = needle.ToCharArray().Sum(c => c);
            long rolling = haystack.ToCharArray().Take(needle.Length).Sum(c => c);

            var idx = 0;
            var head = needle.Length;
            while (rolling != sum)
            {
                if (head >= haystack.Length) return false;
                rolling -= haystack[idx];
                rolling += haystack[head];
                head++;
                idx++;
            }

            return true;
        }

        /// <summary>
        /// Remap from 1 range to another
        /// </summary>
        public static float Remap(this float from, float fromMin, float fromMax, float toMin, float toMax)
        {
            var fromAbs = from - fromMin;
            var fromMaxAbs = fromMax - fromMin;

            var normal = fromAbs / fromMaxAbs;

            var toMaxAbs = toMax - toMin;
            var toAbs = toMaxAbs * normal;

            var to = toAbs + toMin;

            return to;
        }

        /// <summary>
        /// Remap from 1 range to another
        /// </summary>
        public static double Remap(this double from, double fromMin, double fromMax, double toMin, double toMax)
        {
            var fromAbs = from - fromMin;
            var fromMaxAbs = fromMax - fromMin;

            var normal = fromAbs / fromMaxAbs;

            var toMaxAbs = toMax - toMin;
            var toAbs = toMaxAbs * normal;

            var to = toAbs + toMin;

            return to;
        }

        public static string StreamToString(Stream stream)
        {
            StreamReader reader = new StreamReader(stream);
            string text = reader.ReadToEnd();

            return text;
        }

        /// <summary>
        /// Converts a timezone (+08:00) in string form to parsed timespan 
        /// </summary>
        public static TimeSpan StringToTimezone(string timezoneRaw)
        {
            return DateTimeOffset.ParseExact(timezoneRaw, "zzz", CultureInfo.InvariantCulture).Offset;
        }

        /// <summary>
        /// Returns system timezone offset as TimeSpan
        /// </summary>
        public static string GetSystemTimezoneStr() => DateTimeOffset.Now.ToString("zzz");

        /// <summary>
        /// Returns system timezone offset as TimeSpan
        /// </summary>
        public static TimeSpan GetSystemTimezone() => DateTimeOffset.Now.Offset;

        /// <summary>
        /// Given a place's name, using VedAstro API will get location
        /// via HTTP request
        /// </summary>
        public static async Task<WebResult<GeoLocation>> AddressToGeoLocation(string address)
        {

            //CACHE MECHANISM
            return await CacheManager.GetCache(new CacheKey("Tools.AddressToGeoLocation", address), addressToGeoLocation);

            async Task<WebResult<GeoLocation>> addressToGeoLocation()
            {
                //get location data from VedAstro API
                var allUrls = new URL(ThisAssembly.BranchName.Contains("beta")); //todo clean up
                //exp : .../Calculate/AddressToGeoLocation/London
                var url = allUrls.AddressToGeoLocationAPI + $"/Address/{address}";
                var webResult = await Tools.ReadFromServerJsonReplyVedAstro(url);

                //if fail to make call, end here
                if (!webResult.IsPass) { return new WebResult<GeoLocation>(false, GeoLocation.Empty); }

                //if success, get the reply data out
                var rootJson = webResult.Payload;
                var parsed = GeoLocation.FromJson(rootJson);

                //return to caller pass
                return new WebResult<GeoLocation>(true, parsed);

            }

        }

        /// <summary>
        /// Given a location & time, will use Google Timezone API
        /// to get accurate time zone that was/is used
        /// Must input valid geolocation 
        /// NOTE:
        /// - offset of timeAtLocation not important
        /// - googleGeoLocationApiKey needed to work
        /// </summary>
        public static async Task<TimeSpan> GetTimezoneOffset(string locationName, DateTimeOffset timeAtLocation)
        {
            //get geo location first then call underlying method
            var geoLocation = await GeoLocation.FromName(locationName);
            return Tools.StringToTimezone(await GetTimezoneOffsetApi(geoLocation, timeAtLocation));
        }

        public static async Task<string> GetTimezoneOffsetString(string locationName, DateTime timeAtLocation)
        {
            //get geo location first then call underlying method
            var geoLocation = await GeoLocation.FromName(locationName);
            return await GetTimezoneOffsetApi(geoLocation, timeAtLocation);
        }

        public static async Task<string> GetTimezoneOffsetString(string location, string dateTime)
        {
            //get timezone from Google API
            var lifeEvtTimeNoTimezone = DateTime.ParseExact(dateTime, Time.DateTimeFormatNoTimezone, null);
            var timezone = await Tools.GetTimezoneOffsetString(location, lifeEvtTimeNoTimezone);

            return timezone;

            //get start time of life event and find the position of it in slices (same as now line)
            //so that this life event line can be placed exactly on the report where it happened
            //var lifeEvtTimeStr = $"{dateTime} {timezone}"; //add offset 0 only for parsing, not used by API to get timezone
            //var lifeEvtTime = DateTimeOffset.ParseExact(lifeEvtTimeStr, Time.DateTimeFormat, null);

            //return lifeEvtTime;
        }

        /// <summary>
        /// backup approximate non historic timezone calculator
        /// </summary>
        public static string GetTimezoneOffsetLocal(GeoLocation geoLocation, DateTime time)
        {
            // Calculate the timezone offset from the coordinates
            var offset = CalculateTimeZoneOffset(geoLocation.Latitude(), geoLocation.Longitude());

            // Format the offset as a string
            var hours = (int)offset;
            var minutes = (int)((offset - hours) * 60);
            var offsetString = $"{(offset >= 0 ? "+" : "-")}{Math.Abs(hours):D2}:{Math.Abs(minutes):D2}";

            return offsetString;
        }

        /// <summary>
        /// backup approximate non historic timezone calculator
        /// </summary>
        private static double CalculateTimeZoneOffset(double latitude, double longitude)
        {
            // This is a simplified implementation that assumes a linear relationship
            // between longitude and timezone offset. In reality, the relationship is
            // more complex and depends on the country and region.

            // Adjust the longitude to the range [-180, 180]
            longitude = (longitude + 180) % 360 - 180;

            // Calculate the offset in hours
            var offset = longitude / 15;

            return offset;
        }



        /// <summary>
        /// Given a location & time, will use live/local VedAstro API server to get data
        ///  - Cached in runtime memory
        ///  - Checks API live and local during debug mode
        ///  - Uses backup low accuracy timezone if all else fails (via api)
        /// </summary>
        public static async Task<WebResult<string>> GetTimezoneOffsetApi(GeoLocation geoLocation, DateTimeOffset timeAtLocation)
        {
            //CACHE MECHANISM
            return await CacheManager.GetCache(new CacheKey("Tools.GetTimezoneOffsetApi", geoLocation, timeAtLocation), getTimezoneOffsetApi);

            async Task<WebResult<string>> getTimezoneOffsetApi()
            {
                try
                {
                    //get location data from VedAstro API
                    var allUrls = new URL(ThisAssembly.BranchName.Contains("beta")); //todo clean up

                    //call url must be correct format
                    //.../Calculate/GeoLocationToTimezone/Location/Tokyo, Japan/Coordinates/35.65,139.83/Time/14:02/09/11/1977/+00:00
                    var url = allUrls.GeoLocationToTimezoneAPI + geoLocation.ToUrl() + timeAtLocation.ToUrl();

                    //make call to Vedastro Live/Local API
                    //NOTE: when running from python lib will default to live server
                    var webResult = await Tools.ReadFromServerJsonReplyVedAstro(url);

                    //if fail to make call, end here
                    if (!webResult.IsPass) { return new WebResult<string>(false, ""); }

                    //if success, get the reply data out
                    var box = webResult.Payload["GeoLocationToTimezone"];
                    var data = box.Value<string>();

                    //return to caller pass
                    return new WebResult<string>(true, data);

                }
                catch (Exception e)
                {
                    LibLogger.Debug(e);
                    //return to caller pass
                    return new WebResult<string>(false, "");
                }
            }
        }

        /// <summary>
        /// Given a timespan instance converts to string timezone +08:00
        /// </summary>
        public static string TimeSpanToUTCTimezoneString(TimeSpan offsetMinutes)
        {
            var x = DateTimeOffset.UtcNow.ToOffset(offsetMinutes).ToString("zzz");
            return x;
        }


        /// <summary>
        /// Calls a URL and returns the content of the result as XML
        /// Even if content is returned as JSON, it is converted to XML
        /// Note:
        /// - if JSON auto adds "Root" as first element, unless specified
        /// for XML data root element name is ignored
        /// </summary>
        public static async Task<WebResult<XElement>> ReadFromServerXmlReply(string apiUrl)
        {
            //send request to API server
            var result = await RequestServer(apiUrl);

            //get raw reply from the server response
            var rawMessage = await result.Content?.ReadAsStringAsync() ?? "";

            //only good reply from server is accepted, anything else is marked invalid
            //stops invalid replies from being passed as valid
            if (!result.IsSuccessStatusCode) { return new WebResult<XElement>(false, new("RawErrorData", rawMessage)); }

            //tries to parse the raw data received into XML or JSON
            //if all fail will return raw data with fail status
            var parsed = ParseData(rawMessage);


            return parsed;


            //----------------------------------------------------------
            // FUNCTIONS

            WebResult<XElement> ParseData(string inputRawString)
            {
                var exceptionList = new List<Exception>();

                try
                {
                    //OPTION 1 : xml with VedAstro standard reply
                    var parsedXml = XElement.Parse(inputRawString);
                    var returnVal = WebResult<XElement>.FromXml(parsedXml);
                    //if can't parse, raise exception so can check other methods
                    if (!returnVal.IsPass) { throw new InvalidOperationException(); }
                    return returnVal;
                }
                catch (Exception e1)
                {
                    try
                    {
                        //OPTION 2 : xml 3rd party reply (google)
                        var parsedXml = XElement.Parse(inputRawString);
                        return new WebResult<XElement>(true, parsedXml);
                    }
                    catch (Exception e2) { exceptionList.Add(e2); }

                    try
                    {
                        //OPTION 3 : json 3rd party reply
                        var parsedJson = JsonConvert.DeserializeXmlNode(inputRawString, "LocationData");
                        var wrappedXml = XElement.Parse(parsedJson.InnerXml); //expected to fail if not right
                        return new WebResult<XElement>(true, wrappedXml);
                    }
                    catch (Exception e3) { exceptionList.Add(e3); } //if fail just void print

                    exceptionList.Add(e1);

                    //send all exception data to server
                    foreach (var exception in exceptionList) { LibLogger.Debug(exception, inputRawString); }

                    //if control reaches here all has failed
                    return new WebResult<XElement>(false, new XElement("Failed"));
                }
            }

            async Task<HttpResponseMessage> RequestServer(string receiverAddress)
            {
                //prepare the data to be sent
                var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, receiverAddress);


                //get the data sender 
                using var client = new HttpClient();
                client.Timeout = new TimeSpan(0, 0, 0, 0, Timeout.Infinite); //no timeout

                //tell sender to wait for complete reply before exiting
                var waitForContent = HttpCompletionOption.ResponseContentRead;

                //send the data on its way
                var response = await client.SendAsync(httpRequestMessage, waitForContent);

                //return the raw reply to caller
                return response;
            }

        }

        /// <summary>
        /// Makes GET request to given URL, tries 3 times before fail
        /// Handles all JSON replies from VedAstro format, or raw JSON format
        /// </summary>
        public static async Task<WebResult<JToken>> ReadFromServerJsonReply(string apiUrl)
        {
            var exceptionList = new List<Exception>();

            //send request to API server
            var result = await RequestServer(apiUrl, 3);

            //get raw reply from the server response
            var rawMessage = await result.Content?.ReadAsStringAsync() ?? "";

            //only good reply from server is accepted, anything else is marked invalid
            //stops invalid replies from being passed as valid
            if (!result.IsSuccessStatusCode) { var json = new JObject(); json.Add("RawErrorData", rawMessage); return new WebResult<JToken>(false, json); }

            //tries to parse the raw data received into XML or JSON
            //if all fail will return raw data with fail status
            var parsed = ParseData(rawMessage);


            return parsed;


            //----------------------------------------------------------
            // FUNCTIONS

            WebResult<JToken> ParseData(string inputRawString)
            {
                try
                {
                    //OPTION 1 : json 3rd party reply
                    var parsedJson = JObject.Parse(inputRawString);
                    return new WebResult<JToken>(true, parsedJson);
                }
                catch (Exception e1)
                {
                    LibLogger.Debug(e1, inputRawString);

                    //if control reaches here all has failed
                    return new WebResult<JToken>(false, new JObject("Failed"));
                }

            }

            //note uses GET request
            //tries several times, note if all tries fail will return null
            //small delay between calls
            async Task<HttpResponseMessage> RequestServer(string receiverAddress, int retryCount)
            {
                var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, receiverAddress);

                using var client = new HttpClient() { Timeout = new TimeSpan(0, 0, 0, 0, Timeout.Infinite) }; //no timeout
                var waitForContent = HttpCompletionOption.ResponseContentRead;

                for (int attempt = 0; attempt < retryCount; attempt++)
                {
                    try
                    {
                        var response = await client.SendAsync(httpRequestMessage, waitForContent);
                        return response;
                    }
                    catch (Exception ex) when (ex is HttpRequestException || ex is TaskCanceledException)
                    {
                        // Wait before next attempt.
                        await Task.Delay(350);
                    }
                }

                // All attempts have failed, return null.
                return null;
            }

        }

        /// <summary>
        /// Makes GET request to given URL, tries 3 times before fail
        /// Handles all JSON replies from VedAstro format, or raw JSON format
        /// </summary>
        public static async Task<WebResult<JToken>> ReadFromServerJsonReplyVedAstro(string apiUrl)
        {

            //send request to API server
            var result = await RequestServer(apiUrl, 3);

            //get raw reply from the server response
            var rawMessage = await result.Content?.ReadAsStringAsync() ?? "";

            //only good reply from server is accepted, anything else is marked invalid
            //stops invalid replies from being passed as valid
            if (!result.IsSuccessStatusCode) { var json = new JObject(); json.Add("RawErrorData", rawMessage); return new WebResult<JToken>(false, json); }

            //tries to parse the raw data received into XML or JSON
            //if all fail will return raw data with fail status
            var parsed = ParseData(rawMessage);


            return parsed;


            //----------------------------------------------------------
            // FUNCTIONS

            WebResult<JToken> ParseData(string inputRawString)
            {
                try
                {
                    //OPTION 1 : VedAstro API format
                    //make JSON data readable
                    var parsedJson = JObject.Parse(inputRawString);
                    var returnVal = WebResult<JObject>.FromVedAstroJson(parsedJson);

                    //if did not pass, raise exception so can check other methods
                    if (!returnVal.IsPass) { throw new InvalidOperationException(); }
                    return returnVal;
                }
                catch (Exception e1)
                {
                    //send all exception data to server
                    LibLogger.Debug(e1, $"URL:{apiUrl}-->{inputRawString}");

                    //if control reaches here all has failed
                    return new WebResult<JToken>(false, new JObject("Failed"));
                }

            }

            //note uses GET request
            //tries several times, note if all tries fail will return null
            //small delay between calls
            async Task<HttpResponseMessage> RequestServer(string receiverAddress, int retryCount)
            {
                var waitForContent = HttpCompletionOption.ResponseContentRead;

                using var client = new HttpClient(); //no timeout
                client.Timeout = new TimeSpan(0, 0, 0, 0, Timeout.Infinite);
                for (int attempt = 0; attempt < retryCount; attempt++)
                {
                    try
                    {
                        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, receiverAddress);

                        //copy caller data from original caller if any, so calls are traceable
                        //CurrentCallerData.AddOriginalCallerHeadersIfAny(httpRequestMessage);

                        var response = await client.SendAsync(httpRequestMessage, waitForContent);
                        return response;
                    }
                    catch (Exception ex) when (ex is HttpRequestException || ex is TaskCanceledException)
                    {
                        // Wait before next attempt.
                        await Task.Delay(350);
                    }
                }

                // All attempts have failed, return null.
                return null;
            }

        }

        /// <summary>
        /// Given a list of strings will return one by random
        /// Used to make dynamic user error & info messages
        /// </summary>
        public static string RandomSelect(string[] msgList)
        {
            // Create a Random object  
            Random rand = new Random();

            // Generate a random index less than the size of the array.  
            int randomIndexNumber = rand.Next(msgList.Length);

            //return random text from list to caller
            return msgList[randomIndexNumber];
        }

        public static int GetRandomNumber(int maxNumber)
        {
            // Create a Random object  
            Random rand = new Random();

            // Generate a random index less than the size of the array.  
            int randomIndexNumber = rand.Next(maxNumber);

            //return random text from list to caller
            return randomIndexNumber;
        }

        /// <summary>
        /// Split string by character count
        /// </summary>
        public static IEnumerable<string> SplitByCharCount(string str, int maxChunkSize)
        {
            for (int i = 0; i < str.Length; i += maxChunkSize)
                yield return str.Substring(i, Math.Min(maxChunkSize, str.Length - i));
        }

        /// <summary>
        /// Inputed event name has be space separated
        /// </summary>
        public static List<PlanetName> GetPlanetFromName(string eventName)
        {
            var returnList = new List<PlanetName>();

            //lower case it
            var lowerCased = eventName.ToLower();

            //split into words
            var splited = lowerCased.Split(' ');

            //check if any be parsed into planet name
            foreach (var word in splited)
            {
                var result = PlanetName.TryParse(word, out var planetParsed);
                if (result)
                {
                    //add list if parsed
                    returnList.Add(planetParsed);
                }
            }


            //return list to caller
            return returnList;
        }

        /// <summary>
        /// Packages the data into ready form for the HTTP client to use in final sending stage
        /// </summary>
        public static StringContent XmLtoHttpContent(XElement data)
        {
            //gets the main XML data as a string
            var dataString = Tools.XmlToString(data);

            //specify the data encoding
            var encoding = Encoding.UTF8;

            //specify the type of the data sent
            //plain text, stops auto formatting
            var mediaType = "plain/text";

            //return packaged data to caller
            return new StringContent(dataString, encoding, mediaType);
        }

        /// <summary>
        /// Packages the data into ready form for the HTTP client to use in final sending stage
        /// </summary>
        public static StringContent JsontoHttpContent(JToken data)
        {
            //gets the main XML data as a string
            var dataString = data.ToString();

            //specify the data encoding todo es mui nesasito?
            var encoding = Encoding.UTF8;

            //specify the type of the data sent
            //plain text, stops auto formatting
            var mediaType = "application/json";

            //return packaged data to caller
            return new StringContent(dataString, encoding, mediaType);
        }



        /// <summary>
        /// Extracts data from an Exception puts it in a nice JSON
        /// </summary>
        public static JObject ExceptionToJSON(Exception e)
        {
            //place to store the exception data
            string fileName;
            string methodName;
            int line;
            int columnNumber;
            string message;
            string source;

            //get the exception that started it all
            var originalException = e.GetBaseException();

            //extract the data from the error
            StackTrace st = new StackTrace(e, true);

            //Get the first stack frame
            StackFrame frame = st.GetFrame(st.FrameCount - 1);

            //Get the file name
            fileName = frame?.GetFileName();

            //Get the method name
            methodName = frame.GetMethod()?.Name;

            //Get the line number from the stack frame
            line = frame.GetFileLineNumber();

            //Get the column number
            columnNumber = frame.GetFileColumnNumber();

            message = originalException.ToString();

            source = originalException.Source;
            //todo include inner exception data
            var stackTrace = originalException.StackTrace;


            //put together the new error record
            var newRecord = new JObject(
                new JProperty("Error", new JObject(
                    new JProperty("Message", message),
                    new JProperty("Source", source),
                    new JProperty("FileName", fileName),
                    new JProperty("SourceLineNumber", line),
                    new JProperty("SourceColNumber", columnNumber),
                    new JProperty("MethodName", methodName)
                ))
            );


            return newRecord;
        }

        /// <summary>
        /// Gets now time with seconds in wrapped in xml element
        /// used for logging
        /// </summary>
        public static XElement TimeStampSystemXml => new("TimeStamp", Tools.GetNowSystemTimeSecondsText());

        /// <summary>
        /// Gets now time at server location (+8:00) with seconds in wrapped in xml element
        /// used for logging
        /// </summary>
        public static XElement TimeStampServerXml => new("TimeStampServer", Tools.GetNowServerTimeSecondsText());

        /// <summary>
        /// Converts any Enum from URL epx : ../EnumName/EnumValue
        /// NOTE: Handles special SimpleAyanamsa enum as Ayanamsa
        /// </summary>
        public static object EnumFromUrl(string url)
        {
            // Constants for namespace and special enum name
            const string Namespace = "VedAstro.Library.";
            const string SpecialEnumName = "Ayanamsa";

            // Split the URL into parts
            string[] parts = url.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

            // If there are less than 2 parts, the URL is invalid
            if (parts.Length < 2) { throw new ArgumentException("Invalid URL format.", nameof(url)); }

            // Get the enum name and value from the URL parts
            var enumName = parts[0];
            var enumValue = parts[1];

            // Get the type of the enum from the namespace and enum name
            var enumType = Type.GetType(Namespace + enumName);

            // If the enum type is null, it was not found in the namespace
            if (enumType == null) { throw new ArgumentException($"Enum type '{enumName}' not found in namespace '{Namespace}'.", nameof(url)); }

            try
            {
                // Try to parse the enum value
                return Enum.Parse(enumType, enumValue);
            }
            catch (ArgumentException)
            {
                // If parsing failed and the enum name is "Ayanamsa", try to parse as a SimpleAyanamsa enum
                if (enumName == SpecialEnumName)
                {
                    return Enum.Parse(typeof(SimpleAyanamsa), enumValue);
                }
                // If parsing as SimpleAyanamsa also failed, try to parse the value as a double
                if (double.TryParse(enumValue, out var number))
                {
                    return number;
                }

                // If all parsing attempts failed, rethrow the original exception
                throw;
            }
        }

        public static object EnumFromUrl(string url, Type enumType)
        {
            // Constants for namespace and special enum name
            const string Namespace = "VedAstro.Library.";
            const string SpecialEnumName = "Ayanamsa";

            // Split the URL into parts
            string[] parts = url.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

            // If there are less than 2 parts, the URL is invalid
            if (parts.Length < 2) { throw new ArgumentException("Invalid URL format.", nameof(url)); }

            // Get the enum name and value from the URL parts
            var paramName = parts[0];
            var enumValue = parts[1];

            // Get the type of the enum from the namespace and enum name
            //var enumType = Type.GetType(Namespace + enumName);

            // If the enum type is null, it was not found in the namespace
            if (enumType == null) { throw new ArgumentException($"Enum type '{paramName}' not found in namespace '{Namespace}'.", nameof(url)); }

            try
            {
                // Try to parse the enum value
                return Enum.Parse(enumType, enumValue);
            }
            catch (ArgumentException)
            {
                // If parsing failed and the enum name is "Ayanamsa", try to parse as a SimpleAyanamsa enum
                if (paramName == SpecialEnumName)
                {
                    return Enum.Parse(typeof(SimpleAyanamsa), enumValue);
                }
                // If parsing as SimpleAyanamsa also failed, try to parse the value as a double
                if (double.TryParse(enumValue, out var number))
                {
                    return number;
                }

                // If all parsing attempts failed, rethrow the original exception
                throw;
            }
        }

        /// <summary>
        /// Converts any String
        /// </summary>

        /// <summary>
        /// Extracts the string value from a given URL.
        /// from URL exp : ../PersonName/Juliet 
        /// </summary>
        /// <param name="url">The input URL from which to extract the string value.</param>
        /// <returns>The extracted string value from the URL, or null if the URL is null or malformed.</returns>
        public static string StringFromUrl(string url)
        {
            // Return null if the input URL is null or empty
            if (string.IsNullOrEmpty(url)) { return null; }

            // Split the URL into parts based on the '/' character, ignoring empty entries
            string[] parts = url.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

            //must be minimum two elements in string, data & value
            if (parts.Length < 2) { return ""; }

            // Get the second part of the URL as the string value
            var stringValue = parts[1];

            return stringValue; // Return the extracted string value
        }

        /// <summary>
        /// Converts any Int from URL epx : ../Number/5
        /// </summary>

        public static int IntFromUrl(string url)
        {
            string[] parts = url.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

            //string has simple structure ../Number/5
            var stringValue = parts[1];

            var intFromUrl = int.Parse(stringValue);

            return intFromUrl;
        }

        public static double DoubleFromUrl(string url)
        {
            string[] parts = url.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

            //string has simple structure ../Number/5
            var stringValue = parts[1];

            var intFromUrl = double.Parse(stringValue);

            return intFromUrl;
        }

        /// <summary>
        /// .../Time/14:02/09/11/1977/+00:00
        /// </summary>
        public static DateTimeOffset DateTimeOffsetFromUrl(string timeUrl)
        {
            //convert from URL format to standard Vedastro time string format
            //              0     1   2  3   4    5   
            // INPUT -> "/Time/23:59/31/12/2000/+08:00/"
            string[] parts = timeUrl.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            var timeStr = $"{parts[1]} {parts[2]}/{parts[3]}/{parts[4]} {parts[5]}";

            //if fail raise alarm
            var timeInputPassed = Time.TryParseStd(timeStr, out var parsedSTDInputTime);
            if (!timeInputPassed) { throw new Exception("Failed to get timezone!"); }

            return parsedSTDInputTime;
        }

        /// <summary>
        /// Convert current instance of DateTimeOffset for use in OpenAPI URL
        /// EXP: .../Time/14:02/09/11/1977/+00:00
        /// </summary>
        public static string ToUrl(this DateTimeOffset dateTimeOffset)
        {

            var part1 = dateTimeOffset.ToString("HH:mm");
            var part2 = dateTimeOffset.ToString("dd/MM/yyyy");
            var part3 = dateTimeOffset.ToString("zzz"); //timezone separate so can clean date time

            //god knows why, in some time zones date comes with "." instead of "/" (despite above formatting)
            part2 = part2.Replace('.', '/');

            //god knows why, in some time zones date comes with "-" instead of "/" (despite above formatting)
            part2 = part2.Replace('-', '/');

            var url = $"/Time/{part1}/{part2}/{part3}";
            return url;
        }


        private static readonly Random Random = new Random();

        /// <summary>
        /// we get all the values of the enum using Enum.GetValues,
        /// generate a random index, and return the enum value at that index
        /// </summary>
        public static object GetRandomEnumValue(Type enumType)
        {
            if (!enumType.IsEnum)
            {
                throw new ArgumentException("Type must be an enumeration");
            }
            var values = Enum.GetValues(enumType);
            var index = Random.Next(values.Length);
            return values.GetValue(index);
        }

        /// <summary>
        /// Gets now time in UTC +8:00
        /// Because server time is uncertain, all change to UTC8
        /// </summary>
        public static string GetNow()
        {
            //create utc 8
            var utc8 = new TimeSpan(8, 0, 0);
            //get now time in utc 0
            var nowTime = DateTimeOffset.Now.ToUniversalTime();
            //convert time utc 0 to utc 8
            var utc8Time = nowTime.ToOffset(utc8);

            //return converted time to caller
            return utc8Time.ToString(Time.DateTimeFormatSeconds);
        }

        /// <summary>
        /// Also converts to Title case
        /// Removes all invalid characters for an person name
        /// used to clean name field user input. Also
        /// allowed chars : periods (.) and hyphens (-), space ( )
        /// SRC:https://learn.microsoft.com/en-us/dotnet/standard/base-types/how-to-strip-invalid-characters-from-a-string
        /// </summary>
        public static string CleanAndFormatNameText(string nameInput)
        {
            // Replace invalid characters with empty strings.
            try
            {
                //remove invalid
                var cleanText = Regex.Replace(nameInput, @"[^\w\.\s*-]", "", RegexOptions.None, TimeSpan.FromSeconds(2));

                var textinfo = new CultureInfo("en-US", false).TextInfo;

                //tit le case it!, needs all small else will fail when some nut puts all as capital 
                cleanText = cleanText.ToLower(); //lower
                cleanText = textinfo.ToTitleCase(cleanText); //convert

                return cleanText;
            }
            // If we timeout when replacing invalid characters,
            // we should return Empty.
            catch (RegexMatchTimeoutException)
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Removes all invalid characters for an person name
        /// used to clean name field user input. Also
        /// allowed chars : periods (.) and hyphens (-), space ( )
        /// </summary>
        public static string CleanText(string uncleanInput)
        {
            // Replace invalid characters with empty strings.
            try
            {
                //remove invalid
                var cleanText = Regex.Replace(uncleanInput, @"[^\w\.\s*-]", "", RegexOptions.None, TimeSpan.FromSeconds(2));

                return cleanText;
            }
            // If we timeout when replacing invalid characters,
            // we should return Empty.
            catch (RegexMatchTimeoutException)
            {
                return string.Empty;
            }
        }


        /// <summary>
        /// Given a parsed XML element will convert to Json string
        /// Note:
        /// - auto removes Root from XML (since XML needs it & JSON does not)
        /// - this light weight only uses Newtownsoft
        /// - Newtownsoft here because the converter is better than .net's
        /// </summary>
        public static string XmlToJsonString(XElement xElement)
        {
            //no XML indent
            var finalXml = xElement.ToString(SaveOptions.DisableFormatting);

            //convert to JSON
            XmlDocument doc = new XmlDocument(); //NOTE: different xDOC from .Net's
            doc.LoadXml(finalXml);

            //removes "Root" from xml
            string jsonText = JsonConvert.SerializeXmlNode(doc, Formatting.None, true);

            return jsonText;
        }

        ///// <summary>
        ///// Parses from XML > string > .Net JSON
        ///// NOTE:
        ///// - compute heavier than just string, use wisely
        ///// </summary>
        //public static JsonElement XmlToJson(XElement xElement)
        //{
        //    //convert xml to JSON string
        //    var jsonStr = XmlToJsonString(xElement);

        //    //convert string 
        //    using JsonDocument doc = JsonDocument.Parse(jsonStr);
        //    JsonElement root = doc.RootElement;

        //    return root;
        //}
        public static JObject XmlToJson(XElement xElement)
        {
            var x = XmlToJsonString(xElement);

            var y = JObject.Parse(x);

            return y;
        }

        /// <summary>
        /// Converts VedAstro planet name to Swiss Eph planet
        /// </summary>
        /// <returns></returns>
        public static int VedAstroToSwissEph(PlanetName planetName)
        {
            int planet = 0;

            //Convert PlanetName to SE_PLANET type
            if (planetName == PlanetName.Sun)
                planet = SwissEph.SE_SUN;
            else if (planetName == PlanetName.Moon)
            {
                planet = SwissEph.SE_MOON;
            }
            else if (planetName == PlanetName.Mars)
            {
                planet = SwissEph.SE_MARS;
            }
            else if (planetName == PlanetName.Mercury)
            {
                planet = SwissEph.SE_MERCURY;
            }
            else if (planetName == PlanetName.Jupiter)
            {
                planet = SwissEph.SE_JUPITER;
            }
            else if (planetName == PlanetName.Venus)
            {
                planet = SwissEph.SE_VENUS;
            }
            else if (planetName == PlanetName.Saturn)
            {
                planet = SwissEph.SE_SATURN;
            }
            else if (planetName == PlanetName.Earth)
            {
                planet = SwissEph.SE_EARTH;
            }
            else if (planetName == PlanetName.Rahu)
            {
                //set based on user preference
                planet = Calculate.UseMeanRahuKetu ? SwissEph.SE_MEAN_NODE : SwissEph.SE_TRUE_NODE;
            }
            else if (planetName == PlanetName.Ketu)
            {
                //NOTES:
                //the true node, which is the point where the Moon's orbit crosses the ecliptic plane
                //can also be SE_OSCU_APOG, but no need to add 180

                //set based on user preference, ask for rahu values then add 180 later
                planet = Calculate.UseMeanRahuKetu ? SwissEph.SE_MEAN_NODE : SwissEph.SE_TRUE_NODE;
            }

            return planet;
        }

        /// <summary>
        /// Converts string name of planets, all case to swiss type
        /// </summary>
        public static int StringToSwissEph(string planetName)
        {
            int planet = 0;

            //make small case, best reliability
            planetName = planetName.ToLower();

            //Convert PlanetName to SE_PLANET type
            if (planetName == "sun")
                planet = SwissEph.SE_SUN;
            else if (planetName == "moon")
            {
                planet = SwissEph.SE_MOON;
            }
            else if (planetName == "mars")
            {
                planet = SwissEph.SE_MARS;
            }
            else if (planetName == "Mercury")
            {
                planet = SwissEph.SE_MERCURY;
            }
            else if (planetName == "Jupiter")
            {
                planet = SwissEph.SE_JUPITER;
            }
            else if (planetName == "Venus")
            {
                planet = SwissEph.SE_VENUS;
            }
            else if (planetName == "Saturn")
            {
                planet = SwissEph.SE_SATURN;
            }
            else if (planetName == "Rahu")
            {
                //set based on user preference
                planet = Calculate.UseMeanRahuKetu ? SwissEph.SE_MEAN_NODE : SwissEph.SE_TRUE_NODE;
            }
            else if (planetName == "Ketu")
            {
                //NOTES:
                //the true node, which is the point where the Moon's orbit crosses the ecliptic plane
                //can also be SE_OSCU_APOG, but no need to add 180

                //set based on user preference, ask for rahu values then add 180 later
                planet = Calculate.UseMeanRahuKetu ? SwissEph.SE_MEAN_NODE : SwissEph.SE_TRUE_NODE;
            }

            return planet;
        }


        /// <summary>
        /// All possible for all celestial body types
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<string> ApiDataPropertyCallList()
        {
            var returnList = new List<string>();

            //get all possible calls for API
            //get all calculators that can work with the inputed data
            var calculatorClass = typeof(Calculate);

            foreach (var methodInfo in calculatorClass.GetMethods())
            {
                //get special API name
                returnList.Add(methodInfo.Name);
            }

            return returnList;

        }


        /// <summary>
        /// Given any string will remove the white spaces
        /// </summary>
        public static string RemoveWhiteSpace(string stringWithSpace)
        {
            var removed = string.Join("", stringWithSpace.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries));

            return removed;
        }


        /// <summary>
        /// Get all methods that is available to 2 params
        /// this is the list that will appear on the fly in API Builder dropdown
        /// </summary>
        /// <returns></returns>
        public static List<OpenAPIMetadata> GetPlanetApiCallList<T1, T2>()
        {
            //get all the same methods gotten by Open api func
            var calcList = GetCalculatorListByParam<T1, T2>();

            //extract needed data out in convenient form
            var finalList = OpenAPIMetadata.FromMethodInfoList(calcList);

            return finalList;
        }


        /// <summary>
        /// Gets a unique string to repent the methods name and signature
        /// this method has to match output with one in Generate tools
        /// </summary>
        public static string GetMethodSignature(this MethodInfo methodInfo)
        {
            var parameters = methodInfo.GetParameters();
            var parameterDescriptions = parameters.Select(param => $"{GetGenericTypeName(param.ParameterType)} {param.Name}");
            return GetMethodSignature(methodInfo.Name, parameterDescriptions);
        }

        /// <summary>
        /// EXAMPLE: VedAstro.Library.Calculate.List<Avasta> PlanetAvasta(PlanetName planetName, Time time)
        /// </summary>
        public static string GetMethodSignature(this MethodDeclarationSyntax methodDeclaration, SemanticModel semanticModel)
        {
            var parameters = methodDeclaration.ParameterList.Parameters;
            var parameterDescriptions = parameters.Select(param => $"{param.Type.ToString()} {param.Identifier.Text}");
            return GetMethodSignature(methodDeclaration.Identifier.Text, parameterDescriptions);
        }


        /// <summary>
        /// Main purpose is to get unified method signature from Code and Reflection
        /// </summary>
        public static string GetMethodSignature(string methodName, IEnumerable<string> parameterDescriptions)
        {
            //unify type names, one is coming from Reflection and another direct from C# file
            var listString = parameterDescriptions.Select(p => p.Replace("Int32", "int"));
            listString = listString.Select(p => p.Replace("Double", "double"));
            listString = listString.Select(p => p.Replace("String", "string"));
            listString = listString.Select(p => p.Replace("Boolean", "bool"));

            var methodSignature = $"{methodName}({string.Join(", ", listString)})";

            //remove all space, else won't match
            methodSignature = methodSignature.Replace(" ", "");

            return methodSignature;
        }

        public static string GetGenericTypeName(Type type)
        {
            if (!type.IsGenericType)
                return type.Name;
            var genericTypeName = type.GetGenericTypeDefinition().Name;
            // Remove the `1 at the end of the name
            genericTypeName = genericTypeName.Substring(0, genericTypeName.IndexOf('`'));
            var genericArgs = string.Join(",", type.GetGenericArguments().Select(t => t.Name).ToArray());
            var finalReturn = genericTypeName + "<" + genericArgs + ">";

            return finalReturn;
        }

        /// <summary>
        /// Searches text no caps and no space
        /// </summary>
        public static bool SearchText(this string text, string keyword)
        {
            // Remove spaces from the text and the keyword
            string textWithoutSpaces = text.Replace(" ", "");
            string keywordWithoutSpaces = keyword.Replace(" ", "");
            return textWithoutSpaces.Contains(keywordWithoutSpaces, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Gets name, params and return type as string from method info
        /// </summary>
        public static string GetAllDataAsText(this MethodInfo methodInfo)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"{methodInfo.Name}");
            sb.AppendLine($"{methodInfo.ReturnType.Name}");
            ParameterInfo[] parameters = methodInfo.GetParameters();
            foreach (ParameterInfo parameter in parameters)
            {
                sb.AppendLine($"\t{parameter.ParameterType.Name} {parameter.Name}");
            }

            var allDataAsText = sb.ToString();

            return allDataAsText;
        }

        /// <summary>
        /// gets all sample params needed to invoke method
        /// </summary>
        public static List<object> GetInitializedSampleParameters(this MethodInfo methodInfo)
        {
            List<object> parameters = new List<object>();

            try
            {

                foreach (ParameterInfo parameter in methodInfo.GetParameters())
                {
                    // Get the underlying type of the parameter
                    var parameterType = parameter.ParameterType;

                    //final sample initialized data
                    object sampleData = null;

                    //HACK to handle custom types, since can't extend class
                    if (parameterType == typeof(object)) { sampleData = new object(); }
                    if (parameterType == typeof(ZodiacSign)) { sampleData = new ZodiacSign(ZodiacName.Aquarius, Angle.FromDegrees(15)); }
                    if (parameterType == typeof(ZodiacName)) { sampleData = ZodiacName.Aquarius; }
                    if (parameterType == typeof(Time)) { sampleData = Time.StandardHoroscope(); }
                    if (parameterType == typeof(Angle)) { sampleData = Angle.Degrees180; }
                    if (parameterType == typeof(PlanetName)) { sampleData = PlanetName.Sun; }
                    if (parameterType == typeof(Constellation)) { sampleData = new Constellation(1, 1, Angle.FromDegrees(13)); }
                    if (parameterType == typeof(Person)) { sampleData = new Person("101", "12312323", "Juliet", Time.StandardHoroscope(), Gender.Female); }
                    if (parameterType == typeof(HouseName)) { sampleData = HouseName.House4; }
                    if (parameterType == typeof(TimeSpan)) { sampleData = new TimeSpan(1, 0, 0); }
                    if (parameterType == typeof(List<HouseName>)) { sampleData = new List<HouseName>() { HouseName.House1, HouseName.House4 }; }
                    if (parameterType == typeof(List<PlanetName>)) { sampleData = new List<PlanetName>() { PlanetName.Moon, PlanetName.Mars }; }
                    if (parameterType == typeof(int)) { sampleData = 5; }
                    if (parameterType == typeof(double)) { sampleData = 2415018.5; } //julian days
                    if (parameterType == typeof(string)) { sampleData = "sun"; }
                    if (parameterType == typeof(bool)) { sampleData = false; }
                    if (parameterType == typeof(DateTimeOffset)) { sampleData = DateTimeOffset.Now; }
                    if (parameterType == typeof(PlanetName[])) { sampleData = new[] { PlanetName.Mars, PlanetName.Jupiter }; }
                    if (parameterType == typeof(int[])) { sampleData = new[] { 3, 5 }; }
                    if (parameterType == typeof(Dictionary<PlanetName, Shashtiamsa>))
                    {
                        sampleData = new Dictionary<PlanetName, Shashtiamsa>()
                    {
                        { PlanetName.Sun, new Shashtiamsa(103.244) },
                        { PlanetName.Moon, new Shashtiamsa(195.338) },
                        { PlanetName.Mars, new Shashtiamsa(28.665) },
                        { PlanetName.Mercury, new Shashtiamsa(191.879) },
                        { PlanetName.Jupiter, new Shashtiamsa(210.36) },
                        { PlanetName.Venus, new Shashtiamsa(117.7177) },
                        { PlanetName.Saturn, new Shashtiamsa(114.849) },
                    };
                    }

                    //if not found then probably Enum, so use special Enum converter
                    if (sampleData == null) { sampleData = Tools.GetRandomEnumValue(parameterType); }

                    parameters.Add(sampleData);
                }

                return parameters;

            }
            catch (Exception e)
            {
                Console.WriteLine($"Error when getting sample for : {methodInfo.Name}");

                //if fail means type not specified and no sample
                return parameters;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        public static string ToRowKey(this DateTimeOffset dateTimeOffset)
        {
            //get time in standard format without timezone
            var rawString = dateTimeOffset.ToString(Time.DateTimeFormat);

            //convert all spaces to hyphens
            var cleaned1 = rawString.Replace('/', '-');

            //remove all spaces into hypes
            //var cleaned2 = cleaned1.Replace(' ', '-');

            return cleaned1;
        }

        /// <summary>
        /// Gets input params of methods nicely formatted string for display
        /// </summary>
        public static List<string> GetParametersStringList(this MethodInfo methodInfo)
        {
            List<string> parameters = new List<string>();
            foreach (ParameterInfo parameter in methodInfo.GetParameters())
            {
                parameters.Add($"{parameter.ParameterType.Name}");
            }
            return parameters;
        }

        /// <summary>
        /// Gets calculators by param type and count
        /// Gets all calculated data in nice JSON with matching param signature
        /// used to create a dynamic API call list
        /// </summary>
        public static JObject ExecuteCalculatorByParam<T1, T2>(T1 inputedPram1, T2 inputedPram2)
        {
            //get reference to all the calculators that can be used with the inputed param types
            var finalList = GetCalculatorListByParam<T1, T2>();

            //sort alphabetically so easier to eye data point
            var aToZOrder = finalList.OrderBy(method => method.Name).ToList();


            //place the data from all possible methods nicely in JSON
            var rootPayloadJson = new JObject(); //each call below adds to this root
            object[] paramList = new object[] { inputedPram1, inputedPram2 };
            foreach (var methodInfo in aToZOrder)
            {
                var resultParse1 = ExecuteAPICalculator(methodInfo, paramList);
                //done to get JSON formatting right
                var resultParse2 = JToken.FromObject(resultParse1); //jprop needs to be wrapped in JToken
                rootPayloadJson.Add(resultParse2);
            }

            return rootPayloadJson;

        }

        public static JObject ExecuteCalculatorByParam<T1>(T1 inputedPram1)
        {
            //get reference to all the calculators that can be used with the inputed param types
            var finalList = GetCalculatorListByParam<T1>();

            //sort alphabetically so easier to eye data point
            var aToZOrder = finalList.OrderBy(method => method.Name).ToList();


            //place the data from all possible methods nicely in JSON
            var rootPayloadJson = new JObject(); //each call below adds to this root
            object[] paramList = new object[] { inputedPram1 };
            foreach (var methodInfo in aToZOrder)
            {
                var resultParse1 = ExecuteAPICalculator(methodInfo, paramList);
                //done to get JSON formatting right
                var resultParse2 = JToken.FromObject(resultParse1); //jprop needs to be wrapped in JToken
                rootPayloadJson.Add(resultParse2);
            }

            return rootPayloadJson;

        }


        /// <summary>
        /// Given an API name, will find the calc and try to call and wrap it in JSON
        /// </summary>
        public static JProperty ExecuteCalculatorByApiName<T1, T2>(string methodName, T1 param1, T2 param2)
        {
            var calculatorClass = typeof(Calculate);
            var foundMethod = calculatorClass.GetMethods().Where(x => x.Name == methodName).FirstOrDefault();

            //if method not found, possible outdated API call link, end call here
            if (foundMethod == null)
            {
                //let caller know that method not found
                var msg = $"Call not found, make sure API link is latest version : {methodName} ";
                return new JProperty(methodName, $"ERROR:{msg}");
            }

            //pass to main function
            return ExecuteCalculatorByApiName(foundMethod, param1, param2);
        }

        /// <summary>
        /// Given an API name, will find the calc and try to call and wrap it in JSON
        /// </summary>
        public static JProperty ExecuteCalculatorByApiName<T1, T2>(MethodInfo foundMethod, T1 param1, T2 param2)
        {
            //get methods 1st param
            var param1Type = foundMethod.GetParameters()[0].ParameterType;
            object[] paramOrder1 = new object[] { param1, param2 };
            object[] paramOrder2 = new object[] { param2, param1 };

            //if first param match type, then use that
            var finalParamOrder = param1Type == param1.GetType() ? paramOrder1 : paramOrder2;

#if DEBUG
            //print out which order is used more, helps to clean code
            Console.WriteLine(param1Type == param1.GetType() ? "paramOrder1" : "paramOrder2");
#endif

            //based on what type it is we process accordingly, converts better to JSON
            var rawResult = foundMethod?.Invoke(null, finalParamOrder);

            //get correct name for this method, API friendly
            var apiSpecialName = foundMethod.Name;

            //process list differently
            JProperty rootPayloadJson;
            if (rawResult is IList iList) //handles results that have many props from 1 call, exp : SwissEphemeris
            {
                //convert list to comma separated string
                var parsedList = iList.Cast<object>().ToList();
                var stringComma = Tools.ListToString(parsedList);

                rootPayloadJson = new JProperty(apiSpecialName, stringComma);
            }
            //custom JSON converter available
            else if (rawResult is IToJson iToJson)
            {
                rootPayloadJson = new JProperty(apiSpecialName, iToJson.ToJson());
            }
            //normal conversion via to string
            else
            {
                rootPayloadJson = new JProperty(apiSpecialName, rawResult?.ToString());
            }


            return rootPayloadJson;
        }

        public static JProperty ExecuteCalculatorByApiName<T1>(MethodInfo foundMethod, T1 param1)
        {
            //get methods 1st param
            var param1Type = foundMethod.GetParameters()[0].ParameterType;
            object[] paramOrder1 = new object[] { param1 };


            //based on what type it is we process accordingly, converts better to JSON
            var rawResult = foundMethod?.Invoke(null, paramOrder1);

            //get correct name for this method, API friendly
            var apiSpecialName = foundMethod.Name;

            //process list differently
            JProperty rootPayloadJson;
            if (rawResult is IList iList) //handles results that have many props from 1 call, exp : SwissEphemeris
            {
                //convert list to comma separated string
                var parsedList = iList.Cast<object>().ToList();
                var stringComma = Tools.ListToString(parsedList);

                rootPayloadJson = new JProperty(apiSpecialName, stringComma);
            }
            //custom JSON converter available
            else if (rawResult is IToJson iToJson)
            {
                rootPayloadJson = new JProperty(apiSpecialName, iToJson.ToJson());
            }
            //normal conversion via to string
            else
            {
                rootPayloadJson = new JProperty(apiSpecialName, rawResult?.ToString());
            }


            return rootPayloadJson;
        }

        /// <summary>
        /// Executes all calculators for API based on input param type only
        /// Wraps return data in JSON
        /// </summary>
        public static JProperty ExecuteAPICalculator(MethodInfo methodInfo1, object[] param)
        {

            //likely to fail during call, as such just ignore and move along
            try
            {
                JProperty outputResult;
                //execute based on param count
                if (param.Length == 1)
                {
                    outputResult = ExecuteCalculatorByApiName(methodInfo1, param[0]);
                }
                else if (param.Length == 2)
                {
                    outputResult = ExecuteCalculatorByApiName(methodInfo1, param[0], param[1]);
                }
                else
                {
                    //if not filled than not accounted for
                    throw new Exception("END OF THE LINE!");
                }


                return outputResult;
            }
            catch (Exception e)
            {
                try
                {
#if DEBUG
                    Console.WriteLine($"Trying again in reverse! {methodInfo1.Name}:\n{e.Message}\n{e.StackTrace}");
#endif
                    //try again in reverse
                    if (param.Length == 2)
                    {
                        var outputResult3 = ExecuteCalculatorByApiName(methodInfo1, param[1], param[0]);
                        return outputResult3;
                    }

                    var jsonPacked = new JProperty(methodInfo1.Name, $"ERROR: {e.Message}");
                    return jsonPacked;

                }
                //if fail put error in data for easy detection
                catch (Exception e2)
                {
                    //save it nicely in json format
                    var jsonPacked = new JProperty(methodInfo1.Name, $"ERROR: {e2.Message}");
                    return jsonPacked;
                }
            }
        }

        /// <summary>
        /// Gets all methods in Astronomical calculator that has the pram types inputed
        /// Note : also gets when order is reversed
        /// </summary>
        public static IEnumerable<MethodInfo> GetCalculatorListByParam<T1, T2>()
        {
            var inputedParamType1 = typeof(T1);
            var inputedParamType2 = typeof(T2);

            //get all calculators that can work with the inputed data
            var calculatorClass = typeof(Calculate);

            var finalList = new List<MethodInfo>();

            var calculators1 = from calculatorInfo in calculatorClass.GetMethods()
                               let parameter = calculatorInfo.GetParameters()
                               where parameter.Length == 2 //only 2 params
                                     && parameter[0].ParameterType == inputedParamType1
                                     && parameter[1].ParameterType == inputedParamType2
                               select calculatorInfo;

            finalList.AddRange(calculators1);

            //reverse order
            //second possible order, technically should be aligned todo
            var calculators2 = from calculatorInfo in calculatorClass.GetMethods()
                               let parameter = calculatorInfo.GetParameters()
                               where parameter.Length == 2 //only 2 params
                                     && parameter[0].ParameterType == inputedParamType2
                                     && parameter[1].ParameterType == inputedParamType1
                               select calculatorInfo;

            finalList.AddRange(calculators2);

#if true
            //PRINT DEBUG DATA
            Console.WriteLine($"Calculators Type 1 : {calculators1?.Count()}");
            Console.WriteLine($"Calculators Type 2 : {calculators2?.Count()}");
#endif

            return finalList;
        }


        public static IEnumerable<MethodInfo> GetCalculatorListByParam<T1>()
        {
            var inputedParamType1 = typeof(T1);

            //get all calculators that can work with the inputed data
            var calculatorClass = typeof(Calculate);

            var finalList = new List<MethodInfo>();

            var calculators1 = from calculatorInfo in calculatorClass.GetMethods()
                               let parameter = calculatorInfo.GetParameters()
                               where parameter.Length == 1 //only 2 params
                                     && parameter[0].ParameterType == inputedParamType1
                               select calculatorInfo;

            finalList.AddRange(calculators1);


#if true
            //PRINT DEBUG DATA
            Console.WriteLine($"Calculators with 1 param : {calculators1?.Count()}");
#endif

            return finalList;
        }

        /// <summary>
        /// Gets all possible API calculators from code method info
        /// used to make list to show user
        /// </summary>
        public static List<MethodInfo> GetAllApiCalculatorsMethodInfo()
        {

            //-----------NORMAL

            //get all calculators that can work with the inputed data
            var calculatorClass = typeof(Calculate);

            //fine tune, what methods gets set as calculators
            //remove auto properties methods and base methods
            var finalList = calculatorClass.GetMethods()
                .Where(m => !m.Name.StartsWith("get_") && !m.Name.StartsWith("set_") && m.DeclaringType != typeof(object))
                .ToList();


            return finalList;

        }

        /// <summary>
        /// Given a type will convert to json
        /// used for parsing results from all OPEN API calcs
        /// </summary>
        public static JToken AnyToJSON(string dataName, dynamic anyTypeData)
        {
            //process list differently
            JProperty rootPayloadJson = null;

            switch (anyTypeData)
            {
                case JObject jObject:
                    rootPayloadJson = new JProperty(dataName, jObject);
                    break;

                case List<House> dictionary:
                    {
                        var array = new JArray();
                        foreach (var item in dictionary)
                        {
                            var obj = new JObject
                        {
                            { "House", item.GetHouseName().ToString() },
                            { "Begin", item.GetBeginLongitude().ToString() },
                            { "Mid", item.GetMiddleLongitude().ToString() },
                            { "End", item.GetEndLongitude().ToString() }
                        };
                            array.Add(obj);
                        }

                        rootPayloadJson = new JProperty(dataName, array);
                        break;
                    }
                case List<Tuple<Time, Time, ZodiacName, PlanetName>> dictionary:
                    {
                        var array = new JArray();
                        foreach (var item in dictionary)
                        {
                            var obj = new JObject
                        {
                            { "Start", item.Item1.ToJson() },
                            { "End", item.Item2.ToJson() },
                            { "ZodiacSign", item.Item3.ToString() },
                            { "Planet", item.Item4.ToString() }
                        };
                            array.Add(obj);
                        }

                        rootPayloadJson = new JProperty(dataName, array);
                        break;
                    }

                //handles results that have many props from 1 call, exp : SwissEphemeris
                case List<APIFunctionResult> apiList:
                    {
                        //converts into JSON list with property names
                        //NOTE: uses back this AnyToJSON to convert nested types
                        var parsed = APIFunctionResult.ToJsonList(apiList);
                        rootPayloadJson = new JProperty(dataName, parsed);
                        return rootPayloadJson;
                    }

                case List<HoroscopePrediction> apiList:
                    {
                        var parsed = HoroscopePrediction.ToJsonList(apiList);
                        return parsed;
                    }

                case List<PlanetName> planetList:
                    {
                        var parsed = PlanetName.ToJsonList(planetList);
                        rootPayloadJson = new JProperty(dataName, parsed);
                        return rootPayloadJson;
                    }

                case List<JObject> jObjectList:
                    {

                        var parsed = ListToJson(jObjectList);
                        rootPayloadJson = new JProperty(dataName, parsed);

                        return rootPayloadJson;
                    }

                case List<DasaEvent> dasaEventList:
                    {
                        var parsed = DasaEvent.ToJsonList(dasaEventList);
                        rootPayloadJson = new JProperty(dataName, parsed);

                        return rootPayloadJson;
                    }
                case List<GeoLocation> geolocationList:
                    {
                        var parsed = GeoLocation.ToJsonList(geolocationList);
                        rootPayloadJson = new JProperty(dataName, parsed);

                        return rootPayloadJson;
                    }
                case IList iList:
                    {
                        //convert list to comma separated string
                        var parsedList = iList.Cast<object>().ToList();
                        var stringComma = Tools.ListToString(parsedList);

                        rootPayloadJson = new JProperty(dataName, stringComma);
                        break;
                    }

                //handles results that have many props from 1 call, exp : SwissEphemeris
                case Dictionary<PlanetName, ZodiacSign> dictionary: rootPayloadJson = RootPayloadJson("Planet", dataName, dictionary); break;
                case Dictionary<PlanetName, Angle> dictionary: rootPayloadJson = RootPayloadJson("Planet", dataName, dictionary); break;
                case Dictionary<PlanetName, PlanetName> dictionary: rootPayloadJson = RootPayloadJson("Planet", dataName, dictionary); break;
                case Dictionary<PlanetName, ZodiacName> dictionary: rootPayloadJson = RootPayloadJson("Planet", dataName, dictionary); break;
                case Dictionary<PlanetName, Constellation> dictionary: rootPayloadJson = RootPayloadJson("Planet", dataName, dictionary); break;
                case Dictionary<PlanetName, HouseName> dictionary: rootPayloadJson = RootPayloadJson("Planet", dataName, dictionary); break;

                case Dictionary<HouseName, Angle> dictionary: rootPayloadJson = RootPayloadJson("House", dataName, dictionary); break;
                case Dictionary<HouseName, ZodiacSign> dictionary: rootPayloadJson = RootPayloadJson("House", dataName, dictionary); break;
                case Dictionary<HouseName, PlanetName> dictionary: rootPayloadJson = RootPayloadJson("House", dataName, dictionary); break;
                case Dictionary<HouseName, ZodiacName> dictionary: rootPayloadJson = RootPayloadJson("House", dataName, dictionary); break;
                case Dictionary<HouseName, Constellation> dictionary: rootPayloadJson = RootPayloadJson("House", dataName, dictionary); break;
                case Dictionary<string, object> dictionary:
                    {
                        //cast to correct type
                        //var array = new JArray();

                        // Assuming you have a list of JProperty
                        List<JProperty> properties = new();

                        foreach (var item in dictionary)
                        {
                            var allItemKeyName = item.Key.ToString();
                            var apiFunctionResults = item.Value;
                            var ccc = Tools.AnyToJSON(allItemKeyName, apiFunctionResults);

                            properties.Add((JProperty)ccc);
                        }


                        // To place them into a JArray, you can use the following code:
                        JArray jArray = new JArray();
                        foreach (var property in properties)
                        {
                            // Create a JObject from the JProperty and add it to the JArray
                            JObject jObject = new JObject(property);
                            jArray.Add(jObject);
                        }

                        rootPayloadJson = new JProperty(dataName, jArray);
                        break;
                    }
                case Dictionary<HouseName, IList> dictionary:
                    {

                        var array = new JArray();
                        foreach (var item in dictionary)
                        {
                            var obj = new JObject
                        {
                            { "House", item.Key.ToString() },
                            { dataName,  Tools.ListToString((List<IList>)item.Value) }
                        };
                            array.Add(obj);
                        }

                        rootPayloadJson = new JProperty(dataName, array);
                        break;

                    }

                case Dictionary<ZodiacName, int> dictionary:
                    {
                        //convert list to comma separated string
                        var parsedList = dictionary.Cast<object>().ToList();
                        var stringComma = Tools.ListToString(parsedList);

                        rootPayloadJson = new JProperty(dataName, stringComma);
                        break;
                    }
                //custom JSON converter available
                case IToJson iToJson:
                    rootPayloadJson = new JProperty(dataName, iToJson.ToJson());
                    break;
                //normal conversion via "ToString"
                default:
                    rootPayloadJson = new JProperty(dataName, anyTypeData?.ToString());
                    break;
            }

            return rootPayloadJson;

        }

        public static DataTable AnyToDataTable(string dataName, dynamic anyTypeData)
        {
            //process list differently
            var table = new DataTable();

            switch (anyTypeData)
            {
                //case JObject jObject:
                //    rootPayloadJson = new JProperty(dataName, jObject);
                //    break;

                //case List<House> dictionary:
                //    {
                //        var array = new JArray();
                //        foreach (var item in dictionary)
                //        {
                //            var obj = new JObject
                //        {
                //            { "House", item.GetHouseName().ToString() },
                //            { "Begin", item.GetBeginLongitude().ToString() },
                //            { "Mid", item.GetMiddleLongitude().ToString() },
                //            { "End", item.GetEndLongitude().ToString() }
                //        };
                //            array.Add(obj);
                //        }

                //        rootPayloadJson = new JProperty(dataName, array);
                //        break;
                //    }
                //case List<Tuple<Time, Time, ZodiacName, PlanetName>> dictionary:
                //    {
                //        var array = new JArray();
                //        foreach (var item in dictionary)
                //        {
                //            var obj = new JObject
                //        {
                //            { "Start", item.Item1.ToJson() },
                //            { "End", item.Item2.ToJson() },
                //            { "ZodiacSign", item.Item3.ToString() },
                //            { "Planet", item.Item4.ToString() }
                //        };
                //            array.Add(obj);
                //        }

                //        rootPayloadJson = new JProperty(dataName, array);
                //        break;
                //    }

                ////handles results that have many props from 1 call, exp : SwissEphemeris
                //case List<APIFunctionResult> apiList:
                //    {
                //        //converts into JSON list with property names
                //        //NOTE: uses back this AnyToJSON to convert nested types
                //        var parsed = APIFunctionResult.ToJsonList(apiList);
                //        rootPayloadJson = new JProperty(dataName, parsed);
                //        return rootPayloadJson;
                //    }

                case List<HoroscopePrediction> apiList:
                    {
                        table.Columns.Add("Column1", typeof(string));
                        table.Columns.Add("Column2", typeof(string));
                        table.Columns.Add("Column3", typeof(string));
                        //add in rows
                        foreach (var item in apiList)
                        {
                            table.Rows.Add(item.ToDataRow());
                        }
                        break;
                    }

                //case List<PlanetName> planetList:
                //    {
                //        var parsed = PlanetName.ToJsonList(planetList);
                //        rootPayloadJson = new JProperty(dataName, parsed);
                //        return rootPayloadJson;
                //    }

                //case List<JObject> jObjectList:
                //    {

                //        var parsed = ListToJson(jObjectList);
                //        rootPayloadJson = new JProperty(dataName, parsed);

                //        return rootPayloadJson;
                //    }

                //case List<DasaEvent> dasaEventList:
                //    {
                //        var parsed = DasaEvent.ToJsonList(dasaEventList);
                //        rootPayloadJson = new JProperty(dataName, parsed);

                //        return rootPayloadJson;
                //    }

                case IList iList:
                    {
                        //add in rows
                        foreach (var item in iList)
                        {
                            table.Rows.Add(item.ToString());
                        }
                        break;
                    }

                //handles results that have many props from 1 call, exp : SwissEphemeris
                case Dictionary<PlanetName, ZodiacSign> dictionary: DictionaryToDataTable(dictionary, table); break;
                case Dictionary<PlanetName, Angle> dictionary: DictionaryToDataTable(dictionary, table); break;
                case Dictionary<PlanetName, PlanetName> dictionary: DictionaryToDataTable(dictionary, table); break;
                case Dictionary<PlanetName, ZodiacName> dictionary: DictionaryToDataTable(dictionary, table); break;
                case Dictionary<PlanetName, Constellation> dictionary: DictionaryToDataTable(dictionary, table); break;
                case Dictionary<PlanetName, HouseName> dictionary: DictionaryToDataTable(dictionary, table); break;

                case Dictionary<string, object> dictionary: DictionaryToDataTable(dictionary, table); break;
                case Dictionary<HouseName, Angle> dictionary: DictionaryToDataTable(dictionary, table); break;
                case Dictionary<HouseName, ZodiacSign> dictionary: DictionaryToDataTable(dictionary, table); break;
                case Dictionary<HouseName, PlanetName> dictionary: DictionaryToDataTable(dictionary, table); break;
                case Dictionary<HouseName, ZodiacName> dictionary: DictionaryToDataTable(dictionary, table); break;
                case Dictionary<HouseName, Constellation> dictionary: DictionaryToDataTable(dictionary, table); break;
                case Dictionary<HouseName, IList> dictionary: DictionaryToDataTable(dictionary, table); break;
                //case Dictionary<HouseName, IList> dictionary:
                //    {

                //        var array = new JArray();
                //        foreach (var item in dictionary)
                //        {
                //            var obj = new JObject
                //        {
                //            { "House", item.Key.ToString() },
                //            { dataName,  Tools.ListToString((List<IList>)item.Value) }
                //        };
                //            array.Add(obj);
                //        }

                //        rootPayloadJson = new JProperty(dataName, array);
                //        break;

                //    }

                //case Dictionary<ZodiacName, int> dictionary:
                //    {
                //        //convert list to comma separated string
                //        var parsedList = dictionary.Cast<object>().ToList();
                //        var stringComma = Tools.ListToString(parsedList);

                //        rootPayloadJson = new JProperty(dataName, stringComma);
                //        break;
                //    }
                //custom JSON converter available
                case IToJson iToJson:

                    //rootPayloadJson = new JProperty(dataName, iToJson.ToJson());
                    break;
                //normal conversion via "ToString"
                default:
                    //throw new NotImplementedException();
                    //rootPayloadJson = new JProperty(dataName, anyTypeData?.ToString());
                    break;
            }



            return table;

        }


        /// <summary>
        /// Given a dictionary of example Dictionary<PlanetName, ZodiacSign>
        /// will convert to equal DataTable representation
        /// </summary>
        public static void DictionaryToDataTable<T, Y>(Dictionary<T, Y> dictionary, DataTable table)
        {
            //add columns names
            var colName1 = dictionary.First().Key.GetType().Name;
            table.Columns.Add(colName1);

            var value = dictionary.First().Value;
            if (value is List<APIFunctionResult> ccc)
            {
                foreach (var apiFunctionResult in ccc)
                {
                    //add in column name
                    table.Columns.Add(apiFunctionResult.Name);
                }
            }
            else
            {
                var colName2 = value.GetType().Name;
                table.Columns.Add(colName2);

            }

            //add in rows
            foreach (var item in dictionary)
            {
                //data to put in row
                var column1Data = item.Key;
                var column2Data = item.Value;

                if (column2Data is List<APIFunctionResult> allCallResults)
                {
                    //extra logic for rows
                    var arrayObjects = new List<object>();
                    foreach (var apiFunctionResult in allCallResults)
                    {
                        //add in data
                        var result = apiFunctionResult.Result;
                        arrayObjects.Add(Tools.AnyToString(result));
                    }
                    table.Rows.Add(arrayObjects.ToArray());
                }
                //if list need to break down, with comma
                else if (column2Data is IList iList)
                {
                    var listData = string.Join(",", iList.Cast<object>());
                    table.Rows.Add(column1Data, listData);
                }
                //handle ALL API calls
                else
                {
                    table.Rows.Add(column1Data.ToString(), column2Data.ToString());
                }
            }
        }

        /// <summary>
        /// Place Dictionary data nicely into JSON form
        /// </summary>
        private static JProperty RootPayloadJson(string keyColumn, string dataName, dynamic dictionary)
        {
            var array = new JArray();
            foreach (var item in dictionary)
            {
                var obj = new JObject
                {
                    { keyColumn, item.Key.ToString() },
                    { dataName, item.Value.ToString() }
                };
                array.Add(obj);
            }

            var rootPayloadJson = new JProperty(dataName, array);

            return rootPayloadJson;
        }

        /// <summary>
        /// Given any type tries best to convert to string
        /// note: used in ML Table Generator
        /// </summary>
        public static string AnyToString(object result)
        {
            // Use StringBuilder for efficient string concatenation
            var sb = new StringBuilder();
            switch (result)
            {
                // Use 'is' for pattern matching
                case IList iList:
                    sb.Append(string.Join(", ", iList.Cast<object>()));
                    break;
                case Enum enumResult:
                    sb.Append(enumResult.ToString());
                    break;
                case Dictionary<PlanetName, ZodiacName> dictPZodiacName:
                    AppendDictionary(sb, dictPZodiacName);
                    break;
                case Dictionary<PlanetName, ZodiacSign> dictPZodiacSign:
                    AppendDictionary(sb, dictPZodiacSign);
                    break;
                case Dictionary<PlanetName, Constellation> dictPConstellation:
                    AppendDictionary(sb, dictPConstellation);
                    break;
                case Dictionary<PlanetName, Dictionary<ZodiacName, int>> dictPDZ:
                    {
                        foreach (var kv in dictPDZ)
                        {
                            sb.Append($"{kv.Key}: {AnyToString(kv.Value)}, ");
                        }

                        break;
                    }
                case Dictionary<ZodiacName, int> dictZI:
                    AppendDictionary(sb, dictZI);
                    break;
                case Dictionary<PlanetName, double> dictPD:
                    AppendDictionary(sb, dictPD);
                    break;
                default:
                    sb.Append(result.ToString());
                    break;
            }
            return sb.ToString();
        }
        private static void AppendDictionary<TKey, TValue>(StringBuilder sb, Dictionary<TKey, TValue> dict)
        {
            foreach (var kv in dict)
            {
                sb.Append($"{kv.Key}: {kv.Value}, ");
            }
        }


        /// <summary>
        /// QuoteValue method takes care of escaping double quotes and enclosing values in double quotes.
        /// This should handle cases where property values contain commas, newlines, or double quotes.
        /// If a value contains a double quote, you can escape it by doubling it. 
        /// </summary>
        public static string QuoteValue(object value)
        {
            var stringValue = value?.ToString() ?? string.Empty;
            stringValue = stringValue.Replace("\"", "\"\""); // escape double quotes
            return $"\"{stringValue}\""; // enclose in double quotes
        }

        public static string StringToMimeType(string fileFormat)
        {
            switch (fileFormat.ToLower())
            {
                case "pdf": return MediaTypeNames.Application.Pdf;
                case "xml": return MediaTypeNames.Application.Xml;
                case "gif": return MediaTypeNames.Image.Gif;
                case "jpeg": return MediaTypeNames.Image.Jpeg;
                case "jpg": return MediaTypeNames.Image.Jpeg;
                case "tiff": return MediaTypeNames.Image.Tiff;
            }

            throw new Exception("END OF LINE");

        }

        /// <summary>
        /// Given a list of object will make into JSON
        /// </summary>
        public static JArray ListToJson<T>(List<T> itemList)
        {
            //get all as converted to basic string

            JArray arrayJson = new JArray();
            foreach (var item in itemList)
            {
                if (item is XElement personXml)
                {
                    var personJson = Tools.XmlToJson(personXml);
                    arrayJson.Add(personJson);
                }
                else if (item is IToJson toJson)
                {
                    arrayJson.Add(toJson.ToJson());
                }
                else if (item is JObject toJobject)
                {
                    arrayJson.Add(toJobject);
                }
                //do it normal string way
                else
                {
                    arrayJson.Add(item.ToString());
                }

            }

            return arrayJson;
        }

        /// <summary>
        /// Used for Person, Match Report, Chart and all things made by end user
        /// </summary>
        public static string[] GetUserIdFromData(XElement inputXml)
        {
            var userIdRaw = inputXml.Element("UserId")?.Value ?? "";
            //clean, remove white space & new line if any
            userIdRaw = userIdRaw.Replace("\n", "");
            userIdRaw = userIdRaw.Replace(" ", "");

            var userId = userIdRaw.Split(',');//split by comma

            return userId;
        }

        public static string[] GetUserIdFromData(JToken input)
        {
            var userIdRaw = input["UserId"].Value<string>();
            //clean, remove white space & new line if any
            userIdRaw = userIdRaw.Replace("\n", "");
            userIdRaw = userIdRaw.Replace(" ", "");

            var userId = userIdRaw.Split(',');//split by comma

            return userId;
        }

        /// <summary>
        /// Given a doc os records will find by user ID , owners
        /// used by to get stuff created by end user
        /// </summary>
        public static List<XElement> FindXmlByUserId(XDocument allListXmlDoc, string inputUserId)
        {
            var returnList = new List<XElement>();

            //add all  profiles that have the given user ID
            var allItems = allListXmlDoc.Root?.Elements();
            foreach (var itemXml in allItems)
            {
                var allOwnerId = itemXml.Element("UserId")?.Value ?? "";

                //check if inputed ID is found in list, add to return list
                var match = IsUserIdMatch(allOwnerId, inputUserId);
                if (match) { returnList.Add(itemXml); }
            }

            return returnList;
        }

        /// <summary>
        /// check if 2 user id strings match, can't just use contains since 101 can be anywhere
        /// split by comma, and check by direct equality lower case
        /// </summary>
        public static bool IsUserIdMatch(string userIdStringA, string userIdStringB)
        {

            //must be split before can be used
            var userListA = userIdStringA.Split(',');
            var userListB = userIdStringB.Split(',');

            foreach (var userIdA in userListA)
            {
                foreach (var userIdB in userListB)
                {
                    //check direct match with lower case todo maybe lower case not needed since user ID, not person ID
                    var match = userIdA.ToLower() == userIdB.ToLower();

                    //if found even 1 match then return
                    if (match) { return true; }
                }
            }

            //if control reaches here than confirm no match
            return false;
        }

        /// <summary>
        /// Check if inputed time was within last hour
        /// </summary>
        public static bool IsWithinLastHour(Time logItemTime, double hours)
        {
            //get time 1 hour ago
            var time1HourAgo = DateTimeOffset.Now.AddHours(hours);

            //check if inputed time is after this 1 ago mark
            var isAfter = logItemTime.GetStdDateTimeOffset() >= time1HourAgo;

            return isAfter;
        }


        /// <summary>
        /// sends a simple head request to check if file exists (low cost)
        /// </summary>
        public static async Task<bool> DoesFileExist(string url)
        {
            var client = new HttpClient();
            using var request = new HttpRequestMessage(HttpMethod.Head, url);

            using var response = await client.SendAsync(request);
            return response.StatusCode == System.Net.HttpStatusCode.OK;
        }

        /// <summary>
        /// given a time zone will return famous cities using said timezone
        /// </summary>
        public static string TimeZoneToLocation(string timeZone)
        {
            return "Earth";
            //switch (timeZone)
            //{

            //}
        }


        /// <summary>
        /// No parsing direct from horses mouth
        /// GET Request
        /// </summary>
        public static async Task<T> ReadServerRaw<T>(string receiverAddress)
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, receiverAddress);

            var waitForContent = HttpCompletionOption.ResponseContentRead;
            using var client = new HttpClient();
            client.Timeout = Timeout.InfiniteTimeSpan;
            var response = await client.SendAsync(httpRequestMessage, waitForContent);
            if (typeof(T) == typeof(byte[]))
            {
                var dataReturned = await response.Content.ReadAsByteArrayAsync();
                return (T)(object)dataReturned;
            }
            else
            {
                var dataReturned = await response.Content.ReadAsStringAsync();
                if (typeof(T) == typeof(string))
                {
                    return (T)(object)dataReturned;
                }
                else if (typeof(T) == typeof(JObject))
                {
                    return (T)(object)JObject.Parse(dataReturned);
                }
                else
                {
                    throw new InvalidOperationException("Unsupported type parameter");
                }
            }
        }


        /// <summary>
        /// makes HTTP call to address using .NET
        /// </summary>
        public static async Task<T> WriteServer<T, Z>(HttpMethod method, string receiverAddress, Z payload = default)
        {
            //prepare the data to be sent
            var httpRequestMessage = new HttpRequestMessage(method, receiverAddress);

            //tell sender to wait for complete reply before exiting
            var waitForContent = HttpCompletionOption.ResponseContentRead;

            //add in payload if specified
            if (payload != null)
            {
                if (payload is JToken payloadJson)
                {
                    httpRequestMessage.Content = JsontoHttpContent(payloadJson);
                }
                else if (payload is byte[] payloadBinary)
                {
                    httpRequestMessage.Content = new ByteArrayContent(payloadBinary);
                }
                else
                {
                    throw new ArgumentException("Payload must be either a JToken or a byte array.", nameof(payload));
                }
            }

            //send the data on its way (wait forever no timeout)
            using var client = new HttpClient();
            client.Timeout = Timeout.InfiniteTimeSpan;

            //send the data on its way
            var response = await client.SendAsync(httpRequestMessage, waitForContent);
            if (typeof(T) == typeof(byte[]))
            {
                var dataReturned = await response.Content.ReadAsByteArrayAsync();
                return (T)(object)dataReturned;
            }
            else
            {
                var dataReturned = await response.Content.ReadAsStringAsync();
                if (typeof(T) == typeof(string))
                {
                    return (T)(object)dataReturned;
                }
                else if (typeof(T) == typeof(JObject))
                {
                    //return data as JSON as expected from API 
                    return (T)(object)JObject.Parse(dataReturned);
                }
                else
                {
                    throw new InvalidOperationException("Unsupported type parameter");
                }
            }

        }

        /// <summary>
        /// Given a method name in string form, will get it's reference to code
        /// gets from Calculate.cs class
        /// </summary>
        public static MethodInfo MethodNameToMethodInfo(string methodName, Type[] calculatorClass)
        {
            foreach (var classType in calculatorClass)
            {
                var foundList = classType.GetMethods().Where(x => x.Name == methodName);
                var foundMethod = foundList?.FirstOrDefault();

                //if method found, stop looking end here
                if (foundMethod != null)
                {
                    //if more than 1 method found major internal error, crash it!
                    if (foundList.Count() > 1) { Console.WriteLine($"POTENTIAL ERROR: Duplicate API Names : {methodName}"); }

                    return foundMethod;
                }
            }

            //end of line
            throw new Exception("Calculator method not found!");
        }


        /// <summary>
        /// Given a id will return parsed person from main list
        /// Returns empty person if, no person found
        /// NOTE: use owner id if provided else skip it
        /// </summary>
        public static Person GetPersonById(string personId, string ownerId = "")
        {
            // Initialize foundCalls to null
            Pageable<PersonListEntity> foundCalls = null;

            // Query the database based on ownerId
            if (string.IsNullOrEmpty(ownerId))
            {
                // Query without person Id, possible to return multiple values
                foundCalls = AzureTable.PersonList.Query<PersonListEntity>(row => row.RowKey == personId);
            }
            else
            {
                // Query with both ownerId and personId for accurate hit
                foundCalls = AzureTable.PersonList.Query<PersonListEntity>(row => row.PartitionKey == ownerId && row.RowKey == personId);
            }

            // If person not found, check shared list
            if (!foundCalls.Any())
            {
                var rawSharedList = AzureTable.PersonShareList.Query<PersonListEntity>(row => row.PartitionKey == ownerId && row.RowKey == personId);
                // If share found, get person directly without original ownerId
                if (rawSharedList.Any())
                {
                    foundCalls = AzureTable.PersonList.Query<PersonListEntity>(row => row.RowKey == personId);
                }
            }
            // Log error if more than 1 person found
            if (foundCalls.Count() > 1)
            {
                LibLogger.Debug($"More than 1 Person found : PersonId -> {personId} OwnerId -> {ownerId}");
            }

            // Log error and return empty if person not found
            if (!foundCalls.Any())
            {
                LibLogger.Debug($"Person NOT FOUND : {personId} OwnerID :{ownerId}, EMPTY GIVEN");
                return Person.Empty;
            }

            // Convert to readable format and return
            var personToReturn = Person.FromAzureRow(foundCalls.FirstOrDefault());
            return personToReturn;
        }

        public static async Task<Person> GetPersonByIdViaAPI(string personId, string ownerId)
        {

            var url = $"{URL.ApiStableDirect}/GetPerson/OwnerId/{ownerId}/PersonId/{personId}";
            var result = await Tools.ReadServerRaw<JObject>(url);

            //get parsed payload from raw result
            var person = Tools.GetPayload(result, Person.FromJson);

            return person;

        }


        /// <summary>
        /// Gets XML file from Azure blob storage
        /// </summary>
        public static async Task<XDocument> GetXmlFileFromAzureStorage(string fileName, string blobContainerName)
        {
            var fileClient = await GetBlobClientAzure(fileName, blobContainerName);
            var xmlFile = await DownloadToXDoc(fileClient);

            return xmlFile;
        }

        /// <summary>
        /// Converts a blob client of a file to an XML document
        /// </summary>
        public static async Task<XDocument> DownloadToXDoc(BlobClient blobClient)
        {
            var isFileExist = (await blobClient.ExistsAsync()).Value;

            if (isFileExist)
            {
                XDocument xDoc;
                await using (var stream = (await blobClient.DownloadStreamingAsync()).Value.Content)
                {
                    xDoc = await XDocument.LoadAsync(stream, LoadOptions.None, CancellationToken.None);
                }

#if DEBUG
                Console.WriteLine($"Downloaded: {blobClient.Name}");
#endif

                return xDoc;
            }
            else
            {
                //will be logged by caller
                throw new Exception($"No File in Cloud! : {blobClient.Name}");
            }

        }


        /// <summary>
        /// Gets file blob client from azure storage by name
        /// </summary>
        public static async Task<BlobClient> GetBlobClientAzure(string fileName, string blobContainerName)
        {
            //get the connection string stored separately (for security reasons)
            //note: dark art secrets are in local.settings.json
            var storageConnectionString = Secrets.Get("API_STORAGE");

            //get image from storage
            var blobContainerClient = new BlobContainerClient(storageConnectionString, blobContainerName);
            var fileBlobClient = blobContainerClient.GetBlobClient(fileName);

            return fileBlobClient;

            //var returnStream = new MemoryStream();
            //await fileBlobClient.DownloadToAsync(returnStream);

            //return returnStream;
        }

        /// <summary>
        /// INPUT:
        /// /Singapore/Time/23:59/31/12/2000/+08:00/Planet/Sun/Sign/
        /// OUTPUT:
        /// "/Singapore/Time/23:59/"
        /// NOTE:
        /// In this example, if cutCount is 3, the CutString method will return
        /// the first 3 substrings ("Singapore", "Time", "23:59")
        /// from the input string. The result will be "/Singapore/Time/23:59/".
        /// </summary>
        public static string CutOutString(string input, int cutCount)
        {
            var parts = input.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            var firstParts = parts.Take(cutCount);
            return "/" + string.Join("/", firstParts) + "/";
        }

        /// <summary>
        /// removes the what is within count, returns rest
        /// </summary>
        public static string CutRemoveString(string input, int cutCount)
        {
            var parts = input.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            var remainingParts = parts.Skip(cutCount);
            return "/" + string.Join("/", remainingParts) + "/";
        }


        /// <summary>
        /// This algorithm calculates the minimum number of single-character edits (insertions, deletions, or substitutions) required to change one word into the other. It's a good measure of how dissimilar two strings are.
        /// </summary>
        public static int LevenshteinDistance(string a, string b)
        {
            var matrix = new int[a.Length + 1, b.Length + 1];
            for (int i = 0; i <= a.Length; i++)
                matrix[i, 0] = i;
            for (int j = 0; j <= b.Length; j++)
                matrix[0, j] = j;
            for (int i = 1; i <= a.Length; i++)
            {
                for (int j = 1; j <= b.Length; j++)
                {
                    int cost = (a[i - 1] == b[j - 1]) ? 0 : 1;
                    matrix[i, j] = Math.Min(Math.Min(matrix[i - 1, j] + 1, matrix[i, j - 1] + 1), matrix[i - 1, j - 1] + cost);
                }
            }
            return matrix[a.Length, b.Length];
        }


        /// <summary>
        /// Given a MethodInfo will generate Python method stub declaration code (Made by AI in 30s)
        /// EXP: def HousePlanetIsIn(time: Time, planet_name: PlanetName) -> HouseName: 
        /// </summary>
        public static string GeneratePythonDef(MethodInfo methodInfo)
        {
            var sb = new StringBuilder();
            // Get the method name
            string methodName = methodInfo.Name;
            sb.Append("def ");
            sb.Append(methodName);
            sb.Append("(");
            // Get the parameter types and names
            var parameters = methodInfo.GetParameters();
            if (parameters.Length == 0)
            {
                sb.Append("cls");
            }
            else
            {
                for (int i = 0; i < parameters.Length; i++)
                {
                    var param = parameters[i];
                    sb.Append(param.Name);
                    sb.Append(": ");
                    //meaning it is a Task, List or Dictionary
                    var cleanedTypeName = param.ParameterType.Name.Contains("`") ? "Any" : param.ParameterType.Name;
                    cleanedTypeName = cleanedTypeName.Replace("[", "").Replace("]", ""); // Remove square brackets
                    sb.Append(cleanedTypeName);
                    if (i < parameters.Length - 1)
                    {
                        sb.Append(", ");
                    }
                }
            }
            sb.Append(")");
            // Get the return type
            var returnType = methodInfo.ReturnType;
            if (returnType.Name.Contains("`")) //meaning it is a Task, List or Dictionary
            {
                sb.Append(" -> Any");
            }
            else
            {
                sb.Append(" -> ");
                var cleanedReturnTypeName = returnType.Name.Replace("[", "").Replace("]", ""); // Remove square brackets
                sb.Append(cleanedReturnTypeName);
            }
            sb.Append(":");
            return sb.ToString();
        }

        /// <summary>
        /// Given a metadata will give name params stacked
        /// exp : IsPlanetBenefic_Sun
        /// </summary>
        public static string GetSpecialMLTableName(dynamic openApiMetadata, object resultOverride = null)
        {
            if (openApiMetadata.SelectedParams == null)
            {
                //when using methods like All, need to dig out column name
                return $"{openApiMetadata.Name}_{resultOverride?.ToString() ?? "PLEASE INJECT VALUE"}";
            }

            //stack the param values next to each other exp: Sun_House1
            var paramCombined = "";
            foreach (var selectedParam in openApiMetadata.SelectedParams)
            {
                //if time no need to add into column name, since its in the row
                if (selectedParam is Time)
                {
                    continue;
                }
                else if (selectedParam is IList ccc)
                {
                    foreach (object xxx in ccc)
                    {
                        if (xxx is Time)
                        {
                            continue;
                        }
                        var strData = Tools.AnyToString(xxx);
                        paramCombined += "_" + strData;
                    }
                }
                else
                {
                    var strData = Tools.AnyToString(selectedParam);
                    paramCombined += "_" + strData;
                }
            }

            return $"{openApiMetadata.Name}{paramCombined}";
        }

        public static dynamic ephemeris_swe_calc(Time time, int swissPlanet)
        {
            //Converts LMT to UTC (GMT)
            int iflag = 2;//SwissEph.SEFLG_SWIEPH;  //+ SwissEph.SEFLG_SPEED;
            double[] results = new double[6];
            string err_msg = "";
            double jul_day_ET;
            SwissEph ephemeris = new SwissEph();

            // Convert DOB to ET
            jul_day_ET = Calculate.TimeToEphemerisTime(time);

            //Get planet long
            int ret_flag = ephemeris.swe_calc(jul_day_ET, swissPlanet, iflag, results, ref err_msg);

            //data in results at index 0 is longitude
            var sweCalcResults = new
            {
                Longitude = results[0],
                Latitude = results[1],
                DistanceAU = results[2],
                SpeedLongitude = results[3],
                SpeedLatitude = results[4],
                SpeedDistance = results[5]
            };

            return sweCalcResults;
        }

        /// <summary>
        /// Prints each item in list
        /// </summary>
        public static void PrintList<T>(List<T> list)
        {
            foreach (var item in list)
            {
                Console.WriteLine(item.ToString());
            }
        }

        public static bool IsMethodReturnAsync(MethodInfo method)
        {
            // Null-checking omitted for brevity
            if (method.ReturnType == typeof(Task) ||
                (method.ReturnType.IsGenericType && method.ReturnType.GetGenericTypeDefinition() == typeof(Task<>)))
            {
                return true;
            }
            return false;
        }


        /// <summary>
        /// "JupiterSunPD3" --> "Jupiter"
        /// 0 based word position, default is 0 for first word
        /// </summary>
        public static string GetCamelCaseWord(string input, int wordPosition = 0)
        {
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }
            var words = new List<string>();
            var word = new StringBuilder();
            foreach (var ch in input)
            {
                if (char.IsUpper(ch) && word.Length > 0)
                {
                    words.Add(word.ToString());
                    word.Clear();
                }
                word.Append(ch);
            }
            words.Add(word.ToString());
            return words[wordPosition];
        }


        /// <summary>
        /// takes a duration in hours and
        /// returns a string that represents the duration in
        /// a more human-readable format (hours, days, months, or years). 
        /// </summary>
        public static string TimeDurationToHumanText(double durationInHours)
        {
            const double hoursInDay = 24;
            const double daysInMonth = 30;
            const double monthsInYear = 12;
            if (durationInHours < hoursInDay)
            {
                return $"{Math.Round(durationInHours, 1)} hours";
            }
            double durationInDays = durationInHours / hoursInDay;
            if (durationInDays < daysInMonth)
            {
                return $"{Math.Round(durationInDays, 1)} days";
            }
            double durationInMonths = durationInDays / daysInMonth;
            if (durationInMonths < monthsInYear)
            {
                return $"{Math.Round(durationInMonths, 1)} months";
            }
            double durationInYears = durationInMonths / monthsInYear;
            return $"{Math.Round(durationInYears, 1)} years";
        }


        /// <summary>
        /// Given bytes in long will convert to human readable text, exp : 1.2 GB
        /// </summary>
        public static string BytesToHumanText(long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            int order = 0;
            while (bytes >= 1024 && order < sizes.Length - 1)
            {
                order++;
                bytes = bytes / 1024;
            }
            // Format the output to 2 decimal places
            return string.Format("{0:0.##} {1}", bytes, sizes[order]);
        }

        /// <summary>
        /// takes in raw response from API and
        /// gets payload after checking status and shows error if status "Fail"
        /// note :  no parser use direct, support for string, int and double
        /// </summary>
        public static T GetPayload<T>(JToken rawResult, Func<JToken, T>? parser)
        {
            //result must say Pass, else it has failed
            var isPass = rawResult["Status"]?.Value<string>() == "Pass";
            var payloadJson = rawResult["Payload"] ?? new JObject();

            if (isPass)
            {
#if DEBUG
                Console.WriteLine("API SAID: PASS"); //debug to know all went well
#endif

                //use parser if available, use that, end here
                if (parser != null)
                {
                    var personJson = parser(payloadJson);
                    return personJson;
                }

                //if no parser use direct, support for string, int and double
                return payloadJson.Value<T>();

            }
            else
            {
#if DEBUG
                Console.WriteLine($"API SAID : FAIL :\n{payloadJson}");
#endif
                //for now this should notify errors nicely, todo maybe exceptions is not best 
                throw new Exception($"Failed to get {typeof(T).AssemblyQualifiedName} from API payload");
            }

        }

        public static byte[] DataTableToJpeg(DataTable table)
        {
            // Create a new Bitmap object
            Bitmap bitmap = new Bitmap(1, 1);
            Graphics g = Graphics.FromImage(bitmap);

            // Calculate the maximum width for each column
            int[] columnWidths = new int[table.Columns.Count];
            for (int j = 0; j < table.Columns.Count; j++)
            {
                // Use the column name for the header row
                var headerFont = new Font("Arial", 10, FontStyle.Bold);
                var headerSize = g.MeasureString(table.Columns[j].ColumnName, headerFont);
                columnWidths[j] = (int)headerSize.Width;

                for (int i = 0; i < table.Rows.Count; i++)
                {
                    var font = new Font("Arial", 10);
                    var textSize = g.MeasureString(table.Rows[i][j].ToString(), font);
                    columnWidths[j] = Math.Max(columnWidths[j], (int)textSize.Width);
                }
            }

            // Dispose of the initial Graphics object and create a new one with the correct dimensions
            g.Dispose();
            int totalWidth = columnWidths.Sum() + table.Columns.Count * 2; // Add padding
            bitmap = new Bitmap(totalWidth, (table.Rows.Count + 1) * 20); // Add a row for the header
            g = Graphics.FromImage(bitmap);

            // Draw the table
            int currentX = 0;
            for (int j = 0; j < table.Columns.Count; j++)
            {
                // Draw the header row
                g.FillRectangle(Brushes.White, new Rectangle(currentX, 0, columnWidths[j] + 2, 20)); // Add padding
                g.DrawRectangle(Pens.Black, new Rectangle(currentX, 0, columnWidths[j] + 2, 20)); // Add padding
                var headerFont = new Font("Arial", 10, FontStyle.Bold);
                g.DrawString(table.Columns[j].ColumnName, headerFont, Brushes.Black, new PointF(currentX + 3, 1)); // Add padding

                for (int i = 0; i < table.Rows.Count; i++)
                {
                    g.FillRectangle(Brushes.White, new Rectangle(currentX, (i + 1) * 20, columnWidths[j] + 2, 20)); // Add padding
                    g.DrawRectangle(Pens.Black, new Rectangle(currentX, (i + 1) * 20, columnWidths[j] + 2, 20)); // Add padding

                    var font = new Font("Arial", 10);
                    g.DrawString(table.Rows[i][j].ToString(), font, Brushes.Black, new PointF(currentX + 3, (i + 1) * 20 + 1)); // Add padding
                }
                currentX += columnWidths[j] + 2; // Move to next column position
            }

            // Convert the image to a byte array
            using (MemoryStream ms = new MemoryStream())
            {
                bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                return ms.ToArray();
            }
        }

        public static DataTable ConvertJPropertyToDataTable(JProperty jProperty)
        {
            // Create a new DataTable.
            DataTable dataTable = new DataTable();

            // Define a flag for the first row.
            bool firstRow = true;

            // Parse the JProperty values.
            foreach (JObject jObject in jProperty.Value)
            {
                DataRow row = dataTable.NewRow();

                // Extract the property values.
                var properties = jObject.Properties();
                foreach (var property in properties)
                {
                    // If it's the first row, add the property names as column names.
                    if (firstRow)
                    {
                        dataTable.Columns.Add(property.Name, typeof(string));
                    }

                    // Add the value to the row.
                    row[property.Name] = (string)property.Value;
                }

                // Add the row to the DataTable.
                dataTable.Rows.Add(row);

                // After the first row, set the flag to false.
                firstRow = false;
            }

            return dataTable;
        }

        /// <summary>
        /// make a POST request and returns the result as a JObject
        /// </summary>
        public static async Task<JToken> MakePostRequest(string url, string jsonData)
        {

            using var client = new HttpClient();
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            HttpRequestMessage request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(url),
                Content = content
            };

            client.Timeout = TimeSpan.FromMinutes(10); //long timeout, LLM replies slow sometimes
            var response = await client.SendAsync(request);
            var result = await response.Content.ReadAsStringAsync();
            return JToken.Parse(result);
        }

        /// <summary>
        /// Direct GET request than JSON is parsed into LIST
        /// VedAstro JSON format supported
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inputUrl"></param>
        /// <param name="converter"></param>
        /// <returns></returns>
        public static async Task<List<T>> GetListNoPolling<T>(string inputUrl, Func<JToken, List<T>> converter)
        {
            JToken? xListJson = await Tools.ReadServerRaw<JObject>(inputUrl);

            //todo if fail...do something
            //await _jsRuntime.ShowAlert("error", AlertText.ObliviousErrors(), true);

            var timeListJson = xListJson["Payload"];
            var cachedPersonList = converter.Invoke(timeListJson);

            return cachedPersonList;
        }

        public static void PasswordProtect(string inputPassword)
        {
            if (inputPassword != Secrets.Get("Password"))
            {
                throw new ArgumentException("Invalid password");
            }
        }

        ///// <summary>
        ///// make lower case
        ///// </summary>
        //public static string CleanLocationName(string inputLocationName)
        //{
        //    //lower case it
        //    var lower = inputLocationName.ToLower();

        //    //removes any character that is not a letter or a number
        //    var cleanInputAddress = Regex.Replace(lower, @"[^a-zA-Z0-9]", string.Empty);

        //    return cleanInputAddress;
        //}


        /// <summary>
        /// Get's ip address from api.ipify.org
        /// </summary>
        public static async Task<string> GetIPAddress()
        {
            // You may also want to handle exceptions here depending upon the needs
            try
            {
                //NOTE: simple API get request call to get ip as raw text
                var requestUri = "https://api.ipify.org";
                var ipAddressStr = await Tools.ReadServerRaw<string>(requestUri);
                return ipAddressStr;
            }
            catch (System.Net.WebException wex)
            {
                throw new Exception("Failed to connect to external endpoint.", wex);
            }
        }

        public static string CleanAzureTableKey(string input, string newValue = "")
        {
            var invalidChars = new[] { '/', '\\', '#', '?' };
            foreach (var c in invalidChars)
            {
                input = input.Replace(c.ToString(), newValue);
            }

            // Remove control characters
            input = new string(input.Where(ch => !char.IsControl(ch)).ToArray());

            return input;
        }


        public static double[] ConvertStringToDoubleArray(string input)
        {
            // Remove square brackets and split the string by commas
            string[] stringValues = input.Trim('[', ']').Split(',');

            // Initialize an array to store the converted double values
            double[] result = new double[stringValues.Length];

            // Convert each string value to a double
            for (int i = 0; i < stringValues.Length; i++)
            {
                if (double.TryParse(stringValues[i], NumberStyles.Float, CultureInfo.InvariantCulture, out double value))
                {
                    result[i] = value;
                }
                else
                {
                    // Handle invalid input (e.g., non-numeric values)
                    throw new ArgumentException($"Invalid value at index {i}: {stringValues[i]}");
                }
            }

            return result;
        }
    }


}
