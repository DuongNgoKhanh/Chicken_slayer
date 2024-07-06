using System;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Net.Sockets;
using System.Text;

namespace Chicken_slayer
{
    public partial class Waiting_room : Form
    {
        private RoomManager roomManager;
        private Room currentRoom;
        private bool isServer;
        private System.Windows.Forms.Timer lobbyTimer;

        public Waiting_room(RoomManager roomManager, string roomId, bool isServer)
        {
            InitializeComponent();
            this.roomManager = roomManager;
            this.isServer = isServer;
            currentRoom = roomManager.GetRoom(roomId);
            txtb_soPhong.Text = "Room ID: " + roomId;
            //UpdateRoomStatus();
            InitLobby();
        }

        public void UpdateRoomStatus()
        {
            userName1.Text = currentRoom.Player1Name;
            userName2.Text = currentRoom.Player2Name;
        }

        private void btn_back_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void InitLobby()
        {
            lobbyTimer = new System.Windows.Forms.Timer();
            lobbyTimer.Interval = 1000;
            lobbyTimer.Tick += LobbyTimer_Tick;
            lobbyTimer.Start();
        }

        private void LobbyTimer_Tick(object sender, EventArgs e)
        {
            if (currentRoom != null)
            {
                if (currentRoom.IsPlayer1Ready && currentRoom.IsPlayer2Ready)
                {
                    StartGame();
                }
            }
        }

        private void StartGame()
        {
            lobbyTimer.Stop();
            MessageBox.Show("Cả hai người chơi đều đã sẵn sàng! Bắt đầu trò chơi...");
            GameOnline gameForm = new GameOnline();
            gameForm.Show();
            this.Close();
        }

        private void btn_sanSang_Click(object sender, EventArgs e)
        {
            string currentPlayer;
            bool isReady;

            if (ApplicationState.CurrentUsername == currentRoom.Player1Name)
            {
                currentPlayer = currentRoom.Player1Name;
                isReady = !currentRoom.IsPlayer1Ready;
            }
            else
            {
                currentPlayer = currentRoom.Player2Name;
                isReady = !currentRoom.IsPlayer2Ready;
            }

            Message updateStatusRequest = new Message
            {
                Command = "Ready",
                RoomID = currentRoom.RoomID,
                Player1Name = currentPlayer,
                IsPlayer1Ready = isServer ? isReady : currentRoom.IsPlayer1Ready,
                IsPlayer2Ready = isServer ? currentRoom.IsPlayer2Ready : isReady
            };

            string requestJson = JsonConvert.SerializeObject(updateStatusRequest);
            byte[] requestBytes = Encoding.ASCII.GetBytes(requestJson);
            string responseJson = SendToServer(requestBytes);
            Message response = JsonConvert.DeserializeObject<Message>(responseJson);

            if (response.Command == "ReadyStatusUpdated")
            {
                currentRoom.IsPlayer1Ready = response.IsPlayer1Ready;
                currentRoom.IsPlayer2Ready = response.IsPlayer2Ready;
                UpdateRoomStatus();
            }
        }

        private string SendToServer(byte[] requestBytes)
        {
            // Sử dụng TcpConnectionManager để gửi yêu cầu tới server
            TcpConnectionManager connectionManager = TcpConnectionManager.Instance;
            NetworkStream stream = connectionManager.Stream;

            stream.Write(requestBytes, 0, requestBytes.Length);

            byte[] buffer = new byte[1024];
            int bytesRead = stream.Read(buffer, 0, buffer.Length);
            return Encoding.ASCII.GetString(buffer, 0, bytesRead);
        }
    }
}
