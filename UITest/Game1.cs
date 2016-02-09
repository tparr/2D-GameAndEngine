using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace UITest
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        ScreenManager manager;
        SpriteFont font;
        ClickableButton button;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            manager = new ScreenManager(graphics.GraphicsDevice.Viewport.Bounds);
            manager.addScreen(new MainMenuScreen(new Microsoft.Xna.Framework.Content.ContentManager(Content.ServiceProvider)));
            button = new ClickableButton("StartMenu", "StartMenu");
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("Font");
            button.LoadTexture(Content.Load<Texture2D>("StartGame"));
            button._highlighted = true;
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
            KeyboardState keyboard = Keyboard.GetState();
            //if (keyboard.IsKeyDown(Keys.W))
            //    button.Rectangle.Height--;
            //if (keyboard.IsKeyDown(Keys.A))
            //    button.Rectangle.Width--;
            //if (keyboard.IsKeyDown(Keys.S))
            //    button.Rectangle.Height++;
            //if (keyboard.IsKeyDown(Keys.D))
            //    button.Rectangle.Width++;
            manager.Update();
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            manager.Draw(spriteBatch,font);
            spriteBatch.Begin();
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
