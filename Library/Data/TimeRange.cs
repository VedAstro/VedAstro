using Newtonsoft.Json.Linq;

namespace VedAstro.Library
{
    /// <summary>
    /// Simple wrapper for the start and end of a period of Time.
    /// </summary>
    public record TimeRange(Time start, Time end) : IToJson
    {
        public static TimeRange Empty = new TimeRange(Time.Empty, Time.Empty);

        /// <summary>
        /// Gets the number of days between start and end time
        /// </summary>
        public double DaysBetween => this.end.Subtract(this.start).TotalDays;


        /// <summary>
        /// Text of start and end time. Exp: 10/10/2020 - 11/10/2020
        /// </summary>
        public override string ToString()
        {
            var finalString = $"{start.StdDateMonthYearText} - {end.StdDateMonthYearText}";

            return finalString;
        }

        public JObject ToJson()
        {
            var temp = new JObject();
            temp["Start"] = this.start.ToJson();
            temp["End"] = this.end.ToJson();
            temp["DaysBetween"] = this.DaysBetween;

            return temp;

        }
    }
}
