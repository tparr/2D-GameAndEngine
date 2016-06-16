using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using _2D_Game;

namespace AnimationTesting
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch targetBatch;
        Texture2D spritesheet;
        Animation playingAnimation;
        RenderTarget2D target;
        KeyboardState keys;
        KeyboardState oldkeys;
        int renderTargetWidth = 32;
        int renderTargetHeight = 32;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        public Game1(int width, int height)
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            renderTargetWidth = width;
            renderTargetHeight = height;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            targetBatch = new SpriteBatch(GraphicsDevice);
            target = new RenderTarget2D(GraphicsDevice, renderTargetWidth, renderTargetHeight);
            GraphicsDevice.SetRenderTarget(target);

            spritesheet = Content.Load<Texture2D>("player");
            playingAnimation = World.LoadAnimationsofClass("Fighter")["WalkRight"];
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            keys = Keyboard.GetState();

            if (keys.IsKeyDown(Keys.A) && oldkeys.IsKeyUp(Keys.A))
            {
                renderTargetWidth--;
                SetRenderTarget(renderTargetWidth, renderTargetHeight);
            }
            if (keys.IsKeyDown(Keys.D) && oldkeys.IsKeyUp(Keys.D))
            {
                renderTargetWidth++;
                SetRenderTarget(renderTargetWidth, renderTargetHeight);
            }
            if (keys.IsKeyDown(Keys.W) && oldkeys.IsKeyUp(Keys.W))
            {
                renderTargetHeight--;
                SetRenderTarget(renderTargetWidth, renderTargetHeight);
            }
            if (keys.IsKeyDown(Keys.S) && oldkeys.IsKeyUp(Keys.S))
            {
                renderTargetHeight++;
                SetRenderTarget(renderTargetWidth, renderTargetHeight);
            }


            playingAnimation.Update();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            //set rendering back to the back buffer
            GraphicsDevice.SetRenderTarget(null);

            //render target to back buffer
            targetBatch.Begin();

            targetBatch.Draw(spritesheet, new Rectangle(0, 0, renderTargetWidth, renderTargetHeight), playingAnimation.AnimationRect, Color.White);

            targetBatch.End();

            base.Draw(gameTime);
        }

        public void SetRenderTarget(int width, int height)
        {
            target = new RenderTarget2D(GraphicsDevice, width, height);
            GraphicsDevice.SetRenderTarget(target);
        }
    }
}
