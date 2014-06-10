namespace PADE
{
    partial class Hist_and_Scan
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
        this.panel1 = new System.Windows.Forms.Panel();
        this.chkLogY = new System.Windows.Forms.CheckBox();
        this.btnScan = new System.Windows.Forms.Button();
        this.lblInc = new System.Windows.Forms.Label();
        this.udInc = new System.Windows.Forms.NumericUpDown();
        this.lblStop = new System.Windows.Forms.Label();
        this.udStop = new System.Windows.Forms.NumericUpDown();
        this.lblStart = new System.Windows.Forms.Label();
        this.udStart = new System.Windows.Forms.NumericUpDown();
        this.btnCancel = new System.Windows.Forms.Button();
        this.panel2 = new System.Windows.Forms.Panel();
        this.label1 = new System.Windows.Forms.Label();
        this.udSamplePer = new System.Windows.Forms.NumericUpDown();
        this.panel3 = new System.Windows.Forms.Panel();
        this.lblScanMsg = new System.Windows.Forms.Label();
        this.timer1 = new System.Windows.Forms.Timer(this.components);
        this.graphButton = new System.Windows.Forms.Button();
        this.panel1.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.udInc)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.udStop)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.udStart)).BeginInit();
        this.panel2.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.udSamplePer)).BeginInit();
        this.panel3.SuspendLayout();
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
        this.zg1.Size = new System.Drawing.Size(654, 414);
        this.zg1.TabIndex = 0;
        // 
        // panel1
        // 
        this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.panel1.Controls.Add(this.chkLogY);
        this.panel1.Location = new System.Drawing.Point(405, 423);
        this.panel1.Name = "panel1";
        this.panel1.Size = new System.Drawing.Size(255, 31);
        this.panel1.TabIndex = 47;
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
        this.lblInc.Visible = false;
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
            100,
            0,
            0,
            0});
        this.udInc.Visible = false;
        // 
        // lblStop
        // 
        this.lblStop.AutoSize = true;
        this.lblStop.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.lblStop.Location = new System.Drawing.Point(1, 16);
        this.lblStop.Name = "lblStop";
        this.lblStop.Size = new System.Drawing.Size(27, 16);
        this.lblStop.TabIndex = 51;
        this.lblStop.Text = "Vth";
        // 
        // udStop
        // 
        this.udStop.Location = new System.Drawing.Point(43, 16);
        this.udStop.Maximum = new decimal(new int[] {
            1000,
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
            400,
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
        this.lblStart.Visible = false;
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
        this.udStart.Visible = false;
        // 
        // btnCancel
        // 
        this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.btnCancel.Location = new System.Drawing.Point(145, 42);
        this.btnCancel.Name = "btnCancel";
        this.btnCancel.Size = new System.Drawing.Size(95, 33);
        this.btnCancel.TabIndex = 55;
        this.btnCancel.Text = "CANCEL";
        this.btnCancel.UseVisualStyleBackColor = true;
        this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
        // 
        // panel2
        // 
        this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.panel2.Controls.Add(this.label1);
        this.panel2.Controls.Add(this.lblInc);
        this.panel2.Controls.Add(this.udInc);
        this.panel2.Controls.Add(this.udSamplePer);
        this.panel2.Controls.Add(this.btnCancel);
        this.panel2.Controls.Add(this.lblStop);
        this.panel2.Controls.Add(this.btnScan);
        this.panel2.Controls.Add(this.udStop);
        this.panel2.Controls.Add(this.lblStart);
        this.panel2.Controls.Add(this.udStart);
        this.panel2.Location = new System.Drawing.Point(405, 460);
        this.panel2.Name = "panel2";
        this.panel2.Size = new System.Drawing.Size(255, 88);
        this.panel2.TabIndex = 56;
        // 
        // label1
        // 
        this.label1.AutoSize = true;
        this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.label1.Location = new System.Drawing.Point(2, 45);
        this.label1.Name = "label1";
        this.label1.Size = new System.Drawing.Size(78, 16);
        this.label1.TabIndex = 57;
        this.label1.Text = "Sample per";
        // 
        // udSamplePer
        // 
        this.udSamplePer.Location = new System.Drawing.Point(83, 45);
        this.udSamplePer.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
        this.udSamplePer.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
        this.udSamplePer.Name = "udSamplePer";
        this.udSamplePer.Size = new System.Drawing.Size(53, 20);
        this.udSamplePer.TabIndex = 56;
        this.udSamplePer.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
        // 
        // panel3
        // 
        this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.panel3.Controls.Add(this.lblScanMsg);
        this.panel3.Location = new System.Drawing.Point(405, 554);
        this.panel3.Name = "panel3";
        this.panel3.Size = new System.Drawing.Size(255, 66);
        this.panel3.TabIndex = 57;
        // 
        // lblScanMsg
        // 
        this.lblScanMsg.AutoEllipsis = true;
        this.lblScanMsg.AutoSize = true;
        this.lblScanMsg.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.lblScanMsg.Location = new System.Drawing.Point(2, 3);
        this.lblScanMsg.Name = "lblScanMsg";
        this.lblScanMsg.Size = new System.Drawing.Size(95, 16);
        this.lblScanMsg.TabIndex = 49;
        this.lblScanMsg.Text = "Waiting to start";
        // 
        // timer1
        // 
        this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
        // 
        // graphButton
        // 
        this.graphButton.Location = new System.Drawing.Point(275, 427);
        this.graphButton.Name = "graphButton";
        this.graphButton.Size = new System.Drawing.Size(124, 24);
        this.graphButton.TabIndex = 58;
        this.graphButton.Text = "Save Graph";
        this.graphButton.UseVisualStyleBackColor = true;
        // 
        // Hist_and_Scan
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(671, 632);
        this.ControlBox = false;
        this.Controls.Add(this.graphButton);
        this.Controls.Add(this.panel3);
        this.Controls.Add(this.panel2);
        this.Controls.Add(this.panel1);
        this.Controls.Add(this.zg1);
        this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.Name = "Hist_and_Scan";
        this.Tag = "Scanner";
        this.Text = "Hist_and_Scan";
        this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Hist_and_Scan_FormClosing);
        this.Load += new System.EventHandler(this.Hist_and_Scan_Load);
        this.panel1.ResumeLayout(false);
        this.panel1.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)(this.udInc)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.udStop)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.udStart)).EndInit();
        this.panel2.ResumeLayout(false);
        this.panel2.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)(this.udSamplePer)).EndInit();
        this.panel3.ResumeLayout(false);
        this.panel3.PerformLayout();
        this.ResumeLayout(false);

        this.graphButton.Click += new System.EventHandler(graphButton_Click);
    }

    

    

    #endregion

    private ZedGraph.ZedGraphControl zg1;
      private System.Windows.Forms.Panel panel1;
      private System.Windows.Forms.Button btnScan;
      private System.Windows.Forms.Label lblInc;
      private System.Windows.Forms.NumericUpDown udInc;
      private System.Windows.Forms.Label lblStop;
      private System.Windows.Forms.NumericUpDown udStop;
      private System.Windows.Forms.Label lblStart;
      private System.Windows.Forms.NumericUpDown udStart;
      private System.Windows.Forms.CheckBox chkLogY;
      private System.Windows.Forms.Button btnCancel;
      private System.Windows.Forms.Panel panel2;
      private System.Windows.Forms.Panel panel3;
      private System.Windows.Forms.Label lblScanMsg;
      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.NumericUpDown udSamplePer;
      private System.Windows.Forms.Timer timer1;
      private System.Windows.Forms.Button graphButton;
  }
}