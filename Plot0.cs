using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using ZedGraph;
using WeifenLuo.WinFormsUI.Docking;

namespace PADE
{
    public partial class Plot0 : DockContent
    {
        static float[,] ChMaxBuf = new float[32, 20];
        static UInt16[,] ChData = new UInt16[32, 15];
        static int BufPointer = 0;
        static float[] normalization_factor = new float[32];
        static float[] cut = new float[32];

        public CheckBox[] chk_chan = new CheckBox[32];
        public Button btn_all_off_A = new Button();
        public Button btn_all_off_B = new Button();
        public Button btn_all_off_C = new Button();
        public Button btn_all_off_D = new Button();
        public Button btn_all_on_A = new Button();
        public Button btn_all_on_B = new Button();
        public Button btn_all_on_C = new Button();
        public Button btn_all_on_D = new Button();
        //        public string[] cmb_Start0_text = new string[8];
        //        public ComboBox[] cmb_Trace_arr = new ComboBox[4];

        public Plot0()
        {
            InitializeComponent();
            for (int i = 0; i < 32; i++)
            {
                this.chk_chan[i] = new CheckBox();
                this.chk_chan[i].AutoSize = true;
                this.chk_chan[i].Enabled = false;
                this.chk_chan[i].Location = new System.Drawing.Point(5 + (60 * (i & 0x18) >> 3), 425 + (20 * (i & 0x7)));
                this.chk_chan[i].Name = "checkBox" + i.ToString();
                this.chk_chan[i].Size = new System.Drawing.Size(51, 17);
                this.chk_chan[i].UseVisualStyleBackColor = true;
                this.chk_chan[i].Visible = true;
                chk_chan[i].Text = "Ch" + i.ToString();
                this.Controls.Add(chk_chan[i]);
            }
            btn_all_off_A = new Button();
            btn_all_off_A.Enabled = true;
            btn_all_off_A.Text = "A off";
            btn_all_off_A.Size = new System.Drawing.Size(51, 17);
            btn_all_off_A.Location = new System.Drawing.Point(5, 425 + 160);
            btn_all_off_A.Click += new EventHandler(btn_all_off_A_Click);
            this.Controls.Add(btn_all_off_A);

            btn_all_on_A = new Button();
            btn_all_on_A.Enabled = true;
            btn_all_on_A.Text = "A on";
            btn_all_on_A.Size = new System.Drawing.Size(51, 17);
            btn_all_on_A.Location = new System.Drawing.Point(5, 425 + 180);
            btn_all_on_A.Click += new EventHandler(btn_all_on_A_Click);
            this.Controls.Add(btn_all_on_A);

            btn_all_off_B = new Button();
            btn_all_off_B.Enabled = true;
            btn_all_off_B.Text = "B off";
            btn_all_off_B.Size = new System.Drawing.Size(51, 17);
            btn_all_off_B.Location = new System.Drawing.Point(5 + 60, 425 + 160);
            btn_all_off_B.Click += new EventHandler(btn_all_off_B_Click);
            this.Controls.Add(btn_all_off_B);

            btn_all_off_C = new Button();
            btn_all_off_C.Enabled = true;
            btn_all_off_C.Text = "C off";
            btn_all_off_C.Size = new System.Drawing.Size(51, 17);
            btn_all_off_C.Location = new System.Drawing.Point(5 + 120, 425 + 160);
            btn_all_off_C.Click += new EventHandler(btn_all_off_C_Click);
            this.Controls.Add(btn_all_off_C);

            btn_all_off_D = new Button();
            btn_all_off_D.Enabled = true;
            btn_all_off_D.Text = "D off";
            btn_all_off_D.Size = new System.Drawing.Size(51, 17);
            btn_all_off_D.Location = new System.Drawing.Point(5 + 180, 425 + 160);
            btn_all_off_D.Click += new EventHandler(btn_all_off_D_Click);
            this.Controls.Add(btn_all_off_D);

        }

        void btn_all_off_A_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 8; i++)
            { chk_chan[i].Checked = false; }
            Thread.Sleep(0);
        }
        void btn_all_on_A_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 8; i++)
            {
                chk_chan[i].Checked = true;
                TB4.ActivePADE.PADE_ch_enable[i] = true;
            }
            Thread.Sleep(0);
        }
        void btn_all_off_B_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 8; i++)
            { chk_chan[i + 8].Checked = false; }
            Thread.Sleep(0);
        }
        void btn_all_off_C_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 8; i++)
            {
                chk_chan[i + 16].Checked = false;
            }
            Thread.Sleep(0);
        }
        void btn_all_off_D_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 8; i++)
            {
                chk_chan[i + 24].Checked = false;
            }
            Thread.Sleep(0);
        }


        public void Plot8_display()
        {
            double min_val = 66000;
            double max_val = 1;

            GraphPane myPane = zg1.GraphPane;

            if (chk_persist.Checked)
            { myPane.Legend.IsVisible = false; }
            else
            {
                myPane.CurveList.Clear();
                myPane.Legend.IsVisible = false;
            }


            // Set the titles and axis labels
            myPane.Title.Text = "ADC data";
            myPane.XAxis.Title.Text = "sample #";
            myPane.YAxis.Title.Text = "ADC  count";
            double min_x = (double)ud_MinX.Value;
            double max_x = (double)ud_MaxX.Value;

            PointPairList list = new PointPairList();
            if (TB4.myRun.flg_ZS)
            {
                double[] x = new double[4096];
                double[] y = new double[4096];
                if (max_x > 4095) { max_x = 4095; }

                int num_samples = 10;
                int num_presamples = 3;
                int i_offset = -1;
                for (int j = 0; j < 32; j++) //iterate through the channels
                {
                    if (TB4.myRun.ch_enabled[j] && chk_chan[j].Checked)
                    {
                        for (int i = 0; i < 4096; i++) { x[i] = i; y[i] = 47; }
                        if (i_offset < 0) { i_offset = 1; }//{ i_offset = TB4.myRun.event_data[j * 512]; }
                        for (int ne = 0; ne < TB4.myRun.num_zs_ev[j]; ne++)
                        {
                            if (TB4.myRun.flgSumOnly)
                            {
                                int i_start = 1;
                                //int i_start = TB4.myRun.event_data[j * 512 + 2 * ne] - i_offset + num_presamples;
                                int[] ch_data = new int[512];
                                for (int ind = 0; ind < 512; ind++) { ch_data[ind] = TB4.myRun.event_data[ind + j * 512]; }
                                //if (i_start < 1) 

                                { i_start = 1; }
                                if (i_start < 4080)
                                {
                                    for (int ii = 1; ii < 6; ii++)
                                    { y[i_start + ii] = ii * (TB4.myRun.event_data[j * 512 + 2 * ne + 1] / 30) + 47; }
                                    for (int ii = 1; ii < 6; ii++)
                                    { y[i_start + ii + 5] = (5 - ii) * (TB4.myRun.event_data[j * 512 + 2 * ne + 1] / 30) + 47; }
                                }
                            }
                            else
                            {
                                int i_start = 1;
                                i_start = TB4.myRun.event_data[j * 512 + num_samples * ne] - i_offset + num_presamples;
                                int[] ch_data = new int[512];
                                for (int ind = 0; ind < 512; ind++) { ch_data[ind] = TB4.myRun.event_data[ind + j * 512]; }
                                for (int ii = 1; ii < num_samples; ii++)
                                {
                                    if (((i_start + ii) < y.Length) && (i_start + ii > 0))
                                    {
                                        y[i_start + ii] = TB4.myRun.event_data[j * 512 + num_samples * ne + ii];
                                    }
                                    else
                                    {
                                        i_start = 0;
                                    }
                                }
                            }

                        }
                        list = new PointPairList(x, y);
                        max_val = (double)ud_MaxY.Value;
                        min_val = (double)ud_MinY.Value;
                        if (max_val > 0 && max_val < 4100) { } else { max_val = 4100; }
                        if (min_val >= 0 && min_val < 4000) { } else { min_val = 0; }
                        if (max_val > min_val) { } else { max_val = 500; min_val = 0; }

                        string t = "CH" + j.ToString();
                        if ((j & 7) == 0) { LineItem myCurve = myPane.AddCurve(t, list, Color.Orange, SymbolType.None); }
                        if ((j & 7) == 1) { LineItem myCurve = myPane.AddCurve(t, list, Color.Blue, SymbolType.None); }
                        if ((j & 7) == 2) { LineItem myCurve = myPane.AddCurve(t, list, Color.Purple, SymbolType.None); }
                        if ((j & 7) == 3) { LineItem myCurve = myPane.AddCurve(t, list, Color.Green, SymbolType.None); }
                        if ((j & 7) == 4) { LineItem myCurve = myPane.AddCurve(t, list, Color.Red, SymbolType.None); }
                        if ((j & 7) == 5) { LineItem myCurve = myPane.AddCurve(t, list, Color.PowderBlue, SymbolType.None); }
                        if ((j & 7) == 6) { LineItem myCurve = myPane.AddCurve(t, list, Color.PeachPuff, SymbolType.None); }
                        if ((j & 7) == 7) { LineItem myCurve = myPane.AddCurve(t, list, Color.Peru, SymbolType.None); }
                        // if (j >7  ) { LineItem myCurve = myPane.AddCurve(t, list, Color.Pink, SymbolType.None); }

                    }
                }
            }
            else //flg_ZS==false
            {
                double[] x = new double[512];
                double[] y = new double[512];

                if (max_x > 512) { max_x = 512; }

                for (int j = 0; j < 32; j++)
                {
                    if (TB4.myRun.ch_enabled[j] && chk_chan[j].Checked)
                    {
                        for (int i = 0; i < 512; i++) { x[i] = i; y[i] = TB4.myRun.event_data[j * 512 + i]; }
                        list = new PointPairList(x, y);
                        max_val = (double)ud_MaxY.Value;
                        min_val = (double)ud_MinY.Value;
                        if (max_val > 0 && max_val < 4100) { } else { max_val = 2200; }
                        if (min_val >= 0 && min_val < 4100) { } else { min_val = 0; }
                        if (max_val > min_val) { } else { max_val = 2200; min_val = 0; }

                        string t = "CH" + j.ToString();
                        if ((j & 7) == 0) { LineItem myCurve = myPane.AddCurve(t, list, Color.Orange, SymbolType.None); }
                        if ((j & 7) == 1) { LineItem myCurve = myPane.AddCurve(t, list, Color.Blue, SymbolType.None); }
                        if ((j & 7) == 2) { LineItem myCurve = myPane.AddCurve(t, list, Color.Purple, SymbolType.None); }
                        if ((j & 7) == 3) { LineItem myCurve = myPane.AddCurve(t, list, Color.Green, SymbolType.None); }
                        if ((j & 7) == 4) { LineItem myCurve = myPane.AddCurve(t, list, Color.Red, SymbolType.None); }
                        if ((j & 7) == 5) { LineItem myCurve = myPane.AddCurve(t, list, Color.PowderBlue, SymbolType.None); }
                        if ((j & 7) == 6) { LineItem myCurve = myPane.AddCurve(t, list, Color.PeachPuff, SymbolType.None); }
                        if ((j & 7) == 7) { LineItem myCurve = myPane.AddCurve(t, list, Color.Peru, SymbolType.None); }
                        //                    if (j >7  ) { LineItem myCurve = myPane.AddCurve(t, list, Color.Pink, SymbolType.None); }

                    }
                }
            }
            // Show the x axis grid
            myPane.XAxis.MajorGrid.IsVisible = true;
            // Make the Y axis scale red
            //myPane.YAxis.Scale.FontSpec.FontColor = Color.Red;
            //myPane.YAxis.Title.FontSpec.FontColor = Color.Red;
            // turn off the opposite tics so the Y tics don't show up on the Y2 axis
            myPane.YAxis.MajorTic.IsOpposite = false;
            myPane.YAxis.MinorTic.IsOpposite = false;
            // Don't display the Y zero line
            myPane.YAxis.MajorGrid.IsZeroLine = false;
            myPane.YAxis.MajorGrid.IsVisible = true;
            // Align the Y axis labels so they are flush to the axis
            myPane.YAxis.Scale.Align = AlignP.Inside;
            // Manually set the axis range
            myPane.YAxis.Scale.Min = min_val;
            myPane.YAxis.Scale.Max = max_val;
            myPane.XAxis.Scale.Min = min_x;
            myPane.XAxis.Scale.Max = max_x;

            // Enable scrollbars if needed
            zg1.IsShowHScrollBar = true;
            zg1.IsShowVScrollBar = true;
            zg1.IsAutoScrollRange = true;
            //zg1.IsScrollY2 = true;

            zg1.AxisChange();
            zg1.Invalidate();
            Application.DoEvents();
        }

        public void Plot_RS_display()
        {
            double min_val = 0;
            double max_val = 16;


            GraphPane myPane = zg1.GraphPane;

            if (chk_persist.Checked)
            { myPane.Legend.IsVisible = false; }
            else
            {
                myPane.CurveList.Clear();
                myPane.Legend.IsVisible = false;
            }

            TB4.myRun.flgNewDataAvailable = false;
            TB4.myRun.flgResquestData = true;
            Application.DoEvents();
            while (!TB4.myRun.flgNewDataAvailable)
            {
                Thread.Sleep(50);
                Application.DoEvents();
            }
            TB4.myRun.flgResquestData = false;
            TB4.myRun.flgNewDataAvailable = false;
            string s = TB4.myRun.UDPdata_for_parser;
            // Set the titles and axis labels
            //myPane.Title.Text = "ADC data";
            //myPane.XAxis.Title.Text = "sample #";
            //myPane.YAxis.Title.Text = "ADC  count";
            double min_x = (double)ud_MinX.Value;
            double max_x = (double)ud_MaxX.Value;

            Parser.ParseInputLine(s, out ChData);
            int[] max = new int[32];
            for (int i = 0; i < 32; i++)
            {
                max[i] = 0;
                for (int j = 0; j < 15; j++)
                {
                    if (max[i] < ChData[i, j]) { max[i] = ChData[i, j]; }
                }
                ChMaxBuf[i, BufPointer] = max[i];
            }

            BufPointer++;
            if (BufPointer > ChMaxBuf.GetLength(1)) { BufPointer = 0; }

            PointPairList list = new PointPairList();
            double[] x = new double[32];
            double[] y = new double[32];

            max_val = 0;
            for (int j = 0; j < 32; j++)
            {
                x[j] = j;
                y[j] = 0;
                for (int jj = 0; jj < ChMaxBuf.GetLength(1); jj++)
                {
                    y[j] += ChMaxBuf[j, jj];
                }
                if ((y[j] * 1.2) > max_val) { max_val = 1.2 * y[j]; }
            }


            list = new PointPairList(x, y);
            LineItem myCurve = myPane.AddCurve(" ", list, Color.Peru, SymbolType.Circle);

            //max_val = (double)ud_MaxY.Value;
            min_val = (double)ud_MinY.Value;
            // Show the x axis grid
            myPane.XAxis.MajorGrid.IsVisible = true;
            // Make the Y axis scale red
            //myPane.YAxis.Scale.FontSpec.FontColor = Color.Red;
            //myPane.YAxis.Title.FontSpec.FontColor = Color.Red;
            // turn off the opposite tics so the Y tics don't show up on the Y2 axis
            myPane.YAxis.MajorTic.IsOpposite = false;
            myPane.YAxis.MinorTic.IsOpposite = false;
            // Don't display the Y zero line
            myPane.YAxis.MajorGrid.IsZeroLine = false;
            myPane.YAxis.MajorGrid.IsVisible = true;
            // Align the Y axis labels so they are flush to the axis
            myPane.YAxis.Scale.Align = AlignP.Inside;
            // Manually set the axis range
            myPane.YAxis.Scale.Min = min_val;
            myPane.YAxis.Scale.Max = max_val;
            myPane.XAxis.Scale.Min = min_x;
            myPane.XAxis.Scale.Max = max_x;

            // Enable scrollbars if needed
            zg1.IsShowHScrollBar = true;
            zg1.IsShowVScrollBar = true;
            zg1.IsAutoScrollRange = true;
            //zg1.IsScrollY2 = true;

            zg1.AxisChange();
            zg1.Invalidate();
        }

        private void MyZoomEvent(ZedGraphControl sender, ZoomState oldState, ZoomState newState)
        {
        }

        private void btn_PLOT_Click(object sender, EventArgs e)
        {
            if (TB4.myRun.flgRun_pause)
            {
                btn_PLOT.Text = "PAUSE";
                TB4.myRun.flgRun_pause = false;
                Thread.Sleep(1);
            }
            else
            {
                btn_PLOT.Text = "CONT";
                TB4.myRun.flgRun_pause = true;
            }
        }

        public void btn_UDP_Click(object sender, EventArgs e)
        {
            //if (sender.ToString() == "report progress")
            //{ }
            //else
            //{ TB4.myRun.Take_UDP_Event(); }


            //int[] data0 = new int[8095];
            //int[] data1 = new int[8095];
            //int[] data2 = new int[8095];
            //int[] data3 = new int[8095];
            //bool[] data_assigned = new bool[4];

            //for (int j = 0; j < 4; j++)
            //{
            //    if (cmb_Trace_arr[j].SelectedIndex > 0)
            //    {
            //        data_assigned[j] = true;
            //        int mych = cmb_Trace_arr[j].SelectedIndex - 1;
            //        if (mych * TB4.myRun.event_data_length > TB4.myRun.event_data.Length)
            //        {
            //            mych = 0;
            //            data_assigned[j] = false;
            //        }

            //        switch (j)
            //        {
            //            case 0:
            //                Array.Copy(TB4.myRun.event_data, mych * TB4.myRun.event_data_length, data0, 0, TB4.myRun.event_data_length);
            //                break;
            //            case 1:
            //                Array.Copy(TB4.myRun.event_data, mych * TB4.myRun.event_data_length, data1, 0, TB4.myRun.event_data_length);
            //                break;
            //            case 2:
            //                Array.Copy(TB4.myRun.event_data, mych * TB4.myRun.event_data_length, data2, 0, TB4.myRun.event_data_length);
            //                break;
            //            case 3:
            //                Array.Copy(TB4.myRun.event_data, mych * TB4.myRun.event_data_length, data3, 0, TB4.myRun.event_data_length);
            //                break;
            //        }
            //    }
            //    else { data_assigned[j] = false; }
            //}
            //Plot0_display(data0, data1, data2, data3);
        }

        private void chk_ExternalTrig_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_ExternalTrig.Checked) { chkCosmicTrig.Checked = false; }
        }

        private void chkCosmicTrig_CheckedChanged(object sender, EventArgs e)
        {
            if (chkCosmicTrig.Checked)
            {
                chk_ExternalTrig.Checked = false;
                chk_AND_Ch0.Visible = true;
                chk_AND_Ch1.Visible = true;
                chk_AND_Ch2.Visible = true;
                chk_AND_Ch3.Visible = true;
            }
            else
            {
                chk_AND_Ch0.Visible = false;
                chk_AND_Ch1.Visible = false;
                chk_AND_Ch2.Visible = false;
                chk_AND_Ch3.Visible = false;
            }

        }

        public void EnableChanSelection(bool enable_chan_selection)
        {
            if (enable_chan_selection)
            {
                //this.cmb_Trace0.Items.Clear();
                //this.cmb_Trace0.Items.AddRange(new object[] { "none", "CH0", "CH1", "CH2", "CH3", "CH4", "CH5", "CH6", "CH7", "CH8", "CH9", "CH10", "CH11", "CH12", "CH13", "CH14", "CH15" });
                //this.cmb_Trace1.Items.Clear();
                //this.cmb_Trace1.Items.AddRange(new object[] { "none", "CH0", "CH1", "CH2", "CH3", "CH4", "CH5", "CH6", "CH7", "CH8", "CH9", "CH10", "CH11", "CH12", "CH13", "CH14", "CH15" });
                //this.cmb_Trace2.Items.Clear();
                //this.cmb_Trace2.Items.AddRange(new object[] { "none", "CH0", "CH1", "CH2", "CH3", "CH4", "CH5", "CH6", "CH7", "CH8", "CH9", "CH10", "CH11", "CH12", "CH13", "CH14", "CH15" });
                //this.cmb_Trace3.Items.Clear();
                //this.cmb_Trace3.Items.AddRange(new object[] { "none", "CH0", "CH1", "CH2", "CH3", "CH4", "CH5", "CH6", "CH7", "CH8", "CH9", "CH10", "CH11", "CH12", "CH13", "CH14", "CH15" });
            }
            else
            {
                //this.cmb_Trace0.Items.Clear();
                //this.cmb_Trace0.Items.AddRange(new object[] { "none", "CH0" });
                //this.cmb_Trace1.Items.Clear();
                //this.cmb_Trace1.Items.AddRange(new object[] { "none", "CH1" });
                //this.cmb_Trace2.Items.Clear();
                //this.cmb_Trace2.Items.AddRange(new object[] { "none", "CH2" });
                //this.cmb_Trace3.Items.Clear();
                //this.cmb_Trace3.Items.AddRange(new object[] { "none", "CH3" });
            }
        }

        private void ud_MaxX_ValueChanged(object sender, EventArgs e)
        {

        }

        private void lbl_TR0_Click(object sender, EventArgs e)
        {

        }

        private void cmb_Trace0_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Plot0_Load(object sender, EventArgs e)
        {
            TB4.thePlot = this;
        }
        void Plot0_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            TB4.thePADE_explorer.childClosed(this);
            e.Cancel = true;
        }
        //public void Plot0_Resize(object sender, EventArgs e)
        //{
        //    SetSize();
        //}

        //private void SetSize()
        //{
        //    zg1.Location = new Point(10, 10);
        //    // Leave a small margin around the outside of the control
        //    zg1.Size = new Size(this.ClientRectangle.Width - 20,
        //            this.ClientRectangle.Height - 185);
        //}
        

        private void scanDataPush(string dataPathname = null)
        {

            TB4_Register adcCheckReg = new TB4_Register("ADC_CHECK_REG", "Checks the amount of data collected by the PADE waiting to be read.", 0x04000000, 16, false, false);
            int[] rdata = new int[1200];

            bool writeToDisc = false;
            if (dataPathname != null) writeToDisc = true;

            double timestamp = 0;
            System.IO.StreamWriter writer = null;

            try
            {
                writer = new System.IO.StreamWriter(dataPathname);
            }
            catch (Exception Ex)
            {
                TB4_Exception.logError(Ex, "Failed to open streamwriter.", true);
                writeToDisc = false;
            }



                //software reset
                PADE_explorer.registerLookup("SOFTWARE_RESET").RegWrite(1);

                //send software trigger

                TB4.myRun.runSoftTrig.RegWrite(1);

                string[] preappend=new string[4];
                int[] value=new int[5];
                string DH="";
                int counter=0;
                ZedGraph.GraphPane mainPane=zg1.GraphPane;

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
                        timestamp = rdata[i * 20] + rdata[i * 20 + 1] << 8 + rdata[i * 20 + 2]<<16; //get timestamp

                        if (writeToDisc) writer.Write(timestamp + ": ");

                        for (int j = 0; j < 4; j++) //iterate through data points
                        {
                            //construct parts of each word
                            for (int k = 0; k < 4; k++) preappend[k] = Convert.ToString((rdata[i * 20 + j * 4 + k + 4] >> 7) * 4 * (rdata[i * 20 + j * 4 + k + 4] & 128) + (rdata[i * 20 + j * 4 + k + 4] >> 7) * 128 + (rdata[i * 20 + j * 4 + k + 4] & 128) * (1 - (rdata[i * 20 + j * 4 + k + 4] >> 7)));
                            //construct the word from the parts
                            for (int k = 0; k < 4; k++) DH += preappend[k];
                            value[j] = Convert.ToInt16(DH);
                            DH = "";

                            //now plot the data
                            ((IPointListEdit) mainPane.CurveList[i].Points).Add(timestamp + 0.001 * j, value[j]);

                            if (writeToDisc) writer.Write(value[j] + "  ");
                            counter++;
                        }
                        if (writeToDisc) writer.Write('\n');

                        //redraw the plot: if speed is more important than refresh rate, put this outside the block for loop
                        zg1.Invalidate();
                    }


                }
            
            if (writeToDisc)
            {
                //close up shop
                writer.WriteLine("End of data.");
                writer.Close();
            }


            }
            

        }
    }



        
