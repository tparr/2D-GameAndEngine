using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace _2D_Game
{
    public class Npc : Thing
    {
        protected Texture2D SpriteTexture;
        protected int Spritewidth = 19;
        protected int Spriteheight = 28;
        protected int CurrentFramex;
        protected int CurrentFramey;
        public int SpriteSpeed = 1;
        public Rectangle SourceRect;
        protected Vector2 Newpositionx;
        protected Vector2 Newpositiony;

        // protected Rectangle feetrect;
        protected RectangleF Feetrectnew;

        protected int Frameindex = 2;
        protected float Interval = 200f;
        protected float Timer;
        protected bool Left, Right, Up, Down = true;
        private readonly Random _generator = new Random();
        private int _moveTimer;
        public bool Activated;
        public Vector2 Position;
        private const string Message = "Hello Citizen";

        public Texture2D Texture
        {
            get { return SpriteTexture; }
            set { SpriteTexture = value; }
        }

        public RectangleF FeetBox
        {
            get { return Feetrect; }
            set { Feetrect = value; }
        }

        public int CurrentFx
        {
            get { return CurrentFramex; }
            set { CurrentFramex = value; }
        }

        public int CurrentFy
        {
            get { return CurrentFramey; }
            set { CurrentFramey = value; }
        }

        public Npc(Texture2D texture, int px, int py)
        {
            SpriteTexture = texture;
            Position = new Vector2(px, py);
        }

        public void Move()
        {
            throw new NotImplementedException();
            //if (!Activated)
            //{
            //    Newpositionx = Position;
            //    Newpositiony = Position;
            //    Feetrect = new RectangleF(Position.X + 3, Position.Y + 23, 10, 5);
            //    Feetrectnew = Feetrect;
            //    SourceRect = new Rectangle(19 * CurrentFramex, 28 * CurrentFramey, 19, 28);
            //    if (Down)
            //    {
            //        Feetrectnew = Feetrect;
            //        CurrentFramey = 0;
            //        AnimateDown(gameTime);
            //        Feetrectnew.Y += SpriteSpeed;
            //        if (!Game1.CalculateCollision(Feetrectnew))
            //        {
            //            Position.Y += SpriteSpeed;
            //        }
            //        else CurrentFramex = 0;
            //    }
            //    if (Up)
            //    {
            //        Feetrectnew = Feetrect;
            //        CurrentFramey = 3;
            //        AnimateUp(gameTime);
            //        Feetrectnew.Y -= SpriteSpeed;
            //        if (!Game1.CalculateCollision(Feetrectnew))
            //        {
            //            Position.Y -= SpriteSpeed;
            //        }
            //        else CurrentFramex = 0;
            //    }
            //    if (Left)
            //    {
            //        Feetrectnew = Feetrect;
            //        CurrentFramey = 2;
            //        AnimateLeft(gameTime);
            //        Feetrectnew.X -= SpriteSpeed;
            //        if (!Game1.CalculateCollision(Feetrectnew))
            //        {
            //            Position.X -= SpriteSpeed;
            //        }
            //        else CurrentFramex = 0;
            //    }
            //    if (Right)
            //    {
            //        Feetrectnew = Feetrect;
            //        CurrentFramey = 1;
            //        AnimateRight(gameTime);
            //        Feetrectnew.X += SpriteSpeed;
            //        if (!Game1.CalculateCollision(Feetrectnew))
            //        {
            //            Position.X += SpriteSpeed;
            //        }
            //        else CurrentFramex = 0;
            //    }
            //}
        }

        protected void FaceDown()
        {
            Down = true;
            Up = false;
            Left = false;
            Right = false;
        }

        protected void FaceUp()
        {
            Up = true;
            Down = false;
            Left = false;
            Right = false;
        }

        protected void FaceLeft()
        {
            Left = true;
            Right = false;
            Down = false;
            Up = false;
        }

        protected void FaceRight()
        {
            Right = true;
            Left = false;
            Down = false;
            Up = false;
        }

        //ANIMATE RIGHT
        public void AnimateRight(GameTime gameTime)
        {
            Timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (Timer > Interval)
            {
                CurrentFramex++;

                if (CurrentFramex > Frameindex)
                {
                    CurrentFramex = 0;
                }
                Timer = 0f;
            }
        }

        //ANIMATE UP
        public void AnimateUp(GameTime gameTime)
        {
            Timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (Timer > Interval)
            {
                CurrentFramex++;

                if (CurrentFramex > Frameindex)
                {
                    CurrentFramex = 0;
                }
                Timer = 0f;
            }
        }

        //ANIMATE DOWN
        public void AnimateDown(GameTime gameTime)
        {
            Timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (Timer > Interval)
            {
                CurrentFramex++;

                if (CurrentFramex > Frameindex)
                {
                    CurrentFramex = 0;
                }
                Timer = 0f;
            }
        }

        //ANIMATE LEFT
        public void AnimateLeft(GameTime gameTime)
        {
            Timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (Timer > Interval)
            {
                CurrentFramex++;

                if (CurrentFramex > Frameindex)
                {
                    CurrentFramex = 0;
                }
                Timer = 0f;
            }
        }

        public virtual void Draw(SpriteBatch sb, SpriteFont f, Texture2D boundingbox, bool notSeller, World world)
        {
            sb.Draw(SpriteTexture, world.CameraFix(Position), SourceRect, Color.White);
            if (Activated)
            {
                sb.DrawString(f, Message, world.CameraFix(new Vector2(Position.X - (Message.Length * 4), Position.Y - 20)), Color.Red);
                if (notSeller)
                    sb.Draw(boundingbox, new Rectangle(0, 0, 100, 100), Color.Black);
            }
            sb.Draw(boundingbox, Feetrect.ToRectangle(), Color.Red);
        }

        public virtual void Act()
        {
            if (!Activated)
            {
                _moveTimer++;
                if (_moveTimer <= 60)
                {
                    Move();
                }
                if (_moveTimer > 60 && _moveTimer < 120)
                {
                    CurrentFramex = 0;
                    SourceRect = new Rectangle(19 * CurrentFramex, 28 * CurrentFramey, 19, 28);
                }
                if (_moveTimer >= 120)
                {
                    int i = _generator.Next(4);
                    if (i == 0)
                        FaceUp();
                    if (i == 1)
                        FaceLeft();
                    if (i == 2)
                        FaceDown();
                    if (i == 3)
                        FaceRight();
                    _moveTimer = 0;
                }
            }
        }
    }

    public class Seller : Npc
    {
        private Inventory _items = new Inventory(5, 2);

        public Inventory Inventory
        {
            get { return _items; }
            set { _items = value; }
        }

        public Seller(Texture2D texture, int px, int py)
            : base(texture, px, py)
        {
            Texture = texture;
            Frameindex = 1;
            CurrentFramey = 0;
        }

        public override void Act()
        {
            Feetrect = new RectangleF(Position.X + 3, Position.Y + 23, 10, 5);
            SourceRect = new Rectangle(19 * CurrentFramex, 28 * CurrentFramey, 19, 28);
            //AnimateDown(gameTime);
        }

        public override void Draw(SpriteBatch sb, SpriteFont f, Texture2D boundingbox, bool notSeller, World world)
        {
            base.Draw(sb, f, boundingbox, false, world);
        }
    }

    public class Chest : Npc
    {
        private bool _opened;
        private Inventory _items = new Inventory(5, 2);

        public Inventory Inventory
        {
            get { return _items; }
            set { _items = value; }
        }

        public Chest(Texture2D texture, int px, int py)
            : base(texture, px, py)
        {
            Frameindex = 1;
            Spritewidth = 32;
            Spriteheight = 32;
            Feetrect = new RectangleF(Position.X, Position.Y, 32, 32);
            Feetrectnew = Feetrect;
        }

        public override void Act()
        {
            if (_opened == false && Activated)
                _opened = true;
            SourceRect = !_opened ? new Rectangle(0, 0, 32, 32) : new Rectangle(32, 0, 32, 32);
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(Texture, Feetrect.ToRectangle(), SourceRect, Color.White);
        }
    }
}