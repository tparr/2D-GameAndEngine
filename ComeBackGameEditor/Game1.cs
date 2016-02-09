using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ButtonState = Microsoft.Xna.Framework.Input.ButtonState;
using KeyStrokes = Microsoft.Xna.Framework.Input.Keys;
using Drawing = System.Drawing;
using System.Xml;
using System.Xml.Linq;
namespace ComeBackGameEditor
{
    public class Game1 : Game
    {
        public enum MapState
        {
            BaseMap,
            Map
        };

        private const int Tilelength = 32;
        private const int Tilewidth = 32;
        private const int Tileheight = 32;
        private const string Text = "";
        public static TileMap Tilemap;
        public static TileMap BaseTileMap;
        public static List<Vector2> Players = new List<Vector2>();
        public static List<Vector2> Npcs = new List<Vector2>();
        public static List<Door> Doors = new List<Door>();
        public static List<string> Doorfilenames = new List<string>();
        public static int Buttonindex = 0;
        public static MapState SelectedMap = MapState.Map;
        private SpriteFont _bold;
        private Texture2D _boundingbox;
        private List<MyButton> _buttons = new List<MyButton>();
        private int _camerax;
        private int _cameray;
        private Texture2D _cursor;
        private Texture2D _delete;
        private Texture2D _door;
        private SpriteFont _font;
        private Form1 _form;
        private KeyboardState _keys;
        private MouseState _mousepositions;
        private Texture2D _npc;
        private int _oldbuttonindex;
        private KeyboardState _oldkeys;
        private MouseState _oldmouse;
        private Texture2D _player;
        private int _scrollspeed = 3;
        private SpriteFont _small;
        private int _sourceX;
        private SpriteBatch _spriteBatch;
        private MouseStates _state;
        private Rectangle _tilerect;
        private Block _tileset;
        private Rectangle _tilesourcerect;
        private Texture2D _unCollidable;
        private bool showingHelp;
        public Game1()
        {
            var graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            _form = new Form1 {Tilemap = Tilemap, BaseTileMap = BaseTileMap};
            _form.Show();
            graphics.PreferredBackBufferWidth = 900;
            graphics.PreferredBackBufferHeight = 500;
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
            //Display TileSet
            var count = 0;
            var ymod = 0;
            var xmod = 0;
            for (var i = 0; i < _tileset.BlockTexture.Width/32; i++)
            {
                if (_buttons.Count%6 == 0)
                {
                    ymod++;
                    xmod = 0;
                }
                _buttons.Add(new MyButton(new Rectangle(704 + (32*xmod), 32*ymod + 64, 32, 32), _tileset.BlockTexture,
                    count));
                count += 1;
                xmod++;
            }
            _font = Content.Load<SpriteFont>("SpriteFont1");
            Tilemap = new TileMap(10, 10);
            BaseTileMap = new TileMap(10, 10);
            _form.Tilemap = Tilemap;
            _form.BaseTileMap = BaseTileMap;
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
                _state = (MouseStates) Buttonindex;
                _oldbuttonindex = Buttonindex;
            }
            _mousepositions = Mouse.GetState();
            _keys = Keyboard.GetState();

            IsMouseVisible = _state != MouseStates.Select;

            //Exits the Editor
            if (_keys.IsKeyDown(KeyStrokes.Escape))
            {
                _form.Close();
                Exit();
            }
            if (_keys.IsKeyDown(KeyStrokes.Enter) && _oldkeys.IsKeyUp(KeyStrokes.Enter))
                showingHelp = !showingHelp;
            if (_keys.IsKeyDown(KeyStrokes.D1))
                SelectedMap = MapState.Map;
            if (_keys.IsKeyDown(KeyStrokes.D2))
                SelectedMap = MapState.BaseMap;
            if (_keys.IsKeyDown(KeyStrokes.L))
                _state = MouseStates.Select;
            if (_keys.IsKeyDown(KeyStrokes.X) && _oldkeys.IsKeyUp(KeyStrokes.X))
                if ((int) _state + 1 == Enum.GetValues(typeof (MouseStates)).GetLength(0))
                    _state = 0;
                else
                    _state++;

            //Fill Map with Selected Tile (Key.P)
            MapFill(SelectedMap == MapState.BaseMap ? BaseTileMap : Tilemap);
            //Check Mouse input
            if (IsActive && MouseinTile(_mousepositions, new Rectangle(0, 0, 900, 480)))
            {
                if (_mousepositions.LeftButton == ButtonState.Pressed)
                {
                    if (_oldmouse.LeftButton == ButtonState.Released)
                    {
                        foreach (var button in _buttons)
                        {
                            if (MouseinTile(_mousepositions, button.Rect))
                            {
                                _sourceX = button.SourceX;
                                _state = MouseStates.Brush;
                            }
                        }
                        if (_state == MouseStates.Select)
                        {
                            foreach (var door in Doors.Where(door => MouseinTile(_mousepositions, door.Bounds)))
                            {
                                _form.DisplayDoor(door.Bounds.Width, door.Bounds.Height);
                            }
                            foreach (
                                var door in
                                    Doors.Where(
                                        door => MouseinTile(_mousepositions, door.Bounds) && door.Selected == false))
                            {
                                if (door.Selected)
                                {
                                    door.Selected = false;
                                }
                                else
                                {
                                    door.Selected = true;
                                    break;
                                }
                            }
                        }
                    }
                    if (SelectedMap == MapState.BaseMap)
                        MouseMapUpdate(ref BaseTileMap);
                    else
                        MouseMapUpdate(ref Tilemap);
                }
                else
                {
                    foreach (var door in Doors.Where(door => door.Selected))
                    {
                        door.Selected = false;
                    }
                }
                if (_state == MouseStates.Select)
                    foreach (var door in Doors.Where(door => door.Selected))
                    {
                        door.Bounds.X = _mousepositions.X - 20 - _camerax;
                        door.Bounds.Y = _mousepositions.Y - 20 - _cameray;
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
                    AddColumnTilemap(SelectedMap == MapState.BaseMap ? BaseTileMap : Tilemap);
                if (_keys.IsKeyDown(KeyStrokes.Q) && _oldkeys.IsKeyUp(KeyStrokes.Q))
                    RemoveColumn(SelectedMap == MapState.BaseMap ? BaseTileMap : Tilemap);
                if (_keys.IsKeyDown(KeyStrokes.C) && _oldkeys.IsKeyUp(KeyStrokes.C))
                    AddRowTilemap(SelectedMap == MapState.BaseMap ? BaseTileMap : Tilemap);
                if (_keys.IsKeyDown(KeyStrokes.Z) && _oldkeys.IsKeyUp(KeyStrokes.Z))
                    RemoveRow(SelectedMap == MapState.BaseMap ? BaseTileMap : Tilemap);
                if (_keys.IsKeyDown(KeyStrokes.Space) && _oldkeys.IsKeyUp(KeyStrokes.Space))
                    BreakFunction(SelectedMap == MapState.BaseMap ? BaseTileMap : Tilemap);
                _oldkeys = _keys;
                _oldmouse = _mousepositions;
            }
            base.Update(gameTime);
        }

        //(invisible boundingbox, or actually drawn door based on tilemap).
        //Add more detail in NPC(Seller,townsperson) Might come back to it once I have defined their logic in game.
        //
        //Consider One Huge Maps(I'm really considering it)
        //
        public void MouseMapUpdate(ref TileMap map)
        {
            var endx = 22 + _camerax / Tilelength;
            var endy = 16 + _cameray / Tilelength;
            for (var x = _camerax / Tilelength; x < MathHelper.Clamp(endx, 0, map.Width); x++)
            {
                for (var y = _cameray / Tilelength; y < MathHelper.Clamp(endy, 0, map.Height); y++)
                {
                    _tilerect = new Rectangle(
                        (x * Tilewidth) - _camerax,
                        (y * Tileheight) - _cameray,
                        Tilewidth,
                        Tileheight);
                    if (!MouseinTile(_mousepositions, _tilerect)) continue;
                    switch (_state)
                    {
                        case MouseStates.Collide:
                            map.Tilemap[y][x].Collidable = true;
                            break;
                        case MouseStates.Brush:
                            map.Tilemap[y][x].SourceX = _sourceX;
                            break;
                        case MouseStates.Uncollidable:
                            map.Tilemap[y][x].Collidable = false;
                            break;
                        default:
                            if (_state == MouseStates.Player && _oldmouse.LeftButton == ButtonState.Released)
                            {
                                if (Players.Count < 4)
                                    Players.Add(new Vector2(_mousepositions.X - 20 + _camerax,
                                        _mousepositions.Y - 20 + _cameray));
                            }
                            else if (_state == MouseStates.Npc && _oldmouse.LeftButton == ButtonState.Released)
                                Npcs.Add(new Vector2(_mousepositions.X - 20 + _camerax,
                                    _mousepositions.Y - 20 + _cameray));
                            else if (_state == MouseStates.Door && _oldmouse.LeftButton == ButtonState.Released)
                            {
                                string DoorName = String.Empty;

                                if (InputBox("Name", "Enter Level Name", ref DoorName) == DialogResult.OK)
                                    Doors.Add(
                                    new Door(new Rectangle(_mousepositions.X - 20 + _camerax,
                                        _mousepositions.Y - 20 + _cameray, 32, 32), DoorName));
                                _form.NewDoor();
                            }
                            else if (_state == MouseStates.Delete)
                            {
                                for (var i = 0; i < Npcs.Count; i++)
                                    if (MouseinTile(_mousepositions,
                                        new Rectangle((int)Npcs[i].X, (int)Npcs[i].Y, 32, 32)))
                                        Npcs.RemoveAt(i);
                                for (var i = 0; i < Doors.Count; i++)
                                    if (MouseinTile(_mousepositions,
                                        new Rectangle(Doors[i].Bounds.X, Doors[i].Bounds.Y, 32, 32)))
                                        Doors.RemoveAt(i);
                                for (var i = 0; i < Players.Count; i++)
                                    if (MouseinTile(_mousepositions,
                                        new Rectangle((int)Players[i].X, (int)Players[i].Y, 32, 32)))
                                        Players.RemoveAt(i);
                            }
                            break;
                    }
                }
            }
        }

        public void BreakFunction(TileMap map)
        {
            map.Tilemap[-1].Add(new TileAdvanced());
        }

        public static void RemoveColumn(TileMap map)
        {
            if (map.Width <= 0) return;
            for (var i = 0; i < map.Height; i++)
                map.Tilemap[i].RemoveAt(map.Tilemap[i].Count - 1);
            map.Width -= 1;
        }

        public static void RemoveRow(TileMap map)
        {
            if (map.Height <= 0) return;
            map.Tilemap.RemoveAt(map.Tilemap.Count - 1);
            map.Height -= 1;
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

        public static void AddColumnTilemap(TileMap map)
        {
            foreach (var t in map.Tilemap)
                t.Add(new TileAdvanced());
            map.Width += 1;
        }

        public static void AddRowTilemap(TileMap map)
        {
            map.Tilemap.Add(new List<TileAdvanced>());
            for (var i = 0; i < map.Width; i++)
                map.Tilemap[map.Height].Add(new TileAdvanced(0));
            map.Height += 1;
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            _spriteBatch.Begin();

            //DRAW MAP
            DrawMap(SelectedMap == MapState.BaseMap ? BaseTileMap : Tilemap);
            for (var i = 0; i < _buttons.Count; i++)
            {
                _spriteBatch.Draw(_tileset.BlockTexture, _buttons[i].Rect, new Rectangle(i*32, 0, 32, 32), Color.White);
                if (MouseinTile(_mousepositions, _buttons[i].Rect))
                    _spriteBatch.Draw(_boundingbox, _buttons[i].Rect, Color.White);
            }
            for (var i = 0; i < Players.Count; i++)
                _spriteBatch.Draw(_player, new Vector2(Players[i].X - _camerax, Players[i].Y - _cameray), Color.White);
            for (var i = 0; i < Npcs.Count; i++)
                _spriteBatch.Draw(_npc, new Vector2(Npcs[i].X - _camerax, Npcs[i].Y - _cameray), Color.White);
            foreach (var door in Doors)
            {
                if (door.Selected)
                {
                    _spriteBatch.Draw(_door, CameraFix(door.Bounds), Color.Red);
                    _spriteBatch.Draw(_boundingbox, CameraFix(door.Bounds), Color.White);
                }
                else
                {
                    _spriteBatch.Draw(_door, CameraFix(door.Bounds), Color.White);
                }
            }
            if (_state == MouseStates.Player)
                _spriteBatch.Draw(_player, MouseFix(_mousepositions), Color.White);
            if (_state == MouseStates.Npc)
                _spriteBatch.Draw(_npc, MouseFix(_mousepositions), Color.White);
            if (_state == MouseStates.Door)
                _spriteBatch.Draw(_door, MouseFix(_mousepositions), Color.White);
            if (_state == MouseStates.Delete)
                _spriteBatch.Draw(_delete, MouseFix(_mousepositions), Color.White);
            if (_state == MouseStates.Select)
                _spriteBatch.Draw(_cursor, new Vector2(_mousepositions.X - 9, _mousepositions.Y), Color.White);
            if (SelectedMap == MapState.BaseMap)
            {
                _spriteBatch.DrawString(_bold, "Width: " + BaseTileMap.Width + " Height: " + BaseTileMap.Height,
                    new Vector2(700, 0), Color.Red);
                _spriteBatch.DrawString(_bold, "Layer: UpperLayer", new Vector2(700, 20), Color.Red);
            }
            else
            {
                _spriteBatch.DrawString(_bold, "Width: " + Tilemap.Width + " Height: " + Tilemap.Height,
                    new Vector2(700, 0), Color.Red);
                _spriteBatch.DrawString(_bold, "Layer: LowerLayer", new Vector2(700, 20), Color.Red);
            }
            _spriteBatch.DrawString(_bold, "Action: " + _state, new Vector2(700, 40), Color.Red);
            _spriteBatch.DrawString(_font, Text, new Vector2(200, 200), Color.White);
            _spriteBatch.DrawString(_font, _mousepositions.X + " " + _mousepositions.Y, new Vector2(300, 300), Color.White);
            if (showingHelp)
            {
                _spriteBatch.DrawString(_font, "Controls: ESC-(Exit)_L-(SELECT)_X-(ChangeAction)_Q-(RemoveColumn)_E-(AddColumn)_X-(RemoveRow)_C-(AddRow)",
                    new Vector2(0, 470), Color.Green);
            }
            _spriteBatch.DrawString(_font, "Show Help: Enter",
                    new Vector2(_form.Bounds.Width - 120, 60), Color.Green);
            //spriteBatch.Draw(boundingbox, otherButtons[6].Rect, Color.White);
            //spriteBatch.DrawString(font,"old buttong index: " + oldbuttonindex + " button index: " + buttonindex.ToString(), new Vector2(300, 300), Color.Red);
            _spriteBatch.End();
            base.Draw(gameTime);
        }

        public Vector2 MouseFix(MouseState position)
        {
            return new Vector2(position.X - 20, position.Y - 20);
        }

        public Vector2 CameraFix(Vector2 vect)
        {
            return new Vector2(vect.X - _camerax, vect.Y - _cameray);
        }

        public Rectangle CameraFix(Rectangle rect)
        {
            return new Rectangle(rect.X - _camerax, rect.Y - _cameray, rect.Width, rect.Height);
        }

        private void MapFill(TileMap map)
        {
            if (!_keys.IsKeyDown(KeyStrokes.P) || !_oldkeys.IsKeyUp(KeyStrokes.P)) return;
            foreach (var tileList in map.Tilemap)
                for (var j = 0; j < tileList.Count; j++)
                    tileList[j] = new TileAdvanced(_sourceX, false);
        }

        private void DrawMap(TileMap map)
        {
            //26x16
            var endx = 22 + _camerax/Tilelength;
            var endy = 16 + _cameray/Tilelength;
            for (var x = _camerax/Tilelength; x < MathHelper.Clamp(endx, _camerax/Tilelength, map.Width); x++)
            {
                for (var y = _cameray/Tilelength; y < MathHelper.Clamp(endy, _cameray/Tilelength, map.Height); y++)
                {
                    if (y < map.Tilemap.Count && x < map.Tilemap[y].Count)
                    {
                        //Section of image to draw
                        _tilesourcerect = new Rectangle(map.Tilemap[y][x].SourceX*Tilelength, 0, Tilewidth, Tileheight);


                        //Destination Rectangle
                        _tilerect = new Rectangle(
                            (x*Tilewidth) - _camerax,
                            (y*Tileheight) - _cameray,
                            Tilewidth,
                            Tileheight);
                        _spriteBatch.Draw(_tileset.BlockTexture, _tilerect, _tilesourcerect,
                            map.Tilemap[y][x].Collidable ? Color.Red : Color.White);
                    }
                    if (MouseinTile(_mousepositions, _tilerect))
                    {
                        if (_state == MouseStates.Brush)
                            _spriteBatch.Draw(_tileset.BlockTexture, _tilerect, new Rectangle(_sourceX*32, 0, 32, 32),
                                new Color(255, 255, 255, 200));
                        if (_state == MouseStates.Collide)
                            _spriteBatch.Draw(_boundingbox, _tilerect, Color.White);
                        if (_state == MouseStates.Uncollidable)
                            _spriteBatch.Draw(_unCollidable, _tilerect, Color.White);
                    }
                    _spriteBatch.DrawString(_small, x + "," + y, new Vector2(_tilerect.X, _tilerect.Y), Color.White);
                }
            }
        }

        public DialogResult InputBox(string title, string promptText, ref string value)
        {
            Form form = new Form();
            Label label = new Label();
            TextBox textBox = new TextBox();
            Button buttonOk = new Button();
            Button buttonCancel = new Button();

            form.Text = title;
            label.Text = promptText;
            textBox.Text = value;

            buttonOk.Text = "OK";
            buttonCancel.Text = "Cancel";
            buttonOk.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;

            label.SetBounds(9, 20, 600, 13);
            textBox.SetBounds(12, 36, 372, 20);
            buttonOk.SetBounds(228, 72, 75, 23);
            buttonCancel.SetBounds(309, 72, 75, 23);

            label.AutoSize = false;
            textBox.Anchor = textBox.Anchor | AnchorStyles.Right;
            buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            form.ClientSize = new Drawing.Size(396, 107);
            form.Controls.AddRange(new Control[] { label, textBox, buttonOk, buttonCancel });
            form.ClientSize = new Drawing.Size(MathHelper.Clamp(300, 0, label.Right + 10), form.ClientSize.Height);
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.AcceptButton = buttonOk;
            form.CancelButton = buttonCancel;

            DialogResult dialogResult = form.ShowDialog();
            value = textBox.Text;
            return dialogResult;
        }

        private enum MouseStates
        {
            Collide,
            Uncollidable,
            Player,
            Npc,
            Door,
            Delete,
            Select,
            Brush
        }
    }
}