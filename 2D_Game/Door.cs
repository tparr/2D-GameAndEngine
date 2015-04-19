using Microsoft.Xna.Framework;

namespace _2D_Game
{
    public class Door
    {
        readonly Rectangle _testBox;
        readonly Vector2 _destination;
        public string Filename;
        public Rectangle TestBox
        { get { return _testBox; } }
        public Vector2 Destination
        { get { return _destination; } }
        public Door(Rectangle box, Vector2 dest)
        {
            _testBox = box;
            _destination = dest;
        }
        public Door(string filenamez, Vector2 dest)
        {
            _destination = dest;
            Filename = filenamez;
        }
    }
}
