using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Genso.Astrology.Library;

public class URL
{

    /// <summary>
    /// I know its tedious to make it a instance but shows that URLS are
    /// specific to instance and the use of them should be aware of that
    /// sets if in beta or stable URL
    /// All API functions can be accessed by this .org URL
    /// Note: possible via azure CDN rules engine : AccessApiViaWebDomain
    /// </summary>
    public URL(bool isBetaRuntime)
    {
        //keep inside
        const string apiBeta = "https://beta.vedastro.org/api";
        const string apiStable = "https://vedastro.org/api";
        const string webBeta = "https://beta.vedastro.org";
        const string webStable = "https://vedastro.org";

        ApiUrl = isBetaRuntime ? apiBeta : apiStable;
        WebUrl = isBetaRuntime ? webBeta : webStable;

        //done here so that can be readonly
        AddPersonApi = ApiUrl + "/addperson";
        AddUserIdToVisitorPersons = ApiUrl + "/AddUserIdToVisitorPersons";
        GetHoroscope = ApiUrl + "/gethoroscope";
        AddLifeEventApi = ApiUrl + "/addlifeevent";
        AddMessageApi = ApiUrl + "/addmessage";
        DeletePerson = ApiUrl + "/DeletePerson";
        DeleteChartApi = ApiUrl + "/deletesavedchart";
        DeleteVisitorByUserId = ApiUrl + "/deletevisitorbyuserid";
        DeleteVisitorByVisitorId = ApiUrl + "/deletevisitorbyvisitorid";
        AddTaskApi = ApiUrl + "/addtask";
        AddVisitorApi = ApiUrl + "/addvisitor";
        GetPersonList = ApiUrl + "/GetPersonList";
        GetPersonApi = ApiUrl + "/getperson";
        GetPersonIdFromSavedChartId = ApiUrl + "/getpersonidfromsavedchartid";
        UpdatePersonApi = ApiUrl + "/updateperson";
        GetTaskListApi = ApiUrl + "/gettasklist";
        GetVisitorList = ApiUrl + "/getvisitorlist";
        GetMessageList = ApiUrl + "/getmessagelist";
        GetMatchReportApi = ApiUrl + "/getmatchreport";
        GetEventsChart = ApiUrl + "/geteventschart";
        GetSavedEventsChart = ApiUrl + "/getsavedeventschart";
        GetSavedEventsChartIdList = ApiUrl + "/getsavedchartnamelist";
        SaveEventsChart = ApiUrl + "/SaveEventsChart";
        GetEventsApi = ApiUrl + "/getevents";
        SignInGoogle = ApiUrl + "/SignInGoogle";
        SignInFacebook = ApiUrl + "/SignInFacebook";

        //let dev user know
        Console.WriteLine("Dynamic URL:" + (isBetaRuntime ? "Beta" : "Stable"));
    }

    /// <summary>
    /// Gets the file contents of branch-manifest.txt to know which build this is beta or stable
    /// gotten once when app is loading API & Blazor, and save it for use in the instance
    /// </summary>
    public static async Task<URL> CreateInstance(HttpClient client)
    {
        var isBetaRuntime = await Tools.GetIsBetaRuntime(client);
        return new URL(isBetaRuntime);
    }


    /// <summary>
    /// Auto set beta or stable based on branch-manifest.txt
    /// </summary>
    public readonly string ApiUrl;
    public readonly string WebUrl;
    public readonly string AddPersonApi;
    public readonly string AddUserIdToVisitorPersons;
    public readonly string GetHoroscope;
    public readonly string AddLifeEventApi;
    public readonly string AddMessageApi;
    public readonly string DeletePerson;
    public readonly string DeleteChartApi;
    public readonly string DeleteVisitorByUserId;
    public readonly string DeleteVisitorByVisitorId;
    public readonly string AddTaskApi;
    public readonly string AddVisitorApi;
    public static readonly string AddVisitorApi_Stable = "https://vedastro.org/api/addvisitor";

    public readonly string GetPersonList;
    public readonly string GetPersonApi;
    public readonly string GetPersonIdFromSavedChartId;

    public readonly string UpdatePersonApi;
    public readonly string GetTaskListApi;
    public readonly string GetVisitorList;
    public readonly string GetMessageList;
    public readonly string GetMatchReportApi;
    public readonly string GetEventsChart;
    public readonly string GetSavedEventsChart;
    public readonly string GetSavedEventsChartIdList;
    public readonly string SaveEventsChart;
    public readonly string GetEventsApi;
    public readonly string SignInGoogle;
    public readonly string SignInFacebook;

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