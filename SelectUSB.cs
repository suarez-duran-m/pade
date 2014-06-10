using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PADE
{
         
    public partial class SelectUSB : Form
    {
        public UInt32 SelectedItemNum = 0;
    
        public SelectUSB()
        {
            InitializeComponent();
        }

        private void btnOPEN_Click(object sender, EventArgs e)
        {
            string t = listBox1.SelectedItem.ToString();
            int p=t.IndexOf(" ");
            t = t.Substring(0, p);
            uint result=999;
            
            { result = USB_AID.FT_Open(t); }
            USB_AID.FT_ChangeLatency(2);
            //USB_AID.FT_ChangeBuffer(1020, 4096);
            if (result == 0)
            {
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                TB4.theRunForm.Text = listBox1.SelectedItem.ToString();
                if (chkEthernet.Checked) { TB4.myRun.flg_UDP = true; }
                else { TB4.myRun.flg_UDP = false; }
                Console.WriteLine("P: " + p + "    T: " + t);

            }
            else
            {
                this.DialogResult = System.Windows.Forms.DialogResult.Abort;
            }
        }

        private void btnABORT_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Abort;

        }

        private void SelectUSB_Load(object sender, EventArgs e)
        {

        }
    }
}
