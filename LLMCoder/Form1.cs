using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using VedAstro.Library;
using System.Text.Json.Nodes;
using System.Windows.Forms;

namespace LLMCoder
{
    public partial class Form1 : Form
    {

        static IConfigurationRoot config = new ConfigurationBuilder().AddJsonFile("secrets.json", optional: true, reloadOnChange: true).Build();

        static string GPT4oEndpoint = config["GPT4oEndpoint"];
        static string GPT4oApiKey = config["GPT4oApiKey"];

        static string Phi3medium128kinstructEndpoint = config["Phi3medium128kinstructEndpoint"];
        static string Phi3medium128kinstructApiKey = config["Phi3medium128kinstructApiKey"];

        static string MistralNemo128kEndpoint = config["MistralNemo128kEndpoint"];
        static string MistralNemo128kApiKey = config["MistralNemo128kApiKey"];

        static string MetaLlama31405BEndpoint = config["MetaLlama31405BEndpoint"];
        static string MetaLlama31405BApiKey = config["MetaLlama31405BApiKey"];

        static string CohereCommandRPlusEndpoint = config["CohereCommandRPlusEndpoint"];
        static string CohereCommandRPlusApiKey = config["CohereCommandRPlusApiKey"];


        static HttpClient client;

        static List<ConversationMessage> conversationHistory = new List<ConversationMessage>();
        static string chatHistoryFilePath = "C:\\Users\\ASUS\\Desktop\\Projects\\VedAstroMK2\\LLMCoder\\chat-history.json"; // path to save the chat history



        public Form1()
        {
            InitializeComponent();

            string apiKey = CohereCommandRPlusApiKey;
            client = new HttpClient();
            client.Timeout = Timeout.InfiniteTimeSpan;
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
            client.BaseAddress = new Uri(CohereCommandRPlusEndpoint);

            // Initialize the chat history file
            InitializeChatHistoryFile();


            //load previous commands list
            var allUserMessages = GetChatHistoryFromFile().Where(cH => cH.Role == "user").Select(ch => ch.Message);
            foreach (var item in allUserMessages)
            {
                pastUserPrompts.Items.Add(item.ToString());
            }

        }

        private async void sendUserMsgButton_Click(object sender, EventArgs e)
        {

            string userInput = userInputTextBox.Text; //get text from input box 
            chatMessageOutputBox.Text += $"You: {userInput}\n";

            // Save the user's message for the next turn
            conversationHistory.Add(new ConversationMessage { Role = "user", Content = userInput });

            // Log the user's message to the chat history file
            await LogChatMessageToFile(userInput, "user");

            string response = await SendMessageToLLM(client, conversationHistory);
            chatMessageOutputBox.Text += $"LLM: {response}\n"; // Display LLM response

            // Log the LLM's response to the chat history file
            await LogChatMessageToFile(response, "assistant");

            //clear text for next input
            userInputTextBox.Text = "";
        }


        // Send a message to the LLM and return its response
        static async Task<string> SendMessageToLLM(HttpClient client, List<ConversationMessage> conversationHistory)
        {
            var messages = new List<object>
            {
                //new { role = "system", content = "expert programmer helper" },
                 new { role = "user", content = "C# .NET code \n"+@"```

using System.Net.Http.Headers;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using VedAstro.Library;
using Newtonsoft.Json.Linq;
using static System.Console;


namespace LLMCallExperimenter
{
    internal class Program
    {

        static IConfigurationRoot config = new ConfigurationBuilder().AddJsonFile(""secrets.json"", optional: true, reloadOnChange: true).Build();

        static string GPT4oEndpoint = config[""GPT4oEndpoint""];
        static string GPT4oApiKey = config[""GPT4oApiKey""];

        static string Phi3medium128kinstructEndpoint = config[""Phi3medium128kinstructEndpoint""];
        static string Phi3medium128kinstructApiKey = config[""Phi3medium128kinstructApiKey""];

        static string MistralNemo128kEndpoint = config[""MistralNemo128kEndpoint""];
        static string MistralNemo128kApiKey = config[""MistralNemo128kApiKey""];

        static string MetaLlama31405BEndpoint = config[""MetaLlama31405BEndpoint""];
        static string MetaLlama31405BApiKey = config[""MetaLlama31405BApiKey""];

        static string CohereCommandRPlusEndpoint = config[""CohereCommandRPlusEndpoint""];
        static string CohereCommandRPlusApiKey = config[""CohereCommandRPlusApiKey""];


        static HttpClient client;

        static List<ConversationMessage> conversationHistory = new List<ConversationMessage>();
        static string chatHistoryFilePath = ""C:\\Users\\ASUS\\Desktop\\Projects\\VedAstroMK2\\LLMCallExperimenter\\chat-history.json""; // path to save the chat history

        static async Task Main(string[] args)
        {
            string apiKey = CohereCommandRPlusApiKey;
            client = new HttpClient();
            client.Timeout = Timeout.InfiniteTimeSpan;
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(""Bearer"", apiKey);
            client.BaseAddress = new Uri(CohereCommandRPlusEndpoint);

            // Initialize the chat history file
            await InitializeChatHistoryFile();

            while (true)
            {
                Console.Write(""You: ""); // Prompt for user input
                string userInput = Console.ReadLine();

                // Stop chat if the user said exit
                if (userInput.ToLower() == ""exit"")
                {
                    break;
                }

                // Save the user's message for the next turn
                conversationHistory.Add(new ConversationMessage { Role = ""user"", Content = userInput });

                // Log the user's message to the chat history file
                await LogChatMessageToFile(userInput, ""user"");

                string response = await SendMessageToLLM(client, conversationHistory);
                Console.WriteLine($""LLM: {response}\n""); // Display LLM response

                // Log the LLM's response to the chat history file
                await LogChatMessageToFile(response, ""assistant"");
            }
        }


        // Send a message to the LLM and return its response
        static async Task<string> SendMessageToLLM(HttpClient client, List<ConversationMessage> conversationHistory)
        {
            var messages = new List<object>
            {
                //new { role = ""system"", content = ""expert programmer helper"" },
                 new { role = ""user"", content = ""C# .NET code \n""+@""```


DYNAMIC CODE

```
""},
                new { role = ""assistant"", content = ""ok, I've parsed the code, how may I help with it?"" }
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
            content.Headers.ContentType = new MediaTypeHeaderValue(""application/json"");

            //make call to API
            var response = await client.PostAsync("""", content);

            //if failed, scream the error back!
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($""Response Status Code: {response.StatusCode}"");
                Console.WriteLine($""Response Headers: \n{string.Join(""\r\n"", response.Headers.Concat(response.Content.Headers))}"");
                Console.WriteLine($""Response Body: {await response.Content.ReadAsStringAsync()}"");
            }

            var fullReplyRaw = await response.Content.ReadAsStringAsync();// Read response content as string
            var fullReply = new Phi3ReplyJson(fullReplyRaw);

            var replyMessage = fullReply?.Choices?.FirstOrDefault()?.Message.Content ?? ""No response!!"";

            //save in LLM's reply for next chat round
            conversationHistory.Add(new ConversationMessage { Role = ""assistant"", Content = replyMessage });

            return replyMessage;
        }

        // Initialize the chat history file
        static async Task InitializeChatHistoryFile()
        {
            try
            {
                // Create a default chat history array
                JArray chatHistoryArray = new JArray();

                // Check if the chat history file already exists
                if (File.Exists(chatHistoryFilePath))
                {
                    // Read the existing chat history from the file
                    string existingChatHistory = File.ReadAllText(chatHistoryFilePath);

                    // Parse the existing chat history and add it to the array
                    chatHistoryArray.Merge(JArray.Parse(existingChatHistory), new JsonMergeSettings
                    {
                        MergeArrayHandling = MergeArrayHandling.Union
                    });
                }

                // Write the chat history array to the file
                string chatHistoryJson = chatHistoryArray.ToString();
                File.WriteAllText(chatHistoryFilePath, chatHistoryJson);
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during the initialization process
                Console.WriteLine($""Error initializing chat history file: {ex.Message}"");
            }
        }


        // Log a chat message to the history file
        static async Task LogChatMessageToFile(string message, string role)
        {
            try
            {
                ChatLogEntry logEntry = new ChatLogEntry
                {
                    Timestamp = DateTime.Now.ToString(""yyyy-MM-dd HH:mm:ss""), // Current timestamp
                    Role = role, // Role of the message sender
                    Message = message // Message content
                };

                // Read the existing chat history from the file
                string existingChatHistory = File.ReadAllText(chatHistoryFilePath);
                JArray chatHistoryArray = JArray.Parse(existingChatHistory);

                // Add the new log entry to the array
                chatHistoryArray.Add(JObject.FromObject(logEntry));

                // Write the updated chat history back to the file
                string updatedChatHistory = chatHistoryArray.ToString();
                File.WriteAllText(chatHistoryFilePath, updatedChatHistory);
            }
            catch (Exception ex)
            {
                Console.WriteLine($""Error logging chat message: {ex.Message}""); // Handle logging error
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




```

chat-history.json
```json
[
  {
    ""Timestamp"": ""2024-07-29 19:24:41"",
    ""Role"": ""user"",
    ""Message"": ""summarize code""
  },
  {
    ""Timestamp"": ""2024-07-29 19:24:48"",
    ""Role"": ""assistant"",
    ""Message"": ""here is your answer""
  }
]
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
                //max_tokens = 4096, // Maximum number of tokens for the response
                temperature = 0.7, // Temperature for sampling
                top_p = 1, // Nucleus sampling parameter
                //presence_penalty = 0,
                //frequency_penalty = 0
            };

            var requestBody = JsonConvert.SerializeObject(requestBodyObject);

            var content = new StringContent(requestBody);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            //make call to API
            var response = await client.PostAsync("", content);

            //if failed, scream the error back!
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Response Status Code: {response.StatusCode}");
                Console.WriteLine($"Response Headers: \n{string.Join("\r\n", response.Headers.Concat(response.Content.Headers))}");
                Console.WriteLine($"Response Body: {await response.Content.ReadAsStringAsync()}");
            }

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
                // Create a default chat history array
                JArray chatHistoryArray = new JArray();

                // Check if the chat history file already exists
                if (File.Exists(chatHistoryFilePath))
                {
                    // Read the existing chat history from the file
                    string existingChatHistory = File.ReadAllText(chatHistoryFilePath);

                    // Parse the existing chat history and add it to the array
                    chatHistoryArray.Merge(JArray.Parse(existingChatHistory), new JsonMergeSettings
                    {
                        MergeArrayHandling = MergeArrayHandling.Union
                    });
                }

                // Write the chat history array to the file
                string chatHistoryJson = chatHistoryArray.ToString();
                File.WriteAllText(chatHistoryFilePath, chatHistoryJson);
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

                // Read the existing chat history from the file
                string existingChatHistory = File.ReadAllText(chatHistoryFilePath);
                JArray chatHistoryArray = JArray.Parse(existingChatHistory);

                // Add the new log entry to the array
                chatHistoryArray.Add(JObject.FromObject(logEntry));

                // Write the updated chat history back to the file
                string updatedChatHistory = chatHistoryArray.ToString();
                File.WriteAllText(chatHistoryFilePath, updatedChatHistory);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error logging chat message: {ex.Message}"); // Handle logging error
            }
        }

        static List<ChatLogEntry> GetChatHistoryFromFile()
        {
            // Read the existing chat history from the file
            string existingChatHistory = File.ReadAllText(chatHistoryFilePath);
            JArray chatHistoryArray = JArray.Parse(existingChatHistory);

            var returnList = new List<ChatLogEntry>();

            foreach (var chatLog in chatHistoryArray)
            {
                //add new to list
                var chatLogEntry = new ChatLogEntry()
                {
                    Message = chatLog["Message"].ToString(),
                    Role = chatLog["Role"].ToString(),
                    Timestamp = chatLog["Timestamp"].ToString(),
                };
                returnList.Add(chatLogEntry);
            }

            return returnList;
        }

        private void pastUserPrompts_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (pastUserPrompts.SelectedItem != null)
            {
                string selectedItem = pastUserPrompts.SelectedItem.ToString();
                // Perform actions or display information based on the selected item
                userInputTextBox.Text += $" {selectedItem}";
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
