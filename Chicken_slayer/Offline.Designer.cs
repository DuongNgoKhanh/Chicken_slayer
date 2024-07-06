namespace Chicken_slayer
{
    partial class Offline
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
            label1 = new Label();
            bulletTm = new System.Windows.Forms.Timer(components);
            chickenTm = new System.Windows.Forms.Timer(components);
            eggsTm = new System.Windows.Forms.Timer(components);
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Courier New", 14F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = SystemColors.ActiveCaptionText;
            label1.Location = new Point(1, 9);
            label1.Name = "label1";
            label1.Size = new Size(124, 27);
            label1.TabIndex = 5;
            label1.Text = "Score: 0";
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
            // Game
            // 
            AutoScaleDimensions = new SizeF(14F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(1200, 700);
            Controls.Add(label1);
            Font = new Font("Courier New", 14F);
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(5, 3, 5, 3);
            Name = "Game";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Game";
            Load += Game_Load;
            KeyDown += Game_KeyDown;
            KeyUp += Game_KeyUp;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private System.Windows.Forms.Timer bulletTm;
        private System.Windows.Forms.Timer chickenTm;
        private System.Windows.Forms.Timer eggsTm;
    }
}