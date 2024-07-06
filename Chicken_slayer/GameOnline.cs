using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Forms;
using Chicken_slayer;

namespace Chicken_slayer
{
    public partial class GameOnline : Form
    {
        TcpClient client = TcpConnectionManager.Instance.Client;
        NetworkStream stream = TcpConnectionManager.Instance.Stream;
        Game myGame;
        Game enemyGame;
        bool newBullet = false;
        bool gameOver = false;
        bool isWinner = false;

        public GameOnline()
        {
            InitializeComponent();
            myGame = new Game();
            enemyGame = new Game();
            myGame.InitializeGame(my_panel);
            enemyGame.InitializeGame(enemy_panel);
            username1.Text = ApplicationState.CurrentUsername;
            username2.Text = ApplicationState.EnemyUsername;
            this.Shown += GameOnline_Shown;

        }
        private void GameOnline_Shown(object sender, EventArgs e)
        {

            //if(stream != null) { MessageBox.Show(ApplicationState.CurrentUsername + "Has stream"); }
            Task.Run(() => ReceiveGameState());

        }

        public void SendGameState()
        {

            GameState gameState = new GameState
            {
                RocketPosition = new Position { X = myGame.Rocket.Left, Y = myGame.Rocket.Top },
                Bullets = new List<Position>(),
                Chickens = new List<Position>(),
                Eggs = new List<Position>(),
                Lives = myGame.Lives,
                GameOver = gameOver,
                IsWinner = isWinner
            };
            
            //Bullet
            if (myGame.Bullets.Count > 0 && newBullet)
            {
                var lastBullet = myGame.Bullets[myGame.Bullets.Count - 1];
                gameState.Bullets.Add(new Position { X = lastBullet.Left, Y = lastBullet.Top });
            }

            //Chickens
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

            // Eggs
            if (myGame._eggs.Count > 0 && myGame.newEgg)
            {
                var lastEgg = myGame._eggs[myGame._eggs.Count - 1];
                gameState.Eggs.Add(new Position { X = lastEgg.Left, Y = lastEgg.Top });
            }

            //send message
            Message m = new Message();
            m.GameState = gameState;
            string message = JsonConvert.SerializeObject(m);
            byte[] data = Encoding.ASCII.GetBytes(message);
            stream.Write(data, 0, data.Length);
            //MessageBox.Show(ApplicationState.CurrentUsername + "Send: " + message);
            newBullet = false;
            myGame.newEgg = false;
        }

        private void ReceiveGameState()
        {
            byte[] buffer = new byte[1024];
            int bytesRead;

            while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
            {
                string message = Encoding.ASCII.GetString(buffer, 0, bytesRead).Trim();
                Message message1 = JsonConvert.DeserializeObject<Message>(message);
                GameState gameState = message1.GameState;
                //gameState = JsonConvert.DeserializeObject<GameState>(message);
                //MessageBox.Show("from: " + ApplicationState.EnemyUsername + message);
                this.Invoke(new Action(() =>
                {
                    if (gameState !=null && gameState.GameOver)
                    {
                        if (gameState.IsWinner)
                        {
                            myGame.EndGame(Properties.Resources.win, this);

                        }
                        else
                        {
                            myGame.EndGame(Properties.Resources.lose, this);
                        }
                        stopTimer();
                    }
                    else
                    {
                        UpdateEnemyGameState(gameState);
                    }
                }));
            }
        }

        private void UpdateEnemyGameState(GameState gameState)
        {
            enemyGame.UpdateGameState(gameState, enemy_panel);
        }
        private void bulletTm_Tick(object sender, EventArgs e)
        {
            if (myGame.UpdateBulletPositionsAndCheckCollisions(my_panel, label1) ||
                enemyGame.UpdateBulletPositionsAndCheckCollisions(enemy_panel, label2))
                endGame();
        }

        private void chickenTm_Tick(object sender, EventArgs e)
        {
            myGame.UpdateChickenState(my_panel);
            enemyGame.UpdateChickenState(enemy_panel);
            if (myGame.newEgg)
            {
                SendGameState();
                myGame.newEgg = false;
            }
        }

        private void eggsTm_Tick(object sender, EventArgs e)
        {
            myGame.UpdateEggState(my_panel);
            enemyGame.UpdateEggState(enemy_panel);
            if (!myGame.HasLivesLeft())
            {
                // Thực hiện các hành động khi người chơi thua
                gameOver = true;
                isWinner = true;
                SendGameState();
                stopTimer();
                myGame.EndGame(Properties.Resources.lose, this);
            }
        }

        private void GameOnline_KeyDown(object sender, KeyEventArgs e)
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
        private void endGame()
        {
            stopTimer();
            //chuẩn bị 
            Bitmap win = Properties.Resources.win;
            Bitmap lose = Properties.Resources.lose;
            int score1 = int.Parse(label1.Text.Split(" ")[1]);
            int score2 = int.Parse(label2.Text.Split(" ")[1]);

            bool isWinner = score1 > score2;
            myGame.EndGame(isWinner ? win : lose, this);

            // Send game over message
            gameOver = true;
            //isWinner = true;
            SendGameState();
        }
        private void stopTimer()
        {
            //dừng các timer lại
            bulletTm.Stop();
            eggsTm.Stop();
            chickenTm.Stop();
        }
    }
}
