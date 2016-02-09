﻿using Microsoft.Xna.Framework;

namespace _2D_Game
{
    public class Door : Thing
    {
        public Rectangle Rect;
        public string LoadLevelName;

        public Door(Rectangle box, string level = "")
        {
            Rect = box;
            LoadLevelName = level;
            base.Feetrect = new RectangleF(box);
        }
    }
}