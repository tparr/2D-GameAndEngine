using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace _2D_Game
{
    class Mage : Player
    {
        Rectangle _targetrect;
        readonly Texture2D _targettexture;
        enum CastingState
        {
            NotCasting,
            Casting
        };
        private RectangleF energyBallRect;
        private int attacktimer;
        private int attacktimermax = 3;
        CastingState attackState;
        public Rectangle TargetRect
        {
            get { return _targetrect; }
            set { _targetrect = value; }
        }

        public float TargetInterval { get; set; }

        public bool Shot { get; set; }

        public Mage(PlayerIndex index, ContentManager manager)
            : base(index)
        {
            SpriteTexture = manager.Load<Texture2D>("Mage");
            Position = new Vector2(0, 0);
            Upkey = Keys.W;
            Downkey = Keys.S;
            Leftkey = Keys.A;
            Rightkey = Keys.D;
            Attackkey = Keys.Space;
            Sprintkey = Keys.LeftShift;
            //_secondattack = Keys.B;
            _targettexture = manager.Load<Texture2D>("target_icon");
            Alive = true;
            Type = "Mage";
            UpperAnimations = World.LoadAnimations(Type).Item2;
            attackState = CastingState.NotCasting;
        }

        public void Act(World world)
        {
            if (Alive)
            {
                SetMoveVars();
                SprintCheck();
                base.Act();
                SetMovementDirection();
                SwapMovingAnimations();

                //If not casting or done casting allow movement
                if (!Attackmode)
                {
                    NoMovement();
                    MovementCollision(world);
                }
                if (CurrentKbState.IsKeyDown(Attackkey))
                {
                    Attackmode = true;
                    attacktimer += 2;
                    if (PreviousKbState.IsKeyUp(Attackkey))
                    {
                        attacktimer = 0;
                    }
                    if (Up)
                    {
                        if (Left)
                            energyBallRect = new RectangleF((int)Position.X - SpriteWidth, (int)Position.Y + 5, 12, 12);
                        else if (Right)
                            energyBallRect = new RectangleF((int)Position.X + SpriteWidth, (int)Position.Y + 5, 12, 12);
                        else
                            energyBallRect = new RectangleF((int)Position.X + (SpriteWidth / 2), (int)Position.Y - SpriteHeight, 12, 12);
                    }
                    else if (Down)
                    {
                        if (Left)
                            energyBallRect = new RectangleF((int)Position.X, (int)Position.Y, 12, 12);
                        else if (Right)
                            energyBallRect = new RectangleF((int)Position.X + SpriteWidth, (int)Position.Y + SpriteHeight, 12, 12);
                        else
                        {
                            energyBallRect = new RectangleF((int)Position.X, (int)Position.Y, 12, 12);
                        }
                    }
                    else if (Right)
                        energyBallRect = new RectangleF((int)Position.X + SpriteWidth, (int)Position.Y, 12, 12);
                    else//If Left
                    {
                        energyBallRect = new RectangleF((int)Position.X - SpriteWidth, (int)Position.Y, 12, 12);
                    }
                    UpperAnimations[CurrAnimation].Update();
                    if (Attackmode == true)
                    {
                        world.AddEntity(new Projectile((int)Position.X, (int)Position.Y, Velocityx, Velocityy, "EnergyBall"));
                    }
                    Attackmode = false;

                }
                HandleNpcInventoryInput(world);
                UpperAnimations[CurrAnimation].Update();
            }
        }

        public override void Draw(SpriteBatch sb, SpriteFont f, Texture2D t, World world)
        {
            base.Draw(sb, f, t, world);
            if (Attackmode)
            {
                //int currAttackFrame = UpperAnimations[currAttackAnimation].CurrFrame;
                sb.Draw(_targettexture, TargetRect, Color.White);
                //sb.Draw(SpriteTexture,energyBallRect.ToRectangle(),UpperAnimations[currAttackAnimation].Animations[currAttackFrame],Color.White);
            }
        }
    }
}