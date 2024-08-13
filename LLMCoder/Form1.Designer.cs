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
            codeFileInjectHeaderTable = new TableLayoutPanel();
            loadSelectedFileInjectPresetButton = new Button();
            presetLabel = new Label();
            addNewCodeFileInjectButton = new Button();
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
            selectedCodeFileViewTable = new TableLayoutPanel();
            pastPromptsLabel = new Label();
            modalSelectorLabel = new Label();
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
            pastPromptsLabel.Location = new Point(3, 85);
            pastPromptsLabel.Name = "pastPromptsLabel";
            pastPromptsLabel.Size = new Size(210, 15);
            pastPromptsLabel.TabIndex = 2;
            pastPromptsLabel.Text = "📜 Past Prompts";
            // 
            // modalSelectorLabel
            // 
            modalSelectorLabel.Dock = DockStyle.Fill;
            modalSelectorLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            modalSelectorLabel.ForeColor = Color.White;
            modalSelectorLabel.Location = new Point(3, 7);
            modalSelectorLabel.Margin = new Padding(3, 7, 3, 0);
            modalSelectorLabel.Name = "modalSelectorLabel";
            modalSelectorLabel.Size = new Size(74, 22);
            modalSelectorLabel.TabIndex = 3;
            modalSelectorLabel.Text = "✨ Modal";
            // 
            // totalByteUsageMeterTextLabel
            // 
            totalByteUsageMeterTextLabel.AutoSize = true;
            totalByteUsageMeterTextLabel.Dock = DockStyle.Fill;
            totalByteUsageMeterTextLabel.ForeColor = Color.Coral;
            totalByteUsageMeterTextLabel.ImageAlign = ContentAlignment.MiddleRight;
            totalByteUsageMeterTextLabel.Location = new Point(3, 70);
            totalByteUsageMeterTextLabel.Name = "totalByteUsageMeterTextLabel";
            totalByteUsageMeterTextLabel.Size = new Size(210, 15);
            totalByteUsageMeterTextLabel.TabIndex = 9;
            totalByteUsageMeterTextLabel.Text = "0 / 256 KB";
            totalByteUsageMeterTextLabel.TextAlign = ContentAlignment.TopRight;
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 0);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(mainTabControl);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(sidePanelTable);
            splitContainer1.Size = new Size(1080, 691);
            splitContainer1.SplitterDistance = 860;
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
            mainTabControl.Name = "mainTabControl";
            mainTabControl.SelectedIndex = 0;
            mainTabControl.Size = new Size(860, 691);
            mainTabControl.TabIndex = 4;
            // 
            // llmPage
            // 
            llmPage.BackColor = Color.FromArgb(39, 42, 49);
            llmPage.Controls.Add(mainChatPageHolderTable);
            llmPage.ForeColor = Color.FromArgb(28, 30, 35);
            llmPage.Location = new Point(4, 24);
            llmPage.Name = "llmPage";
            llmPage.Padding = new Padding(3);
            llmPage.Size = new Size(852, 663);
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
            mainChatPageHolderTable.Location = new Point(3, 3);
            mainChatPageHolderTable.Name = "mainChatPageHolderTable";
            mainChatPageHolderTable.RowCount = 4;
            mainChatPageHolderTable.RowStyles.Add(new RowStyle());
            mainChatPageHolderTable.RowStyles.Add(new RowStyle(SizeType.Percent, 80F));
            mainChatPageHolderTable.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            mainChatPageHolderTable.RowStyles.Add(new RowStyle());
            mainChatPageHolderTable.RowStyles.Add(new RowStyle());
            mainChatPageHolderTable.Size = new Size(846, 657);
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
            chatInputHolderTable.Location = new Point(3, 507);
            chatInputHolderTable.Name = "chatInputHolderTable";
            chatInputHolderTable.RowCount = 1;
            chatInputHolderTable.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            chatInputHolderTable.Size = new Size(840, 111);
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
            sendUserMsgButton.Location = new Point(774, 3);
            sendUserMsgButton.Name = "sendUserMsgButton";
            sendUserMsgButton.Size = new Size(63, 105);
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
            userInputTextBox.Location = new Point(3, 3);
            userInputTextBox.Name = "userInputTextBox";
            userInputTextBox.Size = new Size(765, 105);
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
            mainChatHeaderRow.Location = new Point(3, 3);
            mainChatHeaderRow.Name = "mainChatHeaderRow";
            mainChatHeaderRow.RowCount = 1;
            mainChatHeaderRow.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            mainChatHeaderRow.Size = new Size(840, 29);
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
            duplicateCurrentChatHistoryButton.Location = new Point(363, 2);
            duplicateCurrentChatHistoryButton.Margin = new Padding(5, 2, 0, 0);
            duplicateCurrentChatHistoryButton.Name = "duplicateCurrentChatHistoryButton";
            duplicateCurrentChatHistoryButton.Size = new Size(84, 25);
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
            resetChatHistoryButton.Location = new Point(295, 2);
            resetChatHistoryButton.Margin = new Padding(0, 2, 0, 0);
            resetChatHistoryButton.Name = "resetChatHistoryButton";
            resetChatHistoryButton.Size = new Size(63, 25);
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
            injectSourceLabel.Location = new Point(592, 3);
            injectSourceLabel.Margin = new Padding(3);
            injectSourceLabel.Name = "injectSourceLabel";
            injectSourceLabel.Padding = new Padding(3);
            injectSourceLabel.Size = new Size(67, 23);
            injectSourceLabel.TabIndex = 9;
            injectSourceLabel.Text = "💉Inject";
            // 
            // llmChatHistorySelectComboBox
            // 
            llmChatHistorySelectComboBox.Dock = DockStyle.Fill;
            llmChatHistorySelectComboBox.FormattingEnabled = true;
            llmChatHistorySelectComboBox.Location = new Point(92, 3);
            llmChatHistorySelectComboBox.Name = "llmChatHistorySelectComboBox";
            llmChatHistorySelectComboBox.Size = new Size(200, 23);
            llmChatHistorySelectComboBox.TabIndex = 8;
            // 
            // llmChatHistoryLabel
            // 
            llmChatHistoryLabel.AutoSize = true;
            llmChatHistoryLabel.Dock = DockStyle.Fill;
            llmChatHistoryLabel.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            llmChatHistoryLabel.ForeColor = Color.White;
            llmChatHistoryLabel.Location = new Point(3, 3);
            llmChatHistoryLabel.Margin = new Padding(3);
            llmChatHistoryLabel.Name = "llmChatHistoryLabel";
            llmChatHistoryLabel.Padding = new Padding(3);
            llmChatHistoryLabel.Size = new Size(83, 23);
            llmChatHistoryLabel.TabIndex = 6;
            llmChatHistoryLabel.Text = "📒 History";
            // 
            // webInjectCheckBox
            // 
            webInjectCheckBox.AutoSize = true;
            webInjectCheckBox.ForeColor = Color.White;
            webInjectCheckBox.Location = new Point(787, 6);
            webInjectCheckBox.Margin = new Padding(3, 6, 3, 3);
            webInjectCheckBox.Name = "webInjectCheckBox";
            webInjectCheckBox.Size = new Size(50, 19);
            webInjectCheckBox.TabIndex = 13;
            webInjectCheckBox.Text = "Web";
            webInjectCheckBox.UseVisualStyleBackColor = true;
            // 
            // fileInjectCheckBox
            // 
            fileInjectCheckBox.AutoSize = true;
            fileInjectCheckBox.ForeColor = Color.White;
            fileInjectCheckBox.Location = new Point(737, 6);
            fileInjectCheckBox.Margin = new Padding(3, 6, 3, 3);
            fileInjectCheckBox.Name = "fileInjectCheckBox";
            fileInjectCheckBox.Size = new Size(44, 19);
            fileInjectCheckBox.TabIndex = 12;
            fileInjectCheckBox.Text = "File";
            fileInjectCheckBox.UseVisualStyleBackColor = true;
            // 
            // snippetInjectCheckBox
            // 
            snippetInjectCheckBox.AutoSize = true;
            snippetInjectCheckBox.ForeColor = Color.White;
            snippetInjectCheckBox.Location = new Point(665, 6);
            snippetInjectCheckBox.Margin = new Padding(3, 6, 3, 3);
            snippetInjectCheckBox.Name = "snippetInjectCheckBox";
            snippetInjectCheckBox.Size = new Size(66, 19);
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
            outputChatMessagePanel.Location = new Point(3, 38);
            outputChatMessagePanel.Name = "outputChatMessagePanel";
            outputChatMessagePanel.RowCount = 1;
            outputChatMessagePanel.RowStyles.Add(new RowStyle());
            outputChatMessagePanel.Size = new Size(840, 463);
            outputChatMessagePanel.TabIndex = 7;
            // 
            // llmButtonRowHolderTable
            // 
            llmButtonRowHolderTable.AutoSize = true;
            llmButtonRowHolderTable.ColumnCount = 6;
            llmButtonRowHolderTable.ColumnStyles.Add(new ColumnStyle());
            llmButtonRowHolderTable.ColumnStyles.Add(new ColumnStyle());
            llmButtonRowHolderTable.ColumnStyles.Add(new ColumnStyle());
            llmButtonRowHolderTable.ColumnStyles.Add(new ColumnStyle());
            llmButtonRowHolderTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            llmButtonRowHolderTable.ColumnStyles.Add(new ColumnStyle());
            llmButtonRowHolderTable.Controls.Add(terminateOngoingLLMCallButton, 2, 0);
            llmButtonRowHolderTable.Controls.Add(llmThinkingLabel, 3, 0);
            llmButtonRowHolderTable.Controls.Add(llmThinkingTimerLabel, 0, 0);
            llmButtonRowHolderTable.Controls.Add(llmThinkingProgressBar, 1, 0);
            llmButtonRowHolderTable.Controls.Add(clearUserMsgButton, 5, 0);
            llmButtonRowHolderTable.Dock = DockStyle.Bottom;
            llmButtonRowHolderTable.Location = new Point(3, 625);
            llmButtonRowHolderTable.Name = "llmButtonRowHolderTable";
            llmButtonRowHolderTable.RowCount = 1;
            llmButtonRowHolderTable.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            llmButtonRowHolderTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            llmButtonRowHolderTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            llmButtonRowHolderTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            llmButtonRowHolderTable.Size = new Size(840, 29);
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
            terminateOngoingLLMCallButton.Location = new Point(132, 2);
            terminateOngoingLLMCallButton.Margin = new Padding(0, 2, 0, 0);
            terminateOngoingLLMCallButton.Name = "terminateOngoingLLMCallButton";
            terminateOngoingLLMCallButton.Size = new Size(69, 25);
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
            llmThinkingLabel.Location = new Point(201, 7);
            llmThinkingLabel.Margin = new Padding(0, 7, 0, 0);
            llmThinkingLabel.Name = "llmThinkingLabel";
            llmThinkingLabel.Size = new Size(105, 15);
            llmThinkingLabel.TabIndex = 9;
            llmThinkingLabel.Text = "💖 LLM Thinking...";
            llmThinkingLabel.Visible = false;
            // 
            // llmThinkingTimerLabel
            // 
            llmThinkingTimerLabel.AutoSize = true;
            llmThinkingTimerLabel.Font = new Font("Segoe UI Black", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            llmThinkingTimerLabel.ForeColor = Color.LawnGreen;
            llmThinkingTimerLabel.Location = new Point(0, 6);
            llmThinkingTimerLabel.Margin = new Padding(0, 6, 0, 0);
            llmThinkingTimerLabel.Name = "llmThinkingTimerLabel";
            llmThinkingTimerLabel.Size = new Size(26, 17);
            llmThinkingTimerLabel.TabIndex = 7;
            llmThinkingTimerLabel.Text = "0 s";
            // 
            // llmThinkingProgressBar
            // 
            llmThinkingProgressBar.Location = new Point(29, 3);
            llmThinkingProgressBar.MarqueeAnimationSpeed = 2000;
            llmThinkingProgressBar.Name = "llmThinkingProgressBar";
            llmThinkingProgressBar.Size = new Size(100, 23);
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
            clearUserMsgButton.Location = new Point(774, 0);
            clearUserMsgButton.Margin = new Padding(0, 0, 3, 0);
            clearUserMsgButton.Name = "clearUserMsgButton";
            clearUserMsgButton.Size = new Size(63, 27);
            clearUserMsgButton.TabIndex = 3;
            clearUserMsgButton.Text = "Clear 🗑️";
            clearUserMsgButton.UseVisualStyleBackColor = false;
            clearUserMsgButton.Click += clearUserMsgButton_Click;
            // 
            // codePage
            // 
            codePage.Controls.Add(codeSnippetInjectMainTablePanel);
            codePage.Location = new Point(4, 24);
            codePage.Name = "codePage";
            codePage.Padding = new Padding(3);
            codePage.Size = new Size(852, 663);
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
            codeSnippetInjectMainTablePanel.Location = new Point(3, 3);
            codeSnippetInjectMainTablePanel.Name = "codeSnippetInjectMainTablePanel";
            codeSnippetInjectMainTablePanel.RowCount = 3;
            codeSnippetInjectMainTablePanel.RowStyles.Add(new RowStyle());
            codeSnippetInjectMainTablePanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            codeSnippetInjectMainTablePanel.RowStyles.Add(new RowStyle());
            codeSnippetInjectMainTablePanel.Size = new Size(846, 657);
            codeSnippetInjectMainTablePanel.TabIndex = 11;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Dock = DockStyle.Left;
            label2.Location = new Point(3, 3);
            label2.Margin = new Padding(3);
            label2.Name = "label2";
            label2.Padding = new Padding(3);
            label2.Size = new Size(50, 23);
            label2.TabIndex = 5;
            label2.Text = "Pretext";
            // 
            // codeInjectPretextTextBox
            // 
            codeInjectPretextTextBox.Dock = DockStyle.Fill;
            codeInjectPretextTextBox.Location = new Point(104, 3);
            codeInjectPretextTextBox.Name = "codeInjectPretextTextBox";
            codeInjectPretextTextBox.Size = new Size(739, 23);
            codeInjectPretextTextBox.TabIndex = 3;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.AutoSize = true;
            flowLayoutPanel1.Controls.Add(codeInjectLabel);
            flowLayoutPanel1.Controls.Add(tokenCountLabel);
            flowLayoutPanel1.FlowDirection = FlowDirection.TopDown;
            flowLayoutPanel1.Location = new Point(3, 32);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(95, 54);
            flowLayoutPanel1.TabIndex = 13;
            // 
            // codeInjectLabel
            // 
            codeInjectLabel.AutoSize = true;
            codeInjectLabel.Location = new Point(3, 3);
            codeInjectLabel.Margin = new Padding(3);
            codeInjectLabel.Name = "codeInjectLabel";
            codeInjectLabel.Padding = new Padding(3);
            codeInjectLabel.Size = new Size(56, 21);
            codeInjectLabel.TabIndex = 6;
            codeInjectLabel.Text = "📜 Code";
            // 
            // tokenCountLabel
            // 
            tokenCountLabel.AutoSize = true;
            tokenCountLabel.Location = new Point(3, 30);
            tokenCountLabel.Margin = new Padding(3);
            tokenCountLabel.Name = "tokenCountLabel";
            tokenCountLabel.Padding = new Padding(3);
            tokenCountLabel.Size = new Size(89, 21);
            tokenCountLabel.TabIndex = 9;
            tokenCountLabel.Text = "Token Count 0";
            // 
            // largeCodeSnippetTextBox
            // 
            largeCodeSnippetTextBox.BackColor = SystemColors.InfoText;
            largeCodeSnippetTextBox.Dock = DockStyle.Fill;
            largeCodeSnippetTextBox.Font = new Font("Cascadia Code", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 2);
            largeCodeSnippetTextBox.ForeColor = SystemColors.MenuHighlight;
            largeCodeSnippetTextBox.Location = new Point(104, 32);
            largeCodeSnippetTextBox.Name = "largeCodeSnippetTextBox";
            largeCodeSnippetTextBox.Size = new Size(739, 593);
            largeCodeSnippetTextBox.TabIndex = 1;
            largeCodeSnippetTextBox.Text = resources.GetString("largeCodeSnippetTextBox.Text");
            largeCodeSnippetTextBox.TextChanged += largeCodeSnippetTextBox_TextChanged;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(3, 631);
            label4.Margin = new Padding(3);
            label4.Name = "label4";
            label4.Padding = new Padding(3);
            label4.Size = new Size(64, 21);
            label4.TabIndex = 7;
            label4.Text = "AI Pretext";
            // 
            // injectAssitantPretextTextBox
            // 
            injectAssitantPretextTextBox.Dock = DockStyle.Fill;
            injectAssitantPretextTextBox.Location = new Point(104, 631);
            injectAssitantPretextTextBox.Name = "injectAssitantPretextTextBox";
            injectAssitantPretextTextBox.Size = new Size(739, 23);
            injectAssitantPretextTextBox.TabIndex = 8;
            // 
            // codeFilePage
            // 
            codeFilePage.Controls.Add(codeFileInjectTabMainTablePanel);
            codeFilePage.Location = new Point(4, 24);
            codeFilePage.Name = "codeFilePage";
            codeFilePage.Size = new Size(852, 663);
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
            codeFileInjectTabMainTablePanel.Controls.Add(selectedCodeFileViewTable, 0, 1);
            codeFileInjectTabMainTablePanel.Controls.Add(codeFileInjectHeaderTable, 0, 0);
            codeFileInjectTabMainTablePanel.Dock = DockStyle.Fill;
            codeFileInjectTabMainTablePanel.Location = new Point(0, 0);
            codeFileInjectTabMainTablePanel.Name = "codeFileInjectTabMainTablePanel";
            codeFileInjectTabMainTablePanel.Padding = new Padding(0, 0, 8, 0);
            codeFileInjectTabMainTablePanel.RowCount = 2;
            codeFileInjectTabMainTablePanel.RowStyles.Add(new RowStyle());
            codeFileInjectTabMainTablePanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            codeFileInjectTabMainTablePanel.Size = new Size(852, 663);
            codeFileInjectTabMainTablePanel.TabIndex = 1;
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
            codeFileInjectHeaderTable.Controls.Add(loadSelectedFileInjectPresetButton, 4, 0);
            codeFileInjectHeaderTable.Controls.Add(presetLabel, 1, 0);
            codeFileInjectHeaderTable.Controls.Add(addNewCodeFileInjectButton, 0, 0);
            codeFileInjectHeaderTable.Controls.Add(presetSelectComboBox, 2, 0);
            codeFileInjectHeaderTable.Controls.Add(saveCodeFileInjectPresetButton, 3, 0);
            codeFileInjectHeaderTable.Dock = DockStyle.Fill;
            codeFileInjectHeaderTable.Location = new Point(3, 3);
            codeFileInjectHeaderTable.Name = "codeFileInjectHeaderTable";
            codeFileInjectHeaderTable.RowCount = 1;
            codeFileInjectHeaderTable.RowStyles.Add(new RowStyle());
            codeFileInjectHeaderTable.Size = new Size(838, 29);
            codeFileInjectHeaderTable.TabIndex = 2;
            // 
            // loadSelectedFileInjectPresetButton
            // 
            loadSelectedFileInjectPresetButton.BackColor = SystemColors.MenuHighlight;
            loadSelectedFileInjectPresetButton.Cursor = Cursors.Hand;
            loadSelectedFileInjectPresetButton.FlatStyle = FlatStyle.Flat;
            loadSelectedFileInjectPresetButton.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            loadSelectedFileInjectPresetButton.ForeColor = Color.White;
            loadSelectedFileInjectPresetButton.Location = new Point(376, 3);
            loadSelectedFileInjectPresetButton.Margin = new Padding(5, 3, 0, 0);
            loadSelectedFileInjectPresetButton.Name = "loadSelectedFileInjectPresetButton";
            loadSelectedFileInjectPresetButton.Size = new Size(75, 23);
            loadSelectedFileInjectPresetButton.TabIndex = 6;
            loadSelectedFileInjectPresetButton.Text = "✈️ Load";
            loadSelectedFileInjectPresetButton.UseVisualStyleBackColor = false;
            loadSelectedFileInjectPresetButton.Visible = false;
            loadSelectedFileInjectPresetButton.Click += loadSelectedFileInjectPresetButton_Click;
            // 
            // presetLabel
            // 
            presetLabel.AutoSize = true;
            presetLabel.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            presetLabel.ForeColor = Color.White;
            presetLabel.Location = new Point(91, 5);
            presetLabel.Margin = new Padding(10, 5, 3, 0);
            presetLabel.Name = "presetLabel";
            presetLabel.Size = new Size(75, 17);
            presetLabel.TabIndex = 4;
            presetLabel.Text = "❄️ Presets";
            // 
            // addNewCodeFileInjectButton
            // 
            addNewCodeFileInjectButton.BackColor = Color.YellowGreen;
            addNewCodeFileInjectButton.ForeColor = SystemColors.ButtonFace;
            addNewCodeFileInjectButton.Location = new Point(3, 3);
            addNewCodeFileInjectButton.Name = "addNewCodeFileInjectButton";
            addNewCodeFileInjectButton.Size = new Size(75, 23);
            addNewCodeFileInjectButton.TabIndex = 1;
            addNewCodeFileInjectButton.Text = "➕ Add File";
            addNewCodeFileInjectButton.UseVisualStyleBackColor = false;
            addNewCodeFileInjectButton.Click += addNewCodeFileInjectButton_Click;
            // 
            // presetSelectComboBox
            // 
            presetSelectComboBox.FormattingEnabled = true;
            presetSelectComboBox.Location = new Point(172, 3);
            presetSelectComboBox.Name = "presetSelectComboBox";
            presetSelectComboBox.Size = new Size(121, 23);
            presetSelectComboBox.TabIndex = 2;
            // 
            // saveCodeFileInjectPresetButton
            // 
            saveCodeFileInjectPresetButton.BackColor = Color.LimeGreen;
            saveCodeFileInjectPresetButton.FlatStyle = FlatStyle.Flat;
            saveCodeFileInjectPresetButton.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            saveCodeFileInjectPresetButton.ForeColor = Color.White;
            saveCodeFileInjectPresetButton.Location = new Point(296, 3);
            saveCodeFileInjectPresetButton.Margin = new Padding(0, 3, 0, 0);
            saveCodeFileInjectPresetButton.Name = "saveCodeFileInjectPresetButton";
            saveCodeFileInjectPresetButton.Size = new Size(75, 23);
            saveCodeFileInjectPresetButton.TabIndex = 5;
            saveCodeFileInjectPresetButton.Text = "💾 Save";
            saveCodeFileInjectPresetButton.UseVisualStyleBackColor = false;
            saveCodeFileInjectPresetButton.Click += saveCodeFileInjectPresetButton_Click;
            // 
            // settingsPage
            // 
            settingsPage.Controls.Add(tableLayoutPanel4);
            settingsPage.Location = new Point(4, 24);
            settingsPage.Name = "settingsPage";
            settingsPage.Padding = new Padding(3);
            settingsPage.Size = new Size(852, 663);
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
            tableLayoutPanel4.Location = new Point(3, 3);
            tableLayoutPanel4.Name = "tableLayoutPanel4";
            tableLayoutPanel4.Padding = new Padding(3);
            tableLayoutPanel4.RowCount = 4;
            tableLayoutPanel4.RowStyles.Add(new RowStyle());
            tableLayoutPanel4.RowStyles.Add(new RowStyle());
            tableLayoutPanel4.RowStyles.Add(new RowStyle());
            tableLayoutPanel4.RowStyles.Add(new RowStyle());
            tableLayoutPanel4.Size = new Size(846, 657);
            tableLayoutPanel4.TabIndex = 1;
            // 
            // topPTextBox
            // 
            topPTextBox.Location = new Point(85, 35);
            topPTextBox.Name = "topPTextBox";
            topPTextBox.Size = new Size(100, 23);
            topPTextBox.TabIndex = 7;
            // 
            // topPLabel
            // 
            topPLabel.AutoSize = true;
            topPLabel.Location = new Point(6, 39);
            topPLabel.Margin = new Padding(3, 7, 3, 0);
            topPLabel.Name = "topPLabel";
            topPLabel.Size = new Size(36, 15);
            topPLabel.TabIndex = 5;
            topPLabel.Text = "Top P";
            // 
            // temperatureTextBox
            // 
            temperatureTextBox.Location = new Point(85, 6);
            temperatureTextBox.Name = "temperatureTextBox";
            temperatureTextBox.Size = new Size(100, 23);
            temperatureTextBox.TabIndex = 3;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(6, 10);
            label3.Margin = new Padding(3, 7, 3, 0);
            label3.Name = "label3";
            label3.Size = new Size(73, 15);
            label3.TabIndex = 1;
            label3.Text = "Temperature";
            // 
            // nerdStatsPage
            // 
            nerdStatsPage.Location = new Point(4, 24);
            nerdStatsPage.Name = "nerdStatsPage";
            nerdStatsPage.Size = new Size(852, 663);
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
            sidePanelTable.Name = "sidePanelTable";
            sidePanelTable.RowCount = 6;
            sidePanelTable.RowStyles.Add(new RowStyle());
            sidePanelTable.RowStyles.Add(new RowStyle());
            sidePanelTable.RowStyles.Add(new RowStyle());
            sidePanelTable.RowStyles.Add(new RowStyle());
            sidePanelTable.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            sidePanelTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            sidePanelTable.Size = new Size(216, 691);
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
            llmSelectorTable.Location = new Point(3, 3);
            llmSelectorTable.Name = "llmSelectorTable";
            llmSelectorTable.RowCount = 1;
            llmSelectorTable.RowStyles.Add(new RowStyle());
            llmSelectorTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            llmSelectorTable.Size = new Size(210, 29);
            llmSelectorTable.TabIndex = 7;
            // 
            // llmSelector
            // 
            llmSelector.Dock = DockStyle.Fill;
            llmSelector.FormattingEnabled = true;
            llmSelector.Location = new Point(83, 3);
            llmSelector.Name = "llmSelector";
            llmSelector.Size = new Size(124, 23);
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
            totalTokenUsageTable.Location = new Point(3, 38);
            totalTokenUsageTable.Name = "totalTokenUsageTable";
            totalTokenUsageTable.RowCount = 1;
            totalTokenUsageTable.RowStyles.Add(new RowStyle());
            totalTokenUsageTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            totalTokenUsageTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            totalTokenUsageTable.Size = new Size(210, 29);
            totalTokenUsageTable.TabIndex = 5;
            // 
            // finalChatTokenLimitLabel
            // 
            finalChatTokenLimitLabel.AutoSize = true;
            finalChatTokenLimitLabel.Dock = DockStyle.Fill;
            finalChatTokenLimitLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            finalChatTokenLimitLabel.ForeColor = Color.White;
            finalChatTokenLimitLabel.Location = new Point(3, 3);
            finalChatTokenLimitLabel.Margin = new Padding(3);
            finalChatTokenLimitLabel.Name = "finalChatTokenLimitLabel";
            finalChatTokenLimitLabel.Padding = new Padding(3);
            finalChatTokenLimitLabel.Size = new Size(74, 23);
            finalChatTokenLimitLabel.TabIndex = 18;
            finalChatTokenLimitLabel.Text = "🌡️ Context";
            // 
            // finalChatTokenUsageProgressBar
            // 
            finalChatTokenUsageProgressBar.BackColor = Color.White;
            finalChatTokenUsageProgressBar.DisplayText = "10K / 32K Tokens";
            finalChatTokenUsageProgressBar.Dock = DockStyle.Fill;
            finalChatTokenUsageProgressBar.Location = new Point(83, 3);
            finalChatTokenUsageProgressBar.MarqueeAnimationSpeed = 1000;
            finalChatTokenUsageProgressBar.Name = "finalChatTokenUsageProgressBar";
            finalChatTokenUsageProgressBar.ProgressBarColor = Color.LightGreen;
            finalChatTokenUsageProgressBar.Size = new Size(124, 23);
            finalChatTokenUsageProgressBar.Style = ProgressBarStyle.Continuous;
            finalChatTokenUsageProgressBar.TabIndex = 17;
            // 
            // pastUserPrompts
            // 
            pastUserPrompts.BackColor = Color.Gainsboro;
            pastUserPrompts.Dock = DockStyle.Fill;
            pastUserPrompts.FormattingEnabled = true;
            pastUserPrompts.ItemHeight = 15;
            pastUserPrompts.Location = new Point(3, 103);
            pastUserPrompts.Name = "pastUserPrompts";
            pastUserPrompts.Size = new Size(210, 565);
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
            // selectedCodeFileViewTable
            // 
            selectedCodeFileViewTable.AutoScroll = true;
            selectedCodeFileViewTable.AutoSize = true;
            selectedCodeFileViewTable.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            selectedCodeFileViewTable.ColumnCount = 1;
            selectedCodeFileViewTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            selectedCodeFileViewTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            selectedCodeFileViewTable.Dock = DockStyle.Fill;
            selectedCodeFileViewTable.Location = new Point(3, 38);
            selectedCodeFileViewTable.Name = "selectedCodeFileViewTable";
            selectedCodeFileViewTable.RowCount = 1;
            selectedCodeFileViewTable.RowStyles.Add(new RowStyle());
            selectedCodeFileViewTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            selectedCodeFileViewTable.Size = new Size(838, 622);
            selectedCodeFileViewTable.TabIndex = 4;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.DimGray;
            ClientSize = new Size(1080, 691);
            Controls.Add(splitContainer1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Form1";
            Text = "LLMCodes";
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
        private Button loadSelectedFileInjectPresetButton;
        private TableLayoutPanel selectedCodeFileViewTable;
    }
}
