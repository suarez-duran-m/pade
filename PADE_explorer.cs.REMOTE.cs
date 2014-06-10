using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Collections.Generic;



namespace PADE
{
  

    public partial class PADE_explorer : Form
    {

        public static Point subFormLocation;
        private TB4_Register tempRegister; //declaring and initializing this (see:form.load) ahead of time saves our butt when we read temperatures...
       
        public List<formPair> openForms = new List<formPair>(1);

     

        

        public PADE_explorer()
        {
            InitializeComponent();
        }


        

        #region Form Startup, Events, and Controls

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


        void PADE_explorer_ResizeEnd(object sender, System.EventArgs e)
        {
            Console.WriteLine("ResizeEnd event");
            for (int i = 0; i < openForms.Count; i++)
            {

                Form currentForm = openForms[i].form;

                Console.WriteLine(currentForm.Name);
                openForms[i].form.Invoke((MethodInvoker)delegate { openForms[i].form.Location = new System.Drawing.Point(this.Location.X + subFormLocation.X, this.Location.Y + subFormLocation.Y); });
                openForms[i].form.Activate();
            }
        }
        private void PADE_explorer_Load(object sender, EventArgs e)
        {
            
            //INITIALIZATION STUFF
            
            subFormLocation = new System.Drawing.Point(panel2.Location.X + 10, panel2.Location.Y + 30);
            tempRegister = registerLookup("PADE_TEMP");
            //--------------------

           // createPADES(); //used for testing purposes: remove when finished
            placeButtons();
            populateTreeview();
            updateStatusText(true, "Standby.");
            updateStatusBar(false, 0);

            TB4.thePADE_explorer = this;
            tempThread.Interval = PADE.samplePeriod;
            tempThread.Enabled = true;

            HookEvents();
        }

        /// <summary>
        /// This is a trick used to prevent child forms from disappearing behind the explorer when a control is clicked.
        /// </summary>
        private void HookEvents()
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
                        catch(Exception ex)
                        {
                            Console.WriteLine("Error adding event handler to control (line 105): " + ex.Message);
                        }

                    }

                }
            }
            treeView1.Click+=new EventHandler(PADE_explorer_Click);
            treeView1.NodeMouseClick += new TreeNodeMouseClickEventHandler(PADE_explorer_Click);
            selectedPADEList.Click += new EventHandler(PADE_explorer_Click);
            selectedPADEList.NodeMouseClick += new TreeNodeMouseClickEventHandler(PADE_explorer_Click);

        } 

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

                    newButton.Name = ctrl.Name;
                    newButton.Text = ctrl.Text;
                    newButton.Size = ctrl.Size;
                    newButton.BackColor = ctrl.BackColor;

                    newButton.Show();

                    panel1.Controls.Add(newButton);

                    if (counter <= 4) { newButton.Location = new Point(counter * 144 + 8, 5); }
                    else { newButton.Location = new Point((counter - 5) * 144 + 8, 40); }

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
        private void toggleForm(Form argument, Button control)
        {
            Type argtype=argument.GetType();
            formPair currentPair = new formPair(argtype, argument);
            if (argument.Visible)
            {
                Console.WriteLine("Made invisible");
                argument.Visible = false;
                control.Font = new Font(control.Font.Name, control.Font.Size, FontStyle.Regular);
                if (openForms.Contains(currentPair)) openForms.Remove(currentPair); //the if() is a safety net...
            }
            else
            {
                Console.WriteLine("Made visible");

                openForms.Add(currentPair);
                control.Font = new Font(control.Font.Name, control.Font.Size, FontStyle.Bold);
                argument.Visible = true;
                argument.Location = new System.Drawing.Point(this.Location.X + subFormLocation.X, this.Location.Y + subFormLocation.Y);
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

        void groupBox2_Click(object sender, System.EventArgs e)
        {
            if (treeView1.Width == 110)
            {
                //enlarge treeview and groupbox width-wise
                treeView1.Width = 200;
                groupBox2.Width = 211;
            }
            else
            {
                //shrink 
                treeView1.Width = 110;
                groupBox2.Width = 121;
            }
        }
        void groupBox3_Click(object sender, System.EventArgs e)
        {
            if (selectedPADEList.Width == 110)
            {
                selectedPADEList.Width = 200;
                groupBox3.Width = 211;
            }
            else
            {
                selectedPADEList.Width = 110;
                groupBox3.Width = 121;
            }
        }
        #endregion

        #region Treeview Controls
        private void populateTreeview()
        {
            PADE item;

            if (TB4.PADE_List != null)
            {
                for (int i = 1; i <= TB4.PADE_List.Count; i++)
                {
                    TB4.PADE_List.TryGetValue(i, out item);
                    addPADENode(item);
                }
                    
            }
        }

        private void addPADENode(PADE item)
        {

            //create parent (the PADE unit)
            TreeNode parent;


            //create an array with every field in the class (IP, MAC address, etc.)
            FieldInfo[] fields = item.GetType().GetFields();

            //create a list in which to contain each field that is part of a given PADE object
            //List used instead of an array because its easier to add members
            System.Collections.Generic.List<TreeNode> nodeList = new System.Collections.Generic.List<TreeNode>();

            foreach (FieldInfo field in fields)
            {  //access each field of the object

                TreeNode memberNode; //a field, such as "IP address"

                TreeNode[] elaborator;

                Console.WriteLine(field.Name);

                if (field.Name == "IP4_add") //parse the IP address, add it as a child node
                {
                    byte[] IPAr = (byte[])field.GetValue(item);

                    elaborator = new TreeNode[1];
                    elaborator[0] = new TreeNode(IPAr[3].ToString() + "." + IPAr[2].ToString() + "." + IPAr[1].ToString()
                                                 + "." + IPAr[0].ToString());

                    memberNode = new TreeNode(field.Name, elaborator);
                    memberNode.Name = field.Name;

                }
                else if (field.Name == "MAC_add") //parse the MAC address and add it as a child node
                {
                    byte[] MACAr = (byte[])field.GetValue(item);

                    string MACADD = MACAr[5].ToString();
                    for (int a = 4; a > -1; a--) MACADD += ":" + MACAr[a].ToString();

                    elaborator = new TreeNode[1] { new TreeNode(MACADD) };

                    memberNode = new TreeNode(field.Name, elaborator);
                    memberNode.Name = field.Name;

                }
                else if (field.Name == "PADE_ch_enable") //add the boolean value for each channel as children
                {
                    bool[] ch_enable_Ar = (bool[])field.GetValue(item); //32 element array of bool

                    elaborator = new TreeNode[32];

                    for (int a = 0; a < 32; a++) elaborator[a] = new TreeNode("Channel " + (a + 1).ToString() + ": " + ch_enable_Ar[a].ToString());

                    memberNode = new TreeNode(field.Name, elaborator);
                    memberNode.Name = field.Name;

                }
                else if (field.Name == "PADE_FTDI") //display the IsOpen property of the object
                {
                    FTD2XX_NET.FTDI p_FTDI = (FTD2XX_NET.FTDI)field.GetValue(item);
                    string FTDISTATUS;
                    if (p_FTDI.IsOpen) FTDISTATUS = "Open";
                    else FTDISTATUS = "Closed";

                    elaborator = new TreeNode[1] { new TreeNode(FTDISTATUS) };

                    memberNode = new TreeNode(field.Name, elaborator);
                    memberNode.Name = field.Name;

                }
                else if (field.Name == "PADE_List")
                {
                    //Do nothing...PADE_COLLECTION is used for testing
                    memberNode = null;
                }
                else if (field.FieldType.IsArray == false) //most properties will be evaluated in this block
                {
                    elaborator = new TreeNode[1];

                    elaborator[0] = new TreeNode(field.GetValue(item).ToString());

                    memberNode = new TreeNode(field.Name, elaborator);
                    memberNode.Name = field.Name;

                }

                else
                {
                    elaborator = new TreeNode[1] { new TreeNode("Unknown Type") };
                    memberNode = new TreeNode(field.Name, elaborator);
                    memberNode.Name = field.Name;

                }

                if (memberNode != null) nodeList.Add(memberNode);

            }

            TreeNode[] children = nodeList.ToArray();

            parent = new TreeNode("PADE " + item.PADE_sn, children);

            treeView1.Nodes.Add(parent);


        }

        private void treeView1_NodeClicked(object sender, System.Windows.Forms.TreeNodeMouseClickEventArgs args)
        {
            if (args.Button == MouseButtons.Left)
            {
                TreeNode tempNode = args.Node;

                while (tempNode.Parent != null) { tempNode = tempNode.Parent; } //retrieve top-level parent 

                selectedPADE_label.Text = tempNode.Nodes["PADE_sn"].Nodes[0].Text;

                TB4.activatePADE(getPADE(tempNode.Nodes["PADE_sn"].Nodes[0].Text), true, false);

                if (args.Node.Text == "Temperature")  //if the user clicks the temperature node, open up the temperature plot
                {
                    
                    
                    openForms.Add(new formPair(TB4.theTempPlot.GetType(), TB4.theTempPlot));
                    TB4.theTempPlot.Visible = true;
                    TB4.theTempPlot.Show();
                    
                  
                }
            }
            else
            {
                if (args.Node.Parent == null) //only add a node if the user clicked the top-level parent
                {
                    //the user wants to add/remove one of the PADEs from the full list to the selected list
                    args.Node.Collapse();
                    TreeNode removeNode = null;
                    for (int i = 0; i < selectedPADEList.Nodes.Count; i++) if (selectedPADEList.Nodes[i].Text == args.Node.Text) removeNode = selectedPADEList.Nodes[i];


                    if (removeNode != null) //the double-clicked node is already in the list, so remove it
                    {
                        selectedPADEList.Nodes.Remove(removeNode);
                    }
                    else //new node, add it to the selected node list (and preserve alphanumeric ordering)
                    {
                        int i;
                        int SN = Convert.ToInt16(args.Node.Nodes["PADE_sn"].Nodes[0].Text.ToString());
                        for (i = 0; i < selectedPADEList.Nodes.Count && (SN >= Convert.ToInt16(selectedPADEList.Nodes[i].Nodes["PADE_sn"].Nodes[0].Text.ToString())); i++) { };
                        TreeNode selectedNode = (TreeNode)args.Node.Clone();
                        selectedPADEList.Nodes.Insert(i, selectedNode);
                        //selectedPADEList.Nodes[i].Tag = args.Node.Nodes["PADE_sn"].Text;
                    }
                }
            }
   
        }

        void selectedPADEList_NodeMouseClick(object sender, System.Windows.Forms.TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                e.Node.Remove();
            }
        }

        #endregion

        #region StatusBar Control
        private void updateStatusBar(bool showBar, int value)
        {
            toolStripProgressBar1.Visible = showBar;
            if (value <= 100) toolStripProgressBar1.Value = value;
            else toolStripProgressBar1.Value = 100;

        }

        
        private void updateStatusText(bool showText, string value)
        {
            statustextType newDel = new statustextType(p_updateStatusText);
            System.Threading.Thread redrawThread = new System.Threading.ParameterizedThreadStart(newDel(showText, value));


        }
        private delegate void statustextType(bool text, string value);
        private void p_updateStatusText(bool showText, string value)
        {

            toolStripStatusLabel1.Visible = showText;
            toolStripStatusLabel1.Text = value;
           
        }
        #endregion

        #region Init Buttons
        private void uploadInitButton_Click(object sender, EventArgs e)
        {
            //first, build a string array containing all the SN's of the PADEs the user wants to initialize.
            string[] PADEList = new string[selectedPADEList.Nodes.Count];

            for (int i = 1; i <= selectedPADEList.Nodes.Count; i++)
            {
                PADEList[i] = selectedPADEList.Nodes[i].Tag.ToString();

            }
            //Initialize the PADEs using the created array.
            writeInitToPADE(PADEList);

        }

        /// <summary>
        /// This subroutine will attempt to load each of the PADE's in the selected PADE list with register values from a file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uploadRegDataButton_Click(object sender, EventArgs e)
        {
            //the user wants to upload a config file.  Create a popup box to ask for a pathname.

            updateStatusText(true, "Uploading Initialization File...");

            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "Init Files (.dat)|*.dat";
            openFileDialog.FilterIndex = 1;

            openFileDialog.Multiselect = false;

            DialogResult userClickedOK = openFileDialog.ShowDialog();
            string filename = openFileDialog.FileName;

            if (userClickedOK == DialogResult.OK)
            {
                
                if (System.IO.File.Exists(filename))
                {

                    foreach (TreeNode item in selectedPADEList.Nodes)
                    {

                        PADE tempPADE = PADE_explorer.getPADE(item.Nodes["PADE_sn"].Nodes[0].Text);
                         
                        TB4_Register.applyRegisterList(tempPADE.PADE_sn);

                    }
                }
                else
                {
                    updateStatusText(true, "Initialization Failed: File Does Not Exist (" + filename + ")");
                }

            }
            else
            {
                updateStatusText(true, "Standby.");
            }
        }
        #endregion

        #region Utilities

      

        /// <summary>
        /// This writes the initialization files to each PADE in the argument array.
        /// </summary>
        /// <param name="PADEList"></param>
        private void writeInitToPADE(string[] PADEList)
        {
            

            updateStatusBar(true, 0);

            updateStatusText(true, "Uploading Initialization File...");

            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "Init Files (.dat)|*.dat";
            openFileDialog.FilterIndex = 1;

            openFileDialog.Multiselect = false;

            DialogResult userClickedOK = openFileDialog.ShowDialog();




            if (userClickedOK == DialogResult.OK)
            {
                string initFileLocation = openFileDialog.FileName;
                for (int i = 1; i <= PADEList.Length; i++)
                { //iterate through all PADE's

                    this.toolStripStatusLabel1.Text = "Attempting Initialization of PADE" + PADEList[i];


                    Boolean exists = false; //determine if the PADE is a member of TB4.PADE_List
                    for (int j = 0; i < TB4.PADE_List.Count; j++) if (TB4.PADE_List[j].PADE_sn == PADEList[i]) exists = true;

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
                        }
                        catch (Exception ex)
                        {
                            this.toolStripStatusLabel1.Text = "Initialization Failed: " + ex.Message;
                            updateStatusBar(false, 0);
                            return;
                        }

                    }


                }
            }

            this.toolStripStatusLabel1.Text = "Initialization Successful!";
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
                    TB4.activatePADE(activePADE, false, false );
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
                if (TB4.PADE_List[i].PADE_sn == SN)
                {
                    
                    returnPADE = TB4.PADE_List[i];
                }
            }
            return returnPADE;
        }

        public static TB4_Register registerLookup(string registerName)
        {
            TB4_Register returnReg;
            try
            {
                returnReg = TB4.my_reg_collection.Find(delegate(TB4_Register reg) { return reg.name == registerName; });
                if (returnReg == null)
                {
                    MessageBox.Show("Register lookup failed: " + registerName);
                }
                return returnReg;
                
            }
            catch(Exception ex)
            {
                TB4_Exception.throwException(TB4_Exception.severity.error, "PADE_explorer", ex);
                return null;
            }

        }

        public tempStamp getTemperature(PADE targetPADE)
        {
            tempStamp returnTemp = new tempStamp();

            TB4.activatePADE(targetPADE, false, false);
            returnTemp.temperature = tempRegister.RegRead();
            returnTemp.time = DateTime.Now;

            return returnTemp;
        }

        Random newRand=new Random();
        public tempStamp getFakeTemperature(PADE targetPADE) //used for testing
        {
            tempStamp returnStamp = new tempStamp(); 
            returnStamp.temperature=(UInt16) newRand.Next(0, 30);
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
        public bool searchForPADE(string SN)
        {

            PADE newPADE=new PADE();
            newPADE.PADE_sn=SN.ToString();
            newPADE.IP4_add[0]=Convert.ToByte(SN);
            newPADE.MAC_add[0]=Convert.ToByte(SN);

            Console.Write("PADE IP ADD: ");
            for (int i = 0; i < 4; i++) { Console.Write(newPADE.IP4_add[i].ToString()); }

            TB4.activatePADE(newPADE, true, false);

            TB4_Register verReg=registerLookup("FIRMWARE_VER");
            ushort newthing = verReg.RegRead();
            Console.WriteLine(SN + "   " + newthing.ToString());
            if (newthing > 0)
            {
               
                return true;
            }

            return false;
        }

        public void findAllPADES()
        {

            treeView1.Nodes.Clear();
            TB4.PADE_List.Clear();

            for (int i = 1; i <= 100; i++)
            {
                if (searchForPADE(i.ToString()))
                {
                    PADE newPADE = new PADE();
                    newPADE.PADE_sn = i.ToString();

                    TB4.activatePADE(newPADE, true, true);
                    
                    addPADENode(newPADE);
                    
                }
            }
            if (TB4.PADE_List.Count>0) selectedPADE_label.Text = TB4.PADE_List[1].PADE_sn;

            for (int i = 1; i <= TB4.PADE_List.Count; i++) Console.WriteLine("PADE " + TB4.PADE_List[i].PADE_sn + "........");
        }
        #endregion

        /// <summary>
        /// Reads all temperature registers, updates graph (if open), runs analysis tools
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tempThread_Tick(object sender, EventArgs e)
        {

            tempStamp newTemp;

            for (int i = 1; i <= TB4.PADE_List.Count; i++) //iterate through all PADEs to read the temperature register
            {
                if (TB4.PADE_List[i].recordTemperature)
                { 
                    //if the user wants to record the temperature of this PADE
                    //newTemp=getTemperature(TB4.PADE_List[i]);
                    newTemp = getFakeTemperature(TB4.PADE_List[i]); //used for testing
                    TB4.PADE_List[i].addTemp(newTemp);
                    

                    if (TB4.ActivePADE == TB4.PADE_List[i] && TempPlot.formOpen)
                    {
                        //the graph is currently open to the same PADE. We need to update it in real time
                        TB4.theTempPlot.addPoint(newTemp, true);
                        TB4.ActivePADE.addTemp(newTemp);

                        if (TB4.ActivePADE.analysisEnabled)
                        {
                            
                            TB4.theTempPlot.averageTextBox.Text = TB4.ActivePADE.averageTemp.ToString();
                            
                            TB4.theTempPlot.minimumTextBox.Text = TB4.ActivePADE.minTemp.temperature.ToString();
                            TB4.theTempPlot.minTimeBox.Text = TB4.ActivePADE.minTemp.time.ToString();
                            TB4.theTempPlot.maximumTextBox.Text = TB4.ActivePADE.maxTemp.temperature.ToString();
                            TB4.theTempPlot.maxTimeBox.Text = TB4.ActivePADE.maxTemp.time.ToString();

                        }
                    }
                }
            }
        }

        private void makeRegisterListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TB4_Register.createRegisterList();
        }

        private void debuggingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            (new PerformanceLog()).Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void registerBaseUnitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Label newLabel = new Label();
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



            newButton.Click+=(_sender, _e)=>{ 
                int num;
                    
                try
                {
                    num=Convert.ToInt16(newBox.Text);
                    if(num>1 && num<=16) TB4.theRegistersForm.returnBase=Convert.ToInt16(newBox.Text); 
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

        private void findAllPADEsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            findAllPADES();
        }

        private void addPADEToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form newForm = new Form();
            Label newLabel=new Label();
            TextBox newBox = new TextBox();
            Button newButton = new Button();

            newForm.Size = new Size(100, 100);
            newBox.Size = new Size(25, 25);
            newButton.Size = new Size(50, 25);
            newBox.Location = new Point(38, 0);
            newButton.Location = new Point(25, 30);

            newForm.Controls.Add(newBox);
            newForm.Controls.Add(newButton);

            newForm.Show();

            newButton.Click += new EventHandler(delegate(object O, EventArgs a)
            {
                PADE newPADE=new PADE();
                newPADE.PADE_sn = newBox.Text;
                TB4.activatePADE(newPADE, true, true);
                addPADENode(newPADE);
            });

        }

        private void downloadInitButton_Click(object sender, EventArgs e)
        {
            TB4_Register.createRegisterList();
        }
            
         
   


        }

        





    }

