using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
namespace _2D_Game
{
    public class Projectile : Thing
    {
        static public Texture2D Texture;
        protected Dictionary<string, Animation> animations;
        private string currAnimation;
        private int _xMovement;
        private int _yMovement;
        protected bool _isactive;
        private int X;
        private int Y;
        private RotatedRectangle _hitbox;
        private int _cameraxp;
        private int _camerayp;
        public RotatedRectangle Hitbox
        {
            get { return _hitbox; }
            set { _hitbox = value; }
        }

        public int XMovement
        {
            get {return _xMovement;}
            set { _xMovement = value;}
        }

        public int YMovement
        {
            get { return _yMovement; }
            set { _yMovement = value; }
        }

        public bool IsActive
        {
            get { return _isactive; }
            set { _isactive = value; }
        }

        public Projectile(int xPos, int yPos, int xMovement, int yMovement, string projectileName)
        {
            X = xPos;
            Y = yPos;
            XMovement = xMovement;
            YMovement = yMovement;
            _isactive = true;
            animations = World.LoadAnimations(projectileName);
        }
        
        static public void InitializeProjectile(Texture2D texture)
        {
            Texture = texture;
        }

        public void Update(int camerax, int cameray, bool paused)
        {
            if (_isactive)
            {
                X += _xMovement;
                Y += _yMovement;

                _cameraxp = camerax;
                _camerayp = cameray;
                Animate();
                throw new NotImplementedException();
                //Hitbox = animations[currAnimation].ColliderRect;

                // If the bullet has moved off of the screen,
                // set it to inactive
                if ((X > _cameraxp + 800) || (X < _cameraxp)
                    || (Y > _camerayp + 600) || (Y < _camerayp))
                {
                    _isactive = false;
                }
            }
        }
        public void DrawText(SpriteBatch sb, SpriteFont f)
        {
            sb.DrawString(f, X.ToString(), new Vector2(500, 300), Color.Blue);
            sb.DrawString(f, Y.ToString(), new Vector2(550, 300), Color.Blue);
        }
        public void Animate()
        {
            bool movingLeft;
            bool movingRight;
            bool movingUp;
            bool movingDown;
            movingLeft = XMovement < 0;
            movingRight = XMovement > 0;
            movingDown = YMovement > 0;
            movingUp = YMovement < 0;
            if (movingLeft)
            {
                if (movingUp)
                    currAnimation = "UpLeft";
                else if (movingDown)
                    currAnimation = "DownLeft";
                else
                    currAnimation = "Left";
            }
            else if (movingRight)
            {
                if (movingUp)
                    currAnimation = "UpRight";
                else if (movingDown)
                    currAnimation = "DownRight";
                else
                    currAnimation = "Right";
            }
            else if (movingUp)
                currAnimation = "Up";
            else if (movingDown)
                currAnimation = "Down";
            if (currAnimation == null)
                currAnimation = "Down";
            animations[currAnimation].Update();
        }
        public void Draw(SpriteBatch sb, int camerax, int cameray)
        {
            _cameraxp = camerax;
            _camerayp = cameray;
            if (_isactive)
            {
                Rectangle drawRect = animations[currAnimation].AnimationRect;
                sb.Draw(Texture, new Rectangle(X, Y, drawRect.Width, drawRect.Height),drawRect, Color.White);
            }
        }
    }
}