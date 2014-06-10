using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using WeifenLuo.WinFormsUI.Docking;
using System.Reflection;

namespace PADE
{
    public partial class SystemViewer : DockContent
    {
        public Point mouseLoc;
        public static Size canvasSize;
        public static Point canvasLocation;
        Graphics graphicObj;

        #region Form Initialization
        bool startup = true;
        public SystemViewer()
        {
            
            InitializeComponent();

            boxSee1.Initialize();
            //createFakeClusters();
            boxSee1.initializeShapes();

            this.Invalidate();
            this.Update();

            
        }

        void SystemViewer_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
            boxSee1.Dispose();
        }

        private void SystemViewer_Load(object sender, EventArgs e)
        {
            
            Invalidate();
            Update();
            this.Show();
            Size returnSize = this.Size;
            this.DockStateChanged += (object a, System.EventArgs b) => { TB4.thePADE_explorer.childChangedDockstate(this, returnSize); };
        
        }

        void createFakeClusters()
        {
            int numberOfPADES = 20;
            int numberofClusters = 3;

            for (int i = 0; i < numberofClusters; i++)
            {
                padeCluster newCluster = new padeCluster(Color.Blue, "Cluster " + i, boxSee1);

                for (int j = 0; j < numberOfPADES; j++)
                {

                    PADE newPADE = new PADE();
                    newPADE.PADE_sn = (j + i * numberOfPADES).ToString();
                    if (i == 2 && j >16) { }
                    else  padeCluster.clusterList[i].addNode(Color.Blue, newPADE);
                }
            }

        }
        #endregion
        

        


        


    }





}
