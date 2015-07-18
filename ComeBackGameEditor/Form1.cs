using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using Microsoft.Xna.Framework;

namespace ComeBackGameEditor
{
    public partial class Form1 : Form
    {
        public TileMap Tilemap;
        public TileMap NewTilemap;
        public TileMap BaseTileMap;
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
            OpenFileDialog openFileDialog = new OpenFileDialog {RestoreDirectory = true, DefaultExt = "lvl"};
            openFileDialog.ShowDialog();
            if (openFileDialog.FileName != "")
            {
                XDocument document = XDocument.Load(openFileDialog.FileName);
                Console.WriteLine();
                IEnumerable<XElement> playerpositions = document.Element("LevelData").Elements("Player");
                Game1.Players.Clear();
                foreach (var player in playerpositions)
                {
                    Game1.Players.Add(new Vector2(float.Parse(player.Attribute("X").Value), float.Parse(player.Attribute("Y").Value)));
                }
                IEnumerable<XElement> npcpositions = document.Element("LevelData").Elements("NPC");
                Game1.Npcs.Clear();
                foreach (var npc in npcpositions)
                {
                    Game1.Npcs.Add(new Vector2(float.Parse(npc.Attribute("X").Value), float.Parse(npc.Attribute("Y").Value)));
                }
                IEnumerable<XElement> doors = document.Element("LevelData").Elements("Door");
                Game1.Doors.Clear();
                foreach (var door in doors)
                {
                    Game1.Doors.Add(new Door(new Rectangle(int.Parse(door.Attribute("X").Value), int.Parse(door.Attribute("Y").Value), int.Parse(door.Attribute("Width").Value), int.Parse(door.Attribute("Height").Value))));
                }
                //throw new NotImplementedException("Finish Loading Level Code");
                IEnumerable<XElement> tiles = document.Element("LevelData").Elements("Tile");
                List<TileAdvanced> newtiles = new List<TileAdvanced>();
                foreach (var tile in tiles)
                {
                    int sourceX;
                    bool collidable;
                    if (int.TryParse(tile.Attribute("SourceX").Value, out sourceX) &&
                        bool.TryParse(tile.Attribute("Collidable").Value, out collidable))
                    {
                        newtiles.Add(new TileAdvanced(sourceX,collidable));
                    }
                }
                int tilemapwidth, tilemapheight;
                int.TryParse(document.Element("LevelData").Attribute("Width").Value,out tilemapwidth);
                int.TryParse(document.Element("LevelData").Attribute("Height").Value, out tilemapheight);
                NewTilemap = new TileMap(tilemapwidth, tilemapheight);
                int count = 0;
                for (int i = 0; i < tilemapheight; i++)
                {
                    for (int j = 0; j < tilemapwidth; j++)
                    {
                        NewTilemap.Tilemap[i][j] = newtiles[count];
                        count++;
                    }
                }
                //Set Global Variables of Game1 Editor
                Game1.Tilemap.Tilemap.Clear();
                for (int i = 0; i < NewTilemap.Tilemap.Count; i++)
                    Game1.Tilemap.Tilemap.Add(NewTilemap.Tilemap[i]);

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

            for (int i = 0; i < (Game1.SelectedMap == Game1.MapState.BaseMap ? Game1.BaseTileMap.Height : Game1.Tilemap.Height); i++)
                (Game1.SelectedMap == Game1.MapState.BaseMap ? Game1.BaseTileMap : Game1.Tilemap).Tilemap[i].Add(new TileAdvanced());
            Game1.AddColumnTilemap(Game1.SelectedMap == Game1.MapState.BaseMap ? Game1.BaseTileMap : Game1.Tilemap);
            NewTilemap = Game1.Tilemap;
            BaseTileMap = Game1.BaseTileMap;

        }
        public void addRowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            (Game1.SelectedMap == Game1.MapState.BaseMap ? Game1.BaseTileMap : Game1.Tilemap).Tilemap.Add(new List<TileAdvanced>());
            for (int i = 0; i < (Game1.SelectedMap == Game1.MapState.BaseMap ? Game1.BaseTileMap.Width : Game1.Tilemap.Width); i++)
                (Game1.SelectedMap == Game1.MapState.BaseMap ? Game1.BaseTileMap : Game1.Tilemap).Tilemap[(Game1.SelectedMap == Game1.MapState.BaseMap ? Game1.BaseTileMap : Game1.Tilemap).Tilemap.Count - 1].Add(new TileAdvanced());
            Game1.AddRowTilemap(Game1.SelectedMap == Game1.MapState.BaseMap ? Game1.BaseTileMap : Game1.Tilemap);
            NewTilemap = Game1.Tilemap;
            BaseTileMap = Game1.BaseTileMap;
        }
        private void saveToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            saveFileDialog1 = new SaveFileDialog {DefaultExt = "lvl"};
            saveFileDialog1.ShowDialog();
            if (saveFileDialog1.FileName != "")
            {
                XDocument document = new XDocument(new XElement("LevelData",
                    new XAttribute("Width", Game1.Tilemap.Tilemap.Count),
                    new XAttribute("Height", Game1.Tilemap.Tilemap[0].Count),
                    Game1.Players.Select(x => new XElement("Player",
                        new XAttribute("X", x.X),
                        new XAttribute("Y", x.Y))),
                    Game1.Npcs.Select(x => new XElement("NPC",
                        new XAttribute("X", x.X),
                        new XAttribute("Y", x.Y))),
                    Game1.Doors.Select(x => new XElement("Door",
                        new XAttribute("X", x.Bounds.X),
                        new XAttribute("Y", x.Bounds.Y),
                        new XAttribute("Width", x.Bounds.Width),
                        new XAttribute("Height", x.Bounds.Height),
                        new XAttribute("LevelName", x.LoadLevelName))),
                    Game1.Tilemap.Tilemap.SelectMany(x => x).ToList().Select(t => new XElement("Tile",
                        new XAttribute("SourceX",t.SourceX),
                        new XAttribute("Collidable",t.Collidable)))));
                Console.WriteLine(document.ToString());
                document.Save(saveFileDialog1.FileName);
                //using (StreamWriter file = new StreamWriter(saveFileDialog1.OpenFile()))
                //{
                //    //Players
                //    for (int i = 0; i < Game1.Players.Count; i++)
                //        file.Write(Game1.Players[i].X + "|" + Game1.Players[i].Y + "|");
                //    if (Game1.Players.Count == 0)
                //        file.Write("Nothing");
                //    file.WriteLine();
                //    //Npcs
                //    for (int i = 0; i < Game1.Npcs.Count; i++)
                //        file.Write(Game1.Npcs[i].X + "|" + Game1.Npcs[i].Y + "|");
                //    if (Game1.Npcs.Count == 0)
                //        file.Write("Nothing");
                //    file.WriteLine();
                //    //Doors
                //    for (int i = 0; i < Game1.Doors.Count; i++)
                //    {
                //        file.Write(Game1.Doors[i].Bounds.X
                //            + "|" + Game1.Doors[i].Bounds.Y
                //            + "|" + Game1.Doors[i].Bounds.Width
                //            + "|" + Game1.Doors[i].Bounds.Height
                //            + "|" + Game1.Doorfilenames[i] + "|");
                //    }
                //    if (Game1.Doors.Count == 0)
                //        file.Write("Nothing");
                //    file.WriteLine();
                //    //Width and Height
                //    file.WriteLine(Game1.Tilemap.Width + "|" + Game1.Tilemap.Height + "|" + Game1.BaseTileMap.Width + "|" + Game1.BaseTileMap.Height);
                //    for (int i = 0; i < Game1.Tilemap.Height; i++)
                //    {
                //        for (int j = 0; j < Game1.Tilemap.Height; j++)
                //        {
                //            _newTile = Game1.Tilemap.Tilemap[i][j];
                //            var c = _newTile.Collidable ? "t" : "f";
                //            file.Write(c + "|" + _newTile.SourceX + "|");
                //        }
                //        file.WriteLine();
                //    }
                //    for (int i = 0; i < Game1.BaseTileMap.Height; i++)
                //    {
                //        for (int j = 0; j < Game1.BaseTileMap.Width; j++)
                //        {
                //            _newTile = Game1.BaseTileMap.Tilemap[i][j];
                //            var c = _newTile.Collidable ? "t" : "f";
                //            file.Write(c + "|" + _newTile.SourceX + "|");
                //        }
                //        file.WriteLine();
                //    }
                //}
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