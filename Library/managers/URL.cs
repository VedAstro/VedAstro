using System;

namespace Genso.Astrology.Library;

public static class URL
{


    /// <summary>
    /// sets if in beta or stable URL
    /// All API functions can be accessed by this .org URL
    /// Note: possible via azure CDN rules engine : AccessApiViaWebDomain
    /// </summary>
    static URL()
    {
        //keep inside
        const string apiBeta = "https://beta.vedastro.org/api";
        const string apiStable = "https://vedastro.org/api";
        const string webBeta = "https://beta.vedastro.org";
        const string webStable = "https://vedastro.org";

        ApiUrl = Tools.IsBetaRuntime ? apiBeta : apiStable;
        WebUrl = Tools.IsBetaRuntime ? webBeta : webStable;

        Console.WriteLine(Tools.IsBetaRuntime ? "BETA URL" : "STABLE URLS");
    }

    /// <summary>
    /// Auto set beta or stable based on branch-manifest.txt
    /// </summary>
    public static string ApiUrl;
    public static string WebUrl;
    public static readonly string AddPersonApi = ApiUrl + "/addperson";
    public static readonly string AddUserIdToVisitorPersons = ApiUrl + "/AddUserIdToVisitorPersons";
    public static readonly string GetHoroscope = ApiUrl + "/gethoroscope";
    public static readonly string AddLifeEventApi = ApiUrl + "/addlifeevent";
    public static readonly string AddMessageApi = ApiUrl + "/addmessage";
    public static readonly string DeletePerson = ApiUrl + "/DeletePerson";
    public static readonly string DeleteChartApi = ApiUrl + "/deletesavedchart";
    public static readonly string DeleteVisitorByUserId = ApiUrl + "/deletevisitorbyuserid";
    public static readonly string DeleteVisitorByVisitorId = ApiUrl + "/deletevisitorbyvisitorid";
    public static readonly string AddTaskApi = ApiUrl + "/addtask";
    public static readonly string AddVisitorApi = ApiUrl + "/addvisitor";

    public static readonly string GetPersonList = ApiUrl + "/GetPersonList";
    public static readonly string GetPersonApi = ApiUrl + "/getperson";
    public static readonly string GetPersonIdFromSavedChartId = ApiUrl + "/getpersonidfromsavedchartid";

    public static readonly string UpdatePersonApi = ApiUrl + "/updateperson";
    public static readonly string GetTaskListApi = ApiUrl + "/gettasklist";
    public static readonly string GetVisitorList = ApiUrl + "/getvisitorlist";
    public static readonly string GetMessageList = ApiUrl + "/getmessagelist";
    public static readonly string GetMatchReportApi = ApiUrl + "/getmatchreport";
    public static readonly string GetEventsChart = ApiUrl + "/geteventschart";
    public static readonly string GetSavedEventsChart = ApiUrl + "/getsavedeventschart";
    public static readonly string GetSavedEventsChartIdList = ApiUrl + "/getsavedchartnamelist";
    public static readonly string SaveEventsChart = ApiUrl + "/SaveEventsChart";
    public static readonly string GetEventsApi = ApiUrl + "/getevents";
    public static readonly string SignInGoogle = ApiUrl + "/SignInGoogle";
    public static readonly string SignInFacebook = ApiUrl + "/SignInFacebook";

    /// <summary>
    /// link to js file used for google sign in function
    /// </summary>
    public const string GoogleSignInJs = "https://accounts.google.com/gsi/client";
    public const string GeoJsApiUrl = "https://get.geojs.io/v1/ip/geo.json";
    public const string Paypal = "https://www.paypal.com/sdk/js?client-id=sb&enable-funding=venmo&currency=USD";
    public const string GitHubDiscussions = "https://github.com/orgs/VedAstro/discussions";
    public const string HttpHome = "https://vedastro.org";
    public const string PatreonPage = "https://patreon.com/vedastro";
    public const string KoFiPage = "https://ko-fi.com/vedastro";
    public const string PaypalMePage = "https://paypal.me/VedAstroOrg";
    public const string AddPersonGuideVideo = "https://youtu.be/RDUPsFOrr3c";
    public const string NasaJplSource = "https://naif.jpl.nasa.gov/pipermail/spice_announce/2007-August/000055.html";
    public const string SwissEphSource = "https://www.astro.com/swisseph/swephinfo_e.htm";
}