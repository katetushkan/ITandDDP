using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            groupAddress = IPAddress.Parse(host);

            GetUsers(ref users);
        }

        bool alive = false;
        const int localPort = 8001;
        const int remotePort = 8001;
        const int TTL = 20;
        const string host = "235.5.5.1";
        const string path = "./Users.txt";
        UdpClient client;
        IPAddress groupAddress;

        User user = new User();
        List<User> users = new List<User>();

        void ChangeDialog(bool action)
        {
            try
            {
                if (action) richTextBox1.LoadFile($"./Log.txt", RichTextBoxStreamType.PlainText);
                else richTextBox1.SaveFile($"./Log.txt", RichTextBoxStreamType.PlainText);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        static void GetUsers(ref List<User> users)
        {
            try
            {
                using (StreamReader sr = new StreamReader(path, Encoding.Default))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        User user = new User()
                        {
                            Id = Int32.Parse(line[..1]),
                            Name = line[2..]
                        };
                        users.Add(user);
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void button1_Click(object sender, EventArgs e)
        {
            foreach (User k in users)
                if (k.Name == userNameBox.Text) user = k;

            if (user.Id != -1)
            {
                userNameBox.ReadOnly = true;
                try
                {
                    ChangeDialog(true);

                    client = new UdpClient(localPort);
                    client.JoinMulticastGroup(groupAddress, TTL);

                    Task receiveTask = new Task(ReceiveMessages);
                    receiveTask.Start();

                    string message = user.Name + " вошел в чат";
                    byte[] data = Encoding.Unicode.GetBytes(message);
                    client.Send(data, data.Length, host, remotePort);

                    loginButton.Enabled = false;
                    logoutButton.Enabled = true;
                    sendMessageButton.Enabled = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else MessageBox.Show("Аккаунт не найден", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        void ReceiveMessages()
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
                        string time = DateTime.Now.ToLongTimeString();
                        richTextBox2.Text = time + " " + message + "\r\n" + richTextBox2.Text;
                    }));
                }
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

        private void button3_Click(object sender, EventArgs e)
        {
            string message = user.Name + " покидает чат";
            byte[] data = Encoding.Unicode.GetBytes(message);
            client.Send(data, data.Length, host, remotePort);
            client.DropMulticastGroup(groupAddress);

            ExitFromUDPChat();
        }

        void ExitFromUDPChat()
        {
            if (user.Id != -1) ChangeDialog(false);

            alive = false;
            client.Close();
            user = new User();

            loginButton.Enabled = true;
            logoutButton.Enabled = false;
            userNameBox.ReadOnly = false;
            sendMessageButton.Enabled = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                string message = String.Format("{0}: {1}", user.Name, richTextBox2.Text);
                byte[] data = Encoding.Unicode.GetBytes(message);
                client.Send(data, data.Length, host, remotePort);
                richTextBox2.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}


