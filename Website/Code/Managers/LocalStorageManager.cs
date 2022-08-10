using Microsoft.JSInterop;

namespace Website
{

    public interface ILocalStorage
    {
        ValueTask<T> GetProperty<T>(string propName);
        ValueTask SetProperty<T>(string propName, T value);
        ValueTask RemoveProperty(string propName);
        ValueTask WatchAsync<T>(T instance, string handlerName) where T : class;
    }


    /// <summary>
    /// Simple class to handle data in local storage
    /// </summary>
    public class LocalStorageManager : ILocalStorage
    {
        private readonly IJSRuntime _js;

        /// <summary>
        /// Initialized by builder service at startup
        /// </summary>
        public LocalStorageManager(IJSRuntime js) => this._js = js;

        public ValueTask<T> GetProperty<T>(string propName) => _js.InvokeAsync<T>("getProperty", propName);
        public ValueTask SetProperty<T>(string propName, T value) => _js.InvokeVoidAsync("setProperty", propName, value);
        public ValueTask RemoveProperty(string propName) => _js.InvokeVoidAsync("removeProperty", propName);

        /// <summary>
        /// Calls given handler when localstorage data changes
        /// </summary>
        public ValueTask WatchAsync<T>(T instance, string handlerName) where T : class
            => _js.InvokeVoidAsync("watchProperty",
                DotNetObjectReference.Create(instance), handlerName);
    }



}
