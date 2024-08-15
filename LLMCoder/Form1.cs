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
        /// <summary>
        /// keeps all files added to file injection list in current runtime
        /// </summary>
        private List<CodeFile> CurrentCodeFiles = new List<CodeFile>();

        /// <summary>
        /// current global preset list
        /// </summary>
        private List<FileInjectPreset> CurrentFileInjectPresets = new List<FileInjectPreset>();

        /// <summary>
        /// so that can kill an ongoing http call
        /// </summary>
        private CancellationTokenSource _cts;

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

            //windows forms loading of designer.cs
            InitializeComponent();

            // Call the LoadPresetsFromFile method to load presets from the presets.json file.
            LoadPresetsFromFile();

            // Update the dropdown with available LLM choices
            InitializeSelectedLLMDropdownView();

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
            snippetInjectCheckBox.Checked = false;
            fileInjectCheckBox.Checked = true;

            // Set the default LLM choice
            UpdateSelectedLLM("MetaLlama31405B");

        }

        private void AddNewFileInjectToVisibleList(CodeFile codeFile = null)
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

            //generates GUI
            var newFileInjectDataHolder = GenerateNewCodeFileInjectTable();

            // Add codeFileInjectTablePanel as last new row in table selectedCodeFileViewTable
            selectedCodeFileViewTable.RowCount++;
            selectedCodeFileViewTable.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            selectedCodeFileViewTable.Controls.Add(newFileInjectDataHolder, 0, selectedCodeFileViewTable.RowCount - 1);

            //if codeFile is passed in, fill the textbox values with data from presets
            if (codeFile != null)
            {
                codeFileInjectPathTextBox.Text = codeFile.FilePath;
                startLineNumberTextBox.Text = codeFile.StartLineNumber.ToString();
                endLineNumberTextBox.Text = codeFile.EndLineNumber.ToString();
                preCodePromptTextBox.Text = codeFile.PrePrompt;
                codeFileInjectTextBox.Text = codeFile.ExtractedCode;
                postCodePromptTextBox.Text = codeFile.PostPrompt;
            }

            Control GenerateNewCodeFileInjectTable()
            {

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
                codeFileInjectTablePanel.Dock = DockStyle.None;
                codeFileInjectTablePanel.Location = new Point(0, 0);
                codeFileInjectTablePanel.Name = "codeFileInjectTablePanel";
                codeFileInjectTablePanel.RowCount = 6;
                codeFileInjectTablePanel.RowStyles.Add(new RowStyle() { SizeType = SizeType.AutoSize });
                codeFileInjectTablePanel.RowStyles.Add(new RowStyle());
                codeFileInjectTablePanel.RowStyles.Add(new RowStyle());
                codeFileInjectTablePanel.RowStyles.Add(new RowStyle());
                codeFileInjectTablePanel.RowStyles.Add(new RowStyle());
                codeFileInjectTablePanel.RowStyles.Add(new RowStyle());
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

                fetchCodeStatusMessageLabel.AutoSize = true;
                fetchCodeStatusMessageLabel.Location = new Point(265, 7);
                fetchCodeStatusMessageLabel.Margin = new Padding(3, 7, 3, 0);
                fetchCodeStatusMessageLabel.Name = "fetchCodeStatusMessageLabel";
                fetchCodeStatusMessageLabel.Size = new Size(98, 15);
                fetchCodeStatusMessageLabel.TabIndex = 12;
                fetchCodeStatusMessageLabel.ForeColor = Color.Azure;

                // update token stats for current file only
                var textSizeKbCurrent = GetBinarySizeOfTextInKB(codeFile?.ExtractedCode ?? "");
                var textSizeToken = ConvertKBToTokenCount(textSizeKbCurrent);
                fetchCodeStatusMessageLabel.Text = $"Size : {textSizeKbCurrent:F2} KB ~ {textSizeToken} Tokens";


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
                codeFileInjectTextBox.ForeColor = Color.LightGreen;
                codeFileInjectTextBox.Text = "";
                codeFileInjectTextBox.ReadOnly = false;
                codeFileInjectTextBox.Multiline = true; // Allow multiple lines
                codeFileInjectTextBox.WordWrap = true; // Wrap text to the next line
                codeFileInjectTextBox.Font = new Font("Cascadia Code", 8F, FontStyle.Regular, GraphicsUnit.Point, 0);
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
                deleteCodeInjectButton.Click += (sender, e) =>
                {
                    // Get the parent table layout panel
                    TableLayoutPanel codeFileInjectTablePanel = (TableLayoutPanel)((Button)sender).Parent;

                    // Remove the table layout panel from the main table layout panel
                    selectedCodeFileViewTable.Controls.Remove(codeFileInjectTablePanel);

                    // Dispose of the table layout panel
                    codeFileInjectTablePanel.Dispose();

                    // Find the CodeFile instance to remove
                    string filePath = null;
                    foreach (Control control in codeFileInjectTablePanel.Controls)
                    {
                        if (control is TextBox textBox && textBox.Name == "codeFileInjectPathTextBox")
                        {
                            filePath = textBox.Text;
                            break;
                        }
                    }

                    // Remove the CodeFile instance from the CurrentCodeFiles list
                    if (filePath != null)
                    {
                        this.CurrentCodeFiles.RemoveAll(cf => cf.FilePath == filePath);
                    }

                    // Update Token Stats
                    UpdateGlobalTokenStats();

                    //make save button visible since changes made
                    saveCodeFileInjectPresetButton.Visible = true;
                };

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
                    // get needed data to fetch file
                    var filePath = codeFileInjectPathTextBox.Text;

                    // if user has not set end line number then help user by auto-filling
                    if (endLineNumberTextBox.Text == "0") { endLineNumberTextBox.Text = GetMaxLinesInFile(filePath).ToString(); }
                    var startLineNum = int.Parse(startLineNumberTextBox.Text);
                    var endLineNum = int.Parse(endLineNumberTextBox.Text);

                    // cut out piece of select code from file
                    var currentExtractedCode = ExtractOutSectionFromCodeFile(filePath, startLineNum, endLineNum);

                    // put extracted code into view
                    codeFileInjectTextBox.Text = currentExtractedCode;

                    // auto set probable pre and post prompt for file
                    var codeFileName = GetCodeFileFullName(filePath);
                    preCodePromptTextBox.Text = $"Analyse and parse below {codeFileName} code";
                    postCodePromptTextBox.Text = $"Ok, I've parsed the code";

                    // update token stats for current file only
                    var textSizeKbCurrent = GetBinarySizeOfTextInKB(currentExtractedCode);
                    var textSizeToken = ConvertKBToTokenCount(textSizeKbCurrent);
                    fetchCodeStatusMessageLabel.Text = $"Size : {textSizeKbCurrent:F2} KB ~ {textSizeToken} Tokens";

                    // update global token stats
                    UpdateGlobalTokenStats();

                    //package data for storing
                    // Create a new CodeFile object with the current values from the text boxes
                    var updatedCodeFile = new CodeFile(
                        codeFileInjectPathTextBox.Text, // File path
                        int.Parse(startLineNumberTextBox.Text), // Start line number
                        int.Parse(endLineNumberTextBox.Text), // End line number
                        preCodePromptTextBox.Text, // Pre-prompt text
                        codeFileInjectTextBox.Text, // Extracted code text
                        postCodePromptTextBox.Text // Post-prompt text
                    );

                    // update global selected code file data list
                    UpdateCurrentCodeFilesGlobalList(updatedCodeFile);

                    //make save button visible since changes made
                    saveCodeFileInjectPresetButton.Visible = true;
                };

                expandCodeFileButton.BackColor = SystemColors.MenuHighlight;
                expandCodeFileButton.ForeColor = SystemColors.ButtonFace;
                expandCodeFileButton.Location = new Point(774, 96);
                expandCodeFileButton.Name = "expandCodeFileButton";
                expandCodeFileButton.Size = new Size(75, 23);
                expandCodeFileButton.TabIndex = 18;
                expandCodeFileButton.Text = "Expand";
                expandCodeFileButton.UseVisualStyleBackColor = false;
                expandCodeFileButton.Click += (sender, e) =>
                {
                    // Check if the button is currently in the "Expand" state
                    if (expandCodeFileButton.Text == "Expand")
                    {
                        // Remove the minimum height constraint to allow the text box to expand
                        codeFileInjectTextBox.MinimumSize = new Size(codeFileInjectTextBox.Width, 0);

                        // Set the maximum height to infinity to allow the text box to expand as much as needed
                        codeFileInjectTextBox.MaximumSize = new Size(codeFileInjectTextBox.Width, int.MaxValue);

                        // Calculate the preferred height of the text box based on its width and content
                        codeFileInjectTextBox.Height = 300;

                        // Update the button text to "Collapse" to reflect the new state
                        expandCodeFileButton.Text = "Collapse";
                    }
                    //expand button click logic
                    else
                    {
                        // Set the minimum height to 28 pixels to collapse the text box
                        codeFileInjectTextBox.MinimumSize = new Size(codeFileInjectTextBox.Width, 28);

                        // Set the maximum height to 28 pixels to prevent the text box from expanding
                        codeFileInjectTextBox.MaximumSize = new Size(codeFileInjectTextBox.Width, 28);

                        // Update the button text to "Expand" to reflect the new state
                        expandCodeFileButton.Text = "Expand";
                    }
                };

                return codeFileInjectTablePanel;
            }
        }

        /// <summary>
        /// add given code file to main list if new else updates existing, uses file path as id 
        /// </summary>
        private void UpdateCurrentCodeFilesGlobalList(CodeFile updatedCodeFile)
        {
            // Check if a CodeFile with the same file path already exists in the CurrentCodeFiles list
            var existingCodeFile = this.CurrentCodeFiles.FirstOrDefault(
                cf => cf.FilePath == updatedCodeFile.FilePath // Compare file paths
            );

            // If a CodeFile with the same file path already exists
            if (existingCodeFile != null)
            {
                // Update the existing CodeFile's start line number
                existingCodeFile.StartLineNumber = updatedCodeFile.StartLineNumber;

                // Update the existing CodeFile's end line number
                existingCodeFile.EndLineNumber = updatedCodeFile.EndLineNumber;

                // Update the existing CodeFile's pre-prompt text
                existingCodeFile.PrePrompt = updatedCodeFile.PrePrompt;

                // Update the existing CodeFile's extracted code text
                existingCodeFile.ExtractedCode = updatedCodeFile.ExtractedCode;

                // Update the existing CodeFile's post-prompt text
                existingCodeFile.PostPrompt = updatedCodeFile.PostPrompt;
            }
            // If a CodeFile with the same file path does not exist
            else
            {
                // Add the new CodeFile to the CurrentCodeFiles list
                this.CurrentCodeFiles.Add(updatedCodeFile);
            }
        }

        private void UpdateGlobalTokenStats()
        {
            //#2 total context window by all (KB)
            //PERCENTAGE
            // Calculate the total size of all extracted codes
            double totalTextSizeKb = GetTotalChatMessageSizeInKiloBytes();
            // Update the usage counter text
            var maxTokenLimitInKb = ConvertTokenCountToKB(this.SelectedLLMConfig.MaxContextWindowTokens);
            var byteUsageMeterText = $"{totalTextSizeKb:F2}/{maxTokenLimitInKb} KB";
            totalByteUsageMeterTextLabel.Text = byteUsageMeterText;

            //#3 total context window by all (Tokens)
            //TEXT
            var maxLlmContextWindowTokens = FormatNumberWithKAbbreviation(SelectedLLMConfig.MaxContextWindowTokens);
            var totalChatMessageSizeInTokens = GetTotalChatMessageSizeInTokens();
            var usedContextTokens = FormatNumberWithKAbbreviation(totalChatMessageSizeInTokens);
            var totalTokenUsageMeterText = $"{usedContextTokens} / {maxLlmContextWindowTokens} Tokens";
            finalChatTokenUsageProgressBar.DisplayText = totalTokenUsageMeterText;
            //PERCENTAGE
            double percentageTotalUsed = ((double)totalChatMessageSizeInTokens / (double)SelectedLLMConfig.MaxContextWindowTokens) * 100;
            percentageTotalUsed = percentageTotalUsed <= 100 ? percentageTotalUsed : 100; // reset to 100% max if exceed
            finalChatTokenUsageProgressBar.Value = (int)percentageTotalUsed;
            UpdateTokenLimitProgressBarColor(finalChatTokenUsageProgressBar); //update meter color
        }

        /// <summary>
        /// given a number like 32000, return text version with K abbreviation example : 32000 = "32K"
        /// </summary>
        private static string FormatNumberWithKAbbreviation(int maxContextWindowTokens)
        {
            if (maxContextWindowTokens < 1000)
            {
                return maxContextWindowTokens.ToString();
            }

            double thousands = (double)maxContextWindowTokens / 1000.0;

            // Format the number with one decimal place if the fractional part is not zero
            if (maxContextWindowTokens % 1000 != 0)
            {
                return $"{thousands:F1}K";
            }

            return $"{thousands}K";

        }

        /// <summary>
        /// on average, a token is about 4 characters
        /// on average, 100 tokens is about 75 words
        /// These are just guidelines.  Different types of language and
        /// different languages are tokenized in different ways.
        /// </summary>
        private static double ConvertTokenCountToKB(int tokenCount)
        {
            // Given input token count, convert to size in kilobytes
            // 1 token = 4 bytes
            double sizeInBytes = tokenCount * 4;  // Each token occupies 4 bytes
            double sizeInKB = sizeInBytes / 1024;  // Convert bytes to kilobytes
            return sizeInKB;
        }

        private static int ConvertKBToTokenCount(double inputKb)
        {
            // Convert kilobytes to bytes
            double sizeInBytes = inputKb * 1024;

            // 1 token = 4 bytes, so divide total bytes by 4 to get token count
            double tokenCount = sizeInBytes / 4;

            return (int)tokenCount;
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

        /// <summary>
        /// given a large string text return its size in KB (kilobytes)
        /// </summary>
        private static double GetBinarySizeOfTextInKB(string largeText)
        {
            if (string.IsNullOrEmpty(largeText)) { return 0; } //if empty end as 0

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

        private void InitializeSelectedLLMDropdownView()
        {
            foreach (var llmChoice in ApiConfigs)
            {
                llmSelector.Items.Add(llmChoice.Name);
            }
        }

        /// <summary>
        /// when user selects/changes an LLM modal to use during runtime
        /// </summary>
        private void UpdateSelectedLLM(string selectedLLM)
        {
            //save end point to global instance for later use 
            this.SelectedLLMConfig = GetApiConfigByName(selectedLLM);

            //update llm HTTP caller
            client = new HttpClient();
            client.Timeout = Timeout.InfiniteTimeSpan;
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.SelectedLLMConfig.ApiKey);
            client.BaseAddress = new Uri(this.SelectedLLMConfig.Endpoint);
            this.llmThinkingProgressBar.Value = 0; //NOTE: reset since previous Call would be dropped if done mid-way of stuck llm

            //update drop down selection
            llmSelector.SelectedIndex = llmSelector.Items.IndexOf(selectedLLM);


            // Update the usage counter text
            var currentRawText = totalByteUsageMeterTextLabel.Text; // sample: "230 / 500 KB"
            var parts = currentRawText.Split('/');
            double divisor1 = double.Parse(parts[0].Trim()); // should be 230

            //update main KB meter
            var maxLlmContextWindowKiloBytes = ConvertTokenCountToKB(this.SelectedLLMConfig.MaxContextWindowTokens);
            totalByteUsageMeterTextLabel.Text = $"{divisor1} / {maxLlmContextWindowKiloBytes} KB";

            //update main Token meter
            //text
            var maxLlmContextWindowTokens = FormatNumberWithKAbbreviation(SelectedLLMConfig.MaxContextWindowTokens);
            var chatMessageSizeInTokens = GetTotalChatMessageSizeInTokens();
            var tokenUsageMeterText = $"{FormatNumberWithKAbbreviation(chatMessageSizeInTokens)} / {maxLlmContextWindowTokens} Tokens";
            finalChatTokenUsageProgressBar.DisplayText = tokenUsageMeterText;
            //percentage
            double percentageTotalUsed = ((double)chatMessageSizeInTokens / (double)SelectedLLMConfig.MaxContextWindowTokens) * 100;
            percentageTotalUsed = percentageTotalUsed <= 100 ? percentageTotalUsed : 100; // reset to 100% max if exceed
            finalChatTokenUsageProgressBar.Value = (int)percentageTotalUsed;
            UpdateTokenLimitProgressBarColor(finalChatTokenUsageProgressBar); //update meter color

        }

        private int GetTotalChatMessageSizeInTokens()
        {
            //create final message history going to LLM
            var generateFinalChatMessageStack = GenerateFinalChatMessageStack();
            var finalMessageText = JsonConvert.SerializeObject(generateFinalChatMessageStack);

            //calculate token count of total text
            var textSizeKB = GetBinarySizeOfTextInKB(finalMessageText);
            var textSizeToken = ConvertKBToTokenCount(textSizeKB);

            return textSizeToken;
        }

        private double GetTotalChatMessageSizeInKiloBytes()
        {
            //create final message history going to LLM
            var generateFinalChatMessageStack = GenerateFinalChatMessageStack();
            var finalMessageText = JsonConvert.SerializeObject(generateFinalChatMessageStack);

            //calculate token count of total text
            var textSizeKB = GetBinarySizeOfTextInKB(finalMessageText);

            return textSizeKB;
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
            // Create a new CancellationTokenSource
            _cts = new CancellationTokenSource();

            try
            {
                // STEP 1 : get all messages to send to LLM stacked properly
                var finalChatMessageStack = GenerateFinalChatMessageStack();

                // prepare for boarding 
                var requestBody = JsonConvert.SerializeObject(finalChatMessageStack);
                var content = new StringContent(requestBody);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                //make call to API with cancellation support
                var response = await client.PostAsync("", content, _cts.Token);

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
            catch (OperationCanceledException)
            {
                // Handle cancellation
                AddMessageToPanel("LLM call cancelled.", "assistant", Color.Red);
                return string.Empty;
            }
            catch (Exception ex)
            {
                // Handle other exceptions
                AddMessageToPanel($"Error: {ex.Message}", "assistant", Color.Red);
                return string.Empty;
            }
            finally
            {
                // Dispose of the CancellationTokenSource
                _cts.Dispose();
            }

        }

        /// <summary>
        /// Puts together all chat messages and LLM settings for final POST data sent to LLM
        /// </summary>
        private object? GenerateFinalChatMessageStack()
        {
            List<object> messages = new List<object>();

            //STEP 1 : Inject selected sources Snippet/File/Web

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
                foreach (Control control in selectedCodeFileViewTable.Controls)
                {
                    if (control is TableLayoutPanel fileInfoTable)
                    {
                        // Recursively search for the TextBox and RichTextBox controls
                        TextBox preCodePromptTextBox = FindControl<TextBox>(fileInfoTable, "preCodePromptTextBox");
                        RichTextBox codeFileInjectTextBox = FindControl<RichTextBox>(fileInfoTable, "codeFileInjectTextBox");
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
            var tempValue = string.IsNullOrEmpty(temperatureTextBox.Text) ? "0" : temperatureTextBox.Text; //auto set 0 for startup
            var topPValue = string.IsNullOrEmpty(topPTextBox.Text) ? "0" : topPTextBox.Text;
            var requestBodyObject = new
            {
                messages, // List of messages
                //max_tokens = 32000, // Maximum number of tokens for the response
                //max_tokens = 4096, // Maximum number of tokens for the response
                temperature = double.Parse(tempValue), // Temperature for sampling
                top_p = double.Parse(topPValue), // Nucleus sampling parameter
                //presence_penalty = 0,
                //frequency_penalty = 0
            };

            return requestBodyObject;
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

        // This method loads presets from the presets.json file into the global data for this instance.
        private void LoadPresetsFromFile()
        {
            // Try to load the presets from the file. If any errors occur, catch the exception and display an error message.
            try
            {
                //add in default unselected state
                CurrentFileInjectPresets = new List<FileInjectPreset>();

                // Add a default "Select..." preset to the list
                CurrentFileInjectPresets.Add(new FileInjectPreset { Name = "Select...", InjectedFilesData = new List<CodeFile>() });

                // Check if the presets.json file exists in the current directory.
                if (File.Exists("presets.json"))
                {
                    // Read the contents of the presets.json file into a string.
                    string presetsJson = File.ReadAllText("presets.json");

                    // Deserialize the JSON data into a list of FileInjectPreset objects and assign it to the CurrentFileInjectPresets variable.
                    var parsedFromFile = JsonConvert.DeserializeObject<List<FileInjectPreset>>(presetsJson);
                    CurrentFileInjectPresets.AddRange(parsedFromFile);
                }

                // Update the presetSelectComboBox with the loaded presets
                presetSelectComboBox.DataSource = CurrentFileInjectPresets;
            }
            // Catch any exceptions that occur during the loading process.
            catch (Exception ex)
            {
                // Display an error message to the user if an exception occurs.
                MessageBox.Show($"Error loading presets from file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
            richTextBox.Font = new Font("Cascadia Code", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            richTextBox.AutoSize = true;
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
            outputChatMessagePanel.Controls.Add(chatMsgHolderTable);

            // Store the message ID and tableLayoutPanel in a dictionary
            messageTableLayouts[messageId] = chatMsgHolderTable;

            //update counter
            UpdateGlobalTokenStats();
        }

        // New method to delete a message from the chat message panel
        private void DeleteMessage(TableLayoutPanel tableLayoutPanel, string messageId)
        {
            // Remove the message from the conversation history
            conversationHistory.RemoveAll(m => m.Id == messageId);

            // Remove the tableLayoutPanel from the chatMessagePanel
            outputChatMessagePanel.Controls.Remove(tableLayoutPanel);

            // Dispose of the tableLayoutPanel
            tableLayoutPanel.Dispose();

            // Remove the message ID from the dictionary
            messageTableLayouts.Remove(messageId);
        }

        private string GetPresetNameFromUser()
        {
            // Open a dialog window to get the preset name from the user
            InputDialog dialog = new InputDialog("Enter preset name:", "Save Preset");
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                return dialog.InputText;
            }
            return string.Empty;
        }

        //saves all preset from current runtime to local file, overwrites file
        private void SavePresetsToFile()
        {
            // Create a new list of FileInjectPreset objects that excludes the default "Select..." preset
            var presetsToSave = CurrentFileInjectPresets.Where(p => p.Name != "Select...").ToList();

            // Save the updated preset list to a file (optional)
            string presetsJson = JsonConvert.SerializeObject(presetsToSave);
            File.WriteAllText("presets.json", presetsJson);

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

        private void resetChatHistoryButton_Click(object sender, EventArgs e)
        {
            // Clear the chat output box text
            outputChatMessagePanel.Controls.Clear();

            // Clear the conversation history list
            conversationHistory.Clear();
        }

        // Event handler for the send user message button click
        private async void sendUserMsgButton_Click(object sender, EventArgs e)
        {
            //let user know status
            llmThinkingLabel.Visible = true;
            llmThinkingLabel.Text = "LLM Thinking...";

            //make cancel button visible
            terminateOngoingLLMCallButton.Visible = true;

            // Start the timer
            var stopwatch = Stopwatch.StartNew();
            var timerTask = Task.Run(async () =>
            {
                while (stopwatch.IsRunning)
                {
                    await Task.Delay(200); // Update every second
                    this.Invoke((MethodInvoker)delegate
                    {
                        llmThinkingTimerLabel.Text = $"{Math.Round(stopwatch.Elapsed.TotalSeconds, 2)} s";
                    });
                }
            });

            // Update the progress bar visibility and value
            llmThinkingProgressBar.Value = 25;

            // Get the user input from the input text box
            string userInput = userInputTextBox.Text;

            // Display the user's message in the chat output box
            AddMessageToPanel(userInput, "user", Color.Aqua);

            llmThinkingProgressBar.Value = 50;

            // Send a message to the LLM and get its response
            string response = await SendMessageToLLM(client);

            llmThinkingProgressBar.Value = 75;

            // Display the LLM's response in the chat output box
            AddMessageToPanel(response, "assistant", Color.LimeGreen);

            // Clear the user input text box for the next input
            userInputTextBox.Text = "";

            // Load and display past user prompts as templates
            UpdatePastPromptsView();

            // Update the progress bar value and hide it
            llmThinkingProgressBar.Value = 100;
            await Task.Delay(300); //short delay
            llmThinkingProgressBar.Value = 0;

            // Stop the timer
            stopwatch.Stop();
            await timerTask;

            //update message
            llmThinkingLabel.Text = "✅ Done";

            //make cancel button disappear, since not needed anymore
            terminateOngoingLLMCallButton.Visible = false;
        }

        private void largeCodeSnippetTextBox_TextChanged(object sender, EventArgs e)
        {
            //update token counter
            tokenCountLabel.Text = $"Token Count : {CountTokens(largeCodeSnippetTextBox.Text)}";
        }

        private void addNewCodeFileInjectButton_Click(object sender, EventArgs e)
        {
            //make save button visible since edited
            saveCodeFileInjectPresetButton.Visible = true;

            //get values from on page new form and to main list
            var newCodeFile = new CodeFile(
                codeFileInjectPathTextBox.Text, // File path
                int.Parse(startLineNumberTextBox.Text), // Start line number
                int.Parse(endLineNumberTextBox.Text), // End line number
                preCodePromptTextBox.Text, // Pre-prompt text
                codeFileInjectTextBox.Text, // Extracted code text
                postCodePromptTextBox.Text // Post-prompt text
            );

            //add to main list
            AddNewFileInjectToVisibleList(newCodeFile);

            //add to global data list
            UpdateCurrentCodeFilesGlobalList(newCodeFile);

            //update global token stats (last to get data)
            UpdateGlobalTokenStats();

            //clear data from entry form
            codeFileInjectPathTextBox.Text = ""; // File path
            startLineNumberTextBox.Text = "1"; // Start line number
            endLineNumberTextBox.Text = "0";// End line number
            preCodePromptTextBox.Text = ""; // Pre-prompt text
            codeFileInjectTextBox.Text = ""; // Extracted code text
            postCodePromptTextBox.Text = ""; // Post-prompt text
            fetchCodeStatusMessageLabel.Text = ""; //stats

        }

        private void llmSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Cast the sender as a ComboBox
            ComboBox comboBox = (ComboBox)sender;

            // Get the selected item's text
            string selectedLLMName = comboBox.SelectedItem.ToString();

            UpdateSelectedLLM(selectedLLMName);

        }

        private void terminateOngoingLLMCallButton_Click(object sender, EventArgs e)
        {
            try
            {
                // Cancel the ongoing HTTP request to the LLM
                client.CancelPendingRequests();

                // Update the UI to reflect the cancellation
                llmThinkingLabel.Text = "LLM Call Cancelled";
                llmThinkingProgressBar.Value = 0;

                // Hide the terminate button
                terminateOngoingLLMCallButton.Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error cancelling LLM call: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void saveCodeFileInjectPresetButton_Click(object sender, EventArgs e)
        {
            //if already selected existing preset, then overwrite that one
            if (presetSelectComboBox.SelectedIndex == 0) //first in list is always default "Select..."
            {
                // Open a dialog window to get the preset name from the user
                string presetName = GetPresetNameFromUser();

                //if not filled by user don't continue
                if (string.IsNullOrEmpty(presetName)) return;

                // Create a new FileInjectPreset object
                FileInjectPreset newPresetData = new FileInjectPreset
                {
                    Name = presetName,
                    InjectedFilesData = this.CurrentCodeFiles
                };

                // Add the preset to the current global preset list
                CurrentFileInjectPresets.Add(newPresetData);

                // Save the updated preset list to a file (optional)
                SavePresetsToFile();

                // Display a success message to the user
                MessageBox.Show("Preset saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                //update dropdown selection to show as though now the newly saved is selected
                presetSelectComboBox.SelectedItem = newPresetData;
            }
            else //overwrite existing preset
            {
                // Get the selected preset data from the presetSelectComboBox
                FileInjectPreset selectedPreset = (FileInjectPreset)presetSelectComboBox.SelectedItem;

                // Update the preset's InjectedFilesData
                selectedPreset.InjectedFilesData = this.CurrentCodeFiles;

                // Save the updated preset list to a file (optional)
                SavePresetsToFile();

                // Display a success message to the user
                MessageBox.Show("Preset updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            //make save button invisible since saved
            saveCodeFileInjectPresetButton.Visible = false;
        }

        private void codeFileInjectPathTextBox_TextChanged(object sender, EventArgs e)
        {

            //package data for storing
            // Create a new CodeFile object with the current values from the text boxes
            var updatedCodeFile = new CodeFile(
                codeFileInjectPathTextBox.Text, // File path
                int.Parse(startLineNumberTextBox.Text), // Start line number
                int.Parse(endLineNumberTextBox.Text), // End line number
                preCodePromptTextBox.Text, // Pre-prompt text
                codeFileInjectTextBox.Text, // Extracted code text
                postCodePromptTextBox.Text // Post-prompt text
            );

            //every time text is changed try to auto get the file and read for easy user UX
            try
            {
                //get path to query
                var possibleFilePath = codeFileInjectPathTextBox.Text;

                // if user has not set end line number then help user by auto-filling
                if (endLineNumberTextBox.Text == "0") { endLineNumberTextBox.Text = GetMaxLinesInFile(possibleFilePath).ToString(); }
                var startLineNum = int.Parse(startLineNumberTextBox.Text);
                var endLineNum = int.Parse(endLineNumberTextBox.Text);

                // cut out piece of select code from file
                var currentExtractedCode = ExtractOutSectionFromCodeFile(possibleFilePath, startLineNum, endLineNum);

                // put extracted code into view
                codeFileInjectTextBox.Text = currentExtractedCode;

                // auto set probable pre and post prompt for file
                var codeFileName = GetCodeFileFullName(possibleFilePath);
                preCodePromptTextBox.Text = $"Analyse and parse below {codeFileName} code";
                postCodePromptTextBox.Text = $"Ok, I've parsed the code";

                // update token stats for current file only
                var textSizeKbCurrent = GetBinarySizeOfTextInKB(currentExtractedCode);
                var textSizeToken = ConvertKBToTokenCount(textSizeKbCurrent);
                fetchCodeStatusMessageLabel.Text = $"Size : {textSizeKbCurrent:F2} KB ~ {textSizeToken} Tokens";


            }
            catch (Exception exception)
            {
                //let user know file failed to retrieve
                codeFileInjectTextBox.Text = "!! Error : File could not be read or does not exist. !!";
            }
        }

        private void presetSelectComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Get the selected preset data from the presetSelectComboBox
            var selectedPreset = (FileInjectPreset)presetSelectComboBox.SelectedItem;

            //skip if selection is empty/default
            if (selectedPreset.Name == "Select...") { return; }

            // Clear existing code file inject table panels
            selectedCodeFileViewTable.Controls.Clear();

            //clear all rows too
            selectedCodeFileViewTable.RowCount = 0;
            selectedCodeFileViewTable.RowStyles.Clear();

            //add each file in preset into the view, similar to clicking `addNewCodeFileInjectButton_Click`
            foreach (var codeFile in selectedPreset.InjectedFilesData)
            {
                AddNewFileInjectToVisibleList(codeFile);

                //add to global data list
                UpdateCurrentCodeFilesGlobalList(codeFile);

                //update global token stats (last to get data)
                UpdateGlobalTokenStats();

            }

            //update main counters of KB and Tokens
            UpdateGlobalTokenStats();

        }
    }

    public class FileInjectPreset
    {
        public string Name { get; set; }
        public List<CodeFile> InjectedFilesData { get; set; }

    }

    public record ApiConfig(string Name, string Endpoint, string ApiKey, int MaxContextWindowTokens);

    public class InputDialog : Form
    {
        private Label label;
        private TextBox textBox;
        private Button okButton;
        private Button cancelButton;

        public InputDialog(string prompt, string title)
        {
            this.Text = title;
            this.Size = new Size(300, 150);
            this.MinimumSize = new Size(300, 150);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            label = new Label();
            label.Text = prompt;
            label.AutoSize = true;
            label.Location = new Point(10, 10);
            label.Size = new Size(280, 20);
            this.Controls.Add(label);

            textBox = new TextBox();
            textBox.Location = new Point(10, 40);
            textBox.Size = new Size(280, 20);
            this.Controls.Add(textBox);

            okButton = new Button();
            okButton.Text = "OK";
            okButton.DialogResult = DialogResult.OK;
            okButton.Location = new Point(120, 70);
            this.Controls.Add(okButton);

            cancelButton = new Button();
            cancelButton.Text = "Cancel";
            cancelButton.DialogResult = DialogResult.Cancel;
            cancelButton.Location = new Point(200, 70);
            this.Controls.Add(cancelButton);
        }

        public string InputText
        {
            get { return textBox.Text; }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            textBox.Focus();
        }
    }

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

    public class CodeFile
    {
        public string FilePath { get; set; }
        public int StartLineNumber { get; set; }
        public int EndLineNumber { get; set; }
        public string PrePrompt { get; set; }
        public string ExtractedCode { get; set; }
        public string PostPrompt { get; set; }

        public CodeFile(string filePath, int startLineNumber, int endLineNumber, string prePrompt, string extractedCode, string postPrompt)
        {
            FilePath = filePath;
            StartLineNumber = startLineNumber;
            EndLineNumber = endLineNumber;
            PrePrompt = prePrompt;
            ExtractedCode = extractedCode;
            PostPrompt = postPrompt;
        }

        public CodeFile()
        {
        }
    }

    /// <summary>
    /// Special class to give Golden Ration nicely
    /// Theory: human brain symmetrical distances, where each distance is relative to the one before
    /// </summary>
    public static class GR
    {
        /// <summary>
        /// used for dynamic design layout
        /// </summary>
        public const double GoldenRatio = 1.61803;
        public const double ContentWidth = 1080; //page content 

        public static double W1080 => 1080;
        public static string W1080px => $"{W1080}px";

        public static double W824 => GR.W667 + GR.W157;
        public static string W824px => $"{W824}px";

        public static double W764 => GR.W667 + GR.W97;
        public static string W764px => $"{W764}px";

        public static double W667 => Math.Round(W1080 / GoldenRatio, 1);
        public static string W667px => $"{W667}px";

        public static double W546 => W509 + W37;
        public static string W546px => $"{W546}px";

        public static double W509 => W412 + W97;
        public static string W509px => $"{W509}px";

        public static double W412 => Math.Round(W667 / GoldenRatio, 1);
        public static string W412px => $"{W412}px";

        public static double W352 => W255 + W97;
        public static string W352px => $"{W352}px";

        public static double W291 => W157 + W134;
        public static string W291px => $"{W291}px";

        public static double W255 => Math.Round(W412 / GoldenRatio, 1);
        public static string W255px => $"{W255}px";

        public static double W231 => GR.W194 + GR.W37;
        public static string W231px => $"{W231}px";

        public static double W194 => GR.W157 + GR.W37;
        public static string W194px => $"{W194}px";

        public static double W157 => Math.Round(W255 / GoldenRatio, 1);
        public static string W157px => $"{W157}px";

        public static double W134 => GR.W97 + GR.W37;
        public static string W134px => $"{W134}px";

        public static double W97 => Math.Round(W157 / GoldenRatio, 1);
        public static string W97px => $"{W97}px";

        public static double W60 => Math.Round(W97 / GoldenRatio, 1);
        public static string W60px => $"{W60}px";

        public static double W37 => Math.Round(W60 / GoldenRatio, 1);
        public static string W37px => $"{W37}px";

        public static double W22 => Math.Round(W37 / GoldenRatio, 1);
        public static string W22px => $"{W22}px";

        public static double W14 => Math.Round(W22 / GoldenRatio, 1);
        public static string W14px => $"{W14}px";

        public static double W8 => Math.Round(W14 / GoldenRatio, 1);
        public static string W8px => $"{W8}px";

        public static double W5 => Math.Round(W8 / GoldenRatio, 1);
        public static string W5px => $"{W5}px";

        public static double W3 => Math.Round(W5 / GoldenRatio, 1);
        public static string W3px => $"{W3}px";

        public static double W2 => Math.Round(W3 / GoldenRatio, 1);
        public static string W2px => $"{W2}px";

        public static double W1 => Math.Round(W2 / GoldenRatio, 1);
        public static string W1px => $"{W1}px";




    }

}
