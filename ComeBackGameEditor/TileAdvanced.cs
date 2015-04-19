namespace ComeBackGameEditor
{
    public class TileAdvanced
    {
        public TileAdvanced(int newSourceX,bool collidableb)
        {
            SourceX = newSourceX;
            Collidable = collidableb;
        }
        public TileAdvanced(int newSourceX)
        {
            SourceX = newSourceX;
            Collidable = false;
        }
        public TileAdvanced()
        {
            SourceX = 0;
            Collidable = false;
        }
        public bool Collidable { get; set; }

        public int SourceX { get; set; }
    }
}
