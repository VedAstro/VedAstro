using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genso.Astrology.Library
{
    /// <summary>
    /// Holds info (Dasa, Bhukti & Antaram) on the ruling period at point in time.
    /// Dasas are major periods in which the indications of the planets are realised.
    /// </summary>
    public struct Dasas
    {
        //DATA FIELDS
        public PlanetName Dasa { get; set; }
        public PlanetName Bhukti { get; set; }
        public PlanetName Antaram { get; set; }
    }
}


