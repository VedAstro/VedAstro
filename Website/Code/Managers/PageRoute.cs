namespace Website
{

    /// <summary>
    /// A centralized place to store hardcoded page links
    /// </summary>
    public static class PageRoute
    {
        public const string OpenAPI = "/OpenAPI";
        public const string SavedCharts = "/SavedCharts";
        public const string Login = "/Login";
        public const string Journal = "/Journal";
        public const string JournalParam = "/Journal/{PersonIdUrl}";
        public const string TaskList = "/Tasklist";
        public const string MessageList = "/MessageList";
        public const string SearchResult = "/SearchResult";
        public const string SearchResultParam = "/SearchResult/{SearchText}";
        public const string Debug = "/Debug";
        public const string AskAstrolger = "/AskAstrolger";
        public const string BirthTimeFinder = "/BirthTimeFinder";
        public const string VisitorListOld = "/VisitorListOld";
        public const string VisitorList = "/VisitorList";
        public const string TaskEditor = "/TaskEditor";
        public const string TaskEditorParam = "/TaskEditor/{TaskHash}";
        public const string PersonList = "/PersonList";
        public const string CalculatorList = "/CalculatorList";
        public const string PersonEditor = "/PersonEditor";
        public const string PersonEditorParam = "/PersonEditor/{PersonId}";
        public const string Donate = "/Donate";
        public const string DonatePayment = $"{Donate}/Payment";
        public const string About = "/About";
        public const string Contact = "/Contact";
        public const string Horoscope = "/Horoscope";
        public const string HoroscopeParam = "/Horoscope/{PersonIdUrl}";
        public const string HoroscopeData = "/HoroscopeData";
        public const string Muhurtha = "/Muhurtha";
        public const string Match = "/Match";
        public const string MatchReport = "/MatchReport";
        public const string MatchReportParam = "/MatchReport/{MaleId}/{FemaleId}";
        public const string SunRiseSetTime = "/SunRiseSetTime";
        public const string AddLifeEvent = "/AddLifeEvent";
        public const string AddPerson = "/AddPerson";
        public const string LocalMeanTime = "/LocalMeanTime";
        public const string Dasa = "/Dasa";
        public const string QuickGuide = "/QuickGuide";
        public const string DasaCached = "/DasaCached";
        public const string FeatureList = "/FeatureList";
        public const string Home = "/";
        public const string HttpHome = "https://www.vedastro.org";
        public const string PatreonPage = "https://patreon.com/vedastro";
        public const string KoFiPage = "https://ko-fi.com/vedastro";
        public const string PaypalMePage = "https://paypal.me/VedAstroOrg";
        public const string AddPersonGuideVideo = "https://youtu.be/RDUPsFOrr3c";
        public const string BlogWhyVedic = "/Blog/WhyVedic";
    }
}
