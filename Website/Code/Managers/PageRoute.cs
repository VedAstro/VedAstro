namespace Website
{

    /// <summary>
    /// A centralized place to store hardcoded page links only for BLAZOR
    /// for API & others go to URL class, it's got the stuff you want
    /// </summary>
    public static class PageRoute
    {


        public const string TaskList = "/Tasklist";
        public const string MessageList = "/MessageList";
        public const string SearchResult = "/SearchResult";
        public const string SearchResultParam = "/SearchResult/{SearchText}";
        public const string Debug = "/Debug";
        public const string AskAstrologer = "/AskAstrologer";
        public const string TrainAIAstrologer = "/TrainAIAstrologer";

        //DOCS
        public const string QuickGuide = "Docs/QuickGuide";
        public const string Glossary = "Docs/Glossary";

        //DOCS > API
        public const string HouseApi = "Docs/API/House";
        public const string PlanetApi = "Docs/API/Planet";
        public const string OpenApi = "Docs/API/";
        public const string MatchApi = "Docs/API/Match";
        public const string NLPApi = "Docs/API/NLP";
        
        //DEVELOPER
        public const string Developer = "Developer/";
        public const string DeveloperParam = "Developer/{TabName}";
        public const string Docker = "https://github.com/VedAstro/VedAstro/discussions/8";
        public const string NuGet = "https://github.com/VedAstro/VedAstro/discussions/18";

        //JOURNAL
        public const string Journal = "Journal/";
        public const string JournalParam = "/Journal/{PersonIdUrl}";
        public const string JournalAdd = "Journal/Add";
        public const string JournalEditor = "Journal/Editor";

        
        //CALCULATORS
        public const string CalculatorList = "Calculator/";
        public const string LifePredictor = "Calculator/LifePredictor";
        public const string LifePredictorParam = "Calculator/LifePredictor/{PersonIdUrl}";
        public const string StarsAboveMe = "Calculator/StarsAboveMe";
        public const string SunRiseSetTime = "Calculator/SunRiseSetTime";
        public const string BirthTimeFinder = "Calculator/BirthTimeFinder";
        public const string LocalMeanTime = "Calculator/LocalMeanTime";
        public const string Horoscope = "Calculator/Horoscope";
        public const string HoroscopeParam = "Calculator/Horoscope/{PersonIdUrl}";
        public const string Muhurtha = "Calculator/Muhurtha";
        public const string FamilyChart = "Calculator/FamilyChart";
        public const string MatchReport = "Calculator/Match/Report";
        public const string APIBuilder = "Calculator/APIBuilder";


        //MATCH
        public const string Match = "Calculator/Match";
        public const string SavedMatchReports = "Calculator/Match/Saved";
        public const string MatchProfile = "Calculator/Match/Profile";
        public const string MatchReportParam = "Calculator/Match/Report/{MaleId}/{FemaleId}";
        public const string MatchFinder = "Calculator/Match/Finder";



        //DONATE
        public const string Donate = "Donate/";
        public const string DonateOld = "DonateOld/";
        public const string DonatePayment = "Donate/Payment"; //todo maybe not needed
        public const string DonateSpacy = "Donate/spaCyAPI";


        //ACCOUNT
        public const string UserAccount = "Account/";
        public const string Login = "Account/Login";
        public const string SavedCharts = "Account/SavedCharts";
        public const string PersonList = "Account/Person/List";
        public const string AddPerson = "Account/Person/Add";
        public const string PersonEditor = "Account/Person/Editor"; //used to make nav link
        public const string PersonEditorParam = "Account/Person/Editor/{PersonIdUrl}"; //actual page access


        //LITTLE PAGES
        public const string NowInDwapara = "/NowInDwapara";
        public const string Remedy = "/Remedy";
        public const string Download = "/Download";
        public const string VisitorList = "/VisitorList";
        public const string FAQ = "/FAQ";
        public const string TaskEditor = "/TaskEditor";
        public const string TaskEditorParam = "/TaskEditor/{TaskHash}";
        public const string About = "/About";
        public const string JoinOurFamily = "/JoinOurFamily";
        public const string Contact = "/Contact";
        public const string MadeOnEarth = "/MadeOnEarth";
        public const string FeatureList = "/FeatureList";
        public const string Home = "/";

        //LINKS
        public const string BlogWhyVedic = "/Blog/WhyVedic";

        public const string EasterEgg = "data/EasterEgg.txt";

    }
}
