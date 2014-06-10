using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using WeifenLuo.WinFormsUI.Docking;


namespace PADE
{
    public partial class PerformanceLog : DockContent
    {
        public PerformanceLog()
        {
            InitializeComponent();
        }

        private void PerformanceLog_Load(object sender, EventArgs e)
        {
            
              
        }

        private void buildTable()
        {
            perfView.View = View.Details;
            perfView.Columns.Add("Name");
            perfView.Columns.Add("Description");
            perfView.Columns.Add("Minimum");
            perfView.Columns.Add("Maximum");
            perfView.Columns.Add("Average");

            //perfView.Groups[0].Name = "Test";

        }

        void PerformanceLog_VisibleChanged(object sender, System.EventArgs e)
        {
            perfView.Items.Clear();
            buildTable();

            foreach (TB4_PerfMon mon in TB4_PerfMon.performanceList)
            {
                string[] row = { mon._name, mon._description, (mon.getMinimum() * 1000 / TimeSpan.TicksPerMillisecond).ToString(), (mon.getMaximum() * 1000 / TimeSpan.TicksPerMillisecond).ToString(), (mon.getAverage() * 1000 / TimeSpan.TicksPerMillisecond).ToString() };
                ListViewItem newItem = new ListViewItem(row);
                perfView.Items.Add(newItem);
            }

            for (int i = 0; i < perfView.Columns.Count; i++) perfView.Columns[i].AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
        }



        
    }
}
