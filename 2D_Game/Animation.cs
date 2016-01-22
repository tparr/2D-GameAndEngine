﻿using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace _2D_Game
{
    public class Animation
    {
        public int CurrFrame = 0;
        public int Frames;
        public List<Rectangle> Animations;
        public string AnimationName;
        public Vector2 PosAdjust;
        public bool Played;
        public List<RotatedRectangle> Colliders;
        public int Xoffset;
        public int Yoffset;
        public List<int> timers;
        private int CurrTimer;
        public RotatedRectangle ColliderRect
        {
            get { return CurrFrame < Colliders.Count ? Colliders[CurrFrame] : new RotatedRectangle(new Rectangle(0, 0, 0, 0), 0); }
        }
        public Rectangle AnimationRect
        {
            get { return Animations[CurrFrame]; }
        }
        public Animation(string name, List<Rectangle> anims,Vector2 positionadjust,List<RotatedRectangle> hitboxesList,int xoffset,int yoffset)
        {
            AnimationName = name;
            Frames = anims.Count;
            Animations = new List<Rectangle>(anims);
            PosAdjust = positionadjust;
            Colliders = new List<RotatedRectangle>(hitboxesList);
            Xoffset = xoffset;
            Yoffset = yoffset;
            CurrTimer = 0;
        }
        public void Update()
        {
            CurrTimer++;
            if (CurrTimer < timers[CurrFrame]) return;
            else if (CurrTimer >= timers[CurrFrame])
            {
                CurrFrame++;
                CurrTimer = 0;
            }
            else if (CurrFrame >= Frames)
            {
                CurrTimer = 0;
                CurrFrame = 0;
                Played = true;
            }
        }

        public void ReverseUpdate()
        {
            if (CurrFrame - 1 < 0)
                CurrFrame = Frames - 1;
            else
            {
                CurrFrame--;
            }
        }

        private int TimerMax
        {
            get { return timers[CurrFrame]; }
        }
    }
}
