using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using _2D_Game;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AnimationEditorForms
{
    public partial class AnimatingPictureBox : UserControl
    {
        private Bitmap Image;
        public Animation Animation;
        bool playing;
        int largestFrameWidth;
        int largestFrameHeight;
        int frame = 0;
        private Action<Animation> AnimationChanged;
        public AnimatedGameWindow game;

        public AnimatingPictureBox()
        {
            InitializeComponent();
        }

        public AnimatingPictureBox(Image image)
        {
            this.Image = new Bitmap(image);
            InitializeComponent();
        }

        public AnimatingPictureBox(Image image, Animation anim, Action<Animation> updateAnimation)
        {
            this.Image = new Bitmap(image);
            this.Animation = anim;
            InitializeComponent();

            this.pctSurface.Width = this.Animation.Animations.Max(x => x.Width);
            this.pctSurface.Height = this.Animation.Animations.Max(x => x.Height);
            this.Size = new Size(300, 300);
            this.label2.Text = "Frame: " + this.Animation.CurrFrame;
            this.label3.Text = "TimeLength: " + this.Animation.timers[this.Animation.CurrFrame];
            this.AnimationChanged = updateAnimation;
        }

        public void SetAnimation(Animation anim)
        {
            this.Animation = anim;
        }

        public IntPtr getDrawSurface()
        {
            return pctSurface.Handle;
        }

        public void ScaleMultiplyButton_Click(object sender, EventArgs e)
        {
            var oldSize = pctSurface.Size;
            var newSize = new Size(oldSize.Width * 2, oldSize.Height * 2);
            pctSurface.Size = newSize;
            if (newSize.Width > this.Width)
                this.Width = newSize.Width + 20;
            if (newSize.Height > this.Height)
                this.Height = newSize.Height + 20;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var oldSize = pctSurface.Size;
            pctSurface.Size = new Size((int)(oldSize.Width * 0.5), (int)(oldSize.Height * 0.5));
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (this.game.Animation.Paused)
            {
                //Now Playing
                this.button3.Text = "Pause";
                this.button4.Enabled = false;
                this.button5.Enabled = false;
                this.button6.Enabled = false;
                this.button7.Enabled = false;
                this.label2.Enabled = false;
                this.label3.Enabled = false;
            }
            else
            {
                //Now Paused
                this.button3.Text = "Play";
                this.button4.Enabled = true;
                this.button5.Enabled = true;
                this.button6.Enabled = true;
                this.button7.Enabled = true;
                this.label2.Enabled = true;
                this.label3.Enabled = true;
            }
            this.game.Animation.Paused = !this.game.Animation.Paused;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.game.Animation.DecreaseFrame();
            this.label2.Text = "Frame: " + this.game.Animation.CurrFrame;
            this.label3.Text = "TimeLength: " + this.game.Animation.timers[this.game.Animation.CurrFrame];
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.game.Animation.IncreaseFrame();
            this.label2.Text = "Frame: " + this.game.Animation.CurrFrame;
            this.label3.Text = "TimeLength: " + this.game.Animation.timers[this.game.Animation.CurrFrame];
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.game.Animation.timers[this.game.Animation.CurrFrame]--;
            this.label3.Text = "TimeLength: " + this.game.Animation.timers[this.game.Animation.CurrFrame];
            AnimationChanged(this.game.Animation);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.game.Animation.timers[this.game.Animation.CurrFrame]++;
            this.label3.Text = "TimeLength: " + this.game.Animation.timers[this.game.Animation.CurrFrame];
            AnimationChanged(this.game.Animation);
        }

        private void button8_Click(object sender, EventArgs e)
        {

        }

        public void UpdateWidthandHeight()
        {
            int tempWidth = this.Animation.Animations.Max(x => x.Width);
            int tempHeight = this.Animation.Animations.Max(x => x.Height);
            if (tempWidth > this.pctSurface.Width)
                this.pctSurface.Width = tempWidth;
            if (tempHeight > this.pctSurface.Height)
                this.pctSurface.Height = tempHeight;
            this.game.Animation = this.Animation;
            this.game.UpdateGameWindow();
        }
    }

    public class AnimatedGameWindow : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private IntPtr drawSurface;
        public Animation Animation;
        Texture2D texture;
        Bitmap originalImage;
        
        public AnimatedGameWindow(IntPtr drawSurface, Animation animation, Bitmap image)
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            this.drawSurface = drawSurface;
            graphics.PreparingDeviceSettings += new EventHandler<PreparingDeviceSettingsEventArgs>(graphics_PreparingDeviceSettings);
            System.Windows.Forms.Control.FromHandle((this.Window.Handle)).VisibleChanged += new EventHandler(Game1_VisibleChanged); 
            originalImage = image;
            this.Animation = animation;
            graphics.PreferredBackBufferWidth = this.Animation.Animations.Max(x => x.Width);
            graphics.PreferredBackBufferHeight = this.Animation.Animations.Max(x => x.Height);
        }

        protected override void Initialize()
        {
            base.Initialize();
        }
        protected override void LoadContent()
        {            
            spriteBatch = new SpriteBatch(GraphicsDevice);
            texture = BitmapToTexture2D(GraphicsDevice, originalImage);
        }

        public void UpdateGameWindow()
        {
            graphics.PreferredBackBufferWidth = this.Animation.Animations.Max(x => x.Width);
            graphics.PreferredBackBufferHeight = this.Animation.Animations.Max(x => x.Height);
            graphics.ApplyChanges();
        }

        protected override void Update(GameTime gameTime)
        {
            this.Animation.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Microsoft.Xna.Framework.Color.CornflowerBlue);

            spriteBatch.Begin();

            spriteBatch.Draw(texture, new Vector2(0, 0), this.Animation.AnimationRect, Microsoft.Xna.Framework.Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        /// <summary>
        /// Event capturing the construction of a draw surface and makes sure this gets redirected to
        /// a predesignated drawsurface marked by pointer drawSurface
        /// </summary>
        ///<param name="sender"></param>
        ///<param name="e"></param>
        void graphics_PreparingDeviceSettings(object sender, PreparingDeviceSettingsEventArgs e)
        {
                e.GraphicsDeviceInformation.PresentationParameters.DeviceWindowHandle = drawSurface;
        }
 
        /// <summary>
        /// Occurs when the original gamewindows' visibility changes and makes sure it stays invisible
        /// </summary>
        ///<param name="sender"></param>
        ///<param name="e"></param>
        private void Game1_VisibleChanged(object sender, EventArgs e)
        {
                if (System.Windows.Forms.Control.FromHandle((this.Window.Handle)).Visible == true)
                    System.Windows.Forms.Control.FromHandle((this.Window.Handle)).Visible = false;
        }

        public static Texture2D BitmapToTexture2D(GraphicsDevice GraphicsDevice, System.Drawing.Bitmap image)
        {
            // Buffer size is size of color array multiplied by 4 because   
            // each pixel has four color bytes  
            int bufferSize = image.Height * image.Width * 4;

            // Create new memory stream and save image to stream so   
            // we don't have to save and read file  
            System.IO.MemoryStream memoryStream =
                   new System.IO.MemoryStream(bufferSize);
            image.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);

            // Creates a texture from IO.Stream - our memory stream  
            Texture2D texture = Texture2D.FromStream(
                GraphicsDevice, memoryStream);

            return texture;
        }
    }   
}