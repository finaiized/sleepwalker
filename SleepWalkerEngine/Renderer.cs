using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SleepwalkerEngine
{
    /// <summary>
    /// A renderer that draws graphics to the scene.
    /// 
    /// This implementation uses XNA's SpriteBatch internally.
    /// </summary>
    public class Renderer
    {
        /// <summary>
        /// The sprite batch which will draw the graphics
        /// </summary>
        private SpriteBatch spriteBatch;

        /// <summary>
        /// Initialize the renderer
        /// </summary>
        /// <param name="sb">The sprite batch to use</param>
        public Renderer(SpriteBatch sb)
        {
            spriteBatch = sb;
        }

        /// <summary>
        /// Prepare the renderer for drawing each frame.
        /// 
        /// Reset settings or prepare batches here.
        /// </summary>
        public void Start(Camera2D camera)
        {
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend,
                null, null, null, null, camera.Transform);
        }

        /// <summary>
        /// Draws a 2D sprite.
        /// </summary>
        /// <param name="sn">The scene node to draw</param>
        public void DrawSprite(SceneNode sn)
        {
            spriteBatch.Draw(sn.Sprite, sn.Rectangle, null, Color.White, sn.Rotation, Vector2.Zero,
                SpriteEffects.None, 0f);
        }

        /// <summary>
        /// Clean up the renderer after drawing each frame.
        /// </summary>
        public void End()
        {
            spriteBatch.End();
        }
    }
}
