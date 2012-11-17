using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SleepwalkerEngine;
using System;
using System.Diagnostics;

namespace Sleepwalker
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        SceneManager world;
        Renderer renderer;
        InputManager inputManager;

        SceneNode sn1;
        SceneNode sn2;
        int snV;
        int snVY;

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
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            world = new SceneManager();
            renderer = new Renderer(spriteBatch);

            sn1 = new SceneNode()
            {
                Name = "Flag1",
                Position = new Vector2(300, 300),
                Sprite = Content.Load<Texture2D>("flag")
            };

            sn2 = new SceneNode()
            {
                Name = "Flag2",
                Position = new Vector2(200, 300),
                Sprite = Content.Load<Texture2D>("flag")
            };

            snV = 0;
            snVY = 0;

            world.Add(sn1);
            world.Add(sn2);

            inputManager = new InputManager();

            inputManager.AddKeyBinding("Exit Game");
            inputManager["Exit Game"].Add(Keys.Escape);
            inputManager["Exit Game"].Add(MouseButton.Right);

            inputManager.AddKeyBinding("Move Left");
            inputManager["Move Left"].Add(Keys.Left);

            inputManager.AddKeyBinding("Move Right");
            inputManager["Move Right"].Add(Keys.Right);

            inputManager.AddKeyBinding("Move Up");
            inputManager["Move Up"].Add(Keys.Up);

            inputManager.AddKeyBinding("Move Down");
            inputManager["Move Down"].Add(Keys.Down);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // TODO: use this.Content to load your game content here
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
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                inputManager["Exit Game"].WasPressed)
                this.Exit();
            snV = 0;
            snVY = 0;
            if (inputManager["Move Left"].IsDown)
            {
                snV = -2;
            }
            else if (inputManager["Move Right"].IsDown)
            {
                snV = 2;
            }

            if (inputManager["Move Up"].IsDown)
            {
                snVY = -2;
            }
            else if (inputManager["Move Down"].IsDown)
            {
                snVY = 2;
            }

            sn1.Position += new Vector2(snV, snVY);

            Vector2 accum = Vector2.Zero;
            // if there is a collision
            if (sn1.Rectangle.Intersects(sn2.Rectangle))
            {
                Rectangle xTest = new Rectangle(sn1.Rectangle.X, sn1.Rectangle.Y, sn1.Rectangle.Width, sn1.Rectangle.Height);
                Rectangle yTest = new Rectangle(sn1.Rectangle.X, sn1.Rectangle.Y, sn1.Rectangle.Width, sn1.Rectangle.Height);
                //check x
                if (Math.Abs(snV) > 0)
                {
                    // while it is still intersecting
                    while (xTest.Intersects(sn2.Rectangle))
                    {
                        // move it away 1 at a time until it doesn't touch
                        xTest.X -= Math.Sign(snV);

                        // accumulate total x movement needed
                        accum.X -= Math.Sign(snV);
                    }

                }
                //check y
                if (Math.Abs(snVY) > 0)
                {
                    while (yTest.Intersects(sn2.Rectangle))
                    {
                        yTest.Y -= Math.Sign(snVY);
                        accum.Y -= Math.Sign(snVY);
                    }
                }
            }


            // if it intersects both x and y
            if (accum.X != 0 && accum.Y != 0)
            {
                // which is larger?
                if (Math.Abs(accum.X) > Math.Abs(accum.Y))
                {
                    // move to the smaller one (Y in this case)
                    sn1.Position += new Vector2(0, accum.Y);
                }
                else
                {
                    // move to the smaller one (X in this case)
                    sn1.Position += new Vector2(accum.X, 0);
                }
            }
            else
            {
                // if just one axis, just move
                sn1.Position += new Vector2(accum.X, accum.Y);
            }

            inputManager.Update();

            base.Update(gameTime);
        }
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            renderer.Start();

            foreach (var sn in world.GetAllNodes())
            {
                renderer.DrawSprite(sn);
            }

            /*foreach (var sn in sceneManager.GetNodeByName("Flag1"))
            {
                renderer.DrawSprite(sn);
            }*/

            renderer.End();

            base.Draw(gameTime);
        }
    }
}
