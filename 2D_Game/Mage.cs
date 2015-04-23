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

        public Mage(Texture2D texture, Texture2D target, PlayerIndex index, HealthBar hudx)
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
        }

        public override void Act(GameTime gametime,TileMap tilemap)
        {
            if (Alive)
            {
                base.Act(gametime, tilemap);
                SetMoveVars();

                if (IsAttacking == false)
                    _targetrect = new Rectangle((int)Position.X - SpriteWidth, (int)Position.Y - SpriteWidth, 32, 32);

                if (CurrentKbState.IsKeyDown(Attackkey) && PreviousKbState.IsKeyUp(Attackkey) && !_usingsecondattack)
                {
                    IsAttacking = true;
                }

                if (CurrentKbState.IsKeyDown(_secondattack) && PreviousKbState.IsKeyUp(_secondattack) && !IsAttacking)
                {
                    _usingsecondattack = true;
                }

                if (IsAttacking)
                {
                    if (CurrentKbState.IsKeyDown(Upkey))
                        _targetrect.Y -= 3;
                    if (CurrentKbState.IsKeyDown(Downkey))
                        _targetrect.Y += 3;
                    if (CurrentKbState.IsKeyDown(Rightkey))
                        _targetrect.X += 3;
                    if (CurrentKbState.IsKeyDown(Leftkey))
                        _targetrect.X -= 3;

                    if (CurrentKbState.IsKeyDown(Keys.V))
                        IsAttacking = false;
                    if (CurrentKbState.IsKeyUp(Attackkey))
                        Shot = true;
                    if (Shot)
                        TargetInterval += 5;

                }
                if (!_usingsecondattack && !IsAttacking)
                {
                    CheckMoving();
                    MovementCollision(gametime);
                }
                HandleSpriteMovement(gametime);
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
