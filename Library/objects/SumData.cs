using System.Collections.Generic;

namespace VedAstro.Library
{

    /// <summary>
    /// Wrapper for summary data used to make events chart "summary row"
    /// </summary>
    public struct SumData
    {
        /// <summary>
        /// the final score to use when generating colors,
        /// voting should be done already
        /// </summary>
        public double NatureScore = 0;
        public List<PlanetName> Planet;
        public Time BirthTime;

        public SumData()
        {
            Planet = null;
        }
    }
}
