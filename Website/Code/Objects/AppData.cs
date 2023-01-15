using System.Xml.Linq;
using Genso.Astrology.Library;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Website.Pages;

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
        public static UserData CurrentUser { get; set; } = UserData.Empty;

        /// <summary>
        /// A copy of this visitor's ID, set when logging new visit
        /// Different from User Id in current user
        /// Used to log user flow
        /// Note: defaults to empty string
        /// </summary>
        public static string? VisitorId { get; set; } = "";


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
        /// </summary>
        public static bool IsNewVisitor { get; set; } = false;

        /// <summary>
        /// Origin URL set by MainLayout
        /// </summary>
        public static Task<string> OriginUrl =>  JsRuntime.GetOriginUrl();

        /// <summary>
        /// Gets latest current page URL using JS
        /// </summary>
        public static Task<string> CurrentUrlJS => JsRuntime.GetCurrentUrl();


        /// <summary>
        /// Return true if User ID is 101
        /// </summary>
        public static bool IsGuestUser => AppData.CurrentUser?.Id == UserData.Empty.Id;

        public static bool DarkMode { get; set; } = false;

        /// <summary>
        /// JS runtime instance set in program
        /// </summary>
        public static IJSRuntime JsRuntime
        {
            get {
                Console.WriteLine("BLZ:Global JSRuntime Initialized");
                return _jsRuntime; }
            set => _jsRuntime = value;
        }

        /// <summary>
        /// If true means, loading box is still in show mode
        /// note: main purpose to stop execution until message has popped
        /// else serious lag at times
        /// </summary>
        public static bool IsShowLoading { get; set; } = false; //default false

        /// <summary>
        /// Hard coded max width used in pages 
        /// </summary>
        public const string MaxWidth = "693px";

        public const string MaxContentWidthPx = "443px";
        
        /// <summary>
        /// Base address currently used by App,
        /// could be http://localhost or https://www.vedastro.org
        /// </summary>
        public static Uri? BaseAddress;

        private static IJSRuntime _jsRuntime;


        /// <summary>
        /// if data already loaded then return the that one,
        /// else get a new one from server
        /// </summary>
        public static async Task<Stream?> GetPredictionDataStreamCached()
        {
            //return already loaded if available
            if (AppData.HoroscopeDataListStream != null) return AppData.HoroscopeDataListStream;

            //else get fresh copy from server
            var client = new HttpClient();
            client.BaseAddress = AppData.BaseAddress;
            AppData.HoroscopeDataListStream = await client.GetStreamAsync("data/HoroscopeDataList.xml");
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
                AppData.DarkMode = await jsRuntime.InvokeAsync<bool>("window.DarkMode.isActivated");
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
        /// data may be cached or from API
        /// Sorted alphabetically
        /// </summary>
        public static async Task<List<Person>> TryGetPersonListSortedAZ(IJSRuntime _jsRuntime)
        {
            var unsorted = await AppData.TryGetPersonList(_jsRuntime);
            var sortedList = unsorted.OrderBy(person => person.Name).ToList();
            return sortedList;

        }
        public static async Task<List<Person>> TryGetPersonList(IJSRuntime _jsRuntime)
        {
            await ServerManager.IfBusyPleaseHold("TryGetPersonList");

            //check if people list already loaded before
            if (AppData.PersonList == null)
            {
                Console.WriteLine("BLZ:Get Fresh PersonList");
                AppData.PersonList = await WebsiteTools.GetPeopleList(_jsRuntime);
            }
            else
            {
                Console.WriteLine("BLZ:Using PersonList Cache");
            }

            return AppData.PersonList;
        }

        public static void ClearPersonList() => AppData.PersonList = null;

    }
}