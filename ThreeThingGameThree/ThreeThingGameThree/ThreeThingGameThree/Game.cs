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
    class GameClass
    {

        NewPlayer player;
        List<Enemy> enemies;
        Moon moon;
        EnemySpawner es;

        int enemyAmount;

        Camera cam;

        enum GameState
        {
            Attack,
            Rest
        }
        GameState gameState = GameState.Attack;

        public GameClass(float enemyNum)
        {
            enemies = new List<Enemy>();            
            cam = new Camera();
            es = new EnemySpawner();

            enemyAmount = 5;
        }

        public void StartGame()
        {
            moon = new Moon(Moon.moonTexture, new Vector2(0, 0), Moon.radius * 2, Moon.radius * 2);
            player = new NewPlayer(NewPlayer.playerTexture, new Vector2(0, 0), 25, 25);
            StartWave();
        }

        public void StartWave()
        {
            gameState = GameState.Attack;
 			enemies = es.StartWave(enemyAmount, moon);
            MediaPlayer.Stop();
            if (Game1.musicOn) 
            {
                MediaPlayer.Play(Game1.actionMusic);
            }
        }

        public void EndWave()
        {
            gameState = GameState.Rest;
            enemyAmount += 5;
            MediaPlayer.Stop();
            if (Game1.musicOn)
            {
                MediaPlayer.Play(Game1.inbetweenWaveMusic);
            }
        }

        public void Update(GameTime gameTime, GraphicsDevice device)
        {         
            float deltaTime = (gameTime.ElapsedGameTime.Milliseconds / 1000f);       

            if(gameState == GameState.Attack)
            {

                List<int> enemyToRemove = new List<int>();
                List<int> bulletToRemove = new List<int>();

                for (int i = 0; i < enemies.Count; i++)
                {
                    enemies[i].Update(deltaTime, moon);

                    for (int b = 0; b < player.bullets.Count; b++ )
                    {
                        if(Vector2.Distance(enemies[i].Position,player.bullets[b].Position) < enemies[i].width/2f)
                        {
                            enemies[i].health -= player.GunDamage;
                            bulletToRemove.Add(b);
                        }
                    }

                    if (enemies[i].distanceFromMoon <= Moon.radius)
                    {
                        enemyToRemove.Add(i);
                        moon.Health -= 5f;
                    }

                    if(enemies[i].health <= 0)
                    {
                        enemyToRemove.Add(i);
                    }
                }

                if(enemies.Count == 0) //At end of Wave
                {
                    EndWave();
                }

                for (int i = 0; i < enemyToRemove.Count; i++)
                {
                    enemies.RemoveAt(enemyToRemove[i]);
                }

                for (int i = 0; i < bulletToRemove.Count; i++)
                {
                    player.bullets.RemoveAt(bulletToRemove[i]);
                }

            }

            player.Update(deltaTime, moon, cam, device);

            //Get Camera to follow Player
            cam.Zoom = MathHelper.Lerp(cam.Zoom, 1f,deltaTime);

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

            for (int i = 0; i < enemies.Count; i++)
            {
                enemies[i].Draw(spriteBatch);
            }

            player.PlayerDraw(spriteBatch, cam, device);
            moon.DrawNoRot(spriteBatch);

            spriteBatch.End();

            //Draw GUI
            spriteBatch.Begin();
                player.DrawGUI(spriteBatch);
            spriteBatch.End();
        }


    }

    class NewPlayer : Sprite
    {

        public float angleOnMoon; //In Radians
        public float distanceFromMoon;
        private float minDistanceFromMoon;

        public static Texture2D playerTexture;
        public static Texture2D heartTexture;
        public static Texture2D noHeartTexture;
        public static Texture2D fuelBackgroundTexture;
        public static Texture2D fuelBarTexture;
        public static Texture2D gunTexture;
        public static Texture2D cursorTexture;
        public static Texture2D bulletTexture;

        public int maxHealth;
        public int currentHealth;

        public float currentFuel;
        private float currentAnimatedFuel;
        public float maxFuel;

        private bool flying;
        private bool isGrounded;

        private bool faceRight;

        private Sprite gun;
        private Sprite cursor;
        private Vector2 gunDir;

 		public float gunDamage;
        public float gunFireRate;
        private float gunWait;
        public List<Bullet> bullets;
	
		public int Health
        {
            get { return currentHealth; }
            set { currentHealth = value; }
        }

        public int MaxHealth
        {
            get { return maxHealth; }
            set { maxHealth = value; }
        }

        public float GunDamage
        {
            get { return gunDamage; }
            set { gunDamage = value; }
        }

        public float GunROF
        {
            get { return gunFireRate; }
            set { gunFireRate = value; }
        }

        public NewPlayer(Texture2D textureVal, Vector2 pos, int widthVal, int heightVal)
                : base(textureVal, pos, widthVal, heightVal)
        {
            angleOnMoon = (float)(-Math.PI / 2.0);
            distanceFromMoon = Moon.radius + (Width*0.5f);
            minDistanceFromMoon = distanceFromMoon;

            MaxHealth = 3;
            Health = 3;

            maxFuel = 100;
            currentFuel = maxFuel;
            currentAnimatedFuel = maxFuel;

            flying = false;
            isGrounded = true;

            faceRight = false;

            gun = new Sprite(gunTexture, pos, widthVal / 3, widthVal / 3);
            cursor = new Sprite(cursorTexture, Vector2.Zero, 10, 10);

            gunDir = new Vector2(1, 0);

            GunDamage = 1f;
            GunROF = 1f;
            bullets = new List<Bullet>();
        }
        

        public void Update(float deltaTime, Moon moon, Camera cam, GraphicsDevice device)
        {
            //Calculate Inputs
            float moveSpeed = deltaTime;
            if (!isGrounded)
                moveSpeed /= 2f;

            if (/*GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X > 0.15 ||*/ Keyboard.GetState().IsKeyDown(Keys.A) == true || Keyboard.GetState().IsKeyDown(Keys.Left) == true)
            { //Thumb stick directed right
                angleOnMoon -= moveSpeed;
                faceRight = false;
            }
            if (/*GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X < 0.15 ||*/ Keyboard.GetState().IsKeyDown(Keys.D) == true || Keyboard.GetState().IsKeyDown(Keys.Right) == true)
            { //Thumb stick directed left
                angleOnMoon += moveSpeed;
                faceRight = true;
            }

            flying = false;

            if (/*GamePad.GetState(PlayerIndex.One).AButton? < 0.15 ||*/ Keyboard.GetState().IsKeyDown(Keys.Space) == true)
            { //Thumb stick directed left
                float amount = deltaTime * 300f;
                if (currentFuel >= 0)
                {
                    currentFuel -= amount;
                    distanceFromMoon += amount;
                    flying = true;
                    isGrounded = false;
                }
            }

            currentAnimatedFuel = MathHelper.Lerp(currentAnimatedFuel, currentFuel, deltaTime * 10f);

            if (!flying)
            {
                distanceFromMoon -= deltaTime * 150f;                
            }

            if (distanceFromMoon < minDistanceFromMoon)
            {
                distanceFromMoon = minDistanceFromMoon;
                currentFuel = maxFuel;
                isGrounded = true;
            }

            //Calculate Position
            float x = moon.Position.X + (distanceFromMoon * (float)Math.Cos(angleOnMoon));
            float y = moon.Position.Y + (distanceFromMoon * (float)Math.Sin(angleOnMoon));

            Position = new Vector2(x, y);

            float angleAdd = 0.05f;
            if (!faceRight)
                angleAdd *= -1;

            x = moon.Position.X + (distanceFromMoon * (float)Math.Cos(angleOnMoon + angleAdd));
            y = moon.Position.Y + (distanceFromMoon * (float)Math.Sin(angleOnMoon + angleAdd));

            gun.Position = new Vector2(x, y);

            float camHeight = 100;
            float camX = moon.Position.X + ((distanceFromMoon + camHeight) * (float)Math.Cos(angleOnMoon));
            float camY = moon.Position.Y + ((distanceFromMoon + camHeight) * (float)Math.Sin(angleOnMoon));

            cam.Pos = Vector2.Lerp(cam.Pos, new Vector2(camX, camY), deltaTime * 6f);
           
            Vector2 lookDir = new Vector2(-(float)Math.Cos(angleOnMoon), -(float)Math.Sin(angleOnMoon));
            cam.Rotation = FaceDirection(lookDir);

            lookDir = new Vector2((float)Math.Cos(angleOnMoon), -(float)Math.Sin(angleOnMoon));
            FaceDirection(lookDir);

            MouseState ms = Mouse.GetState();
            Vector2 mousePosition = new Vector2(ms.X, ms.Y);
            Vector2 worldPosition = Vector2.Transform(mousePosition, Matrix.Invert(cam.get_transformation(device)));

            Vector2 gunDir = worldPosition - gun.Position;
            gunDir.Normalize();

            Vector2 bulletDir = gunDir;

            gunDir.Y *= -1;

            gunDir = Rotatevector(gunDir, (float)-Math.PI / 2f);

            gun.FaceDirection(gunDir);

            if (/*GamePad.GetState(PlayerIndex.One).AButton? < 0.15 ||*/ Mouse.GetState().LeftButton == ButtonState.Pressed)
            { //Thumb stick directed left
                if (gunWait >= GunROF)
                {
                    Bullet nBullet = new Bullet(bulletTexture, gun.Position, gun.width, gun.width, bulletDir);
                    bullets.Add(nBullet);
                    gunWait = 0f;
                }                
            }

            gunWait += deltaTime;

            for (int i = 0; i < bullets.Count; i++)
            {
                bullets[i].Update(deltaTime);

                if (Math.Abs(bullets[i].Position.X) > 1000 || Math.Abs(bullets[i].Position.Y) > 1000)
                {
                    bullets.RemoveAt(i);
                    i -= 1;
                }
            }

        }

        public void PlayerDraw(SpriteBatch spriteBatch, Camera cam, GraphicsDevice device)
        {
            int spriteWidth = (int)(Width);
            int spriteHeight = (int)(Height);

            int spriteX = (int)(Position.X);
            int spriteY = (int)(Position.Y);

            Rectangle destinationRectangle = new Rectangle(spriteX, spriteY, spriteWidth, spriteHeight);

            Vector2 spriteOrigin = new Vector2(spriteTexture.Width / 2f, spriteTexture.Height / 2f);

            SpriteEffects sp = SpriteEffects.None;

            if (!faceRight)
                sp = SpriteEffects.FlipHorizontally;

            spriteBatch.Draw(spriteTexture, destinationRectangle, null, Color.White, Rotation, spriteOrigin, sp, 0);

            gun.Draw(spriteBatch);

            for (int i = 0; i < bullets.Count; i++)
            {
                bullets[i].Draw(spriteBatch);
            }

        }

        public void TakeDamage()
        {
            Health -= 1;
        }

        public void GainHealth()
        {
            if(Health < MaxHealth)
                Health += 1;
        }

        public void DrawGUI(SpriteBatch spriteBatch)
        {
            //Draw Hearts
            for(int i = 0; i < maxHealth; i++)
            {
                Sprite Heart;

                if(i <= currentHealth)
                    Heart = new Sprite(heartTexture, new Vector2(10 + (i*25), 10), 20, 20);
                else
                    Heart = new Sprite(noHeartTexture, new Vector2(10 + (i * 25), 10), 20, 20);

                Heart.DrawNoRotCentre(spriteBatch);
            }

            
            //Draw Jetpack Fuel
            Sprite fuelBackground = new Sprite(fuelBackgroundTexture,new Vector2(10,40),(int)(maxFuel),20);
            Sprite fuelBar = new Sprite(fuelBarTexture, new Vector2(10, 40), (int)(MathHelper.Clamp(currentAnimatedFuel, 0f, maxFuel)), 20);
            fuelBackground.DrawNoRotCentre(spriteBatch);
            fuelBar.DrawNoRotCentre(spriteBatch);

            MouseState ms = Mouse.GetState();
            Vector2 mousePosition = new Vector2(ms.X, ms.Y);
            cursor.Position = mousePosition;

            cursor.DrawNoRotCentre(spriteBatch);

        }

        public Vector2 Rotatevector(Vector2 vector,float angle)
        {
            float x = vector.X;
            float y = vector.Y;

            float newX = (float)((x * Math.Cos(angle)) - (Math.Sin(angle)*y));
            float newY = (float)((y * Math.Cos(angle)) + (Math.Sin(angle) * x));

            return new Vector2(newX,newY);
        }
    }

    class Bullet : Sprite
    {
        private Vector2 dir;
        static public float speed = 150f;
        public Bullet(Texture2D textureVal, Vector2 pos, int widthVal, int heightVal, Vector2 bulletDir)
                : base(textureVal, pos, widthVal, heightVal)
         {
             dir = Vector2.Normalize(bulletDir);
             FaceDirection(bulletDir);
            if (Game1.sfxOn)
            {
                Game1.bulletSound.Play(0.5f, 0f, 0f);
            }
         }

        public void Update(float deltaTime)
        {
            Position += dir * deltaTime * speed;               
        }



    }

    class Moon : Sprite
    {

        public Moon(Texture2D textureVal, Vector2 pos, int widthVal, int heightVal)
                : base(textureVal, pos, widthVal, heightVal){}

        public static Texture2D moonTexture;
        public static int radius = 250;

        float health;
        float maxHealth = 100;

        public float Health
        {
            get { return health; }
            set { health = value; }
        }

        public float MaxHealth
        {
            get { return maxHealth; }
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
            _zoom = 0.0f;
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
