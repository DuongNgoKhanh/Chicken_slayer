using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Chicken_slayer
{
    public partial class GameOnline : Form
    {
        Piece _rocket;

        //bullet
        int _speed = 10;
        List<Piece> _bullets = new List<Piece>();

        //chickens
        Bitmap _mainChickenImage = Properties.Resources.bossRed;
        List<Bitmap> _chickenFrame = new List<Bitmap>();
        Piece[,] _chickens = new Piece[3, 8];
        int[] _topChicken = new int[3];
        int chickenSpeed = 1;
        int leftMostChicken = 0;
        int count = 0;
        int dt = 1;

        //heart
        List<Piece> _liveHeart = new List<Piece>();
        int live = 3;
        int score1 = 0;
        int score2 = 0;
        bool isPlayer1Turn = true;

        //labels for scores
        Label labelPlayer1Score;
        Label labelPlayer2Score;

        //eggs
        Bitmap _mainBrokenEgg = Properties.Resources.eggBreak;
        List<Bitmap> _brokenEggFrames = new List<Bitmap>();
        List<Piece> _eggs = new List<Piece>();

        Random rand = new Random();

        public GameOnline()
        {
            InitializeComponent();
            initial();
        }

        private void initial()
        {
            _rocket = new Piece(100, 100);
            _rocket.Left = Width / 2 - _rocket.Width / 2;
            _rocket.Top = Height - _rocket.Height;
            _rocket.Image = Properties.Resources.spaceship4;
            Controls.Add(_rocket);

            // Initialize labels for scores
            labelPlayer1Score = new Label();
            labelPlayer1Score.Text = "Player 1 Score: 0";
            labelPlayer1Score.Location = new Point(10, 10);
            labelPlayer1Score.AutoSize = true;
            Controls.Add(labelPlayer1Score);

            labelPlayer2Score = new Label();
            labelPlayer2Score.Text = "Player 2 Score: 0";
            labelPlayer2Score.Location = new Point(10, 30);
            labelPlayer2Score.AutoSize = true;
            Controls.Add(labelPlayer2Score);

            divideImageIntoFrames(_mainChickenImage, _chickenFrame, 10);
            createChickens();
            createHeart();
            divideImageIntoFrames(_mainBrokenEgg, _brokenEggFrames, 8);
        }

        private void createChickens()
        {
            Bitmap img = _chickenFrame[0];
            for (int i = 0; i < 3; i++)
            {
                _topChicken[i] = i * 100 + img.Height;
                for (int j = 0; j < 8; j++)
                {
                    Piece chicken = new Piece(img.Width, img.Height);
                    chicken.Image = img;
                    chicken.Left = j * 100;
                    chicken.Top = i * 100 + img.Height;
                    _chickens[i, j] = chicken;
                    Controls.Add(chicken);
                }
            }
        }

        private void divideImageIntoFrames(Bitmap original, List<Bitmap> res, int n)
        {
            int w = original.Width / n;
            int h = original.Height;
            for (int i = 0; i < n; i++)
            {
                int s = w * i;
                Bitmap piece = new Bitmap(w, h);
                for (int j = 0; j < h; j++)
                    for (int k = 0; k < w; k++)
                        piece.SetPixel(k, j, original.GetPixel(k + s, j));
                res.Add(piece);
            }
        }

        private void Game_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Left:
                    _rocket.Left -= _speed;
                    break;
                case Keys.Right:
                    _rocket.Left += _speed;
                    break;
                case Keys.Up:
                    _rocket.Top -= _speed;
                    break;
                case Keys.Down:
                    _rocket.Top += _speed;
                    break;
            }
        }

        private void Game_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space) launchBullet();
        }

        private void launchBullet()
        {
            Piece bullet = new Piece(20, 20);
            bullet.Left = _rocket.Left + _rocket.Width / 2 - bullet.Width / 2;
            bullet.Top = _rocket.Top - bullet.Height;
            bullet.Image = Properties.Resources.bullet1;
            _bullets.Add(bullet);
            Controls.Add(bullet);
        }

        private void bulletTm_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < _bullets.Count; i++)
            {
                _bullets[i].Top -= 10;
            }

            collision();
            if (score1 == 240 || score2 == 240) endGame();
        }

        private void collision()
        {
            for (int i = 0; i < _topChicken.Length; i++)
            {
                int lo = 0, hi = _bullets.Count - 2, md, ans = -1;
                while (lo <= hi)
                {
                    md = lo + (hi - lo) / 2;
                    if (_bullets[md].Top >= _topChicken[i])
                    {
                        hi = md - 1;
                        ans = md;
                    }
                    else
                        lo = md + 1;
                }

                if (ans != -1 && _bullets[ans].Top >= _topChicken[i]
                    && _bullets[ans].Top <= _topChicken[i] + _chickenFrame[0].Height)
                {
                    int j = (_bullets[ans].Left + 9 - leftMostChicken) / 100;
                    if (j >= 0 && j < 8 && _chickens[i, j] != null
                        && _bullets[ans].Bounds.IntersectsWith(_chickens[i, j].Bounds))
                    {
                        Controls.Remove(_bullets[ans]);
                        _bullets.RemoveAt(ans);
                        Controls.Remove(_chickens[i, j]);
                        _chickens[i, j] = null;
                        if (isPlayer1Turn)
                        {
                            score1 += 10;
                            labelPlayer1Score.Text = "Player 1 Score: " + score1.ToString();
                        }
                        else
                        {
                            score2 += 10;
                            labelPlayer2Score.Text = "Player 2 Score: " + score2.ToString();
                        }
                    }
                }
            }
        }

        private void chickenTm_Tick(object sender, EventArgs e)
        {
            if (leftMostChicken + 800 > Width || leftMostChicken < 0)
                chickenSpeed = -chickenSpeed;
            leftMostChicken += chickenSpeed;
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 8; j++)
                {
                    if (_chickens[i, j] == null) continue;
                    _chickens[i, j].Image = _chickenFrame[count];
                    _chickens[i, j].Left += chickenSpeed;
                }
            count = count + dt;
            if (count == 9)
            {
                dt = -1;
            }
            else if (count == 0)
            {
                dt = 1;
            }
        }

        private void createHeart()
        {
            Bitmap heart = Properties.Resources.heart;
            for (int i = 0; i < 3; i++)
            {
                _liveHeart.Add(new Piece(50, 50));
                _liveHeart[i].Image = heart;
                _liveHeart[i].Left = Width - (3 - i) * 45;
                Controls.Add(_liveHeart[i]);
            }
        }

        private void eggsTm_Tick(object sender, EventArgs e)
        {
            if (rand.Next(200) == 5) launchRandomEgg();
            for (int i = 0; i < _eggs.Count; i++)
            {
                _eggs[i].Top += _eggs[i].eggDownSpeed;
                if (_rocket.Bounds.IntersectsWith(_eggs[i].Bounds))
                {
                    Controls.Remove(_eggs[i]);
                    _eggs.RemoveAt(i);
                    decreaseLive();
                    break;
                }
                if (_eggs[i].Top >= Height - (_eggs[i].Height + 20))
                {
                    _eggs[i].eggDownSpeed = 0;
                    if (_eggs[i].eggLandCount / 2 < _brokenEggFrames.Count)
                    {
                        _eggs[i].Image =
                            _brokenEggFrames[_eggs[i].eggLandCount / 2];
                        _eggs[i].eggLandCount += 1;
                    }
                    else
                    {
                        Controls.Remove(_eggs[i]);
                        _eggs.RemoveAt(i);
                    }
                }
            }

        }

        private void decreaseLive()
        {
            live--;
            _liveHeart[live].Image = Properties.Resources.d_heart;
            if (live == 0) endGame();
        }

        private void endGame()
        {
            eggsTm.Stop();
            bulletTm.Stop();
            chickenTm.Stop();
            Controls.Clear();
            string resultMessage;

            if (score1 > score2)
            {
                resultMessage = "Player 1 Wins!";
            }
            else if (score2 > score1)
            {
                resultMessage = "Player 2 Wins!";
            }
            else
            {
                resultMessage = "It's a Tie!";
            }

            Label resultLabel = new Label();
            resultLabel.Text = resultMessage;
            resultLabel.Font = new Font("Arial", 24, FontStyle.Bold);
            resultLabel.AutoSize = true;
            resultLabel.Left = Width / 2 - resultLabel.Width / 2;
            resultLabel.Top = Height / 2 - resultLabel.Height / 2;
            Controls.Add(resultLabel);

            Button closeButton = new Button();
            closeButton.Text = "Close";
            closeButton.AutoSize = true;
            closeButton.Left = Width / 2 - closeButton.Width / 2;
            closeButton.Top = resultLabel.Top + resultLabel.Height + 20;
            closeButton.Click += (sender, e) => { Close(); };
            Controls.Add(closeButton);
        }

        private void launchRandomEgg()
        {
            List<Piece> availablesChickens = new List<Piece>();
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 8; j++)
                    if (_chickens[i, j] != null)
                        availablesChickens.Add(_chickens[i, j]);

            Piece chicken =
                availablesChickens[rand.Next() % availablesChickens.Count];
            Piece egg = new Piece(20, 20);
            egg.Image = Properties.Resources.egg;
            egg.Left = chicken.Left + chicken.Width / 2 - egg.Width;
            egg.Top = chicken.Top + chicken.Height;
            _eggs.Add(egg);
            Controls.Add(egg);
        }

        private void Game_Load(object sender, EventArgs e)
        {

        }

        private void GameOnline_Load(object sender, EventArgs e)
        {

        }
    }
}
