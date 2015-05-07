using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
namespace _2D_Game
{
    public class Player : Things
    {
        protected int Animatecounter;
        protected int Animatetimer = 3;
        //protected Rectangle attackrect;
        private int _index;
        private bool _inventoryMenu;
        private int _invx, _invy;
        private Vector2 _newvect;
        private Inventory _sellerInventory;
        protected bool Activated;
        public bool Alive;
        protected Keys Attackkey;
        public bool Attackmode = false;
        public RectangleF Collisionbox;
        public string CurrAnimation;
        protected KeyboardState CurrentKbState;
        protected Keys Downkey;
        protected RectangleF Feetrectnew;
        protected int Frameindex;
        public int Health = 100;
        public HealthBar Hud;
        protected int Hurtinterval;
        protected float Interval = 200f;
        public float Intervala = 200f;
        protected Inventory Inventory = new Inventory(1, 1);
        public bool Ishurting;
        protected bool Left, Right, Up, Down,LeftPressed,RightPressed,DownPressed,UpPressed;
        protected Keys Leftkey;
        public int Magic = 100;
        protected Dictionary<string, AnimationNew> UpperAnimations;
        protected Dictionary<string, AnimationNew> LowerAnimation;
        protected Vector2 Newpositionx;
        protected Vector2 Newpositiony;
        public Vector2 Origin;
        public  readonly PlayerIndex Playerindex;
        public Vector2 Position;
        public float Positionx;
        public float Positiony;
        protected KeyboardState PreviousKbState;
        protected Keys Rightkey;
        public RectangleF SourceRectBot;
        public RectangleF SourceRectTop;
        protected Keys Sprintkey;
        protected int SpriteHeight;
        public Texture2D SpriteTexture;
        protected int SpriteWidth;
        public RectangleF Testbox;
        protected float Timer;
        public float Timera;
        protected RectangleF Touch;
        public String Type = "Player";
        protected Keys Upkey;
        public int Velocityx;
        public int Velocityy;
        protected int Xoffset = 0;
        public int Xp;
        protected int Yoffset = 0;
        public Texture2D LowerTexture;
        protected bool Moving;
        public int SpriteSpeed { get; protected set; }
        public RotatedRectangle AttackRectangle;
        public Player(PlayerIndex index)
        {
            SpriteSpeed = 2;
            Timera = 0f;
            Playerindex = index;
        }

        public Player(Texture2D texture, PlayerIndex index, HealthBar huds, Texture2D lower)
        {
            SpriteSpeed = 2;
            SpriteTexture = texture;
            Position = new Vector2(500 , 300);
            Alive = true;
            Playerindex = index;
            Hud = new HealthBar(huds.BackgroundImage, huds.HealthTex, huds.MagicTex, huds.ExpTex) {Px = (int) index};
            _sellerInventory = new Inventory();
            LowerTexture = lower;
            CurrAnimation = "StandDown";
        }

        protected Dictionary<string, AnimationNew> LoadAnimations(string filename)
        {
            var animations = new Dictionary<string, AnimationNew>();
            var lines = File.ReadAllLines(filename);
            var animname = "";
            var animlength = 0;
            Vector2 posAdjust = new Vector2();
            var xoffset = 0;
            var yoffset = 0;
            List<RotatedRectangle> collisions = new List<RotatedRectangle>();
            var lowerrects = new List<Rectangle>();
            int counter = 0;
            foreach (var values in lines.Select(line => line.Split(new[] {";"}, StringSplitOptions.RemoveEmptyEntries)))
            {
                switch (counter)
                {
                    case 0:
                        animname = values[0];
                        animlength = Convert.ToInt32(values[1]);
                        posAdjust.X = (float) Convert.ToDouble(values[2]);
                        posAdjust.Y = (float) Convert.ToDouble(values[3]);
                        xoffset = Convert.ToInt32(values[4]);
                        yoffset = Convert.ToInt32(values[5]);
                        break;
                    case 1:
                        for (var j = 0; j < animlength; j++)
                        {
                            var scale = j*4;
                            var rect = new Rectangle(Convert.ToInt32(values[0 + scale]),
                                Convert.ToInt32(values[1 + scale]),
                                Convert.ToInt32(values[2 + scale]), Convert.ToInt32(values[3 + scale]));
                            lowerrects.Add(rect);
                        }
                        break;
                    case 2:
                        for (int i = 0; i < values.Length / 5; i++)
                        {
                            var scale = i * 5;
                            collisions.Add(
                                new RotatedRectangle(
                                    new Rectangle(
                                        Convert.ToInt32(values[0 + scale]),
                                        Convert.ToInt32(values[1 + scale]),
                                        Convert.ToInt32(values[2 + scale]),
                                        Convert.ToInt32(values[3 + scale])),
                                 (float)Convert.ToDouble(values[4 + scale])));
                        }
                        animations.Add(animname, new AnimationNew(animname, lowerrects, posAdjust, collisions, xoffset, yoffset));
                        lowerrects.Clear();
                        collisions.Clear();
                        Console.WriteLine();
                        break;
                }
                if (animations.Count == lines.Length / 2)
                    return animations;
                counter++;
                if (counter >= 3)
                    counter = 0;
            }
            return animations;
        }
        /// <summary>
        /// Updates the animations.
        /// </summary>
        protected void UpdateAnimations()
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
        public void HandleSpriteMovement()
        {
            if (Alive)
            {
                //if not attacking and attacking pressed
                Origin = new Vector2(SourceRectTop.Width/2, SourceRectTop.Height/2);
                if (CurrentKbState.IsKeyDown(Attackkey) && PreviousKbState.IsKeyUp(Attackkey) && !Attackmode)
                {
                    if (Up)
                    {
                        Touch = new RectangleF(Positionx - 10, Positiony, 14, 10);
                        //attackrect = new Rectangle(positionx - 20, positiony - 40, 40, 30);
                    }
                    else if (Down)
                    {
                        Touch = new RectangleF(Positionx - 8, Positiony + 7, 14, 10);
                        //attackrect = new Rectangle(positionx - 20, positiony + 10, 40, 30);
                    }
                    else if (Left)
                    {
                        Touch = new RectangleF(Positionx - 16, Positiony + 5, 10, 10);
                        //attackrect = new Rectangle(positionx - 30, positiony - 20, 30, 40);
                    }
                    else
                    {
                        Touch = new RectangleF(Positionx - 3, Positiony + 5, 10, 10);
                        //attackrect = new Rectangle(positionx + 5, positiony - 20, 30, 40);
                    }
                    //Exit activation
                    if (Activated)
                    {
                        Activated = false;
                        Game1.Npcs[_index].Activated = false;
                    }
                    //Activate
                    else
                    {
                        for (var i = 0; i < Game1.Npcs.Count; i++)
                            if ((Game1.Npcs[i].FeetBox.Intersects(Touch)))
                            {
                                _index = i;
                                Activated = true;
                                Game1.Npcs[_index].Activated = true;
                                if (Game1.Npcs[_index].GetType() == typeof (Seller))
                                    _sellerInventory = ((Seller) Game1.Npcs[_index]).Inventory;
                                if (Game1.Npcs[_index].GetType() == typeof (Chest))
                                    _sellerInventory = ((Chest) Game1.Npcs[_index]).Inventory;
                            }
                    }
                }
                //Activated controls
                if (Activated)
                {
                    if (CurrentKbState.IsKeyDown(Leftkey) && PreviousKbState.IsKeyUp(Leftkey))
                        if (_invx - 1 >= 0)
                            _invx--;
                    if (CurrentKbState.IsKeyDown(Rightkey) && PreviousKbState.IsKeyUp(Rightkey))
                        if (_invx + 1 < _sellerInventory.Items[_invy].Count)
                            _invx++;
                    if (CurrentKbState.IsKeyDown(Downkey) && PreviousKbState.IsKeyUp(Downkey))
                        if (_invy + 1 < _sellerInventory.Items.Count)
                            _invy++;
                    if (CurrentKbState.IsKeyDown(Upkey) && PreviousKbState.IsKeyUp(Upkey))
                        if (_invy - 1 >= 0)
                            _invy--;
                }
            }
            //Reset hit timer
            SetHittable();
        }
        protected void SetMoveVars()
        {
            Newpositionx = Position;
            Newpositiony = Position;
            PreviousKbState = CurrentKbState;
            CurrentKbState = Keyboard.GetState();
            Positionx = (int) Position.X;
            Positiony = (int) Position.Y;
            Testbox = new RectangleF(Positionx, Positiony, SpriteWidth, SpriteHeight);
            Feetrect = new RectangleF(Positionx + 8, Positiony + 26, 10, 5);
            Feetrectnew = Feetrect;
        }
        protected void SetMovementDirection()
        {
            //Check if movement keys are pressed
            LeftPressed = CurrentKbState.IsKeyDown(Leftkey);
            RightPressed = CurrentKbState.IsKeyDown(Rightkey);
            UpPressed = CurrentKbState.IsKeyDown(Upkey);
            DownPressed = CurrentKbState.IsKeyDown(Downkey);
            if (LeftPressed || RightPressed || UpPressed || DownPressed)
                Moving = true;
            else Moving = false;
            //Set directions based on keys pressed
            if (RightPressed && !LeftPressed)
            {
                Left = false;
                Right = true;
            }
            else if (LeftPressed && !RightPressed)
            {
                Left = true;
                Right = false;
            }
            if (UpPressed && !DownPressed)
            {
                Up = true;
                Down = false;
            }
            else if (DownPressed && !UpPressed)
            {
                Down = true;
                Up = false;
            }
            //If only one direction is pressed reset other direction
            if (!LeftPressed && !RightPressed && (UpPressed || DownPressed))
            {
                Left = false;
                Right = false;
            }
            if (!UpPressed && !DownPressed && (LeftPressed || RightPressed))
            {
                Down = false;
                Up = false;
            }
        }
        protected void NoMovement()
        {
            //If no keys are pressed or just sprint
            if (CurrentKbState.GetPressedKeys().Length == 0 ||
                (CurrentKbState.GetPressedKeys().Length == 1 && CurrentKbState.IsKeyDown(Sprintkey)))
            {
                SpriteSpeed = 0;
                //IF NOT ATTACKING
                if (!Attackmode)
                {
                    if (Left)
                    {
                        if (Up)
                            SwitchAnimation("WalkUpLeft");
                        else if (Down)
                            SwitchAnimation("WalkDownLeft");
                        else
                            SwitchAnimation("StandLeft");
                    }
                    if (Right)
                    {
                        if (Up)
                            SwitchAnimation("WalkUpRight");
                        else if (Down)
                            SwitchAnimation("WalkDownRight");
                        else
                            SwitchAnimation("StandRight");
                    }
                    if (Up)
                        SwitchAnimation("StandUp");
                    if (Down)
                        SwitchAnimation("StandDown");
                }
                Moving = false;
            }
            // This check is a little bit I threw in there to allow the character to sprint.
            if (CurrentKbState.IsKeyDown(Sprintkey))
            {
                SpriteSpeed = 3;
                Interval = 100;
                if (!Attackmode)
                    Animatetimer = 3; //prev 10
            }
            else
            {
                SpriteSpeed = 2;
                Interval = 200;
                if (!Attackmode)
                    Animatetimer = 10; //prev 15
            }
        }
        protected void MovementCollision()
        {
            //UP Collision
            if (Up && UpPressed)
            {
                Feetrectnew = Feetrect;
                Feetrectnew.Adjust(0, -SpriteSpeed);

                Game1.PlayerVSplayercollision1(Feetrectnew, Playerindex);
                if (Game1.Check_Collisions(Feetrectnew, Playerindex))
                {
                    Position.Y -= SpriteSpeed;
                }

                Moving = true;
            }
            //DOWN Collision
            if (Down && DownPressed)
            {
                Feetrectnew = Feetrect;
                Feetrectnew.Adjust(0, SpriteSpeed);

                Game1.PlayerVSplayercollision1(Feetrectnew, Playerindex);
                if (Game1.Check_Collisions(Feetrectnew, Playerindex))
                {
                    Position.Y += SpriteSpeed;
                }
                Moving = true;
            }
            //RIGHT Collision
            if (Right && RightPressed)
            {
                Feetrectnew = Feetrect;
                Feetrectnew.Adjust(SpriteSpeed, 0);

                Game1.PlayerVSplayercollision1(Feetrectnew, Playerindex);
                if (Game1.Check_Collisions(Feetrectnew, Playerindex))
                {
                    Position.X += SpriteSpeed;
                }
                Moving = true;
            }
            //LEFT Collision
            if (Left && LeftPressed)
            {
                Feetrectnew = Feetrect;
                Feetrectnew.Adjust(-SpriteSpeed, 0);
                Game1.PlayerVSplayercollision1(Feetrectnew, Playerindex);
                if (Game1.Check_Collisions(Feetrectnew, Playerindex))
                {
                    Position.X -= SpriteSpeed;
                }
                Moving = true;
            }
        }

        protected void SwapMovingAnimations()
        {
            //Animate if moving
            if (!Moving) return;
            if (Left)
            {
                if (Up)
                    SwitchAnimation("WalkUpLeft");
                else if (Down)
                    SwitchAnimation("WalkDownLeft");
                else
                    SwitchAnimation("WalkLeft");
            }
            if (Right)
            {
                if (Up)
                    SwitchAnimation("WalkUpRight");
                else if (Down)
                    SwitchAnimation("WalkDownRight");
                else
                    SwitchAnimation("WalkRight");
            }
            if (Up && !(Left || Right))
                CurrAnimation = "WalkUp";
            if (Down && !(Left || Right))
                CurrAnimation = "WalkDown";
        }
        public void UpdateHud()
        {
            Hud.Update(Health, Magic, Xp);
        }

        public void Dead()
        {
            Alive = false;
        }

        public void Hurt(int damage)
        {
            Health -= damage;
            Ishurting = true;
            Hurtinterval = 0;
        }

        public void SetHittable()
        {
            if (Ishurting)
                Hurtinterval++;
            if (Hurtinterval < 120) return;
            Hurtinterval = 0;
            Ishurting = false;
        }

        private void SetVelocities()
        {
            if (Up || Down)
            {
                if (Down)
                    Velocityy = 1;
                else
                    Velocityy = -1;
            }
            else
                Velocityy = 0;
            if (Left || Right)
            {
                if (Left)
                    Velocityx = -1;
                else
                    Velocityx = 1;
            }
            else
                Velocityx = 0;
        }

        protected Rectangle PositionRectAdjust(Rectangle rect)
        {
            return new Rectangle(rect.X + (int)Position.X, rect.Y + (int)Position.Y, rect.Width, rect.Height);
        }
        public virtual void Draw(SpriteBatch sb, SpriteFont f, int i, Texture2D boundingbox)
        {
            _newvect = Game1.CameraFix(_newvect);
            if (Ishurting)
                sb.Draw(SpriteTexture, Game1.CameraFix(Position), SourceRectTop.ToRectangle(), Color.Red);
            else
            {
                var currentFrame = UpperAnimations[CurrAnimation].CurrFrame;
                var animations = UpperAnimations[CurrAnimation].Animations;
                var colliders = UpperAnimations[CurrAnimation].Colliders;
                //Draw Player
                sb.Draw(SpriteTexture, Game1.CameraFix(new Vector2(Position.X + Xoffset, Position.Y + Yoffset)),
                    animations[currentFrame], Color.White);
                //Draw AttackRect
                sb.Draw(boundingbox, PositionRectAdjust(colliders[currentFrame].CollisionRectangle),
                        null, Color.White, colliders[currentFrame].Rotation, new Vector2(), SpriteEffects.None, 0f);
                if (Left)
                    sb.DrawString(f, "Left", new Vector2(320, 340), Color.Red);
                if (Right)
                    sb.DrawString(f, "Right", new Vector2(360, 340), Color.Red);
                if (Up)
                    sb.DrawString(f, "Up", new Vector2(340, 320), Color.Red); 
                if (Down)
                    sb.DrawString(f, "Down", new Vector2(340, 360), Color.Red);
                sb.DrawString(f,"Directions",new Vector2(300,300),Color.Red);
                //sb.DrawString(f, "CurrAnimation: " + CurrAnimation + " CurrFrame: " + currentFrame, new Vector2(300, 300),Color.Red);
                //Console.WriteLine(CurrAnimation);
                sb.DrawString(f, "Frames: " + UpperAnimations[CurrAnimation].Frames, new Vector2(300, 280), Color.Red);
                sb.Draw(boundingbox, Feetrect.Min, Color.White);
            }
            if (!_inventoryMenu) return;
            for (var j = 0; j < Inventory.Items.Count; j++)
                for (var k = 0; k < Inventory.Items[j].Count; k++)
                {
                    sb.Draw(Inventory.Items[j][k].Texture,
                        new Rectangle(k*35 + (int) _newvect.X - SpriteWidth - 10,
                            j*35 + (int) _newvect.Y - SpriteHeight - 10, 30, 30), Color.White);
                    sb.DrawString(f, "Q:" + Inventory.Items[j][k].Quantity + ", ", new Vector2(k*35, j*35 + 30),
                        Color.Red);
                }
            //sb.Draw(boundingbox, testbox, Color.Red);
            //sb.Draw(boundingbox, AttackRectangle, Color.White);

            //sb.DrawString(f, "invx: " + invx + "  invy: " + invy, new Vector2(300, 300), Color.White);
        }

        public void GetItem(Item item)
        {
            var check = false;
            foreach (var t1 in from items in Inventory.Items from t1 in items where item.GetType() == t1.GetType() select t1)
            {
                t1.Add();
                check = true;
            }
            if (!(check))
            {
                Inventory.Items[0].Add(item);
            }
        }

        public virtual void Act(TileMap tilemap)
        {
            SetVelocities();
            if (CurrentKbState.IsKeyDown(Keys.Z))
                _inventoryMenu = true;
            if (CurrentKbState.IsKeyDown(Keys.Back))
                _inventoryMenu = false;
            if (CurrentKbState.IsKeyDown(Keys.LeftControl) && PreviousKbState.IsKeyUp(Keys.LeftControl))
            {
                if (Inventory.Items[0].Count > 0)
                {
                    Inventory.Items[0][0].Use(this);
                    if (Inventory.Items[0][0].Quantity <= 0)
                        Inventory.Items[0].RemoveAt(0);
                }
            }
            if (Health > 100)
                Health = 100;
            _newvect = Position;
        }

        public void DrawItems(SpriteBatch sb, Texture2D boundingbox)
        {
            if (!Activated) return;
            if (Game1.Npcs[_index].GetType() == typeof (Npc)) return;
            sb.Draw(boundingbox,
                new Rectangle(0, 0, _sellerInventory.Items[0].Count*35, _sellerInventory.Items.Count*35),
                Color.LightSkyBlue);
            if (CurrentKbState.IsKeyDown(Keys.Tab) && PreviousKbState.IsKeyUp(Keys.Tab))
            {
                if (_sellerInventory.Items[_invy].Count > 0)
                {
                    GetItem(_sellerInventory.Items[_invy][_invx]);
                    _sellerInventory.Items[_invy].RemoveAt(_invx);
                    if (Game1.Npcs[_index].GetType() == typeof (Seller))
                        ((Seller) Game1.Npcs[_index]).Inventory = _sellerInventory;
                    if (Game1.Npcs[_index].GetType() == typeof (Chest))
                        ((Chest) Game1.Npcs[_index]).Inventory = _sellerInventory;
                }
            }
            for (var l = 0; l < _sellerInventory.Items.Count; l++)
                for (var k = 0; k < _sellerInventory.Items[l].Count; k++)
                {
                    sb.Draw(boundingbox, new Rectangle(k*35, l*35, 30, 30), Color.Blue);
                    if (!(_sellerInventory.Items[l][k].GetType() == typeof (Item)))
                        sb.Draw(_sellerInventory.Items[l][k].Texture, new Rectangle(k*35, l*35, 30, 30),
                            Color.White);
                }
            sb.Draw(boundingbox, new Rectangle(_invx*35, _invy*35, 30, 30), Color.White);
        }

        #region Animation
        //AnimationSwitch
        public void SwitchAnimation(string animationname)
        {
            UpperAnimations[CurrAnimation].Played = false;
            CurrAnimation = animationname;
        }
        #endregion
    }
}