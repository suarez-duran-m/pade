using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace PADE
{
    public partial class DRAM : DockContent
    {
        public DRAM()
        {
            InitializeComponent();
        }


        private void btnREAD_Click(object sender, EventArgs e)
        {

            TB4_Register regFIFO_READ_LOWER = new TB4_Register("FIFO_READ_LOWER", "temp", 0x00000000, 16, false, false);
            TB4_Register regFIFO_READ_UPPER = new TB4_Register("FIFO_READ_UPPER", "temp", 0x00000000, 16, false, false);
            TB4_Register regDDR_ADDR_LOWER = new TB4_Register("DDR_ADDR_LOWER", "temp", 0x00000000, 16, false, false);
            TB4_Register regDDR_ADDR_UPPER = new TB4_Register("DDR_ADDR_UPPER", "temp", 0x00000000, 16, false, false);
            TB4_Register regDDR_READ_BURST = new TB4_Register("DDR_READ_BURST", "temp", 0x00000000, 16, false, false); //(5 downto 0)

            for (int i = 0; i < TB4.TB4_Registers.Length; i++)
            {
                if (TB4.TB4_Registers[i] != null)
                {
                    if (TB4.TB4_Registers[i].name.Contains("DDR_FIFO_READ_LOWER"))
                    { regFIFO_READ_LOWER = TB4.TB4_Registers[i]; }
                    if (TB4.TB4_Registers[i].name.Contains("DDR_FIFO_READ_UPPER"))
                    { regFIFO_READ_UPPER = TB4.TB4_Registers[i]; }
                    if (TB4.TB4_Registers[i].name.Contains("DDR_ADDR_LOWER"))
                    { regDDR_ADDR_LOWER = TB4.TB4_Registers[i]; }
                    if (TB4.TB4_Registers[i].name.Contains("DDR_ADDR_UPPER"))
                    { regDDR_ADDR_UPPER = TB4.TB4_Registers[i]; }
                    if (TB4.TB4_Registers[i].name.Contains("DDR_READ_PORT_EXECUTE"))
                    { regDDR_READ_BURST = TB4.TB4_Registers[i]; }
                }
            }


            Stream fname;

            saveFileDialog1.Filter = "DRAM files (*.dram)|*.dram|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if ((fname = saveFileDialog1.OpenFile()) != null)
                {
                    StreamWriter sw = new StreamWriter(fname);
                    long max = Convert.ToUInt32(txtLen.Text);
                    max = max & 0x3fffffff;

                    string l;
                    UInt32 lw;
                    long ind = 0;
                    long lAddr = 0;
                    UInt16 uiAddr_lower = 0;
                    UInt16 uiAddr_upper = 0;

                    string t = txtStartingADDR.Text;
                    t = t.ToLower();
                    if (t.Contains("x"))
                    {
                        t = "0" + t.Substring(t.LastIndexOf("x"));
                        lAddr = Convert.ToUInt32(t, 16);
                    }
                    else { lAddr = Convert.ToUInt32(t); }

                    uiAddr_lower = (UInt16)(lAddr & 0xffff);
                    uiAddr_upper = (UInt16)(lAddr >> 16 & 0xffff);
                    regDDR_ADDR_LOWER.RegWrite(uiAddr_lower);
                    regDDR_ADDR_UPPER.RegWrite(uiAddr_upper);

                    for (long i = 0; i < max; i++)
                    {
                        
                        if (ind == 0)//neat to pull DRAM
                        {
                            regDDR_READ_BURST.RegWrite(31);//heck, why not...
                        }
                        ind++;
                        if (ind == 32) 
                        { 
                            ind = 0;
                            lAddr += 32;
                            uiAddr_lower = (UInt16)(lAddr & 0xffff);
                            uiAddr_upper = (UInt16)(lAddr >> 16 & 0xffff);
                            regDDR_ADDR_LOWER.RegWrite(uiAddr_lower);
                            regDDR_ADDR_UPPER.RegWrite(uiAddr_upper);
                        }

                        lw = Convert.ToUInt32(regFIFO_READ_LOWER.RegRead());
                        lw = 0x10000 * Convert.ToUInt32(regFIFO_READ_UPPER.RegRead()) +lw ;
                        t ="0x"+ Convert.ToString(lw,16);
                        sw.WriteLine(t);
                        lblCurrent.Text = i.ToString();
                        //System.Threading.Thread.Sleep(1);
                        Application.DoEvents();
                    }
                    sw.Close();
                    lblCurrent.Text = "";
                }
            }
        }

        private void btnWRITE_Click(object sender, EventArgs e)
        {
            string l;
            UInt32 lw;
            long ind = 0;
            long lAddr = 0;
            UInt16 uiAddr_lower = 0;
            UInt16 uiAddr_upper = 0;

            TB4_Register regFIFO_WRITE_LOWER = new TB4_Register("FIFO_WRITE_LOWER", "temp", 0x00000000, 16, false, false);
            TB4_Register regFIFO_WRITE_UPPER = new TB4_Register("FIFO_WRITE_UPPER", "temp", 0x00000000, 16, false, false);
            TB4_Register regDDR_ADDR_LOWER = new TB4_Register("DDR_ADDR_LOWER", "temp", 0x00000000, 16, false, false);
            TB4_Register regDDR_ADDR_UPPER = new TB4_Register("DDR_ADDR_UPPER", "temp", 0x00000000, 16, false, false);
            TB4_Register regDDR_WRITE_BURST = new TB4_Register("DDR_WRITE_BURST", "temp", 0x00000000, 16, false, false); //(5 downto 0)

            for (int i = 0; i < TB4.TB4_Registers.Length; i++)
            {
                if (TB4.TB4_Registers[i] != null)
                {
                    if (TB4.TB4_Registers[i].name.Contains("DDR_FIFO_WRITE_LOWER"))
                    { regFIFO_WRITE_LOWER = TB4.TB4_Registers[i]; }
                    if (TB4.TB4_Registers[i].name.Contains("DDR_FIFO_WRITE_UPPER"))
                    { regFIFO_WRITE_UPPER = TB4.TB4_Registers[i]; }
                    if (TB4.TB4_Registers[i].name.Contains("DDR_ADDR_LOWER"))
                    { regDDR_ADDR_LOWER = TB4.TB4_Registers[i]; }
                    if (TB4.TB4_Registers[i].name.Contains("DDR_ADDR_UPPER"))
                    { regDDR_ADDR_UPPER = TB4.TB4_Registers[i]; }
                    if (TB4.TB4_Registers[i].name.Contains("DDR_WRITE_PORT_EXECUTE"))
                    { regDDR_WRITE_BURST = TB4.TB4_Registers[i]; }
                }
            }

            
            string t = txtStartingADDR.Text;
            t = t.ToLower();
            if (t.Contains("x"))
            {
                t = "0" + t.Substring(t.LastIndexOf("x"));
                lAddr = Convert.ToUInt32(t, 16);
            }
            else { lAddr = Convert.ToUInt32(t); }

            uiAddr_lower = (UInt16)(lAddr & 0xffff);
            uiAddr_upper = (UInt16)(lAddr>>16 & 0xffff);
            regDDR_ADDR_LOWER.RegWrite(uiAddr_lower);
            regDDR_ADDR_UPPER.RegWrite(uiAddr_upper);

            openFileDialog1.Filter = "DRAM files|*.dram";
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string filename = openFileDialog1.FileName;
                StreamReader sr = new StreamReader(filename);

                while (!sr.EndOfStream)
                {
                    l = sr.ReadLine();
                    l = l.ToLower();
                    if (l.Contains("x"))
                    {
                        l ="0"+ l.Substring(l.LastIndexOf("x"));
                        lw = Convert.ToUInt32(l, 16);
                    }
                    else
                    {
                        try
                        { lw = Convert.ToUInt32(l); }
                        catch { lw = 666; }
                    }
                    ind++;
                    
                    regFIFO_WRITE_LOWER.RegWrite( (UInt16)(lw & 0xffff));
                    regFIFO_WRITE_UPPER.RegWrite((UInt16)((lw>>16) & 0xffff));
                    lblCurrent.Text = (lAddr +ind).ToString();
                    if (ind == 32)//max burst len
                    {
                        regDDR_WRITE_BURST.RegWrite(31);
                        ind = 0;
                        ind = 0;
                        lAddr += 32;
                        uiAddr_lower = (UInt16)(lAddr & 0xffff);
                        uiAddr_upper = (UInt16)(lAddr >> 16 & 0xffff);
                        regDDR_ADDR_LOWER.RegWrite(uiAddr_lower);
                        regDDR_ADDR_UPPER.RegWrite(uiAddr_upper);
                    }
                    System.Threading.Thread.Sleep(1);
                    Application.DoEvents();
                }
                //any leftovers? write them
                if (ind > 0)
                {
                    UInt16 u = Convert.ToUInt16(ind - 1);
                    regDDR_WRITE_BURST.RegWrite(u);
                    ind = 0;
                }
                sr.Close();
                lblCurrent.Text = "";
            }
        }

        private void DRAM_Load(object sender, EventArgs e)
        {
            TB4.theDRAM = this;
            Size returnSize = this.Size;
            this.DockStateChanged += (object a, System.EventArgs b) => { TB4.thePADE_explorer.childChangedDockstate(this, returnSize); };
        }

        void DRAM_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            TB4.thePADE_explorer.childClosed(this);
            e.Cancel = true;
        }

        private void btn_MODE_Click(object sender, EventArgs e)
        {

        }

    }
}
