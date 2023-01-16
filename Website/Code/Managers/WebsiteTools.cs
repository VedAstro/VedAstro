using System.Xml.Linq;
using Genso.Astrology.Library;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Website
{
    public delegate Task AsyncEventHandler();

    /// <summary>
    /// Simple class holding general functions used in project
    /// </summary>
    public static class WebsiteTools
    {

        //░█▀▀▀ ▀█▀ ░█▀▀▀ ░█─── ░█▀▀▄ ░█▀▀▀█ 　 ─█▀▀█ ░█▄─░█ ░█▀▀▄ 　 ░█▀▀█ ░█▀▀█ ░█▀▀▀█ ░█▀▀█ ░█▀▀▀█ 
        //░█▀▀▀ ░█─ ░█▀▀▀ ░█─── ░█─░█ ─▀▀▀▄▄ 　 ░█▄▄█ ░█░█░█ ░█─░█ 　 ░█▄▄█ ░█▄▄▀ ░█──░█ ░█▄▄█ ─▀▀▀▄▄ 
        //░█─── ▄█▄ ░█▄▄▄ ░█▄▄█ ░█▄▄▀ ░█▄▄▄█ 　 ░█─░█ ░█──▀█ ░█▄▄▀ 　 ░█─── ░█─░█ ░█▄▄▄█ ░█─── ░█▄▄▄█



        //░█▀▄▀█ ░█▀▀▀ ▀▀█▀▀ ░█─░█ ░█▀▀▀█ ░█▀▀▄ ░█▀▀▀█ 
        //░█░█░█ ░█▀▀▀ ─░█── ░█▀▀█ ░█──░█ ░█─░█ ─▀▀▀▄▄ 
        //░█──░█ ░█▄▄▄ ─░█── ░█─░█ ░█▄▄▄█ ░█▄▄▀ ░█▄▄▄█


        /// <summary>
        /// Extension method for setting a Timeout for a Task
        /// </summary>
        public static async Task<TResult> TimeoutAfter<TResult>(this Task<TResult> task, TimeSpan timeout)
        {

            using (var timeoutCancellationTokenSource = new CancellationTokenSource())
            {

                var completedTask = await Task.WhenAny(task, Task.Delay(timeout, timeoutCancellationTokenSource.Token));
                if (completedTask == task)
                {
                    timeoutCancellationTokenSource.Cancel();
                    return await task;  // Very important in order to propagate exceptions
                }
                else
                {
                    throw new TimeoutException("The operation has timed out.");
                }
            }
        }



        /// <summary>
        /// Tries to get user login state, else returns public user id.
        /// Note: Public User Id is the standard id all unregistered visitors get
        /// </summary>
        //public static async Task<string> GetUserIdAsync(IJSRuntime jsRuntime)
        //{
        //    //wait here a little if user has not signed in
        //    //5 X 200 delay = 1 sec wait time max
        //    var signInSuccess = await WaitTillGoogleSignInSuccess(5, 200);

        //    //if sign in failed use Public Id
        //    if (!signInSuccess) { Console.WriteLine("BLZ: GetUserIdAsync: Public ID Assigned"); return PublicUserId; }

        //    //get user Id from variable in JS scope
        //    var userId = AppData.CurrentUser?.Id;

        //    return userId;
        //}

        /// <summary>
        /// if try limit is expired then returns false
        /// try limit 5 X 200 delay = 1 sec wait time max
        /// </summary>
        //public static async Task<bool> WaitTillGoogleSignInSuccess(int tryLimit, int delay)
        //{
        //    var count = 0;
        //    while (!AppData.CurrentUser && count < tryLimit)
        //    {
        //        Console.WriteLine("BLZ: GetUserIdAsync: Waiting For Sign In");
        //        await Task.Delay(delay);
        //        count++;
        //    }

        //    if (!GoogleUserSignedIn && count == tryLimit) { return false; }

        //    //if control reaches here than, user sign in success
        //    return true;
        //}

        /// <summary>
        /// Gets all people list from API server
        /// This is the central place all person list is gotten for a User ID/Visitor ID
        /// NOTE: if User ID is Guest ID 101, then person profile under
        /// Visitor ID is also auto added to return list if any by API
        /// - if API fail will return empty list
        /// </summary>
        public static async Task<List<Person>?> GetPeopleList(IJSRuntime jsRuntime)
        {

            //get all person profile owned by current user/visitor
            var payload = new XElement("Root", new XElement("UserId", AppData.CurrentUser.Id), new XElement("VisitorId", AppData.VisitorId));
            var result = await ServerManager.WriteToServerXmlReply(ServerManager.GetPersonList, payload, jsRuntime);

            if (result.IsPass)
            {
                var personList = Person.FromXml(result.Payload.Elements());
                return personList;
            }
            //if fail log it and return empty list as not to break the caller
            else
            {
                await jsRuntime.ShowAlert("error", "Failed to fetch name list.", true);
                return new List<Person>();
            }


        }



        /// <summary>
        /// Gets all visitor list from API server
        /// </summary>
        public static async Task<List<XElement>> GetVisitorList(string userId, IJSRuntime jsRuntime)
        {
            var result = await ServerManager.WriteToServerXmlReply(ServerManager.GetVisitorList, new XElement("UserId", userId), jsRuntime);
            var visitorList = result.Payload.Elements().ToList();//visitorListRootXml
            return visitorList;
        }


        /// <summary>
        /// Gets person from ID
        /// Checks user's person list,
        /// </summary>
        public static async Task<Person> GetPersonFromId(string personId, IJSRuntime jsRuntime)
        {
            Person foundPerson;

            //get person from cached person list assigned to user/visitor
            foundPerson = await GetFromPersonList(personId);

            //if person found return it, here
            var personFound = foundPerson.Name != Person.Empty.Name; //not empty
            if (personFound) { return foundPerson; }

            //if control reaches here, than the person is not in current users list,
            //so get direct from server, this indicates direct link access to another
            //user's person profile, which is allowed but also monitored
            await WebsiteLogManager.LogData($"Direct Link Access:{personId}");

            foundPerson = await GetFromApi(personId, jsRuntime);

            return foundPerson;


            //LOCAL FUNCTION
            async Task<Person> GetFromApi(string personId, IJSRuntime jsRuntime)
            {
                //send request to API
                var xmlData = Tools.AnyTypeToXml(personId);
                var result = await ServerManager.WriteToServerXmlReply(ServerManager.GetPersonApi, xmlData, jsRuntime);

                //check result
                if (result.IsPass)
                {
                    //if pass get person data out & return to caller
                    var personXml = result.Payload;
                    return Person.FromXml(personXml);
                }
                else
                {
                    //let user know fail, and return empty person
                    await jsRuntime.ShowAlert("error", AlertText.NoPersonFound, true);
                    return Person.Empty;
                }
            }

            async Task<Person> GetFromPersonList(string personId)
            {
                //try to get from person's own user list
                var personList = await AppData.TryGetPersonList(jsRuntime);
                var personFromId = personList.Where(p => p.Id == personId);

                //will return Empty person if none found
                return personFromId.FirstOrDefault(Person.Empty);
            }
        }

        /// <summary>
        /// Deletes person from API server  main list
        /// note:
        /// - if fail will show alert message
        /// - cached person list is cleared here
        /// </summary>
        public static async Task DeletePerson(string personId, IJSRuntime jsRuntime)
        {
            var personIdXml = new XElement("PersonId", personId);
            var result = await ServerManager.WriteToServerXmlReply(ServerManager.DeletePerson, personIdXml, jsRuntime);

            //check result, display error if needed
            if (!result.IsPass)
            {
                WebsiteLogManager.LogError($"BLZ:DeletePerson() Fail:\n{result.Payload}");
                await jsRuntime.ShowAlert("error", AlertText.DeletePersonFail, true);
            }

            //if all went well clear stored person list
            else { AppData.ClearPersonList(); }
        }

        public static async Task DeleteSavedChart(string chartId, IJSRuntime jsRuntime)
        {
            var chartIdXml = new XElement("ChartId", chartId);
            var result = await ServerManager.WriteToServerXmlReply(ServerManager.DeleteChartApi, chartIdXml, jsRuntime);

            //check result, display error if needed
            if (!result.IsPass)
            {
                WebsiteLogManager.LogError($"BLZ:DeleteSavedChart() Fail:\n{result.Payload}");
                await jsRuntime.ShowAlert("error", AlertText.DeleteChartFail, true);
            }

        }

        /// <summary>
        /// Send updated person to API server to be saved in main list
        /// note:
        /// - if fail will show alert message
        /// - cached person list is cleared here
        /// </summary>
        public static async Task UpdatePerson(Person person, IJSRuntime jsRuntime)
        {
            //prepare and send updated person to API server
            var updatedPersonXml = person.ToXml();
            var result = await ServerManager.WriteToServerXmlReply(ServerManager.UpdatePersonApi, updatedPersonXml, jsRuntime);

            //check result, display error if needed
            if (!result.IsPass)
            {
                WebsiteLogManager.LogError($"BLZ:UpdatePerson() Fail:\n{result.Payload}");
                await jsRuntime.ShowAlert("error", AlertText.UpdatePersonFail, true);
            }

            //if all went well clear stored person list
            else { AppData.ClearPersonList(); }
        }

        /// <summary>
        /// Adds new person to API server main list
        /// </summary>
        public static async Task AddPerson(Person person)
        {
            //send newly created person to API server
            var xmlData = person.ToXml();
            var result = await ServerManager.WriteToServerXmlReply(ServerManager.AddPersonApi, xmlData, AppData.JsRuntime);

            //check result, display error if needed
            if (!result.IsPass)
            {
                WebsiteLogManager.LogError($"BLZ:AddPerson() Fail:\n{result.Payload}");
                await AppData.JsRuntime.ShowAlert("error", AlertText.UpdatePersonFail, true);
            }

            //if all went well clear stored person list
            else { AppData.ClearPersonList(); }

        }
        public static void ReloadPage(NavigationManager navigation) => navigation.NavigateTo(navigation.Uri, forceLoad: true);

        /// <summary>
        /// Gets Dasa events from API
        /// </summary>
        public static async Task<List<Event>?> GetDasaEvents(double _eventsPrecision, Time startTime, Time endTime, IJSRuntime _jsRuntime, Person person)
            => await EventsByTag(EventTag.Dasa, _eventsPrecision, startTime, endTime, _jsRuntime, person);

        /// <summary>
        /// Gets Bhukti events from API
        /// </summary>
        public static async Task<List<Event>?> GetBhuktiEvents(double _eventsPrecision, Time startTime, Time endTime, IJSRuntime _jsRuntime, Person person)
            => await EventsByTag(EventTag.Bhukti, _eventsPrecision, startTime, endTime, _jsRuntime, person);

        /// <summary>
        /// Gets Antaram events from API
        /// </summary>
        public static async Task<List<Event>?> GetAntaramEvents(double _eventsPrecision, Time startTime, Time endTime, IJSRuntime _jsRuntime, Person person)
            => await EventsByTag(EventTag.Antaram, _eventsPrecision, startTime, endTime, _jsRuntime, person);

        /// <summary>
        /// gets events from server filtered by event tag
        /// </summary>
        public static async Task<List<Event>?> EventsByTag(EventTag tag, double precisionHours, Time startTime, Time endTime, IJSRuntime _jsRuntime, Person person)
        {

            //get events from API server
            var dasaEventsUnsorted =
                await GetEventsFromApi(
                    startTime,
                    endTime,
                    //birth location always as current place,
                    //since place does not matter for Dasa
                    person.GetBirthLocation(),
                    person,
                    tag,
                    precisionHours, _jsRuntime);


            //sort the list by time before sending view
            var orderByAscResult = from dasaEvent in dasaEventsUnsorted
                                   orderby dasaEvent.StartTime.GetStdDateTimeOffset()
                                   select dasaEvent;


            //send sorted events to view
            return orderByAscResult.ToList();
        }

        /// <summary>
        /// Gets Muhurtha events from API
        /// </summary>
        public static async Task<List<Event>> GetEventsFromApi(Time startTime, Time endTime, GeoLocation location, Person person, EventTag tag, double precisionHours, IJSRuntime _jsRuntime)
        {
            //prepare data to send to API
            var root = new XElement("Root");

            root.Add(
                new XElement("StartTime", startTime.ToXml()),
                new XElement("EndTime", endTime.ToXml()),
                location.ToXml(),
                person.ToXml(),
                Tools.AnyTypeToXml(tag),
                Tools.AnyTypeToXml(precisionHours));


            //send to api and get results
            var resultsRaw = await ServerManager.WriteToServerXmlReply(ServerManager.GetEventsApi, root, _jsRuntime);


            //parse raw results
            List<Event> resultsParsed = Event.FromXml(resultsRaw);


            //send to caller
            return resultsParsed;
        }





        //█▀▀ ▄▀█ █░░ █░░ █▀▀ █▀▄   █▀▀ █▀█ █▀█ █▀▄▀█   ░░█ █▀
        //█▄▄ █▀█ █▄▄ █▄▄ ██▄ █▄▀   █▀░ █▀▄ █▄█ █░▀░█   █▄█ ▄█

        #region called from js


        /// <summary>
        /// This method is called from JS when blazor default error box is shown (#blazor-error-ui)
        /// </summary>
        [JSInvokable]
        public static void OnAppError()
        {
            //log it
            WebsiteLogManager.LogError("Blazor Error Box Shown");
        }


        /// <summary>
        /// Gets event's description from file stored in static website
        /// </summary>
        [JSInvokable]
        public static string GetEventDescription(string eventName)
        {
            Console.WriteLine("BLZ:GetEventDescription");



            //get the data sender
            //using var client = new HttpClient();
            ////tell sender to wait for complete reply before exiting
            //var waitForContent = HttpCompletionOption.ResponseContentRead;
            ////send the data on its way (wait forever no timeout)
            //client.Timeout = new TimeSpan(0, 0, 0, 0, Timeout.Infinite);
            ////get main horoscope prediction file (located in wwwroot)
            ////var eventDataListStream = await client.GetStreamAsync("/data/EventDataList.xml");
            //var eventDataListStream = await client.GetStreamAsync("https://www.vedastro.tk/data/EventDataList.xml");
            //Console.WriteLine(eventDataListStream.Length);


            return "Test";


            //parse event data list file
            //var eventDataListXml = XDocument.Load(eventDataListStream);

            //find the event description by the name
            //var desc = (from eventXml in eventDataListXml.Root.Elements()
            //where eventXml.Element("Name").Value == eventName
            //select eventXml.Element("Description").Value).FirstOrDefault();

            //Console.WriteLine(desc);

            //return desc;
            ////parse each raw event data in list
            //var eventDataList = new List<EventData>();
            //foreach (var eventData in eventDataListXml.Root.Elements())
            //{
            //    //add it to the return list
            //    eventDataList.Add(EventData.ToXml(eventData));
            //}

            //return eventDataList;


        }

        /// <summary>
        /// Gets date time string in standard format,
        /// example call : var x = DotNet.invokeMethod('Website', 'GetNowTimeString');
        /// used to generate time for new life events
        /// </summary>
        /// <returns></returns>
        [JSInvokable]
        public static string GetNowTimeString()
        {
            var returnVal = DateTimeOffset.Now.ToString(Time.DateTimeFormat);
#if DEBUG
            Console.WriteLine("BLZ:GetNowTimeString:" + returnVal);
#endif
            return returnVal;
        }


        #endregion




        //█▀▀ ▀▄▀ ▀█▀ █▀▀ █▄░█ █▀ █ █▀█ █▄░█   █▀▄▀█ █▀▀ ▀█▀ █░█ █▀█ █▀▄ █▀
        //██▄ █░█ ░█░ ██▄ █░▀█ ▄█ █ █▄█ █░▀█   █░▀░█ ██▄ ░█░ █▀█ █▄█ █▄▀ ▄█


        /// <summary>
        /// Special function to catch async exceptions, but has to be called correctly
        /// Note:
        /// - If caught here overwrites default blazor error handling 
        /// - Not all await calls need this only the top level needs this,
        /// example use inside OnClick or OnInitialized will do.
        /// example: await InvokeAsync(async () => await DeletePerson()).HandleErrors();
        /// </summary>
        public static async Task Try(this Task invocation, IJSRuntime jsRuntime)
        {
            //counts of failure before force refresh page
            const int failureThreshold = 3;

            try
            {
                //try to make call normally
                await invocation;
            }
            catch (Exception e)
            {

                //based on error show the appropriate message
                switch (e)
                {
                    //no internet just, just show dialog box and do nothing
                    case NoInternetError:
                        await jsRuntime.ShowAlert("error", AlertText.NoInternet, true);
                        break;

                    //here we have internet but somehow failed when talking to API server
                    //possible cause:
                    // - code mismatch between client & server
                    // - slow or unstable internet connection
                    //best choice is to redirect 
                    case ApiCommunicationFailed:
                        await jsRuntime.ShowAlert("error", AlertText.ServerConnectionProblem(), true);
                        break;

                    //failure here can't be recovered, so best choice is to refresh page to home
                    //redirect with reload to clear memory & restart app
                    default:
                        //note : for unknown reason, when app starts multiple failures occur, for now
                        if (AppData.FailureCount > failureThreshold)
                        {
                            await jsRuntime.ShowAlert("warning", AlertText.SorryNeedRefreshToHome, true);
                            await jsRuntime.LoadPage(PageRoute.Home);
                        }
                        else
                        {
                            AppData.FailureCount++;
                            Console.WriteLine($"BLZ: Failure Count: {AppData.FailureCount}");
                        }
                        break;
                }

#if DEBUG
                //if running locally, print error to console
                Console.WriteLine(e.ToString());
#else
                //if Release log error & end silently
                WebsiteLogManager.LogError(e, "Error from WebsiteTools.Try()");
#endif

                //note exception will not go past this point,
                //even calling throw will do nothing
                //throw;
            }
        }


        /// <summary>
        /// Gets now time with seconds in wrapped in xml element
        /// used for logging
        /// </summary>
        public static XElement TimeStampSystemXml => new("TimeStamp", Tools.GetNowSystemTimeSecondsText());

        /// <summary>
        /// Gets now time at server location (+8:00) with seconds in wrapped in xml element
        /// used for logging
        /// </summary>
        public static XElement TimeStampServerXml => new("TimeStampServer", Tools.GetNowServerTimeSecondsText());

        /// <summary>
        /// Gets XML file from any URL and parses it into xelement list
        /// </summary>
        public static async Task<List<XElement>> GetXmlFile(string url, HttpClient? client = null)
        {
            //if client not specified then make new one
            if (client == null)
            {
                client = new HttpClient();
                client.BaseAddress = AppData.BaseAddress;
            }

            //load xml event data files before hand to be used quickly later for search
            //get main horoscope prediction file (located in wwwroot)
            var fileStream = await client.GetStreamAsync(url);

            //parse raw file to xml doc
            var document = XDocument.Load(fileStream);

            //get all records in document
            return document.Root.Elements().ToList();
        }

        /// <summary>
        /// Tries to get visitor ID from browser else makes new Visitor ID
        /// also update is new visitor flag
        /// </summary>
        public static async Task<string?> TryGetVisitorId(IJSRuntime jsRuntime)
        {
            //find out if new visitor just arriving or old one browsing
            var visitorId = await jsRuntime.GetProperty("VisitorId"); //local storage
            AppData.IsNewVisitor = visitorId is null or "";

            //generate new ID if not found
            if (AppData.IsNewVisitor)
            {
                visitorId = Tools.GenerateId();
                //save new Visitor ID browser local storage
                await jsRuntime.SetProperty("VisitorId", visitorId);
            }

            //return new or saved ID
            return visitorId;
        }

        public static async Task<Person> GetPersonIdFromChartId(string selectedChartId, IJSRuntime jsRuntime)
        {
            //get person hash from api
            var chartIdXml = new XElement("ChartId", selectedChartId);
            var result = await ServerManager.WriteToServerXmlReply(ServerManager.GetPersonIdFromSavedChartId, chartIdXml, jsRuntime);
            var personId = result.Payload.Value;//xml named person id
            var selectedPerson = await GetPersonFromId(personId, jsRuntime);

            return selectedPerson;
        }

        /// <summary>
        /// Given true or false will return CSS style string with display none
        /// 
        /// </summary>
        public static string GetCssHideShow(bool isReady)
        {
            var displayProp = isReady ? "" : "display: none; ";
            var css = $"style=\"{displayProp}";

            return css;
        }
    }
}
