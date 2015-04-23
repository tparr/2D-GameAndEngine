using Microsoft.Xna.Framework;

namespace ComeBackGameEditor
{
    public class Selectable
    {
        public Rectangle Bounds;
        public bool Selected;

        public Selectable(Rectangle rectangle)
        {
            Bounds = rectangle;
        }
    }
}