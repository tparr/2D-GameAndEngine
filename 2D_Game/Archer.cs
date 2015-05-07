using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace _2D_Game
{
    public class Archer : Player
    {
        float _aimrotation;
        public List<RotatedProjectile> Arrows;
        private readonly Texture2D _arrowtexture;
        public Archer(Texture2D texture, PlayerIndex index, HealthBar hudz,Texture2D arrowz)
            : base(texture, index, hudz, texture)
        {
            SpriteWidth = 28;
            SpriteHeight = 32;
            Frameindex = 3;
            Upkey = Keys.Up;
            Downkey = Keys.Down;
            Leftkey = Keys.Left;
            Rightkey = Keys.Right;
            Attackkey = Keys.Space;
            Sprintkey = Keys.LeftShift;
            Alive = true;
            Type = "Archer";
            Intervala = 400;
            Arrows = new List<RotatedProjectile>();
            _arrowtexture = arrowz;
            for (int i = 0; i < 5; i++)
                Arrows.Add(new RotatedProjectile());
        }

        public override void Act(TileMap tilemap)
        {
            base.Act(tilemap);
            SetMoveVars();
            if (Attackmode)
            {
                if (CurrentKbState.IsKeyUp(Attackkey))
                {
                    int index = 0;
                    for (int i = 0; i < Arrows.Count; i++)
                    {
                        if (Arrows[i].IsActive == false)
                        {
                            index = i;
                            break;
                        }
                    }
                    Arrows.Add(new RotatedProjectile());
                    Arrows[index] = new RotatedProjectile(new RotatedRectangle(new Rectangle((int)Position.X, (int)Position.Y, 7, 19), _aimrotation), _arrowtexture);
                    if (_aimrotation > 6.24f || _aimrotation < -6.24f)
                        _aimrotation = 0;
                    Arrows[index].Fire(new Vector2((float)Math.Cos(_aimrotation),(float)Math.Sin(_aimrotation)));
                    Arrows[index].IsActive = true;
                }
            }
            if (CurrentKbState.IsKeyUp(Attackkey))
                Attackmode = false;
            if (!Attackmode)
            {
                if (CurrentKbState.IsKeyDown(Attackkey))
                {
                    Attackmode = true;
                    if (Up)
                    {
                        if (Left)
                            _aimrotation = 5.6f;
                        else if (Right)
                            _aimrotation = 0.8f;
                        else _aimrotation = 0;
                    }
                    else if (Down)
                    {
                        if (Left)
                            _aimrotation = 4f;
                        else if (Right)
                            _aimrotation = 2.4f;
                        else _aimrotation = 3.2f;
                    }
                    else if (Right)
                    {
                        _aimrotation = 1.6f;
                    }
                    else
                        _aimrotation = 4.8f;
                }
                else
                    Attackmode = false;
            }
            if (Attackmode)
            {
                if (CurrentKbState.IsKeyDown(Keys.A))
                    _aimrotation -= .05f;
                if (CurrentKbState.IsKeyDown(Keys.D))
                    _aimrotation += .05f;
                if (_aimrotation > 6.24f || _aimrotation < -6.24f)
                    _aimrotation = 0;
            }
            if (!Attackmode)
            {
                SwapMovingAnimations();
                MovementCollision();
                _aimrotation = 0;
            }
            foreach (RotatedProjectile arrow in Arrows)
            {
                if (arrow.IsActive)
                    arrow.Update();
            }
        }

        public override void Draw(SpriteBatch sb,SpriteFont f,int i, Texture2D boundingbox)
        {
            sb.Draw(LowerTexture, Game1.CameraFix(Testbox).ToRectangle(),SourceRectBot.ToRectangle(), Color.White,_aimrotation,new Vector2((float)SpriteWidth / 2, (float)SpriteHeight / 2),SpriteEffects.None,0f);
            sb.DrawString(f, _aimrotation.ToString(CultureInfo.InvariantCulture), new Vector2(300, 210), Color.Red);
            foreach (RotatedProjectile arrow in Arrows)
                if (arrow.IsActive)
                    arrow.Draw(sb);
        }
    }
}
