


using System.Xml.Linq;
using Microsoft.JSInterop;
using Newtonsoft.Json.Linq;
using VedAstro.Library;

namespace Library.API
{

    /// <summary>
    /// Make easy access to API, makes API easily consumable
    /// </summary>
    public class VedAstroAPI
    {
        private readonly string _userId;
        private readonly string _visitorId;
        private readonly IJSRuntime _jsRuntime;

        private List<Person> CachedPersonList { get; set; } = new List<Person>(); //if empty que to get new list

        /// <summary>
        /// public examples profiles always here if needed, list should never be empty, bad UX
        /// </summary>
        private List<Person> CachedPublicPersonList { get; set; } = new List<Person>(); //if empty que to get new list

        private readonly URL _url;


        //--------CTOR
        public VedAstroAPI(string userId, string visitorId, IJSRuntime jsRuntime, URL url)
        {
            _userId = userId;
            _visitorId = visitorId;
            _jsRuntime = jsRuntime;
            _url = url;
        }


        //--------------------------------------------------

        /// <summary>
        /// getting people list is a long process, because of clean up and stuff
        /// so ask server to start prepare, will get results later when needed
        /// </summary>
        public void PreparePersonList()
        {
            //send the calls end of story, dont expect to check on it until needed let server handle it

            //STAGE 1 :get person list for current user, can be empty
            //tell API to get started
            var url = $"{_url.GetPersonList}/UserId/{_userId}/VisitorId/{_visitorId}";

            //no wait for speed
            //we get call status and id to get data from when ready
            Tools.ReadServer<JObject>(url);


            //STAGE 2 : get person list for public, example profiles
            //tell API to get started
            url = $"{_url.GetPersonList}/UserId/101/VisitorId/101";

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
            var url = $"{_url.GetPersonList}/UserId/{_userId}/VisitorId/{_visitorId}";
            CachedPersonList = await GetPersonListBehind(url);

            return CachedPersonList;
        }

        public async Task<List<Person>> GetPublicPersonList()
        {
            //CHECK CACHE
            //cache will be cleared when update is needed
            if (CachedPublicPersonList.Any()) { return CachedPublicPersonList; }

            //tell API to get started
            var url2 = $"{_url.GetPersonList}/UserId/101/VisitorId/101";
            CachedPublicPersonList = await GetPersonListBehind(url2);

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
            var url = $"{_url.AddPerson}/UserId/{_userId}/VisitorId/{_visitorId}";
            var jsonResult = await Tools.WriteServer(HttpMethod.Post, url, personJson);

#if DEBUG
            Console.WriteLine($"SERVER SAID:\n{jsonResult}");
#endif

            //if pass, clear local person cache
            await HandleResultClearLocalCache(jsonResult);

            //up to caller to interpret data, can be failed one also
            return jsonResult;
        }

        /// <summary>
        /// Deletes person from API server  main list
        /// note:
        /// - if fail will show alert message
        /// - cached person list is cleared here
        /// </summary>
        public async Task DeletePerson(string personId)
        {
            //tell API to get started
            //pass in user id to make sure user has right to delete
            var url = $"{_url.DeletePerson}/UserId/{_userId}/VisitorId/{_visitorId}/PersonId/{personId}";

            //API gives a url to check on poll fo results
            var jsonResult = await Tools.WriteServer(HttpMethod.Get, url);

#if DEBUG
            Console.WriteLine($"SERVER SAID:\n{jsonResult}");
#endif

            //if pass, clear local person cache
            await HandleResultClearLocalCache(jsonResult);

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
            var url = $"{_url.UpdatePerson}/UserId/{_userId}/VisitorId/{_visitorId}";
            var jsonResult = await Tools.WriteServer(HttpMethod.Post, url, updatedPerson);



#if DEBUG
            Console.WriteLine($"SERVER SAID:\n{jsonResult}");
#endif

            //if pass, clear local person cache
            await HandleResultClearLocalCache(jsonResult);

        }

        /// <summary>
        /// Shows alert using sweet alert js
        /// will show okay button, no timeout
        /// </summary>
        private  async Task ShowAlert(string icon, string title, string descriptionText)
        {
            //call SweetAlert lib directly via constructor
            const string Swal_fire = "Swal.fire";
            await _jsRuntime.InvokeVoidAsync(Swal_fire, title, descriptionText, icon);
        }


        /// <summary>
        /// used to get person direct not in users list for easy sharing
        /// </summary>
        public async Task<Person> GetPerson(string personId)
        {
            var url = $"{_url.GetPerson}/PersonId/{personId}";
            var result = await Tools.ReadServer<JObject>(url);

            //get parsed payload from raw result
            var person = GetPayload(result, Person.FromJson);

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
            var url = $"{_url.GetNewPersonId}/Name/{personName}/BirthYear/{stdBirthYear}";
            var jsonResult = await Tools.WriteServer(HttpMethod.Get, url);

            //get parsed payload from raw result
            string personId = GetPayload<string>(jsonResult, null);

            return personId;

        }



        //---------------------------------------------PRIVATE
        private async Task<List<Person>> GetPersonListBehind(string inputUrl)
        {

            //call until data appears, API takes care of everything
            JToken? personListJson = null;
            var pollRate = 250;
            var notReady = true;
            while (notReady)
            {
                await Task.Delay(pollRate);
                personListJson = await Tools.ReadOnlyIfPassJSJson(inputUrl, _jsRuntime);
                notReady = personListJson == null;
            }

            var cachedPersonList = Person.FromJsonList(personListJson);

            return cachedPersonList;

        }

        /// <summary>
        /// Calls API till surrender
        /// </summary>
        private async Task<string?> PollApiTillData(string inputUrl, string dataToSend)
        {

            //call until data appears, API takes care of everything
            string? parsedJsonReply = null;
            var pollRate = 300;
            var notReady = true;
            while (notReady)
            {
                await Task.Delay(pollRate);
                parsedJsonReply = await Tools.ReadOnlyIfPassJSString(inputUrl, dataToSend, _jsRuntime);
                notReady = parsedJsonReply == null; //if null no data, continue wait
            }

            return parsedJsonReply;

        }


        /// <summary>
        /// checks status, if pass clears person list cache, for update, delete and add
        /// </summary>
        private async Task HandleResultClearLocalCache(JToken jsonResult)
        {

            //if anything but pass, raise alarm
            var status = jsonResult["Status"]?.Value<string>() ?? "";
            if (status != "Pass") //FAIL
            {
                var failMessage = jsonResult["Payload"]?.Value<string>() ?? "";
                await ShowAlert("error", $"Server is not happy! Why?", failMessage);
            }
            else //PASS
            {
                //1: clear stored person list
                this.CachedPersonList.Clear();
            }
        }


        /// <summary>
        /// takes in raw response from API and
        /// gets payload after checking status and shows error if status "Fail"
        /// note :  no parser use direct, support for string, int and double
        /// </summary>
        private T GetPayload<T>(JToken rawResult, Func<JToken, T>? parser)
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


        public async Task<string> GetEventsChart(Person person, TimeRange timeRange, List<EventTag> inputedEventTags)
        {

            if (Person.Empty.Equals(person))
            {
                throw new InvalidOperationException("NO CHART FOR EMPTY PERSON!");
            }


            //1 : package data to get chart
            var chartSpecsJson = EventsChart.GenerateChartSpecsJson(person, timeRange, inputedEventTags);

            //ask API to make new chart
            var eventsChartApiCallUrl = $"{_url.GetEventsChart}/UserId/{_userId}/VisitorId/{_visitorId}";

            //NOTE:call is held here
            var chartString = await PollApiTillData(eventsChartApiCallUrl, chartSpecsJson.ToString());

            return chartString;
        }
    }
}