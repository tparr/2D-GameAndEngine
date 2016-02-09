using Microsoft.Xna.Framework;

namespace _2D_Game
{
    /// <summary>
    /// Much like Rectangle, but stored as two Vector2s
    /// </summary>
    public struct RectangleF : Collidable
    {
        public Vector2 Min;
        public Vector2 Max;

        public float Left { get { return Min.X; } }
        public float Right { get { return Max.X; } }
        public float Top { get { return Max.Y; } }
        public float Bottom { get { return Min.Y; } }

        public float Width
        {
            get { return Max.X - Min.X; }
            set { Max.X += value; }
        }

        public float Height
        {
            get { return Max.Y - Min.Y; }
            set { Max.Y += value; }
        }

        private static readonly RectangleF _mEmpty;
        private static readonly RectangleF _mMinMax;

        static RectangleF()
        {
            _mEmpty = new RectangleF();
            _mMinMax = new RectangleF(Vector2.One * float.MinValue, Vector2.One * float.MaxValue);
        }

        public Vector2 Center
        {
            get { return (Min + Max) / 2; }
        }

        public static RectangleF Empty
        {
            get { return _mEmpty; }
        }


        public static RectangleF MinMax
        {
            get { return _mMinMax; }
        }
        public float X
        {
            get { return Min.X; }
            set
            {
                Min.X = value;
            }
        }

        public float Y
        {
            get { return Min.Y; }
            set
            {
                Min.Y = value;
            }
        }

        public bool IsZero
        {
            get
            {
                return
                    (Min.X == 0) &&
                    (Min.Y == 0) &&
                    (Max.X == 0) &&
                    (Max.Y == 0);
            }
        }

        public RectangleF(float x, float y, float width, float height)
        {
            Min.X = x;
            Min.Y = y;
            Max.X = x + width;
            Max.Y = y + height;
        }

        public RectangleF(Vector2 min, Vector2 max)
        {
            Min = min;
            Max = max;
        }

        public RectangleF(Rectangle rect)
        {
            Min.X = rect.X;
            Min.Y = rect.Y;
            Max.X = rect.X + rect.Width;
            Max.Y = rect.Y + rect.Height;
        }

        public void Adjust(float xadjust, float yadjust)
        {
            Min.X += xadjust;
            Max.X += xadjust;
            Max.Y += yadjust;
            Min.Y += yadjust;
        }

        public void Adjust(Vector2 adjustment)
        {
            Min += adjustment;
            Max += adjustment;
        }

        public bool Contains(float x, float y)
        {
            return
                (Min.X <= x) &&
                (Min.Y <= y) &&
                (Max.X >= x) &&
                (Max.Y >= y);
        }

        public bool Contains(Vector2 vector)
        {
            return
                (Min.X < vector.X) &&
                (Min.Y < vector.Y) &&
                (Max.X > vector.X) &&
                (Max.Y > vector.Y);
        }

        public void Contains(ref Vector2 rect, out bool result)
        {
            result =
                (Min.X <= rect.X) &&
                (Min.Y <= rect.Y) &&
                (Max.X >= rect.X) &&
                (Max.Y >= rect.Y);
        }

        public bool Contains(RectangleF rect)
        {
            return
                (Min.X <= rect.Min.X) &&
                (Min.Y <= rect.Min.Y) &&
                (Max.X >= rect.Max.X) &&
                (Max.Y >= rect.Max.Y);
        }

        public void Contains(ref RectangleF rect, out bool result)
        {
            result =
                (Min.X <= rect.Min.X) &&
                (Min.Y <= rect.Min.Y) &&
                (Max.X >= rect.Max.X) &&
                (Max.Y >= rect.Max.Y);
        }

        public bool Intersects(RectangleF rect)
        {
            return
                (Min.X < rect.Max.X) &&
                (Min.Y < rect.Max.Y) &&
                (Max.X > rect.Min.X) &&
                (Max.Y > rect.Min.Y);
        }
        public bool Intersects(Rectangle rect)
        {
            return
                !((Min.X < rect.Left) ||
                 (Max.Y > rect.Bottom) ||
                 (Max.X > rect.Right) ||
                 (Min.Y < rect.Top));
        }

        public bool Intersects(Circle circle)
        {
            return circle.Intersects(this);
        }

        //public bool Intersects(RotatedRectangle rect)
        //{
        //    return rect.Intersects(this);
        //}

        public void Intersects(ref RectangleF rect, out bool result)
        {
            result =
                (Min.X < rect.Max.X) &&
                (Min.Y < rect.Max.Y) &&
                (Max.X > rect.Min.X) &&
                (Max.Y > rect.Min.Y);
        }

        public static RectangleF Intersect(RectangleF rect1, RectangleF rect2)
        {
            RectangleF result;

            float num8 = rect1.Max.X;
            float num7 = rect2.Max.X;
            float num6 = rect1.Max.Y;
            float num5 = rect2.Max.Y;
            float num2 = (rect1.Min.X > rect2.Min.X) ? rect1.Min.X : rect2.Min.X;
            float num = (rect1.Min.Y > rect2.Min.Y) ? rect1.Min.Y : rect2.Min.Y;
            float num4 = (num8 < num7) ? num8 : num7;
            float num3 = (num6 < num5) ? num6 : num5;

            if ((num4 > num2) && (num3 > num))
            {
                result.Min.X = num2;
                result.Min.Y = num;
                result.Max.X = num4;
                result.Max.Y = num3;

                return result;
            }

            result.Min.X = 0;
            result.Min.Y = 0;
            result.Max.X = 0;
            result.Max.Y = 0;

            return result;
        }

        public static void Intersect(ref RectangleF rect1, ref RectangleF rect2, out RectangleF result)
        {
            float num8 = rect1.Max.X;
            float num7 = rect2.Max.X;
            float num6 = rect1.Max.Y;
            float num5 = rect2.Max.Y;
            float num2 = (rect1.Min.X > rect2.Min.X) ? rect1.Min.X : rect2.Min.X;
            float num = (rect1.Min.Y > rect2.Min.Y) ? rect1.Min.Y : rect2.Min.Y;
            float num4 = (num8 < num7) ? num8 : num7;
            float num3 = (num6 < num5) ? num6 : num5;

            if ((num4 > num2) && (num3 > num))
            {
                result.Min.X = num2;
                result.Min.Y = num;
                result.Max.X = num4;
                result.Max.Y = num3;
            }

            result.Min.X = 0;
            result.Min.Y = 0;
            result.Max.X = 0;
            result.Max.Y = 0;
        }

        public static RectangleF Union(RectangleF rect1, RectangleF rect2)
        {
            RectangleF result;

            float num6 = rect1.Max.X;
            float num5 = rect2.Max.X;
            float num4 = rect1.Max.Y;
            float num3 = rect2.Max.Y;
            float num2 = (rect1.Min.X < rect2.Min.X) ? rect1.Min.X : rect2.Min.X;
            float num = (rect1.Min.Y < rect2.Min.Y) ? rect1.Min.Y : rect2.Min.Y;
            float num8 = (num6 > num5) ? num6 : num5;
            float num7 = (num4 > num3) ? num4 : num3;

            result.Min.X = num2;
            result.Min.Y = num;
            result.Max.X = num8;
            result.Max.Y = num7;

            return result;
        }

        public static void Union(ref RectangleF rect1, ref RectangleF rect2, out RectangleF result)
        {
            float num6 = rect1.Max.X;
            float num5 = rect2.Max.X;
            float num4 = rect1.Max.Y;
            float num3 = rect2.Max.Y;
            float num2 = (rect1.Min.X < rect2.Min.X) ? rect1.Min.X : rect2.Min.X;
            float num = (rect1.Min.Y < rect2.Min.Y) ? rect1.Min.Y : rect2.Min.Y;
            float num8 = (num6 > num5) ? num6 : num5;
            float num7 = (num4 > num3) ? num4 : num3;

            result.Min.X = num2;
            result.Min.Y = num;
            result.Max.X = num8;
            result.Max.Y = num7;
        }

        public bool Equals(RectangleF other)
        {
            return
                (Min.X == other.Min.X) &&
                (Min.Y == other.Min.Y) &&
                (Max.X == other.Max.X) &&
                (Max.Y == other.Max.Y);
        }

        public override int GetHashCode()
        {
            return Min.GetHashCode() + Max.GetHashCode();
        }

        public static bool operator ==(RectangleF a, RectangleF b)
        {
            return
                (a.Min.X == b.Min.X) &&
                (a.Min.Y == b.Min.Y) &&
                (a.Max.X == b.Max.X) &&
                (a.Max.Y == b.Max.Y);
        }

        public static bool operator !=(RectangleF a, RectangleF b)
        {
            return
                (a.Min.X != b.Min.X) ||
                (a.Min.Y != b.Min.Y) ||
                (a.Max.X != b.Max.X) ||
                (a.Max.Y != b.Max.Y);
        }

        public override bool Equals(object obj)
        {
            if (obj is RectangleF)
            {
                return this == (RectangleF)obj;
            }

            return false;
        }

        public Rectangle ToRectangle()
        {
            return new Rectangle((int) X, (int) Y, (int) Width, (int) Height);
        }
    }
}
