using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _2D_Game
{
    public class Projectile
    {
        Texture2D _texture;
        
        int _realx;
        int _realy;
        bool _isactive;
        int _iX;
        int _iY;
        Rectangle _hitbox;
        int _cameraxp;
        int _camerayp;
        int _dir; //{down=0, right=1, left=2, up=3}
        float _timer;
        int _currentFramex;
        int _currentFramey;
        private const float Interval = 300f;
        Rectangle _sourcerect;
        //16,17
        public Texture2D Texture
        {
            get { return _texture; }
            set { _texture = value; }
        }

        public Rectangle Hitbox
        {
            get { return _hitbox; }
            set { _hitbox = value; }
        }

        public int RealX
        {
            get {return _realx;}
            set { _realx = value;}
        }

        public int RealY
        {
            get { return _realy; }
            set { _realy = value; }
        }

        public int X
        {
            get { return _iX; }
            set { _iX = value; }
        }

        public int Y
        {
            get { return _iY; }
            set { _iY = value; }
        }

        public bool IsActive
        {
            get { return _isactive; }
            set { _isactive = value; }
        }

        public Rectangle SourceRect
        {
            get { return _sourcerect; }
            set { _sourcerect = value; }
        }
        public Projectile(Texture2D texturez)
        {
            _texture = texturez;
            _iX = 0;
            _iY = 0;
            _isactive = false;
        }
        public Projectile()
        {
            _iX = 0;
            _iY = 0;
            _isactive = false;
        }

        public void Fire(int x, int y, int c)
        {
            _iX = x;
            _iY = y;
            _isactive = true;
            _dir = c;
            UpdateDirection(_dir);
        }
        public void Fire(int x, int y)
        {
            _iX = x;
            _iY = y;
            _isactive = true;
        }
        public void UpdateDirection(int c)
        {
            _currentFramey = c;
        }

        public void Update(int camerax, int cameray, GameTime gametime, bool paused)
        {
            if (_isactive)
            {
                    _iX += _realx;
                    _iY += _realy;
                
                    _cameraxp = camerax;
                    _camerayp = cameray;
                
                    _hitbox = new Rectangle(_iX, _iY, 25, 25);
                
                Animate(gametime);
                _sourcerect = new Rectangle(_currentFramex * 16,_currentFramey * 16,16,16);
                // If the bullet has moved off of the screen,
                // set it to inactive
                if ((_iX > _cameraxp + 800) || (_iX < _cameraxp)
                    || (_iY > _camerayp + 600) || (_iY < _camerayp))
                {
                    _isactive = false;
                }
            }
        }
        public void Xy(SpriteBatch sb, SpriteFont f)
        {
            sb.DrawString(f, _iX.ToString(), new Vector2(500, 300), Color.Blue);
            sb.DrawString(f, _iY.ToString(), new Vector2(550, 300), Color.Blue);
        }
        //down is 0
        //right is 1
        //left is 2
        //up is 3
        //ANIMATE RIGHT
        public void Animate(GameTime gameTime)
        {
            _timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (_timer > Interval)
            {
                _currentFramex++;

                if (_currentFramex > 1)
                {
                    _currentFramex = 0;
                }
                _timer = 0f;
            }
        }
        public void Draw(SpriteBatch sb, int camerax, int cameray)
        {
            _cameraxp = camerax;
            _camerayp = cameray;
            if (_isactive)
            {
                sb.Draw(_texture, new Rectangle(_iX - 13 - _cameraxp,_iY - 16 - _camerayp,25,25), Color.White);
            }
        }
    }
}