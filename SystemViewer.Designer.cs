namespace PADE
{
    partial class SystemViewer
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
            this.boxSee1 = new BoxSee();
            this.SuspendLayout();
            // 
            // boxSee1
            // 
            this.boxSee1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.boxSee1.Location = new System.Drawing.Point(12, 12);
            this.boxSee1.Name = "boxSee1";
            this.boxSee1.Size = new System.Drawing.Size(645, 544);
            this.boxSee1.TabIndex = 2;
            // 
            // SystemViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(669, 568);
            this.Controls.Add(this.boxSee1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "SystemViewer";
            this.Text = "SystemViewer";
            this.Load += new System.EventHandler(this.SystemViewer_Load);
            this.ResumeLayout(false);

        }

       
        #endregion

        public BoxSee boxSee1;


    }
}