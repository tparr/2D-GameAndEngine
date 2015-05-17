using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace _2D_Game
{
    class Mage : Player
    {
        Rectangle _targetrect;
        readonly Texture2D _targettexture;
        readonly Keys _secondattack;
        bool _usingsecondattack;
        enum ProjectileLevel
        {
            Level1,
            Level2
        }
        private string currAttackAnimation = "MainBall";
        private RectangleF energyBallRect;
        private int attacktimer;
        private int attacktimermax = 3;
        private List<Projectile> projectiles; 
        public Rectangle TargetRect
        {
            get { return _targetrect; }
            set { _targetrect = value; }
        }

        public float TargetInterval { get; set; }

        public bool Shot { get; set; }

        public Mage(Texture2D texture, Texture2D target, PlayerIndex index, HealthBar hudx,Dictionary<string, AnimationNew> animations)
            : base(texture, index, hudx, texture)
        {
            SpriteTexture = texture;
            Position = new Vector2(0,0);
            Frameindex = 2;
            SpriteWidth = 19;
            SpriteHeight = 29;
            Upkey = Keys.W;
            Downkey = Keys.S;
            Leftkey = Keys.A;
            Rightkey = Keys.D;
            Attackkey = Keys.Space;
            Sprintkey = Keys.LeftShift;
            _secondattack = Keys.B;
            _targettexture = target;
            Alive = true;
            Type = "Mage";
            UpperAnimations = animations;
            projectiles = new List<Projectile>();
        }

        public override void Act(TileMap tilemap)
        {
            if (Alive)
            {
                SetMoveVars();
                base.Act(tilemap);
                SetMovementDirection();
                NoMovement();
                SwapMovingAnimations();
                MovementCollision();
                if (CurrentKbState.IsKeyDown(Attackkey))
                {
                    Attackmode = true;
                    attacktimer += 2;
                    if (PreviousKbState.IsKeyUp(Attackkey))
                    {
                        currAttackAnimation = "MainBall";
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
                    if (attacktimer >= attacktimermax)
                    {
                        UpperAnimations[currAttackAnimation].Update();
                        attacktimer = 0;
                    }
                }
                if (CurrentKbState.IsKeyUp(Attackkey))
                {
                    if (Attackmode == false)
                    {
                        projectiles.Add(new Projectile());
                    }
                    Attackmode = false;
                }
                HandleNpcInventoryInput();
                Animatecounter += 2;
                if (Animatecounter >= Animatetimer)
                {
                    UpperAnimations[CurrAnimation].Update();
                    Animatecounter = 0;
                }
            }
        }

        public override void Draw(SpriteBatch sb, SpriteFont f, int i, Texture2D t)
        {
            base.Draw(sb ,f, i, t);
            if (Attackmode)
            {
                int currAttackFrame = UpperAnimations[currAttackAnimation].CurrFrame;
                sb.Draw(_targettexture, _targetrect, Color.White);
                sb.Draw(SpriteTexture,energyBallRect.ToRectangle(),UpperAnimations[currAttackAnimation].Animations[currAttackFrame],Color.White);
            }
        }
    }
}
