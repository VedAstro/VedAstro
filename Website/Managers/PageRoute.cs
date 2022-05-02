namespace Website
{

    /// <summary>
    /// A centralized place to store hardcoded page links
    /// </summary>
    public static class PageRoute
    {
        public const string TaskList = "/tasklist";
        public const string TaskEditor = "/taskeditor";
        public const string PersonList = "/personlist";
        public const string PersonEditor = "/personeditor";
        public const string PersonEditorParam = "/personeditor/{PersonHash}";
        public const string Donate = "/donate";
        public const string Horoscope = "/horoscope";
        public const string Muhurtha = "/muhurtha";
        public const string Match = "/match";
        public const string SunRiseSetTime = "/sunrisesettime";
        public static string LocalMeanTime = "/localmeantime";
        public static string Dasa = "/dasa";
        public static string QuickGuide = "/quickguide";
    }
}
