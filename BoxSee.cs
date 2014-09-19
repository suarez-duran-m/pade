using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Collections.ObjectModel; //for the observablecollection<t>
using System.IO;

namespace PADE
{
    public partial class BoxSee : Panel, IMessageFilter
    {
        public Graphics graphicObj = null;

        private Point startPoint, endPoint;
        private bool dragFlag = false;
        private bool initialized = false;
        private Timer timer1 = new Timer();
        public bool controlsSystemFile = false;




        public BoxSee()
        {
            this.BorderStyle = BorderStyle.FixedSingle;
            this.MouseDown += new MouseEventHandler(BoxSee_MouseDown);
            this.MouseUp += new MouseEventHandler(BoxSee_MouseUp);
            this.MouseMove += new MouseEventHandler(BoxSee_MouseMove);
            this.Bounds = new Rectangle(new Point(this.Location.X, this.Location.Y), new Size(this.Width, this.Height));
            timer1.Interval = 20;
            Application.AddMessageFilter(this);
            this.Disposed += new EventHandler(BoxSee_Disposed);
        }

        void BoxSee_Disposed(object sender, EventArgs e)
        {
            graphicObj.Dispose();
            timer1.Dispose();
        }

        ContextMenu cm = new ContextMenu();

        /// <summary>
        /// This must be called after the Size and Location of the form have been declared.
        /// </summary>
        public void Initialize()
        {
            graphicObj = CreateGraphics();
            timer1.Tick += new EventHandler(timer1_Tick);
            ContextMenu cm = new ContextMenu();
            this.ContextMenu = cm;
            nodeMoveTimer.Interval = 50;
            nodeMoveTimer.Tick += new EventHandler(nodeMoveTimer_Tick);

        }


        #region State Recording
        public bool recordStateChanges = false;
        public void recordState(bool assertChange=false) //assert change makes the change regardless of the boolean flag
        {
            if (recordStateChanges || assertChange)
            {
                controlsSystemFile = true;
                StreamWriter sWriter = new StreamWriter(TB4.systemFileName, false);

                sWriter.WriteLine("System File Last Modified: " + DateTime.Now.ToUniversalTime());

                foreach (padeCluster cluster in padeCluster.clusterList)
                {
                    sWriter.WriteLine("CLUSTER: " + cluster.name);

                    foreach (buttonNode pade in cluster.padeList)
                    {
                        sWriter.WriteLine("--PADE: ");
                        sWriter.WriteLine("       SN:           " + pade.associatedPADE.PADE_sn);
                        sWriter.WriteLine("       SIB #:        " + pade.associatedPADE.SIB_ID);
                        sWriter.WriteLine("       REG FILE:     " + pade.associatedPADE.registerFile);
                        sWriter.WriteLine("       BIAS FILE:    " + pade.associatedPADE.biasFilePathname);
                        sWriter.WriteLine("       INIT FILE:    " + pade.associatedPADE.initializationFilePathname);
                        sWriter.WriteLine("       FIRM VERSION: " + pade.associatedPADE.PADE_fw_ver);
                        sWriter.WriteLine("       IS SYS MASTER:" + pade.associatedPADE.PADE_is_MASTER);

                    }

                }

                sWriter.Close();
                controlsSystemFile = false;
            }
        }

        public void clearState()
        {
            for (int i = 0; i < padeCluster.clusterList.Count; i++)
            {
                for (int j = 0; j < padeCluster.clusterList[i].padeList.Count; j++)
                {
                    this.Controls.Remove(padeCluster.clusterList[i].padeList[j]);
                    padeCluster.clusterList[i].padeList[j].hideBox(true);
                    padeCluster.clusterList[i].padeList[j].Dispose();
                    buttonNode.nodeList.Remove(padeCluster.clusterList[i].padeList[j]);
                    padeCluster.clusterList[i].padeList.Remove(padeCluster.clusterList[i].padeList[j]);


                    j -= 1;
                }

                padeCluster.clusterList.Remove(padeCluster.clusterList[i]);
                i -= 1;
            }
            buttonNode.notBiasedList.Clear();
            buttonNode.notInitializedList.Clear();
            Console.WriteLine(padeCluster.clusterList.Count);
            Console.WriteLine(buttonNode.nodeList.Count);
            initializeShapes();
            drawpadeClusters();
        }

        /// <summary>
        /// Use a system file to create a new system environment.
        /// </summary>
        /// <param name="pathname"></param>
        /// <param name="draw">Determines whether to redraw the BoxSee</param>
        public List<PADE> loadState(string pathname, bool draw)
        {
            //clean current environment
            clearState();
            List<PADE> returnList = new List<PADE>();

            if (System.IO.File.Exists(pathname))
            {
                //we need to parse the file, and create clusters->pades accordingly
                controlsSystemFile = true;
                StreamReader sReader = new StreamReader(pathname);

                string[] perCluster = sReader.ReadToEnd().Split(new string[]{"CLUSTER:"}, StringSplitOptions.RemoveEmptyEntries);

                for (int i = 1; i < perCluster.Length; i++)
                {
                    string[] perPade = perCluster[i].Split(new string[] {"--PADE:"}, StringSplitOptions.RemoveEmptyEntries);

                    //first, create the cluster
                    string[] DH3 = perPade[0].Split(new char[] { '\n' });
                    string clusterName="";
                    //for (int j = 1; j < DH3.Length; j++) clusterName = clusterName + DH3[j];
                    clusterName = DH3[0];

                    padeCluster newCluster = new padeCluster(Color.Blue, clusterName.Trim(), this);

                    //next, add PADEs to the cluster (in a very unsophisticated way)
                    
                    for (int j = 1; j < perPade.Length; j++)
                    {
                        DH3 = perPade[j].Split(new char[] { '\n' });
                        PADE newPade = new PADE();

                        string[] perLine = DH3[1].Split(new char[] { ':' });
                        newPade.PADE_sn = perLine[1].Trim();

                        perLine = DH3[2].Split(new char[] { ':' });
                        newPade.SIB_ID = perLine[1].Trim();

                        perLine = DH3[3].Split(new char[] { ':' });
                        newPade.registerFile = perLine[1].Trim();

                        perLine = DH3[4].Split(new char[] { ':' });
                        newPade.biasFilePathname = "";
                        for (int k = 1; k < perLine.Length; k++)
                        {
                            newPade.biasFilePathname += perLine[k].Trim();
                            if (k == 1) newPade.biasFilePathname += ':';
                        }

                        perLine = DH3[5].Split(new char[] { ':' });
                        newPade.initializationFilePathname = "";
                        for (int k = 1; k < perLine.Length; k++)
                        {
                            newPade.initializationFilePathname += perLine[k].Trim();
                            if (k == 1) newPade.initializationFilePathname += ':';
                        }

                        perLine = DH3[6].Split(new char[] { ':' });
                        newPade.PADE_fw_ver = Convert.ToUInt16(perLine[1].Trim());

                        try
                        {
                            perLine = DH3[7].Split(new char[] { ':' });
                            newPade.PADE_is_MASTER = Convert.ToBoolean(perLine[1].Trim());
                            // ARE YOU FKING KIDDING ME?!?!?!?!?!
                            //if (newPade.PADE_is_MASTER) newPade.PADE_is_SLAVE = false;
                        }
                        catch (Exception ex)
                        {
                            //user tried to use an old version of the system file
                        }
                        returnList.Add(newPade);
                        newCluster.addNode(Color.Blue, newPade);
                    }
                }

                if (draw)
                {
                    this.initializeShapes();
                    this.drawpadeClusters();
                    TB4.thePADE_Selector.updateTreeview();
                    buttonNode.drawButtons();
                    this.Invalidate();


                }

                sReader.Close();
                controlsSystemFile = false;
            }
            else
            {
                MessageBox.Show("The system file pathname is incorrect.  Please change it in the menu bar.");
                
            }
            return returnList;
        }





        #endregion

        void timer1_Tick(object sender, EventArgs e)
        {
            Pen bluePen = new Pen(Color.Blue);
            graphicObj.DrawLine(new Pen(SystemColors.Control), lastStartPoint, new Point(lastStartPoint.X, lastCurrentPoint.Y));
            graphicObj.DrawLine(new Pen(SystemColors.Control), lastStartPoint, new Point(lastCurrentPoint.X, lastStartPoint.Y));
            graphicObj.DrawLine(new Pen(SystemColors.Control), new Point(lastStartPoint.X, lastCurrentPoint.Y), new Point(lastCurrentPoint.X, lastCurrentPoint.Y));
            graphicObj.DrawLine(new Pen(SystemColors.Control), new Point(lastCurrentPoint.X, lastStartPoint.Y), new Point(lastCurrentPoint.X, lastCurrentPoint.Y));

            graphicObj.DrawLine(bluePen, startPoint, new Point(startPoint.X, mouseLoc.Y));
            graphicObj.DrawLine(bluePen, startPoint, new Point(mouseLoc.X, startPoint.Y));
            graphicObj.DrawLine(bluePen, new Point(startPoint.X, mouseLoc.Y), new Point(mouseLoc.X, mouseLoc.Y));
            graphicObj.DrawLine(bluePen, new Point(mouseLoc.X, startPoint.Y), new Point(mouseLoc.X, mouseLoc.Y));
            bluePen.Dispose();

            lastStartPoint = startPoint;
            lastCurrentPoint = mouseLoc;
        }

        /// <summary>
        /// Before this method is called, the application must create some padeClusters.
        /// </summary>
        public void initializeShapes()
        {
            double numberofClusters = padeCluster.clusterList.Count;
            initialized = true;

            for (int i = 0; i < padeCluster.clusterList.Count; i++)
            {
                int width = (int)(this.Size.Width / (10 * numberofClusters + 8));
                int buttonheight = (int)(this.Size.Height / ((padeCluster.clusterList[i].padeList.Count / numberofClusters) / 4 + 1)) - 20;
                int minVal = Math.Min(width, buttonheight);
                int height = (int)((padeCluster.clusterList[i].padeList.Count - 1) / 3 + 1) * (3 * minVal / 2) + 2;

                //if (padeCluster.clusterList[i].padeList.Count > 3) padeCluster.clusterList[i].Size = (new Size(9 * minVal / 2 +1, height));
                //else padeCluster.clusterList[i].Size = (new Size(padeCluster.clusterList[i].padeList.Count * 3 * minVal / 2 +1, height));

                if (padeCluster.clusterList[i].padeList.Count > 3) padeCluster.clusterList[i].Size = (new Size(9 * minVal / 2 -14, height-13));
                else padeCluster.clusterList[i].Size = (new Size(padeCluster.clusterList[i].padeList.Count * 3 * minVal / 2 -14, height-13));

                padeCluster.clusterList[i].Location = new Point(((this.Size.Width - ((int)numberofClusters + 1) * padeCluster.clusterList[i].Size.Width+padeCluster.clusterList[i].Size.Width) / ((int)numberofClusters + 1)) * (1 + i) + i * padeCluster.clusterList[i].Size.Width, (int)(0.5 * (this.Size.Height - padeCluster.clusterList[i].Size.Height)));
                for (int j = 0; j < padeCluster.clusterList[i].padeList.Count; j++)
                {
                    padeCluster.clusterList[i].padeList[j].Size = new Size(minVal, minVal);
                    padeCluster.clusterList[i].padeList[j].Location = new Point(padeCluster.clusterList[i].Location.X + (j % 3) * (3 * padeCluster.clusterList[i].padeList[0].Size.Width / 2) + 2, padeCluster.clusterList[i].Location.Y + ((int)(j / 3)) * (3 * padeCluster.clusterList[i].padeList[0].Size.Height / 2)+2);

                }
            }

        }

        #region Text Overlay
        public List<textOverlay> textOverlayList = new List<textOverlay>();
        public class textOverlay
        {
            public string overlayName; //used for organization
            public string text;
            public Color textColor;

            public textOverlay(string newoverlayName, string newText, Color newColor)
            {
                overlayName = newoverlayName;
                text = newText;
                textColor = newColor;
            }
        }

        public void addTextOverlay(string overlayName, string text, Color textColor)
        {
            if (text != "")
            {
                textOverlayList.Add(new textOverlay(overlayName, text, textColor));
            }
        }

        public int findOverlayIndex(string overlayName)
        {
            for (int i = 0; i < textOverlayList.Count; i++)
            {
                if (textOverlayList[i].overlayName == overlayName) return i;
            }
            return -1;
        }
        #endregion

        #region Mouse Events
        public Point mouseLoc;

        [DllImport("user32.dll")]
        public static extern IntPtr WindowFromPoint(Point pt);
        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wp, IntPtr lp);

        public bool PreFilterMessage(ref Message m)
        {
            //this code stolen from internet
            if (m.Msg == 0x20a)
            {
                // WM_MOUSEWHEEL, find the control at screen position m.LParam
                Point pos = new Point(m.LParam.ToInt32() & 0xffff, m.LParam.ToInt32() >> 16);
                IntPtr hWnd = WindowFromPoint(pos);
                //if there's ever a problem in this function, I'm supposed to check hWnd!=m.hWnd in the below
                //if statement...but it was messing with my life so I removed it. -ryan
                //if (hWnd != IntPtr.Zero && Control.FromHandle(hWnd) != null)
                {
                    Console.WriteLine(Cursor.Position.X + "     " + Cursor.Position.Y);
                    if (Cursor.Position.X > this.PointToScreen(Point.Empty).X && Cursor.Position.Y > this.PointToScreen(Point.Empty).Y && Cursor.Position.X<this.PointToScreen(Point.Empty).X+this.Width && Cursor.Position.Y<this.PointToScreen(Point.Empty).Y+this.Height)
                    {
                        SendMessage(hWnd, m.Msg, m.WParam, m.LParam);
                        mouseWheel_Turned(m);
                        return true;
                    }
                }
            }
            return false;
        }

        public void mouseWheel_Turned(Message message)
        {
            //get mouse coordinates
            // mouseLoc.X = message.LParam.ToInt32() & 0xfff;
            // mouseLoc.Y = (message.LParam.ToInt32() & 0xfff0000) >> 16;
            //wparam corresponds to mouse scroll direction
            //if up, wparam=0x780000.  if down, wparam=0xffffffffff880000
            
            if (message.WParam.ToInt64() == 0x780000)
            {
                //zoom in
                    zoomIn(mouseLoc);
                
            }
            else if ((message.WParam.ToInt64() & 0x880000) == 0x880000)
            {
                //zoom out
                
                    zoomOut(mouseLoc);
                
            }


        }




        Point lastStartPoint = new Point(0, 0);
        Point lastCurrentPoint = new Point(0, 0);
        void BoxSee_MouseMove(object sender, MouseEventArgs e)
        {
            mouseLoc = e.Location;

        }

        void BoxSee_MouseUp(object sender, MouseEventArgs e)
        {

            timer1.Enabled = false;
            
            if (dragFlag)
            {
                endPoint = e.Location;
                foreach (buttonNode btn in buttonNode.nodeList)
                {
                    if (btn.Location.X < System.Math.Max(startPoint.X, endPoint.X) && btn.Location.X > System.Math.Min(e.X, startPoint.X) && btn.Location.Y < System.Math.Max(e.Y, startPoint.Y) && btn.Location.Y > System.Math.Min(e.Y, startPoint.Y))
                    {
                        if (buttonNode.highlightedList.Contains(btn)) buttonNode.highlightedList.Remove(btn);
                        else buttonNode.highlightedList.Add(btn);
                    }
                }
                buttonNode.drawButtons();
                this.Invalidate();
                dragFlag = false;

                Pen erasePen = new Pen(SystemColors.Control);
                graphicObj.DrawLine(erasePen, lastStartPoint, new Point(lastStartPoint.X, lastCurrentPoint.Y));
                graphicObj.DrawLine(erasePen, lastStartPoint, new Point(lastCurrentPoint.X, lastStartPoint.Y));
                graphicObj.DrawLine(erasePen, new Point(lastStartPoint.X, lastCurrentPoint.Y), new Point(lastCurrentPoint.X, lastCurrentPoint.Y));
                graphicObj.DrawLine(erasePen, new Point(lastCurrentPoint.X, lastStartPoint.Y), new Point(lastCurrentPoint.X, lastCurrentPoint.Y));
                
                
            }
            highlightListChanged.DynamicInvoke(new object[] { null, null });
            this.Invalidate();




        }

        padeCluster rightClickCluster = null;
        buttonNode rightClickPADE = null;
        public void BoxSee_MouseDown(object sender, MouseEventArgs e)
        {
            Point mouseCoords = e.Location;
            if (e.Button == MouseButtons.Left && sender == this)
            {
                if (!nodeMoveFlag)
                {


                    startPoint = e.Location;
                    dragFlag = true;
                    if (Control.ModifierKeys == Keys.Control && e.Button == MouseButtons.Left)
                    {
                        //the "multi-select" is actually handled in the else statement...this exists in case I
                        //ever need to add something here
                    }
                    else
                    {
                        buttonNode.highlightedList.Clear();
                        buttonNode.drawButtons();
                        this.Invalidate();
                    }
                    if (Math.Abs(startPoint.X - mouseLoc.X) < 5 && Math.Abs(startPoint.Y - mouseLoc.Y) < 5 && Control.ModifierKeys != Keys.Control)
                    {
                        //so, the user wasnt actually doing a drag-select, it was just
                        //a regular "deselect everything" click
                        buttonNode.highlightedList.Clear();
                        buttonNode.drawButtons();
                        this.Invalidate();
                    }
                    lastStartPoint = startPoint;
                    lastCurrentPoint = mouseLoc;
                    timer1.Enabled = true;
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                //context menu! the contents depend upon where the pointer is

                cm.MenuItems.Clear();

                if (sender != this) mouseCoords = new Point(((buttonNode)sender).Location.X + e.Location.X, ((buttonNode)sender).Location.Y + e.Location.Y);

                if (recordStateChanges) cm.MenuItems.Add("Disable State Recording", new EventHandler((object sender2, EventArgs e2) => { recordStateChanges = !recordStateChanges; }));
                else cm.MenuItems.Add("Enable State Recording", new EventHandler((object sender2, EventArgs e2) => { recordStateChanges = !recordStateChanges; recordState(); }));

                foreach (padeCluster cluster in padeCluster.clusterList)
                {
                    if (mouseCoords.X > cluster.Location.X && mouseCoords.X < (cluster.Location.X + cluster.Size.Width) && mouseCoords.Y > cluster.Location.Y && mouseCoords.Y < (cluster.Location.Y + cluster.Size.Height))
                    {
                        //the pointer is inside a cluster
                        rightClickCluster = cluster;
                        foreach (buttonNode pade in cluster.padeList)
                        {
                            //the click is inside a button node
                            if (mouseCoords.X > pade.Location.X && mouseCoords.X < (pade.Location.X + pade.Size.Width) && mouseCoords.Y > pade.Location.Y && mouseCoords.Y < (pade.Location.Y + pade.Size.Height))
                            {
                                rightClickPADE = pade;
                                cm.MenuItems.Add("Move PADE " + pade.associatedPADE.PADE_sn, new EventHandler(movePADENode));
                                cm.MenuItems.Add("Remove PADE " + pade.associatedPADE.PADE_sn, new EventHandler(removePADENode));
                               //please forgive the following lambda expressions
                                cm.MenuItems.Add("View PADE " + pade.associatedPADE.PADE_sn + " Bias File", new EventHandler((object o, EventArgs arg) => { 
                                    try 
                                    { 
                                        FileStream stream = new FileStream(pade.associatedPADE.biasFilePathname.ToString(), System.IO.FileMode.Open);
                                        System.IO.StreamReader sr = new StreamReader(stream); 
                                        TB4.myTextDisplay.text = sr.ReadToEnd(); TB4.myTextDisplay.ShowDialog(); 
                                    } 
                                    catch (Exception ex)
                                    { MessageBox.Show("There was a problem opening " + pade.associatedPADE.biasFilePathname + ".  " + ex.Message); }
                                }));

                                cm.MenuItems.Add("View PADE " + pade.associatedPADE.PADE_sn + " Global Init File", new EventHandler((object o, EventArgs arg) => { try { System.IO.StreamReader sr = new StreamReader(pade.associatedPADE.initializationFilePathname); TB4.myTextDisplay.text = sr.ReadToEnd(); TB4.myTextDisplay.ShowDialog(); } catch (Exception ex) { MessageBox.Show("There was a problem opening " + pade.associatedPADE.initializationFilePathname + ".  " + ex.Message); } }));
                            }
                        }

                        cm.MenuItems.Add("Delete " + rightClickCluster.name, new EventHandler(deleteCluster));
                        cm.MenuItems.Add("Rename " + rightClickCluster.name, new EventHandler(renameCluster));
                        if (padeCluster.clusterList.Count > 1) cm.MenuItems.Add("Move " + rightClickCluster.name, new EventHandler(moveCluster));

                    }
                }
                cm.MenuItems.Add("Add Cluster", new EventHandler(addCluster));

               

                if (buttonNode.highlightedList.Count > 1)
                {
                    cm.MenuItems.Add("Move " + buttonNode.highlightedList.Count.ToString() + " PADEs to another cluster", moveHighlightedPADEs);

                }
                cm.Show(this, mouseCoords);
            }
            if (e.Button == MouseButtons.Left)
            {
                //button node left click

                if (sender != this)
                {
                    mouseCoords = new Point(((buttonNode)sender).Location.X + e.Location.X, ((buttonNode)sender).Location.Y + e.Location.Y);
                    highlightListChanged.DynamicInvoke(new object[] { null, null });
                }

                if (nodeMoveFlag)
                {

                    nodeMoveFlag = false;
                    nodeMoveTimer.Enabled = false;
                    bool nodeFoundAHome = false;

                    foreach (buttonNode highlightedNode in buttonNode.highlightedList)
                    {
                        for (int j = 0; j < padeCluster.clusterList.Count; j++)
                        {
                            if (padeCluster.clusterList[j].padeList.Contains(highlightedNode))
                            { 
                                padeCluster.clusterList[j].padeList.Remove(highlightedNode); 
                            }

                            if (mouseCoords.X >= padeCluster.clusterList[j].Location.X && mouseCoords.X <= (padeCluster.clusterList[j].Location.X + padeCluster.clusterList[j].Size.Width)) 
                            { 
                                padeCluster.clusterList[j].padeList.Add(highlightedNode); nodeFoundAHome = true; 
                            }


                        }
                        if (mouseLoc.X > padeCluster.clusterList[padeCluster.clusterList.Count - 1].Location.X + padeCluster.clusterList[padeCluster.clusterList.Count - 1].Size.Width)
                        {
                            padeCluster.clusterList[padeCluster.clusterList.Count - 1].padeList.Add(highlightedNode);
                        }
                        if (mouseLoc.X < padeCluster.clusterList[0].Location.X)
                        {
                            padeCluster.clusterList[0].padeList.Add(highlightedNode);
                        }
                    }

                    initializeShapes();
                    this.Invalidate();

                    buttonNode.drawButtons();

                }

                if (clusterMoveFlag)
                {
                    clusterMoveFlag = false;
                    clusterMoveTimer.Enabled = false;
                    nodeMoveTimer.Enabled = false;
                    //switch the order of the clusters in the list to reflect the current location of the cluster
                    padeCluster.clusterList.Remove(rightClickCluster);

                    for (int i = 0; i < padeCluster.clusterList.Count; i++)
                    {
                        if (padeCluster.clusterList[i].Location.X > rightClickCluster.Location.X)
                        {
                            padeCluster.clusterList.Insert(i, rightClickCluster);
                            i = padeCluster.clusterList.Count + 1;
                        }
                        if (i == padeCluster.clusterList.Count - 1)
                        {
                            padeCluster.clusterList.Add(rightClickCluster);
                            i = padeCluster.clusterList.Count + 1;
                        }
                    }

                    initializeShapes();
                    this.Invalidate();
                    buttonNode.drawButtons();
                }
            }

            //TB4.thePADE_Selector.updateTreeview();
        }

        Timer nodeMoveTimer = new Timer();
        public void movePADENode(object sender, EventArgs e)
        {
            if (!buttonNode.highlightedList.Contains(rightClickPADE)) buttonNode.highlightedList.Add(rightClickPADE);

            for (int i = 0; i < buttonNode.highlightedList.Count; i++)
            {
                if (buttonNode.highlightedList[i] != rightClickPADE)
                {
                    buttonNode.highlightedList.Remove(buttonNode.highlightedList[i]);
                    i -= 1;
                }
            }
            rightClickPADE.offset = new Point(0, 0);
            nodeMoveTimer.Enabled = true;
            nodeMoveFlag = true;

        }
        public bool nodeMoveFlag = false;
        void nodeMoveTimer_Tick(object sender, EventArgs e)
        {
            if (nodeMoveFlag)
            {
                foreach (buttonNode node in buttonNode.highlightedList)
                {
                    node.Location = new Point(mouseLoc.X - node.offset.X, mouseLoc.Y - node.offset.Y);
                    //node.Invalidate();
                }
            }
            else if (clusterMoveFlag)
            {
                foreach (buttonNode node in rightClickCluster.padeList)
                {
                    node.Location = new Point(mouseLoc.X - node.offset.X, mouseLoc.Y - node.offset.Y);
                }
            }

            buttonNode.drawButtons();
        }


        public void moveHighlightedPADEs(object sender, EventArgs e)
        {
            foreach (buttonNode node in buttonNode.highlightedList)
            {
                node.offset = new Point(mouseLoc.X - node.Location.X, mouseLoc.Y - node.Location.Y);

            }

            nodeMoveFlag = true;
            nodeMoveTimer.Enabled = true;

        }
        public void renameCluster(object sender, EventArgs e)
        {
            string answer = Microsoft.VisualBasic.Interaction.InputBox("Please input a new name.");
            //padeCluster.clusterList.Remove(padeCluster.clusterList.First<padeCluster>(item => item.boxText == rightClickCluster.boxText));
            if (answer != null)
            {
                rightClickCluster.name = answer;
            }
            //padeCluster.clusterList.Add(rightClickCluster);
            this.graphicObj.Clear(this.BackColor);
            drawEnvironment();
        }
        public void removePADENode(object sender, EventArgs e)
        {
            for (int i = 0; i < padeCluster.clusterList.Count; i++)
            {
                if (padeCluster.clusterList[i].padeList.Contains(rightClickPADE))
                {
                    
                    this.Controls.Remove(buttonNode.nodeList.Find(item => item.associatedPADE.PADE_sn == rightClickPADE.associatedPADE.PADE_sn));
                    this.Controls.Remove(rightClickPADE);
                    padeCluster.clusterList[i].padeList.Remove(rightClickPADE);
                    buttonNode.nodeList.Remove(rightClickPADE);
                    rightClickPADE.hideBox(true);
                    rightClickPADE.Dispose();
                }
            }
            initializeShapes();
            this.graphicObj.Clear(this.BackColor);
            drawpadeClusters();

        }

        Timer clusterMoveTimer = new Timer();
        bool clusterMoveFlag = false;
        public void moveCluster(object sender, EventArgs e)
        {
            clusterMoveTimer.Tick += new EventHandler(boxMoveTimer_Tick);
            clusterMoveTimer.Interval = 10;
            foreach (buttonNode node in rightClickCluster.padeList)
            {
                node.offset = new Point(rightClickCluster.Location.X - node.Location.X, rightClickCluster.Location.Y - node.Location.Y);

            }


            clusterMoveFlag = true;
            nodeMoveTimer.Enabled = true;
            clusterMoveTimer.Enabled = true;


        }
        void deleteCluster(object sender, EventArgs e)
        {
            int surrogateClusterNumber = 999;
            for (int i = 0; i < padeCluster.clusterList.Count; i++) { if (padeCluster.clusterList[i] != rightClickCluster) surrogateClusterNumber = i; }
            buttonNode.highlightedList.Clear();
            foreach (buttonNode node in rightClickCluster.padeList)
            {
                buttonNode.highlightedList.Add(node);
                if (surrogateClusterNumber != 999) padeCluster.clusterList[surrogateClusterNumber].padeList.Add(node);
            }
            padeCluster.clusterList.Remove(rightClickCluster);

            initializeShapes();
            this.graphicObj.Clear(this.BackColor);
            drawpadeClusters();
        }

        void addCluster(object sender, EventArgs e)
        {
            string answer = Microsoft.VisualBasic.Interaction.InputBox("Please input a new name.");

            if (answer != null) { padeCluster cluster = new padeCluster(Color.Blue, answer, this); }

            initializeShapes();
            this.graphicObj.Clear(this.BackColor);
            drawpadeClusters();
        }

        void boxMoveTimer_Tick(object sender, EventArgs e)
        {
            rightClickCluster.Location = mouseLoc;
            this.Invalidate();
        }
        Stack<Point> mouseList = new Stack<Point>();

        #endregion

        #region Zoom

        double zoomConstant = 1.3; //number between 1 and...infinity.  Near 1 works best
        int zoomLevel = 0;


        public Point transformMouseCoord(Point originalCoord)
        {
            //Point adjustedMouseLoc = new Point(originalCoord.X - this.Location.X - canvasLocation.X, originalCoord.Y - this.Location.Y - canvasLocation.Y);
            Point adjustedMouseLoc = new Point(originalCoord.X - this.Location.X - this.Parent.Location.X, originalCoord.Y - this.Location.Y - this.Parent.Location.Y);
            if (adjustedMouseLoc.X < (this.Size.Width / (2 * zoomConstant))) adjustedMouseLoc.X = (int)(this.Size.Width / (2 * zoomConstant));
            else if (adjustedMouseLoc.X > (this.Size.Width * (1 - 1 / (2 * zoomConstant)))) adjustedMouseLoc.X = (int)(this.Size.Width * (1 - 1 / (2 * zoomConstant)));
            if (adjustedMouseLoc.Y < (this.Size.Height / (2 * zoomConstant))) adjustedMouseLoc.Y = (int)(this.Size.Height / (2 * zoomConstant));
            else if (adjustedMouseLoc.Y > (this.Size.Height * (1 - 1 / (2 * zoomConstant)))) adjustedMouseLoc.Y = (int)(this.Size.Height * (1 - 1 / (2 * zoomConstant)));
            return adjustedMouseLoc;
        }

        public void zoomOut(Point mouseLoc)
        {
            if (zoomLevel > 0)
            {
                //Point adjustedMouseLoc = transformMouseCoord(mouseLoc);
                Point adjustedMouseLoc = mouseLoc;
                Point lastPoint = mouseList.Pop();
                Point transformedPoint = new Point((int)(lastPoint.X + (adjustedMouseLoc.X - lastPoint.X) / zoomConstant), (int)(lastPoint.Y + (adjustedMouseLoc.Y - lastPoint.Y) / zoomConstant));
                foreach (buttonNode btn in buttonNode.nodeList)
                {
                    btn.Size = btn.sizeStack.Pop();
                    btn.Location = btn.locStack.Pop();

                }
                foreach (padeCluster box in padeCluster.clusterList)
                {
                    box.Size = box.sizeStack.Pop();
                    box.Location = box.locStack.Pop();
                }
                zoomLevel--;

                buttonNode.drawButtons();
                this.Invalidate();
            }
        }

        public void zoomIn(Point mouseLoc)
        {
            //create a new point which represents the point with respect to the enclosing canvas.
            //it also changes the point to prevent zoom-in's to offscreen locations
            //Point adjustedMouseLoc = transformMouseCoord(mouseLoc);
            Point adjustedMouseLoc = mouseLoc;
            mouseList.Push(adjustedMouseLoc);
            //the point Q corresponds to the corner of the zoom window such that the sum of the coordinates is the lowest.  In computer
            //graphics, its the upper-left corner.
            Point Q = new Point((int)(adjustedMouseLoc.X - this.Size.Width / (2 * zoomConstant)), (int)(adjustedMouseLoc.Y - this.Size.Height / (2 * zoomConstant)));

            foreach (buttonNode btn in buttonNode.nodeList)
            {

                btn.locStack.Push(btn.Location);
                btn.sizeStack.Push(btn.Size);
                //btn.Location = new Point((int) (zoomConstant * (btn.Location.X - Q.X)), (int) (zoomConstant * (btn.Location.Y - Q.Y)));
                btn.Location = new Point((int)(zoomConstant * (btn.Location.X - Q.X)), (int)(zoomConstant * (btn.Location.Y - Q.Y)));
                btn.Size = new Size((int)(btn.Size.Width * zoomConstant), (int)(btn.Size.Height * zoomConstant));


                btn.Invalidate(); //force the control to redraw itself
                //btn.Update();
                if (zoomLevel < 4 && mouseLoc.X > (btn.locStack.Peek().X) && mouseLoc.X < (btn.locStack.Peek().X + btn.sizeStack.Peek().Width + 1) && mouseLoc.Y > (btn.locStack.Peek().Y) && mouseLoc.Y < (btn.locStack.Peek().Y + btn.sizeStack.Peek().Height) && btn.Location.X > 0 && btn.Location.Y > 0)
                {
                    //move the cursor the same amount as the button is being moved
                    //Cursor.Position = new Point(this.Parent.Location.X + mouseLoc.X + (btn.Location.X - btn.locStack.Peek().X)+(int) btn.Size.Width/2, this.Parent.Location.Y + mouseLoc.Y + (btn.Location.Y - btn.locStack.Peek().Y)+(int) btn.Size.Height/2);
                    Cursor.Position = new Point((btn.Location.X) + this.PointToScreen(Point.Empty).X + btn.Size.Width / 2, this.PointToScreen(Point.Empty).Y + btn.Location.Y + btn.Size.Height / 2);

                }
            }
            foreach (padeCluster box in padeCluster.clusterList)
            {
                box.locStack.Push(box.Location);
                box.sizeStack.Push(box.Size);
                box.setLocation(new Point((int)(zoomConstant * (box.Location.X - Q.X)), (int)(zoomConstant * (box.Location.Y - Q.Y))));
                box.setSize(new Size((int)(box.Size.Width * zoomConstant), (int)(box.Size.Height * zoomConstant)));
                drawpadeClusters();
            }

            //Cursor.Position = new Point((int)((canvasSize.Width/2+Q.X)), (int)(( canvasSize.Height/2+Q.Y)));
            //Cursor.Position = new Point((int)(zoomConstant * (mouseLoc.X - Q.X)), (int)(zoomConstant * (mouseLoc.Y-Q.Y)));
            zoomLevel++;
            this.Invalidate(); //force the parent container to change the location of all the buttonNodes

        }
        
        #endregion

        #region Painting

        public void drawpadeClusters()
        {
            if(padeCluster.clusterList.Count<1) this.graphicObj.Clear(this.BackColor);
            foreach (padeCluster cluster in padeCluster.clusterList)
            {
                cluster.drawRectangle();
            }

        }

        public void drawEnvironment()
        {
            this.initializeShapes();
            this.drawpadeClusters();
            TB4.thePADE_Selector.updateTreeview();
            buttonNode.drawButtons();
            this.Invalidate();
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            if (initialized)
            {
                drawpadeClusters();
                //buttonNode.drawButtons();
            }
            for (int i = 0; i < textOverlayList.Count; i++)
            {
                this.graphicObj.DrawString
                    (
                        textOverlayList[i].text, 
                        new Font("Courier New", (float)10.0, FontStyle.Regular), 
                        new SolidBrush(textOverlayList[i].textColor), 
                        new PointF
                            (
                            this.Width - this.graphicObj.MeasureString(textOverlayList[i].text,
                            new Font("Courier New", (float)10.0, FontStyle.Regular)).Width - 10,
                            5 + i * this.graphicObj.MeasureString(textOverlayList[i].text, 
                            new Font("Courier New", (float)10.0, FontStyle.Regular)).Height
                            )
                       );
            }
            base.OnPaint(e);
            
        }
        #endregion

        #region Events
        public  delegate void highlightChanged(object sender, EventArgs e);
        public  event highlightChanged highlightListChanged;

        #endregion

    }

    public class padeCluster
    {
        public static ObservableCollection<padeCluster> clusterList = new ObservableCollection<padeCluster>();
        public ObservableCollection<buttonNode> padeList = new ObservableCollection<buttonNode>();
        public Stack<Point> locStack = new Stack<Point>();
        public Stack<Size> sizeStack = new Stack<Size>();



        public string name = "";

        public BoxSee Parent;
        public Color borderColor;
        public string boxText;
        public Graphics parentGraphic;
        public Point Location;
        public Point lastLocation;

        public Size lastSize;
        public Size Size;

        public static void setSystemChangeEventHandler(Delegate clusterChange, Delegate padeChange)
        {

            clusterList.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(delegate(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
            {
                clusterChange.DynamicInvoke(sender, e);
            });
            foreach (padeCluster cluster in clusterList)
            {
                cluster.padeList.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(delegate(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
                {
                    padeChange.DynamicInvoke(sender, e);
                });
            }
        }
        public padeCluster(Color new_color, string _name, BoxSee _parent)
        {

            borderColor = new_color;
            parentGraphic = _parent.graphicObj;
            name = _name;
            Parent = _parent;
            clusterList.Add(this);
        }
        public void setSize(Size newSize)
        {
            lastSize = Size;
            Size = newSize;
        }
        public void setLocation(Point newLocation)
        {
            lastLocation = Location;
            Location = newLocation;
        }

        public void drawRectangle()
        {
            Pen erasePen = new Pen(new SolidBrush(SystemColors.Control));
            if (lastLocation != new Point(0, 0))
            {
                parentGraphic.DrawLine(erasePen, this.lastLocation.X, this.lastLocation.Y, this.lastLocation.X + this.lastSize.Width, this.lastLocation.Y);
                parentGraphic.DrawLine(erasePen, this.lastLocation.X, this.lastLocation.Y, this.lastLocation.X, this.lastLocation.Y + this.lastSize.Height);
                parentGraphic.DrawLine(erasePen, this.lastLocation.X, this.lastLocation.Y + this.lastSize.Height, this.lastLocation.X + this.lastSize.Width, this.Location.Y + this.lastSize.Height);
                parentGraphic.DrawLine(erasePen, this.lastLocation.X + this.lastSize.Width, this.lastLocation.Y, this.lastLocation.X + this.lastSize.Width, this.lastLocation.Y + this.lastSize.Height);
            }
            SolidBrush brush = new SolidBrush(Color.Blue);
            Pen currentPen = new Pen(brush);
            parentGraphic.DrawLine(currentPen, this.Location.X, this.Location.Y, this.Location.X + this.Size.Width, this.Location.Y);
            parentGraphic.DrawLine(currentPen, this.Location.X, this.Location.Y, this.Location.X, this.Location.Y + this.Size.Height);
            parentGraphic.DrawLine(currentPen, this.Location.X, this.Location.Y + this.Size.Height, this.Location.X + this.Size.Width, this.Location.Y + this.Size.Height);
            parentGraphic.DrawLine(currentPen, this.Location.X + this.Size.Width, this.Location.Y, this.Location.X + this.Size.Width, this.Location.Y + this.Size.Height);

            int fontSize;
            if (this.Size.Width > 100)
            {
                fontSize = 12;
            }
            else
            {
                fontSize = 10;
            }

            System.Drawing.Font drawFont = new System.Drawing.Font("Arial", fontSize);

            System.Drawing.SolidBrush drawBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Blue);
            parentGraphic.DrawString(name, drawFont, drawBrush, this.Location.X - 5, this.Location.Y - 24);

            drawFont.Dispose();
            erasePen.Dispose();
            currentPen.Dispose();
        }

        public void addNode(Color fillcolor, PADE newPADE)
        {
            this.padeList.Add(new buttonNode(fillcolor, Parent, newPADE));
            TB4.activatePADE(newPADE, false, true);
            
        }
        public void removeNode(PADE newPADE)
        {
            foreach (buttonNode node in this.padeList)
            {
                if (node.associatedPADE.PADE_sn == newPADE.PADE_sn)
                {
                    this.padeList.Remove(node);
                    //buttonNode.nodeList.Remove(node);
                    return;
                }
            }
        }

    }

    public class buttonNode : Control
    {
        //this will be used for keeping track of which nodes are highlighted
        public static List<buttonNode> nodeList = new List<buttonNode>();
        public static ObservableCollection<buttonNode> highlightedList = new ObservableCollection<buttonNode>();
        public static List<buttonNode> notBiasedList = new List<buttonNode>();
        public static List<buttonNode> notInitializedList = new List<buttonNode>();

        #region Properties
        bool isHighlighted = false;

        public Point offset = new Point(0, 0); //used for drag and dropping to maintain relative spacing;
        public Color fillColor;
        public Graphics graphicObj;
        public Stack<Point> locStack = new Stack<Point>();
        public Stack<Size> sizeStack = new Stack<Size>();
        public string simpleText = "N/A";
        public string detailedText = "No details available.";
        ToolTip toolTip;
        public PADE associatedPADE;
        BoxSee Parent;


        public static string registerToDisplay = ""; //This will be displayed alongside the PADE number in the buttonnode


        
        static System.Drawing.Font drawFont = new System.Drawing.Font("Courier New", 12);
        #endregion

        public buttonNode(Color newFillColor, BoxSee sender, PADE newPADE)
        {
            fillColor = newFillColor;

            //subscribe event handlers
            this.MouseClick += new MouseEventHandler(buttonNode_MouseClick);

            this.Parent = sender;
            sender.Controls.Add(this);
            this.Bounds = new Rectangle(Location.X, Location.Y, Size.Width - 1, Size.Height - 1);
            this.BackColor = Color.Blue;

            toolTip = new ToolTip();
            toolTip.SetToolTip(this, detailedText);
            toolTip.Active = false;
            toolTip.InitialDelay = 1500;

            associatedPADE = newPADE;

            graphicObj = this.CreateGraphics();

            nodeList.Add(this);
            notBiasedList.Add(this);
            notInitializedList.Add(this);
            
            this.MouseMove += new MouseEventHandler(buttonNode_MouseMove);
            this.Invalidate();
            detailedText = associatedPADE.biasFilePathname;

        }
        
        void buttonNode_MouseMove(object sender, MouseEventArgs e)
        {
            Parent.mouseLoc = new Point(e.Location.X + this.Location.X, e.Location.Y + this.Location.Y);
            Console.WriteLine(Parent.mouseLoc.X + "    " + Parent.mouseLoc.Y);
        }

        public void fillBox()
        {
            graphicObj.FillRectangle(new SolidBrush(fillColor), 0, 0, Size.Width, Size.Height);

        }
        public static void disposeButtonNode(PADE pade)
        {
            buttonNode node=null;
            for (int i = 0; i < buttonNode.nodeList.Count; i++)
            {
                if (nodeList[i].associatedPADE.PADE_sn == pade.PADE_sn)
                {
                    node = nodeList[i];
                    break;
                }
            }
            if (node != null)
            {
                nodeList.Remove(node);
                highlightedList.Remove(node);
                notBiasedList.Remove(node);
                notInitializedList.Remove(node);
                node.hideBox(true);
                node.Dispose();
            }
            else
            {
                //MessageBox.Show("Could not find PADE");
                int i = 1;
            }
        }
        public void disposeButtonNode()
        {
            nodeList.Remove(this);
            highlightedList.Remove(this);
            notBiasedList.Remove(this);
            notInitializedList.Remove(this);
            this.hideBox(true);
            this.Dispose();

        }
        #region Events

        protected override void OnPaint(PaintEventArgs e)
        {
            //drawButtons();
            drawButton();
            base.OnPaint(e);
            //drawButtons();

        }
        void buttonNode_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (!this.Parent.nodeMoveFlag)
                {
                    doHighlight();
                    this.Parent.Focus();
                }
                TB4.activatePADE(this.associatedPADE, true, false);

            }

            //this is so we can register click events in the parent form...it comes in handy
            this.Parent.BoxSee_MouseDown(sender, e);
        }

        public void doHighlight()
        {
            if (Control.ModifierKeys == Keys.Control)
            {


            }
            else
            {
                highlightedList.Clear();

            }

            if (highlightedList.Contains(this))
            {
                highlightedList.Remove(this);
                this.fillColor = Color.Blue;
                this.isHighlighted = false;
            }
            else
            {
                highlightedList.Add(this);
                this.isHighlighted = true;
            }

            drawButtons();
            this.Invalidate();
        }

        public void hideBox(bool isTrue)
        {
            if (isTrue) this.Visible = false;
            else this.Visible = true;
        }

        static public void drawButtons()
        {
            Color drawColor = Color.White;
            int thickness = 5;
            foreach (buttonNode node in nodeList)
            {
                //node.graphicObj.Dispose();
                //node.graphicObj = node.CreateGraphics();
                node.Invalidate();
            }
        }

        public void drawButton()
        {
            Color drawColor = Color.White;
            this.graphicObj.Dispose();
            this.graphicObj = CreateGraphics();
            //this.graphicObj.Clear(this.fillColor);

            if (associatedPADE.PADE_is_MASTER) this.fillColor = Color.CadetBlue;

            if (highlightedList.Contains(this))
            {
                this.graphicObj.FillRectangle(new SolidBrush(Color.Yellow), 0, 0, this.Size.Width, this.Size.Height);
                drawColor = Color.Blue;

            }
            else
            {
                this.graphicObj.FillRectangle(new SolidBrush(this.fillColor), 0, 0, this.Size.Width, this.Size.Height);
                drawColor = Color.White;

            }

            if (Size.Width > 5 && Size.Width <= 30)
            {
                graphicObj.DrawString(associatedPADE.PADE_sn, new System.Drawing.Font("Courier New", 6), new SolidBrush(drawColor), 0, 0);
            }
            else if (Size.Width > 30 && Size.Width < 225)
            {
                //determine what simple text should be here
                if (buttonNode.registerToDisplay == "") simpleText = "N/A";
                else
                {
                    string oldActive = TB4.activeIndex;
                    TB4_Register tempReg = PADE_explorer.registerLookup(registerToDisplay);
                    TB4.activatePADE(PADE_explorer.getPADE(this.associatedPADE.PADE_sn));
                    simpleText = tempReg.RegRead().ToString();
                    TB4.activatePADE(PADE_explorer.getPADE(oldActive));
                }

                graphicObj.DrawString(associatedPADE.PADE_sn, drawFont, new SolidBrush(drawColor), 0, 0);
                graphicObj.DrawString(simpleText, drawFont, new SolidBrush(drawColor), 0, Height / 2);
            }
            else if (Size.Width >= 225)
            {
                drawFont = new System.Drawing.Font("Courier New", 8);
                graphicObj.DrawString(associatedPADE.biasFilePathname + '\n' + "Is Master: " + associatedPADE.PADE_is_MASTER, drawFont, new SolidBrush(drawColor), 0, 0);
                graphicObj.DrawString("SIB ID: " + associatedPADE.SIB_ID , drawFont, new SolidBrush(drawColor), 0, Height / 2);
            }
            if (Size.Width > 30)
            {
                graphicObj.DrawLine(new Pen(drawColor), new Point(0, Height / 2), new Point(Width, Height / 2));
                toolTip.Active = true;
            }
        }



        #endregion

    }


}

