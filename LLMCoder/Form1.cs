using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using VedAstro.Library;
using System.Text.Json.Nodes;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace LLMCoder
{
    public partial class Form1 : Form
    {

        static IConfigurationRoot config = new ConfigurationBuilder().AddJsonFile("secrets.json", optional: true, reloadOnChange: true).Build();

        //init all LLM choices here
        static List<ApiEndpoint> ApiEndpoints = new()
        {
            new ApiEndpoint("GPT4o", config["GPT4oEndpoint"], config["GPT4oApiKey"]),
            new ApiEndpoint("Phi3medium128kinstruct", config["Phi3medium128kinstructEndpoint"], config["Phi3medium128kinstructApiKey"]),
            new ApiEndpoint("MistralNemo128k", config["MistralNemo128kEndpoint"], config["MistralNemo128kApiKey"]),
            new ApiEndpoint("MetaLlama31405B", config["MetaLlama31405BEndpoint"], config["MetaLlama31405BApiKey"]),
            new ApiEndpoint("CohereCommandRPlus", config["CohereCommandRPlusEndpoint"], config["CohereCommandRPlusApiKey"])
        };

        static HttpClient client;

        static List<ConversationMessage> conversationHistory = new List<ConversationMessage>();
        static string chatHistoryFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "chat-history.json");



        public Form1()
        {
            InitializeComponent();

            // Update the dropdown with available LLM choices
            UpdateSelectedLLMDropdownView();

            // Set the default LLM choice
            UpdateSelectedLLM("MetaLlama31405B");

            // Load and display past user prompts as templates
            UpdatePastPromptsView();

            //auto load ai pretext for quick use
            injectAssitantPretextTextBox.Text = "ok, I've parsed the code, how may I help with it?";

            //auto load ai pretext for quick use
            codeInjectPretextTextBox.Text = "analyse and parse code";

            //set default settings
            temperatureTextBox.Text = "0.7";
            topPTextBox.Text = "1.0";
        }


        private void UpdateSelectedLLMDropdownView()
        {
            foreach (var llmChoice in ApiEndpoints)
            {
                llmSelector.Items.Add(llmChoice.Name);
            }
        }

        private void UpdateSelectedLLM(string selectedLLM)
        {
            var apiEndpointByName = GetApiEndpointByName(selectedLLM);

            client = new HttpClient();
            client.Timeout = Timeout.InfiniteTimeSpan;
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiEndpointByName.ApiKey);
            client.BaseAddress = new Uri(apiEndpointByName.Endpoint);

            // Make visible to user
            llmSelector.SelectedIndex = llmSelector.Items.IndexOf(selectedLLM);
        }

        private void UpdatePastPromptsView()
        {
            // Load previous chat history from file
            var chatHistory = GetChatHistoryFromFile();

            // Filter messages to include only those from the user
            var userMessages = chatHistory.Where(entry => entry.Role == "user").Select(entry => entry.Message);

            // Remove duplicate messages
            var uniqueUserMessages = userMessages.Distinct();

            // Clear the existing items in the pastUserPrompts combo box
            pastUserPrompts.Items.Clear();

            // Add unique user messages to the combo box
            foreach (var message in uniqueUserMessages)
            {
                pastUserPrompts.Items.Add(message);
            }
        }

        public static ApiEndpoint GetApiEndpointByName(string name)
        {
            foreach (var endpoint in ApiEndpoints)
            {
                if (endpoint.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                {
                    return endpoint;
                }
            }

            throw new ArgumentException($"API endpoint with name '{name}' not found.");
        }

        public static int CountTokens(string input)
        {
            // Simple tokenization using whitespace and punctuation as boundaries
            string[] tokens = Regex.Split(input, @"\s+|[,;.!?]+");

            // Filter out empty tokens
            tokens = Array.FindAll(tokens, token => !string.IsNullOrEmpty(token));

            return tokens.Length;
        }


        // Send a message to the LLM and return its response
        async Task<string> SendMessageToLLM(HttpClient client, List<ConversationMessage> conversationHistory)
        {
            var messages = new List<object>
            {
                //new { role = "system", content = "expert programmer helper" },
                new { role = "user", content = $"{codeInjectPretextTextBox.Text}\n```\n{largeCodeSnippetTextBox.Text}\n```"},
                new { role = "assistant", content = $"{injectAssitantPretextTextBox.Text}" }
            };

            //add in 
            foreach (var message in conversationHistory)
            {
                messages.Add(new { role = message.Role, content = message.Content });
            }

            var requestBodyObject = new
            {
                messages, // List of messages
                //max_tokens = 32000, // Maximum number of tokens for the response
                //max_tokens = 4096, // Maximum number of tokens for the response
                temperature = double.Parse(temperatureTextBox.Text), // Temperature for sampling
                top_p = double.Parse(topPTextBox.Text), // Nucleus sampling parameter
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

                chatMessageOutputBox.Text += ($"Response Status Code: {response.StatusCode}");
                chatMessageOutputBox.Text += ($"Response Headers: \n{string.Join("\r\n", response.Headers.Concat(response.Content.Headers))}");
                chatMessageOutputBox.Text += ($"Response Body: {await response.Content.ReadAsStringAsync()}");
            }

            var fullReplyRaw = await response.Content.ReadAsStringAsync();// Read response content as string
            var fullReply = new Phi3ReplyJson(fullReplyRaw);

            var replyMessage = fullReply?.Choices?.FirstOrDefault()?.Message.Content ?? "No response!!";

            //save in LLM's reply for next chat round
            conversationHistory.Add(new ConversationMessage { Role = "assistant", Content = replyMessage });

            return replyMessage;
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


        // Load chat history from file and return as a list of ChatLogEntry objects
        static List<ChatLogEntry> GetChatHistoryFromFile()
        {
            try
            {
                if (!File.Exists(chatHistoryFilePath))
                {
                    File.WriteAllText(chatHistoryFilePath, "[]");
                }

                // Read the chat history file content
                string chatHistoryFileContent = File.ReadAllText(chatHistoryFilePath);

                // Parse the JSON content into a JArray
                JArray chatHistoryArray = JArray.Parse(chatHistoryFileContent);

                // Convert each JObject in the array to a ChatLogEntry object
                var chatHistoryList = chatHistoryArray.Select(entry => new ChatLogEntry
                {
                    Timestamp = entry["Timestamp"].ToString(),
                    Role = entry["Role"].ToString(),
                    Message = entry["Message"].ToString()
                }).ToList();

                return chatHistoryList;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading chat history file: {ex.Message}");
                return new List<ChatLogEntry>();
            }
        }



        //------------------- EVENT HANDLERS ---------------------

        private void pastUserPrompts_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (pastUserPrompts.SelectedItem != null)
            {
                string selectedItem = pastUserPrompts.SelectedItem.ToString();
                // Perform actions or display information based on the selected item
                userInputTextBox.Text += $"{selectedItem}";
            }
        }

        private void clearUserMsgButton_Click(object sender, EventArgs e)
        {
            userInputTextBox.Text = "";
        }

        private void llmSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Cast the sender as a ComboBox
            ComboBox comboBox = (ComboBox)sender;

            // Get the selected item's text
            string selectedLLMName = comboBox.SelectedItem.ToString();

            UpdateSelectedLLM(selectedLLMName);

        }

        private void resetChatHistoryButton_Click(object sender, EventArgs e)
        {
            // Clear the chat output box text
            chatMessageOutputBox.Text = string.Empty;

            // Clear the conversation history list
            conversationHistory.Clear();
        }

        private async void sendUserMsgButton_Click(object sender, EventArgs e)
        {
            // Update the progress bar visibility and value
            progressBar1.Value = 25;

            // Get the user input from the input text box
            string userInput = userInputTextBox.Text;

            // Display the user's message in the chat output box
            chatMessageOutputBox.Text += $"You: {userInput}\n";

            // Add the user's message to the conversation history list
            conversationHistory.Add(new ConversationMessage { Role = "user", Content = userInput });

            // Log the user's message to the chat history file
            await LogChatMessageToFile(userInput, "user");

            progressBar1.Value = 50;

            // Send a message to the LLM and get its response
            string response = await SendMessageToLLM(client, conversationHistory);

            progressBar1.Value = 75;

            // Display the LLM's response in the chat output box
            chatMessageOutputBox.Text += $"LLM: {response}\n";

            // Log the LLM's response to the chat history file
            await LogChatMessageToFile(response, "assistant");

            // Clear the user input text box for the next input
            userInputTextBox.Text = "";

            // Load and display past user prompts as templates
            UpdatePastPromptsView();

            // Update the progress bar value and hide it
            progressBar1.Value = 100;
            await Task.Delay(300); //short delay
            progressBar1.Value = 0;
        }

        private void largeCodeSnippetTextBox_TextChanged(object sender, EventArgs e)
        {
            //update token counter
            tokenCountLabel.Text = $"Token Count : {CountTokens(largeCodeSnippetTextBox.Text)}";
        }
    }

    public record ApiEndpoint(string Name, string Endpoint, string ApiKey);

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
