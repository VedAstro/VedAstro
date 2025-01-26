using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using static VedAstro.Library.PlanetName;

namespace VedAstro.Library
{
    /// <summary>
    /// Represents the final data generated for compatibility
    /// </summary>
    public class MatchReport : IToJson
    {

        public static MatchReport Empty = new MatchReport("0", Person.Empty, Person.Empty, 0, "Empty Notes",
            new List<MatchPrediction>(), new[] { "101" }); //have to use direct variables

        private static readonly string[] DefaultUserId = new[] { "101" };

        public List<MatchPrediction> PredictionList { get; set; }

        /// <summary>
        /// Final score in percentage from report
        /// note hard rounded to nearest for better accuracy
        /// </summary>
        public double KutaScore { get; set; }

        /// <summary>
        /// Yeah! ML Embeddings for kuta! world's 1st 🌍
        /// </summary>
        public double[] Embeddings { get; set; }

        public Person Male { get; set; }

        public Person Female { get; set; }

        /// <summary>
        /// User ID is used by website. Multiple supported, Shows owners
        /// </summary>
        public string[] UserId { get; set; } = new[] { "101" };

        /// <summary>
        /// Comma separated string of Owners
        /// </summary>
        public string UserIdString
        {
            get
            {
                var userIdString = "";

                //joining can fail, so return error note if that happens
                try
                {
                    userIdString = string.Join(",", UserId);
                }
                catch (Exception e)
                {
                    //add message to data to be spotted later
                    userIdString = e.Message;
                }

                return userIdString;
            }
        }

        /// <summary>
        /// Notes to be filled by user more about the match report
        /// </summary>
        public string Notes { get; set; }

        public string Id { get; set; }

        /// <summary>
        /// Dynamic text to summarize the compatibility based on Kuta score
        /// </summary>
        public MatchSummaryData Summary => GetSummary(KutaScore);

        public MatchReport(string id, Person male, Person female, double kutaScore, string notes, List<MatchPrediction> predictionList, string[] userId)
        {
            Id = id;
            Male = male;
            Female = female;
            KutaScore = kutaScore;
            Notes = notes;
            PredictionList = predictionList;
            UserId = userId.Any() ? userId : DefaultUserId;
        }



        //-------------------TO JSON IMPLEMENTATION ----------------------


        JObject IToJson.ToJson() => (JObject)this.ToJson();

        public JToken ToJson()
        {
            var temp = new JObject();
            temp["Embeddings"] = new JArray(this.Embeddings);
            temp["KutaScore"] = this.KutaScore;  //not rounded
            temp["Notes"] = this.Notes; 
            temp["Male"] = Male.ToJson();
            temp["Female"] = Female.ToJson();
            temp["PredictionList"] = MatchPrediction.ToJsonList(this.PredictionList);
            temp["Summary"] = this.Summary.ToJson();
            temp["UserId"] = this.UserIdString; //TODO marked for oblivion
            temp["Id"] = this.Id;

            return temp;
        }

       




        //█ █▄░█ ▀█▀ █▀▀ █▀█ █▄░█ ▄▀█ █░░   █▀▄▀█ █▀▀ ▀█▀ █░█ █▀█ █▀▄ █▀
        //█ █░▀█ ░█░ ██▄ █▀▄ █░▀█ █▀█ █▄▄   █░▀░█ ██▄ ░█░ █▀█ █▄█ █▄▀ ▄█
        //----------------------------------------------------------------------------------------------------------------


        /// <summary>
        /// Based on kuta score will summary data
        /// text color, heart icon, summary text for given score
        /// </summary>
        private static MatchSummaryData GetSummary(double kutaScore)
        {

            if (kutaScore < 15)
            {
                var heartIcon = "ic:round-heart-broken";
                var scoreColor = "#ff6969";
                var scoreSummary = "Not best, avoid if possible";
                return new MatchSummaryData(heartIcon, scoreColor, scoreSummary);
            }

            if (kutaScore >= 15 && kutaScore < 30)
            {
                var heartIcon = "mdi:heart-flash";
                var scoreColor = "#ff6969"; 
                var scoreSummary = "Problematic relationship";
                return new MatchSummaryData(heartIcon, scoreColor, scoreSummary);
            }

            if (kutaScore >= 30 && kutaScore < 40)
            {
                var heartIcon = "mdi:heart-half-full";
                var scoreColor = "#ff6969"; 
                var scoreSummary = "Better than the worst but not best";
                return new MatchSummaryData(heartIcon, scoreColor, scoreSummary);

            }

            if (kutaScore >= 40 && kutaScore < 50)
            {
                var heartIcon = "mdi:heart-half-full";
                var scoreColor = "#ff6969"; 
                var scoreSummary = "Average relationship, equal good and bad";
                return new MatchSummaryData(heartIcon, scoreColor, scoreSummary);

            }

            //tipping point for GOOD and BAD at anything more than 50%
            if (kutaScore >= 50 && kutaScore < 60)
            {
                var heartIcon = "mdi:cards-heart";
                var scoreColor = "#00a702"; 
                var scoreSummary = "Better than average, more good than bad";
                return new MatchSummaryData(heartIcon, scoreColor, scoreSummary);
            }

            if (kutaScore >= 60 && kutaScore <= 80)
            {
                var heartIcon = "mdi:heart-plus";
                var scoreColor = "#00a702";
                var scoreSummary = "Near perfect match, overall happiness";
                return new MatchSummaryData(heartIcon, scoreColor, scoreSummary);
            }

            if (kutaScore > 80)
            {
                var heartIcon = "bi:arrow-through-heart-fill";
                var scoreColor = "#00a702";
                var scoreSummary = "Best possible match, rare in real life";
                return new MatchSummaryData(heartIcon, scoreColor, scoreSummary);
            }

            throw new Exception("END OF THE LINE");

        }

    }
}