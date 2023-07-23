


using System.Text.RegularExpressions;
using System.Xml.Linq;
using Microsoft.JSInterop;
using Microsoft.VisualBasic;
using Newtonsoft.Json.Linq;
using VedAstro.Library;


namespace Library.API
{

    public class MatchTools
    {
        private readonly VedAstroAPI _api;


        public MatchTools(VedAstroAPI vedAstroApi)
        {
            _api = vedAstroApi;
        }

        public async Task<List<PersonKutaScore>> GetList(string personId)
        {
            //CHECK CACHE
            //cache will be cleared when update is needed
            //if (CachedPersonKutaScore.Any()) { return CachedPersonKutaScore; }

            //prepare url to call
            var url = $"{_api.URL.FindMatch}/PersonId/{personId}";
           var  personKutaScore = await _api.GetList(url, PersonKutaScore.FromJsonList);

            return personKutaScore;

        }
    }

    public class PersonTools
    {

        private readonly VedAstroAPI _api;



        private List<Person> CachedPersonList { get; set; } = new List<Person>(); //if empty que to get new list

        /// <summary>
        /// public examples profiles always here if needed, list should never be empty, bad UX
        /// </summary>
        private List<Person> CachedPublicPersonList { get; set; } = new List<Person>(); //if empty que to get new list

        //PUBLIC

        public PersonTools(VedAstroAPI vedAstroApi) => _api = vedAstroApi;

        /// <summary>
        /// getting people list is a long process, because of clean up and stuff
        /// so ask server to start prepare, will get results later when needed
        /// </summary>
        public void PreparePersonList()
        {
            //send the calls end of story, dont expect to check on it until needed let server handle it

            //STAGE 1 :get person list for current user, can be empty
            //tell API to get started
            var url = $"{_api.URL.GetPersonList}/UserId/{_api.UserId}/VisitorId/{_api.VisitorID}";

            //no wait for speed
            //we get call status and id to get data from when ready
            Tools.ReadServer<JObject>(url);


            //STAGE 2 : get person list for public, example profiles
            //tell API to get started
            url = $"{_api.URL.GetPersonList}/UserId/101/VisitorId/101";

            //no wait for speed
            //API gives a url to check on poll fo results
            Tools.ReadServer<JObject>(url);

        }

        /// <summary>
        /// person will be auto prepared, but might be slow
        /// as such prepare before hand if possible, like when app load
        /// </summary>
        public async Task<List<Person>> GetPersonList()
        {
            //CHECK CACHE
            //cache will be cleared when update is needed
            if (CachedPersonList.Any()) { return CachedPersonList; }

            //prepare url to call
            var url = $"{_api.URL.GetPersonList}/UserId/{_api.UserId}/VisitorId/{_api.VisitorID}";
            CachedPersonList = await _api.GetList(url, Person.FromJsonList);

            return CachedPersonList;
        }

        public async Task<List<Person>> GetPublicPersonList()
        {
            //CHECK CACHE
            //cache will be cleared when update is needed
            if (CachedPublicPersonList.Any()) { return CachedPublicPersonList; }

            //tell API to get started
            var url2 = $"{_api.URL.GetPersonList}/UserId/101/VisitorId/101";
            CachedPublicPersonList = await _api.GetList(url2, Person.FromJsonList);

            return CachedPublicPersonList;
        }

        /// <summary>
        /// Adds new person to API server main list
        /// </summary>
        public async Task<JToken> AddPerson(Person person)
        {
            //send newly created person to API server
            var personJson = person.ToJson();
            //pass in user id to make sure user has right to delete
            var url = $"{_api.URL.AddPerson}/UserId/{_api.UserId}/VisitorId/{_api.VisitorID}";
            var jsonResult = await Tools.WriteServer(HttpMethod.Post, url, personJson);

#if DEBUG
            Console.WriteLine($"SERVER SAID:\n{jsonResult}");
#endif

            //if pass, clear local person cache
            await HandleResultClearLocalCache(person,jsonResult, "add");

            //up to caller to interpret data, can be failed one also
            return jsonResult;
        }

        /// <summary>
        /// Deletes person from API server  main list
        /// note:
        /// - takes care of pass and fail messages to end user
        /// - if fail will show alert message
        /// - cached person list is cleared here
        /// </summary>
        public async Task DeletePerson(Person personToDelete)
        {
            //tell API to get started
            //pass in user id to make sure user has right to delete
            var url = $"{_api.URL.DeletePerson}/UserId/{_api.UserId}/VisitorId/{_api.VisitorID}/PersonId/{personToDelete.Id}";

            //API gives a url to check on poll fo results
            var jsonResult = await Tools.WriteServer(HttpMethod.Get, url);

#if DEBUG
            Console.WriteLine($"SERVER SAID:\n{jsonResult}");
#endif

            //if pass, clear local person cache
            await HandleResultClearLocalCache(personToDelete, jsonResult, "delete"); //task is for message box

        }

        /// <summary>
        /// Send updated person to API server to be saved in main list
        /// note:
        /// - if fail will show alert message
        /// - cached person list is cleared here
        /// </summary>
        public async Task UpdatePerson(Person person)
        {
            //todo should check if local copy matches server before updating, cause could overwrite
            //todo detect first using async list if possible to see change from others or use versioning

            //prepare and send updated person to API server
            var updatedPerson = person.ToJson();
            var url = $"{_api.URL.UpdatePerson}/UserId/{_api.UserId}/VisitorId/{_api.VisitorID}";
            var jsonResult = await Tools.WriteServer(HttpMethod.Post, url, updatedPerson);


#if DEBUG
            Console.WriteLine($"SERVER SAID:\n{jsonResult}");
#endif

            //if pass, clear local person cache
            await HandleResultClearLocalCache(person, jsonResult, "update");

        }

        /// <summary>
        /// used to get person direct not in users list for easy sharing
        /// </summary>
        public async Task<Person> GetPerson(string personId)
        {
            var url = $"{_api.URL.GetPerson}/PersonId/{personId}";
            var result = await Tools.ReadServer<JObject>(url);

            //get parsed payload from raw result
            var person = VedAstroAPI.GetPayload(result, Person.FromJson);

            return person;
        }
        /// <summary>
        /// calls API to generate a new person ID, unique and human readable
        /// NOTE:
        /// - API has faster access to person list to cross refer, so done there and not in client
        /// - called before person new person is made on client
        /// </summary>
        public async Task<string> GetNewPersonId(string personName, int stdBirthYear)
        {

            //get all person profile owned by current user/visitor
            var url = $"{_api.URL.GetNewPersonId}/Name/{personName}/BirthYear/{stdBirthYear}";
            var jsonResult = await Tools.WriteServer(HttpMethod.Get, url);

            //get parsed payload from raw result
            string personId = VedAstroAPI.GetPayload<string>(jsonResult, null);

            return personId;

        }


        //PRIVATE




        //---------------------------------------------PRIVATE
        /// <summary>
        /// checks status, if pass clears person list cache, for update, delete and add
        /// </summary>
        private async Task HandleResultClearLocalCache(Person personInQuestion, JToken jsonResult, string task)
        {

            //if anything but pass, raise alarm
            var status = jsonResult["Status"]?.Value<string>() ?? "";
            if (status != "Pass") //FAIL
            {
                var failMessage = jsonResult["Payload"]?.Value<string>() ?? "Server didn't give reason, pls try later.";
                await _api.ShowAlert("error", $"Server said no to your request! Why?", failMessage);
            }
            else //PASS
            {

                //1: clear stored person list
                this.CachedPersonList.Clear();

                //let user know person has been updates
                await _api.ShowAlert("success", $"{personInQuestion.Name} {task} complete!", false, timer: 1000);

            }
        }

    }


    public class EventsChartTools
    {
        private readonly VedAstroAPI _api;

        public EventsChartTools(VedAstroAPI vedAstroApi) => _api = vedAstroApi;


        public async Task<string> GetEventsChart(Person person, TimeRange timeRange, List<EventTag> inputedEventTags, int maxWidth, ChartOptions summaryOptions)
        {
            //no person no entry!
            if (Person.Empty.Equals(person)) { throw new InvalidOperationException("NO CHART FOR EMPTY PERSON!"); }

            //package data to get chart
            var chartSpecsJson = EventsChart.GenerateChartSpecsJson(person, timeRange, inputedEventTags, maxWidth, summaryOptions);

            //ask API to make new chart (user id is for caching)
            var eventsChartApiCallUrl = $"{_api.URL.GetEventsChart}/UserId/{_api.UserId}/VisitorId/{_api.VisitorID}";

            //NOTE:call is held here
            var chartString = await  _api.PollApiTillData(eventsChartApiCallUrl, chartSpecsJson.ToString());

            return chartString;
        }


        /// <summary>
        /// Gets POST call body data that is sent to API, used as a shortcut rather going into network tab in F12 
        /// </summary>
        public string GetPOSTCall(Person person, TimeRange timeRange, List<EventTag> inputedEventTags, int maxWidth,
            ChartOptions summaryOptions)
        {
            //generate the same data used when calling API
            var chartSpecsJson = EventsChart.GenerateChartSpecsJson(person, timeRange, inputedEventTags, maxWidth, summaryOptions);

            var jsonPostData = chartSpecsJson.ToString();

            return jsonPostData;
        }

    }

    /// <summary>
    /// Make easy access to API, makes API easily consumable
    /// </summary>
    public class VedAstroAPI
    {

        public MatchTools Match;
        public PersonTools Person;
        public EventsChartTools EventsChart;

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
        public  async Task ShowAlert( string icon, string title, bool showConfirmButton, int timer = 0, bool useHtml = false)
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
        public async Task ShowAlert( object alertData)
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
        /// </summary>
        public async Task<string?> PollApiTillData(string inputUrl, string dataToSend)
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