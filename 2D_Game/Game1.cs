using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace _2D_Game
{
    /// <summary>
    ///     This is the main Type for your game
    /// </summary>
    public class Game1 : Game
    {
        public enum Classes
        {
            Fighter = 0,
            Mage = 1,
            Archer = 2,
            Dragon = 3,
            Enemies = 4,
            None = 5
        }

        private static TileMap _tilemap;
        private static int _tilelength;
        private static Vector2 _tileLocation;
        public static int Camerax;
        public static int Cameray;
        //enemies VARS
        private static readonly List<Enemy> Enemies = new List<Enemy>();
        //LIST STUFF
        private static readonly RectangleF[] PlayerRects = new RectangleF[4];
        public static Texture2D ExpTex;
        public static List<Exp> Experiencelist = new List<Exp>();
        public static Texture2D Healthpotion;
        public static Texture2D Manapotion;
        public static List<Item> Pickupitems = new List<Item>();
        public static List<Npc> Npcs = new List<Npc>();
        public static List<Door> Doors = new List<Door>();
        public static Texture2D SmallHealthPotion;
        public static Texture2D LargeHealthPotion;
        private static RectangleF _screenRect;
        private readonly List<ClassSelector> _classlist = new List<ClassSelector>(4);
        private readonly GraphicsDeviceManager _graphics;
        private readonly Player[] _list = new Player[4];
        private readonly List<Things> _things = new List<Things>();
        private Rectangle _bottom = new Rectangle(0, 350, 800, 150);
        private Rectangle _bottomnew;
        //DEF RECT TEXTURE
        private Texture2D _boundingbox;
        private bool _check;
        //Extra Stuff
        //private Rectangle _door;
        //private Rectangle _door1;
        private SpriteFont _font;
        //private Random _generator = new Random();
        private bool _hasBeenClicked;
        private Rectangle _left = new Rectangle(0, 0, 200, 480);
        private Rectangle _leftnew;
        private bool _lefttouch, _righttouch, _uptouch, _downtouch;
        private Texture2D _lowerPlayer;
        private int _mapheight;
        private int _mapwidth;
        private Things[] _newList;
        private TileMap _newTilemap;
        private KeyboardState _oldkeys;
        //private List<Item> _pickupItems = new List<Item>();
        private Rectangle _right = new Rectangle(600, 0, 200, 480);
        private Rectangle _rightnew;
        //private SpriteFont _small;
        private SpriteBatch _spriteBatch;
        private GameState _state;
        //PLAYER VARS
        //private Texture2D _target;
        //private Rectangle _testrect = new Rectangle(400, 400, 50, 50);
        private int _tileheight;
        private int _tilemapheight;
        private int _tilemapwidth;
        private Rectangle _tilerect;
        private Block _tileset;
        private Rectangle _tilesourcerect;
        private int _tilewidth;
        //SCREEN MOVING RECTS
        private Rectangle _top = new Rectangle(0, 0, 800, 150);
        private Rectangle _topnew;
        private Texture2D _upperPlayer;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            _graphics.PreferredBackBufferWidth = 800;
            _graphics.PreferredBackBufferHeight = 480;
            _graphics.IsFullScreen = false;
        }

        protected override void Initialize()
        {
            _state = GameState.StartMenu;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _font = Content.Load<SpriteFont>("Font");
            //_small = Content.Load<SpriteFont>("small");
            ExpTex = Content.Load<Texture2D>("ball");
            _lowerPlayer = Content.Load<Texture2D>("lowerPlayer");
            _upperPlayer = Content.Load<Texture2D>("newplayer");
            _tileset = new Block(Content.Load<Texture2D>("tileset"));
            _screenRect = new RectangleF(_graphics.GraphicsDevice.Viewport.Bounds);

            #region Loading

            if (_state == GameState.Loading)
            {
                _boundingbox = Content.Load<Texture2D>("boundingbox");
                //_door = new Rectangle(300, 300, 30, 30);
                //_door1 = new Rectangle(53*32, 8*32, 30, 30);

                //---------------------------------------
                //_target = Content.Load<Texture2D>("target_icon");
                Healthpotion = Content.Load<Texture2D>("HealthPotion");

                Manapotion = Content.Load<Texture2D>("manapotion");
                SmallHealthPotion = Content.Load<Texture2D>("smallHealthpotion");
                LargeHealthPotion = Content.Load<Texture2D>("largeHealthPotion");
            }

            #endregion

            #region Start Menu

            if (_state == GameState.StartMenu)
            {
                ClassSelector.Load_textures(Content.Load<Texture2D>("FighterFront"),
                    Content.Load<Texture2D>("MageFront"),
                    Content.Load<Texture2D>("FighterFront"),
                    Content.Load<Texture2D>("enemyFront"),
                    Content.Load<Texture2D>("enemy"));

                _classlist.Insert(0, (new ClassSelector(Keys.Q, Keys.E, Keys.W)));
                _classlist.Insert(1, (new ClassSelector(Keys.R, Keys.Y, Keys.T)));
                _classlist.Insert(2, (new ClassSelector(Keys.U, Keys.O, Keys.I)));
                _classlist.Insert(3, (new ClassSelector(Keys.Left, Keys.Right, Keys.Space)));

                for (var i = 0; i < _list.Length; i++)
                    _list[i] = new Player((PlayerIndex) i);
            }

            #endregion
        }

        public void NewLoadLevel(string filename)
        {
            string line;
            var width = 0;
            var height = 0;
            var widthcounter = 0;
            var heightcounter = 0;
            var counter = 0;
            var npcsz = new List<Vector2>();
            var doorsz = new List<Door>();
            var playersz = new List<Vector2>();
            var readlist = new List<string>();
            // Read the file and display it line by line.
            _newTilemap = new TileMap();
            var reader = new StreamReader(filename);
            while ((line = reader.ReadLine()) != null)
            {
                if (counter >= 0 && counter < 3)
                {
                    readlist.Add(line != "Nothing" ? line : "Nothing");
                }
                else
                    switch (counter)
                    {
                        case 3:
                            width = Convert.ToInt32(line);
                            break;
                        case 4:
                            height = Convert.ToInt32(line);

                            _newTilemap.Width = width;
                            _newTilemap.Height = height;
                            break;
                    }
                if (counter > 4)
                {
                    if (heightcounter == 0)
                        _newTilemap.Tilemap.Add(new List<TileAdvanced>());
                    if (widthcounter < width)
                    {
                        if (heightcounter < height)
                        {
                            var c = line.Substring(0, 1) != "f";
                            _newTilemap.Tilemap[heightcounter].Add(new TileAdvanced(Convert.ToInt32(line.Substring(1)),
                                c));
                            widthcounter++;
                        }
                    }
                    else if (line == "newline")
                    {
                        _newTilemap.Tilemap.Add(new List<TileAdvanced>());
                        widthcounter = 0;
                        heightcounter++;
                    }
                }
                counter++;
            }
            reader.Close();
            for (var i = 0; i < readlist.Count; i++)
            {
                if (readlist[i].Length > 0)
                {
                    //Add Players and NPCS
                    var players = readlist[i].Split('|').ToList();
                    players.Remove("");
                    if (i == 0 || i == 1)
                    {
                        for (var j = 0; (j/2) < (players.Count/2); j += 2)
                        {
                            if (i == 0)
                                playersz.Add(new Vector2(Convert.ToInt32(players[j]), Convert.ToInt32(players[j + 1])));
                            else if (i == 1)
                                npcsz.Add(new Vector2(Convert.ToInt32(players[j]), Convert.ToInt32(players[j + 1])));
                        }
                    }
                    if (i == 2)
                    {
                        players.Remove("");
                        for (var j = 0; j < players.Count; j += 3)
                        {
                            if (players[0] != "Nothing")
                                doorsz.Add(new Door(players[j + 2],
                                    new Vector2(Convert.ToInt32(players[j]), Convert.ToInt32(players[j + 1]))));
                        }
                    }
                }
            }
            _tilemap = _newTilemap;
            _tilemapwidth = width;
            _tilemapheight = height;
            _tilelength = 32;
            _tilewidth = 32;
            _tileheight = 32;

            for (var i = 0; i < npcsz.Count; i++)
                Npcs.Add(new Npc(Content.Load<Texture2D>("Npc"), (int) npcsz[i].X, (int) npcsz[i].Y));
            for (var i = 0; i < playersz.Count; i++)
            {
                _list[i].Position = playersz[i];
            }
            foreach (var door in doorsz)
                Doors.Add(new Door(new Rectangle(door.TestBox.X, door.TestBox.Y, 0, 0), new Vector2(100, 100)));

            _mapwidth = width*32;
            _mapheight = height*32;
            _list[0].Position = new Vector2(300, 350);
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
                Exit();

            #region Start Menu

            if (_state == GameState.StartMenu)
            {
                for (var i = 0; i < _classlist.Capacity; i++)
                {
                    _classlist[i].Update(keys, _oldkeys);

                    var emptyBars = Content.Load<Texture2D>("EmptyBars");
                    var newRedBar = Content.Load<Texture2D>("NewRedBar");
                    var greenBar = Content.Load<Texture2D>("GreenBar");
                    var blueBar = Content.Load<Texture2D>("BlueBar");
                    if ((Classes) _classlist[i].Choice == Classes.Mage)
                    {
                        _list[i] = new Mage(Content.Load<Texture2D>("Mage"),
                            Content.Load<Texture2D>("target_icon"), (PlayerIndex) i,
                            new HealthBar(emptyBars, newRedBar, blueBar, greenBar));
                    }
                    else if ((Classes) _classlist[i].Choice == Classes.Fighter)
                    {
                        _list[i] = new Fighter(_upperPlayer, (PlayerIndex) i,
                            new HealthBar(emptyBars, newRedBar, blueBar, greenBar), _lowerPlayer,
                            "C:\\Users\\timmy_000\\Desktop\\Animations.txt");
                    }
                    else if ((Classes) _classlist[i].Choice == Classes.Archer)
                    {
                        var arrow = Content.Load<Texture2D>("Arrow");
                        _list[i] = new Archer(_upperPlayer, (PlayerIndex) i,
                            new HealthBar(emptyBars, newRedBar, blueBar, greenBar), arrow);
                    }
                }
                if (keys.IsKeyDown(Keys.Enter))
                {
                    _hasBeenClicked = true;
                    var check = false;
                    foreach (var player in _list.Where(player => player.Type != "Player"))
                        check = true;
                    NewLoadLevel("C:\\Users\\timmy_000\\Documents\\Test\\evennewer.txt");
                    //Enemies.Add(new Dragon(Content.Load<Texture2D>("dragon"), 100, 100,
                    //    Content.Load<Texture2D>("RedBar"), Content.Load<Texture2D>("projectile")));
                    if (check)
                        _state = GameState.Loading;
                }
            }

            #endregion

            #region Loading

            if (_state == GameState.Loading)
            {
                UnloadContent();
                LoadContent();
                _state = GameState.Playing;
            }

            #endregion

            #region Playing

            if (_state == GameState.Playing)
            {
                //Pause the game
                if (keys.IsKeyDown(Keys.P) && _oldkeys.IsKeyUp(Keys.P))
                    _state = GameState.Paused;
                //Update Players
                PlayerUpdates();
                //Update Camera
                CameraUpdate(gameTime);
                //Update Bullets
                Updateshot();
                //UpdateBullets(gameTime);
                //enemies AI
                enemies_AI(gameTime);
                foreach (var npc in Npcs)
                    npc.Act(gameTime);
            }

            #endregion

            #region Paused

            if (_state == GameState.Paused)
            {
                _check = true;
                if (keys.IsKeyDown(Keys.Enter) && _oldkeys.IsKeyUp(Keys.Enter))
                {
                    _state = GameState.Playing;
                    _check = false;
                }
            }

            #endregion

            _oldkeys = keys;
            base.Update(gameTime);
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
                    _spriteBatch.DrawString(_font, "Type: " + _list[i].Type, new Vector2(150*i, 400), Color.White);
                if (_hasBeenClicked)
                    _spriteBatch.DrawString(_font, "Please pick at least one character", new Vector2(400, 300),
                        Color.Red);
            }

            #endregion

            #region Playing

            if (_state == GameState.Playing || _state == GameState.Paused)
            {
                //26x16
                var endx = 26 + Camerax/_tilelength;
                var endy = 16 + Cameray/_tilelength;
                //DRAW MAP
                for (var x = Camerax/_tilelength; x < MathHelper.Clamp(endx, 0, _tilemapwidth); x++)
                {
                    for (var y = Cameray/_tilelength; y < MathHelper.Clamp(endy, 0, _tilemapheight); y++)
                    {
                        //Section of image to draw
                        _tilesourcerect = new Rectangle(_tilemap.Tilemap[y][x].SourceX*_tilelength, 0, _tilewidth,
                            _tileheight);

                        //Destination Rectangle
                        _tilerect = new Rectangle(
                            (x*_tilewidth) - Camerax,
                            (y*_tileheight) - Cameray,
                            _tilewidth,
                            _tileheight);

                        _spriteBatch.Draw(_tileset.BlockTexture,
                            _tilerect,
                            _tilesourcerect, _tilemap.Tilemap[y][x].Collidable == false ? Color.White : Color.Red);

                        //spriteBatch.DrawString(small, x.ToString() + "," + y.ToString(), new Vector2(tilerect.X, tilerect.Y), Color.White);
                    }
                }
                foreach (var door in Doors)
                    _spriteBatch.Draw(_boundingbox, CameraFix(door.TestBox), Color.Red);
                //
                //Combine All Lists and Arrays for Drawing
                //
                foreach (var player in _list)
                {
                    _things.Insert(0, player);
                }
                foreach (var npc in Npcs)
                {
                    _things.Insert(0, npc);
                }
                foreach (var item in Pickupitems)
                {
                    _things.Insert(0, item);
                }
                foreach (var enemy in Enemies)
                {
                    _things.Insert(0, enemy);
                }
                //SORT ARRAY FOR DRAWING
                _newList = _things.OrderByDescending(x => x.Feetrect.Y).ToArray();
                for (var i = _newList.Length - 1; i > -1; i--)
                {
                    if ((_newList[i].GetType() == typeof (Fighter)))
                        ((Fighter) _newList[i]).Draw(_spriteBatch, _font, (int) ((Fighter) _newList[i]).Playerindex,
                            _boundingbox);
                    else if (_newList[i].GetType() == typeof (Seller))
                        ((Seller) _newList[i]).Draw(_spriteBatch, _font, _boundingbox, true);
                    else if (_newList[i].GetType() == typeof (Dragon))
                        ((Dragon) _newList[i]).Draw(_spriteBatch, gameTime, _font, _check, _boundingbox);
                    else if (_newList[i].GetType() == typeof (HealthPotion))
                        ((HealthPotion) _newList[i]).Draw(_spriteBatch);
                    else if (_newList[i].GetType() == typeof (SmallHealthPotion))
                        ((SmallHealthPotion) _newList[i]).Draw(_spriteBatch);
                    else if (_newList[i].GetType() == typeof (LargeHealthPotion))
                        ((LargeHealthPotion) _newList[i]).Draw(_spriteBatch);
                    else if (_newList[i].GetType() == typeof (ManaPotion))
                        ((ManaPotion) _newList[i]).Draw(_spriteBatch);
                    else if (_newList[i].GetType() == typeof (Mage))
                        ((Mage) _newList[i]).Draw(_spriteBatch, _font, (int) ((Mage) _newList[i]).Playerindex,
                            _boundingbox);
                    else if (_newList[i].GetType() == typeof (Archer))
                        ((Archer) _newList[i]).Draw(_spriteBatch, _font, (int) ((Archer) _newList[i]).Playerindex,
                            _boundingbox);
                    else if (_newList[i].GetType() == typeof (Npc))
                        ((Npc) _newList[i]).Draw(_spriteBatch, _font, _boundingbox, true);
                    else if (_newList[i].GetType() == typeof (Chest))
                        ((Chest) _newList[i]).Draw(_spriteBatch);
                    _things.RemoveAt(i);
                    //spriteBatch.DrawString(font,"I:    " +  i.ToString(), new Vector2(300, 300), Color.Red);
                }
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

                //Other Text
                _spriteBatch.DrawString(_font, "CameraX: " + Camerax, new Vector2(200, 320), Color.Red);
                _spriteBatch.DrawString(_font, "CameraY: " + Cameray, new Vector2(200, 340), Color.Red);
                /////////////////////////////////////////////////////////////////////////////////////////////////////////////
                //DEFAULT DEBUG TEXT-----------------------------------------------------------------------------------------   
                //spriteBatch.DrawString(font,"PosX: "+ list[0].Position.X.ToString() + "   PosY: "+list[0].Position.Y.ToString(), new Vector2(300, 300), Color.Red);
                //-----------------------------------------------------------------------------------------------------------
                //spriteBatch.Draw(boundingbox, player1, Color.White);
                //spriteBatch.Draw(boundingbox, player2, Color.White);
                //spriteBatch.Draw(boundingbox, player3, Color.White);
                //spriteBatch.Draw(boundingbox, player4, Color.White);
                //spriteBatch.Draw(boundingbox, new Rectangle(0, 0, 35 * 7, 35 * 5), Color.Blue);
                //DRAW EXP ORBS
                foreach (var expOrb in Experiencelist)
                    expOrb.Draw(_spriteBatch);

                //DRAW NPC INVENTORIES
                foreach (var player in _list)
                {
                    player.DrawItems(_spriteBatch, _boundingbox);
                    _spriteBatch.Draw(_boundingbox, player.Testbox.ToRectangle(), Color.White);
                }
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

            #endregion

            #region Paused

            if (_state == GameState.Paused)
            {
                _spriteBatch.Draw(_boundingbox, new Rectangle(0, 0, 800, 480), Color.White);
                _spriteBatch.DrawString(_font, "PAUSED", new Vector2(340, 230), Color.White);
            }

            #endregion

            _spriteBatch.End();
            base.Draw(gameTime);
        }

        #region Enemy Updates

        public void enemies_AI(GameTime gametime)
        {
            //Finds closest player
            for (var i = 0; i < Enemies.Count; i++)
            {
                if (!Enemies[i].IsActive) continue;
                var firstPlayer = _list[0];
                var xdiff = Math.Abs(_list[0].Position.X - Enemies[i].Position.X);
                var ydiff = Math.Abs(_list[0].Position.Y - Enemies[i].Position.Y);
                var diff = xdiff + ydiff;
                foreach (var player in _list)
                {
                    if (player.GetType() == typeof (Player)) continue;
                    if (!(diff >
                          (Math.Abs(player.Position.X - Enemies[i].Position.X) +
                           Math.Abs(player.Position.Y - Enemies[i].Position.Y)))) continue;
                    firstPlayer = player;
                    diff = (Math.Abs(player.Position.X - Enemies[i].Position.X) +
                            Math.Abs(player.Position.Y - Enemies[i].Position.Y));
                }
                Enemies[i].Act(firstPlayer.Testbox.ToRectangle(),
                    (int) Enemies[i].Position.X, (int) Enemies[i].Position.Y, i, gametime);
            }
        }

        #endregion

        //GAMESTATE VARS
        private enum GameState
        {
            StartMenu,
            Loading,
            Playing,
            Paused
        }

        #region Player Updates

        public void PlayerUpdates()
        {
            //Check Player Stuffz
            foreach (var player in _list)
            {
                if (player.Alive)
                {
                    //Check Fighter attacks
                    if (player.GetType() == typeof (Fighter))
                    {
                        if (player.Attackmode)
                        {
                            foreach (var enemy in Enemies)
                            {
                                if (!enemy.IsActive) continue;
                                if (!((Fighter) player).AttackRectangle.Intersects(enemy.Rectangle)) continue;
                                if (enemy.Ishurting == false)
                                    enemy.Hurt(10, player.Velocityx, player.Velocityy);
                            }
                        }
                    }
                    //Check Mage attack
                    if (player.GetType() == typeof (Mage))
                    {
                        if (((Mage) player).TargetInterval >= 200f)
                        {
                            foreach (var enemy in Enemies)
                            {
                                if (enemy.IsActive)
                                    if (enemy.Rectangle.Intersects(((Mage) player).TargetRect))
                                        if (enemy.Ishurting == false)
                                            enemy.Hurt(100, 0, 0);
                            }
                            ((Mage) player).Shot = false;
                            ((Mage) player).TargetInterval = 0;
                            ((Mage) player).IsAttacking = false;
                        }
                    }
                    if (player.GetType() == typeof (Archer))
                    {
                        foreach (var arrowProjectile in ((Archer) player).Arrows)
                        {
                            if (!arrowProjectile.IsActive) continue;
                            foreach (var enemy in Enemies)
                            {
                                if (!enemy.IsActive) continue;
                                if ((arrowProjectile).Rect.Intersects(enemy.Rectangle))
                                    enemy.Hurt(100, (int) (arrowProjectile).Direction.X,
                                        (int) (arrowProjectile).Direction.Y);
                            }
                        }
                    }
                    //enemies PLAYER COLLISION
                    if (!player.Ishurting)
                    {
                        foreach (var enemy in Enemies)
                        {
                            if (!enemy.IsActive || enemy.Ishurting) continue;
                            if (player.Ishurting || !player.Testbox.Intersects(enemy.Rectangle)) continue;
                            player.Hurt(10);
                            player.Ishurting = true;
                        }
                    }
                    //DEATH
                    if (player.Health <= 0)
                        player.Dead();
                    ////DOOR COLLISION
                    var player1 = player;
                    foreach (var door in Doors.Where(door => player1.Feetrect.Intersects(door.TestBox)))
                        player.Position = door.Destination;
                    //Update huds
                    if (!(player.GetType() == typeof (Player)))
                        player.UpdateHud();
                }
                for (var i = 0; i < Experiencelist.Count; i++)
                    if (player.Testbox.Intersects(Experiencelist[i].HitBox))
                    {
                        player.Xp += 10;
                        Experiencelist.RemoveAt(i);
                    }
                for (var i = 0; i < Pickupitems.Count; i++)
                {
                    if (!player.Testbox.Intersects(Pickupitems[i].HitBox)) continue;
                    player.GetItem(Pickupitems[i]);
                    Pickupitems.RemoveAt(i);
                }
            }
        }

        public void CameraUpdate(GameTime gameTime)
        {
            //Moving Rectangle Collision
            _topnew = new Rectangle(_top.X + Camerax, _top.Y + Cameray, _top.Width, _top.Height);
            _bottomnew = new Rectangle(_bottom.X + Camerax, _bottom.Y + Cameray, _bottom.Width, _bottom.Height);
            _leftnew = new Rectangle(_left.X + Camerax, _left.Y + Cameray, _left.Width, _left.Height);
            _rightnew = new Rectangle(_right.X + Camerax, _right.Y + Cameray, _right.Width, _right.Height);
            //SPRITE MOVEMENT
            _downtouch = false;
            _uptouch = false;
            _lefttouch = false;
            _righttouch = false;
            var upvalue = 0;
            var leftvalue = 0;
            var rightvalue = 0;
            var downvalue = 0;
            foreach (var player in _list.Where(player => player.Alive))
            {
                //Update Player Inputs
                if (player.GetType() == typeof (Fighter))
                {
                    ((Fighter) player).Act(gameTime, _tilemap);
                }
                if (player.Testbox.Intersects(_bottomnew))
                {
                    _downtouch = true;
                    downvalue += player.SpriteSpeed;
                }
                if (player.Testbox.Intersects(_topnew))
                {
                    _uptouch = true;
                    upvalue += player.SpriteSpeed;
                }
                if (player.Testbox.Intersects(_leftnew))
                {
                    _lefttouch = true;
                    leftvalue += player.SpriteSpeed;
                }
                if (player.Testbox.Intersects(_rightnew))
                {
                    _righttouch = true;
                    rightvalue += player.SpriteSpeed;
                }

                if (!(_downtouch && _uptouch))
                {
                    if (_downtouch)
                    {
                        Cameray += downvalue;
                    }
                    if (_uptouch)
                    {
                        Cameray -= upvalue;
                    }
                }
                if (!(_lefttouch && _righttouch))
                {
                    if (_lefttouch)
                    {
                        Camerax -= leftvalue;
                    }
                    if (_righttouch)
                    {
                        Camerax += rightvalue;
                    }
                }
                //Camera Bounds
                if (Camerax < 0)
                    Camerax = 0;
                if (Cameray < 0)
                    Cameray = 0;
                if (Camerax > _mapwidth - 800)
                    Camerax = _mapwidth - 800;
                if (Cameray > _mapheight - 480)
                    Cameray = _mapheight - 480;
            }
        }

        #endregion

        #region Helper Camera Functions

        public static Rectangle CameraFix(Rectangle rect)
        {
            return new Rectangle(rect.X - Camerax, rect.Y - Cameray, rect.Width, rect.Height);
        }

        public static Vector2 CameraFix(Vector2 vect)
        {
            return new Vector2(vect.X - Camerax, vect.Y - Cameray);
        }

        public static RotatedRectangle CameraFix(RotatedRectangle rect)
        {
            rect.ChangePosition(-Camerax, -Cameray);
            return rect;
        }

        public static RectangleF CameraFix(RectangleF rect)
        {
            return new RectangleF(new Vector2(rect.Min.X - Camerax, rect.Min.Y - Cameray),
                new Vector2(rect.Max.X - Camerax, rect.Max.Y - Cameray));
        }

        #endregion

        #region COLLISION AND SHOOTING

        //CALCULATE COLLISION FOR PLAYERS
        //public static bool Check_Collisions(Rectangle feetrectnew, PlayerIndex playerindex)
        //{
        //    return !CalculateCollision(feetrectnew) && !Pvp(playerindex) && _screenRect.Contains(feetrectnew);
        //}
        public static bool Check_Collisions(RectangleF feetrectnew, PlayerIndex playerindex)
        {
            return !CalculateCollision(feetrectnew) && !Pvp(playerindex) && _screenRect.Contains(CameraFix(feetrectnew));
        }

        public static bool CalculateCollision(Rectangle footrect)
        {
            //minus minus(TopLeft)
            _tileLocation = new Vector2((float) footrect.Left/_tilelength,
                (float) footrect.Top/_tilelength);
            if (_tilemap.Tilemap[(int) _tileLocation.Y][(int) _tileLocation.X].Collidable)
                return true;

            //MINUS PLUS(BottomLeft)
            _tileLocation = new Vector2((float) footrect.Left/_tilelength,
                (float) footrect.Bottom/_tilelength);
            if (_tilemap.Tilemap[(int) _tileLocation.Y][(int) _tileLocation.X].Collidable)
                return true;

            //PLUS MINUS(TopRight)
            _tileLocation = new Vector2((float) footrect.Right/_tilelength,
                (float) footrect.Top/_tilelength);
            if (_tilemap.Tilemap[(int) _tileLocation.Y][(int) _tileLocation.X].Collidable)
                return true;

            //PLUS PLUS(BottomRight)
            _tileLocation = new Vector2((float) (footrect.Right)/_tilelength,
                (float) footrect.Bottom/_tilelength);
            if (_tilemap.Tilemap[(int) _tileLocation.Y][(int) _tileLocation.X].Collidable)
                return true;

            return false;
        }

        public static bool CalculateCollision(RectangleF footrect)
        {
            //minus minus(TopLeft)
            _tileLocation = new Vector2(footrect.Left/_tilelength,
                footrect.Top/_tilelength);
            if (_tilemap.Tilemap[(int) _tileLocation.Y][(int) _tileLocation.X].Collidable)
                return true;

            //MINUS PLUS(BottomLeft)
            _tileLocation = new Vector2(footrect.Left/_tilelength,
                footrect.Bottom/_tilelength);
            if (_tilemap.Tilemap[(int) _tileLocation.Y][(int) _tileLocation.X].Collidable)
                return true;

            //PLUS MINUS(TopRight)
            _tileLocation = new Vector2(footrect.Right/_tilelength,
                footrect.Top/_tilelength);
            if (_tilemap.Tilemap[(int) _tileLocation.Y][(int) _tileLocation.X].Collidable)
                return true;

            //PLUS PLUS(BottomRight)
            _tileLocation = new Vector2(footrect.Right/_tilelength,
                footrect.Bottom/_tilelength);
            if (_tilemap.Tilemap[(int) _tileLocation.Y][(int) _tileLocation.X].Collidable)
                return true;

            return false;
        }

        #region PVP_Collision

        //Check enemies rectangles
        public static bool EnemyVSenemycollision(int current, Rectangle rect)
        {
            return
                Enemies.Where((t, i) => current != i)
                    .Where(t => t.Ishurting == false)
                    .Any(t => t.Rectangle.Intersects(rect));
        }

        //Get player rect
        public static void PlayerVSplayercollision1(RectangleF footrect, PlayerIndex playindex)
        {
            PlayerRects[(int) playindex] = footrect;
        }

        //compare rectangles
        public static bool Pvp(PlayerIndex playindex)
        {
            return PlayerRects.Where((t, i) => i != (int) playindex)
                .Any(t => PlayerRects[(int) playindex].Intersects(t))
                   || Npcs.Any(t => PlayerRects[(int) playindex].Intersects(t.FeetBox));
        }

        #endregion

        //Update enemiesShots and player
        public void Updateshot()
        {
            // Updates the location of all of the enemy player shot.
            foreach (var enemy in Enemies.Where(enemy => enemy.GetType() == typeof (Dragon)))
            {
                for (var x = 0; x < ((Dragon) enemy).Shot; x++)
                {
                    if (!((Dragon) enemy).Bullets[x].IsActive) continue;
                    foreach (var player in _list)
                    {
                        if (player.Ishurting || !(player.Testbox.Intersects(((Dragon) enemy).Bullets[x].Hitbox)))
                            continue;
                        player.Hurt(10);
                        player.Ishurting = true;
                    }
                }
            }
        }

        #endregion
    }
}