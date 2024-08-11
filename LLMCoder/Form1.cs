using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using VedAstro.Library;
using System.Text.Json.Nodes;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Text;
using System.Text.Json.Serialization;

namespace LLMCoder
{
    public partial class Form1 : Form
    {

        static IConfigurationRoot config = new ConfigurationBuilder().AddJsonFile("secrets.json", optional: true, reloadOnChange: true).Build();

        //init all LLM choices here
        //static List<ApiConfig> ApiConfigs = new()
        //{
        //    new ApiConfig("GPT4o", config["GPT4o_Endpoint"], config["GPT4o_ApiKey"], int.Parse(config["GPT4o_MaxContextWindowTokens"])),
        //    new ApiConfig("Phi3medium128kinstruct", config["Phi3medium128kinstructEndpoint"], config["Phi3medium128kinstruct_ApiKey"], int.Parse(config["Phi3medium128kinstruct_MaxContextWindowTokens"])),
        //    new ApiConfig("MistralNemo128k", config["MistralNemo128k_Endpoint"], config["MistralNemo128k_ApiKey"], int.Parse(config["MistralNemo128k_MaxContextWindowTokens"])),
        //    new ApiConfig("MetaLlama31405B", config["MetaLlama31405B_Endpoint"], config["MetaLlama31405B_ApiKey"], int.Parse(config["MetaLlama31405B_MaxContextWindowTokens"])),
        //    new ApiConfig("CohereCommandRPlus", config["CohereCommandRPlus_Endpoint"], config["CohereCommandRPlus_ApiKey"], int.Parse(config["CohereCommandRPlus_MaxContextWindowTokens"]))
        //};

        public static List<ApiConfig> ApiConfigs { get; set; }

        static HttpClient client;

        // Create a global instance of conversationHistory
        private static List<ConversationMessage> _conversationHistory = new List<ConversationMessage>();
        public static List<ConversationMessage> conversationHistory { get { return _conversationHistory; } }

        static string chatHistoryFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "chat-history.json");

        // Dictionary to store message IDs and their corresponding tableLayoutPanels
        private Dictionary<string, TableLayoutPanel> messageTableLayouts = new Dictionary<string, TableLayoutPanel>();

        /// <summary>
        /// currently selected or main llm for chat
        /// </summary>
        public ApiConfig SelectedLLMConfig { get; set; }

        public Form1()
        {
            // Load ApiConfigs from config.json file
            LoadApiConfigsFromConfigFile();

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
            snippetInjectCheckBox.Checked = true;

        }



        private void TextBox_MouseEnter(TextBox textBox, EventArgs e)
        {
            // Capture mouse wheel events when the mouse is over textBox
            textBox.Focus();
            this.MouseWheel += (sender, mouseEventArgs) => Form1_MouseWheel(textBox, mouseEventArgs);
        }

        private void TextBox_MouseLeave(TextBox textBox, EventArgs e)
        {
            // Stop capturing mouse wheel events when the mouse leaves textBox
            this.MouseWheel -= (sender, mouseEventArgs) => Form1_MouseWheel(textBox, mouseEventArgs);
        }

        private void Form1_MouseWheel(TextBox textBox, MouseEventArgs e)
        {
            try
            {
                int value = int.Parse(textBox.Text);

                if (e.Delta > 0)
                {
                    // Increment by 5 on scroll up
                    textBox.Text = (value + 5).ToString();
                }
                else
                {
                    // Decrement by 5 on scroll down
                    textBox.Text = (value - 5).ToString();
                }
            }
            catch (FormatException)
            {
                // Handle non-integer value in textBox
                MessageBox.Show("Please enter a valid number.");
                textBox.Text = "0";
            }
        }

        private void InitializeCodeFileInjectTablePanel()
        {
            // Declare and initialize variable types
            TableLayoutPanel codeFileInjectTablePanel = new TableLayoutPanel();
            TextBox codeFileInjectPathTextBox = new TextBox();
            Label fileInjectPathLabel = new Label();
            TableLayoutPanel lineNumRangeInputTable = new TableLayoutPanel();
            TextBox startLineNumberTextBox = new TextBox();
            Label startLabel = new Label();
            Label endLabel = new Label();
            TextBox endLineNumberTextBox = new TextBox();
            Label lineNumberRangeLabel = new Label();
            Label fetchCodeStatusMessageLabel = new Label();
            Label preCodePromptLabel = new Label();
            TextBox preCodePromptTextBox = new TextBox();
            Label codeFileInjectLabel = new Label();
            RichTextBox codeFileInjectTextBox = new RichTextBox();
            Label postCodePromptLabel = new Label();
            TextBox postCodePromptTextBox = new TextBox();
            Button deleteCodeInjectButton = new Button();
            Button fetchLatestInjectedCodeButton = new Button();
            Button expandCodeFileButton = new Button();

            // Initialize codeFileInjectTablePanel
            codeFileInjectTablePanel.AutoSize = true;
            codeFileInjectTablePanel.ColumnCount = 3;
            codeFileInjectTablePanel.ColumnStyles.Add(new ColumnStyle());
            codeFileInjectTablePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            codeFileInjectTablePanel.ColumnStyles.Add(new ColumnStyle());
            codeFileInjectTablePanel.Controls.Add(fetchLatestInjectedCodeButton, 2, 1);
            codeFileInjectTablePanel.Controls.Add(deleteCodeInjectButton, 2, 0);
            codeFileInjectTablePanel.Controls.Add(postCodePromptTextBox, 1, 4);
            codeFileInjectTablePanel.Controls.Add(postCodePromptLabel, 0, 4);
            codeFileInjectTablePanel.Controls.Add(codeFileInjectTextBox, 1, 3);
            codeFileInjectTablePanel.Controls.Add(codeFileInjectLabel, 0, 3);
            codeFileInjectTablePanel.Controls.Add(preCodePromptTextBox, 1, 2);
            codeFileInjectTablePanel.Controls.Add(preCodePromptLabel, 0, 2);
            codeFileInjectTablePanel.Controls.Add(lineNumRangeInputTable, 1, 1);
            codeFileInjectTablePanel.Controls.Add(lineNumberRangeLabel, 0, 1);
            codeFileInjectTablePanel.Controls.Add(codeFileInjectPathTextBox, 1, 0);
            codeFileInjectTablePanel.Controls.Add(fileInjectPathLabel, 0, 0);
            codeFileInjectTablePanel.Controls.Add(expandCodeFileButton, 2, 3);
            codeFileInjectTablePanel.Dock = DockStyle.Fill;
            codeFileInjectTablePanel.Location = new Point(0, 0);
            codeFileInjectTablePanel.Name = "codeFileInjectTablePanel";
            codeFileInjectTablePanel.RowCount = 6;
            codeFileInjectTablePanel.RowStyles.Add(new RowStyle());
            codeFileInjectTablePanel.RowStyles.Add(new RowStyle());
            codeFileInjectTablePanel.RowStyles.Add(new RowStyle());
            codeFileInjectTablePanel.RowStyles.Add(new RowStyle());
            codeFileInjectTablePanel.RowStyles.Add(new RowStyle());
            codeFileInjectTablePanel.RowStyles.Add(new RowStyle());
            codeFileInjectTablePanel.Size = new Size(852, 663);
            codeFileInjectTablePanel.TabIndex = 1;
            codeFileInjectTablePanel.BackColor = Color.Black;

            // Initialize lineNumRangeInputTable
            lineNumRangeInputTable.AutoSize = true;
            lineNumRangeInputTable.ColumnCount = 5;
            lineNumRangeInputTable.ColumnStyles.Add(new ColumnStyle());
            lineNumRangeInputTable.ColumnStyles.Add(new ColumnStyle());
            lineNumRangeInputTable.ColumnStyles.Add(new ColumnStyle());
            lineNumRangeInputTable.ColumnStyles.Add(new ColumnStyle());
            lineNumRangeInputTable.ColumnStyles.Add(new ColumnStyle());
            lineNumRangeInputTable.Controls.Add(fetchCodeStatusMessageLabel, 5, 0);
            lineNumRangeInputTable.Controls.Add(endLineNumberTextBox, 3, 0);
            lineNumRangeInputTable.Controls.Add(endLabel, 2, 0);
            lineNumRangeInputTable.Controls.Add(startLineNumberTextBox, 1, 0);
            lineNumRangeInputTable.Controls.Add(startLabel, 0, 0);
            lineNumRangeInputTable.Dock = DockStyle.Fill;
            lineNumRangeInputTable.Location = new Point(96, 32);
            lineNumRangeInputTable.Name = "lineNumRangeInputTable";
            lineNumRangeInputTable.RowCount = 1;
            lineNumRangeInputTable.RowStyles.Add(new RowStyle());
            lineNumRangeInputTable.Size = new Size(672, 29);
            lineNumRangeInputTable.TabIndex = 6;

            // Initialize other controls
            fileInjectPathLabel.AutoSize = true;
            fileInjectPathLabel.Location = new Point(3, 7);
            fileInjectPathLabel.Margin = new Padding(3, 7, 3, 0);
            fileInjectPathLabel.Name = "fileInjectPathLabel";
            fileInjectPathLabel.Size = new Size(52, 15);
            fileInjectPathLabel.TabIndex = 1;
            fileInjectPathLabel.Text = "File Path";
            fileInjectPathLabel.ForeColor = Color.Azure;

            codeFileInjectPathTextBox.Dock = DockStyle.Fill;
            codeFileInjectPathTextBox.Location = new Point(96, 3);
            codeFileInjectPathTextBox.Name = "codeFileInjectPathTextBox";
            codeFileInjectPathTextBox.Size = new Size(672, 23);
            codeFileInjectPathTextBox.TabIndex = 3;

            lineNumberRangeLabel.AutoSize = true;
            lineNumberRangeLabel.Location = new Point(3, 36);
            lineNumberRangeLabel.Margin = new Padding(3, 7, 3, 0);
            lineNumberRangeLabel.Name = "lineNumberRangeLabel";
            lineNumberRangeLabel.Size = new Size(87, 15);
            lineNumberRangeLabel.TabIndex = 4;
            lineNumberRangeLabel.Text = "Select Range";
            lineNumberRangeLabel.ForeColor = Color.Azure;

            startLabel.AutoSize = true;
            startLabel.Location = new Point(3, 7);
            startLabel.Margin = new Padding(3, 7, 3, 0);
            startLabel.Name = "startLabel";
            startLabel.Size = new Size(31, 15);
            startLabel.TabIndex = 5;
            startLabel.Text = "Start";
            startLabel.ForeColor = Color.Azure;

            startLineNumberTextBox.Dock = DockStyle.Fill;
            startLineNumberTextBox.Location = new Point(40, 3);
            startLineNumberTextBox.MaximumSize = new Size(90, 0);
            startLineNumberTextBox.Name = "startLineNumberTextBox";
            startLineNumberTextBox.Size = new Size(90, 23);
            startLineNumberTextBox.TabIndex = 6;
            startLineNumberTextBox.Text = "1"; //default to 1 as start
            // easy GUI scroll to adjust line numbers
            startLineNumberTextBox.MouseEnter += (sender, e) => TextBox_MouseEnter(startLineNumberTextBox, e);
            startLineNumberTextBox.MouseLeave += (sender, e) => TextBox_MouseLeave(startLineNumberTextBox, e);


            endLabel.AutoSize = true;
            endLabel.Location = new Point(136, 7);
            endLabel.Margin = new Padding(3, 7, 3, 0);
            endLabel.Name = "endLabel";
            endLabel.Size = new Size(27, 15);
            endLabel.TabIndex = 7;
            endLabel.Text = "End";
            endLabel.ForeColor = Color.Azure;

            endLineNumberTextBox.Dock = DockStyle.Fill;
            endLineNumberTextBox.Location = new Point(169, 3);
            endLineNumberTextBox.MaximumSize = new Size(90, 0);
            endLineNumberTextBox.Name = "endLineNumberTextBox";
            endLineNumberTextBox.Size = new Size(90, 23);
            endLineNumberTextBox.TabIndex = 8;
            endLineNumberTextBox.Text = "0"; //set 0 so that can be detected and autofilled later
            // easy GUI scroll to adjust line numbers
            endLineNumberTextBox.MouseEnter += (sender, e) => TextBox_MouseEnter(endLineNumberTextBox, e);
            endLineNumberTextBox.MouseLeave += (sender, e) => TextBox_MouseLeave(endLineNumberTextBox, e);

            fetchCodeStatusMessageLabel.AutoSize = true;
            fetchCodeStatusMessageLabel.Location = new Point(265, 7);
            fetchCodeStatusMessageLabel.Margin = new Padding(3, 7, 3, 0);
            fetchCodeStatusMessageLabel.Name = "fetchCodeStatusMessageLabel";
            fetchCodeStatusMessageLabel.Size = new Size(98, 15);
            fetchCodeStatusMessageLabel.TabIndex = 12;
            fetchCodeStatusMessageLabel.Text = "Ready Captain \U0001fae1";
            fetchCodeStatusMessageLabel.ForeColor = Color.Azure;

            preCodePromptLabel.AutoSize = true;
            preCodePromptLabel.Location = new Point(3, 71);
            preCodePromptLabel.Margin = new Padding(3, 7, 3, 0);
            preCodePromptLabel.Name = "preCodePromptLabel";
            preCodePromptLabel.Size = new Size(67, 15);
            preCodePromptLabel.TabIndex = 7;
            preCodePromptLabel.Text = "Pre Prompt";
            preCodePromptLabel.ForeColor = Color.Azure;

            preCodePromptTextBox.Dock = DockStyle.Fill;
            preCodePromptTextBox.Location = new Point(96, 67);
            preCodePromptTextBox.Name = "preCodePromptTextBox";
            preCodePromptTextBox.Size = new Size(672, 23);
            preCodePromptTextBox.TabIndex = 8;

            codeFileInjectLabel.AutoSize = true;
            codeFileInjectLabel.Location = new Point(3, 100);
            codeFileInjectLabel.Margin = new Padding(3, 7, 3, 0);
            codeFileInjectLabel.Name = "codeFileInjectLabel";
            codeFileInjectLabel.Size = new Size(56, 15);
            codeFileInjectLabel.TabIndex = 9;
            codeFileInjectLabel.Text = "Code File";
            codeFileInjectLabel.ForeColor = Color.Azure;


            // Create a new richTextBox
            codeFileInjectTextBox.Name = "codeFileInjectTextBox";
            codeFileInjectTextBox.BackColor = SystemColors.ActiveCaptionText;
            codeFileInjectTextBox.Dock = DockStyle.Fill;
            codeFileInjectTextBox.ForeColor = Color.Azure;
            codeFileInjectTextBox.Text = "";
            codeFileInjectTextBox.ReadOnly = false;
            codeFileInjectTextBox.Multiline = true; // Allow multiple lines
            codeFileInjectTextBox.WordWrap = true; // Wrap text to the next line
            codeFileInjectTextBox.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            codeFileInjectTextBox.MinimumSize = new Size(codeFileInjectTextBox.Width, 28);


            postCodePromptLabel.AutoSize = true;
            postCodePromptLabel.Location = new Point(3, 129);
            postCodePromptLabel.Margin = new Padding(3, 7, 3, 0);
            postCodePromptLabel.Name = "postCodePromptLabel";
            postCodePromptLabel.Size = new Size(73, 15);
            postCodePromptLabel.TabIndex = 11;
            postCodePromptLabel.Text = "Post Prompt";
            postCodePromptLabel.ForeColor = Color.Azure;

            postCodePromptTextBox.Dock = DockStyle.Fill;
            postCodePromptTextBox.Location = new Point(96, 125);
            postCodePromptTextBox.Name = "postCodePromptTextBox";
            postCodePromptTextBox.Size = new Size(672, 23);
            postCodePromptTextBox.TabIndex = 12;

            deleteCodeInjectButton.BackColor = Color.IndianRed;
            deleteCodeInjectButton.ForeColor = SystemColors.ButtonFace;
            deleteCodeInjectButton.Location = new Point(774, 3);
            deleteCodeInjectButton.Name = "deleteCodeInjectButton";
            deleteCodeInjectButton.Size = new Size(75, 23);
            deleteCodeInjectButton.TabIndex = 14;
            deleteCodeInjectButton.Text = "➖ Remove";
            deleteCodeInjectButton.UseVisualStyleBackColor = false;

            fetchLatestInjectedCodeButton.BackColor = Color.Fuchsia;
            fetchLatestInjectedCodeButton.ForeColor = SystemColors.ButtonFace;
            fetchLatestInjectedCodeButton.Location = new Point(774, 32);
            fetchLatestInjectedCodeButton.Name = "fetchLatestInjectedCodeButton";
            fetchLatestInjectedCodeButton.Size = new Size(75, 23);
            fetchLatestInjectedCodeButton.TabIndex = 16;
            fetchLatestInjectedCodeButton.Text = "⚡ Update";
            fetchLatestInjectedCodeButton.UseVisualStyleBackColor = false;
            fetchLatestInjectedCodeButton.Click += (sender, e) =>
            {
                FetchLatestInjectedCodeButton_Click(
                    codeFileInjectPathTextBox,
                    endLineNumberTextBox,
                    startLineNumberTextBox,
                    codeFileInjectTextBox,
                    preCodePromptTextBox,
                    postCodePromptTextBox,
                    fetchCodeStatusMessageLabel,
                    tokenLimitProgressBar, this.SelectedLLMConfig.MaxContextWindowTokens);
            };

            expandCodeFileButton.BackColor = SystemColors.MenuHighlight;
            expandCodeFileButton.ForeColor = SystemColors.ButtonFace;
            expandCodeFileButton.Location = new Point(774, 96);
            expandCodeFileButton.Name = "expandCodeFileButton";
            expandCodeFileButton.Size = new Size(75, 23);
            expandCodeFileButton.TabIndex = 18;
            expandCodeFileButton.Text = "Expand";
            expandCodeFileButton.UseVisualStyleBackColor = false;

            // Add codeFileInjectTablePanel to the stack with others
            codeFileInjectTabMainTablePanel.Controls.Add(codeFileInjectTablePanel, 0, 1);

        }

        private static void FetchLatestInjectedCodeButton_Click(
            TextBox codeFileInjectPathTextBox,
            TextBox endLineNumberTextBox,
            TextBox startLineNumberTextBox,
            RichTextBox codeFileInjectTextBox,
            TextBox preCodePromptTextBox,
            TextBox postCodePromptTextBox,
            Label fetchCodeStatusMessageLabel,
            CustomProgressBar tokenLimitProgressBar, int maxContextWindowTokens)
        {

            // get needed data to fetch file
            var filePath = codeFileInjectPathTextBox.Text;

            // if user has not set end line number then help user by auto filling
            if (endLineNumberTextBox.Text == "0") { endLineNumberTextBox.Text = GetMaxLinesInFile(filePath).ToString(); }
            var startLineNum = int.Parse(startLineNumberTextBox.Text);
            var endLineNum = int.Parse(endLineNumberTextBox.Text);

            // cut out piece of select code from file
            var extractedCode = ExtractOutSectionFromCodeFile(filePath, startLineNum, endLineNum);

            // put into view
            codeFileInjectTextBox.Text = extractedCode;

            // auto set probable pre and post prompt for file
            var codeFileName = GetCodeFileFullName(filePath);
            preCodePromptTextBox.Text = $"Analyse and parse below {codeFileName} code";
            postCodePromptTextBox.Text = $"Ok, I've parsed the code";

            // give user some stats to user about the fetched code
            var textSizeKB = GetBinarySizeOfTextInKB(extractedCode);
            fetchCodeStatusMessageLabel.Text = $"Parsed : Size : {textSizeKB:F2} KB";

            // update total token limit temperature bar
            // NOTE: These are just guidelines. Different types of language and different languages are tokenized in different ways.
            var maxTokenLimitInKb = EstimateTokenCountSizeKB(maxContextWindowTokens);
            var percentageUsed = (textSizeKB / maxTokenLimitInKb) * 100; // 128K tokens = 512KB
            percentageUsed = percentageUsed <= 100 ? percentageUsed : 100; // reset to 100% max
            tokenLimitProgressBar.Value = (int)percentageUsed;
            tokenLimitProgressBar.DisplayText = $"{textSizeKB:F2}/{maxTokenLimitInKb} KB";
            UpdateTokenLimitProgressBarColor(tokenLimitProgressBar);

        }


        /// <summary>
        /// on average, a token is about 4 characters
        /// on average, 100 tokens is about 75 words
        /// These are just guidelines.  Different types of language and
        /// different languages are tokenized in different ways.
        /// </summary>
        private static double EstimateTokenCountSizeKB(int tokenCount)
        {
            // Given input token count, convert to size in kilobytes
            // 1 token = 4 bytes
            double sizeInBytes = tokenCount * 4;  // Each token occupies 4 bytes
            double sizeInKB = sizeInBytes / 1024;  // Convert bytes to kilobytes
            return sizeInKB;
        }

        private static void UpdateTokenLimitProgressBarColor(CustomProgressBar tokenLimitProgressBar)
        {
            if (tokenLimitProgressBar.Value < 50)
            {
                tokenLimitProgressBar.ProgressBarColor = Color.Green;
            }
            else if (tokenLimitProgressBar.Value < 75)
            {
                // Linearly interpolate between green and yellow for values between 50 and 75
                float ratio = (tokenLimitProgressBar.Value - 50) / 25f;
                Color interpolatedColor = Interpolate(Color.Green, Color.Yellow, ratio);
                tokenLimitProgressBar.ProgressBarColor = interpolatedColor;
            }
            else if (tokenLimitProgressBar.Value < 100)
            {
                // Linearly interpolate between yellow and red for values between 75 and 100
                float ratio = (tokenLimitProgressBar.Value - 75) / 25f;
                Color interpolatedColor = Interpolate(Color.Yellow, Color.Red, ratio);
                tokenLimitProgressBar.ProgressBarColor = interpolatedColor;
            }
            else
            {
                tokenLimitProgressBar.ProgressBarColor = Color.Red;
            }
        }

        private static Color Interpolate(Color startColor, Color endColor, float ratio)
        {
            int r = (int)(startColor.R + ratio * (endColor.R - startColor.R));
            int g = (int)(startColor.G + ratio * (endColor.G - startColor.G));
            int b = (int)(startColor.B + ratio * (endColor.B - startColor.B));
            return Color.FromArgb(r, g, b);
        }

        private void SetTokenLimitProgressBarColor(Color color)
        {
            tokenLimitProgressBar.ForeColor = Color.Red;
            tokenLimitProgressBar.ForeColor = Color.Red; ;
        }

        /// <summary>
        /// given a large string text return its size in KB (kilobytes)
        /// </summary>
        private static double GetBinarySizeOfTextInKB(string largeText)
        {
            // Calculate the byte size of the string using Encoding.UTF8.GetBytes
            byte[] bytes = Encoding.UTF8.GetBytes(largeText);
            long byteSize = bytes.Length;

            // Convert byte size to kilobytes (KB)
            double kilobytes = byteSize / 1024.0;

            // Format the result as a string with two decimal places
            return kilobytes;
        }

        /// <summary>
        /// given a path to a code file like .js, .cs, .html, .py, etc...
        /// return full name of the file example out, javascript, csharp, html, python, etc...
        /// returns empty string if can't detect
        /// </summary>
        public static string GetCodeFileFullName(string filePath)
        {
            // Get the file extension from the file path
            string fileExtension = Path.GetExtension(filePath);

            // Create a dictionary to map file extensions to their corresponding full names
            var fileExtensionMap = new Dictionary<string, string>
            {
                { ".js", "JavaScript" },
                { ".cs", "C#" },
                { ".html", "HTML" },
                { ".py", "Python" },
                { ".java", "Java" },
                { ".cpp", "C++" },
                { ".c", "C" },
                { ".php", "PHP" },
                { ".rb", "Ruby" },
                { ".swift", "Swift" },
                { ".go", "Go" },
                { ".ts", "TypeScript" },
                { ".vb", "Visual Basic" },
                { ".sql", "SQL" },
            };

            // Return the full name of the file based on its extension
            return fileExtensionMap.TryGetValue(fileExtension.ToLower(), out string fullName) ? fullName : "";
        }

        private void UpdateSelectedLLMDropdownView()
        {
            foreach (var llmChoice in ApiConfigs)
            {
                llmSelector.Items.Add(llmChoice.Name);
            }
        }

        /// <summary>
        /// when user selects an LLM modal to use
        /// </summary>
        private void UpdateSelectedLLM(string selectedLLM)
        {
            //save end point to global instance for later use 
            this.SelectedLLMConfig = GetApiConfigByName(selectedLLM);

            client = new HttpClient();
            client.Timeout = Timeout.InfiniteTimeSpan;
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.SelectedLLMConfig.ApiKey);
            client.BaseAddress = new Uri(this.SelectedLLMConfig.Endpoint);

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

        public static ApiConfig GetApiConfigByName(string name)
        {
            foreach (var endpoint in ApiConfigs)
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
            // Use a regular expression to split the input string into tokens
            string[] tokens = Regex.Split(input, @"[^\w\s]+|\s+");

            // Filter out empty tokens
            tokens = Array.FindAll(tokens, token => !string.IsNullOrEmpty(token));

            return tokens.Length;
        }

        // Send a message to the LLM and return its response
        async Task<string> SendMessageToLLM(HttpClient client)
        {
            //STEP 1 : Inject selected sources Snippet/File/Web
            List<object> messages = new List<object>();

            //inject snippet
            if (snippetInjectCheckBox.Checked)
            {
                messages.AddRange(new List<object>
                {
                    new
                    {
                        role = "user",
                        content = $"{codeInjectPretextTextBox.Text}\n```\n{largeCodeSnippetTextBox.Text}\n```"
                    },
                    new { role = "assistant", content = $"{injectAssitantPretextTextBox.Text}" }
                });
            }

            //inject each code file properly
            if (fileInjectCheckBox.Checked)
            {
                //get all the files added as view component table
                foreach (Control control in codeFileInjectTabMainTablePanel.Controls)
                {
                    if (control is TableLayoutPanel fileInfoTable)
                    {
                        // Recursively search for the TextBox and RichTextBox controls
                        TextBox preCodePromptTextBox = FindControl<TextBox>(fileInfoTable, "preCodePromptTextBox");
                        RichTextBox codeFileInjectTextBox =FindControl<RichTextBox>(fileInfoTable, "codeFileInjectTextBox");
                        TextBox postCodePromptTextBox = FindControl<TextBox>(fileInfoTable, "postCodePromptTextBox");

                        //make sure all fields are filled for using
                        if (preCodePromptTextBox != null && codeFileInjectTextBox != null && postCodePromptTextBox != null)
                        {
                            string codePretext = preCodePromptTextBox.Text;
                            string extractedCodeText = codeFileInjectTextBox.Text;
                            string codePosttext = postCodePromptTextBox.Text;

                            messages.AddRange(new List<object>
                            {
                                new
                                {
                                    role = "user",
                                    content = $"{codePretext}\n```\n{extractedCodeText}\n```"
                                },
                                new { role = "assistant", content = $"{codePosttext}" }
                            });
                        }
                    }
                }
            }

            //add in current ongoing conversation at end of chat history  
            foreach (var message in conversationHistory)
            {
                messages.Add(new { role = message.Role, content = message.Content });
            }

            //STEP 2: 
            //package message with llm control options
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

            //prepare for boarding 🛫
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

            return replyMessage;
        }

        private T FindControl<T>(Control control, string name) where T : Control
        {
            return control.Controls.OfType<T>().FirstOrDefault(c => c.Name == name);
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

        /// <summary>
        /// Given a path to a code file, extract only from input line number range
        /// </summary>
        static string ExtractOutSectionFromCodeFile(string filePath, int startLineNum, int endLineNum)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentException("File path cannot be null or empty", nameof(filePath));
            }

            if (startLineNum < 1 || endLineNum < 1)
            {
                throw new ArgumentException("Line numbers must be positive integers", nameof(startLineNum));
            }

            if (startLineNum > endLineNum)
            {
                throw new ArgumentException("Start line number must be less than or equal to end line number", nameof(startLineNum));
            }

            try
            {
                var lines = File.ReadAllLines(filePath);
                var extractedLines = lines.Skip(startLineNum - 1).Take(endLineNum - startLineNum + 1);
                return string.Join(Environment.NewLine, extractedLines);
            }
            catch (FileNotFoundException ex)
            {
                throw new FileNotFoundException($"File not found: {filePath}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error reading file: {filePath}", ex);
            }
        }

        /// <summary>
        /// Given a path to a code file, return the maximum number of lines in the file
        /// </summary>
        static int GetMaxLinesInFile(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                return 0;
            }

            try
            {
                var lines = File.ReadAllLines(filePath);
                return lines.Length;
            }
            catch (FileNotFoundException)
            {
                return 0;
            }
            catch (Exception)
            {
                return 0;
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

            // Start the timer
            var stopwatch = Stopwatch.StartNew();
            var timerTask = Task.Run(async () =>
            {
                while (stopwatch.IsRunning)
                {
                    await Task.Delay(200); // Update every second
                    this.Invoke((MethodInvoker)delegate
                    {
                        llmRunningTimerLabel.Text = $"{Math.Round(stopwatch.Elapsed.TotalSeconds, 2)} s";
                    });
                }
            });


            // Update the progress bar visibility and value
            progressBar1.Value = 25;

            // Get the user input from the input text box
            string userInput = userInputTextBox.Text;

            // Display the user's message in the chat output box
            AddMessageToPanel(userInput, "user", Color.Aqua);

            progressBar1.Value = 50;

            // Send a message to the LLM and get its response
            string response = await SendMessageToLLM(client);

            progressBar1.Value = 75;

            // Display the LLM's response in the chat output box
            AddMessageToPanel(response, "assistant", Color.LimeGreen);

            // Clear the user input text box for the next input
            userInputTextBox.Text = "";

            // Load and display past user prompts as templates
            UpdatePastPromptsView();

            // Update the progress bar value and hide it
            progressBar1.Value = 100;
            await Task.Delay(300); //short delay
            progressBar1.Value = 0;

            // Stop the timer
            stopwatch.Stop();
            await timerTask;

        }

        private void largeCodeSnippetTextBox_TextChanged(object sender, EventArgs e)
        {
            //update token counter
            tokenCountLabel.Text = $"Token Count : {CountTokens(largeCodeSnippetTextBox.Text)}";
        }

        private void Form1_Load(object sender, EventArgs e)
        {


        }

        // New method to update a message in the conversation history
        private void UpdateMessageText(string messageId, string newMessageText)
        {
            // Find the message in the conversation history
            var messageToUpdate = conversationHistory.FirstOrDefault(m => m.Id == messageId);

            // Update the message text if the message is found
            if (messageToUpdate != null)
            {
                messageToUpdate.Content = newMessageText;
            }

            // Log the updated message to the chat history file
            LogChatMessageToFile(messageToUpdate);
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
            TableLayoutPanel chatMsgHolderTable = new TableLayoutPanel();
            chatMsgHolderTable.AutoSize = true;
            chatMsgHolderTable.ColumnCount = 2;
            chatMsgHolderTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            chatMsgHolderTable.RowCount = 1;
            chatMsgHolderTable.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            chatMsgHolderTable.Dock = DockStyle.Fill;

            // Create a new richTextBox
            RichTextBox richTextBox = new RichTextBox();
            richTextBox.BackColor = SystemColors.ActiveCaptionText;
            richTextBox.Dock = DockStyle.Fill;
            richTextBox.ForeColor = color;
            richTextBox.Text = message;
            richTextBox.ReadOnly = false;
            richTextBox.Multiline = true; // Allow multiple lines
            richTextBox.WordWrap = true; // Wrap text to the next line
            richTextBox.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            richTextBox.MinimumSize = new Size(richTextBox.Width, 28);
            richTextBox.TextChanged += (sender, e) =>
            {
                string msgId = (string)chatMsgHolderTable.Tag;
                UpdateMessageText(msgId, richTextBox.Text);
            };

            // Create a new delete button
            Button deleteButton = new Button();
            deleteButton.BackColor = Color.IndianRed;
            deleteButton.ForeColor = SystemColors.ButtonFace;
            deleteButton.Text = "Delete";
            deleteButton.Tag = messageId; // Store the message ID in the button's Tag property
            deleteButton.Click += (sender, e) => DeleteMessage(chatMsgHolderTable, (string)((Button)sender).Tag);

            // Create a new expand/collapse button
            Button expandButton = new Button();
            expandButton.BackColor = Color.DodgerBlue;
            expandButton.ForeColor = SystemColors.ButtonFace;
            expandButton.Text = "Collapse";
            expandButton.Tag = richTextBox; // Store the richTextBox in the button's Tag property
            expandButton.Click += (sender, e) =>
            {
                RichTextBox rtb = (RichTextBox)((Button)sender).Tag;
                if (((Button)sender).Text == "Collapse")
                {
                    rtb.MinimumSize = new Size(rtb.Width, 28);
                    rtb.MaximumSize = new Size(rtb.Width, 28);
                    ((Button)sender).Text = "Expand";
                }
                //expand button click logic
                else
                {
                    rtb.MinimumSize = new Size(rtb.Width, 0);
                    rtb.MaximumSize = new Size(rtb.Width, int.MaxValue);
                    rtb.Height = rtb.GetPreferredSize(new Size(rtb.Width, int.MaxValue)).Height;
                    ((Button)sender).Text = "Collapse";
                }
            };

            // Add the richTextBox and buttons to the tableLayoutPanel
            chatMsgHolderTable.Controls.Add(richTextBox, 0, 0);

            //table to hold buttons
            TableLayoutPanel buttonHolderTable = new TableLayoutPanel();
            buttonHolderTable.AutoSize = true;
            buttonHolderTable.ColumnCount = 1;
            buttonHolderTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            buttonHolderTable.RowCount = 2;
            buttonHolderTable.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            buttonHolderTable.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            buttonHolderTable.Dock = DockStyle.Fill;

            buttonHolderTable.Controls.Add(deleteButton, 0, 0);
            buttonHolderTable.Controls.Add(expandButton, 0, 1);

            chatMsgHolderTable.Controls.Add(buttonHolderTable, 1, 0);

            // Store the message ID in the Tag property of the TableLayoutPanel
            chatMsgHolderTable.Tag = messageId;

            // Add the tableLayoutPanel to the chatMessagePanel
            chatMessagePanel.Controls.Add(chatMsgHolderTable);

            // Store the message ID and tableLayoutPanel in a dictionary
            messageTableLayouts[messageId] = chatMsgHolderTable;
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

        private void addNewCodeFileInjectButton_Click(object sender, EventArgs e)
        {
            InitializeCodeFileInjectTablePanel();
        }

        private void LoadApiConfigsFromConfigFile()
        {
            try
            {
                string configFileContent = File.ReadAllText("secrets.json");
                ConfigFile configFile = JsonConvert.DeserializeObject<ConfigFile>(configFileContent);
                ApiConfigs = configFile.ApiConfigs;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading config file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }

    public record ApiConfig(string Name, string Endpoint, string ApiKey, int MaxContextWindowTokens);

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

    public class CustomProgressBar : ProgressBar
    {
        private Color _progressBarColor = Color.LightGreen;
        private string _displayText = string.Empty;

        public Color ProgressBarColor
        {
            get { return _progressBarColor; }
            set { _progressBarColor = value; this.Invalidate(); }
        }

        public string DisplayText
        {
            get { return _displayText; }
            set { _displayText = value; this.Invalidate(); }
        }

        public CustomProgressBar()
        {
            this.SetStyle(ControlStyles.UserPaint, true);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Rectangle rect = this.ClientRectangle;
            Graphics g = e.Graphics;

            float percent = (this.Value / (float)this.Maximum);

            // Draw the background of the progress bar
            g.FillRectangle(new SolidBrush(Color.White), rect);

            // Draw the progress bar
            g.FillRectangle(new SolidBrush(_progressBarColor), new Rectangle(rect.X, rect.Y, (int)(rect.Width * percent), rect.Height));

            // Draw the border of the progress bar
            g.DrawRectangle(new Pen(Color.Black), rect);

            // Draw the display text
            if (!string.IsNullOrEmpty(_displayText))
            {
                StringFormat sf = new StringFormat(StringFormatFlags.MeasureTrailingSpaces);
                sf.Alignment = StringAlignment.Center;
                sf.LineAlignment = StringAlignment.Center;
                var font = new Font("Segoe UI", 9F, FontStyle.Bold);
                g.DrawString(_displayText, font, new SolidBrush(Color.DarkBlue), rect, sf);
            }
        }
    }

    public class ConfigFile
    {
        [JsonPropertyName("ApiConfigs")]
        public List<ApiConfig> ApiConfigs { get; set; }
    }

}
