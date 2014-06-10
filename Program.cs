using System;
using System.Timers;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Threading;
//using System.Net;
//using System.Net.Sockets;
//using System.Text.RegularExpressions;
using System.IO;
using ZedGraph;
using PcapDotNet.Core;
using PcapDotNet.Packets;
using PcapDotNet.Packets.Ethernet;
using PcapDotNet.Packets.IpV4;
using PcapDotNet.Packets.Icmp;
using PcapDotNet.Packets.Transport;

using PerformanceCounterHelper;
using FTD2XX_NET;
using System.Text;

namespace PADE
{

    public delegate void startupTemplate();

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

        public int trig_timeout = 5;

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
        public int[, ,] event_data_by_ch;
        public string[] raw_udp_string;
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

        //stuff for the plot display
        public bool flgResquestData;
        public bool flgNewDataAvailable;
        public string UDPdata_for_parser;

        private BinaryWriter BinStream;
        public BinaryWriter binstreamRun
        {
            get { return BinStream; }
            set { BinStream = value; }
        }
        //I moved the commented assigment to the Main() function
        //so that I can run this without allowing access to my C drive.
        private StreamWriter privateStream; //= new StreamWriter("c:\\default.dat");

        public StreamWriter streamRun
        {
            get { return privateStream; }
            set { privateStream = value; }
        }

        //constructor
        public TB4_Run()
        {
            try
            {
                string dataPath = "C:\\default.dat";

                privateStream = new StreamWriter(dataPath);
            }
            catch (Exception ex)
            {
                TB4_Exception.logError(ex, "Error opening default data file.", true);
            }
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
            if (TB4.PADE_List.Count < 1)
            {
                MessageBox.Show("You must first add PADEs to the PADE explorer.");
                return;
            }


            this.flgRunning = true;
            this.runCurrentEvents = 0;
            this.RunStartTime = DateTime.Now;  //the only point of this assignment is to have a time when the streamRun failed to close...
            //get run number and current time

            try
            {
                if (this.streamRun != null) { this.streamRun.Close(); }
            }
            catch (Exception ex)
            {
                //TB4.myMessage = new TB4_Message("could not open data stream");
                TB4_Exception.logError(ex, "Could not open data stream.", true);
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
            StreamWriter newRun;
            try
            {
                newRun = new StreamWriter(Run_filename, false);
                this.streamRun = newRun;
            }
            catch (Exception ex)
            {
                TB4_Exception.logError(ex, "Could not open data stream.", true);
                MessageBox.Show(ex.Message);
            }
            ushort res = TB4.regSTATUS.RegRead();
            //if (res == 0) { MessageBox.Show("trying to start run but can not read CSR"); }

            this.Run_event = 0;
            this.RunStartTime = DateTime.Now;
            this.RunStopTime = DateTime.Now.AddSeconds(this.Run_maxseconds);
            this.event_data = new int[512 * 32];
            if (TB4.ActivePADE == null) { TB4.ActivePADE = TB4.PADE_List[1]; }

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
                        if (thisPADE.PADE_is_MASTER) { MasterPADE = thisPADE; } //looks for the Master PADE, does nothing else
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
        double udpAverage = 0;
        int udpCount = 0;
        public void Take_UDP_Event()
        {
            /*
            DateTime startTime = DateTime.Now;

            PcapDotNet.Packets.Packet packet;
            byte[] receive_byte_array;
            byte[] more_byte_array;
            int data_len = event_data_length * event_num_ch;
            byte[] data_array = new byte[0];
            UInt16 j = 0;
            string s;
            int bytes_recieved = 0;

            //have I had a trigger? 
            if (MasterPADE == null)
            { MasterPADE = TB4.PADE_List[1]; }
            TB4.ActivePADE = MasterPADE;
            ushort stat = 0;// = runStatusReg.RegRead();
            if (TB4.ActivePADE.flgUSB_comms)
            { //usb mode
                TB4.myFTDI = TB4.ActivePADE.PADE_FTDI;
                TB4.using_USB_comms = true;
                TB4.using_Ether_comms = false;
            }
            else
            {  //ethernet mode
                TB4.MAC_add = TB4.ActivePADE.MAC_add;
                TB4.IP4_add = TB4.ActivePADE.IP4_add;
                TB4.using_USB_comms = false;
                TB4.using_Ether_comms = true;
            }
            stat = runStatusReg.RegRead();

            if (((stat & 2) == 2) && ((stat & 0x10) == 0))
            {//yep, trigger latched and ZS done
                //loop over all PADEs, ask to spit data
                Console.WriteLine("TRIG");
                s = "";
                this.event_data_by_ch = new int[10, 32, 16];
                this.raw_udp_string = new string[10];

                for (int i = 0; i < TB4.PADE_List.Count; i++)
                {
                    PADE thisPADE;
                    if (TB4.PADE_List.TryGetValue(i + 1, out thisPADE))
                    {
                        //TB4.activatePADE(thisPADE, false, false);
                        
                        TB4.ActivePADE = thisPADE;
                        if (TB4.ActivePADE.flgUSB_comms)
                        {
                            TB4.myFTDI = TB4.ActivePADE.PADE_FTDI;
                            TB4.using_USB_comms = true;
                            TB4.using_Ether_comms = false;
                        }
                        else
                        {
                            TB4.MAC_add = TB4.ActivePADE.MAC_add;
                            TB4.IP4_add = TB4.ActivePADE.IP4_add;
                            TB4.using_USB_comms = false;
                            TB4.using_Ether_comms = true;
                        }
                         
                         
                        runControlReg.RegWrite(0x02c0);
                    }

                    bool all_done = false;
                    bool took_event = false;

                    Console.WriteLine("TIME TO DO TRIVIAL THINGS: " + (DateTime.Now - startTime).TotalMilliseconds);

                    while (!all_done)
                    {
                        PacketCommunicatorReceiveResult result = TB4.myPacketComm.ReceivePacket(out packet);
                        Console.WriteLine("TIME TO DO myPacketComm.ReceivePacket(): " + (DateTime.Now - startTime).TotalMilliseconds);
                        switch (result)
                        {
                            case PacketCommunicatorReceiveResult.Timeout:
                                // Timeout elapsed
                                all_done = true;
                                Console.WriteLine("TIMETOUT");
                                if (took_event)
                                {
                                    if (thisPADE.PADE_index == 1)
                                    { this.Run_event++; }

                                    this.streamRun.WriteLine(s); s = "";
                                    for (int k = 1; k < this.event_UDP_data.Length; k++)
                                    {
                                        s = this.Run_event.ToString() + " ";
                                        s += thisPADE.PADE_index.ToString() + " ";
                                        s += thisPADE.IP4_add[0].ToString() + " ";
                                        //s += this.event_UDP_data[k].SourcePort.ToString() + " ";
                                        s += "  " + this.event_UDP_data[k].Payload.ToHexadecimalString();
                                        
                                        if (thisPADE.PADE_index == TB4.ActivePADE.PADE_index)
                                        {
                                            if (this.flgResquestData && !this.flgNewDataAvailable)
                                            {

                                                this.UDPdata_for_parser = this.event_UDP_data[k].Payload.ToHexadecimalString();
                                                this.flgNewDataAvailable = true;
                                            }
                                        }
                                        this.streamRun.WriteLine(s); s = "";
                                    }

                                }
                                s = "";
                                break;


                            //TB4.active_PAD_index
                            case PacketCommunicatorReceiveResult.Ok:
                                Console.WriteLine("PACKET RECEIVED");
                                IpV4Datagram ip = packet.Ethernet.IpV4;
                                ushort DEST_PORT = packet.Ethernet.IpV4.Udp.DestinationPort;
                                if (DEST_PORT == 0x5353)
                                {
                                    if (this.event_UDP_data == null)
                                    { this.event_UDP_data = new IpV4Datagram[1]; }
                                    int l = this.event_UDP_data.Length;
                                    Array.Resize<IpV4Datagram>(ref this.event_UDP_data, l + 1);
                                    this.event_UDP_data[l] = ip;

                                    if (this.streamRun != null)
                                    {
                                        took_event = true;
                                    }
                                }
                                break;
                            default:
                                throw new InvalidOperationException("The result " + result + " should never be reached here");

                        }
                    }

                    TB4.myRun.event_UDP_data = new IpV4Datagram[1];
                }

            }
            else
            {//no, no trigger found. Just return

            }
            udpCount++;
            udpAverage = (udpAverage * (udpCount - 1) + (DateTime.Now - startTime).Milliseconds) / udpCount;
            Console.WriteLine("UDP EVENT TOOK " + (DateTime.Now - startTime).Milliseconds + " MILLISECONDS.  AVERAGE= " + udpAverage.ToString());
       
             * 
             */
            dataPush(Environment.GetFolderPath(Environment.SpecialFolder.Desktop));
        }

        void dataPush(string dataDumpPathname = null)
        {
            //setup the dump file
            bool writeToDisc = (dataDumpPathname != null);
            System.IO.FileStream newStream = new System.IO.FileStream(dataDumpPathname, System.IO.FileMode.CreateNew);
            System.IO.StreamWriter writer = new System.IO.StreamWriter(newStream);
            writer.WriteLine("Data taken at " + DateTime.Now.ToString() + ":");

            //read 1200 bytes from memory location 0x04000000
            int[] rdata = new int[1200];
            TB4.ReadArray(4, 0, 0, 0, 1200, rdata);

            //turn the data into something that can be displayed
            int timestamp = 0;
            string[] preappend = new string[4];
            string DH = "";
            int[] value = new int[5];
            int counter = 0;
            TB4_Register adcCheckReg = new TB4_Register("NUM_ADC_SAMPLES", "", 0x04800000, 8, true, false);
            TB4.thePlot.zg1.GraphPane.CurveList.Clear();
            GraphPane mainPane = TB4.thePlot.zg1.GraphPane;


            //software reset
            PADE_explorer.registerLookup("SOFTWARE_RESET").RegWrite(1);

            //send software trigger

            TB4.myRun.runSoftTrig.RegWrite(1);


            DateTime startTime = DateTime.Now;
            while (600 > adcCheckReg.RegRead())
            {
                if ((DateTime.Now - startTime).TotalMilliseconds > 1000) { TB4_Exception.logError(null, "scanDataPush timeout.", true); return; }
                Thread.Sleep(1);
            }

            {
                TB4.ReadArray(4, 0, 0, 0, 1200, rdata);
                for (int i = 0; i < 50; i++)  //iterate through each block
                {
                    timestamp = rdata[i * 24] << 16 + rdata[i * 24 + 1] << 8 + rdata[i * 24 + 2]; //get timestamp

                    if (writeToDisc) writer.Write(timestamp + ": ");

                    for (int j = 0; j < 5; j++) //iterate through data points
                    {
                        //construct parts of each word
                        for (int k = 0; k < 4; k++) preappend[k] = Convert.ToString((rdata[i * 24 + j * 4 + k + 4] >> 7) * 4 * (rdata[i * 24 + j * 4 + k + 4] & 128) + (rdata[i * 24 + j * 4 + k + 4] >> 7) * 128 + (rdata[i * 24 + j * 4 + k + 4] & 128) * (1 - (rdata[i * 24 + j * 4 + k + 4] >> 7)));
                        //construct the word from the parts
                        for (int k = 0; k < 4; k++) DH += preappend[k];
                        value[j] = Convert.ToInt16(DH);
                        DH = "";

                        //now plot the data
                        ((IPointListEdit)mainPane.CurveList[i].Points).Add(timestamp + 0.001 * j, value[j]);

                        if (writeToDisc) writer.Write(value[j] + "  ");
                        counter++;
                    }
                    if (writeToDisc) writer.Write('\n');

                    //redraw the plot: if speed is more important than refresh rate, put this outside the block for loop
                    TB4.thePlot.zg1.Invalidate();
                }


            }

            if (writeToDisc)
            {
                //close up shop
                writer.WriteLine("End of data.");
                writer.Close();
                newStream.Close();
            }


        }

        public void ARM_PADE()
        {

            TB4.ActivePADE = this.MasterPADE;
            if (TB4.ActivePADE.flgUSB_comms)
            {
                TB4.myFTDI = TB4.ActivePADE.PADE_FTDI;
                TB4.using_USB_comms = true;
                TB4.using_Ether_comms = false;
            }
            else
            {
                TB4.MAC_add = TB4.ActivePADE.MAC_add;
                TB4.IP4_add = TB4.ActivePADE.IP4_add;
                TB4.using_USB_comms = false;
                TB4.using_Ether_comms = true;
            }
            runControlReg.RegWrite(0);
            // loop over all slave PADs and set Control Reg to 0
            for (int i = 0; i < TB4.PADE_List.Count; i++)
            {
                PADE thisPADE;
                if (TB4.PADE_List.TryGetValue(i + 1, out thisPADE))
                {
                    if (!thisPADE.PADE_is_MASTER)
                    {
                        TB4.ActivePADE = thisPADE;
                        if (TB4.ActivePADE.flgUSB_comms)
                        {
                            TB4.myFTDI = TB4.ActivePADE.PADE_FTDI;
                            TB4.using_USB_comms = true;
                            TB4.using_Ether_comms = false;
                        }
                        else
                        {
                            TB4.MAC_add = TB4.ActivePADE.MAC_add;
                            TB4.IP4_add = TB4.ActivePADE.IP4_add;
                            TB4.using_USB_comms = false;
                            TB4.using_Ether_comms = true;
                        }
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
                    if (!thisPADE.PADE_is_MASTER) { }//master is armed last
                    {
                        TB4.ActivePADE = thisPADE;
                        if (TB4.ActivePADE.flgUSB_comms)
                        {
                            TB4.myFTDI = TB4.ActivePADE.PADE_FTDI;
                            TB4.using_USB_comms = true;
                            TB4.using_Ether_comms = false;
                        }
                        else
                        {
                            TB4.MAC_add = TB4.ActivePADE.MAC_add;
                            TB4.IP4_add = TB4.ActivePADE.IP4_add;
                            TB4.using_USB_comms = false;
                            TB4.using_Ether_comms = true;
                        }
                        runControlReg.RegWrite(0x01c0);
                    }
                }

            }
            // now enable Master
            TB4.ActivePADE = MasterPADE;
            if (TB4.ActivePADE.flgUSB_comms)
            {
                TB4.myFTDI = TB4.ActivePADE.PADE_FTDI;
                TB4.using_USB_comms = true;
                TB4.using_Ether_comms = false;
            }
            else
            {
                TB4.MAC_add = TB4.ActivePADE.MAC_add;
                TB4.IP4_add = TB4.ActivePADE.IP4_add;
                TB4.using_USB_comms = false;
                TB4.using_Ether_comms = true;
            }
            runControlReg.RegWrite(0x01c0);
            //thats it. We are ARMED
            //if this is a software trig, do it now
            if (flgSoftwareTrig)
            {
                runSoftTrig.RegWrite(1);
            }

        }

        #endregion UDP

        TB4_PerfMon arrayRead = new TB4_PerfMon("Event Read Array", "The amount of time it takes to collect all the event data.");
        TB4_PerfMon postProcess = new TB4_PerfMon("Event Data Processing", "The amount of time it takes to process the TakeEvent data.");
        public void TakeEvent() //and store it
        {
            //code hacked for data taking on Sept14
            UInt16 ui_data = 0;
            long[] ms = new long[6];
            //cosmic trigger used to be here...

            UInt16 ArmControl;
            UInt16 DisarmControl;
            UInt16 CSR = 0;

            foreach (KeyValuePair<int, PADE> kvp in TB4.PADE_List)
            {

                if (kvp.Value.flgUSB_comms)
                {
                    TB4.myFTDI = kvp.Value.PADE_FTDI;
                    TB4.using_USB_comms = true;
                    TB4.using_Ether_comms = false;
                }
                else
                {
                    TB4.MAC_add = kvp.Value.MAC_add;
                    TB4.IP4_add = kvp.Value.IP4_add;
                    TB4.using_USB_comms = false;
                    TB4.using_Ether_comms = true;
                }
                if (kvp.Value.PADE_is_MASTER) { ArmControl = 0x1c0; }
                else { ArmControl = 0x0c0; }
                PADE_explorer.registerLookup("CONTROL_REG").RegWrite(ArmControl);
                PADE_explorer.registerLookup("SOFTWARE_RESET").RegWrite(1);

                //uint testCSR = TB4.regSTATUS.RegRead();
                //uint testControl = TB4.regCONTROL.RegRead();
            }


            if (this.flgSoftwareTrig)
            {
                foreach (KeyValuePair<int, PADE> kvp in TB4.PADE_List)
                {
                    if (kvp.Value.flgUSB_comms)
                    {
                        TB4.myFTDI = kvp.Value.PADE_FTDI;
                        TB4.using_USB_comms = true;
                        TB4.using_Ether_comms = false;
                    }
                    else
                    {
                        TB4.MAC_add = kvp.Value.MAC_add;
                        TB4.IP4_add = kvp.Value.IP4_add;
                        TB4.using_USB_comms = false;
                        TB4.using_Ether_comms = true;
                    }

                    //Thread.Sleep(1);
                    if (kvp.Value.PADE_is_MASTER)
                    {//issue software trig
                        int REG_soft_trig_ind = 0;
                        PADE_explorer.registerLookup("SOFTWARE_TRIGGER").RegWrite(1);
                        Thread.Sleep(1);
                    }
                    //uint testCSR = TB4.regSTATUS.RegRead();
                    //uint testControl = TB4.regCONTROL.RegRead();
                }
            }

            foreach (KeyValuePair<int, PADE> kvp in TB4.PADE_List)
            {
                if (kvp.Value.flgUSB_comms)
                {
                    TB4.myFTDI = kvp.Value.PADE_FTDI;
                    TB4.using_USB_comms = true;
                    TB4.using_Ether_comms = false;
                }
                else
                {
                    TB4.MAC_add = kvp.Value.MAC_add;
                    TB4.IP4_add = kvp.Value.IP4_add;
                    TB4.using_USB_comms = false;
                    TB4.using_Ether_comms = true;
                }

                //Thread.Sleep(1);
                //if (flgExtTrig)
                //{//this is an external trig, look for the one
                //    int k = 0; CSR = 0;
                //    int ext_trig_latched = 2;
                //    while (((CSR & ext_trig_latched) != ext_trig_latched) && (k < this.trig_timeout))
                //    {
                //        if (k > 0) { Thread.Sleep(1); }
                //        CSR = TB4.regSTATUS.RegRead();
                //        k++;
                //    }
                //    if ((k + 1) > this.trig_timeout) { return; } //return from this routine without writing an event to disk
                //}

                byte A3 = 0x40;
                byte A2 = 0;
                byte A1 = 0;
                byte A0 = 0;
                UInt16 data_len = 1024;

                if (this.min_bin < 0) { this.min_bin = 0; }
                if (this.max_bin > 4095) { this.max_bin = 4095; }
                if (this.max_bin <= this.min_bin) { this.max_bin = 4095; this.min_bin = 0; }
                if ((this.max_bin - this.min_bin) < 4095) { data_len = (ushort)(this.max_bin - this.min_bin); }
                int[] data = new int[1024];
                int[] data0 = new int[1024];
                int[] data1 = new int[1024];
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
                    arrayRead.startTime();
                    uint hit_count_reg_addr = 0x04800000;
                    ushort hits = PADE_explorer.regAddLookup(hit_count_reg_addr).RegRead();

                    for (int i = 0; i < 2; i++)
                    {
                        StartA = Convert.ToUInt32(0x04000000 + 0x00100000 * i);
                        A3 = Convert.ToByte((StartA & 0xff000000) >> 24);
                        A2 = Convert.ToByte((StartA & 0x00ff0000) >> 16);
                        A1 = Convert.ToByte((StartA & 0x0000ff00) >> 8);
                        A0 = Convert.ToByte((StartA & 0x000000ff));
                        data_len = 256;
                        if (i == 0)
                        { TB4.ReadArray(A3, A2, A1, A0, data_len, data0); }
                        if (i == 1)
                        { TB4.ReadArray(A3, A2, A1, A0, data_len, data1); }

                        if (this.streamRun != null)
                        {
                            s = kvp.Value.PADE_sn.ToString();
                            s += " : " + TB4.myRun.Run_event.ToString();
                            if (i == 0) { s += " ping " + Convert.ToString(hits, 16); }
                            else { s += " pong " + Convert.ToString(hits, 16); }

                            if (i == 0)
                            {
                                for (int ii = 0; ii < 256; ii++)
                                {
                                    string t = Convert.ToString((data0[ii] & 0xff), 16);
                                    if (t.Length < 2) { t = "0" + t; }
                                    s = s + " " + t;
                                    t = Convert.ToString((data0[ii] & 0xff00) >> 8, 16);
                                    if (t.Length < 2) { t = "0" + t; }
                                }
                                TB4.myRun.streamRun.WriteLine(s);
                            }
                            else
                            {
                                for (int ii = 0; ii < 256; ii++)
                                {
                                    string t = Convert.ToString((data1[ii] & 0xff), 16);
                                    if (t.Length < 2) { t = "0" + t; }
                                    s = s + " " + t;
                                    t = Convert.ToString((data1[ii] & 0xff00) >> 8, 16);
                                    if (t.Length < 2) { t = "0" + t; }
                                    //s = s + " " + Convert.ToString(data1[ii], 16);
                                }
                                TB4.myRun.streamRun.WriteLine(s);
                            }

                        }
                    }
                    arrayRead.stopTime(true);
                    postProcess.startTime();
                    if (kvp.Value.PADE_is_MASTER) { DisarmControl = 0x140; }
                    else { DisarmControl = 0x040; }
                    TB4.regCONTROL.RegWrite(DisarmControl);
                    //uint testCSR = TB4.regSTATUS.RegRead();
                    //uint testControl = TB4.regCONTROL.RegRead();
                    Thread.Sleep(0);

                }
                postProcess.stopTime(true);
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
        public string verbose_description = "";
        public UInt32 addr;
        public byte width; //this is supposed to tell you how "wide" this register is
        public bool readOnly;
        public bool writeOnly;
        public UInt32 myStatus;
        public bool hasSubfields;

        #region Persistent Register Writes
        public static string configFile = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\configFile.dat";

        public static void createRegisterList(string pathname)
        {
            StringBuilder sb = new StringBuilder();

            try
            {


                PADE priorActive = null;
                if (TB4.ActivePADE != null)
                {
                    priorActive = TB4.ActivePADE; //used for preserving the current active PADE
                }


                for (int i = 1; i <= TB4.PADE_List.Count; i++)
                {
                    TB4.activatePADE(TB4.PADE_List[i], false, false);
                    Console.WriteLine("REGWRITE: " + TB4.PADE_List[i].PADE_sn);
                    sb.AppendLine("Configuration File for PADE" + TB4.ActivePADE.PADE_sn);

                    foreach (TB4_Register reg in TB4.my_reg_collection)
                    {
                        if (!reg.readOnly && !reg.writeOnly)
                        {
                            sb.Append("#" + reg.name + "$");
                            if (reg.writeOnly) sb.AppendLine("WRITEONLY#");
                            else sb.AppendLine(reg.RegRead().ToString() + "#");
                        }
                    }

                    sb.AppendLine("#End of PADE" + TB4.ActivePADE.PADE_sn + " configuration file.#");

                }

                using (StreamWriter outfile = new StreamWriter(pathname))
                {
                    outfile.Write(sb.ToString());
                }

                if (priorActive != null) TB4.activatePADE(priorActive, true, false); //restore formerly active PADE

            }
            catch (Exception ex)
            {
                TB4_Exception.logError(ex, "Failed to create register list.", true);


            }


        }

        public static void applyRegisterList(string PADE_SN)
        {
            PADE oldPADE = TB4.ActivePADE;
            if (System.IO.File.Exists(configFile))
            {
                StreamReader sr = new StreamReader(configFile);
                string contents = sr.ReadToEnd();
                string[] DH = contents.Split(new string[] { "Configuration File for PADE" }, StringSplitOptions.RemoveEmptyEntries);

                for (int i = 0; i < DH.Length; i++)
                {
                    //this block reads the configuration for one PADE
                    string[] subContents = DH[i].Trim().Split(new char[] { '#' }, StringSplitOptions.RemoveEmptyEntries);  //split the config block up by register

                    if (subContents[0].Trim() == PADE_SN) //if the following content is for the appropriate PADE (the one passed as an argument)
                    {
                        TB4.activatePADE(PADE_explorer.getPADE(subContents[0].Trim()), false, false);
                        if (subContents[0].Trim() == TB4.ActivePADE.PADE_sn)
                        {
                            Console.WriteLine("PADE#" + subContents[0].Trim());
                            TB4.thePADE_explorer.updateStatusText(true, "Updating PADE " + subContents[0].Trim());

                            for (int j = 1; j < subContents.Length - 1; j += 2)  //iterate through each register
                            {
                                //this block represents one line: #regname$Value#

                                string[] lineContents = subContents[j].Split(new char[] { '$' }, StringSplitOptions.RemoveEmptyEntries);
                                TB4_Register currentRegister = PADE_explorer.registerLookup(lineContents[0]);
                                currentRegister.RegWrite(Convert.ToUInt16(lineContents[1].Trim()));
                            }

                        }
                    }

                }
                sr.Close();

            }
            TB4.ActivePADE = oldPADE;
        }

        /// <summary>
        /// Call this simultaneously when writing to a write-only register.  Assumes the target PADE is equal to TB4.ActivePADE.
        /// </summary>
        public static void writeRegisterChange(TB4_Register newReg, UInt16 value)
        {

            StreamReader sr = new StreamReader(configFile);
            StringBuilder sb = new StringBuilder();

            string contents = sr.ReadToEnd();
            string[] DH = contents.Split(new string[] { "Configuration File for PADE" }, StringSplitOptions.None);

            int i;
            for (i = 0; i < DH.Length && !(TB4.ActivePADE.PADE_sn == DH[i].Substring(0, 1)); i++) //find the correct PADE block, and also copy text to the string builder
            {
                sb.AppendLine("Configuration File For PADE" + DH[i]);
            }

            string[] registers = DH[i].Split(new char[] { '#' });
            string[] regProperties;

            for (int j = 0; j < registers.Length; j++)
            {

                regProperties = registers[j].Split(new char[] { '$' });

                sb.Append("#");
                if (regProperties[0] == newReg.name) //we have found the correct register! assume its not a read-only
                {
                    regProperties[6] = value.ToString();

                    for (int k = 0; k < 7; k++)
                    {
                        if (k > 0) sb.Append("$" + regProperties);
                        else sb.Append(regProperties);
                    }
                }
                else
                {
                    sb.Append(registers[j]);
                }
                sb.Append("#\n");

            }

            for (int k = i; k < DH.Length; k++) //continue where we left off, copying the rest of the file
            {
                sb.AppendLine("Configuration File For PADE" + DH[i]);
            }

            File.WriteAllText(configFile, sb.ToString());

        }

        #endregion

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
            //Console.WriteLine("TEMP ADDRESS: " + temp_addr.ToString());
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
                //data[1] = 0x07;
                data[1] = 0x1; //this is the read command
                data[2] = (byte)((temp_addr & 0xff000000) >> 24); //MSB
                data[3] = (byte)((temp_addr & 0x00ff0000) >> 16);
                data[4] = (byte)((temp_addr & 0x0000ff00) >> 8);
                data[5] = (byte)((temp_addr & 0x000000ff));
                data[6] = 0;
                i = 6;
                if (TB4.using_Ether_comms)
                {
                    //Console.WriteLine("USING ETHER COMMS");
                    Eth_comms.Ether_Clear();
                    Eth_comms.Ether_Write(data, i);
                    ushort[] p_data;
                    Eth_comms.Ether_Read(out p_data, 1);
                    if (p_data.Length > 0)
                    { ui_data = p_data[p_data.Length - 1]; }
                }
                //if (TB4.using_USB_comms)
                else
                {
                    string message = "";
                    //Console.WriteLine("Entered usb");
                    USB_AID.FT_GetStatus(ref rxsize, ref txsize);
                    message += "RX Size: " + rxsize.ToString() + "  TX size: " + txsize;
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
                    if (ThisSleep >= MaxSleep) { Console.WriteLine("FAILED."); return 0; }//we failed, basically
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
                    message += '\n' + "TX2: " + txsize.ToString() + "  RX2: " + rxsize.ToString();

                    USB_AID.FT_GetStatus(ref rxsize, ref txsize);

                    message += '\n' + "TX2: " + txsize.ToString() + "  RX2: " + rxsize.ToString();
                    //Thread.Sleep(1);
                    if ((rxsize < 65) && (rxsize > 0)) { length = USB_AID.FT_Read(data, rxsize); }//clean up
                    //USB_AID.FT_GetStatus(ref rxsize, ref txsize);
                    Console.WriteLine(message);
                }

                TB4.myRun.flgRun_pause = false;
                //if (ui_data == 401) { SpeedTest(); }
                if (this.width == 32)
                {
                    temp_addr = old_addr;
                }
                return ui_data;
            }
        }

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
            //data[1] = 0x07;
            data[1] = 0x11; //this is the write command
            data[2] = (byte)((temp_addr & 0xff000000) >> 24); //MSB
            data[3] = (byte)((temp_addr & 0x00ff0000) >> 16);
            data[4] = (byte)((temp_addr & 0x0000ff00) >> 8);
            data[5] = (byte)((temp_addr & 0x000000ff));
            data[6] = (byte)((ui_data & 0xff00) >> 8);
            data[7] = (byte)((ui_data & 0x00ff));
            data[8] = 0;
            data[9] = 0;
            i = 8;

            if (TB4.using_Ether_comms)
            { Eth_comms.Ether_Write(data, i); }
            //if (TB4.using_USB_comms)
            else
            { USB_AID.FT_Write(data, i); }

            TB4.myRun.flgRun_pause = false;
            if (this.width == 32)
            {
                temp_addr = old_addr;
            }
        }

        public void RegWriteArray(UInt16[] ui_data)
        {
            uint array_size = (uint)ui_data.Length;
            if (this.readOnly) { return; }
            TB4.myRun.flgRun_pause = true;
            byte[] data = new Byte[1400];
            byte[] d_data = new Byte[1400];
            UInt32 i;
            uint temp_addr = this.addr;
            if (array_size > 256) { array_size = 256; }

            uint EndAddr = temp_addr + array_size;
            uint old_addr = temp_addr;

            if (this.width == 32)
            {
                uint func_code = (temp_addr & 0xffff0000);
                temp_addr = (temp_addr & 0x0000ffff) >> 1;
                temp_addr += func_code;
            }
            data[0] = 0xA5;

            //data[1] = 0x07;
            data[1] = 0x12; //this is the array write command
            data[2] = (byte)((temp_addr & 0xff000000) >> 24); //MSB
            data[3] = (byte)((temp_addr & 0x00ff0000) >> 16);
            data[4] = (byte)((temp_addr & 0x0000ff00) >> 8);
            data[5] = (byte)((temp_addr & 0x000000ff));
            data[6] = (byte)((EndAddr & 0xff000000) >> 24);
            data[7] = (byte)((EndAddr & 0x00ff0000) >> 16);
            data[8] = (byte)((EndAddr & 0x0000ff00) >> 8);
            if (((EndAddr) & 0x000000ff) == 0xff) { data[8]++; data[9] = 0; }
            data[9] = (byte)(((EndAddr) & 0x000000ff));
            data[10] = 0;
            //
            //**********************************************************************************************************************************************************************
            // this is a bunch of hacks to deal with a bug in the array write of the 0x311 fw that causes an address wrap around such that the last two words are actually the first
            //this code will produce a nice 0 to 255 array
            //byte kk = 2;
            //for (int k = 0; k < array_size-1; k++)
            //{
            //    data[11 + 4 * k+2] = (byte)(kk);
            //    data[11 + 4 * k + 1] = 0;// (byte)((ui_data[k] & 0xff00) >> 8); 
            //    kk++;

            //    data[11 + 4 * k] =kk;
            //    data[11 + 4 * k + 3] =0;
            //    kk++;
            //}
            //data[11 + 1020 + 2] = 0x0;//zeroth byte
            //data[11 + 1020 + 1] = 0;// (byte)((ui_data[k] & 0xff00) >> 8); 
            //data[11 + 1020] = 0x1; //1st byte
            //data[11 + 1020 + 3] = 0;
            //
            //**********************************************************************************************************************************************************************
            //**********************************************************************************************************************************************************************
            for (int k = 0; k < array_size; k = k + 2)
            {
                data[10 + 2 * k + 0] = Convert.ToByte((ui_data[k + 1] >> 8) & 0x00ff);
                data[10 + 2 * k + 1] = (byte)(ui_data[k + 1] & 0x00ff);
                data[10 + 2 * k + 2] = Convert.ToByte((ui_data[k] >> 8) & 0x00ff);
                data[10 + 2 * k + 3] = (byte)(ui_data[k] & 0x00ff);
            }

            i = 10 + 2 * array_size;

            if (TB4.using_Ether_comms)
            { Eth_comms.Ether_Write(data, i); }
            //if (TB4.using_USB_comms)
            else
            { USB_AID.FT_Write(data, i); }

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

        //public void Open()
        //{
        //    privStatus = USB_AID.FT_Open();

        //    if (privStatus != 0) { privStatus = 1; }
        //}

        //public void Close()
        //{
        //    USB_AID.FT_Close();
        //}

        //public void Flush_rx()
        //{
        //    ulong rxsize = 0, txsize = 0;
        //    UInt32 length = 0;
        //    byte[] data = new Byte[UInt16.MaxValue];
        //    length = USB_AID.FT_Read(data, rxsize);
        //    USB_AID.FT_GetStatus(ref rxsize, ref txsize);
        //    if (rxsize != 0)
        //    {
        //        Close();
        //        Open();
        //    }
        //}

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
            //MessageBox.Show("This ver ethernet only!"); return 0;
            FTDI.FT_STATUS ftStatus = FTDI.FT_STATUS.FT_DEVICE_NOT_OPENED;
            TB4.myFTDI.SetLatency(latency);
            if (ftStatus == FTDI.FT_STATUS.FT_OK)
            { return 0; }
            else { return 1; }
        }

        static public UInt32 FT_ChangeTimeout(uint new_timeout)
        {
            //MessageBox.Show("This ver ethernet only!"); return 0;
            FTDI.FT_STATUS ftStatus = FTDI.FT_STATUS.FT_DEVICE_NOT_OPENED;
            TB4.myFTDI.SetTimeouts(new_timeout, new_timeout);
            if (ftStatus == FTDI.FT_STATUS.FT_OK)
            { return 0; }
            else { return 1; }
        }

        static public UInt32 FT_ChangeBuffer(uint in_buffer, uint out_buffer)
        {
            //MessageBox.Show("This ver ethernet only!"); return 0;
            FTDI.FT_STATUS ftStatus = FTDI.FT_STATUS.FT_DEVICE_NOT_OPENED;
            TB4.myFTDI.InTransferSize(in_buffer);

            if (ftStatus == FTDI.FT_STATUS.FT_OK)
            { return 0; }
            else { return 1; }
        }

        static public UInt32 FT_Open()
        {
            //MessageBox.Show("This ver ethernet only!"); return 0;
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
            //MessageBox.Show("This ver ethernet only!"); return 0;
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
                        //TB4.thePAD_selector.Visible = true;
                    }
                    else { thisPADE.PADE_is_MASTER = false; thisPADE.PADE_is_SLAVE = true; }

                    ftStatus = thisPADE.PADE_FTDI.OpenBySerialNumber(ftdiDeviceList[i].SerialNumber);
                    if (ftStatus == FTDI.FT_STATUS.FT_OK)
                    {
                        //TB4.theDisplayControl.Text = listBox1.SelectedItem.ToString();

                        thisPADE.flgEther_comms = false;
                        thisPADE.flgUSB_comms = true;
                        TB4.using_Ether_comms = false;
                        TB4.using_USB_comms = true;
                        TB4.activatePADE(thisPADE, false, true); //FIX
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
            //UInt32 status = 999;
            uint ret_size = 0;
            int isize = Convert.ToInt32(size);

            FTD2XX_NET.FTDI.FT_STATUS stat = FTDI.FT_STATUS.FT_DEVICE_NOT_FOUND;
            if (TB4.myFTDI.IsOpen)
            {
                try
                {
                    stat = TB4.myFTDI.Write(p_data, isize, ref ret_size);
                    if (stat == FTDI.FT_STATUS.FT_DEVICE_NOT_FOUND)
                    {
                        Exception e = new Exception("FTDI status FT_DEVICE_NOT_FOUND");
                        throw e;
                    }
                }
                catch (Exception ex)
                {
                    TB4_Exception.logError(ex, "FTDI could not be found.", true);
                }
            }
            else
            { Console.WriteLine("TB4.myFTDI.IsOpen is FALSE"); }


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

    class SlowControlPacket
    {
        public UInt64 count;
        public string src_ip;
        public string dst_ip;
        public string src_port;
        public string dst_port;
        public string header;
        public string str_val;
        public UInt16 val;
        public UInt16[] array_data;

        public DateTime time_stamp;

        public Int32 Eval(string t)
        {
            if (t == "")
            {
                t = str_val;
            }
            try
            {
                if (t.Length > 4) { t = t.Substring(1, 4); }
                while (t.Length < 4)
                { t = "0" + t; }
                t = "0x" + t;
                int v = Convert.ToInt32(t, 16);
                if ((v >= 0) && (v < 65536)) { val = Convert.ToUInt16(v); }
                return v;
            }
            catch { return -1; }
        }

        public SlowControlPacket()
        {
            src_ip = "x.x.x.x";
            dst_ip = "x.x.x.x";
            src_port = "";
            dst_port = "";
            header = "";
            str_val = "";
            val = 0;
            time_stamp = DateTime.MinValue;
            count = 0;
        }
    }

    static class Eth_comms
    {
        // Open the output device
        //public static LivePacketDevice SlowControl_interface;
        //public static PacketCommunicator communicator;
        static public UInt32 Eth_Open()
        {

            TB4.using_Ether_comms = true;
            TB4.using_USB_comms = false;
            return 0;
        }
        static private List<SlowControlPacket> rec_buffer = new List<SlowControlPacket>(5);
        //static private enum rec_buff_status
        //{

        //}

        static public UInt32 Ether_Write(byte[] p_data, uint size)
        {

            PacketCommunicator communicator = null;
            try
            {
                communicator = TB4.SlowControl_interface.Open(2500, // name of the device
                                        PacketDeviceOpenAttributes.Promiscuous, // promiscuous mode
                                        2); // read timeout
                communicator.SetKernelMinimumBytesToCopy(47);
            }
            catch (Exception ex)
            {

                TB4_Exception.logError(ex, "Failed to open ethernet interface. Please verify that all PADEs are connected.", true);
                return 0;
            }
            //1)send packet
            //2)start a timer
            //3)start a non-blocking listner
            //4)check for timeout or reply
            uint ret_size = 0;
            //Take the selected adapter


            using (BerkeleyPacketFilter filter = communicator.CreateFilter("dst port 2001"))
            {
                // Set the filter to IPv4 and UDP
                communicator.SetFilter(filter);
            }

            using (communicator)
            {
                // Ethernet Layer
                MacAddress source = new MacAddress("00:80:55:00:01:00");

                string mac_dest_string = "";
                for (int i = 6; i > 0; i--)
                {
                    mac_dest_string += Convert.ToString(TB4.MAC_add[i - 1], 16);
                    if (i > 1) { mac_dest_string += ":"; }
                }
                MacAddress destination = new MacAddress(mac_dest_string);
                EthernetLayer ethernetLayer = new EthernetLayer
                                                  {
                                                      Source = source,
                                                      Destination = destination
                                                  };
                // IPv4 Layer
                IpV4Layer ipV4Layer = new IpV4Layer
                                          {
                                              Source = new IpV4Address("192.168.1.0"),
                                              Ttl = 4,
                                              // The rest of the important parameters will be set for each packet
                                          };

                string t = TB4.IP4_add[3].ToString() + ".";
                t += TB4.IP4_add[2].ToString() + ".";
                t += TB4.IP4_add[1].ToString() + ".";
                t += TB4.IP4_add[0].ToString() + ".";

                IpV4Address IPdest = new IpV4Address(t);
                ipV4Layer.CurrentDestination = IPdest;

                /// This function build an UDP over IPv4 over Ethernet with payload packet.
                UdpLayer udpLayer =
                    new UdpLayer
                        {
                            SourcePort = TB4.REG_port,
                            DestinationPort = TB4.REG_port,
                            Checksum = null, // Will be filled automatically.
                            CalculateChecksumValue = true,
                        };

                PayloadLayer payloadLayer =
                    new PayloadLayer
                        {
                            Data = new Datagram(p_data, 0, (int)size),
                        };

                PacketBuilder builder = new PacketBuilder(ethernetLayer, ipV4Layer, udpLayer, payloadLayer);
                // Build the packet
                Packet packet = builder.Build(DateTime.Now);

                //Send down the packet
                try
                {
                    communicator.SendPacket(packet);
                }
                catch (Exception ex)
                {
                    TB4_Exception.logError(ex, "Ethernet error.", true);
                    return 0;
                }
                Application.DoEvents();
                bool all_done = false;
                if (p_data[2] == 0x11) { all_done = true; }//if this is a write, skip the read!
                while (!all_done)
                {
                    PacketCommunicatorReceiveResult result = communicator.ReceivePacket(out packet);
                    switch (result)
                    {
                        case PacketCommunicatorReceiveResult.Timeout:
                            // Timeout elapsed
                            all_done = true;
                            break;

                        case PacketCommunicatorReceiveResult.Ok:
                            try
                            {
                                //packet rec
                                //all_done = true;
                                //IpV4Address rep_addr = new IpV4Address();
                                //string rep_addr = packet.IpV4.CurrentDestination.ToString();
                                //ushort rep_port = packet.IpV4.Udp.DestinationPort;
                                //MemoryStream rep_data= packet.IpV4.Udp.Payload.ToMemoryStream();
                                string rep_string = packet.IpV4.Udp.Payload.ToHexadecimalString();
                                string strSrcPort = rep_string.Substring(52, 4);
                                string strDstPort = rep_string.Substring(56, 4);

                                string tt = rep_string.Substring(80);
                                //byte[] rep_bytes = rep_data.GetBuffer();
                                if ((strSrcPort == "5151") && (strDstPort == "07d1"))
                                {
                                    SlowControlPacket this_packet = new SlowControlPacket();
                                    this_packet.time_stamp = DateTime.Now;
                                    this_packet.src_port = strSrcPort;
                                    this_packet.dst_port = strDstPort;
                                    this_packet.header = tt.Substring(0, 4);
                                    this_packet.str_val = tt.Substring(4, 4);
                                    if (this_packet.header == "a501")
                                    {
                                        rec_buffer.Add(this_packet);
                                        all_done = true;
                                    }
                                    else if (this_packet.header == "a504")
                                    {//this is an array read of the new sort
                                        tt = tt.Substring(8);
                                        int l = tt.Length;
                                        l = (UInt16)(Math.Round(l / 8.0));
                                        //if (l > 3) { l = l - 3; }
                                        this_packet.array_data = new UInt16[l * 2];
                                        ret_size = Convert.ToUInt16(l * 2);
                                        for (int i = 0; i < l * 2; i = i + 2)
                                        {
                                            this_packet.array_data[i] = Convert.ToUInt16(tt.Substring(4 * i + 4, 4), 16);
                                            this_packet.array_data[i + 1] = Convert.ToUInt16(tt.Substring(4 * i, 4), 16);
                                        }
                                        //rec_buffer.Clear();
                                        rec_buffer.Add(this_packet);
                                        all_done = true;
                                    }
                                    else if (this_packet.header == "a502")
                                    {//this is an array read of the old sort
                                        tt = tt.Substring(8);
                                        int l = tt.Length;
                                        l = (UInt16)(Math.Round(l / 8.0));
                                        if (l > 3) { l = l - 3; }
                                        this_packet.array_data = new UInt16[l];
                                        ret_size = Convert.ToUInt16(l);
                                        for (int i = 0; i < l; i++)
                                        {
                                            this_packet.array_data[i] = Convert.ToUInt16(tt.Substring(8 * i + 4, 4), 16);
                                        }
                                        //rec_buffer.Clear();
                                        rec_buffer.Add(this_packet);
                                        all_done = true;
                                    }
                                }
                            }

                            catch
                            {

                            };
                            break;
                    }
                }

            }

            //communicator.Dispose();
            return ret_size;
        }

        static public UInt32 Ether_Clear()
        {
            rec_buffer.Clear();
            return 0;
        }

        static public UInt32 Ether_GetStatus(ref ulong rxsize, ref ulong txsize)
        {

            rxsize = Convert.ToUInt64(rec_buffer.Count);
            txsize = 0;

            return 0;
        }

        static public UInt32 Ether_ReadArray(out UInt16[] p_data, ulong size)
        {
            //UInt32 status = 999;

            UInt32 ret_size = 0;
            int i = 0;
            p_data = null;
            foreach (SlowControlPacket p in rec_buffer)
            {
                if ((p.header.Substring(0, 3) == "a50") && (p.header.Substring(0, 4) != "a501"))
                {
                    p_data = p.array_data;
                    ret_size = Convert.ToUInt32(p_data.Length);
                    break;
                }

            }

            return ret_size;
        }

        static public UInt32 Ether_Read(out UInt16[] p_data, ulong size)
        {
            //UInt32 status = 999;
            p_data = new UInt16[rec_buffer.Count];
            int i = 0;
            try
            {
                foreach (SlowControlPacket p in rec_buffer)
                {
                    p.Eval("");
                    p_data[i] = p.val;
                    i++;
                }
            }
            catch (Exception ex)
            {
                TB4_Exception.logError(ex, "Ethernet read error.", true);
            }

            UInt32 ret_size = Convert.ToUInt32(rec_buffer.Count);
            return ret_size;
        }

    }

    public struct tempStamp
    {
        public int temperature;
        public DateTime time;

        public tempStamp(UInt16 temp, DateTime timeNow)
        {
            temperature = temp;
            time = timeNow;
        }
    }



    public class PADE
    {
        public FTD2XX_NET.FTDI PADE_FTDI = new FTD2XX_NET.FTDI();
        public byte[] IP4_add;
        public byte[] MAC_add;
        public ushort REG_port;
        public ushort DATA_port;
        public bool flgUSB_comms = false;
        public bool flgEther_comms = false;
        public string PADE_sn;
        public string PADE_descr;
        public int PADE_index;
        public bool PADE_is_SLAVE;
        public bool PADE_is_MASTER;
        public bool PADE_initialized;
        public ushort PADE_fw_ver;
        public bool[] PADE_ch_enable = new bool[32];
        public enum type_of_PADE { system_master, crate_master, slave, unknown };

        //"system" variables
        public string initializationFilePathname = "N/A";
        public string biasFilePathname = "N/A";
        public string SIB_ID = "N/A";
        public string registerFile = "N/A";
        //*****************

        #region Temperature Properties

        public Queue<tempStamp> Temperature = new Queue<tempStamp>(); //used for continuously monitoring temperature.
        private Queue<tempStamp> avgQueue = new Queue<tempStamp>(); //we need this because analysis is optional...

        public bool recordTemperature = false;
        private UInt16 p_sampleSize = 1000;
        public UInt16 sampleSize
        {
            get
            {
                return p_sampleSize;
            }
            set
            {
                while (sampleSize < Temperature.Count)
                {
                    Temperature.Dequeue();
                }
                p_sampleSize = sampleSize;
            }

        }

        private static int p_samplePeriod = 3000;
        public static int samplePeriod //sample period in milliseconds
        {
            get
            {
                return p_samplePeriod;
            }
            set
            {
                if (samplePeriod > 250)
                {
                    p_samplePeriod = value;

                    TB4.thePADE_explorer.tempThread.Interval = samplePeriod;
                    MessageBox.Show("SAMPLE PERIOD CHANGED TO " + TB4.thePADE_explorer.tempThread.Interval);
                }
            }
        }

        public static string tempUnits = "Fahrenheit";

        public bool analysisEnabled = false;
        public float averageTemp = 0;

        public tempStamp minTemp = new tempStamp(999, DateTime.Now);
        public tempStamp maxTemp = new tempStamp(0, DateTime.Now);

        public void addTemp(tempStamp arg)
        {
            Temperature.Enqueue(arg);

            if (analysisEnabled)
            {
                avgQueue.Enqueue(arg);

                if (arg.temperature < minTemp.temperature) minTemp = arg;

                if (arg.temperature > maxTemp.temperature) maxTemp = arg;

                if (avgQueue.Count < p_sampleSize)
                {
                    averageTemp = averageTemp * (avgQueue.Count - 1) / Temperature.Count + arg.temperature / avgQueue.Count;

                }
                else averageTemp += arg.temperature / avgQueue.Count - avgQueue.Peek().temperature / Temperature.Count;
                while (avgQueue.Count > p_sampleSize) avgQueue.Dequeue();
            }

            while (Temperature.Count > p_sampleSize)
            {
                Temperature.Dequeue();
            }


        }

        #endregion

        public PADE(int index)
        {

            PADE_initialized = false;
            PADE_index = -1;
            PADE_is_SLAVE = true;
            PADE_is_MASTER = false;

            PADE_FTDI = new FTDI();
            //PADE MAC
            MAC_add = new byte[6];
            MAC_add[5] = 00;
            MAC_add[4] = 0x80;
            MAC_add[3] = 0x55;
            MAC_add[2] = 0x00;
            MAC_add[1] = 0x00;
            MAC_add[0] = 0x00;
            IP4_add = new byte[4];
            IP4_add[3] = 192;
            IP4_add[2] = 168;
            IP4_add[1] = 0;
            IP4_add[0] = 0;
            //we need to call whatevere does the autoassign of the index (ICMP? ARP?)
            //!!!!!
            MAC_add[0] = (byte)(index & 0xff);
            IP4_add[0] = (byte)(index & 0xff);

            REG_port = 2001;// dec 

            DATA_port = 5300;

            for (int i = 0; i < 32; i++)
            { PADE_ch_enable[i] = false; }
        }

        public PADE()
        {
            PADE_initialized = false;
            PADE_index = -1;
            PADE_is_SLAVE = true;
            PADE_is_MASTER = false;
            PADE_FTDI = new FTDI();
            IP4_add = new byte[4];
            IP4_add[3] = 192;
            IP4_add[2] = 168;
            IP4_add[1] = 0;
            IP4_add[0] = (Convert.ToByte(this.PADE_sn));
            //PADE MAC
            MAC_add = new byte[6];
            MAC_add[5] = 00;
            MAC_add[4] = 0x80;
            MAC_add[3] = 0x55;
            MAC_add[2] = 0x00;
            MAC_add[1] = 0x00;
            MAC_add[0] = Convert.ToByte(this.PADE_sn);
            PADE_descr = "";
            REG_port = 2001; //2001 dec 

            DATA_port = 5300;
            for (int i = 0; i < 32; i++)
            { PADE_ch_enable[i] = false; }
        }

    }

    public static class TB4
    {

        [PerformanceCounterCategoryAttribute("ProtonCT",
       System.Diagnostics.PerformanceCounterCategoryType.SingleInstance,
       "Profile info about PAD-E application")]

        public enum SingleInstance_PerformanceCounters
        {
            [PerformanceCounterAttribute("AverageBase",
                "SlowControlComms",
                System.Diagnostics.PerformanceCounterType.AverageBase)]
            CounterSlowControlBase,

            [PerformanceCounterAttribute("Timer1",
                "Example MultiTimer Counter",
                System.Diagnostics.PerformanceCounterType.CounterMultiTimer)]
            MultiTimer1,

            [PerformanceCounterAttribute("Timer2",
                "Example MultiTimer Counter",
                System.Diagnostics.PerformanceCounterType.CounterMultiTimer)]
            MultiTimer2
        }

        public static List<WeifenLuo.WinFormsUI.Docking.DockContent> formList = new List<WeifenLuo.WinFormsUI.Docking.DockContent>();
        public static List<TB4_Register> my_reg_collection;
        public static TempPlot theTempPlot = new TempPlot();
        public static PADE_explorer thePADE_explorer;
        public static Flash0 theFlash;
        public static Plot0 thePlot; //= new Plot0();
        public static Hist0 theHist;
        public static Hist1 theHist1;
        public static Hist_and_Scan theHist_and_Scan;
        public static GBE theGBE;
        public static DRAM theDRAM;
        public static PAD_select thePAD_selector;
        public static BiasOffset theBiasOffset;
        public static PADE_Selector thePADE_Selector;
        public static DataDebug theDataDebug;
        public static PerformanceLog thePerfLog;
        public static SystemViewer theSystemViewer;
        public static RemoteControl theRemoteControl;
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
        public static bool using_USB_comms = false;
        public static bool using_Ether_comms = false;

        public static int[] tempBands = new int[] { 59, 68, 86, 113 };  //in Fahrenheit of course

        public static FTD2XX_NET.FTDI myFTDI = new FTD2XX_NET.FTDI();
        public static PacketCommunicator myPacketComm;
        public static LivePacketDevice SlowControl_interface;

        public static Registers theRegistersForm; //this form is initialized in Main() so that it can take advantage of the registers defined therein.

        public static byte[] IP4_add = new byte[4]; //of the PADE!
        public static byte[] MAC_add = new byte[6];
        public static ushort REG_port = 2001;
        public static ushort DATA_port;

        public static string systemFileName = "";
        //=================================
        public static SortedList<int, PADE> PADE_List = new SortedList<int, PADE>(4);

        public static Arrays theArraysForm = new Arrays();

        public static AboutBox1 myAboutBox = new AboutBox1();

        public static textDisplay myTextDisplay = new textDisplay();

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
        //public static TB4_Register regFLASH_DPRAM_BASE = new TB4_Register("FLASH_DPRAM_BASE_MB", "DPRAM_BASE_MB", 0x08800000, 16, false, false);
        public static TB4_Register regFLASH_DPRAM_BASE_MB = new TB4_Register("FLASH_DPRAM_BASE_MB", "DPRAM_BASE_MB", 0x00088000, 16, false, false);
        public static TB4_Register regFLASH_DPRAM_BASE_DB0 = new TB4_Register("FLASH_DPRAM_BASE_DB0", "DPRAM_BASE_DB0", 0x08800000, 16, false, false);
        public static TB4_Register regFLASH_DPRAM_BASE_DB1 = new TB4_Register("FLASH_DPRAM_BASE_DB1", "DPRAM_BASE_DB1", 0x18800000, 16, false, false);
        public static TB4_Register regFLASH_DPRAM_BASE_DB2 = new TB4_Register("FLASH_DPRAM_BASE_DB2", "DPRAM_BASE_DB2", 0x28800000, 16, false, false);
        public static TB4_Register regFLASH_DPRAM_BASE_DB3 = new TB4_Register("FLASH_DPRAM_BASE_DB3", "DPRAM_BASE_DB3", 0x38800000, 16, false, false);


        public static PADE ActivePADE;

        public static string ParseInit(string s)
        {
            string[] delimeter = new string[64];
            string[] tokens = new string[64];

            delimeter[0] = "<=";
            delimeter[1] = "//";
            delimeter[2] = ";";
            delimeter[3] = "=";
            delimeter[4] = "set ";
            delimeter[5] = "dec";
            delimeter[6] = "0x";
            delimeter[7] = "(";
            delimeter[8] = ")";

            string sz_addr = "";
            string regvalue = "";

            if (s.Contains("//"))
            {
                s = s.Substring(0, s.IndexOf("//"));
            }
            if (s.Contains("<=")) //register asignment
            {
                tokens = s.Split(delimeter, StringSplitOptions.None);
                string regname = tokens[0].Trim();
                if (regname.ToLower() == "addr")
                {
                    sz_addr = tokens[1].Trim();
                    if (sz_addr.Contains("0x")) { } else { sz_addr = "0x" + sz_addr; }
                    UInt32 addr = Convert.ToUInt32(sz_addr, 16);
                    TB4_Register treg = new TB4_Register("temprorary", "temp", addr, 16, false, false);

                    regvalue = tokens[3].Trim();
                    if (regvalue.Contains("0x")) { } else { regvalue = "0x" + regvalue; }
                    UInt16 val = Convert.ToUInt16(regvalue, 16);
                    treg.RegWrite(val);
                }
                else
                {
                    regvalue = tokens[1].Trim();
                }
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
            else if (s.Contains("Temperature Bands"))
            {
                //this sets the values for the T-Bands.  It would look like "Temperature Bands:59,68,86,113
                tokens = s.Split(new char[] { ':', ',' }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < 4; i++) TB4.tempBands[i] = Convert.ToInt16(tokens[i + 1].Trim(new char[] { ' ', '\r' }));


            }
            else
            {  }

            return "done";
        }

        public static void FileInit(string fname)
        {
            //fill the TB4_Registers array 

            TB4.thePADE_explorer.updateStatusBar(true, 0);

            FileStream file;
            StreamReader sr;

            try
            {
                // Specify file, instructions, and privelegdes
                file = new FileStream(fname, FileMode.OpenOrCreate, FileAccess.Read);
                // Create a new stream to read from a file
                sr = new StreamReader(file);
            }
            catch (Exception ex)
            {
                TB4_Exception.logError(ex, "Stream initialization error.", true);
                return;
            };

           
            while (sr.EndOfStream == false)
            {

                TB4.thePADE_explorer.incrementStatusBar();

                string s = sr.ReadLine();
                s = s.ToLower();

                ParseInit(s);
            }
            // Close StreamReader
            sr.Close();

            // Close file
            file.Close();
            TB4.thePADE_explorer.updateStatusBar(false, 0);

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
                    PADE thisPADE = ActivePADE;
                    try
                    {
                        PADE_List.TryGetValue(active_PAD_index, out thisPADE);
                        thisPADE.PADE_ch_enable[ch_num] = true;
                        thePlot.chk_chan[ch_num].Enabled = true;
                        thePlot.chk_chan[ch_num].Checked = true;
                        myRun.ch_enabled[ch_num] = true;
                    }
                    catch (Exception ex)
                    {
                        TB4_Exception.logError(ex, "Invalid PADE is active.", true);
                    }
                }
            }
            else if (paramname == "chan_minbin") //sounds like a Harry Potter character
            {
                int i = Convert.ToInt16(paramvalue);
                if ((i >= 0) && (i < 511))
                {
                    myRun.min_bin = i;
                }
            }
            else if (paramname == "chan_maxbin")
            {
                int i = Convert.ToInt16(paramvalue);
                if ((i > 0) && (i <= 511))
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
            //if (regname == "auto_bitslip") { Console.WriteLine("here"); }
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
                                catch (Exception ex)
                                {
                                    string s1 = "Could not assign to " + regname + " with val=0x" + regvalue;
                                    TB4_Exception.logError(ex, s1, true);
                                }
                                break;
                            }
                        }
                    }
                }

            }
        }

        public static void ReadArray(byte A3, byte A2, byte A1, byte A0, UInt16 data_len, int[] rdata, byte command_code = 0x02)
        {
            //byte[] data = new Byte[1025];
            //byte[] read_data = new Byte[1025];
            //ulong rxsize = 0, txsize = 0;
            //UInt32 i; UInt32 j; UInt32 k = 0;
            //UInt32 length = 2;
            //bool flg_Continue = false;
            //UInt32 StartAddr = 0; UInt32 EndAddr = 0;

            //if (data_len > 1024) { data_len = 1024; }


            //StartAddr = (UInt32)(A3 << 24) + (UInt32)(A2 << 16) + (UInt32)(A1 << 8) + (UInt32)(A0);
            //EndAddr = StartAddr + (UInt32)data_len;

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

            if (TB4.using_Ether_comms)
            {
                UInt32 full_data_length = data_len;
                UInt32 remaining_data_length = data_len;
                UInt32 data_length_finished = 0;
                UInt32 this_StartAddr = StartAddr;

                while (remaining_data_length > 0)
                {
                    this_StartAddr = this_StartAddr + data_length_finished;
                    if (remaining_data_length > 256)
                    {
                        EndAddr = this_StartAddr + 256;
                    }
                    else
                    {
                        EndAddr = this_StartAddr + remaining_data_length;
                    }

                    A3 = Convert.ToByte((this_StartAddr & 0xff000000) >> 24);
                    A2 = Convert.ToByte((this_StartAddr & 0x00ff0000) >> 16);
                    A1 = Convert.ToByte((this_StartAddr & 0x0000ff00) >> 8);
                    A0 = Convert.ToByte((this_StartAddr & 0x000000ff));

                    data[0] = 0xA5;
                    //data[1] = 7;
                    data[1] = 0x2; //this is the array read command
                    if (command_code == 0x03) { data[2] = 0x3; }
                    data[2] = A3;
                    data[3] = A2;
                    data[4] = A1;
                    data[5] = A0;

                    data[6] = (byte)((EndAddr & 0xff000000) >> 24);
                    data[7] = (byte)((EndAddr & 0x00ff0000) >> 16);
                    data[8] = (byte)((EndAddr & 0x0000ff00) >> 8);
                    //if (((EndAddr + 1) & 0x000000ff) == 0xff) { data[9]++; }
                    //data[9] = (byte)(((EndAddr + 1) & 0x000000ff));
                    data[9] = (byte)(EndAddr & 0x000000ff);
                    data[10] = 0;



                    i = 10;

                    flg_Continue = false;
                    Eth_comms.Ether_Clear();

                    length = Eth_comms.Ether_Write(data, i);

                    ushort[] udata = new ushort[length];
                    Eth_comms.Ether_ReadArray(out udata, length);

                    if ((length > 0) && (udata != null))
                    {
                        if (udata.Length > rdata.Length)
                        {
                            Exception ex = new Exception("read message larger than requested message ");
                            TB4_Exception.logError(ex, "read message larger than requested message in array read", true);
                            rdata[0] = -1;
                            return;
                        }
                        for (j = 0; j < udata.Length; j++)
                        {
                            rdata[j + data_length_finished] = (int)udata[j];
                            flg_Continue = true;
                        }
                    }
                    data_length_finished = length;
                    if (remaining_data_length > length)
                    { remaining_data_length = remaining_data_length - length; }
                    else
                    { remaining_data_length = 0; }
                }
                //UInt32 step_addr = StartAddr;
                //int ind = 0;
                //TB4_Register temp_reg=new TB4_Register("temporary","junk",step_addr,16,false,false);
                //while (step_addr <= EndAddr)
                //{
                //    temp_reg.addr = step_addr;
                //    rdata[ind] = temp_reg.RegRead();
                //    ind++;
                //    step_addr++;
                //    System.Threading.Thread.Sleep(1);
                //}

            }
            else
            {
                data[0] = 0xA5;
                //data[1] = 7;
                data[1] = 0x2; //this is the array read command
                data[2] = A3;
                data[3] = A2;
                data[4] = A1;
                data[5] = A0;
                data[6] = (byte)((EndAddr & 0xff000000) >> 24);
                data[7] = (byte)((EndAddr & 0x00ff0000) >> 16);
                data[8] = (byte)((EndAddr & 0x0000ff00) >> 8);
                if (((EndAddr + 1) & 0x000000ff) == 0xff) { data[9]++; }
                data[9] = (byte)(((EndAddr + 1) & 0x000000ff));
                data[10] = 0;
                i = 10;

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
        }
        public static string activeIndex;
        /// <summary>
        /// An easy way to change the active PADE.
        /// </summary>
        /// <param name="newPADE"></param>
        /// <param name="changeFormText"></param>
        public static void activatePADE(PADE newPADE, bool changeFormText = false, bool addToList = false, bool usingEtherComms = true)  //this is the syntax for optional parameters
        {
            newPADE.flgEther_comms = usingEtherComms;
            newPADE.IP4_add[0] = Convert.ToByte(newPADE.PADE_sn);
            newPADE.MAC_add[0] = Convert.ToByte(newPADE.PADE_sn);

            try
            {
                //PADE thisPADE = new PADE();
                //thisPADE = newPADE;

                bool isInList = false;
                try
                {
                    for (int i = 1; i <= PADE_List.Count; i++)
                    {
                        if (PADE_List[i].PADE_sn == newPADE.PADE_sn)
                        { 
                            isInList = true;
                            //thisPADE = PADE_List[i];
                        }
                    }
                }
                catch (Exception ex)
                {
                    TB4_Exception.logError(ex, ex.Message, true); //MessageBox.Show(ex.Message);
                }


                string t;
                if (isInList == false)
                {

                    if (addToList)
                    {
                        int index = TB4.PADE_List.Count + 1;
                        TB4.active_PAD_index = index;

                        t = "";

                        newPADE.PADE_index = index;
                        newPADE.IP4_add[0] = Convert.ToByte(newPADE.PADE_sn);
                        newPADE.MAC_add[0] = Convert.ToByte(newPADE.PADE_sn);

                        t = newPADE.IP4_add[3].ToString() + "."
                            + newPADE.IP4_add[2].ToString() + "."
                            + newPADE.IP4_add[1].ToString() + "."
                            + newPADE.IP4_add[0].ToString();

                        newPADE.PADE_descr = t;
                        TB4_Register newReg = PADE_explorer.registerLookup("STATUS_REG");
                        TB4_Register firmwareVersion = PADE_explorer.registerLookup("FIRMWARE_VER");
                        newPADE.PADE_fw_ver = firmwareVersion.RegRead();
                        ushort var = newReg.RegRead();

                        if ((var & 0x8000) == 0x8000)
                        {
                            newPADE.PADE_is_MASTER = false;
                            newPADE.PADE_is_SLAVE = true;
                        }
                        else
                        {
                            newPADE.PADE_is_MASTER = true;
                            newPADE.PADE_is_SLAVE = false;
                        }



                        TB4.PADE_List.Add(index, newPADE);
                        //TB4.thePAD_selector.AddPAD(index, thisPADE.flgUSB_comms);
                    }
                }
                else
                {
                    t = newPADE.PADE_descr;
                    TB4_Register newReg = PADE_explorer.registerLookup("STATUS_REG");
                    TB4_Register firmwareVersion = PADE_explorer.registerLookup("FIRMWARE_VER");
                    newPADE.PADE_fw_ver = firmwareVersion.RegRead();
                    //ushort var = newReg.RegRead();

                    //if ((var & 0x8000) == 0x8000)
                    //{
                    //    newPADE.PADE_is_MASTER = false;
                    //    newPADE.PADE_is_SLAVE = true;
                    //}
                    //else
                    //{
                    //    newPADE.PADE_is_MASTER = true;
                    //    newPADE.PADE_is_SLAVE = false;
                    //}

                }


                if (newPADE.flgUSB_comms)
                {
                    TB4.myFTDI = newPADE.PADE_FTDI;
                    TB4.myFTDI.GetSerialNumber(out t);
                    TB4.using_USB_comms = true;
                    TB4.using_Ether_comms = false;
                }
                else
                {
                    if (newPADE.flgEther_comms)
                    {
                        TB4.MAC_add = newPADE.MAC_add;
                        TB4.IP4_add = newPADE.IP4_add;
                        TB4.using_USB_comms = false;
                        TB4.using_Ether_comms = true;
                    }
                    else
                    {
                        TB4_Exception.logError(new Exception("neither USB nor Ethernet comms enabled!"), "", true);
                        //throw
                        //MessageBox.Show("neither USB nor Ethernet comms enabled!");
                    }

                }
                if (changeFormText)
                {
                    /*TB4.theRegistersForm.Text = t + " PAD " + thisPADE.PADE_index;
                    TB4.theRunForm.Text = t + " PAD " + thisPADE.PADE_index;
                    TB4.theHist1.Text = t + " PAD " + thisPADE.PADE_index;
                    TB4.thePlot.Text = t + " PAD " + thisPADE.PADE_index;
                    TB4.theHist_and_Scan.Text = t + " PAD " + thisPADE.PADE_index;
                    TB4.theFlash.Text = t + " PAD " + thisPADE.PADE_index;
                    TB4.theGBE.Text = t + " PAD " + thisPADE.PADE_index;*/
                    thePADE_explorer.selectedPADE_label.Text = newPADE.PADE_sn;
                    //thePADE_explorer.selectedPADE_label.Text += "\r\n" + thisPADE.firmwareVersion;
                    //thePADE_explorer.selectedPADE_label.Text += "\r\n" + thisPADE.firmwareVersion;
                    if (newPADE.PADE_is_MASTER)
                    {
                        thePADE_explorer.selectedPADE_label.ForeColor = System.Drawing.Color.Coral;
                        thePADE_explorer.selectedPADE_label.Text += "\r\nMASTER";
                    }
                    else
                    {
                        thePADE_explorer.selectedPADE_label.Text += "\r\n SLAVE ";
                        thePADE_explorer.selectedPADE_label.ForeColor = System.Drawing.Color.LimeGreen;
                    }
                }



                TB4.ActivePADE = newPADE;
                TB4.activeIndex = newPADE.PADE_sn;
                Application.DoEvents();
            }
            catch (Exception ex)
            {
                // MessageBox.Show("Could not find PAD");
                //TB4.theRunForm
                TB4_Exception.logError(ex, "Could not find PADE.", true);
            }

        }

        [STAThread]
        public static void Main(string[] args)
        {
            TB4_PerfMon mainClock = new TB4_PerfMon("Main timer", "This calculates how approximately how long the application takes to start up.");
            mainClock.startTime();

            startupTemplate startupWork = new startupTemplate(startupTasks);
            // startupWork.BeginInvoke(null, null);  if we want to run the slow startup process in a different thread


            startupTasks();
            Application.EnableVisualStyles();
            theRunForm.Visible = false;


            /*
            TB4.myAboutBox.ShowDialog(); //open the splash page in modal mode, disabling main form access until the about box is closed.

            DisplayControl theDisplayControl = new DisplayControl();
            System.Drawing.Rectangle myScreen = Screen.GetBounds(theDisplayControl);
            System.Drawing.Point newLocation = new System.Drawing.Point(myScreen.Width - theDisplayControl.Width, 0);
            */
            mainClock.stopTime(true);
            TB4.thePerfLog = new PerformanceLog();

            Application.Run(TB4.thePADE_explorer);





        }



        public static void startupTasks()
        {
            my_reg_collection = new List<TB4_Register>(200);

            //not used

            //populate register list
            #region DB_register Definitions
            //this code is generated automatically from the MEMEORY_MAP Excel sheet
            //my_reg_collection.Add(new TB4_Register("dummy", "DB0", 0x00100000, 16, false, false, "MAIN"));
            my_reg_collection.Add(new TB4_Register("CONTROL_REG", "DB0", 0x00100000, 16, false, false, "MAIN"));
            my_reg_collection.Add(new TB4_Register("STATUS_REG", "DB0", 0x01000000, 16, true, false, "MAIN"));
            my_reg_collection.Add(new TB4_Register("SOFTWARE_RESET", "DB0", 0x0FF00000, 16, false, true, "ZS"));
            my_reg_collection.Add(new TB4_Register("HDMI_ENABLE", "DB0", 0x01200000, 16, false, false, "EXPERT"));
            my_reg_collection.Add(new TB4_Register("ENUMERATE_SLAVES", "DB0", 0xF0000000, 16, false, true, "EXPERT"));
            my_reg_collection.Add(new TB4_Register("HDMI_STATUS", "DB0", 0x00200000, 16, true, false, "EXPERT"));
            my_reg_collection.Add(new TB4_Register("FLASH_PARAM_VERSION", "DB0", 0x0F900000, 16, true, false, "MAIN"));
            my_reg_collection.Add(new TB4_Register("FIRMWARE_VER", "DB0", 0x0FD00000, 16, true, false, "MAIN"));
            my_reg_collection.Add(new TB4_Register("PADE_TEMP", "DB0", 0x0FC00000, 16, true, false, "MAIN"));
            my_reg_collection.Add(new TB4_Register("SIB_TEMP", "DB0", 0x0EC00000, 16, true, false, "MAIN"));
            my_reg_collection.Add(new TB4_Register("PING HIT COUNT", "DB0", 0x04800000, 16, false, false, "MAIN"));
            my_reg_collection.Add(new TB4_Register("PONG HIT COUNT", "DB0", 0x04800001, 16, false, false, "MAIN"));
            my_reg_collection.Add(new TB4_Register("TOTAL HIT COUNT LOWER", "DB0", 0x04800002, 16, false, false, "MAIN"));
            my_reg_collection.Add(new TB4_Register("TOTAL HIT COUNT UPPER", "DB0", 0x04800003, 16, false, false, "MAIN"));
            //my_reg_collection.Add(new TB4_Register("READ_POINTER_OFFSET", "DB0", 0x06000000, 16, false, false, "MAIN"));
            my_reg_collection.Add(new TB4_Register("DATA_STORAGE_MODE", "DB0", 0x06000000, 16, false, false, "MAIN"));
            my_reg_collection.Add(new TB4_Register("AUTO_BITSLIP", "DB0", 0x00500004, 16, false, true, "MAIN"));
            my_reg_collection.Add(new TB4_Register("FRAME_ALIGNED", "DB0", 0x00500000, 16, true, false, "EXPERT"));
            my_reg_collection.Add(new TB4_Register("AUTO_PED_SET", "DB0", 0x06400000, 16, false, false, "MAIN"));
            my_reg_collection.Add(new TB4_Register("THRESHOLD_SCAN_VAL", "DB0", 0x07100000, 16, false, false, "THR"));
            my_reg_collection.Add(new TB4_Register("TRIG_THRESHOLD_CH0", "DB0", 0x04400000, 16, false, false, "THR"));
            my_reg_collection.Add(new TB4_Register("TRIG_THRESHOLD_CH1", "DB0", 0x04500000, 16, false, false, "THR"));
            my_reg_collection.Add(new TB4_Register("TRIG_THRESHOLD_CH2", "DB0", 0x04600000, 16, false, false, "THR"));
            my_reg_collection.Add(new TB4_Register("TRIG_THRESHOLD_CH3", "DB0", 0x04700000, 16, false, false, "THR"));
            my_reg_collection.Add(new TB4_Register("ADC_SPI_CNTRL_CH0", "DB0", 0x02000000, 16, false, false, "EXPERT"));
            my_reg_collection.Add(new TB4_Register("ADC_SPI_CNTRL_CH1", "DB0", 0x02000001, 16, false, false, "EXPERT"));
            my_reg_collection.Add(new TB4_Register("ADC_SPI_CNTRL_CH2", "DB0", 0x02000002, 16, false, false, "EXPERT"));
            my_reg_collection.Add(new TB4_Register("ADC_SPI_CNTRL_CH3", "DB0", 0x02000003, 16, false, false, "EXPERT"));
            my_reg_collection.Add(new TB4_Register("CONTROL_REG_SLAVE1", "DB0", 0x10100000, 16, false, false, "MAIN"));
            my_reg_collection.Add(new TB4_Register("CONTROL_REG_SLAVE2", "DB0", 0x20100000, 16, false, false, "MAIN"));
            my_reg_collection.Add(new TB4_Register("CONTROL_REG_SLAVE3", "DB0", 0x30100000, 16, false, false, "MAIN"));
            my_reg_collection.Add(new TB4_Register("CONTROL_REG_SLAVE4", "DB0", 0x40100000, 16, false, false, "MAIN"));
            my_reg_collection.Add(new TB4_Register("TRIG_DELAY_MASTER", "DB0", 0x04900000, 16, false, false, "MAIN"));
            my_reg_collection.Add(new TB4_Register("TRIG_DELAY_SLAVE1", "DB0", 0x14900000, 16, false, false, "MAIN"));
            my_reg_collection.Add(new TB4_Register("TRIG_DELAY_SLAVE2", "DB0", 0x24900000, 16, false, false, "MAIN"));
            my_reg_collection.Add(new TB4_Register("TRIG_DELAY_SLAVE3", "DB0", 0x34900000, 16, false, false, "MAIN"));
            my_reg_collection.Add(new TB4_Register("TRIG_DELAY_SLAVE4", "DB0", 0x44900000, 16, false, false, "MAIN"));
            my_reg_collection.Add(new TB4_Register("PARAMETER_INIT", "DB0", 0x0F500000, 16, false, true, "FLASH"));
            my_reg_collection.Add(new TB4_Register("ZS_TOTAL_FRAMES", "DB0", 0x0A300000, 16, false, false, "ZS"));
            my_reg_collection.Add(new TB4_Register("STATUS_REGzs", "DB0", 0x01000000, 16, true, false, "ZS"));
            my_reg_collection.Add(new TB4_Register("CONTROL_REGzs", "DB0", 0x00100000, 16, false, false, "ZS"));
            my_reg_collection.Add(new TB4_Register("PING_HIT_COUNT", "DB0", 0x0A100000, 16, true, false, "ZS"));
            my_reg_collection.Add(new TB4_Register("SOFTWARE_TRIGGER", "DB0", 0x0F800000, 16, false, true, "ZS"));
            my_reg_collection.Add(new TB4_Register("ZERO_SUPPRESS_TOTAL_SAMPLES", "DB0", 0x0A200000, 16, false, false, "ZS"));
            my_reg_collection.Add(new TB4_Register("P0_WRITE_PORT_STATUS", "DB0", 0x0C800000, 16, true, false, "EXPERT"));
            my_reg_collection.Add(new TB4_Register("P0_READ_PORT_STATUS", "DB0", 0x0C900000, 16, true, false, "EXPERT"));
            my_reg_collection.Add(new TB4_Register("P1_WRITE_PORT_STATUS", "DB0", 0x0CA00000, 16, true, false, "EXPERT"));
            my_reg_collection.Add(new TB4_Register("P1_READ_PORT_STATUS", "DB0", 0x0CB00000, 16, true, false, "EXPERT"));
            my_reg_collection.Add(new TB4_Register("P2_READ_PORT_STATUS", "DB0", 0x0CC00000, 16, true, false, "EXPERT"));
            my_reg_collection.Add(new TB4_Register("P3_READ_PORT_STATUS", "DB0", 0x0CD00000, 16, true, false, "EXPERT"));
            my_reg_collection.Add(new TB4_Register("P4_READ_PORT_STATUS", "DB0", 0x0CE00000, 16, true, false, "EXPERT"));
            my_reg_collection.Add(new TB4_Register("P5_READ_PORT_STATUS", "DB0", 0x0CF00000, 16, true, false, "EXPERT"));
            my_reg_collection.Add(new TB4_Register("ADC_STBY", "DB0", 0x02100000, 16, false, true, "EXPERT"));
            my_reg_collection.Add(new TB4_Register("IO_RESET", "DB0", 0x00400000, 16, false, true, "EXPERT"));
            my_reg_collection.Add(new TB4_Register("ADC_PWDN", "DB0", 0x02200000, 16, false, true, "EXPERT"));
            my_reg_collection.Add(new TB4_Register("ADC3_REG_CNTRL", "DB0", 0x00700003, 16, false, true, "EXPERT"));
            my_reg_collection.Add(new TB4_Register("ADC2_REG_CNTRL", "DB0", 0x00700002, 16, false, true, "EXPERT"));
            my_reg_collection.Add(new TB4_Register("ADC1_REG_CNTRL", "DB0", 0x00700001, 16, false, true, "EXPERT"));
            my_reg_collection.Add(new TB4_Register("ADC0_REG_CNTRL", "DB0", 0x00700000, 16, false, true, "EXPERT"));
            my_reg_collection.Add(new TB4_Register("VGA_GAIN_ADC0", "DB0", 0x00600000, 16, false, true, "EXPERT"));
            my_reg_collection.Add(new TB4_Register("VGA_GAIN_ADC1", "DB0", 0x00600001, 16, false, true, "EXPERT"));
            my_reg_collection.Add(new TB4_Register("VGA_GAIN_ADC2", "DB0", 0x00600002, 16, false, true, "EXPERT"));
            my_reg_collection.Add(new TB4_Register("VGA_GAIN_ADC3", "DB0", 0x00600003, 16, false, true, "EXPERT"));
            my_reg_collection.Add(new TB4_Register("REG_ENABLE", "DB0", 0x02300000, 16, false, true, "EXPERT"));
            my_reg_collection.Add(new TB4_Register("XSHTDN", "DB0", 0x02400000, 16, false, true, "EXPERT"));
            my_reg_collection.Add(new TB4_Register("ERR_LATCH_UPPER", "DB0", 0x0FB00000, 16, true, false, "EXPERT"));
            my_reg_collection.Add(new TB4_Register("ERR_LATCH_LOWER", "DB0", 0x0FA00000, 16, true, false, "EXPERT"));
            my_reg_collection.Add(new TB4_Register("BIAS_DAC", "DB0", 0x00A00000, 16, false, false, "BIAS"));
            my_reg_collection.Add(new TB4_Register("BIAS_VMON", "DB0", 0x00B00000, 16, true, false, "BIAS"));
            my_reg_collection.Add(new TB4_Register("BIAS_IMON", "DB0", 0x00C00000, 16, true, false, "BIAS"));
            my_reg_collection.Add(new TB4_Register("BIAS_NUM_AVG", "DB0", 0x00D00000, 16, false, false, "BIAS"));
            my_reg_collection.Add(new TB4_Register("BIAS_OFFSET_CH0", "DB0", 0x00800000, 16, false, false, "BIAS"));
            my_reg_collection.Add(new TB4_Register("BIAS_OFFSET_CH1", "DB0", 0x00800001, 16, false, false, "BIAS"));
            my_reg_collection.Add(new TB4_Register("BIAS_OFFSET_CH2", "DB0", 0x00800002, 16, false, false, "BIAS"));
            my_reg_collection.Add(new TB4_Register("BIAS_OFFSET_CH3", "DB0", 0x00800003, 16, false, false, "BIAS"));
            my_reg_collection.Add(new TB4_Register("BIAS_OFFSET_CH4", "DB0", 0x00800004, 16, false, false, "BIAS"));
            my_reg_collection.Add(new TB4_Register("BIAS_OFFSET_CH5", "DB0", 0x00800005, 16, false, false, "BIAS"));
            my_reg_collection.Add(new TB4_Register("BIAS_OFFSET_CH6", "DB0", 0x00800006, 16, false, false, "BIAS"));
            my_reg_collection.Add(new TB4_Register("BIAS_OFFSET_CH7", "DB0", 0x00800007, 16, false, false, "BIAS"));
            my_reg_collection.Add(new TB4_Register("BIAS_OFFSET_CH8", "DB0", 0x00800008, 16, false, false, "BIAS"));
            my_reg_collection.Add(new TB4_Register("BIAS_OFFSET_CH9", "DB0", 0x00800009, 16, false, false, "BIAS"));
            my_reg_collection.Add(new TB4_Register("BIAS_OFFSET_CH10", "DB0", 0x0080000A, 16, false, false, "BIAS"));
            my_reg_collection.Add(new TB4_Register("BIAS_OFFSET_CH11", "DB0", 0x0080000B, 16, false, false, "BIAS"));
            my_reg_collection.Add(new TB4_Register("BIAS_OFFSET_CH12", "DB0", 0x0080000C, 16, false, false, "BIAS"));
            my_reg_collection.Add(new TB4_Register("BIAS_OFFSET_CH13", "DB0", 0x0080000D, 16, false, false, "BIAS"));
            my_reg_collection.Add(new TB4_Register("BIAS_OFFSET_CH14", "DB0", 0x0080000E, 16, false, false, "BIAS"));
            my_reg_collection.Add(new TB4_Register("BIAS_OFFSET_CH15", "DB0", 0x0080000F, 16, false, false, "BIAS"));
            my_reg_collection.Add(new TB4_Register("BIAS_OFFSET_CH16", "DB0", 0x00800010, 16, false, false, "BIAS"));
            my_reg_collection.Add(new TB4_Register("BIAS_OFFSET_CH17", "DB0", 0x00800011, 16, false, false, "BIAS"));
            my_reg_collection.Add(new TB4_Register("BIAS_OFFSET_CH18", "DB0", 0x00800012, 16, false, false, "BIAS"));
            my_reg_collection.Add(new TB4_Register("BIAS_OFFSET_CH19", "DB0", 0x00800013, 16, false, false, "BIAS"));
            my_reg_collection.Add(new TB4_Register("BIAS_OFFSET_CH20", "DB0", 0x00800014, 16, false, false, "BIAS"));
            my_reg_collection.Add(new TB4_Register("BIAS_OFFSET_CH21", "DB0", 0x00800015, 16, false, false, "BIAS"));
            my_reg_collection.Add(new TB4_Register("BIAS_OFFSET_CH22", "DB0", 0x00800016, 16, false, false, "BIAS"));
            my_reg_collection.Add(new TB4_Register("BIAS_OFFSET_CH23", "DB0", 0x00800017, 16, false, false, "BIAS"));
            my_reg_collection.Add(new TB4_Register("BIAS_OFFSET_CH24", "DB0", 0x00800018, 16, false, false, "BIAS"));
            my_reg_collection.Add(new TB4_Register("BIAS_OFFSET_CH25", "DB0", 0x00800019, 16, false, false, "BIAS"));
            my_reg_collection.Add(new TB4_Register("BIAS_OFFSET_CH26", "DB0", 0x0080001A, 16, false, false, "BIAS"));
            my_reg_collection.Add(new TB4_Register("BIAS_OFFSET_CH27", "DB0", 0x0080001B, 16, false, false, "BIAS"));
            my_reg_collection.Add(new TB4_Register("BIAS_OFFSET_CH28", "DB0", 0x0080001C, 16, false, false, "BIAS"));
            my_reg_collection.Add(new TB4_Register("BIAS_OFFSET_CH29", "DB0", 0x0080001D, 16, false, false, "BIAS"));
            my_reg_collection.Add(new TB4_Register("BIAS_OFFSET_CH30", "DB0", 0x0080001E, 16, false, false, "BIAS"));
            my_reg_collection.Add(new TB4_Register("BIAS_OFFSET_CH31", "DB0", 0x0080001F, 16, false, false, "BIAS"));
            my_reg_collection.Add(new TB4_Register("ZERO_SUPPRESS_EVENTS", "DB0", 0x0A000000, 16, true, false, "ZS"));
            my_reg_collection.Add(new TB4_Register("KILL_MASK_LOWER", "DB0", 0x0A400000, 16, false, false, "ZS"));
            my_reg_collection.Add(new TB4_Register("KILL_MASK_UPPER", "DB0", 0x0A400001, 16, false, false, "ZS"));
            my_reg_collection.Add(new TB4_Register("FRAME_LENGTH", "DB0", 0x0A500000, 16, false, false, "ZS"));
            my_reg_collection.Add(new TB4_Register("TIMER_STATUS", "DB0", 0x0A600000, 16, true, false, "ZS"));
            my_reg_collection.Add(new TB4_Register("DEBUG_CHAN_ADDR", "DB0", 0x0A700000, 16, false, false, "ZS"));
            my_reg_collection.Add(new TB4_Register("BOARD_ID", "DB0", 0x0A800000, 16, false, false, "MAIN"));
            my_reg_collection.Add(new TB4_Register("SIB_ID_W0", "DB0", 0x0A900000, 16, true, false, "MAIN"));
            my_reg_collection.Add(new TB4_Register("SIB_ID_W1", "DB0", 0x0A900001, 16, true, false, "MAIN"));
            my_reg_collection.Add(new TB4_Register("SIB_ID_W2", "DB0", 0x0A900002, 16, true, false, "MAIN"));
            my_reg_collection.Add(new TB4_Register("SIB_ID_W3", "DB0", 0x0A900003, 16, true, false, "MAIN"));
            my_reg_collection.Add(new TB4_Register("ROTATION_ANGLE", "DB0", 0x0AA00000, 16, false, false, "MAIN"));
            my_reg_collection.Add(new TB4_Register("CSR", "DB0", 0x00100000, 16, false, false, "THR"));
            my_reg_collection.Add(new TB4_Register("THR_SCAN_TIME", "DB0", 0x07000000, 16, false, false, "THR"));
            my_reg_collection.Add(new TB4_Register("SOFT_TRIG", "DB0", 0x0F800000, 16, false, false, "THR"));
            my_reg_collection.Add(new TB4_Register("THR_00", "DB0", 0x04400000, 16, false, false, "THR"));
            my_reg_collection.Add(new TB4_Register("THR_01", "DB0", 0x04400001, 16, false, false, "THR"));
            my_reg_collection.Add(new TB4_Register("THR_02", "DB0", 0x04400002, 16, false, false, "THR"));
            my_reg_collection.Add(new TB4_Register("THR_03", "DB0", 0x04400003, 16, false, false, "THR"));
            my_reg_collection.Add(new TB4_Register("THR_04", "DB0", 0x04400004, 16, false, false, "THR"));
            my_reg_collection.Add(new TB4_Register("THR_05", "DB0", 0x04400005, 16, false, false, "THR"));
            my_reg_collection.Add(new TB4_Register("THR_06", "DB0", 0x04400006, 16, false, false, "THR"));
            my_reg_collection.Add(new TB4_Register("THR_07", "DB0", 0x04400007, 16, false, false, "THR"));
            my_reg_collection.Add(new TB4_Register("THR_08", "DB0", 0x04500000, 16, false, false, "THR"));
            my_reg_collection.Add(new TB4_Register("THR_09", "DB0", 0x04500001, 16, false, false, "THR"));
            my_reg_collection.Add(new TB4_Register("THR_10", "DB0", 0x04500002, 16, false, false, "THR"));
            my_reg_collection.Add(new TB4_Register("THR_11", "DB0", 0x04500003, 16, false, false, "THR"));
            my_reg_collection.Add(new TB4_Register("THR_12", "DB0", 0x04500004, 16, false, false, "THR"));
            my_reg_collection.Add(new TB4_Register("THR_13", "DB0", 0x04500005, 16, false, false, "THR"));
            my_reg_collection.Add(new TB4_Register("THR_14", "DB0", 0x04500006, 16, false, false, "THR"));
            my_reg_collection.Add(new TB4_Register("THR_15", "DB0", 0x04500007, 16, false, false, "THR"));
            my_reg_collection.Add(new TB4_Register("THR_16", "DB0", 0x04600000, 16, false, false, "THR"));
            my_reg_collection.Add(new TB4_Register("THR_17", "DB0", 0x04600001, 16, false, false, "THR"));
            my_reg_collection.Add(new TB4_Register("THR_18", "DB0", 0x04600002, 16, false, false, "THR"));
            my_reg_collection.Add(new TB4_Register("THR_19", "DB0", 0x04600003, 16, false, false, "THR"));
            my_reg_collection.Add(new TB4_Register("THR_20", "DB0", 0x04600004, 16, false, false, "THR"));
            my_reg_collection.Add(new TB4_Register("THR_21", "DB0", 0x04600005, 16, false, false, "THR"));
            my_reg_collection.Add(new TB4_Register("THR_22", "DB0", 0x04600006, 16, false, false, "THR"));
            my_reg_collection.Add(new TB4_Register("THR_23", "DB0", 0x04600007, 16, false, false, "THR"));
            my_reg_collection.Add(new TB4_Register("THR_24", "DB0", 0x04600000, 16, false, false, "THR"));
            my_reg_collection.Add(new TB4_Register("THR_25", "DB0", 0x04600001, 16, false, false, "THR"));
            my_reg_collection.Add(new TB4_Register("THR_26", "DB0", 0x04600002, 16, false, false, "THR"));
            my_reg_collection.Add(new TB4_Register("THR_27", "DB0", 0x04600003, 16, false, false, "THR"));
            my_reg_collection.Add(new TB4_Register("THR_28", "DB0", 0x04600004, 16, false, false, "THR"));
            my_reg_collection.Add(new TB4_Register("THR_29", "DB0", 0x04600005, 16, false, false, "THR"));
            my_reg_collection.Add(new TB4_Register("THR_30", "DB0", 0x04600006, 16, false, false, "THR"));
            my_reg_collection.Add(new TB4_Register("THR_31", "DB0", 0x04600007, 16, false, false, "THR"));
            my_reg_collection.Add(new TB4_Register("TRIG_THRESHOLD_CH0-7", "DB0", 0x04400000, 16, false, false, "THR"));
            my_reg_collection.Add(new TB4_Register("TRIG_THRESHOLD_CH8-15", "DB0", 0x04500000, 16, false, false, "THR"));
            my_reg_collection.Add(new TB4_Register("TRIG_THRESHOLD_CH16-23", "DB0", 0x04600000, 16, false, false, "THR"));
            my_reg_collection.Add(new TB4_Register("TRIG_THRESHOLD_CH24-31", "DB0", 0x04700000, 16, false, false, "THR"));

            my_reg_collection.Add(new TB4_Register("PHY_CNTRL", "DB0", 0x0090FC84, 32, false, false, "ETH"));
            my_reg_collection.Add(new TB4_Register("PHY_DATA", "DB0", 0x0090FC88, 32, false, false, "ETH"));
            //my_reg_collection.Add(  new TB4_Register("MAC_GPIO_CTRL", "DB0", 0x0090FC8C, 32, false, false, "ETH"));
            my_reg_collection.Add(new TB4_Register("MAC_RXINDICATOR", "DB0", 0x0090FC90, 32, false, false, "ETH"));
            my_reg_collection.Add(new TB4_Register("MAC_TXST", "DB0", 0x0090FC94, 32, false, false, "ETH"));
            my_reg_collection.Add(new TB4_Register("MAC_MDCLKPAT", "DB0", 0x0090FCA0, 32, false, false, "ETH"));
            my_reg_collection.Add(new TB4_Register("MAC_RXCHKSUMCNT", "DB0", 0x0090FCA4, 32, false, false, "ETH"));
            my_reg_collection.Add(new TB4_Register("MAC_RXCRCNT", "DB0", 0x0090FCA8, 32, false, false, "ETH"));
            my_reg_collection.Add(new TB4_Register("MAC_TXFAILCNT", "DB0", 0x0090FCAC, 32, false, false, "ETH"));
            my_reg_collection.Add(new TB4_Register("MAC_MAXRXLEN", "DB0", 0x0090FCB8, 32, false, false, "ETH"));
            my_reg_collection.Add(new TB4_Register("MAC_SOFTRST", "DB0", 0x0090FCEC, 32, false, false, "ETH"));
            my_reg_collection.Add(new TB4_Register("MAC_UPPERDATA", "DB0", 0x00A00000, 32, false, false, "ETH"));

            my_reg_collection.Add(new TB4_Register("MAC_SOURCE_ADDR_LOW", "DB0", 0x03100000, 16, false, false, "ETH"));
            my_reg_collection.Add(new TB4_Register("MAC_SOURCE_ADDR_MED", "DB0", 0x03100001, 16, false, false, "ETH"));
            my_reg_collection.Add(new TB4_Register("MAC_SOURCE_ADDR_HIGH", "DB0", 0x03100002, 16, false, false, "ETH"));
            my_reg_collection.Add(new TB4_Register("MAC_DEST_ADDR_LOW", "DB0", 0x03200000, 16, false, false, "ETH"));
            my_reg_collection.Add(new TB4_Register("MAC_DEST_ADDR_MED", "DB0", 0x03200001, 16, false, false, "ETH"));
            my_reg_collection.Add(new TB4_Register("MAC_DEST_ADDR_HIGH", "DB0", 0x03200002, 16, false, false, "ETH"));
            my_reg_collection.Add(new TB4_Register("MAC_SOURCE_IP_LOW", "DB0", 0x03300000, 16, false, false, "ETH"));
            my_reg_collection.Add(new TB4_Register("MAC_SOURCE_IP_HIGH", "DB0", 0x03300001, 16, false, false, "ETH"));
            my_reg_collection.Add(new TB4_Register("MAC_DEST_IP_LOW", "DB0", 0x03400000, 16, false, false, "ETH"));
            my_reg_collection.Add(new TB4_Register("MAC_DEST_IP_HIGH", "DB0", 0x03400001, 16, false, false, "ETH"));
            my_reg_collection.Add(new TB4_Register("MAC_TEST_PACK_LOW", "DB0", 0x03500000, 16, false, false, "ETH"));
            my_reg_collection.Add(new TB4_Register("MAC_TEST_PACK_HIGH", "DB0", 0x03500001, 16, false, false, "ETH"));
            my_reg_collection.Add(new TB4_Register("MAC_TEST_DATA_LENGTH", "DB0", 0x03600000, 16, false, false, "ETH"));
            my_reg_collection.Add(new TB4_Register("MAC_RESET", "DB0", 0x0F700000, 16, false, false, "ETH"));

            my_reg_collection.Add(new TB4_Register("TRIG_POST_STORE", "DB0", 0x05100000, 16, false, false, "ZS"));
            my_reg_collection.Add(new TB4_Register("TRIG_ARM", "DB0", 0x05200000, 16, false, false, "ZS"));
            my_reg_collection.Add(new TB4_Register("MEM_CLEAR", "DB0", 0x05300000, 16, false, true, "ZS"));

            my_reg_collection.Add(new TB4_Register("MEM_TRIG_COUNT", "DB0", 0x05400000, 16, true, false, "ZS"));
            my_reg_collection.Add(new TB4_Register("MEM_ERROR_COUNT", "DB0", 0x05400001, 16, true, false, "ZS"));
            my_reg_collection.Add(new TB4_Register("READ_TRIG_NUM", "DB0", 0x05500000, 16, false, false, "ZS"));
            my_reg_collection.Add(new TB4_Register("READ_TRIG_NUM_START", "DB0", 0x05500001, 16, false, false, "ZS"));
            my_reg_collection.Add(new TB4_Register("READ_TRIG_NUM_STOP", "DB0", 0x05500002, 16, false, false, "ZS"));
            my_reg_collection.Add(new TB4_Register("READ_ENABLE", "DB0", 0x05600000, 16, false, false, "ZS"));
            my_reg_collection.Add(new TB4_Register("DDR_STAT_WRITE", "DB0", 0x0C800000, 16, false, false, "ZS"));
            my_reg_collection.Add(new TB4_Register("DDR_STAT_READ","DB0",0x0C800001, 16, false, false, "ZS"));
            my_reg_collection.Add(new TB4_Register("DDR_ADDR_WRITE_LOW", "DB0", 0x0C900000, 16, false, false, "ZS"));
            my_reg_collection.Add(new TB4_Register("DDR_ADDR_WRITE_HIGH", "DB0", 0x0C900001, 16, false, false, "ZS"));
            my_reg_collection.Add(new TB4_Register("DDR_ADDR_READ_LOW", "DB0", 0x0CA00000, 16, false, false, "ZS"));
            my_reg_collection.Add(new TB4_Register("DDR_ADDR_READ_HIGH", "DB0", 0x0CA00001, 16, false, false, "ZS"));
            my_reg_collection.Add(new TB4_Register("DDR_WRITE_DATA_LOW", "DB0", 0x0CB00000, 16, false, false, "ZS"));
            my_reg_collection.Add(new TB4_Register("DDR_WRITE_DATA_HIGH", "DB0", 0x0CB00001, 16, false, false, "ZS"));
            my_reg_collection.Add(new TB4_Register("DDR_WRITE_FIFO_PUSH", "DB0", 0x0CC00000, 16, false, true, "ZS"));
            my_reg_collection.Add(new TB4_Register("DDR_WRITE_ENABLE", "DB0", 0x0CC00001, 16, false, true, "ZS"));
            my_reg_collection.Add(new TB4_Register("DDR_READ_ENABLE", "DB0", 0x0CE00000, 16, false, true, "ZS"));
            my_reg_collection.Add(new TB4_Register("DDR_READ_FIFO_PULL", "DB0", 0x0CE00001, 16, false, true, "ZS"));
            my_reg_collection.Add(new TB4_Register("DDR_READ_DATA_LOW", "DB0", 0x0CF00000, 16, true, false, "ZS"));
            my_reg_collection.Add(new TB4_Register("DDR_READ_DATA_HIGH", "DB0", 0x0CF00001, 16, true, false, "ZS"));

            

            #endregion DB_register Definitions
            #region FLASH_DPRAM
            my_reg_collection.Add(new TB4_Register("HARD_RESET", "DB0", 0x0FE00000, 16, false, true, "FLASH"));
            my_reg_collection.Add(new TB4_Register("FLASH_BINARY", "DB0", 0x00300000, 16, false, true, "FLASH"));
            my_reg_collection.Add(new TB4_Register("FLASH_CONTROL", "DB0", 0x08000000, 16, false, false, "FLASH"));
            my_reg_collection.Add(new TB4_Register("FLASH_OP_CODE", "DB0", 0x08100000, 16, false, false, "FLASH"));
            my_reg_collection.Add(new TB4_Register("FLASH_PAGE_ADDR", "DB0", 0x08200000, 16, false, false, "FLASH"));
            my_reg_collection.Add(new TB4_Register("FLASH_BYTE_ADDR", "DB0", 0x08300000, 16, false, false, "FLASH"));
            my_reg_collection.Add(new TB4_Register("FLASH_RAM_0", "DB0", 0x08800000, 16, false, false, "FLASH"));
            my_reg_collection.Add(new TB4_Register("FLASH_RAM_1", "DB0", 0x08800001, 16, false, false, "FLASH"));
            my_reg_collection.Add(new TB4_Register("FLASH_RAM_2", "DB0", 0x08800002, 16, false, false, "FLASH"));
            my_reg_collection.Add(new TB4_Register("FLASH_RAM_3", "DB0", 0x08800003, 16, false, false, "FLASH"));
            my_reg_collection.Add(new TB4_Register("FLASH_RAM_4", "DB0", 0x08800004, 16, false, false, "FLASH"));
            my_reg_collection.Add(new TB4_Register("FLASH_RAM_5", "DB0", 0x08800005, 16, false, false, "FLASH"));
            my_reg_collection.Add(new TB4_Register("FLASH_RAM_6", "DB0", 0x08800006, 16, false, false, "FLASH"));
            my_reg_collection.Add(new TB4_Register("FLASH_RAM_7", "DB0", 0x08800007, 16, false, false, "FLASH"));
            my_reg_collection.Add(new TB4_Register("FLASH_RAM_1F8", "DB0", 0x088001F8, 16, false, false, "FLASH"));
            my_reg_collection.Add(new TB4_Register("FLASH_RAM_1F9", "DB0", 0x088001F9, 16, false, false, "FLASH"));
            my_reg_collection.Add(new TB4_Register("FLASH_RAM_1FA", "DB0", 0x088001FA, 16, false, false, "FLASH"));
            my_reg_collection.Add(new TB4_Register("FLASH_RAM_1FB", "DB0", 0x088001FB, 16, false, false, "FLASH"));
            my_reg_collection.Add(new TB4_Register("FLASH_RAM_1FC", "DB0", 0x088001FC, 16, false, false, "FLASH"));
            my_reg_collection.Add(new TB4_Register("FLASH_RAM_1FD", "DB0", 0x088001FD, 16, false, false, "FLASH"));
            my_reg_collection.Add(new TB4_Register("FLASH_RAM_1FE", "DB0", 0x088001FE, 16, false, false, "FLASH"));
            my_reg_collection.Add(new TB4_Register("FLASH_RAM_1FF", "DB0", 0x088001FF, 16, false, false, "FLASH"));

            //TB4.TB4_Registers[1]
            int ii = 0;
            foreach (TB4_Register reg in my_reg_collection)
            {
                TB4.TB4_Registers[ii] = reg;
                ii++;
            }
            #endregion FLASH_DPRAM
            //----------------------



            for (int i = 0; i < TB4_Registers.Length; i++)
            {
                if (TB4_Registers[i] != null)  //failsafe
                {
                    if (TB4_Registers[i].name.Length > 0)
                    {
                        int v = 0;

                        TB4_Registers_Dict.TryGetValue(TB4_Registers[i].name, out v);  //out===ref
                        if (v > 0)
                        {
                            TB4_Exception.logError(null, "A duplicate register was added.", true);
                        }
                        else
                        {
                            TB4_Registers_Dict.Add(TB4_Registers[i].name, i);
                        }
                    }
                }

            }
            thePADE_explorer = new PADE_explorer();

            //this takes forever
            TB4.theRegistersForm = new Registers();

            TB4.thePlot = new Plot0();
            thePlot.Visible = false;
            formList.Add(thePlot);

            TB4.theFlash = new Flash0();
            theFlash.Visible = false;
            formList.Add(theFlash);

            TB4.theBiasOffset = new BiasOffset();
            theBiasOffset.Visible = false;
            formList.Add(theBiasOffset);

            TB4.theHist = new Hist0();
            theHist.Visible = false;
            //formList.Add(theHist);

            TB4.theHist1 = new Hist1();
            theHist1.Visible = false;
            formList.Add(theHist1);

            TB4.theHist_and_Scan = new Hist_and_Scan();
            theHist_and_Scan.Visible = false;
            formList.Add(theHist_and_Scan);

            TB4.thePAD_selector = new PAD_select();
            thePAD_selector.Visible = false;

            TB4.theGBE = new GBE();
            theGBE.Visible = false;
            formList.Add(theGBE);

            TB4.theDRAM = new DRAM();
            theDRAM.Visible = false;
            formList.Add(theDRAM);

            TB4.thePADE_Selector = new PADE_Selector();
            thePADE_Selector.Visible = false;
            formList.Add(thePADE_Selector);

            TB4.theDataDebug = new DataDebug();
            theDataDebug.Visible = false;
            formList.Add(theDataDebug);

            TB4.theSystemViewer = new SystemViewer();
            theSystemViewer.Visible = false;
            formList.Add(theSystemViewer);

            TB4.theRemoteControl = new RemoteControl();
            TB4.theRemoteControl.StartRC();

        }


    }




}
