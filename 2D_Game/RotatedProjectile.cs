using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _2D_Game
{
    public class RotatedProjectile : Projectile
    {
        public float Rotation;
        public RotatedRectangle Rect;
        public Vector2 Direction;
        private readonly Texture2D _arrowtexture;
        public RotatedProjectile(RotatedRectangle rotatedrect, Texture2D texture): base(texture)
        {
            Rect = rotatedrect;
            _arrowtexture = texture;
        }
        public RotatedProjectile()
        {
            IsActive = false;
        }

        public void Fire(Vector2 directionz)
        {
            IsActive = true;
            Direction = directionz;
        }

        public void Update()
        {
            Rect.ChangePosition((int)(Direction.X + .5), (int)(Direction.Y + .5));
        }
        public void Draw(SpriteBatch sb)
        {
            sb.Draw(_arrowtexture, Rect.CollisionRectangle, null, Color.White, Rect.Rotation, Rect.Origin,SpriteEffects.None, 0f);
        }
    }
}
