using System.Collections.Generic;

namespace _2D_Game
{
    public class TileMap
    {
        public int Widthtiles;
        public int Heightiles;
        public int Tilelength = 32;
        public List<List<TileAdvanced>> Tilemap;
        public int WidthLength;
        public int HeightLength;

        public TileMap(int width, int height)
        {
            Tilemap = new List<List<TileAdvanced>>();
            Widthtiles = width;
            Heightiles = height;
            WidthLength = width * Tilelength;
            HeightLength = height * Tilelength;
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