using _2D_Game;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AnimationEditorForms
{
    public partial class FrameSelection : Form
    {
        public Rectangle rectangle;
        public Collidable collider;
        public int CurrFrameNum = 0;
        public Animation animation;
        public FrameSelection(Animation animation, Image image)
        {
            InitializeComponent();

            if (animation.Animations.Count > 0)
                this.rectangle = RecttoRect(animation.Animations[CurrFrameNum]);
            else
            {
                MessageBox.Show("No Animations");
                this.Close();
            }
            if (animation.Colliders.Count > 0)
                this.collider = animation.Colliders[CurrFrameNum];
            trackBar1.SetRange(0, animation.Animations.Count - 1);
            this.animation = animation;
            this.AnimationNameLabel.Text = "Animation: " + animation.AnimationName;
            imageBoxEx1.Image = image;
            frameNumberLabel.Text = "Frame Number: " + CurrFrameNum;
            if (AnimationRadioButton.Checked)
            {
                this.rectangle = RecttoRect(animation.Animations[CurrFrameNum]);
                imageBoxEx1.SelectionRegion = this.rectangle;
            }
            if (ColliderRadioButton.Checked)
            {
                this.collider = animation.Colliders[CurrFrameNum];
                imageBoxEx1.SelectionRegion = RecttoRect(this.collider.ToRectangle());
            }
        }

        private void AnimationRadioButtonChecked(object sender, EventArgs e)
        {
            imageBoxEx1.SelectionColor = Color.Blue;
            imageBoxEx1.SelectionRegion = rectangle;
        }

        private void ColliderRadioButtonChecked(object sender, EventArgs e)
        {
            imageBoxEx1.SelectionColor = Color.Red;
            if (collider == null)
                imageBoxEx1.SelectionMode = Cyotek.Windows.Forms.ImageBoxSelectionMode.None;
            else
                imageBoxEx1.SelectionRegion = RecttoRect(collider.ToRectangle());
        }

        private Rectangle RecttoRect(Microsoft.Xna.Framework.Rectangle rect)
        {
            return new Rectangle(rect.X, rect.Y, rect.Width, rect.Height);
        }

        private Microsoft.Xna.Framework.Rectangle RecttoRect(Rectangle rect)
        {
            return new Microsoft.Xna.Framework.Rectangle(rect.X, rect.Y, rect.Width, rect.Height);
        }

        private Microsoft.Xna.Framework.Rectangle RecttoRect(System.Drawing.RectangleF rect)
        {
            return new Microsoft.Xna.Framework.Rectangle((int)rect.X, (int)rect.Y, (int)rect.Width, (int)rect.Height);
        }

        private _2D_Game.RectangleF RecttoRectF(System.Drawing.RectangleF rect)
        {
            return new _2D_Game.RectangleF(rect.X, rect.Y, rect.Width, rect.Height);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (AnimationRadioButton.Checked)
            {
                this.rectangle = RecttoRect(animation.Animations[CurrFrameNum]);
                imageBoxEx1.SelectionRegion = this.rectangle;
            }
            if (ColliderRadioButton.Checked)
            {
                this.collider = animation.Colliders[CurrFrameNum];
                imageBoxEx1.SelectionRegion = RecttoRect(this.collider.ToRectangle());
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (AnimationRadioButton.Checked)
            {
                animation.Animations[CurrFrameNum] = RecttoRect(imageBoxEx1.SelectionRegion);
            }
            if (ColliderRadioButton.Checked)
            {
                animation.Colliders[CurrFrameNum] = RecttoRectF(imageBoxEx1.SelectionRegion);
            }
        }

        private void AnimationTimingButton_Click(object sender, EventArgs e)
        {
            AnimatingPictureBox AnimationTimers = new AnimatingPictureBox(imageBoxEx1.Image, this.animation);
            AnimationTimers.Show();
            //DialogResult result = 
            //if (result == System.Windows.Forms.DialogResult.OK)
            //{

            //}
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            CurrFrameNum = trackBar1.Value;
            frameNumberLabel.Text = "Frame Number: " + CurrFrameNum;
            if (AnimationRadioButton.Checked)
            {
                this.rectangle = RecttoRect(animation.Animations[CurrFrameNum]);
                imageBoxEx1.SelectionRegion = this.rectangle;
            }
            if (ColliderRadioButton.Checked)
            {
                this.collider = animation.Colliders[CurrFrameNum];
                imageBoxEx1.SelectionRegion = RecttoRect(this.collider.ToRectangle());
            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            CurrFrameNum = trackBar1.Value;
            frameNumberLabel.Text = "Frame Number: " + CurrFrameNum;
            if (AnimationRadioButton.Checked)
            {
                this.rectangle = RecttoRect(animation.Animations[CurrFrameNum]);
                imageBoxEx1.SelectionRegion = this.rectangle;
            }
            if (ColliderRadioButton.Checked)
            {
                this.collider = animation.Colliders[CurrFrameNum];
                imageBoxEx1.SelectionRegion = RecttoRect(this.collider.ToRectangle());
            }
        }
    }
}
