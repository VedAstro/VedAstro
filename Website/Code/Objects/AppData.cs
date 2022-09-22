using System.Xml.Linq;
using Genso.Astrology.Library;
using Microsoft.AspNetCore.Components;
using Website.Pages;

namespace Website
{
    /// <summary>
    /// Static class to hold global site data
    /// </summary>
    public static class AppData
    {

        public static int FailureCount = 0;

        /// <summary>
        /// Shortcut text to show credit
        /// </summary>
        public static MarkupString HinduPredictiveAstrologyCredit = new("<p class=\"fst-italic fw-light\"><small>(Source Credit: Hindu Predictive Astrology by B.V.Raman)</small></p>");
        public static MarkupString MuhurthaCredit = new("<p class=\"fst-italic fw-light\"><small>(Source Credit: Muhurtha by B.V.Raman)</small></p>");


        /// <summary>
        /// Represents the currently logged in User
        /// If null then nobody logged in
        /// </summary>
        public static UserData? CurrentUser { get; set; } = UserData.Empty;

        /// <summary>
        /// A copy of this visitor's ID, set when logging new visit
        /// Different from User Id in current user
        /// Used to log user flow
        /// Note: defaults to empty string
        /// </summary>
        public static string? VisitorId { get; set; } = "";

        /// <summary>
        /// Url of current page, set by layout when loading
        /// </summary>
        public static string? CurrentPage { get; set; }

        /// <summary>
        /// Place where global event data list is stored for quick access
        /// loaded in main layout
        /// </summary>
        public static List<XElement>? PredictionDataList { get; set; }
        public static Stream? PredictionDataListStream { get; set; }

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
        public static List<Person>? PersonList { get; set; }


        //-----------------------------FIELDS


        /// <summary>
        /// Hard coded max width used in pages 
        /// </summary>
        public const string MaxWidth = "693px";
        public const string MaxContentWidthPx = "443px";


        /// <summary>
        /// if data already loaded then return the that one,
        /// else get a new one from server
        /// </summary>
        public static async Task<Stream?> GetPredictionDataStreamCached()
        {
            //return already loaded if available
            if (AppData.PredictionDataListStream != null) return AppData.PredictionDataListStream;
            
            //else get fresh copy from server
            var client = new HttpClient();
            client.BaseAddress = AppData.BaseAddres;
            AppData.PredictionDataListStream = await client.GetStreamAsync("data/PredictionDataList.xml");
            return AppData.PredictionDataListStream;
        }

        /// <summary>
        /// Base address currently used by App,
        /// could be http://localhost or https://vedastro.org
        /// </summary>
        public static Uri? BaseAddres;

        public static async Task<List<XElement>?> GetPredictionDataListCached()
        {
            //return already loaded if available
            if (AppData.PredictionDataList != null) return AppData.PredictionDataList;

            //else get fresh copy from server
            AppData.PredictionDataList = await WebsiteTools.GetXmlFile("data/PredictionDataList.xml");
            return AppData.PredictionDataList;

        }


        /// <summary>
        /// If true new Visitor is first visit (theoretically)
        /// </summary>
        public static bool IsNewVisitor { get; set; } = false;

        /// <summary>
        /// Origin URL set by MainLayout
        /// </summary>
        public static string OriginUrl { get; set; }
    }
}