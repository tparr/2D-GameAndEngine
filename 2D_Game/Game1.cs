using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace _2D_Game
{
    public class Game1 : Game
    {
        public static List<Exp> Experiencelist = new List<Exp>();

        public static List<Item> Pickupitems = new List<Item>();

        public static List<Point> touchedTiles = new List<Point>();

        private static RectangleF _screenRect;

        private readonly List<ClassSelector> _classlist = new List<ClassSelector>(4);

        private readonly GraphicsDeviceManager _graphics;

        public static Texture2D _boundingbox;

        private SpriteFont _font;

        private bool _hasBeenClicked;

        private Player[] _list = new Player[4];

        private KeyboardState _oldkeys;

        private SpriteBatch _spriteBatch;

        private GameState _state;

        private TileMap _upperTileMap = new TileMap();

        private World world;
        public static Texture2D Healthpotion;
        public static Texture2D SmallHealthPotion;
        public static Texture2D LargeHealthPotion;
        public static Texture2D Manapotion;
        public static Texture2D ExpTexture;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            _graphics.PreferredBackBufferWidth = 800;
            _graphics.PreferredBackBufferHeight = 480;
            _graphics.IsFullScreen = false;
        }

        public enum Classes
        {
            Fighter = 0,
            Mage = 1,
            Archer = 2,
            Dragon = 3,
            Enemies = 4,
            None = 5
        }

        //GameState VARS
        private enum GameState
        {
            StartMenu,
            Loading,
            Playing,
            Paused
        }

        public static bool IsSameOrSubclass(Type potentialBase, Type potentialDescendant)
        {
            return potentialDescendant.IsSubclassOf(potentialBase)
                   || potentialDescendant == potentialBase;
        }

        //---------Notable Fixes----------------------------------
        //Made PVP Rectangle Collision a lot smoother and took away need of seperate rectangles
        //Sorts all entities and draws accurately for depth
        //Awesome Possiblities with Level Loading!!
        protected override void Draw(GameTime gameTime)
        {
            _graphics.GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();

            #region Start Menu

            if (_state == GameState.StartMenu)
            {
                _spriteBatch.DrawString(_font, "Press Enter to Start", new Vector2(270, 200), Color.Green);
                for (var i = 0; i < _classlist.Count; i++)
                    _classlist[i].Draw(_spriteBatch, _font, i);
                for (var i = 0; i < _list.Length; i++)
                    _spriteBatch.DrawString(_font, "Type: " + _list[i].Type, new Vector2(150 * i, 400), Color.White);
                if (_hasBeenClicked)
                    _spriteBatch.DrawString(_font, "Please pick at least one character", new Vector2(400, 300),
                        Color.Red);
            }

            #endregion Start Menu

            #region Playing

            if (_state == GameState.Playing || _state == GameState.Paused)
            {
                world.Draw(_spriteBatch, _boundingbox, _font);
                //Debug Text Draws
                //spriteBatch.DrawString(font, "SpritePosX:" + sprite.Position.X.ToString(), new Vector2(200, 380), Color.Red);
                //spriteBatch.DrawString(font, "SpritePosY:" + sprite.Position.Y.ToString(), new Vector2(200, 400), Color.Red);
                //spriteBatch.DrawString(font, "mapwidth: " + tilemapwidth.ToString(), new Vector2(0, 0), Color.Blue);
                //spriteBatch.DrawString(font, "mapheight: " + tilemapheight.ToString(), new Vector2(0, 20), Color.Blue);

                //ATTACK RECTANGLES
                //spriteBatch.Draw(boundingbox, CameraFix(list[0].AttackRectangle), Color.White);

                //Movement Camera rectangle drawing
                //_spriteBatch.Draw(_boundingbox, _leftnew, Color.White);
                //_spriteBatch.Draw(_boundingbox, _rightnew, Color.White);
                //_spriteBatch.Draw(_boundingbox, _topnew, Color.White);
                //_spriteBatch.Draw(_boundingbox, _bottomnew, Color.White);

                /////////////////////////////////////////////////////////////////////////////////////////////////////////////
                //DEFAULT DEBUG TEXT-----------------------------------------------------------------------------------------
                //spriteBatch.DrawString(font,"PosX: "+ list[0].Position.X.ToString() + "   PosY: "+list[0].Position.Y.ToString(), new Vector2(300, 300), Color.Red);
                //-----------------------------------------------------------------------------------------------------------
                //spriteBatch.Draw(boundingbox, new Rectangle(0, 0, 35 * 7, 35 * 5), Color.Blue);
                //((Fighter)list[0]).DrawText(spriteBatch, font);

                //spriteBatch.DrawString(font,
                //"Min X: " + ((Fighter)list[0]).minFrameX.ToString() + "  Max X: " + ((Fighter)list[0]).maxFrameX.ToString(),
                //new Vector2(200, 200), Color.Red);
                //spriteBatch.DrawString(font, "ComboInterval: " + ((Fighter)list[0]).combointerval.ToString(), new Vector2(300, 300), Color.Red);
                //if (list[0].attackmode)
                //{
                //    spriteBatch.Draw(boundingbox, ((Fighter)list[0]).aPositionAdjusted, null, Color.White, ((Fighter)list[0]).keyrects[list[0].currentFYT].Rotation,new Vector2(5, 5), SpriteEffects.None, 0f);
                //    //spriteBatch.DrawString(font, ((Fighter)list[0]).attackrect. + "/" + testrect.X,new Vector2(100,60), Color.Red);
                //}
                //spriteBatch.DrawString(font, "MinFrame: " + ((Fighter)list[0]).minFrameX.ToString() + "  MaxFrame: " + ((Fighter)list[0]).maxFrameX.ToString(), new Vector2(100, 100), Color.Blue);
                //spriteBatch.Draw(boundingbox, CameraFix(testrect), Color.White);
                //spriteBatch.DrawString(font, "Touching: " + hit.ToString(), new Vector2(100, 120), Color.Red);
                //spriteBatch.Draw(boundingbox, ScreenRect, Color.White);
                //_spriteBatch.Draw(_boundingbox,_screenRect.ToRectangle(),Color.White);
                // _spriteBatch.Draw(_boundingbox,_screenRect.ToRectangle(),Color.White);
                //_spriteBatch.DrawString(_font, "X: " + _list[0].Feetrect.X + " Y: " + _list[0].Feetrect.Y,
                //    new Vector2(300, 400), Color.Red);
            }

            #endregion Playing

            #region Paused

            if (_state == GameState.Paused)
            {
                _spriteBatch.Draw(_boundingbox, new Rectangle(0, 0, 800, 480), Color.White);
                _spriteBatch.DrawString(_font, "PAUSED", new Vector2(340, 230), Color.White);
            }

            #endregion Paused

            _spriteBatch.End();
            base.Draw(gameTime);
        }

        protected override void Initialize()
        {
            _state = GameState.StartMenu;
            Projectile.InitializeProjectile(Content.Load<Texture2D>("projectile"));
            world = new World(Content);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _font = Content.Load<SpriteFont>("Font");
            //_small = Content.Load<SpriteFont>("small");
            ExpTexture = Content.Load<Texture2D>("ball");
            //_lowerPlayer = Content.Load<Texture2D>("lowerPlayer");
            //_upperPlayer = Content.Load<Texture2D>("newplayer");
            _screenRect = new RectangleF(_graphics.GraphicsDevice.Viewport.Bounds);
            _boundingbox = Content.Load<Texture2D>("boundingbox");
            HealthBar.BackgroundImage = Content.Load<Texture2D>("EmptyBars");
            HealthBar.healthImage = Content.Load<Texture2D>("RedBar");
            HealthBar.experienceImage = Content.Load<Texture2D>("GreenBar");
            HealthBar.magicImage = Content.Load<Texture2D>("BlueBar");

            #region Loading

            if (_state == GameState.Loading)
            {
                world.LoadNewLevel("Level1", new ContentManager(Content.ServiceProvider));

                //_door = new Rectangle(300, 300, 30, 30);
                //_door1 = new Rectangle(53*32, 8*32, 30, 30);

                //---------------------------------------
                //_target = Content.Load<Texture2D>("target_icon");
                Healthpotion = Content.Load<Texture2D>("HealthPotion");

                Manapotion = Content.Load<Texture2D>("manapotion");
                SmallHealthPotion = Content.Load<Texture2D>("smallHealthpotion");
                LargeHealthPotion = Content.Load<Texture2D>("largeHealthPotion");
            }

            #endregion Loading

            #region Start Menu

            if (_state == GameState.StartMenu)
            {
                ClassSelector.Load_textures(
                    Content.Load<Texture2D>("FighterFront"),
                    Content.Load<Texture2D>("MageFront"),
                    Content.Load<Texture2D>("FighterFront"),
                    Content.Load<Texture2D>("enemyFront"),
                    Content.Load<Texture2D>("enemy"));

                _classlist.Insert(0, (new ClassSelector(Keys.Q, Keys.E, Keys.W)));
                _classlist.Insert(1, (new ClassSelector(Keys.R, Keys.Y, Keys.T)));
                _classlist.Insert(2, (new ClassSelector(Keys.U, Keys.O, Keys.I)));
                _classlist.Insert(3, (new ClassSelector(Keys.Left, Keys.Right, Keys.Space)));

                for (var i = 0; i < _list.Length; i++)
                    _list[i] = new Player((PlayerIndex)i);
            }

            #endregion Start Menu
        }

        protected override void UnloadContent()
        {
            if (_state == GameState.Loading)
                _classlist.RemoveRange(0, 4);
        }

        //UPDATE
        //UPDATE
        //UPDATE
        protected override void Update(GameTime gameTime)
        {
            var keys = Keyboard.GetState();
            if (keys.IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            #region Start Menu

            if (_state == GameState.StartMenu)
            {
                for (var i = 0; i < _classlist.Capacity; i++)
                {
                    _classlist[i].Update(keys, _oldkeys);
                    if ((Classes)_classlist[i].Choice == Classes.Mage)
                    {
                        _list[i] = new Mage((PlayerIndex)i, world._content);
                    }
                    else if ((Classes)_classlist[i].Choice == Classes.Fighter)
                    {
                        _list[i] = new Fighter((PlayerIndex)i, world._content);
                    }
                    else if ((Classes)_classlist[i].Choice == Classes.Archer)
                    {
                        _list[i] = new Archer((PlayerIndex)i, world._content);
                    }
                }
                if (keys.IsKeyDown(Keys.Enter))
                {
                    _hasBeenClicked = true;
                    var check = false;
                    foreach (var player in _list.Where(player => player.Type != "Player"))
                        check = true;
                    //Enemies.Add(new Dragon(Content.Load<Texture2D>("dragon"), 100, 100,
                    //    Content.Load<Texture2D>("RedBar"), Content.Load<Texture2D>("projectile")));
                    if (check)
                    {
                        foreach (var player in _list.Where(player => player.Type != "Player"))
                        {
                            world.AddEntity(player);
                        }
                        _state = GameState.Loading;
                    }
                }
            }

            #endregion Start Menu

            #region Loading

            if (_state == GameState.Loading)
            {
                UnloadContent();
                LoadContent();
                _state = GameState.Playing;
            }

            #endregion Loading

            #region Playing

            if (_state == GameState.Playing)
            {
                //Pause the game
                if (keys.IsKeyDown(Keys.P) && _oldkeys.IsKeyUp(Keys.P))
                    _state = GameState.Paused;
                //UpdateBullets(gameTime);
                world.Update();
            }

            #endregion Playing

            #region Paused

            if (_state == GameState.Paused)
            {
                if (keys.IsKeyDown(Keys.Enter) && _oldkeys.IsKeyUp(Keys.Enter))
                {
                    _state = GameState.Playing;
                }
            }

            #endregion Paused

            _oldkeys = keys;
            base.Update(gameTime);
        }

        private void checkTilesUnder(RectangleF rect)
        {
            throw new NotImplementedException();
            ////minus minus(TopLeft)
            //_tileLocation = new Vector2(rect.Left/_tilelength,
            //    rect.Top/_tilelength);
            //touchedTiles.Add(new Point((int) _tileLocation.X, (int) _tileLocation.Y));

            ////MINUS PLUS(BottomLeft)
            //_tileLocation = new Vector2(rect.Left/_tilelength,
            //    rect.Bottom/_tilelength);
            //touchedTiles.Add(new Point((int) _tileLocation.X, (int) _tileLocation.Y));

            ////PLUS MINUS(TopRight)
            //_tileLocation = new Vector2(rect.Right/_tilelength,
            //    rect.Top/_tilelength);
            //touchedTiles.Add(new Point((int) _tileLocation.X, (int) _tileLocation.Y));

            ////PLUS PLUS(BottomRight)
            //_tileLocation = new Vector2((rect.Right)/_tilelength,
            //    rect.Bottom/_tilelength);
            //touchedTiles.Add(new Point((int) _tileLocation.X, (int) _tileLocation.Y));
        }
    }
}