


//extra logic because URL is 
export class URLS {

    static ApiBeta = "https://beta.api.vedastro.org";
    static ApiBetaDirect = "https://vedastroapibeta.azurewebsites.net";
    static ApiStable = "https://api.vedastro.org";
    static ApiStableDirect = "https://vedastroapi.azurewebsites.net";
    static WebBeta = "https://beta.vedastro.org";
    static WebStable = "https://vedastro.org";

    constructor() {

        //check URL address if beta or stable to set correct mode
        var domain = window.location.hostname;
        var isBeta = domain.includes("beta") || domain.includes("localhost"); //when in local

        //let dev know in beta or stable mode
        console.log(`URL MODE -> ${isBeta ? "BETA" : "STABLE"}`);

        //set root domain based on if in beta or stable
        this.APIDomainDirect = isBeta ? URLS.ApiBetaDirect : URLS.ApiStableDirect;
        this.APIDomain = isBeta ? URLS.ApiBeta : URLS.ApiStable;
        this.WebDomain = isBeta ? URLS.WebBeta : URLS.WebStable;

        //set every url is needed
        this.EventDataListXml = `${this.WebDomain}/data/EventDataList.xml`;
        this.ConsoleGreetingTxt = `${this.WebDomain}/data/ConsoleGreeting.txt`;
        this.GetEventsChartEasyDirect = `${this.APIDomainDirect}/api/geteventscharteasy`; //special URL for chart because Azure CDN timeout limit >30s
        this.GetEventsChartDirect = `${this.APIDomainDirect}/api/geteventschart`; //special URL for chart because Azure CDN timeout limit >30s

        //make available to other in runtime (app.js & eventschart.js,...)
    }
}