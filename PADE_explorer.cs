using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Collections.Generic;
using System.IO;

using PcapDotNet.Core;
using PcapDotNet.Packets;
using PcapDotNet.Packets.Ethernet;
using PcapDotNet.Packets.IpV4;
using PcapDotNet.Packets.Icmp;
using PcapDotNet.Packets.Transport;

using WeifenLuo.WinFormsUI.Docking;

using ZedGraph;

using System.Diagnostics;




namespace PADE
{


    public partial class PADE_explorer : Form
    {

        #region Variable Declarations

        BackgroundWorker RC_worker;

        public static string WorkerReturnVal = "";
        public static StreamWriter fs;
        public static Point subFormLocation;
        private TB4_Register tempRegister; //declaring and initializing this (see:form.load) ahead of time saves our butt when we read temperatures...
        public bool usbMode = false;
        public List<formPair> openForms = new List<formPair>(1);
        public TB4_Exception exceptionHandler = new TB4_Exception();

        //these variables are loaded on runtime:
        public static string excelPathname = "";
        public static string boardVersion = "";
        public static string lblPADE_Explorer = "PADE Explorer";

        public class formPair
        {
            public Type formType;
            public Form form;

            public formPair(Type fType, Form fForm)
            {
                formType = fType;
                form = fForm;
            }
        }

        #endregion

        public PADE_explorer()
        {
            InitializeComponent();

            checkRegistryKey();

            this.Text = lblPADE_Explorer;
        }

        #region Form Startup, Events, and Controls




        void PADE_explorer_ResizeEnd(object sender, System.EventArgs e)
        {
            /*
            Console.WriteLine("ResizeEnd event");
            for (int i = 0; i < openForms.Count; i++)
            {

                Form currentForm = openForms[i].form;

                Console.WriteLine(currentForm.Name);
                openForms[i].form.Invoke((MethodInvoker)delegate { openForms[i].form.Location = new System.Drawing.Point(this.Location.X + subFormLocation.X, this.Location.Y + subFormLocation.Y); });
                openForms[i].form.Activate();
            }
             * */
        }
        private void PADE_explorer_Load(object sender, EventArgs e)
        {

            //INITIALIZATION STUFF

            //subFormLocation = new System.Drawing.Point(panel2.Location.X + 10, panel2.Location.Y + 30);
            subFormLocation = new System.Drawing.Point(100, 100);
            tempRegister = registerLookup("PADE_TEMP");
            //--------------------

            // createPADES(); //used for testing purposes: remove when finished
            placeButtons();
            updateStatusText(true, "Standby.");
            updateStatusBar(false, 0);

            TB4.thePADE_explorer = this;
            tempThread.Interval = PADE.samplePeriod;
            tempThread.Enabled = false;

            recolorBackground();
            //hookEvents();

            dockPanel1.Parent = this;

            dockPanel1.BringToFront();

            dockPanel2.BringToFront();
            TB4.thePADE_Selector.Show(dockPanel2, DockState.Document);

            //if the user has a crappy screen, resize everything 
            if (SystemInformation.VirtualScreen.Height < 788)
            {
                dockPanel2.Location = new Point(dockPanel2.Location.X, 82);
                groupBox1.Location = new Point(groupBox1.Location.X, 536);
                groupBox2.Location = new Point(groupBox2.Location.X, 636);
                dockPanel1.Location = new Point(dockPanel1.Location.X, 83);
                dockPanel1.Size = new Size(dockPanel1.Size.Width, dockPanel1.Size.Height - 70);
                this.Height = 788 - 48;

            }
            TB4_Exception.logInfo("Application started ", true);
            TB4.theSystemViewer.Show(dockPanel1, DockState.Document);

        }


        void PADE_explorer_Shown(object sender, System.EventArgs e)
        {
            autoDetectEthernet();
        }

        void PADE_explorer_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            //This is a workaround for a bug in DockPanel Suite which prevents the MDI Container form 
            //from closing while child Forms are open

            e.Cancel = false;
            

        }

        /// <summary>
        /// An annoying hack to fix the main form's background color from the MdiContainer default to something nicer.
        /// </summary>
        void recolorBackground()
        {
            MdiClient ctlMDI;

            foreach (Control ctl in this.Controls)
            {
                try
                {

                    ctlMDI = (MdiClient)ctl;

                    ctlMDI.BackColor = this.BackColor;
                }
                catch (InvalidCastException exc)
                {
                }
            }

        }

        void toolStripStatusLabel2_Click(object sender, System.EventArgs e)
        {
            if (usbMode)
            {
                TB4.using_Ether_comms = true; TB4.using_USB_comms = false;
                updateStatusText(true, "Standby (Ethernet Mode).");
                TB4.theRunForm.Text = "Run";
                if (TB4.PADE_List.Count > 0) TB4.activatePADE(TB4.PADE_List[1], true, false);
            }
            usbMode = false;


        }


        /// <summary>
        /// This is a trick used to prevent child forms from disappearing behind the explorer when a control is clicked.
        /// </summary>\
        /// 
        /*
        private void hookEvents()
        {
            foreach (Control ctrl in this.Controls)
            {
                ctrl.MouseClick += new MouseEventHandler(PADE_explorer_Click);
                if (ctrl.HasChildren)
                {
                    foreach (Control child in ctrl.Controls)
                    {
                        try
                        {
                            child.MouseClick += new MouseEventHandler(PADE_explorer_Click);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error adding event handler to control (line 105): " + ex.Message);
                        }

                    }

                }
            }
            treeView1.Click += new EventHandler(PADE_explorer_Click);
            treeView1.NodeMouseClick += new TreeNodeMouseClickEventHandler(PADE_explorer_Click);
            selectedPADEList.Click += new EventHandler(PADE_explorer_Click);
            selectedPADEList.NodeMouseClick += new TreeNodeMouseClickEventHandler(PADE_explorer_Click);

        }
        */
        private void placeButtons()
        {


            Form displayFormInstance = new DisplayControl(); //instantiate an object of the DisplayControl() class.  
            //We will steal the DisplayControl form's buttons so we don't have to recreate them.

            int counter = 0;
            foreach (Control ctrl in displayFormInstance.Controls)  //iterate through the collection exposed by the DisplayControl class
            {
                Console.WriteLine(ctrl.Name.ToString() + "   " + ctrl.Text.ToString());
                if (ctrl.Name.ToString().ToUpper().IndexOf("BUTTON") >= 0)
                {
                    Button newButton = new Button();
                    // newButton=(Button) ctrl; //this should equate type, size, text, name, etc...


                    int screenOffset = 0;
                    if (SystemInformation.VirtualScreen.Height < 788)
                    {
                        screenOffset = 15;
                        panel1.Height = 50;
                    }
                    newButton.Name = ctrl.Name;
                    newButton.Text = ctrl.Text;
                    newButton.Tag = ctrl.Tag; //will be used for event handling
                    newButton.Size = new Size(ctrl.Size.Width, ctrl.Size.Height - screenOffset); ;
                    newButton.BackColor = ctrl.BackColor;

                    newButton.Show();

                    panel1.Controls.Add(newButton);



                    if (counter <= 4) { newButton.Location = new Point(counter * 144 + 8, 5); }
                    else { newButton.Location = new Point((counter - 5) * 144 + 8, 40 - screenOffset); }

                    counter++;

                    //assign a handler to the click event
                    newButton.Click += (sender, e) =>
                    {
                        //raising the same event as the symmetric event on the DisplayControl form turned out to be "impossible" to do reliably.
                        //There probably is a way though...

                        switch (newButton.Text)
                        {

                            case "RUN":
                                toggleForm(TB4.theRunForm, newButton);

                                break;
                            case "REGISTERS":
                                toggleForm(TB4.theRegistersForm, newButton);

                                break;
                            case "ARRAYS":
                                toggleForm(TB4.theArraysForm, newButton);

                                break;
                            case "SCOPE":
                                toggleForm(TB4.thePlot, newButton);

                                break;
                            case "HISTO":
                                toggleForm(TB4.theHist1, newButton);

                                break;
                            case "Bias Offset":
                                toggleForm(TB4.theBiasOffset, newButton);

                                break;
                            case "FLASH":
                                toggleForm(TB4.theFlash, newButton);

                                break;
                            case "GBE":
                                toggleForm(TB4.theGBE, newButton);

                                break;
                            case "Scanner":
                                toggleForm(TB4.theHist_and_Scan, newButton);

                                break;
                            case "DRAM":
                                toggleForm(TB4.theDRAM, newButton);

                                break;
                            default:
                                MessageBox.Show("Error in the switch statement!");
                                break;
                        }


                    };
                }
            }
        }


        private void toggleForm(DockContent argument, Button control)
        {
            Type argtype = argument.GetType();
            formPair currentPair = new formPair(argtype, argument);
            if (argument.Visible)
            {
                Console.WriteLine("Made invisible");
                argument.Visible = false;
                argument.Hide();
                if (control != null) control.Font = new Font(control.Font.Name, control.Font.Size, FontStyle.Regular);
                if (openForms.Contains(currentPair)) openForms.Remove(currentPair); //the if() is a safety net...


            }
            else
            {
                Console.WriteLine("Made visible");

                openForms.Add(currentPair);
                if (control != null) control.Font = new Font(control.Font.Name, control.Font.Size, FontStyle.Bold);

                argument.Show(dockPanel1, DockState.Document);
                argument.Visible = true;
                //argument.Location = new System.Drawing.Point(this.Location.X + subFormLocation.X, this.Location.Y + subFormLocation.Y);
                argument.Activate();


            }

        }



        void PADE_explorer_Click(object sender, System.EventArgs e)
        {
            System.Drawing.Point mouseLoc = this.PointToClient(Cursor.Position);

            if (mouseLoc.Y > 25)
            {
                foreach (formPair winForm in openForms)
                {
                    winForm.form.Activate();
                }
            }
        }


        public void PADE_explorer_Update_Label(string newLabel)
        {
            PADE_explorer.lblPADE_Explorer = newLabel;
            this.Invalidate();
            Application.DoEvents();
        }

        #endregion

        #region StatusBar Control
        public void updateStatusBar(bool showBar, int value)
        {
            toolStripProgressBar2.Visible = showBar;
            if (value <= 100) toolStripProgressBar2.Value = value;
            else toolStripProgressBar2.Value = 100;
            toolStripProgressBar2.Invalidate();
            this.Refresh();
        }

        private void statusStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }
        public void updateStatusText(bool showText, string value)
        {
            //statustextType newDel = new statustextType(p_updateStatusText);
            //System.Threading.Thread redrawThread = new System.Threading.Thread(() => { if(this.Invoke( () => { toolStripStatusLabel1
            p_updateStatusText(showText, value);


        }
        private delegate void statustextType(bool text, string value);
        private void p_updateStatusText(bool showText, string value)
        {

            toolStripStatusLabel2.Visible = showText;
            toolStripStatusLabel2.Text = value;

            toolStripStatusLabel2.Invalidate();
            this.Refresh();


        }

        public void incrementStatusBar()
        {
            if (this.toolStripProgressBar2.Value < 100) this.toolStripProgressBar2.Value = this.toolStripProgressBar2.Value + 1;
        }

        #endregion

        #region Init Buttons
        private void uploadInitButton_Click(object sender, EventArgs e)
        {
            uploadInit();
        }

        /// <summary>
        /// This subroutine will attempt to load each of the PADE's in the selected PADE list with register values from a file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uploadRegDataButton_Click(object sender, EventArgs e)
        {
            uploadRegData();
        }
        #endregion

        #region Utilities
        private void uploadInit()
        {
            //first, build a string array containing all the SN's of the PADEs the user wants to initialize.
            string[] PADEList = new string[TB4.thePADE_Selector.selectedPADEList.Nodes.Count];

            for (int i = 0; i < TB4.thePADE_Selector.selectedPADEList.Nodes.Count; i++)
            {
                string[] DH = TB4.thePADE_Selector.selectedPADEList.Nodes[i].Text.Split(new char[] { ' ' });
                PADEList[i] = DH[1];
            }
            //Initialize the PADEs using the created array.
            writeInitToPADE(PADEList);
        }
        public void uploadRegData()
        {
            //the user wants to upload a config file.  Create a popup box to ask for a pathname.

            updateStatusText(true, "Updating Registers...");
            updateStatusBar(true, 0);

            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "Init Files (.tb4)|*.tb4";
            openFileDialog.FilterIndex = 1;

            openFileDialog.Multiselect = false;

            DialogResult userClickedOK = openFileDialog.ShowDialog();
            string filename = openFileDialog.FileName;
            TB4_Register.configFile = filename;

            if (userClickedOK == DialogResult.OK)
            {
                if (System.IO.File.Exists(filename))
                {

                    foreach (TreeNode item in TB4.thePADE_Selector.selectedPADEList.Nodes)
                    {

                        PADE tempPADE = PADE_explorer.getPADE(item.Text);
                        TB4.activatePADE(tempPADE, false, false);
                        TB4_Register.applyRegisterList(tempPADE.PADE_sn);
                        updateStatusBar(true, toolStripProgressBar2.Value + 99 / TB4.thePADE_Selector.selectedPADEList.Nodes.Count);
                    }
                }
                else
                {
                    updateStatusText(true, "Initialization Failed: File Does Not Exist (" + filename + ")");
                }

            }
            else
            {

            }
            updateStatusText(true, "Standby.");
            updateStatusBar(false, 0);
        }

        /// <summary>
        /// This writes the initialization files to each PADE in the argument array.
        /// </summary>
        /// <param name="PADEList"></param>
        private void writeInitToPADE(string[] PADEList)
        {
            updateStatusBar(true, 0);

            updateStatusText(true, "Uploading Initialization Files...");

            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "Init Files (.tb4)|*.tb4";
            openFileDialog.FilterIndex = 1;

            openFileDialog.Multiselect = false;

            DialogResult userClickedOK = openFileDialog.ShowDialog();

            if (PADEList.Length < 1) return;


            if (userClickedOK == DialogResult.OK)
            {
                System.IO.StreamReader reader = new System.IO.StreamReader(openFileDialog.FileName);
                TB4.myTextDisplay.text = reader.ReadToEnd();
                reader.Close();
                DialogResult result = TB4.myTextDisplay.ShowDialog();

                if (result == DialogResult.OK)
                {
                    string initFileLocation = openFileDialog.FileName;
                    for (int i = 0; i < PADEList.Length; i++)
                    { //iterate through all PADE's

                        updateStatusText(true, "Attempting Initialization of PADE " + PADEList[i]);


                        Boolean exists = false; //determine if the PADE is a member of TB4.PADE_List
                        for (int j = 1; j <= TB4.PADE_List.Count; j++) if (TB4.PADE_List[j].PADE_sn == PADEList[i]) exists = true;

                        if (exists)  //the current PADE exists, so lets write the init file
                        {

                            try //make sure we're not currently collecting data...
                            {
                                if (TB4.myRun.flgRunning) { MessageBox.Show("stop the run first!"); }
                            }
                            catch
                            {
                                MessageBox.Show("Error initializing PADE: there is no run (TB4 hasn't been initialized).");
                                return;
                            }

                            try //the actual initialization happens in this try block.
                            {
                                activatePADE(PADEList[i]);
                                TB4.FileInit(initFileLocation);
                                updateStatusBar(true, toolStripProgressBar1.Value + (int)100 / PADEList.Length - 1); //success!

                                buttonNode removeNode = buttonNode.notInitializedList.Find(item => item.associatedPADE.PADE_sn == PADEList[i]);
                                removeNode.associatedPADE.initializationFilePathname = initFileLocation;
                                if (removeNode != null) buttonNode.notInitializedList.Remove(removeNode);

                                int val = TB4.theSystemViewer.boxSee1.textOverlayList.Count;
                                if (TB4.theSystemViewer.boxSee1.findOverlayIndex("Initialized Pades") >= 0)
                                {
                                    TB4.theSystemViewer.boxSee1.textOverlayList[TB4.theSystemViewer.boxSee1.findOverlayIndex("Initialized Pades")].text = buttonNode.notInitializedList.Count + " PADEs have not been initialized.";
                                    if (buttonNode.notInitializedList.Count == 0)
                                    {
                                        TB4.theSystemViewer.boxSee1.textOverlayList.Remove(TB4.theSystemViewer.boxSee1.textOverlayList[TB4.theSystemViewer.boxSee1.findOverlayIndex("Initialized Pades")]);
                                        initializationToolStripMenuItem.DropDownItems[2].ForeColor = Color.Black;
                                        if (initializationToolStripMenuItem.DropDownItems[0].ForeColor == Color.Black && initializationToolStripMenuItem.DropDownItems[1].ForeColor == Color.Black && initializationToolStripMenuItem.DropDownItems[2].ForeColor == Color.Black)
                                        {
                                            initializationToolStripMenuItem.ForeColor = Color.Black;
                                        }
                                    }
                                }




                            }
                            catch (Exception ex)
                            {
                                updateStatusText(true, "Initialization Failed: " + ex.Message);
                                updateStatusBar(false, 0);
                                return;
                            }

                        }

                    }
                }
                else
                {
                    updateStatusText(true, "Initialization Cancelled.");
                    updateStatusBar(false, 0);
                }
            }

            updateStatusText(true, "Initialization Successful!");
            updateStatusBar(false, 0);

        }

        public static void activatePADE(string SN)
        {
            //first, make sure the PADE exists and if it does assign it to activePADE 

            PADE activePADE = null;

            activePADE = getPADE(SN);

            if (activePADE != null)
            {
                try
                {

                    TB4.activatePADE(activePADE, false, false);

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error activating PADE: " + ex.Message);
                    throw new Exception("Error activating PADE: " + ex.Message);

                }
            }
            else
            {
                throw new Exception("Error activating PADE: PADE" + SN + " Does Not Exist.");
            }

        }

        public static PADE getPADE(string SN)
        {
            PADE returnPADE = null;
            for (int i = 1; i <= TB4.PADE_List.Count; i++)
            {
                if (("PADE " + TB4.PADE_List[i].PADE_sn == SN) || (TB4.PADE_List[i].PADE_sn == SN))
                {

                    returnPADE = TB4.PADE_List[i];
                }
            }
            return returnPADE;
        }

        public static List<PADE> getallPADE(PADE.type_of_PADE myType)
        {
            List<PADE> thisList = new List<PADE>(2);
            PADE thisPADE;
            foreach (KeyValuePair<int, PADE> kvp in TB4.PADE_List)
            {
                if (myType == PADE.type_of_PADE.slave)
                {
                    if (kvp.Value.PADE_is_MASTER == false) { thisList.Add(kvp.Value); }
                }
                else
                //if ((myType == PADE.type_of_PADE.crate_master) || (myType == PADE.type_of_PADE.system_master))
                {
                    if (kvp.Value.PADE_is_MASTER == true) { thisList.Add(kvp.Value); }
                }

                //thisPADE = kvp.Value;
                //if ((myType == PADE.type_of_PADE.crate_master) && thisPADE.PADE_is_MASTER)
                //{
                //    thisList.Add(thisPADE);
                //}
                //if ((myType == PADE.type_of_PADE.slave) && thisPADE.PADE_is_SLAVE)
                //{
                //    thisList.Add(thisPADE);
                //}

            }
            return thisList;
        }

        public static TB4_Register registerLookup(string registerName)
        {
            TB4_Register returnReg = null;
            try
            {
                returnReg = TB4.my_reg_collection.Find(delegate(TB4_Register reg) { return reg.name == registerName; });
                if (returnReg == null)
                {
                    Console.WriteLine("Register lookup failed: " + registerName);
                    MessageBox.Show("Register lookup failed...");
                }
                return returnReg;

            }
            catch (Exception ex)
            {
                TB4_Exception.logError(ex, null, true);
                return null;
            }

        }

        public static TB4_Register regAddLookup(UInt32 registerAddress)
        {
            TB4_Register returnReg;
            try
            {
                returnReg = TB4.my_reg_collection.Find(delegate(TB4_Register reg) { return reg.addr == registerAddress; });

                return returnReg;

            }
            catch (Exception ex)
            {
                TB4_Exception.logError(ex, null, true);
                return null;
            }

        }

        public static void MonitorPade(PADE thisPADE,out string monitor_string)
        {
            monitor_string = DateTime.Now.ToString() + ": ";
            monitor_string += thisPADE.PADE_sn+" ";
            monitor_string += PADE_explorer.registerLookup("PADE_TEMP").RegRead() + " ";
            monitor_string += PADE_explorer.registerLookup("SIB_TEMP").RegRead() + " ";
            monitor_string += PADE_explorer.registerLookup("BIAS_VMON").RegRead() + " ";
            monitor_string += PADE_explorer.registerLookup("BIAS_IMON").RegRead() + " ";
        }

        public tempStamp getTemperature(PADE targetPADE)
        {
            tempStamp returnTemp = new tempStamp();
            PADE oldPADE = TB4.ActivePADE;
            TB4.activatePADE(targetPADE, false, false);

            UInt16 DH = tempRegister.RegRead();
            DH = Convert.ToUInt16((double)DH * 9 / 80 + 32);
            Console.WriteLine("PRE-CONVERT TEMP: " + DH.ToString());
            returnTemp.temperature = DH;
            returnTemp.time = DateTime.Now;
            TB4.activatePADE(oldPADE, false, false);
            return returnTemp;
        }

        Random newRand = new Random();
        public tempStamp getFakeTemperature(PADE targetPADE) //used for testing
        {
            tempStamp returnStamp = new tempStamp();
            returnStamp.temperature = (UInt16)newRand.Next(0, 30);
            returnStamp.time = DateTime.Now;

            return returnStamp;
        }

        /// <summary>
        /// Moves the argument form to a specific position within the PADE explorer.
        /// </summary>
        /// <param name="childForm"></param>
        public void moveChildForm(Form childForm)
        {
            childForm.Location = new Point(subFormLocation.X + this.Location.X, subFormLocation.Y + this.Location.Y);
        }

        /// <summary>
        /// This subroutine will query the register at some IP address to see if there is a PADE there.
        /// </summary>
        /// <param name="SN"></param>
        public bool searchForPADE(string SN, bool isEthernet, bool changeFormText)
        {
            PADE newPADE = new PADE();
            newPADE.PADE_sn = SN.ToString();
            newPADE.IP4_add[0] = Convert.ToByte(SN);
            newPADE.MAC_add[0] = Convert.ToByte(SN);

            if (isEthernet)
            {
                newPADE.flgEther_comms = true;
                newPADE.flgUSB_comms = false;
            }
            else
            {
                newPADE.flgUSB_comms = true;
                newPADE.flgEther_comms = false;
            }

            //Console.Write("PADE IP ADD: ");
            //for (int i = 0; i < 4; i++) { Console.Write(newPADE.IP4_add[i].ToString()); }

            //if (newPADE.IP4_add[0] == 69) { Console.WriteLine("here"); }
            TB4.activatePADE(newPADE, changeFormText, false);

            TB4_Register verReg = registerLookup("FIRMWARE_VER");
            ushort newthing = verReg.RegRead();
            Console.WriteLine(SN + "   " + newthing.ToString());
            if (newthing > 0)
            {
                newPADE.PADE_fw_ver = newthing;
                return true;
            }

            return false;
        }

        public List<PADE> findAllPADES(bool changeFormText)
        {
            updateStatusText(true, "Searching for PADEs...");
            tempThread.Enabled = false;
            orderedPADEList.Clear();
            TB4.thePADE_Selector.treeView1.Nodes.Clear();
            TB4.PADE_List.Clear();

            for (int i = 1; i <= 150; i++)
            {
                updateStatusBar(true, toolStripProgressBar2.Value + 1);
                if (searchForPADE(i.ToString(), true, changeFormText))
                {
                    PADE newPADE = new PADE();
                    newPADE.PADE_sn = i.ToString();
                    newPADE.flgEther_comms = true;
                    newPADE.recordTemperature = false;

                    newPADE.IP4_add[0] = Convert.ToByte(i);
                    newPADE.MAC_add[0] = Convert.ToByte(i);

                    newPADE.flgEther_comms = true;

                    //TB4.activatePADE(newPADE, true, false); 
                    TB4_Register statReg = registerLookup("STATUS_REG");
                    TB4_Register firmwareVersion = registerLookup("FIRMWARE_VER");
                    newPADE.PADE_fw_ver = firmwareVersion.RegRead();
                    ushort var = statReg.RegRead();

                    if ((var & 0x8000) == 0x8000)
                    {
                        newPADE.PADE_is_MASTER = false;
                        newPADE.PADE_is_SLAVE = true;
                    }
                    else
                    {
                        newPADE.PADE_is_MASTER = true;
                        newPADE.PADE_is_SLAVE = false;
                    }

                    
                    TB4.activatePADE(newPADE, changeFormText, true);

                    /***********************************************************************/
                    orderedPADEList.Add(getPADE(i.ToString()));  //THIS WAS ADDED FOR THE SEPTEMBER 21 TEST BEAM
                    //TB4.thePADE_Selector.addPADENode(getPADE(i.ToString()));  
                    /***********************************************************************/

                    //TB4.thePADE_Selector.addPADENode(getPADE(i.ToString()));

                }
            }
            if (TB4.PADE_List.Count > 0) TB4.activatePADE(TB4.PADE_List[1], true, false);
            else selectedPADE_label.Text = "N/A";

            updateStatusBar(false, 0);
            updateStatusText(true, "Standby.");

            return orderedPADEList;
        }
        List<PADE> orderedPADEList = new List<PADE>();
        public static void saveGraphImage(ZedGraphControl graphObject)
        {

            SaveFileDialog newSaveDialog = new SaveFileDialog();
            newSaveDialog.DefaultExt = ".jpg";
            newSaveDialog.FileName = "Graph";
            newSaveDialog.ShowDialog();
            graphObject.GetImage().Save(newSaveDialog.FileName, System.Drawing.Imaging.ImageFormat.Jpeg);
        }

        private void applyBiasGains()
        {

            OpenFileDialog newDialog = new OpenFileDialog();

            DialogResult result = newDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                string pathname = newDialog.FileName;

                for (int i = 1; i <= TB4.PADE_List.Count; i++)
                {
                    foreach (string filename in System.IO.Directory.EnumerateFiles(pathname))
                    {
                        string[] DH = filename.ToUpper().Split(new string[] { "SN" }, StringSplitOptions.RemoveEmptyEntries);
                        if (DH[1].Contains(TB4.PADE_List[i].PADE_sn.ToUpper()))
                        {
                            //match
                            PADE_explorer.activatePADE(TB4.PADE_List[i].PADE_sn);
                            TB4.FileInit(filename);
                        }
                    }
                }
            }


        }

        List<PADE> returnLikePADES(PADE.type_of_PADE type)
        {
            List<PADE> returnList = new List<PADE>();

            foreach (KeyValuePair<int, PADE> pade in TB4.PADE_List)
            {
                if (pade.Value.PADE_is_MASTER && (type == PADE.type_of_PADE.crate_master || type == PADE.type_of_PADE.system_master))
                {
                    returnList.Add(pade.Value);
                }
                if (pade.Value.PADE_is_SLAVE && type == PADE.type_of_PADE.slave)
                {
                    returnList.Add(pade.Value);
                }
            }
            return returnList;
        }
        #endregion

        #region Other
        /// <summary>
        /// Reads all temperature registers, updates graph (if open), runs analysis tools
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        private TreeNode getTreeNode(string SN)
        {
            TreeNode tempNode = null;
            foreach (TreeNode node in TB4.thePADE_Selector.treeView1.Nodes)
            {
                if (node.Text.Contains(SN)) tempNode = node;
            }
            return tempNode;

        }


        private void tempThread_Tick(object sender, EventArgs e)
        {
            tempThread.Enabled = false;
            string l = "";
            PADE old_PADE = TB4.ActivePADE;
            string[] PADEList = new string[TB4.thePADE_Selector.selectedPADEList.Nodes.Count]; 
            for (int i = 0; i < TB4.thePADE_Selector.selectedPADEList.Nodes.Count; i++)
            {
                string[] DH = TB4.thePADE_Selector.selectedPADEList.Nodes[i].Text.Split(new char[] { ' ' });
                PADEList[i] = DH[1];
                PADE_explorer.activatePADE(PADEList[i]);
                MonitorPade(TB4.ActivePADE,out l);
                if (fs != null) { fs.WriteLine(l); }
            }

            tempThread.Enabled = true;
            
            


            //if (!TB4.myRun.flg_UDP)
            //{
            //    tempStamp newTemp;
            //    for (int i = 1; i <= TB4.PADE_List.Count; i++) //iterate through all PADEs to read the temperature register
            //    {
            //        if (TB4.PADE_List[i].recordTemperature)
            //        {
            //            //if the user wants to record the temperature of this PADE
            //            newTemp = getTemperature(TB4.PADE_List[i]);
            //            //newTemp = getFakeTemperature(TB4.PADE_List[i]); //used for testing
            //            TB4.PADE_List[i].addTemp(newTemp);
            //            Console.WriteLine("TEMP:  " + newTemp.temperature.ToString());

            //            //change the color of the treenode according to the PADE temperature
            //            Color tempColor;
            //            if (newTemp.temperature < TB4.tempBands[0]) tempColor = Color.Blue;
            //            else if (newTemp.temperature < TB4.tempBands[1]) tempColor = Color.MediumAquamarine;
            //            else if (newTemp.temperature < TB4.tempBands[2]) tempColor = Color.Green;
            //            else if (newTemp.temperature < TB4.tempBands[3]) tempColor = Color.Orange;
            //            else tempColor = Color.Red;

            //            getTreeNode(TB4.PADE_List[i].PADE_sn).ForeColor = tempColor;

            //            if (TB4.ActivePADE == TB4.PADE_List[i] && TempPlot.formOpen)
            //            {
            //                //the graph is currently open to the same PADE. We need to update it in real time
            //                TB4.theTempPlot.addPoint(newTemp, true);
            //                TB4.ActivePADE.addTemp(newTemp);

            //                if (TB4.ActivePADE.analysisEnabled)
            //                {

            //                    TB4.theTempPlot.averageTextBox.Text = TB4.ActivePADE.averageTemp.ToString();

            //                    TB4.theTempPlot.minimumTextBox.Text = TB4.ActivePADE.minTemp.temperature.ToString();
            //                    TB4.theTempPlot.minTimeBox.Text = TB4.ActivePADE.minTemp.time.ToString();
            //                    TB4.theTempPlot.maximumTextBox.Text = TB4.ActivePADE.maxTemp.temperature.ToString();
            //                    TB4.theTempPlot.maxTimeBox.Text = TB4.ActivePADE.maxTemp.time.ToString();

            //                }
            //            }
            //        }
            //    }
            //}
        }


        private void debuggingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //(new PerformanceLog()).Show();
            MessageBox.Show("Under Construction");
        }

        private void registerBaseUnitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Label newLabel = new System.Windows.Forms.Label();
            newLabel.Text = "Please input a base:";
            newLabel.Location = new Point(2, 2);
            newLabel.TextAlign = ContentAlignment.MiddleCenter;
            newLabel.Width = 150;
            TextBox newBox = new TextBox();
            newBox.Width = 50;
            newBox.Height = 25;
            newBox.Text = TB4.theRegistersForm.returnBase.ToString();
            newBox.Location = new Point(50, 30);

            Button newButton = new Button();
            newButton.Width = 50;
            newButton.Height = 25;
            newButton.Text = "Accept";
            newButton.Location = new Point(50, 58);

            Form answerForm = new Form();
            answerForm.Width = 160;
            answerForm.Height = 150;

            answerForm.Controls.Add(newLabel);
            answerForm.Controls.Add(newButton);
            answerForm.Controls.Add(newBox);

            answerForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;


            this.Opacity = 0.5;

            answerForm.Show();
            newBox.Show();
            newButton.Show();

            answerForm.Location = new Point(this.Location.X + this.Width / 2 - 50, this.Location.Y + this.Height / 2 - 40);

            Console.WriteLine(answerForm.Location.ToString());
            answerForm.BringToFront();
            newBox.BringToFront();
            newButton.BringToFront();



            newButton.Click += (_sender, _e) =>
            {
                int num;

                try
                {
                    num = Convert.ToInt16(newBox.Text);
                    if (num > 1 && num <= 16) TB4.theRegistersForm.returnBase = Convert.ToInt16(newBox.Text);
                }
                catch
                {
                    MessageBox.Show("Invalid Entry");
                }
                answerForm.Controls.Remove(newButton);
                answerForm.Controls.Remove(newBox);
                answerForm.Close();
                answerForm.Dispose();
                newButton.Hide();
                newButton.Dispose();
                newBox.Hide();
                newButton.Dispose();

                this.Opacity = 1;
            };
        }


        private void downloadInitButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog newDialog = new SaveFileDialog();
            newDialog.DefaultExt = ".txt";
            DialogResult result = newDialog.ShowDialog();

            if (result == DialogResult.OK) TB4_Register.createRegisterList(newDialog.FileName);
        }

        private bool autoDetectEthernet()
        {

            IList<LivePacketDevice> allDevices = LivePacketDevice.AllLocalMachine;
            //hit me
            for (int i = 0; i < allDevices.Count; i++)
            {

                if ((allDevices[i].Description.ToLower().Contains("gigabit")) || (allDevices[i].Description.ToLower().Contains("gbe")) || (allDevices[i].Description.ToLower().Contains("amd")))
                {
                    DialogResult response = MessageBox.Show("Ethernet interface dected: " + allDevices[i].Description + ". Would you like to connect?", "", MessageBoxButtons.YesNo);

                    if (response == DialogResult.Yes)
                    {
                        TB4.ETH_INTERFACE = allDevices[i];
                        TB4.ETH_OK = true;
                        TB4.SlowControl_interface = allDevices[i];

                        Eth_comms.Eth_Open();

                        TB4.theRunForm.txtEthStatus.Text = "ok";
                        TB4.theRunForm.btnRUN.Enabled = true;
                        TB4.theRunForm.btnInit.Enabled = true;

                    }
                }

            }

            return false;

        }

        public void childClosed(DockContent closingForm)
        {

            foreach (Control ctrl in panel1.Controls)
            {
                if (ctrl.Tag.ToString().ToUpper() == closingForm.Name.ToString().ToUpper())
                {
                    toggleForm(closingForm, (Button)ctrl);
                    Console.WriteLine("CONTROL FOUND: " + ctrl.Text);

                    return;
                }
            }

        }

        public void childChangedDockstate(DockContent closingForm, Size returnSize)
        {
            if (closingForm.FloatPane != null)
            {
                closingForm.FloatPane.FloatWindow.Size = new Size(returnSize.Width + 16, returnSize.Height + 36);
                closingForm.TopMost = true;
            }

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void addPADEToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            Form newForm = new Form();
            System.Windows.Forms.Label newLabel = new System.Windows.Forms.Label();
            TextBox newBox = new TextBox();
            Button newButton = new Button();

            newForm.Size = new Size(100, 100);
            newBox.Size = new Size(25, 25);
            newButton.Size = new Size(50, 25);
            newButton.Text = "Submit";
            newBox.Location = new Point(38, 0);
            newButton.Location = new Point(25, 30);

            newForm.Controls.Add(newBox);
            newForm.Controls.Add(newButton);

            newForm.Show();

            newButton.Click += new EventHandler(delegate(object O, EventArgs a)
            {
                PADE newPADE = new PADE();
                newPADE.PADE_sn = newBox.Text;
                TB4.activatePADE(newPADE, true, true);
                TB4.thePADE_Selector.addPADENode(newPADE);
            });
        }

        private void findAllPADEsToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            autoDetectPADES();
        }

        private bool autoDetectPADES()
        {

            TB4.thePADE_Selector.treeView1.Nodes.Clear();
            TB4.PADE_List.Clear();
            List<PADE> foundPADES = findAllPADES(false);
            // NO!!!!!!!!!!!!!!! List<PADE> foundPADES 
            
            if (TB4.PADE_List.Count > 0) enableToolStripItems();

            bool allPADESAccountedFor = true;
            bool DH = false;
            if (System.IO.File.Exists(TB4.systemFileName))
            {
                TB4.theSystemViewer.boxSee1.loadState(TB4.systemFileName, false);
                if (foundPADES.Count != buttonNode.nodeList.Count) allPADESAccountedFor = false;
                else
                {

                    for (int i = 0; i < foundPADES.Count; i++)
                    {
                        for (int j = 0; j < buttonNode.nodeList.Count; j++)
                        {
                            if (buttonNode.nodeList[j].associatedPADE.PADE_sn == foundPADES[i ].PADE_sn) DH = true;
                        }
                        if (DH == false) allPADESAccountedFor = false;
                    }
                    TB4.theSystemViewer.boxSee1.textOverlayList.Clear(); //this may need to be removed at some point...
                    if (TB4.theSystemViewer.boxSee1.findOverlayIndex("Initialized Pades") < 0 && TB4.theSystemViewer.boxSee1.findOverlayIndex("Biased Pades") < 0)
                    {
                        TB4.theSystemViewer.boxSee1.textOverlayList.Clear(); //this may need to be removed at some point...
                        TB4.theSystemViewer.boxSee1.addTextOverlay("Initialized Pades", TB4.PADE_List.Count + " PADEs have not been initialized.", SystemColors.Highlight);
                        TB4.theSystemViewer.boxSee1.addTextOverlay("Biased Pades", TB4.PADE_List.Count + " PADEs have not had biases set.", SystemColors.Highlight);
                        initializationToolStripMenuItem.ForeColor = SystemColors.Highlight;
                        initializationToolStripMenuItem.DropDownItems[0].ForeColor = SystemColors.Highlight; //auto-associate bias files
                        initializationToolStripMenuItem.DropDownItems[2].ForeColor = SystemColors.Highlight; //global initialization
                    }
                }
            }
            else
            {
                allPADESAccountedFor = false;
            }
            if (allPADESAccountedFor)
            {
                TB4.theSystemViewer.boxSee1.drawEnvironment();
                return true;
            }
            else
            {
                if (foundPADES.Count > 0)
                {
                    //the system file was invalid.  So, create a new environment
                    TB4.theSystemViewer.boxSee1.clearState();
                    TB4.theSystemViewer.boxSee1.recordStateChanges = false;

                    padeCluster newCluster = new padeCluster(Color.Blue, "Default Cluster", TB4.theSystemViewer.boxSee1);

                    for (int a = 0; a < foundPADES.Count; a++)
                    {
                        newCluster.addNode(Color.Blue, foundPADES[a ]);
                    }

                    MessageBox.Show("The state in the system file did not match the one detected.  A new system file will be created, please select a filename.");

                    SaveFileDialog sDialog = new SaveFileDialog();
                    sDialog.Filter = "Text File (*.txt) | *.txt";

                    DialogResult result = sDialog.ShowDialog();

                    if (result == DialogResult.OK)
                    {
                        TB4.systemFileName = sDialog.FileName;
                        TB4.theSystemViewer.boxSee1.recordState(true);
                    }

                    TB4.theSystemViewer.boxSee1.loadState(TB4.systemFileName, true);
                    setSystemFileKey(TB4.systemFileName);

                    TB4.theSystemViewer.boxSee1.addTextOverlay("Initialized Pades", foundPADES.Count + " PADEs have not been initialized.", SystemColors.Highlight);
                    TB4.theSystemViewer.boxSee1.addTextOverlay("Biased Pades", foundPADES.Count + " PADEs have not had biases set.", SystemColors.Highlight);
                    initializationToolStripMenuItem.ForeColor = SystemColors.Highlight;
                    for (int i = 1; i < 3; i++) initializationToolStripMenuItem.DropDownItems[i].ForeColor = SystemColors.Highlight;
                }
                else
                {

                    TB4.theSystemViewer.boxSee1.clearState();

                    padeCluster.clusterList.Clear();
                    TB4.thePADE_Selector.treeView1.Nodes.Clear();
                    TB4.theSystemViewer.boxSee1.drawpadeClusters();
                    MessageBox.Show("No PADES were found...");
                }
            }

            return false;


        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void disableTemperatureMonitoringToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (disableTemperatureMonitoringToolStripMenuItem.Text == "Enable Temperature Monitoring") disableTemperatureMonitoringToolStripMenuItem.Text = "Disable Temperature Monitoring";
            else disableTemperatureMonitoringToolStripMenuItem.Text = "Enable Temperature Monitoring";


            tempThread.Enabled = !tempThread.Enabled;
            if ((tempThread.Enabled) && (fs == null))
            {
                DateTime n = System.DateTime.Now;
                string fn = "c:\\data\\rec_capture_" + n.Year.ToString() + n.Month.ToString("00") + n.Day.ToString("00") + "_" + n.Hour.ToString("00") + n.Minute.ToString("00") + n.Second.ToString("00") + ".txt";
                this.toolStripStatusLabel3.Text = fn;
                fs = new StreamWriter(fn);
            }
            else
            {
                if (fs != null) { fs.Close(); }
                fs = null;
            }

        }


        private void checkRegistryKey()
        {
            Microsoft.Win32.RegistryKey topKey = null;

            topKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("PADE");

            if (topKey == null)
            {
                //This is the first run on the user's computer.  Create the key and populate it.
                topKey = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("PADE");

                OpenFileDialog newDialog = new OpenFileDialog();
                newDialog.Filter = "I need Register File in CSV format (.csv)|*.csv";
                newDialog.FilterIndex = 1;
                newDialog.Multiselect = false;
                newDialog.FileName = Environment.SpecialFolder.Desktop.ToString();

                MessageBox.Show("It looks like this is the first time you've used this software on this computer.  Please select the registry map excel file.");
                System.Windows.Forms.DialogResult result = newDialog.ShowDialog();

                if (result == System.Windows.Forms.DialogResult.OK || result == System.Windows.Forms.DialogResult.Yes)
                {
                    //create a new registry key with the savefile dialog pathname
                    topKey.SetValue("Reg_File_Pathname", newDialog.FileName);
                }

                //now populate the firmware version key
                string version = Microsoft.VisualBasic.Interaction.InputBox("Please input the board type: pre-production or release");

                if (version.ToUpper() == "PRE-PRODUCTION" || version.ToUpper() == "RELEASE")
                {
                    topKey.SetValue("Board_Version", version);
                }
                else
                {
                    MessageBox.Show("Invalid entry for board version.  The default \"release\" will be chosen.");
                    topKey.SetValue("Board_Version", version);
                }

            }

            if (topKey.OpenSubKey("System_File_Pathname") != null)
            {
                TB4.systemFileName = (string)topKey.GetValue("System_File_Pathname");
            }

            if (!System.IO.File.Exists((string)topKey.GetValue("Reg_File_Pathname")))
            {
                //Microsoft.Win32.Registry.CurrentUser.DeleteSubKey("PADE");
                //MessageBox.Show("The registry key for this program has an invalid value.  Please restart the program and follow the prompts (or continue running without some optional features).");
            }

            excelPathname = (string)topKey.GetValue("Reg_File_Pathname");
            boardVersion = (string)topKey.GetValue("Board_Version");
            TB4.systemFileName = (string)topKey.GetValue("System_File_Pathname");
            topKey.Close();
        }

        private void setSystemFileKey(string pathname)
        {
            try
            {
                Microsoft.Win32.RegistryKey topKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("PADE", true);
                topKey.SetValue("System_File_Pathname", pathname);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion




        #region MenuStrip Functions

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TB4.myAboutBox.ShowDialog(); //open the splash page in modal mode, disabling main form access until the about box is closed.
        }

        private void changeRegisterSheetPathnameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog newdialog = new OpenFileDialog();
            newdialog.FileName = Environment.SpecialFolder.Desktop.ToString();

            System.Windows.Forms.DialogResult result = newdialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                Microsoft.Win32.RegistryKey topKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("PADE", true);
                topKey.SetValue("Reg_File_Pathname", newdialog.FileName);
            }
        }

        private void changeBoardVersionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string version = Microsoft.VisualBasic.Interaction.InputBox("Please input the board type: pre-production or release");
            Microsoft.Win32.RegistryKey topKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("PADE", true);

            if (version.ToUpper() == "PRE-PRODUCTION" || version.ToUpper() == "RELEASE")
            {
                topKey.SetValue("Board_Version", version.ToUpper());
            }
            else
            {
                MessageBox.Show("Invalid entry for board version.  The default \"release\" will be chosen.");
                topKey.SetValue("Board_Version", "RELEASE");
            }

        }

        private void viewPerformanceDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TB4.thePerfLog.Show(dockPanel1, DockState.Document);
        }

        private void systemViewerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SystemViewer theSystemViewer = new SystemViewer();
            //SystemViewer.canvasLocation = new Point(theSystemViewer.Location.X, theSystemViewer.Location.Y);
            //theSystemViewer.Show(dockPanel1, DockState.Document);
            theSystemViewer.Show();
        }

        private void showDataDebuggerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toggleForm(TB4.theDataDebug, null);
        }

        private void applyBiasFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            applyBiasGains();
        }

        private void loadSystemFileToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            OpenFileDialog newDialog = new OpenFileDialog();
            newDialog.Filter = "Text File (*.txt) | *.txt";

            DialogResult result = newDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                List<PADE> systemList = TB4.theSystemViewer.boxSee1.loadState(newDialog.FileName, true);
                //List<PADE> connectedPADEList = findAllPADES(false);
                findAllPADES(false);

                List<PADE> connectedPADEList = new List<PADE>(TB4.PADE_List.Count);
                foreach (KeyValuePair<int, PADE> thisPADEkvp in TB4.PADE_List)
                { connectedPADEList.Add(thisPADEkvp.Value); }

                List<PADE> unreachablePADES = new List<PADE>();
                List<PADE> extraPADES = new List<PADE>();

                foreach (PADE systemPADE in systemList)
                {
                    bool foundFlag = false;
                    foreach (PADE connectedPADE in connectedPADEList)
                    {
                        if (connectedPADE.PADE_sn == systemPADE.PADE_sn) foundFlag = true;
                    }
                    if (!foundFlag) unreachablePADES.Add(systemPADE);
                }
                foreach (PADE connectedPADE in connectedPADEList)
                {
                    bool foundFlag = false;
                    foreach (PADE systemPADE in systemList)
                    {
                        if (connectedPADE.PADE_sn == systemPADE.PADE_sn) foundFlag = true;
                    }
                    if (!foundFlag) extraPADES.Add(connectedPADE);
                }

                //now, determine how to deal with any mismatch.

                //If there are unreachable PADES, ask the user for the next step

                if (unreachablePADES.Count > 0)
                {
                    string prompt = "There were " + unreachablePADES.Count + " PADES in the system file that were unreachable: \n";
                    foreach (PADE pade in unreachablePADES)
                    {
                        prompt += "PADE " + pade.PADE_sn + "\n";
                    }
                    prompt += "Would you like to remove these from the system file?";

                    DialogResult dresult = MessageBox.Show(prompt, "System File", MessageBoxButtons.YesNo);

                    if (dresult == DialogResult.No) ;//do nothing...
                    else if (dresult == DialogResult.Yes)
                    {

                        foreach (PADE pade in unreachablePADES)
                        {
                            foreach (padeCluster cluster in padeCluster.clusterList)
                            {
                                cluster.removeNode(pade);
                                buttonNode.disposeButtonNode(pade);
                                if (cluster.padeList.Count == 0)
                                {
                                    padeCluster.clusterList.Remove(cluster);
                                    break;
                                    //the assumption with the break is that there are no duplicate button nodes (i.e. two button nodes that are both padeSN 10)
                                }
                            }
                        }

                        saveSystemFileToolStripMenuItem_Click(null, null);
                    }
                }


                //If there are extra PADES, put them in a cluster called "Extras"
                if (extraPADES.Count > 0)
                {
                    string prompt = "There were " + extraPADES.Count + " PADES detected that were not in the system file: \n";
                    foreach (PADE pade in extraPADES)
                    {
                        prompt += "PADE " + pade.PADE_sn + "\n";
                    }
                    prompt += "Would you like to add them to the system?";

                    DialogResult dresult = MessageBox.Show(prompt, "System File", MessageBoxButtons.YesNo);

                    if (dresult == DialogResult.Yes)
                    {
                        padeCluster extraCluster = new padeCluster(Color.Green, "Extras", TB4.theSystemViewer.boxSee1);
                        foreach (PADE pade in extraPADES)
                        {
                            extraCluster.addNode(Color.Green, pade);
                        }
                        TB4.theSystemViewer.boxSee1.recordState(true);
                    }
                }



                if (TB4.theSystemViewer.boxSee1.findOverlayIndex("Initialized Pades") < 0 && TB4.theSystemViewer.boxSee1.findOverlayIndex("Biased Pades") < 0)
                {

                    TB4.theSystemViewer.boxSee1.addTextOverlay("Initialized Pades", buttonNode.notInitializedList.Count + " PADEs have not been initialized.", SystemColors.Highlight);
                    TB4.theSystemViewer.boxSee1.addTextOverlay("Biased Pades", buttonNode.notBiasedList.Count + " PADEs have not had biases set.", SystemColors.Highlight);
                    initializationToolStripMenuItem.ForeColor = SystemColors.Highlight;
                    initializationToolStripMenuItem.DropDownItems[0].ForeColor = SystemColors.Highlight; //auto-associate bias files
                    initializationToolStripMenuItem.DropDownItems[2].ForeColor = SystemColors.Highlight; //global initialization
                }



                padeCluster.setSystemChangeEventHandler(new System.Collections.Specialized.NotifyCollectionChangedEventHandler(TB4.thePADE_Selector.clusterChanged),
                new System.Collections.Specialized.NotifyCollectionChangedEventHandler(TB4.thePADE_Selector.padeChanged));

                enableToolStripItems();
            }
            setSystemFileKey(newDialog.FileName);
            TB4.systemFileName = newDialog.FileName;
            TB4.thePADE_Selector.updateTreeview();
            TB4.theSystemViewer.boxSee1.drawEnvironment();

            if (buttonNode.nodeList.Count > 0) unHighlightInitializationToolStrip();



        }

        private void showSystemViewerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TB4.theSystemViewer.Show(dockPanel1, DockState.Document);
        }

        private void saveSystemFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sDialog = new SaveFileDialog();
            sDialog.Filter = "Text File (*.txt) | *.txt";

            DialogResult result = sDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                TB4.systemFileName = sDialog.FileName;
                TB4.theSystemViewer.boxSee1.recordState(true);
            }
        }

        private void applyBiasInitializationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (buttonNode.highlightedList.Count > 0)
            {
                toolStripStatusLabel2.Text = "Applying Bias Settings...";
                toolStripProgressBar2.Value = 0;
                string s = "The following PADES could not be initialized (make sure you have associated bias files): \n";
                int counter = 0;
                foreach (buttonNode node in buttonNode.highlightedList)
                {
                    toolStripProgressBar2.Value += (int)(100 / buttonNode.highlightedList.Count);
                    toolStripStatusLabel2.Text = "Applying Bias File to PADE " + node.associatedPADE.PADE_sn + ": " + node.associatedPADE.biasFilePathname;
                    TB4.activatePADE(node.associatedPADE, true, false);

                    if (System.IO.File.Exists(node.associatedPADE.biasFilePathname))
                    {
                        TB4.FileInit(node.associatedPADE.biasFilePathname);
                        buttonNode.notBiasedList.Remove(node);

                        TB4.theSystemViewer.boxSee1.textOverlayList[TB4.theSystemViewer.boxSee1.findOverlayIndex("Biased Pades")].text = buttonNode.notBiasedList.Count + " PADEs have not had biases set.";
                        if (buttonNode.notBiasedList.Count == 0)
                        {
                            TB4.theSystemViewer.boxSee1.textOverlayList.Remove(TB4.theSystemViewer.boxSee1.textOverlayList[TB4.theSystemViewer.boxSee1.findOverlayIndex("Biased Pades")]);
                            initializationToolStripMenuItem.DropDownItems[1].ForeColor = Color.Black;
                            if (initializationToolStripMenuItem.DropDownItems[0].ForeColor == Color.Black && initializationToolStripMenuItem.DropDownItems[1].ForeColor == Color.Black && initializationToolStripMenuItem.DropDownItems[2].ForeColor == Color.Black)
                            {
                                initializationToolStripMenuItem.ForeColor = Color.Black;
                            }
                        }
                    }
                    else
                    {
                        s += "PADE# " + node.associatedPADE.PADE_sn + '\n';
                    }
                }
                if (s.Contains('#')) MessageBox.Show(s);



            }
            else
            {
                MessageBox.Show("You haven't selected any PADES!");
            }
            updateStatusText(true, "Standby.");

        }

        private void autoAssociateBiasFilesToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Please choose the folder that stores bias files.  Each bias file should include, in its filename, the text \"SN\" followed by the SN number (e.x. \"mybiasfileSN15.tb4\")");
            FolderBrowserDialog newDialog = new FolderBrowserDialog();

            //newDialog.FileName = Environment.SpecialFolder.Desktop.ToString();

            DialogResult result = newDialog.ShowDialog();
            string unSetPADES = "The following PADES were not successfully associated with bias files: \n";
            int counter = 0;
            if (result == DialogResult.OK)
            {
                string pathname = newDialog.SelectedPath;
                //CHANGE THE BIAS FILE SEARCH TO ASSOCIATE FILES WITH THE PADECLUSTER MEMBERS.
                //pathname = (new System.IO.FileInfo(pathname)).DirectoryName;
                if (System.IO.Directory.Exists(pathname))
                {
                    for (int i = 0; i < buttonNode.nodeList.Count; i++)
                    {
                        foreach (string filename in System.IO.Directory.EnumerateFiles(pathname))
                        {
                            string[] DH = filename.ToUpper().Split(new string[] { "SN" }, StringSplitOptions.RemoveEmptyEntries);
                            if (DH.Length > 1)
                            {
                                string[] DH2 = DH[1].Split(new char[] { '.' });
                                if (DH2[0] == buttonNode.nodeList[i].associatedPADE.PADE_sn.ToUpper())
                                {
                                    //match
                                    TB4.activatePADE(buttonNode.nodeList[i].associatedPADE, true, false);
                                    buttonNode.nodeList[i].associatedPADE.biasFilePathname = filename;
                                    counter++;
                                    //TB4.FileInit(filename);
                                }
                            }
                        }
                    }

                    initializationToolStripMenuItem.DropDownItems[0].ForeColor = Color.Black;
                    initializationToolStripMenuItem.DropDownItems[1].ForeColor = SystemColors.Highlight;
                }


                foreach (buttonNode node in buttonNode.nodeList)
                {
                    if (node.associatedPADE.biasFilePathname == "" || node.associatedPADE.biasFilePathname == "N/A")
                    {
                        unSetPADES += "PADE " + node.associatedPADE.PADE_sn + '\n';
                    }
                }

                if (unSetPADES != "The following PADES were not successfully associated with bias files: \n")
                {
                    MessageBox.Show(unSetPADES);
                }

            }

            TB4.theSystemViewer.boxSee1.recordState(true);
            updateStatusText(true, counter.ToString() + " PADEs successfully associated with bias files.");
        }

        private void applyGlobalInitializationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (buttonNode.highlightedList.Count < 1)
            {
                MessageBox.Show("You haven't selected any PADES!");
            }
            else
            {
                uploadInit();

            }
        }

        private void burnFirmwareToSelectedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog newDialog = new OpenFileDialog();

            newDialog.Filter = "TB4 Files (*.tb4) | *.tb4";


            DialogResult result = newDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                //TB4.theFlash.simuFlash(newDialog.FileName);
            }
        }



        private void openSystemFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (System.IO.File.Exists(TB4.systemFileName))
            {

                try
                {
                    Process.Start("notepad.exe", TB4.systemFileName);

                }
                catch (Exception ex)
                {
                    MessageBox.Show("File open failed: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show(TB4.systemFileName + " does not exist.  Please open a valid system file first.");
            }
        }

        private void enableToolStripItems()
        {
            debugOptionsToolStripMenuItem.Enabled = true;
            optionsToolStripMenuItem.Enabled = true;
            initializationToolStripMenuItem.Enabled = true;
        }

        private void unHighlightInitializationToolStrip()
        {
            foreach (ToolStripDropDownItem item in initializationToolStripMenuItem.DropDownItems)
            {
                item.ForeColor = Color.Black;
            }
        }

        #endregion

        private void enableTemperatureMonitoringToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void changeBoxseeDisplayRegisterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string answer;
            cBoxForm newForm = new cBoxForm();
            newForm.Location = this.Location;
            newForm.ShowDialog(); //blocking

            //TB4.theSystemViewer
            if (newForm.choice != "")
            {
                buttonNode.registerToDisplay = newForm.choice;
                TB4.theSystemViewer.boxSee1.drawEnvironment();
            }

        }

        private void pingPADESToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //List<PADE> tempList = findAllPADES(false);
            List<PADE> tempList = (List<PADE>)TB4.PADE_List.Values;
            List<int> snList = new List<int>();

            bool foundFlag = false;
            bool mismatchFlag = false;

            string response = "The following PADES exist in the current 'system' but could not be reached: \n";

            foreach (KeyValuePair<int, PADE> pade in TB4.PADE_List)
            {
                //determine if the pade from PADE_List is in tempList
                foundFlag = false;
                foreach (PADE tempPade in tempList)
                {
                    if (tempPade.PADE_sn == pade.Value.PADE_sn) foundFlag = true;
                }
                if (!foundFlag)
                {
                    response += "PADE " + pade.Value.PADE_sn + "\n";
                    mismatchFlag = true;
                }
            }

            if (!mismatchFlag) MessageBox.Show("All the PADE's in the system could be reached.");
            else MessageBox.Show(response);
        }

        private void iVTestToolStripMenuItem_Click(object sender, EventArgs e)
        {

            SaveFileDialog sDialog = new SaveFileDialog();
            sDialog.Filter = "Text File (*.txt) | *.txt";
            DateTime n = DateTime.Now;
            sDialog.FileName = "IV" + n.Year.ToString() + n.Month.ToString("00") + n.Day.ToString("00") + "_" + n.Hour.ToString("00") + n.Minute.ToString("00") + n.Second.ToString("00");
            DialogResult result = sDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                StreamWriter swriter = new StreamWriter(sDialog.FileName.ToString());

                double[] x = new double[100];
                double[] y = new double[100];
                double ave = 0;
                double ave0 = 0;

                for (int i = 0; i < 32; i++)
                {
                    //iterate through each channel
                    UInt16 t = 1000;
                    TB4.theHist1.Offset_reg[i].RegWrite(t);
                    System.Threading.Thread.Sleep(1);
                }
                TB4.theHist1.Hist1_display();
                System.Threading.Thread.Sleep(1000);
                TB4.theHist1.flg_StopIV = false;
                updateStatusText(true, "Running Channel Test...");
                int p = 0;
                foreach (TreeNode node in TB4.thePADE_Selector.selectedPADEList.Nodes)
                {
                    p++;
                    TB4.activatePADE(getPADE(node.Tag.ToString()), false, false, true);
                    updateStatusBar(true, p / TB4.thePADE_Selector.selectedPADEList.Nodes.Count);

                    swriter.Write("PADE " + node.Tag.ToString() + ": ");

                    for (int k = 0; k < 32; k++)
                    {
                        if (TB4.theHist1.hist_chk_chan[k].Checked)
                        {
                            ave0 = 0;
                            swriter.Write("#CHAN" + k.ToString() + "#");
                            for (int i = 0; i <= 50; i++)
                            {
                                UInt16 t = Convert.ToUInt16(1000 - (i * 20));
                                TB4.theHist1.Offset_reg[k].RegWrite(t);
                                System.Threading.Thread.Sleep(50);
                                ave = 0;
                                x[i] = 0; y[i] = 0;
                                int num_ave = 2;
                                {
                                    for (int j = 0; j < num_ave; j++)
                                    {
                                        UInt16 m = TB4.theHist1.IMON_reg.RegRead();
                                        System.Threading.Thread.Sleep(10);
                                        ave += m;
                                    }

                                    ave = ave / num_ave;
                                    if (i == 2) { ave0 = ave; }
                                    x[i] = i * 20;
                                    //if ((ave - ave0) < 0) { ave = ave0 + 1; }
                                    //y[i] = Math.Log( ave - ave0,2);
                                    if (i > 2) { y[i] = (ave - ave0); } else { y[i] = 0; }

                                    swriter.Write(" (" + x[i].ToString() + " , " + y[i].ToString() + ") ");

                                    TB4.theHist1.btnScan.Text = i.ToString();
                                }
                                Application.DoEvents();
                                for (int ii = i; ii < 100; ii++) { x[ii] = x[i]; y[ii] = y[i]; }
                            }
                            TB4.theHist1.Hist1_IV(x, y, TB4.theHist1.arr_Curve_Col[k]);
                            TB4.theHist1.btnScan.Text = "SCAN";
                            UInt16 off = 1000;
                            TB4.theHist1.Offset_reg[k].RegWrite(off);
                            System.Threading.Thread.Sleep(500);
                        }
                    }
                }
                updateStatusBar(false, 0);
                updateStatusText(true, "Standby...");
            }
        }

        private void viewBiasWriteHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TB4.myTextDisplay.text = Registers.registerWriteHistory;
            TB4.myTextDisplay.button1.Text = "Save";
            DialogResult result = TB4.myTextDisplay.ShowDialog();

            if (result == DialogResult.OK)
            {
                SaveFileDialog sDialog = new SaveFileDialog();
                sDialog.Filter = "Text File (*.txt) | *.txt";

                DialogResult result2 = sDialog.ShowDialog();

                if (result2 == DialogResult.OK)
                {
                    StreamWriter sw = new StreamWriter(sDialog.FileName);
                    sw.Write(Registers.registerWriteHistory);
                    sw.Close();
                }

            }
            TB4.myTextDisplay.button1.Text = "Accept";

        }

        private void saveOffsetFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog newDialog = new FolderBrowserDialog();
            DialogResult result = newDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                string folderpath = newDialog.SelectedPath;
                string oldActive = TB4.ActivePADE.PADE_sn;

                List<TB4_Register> biasList = new List<TB4_Register>();

                for (int i = 0; i < 32; i++) biasList.Add(registerLookup("BIAS_OFFSET_CH" + i));
                biasList.Add(registerLookup("BIAS_DAC"));
                biasList.Add(registerLookup("BIAS_NUM_AVG"));

                foreach (TreeNode node in TB4.thePADE_Selector.selectedPADEList.Nodes)
                {
                    PADE clonePADE = getPADE(node.Tag.ToString());
                    activatePADE(node.Tag.ToString());

                    StreamWriter sw = new StreamWriter(folderpath + "/Offsets_" + DateTime.Now.ToShortDateString() + "_SN" + node.Tag.ToString());

                    foreach (TB4_Register reg in biasList)
                    {
                        sw.WriteLine(reg.name + "<=dec" + reg.RegRead().ToString());
                    }
                }


            }
        }

        private void viewLogFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string logFilePath = "C:\\PCTlog.txt";
            if (System.IO.File.Exists(logFilePath))
            {

                try
                {
                    Process.Start("notepad.exe", logFilePath);

                }
                catch (Exception ex)
                {
                    MessageBox.Show("File open failed: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show(logFilePath + " does not exist.  Please open a valid system file first.");
            }
        }
        string openPathname = "";
        string savePathname = "";

        void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                StreamReader sr = new StreamReader(openPathname);

                string text = sr.ReadToEnd();
                sr.Close();
                //MessageBox.Show(text);
                string fls = "";
                String[] elements = text.Split(new char[] { '\n' }, StringSplitOptions.None);
                //MessageBox.Show(elements.Length.ToString());
                // String[] op = { };
                // StringBuilder sb = new StringBuilder();
                int pgn = 0, ln = 0;
                DateTime now = DateTime.Now;
                string t;


                StreamWriter sw = new StreamWriter(savePathname);

                for (int i = 0; i < elements.Length - 1; i++)
                {
                    t = elements[i].ToLower();
                    if (t[1] == '1')
                    {
                        sw.WriteLine(":" + pgn.ToString("X").ToLower() + ":" + ln.ToString("X").ToLower() + ": " + t.Substring(9, 2) + " " + t.Substring(11, 2) + " " + t.Substring(13, 2) + " " + t.Substring(15, 2) + " " + t.Substring(17, 2) + " " + t.Substring(19, 2) + " " + t.Substring(21, 2) + " " + t.Substring(23, 2));
                        sw.WriteLine(":" + pgn.ToString("X").ToLower() + ":" + (ln + 1).ToString("X").ToLower() + ": " + t.Substring(25, 2) + " " + t.Substring(27, 2) + " " + t.Substring(29, 2) + " " + t.Substring(31, 2) + " " + t.Substring(33, 2) + " " + t.Substring(35, 2) + " " + t.Substring(37, 2) + " " + t.Substring(39, 2));

                        ln++;
                        if (ln == 63)
                        {
                            ln = 0;
                            pgn++;
                        }
                        else ln++;

                    }

                }
                sw.Close();
                MessageBox.Show("The conversion is complete.");
            }
            catch (Exception)
            {

            }
        }

        private void convertMCSToFLSToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            backgroundWorker1.DoWork += new DoWorkEventHandler(backgroundWorker1_DoWork);

            OpenFileDialog oDialog = new OpenFileDialog();
            oDialog.Filter = "MCS Files (.mcs)|*.mcs";
            oDialog.FilterIndex = 1;

            oDialog.Multiselect = false;

            DialogResult result1 = oDialog.ShowDialog();
            if (result1 == DialogResult.OK)
            {
                SaveFileDialog sDialog = new SaveFileDialog();
                sDialog.FileName = "result.fls";
                DialogResult result2 = sDialog.ShowDialog();
                if (result2 == DialogResult.OK)
                {
                    openPathname = oDialog.FileName;
                    savePathname = sDialog.FileName;

                    backgroundWorker1.RunWorkerAsync();


                }
            }
        }
    }
}

