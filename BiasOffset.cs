using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using WeifenLuo.WinFormsUI.Docking;

namespace PADE
{
    public partial class BiasOffset : DockContent
    {
        private NumericUpDown[] ud_offset=new NumericUpDown[33];
        private Label[] lbl_offset = new Label[33];
        
        public BiasOffset()
        {
            InitializeComponent();

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    int ind = i + 8 * j;
                    ud_offset[ind] = new NumericUpDown();
                    lbl_offset[ind] = new Label();
                    
                    ud_offset[ind].Location = new System.Drawing.Point(25 + j * 100, 20 + i * 30);
                    lbl_offset[ind].Location = new System.Drawing.Point(7 + j * 100, 25 + i * 30);
                    string t = "";
                    if (ind < 10) { t = "0"; } else { t = ""; }
                    lbl_offset[ind].Text = t+Convert.ToString(ind);

                    ud_offset[ind].Size = new System.Drawing.Size(60, 20);
                    ud_offset[ind].Maximum =Convert.ToDecimal( 3);
                    ud_offset[ind].Increment = Convert.ToDecimal(0.02);
                    ud_offset[ind].Minimum = Convert.ToDecimal(0);
                    //ud_offset[ind].TextAlign=
                    ud_offset[ind].DecimalPlaces = 2;

                    ud_offset[ind].TabIndex = ind;
                    this.Controls.Add(ud_offset[ind]);
                    ud_offset[ind].Click += new EventHandler(Click_ud);
                    this.Controls.Add(lbl_offset[ind]);
                }
                // numericUpDown1
                // 
            }
            
        }
        private void Click_ud(object sender, EventArgs e)
        {
            NumericUpDown this_ud = new NumericUpDown();
            this_ud = (NumericUpDown)sender;
            int i = Array.IndexOf<NumericUpDown>(ud_offset, this_ud);
            //danger,danger,danger: hardcoded register address
            UInt32 addr=Convert.ToUInt32( 0x800000+ i);
            TB4_Register temp_reg = new TB4_Register("temp register for bias offset", "temp", addr, 16, false, false);
            if ((this_ud.Value >= 0) && (this_ud.Value <= 3))
            {
                UInt16 hex_offset =Convert.ToUInt16( 1024 / 3 * this_ud.Value);
                temp_reg.RegWrite(hex_offset);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 32; i++)
            {
                if ((ud_offset[i].Value >= 0) && (ud_offset[i].Value <= 3))
                {
                    UInt32 addr = Convert.ToUInt32(0x800000 + i);
                    TB4_Register temp_reg = new TB4_Register("temp register for bias offset", "temp", addr, 16, false, false);
                    UInt16 hex_offset = Convert.ToUInt16(1024 / 3 * ud_offset[i].Value);
                    temp_reg.RegWrite(hex_offset);
                    System.Threading.Thread.Sleep(1);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Stream  fname  ;
            saveFileDialog1.Filter = "TB4 files (*.tb4)|*.tb4|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if ((fname= saveFileDialog1.OpenFile()) != null)
                {
                    StreamWriter sw = new StreamWriter(fname);
                    for (int i =0; i<32;i++)
                    {
                        string t="BIAS_OFFSET_CH"+i.ToString()+"<=";
                        UInt16 hex_offset = Convert.ToUInt16(1024 / 3 * ud_offset[i].Value);
                        t += Convert.ToString( hex_offset,16)+";";
                        sw.WriteLine(t);
                    }
                    sw.Close();
                }
            }
        }

        private void BiasOffset_Load(object sender, EventArgs e)
        {
            TB4.theBiasOffset = this;
            Size returnSize = this.Size;
            this.DockStateChanged += (object a, System.EventArgs b) => { TB4.thePADE_explorer.childChangedDockstate(this, returnSize); };
        }
        void BiasOffset_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            TB4.thePADE_explorer.childClosed(this);
            e.Cancel = true;
        }
    }
}
