using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Chicken_slayer.Resources;
using Google.Cloud.Firestore;
using Microsoft.VisualBasic.ApplicationServices;
using static System.Windows.Forms.DataFormats;

namespace Chicken_slayer
{
    public partial class Log_in : Form
    {
        private FirestoreDb _db;

        public Log_in()
        {
            InitializeComponent();
        }

        private void btn_log_in_Click(object sender, EventArgs e)
        {
            string username = guna2TextBox1.Text.Trim();
            string password = guna2TextBox2.Text;

            var db = FirestoreHelper.Database;
            DocumentReference docRef = db.Collection("UserData").Document(username);
            UserData data = docRef.GetSnapshotAsync().Result.ConvertTo<UserData>();
            if (data != null)
            {
                if (password == Security.Decrypt(data.Password))
                {
                    this.Hide();
                    Che_do_choi newForm = new Che_do_choi();
                    newForm.Show();

                }
                else
                {
                    MessageBox.Show("Failed.");
                }
            }
            else
            {
                MessageBox.Show("Login failed.");
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Hide();
            Sign_up dangki = new Sign_up();
            dangki.Show();
            Close();
        }
    }
}
