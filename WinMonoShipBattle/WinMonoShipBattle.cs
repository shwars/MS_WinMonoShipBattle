#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Input.Touch;
#endregion

namespace WinMonoShipBattle
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class WinMonoShipBattle : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;


        int width, height;

        int score;

        // Frame counter when explosion is shown
        int explode;

        // Variable to display and move ship
        Texture2D ship;
        Vector2 ship_pos = new Vector2(0, 30);
        Vector2 ship_dir = new Vector2(3, 0);

        // Rocket and Explosion
        Texture2D rocket, explosion;
        Vector2 bpos = new Vector2(0, 0);

        public WinMonoShipBattle()
            : base()
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
            base.Initialize();
            width = GraphicsDevice.Viewport.Width;
            height = GraphicsDevice.Viewport.Height;

        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load all my content
            ship = Content.Load<Texture2D>("Ship.PNG");
            rocket = Content.Load<Texture2D>("Rocket.png");
            explosion = Content.Load<Texture2D>("Explode.PNG");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
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

            if (explode > 0)
            {
                explode--;
                if (explode == 0)
                {
                    ship_pos.X = 0;
                    ship_dir.X = 3;
                }
                base.Update(gameTime);
                return;
            }

            ship_pos += ship_dir;
            if (ship_pos.X <= 0 || ship_pos.X > width - ship.Width)
            {
                ship_dir = -ship_dir;
            }
            var tc = TouchPanel.GetState();
            if (tc.Count > 0)
            {
                bpos.X = tc[0].Position.X;
                bpos.Y = height - 10;
            }
            if (bpos != Vector2.Zero)
            {
                bpos += new Vector2(0, -7);
                if (bpos.Y <= ship_pos.Y + ship.Height && bpos.Y >= 0 && bpos.X >= ship_pos.X && bpos.X <= ship_pos.X + ship.Width)
                {
                    score++;
                    explode = 20;
                    ship_pos.X = bpos.X - explosion.Width / 2;
                    bpos = Vector2.Zero;
                }
                if (bpos.Y == 0) bpos = Vector2.Zero;
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            spriteBatch.Begin();
            if (explode == 0)
            {
                spriteBatch.Draw(ship, ship_pos, null, Color.White,0f,Vector2.Zero,1.0f,ship_dir.X>0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None,0f);
            }
            else spriteBatch.Draw(explosion, ship_pos, Color.White);

            if (bpos != Vector2.Zero)
            {
                spriteBatch.Draw(rocket, bpos, Color.Red);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
