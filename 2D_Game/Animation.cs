using Microsoft.Xna.Framework;
using System.Collections.Generic;

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
        public bool Paused;

        public List<Collidable> Colliders;

        public int Xoffset;

        public int Yoffset;

        public int[] timers;

        private int CurrTimer;

        public Collidable Collider
        {
            get { return CurrFrame < Colliders.Count ? Colliders[CurrFrame] : new RectangleF(new Rectangle(0, 0, 0, 0)); }
        }

        public Rectangle AnimationRect
        {
            get { return Animations[CurrFrame]; }
        }

        public Animation()
        {
            AnimationName = "";
            Animations = new List<Rectangle>() { Rectangle.Empty };
            Colliders = new List<Collidable>() { RectangleF.Empty };
            CurrTimer = 0;
            Frames = 1;
            Paused = false;
            Played = false;
            PosAdjust = Vector2.Zero;
            Xoffset = 0;
            Yoffset = 0;

            this.timers = new int[0];
        }

        public Animation(string name)
        {
            AnimationName = name;
            Animations = new List<Rectangle>() { Rectangle.Empty };
            Colliders = new List<Collidable>() { RectangleF.Empty };
            CurrTimer = 0;
            Frames = 1;
            Paused = false;
            Played = false;
            PosAdjust = Vector2.Zero;
            Xoffset = 0;
            Yoffset = 0;

            this.timers = new int[1] { 1 };
        }

        public Animation(string name, List<Rectangle> anims, Vector2 positionadjust, List<Collidable> hitboxesList, int[] timers, int xoffset, int yoffset)
        {
            AnimationName = name;
            Frames = anims.Count;
            Animations = new List<Rectangle>(anims);
            PosAdjust = positionadjust;
            Colliders = new List<Collidable>(hitboxesList);
            Xoffset = xoffset;
            Yoffset = yoffset;
            CurrTimer = 0;
            this.timers = new int[timers.Length];
            timers.CopyTo(this.timers, 0);
        }

        public void Update()
        {
            if (Paused) return;
            CurrTimer++;
            //If animation is still supposed to play. return.
            if (CurrTimer < timers[CurrFrame]) return;
            CurrFrame++;
            CurrTimer = 0;
            //Loop Frames
            if (CurrFrame >= Frames)
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

        public void IncreaseFrame()
        {
            if (CurrFrame + 1 >= Frames)
                CurrFrame = 0;
            else
                CurrFrame++;
            CurrTimer = 0;
        }

        public void DecreaseFrame()
        {
            if (CurrFrame <= 0)
                CurrFrame = Frames - 1;
            else
                CurrFrame--;
            CurrTimer = 0;
        }
    }
}