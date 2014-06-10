namespace PADE
{
    partial class Flash0
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
            this.label2 = new System.Windows.Forms.Label();
            this.txtParameterFile = new System.Windows.Forms.TextBox();
            this.btnExpert = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.txtByte = new System.Windows.Forms.TextBox();
            this.btnBURN_FIRMWARE = new System.Windows.Forms.Button();
            this.btnBURN_PARAMETERS = new System.Windows.Forms.Button();
            this.txtFirmwareFile = new System.Windows.Forms.TextBox();
            this.btnFLASH2SCREEN = new System.Windows.Forms.Button();
            this.radioHEXorDEC = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.btnSCREEN2FLASH = new System.Windows.Forms.Button();
            this.btnSaveFirmware2File = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtPageNum = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblByte = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(10, 2);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(111, 20);
            this.label2.TabIndex = 14;
            this.label2.Text = "FLASH FILE";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtParameterFile
            // 
            this.txtParameterFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtParameterFile.Location = new System.Drawing.Point(12, 50);
            this.txtParameterFile.Name = "txtParameterFile";
            this.txtParameterFile.Size = new System.Drawing.Size(349, 22);
            this.txtParameterFile.TabIndex = 12;
            // 
            // btnExpert
            // 
            this.btnExpert.Location = new System.Drawing.Point(69, 193);
            this.btnExpert.Name = "btnExpert";
            this.btnExpert.Size = new System.Drawing.Size(245, 22);
            this.btnExpert.TabIndex = 19;
            this.btnExpert.Text = "SHOW EXPERT CONTROLS";
            this.btnExpert.UseVisualStyleBackColor = true;
            this.btnExpert.Click += new System.EventHandler(this.btnExpert_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = "FLASH file|*.fls|All files|*.*";
            this.openFileDialog1.Title = "OPEN FLASH FILE";
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.Filter = "FLASH file|*.fls|All files|*.*";
            // 
            // txtByte
            // 
            this.txtByte.CausesValidation = false;
            this.txtByte.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtByte.Location = new System.Drawing.Point(11, 261);
            this.txtByte.Margin = new System.Windows.Forms.Padding(2);
            this.txtByte.Name = "txtByte";
            this.txtByte.Size = new System.Drawing.Size(46, 23);
            this.txtByte.TabIndex = 24;
            this.txtByte.Text = "0xFF";
            this.txtByte.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtByte.Visible = false;
            this.txtByte.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtByte_KeyUp);
            // 
            // btnBURN_FIRMWARE
            // 
            this.btnBURN_FIRMWARE.Location = new System.Drawing.Point(367, 96);
            this.btnBURN_FIRMWARE.Name = "btnBURN_FIRMWARE";
            this.btnBURN_FIRMWARE.Size = new System.Drawing.Size(95, 39);
            this.btnBURN_FIRMWARE.TabIndex = 30;
            this.btnBURN_FIRMWARE.Text = "BURN\r\nFIRMWARE";
            this.btnBURN_FIRMWARE.UseVisualStyleBackColor = true;
            this.btnBURN_FIRMWARE.Click += new System.EventHandler(this.btnBURN_FIRMWARE_Click);
            // 
            // btnBURN_PARAMETERS
            // 
            this.btnBURN_PARAMETERS.Location = new System.Drawing.Point(367, 50);
            this.btnBURN_PARAMETERS.Name = "btnBURN_PARAMETERS";
            this.btnBURN_PARAMETERS.Size = new System.Drawing.Size(95, 39);
            this.btnBURN_PARAMETERS.TabIndex = 31;
            this.btnBURN_PARAMETERS.Text = "BURN\r\nPARAMETERS";
            this.btnBURN_PARAMETERS.UseVisualStyleBackColor = true;
            this.btnBURN_PARAMETERS.Click += new System.EventHandler(this.btnBURN_PARAMETERS_Click);
            // 
            // txtFirmwareFile
            // 
            this.txtFirmwareFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFirmwareFile.Location = new System.Drawing.Point(11, 96);
            this.txtFirmwareFile.Name = "txtFirmwareFile";
            this.txtFirmwareFile.Size = new System.Drawing.Size(350, 22);
            this.txtFirmwareFile.TabIndex = 32;
            // 
            // btnFLASH2SCREEN
            // 
            this.btnFLASH2SCREEN.Location = new System.Drawing.Point(75, 52);
            this.btnFLASH2SCREEN.Name = "btnFLASH2SCREEN";
            this.btnFLASH2SCREEN.Size = new System.Drawing.Size(116, 44);
            this.btnFLASH2SCREEN.TabIndex = 33;
            this.btnFLASH2SCREEN.Text = "Parameter page\r\nFLASH 2 SCREEN";
            this.btnFLASH2SCREEN.UseVisualStyleBackColor = true;
            this.btnFLASH2SCREEN.Click += new System.EventHandler(this.btnFLASH2SCREEN_Click);
            // 
            // radioHEXorDEC
            // 
            this.radioHEXorDEC.AutoSize = true;
            this.radioHEXorDEC.Checked = true;
            this.radioHEXorDEC.Location = new System.Drawing.Point(83, 30);
            this.radioHEXorDEC.Margin = new System.Windows.Forms.Padding(2);
            this.radioHEXorDEC.Name = "radioHEXorDEC";
            this.radioHEXorDEC.Size = new System.Drawing.Size(47, 17);
            this.radioHEXorDEC.TabIndex = 34;
            this.radioHEXorDEC.TabStop = true;
            this.radioHEXorDEC.Text = "HEX";
            this.radioHEXorDEC.UseVisualStyleBackColor = true;
            this.radioHEXorDEC.CheckedChanged += new System.EventHandler(this.radioHEXorDEC_CheckedChanged);
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(130, 30);
            this.radioButton1.Margin = new System.Windows.Forms.Padding(2);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(47, 17);
            this.radioButton1.TabIndex = 35;
            this.radioButton1.Text = "DEC";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // btnSCREEN2FLASH
            // 
            this.btnSCREEN2FLASH.Location = new System.Drawing.Point(75, 102);
            this.btnSCREEN2FLASH.Name = "btnSCREEN2FLASH";
            this.btnSCREEN2FLASH.Size = new System.Drawing.Size(116, 44);
            this.btnSCREEN2FLASH.TabIndex = 36;
            this.btnSCREEN2FLASH.Text = "Parameter page\r\nSCREEN 2 FLASH";
            this.btnSCREEN2FLASH.UseVisualStyleBackColor = true;
            this.btnSCREEN2FLASH.Click += new System.EventHandler(this.btnSCREEN2FLASH_Click);
            // 
            // btnSaveFirmware2File
            // 
            this.btnSaveFirmware2File.Location = new System.Drawing.Point(75, 152);
            this.btnSaveFirmware2File.Name = "btnSaveFirmware2File";
            this.btnSaveFirmware2File.Size = new System.Drawing.Size(116, 57);
            this.btnSaveFirmware2File.TabIndex = 37;
            this.btnSaveFirmware2File.Text = "Save\r\nFIRMWARE\r\nto FILE";
            this.btnSaveFirmware2File.UseVisualStyleBackColor = true;
            this.btnSaveFirmware2File.Click += new System.EventHandler(this.btnSaveFirmware2File_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(543, 245);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 13);
            this.label1.TabIndex = 38;
            this.label1.Text = "0/1700";
            // 
            // txtPageNum
            // 
            this.txtPageNum.Location = new System.Drawing.Point(15, 52);
            this.txtPageNum.Margin = new System.Windows.Forms.Padding(2);
            this.txtPageNum.Name = "txtPageNum";
            this.txtPageNum.Size = new System.Drawing.Size(50, 20);
            this.txtPageNum.TabIndex = 44;
            this.txtPageNum.Text = "0xc00";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.txtFirmwareFile);
            this.panel1.Controls.Add(this.btnBURN_PARAMETERS);
            this.panel1.Controls.Add(this.btnBURN_FIRMWARE);
            this.panel1.Controls.Add(this.btnExpert);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.txtParameterFile);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(469, 216);
            this.panel1.TabIndex = 50;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.textBox1);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.txtPageNum);
            this.panel2.Controls.Add(this.btnSaveFirmware2File);
            this.panel2.Controls.Add(this.btnSCREEN2FLASH);
            this.panel2.Controls.Add(this.radioButton1);
            this.panel2.Controls.Add(this.radioHEXorDEC);
            this.panel2.Controls.Add(this.btnFLASH2SCREEN);
            this.panel2.Location = new System.Drawing.Point(468, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(202, 216);
            this.panel2.TabIndex = 51;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(10, 189);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(55, 20);
            this.textBox1.TabIndex = 46;
            this.textBox1.Text = "0xc00";
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 163);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(51, 13);
            this.label5.TabIndex = 45;
            this.label5.Text = "End Add:";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.Control;
            this.pictureBox1.Location = new System.Drawing.Point(40, 228);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(200, 30);
            this.pictureBox1.TabIndex = 52;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint);
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.SystemColors.Control;
            this.pictureBox2.Location = new System.Drawing.Point(260, 228);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(200, 30);
            this.pictureBox2.TabIndex = 53;
            this.pictureBox2.TabStop = false;
            this.pictureBox2.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox2_Paint);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(113, 238);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 16);
            this.label3.TabIndex = 54;
            this.label3.Text = "Address";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(344, 238);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(37, 16);
            this.label4.TabIndex = 55;
            this.label4.Text = "Data";
            // 
            // lblByte
            // 
            this.lblByte.AutoSize = true;
            this.lblByte.Location = new System.Drawing.Point(543, 267);
            this.lblByte.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblByte.Name = "lblByte";
            this.lblByte.Size = new System.Drawing.Size(36, 13);
            this.lblByte.TabIndex = 56;
            this.lblByte.Text = "0/512";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(620, 233);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(39, 34);
            this.button1.TabIndex = 57;
            this.button1.Text = "test";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(367, 141);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(95, 68);
            this.button2.TabIndex = 33;
            this.button2.Text = "BURN\r\nALL SELECTED\r\nPADEs";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // Flash0
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(671, 852);
            this.ControlBox = false;
            this.Controls.Add(this.button1);
            this.Controls.Add(this.lblByte);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtByte);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Flash0";
            this.Tag = "Flash";
            this.Text = "Flash";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Flash0_FormClosing);
            this.Load += new System.EventHandler(this.Flash0_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        

        
        #endregion

        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.TextBox txtParameterFile;
        private System.Windows.Forms.Button btnExpert;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.TextBox txtByte;
        private System.Windows.Forms.Button btnBURN_FIRMWARE;
        private System.Windows.Forms.Button btnBURN_PARAMETERS;
        public System.Windows.Forms.TextBox txtFirmwareFile;
        private System.Windows.Forms.Button btnFLASH2SCREEN;
        private System.Windows.Forms.RadioButton radioHEXorDEC;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.Button btnSCREEN2FLASH;
        private System.Windows.Forms.Button btnSaveFirmware2File;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtPageNum;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblByte;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}