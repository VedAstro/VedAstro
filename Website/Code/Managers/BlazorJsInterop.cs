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


        //█▀█ █▀█ █▀█ █▀▀ █▀█ █▀▀ █▀ █▀   █▄▄ ▄▀█ █▀█
        //█▀▀ █▀▄ █▄█ █▄█ █▀▄ ██▄ ▄█ ▄█   █▄█ █▀█ █▀▄

        /// <summary>
        /// Adds input value to current progress bar
        /// </summary>
        public static async Task AddToProgressBar(this IJSRuntime jsRuntime, int value) => await jsRuntime.InvokeVoidAsync("AddToProgressBar", value);

        /// <summary>
        /// Adds input value to current progress bar
        /// </summary>
        public static async Task SetProgressBar(this IJSRuntime jsRuntime, int value) => await jsRuntime.InvokeVoidAsync("SetProgressBar", value);

        /// <summary>
        /// Gets value of progress bar now
        /// </summary>
        public static async Task<double> GetProgressBarValue(this IJSRuntime jsRuntime) => await jsRuntime.InvokeAsync<double>("GetProgressBarValue");

        /// <summary>
        /// Resets the progress bar to 0
        /// </summary>
        public static async Task ResetProgressBar(this IJSRuntime jsRuntime) => await jsRuntime.InvokeVoidAsync("ResetProgressBar");

        /// <summary>
        /// Automatically slowly updates the progress bar till 100% (simulation)
        /// </summary>
        public static async Task ProgressBarSlowAutoUpdate(this IJSRuntime jsRuntime) => await jsRuntime.InvokeVoidAsync("ProgressBarSlowAutoUpdate");




        
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
            //log this, don't await to reduce lag
            WebsiteLogManager.LogAlert(jsRuntime, alertData);

             await jsRuntime.InvokeVoidAsync("Swal.fire", alertData);
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
        /// <param name="timer">milliseconds to auto close alert (optional)</param>
        /// <returns></returns>
        public static async Task ShowAlert(this IJSRuntime jsRuntime, string icon, string title, bool showConfirmButton, int timer = 0)
        {

            var alertData = new
            {
                icon = icon,
                title = title,
                showConfirmButton = showConfirmButton,
                timer = timer
            };

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
        /// </summary>
        public static void ShowLoading(this IJSRuntime jsRuntime)
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

             jsRuntime.ShowAlert(alertData);
        }
        
        public static void HideLoading(this IJSRuntime jsRuntime) => jsRuntime.HideAlert();


        //TIPPY TOOLTIP LIBRARY

        /// <summary>
        /// Uses tooltip lib to attach tooltip to an element via selector or blazor reference  
        /// </summary>
        public static async Task Tippy(this IJSRuntime jsRuntime, object elementRef, object tooltipData) => 
            await jsRuntime.InvokeVoidAsync("tippy", elementRef, tooltipData);



        //ACCESS BROWSERS LOCAL STORAGE

        public static async Task<string> GetProperty(this IJSRuntime jsRuntime, string propName) => await jsRuntime.InvokeAsync<string>("getProperty", propName);
        public static async Task SetProperty(this IJSRuntime jsRuntime, string propName, string value) => await jsRuntime.InvokeVoidAsync("setProperty", propName, value);
        public static async Task RemoveProperty(this IJSRuntime jsRuntime, string propName) => await jsRuntime.InvokeVoidAsync("removeProperty", propName);
        /// <summary>
        /// Calls given handler when localstorage data changes
        /// </summary>
        public static async Task RemoveProperty<T>(this IJSRuntime jsRuntime, T instance, string handlerName) where T : class
            => await jsRuntime.InvokeVoidAsync("watchProperty",DotNetObjectReference.Create(instance), handlerName);




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
        public static async Task InjectIntoElement(this IJSRuntime jsRuntime, ElementReference _dasaViewBox, string value) => await jsRuntime.InvokeVoidAsync("InjectIntoElement",_dasaViewBox, value);

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
            else if(rawJson is JsonArray jsonArray)
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
        
        public static async Task<T> Prop<T>(this IJSRuntime jsRuntime, ElementReference element, string propName) => await jsRuntime.InvokeAsync<T>("getPropWrapper", element, propName);
        
        public static async Task SetProp(this IJSRuntime jsRuntime, ElementReference element, string propName, object propVal) => await jsRuntime.InvokeVoidAsync("setPropWrapper", element, propName, propVal);

        public static void OpenNewTab(this IJSRuntime jsRuntime, string url) => jsRuntime.InvokeVoidAsync("open", url, "_blank");

        /// <summary>
        /// Jquery .text()
        /// </summary>
        public static async Task<string> GetText(this IJSRuntime jsRuntime, ElementReference element) => await jsRuntime.InvokeAsync<string>("getTextWrapper", element);
        public static async Task<string> GetText(this IJSRuntime jsRuntime, string jquerySelector) => await jsRuntime.InvokeAsync<string>("getTextWrapper", jquerySelector);
        public static async Task<string> GetValue(this IJSRuntime jsRuntime, ElementReference element) => await jsRuntime.InvokeAsync<string>("getValueWrapper", element);
        public static async Task<string> GetValue(this IJSRuntime jsRuntime, string jquerySelector) => await jsRuntime.InvokeAsync<string>("getValueWrapper", jquerySelector);
        public static async Task SetValue(this IJSRuntime jsRuntime, string jquerySelector, object value) => await jsRuntime.InvokeVoidAsync("setValueWrapper", jquerySelector, value);


    }

}
