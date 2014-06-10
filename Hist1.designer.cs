namespace PADE
{
  partial class Hist1
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
        this.components = new System.ComponentModel.Container();
        this.zg1 = new ZedGraph.ZedGraphControl();
        this.btnUpdate = new System.Windows.Forms.Button();
        this.panel1 = new System.Windows.Forms.Panel();
        this.button1 = new System.Windows.Forms.Button();
        this.chkIntegral = new System.Windows.Forms.CheckBox();
        this.btnAutoScale = new System.Windows.Forms.Button();
        this.chkLogY = new System.Windows.Forms.CheckBox();
        this.btnScan = new System.Windows.Forms.Button();
        this.lblInc = new System.Windows.Forms.Label();
        this.udInc = new System.Windows.Forms.NumericUpDown();
        this.lblStop = new System.Windows.Forms.Label();
        this.udStop = new System.Windows.Forms.NumericUpDown();
        this.lblStart = new System.Windows.Forms.Label();
        this.udStart = new System.Windows.Forms.NumericUpDown();
        this.btnAutoPeds = new System.Windows.Forms.Button();
        this.panel2 = new System.Windows.Forms.Panel();
        this.panel3 = new System.Windows.Forms.Panel();
        this.btn_ZeroOffset = new System.Windows.Forms.Button();
        this.txtTOL = new System.Windows.Forms.TextBox();
        this.lblPE_step = new System.Windows.Forms.Label();
        this.chk_Tuner = new System.Windows.Forms.CheckBox();
        this.chk_PEfinder = new System.Windows.Forms.CheckBox();
        this.txtGoal = new System.Windows.Forms.TextBox();
        this.label7 = new System.Windows.Forms.Label();
        this.btn_OffsetW = new System.Windows.Forms.Button();
        this.txtOffset = new System.Windows.Forms.TextBox();
        this.label6 = new System.Windows.Forms.Label();
        this.btnSavePeds = new System.Windows.Forms.Button();
        this.btn_PedW = new System.Windows.Forms.Button();
        this.txtPed = new System.Windows.Forms.TextBox();
        this.lblPed = new System.Windows.Forms.Label();
        this.label5 = new System.Windows.Forms.Label();
        this.udChan = new System.Windows.Forms.NumericUpDown();
        this.panel4 = new System.Windows.Forms.Panel();
        this.btnReadOffsets = new System.Windows.Forms.Button();
        this.btnCancelScan = new System.Windows.Forms.Button();
        this.btnFixPeds = new System.Windows.Forms.Button();
        this.btnIV = new System.Windows.Forms.Button();
        this.comboBox1 = new System.Windows.Forms.ComboBox();
        this.button2 = new System.Windows.Forms.Button();
        this.label1 = new System.Windows.Forms.Label();
        this.button3 = new System.Windows.Forms.Button();
        this.button4 = new System.Windows.Forms.Button();
        this.button5 = new System.Windows.Forms.Button();
        this.groupBox1 = new System.Windows.Forms.GroupBox();
        this.panel1.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.udInc)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.udStop)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.udStart)).BeginInit();
        this.panel2.SuspendLayout();
        this.panel3.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.udChan)).BeginInit();
        this.panel4.SuspendLayout();
        this.groupBox1.SuspendLayout();
        this.SuspendLayout();
        // 
        // zg1
        // 
        this.zg1.Location = new System.Drawing.Point(4, 1);
        this.zg1.Margin = new System.Windows.Forms.Padding(4);
        this.zg1.Name = "zg1";
        this.zg1.ScrollGrace = 0D;
        this.zg1.ScrollMaxX = 0D;
        this.zg1.ScrollMaxY = 0D;
        this.zg1.ScrollMaxY2 = 0D;
        this.zg1.ScrollMinX = 0D;
        this.zg1.ScrollMinY = 0D;
        this.zg1.ScrollMinY2 = 0D;
        this.zg1.Size = new System.Drawing.Size(654, 369);
        this.zg1.TabIndex = 0;
        // 
        // btnUpdate
        // 
        this.btnUpdate.Enabled = false;
        this.btnUpdate.Location = new System.Drawing.Point(83, 3);
        this.btnUpdate.Name = "btnUpdate";
        this.btnUpdate.Size = new System.Drawing.Size(70, 23);
        this.btnUpdate.TabIndex = 11;
        this.btnUpdate.Text = "update";
        this.btnUpdate.UseVisualStyleBackColor = true;
        this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
        // 
        // panel1
        // 
        this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.panel1.Controls.Add(this.button1);
        this.panel1.Controls.Add(this.chkIntegral);
        this.panel1.Controls.Add(this.btnAutoScale);
        this.panel1.Controls.Add(this.chkLogY);
        this.panel1.Controls.Add(this.btnUpdate);
        this.panel1.Location = new System.Drawing.Point(405, 420);
        this.panel1.Name = "panel1";
        this.panel1.Size = new System.Drawing.Size(255, 61);
        this.panel1.TabIndex = 47;
        // 
        // button1
        // 
        this.button1.Location = new System.Drawing.Point(145, 32);
        this.button1.Name = "button1";
        this.button1.Size = new System.Drawing.Size(94, 24);
        this.button1.TabIndex = 58;
        this.button1.Text = "Save Graph";
        this.button1.UseVisualStyleBackColor = true;
        this.button1.Click += new System.EventHandler(this.button1_Click);
        // 
        // chkIntegral
        // 
        this.chkIntegral.AutoSize = true;
        this.chkIntegral.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.chkIntegral.Location = new System.Drawing.Point(3, 36);
        this.chkIntegral.Name = "chkIntegral";
        this.chkIntegral.Size = new System.Drawing.Size(123, 20);
        this.chkIntegral.TabIndex = 57;
        this.chkIntegral.Text = "Integral Spect";
        this.chkIntegral.UseVisualStyleBackColor = true;
        // 
        // btnAutoScale
        // 
        this.btnAutoScale.Enabled = false;
        this.btnAutoScale.Location = new System.Drawing.Point(188, 3);
        this.btnAutoScale.Name = "btnAutoScale";
        this.btnAutoScale.Size = new System.Drawing.Size(62, 23);
        this.btnAutoScale.TabIndex = 56;
        this.btnAutoScale.Text = "auto";
        this.btnAutoScale.UseVisualStyleBackColor = true;
        this.btnAutoScale.Click += new System.EventHandler(this.btnAutoScale_Click);
        // 
        // chkLogY
        // 
        this.chkLogY.AutoSize = true;
        this.chkLogY.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.chkLogY.Location = new System.Drawing.Point(3, 3);
        this.chkLogY.Name = "chkLogY";
        this.chkLogY.Size = new System.Drawing.Size(63, 20);
        this.chkLogY.TabIndex = 55;
        this.chkLogY.Text = "LogY";
        this.chkLogY.UseVisualStyleBackColor = true;
        // 
        // btnScan
        // 
        this.btnScan.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.btnScan.Location = new System.Drawing.Point(144, 3);
        this.btnScan.Name = "btnScan";
        this.btnScan.Size = new System.Drawing.Size(95, 33);
        this.btnScan.TabIndex = 54;
        this.btnScan.Text = "Scan";
        this.btnScan.UseVisualStyleBackColor = true;
        this.btnScan.Click += new System.EventHandler(this.btnScan_Click_1);
        // 
        // lblInc
        // 
        this.lblInc.AutoSize = true;
        this.lblInc.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.lblInc.Location = new System.Drawing.Point(13, 55);
        this.lblInc.Name = "lblInc";
        this.lblInc.Size = new System.Drawing.Size(25, 16);
        this.lblInc.TabIndex = 53;
        this.lblInc.Text = "Inc";
        // 
        // udInc
        // 
        this.udInc.Location = new System.Drawing.Point(44, 55);
        this.udInc.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
        this.udInc.Name = "udInc";
        this.udInc.Size = new System.Drawing.Size(95, 20);
        this.udInc.TabIndex = 52;
        this.udInc.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
        // 
        // lblStop
        // 
        this.lblStop.AutoSize = true;
        this.lblStop.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.lblStop.Location = new System.Drawing.Point(2, 29);
        this.lblStop.Name = "lblStop";
        this.lblStop.Size = new System.Drawing.Size(36, 16);
        this.lblStop.TabIndex = 51;
        this.lblStop.Text = "Stop";
        // 
        // udStop
        // 
        this.udStop.Location = new System.Drawing.Point(44, 29);
        this.udStop.Maximum = new decimal(new int[] {
            2047,
            0,
            0,
            0});
        this.udStop.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
        this.udStop.Name = "udStop";
        this.udStop.Size = new System.Drawing.Size(95, 20);
        this.udStop.TabIndex = 50;
        this.udStop.Value = new decimal(new int[] {
            200,
            0,
            0,
            0});
        // 
        // lblStart
        // 
        this.lblStart.AutoSize = true;
        this.lblStart.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.lblStart.Location = new System.Drawing.Point(2, 3);
        this.lblStart.Name = "lblStart";
        this.lblStart.Size = new System.Drawing.Size(35, 16);
        this.lblStart.TabIndex = 49;
        this.lblStart.Text = "Start";
        // 
        // udStart
        // 
        this.udStart.Location = new System.Drawing.Point(43, 3);
        this.udStart.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
        this.udStart.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
        this.udStart.Name = "udStart";
        this.udStart.Size = new System.Drawing.Size(95, 20);
        this.udStart.TabIndex = 48;
        this.udStart.Value = new decimal(new int[] {
            75,
            0,
            0,
            0});
        // 
        // btnAutoPeds
        // 
        this.btnAutoPeds.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.btnAutoPeds.Location = new System.Drawing.Point(145, 42);
        this.btnAutoPeds.Name = "btnAutoPeds";
        this.btnAutoPeds.Size = new System.Drawing.Size(95, 33);
        this.btnAutoPeds.TabIndex = 55;
        this.btnAutoPeds.Text = "Save";
        this.btnAutoPeds.UseVisualStyleBackColor = true;
        this.btnAutoPeds.Click += new System.EventHandler(this.btnAutoPeds_Click);
        // 
        // panel2
        // 
        this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.panel2.Controls.Add(this.lblInc);
        this.panel2.Controls.Add(this.udInc);
        this.panel2.Controls.Add(this.btnAutoPeds);
        this.panel2.Controls.Add(this.lblStop);
        this.panel2.Controls.Add(this.btnScan);
        this.panel2.Controls.Add(this.udStop);
        this.panel2.Controls.Add(this.lblStart);
        this.panel2.Controls.Add(this.udStart);
        this.panel2.Location = new System.Drawing.Point(405, 538);
        this.panel2.Name = "panel2";
        this.panel2.Size = new System.Drawing.Size(255, 88);
        this.panel2.TabIndex = 56;
        // 
        // panel3
        // 
        this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.panel3.Controls.Add(this.btn_ZeroOffset);
        this.panel3.Controls.Add(this.txtTOL);
        this.panel3.Controls.Add(this.lblPE_step);
        this.panel3.Controls.Add(this.chk_Tuner);
        this.panel3.Controls.Add(this.chk_PEfinder);
        this.panel3.Controls.Add(this.txtGoal);
        this.panel3.Controls.Add(this.label7);
        this.panel3.Controls.Add(this.btn_OffsetW);
        this.panel3.Controls.Add(this.txtOffset);
        this.panel3.Controls.Add(this.label6);
        this.panel3.Controls.Add(this.btnSavePeds);
        this.panel3.Controls.Add(this.btn_PedW);
        this.panel3.Controls.Add(this.txtPed);
        this.panel3.Controls.Add(this.lblPed);
        this.panel3.Controls.Add(this.label5);
        this.panel3.Controls.Add(this.udChan);
        this.panel3.Location = new System.Drawing.Point(248, 420);
        this.panel3.Name = "panel3";
        this.panel3.Size = new System.Drawing.Size(151, 206);
        this.panel3.TabIndex = 57;
        // 
        // btn_ZeroOffset
        // 
        this.btn_ZeroOffset.Location = new System.Drawing.Point(-1, 63);
        this.btn_ZeroOffset.Name = "btn_ZeroOffset";
        this.btn_ZeroOffset.Size = new System.Drawing.Size(26, 23);
        this.btn_ZeroOffset.TabIndex = 69;
        this.btn_ZeroOffset.Text = "0";
        this.btn_ZeroOffset.UseVisualStyleBackColor = true;
        this.btn_ZeroOffset.Click += new System.EventHandler(this.btn_ZeroOffset_Click);
        // 
        // txtTOL
        // 
        this.txtTOL.Location = new System.Drawing.Point(53, 143);
        this.txtTOL.Name = "txtTOL";
        this.txtTOL.Size = new System.Drawing.Size(53, 20);
        this.txtTOL.TabIndex = 68;
        this.txtTOL.Text = "50";
        // 
        // lblPE_step
        // 
        this.lblPE_step.AutoSize = true;
        this.lblPE_step.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.lblPE_step.Location = new System.Drawing.Point(3, 147);
        this.lblPE_step.Name = "lblPE_step";
        this.lblPE_step.Size = new System.Drawing.Size(44, 16);
        this.lblPE_step.TabIndex = 67;
        this.lblPE_step.Text = "STEP";
        // 
        // chk_Tuner
        // 
        this.chk_Tuner.AutoSize = true;
        this.chk_Tuner.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.chk_Tuner.Location = new System.Drawing.Point(89, 91);
        this.chk_Tuner.Name = "chk_Tuner";
        this.chk_Tuner.Size = new System.Drawing.Size(56, 20);
        this.chk_Tuner.TabIndex = 66;
        this.chk_Tuner.Text = "tune";
        this.chk_Tuner.UseVisualStyleBackColor = true;
        // 
        // chk_PEfinder
        // 
        this.chk_PEfinder.AutoSize = true;
        this.chk_PEfinder.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.chk_PEfinder.Location = new System.Drawing.Point(7, 92);
        this.chk_PEfinder.Name = "chk_PEfinder";
        this.chk_PEfinder.Size = new System.Drawing.Size(76, 20);
        this.chk_PEfinder.TabIndex = 65;
        this.chk_PEfinder.Text = "PE find";
        this.chk_PEfinder.UseVisualStyleBackColor = true;
        this.chk_PEfinder.CheckedChanged += new System.EventHandler(this.chk_PEfinder_CheckedChanged);
        // 
        // txtGoal
        // 
        this.txtGoal.Location = new System.Drawing.Point(53, 118);
        this.txtGoal.Name = "txtGoal";
        this.txtGoal.Size = new System.Drawing.Size(53, 20);
        this.txtGoal.TabIndex = 63;
        this.txtGoal.Text = "145";
        // 
        // label7
        // 
        this.label7.AutoSize = true;
        this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.label7.Location = new System.Drawing.Point(3, 122);
        this.label7.Name = "label7";
        this.label7.Size = new System.Drawing.Size(44, 16);
        this.label7.TabIndex = 62;
        this.label7.Text = "GOAL";
        // 
        // btn_OffsetW
        // 
        this.btn_OffsetW.Location = new System.Drawing.Point(112, 63);
        this.btn_OffsetW.Name = "btn_OffsetW";
        this.btn_OffsetW.Size = new System.Drawing.Size(26, 23);
        this.btn_OffsetW.TabIndex = 61;
        this.btn_OffsetW.Text = "W";
        this.btn_OffsetW.UseVisualStyleBackColor = true;
        this.btn_OffsetW.Click += new System.EventHandler(this.btn_OffsetW_Click);
        // 
        // txtOffset
        // 
        this.txtOffset.Location = new System.Drawing.Point(53, 65);
        this.txtOffset.Name = "txtOffset";
        this.txtOffset.Size = new System.Drawing.Size(53, 20);
        this.txtOffset.TabIndex = 60;
        // 
        // label6
        // 
        this.label6.AutoSize = true;
        this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.label6.Location = new System.Drawing.Point(31, 66);
        this.label6.Name = "label6";
        this.label6.Size = new System.Drawing.Size(24, 16);
        this.label6.TabIndex = 59;
        this.label6.Text = "Off";
        // 
        // btnSavePeds
        // 
        this.btnSavePeds.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.btnSavePeds.Location = new System.Drawing.Point(7, 169);
        this.btnSavePeds.Name = "btnSavePeds";
        this.btnSavePeds.Size = new System.Drawing.Size(120, 29);
        this.btnSavePeds.TabIndex = 58;
        this.btnSavePeds.Text = "Save Offsets";
        this.btnSavePeds.UseVisualStyleBackColor = true;
        this.btnSavePeds.Click += new System.EventHandler(this.btnSavePeds_Click);
        // 
        // btn_PedW
        // 
        this.btn_PedW.Location = new System.Drawing.Point(112, 37);
        this.btn_PedW.Name = "btn_PedW";
        this.btn_PedW.Size = new System.Drawing.Size(26, 23);
        this.btn_PedW.TabIndex = 57;
        this.btn_PedW.Text = "W";
        this.btn_PedW.UseVisualStyleBackColor = true;
        this.btn_PedW.Visible = false;
        this.btn_PedW.Click += new System.EventHandler(this.btn_PedW_Click);
        // 
        // txtPed
        // 
        this.txtPed.Location = new System.Drawing.Point(53, 39);
        this.txtPed.Name = "txtPed";
        this.txtPed.Size = new System.Drawing.Size(53, 20);
        this.txtPed.TabIndex = 51;
        // 
        // lblPed
        // 
        this.lblPed.AutoSize = true;
        this.lblPed.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.lblPed.Location = new System.Drawing.Point(22, 43);
        this.lblPed.Name = "lblPed";
        this.lblPed.Size = new System.Drawing.Size(33, 16);
        this.lblPed.TabIndex = 50;
        this.lblPed.Text = "Ped";
        // 
        // label5
        // 
        this.label5.AutoSize = true;
        this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.label5.Location = new System.Drawing.Point(14, 9);
        this.label5.Name = "label5";
        this.label5.Size = new System.Drawing.Size(41, 20);
        this.label5.TabIndex = 14;
        this.label5.Text = "CH#";
        // 
        // udChan
        // 
        this.udChan.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.udChan.Location = new System.Drawing.Point(53, 7);
        this.udChan.Maximum = new decimal(new int[] {
            31,
            0,
            0,
            0});
        this.udChan.Name = "udChan";
        this.udChan.Size = new System.Drawing.Size(53, 26);
        this.udChan.TabIndex = 13;
        this.udChan.ValueChanged += new System.EventHandler(this.udChan_ValueChanged);
        // 
        // panel4
        // 
        this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.panel4.Controls.Add(this.btnReadOffsets);
        this.panel4.Controls.Add(this.btnCancelScan);
        this.panel4.Controls.Add(this.btnFixPeds);
        this.panel4.Controls.Add(this.btnIV);
        this.panel4.Location = new System.Drawing.Point(405, 484);
        this.panel4.Name = "panel4";
        this.panel4.Size = new System.Drawing.Size(255, 52);
        this.panel4.TabIndex = 57;
        // 
        // btnReadOffsets
        // 
        this.btnReadOffsets.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.btnReadOffsets.Location = new System.Drawing.Point(83, 3);
        this.btnReadOffsets.Name = "btnReadOffsets";
        this.btnReadOffsets.Size = new System.Drawing.Size(95, 33);
        this.btnReadOffsets.TabIndex = 57;
        this.btnReadOffsets.Text = "Read Offsets";
        this.btnReadOffsets.UseVisualStyleBackColor = true;
        this.btnReadOffsets.Click += new System.EventHandler(this.btnReadOffsets_Click);
        // 
        // btnCancelScan
        // 
        this.btnCancelScan.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.btnCancelScan.Location = new System.Drawing.Point(180, 3);
        this.btnCancelScan.Name = "btnCancelScan";
        this.btnCancelScan.Size = new System.Drawing.Size(70, 33);
        this.btnCancelScan.TabIndex = 56;
        this.btnCancelScan.Text = "Cancel";
        this.btnCancelScan.UseVisualStyleBackColor = true;
        this.btnCancelScan.Visible = false;
        this.btnCancelScan.Click += new System.EventHandler(this.btnCancelScan_Click);
        // 
        // btnFixPeds
        // 
        this.btnFixPeds.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.btnFixPeds.Location = new System.Drawing.Point(3, 3);
        this.btnFixPeds.Name = "btnFixPeds";
        this.btnFixPeds.Size = new System.Drawing.Size(79, 33);
        this.btnFixPeds.TabIndex = 56;
        this.btnFixPeds.Text = "Fix PEDs";
        this.btnFixPeds.UseVisualStyleBackColor = true;
        this.btnFixPeds.Click += new System.EventHandler(this.btnFixPeds_Click);
        // 
        // btnIV
        // 
        this.btnIV.Location = new System.Drawing.Point(180, 13);
        this.btnIV.Name = "btnIV";
        this.btnIV.Size = new System.Drawing.Size(70, 23);
        this.btnIV.TabIndex = 12;
        this.btnIV.Text = "IV scan";
        this.btnIV.UseVisualStyleBackColor = true;
        this.btnIV.Click += new System.EventHandler(this.btnIV_Click);
        // 
        // comboBox1
        // 
        this.comboBox1.FormattingEnabled = true;
        this.comboBox1.Location = new System.Drawing.Point(446, 13);
        this.comboBox1.Name = "comboBox1";
        this.comboBox1.Size = new System.Drawing.Size(121, 21);
        this.comboBox1.TabIndex = 58;
        this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
        // 
        // button2
        // 
        this.button2.Location = new System.Drawing.Point(250, 14);
        this.button2.Name = "button2";
        this.button2.Size = new System.Drawing.Size(92, 20);
        this.button2.TabIndex = 59;
        this.button2.Text = "Save Histogram";
        this.button2.UseVisualStyleBackColor = true;
        this.button2.Click += new System.EventHandler(this.button2_Click);
        // 
        // label1
        // 
        this.label1.AutoSize = true;
        this.label1.Location = new System.Drawing.Point(343, 18);
        this.label1.Name = "label1";
        this.label1.Size = new System.Drawing.Size(96, 13);
        this.label1.TabIndex = 60;
        this.label1.Text = "Saved Histograms:";
        // 
        // button3
        // 
        this.button3.Location = new System.Drawing.Point(573, 11);
        this.button3.Name = "button3";
        this.button3.Size = new System.Drawing.Size(75, 23);
        this.button3.TabIndex = 61;
        this.button3.Text = "Show";
        this.button3.UseVisualStyleBackColor = true;
        this.button3.Click += new System.EventHandler(this.button3_Click);
        // 
        // button4
        // 
        this.button4.Location = new System.Drawing.Point(91, 14);
        this.button4.Name = "button4";
        this.button4.Size = new System.Drawing.Size(80, 20);
        this.button4.TabIndex = 62;
        this.button4.Text = "Save Session";
        this.button4.UseVisualStyleBackColor = true;
        this.button4.Click += new System.EventHandler(this.button4_Click);
        // 
        // button5
        // 
        this.button5.Location = new System.Drawing.Point(7, 14);
        this.button5.Name = "button5";
        this.button5.Size = new System.Drawing.Size(82, 20);
        this.button5.TabIndex = 63;
        this.button5.Text = "Load Session";
        this.button5.UseVisualStyleBackColor = true;
        this.button5.Click += new System.EventHandler(this.button5_Click);
        // 
        // groupBox1
        // 
        this.groupBox1.Controls.Add(this.button3);
        this.groupBox1.Controls.Add(this.comboBox1);
        this.groupBox1.Controls.Add(this.button5);
        this.groupBox1.Controls.Add(this.button4);
        this.groupBox1.Controls.Add(this.label1);
        this.groupBox1.Controls.Add(this.button2);
        this.groupBox1.Location = new System.Drawing.Point(5, 371);
        this.groupBox1.Name = "groupBox1";
        this.groupBox1.Size = new System.Drawing.Size(654, 40);
        this.groupBox1.TabIndex = 65;
        this.groupBox1.TabStop = false;
        this.groupBox1.Text = "Saved Histograms";
        // 
        // Hist1
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.AutoScroll = true;
        this.ClientSize = new System.Drawing.Size(671, 632);
        this.ControlBox = false;
        this.Controls.Add(this.panel4);
        this.Controls.Add(this.panel3);
        this.Controls.Add(this.panel2);
        this.Controls.Add(this.panel1);
        this.Controls.Add(this.zg1);
        this.Controls.Add(this.groupBox1);
        this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.Name = "Hist1";
        this.Tag = "HISTO";
        this.Text = "Hist1";
        this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Hist1_FormClosing);
        this.Load += new System.EventHandler(this.Hist1_Load);
        this.panel1.ResumeLayout(false);
        this.panel1.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)(this.udInc)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.udStop)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.udStart)).EndInit();
        this.panel2.ResumeLayout(false);
        this.panel2.PerformLayout();
        this.panel3.ResumeLayout(false);
        this.panel3.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)(this.udChan)).EndInit();
        this.panel4.ResumeLayout(false);
        this.groupBox1.ResumeLayout(false);
        this.groupBox1.PerformLayout();
        this.ResumeLayout(false);

    }

    
    #endregion

    private ZedGraph.ZedGraphControl zg1;
      public System.Windows.Forms.Button btnUpdate;
      private System.Windows.Forms.Panel panel1;
      public System.Windows.Forms.Button btnScan;
      private System.Windows.Forms.Label lblInc;
      private System.Windows.Forms.NumericUpDown udInc;
      private System.Windows.Forms.Label lblStop;
      private System.Windows.Forms.NumericUpDown udStop;
      private System.Windows.Forms.Label lblStart;
      private System.Windows.Forms.NumericUpDown udStart;
      private System.Windows.Forms.CheckBox chkLogY;
      private System.Windows.Forms.Button btnAutoPeds;
      private System.Windows.Forms.Panel panel2;
      private System.Windows.Forms.Panel panel3;
      private System.Windows.Forms.Label label5;
      private System.Windows.Forms.NumericUpDown udChan;
      public System.Windows.Forms.Button btnAutoScale;
      private System.Windows.Forms.TextBox txtPed;
      private System.Windows.Forms.Label lblPed;
      public System.Windows.Forms.Button btn_PedW;
      private System.Windows.Forms.Button btnSavePeds;
      public System.Windows.Forms.Button btn_OffsetW;
      private System.Windows.Forms.TextBox txtOffset;
      private System.Windows.Forms.Label label6;
      private System.Windows.Forms.TextBox txtTOL;
      private System.Windows.Forms.Label lblPE_step;
      private System.Windows.Forms.CheckBox chk_Tuner;
      private System.Windows.Forms.CheckBox chk_PEfinder;
      private System.Windows.Forms.TextBox txtGoal;
      private System.Windows.Forms.Label label7;
      private System.Windows.Forms.Panel panel4;
      public System.Windows.Forms.Button btnIV;
      private System.Windows.Forms.Button btnFixPeds;
      public System.Windows.Forms.Button btn_ZeroOffset;
      private System.Windows.Forms.Button btnCancelScan;
      private System.Windows.Forms.Button btnReadOffsets;
      private System.Windows.Forms.CheckBox chkIntegral;
      private System.Windows.Forms.Button button1;
      private System.Windows.Forms.ComboBox comboBox1;
      private System.Windows.Forms.Button button2;
      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.Button button3;
      private System.Windows.Forms.Button button4;
      private System.Windows.Forms.Button button5;
      private System.Windows.Forms.GroupBox groupBox1;
  }
}