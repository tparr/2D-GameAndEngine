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
            _buttons = new List<ClickableButton>();
            _buttons.Add(new ClickableButton("Playing", "StartGame"));
            _buttons.Add(new ClickableButton("OtherMenu", "OtherMenu"));
            _buttons.Add(new ClickableButton("Quit", "Quit"));
            _buttons.Add(new ClickableButton("btn1", "OtherMenu"));
            _buttons.Add(new ClickableButton("btn2", "OtherMenu"));
            _buttons.Add(new ClickableButton("btn3", "OtherMenu"));
            _buttons.Add(new ClickableButton("btn4", "OtherMenu"));
        }
    }
}
