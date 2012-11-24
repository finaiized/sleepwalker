using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SleepwalkerEngine;
using System;
using System.Diagnostics;
using System.Collections.Generic;

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

        SceneNode sn1;
        SceneNode sn2;
        Vector2 prevPos;
        List<SceneNode> sns;

        List<Rectangle> debugDraw;
        Texture2D rectangleTexture;

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

            world.Add(sn1);
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

            prevPos = sn1.Position;
            cr = new CollisionResolver();
            sns = new List<SceneNode>(world.GetAllNodes());
            sns.Remove(sn1);

            cr.Colliders = sns;

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

            rectangleTexture = new Texture2D(GraphicsDevice, 1, 1);
            rectangleTexture.SetData(new Color[] { Color.White });

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

            quadTree.Clear();

            Vector2 velocity = Vector2.Zero;
            if (inputManager["Move Left"].IsDown)
            {
                velocity.X = -2;
            }
            else if (inputManager["Move Right"].IsDown)
            {
                velocity.X = 2;
            }

            if (inputManager["Move Up"].IsDown)
            {
                velocity.Y = -2;
            }
            else if (inputManager["Move Down"].IsDown)
            {
                velocity.Y = 2;
            }

            sn1.Position += velocity;

            for (int i = 0; i < cr.Colliders.Count; i++)
            {
                quadTree.Insert(cr.Colliders[i].Rectangle);
            }

            List<Rectangle> returnObjects = new List<Rectangle>();
            debugDraw = new List<Rectangle>();
            for (int i = 0; i < ((List<SceneNode>)world.GetAllNodes()).Count; i++)
            {
                returnObjects.Clear();
                quadTree.Retrieve(ref returnObjects, sn1.Rectangle);

                for (int j = 0; j < returnObjects.Count; j++)
                {
                    sn1.Position += cr.ResolveCollisions(velocity, sn1.Rectangle, returnObjects[j]);
                    debugDraw.Add(returnObjects[j]);
                }
            }

            // Fixes 'caught in seams' bug when pressing up and left but only moving left
            cr.Colliders.Reverse();

            inputManager.Update();
            camera.Update(gameTime, new Vector2(sn1.Rectangle.X + (sn1.Rectangle.Width / 2), 
                sn1.Rectangle.Y + (sn1.Rectangle.Height / 2)));

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
                renderer.DrawSprite(sn);
            }

            foreach (var rec in debugDraw)
            {
                spriteBatch.Draw(rectangleTexture, rec, Color.White);
            }
            renderer.End();

            base.Draw(gameTime);
        }
    }
}
