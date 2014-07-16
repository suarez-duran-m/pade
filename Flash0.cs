using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.IO;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace PADE
{

    public partial class Flash0 : DockContent
    {
        private bool debug_flash = true;
        //public TextBox[,] byte_box = new TextBox[8, 33];
        public bool flgShowExpertControls = false;
        public byte[] _Page = new byte[513];
        public UInt16 _Page_num = 0;
        System.Windows.Forms.Label[,] byte_box = new Label[8, 64];
        int current_i, current_j = 0;


        public Flash0()
        {
            InitializeComponent();
            //TB4.theFlash.Height = 210;
            this.Load += new System.EventHandler(this.Flash0_Load);
        }

        private void Flash0_Load(object sender, EventArgs e)
        {
            TB4.theFlash = this;
            TB4.theFlash.Height = 197;
            AssignFlashRegisters("MB");
            showChartHeaders(false);
            Size returnSize = this.Size;
            this.DockStateChanged += (object a, System.EventArgs b) => { TB4.thePADE_explorer.childChangedDockstate(this, returnSize); };
        }
        void Flash0_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            TB4.thePADE_explorer.childClosed(this);
            e.Cancel = true;
        }

        private void AssignFlashRegisters(string BoardName)
        {
            #region FIND SPECIAL FLASH REGISTERS
            for (int i = 1; i < TB4.TB4_Registers.Length; i++)
            {
                if (TB4.TB4_Registers[i] != null)
                {
                    if ((TB4.TB4_Registers[i].name.Contains("FLASH_CONTROL")))
                    {//found
                        TB4.regFLASH_CONTROL = TB4.TB4_Registers[i];
                        break;
                    }
                }
            }
            for (int i = 1; i < TB4.TB4_Registers.Length; i++)
            {
                if (TB4.TB4_Registers[i] != null)
                {
                    if ((TB4.TB4_Registers[i].name.Contains("FLASH_OP_CODE")))
                    {//found
                        TB4.regFLASH_OP_CODE = TB4.TB4_Registers[i];
                        break;
                    }
                }
            }
            for (int i = 1; i < TB4.TB4_Registers.Length; i++)
            {
                if (TB4.TB4_Registers[i] != null)
                {
                    if ((TB4.TB4_Registers[i].name.Contains("FLASH_PAGE_ADDR")))
                    {//found
                        TB4.regFLASH_PAGE_ADDR = TB4.TB4_Registers[i];
                        break;
                    }
                }
            }
            for (int i = 1; i < TB4.TB4_Registers.Length; i++)
            {
                if (TB4.TB4_Registers[i] != null)
                {
                    if ((TB4.TB4_Registers[i].name.Contains("FLASH_BYTE_ADDR")))
                    {//found
                        TB4.regFLASH_BYTE_ADDR = TB4.TB4_Registers[i];
                        break;
                    }
                }
            }

            //if (BoardName == "MB")
            //{ TB4.regFLASH_DPRAM_BASE = TB4.regFLASH_DPRAM_BASE_MB; }
            //else if (BoardName == "DB0")
            { TB4.regFLASH_DPRAM_BASE = TB4.regFLASH_DPRAM_BASE_DB0; }
            //else if (BoardName == "DB1")
            //{ TB4.regFLASH_DPRAM_BASE = TB4.regFLASH_DPRAM_BASE_DB1; }
            //else if (BoardName == "DB2")
            //{ TB4.regFLASH_DPRAM_BASE = TB4.regFLASH_DPRAM_BASE_DB2; }
            //else if (BoardName == "DB3")
            //{ TB4.regFLASH_DPRAM_BASE = TB4.regFLASH_DPRAM_BASE_DB3; }

            #endregion FIND SPECIAL FLASH REGISTERS

        }

        private void ExpertControls(bool show)
        {
            showChartHeaders(show);

            if (show == true)
            {
                TB4.theFlash.Height = 890;

                for (int j = 0; j < 64; j++)
                {
                    for (int i = 0; i < 8; i++)
                    {
                        int k = (j * 8) + i;
                        byte_box[i, j] = new Label();
                        byte_box[i, j].BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                        byte_box[i, j].CausesValidation = false;
                        byte_box[i, j].Location = new System.Drawing.Point(40 + (50 * i), 260 + (20 * j));
                        if (i > 3) byte_box[i, j].Location = new System.Drawing.Point(60 + (50 * i), 260 + (20 * j));
                        byte_box[i, j].Name = "byte_box" + k.ToString();
                        byte_box[i, j].Size = new System.Drawing.Size(50, 20);
                        byte_box[i, j].TabIndex = 19;
                        byte_box[i, j].Text = k.ToString();
                        byte_box[i, j].TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                        byte_box[i, j].Click += new EventHandler(byte_box_Click);
                        this.Controls.Add(byte_box[i, j]);
                        byte_box[i, j].SendToBack();
                        byte_box[i, j].Tag = k;
                    }
                }
            }
            else
            {
                TB4.theFlash.Height = 225;
                for (int j = 0; j < 64; j++)
                {
                    for (int i = 0; i < 8; i++)
                    {
                        byte_box[i, j].Dispose();
                    }
                }
            }
        }

        /// <summary>
        /// reads a _Page(264 bytes) from FLASH to DPRAM
        /// </summary>
        /// <param name="_Page_num">
        /// Flash _Page num
        /// </param>
        //public void ReadFlash_PageMB(UInt16 _Page_num)
        //{
        //    UInt16 Page_min = 0;
        //    UInt16 Page_max = 0xcff;
        //    int[] rdata = new int[4096];
        //    int t = 0;
        //    //int timeout = 10;
        //    TB4.regFLASH_DPRAM_BASE.addr = TB4.regFLASH_DPRAM_BASE_DB0.addr;
        //    TB4_Register regFLASH_DPRAM = new TB4_Register("name", "comment", 0, 16, false, false);
        //    regFLASH_DPRAM.addr = TB4.regFLASH_DPRAM_BASE.addr;
        //    TB4.regFLASH_CONTROL.RegWrite(0);
        //    //setup FLASH_Page_ADDR
        //    if ((_Page_num >= Page_min) && (_Page_num <= Page_max))
        //    {
        //        TB4.regFLASH_PAGE_ADDR.RegWrite(_Page_num);
        //    }
        //    else
        //    {

        //    }
        //    TB4.regFLASH_BYTE_ADDR.RegWrite(0);
        //    //FLASH_OP_CODE = 0x0B to make it read a _Page
        //    TB4.regFLASH_OP_CODE.RegWrite(0x0B);
        //    //FLASH_CONTROL=1 to make it go
        //    if (Flash_FSM_Complete())
        //    {//success
        //        for (int sub_page = 0; sub_page < 2; sub_page++)
        //        {
        //            //TB4.TB4_Registers[301] = TB4.regFLASH_DPRAM_BASE;
        //            ushort len = 256;
        //            {
        //                UInt32 Addr = TB4.regFLASH_DPRAM_BASE.addr + (UInt32)(0x100 * sub_page);
        //                byte A3, A2, A1, A0 = 0;
        //                A3 = Convert.ToByte((Addr & 0xff000000) >> 24);
        //                A2 = Convert.ToByte((Addr & 0x00ff0000) >> 16);
        //                A1 = Convert.ToByte((Addr & 0x0000ff00) >> 8);
        //                A0 = Convert.ToByte((Addr & 0x000000ff) >> 0);

        //                if (len > 4095) { len = 4095; TB4_Exception.logConsoleOnly("Long Length."); }
        //                else if (len < 1) { len = 1; TB4_Exception.logConsoleOnly("Short Length."); }

        //                int[] data = new int[1024];
        //                TB4.ReadArray(A3, A2, A1, A0, len, data);
        //                //TB4_Exception.logConsoleOnly(sub_page+","+data[255]);
        //                data.CopyTo(rdata, 0x100 * sub_page);
        //            }
        //        }

        //        for (uint i = 0; i < 512; i++)
        //        {
        //            _Page[i] = Convert.ToByte(rdata[i]);
        //        }
        //        //TB4.regFLASH_CONTROL_MB.RegWrite(0);
        //    }
        //    else
        //    { t++; }
        //}

        //public void ReadFlash_buffer_ram()
        //{
        //    int[] rdata = new int[4096];
        //    for (int sub_page = 0; sub_page < 2; sub_page++)
        //    {
        //        //TB4.TB4_Registers[301] = TB4.regFLASH_DPRAM_BASE;
        //        ushort len = 256;
        //        {
        //            UInt32 Addr = TB4.regFLASH_DPRAM_BASE.addr + (UInt32)(0x100 * sub_page);
        //            byte A3, A2, A1, A0 = 0;
        //            A3 = Convert.ToByte((Addr & 0xff000000) >> 24);
        //            A2 = Convert.ToByte((Addr & 0x00ff0000) >> 16);
        //            A1 = Convert.ToByte((Addr & 0x0000ff00) >> 8);
        //            A0 = Convert.ToByte((Addr & 0x000000ff) >> 0);

        //            if (len > 4095) { len = 4095; TB4_Exception.logConsoleOnly("Long Length."); }
        //            else if (len < 1) { len = 1; TB4_Exception.logConsoleOnly("Short Length."); }

        //            int[] data = new int[1024];
        //            TB4.ReadArray(A3, A2, A1, A0, len, data);
        //            //TB4_Exception.logConsoleOnly(sub_page+","+data[255]);
        //            data.CopyTo(rdata, 0x100 * sub_page);
        //        }
        //    }

        //    for (uint i = 0; i < 512; i++)
        //    {
        //        _Page[i] = Convert.ToByte(rdata[i]);
        //    }
        //}

        ///// <summary>
        ///// write a _Page(264 bytes) from DRAM to FLASH
        ///// </summary>
        ///// <param name="_Page_num">
        ///// Flash _Page num
        ///// </param>
        ///// 
        //public void WriteFlash_PageMB(UInt16 _Page_num)
        //{
        //    UInt16 Page_min = 0;
        //    UInt16 Page_max = 0xcff;
        //    int[] rdata = new int[4096];
        //    UInt16 poll_stat = 0;
        //    TB4_Register regFLASH_DPRAM = new TB4_Register("name", "comment", 0, 16, false, false);
        //    regFLASH_DPRAM.addr = TB4.regFLASH_DPRAM_BASE.addr;
        //    //regFLASH_DPRAM.addr = 0x088000FF;
        //    TB4.regFLASH_BYTE_ADDR.RegWrite(0);
        //    TB4.regFLASH_CONTROL.RegWrite(0);
        //    //setup FLASHPage_ADDR
        //    if ((_Page_num >= Page_min) && (_Page_num <= Page_max))
        //    {
        //        TB4.regFLASH_PAGE_ADDR.RegWrite((ushort)(_Page_num));
        //    }
        //    TB4.regFLASH_BYTE_ADDR.RegWrite(0);
        //    //copy the _Page from the computer to DPRAM
        //    int[] P = new int[512];
        //    for (uint i = 0; i < 512; i++)
        //    {
        //        regFLASH_DPRAM.addr = TB4.regFLASH_DPRAM_BASE.addr + i;
        //        regFLASH_DPRAM.RegWrite(Convert.ToUInt16(_Page[i]));
        //        P[i] = _Page[i];
        //        lblByte.Text = (i + 1).ToString() + "/512";
        //        //TB4_Exception.logConsoleOnly(i.ToString() + " : " + P[i].ToString());
        //        //Application.DoEvents();
        //    }

        //    //FLASH_OP_CODE = 0x82 to make it write a _Page
        //    TB4.regFLASH_OP_CODE.RegWrite(0x82);

        //    if (Flash_FSM_Complete() != true)
        //    { return; }

        //    TB4.regFLASH_DPRAM_BASE.RegWrite(0);
        //    System.Threading.Thread.Sleep(10);
        //    poll_stat = TB4.regFLASH_DPRAM_BASE.RegRead();
        //    // this is the flash status register. Bit 7 is the busy bit (0=busy)
        //    TB4.regFLASH_OP_CODE.RegWrite(0xD7);

        //    int poll_try = 0;
        //    int max_try = 100;
        //    while (poll_try < max_try)
        //    {
        //        if (Flash_FSM_Complete() != true) { return; }
        //        {//now we need to poll the FLASH itself
        //            poll_stat = TB4.regFLASH_DPRAM_BASE.RegRead();
        //            if ((poll_stat & 0x80) != 0)
        //            {//success!
        //                poll_try = max_try + 1;
        //            }
        //            {//failure
        //                poll_try++;
        //            }
        //        }
        //    }

        //    //For troubleshooting
        //    ReadFlash_PageMB(_Page_num);
        //    for (int k = 0; k < 512; k++)
        //    {
        //        lblByte.Text = "verifying: " + (k + 1).ToString() + "/512";
        //        if (P[k] != _Page[k])
        //        {
        //            MessageBox.Show("Flash not verified.");
        //            return;
        //        }
        //    }
        //    //*******************
        //}

        //public void WriteFlashFast_PageMB(UInt16 _Page_num)
        //{
        //    UInt16 Page_min = 0x0;
        //    UInt16 Page_max = 0xcff;
        //    int[] rdata = new int[4096];
        //    UInt16 poll_stat = 0;
        //    TB4_Register regFLASH_DPRAM = new TB4_Register("name", "comment", 0, 16, false, false);
        //    regFLASH_DPRAM.addr = TB4.regFLASH_DPRAM_BASE.addr;
        //    //regFLASH_DPRAM.addr = 0x088000FF;
        //    TB4.regFLASH_BYTE_ADDR.RegWrite(0);
        //    TB4.regFLASH_CONTROL.RegWrite(0);
        //    //setup FLASHPage_ADDR
        //    if ((_Page_num >= Page_min) && (_Page_num <= Page_max))
        //    {
        //        TB4.regFLASH_PAGE_ADDR.RegWrite((ushort)(_Page_num));
        //    }
        //    TB4.regFLASH_BYTE_ADDR.RegWrite(0);
        //    //copy the _Page from the computer to DPRAM
        //    ushort[] P = new ushort[512];

        //    for (uint i = 0; i < 512; i++)
        //    {
        //        //regFLASH_DPRAM.addr = TB4.regFLASH_DPRAM_BASE.addr + i;
        //        //regFLASH_DPRAM.RegWrite(Convert.ToUInt16(_Page[i]));
        //        P[i] = _Page[i];
        //        //lblByte.Text = (i + 1).ToString() + "/512";
        //        //Application.DoEvents();
        //    }
        //    regFLASH_DPRAM.RegWriteArray(P);
        //    System.Threading.Thread.Sleep(1);

        //    for (uint i = 256; i < 512; i++)
        //    {
        //        //regFLASH_DPRAM.addr = TB4.regFLASH_DPRAM_BASE.addr + i;
        //        //regFLASH_DPRAM.RegWrite(Convert.ToUInt16(_Page[i]));
        //        P[i - 256] = _Page[i];
        //        //lblByte.Text = (i + 1).ToString() + "/512";
        //        //Application.DoEvents();
        //    }

        //    regFLASH_DPRAM.addr = TB4.regFLASH_DPRAM_BASE.addr + 256;
        //    regFLASH_DPRAM.RegWriteArray(P);
        //    System.Threading.Thread.Sleep(1);

        //    //FLASH_OP_CODE = 0x82 to make it write a _Page
        //    TB4.regFLASH_OP_CODE.RegWrite(0x82);

        //    if (Flash_FSM_Complete() != true)
        //    { return; }

        //    TB4.regFLASH_DPRAM_BASE.RegWrite(0);
        //    System.Threading.Thread.Sleep(1);
        //    //poll_stat = TB4.regFLASH_DPRAM_BASE.RegRead();
        //    TB4.regFLASH_OP_CODE.RegWrite(0xD7);

        //    int poll_try = 0;
        //    int max_try = 500;
        //    while (poll_try < max_try)
        //    {
        //        if (Flash_FSM_Complete() != true) { return; }
        //        {//now we need to poll the FLASH itself
        //            poll_stat = TB4.regFLASH_DPRAM_BASE.RegRead();
        //            if ((poll_stat & 0x80) != 0)
        //            {//success!
        //                break;
        //            }
        //            {//failure
        //                poll_try++;
        //            }
        //        }
        //    }
        //    TB4_Exception.logConsoleOnly("Poll try= " + poll_try);


        //    ////For troubleshooting
        //    //ReadFlash_PageMB(_Page_num);
        //    //for (int k = 0; k < 512; k++)
        //    //{
        //    //    lblByte.Text = "verifying: " + (k + 1).ToString() + "/512";
        //    //    if (P[k] != _Page[k])
        //    //    {
        //    //        MessageBox.Show("Flash not verified.");
        //    //        return;
        //    //    }
        //    //}
        //}

        private void byte_box_Click(object sender, EventArgs e)
        {
            Label thisBox = new Label();
            thisBox = (Label)sender;
            int k = (int)thisBox.Tag;
            int div = 8;
            Math.DivRem(k, div, out current_i);
            current_j = (k - current_i) / div;
            txtByte.Location = thisBox.Location;
            TB4_Exception.logConsoleOnly("byte_box_Click " + txtByte.Location);
            txtByte.Text = thisBox.Text;
            txtByte.Show();
            txtByte.Visible = true;
            txtByte.BringToFront();
        }

        private void txtByte_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            { //got it!
                txtByte_TextChanged(sender, null);
            }
        }

        private void txtByte_TextChanged(object sender, EventArgs e)
        {
            byte b = 0;
            try
            {
                try
                { b = Convert.ToByte(txtByte.Text, 16); }
                catch
                { b = Convert.ToByte(txtByte.Text); }
            }
            catch
            {
                if (radioHEXorDEC.Checked)
                { b = Convert.ToByte(byte_box[current_i, current_j].Text, 16); }
                else
                { b = Convert.ToByte(byte_box[current_i, current_j].Text); };
            }
            finally
            {
                if (radioHEXorDEC.Checked)
                {
                    byte_box[current_i, current_j].Text = Convert.ToString(b, 16);
                    txtByte.Visible = false;
                }
                else
                {
                    byte_box[current_i, current_j].Text = b.ToString();
                    txtByte.Visible = false;
                }
                int k = (current_j * 8) + current_i;
                _Page[k] = b;
            }
        }

        //private void udStartingPage_ValueChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        _Page_num = Convert.ToUInt16(udStarting_Page.Value);
        //    }
        //    catch
        //    {
        //        _Page_num = 0;
        //        udStarting_Page.Value = 0;
        //    }
        //    lblHex_Page.Text = Convert.ToString(_Page_num, 16);
        //}


        //private bool Flash_FSM_Complete()
        //{
        //    //FLASH_CONTROL=1 to make it go
        //    TB4.regFLASH_CONTROL.RegWrite(0);
        //    TB4.regFLASH_CONTROL.RegWrite(1);
        //    UInt16 stat = 0;
        //    int k = 0;
        //    int timeout = 10;
        //    while ((stat == 0) && (k < timeout))
        //    {
        //        stat = TB4.regFLASH_CONTROL.RegRead();
        //        stat = Convert.ToUInt16(stat & 0x0f);
        //        System.Threading.Thread.Sleep(1);
        //        k++;
        //        if (k == timeout)
        //        {
        //            TB4_Exception.logConsoleOnly("Flash_FSM_Complete K timeout");
        //            TB4_Exception.logError(null, "K timeout", true);
        //        }
        //    }
        //    //wait until FLASH_CONTROL=3
        //    if (stat == 3)
        //    { return true; }
        //    else
        //    { return false; }
        //}

        public void SaveFile(string fname, UInt16 Starting_Page, UInt16 Ending_Page)
        {
            FlashData myFlashData = new FlashData();
            myFlashData.MaxPageNum = 0x0bff;
            myFlashData.ReadDatafromPADE();
            myFlashData.WriteDataToFile(fname);
        }

        //public void oldSaveFile(string fname, UInt16 Starting_Page, UInt16 Ending_Page)
        //{
        //    UInt16 my_Page_num = 0;
        //    FileStream file;
        //    StreamWriter sw;
        //    try
        //    {
        //        // Specify file, instructions, and privelegdes
        //        file = new FileStream(fname, FileMode.Create, FileAccess.Write);
        //        // Create a new stream to read from a file
        //        sw = new StreamWriter(file);
        //    }
        //    catch (Exception ex)
        //    {
        //        TB4_Exception.logError(ex, "Could not open file to write.", true);
        //        return;
        //    }

        //    my_Page_num = Starting_Page;

        //    Progress0 myProgress = new Progress0();
        //    myProgress.Visible = false;
        //    myProgress.timer1.Enabled = true;
        //    myProgress.timer1.Interval = 1000;
        //    myProgress.lblAction.Text = "Saving FLASH to File";
        //    myProgress.progressBar1.Maximum = Ending_Page + 1;

        //    while (my_Page_num <= Ending_Page)
        //    {
        //        //read the _Page 
        //        ReadFlash_PageMB(my_Page_num);
        //        //check that it is not all ff's

        //        string s = "";
        //        int k = 0;
        //        //write it to file
        //        //s = "-p " + my_Page_num.ToString() + " Begin";
        //        //sw.WriteLine(s);
        //        for (int j = 0; j < 64; j++)
        //        {
        //            s = ":" + Convert.ToString(my_Page_num, 16) + ":" + Convert.ToString(j, 16) + ": ";
        //            string st = "";
        //            for (int i = 0; i < 8; i++)
        //            {
        //                k = (j * 8) + i;
        //                st = Convert.ToString(_Page[k], 16);
        //                if (st.Length == 1) { st = "0" + st; }
        //                s += " " + st;
        //            }
        //            sw.WriteLine(s);
        //        }
        //        my_Page_num++;
        //        //if (my_Page_num.ToString() == (Convert.ToInt16((my_Page_num / 25)) * 25).ToString())
        //        {//report progress
        //            myProgress.Prog = "0x" + Convert.ToString(my_Page_num, 16) + " out of " + Convert.ToString(Ending_Page, 16);
        //            myProgress.val = my_Page_num;
        //            Application.DoEvents();
        //        }

        //    }
        //    sw.Close();
        //    file.Close();
        //    this.label1.Text = "file write done";
        //    myProgress.TopMost = false;
        //    myProgress.Visible = false;
        //    myProgress.timer1.Enabled = false;
        //    myProgress = null;

        //}

        public void FlashFile(string fname)
        {
            FlashData myFlashData = new FlashData();
            FlashData readbackFlashData = new FlashData();
            myFlashData.ReadDataFromFile(fname);
            myFlashData.WriteDatatoPADE(myFlashData.MinPageNum, myFlashData.MaxPageNum);
            //readbackFlashData.ReadDatafromPADE();
            //myFlashData.CompareFlashData(readbackFlashData);
        }

        //public void FlashFileOld(string fname)
        //{
        //    DateTime[] tb = new DateTime[4];
        //    DateTime[] te = new DateTime[4];
        //    TimeSpan[] ts = new TimeSpan[4];
        //    double[] t = new double[4];
        //    double[] t_tot = new double[4];

        //    int max_num_tries = 7;
        //    int num_tries = 0;

        //    UInt16 my_Page_num = 0;
        //    UInt16 max_page_num = 0xc00;
        //    byte[] P = new byte[513];
        //    byte[] V = new byte[513];
        //    FileStream file;
        //    StreamReader sr;

        //    Progress0 myProgress = new Progress0();
        //    myProgress.Visible = false;
        //    myProgress.timer1.Enabled = true;
        //    myProgress.timer1.Interval = 1000;
        //    myProgress.lblAction.Text = "Burning File to FLASH";
        //    myProgress.progressBar1.Maximum = 0xc00;
        //    myProgress.progressBar1.Visible = false;

        //    try
        //    {
        //        // Specify file, instructions, and priveleges
        //        file = new FileStream(fname, FileMode.Open, FileAccess.Read);
        //        // Create a new stream to read from a file
        //        sr = new StreamReader(file);
        //    }
        //    catch (Exception ex)
        //    {
        //        TB4_Exception.logError(ex, "Could not open file for read.", true);
        //        return;
        //    }

        //    string[] delimeter = new string[40];
        //    string[] tokens = new string[40];
        //    byte[] line_o_bytes = new byte[40];
        //    byte[,] byte_tokens = new byte[8, 33];
        //    int i = 0;
        //    int j = 0;
        //    delimeter[0] = " ";
        //    delimeter[1] = ":";
        //    delimeter[2] = "\t";
        //    DateTime start = DateTime.Now;
        //    bool start_nill = true;

        //    while (sr.EndOfStream == false)
        //    {
        //        tb[0] = DateTime.Now;
        //        string s = sr.ReadLine();
        //        s = s.ToLower();
        //        //:0:0:  ff ff ff ff aa 99 55 66
        //        tokens = s.Split(delimeter, StringSplitOptions.RemoveEmptyEntries);
        //        try
        //        {
        //            my_Page_num = Convert.ToUInt16(tokens[0], 16);
        //            j = Convert.ToUInt16(tokens[1], 16);
        //            for (int ind = 2; ind < 10; ind++)
        //            { line_o_bytes[ind - 2] = Convert.ToByte(tokens[ind], 16); }
        //        }
        //        catch
        //        {
        //            my_Page_num = 0xc00;
        //            myProgress.TopMost = false;
        //            myProgress.Visible = false;
        //            myProgress.timer1.Enabled = false;

        //            return;

        //        }

        //        for (i = 0; i < 8; i++)
        //        {
        //            int k = (j * 8) + i;
        //            _Page[k] = line_o_bytes[i];  //page stored here
        //            P[k] = _Page[k];//local copy
        //            V[k] = _Page[k];
        //        }

        //        if (j == 0x3f)
        //        {
        //            if (start_nill) { start = DateTime.Now; start_nill = false; }

        //            //te[0] = DateTime.Now;
        //            //ts[0] = te[0] - tb[0];
        //            //t_tot[0] += ts[0].Seconds *1000 + ts[0].Milliseconds;

        //            //tb[1] = DateTime.Now;
        //            //WriteFlash_PageMB(my_Page_num);
        //            WriteFlashFast_PageMB(my_Page_num);
        //            //te[1] = DateTime.Now;
        //            //ts[1] = te[1] - tb[1];
        //            //t_tot[1] += ts[1].Seconds *1000 + ts[1].Milliseconds;

        //            DateTime stop = DateTime.Now;

        //            //report progress
        //            TimeSpan dif_time = stop - start;
        //            if (my_Page_num > 0)
        //            { myProgress.Prog = "0x" + Convert.ToString(my_Page_num, 16) + "  time left:" + Math.Round((double)(max_page_num - my_Page_num) / (double)my_Page_num * (dif_time.TotalSeconds)) + "s"; }
        //            //start = DateTime.Now;


        //            Application.DoEvents();
        //            //                    tb[2] = DateTime.Now;
        //            ReadFlash_PageMB(my_Page_num);
        //            bool flgVerified = VerifyFlash(V);
        //            num_tries = 0;

        //            while (!flgVerified)
        //            {
        //                if (num_tries < max_num_tries)
        //                {
        //                    num_tries++;
        //                    TB4_Exception.logConsoleOnly("Verify failed on page " + Convert.ToString(my_Page_num, 16));
        //                    //check the buffer bram
        //                    if (CheckBuffer(V))
        //                    {
        //                        //its ok, try again
        //                        for (int k = 0; k < 512; k++)
        //                        { _Page[k] = V[k]; }
        //                    }
        //                    else
        //                    {
        //                        //its not ok, fix it and try again
        //                        for (int k = 0; k < 512; k++)
        //                        { _Page[k] = V[k]; }
        //                    }
        //                    flgVerified = VerifyFlash(V);
        //                    WriteFlashFast_PageMB(my_Page_num);
        //                    ReadFlash_PageMB(my_Page_num);
        //                    flgVerified = VerifyFlash(V);
        //                    if (!flgVerified)
        //                    {
        //                        TB4_Exception.logConsoleOnly("still failing verify");
        //                        if (num_tries > 3)
        //                        {
        //                            TB4_Exception.logConsoleOnly("..and it is serious");
        //                            System.Threading.Thread.Sleep(20);
        //                            WriteFlashFast_PageMB(my_Page_num);
        //                            System.Threading.Thread.Sleep(20);
        //                        }
        //                    }

        //                }
        //                else
        //                {
        //                    if (num_tries >= max_num_tries)
        //                    {
        //                        myProgress.TopMost = false;
        //                        myProgress.Visible = false;
        //                        myProgress.timer1.Enabled = false;
        //                        MessageBox.Show("The write has failed on page 0x" + Convert.ToString(my_Page_num, 16));
        //                        break;
        //                        //exit it all in shame
        //                    }

        //                }
        //            }
        //            //te[2] = DateTime.Now;
        //            //ts[2] = te[2] - tb[2];
        //            //t_tot[2] += ts[2].Seconds * 1000 + ts[2].Milliseconds;
        //            //TB4_Exception.logConsoleOnly("T0: " + t_tot[0] + " T1: " + t_tot[1] + " T2: " + t_tot[2]); 
        //        }
        //    }
        //    // Close StreamReader
        //    sr.Close();

        //    // Close file
        //    file.Close();


        //    myProgress.TopMost = false;
        //    myProgress.Visible = false;
        //    myProgress.timer1.Enabled = false;
        //    myProgress = null;

        //}

        public bool VerifyFlash(byte[] P)
        {
            int i, j, k;
            bool res = true;
            for (j = 0; j < 0x40; j++)
            {
                for (i = 0; i < 8; i++)
                {
                    k = (j * 8) + i;
                    if (P[k] != _Page[k])
                    {
                        res = false;
                        if (debug_flash)
                        {
                            TB4_Exception.logConsoleOnly("comparison failed at " + k.ToString());
                            {

                            }
                        }
                        //break;
                    }
                }
            }
            return res;

        }
        public bool CheckBuffer(byte[] P)
        {
            int[] rdata = new int[2048];
            bool res = true;
            for (int sub_page = 0; sub_page < 2; sub_page++)
            {

                ushort len = 256;
                {
                    UInt32 Addr = TB4.regFLASH_DPRAM_BASE.addr + (UInt32)(0x100 * sub_page);
                    byte A3, A2, A1, A0 = 0;
                    A3 = Convert.ToByte((Addr & 0xff000000) >> 24);
                    A2 = Convert.ToByte((Addr & 0x00ff0000) >> 16);
                    A1 = Convert.ToByte((Addr & 0x0000ff00) >> 8);
                    A0 = Convert.ToByte((Addr & 0x000000ff) >> 0);

                    if (len > 4095) { len = 4095; TB4_Exception.logConsoleOnly("Long Length."); }
                    else if (len < 1) { len = 1; TB4_Exception.logConsoleOnly("Short Length."); }

                    int[] data = new int[1024];
                    TB4.ReadArray(A3, A2, A1, A0, len, data);
                    //TB4_Exception.logConsoleOnly(sub_page+","+data[255]);
                    data.CopyTo(rdata, 0x100 * sub_page);
                }
            }
            for (int k = 0; k < 512; k++)
            {
                if (rdata[k] != P[k])
                { res = false; }
            }
            return (res);
        }

        //public void simuFlash(string fname)
        //{
        //    UInt16 my_Page_num = 0;
        //    byte[] P = new byte[513];
        //    FileStream file;
        //    StreamReader sr;

        //    Progress0 myProgress = new Progress0();
        //    myProgress.Visible = false;
        //    myProgress.timer1.Enabled = true;
        //    myProgress.timer1.Interval = 1000;
        //    myProgress.lblAction.Text = "Burning File to FLASH";
        //    myProgress.progressBar1.Maximum = 0xc00;
        //    myProgress.progressBar1.Visible = false;

        //    try
        //    {
        //        // Specify file, instructions, and privelegdes
        //        file = new FileStream(fname, FileMode.Open, FileAccess.Read);
        //        // Create a new stream to read from a file
        //        sr = new StreamReader(file);
        //    }
        //    catch (Exception ex)
        //    {
        //        TB4_Exception.logError(ex, "Could not open file for read.", true);
        //        return;
        //    }

        //    string[] delimeter = new string[40];
        //    string[] tokens = new string[40];
        //    byte[] line_o_bytes = new byte[40];
        //    byte[,] byte_tokens = new byte[8, 33];
        //    int i = 0;
        //    int j = 0;
        //    delimeter[0] = " ";
        //    delimeter[1] = ":";
        //    delimeter[2] = "\t";

        //    while (sr.EndOfStream == false)
        //    {
        //        string s = sr.ReadLine();
        //        s = s.ToLower();
        //        //:0:0:  ff ff ff ff aa 99 55 66
        //        tokens = s.Split(delimeter, StringSplitOptions.RemoveEmptyEntries);
        //        try
        //        {
        //            my_Page_num = Convert.ToUInt16(tokens[0], 16);
        //            j = Convert.ToUInt16(tokens[1], 16);
        //            for (int ind = 2; ind < 10; ind++)
        //            { line_o_bytes[ind - 2] = Convert.ToByte(tokens[ind], 16); }
        //        }
        //        catch
        //        {
        //            my_Page_num = 0xc00;
        //            myProgress.TopMost = false;
        //            myProgress.Visible = false;
        //            myProgress.timer1.Enabled = false;

        //            return;

        //        }

        //        for (i = 0; i < 8; i++)
        //        {
        //            int k = (j * 8) + i;
        //            _Page[k] = line_o_bytes[i];
        //            P[k] = _Page[k];//local copy
        //        }

        //        if (j == 0x3f)
        //        {
        //            for (int k = 1; k <= TB4.PADE_List.Count; k++)
        //            {
        //                TB4.activatePADE(TB4.PADE_List[k], true, false);
        //                WriteFlash_PageMB(my_Page_num);
        //            }

        //            //report progress
        //            myProgress.Prog = "0x" + Convert.ToString(my_Page_num, 16);

        //            Application.DoEvents();

        //            for (int M = 1; M <= TB4.PADE_List.Count; M++)
        //            {
        //                TB4.activatePADE(TB4.PADE_List[M], true, false);
        //                ReadFlash_PageMB(my_Page_num);
        //                for (j = 0; j < 0x40; j++)
        //                {
        //                    for (i = 0; i < 8; i++)
        //                    {
        //                        int k = (j * 8) + i;
        //                        if (P[k] != _Page[k])
        //                        { MessageBox.Show("flash write not verified... quitting"); return; }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    // Close StreamReader
        //    sr.Close();

        //    // Close file
        //    file.Close();

        //    myProgress.TopMost = false;
        //    myProgress.Visible = false;
        //    myProgress.timer1.Enabled = false;
        //    myProgress = null;


        //}

        private void btnFLASH2SCREEN_Click(object sender, EventArgs e)
        {
            if (!flgShowExpertControls) { return; }
            UInt16 myPageNum = 0xc00;
            try
            {
                myPageNum = Convert.ToUInt16(txtPageNum.Text, 16);
                if (myPageNum > 0xcff) { myPageNum = 0xc00; }
                if (myPageNum < 0) { myPageNum = 0xc00; }
            }
            catch
            {
                myPageNum = 0xc00;
            }
            for (int j = 0; j < 64; j++)
            {
                for (int i = 0; i < 8; i++)
                {
                    int k = (j * 8) + i;
                    byte_box[i, j].Text = "..";
                }
            }
            FlashPage myPage = new FlashPage();
            myPage.page_num = myPageNum;
            myPage.ReadFlash_PageMB();
            for (int j = 0; j < 64; j++)
            {
                for (int i = 0; i < 8; i++)
                {
                    int k = (j * 8) + i;
                    //TB4_Exception.logConsoleOnly("fill: "+k+" : "+_Page[k].ToString());
                    if (radioHEXorDEC.Checked)
                    { byte_box[i, j].Text = Convert.ToString(myPage.page_data[k], 16); }
                    else
                    { byte_box[i, j].Text = myPage.page_data[k].ToString(); }
                }
            }
        }

        private void btnSCREEN2FLASH_Click(object sender, EventArgs e)
        {
            if (!flgShowExpertControls) { return; }

            UInt16 myPageNum = 0xc00;

            for (int j = 0; j < 64; j++)
            {
                for (int i = 0; i < 8; i++)
                {
                    int k = (j * 8) + i;
                    //TB4_Exception.logConsoleOnly("fill: "+k+" : "+_Page[k].ToString());
                    if (radioHEXorDEC.Checked)
                    { _Page[k] = Convert.ToByte(byte_box[i, j].Text, 16); }
                    else
                    { _Page[k] = Convert.ToByte(byte_box[i, j].Text); }
                }
            }

            try
            {
                myPageNum = Convert.ToUInt16(txtPageNum.Text, 16);
                if (myPageNum > 0xcff) { myPageNum = 0xc01; }
                if (myPageNum < 0) { myPageNum = 0xc01; }
            }
            catch
            {
                myPageNum = 0xc00;
            }

            FlashPage myPage = new FlashPage();
            for (int i = 0; i < 512; i++)
            {
                myPage.page_data[i] = _Page[i];
            }
            myPage.page_num = myPageNum;
            //myPage.SlowWrite();
            myPage.FastWrite();
        }

        private void btnExpert_Click(object sender, EventArgs e)
        {
            if (flgShowExpertControls)
            {
                txtByte.Visible = false;
                btnExpert.Text = "SHOW EXPERT CONTROLS";
                ExpertControls(false);
                flgShowExpertControls = false;

            }
            else
            {
                btnExpert.Text = "HIDE EXPERT CONTROLS";
                ExpertControls(true);
                flgShowExpertControls = true;
            }
        }

        private void btnSaveFirmware2File_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txtFirmwareFile.Text = saveFileDialog1.FileName.ToString();
                if (textBox1.Text.ToUpper().Contains("X"))
                {
                    SaveFile(saveFileDialog1.FileName, 0, Convert.ToUInt16(textBox1.Text, 16));
                }
                else
                {
                    SaveFile(saveFileDialog1.FileName, 0, Convert.ToUInt16(textBox1.Text));
                }
            }
        }

        private void radioHEXorDEC_CheckedChanged(object sender, EventArgs e)
        {
            if (radioHEXorDEC.Checked)
            {
                for (int j = 0; j < 32; j++)
                {
                    for (int i = 0; i < 8; i++)
                    {
                        try
                        { byte_box[i, j].Text = Convert.ToString(Convert.ToByte(byte_box[i, j].Text), 16); }
                        catch
                        { }
                    }
                }
            }
            else
            {
                for (int j = 0; j < 32; j++)
                {
                    for (int i = 0; i < 8; i++)
                    {
                        //byte_box[i, j].Text = "0xe";
                        try
                        { byte_box[i, j].Text = Convert.ToString(Convert.ToUInt16(byte_box[i, j].Text, 16)); }
                        catch
                        { }
                    }
                }
            }

        }

        private void btnBURN_FIRMWARE_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.DialogResult browse_result = new DialogResult();

            browse_result = openFileDialog1.ShowDialog();
            if (browse_result == DialogResult.OK)
            {
                txtFirmwareFile.Text = openFileDialog1.FileName;
                FlashFile(openFileDialog1.FileName);
            }
        }

        private void btnBURN_PARAMETERS_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.DialogResult browse_result = new DialogResult();

            browse_result = openFileDialog1.ShowDialog();
            if (browse_result == DialogResult.OK)
            {
                txtParameterFile.Text = openFileDialog1.FileName;
                FlashFile(openFileDialog1.FileName);
            }
        }



        //Experimental..............


        public void FlashFileToAllPADES(string fname, string[] PADEList)
        {
            //Flash to the PADEs using the created array.
            for (int i = 0; i < PADEList.Length; i++)
            { //iterate through all PADE's
                bool exists = false;
                for (int j = 1; j <= TB4.PADE_List.Count; j++)
                {
                    if (TB4.PADE_List[j].PADE_sn == PADEList[i])
                    { exists = true; }
                    else { exists = false; }

                    if (exists)  //the current PADE exists, so lets write the init file
                    {

                        try //make sure we're not currently collecting data...
                        {
                            if (TB4.myRun.flgRunning) { MessageBox.Show("stop the run first!"); }
                        }
                        catch
                        {
                            //MessageBox.Show("Error initializing PADE: there is no run (TB4 hasn't been initialized).");
                            return;
                        }

                        try //the actual initialization happens in this try block.
                        {
                            TB4.activatePADE(TB4.PADE_List[j]);

                            //FlashFile(fname);
                            TB4_Exception.logConsoleOnly("we would now flash " + TB4.PADE_List[j].PADE_sn);
                            FlashFile(fname);
                        }
                        catch
                        {
                            //MessageBox.Show("Error initializing PADE: there is no run (TB4 hasn't been initialized).");
                            return;
                        }
                    }
                }
            }
        }

        void pictureBox2_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            e.Graphics.DrawLine(new Pen(Color.FromArgb(0, 0, 0)), new Point(0, 30), new Point(0, 15));
            e.Graphics.DrawLine(new Pen(Color.FromArgb(0, 0, 0)), new Point(199, 30), new Point(199, 15));
            e.Graphics.DrawLine(new Pen(Color.FromArgb(0, 0, 0)), new Point(0, 15), new Point(80, 15));
            e.Graphics.DrawLine(new Pen(Color.FromArgb(0, 0, 0)), new Point(199, 15), new Point(120, 15));
        }

        void pictureBox1_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            e.Graphics.DrawLine(new Pen(Color.FromArgb(0, 0, 0)), new Point(0, 30), new Point(0, 15));
            e.Graphics.DrawLine(new Pen(Color.FromArgb(0, 0, 0)), new Point(199, 30), new Point(199, 15));
            e.Graphics.DrawLine(new Pen(Color.FromArgb(0, 0, 0)), new Point(0, 15), new Point(70, 15));
            e.Graphics.DrawLine(new Pen(Color.FromArgb(0, 0, 0)), new Point(199, 15), new Point(130, 15));
        }

        private void showChartHeaders(bool show)
        {
            label3.Visible = show;
            label4.Visible = show;
            pictureBox1.Visible = show;
            pictureBox2.Visible = show;

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!flgShowExpertControls) { return; }
            txtPageNum.Text = "0xc01";
            byte[] t = new Byte[512];
            //for (int j = 0; j < 64; j++)
            //{
            //    for (int i = 0; i < 8; i++)
            //    {
            //        int k = (j * 8) + i;
            //        if (i == 0) { t[k] = Convert.ToByte(j); }
            //        else if (i == 7) { t[k] = Convert.ToByte(j); }
            //        else { t[k] = Convert.ToByte(i); }
            //        byte_box[i, j].Text = t[k].ToString();
            //        _Page[k] = t[k];
            //    }
            //}
            ushort my_Page_num = 0xc01;
            FlashPage myFlashPage = new FlashPage();
            myFlashPage.page_num = my_Page_num;
            myFlashPage.ReadFlash_PageMB();

            my_Page_num = 0xc03;
            myFlashPage.page_num = my_Page_num;
            myFlashPage.FastWrite();
            FlashPage newFlashPage = new FlashPage();
            newFlashPage.page_num = my_Page_num;
            newFlashPage.ReadFlash_PageMB();
            for (int i = 0; i < 512; i++)
            {
                if (newFlashPage.page_data[i] != myFlashPage.page_data[i]) { TB4_Exception.logConsoleOnly("not equal on new Page"); }
            }

            //WriteFlashFast_PageMB(my_Page_num);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.DialogResult browse_result = new DialogResult();

            browse_result = openFileDialog1.ShowDialog();
            if (browse_result == DialogResult.OK)
            {
                txtFirmwareFile.Text = openFileDialog1.FileName;
                string[] PADEList;
                if (ConfirmBoardList(out PADEList))
                {
                    if (PADEList != null)
                    {
                        FlashFileToAllPADES(openFileDialog1.FileName, PADEList);
                    }
                }
            }

        }

        private bool ConfirmBoardList(out string[] myPADEList)
        {
            string[] PADEList = new string[TB4.thePADE_Selector.selectedPADEList.Nodes.Count];
            string myList = "";
            for (int i = 0; i < TB4.thePADE_Selector.selectedPADEList.Nodes.Count; i++)
            {
                string[] DH = TB4.thePADE_Selector.selectedPADEList.Nodes[i].Text.Split(new char[] { ' ' });
                PADEList[i] = DH[1];
            }

            for (int i = 0; i < PADEList.Length; i++)
            { //iterate through all PADE's
                bool exists = false;
                for (int j = 1; j <= TB4.PADE_List.Count; j++)
                {
                    if (TB4.PADE_List[j].PADE_sn == PADEList[i])
                    { exists = true; break; }
                    else { exists = false; }
                }
                if (!exists)
                {
                    MessageBox.Show("PADE " + PADEList[i] + " can not be found. We will not proceed.", "PADE " + PADEList[i] + " not found", MessageBoxButtons.OK);
                    myPADEList = null;
                    return false;
                }
                else
                {
                    myList += (i + 1).ToString() + "  PADE sn" + PADEList[i] + "\r\n";
                }
            }
            DialogResult res = MessageBox.Show(myList, "Please confirm fw burn for ", MessageBoxButtons.YesNo);
            if (res == DialogResult.Yes) { myPADEList = PADEList; return true; }
            else { myPADEList = null; return false; }
        }


    }


}