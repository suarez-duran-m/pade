using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace PADE
{
    class graphicUtilities
    {
        public class buttonNode : Control
        {
            //this will be used for keeping track of which nodes are highlighted
            public static List<buttonNode> nodeList = new List<buttonNode>();
            public static List<buttonNode> highlightedList=new List<buttonNode>();

            #region Properties
            public int number;
            public Size boxSize;
            public Color fillColor;
            public Point location;
            public string text;
            Graphics graphicObj;

            #endregion

            public buttonNode(Point newLocation, Size newSize, Color newFillColor, Form sender)
            {
                location = newLocation;
                boxSize = newSize;
                fillColor = newFillColor;

                //subscribe event handlers
                this.MouseClick += new MouseEventHandler(buttonNode_MouseClick);

                this.Parent = sender;
                sender.Controls.Add(this);
                this.Bounds = new Rectangle(location.X, location.Y, boxSize.Width-1, boxSize.Height-1);

                graphicObj = this.CreateGraphics();
                nodeList.Add(this);
                this.Invalidate();
            }

            

            public void fillBox()
            {
                graphicObj.FillRectangle(new SolidBrush(fillColor), 0, 0, boxSize.Width, boxSize.Height);
                
            }
            #region Events

            protected override void OnPaint(PaintEventArgs e)
            {

                //this.Invalidate();
                base.OnPaint(e);
            }

            void buttonNode_MouseClick(object sender, MouseEventArgs e)
            {
                Console.WriteLine("CLICK");
                if (Control.ModifierKeys == Keys.Control)
                {
                    //add this to the list of selected buttons
                    
                }
                else
                {
                    highlightedList.Clear();
                }

                highlightedList.Add(this);
                highlightButtons();
            }

            static public void highlightButtons()
            {
                foreach (buttonNode node in nodeList)
                {
                    if (highlightedList.Contains(node)) node.graphicObj.DrawRectangle(new Pen(Color.Yellow, 5), 1,1, node.Width - 5, node.Height -5);
                    else node.graphicObj.DrawRectangle(new Pen(node.fillColor, 5), 1, 1, node.Width - 5, node.Height - 5);
                }
            }

           

            #endregion

        }




    }
}
