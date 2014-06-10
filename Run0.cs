using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using PcapDotNet.Core;
using WeifenLuo.WinFormsUI.Docking;

using System.Diagnostics;



namespace PADE
{
    public partial class Run0 : DockContent
    {
        interface_UM245 UM245 = new interface_UM245();
        public long last_ev_num = 0;
        TB4_PerfMon runMon = new TB4_PerfMon("Run Monitor", "Analyzer of the background worker event taker in the Run form.");

        public Run0()
        {
            InitializeComponent();
        }

        private void btnInitFileBrowse_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "TB4 Init files|*.tb4";
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtInitFile.Text = openFileDialog1.FileName;
                richTextBox1.LoadFile(openFileDialog1.FileName, RichTextBoxStreamType.PlainText);
            }
        }

        private void btnRunFileBrowse_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "TB4 data file|*.dat";
            saveFileDialog1.Title = "Save Run data";
            saveFileDialog1.ShowDialog();

            //' If the file name is not an empty string open it for saving.
            if (saveFileDialog1.FileName != "")
            {
                TB4.myRun.Run_filename = saveFileDialog1.FileName;
                this.txtRun.Text = saveFileDialog1.FileName;
            }
        }

        #region Run

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            
            while (TB4.myRun.flgRunning)
            {
                //Thread.Sleep(100);
                if (Worker.CancellationPending)
                {
                    TB4.myRun.flgRunning = false;
                }

                if (TB4.myRun.flg_UDP)
                {
                    while (TB4.myRun.flgRun_pause) { Thread.Sleep(10); }
                    long old_evt = TB4.myRun.Run_event;
                    int timeout = 0;
                    DateTime start_time = DateTime.Now;
                    while ((old_evt == TB4.myRun.Run_event) && (timeout < 200))
                    {
                        TB4.myRun.Take_UDP_Event();
                        DateTime current_time = DateTime.Now;
                        int result = DateTime.Compare(start_time.AddMilliseconds(1000), current_time);
                        if (result == -1)
                        { timeout = 1000; }
                    }
                    Console.WriteLine("ARM PADE");
                    TB4.myRun.ARM_PADE();
                }

                else
                {
                    Thread.Sleep(1);
                    while (TB4.myRun.flgRun_pause) { Thread.Sleep(25); }
                    TB4.myRun.TakeEvent();
                }

                if (TB4.myRun.Run_time.CompareTo(TB4.myRun.RunStopTime) >= 0)
                {
                    TB4.myRun.flgRunning = false;
                }

                if (TB4.myRun.Run_event >= TB4.myRun.Run_maxevents)
                {
                    TB4.myRun.flgRunning = false;
                }

                Int16 ev_per = Convert.ToInt16(100 * TB4.myRun.Run_event / TB4.myRun.Run_maxevents);
                long d1 = 0;
                long d2 = 0;
                Int16 time_per = 0;
                d1 = TB4.myRun.Run_time.Ticks - TB4.myRun.RunStartTime.Ticks;
                d2 = TB4.myRun.RunStopTime.Ticks - TB4.myRun.RunStartTime.Ticks;
                try
                {
                    time_per = Convert.ToInt16(100 * (d1 / d2));
                }
                catch
                {
                    time_per = 0;
                }

                if (ev_per > time_per)
                {
                    if (ev_per > 100) { ev_per = 100; }
                    Worker.ReportProgress(ev_per);
                }
                else
                {
                    Worker.ReportProgress(time_per);
                }
            }

        }
        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (this.last_ev_num != TB4.myRun.Run_event)
            {
                this.progressBar1.Value = e.ProgressPercentage;
                string t = System.DateTime.Now.ToLongTimeString();
                t += " " + System.DateTime.Now.ToShortDateString();
                this.lblEventNumAndTime.Text = TB4.myRun.Run_event.ToString() + " taken " + t;
                if (TB4.myRun.flg_UDP)
                {
                    if (TB4.thePlot.Visible) { TB4.thePlot.Plot_RS_display(); }

                }
                else
                {
                    if (TB4.thePlot.Visible && !TB4.myRun.flg_mb_usb)
                    {
                        TB4.thePlot.Plot8_display();
                        Thread.Sleep(10);
                        //TB4.thePlot.Plot0_display(TB4.myRun.plot_data0, TB4.myRun.plot_data1, TB4.myRun.plot_data2, TB4.myRun.plot_data3);
                        TB4.thePlot.lblTRIG_CSR.Text = Convert.ToString(TB4.myRun.glbTrig_Bits, 16);
                    }
                    if (TB4.myRun.flg_mb_usb && TB4.thePlot.Visible)
                    {
                        TB4.thePlot.btn_UDP_Click("report progress", null);
                    }
                }
                this.last_ev_num = TB4.myRun.Run_event;
            }

        }

        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            TB4.myRun.RunStop();
            this.btnRUN.Enabled = true;
            this.btnRUN.Visible = true;
            this.btnStop.Enabled = false;
            this.btnStop.Visible = false;

            this.richTextBox1.AppendText("\n");
            //this.txtRunNum.Text = Convert.ToString(Convert.ToInt16(this.txtRunNum.Text) + 1);
            this.txtRunNum.Text = Convert.ToString(Convert.ToInt16(this.txtRunNum.Text));
            this.richTextBox1.AppendText(DateTime.Now.ToString());
            this.richTextBox1.AppendText(" Done");
        }

        private void btnRUN_Click(object sender, EventArgs e)
        {
            {
                //I am using a static myRun for now.... maybe thats not the best, but...
                try
                { TB4.myRun.Run_maxevents = Convert.ToInt32(txtMaxEvnts.Text); }
                catch
                {
                    TB4.myRun.Run_maxevents = 100;
                    txtMaxEvnts.Text = "100";
                }

                try
                { TB4.myRun.Run_maxseconds = Convert.ToInt32(txtMaxTime.Text); }
                catch
                {
                    TB4.myRun.Run_maxseconds = 3600;
                    txtMaxTime.Text = "3600";
                }

                if (TB4.myRun.flgCosmicTrig)
                {
                    bool[] trigAND = new bool[4];

                    if (TB4.myRun.flg_AND_ch0) { trigAND[0] = true; } else { trigAND[0] = false; }
                    if (TB4.myRun.flg_AND_ch1) { trigAND[1] = true; } else { trigAND[1] = false; }
                    if (TB4.myRun.flg_AND_ch2) { trigAND[2] = true; } else { trigAND[2] = false; }
                    if (TB4.myRun.flg_AND_ch3) { trigAND[3] = true; } else { trigAND[3] = false; }

                    for (int j = 0; j < 4; j++)
                    {
                        if (trigAND[j])
                        {
                            string trig_regname = "TRIG_THRESHOLD_CH" + j.ToString();
                            string noise_regname = "NOISE_THRESHOLD_CH" + j.ToString();
                            for (int i = 1; i < TB4.TB4_Registers.Length; i++)
                            {
                                if (TB4.TB4_Registers[i] != null)
                                {
                                    if ((trig_regname == TB4.TB4_Registers[i].name.ToUpper()) && (TB4.TB4_Registers[i].display_tab == "DB0"))
                                    {
                                        ushort temp = TB4.TB4_Registers[i].RegRead();
                                        TB4.myRun.glbTrig_Level = temp;
                                    }
                                    if ((noise_regname == TB4.TB4_Registers[i].name.ToUpper()) && TB4.TB4_Registers[i].display_tab == "DB0")
                                    {
                                        ushort temp = TB4.TB4_Registers[i].RegRead();
                                        TB4.myRun.glbNoise_Level = temp;
                                    }
                                }
                            }
                        }
                    }

                }
                TB4.myRun.RunStart();
                this.Worker.WorkerReportsProgress = true;
                this.Worker.RunWorkerAsync();
                this.btnRUN.Enabled = false;
                this.btnRUN.Visible = false;

                this.btnStop.Enabled = true;
                this.btnStop.Visible = true;
                this.richTextBox1.AppendText("\n");
                this.txtRunNum.Text = Convert.ToString(Convert.ToInt16(this.txtRunNum.Text) + 1);
                this.richTextBox1.AppendText(DateTime.Now.ToString());
                this.richTextBox1.AppendText(" Started");
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            this.Worker.CancelAsync();
        }
        #endregion Run

        private void btnInit_Click(object sender, EventArgs e)
        {
            

            
            try
            {
                if (TB4.myRun.flgRunning) { MessageBox.Show("stop the run first!"); }
            }
            catch { MessageBox.Show("Odd- there is no run"); }

            richTextBox1.Clear();
            try
            {
                if (txtInitFile.Text != "")
                {
                    TB4.FileInit(txtInitFile.Text);
                }
                else
                {
                    //TB4.myMessage = new TB4_Message("no file name given");
                }
            }
            catch
            {
                //TB4.myMessage.mess="error in InitFile";
            }
            finally
            {
                //richTextBox1.AppendText(TB4.myMessage.mess);
            }
        }

        #region AID
        private void btn_ListDevices_Click(object sender, EventArgs e)
        {
            tbx_No_Devices.Text = Convert.ToString(USB_AID.FT_ListDevices());
        }

        private void btn_Open_Click(object sender, EventArgs e)
        {
            UInt32 tstatus;
            uint n = USB_AID.FT_ListDevices();
            if (n == 0)
            {
                MessageBox.Show("No UM245 devices found!");
                return;
            }
            //if (n == 1)
            //{  
            //    UM245.Open();
            //    tstatus = UM245.status;
            //    tbx_Status.Text = Convert.ToString(tstatus);
            //    btn_Close.Enabled = true;
            //    this.btnRUN.Enabled = true;
            //    this.btnInit.Enabled = true;
            //    TB4.thePlot.btn_PLOT.Enabled = true;
            //    TB4.thePlot.btn_PLOT.Text = "PLOT";
            //}
            if (n >= 1)
            {
                SelectUSB mySelectUSB = new SelectUSB();
                mySelectUSB.label1.Text = n.ToString() + " devices found. Choose one.";
                FTD2XX_NET.FTDI.FT_DEVICE_INFO_NODE[] ftdiDeviceList = new FTD2XX_NET.FTDI.FT_DEVICE_INFO_NODE[n];

                // Populate our device list
                TB4.myFTDI.GetDeviceList(ftdiDeviceList);

                for (UInt32 i = 0; i < n; i++)
                {
                    string t = "";
                    //t =  i.ToString();
                    //t+=": Serial Number = " + ftdiDeviceList[i].SerialNumber.ToString();
                    if (ftdiDeviceList[i].SerialNumber.Length > 0)
                    {
                        t = ftdiDeviceList[i].SerialNumber.ToString() + " of type " + ftdiDeviceList[i].Description;
                    }
                    else
                    { t = "unavailable"; }
                    mySelectUSB.listBox1.Items.Add(t);
                    mySelectUSB.listBox1.SelectedIndex = 0;
                }
                mySelectUSB.ShowDialog();
                if (mySelectUSB.DialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    this.tbx_Status.Text = "OK";
                    ulong rxsize = 0; ulong txsize = 0;
                    tstatus = USB_AID.FT_GetStatus(ref rxsize, ref txsize);
                    btn_Close.Enabled = true;
                    this.btnRUN.Enabled = true;
                    this.btnInit.Enabled = true;
                    TB4.thePlot.btn_PLOT.Enabled = true;
                    TB4.thePlot.btn_PLOT.Text = "PAUSE";

                    TB4.thePADE_explorer.usbMode = true;
                    
                    TB4.thePADE_explorer.updateStatusText(true, "USB Mode enabled.  To Disable, click this text.");
                }
                else
                {
                    this.tbx_Status.Text = "NOT OK";
                }
            }
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            foreach (KeyValuePair<int, PADE> kvp in TB4.PADE_List)
            {
                PADE thisPADE = kvp.Value;
               // thisPADE.PADE_FTDI.Close();
            }
            TB4.PADE_List.Clear();
            TB4.thePAD_selector.Controls.Clear();
            tbx_Status.Text = "";
           
            this.btnRUN.Enabled = false;
            this.btnInit.Enabled = false;
        }
        #endregion AID

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            TB4.myAboutBox.ShowDialog();
        }

        private void chk_ParamExtTrig_CheckedChanged(object sender, EventArgs e)
        {
            if (this.chk_ParamExtTrig.Checked)
            { TB4.myRun.flgExtTrig = true; }
            else { TB4.myRun.flgExtTrig = false; }
        }

        private void chk_ParamZS_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_ParamZS.Checked)
            { TB4.myRun.flg_ZS = true; }
            else
            {
                TB4.myRun.flg_ZS = false;
                if (chk_ParamSumOnly.Checked)
                {
                    TB4.myRun.flgSumOnly = false;
                    chk_ParamSumOnly.Checked = false;
                }
            }
        }

        private void chk_ParamSumOnly_CheckedChanged(object sender, EventArgs e)
        {
            if (this.chk_ParamSumOnly.Checked)
            { TB4.myRun.flg_UDP = true; }
            else { TB4.myRun.flg_UDP = false; }
            //TB4.myRun.flgSumOnly = true;
            //if (!TB4.myRun.flg_ZS)
            //{
            //    TB4.myRun.flg_ZS = true;
            //    chk_ParamZS.Checked = true;
            //}

            //else { TB4.myRun.flgSumOnly = false; }
        }

        private void chk_SoftTrig_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_SoftTrig.Checked) { TB4.myRun.flgSoftwareTrig = true; }
            else { TB4.myRun.flgSoftwareTrig = false; }
        }

        private void btnListEth_Click(object sender, EventArgs e)
        {
            EthSelect theEthSelect = new EthSelect();
            theEthSelect.ShowDialog();
            //theEthSelect.EthSelect_Load(null, null);
            if (TB4.ETH_OK)
            {
                txtEthStatus.Text = "ok";
                this.btnRUN.Enabled = true;
                this.btnInit.Enabled = true;
            }
        }

        private void btnEthClose_Click(object sender, EventArgs e)
        {
            foreach (KeyValuePair<int, PADE> kvp in TB4.PADE_List)
            {
                kvp.Value.flgEther_comms = false;

            }
            tbx_Status.Text = "";

            this.btnRUN.Enabled = false;
            this.btnInit.Enabled = false;
        }

        private void Run0_Load(object sender, EventArgs e)
        {
            Size returnSize = this.Size;    
            this.DockStateChanged += (object a, System.EventArgs b) => { TB4.thePADE_explorer.childChangedDockstate(this, returnSize); };
        }
        void Run0_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            TB4.thePADE_explorer.childClosed(this);
            e.Cancel = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (System.IO.File.Exists(TB4.systemFileName))
            {

                try
                {
                    Process.Start("notepad.exe", this.txtRun.Text);

                }
                catch (Exception ex)
                {
                    MessageBox.Show("File open failed: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show(TB4.systemFileName + " does not exist.");
            }
        }


    }
}