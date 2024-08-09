using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace VedAstro.Library
{

    /// <summary>
    /// place where embeddings are created
    /// </summary>
    public static class LLMEmbeddingManager
    {
        public static HttpClientHandler Handler { get; } = new HttpClientHandler
        {
            ClientCertificateOptions = ClientCertificateOption.Manual,
            ServerCertificateCustomValidationCallback = (httpRequestMessage, cert, cetChain, policyErrors) => true
        };


        /// <summary>
        /// Given any text will output raw embedding vectors from text-embedding-ada-002 API call
        /// </summary>
        public static async Task<string> GetEmbeddingsForText_CohereEmbedV3(string[] inputTextList, string inputType)
        {
            JObject finalResult = new JObject();
            var textsPerBatch = 95;

            using var client = new HttpClient(Handler)
            {
                DefaultRequestHeaders = { Authorization = new AuthenticationHeaderValue("Bearer", Secrets.Get("azureCohereCommandRPlusAPIKey")) },
                BaseAddress = new Uri("https://Cohere-embed-v3-english-bkovp-serverless.westus.inference.ai.azure.com/v1/embed")
            };

            for (int i = 0; i < inputTextList.Length; i += textsPerBatch)
            {
                var batch = inputTextList.Skip(i).Take(textsPerBatch).ToArray();
                var response = await MakeApiCall(client, batch, inputType);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    var batchResult = JObject.Parse(result);
                    finalResult.Merge(batchResult, new JsonMergeSettings { MergeArrayHandling = MergeArrayHandling.Concat });
                }
                else
                {
                    Console.WriteLine($"The request failed with status code: {response.StatusCode}");
                    Console.WriteLine(response.Headers.ToString());
                    Console.WriteLine(await response.Content.ReadAsStringAsync());
                }
            }

            var allEmbeddings = finalResult["embeddings"];
            var embedVectors = allEmbeddings[0].ToString(Formatting.None);


            return embedVectors;


             static async Task<HttpResponseMessage> MakeApiCall(HttpClient client, string[] textArray, string inputType)
            {
                // Create the request body
                var requestBodyRaw = new
                {
                    model = "embed-english-v3.0",
                    texts = textArray,
                    input_type = inputType,
                    truncate = "NONE"
                };

                // Convert the request body to JSON and create the content for the request
                var json = JsonConvert.SerializeObject(requestBodyRaw);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Make the API call
                return await client.PostAsync("", content);
            }

        }


        public static async Task<string> GetEmbeddingsForText_Ada002(string textToEmbed)
        {
            const string endpoint = "https://openaimodelserver.openai.azure.com/openai/deployments/text-embedder/embeddings?api-version=2023-05-15";

            using var client = new HttpClient();

            client.DefaultRequestHeaders.Add("api-key", Secrets.Get("AzureOpenAIAPIKey"));

            var requestBody = new { input = textToEmbed };
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(endpoint, content);
            var result = await response.Content.ReadAsStringAsync();

            //convert to JSON and extract out only the vectors
            var parsedJson = JToken.Parse(result);
            var vectors = parsedJson["data"][0]["embedding"].ToString(Formatting.None);

            return vectors;
        }

        public static async Task<double[]> GetEmbeddingsArrayForText_Ada002(string textToEmbed)
        {
            
            const string endpoint = "https://openaimodelserver.openai.azure.com/openai/deployments/text-embedding-ada-002/embeddings?api-version=2023-05-15";

            using var client = new HttpClient();

            client.DefaultRequestHeaders.Add("api-key", Secrets.Get("AzureOpenAIAPIKey"));

            var requestBody = new { input = textToEmbed };
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(endpoint, content);
            var result = await response.Content.ReadAsStringAsync();

            //convert to JSON and extract out only the vectors
            var parsedJson = JToken.Parse(result);
            var vectors = parsedJson["data"][0]["embedding"];

            var newQueryEmbeds = vectors.Select(jv => (double)jv).ToArray();


            return newQueryEmbeds;
        }

    }
}
