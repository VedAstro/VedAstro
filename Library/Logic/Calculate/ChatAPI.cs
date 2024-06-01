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

namespace VedAstro.Library
{
    public static class ChatAPI
    {
        //auth details to talk to Azure OpenAI
        static Uri azureOpenAIResourceUri = new("https://openaimodelserver.openai.azure.com/");
        static AzureKeyCredential azureOpenAIApiKey = new(Environment.GetEnvironmentVariable("AzureOpenAIAPIKey"));
        static string azureMetaLlama3APIKey = Environment.GetEnvironmentVariable("AzureMetaLlama3APIKey");
        static string azureMistralLargeAPIKey = Environment.GetEnvironmentVariable("AzureMistralLargeAPIKey");
        static OpenAIClient client = new(azureOpenAIResourceUri, azureOpenAIApiKey);


        /// <summary>
        /// Gets sub-lord at a given longitude (planet or house cusp) 
        /// </summary>
        /// <param name="longitude">planet or house cusp longitude</param>
        public static async Task<JObject> SendMessageHoroscope(Time birthTime, string userQuestion, string sessionId = "")
        {
            var replyText = "";

            //#0 is question valid and sane?
            var isValid = IsQuestionValid(userQuestion, out replyText);

            if (!isValid) { return PackageReply(replyText); } //end here if not valid


            //#1 is question about Electional astrology?
            var isElectional = IsElectionalAstrology(userQuestion, out replyText);

            if (isElectional) { return PackageReply(replyText); } //end here if is Electional


            //#2 answer question about Horoscope

            //#1 pick out most relevant predictions to question
            var tasks1 = new List<Task<string>>
            {
                PickOutMostRelevantPredictions_MistralLarge(birthTime, userQuestion),
                PickOutMostRelevantPredictions_GPT4(birthTime, userQuestion),
                PickOutMostRelevantPredictions_MetaLlama3(birthTime, userQuestion)
            };
            var relevantPredictions = await Task.WhenAll(tasks1); //call all models in parallel

            //#2 answer question based on relevant predictions
            var tasks2 = new List<Task<string>>
            {
                AnswerQuestionDirectly(relevantPredictions[0], userQuestion),
                AnswerQuestionDirectly_MetaLlama3(relevantPredictions[0], userQuestion),
            };
            var answerLevel1 = await Task.WhenAll(tasks2); //call all models in parallel

            //#3 simplify answer
            var answerLevel2 = await ImproveFinalAnswer(answerLevel1[0], userQuestion);

            //#4 remove disclaimers if any
            var answerLevel3 = await RemoveAnyDisclaimers(answerLevel2, userQuestion);

            //#5 highlight relevant keywords
            var answerLevel4 = await HighlightKeywords(answerLevel3, userQuestion);

            //package data


            //----------------------------------LOCALS---------------------------------------

            bool IsElectionalAstrology(string s, out string replyText1)
            {
                //#0 extract time range if any
                var tasks0 = new List<Task<string>>
                {
                    ExtractTimeRange_MistralLarge(birthTime, userQuestion),
                };
                var timeRange = Task.WhenAll(tasks0).Result.FirstOrDefault(); //call all models in parallel

                //check if make sense of it
                var isValid = IsTimeRangeValid(timeRange, out TimeRange timeRangeParsed);

                //if not valid end here
                if (!isValid) { replyText1 = ""; return false; }

                //muhurtha question confirmed
                //generate chart
                var chartData = EventsChartManager.GenerateEventsChartForChat(birthTime, timeRangeParsed);

                //


            }

            //check and parses
            bool IsTimeRangeValid(string timeRangeRaw, out TimeRange timeRange)
            {
                JObject parsed;
                try
                {
                    parsed = JObject.Parse(timeRangeRaw);
                    var startRaw = parsed["start"].Value<string>();
                    var endRaw = parsed["end"].Value<string>();

                    var startString = $"";
                    var startTime =
                        
                        new Time(startString, birthTime.GetGeoLocation());
                    var endString = $"";
                    var endTime = new Time(endString, birthTime.GetGeoLocation());

                    timeRange = new TimeRange();
                    return true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }


                throw new NotImplementedException();
            }



            JObject PackageReply(string p0)
            {
                var reply = new JObject
                {
                    { "SessionId", GetSessionId() },
                    { "Message", p0 }
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

            //#0 extract time range if any
            var tasks0 = new List<Task<string>>
            {
                ExtractTimeRange_MistralLarge(maleBirthTime, userQuestion),
            };
            var timeRange = await Task.WhenAll(tasks0); //call all models in parallel

            //#1 pick out most relevant predictions to question
            var tasks1 = new List<Task<string>>
            {
                PickOutMostRelevantPredictions_MistralLarge(maleBirthTime, userQuestion),
                PickOutMostRelevantPredictions_GPT4(maleBirthTime, userQuestion),
                PickOutMostRelevantPredictions_MetaLlama3(maleBirthTime, userQuestion)
            };
            var relevantPredictions = await Task.WhenAll(tasks1); //call all models in parallel

            //#2 answer question based on relevant predictions
            var tasks2 = new List<Task<string>>
            {
                AnswerQuestionDirectly(relevantPredictions[0], userQuestion),
                AnswerQuestionDirectly_MetaLlama3(relevantPredictions[0], userQuestion),
            };
            var answerLevel1 = await Task.WhenAll(tasks2); //call all models in parallel

            //#3 simplify answer
            var answerLevel2 = await ImproveFinalAnswer(answerLevel1[0], userQuestion);

            //#4 remove disclaimers if any
            var answerLevel3 = await RemoveAnyDisclaimers(answerLevel2, userQuestion);

            //#5 highlight relevant keywords
            var answerLevel4 = await HighlightKeywords(answerLevel3, userQuestion);

            //package data
            var reply = new JObject
            {
                { "SessionId", GetSessionId() },
                { "Message", answerLevel4 }
            };

            return reply;


            //----------------------------------LOCALS---------------------------------------

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



        //---------------------------------------------------PRIVATE------------------------------------------------------

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

        private static async Task<string> HighlightKeywords(string answerLevel3, string userQuestion)
        {
            return answerLevel3;
            throw new NotImplementedException();
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

        private static async Task<string> PickOutMostRelevantPredictions_MistralLarge(Time birthTime, string userQuestion)
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
            //var xx = HoroscopePrediction.
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
                    max_tokens = 8192,
                    temperature = 0.8,
                    top_p = 0.1,
                    safe_prompt = "false"
                };

                // ""max_tokens"": 8192,
                //                  ""temperature"": 0.7,
                //                  ""top_p"": 1,
                //                  ""safe_prompt"": ""false""

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
                            content = @"{start:""01/01/2014"", end:""31/12/2014""}"
                        },
                        new
                        {
                            role = "user",
                            content = $"extract time range from this question : ```will i get married in this year?```"
                        },
                        new
                        {
                            role = "assistant",
                            content = @"{start:""01/01/{thisYear}"", end:""31/12/{thisYear}""}"
                        },
                        new
                        {
                            role = "user",
                            content = $"extract time range from this question : ```when will i get married?```"
                        },
                        new
                        {
                            role = "assistant",
                            content = @"{start:""01/01/{birthYear+18}"", end:""31/12/{birthYear+50}""}"
                        },
                        new
                        {
                            role = "user",
                            content = $"extract time range from this question : ```should i do business next year?```"
                        },
                        new
                        {
                            role = "assistant",
                            content = @"{start:""01/01/{nextYear}"", end:""31/12/{nextYear}""}"
                        },
                        new
                        {
                            role = "user",
                            content = $"extract time range from this question : ```when will i get a job?```"
                        },
                        new
                        {
                            role = "assistant",
                            content = @"{start:""01/01/{birthYear+18}"", end:""31/12/{birthYear+40}""}"
                        },
                        new
                        {
                            role = "user",
                            content = $"extract time range from this question : ```describe my life chart```"
                        },
                        new
                        {
                            role = "assistant",
                            content = @"{start:""null"", end:""null""}"
                        },
                        new
                        {
                            role = "user",
                            content = $"extract time range from this question : ```Are there any financial highs and lows predicted in my birth chart from the year 2024 to 2027?```"
                        },
                        new
                        {
                            role = "assistant",
                            content = @"{start:""01/01/2024"", end:""31/12/2027""}"
                        },
                        new
                        {
                            role = "user",
                            content = $"extract time range from this question : ```My DOB 6th Jan 1982 , Time 5:15 AM and Location Nagpur Maharastra India , I need my job switch information```"
                        },
                        new
                        {
                            role = "assistant",
                            content = @"{start:""null"", end:""null""}"
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
