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
        }

        public void StartGame()
        {
            Moon nMoon = new Moon(Moon.moonTexture, new Vector2(-Moon.radius, -Moon.radius), Moon.radius * 2, Moon.radius * 2, 0, 0);
            moons.Add(nMoon);

            Player player = new Player(Player.playerTexture, new Vector2(0, Moon.radius + 5), 5, 5, 70, 25);
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

            physics.Step(gameTime);

        }

        public void Draw(SpriteBatch spriteBatch)
        {
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
        }

    }

    class Player : Physics.RigidBody
    {
        float jumpHeight;
        float fireRate;

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


    }
}
