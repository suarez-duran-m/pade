namespace PADE
{
    partial class PADE_Left_Toolbar
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
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.selectedPADEList = new System.Windows.Forms.TreeView();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.loadInitFileButton = new System.Windows.Forms.Button();
            this.downloadInitButton = new System.Windows.Forms.Button();
            this.uploadInitButton = new System.Windows.Forms.Button();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.selectedPADEList);
            this.groupBox3.Location = new System.Drawing.Point(12, 274);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(121, 124);
            this.groupBox3.TabIndex = 16;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Selected PADE";
            // 
            // selectedPADEList
            // 
            this.selectedPADEList.Location = new System.Drawing.Point(4, 19);
            this.selectedPADEList.Name = "selectedPADEList";
            this.selectedPADEList.Size = new System.Drawing.Size(110, 96);
            this.selectedPADEList.TabIndex = 9;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.treeView1);
            this.groupBox2.Location = new System.Drawing.Point(12, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(121, 256);
            this.groupBox2.TabIndex = 15;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "PADE";
            // 
            // treeView1
            // 
            this.treeView1.Location = new System.Drawing.Point(4, 19);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(110, 227);
            this.treeView1.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.loadInitFileButton);
            this.groupBox1.Controls.Add(this.downloadInitButton);
            this.groupBox1.Controls.Add(this.uploadInitButton);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(12, 404);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(121, 111);
            this.groupBox1.TabIndex = 14;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Initialization";
            // 
            // loadInitFileButton
            // 
            this.loadInitFileButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.loadInitFileButton.Location = new System.Drawing.Point(6, 19);
            this.loadInitFileButton.Name = "loadInitFileButton";
            this.loadInitFileButton.Size = new System.Drawing.Size(108, 24);
            this.loadInitFileButton.TabIndex = 7;
            this.loadInitFileButton.Text = "Load Init File";
            this.loadInitFileButton.UseVisualStyleBackColor = true;
            // 
            // downloadInitButton
            // 
            this.downloadInitButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.downloadInitButton.Location = new System.Drawing.Point(6, 49);
            this.downloadInitButton.Name = "downloadInitButton";
            this.downloadInitButton.Size = new System.Drawing.Size(108, 24);
            this.downloadInitButton.TabIndex = 6;
            this.downloadInitButton.Text = "Download Init";
            this.downloadInitButton.UseVisualStyleBackColor = true;
            // 
            // uploadInitButton
            // 
            this.uploadInitButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.uploadInitButton.Location = new System.Drawing.Point(6, 79);
            this.uploadInitButton.Name = "uploadInitButton";
            this.uploadInitButton.Size = new System.Drawing.Size(108, 24);
            this.uploadInitButton.TabIndex = 5;
            this.uploadInitButton.Text = "Initialize PADE";
            this.uploadInitButton.UseVisualStyleBackColor = true;
            // 
            // PADE_Left_Toolbar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(139, 522);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "PADE_Left_Toolbar";
            this.Text = "PADE_Left_Toolbar";
            this.Load += new System.EventHandler(this.PADE_Left_Toolbar_Load);
            this.groupBox3.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TreeView selectedPADEList;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button loadInitFileButton;
        private System.Windows.Forms.Button downloadInitButton;
        private System.Windows.Forms.Button uploadInitButton;
    }
}