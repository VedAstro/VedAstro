using System.Net.Mime;
using System.Text;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Newtonsoft.Json;

namespace API
{
    public class MessageAPI
    {

        [Function("getmessagelist")]
        public static async Task<HttpResponseData> GetMessageList([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestData incomingRequest)
        {

            try
            {
                //get message list from storage
                var messageListXml = await APITools.GetXmlFileFromAzureStorage(APITools.MessageListFile, APITools.BlobContainerName);


                //send task list to caller
                return APITools.PassMessage(messageListXml.Root, incomingRequest);


            }
            catch (Exception e)
            {
                //log error
                await APILogger.Error(e, incomingRequest);
                //format error nicely to show user
                return APITools.FailMessage(e, incomingRequest);
            }

        }

        [Function("addmessage")]
        public static async Task<HttpResponseData> AddMessage([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestData incomingRequest)
        {

            try
            {
                //get new message data out of incoming request
                //note: inside new person xml already contains user id
                var newMessageXml = await APITools.ExtractDataFromRequestXml(incomingRequest);

                //add new message to main list
                await APITools.AddXElementToXDocumentAzure(newMessageXml, APITools.MessageListFile, APITools.BlobContainerName);

                //notify admin
                await SendMessageToSlack(newMessageXml.Element("Email")?.Value ?? "Empty", newMessageXml.Element("Text")?.Value ?? "Empty");

                return APITools.PassMessage(incomingRequest);

            }
            catch (Exception e)
            {
                //log error
                await APILogger.Error(e, incomingRequest);

                //format error nicely to show user
                return APITools.FailMessage(e, incomingRequest);
            }
        }


        private static async Task SendMessageToSlack(string fromEmail, string msgContent)
        {

            object model = new
            {
                username = "Acmebot",
                attachments = new[]
                {
                    new
                    {
                        text = "New Message",
                        color = "good",
                        fields = new object[]
                        {
                            new
                            {
                                title = "From",
                                value= fromEmail,
                                @short = false
                            },
                            new
                            {
                                title = "Content",
                                value = msgContent,
                                @short = false
                            }
                        }
                    }
                }
            };

            var httpClient = new HttpClient();

            var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, MediaTypeNames.Application.Json);

            //get the connection string stored separately (for security reasons)
            //note: dark art secrets are in local.settings.json
            var slackUserMessageWebHook = Environment.GetEnvironmentVariable("SLACK_EMAIL_WEBHOOK"); //vedastro-api-data

            var response = await httpClient.PostAsync(slackUserMessageWebHook, content);

            var responseData = await response.Content.ReadAsStringAsync();

            Console.WriteLine(responseData);
        }



    }
}
