using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimationEditorForms
{
    public class Circle : Collidable
    {
        public Vector2 Center { get; set; }
        public float Radius { get; set; }
        public float X { get { return Center.X - Radius; } }
        public float Y { get { return Center.Y - Radius; } }
        public float Width { get { return Radius * 2; } }
        public float Height { get { return Radius * 2; } }

        public Circle(Vector2 center, float radius)
        {
            this.Center = center;
            this.Radius = radius;
        }

        public Circle(Circle circle)
        {
            this.Center = circle.Center;
            this.Radius = circle.Radius;
        }

        public bool Contains(Vector2 point)
        {
            return ((point - Center).Length() <= Radius);
        }

        public bool Intersects(Circle other)
        {
            Vector2 newVector = other.Center - this.Center;
            float length = newVector.Length();
            float radius = (other.Radius + Radius);
            return (length < radius);
        }

        public bool Intersects(RectangleF rectangle)
        {
            Vector2 v = new Vector2(MathHelper.Clamp(Center.X, rectangle.Left, rectangle.Right),
                                    MathHelper.Clamp(Center.Y, rectangle.Bottom, rectangle.Top));

            Vector2 direction = Center - v;
            float distanceSquared = direction.LengthSquared();

            return ((distanceSquared > 0) && (distanceSquared < Radius * Radius));
        }

        public bool Intersects(RotatedRectangle rectangle)
        {
            return rectangle.Intersects(this);
        }

        public Rectangle toRectangle()
        {
            return new Rectangle((int)X, (int)Y, (int)Width, (int)Height);
        }

        //Returns true if the circles are touching, or false if they are not
        //public bool Intersects(Circle otherCircle)
        //{
        //    //this=2
        //    //compare the distance to combined radii
        //    int dx = Convert.ToInt16(this.X - otherCircle.X);
        //    int dy = Convert.ToInt16(this.Y - otherCircle.Y);
        //    int radii = Convert.ToInt16(this.Radius + otherCircle.Radius);
        //    if ((dx * dx) + (dy * dy) < radii * radii) return true;
        //    return false;
        //}
    }
}
