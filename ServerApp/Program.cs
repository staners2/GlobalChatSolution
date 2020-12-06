using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ServerApp.Class;

namespace ServerApp
{
    class Program
    {
        static int port = 8080; // 8080

        private static List<cClient> ListClient = new List<cClient>();

        static void Main(string[] args)
        {
            TcpListener ServerSocket = new TcpListener(IPAddress.Any, port);
            Console.WriteLine("Сервер запущен. Ожидание подключений...");
            ServerSocket.Start();

            while (true)
            {
                try
                {
                    TcpClient ClientSocket = ServerSocket.AcceptTcpClient();
                    NetworkStream stream = ClientSocket.GetStream();

                    byte[] ReadBytes = new byte[256];
                    int length = stream.Read(ReadBytes, 0, ReadBytes.Length);

                    string MessageNickName = Encoding.UTF8.GetString(ReadBytes, 0, length);
                    cClient Client = new cClient(MessageNickName,ClientSocket);

                    ListClient.Add(Client);

                    string Message = $"{DateTime.Now} | {Client.NickName} подключился в чат!";
                    Console.WriteLine($"{Message}");
                    SendAllMessage(Client.Id, Message);

                    AsyncStartMethod(Client);

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        static public void RemoveConnection(string Id)
        {
            // получаем по id закрытое подключение
            cClient client = ListClient.FirstOrDefault(c => c.Id == Id);
            // и удаляем его из списка подключений
            if (client != null)
                ListClient.Remove(client);
        }

        static private string GetMessage(cClient Client)
        {
            byte[] data = new byte[256]; // буфер для получаемых данных
            StringBuilder MessageBuilder = new StringBuilder();
            int bytes = 0;
            do
            {
                bytes = Client.Stream.Read(data, 0, data.Length);
                MessageBuilder.Append(Encoding.Unicode.GetString(data, 0, bytes));
            }
            while (Client.Stream.DataAvailable);

            Console.WriteLine($"{MessageBuilder}");

            return MessageBuilder.ToString();
        }

        static public void SendAllMessage(string Id, string Message)
        {

            byte[] bytes = Encoding.UTF8.GetBytes(Message);

            for (int i = 0; i < ListClient.Count; i++)
            {
                
                if (ListClient[i].Id != Id && ListClient[i].tcpClient != null)
                {
                    ListClient[i].Stream.Write(bytes, 0, bytes.Length);
                    ListClient[i].Stream.Flush();
                }
                
            }
        }

        static public async void AsyncStartMethod(cClient Client)
        {
            await Task.Run(() =>
            {
                while (true)
                {
                    if (Client.tcpClient.Connected)
                    {
                        string Message = GetMessage(Client);
                        Message = $"{DateTime.Now} | {Client.NickName}: {Message}";
                        SendAllMessage(Client.Id, Message);
                    }
                    else
                    {
                        string Message = $"{DateTime.Now} | {Client.NickName} покинул чат!";
                        Console.WriteLine(Message);
                        SendAllMessage(Client.Id, Message);
                        RemoveConnection(Client.Id);
                        break;
                    }
                }
            });
        }
    }
}
