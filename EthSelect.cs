using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PcapDotNet.Core;
using PcapDotNet.Packets;
using PcapDotNet.Packets.Ethernet;
using PcapDotNet.Packets.IpV4;
using PcapDotNet.Packets.Icmp;
using PcapDotNet.Packets.Transport;

namespace PADE
{
    public partial class EthSelect : Form
    {
        public LivePacketDevice thisInterface;
        public IList<LivePacketDevice> interfaceList = new List<LivePacketDevice>(4);

        public EthSelect()
        {
            InitializeComponent();
        }

        public void EthSelect_Load(object sender, EventArgs e)
        {
            TB4.ETH_OK = false;
            IList<LivePacketDevice> allDevices = LivePacketDevice.AllLocalMachine;
            

            if (allDevices.Count == 0)
            {
                MessageBox.Show("No interfaces found! Make sure WinPcap is installed.");
                return;
            }

            this.comboBox1.Items.Clear();

            for (int i = 0; i != allDevices.Count; i++)
            {
               
                LivePacketDevice device = allDevices[i];

                if (device.Description == null)
                {
                    Console.WriteLine(" (No description available)");
                }
                else
                {
                    interfaceList.Add(device);
                    string t = device.Description;
                    this.comboBox1.Items.Add(t);
                    if ( (t.ToLower().Contains("gigabit")) || (t.ToLower().Contains("gbe")) )
                    {
                        this.comboBox1.SelectedIndex = i;
                    }
                }
            }
        }

        private void btnActivate_Click(object sender, EventArgs e)
        {
            try
            {
                thisInterface = interfaceList[comboBox1.SelectedIndex];
                if (thisInterface.Description.Length > 0)
                {
                    TB4.ETH_INTERFACE = thisInterface;
                    TB4.ETH_OK = true;
                    TB4.SlowControl_interface = thisInterface;

                    this.Close();
                    Eth_comms.Eth_Open();
                }
            }
            catch
            { TB4.ETH_OK = false; TB4.ETH_INTERFACE = null; }
        }
    }
}
