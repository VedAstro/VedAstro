using Newtonsoft.Json.Linq;
using VedAstro.Library;

namespace Website;

public class PersonTools
{

    private readonly VedAstroAPI _api;

    //PUBLIC

    public PersonTools(VedAstroAPI vedAstroApi) => _api = vedAstroApi;

    /// <summary>
    /// getting people list is a long process, because of clean up and stuff
    /// so ask server to start prepare, will get results later when needed
    /// </summary>
    public void PreparePersonList()
    {
        //send the calls end of story, dont expect to check on it until needed let server handle it

        //get person list from server or cache and stores reference for later use
        AppData.API.Person.GetPersonList();
        AppData.API.Person.GetPublicPersonList();
    }


    /// <summary>
    /// person will be auto prepared, but might be slow
    /// as such prepare beforehand if possible, like when app load
    /// </summary>
    public async Task<List<Person>> GetPersonList()
    {
        //CHECK CACHE
        //will be cleared when update is needed   
        var cachedPersonList = await AppData.GetCachedPersonList();
        if (cachedPersonList.Any()) { return cachedPersonList; }

        //if not yet logged in then use visitor id as owner id
        var ownerId = _api.UserId == "101" ? _api.VisitorID : _api.UserId;

        //prepare url to call
        var url = $"{_api.URL.GetPersonList}/OwnerId/{ownerId}/VisitorId/{_api.VisitorID}";
        var listNoPolling = await _api.GetListNoPolling(url, Person.FromJsonList);

        //NOTE: ToList is needed to make clone, else copies by ref and is lost
        await AppData.SetCachedPersonList(listNoPolling.ToList());

        return await AppData.GetCachedPersonList();
    }

    public async Task<List<Person>> GetPublicPersonList()
    {
        //CHECK CACHE
        //will be cleared when update is needed
        var cachedPersonList = await AppData.GetCachedPersonList(true);
        if (cachedPersonList.Any()) { return cachedPersonList; }

        //tell API to get started
        var url2 = $"{_api.URL.GetPersonList}/OwnerId/101";
        var listNoPolling = await _api.GetListNoPolling(url2, Person.FromJsonList);

        //NOTE: ToList is needed to make clone, else copies by ref and is lost
        await AppData.SetCachedPersonList(listNoPolling.ToList(), true);

        return await AppData.GetCachedPersonList(true);
    }

    /// <summary>
    /// Adds new person to API server main list
    /// </summary>
    public async Task<string> AddPerson(Person person, bool disableAlert = false)
    {

        //create id that will own person, if logged in use "user id" else use "session id"
        var ownerId = _api.UserId == "101" ? _api.VisitorID : _api.UserId;

        //send newly created person to API server
        //pass in user id to make sure user has right to delete
        //http://localhost:7071/api/AddPerson/OwnerId/234324x24/Name/Romeo/Gender/Female/Location/London/Time/13:45/01/06/1990
        var url = $"{_api.URL.AddPerson}/OwnerId/{ownerId}/Name/{person.Name}" +
                  $"/Gender/{person.Gender}" +
                  $"/Location/{Tools.RemoveWhiteSpace(person.GetBirthLocation().Name())}" +
                  $"/Time/{person.BirthHourMinute}/{person.BirthDateMonthYear}";

        //get location data from Azure Maps API
        var apiResult = await Tools.ReadFromServerJsonReply(url);

        if (apiResult.IsPass) // All well
        {
            //get new person id out
            var personId = apiResult.Payload["Payload"].Value<string>();

            //if pass, clear local person cache & show appropriate done message to user
            await HandleResultClearLocalCache(person.DisplayName, apiResult, "add", disableAlert);

            return personId;
        }
        // If result from API is a failure
        else
        {
            //TODO better logging
            var errorText = apiResult.Payload["Payload"].Value<string>();
            Console.WriteLine(errorText);
            throw new Exception("FAILED TO ADD PERSON!");
        }



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
        var url = $"{_api.URL.DeletePerson}/OwnerId/{_api.UserId}/PersonId/{personToDelete.Id}";

        //API gives a url to check on poll fo results
        var jsonResult = await Tools.WriteServer<JObject, object>(HttpMethod.Get, url);

#if DEBUG
        Console.WriteLine($"SERVER SAID:\n{jsonResult}");
#endif

        //if pass, clear local person cache
        await HandleResultClearLocalCache(personToDelete.DisplayName, jsonResult, "delete"); //task is for message box

    }


    public async Task DeleteLifeEvent(LifeEvent lifeEventToDelete)
    {
        //tell API to get started
        //pass in user id to make sure user has right to delete
        var url = $"{_api.URL.DeletePerson}/OwnerId/{_api.UserId}/PersonId/{lifeEventToDelete.Id}";

        //API gives a url to check on poll fo results
        var jsonResult = await Tools.WriteServer<JObject, object>(HttpMethod.Get, url);

#if DEBUG
        Console.WriteLine($"SERVER SAID:\n{jsonResult}");
#endif

        //if pass, clear local person cache
        await HandleResultClearLocalCache("Life event", jsonResult, "delete"); //task is for message box

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
        var url = $"{_api.URL.UpdatePerson}";
        var jsonResult = await Tools.WriteServer<JObject, JToken>(HttpMethod.Post, url, updatedPerson);


#if DEBUG
        Console.WriteLine($"SERVER SAID:\n{jsonResult}");
#endif

        //if pass, clear local person cache
        await HandleResultClearLocalCache(person.DisplayName, jsonResult, "update");

    }

    /// <summary>
    /// Person is needed to clear local cache. Not sent to server.
    /// </summary>
    public async Task UpsertLifeEvent(Person person, LifeEvent lifeEvent)
    {
        //prepare and send updated person to API server
        var eventJson = lifeEvent.ToJson();
        var url = $"{_api.URL.UpsertLifeEvent}";
        var jsonResult = await Tools.WriteServer<JObject, JToken>(HttpMethod.Post, url, eventJson);


#if DEBUG
        Console.WriteLine($"SERVER SAID:\n{jsonResult}");
#endif

        //if pass, clear local person cache
        await HandleResultClearLocalCache(person.DisplayName, jsonResult, "update");
    }

    /// <summary>
    /// used to get person direct not in users list for easy sharing
    /// </summary>
    public async Task<Person> GetPerson(string personId)
    {
        var url = $"{_api.URL.GetPerson}/OwnerId/{AppData.CurrentUser.Id}/PersonId/{personId}";
        var result = await Tools.ReadServerRaw<JObject>(url);

        //get parsed payload from raw result
        var person = Tools.GetPayload(result, Person.FromJson);

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
        var jsonResult = await Tools.WriteServer<JObject, object>(HttpMethod.Get, url);

        //get parsed payload from raw result
        string personId = Tools.GetPayload<string>(jsonResult, null);

        return personId;
    }


    //PRIVATE




    //---------------------------------------------PRIVATE
    /// <summary>
    /// checks status, if pass clears person list cache, for update, delete and add
    /// extra data needed to show pop up message
    /// </summary>
    private async Task HandleResultClearLocalCache(string personName, JToken jsonResult, string task, bool disableAlert = false)
    {

        //if anything but pass, raise alarm
        var status = jsonResult["Status"]?.Value<string>() ?? "";

        //show or hide alert logic based on parent caller
        if (!disableAlert)
        {
            //FAIL
            if (status != "Pass")
            {
                var failMessage = jsonResult["Payload"]?.Value<string>() ?? "Server didn't give reason, pls try later.";
                await _api.ShowAlert("error", $"Server said no to your request! Why?", failMessage);
            }
            //PASS
            else
            {
                //let user know person has been updates
                await _api.ShowAlert("success", $"{personName} {task} complete!", false, timer: 1000);
            }
        }

        //only clear cache if Pass
        if (status == "Pass")
        {
            //1: clear stored person list
            await AppData.ClearCachedPersonList();
        }

    }

}