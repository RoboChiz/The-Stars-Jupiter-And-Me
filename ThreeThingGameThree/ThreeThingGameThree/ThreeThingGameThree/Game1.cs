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

        public static Boolean musicOn = true;
        public static Boolean sfxOn = true;

        private float theta = 0;

        static public int scrW = 800; //TODO - set to screen resolution
        static public int scrH = 600;

        //Saved Sprite Variables
        private Texture2D testTexture;   
        private static Color selectColour = Color.White * 0.5f;
        private static Color selectColourO = Color.White * 0.5f;
        Camera cam;

        private Boolean selectPlay = true;
        private Boolean pressed = false;
        private int selector = 0;
        public enum gameState{menu, options, inGame, gameOver, paused};
        public gameState gameStateNow = gameState.menu;

        private Sprite Title, MenuPlanet, Option_Play, Option_Options, Background, Foreground, Omoon, satellite; //Menu sprites
        private Texture2D blankSprite, testBall, testSBall, jupiter, player, heart, noHeart, fuelbackground, fuelBar, backgroundTexture,
            gun,cursorTexture,bulletTexture, tOptionIcon, tOptions, tPlay, tTitle, enemy, startWave, endWave;
        public static Texture2D storeTexture;
        public static SpriteFont font;

        private Sprite music, sfx, back, mOn, sOn; //Options sprites
        private Texture2D tMusic, tSfx, tBack, tSound;
        public static Song actionMusic, inbetweenWaveMusic;
        public static SoundEffect bulletSound, pop, blast;       

        private Texture2D tMoon1, tMoon2, tMoon3, tSatellite;

        private GameClass currentGame;

        private float deltaX = 0;
        private float deltaY = 0;
        public Game1()
        {
            cam = new Camera();
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
            backgroundTexture = Content.Load<Texture2D>("Background");
            blankSprite = Content.Load<Texture2D>("BlankSprite");
            jupiter = Content.Load<Texture2D>("Jupiter");
            tTitle = Content.Load<Texture2D>("Title2");
            testBall = Content.Load<Texture2D>("testBall");
            testSBall = Content.Load<Texture2D>("testSBall");
            tOptions = Content.Load<Texture2D>("moon_3");
            tOptionIcon = Content.Load<Texture2D>("Options");
            tPlay = Content.Load<Texture2D>("Play");
            tMusic = Content.Load<Texture2D>("Music");
            tSfx = Content.Load<Texture2D>("SFX");
            tBack = Content.Load<Texture2D>("Back");
            tMoon1 = Content.Load<Texture2D>("moon_1");
            tMoon2 = Content.Load<Texture2D>("moon_2");
            tMoon3 = Content.Load<Texture2D>("moon_3");
            tSatellite = Content.Load<Texture2D>("Satellite");
            tSound = Content.Load<Texture2D>("Sound_on");
            storeTexture = Content.Load<Texture2D>("store");

            actionMusic = Content.Load<Song>("MidWave");
            inbetweenWaveMusic = Content.Load<Song>("Betweenwaves");
            bulletSound = Content.Load<SoundEffect>("Blaster_fire");
            blast = Content.Load<SoundEffect>("Blast");
            pop = Content.Load<SoundEffect>("Pop");

            MediaPlayer.Play(inbetweenWaveMusic);

            //----
            Title = new Sprite(tTitle, new Vector2(scrW / 12, scrH / 24), scrW - 2 * (scrW / 12), (scrW - 2 * (scrW / 12)) / 3);
            MenuPlanet = new Sprite(jupiter, new Vector2(scrW / 2 - (scrW / 5), scrH / 4), scrH / 2, scrH / 2);
            Option_Play = new Sprite(tPlay, new Vector2(scrW / 6 - scrW / 16, 3 * scrH / 5), scrW / 8, scrW / 8);
            Option_Options = new Sprite(tOptionIcon, new Vector2(scrW / 2 + scrW / 4, 3 * scrH / 7), scrW / 8, scrW / 8);
            Omoon = new Sprite(tOptions, new Vector2((scrW / 4), scrH / 32), scrH - scrH / 4, scrH - scrH / 4);

            music = new Sprite(tMusic, new Vector2(scrW / 2 - 2 * (scrW / 15), scrH / 32 + scrH / 6), scrW / 3, scrH / 12);
            sfx = new Sprite(tSfx, new Vector2(scrW / 2 - 2 * (scrW / 15), scrH / 32 + scrH / 3), scrW / 3, scrH / 12);
            back = new Sprite(tBack, new Vector2(scrW / 2 - scrW / 11, scrH / 32 + (3 * scrH / 6)), scrW / 4, scrH / 12);
            mOn = new Sprite(tSound, new Vector2(scrW / 2 - 2 * (scrW / 15) + (scrW / 3), (scrH / 32 + scrH / 6) + 5), 19, 47);
            sOn = new Sprite(tSound, new Vector2(scrW / 2 - 2 * (scrW / 15) + (scrW / 3), (scrH / 32 + scrH / 3) + 5), 19, 47);

            satellite = new Sprite(tSatellite, new Vector2(0,0),50,50);

            Background = new Sprite(backgroundTexture, new Vector2(0, 0), scrW, scrH);
            //Foreground = new Sprite(blankSprite, new Vector2(0, 0), 30, 30);

            //Menu assets - end
            font = Content.Load<SpriteFont>("Courier New");
            
            //Game Assets
            player = Content.Load<Texture2D>("player");
            heart = Content.Load<Texture2D>("heart");
            noHeart = Content.Load<Texture2D>("heart_Empty");
            fuelbackground = Content.Load<Texture2D>("fuelbackground");
            fuelBar = Content.Load<Texture2D>("fuelBar");
            gun = Content.Load<Texture2D>("Gun");
            cursorTexture = Content.Load<Texture2D>("pointer");
            bulletTexture = Content.Load<Texture2D>("fire");
            enemy = Content.Load<Texture2D>("monster");
            startWave = Content.Load<Texture2D>("Wave_start");
            endWave = Content.Load<Texture2D>("Wave_complete");

            Moon.moonTexture = tMoon1;
            NewPlayer.playerTexture = player;
            NewPlayer.heartTexture = heart;
            NewPlayer.noHeartTexture = noHeart;
            NewPlayer.fuelBackgroundTexture = fuelbackground;
            NewPlayer.fuelBarTexture = fuelBar;
            NewPlayer.gunTexture = gun;
            NewPlayer.cursorTexture = cursorTexture;
            NewPlayer.bulletTexture = bulletTexture;

            Enemy.enemyTexture = enemy;
            GameClass.startWaveTexture = startWave;
            GameClass.endWaveTexture = endWave;

            music.Colour = Color.Red;
            sfx.Colour = Color.Red;
            back.Colour = Color.Red;
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
                            currentGame = new GameClass(5);//5 = number of enemies to spawn
                            currentGame.StartGame();
                        }
                        else {
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
                        if (selector == 0)
                        { //music on/off
                            if (musicOn)
                            {
                                musicOn = false;
                                MediaPlayer.Stop();
                            } else {
                                musicOn = true;
                                MediaPlayer.Play(inbetweenWaveMusic);
                            }

                        }
                        if (selector == 1) //sfx on/off
                        {
                            if (sfxOn) {
                                sfxOn = false;
                            } else {
                                sfxOn = true;
                            }
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
                        currentGame.Update(gameTime,graphics.GraphicsDevice);

                    if (currentGame.gameState == GameClass.GameState.Finished)
                    {
                        gameStateNow = gameState.menu;
                        MediaPlayer.Play(inbetweenWaveMusic);
                    }
                    break;

                case gameState.paused: //When the game is paused
                    if (/*GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X < 0.15 ||*/!pressed && (Keyboard.GetState().IsKeyDown(Keys.Space) == true || Keyboard.GetState().IsKeyDown(Keys.Enter) == true))
                    { //Select button pressed
                        gameStateNow = gameState.inGame;
                    }
                    break;

                case gameState.gameOver: //Controls while in gameOver
                    break;


            }

            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed) {
                
                this.Exit();
            }
            theta += 0.5f;
            while (theta > 360) {
                theta -= 360;
            }
            orbitPlanets();
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

            Background.DrawNoRotCentre(spriteBatch);

            switch (gameStateNow)
            {
                case gameState.menu: //Draw while in menu                    
                    if (!selectPlay)
                    {
                        Option_Play.Colour = selectColour;
                        Option_Options.Colour = Color.White;
                    }
                    else {
                        Option_Play.Colour = Color.White;
                        Option_Options.Colour = selectColour;
                    }
                    MenuPlanet.DrawNoRotCentre(spriteBatch);
                    Option_Play.DrawNoRotCentre(spriteBatch);
                    Option_Options.DrawNoRotCentre(spriteBatch);
                    satellite.Draw(spriteBatch);
                    Title.DrawNoRotCentre(spriteBatch);
                    
                    break;

                case gameState.options: //Draw while in options menu
                    Omoon.DrawNoRotCentre(spriteBatch);
                    switch (selector) {
                        case 0:
                            music.Colour = selectColourO;
                            mOn.Colour = selectColourO;
                            sOn.Colour = Color.Red;
                            sfx.Colour = Color.Red;
                            back.Colour = Color.Red;
                            break;
                        case 1:
                            music.Colour = Color.Red;
                            mOn.Colour = Color.Red;
                            sOn.Colour = selectColourO;
                            sfx.Colour = selectColourO;
                            back.Colour = Color.Red;
                            break;
                        case 2:
                            music.Colour = Color.Red;
                            mOn.Colour = Color.Red;
                            sOn.Colour = Color.Red;
                            sfx.Colour = Color.Red;
                            back.Colour = selectColourO;
                            break;
                    }
                    if (musicOn) {
                        mOn.DrawNoRotCentre(spriteBatch);                     
                    }
                    if (sfxOn)
                    {                        
                        sOn.DrawNoRotCentre(spriteBatch);
                    }
                    music.DrawNoRotCentre(spriteBatch);
                    sfx.DrawNoRotCentre(spriteBatch);
                    back.DrawNoRotCentre(spriteBatch);
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
            Double angle = theta * (Math.PI / 180);
            int size = 0;
            satellite.Position = MenuPlanet.Position + (new Vector2((float)((350) * Math.Cos(angle)), (float)((140) * Math.Sin(angle))));
            if (theta > 0 && theta < 150 || theta > 310 && theta < 360) 
                size = (int)(50 * (Math.Sin(angle + 70)));
                else
                size = 0;
            satellite.Width = size;
            satellite.Height= size;
        }
    }
}
