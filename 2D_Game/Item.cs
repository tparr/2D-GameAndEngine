using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace _2D_Game
{
    public class Exp : Item
    {
        private readonly int _boost;

        public Exp(RectangleF rect, int value)
        {
            Name = "EXP";
            Hitbox = rect;
            Hitbox.Width = value;
            Hitbox.Height = value;
            _boost = value;
        }

        public override void Draw(SpriteBatch sb, World world)
        {
            sb.Draw(Texture(), world.CameraFix(Hitbox).ToRectangle(), Color.Yellow);
        }

        public override Texture2D Texture()
        {
            return Game1.ExpTexture;
        }

        public override void Use(Player player)
        {
            base.Use(player);
            player.Experience += _boost;
        }
    }

    public class HealthPotion : Item
    {
        public HealthPotion()
        {
            Name = "Potion";
            Quantity = 1;
            Hitbox = new RectangleF(0, 0, DefaultWidth, DefaultHeight);
        }

        public HealthPotion(int x, int y)
        {
            Name = "Potion";
            Quantity = 1;
            Hitbox = new RectangleF(x, y, DefaultWidth, DefaultHeight);
            Feetrect = Hitbox;
        }

        public override int DefaultHeight
        { get { return 25; } }

        public override int DefaultWidth
        { get { return 20; } }

        public override void Draw(SpriteBatch sb, World world)
        {
            sb.Draw(Texture(), world.CameraFix(Hitbox).ToRectangle(), Color.White);
        }

        public override Texture2D Texture()
        {
            return Game1.Healthpotion;
        }

        public override void Use(Player player)
        {
            base.Use(player);
            player.Health += 30;
        }
    }

    public class Inventory
    {
        private List<List<Item>> _items;

        public Inventory()
        {
            _items = new List<List<Item>>();
        }

        public Inventory(int capacity)
        {
            _items = new List<List<Item>>(capacity);
            for (int k = 0; k < capacity; k++)
                _items.Insert(0, new List<Item>(capacity));
        }

        public Inventory(int width, int height)
        {
            _items = new List<List<Item>>(height);
            for (int k = 0; k < height; k++)
                _items.Insert(0, new List<Item>(width));
        }

        public List<List<Item>> Items
        {
            get { return _items; }
            set { _items = value; }
        }

        public void Draw(SpriteBatch sb)
        {
            for (var l = 0; l < this.Items.Count; l++)
                for (var k = 0; k < this.Items[l].Count; k++)
                {
                    sb.Draw(Game1._boundingbox, new Rectangle(k * 35, l * 35, 30, 30), Color.Blue);
                    if (!(this.Items[l][k].GetType() == typeof(Item)))
                        sb.Draw(this.Items[l][k].Texture(), new Rectangle(k * 35, l * 35, 30, 30), Color.White);
                }
        }
    }

    public abstract class Item : Thing
    {
        public String Name;
        public int Quantity;
        public bool Used;
        protected Random Generator = new Random();
        protected RectangleF Hitbox;

        public Item()
        {
            Name = "item";
            Quantity = 0;
        }

        virtual public int DefaultHeight
        { get { return 0; } }

        virtual public int DefaultWidth
        { get { return 0; } }

        public RectangleF HitBox
        {
            get { return Hitbox; }
            set { Hitbox = value; }
        }

        public void Add()
        { Quantity++; }

        public virtual void Draw(SpriteBatch sb, World world)
        { }

        public virtual Texture2D Texture()
        {
            return null;
        }

        public virtual void Use(Player player)
        { Quantity--; }
    }

    public class LargeHealthPotion : Item
    {
        public LargeHealthPotion()
        {
            Name = "Potion";
            Quantity = 1;
            Hitbox = new RectangleF(0, 0, DefaultWidth, DefaultHeight);
        }

        public LargeHealthPotion(int x, int y)
        {
            Name = "Potion";
            Quantity = 1;
            Hitbox = new RectangleF(x, y, DefaultWidth, DefaultHeight);
            Feetrect = Hitbox;
        }

        public override int DefaultHeight
        { get { return 29; } }

        public override int DefaultWidth
        { get { return 25; } }

        public override void Draw(SpriteBatch sb, World world)
        {
            sb.Draw(Texture(), world.CameraFix(Hitbox).ToRectangle(), Color.White);
        }

        public override Texture2D Texture()
        {
            return Game1.LargeHealthPotion;
        }

        public override void Use(Player player)
        {
            base.Use(player);
            player.Health += 50;
        }
    }

    public class ManaPotion : Item
    {
        public ManaPotion()
        {
            Name = "Mana Potion";
            Quantity = 1;
        }

        public ManaPotion(int x, int y)
        {
            Name = "Mana Potion";
            Quantity = 1;
            Hitbox = new RectangleF(x, y, DefaultWidth, DefaultHeight);
            Feetrect = Hitbox;
        }

        public override int DefaultHeight
        { get { return 25; } }

        public override int DefaultWidth
        { get { return 20; } }

        public override void Draw(SpriteBatch sb, World world)
        {
            sb.Draw(Texture(), world.CameraFix(Hitbox).ToRectangle(), Color.White);
        }

        public override Texture2D Texture()
        {
            return Game1.Manapotion;
        }

        public override void Use(Player player)
        {
            base.Use(player);
            player.Magic += 30;
        }
    }

    public class SmallHealthPotion : Item
    {
        public SmallHealthPotion()
        {
            Name = "Potion";
            Quantity = 1;
            Hitbox = new RectangleF(0, 0, DefaultWidth, DefaultHeight);
        }

        public SmallHealthPotion(int x, int y)
        {
            Name = "Potion";
            Quantity = 1;
            Hitbox = new RectangleF(x, y, DefaultWidth, DefaultHeight);
            Feetrect = Hitbox;
        }

        public override int DefaultHeight
        { get { return 30; } }

        public override int DefaultWidth
        { get { return 15; } }

        public override void Draw(SpriteBatch sb, World world)
        {
            sb.Draw(Texture(), world.CameraFix(Hitbox).ToRectangle(), Color.White);
        }

        public override Texture2D Texture()
        {
            return Game1.SmallHealthPotion;
        }

        public override void Use(Player player)
        {
            base.Use(player);
            player.Health += 10;
        }
    }
}