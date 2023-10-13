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
using Parquet;
using Parquet.Data;
using Parquet.Schema;

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
        /// This will update the underlying HTML
        /// </summary>
        public static MLTable FromData(List<MLTableRow> tableRowList, List<OpenAPIMetadata> selectedMetaList)
        {

            //#PROCESS DATA
            //need to bump up hidden many results calcs
            var correctedList = new List<OpenAPIMetadata>();
            foreach (var selectedMethod in selectedMetaList)
            {
                //detect if underlying list of results exist
                if (selectedMethod?.MethodInfo?.ReturnType == typeof(List<APIFunctionResult>))
                {
                    //take first row and get columns names out that way
                    var mlTableRow1st = tableRowList.FirstOrDefault();
                    foreach (var funcResult in mlTableRow1st.DataColumns)
                    {
                        var temp = OpenAPIMetadata.FromAPIFunctionResult(funcResult);
                        temp.SelectedPlanet = selectedMethod.SelectedPlanet; //inject planet from above main method
                        correctedList.Add(temp);
                    }
                }
                //else add like normal
                else
                {
                    correctedList.Add(selectedMethod);
                }
            }

            var tempNew = new MLTable(tableRowList, correctedList);

            return tempNew;
        }

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
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(this.ToHtml());
            var csvBuilder = new StringBuilder();
            var header = htmlDocument.DocumentNode.SelectSingleNode("//tr");
            foreach (var th in header.Elements("th"))
            {
                csvBuilder.Append(th.InnerText);
                csvBuilder.Append(",");
            }
            csvBuilder.AppendLine();
            var rows = htmlDocument.DocumentNode.SelectNodes("//tr[position()>1]");
            foreach (var row in rows)
            {
                foreach (var td in row.Elements("td"))
                {
                    //note careful of commas in data, since CSV uses it
                    var escapeComma = Tools.QuoteValue(td.InnerText);
                    csvBuilder.Append(escapeComma);
                    csvBuilder.Append(",");
                }
                csvBuilder.AppendLine();
            }
            return csvBuilder.ToString();
        }

        public async Task<byte[]> ToParquet()
        {
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(this.ToHtml());

            // Create a new Parquet file
            using var memoryStream = new MemoryStream();

            // Write the header (columns)
            var header = htmlDocument.DocumentNode.SelectSingleNode("//tr");
            var columnNames = header.Elements("th").Select(th => th.InnerText).ToList();

            // create file schema (columns)
            var parsedColumnNames = columnNames.Select(columnName => new DataField<int>(columnName)).ToArray();
            var schema = new ParquetSchema(parsedColumnNames);

            using var parquetWriter = await ParquetWriter.CreateAsync(schema, memoryStream);


            // Write the rows
            var rows = htmlDocument.DocumentNode.SelectNodes("//tr[position()>1]");
            foreach (var row in rows)
            {
                using (ParquetRowGroupWriter groupWriter = parquetWriter.CreateRowGroup())
                {
                    var htmlNodes = row.Elements("td").ToArray();
                    for (int i = 0; i < htmlNodes.Count(); i++)
                    {
                        var td = htmlNodes[i];

                        //parsedColumnNames;
                        var idColumn = new DataColumn(
                            schema.DataFields[i],
                            new string[] { td.InnerText });
                        await groupWriter.WriteColumnAsync(idColumn);

                    }

                }

            }
            parquetWriter.Dispose();
            var parquet = memoryStream.ToArray();

            return parquet;
        }

        public byte[] ToExcel()
        {
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(this.ToHtml());

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet1");
                var header = htmlDocument.DocumentNode.SelectSingleNode("//tr");
                int columnIndex = 1;
                foreach (var th in header.Elements("th"))
                {
                    worksheet.Cells[1, columnIndex].Value = th.InnerText;
                    columnIndex++;
                }
                var rows = htmlDocument.DocumentNode.SelectNodes("//tr[position()>1]");
                int rowIndex = 2;
                foreach (var row in rows)
                {
                    columnIndex = 1;
                    foreach (var td in row.Elements("td"))
                    {
                        worksheet.Cells[rowIndex, columnIndex].Value = td.InnerText;
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

        /// <summary>
        /// converts the table data into json version for transport
        /// </summary>
        //public JToken ToJson() => Tools.ConvertHtmlTableToJson(this.ToHtml());
        public JToken ToJson()
        {
            var temp = new JObject();
            temp[nameof(TimeList)] = Tools.ListToJson(TimeList);
            temp[nameof(ColumnData)] = Tools.ListToJson(ColumnData);

            return temp;
        }



    }

}
