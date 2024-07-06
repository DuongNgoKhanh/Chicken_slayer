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
            label1 = new Label();
            label2 = new Label();
            username1 = new Label();
            username2 = new Label();
            my_panel = new Panel();
            enemy_panel = new Panel();
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
            chickenTm.Tick += chickenTm_Tick;
            // 
            // eggsTm
            // 
            eggsTm.Enabled = true;
            eggsTm.Tick += eggsTm_Tick;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(28, 9);
            label1.Name = "label1";
            label1.Size = new Size(150, 31);
            label1.TabIndex = 0;
            label1.Text = "Score: 0";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(630, 9);
            label2.Name = "label2";
            label2.Size = new Size(150, 31);
            label2.TabIndex = 3;
            label2.Text = "Score: 0";
            // 
            // username1
            // 
            username1.AutoSize = true;
            username1.Location = new Point(211, 700);
            username1.Name = "username1";
            username1.Size = new Size(167, 31);
            username1.TabIndex = 4;
            username1.Text = "Username1";
            // 
            // username2
            // 
            username2.AutoSize = true;
            username2.Location = new Point(843, 700);
            username2.Name = "username2";
            username2.Size = new Size(167, 31);
            username2.TabIndex = 5;
            username2.Text = "Username2";
            // 
            // my_panel
            // 
            my_panel.BackColor = SystemColors.ActiveCaption;
            my_panel.Location = new Point(28, 50);
            my_panel.Name = "my_panel";
            my_panel.Size = new Size(560, 638);
            my_panel.TabIndex = 6;
            // 
            // enemy_panel
            // 
            enemy_panel.BackColor = SystemColors.ActiveCaption;
            enemy_panel.Location = new Point(639, 50);
            enemy_panel.Name = "enemy_panel";
            enemy_panel.Size = new Size(560, 638);
            enemy_panel.TabIndex = 7;
            // 
            // GameOnline
            // 
            AutoScaleDimensions = new SizeF(17F, 31F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(1222, 756);
            Controls.Add(enemy_panel);
            Controls.Add(my_panel);
            Controls.Add(username2);
            Controls.Add(username1);
            Controls.Add(label2);
            Controls.Add(label1);
            Font = new Font("Courier New", 14F);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Margin = new Padding(5, 3, 5, 3);
            Name = "GameOnline";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "GameOnline";
            KeyDown += GameOnline_KeyDown;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private System.Windows.Forms.Timer bulletTm;
        private System.Windows.Forms.Timer chickenTm;
        private System.Windows.Forms.Timer eggsTm;
        private Label label1;
        private Label label2;
        private Label username1;
        private Label username2;
        private Panel my_panel;
        private Panel enemy_panel;
    }
}