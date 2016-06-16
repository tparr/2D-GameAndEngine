using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _2D_Game
{
    public class HealthBar
    {
        private bool _enabled = true;

        public static Texture2D BackgroundImage;
        public static Texture2D healthImage;
        public static Texture2D magicImage;
        public static Texture2D experienceImage;

        public int PlayerIndexx = 0;

        Vector2 _backvect = new Vector2(0, 400);
        Vector2 _hpvect = new Vector2(7, 405);
        Vector2 _mgvect = new Vector2(7, 429);
        Vector2 _expvect = new Vector2(7, 453);

        double _healthwidth;
        double _magicwidth;
        double _expwidth;



        public void Enable(bool enabled)
        {
            _enabled = enabled;
        }

        public void Update(int health, int magic, int exp)
        {
            _healthwidth = health / 100.0;
            _magicwidth = magic / 100.0;
            _expwidth = exp / 100.0;
        }

        public void Draw(SpriteBatch sb, SpriteFont f, int i)
        {
            if (_enabled)
            {
                //Background
                sb.Draw(BackgroundImage, new Vector2(_backvect.X + (BackgroundImage.Width * i), _backvect.Y), Color.White);

                //Health Bar
                sb.Draw(healthImage,
                    new Rectangle((int)_hpvect.X + ((healthImage.Width + 15) * i), (int)_hpvect.Y, (int)(192 * _healthwidth), 12),
                    new Rectangle(0, 0, (int)(192 * _healthwidth) , 12), Color.White);

                //Magic Bar
                sb.Draw(magicImage,
                    new Rectangle((int)_mgvect.X + ((magicImage.Width + 15) * i), (int)_mgvect.Y, (int)(192 * _magicwidth), 12),
                     new Rectangle(0, 0, (int)(192 * _magicwidth), 12), Color.White);

                //Experience Bar
                sb.Draw(experienceImage,
                    new Rectangle((int)_expvect.X + ((experienceImage.Width + 15) * i), (int)_expvect.Y, (int)(192 * _expwidth), 12),
                    new Rectangle(0, 0, (int)(192 * _expwidth), 12), Color.White);
            }
        }
    }
}
