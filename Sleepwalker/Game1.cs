using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SleepwalkerEngine;
using System.Collections.Generic;
using System;

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
        CollisionResolver cr;
        Quadtree quadTree;
        Camera2D camera;

        Player player;

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

            player = new Player()
            {
                Name = "Flag1",
                Position = new Vector2(300, 300),
                Sprite = Content.Load<Texture2D>("flag")
            };

            SceneNode sn2 = new SceneNode()
            {
                Name = "Flag2",
                Position = new Vector2(200, 300),
                Sprite = Content.Load<Texture2D>("flag")
            };

            SceneNode sn3 = new SceneNode()
            {
                Name = "Flag3",
                Position = new Vector2(232, 300),
                Sprite = Content.Load<Texture2D>("flag")
            };


            SceneNode sn4 = new SceneNode()
            {
                Name = "Flag4",
                Position = new Vector2(232, 332),
                Sprite = Content.Load<Texture2D>("flag")
            };

            world.Add(player);
            world.Add(sn2);
            world.Add(sn3);
            world.Add(sn4);

            for (int i = 0; i < 10; i++)
            {
                world.Add(new SceneNode
                {
                    Name = "Flag4",
                    Position = new Vector2(232 + i * 32, 332),
                    Sprite = Content.Load<Texture2D>("flag")
                });
            }

            for (int i = 0; i < 10; i++)
            {
                world.Add(new SceneNode
                {
                    Name = "FlagY" + i.ToString(),
                    Position = new Vector2(232 + 320, 332 - i * 32),
                    Sprite = Content.Load<Texture2D>("flag")
                });
            }

            List<SceneNode> sns = new List<SceneNode>(world.GetAllNodes());
            sns.Remove(player);

            cr = new CollisionResolver(sns);

            inputManager = new InputManager();

            quadTree = new Quadtree(0, new Rectangle(0, 0,
                GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height));

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

            camera = new Camera2D(GraphicsDevice.Viewport);

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


            player.Velocity = Vector2.Zero;
            if (inputManager["Move Left"].IsDown)
            {
                player.Velocity.X = -player.MoveSpeed * gameTime.ElapsedGameTime.Milliseconds;
            }
            else if (inputManager["Move Right"].IsDown)
            {
                player.Velocity.X = player.MoveSpeed * gameTime.ElapsedGameTime.Milliseconds;
            }

            if (inputManager["Move Up"].IsDown)
            {
                player.Velocity.Y = -player.MoveSpeed * gameTime.ElapsedGameTime.Milliseconds;
            }

            if (inputManager["Move Down"].IsDown)
            {
                player.Velocity.Y = player.MoveSpeed * gameTime.ElapsedGameTime.Milliseconds;
            }

            if (player.Velocity.X != 0 && player.Velocity.Y != 0)
            {
                // Divide by sqrt(2)
                player.Velocity.X /= 1.414f;
                player.Velocity.Y /= 1.414f;
            }
            player.Position += player.Velocity;

            quadTree.Clear();
            for (int i = 0; i < cr.Colliders.Count; i++)
            {
                quadTree.Insert(cr.Colliders[i].Rectangle);
            }

            List<Rectangle> nodesToCheck = new List<Rectangle>();
            for (int i = 0; i < ((List<SceneNode>)world.GetAllNodes()).Count; i++)
            {
                nodesToCheck.Clear();
                quadTree.Retrieve(ref nodesToCheck, player.Rectangle);

                for (int j = 0; j < nodesToCheck.Count; j++)
                {
                    player.Position += cr.ResolveCollisions(player.Velocity, player.Rectangle, nodesToCheck[j]);
                }
            }

            // Fixes 'caught in seams' bug when pressing up and left but only moving left
            cr.Colliders.Reverse();

            inputManager.Update();
            camera.Update(gameTime, new Vector2(player.Rectangle.X + (player.Rectangle.Width / 2),
                player.Rectangle.Y + (player.Rectangle.Height / 2)));

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            renderer.Start(camera);

            foreach (var sn in world.GetAllNodes())
            {
                // Quick check to determine if the node is within the screen at all
                if (!(sn.Rectangle.X + sn.Rectangle.Width < player.Rectangle.X - GraphicsDevice.Viewport.Width / 2 ||
                    sn.Rectangle.X > player.Rectangle.X + GraphicsDevice.Viewport.Width / 2 ||
                    sn.Rectangle.Y > player.Rectangle.Y + GraphicsDevice.Viewport.Width / 2 ||
                    sn.Rectangle.Y + sn.Rectangle.Height < player.Rectangle.Y - GraphicsDevice.Viewport.Width / 2))
                {
                    renderer.DrawSprite(sn);
                }
            }

            renderer.End();

            base.Draw(gameTime);
        }
    }
}
