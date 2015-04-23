using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Xna.Framework;

namespace ComeBackGameEditor
{
    public partial class Form1 : Form
    {
        public TileMap Tilemap;
        public TileMap NewTilemap;
        TileAdvanced _newTile;
        public Form1()
        {
            InitializeComponent();
            SetBounds(274, 55, 820, Height);
        }
        public TileMap UpdateMap()
        {
            return Tilemap;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog {RestoreDirectory = true};
            openFileDialog.ShowDialog();
            int width = 0;
            int height = 0;
            int widthcounter = 0;
            int heightcounter = 0;
            int counter = 0;
            List<Vector2> npcs = new List<Vector2>();
            List<Door> doors = new List<Door>();
            List<Vector2> players = new List<Vector2>();
            List<string> list = new List<string>();
            List<string> doorfilenames = new List<string>();
            // Read the file and display it line by line.
            if (openFileDialog.FileName != "")
            {
                NewTilemap = new TileMap();
                var reader = new StreamReader(openFileDialog.OpenFile());
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (counter >= 0 && counter < 3)
                    {
                        list.Add(line != "Nothing" ? line : "Nothing");
                    }
                    else switch (counter)
                    {
                        case 3:
                            width = Convert.ToInt32(line);
                            break;
                        case 4:
                            height = Convert.ToInt32(line);
                            NewTilemap.Width = width;
                            NewTilemap.Height = height;
                            break;
                    }
                    if (counter > 4)
                    {
                        if (heightcounter == 0)
                            NewTilemap.Tilemap.Add(new List<TileAdvanced>());
                        if (widthcounter < width)
                        {
                            if (heightcounter < Tilemap.Tilemap.Count)
                            {
                                var c = line.Substring(0, 1) != "f";
                                NewTilemap.Tilemap[heightcounter].Add(new TileAdvanced(Convert.ToInt32(line.Substring(1)), c));
                                widthcounter++;
                            }
                        }
                        else if (line == "newline")
                        {
                            NewTilemap.Tilemap.Add(new List<TileAdvanced>());
                            widthcounter = 0;
                            heightcounter++;
                        }
                    }
                    counter++;
                }
                reader.Close();
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].Length > 0)
                    {
                        var playerlist = list[i].Split('|').ToList();
                        if (i == 0 || i == 1)
                        {
                            for (int j = 0; j < playerlist.Count / 2; j += 2)
                            {
                                if (i == 0)
                                    players.Add(new Vector2(Convert.ToInt32(playerlist[j]), Convert.ToInt32(playerlist[j + 1])));
                                else if (i == 1)
                                    npcs.Add(new Vector2(Convert.ToInt32(playerlist[j]), Convert.ToInt32(playerlist[j + 1])));
                            }
                        }
                        //if (i == 2)
                        //{
                        //    Players.Remove("");
                        //    for (int j = 0; j < Players.Count; j += 3)
                        //    {
                        //        doors.Add(new Door(new Microsoft.Xna.Framework.Rectangle(Convert.ToInt32(Players[j]), Convert.ToInt32(Players[j + 1]),32,32)));
                                
                        //        doorfilenames.Add(Players[j + 2]);

                        //    }
                        //}
                    }
                }
                Game1.Players = players;
                Game1.Npcs = npcs;
                Game1.Doors = doors;
                Game1.Tilemap.Tilemap.Clear();
                for (int i = 0; i < NewTilemap.Tilemap.Count; i++)
                    Game1.Tilemap.Tilemap.Add(NewTilemap.Tilemap[i]);

                Game1.Tilemapwidth = width;
                Game1.Tilemapheight = height;
                Game1.Doorfilenames = doorfilenames;
            }
            var filename = openFileDialog.FileName;
            filename = filename.Substring(filename.LastIndexOf('\\') + 1);
            toolStripTextBox1.Text = filename;
        }

        public void addColumnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < Game1.Tilemapheight; i++)
                Game1.Tilemap.Tilemap[i].Add(new TileAdvanced());
            Game1.Tilemapwidth += 1;
            NewTilemap = Game1.Tilemap;
        }
        public void addRowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Game1.Tilemap.Tilemap.Add(new List<TileAdvanced>());
            for (int i = 0; i < Game1.Tilemapwidth; i++)
                Game1.Tilemap.Tilemap[Game1.Tilemap.Tilemap.Count - 1].Add(new TileAdvanced());
            Game1.Tilemapheight += 1;
            NewTilemap = Game1.Tilemap;
        }

        private void saveToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.ShowDialog();
            if (saveFileDialog1.FileName != "")
            {
                using (StreamWriter file = new StreamWriter(saveFileDialog1.OpenFile()))
                {
                    //Players
                    for (int i = 0; i < Game1.Players.Count; i++)
                        file.Write(Game1.Players[i].X + "|" + Game1.Players[i].Y + "|");
                    if (Game1.Players.Count == 0)
                        file.Write("Nothing");
                    file.WriteLine();
                    //Npcs
                    //
                    //Check Loading and saving before proceeding
                    //
                    for (int i = 0; i < Game1.Npcs.Count; i++)
                        file.Write(Game1.Npcs[i].X + "|" + Game1.Npcs[i].Y+"|");
                    if (Game1.Npcs.Count == 0)
                        file.Write("Nothing");
                    file.WriteLine();
                    //Doors
                    for (int i = 0; i < Game1.Doors.Count; i++)
                    {
                        file.Write(Game1.Doors[i].Bounds.X + "|" + Game1.Doors[i].Bounds.Y + "|" + Game1.Doorfilenames[i] + "|");
                    }
                    if (Game1.Doors.Count == 0)
                        file.Write("Nothing");
                    file.WriteLine();

                    file.WriteLine(Game1.Tilemapwidth.ToString());
                    file.WriteLine(Game1.Tilemapheight.ToString());
                    for (int i = 0; i < Game1.Tilemapheight; i++)
                    {
                        for (int j = 0; j < Game1.Tilemapwidth; j++)
                        {
                            _newTile = Game1.Tilemap.Tilemap[i][j];
                            var c = _newTile.Collidable ? "t" : "f";
                            file.WriteLine(c + _newTile.SourceX);
                        }
                        file.WriteLine("newline");
                    }
                }
            }
            var filename = saveFileDialog1.FileName;
            filename = filename.Substring(filename.LastIndexOf('\\') + 1);
            toolStripTextBox1.Text = filename;
        }
        public void NewDoor()
        {
            //Width
            toolStripTextBox5.Text = "32";
            //Height
            toolStripTextBox4.Text = "32";
        }
        public void DisplayDoor(int width, int height)
        {
            toolStripTextBox5.Text = Convert.ToString(width);
            toolStripTextBox4.Text = Convert.ToString(height);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            ToolClick(button7.ImageIndex);
        }

        public void ToolClick(int index)
        {
            Game1.Buttonindex = index;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ToolClick(button1.ImageIndex);
        }
        private void button2_Click(object sender, EventArgs e)
        {
            ToolClick(button2.ImageIndex);
        }
        private void button3_Click(object sender, EventArgs e)
        {
            ToolClick(button3.ImageIndex);
        }
        private void button4_Click(object sender, EventArgs e)
        {
            ToolClick(button4.ImageIndex);
        }
        private void button5_Click(object sender, EventArgs e)
        {
            ToolClick(button5.ImageIndex);
        }
        private void button6_Click(object sender, EventArgs e)
        {
            ToolClick(button6.ImageIndex);
        }
    }
}
