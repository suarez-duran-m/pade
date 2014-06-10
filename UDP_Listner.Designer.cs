namespace TB_namespace
{
    partial class UDP_Listner
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
            this.Worker_UDPreceive = new System.ComponentModel.BackgroundWorker();
            this.tbx_Port = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox = new System.Windows.Forms.TextBox();
            this.btn_LISTEN = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Worker_UDPreceive
            // 
            this.Worker_UDPreceive.WorkerReportsProgress = true;
            this.Worker_UDPreceive.WorkerSupportsCancellation = true;
            this.Worker_UDPreceive.DoWork += new System.ComponentModel.DoWorkEventHandler(this.Worker_UDPreceive_DoWork);
            this.Worker_UDPreceive.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.Worker_UDPreceive_ProgressChanged);
            this.Worker_UDPreceive.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.Worker_UDPreceive_RunWorkerCompleted);
            // 
            // tbx_Port
            // 
            this.tbx_Port.Location = new System.Drawing.Point(84, 6);
            this.tbx_Port.Name = "tbx_Port";
            this.tbx_Port.Size = new System.Drawing.Size(70, 20);
            this.tbx_Port.TabIndex = 0;
            this.tbx_Port.Text = "0x5311";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Listen Port";
            // 
            // textBox
            // 
            this.textBox.Location = new System.Drawing.Point(5, 49);
            this.textBox.Multiline = true;
            this.textBox.Name = "textBox";
            this.textBox.Size = new System.Drawing.Size(275, 558);
            this.textBox.TabIndex = 2;
            // 
            // btn_LISTEN
            // 
            this.btn_LISTEN.Location = new System.Drawing.Point(177, 5);
            this.btn_LISTEN.Name = "btn_LISTEN";
            this.btn_LISTEN.Size = new System.Drawing.Size(82, 20);
            this.btn_LISTEN.TabIndex = 3;
            this.btn_LISTEN.Text = "LISTEN";
            this.btn_LISTEN.UseVisualStyleBackColor = true;
            this.btn_LISTEN.Click += new System.EventHandler(this.btn_LISTEN_Click);
            // 
            // UDP_Listner
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 619);
            this.Controls.Add(this.btn_LISTEN);
            this.Controls.Add(this.textBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbx_Port);
            this.Name = "UDP_Listner";
            this.Text = "UDP_Listner";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.ComponentModel.BackgroundWorker Worker_UDPreceive;
        private System.Windows.Forms.TextBox tbx_Port;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox;
        private System.Windows.Forms.Button btn_LISTEN;
    }
}