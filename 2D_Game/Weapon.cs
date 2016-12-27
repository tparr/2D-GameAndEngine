using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.IO;

namespace _2D_Game
{
    public class Weapon
    {
        //public List<Animation> Animations;
        public Weapon(ContentManager loader)
        {
            //Animations = new List<Animation>(4)
            //{
            //    new Animation(loader, "attackrectbottom", 0, 0, 12, 28, 80,
            //        LoadRotatedText("C:/Users/timmy_000/Desktop/AttackRectBottom.txt")),
            //    new Animation(loader, "attackrectright", 0, 0, 12, 50, 32,
            //        LoadRotatedText("C:/Users/timmy_000/Desktop/AttackRectBottom.txt")),
            //    new Animation(loader, "attackrectleft", -25, 0, 12, 50, 32,
            //        LoadRotatedText("C:/Users/timmy_000/Desktop/AttackRectBottom.txt")),
            //    new Animation(loader, "attackrectright", 0, 0, 12, 50, 32,
            //        LoadRotatedText("C:/Users/timmy_000/Desktop/AttackRectBottom.txt"))
            //};
        }

        public List<RotatedRectangle> LoadRotatedText(string filename)
        {
            string line;
            List<RotatedRectangle> rects = new List<RotatedRectangle>();
            StreamReader reader = new StreamReader(filename);
            while ((line = reader.ReadLine()) != null)
            {
                var parts = line.Split('X');
                rects.Add(new RotatedRectangle(new Rectangle(
                    Convert.ToInt32(parts[0]) - 22,//Only comment out when creating rectangle numbers
                    Convert.ToInt32(parts[1]) - 28,//Same
                    Convert.ToInt32(parts[2]),
                    Convert.ToInt32(parts[3])),
                    (float)Convert.ToDouble(parts[4])));
                //for (int i = 0; i < parts.Length; i++)
                //    Console.WriteLine(parts[i]);
            }
            return rects;
        }
    }
}