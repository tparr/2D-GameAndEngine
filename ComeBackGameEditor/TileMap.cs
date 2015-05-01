using System.Collections.Generic;
using System.Windows.Forms;

namespace ComeBackGameEditor
{
    public class TileMap : Control
    {
        public int Tilelength = 32;
        public List<List<TileAdvanced>> Tilemap;
        public TileMap(int width,int height)
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
            Height = height;
            Width = width;
        }
        public TileMap()
        {
            Tilemap = new List<List<TileAdvanced>>();
        }
    }
}
