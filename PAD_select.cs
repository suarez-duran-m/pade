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
    public partial class PAD_select : Form
    {
        public PAD_select()
        {
            InitializeComponent();
        }

        public void AddPAD(int index, bool flgUSB)
        {
            Button thisBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            thisBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            thisBtn.Location = new System.Drawing.Point(5 + (index - 1) * 110, 5);
            if (this.Width < thisBtn.Location.X + 110) { this.Width = thisBtn.Location.X + 110; }
            thisBtn.Name = "thisBtn";
            thisBtn.Size = new System.Drawing.Size(100, 60);
            thisBtn.TabIndex = 0;
            PADE thisPADE = new PADE(index);
            TB4.PADE_List.TryGetValue(index, out thisPADE);
            if (flgUSB)
            {
                TB4.myFTDI = thisPADE.PADE_FTDI;
                TB4.using_Ether_comms = false;
                TB4.using_USB_comms = true;
                thisPADE.PADE_is_MASTER = true;
                for (int i = 1; i < TB4.PADE_List.Count; i++)
                {
                    PADE testp = TB4.PADE_List[i];
                    if ((testp.flgUSB_comms) && (testp.PADE_is_MASTER))
                    { thisPADE.PADE_is_MASTER = false; }
                }
            }
            else
            {
                TB4.IP4_add = thisPADE.IP4_add;
                TB4.MAC_add = thisPADE.MAC_add;
                TB4.REG_port = thisPADE.REG_port;
                TB4.using_Ether_comms = true;
                TB4.using_USB_comms = false;
                thisPADE.PADE_is_MASTER = true;
                for (int i = 1; i < TB4.PADE_List.Count; i++)
                {
                    PADE testp = TB4.PADE_List[i];
                    if ((testp.flgEther_comms) && (testp.PADE_is_MASTER))
                    { thisPADE.PADE_is_MASTER = false; }
                }
            }
            string m = "";
            if (thisPADE.PADE_is_MASTER) { m = " MASTER "; } else { m = " SLAVE "; }

            if (thisPADE.PADE_is_MASTER)
            { TB4.regSTATUS.RegWrite(0); } //switch to Master right away
            else
            { TB4.regSTATUS.RegWrite(1); }
            TB4_Register ver_reg;
            int ind = 0;
            try
            {
                TB4.TB4_Registers_Dict.TryGetValue("FIRMWARE_VER", out ind);
                ver_reg = TB4.TB4_Registers[ind];
                ushort ver = ver_reg.RegRead();
                thisBtn.Text = "PADE " + index.ToString() + " v" + Convert.ToString(ver, 16) + "\r\n" + thisPADE.PADE_sn + "\r\n" + m;
                thisBtn.BackColor = System.Drawing.Color.Green;
                TB4.active_PAD_index = index;
                TB4.ActivePADE = thisPADE;
                
                Int32 it = index;
                thisBtn.Tag = it;
                // 
                // PAD_select
                // 
                //thisBtn.Click += new EventHandler(btn_PAD_Select_Click);  EDIT
                this.Controls.Add(thisBtn);
            
                if (index > 1)
                {
                    foreach (Control thatBtn in this.Controls)
                    {
                        if ((thatBtn.GetType() == typeof(Button)) && (thatBtn.Tag != thisBtn.Tag))
                        { thatBtn.BackColor = System.Drawing.Color.Gray; }
                    }
                }
            }
            catch(Exception ex)
            {
                TB4_Exception.logError(ex, "Could not read firmware version for PADE.", true);
            }
            this.ResumeLayout(false);
        }
        public void btn_PAD_Select_Click(object sender, EventArgs e)
        {
            Button thisBtn;
            thisBtn = (Button)sender;
            PADE thisPADE;
            int index = (Int32)thisBtn.Tag;
            try
            {
                TB4.PADE_List.TryGetValue(index, out thisPADE);
                thisBtn.BackColor = System.Drawing.Color.Green;
                string t = "";

                if (thisPADE.flgUSB_comms)
                {
                    TB4.myFTDI = thisPADE.PADE_FTDI;
                    TB4.myFTDI.GetSerialNumber(out t);
                    TB4.using_USB_comms = true;
                    TB4.using_Ether_comms = false;
                }
                else
                {
                    if (thisPADE.flgEther_comms)
                    {
                        TB4.MAC_add = thisPADE.MAC_add;
                        TB4.IP4_add = thisPADE.IP4_add;
                        t = thisPADE.PADE_sn;
                        TB4.using_USB_comms = false;
                        TB4.using_Ether_comms = true;
                        TB4.ActivePADE = thisPADE;
                    }
                    else
                    {
                        Exception except = new Exception("neither USB nor Ethernet comms enabled!");
                        //throw
                        MessageBox.Show("neither USB nor Ethernet comms enabled!");
                    }
                }
                TB4.theRegistersForm.Text = t + " PAD " + thisPADE.PADE_index;
                TB4.theRunForm.Text = t + " PAD " + thisPADE.PADE_index;
                TB4.theHist1.Text = t + " PAD " + thisPADE.PADE_index;
                TB4.thePlot.Text = t + " PAD " + thisPADE.PADE_index;
                TB4.theHist_and_Scan.Text = t + " PAD " + thisPADE.PADE_index;
                TB4.theFlash.Text = t + " PAD " + thisPADE.PADE_index;
                TB4.theGBE.Text = t + " PAD " + thisPADE.PADE_index;
                Application.DoEvents();
                TB4.active_PAD_index = index;
                foreach (Control thatBtn in this.Controls)
                {
                    if ((thatBtn.GetType() == typeof(Button)) && (thatBtn.Tag != thisBtn.Tag))
                    { thatBtn.BackColor = System.Drawing.Color.Gray; }
                }
            }
            catch(Exception ex)
            {
                TB4_Exception.logError(ex, "Could not find PADE.", true);
                //TB4.theRunForm
            }

        }

        private void PAD_select_Load(object sender, EventArgs e)
        {
            TB4.thePAD_selector = this;
        }
    }
}
