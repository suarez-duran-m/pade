using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using ZedGraph;
using WeifenLuo.WinFormsUI.Docking;

namespace PADE
{
    public partial class Hist1 : DockContent
    {

        public class savedHistogram
        {
            public string histogramName = "";
            public List<PointPairList> channelList = new List<PointPairList>(); //one pointpairlist per channel
            public Boolean isShown = false;
            public savedHistogram(string newName)
            {
                histogramName = newName;
            }

        }
        List<savedHistogram> histogramList = new List<savedHistogram>();
        //list of lists of point pair list. each list of point pair lists contains all channels for a given histogram.

        public Messages theMessagesWindow;
        public TB4_Register ped_reg;
        public bool[] UpdatedPed_flag = new bool[32];
        public UInt16[] UpdatedPed = new UInt16[32];
        public CheckBox[] hist_chk_chan = new CheckBox[32];
        public Button btn_all_off_A = new Button();
        public Button btn_all_off_B = new Button();
        public Button btn_all_off_C = new Button();
        public Button btn_all_off_D = new Button();
        public Button btn_all_on_A = new Button();
        public Button btn_all_on_B = new Button();
        public Button btn_all_on_C = new Button();
        public Button btn_all_on_D = new Button();
        private TB4_Register[] Thresh = new TB4_Register[4];
        public  TB4_Register[] Offset_reg = new TB4_Register[32];
        public TB4_Register IMON_reg = new TB4_Register("IMON", "", 0, 0, false, false);
        private Int32[] offset = new Int32[32];
        private int[] OnePE_pos = new Int32[32];
        private int OnePE_Goal = 0;
        static double[] h = new double[2048];
        static double[] h_unscaled = new double[2048];
        static double[] x = new double[2048];
        List<uint> tt = new List<uint>();
        List<tCnt> hh = new List<tCnt>();
        private bool flgPE;
        private bool flgTuner_active;
        public bool flg_StopIV;
        private bool flg_CancelScan;
        //**********
        TB4_PerfMon scanMon;
        //**********

        public Color[] arr_Curve_Col = new Color[32];


        //        public string[] cmb_Start0_text = new string[8];
        //        public ComboBox[] cmb_Trace_arr = new ComboBox[4];

        public Hist1()
        {
            InitializeComponent();
             scanMon= new TB4_PerfMon("Histogram Scan Monitor", "Counts duration of \"for\"-iterations in the SCAN() function");

            for (int i = 0; i < 32; i++)
            {
                UpdatedPed[i] = 0x840;
                UpdatedPed_flag[i] = false;
            }

            for (int i = 0; i < 8; i++)
            {
                arr_Curve_Col[i] = Color.FromArgb((i + 1) * 32 - 1, 0, 0);
                arr_Curve_Col[i + 8] = Color.FromArgb(0, (i + 1) * 32 - 1, 0);
                arr_Curve_Col[i + 16] = Color.FromArgb(0, 0, (i + 1) * 32 - 1);
                arr_Curve_Col[i + 24] = Color.FromArgb(0, (i + 1) * 32 - 1, (i + 1) * 32 - 1);
            }

            for (int i = 0; i < 32; i++)
            {
                this.hist_chk_chan[i] = new CheckBox();
                this.hist_chk_chan[i].AutoSize = true;
                this.hist_chk_chan[i].Enabled = true;
                this.hist_chk_chan[i].Location = new System.Drawing.Point(5 + (60 * (i & 0x18) >> 3), 425 + (20 * (i & 0x7)));
                this.hist_chk_chan[i].Name = "checkBox" + i.ToString();
                this.hist_chk_chan[i].Size = new System.Drawing.Size(51, 17);
                this.hist_chk_chan[i].UseVisualStyleBackColor = true;
                this.hist_chk_chan[i].Visible = true;
                this.hist_chk_chan[i].Text = "Ch" + i.ToString();
                this.hist_chk_chan[i].CheckedChanged += new EventHandler(Hist1_CheckedChanged);
                this.hist_chk_chan[i].Tag = i;
                this.Controls.Add(hist_chk_chan[i]);
            }
            btn_all_off_A = new Button();
            btn_all_off_A.Enabled = true;
            btn_all_off_A.Text = "A off";
            btn_all_off_A.Size = new System.Drawing.Size(51, 17);
            btn_all_off_A.Location = new System.Drawing.Point(5, 425 + 160);
            btn_all_off_A.Click += new EventHandler(btn_all_off_A_Click);
            this.Controls.Add(btn_all_off_A);

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

            btn_all_on_A = new Button();
            btn_all_on_A.Enabled = true;
            btn_all_on_A.Text = "A on";
            btn_all_on_A.Size = new System.Drawing.Size(51, 17);
            btn_all_on_A.Location = new System.Drawing.Point(5, 445 + 160);
            btn_all_on_A.Click += new EventHandler(btn_all_on_A_Click);
            this.Controls.Add(btn_all_on_A);

            btn_all_on_B = new Button();
            btn_all_on_B.Enabled = true;
            btn_all_on_B.Text = "B on";
            btn_all_on_B.Size = new System.Drawing.Size(51, 17);
            btn_all_on_B.Location = new System.Drawing.Point(5 + 60, 445 + 160);
            btn_all_on_B.Click += new EventHandler(btn_all_on_B_Click);
            this.Controls.Add(btn_all_on_B);

            btn_all_on_C = new Button();
            btn_all_on_C.Enabled = true;
            btn_all_on_C.Text = "C on";
            btn_all_on_C.Size = new System.Drawing.Size(51, 17);
            btn_all_on_C.Location = new System.Drawing.Point(5 + 120, 445 + 160);
            btn_all_on_C.Click += new EventHandler(btn_all_on_C_Click);
            this.Controls.Add(btn_all_on_C);

            btn_all_on_D = new Button();
            btn_all_on_D.Enabled = true;
            btn_all_on_D.Text = "D on";
            btn_all_on_D.Size = new System.Drawing.Size(51, 17);
            btn_all_on_D.Location = new System.Drawing.Point(5 + 180, 445 + 160);
            btn_all_on_D.Click += new EventHandler(btn_all_on_D_Click);
            this.Controls.Add(btn_all_on_D);

            flgPE = false;
            chk_PEfinder.Checked = false;

            for (int j = 1; j < TB4.TB4_Registers.Length - 1; j++)
            {
                if (TB4.TB4_Registers[j] == null)
                {
                    //then nothing
                }
                else
                {
                    if (TB4.TB4_Registers[j].name.Contains("BIAS_OFFSET_CH"))
                    {
                        string t = TB4.TB4_Registers[j].name;
                        string it = t.Substring(14, t.Length - 14);
                        Offset_reg[Convert.ToInt32(it)] = TB4.TB4_Registers[j];
                    }
                    if (TB4.TB4_Registers[j].name.Contains("BIAS_IMON"))
                    {
                        IMON_reg = TB4.TB4_Registers[j];
                    }
                }
            }
            comboBox1.MouseClick += new MouseEventHandler(comboBox1_MouseClick);
        }

        
        #region buttons

        void comboBox1_MouseClick(object sender, MouseEventArgs e)
        {
            comboBox1.Items.Clear();
            foreach (savedHistogram hist in histogramList)
            {
                comboBox1.Items.Add(hist.histogramName);
            }
        }

        void Hist1_CheckedChanged(object sender, EventArgs e)
        {
            Hist1_update();
            CheckBox foo = (CheckBox)sender;
            int i = (int)foo.Tag;
            udChan.Value = i;
        }

        void btn_all_off_A_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 8; i++)
            {
                hist_chk_chan[i].Checked = false;
            }
            Thread.Sleep(0);
        }
        void btn_all_off_B_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 8; i++)
            {
                hist_chk_chan[i + 8].Checked = false;
            }
            Thread.Sleep(0);
        }
        void btn_all_off_C_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 8; i++)
            {
                hist_chk_chan[i + 16].Checked = false;
            }
            Thread.Sleep(0);
        }
        void btn_all_off_D_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 8; i++)
            {
                hist_chk_chan[i + 24].Checked = false;
            }
            Thread.Sleep(0);
        }
        void btn_all_on_A_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 8; i++)
            {
                hist_chk_chan[i].Checked = true;
            }
            Thread.Sleep(0);
        }
        void btn_all_on_B_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 8; i++)
            {
                hist_chk_chan[i + 8].Checked = true;
            }
            Thread.Sleep(0);
        }
        void btn_all_on_C_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 8; i++)
            {
                hist_chk_chan[i + 16].Checked = true;
            }
            Thread.Sleep(0);
        }
        void btn_all_on_D_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 8; i++)
            {
                hist_chk_chan[i + 24].Checked = true;
            }
            Thread.Sleep(0);
        }

        #endregion buttons

        public void Hist1_clear()
        {
            tt.Clear();
            hh.Clear();
            for (int i = 0; i < h.Length; i++)
            {
                h[i] = 0; x[i] = 0;
            }
        }

        public void Hist1_display()
        {
            // Get a reference to the GraphPane instance in the ZedGraphControl
            GraphPane myPane = zg1.GraphPane;
            myPane.CurveList.Clear();

            // Set the titles and axis labels
            myPane.Title.Text = "histo";
            myPane.XAxis.Title.Text = "ADC";
            myPane.YAxis.Title.Text = "#";
            myPane.CurveList.Clear();
            myPane.YAxis.Scale.Max = 66000;
            myPane.CurveList.Clear();


            // Show the x axis grid
            myPane.XAxis.MajorGrid.IsVisible = true;
            myPane.XAxis.MajorTic.ScaledTic(10);
            //myPane.XAxis.Scale.
            myPane.XAxis.Scale.MaxAuto = false;
            myPane.YAxis.Title.IsVisible = false;
            myPane.Legend.IsVisible = false;
            // Make the Y axis scale red
            myPane.YAxis.Scale.FontSpec.FontColor = Color.Red;
            myPane.YAxis.Title.FontSpec.FontColor = Color.Red;
            // turn off the opposite tics so the Y tics don't show up on the Y2 axis
            myPane.YAxis.MajorTic.IsOpposite = false;
            myPane.YAxis.MinorTic.IsOpposite = false;
            // Don't display the Y zero line
            myPane.YAxis.MajorGrid.IsZeroLine = true;
            // Align the Y axis labels so they are flush to the axis
            myPane.YAxis.Scale.Align = AlignP.Inside;
            // Manually set the axis range
            myPane.YAxis.Scale.Min = 0;
            myPane.YAxis.Scale.MaxAuto = true;
            //myPane.YAxis.Scale.Max = max_val;

            // Enable scrollbars if needed
            zg1.IsShowHScrollBar = true;
            zg1.IsShowVScrollBar = true;
            zg1.IsAutoScrollRange = true;

            //// OPTIONAL: Show tooltips when the mouse hovers over a point
            //zg1.IsShowPointValues = true;
            //zg1.PointValueEvent += new ZedGraphControl.PointValueHandler(MyPointValueHandler);

            //// OPTIONAL: Add a custom context menu item
            //zg1.ContextMenuBuilder += new ZedGraphControl.ContextMenuBuilderEventHandler(
            //        MyContextMenuBuilder);

            // OPTIONAL: Handle the Zoom Event
            //      zg1.ZoomEvent += new ZedGraphControl.ZoomEventHandler(MyZoomEvent);

            myPane.XAxis.Scale.MaxAuto = false;
            myPane.XAxis.Scale.MinAuto = false;

            myPane.XAxis.Scale.Max = 1.10 * Convert.ToDouble(udStop.Value);
            myPane.XAxis.Scale.Min = .9 * Convert.ToDouble(udStart.Value);
            //myPane.YAxis.Scale.Max = Convert.ToDouble(ud_MaxY.Value);
            //myPane.YAxis.Scale.Min = Convert.ToDouble(ud_MinY.Value);

            // Size the control to fit the window
            //      SetSize();

            // Tell ZedGraph to calculate the axis ranges
            // Note that you MUST call this after enabling IsAutoScrollRange, since AxisChange() sets
            // up the proper scrolling parameters
            zg1.AxisChange();
            // Make sure the Graph gets redrawn
            zg1.Invalidate();
        }

        void Hist1_update()
        {
            GraphPane myPane = zg1.GraphPane;
            myPane.CurveList.Clear();
            myPane.Title.Text = "histo";
            myPane.XAxis.Title.Text = "ADC";
            myPane.YAxis.Title.Text = "#";
            myPane.XAxis.MajorGrid.IsVisible = true;
            myPane.XAxis.MajorTic.ScaledTic(10);

            tCnt cn = new tCnt();
            int i = 0;
            foreach (uint th in tt)
            {
                x[i + 1] = Convert.ToDouble(th);
                i++;
            }
            i = 0;
            for (int ind = 0; ind < 32; ind++)
            {
                //for each channel
                if (hist_chk_chan[ind].Checked)
                {
                    tCnt[] arr_hi = new tCnt[hh.Count];
                    arr_hi = hh.ToArray();
                    for (i = 0; i < hh.Count; i++)
                    {
                        //for each sample
                        cn = arr_hi[i];


                        if (this.chkLogY.Checked)
                        {
                            if (cn.cnt[ind] > 0) //data of chanhnel {ind}
                            { h[i] = Math.Log(cn.cnt[ind]); h_unscaled[i] = cn.cnt[ind]; }
                            else
                            { h[i] = 0; }
                        }
                        else
                        { h[i] = cn.cnt[ind]; }
                    }
                    //for (i = 0; i < hh.Count - 1; i++)
                    //{
                    //    double d = h[i] - h[i + 1];
                    //    if (d <= 0) { d = 0.1; }
                    //    if (this.chkLogY.Checked)
                    //    { h[i] = Math.Log(d,2); }
                    //    else
                    //    { h[i] = d; }
                    //}
                    myPane.AddCurve("", x, h, arr_Curve_Col[ind], SymbolType.None);

                    if (flgPE)
                    {
                        double PE_pos;
                        double PE_y;

                        find_1PE(x, h, ind, out PE_pos, out PE_y);
                        double[] stick_x = new double[2];
                        double[] stick_y = new double[2];
                        stick_x[0] = PE_pos; stick_x[1] = PE_pos;
                        stick_y[0] = myPane.YAxis.Scale.Min; stick_y[1] = PE_y;

                        myPane.AddStick("", stick_x, stick_y, arr_Curve_Col[ind]);
                    }
                }

            }
            //myPane.YAxis.Scale.MaxAuto = true;
            zg1.AxisChange();
            zg1.Invalidate();
        }

        public void Hist1_IV(double[] x, double[] y, Color IV_color)
        {
            GraphPane myPane = zg1.GraphPane;

            myPane.Title.Text = "I-V";
            myPane.XAxis.Title.Text = "V";
            myPane.YAxis.Title.Text = "I";
            myPane.XAxis.MajorGrid.IsVisible = true;
            myPane.YAxis.MajorGrid.IsVisible = true;

            myPane.AddCurve("", x, y, IV_color, SymbolType.None);

            myPane.YAxis.Scale.MaxAuto = true;
            myPane.YAxis.Scale.MinAuto = true;
            myPane.XAxis.Scale.MaxAuto = true;
            zg1.AxisChange();
            zg1.Invalidate();
        }

        private void btnAutoPeds_Click(object sender, EventArgs e)
        {
            string t;
            if (TB4.theRunForm.txtRun.Text == "")
            {
                TB4.myRun.Run_filename = "c:\\junk";
            }
            else
            {
                TB4.myRun.Run_filename = TB4.theRunForm.txtRun.Text;
            }

            if (TB4.theRunForm.txtRunNum.Text == "")
            {
                TB4.myRun.Run_filename += "_0";
            }
            else
            {
                TB4.myRun.Run_filename += "_" + TB4.theRunForm.txtRunNum.Text;
            }

            string myFileName = TB4.myRun.Run_filename;
            DateTime n = System.DateTime.Now;
            myFileName = "c:\\data\\Offsets_" + n.Year.ToString() + n.Month.ToString("00") + n.Day.ToString("00") + "_" + n.Hour.ToString("00") + n.Minute.ToString("00") + n.Second.ToString("00");
            myFileName += "_SN" + TB4.ActivePADE.IP4_add[0].ToString();
            myFileName += "PADE_" + TB4.active_PAD_index.ToString();
            myFileName += ".histo";
            StreamWriter newHisto = new StreamWriter(myFileName, false);
            int i = 0;
            foreach (uint th in tt)
            {
                x[i + 1] = Convert.ToDouble(th);
                i++;
            }
            tCnt[] arr_hi = new tCnt[hh.Count];
            arr_hi = hh.ToArray();
            for (i = 0; i < hh.Count; i++)
            {
                t = i.ToString();
                t += ", " + x[i].ToString();
                for (int ind = 0; ind < 32; ind++)
                {
                    t += ", " + arr_hi[i].cnt[ind].ToString();
                }

                newHisto.WriteLine(t);
            }

            string rn = TB4.theRunForm.txtRunNum.Text;
            UInt64 irn = 0;
            try { irn = Convert.ToUInt64(rn); }
            catch { irn = 0; }
            TB4.theRunForm.txtRunNum.Text = (irn + 1).ToString();
            newHisto.Close();
            //foreach (CurveItem c in zg_Histo.GraphPane.CurveList)
            //{
            //    for (int i = 0; i < c.NPts; i++)
            //    {
            //        string t=c.Points[i].Y.ToString();
            //        Console.WriteLine(t); 
            //    }
            //}
        }


        private void SCAN()
        {
            btnScan.Text = "...";
            Hist1_clear();

            int ind_thr_A = 0;
            int ind_thr_B = 0;
            int ind_thr_C = 0;
            int ind_thr_D = 0;
            int ind_CSR = 0;
            int ind_THR0 = 0;
            UInt16 csr = 0;
            TB4.TB4_Registers_Dict.TryGetValue("TRIG_THRESHOLD_CH0-7", out ind_thr_A);
            TB4.TB4_Registers_Dict.TryGetValue("TRIG_THRESHOLD_CH8-15", out ind_thr_B);
            TB4.TB4_Registers_Dict.TryGetValue("TRIG_THRESHOLD_CH16-23", out ind_thr_C);
            TB4.TB4_Registers_Dict.TryGetValue("TRIG_THRESHOLD_CH24-31", out ind_thr_D);
            TB4_Register thresh_reg = PADE_explorer.registerLookup("THRESHOLD_SCAN_VAL");
            
            TB4.TB4_Registers_Dict.TryGetValue("CSR", out ind_CSR);
            TB4.TB4_Registers_Dict.TryGetValue("THR_00", out ind_THR0);
            uint min = Convert.ToUInt16(udStart.Value);
            uint max = Convert.ToUInt16(udStop.Value + 1);
            uint inc = Convert.ToUInt16(udInc.Value);

            //if (chkIntegral.Checked)
            //{ csr = 2; TB4.TB4_Registers[ind_CSR].RegWrite(csr); }
            //else
            //{ csr = 0; TB4.TB4_Registers[ind_CSR].RegWrite(csr); }

            for (uint thr = min; thr < max + 1; thr = thr + inc)
            {
                scanMon.startTime();

                btnScan.Text = thr.ToString();

                tt.Add(thr);
                tCnt c = new tCnt();
                ushort v = Convert.ToUInt16(thr);
                mySetThreshold(v);

                mySendSoftReset();

                Application.DoEvents();
                Thread.Sleep(1);


                bool flgScanDone = false;
                
                while (!flgScanDone)
                {
                    Thread.Sleep(1);
                    flgScanDone = myCheckForScanDone();
                }

                byte A3 = 0x01;
                byte A2 = 0x10;
                byte A1 = 0x00;
                byte A0 = 0x00;
                int[] temp_arr = new int[100];
                TB4.ReadArray(A3, A2, A1, A0, 64, temp_arr);
                for (int i = 0; i < 32; i++)
                { c.cnt[i] = temp_arr[2 * i + 1] * 0x10000 + temp_arr[2 * i]; }
                //for (int i=8; i<16;i++)
                //{ c.cnt[i + 8] = c.cnt[i]; c.cnt[i] = 0; }
                hh.Add(c);
                scanMon.stopTime(true);
            }
        }

        private void btnScan_Click_1(object sender, EventArgs e)
        {
            int offset_step = 50;
            try
            {
                offset_step = Convert.ToInt32(txtTOL.Text);
                if (offset_step > 200) { offset_step = 200; }
                if (offset_step < 10) { offset_step = 10; }
            }
            catch { }
            
            mySetDataTakingMode(1);
            SCAN();
            Hist1_display();
            Hist1_update();
            if (chk_Tuner.Checked)
            {
                OnePE_Goal = (int)Math.Round(Convert.ToDecimal(txtGoal.Text));
                if (OnePE_Goal > 0)
                {
                    flgTuner_active = true; //this is so we run through at least once
                    while (flgTuner_active)
                    {
                        btnCancelScan.Visible = true;

                        flgTuner_active = false; //will quit if noone is above the goal
                        if (flgPE != true)
                        {
                            flgPE = true;
                            Hist1_update();
                        }
                        for (int i = 0; i < 32; i++)
                        {
                            if (hist_chk_chan[i].Checked)
                            {
                                if (OnePE_pos[i] > OnePE_Goal)
                                {
                                    if (offset[i] < 1000)
                                    {
                                        flgTuner_active = true;
                                        offset[i] += offset_step;
                                        ushort t = Convert.ToUInt16(offset[i]);
                                        Thread.Sleep(5);
                                        Offset_reg[i].RegWrite(t);
                                        Application.DoEvents();
                                    }
                                }
                            }
                        }
                        if (flgTuner_active)
                        {
                            if (flg_CancelScan) { flgTuner_active = false; flg_CancelScan = false; }

                            SCAN();
                            Application.DoEvents();
                            Hist1_update();
                        }
                    }
                    btnCancelScan.Visible = false;
                    chk_Tuner.Checked = false;
                    Application.DoEvents();
                }
            }
            btnScan.Text = "SCAN";
            mySetDataTakingMode(0);
        }


        private void find_1PE(double[] x, double[] y, int ind, out double PE_pos, out double PE_y)
        {
            //IMPORTANT NOTE: this code actually uses the graphs displayed in the HISTO_1 to do the finding of the PEs so be careful!           

            int imin_pos = 0;
            int imax_pos = 0;

            int smooth_num = 10;
            double max = 0;


            double s, s1, s2;

            //the idea here is to find the last point where we are sure the slope is negative and then the fits point where we are sure the slope is positive
            // this is where the minimum is
            // then we find the next place where we are sure the slope is negative after being positive- whis is where the max must be.


            for (int i = 0; i < x.Length - (3 * smooth_num); i++)
            {
                if (x[i] > 100)
                {
                    s = 0; s1 = 0; s2 = 0;
                    for (int j = 0; j < smooth_num; j++)
                    {
                        s1 += y[i + j];
                        s += y[i + smooth_num + j];
                        s2 += y[i + 2 * smooth_num + j];
                    }
                    if ((s1 > s) && (s2 > s))
                    {
                        imin_pos = i + smooth_num;
                        i = x.Length;
                    }
                }
            }

            for (int i = imin_pos; i < x.Length - smooth_num; i++)
            {
                if (x[i] > 110)
                {
                    s = 0;
                    for (int j = 0; j < smooth_num; j++)
                    {
                        s += y[i + j];
                    }
                    if (s > max)
                    {
                        max = s;
                        imax_pos = i + (int)Math.Round((decimal)(smooth_num) / 2);
                    }
                }
            }

            PE_pos = x[imax_pos];
            PE_y = y[imax_pos];
            OnePE_pos[ind] = (int)(Math.Round(PE_pos));
        }




        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            Hist1_display();
        }

        private void udChan_ValueChanged(object sender, EventArgs e)
        {
            //for (int j = 1; j < TB4.TB4_Registers.Length - 1; j++)
            //{
            //    if (TB4.TB4_Registers[j] == null)
            //    {
            //        //then nothing
            //    }
            //    else
            //    {
            //        if (TB4.TB4_Registers[j].name.Contains("PED_SUBTRACT_" + udChan.Value.ToString()))
            //        {
            //ped_reg = TB4.TB4_Registers[j];
            //txtPed.Text = (ped_reg.RegRead() - 2048).ToString();
            int i = (int)udChan.Value;
            txtOffset.Text = offset[i].ToString();
            //            break;
            //        }

            //    }
            //}
        }

        private void btn_PedW_Click(object sender, EventArgs e)
        {
            ushort t = 0;
            t = Convert.ToUInt16(2048);
            t += Convert.ToUInt16(txtPed.Text);
            ped_reg.RegWrite(t);
            int i = Convert.ToInt32(udChan.Value);
            UpdatedPed[i] = t;
            UpdatedPed_flag[i] = true;
        }

        private void btn_OffsetW_Click(object sender, EventArgs e)
        {
            ushort t = 0;
            t += Convert.ToUInt16(txtOffset.Text);
            int i = Convert.ToInt32(udChan.Value);
            Offset_reg[i].RegWrite(t);
            offset[i] = t;

            //    if (theMessagesWindow != null) { theMessagesWindow.richTextBox1.AppendText("test\n"); }

        }

        private void btn_ZeroOffset_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < Offset_reg.Length; i++)
            {
                Thread.Sleep(5);
                Offset_reg[i].RegWrite(0);
                offset[i] = 0;
            }
        }

        private void btnSavePeds_Click(object sender, EventArgs e)
        {
            string myFileName = "";

            DateTime n = System.DateTime.Now;
            myFileName = "c:\\data\\Offsets_" + n.Year.ToString() + n.Month.ToString("00") + n.Day.ToString("00") + "_" + n.Hour.ToString("00") + n.Minute.ToString("00") + n.Second.ToString("00");
            myFileName += "_SN" + TB4.ActivePADE.IP4_add[0].ToString();
            myFileName += ".TB4";
            StreamWriter newPeds = new StreamWriter(myFileName, false);

            TB4_Exception.logInfo("Changes made to offsets for PADE " + TB4.ActivePADE.PADE_sn +":", true);
            for (int i = 0; i < 32; i++)
            {
                //                if (UpdatedPed_flag[i])
                //                {
                //                    string t = "PED_SUBTRACT_" + i.ToString() + "<=";
                //                    t += "dec" + UpdatedPed[i].ToString();
                //                    newPeds.WriteLine(t);
                //                }
                string t = "BIAS_OFFSET_CH" + i.ToString() + "<=";
                t += "dec" + offset[i].ToString();
                newPeds.WriteLine(t);
                TB4_Exception.logInfo(t, false);
            }
            newPeds.Close();
        }

        private void chk_PEfinder_CheckedChanged(object sender, EventArgs e)
        {
            flgPE = chk_PEfinder.Checked;
            //if (flgPE)
            //{
            //    if (theMessagesWindow == null)
            //    {
            //        theMessagesWindow = new Messages();
            //        theMessagesWindow.Show();
            //    }
            //}
            //else
            //{
            //    theMessagesWindow = null;
            //}
        }

        private void btnAutoScale_Click(object sender, EventArgs e)
        {

        }

        private void btnIV_Click(object sender, EventArgs e)
        {
            double[] x = new double[100];
            double[] y = new double[100];
            double ave = 0;
            double ave0 = 0;

            ushort[] offsetBackup = new ushort[32];
            

            for (int i = 0; i < 32; i++)
            {
                //all offsets maxed out
                UInt16 t = 1000;
                offsetBackup[i] = Offset_reg[i].RegRead();
                Offset_reg[i].RegWrite(t);
                Thread.Sleep(1);
            }

            Hist1_display();
            Thread.Sleep(1);
            flg_StopIV = false;

            List<uint[]> tempX = new List<uint[]>();
            List<uint[]> tempY = new List<uint[]>();
            Hist1_clear();

            for (int k = 0; k < 32; k++) //channel iterator
            {

                tempX.Add(new uint[50]);
                tempY.Add(new uint[50]);
                if (hist_chk_chan[k].Checked)
                {
                    tCnt tempArray = new tCnt();
                    ave0 = 0;
                    for (int i = 0; i < 50; i++) //sample iterator
                    {
                        UInt16 t = Convert.ToUInt16(1000 - (i * 20));
                        Offset_reg[k].RegWrite(t);
                        Thread.Sleep(50);
                        ave = 0;
                        x[i] = 0; y[i] = 0;
                        int num_ave = 2;
                        {
                            for (int j = 0; j < num_ave; j++)
                            {
                                UInt16 m = IMON_reg.RegRead();
                                Thread.Sleep(1);
                                ave += m;
                            }

                            ave = ave / num_ave;
                            if (i == 2) { ave0 = ave; }
                            x[i] = i * 20;


                            //if ((ave - ave0) < 0) { ave = ave0 + 1; }
                            //y[i] = Math.Log( ave - ave0,2);
                            //if (i > 2) { y[i] = (ave - ave0); } else { y[i] = 0; }
                            if (i > 2) { y[i] = ave; } else { y[i] = 0; }
                            tempX[k][i] = Convert.ToUInt16(x[i]); tempY[k][i] = Convert.ToUInt16(y[i]);




                            btnScan.Text = i.ToString();
                        }
                        Application.DoEvents();
                        for (int ii = i; ii < 100; ii++) { x[ii] = x[i]; y[ii] = y[i]; }
                    }

                }
                
                Hist1_IV(x, y, arr_Curve_Col[k]);
                btnScan.Text = "SCAN";
                Offset_reg[k].RegWrite(1000);
                
                Thread.Sleep(50);
                
            }

            //We have to move data around to support checking and unchecking channel preservation
            for (int i = 0; i < tempX[0].Length; i++)
            {
                tt.Add(tempX[0][i]);
            }
            for (int i = 0; i < tempY[0].Length; i++)
            {
                tCnt temptCnt = new tCnt();
                for (int j = 0; j < tempY.Count; j++)
                {
                    temptCnt.cnt[j] = (int) tempY[j][i];
                }
                hh.Add(temptCnt);
            }
            //*********************************************************************************
            for (int i = 0; i < 32; i++) //return registers to original values
            {
                Offset_reg[i].RegWrite(offsetBackup[i]);
                Thread.Sleep(1);
            }

        }

        private void btnFixPeds_Click(object sender, EventArgs e)
        {
            TB4_Register regAutoPedSet = new TB4_Register("temp", "", 0, 16, false, false);

            for (int i = 0; i < TB4.TB4_Registers.Length; i++)
            {
                if (TB4.TB4_Registers[i] != null)
                {
                    if (TB4.TB4_Registers[i].name == "AUTO_PED_SET")
                    {
                        regAutoPedSet.addr = TB4.TB4_Registers[i].addr;
                        regAutoPedSet.RegWrite(0x64);
                    }
                }
            }

            Thread.Sleep(1000);
            for (int i = 0; i < TB4.TB4_Registers.Length; i++)
            {
                if (TB4.TB4_Registers[i] != null)
                {
                    if (TB4.TB4_Registers[i].name == "AUTO_PED_SET")
                    {
                        regAutoPedSet.addr = TB4.TB4_Registers[i].addr;
                        regAutoPedSet.RegWrite(0x0);
                    }
                }
            }

        }

        private void btnCancelScan_Click(object sender, EventArgs e)
        {
            flg_CancelScan = true;
        }

        private void btnReadOffsets_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 32; i++)
            {
                offset[i] = Convert.ToInt32(Offset_reg[i].RegRead());
                Application.DoEvents();
            }
        }

        private void Hist1_Load(object sender, EventArgs e)
        {
            TB4.theHist1 = this;
            Size returnSize = this.Size;
            this.DockStateChanged += (object a, System.EventArgs b) => { TB4.thePADE_explorer.childChangedDockstate(this, returnSize); };
        }
        void Hist1_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            TB4.thePADE_explorer.childClosed(this);
            e.Cancel = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PADE_explorer.saveGraphImage(zg1);
        }
        public void mySendSoftReset()
        {
            TB4_Register x = PADE_explorer.registerLookup("SOFTWARE_RESET");
            x.RegWrite(0);
        }
        public void mySetThreshold(ushort v)
        {
            TB4_Register x = PADE_explorer.registerLookup("THRESHOLD_SCAN_VAL");
            x.RegWrite(v);
        }

        public bool myCheckForScanDone()
        {
            TB4_Register x = PADE_explorer.registerLookup("STATUS_REG");
            ushort v = x.RegRead();
            Thread.Sleep(1);
            return true;
            //if ((v & 8) == 8) { return true; }
            //else { return false; }
        }

        public void mySetDataTakingMode(byte mode)
        {
            ushort csr = (ushort)((mode & 0x0f)*0x1000);
            TB4_Register x = PADE_explorer.registerLookup("CONTROL_REG");
            
            x.RegWrite(csr);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Convert the local point arrays into a list of pointpairlists
            savedHistogram savedHist=new savedHistogram(DateTime.Now.ToShortDateString());
            //populate all x points for all channels in the saved histogram
            for (int i = 0; i < 32; i++) //per channel
            {
                PointPairList tempList=new PointPairList();
                for (int j = 0; j < tt.Count; j++) //per x point
                {
                    if(hist_chk_chan[i].Checked) tempList.Add(tt[i], 0);
                    else tempList.Add(0,0);
                }
                savedHist.channelList.Add(tempList);
            }
            //populate all y points
            for (int i = 0; i < hh.Count; i++) //iterate through all x points
            {
                for (int j = 0; j < 32; j++) //iterate through channels (aka pointpairlists)
                {
                    if(hist_chk_chan[j].Checked) savedHist.channelList[j][i].Y = hh[i].cnt[j];
                }
            }
            histogramList.Add(savedHist);
            //*********************************************************************************

            
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            savedHistogram selectedHist=null;
            foreach (savedHistogram hist in histogramList)
            {
                if (hist.histogramName == comboBox1.Text) selectedHist = hist;
            }
            if (selectedHist != null)
            {
                if (selectedHist.isShown) button3.Text = "Hide";
                else button3.Text = "Show";
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            savedHistogram selectedHist=null;
            int i;
            for(i=0; i<histogramList.Count; i++)
            {
                if(histogramList[i].histogramName==comboBox1.Text) break;
            }

            if (button3.Text == "Show")
            {
                for (int j = 0; j < 32; j++)
                {
                   // zg1.GraphPane.AddCurve(selectedHist.histogramName, selectedHist.channelList[j], arr_Curve_Col[j]);
                }
                histogramList[i].isShown=true;
            }
            else
            {
                //remove the curve
                int j;
                for(j=0; j<zg1.GraphPane.CurveList.Count; j++)
                {
                    if(zg1.GraphPane.CurveList[j].Label.Text==histogramList[i].histogramName)
                    {
                        zg1.GraphPane.CurveList.RemoveAt(j);
                        break;
                    }
                }
                if(j==zg1.GraphPane.CurveList.Count)
                {
                    MessageBox.Show("Couldn't find the curvelist...fix me.");
                    return;
                }
                histogramList[i].isShown=false;
            }
            zg1.Invalidate();
    }

        private void button4_Click(object sender, EventArgs e)
        {
            //save all the saved Histograms
            SaveFileDialog sDialog = new SaveFileDialog();
            sDialog.Filter = "Text File (*.txt) | *.txt";

            DialogResult result = sDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                StreamWriter sw=new StreamWriter(sDialog.FileName);

                foreach (savedHistogram savedHist in histogramList)
                {
                    sw.Write(savedHist.histogramName + ":");
                    int i = 0;
                    foreach (PointPairList channel in savedHist.channelList)
                    {
                        sw.Write("#CHAN" + i.ToString() + "#");
                        int j = 0;
                        foreach (PointPair pair in channel)
                        {
                            sw.Write("(" + pair.X + "," + pair.Y + ")");
                            if (j < channel.Count) sw.Write("_");
                        }
                    }
                    sw.Write("\n");
                }
                sw.Close();

           }

        }

        private void button5_Click(object sender, EventArgs e)
        {
            OpenFileDialog newDialog = new OpenFileDialog();
            newDialog.Filter = "Text File (*.txt) | *.txt";

            DialogResult result = newDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                StreamReader sr = new StreamReader(newDialog.FileName);
                string fullText = sr.ReadToEnd();
                string[] perHistogram = fullText.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

                for (int i = 0; i < perHistogram.Length; i++) //histogram iterator
                {
                    string[] DH = perHistogram[i].Split(new char[] { ':' });
                    string[] DH2 = DH[1].Split(new char[] { '#' }, StringSplitOptions.RemoveEmptyEntries);

                    savedHistogram newHist = new savedHistogram(DH[0]);

                    for (int j = 1; j < DH2.Length; j += 2) //channel iterator
                    {
                        PointPairList newList = new PointPairList();

                        string[] DH3 = DH2[j].Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries);

                        for (int k = 0; k < DH3.Length; k++) //point iterator
                        {
                            PointPair pair = new PointPair();
                            string[] DH4 = DH3[k].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                            for (int l = 0; l < 2; l++) //x-y iterator
                            {
                                if (l == 0)
                                {
                                    DH4[l].Remove(0);
                                    pair.X = Convert.ToDouble(DH4[l]);
                                }
                                else
                                {
                                    DH4[l].Remove(DH4[l].Length - 1);
                                    pair.Y = Convert.ToDouble(DH4[l].Remove(DH4[l].Length - 1));
                                }
                            }
                            newList.Add(pair);
                        }

                        newHist.channelList[Convert.ToUInt16((j - 1) / 2)].Add(newList);
                    }
                    histogramList.Add(newHist);

                }
            }
        }
    }

}
class BiasTuner
{

}

class tCnt
{
    public int[] cnt = new int[32];

    //constructor
    public tCnt()
    {
        for (int i = 0; i < 32; i++) { cnt[i] = 0; }
    }
}



