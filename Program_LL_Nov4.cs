using System;
using System.Timers;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Threading;
//using System.Net;
//using System.Net.Sockets;
//using System.Text.RegularExpressions;
using System.IO;
//using ZedGraph;
using PcapDotNet.Core;
using PcapDotNet.Packets;
using PcapDotNet.Packets.IpV4;
using PcapDotNet.Packets.Transport;
using FTD2XX_NET;

namespace PADE
{
    public class TB4_Run
    {
        public bool flgRun_pause;
        public int Run_num;
        public string Run_filename;
        public long Run_maxevents;
        public long Run_maxseconds;
        public long Run_event;
        public DateTime Run_time;
        public DateTime RunStartTime;
        public DateTime RunStopTime;
        public DateTime InitStartTime;

        public bool flgRunning;
        public long runCurrentEvents;
        public int min_bin;
        public int max_bin;
        public bool[] ch_enabled = new bool[32];

        public int trig_timeout = 50;

        public bool flgCosmicTrig = false;
        public bool flg_slope_pos = false;
        public bool flg_AND_ch0 = false;
        public bool flg_AND_ch1 = false;
        public bool flg_AND_ch2 = false;
        public bool flg_AND_ch3 = false;

        public bool flg_UDP = false;
        public bool flg_delay = false;
        public int delay_time = 50;

        public bool flgExtTrig = false;
        public bool flgSumOnly = false;
        public bool flg_ZS = false;
        public bool flgSoftwareTrig = false;
        public UInt16[] num_zs_ev = new UInt16[32];

        //public int[] event_header = new int[4];

        public int[] event_data;
        public int[,] event_data_by_ch;
        public IpV4Datagram[] event_UDP_data;

        //public int[] plot_data0 = new int[4096];
        //public int[] plot_data1 = new int[4096];
        //public int[] plot_data2 = new int[4096];
        //public int[] plot_data3 = new int[4096];

        public int event_data_length = 128;
        public int event_num_ch = 8;

        public bool flg_noise_filter = false;
        public bool flg_add_rate_register = false;

        public bool flg_mb_usb = false;

        public System.Timers.Timer DelayTimer = new System.Timers.Timer();

        public UInt16 glbTrig_Bits = 0;
        public UInt16 glbTrig_Level = 0;
        public UInt16 glbNoise_Level = 0;

        //these are some global shortcuts which are populated during run_start
        public TB4_Register runStatusReg;
        public TB4_Register runControlReg;
        public TB4_Register runHDMI_StatusReg;
        public TB4_Register runFrameLengthReg;
        public TB4_Register runTotalFramesReg;
        public TB4_Register runSoftTrig;
        public PADE MasterPADE;

        private BinaryWriter BinStream;
        public BinaryWriter binstreamRun
        {
            get { return BinStream; }
            set { BinStream = value; }
        }
        private StreamWriter privateStream = new StreamWriter("c:\\default.dat");
        public StreamWriter streamRun
        {
            get { return privateStream; }
            set { privateStream = value; }
        }

        //constructor
        public TB4_Run()
        {
            this.Run_num = 0;
            this.flgRunning = false;
            this.streamRun = null;
            this.DelayTimer.Enabled = false;
            this.DelayTimer.Interval = 1000;
            this.DelayTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            this.DelayTimer.AutoReset = true;
            this.flgRun_pause = false;
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {

        }

        //public void TB4_RunStart()
        //{
        //    this.ch_enabled[0] = false;
        //    this.ch_enabled[1] = false;
        //    this.ch_enabled[2] = false;
        //    this.ch_enabled[3] = false;
        //    this.flgRunning = false;
        //    //this.Run_filename = filename;
        //    //this.Run_maxevents = newMaxEvnt;
        //    if (this.Run_maxseconds <= 0) { this.Run_maxseconds = long.MaxValue; }
        //    //this.Run_num = //TB4.ReadRunNumberLocal(); -- need to implement local storage
        //    this.Run_num++;
        //    //TB4.SaveRunNumberLocal(RunNumber);-- need to implement local storage
        //    try
        //    {
        //        if (streamRun != null) { streamRun.Close(); }
        //    }
        //    catch
        //    {
        //        //TB4.myMessage.mess="problem with streamRun";
        //    }
        //    string myFileName = this.Run_filename + "_" + this.Run_num.ToString() + ".dat";
        //    this.streamRun = new StreamWriter(myFileName, false);
        //    this.flgRunning = true;
        //}

        public void RunStart()
        {
            this.flgRunning = true;
            this.runCurrentEvents = 0;
            this.RunStartTime = DateTime.Now;
            //get run number and current time
            try
            {
                if (this.streamRun != null) { this.streamRun.Close(); }
            }
            catch
            {
                //TB4.myMessage = new TB4_Message("could not open data stream");
                return;
            }

            if (this.Run_filename == null)
            {
                if (TB4.theRunForm.txtRun.Text == null)
                {
                    this.Run_filename = "c:\\no_name_run";
                }
                else
                {
                    this.Run_filename = TB4.theRunForm.txtRun.Text;
                }
            }
            if (this.Run_filename.Contains(".dat"))
            {
                this.Run_filename = this.Run_filename.Substring(0, this.Run_filename.IndexOf(".dat"));
            }
            if (TB4.theRunForm.txtRunNum.Text != null)
            {
                if (TB4.theRunForm.txtRunNum.Text == "") { TB4.theRunForm.txtRunNum.Text = "1"; }
                this.Run_num = Convert.ToInt16(TB4.theRunForm.txtRunNum.Text);
            }

            string myFileName = this.Run_filename;
            myFileName += "_";
            myFileName += this.Run_num.ToString();
            myFileName += ".dat";
            this.Run_filename = myFileName;
            StreamWriter newRun = new StreamWriter(Run_filename, false);
            ushort res = TB4.regSTATUS.RegRead();
            //if (res == 0) { MessageBox.Show("trying to start run but can not read CSR"); }
            this.streamRun = newRun;
            this.Run_event = 0;
            this.RunStartTime = DateTime.Now;
            this.RunStopTime = DateTime.Now.AddSeconds(this.Run_maxseconds);
            this.event_data = new int[512 * 32];


            try
            {

                //Im going to setup some short cuts here so I don't have to keep looking up the same values every single event
                int ind = 0;
                if (!TB4.TB4_Registers_Dict.TryGetValue("STATUS_REG", out ind)) { MessageBox.Show("big fail, call Paul"); }
                else { runStatusReg = TB4.TB4_Registers[ind]; }

                if (!TB4.TB4_Registers_Dict.TryGetValue("CONTROL_REG", out ind)) { MessageBox.Show("big fail, call Paul"); }
                else { runControlReg = TB4.TB4_Registers[ind]; }

                if (!TB4.TB4_Registers_Dict.TryGetValue("HDMI_STATUS", out ind)) { MessageBox.Show("big fail, call Paul"); }
                else { runHDMI_StatusReg = TB4.TB4_Registers[ind]; }

                if (!TB4.TB4_Registers_Dict.TryGetValue("FRAME_LENGTH", out ind)) { MessageBox.Show("big fail, call Paul"); }
                else { runFrameLengthReg = TB4.TB4_Registers[ind]; }

                if (!TB4.TB4_Registers_Dict.TryGetValue("ZS_TOTAL_FRAMES", out ind)) { MessageBox.Show("big fail, call Paul"); }
                else { runTotalFramesReg = TB4.TB4_Registers[ind]; }

                if (!TB4.TB4_Registers_Dict.TryGetValue("SOFTWARE_TRIGGER", out ind)) { MessageBox.Show("big fail, call Paul"); }
                else { runSoftTrig = TB4.TB4_Registers[ind]; }

                for (int i = 0; i < TB4.PADE_List.Count; i++)
                {
                    PADE thisPADE;
                    if (TB4.PADE_List.TryGetValue(i, out thisPADE))
                    {
                        if (thisPADE.PADE_is_MASTER) { MasterPADE = thisPADE; }
                    }
                }
            }

            catch { }
            if (TB4.myRun.flg_UDP)
            {
                PacketCommunicator communicator =
                  TB4.ETH_INTERFACE.Open(65536, PacketDeviceOpenAttributes.Promiscuous, trig_timeout);
                communicator.SetKernelBufferSize(16777216); //16MB buffer. Default is 1MB
                using (BerkeleyPacketFilter filter = communicator.CreateFilter("src port 20817"))
                {
                    // Set the filter to IPv4 and UDP
                    communicator.SetFilter(filter);
                }
                TB4.myPacketComm = communicator;
            }
        }

        #region UDP
        public void Take_UDP_Event()
        {
            PcapDotNet.Packets.Packet packet;
            byte[] receive_byte_array;
            byte[] more_byte_array;
            int data_len = event_data_length * event_num_ch;
            byte[] data_array = new byte[0];
            UInt16 j = 0;
            string s;
            int bytes_recieved = 0;

            //have I had a trigger? 
            TB4.ActivePADE = MasterPADE;
            ushort stat = runStatusReg.RegRead();
            stat = runStatusReg.RegRead();

            if (((stat & 2) == 2) && ((stat & 0x10)== 0))
            {//yep, trigger latched and ZS done
                //loop over all PADEs, ask to spit data
                s = "";
                for (int i = 0; i < TB4.PADE_List.Count; i++)
                {
                    PADE thisPADE;
                    if (TB4.PADE_List.TryGetValue(i + 1, out thisPADE))
                    {
                        TB4.ActivePADE = thisPADE;
                        runControlReg.RegWrite(0x02c0);
                    }

                    bool all_done = false;
                    bool took_event = false;
                    while (!all_done)
                    {
                        PacketCommunicatorReceiveResult result = TB4.myPacketComm.ReceivePacket(out packet);
                        switch (result)
                        {
                            case PacketCommunicatorReceiveResult.Timeout:
                                // Timeout elapsed
                                all_done = true;
                                if (took_event)
                                {
                                    this.Run_event++;

                                    this.streamRun.WriteLine(s); s = "";
                                    for (int k = 1; k < this.event_UDP_data.Length; k++)
                                    {
                                        s = this.Run_event.ToString() + " ";
                                        s += thisPADE.PADE_index.ToString() + " ";
                                        //s += this.event_UDP_data[k].SourcePort.ToString() + " ";
                                        s += "  " + this.event_UDP_data[k].Payload.ToHexadecimalString();
                                        this.streamRun.WriteLine(s); s = "";
                                    }
                                //    this.streamRun.WriteLine("======");

                                //    string t = this.event_UDP_data[1].Payload.ToHexadecimalString();

                                //    t = t.Substring(20);
                                //    s = t.Substring(0, 8);
                                //    s += " ";
                                //    s += t.Substring(8, 8);
                                //    s += " ";

                                //    int c=Convert.ToInt32(t.Length/8);
                                //    string[] lw = new string[c];
                                //    for (int k=0;k<c-2;k++)
                                //    {
                                //        lw[k] = t.Substring(16 + 8 * k, 8);
                                //        //now swap them around
                                //        string t1 = lw[k].Substring(0, 2);
                                //        string t2 = lw[k].Substring(2, 2);
                                //        string t3 = lw[k].Substring(4, 2);
                                //        string t4 = lw[k].Substring(6, 2);
                                //        lw[k] = t4 + t3 + t2 + t1;
                                //    //    s += lw[k] + " ";
                                //    }
                                //    t = "";
                                //    for (int k = 0; k < c; k++)
                                //    { t = lw[k]+t; }
                                //    c = Convert.ToInt32(t.Length / 3);
                                //    this.event_data_by_ch = new int[32, 16];
                                //    int kk=0;
                                    
                                //    while(kk < c)
                                //    {
                                //        s += t.Substring(t.Length-3*(1+ kk), 3) + " ";
                                //        kk += 3;
                                //    }                                    
                                //    this.streamRun.WriteLine(s);
                                //    this.streamRun.WriteLine("======");
                                
                                }
                                s = "";
                                break;

                            //TB4.active_PAD_index
                            case PacketCommunicatorReceiveResult.Ok:
                                 IpV4Datagram ip = packet.Ethernet.IpV4;
                                 
                                 
                                 if (this.event_UDP_data == null)
                                 { this.event_UDP_data = new IpV4Datagram[1]; }
                                int l = this.event_UDP_data.Length;
                                Array.Resize<IpV4Datagram>(ref this.event_UDP_data, l + 1);
                                this.event_UDP_data[l] = ip;
                                //receive_byte_array = packet.Buffer;
                                // string PackPayload = udp.Payload.ToHexadecimalString();
                                 //receive_byte_array = udp.
                                // int tail = data_array.Length;
                                //Array.Resize<byte>(ref data_array, (data_array.Length + receive_byte_array.Length));
                                //receive_byte_array.CopyTo(data_array, tail);

                                if (this.streamRun != null)
                                {
                                    took_event = true;
                                }
                                break;
                            default:
                                throw new InvalidOperationException("The result " + result + " shoudl never be reached here");
                        }
                    }
                    TB4.myRun.event_UDP_data = new IpV4Datagram[1];
                }

            }
            else
            {//no, no trigger found. Just return

            }
        }

        public void ARM_PADE()
        {

            TB4.ActivePADE = this.MasterPADE;
            runControlReg.RegWrite(0);
            // loop over all slave PADs and set Control Reg to 0
            for (int i = 1; i < TB4.PADE_List.Count; i++)
            {
                PADE thisPADE;
                if (TB4.PADE_List.TryGetValue(i, out thisPADE))
                {
                    if (!thisPADE.PADE_is_MASTER)
                    {
                        TB4.ActivePADE = thisPADE;
                        runControlReg.RegWrite(0);
                    }
                }
            }


            // loop over all Slave PADEs and arm ZS and ext trig (always) with 1C0
            for (int i = 0; i < TB4.PADE_List.Count; i++)
            {
                PADE thisPADE;
                if (TB4.PADE_List.TryGetValue(i + 1, out thisPADE))
                {
                    if (thisPADE.PADE_is_MASTER) { }//master is already active
                    else
                    {
                        TB4.ActivePADE = thisPADE; runControlReg.RegWrite(0x01c0);
                    }
                }

            }
            // now enable Master
            TB4.ActivePADE = MasterPADE;
            runControlReg.RegWrite(0x01c0);
            //thats it. We are ARMED
            //if this is a software trig, do it now
            if (flgSoftwareTrig)
            {
                runSoftTrig.RegWrite(1);
            }

        }

        #endregion UDP

        //public void SpeedTest2()
        //{
        //    int ThisSleep = 0;
        //    int MaxSleep = 200;
        //    byte[] data = new Byte[64];
        //    ulong rxsize = 0, txsize = 0;
        //    ulong[] speed = new ulong[500];
        //    uint length = 0;
        //    uint i = 0;
        //    byte[] wdata = new Byte[2];
        //    wdata[0] = 0x21;
        //    wdata[1] = 0x00;
        //    i = 1;
        //    USB_AID.FT_Write(wdata, i);
        //    while ((ThisSleep < MaxSleep) && (rxsize < 32))
        //    {
        //        USB_AID.FT_GetStatus(ref rxsize, ref txsize);
        //        //if (ThisSleep>0)
        //        { Thread.Sleep(1); }
        //        ThisSleep++;
        //    }
        //    length = USB_AID.FT_Read(data, rxsize);
        //    for (i = 0; i < 32; i++)
        //    {
        //        this.num_zs_ev[i] = (UInt16)(256 * data[i * 2] + data[i * 2 + 1]);
        //    }
        //    USB_AID.FT_GetStatus(ref rxsize, ref txsize);
        //    if (rxsize > 0) { USB_AID.FT_Read(data, rxsize); }
        //}

        public void TakeEvent() //and store it
        {
            UInt16 ui_data = 0;
            long[] ms = new long[6];
            //cosmic trigger used to be here...

            UInt16 ArmControl = Convert.ToUInt16(TB4.regCONTROL.RegRead() & 0x7f);
            UInt16 DisarmControl = Convert.ToUInt16(TB4.regCONTROL.RegRead() & 0x7f);
            UInt16 CSR = 0;

            if (this.flgExtTrig) { ArmControl += 0x80; }
            //if (this.flg_ZS) { ArmCSR += 0x40; }
            //if (this.flgSumOnly) { ArmCSR += 0x20; }

            foreach (KeyValuePair<int, PADE> kvp in TB4.PADE_List)
            {
                TB4.myFTDI = kvp.Value.PADE_FTDI;
                TB4.regCONTROL.RegWrite(ArmControl);
            }

            foreach (KeyValuePair<int, PADE> kvp in TB4.PADE_List)
            {
                if (this.flgSoftwareTrig)
                {
                    //MessageBox.Show("soft trig not yet implemented");
                    if (kvp.Value.PADE_is_MASTER)
                    {//issue software trig
                        int REG_soft_trig_ind = 0;
                        TB4.TB4_Registers_Dict.TryGetValue("SOFT_TRIG", out REG_soft_trig_ind);
                        TB4.TB4_Registers[REG_soft_trig_ind].RegWrite(1);
                    }
                }
                for (int i = 0; i < 32; i++)
                { this.ch_enabled[i] = kvp.Value.PADE_ch_enable[i]; }
                Thread.Sleep(1);
                if (flgExtTrig)
                {//this is an external trig, look for the one
                    int k = 0; CSR = 0;
                    int ext_trig_latched = 2;
                    while (((CSR & ext_trig_latched) != ext_trig_latched) && (k < this.trig_timeout))
                    {
                        if (k > 0) { Thread.Sleep(1); }
                        CSR = TB4.regSTATUS.RegRead();
                        k++;
                    }
                    if ((k + 1) > this.trig_timeout) { return; } //return from this routine without writing an event to disk
                }
                //else
                //{//we are done
                //    Thread.Sleep(1);
                //    TB4.regCONTROL.RegWrite(DisarmControl);
                //}

                //CSR = TB4.regSTATUS.RegRead();
                //if (flg_ZS)
                //{
                //    for (int chn = 0; chn < 32; chn++)
                //    {
                //        if (this.ch_enabled[chn])
                //        {
                //            TB4_Register temp_reg = new TB4_Register("temp", "DB0", Convert.ToUInt32(0x0A000000 + chn), Convert.ToByte(16), true, false);
                //            this.num_zs_ev[chn] = temp_reg.RegRead();
                //        }
                //    }
                //}

                //SpeedTest2();
                byte A3 = 0x40;
                byte A2 = 0;
                byte A1 = 0;
                byte A0 = 0;
                UInt16 data_len = 1024;

                if (this.min_bin < 0) { this.min_bin = 0; }
                if (this.max_bin > 4095) { this.max_bin = 4095; }
                if (this.max_bin <= this.min_bin) { this.max_bin = 4095; this.min_bin = 0; }
                if ((this.max_bin - this.min_bin) < 4095) { data_len = (ushort)(this.max_bin - this.min_bin); }
                int[] data = new int[512];
                int[] data0 = new int[512];
                int[] data1 = new int[512];
                int[] data2 = new int[512];
                int[] data3 = new int[512];
                string s = "";
                //                if ((this.min_bin >= 0) && (this.min_bin < 4095))
                {
                    A0 = (byte)(this.min_bin & 0xff);
                    A1 = (byte)(((this.min_bin & 0x1f00) >> 8) & 0xff);
                    //A2 = (byte)(((this.min_bin & 0xff0000) >> 16) & 0xff);
                }

                UInt32 StartA = 0;
                #region db_direct
                if (!flg_mb_usb)
                {
                    for (int i = 0; i < 32; i++)
                    {

                        if (this.ch_enabled[i])
                        {

                            if (i < 32)
                            {
                                StartA = Convert.ToUInt32(0x04000000 + 0x00010000 * (i & 0x7)) + 0x00300000;
                                A3 = Convert.ToByte((StartA & 0xff000000) >> 24);
                                A2 = Convert.ToByte((StartA & 0x00ff0000) >> 16);
                                A1 = Convert.ToByte((StartA & 0x0000ff00) >> 8);
                                A0 = Convert.ToByte((StartA & 0x000000ff));
                            }

                            if (i < 24)
                            {
                                StartA = Convert.ToUInt32(0x04000000 + 0x00010000 * (i & 0x7)) + 0x00200000;
                                A3 = Convert.ToByte((StartA & 0xff000000) >> 24);
                                A2 = Convert.ToByte((StartA & 0x00ff0000) >> 16);
                                A1 = Convert.ToByte((StartA & 0x0000ff00) >> 8);
                                A0 = Convert.ToByte((StartA & 0x000000ff));
                            }

                            if (i < 16)
                            {
                                StartA = Convert.ToUInt32(0x04000000 + 0x00010000 * (i & 0x7)) + 0x00100000;
                                A3 = Convert.ToByte((StartA & 0xff000000) >> 24);
                                A2 = Convert.ToByte((StartA & 0x00ff0000) >> 16);
                                A1 = Convert.ToByte((StartA & 0x0000ff00) >> 8);
                                A0 = Convert.ToByte((StartA & 0x000000ff));
                            }

                            if (i < 8)
                            {
                                StartA = Convert.ToUInt32(0x04000000 + 0x00010000 * (i & 0x7));
                                A3 = Convert.ToByte((StartA & 0xff000000) >> 24);
                                A2 = Convert.ToByte((StartA & 0x00ff0000) >> 16);
                                A1 = Convert.ToByte((StartA & 0x0000ff00) >> 8);
                                A0 = Convert.ToByte((StartA & 0x000000ff));
                            }

                            data_len = 512;
                            TB4.ReadArray(A3, A2, A1, A0, data_len, data);
                            data.CopyTo(TB4.myRun.event_data, i * 512);
                        }
                    }

                    TB4.regCONTROL.RegWrite(DisarmControl);
                    Thread.Sleep(0);
                    if (this.streamRun != null)
                    {
                        for (int j = 0; j < 32; j++)
                        {
                            if (this.ch_enabled[j])
                            {
                                s = DateTime.Now.ToString();
                                int chnum = j + 32 * kvp.Key - 32;
                                if (flgSumOnly) { s = s + " Sums"; }
                                s = s + " ";
                                if (this.flg_ZS)
                                { s += chnum.ToString() + " zs " + this.num_zs_ev[j].ToString() + " "; }
                                else
                                { s = s + chnum.ToString() + " "; }

                                if (flgSumOnly | flg_ZS)
                                {
                                    //for (int i = 0; i < this.num_zs_ev[j]; i++)
                                    //{
                                    //    s = s + " " + TB4.myRun.event_data[i + 512 * j].ToString();
                                    //}
                                    for (int i = 0; i < 512; i++)
                                    {
                                        s = s + " " + TB4.myRun.event_data[i + 512 * j].ToString();
                                    }
                                }
                                else
                                {
                                    for (int i = 0; i < 512; i++)
                                    {
                                        s = s + " " + TB4.myRun.event_data[i + 512 * j].ToString();
                                    }
                                }
                                TB4.myRun.streamRun.WriteLine(s);
                            }
                        }
                    }
                }
                #endregion db_direct
            }
            TB4.myRun.Run_event++;
            TB4.myRun.Run_time = DateTime.Now;
        }

        public void RunStop()
        {
            this.flgRunning = false;
            if (this.streamRun != null)
            {
                this.streamRun.Close();
                this.streamRun.Dispose();
                this.streamRun = null;
            }
            this.Run_num = 0;
            this.Run_filename = null;
        }

    }

    public class TB4_Message
    {
        public string mess;
        public string comment;
        public string source;
        public int severity; //0=low, 99=error
        public bool isLoged;
        //constructor
        public TB4_Message(string new_mess)
        {
            this.mess = new_mess;
            this.comment = "";
            this.source = "";
            this.severity = 0;
            this.isLoged = false;
        }
        public TB4_Message(string new_mess, string new_source, string new_comment)
        {
            this.mess = new_mess;
            this.comment = new_comment;
            this.source = new_mess;
            this.severity = 0;
            this.isLoged = false;
        }
        public TB4_Message(string new_mess, string new_source, string new_comment, int severity_level, bool log)
        {
            this.mess = new_mess;
            this.comment = new_comment;
            this.source = new_mess;
            this.severity = severity_level;
            this.isLoged = log;
        }
    }

    public class TB4_Register
    {
        public string name;
        public string comment;
        public string display_tab;
        public string private_comment;
        public UInt32 addr;
        public byte width; //this is supposed to tell you how "wide" this register is
        public bool readOnly;
        public bool writeOnly;
        public UInt32 myStatus;
        public bool hasSubfields;
        //    LinkedList<TB4_Subfield> subfieldList = new LinkedList<TB4_Subfield>();

        //constructor
        public TB4_Register(string name, string comment, UInt32 addr, byte width, bool readOnly, bool writeOnly)
        {
            this.name = name;
            this.comment = comment;
            this.addr = addr;
            this.width = width;
            this.readOnly = readOnly;
            this.writeOnly = writeOnly;
            this.display_tab = "MAIN";
            this.private_comment = "";
        }
        public TB4_Register(string name, string comment, UInt32 addr, byte width, bool readOnly, bool writeOnly, string displaytab)
        {
            this.name = name;
            this.comment = comment;
            this.addr = addr;
            this.width = width;
            this.readOnly = readOnly;
            this.writeOnly = writeOnly;
            this.display_tab = displaytab;
            this.private_comment = "";
        }

        public UInt16 RegRead()
        {
            TB4.myRun.flgRun_pause = true;
            if (this.writeOnly) { return 0; }
            uint temp_addr = this.addr;
            uint old_addr = temp_addr;
            if (this.width == 32)
            {
                uint func_code = (temp_addr & 0xffff0000);
                temp_addr = (temp_addr & 0x0000ffff) >> 1;
                temp_addr += func_code;
            }
            //            if (this.width == 16)
            {
                UInt16 ui_data = 0;
                //int OneSleep = 1;
                int ThisSleep = 0;
                int MaxSleep = 20;
                byte[] data = new Byte[64];
                byte[] discard_data = new Byte[64];
                ulong rxsize = 0, txsize = 0;
                UInt32 i;
                UInt32 length = 2;
                data[0] = 0xA5;
                data[1] = 7;
                data[2] = 0x1; //this is the read command
                data[3] = (byte)((temp_addr & 0xff000000) >> 24); //MSB
                data[4] = (byte)((temp_addr & 0x00ff0000) >> 16);
                data[5] = (byte)((temp_addr & 0x0000ff00) >> 8);
                data[6] = (byte)((temp_addr & 0x000000ff));
                data[7] = 0;
                data[8] = 0;
                data[9] = 0;
                data[10] = 0;
                i = 8;
                USB_AID.FT_GetStatus(ref rxsize, ref txsize);
                while (rxsize > 64)
                {
                    length = USB_AID.FT_Read(discard_data, 64);
                    USB_AID.FT_GetStatus(ref rxsize, ref txsize);
                }
                if (rxsize > 0) { USB_AID.FT_Read(discard_data, rxsize); }//if (rxsize > 2) { USB_AID.FT_Read(data, rxsize); }
                //Thread.Sleep(1);
                USB_AID.FT_Write(data, i);
                Thread.Sleep(0);
                //now check to make sure we have 2 bytes returned
                while ((ThisSleep < MaxSleep) && (rxsize != 2))
                {
                    Thread.Sleep(1);
                    USB_AID.FT_GetStatus(ref rxsize, ref txsize);
                    ThisSleep++;
                }
                if (ThisSleep >= MaxSleep) { return 0; }//we failed, basically
                switch (rxsize)
                {
                    case 0:
                        this.myStatus = 100;
                        break;
                    case 1:
                        this.myStatus = 101;
                        break;
                    case 2:
                        length = USB_AID.FT_Read(data, rxsize);
                        if (length == 2)
                        {
                            ui_data = (UInt16)(data[0] * 256 + data[1]);
                        }
                        break;
                    default:
                        this.myStatus = 999;
                        break;
                }
                USB_AID.FT_GetStatus(ref rxsize, ref txsize);
                //Thread.Sleep(1);
                if ((rxsize < 65) && (rxsize > 0)) { length = USB_AID.FT_Read(data, rxsize); }//clean up
                //USB_AID.FT_GetStatus(ref rxsize, ref txsize);
                //Thread.Sleep(5);
                TB4.myRun.flgRun_pause = false;
                //if (ui_data == 401) { SpeedTest(); }
                if (this.width == 32)
                {
                    temp_addr = old_addr;
                }
                return ui_data;
            }
        }

        //public void SpeedTest()
        //{
        //    int ThisSleep = 0;
        //    int MaxSleep = 200;
        //    byte[] discard_data = new Byte[64];
        //    ulong rxsize = 0, txsize = 0;
        //    ulong[] speed = new ulong[500];
        //    uint length = 0;
        //    uint i = 0;
        //    byte[] data = new Byte[3];
        //    data[0] = 0x12;
        //    data[1] = 0x34;
        //    data[2] = 0x00; //this is the write command
        //    i = 1;
        //    USB_AID.FT_Write(data, i);

        //    while (ThisSleep < MaxSleep)
        //    {
        //        USB_AID.FT_GetStatus(ref rxsize, ref txsize);
        //        speed[ThisSleep] = rxsize;
        //        Thread.Sleep(1);
        //        ThisSleep++;
        //    }
        //    while (rxsize > 64)
        //    {
        //        length = USB_AID.FT_Read(discard_data, 64);
        //        USB_AID.FT_GetStatus(ref rxsize, ref txsize);
        //    }
        //    if (rxsize > 0) { USB_AID.FT_Read(discard_data, rxsize); }

        //}

        public void RegWrite(UInt16 ui_data)
        {
            if (this.readOnly) { return; }
            TB4.myRun.flgRun_pause = true;
            byte[] data = new Byte[64];
            byte[] d_data = new Byte[64];
            UInt32 i;
            uint temp_addr = this.addr;

            uint old_addr = temp_addr;
            if (this.width == 32)
            {
                uint func_code = (temp_addr & 0xffff0000);
                temp_addr = (temp_addr & 0x0000ffff) >> 1;
                temp_addr += func_code;
            }
            data[0] = 0xA5;
            data[1] = 7;
            data[2] = 0x11; //this is the write command
            data[3] = (byte)((temp_addr & 0xff000000) >> 24); //MSB
            data[4] = (byte)((temp_addr & 0x00ff0000) >> 16);
            data[5] = (byte)((temp_addr & 0x0000ff00) >> 8);
            data[6] = (byte)((temp_addr & 0x000000ff));
            data[7] = (byte)((ui_data & 0xff00) >> 8);
            data[8] = (byte)((ui_data & 0x00ff));
            data[9] = 0;
            data[10] = 0;
            i = 10;
            USB_AID.FT_Write(data, i);
            TB4.myRun.flgRun_pause = false;
            if (this.width == 32)
            {
                temp_addr = old_addr;
            }
        }

        public void RMW(string RegisterName) { }
    }

    //class TB4_Subfield
    //{
    //  public string name;
    //  public string comment;
    //  public byte width;
    //  public byte position;
    //  public bool readOnly;
    //  public bool writeOnly;
    //}

    #region UM245
    class interface_UM245
    {
        /// <summary>
        /// status codes:
        /// 0= all ok
        /// 1= no USB connection
        /// 100= no bytes available to read
        /// 101= too few bytes in register read
        /// 102= too many bytes in register read
        /// 999= unknown error
        /// </summary>
        //backing store
        private UInt32 privNumDev = 0;
        private UInt32 privStatus = 0;
        public UInt32 numDev { get { return privNumDev; } }
        public UInt32 status { get { return privStatus; } }

        //constructor
        public interface_UM245()
        {
            privNumDev = 0;
            privStatus = 999;
        }

        /// <summary>
        /// The following are utility routines and are AID specific. 
        /// The idea here is that this will be sued as the implementation of the generic 
        /// read, write and RMW routines defined in the register class
        /// </summary>

        //public void Read(ref byte[] data, ulong i) 
        //{
        //}

        //public void Write(ref byte[] data, ulong i)
        //{
        //}

        public void Open()
        {
            privStatus = USB_AID.FT_Open();

            if (privStatus != 0) { privStatus = 1; }
        }
        public void Close()
        {
            USB_AID.FT_Close();
        }
        public void Flush_rx()
        {
            ulong rxsize = 0, txsize = 0;
            UInt32 length = 0;
            byte[] data = new Byte[UInt16.MaxValue];
            length = USB_AID.FT_Read(data, rxsize);
            USB_AID.FT_GetStatus(ref rxsize, ref txsize);
            if (rxsize != 0)
            {
                Close();
                Open();
            }
        }

    }
    #endregion UM245

    #region USB_AID
    /// <summary>
    /// USB_AID interface class: this one uses the AID.dll
    /// </summary>
    static class USB_AID
    {
        //----------------------------------------------------------------------------
        [STAThread]
        //----------------------------------------------------------------------------

        static public UInt32 FT_ListDevices()
        {
            uint devcount = 0;
            TB4.myFTDI.GetNumberOfDevices(ref devcount);
            return devcount;
        }

        static public UInt32 FT_ChangeLatency(byte latency)
        {
            FTDI.FT_STATUS ftStatus = FTDI.FT_STATUS.FT_DEVICE_NOT_OPENED;
            TB4.myFTDI.SetLatency(latency);
            if (ftStatus == FTDI.FT_STATUS.FT_OK)
            { return 0; }
            else { return 1; }
        }

        static public UInt32 FT_ChangeTimeout(uint new_timeout)
        {
            FTDI.FT_STATUS ftStatus = FTDI.FT_STATUS.FT_DEVICE_NOT_OPENED;
            TB4.myFTDI.SetTimeouts(new_timeout, new_timeout);
            if (ftStatus == FTDI.FT_STATUS.FT_OK)
            { return 0; }
            else { return 1; }
        }

        static public UInt32 FT_ChangeBuffer(uint in_buffer, uint out_buffer)
        {
            FTDI.FT_STATUS ftStatus = FTDI.FT_STATUS.FT_DEVICE_NOT_OPENED;
            TB4.myFTDI.InTransferSize(in_buffer);

            if (ftStatus == FTDI.FT_STATUS.FT_OK)
            { return 0; }
            else { return 1; }
        }

        static public UInt32 FT_Open()
        {
            FTDI.FT_STATUS ftStatus = FTDI.FT_STATUS.FT_DEVICE_NOT_OPENED;
            // Allocate storage for device info list
            uint devcount = 0;
            TB4.myFTDI.GetNumberOfDevices(ref devcount);
            FTDI.FT_DEVICE_INFO_NODE[] ftdiDeviceList = new FTDI.FT_DEVICE_INFO_NODE[devcount];

            // Populate our device list
            ftStatus = TB4.myFTDI.GetDeviceList(ftdiDeviceList);
            //now open the device
            ftStatus = TB4.myFTDI.OpenBySerialNumber(ftdiDeviceList[0].SerialNumber);
            if (ftStatus == FTDI.FT_STATUS.FT_OK)
            { return 0; }
            else { return 1; }
        }


        static public UInt32 FT_Open(string SN)
        {
            FTDI.FT_STATUS ftStatus = FTDI.FT_STATUS.FT_DEVICE_NOT_OPENED;
            // Allocate storage for device info list
            uint devcount = 0;
            TB4.myFTDI.GetNumberOfDevices(ref devcount);
            FTDI.FT_DEVICE_INFO_NODE[] ftdiDeviceList = new FTDI.FT_DEVICE_INFO_NODE[devcount];

            // Populate our device list
            ftStatus = TB4.myFTDI.GetDeviceList(ftdiDeviceList);
            //now open the device
            for (uint i = 0; i < devcount; i++)
            {
                if (SN == ftdiDeviceList[i].SerialNumber)
                {


                    int index = TB4.PADE_List.Count + 1;
                    PADE thisPADE = new PADE();
                    if (index == 1)
                    {
                        thisPADE.PADE_is_MASTER = true; thisPADE.PADE_is_SLAVE = false;
                        TB4.thePAD_selector.Visible = true;
                    }
                    else { thisPADE.PADE_is_MASTER = false; thisPADE.PADE_is_SLAVE = true; }

                    ftStatus = thisPADE.PADE_FTDI.OpenBySerialNumber(ftdiDeviceList[i].SerialNumber);
                    if (ftStatus == FTDI.FT_STATUS.FT_OK)
                    {
                        //TB4.theDisplayControl.Text = listBox1.SelectedItem.ToString();
                        string t = "";
                        thisPADE.PADE_index = index;
                        thisPADE.PADE_FTDI.GetDescription(out t);
                        thisPADE.PADE_descr = t;
                        thisPADE.PADE_FTDI.GetSerialNumber(out t);
                        thisPADE.PADE_sn = t;
                        TB4.theRegistersForm.Text = t;
                        TB4.theRunForm.Text = t;
                        TB4.PADE_List.Add(index, thisPADE);
                        TB4.thePAD_selector.AddPAD(index);
                        return 0;
                    }
                }
            }

            return 1;
        }

        static public UInt32 FT_Close()
        {
            TB4.myFTDI.Close();
            return 0;
        }

        static public UInt32 FT_Write(byte[] p_data, uint size)
        {
            //            UInt32 status = 999;
            uint ret_size = 0;
            int isize = Convert.ToInt32(size);
            FTD2XX_NET.FTDI.FT_STATUS stat = FTDI.FT_STATUS.FT_DEVICE_NOT_FOUND;
            try
            {
                stat = TB4.myFTDI.Write(p_data, isize, ref ret_size);
            }
            catch { }
            string s = stat.ToString();
            if (s.Contains("error")) { MessageBox.Show("IO error encountered"); }
            return ret_size;
        }

        //[DllImport("AID.dll")]
        //static public extern UInt32 FT_GetStatus(ref ulong rxsize, ref ulong txsize);
        static public UInt32 FT_GetStatus(ref ulong rxsize, ref ulong txsize)
        {
            uint irxsize = 0;
            uint itxsize = 0;

            TB4.myFTDI.GetRxBytesAvailable(ref irxsize);
            TB4.myFTDI.GetTxBytesWaiting(ref itxsize);
            rxsize = Convert.ToUInt64(irxsize);
            txsize = Convert.ToUInt64(itxsize);

            return 0;
        }

        //[DllImport("AID.dll")]
        //static public extern UInt32 FT_Read([MarshalAs(UnmanagedType.LPArray)] byte[] p_data, ulong size);
        static public UInt32 FT_Read(byte[] p_data, ulong size)
        {
            //UInt32 status = 999;
            uint ret_size = 0;
            uint isize = Convert.ToUInt32(size);
            uint irxsize = 0;
            uint itxsize = 0;

            TB4.myFTDI.GetRxBytesAvailable(ref irxsize);
            if (irxsize < isize) { return 0; }
            TB4.myFTDI.Read(p_data, isize, ref ret_size);
            return ret_size;
        }
        //[DllImport("AID.dll")]
        //static public extern UInt32 FT_SetBitMode(byte mask, byte enable);
        //[DllImport("AID.dll")]
        //static public extern UInt32 FT_EE_Read(ref UInt16 vid, ref UInt16 pid, ref UInt16 power);
        //[DllImport("AID.dll")]
        //static public extern UInt32 FT_EE_Program(UInt16 power);
        //[DllImport("AID.dll")]
        //static public extern UInt32 FT_EE_ProgramToDefault();
        //[DllImport("AID.dll")]
        //static public extern UInt32 KCAN_Send(UInt32 channel, UInt32 id, UInt32 dlc, [MarshalAs(UnmanagedType.LPArray)] byte[] p_data);
        //[DllImport("AID.dll")]
        //static public extern UInt32 KCAN_Receive(ref UInt32 channel, ref UInt32 id, ref UInt32 dlc, [MarshalAs(UnmanagedType.LPArray)] byte[] p_data);
        //[DllImport("AID.dll")]
        //static public extern UInt32 KCAN_Init(UInt32 baudraute);
        //-----------------------------------------------------------------------------
    }
    #endregion USB_AID


    public class PADE
    {
        public FTD2XX_NET.FTDI PADE_FTDI = new FTD2XX_NET.FTDI();
        public string PADE_sn;
        public string PADE_descr;
        public int PADE_index;
        public bool PADE_is_SLAVE;
        public bool PADE_is_MASTER;
        public bool PADE_initialized;
        public bool PADE_fw_ver;
        public bool[] PADE_ch_enable = new bool[32];

        public PADE()
        {
            PADE_initialized = false;
            PADE_index = -1;
            PADE_is_SLAVE = true;
            PADE_is_MASTER = false;
            PADE_FTDI = new FTDI();
            for (int i = 0; i < 32; i++)
            { PADE_ch_enable[i] = false; }
        }
    }

    public class TB4
    {
        public static Flash0 theFlash;
        public static Plot0 thePlot; //= new Plot0();
        public static Hist0 theHist;
        public static Hist1 theHist1;
        public static GBE theGBE;
        public static DRAM theDRAM;
        public static PAD_select thePAD_selector;
        public static BiasOffset theBiasOffset;
        //        public static EthSelect theEthSelect;
        //public static frmMain theMainForm = new frmMain();
        public static Run0 theRunForm = new Run0();
        public static TB4_Run myRun = new TB4_Run();
        public static int num_PADs = 0;
        public static int active_PAD_index = 0;
        public static bool ETH_OK;
        public static LivePacketDevice ETH_INTERFACE = null;
        //this really belongs in the PADE
        //=================================
        public static PacketCommunicator myPacketComm;
        //=================================
        public static SortedList<int, PADE> PADE_List = new SortedList<int, PADE>(4);
        public static Registers theRegistersForm = new Registers();
        public static Arrays theArraysForm = new Arrays();
        public static FTD2XX_NET.FTDI myFTDI = new FTD2XX_NET.FTDI();
        public static AboutBox1 myAboutBox = new AboutBox1();

        public static byte[] ary_FlashPage = new byte[265];

        public static TB4_Register[] TB4_Registers = new TB4_Register[300];
        public static SortedDictionary<String, Int32> TB4_Registers_Dict = new SortedDictionary<string, int>();

        //I need these two registers defined by name because they are used in the PLOT0 form 
        public static TB4_Register regSTATUS = new TB4_Register("STATUS", "CSR", 0x01000000, 16, false, false);
        public static TB4_Register regCONTROL = new TB4_Register("STATUS", "CSR", 0x00100000, 16, false, false);
        public static TB4_Register regTRIG_STATUS = new TB4_Register("TRIG", "TRIG", 0x04D00000, 16, false, false);


        public static TB4_Register regFLASH_CONTROL = new TB4_Register("CSR", "CSR", 0x00100000, 16, false, false);// temp place holder
        public static TB4_Register regFLASH_OP_CODE = new TB4_Register("CSR", "CSR", 0x00100000, 16, false, false);// temp place holder
        public static TB4_Register regFLASH_PAGE_ADDR = new TB4_Register("CSR", "CSR", 0x00100000, 16, false, false);// temp place holder
        public static TB4_Register regFLASH_BYTE_ADDR = new TB4_Register("CSR", "CSR", 0x00100000, 16, false, false);// temp place holder
        public static TB4_Register regFLASH_DPRAM_BASE = new TB4_Register("FLASH_DPRAM_BASE_MB", "DPRAM_BASE_MB", 0x00088000, 16, false, false);
        public static TB4_Register regFLASH_DPRAM_BASE_MB = new TB4_Register("FLASH_DPRAM_BASE_MB", "DPRAM_BASE_MB", 0x00088000, 16, false, false);
        public static TB4_Register regFLASH_DPRAM_BASE_DB0 = new TB4_Register("FLASH_DPRAM_BASE_DB0", "DPRAM_BASE_DB0", 0x08800000, 16, false, false);
        public static TB4_Register regFLASH_DPRAM_BASE_DB1 = new TB4_Register("FLASH_DPRAM_BASE_DB1", "DPRAM_BASE_DB1", 0x18800000, 16, false, false);
        public static TB4_Register regFLASH_DPRAM_BASE_DB2 = new TB4_Register("FLASH_DPRAM_BASE_DB2", "DPRAM_BASE_DB2", 0x28800000, 16, false, false);
        public static TB4_Register regFLASH_DPRAM_BASE_DB3 = new TB4_Register("FLASH_DPRAM_BASE_DB3", "DPRAM_BASE_DB3", 0x38800000, 16, false, false);

        public static PADE ActivePADE;

        public static void FileInit(string fname)
        {
            //fill the TB4_Registers array 

            FileStream file;
            StreamReader sr;

            try
            {
                // Specify file, instructions, and privelegdes
                file = new FileStream(fname, FileMode.OpenOrCreate, FileAccess.Read);
                // Create a new stream to read from a file
                sr = new StreamReader(file);
            }
            catch { return; };

            string[] delimeter = new string[64];
            string[] tokens = new string[64];

            delimeter[0] = "<=";
            delimeter[1] = "//";
            delimeter[2] = ";";
            delimeter[3] = "=";
            delimeter[4] = "set ";
            delimeter[5] = "dec";
            delimeter[6] = "0x";

            while (sr.EndOfStream == false)
            {
                string s = sr.ReadLine();
                s = s.ToLower();
                if (s.Contains("//"))
                {
                    s = s.Substring(0, s.IndexOf("//"));
                }
                if (s.Contains("<=")) //register asignment
                {
                    tokens = s.Split(delimeter, StringSplitOptions.None);
                    string regname = tokens[0].Trim();

                    string regvalue = tokens[1].Trim();
                    if (s.Contains("dec"))
                    {
                        try
                        {
                            UInt16 temp = Convert.ToUInt16(tokens[2].Trim());
                            regvalue = Convert.ToString(temp, 16);
                        }
                        catch
                        {
                            regvalue = "0";
                        }
                    }
                    if (s.Contains("0x")) { regvalue = tokens[2].Trim(); }
                    if (regname == "sleep") { Thread.Sleep(100); }
                    TB4.RegisterAssignment(regname, regvalue);
                }
                else if (s.Contains("set "))
                {
                    tokens = s.Split(delimeter, StringSplitOptions.None);
                    string paramname = tokens[1].Trim();
                    string paramvalue = tokens[2].Trim();
                    TB4.ParamAssignment(paramname, paramvalue);
                }
                else
                { continue; }
            }
            // Close StreamReader
            sr.Close();

            // Close file
            file.Close();

        }

        private static void ParamAssignment(string paramname, string paramvalue)
        {
            if (paramname == "run_num")
            {
                theRunForm.txtRunNum.Text = paramvalue;
            }
            else if (paramname == "run_filename")
            {
                //myRun.Run_filename = paramvalue;
                theRunForm.txtRun.Text = paramvalue;
            }
            else if (paramname == "run_maxevent")
            {
                //myRun.Run_maxevents  =(long) Convert.ToInt32(paramvalue);
                theRunForm.txtMaxEvnts.Text = paramvalue;
            }
            else if (paramname == "run_maxtime")
            {
                //myRun.Run_maxseconds  = (long)Convert.ToInt32(paramvalue);
                theRunForm.txtMaxTime.Text = paramvalue;
            }
            else if (paramname == "pad_enable")
            {
                int next_index = Convert.ToInt32(paramvalue);
                if (next_index <= PADE_List.Count)
                {
                    active_PAD_index = next_index;
                }
            }
            else if (paramname == "chan_enable")
            {
                int ch_num = Convert.ToInt16(paramvalue);
                if ((ch_num >= 0) && (ch_num < 32))
                {
                    PADE thisPADE;
                    try
                    {
                        PADE_List.TryGetValue(active_PAD_index, out thisPADE);
                        thisPADE.PADE_ch_enable[ch_num] = true;
                        thePlot.chk_chan[ch_num].Enabled = true;
                        thePlot.chk_chan[ch_num].Checked = true;
                        myRun.ch_enabled[ch_num] = true;
                    }
                    catch { MessageBox.Show("Invalid PAD is active at index " + active_PAD_index.ToString()); }
                }
            }
            else if (paramname == "chan_minbin")
            {
                int i = Convert.ToInt16(paramvalue);
                if ((i >= 0) && (i < 8093))
                {
                    myRun.min_bin = i;
                }
            }
            else if (paramname == "chan_maxbin")
            {
                int i = Convert.ToInt16(paramvalue);
                if ((i > 0) && (i <= 8093))
                {
                    myRun.max_bin = i;
                }
            }
            else if (paramname == "trig_timeout")
            {
                int i = Convert.ToInt16(paramvalue);
                if (i > 0)
                {
                    myRun.trig_timeout = i;
                }
            }
            else if (paramname == "ext_trig")
            {
                int i = Convert.ToInt16(paramvalue);
                if (i > 0)
                {
                    thePlot.lblSlope.Visible = false;
                    myRun.flgExtTrig = true;
                    myRun.flgCosmicTrig = false;
                    theRunForm.chk_ParamExtTrig.Checked = true;
                    thePlot.chk_ExternalTrig.Checked = true;
                    thePlot.chkCosmicTrig.Checked = false;
                    //thePlot.chk_chan[0].Visible = false;
                    //thePlot.chk_chan[1].Visible = false;
                    //thePlot.chk_chan[2].Visible = false;
                    //thePlot.chk_chan[3].Visible = false;
                }
                else
                {
                    myRun.flgExtTrig = false;
                    theRunForm.chk_ParamExtTrig.Checked = false;
                    thePlot.chk_ExternalTrig.Checked = false;
                }
            }
            else if (paramname == "cosmic_trig")
            {
                int i = Convert.ToInt16(paramvalue);
                if (i > 0)
                {
                    thePlot.lblSlope.Visible = true;
                    myRun.flgCosmicTrig = true;
                    myRun.flgExtTrig = false;
                    myRun.flg_AND_ch0 = false;
                    myRun.flg_AND_ch1 = false;
                    myRun.flg_AND_ch2 = false;
                    myRun.flg_AND_ch3 = false;
                    thePlot.chk_AND_Ch0.Visible = false;
                    thePlot.chk_AND_Ch1.Visible = false;
                    thePlot.chk_AND_Ch2.Visible = false;
                    thePlot.chk_AND_Ch3.Visible = false;
                    thePlot.chk_AND_Ch0.Checked = false;
                    thePlot.chk_AND_Ch1.Checked = false;
                    thePlot.chk_AND_Ch2.Checked = false;
                    thePlot.chk_AND_Ch3.Checked = false;
                    thePlot.chk_ExternalTrig.Checked = false;
                    thePlot.chkCosmicTrig.Checked = true;
                }
            }

            else if (paramname == "flg_slope_pos")
            {
                int i = Convert.ToInt16(paramvalue);
                if (i > 0)
                {
                    myRun.flg_slope_pos = true;
                    thePlot.lblSlope.Text = "POSITIVE";
                }
                else
                {
                    myRun.flg_slope_pos = false;
                    thePlot.lblSlope.Text = "NEGATIVE";
                }
            }

            else if (paramname == "and_ch0")
            {
                int i = Convert.ToInt16(paramvalue);
                if (i > 0)
                {
                    myRun.flg_AND_ch0 = true;
                    thePlot.chk_AND_Ch0.Checked = true;
                }
            }
            else if (paramname == "and_ch1")
            {
                int i = Convert.ToInt16(paramvalue);
                if (i > 0)
                {
                    myRun.flg_AND_ch1 = true;
                    thePlot.chk_AND_Ch1.Checked = true;
                }
            }
            else if (paramname == "and_ch2")
            {
                int i = Convert.ToInt16(paramvalue);
                if (i > 0)
                {
                    myRun.flg_AND_ch2 = true;
                    thePlot.chk_AND_Ch2.Checked = true;
                }
            }
            else if (paramname == "and_ch3")
            {
                int i = Convert.ToInt16(paramvalue);
                if (i > 0)
                {
                    myRun.flg_AND_ch3 = true;
                    thePlot.chk_AND_Ch3.Checked = true;
                }
            }
            else if (paramname == "zero_suppress")
            {
                int i = Convert.ToInt16(paramvalue);
                if (i > 0)
                {
                    myRun.flg_ZS = true;
                    theRunForm.chk_ParamZS.Checked = true;
                }
                else
                {
                    myRun.flg_ZS = false;
                    theRunForm.chk_ParamZS.Checked = false;
                }
            }
            else if (paramname == "sum_only")
            {
                int i = Convert.ToInt16(paramvalue);
                if (i > 0)
                {
                    myRun.flgSumOnly = true;
                    theRunForm.chk_ParamSumOnly.Checked = true;
                }
                else
                {
                    myRun.flg_ZS = false;
                    theRunForm.chk_ParamSumOnly.Checked = false;
                }
            }
            else if (paramname == "flg_udp")
            {
                int i = Convert.ToInt16(paramvalue);
                if (i > 0)
                {
                    myRun.flg_UDP = true;
                    thePlot.EnableChanSelection(true); //disable, its enabled by default
                }
                else
                {
                    myRun.flg_UDP = false;
                    thePlot.EnableChanSelection(false); //disable, its enabled by default
                }
            }
            else if (paramname == "flg_delay")
            {
                int i = Convert.ToInt16(paramvalue);
                if (i > 0)
                {
                    myRun.flg_delay = true;
                    if (myRun.DelayTimer.Interval > 10)
                    { myRun.DelayTimer.Enabled = true; };
                }
                else
                {
                    myRun.flg_delay = false;
                    myRun.DelayTimer.Enabled = false;
                }
            }
            else if (paramname == "delay_time")
            {
                int i = Convert.ToInt16(paramvalue);
                if (i > 0)
                {
                    myRun.delay_time = i;
                    myRun.DelayTimer.Interval = i;
                }
            }
            else if (paramname == "event_num_ch")
            {
                int i = Convert.ToInt16(paramvalue);
                if (i > 0)
                {
                    myRun.event_num_ch = i;
                }
            }
            else if (paramname == "event_num_samples")
            {
                int i = Convert.ToInt16(paramvalue);
                if (i > 0)
                {
                    myRun.event_data_length = i;
                }
            }

            else
            { }

        }

        private static void RegisterAssignment(string regname, string regvalue)
        {
            string[] delimeter = new string[4];
            if (regname == "auto_bitslip") { Console.WriteLine("here"); }
            string[] tokens = new string[4];
            delimeter[0] = ",";
            delimeter[1] = ":";
            string qualifier = "";
            tokens = regname.Split(delimeter, StringSplitOptions.None);
            regname = tokens[0].Trim();
            if (tokens.Length > 1)
            {
                qualifier = tokens[1].Trim();
            }
            else
            { qualifier = ""; }


            for (int i = 1; i < TB4_Registers.Length - 1; i++)
            {
                if (TB4_Registers[i] != null)
                {
                    if ((regname == TB4_Registers[i].name.ToLower()) && (TB4_Registers.IsReadOnly == false))
                    {
                        if (qualifier.Length == 0) { qualifier = "DB0"; }
                        if ((qualifier.Length > 0) && (qualifier.ToLower() == TB4_Registers[i].comment.ToLower()))
                        {
                            {
                                try
                                {
                                    UInt16 v = 0;
                                    v = Convert.ToUInt16(regvalue, 16);
                                    TB4_Registers[i].RegWrite(v);
                                    Thread.Sleep(1);
                                    v = regSTATUS.RegRead();
                                    Console.WriteLine(v.ToString());
                                }
                                catch
                                {
                                    string s1 = "could not assign to " + regname + " with val=0x" + regvalue;
                                    //TB4.myMessage.comment = "RegisterAssignment";
                                    //TB4.myMessage.mess = "could not assign register";
                                }
                                break;
                            }
                        }
                    }
                }

            }
        }

        public static void ReadArray(byte A3, byte A2, byte A1, byte A0, UInt16 data_len, int[] rdata)
        {
            byte[] data = new Byte[16500];
            byte[] read_data = new Byte[16500];
            ulong rxsize = 0, txsize = 0;
            UInt32 i; UInt32 j; UInt32 k = 0;
            UInt32 length = 2;
            bool flg_Continue = false;
            UInt32 StartAddr = 0; UInt32 EndAddr = 0;

            if (data_len > 8193) { data_len = 8193; }


            StartAddr = (UInt32)(A3 << 24) + (UInt32)(A2 << 16) + (UInt32)(A1 << 8) + (UInt32)(A0);
            EndAddr = StartAddr + (UInt32)data_len;

            data[0] = 0xA5;
            data[1] = 7;
            data[2] = 0x2; //this is the array read command
            data[3] = A3;
            data[4] = A2;
            data[5] = A1;
            data[6] = A0;
            data[7] = (byte)((EndAddr & 0xff000000) >> 24);
            data[8] = (byte)((EndAddr & 0x00ff0000) >> 16);
            data[9] = (byte)((EndAddr & 0x0000ff00) >> 8);
            if (((EndAddr + 1) & 0x000000ff) == 0xff) { data[9]++; }
            data[10] = (byte)(((EndAddr + 1) & 0x000000ff));
            data[11] = 0;
            i = 11;

            flg_Continue = false;
            USB_AID.FT_Write(data, i);
            while (!flg_Continue)
            {
                if (k > 20) { MessageBox.Show("trouble reading, aborting"); return; }
                Thread.Sleep(1);
                USB_AID.FT_GetStatus(ref rxsize, ref txsize);
                if (rxsize < (ulong)(data_len * 2))
                {
                    k++;
                }
                else
                {
                    length = USB_AID.FT_Read(read_data, rxsize);
                    for (j = 0; j < data_len; j++)
                    {
                        rdata[j] = read_data[j * 2];
                        rdata[j] = 256 * rdata[j] + read_data[j * 2 + 1];
                        //if (j > 2 ) { rdata[j] = rdata[j] / 2 + rdata[j - 1] / 4 + rdata[j - 2] / 4; }
                        flg_Continue = true;
                    }
                }
            }
        }

        [STAThread]
        public static void Main(string[] args)
        {
            #region MB
            //TB4.TB4_Registers[211] = new TB4_Register("MB_CSR", "MB", 0x00001000, 16, false, false, "MAIN");
            //TB4.TB4_Registers[212] = new TB4_Register("MB_EVENT_SIZE", "MB", 0x00002000, 16, false, false, "MAIN");
            //TB4.TB4_Registers[213] = new TB4_Register("MB_CHAN_MASK", "MB", 0x00003000, 16, false, false, "MAIN");
            //TB4.TB4_Registers[214] = new TB4_Register("NUM_SAMPLES", "MB", 0x00004000, 16, false, false, "MAIN");

            //TB4.TB4_Registers[215] = new TB4_Register("DB0_WORD_COUNT", "MB", 0x00030000, 16, true, false, "MAIN");
            //TB4.TB4_Registers[216] = new TB4_Register("DB1_WORD_COUNT", "MB", 0x00031000, 16, true, false, "MAIN");
            //TB4.TB4_Registers[217] = new TB4_Register("DB2_WORD_COUNT", "MB", 0x00032000, 16, true, false, "MAIN");
            //TB4.TB4_Registers[218] = new TB4_Register("DB3_WORD_COUNT", "MB", 0x00033000, 16, true, false, "MAIN");
            //TB4.TB4_Registers[219] = new TB4_Register("MB_FLASH_CONTROL", "MB", 0x00080000, 16, false, false, "MAIN");
            //TB4.TB4_Registers[220] = new TB4_Register("MB_FLASH_OP_CODE", "MB", 0x00081000, 16, false, false, "MAIN");
            //TB4.TB4_Registers[221] = new TB4_Register("MB_FLASH_PAGE_ADDR", "MB", 0x00082000, 16, false, false, "MAIN");
            //TB4.TB4_Registers[222] = new TB4_Register("MB_FLASH_BYTE_ADDR", "MB", 0x00083000, 16, false, false, "MAIN");
            //TB4.TB4_Registers[223] = new TB4_Register("MB_SOFTWARE_TRIGGER", "MB", 0x000FC000, 16, false, true, "MAIN");
            //TB4.TB4_Registers[224] = new TB4_Register("MB_FIRMWARE_VER", "MB", 0x000FD000, 16, true, false, "MAIN");
            //TB4.TB4_Registers[225] = new TB4_Register("MB_SOFTWARE_RESET", "MB", 0x000FF000, 16, false, true, "MAIN");
            //TB4.TB4_Registers[226] = new TB4_Register("DB0_DATA_MEM_START", "MB", 0x00040000, 16, true, false, "MAIN");
            //TB4.TB4_Registers[227] = new TB4_Register("DB1_DATA_MEM_START", "MB", 0x00041000, 16, true, false, "MAIN");
            //TB4.TB4_Registers[228] = new TB4_Register("DB2_DATA_MEM_START", "MB", 0x00042000, 16, true, false, "MAIN");
            //TB4.TB4_Registers[229] = new TB4_Register("DB3_DATA_MEM_START", "MB", 0x00043000, 16, true, false, "MAIN");
            #endregion MB
            #region DB_register Definitions
            //this code is generated automatically from the MEMEORY_MAP Excel sheet
            TB4.TB4_Registers[1] = new TB4_Register("CONTROL_REG", "DB0", 0x00100000, 16, false, false, "MAIN");
            TB4.TB4_Registers[2] = new TB4_Register("STATUS_REG", "DB0", 0x01000000, 16, true, false, "ZS");
            TB4.TB4_Registers[3] = new TB4_Register("CONTROL_REGzs", "DB0", 0x00100000, 16, false, false, "ZS");
            TB4.TB4_Registers[4] = new TB4_Register("DDR_FIFO_WRITE_LOWER", "DB0", 0x0C000000, 16, false, false, "EXPERT");
            TB4.TB4_Registers[5] = new TB4_Register("DDR_FIFO_WRITE_UPPER", "DB0", 0x0C000001, 16, false, false, "EXPERT");
            TB4.TB4_Registers[6] = new TB4_Register("DDR_ADDR_LOWER", "DB0", 0x0C200000, 16, false, false, "EXPERT");
            TB4.TB4_Registers[7] = new TB4_Register("DDR_ADDR_UPPER", "DB0", 0x0C300000, 16, false, false, "EXPERT");
            TB4.TB4_Registers[8] = new TB4_Register("DDR_WRITE_PORT_EXECUTE", "DB0", 0x0C400000, 16, false, true, "EXPERT");
            TB4.TB4_Registers[9] = new TB4_Register("DDR_READ_PORT_EXECUTE", "DB0", 0x0C500000, 16, false, true, "EXPERT");
            TB4.TB4_Registers[10] = new TB4_Register("DDR_FIFO_READ_LOWER", "DB0", 0x0C600000, 16, true, false, "EXPERT");
            TB4.TB4_Registers[11] = new TB4_Register("DDR_FIFO_READ_UPPER", "DB0", 0x0C600001, 16, true, false, "EXPERT");
            TB4.TB4_Registers[12] = new TB4_Register("DDR_TEST_PATT_TYPE", "DB0", 0x0C700000, 16, false, true, "EXPERT");
            TB4.TB4_Registers[13] = new TB4_Register("P0_WRITE_PORT_STATUS", "DB0", 0x0C800000, 16, true, false, "EXPERT");
            TB4.TB4_Registers[14] = new TB4_Register("P0_READ_PORT_STATUS", "DB0", 0x0C900000, 16, true, false, "EXPERT");
            TB4.TB4_Registers[15] = new TB4_Register("P1_WRITE_PORT_STATUS", "DB0", 0x0CA00000, 16, true, false, "EXPERT");
            TB4.TB4_Registers[16] = new TB4_Register("P1_READ_PORT_STATUS", "DB0", 0x0CB00000, 16, true, false, "EXPERT");
            TB4.TB4_Registers[17] = new TB4_Register("P2_READ_PORT_STATUS", "DB0", 0x0CC00000, 16, true, false, "EXPERT");
            TB4.TB4_Registers[18] = new TB4_Register("P3_READ_PORT_STATUS", "DB0", 0x0CD00000, 16, true, false, "EXPERT");
            TB4.TB4_Registers[19] = new TB4_Register("P4_READ_PORT_STATUS", "DB0", 0x0CE00000, 16, true, false, "EXPERT");
            TB4.TB4_Registers[20] = new TB4_Register("P5_READ_PORT_STATUS", "DB0", 0x0CF00000, 16, true, false, "EXPERT");
            TB4.TB4_Registers[21] = new TB4_Register("BIAS_DAC", "DB0", 0x00A00000, 16, false, true, "BIAS");
            TB4.TB4_Registers[22] = new TB4_Register("BIAS_VMON", "DB0", 0x00B00000, 16, true, false, "BIAS");
            TB4.TB4_Registers[23] = new TB4_Register("BIAS_IMON", "DB0", 0x00C00000, 16, true, false, "BIAS");
            TB4.TB4_Registers[24] = new TB4_Register("BIAS_NUM_AVG", "DB0", 0x00D00000, 16, false, false, "BIAS");
            TB4.TB4_Registers[25] = new TB4_Register("REG_ENABLE", "DB0", 0x02300000, 16, false, true, "MAIN");
            TB4.TB4_Registers[26] = new TB4_Register("XSHTDN", "DB0", 0x02400000, 16, false, true, "MAIN");
            TB4.TB4_Registers[27] = new TB4_Register("ADC3_REG_CNTRL", "DB0", 0x00700003, 16, false, true, "MAIN");
            TB4.TB4_Registers[28] = new TB4_Register("CONTROL_REG_SLAVE1", "DB0", 0x10100000, 16, false, false, "MAIN");
            TB4.TB4_Registers[29] = new TB4_Register("CONTROL_REG_SLAVE2", "DB0", 0x20100000, 16, false, false, "MAIN");
            TB4.TB4_Registers[30] = new TB4_Register("CONTROL_REG_SLAVE3", "DB0", 0x30100000, 16, false, false, "MAIN");
            TB4.TB4_Registers[31] = new TB4_Register("CONTROL_REG_SLAVE4", "DB0", 0x40100000, 16, false, false, "MAIN");
            TB4.TB4_Registers[32] = new TB4_Register("ZS_TOTAL_FRAMES", "DB0", 0x0A300000, 16, false, false, "ZS");
            TB4.TB4_Registers[33] = new TB4_Register("ADC2_REG_CNTRL", "DB0", 0x00700002, 16, false, true, "MAIN");
            TB4.TB4_Registers[34] = new TB4_Register("ADC1_REG_CNTRL", "DB0", 0x00700001, 16, false, true, "MAIN");
            TB4.TB4_Registers[35] = new TB4_Register("ADC0_REG_CNTRL", "DB0", 0x00700000, 16, false, true, "MAIN");
            TB4.TB4_Registers[36] = new TB4_Register("PED_SUBTRACT_0", "DB0", 0x06200000, 16, false, false, "EXPERT");
            TB4.TB4_Registers[37] = new TB4_Register("PED_SUBTRACT_1", "DB0", 0x06200001, 16, false, false, "EXPERT");
            TB4.TB4_Registers[38] = new TB4_Register("PED_SUBTRACT_2", "DB0", 0x06200002, 16, false, false, "EXPERT");
            TB4.TB4_Registers[39] = new TB4_Register("PED_SUBTRACT_3", "DB0", 0x06200003, 16, false, false, "EXPERT");
            TB4.TB4_Registers[40] = new TB4_Register("PED_SUBTRACT_4", "DB0", 0x06200004, 16, false, false, "EXPERT");
            TB4.TB4_Registers[41] = new TB4_Register("PED_SUBTRACT_5", "DB0", 0x06200005, 16, false, false, "EXPERT");
            TB4.TB4_Registers[42] = new TB4_Register("PED_SUBTRACT_6", "DB0", 0x06200006, 16, false, false, "EXPERT");
            TB4.TB4_Registers[43] = new TB4_Register("PED_SUBTRACT_7", "DB0", 0x06200007, 16, false, false, "EXPERT");
            TB4.TB4_Registers[44] = new TB4_Register("PED_SUBTRACT_8", "DB0", 0x06200008, 16, false, false, "EXPERT");
            TB4.TB4_Registers[45] = new TB4_Register("PED_SUBTRACT_9", "DB0", 0x06200009, 16, false, false, "EXPERT");
            TB4.TB4_Registers[46] = new TB4_Register("PED_SUBTRACT_10", "DB0", 0x0620000A, 16, false, false, "EXPERT");
            TB4.TB4_Registers[47] = new TB4_Register("PED_SUBTRACT_11", "DB0", 0x0620000B, 16, false, false, "EXPERT");
            TB4.TB4_Registers[48] = new TB4_Register("PED_SUBTRACT_12", "DB0", 0x0620000C, 16, false, false, "EXPERT");
            TB4.TB4_Registers[49] = new TB4_Register("PED_SUBTRACT_13", "DB0", 0x0620000D, 16, false, false, "EXPERT");
            TB4.TB4_Registers[50] = new TB4_Register("PED_SUBTRACT_14", "DB0", 0x0620000E, 16, false, false, "EXPERT");
            TB4.TB4_Registers[51] = new TB4_Register("PED_SUBTRACT_15", "DB0", 0x0620000F, 16, false, false, "EXPERT");
            TB4.TB4_Registers[52] = new TB4_Register("PED_SUBTRACT_16", "DB0", 0x06200010, 16, false, false, "EXPERT");
            TB4.TB4_Registers[53] = new TB4_Register("PED_SUBTRACT_17", "DB0", 0x06200011, 16, false, false, "EXPERT");
            TB4.TB4_Registers[54] = new TB4_Register("PED_SUBTRACT_18", "DB0", 0x06200012, 16, false, false, "EXPERT");
            TB4.TB4_Registers[55] = new TB4_Register("PED_SUBTRACT_19", "DB0", 0x06200013, 16, false, false, "EXPERT");
            TB4.TB4_Registers[56] = new TB4_Register("PED_SUBTRACT_20", "DB0", 0x06200014, 16, false, false, "EXPERT");
            TB4.TB4_Registers[57] = new TB4_Register("PED_SUBTRACT_21", "DB0", 0x06200015, 16, false, false, "EXPERT");
            TB4.TB4_Registers[58] = new TB4_Register("PED_SUBTRACT_22", "DB0", 0x06200016, 16, false, false, "EXPERT");
            TB4.TB4_Registers[59] = new TB4_Register("PED_SUBTRACT_23", "DB0", 0x06200017, 16, false, false, "EXPERT");
            TB4.TB4_Registers[60] = new TB4_Register("PED_SUBTRACT_24", "DB0", 0x06200018, 16, false, false, "EXPERT");
            TB4.TB4_Registers[61] = new TB4_Register("PED_SUBTRACT_25", "DB0", 0x06200019, 16, false, false, "EXPERT");
            TB4.TB4_Registers[62] = new TB4_Register("PED_SUBTRACT_26", "DB0", 0x0620001A, 16, false, false, "EXPERT");
            TB4.TB4_Registers[63] = new TB4_Register("PED_SUBTRACT_27", "DB0", 0x0620001B, 16, false, false, "EXPERT");
            TB4.TB4_Registers[64] = new TB4_Register("PED_SUBTRACT_28", "DB0", 0x0620001C, 16, false, false, "EXPERT");
            TB4.TB4_Registers[65] = new TB4_Register("PED_SUBTRACT_29", "DB0", 0x0620001D, 16, false, false, "EXPERT");
            TB4.TB4_Registers[66] = new TB4_Register("PED_SUBTRACT_30", "DB0", 0x0620001E, 16, false, false, "EXPERT");
            TB4.TB4_Registers[67] = new TB4_Register("PED_SUBTRACT_31", "DB0", 0x0620001F, 16, false, false, "EXPERT");
            TB4.TB4_Registers[68] = new TB4_Register("VGA_GAIN_ADC0", "DB0", 0x00600000, 16, false, true, "MAIN");
            TB4.TB4_Registers[69] = new TB4_Register("VGA_GAIN_ADC1", "DB0", 0x00600001, 16, false, true, "MAIN");
            TB4.TB4_Registers[70] = new TB4_Register("VGA_GAIN_ADC2", "DB0", 0x00600002, 16, false, true, "MAIN");
            TB4.TB4_Registers[71] = new TB4_Register("VGA_GAIN_ADC3", "DB0", 0x00600003, 16, false, true, "MAIN");
            TB4.TB4_Registers[72] = new TB4_Register("FRAME_ALIGNED", "DB0", 0x00500000, 16, true, false, "MAIN");
            TB4.TB4_Registers[73] = new TB4_Register("ADC_STBY", "DB0", 0x02100000, 16, false, true, "MAIN");
            TB4.TB4_Registers[74] = new TB4_Register("ADC_PWDN", "DB0", 0x02200000, 16, false, true, "MAIN");
            TB4.TB4_Registers[78] = new TB4_Register("AUTO_BITSLIP", "DB0", 0x00500004, 16, false, true, "MAIN");
            TB4.TB4_Registers[79] = new TB4_Register("AUTO_PED_SET", "DB0", 0x06400000, 16, false, false, "MAIN");
            TB4.TB4_Registers[82] = new TB4_Register("ADC_SPI_CNTRL_CH0", "DB0", 0x02000000, 16, false, false, "MAIN");
            TB4.TB4_Registers[83] = new TB4_Register("ADC_SPI_CNTRL_CH1", "DB0", 0x02000001, 16, false, false, "MAIN");
            TB4.TB4_Registers[84] = new TB4_Register("ADC_SPI_CNTRL_CH2", "DB0", 0x02000002, 16, false, false, "MAIN");
            TB4.TB4_Registers[85] = new TB4_Register("ADC_SPI_CNTRL_CH3", "DB0", 0x02000003, 16, false, false, "MAIN");
            TB4.TB4_Registers[86] = new TB4_Register("FIRMWARE_VER", "DB0", 0x0FD00000, 16, false, false, "MAIN");
            TB4.TB4_Registers[87] = new TB4_Register("TEMPERATURE", "DB0", 0x0FC00000, 16, true, false, "MAIN");
            TB4.TB4_Registers[89] = new TB4_Register("TRIG_THRESHOLD_CH0", "DB0", 0x04400000, 16, false, false, "MAIN");
            TB4.TB4_Registers[90] = new TB4_Register("TRIG_THRESHOLD_CH1", "DB0", 0x04500000, 16, false, false, "MAIN");
            TB4.TB4_Registers[91] = new TB4_Register("TRIG_THRESHOLD_CH2", "DB0", 0x04600000, 16, false, false, "MAIN");
            TB4.TB4_Registers[92] = new TB4_Register("TRIG_THRESHOLD_CH3", "DB0", 0x04700000, 16, false, false, "MAIN");
            TB4.TB4_Registers[93] = new TB4_Register("TRIG_POST_STORE", "DB0", 0x04800000, 16, false, false, "MAIN");
            TB4.TB4_Registers[95] = new TB4_Register("IO_RESET", "DB0", 0x00400000, 16, false, true, "EXPERT");
            TB4.TB4_Registers[96] = new TB4_Register("HDMI_STATUS", "DB0", 0x00200000, 16, true, false, "MAIN");
            TB4.TB4_Registers[97] = new TB4_Register("ENUMERATE_SLAVES", "DB0", 0xF0000000, 16, false, true, "MAIN");
            TB4.TB4_Registers[98] = new TB4_Register("HDMI_RESET", "DB0", 0x0F900000, 16, false, true, "MAIN");
            TB4.TB4_Registers[99] = new TB4_Register("SOFTWARE_TRIGGER", "DB0", 0x0F800000, 16, false, true, "ZS");
            TB4.TB4_Registers[100] = new TB4_Register("PARAMETER_INIT", "DB0", 0x0F500000, 16, false, true, "FLASH");
            TB4.TB4_Registers[101] = new TB4_Register("ZERO_SUPPRESS_PRESAMPLE", "DB0", 0x0A100000, 16, false, false, "ZS");
            TB4.TB4_Registers[102] = new TB4_Register("ZERO_SUPPRESS_TOTAL_EVENTS", "DB0", 0x0A200000, 16, false, false, "ZS");
            TB4.TB4_Registers[103] = new TB4_Register("READ_POINTER_OFFSET", "DB0", 0x06000000, 16, false, false, "MAIN");
            TB4.TB4_Registers[104] = new TB4_Register("ERR_LATCH_UPPER", "DB0", 0x0FB00000, 16, true, false, "MAIN");
            TB4.TB4_Registers[105] = new TB4_Register("ERR_LATCH_LOWER", "DB0", 0x0FA00000, 16, true, false, "MAIN");
            TB4.TB4_Registers[106] = new TB4_Register("SOFTWARE_RESET", "DB0", 0x0FF00000, 16, false, true, "MAIN");
            TB4.TB4_Registers[107] = new TB4_Register("BIAS_OFFSET_CH0", "DB0", 0x00800000, 16, false, true, "BIAS");
            TB4.TB4_Registers[108] = new TB4_Register("BIAS_OFFSET_CH1", "DB0", 0x00800001, 16, false, true, "BIAS");
            TB4.TB4_Registers[109] = new TB4_Register("BIAS_OFFSET_CH2", "DB0", 0x00800002, 16, false, true, "BIAS");
            TB4.TB4_Registers[110] = new TB4_Register("BIAS_OFFSET_CH3", "DB0", 0x00800003, 16, false, true, "BIAS");
            TB4.TB4_Registers[111] = new TB4_Register("BIAS_OFFSET_CH4", "DB0", 0x00800004, 16, false, true, "BIAS");
            TB4.TB4_Registers[112] = new TB4_Register("BIAS_OFFSET_CH5", "DB0", 0x00800005, 16, false, true, "BIAS");
            TB4.TB4_Registers[113] = new TB4_Register("BIAS_OFFSET_CH6", "DB0", 0x00800006, 16, false, true, "BIAS");
            TB4.TB4_Registers[114] = new TB4_Register("BIAS_OFFSET_CH7", "DB0", 0x00800007, 16, false, true, "BIAS");
            TB4.TB4_Registers[115] = new TB4_Register("BIAS_OFFSET_CH8", "DB0", 0x00800008, 16, false, true, "BIAS");
            TB4.TB4_Registers[116] = new TB4_Register("BIAS_OFFSET_CH9", "DB0", 0x00800009, 16, false, true, "BIAS");
            TB4.TB4_Registers[117] = new TB4_Register("BIAS_OFFSET_CH10", "DB0", 0x0080000A, 16, false, true, "BIAS");
            TB4.TB4_Registers[118] = new TB4_Register("BIAS_OFFSET_CH11", "DB0", 0x0080000B, 16, false, true, "BIAS");
            TB4.TB4_Registers[119] = new TB4_Register("BIAS_OFFSET_CH12", "DB0", 0x0080000C, 16, false, true, "BIAS");
            TB4.TB4_Registers[120] = new TB4_Register("BIAS_OFFSET_CH13", "DB0", 0x0080000D, 16, false, true, "BIAS");
            TB4.TB4_Registers[121] = new TB4_Register("BIAS_OFFSET_CH14", "DB0", 0x0080000E, 16, false, true, "BIAS");
            TB4.TB4_Registers[122] = new TB4_Register("BIAS_OFFSET_CH15", "DB0", 0x0080000F, 16, false, true, "BIAS");
            TB4.TB4_Registers[123] = new TB4_Register("BIAS_OFFSET_CH16", "DB0", 0x00800010, 16, false, true, "BIAS");
            TB4.TB4_Registers[124] = new TB4_Register("BIAS_OFFSET_CH17", "DB0", 0x00800011, 16, false, true, "BIAS");
            TB4.TB4_Registers[125] = new TB4_Register("BIAS_OFFSET_CH18", "DB0", 0x00800012, 16, false, true, "BIAS");
            TB4.TB4_Registers[126] = new TB4_Register("BIAS_OFFSET_CH19", "DB0", 0x00800013, 16, false, true, "BIAS");
            TB4.TB4_Registers[127] = new TB4_Register("BIAS_OFFSET_CH20", "DB0", 0x00800014, 16, false, true, "BIAS");
            TB4.TB4_Registers[128] = new TB4_Register("BIAS_OFFSET_CH21", "DB0", 0x00800015, 16, false, true, "BIAS");
            TB4.TB4_Registers[129] = new TB4_Register("BIAS_OFFSET_CH22", "DB0", 0x00800016, 16, false, true, "BIAS");
            TB4.TB4_Registers[130] = new TB4_Register("BIAS_OFFSET_CH23", "DB0", 0x00800017, 16, false, true, "BIAS");
            TB4.TB4_Registers[131] = new TB4_Register("BIAS_OFFSET_CH24", "DB0", 0x00800018, 16, false, true, "BIAS");
            TB4.TB4_Registers[132] = new TB4_Register("BIAS_OFFSET_CH25", "DB0", 0x00800019, 16, false, true, "BIAS");
            TB4.TB4_Registers[133] = new TB4_Register("BIAS_OFFSET_CH26", "DB0", 0x0080001A, 16, false, true, "BIAS");
            TB4.TB4_Registers[134] = new TB4_Register("BIAS_OFFSET_CH27", "DB0", 0x0080001B, 16, false, true, "BIAS");
            TB4.TB4_Registers[135] = new TB4_Register("BIAS_OFFSET_CH28", "DB0", 0x0080001C, 16, false, true, "BIAS");
            TB4.TB4_Registers[136] = new TB4_Register("BIAS_OFFSET_CH29", "DB0", 0x0080001D, 16, false, true, "BIAS");
            TB4.TB4_Registers[137] = new TB4_Register("BIAS_OFFSET_CH30", "DB0", 0x0080001E, 16, false, true, "BIAS");
            TB4.TB4_Registers[138] = new TB4_Register("BIAS_OFFSET_CH31", "DB0", 0x0080001F, 16, false, true, "BIAS");
            TB4.TB4_Registers[139] = new TB4_Register("ZERO_SUPPRESS_EVENTS", "DB0", 0x0A000000, 16, true, false, "ZS");
            TB4.TB4_Registers[140] = new TB4_Register("ZERO_SUPPRESS_MAX_HITS", "DB0", 0x0A400000, 16, false, false, "ZS");
            TB4.TB4_Registers[141] = new TB4_Register("FRAME_LENGTH", "DB0", 0x0A500000, 16, false, false, "ZS");
            TB4.TB4_Registers[142] = new TB4_Register("TRIG_DELAY_MASTER", "DB0", 0x04900000, 16, false, false, "MAIN");
            TB4.TB4_Registers[143] = new TB4_Register("TRIG_DELAY_SLAVE1", "DB0", 0x14900000, 16, false, false, "MAIN");
            TB4.TB4_Registers[144] = new TB4_Register("TRIG_DELAY_SLAVE2", "DB0", 0x24900000, 16, false, false, "MAIN");
            TB4.TB4_Registers[145] = new TB4_Register("TRIG_DELAY_SLAVE3", "DB0", 0x34900000, 16, false, false, "MAIN");
            TB4.TB4_Registers[146] = new TB4_Register("TRIG_DELAY_SLAVE4", "DB0", 0x44900000, 16, false, false, "MAIN");
            TB4.TB4_Registers[150] = new TB4_Register("CSR", "DB0", 0x00100000, 16, false, false, "THR");
            TB4.TB4_Registers[151] = new TB4_Register("THR_SCAN_TIME", "DB0", 0x07000000, 16, false, false, "THR");
            TB4.TB4_Registers[152] = new TB4_Register("THR_00", "DB0", 0x07200000, 16, true, false, "THR");
            TB4.TB4_Registers[153] = new TB4_Register("THR_01", "DB0", 0x07200001, 16, true, false, "THR");
            TB4.TB4_Registers[154] = new TB4_Register("THR_02", "DB0", 0x07200002, 16, true, false, "THR");
            TB4.TB4_Registers[155] = new TB4_Register("THR_03", "DB0", 0x07200003, 16, true, false, "THR");
            TB4.TB4_Registers[156] = new TB4_Register("THR_04", "DB0", 0x07200004, 16, true, false, "THR");
            TB4.TB4_Registers[157] = new TB4_Register("THR_05", "DB0", 0x07200005, 16, true, false, "THR");
            TB4.TB4_Registers[158] = new TB4_Register("THR_06", "DB0", 0x07200006, 16, true, false, "THR");
            TB4.TB4_Registers[159] = new TB4_Register("THR_07", "DB0", 0x07200007, 16, true, false, "THR");
            TB4.TB4_Registers[160] = new TB4_Register("THR_08", "DB0", 0x07200008, 16, true, false, "THR");
            TB4.TB4_Registers[161] = new TB4_Register("THR_09", "DB0", 0x07200009, 16, true, false, "THR");
            TB4.TB4_Registers[162] = new TB4_Register("THR_10", "DB0", 0x0720000a, 16, true, false, "THR");
            TB4.TB4_Registers[163] = new TB4_Register("THR_11", "DB0", 0x0720000b, 16, true, false, "THR");
            TB4.TB4_Registers[164] = new TB4_Register("THR_12", "DB0", 0x0720000c, 16, true, false, "THR");
            TB4.TB4_Registers[165] = new TB4_Register("THR_13", "DB0", 0x0720000d, 16, true, false, "THR");
            TB4.TB4_Registers[166] = new TB4_Register("THR_14", "DB0", 0x0720000e, 16, true, false, "THR");
            TB4.TB4_Registers[167] = new TB4_Register("THR_15", "DB0", 0x0720000f, 16, true, false, "THR");
            TB4.TB4_Registers[168] = new TB4_Register("THR_16", "DB0", 0x07200010, 16, true, false, "THR");
            TB4.TB4_Registers[169] = new TB4_Register("THR_17", "DB0", 0x07200011, 16, true, false, "THR");
            TB4.TB4_Registers[170] = new TB4_Register("THR_18", "DB0", 0x07200012, 16, true, false, "THR");
            TB4.TB4_Registers[171] = new TB4_Register("THR_19", "DB0", 0x07200013, 16, true, false, "THR");
            TB4.TB4_Registers[172] = new TB4_Register("THR_20", "DB0", 0x07200014, 16, true, false, "THR");
            TB4.TB4_Registers[173] = new TB4_Register("THR_21", "DB0", 0x07200015, 16, true, false, "THR");
            TB4.TB4_Registers[174] = new TB4_Register("THR_22", "DB0", 0x07200016, 16, true, false, "THR");
            TB4.TB4_Registers[175] = new TB4_Register("THR_23", "DB0", 0x07200017, 16, true, false, "THR");
            TB4.TB4_Registers[176] = new TB4_Register("THR_24", "DB0", 0x07200018, 16, true, false, "THR");
            TB4.TB4_Registers[177] = new TB4_Register("THR_25", "DB0", 0x07200019, 16, true, false, "THR");
            TB4.TB4_Registers[178] = new TB4_Register("THR_26", "DB0", 0x0720001a, 16, true, false, "THR");
            TB4.TB4_Registers[179] = new TB4_Register("THR_27", "DB0", 0x0720001b, 16, true, false, "THR");
            TB4.TB4_Registers[180] = new TB4_Register("THR_28", "DB0", 0x0720001c, 16, true, false, "THR");
            TB4.TB4_Registers[181] = new TB4_Register("THR_29", "DB0", 0x0720001d, 16, true, false, "THR");
            TB4.TB4_Registers[182] = new TB4_Register("THR_30", "DB0", 0x0720001e, 16, true, false, "THR");
            TB4.TB4_Registers[183] = new TB4_Register("THR_31", "DB0", 0x0720001f, 16, true, false, "THR");
            TB4.TB4_Registers[184] = new TB4_Register("TRIG_THRESHOLD_CH0-7", "DB0", 0x04400000, 16, false, false, "THR");
            TB4.TB4_Registers[185] = new TB4_Register("TRIG_THRESHOLD_CH8-15", "DB0", 0x04500000, 16, false, false, "THR");
            TB4.TB4_Registers[186] = new TB4_Register("TRIG_THRESHOLD_CH16-23", "DB0", 0x04600000, 16, false, false, "THR");
            TB4.TB4_Registers[187] = new TB4_Register("TRIG_THRESHOLD_CH24-31", "DB0", 0x04700000, 16, false, false, "THR");

            TB4.TB4_Registers[188] = new TB4_Register("SOFT_TRIG", "DB0", 0x0F800000, 16, false, false, "THR");

            TB4.TB4_Registers[201] = new TB4_Register("MAC_RESET", "DB0", 0x0F700000, 32, false, true, "ETH");
            TB4.TB4_Registers[202] = new TB4_Register("MAC_CMD", "DB0", 0x0090FC00, 32, false, false, "ETH1");
            TB4.TB4_Registers[203] = new TB4_Register("MAC_IMR", "DB0", 0x0090FC04, 32, false, false, "ETH1");
            TB4.TB4_Registers[204] = new TB4_Register("MAC_ISR", "DB0", 0x0090FC08, 32, false, false, "ETH1");
            TB4.TB4_Registers[205] = new TB4_Register("MAC_TX_CFG", "DB0", 0x0090FC10, 32, false, false, "ETH1");
            TB4.TB4_Registers[206] = new TB4_Register("MAC_TX_CMD", "DB0", 0x0090FC14, 32, false, false, "ETH1");
            TB4.TB4_Registers[207] = new TB4_Register("MAC_TXBS", "DB0", 0x0090FC18, 32, false, false, "ETH1");
            TB4.TB4_Registers[208] = new TB4_Register("MAC_TX_DESC_0", "DB0", 0x0090FC20, 32, false, false, "ETH1");
            TB4.TB4_Registers[209] = new TB4_Register("MAC_TX_DESC_1", "DB0", 0x0090FC24, 32, false, false, "ETH1");
            TB4.TB4_Registers[210] = new TB4_Register("MAC_TX_DESC_2", "DB0", 0x0090FC28, 32, false, false, "ETH1");
            TB4.TB4_Registers[211] = new TB4_Register("MAC_TX_DESC_3", "DB0", 0x0090FC2C, 32, false, false, "ETH1");
            TB4.TB4_Registers[212] = new TB4_Register("MAC_RX_CFG", "DB0", 0x0090FC30, 32, false, false, "ETH1");
            TB4.TB4_Registers[213] = new TB4_Register("MAC_RXCURT", "DB0", 0x0090FC34, 32, false, false, "ETH1");
            TB4.TB4_Registers[214] = new TB4_Register("MAC_RX_BOUND", "DB0", 0x0090FC38, 32, false, false, "ETH1");
            TB4.TB4_Registers[215] = new TB4_Register("MAC_CFG0", "DB0", 0x0090FC40, 32, false, false, "ETH1");
            TB4.TB4_Registers[216] = new TB4_Register("MAC_CFG1", "DB0", 0x0090FC44, 32, false, false, "ETH1");
            TB4.TB4_Registers[217] = new TB4_Register("MAC_CFG2", "DB0", 0x0090FC48, 32, false, false, "ETH1");
            TB4.TB4_Registers[218] = new TB4_Register("MAC_CFG3", "DB0", 0x0090FC4C, 32, false, false, "ETH1");
            TB4.TB4_Registers[219] = new TB4_Register("MAC_TXPAUT", "DB0", 0x0090FC54, 32, false, false, "ETH1");
            TB4.TB4_Registers[220] = new TB4_Register("MAC_RXBTHD0", "DB0", 0x0090FC58, 32, false, false, "ETH1");
            TB4.TB4_Registers[221] = new TB4_Register("MAC_RXBTHD1", "DB0", 0x0090FC5C, 32, false, false, "ETH1");
            TB4.TB4_Registers[222] = new TB4_Register("MAC_RXFULTHD", "DB0", 0x0090FC60, 32, false, false, "ETH1");
            TB4.TB4_Registers[223] = new TB4_Register("MAC_MISC", "DB0", 0x0090FC68, 32, false, false, "ETH1");
            TB4.TB4_Registers[224] = new TB4_Register("MAC_MACID0", "DB0", 0x0090FC70, 32, false, false, "ETH1");
            TB4.TB4_Registers[225] = new TB4_Register("MAC_MACID1", "DB0", 0x0090FC74, 32, false, false, "ETH1");
            TB4.TB4_Registers[226] = new TB4_Register("MAC_MACID2", "DB0", 0x0090FC78, 32, false, false, "ETH1");
            TB4.TB4_Registers[227] = new TB4_Register("MAC_TXLEN", "DB0", 0x0090FC7C, 32, false, false, "ETH1");
            TB4.TB4_Registers[228] = new TB4_Register("MAC_RXFILTER", "DB0", 0x0090FC80, 32, false, false, "ETH1");

            TB4.TB4_Registers[229] = new TB4_Register("PHY_CNTRL", "DB0", 0x0090FC84, 32, false, false, "ETH");
            TB4.TB4_Registers[230] = new TB4_Register("PHY_DATA", "DB0", 0x0090FC88, 32, false, false, "ETH");
            //TB4.TB4_Registers[231] = new TB4_Register("MAC_GPIO_CTRL", "DB0", 0x0090FC8C, 32, false, false, "ETH");
            TB4.TB4_Registers[232] = new TB4_Register("MAC_RXINDICATOR", "DB0", 0x0090FC90, 32, false, false, "ETH");
            TB4.TB4_Registers[233] = new TB4_Register("MAC_TXST", "DB0", 0x0090FC94, 32, false, false, "ETH");
            TB4.TB4_Registers[234] = new TB4_Register("MAC_MDCLKPAT", "DB0", 0x0090FCA0, 32, false, false, "ETH");
            TB4.TB4_Registers[235] = new TB4_Register("MAC_RXCHKSUMCNT", "DB0", 0x0090FCA4, 32, false, false, "ETH");
            TB4.TB4_Registers[236] = new TB4_Register("MAC_RXCRCNT", "DB0", 0x0090FCA8, 32, false, false, "ETH");
            TB4.TB4_Registers[237] = new TB4_Register("MAC_TXFAILCNT", "DB0", 0x0090FCAC, 32, false, false, "ETH");
            TB4.TB4_Registers[238] = new TB4_Register("MAC_MAXRXLEN", "DB0", 0x0090FCB8, 32, false, false, "ETH");
            TB4.TB4_Registers[239] = new TB4_Register("MAC_SOFTRST", "DB0", 0x0090FCEC, 32, false, false, "ETH");

            TB4.TB4_Registers[240] = new TB4_Register("MAC_UPPERDATA", "DB0", 0x00A00000, 32, false, false, "ETH");

            TB4.TB4_Registers[241] = new TB4_Register("MAC_SOURCE_ADDR_LOW", "DB0", 0x03100000, 16, false, false, "ETH");
            TB4.TB4_Registers[242] = new TB4_Register("MAC_SOURCE_ADDR_MED", "DB0", 0x03100001, 16, false, false, "ETH");
            TB4.TB4_Registers[243] = new TB4_Register("MAC_SOURCE_ADDR_HIGH", "DB0", 0x03100002, 16, false, false, "ETH");
            TB4.TB4_Registers[244] = new TB4_Register("MAC_DEST_ADDR_LOW", "DB0", 0x03200000, 16, false, false, "ETH");
            TB4.TB4_Registers[245] = new TB4_Register("MAC_DEST_ADDR_MED", "DB0", 0x03200001, 16, false, false, "ETH");
            TB4.TB4_Registers[246] = new TB4_Register("MAC_DEST_ADDR_HIGH", "DB0", 0x03200002, 16, false, false, "ETH");
            TB4.TB4_Registers[247] = new TB4_Register("MAC_SOURCE_IP_LOW", "DB0", 0x03300000, 16, false, false, "ETH");
            TB4.TB4_Registers[248] = new TB4_Register("MAC_SOURCE_IP_HIGH", "DB0", 0x03300001, 16, false, false, "ETH");
            TB4.TB4_Registers[249] = new TB4_Register("MAC_DEST_IP_LOW", "DB0", 0x03400000, 16, false, false, "ETH");
            TB4.TB4_Registers[250] = new TB4_Register("MAC_DEST IP_HIGH", "DB0", 0x03400001, 16, false, false, "ETH");
            TB4.TB4_Registers[251] = new TB4_Register("MAC_TEST_PACK_LOW", "DB0", 0x03500000, 16, false, false, "ETH");
            TB4.TB4_Registers[252] = new TB4_Register("MAC_TEST_PACK_HIGH", "DB0", 0x03500001, 16, false, false, "ETH");
            TB4.TB4_Registers[253] = new TB4_Register("MAC_TEST_DATA_LENGTH", "DB0", 0x03600000, 16, false, false, "ETH");


            #endregion DB_register Definitions

            #region FLASH_DPRAM
            TB4.TB4_Registers[254] = new TB4_Register("HARD_RESET", "DB0", 0x0FE00000, 16, false, true, "FLASH");
            TB4.TB4_Registers[255] = new TB4_Register("FLASH_BINARY", "DB0", 0x00300000, 16, false, true, "FLASH");
            TB4.TB4_Registers[256] = new TB4_Register("FLASH_CONTROL", "DB0", 0x08000000, 16, false, false, "FLASH");
            TB4.TB4_Registers[257] = new TB4_Register("FLASH_OP_CODE", "DB0", 0x08100000, 16, false, false, "FLASH");
            TB4.TB4_Registers[258] = new TB4_Register("FLASH_PAGE_ADDR", "DB0", 0x08200000, 16, false, false, "FLASH");
            TB4.TB4_Registers[259] = new TB4_Register("FLASH_BYTE_ADDR", "DB0", 0x08300000, 16, false, false, "FLASH");
            TB4.TB4_Registers[260] = new TB4_Register("FLASH_RAM_0", "DB0", 0x08800000, 16, false, false, "FLASH");
            TB4.TB4_Registers[261] = new TB4_Register("FLASH_RAM_1", "DB0", 0x08800001, 16, false, false, "FLASH");
            TB4.TB4_Registers[262] = new TB4_Register("FLASH_RAM_2", "DB0", 0x08800002, 16, false, false, "FLASH");
            TB4.TB4_Registers[263] = new TB4_Register("FLASH_RAM_3", "DB0", 0x08800003, 16, false, false, "FLASH");
            TB4.TB4_Registers[264] = new TB4_Register("FLASH_RAM_4", "DB0", 0x08800004, 16, false, false, "FLASH");
            TB4.TB4_Registers[265] = new TB4_Register("FLASH_RAM_5", "DB0", 0x08800005, 16, false, false, "FLASH");
            TB4.TB4_Registers[266] = new TB4_Register("FLASH_RAM_6", "DB0", 0x08800006, 16, false, false, "FLASH");
            TB4.TB4_Registers[267] = new TB4_Register("FLASH_RAM_7", "DB0", 0x08800007, 16, false, false, "FLASH");
            TB4.TB4_Registers[268] = new TB4_Register("FLASH_RAM_8", "DB0", 0x08800008, 16, false, false, "FLASH");
            TB4.TB4_Registers[269] = new TB4_Register("FLASH_RAM_9", "DB0", 0x08800009, 16, false, false, "FLASH");
            TB4.TB4_Registers[270] = new TB4_Register("FLASH_RAM_A", "DB0", 0x0880000A, 16, false, false, "FLASH");
            TB4.TB4_Registers[271] = new TB4_Register("FLASH_RAM_B", "DB0", 0x0880000B, 16, false, false, "FLASH");
            TB4.TB4_Registers[272] = new TB4_Register("FLASH_RAM_C", "DB0", 0x0880000C, 16, false, false, "FLASH");
            TB4.TB4_Registers[273] = new TB4_Register("FLASH_RAM_D", "DB0", 0x0880000D, 16, false, false, "FLASH");
            TB4.TB4_Registers[274] = new TB4_Register("FLASH_RAM_E", "DB0", 0x0880000E, 16, false, false, "FLASH");

            #endregion FLASH_DPRAM

            for (int i = 0; i < TB4_Registers.Length; i++)
            {
                if (TB4_Registers[i] != null)
                {
                    if (TB4_Registers[i].name.Length > 0)
                    {
                        int v = 0;
                        TB4_Registers_Dict.TryGetValue(TB4_Registers[i].name, out v);
                        if (v > 0)
                        { MessageBox.Show("Tom, you defined a duplicate Register " + TB4_Registers[i].name + " at index " + i.ToString()); }
                        else
                        { TB4_Registers_Dict.Add(TB4_Registers[i].name, i); }
                    }
                }

            }

            Application.EnableVisualStyles();
            theRunForm.Visible = false;

            TB4.thePlot = new Plot0();
            thePlot.Visible = false;

            TB4.theFlash = new Flash0();
            theFlash.Visible = false;

            TB4.theBiasOffset = new BiasOffset();
            theBiasOffset.Visible = false;

            TB4.theHist = new Hist0();
            theHist.Visible = false;
            TB4.theHist1 = new Hist1();
            theHist1.Visible = false;

            TB4.thePAD_selector = new PAD_select();
            thePAD_selector.Visible = false;

            TB4.theGBE = new GBE();
            theGBE.Visible = false;

            TB4.theDRAM = new DRAM();
            theDRAM.Visible = false;

            TB4.myAboutBox.ShowDialog();

            DisplayControl theDisplayControl = new DisplayControl();
            System.Drawing.Rectangle myScreen = Screen.GetBounds(theDisplayControl);
            System.Drawing.Point newLocation = new System.Drawing.Point(myScreen.Width - theDisplayControl.Width, 0);
            theDisplayControl.Location = newLocation;
            theDisplayControl.Show();
            //System.Media.SystemSounds.Beep.Play();
            Application.Run(theRunForm);

        }
    }
}
