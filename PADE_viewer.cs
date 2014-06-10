using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PADE
{
    public partial class PADE_viewer : Form
    {

        public PADE_viewer()
        {
            InitializeComponent();
        }

        private void PADE_viewer_Load(object sender, EventArgs e)
        {

        }

        private void parseSystemFile(string filePathname)
        {
            if(System.IO.File.Exists(filePathname))
            {
                System.IO.StreamReader sreader=new System.IO.StreamReader(filePathname);

                string allText=sreader.ReadToEnd();

                string[] splitText=allText.Split(new char[] {'$'});
                string[] DH, DH2;

                for (ushort i = 1; i <= splitText.Length; i++)
                {
                    
                    DH=splitText[i].Split(new char[]{'\n'}, StringSplitOptions.RemoveEmptyEntries);

                    DH2 = DH[0].Split(new char[] { ' ' });

                    PADE currentPADE = PADE_explorer.getPADE(DH2[1]);

                    DH2=DH[1].Split(new char[]{'='});
                    currentPADE.firmwareVersion = DH2[1];
                    DH2 = DH[2].Split(new char[] { '=' });
                    currentPADE.initializationFilePathname = DH2[1];
                    DH2 = DH[3].Split(new char[] { '=' });
                    currentPADE.biasFilePathname = DH2[1];
                    DH2 = DH[4].Split(new char[] { '=' });
                    DH2 = DH2[1].Split(new char[] { ',' });

                    for (ushort a = 0; a < 32; a++) if (DH2.Contains(a.ToString())) currentPADE.PADE_ch_enable[i] = true;

                    DH2 = DH[5].Split(new char[] { '=' });
                    currentPADE.matchingSiPM = DH2[1];
                    
                }

            }
        }


        



    }
}
