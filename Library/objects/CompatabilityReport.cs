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

        public static MatchReport Empty = new MatchReport(Person.Empty, Person.Empty, 0, "Empty Notes",
            new List<CompatibilityPrediction>(), new[] { "101" }); //have to use direct variables

        private static readonly string[] DefaultUserId = new[] { "101" };

        public List<CompatibilityPrediction> PredictionList { get; set; }

        /// <summary>
        /// Final score in percentage from report
        /// </summary>
        public double KutaScore { get; set; }

        public Person Male { get; set; }

        public Person Female { get; set; }

        //todo should be dynamic
        public string ScoreColor => this.KutaScore > 50 ? "Green" : "Red";

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



        public MatchReport(Person male, Person female, double kutaScore,string notes, List<CompatibilityPrediction> predictionList, string[] userId)
        {
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
            var kutaScore = new XElement("KutaScore", this.KutaScore);
            var male = new XElement("Male", Male.ToXml());
            var female = new XElement("Female", Female.ToXml());
            var predictionList = PredictionListToXml(this.PredictionList);
            var userId = new XElement("UserId", this.UserIdString);

            //add in the data
            compatibilityReport.Add(userId, male, female, kutaScore,  predictionList);

            return compatibilityReport;
        }

        private XElement PredictionListToXml(List<CompatibilityPrediction> predictionList)
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
        /// The root element is expected to be Person
        /// Note: Special method done to implement IToXml
        /// </summary>
        public dynamic FromXml<T>(XElement xml) where T : IToXml => FromXml(xml);

        /// <summary>
        /// If unparseable will return null
        /// </summary>
        public static MatchReport FromXml(XElement compatibilityReportXml)
        {
            //end here if null
            if (compatibilityReportXml == null) { return null; }

            //it is possible xml is not valid data, possible error xml, so end here
            if (compatibilityReportXml?.Element("Male")?.Element("Person") == null) { return null; }

            //extract out data from xml
            var male = Person.FromXml(compatibilityReportXml.Element("Male")?.Element("Person"));
            var female = Person.FromXml(compatibilityReportXml.Element("Female")?.Element("Person"));
            var kutaScore = Double.Parse(compatibilityReportXml.Element("KutaScore")?.Value ?? "0");
            var userId = Tools.GetUserIdFromXmlData(compatibilityReportXml);

            var predictionListXml = compatibilityReportXml.Element("PredictionList");
            var predictionList = ParseXmlToPredictionList(predictionListXml);


            //package as new data
            var newCompatibilityReport = new MatchReport(male, female, kutaScore, "Notes", predictionList, userId);
            return newCompatibilityReport;

            //----------------------------------------------
            //FUNCTIONS

            List<CompatibilityPrediction> ParseXmlToPredictionList(XElement xmlData)
            {
                var returnVal = new List<CompatibilityPrediction>();

                foreach (var xElement in xmlData.Elements())
                {
                    var name = Enum.Parse<MatchPredictionName>(xElement.Element("Name")?.Value);
                    var nature = Enum.Parse<EventNature>(xElement.Element("Nature")?.Value);
                    var maleInfo = xElement.Element("MaleInfo")?.Value;
                    var femaleInfo = xElement.Element("FemaleInfo")?.Value;
                    var description = xElement.Element("Description")?.Value;
                    var info = xElement.Element("Info")?.Value;

                    var newPrediction = new CompatibilityPrediction()
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

    }
}