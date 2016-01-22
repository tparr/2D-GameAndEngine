using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace AnimationEditorForms
{
    public partial class Form1 : Form
    {
        string filename;
        string selectedClass;
        Dictionary<string,Animation> animations;
        public Form1()
        {
            InitializeComponent();
        }
        private Form createPictureBox(string filename)
        {
            /* format form */
            Form frmShowPic = new Form();
            frmShowPic.Width = 234;
            frmShowPic.Height = 332;
            frmShowPic.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            frmShowPic.MinimizeBox = false;
            frmShowPic.MaximizeBox = false;
            frmShowPic.ShowIcon = false;
            frmShowPic.StartPosition = FormStartPosition.CenterScreen;
            frmShowPic.FormBorderStyle = FormBorderStyle.None;
            frmShowPic.Text = "Show Picture";
            frmShowPic.Dock = DockStyle.Fill;

            /* add panel */
            Panel panPic = new Panel();
            panPic.AutoSize = false;
            panPic.AutoScroll = true;
            panPic.Dock = DockStyle.Fill;

            /* add picture box */
            PictureBox pbPic = new PictureBox();
            pbPic.SizeMode = PictureBoxSizeMode.AutoSize;
            pbPic.Location = new Point(0, 0);

            panPic.Controls.Add(pbPic);
            frmShowPic.Controls.Add(panPic);

            /* define image */
            pbPic.ImageLocation = filename;

            return frmShowPic;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog1.ShowDialog(); //Show the dialog.
            if (result == DialogResult.OK) //Test result.
            {
                string file = openFileDialog1.FileName;
                string imageFilename = "";
                try
                {
                    StreamReader reader = new StreamReader(file);
                    XDocument doc = XDocument.Load(reader);
                    var animationsChoices = doc.Element("AnimationData").Elements("AnimationClass").Select(x => x.Attribute("class").Value).ToList();
                    Class_Selector classSelector = new Class_Selector(animationsChoices);
                    if (classSelector.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                        return;
                    selectedClass = classSelector.selectedClass;
                    var classAnimations = doc.Element("AnimationData").Elements("AnimationClass").Where(x => x.Attribute("class").Value == selectedClass).FirstOrDefault();
                    animations = new Dictionary<string,Animation>();
                    imageFilename = classAnimations.Attribute("filename").Value;
                    foreach (var node in classAnimations.Elements("Animation"))
                    {
                        int frames = (int)node.Element("Frames");
                        int XMovement = (int)node.Element("XMovement");
                        int YMovement = (int)node.Element("YMovement");
                        int XOffset = (int)node.Element("XOffset");
                        int YOffset = (int)node.Element("YOffset");
                        string name = (string)node.FirstAttribute;
                        List<Microsoft.Xna.Framework.Rectangle> animRects = new List<Microsoft.Xna.Framework.Rectangle>();
                        List<int> timers = new List<int>();
                        foreach (var rect in node.Elements("AnimRect").Elements())
                        {
                            animRects.Add(new Microsoft.Xna.Framework.Rectangle((int)rect.Element("X"),(int)rect.Element("Y"),(int)rect.Element("Width"),(int)rect.Element("Height")));
                        }

                        List<Collidable> colliders = new List<Collidable>();
                        foreach (var collider in node.Elements("Colliders").Elements())
                        {
                            if (collider.Name == "Rect")
                                colliders.Add(new RectangleF((float)collider.Element("X"), (float)collider.Element("Y"), (float)collider.Element("Width"), (float)collider.Element("Height")));
                            else if (collider.Name == "Circle")
                                colliders.Add(new Circle(new Microsoft.Xna.Framework.Vector2((float)collider.Element("CenterX"), (float)collider.Element("CenterY")), (float)collider.Element("Radius")));
                        }
                        animations.Add(name,new Animation(name, animRects, new Microsoft.Xna.Framework.Vector2(XMovement, YMovement), colliders, XOffset, YOffset));
                    }
                }
                catch(IOException)
                {

                }
                if (imageFilename == "")
                    throw new IOException("Class is missing image file");
                PictureBox pictureBox = new PictureBox();
                pictureBox.Location = new Point(0, 0);
                pictureBox.SizeMode = PictureBoxSizeMode.AutoSize;
                pictureBox.Image = Image.FromFile(imageFilename);
                //panel1.Controls.Add(pictureBox);
                Form imageScrollForm = createPictureBox(imageFilename);
                imageScrollForm.TopLevel = false;
                imageScrollForm.AutoScroll = true;
                this.panel1.Controls.Add(imageScrollForm);
                imageScrollForm.Show();
                
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult result = saveFileDialog1.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK) // Test result.
            {
                string name = saveFileDialog1.FileName;
                File.WriteAllText(name, "test");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }
    }
}
