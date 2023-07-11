using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
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
        
        /// <summary>
        /// Only returns 
        /// </summary>
        public static async Task<JToken?> ReadOnlyIfPassJSJson(string receiverAddress, IJSRuntime jsRuntime)
        {
            //this call will take you to NetworkThread.js
            //todo handle empty response
            var rawPayloadStr = await jsRuntime.InvokeAsync<JsonElement>("Interop.ReadOnlyIfPassJson", receiverAddress);
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
        /// </summary>
        public static async Task<string?> ReadOnlyIfPassJSString(string receiverUrl, string? dataToSend, IJSRuntime jsRuntime)
        {
            //holds control till get
            //more efficient than passing control back and form Blazor and JS
            var rawPayloadStr = await jsRuntime.InvokeAsync<string?>("Interop.ReadOnlyIfPassString", receiverUrl, dataToSend);

            return rawPayloadStr;

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



        //        public static async Task<string?> ReadOnlyIfPass(string receiverAddress)
        //        {
        //            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, receiverAddress);

        //            //tell sender to wait for complete reply before exiting
        //            //var waitForContent = HttpCompletionOption.ResponseHeadersRead;
        //            //send the data on its way
        //            using var client = new HttpClient();
        //            //client.Timeout = Timeout.InfiniteTimeSpan;
        //            var response = await client.SendAsync(httpRequestMessage);

        //            //if not pass end here
        //            //if (!response.IsSuccessStatusCode) { return null; }

        //            //var sss = response.Headers.AsEnumerable().Where(x => x.Key.ToLower() == "call-status")?.FirstOrDefault().Value?.FirstOrDefault() ?? "EMPTY";
        //            //only get content if response is pass
        //            //var callStatus = response?.Headers?.GetValues("Call-Status")?.FirstOrDefault() ?? "Fail"; //fail if null
        //            //response.TrailingHeaders.TryGetValues("call-status", out var calls);
        //            response.Headers.TryGetValues("Call-Status", out var calls); //fail if null
        //            var callStatus = calls?.FirstOrDefault() ?? "Fail";

        //#if DEBUG
        //            Console.WriteLine($"API SAID : {callStatus}");
        //#endif
        //            var isPass = callStatus == "Pass";
        //            if (isPass)
        //            {
        //                //return the raw reply to caller
        //                var dataReturned = await response.Content.ReadAsStringAsync();

        //                return dataReturned;

        //            }

        //            //if anything but pass, don't look inside just say nothing
        //            return null;

        //        }

    }
}


