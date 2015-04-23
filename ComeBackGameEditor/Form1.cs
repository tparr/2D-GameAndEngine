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
            SetBounds(274, 25, 820, Height);
        }
        public TileMap UpdateMap()
        {
            return Tilemap;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog { RestoreDirectory = true };
            openFileDialog.ShowDialog();
            List<Vector2> npcs = new List<Vector2>();
            List<Door> doors = new List<Door>();
            List<Vector2> players = new List<Vector2>();
            // Read the file and display it line by line.
            if (openFileDialog.FileName != "")
            {
                NewTilemap = new TileMap();
                string[] lines = File.ReadAllLines(openFileDialog.FileName);
                //Add Players
                if (lines[0] != "Nothing")
                {
                    var playerpositions = lines[0].Split('|').ToList();
                    for (int j = 0; j < playerpositions.Count / 2; j += 2)
                        players.Add(new Vector2(Convert.ToInt32(playerpositions[j]),
                            Convert.ToInt32(playerpositions[j + 1])));
                }
                //Add Npcs
                if (lines[1] != "Nothing")
                {
                    var npcpositions = lines[1].Split('|').ToList();
                    for (int j = 0; j < npcpositions.Count / 2; j += 2)
                        npcs.Add(new Vector2(Convert.ToInt32(npcpositions[j]), Convert.ToInt32(npcpositions[j + 1])));
                }
                //Add Doors
                if (lines[2] != "Nothing")
                {
                    var doorpositions = lines[2].Split('|').ToList();
                    for (int j = 0; j < doorpositions.Count / 5; j += 5)
                        doors.Add(
                            new Door(
                                new Rectangle(Convert.ToInt32(doorpositions[j]), Convert.ToInt32(doorpositions[j + 1]),
                                    Convert.ToInt32(doorpositions[j + 2]), Convert.ToInt32(doorpositions[j + 3])),
                                doorpositions[j + 4]));
                }
                //Add Width and Height
                var dimensions = lines[3].Split('|');
                NewTilemap.Width = Convert.ToInt32(dimensions[0]);
                NewTilemap.Height = Convert.ToInt32(dimensions[1]);
                //Create New Tilemap
                for (int j = 4; j < lines.Length; j++)
                {
                    NewTilemap.Tilemap.Add(new List<TileAdvanced>());
                    var tiles = lines[j].Split(new[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
                    for (int k = 0; k < tiles.Length / 2; k++)
                    {
                        NewTilemap.Tilemap[j - 4].Add(new TileAdvanced(
                            Convert.ToInt32(tiles[(k * 2) + 1]), tiles[(k * 2)] != "f"));
                    }
                }
                //Set Global Variables of Game1 Editor
                Game1.Players = players;
                Game1.Npcs = npcs;
                Game1.Doors = doors;
                Game1.Tilemap.Tilemap.Clear();
                for (int i = 0; i < NewTilemap.Tilemap.Count; i++)
                    Game1.Tilemap.Tilemap.Add(NewTilemap.Tilemap[i]);

                Game1.Tilemapwidth = NewTilemap.Width;
                Game1.Tilemapheight = NewTilemap.Height;
                Game1.Tilemap.Width = NewTilemap.Width;
                Game1.Tilemap.Height = NewTilemap.Height;
                NewTilemap.Tilemap.Clear();
            }
            //Get Level Name
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
                    for (int i = 0; i < Game1.Npcs.Count; i++)
                        file.Write(Game1.Npcs[i].X + "|" + Game1.Npcs[i].Y + "|");
                    if (Game1.Npcs.Count == 0)
                        file.Write("Nothing");
                    file.WriteLine();
                    //Doors
                    for (int i = 0; i < Game1.Doors.Count; i++)
                    {
                        file.Write(Game1.Doors[i].Bounds.X
                            + "|" + Game1.Doors[i].Bounds.Y
                            + "|" + Game1.Doors[i].Bounds.Width
                            + "|" + Game1.Doors[i].Bounds.Height
                            + "|" + Game1.Doorfilenames[i] + "|");
                    }
                    if (Game1.Doors.Count == 0)
                        file.Write("Nothing");
                    file.WriteLine();
                    //Width and Height
                    file.WriteLine(Game1.Tilemapwidth + "|" + Game1.Tilemapheight);
                    for (int i = 0; i < Game1.Tilemapheight; i++)
                    {
                        for (int j = 0; j < Game1.Tilemapwidth; j++)
                        {
                            _newTile = Game1.Tilemap.Tilemap[i][j];
                            var c = _newTile.Collidable ? "t" : "f";
                            file.Write(c + "|" + _newTile.SourceX + "|");
                        }
                        file.WriteLine();
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