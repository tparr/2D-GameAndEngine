using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace _2D_Game
{
    public class Animation
    {
        readonly Texture2D _texture;
        public int Frames;
        public int Xoffset;
        public int Yoffset;
        public int Spritewidth;
        public int Spriteheight;
        public List<RotatedRectangle> Collisions;
        public Texture2D Texture
        {
            get { return _texture; }
        }
        public Animation(ContentManager loader, string filename,int xset,int yset,int framesx,int swidth,int sheight,List<RotatedRectangle> cols)
        {
            _texture = loader.Load<Texture2D>(filename);
            Xoffset = xset;
            Yoffset = yset;
            Frames = framesx;
            Spritewidth = swidth;
            Spriteheight = sheight;
            Collisions = cols;
        }
        //Changes to sourcerect
        public Rectangle AnimateRect(Rectangle sourcerect)
        {
            sourcerect.X += Spritewidth;
            return sourcerect;
        }
    }
}