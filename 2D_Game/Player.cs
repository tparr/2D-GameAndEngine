using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace _2D_Game
{
    public class Player : Thing
    {
        public bool IsAlive; //If Not alive don't update
        public bool IsAttacking = false; //Currently Attacking
        public bool IsTakingDamage; //Taking Damage
        protected bool IsInteractingwithSomething; //Interacting with a NPC or chest.
        protected bool Left, Right, Up, Down, LeftPressed, RightPressed, DownPressed, UpPressed, attackpressed;
        protected bool IsMoving;
        private bool IsInventoryMenuActive;

        public Collidable AttackCollider;

        protected Dictionary<string, Animation> Animations;

        public float Positionx;
        public float Positiony;

        public HealthBar Hud;

        public int Health = 100;
        public int Magic = 100;
        public int Velocityx;
        public int Velocityy;
        public int Experience;
        protected int Hurtinterval;
        private int InventoryXIndex, InventoryYIndex;

        protected Inventory Inventory = new Inventory(1, 1);
        private Inventory SellerInventory;

        protected KeyboardState CurrentKbState;
        protected KeyboardState PreviousKbState;

        protected Keys Attackkey;
        protected Keys Downkey;
        protected Keys Leftkey;
        protected Keys Rightkey;
        protected Keys Sprintkey;
        protected Keys Upkey;

        public readonly PlayerIndex Playerindex;

        public RectangleF PlayerBoundingBox;
        protected RectangleF Feetrectnew;
        protected RectangleF Touch;

        public string currAnimation;
        public Animation CurrentAnimation
        { 
            get { return Animations[currAnimation]; }
            set { Animations[currAnimation] = value; }
        }
        public string textureName;
        public string Type = "Player";

        public Texture2D SpriteTexture;

        public Vector2 Position;
        private Vector2 _newvect;   

        public Player(PlayerIndex index)
        {
            CharacterSpeed = 2;
            Position = new Vector2(100, 100);
            IsAlive = true;
            Playerindex = index;
            Hud = new HealthBar() { PlayerIndexx = (int)index };
            SellerInventory = new Inventory();
            currAnimation = "StandDown";
        }
        
        public int CharacterSpeed { get; protected set; }

        public void Act()
        {
            UpdateAndInitializeMovementVariables();
            SprintCheck();
            SetVelocities();
            SetMovementDirection();
            
            if (CurrentKbState.IsKeyDown(Keys.Z))
                IsInventoryMenuActive = true;
            if (CurrentKbState.IsKeyDown(Keys.Back))
                IsInventoryMenuActive = false;
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

        public void Dead()
        {
            IsAlive = false;
        }

        public virtual void Draw(SpriteBatch sb, SpriteFont f, Texture2D boundingbox, World world)
        {
            _newvect = world.CameraFix(_newvect);
            if (IsTakingDamage)
                sb.Draw(SpriteTexture, world.CameraFix(Position), CurrentAnimation.AnimationRect, Color.Red);
            else
            {
                var currentFrame = CurrentAnimation.CurrFrame;
                var animations = CurrentAnimation.Animations;
                var colliders = CurrentAnimation.Colliders;
                //Draw Player
                if (IsAttacking)
                {
                    sb.Draw(SpriteTexture, world.CameraFix(new Vector2(Position.X + CurrentAnimation.Xoffset, Position.Y + CurrentAnimation.Yoffset)),
                        animations[currentFrame], Color.Red);
                }
                else
                {
                    sb.Draw(SpriteTexture, world.CameraFix(new Vector2(Position.X + CurrentAnimation.Xoffset, Position.Y + CurrentAnimation.Yoffset)),
                        animations[currentFrame], Color.White);
                }
                //if Attacking Draw Attack Rectangle
                if (IsAttacking)
                {
                    if (colliders[currentFrame] != null)
                    {
                        if (colliders[currentFrame].GetType() == typeof(RotatedRectangle))
                            sb.Draw(boundingbox, PositionRectAdjust(((RotatedRectangle)CurrentAnimation.Collider).CollisionRectangle),
                                null, Color.White, ((RotatedRectangle)colliders[currentFrame]).Rotation, new Vector2(), SpriteEffects.None, 0f);
                        else if (colliders[currentFrame].GetType() == typeof(Circle))
                            sb.Draw(boundingbox, ((Circle)CurrentAnimation.Collider).ToRectangle(), Color.White);
                        else if (colliders[currentFrame].GetType() == typeof(RectangleF))
                            sb.Draw(boundingbox, ((RotatedRectangle)CurrentAnimation.Collider).CollisionRectangle, Color.White);
                        else throw new NotImplementedException("Collider Type not Handled: " + colliders[currentFrame].GetType().ToString());
                    }
                }
                if (Left)
                    sb.DrawString(f, "Left", new Vector2(320, 340), Color.Red);
                if (Right)
                    sb.DrawString(f, "Right", new Vector2(360, 340), Color.Red);
                if (Up)
                    sb.DrawString(f, "Up", new Vector2(340, 320), Color.Red);
                if (Down)
                    sb.DrawString(f, "Down", new Vector2(340, 360), Color.Red);
                sb.DrawString(f, "Directions", new Vector2(300, 300), Color.Red);
                //sb.DrawString(f, "CurrAnimation: " + CurrAnimation + " CurrFrame: " + currentFrame, new Vector2(300, 300),Color.Red);
                //Console.WriteLine(CurrAnimation);
                //sb.DrawString(f, "Frames: " + CurrentAnimation.Frames, new Vector2(300, 280), Color.Red);
                sb.Draw(boundingbox, world.CameraFix(Feetrect.Min), Color.White);
            }
            if (IsInventoryMenuActive)
            {
                for (var j = 0; j < Inventory.Items.Count; j++)
                    for (var k = 0; k < Inventory.Items[j].Count; k++)
                    {
                        sb.Draw(Inventory.Items[j][k].Texture,
                            new Rectangle(k * 35 + (int)_newvect.X - CurrentAnimation.AnimationRect.Width - 10,
                                j * 35 + (int)_newvect.Y - CurrentAnimation.AnimationRect.Height - 10, 30, 30), Color.White);
                        sb.DrawString(f, "Q:" + Inventory.Items[j][k].Quantity + ", ", new Vector2(k * 35, j * 35 + 30),
                            Color.Red);
                    }
            }
            //sb.Draw(boundingbox, testbox, Color.Red);
            //sb.Draw(boundingbox, AttackRectangle, Color.White);

            //sb.DrawString(f, "invx: " + invx + "  invy: " + invy, new Vector2(300, 300), Color.White);
        }

        public void DrawItems(SpriteBatch sb, Texture2D boundingbox)
        {
            if (!IsInteractingwithSomething) return;
            throw new NotImplementedException();
            //if (Game1.Npcs[_index].GetType() == typeof (Npc)) return;
            sb.Draw(boundingbox,
                new Rectangle(0, 0, SellerInventory.Items[0].Count * 35, SellerInventory.Items.Count * 35),
                Color.LightSkyBlue);
            if (CurrentKbState.IsKeyDown(Keys.Tab) && PreviousKbState.IsKeyUp(Keys.Tab))
            {
                if (SellerInventory.Items[InventoryYIndex].Count > 0)
                {
                    GetItem(SellerInventory.Items[InventoryYIndex][InventoryXIndex]);
                    SellerInventory.Items[InventoryYIndex].RemoveAt(InventoryXIndex);
                    throw new NotImplementedException();
                    //if (Game1.Npcs[_index].GetType() == typeof (Seller))
                    //    ((Seller) Game1.Npcs[_index]).Inventory = _sellerInventory;
                    //if (Game1.Npcs[_index].GetType() == typeof (Chest))
                    //    ((Chest) Game1.Npcs[_index]).Inventory = _sellerInventory;
                }
            }
            
            sb.Draw(boundingbox, new Rectangle(InventoryXIndex * 35, InventoryYIndex * 35, 30, 30), Color.White);
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

        public void HandleNpcInventoryInput(World world)
        {
            if (IsAlive)
            {
                if (CurrentKbState.IsKeyDown(Keys.A) && PreviousKbState.IsKeyUp(Keys.A))
                {
                    world.AddEntity(new SmallHealthPotion(300, 300));
                    world.AddEntity(new LargeHealthPotion(300, 320));
                    world.AddEntity(new ManaPotion(300, 340));
                    world.AddEntity(new Exp(new RectangleF(200f, 300f, 5f, 6f), 10));
                }
                //if not attacking and attacking pressed
                //Origin = new Vector2(CurrentAnimation.AnimationRect.Width / 2, CurrentAnimation.AnimationRect.Height / 2);
                if (CurrentKbState.IsKeyDown(Attackkey) && PreviousKbState.IsKeyUp(Attackkey) && !IsAttacking)
                {
                    if (Up)
                    {
                        Touch = new RectangleF(Positionx - 10, Positiony, 14, 10);
                    }
                    else if (Down)
                    {
                        Touch = new RectangleF(Positionx - 8, Positiony + 7, 14, 10);
                    }
                    else if (Left)
                    {
                        Touch = new RectangleF(Positionx - 16, Positiony + 5, 10, 10);
                    }
                    else
                    {
                        Touch = new RectangleF(Positionx - 3, Positiony + 5, 10, 10);
                    }
                    //Exit activation
                    if (IsInteractingwithSomething)
                    {
                        IsInteractingwithSomething = false;
                        throw new NotImplementedException();
                        //world.Npcs[_index].Activated = false;
                    }
                    //Activate
                    else
                    {
                        List<Npc> npcs = world.Npcs;
                        for (var i = 0; i < npcs.Count; i++)
                            if (npcs[i].FeetBox.Intersects(Touch))
                            {
                                IsInteractingwithSomething = true;
                                npcs[i].Activated = true;
                                if (npcs[i].GetType() == typeof(Seller))
                                    SellerInventory = ((Seller)npcs[i]).Inventory;
                                if (npcs[i].GetType() == typeof(Chest))
                                    SellerInventory = ((Chest)npcs[i]).Inventory;
                            }
                    }
                }
                //Activated controls
                if (IsInteractingwithSomething)
                {
                    if (CurrentKbState.IsKeyDown(Leftkey) && PreviousKbState.IsKeyUp(Leftkey))
                        if (InventoryXIndex - 1 >= 0)
                            InventoryXIndex--;
                    if (CurrentKbState.IsKeyDown(Rightkey) && PreviousKbState.IsKeyUp(Rightkey))
                        if (InventoryXIndex + 1 < SellerInventory.Items[InventoryYIndex].Count)
                            InventoryXIndex++;
                    if (CurrentKbState.IsKeyDown(Downkey) && PreviousKbState.IsKeyUp(Downkey))
                        if (InventoryYIndex + 1 < SellerInventory.Items.Count)
                            InventoryYIndex++;
                    if (CurrentKbState.IsKeyDown(Upkey) && PreviousKbState.IsKeyUp(Upkey))
                        if (InventoryYIndex - 1 >= 0)
                            InventoryYIndex--;
                }
            }
            //Reset hit timer
            SetHittable();
        }

        public void Hurt(int damage)
        {
            Health -= damage;
            IsTakingDamage = true;
            Hurtinterval = 0;
        }

        public void SetHittable()
        {
            if (IsTakingDamage)
                Hurtinterval++;
            if (Hurtinterval < 120) return;
            Hurtinterval = 0;
            IsTakingDamage = false;
        }

        public void SprintCheck()
        {
            // This check is a little bit I threw in there to allow the character to sprint.
            if (CurrentKbState.IsKeyDown(Sprintkey))
                CharacterSpeed = 3;
            else 
                CharacterSpeed = 2;
        }

        public void UpdateHud()
        {
            Hud.Update(Health, Magic, Experience);
        }

        protected void MovementCollision(World world)
        {
            if (!IsAttacking)
            {

                //UP Collision
                if (Up && UpPressed)
                {
                    Feetrectnew = Feetrect;
                    Feetrectnew.Adjust(0, -CharacterSpeed);

                    if (world.isColliding(Feetrectnew, (int)Playerindex) == false)
                    {
                        Position.Y -= CharacterSpeed;
                    }
                }
                //DOWN Collision
                if (Down && DownPressed)
                {
                    Feetrectnew = Feetrect;
                    Feetrectnew.Adjust(0, CharacterSpeed);

                    if (world.isColliding(Feetrectnew, (int)Playerindex) == false)
                    {
                        Position.Y += CharacterSpeed;
                    }
                }
                //RIGHT Collision
                if (Right && RightPressed)
                {
                    Feetrectnew = Feetrect;
                    Feetrectnew.Adjust(CharacterSpeed, 0);

                    if (world.isColliding(Feetrectnew, (int)Playerindex) == false)
                    {
                        Position.X += CharacterSpeed;
                    }
                }
                //LEFT Collision
                if (Left && LeftPressed)
                {
                    Feetrectnew = Feetrect;
                    Feetrectnew.Adjust(-CharacterSpeed, 0);
                    if (world.isColliding(Feetrectnew, (int)Playerindex) == false)
                    {
                        Position.X -= CharacterSpeed;
                    }
                }
            }
        }

        protected void NoMovement()
        {
            //If no keys are pressed or just sprint
            if (CurrentKbState.GetPressedKeys().Length == 0 ||
                (CurrentKbState.GetPressedKeys().Length == 1 && CurrentKbState.IsKeyDown(Sprintkey)))
            {
                //IF NOT ATTACKING
                if (!IsAttacking)
                {
                    //SpriteSpeed = 0;
                    if (Left)
                    {
                        if (Up)
                            SwitchAnimation("StandUpLeft");
                        else if (Down)
                            SwitchAnimation("StandDownLeft");
                        else
                            SwitchAnimation("StandLeft");
                    }
                    else if (Right)
                    {
                        if (Up)
                            SwitchAnimation("StandRight");
                        else if (Down)
                            SwitchAnimation("StandDownRight");
                        else
                            SwitchAnimation("StandRight");
                    }
                    else if (Up)
                        SwitchAnimation("StandUp");
                    else if (Down)
                        SwitchAnimation("StandDown");
                }
                IsMoving = false;
            }
        }

        protected Rectangle PositionRectAdjust(Rectangle rect)
        {
            return new Rectangle(rect.X + (int)Position.X, rect.Y + (int)Position.Y, rect.Width, rect.Height);
        }

        protected void SetMovementDirection()
        {
            //Check if movement keys are pressed
            LeftPressed = CurrentKbState.IsKeyDown(Leftkey);
            RightPressed = CurrentKbState.IsKeyDown(Rightkey);
            UpPressed = CurrentKbState.IsKeyDown(Upkey);
            DownPressed = CurrentKbState.IsKeyDown(Downkey);
            attackpressed = CurrentKbState.IsKeyDown(Attackkey);
            if (LeftPressed || RightPressed || UpPressed || DownPressed)
                IsMoving = true;
            else IsMoving = false;
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

        protected void UpdateAndInitializeMovementVariables()
        {
            PreviousKbState = CurrentKbState;
            CurrentKbState = Keyboard.GetState();
            Positionx = Position.X;
            Positiony = Position.Y;
            PlayerBoundingBox = new RectangleF(Positionx, Positiony, CurrentAnimation.AnimationRect.Width, CurrentAnimation.AnimationRect.Height);
            Feetrect = new RectangleF(Positionx + 8, Positiony + 26, 10, 5);
            Feetrectnew = Feetrect;
        }

        protected void SwapMovingAnimations()
        {
            //Animate if moving
            if (!IsMoving || IsAttacking) return;

            if (Left)
            {
                if (Up)
                    SwitchAnimation("WalkUpLeft");
                else if (Down)
                    SwitchAnimation("WalkDownLeft");
                else
                    SwitchAnimation("WalkLeft");
            }
            else if (Right)
            {
                if (Up)
                    SwitchAnimation("WalkUpRight");
                else if (Down)
                    SwitchAnimation("WalkDownRight");
                else
                    SwitchAnimation("WalkRight");
            }
            else if (Up)
                currAnimation = "WalkUp";
            else if (Down)
                currAnimation = "WalkDown";
        }

        protected void UpdateAnimations()
        {
            if (CurrentAnimation.Played && IsAttacking)
            {
                if (Up)
                    SwitchAnimation("StandUp");
                if (Left)
                    SwitchAnimation("StandLeft");
                if (Right)
                    SwitchAnimation("StandRight");
                if (Down)
                    SwitchAnimation("StandDown");
                IsAttacking = false;
            }
            CurrentAnimation.Update();
        }

        private void SetVelocities()
        {
            //Set Vertical Velocity
            if (Up)
                Velocityy = -1;
            else if (Down)
                Velocityy = 1;
            else
                Velocityy = 0;

            //Set Horizontal Velocity
            if (Right)
                Velocityx = 1;
            else if (Left)
                Velocityx = -1;
            else
                Velocityx = 0;
        }

        public void SwitchAnimation(string animationname)
        {
            CurrentAnimation.Played = false;
            currAnimation = animationname;
        }
    }
}