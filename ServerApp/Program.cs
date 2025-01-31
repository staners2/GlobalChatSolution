﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            Console.OutputEncoding = Encoding.UTF8;
            var IP = IPAddress.Any;
            TcpListener ServerSocket = new TcpListener(IP, port);
            Console.WriteLine($"Сервер запущен на IP => {IP.Address}:{port} . Ожидание подключений...");
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

                    string Message = $"{DateTime.Now} | {Client.NickName} connect in chat";
                    Console.WriteLine(Message);

                    Process cmd = new Process();
                    cmd.StartInfo.RedirectStandardOutput = true;
                    cmd.StartInfo.UseShellExecute = false;
                    cmd.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    cmd.StartInfo.FileName = "py";
                    cmd.StartInfo.Arguments = $"crypt.py --e --m \"{Message}\"";
                    cmd.Start();
                    cmd.WaitForExit();
                    string result_message = cmd.StandardOutput.ReadToEnd();

                    SendAllMessage(Client.Id, result_message);
                    AsyncListenServer(Client);

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        static public void RemoveConnection(string Id)
        {
            cClient client = ListClient.FirstOrDefault(c => c.Id == Id);
            if (client != null)
                ListClient.Remove(client);
        }

        static private string GetMessage(cClient Client)
        {
            StringBuilder MessageBuilder = new StringBuilder();
            try
            {
                byte[] data = new byte[256]; // буфер для получаемых данных
                int bytes = 0;
                do
                {
                    bytes = Client.Stream.Read(data, 0, data.Length);
                    MessageBuilder.Append(Encoding.UTF8.GetString(data, 0, bytes));
                }
                while (Client.Stream.DataAvailable);
            }
            catch
            {
                MessageBuilder = MessageBuilder.Append("");
            }
            

            if (MessageBuilder.ToString() == "")
            {
                string Message = $"{DateTime.Now} | {Client.NickName} leave the chat";
                Console.WriteLine(Message);

                Process cmd = new Process();
                cmd.StartInfo.RedirectStandardOutput = true;
                cmd.StartInfo.UseShellExecute = false;
                cmd.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                cmd.StartInfo.FileName = "py";
                cmd.StartInfo.Arguments = $"crypt.py --e --m \"{Message}\"";
                cmd.Start();
                cmd.WaitForExit();
                string result_message = cmd.StandardOutput.ReadToEnd();

                SendAllMessage(Client.Id, result_message);
                RemoveConnection(Client.Id);
            }

            return MessageBuilder.ToString();
        }

        static public void SendAllMessage(string Id, string Message)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(Message);

            for (int i = 0; i < ListClient.Count; i++)
            {
                
                if (ListClient[i].tcpClient != null) // ListClient[i].Id != Id
                {
                    try
                    {
                        ListClient[i].Stream.Write(bytes, 0, bytes.Length);
                        ListClient[i].Stream.Flush();
                    }
                    catch
                    {

                    }
                }
                
            }
        }

        static public async void AsyncListenServer(cClient Client)
        {
            await Task.Run(() =>
            {
                while (true)
                {
                    string Message = GetMessage(Client);
                    if (Message == "")
                    {
                        break;
                    }

                    Process cmd = new Process();
                    cmd.StartInfo.RedirectStandardOutput = true;
                    cmd.StartInfo.UseShellExecute = false;
                    cmd.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    cmd.StartInfo.FileName = "py";
                    cmd.StartInfo.Arguments = $"crypt.py --m \"{Message}\"";
                    cmd.Start();
                    cmd.WaitForExit();
                    string result_message = cmd.StandardOutput.ReadToEnd();

                    Message = $"{DateTime.Now} | {Client.NickName} => {result_message}";
                    Console.WriteLine(Message);

                    cmd.StartInfo.Arguments = $"crypt.py --e --m \"{Message}\"";
                    cmd.Start();
                    cmd.WaitForExit();
                    string res = cmd.StandardOutput.ReadToEnd();
                    Console.WriteLine(res);

                    SendAllMessage(Client.Id, res);
                }
            });
        }
    }
}
