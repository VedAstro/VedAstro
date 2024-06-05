using Azure.AI.OpenAI;
using Azure;
using SwissEphNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using Fizzler;

namespace VedAstro.Library
{
    public static class ChatAPI
    {
        //auth details to talk to Azure OpenAI
        static Uri azureOpenAIResourceUri = new("https://openaimodelserver.openai.azure.com/");
        static AzureKeyCredential azureOpenAIApiKey = new(Environment.GetEnvironmentVariable("AzureOpenAIAPIKey"));
        static string azureMetaLlama3APIKey = Environment.GetEnvironmentVariable("AzureMetaLlama3APIKey");
        static string azureMistralLargeAPIKey = Environment.GetEnvironmentVariable("AzureMistralLargeAPIKey");
        static string azureMistralSmallAPIKey = Environment.GetEnvironmentVariable("AzureMistralSmallAPIKey");
        static OpenAIClient client = new(azureOpenAIResourceUri, azureOpenAIApiKey);



        /// <summary>
        /// Gets sub-lord at a given longitude (planet or house cusp) 
        /// </summary>
        /// <param name="longitude">planet or house cusp longitude</param>
        public static async Task<JObject> SendMessageHoroscope(Time birthTime, string userQuestion, string sessionId = "")
        {

            //#             +> FOLLOW-UP --> specialized lite llm call
            //#             |
            //# QUESTION ---+
            //#             |
            //#             +> UNRELATED --> full llama raq synthesis



            //         USER QUESTION          
            //               │                
            //               ▼                
            //         #0 IS VALID?            
            //               │                
            //           ◄───┴───►            
            //#1 ELECTIONAL       #2 HOROSCOPE    

            var replyText = "";
            var followupQuestions = new List<string> { "Why?", "How?", "Tell me more..." };

            //#0 is question valid and sane?
            var isValid = IsQuestionValid(userQuestion, out replyText);
            if (!isValid) { return PackageReply(userQuestion, replyText, followupQuestions); } //end here if not valid


            //#1 is question about Electional astrology?
            var isElectional = IsElectionalAstrology(birthTime, userQuestion, out replyText);
            if (isElectional) { return PackageReply(userQuestion, replyText, followupQuestions); } //end here if is Electional


            //#2 answer question about Horoscope
            replyText = await IsHoroscopeAstrology(birthTime, userQuestion);
            return PackageReply(userQuestion, replyText, followupQuestions);



            //----------------------------------LOCALS---------------------------------------



            JObject PackageReply(string userQuestion, string text, List<string> followupQuestions)
            {

                //using user question and LLM make answer more readable in HTML, bolding, paragraphing...etc
                string textHtml = ChatAPI.HighlightKeywords_MistralSmall(userQuestion, text).Result;

                var reply = new JObject
                {
                    { "sessionId", GetSessionId() },
                    { "text", text },
                    { "textHtml", textHtml },
                    { "textHash", Tools.GetStringHashCode(text) },
                    { "followupQuestions", new JArray(followupQuestions) }
                };

                return reply;

            }


            //if session id provided use back same, else create a new one
            string GetSessionId()
            {
                if (string.IsNullOrEmpty(sessionId))
                {
                    sessionId = Tools.GenerateId();
                }
                return sessionId;
            }

        }


        public static async Task<JObject> SendMessageMatch(Time maleBirthTime, Time femaleBirthTime, string userQuestion, string sessionId = "")
        {

            throw new NotImplementedException();

        }



        //---------------------------------------------------PRIVATE------------------------------------------------------
        private static bool IsElectionalAstrology(Time birthTime, string userQuestion, out string replyText)
        {
            //#0 extract time range if any
            var tasks0 = new List<Task<string>>
            {
                ExtractTimeRange_MistralLarge(birthTime, userQuestion),
            };
            var timeRangeDataRaw = Task.WhenAll(tasks0).Result.FirstOrDefault(); //call all models in parallel
            var timeRangeDataJson = JObject.Parse(timeRangeDataRaw);

            //see if LLM said it is valid Muhurtha question
            bool.TryParse(timeRangeDataJson["valid"].ToString(), out bool isElectionalQuestion);

            bool isValid = false; //default to Horoscope

            if (isElectionalQuestion)
            {
                //double check if time can be extracted properly
                isValid = IsTimeRangeValid(timeRangeDataJson, out TimeRange timeRangeParsed);
            }


            //if not valid end here
            if (!isValid) { replyText = ""; return false; }
            else
            {
                //todo 
                replyText = "Muhurtha feature still being coded. Please come back next week.";
                return true;
            }
            //muhurtha question confirmed
            //generate chart
            //var chartData = EventsChartManager.GenerateEventsChartForChat(birthTime, timeRangeParsed);

            //


            //----------------------------------LOCALS---------------------------------------


            //check and parses
            bool IsTimeRangeValid(JObject timeRangeJson, out TimeRange timeRange)
            {
                try
                {
                    var startRaw = timeRangeJson["start"].Value<string>();
                    var endRaw = timeRangeJson["end"].Value<string>();

                    var start = new Time($"00:00 {startRaw} {birthTime.StdTimezoneText}", birthTime.GetGeoLocation());
                    var end = new Time($"00:00 {endRaw} {birthTime.StdTimezoneText}", birthTime.GetGeoLocation());

                    timeRange = new TimeRange(start, end);
                    return true;
                }
                catch (Exception e)
                {
                    timeRange = null;
                    return false;
                }
            }

        }

        private static async Task<string> IsHoroscopeAstrology(Time birthTime, string userQuestion)
        {
            //#0 split predictions into sizeable chunks
            var predictionListChunks = ChatAPI.GetPredictionAsChunks(birthTime);

            //#1 pick out most relevant predictions to question
            var tasks1 = new List<Task<string>>
            {
                PickOutMostRelevantPredictions_MistralSmall(birthTime, userQuestion, predictionListChunks[0]),
                //PickOutMostRelevantPredictions_MistralLarge(birthTime, userQuestion, predictionListChunks[0]),
                //PickOutMostRelevantPredictions_GPT4(birthTime, userQuestion),
                //PickOutMostRelevantPredictions_MetaLlama3(birthTime, userQuestion)
            };
            var relevantPredictions = await Task.WhenAll(tasks1); //call all models in parallel

            //#2 answer question based on relevant predictions
            var tasks2 = new List<Task<string>>
            {
                //AnswerQuestionDirectly(relevantPredictions[0], userQuestion),
                //AnswerQuestionDirectly_MetaLlama3(relevantPredictions[0], userQuestion),
                AnswerQuestionDirectly_MistralSmall(relevantPredictions[0], userQuestion),
            };
            var answerLevel1 = await Task.WhenAll(tasks2); //call all models in parallel

            //#3 simplify answer
            //var answerLevel2 = await ImproveFinalAnswer_MistralLarge(answerLevel1[0], userQuestion);
            //var answerLevel2 = await ImproveFinalAnswer_MistralSmall(answerLevel1[0], userQuestion);

            //#4 remove disclaimers if any
            //todo check if contains disclaimers then remove
            //var answerLevel3 = await RemoveAnyDisclaimers_MistralLarge(answerLevel2, userQuestion);

            return answerLevel1[0];
        }

        private static List<string> GetPredictionAsChunks(Time birthTime)
        {
            //calculate predictions for current person
            var predictionList = Tools.GetHoroscopePrediction(birthTime);

            //extract only name and description and place nicely
            var jsonString = $"[{string.Join(",",
                predictionList.Select(o => $"{{\"Name\":\"{o.FormattedName}\",\"Description\":\"{o.Description}\"}}"))}]";

            //nicely create convert to text form

            //convert prediction to text 
            //var predictJson = Tools.ListToJson(predictionList);
            //var predictText = predictJson.ToString(Formatting.None);

            var chunkedList = new List<string>() { jsonString };

            return chunkedList;
        }

        private static async Task<string> AnswerQuestionDirectly_MetaLlama3(string answerLevelN, string userQuestion)
        {
            var handler = new HttpClientHandler()
            {
                ClientCertificateOptions = ClientCertificateOption.Manual,
                ServerCertificateCustomValidationCallback =
                        (httpRequestMessage, cert, cetChain, policyErrors) => { return true; }
            };

            var sysMessage =
                $"analyse life description text, answer question directly\n" +
                $"QUESTION:\n{userQuestion}\n" +
                $"LIFE DESCRIPTION:\n{answerLevelN}";

            // Remove invalid characters
            sysMessage = System.Text.RegularExpressions.Regex.Unescape(sysMessage);

            using (var client = new HttpClient(handler))
            {
                var requestBodyObject = new
                {
                    messages = new[]
                    {
                        new
                        {
                            role = "user",
                            content = sysMessage
                        }
                    },
                    max_tokens = 2000,
                    temperature = 0.8,
                    top_p = 0.1,
                    best_of = 1,
                    presence_penalty = 0,
                    use_beam_search = "false",
                    ignore_eos = "false",
                    skip_special_tokens = "false",
                    logprobs = "false"
                };

                string requestBody = JsonConvert.SerializeObject(requestBodyObject);

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", azureMetaLlama3APIKey);
                client.BaseAddress = new Uri("https://Meta-Llama-3-70B-Instruct-ydbrc-serverless.westus.inference.ai.azure.com/v1/chat/completions");

                var content = new StringContent(requestBody);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                HttpResponseMessage response = await client.PostAsync("", content);

                if (response.IsSuccessStatusCode)
                {
                    //get full reply and parse it
                    string fullReplyRaw = await response.Content.ReadAsStringAsync();
                    var fullReply = new LlamaReplyJson(fullReplyRaw);

                    //return only message text
                    var replyMessage = fullReply.Choices.FirstOrDefault().Message.Content;
                    return replyMessage;
                }
                else
                {
                    //TODO better logging
                    Console.WriteLine(string.Format("The request failed with status code: {0}", response.StatusCode));

                    // Print the headers - they include the requert ID and the timestamp,
                    // which are useful for debugging the failure
                    Console.WriteLine(response.Headers.ToString());

                    string responseContent = await response.Content.ReadAsStringAsync();
                    return responseContent;
                }
            }
        }

        private static async Task<string> AnswerQuestionDirectly_MistralSmall(string answerLevelN, string userQuestion)
        {
            var handler = CreateHttpClientHandler();
            var sysMessage = PrepareSystemMessage();
            var requestBody = CreateRequestBody(sysMessage);
            var content = new StringContent(requestBody);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            using (var client = new HttpClient(handler))
            {
                HttpResponseMessage response = await PostRequestAsync(client, content);
                return await ProcessResponseAsync(response);
            }


            //------------------------------- LOCALS --------------

            HttpClientHandler CreateHttpClientHandler()
            {
                return new HttpClientHandler()
                {
                    ClientCertificateOptions = ClientCertificateOption.Manual,
                    ServerCertificateCustomValidationCallback = (httpRequestMessage, cert, cetChain, policyErrors) => true
                };
            }

            string PrepareSystemMessage()
            {
                var sysMessage =
                    $"analyse life description text, answer question directly\n" +
                    $"QUESTION:\n{userQuestion}\n" +
                    $"LIFE DESCRIPTION:\n{answerLevelN}";

                return System.Text.RegularExpressions.Regex.Unescape(sysMessage);
            }

            string CreateRequestBody(string sysMessage)
            {
                var requestBodyObject = new
                {
                    messages = new[]
                    {
                new
                {
                    role = "user",
                    content = sysMessage
                }
            },
                    max_tokens = 2000,
                    temperature = 0.5,
                    top_p = 0.5,
                    safe_prompt = "false"
                };

                return JsonConvert.SerializeObject(requestBodyObject);
            }

            async Task<HttpResponseMessage> PostRequestAsync(HttpClient client, StringContent content)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", azureMistralSmallAPIKey);
                client.BaseAddress = new Uri("https://Mistral-small-xcvuv-serverless.westus.inference.ai.azure.com/v1/chat/completions");

                return await client.PostAsync("", content);
            }

            async Task<string> ProcessResponseAsync(HttpResponseMessage response)
            {
                if (response.IsSuccessStatusCode)
                {
                    string fullReplyRaw = await response.Content.ReadAsStringAsync();
                    var fullReply = new LlamaReplyJson(fullReplyRaw);

                    return fullReply.Choices.FirstOrDefault().Message.Content;
                }
                else
                {
                    Console.WriteLine($"The request failed with status code: {response.StatusCode}");
                    Console.WriteLine(response.Headers.ToString());

                    string responseContent = await response.Content.ReadAsStringAsync();
                    return responseContent;
                }
            }
        }


        private static async Task<string> HighlightKeywords(string userQuestion, string answerLevel3)
        {
            return answerLevel3;
            throw new NotImplementedException();
        }

        private static async Task<string> HighlightKeywords_MistralLarge(string answerText, string userQuestion)
        {
            var handler = new HttpClientHandler()
            {
                ClientCertificateOptions = ClientCertificateOption.Manual,
                ServerCertificateCustomValidationCallback =
                        (httpRequestMessage, cert, cetChain, policyErrors) => { return true; }
            };


            //var sysMessage = "Provide a confident answer without any disclaimers for the following text:\n" +
            //                 $"```TEXT\n{answerLevel1}```";
            var sysMessage = "Follow rules:\n" +
                             "1. Output ANSWER text in HTML format for use between <p> tag element\n" +
                             "2. Highlight relevant words and phrases in ANSWER that is related to QUESTION\n" +
                             "3. Break long text and organize ANSWER text structure for easy readability." +
                             "4. All html element must be valid to be placed inside <p> tag element\n";


            using (var client = new HttpClient(handler))
            {
                var requestBodyObject = new
                {
                    messages = new[]
                    {
                        new
                        {
                            role= "system",
                            content= sysMessage
                        },
                        new
                        {
                            role= "user",
                            content= $"QUESTION:\n\n{userQuestion}\n\nANSWER:\n\n{answerText}"
                        },

                    },
                    max_tokens = 300,
                    temperature = 0.8,
                    top_p = 0.4,
                    safe_prompt = "false"
                };


                string requestBody = JsonConvert.SerializeObject(requestBodyObject);

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", azureMistralLargeAPIKey);
                client.BaseAddress = new Uri("https://Mistral-large-cahy-serverless.westus.inference.ai.azure.com/v1/chat/completions");

                var content = new StringContent(requestBody);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                HttpResponseMessage response = await client.PostAsync("", content);

                if (response.IsSuccessStatusCode)
                {
                    //get full reply and parse it
                    string fullReplyRaw = await response.Content.ReadAsStringAsync();
                    var fullReply = new LlamaReplyJson(fullReplyRaw);

                    //return only message text
                    var replyMessage = fullReply.Choices.FirstOrDefault().Message.Content;
                    return replyMessage;
                }
                else
                {
                    //TODO better logging
                    Console.WriteLine(string.Format("The request failed with status code: {0}", response.StatusCode));

                    // Print the headers - they include the requert ID and the timestamp,
                    // which are useful for debugging the failure
                    Console.WriteLine(response.Headers.ToString());

                    string responseContent = await response.Content.ReadAsStringAsync();
                    return responseContent;
                }
            }
        }

        private static async Task<string> HighlightKeywords_MistralSmall(string answerText, string userQuestion)
        {
            var handler = CreateHttpClientHandler();
            var sysMessage = PrepareSystemMessage();
            var requestBody = CreateRequestBody(sysMessage);
            var content = new StringContent(requestBody);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            using (var client = new HttpClient(handler))
            {
                HttpResponseMessage response = await PostRequestAsync(client, content);
                return await ProcessResponseAsync(response);
            }


            //------------------------------- LOCALS --------------

            HttpClientHandler CreateHttpClientHandler()
            {
                return new HttpClientHandler()
                {
                    ClientCertificateOptions = ClientCertificateOption.Manual,
                    ServerCertificateCustomValidationCallback = (httpRequestMessage, cert, cetChain, policyErrors) => true
                };
            }

            string PrepareSystemMessage()
            {
                var sysMessage = "Follow rules:\n" +
                                 "1. Output ANSWER text in HTML format for use between <p> tag element\n" +
                                 "2. Highlight relevant words and phrases in ANSWER that is related to QUESTION\n" +
                                 "3. Break long text and organize ANSWER text structure for easy readability." +
                                 "4. All html element must be valid to be placed inside <p> tag element\n\n" +
                                 $"QUESTION:\n\n{userQuestion}\n\nANSWER:\n\n{answerText}";


                return System.Text.RegularExpressions.Regex.Unescape(sysMessage);
            }

            string CreateRequestBody(string sysMessage)
            {
                var requestBodyObject = new
                {
                    messages = new[]
                    {
                new
                {
                    role = "user",
                    content = sysMessage
                }
            },
                    max_tokens = 1000,
                    temperature = 0.2,
                    top_p = 0.1,
                    safe_prompt = "false"
                };

                return JsonConvert.SerializeObject(requestBodyObject);
            }

            async Task<HttpResponseMessage> PostRequestAsync(HttpClient client, StringContent content)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", azureMistralSmallAPIKey);
                client.BaseAddress = new Uri("https://Mistral-small-xcvuv-serverless.westus.inference.ai.azure.com/v1/chat/completions");

                return await client.PostAsync("", content);
            }

            async Task<string> ProcessResponseAsync(HttpResponseMessage response)
            {
                if (response.IsSuccessStatusCode)
                {
                    string fullReplyRaw = await response.Content.ReadAsStringAsync();
                    var fullReply = new LlamaReplyJson(fullReplyRaw);

                    return fullReply.Choices.FirstOrDefault().Message.Content;
                }
                else
                {
                    Console.WriteLine($"The request failed with status code: {response.StatusCode}");
                    Console.WriteLine(response.Headers.ToString());

                    string responseContent = await response.Content.ReadAsStringAsync();
                    return responseContent;
                }
            }
        }


        private static async Task<string> RemoveAnyDisclaimers(string rawAnswer, string userQuestion)
        {
            //prepare LLM call
            var chatCompletionsOptions = new ChatCompletionsOptions()
            {
                DeploymentName = "chatapi-mk8",
                Messages = {
                    new ChatRequestSystemMessage(
                        "Provide a confident answer without any disclaimers for the following text:\n" +
                        $"```TEXT\n{rawAnswer}```")
                },
                Temperature = (float)0.7,
                MaxTokens = 300,
                NucleusSamplingFactor = (float)0.95,
                FrequencyPenalty = 0,
                PresencePenalty = (float)0.0,
                ResponseFormat = ChatCompletionsResponseFormat.Text
            };

            //make the call
            Response<ChatCompletions> response = await client.GetChatCompletionsAsync(chatCompletionsOptions);
            ChatResponseMessage responseMessage = response.Value.Choices[0].Message;

            //get reply out
            var aiReplyText = responseMessage.Content;
            return aiReplyText;

        }

        private static async Task<string> RemoveAnyDisclaimers_MistralLarge(string answerLevel1, string userQuestion)
        {
            var handler = new HttpClientHandler()
            {
                ClientCertificateOptions = ClientCertificateOption.Manual,
                ServerCertificateCustomValidationCallback =
                        (httpRequestMessage, cert, cetChain, policyErrors) => { return true; }
            };


            var sysMessage = "Provide a confident answer without any disclaimers for the following text:\n" +
                             $"```TEXT\n{answerLevel1}```";


            using (var client = new HttpClient(handler))
            {
                var requestBodyObject = new
                {
                    messages = new[]
                    {
                        new
                        {
                            role = "user",
                            content = sysMessage
                        }
                    },
                    max_tokens = 300,
                    temperature = 0.8,
                    top_p = 0.4,
                    safe_prompt = "false"
                };


                string requestBody = JsonConvert.SerializeObject(requestBodyObject);

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", azureMistralLargeAPIKey);
                client.BaseAddress = new Uri("https://Mistral-large-cahy-serverless.westus.inference.ai.azure.com/v1/chat/completions");

                var content = new StringContent(requestBody);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                HttpResponseMessage response = await client.PostAsync("", content);

                if (response.IsSuccessStatusCode)
                {
                    //get full reply and parse it
                    string fullReplyRaw = await response.Content.ReadAsStringAsync();
                    var fullReply = new LlamaReplyJson(fullReplyRaw);

                    //return only message text
                    var replyMessage = fullReply.Choices.FirstOrDefault().Message.Content;
                    return replyMessage;
                }
                else
                {
                    //TODO better logging
                    Console.WriteLine(string.Format("The request failed with status code: {0}", response.StatusCode));

                    // Print the headers - they include the requert ID and the timestamp,
                    // which are useful for debugging the failure
                    Console.WriteLine(response.Headers.ToString());

                    string responseContent = await response.Content.ReadAsStringAsync();
                    return responseContent;
                }
            }
        }


        private static async Task<string> ImproveFinalAnswer(string answerLevel1, string userQuestion)
        {
            var sysMessage =
                $"summarize below raw answer as reply to this question, '{userQuestion}?'\n" +
                $"summary is easier to understand,'\n" +
                $"RAW ANSWER:\n{answerLevel1}";

            var chatCompletionsOptions = new ChatCompletionsOptions()
            {
                DeploymentName = "chatapi-mk8",
                Messages = {
                    new ChatRequestSystemMessage(sysMessage)
                },
                Temperature = (float)0.7,
                MaxTokens = 300,
                NucleusSamplingFactor = (float)0.95,
                FrequencyPenalty = 0,
                PresencePenalty = (float)0.0,
            };

            Response<ChatCompletions> response = await client.GetChatCompletionsAsync(chatCompletionsOptions);
            ChatResponseMessage responseMessage = response.Value.Choices[0].Message;


            //return final message from AI
            return responseMessage.Content;
        }

        private static async Task<string> ImproveFinalAnswer_MistralLarge(string answerLevelN, string userQuestion)
        {
            var handler = new HttpClientHandler()
            {
                ClientCertificateOptions = ClientCertificateOption.Manual,
                ServerCertificateCustomValidationCallback =
                        (httpRequestMessage, cert, cetChain, policyErrors) => { return true; }
            };


            var sysMessage =
                $"summarize below raw answer as reply to this question, '{userQuestion}?'\n" +
                $"summary is easier to understand,'\n" +
                $"RAW ANSWER:\n{answerLevelN}";


            using (var client = new HttpClient(handler))
            {
                var requestBodyObject = new
                {
                    messages = new[]
                    {
                        new
                        {
                            role = "user",
                            content = sysMessage
                        }

                    },
                    max_tokens = 300,
                    temperature = 0.8,
                    top_p = 0.4,
                    safe_prompt = "false"
                };


                string requestBody = JsonConvert.SerializeObject(requestBodyObject);

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", azureMistralLargeAPIKey);
                client.BaseAddress = new Uri("https://Mistral-large-cahy-serverless.westus.inference.ai.azure.com/v1/chat/completions");

                var content = new StringContent(requestBody);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                // WARNING: The 'await' statement below can result in a deadlock
                // if you are calling this code from the UI thread of an ASP.Net application.
                // One way to address this would be to call ConfigureAwait(false)
                // so that the execution does not attempt to resume on the original context.
                // For instance, replace code such as:
                //      result = await DoSomeTask()
                // with the following:
                //      result = await DoSomeTask().ConfigureAwait(false)
                HttpResponseMessage response = await client.PostAsync("", content);

                if (response.IsSuccessStatusCode)
                {
                    //get full reply and parse it
                    string fullReplyRaw = await response.Content.ReadAsStringAsync();
                    var fullReply = new LlamaReplyJson(fullReplyRaw);

                    //return only message text
                    var replyMessage = fullReply.Choices.FirstOrDefault().Message.Content;
                    return replyMessage;
                }
                else
                {
                    //TODO better logging
                    Console.WriteLine(string.Format("The request failed with status code: {0}", response.StatusCode));

                    // Print the headers - they include the requert ID and the timestamp,
                    // which are useful for debugging the failure
                    Console.WriteLine(response.Headers.ToString());

                    string responseContent = await response.Content.ReadAsStringAsync();
                    return responseContent;
                }
            }
        }

        private static async Task<string> ImproveFinalAnswer_MistralSmall(string answerLevelN, string userQuestion)
        {
            var handler = CreateHttpClientHandler();
            var sysMessage = PrepareSystemMessage();
            var requestBody = CreateRequestBody(sysMessage);
            var content = new StringContent(requestBody);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            using (var client = new HttpClient(handler))
            {
                HttpResponseMessage response = await PostRequestAsync(client, content);
                return await ProcessResponseAsync(response);
            }


            //------------------------------- LOCALS --------------

            HttpClientHandler CreateHttpClientHandler()
            {
                return new HttpClientHandler()
                {
                    ClientCertificateOptions = ClientCertificateOption.Manual,
                    ServerCertificateCustomValidationCallback = (httpRequestMessage, cert, cetChain, policyErrors) => true
                };
            }

            string PrepareSystemMessage()
            {
                var sysMessage =
                    $"summarize below raw answer as reply to this question, '{userQuestion}?'\n" +
                    $"summary is easier to understand,'\n" +
                    $"RAW ANSWER:\n{answerLevelN}";

                return System.Text.RegularExpressions.Regex.Unescape(sysMessage);
            }

            string CreateRequestBody(string sysMessage)
            {
                var requestBodyObject = new
                {
                    messages = new[]
                    {
                new
                {
                    role = "user",
                    content = sysMessage
                }
            },
                    max_tokens = 300,
                    temperature = 0.8,
                    top_p = 0.5,
                    safe_prompt = "false"
                };

                return JsonConvert.SerializeObject(requestBodyObject);
            }

            async Task<HttpResponseMessage> PostRequestAsync(HttpClient client, StringContent content)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", azureMistralSmallAPIKey);
                client.BaseAddress = new Uri("https://Mistral-small-xcvuv-serverless.westus.inference.ai.azure.com/v1/chat/completions");

                return await client.PostAsync("", content);
            }

            async Task<string> ProcessResponseAsync(HttpResponseMessage response)
            {
                if (response.IsSuccessStatusCode)
                {
                    string fullReplyRaw = await response.Content.ReadAsStringAsync();
                    var fullReply = new LlamaReplyJson(fullReplyRaw);

                    return fullReply.Choices.FirstOrDefault().Message.Content;
                }
                else
                {
                    Console.WriteLine($"The request failed with status code: {response.StatusCode}");
                    Console.WriteLine(response.Headers.ToString());

                    string responseContent = await response.Content.ReadAsStringAsync();
                    return responseContent;
                }
            }
        }


        private static async Task<string> PickOutMostRelevantPredictions_GPT4(Time birthTime, string userQuestion)
        {
            //calculate predictions for current person
            var predictionList = Tools.GetHoroscopePrediction(birthTime);

            //convert prediction to text 
            var predictJson = Tools.ListToJson(predictionList);
            var predictText = predictJson.ToString(Formatting.None);

            //prepare LLM call
            var sysMessage = "Output only JSON.\n" +
                                    //"From the below horoscope predictions list in JSON format.\n" +
                                    $"Return all predictions that is relevant to the question, '{userQuestion}'." +
                                    "Sort based on relevance, most relevant at top\n" +
                                    $"```JSON\n{predictText}```";

            var chatCompletionsOptions = new ChatCompletionsOptions()
            {
                DeploymentName = "chatapi-mk8",
                Messages = {
                    new ChatRequestSystemMessage(sysMessage)
                },
                Temperature = (float)0.2,
                MaxTokens = 4096,
                NucleusSamplingFactor = (float)0.95,
                FrequencyPenalty = 0,
                PresencePenalty = (float)0.0,
                ResponseFormat = ChatCompletionsResponseFormat.JsonObject
            };

            //make the call
            Response<ChatCompletions> response = await client.GetChatCompletionsAsync(chatCompletionsOptions);
            ChatResponseMessage responseMessage = response.Value.Choices[0].Message;

            //get reply out
            var aiReplyText = responseMessage.Content;
            return aiReplyText;
        }

        private static async Task<string> PickOutMostRelevantPredictions_MetaLlama3(Time birthTime, string userQuestion)
        {
            var handler = new HttpClientHandler()
            {
                ClientCertificateOptions = ClientCertificateOption.Manual,
                ServerCertificateCustomValidationCallback =
                        (httpRequestMessage, cert, cetChain, policyErrors) => { return true; }
            };

            //calculate predictions for current person
            var predictionList = Tools.GetHoroscopePrediction(birthTime);

            //convert prediction to text 
            var predictJson = Tools.ListToJson(predictionList);
            var predictText = predictJson.ToString(Formatting.None);


            //prepare LLM call
            var sysMessage = "Output only JSON.\n" +
                             //"From the below horoscope predictions list in JSON format.\n" +
                             $"Return all predictions that is relevant to the question, '{userQuestion}'." +
                             "Sort based on relevance, most relevant at top\n" +
                             $"```JSON\n{predictText}```";

            // Remove invalid characters
            sysMessage = System.Text.RegularExpressions.Regex.Unescape(sysMessage);

            using (var client = new HttpClient(handler))
            {
                var requestBodyObject = new
                {
                    messages = new[]
                    {
                        new
                        {
                            role = "user",
                            content = sysMessage
                        }
                    },
                    max_tokens = 3000,
                    temperature = 0.8,
                    top_p = 0.1,
                    best_of = 1,
                    presence_penalty = 0,
                    use_beam_search = "false",
                    ignore_eos = "false",
                    skip_special_tokens = "false",
                    logprobs = "false"
                };

                string requestBody = JsonConvert.SerializeObject(requestBodyObject);

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", azureMetaLlama3APIKey);
                client.BaseAddress = new Uri("https://Meta-Llama-3-70B-Instruct-ydbrc-serverless.westus.inference.ai.azure.com/v1/chat/completions");

                var content = new StringContent(requestBody);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                // WARNING: The 'await' statement below can result in a deadlock
                // if you are calling this code from the UI thread of an ASP.Net application.
                // One way to address this would be to call ConfigureAwait(false)
                // so that the execution does not attempt to resume on the original context.
                // For instance, replace code such as:
                //      result = await DoSomeTask()
                // with the following:
                //      result = await DoSomeTask().ConfigureAwait(false)
                HttpResponseMessage response = await client.PostAsync("", content);

                if (response.IsSuccessStatusCode)
                {
                    //get full reply and parse it
                    string fullReplyRaw = await response.Content.ReadAsStringAsync();
                    var fullReply = new LlamaReplyJson(fullReplyRaw);

                    //return only message text
                    var replyMessage = fullReply.Choices.FirstOrDefault().Message.Content;
                    return replyMessage;
                }
                else
                {
                    //TODO better logging
                    Console.WriteLine(string.Format("The request failed with status code: {0}", response.StatusCode));

                    // Print the headers - they include the requert ID and the timestamp,
                    // which are useful for debugging the failure
                    Console.WriteLine(response.Headers.ToString());

                    string responseContent = await response.Content.ReadAsStringAsync();
                    return responseContent;
                }
            }
        }

        private static async Task<string> PickOutMostRelevantPredictions_MistralLarge(Time birthTime, string userQuestion, string predictText)
        {
            var handler = new HttpClientHandler()
            {
                ClientCertificateOptions = ClientCertificateOption.Manual,
                ServerCertificateCustomValidationCallback =
                        (httpRequestMessage, cert, cetChain, policyErrors) => { return true; }
            };



            //prepare LLM call
            var sysMessage = "Output only JSON.\n" +
                             //"From the below horoscope predictions list in JSON format.\n" +
                             $"Return all predictions that is relevant to the question, '{userQuestion}'." +
                             "Sort based on relevance, most relevant at top\n" +
                             $"```JSON\n{predictText}```";

            // Remove invalid characters
            sysMessage = System.Text.RegularExpressions.Regex.Unescape(sysMessage);

            using (var client = new HttpClient(handler))
            {
                var requestBodyObject = new
                {
                    messages = new[]
                    {
                        new
                        {
                            role = "user",
                            content = sysMessage
                        }
                    },
                    max_tokens = 8192,
                    temperature = 0.8,
                    top_p = 0.1,
                    safe_prompt = "false"
                };


                string requestBody = JsonConvert.SerializeObject(requestBodyObject);

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", azureMistralLargeAPIKey);
                client.BaseAddress = new Uri("https://Mistral-large-cahy-serverless.westus.inference.ai.azure.com/v1/chat/completions");

                var content = new StringContent(requestBody);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                // WARNING: The 'await' statement below can result in a deadlock
                // if you are calling this code from the UI thread of an ASP.Net application.
                // One way to address this would be to call ConfigureAwait(false)
                // so that the execution does not attempt to resume on the original context.
                // For instance, replace code such as:
                //      result = await DoSomeTask()
                // with the following:
                //      result = await DoSomeTask().ConfigureAwait(false)
                HttpResponseMessage response = await client.PostAsync("", content);

                if (response.IsSuccessStatusCode)
                {
                    //get full reply and parse it
                    string fullReplyRaw = await response.Content.ReadAsStringAsync();
                    var fullReply = new LlamaReplyJson(fullReplyRaw);

                    //return only message text
                    var replyMessage = fullReply.Choices.FirstOrDefault().Message.Content;
                    return replyMessage;
                }
                else
                {
                    //TODO better logging
                    Console.WriteLine(string.Format("The request failed with status code: {0}", response.StatusCode));

                    // Print the headers - they include the requert ID and the timestamp,
                    // which are useful for debugging the failure
                    Console.WriteLine(response.Headers.ToString());

                    string responseContent = await response.Content.ReadAsStringAsync();
                    return responseContent;
                }
            }
        }


        private static async Task<string> PickOutMostRelevantPredictions_MistralSmall(Time birthTime, string userQuestion, string predictText)
        {
            var handler = CreateHttpClientHandler();
            var sysMessage = PrepareSystemMessage();
            var requestBody = CreateRequestBody(sysMessage);
            var content = new StringContent(requestBody);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            using (var client = new HttpClient(handler))
            {
                HttpResponseMessage response = await PostRequestAsync(client, content);
                return await ProcessResponseAsync(response);
            }


            //------------------------------- LOCALS --------------

            HttpClientHandler CreateHttpClientHandler()
            {
                return new HttpClientHandler()
                {
                    ClientCertificateOptions = ClientCertificateOption.Manual,
                    ServerCertificateCustomValidationCallback = (httpRequestMessage, cert, cetChain, policyErrors) => true
                };
            }

            string PrepareSystemMessage()
            {
                var sysMessage = $"Output only JSON.\nReturn all predictions that is relevant to the question, '{userQuestion}'.Sort based on relevance, most relevant at top\n```JSON\n{predictText}```";
                return System.Text.RegularExpressions.Regex.Unescape(sysMessage);
            }

            string CreateRequestBody(string sysMessage)
            {
                var requestBodyObject = new
                {
                    messages = new[]
                    {
                new
                {
                    role = "user",
                    content = sysMessage
                }
            },
                    max_tokens = 8192,
                    temperature = 0.8,
                    top_p = 0.1,
                    safe_prompt = "false"
                };

                return JsonConvert.SerializeObject(requestBodyObject);
            }

            async Task<HttpResponseMessage> PostRequestAsync(HttpClient client, StringContent content)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", azureMistralSmallAPIKey);
                client.BaseAddress = new Uri("https://Mistral-small-xcvuv-serverless.westus.inference.ai.azure.com/v1/chat/completions");

                return await client.PostAsync("", content);
            }

            async Task<string> ProcessResponseAsync(HttpResponseMessage response)
            {
                if (response.IsSuccessStatusCode)
                {
                    string fullReplyRaw = await response.Content.ReadAsStringAsync();
                    var fullReply = new LlamaReplyJson(fullReplyRaw);

                    return fullReply.Choices.FirstOrDefault().Message.Content;
                }
                else
                {
                    Console.WriteLine($"The request failed with status code: {response.StatusCode}");
                    Console.WriteLine(response.Headers.ToString());

                    string responseContent = await response.Content.ReadAsStringAsync();
                    return responseContent;
                }
            }
        }
        

        private static async Task<string> ExtractTimeRange_MistralLarge(Time birthTime, string userQuestion)
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
                        new
                        {
                            role = "user",
                            content = $"extract time range from this question : ```will i get married in 2014?```"
                        },
                        new
                        {
                            role = "assistant",
                            content = @"{valid:""true"", start:""01/01/2014"", end:""31/12/2014""}"
                        },
                        new
                        {
                            role = "user",
                            content = $"extract time range from this question : ```will i get married in this year?```"
                        },
                        new
                        {
                            role = "assistant",
                            content = @"{valid:""true"", start:""01/01/{thisYear}"", end:""31/12/{thisYear}""}"
                        },
                        new
                        {
                            role = "user",
                            content = $"extract time range from this question : ```when will i get married?```"
                        },
                        new
                        {
                            role = "assistant",
                            content = @"{valid:""true"", start:""01/01/{birthYear+18}"", end:""31/12/{birthYear+50}""}"
                        },
                        new
                        {
                            role = "user",
                            content = $"extract time range from this question : ```should i do business next year?```"
                        },
                        new
                        {
                            role = "assistant",
                            content = @"{valid:""true"", start:""01/01/{nextYear}"", end:""31/12/{nextYear}""}"
                        },
                        new
                        {
                            role = "user",
                            content = $"extract time range from this question : ```when will i get a job?```"
                        },
                        new
                        {
                            role = "assistant",
                            content = @"{valid:""true"", start:""01/01/{birthYear+18}"", end:""31/12/{birthYear+40}""}"
                        },
                        new
                        {
                            role = "user",
                            content = $"extract time range from this question : ```describe my life chart```"
                        },
                        new
                        {
                            role = "assistant",
                            content = @"{valid:""false"", start:""null"", end:""null""}"
                        },
                        new
                        {
                            role = "user",
                            content = $"extract time range from this question : ```Are there any financial highs and lows predicted in my birth chart from the year 2024 to 2027?```"
                        },
                        new
                        {
                            role = "assistant",
                            content = @"{valid:""true"", start:""01/01/2024"", end:""31/12/2027""}"
                        },
                        new
                        {
                            role = "user",
                            content = $"extract time range from this question : ```My DOB 6th Jan 1982 , Time 5:15 AM and Location Nagpur Maharastra India , I need my job switch information```"
                        },
                        new
                        {
                            role = "assistant",
                            content = @"{valid:""false"", start:""null"", end:""null""}"
                        },
                        new
                        {
                            role = "user",
                            content = $"extract time range from this question : ```{userQuestion}```"
                        },

                    },
                    max_tokens = 8192,
                    temperature = 0.8,
                    top_p = 0.1,
                    safe_prompt = "false"
                };


                string requestBody = JsonConvert.SerializeObject(requestBodyObject);

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", azureMistralLargeAPIKey);
                client.BaseAddress = new Uri("https://Mistral-large-cahy-serverless.westus.inference.ai.azure.com/v1/chat/completions");

                var content = new StringContent(requestBody);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                // WARNING: The 'await' statement below can result in a deadlock
                // if you are calling this code from the UI thread of an ASP.Net application.
                // One way to address this would be to call ConfigureAwait(false)
                // so that the execution does not attempt to resume on the original context.
                // For instance, replace code such as:
                //      result = await DoSomeTask()
                // with the following:
                //      result = await DoSomeTask().ConfigureAwait(false)
                HttpResponseMessage response = await client.PostAsync("", content);

                if (response.IsSuccessStatusCode)
                {
                    //get full reply and parse it
                    string fullReplyRaw = await response.Content.ReadAsStringAsync();
                    var fullReply = new LlamaReplyJson(fullReplyRaw);

                    //return only message text
                    var replyMessage = fullReply.Choices.FirstOrDefault().Message.Content;
                    return replyMessage;
                }
                else
                {
                    //TODO better logging
                    Console.WriteLine(string.Format("The request failed with status code: {0}", response.StatusCode));

                    // Print the headers - they include the requert ID and the timestamp,
                    // which are useful for debugging the failure
                    Console.WriteLine(response.Headers.ToString());

                    string responseContent = await response.Content.ReadAsStringAsync();
                    return responseContent;
                }
            }
        }

        private static bool IsQuestionValid(string userQuestion, out string replyText)
        {
            // Get the result from the CheckQuestionValidity method
            var tasks1 = new List<Task<string>>
            {
                CheckQuestionValidity_MistralLarge(userQuestion),
            };
            var validityCheckResult = Task.WhenAll(tasks1).Result.FirstOrDefault(); //call all models in parallel


            // Parse the result into a JObject
            var parsedResult = JObject.Parse(validityCheckResult);

            // Extract the "valid" field value as a string
            var isValidText = parsedResult["valid"]?.Value<string>() ?? "false";

            // Convert the string to a boolean
            bool.TryParse(isValidText, out bool isQuestionValid);

            // Extract the "reply" field value as a string, if it exists
            // If it doesn't exist, assign an empty string
            replyText = parsedResult["reply"]?.Value<string>() ?? "";

            // Return the validity of the question
            return isQuestionValid;
        }

        private static async Task<string> CheckQuestionValidity_MistralLarge(string userQuestion)
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

                        new
                        {
                            role = "user",
                            content = $"hi"
                        },
                        new
                        {
                            role = "assistant",
                            content = @"{valid:""false"", reply:""yes hi, please ask questions""}"
                        },
                        new
                        {
                            role = "user",
                            content = $"how do you work?"
                        },
                        new
                        {
                            role = "assistant",
                            content = @"{valid:""false"", reply:""i've analysed predictions about your horoscope, and can answer questions about it""}"
                        },
                        new
                        {
                            role = "user",
                            content = $"hi"
                        },
                        new
                        {
                            role = "assistant",
                            content = @"{valid:""false"", reply:""yes hi, let's talk about your horoscope""}"
                        },
                        new
                        {
                            role = "user",
                            content = $"I want to learn astrology"
                        },
                        new
                        {
                            role = "assistant",
                            content = @"{valid:""false"", reply:""i'm here to help you understand a particular horoscope, not teach you astrology text, sorry""}"
                        },
                        new
                        {
                            role = "user",
                            content = $"what can you do?"
                        },
                        new
                        {
                            role = "assistant",
                            content = @"{valid:""false"", reply:""i'm here to help you understand a particular horoscope""}"
                        },
                        new
                        {
                            role = "user",
                            content = $"how to generate vedic astro chart "
                        },
                        new
                        {
                            role = "assistant",
                            content = @"{valid:""false"", reply:""to get help on using vedastro please contact us, i'm here to help you understand a particular horoscope""}"
                        },
                        new
                        {
                            role = "user",
                            content = $"my birth details are date/month/year at location"
                        },
                        new
                        {
                            role = "assistant",
                            content = @"{valid:""false"", reply:""i don't need your birth details, i've already analysed your horoscope, please ask questions directly""}"
                        },
                        new
                        {
                            role = "user",
                            content = $"can you create my chart based on Jan 1, 2001, Delhi 10am"
                        },
                        new
                        {
                            role = "assistant",
                            content = @"{valid:""false"", reply:""i don't need your birth details, i've already analysed your horoscope, please ask questions directly""}"
                        },
                        new
                        {
                            role = "user",
                            content = $"describe yogas in my life"
                        },
                        new
                        {
                            role = "assistant",
                            content = @"{valid:""true"", reply:""""}"
                        },
                        new
                        {
                            role = "user",
                            content = $"character of modi"
                        },
                        new
                        {
                            role = "assistant",
                            content = @"{valid:""true"", reply:""""}"
                        },
                        new
                        {
                            role = "user",
                            content = $"wil he win in 2024 elections ?"
                        },
                        new
                        {
                            role = "assistant",
                            content = @"{valid:""true"", reply:""""}"
                        },
                        new
                        {
                            role = "user",
                            content = $"Can you tell me about money flow throughout the year 2024?"
                        },
                        new
                        {
                            role = "assistant",
                            content = @"{valid:""true"", reply:""""}"
                        },
                        new
                        {
                            role = "user",
                            content = $"Do my stars indicate any entrepreneurial talents or inclinations?"
                        },
                        new
                        {
                            role = "assistant",
                            content = @"{valid:""true"", reply:""""}"
                        },
                        new
                        {
                            role = "user",
                            content = $"{userQuestion}"
                        },

                    },
                    max_tokens = 8192,
                    temperature = 1, //max creativity
                    top_p = 1, //max creativity
                    safe_prompt = "false"
                };


                string requestBody = JsonConvert.SerializeObject(requestBodyObject);

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", azureMistralLargeAPIKey);
                client.BaseAddress = new Uri("https://Mistral-large-cahy-serverless.westus.inference.ai.azure.com/v1/chat/completions");

                var content = new StringContent(requestBody);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                HttpResponseMessage response = await client.PostAsync("", content);

                if (response.IsSuccessStatusCode)
                {
                    //get full reply and parse it
                    string fullReplyRaw = await response.Content.ReadAsStringAsync();
                    var fullReply = new LlamaReplyJson(fullReplyRaw);

                    //return only message text
                    var replyMessage = fullReply.Choices.FirstOrDefault().Message.Content;
                    return replyMessage;
                }
                else
                {
                    //TODO better logging
                    Console.WriteLine(string.Format("The request failed with status code: {0}", response.StatusCode));

                    // Print the headers - they include the requert ID and the timestamp,
                    // which are useful for debugging the failure
                    Console.WriteLine(response.Headers.ToString());

                    string responseContent = await response.Content.ReadAsStringAsync();
                    return responseContent;
                }
            }

        }


        private static async Task<string> AnswerQuestionDirectly(string relevantPredictions, string userQuestion)
        {
            var sysMessage =
                $"analyse life description text, answer question directly\n" +
                $"QUESTION:\n{userQuestion}\n\n" +
                $"LIFE DESCRIPTION:\n{relevantPredictions}";

            var chatCompletionsOptions = new ChatCompletionsOptions()
            {
                DeploymentName = "chatapi-mk8",
                Messages = {
                    new ChatRequestSystemMessage(sysMessage),
                },
                Temperature = (float)0.7,
                MaxTokens = 2000,
                NucleusSamplingFactor = (float)0.95,
                FrequencyPenalty = 0,
                PresencePenalty = (float)0.0,
            };

            Response<ChatCompletions> response = await client.GetChatCompletionsAsync(chatCompletionsOptions);
            ChatResponseMessage responseMessage = response.Value.Choices[0].Message;


            //return final message from AI
            var unrefinedSugar = responseMessage.Content;

            return unrefinedSugar;
        }
    }
}
