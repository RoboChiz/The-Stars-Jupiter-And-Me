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

        List<Player> players;
        List<Enemy> enemies;
        List<Moon> moons;
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
            players = new List<Player>();
            moons = new List<Moon>();

            cam = new Camera();
        }

        public void StartGame()
        {
            Moon nMoon = new Moon(Moon.moonTexture, new Vector2(0, 0), Moon.radius * 2, Moon.radius * 2);
            moons.Add(nMoon);

            Player player = new Player(Player.playerTexture, new Vector2(Moon.radius / 2, -(Moon.radius + 10)), 5, 5);
            players.Add(player);

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
                    enemies[i].Update(gameTime, moons[i]);
                }

                if(enemies.Count == 0) //At end of Wave
                {
                    EndWave();
                }

            }

            //Get Camera to follow Player
            Vector2 dir = Vector2.Zero;
            if((players[0].Position - cam.Pos).Length() > 0.5f)
                dir = Vector2.Normalize(players[0].Position - cam.Pos);

            cam.Pos = Vector2.Lerp(cam.Pos, players[0].GetCentre(), deltaTime * 2f);

            cam.Zoom = MathHelper.Lerp(cam.Zoom, 7f,deltaTime);

            for (int i = 0; i < players.Count; i++)
            {
                Vector2 Gravity = Vector2.Normalize(moons[0].GetCentre() - players[i].GetCentre());
               // players[i].FaceDirection(-Gravity);

                players[i].Update(deltaTime,moons[0]);

                //Get Player Inputs

                if (/*GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X > 0.15 ||*/ Keyboard.GetState().IsKeyDown(Keys.A) == true || Keyboard.GetState().IsKeyDown(Keys.Left) == true)
                { //Thumb stick directed right
                    players[i].angleRadians -= deltaTime;
                }
                if (/*GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X < 0.15 ||*/ Keyboard.GetState().IsKeyDown(Keys.D) == true || Keyboard.GetState().IsKeyDown(Keys.Right) == true)
                { //Thumb stick directed left
                    players[i].angleRadians += deltaTime;
                }

                players[i].SlipangleRadians = MathHelper.Lerp(players[i].angleRadians, players[i].SlipangleRadians, deltaTime);
            }

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

            for(int i = 0; i < moons.Count; i++)
            {
                moons[i].Draw(spriteBatch);
            }

            for (int i = 0; i < enemies.Count; i++)
            {
                enemies[i].Draw(spriteBatch);
            }

            for (int i = 0; i < players.Count; i++)
            {
                players[i].Draw(spriteBatch);
            }

            spriteBatch.End();
        }


    }

    class Player : Sprite
    {
        
        public float currentOrbitHeight = 50;
        public float angleRadians;
        public float SlipangleRadians;

        float jumpHeight;
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
        }

        public void Update(float deltaTime, Moon moon)
        {
            //currentOrbitHeight -= 9.81f * deltaTime;

            if (currentOrbitHeight < 50)
                currentOrbitHeight = 50;

            float x = moon.GetCentre().X + (currentOrbitHeight * (float)Math.Cos(SlipangleRadians));
            float y = moon.GetCentre().Y + (currentOrbitHeight * (float)Math.Sin(SlipangleRadians));

            for (int i = 0; i < 4; i++)
            {

            }

                Position = new Vector2(x, y);
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
