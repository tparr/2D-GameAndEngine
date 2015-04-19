using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ComeBackGameEditor
{
    class Button
    {
        Texture2D _texture;

        public Button(Rectangle rect, Texture2D texture,int sourceX)
        {
            Rect = rect;
            _texture = texture;
            SourceX = sourceX;
        }
        public Rectangle Rect { get; private set; }
        public int SourceX { get; set; }

        public string Text { get; set; }
    }
}
