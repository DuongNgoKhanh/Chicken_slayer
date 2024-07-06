using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Newtonsoft.Json;

namespace Chicken_slayer
{
    //public class Server
    //{
    //    private TcpListener server;
    //    private List<TcpClient> clients = new List<TcpClient>();
    //    private List<GameSession> gameSessions = new List<GameSession>();
    //    private RoomManager roomManager = new RoomManager();
    //    private Thread serverThread;
    //    private bool isRunning;

    //    public Server()
    //    {
    //        //StartServer();
    //    }
    //    public void StartServer()
    //    {
    //        serverThread = new Thread(new ThreadStart(RunServer));
    //        serverThread.IsBackground = true;
    //        serverThread.Start();
    //    }
    //    //public void StartServer() // Make sure this method is public or internal
    //    //{
            

    //    //    serverThread = new Thread(new ThreadStart(RunServer));
    //    //    serverThread.IsBackground = true;
    //    //    serverThread.Start();
    //    //}

    //    private void RunServer()
    //    {
    //        //isRunning = true;
    //        server = new TcpListener(IPAddress.Any, 5000);
    //        server.Start();

    //        while (isRunning)
    //        {
    //            if (server.Pending())
    //            {
    //                TcpClient client = server.AcceptTcpClient();
    //                clients.Add(client);

    //                if (clients.Count >= 2)
    //                {
    //                    var client1 = clients[0];
    //                    var client2 = clients[1];
    //                    clients.RemoveRange(0, 2);

    //                    GameSession session = new GameSession(client1, client2);
    //                    gameSessions.Add(session);

    //                    Thread sessionThread = new Thread(session.Run);
    //                    sessionThread.IsBackground = true;
    //                    sessionThread.Start();
    //                }
    //            }
    //            Thread.Sleep(100); // Add a small delay to avoid tight looping
    //        }
    //    }

    //    public void StopServer()
    //    {
    //        isRunning = false;
    //        server.Stop();
    //    }

    

    //private void HandleClient(object obj)
    //    {
    //        TcpClient client = (TcpClient)obj;
    //        NetworkStream stream = client.GetStream();
    //        byte[] buffer = new byte[1024];
    //        int bytesRead;

    //        while (isRunning && (bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
    //        {
    //            string requestJson = Encoding.ASCII.GetString(buffer, 0, bytesRead);
    //            Message request = JsonConvert.DeserializeObject<Message>(requestJson);
    //            Message response = ProcessRequest(request);
    //            string responseJson = JsonConvert.SerializeObject(response);
    //            byte[] responseBytes = Encoding.ASCII.GetBytes(responseJson);
    //            stream.Write(responseBytes, 0, responseBytes.Length);
    //        }

    //        client.Close();
    //        clients.Remove(client);
    //        Console.WriteLine("Client disconnected...");
    //    }

    //    private Message ProcessRequest(Message request)
    //    {
    //        Message response = new Message();
    //        switch (request.Command)
    //        {
    //            case "CreateRoom":
    //                string roomId = roomManager.CreateRoom(request.PlayerName);
    //                response.Command = "RoomCreated";
    //                response.RoomID = roomId;
    //                break;
    //            case "JoinRoom":
    //                Room room = roomManager.JoinRoom(request.RoomID, request.PlayerName);
    //                if (room != null)
    //                {
    //                    response.Command = "RoomJoined";
    //                    response.RoomID = room.RoomID;
    //                    response.PlayerName = room.Player2Name;
    //                }
    //                else
    //                {
    //                    response.Command = "Error";
    //                    response.MessageContent = "Room does not exist or is full.";
    //                }
    //                break;
    //            default:
    //                response.Command = "Error";
    //                response.MessageContent = "Unknown command.";
    //                break;
    //        }
    //        return response;
    //    }
    //}

    public class TcpConnectionManager
    {
        private static TcpConnectionManager instance;
        private static readonly object lockObject = new object();

        public TcpClient Client { get; private set; }
        public NetworkStream Stream { get; private set; }

        private TcpConnectionManager() { }

        public static TcpConnectionManager Instance
        {
            get
            {
                lock (lockObject)
                {
                    if (instance == null)
                    {
                        instance = new TcpConnectionManager();
                        instance.ConnectToServer();
                    }
                    return instance;
                }
            }
        }

        public void ConnectToServer()
        {
            try
            {
                Client = new TcpClient("127.0.0.1", 12345);
                Stream = Client.GetStream();
            }
            catch (SocketException ex)
            {
                // Handle connection error
                throw new Exception("Could not connect to server: " + ex.Message);
            }
        }

        public void StartReadingStream(Action<string> messageHandler)
        {
            Task.Run(() =>
            {
                byte[] buffer = new byte[1024];
                int bytesRead;

                while ((bytesRead = Stream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    string message = Encoding.ASCII.GetString(buffer, 0, bytesRead).Trim();
                    messageHandler?.Invoke(message);
                }
            });
        }

        public void CloseConnection()
        {
            Stream?.Close();
            Client?.Close();
            instance = null;
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
        public GameSession(NetworkStream stream1, NetworkStream stream2)
        {
            this.stream1 = stream1;
            this.stream2 = stream2;
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

    public class Message
    {
        public string Command { get; set; }
        public string RoomID { get; set; }
        public string Player1Name { get; set; }
        public string Player2Name { get; set; }
        public string MessageContent { get; set; }
        public bool IsPlayer1Ready { get; set; }
        public bool IsPlayer2Ready { get; set; }
        public string myName { get; set; }
        public GameState GameState { get; set; } // Add GameState property

    }

}
