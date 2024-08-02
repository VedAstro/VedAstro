


//░█▀▀█ ─█▀▀█ ░█▄─░█ ░█▀▀▄ ░█─░█ ▀█▀ 　 ░█─░█ ─█▀▀█ ░█▀▀▄ 　 ░█──░█ ░█▀▀▀█ ░█▄─░█ 　 ▀▀█▀▀ ░█─░█ ░█▀▀▀ 
//░█─▄▄ ░█▄▄█ ░█░█░█ ░█─░█ ░█▀▀█ ░█─ 　 ░█▀▀█ ░█▄▄█ ░█─░█ 　 ░█░█░█ ░█──░█ ░█░█░█ 　 ─░█── ░█▀▀█ ░█▀▀▀ 
//░█▄▄█ ░█─░█ ░█──▀█ ░█▄▄▀ ░█─░█ ▄█▄ 　 ░█─░█ ░█─░█ ░█▄▄▀ 　 ░█▄▀▄█ ░█▄▄▄█ ░█──▀█ 　 ─░█── ░█─░█ ░█▄▄▄ 

//░█▀▀█ ░█▀▀█ ▀█▀ ▀▀█▀▀ ▀█▀ ░█▀▀▀█ ░█─░█ 　 ░█▀▀▀ ░█──░█ ░█▀▀▀ ░█▄─░█ 　 ░█▀▀█ ░█▀▀▀ ░█▀▀▀ ░█▀▀▀█ ░█▀▀█ ░█▀▀▀ 　 ▀▀█▀▀ ░█─░█ ░█▀▀▀ ░█──░█ 
//░█▀▀▄ ░█▄▄▀ ░█─ ─░█── ░█─ ─▀▀▀▄▄ ░█▀▀█ 　 ░█▀▀▀ ─░█░█─ ░█▀▀▀ ░█░█░█ 　 ░█▀▀▄ ░█▀▀▀ ░█▀▀▀ ░█──░█ ░█▄▄▀ ░█▀▀▀ 　 ─░█── ░█▀▀█ ░█▀▀▀ ░█▄▄▄█ 
//░█▄▄█ ░█─░█ ▄█▄ ─░█── ▄█▄ ░█▄▄▄█ ░█─░█ 　 ░█▄▄▄ ──▀▄▀─ ░█▄▄▄ ░█──▀█ 　 ░█▄▄█ ░█▄▄▄ ░█─── ░█▄▄▄█ ░█─░█ ░█▄▄▄ 　 ─░█── ░█─░█ ░█▄▄▄ ──░█── 

//░█─░█ ░█▀▀▀ 　 ░█─▄▀ ░█▄─░█ ░█▀▀▀ ░█──░█ 　 ░█─░█ ░█▀▀▀ 　 ░█▀▀▀ ▀▄░▄▀ ▀█▀ ░█▀▀▀█ ▀▀█▀▀ ░█▀▀▀ ░█▀▀▄ 
//░█▀▀█ ░█▀▀▀ 　 ░█▀▄─ ░█░█░█ ░█▀▀▀ ░█░█░█ 　 ░█▀▀█ ░█▀▀▀ 　 ░█▀▀▀ ─░█── ░█─ ─▀▀▀▄▄ ─░█── ░█▀▀▀ ░█─░█ 
//░█─░█ ░█▄▄▄ 　 ░█─░█ ░█──▀█ ░█▄▄▄ ░█▄▀▄█ 　 ░█─░█ ░█▄▄▄ 　 ░█▄▄▄ ▄▀░▀▄ ▄█▄ ░█▄▄▄█ ─░█── ░█▄▄▄ ░█▄▄▀


//MAN RISE & FALL NOT TO THEIR DICTUM, BUT GOD'S


using System.Data;
using Azure;
using Azure.Communication.Email;
using Microsoft.Azure.Functions.Worker.Http;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Mime;
using System.Text.Json;
using System.Xml.Linq;
using Azure.Data.Tables;
using Azure.Storage.Blobs;
using SuperConvert.Extensions;
using VedAstro.Library;
using Microsoft.Bing.ImageSearch.Models;
using Microsoft.Bing.ImageSearch;
using Microsoft.Extensions.DependencyModel;
using Person = VedAstro.Library.Person;

using MimeDetective;
using HtmlAgilityPack;

namespace API
{
    /// <summary>
    /// A collection of general tools used by API
    /// </summary>
    public static partial class APITools
    {
        //█░█ ▄▀█ █▀█ █▀▄   █▀▄ ▄▀█ ▀█▀ ▄▀█
        //█▀█ █▀█ █▀▄ █▄▀   █▄▀ █▀█ ░█░ █▀█

        //hard coded links to files stored in storage
        //public const string ApiDataStorageContainer = "vedastro-site-data";

        //NAMES OF FILES IN AZURE STORAGE FOR ACCESS
        public const string LiveChartHtml = "LiveChart.html";

        public const string VisitorLogFile = "VisitorLog.xml";
        public const string MessageListFile = "MessageList.xml";
        public const string SavedEventsChartListFile = "SavedChartList.xml";
        public const string SavedMatchReportList = "SavedMatchReportList.xml";
        public const string UserDataListXml = "UserDataList.xml";


        public static URL Url { get; set; } //instance of beta or stable URLs

        static APITools()
        {
            //make urls used here for beta or stable
            Url = new URL(GetIsBetaRuntime(), false); //obviously no debug mode
        }


        public static async Task<JObject> ExtractDataFromRequestJson(HttpRequestData request)
        {
            try
            {

                //get xml string from caller
                var readAsStringAsync = await request?.ReadAsStringAsync();
                var xmlString = readAsStringAsync ?? (new JObject()).ToString();

                //parse xml string todo needs catch here
                var parsedJson = JObject.Parse(xmlString);

                return parsedJson;
            }
            catch (Exception e)
            {
                //APILogger.Error("ERROR NO DATA FROM CALLER"); //log it
                //APILogger.Error(e); //log it
                return new JObject(); //null to be detected by caller
            }
        }


        /// <summary>
        /// Default success message sent to caller
        /// - .ToString(SaveOptions.DisableFormatting); to remove make xml indented
        /// </summary>
        public static HttpResponseData PassMessage(HttpRequestData req) => PassMessage("", req);

        /// <summary>
        /// we specify xml catch error at compile time, likely to fail
        /// </summary>
        public static HttpResponseData PassMessage(XElement payload, HttpRequestData req)
        {
            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/xml"); //todo check if charset is needed

            //wrap data in nice tag
            var finalXml =
                new XElement("Root", new XElement("Status", "Pass"), new XElement("Payload", payload)).ToString(
                    SaveOptions.DisableFormatting); //no XML indent

            //place in response body
            response.WriteString(finalXml);

            return response;
        }

        public static HttpResponseData PassMessage(string payload, HttpRequestData req)
        {
            var response = req.CreateResponse(HttpStatusCode.OK);
            //response.Headers.Add("Content-Type", "text/xml"); //todo check if charset is needed

            //wrap data in nice tag
            var finalXml =
                new XElement("Root", new XElement("Status", "Pass"), new XElement("Payload", payload)).ToString(
                    SaveOptions.DisableFormatting); //no XML indent

            //place in response body
            response.WriteString(finalXml);

            return response;
        }

        /// <summary>
        /// data comes in as XML should leave as JSON ready for sending to client via HTTP
        /// </summary>
        public static HttpResponseData MessageJson<T>(string statusResult, T payload, HttpRequestData req, string contentType = MediaTypeNames.Application.Json)
        {
            //STAGE 1 : SET HTTP HEADERS
            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", contentType);
            response.Headers.Add("Call-Status", statusResult); //lets caller know data is in payload
            response.Headers.Add("Access-Control-Expose-Headers", "Call-Status"); //needed by silly browser to read call-status

            //STAGE 2 : METADATA ABOUT CALL
            var finalPayloadJson = new JObject();
            finalPayloadJson["Status"] = statusResult;
            //add "Note" if specified
            if (!string.IsNullOrEmpty(APITools.ApiExtraNote)) { finalPayloadJson["Note"] = APITools.ApiExtraNote; }

            //STAGE 3 : PAYLOAD
            //if xelement than use xelement converter
            if (payload is List<XElement> payloadXmlList)
            {
                //convert XML to Json text
                var finalPayload = Tools.ListToJson(payloadXmlList);
                finalPayloadJson["Payload"] = finalPayload;
            }
            else if (payload is JProperty payloadJProperty)
            {
                //convert XML to Json text
                var temp = new JProperty("Payload", new JObject(payloadJProperty));
                finalPayloadJson.Add(temp);
            }
            else if (payload is JArray payloadJArray)
            {
                //place directly in
                finalPayloadJson["Payload"] = payloadJArray;
            }
            else if (payload is JToken payloadJToken)
            {
                //place directly in
                finalPayloadJson["Payload"] = payloadJToken;
            }
            else if (payload is JObject payloadJObject)
            {
                //place directly in
                finalPayloadJson["Payload"] = payloadJObject;
            }
            else if (payload is string payloadStr)
            {
                finalPayloadJson["Payload"] = payloadStr;
            }
            else if (payload is List<OpenAPIMetadata> payloadList)
            {
                finalPayloadJson["Payload"] = Tools.ListToJson(payloadList);
            }
            //if not special type than assign direct
            else
            {
                //if no payload just status, used for status only messages
                if (payload != null)
                {
                    finalPayloadJson["Payload"] = JToken.Parse(payload.ToString());
                }
            }

            //convert XML to Json text
            string jsonText = finalPayloadJson.ToString();

            //place in response body
            response.WriteString(jsonText);

            return response;
        }

        /// <summary>
        /// if specified will be included in open api return response to caller
        /// used to tell if call has been slowed, or other notifications
        /// </summary>
        public static string? ApiExtraNote { get; set; } = "";

        public static HttpResponseData FailMessageJson(XElement payload, HttpRequestData req) =>
            MessageJson("Fail", payload, req);

        public static HttpResponseData FailMessageJson(string payload, HttpRequestData req) =>
            MessageJson("Fail", payload, req);

        public static HttpResponseData FailMessageJson(Exception payloadException, HttpRequestData req) =>
            MessageJson("Fail", Tools.ExceptionToXML(payloadException), req);

        public static HttpResponseData PassMessageJson(object payload, HttpRequestData req) =>
            MessageJson("Pass", payload, req);

        public static HttpResponseData PassMessageJson(HttpRequestData req) => MessageJson<object>("Pass", null, req);

        public static HttpResponseData FailMessage(object payload, HttpRequestData req)
        {
            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain");

            //wrap data in nice tag
            var finalXml =
                new XElement("Root", new XElement("Status", "Fail"), new XElement("Payload", payload)).ToString(
                    SaveOptions.DisableFormatting); //no XML indent

            //place in response body
            response.WriteString(finalXml);

            return response;
        }

        public static HttpResponseData FailMessage(Exception payloadException, HttpRequestData req) =>
            FailMessage(Tools.ExceptionToXML(payloadException), req);


        //----------------------------------------FUNCTIONS---------------------------------------------

        public static byte[] ExtractRawImageFromRequest(HttpRequestMessage req)
        {
            var rawStream = req.Content.ReadAsByteArrayAsync().Result;

            return rawStream;
        }


        /// <summary>
        /// Reads data stamped build version, if "beta" is found in that name, return true
        /// </summary>
        public static bool GetIsBetaRuntime() => ThisAssembly.BranchName.Contains("beta");
        public static async Task<JsonElement> ExtractDataFromRequestJsonNET(HttpRequestData request)
        {
            string jsonString = "";

            try
            {
                //get raw string from caller
                jsonString = (await request?.ReadAsStringAsync()) ?? @"{Root:""Empty""}";

                JsonDocument doc = JsonDocument.Parse(jsonString);
                JsonElement root = doc.RootElement;

                return root;
            }
            //todo better logging
            catch (Exception e)
            {
                APILogger.Error(e); //log it
                throw new Exception($"ExtractDataFromRequestJson : FAILED : {jsonString} \n {e.Message}");
            }
        }

        


        ///// <summary>
        ///// If there is binary file in request it will take it out,
        ///// not support multipart form
        ///// </summary>
        //public static async Task<byte[]> ExtractFileFromRequest(HttpRequestData request)
        //{
        //    request.Body.Position = 0; //need to set to 0 else will get 0 bytes
        //    var x = request.Body.ToByteArray();
        //    return x;
        //}


        /// <summary>
        /// Get all charts belonging to owner ID
        /// </summary>

        /// <summary>
        /// Gets user data, if user does
        /// not exist makes a new one & returns that
        /// Note :
        /// - email is used to find user, not hash or id (unique)
        /// - Uses UserDataList.xml
        /// </summary>
        public static async Task<UserData> GetUserData(string id, string name, string email)
        {
            //get user data list file (UserDataList.xml) Azure storage
            var userDataListXml = await Tools.GetXmlFileFromAzureStorage(UserDataListXml, Tools.BlobContainerName);

            //look for user with matching email
            var foundUserXml = userDataListXml.Root?.Elements()
                .Where(userDataXml => userDataXml.Element("Email")?.Value == email)?
                .FirstOrDefault();

            //if user found, initialize xml and send that
            if (foundUserXml != null)
            {
                return UserData.FromXml(foundUserXml);
            }

            //if no user found, make new user and send that
            else
            {
                //create new user from google's data
                var newUser = new UserData(id, name, email);

                //add new user xml to main list
                await Tools.AddXElementToXDocumentAzure(newUser.ToXml(), UserDataListXml, Tools.BlobContainerName);

                //return newly created user to caller
                return newUser;
            }
        }

        /// <summary>
        /// Given a user data it will find matching user email and replace the existing UserData with inputed
        /// Note :
        /// - Uses UserDataList.xml
        /// </summary>
        public static async Task<UserData> UpdateUserData(string id, string name, string email)
        {
            //get user data list file (UserDataList.xml) Azure storage
            var userDataListXml = await Tools.GetXmlFileFromAzureStorage(UserDataListXml, Tools.BlobContainerName);

            //look for user with matching email
            var foundUserXml = userDataListXml.Root?.Elements()
                .Where(userDataXml => userDataXml.Element("Email")?.Value == email)?
                .FirstOrDefault();

            //if user found, initialize xml and send that
            if (foundUserXml != null)
            {
                return UserData.FromXml(foundUserXml);
            }

            //if no user found, make new user and send that
            else
            {
                //create new user from google's data
                var newUser = new UserData(id, name, email);

                //add new user xml to main list
                await Tools.AddXElementToXDocumentAzure(newUser.ToXml(), UserDataListXml, Tools.BlobContainerName);

                //return newly created user to caller
                return newUser;
            }
        }


        /// <summary>
        /// Makes a HTTP GET request and return the data as HTTP response message
        /// </summary>
        public static async Task<HttpResponseMessage> GetRequest(string receiverAddress)
        {
            //prepare the data to be sent
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, receiverAddress);

            //get the data sender
            using var client = new HttpClient() { Timeout = new TimeSpan(0, 0, 0, 0, Timeout.Infinite) }; //no timeout

            //tell sender to wait for complete reply before exiting
            var waitForContent = HttpCompletionOption.ResponseContentRead;

            //send the data on its way
            var response = await client.SendAsync(httpRequestMessage, waitForContent);

            //return the raw reply to caller
            return response;
        }


        public static List<Person> GetAllPersonList()
        {
            //get all
            var foundCalls = AzureTable.PersonList.Query<PersonListEntity>();

            var returnList = new List<Person>();
            foreach (var call in foundCalls)
            {
                returnList.Add(Person.FromAzureRow(call));
            }


            return returnList;
        }

        public static HttpResponseData SendHtmlToCaller(string chartContentSvg, HttpRequestData incomingRequest)
        {
            //send image back to caller
            var response = incomingRequest.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/html");
            //place in response body
            response.WriteString(chartContentSvg);
            return response;
        }

        public static HttpResponseData SendTextToCaller(string chartContentSvg, HttpRequestData incomingRequest)
        {
            //send image back to caller
            var response = incomingRequest.CreateResponse(HttpStatusCode.OK);
            //response.Headers.Add("Content-Type", "text/plain");
            //place in response body
            response.WriteString(chartContentSvg);

            return response;
        }

        public static void SendEmail(string fileName, string fileFormat, string receiverEmailAddress,
            Stream rawFileBytes)
        {
            var emailClient = getEmailClient();

            var fileNameFull = $"{fileName}.{fileFormat.ToLower()}";

            var emailTitle = $"Shared {fileFormat.ToUpper()} from VedAstro";

            // Create the email content, visible to user
            var emailContent = new EmailContent(emailTitle)
            {
                PlainText = $"Find attached your {fileName}, from VedAstro.org -> {fileNameFull}",
                Html = "<html><body>Shared file from VedAstro.org</body></html>"
            };

            var emailMessage = new EmailMessage(
                senderAddress: "contact@vedastro.org", // The email address of the domain registered with the Communication Services resource
                recipientAddress: receiverEmailAddress,
                content: emailContent);

            var attachmentName = fileNameFull;
            var contentType =
                Tools.StringToMimeType(fileFormat) ?? MediaTypeNames.Text.Plain; //if fail just plain noodle will do

            var content = BinaryData.FromStream(rawFileBytes);
            var emailAttachment = new EmailAttachment(attachmentName, contentType, content);

            emailMessage.Attachments.Add(emailAttachment);

            try
            {
                EmailSendOperation emailSendOperation = emailClient.Send(WaitUntil.Completed, emailMessage);
                Console.WriteLine($"Email Sent. Status = {emailSendOperation.Value.Status}");

                /// Get the OperationId so that it can be used for tracking the message for troubleshooting
                string operationId = emailSendOperation.Id;
                Console.WriteLine($"Email operation id = {operationId}");
            }
            catch (RequestFailedException ex)
            {
                /// OperationID is contained in the exception message and can be used for troubleshooting purposes
                Console.WriteLine(
                    $"Email send operation failed with error code: {ex.ErrorCode}, message: {ex.Message}");
            }

            //-------------LOCAL FUNCS

            EmailClient getEmailClient()
            {
                //read the connection string
                var connectionString = Secrets.Get("AutoEmailerConnectString");


                //raise alarm if no connection string
                if (string.IsNullOrEmpty(connectionString))
                {
                    throw new Exception($"Failed to get connection string : AutoEmailerConnectString!");
                }

                //sign in to email
                return new EmailClient(connectionString);
            }
        }

        public static async Task SwapUserId(string ownerId, string visitorId)
        {
            //if both same no swap needed
            if (ownerId == visitorId) { return; }

            //if not yet logged in then skip
            if (ownerId == "101") { return; }

            //get all person's under visitor id
            var visitorIdPersons = AzureTable.PersonList.Query<PersonListEntity>(call => call.PartitionKey == visitorId);

            //if no records, then end here
            if (!visitorIdPersons.Any()) { return; }

            //transfer each person one by one
            foreach (var personOriRecord in visitorIdPersons)
            {
                //1: make duplicate record with new owner id
                //overwrite visitor id with user id
                var modifiedPerson = personOriRecord.Clone();
                modifiedPerson.PartitionKey = ownerId;
                AzureTable.PersonList.AddEntity(modifiedPerson);

                //2: delete original "visitor" record 
                await AzureTable.PersonList.DeleteEntityAsync(personOriRecord.PartitionKey, personOriRecord.RowKey);
            }


        }


        public static IEnumerable<LogItem> GetOnlineVisitors(XDocument visitorLogDocument)
        {

            //parse all logs
            var xmlRecordList = visitorLogDocument.Root?.Elements() ?? new List<XElement>();
            List<LogItem> logItemList = LogItem.FromXml(xmlRecordList);

            //last hour
            var lastHourRecords = from logItem in logItemList
                                  where Tools.IsWithinLastHour(logItem.Time, -24)
                                  select logItem;

            //unique visitors
            List<LogItem> uniqueList = lastHourRecords.DistinctBy(p => p.VisitorId).ToList();

            return uniqueList;
        }


        /// <summary>
        /// data comes in as XML should leave as JSON ready for sending to client via HTTP
        /// </summary>
        public static JObject AnyTypeToJson<T>(T payload)
        {
            var finalPayloadJson = new JObject();

            //if xelement than use xelement converter
            if (payload is List<XElement> payloadXmlList)
            {
                //convert XML to Json text
                var finalPayload = Tools.ListToJson(payloadXmlList);
                finalPayloadJson["Payload"] = finalPayload;
            }
            else if (payload is JProperty payloadJToken)
            {
                //convert XML to Json text
                //finalPayloadJson["Payload"] = JToken.FromObject(payloadJToken);
                var temp = new JProperty("Payload", new JObject(payloadJToken));
                finalPayloadJson.Add(temp);
            }
            else if (payload is string payloadStr)
            {
                finalPayloadJson["Payload"] = payloadStr;
            }
            //if not special type than assign direct
            else
            {
                finalPayloadJson["Payload"] = JToken.Parse(payload.ToString());
            }

            //convert XML to Json text
            // string jsonText = finalPayloadJson.ToString(); //todo can be direct aslo

            return finalPayloadJson;
        }

        public static string GetHeaderValue(HttpResponseData request, string headerName)
        {
            IEnumerable<string> list;
            return request.Headers.TryGetValues(headerName, out list) ? list.FirstOrDefault() : null;
        }

        public static object GetHeaderValue(HttpResponseMessage request, string headerName)
        {
            IEnumerable<string> list;
            return request.Headers.TryGetValues(headerName, out list) ? list.FirstOrDefault() : null;
        }

        public static HttpResponseData SendSvgToCaller(string chartContentSvg, HttpRequestData incomingRequest)
        {
            //send image back to caller
            var response = incomingRequest.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "image/svg+xml");

            //place in response body
            response.WriteString(chartContentSvg);
            return response;
        }






        //--------------------TODO NEEDS MOVING



        /// <summary>
        /// Uses cache if available else calculates the data
        /// also auto adds the newly calculated data cache for future
        /// </summary>
        public static async Task<T> CacheExecuteTask<T>(Func<Task<T>> generateChart, string callerId, string mimeType = "")
        {
            //check if cache exist
            var isExist = await AzureCache.IsExist(callerId);

            T chart;

            if (!isExist)
            {
                //squeeze the Sky Juice!
                chart = await generateChart.Invoke();
                //save for future
                var blobClient = await AzureCache.Add<T>(callerId, chart, mimeType);
            }
            else
            {
                chart = await AzureCache.GetData<T>(callerId);
            }

            return chart;
        }



        /// <summary>
        /// Given a cache generator function and a name for the data
        /// it'll calculate and save data to cache Data Blob storage
        /// </summary>
        public static async Task<BlobClient> ExecuteAndSaveToCache(Func<string> cacheGenerator, string cacheName, string mimeType = "")
        {

#if DEBUG
            Console.WriteLine($"NO CACHE! RUNNING COMPUTE : {cacheName}");
#endif

            BlobClient? chartBlobClient;

            try
            {
                //lets everybody know call is running
                CallTracker.CallStart(cacheName);

                //squeeze the Sky Juice!
                var chartBytes = cacheGenerator.Invoke();

                //save for future
                chartBlobClient = await AzureCache.Add(cacheName, chartBytes, mimeType);

            }
            //always mark the call as ended
            finally
            {
                CallTracker.CallEnd(cacheName); //mark the call as ended
            }


            return chartBlobClient;
        }

        public static string JsonToCsv(string jsonData)
        {

            string csvPath = jsonData.ToCsv();


            Console.WriteLine(csvPath);

            return csvPath;
        }


       


        /// <summary>
        /// based on caller's ip address, set limit
        /// </summary>
        /// <returns></returns>
        public static async Task AutoControlOpenAPIOverload(OpenAPILogBookEntity callData)
        {
            var minute1 = 1;
            var minute30 = 30;
            var ipAddress = callData.PartitionKey;
            var lastCallsCount = APILogger.GetAllCallsWithinLastTimeperiod(ipAddress, minute1);

            //rate set in runtime settings is multipliedfull 
            var msDelayRate = 800;
            var freeCallRate = 50;//allowed high speed calls per minute //int.Parse(Secrets.OpenAPICallDelayMs); TODO add to Secrets

            //if more than 1 abuse count in the last 10 minutes than end the call here with no reply
            //NOTE : no default way in Azure Function to cut connection, so THROW EXCEPTION cuts call
            var abuseCount = APILogger.GetAbuseCountWithinLastTimeperiod(ipAddress, minute30);
            if (abuseCount > 2)
            {
                //drop the call
                await Task.Delay(9999999); //12min
                throw new Exception();
            }

            //if delay applied then let caller know
            //NOTE : other words allowed 1 call every 30 seconds
            var userCallRate = lastCallsCount / minute1; //calls per minute
            if (userCallRate > freeCallRate)
            {
                //make a mark in API abuse list, to detect excessive non-stop calls
                await AzureTable.APIAbuseList.UpsertEntityAsync(new APIAbuseRow() { PartitionKey = ipAddress, RowKey = Tools.GenerateId() });

                //every additional call within specified time limit gets slowed accordingly
                //exp: last 3 calls x 800ms = 4th call delay --> 2400ms
                var msDelay = lastCallsCount * msDelayRate;

                //todo shorten link
                APITools.ApiExtraNote = $"Donate To Increase Speed : " +
                                        $"{URL.Donate}";

                await Task.Delay(msDelay);
#if DEBUG
                Console.WriteLine($"AUTO Throttle : IP -> {ipAddress} Delay ->{msDelay}ms");
#endif

            }
            else
            {
                //if below limit than let call run, clear message
                APITools.ApiExtraNote = "";
            }

        }


        /// <summary>
        /// only for specified table vedastroapistorage
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static TableClient GetTableClientFromTableName(string tableName)
        {
            //prepare call stuff
            string storageAccountKey = Secrets.Get("CentralStorageKey");
            string accountName = Secrets.Get("CentralStorageAccountName");
            var tableUlr = $"https://{accountName}.table.core.windows.net/{tableName}";
            
            //get connection
            var tableServiceClient = new TableServiceClient(new Uri(tableUlr), new TableSharedKeyCredential(accountName, storageAccountKey));
            var client = tableServiceClient.GetTableClient(tableName);

            return client;
        }


        /// <summary>
        /// Given a file or string convertible data, send it to caller accordingly
        /// </summary>
        public static HttpResponseData SendAnyToCaller(string format, string calculatorName, dynamic rawPlanetData, HttpRequestData incomingRequest)
        {
            //if format specified as JPEG, then process separately if body is not binary already
            //meaning here process methods that can output JSON
            if (format == "JPEG" && rawPlanetData is not byte[])
            {
                //if supports JPEG convert here and end it
                if (rawPlanetData is IToJpeg iToJpeg)
                {
                    var rawFileData = iToJpeg.ToJpeg();

                    //get correct mime type so browser or receiver knows how to present
                    var mimeType = GetMimeType(rawFileData);

                    return Tools.SendFileToCaller(rawFileData, incomingRequest, mimeType);
                }
                //JSON convert to table needs extra step
                //NOTE: this generic JSON to JPEG converter,
                //if rendering not good implement custom IToJpeg
                else  /*(rawPlanetData is IToJson iToJson)*/
                {
                    //first convert to json
                    DataTable rawTable = Tools.AnyToDataTable(calculatorName, rawPlanetData);

                    //convert data table to JPEG image
                    var image = Tools.DataTableToJpeg(rawTable);

                    //get correct mime type so browser or receiver knows how to present
                    var mimeType = GetMimeType(image);

                    return Tools.SendFileToCaller(image, incomingRequest, mimeType);
                }

                Type type = rawPlanetData.GetType();
                return APITools.FailMessageJson($"JPEG Formatter for {type.Name} under construction. Donate to speed up development.", incomingRequest);
            }

            //then it is a file
            else if (rawPlanetData is byte[] rawFileData)
            {
                //get correct mime type so browser or receiver knows how to present
                var mimeType = GetMimeType(rawFileData);

                return Tools.SendFileToCaller(rawFileData, incomingRequest, mimeType);
            }

            //if array pass directly
            else if (rawPlanetData is JArray rawPlanetDataJson)
            {
                return APITools.PassMessageJson(rawPlanetDataJson, incomingRequest);
            }
            //probably data that can be sent as JSON text
            else
            {
                //4 : CONVERT TO JSON
                var payloadJson = Tools.AnyToJSON(calculatorName, rawPlanetData); //use calculator name as key

                //5 : SEND DATA
                return APITools.PassMessageJson(payloadJson, incomingRequest);
            }

        }

        public static string GetMimeType(byte[] fileBytes)
        {
            var inspector = new ContentInspectorBuilder()
            {
                Definitions = MimeDetective.Definitions.Default.All()
            }.Build();

            var fileType = inspector.Inspect(fileBytes);

            var resultsByMimeType = fileType.ByMimeType();

            // Return the MIME type
            var mimeType = resultsByMimeType[0].MimeType;
            return mimeType ?? "application/octet-stream";
        }
    }

}