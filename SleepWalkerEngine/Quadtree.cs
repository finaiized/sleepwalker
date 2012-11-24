using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace SleepwalkerEngine
{
    public class Quadtree
    {
        private const int MAX_OBJECTS = 10;
        private const int MAX_LEVELS = 5;

        private int level;
        private List<Rectangle> objects;
        private Rectangle bounds;
        private Quadtree[] nodes;

        public Quadtree(int Level, Rectangle Bounds)
        {
            level = Level;
            objects = new List<Rectangle>();
            bounds = Bounds;
            nodes = new Quadtree[4];
        }

        /// <summary>
        /// Clear the contents of the quadtree
        /// </summary>
        public void Clear()
        {
            objects.Clear();

            for (int i = 0; i < nodes.Length; i++)
            {
                if (nodes[i] != null)
                {
                    nodes[i].Clear();
                    nodes[i] = null;
                }
            }
        }

        /// <summary>
        /// Divide a node into four subnodes
        /// </summary>
        private void Split()
        {
            int subWidth = (bounds.Width / 2);
            int subHeight = (bounds.Height / 2);
            int x = bounds.X;
            int y = bounds.Y;

            nodes[0] = new Quadtree(level + 1, new Rectangle(x + subWidth, y, subWidth, subHeight));
            nodes[1] = new Quadtree(level + 1, new Rectangle(x, y, subWidth, subHeight));
            nodes[2] = new Quadtree(level + 1, new Rectangle(x, y + subHeight, subWidth, subHeight));
            nodes[3] = new Quadtree(level + 1, new Rectangle(x + subWidth, y + subHeight, subWidth, subHeight));
        }

        /// <summary>
        /// Determine which node a rectangle belongs to.
        /// </summary>
        /// <param name="rec">The rectangle to determine which node it belongs to</param>
        /// <returns>-1 if the rectangle cannot completely fit within a child node.
        /// 0, 1, 2, or 3 for each quadrant</returns>
        private int GetIndex(Rectangle rec)
        {
            int index = -1;
            float verticalMidpoint = bounds.X + ((float)bounds.Width / 2);
            float horizontalMidpoint = bounds.Y + ((float)bounds.Height / 2);

            // Can it fit in the top or bottom quadrant completely?
            bool topQuadrant = (rec.Y < horizontalMidpoint && rec.Y + rec.Height < horizontalMidpoint);
            bool bottomQuadrant = (rec.Y > horizontalMidpoint);

            if (rec.X < verticalMidpoint && rec.X + rec.Width < verticalMidpoint)
            {
                if (topQuadrant)
                    index = 1;
                else if (bottomQuadrant)
                    index = 2;
            }
            else if (rec.X > verticalMidpoint)
            {
                if (topQuadrant)
                    index = 0;
                else if (bottomQuadrant)
                    index = 3;
            }

            return index;
        }

        /// <summary>
        /// Insert a rectangle into the quadtree. If the number of items in the node exceeds
        /// its capacity, split and readd all objects.
        /// </summary>
        /// <param name="rec">The rectangle to insert.</param>
        public void Insert(Rectangle rec)
        {
            if (nodes[0] != null)
            {
                int index = GetIndex(rec);
                if (index != -1)
                {
                    nodes[index].Insert(rec);
                    return;
                }
            }
            objects.Add(rec);

            // Does it need splitting?
            if (objects.Count > MAX_OBJECTS && level < MAX_LEVELS)
            {
                if (nodes[0] == null)
                    Split();
                int i = 0;
                while (i < objects.Count)
                {
                    int index = GetIndex(objects[i]);
                    if (index != -1)
                    {
                        nodes[index].Insert(objects[i]);
                        objects.Remove(objects[i]);
                    }
                    else
                        i++;
                }
            }
        }

        /// <summary>
        /// Return all rectangles near a given rectangle
        /// </summary>
        /// <param name="returnObjects">A list of rectangles that could possibly be collided against.</param>
        /// <param name="rec">The rectangle used as a reference to collide against all others.</param>
        public void Retrieve(ref List<Rectangle> returnObjects, Rectangle rec)
        {
            int index = GetIndex(rec);
            if (index != -1 && nodes[0] != null)
            {
                nodes[index].Retrieve(ref returnObjects, rec);
            }

            returnObjects.AddRange(objects);
        }
    }
}
