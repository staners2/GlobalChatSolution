using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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
        }

        static int port = 8080; // 8080
        private string address = "127.0.0.1"; // 127.0.0.1

        public Client Client;

        public bool IsConnect = false;
        private string NickName = "";
        private void ConnectButton_Click(object sender, EventArgs e)
        {
            NickName = NickNameBox.Text;
            try
            {
                if (NickNameBox.Text != "")
                {

                    if (!IsConnect)
                    {
                        Client = new Client();
                        if (Client.ConnectServer(NickName))
                        {
                            IsConnect = true;
                            NickNameBox.Enabled = false;
                            SendButton.Enabled = true;
                            SendMessageBox.Enabled = true;
                            SendMessageBox.Text = "";
                            ConnectButton.Text = "Disconnect";
                            ChatBox.Items.Clear();
                            ChatBox.Items.Add($"{DateTime.Now} | {Client.NickName} подключился в чат!");
                            Client.ListMessage.CollectionChanged += ListMessage_Changed;
                            Client.ListClient.CollectionChanged -= ListClient_Changed;

                        }
                    }
                    else
                    {
                        IsConnect = false;
                        Client.DisconnectServer();
                        NickNameBox.Enabled = true;
                        SendButton.Enabled = false;
                        SendMessageBox.Enabled = false;
                        SendMessageBox.Text = "";
                        ConnectButton.Text = "Connect";
                        ChatBox.Items.Add($"{DateTime.Now} | {Client.NickName} покинул чат!");
                        Client.ListMessage.CollectionChanged -= ListMessage_Changed;
                        Client.ListClient.CollectionChanged -= ListClient_Changed;
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

        private void ListMessage_Changed(object sender, NotifyCollectionChangedEventArgs e)
        {

            Invoke((MethodInvoker) (() =>
            {

                ChatBox.Items.Add(e.NewItems[0].ToString());
                ChatBox.SetSelected(Client.ListMessage.Count - 1, true);
                ChatBox.SetSelected(Client.ListMessage.Count - 1, false);

            }));
        }

        private void ListClient_Changed(object sender, NotifyCollectionChangedEventArgs e)
        {
            // Bag меню подключенных пользователей
            Invoke((MethodInvoker) (() =>
            {
                UserBox.Items.Clear();
                foreach (var VARIABLE in Client.ListClient)
                {
                    UserBox.Items.Add(e.NewItems[0].ToString());
                }
                
            }));
        }

            //ChatBox.Items.Add(e.NewItems[0].ToString());
            //_ChatBox.Items.Add(e.NewItems[0].ToString());

            //Invoke((MethodInvoker)(() => ChatBox.Items.Add($"{Message}")));
            /*switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add: // если добавление
                    User newUser = e.NewItems[0] as User;
                    Console.WriteLine($"Добавлен новый объект: {newUser.Name}");
                    break;
                case NotifyCollectionChangedAction.Remove: // если удаление
                    User oldUser = e.OldItems[0] as User;
                    Console.WriteLine($"Удален объект: {oldUser.Name}");
                    break;
                case NotifyCollectionChangedAction.Replace: // если замена
                    User replacedUser = e.OldItems[0] as User;
                    User replacingUser = e.NewItems[0] as User;
                    Console.WriteLine($"Объект {replacedUser.Name} заменен объектом {replacingUser.Name}");
                    break;
            }*/


            public async void ListenServer()
        {
            await Task.Run(() => ListenServer());
        }

 
            /* if (.ToString() != "")
             {
                 Invoke((MethodInvoker) (() => ChatBox.Items.Add($"{}")));
             }*/
            
        

        private void SendButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (SendMessageBox.Text != "")
                {
                    string Message = $"{SendMessageBox.Text}";
                    Client.SendMessage(Message);
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
                NickNameBox.Enabled = true;
                SendButton.Enabled = false;
                SendMessageBox.Enabled = false;
                SendMessageBox.Text = "";
                ConnectButton.Text = "Connect";
            }
        }
    }
}
