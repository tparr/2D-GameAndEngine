using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;

/*
Thanks to:
 * Shawn Hargreaves - http://blogs.msdn.com/shawnhar/archive/2007/01/23/using-xna-with-winforms.aspx
 * Roger Boesche    - http://blogs.msdn.com/rogerboesch/archive/2007/01/20/tipp-xna-in-eigene-applikationen-einbinden-winforms.aspx
 * Ziggy            - http://www.ziggyware.com/readarticle.php?article_id=82
*/

namespace AnimationEditorForms
{
    public partial class XNAForm : Form
    {
        protected ContentManager content;
        protected GraphicsDevice device;
        private Control xnaControl;

        protected SimulationTime gameTime;
        private DateTime oldTime, currentTime, startTime;
        private TimeSpan delta;
        private bool done = false;

        private const float timeStep = 1000f / 60f; // 60 fps update
        private float nextUpdate = 0;

        private bool isFixedTimeStep = true;

        /// <summary>
        /// The System.Windows.Forms.Control to render to
        /// </summary>
        protected Control XNAControl
        {
            get { return xnaControl; }
            set { xnaControl = value; }
        }

        /// <summary>
        /// Whether to call Update() every 
        /// </summary>
        protected bool IsFixedTimeStep
        {
            get { return isFixedTimeStep; }
            set { isFixedTimeStep = value; }
        }

        public XNAForm()
        {

        }

        protected virtual void Initialise()
        {
            FormClosing += new FormClosingEventHandler(Shutdown);
            
            startTime = currentTime = DateTime.Now;

            gameTime = new SimulationTime();

            if (xnaControl == null)
                throw new Exception("No control to render to (this.XNAControl)");

            PresentationParameters parameters = new PresentationParameters();
            
            parameters.BackBufferCount = 1;
            parameters.IsFullScreen = false;
            parameters.SwapEffect = SwapEffect.Discard;
            parameters.BackBufferWidth = xnaControl.Width;
            parameters.BackBufferHeight = xnaControl.Height;

            parameters.AutoDepthStencilFormat   = DepthFormat.Depth24Stencil8;
            parameters.EnableAutoDepthStencil   = true;
            parameters.PresentationInterval     = PresentInterval.Default;
            parameters.BackBufferFormat         = SurfaceFormat.Unknown;
            parameters.MultiSampleType          = MultiSampleType.None;

            device = new GraphicsDevice(GraphicsAdapter.DefaultAdapter, DeviceType.Hardware, xnaControl.Handle, CreateOptions.HardwareVertexProcessing, parameters);

            content = new ContentManager(new GraphicsDeviceService(device));

            xnaControl.Resize += new EventHandler(ResizeWindow);
        }

        private void Shutdown(object sender, FormClosingEventArgs e)
        {
            done = true;
            e.Cancel = true;
        }

        protected virtual void LoadGraphicsContent(bool loadAllContent)
        {
            if (loadAllContent)
            {

            }
        }

        protected virtual void UnloadGraphicsContent(bool unloadAllContent)
        {
            if (unloadAllContent)
            {
                content.Unload();
            }
        }

        public virtual void Update(SimulationTime gameTime)
        {

        }

        public virtual void Draw(SimulationTime gameTime)
        {
            device.Present();
        }

        private void ResizeWindow(object sender, EventArgs e)
        {
            if (xnaControl.Width < 1 || xnaControl.Height < 1)
                return;

            device.PresentationParameters.BackBufferWidth = xnaControl.Width;
            device.PresentationParameters.BackBufferHeight = xnaControl.Height;

            device.Reset();
        }

        private void UpdateGameTime()
        {
            gameTime.ElapsedGameTime = currentTime - oldTime;
        }

        public void Run()
        {
            Initialise();

            LoadGraphicsContent(true);

            while (!done)
            {
                oldTime = currentTime;

                currentTime = DateTime.Now;

                delta = currentTime - oldTime;

                gameTime.TotalGameTime = currentTime - startTime;
                gameTime.TotalMilliseconds += delta.Milliseconds;

                if (isFixedTimeStep)
                {
                    if (gameTime.TotalMilliseconds >= nextUpdate)
                    {
                        UpdateGameTime();

                        Update(gameTime);

                        nextUpdate = gameTime.TotalMilliseconds + timeStep;
                    }
                }
                else
                {
                    UpdateGameTime();

                    Update(gameTime);
                }

                Draw(gameTime);

                Application.DoEvents();
            }

            UnloadGraphicsContent(true);

            Close();
        }

        
    }

#region GraphicsDeviceService
    public class GraphicsDeviceService : IGraphicsDeviceService, IServiceProvider
    {
        private GraphicsDevice graphicsDevice = null;

        public GraphicsDeviceService(GraphicsDevice GraphicsDevice)
        {
            graphicsDevice = GraphicsDevice;
        }

        public GraphicsDevice GraphicsDevice
        {
            get { return graphicsDevice; }
        }

        public event EventHandler DeviceCreated;
        public event EventHandler DeviceDisposing;
        public event EventHandler DeviceReset;
        public event EventHandler DeviceResetting;

        public Object GetService(System.Type ServiceType)
        {
            if (ServiceType == typeof(IGraphicsDeviceService))
                return this;
            else
                throw new ArgumentException(); ;
        }
    }
#endregion
#region SimulationTime
    public struct SimulationTime
    {
        private TimeSpan elapasedGameTime;
        private TimeSpan totalGameTime;
        private int totalMilliseconds;

        public TimeSpan ElapsedGameTime
        {
            get { return elapasedGameTime; }
            set { elapasedGameTime = value; }
        }

        public TimeSpan TotalGameTime
        {
            get { return totalGameTime; }
            set { totalGameTime = value; }
        }

        public int TotalMilliseconds
        {
            get { return totalMilliseconds; }
            set { totalMilliseconds = value; }
        }
    }
#endregion
}