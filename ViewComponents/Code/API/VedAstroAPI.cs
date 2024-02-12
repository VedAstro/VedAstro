


using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Microsoft.JSInterop;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using VedAstro.Library;


namespace Website
{
    /// <summary>
    /// Make easy access to API, makes API easily consumable
    /// </summary>
    public class VedAstroAPI
    {

        public MatchTools Match;
        public PersonTools Person;
        public EventsChartTools EventsChart;

        public readonly string UserId;
        public readonly string VisitorID;
        public readonly IJSRuntime JsRuntime;
        public readonly URL URL;


        //--------CTOR
        public VedAstroAPI(string userId, string visitorId, IJSRuntime jsRuntime, URL url)
        {
            UserId = userId;
            VisitorID = visitorId;
            JsRuntime = jsRuntime;
            URL = url;
            Match = new MatchTools(this);
            Person = new PersonTools(this);
            EventsChart = new EventsChartTools(this);

#if DEBUG
            Console.WriteLine("ID's Set");
            Console.WriteLine($"UserId : {UserId}");
            Console.WriteLine($"VisitorID : {VisitorID}");
#endif
        }


        //--------------------------------------------------

        /// <summary>
        /// Polls the URL, gets the data and auto converts it
        /// </summary>
        public async Task<List<T>> GetList<T>(string inputUrl, Func<JToken, List<T>> converter)
        {
            // Check if inputUrl or converter is null and throw an exception if they are
            if (string.IsNullOrEmpty(inputUrl))
            {
                throw new ArgumentNullException(nameof(inputUrl));
            }
            if (converter == null)
            {
                throw new ArgumentNullException(nameof(converter));
            }

            // Define the polling rate in milliseconds
            const int pollRate = 3000;
            JToken? xListJson;

            // Keep polling the API until data is returned
            do
            {
                // Wait for the specified poll rate before making the next request
                await Task.Delay(pollRate);
                // Make a request to the API and get the JSON response
                xListJson = await WebsiteTools.ReadOnlyIfPassJSJson(inputUrl, JsRuntime);
            }
            while (xListJson == null); // Continue polling as long as the response is null

            // Extract the payload from the JSON response
            var payloadJson = xListJson["Payload"];

            // Convert the payload to a list of type T using the provided converter function
            var cachedPersonList = converter(payloadJson);

            // Return the converted list
            return cachedPersonList;
        }


        /// <summary>
        /// Makes direct call to API and waits tills she replies, could fail if call wait time is long
        ///  header with value pass is not checked for
        /// </summary>
        public async Task<List<T>> GetListNoPolling<T, Y>(string inputUrl, Y byteData, Func<JToken, List<T>> converter)
        {

            JToken? xListJson = await Tools.WriteServer<JObject, Y>(HttpMethod.Post, inputUrl, byteData);

            //var cachedPersonList = Person.FromJsonList(personListJson);
            var timeListJson = xListJson["Payload"];
            var cachedPersonList = converter.Invoke(timeListJson);

            return cachedPersonList;

        }
        public async Task<List<T>> GetListNoPolling<T>(string inputUrl, Func<JToken, List<T>> converter)
        {
            JToken? xListJson = await Tools.ReadServerRaw<JObject>(inputUrl);

            var timeListJson = xListJson["Payload"];
            var cachedPersonList = converter.Invoke(timeListJson);

            return cachedPersonList;

        }

        /// <summary>
        /// Shows alert using sweet alert js
        /// </summary>
        /// <param name="timer">milliseconds to auto close alert, if 0 then won't close which is default (optional)</param>
        /// <param name="useHtml">If true title can be HTML, default is false (optional)</param>
        public async Task ShowAlert(string icon, string title, bool showConfirmButton, int timer = 0, bool useHtml = false)
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


            await ShowAlert(alertData);
        }

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
        public async Task ShowAlert(object alertData)
        {
            try
            {
                await JsRuntime.InvokeVoidAsync(Swal_fire, alertData);
            }
            //above code will fail when called during app start, because haven't load lib
            //as such catch failure and silently ignore
            catch (Exception)
            {
                Console.WriteLine($"BLZ: ShowAlert Not Yet Load Lib Silent Fail!");
            }

        }

        const string Swal_fire = "Swal.fire";

        /// <summary>
        /// Shows alert using sweet alert js
        /// will show okay button, no timeout
        /// </summary>
        public async Task ShowAlert(string icon, string title, string descriptionText)
        {
            //call SweetAlert lib directly via constructor
            await JsRuntime.InvokeVoidAsync(Swal_fire, title, descriptionText, icon);
        }

        /// <summary>
        /// This method continuously polls a specified API until it receives data,
        /// with a delay between each poll, and then returns the data.
        /// Defaults to GET request when payload is null
        /// NOTE : 5S delay timeout
        /// </summary>

        public async Task<string> PollApiTillDataEventsChart(string url, object dataToSend = null)
        {
            var pollRate = TimeSpan.FromMilliseconds(5000);

            // Determine the HTTP method based on whether dataToSend is null or not
            var httpMethod = dataToSend == null ? HttpMethod.Get : HttpMethod.Post;

            // Define the headers for the HTTP request
            var requestHeaders = new Dictionary<string, string>
                                {
                                    {"Accept", "*/*"},
                                    {"Connection", "keep-alive"}
                                };

            // Start a loop that will continue making requests until the 'Call-Status' is 'Pass'
            while (true)
            {
                // Create the HTTP request
                var request = new HttpRequestMessage(httpMethod, url)
                {
                    Content = dataToSend != null ? new StringContent(JsonConvert.SerializeObject(dataToSend), Encoding.UTF8, "application/json") : null
                };

                // Add headers to the request
                foreach (var header in requestHeaders)
                {
                    request.Headers.Add(header.Key, header.Value);
                }

                // Make the HTTP request
                using (var client = new HttpClient())
                {
                    var response = await client.SendAsync(request);
                    // Get the 'Call-Status' from the response headers
                    var callStatus = response.Headers.GetValues("Call-Status").FirstOrDefault();
                    Console.WriteLine($"API SAID : {callStatus}");

                    // If 'Call-Status' is 'Pass', return the response text
                    if (callStatus == "Pass")
                    {
                        return await response.Content.ReadAsStringAsync();
                    }

                    // If 'Call-Status' is 'Fail', stop making requests and return null
                    if (callStatus == "Fail")
                    {
                        return null;
                    }

                    //control comes here
                    //If 'Call-Status' is 'Running'
                    //wait some time before trying again
                    await Task.Delay(pollRate);

                }
            }
        }



    }
}