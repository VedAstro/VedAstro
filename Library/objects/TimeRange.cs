using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VedAstro.Library
{
    /// <summary>
    /// Simple wrapper for the start and end of a period of Time.
    /// </summary>
    public record TimeRange(Time start, Time end)
    {
        public static TimeRange Empty = new TimeRange(Time.Empty, Time.Empty);

        /// <summary>
        /// Gets the number of days between start and end time
        /// </summary>
        public double daysBetween => this.end.Subtract(this.start).TotalDays;
    }
}
