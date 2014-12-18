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
using WhiteSpace.Components.Animation;
using WhiteSpace.Network;
using WhiteSpace.Components;
using WhiteSpace.Components.Physics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;


#endregion

namespace WhiteSpace
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        public static GraphicsDeviceManager graphics;
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
            KeyboardInput.updateKeyStates();
            KeyboardInput.start();
            //new GameObject<gamestate>(gamestate.gameover);
            //StateMachine<gamestate>.getInstance().changeState(gamestate.lobby);
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// 
        /// 
        /// 

        Transform t;
        Transform tt;
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            ContentLoader.ContentManager = Content;
            StateMachine<gamestate>.getInstance().changeState(gamestate.main);

            TestRotationGameObject<gamestate> testt = new TestRotationGameObject<gamestate>(gamestate.main, Transform.createTransformWithSizeOnPosition(new Vector2(0,0), new Vector2(200, 200)));
            //TestRotationGameObjectType2<gamestate> tes = new TestRotationGameObjectType2<gamestate>(gamestate.main, Transform.createTransformWithSizeOnPosition(new Vector2(500, 0), new Vector2(200, 200)));
            t = Transform.createTransformWithSizeOnPosition(new Vector2(0, 400), new Vector2(800, 100));
            tt = Transform.createTransformWithSizeOnPosition(new Vector2(200, 500), new Vector2(800, 100));
            BoxCollider<gamestate> collider = new BoxCollider<gamestate>(t);

            t.Rotation = MathHelper.ToRadians(10);

            //BoxCollider<gamestate> collider1 = new BoxCollider<gamestate>(Transform.createTransformWithSizeOnPosition(new Vector2(0, 0), new Vector2(25, 500)));
            //BoxCollider<gamestate> collider2 = new BoxCollider<gamestate>(Transform.createTransformWithSizeOnPosition(new Vector2(770, 0), new Vector2(25, 500)));
            //BoxCollider<gamestate> collider3 = new BoxCollider<gamestate>(Transform.createTransformWithSizeOnPosition(new Vector2(0, 0), new Vector2(800, 25)));
            //collider.body.BodyType = FarseerPhysics.Dynamics.BodyType.Kinematic;

            //collider3.body.BodyType = FarseerPhysics.Dynamics.BodyType.Static;
            //collider2.body.BodyType = FarseerPhysics.Dynamics.BodyType.Static;

            TextureRegion<gamestate> region = new TextureRegion<gamestate>(t, ContentLoader.getContent<Texture2D>("Knight"));
            TextureRegion<gamestate> region2 = new TextureRegion<gamestate>(tt, ContentLoader.getContent<Texture2D>("Knight"));


            EditableText<gamestate> editor = new EditableText<gamestate>(Transform.createTransformWithSizeOnPosition(new Vector2(0,0), new Vector2(100,100)));

            //KeyboardInput.start();
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
        /// 
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            //t.Rotation += MathHelper.ToRadians(5.0f);
            //tt.Rotation += MathHelper.ToRadians(-10.0f);

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
