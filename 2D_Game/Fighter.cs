﻿using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace _2D_Game
{
   public class Fighter : Player
    {
       public int Combointerval;
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
       public Fighter(Texture2D texture, PlayerIndex index, HealthBar hudz,Texture2D lower,Dictionary<string,AnimationNew> animations)
           : base(texture, index, hudz,lower)
       {
           SpriteWidth = 28;
           SpriteHeight = 32;
           Frameindex = 3;
           Upkey       = Keys.Up;
           Downkey     = Keys.Down;
           Leftkey     = Keys.Left;
           Rightkey    = Keys.Right;
           Attackkey   = Keys.Space;
           Sprintkey   = Keys.LeftShift;
           Alive = true;
           Type = "Fighter";
           Intervala = 300;
           UpperAnimations = animations;
           Animatetimer = 3;
       }
       //static public void Load(ContentManager Loader, string Root)
       //{
       //    Loader.RootDirectory = Root;
       //    attacktexturebot = Loader.Load<Texture2D>("attackrectbottom");
       //}
       /// <summary>
       /// Acts the specified gametime.
       /// </summary>
       /// <param name="tilemap">The tilemap.</param>
       public void Act(World world)
       {
           SetMoveVars();
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
                   Animatecounter = 3;
                   if (UpperAnimations[CurrAnimation].CurrFrame >= UpperAnimations[CurrAnimation].Frames - 2)
                   {
                       SetAttackAnimations();
                       UpperAnimations[CurrAnimation].CurrFrame = 0;
                   }
                   Attackmode = true;
               }
               if (Attackmode)
               {
                   if (UpperAnimations[CurrAnimation].CurrFrame >= UpperAnimations[CurrAnimation].Frames - 2)
                   {
                       SwapMovingAnimations();
                       MovementCollision(world);
                       if (Moving)
                           Attackmode = false;
                   }
               }
               AttackRectangle = UpperAnimations[CurrAnimation].Colliders[UpperAnimations[CurrAnimation].CurrFrame];
           }
           HandleNpcInventoryInput(world);
           UpdateAnimations();
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
           if (Right)
           {
               if (Up)
                   SwitchAnimation("AttackRight");
               else if (Down)
                   SwitchAnimation("AttackRight");
               else
                   SwitchAnimation("AttackRight");
           }
           if (Up)
               SwitchAnimation("AttackRight");
           if (Down)
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
           Combointerval = 0;
       }
       /// <summary>
       /// Draws fighter specific code.
       /// </summary>
       /// <param name="sb">spritebatch.</param>
       /// <param name="f">spritefont.</param>
       /// <param name="i">The i.</param>
       /// <param name="boundingbox">The boundingbox.</param>
       public override void Draw(SpriteBatch sb, SpriteFont f, Texture2D boundingbox,World world)
       {
           base.Draw(sb, f, boundingbox,world);
           if (Attackmode)
           {
               sb.DrawString(f,"ComboInterval: " + Combointerval,new Vector2(300,360),Color.Red);
           }
           sb.DrawString(f,"PosX: " + Position.X + "  PosY: " + Position.Y,new Vector2(300,260),Color.Blue);
           //sb.Draw(boundingbox, origin, Color.White);
           //sb.Draw(boundingbox, testbox, Color.White);
           //sb.Draw(boundingbox, collisionbox, Color.Blue);
           //sb.Draw(boundingbox, this.FeetBox, Color.White);
           //sb.Draw(boundingbox, this.FeetRect, Color.White);
           //sb.Draw(boundingbox, this.position, Color.White);
       }
    }
}