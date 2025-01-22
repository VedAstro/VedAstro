using System.Net.Mime;
using System.Text;
using Newtonsoft.Json;
using VedAstro.Library;

namespace API
{
    public class MessageAPI
    {


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
            var slackUserMessageWebHook = Secrets.Get("SLACK_EMAIL_WEBHOOK");

            var response = await httpClient.PostAsync(slackUserMessageWebHook, content);

            var responseData = await response.Content.ReadAsStringAsync();

            Console.WriteLine(responseData);
        }



    }
}
