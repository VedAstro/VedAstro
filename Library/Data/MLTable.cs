using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using OfficeOpenXml;

namespace VedAstro.Library
{

    /// <summary>
    /// Represents the data of table showed in ML Table Generator page
    /// </summary>
    public class MLTable
    {
        public static readonly MLTable Empty = new MLTable(new List<MLTableRow>(), new List<OpenAPIMetadata>());

        private MLTable(List<MLTableRow> rowData, List<OpenAPIMetadata> columnData)
        {
            RowData = rowData;
            ColumnData = columnData;
        }

        private List<MLTableRow> RowData { get; set; } = new List<MLTableRow>();

        private List<Time> TimeList { get; set; } = new List<Time>();

        private List<OpenAPIMetadata?> ColumnData { get; set; } = new List<OpenAPIMetadata>();

        public int RowsCount => RowData.Count;

        public int ColumnsCount { get; set; }


        private static int _progressCounter;

        /// <summary>
        /// when incremented will raise trigger event
        /// which in turn will update data for loading box
        /// </summary>
        public static int ProgressCounter
        {
            get => _progressCounter;
            set
            {
                //set value
                _progressCounter = value;

                //trigger event
                OnProgressCounterChanged?.Invoke(null, value.ToString());
            }
        }

        // Declare the event using EventHandler<T>
        public static event EventHandler<string> OnProgressCounterChanged;

        /// <summary>
        /// Given a raw data from User selection in GUI, generate ML Table
        /// </summary>
        public static MLTable FromData(List<Time> timeSlices, List<OpenAPIMetadata> columnData)
        {
            //# PREPARE DATA
            //need to reset list, else won't update properly on 2nd generate
            var rowData = new List<MLTableRow>();

            //make copy column for adding & removing dynamic columns at final output
            var expandedColumnData = columnData.ToList();

            //using time as 1 column generate the other data columns
            for (var rowNumber = 0; rowNumber < timeSlices.Count; rowNumber++)
            {
                var time = timeSlices[rowNumber];
                var finalResultList = new List<APIFunctionResult>();
                foreach (var metaInfo in columnData)
                {
                    //get the planet or house selected by user in each individual data point packet
                    var param = metaInfo.SelectedParams.ToList(); //clone else will affect underlying list
                    param.Add((object)time); //time injected from different component

                    //calculate together all the parameters given by user (heavy computation)
                    var wrapped = new List<MethodInfo>() { metaInfo.MethodInfo };
                    var calcDataList = AutoCalculator.ExecuteCals(wrapped, param.ToArray());

                    //inject selected params for use when converting into CSV & EXCEL (HACK)
                    calcDataList.ForEach(aR => aR.AddSelectedParams(param));

                    //combine data columns for this row
                    //check if many or just 1 result (is many results inside)
                    foreach (var calcData in calcDataList)
                    {
                        //if list hidden inside, exp when AllPlanetData is used
                        if (calcData?.Result is List<APIFunctionResult> subResults)
                        {
                            //add all the columns data
                            finalResultList.AddRange(subResults);

                            //add the column header names, BUT only do this once for first row
                            if (rowNumber == 0)
                            {
                                //delete the parent column name, since now child columns are added in place
                                expandedColumnData.Remove(metaInfo);

                                //add each column into main column list
                                foreach (var singleResult in subResults)
                                {
                                    //hack in new column (sub data into main columns)
                                    var tempColumn = new OpenAPIMetadata
                                    {
                                        Name = singleResult.Name,
                                        SelectedParams = calcData.SelectedParams
                                    };
                                    expandedColumnData.Add(tempColumn);
                                }
                            }
                        }

                        //else add like normal
                        else { finalResultList.AddRange(calcDataList); }

                        //NOTE: progress counter not working here because JS single thread
                    }
                }

                //add row to table
                rowData.Add(new MLTableRow(time, finalResultList));
            }

            //# BRING DATA TO LIVE AS TABLE
            var newTable = new MLTable(rowData, expandedColumnData);

            return newTable;
        }

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

            /// <summary>
            /// Given an excel file will extract out 1 column that contains parseable time
            /// </summary>
            static async Task<List<DateTimeOffset>> ExtractTimeColumnFromExcel(Stream excelBinary)
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
            /// Given an excel file will extract out 1 column that contains parseable time
            /// </summary>
            static async Task<List<GeoLocation>> ExtractLocationColumnFromExcel(Stream excelBinary)
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



                static async Task<int> FindGeoLocationColumnIndex(ExcelWorksheet worksheet, int rowIndex)
                {
                    for (int colIndex = 1; colIndex <= worksheet.Dimension.Columns; colIndex++)
                    {
                        // Check if column name has the word "location" in it
                        var columnName = worksheet.Cells[1, colIndex].Value?.ToString();
                        if (columnName != null && columnName.ToLower().Contains("location"))
                        {
                            return colIndex;
                        }

                        //NOTE: sometimes names can be wrongly detected as location
                        //else check the value
                        //var cellValue = worksheet.Cells[rowIndex, colIndex].Value?.ToString();
                        //// If the cell value can be parsed as a GeoLocation, this is the geoLocation column
                        //var tryParse = await GeoLocation.TryParse(cellValue);
                        //if (tryParse.Item1) //is parsed, we no need the parsed val
                        //{
                        //    return colIndex;
                        //}
                    }
                    return -1;
                }

            }

        }

        public static async Task<List<Time>> GetTimeListFromCsv(Stream inputedFile)
        {

            using (var copiedInputedFile = new MemoryStream())
            {
                inputedFile.CopyTo(copiedInputedFile);
                copiedInputedFile.Position = 0; // Reset the position of the MemoryStream to the beginning

                var foundRawTimeList = await ExtractTimeColumnFromCsv(copiedInputedFile.Clone());
                copiedInputedFile.Position = 0; // Reset the position of the MemoryStream to the beginning
                var foundGeoLocationList = await ExtractLocationColumnFromCsv(copiedInputedFile.Clone());

                //3 : COMBINE DATA
                var returnList = foundRawTimeList.Select((dateTimeOffset, index) => new Time(dateTimeOffset, foundGeoLocationList[index])).ToList();

                return returnList;

                /// <summary>
                /// Given a CSV file will extract out 1 column that contains parseable time
                /// </summary>
                static async Task<List<DateTimeOffset>> ExtractTimeColumnFromCsv(Stream csvStream)
                {
                    using (var reader = new StreamReader(csvStream))
                    {
                        string line;
                        bool headersFound = false;
                        int timeColumnIndex = -1;

                        ConcurrentBag<DateTimeOffset> timeList = new();

                        ParallelOptions options = new()
                        {
                            MaxDegreeOfParallelism = Environment.ProcessorCount * 2
                        };

                        while ((line = await reader.ReadLineAsync()) != null)
                        {
                            var values = line.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(v => v.Trim()).ToList();

                            if (!headersFound)
                            {
                                timeColumnIndex = values.FindLastIndex(x => x.Equals("Date", StringComparison.OrdinalIgnoreCase));

                                headersFound = true;
                            }
                            else
                            {
                                if (timeColumnIndex >= 0 && timeColumnIndex < values.Count)
                                {

                                    Parallel.ForEach(values, options, (value) =>
                                    {
                                        if (DateTimeOffset.TryParseExact(value, "yyyy-MM-dd HH:mm:ss zzzz", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTimeOffset dto))
                                        {

                                            timeList.Add(dto);

                                        }
                                    });
                                }
                            }
                        }

                        return timeList.OrderByDescending(t => t).TakeWhile((_, i) => i <= 5).Reverse().ToList();
                    }
                }
                /// <summary>
                /// Given a CSV file will extract out 1 column that contains parseable time
                /// </summary>
                static async Task<List<GeoLocation>> ExtractLocationColumnFromCsv(Stream csvStream)
                {
                    csvStream.Position = 0;
                    using (var reader = new StreamReader(csvStream))
                    {
                        string line;
                        bool headersFound = false;
                        int geoLocationColumnIndex = -1;
                        var geoLocations = new List<GeoLocation>();

                        while ((line = await reader.ReadLineAsync()) != null)
                        {
                            var values = line.Split(',');

                            if (!headersFound)
                            {
                                // Search for the geoLocation column by checking for header labels containing words like 'location', 'latitude', or 'longitude'
                                geoLocationColumnIndex = Array.FindIndex(values, v => v.ToLower().Contains("location") || v.ToLower().Contains("latitude") || v.ToLower().Contains("longitude"));
                                headersFound = true;
                            }
                            else
                            {
                                // Parse geoLocation when geoLocationColumnIndex is valid
                                if (geoLocationColumnIndex >= 0 && geoLocationColumnIndex < values.Length)
                                {
                                    var tryParse = await GeoLocation.TryParse(values[geoLocationColumnIndex]);
                                    if (tryParse.Item1)
                                    {
                                        geoLocations.Add(tryParse.Item2);
                                        ProgressCounter++;
                                    }
                                }
                            }
                        }

                        return geoLocations;
                    }
                }

            }

        }


        /// <summary>
        /// Send time list column as Json to API to make HTML table
        /// </summary>
        public static async Task<T> GenerateMLTable<T>(List<Time> timeList, List<OpenAPIMetadata> columnNameList, string selectedFormat)
        {
            var newMlTable = MLTable.FromData(timeList, columnNameList);

            switch (selectedFormat)
            {
                case "HTML": { return (T)(object)newMlTable.ToHtml(); }
                case "CSV": { return (T)(object)newMlTable.ToCSV(); }
                case "JSON": { return (T)(object)newMlTable.ToJson(); }
                case "EXCEL": { return (T)(object)newMlTable.ToExcel(); }
                default: throw new Exception("END OF LINE!");
            }
        }


        //----------------------------- CONVERTERS ------------------------------------------

        /// <summary>
        /// converts current instance to CSV string
        /// </summary>
        public string ToCSV()
        {
            StringBuilder sb = new StringBuilder();
            //make Columns
            sb.Append("Time,");
            foreach (var result in ColumnData)
            {
                sb.Append($"{result.MLTableName(result.SelectedPlanet)},");
            }
            sb.AppendLine();
            //make Rows
            foreach (var mlTableRow in RowData)
            {
                sb.Append($"{mlTableRow.Time},");
                foreach (var resultX in mlTableRow.DataColumns)
                {
                    sb.Append($"{resultX.ResultAsString()},");
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }

        public JObject ToJson()
        {
            JObject jObject = new JObject();
            JArray columns = new JArray();
            JArray rows = new JArray();
            //make Columns
            foreach (var result in ColumnData)
            {
                columns.Add(result.MLTableName(result.SelectedPlanet));
            }
            //make Rows
            foreach (var mlTableRow in RowData)
            {
                JObject row = new JObject();
                row.Add("Time", mlTableRow.Time.ToJson());
                foreach (var resultX in mlTableRow.DataColumns)
                {
                    row.Add(resultX.ResultAsString(), resultX.ResultAsString());
                }
                rows.Add(row);
            }
            jObject.Add("Columns", columns);
            jObject.Add("Rows", rows);
            return jObject;
        }

        public async Task<byte[]> ToParquet()
        {
            throw new NotImplementedException();
            //var htmlDocument = new HtmlDocument();
            //htmlDocument.LoadHtml(this.ToHtml());

            //// Create a new Parquet file
            //using var memoryStream = new MemoryStream();

            //// Write the header (columns)
            //var header = htmlDocument.DocumentNode.SelectSingleNode("//tr");
            //var columnNames = header.Elements("th").Select(th => th.InnerText).ToList();

            //// create file schema (columns)
            //var parsedColumnNames = columnNames.Select(columnName => new DataField<int>(columnName)).ToArray();
            //var schema = new ParquetSchema(parsedColumnNames);

            //using var parquetWriter = await ParquetWriter.CreateAsync(schema, memoryStream);


            //// Write the rows
            //var rows = htmlDocument.DocumentNode.SelectNodes("//tr[position()>1]");
            //foreach (var row in rows)
            //{
            //    using (ParquetRowGroupWriter groupWriter = parquetWriter.CreateRowGroup())
            //    {
            //        var htmlNodes = row.Elements("td").ToArray();
            //        for (int i = 0; i < htmlNodes.Count(); i++)
            //        {
            //            var td = htmlNodes[i];

            //            //parsedColumnNames;
            //            var idColumn = new DataColumn(
            //                schema.DataFields[i],
            //                new string[] { td.InnerText });
            //            await groupWriter.WriteColumnAsync(idColumn);

            //        }

            //    }

            //}
            //parquetWriter.Dispose();
            //var parquet = memoryStream.ToArray();

            //return parquet;
        }

        public byte[] ToExcel()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (ExcelPackage package = new ExcelPackage())
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Sheet1");
                // Add headers
                worksheet.Cells[1, 1].Value = "Time";
                int headerColumnIndex = 2;
                foreach (var result in ColumnData)
                {
                    worksheet.Cells[1, headerColumnIndex].Value = result.MLTableName(result.SelectedPlanet);
                    headerColumnIndex++;
                }
                // Add rows
                int rowIndex = 2;
                foreach (var mlTableRow in RowData)
                {
                    worksheet.Cells[rowIndex, 1].Value = mlTableRow.Time;
                    int columnIndex = 2;
                    foreach (var resultX in mlTableRow.DataColumns)
                    {
                        worksheet.Cells[rowIndex, columnIndex].Value = resultX.ResultAsString();
                        columnIndex++;
                    }
                    rowIndex++;
                }
                return package.GetAsByteArray();
            }
        }

        public byte[] ToHDF5()
        {
            throw new NotImplementedException();
        }

        public string ToHtml()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<table id=\"GeneratedTable\" class=\"table table-bordered table-hover\">");
            sb.AppendLine("<thead class=\"table-dark\">");
            sb.AppendLine("<tr>");
            sb.AppendLine("<th>Time</th>");

            //make Columns
            foreach (var result in ColumnData)
            {
                sb.AppendLine($"<th>{result.MLTableName(result.SelectedPlanet)}</th>");
            }
            sb.AppendLine("</tr>");
            sb.AppendLine("</thead>");
            sb.AppendLine("<tbody>");

            //make Rows
            foreach (var mlTableRow in RowData)
            {
                sb.AppendLine("<tr>");
                sb.AppendLine($"<td>{mlTableRow.Time}</td>");
                foreach (var resultX in mlTableRow.DataColumns)
                {
                    sb.AppendLine($"<td>{resultX.ResultAsString()}</td>");
                }
                sb.AppendLine("</tr>");

            }

            sb.AppendLine("</tbody>");
            sb.AppendLine("</table>");
            return sb.ToString();
        }



        //------------------------- METHODS TO SEND ACROSS THE VAST INTERNATIONAL NETWORKS ---------------------------------




    }

}
