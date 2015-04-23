using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace _2D_Game
{
    public class AnimationNew
    {
        public int CurrFrame;
        public int Frames;
        public List<Rectangle> Animations;
        public string AnimationName;
        public Vector2 PosAdjust;
        public bool Played;
        public List<RotatedRectangle> Colliders;
        public int Xoffset;
        public int Yoffset;
        public AnimationNew(string name, List<Rectangle> anims,Vector2 positionadjust,List<RotatedRectangle> hitboxesList,int xoffset,int yoffset)
        {
            AnimationName = name;
            Frames = anims.Count;
            Animations = new List<Rectangle>(anims);
            PosAdjust = positionadjust;
            Colliders = new List<RotatedRectangle>(hitboxesList);
            Xoffset = xoffset;
            Yoffset = yoffset;
        }
        public void Update()
        {
            if (CurrFrame + 1 >= Frames)
            {
                CurrFrame = 0;
                Played = true;
            }
            else
                CurrFrame++;
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
    }
}
