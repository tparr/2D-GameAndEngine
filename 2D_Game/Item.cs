using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _2D_Game
{
   public class Inventory
    {
       List<List<Item>> _items;

        public Inventory()
        {
            _items = new List<List<Item>>();
        }
        public Inventory(int capacity)
        {
            _items = new List<List<Item>>(capacity);
            for (int k = 0; k < capacity; k++)
                _items.Insert(0,new List<Item>(capacity));

            for (int i = 0; i < _items.Capacity; i++)
                for (int j = 0; j < _items[i].Capacity; j++)
                    _items[i].Insert(j,(new Item()));
        }
        public Inventory(int width, int height)
        {
            _items = new List<List<Item>>(height);
            for (int k = 0; k < height; k++)
                _items.Insert(0, new List<Item>(width));
            //for (int i = 0; i < items.Capacity; i++)
            //    for (int j = 0; j < items[i].Capacity; j++)
            //        this.items[i].Insert(j, (new Item()));
        }
        public List<List<Item>> Items
        {
            get { return _items; }
            set { _items = value; }
        }
    }
    public class Item : Thing
    {
        public String Name;
        public int Quantity;
        protected RectangleF Hitbox;
        public Texture2D Texture;
        public bool Used;
        protected Random Generator = new Random();

        virtual public int DefaultWidth
        { get { return 0; } }
        virtual public int DefaultHeight
        { get { return 0; } }

        public RectangleF HitBox
        {
            get { return Hitbox; }
            set { Hitbox = value; }
        }

        public Item()
        {
            Name = "item";
            Quantity = 0;
        }
        public virtual void Use(Player player)
        { Quantity--; }

        public void Add()
        { Quantity++; }

        public virtual void Draw(SpriteBatch sb, World world)
        { }
        

    }
    public class HealthPotion : Item
    {
        public override int DefaultWidth
        { get { return 20; } }
        public override int DefaultHeight
        { get { return 25; } }
        public HealthPotion()
        {
            Texture = Game1.Healthpotion;
            Name = "Potion";
            Quantity = 1;
            Hitbox = new RectangleF(0, 0, DefaultWidth, DefaultHeight);
        }
        public HealthPotion(int x, int y)
        {
            Texture = Game1.Healthpotion;
            Name = "Potion";
            Quantity = 1;
            Hitbox = new RectangleF(x, y, DefaultWidth, DefaultHeight);
            Feetrect = Hitbox;
        }

        public override void Use(Player player)
        {
            base.Use(player);
            player.Health += 30;

        }

        public override void Draw(SpriteBatch sb,World world)
        {
            sb.Draw(Texture, world.CameraFix(Hitbox).ToRectangle(), Color.White);
        }
    }
    public class SmallHealthPotion : Item
    {
        public override int DefaultWidth
        { get { return 15; } }
        public override int DefaultHeight
        { get { return 30; } }
        public SmallHealthPotion()
        {
            Texture = Game1.SmallHealthPotion;
            Name = "Potion";
            Quantity = 1;
            Hitbox = new RectangleF(0, 0, DefaultWidth, DefaultHeight);
        }
        public SmallHealthPotion(int x, int y)
        {
            Texture = Game1.SmallHealthPotion;
            Name = "Potion";
            Quantity = 1;
            Hitbox = new RectangleF(x, y, DefaultWidth, DefaultHeight);
            Feetrect = Hitbox;
        }

        public override void Use(Player player)
        {
            base.Use(player);
            player.Health += 10;

        }

        public override void Draw(SpriteBatch sb, World world)
        {
            sb.Draw(Texture, world.CameraFix(Hitbox).ToRectangle(), Color.White);
        }
    }
    public class LargeHealthPotion : Item
    {
        public override int DefaultWidth
        { get { return 25; } }
        public override int DefaultHeight
        { get { return 29; } }
        public LargeHealthPotion()
        {
            Texture = Game1.LargeHealthPotion;
            Name = "Potion";
            Quantity = 1;
            Hitbox = new RectangleF(0, 0, DefaultWidth, DefaultHeight);
        }
        public LargeHealthPotion(int x, int y)
        {
            Texture = Game1.LargeHealthPotion;
            Name = "Potion";
            Quantity = 1;
            Hitbox = new RectangleF(x, y, DefaultWidth, DefaultHeight);
            Feetrect = Hitbox;
        }

        public override void Use(Player player)
        {
            base.Use(player);
            player.Health += 50;

        }

        public override void Draw(SpriteBatch sb, World world)
        {
            sb.Draw(Texture, world.CameraFix(Hitbox).ToRectangle(), Color.White);
        }
    }
    public class ManaPotion : Item
    {
        public override int DefaultWidth
        { get { return 20; } }
        public override int DefaultHeight
        { get { return 25; } }
        public ManaPotion()
        {
            Texture = Game1.Manapotion;
            Name = "Mana Potion";
            Quantity = 1;
        }
        public ManaPotion(int x, int y)
        {
            Texture = Game1.Manapotion;
            Name = "Mana Potion";
            Quantity = 1;
            Hitbox = new RectangleF(x, y, DefaultWidth, DefaultHeight);
            Feetrect = Hitbox;
        }
        public override void Use(Player player)
        {
            base.Use(player);
            player.Magic += 30;
        }

        public override void Draw(SpriteBatch sb, World world)
        {
            sb.Draw(Texture, world.CameraFix(Hitbox).ToRectangle(), Color.White);
        }
    }
    public class Exp : Item
    {
        readonly int _boost;
        public Exp(RectangleF rect, int value)
        {
            Name = "EXP";
            Hitbox = rect;
            Hitbox.Width = value;
            Hitbox.Height = value;
            _boost = value;
        }

        public override void Use(Player player)
        {
            base.Use(player);
            player.Xp += _boost;
        }

        public override void Draw(SpriteBatch sb,World world)
        {
            sb.Draw(Game1.ExpTex, world.CameraFix(Hitbox).ToRectangle(), Color.Yellow);
        }
    }
}