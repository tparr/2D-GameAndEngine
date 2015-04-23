using System.Collections.Generic;

namespace _2D_Game
{
    public class TileMap
    {
        public int Width;
        public int Height;
        public int Tilelength = 32;
        public List<List<TileAdvanced>> Tilemap;
        public TileMap(int width, int height)
        {
            Tilemap = new List<List<TileAdvanced>>();
            for (int i = 0; i < height; i++)
            {
                Tilemap.Add(new List<TileAdvanced>());
                for (int j = 0; j < width; j++)
                {
                    Tilemap[i].Add(new TileAdvanced(0, false));
                }
            }
        }
        public TileMap()
        {
            Tilemap = new List<List<TileAdvanced>>();
        }
    }
}
