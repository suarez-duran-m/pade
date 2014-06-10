using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using ZedGraph;
using WeifenLuo.WinFormsUI.Docking;

namespace PADE
{
    public partial class DataDebug : DockContent
    {

        string dumpPathname = null;
        UInt32 startAdd = 0;
        UInt32 endAdd = 0;
        double[] x= new double [1000];

        BackgroundWorker bkgdWork = new BackgroundWorker();
        ushort activeChannel = new ushort();
        

        public DataDebug()
        {
            InitializeComponent();
            for (int i = 0; i < 1000; i++) { x[i] = (double)i; }
            this.LostFocus += new EventHandler(DataDebug_LostFocus);
            this.regComboBox.Hide();
            populateRegComboBox();
        }

        void DataDebug_LostFocus(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            Console.WriteLine("LOST FOCUS");
        }

        void DataDebug_VisibleChanged(object sender, System.EventArgs e)
        {
            GraphPane myPane = zg1.GraphPane;
            myPane.Title.Text = "Data Debugging";
            myPane.XAxis.Title.Text = "Sample";
            myPane.YAxis.Title.Text = "#";
            myPane.XAxis.MajorGrid.IsVisible = true;
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

            bkgdWork.DoWork += new DoWorkEventHandler(bkgdWork_DoWork);
            myPane.XAxis.MajorTic.ScaledTic(10);
            zg1.AxisChange();
            zg1.Invalidate();

            mainPane = zg1.GraphPane;
            GraphItem = mainPane.AddCurve("Data", pointlist, Color.Red, SymbolType.None);
        }



        Timer timer1 = new Timer();

        
        private void DataDebug_Load(object sender, EventArgs e)
        {
            startBox.Text = "0x01100000";
            startAdd = 0x01100000;
            endBox.Text = "0x011001A9";
            endAdd = 0x011001A9;
            numRegBox.Text = "425";

            compressionCombo.Items.Add("Floating Point");
            compressionCombo.Items.Add("Divide By Two");
            compressionCombo.Items.Add("Divide By Four");
            timer1.Interval =1000 ;
            timer1.Tick += new EventHandler(timer1_Tick);
            Size returnSize=this.Size;
            this.DockStateChanged += (object a, System.EventArgs b) => { TB4.thePADE_explorer.childChangedDockstate(this, returnSize); };
        }


        void timer1_Tick(object sender, EventArgs e)
        {

            if (compressionCombo.Text != "Reg Val vs. Time") zg1.GraphPane.CurveList.Clear();
            pullData(dumpPathname);
            zg1.Invalidate();
            //zg1.AxisChange();



            //plotRandom();
            this.Invalidate();
            
            Application.DoEvents();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            zg1.Invalidate();
            base.OnPaint(e);
        }

        
        private void endSubmit_Click(object sender, EventArgs e)
        {
            endAdd = convertToDecimal(endBox.Text);
            numRegBox.Text = Convert.ToString(endAdd - startAdd, 10);
        }

        public void plotRandom()
        {
            PointPairList returnList = new PointPairList();
            Random newRandom=new Random((int) DateTime.Now.Ticks);

            for(int i=1; i<100; i++)
            {
                returnList.Add(i, newRandom.Next());
            }

            ZedGraph.LineItem myLine = zg1.GraphPane.AddCurve("Channel" + activeChannel.ToString(), returnList, Color.Red, SymbolType.None);
            myLine.Label.IsVisible = false;



            zg1.GraphPane.YAxis.Scale.MaxAuto = true;
            zg1.GraphPane.YAxis.Scale.MaxAuto = true;
            zg1.AxisChange();
            zg1.Invalidate();
        }
            
             

        private void updateNumReg()
        {
            UInt32 startresult, endresult = 0;
            try{
            
             endresult = Convert.ToUInt32(endBox.Text,16);
             startresult= Convert.ToUInt32(startBox.Text,16);
             numRegBox.Text = Convert.ToString(endresult - startresult);
            }
            catch
            {
                MessageBox.Show("Error: the starting or ending address is not numeric.");
            }
        }
        private void updateEndAdd()
        {
            UInt32 numReg=0, startadd = 0;

            if((startAdd= Convert.ToUInt32(startBox.Text,16))>0)
            {
                if (numRegBox.Text.ToLower().Contains("x"))
                { numReg = Convert.ToUInt32(numRegBox.Text, 16); }
                else
                { numReg = Convert.ToUInt32(numRegBox.Text); }

                if (numReg>0 )
                {
                    endBox.Text = "0x" + Convert.ToString(startadd + numReg, 16);
                    endAdd = startadd + numReg;
                }
            }

        }
        
       

        private void numRegButton_Click(object sender, EventArgs e)
        {
            endAdd = startAdd + convertToDecimal(numRegBox.Text); 

            endBox.Text = "0x" + Convert.ToString(endAdd, 16);
        }

        LineItem GraphItem;
        ZedGraph.GraphPane mainPane;
        PointPairList pointlist = new PointPairList();

        System.IO.StreamWriter swriter=null;
                
        private void pullData(string dumpPathname=null)
        {
            PointPairList channelData=new PointPairList();
            if (radioButton1.Checked == false)
            {
                //software reset
                PADE_explorer.registerLookup("SOFTWARE_RESET").RegWrite(1);

                PointPairList list = new PointPairList();

                button5.Text = "waiting...";
                System.Threading.Thread.Sleep(500);
                button5.Text = "Plot";
                TB4_Register tempReg = new TB4_Register("DEBUG_REG", "$", (uint)startAdd, 16, false, false);
                uint var = endAdd - startAdd;
                int[] data = new int[endAdd - startAdd];
                int[] x = new int[endAdd - startAdd];
                int counter = 0;
                TB4_Register dataMode = PADE_explorer.registerLookup("DATA_STORAGE_MODE");

                TB4_Register controlReg = PADE_explorer.registerLookup("CONTROL_REG");
                BoardFunctions.dataMode currentMode = BoardFunctions.dataMode.scope;
                if (counterRadio.Checked) currentMode = BoardFunctions.dataMode.counter;

                BoardFunctions.dataCompressionMode compressionMode;
                if (compressionCombo.Text == "Floating Point") compressionMode = BoardFunctions.dataCompressionMode.floatingPoint;
                else if (compressionCombo.Text == "Divide By Two") compressionMode = BoardFunctions.dataCompressionMode.divideByTwo;
                else compressionMode = BoardFunctions.dataCompressionMode.divideByFour;

                

                if (checkBox1.Checked && dumpPathname != null && swriter == null) swriter = new System.IO.StreamWriter(dumpPathname);


                channelData = BoardFunctions.takeData(currentMode, activeChannel, compressionMode, new BoardFunctions.addressRange(startAdd, endAdd));

                if (checkBox1.Checked && dumpPathname != null)
                {
                    swriter.WriteLine("\nCHANNEL " + activeChannel.ToString() + " DATA:");
                    for (ushort A = 0; A < channelData.Count; A++) swriter.Write(channelData[A].Y.ToString() + ",");
                }



                GraphItem = zg1.GraphPane.AddCurve("Channel" + activeChannel.ToString(), channelData, Color.Red, SymbolType.None);
                GraphItem.Label.IsVisible = false;

            }
            else
            {
                if (compressionCombo.Text == "Reg Val vs. Board")
                {
                    string oldActive=TB4.activeIndex;
                    TB4_Register tempReg=PADE_explorer.registerLookup(regComboBox.Text);
                    double[] x=new double[TB4.PADE_List.Count];
                    double[] y=new double[TB4.PADE_List.Count];

                    int ocrapcounter=0;
                    foreach (KeyValuePair<int, PADE> pade in TB4.PADE_List)
                    {
                        
                        TB4.activatePADE(pade.Value);

                        x[ocrapcounter]=Convert.ToDouble(pade.Value.PADE_sn);
                        y[ocrapcounter]=Convert.ToDouble(tempReg.RegRead());
                        ocrapcounter++;

                    }
                    TB4.activatePADE(PADE_explorer.getPADE(oldActive));
                    BarItem bar = zg1.GraphPane.AddBar("Register Values", x, y, Color.Red);
                    bar.Label.IsVisible = false;
                }
                else if (compressionCombo.Text == "Reg Val vs. Time")
                {
                    string oldActive = TB4.activeIndex;
                    TB4_Register tempReg = PADE_explorer.registerLookup(regComboBox.Text);

                    GraphItem.AddPoint(new XDate(DateTime.Now), tempReg.RegRead());
                    //if(GraphItem.Points.Count>49) GraphItem.RemovePoint(0);
                    GraphItem.Label.IsVisible = false;


                    TB4.activatePADE(PADE_explorer.getPADE(oldActive));

                    mainPane.AxisChange();
                    zg1.Invalidate();

                }
                


            }

            zg1.GraphPane.YAxis.Scale.MaxAuto = true;
            zg1.GraphPane.YAxis.Scale.MaxAuto = true;
            
            zg1.AxisChange();
            zg1.Invalidate();
        }
        PointPairList timeSeries = new PointPairList();
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                saveFileDialog1.FileName = "";
                System.Windows.Forms.DialogResult result=saveFileDialog1.ShowDialog();

                if (result == DialogResult.OK)
                {
                    dumpPathname = saveFileDialog1.FileName;
                    swriter = null;
                }
                else
                {
                    checkBox1.Checked = false;
                }
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (scopeRadio.Checked && !counterRadio.Checked)
            {
                registerModeLabelSwitch(false);
                counterRadio.Checked = false;
                GraphPane myPane = zg1.GraphPane;
                myPane.Title.Text = "Scope Mode";
                zg1.AxisChange();
                zg1.Invalidate();
            }
        }

        private void counterRadio_CheckedChanged(object sender, EventArgs e)
        {
            if (!scopeRadio.Checked && counterRadio.Checked)
            {
                registerModeLabelSwitch(false);
                scopeRadio.Checked = false;

                zg1.GraphPane.Title.Text = "Counter Mode";
                zg1.AxisChange();
                zg1.Invalidate();
            }
           
        }
        
        private void startSubmit_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked == false)
            {

                startAdd = convertToDecimal(startBox.Text);
                try
                {

                    numRegBox.Text = Convert.ToString(endAdd - startAdd, 10);

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                bool regFound=false;
                PADE_explorer.registerLookup(startBox.Text);
            }

            
            
        }

        private void button5_Click(object sender, EventArgs e)
        {
            pullData(dumpPathname);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //GraphPane myPane = zg1.GraphPane;
            if (compressionCombo.Text != "Reg Val vs. Time") zg1.GraphPane.CurveList.Clear();
            else GraphItem.Clear();
            zg1.Invalidate();
            zg1.AxisChange();
        }

        private UInt32 convertToDecimal(string number)
        {
            UInt32 returnVal=0;
            try
            {
                if (number.ToLower().Contains('x'))
                {
                    //the number is hexadecimal
                    returnVal = Convert.ToUInt32(number, 16);

                }
                else
                {
                    returnVal = Convert.ToUInt32(number);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return returnVal;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox3.Text="";
            int[] data = new int[endAdd - startAdd];
            try
            {
                TB4.ReadArray((byte)(startAdd >> 24), (byte)((startAdd & 0x00ff0000) >> 16), (byte)((startAdd & 0x0000ff00) >> 8), (byte)(startAdd & 0x000000ff), (ushort)(endAdd - startAdd), data);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            for (int i = 0; i < data.Length; i++)
            {
                textBox3.Text += Convert.ToString(i.ToString() + ": " + Convert.ToString(data[i])) + '\n';
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //bkgdWork.RunWorkerAsync();
            timer1.Enabled=true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            textBox3.Text = "";
        }
        delegate void newDelegate(string a);
        void bkgdWork_DoWork(object sender, DoWorkEventArgs e)
        {
            TB4.theDataDebug.Invoke(new Action(delegate() { pullData(dumpPathname); }));
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //bkgdWork.CancelAsync();
            timer1.Enabled = false;
        }

        bool[] isChannelSelected = new bool[16];
        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //show channel selector
            Form channelForm = new Form();
            channelForm.Size=new Size(300, 110);
            TextBox newBox = new TextBox();
            newBox.Size = new Size(100, 15);
            newBox.Location = new Point(100, 20);
            channelForm.Controls.Add(newBox);

            Button submitButton=new Button();
            submitButton.Text="Submit";
            submitButton.Size = new Size(60, 20);
            submitButton.Location = new Point(channelForm.Size.Width / 2 - 30, 60);

            submitButton.Click += new EventHandler((object sender2, EventArgs e2) => { activeChannel = UInt16.Parse(newBox.Text); channelForm.Hide(); });

            channelForm.Controls.Add(submitButton);
            channelForm.Show();
        }
        private void registerModeLabelSwitch(bool registerModeOn)
        {
            if (registerModeOn)
            {
                label4.Text = "Mode";
                label1.Text = "Register Name";
                startBox.Text = "PADE_TEMP";

                label3.Hide();
                label2.Hide();
                numRegBox.Hide();
                endBox.Hide();

                endSubmit.Hide();
                numRegButton.Hide();

                regComboBox.Show();
                regComboBox.SelectedIndex = 0;
                startBox.Hide();

                compressionCombo.Items.Clear();
                compressionCombo.Items.Add("Reg Val vs. Board");
                compressionCombo.Items.Add("Reg Val vs. Time");
            }
            else
            {
                label4.Text = "Compression Mode";
                label1.Text = "Start Address:";
                startBox.Text = "";

                label3.Show();
                label2.Show();
                numRegBox.Show();
                endBox.Show();

                endSubmit.Show();
                numRegButton.Show();

                regComboBox.Hide();
                startBox.Show();

                compressionCombo.Items.Clear();
                compressionCombo.Items.Add("Floating Point");
                compressionCombo.Items.Add("Divide By Two");
                compressionCombo.Items.Add("Divide By Four");

            }
        
        
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            registerModeLabelSwitch(true);
            zg1.GraphPane.Title.Text = "Reg Val vs. Board";
            compressionCombo.SelectedIndex = 0;
            zg1.AxisChange();
            zg1.Invalidate();
        }

        private void compressionCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (compressionCombo.Text == "Reg Val vs. Time")
            {
                GraphItem = mainPane.AddCurve(regComboBox.Text, pointlist, Color.Red, SymbolType.None);

                
                mainPane.XAxis.Title.Text = "Time";
                mainPane.XAxis.Type = AxisType.Date;
                mainPane.XAxis.Scale.Format = "HH:mm:ss";
                mainPane.YAxis.Title.Text = "Value";
                mainPane.YAxis.Scale.BaseTic = 5;



            }
        }

        private void populateRegComboBox()
        {
            regComboBox.Items.Clear();
            foreach (TB4_Register reg in TB4.TB4_Registers)
            {
                if(reg!=null) regComboBox.Items.Add(reg.name);
            }

        }

       
    }
}
