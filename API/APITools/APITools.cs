using Azure;
using Azure.Communication.Email;
using Microsoft.Azure.Functions.Worker.Http;
using Newtonsoft.Json.Linq;
using System.Drawing;
using System.Net;
using System.Net.Mime;
using System.Text.Json;
using System.Xml.Linq;
using VedAstro.Library;

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

        public const string PersonListFile = "PersonList.xml";
        public const string VisitorLogFile = "VisitorLog.xml";
        public const string TaskListFile = "TaskList.xml";
        public const string MessageListFile = "MessageList.xml";
        public const string SavedEventsChartListFile = "SavedChartList.xml";
        public const string SavedMatchReportList = "SavedMatchReportList.xml";
        public const string RecycleBinFile = "RecycleBin.xml";
        public const string UserDataListXml = "UserDataList.xml";
        public const string HoroscopeDataListFile = "https://vedastro.org/data/HoroscopeDataList.xml"; //todo should be getting beta version
        public const string BlobContainerName = "vedastro-site-data";
        public static URL Url { get; set; } //instance of beta or stable URLs

        static APITools()
        {
            //make urls used here for beta or stable
            Url = new URL(GetIsBetaRuntime());
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
            var finalXml = new XElement("Root", new XElement("Status", "Pass"), new XElement("Payload", payload)).ToString(SaveOptions.DisableFormatting); //no XML indent

            //place in response body
            response.WriteString(finalXml);

            return response;
        }

        public static HttpResponseData PassMessage(string payload, HttpRequestData req)
        {
            var response = req.CreateResponse(HttpStatusCode.OK);
            //response.Headers.Add("Content-Type", "text/xml"); //todo check if charset is needed

            //wrap data in nice tag
            var finalXml = new XElement("Root", new XElement("Status", "Pass"), new XElement("Payload", payload)).ToString(SaveOptions.DisableFormatting); //no XML indent

            //place in response body
            response.WriteString(finalXml);

            return response;
        }

        /// <summary>
        /// data comes in as XML should leave as JSON ready for sending to client via HTTP
        /// </summary>
        public static HttpResponseData MessageJson<T>(string statusResult, T payload, HttpRequestData req)
        {
            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", MediaTypeNames.Application.Json);

            var finalPayloadJson = new JObject();
            finalPayloadJson["Status"] = statusResult;

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

        public static HttpResponseData FailMessageJson(XElement payload, HttpRequestData req) => MessageJson("Fail", payload, req);
        public static HttpResponseData FailMessageJson(string payload, HttpRequestData req) => MessageJson("Fail", payload, req);
        public static HttpResponseData FailMessageJson(Exception payloadException, HttpRequestData req) => MessageJson("Fail", Tools.ExceptionToXml(payloadException), req);

        public static HttpResponseData PassMessageJson(object payload, HttpRequestData req) => MessageJson("Pass", payload, req);
        public static HttpResponseData PassMessageJson(HttpRequestData req) => MessageJson<object>("Pass", null, req);

        public static HttpResponseData FailMessage(object payload, HttpRequestData req)
        {
            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain");

            //wrap data in nice tag
            var finalXml = new XElement("Root", new XElement("Status", "Fail"), new XElement("Payload", payload)).ToString(SaveOptions.DisableFormatting); //no XML indent

            //place in response body
            response.WriteString(finalXml);

            return response;
        }

        public static HttpResponseData FailMessage(Exception payloadException, HttpRequestData req) => FailMessage(Tools.ExceptionToXml(payloadException), req);


        //----------------------------------------FUNCTIONS---------------------------------------------

        public static byte[] ExtractRawImageFromRequest(HttpRequestMessage req)
        {
            var rawStream = req.Content.ReadAsByteArrayAsync().Result;

            return rawStream;
        }

        /// <summary>
        /// Gets public IP address of client sending the http request
        /// </summary>
        public static IPAddress GetCallerIp(this HttpRequestData req)
        {
            var headerDictionary = req.Headers.ToDictionary(x => x.Key, x => x.Value, StringComparer.Ordinal);
            var key = "x-forwarded-for";
            var key2 = "x-azure-clientip";
            IPAddress? ipAddress = null;

            if (headerDictionary.ContainsKey(key) || headerDictionary.ContainsKey(key2))
            {
                var headerValues = headerDictionary[key];
                var ipn = headerValues?.FirstOrDefault()?.Split(new char[] { ',' }).FirstOrDefault()?.Split(new char[] { ':' }).FirstOrDefault();
                var key1ParseResult = IPAddress.TryParse(ipn, out ipAddress);

                //if key 1 fail , try key 2
                if (!key1ParseResult)
                {
                    headerValues = headerDictionary[key];
                    ipn = headerValues?.FirstOrDefault()?.Split(new char[] { ',' }).FirstOrDefault()?.Split(new char[] { ':' }).FirstOrDefault();
                    key1ParseResult = IPAddress.TryParse(ipn, out ipAddress);
                }
            }

            return ipAddress ?? IPAddress.None;
        }

        public static IPAddress GetCallerIp(this HttpRequestMessage request)
        {
            IPAddress result = null;
            if (request.Headers.TryGetValues("X-Forwarded-For", out IEnumerable<string> values))
            {
                var ipn = values.FirstOrDefault().Split(new char[] { ',' }).FirstOrDefault().Split(new char[] { ':' }).FirstOrDefault();
                IPAddress.TryParse(ipn, out result);
            }
            return result;
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
            catch (Exception e) { throw new Exception($"ExtractDataFromRequestJson : FAILED : {jsonString} \n {e.Message}"); }
        }


        public static async Task<JObject> ExtractDataFromRequestJson(HttpRequestData request)
        {
            //get xml string from caller
            var xmlString = (await request?.ReadAsStringAsync()) ?? "{}";

            //parse xml string todo needs catch here
            var parsedJson = JObject.Parse(xmlString);

            return parsedJson;
        }



        /// <summary>
        /// data received as json is converted to
        /// </summary>
        public static async Task<JsonDocument> ExtractDataFromRequestJsonToXml(HttpRequestData request)
        {
            string jsonString = "";

            try
            {
                //get raw string from caller
                jsonString = (await request?.ReadAsStringAsync()) ?? "{Root:\"Empty\"}";

                var parsedJson = JsonDocument.Parse(jsonString);
                return parsedJson;
            }
            //todo better logging
            catch (Exception e) { throw new Exception($"ExtractDataFromRequestJson : FAILED : {jsonString} \n {e.Message}"); }
        }


        public static Stream GenerateStreamFromString(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }



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
            var userDataListXml = await GetXmlFileFromAzureStorage(UserDataListXml, BlobContainerName);

            //look for user with matching email
            var foundUserXml = userDataListXml.Root?.Elements()
                .Where(userDataXml => userDataXml.Element("Email")?.Value == email)?
                .FirstOrDefault();

            //if user found, initialize xml and send that
            if (foundUserXml != null) { return UserData.FromXml(foundUserXml); }

            //if no user found, make new user and send that
            else
            {
                //create new user from google's data
                var newUser = new UserData(id, name, email);

                //add new user xml to main list
                await AddXElementToXDocumentAzure(newUser.ToXml(), UserDataListXml, BlobContainerName);

                //return newly created user to caller
                return newUser;
            }
        }

        /// <summary>
        /// Given a user data it will find mathcing user email and replace the existing UserData with inputed
        /// Note :
        /// - Uses UserDataList.xml
        /// </summary>
        public static async Task<UserData> UpdateUserData(string id, string name, string email)
        {
            //get user data list file (UserDataList.xml) Azure storage
            var userDataListXml = await GetXmlFileFromAzureStorage(UserDataListXml, BlobContainerName);

            //look for user with matching email
            var foundUserXml = userDataListXml.Root?.Elements()
                .Where(userDataXml => userDataXml.Element("Email")?.Value == email)?
                .FirstOrDefault();

            //if user found, initialize xml and send that
            if (foundUserXml != null) { return UserData.FromXml(foundUserXml); }

            //if no user found, make new user and send that
            else
            {
                //create new user from google's data
                var newUser = new UserData(id, name, email);

                //add new user xml to main list
                await AddXElementToXDocumentAzure(newUser.ToXml(), UserDataListXml, BlobContainerName);

                //return newly created user to caller
                return newUser;
            }
        }



        /// <summary>
        /// Gets any file at given url as string
        /// </summary>
        public static async Task<string> GetStringFileHttp(string url)
        {
            //get the data sender
            using var client = new HttpClient();

            //load xml event data files before hand to be used quickly later for search
            //get main horoscope prediction file (located in wwwroot)
            var fileString = await client.GetStringAsync(url);

            return fileString;
        }


        /// <summary>
        /// If start time and end time is same then will only return 1 time in list
        /// </summary>
        public static List<Time> GetTimeListFromRange(Time startTime, Time endTime, double precisionInHours)
        {
            //declare return value
            var timeList = new List<Time>();

            //create list
            for (var day = startTime; day.GetStdDateTimeOffset() <= endTime.GetStdDateTimeOffset(); day = day.AddHours(precisionInHours))
            {
                timeList.Add(day);
            }

            //return value
            return timeList;
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

        public static async Task UpdatePersonRecord(Person updatedPerson)
        {
            //get the person record that needs to be updated
            var personToUpdate = await FindPersonXMLById(updatedPerson.Id);

            //delete the previous person record,
            //and insert updated record in the same place
            personToUpdate?.ReplaceWith(updatedPerson.ToXml());

            //upload modified list file to storage
            var personListXmlDoc = await GetXmlFileFromAzureStorage(PersonListFile, BlobContainerName);
            await SaveXDocumentToAzure(personListXmlDoc, PersonListFile, BlobContainerName);
        }

        public static async Task UpdateRecordInDoc<T>(XElement updatedPersonXml, string cloudFileName) where T : IToXml
        {
            var allListXmlDoc = await GetXmlFileFromAzureStorage(cloudFileName, BlobContainerName);

            var updatedPerson = Person.FromXml(updatedPersonXml);

            //get the person record that needs to be updated
            var personToUpdate = await FindPersonXMLById(updatedPerson.Id);

            //delete the previous person record,
            //and insert updated record in the same place
            personToUpdate?.ReplaceWith(updatedPersonXml);

            //upload modified list file to storage
            await SaveXDocumentToAzure(allListXmlDoc, cloudFileName, BlobContainerName);
        }

        public static async Task<List<Person>> GetAllPersonList()
        {
            //get all person list from storage
            var personListXml = await GetXmlFileFromAzureStorage(PersonListFile, BlobContainerName);
            var allPersonList = personListXml.Root?.Elements();

            var returnList = Person.FromXml(allPersonList);

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

        public static void SendEmail(string fileName, string fileFormat, string receiverEmailAddress, Stream rawFileBytes)
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
            var contentType = Tools.StringToMimeType(fileFormat) ?? MediaTypeNames.Text.Plain; //if fail just plain noodle will do

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
                Console.WriteLine($"Email send operation failed with error code: {ex.ErrorCode}, message: {ex.Message}");
            }

            //-------------LOCAL FUNCS

            EmailClient getEmailClient()
            {
                //read the connection string
                var connectionString = Environment.GetEnvironmentVariable("AutoEmailerConnectString"); //vedastro-api-data

                //raise alarm if no connection string
                if (string.IsNullOrEmpty(connectionString)) { throw new Exception($"Failed to get connection string : AutoEmailerConnectString!"); }

                //sign in to email
                return new EmailClient(connectionString);
            }
        }

        //if user made profile while logged out then logs in, transfer the profiles created with visitor id to the new user id
        //if this is not done, then when user loses the visitor ID, they also loose access to the person profile
        public static async Task<bool> SwapUserId(string? visitorId, string? userId, string cloudXmlFile)
        {
            var allListXmlDoc = await APITools.GetXmlFileFromAzureStorage(cloudXmlFile, APITools.BlobContainerName);

            //filter out record by visitor id
            var visitorIdList = Tools.FindXmlByUserId(allListXmlDoc, visitorId);

            //if user made profile while logged out then logs in, transfer the profiles created with visitor id to the new user id
            //if this is not done, then when user loses the visitor ID, they also loose access to the person profile
            var loggedIn = userId != "101" && !(string.IsNullOrEmpty(userId));//already logged in if true
            var visitorProfileExists = visitorIdList.Any();

            if (loggedIn && visitorProfileExists)
            {
                //convert visitor to user
                foreach (var xmlRecord in visitorIdList)
                {
                    //get existing data
                    var existingOwners = xmlRecord?.Element("UserId")?.Value ?? "";

                    //replace visitor id with user id
                    var updatedOwners = existingOwners.Replace(visitorId, userId);

                    //check if data is valid, should not match
                    if (updatedOwners.Equals(existingOwners)) { throw new Exception("ID Swap Failed"); }

                    //pump value into local updated list
                    var cleaned = Tools.RemoveWhiteSpace(updatedOwners); //remove any space crept it
                    xmlRecord.Element("UserId")!.Value = cleaned;
                }

                //upload modified list file to storage
                await SaveXDocumentToAzure(allListXmlDoc, cloudXmlFile, BlobContainerName);

                //since heavy computation, log if happens
                APILogger.Data($"Profiles swapped : {visitorIdList.Count}", AppInstance.IncomingRequest);

                //return true of swap was done
                return true;
            }
            //false if no swap
            else { return false; }
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
        /// Uses name and birth year to generate human readable ID for a new person record
        /// created so that user can type ID direct into URL based on only memory of name and birth year
        /// </summary>
        public static async Task<string> GeneratePersonId(string personName, string birthYear)
        {
            //remove all space from name : Jamés Brown > JamésBrown
            var spaceLessName = Tools.RemoveWhiteSpace(personName);

            //almost done, name with birth year at the back
            var humanId = spaceLessName + birthYear;

            //check if ID is really unique, else it would need a number at the back 
            //try to find a person, if null then no new id is unique
            //jamesbrown and JamesBrown, both should by common sense work
            var idIsSafe = await CheckBothCase(humanId);

            //if id NOT safe, add nonce and try again, possible nonce has been used
            //JamésBrown > JamésBrown1
            var nonceCount = 1; //start nonce at 1
        TryAgain:
            var noncedId = humanId; //clear pre nonce if any 
            if (!idIsSafe)
            {
                //make unique
                noncedId += nonceCount;
                nonceCount++; //increment for next if needed
                //try again
                idIsSafe = await CheckBothCase(noncedId); ; //anybody with same id found?
                goto TryAgain;
            }

            //once control reaches here id should be all good
            return noncedId;


            //---------------LOCAL FUNCTIONS-------------------------------

            //check both case to allow user to make mistake of adding in
            //jamesbrown and JamesBrown, both should by common sense work
            async Task<bool> CheckBothCase(string checkThis)
            {
                var x = (await APITools.FindPersonXMLById(checkThis)) == null;
                var y = (await APITools.FindPersonXMLById(checkThis.ToLower())) == null;
                return x || y;
            }




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
        public static HttpResponseData SendGifToCaller(byte[] gif, HttpRequestData incomingRequest)
        {
            //send image back to caller
            var response = incomingRequest.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "image/gif");
            //place in response body
            response.WriteBytes(gif);
            return response;
        }


        //gets the exact width of a text based on Font size & type
        //used to generate nicely fitting background for text
        public static double GetTextWidthPx(string textInput)
        {
            //TODO handle max & min
            //set max & min width background
            //const int maxWidth = 70;
            //backgroundWidth = backgroundWidth > maxWidth ? maxWidth : backgroundWidth;
            //const int minWidth = 30;
            //backgroundWidth = backgroundWidth > minWidth ? minWidth : backgroundWidth;


            SizeF size;
            using (var graphics = Graphics.FromHwnd(IntPtr.Zero))
            {
                size = graphics.MeasureString(textInput, new Font("Calibri", 12, FontStyle.Regular, GraphicsUnit.Pixel));
            }
            var widthPx = Math.Round(size.Width);

            return widthPx;
        }

        public static string GetCallerId(string userId,string visitorId )
        {
            var IsLoggedIn = userId != "101";
            if (IsLoggedIn)
            {
                return userId;
            }
            //if user NOT logged in then take his visitor ID as caller id
            else
            {
                return visitorId;
            }

        }

    }
}