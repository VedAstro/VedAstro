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
            Label label1;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            splitContainer1 = new SplitContainer();
            mainTabControl = new TabControl();
            llmPage = new TabPage();
            tableLayoutPanel3 = new TableLayoutPanel();
            chatMessagePanel = new TableLayoutPanel();
            llmButtonRowHolderTable = new TableLayoutPanel();
            llmRunningTimerLabel = new Label();
            sendUserMsgButton = new Button();
            progressBar1 = new ProgressBar();
            resetChatHistoryButton = new Button();
            clearUserMsgButton = new Button();
            codePage = new TabPage();
            tableLayoutPanel1 = new TableLayoutPanel();
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
            tableLayoutPanel5 = new TableLayoutPanel();
            tokenLimitProgressBar = new CustomProgressBar();
            llmTokenLimitLabel = new Label();
            addNewCodeFileInjectButton = new Button();
            settingsPage = new TabPage();
            tableLayoutPanel4 = new TableLayoutPanel();
            includeCodeInjectCheckBox = new CheckBox();
            label5 = new Label();
            topPTextBox = new TextBox();
            topPLabel = new Label();
            temperatureTextBox = new TextBox();
            label3 = new Label();
            nerdStatsPage = new TabPage();
            tableLayoutPanel2 = new TableLayoutPanel();
            llmSelector = new ComboBox();
            pastUserPrompts = new ListBox();
            tabPage3 = new TabPage();
            tabPage4 = new TabPage();
            modalTokenLimitLabel = new Label();
            mainChatHeaderRow = new TableLayoutPanel();
            llmChatHistoryLabel = new Label();
            llmChatHistorySelectComboBox = new ComboBox();
            injectSourceLabel = new Label();
            snippetInjectCheckBox = new CheckBox();
            fileInjectCheckBox = new CheckBox();
            webInjectCheckBox = new CheckBox();
            finalChatTokenLimitLabel = new Label();
            finalChatTokenUsageProgressBar = new ProgressBar();
            userInputTextBox = new RichTextBox();
            label1 = new Label();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            mainTabControl.SuspendLayout();
            llmPage.SuspendLayout();
            tableLayoutPanel3.SuspendLayout();
            llmButtonRowHolderTable.SuspendLayout();
            codePage.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            flowLayoutPanel1.SuspendLayout();
            codeFilePage.SuspendLayout();
            codeFileInjectTabMainTablePanel.SuspendLayout();
            tableLayoutPanel5.SuspendLayout();
            settingsPage.SuspendLayout();
            tableLayoutPanel4.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            mainChatHeaderRow.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Dock = DockStyle.Fill;
            label1.Location = new Point(3, 29);
            label1.Name = "label1";
            label1.Size = new Size(210, 15);
            label1.TabIndex = 2;
            label1.Text = "📜 Past Prompts";
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
            splitContainer1.Panel2.Controls.Add(tableLayoutPanel2);
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
            llmPage.Controls.Add(tableLayoutPanel3);
            llmPage.Location = new Point(4, 24);
            llmPage.Name = "llmPage";
            llmPage.Padding = new Padding(3);
            llmPage.Size = new Size(852, 663);
            llmPage.TabIndex = 0;
            llmPage.Text = "🗨️ LLM";
            llmPage.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.ColumnCount = 1;
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel3.Controls.Add(mainChatHeaderRow, 0, 0);
            tableLayoutPanel3.Controls.Add(chatMessagePanel, 0, 1);
            tableLayoutPanel3.Controls.Add(userInputTextBox, 0, 2);
            tableLayoutPanel3.Controls.Add(llmButtonRowHolderTable, 0, 3);
            tableLayoutPanel3.Dock = DockStyle.Fill;
            tableLayoutPanel3.Location = new Point(3, 3);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 4;
            tableLayoutPanel3.RowStyles.Add(new RowStyle());
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 80F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle());
            tableLayoutPanel3.Size = new Size(846, 657);
            tableLayoutPanel3.TabIndex = 4;
            // 
            // chatMessagePanel
            // 
            chatMessagePanel.AutoScroll = true;
            chatMessagePanel.AutoSize = true;
            chatMessagePanel.BackColor = Color.Black;
            chatMessagePanel.ColumnCount = 1;
            chatMessagePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            chatMessagePanel.Dock = DockStyle.Fill;
            chatMessagePanel.Location = new Point(3, 38);
            chatMessagePanel.Name = "chatMessagePanel";
            chatMessagePanel.RowCount = 2;
            chatMessagePanel.RowStyles.Add(new RowStyle());
            chatMessagePanel.RowStyles.Add(new RowStyle());
            chatMessagePanel.Size = new Size(840, 462);
            chatMessagePanel.TabIndex = 7;
            // 
            // llmButtonRowHolderTable
            // 
            llmButtonRowHolderTable.AutoSize = true;
            llmButtonRowHolderTable.ColumnCount = 5;
            llmButtonRowHolderTable.ColumnStyles.Add(new ColumnStyle());
            llmButtonRowHolderTable.ColumnStyles.Add(new ColumnStyle());
            llmButtonRowHolderTable.ColumnStyles.Add(new ColumnStyle());
            llmButtonRowHolderTable.ColumnStyles.Add(new ColumnStyle());
            llmButtonRowHolderTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            llmButtonRowHolderTable.Controls.Add(llmRunningTimerLabel, 1, 0);
            llmButtonRowHolderTable.Controls.Add(sendUserMsgButton, 4, 0);
            llmButtonRowHolderTable.Controls.Add(progressBar1, 3, 0);
            llmButtonRowHolderTable.Controls.Add(resetChatHistoryButton, 2, 0);
            llmButtonRowHolderTable.Controls.Add(clearUserMsgButton, 0, 0);
            llmButtonRowHolderTable.Dock = DockStyle.Bottom;
            llmButtonRowHolderTable.Location = new Point(3, 623);
            llmButtonRowHolderTable.Name = "llmButtonRowHolderTable";
            llmButtonRowHolderTable.RowCount = 1;
            llmButtonRowHolderTable.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            llmButtonRowHolderTable.Size = new Size(840, 31);
            llmButtonRowHolderTable.TabIndex = 5;
            // 
            // llmRunningTimerLabel
            // 
            llmRunningTimerLabel.AutoSize = true;
            llmRunningTimerLabel.Location = new Point(83, 7);
            llmRunningTimerLabel.Margin = new Padding(3, 7, 3, 0);
            llmRunningTimerLabel.Name = "llmRunningTimerLabel";
            llmRunningTimerLabel.Size = new Size(21, 15);
            llmRunningTimerLabel.TabIndex = 7;
            llmRunningTimerLabel.Text = "0 s";
            // 
            // sendUserMsgButton
            // 
            sendUserMsgButton.AutoSize = true;
            sendUserMsgButton.BackColor = SystemColors.Highlight;
            sendUserMsgButton.Dock = DockStyle.Fill;
            sendUserMsgButton.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            sendUserMsgButton.ForeColor = SystemColors.ButtonFace;
            sendUserMsgButton.Location = new Point(296, 3);
            sendUserMsgButton.Name = "sendUserMsgButton";
            sendUserMsgButton.Size = new Size(541, 25);
            sendUserMsgButton.TabIndex = 1;
            sendUserMsgButton.Text = "Send 🚀";
            sendUserMsgButton.UseVisualStyleBackColor = false;
            sendUserMsgButton.Click += sendUserMsgButton_Click;
            // 
            // progressBar1
            // 
            progressBar1.Location = new Point(190, 3);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(100, 23);
            progressBar1.Style = ProgressBarStyle.Continuous;
            progressBar1.TabIndex = 5;
            // 
            // resetChatHistoryButton
            // 
            resetChatHistoryButton.AutoSize = true;
            resetChatHistoryButton.BackColor = Color.DeepPink;
            resetChatHistoryButton.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            resetChatHistoryButton.ForeColor = SystemColors.ButtonFace;
            resetChatHistoryButton.Location = new Point(110, 3);
            resetChatHistoryButton.Name = "resetChatHistoryButton";
            resetChatHistoryButton.Size = new Size(74, 25);
            resetChatHistoryButton.TabIndex = 4;
            resetChatHistoryButton.Text = "Reset 🔄️";
            resetChatHistoryButton.UseVisualStyleBackColor = false;
            resetChatHistoryButton.Click += resetChatHistoryButton_Click;
            // 
            // clearUserMsgButton
            // 
            clearUserMsgButton.AutoSize = true;
            clearUserMsgButton.BackColor = Color.IndianRed;
            clearUserMsgButton.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            clearUserMsgButton.ForeColor = SystemColors.ButtonFace;
            clearUserMsgButton.Location = new Point(3, 3);
            clearUserMsgButton.Name = "clearUserMsgButton";
            clearUserMsgButton.Size = new Size(74, 25);
            clearUserMsgButton.TabIndex = 3;
            clearUserMsgButton.Text = "Clear 🗑️";
            clearUserMsgButton.UseVisualStyleBackColor = false;
            clearUserMsgButton.Click += clearUserMsgButton_Click;
            // 
            // codePage
            // 
            codePage.Controls.Add(tableLayoutPanel1);
            codePage.Location = new Point(4, 24);
            codePage.Name = "codePage";
            codePage.Padding = new Padding(3);
            codePage.Size = new Size(852, 663);
            codePage.TabIndex = 3;
            codePage.Text = "🎞️ Code Inject";
            codePage.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.AutoSize = true;
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(label2, 0, 0);
            tableLayoutPanel1.Controls.Add(codeInjectPretextTextBox, 1, 0);
            tableLayoutPanel1.Controls.Add(flowLayoutPanel1, 0, 1);
            tableLayoutPanel1.Controls.Add(largeCodeSnippetTextBox, 1, 1);
            tableLayoutPanel1.Controls.Add(label4, 0, 2);
            tableLayoutPanel1.Controls.Add(injectAssitantPretextTextBox, 1, 2);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;
            tableLayoutPanel1.Location = new Point(3, 3);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.Size = new Size(846, 657);
            tableLayoutPanel1.TabIndex = 11;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Dock = DockStyle.Left;
            label2.Location = new Point(3, 3);
            label2.Margin = new Padding(3);
            label2.Name = "label2";
            label2.Padding = new Padding(3);
            label2.Size = new Size(81, 23);
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
            largeCodeSnippetTextBox.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            largeCodeSnippetTextBox.ForeColor = SystemColors.MenuHighlight;
            largeCodeSnippetTextBox.Location = new Point(104, 32);
            largeCodeSnippetTextBox.Name = "largeCodeSnippetTextBox";
            largeCodeSnippetTextBox.Size = new Size(739, 593);
            largeCodeSnippetTextBox.TabIndex = 1;
            largeCodeSnippetTextBox.Text = "";
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
            codeFilePage.Text = "📜 Code File Inject";
            codeFilePage.UseVisualStyleBackColor = true;
            // 
            // codeFileInjectTabMainTablePanel
            // 
            codeFileInjectTabMainTablePanel.AutoSize = true;
            codeFileInjectTabMainTablePanel.BackColor = Color.DarkGray;
            codeFileInjectTabMainTablePanel.ColumnCount = 1;
            codeFileInjectTabMainTablePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            codeFileInjectTabMainTablePanel.Controls.Add(tableLayoutPanel5, 0, 0);
            codeFileInjectTabMainTablePanel.Dock = DockStyle.Fill;
            codeFileInjectTabMainTablePanel.Location = new Point(0, 0);
            codeFileInjectTabMainTablePanel.Name = "codeFileInjectTabMainTablePanel";
            codeFileInjectTabMainTablePanel.RowCount = 2;
            codeFileInjectTabMainTablePanel.RowStyles.Add(new RowStyle());
            codeFileInjectTabMainTablePanel.RowStyles.Add(new RowStyle());
            codeFileInjectTabMainTablePanel.Size = new Size(852, 663);
            codeFileInjectTabMainTablePanel.TabIndex = 1;
            // 
            // tableLayoutPanel5
            // 
            tableLayoutPanel5.AutoSize = true;
            tableLayoutPanel5.BackColor = Color.DarkGray;
            tableLayoutPanel5.ColumnCount = 5;
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel5.Controls.Add(modalTokenLimitLabel, 4, 0);
            tableLayoutPanel5.Controls.Add(tokenLimitProgressBar, 3, 0);
            tableLayoutPanel5.Controls.Add(llmTokenLimitLabel, 2, 0);
            tableLayoutPanel5.Controls.Add(addNewCodeFileInjectButton, 0, 0);
            tableLayoutPanel5.Dock = DockStyle.Fill;
            tableLayoutPanel5.Location = new Point(3, 3);
            tableLayoutPanel5.Name = "tableLayoutPanel5";
            tableLayoutPanel5.RowCount = 1;
            tableLayoutPanel5.RowStyles.Add(new RowStyle());
            tableLayoutPanel5.Size = new Size(846, 29);
            tableLayoutPanel5.TabIndex = 2;
            // 
            // tokenLimitProgressBar
            // 
            tokenLimitProgressBar.DisplayText = "";
            tokenLimitProgressBar.Location = new Point(636, 3);
            tokenLimitProgressBar.MarqueeAnimationSpeed = 0;
            tokenLimitProgressBar.Name = "tokenLimitProgressBar";
            tokenLimitProgressBar.ProgressBarColor = Color.Green;
            tokenLimitProgressBar.Size = new Size(120, 23);
            tokenLimitProgressBar.Style = ProgressBarStyle.Continuous;
            tokenLimitProgressBar.TabIndex = 12;
            // 
            // llmTokenLimitLabel
            // 
            llmTokenLimitLabel.AutoSize = true;
            llmTokenLimitLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            llmTokenLimitLabel.ForeColor = SystemColors.ButtonFace;
            llmTokenLimitLabel.Location = new Point(536, 7);
            llmTokenLimitLabel.Margin = new Padding(3, 7, 3, 0);
            llmTokenLimitLabel.Name = "llmTokenLimitLabel";
            llmTokenLimitLabel.Size = new Size(94, 15);
            llmTokenLimitLabel.TabIndex = 10;
            llmTokenLimitLabel.Text = "🌡️ Token Usage";
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
            tableLayoutPanel4.Controls.Add(includeCodeInjectCheckBox, 1, 2);
            tableLayoutPanel4.Controls.Add(label5, 0, 2);
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
            // includeCodeInjectCheckBox
            // 
            includeCodeInjectCheckBox.AutoSize = true;
            includeCodeInjectCheckBox.Location = new Point(85, 68);
            includeCodeInjectCheckBox.Margin = new Padding(3, 7, 3, 3);
            includeCodeInjectCheckBox.Name = "includeCodeInjectCheckBox";
            includeCodeInjectCheckBox.Size = new Size(65, 19);
            includeCodeInjectCheckBox.TabIndex = 11;
            includeCodeInjectCheckBox.Text = "Include";
            includeCodeInjectCheckBox.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(6, 68);
            label5.Margin = new Padding(3, 7, 3, 0);
            label5.Name = "label5";
            label5.Size = new Size(67, 15);
            label5.TabIndex = 9;
            label5.Text = "Code Inject";
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
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 1;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.Controls.Add(llmSelector, 0, 0);
            tableLayoutPanel2.Controls.Add(label1, 0, 1);
            tableLayoutPanel2.Controls.Add(pastUserPrompts, 0, 2);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(0, 0);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 2;
            tableLayoutPanel2.RowStyles.Add(new RowStyle());
            tableLayoutPanel2.RowStyles.Add(new RowStyle());
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.Size = new Size(216, 691);
            tableLayoutPanel2.TabIndex = 4;
            // 
            // llmSelector
            // 
            llmSelector.Dock = DockStyle.Fill;
            llmSelector.FormattingEnabled = true;
            llmSelector.Location = new Point(3, 3);
            llmSelector.Name = "llmSelector";
            llmSelector.Size = new Size(210, 23);
            llmSelector.TabIndex = 0;
            llmSelector.SelectedIndexChanged += llmSelector_SelectedIndexChanged;
            // 
            // pastUserPrompts
            // 
            pastUserPrompts.Dock = DockStyle.Fill;
            pastUserPrompts.FormattingEnabled = true;
            pastUserPrompts.ItemHeight = 15;
            pastUserPrompts.Location = new Point(3, 47);
            pastUserPrompts.Name = "pastUserPrompts";
            pastUserPrompts.Size = new Size(210, 641);
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
            // modalTokenLimitLabel
            // 
            modalTokenLimitLabel.AutoSize = true;
            modalTokenLimitLabel.Font = new Font("Segoe UI Light", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            modalTokenLimitLabel.ForeColor = SystemColors.ControlLight;
            modalTokenLimitLabel.Location = new Point(762, 7);
            modalTokenLimitLabel.Margin = new Padding(3, 7, 3, 0);
            modalTokenLimitLabel.Name = "modalTokenLimitLabel";
            modalTokenLimitLabel.Size = new Size(81, 15);
            modalTokenLimitLabel.TabIndex = 13;
            modalTokenLimitLabel.Text = "@ 4096 Tokens";
            // 
            // mainChatHeaderRow
            // 
            mainChatHeaderRow.AutoSize = true;
            mainChatHeaderRow.ColumnCount = 9;
            mainChatHeaderRow.ColumnStyles.Add(new ColumnStyle());
            mainChatHeaderRow.ColumnStyles.Add(new ColumnStyle());
            mainChatHeaderRow.ColumnStyles.Add(new ColumnStyle());
            mainChatHeaderRow.ColumnStyles.Add(new ColumnStyle());
            mainChatHeaderRow.ColumnStyles.Add(new ColumnStyle());
            mainChatHeaderRow.ColumnStyles.Add(new ColumnStyle());
            mainChatHeaderRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            mainChatHeaderRow.ColumnStyles.Add(new ColumnStyle());
            mainChatHeaderRow.ColumnStyles.Add(new ColumnStyle());
            mainChatHeaderRow.Controls.Add(finalChatTokenUsageProgressBar, 8, 0);
            mainChatHeaderRow.Controls.Add(finalChatTokenLimitLabel, 7, 0);
            mainChatHeaderRow.Controls.Add(webInjectCheckBox, 5, 0);
            mainChatHeaderRow.Controls.Add(fileInjectCheckBox, 4, 0);
            mainChatHeaderRow.Controls.Add(snippetInjectCheckBox, 3, 0);
            mainChatHeaderRow.Controls.Add(injectSourceLabel, 2, 0);
            mainChatHeaderRow.Controls.Add(llmChatHistorySelectComboBox, 1, 0);
            mainChatHeaderRow.Controls.Add(llmChatHistoryLabel, 0, 0);
            mainChatHeaderRow.Dock = DockStyle.Fill;
            mainChatHeaderRow.Location = new Point(3, 3);
            mainChatHeaderRow.Name = "mainChatHeaderRow";
            mainChatHeaderRow.RowCount = 1;
            mainChatHeaderRow.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            mainChatHeaderRow.Size = new Size(840, 29);
            mainChatHeaderRow.TabIndex = 9;
            // 
            // llmChatHistoryLabel
            // 
            llmChatHistoryLabel.AutoSize = true;
            llmChatHistoryLabel.Dock = DockStyle.Fill;
            llmChatHistoryLabel.Location = new Point(3, 3);
            llmChatHistoryLabel.Margin = new Padding(3);
            llmChatHistoryLabel.Name = "llmChatHistoryLabel";
            llmChatHistoryLabel.Padding = new Padding(3);
            llmChatHistoryLabel.Size = new Size(66, 23);
            llmChatHistoryLabel.TabIndex = 6;
            llmChatHistoryLabel.Text = "📒 History";
            // 
            // llmChatHistorySelectComboBox
            // 
            llmChatHistorySelectComboBox.Dock = DockStyle.Fill;
            llmChatHistorySelectComboBox.FormattingEnabled = true;
            llmChatHistorySelectComboBox.Location = new Point(75, 3);
            llmChatHistorySelectComboBox.Name = "llmChatHistorySelectComboBox";
            llmChatHistorySelectComboBox.Size = new Size(200, 23);
            llmChatHistorySelectComboBox.TabIndex = 8;
            // 
            // injectSourceLabel
            // 
            injectSourceLabel.AutoSize = true;
            injectSourceLabel.Dock = DockStyle.Fill;
            injectSourceLabel.Location = new Point(281, 3);
            injectSourceLabel.Margin = new Padding(3);
            injectSourceLabel.Name = "injectSourceLabel";
            injectSourceLabel.Padding = new Padding(3);
            injectSourceLabel.Size = new Size(83, 23);
            injectSourceLabel.TabIndex = 9;
            injectSourceLabel.Text = "💉Inject from";
            // 
            // snippetInjectCheckBox
            // 
            snippetInjectCheckBox.AutoSize = true;
            snippetInjectCheckBox.Location = new Point(370, 6);
            snippetInjectCheckBox.Margin = new Padding(3, 6, 3, 3);
            snippetInjectCheckBox.Name = "snippetInjectCheckBox";
            snippetInjectCheckBox.Size = new Size(66, 19);
            snippetInjectCheckBox.TabIndex = 11;
            snippetInjectCheckBox.Text = "Snippet";
            snippetInjectCheckBox.UseVisualStyleBackColor = true;
            // 
            // fileInjectCheckBox
            // 
            fileInjectCheckBox.AutoSize = true;
            fileInjectCheckBox.Location = new Point(442, 6);
            fileInjectCheckBox.Margin = new Padding(3, 6, 3, 3);
            fileInjectCheckBox.Name = "fileInjectCheckBox";
            fileInjectCheckBox.Size = new Size(44, 19);
            fileInjectCheckBox.TabIndex = 12;
            fileInjectCheckBox.Text = "File";
            fileInjectCheckBox.UseVisualStyleBackColor = true;
            // 
            // webInjectCheckBox
            // 
            webInjectCheckBox.AutoSize = true;
            webInjectCheckBox.Location = new Point(492, 6);
            webInjectCheckBox.Margin = new Padding(3, 6, 3, 3);
            webInjectCheckBox.Name = "webInjectCheckBox";
            webInjectCheckBox.Size = new Size(50, 19);
            webInjectCheckBox.TabIndex = 13;
            webInjectCheckBox.Text = "Web";
            webInjectCheckBox.UseVisualStyleBackColor = true;
            // 
            // finalChatTokenLimitLabel
            // 
            finalChatTokenLimitLabel.AutoSize = true;
            finalChatTokenLimitLabel.Dock = DockStyle.Fill;
            finalChatTokenLimitLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            finalChatTokenLimitLabel.Location = new Point(631, 3);
            finalChatTokenLimitLabel.Margin = new Padding(3);
            finalChatTokenLimitLabel.Name = "finalChatTokenLimitLabel";
            finalChatTokenLimitLabel.Padding = new Padding(3);
            finalChatTokenLimitLabel.Size = new Size(100, 23);
            finalChatTokenLimitLabel.TabIndex = 14;
            finalChatTokenLimitLabel.Text = "🌡️ Token Usage";
            // 
            // finalChatTokenUsageProgressBar
            // 
            finalChatTokenUsageProgressBar.Location = new Point(737, 3);
            finalChatTokenUsageProgressBar.Name = "finalChatTokenUsageProgressBar";
            finalChatTokenUsageProgressBar.Size = new Size(100, 23);
            finalChatTokenUsageProgressBar.TabIndex = 16;
            // 
            // userInputTextBox
            // 
            userInputTextBox.BackColor = SystemColors.InfoText;
            userInputTextBox.Dock = DockStyle.Fill;
            userInputTextBox.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            userInputTextBox.ForeColor = SystemColors.MenuHighlight;
            userInputTextBox.Location = new Point(3, 506);
            userInputTextBox.Name = "userInputTextBox";
            userInputTextBox.Size = new Size(840, 111);
            userInputTextBox.TabIndex = 0;
            userInputTextBox.Text = "";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1080, 691);
            Controls.Add(splitContainer1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Form1";
            Text = "LLMCodes";
            Load += Form1_Load;
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            mainTabControl.ResumeLayout(false);
            llmPage.ResumeLayout(false);
            tableLayoutPanel3.ResumeLayout(false);
            tableLayoutPanel3.PerformLayout();
            llmButtonRowHolderTable.ResumeLayout(false);
            llmButtonRowHolderTable.PerformLayout();
            codePage.ResumeLayout(false);
            codePage.PerformLayout();
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            flowLayoutPanel1.ResumeLayout(false);
            flowLayoutPanel1.PerformLayout();
            codeFilePage.ResumeLayout(false);
            codeFilePage.PerformLayout();
            codeFileInjectTabMainTablePanel.ResumeLayout(false);
            codeFileInjectTabMainTablePanel.PerformLayout();
            tableLayoutPanel5.ResumeLayout(false);
            tableLayoutPanel5.PerformLayout();
            settingsPage.ResumeLayout(false);
            settingsPage.PerformLayout();
            tableLayoutPanel4.ResumeLayout(false);
            tableLayoutPanel4.PerformLayout();
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            mainChatHeaderRow.ResumeLayout(false);
            mainChatHeaderRow.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private SplitContainer splitContainer1;
        private Button sendUserMsgButton;
        private ListBox pastUserPrompts;
        private ComboBox llmSelector;
        private Button clearUserMsgButton;
        private TabControl mainTabControl;
        private TabPage llmPage;
        private Button resetChatHistoryButton;
        private ProgressBar progressBar1;
        private TableLayoutPanel tableLayoutPanel2;
        private TableLayoutPanel tableLayoutPanel3;
        private TabPage settingsPage;
        private TabPage tabPage3;
        private TabPage tabPage4;
        private TableLayoutPanel tableLayoutPanel4;
        private TabPage codePage;
        private TableLayoutPanel tableLayoutPanel1;
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
        private TableLayoutPanel chatMessagePanel;
        private CheckBox includeCodeInjectCheckBox;
        private Label label5;
        private Label llmRunningTimerLabel;
        private TabPage codeFilePage;
        private TableLayoutPanel codeFileInjectTabMainTablePanel;
        private Button addNewCodeFileInjectButton;
        private TableLayoutPanel tableLayoutPanel5;
        private CustomProgressBar tokenLimitProgressBar;
        private Label llmTokenLimitLabel;
        private TabPage nerdStatsPage;
        private Label modalTokenLimitLabel;
        private TableLayoutPanel mainChatHeaderRow;
        private ComboBox llmChatHistorySelectComboBox;
        private Label llmChatHistoryLabel;
        private Label injectSourceLabel;
        private CheckBox webInjectCheckBox;
        private CheckBox fileInjectCheckBox;
        private CheckBox snippetInjectCheckBox;
        private Label finalChatTokenLimitLabel;
        private ProgressBar finalChatTokenUsageProgressBar;
        private RichTextBox userInputTextBox;
    }
}
