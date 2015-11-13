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
        static Random random = new Random();


        public List<Enemy> StartWave(int numOfEnemies)
        {
            for (int i = 0; i < numOfEnemies; i++)
            {
                Enemies.Add(new Enemy(Enemies[i].spriteTexture,generatePosition(),Enemies[i].Width, Enemies[i].Height));
            }
            return Enemies;
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
