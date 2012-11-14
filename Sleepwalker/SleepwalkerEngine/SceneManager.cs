using System.Collections.Generic;
using System.Linq;

namespace Sleepwalker
{
    /// <summary>
    /// The manager of all scene nodes.
    /// </summary>
    public class SceneManager
    {
        /// <summary>
        /// A list of all scene nodes in the scene.
        /// </summary>
        List<SceneNode> _nodes;

        /// <summary>
        /// Initialize the scene manager.
        /// </summary>
        public SceneManager()
        {
            _nodes = new List<SceneNode>();
        }

        public void Add(SceneNode sn)
        {
            _nodes.Add(sn);
        }

        /// <summary>
        /// Returns a list of all the nodes with a given name.
        /// </summary>
        /// <param name="name">The name of the node to be found.</param>
        /// <returns>A list of all scene nodes with the given name.</returns>
        public IEnumerable<SceneNode> GetNodeByName(string name)
        {
            return _nodes.Where(sn => sn.Name == name);
        }

        /// <summary>
        /// A list of all the nodes in the scene.
        /// </summary>
        /// <returns>All the nodes in the scene.</returns>
        public IEnumerable<SceneNode> GetAllNodes()
        {
            return _nodes;
        }
    }
}
