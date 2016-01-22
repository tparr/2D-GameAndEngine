using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _2D_Game
{
    public class Enemy : Thing
    {
        public int Health = 100;
        protected Vector2 Newpositionx;
        protected Vector2 Newpositiony;
        protected Texture2D Texture;
        protected Rectangle Rect;
        protected bool BActive;
        public bool Ishurting;
        protected Texture2D Healthbar;
        protected int Hurtinterval;
        protected int Velocityx;
        protected int Velocityy;
        protected RectangleF Feetrectnew;
        protected float Timer;
        protected float Interval = 200f;
        protected float Timera;
        protected float Intervala = 400f;
        protected int Xdiff;
        protected int Ydiff;
        protected bool Up, Left, Right, Down;
        protected bool Attacking;
        protected Random Generator = new Random();
        public Vector2 Position;
        protected RectangleF Hurtposition;
        protected int Feetrectmodx;
        protected int Feetrectmody;
        protected int Feetrectwidth;
        protected int Feetrectheight;
        protected Dictionary<string, Animation> Animations;
        protected string CurrAnimation;
        protected int Animatetimer = 15;
        protected int Animatecounter = 0;

        public Rectangle Rectangle
        {
            get { return Rect; }
            set { Rect = value; }
        }
        public bool IsActive
        {
            get { return BActive; }
        }
        public int X
        {
            get { return (int)Position.X; }
        }
        public int Y
        {
            get { return (int)Position.Y; }
        }

        public virtual void Act(Rectangle playerbox, int enemyx, int enemyy, int current)
        {
            SetHittable();
            Move(playerbox, enemyx, enemyy, current);
        }

        public Enemy(Texture2D texture, int positonx, int positiony, Texture2D healthbar,string Class)
        {
            Texture = texture;
            Position.X = positonx;
            Position.Y = positiony;
            Healthbar = healthbar;
            BActive = true;
            Animations = World.LoadAnimations(Class);
        }

        public virtual void Move(Rectangle playerbox, int enemyx, int enemyy, int current)
        {
            //throw new NotImplementedException();
            Feetrect = new RectangleF(Position.X + Feetrectmodx, Position.Y + Feetrectmody, 10, 5);
            Feetrectnew = Feetrect;
            //Timera += (float)gametime.ElapsedGameTime.TotalMilliseconds;
            if (Health <= 0)
                Dead();
            if (Ishurting == false)
            {
                Newpositionx = Position;
                Newpositiony = Position;
                if (playerbox.Y > enemyy)
                {
                    Down = true;
                    Up = false;
                    Ydiff = playerbox.Y - enemyy;
                    Feetrectnew = Feetrect;
                    Velocityy = 25;
                    Newpositiony.Y += 1;
                    Feetrectnew.Y += 1;
                    Attacking = Ydiff < 10;
                    if (!Attacking)
                        if (Colliding(current, playerbox))
                            Position.Y = Newpositiony.Y;
                }
                else if (playerbox.Y < enemyy)
                {
                    Up = true;
                    Down = false;
                    Ydiff = enemyy - playerbox.Y;
                    Feetrectnew = Feetrect;
                    Velocityy = -25;
                    Newpositiony.Y -= 1;
                    Feetrectnew.Y -= 1;
                    Attacking = Ydiff < 100;
                    if (!Attacking)
                        if (Colliding(current, playerbox))
                            Position.Y = Newpositiony.Y;

                }
                else Velocityy = 0;

                if (playerbox.X > enemyx)
                {
                    Right = true;
                    Left = false;
                    Xdiff = playerbox.X - enemyx;
                    Velocityx = 25;
                    Newpositionx.X += 1;
                    Feetrectnew.X += 1;
                    Attacking = Xdiff < 100;
                    if (!Attacking)
                        if (Colliding(current, playerbox))
                            Position.X = Newpositionx.X;
                }
                else if (playerbox.X < enemyx)
                {
                    Left = true;
                    Right = false;
                    Xdiff = enemyx - playerbox.X;
                    Feetrectnew = Feetrect;
                    Velocityx = -25;
                    Newpositionx.X -= 1;
                    Feetrectnew.X -= 1;
                    Attacking = Xdiff < 100;
                    if (!Attacking)
                        if (Colliding(current, playerbox))
                            Position.X = Newpositionx.X;
                }
                else Velocityx = 0;
                //if (Attacking == false)
                //{
                //    if (Xdiff > Ydiff)
                //    {
                //        if (Right)
                //            AnimateRight(gametime);
                //        if (Left)
                //            AnimateLeft(gametime);
                //    }
                //    else
                //    {
                //        if (Down)
                //            AnimateDown(gametime);
                //        if (Up)
                //            AnimateUp(gametime);
                //    }
                //}
                //else
                //{
                //    CurrentFramex = 0;
                //}
            }
        }
        protected bool Colliding(int current, Rectangle playerbox)
        {
            throw new NotImplementedException();
            //if (!Game1.CalculateCollision(Feetrectnew)
            //            && !Game1.EnemyVSenemycollision(current, Feetrectnew.ToRectangle())
            //            && !(new Rectangle((int)Newpositionx.X, (int)Newpositionx.Y, 25, 25).Intersects(playerbox)))
            //    return true;
            return false;
        }
        #region Ishurting Methods
        public void Dead()
        {
            BActive = false;
            Game1.Experiencelist.Add(new Exp(new RectangleF(Generator.Next(Rect.X,Rect.X + Rect.Width),
                Generator.Next(Rect.Y,Rect.Y + Rect.Height),
                Rect.Width,
                Rect.Height),10));
        }

        //HURT WITH PLAYER DIRECTION
        public void Hurt(int damage, int speedx, int speedy)
        {
            Health -= damage;
            Hurtposition = new RectangleF(Feetrect.Min.X += (20 * speedx), Feetrect.Y += (20 * speedy),Feetrect.Width,Feetrect.Height);
            throw new NotImplementedException();
            //if (!Game1.CalculateCollision(Hurtposition))
            //    Feetrect = Hurtposition;
            //else
            //    Hurtposition = Feetrect;
            Position.X += 20 * speedx;
            Position.Y += 20 * speedy;
            Ishurting = true;
            Hurtinterval = 0;
            
        }
        //HURT WITHOUT PLAYER DIRECTION
        public void Hurt(int damage)
        {
            Health -= damage;
            Position.X -= Velocityx;
            Position.Y -= Velocityy;
            Ishurting = true;
            Hurtinterval = 0;
        }
        public void SetHittable()
        {
            if (Ishurting)
                Hurtinterval++;
            if (Hurtinterval >= 120)
            {
                Hurtinterval = 0;
                Ishurting = false;
            }
        }
        #endregion

        public virtual void Draw(SpriteBatch sb, SpriteFont f, bool paused, World world)
        {
            if (!BActive) return;
            Rectangle newrect = world.CameraFix(Rect);
            Rectangle drawRect = new Rectangle(newrect.X, newrect.Y, Animations[CurrAnimation].AnimationRect.Width, Animations[CurrAnimation].AnimationRect.Height);
            //DRAW HealthBAR
            sb.Draw(Healthbar, new Rectangle(newrect.X - 12, newrect.Y - 8, (int)(.5 * Health), 2), Color.White);
            //DRAW ENEMY
            sb.Draw(Texture, Animations[CurrAnimation].AnimationRect, Ishurting ? Color.Purple : Color.White);
            //else EXP.Draw(sb, 2);
        }
    }
}
