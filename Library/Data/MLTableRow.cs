using System.Collections.Generic;

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

        


    }

}
