namespace PADE
{
  partial class Run0
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
        this.btnRUN = new System.Windows.Forms.Button();
        this.btnInit = new System.Windows.Forms.Button();
        this.btnStop = new System.Windows.Forms.Button();
        this.txtInitFile = new System.Windows.Forms.TextBox();
        this.txtRun = new System.Windows.Forms.TextBox();
        this.btnInitFileBrowse = new System.Windows.Forms.Button();
        this.btnRunFileBrowse = new System.Windows.Forms.Button();
        this.Worker = new System.ComponentModel.BackgroundWorker();
        this.progressBar1 = new System.Windows.Forms.ProgressBar();
        this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
        this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
        this.richTextBox1 = new System.Windows.Forms.RichTextBox();
        this.label1 = new System.Windows.Forms.Label();
        this.label2 = new System.Windows.Forms.Label();
        this.label3 = new System.Windows.Forms.Label();
        this.tbx_Status = new System.Windows.Forms.TextBox();
        this.tbx_No_Devices = new System.Windows.Forms.TextBox();
        this.btn_ListDevices = new System.Windows.Forms.Button();
        this.groupBox1 = new System.Windows.Forms.GroupBox();
        this.btn_Close = new System.Windows.Forms.Button();
        this.btn_Open = new System.Windows.Forms.Button();
        this.label4 = new System.Windows.Forms.Label();
        this.label5 = new System.Windows.Forms.Label();
        this.label6 = new System.Windows.Forms.Label();
        this.txtRunNum = new System.Windows.Forms.TextBox();
        this.txtMaxEvnts = new System.Windows.Forms.TextBox();
        this.txtMaxTime = new System.Windows.Forms.TextBox();
        this.menuStrip1 = new System.Windows.Forms.MenuStrip();
        this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
        this.lblEventNumAndTime = new System.Windows.Forms.Label();
        this.chk_ParamExtTrig = new System.Windows.Forms.CheckBox();
        this.chk_ParamZS = new System.Windows.Forms.CheckBox();
        this.chk_ParamSumOnly = new System.Windows.Forms.CheckBox();
        this.chk_SoftTrig = new System.Windows.Forms.CheckBox();
        this.groupBox2 = new System.Windows.Forms.GroupBox();
        this.label7 = new System.Windows.Forms.Label();
        this.btnEthClose = new System.Windows.Forms.Button();
        this.txtEthStatus = new System.Windows.Forms.TextBox();
        this.textBox2 = new System.Windows.Forms.TextBox();
        this.button2 = new System.Windows.Forms.Button();
        this.btnListEth = new System.Windows.Forms.Button();
        this.button1 = new System.Windows.Forms.Button();
        this.groupBox1.SuspendLayout();
        this.menuStrip1.SuspendLayout();
        this.groupBox2.SuspendLayout();
        this.SuspendLayout();
        // 
        // btnRUN
        // 
        this.btnRUN.BackColor = System.Drawing.Color.LimeGreen;
        this.btnRUN.Enabled = false;
        this.btnRUN.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.btnRUN.ForeColor = System.Drawing.SystemColors.WindowText;
        this.btnRUN.Location = new System.Drawing.Point(275, 308);
        this.btnRUN.Name = "btnRUN";
        this.btnRUN.Size = new System.Drawing.Size(117, 66);
        this.btnRUN.TabIndex = 0;
        this.btnRUN.Text = "RUN";
        this.btnRUN.UseVisualStyleBackColor = false;
        this.btnRUN.Click += new System.EventHandler(this.btnRUN_Click);
        // 
        // btnInit
        // 
        this.btnInit.BackColor = System.Drawing.Color.Yellow;
        this.btnInit.Enabled = false;
        this.btnInit.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.btnInit.ForeColor = System.Drawing.SystemColors.WindowText;
        this.btnInit.Location = new System.Drawing.Point(144, 308);
        this.btnInit.Name = "btnInit";
        this.btnInit.Size = new System.Drawing.Size(117, 66);
        this.btnInit.TabIndex = 1;
        this.btnInit.Text = "INIT";
        this.btnInit.UseVisualStyleBackColor = false;
        this.btnInit.Click += new System.EventHandler(this.btnInit_Click);
        // 
        // btnStop
        // 
        this.btnStop.BackColor = System.Drawing.Color.OrangeRed;
        this.btnStop.Enabled = false;
        this.btnStop.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.btnStop.ForeColor = System.Drawing.SystemColors.WindowText;
        this.btnStop.Location = new System.Drawing.Point(13, 308);
        this.btnStop.Name = "btnStop";
        this.btnStop.Size = new System.Drawing.Size(117, 66);
        this.btnStop.TabIndex = 2;
        this.btnStop.Text = "STOP";
        this.btnStop.UseVisualStyleBackColor = false;
        this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
        // 
        // txtInitFile
        // 
        this.txtInitFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.txtInitFile.Location = new System.Drawing.Point(12, 161);
        this.txtInitFile.Name = "txtInitFile";
        this.txtInitFile.Size = new System.Drawing.Size(305, 22);
        this.txtInitFile.TabIndex = 3;
        // 
        // txtRun
        // 
        this.txtRun.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.txtRun.Location = new System.Drawing.Point(13, 216);
        this.txtRun.Name = "txtRun";
        this.txtRun.Size = new System.Drawing.Size(248, 22);
        this.txtRun.TabIndex = 4;
        // 
        // btnInitFileBrowse
        // 
        this.btnInitFileBrowse.Location = new System.Drawing.Point(324, 161);
        this.btnInitFileBrowse.Name = "btnInitFileBrowse";
        this.btnInitFileBrowse.Size = new System.Drawing.Size(68, 22);
        this.btnInitFileBrowse.TabIndex = 5;
        this.btnInitFileBrowse.Text = "BROWSE";
        this.btnInitFileBrowse.UseVisualStyleBackColor = true;
        this.btnInitFileBrowse.Click += new System.EventHandler(this.btnInitFileBrowse_Click);
        // 
        // btnRunFileBrowse
        // 
        this.btnRunFileBrowse.Location = new System.Drawing.Point(267, 216);
        this.btnRunFileBrowse.Name = "btnRunFileBrowse";
        this.btnRunFileBrowse.Size = new System.Drawing.Size(68, 22);
        this.btnRunFileBrowse.TabIndex = 6;
        this.btnRunFileBrowse.Text = "BROWSE";
        this.btnRunFileBrowse.UseVisualStyleBackColor = true;
        this.btnRunFileBrowse.Click += new System.EventHandler(this.btnRunFileBrowse_Click);
        // 
        // Worker
        // 
        this.Worker.WorkerReportsProgress = true;
        this.Worker.WorkerSupportsCancellation = true;
        this.Worker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.Worker_DoWork);
        this.Worker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.Worker_ProgressChanged);
        this.Worker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.Worker_RunWorkerCompleted);
        // 
        // progressBar1
        // 
        this.progressBar1.Location = new System.Drawing.Point(12, 589);
        this.progressBar1.Name = "progressBar1";
        this.progressBar1.Size = new System.Drawing.Size(380, 20);
        this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
        this.progressBar1.TabIndex = 8;
        // 
        // openFileDialog1
        // 
        this.openFileDialog1.FileName = "openFileDialog1";
        // 
        // richTextBox1
        // 
        this.richTextBox1.Location = new System.Drawing.Point(12, 380);
        this.richTextBox1.Name = "richTextBox1";
        this.richTextBox1.Size = new System.Drawing.Size(380, 180);
        this.richTextBox1.TabIndex = 9;
        this.richTextBox1.Text = "";
        // 
        // label1
        // 
        this.label1.AutoSize = true;
        this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.label1.Location = new System.Drawing.Point(12, 193);
        this.label1.Name = "label1";
        this.label1.Size = new System.Drawing.Size(143, 20);
        this.label1.TabIndex = 10;
        this.label1.Text = "RUN DATA FILE";
        // 
        // label2
        // 
        this.label2.AutoSize = true;
        this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.label2.Location = new System.Drawing.Point(12, 134);
        this.label2.Name = "label2";
        this.label2.Size = new System.Drawing.Size(87, 20);
        this.label2.TabIndex = 11;
        this.label2.Text = "INIT FILE";
        // 
        // label3
        // 
        this.label3.Location = new System.Drawing.Point(260, 20);
        this.label3.Name = "label3";
        this.label3.Size = new System.Drawing.Size(47, 16);
        this.label3.TabIndex = 16;
        this.label3.Text = "Status";
        // 
        // tbx_Status
        // 
        this.tbx_Status.Location = new System.Drawing.Point(312, 17);
        this.tbx_Status.Name = "tbx_Status";
        this.tbx_Status.Size = new System.Drawing.Size(57, 20);
        this.tbx_Status.TabIndex = 15;
        // 
        // tbx_No_Devices
        // 
        this.tbx_No_Devices.Location = new System.Drawing.Point(89, 17);
        this.tbx_No_Devices.Name = "tbx_No_Devices";
        this.tbx_No_Devices.Size = new System.Drawing.Size(29, 20);
        this.tbx_No_Devices.TabIndex = 14;
        // 
        // btn_ListDevices
        // 
        this.btn_ListDevices.Location = new System.Drawing.Point(5, 17);
        this.btn_ListDevices.Name = "btn_ListDevices";
        this.btn_ListDevices.Size = new System.Drawing.Size(75, 20);
        this.btn_ListDevices.TabIndex = 13;
        this.btn_ListDevices.Text = "ListDevices";
        this.btn_ListDevices.Click += new System.EventHandler(this.btn_ListDevices_Click);
        // 
        // groupBox1
        // 
        this.groupBox1.Controls.Add(this.label3);
        this.groupBox1.Controls.Add(this.btn_Close);
        this.groupBox1.Controls.Add(this.tbx_Status);
        this.groupBox1.Controls.Add(this.tbx_No_Devices);
        this.groupBox1.Controls.Add(this.btn_Open);
        this.groupBox1.Controls.Add(this.btn_ListDevices);
        this.groupBox1.Location = new System.Drawing.Point(13, 82);
        this.groupBox1.Name = "groupBox1";
        this.groupBox1.Size = new System.Drawing.Size(379, 49);
        this.groupBox1.TabIndex = 17;
        this.groupBox1.TabStop = false;
        this.groupBox1.Text = "USB Interface";
        // 
        // btn_Close
        // 
        this.btn_Close.Enabled = false;
        this.btn_Close.Location = new System.Drawing.Point(206, 16);
        this.btn_Close.Name = "btn_Close";
        this.btn_Close.Size = new System.Drawing.Size(44, 20);
        this.btn_Close.TabIndex = 18;
        this.btn_Close.Text = "Close";
        this.btn_Close.Click += new System.EventHandler(this.btn_Close_Click);
        // 
        // btn_Open
        // 
        this.btn_Open.Location = new System.Drawing.Point(151, 16);
        this.btn_Open.Name = "btn_Open";
        this.btn_Open.Size = new System.Drawing.Size(49, 20);
        this.btn_Open.TabIndex = 17;
        this.btn_Open.Text = "Open";
        this.btn_Open.Click += new System.EventHandler(this.btn_Open_Click);
        // 
        // label4
        // 
        this.label4.AutoSize = true;
        this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.label4.Location = new System.Drawing.Point(13, 251);
        this.label4.Name = "label4";
        this.label4.Size = new System.Drawing.Size(83, 20);
        this.label4.TabIndex = 18;
        this.label4.Text = "Run Num";
        // 
        // label5
        // 
        this.label5.AutoSize = true;
        this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.label5.Location = new System.Drawing.Point(140, 251);
        this.label5.Name = "label5";
        this.label5.Size = new System.Drawing.Size(101, 20);
        this.label5.TabIndex = 19;
        this.label5.Text = "Max Events";
        // 
        // label6
        // 
        this.label6.AutoSize = true;
        this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.label6.Location = new System.Drawing.Point(271, 251);
        this.label6.Name = "label6";
        this.label6.Size = new System.Drawing.Size(84, 20);
        this.label6.TabIndex = 20;
        this.label6.Text = "Max Time";
        // 
        // txtRunNum
        // 
        this.txtRunNum.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.txtRunNum.Location = new System.Drawing.Point(12, 274);
        this.txtRunNum.Name = "txtRunNum";
        this.txtRunNum.Size = new System.Drawing.Size(118, 22);
        this.txtRunNum.TabIndex = 21;
        // 
        // txtMaxEvnts
        // 
        this.txtMaxEvnts.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.txtMaxEvnts.Location = new System.Drawing.Point(144, 274);
        this.txtMaxEvnts.Name = "txtMaxEvnts";
        this.txtMaxEvnts.Size = new System.Drawing.Size(117, 22);
        this.txtMaxEvnts.TabIndex = 22;
        // 
        // txtMaxTime
        // 
        this.txtMaxTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.txtMaxTime.Location = new System.Drawing.Point(275, 274);
        this.txtMaxTime.Name = "txtMaxTime";
        this.txtMaxTime.Size = new System.Drawing.Size(117, 22);
        this.txtMaxTime.TabIndex = 23;
        // 
        // menuStrip1
        // 
        this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1});
        this.menuStrip1.Location = new System.Drawing.Point(0, 0);
        this.menuStrip1.Name = "menuStrip1";
        this.menuStrip1.Padding = new System.Windows.Forms.Padding(4, 2, 0, 2);
        this.menuStrip1.Size = new System.Drawing.Size(404, 24);
        this.menuStrip1.TabIndex = 27;
        this.menuStrip1.Text = "menuStrip1";
        // 
        // toolStripMenuItem1
        // 
        this.toolStripMenuItem1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
        this.toolStripMenuItem1.Name = "toolStripMenuItem1";
        this.toolStripMenuItem1.Size = new System.Drawing.Size(58, 20);
        this.toolStripMenuItem1.Text = "ABOUT";
        this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
        // 
        // lblEventNumAndTime
        // 
        this.lblEventNumAndTime.BackColor = System.Drawing.SystemColors.ButtonHighlight;
        this.lblEventNumAndTime.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
        this.lblEventNumAndTime.Location = new System.Drawing.Point(12, 565);
        this.lblEventNumAndTime.Name = "lblEventNumAndTime";
        this.lblEventNumAndTime.Size = new System.Drawing.Size(378, 21);
        this.lblEventNumAndTime.TabIndex = 28;
        // 
        // chk_ParamExtTrig
        // 
        this.chk_ParamExtTrig.AutoSize = true;
        this.chk_ParamExtTrig.Location = new System.Drawing.Point(12, 616);
        this.chk_ParamExtTrig.Name = "chk_ParamExtTrig";
        this.chk_ParamExtTrig.Size = new System.Drawing.Size(85, 17);
        this.chk_ParamExtTrig.TabIndex = 29;
        this.chk_ParamExtTrig.Text = "External Trig";
        this.chk_ParamExtTrig.UseVisualStyleBackColor = true;
        this.chk_ParamExtTrig.CheckedChanged += new System.EventHandler(this.chk_ParamExtTrig_CheckedChanged);
        // 
        // chk_ParamZS
        // 
        this.chk_ParamZS.AutoSize = true;
        this.chk_ParamZS.Location = new System.Drawing.Point(103, 616);
        this.chk_ParamZS.Name = "chk_ParamZS";
        this.chk_ParamZS.Size = new System.Drawing.Size(95, 17);
        this.chk_ParamZS.TabIndex = 30;
        this.chk_ParamZS.Text = "Zero Suppress";
        this.chk_ParamZS.UseVisualStyleBackColor = true;
        this.chk_ParamZS.CheckedChanged += new System.EventHandler(this.chk_ParamZS_CheckedChanged);
        // 
        // chk_ParamSumOnly
        // 
        this.chk_ParamSumOnly.AutoSize = true;
        this.chk_ParamSumOnly.Location = new System.Drawing.Point(204, 616);
        this.chk_ParamSumOnly.Name = "chk_ParamSumOnly";
        this.chk_ParamSumOnly.Size = new System.Drawing.Size(49, 17);
        this.chk_ParamSumOnly.TabIndex = 31;
        this.chk_ParamSumOnly.Text = "UDP";
        this.chk_ParamSumOnly.UseVisualStyleBackColor = true;
        this.chk_ParamSumOnly.CheckedChanged += new System.EventHandler(this.chk_ParamSumOnly_CheckedChanged);
        // 
        // chk_SoftTrig
        // 
        this.chk_SoftTrig.AutoSize = true;
        this.chk_SoftTrig.Location = new System.Drawing.Point(281, 615);
        this.chk_SoftTrig.Name = "chk_SoftTrig";
        this.chk_SoftTrig.Size = new System.Drawing.Size(89, 17);
        this.chk_SoftTrig.TabIndex = 32;
        this.chk_SoftTrig.Text = "Software Trig";
        this.chk_SoftTrig.UseVisualStyleBackColor = true;
        this.chk_SoftTrig.CheckedChanged += new System.EventHandler(this.chk_SoftTrig_CheckedChanged);
        // 
        // groupBox2
        // 
        this.groupBox2.Controls.Add(this.label7);
        this.groupBox2.Controls.Add(this.btnEthClose);
        this.groupBox2.Controls.Add(this.txtEthStatus);
        this.groupBox2.Controls.Add(this.textBox2);
        this.groupBox2.Controls.Add(this.button2);
        this.groupBox2.Controls.Add(this.btnListEth);
        this.groupBox2.Location = new System.Drawing.Point(13, 27);
        this.groupBox2.Name = "groupBox2";
        this.groupBox2.Size = new System.Drawing.Size(379, 49);
        this.groupBox2.TabIndex = 20;
        this.groupBox2.TabStop = false;
        this.groupBox2.Text = "Ethernet Interface";
        // 
        // label7
        // 
        this.label7.Location = new System.Drawing.Point(260, 20);
        this.label7.Name = "label7";
        this.label7.Size = new System.Drawing.Size(47, 16);
        this.label7.TabIndex = 16;
        this.label7.Text = "Status";
        // 
        // btnEthClose
        // 
        this.btnEthClose.Location = new System.Drawing.Point(206, 16);
        this.btnEthClose.Name = "btnEthClose";
        this.btnEthClose.Size = new System.Drawing.Size(44, 20);
        this.btnEthClose.TabIndex = 18;
        this.btnEthClose.Text = "Close";
        this.btnEthClose.Click += new System.EventHandler(this.btnEthClose_Click);
        // 
        // txtEthStatus
        // 
        this.txtEthStatus.Location = new System.Drawing.Point(312, 17);
        this.txtEthStatus.Name = "txtEthStatus";
        this.txtEthStatus.Size = new System.Drawing.Size(57, 20);
        this.txtEthStatus.TabIndex = 15;
        // 
        // textBox2
        // 
        this.textBox2.Location = new System.Drawing.Point(89, 17);
        this.textBox2.Name = "textBox2";
        this.textBox2.Size = new System.Drawing.Size(29, 20);
        this.textBox2.TabIndex = 14;
        this.textBox2.Visible = false;
        // 
        // button2
        // 
        this.button2.Location = new System.Drawing.Point(151, 16);
        this.button2.Name = "button2";
        this.button2.Size = new System.Drawing.Size(49, 20);
        this.button2.TabIndex = 17;
        this.button2.Text = "Open";
        this.button2.Visible = false;
        // 
        // btnListEth
        // 
        this.btnListEth.Location = new System.Drawing.Point(5, 17);
        this.btnListEth.Name = "btnListEth";
        this.btnListEth.Size = new System.Drawing.Size(75, 20);
        this.btnListEth.TabIndex = 13;
        this.btnListEth.Text = "Choose";
        this.btnListEth.Click += new System.EventHandler(this.btnListEth_Click);
        // 
        // button1
        // 
        this.button1.Location = new System.Drawing.Point(336, 216);
        this.button1.Name = "button1";
        this.button1.Size = new System.Drawing.Size(56, 22);
        this.button1.TabIndex = 33;
        this.button1.Text = "OPEN";
        this.button1.UseVisualStyleBackColor = true;
        this.button1.Click += new System.EventHandler(this.button1_Click);
        // 
        // Run0
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.AutoScroll = true;
        this.ClientSize = new System.Drawing.Size(404, 642);
        this.Controls.Add(this.button1);
        this.Controls.Add(this.groupBox2);
        this.Controls.Add(this.chk_SoftTrig);
        this.Controls.Add(this.chk_ParamSumOnly);
        this.Controls.Add(this.chk_ParamZS);
        this.Controls.Add(this.chk_ParamExtTrig);
        this.Controls.Add(this.lblEventNumAndTime);
        this.Controls.Add(this.txtMaxTime);
        this.Controls.Add(this.txtMaxEvnts);
        this.Controls.Add(this.txtRunNum);
        this.Controls.Add(this.label6);
        this.Controls.Add(this.label5);
        this.Controls.Add(this.label4);
        this.Controls.Add(this.groupBox1);
        this.Controls.Add(this.label2);
        this.Controls.Add(this.label1);
        this.Controls.Add(this.richTextBox1);
        this.Controls.Add(this.progressBar1);
        this.Controls.Add(this.btnRunFileBrowse);
        this.Controls.Add(this.btnInitFileBrowse);
        this.Controls.Add(this.txtRun);
        this.Controls.Add(this.txtInitFile);
        this.Controls.Add(this.btnStop);
        this.Controls.Add(this.btnInit);
        this.Controls.Add(this.btnRUN);
        this.Controls.Add(this.menuStrip1);
        this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.MainMenuStrip = this.menuStrip1;
        this.MaximizeBox = false;
        this.Name = "Run0";
        this.Tag = "Run";
        this.Text = "Run0";
        this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Run0_FormClosing);
        this.Load += new System.EventHandler(this.Run0_Load);
        this.groupBox1.ResumeLayout(false);
        this.groupBox1.PerformLayout();
        this.menuStrip1.ResumeLayout(false);
        this.menuStrip1.PerformLayout();
        this.groupBox2.ResumeLayout(false);
        this.groupBox2.PerformLayout();
        this.ResumeLayout(false);
        this.PerformLayout();

    }

   

    

    #endregion

    private System.Windows.Forms.Button btnStop;
    private System.Windows.Forms.Button btnInitFileBrowse;
    private System.Windows.Forms.Button btnRunFileBrowse;
    private System.Windows.Forms.ProgressBar progressBar1;
    private System.Windows.Forms.OpenFileDialog openFileDialog1;
    private System.Windows.Forms.SaveFileDialog saveFileDialog1;
    private System.Windows.Forms.RichTextBox richTextBox1;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.TextBox tbx_Status;
    private System.Windows.Forms.TextBox tbx_No_Devices;
    private System.Windows.Forms.Button btn_ListDevices;
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.Button btn_Close;
    private System.Windows.Forms.Button btn_Open;
    public System.Windows.Forms.TextBox txtInitFile;
    public System.Windows.Forms.TextBox txtRun;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.Label label6;
    public System.Windows.Forms.TextBox txtRunNum;
    public System.Windows.Forms.TextBox txtMaxEvnts;
    public System.Windows.Forms.TextBox txtMaxTime;
    public System.ComponentModel.BackgroundWorker Worker;
    private System.Windows.Forms.MenuStrip menuStrip1;
    private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
    private System.Windows.Forms.Label lblEventNumAndTime;
    public System.Windows.Forms.CheckBox chk_ParamExtTrig;
    public System.Windows.Forms.CheckBox chk_ParamZS;
    public System.Windows.Forms.CheckBox chk_ParamSumOnly;
    public System.Windows.Forms.CheckBox chk_SoftTrig;
    private System.Windows.Forms.GroupBox groupBox2;
    private System.Windows.Forms.Label label7;
    private System.Windows.Forms.Button btnEthClose;
    private System.Windows.Forms.TextBox textBox2;
    private System.Windows.Forms.Button button2;
    private System.Windows.Forms.Button btnListEth;
    public System.Windows.Forms.TextBox txtEthStatus;
    public System.Windows.Forms.Button btnRUN;
    public System.Windows.Forms.Button btnInit;
    private System.Windows.Forms.Button button1;
  }
}