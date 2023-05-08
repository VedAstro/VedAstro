


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

        private readonly URL _url;
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Standard xml identity payload sent to API for calls, contains user ID and visitor ID
        /// </summary>
        public  XElement StandardPayload => new XElement("Root", new XElement("UserId", _userId), new XElement("VisitorId", _visitorId));


        /// <summary>
        /// Durable function API URL to get ready personlist for this caller
        /// </summary>
        public static string PersonListWebhook { get; set; } = "";

        public static bool IsPersonListReady => string.IsNullOrEmpty(PersonListWebhook);


        public VedAstroAPI(string userId, string visitorId, IJSRuntime jsRuntime, URL url, HttpClient httpClient)
        {
            _userId = userId;
            _visitorId = visitorId;
            _jsRuntime = jsRuntime;
            _url = url;
            _httpClient = httpClient;
        }


        /// <summary>
        /// getting people list is a long process, because of clean up and stuff
        /// so ask server to start prepare, will get results later when needed
        /// </summary>
        public async Task PreparePersonList()
        {
            //tell API to get started
            var url = $"{_url.GetPersonListAsync}/UserId/{_userId}/VisitorId/{_visitorId}";

            //API gives a url to check on poll fo results
            PersonListWebhook = await Tools.ReadServer(url, _httpClient);

        }


        public async Task<List<Person>> GetPersonList()
        {
            //check if person list is ready
            //here result can contain payload if ready
            TryAgain:
            var result = await Tools.ReadServer(PersonListWebhook, _httpClient);
            var parsed = JObject.Parse(result);
            var runtimeStatus = parsed["runtimeStatus"]?.Value<string>() ?? "";
            var isDone = runtimeStatus == "Completed";

            //if not yet done, wait little and call back
            if (!isDone) { await Task.Delay(100); goto TryAgain;}

            //once done, get data out
            //person list json to parsed
            var outputToken = parsed["output"]; //json array
            CachedPersonList = Person.FromJsonList(outputToken); //cache for later use

            return CachedPersonList;
        }

        public List<Person> CachedPersonList { get; set; }


        //--------------------------------------------------

        /// <summary>
        /// data may be cached or from API
        /// Sorted alphabetically
        /// </summary>
        //private async Task<List<Person>> TryGetPersonListSortedAZ()
        //{
        //    var unsorted = await this.TryGetPersonList();
        //    var sortedList = unsorted.OrderBy(person => person.Name).ToList();
        //    return sortedList;
        //}

        //private  async Task<List<Person>> TryGetPersonList()
        //{
        //    //check if people list already loaded before
        //    if (AppData.PersonList == null)
        //    {
        //        Console.WriteLine("BLZ:Get Fresh PersonList");
        //        AppData.PersonList = await WebsiteTools.GetPeopleList();
        //    }
        //    else
        //    {
        //        Console.WriteLine("BLZ:Using PersonList Cache");
        //    }

        //    return AppData.PersonList;
        //}

        ///// <summary>
        ///// Gets all people list from API server
        ///// This is the central place all person list is gotten for a User ID/Visitor ID
        ///// NOTE: API combines person list from visitor id and 
        ///// - if API fail will return empty list
        ///// </summary>
        //public static async Task<List<Person>?> GetPeopleList()
        //{
        //    //STEP 1: make request if needed
        //    if (!AppData.IsPersonListReady)
        //    {
        //        var x = await ServerManager.WriteToServerXmlReply(AppData.URL.GetPersonListAsync, AppData.StandardPayload);
        //        var y = x.Payload.Value;
        //    }

        //    //STEP 2: check request status, wait for completion


        //    //STEP 3: get result



        //}



        /// <summary>
        /// Gets all people list from API server
        /// This is the central place all person list is gotten for a User ID/Visitor ID
        /// NOTE: API combines person list from visitor id and 
        /// - if API fail will return empty list
        /// </summary>
        //public async Task<List<Person>?> GetPeopleListOLD()
        //{

        //    //get all person profile owned by current user/visitor
        //    int timeout = 2;//probability of GOOD reply from API goes down after 2s, so no point waiting
        //    var result = await Tools.WriteToServerXmlReply(_url.GetPersonList,StandardPayload, timeout);

        //    if (result.IsPass)
        //    {
        //        var personList = Person.FromXml(result.Payload.Elements());
        //        return personList;
        //    }
        //    //if fail log it and return empty list as not to break the caller
        //    else
        //    {
        //        //await AppData.JsRuntime.ShowAlert("error", AlertText.FailedNameList, true);
        //        return new List<Person>();
        //    }


        //}

    }
}