using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _2D_Game
{
    public interface Collidable
    {
        bool Intersects(Circle circle);
        bool Intersects(RectangleF rect);
        bool Intersects(Rectangle rect);
        float X { get; }
        float Y { get; }
        float Width { get; }
        float Height { get; }
    }
}
