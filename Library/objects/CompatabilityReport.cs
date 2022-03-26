using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Genso.Astrology.Library;

namespace Genso.Astrology.Library.Compatibility
{
    /// <summary>
    /// Represents the final data generated for compatibility
    /// </summary>
    public class CompatibilityReport
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
        public XElement ToXML()
        {
            //create root tag to hold data
            var compatibilityReport = new XElement("CompatibilityReport");

            //place data in individual tags
            var kutaScore = new XElement("KutaScore", this.KutaScore);
            var male = new XElement("Male", Male.toXML());
            var female = new XElement("Female", Female.toXML());
            var predictionList = PredictionListToXML(this.PredictionList);

            //add in the data
            compatibilityReport.Add(kutaScore,male, female, predictionList);

            return compatibilityReport;

        }

        private XElement PredictionListToXML(List<CompatibilityPrediction> predictionList)
        {
            //create root tag to hold data
            var predictionListXml = new XElement("PredictionList");

            //convert each prediction to XML and add it in
            foreach (var prediction in predictionList)
            {
                predictionListXml.Add(prediction.ToXML());
            }

            return predictionListXml;
        }

        public static CompatibilityReport FromXml(XElement compatibilityReportXml)
        {
            var male = Person.fromXml(compatibilityReportXml.Element("Male").Element("Person"));
            var female = Person.fromXml(compatibilityReportXml.Element("Female").Element("Person"));
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

            //FUNCTIONS

            List<CompatibilityPrediction> ParseXmlToPredictionList(XElement xmlData)
            {
                var returnVal = new List<CompatibilityPrediction>();

                foreach (var xElement in xmlData.Elements())
                {
                    var name = Enum.Parse<Name>(xElement.Element("Name")?.Value);
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
