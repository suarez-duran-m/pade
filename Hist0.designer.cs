namespace PADE
{
    partial class Hist0
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
            this.zg_Histo = new ZedGraph.ZedGraphControl();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.chkLogY = new System.Windows.Forms.CheckBox();
            this.udChan = new System.Windows.Forms.NumericUpDown();
            this.btnRestart = new System.Windows.Forms.Button();
            this.lblCSR = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.chkPedMode = new System.Windows.Forms.CheckBox();
            this.btnAutoPed = new System.Windows.Forms.Button();
            this.udStart = new System.Windows.Forms.NumericUpDown();
            this.lblStart = new System.Windows.Forms.Label();
            this.lblStop = new System.Windows.Forms.Label();
            this.udStop = new System.Windows.Forms.NumericUpDown();
            this.lblInc = new System.Windows.Forms.Label();
            this.udInc = new System.Windows.Forms.NumericUpDown();
            this.btnScan = new System.Windows.Forms.Button();
            this.labelA = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.udChan)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udStart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udStop)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udInc)).BeginInit();
            this.SuspendLayout();
            // 
            // zg_Histo
            // 
            this.zg_Histo.Location = new System.Drawing.Point(2, 2);
            this.zg_Histo.Name = "zg_Histo";
            this.zg_Histo.ScrollGrace = 0D;
            this.zg_Histo.ScrollMaxX = 0D;
            this.zg_Histo.ScrollMaxY = 0D;
            this.zg_Histo.ScrollMaxY2 = 0D;
            this.zg_Histo.ScrollMinX = 0D;
            this.zg_Histo.ScrollMinY = 0D;
            this.zg_Histo.ScrollMinY2 = 0D;
            this.zg_Histo.Size = new System.Drawing.Size(541, 328);
            this.zg_Histo.TabIndex = 1;
            // 
            // btnUpdate
            // 
            this.btnUpdate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUpdate.Location = new System.Drawing.Point(437, 381);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(95, 39);
            this.btnUpdate.TabIndex = 2;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.button1_Click);
            // 
            // chkLogY
            // 
            this.chkLogY.AutoSize = true;
            this.chkLogY.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkLogY.Location = new System.Drawing.Point(356, 338);
            this.chkLogY.Name = "chkLogY";
            this.chkLogY.Size = new System.Drawing.Size(63, 20);
            this.chkLogY.TabIndex = 3;
            this.chkLogY.Text = "LogY";
            this.chkLogY.UseVisualStyleBackColor = true;
            this.chkLogY.CheckedChanged += new System.EventHandler(this.chkLogY_CheckedChanged);
            // 
            // udChan
            // 
            this.udChan.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.udChan.Location = new System.Drawing.Point(204, 340);
            this.udChan.Maximum = new decimal(new int[] {
            31,
            0,
            0,
            0});
            this.udChan.Name = "udChan";
            this.udChan.Size = new System.Drawing.Size(53, 26);
            this.udChan.TabIndex = 12;
            this.udChan.ValueChanged += new System.EventHandler(this.udChan_ValueChanged);
            // 
            // btnRestart
            // 
            this.btnRestart.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRestart.Location = new System.Drawing.Point(437, 336);
            this.btnRestart.Name = "btnRestart";
            this.btnRestart.Size = new System.Drawing.Size(95, 39);
            this.btnRestart.TabIndex = 13;
            this.btnRestart.Text = "Restart";
            this.btnRestart.UseVisualStyleBackColor = true;
            this.btnRestart.Click += new System.EventHandler(this.btnRestart_Click);
            // 
            // lblCSR
            // 
            this.lblCSR.AutoSize = true;
            this.lblCSR.Location = new System.Drawing.Point(219, 381);
            this.lblCSR.Name = "lblCSR";
            this.lblCSR.Size = new System.Drawing.Size(38, 13);
            this.lblCSR.TabIndex = 14;
            this.lblCSR.Text = "CSR =";
            // 
            // btnSave
            // 
            this.btnSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Location = new System.Drawing.Point(437, 471);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(95, 39);
            this.btnSave.TabIndex = 15;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // chkPedMode
            // 
            this.chkPedMode.AutoSize = true;
            this.chkPedMode.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkPedMode.Location = new System.Drawing.Point(287, 338);
            this.chkPedMode.Name = "chkPedMode";
            this.chkPedMode.Size = new System.Drawing.Size(58, 20);
            this.chkPedMode.TabIndex = 16;
            this.chkPedMode.Text = "PED";
            this.chkPedMode.UseVisualStyleBackColor = true;
            // 
            // btnAutoPed
            // 
            this.btnAutoPed.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAutoPed.Location = new System.Drawing.Point(437, 426);
            this.btnAutoPed.Name = "btnAutoPed";
            this.btnAutoPed.Size = new System.Drawing.Size(95, 39);
            this.btnAutoPed.TabIndex = 17;
            this.btnAutoPed.Text = "Auto Peds";
            this.btnAutoPed.UseVisualStyleBackColor = true;
            this.btnAutoPed.Click += new System.EventHandler(this.btnAutoPed_Click);
            // 
            // udStart
            // 
            this.udStart.Location = new System.Drawing.Point(437, 528);
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
            this.udStart.TabIndex = 18;
            this.udStart.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.udStart.ValueChanged += new System.EventHandler(this.udStart_ValueChanged);
            // 
            // lblStart
            // 
            this.lblStart.AutoSize = true;
            this.lblStart.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStart.Location = new System.Drawing.Point(402, 530);
            this.lblStart.Name = "lblStart";
            this.lblStart.Size = new System.Drawing.Size(35, 16);
            this.lblStart.TabIndex = 19;
            this.lblStart.Text = "Start";
            // 
            // lblStop
            // 
            this.lblStop.AutoSize = true;
            this.lblStop.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStop.Location = new System.Drawing.Point(402, 556);
            this.lblStop.Name = "lblStop";
            this.lblStop.Size = new System.Drawing.Size(36, 16);
            this.lblStop.TabIndex = 21;
            this.lblStop.Text = "Stop";
            // 
            // udStop
            // 
            this.udStop.Location = new System.Drawing.Point(437, 554);
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
            this.udStop.TabIndex = 20;
            this.udStop.Value = new decimal(new int[] {
            250,
            0,
            0,
            0});
            this.udStop.ValueChanged += new System.EventHandler(this.udStop_ValueChanged);
            // 
            // lblInc
            // 
            this.lblInc.AutoSize = true;
            this.lblInc.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInc.Location = new System.Drawing.Point(409, 582);
            this.lblInc.Name = "lblInc";
            this.lblInc.Size = new System.Drawing.Size(25, 16);
            this.lblInc.TabIndex = 23;
            this.lblInc.Text = "Inc";
            // 
            // udInc
            // 
            this.udInc.Location = new System.Drawing.Point(437, 580);
            this.udInc.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.udInc.Name = "udInc";
            this.udInc.Size = new System.Drawing.Size(95, 20);
            this.udInc.TabIndex = 22;
            this.udInc.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // btnScan
            // 
            this.btnScan.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnScan.Location = new System.Drawing.Point(437, 606);
            this.btnScan.Name = "btnScan";
            this.btnScan.Size = new System.Drawing.Size(95, 39);
            this.btnScan.TabIndex = 24;
            this.btnScan.Text = "Scan";
            this.btnScan.UseVisualStyleBackColor = true;
            this.btnScan.Click += new System.EventHandler(this.btnScan_Click);
            // 
            // labelA
            // 
            this.labelA.AutoSize = true;
            this.labelA.Location = new System.Drawing.Point(12, 341);
            this.labelA.Name = "labelA";
            this.labelA.Size = new System.Drawing.Size(35, 13);
            this.labelA.TabIndex = 26;
            this.labelA.Text = "label2";
            // 
            // Hist0
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(560, 732);
            this.ControlBox = false;
            this.Controls.Add(this.labelA);
            this.Controls.Add(this.btnScan);
            this.Controls.Add(this.lblInc);
            this.Controls.Add(this.udInc);
            this.Controls.Add(this.lblStop);
            this.Controls.Add(this.udStop);
            this.Controls.Add(this.lblStart);
            this.Controls.Add(this.udStart);
            this.Controls.Add(this.btnAutoPed);
            this.Controls.Add(this.chkPedMode);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.lblCSR);
            this.Controls.Add(this.btnRestart);
            this.Controls.Add(this.udChan);
            this.Controls.Add(this.chkLogY);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.zg_Histo);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "Hist0";
            this.Tag = "";
            this.Text = "Hist0";
            this.Load += new System.EventHandler(this.Hist0_Load);
            ((System.ComponentModel.ISupportInitialize)(this.udChan)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udStart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udStop)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udInc)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ZedGraph.ZedGraphControl zg_Histo;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.CheckBox chkLogY;
        private System.Windows.Forms.NumericUpDown udChan;
        private System.Windows.Forms.Button btnRestart;
        private System.Windows.Forms.Label lblCSR;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.CheckBox chkPedMode;
        private System.Windows.Forms.Button btnAutoPed;
        private System.Windows.Forms.NumericUpDown udStart;
        private System.Windows.Forms.Label lblStart;
        private System.Windows.Forms.Label lblStop;
        private System.Windows.Forms.NumericUpDown udStop;
        private System.Windows.Forms.Label lblInc;
        private System.Windows.Forms.NumericUpDown udInc;
        private System.Windows.Forms.Button btnScan;
        private System.Windows.Forms.Label labelA;
    }
}