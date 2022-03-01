using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

#pragma warning disable CS8604 // Возможно, аргумент-ссылка, допускающий значение NULL.

namespace UdpClientApp
{
    class Program
    {
        
        static string remoteAddress; // хост для отправки данных
        static int remotePort; // порт для отправки данных
        static int localPort; // локальный порт для прослушивания входящих подключений

        static int number = 0;
        static List<bool> messagesOrder;
        static List<string> myMessages;

        static bool gotNum = false;
        static bool gotDate = false;
        static bool gotMes = false;

        static bool mesStatusTaken = false;
        static bool mesStatusSent = false;

        static int lostMes = -1;
        static void Main(string[] args)
        {
            try
            {
                Console.Write("Введите порт для прослушивания: ");
                localPort = Int32.Parse(Console.ReadLine());
                Console.Write("Введите удаленный адрес для подключения: ");
                remoteAddress = Console.ReadLine();
                Console.Write("Введите порт для подключения: ");
                remotePort = Int32.Parse(Console.ReadLine());
                myMessages = new List<string>();
                messagesOrder = new List<bool>() { false }; 


                Thread receiveThread = new Thread(new ThreadStart(ReceiveMessage));
                receiveThread.Start();
                Thread sendThread = new Thread(new ThreadStart(SendMessage));
                sendThread.Start();

                Thread ansThread = new Thread(new ThreadStart(AnswerToCalls));
                ansThread.Start();
               
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private static void SendMessage()
        {
            UdpClient sender = new UdpClient();
            
            try
            {
                while (true)
                {
                    
                    byte[] lostData;
                    byte[] messageNumber;
                    byte[] timeOfRecievedMes;

                    if (lostMes != -1)
                    {
                        messageNumber = BitConverter.GetBytes(lostMes);
                        timeOfRecievedMes = Encoding.Unicode.GetBytes(DateTime.Now.ToString());
                        lostData = Encoding.Unicode.GetBytes(myMessages[lostMes]);
                        sender.Send(lostData, lostData.Length, remoteAddress, remotePort);
                        sender.Send(messageNumber, messageNumber.Length, remoteAddress, remotePort);
                        sender.Send(timeOfRecievedMes, timeOfRecievedMes.Length, remoteAddress, remotePort);
                    }
                    

                    string message = Console.ReadLine();

                   
                    number++;
                    
                    messageNumber = BitConverter.GetBytes(number);
                    timeOfRecievedMes = Encoding.Unicode.GetBytes(DateTime.Now.ToString());
                    byte[] data = Encoding.Unicode.GetBytes(message);

                    myMessages.Add(message);

                    while (true)
                    {
                        
                            sender.Send(data, data.Length, remoteAddress, remotePort);
                            sender.Send(messageNumber, messageNumber.Length, remoteAddress, remotePort);
                            sender.Send(timeOfRecievedMes, timeOfRecievedMes.Length, remoteAddress, remotePort);
                       
                        

                        mesStatusSent = true;
                        Thread.Sleep(2000);
                        if (mesStatusSent)
                        {
                            Console.WriteLine($"\nyour message wasnt sent due to some occasions\n" +
                                "We are goint ot resend it...\n");
                        }
                        else 
                        {
                           
                            gotDate = false;
                            gotMes = false;
                            gotNum = false;
                            if (lostMes != -1)
                            {
                                lostMes = -1;
                            }

                            break;
                        }
                    }
                    
                    
                 
                    
                }
            }
            catch (Exception ex)
            {
                number--;
                Console.WriteLine(ex.Message);
            }
            finally
            {
                sender.Close();
            }
        }

        private static void ReceiveMessage()
        {
            UdpClient receiver = new UdpClient(localPort);
           
            IPEndPoint? remoteIp = null;
            try
            {
                while (true)
                {
                    
                    byte[] data = receiver.Receive(ref remoteIp);                   
                    string message = Encoding.Unicode.GetString(data);
                    if (message == "#data accepted#")
                    {
                        Console.WriteLine("\n#MESSAGE WAS DELIVERED#\n");
                        mesStatusSent = false;
                    }
                    else
                    {
                        if (message != null)
                        {
                            gotMes = true;
                        }
                        

                        data = receiver.Receive(ref remoteIp);
                        var num = BitConverter.ToInt32(data, 0);      
                        if (num == messagesOrder.Count)
                        {
                            gotNum = true;
                        }
                        else if (num > messagesOrder.Count)
                        {
                            lostMes = messagesOrder.Count;
                        }
                        
  
                        data = receiver.Receive(ref remoteIp);
                        var date = Encoding.Unicode.GetString(data);
                        if (!(date is null))
                        {
                            gotDate = true;
                        }
                        
                        

                        if (gotMes == gotDate && gotMes == gotNum && gotNum)
                        {
                            gotDate = false;
                            gotMes = false;
                            gotNum = false;
                            mesStatusTaken = true;
                            var finalMessage = string.Format("\n{0}: ({1}) {2} - \t{3}\n", "Companion",
                                                        num, DateTime.Now, message);
                            if (messagesOrder.Count == 1 || messagesOrder[num - 2] is true)
                            {
                                Console.WriteLine(finalMessage);
                            }
                           
                            messagesOrder[num - 1] = true;
                            messagesOrder.Add(false);
                        }                       
                    }        
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                receiver.Close(); 
            }
        }

        private static void AnswerToCalls()
        {
            UdpClient sender = new UdpClient();      
            try
            {
                while (true)
                {
                    //Console.WriteLine("Answer linew works");
                    Thread.Sleep(1000);
                    while(!mesStatusTaken)
                    { }
                    if (mesStatusTaken)
                    {
                        //Console.WriteLine("DELIVERING ANSWER");
                        byte[] answer = Encoding.Unicode.GetBytes("#data accepted#");
                        sender.Send(answer, answer.Length, remoteAddress, remotePort);
                        mesStatusTaken = false;
                        
                    } 
                }
            }
            catch (Exception ex)
            {           
                Console.WriteLine(ex.Message);
            }
            finally
            {
                sender.Close();
            }
        }
    }
}