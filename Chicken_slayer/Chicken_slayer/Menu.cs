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
            this.Hide();
            Che_do_choi che_Do_Choi = new Che_do_choi();
            che_Do_Choi.ShowDialog();
            this.Show();
        }

        private void btn_dang_nhap_Click(object sender, EventArgs e)
        {
            this.Hide();
            Log_in sign_in = new Log_in();
            sign_in.ShowDialog();
            this.Show();
        }

        private void btn_xep_hang_Click(object sender, EventArgs e)
        {
            this.Hide();
            BXH bXH = new BXH();
            bXH.ShowDialog();
            this.Show();
            
        }

    }
}
