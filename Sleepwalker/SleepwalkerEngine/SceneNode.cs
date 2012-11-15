using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SleepwalkerEngine
{
    /// <summary>
    /// A node in the scene.
    /// </summary>
    public class SceneNode
    {
        /// <summary>
        /// The position of the scene node.
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// The rotation of the scene node, in radians.
        /// </summary>
        public float Rotation { get; set; }

        /// <summary>
        /// The x and y scale of the scene node.
        /// </summary>
        public Vector2 Scale { get; set; }

        /// <summary>
        /// The name of the scene node. It need not be unique.
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// The texture representing the scene node.
        /// </summary>
        public Texture2D Sprite { get; set; }

        public SceneNode()
        {
            Position = Vector2.Zero;
            Rotation = 0f;
            Scale = Vector2.One;
        }
    }
}
