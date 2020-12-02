using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ServerApp
{
    class cClient
    {
        public cClient(Socket pClientSocket, string pNick)
        {
            ClientSocket = pClientSocket;
            ClientNick = pNick;
        }

        public string ClientNick { get; set; }
        public Socket ClientSocket { get; set; }
    }
}
