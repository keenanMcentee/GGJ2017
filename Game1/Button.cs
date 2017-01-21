using Microsoft.Xna.Framework;
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
    class Button
    {
        private Texture2D texture;      //The background texture for the button
        private Vector2 position;       //A vector which will be used to position the button on the screen.
        private Vector2 size;           //Vector that sets the size of the button.
        private Rectangle rectangle;    //rectangle used for checking if mouse is over it.
        private bool isClicked;         //Boolean that checks if the button is clicked.
        Color currentColour;
        Color colourOff;
        Color colourOn;
        string text = "";
        SpriteFont textFont;
        bool locked = false;
        public Vector2 Size
        {
            get { return size; }
            set { size = value; }
        }
        public bool Locked
        {
            set { locked = value; }
        }
        public bool IsClicked
        {
            get { return isClicked; }
            set { isClicked = value; }
        }
        /// <summary>
        /// Constructor method for the button class.
        /// It needs to be passed a texture, the windows width
        /// & the window height.
        /// The window width and height are used for the button to automatically scale itself.
        /// </summary>
        /// <param name="newTexture"></param>
        /// <param name="windowWidth"></param>
        /// <param name="windowHeight"></param>
        public Button(Texture2D newTexture, int windowWidth, int windowHeight, SpriteFont gameFont, Color _colourOff, Color _colourOn, ContentManager Content)
        {
            if (newTexture != null)
                texture = newTexture;
            else
            {
                texture = Content.Load<Texture2D>("Tile2");
            }
            size = new Vector2(windowWidth / 8, windowHeight / 30); //This sets a universal button size.
            textFont = gameFont;
            colourOff = _colourOff;
            colourOn = _colourOn;
            currentColour = colourOff;
        }

        /// <summary>
        /// Method that handles all the update of the button.
        /// First checks if the mouse is inside the button rectangle
        /// if the button is inside, it changes the button to yellow.
        /// else the button remains white.
        /// If the mouse is inside the rectangle and left mouse button is clicked the set isClicked to true.
        /// </summary>
        /// <param name="mouse"></param>
        public void Update(MouseState mouse, MouseState previousMouseState)
        {
            rectangle = new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
            Rectangle mouseRect = new Rectangle(mouse.X, mouse.Y, 1, 1);    //This rectangle is used to keep track of the pointer on the screen;

            if (mouseRect.Intersects(rectangle) && locked == false)
            {
                currentColour = colourOn;
                if (mouse.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released)
                {
                    isClicked = true;
                }
            }
            else
            {
                currentColour = colourOff;
            }
        }
        /// <summary>
        /// This is a method that handles setting the position of the button.
        /// </summary>
        /// <param name="position"></param>
        public void setPosition(Vector2 position)
        {
            this.position = position;
        }
        public void setText(string _text)
        {
            this.text = _text;
        }
        /// <summary>
        /// This is the basic draw method which handles drawing the button the the screen.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, rectangle, currentColour);
            spriteBatch.DrawString(textFont, text, new Vector2(rectangle.X + 1, rectangle.Y + 1), Color.Black);
        }
    }
}
