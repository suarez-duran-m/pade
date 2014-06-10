namespace PADE
{
    partial class DataDebug
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button6 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.numRegBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.numRegButton = new System.Windows.Forms.Button();
            this.endSubmit = new System.Windows.Forms.Button();
            this.startSubmit = new System.Windows.Forms.Button();
            this.endBox = new System.Windows.Forms.TextBox();
            this.startBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.toolTip2 = new System.Windows.Forms.ToolTip(this.components);
            this.counterRadio = new System.Windows.Forms.RadioButton();
            this.scopeRadio = new System.Windows.Forms.RadioButton();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.compressionCombo = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.regComboBox = new System.Windows.Forms.ComboBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // zg1
            // 
            this.zg1.Location = new System.Drawing.Point(12, 38);
            this.zg1.Name = "zg1";
            this.zg1.ScrollGrace = 0D;
            this.zg1.ScrollMaxX = 0D;
            this.zg1.ScrollMaxY = 0D;
            this.zg1.ScrollMaxY2 = 0D;
            this.zg1.ScrollMinX = 0D;
            this.zg1.ScrollMinY = 0D;
            this.zg1.ScrollMinY2 = 0D;
            this.zg1.Size = new System.Drawing.Size(647, 393);
            this.zg1.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button6);
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.button5);
            this.groupBox1.Location = new System.Drawing.Point(537, 437);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(122, 175);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Graphing";
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(11, 130);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(99, 30);
            this.button6.TabIndex = 3;
            this.button6.Text = "Stop AutoPlot";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(11, 96);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(99, 30);
            this.button2.TabIndex = 2;
            this.button2.Text = "AutoPlot";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(11, 61);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(99, 30);
            this.button1.TabIndex = 1;
            this.button1.Text = "Clear";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(11, 25);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(99, 30);
            this.button5.TabIndex = 0;
            this.button5.Text = "Plot";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.regComboBox);
            this.groupBox2.Controls.Add(this.numRegBox);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.numRegButton);
            this.groupBox2.Controls.Add(this.endSubmit);
            this.groupBox2.Controls.Add(this.startSubmit);
            this.groupBox2.Controls.Add(this.endBox);
            this.groupBox2.Controls.Add(this.startBox);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(139, 437);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(190, 175);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Registers";
            // 
            // numRegBox
            // 
            this.numRegBox.Location = new System.Drawing.Point(9, 85);
            this.numRegBox.Name = "numRegBox";
            this.numRegBox.Size = new System.Drawing.Size(106, 20);
            this.numRegBox.TabIndex = 8;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 67);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(106, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Number of Registers:";
            // 
            // numRegButton
            // 
            this.numRegButton.Location = new System.Drawing.Point(125, 83);
            this.numRegButton.Name = "numRegButton";
            this.numRegButton.Size = new System.Drawing.Size(59, 23);
            this.numRegButton.TabIndex = 6;
            this.numRegButton.Text = "Submit";
            this.numRegButton.UseVisualStyleBackColor = true;
            this.numRegButton.Click += new System.EventHandler(this.numRegButton_Click);
            // 
            // endSubmit
            // 
            this.endSubmit.Location = new System.Drawing.Point(125, 128);
            this.endSubmit.Name = "endSubmit";
            this.endSubmit.Size = new System.Drawing.Size(59, 23);
            this.endSubmit.TabIndex = 5;
            this.endSubmit.Text = "Submit";
            this.endSubmit.UseVisualStyleBackColor = true;
            this.endSubmit.Click += new System.EventHandler(this.endSubmit_Click);
            // 
            // startSubmit
            // 
            this.startSubmit.Location = new System.Drawing.Point(125, 42);
            this.startSubmit.Name = "startSubmit";
            this.startSubmit.Size = new System.Drawing.Size(59, 23);
            this.startSubmit.TabIndex = 4;
            this.startSubmit.Text = "Submit";
            this.startSubmit.UseVisualStyleBackColor = true;
            this.startSubmit.Click += new System.EventHandler(this.startSubmit_Click);
            // 
            // endBox
            // 
            this.endBox.Location = new System.Drawing.Point(9, 131);
            this.endBox.Name = "endBox";
            this.endBox.Size = new System.Drawing.Size(110, 20);
            this.endBox.TabIndex = 3;
            // 
            // startBox
            // 
            this.startBox.Location = new System.Drawing.Point(9, 44);
            this.startBox.Name = "startBox";
            this.startBox.Size = new System.Drawing.Size(110, 20);
            this.startBox.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 115);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "End Address:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Start Address:";
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(6, 26);
            this.textBox3.Multiline = true;
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(166, 102);
            this.textBox3.TabIndex = 3;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.button4);
            this.groupBox3.Controls.Add(this.button3);
            this.groupBox3.Controls.Add(this.textBox3);
            this.groupBox3.Location = new System.Drawing.Point(344, 437);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(178, 174);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Debugging";
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(103, 134);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(69, 23);
            this.button4.TabIndex = 5;
            this.button4.Text = "Clear";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(6, 134);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(97, 23);
            this.button3.TabIndex = 4;
            this.button3.Text = "Read Range";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBox1.Location = new System.Drawing.Point(8, 25);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(94, 17);
            this.checkBox1.TabIndex = 5;
            this.checkBox1.Text = "Dump To Disc";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // counterRadio
            // 
            this.counterRadio.AutoSize = true;
            this.counterRadio.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.counterRadio.Checked = true;
            this.counterRadio.Location = new System.Drawing.Point(6, 48);
            this.counterRadio.Name = "counterRadio";
            this.counterRadio.Size = new System.Drawing.Size(92, 17);
            this.counterRadio.TabIndex = 6;
            this.counterRadio.TabStop = true;
            this.counterRadio.Text = "Counter Mode";
            this.counterRadio.UseVisualStyleBackColor = true;
            this.counterRadio.CheckedChanged += new System.EventHandler(this.counterRadio_CheckedChanged);
            // 
            // scopeRadio
            // 
            this.scopeRadio.AutoSize = true;
            this.scopeRadio.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.scopeRadio.Location = new System.Drawing.Point(6, 71);
            this.scopeRadio.Name = "scopeRadio";
            this.scopeRadio.Size = new System.Drawing.Size(86, 17);
            this.scopeRadio.TabIndex = 7;
            this.scopeRadio.TabStop = true;
            this.scopeRadio.Text = "Scope Mode";
            this.scopeRadio.UseVisualStyleBackColor = true;
            this.scopeRadio.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.radioButton1);
            this.groupBox4.Controls.Add(this.compressionCombo);
            this.groupBox4.Controls.Add(this.label4);
            this.groupBox4.Controls.Add(this.scopeRadio);
            this.groupBox4.Controls.Add(this.counterRadio);
            this.groupBox4.Controls.Add(this.checkBox1);
            this.groupBox4.Location = new System.Drawing.Point(12, 437);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(120, 175);
            this.groupBox4.TabIndex = 8;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Settings";
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.radioButton1.Location = new System.Drawing.Point(8, 94);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(94, 17);
            this.radioButton1.TabIndex = 10;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "Register Mode";
            this.radioButton1.UseVisualStyleBackColor = true;
            this.radioButton1.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // compressionCombo
            // 
            this.compressionCombo.FormattingEnabled = true;
            this.compressionCombo.Location = new System.Drawing.Point(14, 148);
            this.compressionCombo.Name = "compressionCombo";
            this.compressionCombo.Size = new System.Drawing.Size(101, 21);
            this.compressionCombo.TabIndex = 9;
            this.compressionCombo.SelectedIndexChanged += new System.EventHandler(this.compressionCombo_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(14, 128);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(100, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Compression Mode:";
            // 
            // menuStrip1
            // 
            this.menuStrip1.AllowMerge = false;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(671, 24);
            this.menuStrip1.TabIndex = 9;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(63, 20);
            this.fileToolStripMenuItem.Text = "Channels";
            this.fileToolStripMenuItem.Click += new System.EventHandler(this.fileToolStripMenuItem_Click);
            // 
            // regComboBox
            // 
            this.regComboBox.FormattingEnabled = true;
            this.regComboBox.Location = new System.Drawing.Point(9, 42);
            this.regComboBox.Name = "regComboBox";
            this.regComboBox.Size = new System.Drawing.Size(110, 21);
            this.regComboBox.TabIndex = 9;
            // 
            // DataDebug
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(671, 616);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.zg1);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "DataDebug";
            this.Text = "DataDebug";
            this.Load += new System.EventHandler(this.DataDebug_Load);
            this.VisibleChanged += new System.EventHandler(this.DataDebug_VisibleChanged);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        

        #endregion

        private ZedGraph.ZedGraphControl zg1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button endSubmit;
        private System.Windows.Forms.Button startSubmit;
        private System.Windows.Forms.TextBox endBox;
        private System.Windows.Forms.TextBox startBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.TextBox numRegBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button numRegButton;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ToolTip toolTip2;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.RadioButton counterRadio;
        private System.Windows.Forms.RadioButton scopeRadio;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ComboBox compressionCombo;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.ComboBox regComboBox;
    }
}