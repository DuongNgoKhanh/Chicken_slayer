using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Chicken_slayer
{
    public class Server
    {
        private TcpListener server;
        private NetworkStream stream;
        private TcpClient client;
        public bool Player1Ready { get; set; }
        public bool Player2Ready { get; set; }

        public Server()
        {
            server = new TcpListener(IPAddress.Any, 8888);
            server.Start();
        }

        public void WaitForClient()
        {
            client = server.AcceptTcpClient();
            stream = client.GetStream();
        }

        public void SendData(string data)
        {
            byte[] bytesToSend = Encoding.ASCII.GetBytes(data);
            stream.Write(bytesToSend, 0, bytesToSend.Length);
        }

        public string ReceiveData()
        {
            byte[] buffer = new byte[1024];
            int bytesRead = stream.Read(buffer, 0, buffer.Length);
            return Encoding.ASCII.GetString(buffer, 0, bytesRead);
        }
    }
}
