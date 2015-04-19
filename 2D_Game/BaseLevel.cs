using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace _2D_Game
{
    public class BaseLevel
    {
        //MAP VARS
        protected int Tilemapwidth;
        protected int Tilemapheight;
        protected int Tilewidth;
        protected int Tileheight;
        protected int Tilelength;
        protected List<Npc> Npcs = new List<Npc>();
        public Block Tileset;
        protected int Solid;
        public List<Vector2> Positions = new List<Vector2>();
        public List<Door> Doors = new List<Door>();
        public List<Item> PickupItems;
        public List<Enemy> Enemies = new List<Enemy>();
        #region TileMap
        //73x70 tilemap x,y
        public List<List<TileAdvanced>> Tilemap;
        #endregion
        public List<List<TileAdvanced>> TileMap
        { get { return Tilemap; } }
        public int TileMapWidth
        { get { return Tilemapwidth; } }
        public int TileMapHeight
        { get { return Tilemapheight; } }
        public int MapWidth
        { get { return Tilemapwidth * Tilewidth; } }
        public int MapHeight
        { get { return Tilemapheight * Tileheight; } }
        public int TileWidth
        { get { return Tilewidth; } }
        public int TileHeight
        { get { return Tileheight; } }
        public int TileLength
        { get { return Tilelength; } }

        public List<Npc> GetNpcs
        { get { return Npcs; } }
        public int SolidNumber
        { get { return Solid; } }
        public List<Item> Pickups
        { get { return PickupItems; } }
        
    }
}