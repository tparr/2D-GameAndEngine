using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
namespace _2D_Game
{
    class Squid : Enemy
    {
        public void Act()
        {
            SetHittable();
        }
        public override void Move(Rectangle playerbox, int enemyx, int enemyy, int current)
        {
            Feetrect = new RectangleF(Position.X + Feetrectmodx, Position.Y + Feetrectmody, 10, 5);
            Feetrectnew = Feetrect;
            throw new NotImplementedException();
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
                if (Xdiff > Ydiff)
                {
                    if (Right)
                    {
                        CurrAnimation = !Attacking ? "WalkRight" : "WalkRight";
                    }
                    if (Left)
                    {
                        CurrAnimation = !Attacking ? "WalkLeft" : "WalkLeft";
                    }
                }
                else
                {
                    if (Down)
                    {
                        CurrAnimation = !Attacking ? "WalkDown" : "WalkDown";
                    }
                    if (Up)
                    {
                        CurrAnimation = !Attacking ? "WalkUp" : "WalkUp";
                    }
                }
            }
        }

        public Squid(Texture2D texture, int positonx, int positiony, Texture2D healthbar)
            : base(texture, positonx, positiony, healthbar,"Squid")
        {
            Feetrectmodx = 8;
            Feetrectmody = 16;
            Feetrectwidth = 33;
            Feetrectheight = 8;
        }

        public override void Draw(SpriteBatch sb, SpriteFont spriteFont, bool paused, World world)
        {
            if (BActive)
            {
                Rectangle newrect = world.CameraFix(Rect);
                //DRAW HealthBAR
                sb.Draw(Healthbar, new Rectangle(newrect.X - 12, newrect.Y - 8, (int)(.5 * Health), 2), Color.White);
                //DRAW ENEMY
                Rectangle drawRect = Animations[CurrAnimation].AnimationRect;
                sb.Draw(Texture, new Rectangle(X,Y,drawRect.Width,drawRect.Height), drawRect, Ishurting ? Color.Purple : Color.White);
            }
            base.Draw(sb,spriteFont,paused,world);
            //else EXP.Draw(sb, 2);
        }
    }
}