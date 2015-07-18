using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using _2D_Game;
namespace ComeBackGameEditor
{
    public class Level
    {
        public List<Player> Players;
        public List<Npc> NPCs;
        public List<Door> Doors;
        public int TileMapWidth;
        public int TileMapHeight;
        public List<List<TileAdvanced>> Tilemap;
    }
}