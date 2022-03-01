using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace CLL
{
    class Program
    {


        static void Main(string[] args)
        {
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            int port = 5000;
            TcpClient client = new TcpClient();
            client.Connect(ip, port);
            Console.WriteLine("client connected!!");
            NetworkStream ns = client.GetStream();
            Thread thread = new Thread(o => ReceiveData((TcpClient)o));
            thread.Start(client);
            string s;



            string name = string.Empty;
            Console.ReadKey();
            while (!chat)
            //while (string.IsNullOrEmpty(name))
            {
                Console.WriteLine();
                Console.WriteLine("Dont use symbol |");
                Console.Write("Your name: ");
                if (chat)
                {
                    break;
                }
                name = Console.ReadLine();
                if (string.IsNullOrEmpty(name))
                {
                    continue;
                }
                Console.WriteLine("Dont use symbol |");
                Console.Write("Your password: ");
                string passw = Console.ReadLine();

                if (name.IndexOf('|') != -1 || passw.IndexOf('|') != -1)
                {
                    Console.WriteLine("Found |");
                    continue;
                }

                string ss = string.Concat(name, "|", passw);
                byte[] logs = Encoding.ASCII.GetBytes(ss);
                ns.Write(logs, 0, logs.Length);
                Thread.Sleep(3000);
            }
            name = string.Concat(name, ": ");








            while (!string.IsNullOrEmpty((s = Console.ReadLine())))
            {
                s = string.Concat(name, s);
                byte[] buffer = Encoding.ASCII.GetBytes(s);
                ns.Write(buffer, 0, buffer.Length);
            }

            client.Client.Shutdown(SocketShutdown.Send);
            thread.Join();
            ns.Close();
            client.Close();
            Console.WriteLine("disconnect from server!!");
            Console.ReadKey();
        }


        public static bool chat = false;

        static void ReceiveData(TcpClient client)
        {
            NetworkStream ns = client.GetStream();
            byte[] receivedBytes = new byte[1024];
            int byte_count;
            /*
            while ((byte_count = ns.Read(receivedBytes, 0, receivedBytes.Length)) > 0)
            {
                Console.Write(Encoding.ASCII.GetString(receivedBytes, 0, byte_count));
            }
            */
            
            while ((byte_count = ns.Read(receivedBytes, 0, receivedBytes.Length)) > 0)
            {
                if (chat)
                {
                    Console.Write(Encoding.ASCII.GetString(receivedBytes, 0, byte_count));
                }
                else
                {
                    string k = (Encoding.ASCII.GetString(receivedBytes, 0, byte_count));
                    if (k == "1\r\n")
                    {
                        Console.Write(Encoding.ASCII.GetString(receivedBytes, 0, byte_count));
                        chat = true;
                    }
                    else
                    {
                        Console.WriteLine("Password not right");
                        Console.Write(Encoding.ASCII.GetString(receivedBytes, 0, byte_count));
                    }
                }
            }
            

        }
    }
}