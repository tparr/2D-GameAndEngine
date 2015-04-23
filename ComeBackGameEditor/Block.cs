using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ComeBackGameEditor
{
    public class Block
    {
        public Vector2 BlockPosition { get; set; }

        public Texture2D BlockTexture { get; set; }

        public Block(Texture2D texture)
        {
            BlockTexture = texture;
        }

    }
}
