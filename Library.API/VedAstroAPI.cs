


using System.Text.RegularExpressions;
using System.Xml.Linq;
using Microsoft.JSInterop;
using Microsoft.VisualBasic;
using Newtonsoft.Json.Linq;
using VedAstro.Library;


namespace Library.API
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

        public async Task<List<T>> GetList<T>(string inputUrl, Func<JToken, List<T>> converter)
        {

            //call until data appears, API takes care of everything
            JToken? personListJson = null;
            var pollRate = 1500;
            var notReady = true;
            while (notReady)
            {
                await Task.Delay(pollRate);
                personListJson = await Tools.ReadOnlyIfPassJSJson(inputUrl, JsRuntime);
                notReady = personListJson == null;
            }

            //var cachedPersonList = Person.FromJsonList(personListJson);
            var cachedPersonList = converter.Invoke(personListJson);

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
                parsedJsonReply = await Tools.ReadOnlyIfPassJSString(inputUrl, dataToSend, JsRuntime);
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

    }
}