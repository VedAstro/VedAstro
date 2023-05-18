using System.Xml.Linq;
using VedAstro.Library;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Threading;

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

        public static async Task<string> Post(string apiUrl, XElement xmlData)
        {
            //this call will take you to NetworkThread.js
            var rawPayload = await AppData.JsRuntime.InvokeAsync<string>(JS.postWrapper, apiUrl, xmlData.ToString(SaveOptions.DisableFormatting));

            //todo proper checking of status needed
            return rawPayload;
        }

        public static async Task OnClickShareFacebook(string pdfFileName, ElementReference elementToConvert)
        {
            var currentUrl = await AppData.JsRuntime.GetCurrentUrl();
            await AppData.JsRuntime.InvokeVoidAsync(JS.shareDialogFacebook, currentUrl);
        }

        /// <summary>
        /// show box to get email and log for sending todo
        /// </summary>
        public static async Task OnClickSendToEmail(string pdfFileName, ElementReference elementToConvert)
        {
            //get email from user via js sweet alert lib
            var emailFromAlert = await AppData.JsRuntime.ShowSendToEmail("Send PDF to...");

            //calls special JS lib to convert html version of the chart to PDF
            //and initiated download as well, with 1 call
            var cleanFileName = Tools.RemoveWhiteSpace(pdfFileName); //remove spaces so that no errors and looks clean in URL

            //will also show complete alert after done
            await AppData.JsRuntime.InvokeVoidAsync(JS.htmlToEmail, elementToConvert, cleanFileName, "pdf", emailFromAlert);

        }


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
        /// Reads data stamped build version, if "beta" is found in that name, return true
        /// note, AssemblyInformationalVersion is custom set in Directory.Build.props
        /// </summary>
        public static bool GetIsBetaRuntime() => ThisAssembly.BranchName.Contains("beta");



        /// <summary>
        /// Gets a list of saved match reports for a user/visitor
        /// </summary>
        /// <returns></returns>
        public static async Task<List<MatchReport>?> GetSavedMatchList(string userId, string visitorId)
        {
            //get all person profile owned by current user/visitor
            var payload = new XElement("Root", new XElement("UserId", userId), new XElement("VisitorId", visitorId));
            var result = await ServerManager.WriteToServerXmlReply(AppData.URL.GetMatchReportList, payload);

            //get match data out and parse it (if all went well)
            if (result.IsPass) { return MatchReport.FromXml(result.Payload.Elements()); }

            //if fail log it and return empty list as not to break the caller
            else
            {
                await AppData.JsRuntime.ShowAlert("error", AlertText.ServerConnectionProblem(), true);
                return new List<MatchReport>();
            }


        }

        /// <summary>
        /// Gets all visitor list from API server
        /// </summary>
        public static async Task<List<XElement>> GetVisitorList(string userId)
        {
            //get result from server
            var result = await ServerManager.WriteToServerXmlReply(AppData.URL.GetVisitorList, new XElement("UserId", userId));

            //if server replied pass, then forward data to caller,
            //else raise alarm and return empty list
            List<XElement> visitorList;//visitorListRootXml
            if (result.IsPass)
            {
                visitorList = result.Payload.Elements().ToList();
            }
            else
            {
                await WebLogger.Error("GetVisitorList Failed, returned empty list");
                visitorList = new List<XElement>();
            }
            return visitorList;
        }

        //marked for deletion
        public static Person GetPersonFromId2(string personId, IJSRuntime jsRuntime)
        {
            var result = GetPersonById(personId, jsRuntime).Result;
            return result;
        }
        
        /// <summary>
        /// Gets person from ID
        /// Checks user's person list,
        /// </summary>
        public static async Task<Person> GetPersonById(string personId, IJSRuntime jsRuntime)
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
            await WebLogger.Data($"Direct Link Access:{personId}");

            foundPerson = await AppData.API.GetPerson(personId);

            return foundPerson;


            //LOCAL FUNCTION

            async Task<Person> GetFromPersonList(string personId)
            {
                //try to get from person's own user list
                var personList = await AppData.API.GetPersonList();
                var personFromId = personList.Where(p => p.Id == personId);

                //will return Empty person if none found
                return personFromId.FirstOrDefault(Person.Empty);
            }
        }


        public static async Task DeleteSavedChart(string chartId, IJSRuntime jsRuntime)
        {
            var chartIdXml = new XElement("ChartId", chartId);
            var result = await ServerManager.WriteToServerXmlReply(AppData.URL.DeleteChartApi, chartIdXml);

            //check result, display error if needed
            if (!result.IsPass)
            {
                WebLogger.Error($"BLZ:DeleteSavedChart() Fail:\n{result.Payload}");
                await jsRuntime.ShowAlert("error", AlertText.DeleteChartFail, true);
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
            var resultsRaw = await ServerManager.WriteToServerXmlReply(AppData.URL.GetEventsApi, root);


            //parse raw results
            List<Event> resultsParsed = Event.FromXml(resultsRaw);


            //send to caller
            return resultsParsed;
        }

        public static async Task<List<ChartName>> GetSavedChartNameList()
        {
            //get name list of all charts
            //note: API will return readable name & hash for finding the chart later
            var result = await ServerManager.ReadFromServerXmlReply(AppData.URL.GetSavedEventsChartIdList, AppData.JsRuntime);

            if (result.IsPass)
            {
                var rootXml = result.Payload;

                var chartIdXmlList = rootXml.Elements().ToList();

                //parse each xml, then return
                var returnList = new List<ChartName>();
                foreach (var chartXml in chartIdXmlList)
                {
                    returnList.Add(ChartName.FromXml(chartXml));
                }

                return returnList;
            }
            else
            {
                //raise alarm if fail
                //todo better logging
                throw new Exception("BLZ:GetSavedChartNameList FAIL");
            }

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
            WebLogger.Error("Blazor Error Box Shown");
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
                WebLogger.Error(e, "Error from WebsiteTools.Try()");
#endif

                //note exception will not go past this point,
                //even calling throw will do nothing
                //throw;
            }
        }

        /// <summary>
        /// Gets XML file from any URL and parses it into xelement list
        /// </summary>
        public static async Task<List<XElement>> GetXmlFile(string url)
        {

            //load xml event data files before hand to be used quickly later for search
            //get main horoscope prediction file (located in wwwroot)
            var fileStream = await AppData.HttpClient.GetStreamAsync(url);

            //parse raw file to xml doc
            var document = XDocument.Load(fileStream);

            //get all records in document
            return document.Root.Elements().ToList();
        }

        /// <summary>
        /// Tries to get visitor ID from browser else makes new Visitor ID
        /// also update is new visitor flag
        /// </summary>
        public static async Task<string> TryGetVisitorId(IJSRuntime jsRuntime)
        {
            //find out if new visitor just arriving or old one browsing
            var visitorId = await jsRuntime.GetProperty("VisitorId") ; //local storage
            AppData.IsNewVisitor = string.IsNullOrEmpty(visitorId);

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
            var result = await ServerManager.WriteToServerXmlReply(AppData.URL.GetPersonIdFromSavedChartId, chartIdXml);
            var personId = result.Payload.Value;//xml named person id
            var selectedPerson = await GetPersonById(personId, jsRuntime);

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

        public static async Task<MatchReport> GetCompatibilityReport(string maleId, string femaleId)
        {
            var male = await WebsiteTools.GetPersonById(maleId, AppData.JsRuntime);
            var female = await WebsiteTools.GetPersonById(femaleId, AppData.JsRuntime);

            //if male & female profile found, make report and return caller
            var notEmpty = !Person.Empty.Equals(male) && !Person.Empty.Equals(female);
            if (notEmpty)
            {
                return MatchCalculator.GetNewMatchReport(male, female, "101");
            }
            else
            {
                throw new Exception(AlertText.PersonProfileNoExist);
            }
        }


       

    }
}
