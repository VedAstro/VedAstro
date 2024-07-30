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
            userInputTextBox = new RichTextBox();
            splitContainer1 = new SplitContainer();
            splitContainer2 = new SplitContainer();
            chatMessageOutputBox = new RichTextBox();
            sendUserMsgButton = new Button();
            llmSelector = new ComboBox();
            pastUserPrompts = new ListBox();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer2).BeginInit();
            splitContainer2.Panel1.SuspendLayout();
            splitContainer2.Panel2.SuspendLayout();
            splitContainer2.SuspendLayout();
            SuspendLayout();
            // 
            // userInputTextBox
            // 
            userInputTextBox.BackColor = SystemColors.InfoText;
            userInputTextBox.Dock = DockStyle.Fill;
            userInputTextBox.ForeColor = SystemColors.MenuHighlight;
            userInputTextBox.Location = new Point(0, 0);
            userInputTextBox.Name = "userInputTextBox";
            userInputTextBox.Size = new Size(712, 289);
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
            splitContainer1.Panel1.Controls.Add(splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(llmSelector);
            splitContainer1.Panel2.Controls.Add(pastUserPrompts);
            splitContainer1.Size = new Size(894, 585);
            splitContainer1.SplitterDistance = 712;
            splitContainer1.TabIndex = 1;
            // 
            // splitContainer2
            // 
            splitContainer2.Dock = DockStyle.Fill;
            splitContainer2.Location = new Point(0, 0);
            splitContainer2.Name = "splitContainer2";
            splitContainer2.Orientation = Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            splitContainer2.Panel1.Controls.Add(chatMessageOutputBox);
            // 
            // splitContainer2.Panel2
            // 
            splitContainer2.Panel2.Controls.Add(sendUserMsgButton);
            splitContainer2.Panel2.Controls.Add(userInputTextBox);
            splitContainer2.Size = new Size(712, 585);
            splitContainer2.SplitterDistance = 292;
            splitContainer2.TabIndex = 2;
            // 
            // chatMessageOutputBox
            // 
            chatMessageOutputBox.BackColor = SystemColors.InfoText;
            chatMessageOutputBox.Dock = DockStyle.Fill;
            chatMessageOutputBox.ForeColor = SystemColors.MenuHighlight;
            chatMessageOutputBox.Location = new Point(0, 0);
            chatMessageOutputBox.Name = "chatMessageOutputBox";
            chatMessageOutputBox.Size = new Size(712, 292);
            chatMessageOutputBox.TabIndex = 1;
            chatMessageOutputBox.Text = "";
            // 
            // sendUserMsgButton
            // 
            sendUserMsgButton.BackColor = SystemColors.Highlight;
            sendUserMsgButton.Dock = DockStyle.Bottom;
            sendUserMsgButton.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            sendUserMsgButton.ForeColor = SystemColors.ButtonFace;
            sendUserMsgButton.Location = new Point(0, 266);
            sendUserMsgButton.Name = "sendUserMsgButton";
            sendUserMsgButton.Size = new Size(712, 23);
            sendUserMsgButton.TabIndex = 1;
            sendUserMsgButton.Text = "Send 🚀";
            sendUserMsgButton.UseVisualStyleBackColor = false;
            sendUserMsgButton.Click += sendUserMsgButton_Click;
            // 
            // llmSelector
            // 
            llmSelector.Dock = DockStyle.Fill;
            llmSelector.FormattingEnabled = true;
            llmSelector.Location = new Point(0, 0);
            llmSelector.Name = "llmSelector";
            llmSelector.Size = new Size(178, 23);
            llmSelector.TabIndex = 0;
            // 
            // pastUserPrompts
            // 
            pastUserPrompts.Dock = DockStyle.Fill;
            pastUserPrompts.FormattingEnabled = true;
            pastUserPrompts.ItemHeight = 15;
            pastUserPrompts.Location = new Point(0, 0);
            pastUserPrompts.Name = "pastUserPrompts";
            pastUserPrompts.Size = new Size(178, 585);
            pastUserPrompts.TabIndex = 1;
            pastUserPrompts.SelectedIndexChanged += pastUserPrompts_SelectedIndexChanged;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(894, 585);
            Controls.Add(splitContainer1);
            Name = "Form1";
            Text = "🤖 LLMCoder";
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            splitContainer2.Panel1.ResumeLayout(false);
            splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer2).EndInit();
            splitContainer2.ResumeLayout(false);
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
    }
}
