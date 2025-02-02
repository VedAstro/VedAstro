


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
using Azure.Storage.Blobs;
using SuperConvert.Extensions;
using VedAstro.Library;
using Person = VedAstro.Library.Person;

using MimeDetective;

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
            MessageJson("Fail", Tools.ExceptionToJSON(payloadException), req);

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

        /// <summary>
        /// Gets all person profiles from DB, has option to skip getting life events (save DB calls & faster)
        /// default gets life events
        /// </summary>
        public static List<Person> GetAllPersonList(bool skipLifeEvents = false)
        {
            //get all
            var foundCalls = AzureTable.PersonList.Query<PersonListEntity>();

            var returnList = new List<Person>();
            foreach (var call in foundCalls)
            {
                returnList.Add(Person.FromAzureRow(call, skipLifeEvents));
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






        public static string JsonToCsv(string jsonData)
        {

            string csvPath = jsonData.ToCsv();


            Console.WriteLine(csvPath);

            return csvPath;
        }


        /// <summary>
        /// Given a file or string convertible data, send it to caller accordingly
        /// </summary>
        public static HttpResponseData SendAnyToCaller(string calculatorName, dynamic rawPlanetData, HttpRequestData incomingRequest)
        {
            //then it is a file
            if (rawPlanetData is byte[] rawFileData)
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