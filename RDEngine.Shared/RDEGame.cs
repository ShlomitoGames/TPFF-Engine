using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using RDEngine.Engine;
using RDEngine.GameScripts;

namespace RDEngine
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class RDEGame : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private RenderTarget2D _target; //The RenderTarget for the pixelated scene
        public static int ScaleFactor { get; } = 3;

        public static int UpscaledWidth = 1366; //The game wont run at exactly this resolution, it will instead run at the closest multiple of the ScaleFactor
        public static int UpscaledHeight = 768;

        public static int ScreenWidth { get; } = UpscaledWidth / ScaleFactor;
        public static int ScreenHeight { get; } = UpscaledHeight / ScaleFactor;
        public static Point ScreenSize = new Point(ScreenWidth, ScreenHeight);

        public RDEGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = ScreenWidth * ScaleFactor;
            graphics.PreferredBackBufferHeight = ScreenHeight * ScaleFactor;
            IsMouseVisible = true;

            //Unlocks the FPS
            graphics.SynchronizeWithVerticalRetrace = false;
            IsFixedTimeStep = false;

            graphics.ApplyChanges();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {

            //Create a RenderTarget where the pixelated scene will be drawn on
            //A pixel of padding is added to the screen to account for the smooth offset
            _target = new RenderTarget2D(GraphicsDevice, ScreenWidth + 2, ScreenHeight + 2);

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
            ContentStorer.LoadContent(Content,
                new List<string>()
                {
                    "whitepixel", "Mario", "Block", "Koopa"
                },
                new List<string>()
                {
                    "testfont",
                    "wreckside"
                }
                );

            SceneHandler.Content = Content;
            SceneHandler.LoadScene(new TestScene());
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
            //MouseState mouseState = Mouse.GetState();
            KeyboardState keyboardState = Keyboard.GetState();
            GamePadState gamePadState = default;
            try { gamePadState = GamePad.GetState(PlayerIndex.One); }
            catch (NotImplementedException) { /* ignore gamePadState */ }

            if (keyboardState.IsKeyDown(Keys.Escape) ||
                keyboardState.IsKeyDown(Keys.Back) ||
                gamePadState.Buttons.Back == ButtonState.Pressed)
            {
                try { Exit(); }
                catch (PlatformNotSupportedException) { /* ignore */ }
            }


            // Add your update logic here

            Input.UpdateInput();
            Time.GameTime = gameTime;

            //Scene functions
            SceneHandler.ActiveScene.UpdateScene();

            Input.UpdateLastInput();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            //Set the target for the pixelated scene
            GraphicsDevice.SetRenderTarget(_target);
            GraphicsDevice.Clear(Color.CornflowerBlue);

            //Drawing the pixelated scene
            spriteBatch.Begin();
            SceneHandler.ActiveScene.DrawScene(spriteBatch);
            spriteBatch.End();

            //Set rendering back to the back buffer
            GraphicsDevice.SetRenderTarget(null);

            //Drawing the normal scene
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            //Drawing the pixelated scene inside the normal scene
                //The offset is how much of the camera positions changes when snapped to the pixel grid, so it's smooth once scaled up.
            Vector2 offset = SceneHandler.ActiveScene.WorldCameraPos * ScaleFactor - Vector2.Floor(SceneHandler.ActiveScene.CameraPos);
            if (MathF.Abs(offset.X) > 2 * ScaleFactor || MathF.Abs(offset.Y) > 2 * ScaleFactor)
                throw new ArithmeticException("Offset cannot be greater than scaling factor");

            //ScaleFactor is subtracted from X and Y to account for the 1px-wide padding for the offset
            spriteBatch.Draw(_target, new Rectangle((int)offset.X - ScaleFactor, (int)offset.Y - ScaleFactor, (ScreenWidth + 2) * ScaleFactor, (ScreenHeight + 2) * ScaleFactor), Color.White);

            //Draws the component debug lines
            SceneHandler.ActiveScene.DrawComponents(GraphicsDevice, spriteBatch);

            //Draws the UI
            SceneHandler.ActiveScene.DrawUI(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
