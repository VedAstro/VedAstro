using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace VedAstro.Library
{

    /// <summary>
    /// Represents 1 row in table used to make ML Data Generator 
    /// </summary>
    public class MLTableRow(Time time, List<APIFunctionResult> dataRowList)
    {
        
        public Time Time { get; set; } = time;

        public List<APIFunctionResult> DataColumns { get; set; } = dataRowList;

        /// <summary>
        /// Converts a list of TableRow aka a full ML Data Table into CSV string 
        /// </summary>
        public static string ListToCSV(List<MLTableRow> tableRowList)
        {
            if (tableRowList == null || !tableRowList.Any())
            {
                return string.Empty;
            }
            StringBuilder csv = new StringBuilder();
            
            // Get property names (column names)
            PropertyInfo[] properties = typeof(MLTableRow).GetProperties().Where(p => p.Name != "DataColumns").ToArray();
            
            //names of the columns with custom name combined with param EXP: IsPlanetBenefic_Sun
            var columnNames = tableRowList[0].DataColumns.Select(result => result.MLTableName);

            // Assuming DataRowList is the first property in MLTableRow
            csv.AppendLine(string.Join(",", properties.Select(p => p.Name).Concat(columnNames)));

            // Get property values (rows)
            foreach (var row in tableRowList)
            {
                var rowValues = properties.Select(p => Tools.QuoteValue(p.GetValue(row, null))).ToList();
                rowValues.AddRange(row.DataColumns.Select(d => Tools.QuoteValue(d.ResultAsString())));
                csv.AppendLine(string.Join(",", rowValues));
            }

            return csv.ToString();
        }


    }

}
