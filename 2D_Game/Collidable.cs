using Microsoft.Xna.Framework;

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

        Rectangle ToRectangle();
    }
}