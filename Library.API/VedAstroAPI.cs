


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

        /// <summary>
        /// Durable function API URL to get ready personlist for this caller
        /// </summary>
        private string PersonListWebhook { get; set; } = "";
        private string PublicPersonListWebhook { get; set; } = "";

        /// <summary>
        /// true of person is already prepared
        /// </summary>
        private bool IsPersonPrepared => !string.IsNullOrEmpty(PersonListWebhook);

        private bool IsPublicPersonPrepared => !string.IsNullOrEmpty(PublicPersonListWebhook);


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
        public async Task PreparePersonList()
        {
            //STAGE 1 :get person list for current user, can be empty
            //tell API to get started
            var url = $"{_url.GetPersonListAsync}/UserId/{_userId}/VisitorId/{_visitorId}";

            //API gives a url to check on poll fo results
            var rawResult = await Tools.ReadServer(url);
            PersonListWebhook = GetPayload<string>(rawResult, null);


            //STAGE 2 : get person list for public, example profiles
            //tell API to get started
            url = $"{_url.GetPersonListAsync}/UserId/101/VisitorId/101";

            //API gives a url to check on poll fo results
            rawResult = await Tools.ReadServer(url);
            PublicPersonListWebhook = GetPayload<string>(rawResult, null);


        }

        /// <summary>
        /// person will be auto prepared, but might be slow
        /// as such prepare before hand if possible, like when app load
        /// </summary>
        public async Task<List<Person>> GetPersonList()
        {
            //take cache if available
            //cache will be cleared when update is needed
            if (CachedPersonList.Any()) { return CachedPersonList; }

            //if person not prepared then prepare before continuing
            if (!IsPersonPrepared) { await PreparePersonList(); }


        //check if person list is ready
        //here result can contain payload if ready
        TryAgain:
            var result = await Tools.ReadServer(PersonListWebhook);
            //todo change to payload standard
            var runtimeStatus = result["runtimeStatus"]?.Value<string>() ?? "";

            //note : if error status, will be corrected by API,
            //so here we wait till complete
            var isDone = runtimeStatus == "Completed";

            //if not yet done, wait little and call back
            if (!isDone)
            {
                //print holding status for easy debug
                Console.WriteLine($"API SAID : {runtimeStatus}");
                await Task.Delay(300);
                goto TryAgain;
            }

            //once done, get data out
            //person list json to parsed
            var outputToken = result["output"]; //json array
            CachedPersonList = Person.FromJsonList(outputToken); //cache for later use

            return CachedPersonList;
        }
        public async Task<List<Person>> GetPublicPersonList()
        {
            //take cache if available
            //cache will be cleared when update is needed
            if (CachedPublicPersonList.Any()) { return CachedPublicPersonList; }

            //if person not prepared then prepare before continuing
            if (!IsPublicPersonPrepared) { await PreparePersonList(); }


        //check if person list is ready
        //here result can contain payload if ready
        TryAgain:
            var result = await Tools.ReadServer(PublicPersonListWebhook);
            //todo change to payload standard
            var runtimeStatus = result["runtimeStatus"]?.Value<string>() ?? "";

            //note : if error status, will be corrected by API,
            //so here we wait till complete
            var isDone = runtimeStatus == "Completed";

            //if not yet done, wait little and call back
            if (!isDone)
            {
                //print holding status for easy debug
                Console.WriteLine($"API SAID : {runtimeStatus}");
                await Task.Delay(300);
                goto TryAgain;
            }

            //once done, get data out
            //person list json to parsed
            var outputToken = result["output"]; //json array
            CachedPublicPersonList = Person.FromJsonList(outputToken); //cache for later use

            return CachedPublicPersonList;
        }

        /// <summary>
        /// Adds new person to API server main list
        /// </summary>
        public async Task AddPerson(Person person)
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

            //if pass, clear local person cache
            await HandleResultClearLocalCache(jsonResult);

        }

        /// <summary>
        /// used to get person direct not in users list for easy sharing
        /// </summary>
        public async Task<Person> GetPerson(string personId)
        {
            var url = $"{_url.GetPerson}/PersonId/{personId}";
            var result = await Tools.ReadServer(url);

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


        /// <summary>
        /// checks status, if pass clears person list cache, for update, delete and add
        /// </summary>
        private async Task HandleResultClearLocalCache(JObject jsonResult)
        {
            //check result, display error if needed
            var isPass = jsonResult["Status"].Value<string>() == "Pass";
            if (isPass)
            {
                //1: clear stored person list
                this.CachedPersonList.Clear();

                //2: clear link to get person list, this will cause to fetch new list
                //since already purged by API, the webhook link will go 404
                this.PersonListWebhook = "";
            }
            else
            {
                //todo handle reply properly
                Console.WriteLine("API SAID : FAIL");
            }
        }


        /// <summary>
        /// takes in raw response from API and
        /// gets payload after checking status and shows error if status "Fail"
        /// note :  no parser use direct, support for string, int and double
        /// </summary>
        private T GetPayload<T>(JObject rawResult, Func<JToken, T>? parser)
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