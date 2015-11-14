using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RobsSprite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace ThreeThingGameThree
{
    class Enemy : Sprite
    {

        static Random random = new Random();
        static public Texture2D enemyTexture;

        public float angleOnMoon; //In Radians
        public float distanceFromMoon;

        private float speed;

        public Enemy(Texture2D textureVal, Vector2 pos, int widthVal, int heightVal, float angle, float distance)
            : base(enemyTexture, pos, widthVal, heightVal)
        {
            speed = random.Next(50, 100);
            angleOnMoon = angle;
            distanceFromMoon = distance;
        }

        public void Update(float deltaTime, Moon moon)
        {
            //Move towards moon position
            distanceFromMoon -= speed * deltaTime;
            
            float x = moon.Position.X + (distanceFromMoon * (float)Math.Cos(angleOnMoon));
            float y = moon.Position.Y + (distanceFromMoon * (float)Math.Sin(angleOnMoon));

            Position = new Vector2(x, y);
        }

    }
}
