

using Newtonsoft.Json.Linq;

namespace VedAstro.Library
{
    /// <summary>
    /// Simple data structure to hold summarized data for match compatibility report
    /// </summary>
    public class MatchSummaryData(string heartIcon, string scoreColor, string scoreSummary) : IToJson
    {
        public string HeartIcon { get; init; } = heartIcon;
        public string ScoreColor { get; init; } = scoreColor;
        public string ScoreSummary { get; init; } = scoreSummary;


        public JObject ToJson()
        {
            var temp = new JObject();
            temp["HeartIcon"] = this.HeartIcon;
            temp["ScoreColor"] = this.ScoreColor;  //not rounded
            temp["ScoreSummary"] = this.ScoreSummary;  //not rounded
            return temp;
        }


    }
}
