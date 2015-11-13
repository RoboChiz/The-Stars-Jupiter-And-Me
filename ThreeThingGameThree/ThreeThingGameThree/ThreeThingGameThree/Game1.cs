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
using RobsSprite;
using RobsPhysics;

namespace ThreeThingGameThree
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        static GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        int scrW = graphics.PreferredBackBufferWidth; //TODO - set to screen resolution
        int scrH = graphics.PreferredBackBufferHeight;

        //Saved Sprite Variables
        private Texture2D testTexture;
        private Physics physics;

        enum gameState{menu, inGame, gameOver};
        gameState gameStateNow = gameState.menu;

        private Physics.RigidBody testRbody;
        private Physics.RigidBody testGround;

        private Sprite Title, MenuPlanet, Option_Play, Option_Options, Background, Foreground; //Menu textures
        private Texture2D blankSprite;

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
            physics = new Physics();
            
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
            //Menu assets - start
            blankSprite = Content.Load<Texture2D>("BlankSprite");
            //----
            Title = new Sprite(blankSprite, new Vector2(scrW/13, scrH/22), scrW - 2*(scrW/13), scrH - scrH/10);
            MenuPlanet = new Sprite(blankSprite, new Vector2(scrW / 2 - scrW / 6, scrH), scrH / 4, scrH / 4);
            Option_Play = new Sprite(blankSprite, new Vector2(0, 0), 30, 30);
            Option_Options = new Sprite(blankSprite, new Vector2(0, 0), 30, 30);
            Background = new Sprite(blankSprite, new Vector2(0, 0), 30, 30);
            Foreground = new Sprite(blankSprite, new Vector2(0, 0), 30, 30);
            //Menu assets - end


            testTexture = Content.Load<Texture2D>("OrangeBall");
            testRbody = new Physics.RigidBody(testTexture, new Vector2(0, 0), 30, 30, 5, 140);
            testGround = new Physics.RigidBody(testTexture, new Vector2(0, 150), 30, 30, 0, 140);
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
            switch (gameStateNow)
            {
                case gameState.menu: //Controls while in menu
                    if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X > 0.1)
                    { //Thumb stick directed right

                    }
                    if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X < 0.1)
                    { //Thumb stick directed left
                        
                    }
                    break;
                    
                case gameState.inGame: //Controls while in game
                   
                    break;

                case gameState.gameOver: //Controls while in gameOver
                    break;
            }

            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed) {
                
                this.Exit();
            }
                

            
            testRbody.AddForce(new Vector2(0, 9.81f));
            physics.Step(gameTime);

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
            switch (gameStateNow)
            {
                case gameState.menu: //Draw while in menu
                    Title.Draw(spriteBatch);
                    MenuPlanet.Draw(spriteBatch);
                    break;
                case gameState.inGame: //Draw while in game
                    break;
                case gameState.gameOver: //Draw while in gameOver
                    break;
            }
            testRbody.Draw(spriteBatch);
            testGround.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
