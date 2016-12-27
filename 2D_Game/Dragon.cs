using Microsoft.Xna.Framework.Graphics;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace _2D_Game
{
    internal class Dragon : Enemy
    {
        private int _shot;
        private int _direction;
        private Rectangle _rectangle;

        public int Shot
        {
            get { return _shot; }
        }

        public Rectangle TestBox
        {
            get { return _rectangle; }
        }

        public override void Act(Rectangle playerbox, int enemyx, int enemyy, int current)
        {
            SetHittable();
            Move(playerbox, enemyx, enemyy, current);
        }

        public override void Move(Rectangle playerbox, int enemyx, int enemyy, int current)
        {
            //SourceRect = new Rectangle(currentFramex * 32, currentFramey * 32, 32, 32);
            //feetrect = new Rectangle((int)position.X + 8, (int)position.Y + 25, 33, 8);
            //feetrectnew = feetrect;
            //timera += (float)gametime.ElapsedGameTime.TotalMilliseconds;
            //if (Health <= 0)
            //    Dead();
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
                    if (Ydiff < 100)
                        Attacking = true;
                    else Attacking = false;
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
                    if (Ydiff < 100)
                        Attacking = true;
                    else Attacking = false;
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
                    if (Xdiff < 100)
                        Attacking = true;
                    else Attacking = false;
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
                    if (Xdiff < 100)
                        Attacking = true;
                    else Attacking = false;
                    if (!Attacking)
                        if (Colliding(current, playerbox))
                            Position.X = Newpositionx.X;
                }
                else Velocityx = 0;
                if (Attacking == false)
                {
                    //if (Xdiff > Ydiff)
                    //{
                    //    if (Right)
                    //        AnimateRight(gametime);
                    //    if (Left)
                    //        AnimateLeft(gametime);
                    //}
                    //else
                    //{
                    //    if (Down)
                    //        AnimateDown(gametime);
                    //    if (Up)
                    //        AnimateUp(gametime);
                    //}
                }
            }
        }

        public Dragon(Texture2D texture, int positonx, int positiony, Texture2D healthbar, Texture2D projectileTexture)
            : base(texture, positonx, positiony, healthbar, "Dragon")
        {
        }

        public void Draw(SpriteBatch sb, SpriteFont f, bool paused, Texture2D boundingbox, World world)
        {
            //sb.Draw(boundingbox, rect, Color.White);
            //sb.Draw(boundingbox, hurtposition, Color.White);
            //sb.Draw(boundingbox, feetrect, Color.White);
            base.Draw(sb, f, paused, world);
            //sb.Draw(_bullets[0].Texture, Feetrect.ToRectangle(), Color.Red);
        }
    }
}