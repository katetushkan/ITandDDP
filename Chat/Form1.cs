using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chat
{
    public partial class fMain : Form
    {

        private Point mouseOffset;
        private bool isMouseDown = false;

        bool alive = false;
        UdpClient client;
        int LOCALPORT; 
        int REMOTEPORT; 
        const int TTL = 20;
        const string HOST = "235.5.5.1"; 
        IPAddress groupAddress; 

        string userName; 

        public fMain()
        {
            InitializeComponent();
            btnLogin.Enabled = true; 
            btnLogout.Enabled = false; 
            btnSend.Enabled = false; 
            groupAddress = IPAddress.Parse(HOST);
        }

        private void loginButton_Click(object sender, EventArgs e)
        {
            
            pOptionsMenu.Visible = false;
            LOCALPORT = Convert.ToInt32(tbxLocalPort.Text);
            REMOTEPORT = Convert.ToInt32(tbxRemotePort.Text);
            userName = tbxUsername.Text;
            tbxUsername.ReadOnly = true;
            try
            {
                client = new UdpClient(LOCALPORT);
                
                client.JoinMulticastGroup(groupAddress, TTL);

                
                Task receiveTask = new Task(ReceiveMessages);
                receiveTask.Start();

                string message = userName + " joined chat";
                byte[] data = Encoding.Unicode.GetBytes(message);
                client.Send(data, data.Length, HOST, REMOTEPORT);

                btnLogin.Enabled = false;
                btnLogout.Enabled = true;

                tbxMessage.Enabled = true;
                btnSend.Enabled = true;
                lblStatus.Text = "Online";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        
        private void ReceiveMessages()
        {
            alive = true;
            try
            {
                while (alive)
                {
                    IPEndPoint remoteIp = null;
                    byte[] data = client.Receive(ref remoteIp);
                    string message = Encoding.Unicode.GetString(data);

                    
                    this.Invoke(new MethodInvoker(() =>
                    {
                        string time = DateTime.Now.ToShortTimeString();
                        lsbxChat.Items.Add(time + " " + message + "\r\n" + lsbxChat.Text);
                    }));
                }
                lsbxChat.SelectedIndex = lsbxChat.Items.Count - 1;
            }
            catch (ObjectDisposedException)
            {
                if (!alive)
                    return;
                throw;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        
        private void sendButton_Click(object sender, EventArgs e)
        {
            try
            {
                string message = String.Format("{0}: {1}", userName, tbxMessage.Text);
                byte[] data = Encoding.Unicode.GetBytes(message);
                client.Send(data, data.Length, HOST, REMOTEPORT);
                tbxMessage.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void logoutButton_Click(object sender, EventArgs e)
        {
            lblStatus.Text = "Offline";
            tbxMessage.Enabled = false;
            btnSend.Enabled = false;
            pOptionsMenu.Visible = false;
            ExitChat();
        }
        
        private void ExitChat()
        {
            string message = userName + " leeve chat";
            byte[] data = Encoding.Unicode.GetBytes(message);
            client.Send(data, data.Length, HOST, REMOTEPORT);
            client.DropMulticastGroup(groupAddress);

            alive = false;
            client.Close();

            btnLogin.Enabled = true;
            btnLogout.Enabled = false;
            btnSend.Enabled = false;
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (alive)
                ExitChat();
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void pTop_MouseDown(object sender, MouseEventArgs e)
        {
            int xOffset;
            int yOffset;

            if (e.Button == MouseButtons.Left)
            {
                xOffset = -e.X - SystemInformation.FrameBorderSize.Width;
                yOffset = -e.Y - SystemInformation.CaptionHeight -
                    SystemInformation.FrameBorderSize.Height;
                mouseOffset = new Point(xOffset, yOffset);
                isMouseDown = true;
            }
        }
        private void pTop_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMouseDown)
            {
                Point mousePos = Control.MousePosition;
                mousePos.Offset(mouseOffset.X, mouseOffset.Y);
                Location = mousePos;
            }
        }
        private void pTop_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isMouseDown = false;
            }
        }
        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) this.Close();
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            if (alive)
                ExitChat();
            Close();
        }

        private void btnClose_MouseEnter(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            btn.BackColor = Color.Red;
        }
        private void btnClose_MouseLeave(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            btn.BackColor = Color.SteelBlue;
        }


        private void btnOptions_Click(object sender, EventArgs e)
        {
            pOptionsMenu.Visible = !pOptionsMenu.Visible;
        }

        private void lsbxChat_SelectedIndexChanged(object sender, EventArgs e)
        {
            lsbxChat.SelectedIndex = lsbxChat.Items.Count - 1;

        }
    }
}
