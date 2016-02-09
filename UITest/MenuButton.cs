using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
namespace UITest
{
    public class ClickableButton
    {
        Texture2D _texture;
        public Rectangle Rectangle;
        public bool _highlighted;
        string _displayText;
        public string _moveToScreen;
        public string ButtonName
        {
            get { return _displayText; }
        }

        public ClickableButton(string moveToScreen,string displayText)
        {
            _moveToScreen = moveToScreen;
            _displayText = displayText;
            _highlighted = false;
            Rectangle = new Rectangle();
        }

        public void LoadTexture(Texture2D texture)
        {
            _texture = texture;
        }

        public void Draw(SpriteBatch sb,SpriteFont f)
        {
            if (_highlighted)
                sb.Draw(_texture, Rectangle, Color.Green);
            else
                sb.Draw(_texture, Rectangle, Color.WhiteSmoke);
            var centeredvect = CenterX(Rectangle,_displayText,f);
            sb.DrawString(f, _displayText, new Vector2(centeredvect.X,centeredvect.Y + 200), Color.Black);
        }
        private static Vector2 CenterText(Rectangle rect, string text,SpriteFont f)
        {
            var vect = new Vector2((rect.X + (rect.Width / 2f)) - (f.MeasureString(text).X / 2f), (rect.Y + (rect.Height / 2f)) - (f.MeasureString(text).Y / 2f));
            return vect;
        }
        private static Vector2 CenterX(Rectangle rect, string text, SpriteFont f)
        {
            var vect = new Vector2((rect.X + (rect.Width / 2f)) - (f.MeasureString(text).X / 2f), rect.Y);
            return vect;
        }
        private static Vector2 CenterY(Rectangle rect, string text, SpriteFont f)
        {
            var vect = new Vector2(rect.X, (rect.Y + (rect.Height / 2f)) - (f.MeasureString(text).Y / 2f));
            return vect;
        }
        //public string MakeMove(KeyboardState keypress)
        //{
        //    string direction = "";
        //    if (keypress.IsKeyDown(Keys.W) && keypress.IsKeyUp(Keys.W))
        //        direction = "up";
        //    else if (keypress.IsKeyDown(Keys.A) && keypress.IsKeyUp(Keys.A))
        //        direction = "left";
        //    else if (keypress.IsKeyDown(Keys.S) && keypress.IsKeyUp(Keys.S))
        //        direction = "down";
        //    else if (keypress.IsKeyDown(Keys.D) && keypress.IsKeyUp(Keys.D))
        //        direction = "right";
        //    if (_moves.ContainsKey(direction))
        //        return _moves[direction];
        //    else return "";
        //}
    }
}
