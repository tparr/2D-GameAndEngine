using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ButtonState = Microsoft.Xna.Framework.Input.ButtonState;
using KeyStrokes = Microsoft.Xna.Framework.Input.Keys;
namespace ComeBackGameEditor
{
    public class Game1 : Game
    {
        readonly GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;
        int _camerax;
        int _cameray;
        private const int Tilelength = 32;
        public static int Tilemapwidth=10;
        public static int Tilemapheight=10;
        Rectangle _tilesourcerect;
        private const int Tilewidth = 32;
        private const int Tileheight = 32;
        Block _tileset;
        Rectangle _tilerect;
        KeyboardState _keys;
        KeyboardState _oldkeys;
        MouseState _mousepositions;
        Texture2D _boundingbox;
        List<Button> _buttons = new List<Button>();
        int _sourceX;
        List<Button> _otherButtons = new List<Button>();
        enum MouseStates {Collide,Uncollidable,Player,Npc,Door,Delete,Select,Brush}
        MouseStates _state;
        Texture2D _unCollidable;
        SpriteFont _font;
        private const string Text = "";
        Form1 _form;
        static public TileMap Tilemap;
        int _scrollspeed=3;
        SpriteFont _small;
        Texture2D _player;
        Texture2D _npc;
        Texture2D _door;
        static public List<Vector2> Players = new List<Vector2>();
        static public List<Vector2> Npcs = new List<Vector2>();
        static public List<Door> Doors = new List<Door>();
        MouseState _oldmouse;
        Texture2D _delete;
        static public List<string> Doorfilenames = new List<string>();
        SpriteFont _bold;
        Texture2D _cursor;
        static public int Buttonindex = 0;
        int _oldbuttonindex;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            _form = new Form1 {Tilemap = Tilemap};
            _form.Show();
            _graphics.PreferredBackBufferWidth = 900;
            _graphics.PreferredBackBufferHeight = 500;
        }
        protected override void Initialize()
        {
            base.Initialize();
            IsMouseVisible = true;
            
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _tileset = new Block(Content.Load<Texture2D>("tileset"));
            _boundingbox = Content.Load<Texture2D>("boundingbox");
            _unCollidable = Content.Load<Texture2D>("UnCollidable");
            _delete = Content.Load<Texture2D>("delete-32x32");
            _cursor = Content.Load<Texture2D>("cursor");
            int count = 0;
            for (int i = 0; i < ((_tileset.BlockTexture.Width / 32) / 3) + 1; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (_buttons.Count < _tileset.BlockTexture.Width / 32)
                    {
                        _buttons.Add(new Button(new Rectangle(704 + (32 * j), 32 * i, 32, 32), _tileset.BlockTexture, count));
                        count += 1;
                    }
                }
            }
            _otherButtons.Add(new Button(new Rectangle(704, 320, 32, 32), _boundingbox, 0));
            _otherButtons[0].Text = "Collidable";
            _otherButtons.Add(new Button(new Rectangle(736, 320, 32, 32), _unCollidable, 0));
            _otherButtons[1].Text = "UnCollidable";
            _otherButtons.Add(new Button(new Rectangle(704, 352, 32, 32), _player, 0));
            _otherButtons[2].Text = "Player";
            _otherButtons.Add(new Button(new Rectangle(736, 352, 32, 32), _npc, 0));
            _otherButtons[3].Text = "Npc";
            _otherButtons.Add(new Button(new Rectangle(768, 352, 32, 32), _door, 0));
            _otherButtons[4].Text = "Door";
            _otherButtons.Add(new Button(new Rectangle(768,320,32,32),_delete,0));
            _otherButtons[5].Text = "Delete";
            _otherButtons.Add(new Button(new Rectangle(800, 352, 32, 32), _cursor, 0));
            _otherButtons[6].Text = "Select";

            _font = Content.Load<SpriteFont>("SpriteFont1");
            Tilemap = new TileMap(Tilemapwidth, Tilemapheight);
            _form.Tilemap = Tilemap;
            _small = Content.Load<SpriteFont>("small");
            _player = Content.Load<Texture2D>("FighterFront");
            _door = Content.Load<Texture2D>("SHDoor2");
            _npc = Content.Load<Texture2D>("Seller");
            _bold = Content.Load<SpriteFont>("Bold");
            _oldbuttonindex = Buttonindex;
        }

        protected override void UnloadContent()
        {

        }
        protected override void Update(GameTime gameTime)
        {
            if (_oldbuttonindex != Buttonindex)
            {
                _state = (MouseStates)Buttonindex;
                _oldbuttonindex = Buttonindex;
            }
            _mousepositions = Mouse.GetState();
            _keys = Keyboard.GetState();
            // Allows the game to exit
            if (_keys.IsKeyDown(KeyStrokes.Escape))
                Exit();
            if (_keys.IsKeyDown(KeyStrokes.P) && _oldkeys.IsKeyUp(KeyStrokes.P))
                foreach (List<TileAdvanced> tileList in Tilemap.Tilemap)
                    for (int j = 0; j < tileList.Count; j++)
                        tileList[j] = new TileAdvanced(4,false);
            if (IsActive && MouseinTile(_mousepositions, new Rectangle(0, 0, 800, 480)))
            {
                if (_mousepositions.LeftButton == ButtonState.Pressed)
                {
                    if (_oldmouse.LeftButton == ButtonState.Released)
                    {
                        foreach (Button button in _buttons.Where(button => MouseinTile(_mousepositions, button.Rect)))
                        {
                            _sourceX = button.SourceX;
                            _state = MouseStates.Brush;
                        }
                        if (_state == MouseStates.Select)
                        {
                            foreach (Door door in Doors)
                            {
                                if (MouseinTile(_mousepositions, door.Bounds))
                                {
                                    _form.DisplayDoor(door.Bounds.Width, door.Bounds.Height);
                                }
                            }
                        }
                        foreach (Button button in _otherButtons.Where(button => MouseinTile(_mousepositions, button.Rect)))
                            if (button.Text == "Collidable")
                                _state = MouseStates.Collide;
                            else if (button.Text == "UnCollidable")
                                _state = MouseStates.Uncollidable;
                            else if (button.Text == "Player")
                                _state = MouseStates.Player;
                            else if (button.Text == "Npc")
                                _state = MouseStates.Npc;
                            else if (button.Text == "Door")
                                _state = MouseStates.Door;
                            else if (button.Text == "Delete")
                                _state = MouseStates.Delete;
                            else if (button.Text == "Select")
                                _state = MouseStates.Select;
                    }
                    int endx = 22 + _camerax / Tilelength;
                    int endy = 16 + _cameray / Tilelength;
                    for (int x = _camerax / Tilelength; x < MathHelper.Clamp(endx, 0, Tilemapwidth); x++)
                    {
                        for (int y = _cameray / Tilelength; y < MathHelper.Clamp(endy, 0, Tilemapheight); y++)
                        {
                            _tilerect = new Rectangle(
                            (x * Tilewidth) - _camerax,
                            (y * Tileheight) - _cameray,
                            Tilewidth,
                            Tileheight);
                            if (MouseinTile(_mousepositions, _tilerect))
                            {
                                switch (_state)
                                {
                                    case MouseStates.Collide:
                                        Tilemap.Tilemap[y][x].Collidable = true;
                                        break;
                                    case MouseStates.Brush:
                                        Tilemap.Tilemap[y][x].SourceX = _sourceX;
                                        break;
                                    case MouseStates.Uncollidable:
                                        Tilemap.Tilemap[y][x].Collidable = false;
                                        break;
                                    default:
                                        if (_state == MouseStates.Player && _oldmouse.LeftButton == ButtonState.Released)
                                        {
                                            if (Players.Count < 4)
                                                Players.Add(new Vector2(_mousepositions.X - 20 + _camerax, _mousepositions.Y - 20 + _cameray));
                                        }
                                        else if (_state == MouseStates.Npc && _oldmouse.LeftButton == ButtonState.Released)
                                            Npcs.Add(new Vector2(_mousepositions.X - 20 + _camerax, _mousepositions.Y - 20 + _cameray));
                                        else if (_state == MouseStates.Door && _oldmouse.LeftButton == ButtonState.Released)
                                        {
                                            Doors.Add(new Door(new Rectangle(_mousepositions.X - 20 + _camerax, _mousepositions.Y - 20 + _cameray, 32, 32)));
                                            OpenFileDialog saver = new OpenFileDialog();
                                            saver.ShowDialog();
                                            Doorfilenames.Add(saver.FileName);
                                            if (saver.FileName == "")
                                                Doors.RemoveAt(Doors.Count - 1);
                                            _form.NewDoor();
                                        }
                                        else if (_state == MouseStates.Delete)
                                        {
                                            for (int i = 0; i < Npcs.Count; i++)
                                                if (MouseinTile(_mousepositions, new Rectangle((int)Npcs[i].X, (int)Npcs[i].Y, 32, 32)))
                                                    Npcs.RemoveAt(i);
                                            for (int i = 0; i < Doors.Count; i++)
                                                if (MouseinTile(_mousepositions, new Rectangle(Doors[i].Bounds.X, Doors[i].Bounds.Y, 32, 32)))
                                                    Doors.RemoveAt(i);
                                            for (int i = 0; i < Players.Count; i++)
                                                if (MouseinTile(_mousepositions, new Rectangle((int)Players[i].X, (int)Players[i].Y, 32, 32)))
                                                    Players.RemoveAt(i);
                                        }
                                        break;
                                }
                            }
                        }
                    }
                }


                _scrollspeed = _keys.IsKeyDown(KeyStrokes.LeftShift) ? 5 : 3;
                if (_keys.IsKeyDown(KeyStrokes.A))
                    _camerax -= _scrollspeed;
                if (_keys.IsKeyDown(KeyStrokes.D))
                    _camerax += _scrollspeed;
                if (_keys.IsKeyDown(KeyStrokes.W))
                    _cameray -= _scrollspeed;
                if (_keys.IsKeyDown(KeyStrokes.S))
                    _cameray += _scrollspeed;
                if (_camerax < 0)
                    _camerax = 0;
                if (_cameray < 0)
                    _cameray = 0;

                if (_keys.IsKeyDown(KeyStrokes.E) && _oldkeys.IsKeyUp(KeyStrokes.E))
                    AddColumnTilemap();
                if (_keys.IsKeyDown(KeyStrokes.Q) && _oldkeys.IsKeyUp(KeyStrokes.Q))
                    RemoveColumn();
                if (_keys.IsKeyDown(KeyStrokes.C) && _oldkeys.IsKeyUp(KeyStrokes.C))
                    AddRowTilemap();
                if (_keys.IsKeyDown(KeyStrokes.Z) && _oldkeys.IsKeyUp(KeyStrokes.Z))
                    RemoveRow();
                if (_keys.IsKeyDown(KeyStrokes.Space) && _oldkeys.IsKeyUp(KeyStrokes.Space))
                    BreakFunction();
                _oldkeys = _keys;
                _oldmouse = _mousepositions;
            }
            base.Update(gameTime);
        }
        //Was looking to make doors more dynamic but not sure what to do. Might pass text through that says what type of door it is.
        //(invisible boundingbox, or actually drawn door based on tilemap).
        //Add more detail in NPC(Seller,townsperson) Might come back to it once I have defined their logic in game.
        //
        //Consider One Huge Maps(I'm really considering it)
        //
        public void BreakFunction()
        {
            Tilemap.Tilemap[100].Add(new TileAdvanced());
        }
        public void RemoveColumn()
        {
            if (Tilemapwidth <= 0) return;
            for (int i = 0; i < Tilemapheight; i++)
                Tilemap.Tilemap[i].RemoveAt(Tilemap.Tilemap[i].Count - 1);
            Tilemapwidth -= 1;
        }
        public void RemoveRow()
        {
            if (Tilemapheight <= 0) return;
            Tilemap.Tilemap.RemoveAt(Tilemap.Tilemap.Count - 1);
            Tilemapheight -= 1;
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
        public void AddColumnTilemap()
        {
            foreach (List<TileAdvanced> t in Tilemap.Tilemap)
                t.Add(new TileAdvanced());
            Tilemapwidth += 1;
        }

        public void AddRowTilemap()
        {
            Tilemap.Tilemap.Add(new List<TileAdvanced>());
            for (int i = 0; i < Tilemapwidth; i++)
                Tilemap.Tilemap[Tilemapheight].Add(new TileAdvanced(0));
            Tilemapheight += 1;
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            _spriteBatch.Begin(SpriteSortMode.Texture);
            //26x16
            int endx = 22 + _camerax / Tilelength;
            int endy = 16 + _cameray / Tilelength;
            //DRAW MAP
            for (int x = _camerax / Tilelength; x < MathHelper.Clamp(endx, 0, Tilemapwidth); x++)
            {
                for (int y = _cameray / Tilelength; y < MathHelper.Clamp(endy, 0, Tilemapheight); y++)
                {
                    if (y < Tilemap.Tilemap.Count && x < Tilemap.Tilemap[y].Count)
                    {
                        //Section of image to draw
                        _tilesourcerect = new Rectangle(Tilemap.Tilemap[y][x].SourceX * Tilelength, 0, Tilewidth, Tileheight);


                        //Destination Rectangle
                        _tilerect = new Rectangle(
                            (x * Tilewidth) - _camerax,
                            (y * Tileheight) - _cameray,
                            Tilewidth,
                            Tileheight);
                        _spriteBatch.Draw(_tileset.BlockTexture, _tilerect, _tilesourcerect,
                            Tilemap.Tilemap[y][x].Collidable ? Color.Red : Color.White);
                    }
                    if (MouseinTile(_mousepositions, _tilerect))
                    {
                        if (_state == MouseStates.Brush)
                            _spriteBatch.Draw(_tileset.BlockTexture, _tilerect, new Rectangle(_sourceX * 32, 0, 32, 32), new Color(255, 255, 255, 200));
                        if (_state == MouseStates.Collide)
                            _spriteBatch.Draw(_boundingbox, _tilerect, Color.White);
                        if (_state == MouseStates.Uncollidable)
                            _spriteBatch.Draw(_unCollidable, _tilerect, Color.White);
                    }
                    _spriteBatch.DrawString(_small, x + "," + y, new Vector2(_tilerect.X, _tilerect.Y), Color.White);
                }
            }
            for (int i = 0; i < _buttons.Count; i++)
            {
                _spriteBatch.Draw(_tileset.BlockTexture, _buttons[i].Rect, new Rectangle(i * 32, 0, 32, 32), Color.White);
                if (MouseinTile(_mousepositions, _buttons[i].Rect))
                    _spriteBatch.Draw(_boundingbox, _buttons[i].Rect, Color.White);
            }
            foreach (Button button in _otherButtons)
            {
                switch (button.Text)
                {
                    case "Collidable":
                        _spriteBatch.Draw(_boundingbox, button.Rect, Color.White);
                        break;
                    case "UnCollidable":
                        _spriteBatch.Draw(_unCollidable, button.Rect, Color.White);
                        break;
                    case "Player":
                        _spriteBatch.Draw(_player, button.Rect, Color.White);
                        break;
                    case "Npc":
                        _spriteBatch.Draw(_npc, button.Rect, Color.White);
                        break;
                    case "Door":
                        _spriteBatch.Draw(_door, button.Rect, Color.White);
                        break;
                    case "Delete":
                        _spriteBatch.Draw(_delete, button.Rect, Color.White);
                        break;
                    case "Select":
                        _spriteBatch.Draw(_cursor, button.Rect, Color.White);
                        break;
                }
            }
            for (int i = 0; i < Players.Count; i++)
                _spriteBatch.Draw(_player, new Vector2(Players[i].X-_camerax,Players[i].Y-_cameray), Color.White);
            for (int i = 0; i < Npcs.Count; i++)
                _spriteBatch.Draw(_npc, new Vector2(Npcs[i].X - _camerax, Npcs[i].Y - _cameray), Color.White);
            foreach (Door door in Doors)
                _spriteBatch.Draw(_door, new Vector2(door.Bounds.X - _camerax, door.Bounds.Y - _cameray), Color.White);
            if (_state == MouseStates.Player)
                _spriteBatch.Draw(_player, new Vector2(_mousepositions.X-20,_mousepositions.Y-20), Color.White);
            if (_state == MouseStates.Npc)
                _spriteBatch.Draw(_npc, new Vector2(_mousepositions.X - 20, _mousepositions.Y - 20), Color.White);
            if (_state == MouseStates.Door)
                _spriteBatch.Draw(_door, new Vector2(_mousepositions.X - 20, _mousepositions.Y - 20), Color.White);
            if (_state == MouseStates.Delete)
                _spriteBatch.Draw(_delete, new Vector2(_mousepositions.X - 20, _mousepositions.Y - 20), Color.White);
            if (_state == MouseStates.Select)
                _spriteBatch.Draw(_cursor, new Vector2(_mousepositions.X - 20, _mousepositions.Y - 20), Color.White);
            _spriteBatch.DrawString(_bold," Width: " + Tilemapwidth + " Height: " + Tilemapheight, new Vector2(10, 0), Color.Red);
            _spriteBatch.DrawString(_font, Text, new Vector2(200, 200), Color.White);
            //spriteBatch.Draw(boundingbox, otherButtons[6].Rect, Color.White);
            //spriteBatch.DrawString(font,"old buttong index: " + oldbuttonindex + " button index: " + buttonindex.ToString(), new Vector2(300, 300), Color.Red);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
