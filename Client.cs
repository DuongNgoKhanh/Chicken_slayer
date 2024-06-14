using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Chicken_slayer
{
    internal class Client
    {
        private TcpClient client;
        private NetworkStream stream;
        public bool PlayerReady { get; set; }

        public Client(string ipAddress)
        {
            client = new TcpClient(ipAddress, 8888);
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
