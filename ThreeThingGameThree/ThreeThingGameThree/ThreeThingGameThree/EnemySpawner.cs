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
        private Random random;

        public EnemySpawner()
        {
            random = new Random();
        }

        public List<Enemy> StartWave(int numOfEnemies, Moon moon)
        {
            CircleSpawning(numOfEnemies, moon);
            return Enemies;
        }

        public void CircleSpawning(int numOfEnemies, Moon moon)
        {
            for (int i = 0; i < numOfEnemies; i++)
            {
                Enemies.Add(new Enemy(null, Vector2.Zero, 25, 25, generateCirclePositioning(),Moon.radius + 700 + (i * 100)));
            }
        }

        public float generateCirclePositioning()
        {
            float angleDegrees = random.Next(0, 360);
            float angleRadians = angleDegrees * (float)Math.PI / 180.0f;

            return angleRadians;
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
