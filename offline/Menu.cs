using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace offline
{
    public partial class Menu : Form
    {
        public Menu()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Server server = new Server();
            server.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Client form1 = new Client();
            form1.Show();

            Client form2 = new Client();
            form2.Show();
            //form1.ShowWaitingMessage(); // Hiển thị thông báo chờ
        }
    }
}
