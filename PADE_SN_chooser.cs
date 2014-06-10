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
    public partial class PADE_SN_chooser : Form
    {
        public Int16 sn = 0;

        public PADE_SN_chooser()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            sn = Convert.ToInt16(numericUpDown1.Value);
            DialogResult = DialogResult.OK;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            sn = 0;
            DialogResult = DialogResult.Abort;
        }

        private void PADE_SN_chooser_Load(object sender, EventArgs e)
        {
            
        }
    }
}
