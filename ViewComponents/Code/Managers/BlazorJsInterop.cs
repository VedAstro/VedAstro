using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Xml.Linq;


namespace Website
{

    /// <summary>
    /// Methods that connect blazor & js
    /// Wrapper for JS methods
    /// </summary>
    public static class BlazorJsInterop
    {


        //▄▀█ █░░ █▀▀ █▀█ ▀█▀
        //█▀█ █▄▄ ██▄ █▀▄ ░█░
        //FUNCTIONS CALLING SWEET ALERT JS LIB

        /// <summary>
        /// Shows alerts on page using SweetAlert js lib 
        /// this call is equivalent to
        /// Note: create alter data as anonymous type exactly like js version
        /// 
        /// Swal.fire({
        /// title: 'Error!',
        /// text: 'Do you want to continue',
        /// icon: 'error',
        /// confirmButtonText: 'Cool'
        /// })
        /// 
        /// </summary>
        public static async Task ShowAlert(this IJSRuntime jsRuntime, object alertData)
        {
            try
            {
                //log this, don't await to reduce lag
                //WebLogger.Alert(alertData);

                await jsRuntime.InvokeVoidAsync(JS.Swal_fire, alertData);
            }
            //above code will fail when called during app start, because haven't load lib
            //as such catch failure and silently ignore
            catch (Exception)
            {
                Console.WriteLine($"BLZ: ShowAlert Not Yet Load Lib Silent Fail!");
            }

        }

        /// <summary>
        /// Closes any currently showing SweetAlert
        /// </summary>
        public static void HideAlert(this IJSRuntime jsRuntime)
        {
            //log this, don't await to reduce lag
            //WebLogger.Data("Alert Close");

            jsRuntime.InvokeVoidAsync(JS.Swal_close);
        }

        /// <summary>
        /// Shows alert with return data for alerts with confirm button
        /// will return SweetAlertResult json object
        /// </summary>
        public static async Task<JsonElement> ShowAlertResult(this IJSRuntime jsRuntime, object alertData)
        {
            //log this, don't await to reduce lag
            //WebLogger.Alert(alertData);

            return await jsRuntime.InvokeAsync<JsonElement>(JS.Swal_fire, alertData);
        }

        /// <summary>
        /// Shows alert using sweet alert js
        /// </summary>
        /// <param name="timer">milliseconds to auto close alert, if 0 then won't close which is default (optional)</param>
        /// <param name="useHtml">If true title can be HTML, default is false (optional)</param>
        public static async Task GoBackRememberMePageOrBackHistory(this IJSRuntime jsRuntime)
        {
            //get the previously saved page before login, if any else default to home
            var previousPage = await jsRuntime.GetProperty("PreviousPage");

            //if not specified
            if (string.IsNullOrEmpty(previousPage))
            {
                jsRuntime?.GoBack();
            }
            //page has been specified time to go
            else
            {
                AppData.Go(previousPage, forceReload: true);
            }

        }


        /// <summary>
        /// Shows alert using sweet alert js
        /// </summary>
        /// <param name="timer">milliseconds to auto close alert, if 0 then won't close which is default (optional)</param>
        /// <param name="useHtml">If true title can be HTML, default is false (optional)</param>
        public static async Task ShowAlert(this IJSRuntime jsRuntime, string icon, string title, bool showConfirmButton, int timer = 0, bool useHtml = false)
        {
            object alertData;

            if (useHtml)
            {
                alertData = new
                {
                    icon = icon,
                    html = title,
                    showConfirmButton = showConfirmButton,
                    timer = timer
                };
            }
            else
            {
                alertData = new
                {
                    icon = icon,
                    title = title,
                    showConfirmButton = showConfirmButton,
                    timer = timer
                };
            }


            await jsRuntime.ShowAlert(alertData);
        }

        /// <summary>
        /// Uses icon from Iconify plugin
        /// </summary>
        public static async Task ShowAlertCustomIcon(this IJSRuntime jsRuntime, string iconName, string title, bool showConfirmButton, int timer = 0, bool useHtml = false)
        {
            object alertData;

            if (useHtml)
            {
                alertData = new
                {
                    iconHtml = $"<span class=\"iconify\" data-icon=\"{iconName}\" data-inline=\"false\"></span>",
                    html = title,
                    showConfirmButton = showConfirmButton,
                    timer = timer
                };
            }
            else
            {
                alertData = new
                {
                    iconHtml = $"<span class=\"iconify\" data-icon=\"{iconName}\" data-inline=\"false\"></span>",
                    title = title,
                    showConfirmButton = showConfirmButton,
                    timer = timer
                };
            }


            await jsRuntime.ShowAlert(alertData);
        }

        /// <summary>
        /// Shows alert using sweet alert js
        /// will show okay button, no timeout
        /// </summary>
        public static async Task ShowAlert(this IJSRuntime jsRuntime, string icon, string title, string descriptionText)
        {
            //log this, don't await to reduce lag
            //WebLogger.Data($"Alert : {title} : {descriptionText}");

            //call SweetAlert lib directly via constructor
            await jsRuntime.InvokeVoidAsync(JS.Swal_fire, title, descriptionText, icon);
        }

        /// <summary>
        /// Shows alert using sweet alert js
        /// will show okay button, no timeout
        /// Uses icon from Iconify plugin
        /// </summary>
        public static async Task ShowAlertCustomIcon(this IJSRuntime jsRuntime, string iconName, string title, string descriptionText)
        {
            //log this, don't await to reduce lag
            //WebLogger.Data($"Alert : {title} : {descriptionText}");

            var alertData = new
            {
                iconHtml = $"<span class=\"iconify\" data-icon=\"{iconName}\" data-inline=\"false\"></span>",
                title = title,
                text = descriptionText,
                showConfirmButton = true,
            };

            //call SweetAlert lib directly via constructor
            await jsRuntime.ShowAlert(alertData);
        }


        /// <summary>
        /// Shows a dialog box with request for email, done via SweetAlert JS
        /// </summary>
        public static async Task<string> PopupTextInput(this IJSRuntime jsRuntime, string message, string inputType = "email", string inputPlaceholder = "Enter your email address") => await jsRuntime.InvokeAsync<string>(JS.PopupTextInput, message, inputType, inputPlaceholder);

        /// <summary>
        /// Will inject an option with the data given the data
        /// </summary>
        public static async Task AddOptionToSelectDropdown(this IJSRuntime jsRuntime, object elementRef, string visibleText, string selectValue)
        {
            await jsRuntime.InvokeVoidAsync(JS.addOptionToSelectDropdown, elementRef, visibleText, selectValue); //select value is the hidden value
        }
        /// <summary>
        /// Shows loading box with auto progress bar using sweetalert
        /// note: hide using HideAlert()
        /// set 0 delay to skip auto wait
        /// </summary>
        public static async Task ShowLoading(this IJSRuntime jsRuntime, int delayMs = 300)
        {
            //note: - id needed for tooltip js, init from app js
            //      - div need to wrap image for nice formatting
            //      - all code placed here for easy of maintainer and it works!
            var clear = "function hi(){$('#LoadingBoxStatus').text('');};hi()";
            var showReload = "function hi(){$('#LoadingBoxStatus').text('reload');};hi()";
            var newTab = "function hi(){$('#LoadingBoxStatus').text('new tab');};hi()";
            var styleText = "font-size: 15px; align-self: center;color: #a3a3a3; width:64px;white-space: nowrap;";
            var loadingBoxOptions = $@"
                <div class=""vstack"">
                    <div>
                        <img src=""images/loading-animation-progress-transparent.gif"">
                    </div>
                    <div class=""my-1 d-flex justify-content-center gap-3"">
                        <button onmouseout=""{clear}"" onmouseover=""{showReload}"" onclick=""window.location.href=window.location.href"" style=""width: fit-content;"" class=""btn-sm iconOnlyButton btn-outline-danger btn"" ><svg xmlns=""http://www.w3.org/2000/svg"" xmlns:xlink=""http://www.w3.org/1999/xlink"" aria-hidden=""true"" role=""img"" class=""iconify iconify--ooui"" width=""25"" height=""25"" preserveAspectRatio=""xMidYMid meet"" viewBox=""0 0 20 20"" data-icon=""ooui:reload"" data-width=""25""><path fill=""currentColor"" d=""M15.65 4.35A8 8 0 1 0 17.4 13h-2.22a6 6 0 1 1-1-7.22L11 9h7V2z""></path></svg></button><!--!-->
                        <span id=""LoadingBoxStatus"" style=""{styleText}"" class=""px-2""></span>                
                        <button  onmouseout=""{clear}"" onmouseover=""{newTab}"" onclick=""window.open('{AppData.URL.WebUrl}')"" style=""width: fit-content;"" class=""btn-sm iconOnlyButton btn-outline-primary btn"" ><svg xmlns=""http://www.w3.org/2000/svg"" xmlns:xlink=""http://www.w3.org/1999/xlink"" aria-hidden=""true"" role=""img"" class=""iconify iconify--iconoir"" width=""25"" height=""25"" preserveAspectRatio=""xMidYMid meet"" viewBox=""0 0 24 24"" data-icon=""iconoir:new-tab"" data-width=""25""><g fill=""none"" stroke=""currentColor"" stroke-width=""1.5""><path d=""M2 19V5a2 2 0 0 1 2-2h16a2 2 0 0 1 2 2v14a2 2 0 0 1-2 2H4a2 2 0 0 1-2-2Z""></path><path stroke-linecap=""round"" stroke-linejoin=""round"" d=""M2 7h20M9 14h3m3 0h-3m0 0v-3m0 3v3""></path></g></svg></button>
                    </div>
                </div>";

            var alertData = new
            {
                showConfirmButton = false,
                width = "280px",
                padding = "1px",
                allowOutsideClick = false,
                allowEscapeKey = false,
                stopKeydownPropagation = true,
                keydownListenerCapture = true,
                html = loadingBoxOptions
            };

            //log it
            //await WebLogger.Data("Show Loading Box");

            //don't wait here
            jsRuntime.ShowAlert(alertData);

            //needed time to pop
            //skip if set 0
            if (delayMs > 0) { await Task.Delay(delayMs); }

            //let others know loading box has popped
            AppData.IsShowLoading = true;
        }


        public static void HideLoading(this IJSRuntime jsRuntime)
        {
            //log it
            //WebLogger.Data("Hide Loading Box");

            jsRuntime.HideAlert();

            //let others know loading box is closed
            AppData.IsShowLoading = false;
        }


        /// <summary>
        /// uses basic JS plays sound stored in wwwroot via element in index.html
        /// </summary>
        /// <param name="jsRuntime"></param>
        public static void PlayDoneSound(this IJSRuntime jsRuntime)
        {
            var notifySoundFile = "/sound/positive-notification.mp3";

            jsRuntime.InvokeVoidAsync(JS.PlaySoundFromUrl, Path.Combine(AppData.URL.WebUrl, notifySoundFile));
        }


        //TIPPY TOOLTIP LIBRARY

        /// <summary>
        /// Uses tooltip lib to attach tooltip to an element via selector or blazor reference  
        /// </summary>
        public static async Task Tippy(this IJSRuntime jsRuntime, object elementRef, object tooltipData)
        {
            try
            {
                await jsRuntime.InvokeVoidAsync(JS.tippy, elementRef, tooltipData);
            }
            catch (Exception e)
            {
                //not important ignore if fail
                Console.WriteLine("BLZ: Tippy Silent Fail!");
            }
        }

        public static async Task Tippy(this IJSRuntime jsRuntime, string cssSelector, object tooltipData)
        {
            try
            {
                await jsRuntime.InvokeVoidAsync(JS.tippy, cssSelector, tooltipData);
            }
            catch (Exception)
            {
                //not important ignore if fail
                Console.WriteLine("BLZ: Tippy Silent Fail!");
            }
        }


        //ACCESS BROWSERS LOCAL STORAGE

        /// <summary>
        /// Gets data stored in browser storage
        /// </summary>
        public static async Task<string> GetProperty(this IJSRuntime jsRuntime, string propName)
        {
            var value = await jsRuntime.InvokeAsync<string>(JS.getProperty, propName);

            //#if DEBUG
            //            Console.WriteLine($"GET Prop : {propName} = {value}");
            //#endif

            return value;
        }

        /// <summary>
        /// Set data into browser local storage
        /// </summary>
        public static async Task SetProperty(this IJSRuntime jsRuntime, string propName, string value)
        {
            //#if DEBUG
            //            Console.WriteLine($"SET Prop : {propName} = {value}");
            //#endif
            await jsRuntime.InvokeVoidAsync(JS.setProperty, propName, value);
        }

        public static async Task RemoveProperty(this IJSRuntime jsRuntime, string propName) => await jsRuntime.InvokeVoidAsync(JS.removeProperty, propName);

        /// <summary>
        /// Calls given handler when localstorage data changes
        /// </summary>
        public static async Task RemoveProperty<T>(this IJSRuntime jsRuntime, T instance, string handlerName) where T : class
            => await jsRuntime.InvokeVoidAsync(JS.watchProperty, DotNetObjectReference.Create(instance), handlerName);





        //█▀▀ █▀▀ █▄░█ █▀▀ █▀█ ▄▀█ █░░   █▀▀ █░█ █▄░█ █▀▀ ▀█▀ █ █▀█ █▄░█ █▀
        //█▄█ ██▄ █░▀█ ██▄ █▀▄ █▀█ █▄▄   █▀░ █▄█ █░▀█ █▄▄ ░█░ █ █▄█ █░▀█ ▄█

        /// <summary>
        /// Uses jQuery to show element via blazor reference
        /// </summary>
        public static async Task Show(this IJSRuntime jsRuntime, ElementReference element) => await jsRuntime.InvokeVoidAsync(JS.showWrapper, element);

        /// <summary>
        /// Uses jQuery to show element via blazor reference
        /// another version with direct to element ref
        /// uses JS runtime attached to AppData
        /// </summary>
        public static async Task Show(this ElementReference element) => await AppData.JsRuntime.InvokeVoidAsync(JS.showWrapper, element);

        /// <summary>
        /// toggles the display property of an element, show / hide
        /// uses jquery toggle method
        /// ID without #
        /// </summary>
        public static async Task ToggleDisplay(this IJSRuntime jsRuntime, string elementID)
        {
            await jsRuntime.InvokeVoidAsync("eval", $"$('#{elementID}').toggle()");
        }

        /// <summary>
        /// Jquery wrapper for .slideToggle("slow")
        /// </summary>
        public static async Task SlideToggle(this IJSRuntime jsRuntime, string elementID)
        {
            await jsRuntime.InvokeVoidAsync("eval", $"$('#{elementID}').slideToggle(\"slow\")");
        }

        /// <summary>
        /// Improved Jquery wrapper for .slideToggle("slow")
        /// </summary>
        public static async Task SmoothSlideToggle(this IJSRuntime jsRuntime, string elementID)
        {
            //make scroll movement to place
            await jsRuntime.InvokeVoidAsync(JS.smoothSlideToggle, "#" + elementID);
        }


        /// <summary>
        /// Jquery wrapper for .slideDown("slow")
        /// </summary>
        public static async Task SlideDown(this IJSRuntime jsRuntime, string elementID)
        {
            await jsRuntime.InvokeVoidAsync("eval", $"$('#{elementID}').slideDown(\"slow\")");
        }

        /// <summary>
        /// Jquery wrapper for highlight div animate
        /// </summary>
        public static async Task AnimateHighlightElement(this IJSRuntime jsRuntime, string elementID)
        {
            //make scroll movement to place
            await jsRuntime.InvokeVoidAsync(JS.animateHighlightElement, "#" + elementID);
        }

        /// <summary>
        /// Jquery wrapper for highlight div animate
        /// </summary>
        public static async Task AnimateHighlightElement(this IJSRuntime jsRuntime, ElementReference element)
        {
            //make scroll movement to place
            await jsRuntime.InvokeVoidAsync(JS.animateHighlightElement, element);
        }

        /// <summary>
        /// Uses jQuery to show element via selector (#ID,.class)
        /// </summary>
        public static async Task Show(this IJSRuntime jsRuntime, string elementSelector) => await jsRuntime.InvokeVoidAsync(JS.showWrapper, elementSelector);

        /// <summary>
        /// Given list of IDs will show them all
        /// </summary>
        public static async Task ShowList(this IJSRuntime jsRuntime, List<string> elementSelector) => await jsRuntime.InvokeVoidAsync(JS.showListWrapper, elementSelector);

        /// <summary>
        /// to be used like this @(() => _jsRuntime.FunFeaturePopUp("Custom Ayanamsa"))
        /// </summary>
        public static async Task FunFeaturePopUp(this IJSRuntime jsRuntime, string featureName)
        {
            //log this
            WebLogger.Click($"Fund : {featureName}");

            await jsRuntime.InvokeVoidAsync(JS.FunFeaturePopUp);
        }


        /// <summary>
        /// Uses jQuery to hide element via blazor reference
        /// </summary>
        public static async Task Hide(this IJSRuntime jsRuntime, ElementReference element) => await jsRuntime.InvokeVoidAsync(JS.hideWrapper, element);

        /// <summary>
        /// Uses jQuery to hide element via selector (#ID,.class)
        /// </summary>
        public static async Task Hide(this IJSRuntime jsRuntime, string elementSelector) => await jsRuntime.InvokeVoidAsync(JS.hideWrapper, elementSelector);

        /// <summary>
        /// Injects html/svg into an element
        /// </summary>
        public static async Task InjectIntoElement(this IJSRuntime jsRuntime, ElementReference elmReference, string value) => await jsRuntime.InvokeVoidAsync(JS.InjectIntoElement, elmReference, value);

        /// <summary>
        /// Uses jQuery to attach a function by name to a HTML element event
        /// </summary>
        public static async Task AddEventListener(this IJSRuntime jsRuntime, ElementReference element, string eventName, string eventHandlerName) => await jsRuntime.InvokeVoidAsync(JS.addEventListenerWrapper, element, eventName, eventHandlerName);

        public static async Task AddEventListener(this IJSRuntime jsRuntime, string jquerySelector, string eventName, string eventHandlerName) => await jsRuntime.InvokeVoidAsync(JS.addEventListenerByClass, jquerySelector, eventName, eventHandlerName);

        /// <summary>
        /// Calls the js function specified and returns function data JSON to XML
        /// Note: only for JS functions that confirm return JSON data
        /// RootElementNames not need if original json already has root object
        /// Will throw error if json doesn't have root & no root element name is specified
        /// </summary>
        public static async Task<XElement> InvokeAsyncJson(this IJSRuntime jsRuntime, string jsFunctionName, string rootElementName = "Root")
        {
            //data coming in though passed as JsonNode, it can be JsonArray or JsonObject
            var rawJson = await jsRuntime.InvokeAsync<JsonNode>(jsFunctionName);
            XElement finalXml = null;

            //parse differently based on array or object
            if (rawJson is JsonObject jsonObject)
            {
                //convert json object to string
                var jsonString = jsonObject.ToString();

                //only use root element name if specified
                var rawXml = JsonConvert.DeserializeXmlNode(jsonString, rootElementName);

                finalXml = XElement.Parse(rawXml.InnerXml);

            }
            //if array place one by one into final xml
            else if (rawJson is JsonArray jsonArray)
            {
                finalXml = new XElement(rootElementName);
                foreach (var tableData in jsonArray)
                {
                    var xmlString = (JsonConvert.DeserializeXmlNode(tableData.ToJsonString(), "LifeEvent"))?.InnerXml;
                    finalXml.Add(XElement.Parse(xmlString));
                }
            }

            if (finalXml == null) { throw new Exception("Json Type Not Specified!"); }

            return finalXml;
        }

        /// <summary>
        /// Scrolls to a div on page given an ID without #
        /// </summary>
        public static async Task ScrollToDivById(this IJSRuntime jsRuntime, string predictionName)
        {
            //make scroll movement to place
            await jsRuntime.InvokeVoidAsync(JS.scrollToDiv, "#" + predictionName);
        }

        public static async Task AddClass(this IJSRuntime jsRuntime, ElementReference element, string classNames) => await jsRuntime.InvokeVoidAsync(JS.addClassWrapper, element, classNames);

        public static async Task RemoveClass(this IJSRuntime jsRuntime, ElementReference element, string classNames) => await jsRuntime.InvokeVoidAsync(JS.removeClassWrapper, element, classNames);

        public static async Task ToggleClass(this IJSRuntime jsRuntime, ElementReference element, string classNames) => await jsRuntime.InvokeVoidAsync(JS.toggleClassWrapper, element, classNames);

        public static async Task ToggleClass(this IJSRuntime jsRuntime, string jquerySelector, string classNames) => await jsRuntime.InvokeVoidAsync(JS.toggleClassWrapper, jquerySelector, classNames);

        public static async Task<double> ElementWidth(this IJSRuntime jsRuntime, ElementReference element) => await jsRuntime.InvokeAsync<double>(JS.getElementWidth, element);

        public static async Task<double> AddWidthToEveryChild(this IJSRuntime jsRuntime, ElementReference element, double valueToAdd) => await jsRuntime.InvokeAsync<double>(JS.addWidthToEveryChild, element, valueToAdd);

        public static async Task<T> GetProp<T>(this IJSRuntime jsRuntime, ElementReference element, string propName) => await jsRuntime.InvokeAsync<T>(JS.getPropWrapper, element, propName);

        /// <summary>
        /// wrapper for JQuery .prop() 
        /// </summary>
        public static async Task SetProp(this IJSRuntime jsRuntime, ElementReference element, string propName, object propVal) => await jsRuntime.InvokeVoidAsync(JS.setPropWrapper, element, propName, propVal);

        /// <summary>
        /// wrapper for JQuery .prop() 
        /// </summary>
        public static async Task SetProp(this IJSRuntime jsRuntime, string element, string propName, object propVal) => await jsRuntime.InvokeVoidAsync(JS.setPropWrapper, element, propName, propVal);

        /// <summary>
        /// wrapper for JQuery .attr() 
        /// </summary>
        public static async Task SetAttr(this IJSRuntime jsRuntime, string element, string propName, object propVal) => await jsRuntime.InvokeVoidAsync(JS.setAttrWrapper, element, propName, propVal);

        /// <summary>
        /// wrapper for JQuery .css() 
        /// </summary>
        public static async Task SetCss(this IJSRuntime jsRuntime, string element, string propName, object propVal) => await jsRuntime.InvokeVoidAsync(JS.setCssWrapper, element, propName, propVal);

        public static void OpenNewTab(this IJSRuntime jsRuntime, string url)
        {
            WebLogger.Data($"NEW TAB TO: {url}");
            jsRuntime.InvokeVoidAsync(JS.open, url, "_blank");
        }

        /// <summary>
        /// Jquery .text()
        /// </summary>
        public static async Task<string> GetText(this IJSRuntime jsRuntime, ElementReference element) => await jsRuntime.InvokeAsync<string>(JS.getTextWrapper, element);

        /// <summary>
        /// wrapper for JQuery .text() get only
        /// </summary>
        public static async Task<string> GetText(this IJSRuntime jsRuntime, string jquerySelector) => await jsRuntime.InvokeAsync<string>(JS.getTextWrapper, jquerySelector);

        /// <summary>
        /// wrapper for JQuery .text() set only
        /// </summary>
        public static async void SetText(this IJSRuntime jsRuntime, string jquerySelector, string value) => await jsRuntime.InvokeVoidAsync(JS.setTextWrapper, jquerySelector, value);

        /// <summary>
        /// wrapper for JQuery .text() set only
        /// </summary>
        public static async void SetText(this IJSRuntime jsRuntime, ElementReference element, string value) => await jsRuntime.InvokeVoidAsync(JS.setTextWrapper, element, value);

        public static async Task<string> GetValue(this IJSRuntime jsRuntime, ElementReference element) => await jsRuntime.InvokeAsync<string>(JS.getValueWrapper, element);

        public static async Task<string> GetValue(this IJSRuntime jsRuntime, string jquerySelector) => await jsRuntime.InvokeAsync<string>(JS.getValueWrapper, jquerySelector);

        /// <summary>
        /// wrapper for JQuery .val() set only
        /// </summary>
        public static async Task SetValue(this IJSRuntime jsRuntime, object elementRef, object value) => await jsRuntime.InvokeVoidAsync(JS.setValueWrapper, elementRef, value);

        /// <summary>
        /// Load page to given url using JS, reloads Blazor app as well, good for error recovery
        /// </summary>
        public static async Task LoadPage(this IJSRuntime jsRuntime, string url) => await jsRuntime.InvokeVoidAsync(JS.window_location_assign, url);
        public static async Task ReloadPageToUrl(this IJSRuntime jsRuntime, string url) => await jsRuntime.InvokeVoidAsync(JS.window_location_assign, url);
        public static async Task ReloadPage(this IJSRuntime jsRuntime) => await jsRuntime.InvokeVoidAsync(JS.window_location_reload);


        /// <summary>
        /// Gets the previous page/origin url from JS
        /// </summary>
        public static async Task<string> GetOriginUrl(this IJSRuntime jsRuntime) => await jsRuntime.InvokeAsync<string>(JS.getOriginUrl);

        /// <summary>
        /// Gets current page's url
        /// </summary>
        public static async Task<string> GetCurrentUrl(this IJSRuntime jsRuntime) => await jsRuntime.InvokeAsync<string>(JS.getUrl);

        /// <summary>
        /// Equal to pressing Back button
        /// goes back to when things were simple
        /// </summary>
        public static async Task GoBack(this IJSRuntime jsRuntime)
        {
            await jsRuntime.InvokeVoidAsync(JS.history_back);
        }


        /// <summary>
        /// Load JS file programmatically
        /// </summary>
        public static async Task LoadJs(this IJSRuntime jsRuntime, string url)
        {
            await jsRuntime.InvokeVoidAsync(JS.loadJs, new { name = "JsInterop", url = url });
        }

        /// <summary>
        /// Load JS file programmatically
        /// </summary>
        public static async Task<IJSObjectReference> LoadJsBlazor(this IJSRuntime jsRuntime, string url) => await jsRuntime.InvokeAsync<IJSObjectReference>(JS.import, url);
    }

}
