using Microsoft.Xna.Framework;

namespace ComeBackGameEditor
{
    public class Door
    {
        readonly Rectangle _testBox;
        public Rectangle TestBox
        { get { return _testBox; } }
        public Door(Rectangle box)
        {
            _testBox = box;
        }
    }
}
