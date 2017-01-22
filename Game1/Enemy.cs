using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1
{
    class Enemy
    {
        /// <summary>
        /// Declaration and accessors for variables of the enemy class.
        /// </summary>
        Vector2 startingPos;
        Vector2 position;
        Vector2 velocity;
        Texture2D t_left, t_right;
        Texture2D texture; public Texture2D Texture { get { return texture; } }
        Rectangle rectangle; public Rectangle Rectangle { get { return rectangle; } }
        int direction;
        const int Left = 0;
        const int Right = 1;
        int id;
        bool alive = true; public bool Alive { get { return alive; } set { alive = value; } }
        /// <summary>
        /// Constructor method which gives the enemy a position passed from the array.
        /// </summary>
        /// <param name="_position"></param>
        /// <param name="size"></param>
        public Enemy(Vector2 _position, int size, int ID)
        {
            position = new Vector2(_position.X * size, _position.Y * size);
            startingPos = position;
            id = ID;
        }
        /// <summary>
        /// Load method so the enemy can load his own textures and decide a direction.
        /// </summary>
        /// <param name="Content"></param>
        /// <param name="rnd"></param>
        public void Load(ContentManager Content, Random rnd)
        {
            t_left = Content.Load<Texture2D>("enemy" + (id - 1000));
            t_right = Content.Load<Texture2D>("enemy"+(id - 1000 + 1));
            texture = t_left;
            direction = rnd.Next(0, 2);
        }
        /// <summary>
        /// Update method to move the enemy.
        /// </summary>
        public void Update()
        {
            if (direction == Left)
            {
                velocity = new Vector2(-1, 0);
                texture = t_right;
            }
            else
            {
                texture = t_left;
                velocity = new Vector2(1, 0);
            }
            position += velocity;
        }
        /// <summary>
        /// Draw method to draw the enemy to screen.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            if (alive)
            {
                rectangle = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
                spriteBatch.Draw(texture, rectangle, Color.White);
            }
        }
        /// <summary>
        /// Collision method that handles if the enemy hits the a wall to reverse direction.
        /// </summary>
        /// <param name="newRectangle"></param>
        /// <param name="xOffset"></param>
        /// <param name="yOffset"></param>
        public void Collision(Rectangle newRectangle, int xOffset, int yOffset)
        {
            if (rectangle.TouchLeftOf(newRectangle))
            {
                direction = Left;
            }
            if (rectangle.TouchRightOf(newRectangle))
            {
                direction = Right;
            }
        }

    }
}
