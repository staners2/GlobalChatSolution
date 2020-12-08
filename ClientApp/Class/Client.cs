using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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
            tcpClient = new TcpClient();
        }

        public string NickName;
        public TcpClient tcpClient;
        public NetworkStream Stream;

        public ObservableCollection<string> ListMessage { get; set; }

        public bool IsConnect = false;

        private const string host = "127.0.0.1";
        private const int port = 8080;

        public bool ConnectServer(string pNickName)
        {
            try
            {
                tcpClient.Connect(host,port);
                Stream = tcpClient.GetStream();
                IsConnect = true;
                NickName = pNickName;

                byte[] data = Encoding.UTF8.GetBytes(NickName);
                Stream.Write(data, 0, data.Length);
                Stream.Flush();

                ListMessage = new ObservableCollection<string>();

                this.GetMessage();  // Получать сообщения и выводить их куда-то | ListMessage - список хранящий сообщения
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
            byte[] data = Encoding.UTF8.GetBytes(pMessage);
            Stream.Write(data, 0, data.Length);
            Stream.Flush();
        }

        public bool DisconnectServer()
        {
            try
            {
                Stream.Close();
                tcpClient.Close();
                IsConnect = false;
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
                        string Message = $"{Encoding.UTF8.GetString(WriteBytes, 0, length)}";
                        ListMessage.Add(Message);
                        //ChatBox.Items.Add(Message);
                        //Invoke((MethodInvoker)(() => ChatBox.Items.Add($"{Message}")));
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
