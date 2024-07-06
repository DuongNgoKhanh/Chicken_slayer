using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace Chicken_slayer
{
    public partial class Lobby : Form
    {
        private RoomManager roomManager = new RoomManager();
        TcpClient client = TcpConnectionManager.Instance.Client;
        NetworkStream stream = TcpConnectionManager.Instance.Stream;
        //private System.Threading.Timer checkOpponentTimer;

        public Lobby()
        {
            InitializeComponent();
            //TcpConnectionManager.Instance.ConnectToServer();
            //MessageBox.Show("Đã kết nối");

        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            // Tạo phòng trước
            string roomId = roomManager.CreateRoom(ApplicationState.CurrentUsername);

            // Tạo yêu cầu tạo phòng để gửi cho server
            Message createRoomRequest = new Message
            {
                Command = "CreateRoom",
                RoomID = roomId,
                Player1Name = ApplicationState.CurrentUsername,
                Player2Name = null
            };

            string requestJson = JsonConvert.SerializeObject(createRoomRequest);
            byte[] requestBytes = Encoding.ASCII.GetBytes(requestJson);
            string responseJson = SendToServer(requestBytes);
            Message response = JsonConvert.DeserializeObject<Message>(responseJson);
            if (response.Command == "RoomCreated")
            {
                MessageBox.Show("Đã tạo phòng");
                Waiting_room roomForm = new Waiting_room(roomManager, response.RoomID, true);
                roomForm.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Không thể tạo phòng trên server.");
            }
        }


        private void VaoPhong_Click(object sender, EventArgs e)
        {
            string roomId = guna2TextBox2.Text;

            Message joinRoomRequest = new Message
            {
                Command = "JoinRoom",
                RoomID = roomId,
                Player1Name = null,
                Player2Name = ApplicationState.CurrentUsername
            };

            string requestJson = JsonConvert.SerializeObject(joinRoomRequest);
            byte[] requestBytes = Encoding.ASCII.GetBytes(requestJson);
            string responseJson = SendToServer(requestBytes);
            Message response = JsonConvert.DeserializeObject<Message>(responseJson);

            if (response.Command == "RoomJoined")
            {
                Waiting_room waitingRoom = new Waiting_room(roomManager, roomId, false);
                //waitingRoom.UpdateRoomStatus();
                waitingRoom.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Phòng không tồn tại hoặc đã đủ người");
            }
        }

        private void btn_back_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void guna2TextBox2_Click(object sender, EventArgs e)
        {
            //guna2TextBox2.Text = "";
        }

        private string SendToServer(byte[] requestBytes)
        {
            // Thực hiện gửi yêu cầu tới server và nhận phản hồi
            //using (TcpClient client = new TcpClient("127.0.0.1", 12345))
            //{
            //NetworkStream stream = client.GetStream();
            stream.Write(requestBytes, 0, requestBytes.Length);

            byte[] buffer = new byte[1024];
            int bytesRead = stream.Read(buffer, 0, buffer.Length);
            return Encoding.ASCII.GetString(buffer, 0, bytesRead);
            //}
        }

        private void btn_ngaunhien_Click(object sender, EventArgs e)
        {
            lb_waiting.Text = "Waiting...";

            Message readyRequest = new Message
            {
                Command = "FindRandomOpponent",
                Player1Name = ApplicationState.CurrentUsername,
                myName = ApplicationState.CurrentUsername,
            };

            string requestJson = JsonConvert.SerializeObject(readyRequest);
            byte[] requestBytes = Encoding.ASCII.GetBytes(requestJson);
            string responseJson = SendToServer(requestBytes);
            Message response = JsonConvert.DeserializeObject<Message>(responseJson);

            //Thread waiting = new Thread(() => { CheckForOpponent(); });
            //waiting.Start();
            if (response.Command == "FoundOpponent")
            {
                ApplicationState.EnemyUsername = response.myName;
                Invoke(new Action(() =>
                {
                    this.Hide();
                    GameOnline gameForm = new GameOnline();
                    gameForm.Show();
                }));

            }
            else if (response.Command == "WaitingForOpponent")
            {
                lb_waiting.Text = "Waiting...";
                //StartCheckingForOpponent();
                Thread waiting = new Thread(() => { CheckForOpponent(); });
                waiting.Start();

            }
        }
        //private void StartCheckingForOpponent()
        //{
        //   // TimerCallback timerCallback = new TimerCallback(CheckForOpponent);
        //    checkOpponentTimer = new System.Threading.Timer(timerCallback, null, 0, 1); // Check every 5 seconds
        //}
        private volatile bool stopChecking = false;

        private void CheckForOpponent()
        {
            byte[] buffer = new byte[1024];
            int bytesRead;
            stopChecking = false;

            while (!stopChecking && (bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
            {
                string message = Encoding.ASCII.GetString(buffer, 0, bytesRead).Trim();
                Message response = JsonConvert.DeserializeObject<Message>(message);

                if (response.Command == "FoundOpponent")
                {
                    ApplicationState.EnemyUsername = response.myName;
                    Invoke(new Action(() =>
                    {
                        this.Hide();
                        GameOnline gameForm = new GameOnline();
                        gameForm.Show();
                        stopChecking = true;
                    }));
                    //checkOpponentTimer.Dispose();
                    
                    break;
                }
                //else if (response.Command == "WaitingForOpponent")
                //{
                //    lb_waiting.Text = "Waiting...";
                //    //StartCheckingForOpponent();
                //    //Thread waiting = new Thread(() => { CheckForOpponent(); });
                //    //waiting.Start();

                //}

            }
        }

    }
}
