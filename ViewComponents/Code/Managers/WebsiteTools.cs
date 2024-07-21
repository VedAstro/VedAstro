using System.Xml.Linq;
using VedAstro.Library;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Reflection;
using Newtonsoft.Json.Linq;
using System.Text.Json;
using Microsoft.CodeAnalysis;

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
        /// show box to get email and log for sending todo
        /// </summary>
        public static async Task OnClickSendToEmail(string pdfFileName, ElementReference elementToConvert)
        {
            //get email from user via js sweet alert lib
            var emailFromAlert = await AppData.JsRuntime.PopupTextInput("Send PDF to...");

            //calls special JS lib to convert html version of the chart to PDF
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

            foundPerson = await AppData.API.Person.GetPerson(personId);

            return foundPerson;


            //LOCAL FUNCTION

            async Task<Person> GetFromPersonList(string personId)
            {
                //try to get from person's own user list
                var personList = await AppData.API.Person.GetPersonList();
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
            var result = await Tools.ReadFromServerXmlReply(AppData.URL.GetSavedEventsChartIdList);

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
        /// Tries to get visitor ID from browser else makes new Visitor ID
        /// also update is new visitor flag
        /// </summary>
        public static async Task<string> TryGetVisitorId(IJSRuntime jsRuntime)
        {
            //find out if new visitor just arriving or old one browsing
            var visitorId = await jsRuntime.GetProperty("VisitorId"); //local storage
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
                return MatchReportFactory.GetNewMatchReport(male, female, "101");
            }
            else
            {
                throw new Exception(AlertText.PersonProfileNoExist);
            }
        }

        public static async Task ShareCurrentPageOnFacebook()
        {
            var currentUrl = await AppData.JsRuntime.GetCurrentUrl();
            await AppData.JsRuntime.InvokeVoidAsync(JS.shareDialogFacebook, currentUrl);
        }

        /// <summary>
        /// given a component instance will make it appear
        /// </summary>
        public static RenderFragment RenderContent(ComponentBase instance)
        {
            var fragmentField = GetPrivateField(instance.GetType(), "_renderFragment");

            var value = (RenderFragment)fragmentField.GetValue(instance);

            return value;
        }

        //https://stackoverflow.com/a/48551735/66988
        private static FieldInfo GetPrivateField(Type t, String name)
        {
            const BindingFlags bf = BindingFlags.Instance |
                                    BindingFlags.NonPublic |
                                    BindingFlags.DeclaredOnly;

            FieldInfo fi;
            while ((fi = t.GetField(name, bf)) == null && (t = t.BaseType) != null) ;

            return fi;
        }


        /// <summary>
        /// Adds a message to API server records from user
        /// </summary>
        public static async Task SendMailToAPI(string? title, string? description)
        {
            //package message data to be sent
            var textXml = new XElement("Title", title);
            var emailXml = new XElement("Text", description);
            var userIdXml = new XElement("UserId", AppData.CurrentUser?.Id);
            var visitorIdXml = new XElement("VisitorId", AppData.VisitorId);
            var messageXml = new XElement("Message", userIdXml, visitorIdXml, emailXml, Tools.TimeStampSystemXml, Tools.TimeStampServerXml, textXml);

            //send message to API server
            await ServerManager.WriteToServerXmlReply(AppData.URL.AddMessageApi, messageXml);
        }


        /// <summary>
        /// gets debug mode set in browser memory
        /// false = disabled, enabled = true
        /// defaults to false
        /// </summary>
        public static async Task<bool> IsLocalServerDebugMode()
        {
            var dataInBrowser = await AppData.JsRuntime.GetProperty("DebugMode"); //enabled/disabled
            var debugMode = string.IsNullOrEmpty(dataInBrowser) ? false : (dataInBrowser == "enabled" ? true : false);

            return debugMode;
        }

        public static async Task<string> GetDebugModeText()
        {
            var debugMode = await WebsiteTools.IsLocalServerDebugMode();
            var selectedDebugMode = debugMode ? "enabled" : "disabled";

            return selectedDebugMode;
        }



        public static void OnClickGotoGithubCode(OpenAPIMetadata metadata)
        {
            //if null, tell user to get act together
            if (metadata == null)
            {
                var msg = "You need to choose a calculator to view it's source code.";
                AppData.JsRuntime.ShowAlert("warning", $"Select a calculator first, {AlertText.RandomNoun()}!", msg);

                //end here
                return;
            }

            //get line number where code starts
            var lineNumber = metadata.LineNumber;

            //make the GitHub link
            var searchLink = $"https://github.com/VedAstro/VedAstro/blob/master/Library/Logic/Calculate/Calculate.cs#L{lineNumber}";

            //open link in new tab
            AppData.Go(searchLink, newTab: true);
        }


        public static async Task OnClickCopyPythonSnippet(OpenAPIMetadata? methodData, dynamic component = null)
        {
            //if null, tell user to get act together
            if (methodData == null)
            {
                var msg = "You need to choose a calculator to auto generate Python code.";
                await AppData.JsRuntime.ShowAlert("warning", $"Select a calculator first, {AlertText.RandomNoun()}!", msg);

                //end here
                return;
            }

            //only continue if passed input field validation
            if (component != null && !(await component.IsValidationPassed())) { return; }

            //generate the param field dynamically
            var paramNameList = GetParamNameList(methodData.Name);

            //declare the used variables
            var paramDeclaration = GetParamDeclaration(methodData.Name);

            var importDeclaration = "from vedastro import *  # install via pip";

            var pythonCode = $"{importDeclaration}\n\n" +
                                    $"{paramDeclaration}\n\n" +
                                    $"# run calculator to get result\n" +
                                    $"calcResult = Calculate.{methodData.Name}({paramNameList})\n\n" +
                                    $"# display results\n" +
                                    $"Tools.Print(calcResult)";

            //use js to transfer to clipboard
            await AppData.JsRuntime.InvokeVoidAsync(JS.CopyToClipboard, pythonCode);

            //let user know link copied
            await AppData.JsRuntime.ShowAlert("success", $"Code Copied!", "Remember to run <strong>pip install vedastro</strong>");

        }

        /// <summary>
        /// given a method name, get the names of the parameters to used in the method's signature
        /// </summary>
        public static string GetParamNameList(string methodName)
        {
            var methodInfo = typeof(Calculate).GetMethod(methodName);
            if (methodInfo != null)
            {
                var parameters = methodInfo.GetParameters();
                var parameterNames = parameters.Select(p => p.Name).ToArray();

                return string.Join(", ", parameterNames);
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// generate variable declaration string for python given a method's name
        /// </summary>
        public static string GetParamDeclaration(string methodName)
        {
            var final = "";

            //get all the data needed
            var methodInfo = typeof(Calculate).GetMethod(methodName);
            if (methodInfo != null)
            {
                var parameters = methodInfo.GetParameters();
                var parameterNames = parameters.Select(p => p.Name).ToArray();

                for (int index = 0; index < parameterNames.Length; index++)
                {
                    //NOTE: use type name instead of generics is because code is much easier and cleaner
                    var typeName = parameters[index].ParameterType.FullName;
                    final += GetTypeDemoDeclarationPython(parameterNames[index], typeName);

                    //add new line only between declarations
                    if (index != parameterNames.Length - 1) { final += "\n\n"; }
                }
            }


            return final;
        }

        public static string GetTypeDemoDeclarationPython(string variableName, string typeName)
        {
            var final = "";

            if (typeName == typeof(Time).FullName)
            {
                final =
                    $"""
					# set location
					geolocation = GeoLocation("Tokyo, Japan", 139.83, 35.65)

					# set time hh:mm dd/mm/yyyy zzz
					{variableName} = Time("23:40 31/12/2010 +08:00", geolocation)
					""";
            }

            if (typeName == typeof(PlanetName).FullName)
            {
                final =
                    $"""
					# set planet
					{variableName} = PlanetName.Sun
					""";
            }
            if (typeName == typeof(HouseName).FullName)
            {
                final =
                    $"""
					# set house (bhava)
					{variableName} = HouseName.House3
					""";
            }
            if (typeName == typeof(ZodiacName).FullName)
            {
                final =
                    $"""
					# set zodiac sign
					{variableName} = ZodiacName.Gemini
					""";
            }
            if (typeName == typeof(Person).FullName)
            {
                final =
                    $"""
					# set person
					{variableName} = Person("", "", "Juliet", Gender.Female, birth_time, "", List[LifeEvent.EventTag]())
					""";
            }

            return final;
        }


        /// <summary>
        /// Gets latest VedAstro main repo last committed build number
        /// can be used to check version
        /// </summary>
        public static async Task<int> GetLatestCommitNumber()
        {
            //get all tags
            var rawFromGitHub = await GetAllTagsAsync();

            //convert to list of versions (middle number)
            var allPublishedVersions = rawFromGitHub.Select(tagStr => ExtractMiddleNumber(tagStr));

            //get the highest number as latest tag from repo
            var latestCommitNumber = allPublishedVersions.Max();

            return latestCommitNumber ?? 0;


            static int? ExtractMiddleNumber(string input)
            {
                string[] parts = input.Split('-');
                if (parts.Length >= 2)
                {
                    if (int.TryParse(parts[1], out int number))
                    {
                        return number;
                    }
                    else
                    {
                        // handle the case where the middle part is not a valid integer
                        Console.WriteLine("The middle part of the string is not a valid integer.");
                        return null;
                    }
                }
                else
                {
                    // handle the case where the string does not contain a '-'
                    Console.WriteLine("The string does not contain a '-'.");
                    return null;
                }
            }
        }


        /// <summary>
        /// Gets all tags from repo as string in random order
        /// </summary>
        public static async Task<List<string>> GetAllTagsAsync()
        {
            string repoUrl = URL.GitHubRepo;
            var client = new HttpClient();
            string apiUrl = repoUrl.Replace("github.com", "api.github.com/repos") + "/tags";
            string jsonString = await client.GetStringAsync(apiUrl);
            JArray json = JArray.Parse(jsonString);
            var tags = json?.Select(j => j["name"]?.ToString()).ToList();
            return tags ?? new List<string> { "0000-0000-stable" }; //mean no tags found
        }




        /// <summary>
        /// only give response if header says ok
        /// defaults to get, but can be changed to any
        /// </summary>
        public static async Task<JToken?> ReadOnlyIfPassJSJson(string receiverAddress, IJSRuntime jsRuntime, string callMethod = "GET")
        {
            //this call will take you to NetworkThread.js
            var rawPayloadStr = await jsRuntime.InvokeAsync<JsonElement>("Interop.ReadOnlyIfPassJson", receiverAddress, callMethod);
            var status = rawPayloadStr.GetProperty("Status").GetString();
            var payload = rawPayloadStr.GetProperty("Payload").GetString() ?? "{}";


            var isPass = status == "Pass";
            //var payload = rawPayload["Payload"]?.Value<JToken>() ?? new JObject();
            if (isPass)
            {
                var rawPayload = JToken.Parse(payload);

                //return the raw reply to caller
                return rawPayload;

            }

            //if anything but pass, don't look inside just say nothing
            return null;

        }


        /// <summary>
        /// give null data to send to get auto use GET instead of POST done by JS side
        /// TODO MARKED FOR DELETION
        /// </summary>
        public static async Task<string?> ReadOnlyIfPassJSString(string receiverUrl, string? dataToSend, IJSRuntime jsRuntime)
        {
            //holds control till get
            //more efficient than passing control back and form Blazor and JS
            var rawPayloadStr = await jsRuntime.InvokeAsync<string?>("Interop.ReadOnlyIfPassString", receiverUrl, dataToSend);

            return rawPayloadStr;
        }




        public static async Task<JToken> WriteServer(HttpMethod method, string receiverAddress, JToken? payloadJson = null)
        {

            //prepare the data to be sent
            var httpRequestMessage = new HttpRequestMessage(method, receiverAddress);

            //tell sender to wait for complete reply before exiting
            var waitForContent = HttpCompletionOption.ResponseContentRead;

            //add in payload if specified
            if (payloadJson != null) { httpRequestMessage.Content = VedAstro.Library.Tools.JsontoHttpContent(payloadJson); }

            //send the data on its way (wait forever no timeout)
            using var client = new HttpClient();
            client.Timeout = new TimeSpan(0, 0, 0, 0, Timeout.Infinite);

            //send the data on its way
            var response = await client.SendAsync(httpRequestMessage, waitForContent);

            //return the raw reply to caller
            var dataReturned = await response.Content.ReadAsStringAsync();

            //return data as JSON as expected from API 
            return JObject.Parse(dataReturned);
        }


        /// <summary>
        /// Send file will receive JSon list
        /// </summary>
        public static async Task<JToken> WriteServer(HttpMethod method, string receiverAddress, byte[] payloadFile)
        {
            //prepare the data to be sent
            var httpRequestMessage = new HttpRequestMessage(method, receiverAddress);
            //tell sender to wait for complete reply before exiting
            var waitForContent = HttpCompletionOption.ResponseContentRead;
            //add in payload if specified
            httpRequestMessage.Content = new ByteArrayContent(payloadFile);
            //send the data on its way (wait forever no timeout)
            using var client = new HttpClient();
            client.Timeout = new TimeSpan(0, 0, 0, 0, Timeout.Infinite);
            //send the data on its way
            var response = await client.SendAsync(httpRequestMessage, waitForContent);
            //return the raw reply to caller
            var dataReturned = await response.Content.ReadAsStringAsync();
            //return data as JSON as expected from API 
            return JObject.Parse(dataReturned);
        }


        /// <summary>
        /// tries to get location from local storage, if not found
        /// gets from API and saves a copy for future
        /// </summary>
        /// <returns></returns>
        public static async Task<GeoLocation> GetClientLocation()
        {
            //try get from browser storage
            var clientLocXml = await AppData.JsRuntime.GetProperty("ClientLocation");
            var isFound = clientLocXml != null;

            //if got cache, then just parse that and return (1 http call saved)
            GeoLocation parsedLocation;
            if (isFound)
            {
                var locationXml = XElement.Parse(clientLocXml);
                parsedLocation = GeoLocation.FromXml(locationXml);
            }
            //no cache, call Google API with IP
            else
            {
                parsedLocation = await GeoLocation.FromIpAddress();
                //save for future use
                await AppData.JsRuntime.SetProperty("ClientLocation", parsedLocation.ToXml().ToString());
            }

            Console.WriteLine($"Client Location:{parsedLocation.Name()}");

            return parsedLocation;
        }
    }
}
