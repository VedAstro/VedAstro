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

        //DEVELOPER
        public const string Developer = "Developer/";
        public const string DeveloperParam = "Developer/{TabName}";
        public const string Docker = "https://github.com/VedAstro/VedAstro/discussions/8";
        public const string NuGet = "https://github.com/VedAstro/VedAstro/discussions/18";

        //JOURNAL
        public const string Journal = "Journal";
        public const string JournalParam = "/Journal/{PersonIdUrl}";
        public const string JournalEditorParam = "Journal/Editor/{PersonIdUrl}/{LifeEventIdUrl}"; //for page declaration
        public const string JournalEditor = "Journal/Editor"; //for page URL generation


        //CALCULATORS
        public const string CalculatorList = "Calculator/";
        public const string LifePredictor = "LifePredictor";
        public const string LifePredictorHD = "LifePredictorHD";
        public const string LifePredictorParam = "LifePredictor/{PersonIdUrl}";
        public const string GoodTimeFinder = "GoodTimeFinder";
        public const string StarsAboveMe = "StarsAboveMe";
        public const string TableGenerator = "TableGenerator";
        public const string TimeListGenerator = "TimeListGenerator";
        public const string Numerology = "Numerology";
        public const string SunRiseSetTime = "SunRiseSetTime";
        public const string BirthTimeFinder = "BirthTimeFinder";
        public const string LocalMeanTime = "LocalMeanTime";
        public const string Horoscope = "Horoscope";
        public const string HoroscopeParam = "Horoscope/{PersonIdUrl}";
        public const string FamilyChart = "FamilyChart";
        public const string MatchReport = "Match/Report";
        public const string APIBuilder = "APIBuilder";
        public const string APICallList = "APIBuilder#APICallList";
        public const string UrlDisplayOut = "APIBuilder#UrlDisplayOut";
        public const string GeneratedTableOut = "TableGenerator#GeneratedTable";
        public const string DataColumnSelector = "TableGenerator#DataColumnSelector";


        //MATCH
        public const string Match = "Match";
        public const string SavedMatchReports = "Match/Saved";
        public const string MatchProfile = "Match/Profile";
        public const string MatchReportParam = "Match/Report/{MaleId}/{FemaleId}";
        public const string MatchFinder = "Match/Finder";



        //DONATE
        public const string ThankYou = "Donate/ThankYou";
        public const string Donate = "Donate/";
        public const string DonateOld = "DonateOld/";
        public const string DonatePayment = "Donate/Payment"; //contains Paypal sample code


        //ACCOUNT
        public const string UserAccount = "Account/";
        public const string UserAccountGuest = "Account/Guest";
        public const string Login = "Account/Login";
        public const string LoginRememberMe = "Account/Login/RememberMe";
        public const string SavedCharts = "Account/SavedCharts";
        public const string PersonList = "Account/Person/List";
        public const string AddPerson = "Account/Person/Add";
        public const string Import = "Account/Person/Import";
        public const string PersonEditor = "Account/Person/Editor"; //used to make nav link
        public const string PersonEditorParam = "Account/Person/Editor/{PersonIdUrl}"; //actual page access


        //LITTLE PAGES
        public const string NowInDwapara = "/NowInDwapara";
        public const string Remedy = "/Remedy";
        public const string Download = "/Download";
        public const string VisitorList = "/VisitorList";
        public const string VisitorListOld = "/VisitorListOld";
        public const string FAQ = "/FAQ";
        public const string TaskEditor = "/TaskEditor";
        public const string TaskEditorParam = "/TaskEditor/{TaskHash}";
        public const string About = "/About";
        public const string PrivacyPolicy = "/PrivacyPolicy";
        public const string ShippingDelivery = "/ShippingDelivery";
        public const string Sitemap = "/sitemap.xml";
        public const string CancellationRefund = "/CancellationRefund";
        public const string TermsOfService = "/TermsOfService";
        public const string ChatAPI = "/ChatAPI";
        public const string Payment = "/Payment";
        public const string Sponsor = "/Sponsor";
        public const string VSLifeSharePublicSession = "/VSLifeSharePublicSession";
        public const string PrivateServer = "/PrivateServer";
        public const string JoinOurFamily = "/JoinOurFamily";
        public const string BodyTypes = "/BodyTypes";
        public const string Contact = "/Contact";
        public const string MadeOnEarth = "/MadeOnEarth";
        public const string FeatureList = "/FeatureList";
        public const string Home = "/";

        //LINKS
        public const string BlogWhyVedic = "/Blog/WhyVedic";

        public const string EasterEgg = "data/EasterEgg.txt";

    }
}
