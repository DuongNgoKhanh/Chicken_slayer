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
    public partial class Che_do_choi : Form
    {
        private RoomManager roomManager = new RoomManager();
        public Che_do_choi()
        {
            InitializeComponent();
        }


        private void btn_back_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void btn_doi_khang_Click(object sender, EventArgs e)
        {
            this.Hide();
            Lobby waiting_Room = new Lobby();
            waiting_Room.ShowDialog();
            this.Show();
        }

        private void btn_choi_thuong_Click(object sender, EventArgs e)
        {
            this.Hide();
            Game game = new Game();
            game.Show();
        }
    }
}
