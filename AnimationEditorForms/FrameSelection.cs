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
    public partial class FrameSelection : UserControl
    {
        public Rectangle rectangle;
        public Collidable collider;
        public int CurrFrameNum = 0;
        public Animation animation;
        private Action<Animation> AnimationChanged;
        private Action updateGameWindowMethod;
        public FrameSelection(Animation animation, Image image, Action<Animation> updateAnimation, Action updateWidthandHeight)
        {
            InitializeComponent();

            if (animation.Animations.Count > 0)
                this.rectangle = RecttoRect(animation.Animations[CurrFrameNum]);
            else
            {
                MessageBox.Show("No Animations");
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

            this.AnimationChanged = updateAnimation;
            this.updateGameWindowMethod = updateWidthandHeight;
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

        private void setButton_Click(object sender, EventArgs e)
        {
            if (AnimationRadioButton.Checked)
            {
                animation.Animations[CurrFrameNum] = RecttoRect(imageBoxEx1.SelectionRegion);
            }
            if (ColliderRadioButton.Checked)
            {
                animation.Colliders[CurrFrameNum] = RecttoRectF(imageBoxEx1.SelectionRegion);
            }
            this.AnimationChanged(this.animation);
            this.updateGameWindowMethod();
        }

        private void AnimationTimingButton_Click(object sender, EventArgs e)
        {
            //AnimatingPictureBox AnimationTimers = new AnimatingPictureBox(imageBoxEx1.Image, this.animation);
            //AnimationTimers.Show();
            //AnimationTimers.game = new AnimatedGameWindow(AnimationTimers.getDrawSurface(), this.animation, new Bitmap(this.imageBoxEx1.Image));
            //AnimationTimers.game.Run();
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

        private void AddFramebutton_Click(object sender, EventArgs e)
        {
            //Add Animation
            animation.Frames++;
            animation.Animations.Add(Microsoft.Xna.Framework.Rectangle.Empty);
            //Add Timer
            var timerList = animation.timers.ToList();
            timerList.Add(1);
            animation.timers = timerList.ToArray();
            //Add Collider
            animation.Colliders.Add(_2D_Game.RectangleF.Empty);
            //Update Trackbar
            trackBar1.SetRange(0, animation.Frames - 1);
            trackBar1.Value = animation.Frames - 1;

            AnimationChanged(this.animation);
        }

        private void DeleteFrameButton_Click(object sender, EventArgs e)
        {
            if (animation.Frames == 1) return;
            //Add Animation
            animation.Frames--;
            animation.Animations.RemoveAt(CurrFrameNum);
            //Add Timer
            var timerList = animation.timers.ToList();
            timerList.RemoveAt(CurrFrameNum);
            animation.timers = timerList.ToArray();
            //Add Collider
            animation.Colliders.RemoveAt(CurrFrameNum);

            trackBar1.SetRange(0, animation.Frames - 1);
            trackBar1.Value = animation.Frames - 1;

            AnimationChanged(this.animation);
        }
    }
}
