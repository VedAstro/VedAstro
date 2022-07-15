namespace Website
{

    /// <summary>
    /// A centralized place to store hardcoded page links
    /// </summary>
    public static class PageRoute
    {
        public const string TaskList = "/tasklist";
        public const string VisitorList = "/visitorlist";
        public const string TaskEditor = "/taskeditor";
        public const string PersonList = "/personlist";
        public const string PersonEditor = "/personeditor";
        public const string PersonEditorParam = "/personeditor/{PersonHash}";
        public const string Donate = "/donate";
        public const string DonatePayment = $"{Donate}/payment";
        public const string About = "/about";
        public const string Contact = "/contact";
        public const string Horoscope = "/horoscope";
        public const string Muhurtha = "/muhurtha";
        public const string Match = "/match";
        public const string MatchReport = "/matchreport";
        public const string SunRiseSetTime = "/sunrisesettime";
        public const string AddLifeEvent = "/addlifeevent";
        public static string LocalMeanTime = "/localmeantime";
        public static string Dasa = "/dasa";
        public static string QuickGuide = "/quickguide";
    }
}
