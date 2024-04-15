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
            Waiting_room waiting_Room = new Waiting_room();
            waiting_Room.ShowDialog();
            this.Show();
        }
    }
}
