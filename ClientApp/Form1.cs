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
using ClientApp.Class;

namespace ClientApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Client = new Client();
        }

        static int port = 8080; // 8080
        private string address = "127.0.0.1"; // 127.0.0.1

        public Client Client;

        private string NickName = "";
        private void ConnectButton_Click(object sender, EventArgs e)
        {
            NickName = NickNameBox.Text;
            try
            {
                if (NickNameBox.Text != "")
                {
                    if (!Client.IsConnect)
                    {
                        if (Client.ConnectServer(NickName))
                        {
                            NickNameBox.Enabled = false;
                            SendButton.Enabled = true;
                            SendMessageBox.Enabled = true;
                            SendMessageBox.Text = "";
                            ConnectButton.Text = "Disconnect";
                            ///
                            ///
                            /// 
                        }
                    }
                    else
                    {
                        Client.DisconnectServer();
                        NickNameBox.Enabled = true;
                        SendButton.Enabled = false;
                        SendMessageBox.Enabled = false;
                        SendMessageBox.Text = "";
                        ConnectButton.Text = "Connect";
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
            
                

           /* if (.ToString() != "")
            {
                Invoke((MethodInvoker) (() => ChatBox.Items.Add($"{}")));
            }*/
            
        }

        private void SendButton_Click(object sender, EventArgs e)
        {
            try
            {
                
                /*if (SendMessageBox.Text != "")
                {
                    string Message = $"{DateTime.Now} | {NickName}: {SendMessageBox.Text}";
                    // ClientSocket.Send(data);
                    SendMessageBox.Text = "";
                }
                else
                {
                    MessageBox.Show($"Введите сообщение!");
                }*/
            }
            catch
            {
                ChatBox.Items.Add($"{DateTime.Now} | Сервер выключен | Вы покинули чат ({NickName})");
                NickNameBox.Enabled = true;
                SendButton.Enabled = false;
                SendMessageBox.Enabled = false;
                SendMessageBox.Text = "";
                ConnectButton.Text = "Connect";
            }
        }
    }
}
