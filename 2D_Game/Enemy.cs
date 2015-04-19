using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _2D_Game
{
    public class Enemy : Things
    {
        public int Health = 100;
        protected Vector2 Newpositionx;
        protected Vector2 Newpositiony;
        protected Texture2D Texture;
        protected Rectangle Rect;
        protected Rectangle Testrect;
        protected bool BActive;
        public bool Ishurting;
        protected Texture2D Healthbar;
        protected int Hurtinterval;
        protected int Velocityx;
        protected int Velocityy;
       // protected Rectangle feetrect;
        protected RectangleF Feetrectnew;
        protected float Timer;
        protected float Interval = 200f;
        protected float Timera;
        protected float Intervala = 400f;
        protected int CurrentFramex;
        protected int CurrentFramey;
        protected int SpriteWidth;
        protected int SpriteHeight;
        protected Rectangle SourceRect;
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
        protected Dictionary<string, AnimationNew> NewAnimations;
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
        protected Dictionary<string, AnimationNew> LoadAnimations(string fileLoc)
        {
            Dictionary<string, AnimationNew> baseNewAnimations = new Dictionary<string, AnimationNew>();
            string[] lines = File.ReadAllLines(fileLoc);
            string animname = "";
            int animlength = 0;
            for (int i = 0; i < lines.Length; i++)
            {
                string[] values = lines[i].Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                if (i % 2 == 0)
                {
                    animname = values[0];
                    animlength = Convert.ToInt32(values[1]);
                }
                else
                {
                    var rects = new List<Rectangle>();
                    for (int j = 0; j < animlength; j++)
                    {
                        var scale = j * 4;
                        Rectangle rect = new Rectangle(Convert.ToInt32(values[0 + scale]), Convert.ToInt32(values[1 + scale]),
                                                Convert.ToInt32(values[2 + scale]), Convert.ToInt32(values[3 + scale]));
                        rects.Add(rect);
                    }
                    //aseNewAnimations.Add(animname, new AnimationNew(animname, rects,new Vector2()));
                    rects.Clear();
                    Console.WriteLine();
                }
                if (baseNewAnimations.Count == lines.Length / 2)
                    return baseNewAnimations;
            }
            return baseNewAnimations;
        }

        public virtual void Act(Rectangle playerbox, int enemyx, int enemyy, int current, GameTime gametime)
        {
            SetHittable();
            Move(playerbox, enemyx, enemyy, current, gametime);
        }

        public Enemy(Texture2D texture, int positonx, int positiony, Texture2D healthbar)
        {
            Texture = texture;
            Position.X = positonx;
            Position.Y = positiony;
            Healthbar = healthbar;
            BActive = true;
        }

        public virtual void Move(Rectangle playerbox, int enemyx, int enemyy, int current, GameTime gametime)
        {
            SourceRect = new Rectangle(CurrentFramex * 32, CurrentFramey * 32, 32, 32);
            Feetrect = new RectangleF(Position.X + Feetrectmodx, Position.Y + Feetrectmody, 10, 5);
            Feetrectnew = Feetrect;
            Testrect = Rect;
            Timera += (float)gametime.ElapsedGameTime.TotalMilliseconds;
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
                if (Attacking == false)
                {
                    if (Xdiff > Ydiff)
                    {
                        if (Right)
                            AnimateRight(gametime);
                        if (Left)
                            AnimateLeft(gametime);
                    }
                    else
                    {
                        if (Down)
                            AnimateDown(gametime);
                        if (Up)
                            AnimateUp(gametime);
                    }
                }
                else
                {
                    CurrentFramex = 0;
                }
            }
            Rect = new Rectangle((int)Position.X, (int)Position.Y, SpriteWidth, SpriteHeight);
        }
        protected bool Colliding(int current, Rectangle playerbox)
        {
            if (!Game1.CalculateCollision(Feetrectnew)
                        && !Game1.EnemyVSenemycollision(current, Feetrectnew.ToRectangle())
                        && !(new Rectangle((int)Newpositionx.X, (int)Newpositionx.Y, 25, 25).Intersects(playerbox)))
                return true;
            return false;
        }

        #region Animation
        //ANIMATE RIGHT
        public void AnimateRight(GameTime gameTime)
        {
            CurrentFramey = 1;
            Timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (Timer > Interval)
            {
                CurrentFramex++;

                if (CurrentFramex > 3)
                {
                    CurrentFramex = 0;
                }
                Timer = 0f;
            }
        }
        //ANIMATE UP
        public void AnimateUp(GameTime gameTime)
        {
            CurrentFramey = 3;
            Timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (Timer > Interval)
            {
                CurrentFramex++;

                if (CurrentFramex > 3)
                {
                    CurrentFramex = 0;
                }
                Timer = 0f;
            }
        }
        //ANIMATE DOWN
        public void AnimateDown(GameTime gameTime)
        {
            CurrentFramey = 0;
            Timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (Timer > Interval)
            {
                CurrentFramex++;

                if (CurrentFramex > 3)
                {
                    CurrentFramex = 0;
                }
                Timer = 0f;
            }
        }
        //ANIMATE LEFT
        public void AnimateLeft(GameTime gameTime)
        {
            CurrentFramey = 2;
            Timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (Timer > Interval)
            {
                CurrentFramex++;

                if (CurrentFramex > 3)
                {
                    CurrentFramex = 0;
                }
                Timer = 0f;
            }
        }
        #endregion
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
            if (!Game1.CalculateCollision(Hurtposition))
                Feetrect = Hurtposition;
            else
                Hurtposition = Feetrect;
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

        public virtual void Draw(SpriteBatch sb, GameTime gametime, SpriteFont f, bool paused)
        {
            if (!BActive) return;
            Rectangle newrect = Game1.CameraFix(Rect);
            //DRAW HealthBAR
            sb.Draw(Healthbar, new Rectangle(newrect.X - 12, newrect.Y - 8, (int)(.5 * Health), 2), Color.White);
            //DRAW ENEMY
            sb.Draw(Texture, new Rectangle(newrect.X, newrect.Y, SpriteWidth, SpriteHeight), SourceRect,
                Ishurting ? Color.Purple : Color.White);
            //else EXP.Draw(sb, 2);
        }
    }
}
