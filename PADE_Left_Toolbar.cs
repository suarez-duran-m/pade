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

namespace PADE
{
    public partial class PADE_Left_Toolbar : DockContent
    {
        public PADE_Left_Toolbar()
        {
            InitializeComponent();
        }

        private void PADE_Left_Toolbar_Load(object sender, EventArgs e)
        {

        }
        #region Treeview Controls
        private void populateTreeview()
        {
            PADE item;

            if (TB4.PADE_List != null)
            {
                for (int i = 1; i <= TB4.PADE_List.Count; i++)
                {
                    TB4.PADE_List.TryGetValue(i, out item);


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

                    parent = new TreeNode(item.PADE_descr, children);

                    treeView1.Nodes.Add(parent);
                }
            }
        }

        private void treeView1_NodeClicked(object sender, System.Windows.Forms.TreeNodeMouseClickEventArgs args)
        {
            if (args.Button == MouseButtons.Left)
            {
                TreeNode tempNode = args.Node;

                while (tempNode.Parent != null) { tempNode = tempNode.Parent; } //retrieve top-level parent 

                

                if (args.Node.Text == "Temperature")  //if the user clicks the temperature node, open up the temperature plot
                {
                    PADE_explorer.activatePADE(tempNode.Nodes["PADE_sn"].Nodes[0].Text);

                   // openForms.Add(new formPair(TB4.theTempPlot.GetType(), TB4.theTempPlot));
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
    }
}
