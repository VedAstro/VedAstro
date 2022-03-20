using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Genso.Astrology.Library;

namespace Compatibility
{
    /// <summary>
    /// Represents the final data generated for compatibility
    /// </summary>
    internal class CompatibilityReport
    {
        public List<Prediction> PredictionList { get; set; }

        /// <summary>
        /// Final score in percentage from report
        /// </summary>
        public double KutaScore { get; set; }
        public Person Male { get; set; }
        public Person Female { get; set; }
    }
}
