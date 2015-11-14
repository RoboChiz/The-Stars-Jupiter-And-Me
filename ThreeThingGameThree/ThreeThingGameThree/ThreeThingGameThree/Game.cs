using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using RobsSprite;

namespace ThreeThingGameThree
{
    class GameClass
    {

        NewPlayer player;
        List<Enemy> enemies;
        Moon moon;

        Camera cam;

        enum GameState
        {
            Attack,
            Rest
        }
        GameState gameState = GameState.Attack;

        public GameClass(float enemyNum)
        {
            enemies = new List<Enemy>();            
            cam = new Camera();
        }

        public void StartGame()
        {
            moon = new Moon(Moon.moonTexture, new Vector2(0, 0), Moon.radius * 2, Moon.radius * 2);
            player = new NewPlayer(NewPlayer.playerTexture, new Vector2(0, 0), 5, 5);
        }

        public void StartWave()
        {
            gameState = GameState.Attack;
        }

        public void EndWave()
        {
            gameState = GameState.Rest;
        }

        public void Update(GameTime gameTime)
        {         
            float deltaTime = (gameTime.ElapsedGameTime.Milliseconds / 1000f);       

            if(gameState == GameState.Attack)
            {
               
                for (int i = 0; i < enemies.Count; i++)
                {
                    enemies[i].Update(gameTime, moon);
                }

                if(enemies.Count == 0) //At end of Wave
                {
                    EndWave();
                }

            }

            player.Update(deltaTime,moon);

            //Get Camera to follow Player
            cam.Pos = Vector2.Lerp(cam.Pos, moon.Position, deltaTime * 15f);
            cam.Zoom = MathHelper.Lerp(cam.Zoom, 4f,deltaTime);
           // cam.Rotation = player.angleOnMoon;


        }

        public void Draw(SpriteBatch spriteBatch, GraphicsDevice device)
        {
            
            spriteBatch.Begin(SpriteSortMode.BackToFront,
                        BlendState.AlphaBlend,
                        null,
                        null,
                        null,
                        null,
                        cam.get_transformation(device));

            for (int i = 0; i < enemies.Count; i++)
            {
                enemies[i].Draw(spriteBatch);
            }

            player.Draw(spriteBatch);

            moon.Draw(spriteBatch);

            spriteBatch.End();
        }


    }

    class Player : Sprite
    {
        
        public float currentOrbitHeight = 50;
        public float angleRadians;

        public float jumpMax;
        public float jumpCurrent;

        float fireRate;
        double money;

        public double Money
        {
            get { return money; }
            set { money = value; }
        }

        public static Texture2D playerTexture;

        public Player(Texture2D textureVal, Vector2 pos, int widthVal, int heightVal)
                : base(textureVal, pos, widthVal, heightVal)
        {
            currentOrbitHeight = 50;
            angleRadians = (float)(-Math.PI/2.0);

            jumpMax = 15;
            jumpCurrent = 0;
        }

        public void Update(float deltaTime, Moon moon)
        {
            currentOrbitHeight -= 9.81f * deltaTime;

            if (currentOrbitHeight < 53f)
            {
                currentOrbitHeight = 53f;
                jumpCurrent = jumpMax;
            }

            float x = moon.Position.X + (currentOrbitHeight * (float)Math.Cos(angleRadians));
            float y = moon.Position.Y + (currentOrbitHeight * (float)Math.Sin(angleRadians));

            Position = new Vector2(x, y);
        }
    }

    class NewPlayer : Sprite
    {

        public float angleOnMoon; //In Radians
        public float distanceFromMoon;

        public static Texture2D playerTexture;

        public NewPlayer(Texture2D textureVal, Vector2 pos, int widthVal, int heightVal)
                : base(textureVal, pos, widthVal, heightVal)
        {
            angleOnMoon = (float)(-Math.PI / 2.0);
            distanceFromMoon = 53;
        }

        public void Update(float deltaTime,Moon moon)
        {
            //Calculate Inputs
            if (/*GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X > 0.15 ||*/ Keyboard.GetState().IsKeyDown(Keys.A) == true || Keyboard.GetState().IsKeyDown(Keys.Left) == true)
            { //Thumb stick directed right
                angleOnMoon -= deltaTime;
            }
            if (/*GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X < 0.15 ||*/ Keyboard.GetState().IsKeyDown(Keys.D) == true || Keyboard.GetState().IsKeyDown(Keys.Right) == true)
            { //Thumb stick directed left
                angleOnMoon += deltaTime;
            }
            
            //Calculate Position
            float x = moon.Position.X + (distanceFromMoon * (float)Math.Cos(angleOnMoon));
            float y = moon.Position.Y + (distanceFromMoon * (float)Math.Sin(angleOnMoon));

            Vector2 Test = new Vector2(x, y);

            FaceDirection(moon.Position - Test);


        }
    }

    class Moon : Sprite
    {

        public Moon(Texture2D textureVal, Vector2 pos, int widthVal, int heightVal)
                : base(textureVal, pos, widthVal, heightVal){}

        public static Texture2D moonTexture;
        public static int radius = 50;
        float health;

        public float Health
        {
            get { return health; }
            set { health = value; }
        }


    }

    class Camera
    {
        protected float          _zoom; // Camera Zoom
        public Matrix             _transform; // Matrix Transform
        public Vector2          _pos; // Camera Position
        protected float         _rotation; // Camera Rotation

        public Camera()
        {
            _zoom = 1.0f;
            _rotation = 0.0f;
            _pos = Vector2.Zero;
        }
        // Sets and gets zoom
        public float Zoom
        {
            get { return _zoom; }
            set { _zoom = value; if (_zoom < 0.1f) _zoom = 0.1f; } // Negative zoom will flip image
        }

        public float Rotation
        {
            get { return _rotation; }
            set { _rotation = value; }
        }

        // Auxiliary function to move the camera
        public void Move(Vector2 amount)
        {
            _pos += amount;
        }
        // Get set position
        public Vector2 Pos
        {
            get { return _pos; }
            set { _pos = value; }
        }

        public Matrix get_transformation(GraphicsDevice graphicsDevice)
        {
            _transform =       // Thanks to o KB o for this solution
              Matrix.CreateTranslation(new Vector3(-_pos.X, -_pos.Y, 0)) *
                                         Matrix.CreateRotationZ(Rotation) *
                                         Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) *
                                         Matrix.CreateTranslation(new Vector3(graphicsDevice.Viewport.Width * 0.5f, graphicsDevice.Viewport.Height * 0.5f, 0));
            return _transform;
        }
    }

}
