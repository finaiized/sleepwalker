using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SleepwalkerEngine
{
    /// <summary>
    /// A basic camera class for 2D games. Centers on a vector.
    /// </summary>
    public class Camera2D
    {
        /// <summary>
        /// The matrix that contains the necessary camera transforms.
        /// </summary>
        public Matrix Transform;

        /// <summary>
        /// The center that the camera should focus on.
        /// </summary>
        Vector2 center;

        /// <summary>
        /// The viewport containing the width and height of the screen.
        /// </summary>
        Viewport viewport;

        public Camera2D(Viewport newView)
        {
            viewport = newView;
        }

        public void Update(GameTime gameTime, Vector2 centerV)
        {
            center = centerV;

            // Translate the matrix to center, adding on a translation of the middle of the viewport
            Transform = Matrix.CreateTranslation(new Vector3(-center.X, -center.Y, 0)) *
                Matrix.CreateTranslation(new Vector3(viewport.Width * 0.5f, viewport.Height * 0.5f, 0));
        }
    }
}
