using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Genso.Astrology.Library;

namespace Compatibility
{
    /// <summary>
    /// Represents the final data generated for compatibility
    /// </summary>
    public class CompatibilityReport
    {
        public List<Prediction> PredictionList { get; set; }

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
            var male = new XElement("Male", this.Male.GetName());
            var female = new XElement("Female", this.Female.GetName());
            var predictionList = PredictionListToXML(this.PredictionList);

            //add in the data
            compatibilityReport.Add(kutaScore,male, female, predictionList);

            return compatibilityReport;

        }

        private XElement PredictionListToXML(List<Prediction> predictionList)
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
    }
}
