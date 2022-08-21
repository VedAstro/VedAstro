namespace Website
{

    /// <summary>
    /// A centralized place to store hardcoded page links
    /// </summary>
    public static class PageRoute
    {
        public const string TaskList = "/tasklist";
        public const string BirthTimeFinder = "/BirthTimeFinder";
        public const string VisitorListOld = "/visitorlistold";
        public const string VisitorList = "/visitorlist";
        public const string TaskEditor = "/taskeditor";
        public const string TaskEditorParam = "/taskeditor/{TaskHash}";
        public const string PersonList = "/personlist";
        public const string CalculatorList = "/calculatorlist";
        public const string PersonEditor = "/personeditor";
        public const string PersonEditorParam = "/personeditor/{PersonHash}";
        public const string Donate = "/donate";
        public const string DonatePayment = $"{Donate}/payment";
        public const string About = "/about";
        public const string Contact = "/contact";
        public const string Horoscope = "/horoscope";
        public const string HoroscopeData = "/horoscopedata";
        public const string Muhurtha = "/muhurtha";
        public const string Match = "/match";
        public const string MatchReport = "/matchreport";
        public const string MatchReportParam = "/matchreport/{MaleHash}/{FemaleHash}";
        public const string SunRiseSetTime = "/sunrisesettime";
        public const string AddLifeEvent = "/addlifeevent";
        public const string LocalMeanTime = "/localmeantime";
        public const string Dasa = "/dasa";
        public const string QuickGuide = "/quickguide";
        public const string DasaCached = "/DasaCached";
        public const string FeatureList = "/featurelist";
        public const string Home = "/";
    }
}
