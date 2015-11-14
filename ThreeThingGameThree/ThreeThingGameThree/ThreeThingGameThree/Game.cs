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
            player = new NewPlayer(NewPlayer.playerTexture, new Vector2(0, 0), 25, 25);
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

            player.Update(deltaTime,moon,cam);

            //Get Camera to follow Player
            cam.Zoom = MathHelper.Lerp(cam.Zoom, 1f,deltaTime);

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

            moon.DrawNoRot(spriteBatch);

            spriteBatch.End();
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
            distanceFromMoon = Moon.radius + (Width*0.5f);
        }

        public void Update(float deltaTime,Moon moon, Camera cam)
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

            Position = new Vector2(x, y);

            float camHeight = 50;
            float camX = moon.Position.X + ((distanceFromMoon + camHeight) * (float)Math.Cos(angleOnMoon));
            float camY = moon.Position.Y + ((distanceFromMoon + camHeight) * (float)Math.Sin(angleOnMoon));

            cam.Pos = Vector2.Lerp(cam.Pos, new Vector2(camX, camY), deltaTime * 2f);
           
            Vector2 lookDir = new Vector2((float)Math.Cos(angleOnMoon), -(float)Math.Sin(angleOnMoon));
            FaceDirection(lookDir);


        }
    }

    class Moon : Sprite
    {

        public Moon(Texture2D textureVal, Vector2 pos, int widthVal, int heightVal)
                : base(textureVal, pos, widthVal, heightVal){}

        public static Texture2D moonTexture;
        public static int radius = 250;
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
            _zoom = 0.0f;
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
