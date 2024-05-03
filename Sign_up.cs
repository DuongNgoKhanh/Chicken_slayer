using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chicken_slayer
{
    public partial class Sign_up : Form
    {
        private FirestoreDb db;
        public Sign_up()
        {
            InitializeComponent();
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", "D:\\New folder\\leaderboard-935a2-firebase-adminsdk-96hw7-da1c87d419.json");
            db = FirestoreDb.Create("leaderboard-935a2");
        }

        private async void btn_sign_up_Click(object sender, EventArgs e)
        {
            string username = guna2TextBox1.Text;
            string password = guna2TextBox2.Text;
            string confirmPassword = guna2TextBox3.Text;
            bool isUsernameExists = await IsUsernameExists(username);

            if (isUsernameExists)
            {
                MessageBox.Show("Username already exists.");
                return;
            }
            await CreateAccount(username, password);
            MessageBox.Show("Account created successfully.");
        }


        private async Task<bool> IsUsernameExists(string username)
        {
            try
            {

                DocumentReference docRef = db.Collection("Tài khoản").Document(username);
                DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();
                return snapshot.Exists;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error checking username: {ex.Message}");
                return true;
            }
        }

        private async Task CreateAccount(string username, string password)
        {
            try
            {
                DocumentReference docRef = db.Collection("Tài khoản").Document(username);
                Dictionary<string, object> data = new Dictionary<string, object>
                {
                    { "password", password }
                };
                await docRef.SetAsync(data);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error creating account: {ex.Message}");
            }
        }

        private void btn_back_Click(object sender, EventArgs e)
        {
            Log_in dangnhap = new Log_in();
            dangnhap.Show();
            this.Close();
        }
    }
}
