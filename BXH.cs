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

namespace Chicken_slayer
{
    
    public partial class BXH : Form
    {
        private FirestoreDb db;
        public BXH()
        {
            InitializeComponent();
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", "D:\\New folder\\leaderboard-935a2-firebase-adminsdk-96hw7-da1c87d419.json");
            db = FirestoreDb.Create("leaderboard-935a2");
        }

        private async void BXH_Load(object sender, EventArgs e)
        {
            await LoadBXH();
        }



        private void btn_back_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }
        private async Task LoadBXH()
        {
            try
            {
                QuerySnapshot querySnapshot = await db.Collection("BXH").GetSnapshotAsync();
                guna2DataGridView1.Rows.Clear();
                foreach (DocumentSnapshot documentSnapshot in querySnapshot.Documents)
                {
                    if (documentSnapshot.Exists)
                    {
                        string name = documentSnapshot.GetValue<string>("Name");
                        int score = documentSnapshot.GetValue<int>("Score");
                        guna2DataGridView1.Rows.Add(name, score);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading BXH: {ex.Message}");
            }
        }
    }
}
