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
        
        

        public static async Task addClass(this IJSRuntime JsRuntime, string element, string classNames) => await JsRuntime.InvokeVoidAsync("addClassWrapper", element, classNames);
        public static async Task addClass(this IJSRuntime JsRuntime, ElementReference element, string classNames) => await JsRuntime.InvokeVoidAsync("addClassWrapper", element, classNames);
        public static async Task show(this IJSRuntime JsRuntime, string element) => await JsRuntime.InvokeVoidAsync("showWrapper", element);
        public static async Task show(this IJSRuntime JsRuntime, ElementReference element) => await JsRuntime.InvokeVoidAsync("showWrapper", element);
        public static async Task hide(this IJSRuntime JsRuntime, string element) => await JsRuntime.InvokeVoidAsync("hideWrapper", element);
        public static async Task hide(this IJSRuntime JsRuntime, ElementReference element) => await JsRuntime.InvokeVoidAsync("hideWrapper", element);
    }

}
