using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace offline
{
    public partial class Server : Form
    {
        TcpListener server;
        List<TcpClient> clients = new List<TcpClient>();
        Thread serverThread;
        List<GameSession> gameSessions = new List<GameSession>();

        public Server()
        {
            InitializeComponent();
            StartServer();
        }

        private void StartServer()
        {
            serverThread = new Thread(new ThreadStart(RunServer));
            serverThread.IsBackground = true;
            serverThread.Start();
        }

        private void RunServer()
        {
            server = new TcpListener(IPAddress.Any, 12345);
            server.Start();
            UpdateStatus("Server started...");

            while (true)
            {
                TcpClient client = server.AcceptTcpClient();
                clients.Add(client);
                UpdateStatus("Client connected...");

                if (clients.Count >= 2)
                {
                    var client1 = clients[0];
                    var client2 = clients[1];
                    clients.RemoveRange(0, 2);

                    GameSession session = new GameSession(client1, client2);
                    gameSessions.Add(session);

                    Thread sessionThread = new Thread(session.Run);
                    sessionThread.IsBackground = true;
                    sessionThread.Start();

                    UpdateStatus("Game session started with two clients...");
                }
            }
        }

        private void UpdateStatus(string message)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string>(UpdateStatus), new object[] { message });
                return;
            }
            listBox1.Items.Add(message);
        }
    }

    public class GameSession
    {
        TcpClient client1;
        TcpClient client2;
        NetworkStream stream1;
        NetworkStream stream2;

        public GameSession(TcpClient client1, TcpClient client2)
        {
            this.client1 = client1;
            this.client2 = client2;
            this.stream1 = client1.GetStream();
            this.stream2 = client2.GetStream();
        }

        public void Run()
        {
            byte[] buffer1 = new byte[1024];
            byte[] buffer2 = new byte[1024];
            int byteCount1;
            int byteCount2;

            while (true)
            {
                if (stream1.DataAvailable)
                {
                    byteCount1 = stream1.Read(buffer1, 0, buffer1.Length);
                    if (byteCount1 > 0)
                    {
                        string message1 = Encoding.ASCII.GetString(buffer1, 0, byteCount1);
                        BroadcastMessage(message1, stream2);
                    }
                }

                if (stream2.DataAvailable)
                {
                    byteCount2 = stream2.Read(buffer2, 0, buffer2.Length);
                    if (byteCount2 > 0)
                    {
                        string message2 = Encoding.ASCII.GetString(buffer2, 0, byteCount2);
                        BroadcastMessage(message2, stream1);
                    }
                }
            }
        }

        private void BroadcastMessage(string message, NetworkStream stream)
        {
            byte[] data = Encoding.ASCII.GetBytes(message);
            stream.Write(data, 0, data.Length);
        }
    }
}
