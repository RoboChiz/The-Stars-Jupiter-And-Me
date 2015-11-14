#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.GamerServices;
using RobsSprite;
#endregion

namespace RobsPhysics
{
    class Physics
    {

        public static List<RigidBody> objs;

        public Physics()
        {
            objs = new List<RigidBody>();
        }

        public class RigidBody : Sprite
        {

            public float Mass;
            public float inv_mass;

            public Vector2 Velocity;
            public Vector2 Force;

            public float terminalVelocity = 15f;

            public bool colliding;
            public RigidBody collidingWith;
            public RigidBody avoid;

            public RigidBody(Texture2D textureVal, Vector2 pos, int widthVal, int heightVal, float mass, float maxSpeed)
                : base(textureVal, pos, widthVal, heightVal)
            {
                Mass = mass;

                if (mass == 0)
                    inv_mass = 0;
                else
                    inv_mass = 1 / mass;

                Velocity = new Vector2(0, 0);
                Force = new Vector2(0, 0);
                terminalVelocity = maxSpeed;
                colliding = false;

                objs.Add(this);

            }

            public void AddForce(Vector2 dir)
            {
                Force += dir;
            }

            public bool checkCollision(RigidBody other)
            {

                Vector2 myCente = Position;
                Vector2 thereCente = other.Position;

                Vector2 normal = thereCente - myCente;

                if (normal.Length() < Width / 2)
                    return true;

                return false;
            }

        }

        void ResolveCollision(RigidBody A, RigidBody B)
        {

            A.Velocity = Vector2.Zero;
            B.Velocity = Vector2.Zero;

            // Calculate relative velocity
            Vector2 rv = B.Velocity - A.Velocity;
            Vector2 normal = B.Position - A.Position;           

            // Calculate relative velocity in terms of the normal direction
            float velAlongNormal = Vector2.Dot(rv, normal);

            /*// Do not resolve if velocities are separating
            if (velAlongNormal > 0)
                return;*/

            // Calculate restitution
            float e = 0.1f;

            // Calculate impulse scalar
            float j = -(1 + e) * velAlongNormal;
            j /= A.inv_mass + B.inv_mass;

            // Apply impulse
            Vector2 impulse = j * normal;
            A.AddForce(-impulse);
            B.AddForce(impulse);

        }

        public void Step(GameTime gameTime)
        {

            float deltaTime = (gameTime.ElapsedGameTime.Milliseconds / 1000f);

            foreach (RigidBody rb in objs)
            {

                rb.colliding = false;

                foreach (RigidBody rbo in objs)
                {
                    if (rbo != rb && (rb.avoid == null || rb.avoid != rbo) && rb.checkCollision(rbo))
                    {
                        rb.colliding = true;
                        rb.collidingWith = rbo;
                        ResolveCollision(rb, rbo);
                    }
                }

                if (rb.Mass != 0)
                {

                    Vector2 currentVelocity = rb.Velocity;

                    rb.Velocity += (rb.Force / rb.Mass) * deltaTime;

                    if (rb.Velocity.Length() > rb.terminalVelocity)
                        rb.Velocity = currentVelocity;

                    rb.Force = new Vector2(0, 0);

                    rb.Position += rb.Velocity * deltaTime;

                }

            }

        }

        public void AddObj(RigidBody rbody)
        {
            objs.Add(rbody);
        }

    }
}
