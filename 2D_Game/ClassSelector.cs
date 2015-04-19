using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace _2D_Game
{
    class ClassSelector
    {
        static Texture2D _fighter;
        static Texture2D _mage;
        static Texture2D _enemy;
        static Texture2D _boundingbox;
        static Texture2D _archer;
        private int _index;
        readonly List<Texture2D> _textures = new List<Texture2D>();
        int _choice = 4;
        readonly Keys _leftkey;
        readonly Keys _rightkey;
        readonly Keys _selectkey;
        public int Choice
        {
            get { return _choice; }
        }

        static public void Load_textures(Texture2D fighter, Texture2D mage,Texture2D archer, Texture2D enemy, Texture2D boundingbox)
        {
            _fighter = fighter;
            _mage = mage;
            _archer = archer;
            _enemy = enemy;
            _boundingbox = boundingbox;
        }
        public ClassSelector(Keys leftkey, Keys rightkey, Keys selectKey)
        {
            _leftkey = leftkey;
            _rightkey = rightkey;
            _selectkey = selectKey;
            _textures.Add(_fighter);
            _textures.Add(_mage);
            _textures.Add(_archer);
            _textures.Add(_enemy);
            _textures.Add(_boundingbox);
        }
        public void Update(KeyboardState curr, KeyboardState oldkey)
        {
            if (curr.IsKeyDown(_rightkey) && oldkey.IsKeyUp(_rightkey))
            {
                    _index++;
            }
            if (curr.IsKeyDown(_leftkey) && oldkey.IsKeyUp(_leftkey))
            {
                    _index--;
            }
            if (_index < 0)
                _index = _textures.Count - 1;
            if (_index >= _textures.Count)
                _index = 0;
            if (curr.IsKeyDown(_selectkey) && oldkey.IsKeyUp(_selectkey))
            {
                _choice = _index;
            }
        }

        public void Draw(SpriteBatch sb, SpriteFont f, int shift)
        {
            sb.DrawString(f, _index.ToString(), new Vector2((100 * shift) + 50, 200), Color.White);
            sb.Draw(_textures[_index], new Rectangle((100 * shift) + 50, 50, 50, 50), Color.White);
        }
    }
}