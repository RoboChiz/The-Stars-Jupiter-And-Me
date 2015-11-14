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

    class EnemySpawner
    {
        List<Enemy> Enemies = new List<Enemy>();
        Random random = new Random();

        public List<Enemy> StartWave(int numOfEnemies, Moon moon)
        {
            CircleSpawning(3, moon);
            return Enemies;
        }

        public void SimpleWave(int numOfEnemies)
        {
            for (int i = 0; i < numOfEnemies; i++)
            {
                Enemies.Add(new Enemy(Enemies[i].spriteTexture, generatePosition(), Enemies[i].Width, Enemies[i].Height));
            }
        }

        public void CircleSpawning(int numOfEnemies, Moon moon)
        {
            for (int i = 0; i < numOfEnemies; i++)
            {
                Enemies.Add(new Enemy(Enemies[i].spriteTexture, generateCirclePositioning(moon), Enemies[i].Width, Enemies[i].Height));
            }
        }

        public Vector2 generateCirclePositioning(Moon moon)
        {
            Random random = new Random();
            float angleDegrees = random.Next(0, 360);
            float angleRadians = angleDegrees * (float)Math.PI / 180.0f;

            double x = moon.GetCentre().X + ((double)random.Next(100, 200) * Math.Cos(angleRadians));
            double y = moon.GetCentre().Y + ((double)random.Next(100, 200) * Math.Cos(angleRadians));

            return new Vector2((float)x, (float)y);
        }

        public Vector2 generatePosition()
        {
            Vector2 newPosition = Vector2.Zero;
            while(Math.Abs(newPosition.X) < 100)
                newPosition.X = random.Next(-200, 200);
            while (Math.Abs(newPosition.Y) < 100)
                newPosition.X = random.Next(-200, 200);

            return newPosition;
        } 


    }
}
