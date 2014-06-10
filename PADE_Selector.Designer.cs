using System.Windows.Forms;

namespace PADE
{
    partial class PADE_Selector 
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
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.selectedPADEList);
            this.groupBox3.Location = new System.Drawing.Point(4, 274);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(142, 124);
            this.groupBox3.TabIndex = 15;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Selected PADE";
            // 
            // selectedPADEList
            // 
            this.selectedPADEList.Location = new System.Drawing.Point(4, 19);
            this.selectedPADEList.Name = "selectedPADEList";
            this.selectedPADEList.Size = new System.Drawing.Size(132, 96);
            this.selectedPADEList.TabIndex = 9;
            
            this.selectedPADEList.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.selectedPADEList_Click);
            this.selectedPADEList.ControlAdded += new System.Windows.Forms.ControlEventHandler(this.syncListWithBoxSee);
            this.selectedPADEList.ControlRemoved += new System.Windows.Forms.ControlEventHandler(this.syncListWithBoxSee);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.treeView1);
            this.groupBox2.Location = new System.Drawing.Point(4, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(142, 256);
            this.groupBox2.TabIndex = 14;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "PADE";
            // 
            // treeView1
            // 
            this.treeView1.Location = new System.Drawing.Point(4, 19);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(132, 227);
            this.treeView1.TabIndex = 1;
            this.treeView1.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView1_Click);
            // 
            // PADE_Selector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(149, 401);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "PADE_Selector";
            this.Text = "PADE_Selector";
            this.DockStateChanged += new System.EventHandler(this.PADE_Selector_DockStateChanged);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PADE_Selector_FormClosing);
            this.Load += new System.EventHandler(this.PADE_Selector_Load);
            this.groupBox3.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }


        

        

        

        

        

        #endregion

        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox2;
        public TreeView selectedPADEList;
        public TreeView treeView1;
    }
}