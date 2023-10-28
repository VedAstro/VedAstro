


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
        public MLTableTools MLTable;

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
            MLTable = new MLTableTools(this);
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
        public async Task<string?> PollApiTillDataEVENTSChart(string inputUrl, string dataToSend = null)
        {
            string? parsedJsonReply = null;
            var pollRate = 5000;
            var notReady = true;
            while (notReady)
            {
                var cts = new CancellationTokenSource(pollRate);
                try
                {
                    parsedJsonReply = await ReadOnlyIfPassJSString(inputUrl, dataToSend, cts.Token).WithCancellation(cts.Token);
                    notReady = parsedJsonReply == null; //if null no data, continue wait
                }
                catch (OperationCanceledException)
                {
                    // The operation didn't finish within the timeout.
                }
                if (notReady)
                {
                    // If no data, wait for the poll rate before trying again.
                    await Task.Delay(pollRate);
                }
            }
            return parsedJsonReply;
            async Task<string> ReadOnlyIfPassJSString(string url, object dataToSend, CancellationToken cancellationToken)
            {
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
                    try
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
                            var response = await client.SendAsync(request, cancellationToken);
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
                        }
                    }
                    catch (Exception ex)
                    {
                        // If an error occurs during the fetch operation, log the error and return null
                        Console.Error.WriteLine($"Error occurred while making HTTP request: {ex}");
                        return null;
                    }
                }
            }
        }

        /// <summary>
        /// takes in raw response from API and
        /// gets payload after checking status and shows error if status "Fail"
        /// note :  no parser use direct, support for string, int and double
        /// </summary>
        public static T GetPayload<T>(JToken rawResult, Func<JToken, T>? parser)
        {
            //result must say Pass, else it has failed
            var isPass = rawResult["Status"]?.Value<string>() == "Pass";
            var payloadJson = rawResult["Payload"] ?? new JObject();

            if (isPass)
            {
#if DEBUG
                Console.WriteLine("API SAID: PASS"); //debug to know all went well
#endif

                //use parser if available, use that, end here
                if (parser != null)
                {
                    var personJson = parser(payloadJson);
                    return personJson;
                }

                //if no parser use direct, support for string, int and double
                return payloadJson.Value<T>();

            }
            else
            {
#if DEBUG
                Console.WriteLine($"API SAID : FAIL :\n{payloadJson}");
#endif
                //for now this should notify errors nicely, todo maybe exceptions is not best 
                throw new Exception($"Failed to get {typeof(T).AssemblyQualifiedName} from API payload");
            }

        }

        /// <summary>
        /// HTTP Post via JS interop
        /// </summary>
        public static async Task<WebResult<JToken>> WriteToServerJsonReply(string apiUrl, string stringData, int timeout = 60)
        {

        TryAgain:

            //ACT 1:
            //send data to URL, using JS for reliability & speed
            //also if call does not respond in time, we replay the call over & over
            string receivedData;
            try { receivedData = await Tools.TaskWithTimeoutAndException(ServerManager.Post(apiUrl, stringData), TimeSpan.FromSeconds(timeout)); }

            //if fail replay and log it
            catch (Exception e)
            {
                var debugInfo = $"Call to \"{apiUrl}\" timeout at : {timeout}s";

                WebLogger.Data(debugInfo);
#if DEBUG
                Console.WriteLine(debugInfo);
#endif
                goto TryAgain;
            }

            //ACT 2:
            //check raw data 
            if (string.IsNullOrEmpty(receivedData))
            {
                //log it
                await WebLogger.Error($"BLZ > Call returned empty\n To:{apiUrl} with payload:\n{stringData}");

                //send failed empty data to caller, it should know what to do with it
                return new WebResult<JToken>(false, new JObject("CallEmptyError"));
            }

            //ACT 3:
            //return data as Json
            var writeToServerXmlReply = JObject.Parse(receivedData);
            var returnVal = WebResult<XElement>.FromJson(writeToServerXmlReply);

            //ACT 4:
            return returnVal;
        }

    }
}