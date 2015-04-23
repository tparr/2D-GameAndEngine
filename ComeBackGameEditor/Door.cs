﻿using Microsoft.Xna.Framework;

namespace ComeBackGameEditor
{
    public class Door : Selectable
    {
        public Door(Rectangle box, string level = "")
            : base(box)
        {
            LoadLevelName = level;
        }

        public string LoadLevelName;
    }
}