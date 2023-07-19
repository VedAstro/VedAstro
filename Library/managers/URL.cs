using System;

namespace VedAstro.Library;

public class URL
{
    //keep inside
    public const string ApiBeta = "https://beta.api.vedastro.org";
    public const string ApiBetaDirect = "https://vedastroapibeta.azurewebsites.net";
    public const string ApiStable = "https://api.vedastro.org";
    public const string ApiStableDirect = "https://vedastroapi.azurewebsites.net";
    public const string WebBeta = "https://beta.vedastro.org";
    public const string WebStable = "https://vedastro.org";

    /// <summary>
    /// I know its tedious to make it a instance but shows that URLS are
    /// specific to instance and the use of them should be aware of that
    /// sets if in beta or stable URL both API & CLIENT consult this singular truth file 
    /// All API functions can be accessed by this .org URL
    /// Note: possible via azure CDN rules engine : AccessApiViaWebDomain
    /// </summary>
    public URL(bool isBetaRuntime, bool debugMode) //don't hide to make obvious easy id
    {
        //set beta or stable based on Runtime Stamp data
        ApiUrl = isBetaRuntime ? ApiBeta : ApiStable;
        ApiUrlDirect = isBetaRuntime ? ApiBetaDirect : ApiStableDirect;
        WebUrl = isBetaRuntime ? WebBeta : WebStable;

        //if DEBUG MODE set all to local (bye bye Postman! we don't need you anymore!)
        if (debugMode)
        {
            ApiUrl = "http://localhost:7071/api";
            ApiUrlDirect = "http://localhost:7071/api";
        }


        //--------------done here so that can be readonly------------------

        //GENERAL
        GetCallData = ApiUrl + "/GetCallData";
        Login = WebUrl + "/Account/Login"; //note must match with page route

        //PERSON
        GetPersonList = ApiUrl + "/GetPersonList";
        AddPerson = ApiUrl + "/AddPerson";
        DeletePerson = ApiUrl + "/DeletePerson";
        UpdatePerson = ApiUrl + "/UpdatePerson";
        GetPerson = ApiUrl + "/getperson";
        GetPersonImage = ApiUrl + "/GetPersonImage/PersonId/";
        GetNewPersonId = ApiUrl + "/GetNewPersonId";


        GetHoroscope = ApiUrl + "/gethoroscope";
        AddLifeEventApi = ApiUrl + "/addlifeevent";
        AddMessageApi = ApiUrl + "/addmessage";
        DeleteChartApi = ApiUrl + "/deletesavedchart";
        DeleteVisitorByUserId = ApiUrl + "/deletevisitorbyuserid";
        DeleteVisitorByVisitorId = ApiUrl + "/deletevisitorbyvisitorid";
        AddTaskApi = ApiUrl + "/addtask";
        AddVisitorApi = ApiUrl + "/addvisitor";
        GetTaskListApi = ApiUrl + "/gettasklist";
        GetVisitorList = ApiUrl + "/getvisitorlist";
        GetMessageList = ApiUrl + "/getmessagelist";

        FindMatch = ApiUrl + "/FindMatch";
        GetMatchReportList = ApiUrl + "/GetMatchReportList";
        GetPersonIdFromSavedChartId = ApiUrl + "/getpersonidfromsavedchartid";
        GetMatchReportApi = ApiUrl + "/getmatchreport";
        GetSavedMatchReport = ApiUrl + "/GetSavedMatchReport";
        SaveMatchReportApi = ApiUrl + "/SaveMatchReport";
        GetEventsChart = ApiUrl + "/GetEventsChart";
        GetCallStatus = ApiUrl + "/GetCallStatus";
        //TODO special URL for chart because timeout Azure CDN timeout >30s
        GetEventsChartDirect = ApiUrlDirect + "/api/geteventschart"; 
        GetSavedEventsChart = ApiUrl + "/getsavedeventschart";
        GetSavedEventsChartIdList = ApiUrl + "/getsavedchartnamelist";
        SaveEventsChart = ApiUrl + "/SaveEventsChart";
        GetEventsApi = ApiUrl + "/getevents";
        SignInGoogle = ApiUrl + "/SignInGoogle";
        SignInFacebook = ApiUrl + "/SignInFacebook"; 
        HoroscopeDataListXml = $"{WebUrl}/data/HoroscopeDataList.xml";//used in horoscope prediction
        EventsChartViewerHtml = $"{WebUrl}/data/EventsChartViewer.html";
        ToolbarSvgAzure = $"{WebUrl}/svg/Toolbar.svg";// Toolbar.svg used in when rendering events chart
        APIHomePageTxt = $"{WebUrl}/data/APIHomePage.html";


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
    public readonly string ApiUrlDirect;
    public readonly string WebUrl;
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

    //MATCH
    public readonly string GetSavedMatchReport;
    public readonly string GetMatchReportApi;
    public readonly string SaveMatchReportApi;
    public readonly string GetMatchReportList;
    public readonly string FindMatch;

    //PERSON
    public readonly string AddPerson;
    /// <summary>
    /// Gets raw data at API data cache
    /// </summary>
    public readonly string GetCallData;
    public readonly string GetPersonList;
    public readonly string GetPerson;
    public readonly string GetPersonImage;
    public readonly string GetNewPersonId;
    public readonly string GetPersonIdFromSavedChartId;
    public readonly string UpdatePerson;

    public readonly string GetTaskListApi;
    public readonly string GetVisitorList;
    public readonly string GetMessageList;
    public readonly string GetEventsChart;
    public readonly string GetCallStatus;
    public readonly string GetEventsChartDirect;
    public readonly string GetSavedEventsChart;
    public readonly string GetSavedEventsChartIdList;
    public readonly string SaveEventsChart;
    public readonly string GetEventsApi;
    public readonly string SignInGoogle;
    public readonly string SignInFacebook;
    public readonly string Login;

    /// <summary>
    /// link to js file used for google sign in function
    /// </summary>
    public const string GoogleSignInJs = "https://accounts.google.com/gsi/client";
    public const string GeoJsApiUrl = "https://get.geojs.io/v1/ip/geo.json";
    public const string TelescopeBuyPage = "https://www.ebay.com/itm/285323405966";
    public const string LaptopBuyPage = "https://www.ebay.com/itm/304878531330";
    public const string Paypal = "https://www.paypal.com/sdk/js?client-id=sb&enable-funding=venmo&currency=USD";
    public const string GitHubDiscussions = "https://github.com/orgs/VedAstro/discussions";
    public const string YoutubeChannel = "https://www.youtube.com/@vedastro";
    public const string FacebookPage = "https://www.facebook.com/vedastro.org";
    public const string Instagram = "https://www.instagram.com/_vedastro/";
    public const string Twitter = "https://twitter.com/_VedAstro";
    public const string GitHub88000Lines = "https://github.com/VedAstro/VedAstro/discussions/4";
    public const string VedAstroJS = "https://github.com/VedAstro/VedAstro.js";
    public const string VedAstroSwift = "https://github.com/VedAstro/VedAstro.Swift";
    public const string VedAstroNuget = "https://www.nuget.org/packages/VedAstro.Library";
    public const string PatreonPage = "https://patreon.com/vedastro";
    public const string KoFiPage = "https://ko-fi.com/vedastro";
    public const string KoFiDonateIframe = "https://ko-fi.com/vedastro/?hidefeed=true&widget=true&embed=true&preview=true";
    public const string PaypalMePage = "https://paypal.me/VedAstroOrg";
    public const string NasaJplSource = "https://naif.jpl.nasa.gov/pipermail/spice_announce/2007-August/000055.html";
    public const string SwissEphSource = "https://www.astro.com/swisseph/swephinfo_e.htm";
    public const string AzureStorage = "vedastrowebsitestorage.z5.web.core.windows.net";
    public const string WhatsAppContact = "https://wa.me/60142938084?text=Hi";
    public const string TelegramContact = "https://t.me/vedastro_org";
    public const string EmailToClick = "mailto:contact@vedastro.org";
    public const string GitHubRepo = "https://github.com/VedAstro/VedAstro";
    public const string GitHubIssues = "https://github.com/VedAstro/VedAstro/issues";
    public const string GitHubCommits = "https://github.com/gen-so/Genso.Astrology/commits/master";
    public const string GitDeveloperRoomProject = "https://github.com/orgs/VedAstro/projects/1";
    public const string SlackInviteURL = "https://join.slack.com/t/vedastro/shared_invite/zt-1u7pdqjky-hrJZ7e3_vM2dZOmVY8FeHA";
    public const string JohnLenonImagine = "https://youtu.be/rAn-AWXtHv0";
    public const string YukteswarWiki = "https://en.wikipedia.org/wiki/Swami_Sri_Yukteswar_Giri";
    public const string WHAudenWiki = "https://en.wikipedia.org/wiki/W._H._Auden";
    public const string SwamiRamaWiki = "https://en.wikipedia.org/wiki/Swami_Rama";
    public const string StarsThatDontGiveDam = "https://youtu.be/RhtrXdirXk4";
    public const string YoganandaWiki = "https://en.wikipedia.org/wiki/Paramahansa_Yogananda";
    public const string CarlSaganWiki = "https://en.wikipedia.org/wiki/Carl_Sagan";
    public const string RamanujanWiki = "https://en.wikipedia.org/wiki/Srinivasa_Ramanujan";
    public const string EmersonWiki = "https://en.wikipedia.org/wiki/Ralph_Waldo_Emerson";
    public const string MahatmaGandhiWiki = "https://en.wikipedia.org/wiki/Mahatma_Gandhi";
    public const string LaSalleWiki = "https://en.wikipedia.org/wiki/Jean-Baptiste_de_La_Salle";
    public const string BVRamanWiki = "https://en.wikipedia.org/wiki/B._V._Raman";
    public const string MiltonFriedmanWiki = "https://en.wikipedia.org/wiki/Milton_Friedman";
    public const string RamanujanQuoteWiki = "https://en.wikipedia.org/wiki/Srinivasa_Ramanujan#Personality_and_spiritual_life";
    public const string StPaulWiki = "https://en.wikipedia.org/wiki/Paul_the_Apostle";
    public const string FreedmanYoutubePencil = "https://youtu.be/67tHtpac5ws";
    public const string APIGuideNextStep = "https://youtu.be/y110RAgIorY?t=127";
}