namespace VedAstro.Library
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
        public PlanetName Sukshma { get; set; }
        public PlanetName Prana { get; set; }
    }
}


