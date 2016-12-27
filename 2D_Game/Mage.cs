using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace _2D_Game
{
    internal class Mage : Player
    {
        private Rectangle _targetrect;
        private readonly Texture2D _targettexture;

        private enum CastingState
        {
            NotCasting,
            Casting
        };

        private RectangleF energyBallRect;
        private int attacktimer;
        private int attacktimermax = 3;
        private CastingState attackState;

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
            IsAlive = true;
            Type = "Mage";
            Animations = World.LoadAnimations(Type).Item2;
            attackState = CastingState.NotCasting;
        }

        public void Act(World world)
        {
            if (IsAlive)
            {
                UpdateAndInitializeMovementVariables();
                SprintCheck();
                base.Act();
                SetMovementDirection();
                SwapMovingAnimations();

                //If not casting or done casting allow movement
                if (!IsAttacking)
                {
                    NoMovement();
                    MovementCollision(world);
                }
                if (CurrentKbState.IsKeyDown(Attackkey))
                {
                    IsAttacking = true;
                    attacktimer += 2;
                    if (PreviousKbState.IsKeyUp(Attackkey))
                    {
                        attacktimer = 0;
                    }
                    var spriteWidth = CurrentAnimation.AnimationRect.Width;
                    var spriteHeight = CurrentAnimation.AnimationRect.Height;
                    if (Up)
                    {
                        if (Left)
                            energyBallRect = new RectangleF((int)Position.X - spriteWidth, (int)Position.Y + 5, 12, 12);
                        else if (Right)
                            energyBallRect = new RectangleF((int)Position.X + spriteWidth, (int)Position.Y + 5, 12, 12);
                        else
                            energyBallRect = new RectangleF((int)Position.X + (spriteWidth / 2), (int)Position.Y - spriteHeight, 12, 12);
                    }
                    else if (Down)
                    {
                        if (Left)
                            energyBallRect = new RectangleF((int)Position.X, (int)Position.Y, 12, 12);
                        else if (Right)
                            energyBallRect = new RectangleF((int)Position.X + spriteWidth, (int)Position.Y + spriteHeight, 12, 12);
                        else
                        {
                            energyBallRect = new RectangleF((int)Position.X, (int)Position.Y, 12, 12);
                        }
                    }
                    else if (Right)
                        energyBallRect = new RectangleF((int)Position.X + spriteWidth, (int)Position.Y, 12, 12);
                    else//If Left
                    {
                        energyBallRect = new RectangleF((int)Position.X - spriteWidth, (int)Position.Y, 12, 12);
                    }
                    CurrentAnimation.Update();
                    if (IsAttacking == true)
                    {
                        world.AddEntity(new Projectile((int)Position.X, (int)Position.Y, Velocityx, Velocityy, "EnergyBall"));
                    }
                    IsAttacking = false;
                }
                HandleNpcInventoryInput(world);
                CurrentAnimation.Update();
            }
        }

        public override void Draw(SpriteBatch sb, SpriteFont f, Texture2D t, World world)
        {
            base.Draw(sb, f, t, world);
            if (IsAttacking)
            {
                //int currAttackFrame = UpperAnimations[currAttackAnimation].CurrFrame;
                sb.Draw(_targettexture, TargetRect, Color.White);
                //sb.Draw(SpriteTexture,energyBallRect.ToRectangle(),UpperAnimations[currAttackAnimation].Animations[currAttackFrame],Color.White);
            }
        }
    }
}