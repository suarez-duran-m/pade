using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;



namespace PADE
{
    public partial class GBE : DockContent
    {
        GroupBox[] Box = new GroupBox[6];
        CheckBox[,] chk_bit = new CheckBox[6, 16];
        TextBox[] txt_ADDR = new TextBox[6];
        TextBox[] txt_DATA = new TextBox[6];
        NumericUpDown[] ud_ADDR = new NumericUpDown[6];
        Button[] btn_W = new Button[6];
        Button[] btn_R = new Button[6];
        byte[] TX = new byte[1500];
        byte[] RX = new byte[1500];
        TB4_Register TX_temp = new TB4_Register("TX_temp", "temp", 0x00908000, 16, false, false);
        TB4_Register RX_temp = new TB4_Register("TX_temp", "temp", 0x00900000, 16, false, false);

        public GBE()
        {
            InitializeComponent();
            Add_Controls();
        }

        public void Add_Controls()
        {
            //setup groupbox
            for (int i = 0; i < 6; i++)
            {
                this.Box[i] = new GroupBox();
                this.txt_ADDR[i] = new TextBox();
                this.txt_DATA[i] = new TextBox();
                this.ud_ADDR[i] = new NumericUpDown();
                this.btn_R[i] = new Button();
                this.btn_W[i] = new Button();

                //this.Box[i].SuspendLayout();
                if (i % 2 == 0) { this.Box[i].Location = new System.Drawing.Point(1, 45 + (i/2) * 130); }
                else { this.Box[i].Location = new System.Drawing.Point(335, 45 + (int) ((i-1)/2 * 130)); }
                this.Box[i].Name = "groupBox" + i.ToString();
                this.Box[i].Size = new System.Drawing.Size(331, 115);
                this.Box[i].TabStop = false;
                if (i < 5)
                { this.Box[i].Text = "MAC reg"; }
                else
                { this.Box[i].Text = "PHY reg"; }



                // 
                // ud_MAC0
                // 

                this.ud_ADDR[i].Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                this.ud_ADDR[i].Location = new System.Drawing.Point(51, 40);
                this.ud_ADDR[i].Name = "ud_ADDR" + i.ToString();
                this.ud_ADDR[i].Size = new System.Drawing.Size(20, 22);
                this.ud_ADDR[i].TabIndex = 4;
                this.ud_ADDR[i].Maximum = 60;
                this.ud_ADDR[i].Minimum = -1;
                this.ud_ADDR[i].Increment = 1;
                this.ud_ADDR[i].Value = 0;
                this.ud_ADDR[i].UpDownAlign = System.Windows.Forms.LeftRightAlignment.Left;
                this.ud_ADDR[i].ValueChanged += new EventHandler(ud_ValueChanged);
                this.ud_ADDR[i].Tag = i;
                if (i == 5) { this.ud_ADDR[i].Visible = false; }
                // 
                // txt_MAC0_ADDR
                // 
                this.txt_ADDR[i].Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                this.txt_ADDR[i].Location = new System.Drawing.Point(6, 40);
                this.txt_ADDR[i].Name = "txt_ADDR" + i.ToString();
                this.txt_ADDR[i].Size = new System.Drawing.Size(48, 22);
                this.txt_ADDR[i].TabIndex = 2;
                this.txt_ADDR[i].Text = "FC08";
                if (i == 5) { this.txt_ADDR[i].Text = "0"; }
                this.txt_ADDR[i].TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
                //this.txt_ADDR[i].TextChanged += new EventHandler(ADDR_changed);// 
                this.txt_ADDR[i].Tag = i;
                // txt_MAC0_DATA
                // 
                this.txt_DATA[i].Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                this.txt_DATA[i].Location = new System.Drawing.Point(86, 40);
                this.txt_DATA[i].Name = "txt_DATA" + i.ToString();
                this.txt_DATA[i].Size = new System.Drawing.Size(60, 22);
                this.txt_DATA[i].TabIndex = 5;
                this.txt_DATA[i].Text = "0000";
                this.txt_DATA[i].TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
                this.txt_DATA[i].LostFocus += new EventHandler(DATA_changed);

                this.txt_DATA[i].Tag = i;
                // 
                // btn_MAC0_R
                // 
                this.btn_R[i].Location = new System.Drawing.Point(152, 36);
                this.btn_R[i].Name = "btn_R" + i.ToString();
                this.btn_R[i].Size = new System.Drawing.Size(28, 31);
                this.btn_R[i].TabIndex = 1;
                this.btn_R[i].Text = "R";
                this.btn_R[i].UseVisualStyleBackColor = true;
                this.btn_R[i].Click += new EventHandler(R_click);
                this.btn_R[i].Tag = i;
                // 
                // btn_MAC0_W
                // 
                this.btn_W[i].Location = new System.Drawing.Point(186, 36);
                this.btn_W[i].Name = "btn_W" + i.ToString();
                this.btn_W[i].Size = new System.Drawing.Size(28, 31);
                this.btn_W[i].TabIndex = 0;
                this.btn_W[i].Text = "W";
                this.btn_W[i].UseVisualStyleBackColor = true;
                this.btn_W[i].Click += new EventHandler(W_click);
                this.btn_W[i].Tag = i;
                // 
                // chk_MAC0_b15
                // 
                for (int j = 0; j < 16; j++)
                {
                    this.chk_bit[i, j] = new CheckBox();
                    this.Box[i].Controls.Add(this.chk_bit[i, j]);
                    this.chk_bit[i, j].AutoSize = true;
                    this.chk_bit[i, j].CheckAlign = System.Drawing.ContentAlignment.TopCenter;
                    this.chk_bit[i, j].Location = new System.Drawing.Point(6 + (15 - j) * 20, 77);
                    this.chk_bit[i, j].Name = "chk_bit" + i.ToString() + "_" + j.ToString();
                    this.chk_bit[i, j].Size = new System.Drawing.Size(23, 31);
                    this.chk_bit[i, j].TabIndex = 8;
                    this.chk_bit[i, j].Text = j.ToString();
                    this.chk_bit[i, j].UseVisualStyleBackColor = true;
                    this.chk_bit[i, j].CheckedChanged += new EventHandler(bit_CheckedChanged);
                    this.chk_bit[i, j].Tag = (double)(j + (double)i / 10);
                }
                //this.Box[i].ResumeLayout(false);
                this.Controls.Add(Box[i]);
                this.Box[i].Controls.Add(this.txt_ADDR[i]);
                this.Box[i].Controls.Add(this.txt_DATA[i]);
                this.Box[i].Controls.Add(this.ud_ADDR[i]);
                this.Box[i].Controls.Add(this.btn_R[i]);
                this.Box[i].Controls.Add(this.btn_W[i]);
            }


            //// 
            //// label1
            //// 
            //this.label1.AutoSize = true;
            //this.label1.Location = new System.Drawing.Point(3, 24);
            //this.label1.Name = "label1";
            //this.label1.Size = new System.Drawing.Size(38, 13);
            //this.label1.TabIndex = 3;
            //this.label1.Text = "ADDR";
            //// 
            //// label2
            //// 
            //this.label2.AutoSize = true;
            //this.label2.Location = new System.Drawing.Point(86, 24);
            //this.label2.Name = "label2";
            //this.label2.Size = new System.Drawing.Size(36, 13);
            //this.label2.TabIndex = 6;
            //this.label2.Text = "DATA";

        }

        void ud_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown t = (NumericUpDown)sender;
            int i = (int)t.Tag;
            int j = (int)t.Value;
            if (j < 0) { j = 59; ud_ADDR[i].Value = j; }
            if (j > 59) { j = 0; ud_ADDR[i].Value = j; }
            UInt16 v = (UInt16)j;
            txt_ADDR[i].Text = Convert.ToString(0xfc00 + v * 4, 16);
        }

        private void bit_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox t = (CheckBox)sender;
            int i = (int)Math.Round(((double)t.Tag - Math.Truncate((double)t.Tag)) * 10, 0);
            double v = 0;
            for (int j = 0; j < 16; j++)
            {
                if (chk_bit[i, j].Checked) { v += Math.Pow(2, j); }
            }
            UInt16 vv = Convert.ToUInt16(v);
            txt_DATA[i].Text = "0x" + Convert.ToString(vv, 16);
        }

        private void R_click(object sender, EventArgs e)
        {
            Button t = (Button)sender;
            int i = (int)t.Tag;
            if (i == 5)
            {
                PHY_R();
            }
            else
            {
                UInt32 reg_addr = 0x900000 + Convert.ToUInt32(txt_ADDR[i].Text, 16);

                TB4_Register reg = new TB4_Register("temp", "temp", reg_addr, 32, false, false);
                UInt16 data = reg.RegRead();
                txt_DATA[i].Text = "0x" + Convert.ToString(data, 16);
                DATA_changed(txt_DATA[i], null);
            }
        }

        private void W_click(object sender, EventArgs e)
        {
            Button t = (Button)sender;
            int i = (int)t.Tag;
            if (i == 5)
            {
                PHY_W();
            }
            else
            {
                UInt32 reg_addr = 0x900000 + Convert.ToUInt32(txt_ADDR[i].Text, 16);

                TB4_Register reg = new TB4_Register("temp", "temp", reg_addr, 32, false, false);
                DATA_changed(txt_DATA[i], null);
                UInt16 data = Convert.ToUInt16(txt_DATA[i].Text, 16);
                reg.RegWrite(data);
            }
        }

        private void ADDR_changed(object sender, EventArgs e)
        {

        }

        private void DATA_changed(object sender, EventArgs e)
        {
            TextBox t = (TextBox)sender;
            int i = (int)(t.Tag);
            UInt16 v = 0;
            if (t.Text.Contains("0x") || t.Text.Contains("0X"))
            {
                t.Text = t.Text.Substring(2);
                v = Convert.ToUInt16(t.Text, 16);
                t.Text = "0x" + t.Text;
            }
            else
            {
                v = Convert.ToUInt16(t.Text);
            }
            for (int j = 0; j < 16; j++)
            {
                if ((v & (1 << j)) > 0) { chk_bit[i, j].Checked = true; }
                else { chk_bit[i, j].Checked = false; }
            }
        }

        private void PHY_W()
        {
            UInt32 phy_data = 0x90FC88;
            UInt32 phy_addr = 0x90FC84;

            TB4_Register phy_data_reg = new TB4_Register("phy_data", "temp", phy_data, 32, false, false);
            TB4_Register phy_addr_reg = new TB4_Register("phy_addr", "temp", phy_addr, 32, false, false);

            UInt16 addr = Convert.ToUInt16(txt_ADDR[5].Text, 16);
            UInt16 data = Convert.ToUInt16(txt_DATA[5].Text, 16);

            if (addr < 0x100)
            {
                //direct
                phy_data_reg.RegWrite(data);
                addr = (UInt16)(addr * 256 + 3 + 0x8000);
                phy_addr_reg.RegWrite(addr);
            }
            else
            {
                //indirect
                phy_data_reg.RegWrite(data);
                UInt16 iaddr = (UInt16)(0x8C03);
                phy_addr_reg.RegWrite(iaddr);

                UInt16 idata = (UInt16)((addr & 0x01FF) + 0x8000);
                phy_data_reg.RegWrite(idata);
                iaddr = (UInt16)(0x8B03);
                phy_addr_reg.RegWrite(iaddr);
            }

        }

        private void PHY_R()
        {
            UInt32 phy_data = 0x90FC88;
            UInt32 phy_addr = 0x90FC84;

            TB4_Register phy_data_reg = new TB4_Register("phy_data", "temp", phy_data, 32, false, false);
            TB4_Register phy_addr_reg = new TB4_Register("phy_addr", "temp", phy_addr, 32, false, false);

            UInt16 addr = Convert.ToUInt16(txt_ADDR[5].Text, 16);
            UInt16 data = Convert.ToUInt16(txt_DATA[5].Text, 16);

            if (addr < 0x100)
            {
                //direct
                addr = (UInt16)(addr * 256 + 3 + 0x4000);
                phy_addr_reg.RegWrite(addr);
                data = phy_data_reg.RegRead();
                txt_DATA[5].Text = "0x" + Convert.ToString(data, 16);
            }
            else
            {
                //indirect
                UInt16 idata = (UInt16)((addr & 0x01FF));
                UInt16 iaddr = (UInt16)(0x8B03);
                phy_data_reg.RegWrite(idata);
                phy_addr_reg.RegWrite(iaddr);
                System.Threading.Thread.Sleep(5);
                iaddr = (UInt16)(0x4D03);
                phy_addr_reg.RegWrite(iaddr);
                data = phy_data_reg.RegRead();
                txt_DATA[5].Text = "0x" + Convert.ToString(data, 16);
            }
            DATA_changed(txt_DATA[5], null);
        }

        private void btn_MAC_TX_Click(object sender, EventArgs e)
        {
            test_TX();
        }

        //public void test_TX()
        //{
        //    UInt16 dlen = 0;
        //    TB4_Register ISR_reg = new TB4_Register("ISR", "temp", 0x90FC08, 32, false, false);
        //    TB4_Register TX_CMD_reg = new TB4_Register("TX_CMD", "temp", 0x90FC14, 32, false, false);
        //    TB4_Register TX_DES0_reg = new TB4_Register("TX_DES0", "temp", 0x90FC20, 32, false, false);
        //    TB4_Register TX_DES1_reg = new TB4_Register("TX_DES1", "temp", 0x90FC24, 32, false, false);
        //    TB4_Register TX_DES2_reg = new TB4_Register("TX_DES2", "temp", 0x90FC28, 32, false, false);
        //    TB4_Register TX_DES3_reg = new TB4_Register("TX_DES3", "temp", 0x90FC2C, 32, false, false);

        //    byte[] TX = new byte[256];

        //    for (int i = 0; i < 256; i++)
        //    { TX[i] = (byte)(0x00); }

        //    openFileDialog1.Filter = "eth frame files|*.frame";
        //    if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
        //    {
        //        string TX_filename = openFileDialog1.FileName;
        //        rtxt_TX.LoadFile(TX_filename, RichTextBoxStreamType.PlainText);
        //        for (int i = 0; i < rtxt_TX.Lines.Length-1; i++)
        //        {
        //            try
        //            {
        //                if (rtxt_TX.Lines[i].Contains("0x") || rtxt_TX.Lines[i].Contains("0X"))
        //                {
        //                    TX[i] = Convert.ToByte(rtxt_TX.Lines[i], 16);
        //                }
        //                else
        //                {
        //                    TX[i] = Convert.ToByte(rtxt_TX.Lines[i]);
        //                }
        //            }
        //            catch
        //            {

        //                MessageBox.Show("Doh. Can not convert to byte.");
        //            }
        //        }
        //        dlen = (UInt16)rtxt_TX.Lines.Length;
        //    }
        //    else
        //    {
        //        TX[0] = 0xff;
        //        TX[1] = 0xff;
        //        TX[2] = 0xff;
        //        TX[3] = 0xff;
        //        TX[4] = 0xff;
        //        TX[5] = 0xff;
        //        TX[6] = 0x00;
        //        TX[7] = 0x12;
        //        TX[8] = 0x3f;
        //        TX[9] = 0x3d;
        //        TX[10] = 0xed;
        //        TX[11] = 0x01;
        //        TX[12] = 0x08;
        //        TX[13] = 0x06;
        //        TX[14] = 0x00;
        //        TX[15] = 0x01;
        //        TX[16] = 0x08;
        //        TX[17] = 0x00;
        //        TX[18] = 0x06;
        //        TX[19] = 0x04;
        //        TX[20] = 0x00;
        //        TX[21] = 0x01;
        //        TX[22] = 0x00;
        //        TX[23] = 0x12;
        //        TX[24] = 0x3f;
        //        TX[25] = 0x3d;
        //        TX[26] = 0xed;
        //        TX[27] = 0x00;
        //        TX[28] = 0x00;
        //        TX[29] = 0x00;
        //        TX[30] = 0x00;
        //        TX[31] = 0x00;
        //        TX[32] = 0x00;
        //        TX[33] = 0x00;
        //        TX[34] = 0x00;
        //        TX[35] = 0x00;
        //        TX[36] = 0x00;
        //        TX[35] = 0x00;
        //        TX[36] = 0xa9;
        //        TX[36] = 0xfe;
        //        TX[36] = 0xe3;
        //        TX[36] = 0x33;
        //        dlen = 0x35;
        //    }

        //    //for (int i = 18; i < 250; i++)
        //    ////{ TX[i] = (byte)(0xff & (i - 6)); }
        //    //{
        //    //    if ((i & 0x01) == 1) { TX[i] = 0x0f; } else { TX[i] = 0xf0; }
        //    //}

        //    //reset Interrupt

        //    UInt16 v = ISR_reg.RegRead();
        //    ISR_reg.RegWrite(v);
        //    for (int loop = 0; loop < 100; loop++)
        //    {
        //        //enable write to DES by setting TX_CMD
        //        TX_CMD_reg.RegWrite(0x8050);

        //        //write the data
        //        #region write

        //        uint addr = 0x908000;
        //        TB4_Register temp_TX = new TB4_Register("temp", "temp", addr, 32, false, false);
        //        for (int i = 0; i < TX.Length; i++)
        //        {
        //            if ((i & 0x03) == 3)
        //            {
        //                try
        //                {
        //                    UInt16 b0 = TX[i - 3];
        //                    UInt16 b1 = TX[i - 2];
        //                    UInt16 b2 = TX[i - 1];
        //                    UInt16 b3 = TX[i - 0];
        //                    temp_TX.addr += ((uint)i & 0xfffffffc) + 2;
        //                    temp_TX.RegWrite((UInt16)(b3 * 256 + b2));
        //                    temp_TX.addr -= 2;
        //                    temp_TX.RegWrite((UInt16)(b1 * 256 + b0));

        //                }
        //                catch { }
        //            }
        //        }
        //        #endregion write

        //        //send it
        //        TX_DES0_reg.RegWrite(0x803a);

        //        v = 0x8000;
        //        System.Threading.Thread.Sleep(1);
        //        while ((v & 0x8000) > 0)
        //        {
        //            v = TX_DES0_reg.RegRead();
        //            System.Threading.Thread.Sleep(5);
        //        }

        //        //disable write
        //        TX_CMD_reg.RegWrite(dlen);
        //    }

        //    R_click(btn_R[0], null);

        //    //read
        //    //list_RX.Items.Clear();
        //    //TB4_Register RX_IND = new TB4_Register("RX_IND", "temp", 0x90FC90, 32, false, false);
        //    //TB4_Register RX_CURP = new TB4_Register("RX_CURP", "temp", 0x90FC34, 32, false, false);
        //    //TB4_Register RX = new TB4_Register("RX", "temp", 0x900000, 32, false, false);
        //    //UInt32 rx_addr = 0x900000;

        //    //RX_IND.RegWrite(1);
        //    //for (int i = 0; i < 40; i++)
        //    //{
        //    //    rx_addr += (UInt32)i * 4;
        //    //    RX = new TB4_Register("RX", "temp", rx_addr, 32, false, false);
        //    //    UInt16 rxv = RX.RegRead();
        //    //    list_RX.Items.Add("0x" + Convert.ToString(rxv, 16));
        //    //}
        //    //RX_CURP.RegWrite(0);
        //    //RX_IND.RegWrite(0);


        //}
        public void test_TX()
        {
            UInt16 dlen = 0;


            for (int i = 0; i < 256; i++)
            { TX[i] = (byte)(0x00); }

            openFileDialog1.Filter = "eth frame files|*.frame";
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string TX_filename = openFileDialog1.FileName;
                rtxt_TX_dec.LoadFile(TX_filename, RichTextBoxStreamType.PlainText);
                for (int i = 0; i < rtxt_TX_dec.Lines.Length - 1; i++)
                {
                    try
                    {
                        if (rtxt_TX_dec.Lines[i].Contains("0x") || rtxt_TX_dec.Lines[i].Contains("0X"))
                        {
                            TX[i] = Convert.ToByte(rtxt_TX_dec.Lines[i], 16);
                        }
                        else if(rtxt_TX_dec.Lines[i].Contains(": "))
                        {
                            int from_pos = rtxt_TX_dec.Lines[i].IndexOf(": ");
                            TX[i] = Convert.ToByte(rtxt_TX_dec.Lines[i].Substring(from_pos+2), 16);
                        }
                        else 
                        {
                            TX[i] = Convert.ToByte(rtxt_TX_dec.Lines[i]);
                        }
                    }
                    catch
                    {

                        MessageBox.Show("Doh. Can not convert to byte.");
                    }
                }
                dlen = (UInt16)rtxt_TX_dec.Lines.Length;

            }
            else
            {
                TX[0] = 0xff;
                TX[1] = 0xff;
                TX[2] = 0xff;
                TX[3] = 0xff;
                TX[4] = 0xff;
                TX[5] = 0xff;
                TX[6] = 0x00;
                TX[7] = 0x12;
                TX[8] = 0x3f;
                TX[9] = 0x3d;
                TX[10] = 0xed;
                TX[11] = 0x01;
                TX[12] = 0x08;
                TX[13] = 0x06;
                TX[14] = 0x00;
                TX[15] = 0x01;
                TX[16] = 0x08;
                TX[17] = 0x00;
                TX[18] = 0x06;
                TX[19] = 0x04;
                TX[20] = 0x00;
                TX[21] = 0x01;
                TX[22] = 0x00;
                TX[23] = 0x12;
                TX[24] = 0x3f;
                TX[25] = 0x3d;
                TX[26] = 0xed;
                TX[27] = 0x00;
                TX[28] = 0x00;
                TX[29] = 0x00;
                TX[30] = 0x00;
                TX[31] = 0x00;
                TX[32] = 0x00;
                TX[33] = 0x00;
                TX[34] = 0x00;
                TX[35] = 0x00;
                TX[36] = 0x00;
                TX[35] = 0x00;
                TX[36] = 0xa9;
                TX[36] = 0xfe;
                TX[36] = 0xe3;
                TX[36] = 0x33;
                dlen = 0x35;
            }

            txt_TXlen.Text = dlen.ToString();

            uint addr = 0x00908000;
            TB4_Register temp_TX = new TB4_Register("temp", "temp", addr, 32, false, false);
            addr = 0x0090fc14;
            TB4_Register temp_reg14 = new TB4_Register("temp14", "temp", addr, 32, false, false);
            addr = 0x0090fc20;
            TB4_Register temp_reg20 = new TB4_Register("temp20", "temp", addr, 32, false, false);


            

            #region Descr0
            // write to descr 0
            UInt16 data = Convert.ToUInt16(0x8000 + dlen + 4);
            temp_reg14.RegWrite(data);

            UInt16 b0 = 0;
            UInt16 b1 = 0;
            UInt16 b2 = 0;
            UInt16 b3 = 0;

            for (int i = 0; i < dlen + 4; i++)
            {
                if ((i & 0x03) == 3)
                {
                    try
                    {
                        if (i < dlen)
                        {
                            b0 = TX[i - 3];
                            b1 = TX[i - 2];
                            b2 = TX[i - 1];
                            b3 = TX[i - 0];
                        }
                        else
                        {
                            b0 = 0;
                            b1 = 0;
                            b2 = 0;
                            b3 = 0;
                        }
                        temp_TX.addr += ((uint)i & 0xfffffffc) + 2;
                        temp_TX.RegWrite((UInt16)(b3 * 256 + b2));
                        temp_TX.addr -= 2;
                        temp_TX.RegWrite((UInt16)(b1 * 256 + b0));
                    }
                    catch { }
                }
            }
            data = Convert.ToUInt16(dlen);
            temp_reg14.RegWrite(data);
            //stop writing to descr 0 
            #endregion

            #region Descr1
            //// write to descr 1
            //data = Convert.ToUInt16(0xa000 + dlen + 4);
            //temp_reg14.RegWrite(data);

            //b0 = 0;
            //b1 = 0;
            //b2 = 0;
            //b3 = 0;

            //for (int i = 0; i < dlen + 4; i++)
            //{
            //    if ((i & 0x03) == 3)
            //    {
            //        try
            //        {
            //            if (i < dlen)
            //            {
            //                b0 = TX[i - 3];
            //                b1 = TX[i - 2];
            //                b2 = TX[i - 1];
            //                b3 = TX[i - 0];
            //            }
            //            else
            //            {
            //                b0 = 0;
            //                b1 = 0;
            //                b2 = 0;
            //                b3 = 0;
            //            }
            //            temp_TX.addr += ((uint)i & 0xfffffffc) + 2;
            //            temp_TX.RegWrite((UInt16)(b3 * 256 + b2));
            //            temp_TX.addr -= 2;
            //            temp_TX.RegWrite((UInt16)(b1 * 256 + b0));
            //        }
            //        catch { }
            //    }
            //}
            //data = Convert.ToUInt16(0x2000+dlen + 4);
            //temp_reg14.RegWrite(data);
            ////stop writing to descr 1 
            #endregion


            //temp_reg20.addr = 0x90fc24;
            //data = Convert.ToUInt16(0x8000 + dlen + 4);
            //temp_reg20.RegWrite(data);

            temp_reg20.addr = 0x90fc20;
            //data = Convert.ToUInt16(0x8000 + dlen + 4);
            data = Convert.ToUInt16(0x8000 + dlen-4 ); 
            temp_reg20.RegWrite(data);
        }

        private void btn_MAC_RX_Click(object sender, EventArgs e)
        {
            //txt_RXlen.Text;
            //read

            TB4_Register RX_IND = new TB4_Register("RX_IND", "temp", 0x90FC90, 32, false, false);
            TB4_Register RX_CURP = new TB4_Register("RX_CURP", "temp", 0x90FC34, 32, false, false);
            TB4_Register RX = new TB4_Register("RX", "temp", 0x900000, 32, false, false);

            RX_IND.RegWrite(1);
            UInt16 junk = RX_IND.RegRead();
            System.Threading.Thread.Sleep(1);

            Generic_Read((Int32)0x00900000, (UInt16)600, ref list_RX,true,true  );

            RX_CURP.RegWrite(0);
            RX_IND.RegWrite(0);
        }

        private void Generic_Read(Int32 StartA, UInt16 Len, ref ListBox genericList, bool disp_byte = false, bool save_file=true)
        {
            int i = 0; string t = "";
            byte A3 = 0; byte A2 = 0; byte A1 = 0; byte A0 = 0;
            
            A3 = Convert.ToByte((StartA & 0xff000000) >> 24);
            A2 = Convert.ToByte((StartA & 0x00ff0000) >> 16);
            A1 = Convert.ToByte((StartA & 0x0000ff00) >> 8);
            A0 = Convert.ToByte((StartA & 0x000000ff));

            UInt16 data_len = Len;

            int[] data = new int[data_len];
            TB4.ReadArray(A3, A2, A1, A0, data_len, data);
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

            if (save_file)
            {
                saveFileDialog1.Filter = "raw frame files|*.mac_raw";
                if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    System.IO.Stream myStream;
                    
                    if ((myStream = saveFileDialog1.OpenFile()) != null)
                    {
                        System.IO.StreamWriter myFile=new System.IO.StreamWriter(myStream);
                        for (i = 0; i < genericList.Items.Count; i++)
                        {
                            myFile.WriteLine(genericList.Items[i]);
                        }
                        myFile.Close();
                        myStream.Close();
                    }


                }
            }
        }

        private void GBE_Load(object sender, EventArgs e)
        {
            TB4.theGBE = this;
            Size returnSize=this.Size;
            this.DockStateChanged += (object a, System.EventArgs b) => { TB4.thePADE_explorer.childChangedDockstate(this, returnSize); };
        }

        void GBE_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            TB4.thePADE_explorer.childClosed(this);
            e.Cancel = true;
        }

        private void rtxt_TX_dec_TextChanged(object sender, EventArgs e)
        {

        }

        private void list_RX_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        public void changeLocation(Point newloc)
        {
            this.Location = newloc;
        }
    }

}
