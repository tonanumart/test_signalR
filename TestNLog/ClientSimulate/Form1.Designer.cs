namespace ClientSimulate
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.connectingLabel = new System.Windows.Forms.Label();
            this.ReconnectingLabel = new System.Windows.Forms.Label();
            this.connectedLabel = new System.Windows.Forms.Label();
            this.disconnectedLabel = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.closeLabel = new System.Windows.Forms.Label();
            this.logTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.labelOpen = new System.Windows.Forms.Label();
            this.labelClose = new System.Windows.Forms.Label();
            this.labelCall = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(148, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Connect";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(22, 14);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(120, 20);
            this.numericUpDown1.TabIndex = 1;
            this.numericUpDown1.Value = new decimal(new int[] {
            350,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 104);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Connecting";
            // 
            // connectingLabel
            // 
            this.connectingLabel.AutoSize = true;
            this.connectingLabel.Location = new System.Drawing.Point(107, 104);
            this.connectingLabel.Name = "connectingLabel";
            this.connectingLabel.Size = new System.Drawing.Size(13, 13);
            this.connectingLabel.TabIndex = 4;
            this.connectingLabel.Text = "0";
            // 
            // ReconnectingLabel
            // 
            this.ReconnectingLabel.AutoSize = true;
            this.ReconnectingLabel.Location = new System.Drawing.Point(107, 133);
            this.ReconnectingLabel.Name = "ReconnectingLabel";
            this.ReconnectingLabel.Size = new System.Drawing.Size(13, 13);
            this.ReconnectingLabel.TabIndex = 5;
            this.ReconnectingLabel.Text = "0";
            // 
            // connectedLabel
            // 
            this.connectedLabel.AutoSize = true;
            this.connectedLabel.Location = new System.Drawing.Point(107, 156);
            this.connectedLabel.Name = "connectedLabel";
            this.connectedLabel.Size = new System.Drawing.Size(13, 13);
            this.connectedLabel.TabIndex = 6;
            this.connectedLabel.Text = "0";
            // 
            // disconnectedLabel
            // 
            this.disconnectedLabel.AutoSize = true;
            this.disconnectedLabel.Location = new System.Drawing.Point(107, 179);
            this.disconnectedLabel.Name = "disconnectedLabel";
            this.disconnectedLabel.Size = new System.Drawing.Size(13, 13);
            this.disconnectedLabel.TabIndex = 7;
            this.disconnectedLabel.Text = "0";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(19, 133);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(74, 13);
            this.label6.TabIndex = 8;
            this.label6.Text = "Reconnecting";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(19, 156);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(59, 13);
            this.label7.TabIndex = 9;
            this.label7.Text = "Connected";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(19, 179);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(61, 13);
            this.label8.TabIndex = 10;
            this.label8.Text = "Disconnect";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(19, 203);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(33, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "Close";
            // 
            // closeLabel
            // 
            this.closeLabel.AutoSize = true;
            this.closeLabel.Location = new System.Drawing.Point(107, 203);
            this.closeLabel.Name = "closeLabel";
            this.closeLabel.Size = new System.Drawing.Size(13, 13);
            this.closeLabel.TabIndex = 12;
            this.closeLabel.Text = "0";
            // 
            // logTextBox
            // 
            this.logTextBox.Location = new System.Drawing.Point(302, 12);
            this.logTextBox.Multiline = true;
            this.logTextBox.Name = "logTextBox";
            this.logTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.logTextBox.Size = new System.Drawing.Size(218, 238);
            this.logTextBox.TabIndex = 13;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(172, 104);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(23, 13);
            this.label3.TabIndex = 14;
            this.label3.Text = "MK";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(172, 133);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(33, 13);
            this.label4.TabIndex = 15;
            this.label4.Text = "Open";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(172, 156);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(33, 13);
            this.label5.TabIndex = 16;
            this.label5.Text = "Close";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(175, 179);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(24, 13);
            this.label9.TabIndex = 17;
            this.label9.Text = "Call";
            // 
            // labelOpen
            // 
            this.labelOpen.AutoSize = true;
            this.labelOpen.Location = new System.Drawing.Point(230, 132);
            this.labelOpen.Name = "labelOpen";
            this.labelOpen.Size = new System.Drawing.Size(13, 13);
            this.labelOpen.TabIndex = 18;
            this.labelOpen.Text = "0";
            // 
            // labelClose
            // 
            this.labelClose.AutoSize = true;
            this.labelClose.Location = new System.Drawing.Point(230, 156);
            this.labelClose.Name = "labelClose";
            this.labelClose.Size = new System.Drawing.Size(13, 13);
            this.labelClose.TabIndex = 19;
            this.labelClose.Text = "0";
            // 
            // labelCall
            // 
            this.labelCall.AutoSize = true;
            this.labelCall.Location = new System.Drawing.Point(230, 179);
            this.labelCall.Name = "labelCall";
            this.labelCall.Size = new System.Drawing.Size(13, 13);
            this.labelCall.TabIndex = 20;
            this.labelCall.Text = "0";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(532, 262);
            this.Controls.Add(this.labelCall);
            this.Controls.Add(this.labelClose);
            this.Controls.Add(this.labelOpen);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.logTextBox);
            this.Controls.Add(this.closeLabel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.disconnectedLabel);
            this.Controls.Add(this.connectedLabel);
            this.Controls.Add(this.ReconnectingLabel);
            this.Controls.Add(this.connectingLabel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label connectingLabel;
        private System.Windows.Forms.Label ReconnectingLabel;
        private System.Windows.Forms.Label connectedLabel;
        private System.Windows.Forms.Label disconnectedLabel;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label closeLabel;
        private System.Windows.Forms.TextBox logTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label labelOpen;
        private System.Windows.Forms.Label labelClose;
        private System.Windows.Forms.Label labelCall;
    }
}

