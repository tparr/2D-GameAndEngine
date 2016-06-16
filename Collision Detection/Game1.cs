using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using AnimationEditorForms;
using System;
using _2D_Game;
namespace Collision_Detection
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Circle circle1;
        Circle circle2;
        Circle tempCircle1;
        Circle tempCircle2;
        Texture2D circleTexture;
        Texture2D RectangleTexture;
        KeyboardState keys;
        SpriteFont font;
        AnimationEditorForms.RotatedRectangle rect;
        Vector2 origin;
        float rotation;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

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
            RectangleTexture = Content.Load<Texture2D>("enemy");
            circleTexture = Content.Load<Texture2D>("ball");
            circle1 = new Circle(new Vector2(100,100), 50);
            circle2 = new Circle(new Vector2(200, 200), 50);
            rect = new AnimationEditorForms.RotatedRectangle(new Rectangle(300, 200, 100, 50),50);
            font = Content.Load<SpriteFont>("Font");
            tempCircle1 = new Circle(circle1);
            tempCircle2 = new Circle(circle2);
            origin = rect.Origin;
            rotation = rect.Rotation;
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

            //Rotated Rectangle Collision not working
            //if (keys.IsKeyDown(Keys.W))
            //{
            //    tempCircle1.Center = new Vector2(tempCircle1.Center.X, tempCircle1.Center.Y - 1);
            //    if (tempCircle1.Intersects(rect))
            //        tempCircle1.Center = circle1.Center;
            //    else
            //        circle1.Center = tempCircle1.Center;
            //}
            //if (keys.IsKeyDown(Keys.S))
            //{
            //    tempCircle1.Center = new Vector2(tempCircle1.Center.X, tempCircle1.Center.Y + 1);
            //    if (tempCircle1.Intersects(rect))
            //        tempCircle1.Center = circle1.Center;
            //    else
            //        circle1.Center = tempCircle1.Center;
            //}
            //if (keys.IsKeyDown(Keys.A))
            //{
            //    tempCircle1.Center = new Vector2(tempCircle1.Center.X - 1, tempCircle1.Center.Y);
            //    if (tempCircle1.Intersects(rect))
            //        tempCircle1.Center = circle1.Center;
            //    else
            //        circle1.Center = tempCircle1.Center;
            //}
            //if (keys.IsKeyDown(Keys.D))
            //{
            //    tempCircle1.Center = new Vector2(tempCircle1.Center.X + 1, tempCircle1.Center.Y);
            //    if (tempCircle1.Intersects(rect))
            //        tempCircle1.Center = circle1.Center;
            //    else
            //        circle1.Center = tempCircle1.Center;
            //}

            if (keys.IsKeyDown(Keys.NumPad4))
                origin.X--;
            if (keys.IsKeyDown(Keys.NumPad6))
                origin.X++;
            if (keys.IsKeyDown(Keys.NumPad8))
                origin.Y++;
            if (keys.IsKeyDown(Keys.NumPad2))
                origin.Y--;

            if (keys.IsKeyDown(Keys.Left))
                rotation -= 0.1f;
            if (keys.IsKeyDown(Keys.Right))
                rotation += 0.1f;

            if (keys.IsKeyDown(Keys.P))
                Console.WriteLine();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Blue);

            spriteBatch.Begin();

            spriteBatch.Draw(circleTexture, circle1.ToRectangle(), Color.Green);
            //spriteBatch.Draw(circleTexture, circle2.toRectangle(), Color.White);
            spriteBatch.Draw(RectangleTexture, rect.CollisionRectangle,null, Color.White,rect.Rotation,new Vector2(15,10),SpriteEffects.None,0f);
            spriteBatch.Draw(RectangleTexture, rect.CollisionRectangle, null, Color.Yellow, rotation, origin, SpriteEffects.None, 0f);
            spriteBatch.DrawString(font, "Rotation: " + rotation + " OriginX: " + origin.X + " OriginY: " + origin.Y, new Vector2(200, 400), Color.White);
            //if (circle1.Intersects(rect))
                //spriteBatch.DrawString(font, "Colliding", new Vector2(100, 100), Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
