using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using OfficeOpenXml;

namespace VedAstro.Library
{

    /// <summary>
    /// Represents 1 row in table used to make ML Data Generator 
    /// </summary>
    public class MLTableRow
    {
        /// <summary>
        /// Represents 1 row in table used to make ML Data Generator 
        /// </summary>
        public MLTableRow(Time time, List<APIFunctionResult> dataRowList)
        {
            Time = time;
            DataColumns = dataRowList;
        }

        public Time Time { get; set; }

        public List<APIFunctionResult> DataColumns { get; set; }

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


    }

}
