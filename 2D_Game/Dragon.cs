using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Color = Microsoft.Xna.Framework.Color;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace _2D_Game
{
    class Dragon : Enemy
    {
        static public readonly int Maxshots = 100;
        int _shot;
        int _direction;
        Rectangle _rectangle;
        readonly Projectile[] _bullets = new Projectile[Maxshots];
        readonly Texture2D _projectile;

        public Projectile[] Bullets
        {
            get { return _bullets; }
        }

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
                    if (Ydiff < 100 && _shot < Maxshots)
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
                    if (Ydiff < 100 && _shot < Maxshots)
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
                    if (Xdiff < 100 && _shot < Maxshots)
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
                    if (Xdiff < 100 && _shot < Maxshots)
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
                else
                {
                    CurrentFramex = 0;
                    //BULLET SPEED SHOT
                    if (_shot < Maxshots)
                    {
                        if (Timera > Intervala)
                        {
                            if (Xdiff < 100 && Ydiff < 100)
                            {
                                if (Xdiff > Ydiff)
                                {
                                    if (Right)
                                    {
                                        CurrentFramey = 1;
                                        _direction = 1;
                                    }
                                    else
                                    {
                                        CurrentFramey = 2;
                                        _direction = 2;
                                    }
                                }
                                else
                                {
                                    if (Up)
                                    {
                                        CurrentFramey = 3;
                                        _direction = 3;
                                    }
                                    else
                                    {
                                        CurrentFramey = 0;
                                        _direction = 0;
                                    }
                                }

                                if (Xdiff > Ydiff)
                                {
                                    _bullets[_shot].RealX = Velocityx / 10;
                                    _bullets[_shot].RealY = 0;
                                }
                                else
                                {
                                    _bullets[_shot].RealY = Velocityy / 10;
                                    _bullets[_shot].RealX = 0;
                                }
                                if (!_bullets[_shot].IsActive)
                                {
                                    _bullets[_shot].Fire((int)Position.X, (int)Position.Y, _direction);
                                    _shot++;
                                }
                            }
                            Timera = 0f;
                        }
                    }
                }
            }
            Rect = new Rectangle((int)Position.X, (int)Position.Y, SpriteWidth, SpriteHeight);
        }

        public Dragon(Texture2D texture, int positonx, int positiony, Texture2D healthbar, Texture2D projectileTexture)
            :base(texture, positonx,positiony, healthbar)
        {
            SpriteWidth = 32;
            SpriteHeight = 32;
            _projectile = projectileTexture;
            MakeArray();
            Feetrectmodx = 8;
            Feetrectmody = 16;
            Feetrectwidth = 33;
            Feetrectheight = 8;
        }

        public void MakeArray()
        {
            for (int i = 0; i < _bullets.Length; i++)
                _bullets[i] = new Projectile(_projectile);
        }

        public void Draw(SpriteBatch sb, SpriteFont f, bool paused, Texture2D boundingbox, World world)
        {
                for (int i = 0; i < Maxshots; i++)
                {
                    if (_bullets[i].IsActive)
                    {
                        _bullets[i].Update(world.CameraX, world.CameraY, paused);
                        //sb.DrawString(f, "SPEEDX: " + bullets[i].RealX + "    " + i +"    SPEEDY: " + bullets[i].RealY, new Vector2(300, i * 20), Color.Red); 
                        _rectangle = _bullets[i].Hitbox;
                        //sb.Draw(_bullets[i].Texture, Game1.CameraFix(_rectangle), _bullets[i].SourceRect, Color.White);
                    }
                }
                //sb.Draw(boundingbox, rect, Color.White);
                //sb.Draw(boundingbox, hurtposition, Color.White);
                //sb.Draw(boundingbox, feetrect, Color.White);
                base.Draw(sb, f, paused, world);
            //sb.Draw(_bullets[0].Texture, Feetrect.ToRectangle(), Color.Red);
        }
    }
}