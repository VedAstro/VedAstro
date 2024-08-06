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

        // Create a global instance of conversationHistory
        private static List<ConversationMessage> _conversationHistory = new List<ConversationMessage>();
        public static List<ConversationMessage> conversationHistory { get { return _conversationHistory; } }

        static string chatHistoryFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "chat-history.json");

        // Dictionary to store message IDs and their corresponding tableLayoutPanels
        private Dictionary<string, TableLayoutPanel> messageTableLayouts = new Dictionary<string, TableLayoutPanel>();


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

            //default inject code
            includeCodeInjectCheckBox.Checked = true;
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
        async Task<string> SendMessageToLLM(HttpClient client)
        {
            //inject code pretext ONLY if specified
            var messages = includeCodeInjectCheckBox.Checked ? new List<object>
            {
                //new { role = "system", content = "expert programmer helper" },
                new { role = "user", content = $"{codeInjectPretextTextBox.Text}\n```\n{largeCodeSnippetTextBox.Text}\n```"},
                new { role = "assistant", content = $"{injectAssitantPretextTextBox.Text}" }
            } : new List<object>();

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
                var errorResponse = await response.Content.ReadAsStringAsync();
                AddMessageToPanel($"Error: {response.StatusCode} - {errorResponse}", "assistant", Color.Red);
                return "Error occurred. Please check the error message above.";

            }

            var fullReplyRaw = await response.Content.ReadAsStringAsync();// Read response content as string
            var fullReply = new Phi3ReplyJson(fullReplyRaw);

            var replyMessage = fullReply?.Choices?.FirstOrDefault()?.Message.Content ?? "No response!!";

            //save in LLM's reply for next chat round
            //conversationHistory.Add(new ConversationMessage { Role = "assistant", Content = replyMessage });
            //conversationHistory.Add(new ConversationMessage { Role = "user", Content = userInput });

            return replyMessage;
        }

        // Log a chat message to the history file
        // New method to log a chat message to the history file
        private async void LogChatMessageToFile(ConversationMessage conversationMessage)
        {
            try
            {
                ChatLogEntry logEntry = new ChatLogEntry
                {
                    Timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), // Current timestamp
                    Id = conversationMessage.Id, // Message ID
                    Role = conversationMessage.Role, // Role of the message sender
                    Message = conversationMessage.Content // Message content
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
            chatMessagePanel.Controls.Clear();

            // Clear the conversation history list
            conversationHistory.Clear();
        }


        // Event handler for the send user message button click
        private async void sendUserMsgButton_Click(object sender, EventArgs e)
        {
            // Update the progress bar visibility and value
            progressBar1.Value = 25;

            // Get the user input from the input text box
            string userInput = userInputTextBox.Text;

            // Display the user's message in the chat output box
            AddMessageToPanel(userInput, "user", Color.Aqua);

            // Log the user's message to the chat history file
            // Note: This is now handled in the AddMessageToPanel method

            // Add the user's message to the conversation history list
            // Note: This is now handled in the AddMessageToPanel method

            progressBar1.Value = 50;

            // Send a message to the LLM and get its response
            string response = await SendMessageToLLM(client);

            progressBar1.Value = 75;

            // Display the LLM's response in the chat output box
            AddMessageToPanel(response, "assistant", Color.LimeGreen);

            // Log the LLM's response to the chat history file
            // Note: This is now handled in the AddMessageToPanel method

            // Add the LLM's response to the conversation history list
            // Note: This is now handled in the AddMessageToPanel method

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

        private void Form1_Load(object sender, EventArgs e)
        {


        }


        // New method to add a message to the chat message panel
        private void AddMessageToPanel(string message, string role, Color color)
        {
            //support empty calls for user to resend existing msg history
            //so do nothing here
            if (string.IsNullOrEmpty(message)) { return; }

            // Generate a random unique ID for the message
            string messageId = Guid.NewGuid().ToString();

            // Create a new ConversationMessage
            ConversationMessage conversationMessage = new ConversationMessage
            {
                Id = messageId,
                Role = role,
                Content = message
            };

            // Add the message to the conversation history
            conversationHistory.Add(conversationMessage);

            // Log the message to the chat history file
            LogChatMessageToFile(conversationMessage);

            // Create a new tableLayoutPanel
            TableLayoutPanel tableLayoutPanel = new TableLayoutPanel();
            tableLayoutPanel.AutoSize = true;
            tableLayoutPanel.ColumnCount = 2;
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel.Dock = DockStyle.Fill;

            // Create a new richTextBox
            RichTextBox richTextBox = new RichTextBox();
            richTextBox.BackColor = SystemColors.ActiveCaptionText;
            richTextBox.Dock = DockStyle.Fill;
            richTextBox.ForeColor = color;
            richTextBox.Text = $"{role}: {message}";
            richTextBox.ReadOnly = true; // Make it read-only to prevent editing
            richTextBox.ScrollBars = RichTextBoxScrollBars.Vertical; // Add vertical scrollbar
            richTextBox.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);

            // Create a new button
            Button button = new Button();
            button.BackColor = Color.IndianRed;
            button.Dock = DockStyle.Fill;
            button.ForeColor = SystemColors.ButtonFace;
            button.Text = "Delete";
            button.Tag = messageId; // Store the message ID in the button's Tag property
            button.Click += (sender, e) => DeleteMessage(tableLayoutPanel, (string)((Button)sender).Tag);

            // Add controls to the tableLayoutPanel
            tableLayoutPanel.Controls.Add(richTextBox, 0, 0);
            tableLayoutPanel.Controls.Add(button, 1, 0);

            // Store the message ID in the Tag property of the TableLayoutPanel
            tableLayoutPanel.Tag = messageId;

            // Add the tableLayoutPanel to the chatMessagePanel
            chatMessagePanel.Controls.Add(tableLayoutPanel);

            // Store the message ID and tableLayoutPanel in a dictionary
            messageTableLayouts[messageId] = tableLayoutPanel;
        }


        // New method to delete a message from the chat message panel
        private void DeleteMessage(TableLayoutPanel tableLayoutPanel, string messageId)
        {
            // Remove the message from the conversation history
            conversationHistory.RemoveAll(m => m.Id == messageId);

            // Remove the tableLayoutPanel from the chatMessagePanel
            chatMessagePanel.Controls.Remove(tableLayoutPanel);

            // Dispose of the tableLayoutPanel
            tableLayoutPanel.Dispose();

            // Remove the message ID from the dictionary
            messageTableLayouts.Remove(messageId);
        }



    }

    public record ApiEndpoint(string Name, string Endpoint, string ApiKey);

    // Class to represent a conversation message
    public class ConversationMessage
    {
        public string Id { get; set; }
        public string Role { get; set; }
        public string Content { get; set; }
    }

    // Class to represent a chat log entry
    public class ChatLogEntry
    {
        public string Timestamp { get; set; }
        public string Id { get; set; }
        public string Role { get; set; }
        public string Message { get; set; }
    }
}
