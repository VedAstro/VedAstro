namespace Website
{

    /// <summary>
    /// A centralized place to store hardcoded page links
    /// </summary>
    public static class PageRoute
    {
        public const string NasaJPLSource = "https://naif.jpl.nasa.gov/pipermail/spice_announce/2007-August/000055.html";
        public const string SwissEphSource = "https://www.astro.com/swisseph/swephinfo_e.htm";
        public const string SwissEphemerisAPI = "API/SwissEphemeris";
        public const string HouseAPI = "API/House";
        public const string PlanetAPI = "API/Planet";
        public const string OpenAPI = "API/Intri";
        public const string SavedCharts = "/SavedCharts";
        public const string Login = "/Login";
        public const string Journal = "/Journal";
        public const string JournalParam = "/Journal/{PersonIdUrl}";
        public const string TaskList = "/Tasklist";
        public const string MessageList = "/MessageList";
        public const string SearchResult = "/SearchResult";
        public const string SearchResultParam = "/SearchResult/{SearchText}";
        public const string Debug = "/Debug";
        public const string AskAstrologer = "/AskAstrologer";
        
        //CALCULATORS
        public const string CalculatorList = "/Calculator";
        public const string SunRiseSetTime = "Calculator/SunRiseSetTime";
        public const string BirthTimeFinder = "Calculator/BirthTimeFinder";
        public const string LocalMeanTime = "Calculator/LocalMeanTime";
        public const string Horoscope = "Calculator/Horoscope";
        public const string HoroscopeParam = "Calculator/Horoscope/{PersonIdUrl}";
        public const string HoroscopeData = "Calculator/HoroscopeData";
        public const string Muhurtha = "Calculator/Muhurtha";
        public const string Match = "Calculator/Match";
        public const string MatchReport = "Calculator/MatchReport";
        public const string MatchReportParam = "Calculator/MatchReport/{MaleId}/{FemaleId}";

        //PAGES
        public const string VisitorList = "/VisitorList";
        public const string TaskEditor = "/TaskEditor";
        public const string TaskEditorParam = "/TaskEditor/{TaskHash}";
        public const string PersonList = "/PersonList";
        public const string PersonEditor = "/PersonEditor";
        public const string PersonEditorParam = "/PersonEditor/{PersonId}";
        public const string Donate = "/Donate";
        public const string DonatePayment = $"{Donate}/Payment";
        public const string About = "/About";
        public const string Contact = "/Contact";
        public const string AddLifeEvent = "/AddLifeEvent";
        public const string AddPerson = "/AddPerson";
        public const string Dasa = "/Dasa";
        public const string QuickGuide = "/QuickGuide";
        public const string DasaCached = "/DasaCached";
        public const string FeatureList = "/FeatureList";
        public const string Home = "/";

        //LINKS
        public const string HttpHome = "https://www.vedastro.org";
        public const string PatreonPage = "https://patreon.com/vedastro";
        public const string KoFiPage = "https://ko-fi.com/vedastro";
        public const string PaypalMePage = "https://paypal.me/VedAstroOrg";
        public const string AddPersonGuideVideo = "https://youtu.be/RDUPsFOrr3c";
        public const string BlogWhyVedic = "/Blog/WhyVedic";
    }
}
