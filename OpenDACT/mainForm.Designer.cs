namespace OpenDACT.Class_Files
{
    partial class mainForm
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
            this.consoleMain = new System.Windows.Forms.RichTextBox();
            this.consolePrinter = new System.Windows.Forms.RichTextBox();
            this.connectButton = new System.Windows.Forms.Button();
            this.disconnectButton = new System.Windows.Forms.Button();
            this.calibrateButton = new System.Windows.Forms.Button();
            this.baudRateCombo = new System.Windows.Forms.ComboBox();
            this.portsCombo = new System.Windows.Forms.ComboBox();
            this.comboBoxZMinimumValue = new System.Windows.Forms.ComboBox();
            this.textFSRPlateOffset = new System.Windows.Forms.TextBox();
            this.resetPrinter = new System.Windows.Forms.Button();
            this.openAdvanced = new System.Windows.Forms.Button();
            this.printerLogPanel = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.advancedPanel = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.printerLogPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // consoleMain
            // 
            this.consoleMain.Location = new System.Drawing.Point(12, 250);
            this.consoleMain.Name = "consoleMain";
            this.consoleMain.Size = new System.Drawing.Size(605, 147);
            this.consoleMain.TabIndex = 0;
            this.consoleMain.Text = "";
            // 
            // consolePrinter
            // 
            this.consolePrinter.Location = new System.Drawing.Point(14, 31);
            this.consolePrinter.Name = "consolePrinter";
            this.consolePrinter.Size = new System.Drawing.Size(604, 160);
            this.consolePrinter.TabIndex = 1;
            this.consolePrinter.Text = "";
            // 
            // connectButton
            // 
            this.connectButton.Location = new System.Drawing.Point(13, 13);
            this.connectButton.Name = "connectButton";
            this.connectButton.Size = new System.Drawing.Size(75, 23);
            this.connectButton.TabIndex = 2;
            this.connectButton.Text = "Connect";
            this.connectButton.UseVisualStyleBackColor = true;
            this.connectButton.Click += new System.EventHandler(this.connectButton_Click);
            // 
            // disconnectButton
            // 
            this.disconnectButton.Location = new System.Drawing.Point(95, 13);
            this.disconnectButton.Name = "disconnectButton";
            this.disconnectButton.Size = new System.Drawing.Size(75, 23);
            this.disconnectButton.TabIndex = 3;
            this.disconnectButton.Text = "Disconnect";
            this.disconnectButton.UseVisualStyleBackColor = true;
            this.disconnectButton.Click += new System.EventHandler(this.disconnectButton_Click);
            // 
            // calibrateButton
            // 
            this.calibrateButton.Location = new System.Drawing.Point(176, 13);
            this.calibrateButton.Name = "calibrateButton";
            this.calibrateButton.Size = new System.Drawing.Size(75, 23);
            this.calibrateButton.TabIndex = 4;
            this.calibrateButton.Text = "Calibrate";
            this.calibrateButton.UseVisualStyleBackColor = true;
            this.calibrateButton.Click += new System.EventHandler(this.calibrateButton_Click);
            // 
            // baudRateCombo
            // 
            this.baudRateCombo.FormattingEnabled = true;
            this.baudRateCombo.Location = new System.Drawing.Point(496, 13);
            this.baudRateCombo.Name = "baudRateCombo";
            this.baudRateCombo.Size = new System.Drawing.Size(121, 21);
            this.baudRateCombo.TabIndex = 5;
            // 
            // portsCombo
            // 
            this.portsCombo.FormattingEnabled = true;
            this.portsCombo.Location = new System.Drawing.Point(496, 40);
            this.portsCombo.Name = "portsCombo";
            this.portsCombo.Size = new System.Drawing.Size(121, 21);
            this.portsCombo.TabIndex = 6;
            // 
            // comboBoxZMinimumValue
            // 
            this.comboBoxZMinimumValue.FormattingEnabled = true;
            this.comboBoxZMinimumValue.Location = new System.Drawing.Point(331, 12);
            this.comboBoxZMinimumValue.Name = "comboBoxZMinimumValue";
            this.comboBoxZMinimumValue.Size = new System.Drawing.Size(100, 21);
            this.comboBoxZMinimumValue.TabIndex = 7;
            // 
            // textFSRPlateOffset
            // 
            this.textFSRPlateOffset.Location = new System.Drawing.Point(331, 41);
            this.textFSRPlateOffset.Name = "textFSRPlateOffset";
            this.textFSRPlateOffset.Size = new System.Drawing.Size(100, 20);
            this.textFSRPlateOffset.TabIndex = 8;
            this.textFSRPlateOffset.Text = "0.6";
            this.textFSRPlateOffset.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // resetPrinter
            // 
            this.resetPrinter.Location = new System.Drawing.Point(13, 43);
            this.resetPrinter.Name = "resetPrinter";
            this.resetPrinter.Size = new System.Drawing.Size(75, 23);
            this.resetPrinter.TabIndex = 9;
            this.resetPrinter.Text = "Reset";
            this.resetPrinter.UseVisualStyleBackColor = true;
            this.resetPrinter.Click += new System.EventHandler(this.resetPrinter_Click);
            // 
            // openAdvanced
            // 
            this.openAdvanced.Location = new System.Drawing.Point(176, 44);
            this.openAdvanced.Name = "openAdvanced";
            this.openAdvanced.Size = new System.Drawing.Size(75, 23);
            this.openAdvanced.TabIndex = 10;
            this.openAdvanced.Text = "Advanced";
            this.openAdvanced.UseVisualStyleBackColor = true;
            this.openAdvanced.Click += new System.EventHandler(this.openAdvanced_Click);
            // 
            // printerLogPanel
            // 
            this.printerLogPanel.Controls.Add(this.label1);
            this.printerLogPanel.Controls.Add(this.consolePrinter);
            this.printerLogPanel.Location = new System.Drawing.Point(-1, 403);
            this.printerLogPanel.Name = "printerLogPanel";
            this.printerLogPanel.Size = new System.Drawing.Size(618, 352);
            this.printerLogPanel.TabIndex = 11;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(81, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Printer Console:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(437, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "Baudrate:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(437, 43);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 13);
            this.label3.TabIndex = 13;
            this.label3.Text = "COM Port:";
            // 
            // advancedPanel
            // 
            this.advancedPanel.Location = new System.Drawing.Point(623, 12);
            this.advancedPanel.Name = "advancedPanel";
            this.advancedPanel.Size = new System.Drawing.Size(652, 727);
            this.advancedPanel.TabIndex = 14;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(260, 15);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 13);
            this.label4.TabIndex = 15;
            this.label4.Text = "Probe Type:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(260, 44);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(62, 13);
            this.label5.TabIndex = 16;
            this.label5.Text = "FSR Offset:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(16, 231);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(48, 13);
            this.label6.TabIndex = 17;
            this.label6.Text = "Console:";
            // 
            // mainForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.AutoScroll = true;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(1287, 751);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.advancedPanel);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.printerLogPanel);
            this.Controls.Add(this.openAdvanced);
            this.Controls.Add(this.resetPrinter);
            this.Controls.Add(this.textFSRPlateOffset);
            this.Controls.Add(this.comboBoxZMinimumValue);
            this.Controls.Add(this.portsCombo);
            this.Controls.Add(this.baudRateCombo);
            this.Controls.Add(this.calibrateButton);
            this.Controls.Add(this.disconnectButton);
            this.Controls.Add(this.connectButton);
            this.Controls.Add(this.consoleMain);
            this.Name = "mainForm";
            this.Text = "mainForm";
            this.printerLogPanel.ResumeLayout(false);
            this.printerLogPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button connectButton;
        private System.Windows.Forms.Button disconnectButton;
        private System.Windows.Forms.Button calibrateButton;
        private System.Windows.Forms.Button resetPrinter;
        private System.Windows.Forms.Button openAdvanced;
        public System.Windows.Forms.RichTextBox consoleMain;
        public System.Windows.Forms.RichTextBox consolePrinter;
        public System.Windows.Forms.ComboBox baudRateCombo;
        public System.Windows.Forms.ComboBox portsCombo;
        public System.Windows.Forms.ComboBox comboBoxZMinimumValue;
        public System.Windows.Forms.TextBox textFSRPlateOffset;
        private System.Windows.Forms.Panel printerLogPanel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel advancedPanel;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
    }
}