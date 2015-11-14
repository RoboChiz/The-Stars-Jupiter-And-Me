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

        int scrW = 800; //TODO - set to screen resolution
        int scrH = 600;

        //Saved Sprite Variables
        private Texture2D testTexture;   
        private static Color selectColour = Color.Blue;

        private Boolean selectPlay = true;
        private Boolean pressed = false;
        private int selector = 0;
        enum gameState{menu, options, inGame, gameOver};
        gameState gameStateNow = gameState.menu;

        private Sprite Title, MenuPlanet, Option_Play, Option_Options, Background, Foreground, Omoon; //Menu sprites
        private Texture2D blankSprite, testBall, testSBall, jupiter, player;

        private Sprite music, sfx, back; //Options sprites

        private Game currentGame;

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
            jupiter = Content.Load<Texture2D>("Jupiter");
            testBall = Content.Load<Texture2D>("testBall");
            testSBall = Content.Load<Texture2D>("testSBall");
            //----
            Title = new Sprite(blankSprite, new Vector2(scrW / 12, scrH / 24), scrW - 2 * (scrW / 12), scrH / 6);
            MenuPlanet = new Sprite(jupiter, new Vector2(scrW / 2 - (scrW / 5), scrH / 4), scrH / 2, scrH / 2);
            Option_Play = new Sprite(testSBall, new Vector2(scrW / 6 - scrW / 16, 3 * scrH / 5), scrW / 8, scrW / 8);
            Option_Options = new Sprite(testSBall, new Vector2(scrW / 2 + scrW / 4, 3 * scrH / 7), scrW / 8, scrW / 8);
            Omoon = new Sprite(testSBall, new Vector2((scrW / 4), scrH / 32), scrH - scrH / 4, scrH - scrH / 4);

            music = new Sprite(blankSprite, new Vector2(scrW / 2 - 2*(scrW / 15), scrH / 32 + scrH / 6), scrW / 3, scrH / 12);
            sfx = new Sprite(blankSprite, new Vector2(scrW / 2 - 2*(scrW / 15) , scrH / 32 + scrH / 3), scrW / 3, scrH / 12);
            back = new Sprite(blankSprite, new Vector2(scrW / 2 - scrW / 11, scrH / 32 + (3 * scrH / 6)), scrW / 4, scrH / 12);

            //Background = new Sprite(blankSprite, new Vector2(0, 0), 30, 30);
            //Foreground = new Sprite(blankSprite, new Vector2(0, 0), 30, 30);
            //Menu assets - end


            //Game Assets
            player = Content.Load<Texture2D>("OrangeBall");
            Moon.moonTexture = jupiter;
            Player.playerTexture = player;
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
                    if (/*GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X > 0.15 ||*/ Keyboard.GetState().IsKeyDown(Keys.A) == true || Keyboard.GetState().IsKeyDown(Keys.Left) == true)
                    { //Thumb stick directed right
                        selectPlay = true;                        
                    }
                    if (/*GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X < 0.15 ||*/ Keyboard.GetState().IsKeyDown(Keys.D) == true || Keyboard.GetState().IsKeyDown(Keys.Right) == true)
                    { //Thumb stick directed left
                        selectPlay = false;
                    }
                    if (/*GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X < 0.15 ||*/ !pressed && (Keyboard.GetState().IsKeyDown(Keys.Space) == true || Keyboard.GetState().IsKeyDown(Keys.Enter) == true)) 
                    { //Select button pressed
                        if (selectPlay)
                        {
                            gameStateNow = gameState.inGame;
                            currentGame = new Game(5);//5 = number of enemies to spawn
                            currentGame.StartGame();
                        }
                        else {
                           //TODO - OPTIONS MENU
                            gameStateNow = gameState.options;
                            selector = 0;
                            }
                        pressed = true;
                    }
                    if (/*GamePad.GetState(PlayerIndex.One).Buttons.A ||*/ pressed && (Keyboard.GetState().IsKeyUp(Keys.Space) == true && Keyboard.GetState().IsKeyUp(Keys.Enter) == true))
                    { //Select button pressed                       
                        pressed = false;
                    }
                    break;

                case gameState.options: //Controls while in game
                    if (/*GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y > 0.15 ||*/ !pressed && (Keyboard.GetState().IsKeyDown(Keys.W) == true || Keyboard.GetState().IsKeyDown(Keys.Up) == true))
                    { //Thumb stick directed right                        
                        if (selector != 0)
                        {
                            selector -= 1;
                        }
                        pressed = true;
                    }
                    if (/*GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y < 0.15 ||*/ !pressed &&(Keyboard.GetState().IsKeyDown(Keys.S) == true || Keyboard.GetState().IsKeyDown(Keys.Down) == true))
                    { //Thumb stick directed left
                        if (selector != 2)
                        {
                            selector += 1;
                        }
                        pressed = true;
                    }
                    if (/*GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X < 0.15 ||*/!pressed && (Keyboard.GetState().IsKeyDown(Keys.Space) == true || Keyboard.GetState().IsKeyDown(Keys.Enter) == true)) 
                    { //Select button pressed
                        if (selector == 0) { //music on/off
                            
                        }
                        if (selector == 1) //sfx on/off
                        {
                        }
                        if (selector == 2) //return to menu
                        {
                            gameStateNow = gameState.menu;
                        }
                        pressed = true;
                    }
                    if (/*GamePad.GetState(PlayerIndex.One).Buttons.A ||*/ pressed && (Keyboard.GetState().IsKeyUp(Keys.Space) == true && Keyboard.GetState().IsKeyUp(Keys.Enter) == true && Keyboard.GetState().IsKeyUp(Keys.W) == true && Keyboard.GetState().IsKeyUp(Keys.Up) == true && Keyboard.GetState().IsKeyUp(Keys.S) == true && Keyboard.GetState().IsKeyUp(Keys.Down) == true))
                    { //Select button pressed                       
                        pressed = false;
                    }
                    break;
                    
                case gameState.inGame: //Controls while in game
                    if (currentGame != null)
                        currentGame.Update(gameTime);
                    break;

                case gameState.gameOver: //Controls while in gameOver
                    break;


            }

            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed) {
                
                this.Exit();
            }

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
                    if (selectPlay)
                    {
                        Option_Play.Colour = selectColour;
                        Option_Options.Colour = Color.White;
                    }
                    else {
                        Option_Play.Colour = Color.White;
                        Option_Options.Colour = selectColour;
                    }
                    MenuPlanet.Draw(spriteBatch);
                    Option_Play.Draw(spriteBatch);
                    Option_Options.Draw(spriteBatch);
                    break;

                case gameState.options: //Draw while in options menu
                    Omoon.Draw(spriteBatch);
                    switch (selector) {
                        case 0:
                            music.Colour = selectColour;
                            sfx.Colour = Color.White;
                            back.Colour = Color.White;
                            break;
                        case 1:
                            music.Colour = Color.White;
                            sfx.Colour = selectColour;
                            back.Colour = Color.White;
                            break;
                        case 2:
                            music.Colour = Color.White;
                            sfx.Colour = Color.White;
                            back.Colour = selectColour;
                            break;
                    }

                    music.Draw(spriteBatch);
                    sfx.Draw(spriteBatch);
                    back.Draw(spriteBatch);
                    break;
                case gameState.inGame: //Draw while in game
                    break;
                case gameState.gameOver: //Draw while in gameOver
                    break;
            }



            spriteBatch.End();

            if(gameStateNow == gameState.inGame && currentGame != null)
                currentGame.Draw(spriteBatch, graphics.GraphicsDevice);

            base.Draw(gameTime);
        }

        private void orbitPlanets()
        {
            Vector2 planetA = Option_Play.Position;
            Vector2 planetB = Option_Options.Position;

            Sprite sprite = Option_Play;

            double theta, phi;

            float ax = MenuPlanet.Position.X;
            float ay = MenuPlanet.Position.Y;
            float bx = sprite.Position.X;
            float by = sprite.Position.Y;

            double top = (double)(ax * bx) + (double)(ay * by);
            double bottom = Math.Pow(ax * ax + (ay * ay), bx * bx + (by * by));
            theta = Math.Acos(top / bottom);


        }
    }
}
