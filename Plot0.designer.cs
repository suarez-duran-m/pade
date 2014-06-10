namespace PADE
{
  partial class Plot0
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
        this.zg1 = new ZedGraph.ZedGraphControl();
        this.ud_MaxY = new System.Windows.Forms.NumericUpDown();
        this.ud_MinY = new System.Windows.Forms.NumericUpDown();
        this.label1 = new System.Windows.Forms.Label();
        this.label2 = new System.Windows.Forms.Label();
        this.label3 = new System.Windows.Forms.Label();
        this.ud_MinX = new System.Windows.Forms.NumericUpDown();
        this.btn_PLOT = new System.Windows.Forms.Button();
        this.label4 = new System.Windows.Forms.Label();
        this.ud_MaxX = new System.Windows.Forms.NumericUpDown();
        this.chk_ExternalTrig = new System.Windows.Forms.CheckBox();
        this.chk_persist = new System.Windows.Forms.CheckBox();
        this.chkCosmicTrig = new System.Windows.Forms.CheckBox();
        this.chk_AND_Ch0 = new System.Windows.Forms.CheckBox();
        this.chk_AND_Ch1 = new System.Windows.Forms.CheckBox();
        this.chk_AND_Ch2 = new System.Windows.Forms.CheckBox();
        this.chk_AND_Ch3 = new System.Windows.Forms.CheckBox();
        this.lblTimeOut = new System.Windows.Forms.Label();
        this.label5 = new System.Windows.Forms.Label();
        this.txt_TimeOutVal = new System.Windows.Forms.TextBox();
        this.lblSlope = new System.Windows.Forms.Label();
        this.lblTRIG_CSR = new System.Windows.Forms.Label();
        this.panel1 = new System.Windows.Forms.Panel();
        this.button1 = new System.Windows.Forms.Button();
        ((System.ComponentModel.ISupportInitialize)(this.ud_MaxY)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.ud_MinY)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.ud_MinX)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.ud_MaxX)).BeginInit();
        this.panel1.SuspendLayout();
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
        // ud_MaxY
        // 
        this.ud_MaxY.Location = new System.Drawing.Point(101, 26);
        this.ud_MaxY.Maximum = new decimal(new int[] {
            16500,
            0,
            0,
            0});
        this.ud_MaxY.Name = "ud_MaxY";
        this.ud_MaxY.Size = new System.Drawing.Size(71, 20);
        this.ud_MaxY.TabIndex = 5;
        this.ud_MaxY.Value = new decimal(new int[] {
            2100,
            0,
            0,
            0});
        // 
        // ud_MinY
        // 
        this.ud_MinY.Location = new System.Drawing.Point(101, 81);
        this.ud_MinY.Maximum = new decimal(new int[] {
            16000,
            0,
            0,
            0});
        this.ud_MinY.Name = "ud_MinY";
        this.ud_MinY.Size = new System.Drawing.Size(71, 20);
        this.ud_MinY.TabIndex = 6;
        // 
        // label1
        // 
        this.label1.AutoSize = true;
        this.label1.Location = new System.Drawing.Point(110, 10);
        this.label1.Name = "label1";
        this.label1.Size = new System.Drawing.Size(37, 13);
        this.label1.TabIndex = 7;
        this.label1.Text = "Max Y";
        // 
        // label2
        // 
        this.label2.AutoSize = true;
        this.label2.Location = new System.Drawing.Point(113, 104);
        this.label2.Name = "label2";
        this.label2.Size = new System.Drawing.Size(34, 13);
        this.label2.TabIndex = 8;
        this.label2.Text = "Min Y";
        // 
        // label3
        // 
        this.label3.AutoSize = true;
        this.label3.Location = new System.Drawing.Point(22, 40);
        this.label3.Name = "label3";
        this.label3.Size = new System.Drawing.Size(34, 13);
        this.label3.TabIndex = 10;
        this.label3.Text = "Min X";
        // 
        // ud_MinX
        // 
        this.ud_MinX.Location = new System.Drawing.Point(24, 55);
        this.ud_MinX.Maximum = new decimal(new int[] {
            4095,
            0,
            0,
            0});
        this.ud_MinX.Name = "ud_MinX";
        this.ud_MinX.Size = new System.Drawing.Size(71, 20);
        this.ud_MinX.TabIndex = 9;
        // 
        // btn_PLOT
        // 
        this.btn_PLOT.Enabled = false;
        this.btn_PLOT.Location = new System.Drawing.Point(101, 52);
        this.btn_PLOT.Name = "btn_PLOT";
        this.btn_PLOT.Size = new System.Drawing.Size(70, 23);
        this.btn_PLOT.TabIndex = 11;
        this.btn_PLOT.Text = "No USB!";
        this.btn_PLOT.UseVisualStyleBackColor = true;
        this.btn_PLOT.Click += new System.EventHandler(this.btn_PLOT_Click);
        // 
        // label4
        // 
        this.label4.AutoSize = true;
        this.label4.Location = new System.Drawing.Point(206, 39);
        this.label4.Name = "label4";
        this.label4.Size = new System.Drawing.Size(37, 13);
        this.label4.TabIndex = 13;
        this.label4.Text = "Max X";
        // 
        // ud_MaxX
        // 
        this.ud_MaxX.Location = new System.Drawing.Point(177, 55);
        this.ud_MaxX.Maximum = new decimal(new int[] {
            4095,
            0,
            0,
            0});
        this.ud_MaxX.Name = "ud_MaxX";
        this.ud_MaxX.Size = new System.Drawing.Size(71, 20);
        this.ud_MaxX.TabIndex = 12;
        this.ud_MaxX.Value = new decimal(new int[] {
            512,
            0,
            0,
            0});
        this.ud_MaxX.ValueChanged += new System.EventHandler(this.ud_MaxX_ValueChanged);
        // 
        // chk_ExternalTrig
        // 
        this.chk_ExternalTrig.AutoSize = true;
        this.chk_ExternalTrig.Checked = true;
        this.chk_ExternalTrig.CheckState = System.Windows.Forms.CheckState.Checked;
        this.chk_ExternalTrig.Enabled = false;
        this.chk_ExternalTrig.Location = new System.Drawing.Point(568, 443);
        this.chk_ExternalTrig.Name = "chk_ExternalTrig";
        this.chk_ExternalTrig.Size = new System.Drawing.Size(58, 17);
        this.chk_ExternalTrig.TabIndex = 14;
        this.chk_ExternalTrig.Text = "Ext trig";
        this.chk_ExternalTrig.UseVisualStyleBackColor = true;
        this.chk_ExternalTrig.CheckedChanged += new System.EventHandler(this.chk_ExternalTrig_CheckedChanged);
        // 
        // chk_persist
        // 
        this.chk_persist.AutoSize = true;
        this.chk_persist.Location = new System.Drawing.Point(568, 420);
        this.chk_persist.Name = "chk_persist";
        this.chk_persist.Size = new System.Drawing.Size(90, 17);
        this.chk_persist.TabIndex = 15;
        this.chk_persist.Text = "Infinite persist";
        this.chk_persist.UseVisualStyleBackColor = true;
        // 
        // chkCosmicTrig
        // 
        this.chkCosmicTrig.AutoSize = true;
        this.chkCosmicTrig.Enabled = false;
        this.chkCosmicTrig.Location = new System.Drawing.Point(568, 467);
        this.chkCosmicTrig.Name = "chkCosmicTrig";
        this.chkCosmicTrig.Size = new System.Drawing.Size(77, 17);
        this.chkCosmicTrig.TabIndex = 20;
        this.chkCosmicTrig.Text = "Cosmic trig";
        this.chkCosmicTrig.UseVisualStyleBackColor = true;
        this.chkCosmicTrig.CheckedChanged += new System.EventHandler(this.chkCosmicTrig_CheckedChanged);
        // 
        // chk_AND_Ch0
        // 
        this.chk_AND_Ch0.AutoSize = true;
        this.chk_AND_Ch0.Enabled = false;
        this.chk_AND_Ch0.Location = new System.Drawing.Point(568, 490);
        this.chk_AND_Ch0.Name = "chk_AND_Ch0";
        this.chk_AND_Ch0.Size = new System.Drawing.Size(45, 17);
        this.chk_AND_Ch0.TabIndex = 21;
        this.chk_AND_Ch0.Text = "Ch0";
        this.chk_AND_Ch0.UseVisualStyleBackColor = true;
        this.chk_AND_Ch0.Visible = false;
        // 
        // chk_AND_Ch1
        // 
        this.chk_AND_Ch1.AutoSize = true;
        this.chk_AND_Ch1.Enabled = false;
        this.chk_AND_Ch1.Location = new System.Drawing.Point(568, 514);
        this.chk_AND_Ch1.Name = "chk_AND_Ch1";
        this.chk_AND_Ch1.Size = new System.Drawing.Size(45, 17);
        this.chk_AND_Ch1.TabIndex = 22;
        this.chk_AND_Ch1.Text = "Ch1";
        this.chk_AND_Ch1.UseVisualStyleBackColor = true;
        this.chk_AND_Ch1.Visible = false;
        // 
        // chk_AND_Ch2
        // 
        this.chk_AND_Ch2.AutoSize = true;
        this.chk_AND_Ch2.Enabled = false;
        this.chk_AND_Ch2.Location = new System.Drawing.Point(616, 490);
        this.chk_AND_Ch2.Name = "chk_AND_Ch2";
        this.chk_AND_Ch2.Size = new System.Drawing.Size(45, 17);
        this.chk_AND_Ch2.TabIndex = 23;
        this.chk_AND_Ch2.Text = "Ch2";
        this.chk_AND_Ch2.UseVisualStyleBackColor = true;
        this.chk_AND_Ch2.Visible = false;
        // 
        // chk_AND_Ch3
        // 
        this.chk_AND_Ch3.AutoSize = true;
        this.chk_AND_Ch3.Enabled = false;
        this.chk_AND_Ch3.Location = new System.Drawing.Point(616, 514);
        this.chk_AND_Ch3.Name = "chk_AND_Ch3";
        this.chk_AND_Ch3.Size = new System.Drawing.Size(45, 17);
        this.chk_AND_Ch3.TabIndex = 24;
        this.chk_AND_Ch3.Text = "Ch3";
        this.chk_AND_Ch3.UseVisualStyleBackColor = true;
        this.chk_AND_Ch3.Visible = false;
        // 
        // lblTimeOut
        // 
        this.lblTimeOut.AutoSize = true;
        this.lblTimeOut.Location = new System.Drawing.Point(575, 580);
        this.lblTimeOut.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
        this.lblTimeOut.Name = "lblTimeOut";
        this.lblTimeOut.Size = new System.Drawing.Size(71, 13);
        this.lblTimeOut.TabIndex = 25;
        this.lblTimeOut.Text = "Trig timed out";
        // 
        // label5
        // 
        this.label5.AutoSize = true;
        this.label5.Location = new System.Drawing.Point(521, 560);
        this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
        this.label5.Name = "label5";
        this.label5.Size = new System.Drawing.Size(66, 13);
        this.label5.TabIndex = 28;
        this.label5.Text = "Time out per";
        // 
        // txt_TimeOutVal
        // 
        this.txt_TimeOutVal.Location = new System.Drawing.Point(601, 560);
        this.txt_TimeOutVal.Name = "txt_TimeOutVal";
        this.txt_TimeOutVal.Size = new System.Drawing.Size(57, 20);
        this.txt_TimeOutVal.TabIndex = 29;
        this.txt_TimeOutVal.Text = "1000";
        // 
        // lblSlope
        // 
        this.lblSlope.AutoSize = true;
        this.lblSlope.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.lblSlope.Location = new System.Drawing.Point(565, 538);
        this.lblSlope.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
        this.lblSlope.Name = "lblSlope";
        this.lblSlope.Size = new System.Drawing.Size(69, 13);
        this.lblSlope.TabIndex = 37;
        this.lblSlope.Text = "NEGATIVE";
        this.lblSlope.Visible = false;
        // 
        // lblTRIG_CSR
        // 
        this.lblTRIG_CSR.AutoSize = true;
        this.lblTRIG_CSR.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.lblTRIG_CSR.Location = new System.Drawing.Point(521, 580);
        this.lblTRIG_CSR.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
        this.lblTRIG_CSR.Name = "lblTRIG_CSR";
        this.lblTRIG_CSR.Size = new System.Drawing.Size(35, 13);
        this.lblTRIG_CSR.TabIndex = 38;
        this.lblTRIG_CSR.Text = "FFFF";
        // 
        // panel1
        // 
        this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.panel1.Controls.Add(this.label4);
        this.panel1.Controls.Add(this.ud_MaxX);
        this.panel1.Controls.Add(this.btn_PLOT);
        this.panel1.Controls.Add(this.label3);
        this.panel1.Controls.Add(this.ud_MinX);
        this.panel1.Controls.Add(this.label2);
        this.panel1.Controls.Add(this.label1);
        this.panel1.Controls.Add(this.ud_MinY);
        this.panel1.Controls.Add(this.ud_MaxY);
        this.panel1.Location = new System.Drawing.Point(307, 420);
        this.panel1.Name = "panel1";
        this.panel1.Size = new System.Drawing.Size(253, 122);
        this.panel1.TabIndex = 47;
        // 
        // button1
        // 
        this.button1.Location = new System.Drawing.Point(409, 573);
        this.button1.Name = "button1";
        this.button1.Size = new System.Drawing.Size(91, 20);
        this.button1.TabIndex = 48;
        this.button1.Text = "Save Graph";
        this.button1.UseVisualStyleBackColor = true;
        // 
        // Plot0
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.AutoScroll = true;
        this.ClientSize = new System.Drawing.Size(671, 629);
        this.ControlBox = false;
        this.Controls.Add(this.button1);
        this.Controls.Add(this.panel1);
        this.Controls.Add(this.lblTRIG_CSR);
        this.Controls.Add(this.lblSlope);
        this.Controls.Add(this.txt_TimeOutVal);
        this.Controls.Add(this.label5);
        this.Controls.Add(this.lblTimeOut);
        this.Controls.Add(this.chk_AND_Ch3);
        this.Controls.Add(this.chk_AND_Ch2);
        this.Controls.Add(this.chk_AND_Ch1);
        this.Controls.Add(this.chk_AND_Ch0);
        this.Controls.Add(this.chkCosmicTrig);
        this.Controls.Add(this.chk_persist);
        this.Controls.Add(this.chk_ExternalTrig);
        this.Controls.Add(this.zg1);
        this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.Name = "Plot0";
        this.ShowInTaskbar = false;
        this.Tag = "Plot";
        this.Text = "Plot0";
        this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Plot0_FormClosing);
        this.Load += new System.EventHandler(this.Plot0_Load);
        ((System.ComponentModel.ISupportInitialize)(this.ud_MaxY)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.ud_MinY)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.ud_MinX)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.ud_MaxX)).EndInit();
        this.panel1.ResumeLayout(false);
        this.panel1.PerformLayout();
        this.ResumeLayout(false);
        this.PerformLayout();

    }

    

    #endregion

    private System.Windows.Forms.NumericUpDown ud_MaxY;
      private System.Windows.Forms.NumericUpDown ud_MinY;
      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.Label label2;
      private System.Windows.Forms.Label label3;
      private System.Windows.Forms.NumericUpDown ud_MinX;
      private System.Windows.Forms.Label label4;
      private System.Windows.Forms.NumericUpDown ud_MaxX;
      private System.Windows.Forms.CheckBox chk_persist;
      public System.Windows.Forms.Button btn_PLOT;
      private System.Windows.Forms.Label lblTimeOut;
      private System.Windows.Forms.Label label5;
      private System.Windows.Forms.TextBox txt_TimeOutVal;
      public System.Windows.Forms.Label lblSlope;
      public System.Windows.Forms.CheckBox chk_ExternalTrig;
      public System.Windows.Forms.CheckBox chkCosmicTrig;
      public System.Windows.Forms.CheckBox chk_AND_Ch0;
      public System.Windows.Forms.CheckBox chk_AND_Ch1;
      public System.Windows.Forms.CheckBox chk_AND_Ch2;
      public System.Windows.Forms.CheckBox chk_AND_Ch3;
      public System.Windows.Forms.Label lblTRIG_CSR;
      private System.Windows.Forms.Panel panel1;
      private System.Windows.Forms.Button button1;
      public ZedGraph.ZedGraphControl zg1;
  }
}