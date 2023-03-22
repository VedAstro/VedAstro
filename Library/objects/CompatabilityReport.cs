using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace VedAstro.Library.Compatibility
{
    /// <summary>
    /// Represents the final data generated for compatibility
    /// </summary>
    public class CompatibilityReport : IToXml
    {

        public List<CompatibilityPrediction> PredictionList { get; set; }

        /// <summary>
        /// Final score in percentage from report
        /// </summary>
        public double KutaScore { get; set; }
        public Person Male { get; set; }
        public Person Female { get; set; }

        /// <summary>
        /// Converts the instance data into XML
        /// Used for transmitting across net
        /// </summary>
        public XElement ToXml()
        {
            //create root tag to hold data
            var compatibilityReport = new XElement("CompatibilityReport");

            //place data in individual tags
            var kutaScore = new XElement("KutaScore", this.KutaScore);
            var male = new XElement("Male", Male.ToXml());
            var female = new XElement("Female", Female.ToXml());
            var predictionList = PredictionListToXml(this.PredictionList);

            //add in the data
            compatibilityReport.Add(kutaScore, male, female, predictionList);

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
        public static CompatibilityReport FromXml(XElement compatibilityReportXml)
        {
            //end here if null
            if (compatibilityReportXml == null) { return null; }

            //it is possible xml is not valid data, possible error xml, so end here
            if (compatibilityReportXml?.Element("Male")?.Element("Person") == null) { return null; }

            //extract out data from xml
            var male = Person.FromXml(compatibilityReportXml.Element("Male")?.Element("Person"));
            var female = Person.FromXml(compatibilityReportXml.Element("Female")?.Element("Person"));
            var kutaScore = Double.Parse(compatibilityReportXml.Element("KutaScore")?.Value ?? "0");

            var predictionListXml = compatibilityReportXml.Element("PredictionList");
            var predictionList = ParseXmlToPredictionList(predictionListXml);

            var newCompatibilityReport = new CompatibilityReport()
            {
                Male = male,
                Female = female,
                KutaScore = kutaScore,
                PredictionList = predictionList
            };

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
    }
}
