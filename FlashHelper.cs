using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.IO;
using System.Windows.Forms;


namespace PADE
{
    public class FlashPage
    {
        public UInt16 page_num = 0;
        public byte[] page_data = new byte[512];
        private UInt16 Page_min = 0;
        private UInt16 Page_max = 0xcff;
        public bool Verified = false;


        private bool Flash_FSM_Complete()
        {
            //FLASH_CONTROL=1 to make it go
            //TB4.regFLASH_CONTROL.RegWrite(0);
            TB4.regFLASH_CONTROL.RegWrite(1);
            UInt16 stat = 0;
            int k = 0;
            int timeout = 10;
            //while ((stat == 0) && (k < timeout))
            //{
            //    stat = TB4.regFLASH_CONTROL.RegRead();
            //    stat = Convert.ToUInt16(stat & 0x0f);
            //    System.Threading.Thread.Sleep(1);
            //    k++;
            //    if (k == timeout)
            //    {
            //        TB4_Exception.logConsoleOnly("Flash_FSM_Complete K timeout");
            //        TB4_Exception.logError(null, "K timeout", true);
            //        MessageBox.Show("Lost ethernet...");
            //    }
            //}
            ////wait until FLASH_CONTROL=3
            //if (stat == 3)
            //{ return true; }
            //else
            //{ return false; }
            return true; // the COMPLETE flag is now auto ressetting so there is no need to do anything like this!
        }

        public void ReadFlash_PageMB(int pnum = -1)
        {

            UInt16 _Page_num = this.page_num;
            if (pnum >= 0) { _Page_num = Convert.ToUInt16(pnum); }
            int t = 0;
            //int timeout = 10;
            TB4.regFLASH_DPRAM_BASE.addr = TB4.regFLASH_DPRAM_BASE_DB0.addr;
            TB4_Register regFLASH_DPRAM = new TB4_Register("name", "comment", 0, 16, false, false);
            regFLASH_DPRAM.addr = TB4.regFLASH_DPRAM_BASE.addr;
            //TB4.regFLASH_CONTROL.RegWrite(0);
            //setup FLASH_Page_ADDR
            if ((_Page_num >= Page_min) && (_Page_num <= Page_max))
            {
                TB4.regFLASH_PAGE_ADDR.RegWrite(_Page_num);
            }
            else
            {

            }
            //TB4.regFLASH_BYTE_ADDR.RegWrite(0);
            //FLASH_OP_CODE = 0x0B to make it read a _Page
            TB4.regFLASH_OP_CODE.RegWrite(0x0B);
            //FLASH_CONTROL=1 to make it go
            if (Flash_FSM_Complete())
            {//success
                ReadFlash_RAM();
                //TB4.regFLASH_CONTROL_MB.RegWrite(0);
                //TB4_Exception.logConsoleOnly("ff=" + this.page_data[0xff]);
                //TB4_Exception.logConsoleOnly("100=" + this.page_data[0x100]);
            }
            else
            { t++; } //need to add timeout here
        }

        public void ReadFlash_RAM()
        {

            TB4_Exception.logConsoleOnly(TB4.ActivePADE.PADE_fw_ver.ToString());
            ushort len = 256;
            {
                UInt32 Addr = TB4.regFLASH_DPRAM_BASE.addr;
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
                for (uint i = 0; i < 256; i++)
                {
                    this.page_data[2 * i] = Convert.ToByte(data[i] & 0xff);
                    this.page_data[2 * i + 1] = Convert.ToByte((data[i] >> 8) & 0xff);
                }

            }


        }

        public void FastWrite()
        {
            UInt16 Page_min = 0x0;
            UInt16 Page_max = 0xcff;
            int[] rdata = new int[4096];
            UInt16 poll_stat = 0;
            TB4_Register regFLASH_DPRAM = new TB4_Register("name", "comment", 0, 16, false, false);
            regFLASH_DPRAM.addr = TB4.regFLASH_DPRAM_BASE.addr;
            //regFLASH_DPRAM.addr = 0x088000FF;
            //TB4.regFLASH_BYTE_ADDR.RegWrite(0);
            //TB4.regFLASH_CONTROL.RegWrite(0);
            //setup FLASHPage_ADDR
            if ((page_num >= Page_min) && (page_num <= Page_max))
            {
                TB4.regFLASH_PAGE_ADDR.RegWrite((ushort)(page_num));
            }
            //TB4.regFLASH_BYTE_ADDR.RegWrite(0);
            //copy the _Page from the computer to DPRAM
            ushort[] P = new ushort[256];
            for (uint i = 0; i < 256; i++)
            {
                P[i] = Convert.ToUInt16(page_data[2 * i + 1] * 256 + page_data[2 * i]);
            }

            regFLASH_DPRAM.RegWriteArray(P);

            //FLASH_OP_CODE = 0x82 to make it write a _Page
            TB4.regFLASH_OP_CODE.RegWrite(0x82);
            System.Threading.Thread.Sleep(1);

            if (Flash_FSM_Complete() != true)
            { return; }

            System.Threading.Thread.Sleep(15); //blind wait instead of:
            //=================================================================
            //TB4.regFLASH_DPRAM_BASE.RegWrite(0);
            ////poll_stat = TB4.regFLASH_DPRAM_BASE.RegRead();
            //TB4.regFLASH_OP_CODE.RegWrite(0xD7);

            //int poll_try = 0;
            //int max_try = 10;
            //while (poll_try < max_try)
            //{
            //    if (Flash_FSM_Complete() != true) { return; }
            //    {//now we need to poll the FLASH itself
            //        System.Threading.Thread.Sleep(1);
            //        poll_stat = TB4.regFLASH_DPRAM_BASE.RegRead();
            //        if ((poll_stat & 0x80) != 0)
            //        {//success!
            //            break;
            //        }
            //        {//failure
            //            poll_try++;
            //            TB4_Exception.logError(null, "FLASH_WRITE timeout", true);
            //        }
            //    }
            //}
            //TB4_Exception.logConsoleOnly("Poll try= " + poll_try);
            //=================================================================

            ////For troubleshooting
            //ReadFlash_PageMB(_Page_num);
            //for (int k = 0; k < 512; k++)
            //{
            //    lblByte.Text = "verifying: " + (k + 1).ToString() + "/512";
            //    if (P[k] != _Page[k])
            //    {
            //        MessageBox.Show("Flash not verified.");
            //        return;
            //    }
            //}

        }

        public void SlowWrite()
        {

            UInt16 Page_min = 0x0;
            UInt16 Page_max = 0xcff;
            int[] rdata = new int[4096];
            UInt16 poll_stat = 0;
            TB4_Register regFLASH_DPRAM = new TB4_Register("name", "comment", 0, 16, false, false);
            regFLASH_DPRAM.addr = TB4.regFLASH_DPRAM_BASE.addr;
            //regFLASH_DPRAM.addr = 0x088000FF;
            TB4.regFLASH_BYTE_ADDR.RegWrite(0);
            TB4.regFLASH_CONTROL.RegWrite(0);
            //setup FLASHPage_ADDR
            if ((page_num >= Page_min) && (page_num <= Page_max))
            {
                TB4.regFLASH_PAGE_ADDR.RegWrite((ushort)(page_num));
            }
            TB4.regFLASH_BYTE_ADDR.RegWrite(0);
            //copy the _Page from the computer to DPRAM
            ushort[] P = new ushort[256];

            for (uint i = 0; i < 256; i++)
            {
                regFLASH_DPRAM.addr = TB4.regFLASH_DPRAM_BASE.addr + i;
                ushort v = Convert.ToUInt16(page_data[2 * i + 1] * 256 + page_data[2 * i]);
                regFLASH_DPRAM.RegWrite(v);
                P[i] = page_data[i];
                Application.DoEvents();
            }


            //FLASH_OP_CODE = 0x82 to make it write a _Page
            TB4.regFLASH_OP_CODE.RegWrite(0x82);

            if (Flash_FSM_Complete() != true)
            { return; }

            TB4.regFLASH_DPRAM_BASE.RegWrite(0);
            System.Threading.Thread.Sleep(1);
            //poll_stat = TB4.regFLASH_DPRAM_BASE.RegRead();
            TB4.regFLASH_OP_CODE.RegWrite(0xD7);

            int poll_try = 0;
            int max_try = 500;
            while (poll_try < max_try)
            {
                if (Flash_FSM_Complete() != true) { return; }
                {//now we need to poll the FLASH itself
                    poll_stat = TB4.regFLASH_DPRAM_BASE.RegRead();
                    if ((poll_stat & 0x80) != 0)
                    {//success!
                        break;
                    }
                    {//failure
                        poll_try++;
                    }
                }
            }
            TB4_Exception.logConsoleOnly("Poll try= " + poll_try);
        }

    }

    public class FlashData
    {
        public LinkedList<FlashPage> PAGES = new LinkedList<FlashPage>();

        private int min_page;
        private int max_page;
        private string flash_path;
        public Progress0 myProgress;

        public int PageCount
        {
            get
            {
                return PAGES.Count;
            }
        }
        public int MinPageNum
        {
            get { return min_page; }
            set { min_page = value; }
        }
        public int MaxPageNum
        {
            get { return max_page; }
            set { max_page = value; }
        }

        public bool flgReportProgress { get; set; }

        public FlashData()
        { //constructor
            flgReportProgress = true;
            min_page = 0;
            max_page = 0xbff;
            flash_path = "";
            myProgress = new Progress0();
            myProgress.Visible = false;
            myProgress.timer1.Enabled = false;
        }

        public void ClearMem()
        {
            PAGES.Clear();

        }

        public void ReadDataFromFile(string file_name)
        {

            UInt16 my_Page_num = 0;
            UInt16 max_page_num = 0xc00;
            byte[] P = new byte[512];
            byte[] V = new byte[512];
            FileStream file;
            StreamReader sr;

            try
            {
                // Specify file, instructions, and priveleges
                file = new FileStream(file_name, FileMode.Open, FileAccess.Read);
                // Create a new stream to read from a file
                sr = new StreamReader(file);
            }
            catch (Exception ex)
            {
                TB4_Exception.logError(ex, "Could not open file for read.", true);
                return;
            }

            string[] delimeter = new string[40];
            string[] tokens = new string[40];
            byte[] line_o_bytes = new byte[40];
            byte[,] byte_tokens = new byte[8, 33];
            int i = 0;
            int j = 0;
            delimeter[0] = " ";
            delimeter[1] = ":";
            delimeter[2] = "\t";

            this.ClearMem();
            while (sr.EndOfStream == false)
            {
                string s = sr.ReadLine();
                s = s.ToLower();
                //:0:0:  ff ff ff ff aa 99 55 66
                tokens = s.Split(delimeter, StringSplitOptions.RemoveEmptyEntries);
                try
                {
                    ShowProgress("Reading from file", 0x0bff, my_Page_num);
                    my_Page_num = Convert.ToUInt16(tokens[0], 16);
                    j = Convert.ToUInt16(tokens[1], 16);
                    for (int ind = 2; ind < 10; ind++)
                    { line_o_bytes[ind - 2] = Convert.ToByte(tokens[ind], 16); }
                }
                catch
                {
                    my_Page_num = 0xc01;
                    return;
                }

                for (i = 0; i < 8; i++)
                {
                    int k = (j * 8) + i;
                    TB4.theFlash._Page[k] = line_o_bytes[i];  //page stored here
                    P[k] = TB4.theFlash._Page[k];//local copy
                }

                if (j == 0x3f)
                {
                    FlashPage new_page = new FlashPage();
                    for (int ii = 0; ii < 512; ii++)
                    {
                        new_page.page_data[ii] = P[ii];
                    }
                    new_page.page_num = my_Page_num;
                    this.PAGES.AddLast(new_page);
                    if (this.PAGES.Count == 1) { this.min_page = my_Page_num; this.max_page = my_Page_num; }
                    else { this.max_page = my_Page_num; }

                    new_page = null;
                    Application.DoEvents();
                }
            }
            // Close StreamReader
            sr.Close();
            ShowProgress("close", 100, 100);

            // Close file
            file.Close();

        }

        public void WriteDataToFile(string file_name)
        {
            FileStream file;
            StreamWriter sw;
            try
            {
                // Specify file, instructions, and privelegdes
                file = new FileStream(file_name, FileMode.Create, FileAccess.Write);
                // Create a new stream to read from a file
                sw = new StreamWriter(file);
            }
            catch (Exception ex)
            {
                TB4_Exception.logError(ex, "Could not open file to write.", true);
                return;
            }


            foreach (FlashPage fp in PAGES)
            {
                string s = "";
                int k = 0;
                //write it to file
                for (int j = 0; j < 64; j++)
                {
                    s = ":" + Convert.ToString(fp.page_num, 16) + ":" + Convert.ToString(j, 16) + ": ";
                    string st = "";
                    for (int i = 0; i < 8; i++)
                    {
                        k = (j * 8) + i;
                        st = Convert.ToString(fp.page_data[k], 16);
                        if (st.Length == 1) { st = "0" + st; }
                        s += " " + st;
                    }
                    sw.WriteLine(s);
                }
            }
            sw.Close();
            file.Close();
        }

        public void ReadDatafromPADE(int max_page = 0x0bff)
        {
            this.ClearMem();

            for (ushort i = 0; i < max_page; i++)
            {
                FlashPage p = new FlashPage();
                p.page_num = i;
                p.ReadFlash_PageMB();
                this.PAGES.AddLast(p);
                ShowProgress("Reading from PADE " + TB4.ActivePADE.PADE_sn.ToString(), max_page, i);
                p = null;
            }
            ShowProgress("close", 100, 100);
        }

        public void WriteDatatoPADE(int min_page = 0, int max_page = 0x0bff)
        {
            if (PageCount >= max_page - min_page)
            {
                LinkedListNode<FlashPage> thisNode;

                thisNode = PAGES.First;
                for (int i = min_page; i <= max_page; i++)
                {
                    while (thisNode.Value.page_num < min_page)
                    {
                        thisNode = thisNode.Next;
                    }
                    if ((thisNode.Value.page_num <= max_page) && (thisNode.Value.page_num >= min_page))
                    {
                        ShowProgress("Writing data to PADE " + TB4.ActivePADE.PADE_sn.ToString(), max_page - min_page, i - min_page);
                        if (thisNode.Value.page_num == (ushort)i)
                        {
                            thisNode.Value.FastWrite();
                            TB4_Exception.logConsoleOnly("wrote page " + thisNode.Value.page_num);
                            if (thisNode != PAGES.Last)
                            { thisNode = thisNode.Next; }
                            else
                            { break; }

                        }
                        else
                        { TB4_Exception.logError(null, "writing the wrong page", false); }
                    }
                }
            }
            else
            {
                TB4_Exception.logError(null, "WriteDatatoPADE can not write from " + min_page + " to " + max_page + " with " + PageCount + " pages.", true);
            }
            ShowProgress("close", 100, 100);
        }

        public void ComparePadeToFile(string file_name)
        {
            ReadDataFromFile(file_name);
            foreach (FlashPage fp in PAGES)
            {
                FlashPage pp = new FlashPage();
                pp.ReadFlash_PageMB(fp.page_num);
                for (int i = 0; i < 512; i++)
                {
                    if (pp.page_data[i] != fp.page_data[i])
                    {
                        TB4_Exception.logError(null, "File does not match PADE at page " + pp.page_num + " and byte " + i + " because " + pp.page_data[i] + " is not equal to " + fp.page_data[i], false);
                    }
                }
            }

        }

        public void ShowProgress(string action, int max, int current)
        {

            DateTime current_time = DateTime.Now;
            double percent;
            if (myProgress == null)
            {
                myProgress = new Progress0();
                myProgress.timer1.Enabled = true;
                myProgress.Action = action;
                myProgress.Prog = "N/A";
                myProgress.progressBar1.Maximum = 100;
                myProgress.progressBar1.Value = 0;
            }
            else
            {
                if (action.ToLower().Contains("close"))
                {
                    myProgress.timer1.Enabled = false;
                    myProgress.Visible = false;

                    //myProgress = null;
                }
                else
                {
                    if (max > 0)
                    { percent = 100 * (double)current / (double)max; }
                    else
                    { percent = -1; }

                    if (current == 0)
                    {
                        myProgress.timer1.Enabled = true;
                        myProgress.Action = action;
                        myProgress.Prog = "N/A";
                        myProgress.progressBar1.Maximum = 100;
                        myProgress.progressBar1.Value = 0;
                        myProgress.Visible = true;
                        myProgress.BringToFront();
                        myProgress.StartTime = DateTime.Now;
                    }
                    TimeSpan dif_time = current_time - myProgress.StartTime;
                    if (percent > 1 && percent < 100)
                    {
                        if (myProgress.Action == "") { myProgress.Action = action; }
                        myProgress.Prog = Convert.ToString(current, 16) + " of " + Convert.ToString(max, 16) + "  remains:" + Math.Round((100 - percent) / percent * (dif_time.TotalSeconds)) + "s";
                        myProgress.progressBar1.Value = (int)Math.Round(percent);
                    }
                }
            }
        }


    }

    public static class FlashDataHelper
    {


        public static bool CompareFlashData(this FlashData fd1, FlashData fd2, int max_page = -1)
        {
            bool res = false;
            LinkedListNode<FlashPage> fdp1;
            LinkedListNode<FlashPage> fdp2;
            byte[] fdpd1 = new byte[512];
            byte[] fdpd2 = new byte[512];


            if (max_page < 0) { max_page = fd1.PageCount; }
            if (fd2.PageCount < max_page) { max_page = fd2.PageCount; }
            fdp1 = fd1.PAGES.First;
            fdp2 = fd2.PAGES.First;
            res = true;
            for (int i = 1; i < max_page; i++)
            {
                fdpd1 = fdp1.Value.page_data;
                fdpd2 = fdp2.Value.page_data;
                fdp1.Value.Verified = true;
                for (int j = 0; j < 512; j++)
                {
                    if (fdpd1[j] != fdpd2[j])
                    {
                        fdp1.Value.Verified = false;
                        res = false;
                        TB4_Exception.logConsoleOnly("Page val at " + fdp1.Value.page_num + "," + j + fdpd1[j] + "!=" + fdp2.Value.page_num + "," + fdpd2[j]);

                        break;
                    }

                }
                fdp1 = fdp1.Next;
                fdp2 = fdp2.Next;
            }
            return res;
        }

    }
}

