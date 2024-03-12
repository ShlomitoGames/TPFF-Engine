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
using RDEngine.GameScripts.Scenes;

namespace RDEngine
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class RDEGame : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private RenderTarget2D _worldTarget; //The RenderTarget for the pixelated scene
        public static int ScaleFactor { get; } = 4;

        public static int UpscaledScrWidth { get; } = 1366;
        public static int UpscaledScrHeight { get; } = 768;
        public static Vector2 UpscaledScrSize { get; } = new Vector2(UpscaledScrWidth, UpscaledScrHeight);

        public static int ScreenWidth { get; } = UpscaledScrWidth / ScaleFactor;
        public static int ScreenHeight { get; } = UpscaledScrHeight / ScaleFactor;
        public static Vector2 ScreenSize { get; } = new Vector2(ScreenWidth, ScreenHeight);

        public RDEGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = UpscaledScrWidth;
            graphics.PreferredBackBufferHeight = UpscaledScrHeight;
            IsMouseVisible = false;

//#if BLAZORGL
            //Unlocks the FPS
            graphics.SynchronizeWithVerticalRetrace = false;
            IsFixedTimeStep = false;
            //I'd enable it by default if my bad physics were less frame-rate independant,
            //but as it stands I'll only use it because on Web it becomes unplayably laggy if it's not enabled for some reason
//#endif

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
            _worldTarget = new RenderTarget2D(GraphicsDevice, ScreenWidth + 4, ScreenHeight + 4);

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
                    "Level1", "Level2", "Level3",
                    "Floor", "Rug", "Border", "Wall", "Floor2",
                    "Table1x2", "Table1x3", "Table2x1", "Table3x1",
                    "ATable1x1", "ATable3x2", "ATable2x2", "ATable1x2", "ATable2x1", "ATable4x3",
                    "Door", "DoorOpen", "Key", "Spot",
                    "Player1", "Player2", "Player3", "Player4", "Player5"
                },
                new List<string>()
                {
                    "Arial", "Coolvetica", "Pixel", "PixelBig"
                },
                new List<string>()
                {
                    "MysteryLoop"
                },
                new List<string>()
                {
                    "Door", "SpringyThud", "Thud", "Key", "Spot",
                    "Talk1", "Talk2", "Talk3",
                }
            );
#if DEBUG
            SceneHandler.LoadScene(new Level3());
#else
            SceneHandler.LoadScene(new SplashScreen());
#endif
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
            SceneHandler.ActiveScene.UpdateSceneElements();

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
            GraphicsDevice.SetRenderTarget(_worldTarget);
            GraphicsDevice.Clear(SceneHandler.ActiveScene.CameraColor);

            //Drawing the pixelated scene
            spriteBatch.Begin(blendState: BlendState.AlphaBlend, sortMode: SpriteSortMode.FrontToBack);
            SceneHandler.ActiveScene.DrawScene(spriteBatch);
            spriteBatch.End();

            //Set rendering back to the back buffer
            GraphicsDevice.SetRenderTarget(null);
            //Drawing the normal scene
            spriteBatch.Begin(samplerState: SamplerState.PointClamp, blendState: BlendState.AlphaBlend, sortMode: SpriteSortMode.FrontToBack);

            //Drawing the pixelated scene inside the normal scene
                //The offset is how much of the camera positions changes when snapped to the pixel grid, so it's smooth once scaled up.
            Vector2 offset = SceneHandler.ActiveScene.PixelCameraPos * ScaleFactor - Vector2.Floor(SceneHandler.ActiveScene.CameraPos);
            if (MathF.Abs(offset.X) > 2 * ScaleFactor || MathF.Abs(offset.Y) > 2 * ScaleFactor)
                throw new ArithmeticException("Offset cannot be greater than scaling factor");

            //2 * ScaleFactor is subtracted from X and Y to account for the 2px-wide padding for the offset
            spriteBatch.Draw(_worldTarget, new Rectangle((int)offset.X - ScaleFactor * 2, (int)offset.Y - ScaleFactor * 2, (ScreenWidth + 4) * ScaleFactor, (ScreenHeight + 4) * ScaleFactor), Color.White);

            //Draws the component debug lines
            SceneHandler.ActiveScene.DrawComponents(GraphicsDevice, spriteBatch);

            //Draws the UI
            SceneHandler.ActiveScene.DrawUI(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
