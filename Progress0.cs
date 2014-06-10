using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PADE
{
    public partial class Progress0 : Form
    {
        public string Action = "";
        public string Prog = "";
        public DateTime StartTime;

        public UInt16 val = 0;
        public Progress0()
        {
            InitializeComponent();
            StartTime = DateTime.Now;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblAction.Text = Action;
            lblProgress.Text = Prog;
            progressBar1.Value = val;
            //this.Invalidate();
            if (!this.Visible) { this.Visible = true; }
            if (!this.TopMost) { this.TopMost = true; }
        }

        private void Progress0_Load(object sender, EventArgs e)
        {

        }
    }
}
