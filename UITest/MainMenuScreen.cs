using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
namespace UITest
{
    public class MainMenuScreen : GameScreen
    {
        public MainMenuScreen(ContentManager content) : base("MainMenu",content)
        {
            _buttons = new Dictionary<string, ClickableButton>();
            _buttons.Add("StartGame"
                , new ClickableButton("Playing", "StartGame"
                , new Rectangle(50, 15, 100, 20)
                , new Dictionary<string, string>() { }));
            _buttons.Add("OtherMenu"
                , new ClickableButton("OtherMenu", "OtherMenu"
                , new Rectangle(50, 55, 100, 20)
                , new Dictionary<string, string>() { }));
            _buttons.Add("Quit"
                , new ClickableButton("Quit", "Quit"
                , new Rectangle(50, 95, 100, 20)
                , new Dictionary<string, string>() { }));
            InitializeButtons();
        }
    }
}
