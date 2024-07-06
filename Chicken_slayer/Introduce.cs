using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chicken_slayer
{
    public partial class Introduce : Form
    {
        private serrver server;
        bool isServerClicked = false;
        public Introduce()
        {
            InitializeComponent();
            
        }

        private void btn_play_Click(object sender, EventArgs e)
        {
            Menu menu = new Menu();
            menu.Show(this);
            this.Hide();

        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            Menu menu = new Menu();
            menu.Show(this);
            this.Hide();
            if (!isServerClicked)
                ApplicationState.CurrentUsername = "duongngo";

        }
        private void guna2Button2_Click(object sender, EventArgs e)
        {
            server = new serrver();
            server.Show();
            ApplicationState.CurrentUsername = "duong";
            isServerClicked = true;
        }
    }
}
