using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ServerApp.Class
{
    public class cClient
    {
        public cClient(string pNickName, TcpClient pTcpClient)
        {
            NickName = pNickName;
            Id = Guid.NewGuid().ToString();

            tcpClient = pTcpClient;
            Stream = tcpClient.GetStream();
        }

        public string NickName;

        public string Id;

        public TcpClient tcpClient;

        public NetworkStream Stream;


    }
}
