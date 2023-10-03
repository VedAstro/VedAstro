using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Apache.Arrow;
using HtmlAgilityPack;
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
        private string HtmlTable => this.ToHtml();

        private List<MLTableRow> TableRowList { get; set; } = new List<MLTableRow>();
        private List<OpenAPIMetadata?> SelectedMethodMetaList { get; set; } = new List<OpenAPIMetadata>();
        public int RowsCount => TableRowList.Count;
        public int ColumnsCount => SelectedMethodMetaList.Count;

        /// <summary>
        /// converts current instance to CSV string
        /// </summary>
        public string ToCSV()
        {
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(HtmlTable);
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
            htmlDocument.LoadHtml(HtmlTable);
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
            htmlDocument.LoadHtml(HtmlTable);
            
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
            foreach (var result in SelectedMethodMetaList)
            {
                sb.AppendLine($"<th>{result.MLTableName(result.SelectedPlanet)}</th>");
            }
            sb.AppendLine("</tr>");
            sb.AppendLine("</thead>");
            sb.AppendLine("<tbody>");
            for (int index = 0; index < TableRowList.Count; index++)
            {
                sb.AppendLine("<tr>");
                sb.AppendLine($"<td>{TableRowList[index].Time}</td>");
                foreach (var resultX in TableRowList[index].DataColumns)
                {
                    sb.AppendLine($"<td>{resultX.ResultAsString()}</td>");
                }
                sb.AppendLine("</tr>");
            }
            sb.AppendLine("</tbody>");
            sb.AppendLine("</table>");
            return sb.ToString();
        }

        public void Update(List<MLTableRow> tableRowList, List<OpenAPIMetadata> selectedMetaList)
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


            TableRowList = tableRowList;
            SelectedMethodMetaList = correctedList;
        }
    }




}
