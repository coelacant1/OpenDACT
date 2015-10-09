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
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // consoleMain
            // 
            this.consoleMain.Location = new System.Drawing.Point(169, 71);
            this.consoleMain.Name = "consoleMain";
            this.consoleMain.Size = new System.Drawing.Size(321, 147);
            this.consoleMain.TabIndex = 0;
            this.consoleMain.Text = "";
            // 
            // consolePrinter
            // 
            this.consolePrinter.Location = new System.Drawing.Point(169, 225);
            this.consolePrinter.Name = "consolePrinter";
            this.consolePrinter.Size = new System.Drawing.Size(321, 96);
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
            this.baudRateCombo.Location = new System.Drawing.Point(258, 14);
            this.baudRateCombo.Name = "baudRateCombo";
            this.baudRateCombo.Size = new System.Drawing.Size(121, 21);
            this.baudRateCombo.TabIndex = 5;
            // 
            // portsCombo
            // 
            this.portsCombo.FormattingEnabled = true;
            this.portsCombo.Location = new System.Drawing.Point(385, 15);
            this.portsCombo.Name = "portsCombo";
            this.portsCombo.Size = new System.Drawing.Size(121, 21);
            this.portsCombo.TabIndex = 6;
            // 
            // comboBoxZMinimumValue
            // 
            this.comboBoxZMinimumValue.FormattingEnabled = true;
            this.comboBoxZMinimumValue.Location = new System.Drawing.Point(39, 131);
            this.comboBoxZMinimumValue.Name = "comboBoxZMinimumValue";
            this.comboBoxZMinimumValue.Size = new System.Drawing.Size(121, 21);
            this.comboBoxZMinimumValue.TabIndex = 7;
            // 
            // textFSRPlateOffset
            // 
            this.textFSRPlateOffset.Location = new System.Drawing.Point(39, 178);
            this.textFSRPlateOffset.Name = "textFSRPlateOffset";
            this.textFSRPlateOffset.Size = new System.Drawing.Size(100, 20);
            this.textFSRPlateOffset.TabIndex = 8;
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
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(528, 160);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 10;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // mainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(629, 491);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.resetPrinter);
            this.Controls.Add(this.textFSRPlateOffset);
            this.Controls.Add(this.comboBoxZMinimumValue);
            this.Controls.Add(this.portsCombo);
            this.Controls.Add(this.baudRateCombo);
            this.Controls.Add(this.calibrateButton);
            this.Controls.Add(this.disconnectButton);
            this.Controls.Add(this.connectButton);
            this.Controls.Add(this.consolePrinter);
            this.Controls.Add(this.consoleMain);
            this.Name = "mainForm";
            this.Text = "mainForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button connectButton;
        private System.Windows.Forms.Button disconnectButton;
        private System.Windows.Forms.Button calibrateButton;
        private System.Windows.Forms.Button resetPrinter;
        private System.Windows.Forms.Button button1;
        public System.Windows.Forms.RichTextBox consoleMain;
        public System.Windows.Forms.RichTextBox consolePrinter;
        public System.Windows.Forms.ComboBox baudRateCombo;
        public System.Windows.Forms.ComboBox portsCombo;
        public System.Windows.Forms.ComboBox comboBoxZMinimumValue;
        public System.Windows.Forms.TextBox textFSRPlateOffset;
    }
}