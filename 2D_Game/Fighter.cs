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

        public Fighter(PlayerIndex index, ContentManager manager)
            : base(index)
        {
            Upkey = Keys.Up;
            Downkey = Keys.Down;
            Leftkey = Keys.Left;
            Rightkey = Keys.Right;
            Attackkey = Keys.Space;
            Sprintkey = Keys.LeftShift;
            IsAlive = true;
            Type = "Fighter";
            var tempAnimations = World.LoadAnimations(Type);
            Animations = tempAnimations.Item2;
            SpriteTexture = manager.Load<Texture2D>(tempAnimations.Item1);
        }

        public void Act(World world)
        {
            base.Act();
            NoMovement();
            SwapMovingAnimations();

            if (!IsAttacking)
            {
                MovementCollision(world);
            }
            if (!IsInteractingwithSomething)
            {
                //COMBO COUNTER
                if (CurrentKbState.IsKeyDown(Attackkey) && PreviousKbState.IsKeyUp(Attackkey))
                {
                    if (CurrentAnimation.CurrFrame == CurrentAnimation.Frames - 1)
                    {
                        SetAttackAnimations();
                        CurrentAnimation.CurrFrame = 0;
                    }
                    IsAttacking = true;
                }
                UpdateAnimations();
                if (IsAttacking)
                {
                    //If Moving out of attack available
                    if (CurrentAnimation.CurrFrame == CurrentAnimation.Frames - 1)
                    {
                        //Move out of Attack
                        SwapMovingAnimations();
                        //Check Movement Collision
                        MovementCollision(world);
                        if (IsMoving)
                            IsAttacking = false;
                    }
                    if (IsAttacking)
                        AttackCollider = CurrentAnimation.Collider;
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
            Feetrectnew.Adjust(CurrentAnimation.PosAdjust.X, 0);
            if (world.isColliding(Feetrectnew, (int)Playerindex) == false)
            {
                Position.X += CurrentAnimation.PosAdjust.X;
            }
            Feetrectnew = Feetrect;
            Feetrectnew.Adjust(0, CurrentAnimation.PosAdjust.Y);
            if (world.isColliding(Feetrectnew, (int)Playerindex) == false)
            {
                Position.X += CurrentAnimation.PosAdjust.Y;
            }
            Position.Y += CurrentAnimation.PosAdjust.Y;
        }

        /// <summary>
        /// Exits the attack.
        /// </summary>
        public void ExitAttack()
        {
            IsAttacking = false;
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
            sb.DrawString(f, "SpriteSpeed: " + CharacterSpeed, new Vector2(300, 240), Color.Blue);
            //sb.Draw(boundingbox, origin, Color.White);
            //sb.Draw(boundingbox, testbox, Color.White);
            //sb.Draw(boundingbox, collisionbox, Color.Blue);
            //sb.Draw(boundingbox, this.FeetBox, Color.White);
            //sb.Draw(boundingbox, this.FeetRect, Color.White);
            //sb.Draw(boundingbox, this.position, Color.White);
        }
    }
}