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
            userInputTextBox = new RichTextBox();
            splitContainer1 = new SplitContainer();
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            tableLayoutPanel3 = new TableLayoutPanel();
            chatMessageOutputBox = new RichTextBox();
            flowLayoutPanel2 = new FlowLayoutPanel();
            clearUserMsgButton = new Button();
            resetChatHistoryButton = new Button();
            sendUserMsgButton = new Button();
            progressBar1 = new ProgressBar();
            tabPage2 = new TabPage();
            tableLayoutPanel1 = new TableLayoutPanel();
            label2 = new Label();
            codeInjectPretextTextBox = new TextBox();
            flowLayoutPanel1 = new FlowLayoutPanel();
            codeInjectLabel = new Label();
            tokenCountLabel = new Label();
            largeCodeSnippetTextBox = new RichTextBox();
            label4 = new Label();
            injectAssitantPretextTextBox = new TextBox();
            tableLayoutPanel2 = new TableLayoutPanel();
            llmSelector = new ComboBox();
            pastUserPrompts = new ListBox();
            label1 = new Label();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            tableLayoutPanel3.SuspendLayout();
            flowLayoutPanel2.SuspendLayout();
            tabPage2.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            flowLayoutPanel1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
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
            // userInputTextBox
            // 
            userInputTextBox.BackColor = SystemColors.InfoText;
            userInputTextBox.Dock = DockStyle.Fill;
            userInputTextBox.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            userInputTextBox.ForeColor = SystemColors.MenuHighlight;
            userInputTextBox.Location = new Point(3, 453);
            userInputTextBox.Name = "userInputTextBox";
            userInputTextBox.Size = new Size(840, 163);
            userInputTextBox.TabIndex = 0;
            userInputTextBox.Text = "";
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 0);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(tabControl1);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(tableLayoutPanel2);
            splitContainer1.Size = new Size(1080, 691);
            splitContainer1.SplitterDistance = 860;
            splitContainer1.TabIndex = 1;
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.Location = new Point(0, 0);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(860, 691);
            tabControl1.TabIndex = 4;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(tableLayoutPanel3);
            tabPage1.Location = new Point(4, 24);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(852, 663);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "LLM";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.ColumnCount = 1;
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel3.Controls.Add(chatMessageOutputBox, 0, 0);
            tableLayoutPanel3.Controls.Add(userInputTextBox, 0, 1);
            tableLayoutPanel3.Controls.Add(flowLayoutPanel2, 0, 2);
            tableLayoutPanel3.Dock = DockStyle.Fill;
            tableLayoutPanel3.Location = new Point(3, 3);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 2;
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 72.63923F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 27.360775F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle());
            tableLayoutPanel3.Size = new Size(846, 657);
            tableLayoutPanel3.TabIndex = 4;
            // 
            // chatMessageOutputBox
            // 
            chatMessageOutputBox.BackColor = SystemColors.InfoText;
            chatMessageOutputBox.Dock = DockStyle.Fill;
            chatMessageOutputBox.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            chatMessageOutputBox.ForeColor = Color.LimeGreen;
            chatMessageOutputBox.Location = new Point(3, 3);
            chatMessageOutputBox.Name = "chatMessageOutputBox";
            chatMessageOutputBox.Size = new Size(840, 444);
            chatMessageOutputBox.TabIndex = 1;
            chatMessageOutputBox.Text = "";
            // 
            // flowLayoutPanel2
            // 
            flowLayoutPanel2.AutoSize = true;
            flowLayoutPanel2.Controls.Add(clearUserMsgButton);
            flowLayoutPanel2.Controls.Add(resetChatHistoryButton);
            flowLayoutPanel2.Controls.Add(progressBar1);
            flowLayoutPanel2.Controls.Add(sendUserMsgButton);
            flowLayoutPanel2.Dock = DockStyle.Bottom;
            flowLayoutPanel2.Location = new Point(3, 623);
            flowLayoutPanel2.Name = "flowLayoutPanel2";
            flowLayoutPanel2.Size = new Size(840, 31);
            flowLayoutPanel2.TabIndex = 3;
            // 
            // clearUserMsgButton
            // 
            clearUserMsgButton.AutoSize = true;
            clearUserMsgButton.BackColor = Color.IndianRed;
            clearUserMsgButton.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            clearUserMsgButton.ForeColor = SystemColors.ButtonFace;
            clearUserMsgButton.Location = new Point(3, 3);
            clearUserMsgButton.Name = "clearUserMsgButton";
            clearUserMsgButton.Size = new Size(141, 25);
            clearUserMsgButton.TabIndex = 3;
            clearUserMsgButton.Text = "Clear 🗑️";
            clearUserMsgButton.UseVisualStyleBackColor = false;
            clearUserMsgButton.Click += clearUserMsgButton_Click;
            // 
            // resetChatHistoryButton
            // 
            resetChatHistoryButton.AutoSize = true;
            resetChatHistoryButton.BackColor = Color.DeepPink;
            resetChatHistoryButton.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            resetChatHistoryButton.ForeColor = SystemColors.ButtonFace;
            resetChatHistoryButton.Location = new Point(150, 3);
            resetChatHistoryButton.Name = "resetChatHistoryButton";
            resetChatHistoryButton.Size = new Size(141, 25);
            resetChatHistoryButton.TabIndex = 4;
            resetChatHistoryButton.Text = "Reset 🔄️";
            resetChatHistoryButton.UseVisualStyleBackColor = false;
            resetChatHistoryButton.Click += resetChatHistoryButton_Click;
            // 
            // sendUserMsgButton
            // 
            sendUserMsgButton.AutoSize = true;
            sendUserMsgButton.BackColor = SystemColors.Highlight;
            sendUserMsgButton.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            sendUserMsgButton.ForeColor = SystemColors.ButtonFace;
            sendUserMsgButton.Location = new Point(403, 3);
            sendUserMsgButton.Name = "sendUserMsgButton";
            sendUserMsgButton.Size = new Size(421, 25);
            sendUserMsgButton.TabIndex = 1;
            sendUserMsgButton.Text = "Send 🚀";
            sendUserMsgButton.UseVisualStyleBackColor = false;
            sendUserMsgButton.Click += sendUserMsgButton_Click;
            // 
            // progressBar1
            // 
            progressBar1.Location = new Point(297, 3);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(100, 23);
            progressBar1.Style = ProgressBarStyle.Continuous;
            progressBar1.TabIndex = 5;
            progressBar1.Visible = false;
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(tableLayoutPanel1);
            tabPage2.Location = new Point(4, 24);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(852, 663);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "Code Inject";
            tabPage2.UseVisualStyleBackColor = true;
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
            label2.Text = "Code Pretext";
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
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1080, 691);
            Controls.Add(splitContainer1);
            Name = "Form1";
            Text = "🤖 LLMCoder";
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tableLayoutPanel3.ResumeLayout(false);
            tableLayoutPanel3.PerformLayout();
            flowLayoutPanel2.ResumeLayout(false);
            flowLayoutPanel2.PerformLayout();
            tabPage2.ResumeLayout(false);
            tabPage2.PerformLayout();
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            flowLayoutPanel1.ResumeLayout(false);
            flowLayoutPanel1.PerformLayout();
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private RichTextBox userInputTextBox;
        private SplitContainer splitContainer1;
        private Button sendUserMsgButton;
        private ListBox pastUserPrompts;
        private ComboBox llmSelector;
        private RichTextBox chatMessageOutputBox;
        private FlowLayoutPanel flowLayoutPanel2;
        private Button clearUserMsgButton;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private RichTextBox largeCodeSnippetTextBox;
        private Button resetChatHistoryButton;
        private ProgressBar progressBar1;
        private TextBox codeInjectPretextTextBox;
        private Label label2;
        private Label codeInjectLabel;
        private Label label4;
        private TextBox injectAssitantPretextTextBox;
        private Label tokenCountLabel;
        private TableLayoutPanel tableLayoutPanel1;
        private FlowLayoutPanel flowLayoutPanel1;
        private TableLayoutPanel tableLayoutPanel2;
        private TableLayoutPanel tableLayoutPanel3;
    }
}
