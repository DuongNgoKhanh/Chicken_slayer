using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chicken_slayer
{
    public partial class Lobby : Form
    {
        private RoomManager roomManager = new RoomManager();
        public Lobby()
        {
            InitializeComponent();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            string roomId = roomManager.CreateRoom();
            Waiting_room roomForm = new Waiting_room(roomManager, roomId, true);
            roomForm.Show();
        }

        private void VaoPhong_Click(object sender, EventArgs e)
        {
            string roomId = guna2TextBox2.Text;
            Room room = roomManager.JoinRoom(roomId);
            if (room != null)
            {
                Waiting_room roomForm = new Waiting_room(roomManager, roomId, false);
                roomForm.Show();
            }
            else
            {
                MessageBox.Show("Phòng không tồn tại hoặc đã đủ người");
            }
        }

        private void btn_back_Click(object sender, EventArgs e)
        {
            this.Hide();
            Che_do_choi che_Do_Choi = new Che_do_choi();
            che_Do_Choi.Show();
        }
    }
}
