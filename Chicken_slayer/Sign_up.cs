using Chicken_slayer.Resources;
using Google.Cloud.Firestore;
using Microsoft.VisualBasic.ApplicationServices;
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
        private FirestoreDb _db;
        public Sign_up()
        {
            InitializeComponent();
        }



        private UserData GetWriteData()
        {

            string username = guna2TextBox1.Text.Trim();
            string password = Security.Encrypt(guna2TextBox2.Text);

            return new UserData()
            {
                Username = username,
                Password = password
            };
        }
        private bool CheckIfUserAlreadyExist()
        {
            string username = guna2TextBox1.Text.Trim();
            string password = guna2TextBox2.Text;

            var db = FirestoreHelper.Database;
            DocumentReference docRef = db.Collection("UserData").Document(username);
            UserData data = docRef.GetSnapshotAsync().Result.ConvertTo<UserData>();
            if (data != null)
            {
                return true;
            }
            return false;
        }


        private void btn_sign_up_Click(object sender, EventArgs e)
        {
            var db = FirestoreHelper.Database;
            if (CheckIfUserAlreadyExist() == true)
            {
                MessageBox.Show("User already exist.");
                return;
            }
            var data = GetWriteData();
            DocumentReference docRef = db.Collection("UserData").Document(data.Username);
            docRef.SetAsync(data);
            MessageBox.Show("Success.");
        }

        private void btn_back_Click(object sender, EventArgs e)
        {
            Log_in Dang_nhap = new Log_in();
            this.Hide();
            Dang_nhap.Show();
        }
    }
}