using Azure.AI.OpenAI;
using Azure;
using SwissEphNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using Fizzler;
using Azure.Data.Tables;
using System.Collections.Concurrent;

namespace VedAstro.Library
{
    public class PredictionSettings
    {
        public string ServerUrl { get; set; }
        public string ApiKey { get; set; }
        public double MaxTokens { get; set; }
        public double Temperature { get; set; }
        public double TopP { get; set; }
        public object[] SysMessage { get; set; }
    }


    public static class ChatAPI
    {
        //auth details to talk to Azure OpenAI
        static Uri azureOpenAIResourceUri = new("https://openaimodelserver.openai.azure.com/");
        static AzureKeyCredential azureOpenAIApiKey = new(Environment.GetEnvironmentVariable("AzureOpenAIAPIKey"));
        static string azureMetaLlama3APIKey = Environment.GetEnvironmentVariable("AzureMetaLlama3APIKey");
        static string azureMistralLargeAPIKey = Environment.GetEnvironmentVariable("AzureMistralLargeAPIKey");
        static string azureCohereCommandRPlusAPIKey = Environment.GetEnvironmentVariable("AzureCohereCommandRPlusAPIKey");
        static string azureMistralSmallAPIKey = Environment.GetEnvironmentVariable("AzureMistralSmallAPIKey");
        static OpenAIClient client = new(azureOpenAIResourceUri, azureOpenAIApiKey);

        private static string chatTableconnectionString = Environment.GetEnvironmentVariable("CENTRAL_API_STORAGE_CONNECTION_STRING");
        private static List<string> followupQuestions = new List<string> { "Why?", "How?", "Tell me more..." };
        private static TableClient chatTableClient = new TableClient(chatTableconnectionString, "ChatMessage");


        //#             +> FOLLOW-UP --> specialized lite llm call
        //#             |
        //# QUESTION ---+> GIVE FEEDBACK --> 
        //#             |
        //#             +> UNRELATED --> full llama raq synthesis



        public static async Task<JObject> SendMessageHoroscopeFollowUp(Time birthTime, string followUpQuestion = "", string primaryAnswerHash = "",
            string userId = "", string sessionId = "")
        {
            //log the follow-up first
            SaveToTable(new ChatMessageEntity(sessionId, birthTime, followUpQuestion, "Human", userId));

            //based on hash get full question as pure text
            var primaryAnswerData = ReadFromTable(sessionId, primaryAnswerHash);

            var primaryAnswer = primaryAnswerData.Text;

            //based on primary answer, back track to primary question
            var primaryQuestionMsgNumber = primaryAnswerData.MessageNumber - 1; // go up 1 step
            var primaryQuestionData = ReadFromTableByMessageNumber(sessionId, primaryQuestionMsgNumber);

            var primaryQuestion = primaryQuestionData.Text;

            //get predictions used as before
            var horoscopePredictions = ChatAPI.GetPredictionAsChunks(birthTime)[0];

            //get reply from LLM 🚅
            var aiReply = await AnswerFollowUpHoroscopeQuestion_CohereCommandRPlus(primaryQuestion,
                 primaryAnswer, horoscopePredictions, followUpQuestion);

            //log the AI reply
            var textHash = SaveToTable(new ChatMessageEntity(sessionId, birthTime, aiReply, "AI", userId)).RowKey;

            var noFollowUpAnyMore = new List<string>(); //no follow up to a follow-up
            return PackageReply("", aiReply, textHash, noFollowUpAnyMore, sessionId);
        }

        public static async Task<JObject> HoroscopeChatFeedback(string answerHash, int feedbackScore)
        {

            //find answer record that user has asked to rate
            string filter = $"RowKey eq '{answerHash}'";

            var recordFound = chatTableClient.Query<ChatMessageEntity>(filter).FirstOrDefault();

            //combine rating
            recordFound.Rating += feedbackScore;

            //save back to DB
            chatTableClient.UpsertEntity(recordFound, TableUpdateMode.Replace);

            //# say thanks 🙏
            //# NOTE: DO NOT tell the user explicitly to give more feedback
            //# psychology 101 : give them the sincere motivation to help instead -> better quality/quantity
            //#if we tell the user explicitly, we increase the probability of deterministic failure, pushing the user into 1 of 2 camps
            var aiReply =
                "Congratulation!🫡\n You have just helped improve astrology worldwide🌍\n I have now memorized your feedback,🧠\n now on all my answer will take your feedback into consideration.\n Thank you so much for the rating🙏\n";

            //<h5>🥇 AI Contributor Score : <b>{contribution_score*10}</b></h5>
            var aiHtmlReply = """
                              	
                                  Congratulation!🫡<br>
                                  You have just helped improve astrology worldwide🌍<br><br>
                                  I have now <b>memorized your feedback</b>,🧠<br>
                                  now on all my answer will take your feedback into consideration.<br><br>
                                  Thank you so much for the rating🙏<br><br>
                              """;



            var noFollowUpAnyMore = new List<string>(); //no follow up to a follow-up
            var sameSessionId = recordFound.PartitionKey; //use back same session ID
            var noFeedbackCommand = new List<string>(){ "noFeedback" }; //stop the feedback on feedback loop
            var randomHash = Tools.GenerateId(10); //has to be unique else will interfere with client rendering
            return PackageReply("", aiReply, randomHash, noFollowUpAnyMore, sameSessionId, aiHtmlReply, noFeedbackCommand);

        }

        /// <summary>
        /// Gets sub-lord at a given longitude (planet or house cusp) 
        /// </summary>
        /// <param name="longitude">planet or house cusp longitude</param>
        public static async Task<JObject> SendMessageHoroscope(Time birthTime, string userQuestion, string sessionId, string userId)
        {

            //save incoming message to log
            // If session id is empty, generate a new one
            if (string.IsNullOrEmpty(sessionId)) { sessionId = Tools.GenerateId(); }
            SaveToTable(new ChatMessageEntity(sessionId, birthTime, userQuestion, "Human", userId));

            var replyText = "";


            //         USER QUESTION          
            //               │                
            //               ▼                
            //         #0 IS VALID?            
            //               │                
            //           ◄───┴───►            
            //#1 ELECTIONAL       #2 HOROSCOPE    


            //#0 is question valid and sane?
            //var isValid = IsQuestionValid(userQuestion, out replyText);
            //if (!isValid) { return PackageReply(userQuestion, replyText, followupQuestions); } //end here if not valid


            ////#1 is question about Electional astrology?
            //var isElectional = IsElectionalAstrology(birthTime, userQuestion, out replyText);
            //if (isElectional) { return PackageReply(userQuestion, replyText, followupQuestions); } //end here if is Electional


            //#2 answer question about Horoscope
            replyText = await IsHoroscopeAstrology(birthTime, userQuestion);

            //save AI's reply
            var textHash = SaveToTable(new ChatMessageEntity(sessionId, birthTime, replyText, "AI", userId)).RowKey;

            //pack nicely and send to user
            return PackageReply(userQuestion, replyText, textHash, followupQuestions, sessionId);
        }

        public static int GetLastMessageNumberNumberFromSessionId(string sessionId)
        {

            // If session id already exists, check Azure data tables and get the message number of the latest record by timestamp
            Expression<Func<ChatMessageEntity, bool>> expression = call => call.PartitionKey == sessionId;

            // Execute search
            var recordFound = chatTableClient?.Query(expression)
                ?.OrderByDescending(call => call.Timestamp)
                ?.FirstOrDefault();

            // If no record is found, start with message number 0
            var messageNumber = recordFound?.MessageNumber ?? 0; //set 0 so caller can easily add 1 on top

            return messageNumber;
        }


        public static async Task<JObject> SendMessageMatch(Time maleBirthTime, Time femaleBirthTime, string userQuestion, string sessionId = "")
        {

            throw new NotImplementedException();

        }




        //---------------------------------------------------PRIVATE------------------------------------------------------

        private static async Task<string> AnswerFollowUpHoroscopeQuestion_CohereCommandRPlus(string primaryQuestion, string primaryAnswer, string horoscopePredictions, string followUpQuestion)
        {


            var sysMessageArray = new[]
            {
                new
                {
                    role = "system",
                    content = $"an over confident astrologer, use context text\n" +
                              $"CONTEXT:\n\n{horoscopePredictions}"
                },
                new
                {
                    role = "user",
                    content = $"{primaryQuestion}"
                },
                new
                {
                    role = "assistant",
                    content = $"{primaryAnswer}"
                },
                new
                {
                    role = "user",
                    content = $"{followUpQuestion}?"
                },

            };

            var settings = new PredictionSettings
            {
                ServerUrl = "https://Cohere-command-r-plus-rusng-serverless.westus.inference.ai.azure.com/v1/chat/completions",
                ApiKey = azureCohereCommandRPlusAPIKey,
                MaxTokens = 600,
                Temperature = 0.9,
                TopP = 0.1,
                SysMessage = sysMessageArray
            };


            //make call to LLM, NOTE : high time consumption in chain
            var llmReply = await ProcessPrediction(settings);

            return llmReply;

        }



        private static JObject PackageReply(string userQuestion, string aiReplyText, string textHash, List<string> followUpQuestions, string sessionId, string aiReplyHtml = "", List<string> commands = null)
        {

            //using user question and LLM make answer more readable in HTML, bolding, paragraphing...etc
            //string textHtml = ChatAPI.HighlightKeywords_MistralSmall(aiReplyText, userQuestion).Result;

            //use back same if not specified custom HTML version
            string finalHtml = string.IsNullOrEmpty(aiReplyHtml) ? aiReplyText : aiReplyHtml;

            //only generate if not specified
            //string finalTextHash = string.IsNullOrEmpty(textHash) ? Tools.GetStringHashCodeMD5(aiReplyText, 15) : aiReplyHtml;
            commands ??= new List<string>();

            var reply = new JObject
            {
                { "SessionId", sessionId },
                { "Text", aiReplyText },
                { "TextHtml", finalHtml },
                { "TextHash", textHash},
                { "FollowUpQuestions", new JArray(followUpQuestions) },
                { "Commands", new JArray(commands) }
            };

            return reply;

        }

        private static ChatMessageEntity ReadFromTableByMessageNumber(string sessionId, int messageNumber)
        {

            Expression<Func<ChatMessageEntity, bool>> expression = call =>
                call.PartitionKey == sessionId &&
                call.MessageNumber == messageNumber;

            //execute search
            var recordFound = chatTableClient.Query(expression).FirstOrDefault();


            return recordFound;

        }

        public static ChatMessageEntity ReadFromTable(string sessionId, string messageHash)
        {
            string filter = $"PartitionKey eq '{sessionId}' and RowKey ge '{messageHash}'";

            var recordFound = chatTableClient.Query<ChatMessageEntity>(filter).FirstOrDefault();

            return recordFound;

        }


        public static ChatMessageEntity SaveToTable(ChatMessageEntity inputChatMessageEntity)
        {

            var response = chatTableClient.AddEntity(inputChatMessageEntity);

            return inputChatMessageEntity;
            //Console.WriteLine(response);

        }



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
            var relevantPredictions = await PickOutMostRelevantPredictions_MistralSmall(birthTime, userQuestion, predictionListChunks[0]); //call all models in parallel

            //#2 answer question based on relevant predictions
            //var answerLevel1 = await AnswerQuestionDirectly_MistralSmall(relevantPredictions, userQuestion); //call all models in parallel
            var answerLevel1 = await AnswerQuestionDirectly_CohereCommandRPlus(relevantPredictions, userQuestion); //call all models in parallel

            //#3 simplify answer
            //var answerLevel2 = await ImproveFinalAnswer_MistralLarge(answerLevel1[0], userQuestion);
            //var answerLevel2 = await ImproveFinalAnswer_MistralSmall(answerLevel1[0], userQuestion);

            //#4 remove disclaimers if any
            //todo check if contains disclaimers then remove
            //var answerLevel3 = await RemoveAnyDisclaimers_MistralLarge(answerLevel2, userQuestion);

            return answerLevel1;
        }

        private static List<string> GetPredictionAsChunks(Time birthTime)
        {
            //calculate predictions for current person
            var predictionList = Tools.GetHoroscopePrediction(birthTime);

            //extract only name and description and place nicely
            // Create a new list to hold the modified prediction objects
            List<dynamic> modifiedPredictionList = new List<dynamic>();

            // Iterate over each object in the predictionList
            foreach (var prediction in predictionList)
            {
                //calculate weigh of each prediction based on shadbala
                //NOTE: sum all weights of all houses and planets
                var weight = prediction.RelatedBody.RelatedHouses.Sum(relatedHouse => Calculate.HouseStrength(relatedHouse, birthTime).ToDouble());

                //add in planets
                weight += prediction.RelatedBody.RelatedPlanets.Sum(relatedHouse => Calculate.PlanetStrength(relatedHouse, birthTime).ToDouble());

                // Create a new anonymous object with the properties you want
                var modifiedPrediction = new
                {
                    Name = prediction.FormattedName,
                    Description = prediction.Description,
                    Relevance = string.Empty,
                    Weight = weight
                };

                // Add the modified prediction object to the new list
                modifiedPredictionList.Add(modifiedPrediction);
            }

            // Sort the list by weight in descending order and remove predictions with zero weight
            modifiedPredictionList = modifiedPredictionList.OrderByDescending(prediction => prediction.Weight).ToList();
            modifiedPredictionList.RemoveAll(prediction => prediction.Weight == 0);


            // Convert the list of modified prediction objects to a JSON string
            var jsonString = JsonConvert.SerializeObject(modifiedPredictionList, Formatting.None);

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

            var sysMessageArray = new[]
            {
                new
                {
                    role = "system",
                    content = $"as expert astrologer analyse life description text, concise answer, based on relevance and weight answer question with reason\n" +
                              $"QUESTION:\n{userQuestion}" +
                              $"\nLIFE DESCRIPTION:\n{answerLevelN}"
                }
            };

            var settings = new PredictionSettings
            {
                ServerUrl = "https://Mistral-small-xcvuv-serverless.westus.inference.ai.azure.com/v1/chat/completions",
                ApiKey = azureMistralSmallAPIKey,
                MaxTokens = 600,
                Temperature = 0.5,
                TopP = 0.2,
                SysMessage = sysMessageArray
            };


            //make call to LLM, NOTE : high time consumption in chain
            var llmReply = await ProcessPrediction(settings);

            return llmReply;

        }

        private static async Task<string> AnswerQuestionDirectly_CohereCommandRPlus(string answerLevelN, string userQuestion)
        {

            var sysMessageArray = new[]
            {
                new
                {
                    role = "user",
                    content = $"as expert astrologer analyse life description text, concise answer, based on relevance and weight answer question with reason\n" +
                              $"QUESTION:\n{userQuestion}" +
                              $"\nLIFE DESCRIPTION:\n{answerLevelN}"
                }
            };

            var settings = new PredictionSettings
            {
                ServerUrl = "https://Cohere-command-r-plus-rusng-serverless.westus.inference.ai.azure.com/v1/chat/completions",
                ApiKey = azureCohereCommandRPlusAPIKey,
                MaxTokens = 600,
                Temperature = 0.5,
                TopP = 0.5,
                SysMessage = sysMessageArray
            };


            //make call to LLM, NOTE : high time consumption in chain
            var llmReply = await ProcessPrediction(settings);

            return llmReply;

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
            var sysMessage =
                             "1. Output ANSWER text in HTML format for use between <p> tag element\n" +
                             "2. Highlight relevant words and phrases in ANSWER that is related to QUESTION\n" +
                             "3. Break long text and organize ANSWER text structure for easy readability." +
                             "4. All html element must be valid to be placed inside <p> tag element\n\n";


            var sysMessageArray = new[]
            {
                new
                {
                    role = "system",
                    content = "bold and paragraph structure DESCRIPTION text as HTML based on relevance to TOPIC text"
                },
                new
                {
                    role = "user",
                    content = @"{TOPIC:""Quantum Computing"", DESCRIPTION:""Quantum Computing uses principles of quantum mechanics to process information. It’s expected to revolutionize computing by performing complex calculations quickly.""}"
                },
                new
                {
                    role = "assistant",
                    content = "<p><strong>Quantum Computing</strong> uses principles of <strong>quantum mechanics</strong> to process information.<br> It's expected to revolutionize computing by performing complex calculations quickly. </p>"
                },
                new
                {
                    role = "user",
                    content = @"{TOPIC:""Artificial Intelligence"", DESCRIPTION:""Artificial Intelligence (AI) refers to the simulation of human intelligence in machines that are programmed to think like humans and mimic their actions.""}"
                },
                new
                {
                    role = "assistant",
                    content = @"<p>This term refers to the simulation of <strong>human intelligence in machines</strong>.<br>  These machines are <strong>programmed to think</strong> like humans and mimic their actions, <br> which is a significant advancement in the field of technology.</p>"
                },
                new
                {
                    role = "user",
                    content = @$"{{TOPIC:""{userQuestion}"", DESCRIPTION:""{answerText}""}}"
                },
            };

            var settings = new PredictionSettings
            {
                ServerUrl = "https://Cohere-command-r-plus-rusng-serverless.westus.inference.ai.azure.com/v1/chat/completions",
                ApiKey = azureCohereCommandRPlusAPIKey,
                //ServerUrl = "https://Mistral-large-cahy-serverless.westus.inference.ai.azure.com/v1/chat/completions",
                //ApiKey = azureMistralLargeAPIKey,
                //ServerUrl = "https://Mistral-small-xcvuv-serverless.westus.inference.ai.azure.com/v1/chat/completions",
                //ApiKey = azureMistralSmallAPIKey,
                MaxTokens = 8196,
                Temperature = 0.4,
                TopP = 0.1,
                SysMessage = sysMessageArray
            };


            //make call to LLM, NOTE : high time consumption in chain
            var llmReply = await ProcessPrediction(settings);

            return llmReply;

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


        private static async Task<string> ProcessPrediction(PredictionSettings settings)
        {
            var handler = CreateHttpClientHandler();
            var requestBody = CreateRequestBody(settings.SysMessage, settings.MaxTokens, settings.Temperature, settings.TopP);
            var content = new StringContent(requestBody);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            using (var client = new HttpClient(handler))
            {
                HttpResponseMessage response = await PostRequestAsync(client, content, settings.ServerUrl, settings.ApiKey);
                return await ProcessResponseAsync(response);
            }
        }

        private static async Task<string> PickOutMostRelevantPredictions_MistralSmall(Time birthTime, string userQuestion, string predictText)
        {

            var sysMessageArray = new[]
            {
                new
                {
                    role = "system",
                    content =
                        $"Output JSON.\n" +
                        $"Filter DESCRIPTION relevant to the QUESTION.\n" +
                        $"Judge based on weight.\n" +
                        $"Sort based on relevance, most relevant at top.\n\n"

                },
                new
                {
                    role = "user",
                    content = "QUESTION:{userQuestion}" +
                              "LIFE DESCRIPTION:{descriptionText}"
                },
                new
                {
                    role = "assistant",
                    content = @"{name:""{predictionName}"", description:""{description}"", relevance:""{relevanceScore}"", weight:""{weightScore}""}"
                },
                new
                {
                    role = "user",
                    content = $"QUESTION:\n{userQuestion}\n" +
                              $"LIFE DESCRIPTION:\n{predictText}"
                },

            };



            var settings = new PredictionSettings
            {
                ServerUrl = "https://Mistral-small-xcvuv-serverless.westus.inference.ai.azure.com/v1/chat/completions",
                ApiKey = azureMistralSmallAPIKey,
                MaxTokens = 8196,
                Temperature = 0.5,
                TopP = 0.5,
                SysMessage = sysMessageArray
            };


            //make call to LLM, NOTE : high time consumption in chain
            var llmReply = await ProcessPrediction(settings);

            return llmReply;

        }



        private static string CreateRequestBody(object[] sysMessage, double maxTokens, double temperature, double topP)
        {
            var requestBodyObject = new
            {
                messages = sysMessage,
                max_tokens = maxTokens,
                temperature = temperature,
                top_p = topP,
                safe_prompt = "false"
            };

            return JsonConvert.SerializeObject(requestBodyObject);
        }

        private static async Task<HttpResponseMessage> PostRequestAsync(HttpClient client, StringContent content, string serverUrl, string apiKey)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
            client.BaseAddress = new Uri(serverUrl);

            return await client.PostAsync("", content);
        }


        private static HttpClientHandler CreateHttpClientHandler() =>
            new()
            {
                ClientCertificateOptions = ClientCertificateOption.Manual,
                ServerCertificateCustomValidationCallback = (httpRequestMessage, cert, cetChain, policyErrors) => true
            };


        private static async Task<string> ProcessResponseAsync(HttpResponseMessage response)
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
