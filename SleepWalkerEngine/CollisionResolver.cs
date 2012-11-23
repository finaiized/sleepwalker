using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace SleepwalkerEngine
{
    public class CollisionResolver
    {
        public List<SceneNode> Colliders = new List<SceneNode>();

        public Vector2 ResolveCollisions(Vector2 velocity, SceneNode sn1, SceneNode sn2)
        {

            Vector2 xyPenetrationDepth = Vector2.Zero;
            // if there is a collision
            if (sn1.Rectangle.Intersects(sn2.Rectangle))
            {
                Rectangle testRect = sn1.Rectangle;
                //check x
                if (Math.Abs(velocity.X) > 0)
                {
                    // while it is still intersecting
                    while (testRect.Intersects(sn2.Rectangle))
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
                    testRect = sn1.Rectangle;
                    while (testRect.Intersects(sn2.Rectangle))
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
