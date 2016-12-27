namespace _2D_Game
{
    public class Timer
    {
        public float Interval;
        public float Max;
        public float Increment;

        public Timer(float max = 0, float increment = 1)
        {
            Max = max;
            Increment = increment;
        }

        public void Update()
        {
            Interval += Increment;
        }

        public void Update(float incrementer)
        {
            Interval += incrementer;
        }

        public bool Finished()
        {
            if (Interval >= Max)
            {
                Interval = 0;
                return true;
            }
            else
                return false;
        }
    }
}