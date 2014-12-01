﻿#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
using WhiteSpace.GameObjects;
using WhiteSpace.GameLoop;
using WhiteSpace.Temp;
using WhiteSpace.Drawables;

#endregion

namespace WhiteSpace
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public Game1()
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
            // TODO: Add your initialization logic here
            base.Initialize();
            IsMouseVisible = true;
            //new GameObject<gamestate>(gamestate.gameover);
            //StateMachine<gamestate>.getInstance().changeState(gamestate.lobby);
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //Unit<gamestate> unit = new Unit<gamestate>(gamestate.game, Temp.Transform.createTransformWithSizeOnPosition(new Vector2(200,200), new Vector2(100, 100)), Content.Load<Texture2D>("Knight"));
            //new Unit<gamestate>(Temp.Transform.createTransformWithSize(new Vector2(50, 50)), Content.Load<Texture2D>("Knight"));
            //new Clickable<gamestate>(Transform.createTransformWithSize(new Vector2(100, 100)));


            SpriteSheet sheet = new SpriteSheet(Content.Load<Texture2D>("RunningBastard"), 6, 5);
            Unit<gamestate> u = new Unit<gamestate>(Temp.Transform.createTransformWithSize(new Vector2(150, 150)), sheet);
            StateMachine<gamestate>.getInstance().changeState(gamestate.main);
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            //if(Keyboard.GetState().IsKeyDown(Keys.A))
            //{
            //    StateMachine<gamestate>.getInstance().changeState(gamestate.gameover);
            //}

            //else
            //{
            //    StateMachine<gamestate>.getInstance().changeState(gamestate.game);
            //}
            // TODO: Add your update logic here
            UpdateExecuter.executeUpdates(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            DrawExecuter.executeRegisteredDrawMethods(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
