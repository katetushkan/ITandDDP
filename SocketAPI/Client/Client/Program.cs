using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;

namespace SocketUdpClient
{
    public static class Program
    {
        private static List<string> messages = new();

        private static IPAddress? localhostIP;

        private static string? storedMessagesFilePath;

        private static string? hostName;

        public static void Main(string[] args)
        {
            Console.CancelKeyPress += OnExit;

            Console.Write("Enter port for listening for messages: ");
            int localPort = int.Parse(Console.ReadLine()!);

            Console.Write("Enter port for sending messages: ");
            int remotePort = int.Parse(Console.ReadLine()!);

            storedMessagesFilePath = $"messages_{localPort}_{remotePort}.json";
            var task = new Task(RestoreAllMessages);
            task.Start();

            Console.WriteLine($"Your messages:{Environment.NewLine}");

            task.Wait();
            PrintStoredMessages(remotePort);

            try
            {
                hostName = Dns.GetHostName();
                using var listeningSocket = ConnectSocket(hostName!, localPort);

                _ = Task.Run(() => Listener(listeningSocket, remotePort));
                Sender(listeningSocket, remotePort);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                StoreAllMessages();
            }
        }

        private static Socket ConnectSocket(string hostName, int port)
        {
            var hostEntry = Dns.GetHostEntry(hostName);
            var listeningSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            localhostIP = hostEntry.AddressList[1];
            var localEP = new IPEndPoint(localhostIP, port);
            listeningSocket.Bind(localEP);
            return listeningSocket;
        }

        private static void Sender(Socket listeningSocket, int remotePort)
        {
            while (true)
            {
                var message = Console.ReadLine();
                if (string.IsNullOrEmpty(message))
                {
                    continue;
                }

                var buffer = Encoding.UTF8.GetBytes(message);

                EndPoint remoteEP = new IPEndPoint(localhostIP!, remotePort);
                listeningSocket.SendTo(buffer, remoteEP);

                StoreMessage(remotePort, message);
            }
        }

        private static void Listener(Socket listeningSocket, int remotePort)
        {
            var sb = new StringBuilder();
            var data = new byte[256];
            int bytes;

            while (true)
            {
                EndPoint remoteEP = new IPEndPoint(localhostIP!, remotePort);

                do
                {
                    bytes = listeningSocket.ReceiveFrom(data, ref remoteEP);
                    sb.Append(Encoding.UTF8.GetString(data, 0, bytes));
                }
                while (listeningSocket.Available > 0);

                var remoteIPEP = remoteEP as IPEndPoint;

                var message = $"{remoteIPEP!.Address}:{remoteIPEP.Port:0000} - {sb}";
                Console.WriteLine(message);

                StoreMessage(remotePort, message);

                sb.Clear();
            }
        }

        private static void StoreMessage(int port, string message)
        {
             messages.Add(message);
        }

        private static void StoreAllMessages()
        {
            var serialized = JsonSerializer.Serialize(messages);
            File.WriteAllText(storedMessagesFilePath!, serialized);
        }

        private static void RestoreAllMessages()
        {
            if (!File.Exists(storedMessagesFilePath))
            {
                return;
            }

            var serialized = File.ReadAllText(storedMessagesFilePath);
            var res = JsonSerializer.Deserialize(serialized, typeof(List<string>)) as List<string>;

            if (res is not null)
            {
                messages = res;
            }
        }

        private static void PrintStoredMessages(int port)
        {
            foreach (var message in messages)
            {
                Console.WriteLine(message);
            }
        }

        private static void OnExit(object? sender, ConsoleCancelEventArgs args)
        {
            StoreAllMessages();
        }
    }
}