using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClientApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        static int port = 36528; // 8080
        private string address = "127.0.0.1"; // 127.0.0.1

        public bool IsConnect = false;

        private Socket ClientSocket;

        private string NickName = "";
        private void ConnectButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (NickNameBox.Text != "")
                {
                    if (IsConnect)
                    {
                        ChatBox.Items.Add($"{DateTime.Now} | Вы покинули чат ({NickName})");
                        IsConnect = false;
                        NickNameBox.Enabled = true;
                        SendButton.Enabled = false;
                        SendMessageBox.Enabled = false;
                        SendMessageBox.Text = "";
                        ConnectButton.Text = "Connect";

                        string Message = $"{DateTime.Now} | {NickName} отключился от чата!";
                        byte[] data = Encoding.UTF8.GetBytes(Message);
                        ClientSocket.Send(data);

                        ClientSocket.Shutdown(SocketShutdown.Both);
                        ClientSocket.Close();
                    }
                    else if (!IsConnect)
                    {
                        IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(address), port);
                        ClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                        try
                        {
                            // подключаемся к удаленному хосту
                            ClientSocket.Connect(ipPoint);
                            NickName = NickNameBox.Text;
                            IsConnect = true;
                            NickNameBox.Enabled = false;
                            SendButton.Enabled = true;
                            SendMessageBox.Enabled = true;
                            ConnectButton.Text = "Disconnect";

                            AsyncStartMethod();

                            string Message = $"{DateTime.Now} | {NickName} подключился в чат!";
                            byte[] data = Encoding.UTF8.GetBytes(Message);
                            ClientSocket.Send(data);
                        }
                        catch
                        {
                            IsConnect = false;
                            NickNameBox.Enabled = true;
                            SendButton.Enabled = false;
                            SendMessageBox.Enabled = false;
                            SendMessageBox.Text = "";
                            ConnectButton.Text = "Connect";
                            MessageBox.Show("Сервер выключен!");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Введите ваш NickName для подключения к чату",
                        "Error", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public async void AsyncStartMethod()
        {
            await Task.Run(() => ListenServer());
        }

        public void ListenServer()
        {
            while (ClientSocket.Connected && IsConnect)
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
                    Invoke((MethodInvoker) (() => ChatBox.Items.Add($"{MessageBuilder}")));
                }
            }
        }

        private void SendButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (SendMessageBox.Text != "")
                {
                    string Message = $"{DateTime.Now} | {NickName}: {SendMessageBox.Text}";
                    byte[] data = Encoding.UTF8.GetBytes(Message);
                    ClientSocket.Send(data);
                    SendMessageBox.Text = "";
                }
                else
                {
                    MessageBox.Show($"Введите сообщение!");
                }
            }
            catch
            {
                ChatBox.Items.Add($"{DateTime.Now} | Сервер выключен | Вы покинули чат ({NickName})");
                IsConnect = false;
                NickNameBox.Enabled = true;
                SendButton.Enabled = false;
                SendMessageBox.Enabled = false;
                SendMessageBox.Text = "";
                ConnectButton.Text = "Connect";
            }
        }
    }
}
