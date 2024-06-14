using Chicken_slayer.Resources;
using Google.Cloud.Firestore;
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
    public partial class BXHsaving : Form
    {
        FirestoreDb database;
        public BXHsaving()
        {
            InitializeComponent();
        }

        private BXHdata GetWriteData()
        {

            string name = guna2TextBox2.Text.Trim();
            string score = guna2TextBox1.Text.Trim();

            return new BXHdata()
            {
                NAME = name,
                SCORE = score
            };
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            var db = FirestoreHelper.Database;
            var data = GetWriteData();
            DocumentReference docRef = db.Collection("BXH").Document(data.NAME);
            docRef.SetAsync(data);
            MessageBox.Show("Success.");
        }

        private void guna2TextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2TextBox2_TextChanged_1(object sender, EventArgs e)
        {

        }
    }
}
