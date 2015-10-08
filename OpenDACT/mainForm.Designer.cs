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
            consoleMain = new System.Windows.Forms.RichTextBox();
            consolePrinter = new System.Windows.Forms.RichTextBox();
            this.connectButton = new System.Windows.Forms.Button();
            this.disconnectButton = new System.Windows.Forms.Button();
            this.calibrateButton = new System.Windows.Forms.Button();
            baudRateCombo = new System.Windows.Forms.ComboBox();
            portsCombo = new System.Windows.Forms.ComboBox();
            comboBoxZMinimumValue = new System.Windows.Forms.ComboBox();
            textFSRPlateOffset = new System.Windows.Forms.TextBox();
            this.resetPrinter = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // consoleMain
            // 
            consoleMain.Location = new System.Drawing.Point(169, 71);
            consoleMain.Name = "consoleMain";
            consoleMain.Size = new System.Drawing.Size(321, 147);
            consoleMain.TabIndex = 0;
            consoleMain.Text = "";
            // 
            // consolePrinter
            // 
            consolePrinter.Location = new System.Drawing.Point(169, 225);
            consolePrinter.Name = "consolePrinter";
            consolePrinter.Size = new System.Drawing.Size(321, 96);
            consolePrinter.TabIndex = 1;
            consolePrinter.Text = "";
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
            baudRateCombo.FormattingEnabled = true;
            baudRateCombo.Location = new System.Drawing.Point(258, 14);
            baudRateCombo.Name = "baudRateCombo";
            baudRateCombo.Size = new System.Drawing.Size(121, 21);
            baudRateCombo.TabIndex = 5;
            // 
            // portsCombo
            // 
            portsCombo.FormattingEnabled = true;
            portsCombo.Location = new System.Drawing.Point(385, 15);
            portsCombo.Name = "portsCombo";
            portsCombo.Size = new System.Drawing.Size(121, 21);
            portsCombo.TabIndex = 6;
            // 
            // comboBoxZMinimumValue
            // 
            comboBoxZMinimumValue.FormattingEnabled = true;
            comboBoxZMinimumValue.Location = new System.Drawing.Point(39, 131);
            comboBoxZMinimumValue.Name = "comboBoxZMinimumValue";
            comboBoxZMinimumValue.Size = new System.Drawing.Size(121, 21);
            comboBoxZMinimumValue.TabIndex = 7;
            // 
            // textFSRPlateOffset
            // 
            textFSRPlateOffset.Location = new System.Drawing.Point(39, 178);
            textFSRPlateOffset.Name = "textFSRPlateOffset";
            textFSRPlateOffset.Size = new System.Drawing.Size(100, 20);
            textFSRPlateOffset.TabIndex = 8;
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
            // mainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(629, 491);
            this.Controls.Add(this.resetPrinter);
            this.Controls.Add(textFSRPlateOffset);
            this.Controls.Add(comboBoxZMinimumValue);
            this.Controls.Add(portsCombo);
            this.Controls.Add(baudRateCombo);
            this.Controls.Add(this.calibrateButton);
            this.Controls.Add(this.disconnectButton);
            this.Controls.Add(this.connectButton);
            this.Controls.Add(consolePrinter);
            this.Controls.Add(consoleMain);
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
        public static System.Windows.Forms.RichTextBox consoleMain;
        public static System.Windows.Forms.RichTextBox consolePrinter;
        public static System.Windows.Forms.ComboBox baudRateCombo;
        public static System.Windows.Forms.ComboBox portsCombo;
        public static System.Windows.Forms.ComboBox comboBoxZMinimumValue;
        public static System.Windows.Forms.TextBox textFSRPlateOffset;
    }
}