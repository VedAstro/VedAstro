using System.Text.Json.Nodes;
using System.Xml;
using System.Xml.Linq;
using Genso.Astrology.Library;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using Website.Managers;
using Website.Pages;

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


        /// <summary>
        /// Event fired just after user has signed in
        /// </summary>
        public static event AsyncEventHandler OnUserSignIn;


        /// <summary>
        /// Event fired just after user has signed out
        /// </summary>
        public static event AsyncEventHandler OnUserSignOut;

        /// <summary>
        /// Is true when only when Google Sign In success
        /// False when Google Sign Out success
        /// </summary>
        public static bool GoogleUserSignedIn { get; set; }


        /// <summary>
        /// Default User ID for all before they sign in
        /// </summary>
        public const string PublicUserId = "101";




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



        public static async Task<dynamic> GetAddressLocation(string address)
        {
            //create the request url for Google API
            var url = $"https://maps.googleapis.com/maps/api/geocode/xml?key={ServerManager.GoogleGeoLocationApiKey}&address={Uri.EscapeDataString(address)}&sensor=false";

            //get location data from GoogleAPI
            var rawReplyXml = await ServerManager.ReadFromServer(url);

            //extract out the longitude & latitude
            var locationData = new XDocument(rawReplyXml);
            var result = locationData.Element("GeocodeResponse").Element("result");
            var locationElement = result.Element("geometry").Element("location");
            var lat = Double.Parse(locationElement.Element("lat").Value);
            var lng = Double.Parse(locationElement.Element("lng").Value);

            //round coordinates to 3 decimal places
            lat = Math.Round(lat, 3);
            lng = Math.Round(lng, 3);

            //get full name with country & state
            var fullName = result.Element("formatted_address").Value;

            return new { FullName = fullName, Latitude = lat, Longitude = lng };

        }


        /// <summary>
        /// gets the name of the place given th coordinates, uses Google API
        /// </summary>
        public static async Task<string> CoordinateToAddress(decimal longitude, decimal latitude)
        {
            //create the request url for Google API
            var url = string.Format($"https://maps.googleapis.com/maps/api/geocode/xml?latlng={latitude},{longitude}&key={ServerManager.GoogleGeoLocationApiKey}");


            //get location data from GoogleAPI
            var rawReplyXml = await ServerManager.ReadFromServer(url);

            //extract out the longitude & latitude
            var locationData = new XDocument(rawReplyXml);
            var localityResult = locationData.Element("GeocodeResponse")?.Elements("result").FirstOrDefault(result => result.Element("type")?.Value == "locality");
            var locationName = localityResult?.Element("formatted_address")?.Value;


            return locationName;

        }




        /// <summary>
        /// Tries to get user login state, else returns public user id.
        /// Note: Public User Id is the standard id all unregistered visitors get
        /// </summary>
        public static async Task<string> GetUserIdAsync(IJSRuntime jsRuntime)
        {
            //wait here a little if user has not signed in
            //5 X 200 delay = 1 sec wait time max
            var signInSuccess = await WaitTillGoogleSignInSuccess(5, 200);

            //if sign in failed use Public Id
            if (!signInSuccess) { Console.WriteLine("BLZ: GetUserIdAsync: Public ID Assigned"); return PublicUserId; }

            //get user Id from variable in JS scope
            var userId = await jsRuntime.InvokeAsync<string>("getGoogleUserIdToken");

            return userId;
        }


        /// <summary>
        /// if try limit is expired then returns false
        /// try limit 5 X 200 delay = 1 sec wait time max
        /// </summary>
        public static async Task<bool> WaitTillGoogleSignInSuccess(int tryLimit, int delay)
        {
            var count = 0;
            while (!GoogleUserSignedIn && count < tryLimit)
            {
                Console.WriteLine("BLZ: GetUserIdAsync: Waiting For Sign In");
                await Task.Delay(delay);
                count++;
            }

            if (!GoogleUserSignedIn && count == tryLimit) { return false; }

            //if control reaches here than, user sign in success
            return true;
        }

        /// <summary>
        /// Gets all people list from API server
        /// </summary>
        public static async Task<List<Person>> GetPeopleList(string userId)
        {
            var personListRootXml = await ServerManager.WriteToServer(ServerManager.GetPersonListApi, new XElement("UserId", userId));
            var personList = personListRootXml.Elements().Select(personXml => Person.FromXml(personXml)).ToList();
            return personList;
        }
        /// <summary>
        /// Gets all visitor list from API server
        /// </summary>
        public static async Task<List<XElement>> GetVisitorList(string userId)
        {
            var visitorListRootXml = await ServerManager.WriteToServer(ServerManager.GetVisitorList, new XElement("UserId", userId));
            //var visitorList = visitorListRootXml.Elements().Select(personXml => Person.FromXml(personXml)).ToList();
            var visitorList = visitorListRootXml.Elements().ToList();
            return visitorList;
        }

        public static async Task<List<Person>> GetMalePeopleList(string userId)
        {
            var rawMaleListXml = await ServerManager.WriteToServer(ServerManager.GetMaleListApi, new XElement("UserId", userId));
            return rawMaleListXml.Elements().Select(maleXml => Person.FromXml(maleXml)).ToList();
        }

        public static async Task<List<Person>> GetFemalePeopleList(string userId)
        {
            var rawMaleListXml = await ServerManager.WriteToServer(ServerManager.GetFemaleListApi, new XElement("UserId", userId));
            return rawMaleListXml.Elements().Select(maleXml => Person.FromXml(maleXml)).ToList();
        }

        /// <summary>
        /// Gets person instance from name contacts API
        /// Note: - uses API to get latest data
        ///       - will retry forever till result comes
        /// </summary>
        public static async Task<Person> GetPersonFromHash(string personHash)
        {
            //timeout implemented because calls have known to fail
            var timeoutMs = 500;
            Person personFromHash;
        TryAgain:
            try
            {
                personFromHash = await DoWork().TimeoutAfter(TimeSpan.FromMilliseconds(timeoutMs));
                return personFromHash;
            }
            catch (Exception e)
            {
                Console.WriteLine($"BLZ: GetPersonFromHash: {timeoutMs}ms Timeout!!");
                goto TryAgain;
            }


            async Task<Person> DoWork()
            {
                //send newly created person to API server
                var xmlData = Tools.AnyTypeToXml(personHash);
                var result = await ServerManager.WriteToServer(ServerManager.GetPersonApi, xmlData);

                var personXml = result.Element("Person");

                //parse received person
                var receivedPerson = Person.FromXml(personXml);

                return receivedPerson;
            }

        }

        public static async Task DeletePerson(int personHash)
        {
            var personHashXml = new XElement("PersonHash", personHash);
            var result = await ServerManager.WriteToServer(ServerManager.DeletePersonApi, personHashXml);

            //check result, display error if needed
            if (result.Value != "Pass") { Console.WriteLine($"BLZ: ERROR: Delete Person API\n{result.Value}"); }
        }

        public static async Task UpdatePerson(Person person, int originalPersonHash)
        {
            var updatedPersonXml = person.ToXml();
            var oriPersonHashXml = new XElement("PersonHash", originalPersonHash);
            var rootXml = new XElement("Root");
            rootXml.Add(oriPersonHashXml, updatedPersonXml);
            var result = await ServerManager.WriteToServer(ServerManager.UpdatePersonApi, rootXml);

            //check result, display error if needed
            if (result.Value != "Pass") { Console.WriteLine($"BLZ: ERROR: Update Person API\n{result.Value}"); }

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


            await _jsRuntime.AddToProgressBar(10);

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
            var resultsRaw = await ServerManager.WriteToServer(ServerManager.GetEventsApi, root);


            //parse raw results
            List<Event> resultsParsed = Event.FromXml(resultsRaw);

            await _jsRuntime.AddToProgressBar(10);

            //send to caller
            return resultsParsed;
        }



        //█▀▀ ▄▀█ █░░ █░░ █▀▀ █▀▄   █▀▀ █▀█ █▀█ █▀▄▀█   ░░█ █▀
        //█▄▄ █▀█ █▄▄ █▄▄ ██▄ █▄▀   █▀░ █▀▄ █▄█ █░▀░█   █▄█ ▄█

        #region called from js

        /// <summary>
        /// This method is called from JS when user signs in
        /// </summary>
        [JSInvokable]
        public static void InvokeOnUserSignIn()
        {
            //remember user signed in
            GoogleUserSignedIn = true;
            //let others know
            OnUserSignIn?.Invoke();
        }

        /// <summary>
        /// This method is called from JS when user signs out
        /// </summary>
        [JSInvokable]
        public static void InvokeOnUserSignOut()
        {
            //remember user signed out
            GoogleUserSignedIn = false;
            //let others know
            OnUserSignOut?.Invoke();
        }

        /// <summary>
        /// This method is called from JS when user signs out
        /// </summary>
        [JSInvokable]
        public static void OnAppError()
        {
            Console.WriteLine("BLZ: OnAppError");
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
        public static async Task Try(this Task invocation)
        {
            try
            {
                //try to make call normally
                await invocation;
            }
            catch (Exception e)
            {
#if DEBUG
                //if running locally, print error to console
                Console.WriteLine(e.ToString());
#else
                //if Release log error & end silently
                WebsiteLogManager.LogError(e);
#endif


                //note exception will not go past this point,
                //even calling throw will do nothing
                //throw;
            }
        }

    }
}
