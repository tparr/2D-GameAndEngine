using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Collections;
namespace _2D_Game
{
    public class EnemyNew : Thing
    {
        public Vector2 Position;
        protected Dictionary<string, Animation> animations;
        public int Health;
        protected string CurrAnimation;

        public int X
        {
            get { return (int)Position.X; }
        }
        public int Y
        {
            get { return (int)Position.Y; }
        }
        public EnemyNew(int XPos, int YPos, string Class)
        {
            Position.X = XPos;
            Position.Y = YPos;
            animations = World.LoadAnimations(Class).Item2;
        }

        public void Act(World World)
        {

        }

        public int GetNearestPlayer(World world)
        {
            if (world.Players.Count == 1) return 0;
            int closestPlayer = 0;
            List<Player> players = world.Players;
            for (int i = 0; i < players.Count - 1; i++)
            {
                if ((Math.Abs(players[i].Position.X - X) + Math.Abs(players[i].Position.Y)) > (Math.Abs(players[i].Position.X - X) + Math.Abs(players[i].Position.Y - Y)))
                    closestPlayer = i + 1;
            }
            return closestPlayer;
        }
    }
}