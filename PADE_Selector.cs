using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using System.Reflection;
using System.Collections.ObjectModel;

namespace PADE
{
    public partial class PADE_Selector : DockContent
    {
        public PADE_Selector()
        {
            InitializeComponent();
        }

        private void PADE_Selector_Load(object sender, EventArgs e)
        {
            TB4.thePADE_Selector = this;
            padeCluster.setSystemChangeEventHandler(new System.Collections.Specialized.NotifyCollectionChangedEventHandler(clusterChanged),
                new System.Collections.Specialized.NotifyCollectionChangedEventHandler(padeChanged));

            TB4.theSystemViewer.boxSee1.highlightListChanged += (highlightChanged);
        }

        public void highlightChanged(object sender, EventArgs e)
        {
            syncListWithBoxSee(null, null);
        }
        public void clusterChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            ObservableCollection<padeCluster> clusterList = (ObservableCollection<padeCluster>)sender;
            TB4.theSystemViewer.boxSee1.drawpadeClusters();
            updateTreeview();
            if (!TB4.theSystemViewer.boxSee1.controlsSystemFile) TB4.theSystemViewer.boxSee1.recordState();
        }

        public void padeChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            ObservableCollection<buttonNode> padeList = (ObservableCollection<buttonNode>)sender;
            //TB4.theSystemViewer.boxSee1.drawpadeClusters();
            updateTreeview();
            if(!TB4.theSystemViewer.boxSee1.controlsSystemFile) TB4.theSystemViewer.boxSee1.recordState();
        }

        /// <summary>
        /// This method uses the PADE's enclosed in the padeCluster class 
        /// </summary>
        public void updateTreeview()
        {
            treeView1.Nodes.Clear();
            foreach (padeCluster cluster in padeCluster.clusterList)
            {
                clusterTreeNode clusterNode = new clusterTreeNode(cluster);

                foreach (buttonNode pade in cluster.padeList)
                {
                    padeTreeNode pNode = new padeTreeNode(pade);
                    clusterNode.Nodes.Add(pNode);
                }
                treeView1.Nodes.Add(clusterNode);
            }
        }

        void treeView1_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
        {
        }

        void treeView1_Click(object sender, System.Windows.Forms.TreeNodeMouseClickEventArgs args)
        {
            if (args.Button == MouseButtons.Left)
            {
                if (args.Node.Nodes.Count == 0)
                {
                    //this is a padetreenode
                    string[] DH = args.Node.Text.Split(new char[] { ' ' });
                    if (DH.Length > 1)
                    {
                        string padeNum = DH[1];

                        TB4.activatePADE(PADE_explorer.getPADE(DH[1]), true, false);
                        foreach (buttonNode pade in buttonNode.nodeList)
                        {
                            if (pade.associatedPADE.PADE_sn == padeNum)
                            {
                                if (selectedPADEList.Nodes.Contains(new TreeNode("PADE " + pade.associatedPADE.PADE_sn))) buttonNode.highlightedList.Remove(pade);
                                else
                                {
                                    //select the PADE and remove all other PADES from selectedPADEList
                                    selectedPADEList.Nodes.Clear();
                                    object nodeCopy = args.Node.Clone();
                                    padeTreeNode nodeCopy2 = (padeTreeNode)nodeCopy;
                                    selectedPADEList.Nodes.Add(nodeCopy2);
                                    syncListWithBoxSee(this, null);
                                }
                            }
                        }

                    }
                }

            }
            else
            {
                if (args.Node.Parent == null) //add or remove an entire cluster
                {
                    foreach (padeTreeNode node in args.Node.Nodes)
                    {
                        bool nodeAlreadyAdded=false;
                        for (int i = 0; i < selectedPADEList.Nodes.Count; i++)
                        {
                            if (selectedPADEList.Nodes[i].Text == node.Text)
                            {
                                nodeAlreadyAdded = true;
                                selectedPADEList.Nodes[i].Remove();
                            }
                        }

                        if (nodeAlreadyAdded==false)
                        {
                            object nodeCopy = node.Clone();
                            padeTreeNode nodeCopy2 = (padeTreeNode)nodeCopy;
                            selectedPADEList.Nodes.Add(nodeCopy2);
                            
                        }
                        
                    }
                }
                else
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
                        string[] DH = args.Node.Text.Split(new char[] { ' ' });
                        int SN = Convert.ToInt16(DH[1]);
                        for (i = 0; i < selectedPADEList.Nodes.Count && (SN >= Convert.ToInt16(selectedPADEList.Nodes[i].Tag)); i++) { };

                        object nodeCopy = args.Node.Clone();
                        //treeView1.Nodes.Remove(selectedNode);
                        padeTreeNode nodeCopy2 = (padeTreeNode)nodeCopy;
                        selectedPADEList.Nodes.Add(nodeCopy2);
                       
                        //selectedPADEList.Nodes[i].Tag = args.Node.Nodes["PADE_sn"].Text;
                    }
                    
                }
                syncListWithBoxSee(this, null);
                
            }

        }

        void selectedPADEList_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
        {
        }

        void selectedPADEList_Click(object sender, System.Windows.Forms.TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                e.Node.Remove();
                groupBox3.Text = "Selected PADE (" + selectedPADEList.Nodes.Count + ")";
            }
            syncListWithBoxSee(this, null);

        }

        public void addPADENode(PADE item)
        {

            //create parent (the PADE unit)
            TreeNode parent;


            //create an array with every field in the class (IP, MAC address, etc.)
            FieldInfo[] fields = item.GetType().GetFields();

            //create a list in which to contain each field that is part of a given PADE object
            //List used instead of an array because its easier to add members
            System.Collections.Generic.List<TreeNode> nodeList = new System.Collections.Generic.List<TreeNode>();

            for (int i = 0; i < TB4.formList.Count; i++)
            {
                nodeList.Add(new TreeNode(TB4.formList[i].Name.ToString()));
            }

            TreeNode[] children = nodeList.ToArray();

            parent = new TreeNode("PADE " + item.PADE_sn, children);
            parent.Tag = item.PADE_sn;

            TB4.thePADE_Selector.treeView1.Nodes.Add(parent);


        }

        public void rescaleControls(int formwidth)
        {
            groupBox2.Width = formwidth - groupBox2.Location.X - 2;
            groupBox3.Width = formwidth - groupBox3.Location.X - 2;
            treeView1.Width = formwidth - treeView1.Location.X - 12;
            selectedPADEList.Width = formwidth - selectedPADEList.Location.X - 12;

        }

        void PADE_Selector_DockStateChanged(object sender, System.EventArgs e)
        {
            if (this.DockState == DockState.Float)
            {
                rescaleControls(245);
                this.FloatPane.FloatWindow.Size = new System.Drawing.Size(266, 427);
            }
            else
            {

                this.DockState = DockState.Document;
                //this.Size = new System.Drawing.Size(131, 427);
                rescaleControls(150);
            }

        }
        int counter = 0;
        void PADE_Selector_FormClosing(object sender, FormClosingEventArgs e)
        {
            // e.Cancel = true;
            //TB4.thePADE_explorer.updateStatusText(true, "Error: cannot close the PADE form.");
        }
        void syncListWithBoxSee(object sender, ControlEventArgs e)
        {
            if (sender == this)
            {
                //update the boxsee to reflect changes in the pade selector
                buttonNode.highlightedList.Clear();
                for (int i = 0; i < selectedPADEList.Nodes.Count; i++)
                {
                    buttonNode.highlightedList.Add(((padeTreeNode)selectedPADEList.Nodes[i])._associatedPADE);
                }
                buttonNode.drawButtons();
            }
            else
            {
                //update the pade selector to reflect changes in the boxsee
                selectedPADEList.Nodes.Clear();
                counter++;
                Console.WriteLine(counter);
                List<buttonNode> orderedHighlightList = new List<buttonNode>();
                ObservableCollection<buttonNode> highlightedCopy=buttonNode.highlightedList;

                int lastSN = 0;
                buttonNode currentNode=null;
                for(int i=0; i<buttonNode.highlightedList.Count; i++)
                {
                    int minSN=999;
                    for (int j = 0; j < buttonNode.highlightedList.Count; j++)
                    {
                        if (Convert.ToInt16(buttonNode.highlightedList[j].associatedPADE.PADE_sn) < minSN &&Convert.ToInt16(buttonNode.highlightedList[j].associatedPADE.PADE_sn)>lastSN)
                        {
                            minSN = Convert.ToInt16(buttonNode.highlightedList[j].associatedPADE.PADE_sn);
                            currentNode=buttonNode.highlightedList[j];
                        }
                    }
                    lastSN = minSN;
                    orderedHighlightList.Add(currentNode);
                }
                foreach (buttonNode node in orderedHighlightList)
                {
                    selectedPADEList.Nodes.Add(new padeTreeNode(node));
                }

            }
            groupBox3.Text = "Selected PADE (" + selectedPADEList.Nodes.Count + ")";
            syncWithOldPADEList();
        }

        /// <summary>
        /// This exists so that we can interface the old way of doing things (TB4.PADE_List) with the new way (padeClusters)
        /// </summary>
        void syncWithOldPADEList()
        {
            TB4.PADE_List.Clear();
            for (int i = 1; i <= buttonNode.nodeList.Count; i++)
            {
                TB4.PADE_List[i] = buttonNode.nodeList[i - 1].associatedPADE;
            }
        }


    }

    public class clusterTreeNode : TreeNode
    {
        public padeCluster _associatedCluster;

        public clusterTreeNode(padeCluster _newCluster)
            : base()
        {
            _associatedCluster = _newCluster;
            this.Text = _associatedCluster.name;

        }

    }

    public class padeTreeNode : TreeNode
    {
        public buttonNode _associatedPADE;
        public padeTreeNode()
            : base()
        {
            _associatedPADE = null;
        }

        public padeTreeNode(buttonNode _newNode)
            : base()
        {
            
            _associatedPADE = _newNode;
            this.Text = "PADE " + _associatedPADE.associatedPADE.PADE_sn;
            this.Tag = _associatedPADE.associatedPADE.PADE_sn;
        }
        public override object Clone()
        {
            padeTreeNode returnObj = (padeTreeNode)base.Clone();
            returnObj._associatedPADE = _associatedPADE;
            return (object)returnObj;
        }


    }
}
