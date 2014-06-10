using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PADE
{
    public partial class DisplayControl : Form
    {
        public DisplayControl()
        {
            InitializeComponent();
            this.Text = "PAD";
        }

        private void DisplayControl_Load(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (TB4.theRunForm.Visible)
            {
                TB4.theRunForm.Visible = false;
            }
            else
            {
                TB4.theRunForm.Visible = true;
                TB4.theRunForm.Activate();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (TB4.theRegistersForm.Visible)
            {
                TB4.theRegistersForm.Visible = false;
            }
            else
            {
                TB4.theRegistersForm.Visible = true;
                TB4.theRegistersForm.Activate();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (TB4.theArraysForm.Visible)
            {
                TB4.theArraysForm.Visible = false;
            }
            else
            {
                TB4.theArraysForm.Visible = true;
                //TB4.theArraysForm.Activate();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (TB4.thePlot.Visible)
            {
                TB4.thePlot.Visible = false;
            }
            else
            {
                TB4.thePlot.Visible = true;
                TB4.thePlot.Activate();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (TB4.theFlash.Visible)
            {
                TB4.theFlash.Visible = false;
            }
            else
            {
                TB4.theFlash.Visible = true;
                TB4.theFlash.Activate();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (TB4.theBiasOffset.Visible)
            {
                TB4.theBiasOffset.Visible = false;
            }
            else
            {
                TB4.theBiasOffset.Visible = true;
                TB4.theBiasOffset.Activate();           
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (TB4.theHist1.Visible)
            {
                TB4.theHist.Visible = false;
                TB4.theHist1.Visible = false;
            }
            else 
            {
                //TB4.theHist.Visible = true;
                //TB4.theHist.Activate();
                TB4.theHist1.Visible = true;
                TB4.theHist1.Activate();
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (TB4.theGBE.Visible) { TB4.theGBE.Visible = false; }
            else { TB4.theGBE.Visible = true; }

        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (TB4.theDRAM.Visible) { TB4.theDRAM.Visible = false; }
            else { TB4.theDRAM.Visible = true; }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (TB4.theHist_and_Scan.Visible) { TB4.theHist_and_Scan.Visible = false; }
            else { TB4.theHist_and_Scan.Visible = true; }
        }

    }
}
