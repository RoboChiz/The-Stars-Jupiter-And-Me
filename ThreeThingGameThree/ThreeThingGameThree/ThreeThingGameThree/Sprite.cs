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

namespace ThreeThingGameThree
{
    public class Sprite
    {

        public Vector2 position;
        public float width;
        public float height;

        public float rotation = 0f;

        public Texture2D texture;
        public Color colour;

        public Sprite(Texture2D textureVal, Vector2 pos, float widthVal, float heightVal)
        {
            texture = textureVal;
            position = pos;
            width = widthVal;
            height = heightVal;
            colour = Color.White;
        }

        public void FaceDirection(Vector2 dir)
        {

            rotation = (float)(Math.Atan2(-dir.Y, dir.X) + (85 * (Math.PI / 180)));
        }

    }

        public class Camera
        {
            public Vector2 position;
            public float zoom = 1f;

            public Camera(Vector2 pos, float zoomVal)
            {
                position = pos;
                zoom = zoomVal;
            }
        }

}
