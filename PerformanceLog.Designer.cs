namespace PADE
{
    partial class PerformanceLog
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
            
            this.perfView = new System.Windows.Forms.ListView();
            
            this.SuspendLayout();
            // 
            // perfView
            // 
            
            this.perfView.GridLines = true;
            
            this.perfView.Location = new System.Drawing.Point(9, 16);
            this.perfView.Name = "perfView";
            this.perfView.Size = new System.Drawing.Size(650, 206);
            this.perfView.TabIndex = 0;
            this.perfView.UseCompatibleStateImageBehavior = false;
            this.perfView.View = System.Windows.Forms.View.List;
            
            // PerformanceLog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(671, 244);
            this.Controls.Add(this.perfView);
            this.Name = "PerformanceLog";
            this.Text = "Performance Logger";
            this.Load += new System.EventHandler(this.PerformanceLog_Load);
            this.ResumeLayout(false);


            this.VisibleChanged += new System.EventHandler(PerformanceLog_VisibleChanged);

        }

        

        #endregion

        private System.Windows.Forms.ListView perfView;
        private System.Windows.Forms.ColumnHeader severityColumn;
        private System.Windows.Forms.ColumnHeader messageColumn;
        private System.Windows.Forms.ColumnHeader callingClassColumn;
        private System.Windows.Forms.ColumnHeader timeColumn;
    }
}