using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Microsoft.Xna.Framework.Content;
using System.Xml;
using System.Xml.Linq;
namespace _2D_Game
{
    public class World
    {
        private TileMap _map;
        private List<Thing> _entities;
        private int _tilelength = 32;
        private Rectangle _bottom = new Rectangle(0, 350, 800, 150);
        private Rectangle _right = new Rectangle(600, 0, 200, 480);
        private Rectangle _left = new Rectangle(0, 0, 200, 480);
        private Rectangle _top = new Rectangle(0, 0, 800, 150);
        Texture2D _tileset;
        public int CameraX
        { get; private set; }
        public int CameraY
        { get; private set; }
        ContentManager _content;
        bool paused;
        /// <summary>
        /// Initializes a new instance of the <see cref="World"/> class.
        /// </summary>
        public World()
        {
            _entities = new List<Thing>();
            _map = new TileMap();
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="World"/> class.
        /// </summary>
        /// <param name="LevelFilePath">The level file path.</param>
        public World(string LevelName)
        {
            _map = new TileMap();
            NewestLevelLoad(LevelName);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="World"/> class.
        /// </summary>
        /// <param name="LevelFilePath">The level file path.</param>
        /// <param name="screenrect">The screenrect.</param>
        /// <param name="TileSet">The tile set.</param>
        public World(string LevelName, ContentManager manager)
            : this(LevelName)
        {
            _content = new ContentManager(manager.ServiceProvider);
            _content.RootDirectory = "Content";
            _tileset = _content.Load<Texture2D>("tileset");
        }
        /// <summary>
        /// Loads the new level.
        /// </summary>
        /// <param name="LevelFilePath">The level file path.</param>
        /// <param name="TileSet">The tile set.</param>
        public void LoadNewLevel(string LevelName, ContentManager manager)
        {
            _content = new ContentManager(manager.ServiceProvider);
            _content.RootDirectory = String.Format("Content/{0}",LevelName);
            _tileset = _content.Load<Texture2D>("tileset");
            NewestLevelLoad(LevelName);
        }
        /// <summary>
        /// Adds an entity.
        /// </summary>
        /// <param name="added">The added.</param>
        public void AddEntity(Thing added)
        {
            _entities.Add(added);
        }
        /// <summary>
        /// Updates this World.
        /// </summary>
        public void Update()
        {
            //Update Camera
            CameraUpdate();
            //Update Players
            PlayerUpdates();
            //enemies AI
            enemies_AI();
            foreach (Npc npc in _entities.Where(thing => IsSameOrSubclass(typeof(Npc),thing.GetType())))
                npc.Act();
            //Update Bullets
            UpdateBullets();
        }
        /// <summary>
        /// Draws the current Game World.
        /// </summary>
        /// <param name="sb">The sb.</param>
        /// <param name="boundingbox">The boundingbox.</param>
        /// <param name="f">The f.</param>
        public void Draw(SpriteBatch sb, Texture2D boundingbox, SpriteFont f)
        {
            //Draw Map First
            DrawMap(sb);
            //Draw all things that need to be sorted by depth
            var depthSortedEntities = _entities.OrderBy(rect => rect.Feetrect.Bottom).ToList();
            for (int i = 0; i < depthSortedEntities.Count; i++)
            {
                var entity = depthSortedEntities[i];
                if ((entity.GetType() == typeof(Fighter)))
                    ((Fighter)entity).Draw(sb, f, boundingbox, this);
                else if (entity.GetType() == typeof(Seller))
                    ((Seller)entity).Draw(sb, f, boundingbox, false, this);
                else if (entity.GetType() == typeof(Dragon))
                    ((Dragon)entity).Draw(sb, f, false, this);
                else if (entity.GetType() == typeof(HealthPotion))
                    ((HealthPotion)entity).Draw(sb, this);
                else if (entity.GetType() == typeof(SmallHealthPotion))
                    ((SmallHealthPotion)entity).Draw(sb, this);
                else if (entity.GetType() == typeof(LargeHealthPotion))
                    ((LargeHealthPotion)entity).Draw(sb, this);
                else if (entity.GetType() == typeof(ManaPotion))
                    ((ManaPotion)entity).Draw(sb, this);
                else if (entity.GetType() == typeof(Mage))
                    ((Mage)entity).Draw(sb, f, boundingbox, this);
                else if (entity.GetType() == typeof(Archer))
                    ((Archer)entity).Draw(sb, f, boundingbox, this);
                else if (entity.GetType() == typeof(Npc))
                    ((Npc)entity).Draw(sb, f, boundingbox, true, this);
                else if (entity.GetType() == typeof(Chest))
                    ((Chest)entity).Draw(sb);
                else if (entity.GetType() == typeof(Exp))
                    ((Exp)entity).Draw(sb, this);
                else if (entity.GetType() == typeof(Projectile))
                    ((Projectile)entity).Draw(sb,CameraX,CameraY);
            }
            depthSortedEntities.Clear();
            //Draw Player Huds
            foreach (Player player in _entities.Where(thing => thing.GetType() == typeof(Player)))
            {
                if (player.Alive)
                    player.Hud.Draw(sb, f, (int)player.Playerindex);
            }
            //Draw extra things such as doors and inventories
            foreach (Thing thing in _entities.Where(thing => thing.GetType() == typeof(Door)))
            {
                if (thing.GetType() == typeof(Door))
                    sb.Draw(boundingbox, ((Door)thing).Rect, Color.Red);
            }
        }
        public void UpdateBullets()
        {
            for (int i = 0; i < _entities.Count; i++)
            {
                if (_entities[i].GetType() == typeof(Projectile))
                {
                    ((Projectile)_entities[i]).Update(CameraX, CameraY, paused);
                    if (((Projectile)_entities[i]).IsActive == false)
                        _entities.RemoveAt(i);
                }
            }
        }
        /// <summary>
        /// Updates Enemy AI
        /// </summary>
        public void enemies_AI()
        {
            //Finds closest player
            Player firstPlayer = (Player)_entities.Find(thing => thing.GetType() == typeof(Player));
            for (var i = 0; i < _entities.Count; i++)
            {
                if (_entities.GetType() != typeof(Enemy) || !((Enemy)_entities[i]).IsActive) continue;
                Enemy currEnemy = (Enemy)_entities[i];
                var xdiff = Math.Abs(firstPlayer.Position.X - currEnemy.Position.X);
                var ydiff = Math.Abs(firstPlayer.Position.Y - currEnemy.Position.Y);
                var diff = xdiff + ydiff;
                foreach (Player player in _entities.Where(thing => thing.GetType() == typeof(Player)))
                {
                    if (!(diff >
                          (Math.Abs(player.Position.X - currEnemy.Position.X) +
                           Math.Abs(player.Position.Y - currEnemy.Position.Y)))) continue;
                    firstPlayer = player;
                    diff = (Math.Abs(player.Position.X - currEnemy.Position.X) +
                            Math.Abs(player.Position.Y - currEnemy.Position.Y));
                }
                ((Enemy)_entities[i]).Act(firstPlayer.Testbox.ToRectangle(),
                    (int)currEnemy.Position.X, (int)currEnemy.Position.Y, i);
            }
        }
        /// <summary>
        /// Updates the Players.
        /// </summary>
        public void PlayerUpdates()
        {
            //touchedTiles = new List<Point>();
            //Check Player Stuffz
            foreach (Player player in _entities.Where(player => IsSameOrSubclass(typeof(Player),player.GetType())))
            {
                if (player.Alive)
                {
                    //Check Fighter attacks
                    if (player.GetType() == typeof(Fighter))
                    {
                        if (player.Attackmode)
                        {
                            foreach (Enemy enemy in _entities.Where(enemy => IsSameOrSubclass(typeof(Enemy),enemy.GetType())))
                            {
                                if (!enemy.IsActive) continue;
                                if (!((Fighter)player).AttackRectangle.Intersects(enemy.Rectangle)) continue;
                                if (enemy.Ishurting == false)
                                    enemy.Hurt(10, player.Velocityx, player.Velocityy);
                            }
                        }
                    }
                    //Check Mage attack
                    else if (player.GetType() == typeof(Mage))
                    {

                    }
                    else if (player.GetType() == typeof(Archer))
                    {
                        //foreach (var arrowProjectile in ((Archer)player).Arrows)
                        //{
                        //    //if (!arrowProjectile.IsActive) continue;
                        //    foreach (Enemy enemy in _entities.Where(thing => IsSameOrSubclass(typeof(Enemy),thing.GetType())))
                        //    {
                        //        if (!enemy.IsActive) continue;
                        //        if ((arrowProjectile).Rect.Intersects(enemy.Rectangle))
                        //            enemy.Hurt(100, (int)(arrowProjectile).Direction.X,
                        //                (int)(arrowProjectile).Direction.Y);
                        //    }
                        //}
                    }
                    //enemies PLAYER COLLISION
                    if (!player.Ishurting)
                    {
                        foreach (Enemy enemy in _entities.Where(thing => IsSameOrSubclass(typeof(Enemy), thing.GetType())))
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
                    //Update huds
                    if (IsSameOrSubclass(typeof(Player),player.GetType()))
                        player.UpdateHud();
                }
                //checkTilesUnder(player.Testbox);
                //for (var i = 0; i < Experiencelist.Count; i++)
                //    if (player.Testbox.Intersects(Experiencelist[i].HitBox))
                //    {
                //        player.Xp += 10;
                //        Experiencelist.RemoveAt(i);
                //    }
                //for (var i = 0; i < Pickupitems.Count; i++)
                //{
                //    if (!player.Testbox.Intersects(Pickupitems[i].HitBox)) continue;
                //    player.GetItem(Pickupitems[i]);
                //    Pickupitems.RemoveAt(i);
                //}
            }
        }
        /// <summary>
        /// Updates the Camera
        /// </summary>
        public void CameraUpdate()
        {
            //Moving Rectangle Collision
            var _topnew = new Rectangle(_top.X + CameraX, _top.Y + CameraY, _top.Width, _top.Height);
            var _bottomnew = new Rectangle(_bottom.X + CameraX, _bottom.Y + CameraY, _bottom.Width, _bottom.Height);
            var _leftnew = new Rectangle(_left.X + CameraX, _left.Y + CameraY, _left.Width, _left.Height);
            var _rightnew = new Rectangle(_right.X + CameraX, _right.Y + CameraY, _right.Width, _right.Height);
            //SPRITE MOVEMENT
            var _downtouch = false;
            var _uptouch = false;
            var _lefttouch = false;
            var _righttouch = false;
            var upvalue = 0;
            var leftvalue = 0;
            var rightvalue = 0;
            var downvalue = 0;
            foreach (var player in _entities.Where(player => IsSameOrSubclass(typeof(Player),player.GetType()) && ((Player)player).Alive).ToList())
            {
                //Update Player Inputs
                if (player.GetType() == typeof(Fighter))
                    ((Fighter)player).Act(this);
                if (player.GetType() == typeof(Mage))
                    ((Mage)player).Act(this);

                if (((Player)player).Testbox.Intersects(_bottomnew))
                {
                    _downtouch = true;
                    downvalue += ((Player)player).SpriteSpeed;
                }
                if (((Player)player).Testbox.Intersects(_topnew))
                {
                    _uptouch = true;
                    upvalue += ((Player)player).SpriteSpeed;
                }
                if (((Player)player).Testbox.Intersects(_leftnew))
                {
                    _lefttouch = true;
                    leftvalue += ((Player)player).SpriteSpeed;
                }
                if (((Player)player).Testbox.Intersects(_rightnew))
                {
                    _righttouch = true;
                    rightvalue += ((Player)player).SpriteSpeed;
                }

                if (!(_downtouch && _uptouch))
                {
                    if (_downtouch)
                    {
                        CameraY += downvalue;
                    }
                    if (_uptouch)
                    {
                        CameraY -= upvalue;
                    }
                }
                if (!(_lefttouch && _righttouch))
                {
                    if (_lefttouch)
                    {
                        CameraX -= leftvalue;
                    }
                    if (_righttouch)
                    {
                        CameraX += rightvalue;
                    }
                }
                //Camera Bounds
                if (CameraX > _map.WidthLength - 800)
                    CameraX = _map.WidthLength - 800;
                if (CameraY > _map.HeightLength - 480)
                    CameraY = _map.HeightLength - 480;
                if (CameraX < 0)
                    CameraX = 0;
                if (CameraY < 0)
                    CameraY = 0;
            }
        }
        /// <summary>
        /// Determines whether the specified footrect is colliding.
        /// </summary>
        /// <param name="footrect">The footrect.</param>
        /// <param name="playerindex">The playerindex.</param>
        /// <returns></returns>
        public bool isColliding(Rectangle footrect, int playerindex = -1)
        {
            isOutOfBounds(footrect);
            //minus minus(TopLeft)
            var _tileLocation = new Vector2((float)footrect.Left / _tilelength,
                (float)footrect.Top / _tilelength);
            if (_map.Tilemap[(int)_tileLocation.Y][(int)_tileLocation.X].Collidable)
                return true;

            //MINUS PLUS(BottomLeft)
            _tileLocation = new Vector2((float)footrect.Left / _tilelength,
                (float)footrect.Bottom / _tilelength);
            if (_map.Tilemap[(int)_tileLocation.Y][(int)_tileLocation.X].Collidable)
                return true;

            //PLUS MINUS(TopRight)
            _tileLocation = new Vector2((float)footrect.Right / _tilelength,
                (float)footrect.Top / _tilelength);
            if (_map.Tilemap[(int)_tileLocation.Y][(int)_tileLocation.X].Collidable)
                return true;

            //PLUS PLUS(BottomRight)
            _tileLocation = new Vector2((float)(footrect.Right) / _tilelength,
                (float)footrect.Bottom / _tilelength);
            if (_map.Tilemap[(int)_tileLocation.Y][(int)_tileLocation.X].Collidable)
                return true;
            //check for other players
            if (playerindex != -1)
            {
                foreach (Player player in _entities)
                {
                    if (playerindex != (int)player.Playerindex)
                    {
                        if (player.Feetrect.Intersects(footrect))
                            return true;
                    }
                }
                foreach (Npc computerperson in _entities)
                {
                    if (computerperson.FeetBox.Intersects(footrect))
                        return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Determines whether the specified footrect is colliding.
        /// </summary>
        /// <param name="footrect">The footrect.</param>
        /// <param name="playerindex">The playerindex.</param>
        /// <returns></returns>
        public bool isColliding(RectangleF footrect, int playerindex = -1)
        {
            if (isOutOfBounds(footrect))
                return true;
            //minus minus(TopLeft)
            var _tileLocation = new Vector2(footrect.Left / _tilelength,
                footrect.Top / _tilelength);
            if (_map.Tilemap[(int)_tileLocation.Y][(int)_tileLocation.X].Collidable)
                return true;

            //MINUS PLUS(BottomLeft)
            _tileLocation = new Vector2(footrect.Left / _tilelength,
                footrect.Bottom / _tilelength);
            if (_map.Tilemap[(int)_tileLocation.Y][(int)_tileLocation.X].Collidable)
                return true;

            //PLUS MINUS(TopRight)
            _tileLocation = new Vector2(footrect.Right / _tilelength,
                footrect.Top / _tilelength);
            if (_map.Tilemap[(int)_tileLocation.Y][(int)_tileLocation.X].Collidable)
                return true;

            //PLUS PLUS(BottomRight)
            _tileLocation = new Vector2((footrect.Right) / _tilelength,
                footrect.Bottom / _tilelength);
            if (_map.Tilemap[(int)_tileLocation.Y][(int)_tileLocation.X].Collidable)
                return true;
            //check for other players
            if (playerindex != -1)
            {
                foreach (Player player in _entities.Where(player => IsSameOrSubclass(typeof(Player),player.GetType())))
                {
                    if (playerindex != (int)player.Playerindex)
                    {
                        if (footrect.Intersects(player.Feetrect))
                            return true;
                    }
                }
            }
            return false;
        }
        public bool isOutOfBounds(RectangleF footrect)
        {
            return (footrect.X < 0 || footrect.Y < 0 || footrect.Right >= _map.WidthLength || footrect.Top >= _map.HeightLength);
        }
        public bool isOutOfBounds(Rectangle footrect)
        {
            return (footrect.X < 0 || footrect.Y < 0 || footrect.Right >= _map.WidthLength || footrect.Bottom >= _map.HeightLength);
        }
        /// <summary>
        /// Adjust for drawing in relation to Camera
        /// </summary>
        /// <param name="rect">The rect.</param>
        /// <returns></returns>
        public Rectangle CameraFix(Rectangle rect)
        {
            return new Rectangle(rect.X - CameraX, rect.Y - CameraY, rect.Width, rect.Height);
        }
        /// <summary>
        /// Adjust for drawing in relation to Camera
        /// </summary>
        /// <param name="vect">The vect.</param>
        /// <returns></returns>
        public Vector2 CameraFix(Vector2 vect)
        {
            return new Vector2(vect.X - CameraX, vect.Y - CameraY);
        }
        /// <summary>
        /// Adjust for drawing in relation to Camera
        /// </summary>
        /// <param name="rect">The rect.</param>
        /// <returns></returns>
        public RotatedRectangle CameraFix(RotatedRectangle rect)
        {
            rect.ChangePosition(-CameraX, -CameraY);
            return rect;
        }
        /// <summary>
        /// Adjust for drawing in relation to Camera
        /// </summary>
        /// <param name="rect">The rect.</param>
        /// <returns></returns>
        public RectangleF CameraFix(RectangleF rect)
        {
            return new RectangleF(new Vector2(rect.Min.X - CameraX, rect.Min.Y - CameraY),
                new Vector2(rect.Max.X - CameraX, rect.Max.Y - CameraY));
        }
        /// <summary>
        /// Draws the map.
        /// </summary>
        /// <param name="sb">The sb.</param>
        /// <param name="upper">if set to <c>true</c> [upper].</param>
        public void DrawMap(SpriteBatch sb, bool upper = false)
        {
            //26x16
            var endx = 26 + CameraX / _tilelength;
            var endy = 16 + CameraY / _tilelength;
            //DRAW MAP
            for (var x = CameraX / _tilelength; x < MathHelper.Clamp(endx, 0, _map.Widthtiles); x++)
            {
                for (var y = CameraY / _tilelength; y < MathHelper.Clamp(endy, 0, _map.Heightiles); y++)
                {
                    if (_map.Tilemap[y][x].SourceX == 0)
                        continue;
                    //Section of image to draw
                    var _tilesourcerect = new Rectangle(_map.Tilemap[y][x].SourceX * _map.Tilelength, 0, _tilelength,
                        _tilelength);

                    //Destination Rectangle
                    var _tilerect = new Rectangle(
                        (x * _map.Tilelength) - CameraX,
                        (y * _map.Tilelength) - CameraY,
                        _map.Tilelength,
                        _map.Tilelength);
                    //if (upper)
                    //{
                    //    sb.Draw(_tileset.BlockTexture, _tilerect, _tilesourcerect,
                    //        touchedTiles.Contains(new Point(x, y)) ? Color.Transparent : Color.White);
                    //}
                    //else
                    //{
                    sb.Draw(_tileset, _tilerect, _tilesourcerect, Color.White);
                    //}


                    //spriteBatch.DrawString(small, x.ToString() + "," + y.ToString(), new Vector2(tilerect.X, tilerect.Y), Color.White);
                }
            }
        }
        /// <summary>
        /// Updates shots
        /// </summary>
        public void Updateshot()
        {
            // Updates the location of all of the enemy player shot.
            foreach (var enemy in _entities.Where(enemy => enemy.GetType() == typeof(Dragon)))
            {
                throw new NotImplementedException();
                //for (var x = 0; x < ((Dragon)enemy).Shot; x++)
                //{
                //    if (!((Dragon)enemy).Bullets[x].IsActive) continue;
                //    foreach (Player player in _entities.Where(thing => thing.GetType() == typeof(Player)))
                //    {
                //        if (player.Ishurting || !(player.Testbox.Intersects(((Dragon)enemy).Bullets[x].Hitbox)))
                //            continue;
                //        player.Hurt(10);
                //        player.Ishurting = true;
                //    }
                //}
            }
        }
        /// <summary>
        /// Newest Level Loader
        /// </summary>
        /// <param name="filename">The filename.</param>
        private void NewestLevelLoad(string LevelName)
        {
            StreamReader reader = new StreamReader(string.Format("Content/{0}/{0}.lvl", LevelName));
            XDocument document = XDocument.Load(reader);
            Console.WriteLine();
            var playerpositions = document.Element("LevelData").Elements("Player").ToList();
            int count = 0;
            foreach (var player in _entities.Where(x => IsSameOrSubclass(typeof(Player),x.GetType())))
            {
                player.Feetrect.X = float.Parse(playerpositions[count].Attribute("X").Value);
                player.Feetrect.Y = float.Parse(playerpositions[count].Attribute("Y").Value);
                count++;
            }
            Console.WriteLine();
            IEnumerable<XElement> npcpositions = document.Element("LevelData").Elements("NPC");
            _entities.RemoveAll(x => IsSameOrSubclass(typeof(Npc), x.GetType()));
            foreach (var npc in npcpositions)
            {
                //AddEntity(new Npc(new Vector2(float.Parse(npc.Attribute("X").Value), float.Parse(npc.Attribute("Y").Value)));
            }
            IEnumerable<XElement> doors = document.Element("LevelData").Elements("Door");
            foreach (var door in doors)
            {
                AddEntity(new Door(new Rectangle(int.Parse(door.Attribute("X").Value), int.Parse(door.Attribute("Y").Value), int.Parse(door.Attribute("Width").Value), int.Parse(door.Attribute("Height").Value))));
            }
            //throw new NotImplementedException("Finish Loading Level Code");
            IEnumerable<XElement> tiles = document.Element("LevelData").Elements("Tile");
            List<TileAdvanced> newtiles = new List<TileAdvanced>();
            foreach (var tile in tiles)
            {
                int sourceX;
                bool collidable;
                if (int.TryParse(tile.Attribute("SourceX").Value, out sourceX) &&
                    bool.TryParse(tile.Attribute("Collidable").Value, out collidable))
                {
                    newtiles.Add(new TileAdvanced(sourceX, collidable));
                }
            }
            int tilemapwidth, tilemapheight;
            int.TryParse(document.Element("LevelData").Attribute("Width").Value, out tilemapwidth);
            int.TryParse(document.Element("LevelData").Attribute("Height").Value, out tilemapheight);
            _map = new TileMap(tilemapwidth, tilemapheight);
            int tilecount = 0;
            for (int i = 0; i < tilemapheight; i++)
            {
                for (int j = 0; j < tilemapwidth; j++)
                {
                    _map.Tilemap[i][j] = newtiles[tilecount];
                    count++;
                }
            }

            _map.Widthtiles = tilemapwidth;
            _map.Heightiles = tilemapheight;
            Console.WriteLine();
            //var npcs = new List<Vector2>();
            //var doors = new List<Door>();
            //var players = new List<Vector2>();
            //var tempLowerMap = new TileMap();
            //var tempUpperMap = new TileMap();
            //// Read the file and display it line by line.
            //var lines = File.ReadAllLines(string.Format("Content/{0}/{0}.lvl", LevelName));
            ////Add Players
            //if (lines[0] != "Nothing")
            //{
            //    var playerpositions = lines[0].Split('|').ToList();
            //    for (var j = 0; j < playerpositions.Count / 2; j += 2)
            //        players.Add(new Vector2(Convert.ToInt32(playerpositions[j]),
            //            Convert.ToInt32(playerpositions[j + 1])));
            //}
            ////Add Npcs
            //if (lines[1] != "Nothing")
            //{
            //    var npcpositions = lines[1].Split('|').ToList();
            //    for (var j = 0; j < npcpositions.Count / 2; j += 2)
            //        npcs.Add(new Vector2(Convert.ToInt32(npcpositions[j]), Convert.ToInt32(npcpositions[j + 1])));
            //}
            ////Add Doors
            //if (lines[2] != "Nothing")
            //{
            //    var doorpositions = lines[2].Split('|').ToList();
            //    for (var j = 0; j < doorpositions.Count / 5; j += 5)
            //        doors.Add(
            //            new Door(
            //                new Rectangle(Convert.ToInt32(doorpositions[j]), Convert.ToInt32(doorpositions[j + 1]),
            //                    Convert.ToInt32(doorpositions[j + 2]), Convert.ToInt32(doorpositions[j + 3])),
            //                doorpositions[j + 4]));
            //}
            ////Add Width and Height
            //var dimensions = lines[3].Split('|');
            //tempLowerMap.Widthtiles = Convert.ToInt32(dimensions[0]);
            //tempLowerMap.Heightiles = Convert.ToInt32(dimensions[1]);
            //tempUpperMap.Widthtiles = Convert.ToInt32(dimensions[2]);
            //tempUpperMap.Heightiles = Convert.ToInt32(dimensions[3]);
            ////Create New Tilemap
            //for (var j = 4; j < tempLowerMap.Heightiles + 4; j++)
            //{
            //    tempLowerMap.Tilemap.Add(new List<TileAdvanced>());
            //    var tiles = lines[j].Split(new[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
            //    for (var k = 0; k < tiles.Length / 2; k++)
            //    {
            //        tempLowerMap.Tilemap[j - 4].Add(new TileAdvanced(
            //            Convert.ToInt32(tiles[(k * 2) + 1]), tiles[(k * 2)] != "f"));
            //    }
            //}
            //for (var j = tempLowerMap.Heightiles + 4; j < tempUpperMap.Heightiles + tempLowerMap.Heightiles + 4; j++)
            //{
            //    tempUpperMap.Tilemap.Add(new List<TileAdvanced>());
            //    var tiles = lines[j].Split(new[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
            //    for (var k = 0; k < tiles.Length / 2; k++)
            //    {
            //        tempUpperMap.Tilemap[j - 4 - tempLowerMap.Heightiles].Add(new TileAdvanced(
            //            Convert.ToInt32(tiles[(k * 2) + 1]), tiles[(k * 2)] != "f"));
            //    }
            //}
            ////Set Global Variables of Game1 Editor
            //int index = 0;
            //foreach (Player player in _entities.Where(thing => IsSameOrSubclass(typeof(Player),thing.GetType())))
            //{
            //    player.Position = players[index];
            //    index++;
            //}
            //foreach (var thing in doors)
            //{
            //    _entities.Add(thing);
            //}

            //_map.Tilemap.Clear();
            //for (var i = 0; i < tempLowerMap.Tilemap.Count; i++)
            //    _map.Tilemap.Add(tempLowerMap.Tilemap[i]);

            //_map.Widthtiles = tempLowerMap.Widthtiles;
            //_map.Heightiles = tempLowerMap.Heightiles;

            //_tilelength = 32;
            //_map.WidthLength = _map.Widthtiles * 32;
            //_map.HeightLength = _map.Heightiles * 32;
        }
        /// <summary>
        /// Checks Collision between Enemies.
        /// </summary>
        /// <param name="current">The current.</param>
        /// <param name="rect">The rect.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public static bool EnemyVSenemycollision(int current, Rectangle rect)
        {
            throw new NotImplementedException();
            return false;
            //Enemies.Where((t, i) => current != i)
            //    .Where(t => t.Ishurting == false)
            //    .Any(t => t.Rectangle.Intersects(rect));
        }
        /// <summary>
        /// Checks collision between players and NPCs
        /// </summary>
        /// <param name="playindex">The playindex.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public static bool Pvp(PlayerIndex playindex)
        {
            throw new NotImplementedException();
            return false;
            /*
            return PlayerRects.Where((t, i) => i != (int)playindex)
                .Any(t => PlayerRects[(int)playindex].Intersects(t))
                   || Npcs.Any(t => PlayerRects[(int)playindex].Intersects(t.FeetBox));
             */
        }
        /// <summary>
        /// Is potentialDescendant a sublcass of BaseClass
        /// </summary>
        /// <param name="potentialBase">The potential base.</param>
        /// <param name="potentialDescendant">The potential descendant.</param>
        /// <returns></returns>
        public static bool IsSameOrSubclass(Type potentialBase, Type potentialDescendant)
        {
            return potentialDescendant.IsSubclassOf(potentialBase)
                   || potentialDescendant == potentialBase;
        }
        public static Dictionary<string, Animation> LoadAnimations(string Class)
        {
            var animations = new Dictionary<string, Animation>();
            var document = XDocument.Load("Content\\Animations.xml");
            var nodes = document.Root.Elements("AnimationClass").Where(x => x.Attribute("class").Value == Class).FirstOrDefault();
            if (nodes == null)
                throw new IOException("Invalid Xml");
            foreach (var node in nodes.Elements("Animation"))
            {
                int frames = (int)node.Element("Frames");
                int XMovement = (int)node.Element("XMovement");
                int YMovement = (int)node.Element("YMovement");
                int XOffset = (int)node.Element("XOffset");
                int YOffset = (int)node.Element("YOffset");
                string name = (string)node.FirstAttribute;

                //Load animation Rectangles
                List<Microsoft.Xna.Framework.Rectangle> animRects = new List<Microsoft.Xna.Framework.Rectangle>();
                List<int> timers = new List<int>();

                foreach (var rect in node.Elements("AnimRects").Elements())
                {
                    animRects.Add(new Microsoft.Xna.Framework.Rectangle((int)rect.Element("X"), (int)rect.Element("Y"), (int)rect.Element("Width"), (int)rect.Element("Height")));
                    var tempTime = rect.Attribute("timeLength");
                    if (tempTime == null)
                        timers.Add(0);
                    else
                        timers.Add((int)tempTime);
                }


                List<Collidable> colliders = new List<Collidable>();
                foreach (var collider in node.Elements("Colliders").Elements())
                {
                    if (collider.Name == "Rect")
                        colliders.Add(new _2D_Game.RectangleF((float)collider.Element("X"), (float)collider.Element("Y"), (float)collider.Element("Width"), (float)collider.Element("Height")));
                    else if (collider.Name == "Circle")
                        colliders.Add(new Circle(new Microsoft.Xna.Framework.Vector2((float)collider.Element("CenterX"), (float)collider.Element("CenterY")), (float)collider.Element("Radius")));
                }

                animations.Add(name, new Animation(name, animRects, new Microsoft.Xna.Framework.Vector2(XMovement, YMovement), colliders,timers.ToArray(), XOffset, YOffset));
            }
            return animations;
        }
        public List<Player> Players
        {
            get { return _entities.Where(player => World.IsSameOrSubclass(typeof(Player), player.GetType())).Select(player => (Player)player).ToList(); }
        }

        public List<Enemy> Enemies
        {
            get { return _entities.Where(enemy => World.IsSameOrSubclass(typeof(Enemy), enemy.GetType())).Select(enemy => (Enemy)enemy).ToList(); }
        }

        public List<Npc> Npcs
        {
            get { return _entities.Where(npc => World.IsSameOrSubclass(typeof(Npc), npc.GetType())).Select(npc => (Npc)npc).ToList(); }
        }
    }
}