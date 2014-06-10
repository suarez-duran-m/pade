using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using System.IO;


namespace PADE
{
    public partial class Registers : DockContent
    {
        public int returnBase = 16;
        public static string registerWriteHistory = DateTime.Now.ToShortDateString()+"\n";
        private List<Button> regList = new List<Button>();

        private System.Windows.Forms.Label[] RegisterIndex = new System.Windows.Forms.Label[500];
        private System.Windows.Forms.Label[] RegisterLabel = new System.Windows.Forms.Label[500];
        private System.Windows.Forms.TextBox[] RegisterValue = new System.Windows.Forms.TextBox[500];
        private System.Windows.Forms.Button[] RegisterWrite = new System.Windows.Forms.Button[500];
        private System.Windows.Forms.Button[] RegisterRead = new System.Windows.Forms.Button[500];
        public System.Windows.Forms.Button[] RegisterWriteAll = new System.Windows.Forms.Button[500];

        public Registers()
        {
            InitializeComponent();
            regDescriptionsFromExcel();
            createButtons();
        }

        private void Registers_Load(object sender, EventArgs e)
        {
            TB4.theRegistersForm = this;
        }
        void Registers_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            TB4.thePADE_explorer.childClosed(this);
            e.Cancel = true;
        }


        private void createButtons()
        {
            int[,] reg_count = new int[7, 7];
            System.Windows.Forms.TabPage[,] myTab = new TabPage[7, 7];




            for (int ii = 0; ii < 1; ii++)
            {
                for (int jj = 0; jj < 7; jj++)
                {

                    reg_count[ii, jj] = 0;
                    if (ii == 4)
                    {
                        myTab[ii, jj] = null;// MB;
                    }
                    else
                    {
                        switch (ii * 10 + jj)
                        {
                            case 00: myTab[ii, jj] = DB0_MAIN;
                                break;
                            case 01: myTab[ii, jj] = DB0_BIAS;
                                break;
                            case 02: myTab[ii, jj] = DB0_FLASH;
                                break;
                            case 03: myTab[ii, jj] = DB0_EXPERT;
                                break;
                            case 04: myTab[ii, jj] = DB0_ZS;
                                break;
                            case 05: myTab[ii, jj] = DB0_THR;
                                break;
                            case 06: myTab[ii, jj] = DB0_ETH;
                                break;
                            //case 10: myTab[ii, jj] = DB1_MAIN;
                            //    break;
                            //case 20: myTab[ii, jj] = DB2_MAIN;
                            //    break;
                            //case 30: myTab[ii, jj] = DB3_MAIN;
                            //    break;
                            //case 11: myTab[ii, jj] = DB1_BIAS;
                            //    break;
                            //case 21: myTab[ii, jj] = DB2_BIAS;
                            //    break;
                            //case 31: myTab[ii, jj] = DB3_BIAS;
                            //    break;
                            //case 12: myTab[ii, jj] = DB1_FLASH;
                            //    break;
                            //case 22: myTab[ii, jj] = DB2_FLASH;
                            //    break;
                            //case 32: myTab[ii, jj] = DB3_FLASH;
                            //    break;
                            //case 13: myTab[ii, jj] = DB1_EXPERT;
                            //    break;
                            //case 23: myTab[ii, jj] = DB2_EXPERT;
                            //    break;
                            //case 33: myTab[ii, jj] = DB3_EXPERT;
                            //    break;
                        }
                    }
                }
            }
            int i = 0;
            for (int j = 1; j < TB4.TB4_Registers.Length - 1; j++)
            //for (int i = 1; i < 10; i++)
            {
                int ypos = -20;
                int xpos = 0;
                int t1 = 0;
                int t2 = 0;

                if (TB4.TB4_Registers[j] == null)
                {
                    //then nothing
                }
                else
                {
                    if (TB4.TB4_Registers[j].comment == "MB")
                    { t1 = 4; this.tabControl1.SelectTab("MB"); }
                    if (TB4.TB4_Registers[j].comment == "DB0")
                    { t1 = 0; this.tabControl1.SelectTab("DB0"); }
                    if (TB4.TB4_Registers[j].comment == "DB1")
                    { t1 = 1; this.tabControl1.SelectTab("DB1"); }
                    if (TB4.TB4_Registers[j].comment == "DB2")
                    { t1 = 2; this.tabControl1.SelectTab("DB2"); }
                    if (TB4.TB4_Registers[j].comment == "DB3")
                    { t1 = 3; this.tabControl1.SelectTab("DB3"); }
                    if (TB4.TB4_Registers[j].display_tab == "MAIN")
                    {
                        t2 = 0;
                    }
                    if (TB4.TB4_Registers[j].display_tab == "BIAS")
                    {
                        t2 = 1;
                    }
                    if (TB4.TB4_Registers[j].display_tab == "FLASH")
                    {
                        t2 = 2;
                    }
                    if (TB4.TB4_Registers[j].display_tab == "EXPERT")
                    {
                        t2 = 3;
                    }
                    if (TB4.TB4_Registers[j].display_tab == "ZS")
                    {
                        t2 = 4; t1 = 0;
                    }
                    if (TB4.TB4_Registers[j].display_tab == "THR")
                    {
                        t2 = 5; t1 = 0;
                    }
                    if (TB4.TB4_Registers[j].display_tab == "ETH")
                    {
                        t2 = 6; t1 = 0;
                    }
                    reg_count[t1, t2]++;
                    ypos += 25 * reg_count[t1, t2];

                    i++;
                    //if (i > 40) { ypos += 25 * (i - 40); xpos = 450; } else { ypos += 25 * i; xpos = 0; }
                    if (ypos + 10 > tabControl1.Height)
                    {

                        tabControl1.Height += 25;
                        DB0_subtab_control.Height += 25;
                    }
                    xpos = 0;

                    RegisterIndex[i] = new System.Windows.Forms.Label();
                    RegisterIndex[i].Name = "RegIndex" + i.ToString();
                    RegisterIndex[i].Location = new System.Drawing.Point(xpos, ypos);
                    RegisterIndex[i].Size = new System.Drawing.Size(5, 20);
                    RegisterIndex[i].Text = j.ToString();
                    RegisterIndex[i].Visible = false;




                    RegisterLabel[i] = new System.Windows.Forms.Label();
                    RegisterLabel[i].Name = "RegLabel" + i.ToString();
                    RegisterLabel[i].Location = new System.Drawing.Point(5 + xpos, ypos);
                    RegisterLabel[i].Size = new System.Drawing.Size(400, 20);
                    string t = "";
                    t = Convert.ToString(TB4.TB4_Registers[j].addr, 16) + "   ";
                    t += TB4.TB4_Registers[j].name;
                    RegisterLabel[i].Text = t;

                    //associate the excel file details
                    System.Windows.Forms.ToolTip ttip = new System.Windows.Forms.ToolTip();
                    string descr = PADE_explorer.registerLookup(TB4.TB4_Registers[j].name).verbose_description;
                    if (descr == "") descr = "No information available for this register.";
                    ttip.SetToolTip(RegisterLabel[i], descr);





                    RegisterValue[i] = new System.Windows.Forms.TextBox();
                    RegisterValue[i].Name = "RegValue" + i.ToString();
                    RegisterValue[i].Location = new System.Drawing.Point(410 + xpos, ypos);
                    RegisterValue[i].Size = new System.Drawing.Size(70, 20);

                    RegisterRead[i] = new System.Windows.Forms.Button();
                    RegisterRead[i].Name = "RegRead" + i.ToString();
                    RegisterRead[i].Location = new System.Drawing.Point(490 + xpos, ypos);
                    RegisterRead[i].Size = new System.Drawing.Size(30, 20);
                    RegisterRead[i].Text = "R";
                    regList.Add(RegisterRead[i]);

                    RegisterRead[i].Click += new EventHandler(RegisterRead_Click);
                    if (TB4.TB4_Registers[j].writeOnly)
                    { RegisterRead[i].Visible = false; }
                    else
                    { RegisterRead[i].Visible = true; }

                    RegisterWrite[i] = new System.Windows.Forms.Button();
                    RegisterWrite[i].Name = "RegWrite" + i.ToString();
                    RegisterWrite[i].Location = new System.Drawing.Point(530 + xpos, ypos);
                    RegisterWrite[i].Size = new System.Drawing.Size(30, 20);
                    RegisterWrite[i].Text = "W";
                    RegisterWrite[i].Click += new EventHandler(RegisterWrite_Click);
                    if (TB4.TB4_Registers[j].readOnly)
                    { RegisterWrite[i].Visible = false; }
                    else
                    { RegisterWrite[i].Visible = true; }


                    RegisterWriteAll[i] = new System.Windows.Forms.Button();
                    RegisterWriteAll[i].Name = "RegWriteAll" + i.ToString();
                    RegisterWriteAll[i].Location = new System.Drawing.Point(570 + xpos, ypos);
                    RegisterWriteAll[i].Size=new System.Drawing.Size(50, 20);
                    RegisterWriteAll[i].Text="W/all";
                    RegisterWriteAll[i].Click +=new EventHandler(RegisterWriteAll_Click);
                    if (TB4.TB4_Registers[j].readOnly)
                    { RegisterWriteAll[i].Visible = false; }
                    else
                    { RegisterWriteAll[i].Visible = true; }

                    //if (TB4.TB4_Registers[j].comment == "MB")
                    //{
                    //    MB.Controls.Add(RegisterLabel[i]);
                    //    MB.Controls.Add(RegisterValue[i]);
                    //    MB.Controls.Add(RegisterRead[i]);
                    //    MB.Controls.Add(RegisterWrite[i]);
                    //}
                    //if (TB4.TB4_Registers[j].comment == "DB0")
                    //else
                    {
                        myTab[t1, t2].Controls.Add(RegisterLabel[i]);
                        myTab[t1, t2].Controls.Add(RegisterValue[i]);
                        myTab[t1, t2].Controls.Add(RegisterRead[i]);
                        myTab[t1, t2].Controls.Add(RegisterWrite[i]);
                        myTab[t1, t2].Controls.Add(RegisterWriteAll[i]);
                        //    DB0.Controls.Add(RegisterLabel[i]);
                        //    DB0.Controls.Add(RegisterValue[i]);
                        //    DB0.Controls.Add(RegisterRead[i]);
                        //    DB0.Controls.Add(RegisterWrite[i]);
                    }
                    //if (TB4.TB4_Registers[j].comment == "DB1")
                    //{
                    //    DB1.Controls.Add(RegisterLabel[i]);
                    //    DB1.Controls.Add(RegisterValue[i]);
                    //    DB1.Controls.Add(RegisterRead[i]);
                    //    DB1.Controls.Add(RegisterWrite[i]);
                    //}
                    //if (TB4.TB4_Registers[j].comment == "DB2")
                    //{
                    //    DB2.Controls.Add(RegisterLabel[i]);
                    //    DB2.Controls.Add(RegisterValue[i]);
                    //    DB2.Controls.Add(RegisterRead[i]);
                    //    DB2.Controls.Add(RegisterWrite[i]);
                    //}
                    //if (TB4.TB4_Registers[j].comment == "DB3")
                    //{
                    //    DB3.Controls.Add(RegisterLabel[i]);
                    //    DB3.Controls.Add(RegisterValue[i]);
                    //    DB3.Controls.Add(RegisterRead[i]);
                    //    DB3.Controls.Add(RegisterWrite[i]);
                    //}


                }
            }

            Console.WriteLine("REGISTER FORM INITIALIZED.");

        }

        void RegisterWriteAll_Click(object sender, EventArgs e)
        {
            int i = 0; int j = 0;
            System.Windows.Forms.Button thisButton = new Button();
            thisButton = (Button)sender;
            i = Array.IndexOf(RegisterWriteAll, thisButton);
            j = Convert.ToInt16(RegisterIndex[i].Text);
            UInt16 ui_data = 0;
            try
            {
                ui_data = Convert.ToUInt16(this.RegisterValue[i].Text, 16);
            }
            catch
            {
                ui_data = 0;
            }
            PADE currentActivePADE=TB4.ActivePADE;

            foreach (KeyValuePair<int, PADE> pade in TB4.PADE_List)
            {
                TB4.activatePADE(pade.Value, false, false);
                TB4.TB4_Registers[j].RegWrite(ui_data);
            }
            TB4.activatePADE(currentActivePADE);
        }
        private void RegisterWrite_Click(object sender, EventArgs e)
        {
            int i = 0; int j = 0;
            System.Windows.Forms.Button thisButton = new Button();
            thisButton = (Button)sender;
            i = Array.IndexOf(RegisterWrite, thisButton);
            j = Convert.ToInt16(RegisterIndex[i].Text);
            UInt16 ui_data = 0;
            try
            {
                ui_data = Convert.ToUInt16(this.RegisterValue[i].Text, 16);
            }
            catch
            {
                ui_data = 0;
            }

            TB4.TB4_Registers[j].RegWrite(ui_data);

            if (TB4.TB4_Registers[j].name.Contains("TRIG_THRESHOLD")) { TB4.myRun.glbTrig_Level = ui_data; }
            if (TB4.TB4_Registers[j].name.Contains("NOISE_THRESHOLD")) { TB4.myRun.glbNoise_Level = ui_data; }

            if(TB4.TB4_Registers[j].name.ToUpper().Contains("BIAS"))
            {
                registerWriteHistory += DateTime.Now.ToShortDateString() + ": " + TB4.TB4_Registers[j].name + " = " + ui_data.ToString() + "\n"; 
            }
        }

        private void RegisterRead_Click(object sender, EventArgs e)
        {
            int i = 0; int j = 0;
            System.Windows.Forms.Button thisButton = new Button();
            thisButton = (Button)sender;
            i = Array.IndexOf(RegisterRead, thisButton);
            j = Convert.ToInt16(RegisterIndex[i].Text);
            UInt16 val = 0;
            val = TB4.TB4_Registers[j].RegRead();

            if (TB4.TB4_Registers[j].name == "PADE_TEMP")
            {
                val = Convert.ToUInt16((double)val * 9 / 80 + 32);
            }

            if (returnBase == 16) this.RegisterValue[i].Text = "0x" + Convert.ToString(val, returnBase);
            else this.RegisterValue[i].Text = Convert.ToString(val, returnBase);
        }

        public Int16 convertNum(string hexNum)
        {

            return Convert.ToInt16(hexNum, returnBase);
        }

        public string findRegDescription(string regAddr)
        {
            //Microsoft.Office.Interop.Excel.Worksheet XLwksht = WB.Worksheets.Item[0];

            //for (int i = 0; i < 200; i++) //200 is an arbitrary "big" number
            //{
            //    if (XLwksht.Range["A" + i + 3].Value() == regAddr)
            //    {
            //        return XLwksht.Range["I" + i + 3].Value();
            //    }
            //}
            return null;
        }

        private void readAllRegistersToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < regList.Count; i++)
            {
                RegisterRead_Click(regList[i], null);
            }

        }

        private void regDescriptionsFromExcel()
        {

            FileStream file;
            StreamReader sr;

            try
            {
                // Specify file, instructions, and privelegdes
                file = new FileStream(PADE_explorer.excelPathname, FileMode.Open, FileAccess.Read);
                // Create a new stream to read from a file
                sr = new StreamReader(file);
            }
            catch (Exception ex)
            {
                TB4_Exception.logError(ex, "Error trying to init tooltips - missing file", true);
                return;
            };

            string[] delimeter = new string[64];
            string[] token = new string[64];

            delimeter[0] = "<=";
            delimeter[1] = "//";
            delimeter[2] = ";";
            delimeter[3] = "=";
            delimeter[4] = "set ";
            delimeter[5] = "dec";
            delimeter[6] = ",";

            int line_index = 0;
            while (sr.EndOfStream == false)
            {
                string lineText = sr.ReadLine();
                line_index++;
                if (line_index > 2)
                {
                    try
                    {
                        token = lineText.Split(delimeter, System.StringSplitOptions.None);

                        UInt32 DH;
                        try
                        {
                            DH = Convert.ToUInt32(token[0], 16);
                            TB4_Register tempReg = PADE_explorer.regAddLookup(DH);
                            if (tempReg != null) tempReg.verbose_description = token[1] + ": " + token[8];
                        }
                        catch { }

                    }

                    catch (Exception ex)
                    {
                        TB4_Exception.logError(ex, "Error parsing the register CSV file", true);
                    }
                }
            }

            sr.Close();
            file.Close();
        }


    }
}
