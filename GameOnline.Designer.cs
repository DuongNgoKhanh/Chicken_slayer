namespace Chicken_slayer
{
    partial class GameOnline
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            bulletTm = new System.Windows.Forms.Timer(components);
            chickenTm = new System.Windows.Forms.Timer(components);
            eggsTm = new System.Windows.Forms.Timer(components);
            SuspendLayout();
            // 
            // bulletTm
            // 
            bulletTm.Enabled = true;
            bulletTm.Tick += bulletTm_Tick;
            // 
            // chickenTm
            // 
            chickenTm.Enabled = true;
            chickenTm.Interval = 20;
            chickenTm.Tick += chickenTm_Tick;
            // 
            // eggsTm
            // 
            eggsTm.Enabled = true;
            eggsTm.Interval = 20;
            eggsTm.Tick += eggsTm_Tick;
            // 
            // GameOnline
            // 
            AutoScaleDimensions = new SizeF(14F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(1200, 700);
            Font = new Font("Courier New", 14F);
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(5, 3, 5, 3);
            Name = "GameOnline";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "GameOnline";
            Load += Game_Load;
            KeyDown += Game_KeyDown;
            KeyUp += Game_KeyUp;
            ResumeLayout(false);
        }

        #endregion
        private System.Windows.Forms.Timer bulletTm;
        private System.Windows.Forms.Timer chickenTm;
        private System.Windows.Forms.Timer eggsTm;
    }
}