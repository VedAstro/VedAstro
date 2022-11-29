using System.Collections.Generic;

namespace Genso.Astrology.Library
{
    /// <summary>
    /// Simple data type to enclose the data coming out of a calculator.
    /// Prediction = did it occur & the strength 
    /// </summary>
    public class CalculatorResult
    {
        /// <summary>
        /// Indication if prediction is occurring
        /// </summary>
        public bool Occuring { get; set; }
        public string Info { get; set; }

        /// <summary>
        /// List of planets related to this calculation result
        /// </summary>
        public List<PlanetName> RelatedPlanets { get; set; }

        /// <summary>
        /// if specified overrides event nature from XML file
        /// will default to Empty when not set
        /// note: implemented to allow calculator method to modify final event nature
        /// </summary>
        public EventNature NatureOverride { get; set; }

        /// <summary>
        /// Defaults set here
        /// </summary>
        public CalculatorResult()
        {
            Info = "";
        }

        /// <summary>
        /// Return an Not Occuring Prediction
        /// </summary>
        public static CalculatorResult NotOccuring()
        {
            var prediction = new CalculatorResult()
            {
                Occuring = false,
                Info = ""
            };

            return prediction;
        }
        /// <summary>
        /// Return an Occuring Prediction
        /// </summary>
        public static CalculatorResult IsOccuring()
        {
            var prediction = new CalculatorResult()
            {
                Occuring = true,
                Info = ""
            };

            return prediction;
        }
    }
}
