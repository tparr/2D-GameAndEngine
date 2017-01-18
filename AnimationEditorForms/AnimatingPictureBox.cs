using System;
using System.Drawing;
using System.Linq;
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
            Image = new Bitmap(image);
            InitializeComponent();
        }

        public AnimatingPictureBox(Image image, Animation anim, Action<Animation> updateAnimation)
        {
            Image = new Bitmap(image);
            Animation = anim;
            InitializeComponent();

            pctSurface.Width = Animation.Animations.Max(x => x.Width);
            pctSurface.Height = Animation.Animations.Max(x => x.Height);
            Size = new Size(300, 300);
            label2.Text = "Frame: " + Animation.CurrFrame;
            label3.Text = "TimeLength: " + Animation.timers[Animation.CurrFrame];
            AnimationChanged = updateAnimation;
        }

        public void SetAnimation(Animation anim)
        {
            Animation = anim;
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
            if (newSize.Width > Width)
                Width = newSize.Width + 20;
            if (newSize.Height > Height)
                Height = newSize.Height + 20;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var oldSize = pctSurface.Size;
            pctSurface.Size = new Size((int)(oldSize.Width * 0.5), (int)(oldSize.Height * 0.5));
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (game.Animation.Paused)
            {
                //Now Playing
                button3.Text = "Pause";
                button4.Enabled = false;
                button5.Enabled = false;
                button6.Enabled = false;
                button7.Enabled = false;
                label2.Enabled = false;
                label3.Enabled = false;
            }
            else
            {
                //Now Paused
                button3.Text = "Play";
                button4.Enabled = true;
                button5.Enabled = true;
                button6.Enabled = true;
                button7.Enabled = true;
                label2.Enabled = true;
                label3.Enabled = true;
            }
            game.Animation.Paused = !game.Animation.Paused;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            game.Animation.DecreaseFrame();
            label2.Text = "Frame: " + game.Animation.CurrFrame;
            label3.Text = "TimeLength: " + game.Animation.timers[game.Animation.CurrFrame];
        }

        private void button5_Click(object sender, EventArgs e)
        {
            game.Animation.IncreaseFrame();
            label2.Text = "Frame: " + game.Animation.CurrFrame;
            label3.Text = "TimeLength: " + game.Animation.timers[game.Animation.CurrFrame];
        }

        private void button7_Click(object sender, EventArgs e)
        {
            game.Animation.timers[game.Animation.CurrFrame]--;
            label3.Text = "TimeLength: " + game.Animation.timers[game.Animation.CurrFrame];
            AnimationChanged(game.Animation);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            game.Animation.timers[game.Animation.CurrFrame]++;
            label3.Text = "TimeLength: " + game.Animation.timers[game.Animation.CurrFrame];
            AnimationChanged(game.Animation);
        }

        private void button8_Click(object sender, EventArgs e)
        {

        }

        public void UpdateWidthandHeight()
        {
            int tempWidth = Animation.Animations.Max(x => x.Width);
            int tempHeight = Animation.Animations.Max(x => x.Height);
            if (tempWidth > pctSurface.Width)
                pctSurface.Width = tempWidth;
            if (tempHeight > pctSurface.Height)
                pctSurface.Height = tempHeight;
            game.Animation = Animation;
            game.UpdateGameWindow();
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
            System.Windows.Forms.Control.FromHandle((Window.Handle)).VisibleChanged += new EventHandler(Game1_VisibleChanged); 
            originalImage = image;
            Animation = animation;
            graphics.PreferredBackBufferWidth = Animation.Animations.Max(x => x.Width);
            graphics.PreferredBackBufferHeight = Animation.Animations.Max(x => x.Height);
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
            graphics.PreferredBackBufferWidth = Animation.Animations.Max(x => x.Width);
            graphics.PreferredBackBufferHeight = Animation.Animations.Max(x => x.Height);
            graphics.ApplyChanges();
        }

        protected override void Update(GameTime gameTime)
        {
            Animation.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Microsoft.Xna.Framework.Color.CornflowerBlue);

            spriteBatch.Begin();

            spriteBatch.Draw(texture, new Vector2(0, 0), Animation.AnimationRect, Microsoft.Xna.Framework.Color.White);

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
                if (System.Windows.Forms.Control.FromHandle((Window.Handle)).Visible == true)
                    System.Windows.Forms.Control.FromHandle((Window.Handle)).Visible = false;
        }

        public static Texture2D BitmapToTexture2D(GraphicsDevice GraphicsDevice, Bitmap image)
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