using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1
{
    class Player
    {
        /// <summary>
        /// Declaration and initilisation of variables and data.
        /// </summary>
        private Texture2D textureL;
        Texture2D textureR;
        Texture2D texture;
        
        private Vector2 position;
        public Vector2 velocity;
        private Rectangle rectangle;
        private Vector2 arrayPosition;
        private bool hasJumped = false;
        SoundEffect deathSound;
        SoundEffectInstance deadSound;
        SoundEffect desintegrate;
        SoundEffectInstance desintegration;
        bool alive; public bool Alive { set { alive = value; } get { return alive; } }
        bool roundOver; public bool RoundOver { get { return roundOver; } }
        KeyboardState keyboard;
        KeyboardState pastKeyboard;
        GamePadState gamePad;
        GamePadState previousGamePad;
        const int speed = 4;
        int score = 0;
        int blockSize = 64;
        //List<Particles> particle;
        Random rnd;
        bool loadNextLevel;
        /// <summary>
        /// Accessors for the variables.
        /// </summary>
        public bool LoadNextLevel
        {
            get { return loadNextLevel; }
            set
            {
                loadNextLevel = value;
            }
        }

        public Vector2 Position
        {
            get { return position; }
        }
        public int Score
        {
            get { return score; }
            set { score = value; }
        }
        /// <summary>
        /// Player constructore method.
        /// </summary>
        public Player()
        {
           
            alive = true;
            //particle = new List<Particles>();
        }
        /// <summary>
        /// Player load method which situates the player on the screen and loads in necessary resources.
        /// </summary>
        /// <param name="Content"></param>
        /// <param name="map"></param>
        public void Load(ContentManager Content, int[,] map)
        {
            textureL = Content.Load<Texture2D>("player");
            textureR = Content.Load<Texture2D>("player2");
            texture = textureL;
            for (int x = 0; x < map.GetLength(1); x++)
            {
                for (int y = 0; y < map.GetLength(0); y++)
                {
                    int number = map[y, x];
                    if (number == 1000)
                    {
                        position = new Vector2(x * blockSize, y * blockSize);
                    }
                }

            }
            //deathSound = Content.Load<SoundEffect>("1 Scream");
            //deadSound = deathSound.CreateInstance();
            //desintegrate = Content.Load<SoundEffect>("Desintegration");
            //desintegration = desintegrate.CreateInstance();
        }
        /// <summary>
        /// Update for the player class,
        /// performs all updates the player will need in game,
        /// also adds in user control.
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {

            if (alive)
            {
                position += velocity;
                rectangle = new Rectangle((int)position.X, (int)position.Y, blockSize, blockSize);

                Input(gameTime);

                if (velocity.Y < 5)
                    velocity.Y += 0.5f;
                if (position.Y < 0)
                    position.Y = 2;
                arrayPosition.X = (int)position.X / blockSize;
                arrayPosition.Y = (int)position.Y / blockSize;
            }
            //if (particle != null)
            //{
            //    //foreach (Particles a in particle)
            //    //{
            //    //    a.Update();
            //    //}
            //    int particleCount = particle.Count;
            //    for (int i = 0; i < particleCount; i++)
            //    {
            //        if (particle[i].Alive == false)
            //        {
            //            particle.Remove(particle[i]);
            //            i--;
            //            particleCount--;
            //        }
            //    }
            //}
            //if (particle == null && alive == false)
            //{
            //    roundOver = true;
            //}
        }
        /// <summary>
        /// Input method which handles all the controls and user input for the player object.
        /// </summary>
        /// <param name="gameTime"></param>
        private void Input(GameTime gameTime)
        {
            gamePad = GamePad.GetState(PlayerIndex.One);
            keyboard = Keyboard.GetState();

            if (keyboard.IsKeyDown(Keys.D) || keyboard.IsKeyDown(Keys.Right) || (gamePad.DPad.Right == ButtonState.Pressed) || (gamePad.ThumbSticks.Left.X > 0.5))
            {
                texture = textureR;
                velocity.X = speed;
            }
            else if (keyboard.IsKeyDown(Keys.Left) || keyboard.IsKeyDown(Keys.A) || (gamePad.DPad.Left == ButtonState.Pressed) || (gamePad.ThumbSticks.Left.X < -0.5))
            {
                texture = textureL;
                velocity.X = -speed;
            }
            else velocity.X = 0f;

            if (((keyboard.IsKeyDown(Keys.Space) && pastKeyboard.IsKeyUp(Keys.Space)) || (gamePad.Buttons.A == ButtonState.Pressed) && (previousGamePad.Buttons.A == ButtonState.Released)) && hasJumped == false)
            {
                velocity.Y -= 16;
                //position.Y -= 50;
                hasJumped = true;
            }
            pastKeyboard = Keyboard.GetState();
            previousGamePad = GamePad.GetState(PlayerIndex.One);
        }
        /// <summary>
        /// MEthod to handle all the collisions of the player with the world blocks (static ones)
        /// uses the array to determine the blocks position and prevents the player from moving in to that space.
        /// </summary>
        /// <param name="newRectangle"></param>
        /// <param name="xOffset"></param>
        /// <param name="yOffset"></param>
        /// <param name="mapArray"></param>
        public void CollisionWorld(Rectangle newRectangle, int xOffset, int yOffset, int[,] mapArray)
        {
            Vector2 previousPos = position;
            xOffset -= 5;
            yOffset -= 5;
            arrayPosition.X = (int)position.X / blockSize;
            arrayPosition.Y = (int)position.Y / blockSize;
            if (velocity.Y < 3f)
            {
                velocity.Y += 0.002f;
            }
            if (arrayPosition.Y > 0 && arrayPosition.Y < 200)
            {
                //If touching the ground or top of a block.
                if (/*mapArray[(int)(arrayPosition.Y + 1), (int)arrayPosition.X] == 1 ||*/ 
                    mapArray[(int)(arrayPosition.Y + 1), (int)arrayPosition.X] == 2 || 
                    mapArray[(int)(arrayPosition.Y + 1), (int)arrayPosition.X] == 7 ||
                    mapArray[(int)(arrayPosition.Y + 1), (int)arrayPosition.X] == 8 ||
                    mapArray[(int)(arrayPosition.Y + 1), (int)arrayPosition.X] == 12 ||
                    mapArray[(int)(arrayPosition.Y + 1), (int)arrayPosition.X] == 17 ||
                    mapArray[(int)(arrayPosition.Y + 1), (int)arrayPosition.X] == 18 ||
                    mapArray[(int)(arrayPosition.Y + 1), (int)arrayPosition.X] == 22 ||
                    mapArray[(int)(arrayPosition.Y + 1), (int)arrayPosition.X] == 27 ||
                    mapArray[(int)(arrayPosition.Y + 1), (int)arrayPosition.X] == 28 ||
                    mapArray[(int)(arrayPosition.Y + 1), (int)arrayPosition.X] == 37 ||
                    mapArray[(int)(arrayPosition.Y + 1), (int)arrayPosition.X] == 38 ||
                    mapArray[(int)(arrayPosition.Y + 1), (int)arrayPosition.X] == 43 ||
                    mapArray[(int)(arrayPosition.Y + 1), (int)arrayPosition.X] == 48 ||
                    mapArray[(int)(arrayPosition.Y + 1), (int)arrayPosition.X] == 49 ||
                    mapArray[(int)(arrayPosition.Y + 1), (int)arrayPosition.X] == 54 ||
                    mapArray[(int)(arrayPosition.Y + 1), (int)arrayPosition.X] == 55 ||
                    mapArray[(int)(arrayPosition.Y + 1), (int)arrayPosition.X] == 60 ||
                    mapArray[(int)(arrayPosition.Y + 1), (int)arrayPosition.X] == 61 ||
                    mapArray[(int)(arrayPosition.Y + 1), (int)arrayPosition.X] == 66 ||
                    mapArray[(int)(arrayPosition.Y + 1), (int)arrayPosition.X] == 71 ||
                    mapArray[(int)(arrayPosition.Y + 1), (int)arrayPosition.X] == 72 ||
                    mapArray[(int)(arrayPosition.Y + 1), (int)arrayPosition.X] == 76 ||
                    mapArray[(int)(arrayPosition.Y + 1), (int)arrayPosition.X] == 81 ||
                    mapArray[(int)(arrayPosition.Y + 1), (int)arrayPosition.X] == 82 ||
                    rectangle.Intersects(newRectangle))
                {
                    if (velocity.Y > 0)
                        velocity.Y = 0;

                    hasJumped = false;
                }
                else if (mapArray[(int)(arrayPosition.Y + 1), (int)arrayPosition.X] == 3)
                {
                    if (alive)
                    {
                        alive = false;
                        //deathSound.Play();
                        //for (int i = 0, j = rnd.Next(200, 400); i < j; i++)
                        //{
                        //    particle.Add(new Particles(new Vector2(rectangle.X, rectangle.Y), rnd, texture));
                        //}
                    }
                }
                else if ((mapArray[(int)(arrayPosition.Y - 1), (int)arrayPosition.X + 1] == 1 || mapArray[(int)(arrayPosition.Y - 1), (int)arrayPosition.X + 1] == 2))
                {
                    if (velocity.Y < 0 && position.Y % blockSize != 0)
                        velocity.Y = 0f;
                    //position.Y += position.Y % (blockSize / 100);
                }
            }
            //If player ain't touching the ground.
            if (arrayPosition.Y > 0 && arrayPosition.Y < 100)
            {

                //if the player hits off the top of a block.
                if ((mapArray[(int)(arrayPosition.Y + 1), (int)arrayPosition.X] == 1) && hasJumped == true||
                    (mapArray[(int)(arrayPosition.Y + 1), (int)arrayPosition.X] == 2) && hasJumped == true ||
                    (mapArray[(int)(arrayPosition.Y + 1), (int)arrayPosition.X] == 7) && hasJumped == true ||
                    (mapArray[(int)(arrayPosition.Y + 1), (int)arrayPosition.X] == 8) && hasJumped == true ||
                    (mapArray[(int)(arrayPosition.Y + 1), (int)arrayPosition.X] == 12) && hasJumped == true ||
                    (mapArray[(int)(arrayPosition.Y + 1), (int)arrayPosition.X] == 17) && hasJumped == true ||
                    (mapArray[(int)(arrayPosition.Y + 1), (int)arrayPosition.X] == 18) && hasJumped == true ||
                    (mapArray[(int)(arrayPosition.Y + 1), (int)arrayPosition.X] == 22) && hasJumped == true ||
                    (mapArray[(int)(arrayPosition.Y + 1), (int)arrayPosition.X] == 27) && hasJumped == true ||
                    (mapArray[(int)(arrayPosition.Y + 1), (int)arrayPosition.X] == 28) && hasJumped == true ||
                    (mapArray[(int)(arrayPosition.Y + 1), (int)arrayPosition.X] == 37) && hasJumped == true ||
                    (mapArray[(int)(arrayPosition.Y + 1), (int)arrayPosition.X] == 38) && hasJumped == true ||
                    (mapArray[(int)(arrayPosition.Y + 1), (int)arrayPosition.X] == 43) && hasJumped == true ||
                    (mapArray[(int)(arrayPosition.Y + 1), (int)arrayPosition.X] == 48) && hasJumped == true ||
                    (mapArray[(int)(arrayPosition.Y + 1), (int)arrayPosition.X] == 49) && hasJumped == true ||
                    (mapArray[(int)(arrayPosition.Y + 1), (int)arrayPosition.X] == 54) && hasJumped == true ||
                    (mapArray[(int)(arrayPosition.Y + 1), (int)arrayPosition.X] == 55) && hasJumped == true ||
                    (mapArray[(int)(arrayPosition.Y + 1), (int)arrayPosition.X] == 60) && hasJumped == true ||
                    (mapArray[(int)(arrayPosition.Y + 1), (int)arrayPosition.X] == 61) && hasJumped == true ||
                    (mapArray[(int)(arrayPosition.Y + 1), (int)arrayPosition.X] == 66) && hasJumped == true ||
                    (mapArray[(int)(arrayPosition.Y + 1), (int)arrayPosition.X] == 71) && hasJumped == true ||
                    (mapArray[(int)(arrayPosition.Y + 1), (int)arrayPosition.X] == 72) && hasJumped == true ||
                    (mapArray[(int)(arrayPosition.Y + 1), (int)arrayPosition.X] == 76) && hasJumped == true ||
                    (mapArray[(int)(arrayPosition.Y + 1), (int)arrayPosition.X] == 81) && hasJumped == true ||
                    (mapArray[(int)(arrayPosition.Y + 1), (int)arrayPosition.X] == 82) && hasJumped == true ||
                    rectangle.Intersects(newRectangle))
                {
                    velocity.Y = 0;
                    position = previousPos;
                }
                //if (mapArray[(int)(arrayPosition.Y), (int)arrayPosition.X] == 2)
                //    {
                //    velocity.Y -= 1;
                //    }
                else
                {
                    if (velocity.Y < 1f)
                    {
                        velocity.Y += 0.001f;
                    }
                    if (velocity.Y < 0.05f)
                    {
                        hasJumped = false;
                    }
                }
                //If the player hits is inside a block, put him on top of it
                if ((mapArray[(int)(arrayPosition.Y + 1), (int)arrayPosition.X] == 1) && hasJumped == true ||
                    (mapArray[(int)(arrayPosition.Y + 1), (int)arrayPosition.X] == 2) && hasJumped == true ||
                    (mapArray[(int)(arrayPosition.Y + 1), (int)arrayPosition.X] == 7) && hasJumped == true ||
                    (mapArray[(int)(arrayPosition.Y + 1), (int)arrayPosition.X] == 8) && hasJumped == true ||
                    (mapArray[(int)(arrayPosition.Y + 1), (int)arrayPosition.X] == 12) && hasJumped == true ||
                    (mapArray[(int)(arrayPosition.Y + 1), (int)arrayPosition.X] == 17) && hasJumped == true ||
                    (mapArray[(int)(arrayPosition.Y + 1), (int)arrayPosition.X] == 18) && hasJumped == true ||
                    (mapArray[(int)(arrayPosition.Y + 1), (int)arrayPosition.X] == 22) && hasJumped == true ||
                    (mapArray[(int)(arrayPosition.Y + 1), (int)arrayPosition.X] == 27) && hasJumped == true ||
                    (mapArray[(int)(arrayPosition.Y + 1), (int)arrayPosition.X] == 28) && hasJumped == true ||
                    (mapArray[(int)(arrayPosition.Y + 1), (int)arrayPosition.X] == 37) && hasJumped == true ||
                    (mapArray[(int)(arrayPosition.Y + 1), (int)arrayPosition.X] == 38) && hasJumped == true ||
                    (mapArray[(int)(arrayPosition.Y + 1), (int)arrayPosition.X] == 43) && hasJumped == true ||
                    (mapArray[(int)(arrayPosition.Y + 1), (int)arrayPosition.X] == 48) && hasJumped == true ||
                    (mapArray[(int)(arrayPosition.Y + 1), (int)arrayPosition.X] == 49) && hasJumped == true ||
                    (mapArray[(int)(arrayPosition.Y + 1), (int)arrayPosition.X] == 54) && hasJumped == true ||
                    (mapArray[(int)(arrayPosition.Y + 1), (int)arrayPosition.X] == 55) && hasJumped == true ||
                    (mapArray[(int)(arrayPosition.Y + 1), (int)arrayPosition.X] == 60) && hasJumped == true ||
                    (mapArray[(int)(arrayPosition.Y + 1), (int)arrayPosition.X] == 61) && hasJumped == true ||
                    (mapArray[(int)(arrayPosition.Y + 1), (int)arrayPosition.X] == 66) && hasJumped == true ||
                    (mapArray[(int)(arrayPosition.Y + 1), (int)arrayPosition.X] == 71) && hasJumped == true ||
                    (mapArray[(int)(arrayPosition.Y + 1), (int)arrayPosition.X] == 72) && hasJumped == true ||
                    (mapArray[(int)(arrayPosition.Y + 1), (int)arrayPosition.X] == 76) && hasJumped == true ||
                    (mapArray[(int)(arrayPosition.Y + 1), (int)arrayPosition.X] == 81) && hasJumped == true ||
                    (mapArray[(int)(arrayPosition.Y + 1), (int)arrayPosition.X] == 82) && hasJumped == true )
                {
                    position.Y = previousPos.Y;
                    position.Y = position.Y - blockSize;
                }
                //else if (mapArray[(int)(arrayPosition.Y), (int)arrayPosition.X] == 4)
                //{
                //    loadNextLevel = true;
                //}
            }
            //Collision with blocks side to side
            if (position.X > 0 && position.X < mapArray.GetLength(1) * blockSize)
            {
                /*if (arrayPosition.X > 0 && arrayPosition.X < mapArray.GetLength(1) && arrayPosition.Y > 0 && arrayPosition.Y < mapArray.GetLength(1))
                {*/
                    if (rectangle.TouchLeftOf(newRectangle))
                    {
                        if (velocity.X > 0)
                        {
                            velocity.X = 0;
                            position = previousPos;
                        }
                    }

                    if (rectangle.TouchRightOf(newRectangle))
                        if (velocity.X < 0 && position.X % blockSize == 0)
                        {
                            velocity.X = 0;
                            position = previousPos;
                        }
                    
                
            }
            if (velocity.Y < 0.1)
            {
                position.Y -= position.Y % blockSize;
            }


            if (position.X < 0) position.X = 0;
            if (position.X > xOffset - rectangle.Width) position.X = xOffset - rectangle.Width;
            if (position.Y < 0) velocity.Y = 1f;
            if (position.Y > yOffset - rectangle.Height) position.Y = yOffset - rectangle.Height;
        }
        /// <summary>
        /// Collision method for when player hits enemy, this will either kill player or enemy
        /// depending on which part of eachother they hit off.
        /// </summary>
        /// <param name="enemy"></param>
        /// <param name="rnd"></param>
        public void CollisionEnemy(Enemy enemy, Random rnd)
        {
            this.rnd = rnd;
            //if (enemy.Alive)
            //{
                //if (rectangle.TouchTopOf(enemy.Rectangle))
                //{
                //    rectangle.Y = enemy.Rectangle.Y - rectangle.Height;
                //    position.Y -= 2f;
                //    velocity.Y -= blockSize;
                //    enemy.Alive = false;
                //    //desintegration.Play();

                //    //score += 100;
                //}
                //else if (enemy.Rectangle.Intersects(rectangle))
                //{
                //    if (alive)
                //    {
                //        alive = false;
                //        deathSound.Play();
                //        for (int i = 0, j = rnd.Next(200, 400); i < j; i++)
                //        {

                //            particle.Add(new Particles(new Vector2(rectangle.X, rectangle.Y), rnd, texture));
                //        }
                //    }
                //}


            
        }
        
        /// <summary>
        /// Draw method for the player to draw him to screen.
        /// </summary>
        /// <param name="spritebatch"></param>
        /// <param name="e"></param>
        public void Draw(SpriteBatch spritebatch, GraphicsDeviceManager e)
        {

            if (alive)
            {
                spritebatch.Draw(texture, rectangle, Color.White);
            }
            //if (particle != null)
            //{
            //    foreach (Particles a in particle)
            //    {
            //        a.Draw(spritebatch);
            //    }
            //}
        }
    }
}
