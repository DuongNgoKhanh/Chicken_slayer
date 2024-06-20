using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace offline
{
    public partial class Client : Form
    {
        TcpClient client;
        NetworkStream stream;
        Game myGame;
        Game enemyGame;
        bool newBullet = false;
        //bool newEgg;
        public Client()
        {
            InitializeComponent();
            myGame = new Game();
            enemyGame = new Game();
            myGame.InitializeGame(my_panel);
            enemyGame.InitializeGame(enemy_panel);
            //newEgg = myGame.newEgg;
            ConnectToServer();
            //ShowWaitingMessage(); // Hiển thị thông báo chờ
            Task.Run(() => ReceiveGameState());
        }

        private void ConnectToServer()
        {
            client = new TcpClient("127.0.0.1", 12345);
            stream = client.GetStream();
        }

        public void ShowWaitingMessage()
        {
            MessageBox.Show("Waiting for opponent...", "Waiting", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void SendGameState()
        {

            GameState gameState = new GameState
            {
                RocketPosition = new Position { X = myGame.Rocket.Left, Y = myGame.Rocket.Top },
                Bullets = new List<Position>(),
                Chickens = new List<Position>(),
                Eggs = new List<Position>(),
                Lives = myGame.Lives,
                NewEgg = null
            };

            if (myGame.Bullets.Count > 0 && newBullet)
            {
                var lastBullet = myGame.Bullets[myGame.Bullets.Count - 1];
                gameState.Bullets.Add(new Position { X = lastBullet.Left, Y = lastBullet.Top });
            }

            for (int i = 0; i < myGame._chickenRows; i++)
            {
                for (int j = 0; j < myGame._chickenCols; j++)
                {
                    if (myGame.Chickens[i, j] == null)
                    {
                        gameState.Chickens.Add(new Position { X = j * 100, Y = i * 100 });
                    }
                }
            }
            //MessageBox.Show(myGame._eggs.ToString());
            // Eggs
            if (myGame._eggs.Count > 0 && myGame.newEgg)
            {
                //MessageBox.Show("Có new egg");
                var lastEgg = myGame._eggs[myGame._eggs.Count - 1];
                gameState.Eggs.Add(new Position { X = lastEgg.Left, Y = lastEgg.Top });
                //MessageBox.Show("X: ", lastEgg.Left.ToString() + ", Y:" + lastEgg.Top.ToString());
                //gameState.NewEgg = new Position { X = lastEgg.Left, Y = lastEgg.Top };  
            }

            //send message
            string message = JsonConvert.SerializeObject(gameState);
            byte[] data = Encoding.ASCII.GetBytes(message);
            stream.Write(data, 0, data.Length);

            // Debug output
            //MessageBox.Show("GameState sent: " + message);
            newBullet = false;
            myGame.newEgg = false;
        }

        private void ReceiveGameState()
        {
            byte[] buffer = new byte[1024];
            int bytesRead;

            while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
            {
                string message = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                GameState gameState = JsonConvert.DeserializeObject<GameState>(message);

                // Debug output
                //MessageBox.Show("GameState received: " + message);

                this.Invoke(new Action(() =>
                {
                    UpdateEnemyGameState(gameState);
                }));
            }
        }

        private void UpdateEnemyGameState(GameState gameState)
        {
            //enemyGame.UpdateRocketPosition(gameState);
            //enemyGame.UpdateBullets(gameState, enemy_panel);
            //enemyGame.RemoveNullChickens(gameState, enemy_panel);
            enemyGame.UpdateGameState(gameState, enemy_panel);
            //Thêm egg mới vào enemy panel nếu có
            //if (gameState.NewEgg != null || gameState.Eggs.Count > 0)
            //{     
            //    enemyGame.AddEgg(new Position { X = gameState.NewEgg.X, Y = gameState.NewEgg.Y }, enemy_panel);
            //    //enemyGame.AddEgg(new Position { X = gameState.Eggs[gameState.Eggs.Count-1].X, Y = gameState.Eggs[gameState.Eggs.Count - 1].Y}, enemy_panel);
            //    MessageBox.Show("đã thêm trứng");
            //}
        }

        private void Game_KeyDown(object sender, KeyEventArgs e)
        {
            bool stateChanged = false;

            switch (e.KeyCode)
            {
                case Keys.Left:
                    myGame.Rocket.Left -= 10;
                    stateChanged = true;
                    break;
                case Keys.Right:
                    myGame.Rocket.Left += 10;
                    stateChanged = true;
                    break;
                case Keys.Up:
                    myGame.Rocket.Top -= 10;
                    stateChanged = true;
                    break;
                case Keys.Down:
                    myGame.Rocket.Top += 10;
                    stateChanged = true;
                    break;
                case Keys.Space:
                    myGame.LaunchBullet(my_panel);
                    stateChanged = true;
                    newBullet = true;
                    break;
            }
            

            if (stateChanged)
            {
                SendGameState();
            }
        }

        private void chickenTm_Tick(object sender, EventArgs e)
        {
            myGame.UpdateChickenState(my_panel);
            enemyGame.UpdateChickenState(enemy_panel);
            if (myGame.newEgg)
            {
                //MessageBox.Show("newwgg");
                SendGameState();
                myGame.newEgg = false;
            }
        }

        private void eggsTm_Tick(object sender, EventArgs e)
        {
            myGame.UpdateEggState(my_panel);
            enemyGame.UpdateEggState(enemy_panel);
        }

        private void bulletTm_Tick(object sender, EventArgs e)
        {
            if (myGame.UpdateBulletPositionsAndCheckCollisions(my_panel, label1) ||
                enemyGame.UpdateBulletPositionsAndCheckCollisions(enemy_panel, label2))
                endGame();
        }

        private void endGame()
        {
            bulletTm.Stop();
            eggsTm.Stop();
            chickenTm.Stop();

            Controls.Clear();

            Bitmap win = Properties.Resources.win;
            Bitmap lose = Properties.Resources.lose;
            int score1 = int.Parse(label1.Text.Split(" ")[1]);
            int score2 = int.Parse(label2.Text.Split(" ")[1]);

            if (score1 > score2)
            {
                myGame.EndGame(win, this);
            }
            else
            {
                myGame.EndGame(lose, this);
            }
        }
    }
}
