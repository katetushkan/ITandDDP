using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Itit_2
{
    class Program
    {
            static readonly object _lock = new object();
            static readonly Dictionary<int, TcpClient> list_clients = new Dictionary<int, TcpClient>();

            static void Main(string[] args)
            {
                int count = 1;

                TcpListener ServerSocket = new TcpListener(IPAddress.Any, 5000);
                ServerSocket.Start();

                while (true)
                {
                    TcpClient client = ServerSocket.AcceptTcpClient();
                    lock (_lock) list_clients.Add(count, client);
                    Console.WriteLine("Someone connected!!", count, "    ", client);

                    Thread t = new Thread(handle_clients);
                    t.Start(count);
                    count++;
                }
            }

            public static void handle_clients(object o)
            {
                int id = (int)o;
                TcpClient client;

                lock (_lock) client = list_clients[id];



                //
                string data = string.Empty;
                byte[] b = new byte[1024];
                bool kk = true;
                while (kk)
                {
                    NetworkStream stream = client.GetStream();
                    byte[] buffer = new byte[1024];
                    int byte_count = stream.Read(buffer, 0, buffer.Length);
                    if (byte_count == 0)
                    {
                        break;
                    }

                    data = Encoding.ASCII.GetString(buffer, 0, byte_count);
                    if (data.IndexOf('|')==-1)
                    {
                        continue;
                    }

                    string[] cont = data.Split('|');
                    string[] l = File.ReadAllLines("C:\\Users\\user\\source\\repos\\Itir1\\Itir1\\Logs.txt");
                    int password = 0;
                    for (int i=0; i<l.Length; i++)
                    {
                        string[] ll = l[i].Split('|');
                        if (cont[0] == ll[0])
                        {
                            if (cont[1]==ll[1])
                            {
                                b = Encoding.ASCII.GetBytes("1" + Environment.NewLine);
                                stream.Write(b, 0, b.Length);
                                password = 1;
                                kk = false;
                            }
                            else
                            {
                                b = Encoding.ASCII.GetBytes("2" + Environment.NewLine);
                                stream.Write(b, 0, b.Length);
                                password = 2;
                                continue;
                            }
                        }
                    }

                    if (password==0)
                    {
                        b = Encoding.ASCII.GetBytes("1" + Environment.NewLine);
                        stream.Write(b, 0, b.Length);
                        using (StreamWriter writer = new StreamWriter("C:\\Users\\user\\source\\repos\\Itir1\\Itir1\\Logs.txt", true))
                        {
                            writer.WriteLineAsync(data);
                        }
                        kk = false;
                        break;
                    }
                }
                byte[] bufferr = Encoding.ASCII.GetBytes(data + Environment.NewLine);
            //




                string[] t = File.ReadAllLines("C:\\Users\\user\\source\\repos\\Itir1\\Itir1\\data.txt");
                lock (_lock)
                {
                    NetworkStream stream = client.GetStream();
                    for (int i = 0; i < t.Length; i++)
                    {
                        string l = t[i];
                        bufferr = Encoding.ASCII.GetBytes(t[i] + Environment.NewLine);
                        stream.Write(bufferr, 0, bufferr.Length);
                    }
                }

                while (true)
                {
                    NetworkStream stream = client.GetStream();
                    byte[] buffer = new byte[1024];
                    int byte_count = stream.Read(buffer, 0, buffer.Length);

                    if (byte_count == 0)
                    {
                        break;
                    }

                    data = Encoding.ASCII.GetString(buffer, 0, byte_count);
                    broadcast(data);
                    Console.WriteLine(data);
                }

                lock (_lock) list_clients.Remove(id);
                client.Client.Shutdown(SocketShutdown.Both);
                client.Close();
            }

            public static void broadcast(string data)
            {
                byte[] buffer = Encoding.ASCII.GetBytes(data + Environment.NewLine);
                using (StreamWriter writer = new StreamWriter("C:\\Users\\user\\source\\repos\\Itir1\\Itir1\\data.txt", true))
                {
                    writer.WriteLineAsync(data);
                }

                lock (_lock)
                {
                    foreach (TcpClient c in list_clients.Values)
                    {
                        NetworkStream stream = c.GetStream();
                        stream.Write(buffer, 0, buffer.Length);
                    }
                }
            }
        }
    }

