using System.Text.Json;
using System.Text.Json.Nodes;
using System.Xml.Linq;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Newtonsoft.Json;


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
                WebsiteLogManager.LogAlert(jsRuntime, alertData);

                await jsRuntime.InvokeVoidAsync("Swal.fire", alertData);
            }
            //above code will fail when called during app start, because haven't load lib
            //as such catch failure and silently ignore
            catch (Exception e)
            {
                Console.WriteLine($"BLZ: ShowAlert Not Yet Load Lib Silent Fail!");
            }

        }

        /// <summary>
        /// Closes any currently showing SweetAlert
        /// </summary>
        public static void HideAlert(this IJSRuntime jsRuntime)
        {
            //todo disable for now because over logging
            //log this, don't await to reduce lag
            //WebsiteLogManager.LogData(jsRuntime, "Alert Close");

            jsRuntime.InvokeVoidAsync("Swal.close");
        }

        /// <summary>
        /// Shows alert with return data for alerts with confirm button
        /// will return SweetAlertResult json object
        /// </summary>
        public static async Task<JsonElement> ShowAlertResult(this IJSRuntime jsRuntime, object alertData)
        {
            //log this, don't await to reduce lag
            WebsiteLogManager.LogAlert(jsRuntime, alertData);

            return await jsRuntime.InvokeAsync<JsonElement>("Swal.fire", alertData);
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
        /// Shows leave email alert box and returns the email as string
        /// note: uses sweet alert js
        /// </summary>
        public static async Task<string> ShowLeaveEmailAlert(this IJSRuntime jsRuntime) => await jsRuntime.InvokeAsync<string>("ShowLeaveEmailAlert");

        /// <summary>
        /// Shows loading box with auto progress bar using sweetalert
        /// note: hide using HideAlert()
        /// set 0 delay to skip auto wait
        /// </summary>
        public static async Task ShowLoading(this IJSRuntime jsRuntime, int delayMs = 300)
        {

            var alertData = new
            {
                showConfirmButton = false,
                width = "280px",
                padding = "1px",
                allowOutsideClick = false,
                allowEscapeKey = false,
                stopKeydownPropagation = true,
                keydownListenerCapture = true,
                html = "<img src=\"images/loading-animation-progress-transparent.gif\" />"

            };

            //don't wait here
            jsRuntime.ShowAlert(alertData);

            //needed time to pop
            //skip if set 0
            if (delayMs > 0) { await Task.Delay(delayMs); }
        }
        
        public static void HideLoading(this IJSRuntime jsRuntime) => jsRuntime.HideAlert();



        //TIPPY TOOLTIP LIBRARY

        /// <summary>
        /// Uses tooltip lib to attach tooltip to an element via selector or blazor reference  
        /// </summary>
        public static async Task Tippy(this IJSRuntime jsRuntime, object elementRef, object tooltipData)
        {
            try
            {
                await jsRuntime.InvokeVoidAsync("tippy", elementRef, tooltipData);
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
                await jsRuntime.InvokeVoidAsync("tippy", cssSelector, tooltipData);
            }
            catch (Exception e)
            {
                //not important ignore if fail
                Console.WriteLine("BLZ: Tippy Silent Fail!");
            }
        }


        //ACCESS BROWSERS LOCAL STORAGE

        public static async Task<string> GetProperty(this IJSRuntime jsRuntime, string propName) => await jsRuntime.InvokeAsync<string>("getProperty", propName);
        /// <summary>
        /// Set data into browser local storage
        /// </summary>
        public static async Task SetProperty(this IJSRuntime jsRuntime, string propName, string value) => await jsRuntime.InvokeVoidAsync("setProperty", propName, value);
        public static async Task RemoveProperty(this IJSRuntime jsRuntime, string propName) => await jsRuntime.InvokeVoidAsync("removeProperty", propName);
        /// <summary>
        /// Calls given handler when localstorage data changes
        /// </summary>
        public static async Task RemoveProperty<T>(this IJSRuntime jsRuntime, T instance, string handlerName) where T : class
            => await jsRuntime.InvokeVoidAsync("watchProperty", DotNetObjectReference.Create(instance), handlerName);





        //█▀▀ █▀▀ █▄░█ █▀▀ █▀█ ▄▀█ █░░   █▀▀ █░█ █▄░█ █▀▀ ▀█▀ █ █▀█ █▄░█ █▀
        //█▄█ ██▄ █░▀█ ██▄ █▀▄ █▀█ █▄▄   █▀░ █▄█ █░▀█ █▄▄ ░█░ █ █▄█ █░▀█ ▄█

        /// <summary>
        /// Uses jQuery to show element via blazor reference
        /// </summary>
        public static async Task Show(this IJSRuntime jsRuntime, ElementReference element) => await jsRuntime.InvokeVoidAsync("showWrapper", element);
        /// <summary>
        /// Uses jQuery to show element via selector (#ID,.class)
        /// </summary>
        public static async Task Show(this IJSRuntime jsRuntime, string elementSelector) => await jsRuntime.InvokeVoidAsync("showWrapper", elementSelector);

        /// <summary>
        /// Uses jQuery to hide element via blazor reference
        /// </summary>
        public static async Task Hide(this IJSRuntime jsRuntime, ElementReference element) => await jsRuntime.InvokeVoidAsync("hideWrapper", element);

        /// <summary>
        /// Uses jQuery to hide element via selector (#ID,.class)
        /// </summary>
        public static async Task Hide(this IJSRuntime jsRuntime, string elementSelector) => await jsRuntime.InvokeVoidAsync("hideWrapper", elementSelector);

        /// <summary>
        /// Injects html/svg into an element
        /// </summary>
        public static async Task InjectIntoElement(this IJSRuntime jsRuntime, ElementReference _dasaViewBox, string value) => await jsRuntime.InvokeVoidAsync("InjectIntoElement", _dasaViewBox, value);

        /// <summary>
        /// Uses jQuery to attach a function by name to a HTML element event
        /// </summary>
        public static async Task AddEventListener(this IJSRuntime jsRuntime, ElementReference element, string eventName, string eventHandlerName) => await jsRuntime.InvokeVoidAsync("addEventListenerWrapper", element, eventName, eventHandlerName);

        public static async Task AddEventListener(this IJSRuntime jsRuntime, string jquerySelector, string eventName, string eventHandlerName) => await jsRuntime.InvokeVoidAsync("addEventListenerByClass", jquerySelector, eventName, eventHandlerName);

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

        public static async Task AddClass(this IJSRuntime jsRuntime, ElementReference element, string classNames) => await jsRuntime.InvokeVoidAsync("addClassWrapper", element, classNames);

        public static async Task RemoveClass(this IJSRuntime jsRuntime, ElementReference element, string classNames) => await jsRuntime.InvokeVoidAsync("removeClassWrapper", element, classNames);

        public static async Task ToggleClass(this IJSRuntime jsRuntime, ElementReference element, string classNames) => await jsRuntime.InvokeVoidAsync("toggleClassWrapper", element, classNames);

        public static async Task ToggleClass(this IJSRuntime jsRuntime, string jquerySelector, string classNames) => await jsRuntime.InvokeVoidAsync("toggleClassWrapper", jquerySelector, classNames);

        public static async Task<double> ElementWidth(this IJSRuntime jsRuntime, ElementReference element) => await jsRuntime.InvokeAsync<double>("getElementWidth", element);

        public static async Task<double> AddWidthToEveryChild(this IJSRuntime jsRuntime, ElementReference element, double valueToAdd) => await jsRuntime.InvokeAsync<double>("addWidthToEveryChild", element, valueToAdd);

        public static async Task<T> GetProp<T>(this IJSRuntime jsRuntime, ElementReference element, string propName) => await jsRuntime.InvokeAsync<T>("getPropWrapper", element, propName);

        /// <summary>
        /// wrapper for JQuery .prop() 
        /// </summary>
        public static async Task SetProp(this IJSRuntime jsRuntime, ElementReference element, string propName, object propVal) => await jsRuntime.InvokeVoidAsync("setPropWrapper", element, propName, propVal);

        /// <summary>
        /// wrapper for JQuery .prop() 
        /// </summary>
        public static async Task SetProp(this IJSRuntime jsRuntime, string element, string propName, object propVal) => await jsRuntime.InvokeVoidAsync("setPropWrapper", element, propName, propVal);

        /// <summary>
        /// wrapper for JQuery .attr() 
        /// </summary>
        public static async Task SetAttr(this IJSRuntime jsRuntime, string element, string propName, object propVal) => await jsRuntime.InvokeVoidAsync("setAttrWrapper", element, propName, propVal);

        /// <summary>
        /// wrapper for JQuery .css() 
        /// </summary>
        public static async Task SetCss(this IJSRuntime jsRuntime, string element, string propName, object propVal) => await jsRuntime.InvokeVoidAsync("setCssWrapper", element, propName, propVal);

        public static void OpenNewTab(this IJSRuntime jsRuntime, string url) => jsRuntime.InvokeVoidAsync("open", url, "_blank");

        /// <summary>
        /// Jquery .text()
        /// </summary>
        public static async Task<string> GetText(this IJSRuntime jsRuntime, ElementReference element) => await jsRuntime.InvokeAsync<string>("getTextWrapper", element);
        public static async Task<string> GetText(this IJSRuntime jsRuntime, string jquerySelector) => await jsRuntime.InvokeAsync<string>("getTextWrapper", jquerySelector);
        public static async Task<string> GetValue(this IJSRuntime jsRuntime, ElementReference element) => await jsRuntime.InvokeAsync<string>("getValueWrapper", element);
        public static async Task<string> GetValue(this IJSRuntime jsRuntime, string jquerySelector) => await jsRuntime.InvokeAsync<string>("getValueWrapper", jquerySelector);
        public static async Task SetValue(this IJSRuntime jsRuntime, string jquerySelector, object value) => await jsRuntime.InvokeVoidAsync("setValueWrapper", jquerySelector, value);

        /// <summary>
        /// Load page to given url using JS, reloads Blazor app as well, good for error recovery
        /// </summary>
        public static async Task LoadPage(this IJSRuntime jsRuntime, string url) => await jsRuntime.InvokeVoidAsync("window.location.assign", url);

        /// <summary>
        /// Checks if browser is online
        /// </summary>
        public static async Task<bool> IsOnline(this IJSRuntime jsRuntime) => await jsRuntime.InvokeAsync<bool>("IsOnline");

        /// <summary>
        /// Raises exception if not online
        /// else lets control pass through silently
        /// Note: exception is designed to be caught by central error handler
        /// </summary>
        public static async Task CheckInternet(this IJSRuntime jsRuntime)
        {
            var isOnline = await jsRuntime.IsOnline();
            if (!isOnline) { throw new NoInternetError(); }
        }

        /// <summary>
        /// Gets the previous page/origin url from JS
        /// </summary>
        public static async Task<string> GetOriginUrl(this IJSRuntime jsRuntime) => await jsRuntime.InvokeAsync<string>("getOriginUrl");

        /// <summary>
        /// Equal to pressing Back button
        /// </summary>
        public static async Task GoBack(this IJSRuntime jsRuntime) => await jsRuntime.InvokeVoidAsync("history.back");

        /// <summary>
        /// Sets new page/tab title
        /// </summary>
        public static async Task SetTitle(this IJSRuntime jsRuntime, string newTitle) => await jsRuntime.InvokeVoidAsync("setTitleWrapper", newTitle);

        /// <summary>
        /// Load JS file programmatically
        /// </summary>
        public static async Task LoadJs(this IJSRuntime jsRuntime, string url)
        {
            await jsRuntime.InvokeVoidAsync("loadJs", new { name = "JsInterop", url = url });
        }

        /// <summary>
        /// Load JS file programmatically
        /// </summary>
        public static async Task<IJSObjectReference> LoadJsBlazor(this IJSRuntime jsRuntime, string url) => await jsRuntime.InvokeAsync<IJSObjectReference>("import", url);
        //public static async Task LoadJsBlazorList(this IJSRuntime jsRuntime, List<s> url)
        //{

        //     await jsRuntime.InvokeAsync<IJSObjectReference>("import", url);
        //}
    }

}
