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
            this.Close();
        }

        private void btn_doi_khang_Click(object sender, EventArgs e)
        {
            if(ApplicationState.CurrentUsername!=null)
            {
                using (Lobby waiting_Room = new Lobby())
                {
                    this.Hide();  // Hide the main menu while Lobby is open
                    var result = waiting_Room.ShowDialog();
                    if (result == DialogResult.OK)
                    {
                        // User clicked back button in Lobby form
                        this.Show();
                    }
                }
            }    
            
            //this.Show();
        }

        private void btn_choi_thuong_Click(object sender, EventArgs e)
        {
            using (Offline game = new Offline())
            {
                this.Hide();  // Hide the main menu while Lobby is open
                var result = game.ShowDialog();
                if (result == DialogResult.OK)
                {
                    // User clicked back button in Lobby form
                    this.Show();
                }
            }
        }
    }
}
