using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1
{
    class Camera
    {
        /// <summary>
        /// Declaration and accessors for variables.
        /// </summary>
        int x;
        private Matrix transform;
        public Matrix Transform
        {
            get { return transform; }
        }
        public int X
        {
            get { return x; }
        }
        private Vector2 centre;
        private Viewport viewport;
        /// <summary>
        /// Constructor class.
        /// </summary>
        /// <param name="newViewport"></param>
        public Camera(Viewport newViewport)
        {
            viewport = newViewport;
        }
        /// <summary>
        /// Update method which uses a matrix and the players position to determine the camera location.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="xOffset"></param>
        /// <param name="yOffset"></param>
        public void Update(Vector2 position, int xOffset, int yOffset)
        {
            if (position.X < viewport.Width / 2)
                centre.X = viewport.Width / 2;
            else if (position.X > xOffset - viewport.Width / 2)
            {
                centre.X = xOffset - (viewport.Width / 2);
            }
            else centre.X = position.X;

            if (position.Y < viewport.Height / 2)
                centre.Y = viewport.Height / 2;
            else if (position.Y > yOffset - viewport.Height / 2)
            {
                centre.Y = yOffset - (viewport.Height / 2);
            }
            else centre.Y = position.Y;

            transform = Matrix.CreateTranslation(new Vector3(-centre.X + (viewport.Width / 2),
                                                             -centre.Y + (viewport.Height / 2), 0));
            x = (int)-centre.X + (viewport.Width / 2);
        }
    }
}
