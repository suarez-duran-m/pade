using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;

namespace TB_namespace
{
    public partial class UDP_Listner : Form
    {
        static int listenPort = 0x5311;
        UdpClient listener = new UdpClient(listenPort);
        IPEndPoint groupEP = new IPEndPoint(IPAddress.Any, listenPort);
        public string received_data;

        public UDP_Listner()
        {
            InitializeComponent();
            btn_LISTEN.Text = "Listen";
        }

        private void btn_LISTEN_Click(object sender, EventArgs e)
        {
            if (btn_LISTEN.Text == "Listen")
            {
                btn_LISTEN.Text = "Stop";
                try
                {
                    listenPort = Convert.ToUInt16(tbx_Port.Text, 16);
                }
                catch
                {
                    listenPort = 0x5311;
                }
                this.Worker_UDPreceive.RunWorkerAsync();
                textBox.Text = received_data;
            }
            else
            {
                btn_LISTEN.Text = "Listen";
                this.Worker_UDPreceive.CancelAsync();
            }
        }



        private void Worker_UDPreceive_DoWork(object sender, DoWorkEventArgs e)
        {

            byte[] receive_byte_array;
            UInt16 data_len = 90 * 8;
            UInt16 j;
            try
            {
                while (!this.Worker_UDPreceive.CancellationPending)
                {
                    received_data = listener.Available.ToString() + " ";
                    receive_byte_array = listener.Receive(ref groupEP);
                    received_data = receive_byte_array.Length.ToString();
                    for (j = 0; j < data_len; j++)
                    {
                        TB4.myRun.event_data[j] = receive_byte_array[j * 2 + 2];
                        TB4.myRun.event_data[j] = 256 * TB4.myRun.event_data[j] + receive_byte_array[j * 2 + 1 + 2];
                        //if (j > 2 ) { rdata[j] = rdata[j] / 2 + rdata[j - 1] / 4 + rdata[j - 2] / 4; }
                    }
                    TB4.myRun.Run_event++;
                    Worker_UDPreceive.ReportProgress(50);
                }

            }
            catch
            {
                //textBox.Text = "Error,closing listner";
                //listener.Close();
            }
        }
        private void Worker_UDPreceive_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            textBox.Text = TB4.myRun.Run_event.ToString() + " " + received_data;
        }

        private void Worker_UDPreceive_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //listener.Close();

            received_data = "Listner stopped";
        }
    }
}
