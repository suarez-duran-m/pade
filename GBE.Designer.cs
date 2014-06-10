namespace PADE
{
    partial class GBE
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
            this.btn_MAC_TX = new System.Windows.Forms.Button();
            this.btn_MAC_RX = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.rtxt_TX_dec = new System.Windows.Forms.RichTextBox();
            this.txt_RXaddr = new System.Windows.Forms.TextBox();
            this.txt_TXaddr = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txt_TXlen = new System.Windows.Forms.TextBox();
            this.txt_RXlen = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.list_RX = new System.Windows.Forms.ListBox();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btn_MAC_TX
            // 
            this.btn_MAC_TX.Location = new System.Drawing.Point(490, 434);
            this.btn_MAC_TX.Name = "btn_MAC_TX";
            this.btn_MAC_TX.Size = new System.Drawing.Size(88, 20);
            this.btn_MAC_TX.TabIndex = 150;
            this.btn_MAC_TX.Text = "TX";
            this.btn_MAC_TX.UseVisualStyleBackColor = true;
            this.btn_MAC_TX.Click += new System.EventHandler(this.btn_MAC_TX_Click);
            // 
            // btn_MAC_RX
            // 
            this.btn_MAC_RX.Location = new System.Drawing.Point(389, 434);
            this.btn_MAC_RX.Name = "btn_MAC_RX";
            this.btn_MAC_RX.Size = new System.Drawing.Size(88, 20);
            this.btn_MAC_RX.TabIndex = 149;
            this.btn_MAC_RX.Text = "RX";
            this.btn_MAC_RX.UseVisualStyleBackColor = true;
            this.btn_MAC_RX.Click += new System.EventHandler(this.btn_MAC_RX_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // rtxt_TX_dec
            // 
            this.rtxt_TX_dec.Location = new System.Drawing.Point(490, 512);
            this.rtxt_TX_dec.Name = "rtxt_TX_dec";
            this.rtxt_TX_dec.Size = new System.Drawing.Size(85, 108);
            this.rtxt_TX_dec.TabIndex = 151;
            this.rtxt_TX_dec.Text = "";
            this.rtxt_TX_dec.TextChanged += new System.EventHandler(this.rtxt_TX_dec_TextChanged);
            // 
            // txt_RXaddr
            // 
            this.txt_RXaddr.Location = new System.Drawing.Point(389, 460);
            this.txt_RXaddr.Name = "txt_RXaddr";
            this.txt_RXaddr.Size = new System.Drawing.Size(86, 20);
            this.txt_RXaddr.TabIndex = 153;
            // 
            // txt_TXaddr
            // 
            this.txt_TXaddr.Location = new System.Drawing.Point(490, 460);
            this.txt_TXaddr.Name = "txt_TXaddr";
            this.txt_TXaddr.Size = new System.Drawing.Size(86, 20);
            this.txt_TXaddr.TabIndex = 154;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(329, 463);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 13);
            this.label1.TabIndex = 156;
            this.label1.Text = "AddOffset";
            // 
            // txt_TXlen
            // 
            this.txt_TXlen.Location = new System.Drawing.Point(490, 486);
            this.txt_TXlen.Name = "txt_TXlen";
            this.txt_TXlen.Size = new System.Drawing.Size(86, 20);
            this.txt_TXlen.TabIndex = 158;
            // 
            // txt_RXlen
            // 
            this.txt_RXlen.Location = new System.Drawing.Point(389, 486);
            this.txt_RXlen.Name = "txt_RXlen";
            this.txt_RXlen.Size = new System.Drawing.Size(86, 20);
            this.txt_RXlen.TabIndex = 157;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(329, 489);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 13);
            this.label2.TabIndex = 159;
            this.label2.Text = "Length";
            // 
            // list_RX
            // 
            this.list_RX.FormattingEnabled = true;
            this.list_RX.Location = new System.Drawing.Point(389, 512);
            this.list_RX.Name = "list_RX";
            this.list_RX.Size = new System.Drawing.Size(86, 108);
            this.list_RX.TabIndex = 160;
            this.list_RX.SelectedIndexChanged += new System.EventHandler(this.list_RX_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(27, 434);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(156, 20);
            this.label3.TabIndex = 161;
            this.label3.Text = "Send/Receive Offset";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(11, 7);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(83, 20);
            this.label4.TabIndex = 162;
            this.label4.Text = "Bit Setting";
            // 
            // GBE
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(671, 632);
            this.ControlBox = false;
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.list_RX);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txt_TXlen);
            this.Controls.Add(this.txt_RXlen);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txt_TXaddr);
            this.Controls.Add(this.txt_RXaddr);
            this.Controls.Add(this.rtxt_TX_dec);
            this.Controls.Add(this.btn_MAC_TX);
            this.Controls.Add(this.btn_MAC_RX);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "GBE";
            this.Tag = "GBE";
            this.Text = "GBE";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GBE_FormClosing);
            this.Load += new System.EventHandler(this.GBE_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

            
        }

        

        #endregion

        private System.Windows.Forms.Button btn_MAC_TX;
        private System.Windows.Forms.Button btn_MAC_RX;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.RichTextBox rtxt_TX_dec;
        private System.Windows.Forms.TextBox txt_RXaddr;
        private System.Windows.Forms.TextBox txt_TXaddr;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txt_TXlen;
        private System.Windows.Forms.TextBox txt_RXlen;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox list_RX;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;

    }
}