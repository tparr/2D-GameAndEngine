using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using _2D_Game;
using Cyotek.Windows.Forms;
using Cyotek.Windows.Forms.Demo;

namespace AnimationEditorForms
{
    struct AnimationDictionary
    {
        public Dictionary<string, Animation> Animations;
        public string imgName;
        public string filename;
    }
    public partial class Form1 : Form
    {
        string filename = "";
        string selectedClass;
        Dictionary<string, AnimationDictionary> Allanimations;
        AnimationDictionary animations;
        List<Collidable> colliders;
        Bitmap spriteSheet;
        string file;
        List<string> animationChoices;
        public Form1()
        {
            InitializeComponent();
        }

        private void OpenAnimationButtonClicked(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog1.ShowDialog(); //Show the dialog.
            if (result == DialogResult.OK) //Test result.
            {
                file = openFileDialog1.FileName;
                StreamReader reader = new StreamReader(file);
                XDocument doc = XDocument.Load(reader);
                animationChoices = doc.Element("AnimationData").Elements("Animations").Select(x => x.Attribute("class").Value).ToList();

                //Create Option Selector to choose class from Xml
                OptionSelector classSelector = new OptionSelector(animationChoices, "Edit this Class");
                if (classSelector.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                    return;
                classSelector.Close();

                //Load Animations from Xml and create List.
                this.selectedClass = classSelector.selectedOption;
                this.selectedClassLabel.Text = selectedClass + " Animations";
                this.Allanimations = loadAllAnimationsfromXml(doc);
                this.animations = this.Allanimations[selectedClass];
                //Create and show Scrollable Image form.
                if (this.animations.filename == "" || this.animations.filename == null)
                    throw new IOException("filepath to image file is missing");

                this.Text = String.Format("Animation Editor (Editing: {0})",selectedClass);
                spriteSheet = new Bitmap(this.animations.filename);
                SaveButton.Enabled = true;
                ChangeClassButton.Enabled = true;
                ChangeClassButton.Visible = true;


                this.frameSelectionPanel.Controls.Clear();
                this.timingPanel.Controls.Clear();
                this.animationPanel.Controls.Clear();
                //Create OptionSelector from Class to choose from.
                OptionSelector animationSelector = new OptionSelector(animations.Animations.Select(x => x.Key).ToList(), "Edit this Animation", false);
                animationSelector.onOptionChosen += animationSelector_onOptionChosen;
                animationSelector.MinimumSize = new Size(animationPanel.Size.Width, 150);
                animationSelector.MaximumSize = new Size(animationPanel.Size.Width, 450);
                animationSelector.Size = new Size(animationPanel.Size.Width, animationPanel.Size.Height);
                animationSelector.Dock = DockStyle.Fill;
                animationSelector.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom;
                animationPanel.Controls.Add(animationSelector);
                
                animationSelector.Show();
            }
        }

        private Dictionary<string, AnimationDictionary> loadAllAnimationsfromXml(XDocument doc)
        {
            var classAnimations = doc.Element("AnimationData").Elements("Animations");
            var Allanimations = new Dictionary<string, AnimationDictionary>();
            foreach (var animationClass in classAnimations)
            {
                Dictionary<string, Animation> animations = new Dictionary<string, Animation>();
                foreach (var node in animationClass.Elements("Animation"))
                {
                    int frames = (int)node.Element("Frames");
                    int XMovement = (int)node.Element("XMovement");
                    int YMovement = (int)node.Element("YMovement");
                    int XOffset = (int)node.Element("XOffset");
                    int YOffset = (int)node.Element("YOffset");
                    string name = (string)node.FirstAttribute;

                    //Load animation Rectangles
                    List<Microsoft.Xna.Framework.Rectangle> animRects = new List<Microsoft.Xna.Framework.Rectangle>();
                    List<int> timers = new List<int>();
                    foreach (var rect in node.Elements("AnimRects").Elements())
                    {
                        animRects.Add(new Microsoft.Xna.Framework.Rectangle((int)rect.Element("X"), (int)rect.Element("Y"), (int)rect.Element("Width"), (int)rect.Element("Height")));
                        timers.Add((int)rect.Attribute("timeLength"));
                    }

                    //Load Colliders
                    List<Collidable> colliders = new List<Collidable>();
                    foreach (var collider in node.Elements("Colliders").Elements())
                    {
                        if (collider.Name == "Rect")
                            colliders.Add(new _2D_Game.RectangleF((float)collider.Element("X"), (float)collider.Element("Y"), (float)collider.Element("Width"), (float)collider.Element("Height")));
                        else if (collider.Name == "Circle")
                            colliders.Add(new Circle(new Microsoft.Xna.Framework.Vector2((float)collider.Element("CenterX"), (float)collider.Element("CenterY")), (float)collider.Element("Radius")));
                    }
                    animations.Add(name, new Animation(name, animRects, new Microsoft.Xna.Framework.Vector2(XMovement, YMovement), colliders, timers.ToArray(), XOffset, YOffset));
                }
                AnimationDictionary dictionary = new AnimationDictionary()
                {
                    Animations = animations,
                    imgName = animationClass.Attribute("img").Value,
                    filename = animationClass.Attribute("filename").Value
                };
                Allanimations.Add(animationClass.Attribute("class").Value, dictionary);
            }
            return Allanimations;
        }

        private Dictionary<string, Animation> loadAnimationfromXml(XDocument doc, string selectedClass)
        {
            var classAnimations = doc.Element("AnimationData").Elements("Animations").Where(x => x.Attribute("class").Value == selectedClass).FirstOrDefault();
            var animations = new Dictionary<string, Animation>();
            filename = classAnimations.Attribute("filename").Value;
            foreach (var node in classAnimations.Elements("Animation"))
            {
                int frames = (int)node.Element("Frames");
                int XMovement = (int)node.Element("XMovement");
                int YMovement = (int)node.Element("YMovement");
                int XOffset = (int)node.Element("XOffset");
                int YOffset = (int)node.Element("YOffset");
                string name = (string)node.FirstAttribute;

                //Load animation Rectangles
                List<Microsoft.Xna.Framework.Rectangle> animRects = new List<Microsoft.Xna.Framework.Rectangle>();
                List<int> timers = new List<int>();
                foreach (var rect in node.Elements("AnimRects").Elements())
                {
                    animRects.Add(new Microsoft.Xna.Framework.Rectangle((int)rect.Element("X"), (int)rect.Element("Y"), (int)rect.Element("Width"), (int)rect.Element("Height")));
                    timers.Add((int)rect.Attribute("timeLength"));
                }

                //Load Colliders
                List<Collidable> colliders = new List<Collidable>();
                foreach (var collider in node.Elements("Colliders").Elements())
                {
                    if (collider.Name == "Rect")
                        colliders.Add(new _2D_Game.RectangleF((float)collider.Element("X"), (float)collider.Element("Y"), (float)collider.Element("Width"), (float)collider.Element("Height")));
                    else if (collider.Name == "Circle")
                        colliders.Add(new Circle(new Microsoft.Xna.Framework.Vector2((float)collider.Element("CenterX"), (float)collider.Element("CenterY")), (float)collider.Element("Radius")));
                }
                animations.Add(name, new Animation(name, animRects, new Microsoft.Xna.Framework.Vector2(XMovement, YMovement), colliders, timers.ToArray(), XOffset, YOffset));
            }
            return animations;
        }

        private void animationSelector_onOptionChosen(object sender, EventArgs e)
        {
            string animationChosen = (string)sender;
            if (!animations.Animations.ContainsKey(animationChosen))
                return;
            colliders = new List<Collidable>(animations.Animations[animationChosen].Colliders);
            Action<Animation> changeAnimation = (animation) => { this.animations.Animations[animationChosen] = animation; };

            AnimatingPictureBox timingPictureBox = new AnimatingPictureBox(spriteSheet, animations.Animations[animationChosen], changeAnimation);
            timingPictureBox.Size = this.timingPanel.Size - new Size(10,10);
            this.timingPanel.Controls.Clear();
            this.timingPanel.Controls.Add(timingPictureBox);

            FrameSelection selector = new FrameSelection(animations.Animations[animationChosen], spriteSheet, changeAnimation, timingPictureBox.UpdateWidthandHeight);
            this.frameSelectionPanel.Controls.Clear();
            this.frameSelectionPanel.Controls.Add(selector);

            timingPictureBox.game = new AnimatedGameWindow(timingPictureBox.getDrawSurface(), animations.Animations[animationChosen], spriteSheet);
            timingPictureBox.game.Run();
            
        }

        private void SaveButtonClicked(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK) // Test result.
            {
                string saveFileName = saveFileDialog1.FileName;
                Allanimations[selectedClass] = this.animations;

                var animationElements = Allanimations.Select(a => new XElement("Animations", new XAttribute("class", a.Key), new XAttribute("filename", a.Value.filename), new XAttribute("img", a.Value.imgName),
                    a.Value.Animations.Select(x =>
                        new XElement("Animation", new XAttribute("name", x.Value.AnimationName),
                            new XElement("Frames", x.Value.Frames),
                            new XElement("XMovement", x.Value.PosAdjust.X),
                            new XElement("YMovement", x.Value.PosAdjust.Y),
                            new XElement("XOffset", x.Value.Xoffset),
                            new XElement("YOffset", x.Value.Yoffset),
                            new XElement("AnimRects", x.Value.Animations.Select((r, i) =>
                                new XElement("Rect", new XAttribute("timeLength", x.Value.timers[i]),
                                    new XElement("X", r.X),
                                    new XElement("Y", r.Y),
                                    new XElement("Width", r.Width),
                                    new XElement("Height", r.Height)))),
                            new XElement("Colliders", x.Value.Colliders.Select(c => c.GetType() == typeof(_2D_Game.RectangleF) ?
                                new XElement("Rect",
                                    new XElement("X", c.X),
                                    new XElement("Y", c.Y),
                                    new XElement("Width", c.Width),
                                    new XElement("Height", c.Height))
                                :
                                    new XElement("Circle",
                                    new XElement("CenterX", ((Circle)c).Center.X),
                                    new XElement("CenterY", ((Circle)c).Center.Y),
                                    new XElement("Radius", ((Circle)c).Radius))))))));

                XElement documentElements = new XElement("AnimationData", animationElements);

                //XElement root = new XElement("Animations", new XAttribute("class", selectedClass), new XAttribute("filename", filename), new XAttribute("img", "player"),
                //    animations.Select(x =>
                //        new XElement("Animation", new XAttribute("name", x.Value.AnimationName),
                //            new XElement("Frames", x.Value.Frames),
                //            new XElement("XMovement", x.Value.PosAdjust.X),
                //            new XElement("YMovement", x.Value.PosAdjust.Y),
                //            new XElement("XOffset", x.Value.Xoffset),
                //            new XElement("YOffset", x.Value.Yoffset),
                //            new XElement("AnimRects", x.Value.Animations.Select((r, i) =>
                //                new XElement("Rect", new XAttribute("timeLength", x.Value.timers[i]),
                //                    new XElement("X", r.X),
                //                    new XElement("Y", r.Y),
                //                    new XElement("Width", r.Width),
                //                    new XElement("Height", r.Height)))),
                //            new XElement("Colliders", x.Value.Colliders.Select(c => c.GetType() == typeof(_2D_Game.RectangleF) ?
                //                new XElement("Rect",
                //                    new XElement("X", c.X),
                //                    new XElement("Y", c.Y),
                //                    new XElement("Width", c.Width),
                //                    new XElement("Height", c.Height))
                //                :
                //                    new XElement("Circle",
                //                    new XElement("CenterX", ((Circle)c).Center.X),
                //                    new XElement("CenterY", ((Circle)c).Center.Y),
                //                    new XElement("Radius", ((Circle)c).Radius)))))));
                var doc = new XDocument(documentElements);
                doc.Save(saveFileName);
            }
        }

        private void NewButtonClicked(object sender, EventArgs e)
        {

        }

        private void colliderPanel_Paint(object sender, PaintEventArgs e)
        {
            if (colliders == null) return;
            Graphics g = e.Graphics;
            g.Clear(Color.Transparent);

            Pen pen = new Pen(Color.Black, 1);
            pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;

            //foreach (var collider in colliders)
            //{
            //    if (collider.GetType() == typeof(Circle))
            //    {
            //        Microsoft.Xna.Framework.Rectangle rect = collider.ToRectangle();
            //        g.DrawEllipse(pen, colliderPanel.Location.X + rect.X, colliderPanel.Location.Y + rect.Y, rect.Width, rect.Height);
            //    }
            //    if (collider.GetType() == typeof(Microsoft.Xna.Framework.Rectangle))
            //    {
            //        Microsoft.Xna.Framework.Rectangle rect = collider.ToRectangle();
            //        g.DrawRectangle(pen, colliderPanel.Location.X + rect.X, colliderPanel.Location.Y + rect.Y, rect.Width, rect.Height);
            //    }
            //    if (collider.GetType() == typeof(Rectangle))
            //    {
            //        Microsoft.Xna.Framework.Rectangle rect = collider.ToRectangle();
            //        g.DrawRectangle(pen, colliderPanel.Location.X + rect.X, colliderPanel.Location.Y + rect.Y, rect.Width, rect.Height);
            //    }
            //    if (collider.GetType() == typeof(_2D_Game.RectangleF))
            //    {
            //        Microsoft.Xna.Framework.Rectangle rect = collider.ToRectangle();
            //        g.DrawRectangle(pen, colliderPanel.Location.X + rect.X, colliderPanel.Location.Y + rect.Y, rect.Width, rect.Height);
            //    }
            //}
        }

        private void ChangeAnimationButton_Click(object sender, EventArgs e)
        {
            //Create Option Selector to choose class from Xml
            OptionSelector classSelector = new OptionSelector(animationChoices, "Edit this Class");
            if (classSelector.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                return;
            classSelector.Close();

            //Load Animations from Xml and create List.
            this.Allanimations[selectedClass] = this.animations;
            this.selectedClass = classSelector.selectedOption;
            this.Text = String.Format("Animation Editor (Editing: {0})", selectedClass);
            this.selectedClassLabel.Text = selectedClass + " Animations";
            this.animations = this.Allanimations[selectedClass];

            //Create and show Scrollable Image form.
            if (this.animations.filename == "")
                throw new IOException("Class is missing image file");

            spriteSheet = new Bitmap(this.animations.filename);
            SaveButton.Enabled = true;
            ChangeClassButton.Enabled = true;
            ChangeClassButton.Visible = true;

            //Create OptionSelector to choose animations to edit.
            OptionSelector animationSelector = new OptionSelector(animations.Animations.Select(x => x.Key).ToList(), "Edit this Animation", false);
            animationSelector.onOptionChosen += animationSelector_onOptionChosen;
            animationSelector.MinimumSize = new Size(animationPanel.Size.Width, 150);
            animationSelector.MaximumSize = new Size(animationPanel.Size.Width, 450);
            animationSelector.Size = new Size(animationPanel.Size.Width, animationPanel.Size.Height);
            animationSelector.Dock = DockStyle.Fill;
            animationSelector.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom;
            animationPanel.Controls.RemoveAt(0);
            animationPanel.Controls.Add(animationSelector);
            animationSelector.Show();
        }
    }
}
