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

        enum State
        {
            Idling,
            Moving
        }

        public Enemy(Texture2D textureVal, Vector2 pos, int widthVal, int heightVal)
            : base(textureVal, pos, widthVal, heightVal) {}

        public void Update(GameTime gameTime, RobsPhysics.Physics.RigidBody moonObj)
        {
            //Move towards moon position
            Direction = Vector2.Normalize(moonObj.Position - this.Position);
            Position += Direction * 2f;
        }





    }
}
