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

        public static async Task<dynamic> GetAddressLocation(string address, IJSRuntime jsRuntime)
        {
            //create the request url for Google API
            var url = $"https://maps.googleapis.com/maps/api/geocode/xml?key={ServerManager.GoogleGeoLocationApiKey}&address={Uri.EscapeDataString(address)}&sensor=false";

            //get location data from GoogleAPI
            var rawReplyXml = await ServerManager.ReadFromServerXmlReply(url, jsRuntime);

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
        public static async Task<string> CoordinateToAddress(decimal longitude, decimal latitude, IJSRuntime jsRuntime)
        {
            //create the request url for Google API
            var url = string.Format($"https://maps.googleapis.com/maps/api/geocode/xml?latlng={latitude},{longitude}&key={ServerManager.GoogleGeoLocationApiKey}");


            //get location data from GoogleAPI
            var rawReplyXml = await ServerManager.ReadFromServerXmlReply(url, jsRuntime);

            //extract out the longitude & latitude
            var locationData = new XDocument(rawReplyXml);
            var localityResult = locationData.Element("GeocodeResponse")?.Elements("result").FirstOrDefault(result => result.Element("type")?.Value == "locality");
            var locationName = localityResult?.Element("formatted_address")?.Value;


            return locationName;

        }

        /// <summary>
        /// Given a location & time, will use Google Timezone API
        /// to get accurate time zone that was/is used
        /// </summary>
        public static async Task<string> GetTimezoneOffset(GeoLocation geoLocation, DateTimeOffset timeAtLocation, IJSRuntime jsRuntime)
        {
            //use timestamp to account for historic timezone changes
            var locationTimeUnix = timeAtLocation.ToUnixTimeSeconds();
            var longitude = geoLocation.GetLongitude();
            var latitude = geoLocation.GetLatitude();

            //create the request url for Google API
            var url = string.Format($@"https://maps.googleapis.com/maps/api/timezone/xml?location={latitude},{longitude}&timestamp={locationTimeUnix}&key={ServerManager.GoogleGeoLocationApiKey}");


            //get location data from GoogleAPI
            //< TimeZoneResponse >
            //  < status > OK </ status >
            //  < raw_offset > 28800.0000000 </ raw_offset >
            //  < dst_offset > 0.0000000 </ dst_offset >
            //  < time_zone_id > Asia / Kuala_Lumpur </ time_zone_id >
            //  < time_zone_name > Malaysia Time </ time_zone_name >
            //</ TimeZoneResponse >
            var timeZoneResponseXml = await ServerManager.ReadFromServerXmlReply(url, jsRuntime);

            //extract out the timezone offset
            var offsetSeconds = double.Parse(timeZoneResponseXml?.Element("raw_offset")?.Value);
            //offset needs to be "whole" minutes, else fail
            //purposely hard cast to int to remove not whole minutes
            var notWhole = TimeSpan.FromSeconds(offsetSeconds).TotalMinutes;
            var offsetMinutes = TimeSpan.FromMinutes((int)Math.Round(notWhole));
            var parsedOffsetString = DateTimeOffset.UtcNow.ToOffset(offsetMinutes).ToString("zzz");

            return parsedOffsetString;

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
        /// This is the central place all person list is gotten for a User ID
        /// NOTE: if User ID is Guest ID 101, then person profile under
        /// Visitor ID is also auto added to return list if any
        /// </summary>
        public static async Task<List<Person>?> GetPeopleList(string userId, IJSRuntime jsRuntime)
        {

            var personListRootXml = await ServerManager.WriteToServerXmlReply(ServerManager.GetPersonListApi, new XElement("UserId", userId), jsRuntime);
            var personList = personListRootXml.Elements().Select(personXml => Person.FromXml(personXml)).ToList();

            //if guest user, include person list from visitor ID if any
            //this is done because any person profile saved without login will be
            //stored under Visitor ID
            if (AppData.IsGuestUser)
            {
                //get list for visitor ID instead of User Id
                personListRootXml = await ServerManager.WriteToServerXmlReply(
                     ServerManager.GetPersonListApi, new XElement("UserId", AppData.VisitorId), jsRuntime);
                var tempList = personListRootXml.Elements().Select(personXml => Person.FromXml(personXml)).ToList();

                //combine with previous list
                if (tempList.Any()) { personList.AddRange(tempList); }
            }


            return personList;
        }



        /// <summary>
        /// Gets all visitor list from API server
        /// </summary>
        public static async Task<List<XElement>> GetVisitorList(string userId, IJSRuntime jsRuntime)
        {
            var visitorListRootXml = await ServerManager.WriteToServerXmlReply(ServerManager.GetVisitorList, new XElement("UserId", userId), jsRuntime);
            var visitorList = visitorListRootXml.Elements().ToList();
            return visitorList;
        }

        public static async Task<List<Person>> GetMalePeopleList(string userId, IJSRuntime jsRuntime)
        {
            var rawMaleListXml = await ServerManager.WriteToServerXmlReply(ServerManager.GetMaleListApi, new XElement("UserId", userId), jsRuntime);
            return rawMaleListXml.Elements().Select(maleXml => Person.FromXml(maleXml)).ToList();
        }

        public static async Task<List<Person>> GetFemalePeopleList(string userId, IJSRuntime jsRuntime)
        {
            var rawMaleListXml = await ServerManager.WriteToServerXmlReply(ServerManager.GetFemaleListApi, new XElement("UserId", userId), jsRuntime);
            return rawMaleListXml.Elements().Select(maleXml => Person.FromXml(maleXml)).ToList();
        }

        /// <summary>
        /// Gets person instance from name contacts API
        /// Note: - uses API to get latest data
        ///       - will retry forever till result comes
        /// </summary>
        public static async Task<Person> GetPersonFromHash(string personHash, IJSRuntime jsRuntime)
        {
            //timeout implemented because calls have
            //known to fail with stable connection
            var timeoutMs = 650;

            //if can't be recovered within limit
            //then code related error
            var tryLimit = 3;
            var tryCount = 0;

            Person personFromHash;

        TryAgain:
            try
            {
                personFromHash = await DoWork().TimeoutAfter(TimeSpan.FromMilliseconds(timeoutMs));
                return personFromHash;
            }
            catch (Exception e)
            {
                //if internet connection is fine, try again
                //also keep within try max, to stop looping
                var isOnline = await jsRuntime.IsOnline();
                if (isOnline && tryCount <= tryLimit)
                {
                    Console.WriteLine($"BLZ: GetPersonFromHash: {timeoutMs}ms Timeout!!");
                    tryCount++;
                    goto TryAgain;
                }

                //we need to throw the exception to stop execution
                //to show error alert which will reload to home
                //since at this point it is unrecoverable
                throw new ApiCommunicationFailed($"Error in GetPersonFromHash()", e);
            }


            async Task<Person> DoWork()
            {
                var xmlData = Tools.AnyTypeToXml(personHash);
                var result = await ServerManager.WriteToServerXmlReply(ServerManager.GetPersonApi, xmlData, jsRuntime);

                var personXml = result.Element("Person");

                //parse received person
                var receivedPerson = Person.FromXml(personXml);

                return receivedPerson;
            }

        }

        /// <summary>
        /// NOTE Person list has to be loaded or will fail
        /// </summary>
        public static Person GetPersonFromHashCached(string personHash)
        {
            return AppData.PersonList.Find(p => p.GetHashCode() == int.Parse(personHash));
        }

        public static async Task DeletePerson(int personHash, IJSRuntime jsRuntime)
        {
            var personHashXml = new XElement("PersonHash", personHash);
            var result = await ServerManager.WriteToServerXmlReply(ServerManager.DeletePersonApi, personHashXml, jsRuntime);

            //check result, display error if needed
            if (result.Value != "Pass") { Console.WriteLine($"BLZ: ERROR: Delete Person API\n{result.Value}"); }
        }

        public static async Task UpdatePerson(Person person, int originalPersonHash, IJSRuntime jsRuntime)
        {
            var updatedPersonXml = person.ToXml();
            Console.WriteLine(updatedPersonXml.ToString());
            var oriPersonHashXml = new XElement("PersonHash", originalPersonHash);
            var rootXml = new XElement("Root");
            rootXml.Add(oriPersonHashXml, updatedPersonXml);
            var result = await ServerManager.WriteToServerXmlReply(ServerManager.UpdatePersonApi, rootXml, jsRuntime);

            //check result, display error if needed
            if (result.Value != "Pass")
            {
                WebsiteLogManager.LogError($"Update Person API Fail\n{result.Value}");
                await jsRuntime.ShowAlert("error", AlertText.UpdatePersonFail, true);
            }

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
                        await jsRuntime.ShowAlert("error", AlertText.ServerConnectionProblem, true);
                        break;

                    //failure here can't be recovered, so best choice is to refresh page to home
                    //redirect with reload to clear memory & restart app
                    default:
                        //note : for unknown reason, when app starts multiple failures occur, for now
                        if (AppData.FailureCount > 3)
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
        public static XElement TimeStampXml => new("TimeStamp", Tools.GetNowSystemTimeSecondsText());

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
    }
}
