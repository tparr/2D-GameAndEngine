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
    public partial class AnimatingPictureBox : Form
    {
        private Bitmap Image;
        public Animation Animation;
        bool playing;
        int largestFrameWidth;
        int largestFrameHeight;
        int frame = 0;
        public AnimatedGameWindow game;
        public PictureBox PctSurface;

        public AnimatingPictureBox()
        {
            InitializeComponent();
        }

        public AnimatingPictureBox(Image image)
        {
            this.Image = new Bitmap(image);
            InitializeComponent();
        }

        public AnimatingPictureBox(Image image, Animation anim)
        {
            this.Image = new Bitmap(image);
            this.Animation = anim;

            this.largestFrameWidth = this.Animation.Animations.Max(x => x.Width);
            this.largestFrameHeight = this.Animation.Animations.Max(x => x.Height);
            this.Size = new Size(300, 300);
            InitializeComponent();
        }

        public void SetAnimation(Animation anim)
        {
            this.Animation = anim;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            return;
            //base.OnPaint(e);
            //if (playing == false) return;
            Graphics g = e.Graphics;

            if (this.Animation == null || this.Animation.Animations == null)
            {
                Font drawFont = new System.Drawing.Font("Arial", 16);
                SolidBrush drawBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);
                System.Drawing.Point drawPoint = new System.Drawing.Point(this.Width / 2, this.Height / 2);
                g.DrawString("No Animation to Play", drawFont, drawBrush, drawPoint);
            }
            else
            {
                g.DrawImage(
                this.Image,
                new System.Drawing.Rectangle(this.Location.X, this.Location.Y, Animation.Animations[frame].Width, Animation.Animations[frame].Height),
                new System.Drawing.Rectangle(Animation.Animations[frame].X, Animation.Animations[frame].Y, Animation.Animations[frame].Width, Animation.Animations[frame].Height),
                GraphicsUnit.Pixel);
                frame++;
                if (frame >= Animation.Animations.Count)
                    frame = 0;
            }
        }
        public IntPtr getDrawSurface()
        {
            return pctSurface.Handle;
        }
    }

    public class AnimatedGameWindow : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private IntPtr drawSurface;
        Animation Animation;
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

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Microsoft.Xna.Framework.Color.Blue);

            spriteBatch.Begin();

            spriteBatch.Draw(texture, new Microsoft.Xna.Framework.Rectangle(0, 0, 28, 28), Microsoft.Xna.Framework.Color.White);

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
                e.GraphicsDeviceInformation.PresentationParameters.DeviceWindowHandle =
                drawSurface;
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