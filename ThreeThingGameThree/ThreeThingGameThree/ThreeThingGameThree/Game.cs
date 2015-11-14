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
using RobsPhysics;
using RobsSprite;

namespace ThreeThingGameThree
{
    class Game
    {

        Physics physics;

        List<Player> players;
        List<Enemy> enemies;
        List<Physics.RigidBody> moons;
        Camera cam;

        enum GameState
        {
            Attack,
            Rest
        }
        GameState gameState = GameState.Attack;

        public Game(float enemyNum)
        {
            enemies = new List<Enemy>();
            players = new List<Player>();
            moons = new List<Physics.RigidBody>();

            physics = new Physics();
            cam = new Camera();
        }

        public void StartGame()
        {
            Moon nMoon = new Moon(Moon.moonTexture, new Vector2(0,0), Moon.radius * 2, Moon.radius * 2, 0, 0);
            moons.Add(nMoon);

            Player player = new Player(Player.playerTexture, new Vector2(0, -(Moon.radius + 30)), 5, 5, 70, 25);
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
            if(gameState == GameState.Attack)
            {
               
                for (int i = 0; i < enemies.Count; i++)
                {
                    enemies[i].Update(gameTime,moons[i]);
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

            cam.Move(dir);
            cam.Zoom = MathHelper.Lerp(cam.Zoom, 7f, (gameTime.ElapsedGameTime.Milliseconds / 1000f));

            //Add Gravity
            for (int i = 0; i < players.Count; i++)
            {
                players[i].AddForce(Vector2.Normalize(moons[0].GetCentre() - players[i].GetCentre()) * 981f);             
            }


            physics.Step(gameTime);

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

    class Player : Physics.RigidBody
    {
        float jumpHeight;
        float fireRate;
        double money;

        public double Money
        {
            get { return money; }
            set { money = value; }
        }

        public static Texture2D playerTexture;

        public Player(Texture2D textureVal, Vector2 pos, int widthVal, int heightVal, float mass, float maxSpeed)
                : base(textureVal, pos, widthVal, heightVal,mass,maxSpeed){}
    }

    class Moon : Physics.RigidBody
    {

        public Moon(Texture2D textureVal, Vector2 pos, int widthVal, int heightVal, float mass, float maxSpeed)
                : base(textureVal, pos, widthVal, heightVal,mass,maxSpeed){}

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
