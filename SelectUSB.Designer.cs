namespace PADE
{
    partial class SelectUSB
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
            this.label1 = new System.Windows.Forms.Label();
            this.btnOPEN = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.btnABORT = new System.Windows.Forms.Button();
            this.chkEthernet = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(14, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(293, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "There are n devices found. You must select one.";
            // 
            // btnOPEN
            // 
            this.btnOPEN.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOPEN.Location = new System.Drawing.Point(12, 126);
            this.btnOPEN.Name = "btnOPEN";
            this.btnOPEN.Size = new System.Drawing.Size(141, 40);
            this.btnOPEN.TabIndex = 2;
            this.btnOPEN.Text = "OPEN";
            this.btnOPEN.UseVisualStyleBackColor = true;
            this.btnOPEN.Click += new System.EventHandler(this.btnOPEN_Click);
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(12, 38);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(294, 82);
            this.listBox1.TabIndex = 3;
            // 
            // btnABORT
            // 
            this.btnABORT.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnABORT.Location = new System.Drawing.Point(159, 126);
            this.btnABORT.Name = "btnABORT";
            this.btnABORT.Size = new System.Drawing.Size(148, 40);
            this.btnABORT.TabIndex = 5;
            this.btnABORT.Text = "ABORT";
            this.btnABORT.UseVisualStyleBackColor = true;
            this.btnABORT.Click += new System.EventHandler(this.btnABORT_Click);
            // 
            // chkEthernet
            // 
            this.chkEthernet.AutoSize = true;
            this.chkEthernet.Location = new System.Drawing.Point(12, 189);
            this.chkEthernet.Name = "chkEthernet";
            this.chkEthernet.Size = new System.Drawing.Size(157, 17);
            this.chkEthernet.TabIndex = 6;
            this.chkEthernet.Text = "USE ETHERNET for DATA";
            this.chkEthernet.UseVisualStyleBackColor = true;
            // 
            // SelectUSB
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(330, 226);
            this.ControlBox = false;
            this.Controls.Add(this.chkEthernet);
            this.Controls.Add(this.btnABORT);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.btnOPEN);
            this.Controls.Add(this.label1);
            this.Name = "SelectUSB";
            this.Text = "SelectUSB";
            this.Load += new System.EventHandler(this.SelectUSB_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Label label1;
        public System.Windows.Forms.Button btnOPEN;
        public System.Windows.Forms.ListBox listBox1;
        public System.Windows.Forms.Button btnABORT;
        private System.Windows.Forms.CheckBox chkEthernet;

    }
}