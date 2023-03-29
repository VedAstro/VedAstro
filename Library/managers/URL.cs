using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace VedAstro.Library;

public class URL
{
    //keep inside
    public const string ApiBeta = "https://beta.api.vedastro.org/";
    public const string ApiStable = "https://api.vedastro.org";
    public const string WebBeta = "https://beta.vedastro.org/";
    public const string WebStable = "https://vedastro.org";

    /// <summary>
    /// I know its tedious to make it a instance but shows that URLS are
    /// specific to instance and the use of them should be aware of that
    /// sets if in beta or stable URL both API & CLIENT consult this singular truth file 
    /// All API functions can be accessed by this .org URL
    /// Note: possible via azure CDN rules engine : AccessApiViaWebDomain
    /// </summary>
    public URL(bool isBetaRuntime)
    {
        ApiUrl = isBetaRuntime ? ApiBeta : ApiStable;
        WebUrl = isBetaRuntime ? WebBeta : WebStable;

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
        //TODO special URL for chart because timeout Azure CDN timeout >30s
        GetEventsChartDirect = "https://vedastroapi.azurewebsites.net/api/geteventschart"; 
        GetSavedEventsChart = ApiUrl + "/getsavedeventschart";
        GetSavedEventsChartIdList = ApiUrl + "/getsavedchartnamelist";
        SaveEventsChart = ApiUrl + "/SaveEventsChart";
        GetEventsApi = ApiUrl + "/getevents";
        SignInGoogle = ApiUrl + "/SignInGoogle";
        SignInFacebook = ApiUrl + "/SignInFacebook";
        HoroscopeDataListXml = $"{WebUrl}/data/HoroscopeDataList.xml";//used in horoscope prediction
        EventsChartViewerHtml = $"{WebUrl}/data/EventsChartViewer.html";
        ToolbarSvgAzure = $"{WebUrl}/svg/Toolbar.svg";// Toolbar.svg used in when rendering events chart
        APIHomePageTxt = $"{WebUrl}/data/APIHomePage.txt";


        //let dev user know
        Console.WriteLine("Dynamic URL:" + (isBetaRuntime ? "Beta" : "Stable"));
    }


    /// <summary>
    /// Auto set beta or stable based on build settings
    /// </summary>
    public readonly string ToolbarSvgAzure;
    public readonly string EventsChartViewerHtml;
    public readonly string HoroscopeDataListXml;
    public readonly string APIHomePageTxt;
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
    public static readonly string AddVisitorApiStable = "https://api.vedastro.org/addvisitor";

    public readonly string GetPersonList;
    public readonly string GetPersonApi;
    public readonly string GetPersonIdFromSavedChartId;

    public readonly string UpdatePersonApi;
    public readonly string GetTaskListApi;
    public readonly string GetVisitorList;
    public readonly string GetMessageList;
    public readonly string GetMatchReportApi;
    public readonly string GetEventsChart;
    public readonly string GetEventsChartDirect;
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
    public const string AzureStorage = "vedastrowebsitestorage.z5.web.core.windows.net";
    public const string WhatsAppContact = "https://wa.me/60142938084?text=Hi";
    public const string EmailToClick = "mailto:contact@vedastro.org";
    public const string GitHubRepo = "https://github.com/VedAstro/VedAstro";
}