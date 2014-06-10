namespace PADE
{
    partial class TempPlot
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
            this.zGraph = new ZedGraph.ZedGraphControl();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.unitsCombo = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.periodUpDown = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.sampleSizeUpDown = new System.Windows.Forms.NumericUpDown();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.minTimeBox = new System.Windows.Forms.TextBox();
            this.maxTimeBox = new System.Windows.Forms.TextBox();
            this.unitsLabel4 = new System.Windows.Forms.Label();
            this.unitsLabel3 = new System.Windows.Forms.Label();
            this.minimumTextBox = new System.Windows.Forms.TextBox();
            this.maximumTextBox = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.averageTextBox = new System.Windows.Forms.TextBox();
            this.unitsLabel1 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.periodUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sampleSizeUpDown)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // zGraph
            // 
            this.zGraph.Location = new System.Drawing.Point(12, 12);
            this.zGraph.Name = "zGraph";
            this.zGraph.ScrollGrace = 0D;
            this.zGraph.ScrollMaxX = 0D;
            this.zGraph.ScrollMaxY = 0D;
            this.zGraph.ScrollMaxY2 = 0D;
            this.zGraph.ScrollMinX = 0D;
            this.zGraph.ScrollMinY = 0D;
            this.zGraph.ScrollMinY2 = 0D;
            this.zGraph.Size = new System.Drawing.Size(647, 418);
            this.zGraph.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.checkBox2);
            this.groupBox1.Controls.Add(this.unitsCombo);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.periodUpDown);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.sampleSizeUpDown);
            this.groupBox1.Controls.Add(this.checkBox1);
            this.groupBox1.Location = new System.Drawing.Point(12, 445);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(270, 139);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "PADE Settings";
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBox2.Location = new System.Drawing.Point(139, 31);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(106, 17);
            this.checkBox2.TabIndex = 11;
            this.checkBox2.Text = "Analysis Enabled";
            this.checkBox2.UseVisualStyleBackColor = true;
            this.checkBox2.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
            // 
            // unitsCombo
            // 
            this.unitsCombo.FormattingEnabled = true;
            this.unitsCombo.Location = new System.Drawing.Point(138, 103);
            this.unitsCombo.Name = "unitsCombo";
            this.unitsCombo.Size = new System.Drawing.Size(107, 21);
            this.unitsCombo.TabIndex = 10;
            this.unitsCombo.SelectedIndexChanged += new System.EventHandler(this.unitsCombo_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(18, 106);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(31, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Units";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(196, 81);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(64, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Milliseconds";
            // 
            // periodUpDown
            // 
            this.periodUpDown.Location = new System.Drawing.Point(138, 79);
            this.periodUpDown.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.periodUpDown.Minimum = new decimal(new int[] {
            250,
            0,
            0,
            0});
            this.periodUpDown.Name = "periodUpDown";
            this.periodUpDown.Size = new System.Drawing.Size(52, 20);
            this.periodUpDown.TabIndex = 7;
            this.periodUpDown.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.periodUpDown.ValueChanged += new System.EventHandler(this.periodUpDown_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(196, 56);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Samples";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 81);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Sample Period:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 56);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Dataset Size:";
            // 
            // sampleSizeUpDown
            // 
            this.sampleSizeUpDown.Location = new System.Drawing.Point(138, 54);
            this.sampleSizeUpDown.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.sampleSizeUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.sampleSizeUpDown.Name = "sampleSizeUpDown";
            this.sampleSizeUpDown.Size = new System.Drawing.Size(52, 20);
            this.sampleSizeUpDown.TabIndex = 3;
            this.sampleSizeUpDown.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.sampleSizeUpDown.ValueChanged += new System.EventHandler(this.sampleSizeUpDown_ValueChanged);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBox1.Location = new System.Drawing.Point(17, 31);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(107, 17);
            this.checkBox1.TabIndex = 2;
            this.checkBox1.Text = "Log Temperature";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.minTimeBox);
            this.groupBox2.Controls.Add(this.maxTimeBox);
            this.groupBox2.Controls.Add(this.unitsLabel4);
            this.groupBox2.Controls.Add(this.unitsLabel3);
            this.groupBox2.Controls.Add(this.minimumTextBox);
            this.groupBox2.Controls.Add(this.maximumTextBox);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.averageTextBox);
            this.groupBox2.Controls.Add(this.unitsLabel1);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Location = new System.Drawing.Point(316, 445);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(343, 112);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Analysis";
            // 
            // minTimeBox
            // 
            this.minTimeBox.Location = new System.Drawing.Point(219, 78);
            this.minTimeBox.Name = "minTimeBox";
            this.minTimeBox.ReadOnly = true;
            this.minTimeBox.Size = new System.Drawing.Size(118, 20);
            this.minTimeBox.TabIndex = 14;
            this.minTimeBox.Text = "N/A";
            // 
            // maxTimeBox
            // 
            this.maxTimeBox.Location = new System.Drawing.Point(219, 53);
            this.maxTimeBox.Name = "maxTimeBox";
            this.maxTimeBox.ReadOnly = true;
            this.maxTimeBox.Size = new System.Drawing.Size(118, 20);
            this.maxTimeBox.TabIndex = 13;
            this.maxTimeBox.Text = "N/A";
            // 
            // unitsLabel4
            // 
            this.unitsLabel4.AutoSize = true;
            this.unitsLabel4.Location = new System.Drawing.Point(161, 81);
            this.unitsLabel4.Name = "unitsLabel4";
            this.unitsLabel4.Size = new System.Drawing.Size(40, 13);
            this.unitsLabel4.TabIndex = 11;
            this.unitsLabel4.Text = "Celsius";
            // 
            // unitsLabel3
            // 
            this.unitsLabel3.AutoSize = true;
            this.unitsLabel3.Location = new System.Drawing.Point(161, 56);
            this.unitsLabel3.Name = "unitsLabel3";
            this.unitsLabel3.Size = new System.Drawing.Size(40, 13);
            this.unitsLabel3.TabIndex = 10;
            this.unitsLabel3.Text = "Celsius";
            // 
            // minimumTextBox
            // 
            this.minimumTextBox.Location = new System.Drawing.Point(81, 78);
            this.minimumTextBox.Name = "minimumTextBox";
            this.minimumTextBox.ReadOnly = true;
            this.minimumTextBox.Size = new System.Drawing.Size(74, 20);
            this.minimumTextBox.TabIndex = 9;
            this.minimumTextBox.Text = "N/A";
            // 
            // maximumTextBox
            // 
            this.maximumTextBox.Location = new System.Drawing.Point(81, 53);
            this.maximumTextBox.Name = "maximumTextBox";
            this.maximumTextBox.ReadOnly = true;
            this.maximumTextBox.Size = new System.Drawing.Size(74, 20);
            this.maximumTextBox.TabIndex = 8;
            this.maximumTextBox.Text = "N/A";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 81);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(51, 13);
            this.label9.TabIndex = 7;
            this.label9.Text = "Minimum:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 56);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(54, 13);
            this.label7.TabIndex = 6;
            this.label7.Text = "Maximum:";
            // 
            // averageTextBox
            // 
            this.averageTextBox.Location = new System.Drawing.Point(81, 29);
            this.averageTextBox.Name = "averageTextBox";
            this.averageTextBox.ReadOnly = true;
            this.averageTextBox.Size = new System.Drawing.Size(74, 20);
            this.averageTextBox.TabIndex = 2;
            this.averageTextBox.Text = "N/A";
            // 
            // unitsLabel1
            // 
            this.unitsLabel1.AutoSize = true;
            this.unitsLabel1.Location = new System.Drawing.Point(161, 32);
            this.unitsLabel1.Name = "unitsLabel1";
            this.unitsLabel1.Size = new System.Drawing.Size(40, 13);
            this.unitsLabel1.TabIndex = 1;
            this.unitsLabel1.Text = "Celsius";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(7, 32);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(50, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "Average:";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(325, 563);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 21);
            this.button1.TabIndex = 4;
            this.button1.Text = "Save Graph";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // TempPlot
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(671, 598);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.zGraph);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "TempPlot";
            this.Text = "TempPlot";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TempPlot_FormClosing);
            this.Load += new System.EventHandler(this.TempPlot_Load);
            this.VisibleChanged += new System.EventHandler(this.TempPlot_VisibleChanged);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.periodUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sampleSizeUpDown)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        
        

        

        #endregion

        private ZedGraph.ZedGraphControl zedGraphControl1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        public ZedGraph.ZedGraphControl zGraph;
        public System.Windows.Forms.CheckBox checkBox1;
        public System.Windows.Forms.ComboBox unitsCombo;
        public System.Windows.Forms.NumericUpDown periodUpDown;
        public System.Windows.Forms.Label label2;
        public System.Windows.Forms.Label label1;
        public System.Windows.Forms.NumericUpDown sampleSizeUpDown;
        public System.Windows.Forms.TextBox minimumTextBox;
        public System.Windows.Forms.TextBox maximumTextBox;
        public System.Windows.Forms.TextBox averageTextBox;
        public System.Windows.Forms.Label unitsLabel1;
        public System.Windows.Forms.Label unitsLabel4;
        public System.Windows.Forms.Label unitsLabel3;
        private System.Windows.Forms.CheckBox checkBox2;
        public System.Windows.Forms.TextBox minTimeBox;
        public System.Windows.Forms.TextBox maxTimeBox;
        private System.Windows.Forms.Button button1;
    }
}