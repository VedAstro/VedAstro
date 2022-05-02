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
        public static async Task AddClass(this IJSRuntime jsRuntime, ElementReference element, string classNames) => await jsRuntime.InvokeVoidAsync("addClassWrapper", element, classNames);
        public static async Task Show(this IJSRuntime jsRuntime, ElementReference element) => await jsRuntime.InvokeVoidAsync("showWrapper", element);
        public static async Task Hide(this IJSRuntime jsRuntime, ElementReference element) => await jsRuntime.InvokeVoidAsync("hideWrapper", element);
        public static async Task AddEventListener(this IJSRuntime jsRuntime, ElementReference element, string eventName, string eventHandlerName) => await jsRuntime.InvokeVoidAsync("addEventListenerWrapper", element, eventName, eventHandlerName);
        public static async Task<double> ElementWidth(this IJSRuntime jsRuntime, ElementReference element) => await jsRuntime.InvokeAsync<double>("getElementWidth", element);
        public static async Task<double> AddWidthToEveryChild(this IJSRuntime jsRuntime, ElementReference element, double valueToAdd) => await jsRuntime.InvokeAsync<double>("addWidthToEveryChild", element, valueToAdd);
        public static async Task<T> Prop<T>(this IJSRuntime jsRuntime, ElementReference element, string propName) => await jsRuntime.InvokeAsync<T>("getPropWrapper", element, propName);
        public static async Task SetProp(this IJSRuntime jsRuntime, ElementReference element, string propName, object propVal) => await jsRuntime.InvokeVoidAsync("setPropWrapper", element, propName, propVal);
    }

}
