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

        public static async Task<JObject> ReadServer(string receiverAddress)
        {
            //prepare the data to be sent
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, receiverAddress);

            //tell sender to wait for complete reply before exiting
            var waitForContent = HttpCompletionOption.ResponseContentRead;

            //send the data on its way
            using var client = new HttpClient();
            var response = await client.SendAsync(httpRequestMessage, waitForContent);

            //return the raw reply to caller
            var dataReturned = await response.Content.ReadAsStringAsync();
            return JObject.Parse(dataReturned);
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



//ARCHIVED CODE
//public static async Task<string> Post(string apiUrl, XElement xmlData)
//{
//    //this call will take you to NetworkThread.js
//    var rawPayload = await AppData.JsRuntime.InvokeAsync<string>(AppData.postWrapper, apiUrl, xmlData.ToString(SaveOptions.DisableFormatting));

//    //todo proper checking of status needed
//    return rawPayload;
//}

///// <summary>
///// HTTP Post via JS interop
///// </summary>
//public static async Task<WebResult<XElement>> WriteToServerXmlReply(string apiUrl, XElement xmlData, int timeout = 60)
//{

//TryAgain:

//    //ACT 1:
//    //send data to URL, using JS for reliability & speed
//    //also if call does not respond in time, we replay the call over & over
//    string receivedData;
//    try { receivedData = await VedAstro.Library.Tools.TaskWithTimeoutAndException(Post(apiUrl, xmlData), TimeSpan.FromSeconds(timeout)); }

//    //if fail replay and log it
//    catch (Exception e)
//    {
//        var debugInfo = $"Call to \"{apiUrl}\" timeout at : {timeout}s";

//        //WebLogger.Data(debugInfo);
//#if DEBUG
//        Console.WriteLine(debugInfo);
//#endif
//        goto TryAgain;
//    }

//    //ACT 2:
//    //check raw data 
//    if (string.IsNullOrEmpty(receivedData))
//    {
//        //log it
//        //await WebLogger.Error($"BLZ > Call returned empty\n To:{apiUrl} with payload:\n{xmlData}");

//        //send failed empty data to caller, it should know what to do with it
//        return new WebResult<XElement>(false, new XElement("CallEmptyError"));
//    }

//    //ACT 3:
//    //return data as XML
//    //problems might occur when parsing
//    //try to parse as XML
//    var writeToServerXmlReply = XElement.Parse(receivedData);
//    var returnVal = WebResult<XElement>.FromXml(writeToServerXmlReply);

//    //ACT 4:
//    return returnVal;
//}
