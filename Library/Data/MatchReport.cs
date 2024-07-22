using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace VedAstro.Library
{
    /// <summary>
    /// Represents the final data generated for compatibility
    /// </summary>
    public class MatchReport : IToXml
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

        /// <summary>
        /// Converts the instance data into XML
        /// Used for transmitting across net
        /// </summary>
        public XElement ToXml()
        {
            //create root tag to hold data
            var compatibilityReport = new XElement("MatchReport");

            //place data in individual tags
            var kutaScore = new XElement("KutaScore", this.KutaScore); //not rounded
            var male = new XElement("Male", Male.ToXml());
            var female = new XElement("Female", Female.ToXml());
            var predictionList = PredictionListToXml(this.PredictionList);
            var userId = new XElement("UserId", this.UserIdString);
            var notes = new XElement("Notes", this.Notes);
            var matchId = new XElement("Id", Id);

            //add in the data
            compatibilityReport.Add(matchId, userId, notes, male, female, kutaScore, predictionList);

            return compatibilityReport;
        }

        public JToken ToJson()
        {

            var temp = new JObject();
            temp["Embeddings"] = new JArray(this.Embeddings);
            temp["KutaScore"] = this.KutaScore;  //not rounded
            temp["Notes"] = this.Notes;  //not rounded
            temp["Male"] = Male.ToJson();
            temp["Female"] = Female.ToJson();
            temp["PredictionList"] = MatchPrediction.ToJsonList(this.PredictionList);
            temp["UserId"] = this.UserIdString;
            temp["Id"] = this.Id;

            return temp;
        }


        /// <summary>
        /// The root element is expected to be Person
        /// Note: Special method done to implement IToXml
        /// </summary>
        public dynamic FromXml<T>(XElement xml) where T : IToXml => FromXml(xml);

        /// <summary>
        /// If unparseable will return null
        /// </summary>
        public static MatchReport FromXml(XElement reportXml)
        {
            //end here if null
            if (reportXml == null) { return null; }

            //it is possible xml is not valid data, possible error xml, so end here
            if (reportXml?.Element("Male")?.Element("Person") == null) { return null; }

            //extract out data from xml
            var matchId = reportXml.Element("Id")?.Value ?? "0";
            var notes = reportXml.Element("Notes")?.Value ?? "Notes";
            var male = Person.FromXml(reportXml.Element("Male")?.Element("Person"));
            var female = Person.FromXml(reportXml.Element("Female")?.Element("Person"));
            var kutaScore = Double.Parse(reportXml.Element("KutaScore")?.Value ?? "0");
            var userId = Tools.GetUserIdFromData(reportXml);

            var predictionListXml = reportXml.Element("PredictionList");
            var predictionList = ParseXmlToPredictionList(predictionListXml);


            //package as new data
            var newCompatibilityReport = new MatchReport(matchId, male, female, kutaScore, notes, predictionList, userId);
            return newCompatibilityReport;

            //----------------------------------------------
            //FUNCTIONS

            List<MatchPrediction> ParseXmlToPredictionList(XElement xmlData)
            {
                var returnVal = new List<MatchPrediction>();

                foreach (var xElement in xmlData.Elements())
                {
                    var name = Enum.Parse<MatchPredictionName>(xElement.Element("Name")?.Value);
                    var nature = Enum.Parse<EventNature>(xElement.Element("Nature")?.Value);
                    var maleInfo = xElement.Element("MaleInfo")?.Value;
                    var femaleInfo = xElement.Element("FemaleInfo")?.Value;
                    var description = xElement.Element("Description")?.Value;
                    var info = xElement.Element("Info")?.Value;

                    var newPrediction = new MatchPrediction()
                    {
                        Name = name,
                        Nature = nature,
                        MaleInfo = maleInfo,
                        FemaleInfo = femaleInfo,
                        Description = description,
                        Info = info
                    };

                    returnVal.Add(newPrediction);
                }

                return returnVal;
            }
        }

        /// <summary>
        /// Parse list of XML directly
        /// </summary>
        public static List<MatchReport> FromXml(IEnumerable<XElement> xmlList) => xmlList.Select(matchXml => MatchReport.FromXml(matchXml)).ToList();




        //█ █▄░█ ▀█▀ █▀▀ █▀█ █▄░█ ▄▀█ █░░   █▀▄▀█ █▀▀ ▀█▀ █░█ █▀█ █▀▄ █▀
        //█ █░▀█ ░█░ ██▄ █▀▄ █░▀█ █▀█ █▄▄   █░▀░█ ██▄ ░█░ █▀█ █▄█ █▄▀ ▄█
        //----------------------------------------------------------------------------------------------------------------


        private XElement PredictionListToXml(List<MatchPrediction> predictionList)
        {
            //create root tag to hold data
            var predictionListXml = new XElement("PredictionList");

            //convert each prediction to XML and add it in
            foreach (var prediction in predictionList)
            {
                predictionListXml.Add(prediction.ToXml());
            }

            return predictionListXml;
        }

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