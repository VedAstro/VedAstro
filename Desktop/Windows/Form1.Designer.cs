namespace Desktop_Windows
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
            splitContainer1 = new SplitContainer();
            pictureBox2 = new PictureBox();
            label1 = new Label();
            button3 = new Button();
            button2 = new Button();
            button1 = new Button();
            label2 = new Label();
            pictureBox1 = new PictureBox();
            serverOutput = new TextBox();
            label3 = new Label();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // splitContainer1
            // 
            splitContainer1.BorderStyle = BorderStyle.FixedSingle;
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 0);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(pictureBox2);
            splitContainer1.Panel1.Controls.Add(label1);
            splitContainer1.Panel1.Controls.Add(button3);
            splitContainer1.Panel1.Controls.Add(button2);
            splitContainer1.Panel1.Controls.Add(button1);
            splitContainer1.Panel1.Controls.Add(label2);
            splitContainer1.Panel1.Controls.Add(pictureBox1);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(serverOutput);
            splitContainer1.Panel2.Controls.Add(label3);
            splitContainer1.Size = new Size(808, 470);
            splitContainer1.SplitterDistance = 194;
            splitContainer1.TabIndex = 0;
            // 
            // pictureBox2
            // 
            pictureBox2.Image = Properties.Resources.ce_fcc_recycle;
            pictureBox2.Location = new Point(36, 418);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(120, 39);
            pictureBox2.TabIndex = 6;
            pictureBox2.TabStop = false;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(31, 172);
            label1.Name = "label1";
            label1.Size = new Size(131, 15);
            label1.TabIndex = 4;
            label1.Text = "Open ⁕ Powerful ⁕Easy";
            // 
            // button3
            // 
            button3.Location = new Point(31, 257);
            button3.Name = "button3";
            button3.Size = new Size(131, 23);
            button3.TabIndex = 3;
            button3.Text = "💌 Donate";
            button3.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            button2.Location = new Point(31, 228);
            button2.Name = "button2";
            button2.Size = new Size(131, 23);
            button2.TabIndex = 2;
            button2.Text = "☎️ Ask Help";
            button2.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            button1.Location = new Point(31, 199);
            button1.Name = "button1";
            button1.Size = new Size(131, 23);
            button1.TabIndex = 1;
            button1.Text = "🚀 Relaunch";
            button1.UseVisualStyleBackColor = true;
            button1.Click += relaunchButton_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = Color.White;
            label2.Cursor = Cursors.Hand;
            label2.FlatStyle = FlatStyle.Flat;
            label2.Font = new Font("Segoe UI Semilight", 14F);
            label2.Location = new Point(57, 113);
            label2.Margin = new Padding(0);
            label2.Name = "label2";
            label2.Size = new Size(80, 25);
            label2.TabIndex = 5;
            label2.Text = "Desktop";
            // 
            // pictureBox1
            // 
            pictureBox1.Image = Properties.Resources.vedastro_logo_for_appiconco;
            pictureBox1.Location = new Point(2, 0);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(189, 169);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // serverOutput
            // 
            serverOutput.BackColor = Color.Black;
            serverOutput.Dock = DockStyle.Fill;
            serverOutput.Font = new Font("Consolas", 10F);
            serverOutput.ForeColor = Color.White;
            serverOutput.Location = new Point(0, 0);
            serverOutput.Multiline = true;
            serverOutput.Name = "serverOutput";
            serverOutput.ReadOnly = true;
            serverOutput.ScrollBars = ScrollBars.Vertical;
            serverOutput.Size = new Size(608, 468);
            serverOutput.TabIndex = 2;
            serverOutput.WordWrap = false;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(3, 8);
            label3.Name = "label3";
            label3.Size = new Size(95, 15);
            label3.TabIndex = 1;
            label3.Text = "💻 Server Output";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(808, 470);
            Controls.Add(splitContainer1);
            Name = "Form1";
            Text = "VedAstro";
            FormClosing += Form1_FormClosing;
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel1.PerformLayout();
            splitContainer1.Panel2.ResumeLayout(false);
            splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private SplitContainer splitContainer1;
        private PictureBox pictureBox1;
        private Button button3;
        private Button button2;
        private Button button1;
        private Label label2;
        private Label label1;
        private Label label3;
        private PictureBox pictureBox2;
        private TextBox serverOutput;
    }
}
