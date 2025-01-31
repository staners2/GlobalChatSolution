﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
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

        private const int port = 8080;

        public bool ConnectServer(string pNickName, string IP)
        {
            try
            {
                tcpClient.Connect(IP, port);
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
            Process cmd = new Process();
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.UseShellExecute = false;
            cmd.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            cmd.StartInfo.FileName = "py";
            cmd.StartInfo.Arguments = $"crypt.py --e --m \"{pMessage}\"";
            cmd.Start();
            cmd.WaitForExit();
            string result = cmd.StandardOutput.ReadToEnd();

            byte[] data = Encoding.UTF8.GetBytes(result);
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

                        Process cmd = new Process();
                        cmd.StartInfo.RedirectStandardOutput = true;
                        cmd.StartInfo.UseShellExecute = false;
                        cmd.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                        cmd.StartInfo.FileName = "py";
                        cmd.StartInfo.Arguments = $"crypt.py --m \"{Message}\"";
                        cmd.Start();
                        cmd.WaitForExit();
                        string decode_message = cmd.StandardOutput.ReadToEnd();

                        ListMessage.Add($"{decode_message}");
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
