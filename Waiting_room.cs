using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            InitLobby();
        }

        private void btn_back_Click(object sender, EventArgs e)
        {
            this.Hide();
            Lobby lobby = new Lobby();
            lobby.Show();
        }

        private void InitLobby()
        {
            lobbyTimer = new System.Windows.Forms.Timer(); ;
            lobbyTimer.Interval = 1000;
            lobbyTimer.Tick += LobbyTimer_Tick;
            lobbyTimer.Start();
        }

        private void LobbyTimer_Tick(object sender, EventArgs e)
        {
            if (currentRoom != null)
            {
                if (isServer)
                {
                    if (currentRoom.Player1Ready && currentRoom.Player2Ready)
                    {
                        StartGame();
                    }
                }
                else
                {
                    if (currentRoom.Player1Ready && currentRoom.Player2Ready)
                    {
                        StartGame();
                    }
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

        private void Waiting_room_Load(object sender, EventArgs e)
        {
        }

        private void btn_sanSang_Click(object sender, EventArgs e)
        {
            if (isServer)
            {
                currentRoom.Player1Ready = !currentRoom.Player1Ready;
            }
            else
            {
                currentRoom.Player2Ready = !currentRoom.Player2Ready;
            }
            roomManager.GetRoom(currentRoom.RoomID).Player1Ready = currentRoom.Player1Ready;
            roomManager.GetRoom(currentRoom.RoomID).Player2Ready = currentRoom.Player2Ready;
            btn_sanSang.Text = (isServer && currentRoom.Player1Ready) || (!isServer && currentRoom.Player2Ready) ? "Huỷ sẵn sàng" : "Sẵn sàng";
        }

        private void txtb_soPhong_TextChanged(object sender, EventArgs e)
        {
        }


    }
}
