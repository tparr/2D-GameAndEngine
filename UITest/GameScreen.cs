using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
namespace UITest
{
    public class GameScreen
    {
        Texture2D _background;
        protected Dictionary<string,ClickableButton> _buttons;
        public string ScreenName;
        ContentManager _content;
        string currButton;
        KeyboardState currKeys;
        KeyboardState prevKeys;
        public GameScreen(string name, ContentManager manager)
        {
            _content = new ContentManager(manager.ServiceProvider);
            _content.RootDirectory = "Content";
            _background = _content.Load<Texture2D>(name);
            ScreenName = name;
        }

       protected void InitializeButtons()
        {
            currButton = _buttons.Keys.First();
            _buttons[currButton]._highlighted = true;
           foreach (var buttonkey in _buttons.Keys)
           {
               _buttons[buttonkey].LoadTexture(_content.Load<Texture2D>(_buttons[buttonkey].ButtonName));
           }
        }

        public void Update(ScreenManager manager)
        {
            currKeys = Keyboard.GetState();
            var mouse = Mouse.GetState();
            if (currKeys.GetPressedKeys().Length != 0)
            {
                if (currKeys.IsKeyDown(Keys.W) && prevKeys.IsKeyUp(Keys.W)
                    || currKeys.IsKeyDown(Keys.A) && prevKeys.IsKeyUp(Keys.A)
                    || currKeys.IsKeyDown(Keys.S) && prevKeys.IsKeyUp(Keys.S)
                    || currKeys.IsKeyDown(Keys.D) && prevKeys.IsKeyUp(Keys.D))
                {
                    string move = _buttons[currButton].MakeMove(currKeys);
                    if (move == "")
                        return;
                    _buttons[currButton]._highlighted = false;
                    currButton = move;
                    _buttons[currButton]._highlighted = true;
                }
                else if (currKeys.IsKeyDown(Keys.Space) && prevKeys.IsKeyUp(Keys.Space))
                {
                    manager.SwitchScreen(_buttons[currButton]._moveToScreen);
                }
            }
            if (mouse.LeftButton == ButtonState.Pressed)
            {
                foreach (var buttonkey in _buttons.Keys)
                {
                    if (_buttons[buttonkey].Rectangle.Contains(mouse.Position))
                         manager.SwitchScreen(_buttons[buttonkey]._moveToScreen);
                }
            }
            prevKeys = currKeys;
        }
        public void Draw(SpriteBatch sb, SpriteFont f)
        {
            sb.Begin();
            foreach (var button in _buttons.Keys)
            {
                _buttons[button].Draw(sb,f);
            }
            sb.End();
        }
    }
}