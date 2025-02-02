namespace LLMCoder
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Label pastPromptsLabel;
            Label modalSelectorLabel;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            codeFileInjectTablePanel = new TableLayoutPanel();
            postCodePromptTextBox = new TextBox();
            postCodePromptLabel = new Label();
            codeFileInjectTextBox = new RichTextBox();
            codeFileInjectLabel = new Label();
            preCodePromptTextBox = new TextBox();
            preCodePromptLabel = new Label();
            lineNumRangeInputTable = new TableLayoutPanel();
            fetchCodeStatusMessageLabel = new Label();
            endLineNumberTextBox = new TextBox();
            endLabel = new Label();
            startLineNumberTextBox = new TextBox();
            startLabel = new Label();
            lineNumberRangeLabel = new Label();
            codeFileInjectPathTextBox = new TextBox();
            fileInjectPathLabel = new Label();
            totalByteUsageMeterTextLabel = new Label();
            splitContainer1 = new SplitContainer();
            mainTabControl = new TabControl();
            llmPage = new TabPage();
            mainChatPageHolderTable = new TableLayoutPanel();
            chatInputHolderTable = new TableLayoutPanel();
            sendUserMsgButton = new Button();
            userInputTextBox = new RichTextBox();
            mainChatHeaderRow = new TableLayoutPanel();
            duplicateCurrentChatHistoryButton = new Button();
            resetChatHistoryButton = new Button();
            injectSourceLabel = new Label();
            llmChatHistorySelectComboBox = new ComboBox();
            llmChatHistoryLabel = new Label();
            webInjectCheckBox = new CheckBox();
            fileInjectCheckBox = new CheckBox();
            snippetInjectCheckBox = new CheckBox();
            outputChatMessagePanel = new TableLayoutPanel();
            llmButtonRowHolderTable = new TableLayoutPanel();
            terminateOngoingLLMCallButton = new Button();
            llmThinkingLabel = new Label();
            llmThinkingTimerLabel = new Label();
            llmThinkingProgressBar = new ProgressBar();
            clearUserMsgButton = new Button();
            copyFinalPromptButton = new Button();
            codePage = new TabPage();
            codeSnippetInjectMainTablePanel = new TableLayoutPanel();
            label2 = new Label();
            codeInjectPretextTextBox = new TextBox();
            flowLayoutPanel1 = new FlowLayoutPanel();
            codeInjectLabel = new Label();
            tokenCountLabel = new Label();
            largeCodeSnippetTextBox = new RichTextBox();
            label4 = new Label();
            injectAssitantPretextTextBox = new TextBox();
            codeFilePage = new TabPage();
            codeFileInjectTabMainTablePanel = new TableLayoutPanel();
            newFileInjectHolderTable = new TableLayoutPanel();
            addNewCodeFileInjectButton = new Button();
            selectedCodeFileViewTable = new TableLayoutPanel();
            codeFileInjectHeaderTable = new TableLayoutPanel();
            presetLabel = new Label();
            presetSelectComboBox = new ComboBox();
            saveCodeFileInjectPresetButton = new Button();
            settingsPage = new TabPage();
            tableLayoutPanel4 = new TableLayoutPanel();
            topPTextBox = new TextBox();
            topPLabel = new Label();
            temperatureTextBox = new TextBox();
            label3 = new Label();
            nerdStatsPage = new TabPage();
            sidePanelTable = new TableLayoutPanel();
            llmSelectorTable = new TableLayoutPanel();
            llmSelector = new ComboBox();
            totalTokenUsageTable = new TableLayoutPanel();
            finalChatTokenLimitLabel = new Label();
            finalChatTokenUsageProgressBar = new CustomProgressBar();
            pastUserPrompts = new ListBox();
            tabPage3 = new TabPage();
            tabPage4 = new TabPage();
            pastPromptsLabel = new Label();
            modalSelectorLabel = new Label();
            codeFileInjectTablePanel.SuspendLayout();
            lineNumRangeInputTable.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            mainTabControl.SuspendLayout();
            llmPage.SuspendLayout();
            mainChatPageHolderTable.SuspendLayout();
            chatInputHolderTable.SuspendLayout();
            mainChatHeaderRow.SuspendLayout();
            llmButtonRowHolderTable.SuspendLayout();
            codePage.SuspendLayout();
            codeSnippetInjectMainTablePanel.SuspendLayout();
            flowLayoutPanel1.SuspendLayout();
            codeFilePage.SuspendLayout();
            codeFileInjectTabMainTablePanel.SuspendLayout();
            newFileInjectHolderTable.SuspendLayout();
            codeFileInjectHeaderTable.SuspendLayout();
            settingsPage.SuspendLayout();
            tableLayoutPanel4.SuspendLayout();
            sidePanelTable.SuspendLayout();
            llmSelectorTable.SuspendLayout();
            totalTokenUsageTable.SuspendLayout();
            SuspendLayout();
            // 
            // pastPromptsLabel
            // 
            pastPromptsLabel.AutoSize = true;
            pastPromptsLabel.Dock = DockStyle.Fill;
            pastPromptsLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            pastPromptsLabel.ForeColor = Color.White;
            pastPromptsLabel.Location = new Point(3, 124);
            pastPromptsLabel.Name = "pastPromptsLabel";
            pastPromptsLabel.Size = new Size(242, 20);
            pastPromptsLabel.TabIndex = 2;
            pastPromptsLabel.Text = "📜 Past Prompts";
            // 
            // modalSelectorLabel
            // 
            modalSelectorLabel.Dock = DockStyle.Fill;
            modalSelectorLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            modalSelectorLabel.ForeColor = Color.White;
            modalSelectorLabel.Location = new Point(3, 9);
            modalSelectorLabel.Margin = new Padding(3, 9, 3, 0);
            modalSelectorLabel.Name = "modalSelectorLabel";
            modalSelectorLabel.Size = new Size(85, 36);
            modalSelectorLabel.TabIndex = 3;
            modalSelectorLabel.Text = "✨ Modal";
            // 
            // codeFileInjectTablePanel
            // 
            codeFileInjectTablePanel.AutoSize = true;
            codeFileInjectTablePanel.BackColor = Color.Black;
            codeFileInjectTablePanel.ColumnCount = 3;
            codeFileInjectTablePanel.ColumnStyles.Add(new ColumnStyle());
            codeFileInjectTablePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            codeFileInjectTablePanel.ColumnStyles.Add(new ColumnStyle());
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
            codeFileInjectTablePanel.Dock = DockStyle.Fill;
            codeFileInjectTablePanel.Location = new Point(3, 4);
            codeFileInjectTablePanel.Margin = new Padding(3, 4, 3, 4);
            codeFileInjectTablePanel.Name = "codeFileInjectTablePanel";
            codeFileInjectTablePanel.RowCount = 6;
            codeFileInjectTablePanel.RowStyles.Add(new RowStyle());
            codeFileInjectTablePanel.RowStyles.Add(new RowStyle());
            codeFileInjectTablePanel.RowStyles.Add(new RowStyle());
            codeFileInjectTablePanel.RowStyles.Add(new RowStyle());
            codeFileInjectTablePanel.RowStyles.Add(new RowStyle());
            codeFileInjectTablePanel.RowStyles.Add(new RowStyle());
            codeFileInjectTablePanel.Size = new Size(831, 320);
            codeFileInjectTablePanel.TabIndex = 1;
            // 
            // postCodePromptTextBox
            // 
            postCodePromptTextBox.Dock = DockStyle.Fill;
            postCodePromptTextBox.Location = new Point(104, 246);
            postCodePromptTextBox.Margin = new Padding(3, 4, 3, 4);
            postCodePromptTextBox.Name = "postCodePromptTextBox";
            postCodePromptTextBox.Size = new Size(724, 27);
            postCodePromptTextBox.TabIndex = 12;
            // 
            // postCodePromptLabel
            // 
            postCodePromptLabel.AutoSize = true;
            postCodePromptLabel.ForeColor = Color.Azure;
            postCodePromptLabel.Location = new Point(3, 251);
            postCodePromptLabel.Margin = new Padding(3, 9, 3, 0);
            postCodePromptLabel.Name = "postCodePromptLabel";
            postCodePromptLabel.Size = new Size(89, 20);
            postCodePromptLabel.TabIndex = 11;
            postCodePromptLabel.Text = "Post Prompt";
            // 
            // codeFileInjectTextBox
            // 
            codeFileInjectTextBox.BackColor = SystemColors.ActiveCaptionText;
            codeFileInjectTextBox.Dock = DockStyle.Fill;
            codeFileInjectTextBox.Font = new Font("Cascadia Code", 8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            codeFileInjectTextBox.ForeColor = Color.LightGreen;
            codeFileInjectTextBox.Location = new Point(104, 111);
            codeFileInjectTextBox.Margin = new Padding(3, 4, 3, 4);
            codeFileInjectTextBox.MinimumSize = new Size(114, 36);
            codeFileInjectTextBox.Name = "codeFileInjectTextBox";
            codeFileInjectTextBox.Size = new Size(724, 127);
            codeFileInjectTextBox.TabIndex = 13;
            codeFileInjectTextBox.Text = "";
            // 
            // codeFileInjectLabel
            // 
            codeFileInjectLabel.AutoSize = true;
            codeFileInjectLabel.ForeColor = Color.Azure;
            codeFileInjectLabel.Location = new Point(3, 116);
            codeFileInjectLabel.Margin = new Padding(3, 9, 3, 0);
            codeFileInjectLabel.Name = "codeFileInjectLabel";
            codeFileInjectLabel.Size = new Size(71, 20);
            codeFileInjectLabel.TabIndex = 9;
            codeFileInjectLabel.Text = "Code File";
            // 
            // preCodePromptTextBox
            // 
            preCodePromptTextBox.Dock = DockStyle.Fill;
            preCodePromptTextBox.Location = new Point(104, 76);
            preCodePromptTextBox.Margin = new Padding(3, 4, 3, 4);
            preCodePromptTextBox.Name = "preCodePromptTextBox";
            preCodePromptTextBox.Size = new Size(724, 27);
            preCodePromptTextBox.TabIndex = 8;
            // 
            // preCodePromptLabel
            // 
            preCodePromptLabel.AutoSize = true;
            preCodePromptLabel.ForeColor = Color.Azure;
            preCodePromptLabel.Location = new Point(3, 81);
            preCodePromptLabel.Margin = new Padding(3, 9, 3, 0);
            preCodePromptLabel.Name = "preCodePromptLabel";
            preCodePromptLabel.Size = new Size(83, 20);
            preCodePromptLabel.TabIndex = 7;
            preCodePromptLabel.Text = "Pre Prompt";
            // 
            // lineNumRangeInputTable
            // 
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
            lineNumRangeInputTable.Location = new Point(104, 39);
            lineNumRangeInputTable.Margin = new Padding(3, 4, 3, 4);
            lineNumRangeInputTable.Name = "lineNumRangeInputTable";
            lineNumRangeInputTable.RowCount = 1;
            lineNumRangeInputTable.RowStyles.Add(new RowStyle());
            lineNumRangeInputTable.Size = new Size(724, 29);
            lineNumRangeInputTable.TabIndex = 6;
            // 
            // fetchCodeStatusMessageLabel
            // 
            fetchCodeStatusMessageLabel.AutoSize = true;
            fetchCodeStatusMessageLabel.ForeColor = Color.Azure;
            fetchCodeStatusMessageLabel.Location = new Point(305, 9);
            fetchCodeStatusMessageLabel.Margin = new Padding(3, 9, 3, 0);
            fetchCodeStatusMessageLabel.Name = "fetchCodeStatusMessageLabel";
            fetchCodeStatusMessageLabel.Size = new Size(130, 20);
            fetchCodeStatusMessageLabel.TabIndex = 12;
            fetchCodeStatusMessageLabel.Text = "Ready Captain \U0001fae1";
            // 
            // endLineNumberTextBox
            // 
            endLineNumberTextBox.Dock = DockStyle.Fill;
            endLineNumberTextBox.Location = new Point(197, 4);
            endLineNumberTextBox.Margin = new Padding(3, 4, 3, 4);
            endLineNumberTextBox.MaximumSize = new Size(102, 4);
            endLineNumberTextBox.Name = "endLineNumberTextBox";
            endLineNumberTextBox.Size = new Size(102, 4);
            endLineNumberTextBox.TabIndex = 8;
            endLineNumberTextBox.Text = "0";
            // 
            // endLabel
            // 
            endLabel.AutoSize = true;
            endLabel.ForeColor = Color.Azure;
            endLabel.Location = new Point(157, 9);
            endLabel.Margin = new Padding(3, 9, 3, 0);
            endLabel.Name = "endLabel";
            endLabel.Size = new Size(34, 20);
            endLabel.TabIndex = 7;
            endLabel.Text = "End";
            // 
            // startLineNumberTextBox
            // 
            startLineNumberTextBox.Dock = DockStyle.Fill;
            startLineNumberTextBox.Location = new Point(49, 4);
            startLineNumberTextBox.Margin = new Padding(3, 4, 3, 4);
            startLineNumberTextBox.MaximumSize = new Size(102, 4);
            startLineNumberTextBox.Name = "startLineNumberTextBox";
            startLineNumberTextBox.Size = new Size(102, 4);
            startLineNumberTextBox.TabIndex = 6;
            startLineNumberTextBox.Text = "1";
            // 
            // startLabel
            // 
            startLabel.AutoSize = true;
            startLabel.ForeColor = Color.Azure;
            startLabel.Location = new Point(3, 9);
            startLabel.Margin = new Padding(3, 9, 3, 0);
            startLabel.Name = "startLabel";
            startLabel.Size = new Size(40, 20);
            startLabel.TabIndex = 5;
            startLabel.Text = "Start";
            // 
            // lineNumberRangeLabel
            // 
            lineNumberRangeLabel.AutoSize = true;
            lineNumberRangeLabel.ForeColor = Color.Azure;
            lineNumberRangeLabel.Location = new Point(3, 44);
            lineNumberRangeLabel.Margin = new Padding(3, 9, 3, 0);
            lineNumberRangeLabel.Name = "lineNumberRangeLabel";
            lineNumberRangeLabel.Size = new Size(95, 20);
            lineNumberRangeLabel.TabIndex = 4;
            lineNumberRangeLabel.Text = "Select Range";
            // 
            // codeFileInjectPathTextBox
            // 
            codeFileInjectPathTextBox.Dock = DockStyle.Fill;
            codeFileInjectPathTextBox.Location = new Point(104, 4);
            codeFileInjectPathTextBox.Margin = new Padding(3, 4, 3, 4);
            codeFileInjectPathTextBox.Name = "codeFileInjectPathTextBox";
            codeFileInjectPathTextBox.Size = new Size(724, 27);
            codeFileInjectPathTextBox.TabIndex = 3;
            codeFileInjectPathTextBox.TextChanged += codeFileInjectPathTextBox_TextChanged;
            // 
            // fileInjectPathLabel
            // 
            fileInjectPathLabel.AutoSize = true;
            fileInjectPathLabel.ForeColor = Color.Azure;
            fileInjectPathLabel.Location = new Point(3, 9);
            fileInjectPathLabel.Margin = new Padding(3, 9, 3, 0);
            fileInjectPathLabel.Name = "fileInjectPathLabel";
            fileInjectPathLabel.Size = new Size(64, 20);
            fileInjectPathLabel.TabIndex = 1;
            fileInjectPathLabel.Text = "File Path";
            // 
            // totalByteUsageMeterTextLabel
            // 
            totalByteUsageMeterTextLabel.AutoSize = true;
            totalByteUsageMeterTextLabel.Dock = DockStyle.Fill;
            totalByteUsageMeterTextLabel.ForeColor = Color.Coral;
            totalByteUsageMeterTextLabel.ImageAlign = ContentAlignment.MiddleRight;
            totalByteUsageMeterTextLabel.Location = new Point(3, 104);
            totalByteUsageMeterTextLabel.Name = "totalByteUsageMeterTextLabel";
            totalByteUsageMeterTextLabel.Size = new Size(242, 20);
            totalByteUsageMeterTextLabel.TabIndex = 9;
            totalByteUsageMeterTextLabel.Text = "0 / 256 KB";
            totalByteUsageMeterTextLabel.TextAlign = ContentAlignment.TopRight;
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 0);
            splitContainer1.Margin = new Padding(3, 4, 3, 4);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(mainTabControl);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(sidePanelTable);
            splitContainer1.Size = new Size(1234, 921);
            splitContainer1.SplitterDistance = 981;
            splitContainer1.SplitterWidth = 5;
            splitContainer1.TabIndex = 1;
            // 
            // mainTabControl
            // 
            mainTabControl.Controls.Add(llmPage);
            mainTabControl.Controls.Add(codePage);
            mainTabControl.Controls.Add(codeFilePage);
            mainTabControl.Controls.Add(settingsPage);
            mainTabControl.Controls.Add(nerdStatsPage);
            mainTabControl.Dock = DockStyle.Fill;
            mainTabControl.Location = new Point(0, 0);
            mainTabControl.Margin = new Padding(3, 4, 3, 4);
            mainTabControl.Name = "mainTabControl";
            mainTabControl.SelectedIndex = 0;
            mainTabControl.Size = new Size(981, 921);
            mainTabControl.TabIndex = 4;
            // 
            // llmPage
            // 
            llmPage.BackColor = Color.FromArgb(39, 42, 49);
            llmPage.Controls.Add(mainChatPageHolderTable);
            llmPage.ForeColor = Color.FromArgb(28, 30, 35);
            llmPage.Location = new Point(4, 29);
            llmPage.Margin = new Padding(3, 4, 3, 4);
            llmPage.Name = "llmPage";
            llmPage.Padding = new Padding(3, 4, 3, 4);
            llmPage.Size = new Size(973, 888);
            llmPage.TabIndex = 0;
            llmPage.Text = "🗨️ Chat";
            // 
            // mainChatPageHolderTable
            // 
            mainChatPageHolderTable.ColumnCount = 1;
            mainChatPageHolderTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            mainChatPageHolderTable.Controls.Add(chatInputHolderTable, 0, 2);
            mainChatPageHolderTable.Controls.Add(mainChatHeaderRow, 0, 0);
            mainChatPageHolderTable.Controls.Add(outputChatMessagePanel, 0, 1);
            mainChatPageHolderTable.Controls.Add(llmButtonRowHolderTable, 0, 3);
            mainChatPageHolderTable.Dock = DockStyle.Fill;
            mainChatPageHolderTable.Location = new Point(3, 4);
            mainChatPageHolderTable.Margin = new Padding(3, 4, 3, 4);
            mainChatPageHolderTable.Name = "mainChatPageHolderTable";
            mainChatPageHolderTable.RowCount = 4;
            mainChatPageHolderTable.RowStyles.Add(new RowStyle());
            mainChatPageHolderTable.RowStyles.Add(new RowStyle(SizeType.Percent, 80F));
            mainChatPageHolderTable.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            mainChatPageHolderTable.RowStyles.Add(new RowStyle());
            mainChatPageHolderTable.RowStyles.Add(new RowStyle());
            mainChatPageHolderTable.Size = new Size(967, 880);
            mainChatPageHolderTable.TabIndex = 4;
            // 
            // chatInputHolderTable
            // 
            chatInputHolderTable.AutoSize = true;
            chatInputHolderTable.ColumnCount = 2;
            chatInputHolderTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            chatInputHolderTable.ColumnStyles.Add(new ColumnStyle());
            chatInputHolderTable.Controls.Add(sendUserMsgButton, 1, 0);
            chatInputHolderTable.Controls.Add(userInputTextBox, 0, 0);
            chatInputHolderTable.Dock = DockStyle.Fill;
            chatInputHolderTable.Location = new Point(3, 676);
            chatInputHolderTable.Margin = new Padding(3, 4, 3, 4);
            chatInputHolderTable.Name = "chatInputHolderTable";
            chatInputHolderTable.RowCount = 1;
            chatInputHolderTable.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            chatInputHolderTable.Size = new Size(961, 148);
            chatInputHolderTable.TabIndex = 11;
            // 
            // sendUserMsgButton
            // 
            sendUserMsgButton.AutoSize = true;
            sendUserMsgButton.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            sendUserMsgButton.BackColor = SystemColors.Highlight;
            sendUserMsgButton.Cursor = Cursors.Hand;
            sendUserMsgButton.Dock = DockStyle.Fill;
            sendUserMsgButton.FlatStyle = FlatStyle.Flat;
            sendUserMsgButton.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            sendUserMsgButton.ForeColor = SystemColors.ButtonFace;
            sendUserMsgButton.Location = new Point(877, 4);
            sendUserMsgButton.Margin = new Padding(3, 4, 3, 4);
            sendUserMsgButton.Name = "sendUserMsgButton";
            sendUserMsgButton.Size = new Size(81, 140);
            sendUserMsgButton.TabIndex = 1;
            sendUserMsgButton.Text = "Send \U0001f98b";
            sendUserMsgButton.UseVisualStyleBackColor = false;
            sendUserMsgButton.Click += sendUserMsgButton_Click;
            // 
            // userInputTextBox
            // 
            userInputTextBox.BackColor = Color.FromArgb(28, 30, 35);
            userInputTextBox.Dock = DockStyle.Fill;
            userInputTextBox.Font = new Font("Cascadia Code", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            userInputTextBox.ForeColor = Color.Aqua;
            userInputTextBox.Location = new Point(3, 4);
            userInputTextBox.Margin = new Padding(3, 4, 3, 4);
            userInputTextBox.Name = "userInputTextBox";
            userInputTextBox.Size = new Size(868, 140);
            userInputTextBox.TabIndex = 0;
            userInputTextBox.Text = "";
            // 
            // mainChatHeaderRow
            // 
            mainChatHeaderRow.AutoSize = true;
            mainChatHeaderRow.ColumnCount = 11;
            mainChatHeaderRow.ColumnStyles.Add(new ColumnStyle());
            mainChatHeaderRow.ColumnStyles.Add(new ColumnStyle());
            mainChatHeaderRow.ColumnStyles.Add(new ColumnStyle());
            mainChatHeaderRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            mainChatHeaderRow.ColumnStyles.Add(new ColumnStyle());
            mainChatHeaderRow.ColumnStyles.Add(new ColumnStyle());
            mainChatHeaderRow.ColumnStyles.Add(new ColumnStyle());
            mainChatHeaderRow.ColumnStyles.Add(new ColumnStyle());
            mainChatHeaderRow.ColumnStyles.Add(new ColumnStyle());
            mainChatHeaderRow.ColumnStyles.Add(new ColumnStyle());
            mainChatHeaderRow.ColumnStyles.Add(new ColumnStyle());
            mainChatHeaderRow.Controls.Add(duplicateCurrentChatHistoryButton, 3, 0);
            mainChatHeaderRow.Controls.Add(resetChatHistoryButton, 2, 0);
            mainChatHeaderRow.Controls.Add(injectSourceLabel, 4, 0);
            mainChatHeaderRow.Controls.Add(llmChatHistorySelectComboBox, 1, 0);
            mainChatHeaderRow.Controls.Add(llmChatHistoryLabel, 0, 0);
            mainChatHeaderRow.Controls.Add(webInjectCheckBox, 10, 0);
            mainChatHeaderRow.Controls.Add(fileInjectCheckBox, 9, 0);
            mainChatHeaderRow.Controls.Add(snippetInjectCheckBox, 8, 0);
            mainChatHeaderRow.Dock = DockStyle.Fill;
            mainChatHeaderRow.Location = new Point(3, 4);
            mainChatHeaderRow.Margin = new Padding(3, 4, 3, 4);
            mainChatHeaderRow.Name = "mainChatHeaderRow";
            mainChatHeaderRow.RowCount = 1;
            mainChatHeaderRow.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            mainChatHeaderRow.Size = new Size(961, 39);
            mainChatHeaderRow.TabIndex = 9;
            // 
            // duplicateCurrentChatHistoryButton
            // 
            duplicateCurrentChatHistoryButton.AutoSize = true;
            duplicateCurrentChatHistoryButton.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            duplicateCurrentChatHistoryButton.BackColor = Color.YellowGreen;
            duplicateCurrentChatHistoryButton.Cursor = Cursors.Hand;
            duplicateCurrentChatHistoryButton.FlatStyle = FlatStyle.Flat;
            duplicateCurrentChatHistoryButton.Font = new Font("Segoe UI", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            duplicateCurrentChatHistoryButton.ForeColor = SystemColors.ButtonFace;
            duplicateCurrentChatHistoryButton.Location = new Point(431, 3);
            duplicateCurrentChatHistoryButton.Margin = new Padding(6, 3, 0, 0);
            duplicateCurrentChatHistoryButton.Name = "duplicateCurrentChatHistoryButton";
            duplicateCurrentChatHistoryButton.Size = new Size(108, 31);
            duplicateCurrentChatHistoryButton.TabIndex = 14;
            duplicateCurrentChatHistoryButton.Text = "Duplicate ❇️";
            duplicateCurrentChatHistoryButton.UseVisualStyleBackColor = false;
            // 
            // resetChatHistoryButton
            // 
            resetChatHistoryButton.AutoSize = true;
            resetChatHistoryButton.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            resetChatHistoryButton.BackColor = Color.Tomato;
            resetChatHistoryButton.Cursor = Cursors.Hand;
            resetChatHistoryButton.FlatStyle = FlatStyle.Flat;
            resetChatHistoryButton.Font = new Font("Segoe UI", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            resetChatHistoryButton.ForeColor = SystemColors.ButtonFace;
            resetChatHistoryButton.Location = new Point(344, 3);
            resetChatHistoryButton.Margin = new Padding(0, 3, 0, 0);
            resetChatHistoryButton.Name = "resetChatHistoryButton";
            resetChatHistoryButton.Size = new Size(81, 31);
            resetChatHistoryButton.TabIndex = 4;
            resetChatHistoryButton.Text = "Reset ⚡";
            resetChatHistoryButton.UseVisualStyleBackColor = false;
            resetChatHistoryButton.Click += resetChatHistoryButton_Click;
            // 
            // injectSourceLabel
            // 
            injectSourceLabel.AutoSize = true;
            injectSourceLabel.Dock = DockStyle.Fill;
            injectSourceLabel.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            injectSourceLabel.ForeColor = Color.White;
            injectSourceLabel.Location = new Point(659, 4);
            injectSourceLabel.Margin = new Padding(3, 4, 3, 4);
            injectSourceLabel.Name = "injectSourceLabel";
            injectSourceLabel.Padding = new Padding(3, 4, 3, 4);
            injectSourceLabel.Size = new Size(84, 31);
            injectSourceLabel.TabIndex = 9;
            injectSourceLabel.Text = "💉Inject";
            // 
            // llmChatHistorySelectComboBox
            // 
            llmChatHistorySelectComboBox.Dock = DockStyle.Fill;
            llmChatHistorySelectComboBox.FormattingEnabled = true;
            llmChatHistorySelectComboBox.Location = new Point(113, 4);
            llmChatHistorySelectComboBox.Margin = new Padding(3, 4, 3, 4);
            llmChatHistorySelectComboBox.Name = "llmChatHistorySelectComboBox";
            llmChatHistorySelectComboBox.Size = new Size(228, 28);
            llmChatHistorySelectComboBox.TabIndex = 8;
            // 
            // llmChatHistoryLabel
            // 
            llmChatHistoryLabel.AutoSize = true;
            llmChatHistoryLabel.Dock = DockStyle.Fill;
            llmChatHistoryLabel.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            llmChatHistoryLabel.ForeColor = Color.White;
            llmChatHistoryLabel.Location = new Point(3, 4);
            llmChatHistoryLabel.Margin = new Padding(3, 4, 3, 4);
            llmChatHistoryLabel.Name = "llmChatHistoryLabel";
            llmChatHistoryLabel.Padding = new Padding(3, 4, 3, 4);
            llmChatHistoryLabel.Size = new Size(104, 31);
            llmChatHistoryLabel.TabIndex = 6;
            llmChatHistoryLabel.Text = "📒 History";
            // 
            // webInjectCheckBox
            // 
            webInjectCheckBox.AutoSize = true;
            webInjectCheckBox.ForeColor = Color.White;
            webInjectCheckBox.Location = new Point(897, 8);
            webInjectCheckBox.Margin = new Padding(3, 8, 3, 4);
            webInjectCheckBox.Name = "webInjectCheckBox";
            webInjectCheckBox.Size = new Size(61, 24);
            webInjectCheckBox.TabIndex = 13;
            webInjectCheckBox.Text = "Web";
            webInjectCheckBox.UseVisualStyleBackColor = true;
            // 
            // fileInjectCheckBox
            // 
            fileInjectCheckBox.AutoSize = true;
            fileInjectCheckBox.ForeColor = Color.White;
            fileInjectCheckBox.Location = new Point(837, 8);
            fileInjectCheckBox.Margin = new Padding(3, 8, 3, 4);
            fileInjectCheckBox.Name = "fileInjectCheckBox";
            fileInjectCheckBox.Size = new Size(54, 24);
            fileInjectCheckBox.TabIndex = 12;
            fileInjectCheckBox.Text = "File";
            fileInjectCheckBox.UseVisualStyleBackColor = true;
            // 
            // snippetInjectCheckBox
            // 
            snippetInjectCheckBox.AutoSize = true;
            snippetInjectCheckBox.ForeColor = Color.White;
            snippetInjectCheckBox.Location = new Point(749, 8);
            snippetInjectCheckBox.Margin = new Padding(3, 8, 3, 4);
            snippetInjectCheckBox.Name = "snippetInjectCheckBox";
            snippetInjectCheckBox.Size = new Size(82, 24);
            snippetInjectCheckBox.TabIndex = 11;
            snippetInjectCheckBox.Text = "Snippet";
            snippetInjectCheckBox.UseVisualStyleBackColor = true;
            // 
            // outputChatMessagePanel
            // 
            outputChatMessagePanel.AutoScroll = true;
            outputChatMessagePanel.AutoSize = true;
            outputChatMessagePanel.BackColor = Color.Black;
            outputChatMessagePanel.ColumnCount = 1;
            outputChatMessagePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            outputChatMessagePanel.Dock = DockStyle.Fill;
            outputChatMessagePanel.Location = new Point(3, 51);
            outputChatMessagePanel.Margin = new Padding(3, 4, 3, 4);
            outputChatMessagePanel.Name = "outputChatMessagePanel";
            outputChatMessagePanel.RowCount = 1;
            outputChatMessagePanel.RowStyles.Add(new RowStyle());
            outputChatMessagePanel.Size = new Size(961, 617);
            outputChatMessagePanel.TabIndex = 7;
            // 
            // llmButtonRowHolderTable
            // 
            llmButtonRowHolderTable.AutoSize = true;
            llmButtonRowHolderTable.ColumnCount = 7;
            llmButtonRowHolderTable.ColumnStyles.Add(new ColumnStyle());
            llmButtonRowHolderTable.ColumnStyles.Add(new ColumnStyle());
            llmButtonRowHolderTable.ColumnStyles.Add(new ColumnStyle());
            llmButtonRowHolderTable.ColumnStyles.Add(new ColumnStyle());
            llmButtonRowHolderTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            llmButtonRowHolderTable.ColumnStyles.Add(new ColumnStyle());
            llmButtonRowHolderTable.ColumnStyles.Add(new ColumnStyle());
            llmButtonRowHolderTable.Controls.Add(terminateOngoingLLMCallButton, 2, 0);
            llmButtonRowHolderTable.Controls.Add(llmThinkingLabel, 3, 0);
            llmButtonRowHolderTable.Controls.Add(llmThinkingTimerLabel, 0, 0);
            llmButtonRowHolderTable.Controls.Add(llmThinkingProgressBar, 1, 0);
            llmButtonRowHolderTable.Controls.Add(clearUserMsgButton, 6, 0);
            llmButtonRowHolderTable.Controls.Add(copyFinalPromptButton, 5, 0);
            llmButtonRowHolderTable.Dock = DockStyle.Bottom;
            llmButtonRowHolderTable.Location = new Point(3, 833);
            llmButtonRowHolderTable.Margin = new Padding(3, 4, 3, 4);
            llmButtonRowHolderTable.Name = "llmButtonRowHolderTable";
            llmButtonRowHolderTable.RowCount = 1;
            llmButtonRowHolderTable.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            llmButtonRowHolderTable.Size = new Size(961, 43);
            llmButtonRowHolderTable.TabIndex = 5;
            // 
            // terminateOngoingLLMCallButton
            // 
            terminateOngoingLLMCallButton.AutoSize = true;
            terminateOngoingLLMCallButton.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            terminateOngoingLLMCallButton.BackColor = Color.Red;
            terminateOngoingLLMCallButton.Cursor = Cursors.Hand;
            terminateOngoingLLMCallButton.FlatStyle = FlatStyle.Flat;
            terminateOngoingLLMCallButton.Font = new Font("Segoe UI", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            terminateOngoingLLMCallButton.ForeColor = SystemColors.ButtonFace;
            terminateOngoingLLMCallButton.Location = new Point(153, 3);
            terminateOngoingLLMCallButton.Margin = new Padding(0, 3, 0, 0);
            terminateOngoingLLMCallButton.Name = "terminateOngoingLLMCallButton";
            terminateOngoingLLMCallButton.Size = new Size(89, 31);
            terminateOngoingLLMCallButton.TabIndex = 10;
            terminateOngoingLLMCallButton.Text = "✂️ Cancel";
            terminateOngoingLLMCallButton.UseVisualStyleBackColor = false;
            terminateOngoingLLMCallButton.Visible = false;
            terminateOngoingLLMCallButton.Click += terminateOngoingLLMCallButton_Click;
            // 
            // llmThinkingLabel
            // 
            llmThinkingLabel.AutoSize = true;
            llmThinkingLabel.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            llmThinkingLabel.ForeColor = Color.Yellow;
            llmThinkingLabel.Location = new Point(242, 9);
            llmThinkingLabel.Margin = new Padding(0, 9, 0, 0);
            llmThinkingLabel.Name = "llmThinkingLabel";
            llmThinkingLabel.Size = new Size(139, 20);
            llmThinkingLabel.TabIndex = 9;
            llmThinkingLabel.Text = "💖 LLM Thinking...";
            llmThinkingLabel.Visible = false;
            // 
            // llmThinkingTimerLabel
            // 
            llmThinkingTimerLabel.AutoSize = true;
            llmThinkingTimerLabel.Font = new Font("Segoe UI Black", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            llmThinkingTimerLabel.ForeColor = Color.LawnGreen;
            llmThinkingTimerLabel.Location = new Point(0, 8);
            llmThinkingTimerLabel.Margin = new Padding(0, 8, 0, 0);
            llmThinkingTimerLabel.Name = "llmThinkingTimerLabel";
            llmThinkingTimerLabel.Size = new Size(33, 23);
            llmThinkingTimerLabel.TabIndex = 7;
            llmThinkingTimerLabel.Text = "0 s";
            // 
            // llmThinkingProgressBar
            // 
            llmThinkingProgressBar.Location = new Point(36, 4);
            llmThinkingProgressBar.Margin = new Padding(3, 4, 3, 4);
            llmThinkingProgressBar.MarqueeAnimationSpeed = 2000;
            llmThinkingProgressBar.Name = "llmThinkingProgressBar";
            llmThinkingProgressBar.Size = new Size(114, 31);
            llmThinkingProgressBar.Style = ProgressBarStyle.Continuous;
            llmThinkingProgressBar.TabIndex = 5;
            // 
            // clearUserMsgButton
            // 
            clearUserMsgButton.AutoSize = true;
            clearUserMsgButton.BackColor = Color.Tomato;
            clearUserMsgButton.Cursor = Cursors.Hand;
            clearUserMsgButton.FlatStyle = FlatStyle.Flat;
            clearUserMsgButton.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            clearUserMsgButton.ForeColor = SystemColors.ButtonFace;
            clearUserMsgButton.Location = new Point(864, 0);
            clearUserMsgButton.Margin = new Padding(0, 0, 3, 0);
            clearUserMsgButton.Name = "clearUserMsgButton";
            clearUserMsgButton.Size = new Size(94, 43);
            clearUserMsgButton.TabIndex = 3;
            clearUserMsgButton.Text = "Clear 🗑️";
            clearUserMsgButton.UseVisualStyleBackColor = false;
            clearUserMsgButton.Click += clearUserMsgButton_Click;
            // 
            // copyFinalPromptButton
            // 
            copyFinalPromptButton.AutoSize = true;
            copyFinalPromptButton.BackColor = Color.MediumSeaGreen;
            copyFinalPromptButton.Cursor = Cursors.Hand;
            copyFinalPromptButton.FlatStyle = FlatStyle.Flat;
            copyFinalPromptButton.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            copyFinalPromptButton.ForeColor = SystemColors.ButtonFace;
            copyFinalPromptButton.Location = new Point(722, 0);
            copyFinalPromptButton.Margin = new Padding(0, 0, 3, 0);
            copyFinalPromptButton.Name = "copyFinalPromptButton";
            copyFinalPromptButton.Size = new Size(139, 43);
            copyFinalPromptButton.TabIndex = 11;
            copyFinalPromptButton.Text = "Copy Prompt \U0001f9fe";
            copyFinalPromptButton.UseVisualStyleBackColor = false;
            copyFinalPromptButton.Click += copyFinalPromptButton_Click;
            // 
            // codePage
            // 
            codePage.Controls.Add(codeSnippetInjectMainTablePanel);
            codePage.Location = new Point(4, 29);
            codePage.Margin = new Padding(3, 4, 3, 4);
            codePage.Name = "codePage";
            codePage.Padding = new Padding(3, 4, 3, 4);
            codePage.Size = new Size(973, 888);
            codePage.TabIndex = 3;
            codePage.Text = "🎞️ Snippet";
            codePage.UseVisualStyleBackColor = true;
            // 
            // codeSnippetInjectMainTablePanel
            // 
            codeSnippetInjectMainTablePanel.AutoSize = true;
            codeSnippetInjectMainTablePanel.ColumnCount = 2;
            codeSnippetInjectMainTablePanel.ColumnStyles.Add(new ColumnStyle());
            codeSnippetInjectMainTablePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            codeSnippetInjectMainTablePanel.Controls.Add(label2, 0, 0);
            codeSnippetInjectMainTablePanel.Controls.Add(codeInjectPretextTextBox, 1, 0);
            codeSnippetInjectMainTablePanel.Controls.Add(flowLayoutPanel1, 0, 1);
            codeSnippetInjectMainTablePanel.Controls.Add(largeCodeSnippetTextBox, 1, 1);
            codeSnippetInjectMainTablePanel.Controls.Add(label4, 0, 2);
            codeSnippetInjectMainTablePanel.Controls.Add(injectAssitantPretextTextBox, 1, 2);
            codeSnippetInjectMainTablePanel.Dock = DockStyle.Fill;
            codeSnippetInjectMainTablePanel.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;
            codeSnippetInjectMainTablePanel.Location = new Point(3, 4);
            codeSnippetInjectMainTablePanel.Margin = new Padding(3, 4, 3, 4);
            codeSnippetInjectMainTablePanel.Name = "codeSnippetInjectMainTablePanel";
            codeSnippetInjectMainTablePanel.RowCount = 3;
            codeSnippetInjectMainTablePanel.RowStyles.Add(new RowStyle());
            codeSnippetInjectMainTablePanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            codeSnippetInjectMainTablePanel.RowStyles.Add(new RowStyle());
            codeSnippetInjectMainTablePanel.Size = new Size(967, 880);
            codeSnippetInjectMainTablePanel.TabIndex = 11;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Dock = DockStyle.Left;
            label2.Location = new Point(3, 4);
            label2.Margin = new Padding(3, 4, 3, 4);
            label2.Name = "label2";
            label2.Padding = new Padding(3, 4, 3, 4);
            label2.Size = new Size(61, 28);
            label2.TabIndex = 5;
            label2.Text = "Pretext";
            // 
            // codeInjectPretextTextBox
            // 
            codeInjectPretextTextBox.Dock = DockStyle.Fill;
            codeInjectPretextTextBox.Location = new Point(124, 4);
            codeInjectPretextTextBox.Margin = new Padding(3, 4, 3, 4);
            codeInjectPretextTextBox.Name = "codeInjectPretextTextBox";
            codeInjectPretextTextBox.Size = new Size(840, 27);
            codeInjectPretextTextBox.TabIndex = 3;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.AutoSize = true;
            flowLayoutPanel1.Controls.Add(codeInjectLabel);
            flowLayoutPanel1.Controls.Add(tokenCountLabel);
            flowLayoutPanel1.FlowDirection = FlowDirection.TopDown;
            flowLayoutPanel1.Location = new Point(3, 40);
            flowLayoutPanel1.Margin = new Padding(3, 4, 3, 4);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(115, 72);
            flowLayoutPanel1.TabIndex = 13;
            // 
            // codeInjectLabel
            // 
            codeInjectLabel.AutoSize = true;
            codeInjectLabel.Location = new Point(3, 4);
            codeInjectLabel.Margin = new Padding(3, 4, 3, 4);
            codeInjectLabel.Name = "codeInjectLabel";
            codeInjectLabel.Padding = new Padding(3, 4, 3, 4);
            codeInjectLabel.Size = new Size(75, 28);
            codeInjectLabel.TabIndex = 6;
            codeInjectLabel.Text = "📜 Code";
            // 
            // tokenCountLabel
            // 
            tokenCountLabel.AutoSize = true;
            tokenCountLabel.Location = new Point(3, 40);
            tokenCountLabel.Margin = new Padding(3, 4, 3, 4);
            tokenCountLabel.Name = "tokenCountLabel";
            tokenCountLabel.Padding = new Padding(3, 4, 3, 4);
            tokenCountLabel.Size = new Size(109, 28);
            tokenCountLabel.TabIndex = 9;
            tokenCountLabel.Text = "Token Count 0";
            // 
            // largeCodeSnippetTextBox
            // 
            largeCodeSnippetTextBox.BackColor = SystemColors.InfoText;
            largeCodeSnippetTextBox.Dock = DockStyle.Fill;
            largeCodeSnippetTextBox.Font = new Font("Cascadia Code", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 2);
            largeCodeSnippetTextBox.ForeColor = SystemColors.MenuHighlight;
            largeCodeSnippetTextBox.Location = new Point(124, 40);
            largeCodeSnippetTextBox.Margin = new Padding(3, 4, 3, 4);
            largeCodeSnippetTextBox.Name = "largeCodeSnippetTextBox";
            largeCodeSnippetTextBox.Size = new Size(840, 800);
            largeCodeSnippetTextBox.TabIndex = 1;
            largeCodeSnippetTextBox.Text = resources.GetString("largeCodeSnippetTextBox.Text");
            largeCodeSnippetTextBox.TextChanged += largeCodeSnippetTextBox_TextChanged;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(3, 848);
            label4.Margin = new Padding(3, 4, 3, 4);
            label4.Name = "label4";
            label4.Padding = new Padding(3, 4, 3, 4);
            label4.Size = new Size(79, 28);
            label4.TabIndex = 7;
            label4.Text = "AI Pretext";
            // 
            // injectAssitantPretextTextBox
            // 
            injectAssitantPretextTextBox.Dock = DockStyle.Fill;
            injectAssitantPretextTextBox.Location = new Point(124, 848);
            injectAssitantPretextTextBox.Margin = new Padding(3, 4, 3, 4);
            injectAssitantPretextTextBox.Name = "injectAssitantPretextTextBox";
            injectAssitantPretextTextBox.Size = new Size(840, 27);
            injectAssitantPretextTextBox.TabIndex = 8;
            // 
            // codeFilePage
            // 
            codeFilePage.Controls.Add(codeFileInjectTabMainTablePanel);
            codeFilePage.Location = new Point(4, 29);
            codeFilePage.Margin = new Padding(3, 4, 3, 4);
            codeFilePage.Name = "codeFilePage";
            codeFilePage.Size = new Size(973, 888);
            codeFilePage.TabIndex = 4;
            codeFilePage.Text = "📜 Code File";
            codeFilePage.UseVisualStyleBackColor = true;
            // 
            // codeFileInjectTabMainTablePanel
            // 
            codeFileInjectTabMainTablePanel.AutoScroll = true;
            codeFileInjectTabMainTablePanel.AutoSize = true;
            codeFileInjectTabMainTablePanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            codeFileInjectTabMainTablePanel.BackColor = Color.DarkGray;
            codeFileInjectTabMainTablePanel.ColumnCount = 1;
            codeFileInjectTabMainTablePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            codeFileInjectTabMainTablePanel.Controls.Add(newFileInjectHolderTable, 0, 2);
            codeFileInjectTabMainTablePanel.Controls.Add(selectedCodeFileViewTable, 0, 1);
            codeFileInjectTabMainTablePanel.Controls.Add(codeFileInjectHeaderTable, 0, 0);
            codeFileInjectTabMainTablePanel.Dock = DockStyle.Fill;
            codeFileInjectTabMainTablePanel.Location = new Point(0, 0);
            codeFileInjectTabMainTablePanel.Margin = new Padding(3, 4, 3, 4);
            codeFileInjectTabMainTablePanel.Name = "codeFileInjectTabMainTablePanel";
            codeFileInjectTabMainTablePanel.Padding = new Padding(0, 0, 9, 0);
            codeFileInjectTabMainTablePanel.RowCount = 3;
            codeFileInjectTabMainTablePanel.RowStyles.Add(new RowStyle());
            codeFileInjectTabMainTablePanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            codeFileInjectTabMainTablePanel.RowStyles.Add(new RowStyle());
            codeFileInjectTabMainTablePanel.Size = new Size(973, 888);
            codeFileInjectTabMainTablePanel.TabIndex = 1;
            // 
            // newFileInjectHolderTable
            // 
            newFileInjectHolderTable.AutoSize = true;
            newFileInjectHolderTable.ColumnCount = 2;
            newFileInjectHolderTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            newFileInjectHolderTable.ColumnStyles.Add(new ColumnStyle());
            newFileInjectHolderTable.Controls.Add(addNewCodeFileInjectButton, 1, 0);
            newFileInjectHolderTable.Controls.Add(codeFileInjectTablePanel, 0, 0);
            newFileInjectHolderTable.Dock = DockStyle.Fill;
            newFileInjectHolderTable.Location = new Point(3, 556);
            newFileInjectHolderTable.Margin = new Padding(3, 4, 3, 4);
            newFileInjectHolderTable.Name = "newFileInjectHolderTable";
            newFileInjectHolderTable.RowCount = 1;
            newFileInjectHolderTable.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            newFileInjectHolderTable.Size = new Size(958, 328);
            newFileInjectHolderTable.TabIndex = 6;
            // 
            // addNewCodeFileInjectButton
            // 
            addNewCodeFileInjectButton.AutoSize = true;
            addNewCodeFileInjectButton.BackColor = Color.YellowGreen;
            addNewCodeFileInjectButton.Dock = DockStyle.Fill;
            addNewCodeFileInjectButton.FlatStyle = FlatStyle.Flat;
            addNewCodeFileInjectButton.ForeColor = SystemColors.ButtonFace;
            addNewCodeFileInjectButton.Location = new Point(840, 4);
            addNewCodeFileInjectButton.Margin = new Padding(3, 4, 3, 4);
            addNewCodeFileInjectButton.Name = "addNewCodeFileInjectButton";
            addNewCodeFileInjectButton.Size = new Size(115, 320);
            addNewCodeFileInjectButton.TabIndex = 1;
            addNewCodeFileInjectButton.Text = "➕ Add File";
            addNewCodeFileInjectButton.UseVisualStyleBackColor = false;
            addNewCodeFileInjectButton.Click += addNewCodeFileInjectButton_Click;
            // 
            // selectedCodeFileViewTable
            // 
            selectedCodeFileViewTable.AutoScroll = true;
            selectedCodeFileViewTable.AutoSize = true;
            selectedCodeFileViewTable.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            selectedCodeFileViewTable.ColumnCount = 1;
            selectedCodeFileViewTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            selectedCodeFileViewTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 23F));
            selectedCodeFileViewTable.Dock = DockStyle.Fill;
            selectedCodeFileViewTable.Location = new Point(3, 48);
            selectedCodeFileViewTable.Margin = new Padding(3, 4, 3, 4);
            selectedCodeFileViewTable.Name = "selectedCodeFileViewTable";
            selectedCodeFileViewTable.RowCount = 1;
            selectedCodeFileViewTable.RowStyles.Add(new RowStyle());
            selectedCodeFileViewTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 480F));
            selectedCodeFileViewTable.Size = new Size(958, 500);
            selectedCodeFileViewTable.TabIndex = 4;
            // 
            // codeFileInjectHeaderTable
            // 
            codeFileInjectHeaderTable.AutoSize = true;
            codeFileInjectHeaderTable.BackColor = Color.DarkGray;
            codeFileInjectHeaderTable.ColumnCount = 5;
            codeFileInjectHeaderTable.ColumnStyles.Add(new ColumnStyle());
            codeFileInjectHeaderTable.ColumnStyles.Add(new ColumnStyle());
            codeFileInjectHeaderTable.ColumnStyles.Add(new ColumnStyle());
            codeFileInjectHeaderTable.ColumnStyles.Add(new ColumnStyle());
            codeFileInjectHeaderTable.ColumnStyles.Add(new ColumnStyle());
            codeFileInjectHeaderTable.Controls.Add(presetLabel, 1, 0);
            codeFileInjectHeaderTable.Controls.Add(presetSelectComboBox, 2, 0);
            codeFileInjectHeaderTable.Controls.Add(saveCodeFileInjectPresetButton, 3, 0);
            codeFileInjectHeaderTable.Dock = DockStyle.Fill;
            codeFileInjectHeaderTable.Location = new Point(3, 4);
            codeFileInjectHeaderTable.Margin = new Padding(3, 4, 3, 4);
            codeFileInjectHeaderTable.Name = "codeFileInjectHeaderTable";
            codeFileInjectHeaderTable.RowCount = 1;
            codeFileInjectHeaderTable.RowStyles.Add(new RowStyle());
            codeFileInjectHeaderTable.Size = new Size(958, 36);
            codeFileInjectHeaderTable.TabIndex = 2;
            // 
            // presetLabel
            // 
            presetLabel.AutoSize = true;
            presetLabel.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            presetLabel.ForeColor = Color.White;
            presetLabel.Location = new Point(11, 7);
            presetLabel.Margin = new Padding(11, 7, 3, 0);
            presetLabel.Name = "presetLabel";
            presetLabel.Size = new Size(95, 23);
            presetLabel.TabIndex = 4;
            presetLabel.Text = "❄️ Presets";
            // 
            // presetSelectComboBox
            // 
            presetSelectComboBox.DisplayMember = "Name";
            presetSelectComboBox.FormattingEnabled = true;
            presetSelectComboBox.Location = new Point(112, 4);
            presetSelectComboBox.Margin = new Padding(3, 4, 3, 4);
            presetSelectComboBox.Name = "presetSelectComboBox";
            presetSelectComboBox.Size = new Size(138, 28);
            presetSelectComboBox.TabIndex = 2;
            presetSelectComboBox.SelectedIndexChanged += presetSelectComboBox_SelectedIndexChanged;
            // 
            // saveCodeFileInjectPresetButton
            // 
            saveCodeFileInjectPresetButton.BackColor = Color.LimeGreen;
            saveCodeFileInjectPresetButton.Cursor = Cursors.Hand;
            saveCodeFileInjectPresetButton.FlatStyle = FlatStyle.Flat;
            saveCodeFileInjectPresetButton.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            saveCodeFileInjectPresetButton.ForeColor = Color.White;
            saveCodeFileInjectPresetButton.Location = new Point(253, 4);
            saveCodeFileInjectPresetButton.Margin = new Padding(0, 4, 0, 0);
            saveCodeFileInjectPresetButton.Name = "saveCodeFileInjectPresetButton";
            saveCodeFileInjectPresetButton.Size = new Size(86, 31);
            saveCodeFileInjectPresetButton.TabIndex = 5;
            saveCodeFileInjectPresetButton.Text = "💾 Save";
            saveCodeFileInjectPresetButton.UseVisualStyleBackColor = false;
            saveCodeFileInjectPresetButton.Visible = false;
            saveCodeFileInjectPresetButton.Click += saveCodeFileInjectPresetButton_Click;
            // 
            // settingsPage
            // 
            settingsPage.Controls.Add(tableLayoutPanel4);
            settingsPage.Location = new Point(4, 29);
            settingsPage.Margin = new Padding(3, 4, 3, 4);
            settingsPage.Name = "settingsPage";
            settingsPage.Padding = new Padding(3, 4, 3, 4);
            settingsPage.Size = new Size(973, 888);
            settingsPage.TabIndex = 2;
            settingsPage.Text = "⚙️ Settings";
            settingsPage.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel4
            // 
            tableLayoutPanel4.AutoSize = true;
            tableLayoutPanel4.ColumnCount = 2;
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel4.Controls.Add(topPTextBox, 1, 1);
            tableLayoutPanel4.Controls.Add(topPLabel, 0, 1);
            tableLayoutPanel4.Controls.Add(temperatureTextBox, 1, 0);
            tableLayoutPanel4.Controls.Add(label3, 0, 0);
            tableLayoutPanel4.Dock = DockStyle.Fill;
            tableLayoutPanel4.Location = new Point(3, 4);
            tableLayoutPanel4.Margin = new Padding(3, 4, 3, 4);
            tableLayoutPanel4.Name = "tableLayoutPanel4";
            tableLayoutPanel4.Padding = new Padding(3, 4, 3, 4);
            tableLayoutPanel4.RowCount = 4;
            tableLayoutPanel4.RowStyles.Add(new RowStyle());
            tableLayoutPanel4.RowStyles.Add(new RowStyle());
            tableLayoutPanel4.RowStyles.Add(new RowStyle());
            tableLayoutPanel4.RowStyles.Add(new RowStyle());
            tableLayoutPanel4.Size = new Size(967, 880);
            tableLayoutPanel4.TabIndex = 1;
            // 
            // topPTextBox
            // 
            topPTextBox.Location = new Point(105, 43);
            topPTextBox.Margin = new Padding(3, 4, 3, 4);
            topPTextBox.Name = "topPTextBox";
            topPTextBox.Size = new Size(114, 27);
            topPTextBox.TabIndex = 7;
            // 
            // topPLabel
            // 
            topPLabel.AutoSize = true;
            topPLabel.Location = new Point(6, 48);
            topPLabel.Margin = new Padding(3, 9, 3, 0);
            topPLabel.Name = "topPLabel";
            topPLabel.Size = new Size(46, 20);
            topPLabel.TabIndex = 5;
            topPLabel.Text = "Top P";
            // 
            // temperatureTextBox
            // 
            temperatureTextBox.Location = new Point(105, 8);
            temperatureTextBox.Margin = new Padding(3, 4, 3, 4);
            temperatureTextBox.Name = "temperatureTextBox";
            temperatureTextBox.Size = new Size(114, 27);
            temperatureTextBox.TabIndex = 3;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(6, 13);
            label3.Margin = new Padding(3, 9, 3, 0);
            label3.Name = "label3";
            label3.Size = new Size(93, 20);
            label3.TabIndex = 1;
            label3.Text = "Temperature";
            // 
            // nerdStatsPage
            // 
            nerdStatsPage.Location = new Point(4, 29);
            nerdStatsPage.Margin = new Padding(3, 4, 3, 4);
            nerdStatsPage.Name = "nerdStatsPage";
            nerdStatsPage.Size = new Size(973, 888);
            nerdStatsPage.TabIndex = 5;
            nerdStatsPage.Text = "\U0001f9e0 Nerd Stats";
            nerdStatsPage.UseVisualStyleBackColor = true;
            // 
            // sidePanelTable
            // 
            sidePanelTable.ColumnCount = 1;
            sidePanelTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            sidePanelTable.Controls.Add(totalByteUsageMeterTextLabel, 0, 1);
            sidePanelTable.Controls.Add(llmSelectorTable, 0, 0);
            sidePanelTable.Controls.Add(totalTokenUsageTable, 0, 1);
            sidePanelTable.Controls.Add(pastPromptsLabel, 0, 2);
            sidePanelTable.Controls.Add(pastUserPrompts, 0, 3);
            sidePanelTable.Dock = DockStyle.Fill;
            sidePanelTable.Location = new Point(0, 0);
            sidePanelTable.Margin = new Padding(3, 4, 3, 4);
            sidePanelTable.Name = "sidePanelTable";
            sidePanelTable.RowCount = 6;
            sidePanelTable.RowStyles.Add(new RowStyle());
            sidePanelTable.RowStyles.Add(new RowStyle());
            sidePanelTable.RowStyles.Add(new RowStyle());
            sidePanelTable.RowStyles.Add(new RowStyle());
            sidePanelTable.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            sidePanelTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 27F));
            sidePanelTable.Size = new Size(248, 921);
            sidePanelTable.TabIndex = 4;
            // 
            // llmSelectorTable
            // 
            llmSelectorTable.AutoSize = true;
            llmSelectorTable.ColumnCount = 2;
            llmSelectorTable.ColumnStyles.Add(new ColumnStyle());
            llmSelectorTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            llmSelectorTable.Controls.Add(modalSelectorLabel, 0, 0);
            llmSelectorTable.Controls.Add(llmSelector, 1, 0);
            llmSelectorTable.Dock = DockStyle.Fill;
            llmSelectorTable.Location = new Point(3, 4);
            llmSelectorTable.Margin = new Padding(3, 4, 3, 4);
            llmSelectorTable.Name = "llmSelectorTable";
            llmSelectorTable.RowCount = 1;
            llmSelectorTable.RowStyles.Add(new RowStyle());
            llmSelectorTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 45F));
            llmSelectorTable.Size = new Size(242, 45);
            llmSelectorTable.TabIndex = 7;
            // 
            // llmSelector
            // 
            llmSelector.Dock = DockStyle.Fill;
            llmSelector.FormattingEnabled = true;
            llmSelector.Location = new Point(94, 4);
            llmSelector.Margin = new Padding(3, 4, 3, 4);
            llmSelector.Name = "llmSelector";
            llmSelector.Size = new Size(145, 28);
            llmSelector.TabIndex = 1;
            llmSelector.SelectedIndexChanged += llmSelector_SelectedIndexChanged;
            // 
            // totalTokenUsageTable
            // 
            totalTokenUsageTable.AutoSize = true;
            totalTokenUsageTable.ColumnCount = 2;
            totalTokenUsageTable.ColumnStyles.Add(new ColumnStyle());
            totalTokenUsageTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            totalTokenUsageTable.Controls.Add(finalChatTokenLimitLabel, 0, 0);
            totalTokenUsageTable.Controls.Add(finalChatTokenUsageProgressBar, 1, 0);
            totalTokenUsageTable.Dock = DockStyle.Fill;
            totalTokenUsageTable.Location = new Point(3, 57);
            totalTokenUsageTable.Margin = new Padding(3, 4, 3, 4);
            totalTokenUsageTable.Name = "totalTokenUsageTable";
            totalTokenUsageTable.RowCount = 1;
            totalTokenUsageTable.RowStyles.Add(new RowStyle());
            totalTokenUsageTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 43F));
            totalTokenUsageTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 43F));
            totalTokenUsageTable.Size = new Size(242, 43);
            totalTokenUsageTable.TabIndex = 5;
            // 
            // finalChatTokenLimitLabel
            // 
            finalChatTokenLimitLabel.AutoSize = true;
            finalChatTokenLimitLabel.Dock = DockStyle.Fill;
            finalChatTokenLimitLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            finalChatTokenLimitLabel.ForeColor = Color.White;
            finalChatTokenLimitLabel.Location = new Point(3, 4);
            finalChatTokenLimitLabel.Margin = new Padding(3, 4, 3, 4);
            finalChatTokenLimitLabel.Name = "finalChatTokenLimitLabel";
            finalChatTokenLimitLabel.Padding = new Padding(3, 4, 3, 4);
            finalChatTokenLimitLabel.Size = new Size(96, 35);
            finalChatTokenLimitLabel.TabIndex = 18;
            finalChatTokenLimitLabel.Text = "🌡️ Context";
            // 
            // finalChatTokenUsageProgressBar
            // 
            finalChatTokenUsageProgressBar.BackColor = Color.White;
            finalChatTokenUsageProgressBar.DisplayText = "10K / 32K Tokens";
            finalChatTokenUsageProgressBar.Dock = DockStyle.Fill;
            finalChatTokenUsageProgressBar.Location = new Point(105, 4);
            finalChatTokenUsageProgressBar.Margin = new Padding(3, 4, 3, 4);
            finalChatTokenUsageProgressBar.MarqueeAnimationSpeed = 1000;
            finalChatTokenUsageProgressBar.Name = "finalChatTokenUsageProgressBar";
            finalChatTokenUsageProgressBar.ProgressBarColor = Color.LightGreen;
            finalChatTokenUsageProgressBar.Size = new Size(134, 35);
            finalChatTokenUsageProgressBar.Style = ProgressBarStyle.Continuous;
            finalChatTokenUsageProgressBar.TabIndex = 17;
            // 
            // pastUserPrompts
            // 
            pastUserPrompts.BackColor = Color.Gainsboro;
            pastUserPrompts.Dock = DockStyle.Fill;
            pastUserPrompts.FormattingEnabled = true;
            pastUserPrompts.Location = new Point(3, 148);
            pastUserPrompts.Margin = new Padding(3, 4, 3, 4);
            pastUserPrompts.Name = "pastUserPrompts";
            pastUserPrompts.Size = new Size(242, 742);
            pastUserPrompts.TabIndex = 1;
            pastUserPrompts.SelectedIndexChanged += pastUserPrompts_SelectedIndexChanged;
            // 
            // tabPage3
            // 
            tabPage3.Location = new Point(0, 0);
            tabPage3.Name = "tabPage3";
            tabPage3.Padding = new Padding(3);
            tabPage3.Size = new Size(200, 100);
            tabPage3.TabIndex = 0;
            tabPage3.Text = "tabPage3";
            tabPage3.UseVisualStyleBackColor = true;
            // 
            // tabPage4
            // 
            tabPage4.Location = new Point(0, 0);
            tabPage4.Name = "tabPage4";
            tabPage4.Padding = new Padding(3);
            tabPage4.Size = new Size(200, 100);
            tabPage4.TabIndex = 1;
            tabPage4.Text = "tabPage4";
            tabPage4.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.DimGray;
            ClientSize = new Size(1234, 921);
            Controls.Add(splitContainer1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(3, 4, 3, 4);
            Name = "Form1";
            Text = "LLMCodes";
            codeFileInjectTablePanel.ResumeLayout(false);
            codeFileInjectTablePanel.PerformLayout();
            lineNumRangeInputTable.ResumeLayout(false);
            lineNumRangeInputTable.PerformLayout();
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            mainTabControl.ResumeLayout(false);
            llmPage.ResumeLayout(false);
            mainChatPageHolderTable.ResumeLayout(false);
            mainChatPageHolderTable.PerformLayout();
            chatInputHolderTable.ResumeLayout(false);
            chatInputHolderTable.PerformLayout();
            mainChatHeaderRow.ResumeLayout(false);
            mainChatHeaderRow.PerformLayout();
            llmButtonRowHolderTable.ResumeLayout(false);
            llmButtonRowHolderTable.PerformLayout();
            codePage.ResumeLayout(false);
            codePage.PerformLayout();
            codeSnippetInjectMainTablePanel.ResumeLayout(false);
            codeSnippetInjectMainTablePanel.PerformLayout();
            flowLayoutPanel1.ResumeLayout(false);
            flowLayoutPanel1.PerformLayout();
            codeFilePage.ResumeLayout(false);
            codeFilePage.PerformLayout();
            codeFileInjectTabMainTablePanel.ResumeLayout(false);
            codeFileInjectTabMainTablePanel.PerformLayout();
            newFileInjectHolderTable.ResumeLayout(false);
            newFileInjectHolderTable.PerformLayout();
            codeFileInjectHeaderTable.ResumeLayout(false);
            codeFileInjectHeaderTable.PerformLayout();
            settingsPage.ResumeLayout(false);
            settingsPage.PerformLayout();
            tableLayoutPanel4.ResumeLayout(false);
            tableLayoutPanel4.PerformLayout();
            sidePanelTable.ResumeLayout(false);
            sidePanelTable.PerformLayout();
            llmSelectorTable.ResumeLayout(false);
            totalTokenUsageTable.ResumeLayout(false);
            totalTokenUsageTable.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private SplitContainer splitContainer1;
        private Button sendUserMsgButton;
        private ListBox pastUserPrompts;
        private Button clearUserMsgButton;
        private TabControl mainTabControl;
        private TabPage llmPage;
        private Button resetChatHistoryButton;
        private ProgressBar llmThinkingProgressBar;
        private TableLayoutPanel sidePanelTable;
        private TableLayoutPanel mainChatPageHolderTable;
        private TabPage settingsPage;
        private TabPage tabPage3;
        private TabPage tabPage4;
        private TableLayoutPanel tableLayoutPanel4;
        private TabPage codePage;
        private TableLayoutPanel codeSnippetInjectMainTablePanel;
        private Label label2;
        private TextBox codeInjectPretextTextBox;
        private FlowLayoutPanel flowLayoutPanel1;
        private Label codeInjectLabel;
        private Label tokenCountLabel;
        private RichTextBox largeCodeSnippetTextBox;
        private Label label4;
        private TextBox injectAssitantPretextTextBox;
        private TextBox topPTextBox;
        private Label topPLabel;
        private TextBox temperatureTextBox;
        private Label label3;
        private TableLayoutPanel llmButtonRowHolderTable;
        private TableLayoutPanel outputChatMessagePanel;
        private Label llmThinkingTimerLabel;
        private TabPage codeFilePage;
        private TableLayoutPanel codeFileInjectTabMainTablePanel;
        private TabPage nerdStatsPage;
        private TableLayoutPanel mainChatHeaderRow;
        private ComboBox llmChatHistorySelectComboBox;
        private Label llmChatHistoryLabel;
        private Label injectSourceLabel;
        private CheckBox webInjectCheckBox;
        private CheckBox fileInjectCheckBox;
        private CheckBox snippetInjectCheckBox;
        private RichTextBox userInputTextBox;
        private TableLayoutPanel totalTokenUsageTable;
        private Label finalChatTokenLimitLabel;
        private CustomProgressBar finalChatTokenUsageProgressBar;
        private TableLayoutPanel codeFileInjectHeaderTable;
        private Button addNewCodeFileInjectButton;
        private Label totalByteUsageMeterTextLabel;
        private TableLayoutPanel chatInputHolderTable;
        private Label llmThinkingLabel;
        private Button terminateOngoingLLMCallButton;
        private TableLayoutPanel llmSelectorTable;
        private ComboBox llmSelector;
        private Button duplicateCurrentChatHistoryButton;
        private Label presetLabel;
        private ComboBox presetSelectComboBox;
        private Button saveCodeFileInjectPresetButton;
        private TableLayoutPanel selectedCodeFileViewTable;
        private TableLayoutPanel newFileInjectHolderTable;
        private TableLayoutPanel codeFileInjectTablePanel;
        private TextBox codeFileInjectPathTextBox;
        private Label fileInjectPathLabel;
        private TableLayoutPanel lineNumRangeInputTable;
        private TextBox startLineNumberTextBox = new TextBox();
        private Label startLabel;
        private Label endLabel;
        private TextBox endLineNumberTextBox;
        private Label lineNumberRangeLabel;
        private Label fetchCodeStatusMessageLabel;
        private Label preCodePromptLabel;
        private TextBox preCodePromptTextBox;
        private Label codeFileInjectLabel;
        private RichTextBox codeFileInjectTextBox;
        private Label postCodePromptLabel;
        private TextBox postCodePromptTextBox;
        private Button copyFinalPromptButton;
    }
}
