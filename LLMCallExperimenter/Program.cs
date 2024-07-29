using System.Net.Http.Headers;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using VedAstro.Library;
using Newtonsoft.Json.Linq;

namespace LLMCallExperimenter
{
    internal class Program
    {

        static IConfigurationRoot config = new ConfigurationBuilder().AddJsonFile("secrets.json", optional: true, reloadOnChange: true).Build();

        static string GPT4oEndpoint = config["GPT4oEndpoint"];
        static string GPT4oApiKey = config["GPT4oApiKey"];

        static string Phi3medium128kinstructEndpoint = config["Phi3medium128kinstructEndpoint"];
        static string Phi3medium128kinstructApiKey = config["Phi3medium128kinstructApiKey"];

        static string MistralNemo128kEndpoint = config["MistralNemo128kEndpoint"];
        static string MistralNemo128kApiKey = config["MistralNemo128kApiKey"];

        static string CohereCommandRPlusEndpoint = config["CohereCommandRPlusEndpoint"];
        static string CohereCommandRPlusApiKey = config["CohereCommandRPlusApiKey"];


        static HttpClient client;

        static List<ConversationMessage> conversationHistory = new List<ConversationMessage>();
        static string chatHistoryFilePath = "C:\\Users\\ASUS\\Desktop\\Projects\\VedAstroMK2\\LLMCallExperimenter\\chat-history.json"; // path to save the chat history

        static async Task Main(string[] args)
        {
            string apiKey = CohereCommandRPlusApiKey;
            client = new HttpClient();
            client.Timeout = Timeout.InfiniteTimeSpan;
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
            client.BaseAddress = new Uri(CohereCommandRPlusEndpoint);

            // Initialize the chat history file
            await InitializeChatHistoryFile();

            while (true)
            {
                Console.Write("You: "); // Prompt for user input
                string userInput = Console.ReadLine();

                // Stop chat if the user said exit
                if (userInput.ToLower() == "exit")
                {
                    break;
                }

                // Save the user's message for the next turn
                conversationHistory.Add(new ConversationMessage { Role = "user", Content = userInput });

                // Log the user's message to the chat history file
                await LogChatMessageToFile(userInput, "user");

                string response = await SendMessageToLLM(client, conversationHistory);
                Console.WriteLine($"LLM: {response}\n"); // Display LLM response

                // Log the LLM's response to the chat history file
                await LogChatMessageToFile(response, "assistant");
            }
        }


        // Send a message to the LLM and return its response
        static async Task<string> SendMessageToLLM(HttpClient client, List<ConversationMessage> conversationHistory)
        {
            var messages = new List<object>
            {
                //new { role = "system", content = "expert programmer helper" },
                 new { role = "user", content = "C# .NET code \n"+@"```

DYNAMIC CODE


```
"},
                new { role = "assistant", content = "ok, I've parsed the code, how may I help with it?" }
            };

            foreach (var message in conversationHistory)
            {
                messages.Add(new { role = message.Role, content = message.Content });
            }

            var requestBodyObject = new
            {
                messages, // List of messages
                max_tokens = 32000, // Maximum number of tokens for the response
                temperature = 0.7, // Temperature for sampling
                top_p = 1, // Nucleus sampling parameter
                //presence_penalty = 0,
                //frequency_penalty = 0
            };

            var requestBody = JsonConvert.SerializeObject(requestBodyObject);

            var content = new StringContent(requestBody);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await client.PostAsync("", content);

            var fullReplyRaw = await response.Content.ReadAsStringAsync();// Read response content as string
            var fullReply = new Phi3ReplyJson(fullReplyRaw);

            var replyMessage = fullReply?.Choices?.FirstOrDefault()?.Message.Content ?? "No response!!";

            //save in LLM's reply for next chat round
            conversationHistory.Add(new ConversationMessage { Role = "assistant", Content = replyMessage });

            return replyMessage;
        }

        // Initialize the chat history file
        static async Task InitializeChatHistoryFile()
        {
            try
            {
                // Check if the chat history file already exists
                if (!File.Exists(chatHistoryFilePath))
                {
                    // Create a new file stream to create the file
                    using (FileStream fs = new FileStream(chatHistoryFilePath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
                    {
                        // Dispose of the file stream to ensure proper cleanup
                        await fs.DisposeAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during the initialization process
                Console.WriteLine($"Error initializing chat history file: {ex.Message}");
            }
        }

        // Log a chat message to the history file
        static async Task LogChatMessageToFile(string message, string role)
        {
            try
            {
                ChatLogEntry logEntry = new ChatLogEntry
                {
                    Timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), // Current timestamp
                    Role = role, // Role of the message sender
                    Message = message // Message content
                };

                using (FileStream fs = new FileStream(chatHistoryFilePath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
                {
                    using (StreamWriter writer = new StreamWriter(fs))
                    {
                        // Write the log entry to the file
                        var xxx = JObject.FromObject(logEntry);
                        await writer.WriteAsync(xxx.ToString());
                        await writer.WriteAsync(Environment.NewLine);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error logging chat message: {ex.Message}"); // Handle logging error
            }
        }
    }

    // Class to represent a conversation message
    public class ConversationMessage
    {
        public string Role { get; set; }
        public string Content { get; set; }
    }

    // Class to represent a chat log entry
    public class ChatLogEntry
    {
        public string Timestamp { get; set; }
        public string Role { get; set; }
        public string Message { get; set; }
    }
}

