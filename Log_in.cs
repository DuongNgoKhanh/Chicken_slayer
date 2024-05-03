using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Google.Cloud.Firestore;
using Microsoft.VisualBasic.ApplicationServices;
using static System.Windows.Forms.DataFormats;

namespace Chicken_slayer
{
    public partial class Log_in : Form
    {
        private FirestoreDb db;
        public Log_in()
        {
            InitializeComponent();
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", "D:\\New folder\\leaderboard-935a2-firebase-adminsdk-96hw7-da1c87d419.json"); //tải file json đổi đường dẫn thành đường dẫn file tải về
            db = FirestoreDb.Create("leaderboard-935a2");
        }


        private  void btn_back_Click(object sender, EventArgs e)
        {
            Menu back = new Menu();
            back.Show();
            this.Close();
        }

        private void Log_in_Load(object sender, EventArgs e)
        {

        }

        private async void btn_log_in_Click(object sender, EventArgs e)
        {
            string username = guna2TextBox1.Text;
            string password = guna2TextBox2.Text;

            // Kiểm tra đăng nhập
            bool isLoginSuccessful = await Login(username, password);

            if (isLoginSuccessful)
            {
                MessageBox.Show("Đăng nhập thành công.");
                OpenNewForm();
            }
            else
            {
                MessageBox.Show("Người dùng hoặc mật khẩu không chính xác.");
            }
        }
        private async Task<bool> Login(string username, string password)
        {
            try
            {
                DocumentReference docRef = db.Collection("Tài khoản").Document(username);
                DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();
                if (snapshot.Exists)
                {
                    string actualPassword = snapshot.GetValue<string>("password");
                    return actualPassword == password;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi đăng nhập: {ex.Message}");
                return false;
            }
        }

        private void OpenNewForm()
        {
            //this.Hide();
            //Che_do_choi Vao_Game = new Che_do_choi();
            //Vao_Game.FormClosed += (s, args) => this.Close();
            //Vao_Game.Show();

            Che_do_choi choi = new Che_do_choi();
            choi.Show(this);
            this.Hide();
        }

        private void Log_in_Load_1(object sender, EventArgs e)
        {

        }
    }
}
