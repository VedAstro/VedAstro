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
            splitContainer2 = new SplitContainer();
            chatMessageOutputBox = new RichTextBox();
            flowLayoutPanel2 = new FlowLayoutPanel();
            clearUserMsgButton = new Button();
            resetChatHistoryButton = new Button();
            sendUserMsgButton = new Button();
            progressBar1 = new ProgressBar();
            tabPage2 = new TabPage();
            largeCodeSnippetTextBox = new RichTextBox();
            flowLayoutPanel1 = new FlowLayoutPanel();
            llmSelector = new ComboBox();
            pastUserPrompts = new ListBox();
            label1 = new Label();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer2).BeginInit();
            splitContainer2.Panel1.SuspendLayout();
            splitContainer2.Panel2.SuspendLayout();
            splitContainer2.SuspendLayout();
            flowLayoutPanel2.SuspendLayout();
            tabPage2.SuspendLayout();
            flowLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(3, 29);
            label1.Name = "label1";
            label1.Size = new Size(92, 15);
            label1.TabIndex = 2;
            label1.Text = "📜 Past Prompts";
            // 
            // userInputTextBox
            // 
            userInputTextBox.BackColor = SystemColors.InfoText;
            userInputTextBox.Dock = DockStyle.Fill;
            userInputTextBox.ForeColor = SystemColors.MenuHighlight;
            userInputTextBox.Location = new Point(0, 0);
            userInputTextBox.Name = "userInputTextBox";
            userInputTextBox.Size = new Size(846, 93);
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
            splitContainer1.Panel2.Controls.Add(flowLayoutPanel1);
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
            tabPage1.Controls.Add(splitContainer2);
            tabPage1.Location = new Point(4, 24);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(852, 663);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "LLM";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // splitContainer2
            // 
            splitContainer2.Dock = DockStyle.Fill;
            splitContainer2.Location = new Point(3, 3);
            splitContainer2.Name = "splitContainer2";
            splitContainer2.Orientation = Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            splitContainer2.Panel1.Controls.Add(chatMessageOutputBox);
            // 
            // splitContainer2.Panel2
            // 
            splitContainer2.Panel2.Controls.Add(userInputTextBox);
            splitContainer2.Panel2.Controls.Add(flowLayoutPanel2);
            splitContainer2.Size = new Size(846, 657);
            splitContainer2.SplitterDistance = 529;
            splitContainer2.TabIndex = 2;
            // 
            // chatMessageOutputBox
            // 
            chatMessageOutputBox.BackColor = SystemColors.InfoText;
            chatMessageOutputBox.Dock = DockStyle.Fill;
            chatMessageOutputBox.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            chatMessageOutputBox.ForeColor = Color.LimeGreen;
            chatMessageOutputBox.Location = new Point(0, 0);
            chatMessageOutputBox.Name = "chatMessageOutputBox";
            chatMessageOutputBox.Size = new Size(846, 529);
            chatMessageOutputBox.TabIndex = 1;
            chatMessageOutputBox.Text = "";
            // 
            // flowLayoutPanel2
            // 
            flowLayoutPanel2.AutoSize = true;
            flowLayoutPanel2.Controls.Add(clearUserMsgButton);
            flowLayoutPanel2.Controls.Add(resetChatHistoryButton);
            flowLayoutPanel2.Controls.Add(sendUserMsgButton);
            flowLayoutPanel2.Controls.Add(progressBar1);
            flowLayoutPanel2.Dock = DockStyle.Bottom;
            flowLayoutPanel2.Location = new Point(0, 93);
            flowLayoutPanel2.Name = "flowLayoutPanel2";
            flowLayoutPanel2.Size = new Size(846, 31);
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
            sendUserMsgButton.Location = new Point(297, 3);
            sendUserMsgButton.Name = "sendUserMsgButton";
            sendUserMsgButton.Size = new Size(421, 25);
            sendUserMsgButton.TabIndex = 1;
            sendUserMsgButton.Text = "Send 🚀";
            sendUserMsgButton.UseVisualStyleBackColor = false;
            sendUserMsgButton.Click += sendUserMsgButton_Click;
            // 
            // progressBar1
            // 
            progressBar1.Location = new Point(724, 3);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(100, 23);
            progressBar1.Style = ProgressBarStyle.Continuous;
            progressBar1.TabIndex = 5;
            progressBar1.Visible = false;
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(largeCodeSnippetTextBox);
            tabPage2.Location = new Point(4, 24);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(852, 663);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "Code Inject";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // largeCodeSnippetTextBox
            // 
            largeCodeSnippetTextBox.BackColor = SystemColors.InfoText;
            largeCodeSnippetTextBox.Dock = DockStyle.Fill;
            largeCodeSnippetTextBox.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            largeCodeSnippetTextBox.ForeColor = SystemColors.MenuHighlight;
            largeCodeSnippetTextBox.Location = new Point(3, 3);
            largeCodeSnippetTextBox.Name = "largeCodeSnippetTextBox";
            largeCodeSnippetTextBox.Size = new Size(846, 657);
            largeCodeSnippetTextBox.TabIndex = 1;
            largeCodeSnippetTextBox.Text = "";
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            flowLayoutPanel1.AutoSize = true;
            flowLayoutPanel1.Controls.Add(llmSelector);
            flowLayoutPanel1.Controls.Add(label1);
            flowLayoutPanel1.Controls.Add(pastUserPrompts);
            flowLayoutPanel1.FlowDirection = FlowDirection.TopDown;
            flowLayoutPanel1.Location = new Point(0, 0);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(216, 688);
            flowLayoutPanel1.TabIndex = 4;
            // 
            // llmSelector
            // 
            llmSelector.FormattingEnabled = true;
            llmSelector.Location = new Point(3, 3);
            llmSelector.Name = "llmSelector";
            llmSelector.Size = new Size(210, 23);
            llmSelector.TabIndex = 0;
            llmSelector.SelectedIndexChanged += llmSelector_SelectedIndexChanged;
            // 
            // pastUserPrompts
            // 
            pastUserPrompts.FormattingEnabled = true;
            pastUserPrompts.ItemHeight = 15;
            pastUserPrompts.Location = new Point(3, 47);
            pastUserPrompts.Name = "pastUserPrompts";
            pastUserPrompts.Size = new Size(210, 634);
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
            splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            splitContainer2.Panel1.ResumeLayout(false);
            splitContainer2.Panel2.ResumeLayout(false);
            splitContainer2.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer2).EndInit();
            splitContainer2.ResumeLayout(false);
            flowLayoutPanel2.ResumeLayout(false);
            flowLayoutPanel2.PerformLayout();
            tabPage2.ResumeLayout(false);
            flowLayoutPanel1.ResumeLayout(false);
            flowLayoutPanel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private RichTextBox userInputTextBox;
        private SplitContainer splitContainer1;
        private Button sendUserMsgButton;
        private SplitContainer splitContainer2;
        private ListBox pastUserPrompts;
        private ComboBox llmSelector;
        private RichTextBox chatMessageOutputBox;
        private FlowLayoutPanel flowLayoutPanel1;
        private FlowLayoutPanel flowLayoutPanel2;
        private Button clearUserMsgButton;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private RichTextBox largeCodeSnippetTextBox;
        private Button resetChatHistoryButton;
        private ProgressBar progressBar1;
    }
}
