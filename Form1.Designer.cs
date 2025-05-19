namespace JankNubTools
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
            button2 = new Button();
            button1 = new Button();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            button3 = new Button();
            button4 = new Button();
            comboBox1 = new ComboBox();
            label6 = new Label();
            label7 = new Label();
            button5 = new Button();
            button6 = new Button();
            label8 = new Label();
            label9 = new Label();
            comboBox2 = new ComboBox();
            label10 = new Label();
            button7 = new Button();
            SuspendLayout();
            // 
            // button2
            // 
            button2.Location = new Point(373, 66);
            button2.Name = "button2";
            button2.Size = new Size(94, 29);
            button2.TabIndex = 1;
            button2.Text = "Browse";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button1
            // 
            button1.Location = new Point(106, 66);
            button1.Name = "button1";
            button1.Size = new Size(94, 29);
            button1.TabIndex = 0;
            button1.Text = "Browse";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 43);
            label1.Name = "label1";
            label1.Size = new Size(275, 20);
            label1.TabIndex = 2;
            label1.Text = "Select track that will be played one time";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(321, 43);
            label2.Name = "label2";
            label2.Size = new Size(211, 20);
            label2.TabIndex = 3;
            label2.Text = "Select track that wil be looped";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            label3.Location = new Point(30, 9);
            label3.Name = "label3";
            label3.Size = new Size(516, 32);
            label3.TabIndex = 4;
            label3.Text = "A NUB Generator by merging browsed files ";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(12, 99);
            label4.Name = "label4";
            label4.Size = new Size(178, 20);
            label4.TabIndex = 5;
            label4.Text = "Play Once File path: None";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(12, 119);
            label5.Name = "label5";
            label5.Size = new Size(178, 20);
            label5.TabIndex = 6;
            label5.Text = "Play Loop File path: None";
            // 
            // button3
            // 
            button3.Location = new Point(106, 188);
            button3.Name = "button3";
            button3.Size = new Size(94, 79);
            button3.TabIndex = 7;
            button3.Text = "Merge Generate";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // button4
            // 
            button4.Location = new Point(373, 188);
            button4.Name = "button4";
            button4.Size = new Size(94, 79);
            button4.TabIndex = 8;
            button4.Text = "Generate just the loop part";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // comboBox1
            // 
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox1.Items.AddRange(new object[] { "22050 Khz", "44100 Khz", "48000 Khz", "96000 Khz", "192000 Khz" });
            comboBox1.Location = new Point(141, 146);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(100, 28);
            comboBox1.TabIndex = 9;
            comboBox1.DropDown += comboBox1_SelectedIndexChanged;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(12, 149);
            label6.Name = "label6";
            label6.Size = new Size(123, 20);
            label6.TabIndex = 10;
            label6.Text = "Set playback rate";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            label7.Location = new Point(103, 273);
            label7.Name = "label7";
            label7.Size = new Size(361, 32);
            label7.TabIndex = 11;
            label7.Text = "Track Preview before generate";
            // 
            // button5
            // 
            button5.Location = new Point(235, 308);
            button5.Name = "button5";
            button5.Size = new Size(94, 29);
            button5.TabIndex = 12;
            button5.Text = "Listen";
            button5.UseVisualStyleBackColor = true;
            button5.Click += button5_Click;
            // 
            // button6
            // 
            button6.Location = new Point(233, 377);
            button6.Name = "button6";
            button6.Size = new Size(94, 29);
            button6.TabIndex = 14;
            button6.Text = "Listen";
            button6.UseVisualStyleBackColor = true;
            button6.Click += button6_Click;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            label8.Location = new Point(133, 339);
            label8.Name = "label8";
            label8.Size = new Size(307, 32);
            label8.TabIndex = 13;
            label8.Text = "Play generated NUB track";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(341, 149);
            label9.Name = "label9";
            label9.Size = new Size(83, 20);
            label9.TabIndex = 15;
            label9.Text = "Set volume";
            // 
            // comboBox2
            // 
            comboBox2.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox2.Items.AddRange(new object[] { "Low", "Normal", "High", "Ultra" });
            comboBox2.Location = new Point(430, 146);
            comboBox2.Name = "comboBox2";
            comboBox2.Size = new Size(102, 28);
            comboBox2.TabIndex = 16;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            label10.Location = new Point(132, 412);
            label10.Name = "label10";
            label10.Size = new Size(296, 32);
            label10.TabIndex = 17;
            label10.Text = "Change Volume function";
            // 
            // button7
            // 
            button7.Location = new Point(235, 450);
            button7.Name = "button7";
            button7.Size = new Size(94, 29);
            button7.TabIndex = 18;
            button7.Text = "Use";
            button7.UseVisualStyleBackColor = true;
            button7.Click += button7_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(562, 483);
            Controls.Add(button7);
            Controls.Add(label10);
            Controls.Add(comboBox2);
            Controls.Add(label9);
            Controls.Add(button6);
            Controls.Add(label8);
            Controls.Add(button5);
            Controls.Add(label7);
            Controls.Add(label6);
            Controls.Add(comboBox1);
            Controls.Add(button4);
            Controls.Add(button3);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(button2);
            Controls.Add(button1);
            MaximizeBox = false;
            MaximumSize = new Size(580, 530);
            MinimumSize = new Size(580, 530);
            Name = "Form1";
            Text = "Jank Nub Creator";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button2;
        private Button button1;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Button button3;
        private Button button4;
        private ComboBox comboBox1;
        private Label label6;
        private Label label7;
        private Button button5;
        private Button button6;
        private Label label8;
        private Label label9;
        private ComboBox comboBox2;
        private Label label10;
        private Button button7;
    }

    partial class PlaybackForm
    {
        private void InitializePlay()
        {
            this.playOnceButton = new System.Windows.Forms.Button();
            this.playLoopButton = new System.Windows.Forms.Button();
            this.stopButton = new System.Windows.Forms.Button();
            this.SuspendLayout();

            // Play Once Button
            this.playOnceButton.Location = new System.Drawing.Point(12, 12);
            this.playOnceButton.Name = "playOnceButton";
            this.playOnceButton.Size = new System.Drawing.Size(75, 23);
            this.playOnceButton.TabIndex = 0;
            this.playOnceButton.Text = "Play Once";
            this.playOnceButton.UseVisualStyleBackColor = true;
            this.playOnceButton.Click += new System.EventHandler(this.PlayOnceButton_Click);

            // Play Loop Button
            this.playLoopButton.Location = new System.Drawing.Point(93, 12);
            this.playLoopButton.Name = "playLoopButton";
            this.playLoopButton.Size = new System.Drawing.Size(75, 23);
            this.playLoopButton.TabIndex = 1;
            this.playLoopButton.Text = "Play Loop";
            this.playLoopButton.UseVisualStyleBackColor = true;
            this.playLoopButton.Click += new System.EventHandler(this.PlayLoopButton_Click);

            // Stop Button
            this.stopButton.Location = new System.Drawing.Point(174, 12);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(75, 23);
            this.stopButton.TabIndex = 2;
            this.stopButton.Text = "Stop";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.StopButton_Click);

            // PlaybackForm
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(261, 48);
            this.Controls.Add(this.stopButton);
            this.Controls.Add(this.playLoopButton);
            this.Controls.Add(this.playOnceButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PlaybackForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Audio Playback";
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Button playOnceButton;
        private System.Windows.Forms.Button playLoopButton;
        private System.Windows.Forms.Button stopButton;
    }

    partial class VolumeChangeForm
    {
        private void InitializeVolumeChange()
        {
            this.comboBox = new ComboBox();
            this.dataGridView1 = new DataGridView();
            this.label1 = new Label();
            this.button1 = new Button();
            this.SuspendLayout();

            // Label setup
            this.label1.AutoSize = true;
            this.label1.Name = "label1";
            this.label1.Text = "Set volume on all loaded files to";
            this.label1.Location = new Point(20, 20);
            this.label1.Size = new Size(150, 28);
            this.label1.TabIndex = 2;

            // ComboBox setup
            this.comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBox.Items.AddRange(new object[] { "Low", "Normal", "High", "Ultra" });
            this.comboBox.Location = new Point(250, 20);
            this.comboBox.Name = "comboBox";
            this.comboBox.Size = new Size(150, 28);
            this.comboBox.TabIndex = 0;

            // DataGridView setup
            this.dataGridView1.AutoSize = true;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridView1.AllowDrop = true;
            this.dataGridView1.Location = new Point(20, 50);
            this.dataGridView1.Size = new Size(360, 200);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ColumnCount = 2;
            this.dataGridView1.Columns[0].Name = "File Path";
            this.dataGridView1.Columns[1].Name = "Detected Volume";
            this.dataGridView1.Columns[1].Width = 150;
            this.dataGridView1.TabIndex = 1;
            this.dataGridView1.Rows.Add("Drop files here...", "");
            this.dataGridView1.ScrollBars = ScrollBars.Both;

            // Button Setup
            this.button1.Name = "button1";
            this.button1.Text = "Apply";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Location = new Point(150, 270);
            this.button1.Size = new Size(100, 28);
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom)));
            this.button1.Click += button1_Click;

            // Form setup
            this.ClientSize = new Size(400, 300);
            this.AutoSizeMode = AutoSizeMode.GrowOnly;
            this.Controls.Add(this.comboBox);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.MaximizeBox = true;
            this.MinimizeBox = true;
            this.AutoScroll = true;
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Volume Changer";
            this.ResumeLayout(false);
        }

        private ComboBox comboBox;
        private DataGridView dataGridView1;
        private Label label1;
        private Button button1;
    }
}
