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
       public Fighter(Texture2D texture, PlayerIndex index, HealthBar hudz,Texture2D lower,string animationtext)
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
           UpperAnimations = LoadAnimations(animationtext);
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
       public override void Act(TileMap tilemap)
       {
           SetMoveVars();
           base.Act(tilemap);
           CheckMovementInput();
           if (!Attackmode)
           {
               NoMovement();
               CheckMoving();
               MovementCollision();
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
                       CheckMoving();
                       MovementCollision();
                       if (Moving)
                           Attackmode = false;
                   }
               }
               AttackRectangle = UpperAnimations[CurrAnimation].Colliders[UpperAnimations[CurrAnimation].CurrFrame];
           }
           HandleSpriteMovement();
           UpdateAnimations();
           AttackAdjustment();
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

       /// <summary>
       /// Updates the animations.
       /// </summary>
       private void UpdateAnimations()
       {
           if (UpperAnimations[CurrAnimation].Played && Attackmode)
           {
               if (Up)
                   SwitchAnimation("StandUp");
               if (Left)
                   SwitchAnimation("StandLeft");
               if (Right)
                   SwitchAnimation("StandRight");
               if (Down)
                   SwitchAnimation("StandDown");

           }
           Animatecounter += 2;
           if (Animatecounter >= Animatetimer)
           {
               UpperAnimations[CurrAnimation].Update();
               Animatecounter = 0;
           }
       }

       private void AttackAdjustment()
       {
           Feetrectnew.Adjust(UpperAnimations[CurrAnimation].PosAdjust.X, 0);
           Game1.PlayerVSplayercollision1(Feetrectnew, Playerindex);
           if (Game1.Check_Collisions(Feetrectnew, Playerindex))
           {
               Position.X += UpperAnimations[CurrAnimation].PosAdjust.X;
           }
           Feetrectnew = Feetrect;
           Feetrectnew.Adjust(0, UpperAnimations[CurrAnimation].PosAdjust.Y);
           Game1.PlayerVSplayercollision1(Feetrectnew, Playerindex);
           if (Game1.Check_Collisions(Feetrectnew, Playerindex))
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
       public override void Draw(SpriteBatch sb, SpriteFont f, int i, Texture2D boundingbox)
       {
           base.Draw(sb, f, i, boundingbox);
           if (Attackmode)
           {
               sb.DrawString(f,"ComboInerval: " + Combointerval,new Vector2(300,360),Color.Red);
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