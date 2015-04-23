using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _2D_Game
{
    public class HealthBar
    {
        private bool _enabled = true;

        private readonly Texture2D _backgroundImage;
        private readonly Texture2D _healthtex;
        private readonly Texture2D _magic;
        private readonly Texture2D _exptex;
        public int Px = 0;
        Vector2 _backvect = new Vector2(0, 400);
        Vector2 _hpvect = new Vector2(7, 405);
        Vector2 _mgvect = new Vector2(7, 429);
        Vector2 _expvect = new Vector2(7, 453);
        double _healthwidth;
        double _magicwidth;
        double _expwidth;
        public Texture2D BackgroundImage
        { get { return _backgroundImage; } }
        public Texture2D HealthTex
        { get { return _healthtex; } }
        public Texture2D MagicTex
        { get { return _magic; } }
        public Texture2D ExpTex
        { get { return _exptex; } }
        

        public HealthBar(Texture2D backgroundImage, Texture2D healthimage, Texture2D mpimage, Texture2D expTex)
        {
            _backgroundImage = backgroundImage;
            _healthtex = healthimage;
            _magic = mpimage;
            _exptex = expTex;
        }

        public HealthBar()
        {
        }

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
                sb.Draw(_backgroundImage, new Vector2(_backvect.X + (_backgroundImage.Width * i), _backvect.Y), Color.White);
                //HealthBAR
                sb.Draw(_healthtex,
                    new Rectangle((int)_hpvect.X + ((_healthtex.Width + 15) * i), (int)_hpvect.Y, (int)(192 * _healthwidth), 12),
                    new Rectangle(0, 0, (int)(192 * _healthwidth) , 12),
                    Color.White);
                //MAGICBAR
                sb.Draw(_magic,
                    new Rectangle((int)_mgvect.X + ((_magic.Width + 15) * i), (int)_mgvect.Y, (int)(192 * _magicwidth), 12),
                     new Rectangle(0, 0, (int)(192 * _magicwidth), 12),
                     Color.White);
                //EXPBAR
                sb.Draw(_exptex,
                    new Rectangle((int)_expvect.X + ((_exptex.Width + 15) * i), (int)_expvect.Y, (int)(192 * _expwidth), 12),
                    new Rectangle(0, 0, (int)(192 * _expwidth), 12),
                    Color.White);
            }
        }
    }
}
