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
        private string currAttackAnimation;
        private int currAttackFrame;
        public bool IsAttacking
        {
            get { return Attackmode; }
            set { Attackmode = value; }
        }

        public Rectangle TargetRect
        {
            get { return _targetrect; }
            set { _targetrect = value; }
        }

        public float TargetInterval { get; set; }

        public bool Shot { get; set; }

        public Mage(Texture2D texture, Texture2D target, PlayerIndex index, HealthBar hudx,string animationtext)
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
            Attackkey = Keys.C;
            Sprintkey = Keys.LeftShift;
            _secondattack = Keys.B;
            _targettexture = target;
            Alive = true;
            Type = "Mage";
            UpperAnimations = LoadAnimations(animationtext);
        }

        public override void Act(TileMap tilemap)
        {
            if (Alive)
            {
                SetMoveVars();
                base.Act(tilemap);
                SetMovementDirection();

                if (!Attackmode)
                {
                    NoMovement();
                    SwapMovingAnimations();
                    MovementCollision();
                }

                if (CurrentKbState.IsKeyDown(Attackkey))
                {
                    IsAttacking = true;
                }

                if (IsAttacking)
                {
                    if (Up)
                    {
                        if (Left)
                            currAttackAnimation = "AttackUpLeft";
                        else if (Right)
                            currAttackAnimation = "AttackUpRight";
                        else
                            currAttackAnimation = "AttackUp";
                        
                    }
                }
                if (!_usingsecondattack && !IsAttacking)
                {
                    SwapMovingAnimations();
                    MovementCollision();
                }
                HandleSpriteMovement();
                UpdateAnimations();
            }
        }

        public override void Draw(SpriteBatch sb, SpriteFont f, int i, Texture2D t)
        {
            base.Draw(sb ,f, i, t);
            if (Attackmode)
                sb.Draw(_targettexture, _targetrect, Color.White);
        }
    }
}
