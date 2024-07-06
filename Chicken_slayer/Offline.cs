using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.DirectoryServices.ActiveDirectory;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chicken_slayer
{
    public partial class Offline : Form
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
        int score = 0;

        //eggs
        Bitmap _mainBrokenEgg = Properties.Resources.eggBreak;
        List<Bitmap> _brokenEggFrames = new List<Bitmap>();
        List<Piece> _eggs = new List<Piece>();

        Random rand = new Random();

        public Offline()
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
            if (score == 240) endGame(Properties.Resources.win);
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
                        score += 10;
                        label1.Text = "Score: " + score.ToString();
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
            if (live == 0) endGame(Properties.Resources.lose);
        }

        private void endGame(Bitmap img)
        {
            eggsTm.Stop();
            bulletTm.Stop();
            chickenTm.Stop();
            Controls.Clear();
            Piece pic = new Piece(100, 100);
            pic.Click += cls;
            pic.Image = img;
            pic.Left = Width / 2 - pic.Width / 2;
            pic.Top = Height / 2 - pic.Height / 2;
            Controls.Add(pic);
            ShowScoreForm();
        }

        private void ShowScoreForm()
        {
            BXHsaving scoreForm = new BXHsaving();
            scoreForm.SetScore(score);
            scoreForm.Show();
        }

        private void cls(object sender, EventArgs e)
        {
            Close();
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


    }
}
