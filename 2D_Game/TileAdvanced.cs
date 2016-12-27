namespace _2D_Game
{
    public class TileAdvanced
    {
        public bool Collidable;
        public int SourceX;

        public TileAdvanced(int newSourceX = 0, bool collidable = false)
        {
            SourceX = newSourceX;
            Collidable = collidable;
        }
    }
}