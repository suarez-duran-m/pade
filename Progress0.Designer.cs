namespace PADE
{
  partial class Progress0
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
        this.timer1 = new System.Windows.Forms.Timer(this.components);
        this.lblAction = new System.Windows.Forms.Label();
        this.lblProgress = new System.Windows.Forms.Label();
        this.progressBar1 = new System.Windows.Forms.ProgressBar();
        this.SuspendLayout();
        // 
        // timer1
        // 
        this.timer1.Interval = 1000;
        this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
        // 
        // lblAction
        // 
        this.lblAction.AutoSize = true;
        this.lblAction.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.lblAction.Location = new System.Drawing.Point(9, 18);
        this.lblAction.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
        this.lblAction.Name = "lblAction";
        this.lblAction.Size = new System.Drawing.Size(58, 20);
        this.lblAction.TabIndex = 0;
        this.lblAction.Text = "action";
        // 
        // lblProgress
        // 
        this.lblProgress.AutoSize = true;
        this.lblProgress.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.lblProgress.Location = new System.Drawing.Point(9, 77);
        this.lblProgress.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
        this.lblProgress.Name = "lblProgress";
        this.lblProgress.Size = new System.Drawing.Size(79, 20);
        this.lblProgress.TabIndex = 1;
        this.lblProgress.Text = "progress";
        // 
        // progressBar1
        // 
        this.progressBar1.Location = new System.Drawing.Point(9, 132);
        this.progressBar1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
        this.progressBar1.Name = "progressBar1";
        this.progressBar1.Size = new System.Drawing.Size(200, 32);
        this.progressBar1.TabIndex = 2;
        // 
        // Progress0
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(219, 192);
        this.ControlBox = false;
        this.Controls.Add(this.progressBar1);
        this.Controls.Add(this.lblProgress);
        this.Controls.Add(this.lblAction);
        this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
        this.Name = "Progress0";
        this.ShowIcon = false;
        this.ShowInTaskbar = false;
        this.Text = "Progress";
        this.TopMost = true;
        this.Load += new System.EventHandler(this.Progress0_Load);
        this.ResumeLayout(false);
        this.PerformLayout();

    }

    #endregion

    public System.Windows.Forms.Timer timer1;
    public System.Windows.Forms.Label lblAction;
    public System.Windows.Forms.Label lblProgress;
    public System.Windows.Forms.ProgressBar progressBar1;
  }
}