using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System.IO;
namespace UITest
{
    public class GameScreen
    {
        Texture2D _background;
        protected List<ClickableButton> _buttons;
        public string ScreenName;
        ContentManager _content;
        int currButton = 0;
        KeyboardState currKeys;
        KeyboardState prevKeys;
        bool Animating = false;
        bool movingLeft = false;
        bool movingRight = false;
        Timer movingtimer;
        public GameScreen(string name, ContentManager manager)
        {
            _content = new ContentManager(manager.ServiceProvider);
            _content.RootDirectory = "Content";
            _background = _content.Load<Texture2D>(name);
            ScreenName = name;
            movingtimer = new Timer(10);
        }
        public void Initialize(ScreenManager manager)
        {
            InitializeButtons(manager);
        }
        protected void InitializeButtons(ScreenManager manager)
        {
            _buttons[currButton]._highlighted = true;
           foreach (var button in _buttons)
           {
               button.LoadTexture(_content.Load<Texture2D>(button.ButtonName));
           }
            foreach (var button in _buttons)
            {
                button.Rectangle = CenterRect(manager._screenrect);
            }
            for (int i = currButton + 1; i < _buttons.Count; i++)
            {
                _buttons[i].Rectangle.X = _buttons[currButton].Rectangle.X + (i * 200);
            }
        }

        public void Update(ScreenManager manager)
        {
            //throw new NotImplementedException();
            currKeys = Keyboard.GetState();
            var mouse = Mouse.GetState();
            if (!movingRight && !movingLeft)
            {
                if (currKeys.GetPressedKeys().Length != 0)
                {
                    if (currKeys.IsKeyDown(Keys.W) && prevKeys.IsKeyUp(Keys.W)
                        || currKeys.IsKeyDown(Keys.A) && prevKeys.IsKeyUp(Keys.A)
                        || currKeys.IsKeyDown(Keys.S) && prevKeys.IsKeyUp(Keys.S)
                        || currKeys.IsKeyDown(Keys.D) && prevKeys.IsKeyUp(Keys.D))
                    {
                        if (currKeys.IsKeyDown(Keys.A) && prevKeys.IsKeyUp(Keys.A))
                        {
                            if (currButton > 0)
                            {
                                _buttons[currButton]._highlighted = false;
                                currButton--;
                                _buttons[currButton]._highlighted = true;
                                movingRight = true;
                            }
                        }
                        if (currKeys.IsKeyDown(Keys.D) && prevKeys.IsKeyUp(Keys.D))
                        {
                            if (currButton < _buttons.Count - 1)
                            {
                                _buttons[currButton]._highlighted = false;
                                currButton++;
                                _buttons[currButton]._highlighted = true;
                                movingLeft = true;
                            }
                        }
                        //    string move = _buttons[currButton].MakeMove(currKeys);
                        //    if (move == "")
                        //        return;
                        //    _buttons[currButton]._highlighted = false;
                        //    currButton = move;
                        //    _buttons[currButton]._highlighted = true;
                    }
                    //else if (currKeys.IsKeyDown(Keys.Space) && prevKeys.IsKeyUp(Keys.Space))
                    //{
                    //    manager.SwitchScreen(_buttons[currButton]._moveToScreen);
                    //}
                }
                for (int i = 0; i < _buttons.Count; i++)
                {
                    if (MouseinTile(mouse,_buttons[i].Rectangle))
                    {
                        if (i < currButton)
                        {
                            _buttons[currButton]._highlighted = false;
                            currButton--;
                            _buttons[currButton]._highlighted = true;
                            movingRight = true;
                        }
                        if (i > currButton)
                        {
                            _buttons[currButton]._highlighted = false;
                            currButton++;
                            _buttons[currButton]._highlighted = true;
                            movingLeft = true;
                        }
                    }
                }
            }
            else
            {
                movingtimer.Update();
                if (movingLeft)
                    MoveLeft();
                if (movingRight)
                    MoveRight();
                if (movingtimer.Finished())
                {
                    movingLeft = false;
                    movingRight = false;
                }
            }
            //if (mouse.LeftButton == ButtonState.Pressed)
            //{
            //    foreach (var buttonkey in _buttons.Keys)
            //    {
            //        if (_buttons[buttonkey].Rectangle.Contains(mouse.Position))
            //             manager.SwitchScreen(_buttons[buttonkey]._moveToScreen);
            //    }
            //}
            prevKeys = currKeys;
        }
        private void MoveLeft()
        {
            foreach (var button in _buttons)
            {
                button.Rectangle.X -= 20;
            }
        }
        private void MoveRight()
        {
            foreach (var button in _buttons)
            {
                button.Rectangle.X += 20;
            }
        }
        private Rectangle CenterRect(Rectangle screenrect)
        {
            return new Rectangle(screenrect.X + (int)(screenrect.Width / 2f) - 200, screenrect.Y + (int)(screenrect.Height / 3f), 150,200);
        }

        public void Draw(SpriteBatch sb, SpriteFont f)
        {
            sb.Begin();
            sb.Draw(_background, Vector2.Zero, Color.White);
            foreach (var button in _buttons)
                button.Draw(sb, f);
            sb.DrawString(f, "CurrButton: " + currButton, new Vector2(100, 100), Color.Black);
            sb.End();
        }
        public bool MouseinTile(MouseState mouse, Rectangle rect)
        {
            if (mouse.X <= rect.Left || mouse.X >= rect.Right) return false;
            return mouse.Y < rect.Bottom && mouse.Y > rect.Top;
        }

        public bool MouseinTile(Vector2 vect, Rectangle rect)
        {
            if (!(vect.X > rect.Left) || !(vect.X < rect.Right)) return false;
            return vect.Y < rect.Bottom && vect.Y > rect.Top;
        }
    }
}