using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Library.API;
using Microsoft.JSInterop;
using Newtonsoft.Json.Linq;
using VedAstro.Library;

namespace Library.API
{
    internal static class AppData
    {
        public static IJSRuntime JsRuntime { get; set; }
        public const string postWrapper = "Interop.postWrapper";

    }

    /// <summary>
    /// Collection of static doers
    /// </summary>
    public static class Tools
    {

        /// <summary>
        /// No parsing direct from horses mouth
        /// </summary>
        public static async Task<string> ReadServerRaw(string receiverAddress)
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, receiverAddress);

            //tell sender to wait for complete reply before exiting
            var waitForContent = HttpCompletionOption.ResponseContentRead;

            //send the data on its way
            using var client = new HttpClient();
            client.Timeout = Timeout.InfiniteTimeSpan;
            var response = await client.SendAsync(httpRequestMessage, waitForContent);

            //return the raw reply to caller
            var dataReturned = await response.Content.ReadAsStringAsync();

            return dataReturned;
        }

        public static async Task<T?> ReadServer<T>(string receiverAddress) where T : JToken
        {
            //prepare the data to be sent
            var dataReturned = await ReadServerRaw(receiverAddress);

            //if no data from server than empty, end here
            if (string.IsNullOrEmpty(dataReturned)) { return null; }

            try
            {
                var parsed = JToken.Parse(dataReturned);
                return parsed as T;
            }
            catch (Exception e)
            {
                //todo proper logging
                Console.WriteLine(e);
            }

            return null;
        }

        public static async Task<JObject> WriteServer(HttpMethod method, string receiverAddress, JToken? payloadJson = null)
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
    }
}


