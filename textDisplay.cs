using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PADE
{
    public partial class textDisplay : Form
    {

        public string text="";
        public textDisplay()
        {
            InitializeComponent();
        }

        private void textDisplay_Load(object sender, EventArgs e)
        {

        }

        void textDisplay_VisibleChanged(object sender, System.EventArgs e)
        {
            richTextBox1.Text = text;
        }
    }
}
