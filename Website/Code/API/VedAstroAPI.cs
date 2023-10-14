


using System.Text.RegularExpressions;
using System.Xml.Linq;
using Microsoft.JSInterop;
using Microsoft.VisualBasic;
using Newtonsoft.Json.Linq;
using VedAstro.Library;


namespace Website
{
    /// <summary>
    /// Make easy access to API, makes API easily consumable
    /// </summary>
    public class VedAstroAPI
    {

        public MatchTools Match;
        public PersonTools Person;
        public EventsChartTools EventsChart;
        public MLTableTools MLTable;

        public readonly string UserId;
        public readonly string VisitorID;
        public readonly IJSRuntime JsRuntime;
        public readonly URL URL;



        //--------CTOR
        public VedAstroAPI(string userId, string visitorId, IJSRuntime jsRuntime, URL url)
        {
            UserId = userId;
            VisitorID = visitorId;
            JsRuntime = jsRuntime;
            URL = url;
            Match = new MatchTools(this);
            Person = new PersonTools(this);
            EventsChart = new EventsChartTools(this);
            MLTable = new MLTableTools(this);
        }


        //--------------------------------------------------

        /// <summary>
        /// Polls the URL, gets the data and auto converts it
        /// </summary>
        public async Task<List<T>> GetList<T>(string inputUrl, Func<JToken, List<T>> converter)
        {

            //call until data appears, API takes care of everything
            JToken? xListJson = null;
            var pollRate = 2000;
            var notReady = true;
            while (notReady)
            {
                await Task.Delay(pollRate);
                xListJson = await WebsiteTools.ReadOnlyIfPassJSJson(inputUrl, JsRuntime);
                notReady = xListJson == null;
            }

            //var cachedPersonList = Person.FromJsonList(personListJson);
            var cachedPersonList = converter.Invoke(xListJson);

            return cachedPersonList;

        }


        /// <summary>
        /// Makes direct call to API and waits tills she replies, could fail if call wait time is long
        ///  header with value pass is not checked for
        /// </summary>
        public async Task<List<T>> GetListNoPolling<T>(string inputUrl, byte[] byteData, Func<JToken, List<T>> converter)
        {

            //call until data appears, API takes care of everything
            JToken? xListJson = null;

            xListJson = await Tools.WriteServer<JObject, byte[]>(HttpMethod.Post, inputUrl, byteData);

            //var cachedPersonList = Person.FromJsonList(personListJson);
            var timeListJson = xListJson["Payload"];
            var cachedPersonList = converter.Invoke(timeListJson);

            return cachedPersonList;

        }
       
        /// <summary>
        /// Shows alert using sweet alert js
        /// </summary>
        /// <param name="timer">milliseconds to auto close alert, if 0 then won't close which is default (optional)</param>
        /// <param name="useHtml">If true title can be HTML, default is false (optional)</param>
        public async Task ShowAlert(string icon, string title, bool showConfirmButton, int timer = 0, bool useHtml = false)
        {
            object alertData;

            if (useHtml)
            {
                alertData = new
                {
                    icon = icon,
                    html = title,
                    showConfirmButton = showConfirmButton,
                    timer = timer
                };
            }
            else
            {
                alertData = new
                {
                    icon = icon,
                    title = title,
                    showConfirmButton = showConfirmButton,
                    timer = timer
                };
            }


            await ShowAlert(alertData);
        }

        /// <summary>
        /// Shows alerts on page using SweetAlert js lib 
        /// this call is equivalent to
        /// Note: create alter data as anonymous type exactly like js version
        /// 
        /// Swal.fire({
        /// title: 'Error!',
        /// text: 'Do you want to continue',
        /// icon: 'error',
        /// confirmButtonText: 'Cool'
        /// })
        /// 
        /// </summary>
        public async Task ShowAlert(object alertData)
        {
            try
            {
                await JsRuntime.InvokeVoidAsync(Swal_fire, alertData);
            }
            //above code will fail when called during app start, because haven't load lib
            //as such catch failure and silently ignore
            catch (Exception)
            {
                Console.WriteLine($"BLZ: ShowAlert Not Yet Load Lib Silent Fail!");
            }

        }

        const string Swal_fire = "Swal.fire";

        /// <summary>
        /// Shows alert using sweet alert js
        /// will show okay button, no timeout
        /// </summary>
        public async Task ShowAlert(string icon, string title, string descriptionText)
        {
            //call SweetAlert lib directly via constructor
            await JsRuntime.InvokeVoidAsync(Swal_fire, title, descriptionText, icon);
        }

        /// <summary>
        /// Calls API till surrender
        /// Defaults to GET request when payload is null
        /// POST/GET request with URL and payload needed
        /// </summary>
        public async Task<string?> PollApiTillData(string inputUrl, string dataToSend = null)
        {

            //call until data appears, API takes care of everything
            string? parsedJsonReply = null;
            var pollRate = 5000;
            var notReady = true;
            while (notReady)
            {
                await Task.Delay(pollRate);
                parsedJsonReply = await WebsiteTools.ReadOnlyIfPassJSString(inputUrl, dataToSend, JsRuntime);
                notReady = parsedJsonReply == null; //if null no data, continue wait
            }

            return parsedJsonReply;

        }

        /// <summary>
        /// takes in raw response from API and
        /// gets payload after checking status and shows error if status "Fail"
        /// note :  no parser use direct, support for string, int and double
        /// </summary>
        public static T GetPayload<T>(JToken rawResult, Func<JToken, T>? parser)
        {
            //result must say Pass, else it has failed
            var isPass = rawResult["Status"]?.Value<string>() == "Pass";
            var payloadJson = rawResult["Payload"] ?? new JObject();

            if (isPass)
            {
#if DEBUG
                Console.WriteLine("API SAID: PASS"); //debug to know all went well
#endif

                //use parser if available, use that, end here
                if (parser != null)
                {
                    var personJson = parser(payloadJson);
                    return personJson;
                }

                //if no parser use direct, support for string, int and double
                return payloadJson.Value<T>();

            }
            else
            {
#if DEBUG
                Console.WriteLine($"API SAID : FAIL :\n{payloadJson}");
#endif
                //for now this should notify errors nicely, todo maybe exceptions is not best 
                throw new Exception($"Failed to get {typeof(T).AssemblyQualifiedName} from API payload");
            }

        }

        /// <summary>
        /// HTTP Post via JS interop
        /// </summary>
        public static async Task<WebResult<JToken>> WriteToServerJsonReply(string apiUrl, string stringData, int timeout = 60)
        {

            TryAgain:

            //ACT 1:
            //send data to URL, using JS for reliability & speed
            //also if call does not respond in time, we replay the call over & over
            string receivedData;
            try { receivedData = await Tools.TaskWithTimeoutAndException(ServerManager.Post(apiUrl, stringData), TimeSpan.FromSeconds(timeout)); }

            //if fail replay and log it
            catch (Exception e)
            {
                var debugInfo = $"Call to \"{apiUrl}\" timeout at : {timeout}s";

                WebLogger.Data(debugInfo);
#if DEBUG
                Console.WriteLine(debugInfo);
#endif
                goto TryAgain;
            }

            //ACT 2:
            //check raw data 
            if (string.IsNullOrEmpty(receivedData))
            {
                //log it
                await WebLogger.Error($"BLZ > Call returned empty\n To:{apiUrl} with payload:\n{stringData}");

                //send failed empty data to caller, it should know what to do with it
                return new WebResult<JToken>(false, new JObject("CallEmptyError"));
            }

            //ACT 3:
            //return data as Json
            var writeToServerXmlReply = JObject.Parse(receivedData);
            var returnVal = WebResult<XElement>.FromJson(writeToServerXmlReply);

            //ACT 4:
            return returnVal;
        }

    }
}