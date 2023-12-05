using OfficeOpenXml;
using System.Text;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;
using VedAstro.Library;
using Microsoft.AspNetCore.Components.Forms;


namespace Desktop;


/// <summary>
/// Code to generate ML Table via Open API from a web/desktop client
/// </summary>
public class MLTableTools
{
    private static int progressCounter;

    /// <summary>
    /// when incremented will raise trigger event
    /// </summary>
    public static int ProgressCounter
    {
        get => progressCounter;
        set
        {
            //set value
            progressCounter = value;

            //trigger event
            OnProgressCounterChanged?.Invoke(null, value.ToString());
        }
    }

    // Declare the event using EventHandler<T>
    public static event EventHandler<string> OnProgressCounterChanged;


    //PUBLIC


    /// <summary>
    /// Given an excel file uploaded by user, send to server to process and extract out a Time list
    /// </summary>
    public static async Task<List<Time>> GetTimeListFromExcel(Stream inputedFile)
    {
        var foundRawTimeList = await ExtractTimeColumnFromExcel(inputedFile);
        var foundGeoLocationList = await ExtractLocationColumnFromExcel(inputedFile);

        //3 : COMBINE DATA
        var returnList = foundRawTimeList.Select(dateTimeOffset => new Time(dateTimeOffset, foundGeoLocationList[foundRawTimeList.IndexOf(dateTimeOffset)])).ToList();

        return returnList;
    }

    /// <summary>
    /// Given an excel file will extract out 1 column that contains parseable time
    /// </summary>
    public static async Task<List<GeoLocation>> ExtractLocationColumnFromExcel(Stream excelBinary)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // Set the license context for EPPlus
        var geoLocations = new List<GeoLocation>();
        int geoLocationColumnIndex = -1;
        // Reset the position of the Stream to the beginning
        excelBinary.Position = 0;
        // Use 'using' statement to ensure the MemoryStream is disposed off after use
        using (var excelFileStream = new MemoryStream())
        {
            await excelBinary.CopyToAsync(excelFileStream);
            excelFileStream.Position = 0; // Reset the position of the MemoryStream to the beginning
            using (var package = new ExcelPackage(excelFileStream))
            {
                var worksheet = package.Workbook.Worksheets[0]; // Get the first worksheet
                                                                // Start from the second row (index 2) to skip the header

                //NOTE: not all rows will have data, exp 100 rows but only 10 rows have data
                var rowsInSheet = worksheet.Dimension.Rows;
                for (int rowIndex = 2; rowIndex <= rowsInSheet; rowIndex++)
                {
                    // If we haven't found the geoLocation column yet, search for it
                    if (geoLocationColumnIndex == -1)
                    {
                        geoLocationColumnIndex = await FindGeoLocationColumnIndex(worksheet, rowIndex);
                    }

                    // If we've found the geoLocation column, add the cell value to the list
                    if (geoLocationColumnIndex != -1)
                    {
                        //get the raw location text to parse
                        var rawLocationText = worksheet.Cells[rowIndex, geoLocationColumnIndex].Value?.ToString();

                        //NOTE: this will cause even 1 row without data to be the "Stopper"
                        //if raw text is empty, then assume data rows has ended
                        if (string.IsNullOrWhiteSpace(rawLocationText))
                        {
                            break;
                        }

                        var tryParse = await GeoLocation.TryParse(rawLocationText);
                        if (tryParse.Item1)
                        {
                            //add in the final parsed location into return list
                            geoLocations.Add(tryParse.Item2);
                            ProgressCounter++;
                        }
                    }
                }
            }
        }
#if DEBUG
        Console.WriteLine($"File Size : {excelBinary.Length}");
        Console.WriteLine($"Rows Found : {geoLocations.Count}");
#endif
        return geoLocations;
    }

    private static async Task<int> FindGeoLocationColumnIndex(ExcelWorksheet worksheet, int rowIndex)
    {
        for (int colIndex = 1; colIndex <= worksheet.Dimension.Columns; colIndex++)
        {
            var cellValue = worksheet.Cells[rowIndex, colIndex].Value?.ToString();
            // If the cell value can be parsed as a GeoLocation, this is the geoLocation column
            var tryParse = await GeoLocation.TryParse(cellValue);
            if (tryParse.Item1) //is parsed, we no need the parsed val
            {
                return colIndex;
            }
        }
        return -1;
    }

    /// <summary>
    /// Given an excel file will extract out 1 column that contains parseable time
    /// </summary>
    public static async Task<List<DateTimeOffset>> ExtractTimeColumnFromExcel(Stream excelBinary)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // Set the license context for EPPlus
        var timeList = new List<DateTimeOffset>();
        int timeColumnIndex = -1;

        var excelFileStream = new MemoryStream();
        await excelBinary.CopyToAsync(excelFileStream);
        excelFileStream.Position = 0; // Reset the position of the MemoryStream to the beginning

        using (var package = new ExcelPackage(excelFileStream))
        {
            var worksheet = package.Workbook.Worksheets[0]; // Get the first worksheet
                                                            // Start from the second row (index 2) to skip the header

            //NOTE: not all rows will have data, exp 100 rows but only 10 rows have data
            var rowsInSheet = worksheet.Dimension.Rows;
            for (int rowIndex = 2; rowIndex <= rowsInSheet; rowIndex++)
            {
                // If we haven't found the time column yet, search for it
                if (timeColumnIndex == -1)
                {
                    for (int colIndex = 1; colIndex <= worksheet.Dimension.Columns; colIndex++)
                    {
                        var cellValue = worksheet.Cells[rowIndex, colIndex].Value?.ToString();
                        // If the cell value can be parsed as a Time, this is the time column
                        if (Time.TryParseStd(cellValue, out _))
                        {
                            timeColumnIndex = colIndex;
                            break;
                        }
                    }
                }
                // If we've found the time column, add the cell value to the list
                if (timeColumnIndex != -1)
                {
                    //get the raw time text to parse
                    var rawTimeText = worksheet.Cells[rowIndex, timeColumnIndex].Value?.ToString();

                    //NOTE: this will cause even 1 row without data to be the "Stopper"
                    //if raw text is empty, then assume data rows has ended
                    if (string.IsNullOrWhiteSpace(rawTimeText))
                    {
                        break;
                    }

                    //parse raw text to time
                    if (Time.TryParseStd(rawTimeText, out var time))
                    {
                        timeList.Add(time);
                        ProgressCounter++;
                    }
                }
            }

        }

#if DEBUG
        Console.WriteLine($"File Size : {excelBinary.Length}");
        Console.WriteLine($"Rows Found : {timeList.Count}");
#endif

        return timeList;
    }


    /// <summary>
    /// Send time list column as Json to API to make HTML table
    /// </summary>
    public static async Task<T> GenerateMLTable<T>(List<Time> timeList, List<OpenAPIMetadata> columnNameList, string selectedFormat)
    {

        var newMLTable = MLTable.FromData(timeList, columnNameList);


        switch (selectedFormat)
        {
            case "HTML":
                {
                    return (T)(object)newMLTable.ToHtml();
                }
            case "CSV":
                {
                    return (T)(object)newMLTable.ToCSV();
                }
            case "JSON":
                {
                    return (T)(object)newMLTable.ToJson();
                }
            case "EXCEL":
                {
                    return (T)(object)newMLTable.ToExcel();
                }

            default: throw new Exception("END OF LINE!");

        }



        ////prepare to send data to API
        //var payloadJson = new JObject();
        //payloadJson["TimeList"] = Tools.ListToJson(timeList);
        //payloadJson["ColumnData"] = Tools.ListToJson(columnNameList);


        ////send to api and get results 
        ////.../GenerateMLTable/HTML or CSV or EXCEL
        //var url = _api.URL.GenerateMLTable + $"/{selectedFormat}";

        //if (typeof(T) == typeof(string))
        //{
        //    var xListJson = await Tools.WriteServer<JObject, JObject>(HttpMethod.Post, url, payloadJson);

        //    var htmlTable = xListJson["Payload"]?[selectedFormat].Value<string>();
        //    return (T)(object)htmlTable;
        //}
        //else if (typeof(T) == typeof(byte[]))
        //{
        //    var xListJson = await Tools.WriteServer<byte[], JObject>(HttpMethod.Post, url, payloadJson);

        //    return (T)(object)xListJson;
        //}


        //throw new Exception("END OF LINE!");
    }


}