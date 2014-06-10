namespace PADE
{
    partial class DRAM
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
            this.label2 = new System.Windows.Forms.Label();
            this.btnWRITE = new System.Windows.Forms.Button();
            this.txtStartingADDR = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtLen = new System.Windows.Forms.TextBox();
            this.btnREAD = new System.Windows.Forms.Button();
            this.lblCurrent = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.btn_MODE = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(8, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(152, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Starting Addr (27..0)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(207, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(142, 20);
            this.label2.TabIndex = 1;
            this.label2.Text = "Num words to read";
            // 
            // btnWRITE
            // 
            this.btnWRITE.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnWRITE.Location = new System.Drawing.Point(12, 531);
            this.btnWRITE.Name = "btnWRITE";
            this.btnWRITE.Size = new System.Drawing.Size(148, 89);
            this.btnWRITE.TabIndex = 2;
            this.btnWRITE.Text = "WRITE\r\nto DRAM\r\nfrom FILE";
            this.btnWRITE.UseVisualStyleBackColor = true;
            this.btnWRITE.Click += new System.EventHandler(this.btnWRITE_Click);
            // 
            // txtStartingADDR
            // 
            this.txtStartingADDR.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtStartingADDR.Location = new System.Drawing.Point(10, 67);
            this.txtStartingADDR.Name = "txtStartingADDR";
            this.txtStartingADDR.Size = new System.Drawing.Size(150, 22);
            this.txtStartingADDR.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(12, 39);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(107, 15);
            this.label3.TabIndex = 4;
            this.label3.Text = "max=268,435,455";
            // 
            // txtLen
            // 
            this.txtLen.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLen.Location = new System.Drawing.Point(211, 67);
            this.txtLen.Name = "txtLen";
            this.txtLen.Size = new System.Drawing.Size(150, 22);
            this.txtLen.TabIndex = 5;
            // 
            // btnREAD
            // 
            this.btnREAD.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnREAD.Location = new System.Drawing.Point(211, 531);
            this.btnREAD.Name = "btnREAD";
            this.btnREAD.Size = new System.Drawing.Size(150, 89);
            this.btnREAD.TabIndex = 6;
            this.btnREAD.Text = "READ\r\nfrom DRAM\r\nto FILE";
            this.btnREAD.UseVisualStyleBackColor = true;
            this.btnREAD.Click += new System.EventHandler(this.btnREAD_Click);
            // 
            // lblCurrent
            // 
            this.lblCurrent.AutoSize = true;
            this.lblCurrent.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrent.Location = new System.Drawing.Point(118, 227);
            this.lblCurrent.Name = "lblCurrent";
            this.lblCurrent.Size = new System.Drawing.Size(0, 24);
            this.lblCurrent.TabIndex = 7;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // btn_MODE
            // 
            this.btn_MODE.Location = new System.Drawing.Point(395, 67);
            this.btn_MODE.Name = "btn_MODE";
            this.btn_MODE.Size = new System.Drawing.Size(130, 22);
            this.btn_MODE.TabIndex = 8;
            this.btn_MODE.Text = "ACCESS mode";
            this.btn_MODE.UseVisualStyleBackColor = true;
            this.btn_MODE.Click += new System.EventHandler(this.btn_MODE_Click);
            // 
            // DRAM
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(671, 632);
            this.Controls.Add(this.btn_MODE);
            this.Controls.Add(this.lblCurrent);
            this.Controls.Add(this.btnREAD);
            this.Controls.Add(this.txtLen);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtStartingADDR);
            this.Controls.Add(this.btnWRITE);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "DRAM";
            this.Tag = "DRAM";
            this.Text = "DRAM";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DRAM_FormClosing);
            this.Load += new System.EventHandler(this.DRAM_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnWRITE;
        private System.Windows.Forms.TextBox txtStartingADDR;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtLen;
        private System.Windows.Forms.Button btnREAD;
        private System.Windows.Forms.Label lblCurrent;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Button btn_MODE;
    }
}