using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
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

        public int ColumnsCount => ColumnData.Count;

        /// <summary>
        /// Given a raw data from User selection in GUI, generate ML Table
        /// </summary>
        public static MLTable FromData(List<Time> timeSlices, List<OpenAPIMetadata> columnData)
        {
            //# PREPARE DATA

            //need to reset list, else won't update properly on 2nd generate
            var rowData = new List<MLTableRow>();

            //using time as 1 column generate the other data columns
            foreach (var time in timeSlices)
            {
                var finalResultList = new List<APIFunctionResult>();
                foreach (var metaInfo in columnData)
                {
                    //get the planet or house selected by user in each individual data point packet
                    var param = metaInfo.SelectedParams.ToList(); //clone else will effect underlying list
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
                        //if list hidden inside
                        if (calcData?.Result is List<APIFunctionResult> subResults) { finalResultList.AddRange(subResults); }

                        //else add like normal
                        else { finalResultList.AddRange(calcDataList); }

                    }

                }

                //add row to table
                rowData.Add(new MLTableRow(time, finalResultList));

            }

            //# BRING DATA TO LIVE AS TABLE
            var newTable = new MLTable(rowData, columnData);

            return newTable;
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
