// author peng cheng

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace WindowsGame1
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        GraphicsDevice device;
        BasicEffect effect;
        public float aspectRatio;
        SpriteBatch spriteBatch;
        KeyboardState oldState;
        MouseState originalMouseState;


        Character model_character;
        Character[] characterList = new Character[10];
        Character[] treeList = new Character[50];
        Character[] allanTreeList = new Character[50];


        Camera camera;
        Terrain terrain;
        People people;

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
            oldState = Keyboard.GetState();
            originalMouseState = new MouseState();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            device = GraphicsDevice;
            effect = new BasicEffect(this.GraphicsDevice);
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            aspectRatio = graphics.GraphicsDevice.Viewport.AspectRatio;
            // TODO: use this.Content to load your game content here
            camera = new Camera(this);

            // panda
            model_character = new Character(camera,this, new Vector3(0.0f, 0.0f, 0.0f),new Vector3(0.0f, 0.0f, 0.0f), "Models\\poO");

            //Cizhen
            Random rnd = new Random();
            Character c1 = new Character(camera, this, new Vector3(0.1f, 0.0f, 0.1f), Vector3.Zero, "Models\\Monster2\\monster1_modify");
            for (int i = 0; i < 10; i++)
            {
                float a = (float)rnd.NextDouble() * 128.0f;
                float b = (float)rnd.NextDouble() * 128.0f;
                Character c = new Character(camera, this, new Vector3(a, 0.0f, -b), Vector3.Zero, "Models\\Monster2\\monster1_modify");
                characterList[i] = c;
            }

            for (int i = 0; i < 50; i++)
            {
                float a = (float)rnd.NextDouble() * 128.0f;
                float b = (float)rnd.NextDouble() * 128.0f;
                Character tree = new Character(camera, this, new Vector3(a, 0.0f, -b), Vector3.Zero, "Models\\Tree\\tree1");
                treeList[i] = tree;

                a = (float)rnd.NextDouble() * 128.0f;
                b = (float)rnd.NextDouble() * 128.0f;
                Character alanTree = new Character(camera, this, new Vector3(i * 10.0f, 0.0f, -b), Vector3.Zero, "Models\\Tree\\AlanTree1");
                allanTreeList[i] = alanTree;
            }

            //Cizhen

            terrain = new Terrain(this, camera, device, effect);

            // people
            people = new People(camera, this, new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0.0f, 0.0f, 0.0f));

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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
            model_character.update(gameTime);
            camera.update(gameTime, people, this);

            
            processInput(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            terrain.Draw(camera);
            //model_character.Draw();
            // people.Draw();
            //people.update(gameTime);
            //Cizhen
            model_character.Draw();
            for (int i = 0; i < characterList.Length; i++)
            {
                if (characterList[i].isActive)
                    characterList[i].Draw();
            }

            if (people.isActive)
            {
                people.Draw();
                people.update(gameTime);
            }

            for (int i = 0; i < treeList.Length; i++)
            {
                treeList[i].Draw();
                allanTreeList[i].Draw();
            }
            //Cizhen
            base.Draw(gameTime);
        }

        private void processInput(GameTime gameTime)
        {
            KeyboardState newState = Keyboard.GetState();
            MouseState newMouseState = Mouse.GetState();
            // keyboard control
            KeyboardState keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.Escape)) this.Exit();
            if (keyboardState.IsKeyDown(Keys.A))
            {
                //  model_character.modelPosition.X -= 0.1f;
                Matrix rot = Matrix.CreateRotationX(people.modelRotation.X) * Matrix.CreateRotationY(people.modelRotation.Y
                        ) * Matrix.CreateRotationZ(people.modelRotation.Z);
                people.modelPosition += rot.Left / 100;
                people.update(gameTime);
            }
            if (keyboardState.IsKeyDown(Keys.D))
            {
                //  model_character.modelPosition.X += 0.1f;
                Matrix rot = Matrix.CreateRotationX(people.modelRotation.X) * Matrix.CreateRotationY(people.modelRotation.Y
                        ) * Matrix.CreateRotationZ(people.modelRotation.Z);
                people.modelPosition += rot.Right/100;
                people.update(gameTime);
            }
            if (keyboardState.IsKeyDown(Keys.Left))
            {
                // model_character.modelPosition.Y += 0.1f;
                people.modelRotation += new Vector3(0.0f, 0.01f, 0.0f);
                camera.rotationz += 0.01f;
            }
            if (keyboardState.IsKeyDown(Keys.Right))
            {
                //  model_character.modelPosition.Y -= 0.1f;
                people.modelRotation += new Vector3(0.0f, -0.01f, 0.0f);
                camera.rotationz -= 0.01f;
            }
            if (keyboardState.IsKeyDown(Keys.S))
            {
              //  model_character.modelPosition.Z += 0.1f;
                people.modelPosition.Z += 0.01f;
                Matrix rot = Matrix.CreateRotationX(people.modelRotation.X) * Matrix.CreateRotationY(people.modelRotation.Y
                        ) * Matrix.CreateRotationZ(people.modelRotation.Z);
                people.modelPosition += rot.Backward / 100;
                people.update(gameTime);
            }
            if (keyboardState.IsKeyDown(Keys.W))
            {
                model_character.modelPosition.Z -= 0.1f;
                //  people.modelPosition.Z -= 0.01f;
                Matrix rot = Matrix.CreateRotationX(people.modelRotation.X) * Matrix.CreateRotationY(people.modelRotation.Y
                          ) * Matrix.CreateRotationZ(people.modelRotation.Z);
                people.modelPosition += rot.Forward / 100;
                people.update(gameTime);
            }
            
            /*            if (keyboardState.IsKeyDown(Keys.A))
                        {
                            model_character.modelRotation.Z += 0.05f;

                        }
                        if (keyboardState.IsKeyDown(Keys.D))
                        {
                            model_character.modelRotation.Z -= 0.05f;
                        }
                        if (keyboardState.IsKeyDown(Keys.W))
                        {
                            model_character.modelRotation.Y += 0.05f;
                        }
                        if (keyboardState.IsKeyDown(Keys.S))
                        {
                            model_character.modelRotation.Y -= 0.05f;
                        }
                        if (keyboardState.IsKeyDown(Keys.Y))
                        {
                            LoadContent();
                        }*/
            oldState = newState;

        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>

    }
}
