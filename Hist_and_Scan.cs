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
    public partial class Hist_and_Scan : DockContent
    {
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
        private TB4_Register[] Offset_reg = new TB4_Register[32];
        private TB4_Register IMON_reg = new TB4_Register("IMON", "", 0, 0, false, false);
        private Int32[] offset = new Int32[32];
        private int[] OnePE_pos = new Int32[32];
        private int OnePE_Goal = 0;

        static bool flgScannerActive = false;
        static bool flgGoScan = false;
        static double[] h = new double[2048];
        static double[] x = new double[2048];
        List<uint> tt = new List<uint>();
        List<tCnt> hh = new List<tCnt>();
        tCnt tot_c = new tCnt();
        tCnt c400 = new tCnt();
        tCnt c500 = new tCnt();
        tCnt c600 = new tCnt();

        private bool flgPE;
        private bool flgTuner_active;
        private bool flg_StopIV;
        private bool flg_CancelScan;


        Color[] arr_Curve_Col = new Color[32];


        //        public string[] cmb_Start0_text = new string[8];
        //        public ComboBox[] cmb_Trace_arr = new ComboBox[4];

        public Hist_and_Scan()
        {
            InitializeComponent();

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
                this.hist_chk_chan[i].CheckedChanged += new EventHandler(Hist_and_Scan_CheckedChanged);
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
        }
        #region buttons

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

        public void Hist_and_Scan_clear()
        {
            tt.Clear();
            hh.Clear();
            for (int i = 0; i < h.Length; i++)
            {
                h[i] = 0; x[i] = 0;
            }
        }

        void Hist_and_Scan_CheckedChanged(object sender, EventArgs e)
        {
            Hist_and_Scan_update();
            CheckBox foo = (CheckBox)sender;
            int i = (int)foo.Tag;
        }

        public void Hist_and_Scan_display()
        {
            // Get a reference to the GraphPane instance in the ZedGraphControl
            GraphPane myPane = zg1.GraphPane;
            myPane.CurveList.Clear();

            // Set the titles and axis labels
            myPane.Title.Text = "histo";
            myPane.XAxis.Title.Text = "t";
            myPane.YAxis.Title.Text = "#";
            myPane.CurveList.Clear();
            myPane.YAxis.Scale.Max = 66000;
            myPane.CurveList.Clear();


            // Show the x axis grid
            myPane.XAxis.MajorGrid.IsVisible = true;
            myPane.XAxis.MajorTic.ScaledTic(10);
            //myPane.XAxis.Scale.
            myPane.XAxis.Scale.MaxAuto = false;
            myPane.YAxis.Title.IsVisible = true;
            myPane.Legend.IsVisible = false;
            // Make the Y axis scale red
            myPane.YAxis.Scale.FontSpec.FontColor = Color.Black ;
            myPane.YAxis.Title.FontSpec.FontColor = Color.Black ;
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


        void Hist_and_Scan_update()
        {
            GraphPane myPane = zg1.GraphPane;
            myPane.CurveList.Clear();
            myPane.Title.Text = "histo";
            myPane.XAxis.Title.Text = "ADC";
            myPane.YAxis.Title.Text = "#";
            myPane.XAxis.MajorGrid.IsVisible = true;
            myPane.XAxis.MajorTic.ScaledTic(10);
            myPane.XAxis.Scale.Max = Math.Min(1000, 1.15 * Convert.ToDouble(hh.Count));
            myPane.XAxis.Scale.Min = 0;
            tCnt cn = new tCnt();
            int i = 0;
            //foreach (uint th in tt)
            //{
            //    x[i + 1] = Convert.ToDouble(th);
            //    i++;
            //}
            x[0] = 0;
            for (i = 0; i < hh.Count; i++)
            { x[i + 1] = i; }
            i = 0;
            for (int ind = 0; ind < 32; ind++)
            {
                if (hist_chk_chan[ind].Checked)
                {
                    tCnt[] arr_hi = hh.ToArray();
                    for (i = 0; i < hh.Count; i++)
                    {
                        cn = arr_hi[i];

                        h[i] = cn.cnt[ind];
                        if (this.chkLogY.Checked)
                        {
                            if (cn.cnt[ind] > 0)
                            { h[i] = Math.Log(cn.cnt[ind]); }
                            else
                            { h[i] = 0; }
                        }
                    }
                    for (i = 0; i < hh.Count - 1; i++)
                    {
                        //double d = h[i] - h[i + 1];
                        double d = h[i];
                        if (d <= 0) { d = 0.01; }
                        if (this.chkLogY.Checked)
                        { h[i] = Math.Log(d,2); }
                        //else
                        //{ h[i] = d; }
                    }
                    myPane.AddCurve("", x, h, arr_Curve_Col[ind], SymbolType.None);


                }

            }
            myPane.YAxis.Scale.MaxAuto = true;
            zg1.AxisChange();
            zg1.Invalidate();
        }



        private void btnCancel_Click(object sender, EventArgs e)
        {
            flgScannerActive = false;
            flgGoScan = false;
            timer1.Enabled = false;
            Hist_and_Scan_clear();
        }


        private void SCAN()
        {
            btnScan.Text = "...";
            //Hist_and_Scan_clear();

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
            TB4.TB4_Registers_Dict.TryGetValue("CSR", out ind_CSR);
            TB4.TB4_Registers_Dict.TryGetValue("THR_00", out ind_THR0);
            uint min = Convert.ToUInt16(udStart.Value);
            uint max = Convert.ToUInt16(udStop.Value + 1);
            uint inc = Convert.ToUInt16(udInc.Value);


            for (int c_c = 0; c_c < tot_c.cnt.Length; c_c++)
            { tot_c.cnt[c_c] = 0; }
            for (int c_c = 0; c_c < c400.cnt.Length; c_c++)
            { c400.cnt[c_c] = 0; }
            for (int c_c = 0; c_c < c500.cnt.Length; c_c++)
            { c500.cnt[c_c] = 0; }
            for (int c_c = 0; c_c < c600.cnt.Length; c_c++)
            { c600.cnt[c_c] = 0; }

            //for (uint thr = min; thr < max + 1; thr = thr + inc)
            uint thr = max;
            {
                btnScan.Text = thr.ToString();

                //tt.Add(thr);
                tCnt c = new tCnt();

                ushort v = Convert.ToUInt16(thr);
                TB4.TB4_Registers[ind_thr_A].RegWrite(v);
                TB4.TB4_Registers[ind_thr_B].RegWrite(v);
                TB4.TB4_Registers[ind_thr_C].RegWrite(v);
                TB4.TB4_Registers[ind_thr_D].RegWrite(v);
                csr = TB4.TB4_Registers[ind_CSR].RegRead();
                csr = (ushort)(csr | 0x06);
                TB4.TB4_Registers[ind_CSR].RegWrite(csr);

                Application.DoEvents();
                Thread.Sleep(0);
                csr = TB4.TB4_Registers[ind_CSR].RegRead();
                while ((csr & 0x04) != 4)
                {
                    Thread.Sleep(1);
                    csr = TB4.TB4_Registers[ind_CSR].RegRead();
                }


                byte A3 = (byte)((TB4.TB4_Registers[ind_THR0].addr & 0xff000000) >> 24);
                byte A2 = (byte)((TB4.TB4_Registers[ind_THR0].addr & 0x00ff0000) >> 16);
                byte A1 = (byte)((TB4.TB4_Registers[ind_THR0].addr & 0x0000ff00) >> 8);
                byte A0 = (byte)(TB4.TB4_Registers[ind_THR0].addr & 0x000000ff);
                TB4.ReadArray(A3, A2, A1, A0, 32, c.cnt);

                hh.Add(c);
                c.cnt.CopyTo(tot_c.cnt, 0);
                //for (int c_c = 0; c_c < c.cnt.Length; c_c++)
                //{ tot_c.cnt[c_c] += c.cnt[c_c]; }
                //hh.Add(tot_c);
                //if (thr == 400)
                //{
                //    for (int c_c = 0; c_c < c.cnt.Length; c_c++)
                //    { c400.cnt[c_c] = c.cnt[c_c]; }
                //}
                //if (thr == 500)
                //{
                //    for (int c_c = 0; c_c < c.cnt.Length; c_c++)
                //    { c500.cnt[c_c] = c.cnt[c_c]; }
                //}
                //if (thr == 600)
                //{
                //    for (int c_c = 0; c_c < c.cnt.Length; c_c++)
                //    { c600.cnt[c_c] = c.cnt[c_c]; }
                //}

            }
        }

        private void btnScan_Click_1(object sender, EventArgs e)
        {
            scanDataPush();
            /*
            string myFileName = "";
            DateTime n = System.DateTime.Now;
            myFileName = "c:\\data\\Scanner_" + n.Year.ToString() + n.Month.ToString("00") + n.Day.ToString("00") + "_" + n.Hour.ToString("00") + n.Minute.ToString("00") + n.Second.ToString("00");
            myFileName += ".TB4";
            StreamWriter newScan;
            try
            {
                newScan = new StreamWriter(myFileName, false);
            }
            catch (Exception ex)
            {
                TB4_Exception.logError(ex, "Could not open a stream to the file " + myFileName + ".", true);
                return;
            }

            flgScannerActive = true;
            btnScan.Text = "SCANing...";

            timer1.Interval = Convert.ToInt32(udSamplePer.Value);
            timer1.Enabled = true;

            Hist_and_Scan_display();

           

            while (flgScannerActive)
            {
                lblScanMsg.Text = "waiting ...";
                Application.DoEvents();
                if (flgGoScan)
                {
                    
                    //for (int i = 0; i < TB4.PADE_List.Count; i++)
                    //{
                        Application.DoEvents();
                        PADE thisPADE;
                        thisPADE = TB4.ActivePADE;

                        //if (TB4.PADE_List.TryGetValue(i + 1, out thisPADE))
                        //{

                            string t =DateTime.Now.ToString()+ " SN: " + thisPADE.PADE_sn.ToString();
                            lblScanMsg.Text = "Now doing board " + "SN: " + thisPADE.PADE_sn.ToString(); ;
                            //TB4.ActivePADE = thisPADE;

                            SCAN();
                            if (hh.Count > 500) { hh.RemoveRange(0, 200); }

                            
                            Hist_and_Scan_update();
                            Application.DoEvents();
                            for (int ii = 0; ii < 32; ii++)
                            {
                                t = t + " " + tot_c.cnt[ii].ToString();
                            }
                            //newScan.WriteLine(t);

                            //t = "thr = 400";
                            //for (int ii = 0; ii < 32; ii++)
                            //{
                            //    t = t + " " + c400.cnt[ii].ToString();
                            //}
                            //newScan.WriteLine(t);

                            //t = "thr = 500";
                            //for (int ii = 0; ii < 32; ii++)
                            //{
                            //    t = t + " " + c500.cnt[ii].ToString();
                            //}
                            //newScan.WriteLine(t);

                            //t = "thr = 600";
                            //for (int ii = 0; ii < 32; ii++)
                            //{
                            //    t = t + " " + c600.cnt[ii].ToString();
                            //}
                            newScan.WriteLine(t);
                    //    }
                    //}
                    flgGoScan = false;
                    Application.DoEvents();
                    //bool fileExists = true;
                    //string myHandShake = "c:\\data\\LV\\done";
                    //int file_count = 0;
                    //while (fileExists)
                    //{
                    //    fileExists = false;
                    //    file_count++;
                    //    DirectoryInfo source = new DirectoryInfo("c:\\data\\LV");
                    //    foreach (FileInfo fi in source.GetFiles())
                    //    {
                    //        if (fi.FullName == (myHandShake + "_" + file_count.ToString()))
                    //        { fileExists = true; }
                    //    }
                    //}
                    //myHandShake = myHandShake + "_" + file_count.ToString() + ".";
                    //StreamWriter swHandShake = new StreamWriter(myHandShake, false);
                    //swHandShake.WriteLine(DateTime.Now.ToString());
                    //swHandShake.Close();
                }
                //Thread.Sleep(10);
            }
            newScan.Close();
            btnScan.Text = "SCAN";
            lblScanMsg.Text = "All done at " + Convert.ToString(DateTime.Now);
             * */
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            flgGoScan = true;
        }

        private static void OnChanged(object source, FileSystemEventArgs e)
        {
            // Specify what is done when a file is changed, created, or deleted.
            Console.WriteLine("File: " + e.FullPath + " " + e.ChangeType);
            if (e.Name.Contains("done")) { flgScannerActive = false; }
            if (!flgGoScan) { flgGoScan = true; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            Hist_and_Scan_display();
        }

        private void btnSavePeds_Click(object sender, EventArgs e)
        {
            //    string myFileName = "";

            //    DateTime n = System.DateTime.Now;
            //    myFileName = "c:\\data\\Offsets_" + n.Year.ToString() + n.Month.ToString("00") + n.Day.ToString("00") + "_" + n.Hour.ToString("00") + n.Minute.ToString("00") + n.Second.ToString("00");
            //    myFileName += "_SN" + TB4.ActivePADE.IP4_add[0].ToString();
            //    myFileName += ".TB4";
            //    StreamWriter newPeds = new StreamWriter(myFileName, false);
            //    for (int i = 0; i < 32; i++)
            //    {
            //        //                if (UpdatedPed_flag[i])
            //        //                {
            //        //                    string t = "PED_SUBTRACT_" + i.ToString() + "<=";
            //        //                    t += "dec" + UpdatedPed[i].ToString();
            //        //                    newPeds.WriteLine(t);
            //        //                }
            //        string t = "BIAS_OFFSET_CH" + i.ToString() + "<=";
            //        t += "dec" + offset[i].ToString();
            //        newPeds.WriteLine(t);
            //    }
            //    newPeds.Close();
        }

        private void Hist_and_Scan_Load(object sender, EventArgs e)
        {
            TB4.theHist_and_Scan = this;
            Size returnSize = this.Size;
            this.DockStateChanged += (object a, System.EventArgs b) => { TB4.thePADE_explorer.childChangedDockstate(this, returnSize); };
        }

        void Hist_and_Scan_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            TB4.thePADE_explorer.childClosed(this);
            e.Cancel = true;
        }

        void graphButton_Click(object sender, System.EventArgs e)
        {
            PADE_explorer.saveGraphImage(zg1);
        }

        void scanDataPush()
        {
            //retrieve data
            int[] rdata=new int[1200];
            

            zg1.GraphPane.CurveList.Clear();
            GraphPane mainPane=zg1.GraphPane;

            TB4_Register adcCheckReg=new TB4_Register("num_ADC_samples", "", 0x04800000, 8, true, false);
            int counter=0;

            //software reset
            PADE_explorer.registerLookup("SOFTWARE_RESET").RegWrite(1);

            //send software trigger

            PADE_explorer.registerLookup("SOFTWARE_TRIGGER").RegWrite(1);
           
            DateTime startTime = DateTime.Now; 
            while (600 > adcCheckReg.RegRead()) 
            {
                if ((DateTime.Now - startTime).TotalMilliseconds > 1000) { TB4_Exception.logError(null,"scanDataPush timeout.", true); return; }
                Thread.Sleep(1); 
            }

            {
                TB4.ReadArray(04, 00, 00, 00, 1200, rdata);
                for (int i = 0; i < 1200; i += 2)
                {
                    ((IPointListEdit)mainPane.CurveList[i].Points).Add(i / 2, (rdata[i + 1] + rdata[i] << 8));
                    counter++;
                }
                zg1.Invalidate();
            }
            


        }






    }
}
