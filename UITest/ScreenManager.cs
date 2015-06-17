using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
namespace UITest
{
    public class ScreenManager
    {
        Dictionary<string,GameScreen> _screens;
        string _currScreen;

        public ScreenManager()
        {
            _screens = new Dictionary<string, GameScreen>();
        }

        public void addScreen(GameScreen screen)
        {
            _screens.Add(screen.ScreenName,screen);
            if (_screens.Count == 1)
                SwitchScreen(screen.ScreenName);
        }

        public void SwitchScreen(string pickedScreen)
        {
            if (_screens.Keys.Contains(pickedScreen))
                _currScreen = pickedScreen;
        }

        public void Update()
        {
            if (_currScreen != "")
            {
                _screens[_currScreen].Update(this);
            }
        }

        public void Draw(SpriteBatch sb, SpriteFont f)
        {
            if (_currScreen != "")
            {
                _screens[_currScreen].Draw(sb,f);
            }
        }
    }
}
