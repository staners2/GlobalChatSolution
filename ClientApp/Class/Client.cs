using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClientApp.Class
{
    public class Client
    {
        public Client()
        {
            // Id = Guid.NewGuid().ToString();
            tcpClient = new TcpClient();
        }

        public string NickName;
        // public string Id;
        public TcpClient tcpClient;
        public NetworkStream Stream;

        public List<string> ListMessage;

        public bool IsConnect { get; set; } = false;

        private const string host = "127.0.0.1";
        private const int port = 8080;

        public bool ConnectServer(string pNickName)
        {
            try
            {
                tcpClient.Connect(host,port);
                Stream = tcpClient.GetStream();
                IsConnect = true;
                ListMessage = new List<string>();
                NickName = pNickName;

                this.GetMessage();  // Получать сообщения и выводить их куда-то | ListMessage - список хранящий сообщения
                this.SendMessage($"{DateTime.Now} | {NickName} подключился в чат!");
                return (true);
            }
            catch
            {
                IsConnect = false;
                return (false);
            }
        }

        public void SendMessage(string pMessage)
        {
            string Message = $"{DateTime.Now} | {NickName}: {pMessage}";
            byte[] data = Encoding.UTF8.GetBytes(Message);
            Stream.Write(data, 0, data.Length);
            Stream.Flush();
        }

        public bool DisconnectServer()
        {
            try
            {
                this.SendMessage($"{DateTime.Now} | {NickName} покинул чат!");
                IsConnect = false;
                Stream.Close();
                tcpClient.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async void GetMessage()
        {
            await Task.Run(() =>
            {
                while (IsConnect)
                {
                    try
                    {
                        byte[] WriteBytes = new byte[256];
                        int length = Stream.Read(WriteBytes, 0, WriteBytes.Length);
                        string Message = Encoding.UTF8.GetString(WriteBytes, 0, length);
                        ListMessage.Add(Message);
                    }
                    catch
                    {
                        break;
                    }
                }
            });
        }

    }
}
