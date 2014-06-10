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
    public partial class TempPlot : DockContent
    {

        
        public TempPlot()
        {
            InitializeComponent();
        }

        #region Form Events
        public static bool formOpen = false;

        void TempPlot_VisibleChanged(object sender, System.EventArgs e)
        {
            formOpen = !formOpen;

            if (formOpen)
            {

                if (TB4.ActivePADE != null) //it shouldn't ever be null, because this form is called by a form that must activate a PADE
                {
                    updateFields(TB4.ActivePADE);
                    this.Location = new Point(TB4.thePADE_explorer.Location.X + PADE_explorer.subFormLocation.X, TB4.thePADE_explorer.Location.Y + PADE_explorer.subFormLocation.Y);
                }

                this.Activate();
                Console.WriteLine("Temp Plot Shown");
                this.BringToFront();
                if (this.Parent != null)
                {
                    Console.WriteLine(this.Parent.ToString());
                }
            }
            else
            {
                if (TB4.thePADE_explorer.openForms.Contains(new PADE_explorer.formPair(TB4.theTempPlot.GetType(), TB4.theTempPlot)))
                {
                    TB4.thePADE_explorer.openForms.Remove(new PADE_explorer.formPair(TB4.theTempPlot.GetType(), TB4.theTempPlot));
                    
                    
                }
            }

            
        }
        private void TempPlot_Load(object sender, EventArgs e)
        {
            unitsCombo.Items.Add("Celsius");
            unitsCombo.Items.Add("Fahrenheit");
            unitsLabel1.Text = unitsLabel3.Text = unitsLabel4.Text="";
            //updateFields(TB4.ActivePADE);

            Size returnSize = this.Size;
            createGraph(TB4.ActivePADE.Temperature); this.DockStateChanged += (object a, System.EventArgs b) => { TB4.thePADE_explorer.childChangedDockstate(this, returnSize); };

        }
        void TempPlot_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            //TB4.thePADE_explorer.childClosed(this);
            //e.Cancel = true;
        }

        private void unitsCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            PADE.tempUnits = unitsCombo.Text;
            unitsLabel1.Text = unitsLabel3.Text = unitsLabel4.Text = unitsCombo.Text;
        }

        private void periodUpDown_ValueChanged(object sender, EventArgs e)
        {
            PADE.samplePeriod = (int)periodUpDown.Value;
        }

        private void sampleSizeUpDown_ValueChanged(object sender, EventArgs e)
        {
            TB4.ActivePADE.sampleSize = (UInt16)sampleSizeUpDown.Value;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            TB4.ActivePADE.analysisEnabled = checkBox2.Checked;
        }

        #endregion

        #region Graphing Control
        LineItem GraphItem;
        ZedGraph.GraphPane mainPane;
        PointPairList pointlist = new PointPairList();



        public void createGraph(Queue<tempStamp> queueArg)
        {
            
            mainPane = zGraph.GraphPane;
            GraphItem = mainPane.AddCurve("Temperature", pointlist, Color.Red, SymbolType.None);

            //mainPane.Title.Text = "PADE" + TB4.ActivePADE.PADE_sn + " Temperature";
            mainPane.XAxis.Title.Text = "Time";
            mainPane.XAxis.Type = AxisType.Date;
            mainPane.XAxis.Scale.Format = "HH:mm:ss";
            mainPane.YAxis.Title.Text = "Temperature";
            mainPane.YAxis.Scale.BaseTic = 5;
            pointsFromQueue(queueArg);
            updateGraph();

        }

        public void addPoint(tempStamp arg, bool updateAfterAdd)
        {
            GraphItem.AddPoint(new XDate(arg.time), arg.temperature);
            if (updateAfterAdd)
            {
                updateGraph();
            }
            Console.WriteLine("Point Added.");
        }

        private void pointsFromQueue(Queue<tempStamp> queueArg)
        {
            GraphItem.Clear();
          
            for (int i = 0; i < queueArg.Count; i++)
            {
                addPoint(queueArg.ElementAt<tempStamp>(i), false);
            }
            updateGraph();
            
        }

        private void updateGraph()
        {
            mainPane.AxisChange();
            zGraph.Invalidate();
        }
        #endregion

        #region Data Control

        /// <summary>
        /// Copy relevant data to the PADE settings and Analysis Group Boxes
        /// </summary>
        /// <param name="sourcePADE"></param>
        private void updateFields(PADE sourcePADE)
        {
            
            checkBox1.Checked = sourcePADE.recordTemperature;
            checkBox2.Checked = sourcePADE.analysisEnabled;
            sampleSizeUpDown.Value = sourcePADE.sampleSize;
            periodUpDown.Value = PADE.samplePeriod;
            unitsCombo.Text = unitsLabel1.Text = unitsLabel3.Text = unitsLabel4.Text = PADE.tempUnits;
            
            this.Text = "PADE" +sourcePADE.PADE_sn + " Temperature Plot";
            
            //change some graph characteristics
            mainPane.Title.Text = "PADE" + sourcePADE.PADE_sn + " Temperature"; 
           

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            TB4.ActivePADE.recordTemperature = checkBox1.Checked;
        }

        #endregion
        Random rand = new Random();

        private void button1_Click(object sender, EventArgs e)
        {
            PADE_explorer.saveGraphImage(zGraph);
        }

        
    

        


    }
}
