using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace _2D_Game
{
    public class Fighter : Player
    {
        public int MinFrameX;
        public int MaxFrameX;

        /// <summary>
        /// Initializes a new instance of the <see cref="Fighter"/> class.
        /// </summary>
        /// <param name="texture">The texture.</param>
        /// <param name="index">The index.</param>
        /// <param name="hudz">The hudz.</param>
        /// <param name="lower">The lower.</param>
        /// <param name="animationtext">The animationtext.</param>
        /// <param name="animations"></param>
        public Fighter(PlayerIndex index, ContentManager manager)
            : base(index)
        {
            this.SpriteTexture = manager.Load<Texture2D>("lowerPlayer");
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
            Type = "Fighter";
            var tempAnimations = World.LoadAnimations(Type);
            UpperAnimations = tempAnimations.Item2;
            this.SpriteTexture = manager.Load<Texture2D>(tempAnimations.Item1);
        }

        /// <summary>
        /// Acts the specified gametime.
        /// </summary>
        /// <param name="tilemap">The tilemap.</param>
        public void Act(World world)
        {
            SetMoveVars();
            SprintCheck();
            base.Act();
            SetMovementDirection();

            if (!Attackmode)
            {
                NoMovement();
                SwapMovingAnimations();
                MovementCollision(world);
            }
            if (!Activated)
            {
                //COMBO COUNTER
                if (CurrentKbState.IsKeyDown(Attackkey) && PreviousKbState.IsKeyUp(Attackkey))
                {
                    if (UpperAnimations[CurrAnimation].CurrFrame == UpperAnimations[CurrAnimation].Frames - 1)
                    {
                        SetAttackAnimations();
                        UpperAnimations[CurrAnimation].CurrFrame = 0;
                    }
                    Attackmode = true;
                }
                UpdateAnimations();
                if (Attackmode)
                {
                    //If Moving out of attack available
                    if (UpperAnimations[CurrAnimation].CurrFrame == UpperAnimations[CurrAnimation].Frames - 1)
                    {
                        //Move out of Attack
                        SwapMovingAnimations();
                        //Check Movement Collision
                        MovementCollision(world);
                        if (Moving)
                            Attackmode = false;
                    }
                    if (Attackmode)
                        AttackRectangle = UpperAnimations[CurrAnimation].ColliderRect;
                }
            }
            HandleNpcInventoryInput(world);
            AttackAdjustment(world);
        }
        private void SetAttackAnimations()
        {
            if (Left)
            {
                if (Up)
                    SwitchAnimation("AttackRight");
                else if (Down)
                    SwitchAnimation("AttackRight");
                else
                    SwitchAnimation("AttackRight");
            }
            else if (Right)
            {
                if (Up)
                    SwitchAnimation("AttackRight");
                else if (Down)
                    SwitchAnimation("AttackRight");
                else
                    SwitchAnimation("AttackRight");
            }
            else if (Up)
                SwitchAnimation("AttackRight");
            else if (Down)
                SwitchAnimation("AttackRight");
        }

        private void AttackAdjustment(World world)
        {
            Feetrectnew.Adjust(UpperAnimations[CurrAnimation].PosAdjust.X, 0);
            if (world.isColliding(Feetrectnew, (int)Playerindex) == false)
            {
                Position.X += UpperAnimations[CurrAnimation].PosAdjust.X;
            }
            Feetrectnew = Feetrect;
            Feetrectnew.Adjust(0, UpperAnimations[CurrAnimation].PosAdjust.Y);
            if (world.isColliding(Feetrectnew, (int)Playerindex) == false)
            {
                Position.X += UpperAnimations[CurrAnimation].PosAdjust.Y;
            }
            Position.Y += UpperAnimations[CurrAnimation].PosAdjust.Y;
        }

        /// <summary>
        /// Exits the attack.
        /// </summary>
        public void ExitAttack()
        {
            Attackmode = false;
        }

        /// <summary>
        /// Draws fighter specific code.
        /// </summary>
        /// <param name="sb">spritebatch.</param>
        /// <param name="f">spritefont.</param>
        /// <param name="i">The i.</param>
        /// <param name="boundingbox">The boundingbox.</param>
        public override void Draw(SpriteBatch sb, SpriteFont f, Texture2D boundingbox, World world)
        {
            base.Draw(sb, f, boundingbox, world);
            sb.DrawString(f, "PosX: " + Position.X + "  PosY: " + Position.Y, new Vector2(300, 260), Color.Blue);
            sb.DrawString(f, "SpriteSpeed: " + SpriteSpeed, new Vector2(300, 240), Color.Blue);
            //sb.Draw(boundingbox, origin, Color.White);
            //sb.Draw(boundingbox, testbox, Color.White);
            //sb.Draw(boundingbox, collisionbox, Color.Blue);
            //sb.Draw(boundingbox, this.FeetBox, Color.White);
            //sb.Draw(boundingbox, this.FeetRect, Color.White);
            //sb.Draw(boundingbox, this.position, Color.White);
        }
    }
}