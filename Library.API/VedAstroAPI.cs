


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

        private List<Person> CachedPersonList { get; set; }

        private readonly URL _url;

        /// <summary>
        /// Durable function API URL to get ready personlist for this caller
        /// </summary>
        private static string PersonListWebhook { get; set; } = "";

        /// <summary>
        /// true of person is already prepared
        /// </summary>
        private static bool IsPersonPrepared => !string.IsNullOrEmpty(PersonListWebhook);


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
            //tell API to get started
            var url = $"{_url.GetPersonListAsync}/UserId/{_userId}/VisitorId/{_visitorId}";

            //API gives a url to check on poll fo results
            PersonListWebhook = await Tools.ReadServer(url);

        }

        /// <summary>
        /// person will be auto prepared, but might be slow
        /// as such prepare before hand if possible, like when app load
        /// </summary>
        /// <returns></returns>
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
            var parsed = JObject.Parse(result);
            var runtimeStatus = parsed["runtimeStatus"]?.Value<string>() ?? "";

            //note : if error status, will be corrected by API,
            //so here we wait till complete
            var isDone = runtimeStatus == "Completed";

            //if not yet done, wait little and call back
            if (!isDone)
            {
                //print holding status for easy debug
                Console.WriteLine($"API SAID : {runtimeStatus}");
                await Task.Delay(250);
                goto TryAgain;
            }

            //once done, get data out
            //person list json to parsed
            var outputToken = parsed["output"]; //json array
            CachedPersonList = Person.FromJsonList(outputToken); //cache for later use

            return CachedPersonList;
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
            var jsonResult = await Tools.WriteServer(url, personJson);

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
            var jsonResult = await Tools.WriteServer(url);

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
            var jsonResult = await Tools.WriteServer(url, updatedPerson);

            await HandleResultClearLocalCache(jsonResult);

        }

        /// <summary>
        /// used to get person direct not in users list for easy sharing
        /// </summary>
        public async Task<Person> GetPerson(string personId)
        {
            var url = $"{_url.GetPerson}/PersonId/{personId}";
            var result = await Tools.ReadServer(url);
            var parsedResult = JObject.Parse(result);

            //check result, display error if needed
            var isPass = parsedResult["Status"].Value<string>() == "Pass";
            if (isPass)
            {
                var personJson = Person.FromJson(parsedResult["Payload"]);

                return personJson;
            }
            else
            {
                //todo handle reply properly
                Console.WriteLine("API SAID : FAIL");
                return Person.Empty;
            }

        }



        //---------------------------------------------PRIVATE

        private async Task HandleResultClearLocalCache(JObject jsonResult)
        {
            //check result, display error if needed
            var isPass = jsonResult["Status"].Value<string>() == "Pass";
            if (isPass)
            {

                //if all went well clear stored person list
                this.CachedPersonList.Clear();
            }
            else
            {
                //todo handle reply properly
                Console.WriteLine("API SAID : FAIL");
            }
        }



    }
}