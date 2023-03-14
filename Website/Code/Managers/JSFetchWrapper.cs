using Microsoft.JSInterop;
using System.Xml.Linq;
using Genso.Astrology.Library;

namespace Website.Code.Managers
{
    /// <summary>
    /// Simple wrapper for using JS runtime FETCH from bla
    /// Notes:
    /// - added benefit of parallel access via workers
    /// - HttpClient is erroneous so direct via JS
    /// </summary>
    public class JSFetchWrapper
    {
        //public delegate void ThresholdReachedEventHandler(object sender);

        public event EventHandler NetworkReply;

        public JSFetchWrapper()
        {
            //makes this components instance available in JS, for receiving 
            AppData.JsRuntime.InvokeVoidAsync("SetJSFetchWrapperInstance", DotNetObjectReference.Create(this));

        }

        public async Task<string> Post(string apiUrl, XElement xmlData)
        {
            ////send using worker JS
            //var callMeBack = Tools.GenerateId();
            //var postData = new
            //{
            //    callMeBack = callMeBack, //id so that caller can return data from worker independent of main thread
            //    method = "POST",
            //    url = apiUrl,
            //    payload = xmlData.ToString(), //todo remove /n & space between tags, compression level 1
            //};

            //this call will take you to NetworkThread.js
            var rawPayload = await AppData.JsRuntime.InvokeAsync<string>("postWrapper", apiUrl, xmlData.ToString());

            //var receivedData = await GetMyData(callMeBack);

            //todo proper checking of status needed
            return rawPayload;
        }

        private async Task<string> GetMyData(string callMeBack)
        {

            //check main list till find
            var notFound = true;
            ReceivedData found = default;
            while (notFound)
            {
                var foundList = ReceivedDataList.Where(x => x.CallMeBack == callMeBack);
                //once data sent by worker has arrived, delete from main list & return to caller
                var any = foundList != null && (foundList?.Any() ?? false);
                if (any)
                {
                    notFound = false; //stop looking
                    found = foundList.First();
                    ReceivedDataList.Remove(found);
                }
            }

            return found.Payload;
        }

        public List<ReceivedData> ReceivedDataList { get; set; } = new List<ReceivedData>();

        public readonly record struct ReceivedData(string CallMeBack, string Payload);

        /// <summary>
        /// Called from JS when return data from call
        /// </summary>
        [JSInvokable]
        public void OnNetworkReply(dynamic receivedData)
        {

            //get who's call this is
            var callMeBack = (string)receivedData.callMeBack;

            //payload data
            var receivedRaw = (string)receivedData.payload;

            //parse the data
            var x = new ReceivedData(callMeBack, receivedRaw);

            //add to main list
            ReceivedDataList.Add(x);
        }
    }
}
