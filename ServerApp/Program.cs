using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServerApp
{
    class Program
    {
        static int port = 36528; // 8080

        private static List<Socket> ListClient = new List<Socket>();

        static void Main(string[] args)
        {
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Any, port); // new IPEndPoint(IPAddress.Any, port);
            Socket listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            listenSocket.Bind(ipPoint);
            listenSocket.Listen(10);

            Console.WriteLine("Сервер запущен. Ожидание подключений...");
            while (true)
            {
                try
                {
                    Socket ClientSocket = listenSocket.Accept();
                    Console.WriteLine($"IP: {ClientSocket.RemoteEndPoint}");
                    ListClient.Add(ClientSocket);
                    AsyncStartMethod(ClientSocket);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        static public async void AsyncStartMethod(Socket pClientSocket)
        {
            await Task.Run(() => ListenClient(pClientSocket));
        }

        static public void ListenClient(Socket ClientSocket)
        {
            while (ClientSocket.Connected)
            {
                StringBuilder MessageBuilder = new StringBuilder();
                int bytes = 0; // количество полученных байтов
                byte[] data = new byte[256]; // буфер для получаемых данных

                try
                {
                    do
                    {
                        bytes = ClientSocket.Receive(data);
                        MessageBuilder.Append(Encoding.UTF8.GetString(data, 0, bytes));
                    } while (ClientSocket.Available > 0);
                }
                catch
                {
                    break;
                }

                if (MessageBuilder.ToString() != "")
                {
                    Console.WriteLine($"{MessageBuilder.ToString()}");
                    foreach (Socket Client in ListClient)
                    {
                        try
                        {
                            if (Client.Connected)
                            {
                                data = Encoding.UTF8.GetBytes(Convert.ToString(MessageBuilder));
                                Client.Send(data);
                            }
                            else
                            {
                                Client.Shutdown(SocketShutdown.Both);
                                Client.Close();
                            }
                        }
                        catch
                        {

                        }
                    }
                }
            }
        }
    }
}
