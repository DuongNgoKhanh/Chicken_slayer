using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Firebase.Database;
using Firebase.Database.Query;
using Microsoft.VisualBasic.ApplicationServices;
using static System.Windows.Forms.DataFormats;

namespace Chicken_slayer
{
    public partial class Log_in : Form
    {
        private readonly FirebaseClient firebase;
        public Log_in()
        {
            InitializeComponent();
            firebase = new FirebaseClient("https://project1-cb49f-default-rtdb.asia-southeast1.firebasedatabase.app/");
        }


        private void btn_back_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void Log_in_Load(object sender, EventArgs e)
        {

        }

        private async void btn_log_in_Click(object sender, EventArgs e)
        {
            string username = guna2TextBox1.Text;
            string password = guna2TextBox2.Text;
            var user = await GetUserAsync(username, password);
            if (user != null)
            {
                MessageBox.Show("Đăng nhập thành công!");
                OpenNewForm();
            }
            else
            {
                MessageBox.Show("Tài khoản hoặc mật khẩu không đúng!");
            }
        }
        private async Task<User> GetUserAsync(string username, string password)
        {
            var users = await firebase.Child("users").OnceAsync<User>();
            return users.Select(u => u.Object).FirstOrDefault(u => u.Username == username && u.Password == password);
        }
        public class User
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }

        private void OpenNewForm()
        {
            this.Hide(); 
            Che_do_choi Vao_Game = new Che_do_choi();
            Vao_Game.FormClosed += (s, args) => this.Close(); 
            Vao_Game.Show();
        }

    }
}
