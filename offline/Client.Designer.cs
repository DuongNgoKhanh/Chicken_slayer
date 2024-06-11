namespace offline
{
    partial class Client
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            label1 = new Label();
            bulletTm = new System.Windows.Forms.Timer(components);
            chickenTm = new System.Windows.Forms.Timer(components);
            eggsTm = new System.Windows.Forms.Timer(components);
            my_panel = new Panel();
            enemy_panel = new Panel();
            label2 = new Label();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(28, 9);
            label1.Name = "label1";
            label1.Size = new Size(95, 25);
            label1.TabIndex = 0;
            label1.Text = "Score: 100";
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
            // my_panel
            // 
            my_panel.BackColor = SystemColors.ActiveCaption;
            my_panel.BorderStyle = BorderStyle.FixedSingle;
            my_panel.Location = new Point(28, 50);
            my_panel.Name = "my_panel";
            my_panel.Size = new Size(560, 638);
            my_panel.TabIndex = 1;
            // 
            // enemy_panel
            // 
            enemy_panel.Location = new Point(630, 50);
            enemy_panel.Name = "enemy_panel";
            enemy_panel.Size = new Size(558, 638);
            enemy_panel.TabIndex = 2;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(630, 9);
            label2.Name = "label2";
            label2.Size = new Size(95, 25);
            label2.TabIndex = 3;
            label2.Text = "Score: 100";
            // 
            // Client
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1200, 700);
            Controls.Add(label2);
            Controls.Add(enemy_panel);
            Controls.Add(my_panel);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Name = "Client";
            Text = "Form1";
            KeyDown += Game_KeyDown;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private System.Windows.Forms.Timer bulletTm;
        private System.Windows.Forms.Timer chickenTm;
        private System.Windows.Forms.Timer eggsTm;
        private Panel my_panel;
        private Panel enemy_panel;
        private Label label2;
    }
}
