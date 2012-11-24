using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace SleepwalkerEngine
{
    public class CollisionResolver
    {
        public List<SceneNode> Colliders;

        /// <summary>
        /// Initialize the collision resolver
        /// </summary>
        /// <param name="colliders">The list of objects in the scene, excluding the main object.</param>
        public CollisionResolver(List<SceneNode> colliders)
        {
            Colliders = colliders;
        }

        public Vector2 ResolveCollisions(Vector2 velocity, Rectangle sn1, Rectangle sn2)
        {

            Vector2 xyPenetrationDepth = Vector2.Zero;
            // if there is a collision
            if (sn1.Intersects(sn2))
            {
                Rectangle testRect = sn1;
                //check x
                if (Math.Abs(velocity.X) > 0)
                {
                    // while it is still intersecting
                    while (testRect.Intersects(sn2))
                    {
                        // move it away 1 at a time until it doesn't touch
                        testRect.X -= Math.Sign(velocity.X);

                        // accumulate total x movement needed
                        xyPenetrationDepth.X -= Math.Sign(velocity.X);
                    }

                }

                //check y
                if (Math.Abs(velocity.Y) > 0)
                {
                    testRect = sn1;
                    while (testRect.Intersects(sn2))
                    {
                        testRect.Y -= Math.Sign(velocity.Y);
                        xyPenetrationDepth.Y -= Math.Sign(velocity.Y);
                    }
                }
            }

            // if it intersects both x and y
            if (xyPenetrationDepth.X != 0 && xyPenetrationDepth.Y != 0)
            {

                // which is larger?
                if (Math.Abs(xyPenetrationDepth.X) > Math.Abs(xyPenetrationDepth.Y))
                {

                    // move to the smaller one (Y in this case)
                    return new Vector2(0, xyPenetrationDepth.Y);
                }
                else if (Math.Abs(xyPenetrationDepth.X) == Math.Abs(xyPenetrationDepth.Y))
                {
                    return new Vector2(xyPenetrationDepth.Y, 0);
                }
                else
                {
                    // move to the smaller one (X in this case)
                    return new Vector2(xyPenetrationDepth.X, 0);
                }
            }
            else
            {
                // if just one axis, just move
                return new Vector2(xyPenetrationDepth.X, xyPenetrationDepth.Y);
            }
        }
    }
}
