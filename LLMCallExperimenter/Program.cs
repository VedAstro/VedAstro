using System.Net.Http.Headers;
using System;
using System.IO;
using System.Security.Cryptography;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ExCSS;

namespace LLMCallExperimenter
{
    internal class Program
    {

        static IConfigurationRoot config = new ConfigurationBuilder()
            .AddJsonFile("secrets.json", optional: true, reloadOnChange: true)
            .Build();

        static string GPT4oEndpoint = config["GPT4oEndpoint"];
        static string GPT4oApiKey = config["GPT4oApiKey"];


        static void Main(string[] args)
        {

            //make LLM call, get reply text only
            var result = MakeCall().Result;

            Console.WriteLine(result);

            // Keep the console open
            Console.ReadLine();
        }




        public static async Task<string> MakeCall()
        {
            var handler = new HttpClientHandler()
            {
                ClientCertificateOptions = ClientCertificateOption.Manual,
                ServerCertificateCustomValidationCallback =
                           (httpRequestMessage, cert, cetChain, policyErrors) => { return true; }
            };


            using (var client = new HttpClient(handler))
            {
                var requestBodyObject = new
                {
                    messages = new[]
                    {
                    //new { role = "system", content = "given person name & birth year output marriage data" },
                    new { role = "system", content = "given person name & birth year output only JSON body data in valid ```json" +
                                                     @"{
  ""body"": {
    ""type"": ""ectomorph/mesomorph/endomorph"",
    ""breastSize"": ""small/medium/large"",
    ""height"": ""short/average/tall"",
    ""weight"": ""light/medium/heavy"",
    ""dataCredibility"": ""low/medium/high"",
  }
}```" },
                    //new { role = "user", content = $"Marilyn Monroe born 1926"}
                    new { role = "user", content = $"angelina jolie born 1975"}
                    },

                    max_tokens = 4096,
                    temperature = 0.1,
                    top_p = 0.1,
                    presence_penalty = 0,
                    frequency_penalty = 0
                };

                var requestBody = JsonConvert.SerializeObject(requestBodyObject);

                client.BaseAddress = new Uri(GPT4oEndpoint);

                var content = new StringContent(requestBody);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                content.Headers.Add("api-key", GPT4oApiKey);

                var response = await client.PostAsync("", content);

                //get full reply and parse it
                var fullReplyRaw = await response.Content.ReadAsStringAsync();
                var fullReply = new Gpt4ReplyJson(fullReplyRaw);

                //return only message text
                var replyMessage = fullReply.Choices.FirstOrDefault().Message.Content;
                return replyMessage;
            }
        }


    }
}
