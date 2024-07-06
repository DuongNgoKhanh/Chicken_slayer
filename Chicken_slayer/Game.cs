using System.Diagnostics;
using System.Windows.Forms;
using Chicken_slayer;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;


namespace Chicken_slayer
{
    public class Game
    {
        // Rocket
        public Piece Rocket { get; private set; }

        // Bullets
        public List<Piece> Bullets { get; private set; }

        // Chickens
        public Piece[,] Chickens { get; private set; }
        public List<Bitmap> _chickenFrames;
        public int _chickenFrameIndex = 0;
        public int _chickenFrameDirection = 1;

        // Eggs
        public List<Piece> _eggs;
        public List<Bitmap> _brokenEggFrames;
        public List<Piece> Eggs { get; set; }
        public bool newEgg { get; set; } = false;

        // Hearts and Lives
        public List<Piece> _liveHearts;
        public int Lives { get; private set; } = 3;

        // Game Configurations
        public Random _rand;
        public int[] _topChicken;
        public int _chickenSpeed = 3;
        public int _leftMostChicken = 0;
        public int _chickenRows = 3;
        public int _chickenCols = 8;

        // Resources
        public Bitmap _mainChickenImage = Properties.Resources.bossRed;
        public Bitmap _mainBrokenEgg = Properties.Resources.eggBreak;

        // Score
        public int Score { get; private set; } = 0;

        // Bullet speed
        public const int BulletSpeed = 10;


        public Game()
        {
            Bullets = new List<Piece>();
            Chickens = new Piece[_chickenRows, _chickenCols];
            _chickenFrames = new List<Bitmap>();
            _brokenEggFrames = new List<Bitmap>();
            _rand = new Random();
            _eggs = new List<Piece>();
            _liveHearts = new List<Piece>();
            _topChicken = new int[_chickenRows];
            Eggs = new List<Piece>();
        }

        public void InitializeGame(Panel panel)
        {
            InitializeRocket(panel);
            InitializeChickens(panel);
            InitializeHearts(panel);
            LoadEggFrames();
        }

        public void InitializeRocket(Panel panel)
        {
            Rocket = new Piece(50, 50)
            {
                Left = panel.Width / 2 - 25,
                Top = panel.Height - 50,
                Image = Properties.Resources.spaceship4
            };
            panel.Controls.Add(Rocket);
        }

        public void InitializeChickens(Panel panel)
        {
            divideImageIntoFrames(_mainChickenImage, _chickenFrames, 10);
            CreateChickens();
            foreach (var chicken in Chickens)
            {
                if (chicken != null)
                {
                    panel.Controls.Add(chicken);
                }
            }
        }

        public void InitializeHearts(Panel panel)
        {
            Bitmap heart = Properties.Resources.heart;
            for (int i = 0; i < _chickenRows; i++)
            {
                var heartPiece = new Piece(25, 25)
                {
                    Image = heart,
                    Left = panel.Width - (3 - i) * 45
                };
                _liveHearts.Add(heartPiece);
                panel.Controls.Add(heartPiece);
            }
        }

        public void LoadEggFrames()
        {
            divideImageIntoFrames(_mainBrokenEgg, _brokenEggFrames, 8);
        }

        public void divideImageIntoFrames(Bitmap original, List<Bitmap> res, int n)
        {
            int w = original.Width / n;
            int h = original.Height;
            for (int i = 0; i < n; i++)
            {
                int s = w * i;
                Bitmap piece = new Bitmap(w, h);
                for (int j = 0; j < h; j++)
                {
                    for (int k = 0; k < w; k++)
                    {
                        piece.SetPixel(k, j, original.GetPixel(k + s, j));
                    }
                }
                res.Add(piece);
            }
        }

        public void CreateChickens()
        {
            Bitmap img = _chickenFrames[0];
            for (int i = 0; i < _chickenRows; i++)
            {
                _topChicken[i] = i * 50 + img.Height;
                for (int j = 0; j < _chickenCols; j++)
                {
                    Piece chicken = new Piece(img.Width / 2, img.Height / 2)
                    {
                        Image = img,
                        Left = j * 50,
                        Top = i * 50 + img.Height / 2
                    };
                    Chickens[i, j] = chicken;
                }
            }
        }

        public void LaunchBullet(Panel panel)
        {
            var bullet = new Piece(20, 20)
            {
                Left = Rocket.Left + Rocket.Width / 2 - 10,
                Top = Rocket.Top - 20,
                Image = Properties.Resources.bullet1
            };
            Bullets.Add(bullet);
            panel.Controls.Add(bullet);
        }

        public void UpdateGameState(GameState gameState, Panel panel)
        {
            UpdateRocketPosition(gameState);
            UpdateBullets(gameState, panel);
            RemoveNullChickens(gameState, panel);
            UpdateEggsPosition(gameState, panel);
            UpdateLives(gameState);
        }

        public void UpdateRocketPosition(GameState gameState)
        {
            if(gameState != null && gameState.RocketPosition != null)
            {
                Rocket.Left = gameState.RocketPosition.X;
                Rocket.Top = gameState.RocketPosition.Y;
            }
            
        }

        public void UpdateBullets(GameState gameState, Panel panel)
        {
            if(gameState != null && gameState.Bullets != null)
            {
                foreach (var bulletState in gameState.Bullets)
                {
                    var bullet = new Piece(20, 20)
                    {
                        Left = bulletState.X,
                        Top = bulletState.Y,
                        Image = Properties.Resources.bullet1
                    };
                    Bullets.Add(bullet);
                    panel.Controls.Add(bullet);
                }
            }    
            
        }

        public void RemoveNullChickens(GameState gameState, Panel panel)
        {
            if(gameState != null && gameState.Chickens != null)
            {
                foreach (var chickenState in gameState.Chickens)
                {
                    int row = chickenState.Y / 100;
                    int col = chickenState.X / 100;
                    if (Chickens[row, col] != null)
                    {
                        panel.Controls.Remove(Chickens[row, col]);
                        Chickens[row, col] = null;
                    }
                }
            }    
            
        }

        public void UpdateChickenState(Panel panel)
        {
            int chickenGap = 50 - _chickenFrames[0].Width / 2;
            int width = panel.Width - _chickenCols * (_chickenFrames[0].Width / 2 + chickenGap);

            if (_leftMostChicken >= width || _leftMostChicken < 0)
                _chickenSpeed = -_chickenSpeed;
            _leftMostChicken += _chickenSpeed;

            for (int i = 0; i < _chickenRows; i++)
            {
                for (int j = 0; j < _chickenCols; j++)
                {
                    if (Chickens[i, j] == null) continue;

                    Chickens[i, j].Left += _chickenSpeed;
                    Chickens[i, j].Image = _chickenFrames[_chickenFrameIndex];
                }
            }

            _chickenFrameIndex += _chickenFrameDirection;
            if (_chickenFrameIndex == _chickenFrames.Count - 1)
            {
                _chickenFrameDirection = -1;
            }
            else if (_chickenFrameIndex == 0)
            {
                _chickenFrameDirection = 1;
            }
        }

        public void UpdateEggsPosition(GameState gameState, Panel panel)
        {
            if (gameState != null && gameState.Eggs != null) 
            {
                foreach (var eggState in gameState.Eggs)
                {
                    var egg = new Piece(20, 20)
                    {
                        Left = eggState.X,
                        Top = eggState.Y,
                        Image = Properties.Resources.egg
                    };
                    _eggs.Add(egg);
                    panel.Controls.Add(egg);
                }
            }
            
        }

        public void UpdateLives(GameState gameState)
        {
            if (gameState != null && Lives > gameState.Lives && gameState.Lives!=null)
                DecreaseLive();
        }
        public void DecreaseLive()
        {
            Lives--;
            _liveHearts[Lives].Image = Properties.Resources.d_heart;
        }

        public bool HasLivesLeft()
        {
            return Lives > 0;
        }

        private Piece LaunchRandomEgg()
        {
            List<Piece> availableChickens = new List<Piece>();
            for (int i = 0; i < _chickenRows; i++)
            {
                for (int j = 0; j < _chickenCols; j++)
                {
                    if (Chickens[i, j] != null)
                    {
                        availableChickens.Add(Chickens[i, j]);
                    }
                }
            }

            Piece chicken = availableChickens[_rand.Next(availableChickens.Count)];
            Piece egg = new Piece(20, 20)
            {
                Image = Properties.Resources.egg,
                Left = chicken.Left + chicken.Width / 2 - 10,
                Top = chicken.Top + chicken.Height
            };
            _eggs.Add(egg);
            newEgg = true;
            return egg;
        }

        public bool UpdateBulletPositionsAndCheckCollisions(Panel panel, System.Windows.Forms.Label label)
        {
            // Danh sách tạm thời để lưu trữ các viên đạn cần xóa
            List<Piece> bulletsToRemove = new List<Piece>();
            List<Piece> chickensToRemove = new List<Piece>();

            // Cập nhật vị trí của đạn
            for (int i = Bullets.Count - 1; i >= 0; i--)
            {
                Bullets[i].Top -= BulletSpeed;

                // Kiểm tra va chạm với các con gà
                for (int row = 0; row < _chickenRows; row++)
                {
                    for (int col = 0; col < _chickenCols; col++)
                    {
                        Piece chicken = Chickens[row, col];
                        if (chicken != null && Bullets[i].Bounds.IntersectsWith(chicken.Bounds))
                        {
                            // Thêm đạn và con gà vào danh sách tạm thời để xóa sau
                            bulletsToRemove.Add(Bullets[i]);
                            chickensToRemove.Add(chicken);
                            Chickens[row, col] = null;

                            // Tăng điểm
                            Score += 10;
                            label.Text = "Score: " + Score.ToString();
                            break;
                        }
                    }
                }

                // Kiểm tra nếu đạn ra khỏi màn hình, thì thêm nó6 vào danh sách tạm thời để xóa
                if (Bullets[i].Top < 0)
                {
                    bulletsToRemove.Add(Bullets[i]);
                }
            }

            // Thực hiện xóa các viên đạn và con gà sau khi hoàn thành vòng lặp
            foreach (var bullet in bulletsToRemove)
            {
                panel.Controls.Remove(bullet);
                Bullets.Remove(bullet);
            }

            foreach (var chicken in chickensToRemove)
            {
                panel.Controls.Remove(chicken);
            }

            // Kiểm tra nếu đạt được điểm tối đa, kết thúc trò chơi
            int maxScore = _chickenCols * _chickenRows * 10;
            if (Score == maxScore)
            {
                return true;
            }
            else return false;

        }

        public void UpdateEggState(Panel panel)
        {
            int panelHeight = panel.Height;


            if (_rand.Next(200) == 5 && (panel.Name != "enemy_panel"))
            {
                panel.Controls.Add(LaunchRandomEgg());
                //newEgg = true;
            }

            for (int i = 0; i < _eggs.Count; i++)
            {
                _eggs[i].Top += _eggs[i].eggDownSpeed;

                if (Rocket.Bounds.IntersectsWith(_eggs[i].Bounds))
                {
                    panel.Controls.Remove(_eggs[i]);
                    _eggs.RemoveAt(i);
                    DecreaseLive();
                    break;
                }

                if (_eggs[i].Top >= panelHeight - (_eggs[i].Height + 20))
                {
                    _eggs[i].eggDownSpeed = 0;
                    if (_eggs[i].eggLandCount / 2 < _brokenEggFrames.Count)
                    {
                        _eggs[i].Image = _brokenEggFrames[_eggs[i].eggLandCount / 2];
                        _eggs[i].eggLandCount += 1;
                    }
                    else
                    {
                        panel.Controls.Remove(_eggs[i]);
                        _eggs.RemoveAt(i);
                    }
                }
            }
        }

        public void cls(object sender, EventArgs e, Form form)
        {
            form.Close();
            
        }

        public void EndGame(Bitmap img, Form form)
        {
            //clear
            form.Controls.Clear();

            // Tạo và cấu hình hình ảnh kết thúc trò chơi
            Piece endGameImage = new Piece(100, 100)
            {
                Image = img,
                Left = form.Width / 2 - 50,
                Top = form.Height / 2 - 50
            };

            // Thêm sự kiện Click để đóng Form khi người dùng nhấp vào hình ảnh kết thúc trò chơi
            endGameImage.Click += (sender, e) =>
            {
                if (img == Properties.Resources.win)
                {
                    BXHsaving bXHsaving = new BXHsaving();
                    bXHsaving.Show();
                }
                cls(sender, e, form);
            };


            // Thêm hình ảnh kết thúc trò chơi vào Form
            form.Controls.Add(endGameImage);
            
        }
    }

    public class GameState
    {
        public Position RocketPosition { get; set; }
        public List<Position> Bullets { get; set; }
        public List<Position> Chickens { get; set; }
        public List<Position> Eggs { get; set; }
        public int Lives { get; set; }
        public bool GameOver { get; set; }
        public bool IsWinner { get; set; }
    }

    public class Position
    {
        public int X { get; set; }
        public int Y { get; set; }
    }
}
