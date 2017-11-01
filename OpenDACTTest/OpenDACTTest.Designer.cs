namespace OpenDACTTest
{
    partial class OpenDACTTest
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
            this.bmpPictureTest = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.bmpPictureTest)).BeginInit();
            this.SuspendLayout();
            // 
            // bmpPictureTest
            // 
            this.bmpPictureTest.Location = new System.Drawing.Point(13, 13);
            this.bmpPictureTest.Name = "bmpPictureTest";
            this.bmpPictureTest.Size = new System.Drawing.Size(500, 500);
            this.bmpPictureTest.TabIndex = 0;
            this.bmpPictureTest.TabStop = false;
            // 
            // OpenDACTTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(524, 526);
            this.Controls.Add(this.bmpPictureTest);
            this.Name = "OpenDACTTest";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.bmpPictureTest)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.PictureBox bmpPictureTest;
    }
}

