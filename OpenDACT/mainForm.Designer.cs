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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
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
            this.GCodeBox = new System.Windows.Forms.TextBox();
            this.sendGCode = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.accuracyTime = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.printerLogPanel.SuspendLayout();
            this.advancedPanel.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.accuracyTime)).BeginInit();
            this.SuspendLayout();
            // 
            // consoleMain
            // 
            this.consoleMain.Location = new System.Drawing.Point(262, 121);
            this.consoleMain.Name = "consoleMain";
            this.consoleMain.Size = new System.Drawing.Size(354, 276);
            this.consoleMain.TabIndex = 0;
            this.consoleMain.Text = "";
            // 
            // consolePrinter
            // 
            this.consolePrinter.Location = new System.Drawing.Point(14, 31);
            this.consolePrinter.Name = "consolePrinter";
            this.consolePrinter.Size = new System.Drawing.Size(604, 186);
            this.consolePrinter.TabIndex = 1;
            this.consolePrinter.Text = "";
            // 
            // connectButton
            // 
            this.connectButton.Location = new System.Drawing.Point(12, 12);
            this.connectButton.Name = "connectButton";
            this.connectButton.Size = new System.Drawing.Size(75, 23);
            this.connectButton.TabIndex = 2;
            this.connectButton.Text = "Connect";
            this.connectButton.UseVisualStyleBackColor = true;
            this.connectButton.Click += new System.EventHandler(this.connectButton_Click);
            // 
            // disconnectButton
            // 
            this.disconnectButton.Location = new System.Drawing.Point(94, 12);
            this.disconnectButton.Name = "disconnectButton";
            this.disconnectButton.Size = new System.Drawing.Size(75, 23);
            this.disconnectButton.TabIndex = 3;
            this.disconnectButton.Text = "Disconnect";
            this.disconnectButton.UseVisualStyleBackColor = true;
            this.disconnectButton.Click += new System.EventHandler(this.disconnectButton_Click);
            // 
            // calibrateButton
            // 
            this.calibrateButton.Location = new System.Drawing.Point(175, 12);
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
            this.baudRateCombo.Location = new System.Drawing.Point(508, 12);
            this.baudRateCombo.Name = "baudRateCombo";
            this.baudRateCombo.Size = new System.Drawing.Size(108, 21);
            this.baudRateCombo.TabIndex = 5;
            // 
            // portsCombo
            // 
            this.portsCombo.FormattingEnabled = true;
            this.portsCombo.Location = new System.Drawing.Point(508, 39);
            this.portsCombo.Name = "portsCombo";
            this.portsCombo.Size = new System.Drawing.Size(108, 21);
            this.portsCombo.TabIndex = 6;
            // 
            // comboBoxZMinimumValue
            // 
            this.comboBoxZMinimumValue.FormattingEnabled = true;
            this.comboBoxZMinimumValue.Location = new System.Drawing.Point(343, 11);
            this.comboBoxZMinimumValue.Name = "comboBoxZMinimumValue";
            this.comboBoxZMinimumValue.Size = new System.Drawing.Size(100, 21);
            this.comboBoxZMinimumValue.TabIndex = 7;
            // 
            // textFSRPlateOffset
            // 
            this.textFSRPlateOffset.Location = new System.Drawing.Point(343, 40);
            this.textFSRPlateOffset.Name = "textFSRPlateOffset";
            this.textFSRPlateOffset.Size = new System.Drawing.Size(100, 20);
            this.textFSRPlateOffset.TabIndex = 8;
            this.textFSRPlateOffset.Text = "0.6";
            // 
            // resetPrinter
            // 
            this.resetPrinter.Location = new System.Drawing.Point(12, 42);
            this.resetPrinter.Name = "resetPrinter";
            this.resetPrinter.Size = new System.Drawing.Size(75, 23);
            this.resetPrinter.TabIndex = 9;
            this.resetPrinter.Text = "Reset";
            this.resetPrinter.UseVisualStyleBackColor = true;
            this.resetPrinter.Click += new System.EventHandler(this.resetPrinter_Click);
            // 
            // openAdvanced
            // 
            this.openAdvanced.Location = new System.Drawing.Point(175, 43);
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
            this.printerLogPanel.Size = new System.Drawing.Size(629, 221);
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
            this.label2.Location = new System.Drawing.Point(449, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "Baudrate:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(449, 42);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 13);
            this.label3.TabIndex = 13;
            this.label3.Text = "COM Port:";
            // 
            // advancedPanel
            // 
            this.advancedPanel.Controls.Add(this.tabControl1);
            this.advancedPanel.Location = new System.Drawing.Point(623, 12);
            this.advancedPanel.Name = "advancedPanel";
            this.advancedPanel.Size = new System.Drawing.Size(652, 612);
            this.advancedPanel.TabIndex = 14;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(262, 15);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 13);
            this.label4.TabIndex = 15;
            this.label4.Text = "Probe Type:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(262, 43);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(62, 13);
            this.label5.TabIndex = 16;
            this.label5.Text = "FSR Offset:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(259, 105);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(48, 13);
            this.label6.TabIndex = 17;
            this.label6.Text = "Console:";
            // 
            // GCodeBox
            // 
            this.GCodeBox.Location = new System.Drawing.Point(343, 76);
            this.GCodeBox.Name = "GCodeBox";
            this.GCodeBox.Size = new System.Drawing.Size(273, 20);
            this.GCodeBox.TabIndex = 18;
            // 
            // sendGCode
            // 
            this.sendGCode.Location = new System.Drawing.Point(262, 74);
            this.sendGCode.Name = "sendGCode";
            this.sendGCode.Size = new System.Drawing.Size(75, 23);
            this.sendGCode.TabIndex = 19;
            this.sendGCode.Text = "Send GC";
            this.sendGCode.UseVisualStyleBackColor = true;
            this.sendGCode.Click += new System.EventHandler(this.sendGCode_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(3, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(646, 612);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(638, 586);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Settings";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(638, 586);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Delta Analysis";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.accuracyTime);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(638, 586);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Accuracy ";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // accuracyTime
            // 
            this.accuracyTime.BackColor = System.Drawing.Color.Transparent;
            chartArea1.BackHatchStyle = System.Windows.Forms.DataVisualization.Charting.ChartHatchStyle.Percent60;
            chartArea1.Name = "ChartArea1";
            this.accuracyTime.ChartAreas.Add(chartArea1);
            legend1.Enabled = false;
            legend1.Name = "Legend1";
            this.accuracyTime.Legends.Add(legend1);
            this.accuracyTime.Location = new System.Drawing.Point(3, 3);
            this.accuracyTime.Name = "accuracyTime";
            series1.BackImageTransparentColor = System.Drawing.Color.White;
            series1.BorderColor = System.Drawing.Color.Transparent;
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series1.IsXValueIndexed = true;
            series1.Legend = "Legend1";
            series1.Name = "Accuracy";
            this.accuracyTime.Series.Add(series1);
            this.accuracyTime.Size = new System.Drawing.Size(632, 580);
            this.accuracyTime.TabIndex = 0;
            this.accuracyTime.Text = "chart1";
            // 
            // mainForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(1287, 633);
            this.Controls.Add(this.sendGCode);
            this.Controls.Add(this.GCodeBox);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
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
            this.Controls.Add(this.advancedPanel);
            this.Controls.Add(this.printerLogPanel);
            this.Name = "mainForm";
            this.Text = "mainForm";
            this.printerLogPanel.ResumeLayout(false);
            this.printerLogPanel.PerformLayout();
            this.advancedPanel.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.accuracyTime)).EndInit();
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
        private System.Windows.Forms.TextBox GCodeBox;
        private System.Windows.Forms.Button sendGCode;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.DataVisualization.Charting.Chart accuracyTime;
    }
}