namespace Chicken_slayer
{
    public partial class Menu : Form
    {
        public Menu()
        {
            InitializeComponent();
        }

        private void btn_choi_ngay_Click(object sender, EventArgs e)
        {
            using (Che_do_choi che_Do_Choi = new Che_do_choi())
            {
                this.Hide();  // Hide the main menu while Lobby is open
                var result = che_Do_Choi.ShowDialog();
                if (result == DialogResult.OK)
                {
                    // User clicked back button in Lobby form
                    this.Show();
                }
            }
        }

        private void btn_dang_nhap_Click(object sender, EventArgs e)
        {
            using (Log_in sign_in = new Log_in())
            {
                this.Hide();  // Hide the main menu while Lobby is open
                var result = sign_in.ShowDialog();
                if (result == DialogResult.OK)
                {
                    // User clicked back button in Lobby form
                    this.Show();
                }
            }
        }

        private void btn_xep_hang_Click(object sender, EventArgs e)
        {
            using (BXH bXH = new BXH())
            {
                this.Hide();  // Hide the main menu while Lobby is open
                var result = bXH.ShowDialog();
                if (result == DialogResult.OK)
                {
                    // User clicked back button in Lobby form
                    this.Show();
                }
            }
        }

    }
}
