using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using Chicken_slayer.Resources;
using Google.Cloud.Firestore;

namespace Chicken_slayer
{

    public partial class BXH : Form
    {
        FirestoreDb database;
        public BXH()
        {
            InitializeComponent();
            this.Load += new EventHandler(BXH_Load);
        }

        private void BXH_Load(object sender, EventArgs e)
        {
                string path = AppDomain.CurrentDomain.BaseDirectory + @"cloudfire.json";
                Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);
                database = FirestoreDb.Create("leaderboard-935a2");
        }


        private void btn_back_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }
        async void TaiLenBXH(string nameofCollection)
        {
            Query BXH = database.Collection(nameofCollection);
            QuerySnapshot snap = await BXH.GetSnapshotAsync();

            foreach (DocumentSnapshot docsnap in snap.Documents)
            {
                BXHstruct bxh = docsnap.ConvertTo<BXHstruct>();
                if (docsnap.Exists)
                {
                    guna2DataGridView1.Rows.Add(docsnap.Id, bxh.SCORE);
                }
            }
        }

        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btn_choi_ngay_Click(object sender, EventArgs e)
        {
            TaiLenBXH("BXH");
        }
    }
}
