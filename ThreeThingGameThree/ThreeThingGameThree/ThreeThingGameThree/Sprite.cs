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

namespace RobsSprite
{
    public class Sprite
    {
        // Asset name of Sprite texture
        public string assetName;

        // Texture of the sprite
        public Texture2D spriteTexture;

        //The size of the Sprite (with scale applied)
        public Rectangle Size;

        public static float X = 0.0f;
        public static float Y = 0.0f;

        public int width;
        public int height;

        //The position of the Sprite
        private Vector2 position = new Vector2(X, Y);

        //The amount to increase/decrease the size of the original sprite. 
        private float mScale = 1f;

        // Rotation of sprite
        public float rotation = 0f;

        private Color colour;

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public float Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }

        public int Width
        {
            get { return width; }
            set { width = value; }
        }

        public int Height
        {
            get { return height; }
            set { height = value; }
        }

         public float Scale
        {
            get { return mScale; }
            set
            {
                mScale = value;
                //Recalculate the Size of the Sprite with the new scale
                Size = new Rectangle(0, 0, (int)(spriteTexture.Width * Scale), (int)(spriteTexture.Height * Scale));
            }
        }

        public Sprite(Texture2D textureVal, Vector2 pos, int widthVal, int heightVal)
        {
            spriteTexture = textureVal;
            Position = pos;
            Width = widthVal;
            Height = heightVal;
            colour = Color.White;
        }

        public void FaceDirection(Vector2 dir)
        {

            rotation = (float)(Math.Atan2(-dir.Y, dir.X) + (85 * (Math.PI / 180)));
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(spriteTexture, new Rectangle((int)position.X, (int)position.Y, width, height), null, colour, rotation, Vector2.Zero, SpriteEffects.None, 0);
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
