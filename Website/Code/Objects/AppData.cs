using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Xml.Linq;
using Library.API;
using VedAstro.Library;
using Website.Pages;
using Website.Pages.Account;

namespace Website
{
    /// <summary>
    /// Static class to hold global site data
    /// </summary>
    public static class AppData
    {
        //-----------------------------FIELDS

        public static int FailureCount = 0;

        /// <summary>
        /// Shortcut text to show credit
        /// </summary>
        public static MarkupString HinduPredictiveAstrologyCredit = new("<p class=\"fst-italic fw-light\"><small>(Source Credit: Hindu Predictive Astrology by B.V.Raman)</small></p>");

        public static MarkupString MuhurthaCredit = new("<p class=\"fst-italic fw-light\"><small>(Source Credit: Muhurtha by B.V.Raman)</small></p>");

        /// <summary>
        /// Represents the currently logged in User
        /// If null then nobody logged in, default Empty/Public User
        /// </summary>
        public static UserData CurrentUser { get; set; } = UserData.Guest;

        /// <summary>
        /// default to 101, though it should be updated when running, else must be error
        /// A copy of this visitor's ID, set when logging new visit
        /// Different from User Id in current user
        /// Used to log user flow
        /// Note: defaults to empty string
        /// </summary>
        public static string VisitorId { get; set; } = "101";

        /// <summary>
        /// Place where global event data list is stored for quick access
        /// loaded in main layout
        /// </summary>
        public static List<XElement>? HoroscopeDataList { get; set; }

        public static Stream? HoroscopeDataListStream { get; set; }

        /// <summary>
        /// Place where global event data list is stored for quick access
        /// loaded in main layout
        /// </summary>
        public static List<XElement> EventDataList { get; set; }

        /// <summary>
        /// global ReferenceList.xml data list is stored for quick access
        /// loaded in main layout
        /// </summary>
        public static List<XElement> ReferenceList { get; set; }

        public static SearchResult SearchPage { get; set; }

        /// <summary>
        /// Note access via try get person list
        /// To clear & get fresh on next call set null
        /// </summary>
        public static List<Person>? PersonList { get; set; }

        /// <summary>
        /// If true new Visitor is first visit (theoretically)
        /// Sets true on every page refresh
        /// </summary>
        public static bool IsNewVisitor { get; set; } = true;

        /// <summary>
        /// Origin URL set by MainLayout
        /// </summary>
        public static Task<string> OriginUrl => JsRuntime.GetOriginUrl();

        /// <summary>
        /// Gets latest current page URL using JS
        /// </summary>
        public static Task<string> CurrentUrlJS => JsRuntime.GetCurrentUrl();

        /// <summary>
        /// Return true if User ID is 101
        /// </summary>
        public static bool IsGuestUser => AppData.CurrentUser?.Id == UserData.Guest.Id;

        public static bool DarkMode { get; set; } = false;

        /// <summary>
        /// JS runtime instance set in program
        /// </summary>
        public static IJSRuntime JsRuntime
        {
            get { return _jsRuntime; }
            set
            {
                Console.WriteLine("BLZ:Global JSRuntime Initialized");
                _jsRuntime = value;
            }
        }

        /// <summary>
        /// If true means, loading box is still in show mode
        /// note: main purpose to stop execution until message has popped (shown)
        /// else serious lag at times
        /// </summary>
        public static bool IsShowLoading { get; set; } = false; //default false

        /// <summary>
        /// If true login is success
        /// </summary>
        public static bool IsLoginSuccess => AppData.CurrentUser != UserData.Guest;

        /// <summary>
        /// set by when app starts
        /// </summary>
        public static URL URL { get; set; }

        /// <summary>
        /// set by when app starts
        /// </summary>
        public static HttpClient HttpClient { get; set; }

        /// <summary>
        /// Default icon size used in pages
        /// </summary>
        public static int DefaultIconSize => 38;

        /// <summary>
        /// updated every time location is set by user, when making new geo location this is used as default
        /// </summary>
        public static string? LastUsedLocation { get; set; } = DefaultLocationCountry;

        /// <summary>
        /// image html of loading icon ready to be used razor page (shortcut method)
        /// </summary>
        public static RenderFragment LoadingImage => (builder) =>
        {
            builder.AddMarkupContent(0, $"<img style=\"position: relative; left: 39%; top: 30%;\" src=\"images/loading-animation-progress-transparent.gif\" />");
        };

        /// <summary>
        /// filled when loading app via Google API IP location detect
        /// used as backup location for selector
        /// </summary>
        public static GeoLocation ClientLocation { get; set; }

        public static string DefaultLocationCountry = "Singapore";


        /// <summary>
        /// Base address currently used by App,
        /// could be http://localhost / www.vedastro.org / vedastro.org / beta.vedastro.org
        /// </summary>
        public static Uri? BaseAddress;

        private static IJSRuntime _jsRuntime;

        /// <summary>
        /// Counts the number of times the stamp was clicked
        /// </summary>
        public static int StampClickCount;

        /// <summary>
        /// manager to access everything API
        /// </summary>
        public static VedAstroAPI API;


        /// <summary>
        /// if data already loaded then return the that one,
        /// else get a new one from server
        /// </summary>
        public static async Task<Stream?> GetPredictionDataStreamCached()
        {
            //return already loaded if available
            if (AppData.HoroscopeDataListStream != null) return AppData.HoroscopeDataListStream;

            //else get fresh copy from server
            AppData.HoroscopeDataListStream = await AppData.HttpClient.GetStreamAsync("data/HoroscopeDataList.xml");
            return AppData.HoroscopeDataListStream;
        }

        public static async Task<List<XElement>?> GetHoroscopeDataListCached()
        {
            //return already loaded if available
            if (AppData.HoroscopeDataList != null) return AppData.HoroscopeDataList;

            //else get fresh copy from server
            AppData.HoroscopeDataList = await WebsiteTools.GetXmlFile("data/HoroscopeDataList.xml");
            return AppData.HoroscopeDataList;
        }

        /// <summary>
        /// Gets currently set Dark Mode from JS lib and sets in AppData
        /// </summary>
        public static async Task UpdateDarkMode(IJSRuntime jsRuntime)
        {
            try
            {
                //get value from JS and save it, for others if needed
                AppData.DarkMode = await jsRuntime.InvokeAsync<bool>(JS.DarkMode_isActivated);
            }
            catch (Exception e)
            {
                Console.WriteLine("BLZ:Update dark mode silent fail!");
            }
        }

        public static async Task IfNoLoadingBoxPleaseHold(string caller = "")
        {
            //hold till loading is visible
            while (!AppData.IsShowLoading)
            {
                Console.WriteLine($"BLZ:Waiting for loading box");
                await Task.Delay(100);
                //_waitingInLineCount++; //increment  count
            }

            //reset
            //_waitingInLineCount = 0;
        }


        /// <summary>
        /// When called clears person list from memory, so new list is auto loaded from API on next get
        /// </summary>
        //public static void ClearPersonList() => AppData.PersonList = null;

    }
}