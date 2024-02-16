using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;
using VedAstro.Library;
using Website.Pages;

namespace Website
{

    /// <summary>
    /// Static class to hold global site data
    /// </summary>
    public static class AppData
    {
        //-----------------------------FIELDS

        public static List<SimpleAyanamsa> SimpleAyanamsaAll = new()
        {
            SimpleAyanamsa.LahiriChitrapaksha,
            SimpleAyanamsa.KrishnamurtiKP,
            SimpleAyanamsa.Raman,
            SimpleAyanamsa.FaganBradley,
            SimpleAyanamsa.J2000,
            SimpleAyanamsa.Yukteshwar,

        };

        public static List<Ayanamsa> AdvancedAyanamsaAll = new()
        {

            Ayanamsa.FAGAN_BRADLEY,

            Ayanamsa.LAHIRI,

            Ayanamsa.DELUCE,

            Ayanamsa.RAMAN,

            Ayanamsa.USHASHASHI,

            Ayanamsa.KRISHNAMURTI,

            Ayanamsa.DJWHAL_KHUL,

            Ayanamsa.YUKTESHWAR,

            Ayanamsa.JN_BHASIN,

            Ayanamsa.BABYL_KUGLER1,

            Ayanamsa.BABYL_KUGLER2,

            Ayanamsa.BABYL_KUGLER3,

            Ayanamsa.BABYL_HUBER,

            Ayanamsa.BABYL_ETPSC,

            Ayanamsa.ALDEBARAN_15TAU,

            Ayanamsa.HIPPARCHOS,

            Ayanamsa.SASSANIAN,

            Ayanamsa.GALCENT_0SAG,

            Ayanamsa.J1900,

            Ayanamsa.B1950,

            Ayanamsa.SURYASIDDHANTA,

            Ayanamsa.SURYASIDDHANTA_MSUN,

            Ayanamsa.ARYABHATA,

            Ayanamsa.ARYABHATA_MSUN,

            Ayanamsa.SS_REVATI ,

            Ayanamsa.SS_CITRA,

            Ayanamsa.TRUE_CITRA,

            Ayanamsa.TRUE_REVATI,

            Ayanamsa.TRUE_PUSHYA,

            Ayanamsa.GALCENT_RGBRAND ,

            Ayanamsa.GALEQU_IAU1958,

            Ayanamsa.GALEQU_TRUE,

            Ayanamsa.GALEQU_MULA,
            Ayanamsa.GALALIGN_MARDYKS,

            Ayanamsa.TRUE_MULA ,

            Ayanamsa.GALCENT_MULA_WILHELM,

            Ayanamsa.ARYABHATA_522,

            Ayanamsa.BABYL_BRITTON,

            Ayanamsa.TRUE_SHEORAN,

            Ayanamsa.GALCENT_COCHRANE,

            Ayanamsa.GALEQU_FIORENZA,

            Ayanamsa.VALENS_MOON,

            Ayanamsa.LAHIRI_1940,

            Ayanamsa.LAHIRI_VP285,

            Ayanamsa.KRISHNAMURTI_VP291,

            Ayanamsa.LAHIRI_ICRC,
        };


        public static SearchResult SearchPage { get; set; }

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


        public static Stream? HoroscopeDataListStream { get; set; }


        /// <summary>
        /// global ReferenceList.xml data list is stored for quick access
        /// loaded in main layout
        /// </summary>
        public static List<XElement> ReferenceList { get; set; }

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
        public static NavigationManager Navigation
        {
            get { return _navigation; }
            set
            {
                Console.WriteLine("BLZ:Global Navigation Manager Initialized");
                _navigation = value;
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
            builder.AddMarkupContent(0, $"<img style=\"position: relative; left: 39%; top: 30%; cursor: progress;\" src=\"images/loading-animation-progress-transparent.gif\" />");
        };

        /// <summary>
        /// filled when loading app via Google API IP location detect
        /// used as backup location for selector
        /// </summary>
        public static GeoLocation ClientLocation { get; set; }

        /// <summary>
        /// starts false, set when search is used for the 1st time
        /// </summary>
        public static bool SearchFilesLoaded { get; set; }


        /// <summary>
        /// standardized grey used in small text 
        /// </summary>
        public const string Grey = "#8f8f8f"; //#969696

        public const string DefaultLocationCountry = "Singapore";

        public const string TitleFont = "Lexend Deca";

        public const string DescriptionFont = "font-family: 'Gowun Dodum', serif;";

        public const string ButtonFont = "Varta";
        public const string CursiveFont = "Homemade Apple";


        /// <summary>
        /// Base address currently used by App,
        /// could be http://localhost / www.vedastro.org / vedastro.org / beta.vedastro.org
        /// </summary>
        public static Uri? BaseAddress;

        private static IJSRuntime _jsRuntime;
        private static NavigationManager _navigation;

        /// <summary>
        /// Counts the number of times the stamp was clicked
        /// </summary>
        public static int StampClickCount;

        /// <summary>
        /// manager to access everything API
        /// </summary>
        public static VedAstroAPI API;


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
        /// Simple blazor navigation wrapper with standard logging
        /// if going to login page will auto set come back url
        /// receiving page must then choose to use come back url
        /// </summary>
        public static void Go(string url, bool forceReload = false, bool newTab = false, bool rememberMe = false)
        {
            //WebLogger.Data($"NAVIGATE -> {url}"); //log

            //if going to login page then obviously enable auto comeback
            //NOTE: this only sets data into browser storage, page as to start comeback
            //saved in browser, so doesn't get deleted by refresh
            if (url == PageRoute.Login || rememberMe)
            {
                var comebackUrl = _navigation.Uri; //current page url
                _jsRuntime.SetProperty("PreviousPage", comebackUrl);
            }


            //same tab navigation
            if (!newTab)
            {
                Navigation.NavigateTo(url, forceReload);
            }
            //new tab navigation
            else
            {
                JsRuntime.OpenNewTab(url);
            }

        }



        public static async Task LoadSearchFiles()
        {
            //this data is used later search for fast loading
            AppData.ReferenceList = await Tools.GetXmlFile("data/ReferenceList.xml", AppData.HttpClient);

            //mark as loaded so on next search won't reload
            AppData.SearchFilesLoaded = true;

        }

        /// <summary>
        /// used for quick search, no need to load all
        /// </summary>
        /// <returns></returns>
        public static async Task LoadReferenceSearchFiles()
        {
            //this data is used later search for fast loading
            AppData.ReferenceList = await Tools.GetXmlFile("data/ReferenceList.xml");

            //mark as loaded so on next search won't reload
            AppData.ReferenceSearchFilesLoaded = true;

        }

        public static bool ReferenceSearchFilesLoaded { get; set; }


        /// <summary>
        /// HTML reference to desktop sidebar div, used to show and hide by button
        /// </summary>
        public static ElementReference DesktopSidebar { get; set; }


        /// <summary>
        /// gets cached person list stored in browser memory
        /// </summary>
        public static async Task<List<Person>> GetCachedPersonList(bool isPublic = false)
        {
            //get raw cache if any
            //set either public or private list
            var propName = isPublic ? "CachedPublicPersonList" : "CachedPersonList";

            var listJson = await _jsRuntime.GetProperty(propName);

            if (listJson is not null or "")
            {
#if DEBUG
                Console.WriteLine("BLZ: Using cached person list");
#endif
                //convert string to parsed list
                var parsedList = Person.FromJsonList(JToken.Parse(listJson));

                return parsedList;
            }

            //no data return empty to be detected
            return new List<Person>();
        }

        //private static List<EventData> CachedEventDataList { get; set; } = new(); //if empty que to get new list

        //        public static async Task<List<EventData>> GetCachedEventDataList()
        //        {
        //            //check cache is expired
        //            var creationDate = await _jsRuntime.GetProperty("CachedEventDataList_SetDate");
        //            creationDate = string.IsNullOrEmpty(creationDate) ? DateTime.Now.ToString() : creationDate;
        //            var timeSinceCreation = DateTime.Now - DateTime.Parse(creationDate);
        //            var isExpired = timeSinceCreation.TotalDays > 7; //1 week expired

        //            //get raw cache if any
        //            var listJson = await _jsRuntime.GetProperty("CachedEventDataList");
        //            var cacheExist = listJson is not null or "";

        //            //use cache exist if not expired, less the 1 week
        //            if (cacheExist && !isExpired)
        //            {
        //#if DEBUG
        //                Console.WriteLine("BLZ: Using Cached Event DataList");
        //#endif
        //                //convert string to parsed list
        //                var parsedList = EventData.FromJsonList(JToken.Parse(listJson));

        //                return parsedList;
        //            }

        //            //if expired
        //            //no data return empty to be detected
        //            return new List<EventData>();
        //        }


        public static async Task SetCachedPersonList(List<Person> personList, bool isPublic = false)
        {
            //convert to storable format in browser memory
            JArray personJson = Person.ListToJson(personList);
            var jsonString = personJson.ToString();

            //set either public or private list
            var propName = isPublic ? "CachedPublicPersonList" : "CachedPersonList";
            await _jsRuntime.SetProperty(propName, jsonString);

#if DEBUG
            Console.WriteLine("BLZ: New person cache set in memory");
#endif
        }


        //        /// <summary>
        //        /// also auto sets creation date to check expiry when read
        //        /// </summary>
        //        public static async Task SetCachedEventDataList(List<EventData> eventDataList)
        //        {
        //            //convert to storable format in browser memory
        //            JArray personJson = EventData.ListToJson(eventDataList);
        //            var jsonString = personJson.ToString();

        //            //set either public or private list
        //            await _jsRuntime.SetProperty("CachedEventDataList", jsonString);

        //            //set cache date, as to delete if not needed
        //            await _jsRuntime.SetProperty("CachedEventDataList_SetDate", DateTime.Now.ToString());

        //#if DEBUG
        //            Console.WriteLine("BLZ: New Cached EventData List set in memory");
        //#endif
        //        }

        public static async Task ClearCachedPersonList()
        {
            //clear both saved list in memory
            await _jsRuntime.RemoveProperty("CachedPersonList");
            await _jsRuntime.RemoveProperty("CachedPublicPersonList");

            //also clear last "selected person" (will set later once button saved)
            await _jsRuntime.RemoveProperty("SelectedPerson");


#if DEBUG
            Console.WriteLine("BLZ: Person list cache cleared");
#endif
        }


    }
}