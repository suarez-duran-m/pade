using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Timers;
using ZedGraph;
using System.IO;
using WeifenLuo.WinFormsUI.Docking;

namespace PADE
{
   

    public partial class Hist0 : DockContent
    {
        static double[] h = new double[2048];
        static double[] x = new double[2048];
        static int reg_count = 0;
        int[] dataA = new int[4096];
        int[] dataB = new int[4096];
        int[] dataC = new int[4096];
        int[] dataD = new int[4096];
        private System.Windows.Forms.Label[] RegisterIndex = new System.Windows.Forms.Label[50];
        private System.Windows.Forms.Label[] RegisterLabel = new System.Windows.Forms.Label[50];
        private System.Windows.Forms.TextBox[] RegisterValue = new System.Windows.Forms.TextBox[50];
        private System.Windows.Forms.Button[] RegisterWrite = new System.Windows.Forms.Button[50];
        private System.Windows.Forms.Button[] RegisterRead = new System.Windows.Forms.Button[50];
        private TB4_Register[] Thresh = new TB4_Register[4];
        private TB4_Register[] Offset = new TB4_Register[32];

        List<uint> tt = new List<uint>();
        List<tCnt> hh = new List<tCnt>();

        public Hist0()
        {
            InitializeComponent();


            chkPedMode.Text = "PED";
            for (int i = 0; i < 2048; i++)
            {
                if (i < 2048) { x[i] = i; }
                h[i] = 0;
            }
        }

        private void Hist0_Load(object sender, EventArgs e)
        {

            for (int j = 1; j < TB4.TB4_Registers.Length - 1; j++)
            {


                if (TB4.TB4_Registers[j] == null)
                {
                    //then nothing
                }
                else
                {
                    if (TB4.TB4_Registers[j].name.Contains("PED_SUBTRACT_") || TB4.TB4_Registers[j].name.Contains("TRIG_THRESHOLD"))
                    {

                        reg_count++;

                        for (int i = 0; i < 32; i++)
                        {
                            if (TB4.TB4_Registers[j].name == ("PED_SUBTRACT_" + i.ToString()))
                            {
                                Offset[i] = TB4.TB4_Registers[j];
                                Offset[i].private_comment = "A";
                                if (i > 7) { Offset[i].private_comment = "B"; }
                                if (i > 15) { Offset[i].private_comment = "C"; }
                                if (i > 23) { Offset[i].private_comment = "D"; }
                                i = 100;
                            }
                        }
                        for (int i = 0; i < 0; i++)
                        {
                            if (TB4.TB4_Registers[j].name.Contains("TRIG_THRESHOLD_CH" + i.ToString()))
                            {
                                Thresh[i] = TB4.TB4_Registers[j];
                                Thresh[i].private_comment = "A";
                                if (i == 1) { Thresh[i].private_comment = "B"; }
                                if (i == 2) { Thresh[i].private_comment = "C"; }
                                if (i == 3) { Thresh[i].private_comment = "D"; }
                                i = 100;
                            }
                        }
                    }
                }
            }

            int ypos = 450;
            int xpos = 0;

            for (int i = 0; i < 4; i++)
            {
                ypos = 450 + 25 * i;
                RegisterIndex[i] = new System.Windows.Forms.Label();
                RegisterIndex[i].Name = "RegIndex" + i.ToString();
                RegisterIndex[i].Location = new System.Drawing.Point(xpos, ypos);
                RegisterIndex[i].Size = new System.Drawing.Size(5, 20);
                RegisterIndex[i].Text = i.ToString();
                RegisterIndex[i].Visible = true;

                RegisterLabel[i] = new System.Windows.Forms.Label();
                RegisterLabel[i].Name = "RegLabel" + i.ToString();
                RegisterLabel[i].Location = new System.Drawing.Point(5 + xpos, ypos);
                RegisterLabel[i].Size = new System.Drawing.Size(200, 20);
                string t = "label";
                //t = Convert.ToString(Thresh[i].addr, 16) + "   ";
                //t += Thresh[i].name;
                RegisterLabel[i].Text = t;
                RegisterLabel[i].Visible = true;

                RegisterValue[i] = new System.Windows.Forms.TextBox();
                RegisterValue[i].Name = "RegValue" + i.ToString();
                RegisterValue[i].Location = new System.Drawing.Point(210 + xpos, ypos);
                RegisterValue[i].Size = new System.Drawing.Size(70, 20);
                RegisterValue[i].Text = "value";
                RegisterValue[i].Visible = true;

                RegisterRead[i] = new System.Windows.Forms.Button();
                RegisterRead[i].Name = "RegRead" + i.ToString();
                RegisterRead[i].Location = new System.Drawing.Point(290 + xpos, ypos);
                RegisterRead[i].Size = new System.Drawing.Size(30, 20);
                RegisterRead[i].Text = "R";
                RegisterRead[i].Visible = true;
                RegisterRead[i].Click += new EventHandler(RegisterRead_Click);

                RegisterWrite[i] = new System.Windows.Forms.Button();
                RegisterWrite[i].Name = "RegWrite" + i.ToString();
                RegisterWrite[i].Location = new System.Drawing.Point(330 + xpos, ypos);
                RegisterWrite[i].Size = new System.Drawing.Size(30, 20);
                RegisterWrite[i].Text = "W";
                RegisterWrite[i].Visible = true;
                RegisterWrite[i].Click += new EventHandler(RegisterWrite_Click);

                {
                    this.Controls.Add(RegisterLabel[i]);
                    this.Controls.Add(RegisterValue[i]);
                    this.Controls.Add(RegisterRead[i]);
                    this.Controls.Add(RegisterWrite[i]);
                }
            }
            for (int i = 4; i < 8; i++)
            {
                ypos = 470 + 25 * i;
                RegisterIndex[i] = new System.Windows.Forms.Label();
                RegisterIndex[i].Name = "RegIndex" + i.ToString();
                RegisterIndex[i].Location = new System.Drawing.Point(xpos, ypos);
                RegisterIndex[i].Size = new System.Drawing.Size(5, 20);
                RegisterIndex[i].Text = i.ToString();
                RegisterIndex[i].Visible = true;

                RegisterLabel[i] = new System.Windows.Forms.Label();
                RegisterLabel[i].Name = "RegLabel" + i.ToString();
                RegisterLabel[i].Location = new System.Drawing.Point(5 + xpos, ypos);
                RegisterLabel[i].Size = new System.Drawing.Size(200, 20);
                string t = "label";
                //t = Convert.ToString(Thresh[i].addr, 16) + "   ";
                //t += Thresh[i].name;
                RegisterLabel[i].Text = t;
                RegisterLabel[i].Visible = true;

                RegisterValue[i] = new System.Windows.Forms.TextBox();
                RegisterValue[i].Name = "RegValue" + i.ToString();
                RegisterValue[i].Location = new System.Drawing.Point(210 + xpos, ypos);
                RegisterValue[i].Size = new System.Drawing.Size(70, 20);
                RegisterValue[i].Text = "value";
                RegisterValue[i].Visible = true;

                RegisterRead[i] = new System.Windows.Forms.Button();
                RegisterRead[i].Name = "RegRead" + i.ToString();
                RegisterRead[i].Location = new System.Drawing.Point(290 + xpos, ypos);
                RegisterRead[i].Size = new System.Drawing.Size(30, 20);
                RegisterRead[i].Text = "R";
                RegisterRead[i].Visible = true;
                RegisterRead[i].Click += new EventHandler(RegisterRead_Click);
                //if (TB4.TB4_Registers[j].writeOnly)
                //{ RegisterRead[i].Visible = false; }
                //else
                //{ RegisterRead[i].Visible = true; }

                RegisterWrite[i] = new System.Windows.Forms.Button();
                RegisterWrite[i].Name = "RegWrite" + i.ToString();
                RegisterWrite[i].Location = new System.Drawing.Point(330 + xpos, ypos);
                RegisterWrite[i].Size = new System.Drawing.Size(30, 20);
                RegisterWrite[i].Text = "W";
                RegisterWrite[i].Visible = true;
                RegisterWrite[i].Click += new EventHandler(RegisterWrite_Click);
                //if (TB4.TB4_Registers[j].readOnly)
                //{ RegisterWrite[i].Visible = false; }
                //else
                //{ RegisterWrite[i].Visible = true; }

                {
                    this.Controls.Add(RegisterLabel[i]);
                    this.Controls.Add(RegisterValue[i]);
                    this.Controls.Add(RegisterRead[i]);
                    this.Controls.Add(RegisterWrite[i]);
                }
            }

            for (int i = 0; i < 4; i++)
            {
                RegisterLabel[i].Text = Convert.ToString(Offset[8 * i].addr, 16) + "  " + Offset[8 * i].name;
                //RegisterLabel[i + 4].Text = Convert.ToString(Thresh[i].addr, 16) + "  " + Thresh[i].name;
            }


        }
        public void Hist0_display(bool flg_log, bool flg_ped)
        {
            // Get a reference to the GraphPane instance in the ZedGraphControl
            GraphPane myPane = zg_Histo.GraphPane;
            myPane.CurveList.Clear();

            // Set the titles and axis labels
            myPane.Title.Text = "ADC histo";
            myPane.XAxis.Title.Text = "ADC";
            myPane.YAxis.Title.Text = "#";
            myPane.CurveList.Clear();
            CurveItem myHistA = myPane.AddCurve("Hist", x, h, Color.Red, SymbolType.None);
            myPane.YAxis.Scale.Max = 66000;

            // Show the x axis grid
            myPane.XAxis.MajorGrid.IsVisible = true;
            myPane.XAxis.MajorTic.ScaledTic(10);
            //myPane.XAxis.Scale.
            myPane.XAxis.Scale.MaxAuto = true;
            myPane.YAxis.Title.IsVisible = false;
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
            zg_Histo.IsShowHScrollBar = true;
            zg_Histo.IsShowVScrollBar = true;
            zg_Histo.IsAutoScrollRange = true;

            //// OPTIONAL: Show tooltips when the mouse hovers over a point
            //zg1.IsShowPointValues = true;
            //zg1.PointValueEvent += new ZedGraphControl.PointValueHandler(MyPointValueHandler);

            //// OPTIONAL: Add a custom context menu item
            //zg1.ContextMenuBuilder += new ZedGraphControl.ContextMenuBuilderEventHandler(
            //        MyContextMenuBuilder);

            // OPTIONAL: Handle the Zoom Event
            //      zg1.ZoomEvent += new ZedGraphControl.ZoomEventHandler(MyZoomEvent);


            myPane.XAxis.Scale.Max = 1.2 * Convert.ToDouble(udStop.Value);
            myPane.XAxis.Scale.Min = .8 * Convert.ToDouble(udStart.Value);
            // Size the control to fit the window
            //      SetSize();

            // Tell ZedGraph to calculate the axis ranges
            // Note that you MUST call this after enabling IsAutoScrollRange, since AxisChange() sets
            // up the proper scrolling parameters
            zg_Histo.AxisChange();
            // Make sure the Graph gets redrawn
            zg_Histo.Invalidate();
            
        }

        public void Hist0_clear()
        {
            tt.Clear();
            hh.Clear();
            for (int i = 0; i < h.Length; i++)
            {
                h[i] = 0; x[i] = 0;
            }
        }
        void Hist0_update()
        {
            tCnt cn = new tCnt();
            int i = 0;
            foreach (uint th in tt)
            {
                x[i] = Convert.ToDouble(th);
                i++;
            }
            i = 0;
            foreach (tCnt hi in hh)
            {
                cn = hi;
                int ind = Convert.ToInt32(udChan.Value);
                h[i] = cn.cnt[ind];
                if (this.chkLogY.Checked) 
                {
                    if (cn.cnt[ind] > 0)
                    { h[i] = Math.Log(cn.cnt[ind]); }
                    else
                    { h[i] = 0; }
                }
                i++;
            }
//            h[i-1] = h[i-2];
            if ((i > 0) && (i + 1 < x.Length))
            {
                x[i] = x[i - 1] + 1;
                x[i + 1] = x[i] + 1;
            }
            Hist0_display(false, false);
            
        }
        private void button1_Click(object sender, EventArgs e)//update
        {
            bool flg_logy = false; bool flg_ped = false;
            if (chkLogY.Checked) { flg_logy = true; }
            if (chkPedMode.Checked) { flg_ped = true; }
            Hist0_display(flg_logy, flg_ped);
            lblCSR.Text = "CSR = " + Convert.ToString(TB4.regSTATUS.RegRead(), 16);
            Application.DoEvents();
        }

        private void btnRestart_Click(object sender, EventArgs e)
        {
            TB4_Register sw_reset = new TB4_Register("temp", "temp", 0xff00000, 16, false, true);
            TB4_Register hist_type = new TB4_Register("temp", "temp", 0x9600000, 16, false, false);
            TB4_Register chan_select = new TB4_Register("temp", "temp", 0x9500000, 16, false, false);
            //turn off histo
            hist_type.RegWrite(0);
            System.Threading.Thread.Sleep(10);
            //soft reset
            sw_reset.RegWrite(0);
            System.Threading.Thread.Sleep(1);
            //select chan
            ushort cs = 0;
            try { cs = Convert.ToUInt16(udChan.Value); }
            catch { cs = 0; }
            cs = Convert.ToUInt16(cs + cs * 16 + cs * 256 + cs * 4096);
            chan_select.RegWrite(cs);
            //turn on histo
            if (chkPedMode.Checked) { hist_type.RegWrite(3); }
            else { hist_type.RegWrite(2); }
            System.Threading.Thread.Sleep(50);
            button1_Click(null, null);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
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
            myFileName += ".histo";
            StreamWriter newHisto = new StreamWriter(myFileName, false);

            for (int i = 0; i < 2000; i++)
            {
                string l = i.ToString() + ", ";
                //if (checkBoxA.Checked)
                { l += dataA[i].ToString() + ", "; }
                { l += dataB[i].ToString() + ", "; }
                { l += dataC[i].ToString() + ", "; }
                { l += dataD[i].ToString() + ", "; }
                newHisto.WriteLine(l);
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

        private void udChan_ValueChanged(object sender, EventArgs e)
        {
            labelA.Text = "CHAN A= " + udChan.Value.ToString();
            //labelB.Text = "CHAN B= " + (8 + udChan.Value).ToString();
            //labelC.Text = "CHAN C= " + (16 + udChan.Value).ToString();
            //labelD.Text = "CHAN D= " + (24 + udChan.Value).ToString();

            int j = Convert.ToInt32(udChan.Value);
            Hist0_update();

        }

        private void RegisterWrite_Click(object sender, EventArgs e)
        {
            int i = 0; int j = 0;
            System.Windows.Forms.Button thisButton = new Button();
            thisButton = (Button)sender;
            i = Array.IndexOf(RegisterWrite, thisButton);
            UInt16 ui_data = 0;
            try
            { ui_data = Convert.ToUInt16(this.RegisterValue[i].Text, 16); }
            catch
            { ui_data = 0; }

            if (i < 4)
            {
                j = i * 8 + Convert.ToInt32(udChan.Value);
                Offset[j].RegWrite(ui_data);
            }
            else
            {
                j = (i - 4);
                Thresh[j].RegWrite(ui_data);
            }
        }

        private void RegisterRead_Click(object sender, EventArgs e)
        {
            int i = 0; int j = 0;
            System.Windows.Forms.Button thisButton = new Button();
            thisButton = (Button)sender;
            i = Array.IndexOf(RegisterRead, thisButton);
            if (i < 4)
            {
                j = i *8 + Convert.ToInt32(udChan.Value);
                ushort val = 0;
                val = Offset[j].RegRead();
                this.RegisterValue[i].Text = Convert.ToString(val, 16);
            }
            else
            {
                j = (i - 4);
                ushort val = 0;
                val = Thresh[j].RegRead();
                this.RegisterValue[i].Text = Convert.ToString(val, 16);
            }
        }

        private void btnAutoPed_Click(object sender, EventArgs e)
        {
            int ind_SOFT_TRIG = 0;
            MessageBox.Show("make sure bias is down", "BIAS OFF", MessageBoxButtons.OK);
            TB4.TB4_Registers_Dict.TryGetValue("SOFT_TRIG", out ind_SOFT_TRIG);

            bool saved_ParamExtTrig;
            bool saved_ParamZS;
            bool saved_ParamSumOnly;

            saved_ParamExtTrig=TB4.theRunForm.chk_ParamExtTrig.Checked;
            saved_ParamZS = TB4.theRunForm.chk_ParamZS.Checked;
            saved_ParamSumOnly = TB4.theRunForm.chk_ParamSumOnly.Checked;

            TB4.theRunForm.chk_ParamExtTrig.Checked = false;
            TB4.theRunForm.chk_ParamZS.Checked = false;
            TB4.theRunForm.chk_ParamSumOnly.Checked = false;

            TB4_Register SoftTrig_reg;

            SoftTrig_reg = TB4.TB4_Registers[ind_SOFT_TRIG];
            for (int i = 0; i < 1; i++)
            {
                SoftTrig_reg.RegWrite(1);
            }
            
        }

        private void udStart_ValueChanged(object sender, EventArgs e)
        {
            if (udStart.Value >= udStop.Value) { udStop.Value = udStart.Value + 1; }
        }

        private void udStop_ValueChanged(object sender, EventArgs e)
        {
            if (udStop.Value <= udStart.Value) { udStart.Value = udStop.Value - 1; }
        }

        

        private void btnScan_Click(object sender, EventArgs e)
        {

            btnScan.Text = "...";
            Hist0_clear();
            
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
            for (uint thr = min; thr < max+1; thr = thr + inc)
            {
                btnScan.Text = thr.ToString();
            
                tt.Add(thr);
                tCnt c = new tCnt();
                ushort v = Convert.ToUInt16(thr);
                TB4.TB4_Registers[ind_thr_A].RegWrite(v);
                TB4.TB4_Registers[ind_thr_B].RegWrite(v);
                TB4.TB4_Registers[ind_thr_C].RegWrite(v);
                TB4.TB4_Registers[ind_thr_D].RegWrite(v);
                csr = TB4.TB4_Registers[ind_CSR].RegRead();
                csr = (ushort)(csr | 0x04);
                TB4.TB4_Registers[ind_CSR].RegWrite(csr);
                
                Thread.Sleep(1);
                Application.DoEvents();
                csr = TB4.TB4_Registers[ind_CSR].RegRead();
                while ((csr & 0x04) != 4)
                {
                    Thread.Sleep(2);
                    csr = TB4.TB4_Registers[ind_CSR].RegRead();
                }

                
                byte A3 = (byte)((TB4.TB4_Registers[ind_THR0].addr & 0xff000000)>> 24);
                byte A2 = (byte)((TB4.TB4_Registers[ind_THR0].addr & 0x00ff0000) >> 16);
                byte A1 = (byte)((TB4.TB4_Registers[ind_THR0].addr & 0x0000ff00) >> 8);
                byte A0 = (byte)(TB4.TB4_Registers[ind_THR0].addr & 0x000000ff );
                TB4.ReadArray(A3, A2, A1, A0, 32, c.cnt);
                //for (int i = 0; i < 32; i++) 
                //{ c.cnt[i] = TB4.TB4_Registers[ind_THR0 + i].RegRead(); }

                hh.Add(c);
                
            }
            btnScan.Text = "SCAN";
            Hist0_update();
        }

        private void chkLogY_CheckedChanged(object sender, EventArgs e)
        {

        }



    }
    
}