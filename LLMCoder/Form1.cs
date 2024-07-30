using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using VedAstro.Library;
using System.Text.Json.Nodes;
using System.Windows.Forms;

namespace LLMCoder
{
    public record ApiEndpoint(string Name, string Endpoint, string ApiKey);

    public partial class Form1 : Form
    {

        static IConfigurationRoot config = new ConfigurationBuilder().AddJsonFile("secrets.json", optional: true, reloadOnChange: true).Build();

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
        static string chatHistoryFilePath = "C:\\Users\\ASUS\\Desktop\\Projects\\VedAstroMK2\\LLMCoder\\chat-history.json"; // path to save the chat history



        public Form1()
        {
            InitializeComponent();

            //update selected LLM dropdown
            UpdateSelectedLLMDropdownView();

            //SET DEFAULT LLM CHOICE
            UpdateSelectedLLM("MetaLlama31405B");

            UpdatePastPromptsView();

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
            //load previous commands list and put them in view
            var allUserMessages = GetChatHistoryFromFile().Where(cH => cH.Role == "user").Select(ch => ch.Message);
            foreach (var item in allUserMessages)
            {
                pastUserPrompts.Items.Add(item.ToString());
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

        private async void sendUserMsgButton_Click(object sender, EventArgs e)
        {
            progressBar1.Visible = true;
            progressBar1.Maximum = 100;
            progressBar1.Value = 25;

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

            progressBar1.Value = 100;
            progressBar1.Visible = false;

        }

        // Send a message to the LLM and return its response
        async Task<string> SendMessageToLLM(HttpClient client, List<ConversationMessage> conversationHistory)
        {
            var messages = new List<object>
            {
                //new { role = "system", content = "expert programmer helper" },
                 new { role = "user", content = $@"analyse code ```{largeCodeSnippetTextBox.Text}```"},
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
            //clear view
            chatMessageOutputBox.Text = string.Empty;

            //clear chat history aswell
            conversationHistory.Clear();
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
