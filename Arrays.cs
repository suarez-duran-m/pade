using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using WeifenLuo.WinFormsUI.Docking;

namespace PADE
{
    public partial class Arrays : DockContent
    {
        public Arrays()
        {
            InitializeComponent();
        }

        TB4_Register rxind = new TB4_Register("rxind", "rxind", 0x0090fc90, 32, false, false);

        private void Arrays_Load(object sender, EventArgs e)
        {
            TB4.theArraysForm = this;
            try
            {
                txtStopAddr.Text = TB4.myRun.event_data_length.ToString();
            }
            catch
            {
                txtStopAddr.Text = "NA";
            }

            Size returnSize = this.Size;
            this.DockStateChanged += (object a, System.EventArgs b) => { TB4.thePADE_explorer.childChangedDockstate(this, returnSize); };
        }

        void Arrays_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            TB4.thePADE_explorer.childClosed(this);
            e.Cancel = true;


        }
        void Arrays_DockStateChanged(object sender, System.EventArgs e)
        {
            if (this.FloatPane != null) this.FloatPane.FloatWindow.Size = new Size(687, 668);
        }



        private void Generic_Ch_Read(ComboBox cmb_StartA, ComboBox cmb_Len, ref ListBox genericList, bool disp_byte = false, byte command_code = 0x02)
        {
            int i = 0; string t = "";
            byte A3 = 0; byte A2 = 0; byte A1 = 0; byte A0 = 0;
            Int32 StartA = 0;

            StartA = Convert.ToInt32(cmb_StartA.Text, 16);
            A3 = Convert.ToByte((StartA & 0xff000000) >> 24);
            A2 = Convert.ToByte((StartA & 0x00ff0000) >> 16);
            A1 = Convert.ToByte((StartA & 0x0000ff00) >> 8);
            A0 = Convert.ToByte((StartA & 0x000000ff));

            UInt16 data_len = 128;
            if (Convert.ToUInt16(cmb_Len.Text) > 4095)
            { data_len = 4095; }
            else if (Convert.ToUInt16(cmb_Len.Text) < 1)
            { data_len = 1; }
            else
            { data_len = Convert.ToUInt16(cmb_Len.Text); }

            int[] data = new int[4096];
            if (command_code == 0x03) { TB4.ReadArray(A3, A2, A1, A0, data_len, data, command_code = 0x03); }
            else { TB4.ReadArray(A3, A2, A1, A0, data_len, data); }

            if (genericList.Items.Count > data_len)
            {
                genericList.Items.Clear();
            }
            if (disp_byte)
            {
                for (i = 0; i < data_len; i++)
                {
                    int ub = (data[i] & 0xff00) >> 8;
                    int lb = (data[i] & 0x00ff);
                    t = Convert.ToString(2 * i + 1) + ": " + Convert.ToString(lb, 16);
                    if (genericList.Items.Count < i * 2 + 1)
                    { genericList.Items.Add(t); }
                    else
                    { genericList.Items[i * 2 + 1] = t; }

                    t = Convert.ToString(2 * i + 2) + ": " + Convert.ToString(ub, 16);
                    if (genericList.Items.Count < i * 2 + 2)
                    { genericList.Items.Add(t); }
                    else
                    { genericList.Items[i * 2 + 1] = t; }
                }
            }
            else
            {
                for (i = 0; i < data_len; i++)
                {
                    t = Convert.ToString(data[i], 16);
                    //if (t.Length == 0) { t = "000"; }
                    //if (t.Length == 1) { t = "00" + t; }
                    //if (t.Length == 2) { t = "0" + t; }
                    if (genericList.Items.Count < i + 1)
                    {
                        genericList.Items.Add(t);
                    }
                    else
                    {
                        genericList.Items[i] = t;
                    }
                }
            }
        }

        private void btn_Ch0_Click(object sender, EventArgs e)
        {
            Generic_Ch_Read(cmb_Start0, cmb_Len0, ref list0);
            list0.Refresh();
            int[] data = new int[4096];
            for (int i = 0; i < list0.Items.Count; i++)
            {
                data[i] = Convert.ToInt32(list0.Items[i].ToString(), 16);
            }
            //if (TB4.thePlot == null) { TB4.thePlot = new Plot0(); }
            //TB4.thePlot.Plot0_display(data, data, data,data);
            //TB4.thePlot.Show();
        }

        private void btn_Ch1_Click(object sender, EventArgs e)
        {
            Generic_Ch_Read(cmb_Start1, cmb_Len1, ref list1);
            list1.Refresh();
        }

        private void btn_Ch2_Click(object sender, EventArgs e)
        {
            Generic_Ch_Read(cmb_Start2, cmb_Len2, ref list2);
            list2.Refresh();
        }

        private void btn_Ch3_Click(object sender, EventArgs e)
        {
            Generic_Ch_Read(cmb_Start3, cmb_Len3, ref list3);
            list3.Refresh();
        }

        private void btn_Ch4_Click(object sender, EventArgs e)
        {
            Generic_Ch_Read(cmb_Start4, cmb_Len4, ref list4);
            list4.Refresh();
        }

        private void btn_Ch5_Click(object sender, EventArgs e)
        {
            Generic_Ch_Read(cmb_Start5, cmb_Len5, ref list5);
            list5.Refresh();
        }

        private void btn_Ch6_Click(object sender, EventArgs e)
        {
            Generic_Ch_Read(cmb_Start6, cmb_Len6, ref list6);
            list6.Refresh();
        }

        private void btn_Ch7_Click(object sender, EventArgs e)
        {
            Generic_Ch_Read(cmb_Start7, cmb_Len7, ref list7);
            list7.Refresh();
        }

        private void btn_MAC_RX_Click(object sender, EventArgs e)
        {
            //read

            TB4_Register RX_IND = new TB4_Register("RX_IND", "temp", 0x90FC90, 32, false, false);
            TB4_Register RX_CURP = new TB4_Register("RX_CURP", "temp", 0x90FC34, 32, false, false);
            TB4_Register RX = new TB4_Register("RX", "temp", 0x900000, 32, false, false);

            RX_IND.RegWrite(1);
            UInt16 junk = RX_IND.RegRead();
            System.Threading.Thread.Sleep(1);

            Generic_Ch_Read(cmb_RX_start, cmb_RX_Len, ref list_RX, true);

            RX_CURP.RegWrite(0);
            RX_IND.RegWrite(0);
        }

        private void Set_Addr(int c, int ch, int db)
        {
            Int32 addr = 0x04000000;
            String t = Convert.ToString(addr
                    + (0x00100000 * ch)
                    + (0x10000000 * db), 16);
            if (t.Length < 8)
            { t = "0x0" + t; }
            else
            { t = "0x" + t; }
            switch (c)
            {
                case 0: cmb_Start0.Text = t; break;
                case 1: cmb_Start1.Text = t; break;
                case 2: cmb_Start2.Text = t; break;
                case 3: cmb_Start3.Text = t; break;
                case 4: cmb_Start4.Text = t; break;
                case 5: cmb_Start5.Text = t; break;
                case 6: cmb_Start6.Text = t; break;
                case 7: cmb_Start7.Text = t; break;
            }

        }
        private void cmb_CH_Ch0_SelectedIndexChanged(object sender, EventArgs e)
        {
            int c = 0;
            int ch = Convert.ToInt32(cmb_CH_Ch0.Text);
            int db = Convert.ToInt32(cmb_DB_Ch0.Text);
            Set_Addr(c, ch, db);
        }

        private void cmb_DB_Ch0_SelectedIndexChanged(object sender, EventArgs e)
        {
            int c = 0;
            int ch = Convert.ToInt32(cmb_CH_Ch0.Text);
            int db = Convert.ToInt32(cmb_DB_Ch0.Text);
            Set_Addr(c, ch, db);
        }

        private void cmb_DB_Ch1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int c = 1;
            int ch = Convert.ToInt32(cmb_CH_Ch1.Text);
            int db = Convert.ToInt32(cmb_DB_Ch1.Text);
            Set_Addr(c, ch, db);
        }

        private void cmb_CH_Ch1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int c = 1;
            int ch = Convert.ToInt32(cmb_CH_Ch1.Text);
            int db = Convert.ToInt32(cmb_DB_Ch1.Text);
            Set_Addr(c, ch, db);

        }

        private void cmb_DB_Ch2_SelectedIndexChanged(object sender, EventArgs e)
        {
            int c = 2;
            int ch = Convert.ToInt32(cmb_CH_Ch2.Text);
            int db = Convert.ToInt32(cmb_DB_Ch2.Text);
            Set_Addr(c, ch, db);

        }

        private void cmb_CH_Ch2_SelectedIndexChanged(object sender, EventArgs e)
        {
            int c = 2;
            int ch = Convert.ToInt32(cmb_CH_Ch2.Text);
            int db = Convert.ToInt32(cmb_DB_Ch2.Text);
            Set_Addr(c, ch, db);

        }

        private void cmb_DB_Ch3_SelectedIndexChanged(object sender, EventArgs e)
        {
            int c = 3;
            int ch = Convert.ToInt32(cmb_CH_Ch3.Text);
            int db = Convert.ToInt32(cmb_DB_Ch3.Text);
            Set_Addr(c, ch, db);

        }

        private void cmb_CH_Ch3_SelectedIndexChanged(object sender, EventArgs e)
        {
            int c = 3;
            int ch = Convert.ToInt32(cmb_CH_Ch3.Text);
            int db = Convert.ToInt32(cmb_DB_Ch3.Text);
            Set_Addr(c, ch, db);

        }

        private void cmb_DB_Ch4_SelectedIndexChanged(object sender, EventArgs e)
        {
            int c = 4;
            int ch = Convert.ToInt32(cmb_CH_Ch4.Text);
            int db = Convert.ToInt32(cmb_DB_Ch4.Text);
            Set_Addr(c, ch, db);

        }

        private void cmb_CH_Ch4_SelectedIndexChanged(object sender, EventArgs e)
        {
            int c = 4;
            int ch = Convert.ToInt32(cmb_CH_Ch4.Text);
            int db = Convert.ToInt32(cmb_DB_Ch4.Text);
            Set_Addr(c, ch, db);

        }


        private void cmb_CH_Ch5_SelectedIndexChanged(object sender, EventArgs e)
        {
            int c = 5;
            int ch = Convert.ToInt32(cmb_CH_Ch5.Text);
            int db = Convert.ToInt32(cmb_DB_Ch5.Text);
            Set_Addr(c, ch, db);

        }

        private void cmb_DB_Ch5_SelectedIndexChanged(object sender, EventArgs e)
        {
            int c = 5;
            int ch = Convert.ToInt32(cmb_CH_Ch5.Text);
            int db = Convert.ToInt32(cmb_DB_Ch5.Text);
            Set_Addr(c, ch, db);

        }
        private void cmb_DB_Ch6_SelectedIndexChanged(object sender, EventArgs e)
        {
            int c = 6;
            int ch = Convert.ToInt32(cmb_CH_Ch6.Text);
            int db = Convert.ToInt32(cmb_DB_Ch6.Text);
            Set_Addr(c, ch, db);

        }

        private void cmb_CH_Ch6_SelectedIndexChanged(object sender, EventArgs e)
        {
            int c = 6;
            int ch = Convert.ToInt32(cmb_CH_Ch6.Text);
            int db = Convert.ToInt32(cmb_DB_Ch6.Text);
            Set_Addr(c, ch, db);

        }

        private void cmb_DB_Ch7_SelectedIndexChanged(object sender, EventArgs e)
        {
            int c = 7;
            int ch = Convert.ToInt32(cmb_CH_Ch7.Text);
            int db = Convert.ToInt32(cmb_DB_Ch7.Text);
            Set_Addr(c, ch, db);

        }

        private void cmb_CH_Ch7_SelectedIndexChanged(object sender, EventArgs e)
        {
            int c = 7;
            int ch = Convert.ToInt32(cmb_CH_Ch7.Text);
            int db = Convert.ToInt32(cmb_DB_Ch7.Text);
            Set_Addr(c, ch, db);

        }

        private void btn_MAC_TX_Click(object sender, EventArgs e)
        {
            uint addr = Convert.ToUInt32(cmb_TX_start.Text, 16);
            TB4_Register temp_TX = new TB4_Register("temp", "temp", addr, 32, false, false);
            for (int i = 0; i < list_TX.Items.Count; i++)
            {
                if ((i & 0x03) == 3)
                {
                    try
                    {
                        UInt16 b0 = Convert.ToUInt16(list_TX.Items[i - 3].ToString(), 16);
                        UInt16 b1 = Convert.ToUInt16(list_TX.Items[i - 2].ToString(), 16);
                        UInt16 b2 = Convert.ToUInt16(list_TX.Items[i - 1].ToString(), 16);
                        UInt16 b3 = Convert.ToUInt16(list_TX.Items[i - 0].ToString(), 16);
                        temp_TX.addr += ((uint)i & 0xfffffffc) + 2;
                        temp_TX.RegWrite((UInt16)(b3 * 256 + b2));
                        temp_TX.addr -= 2;
                        temp_TX.RegWrite((UInt16)(b1 * 256 + b0));

                    }
                    catch { }
                }
            }
        }



        private void btnClear_Click(object sender, EventArgs e)
        {
            list_TX.Items.Clear();

        }



        private void txt_TX_keydown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                list_TX.Items.Add(txt_TX_add.Text);
                txt_TX_add.Text = "";
            }
        }

        private void btn_DDR_Click(object sender, EventArgs e)
        {

            Generic_Ch_Read(cmb_Start0, cmb_Len0, ref list0, command_code: 0x3);
            list0.Refresh();
            int[] data = new int[4096];
            for (int i = 0; i < list0.Items.Count; i++)
            {
                data[i] = Convert.ToInt32(list0.Items[i].ToString(), 16);
            }
            //if (TB4.thePlot == null) { TB4.thePlot = new Plot0(); }
            //TB4.thePlot.Plot0_display(data, data, data,data);
            //TB4.thePlot.Show();
        }

        private void btn_UDP_Click(object sender, EventArgs e)
        {
            TB4_Register temp_reg = new TB4_Register("temp", "", 0x01100000, 16, true, false);
            listDebugData.Items.Clear();
            for (uint i = 0; i < 500; i++)
            {
                temp_reg.addr = 0x01100000 + i;
                UInt16 v = temp_reg.RegRead();
                string t = i.ToString() + ":" + Convert.ToString(v, 16);

                listDebugData.Items.Add(t);
            }
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            List<PADE> Slaves = PADE_explorer.getallPADE(PADE.type_of_PADE.slave);
            List<PADE> Master = PADE_explorer.getallPADE(PADE.type_of_PADE.crate_master); //need to check there is only one!


            foreach (PADE thisPADE in Slaves)
            {
                TB4.activatePADE(thisPADE);
                PADE_explorer.registerLookup("CONTROL_REG").RegWrite(0x240);
                PADE_explorer.registerLookup("CONTROL_REG").RegWrite(0x2c0);
                PADE_explorer.registerLookup("CONTROL_REG").RegWrite(0x0240);
                PADE_explorer.registerLookup("CONTROL_REG").RegWrite(0x02c0);

                PADE_explorer.registerLookup("SOFTWARE_RESET").RegWrite(1); //???this resets the ping and pong pointers, I guess
                Console.WriteLine("slave " + Convert.ToString(PADE_explorer.registerLookup("STATUS_REG").RegRead(), 16));
            }

            foreach (PADE thisPADE in Master)
            {
                TB4.activatePADE(thisPADE);
                PADE_explorer.registerLookup("CONTROL_REG").RegWrite(0x340);
                PADE_explorer.registerLookup("CONTROL_REG").RegWrite(0x3c0);
                PADE_explorer.registerLookup("CONTROL_REG").RegWrite(0x0340);
                PADE_explorer.registerLookup("CONTROL_REG").RegWrite(0x03c0);
                PADE_explorer.registerLookup("SOFTWARE_RESET").RegWrite(1); //???this resets the ping and pong pointers, I guess
                Console.WriteLine("master " + Convert.ToString(PADE_explorer.registerLookup("STATUS_REG").RegRead(), 16));
                PADE_explorer.registerLookup("SOFTWARE_TRIGGER").RegWrite(1);
                Console.WriteLine("master " + Convert.ToString(PADE_explorer.registerLookup("STATUS_REG").RegRead(), 16));
            }
            foreach (PADE thisPADE in Slaves)
            {
                TB4.activatePADE(thisPADE);
                Console.WriteLine("slave " + Convert.ToString(PADE_explorer.registerLookup("STATUS_REG").RegRead(), 16));
            }
        }

        private void btn_Cont_Test_Click(object sender, EventArgs e)
        {
            if (btn_Cont_Test.Text.Contains("STOP"))
            {
                btn_Cont_Test.Text = "CONT TEST";
                Application.DoEvents();
                timer1.Enabled = false;
                timer1.Interval = 50;
            }
            else
            {
                btn_Cont_Test.Text = "STOP";
                Application.DoEvents();
                timer1.Enabled = true;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            btnTest_Click(null, null);
        }

        

        //private void btn_UDP_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        TB4.myRun.event_data_length = Convert.ToInt32(this.txt_UDP_num_samples.Text);
        //    }
        //    catch
        //    {
        //        TB4.myRun.event_data_length = 90;
        //    }
        //    TB4.myRun.Take_UDP_Event();
        //    this.lblHdr1.Text = TB4.myRun.event_header[0].ToString();
        //    this.lblHdr2.Text = TB4.myRun.event_header[1].ToString();
        //    this.lblHdr3.Text = TB4.myRun.event_header[2].ToString();
        //    this.lblHdr4.Text = TB4.myRun.event_header[3].ToString();

        //    ListBox genericList=new ListBox();
        //    int data_len = TB4.myRun.event_data_length;//TB4.myRun.event_header[3];
        //    string t;

        //    for (int i = 0; i < 8; i++)
        //    {
        //        if (i == 0) { genericList = list0; }
        //        if (i == 1) { genericList = list1; }
        //        if (i == 2) { genericList = list2; }
        //        if (i == 3) { genericList = list3; }
        //        if (i == 4) { genericList = list4; }
        //        if (i == 5) { genericList = list5; }
        //        if (i == 6) { genericList = list6; }
        //        if (i == 7) { genericList = list7; }

        //        if (genericList.Items.Count > data_len)
        //        {
        //            genericList.Items.Clear();
        //        }
        //        for (int j = 0; j < data_len; j++)
        //        {
        //            t = Convert.ToString(TB4.myRun.event_data[j+(data_len*i)], 16);
        //            if (t.Length == 0) { t = "000"; }
        //            if (t.Length == 1) { t = "00" + t; }
        //            if (t.Length == 2) { t = "0" + t; }
        //            if (genericList.Items.Count < j + 1)
        //            {
        //                genericList.Items.Add(t);
        //            }
        //            else
        //            {
        //                genericList.Items[j] = t;
        //            }
        //        }
        //    }

        //    list0.Refresh();
        //    int[] data = new int[4096];
        //    for (int i = 0; i < list0.Items.Count; i++)
        //    {
        //        data[i] = Convert.ToInt32(list0.Items[i].ToString(), 16);
        //    }
        //    //if (TB4.thePlot == null) { TB4.thePlot = new Plot0(); }
        //    //TB4.thePlot.Plot0_display(data, data, data,data);
        //    //TB4.thePlot.Show();
        //}





    }
}
