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
    public partial class cBoxForm : Form
    {
        public string choice = "";
        public cBoxForm()
        {
            InitializeComponent();
            this.Shown += new EventHandler(cBoxForm_Shown);
            button1.Click += new EventHandler(button1_Click);
            foreach (TB4_Register reg in TB4.TB4_Registers)
            {
                if(reg!=null)comboBox1.Items.Add(reg.name);
            }
        }

        void button1_Click(object sender, EventArgs e)
        {
            choice = comboBox1.Text;
            this.Hide();
        }

        void cBoxForm_Shown(object sender, EventArgs e)
        {
            choice = "";
        }

        private void cBoxForm_Load(object sender, EventArgs e)
        {

        }
    }
}
