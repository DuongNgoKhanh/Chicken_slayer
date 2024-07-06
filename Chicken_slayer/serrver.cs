using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chicken_slayer
{
    public partial class serrver : Form
    {
        private TcpListener server;
        private List<TcpClient> clients = new List<TcpClient>();
        private List<TcpClient> readyClients = new List<TcpClient>();
        private List<GameSession> gameSessions = new List<GameSession>();
        private RoomManager roomManager = new RoomManager();
        private Thread serverThread;
        private bool isRunning;
        private object streamLock = new object();
        public serrver()
        {
            InitializeComponent();
            StartServer();
        }

        public void StartServer()
        {
            serverThread = new Thread(new ThreadStart(RunServer));
            serverThread.IsBackground = true;
            serverThread.Start();
        }

        private void RunServer()
        {
            isRunning = true;
            server = new TcpListener(IPAddress.Any, 12345);
            server.Start();
            UpdateStatus("Server started...");

            while (isRunning)
            {
                if (server.Pending())
                {
                    TcpClient client = server.AcceptTcpClient();
                    clients.Add(client);
                    UpdateStatus($"Client {client.Client.RemoteEndPoint} connected...");

                    // Khởi chạy luồng để xử lý client
                    Thread clientThread = new Thread(() => HandleClient(client));
                    clientThread.IsBackground = true;
                    clientThread.Start();
                }

                Thread.Sleep(100); // Add a small delay to avoid tight looping
            }
        }

        public void StopServer()
        {
            isRunning = false;
            server.Stop();
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

        private void HandleClient(TcpClient client)
        {
            NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[1024];
            int bytesRead;

            while (isRunning && (bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
            {
                string requestJson = Encoding.ASCII.GetString(buffer, 0, bytesRead).Trim();
                Message request;
                try
                {
                    request = JsonConvert.DeserializeObject<Message>(requestJson);
                }
                catch (JsonReaderException)
                {
                    // Handle the exception, e.g., log it or take corrective action
                    continue;
                }

                Message response = ProcessRequest(request, client);
                string responseJson = JsonConvert.SerializeObject(response);

                lock (streamLock)
                {
                    byte[] responseBytes = Encoding.ASCII.GetBytes(responseJson);
                    stream.Write(responseBytes, 0, responseBytes.Length);
                }
            }

            client.Close();
            clients.Remove(client);
            Console.WriteLine("Client disconnected...");
        }
        string player1name = null;
        string player2name;
        private Message ProcessRequest(Message request, TcpClient client)
        {
            Message response = new Message();
            switch (request.Command)
            {
                case "CreateRoom":
                    string roomId = roomManager.CreateRoom(request.Player1Name);
                    response.Command = "RoomCreated";
                    response.RoomID = roomId;
                    response.Player1Name = request.Player1Name;
                    break;
                case "JoinRoom":
                    Room room = roomManager.JoinRoom(request.RoomID, request.Player2Name);
                    if (room != null)
                    {
                        response.Command = "RoomJoined";
                        response.RoomID = room.RoomID;
                        response.Player1Name = room.Player1Name;
                        response.Player2Name = room.Player2Name;
                    }
                    else
                    {
                        response.Command = "Error";
                        response.MessageContent = "Room does not exist or is full.";
                    }
                    break;
                case "Ready":
                    Room currentRoom = roomManager.GetRoom(request.RoomID);
                    if (currentRoom != null)
                    {
                        if (request.Player1Name == currentRoom.Player1Name)
                        {
                            currentRoom.IsPlayer1Ready = request.IsPlayer1Ready;
                        }
                        else if (request.Player2Name == currentRoom.Player2Name)
                        {
                            currentRoom.IsPlayer2Ready = request.IsPlayer2Ready;
                        }

                        response.Command = "ReadyStatusUpdated";
                        response.RoomID = currentRoom.RoomID;
                        response.IsPlayer1Ready = currentRoom.IsPlayer1Ready;
                        response.IsPlayer2Ready = currentRoom.IsPlayer2Ready;
                    }
                    else
                    {
                        response.Command = "Error";
                        response.MessageContent = "Room does not exist.";
                    }
                    break;
                case "FindRandomOpponent":
                case "CheckOpponentStatus":
                    
                    lock (streamLock)
                    {
                        if (!readyClients.Contains(client))
                        {
                            readyClients.Add(client);
                            if(player1name!=null)player1name = request.myName;
                        }    
                           
                    }

                    if (readyClients.Count >= 2)
                    {
                        Message response1 = new Message();
                        Message response2 = new Message();
                        player2name = request.myName;
                        response1.Command = "FoundOpponent";
                        response2.Command = "FoundOpponent";
                        response1.myName = player2name;
                        response2.myName = player1name;

                        NetworkStream stream1;
                        NetworkStream stream2;

                        string responseJson1 = JsonConvert.SerializeObject(response1);
                        string responseJson2 = JsonConvert.SerializeObject(response2);
                        byte[] responseBytes1 = Encoding.ASCII.GetBytes(responseJson1);
                        byte[] responseBytes2 = Encoding.ASCII.GetBytes(responseJson2);

                        lock (streamLock)
                        {
                            stream1 = readyClients[0].GetStream();
                            stream2 = readyClients[1].GetStream();
                        }

                        lock (streamLock)
                        {
                            stream1.Write(responseBytes1, 0, responseBytes1.Length);
                            stream2.Write(responseBytes2, 0, responseBytes2.Length);

                            TcpClient client1 = readyClients[0];
                            TcpClient client2 = readyClients[1];
                            readyClients.RemoveRange(0, 2);

                            GameSession session = new GameSession(client1, client2);
                            gameSessions.Add(session);

                            Thread sessionThread = new Thread(session.Run);
                            sessionThread.IsBackground = true;
                            sessionThread.Start();
                        }
                    }
                    else
                    {
                        response.Command = "WaitingForOpponent";
                    }
                    break;
                default:
                    response.Command = "Error";
                    response.MessageContent = "Unknown command.";
                    break;
            }
            return response;
        }
    }
}
