using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;


namespace Website
{

    /// <summary>
    /// Methods that connect blazor & js
    /// Wrapper for JS methods
    /// </summary>
    public static class BlazorJsInterop
    {
        /** FUNCTIONS **/
        public static async Task addClass(this IJSRuntime JsRuntime, ElementReference element, string classNames) => await JsRuntime.InvokeVoidAsync("addClassWrapper", element, classNames);
        public static async Task show(this IJSRuntime JsRuntime, ElementReference element) => await JsRuntime.InvokeVoidAsync("showWrapper", element);
        public static async Task hide(this IJSRuntime JsRuntime, ElementReference element) => await JsRuntime.InvokeVoidAsync("hideWrapper", element);
        public static async Task addEventListener(this IJSRuntime JsRuntime, ElementReference element, string eventName, string eventHandlerName) => await JsRuntime.InvokeVoidAsync("addEventListenerWrapper", element, eventName, eventHandlerName);
        public static async Task<double> getElementWidth(this IJSRuntime JsRuntime, ElementReference element) => await JsRuntime.InvokeAsync<double>("getElementWidth", element);
        public static async Task<double> addWidthToEveryChild(this IJSRuntime JsRuntime, ElementReference element, double valueToAdd) => await JsRuntime.InvokeAsync<double>("addWidthToEveryChild", element, valueToAdd);
        public static async Task<T> getProp<T>(this IJSRuntime JsRuntime, ElementReference element, string propName) => await JsRuntime.InvokeAsync<T>("getPropWrapper", element, propName);
        public static async Task setProp(this IJSRuntime JsRuntime, ElementReference element, string propName, object propVal) => await JsRuntime.InvokeVoidAsync("setPropWrapper", element, propName, propVal);
    }

}
