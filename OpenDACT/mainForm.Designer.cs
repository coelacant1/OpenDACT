namespace OpenDACT
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
            // mainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(629, 491);
            this.Controls.Add(this.consolePrinter);
            this.Controls.Add(this.consoleMain);
            this.Name = "mainForm";
            this.Text = "mainForm";
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.RichTextBox consoleMain;
        public System.Windows.Forms.RichTextBox consolePrinter;
    }
}