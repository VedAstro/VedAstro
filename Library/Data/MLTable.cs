using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using HtmlAgilityPack;

namespace VedAstro.Library
{

    /// <summary>
    /// Represents the data of table showed in ML Table Generator page
    /// </summary>
    public class MLTable
    {
        private string _htmlTable;
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
            htmlDocument.LoadHtml(_htmlTable);
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
                    csvBuilder.Append(td.InnerText);
                    csvBuilder.Append(",");
                }
                csvBuilder.AppendLine();
            }
            return csvBuilder.ToString();
        }

        public byte[] ToExcel()
        {
            throw new NotImplementedException();
        }
        
        public byte[] ToHDF5()
        {
            throw new NotImplementedException();
        }
        
        public byte[] ToParquet()
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
                    //take first row and get data out that way
                    var mlTableRow = tableRowList.FirstOrDefault();
                    foreach (var funcResult in mlTableRow.DataColumns)
                    {
                        var temp = OpenAPIMetadata.FromAPIFunctionResult(funcResult);
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
