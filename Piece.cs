﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chicken_slayer
{
    class Piece: PictureBox
    {
        public int eggLandCount = 0, eggDownSpeed = 2;

        public Piece(int width, int height)
        {
            this.Width = width;
            this.Height = height;
            this.SizeMode = PictureBoxSizeMode.StretchImage;
            this.BackColor = Color.Transparent;
        }
    }
}
