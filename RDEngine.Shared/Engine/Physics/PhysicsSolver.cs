using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace RDEngine.Engine.Physics
{
    public class PhysicsSolver
    {
        private const int SUBSTEPS = 5;
        
        public List<RigidBody> RigidBodies;

        public PhysicsSolver()
        {
            RigidBodies = new List<RigidBody>();
        }

        public void Update()
        {
            float deltaTime = Time.DeltaTime;

            //First we get the velocity to that we can make the collision detection and resolution,
            //at the end we update the position based on the velocity
            UpdateVelocities(deltaTime);
            for (int i = 0; i < SUBSTEPS; i++) //I'll probably want to remove this entirely eventually
            {
                SolveCollisions(deltaTime, i == 0);
            }
            UpdatePositions(deltaTime);
            SolveTriggers();

            SendCollisions();
        }

        private void UpdateVelocities(float deltaTime)
        {
            foreach (var rb in RigidBodies)
            {
                rb.UpdateVelocity(deltaTime);
            }
        }

        private void UpdatePositions(float deltaTime)
        {
            foreach (var rb in RigidBodies)
            {
                rb.UpdatePosition(deltaTime);
                //rb.Position += rb.Velocity;
            }
        }

        private void SendCollisions()
        {
            foreach (var rb in RigidBodies)
            {
                rb.UpdateCollisions();
                rb.UpdateTriggerIntersections();
            }
        }

        //https://www.youtube.com/watch?v=8JJ-4JgR7Dg
        //I am *not* smart enough to have come up with this myself.
        private void SolveCollisions(float deltaTime, bool storeCols)
        {
            Vector2 contactPoint, contactNormal;
            float contactTime;

            foreach (var rb1 in RigidBodies)
            {
                if (rb1.IsStatic || rb1.IsTrigger) continue;

                List<Tuple<int, float>> cols = new List<Tuple<int, float>>();

                for (int i = 0; i < RigidBodies.Count; i++)
                {
                    var rb2 = RigidBodies[i];

                    if (rb1 == rb2 || rb2.IsTrigger) continue;

                    if (DynamicRectVsRect(rb1.Rect, rb2.Rect, rb1.Velocity, deltaTime, out contactPoint, out contactNormal, out contactTime))
                    {
                        cols.Add(new Tuple<int, float>(i, contactTime));
                    }
                }

                //It sorts the collisions by their colTime to evaluate collisions properly
                var orderedCols = cols.OrderBy(t => t.Item2);

                foreach (var col in orderedCols)
                {
                    var rb2 = RigidBodies[col.Item1];

                    //Does the collision detection again, but in order
                    RectF target = rb2.Rect;
                    target.Position += rb2.Velocity * deltaTime;
                    if (DynamicRectVsRect(rb1.Rect, target, rb1.Velocity, deltaTime, out contactPoint, out contactNormal, out contactTime))
                    {
                        Vector2 newVel = contactNormal * new Vector2(MathF.Abs(rb1.Velocity.X), MathF.Abs(rb1.Velocity.Y)) * (1 - contactTime);

                        float ratio1 = (!rb2.IsStatic && !rb2.IsKinematic) ? rb2.Mass / (rb1.Mass + rb2.Mass) : 1;

                        rb1.Velocity += newVel * ratio1;
                        if (storeCols) rb1.AddCollision(new Collision(rb2, contactPoint, contactNormal));

                        if (!rb2.IsStatic && !rb2.IsKinematic)
                            rb2.Velocity -= newVel * (rb1.Mass / (rb1.Mass + rb2.Mass));
                        if (storeCols) rb2.AddCollision(new Collision(rb1, contactPoint, -contactNormal));
                    }
                }
            }
        }

        private void SolveTriggers()
        {
            foreach (var rb1 in RigidBodies)
            {
                if (!rb1.IsTrigger) continue;

                foreach (var rb2 in RigidBodies)
                {
                    if (rb1 == rb2) continue;

                    if (RectIntersectsRect(rb1.Rect, rb2.Rect))
                    {
                        rb1.AddIntersection(rb2);
                    }
                }
            }
        }
        
        //rayDirection should be passed in relation to rayOrigin
        public static bool RayVsRect(Vector2 rayOrigin, Vector2 rayDirection, RectF rect,
            out Vector2 contactPoint, out Vector2 contactNormal, out float tHitNear, bool infinite = false)
        {
            contactPoint = Vector2.Zero;
            contactNormal = Vector2.Zero;
            tHitNear = 0;

            if (rayDirection.X == 0) rayDirection.X = float.Epsilon;
            if (rayDirection.Y == 0) rayDirection.Y = float.Epsilon;

            Vector2 tNear;
            Vector2 tFar;

            //It calculates the time (value 0 - 1 along the origin to the direction) where the Ray intersects the sides of the Rect
            //in X and Y, which can happen outside of the Rect itself.
            tNear = (rect.Position - rayOrigin) / rayDirection;
            tFar = (rect.Position + rect.Size - rayOrigin) / rayDirection;

            if (tNear.X > tFar.X) Utils.Swap(ref tNear.X, ref tFar.X);
            if (tNear.Y > tFar.Y) Utils.Swap(ref tNear.Y, ref tFar.Y);

            //This checks if the Ray actually intersects the Rect
            if (tNear.X > tFar.Y || tNear.Y > tFar.X) return false;

            tHitNear = MathF.Max(tNear.X, tNear.Y);
            float tHitFar = MathF.Min(tFar.X, tFar.Y);

            //If the time is less than 0 or greater than 1 a collision is along the direction of the Ray but not in the Ray itself
            if (tHitFar < 0 || tHitNear < 0) return false;
            if (tHitNear > 1 && !infinite) return false;

            //Get the point based on the time
            contactPoint = rayOrigin + rayDirection * tHitNear;

            //Get the normal, or "side" of the Rect it collided in
            if (tNear.X > tNear.Y)
            {
                if (rayDirection.X < 0)
                    contactNormal = Vector2.UnitX;
                else
                    contactNormal = -Vector2.UnitX;
            }
            else if (tNear.X < tNear.Y)
            {
                if (rayDirection.Y < 0)
                    contactNormal = Vector2.UnitY;
                else
                    contactNormal = -Vector2.UnitY;
            }

            return true;
        }

        //Does not work well when both Rects are moving
        private bool DynamicRectVsRect(RectF source, RectF target, Vector2 velocity, float deltaTime,
            out Vector2 contactPoint, out Vector2 contactNormal, out float contactTime)
        {
            contactPoint = Vector2.Zero;
            contactNormal = Vector2.Zero;
            contactTime = 0;

            if (velocity == Vector2.Zero)
                return false;

            Vector2 halfSize = source.Size / 2;

            //Expand the target by giving it a border the size of half the size of the source,
            //if the Ray intersects this rectangle then the source Rect would too
            RectF expandedTarget = new RectF();
            expandedTarget.Position = target.Position - halfSize;
            expandedTarget.Size = target.Size + source.Size;

            if (RayVsRect(source.Position + halfSize, velocity * deltaTime, expandedTarget, out contactPoint, out contactNormal, out contactTime))
            {
                return true;
            }
            return false;
        }

        public static bool RectIntersectsRect(RectF source, RectF target)
        {
            return !(target.Position.X > source.Position.X + source.Size.X ||
                target.Position.X + target.Size.X < source.Position.X ||
                target.Position.Y > source.Position.Y + source.Size.Y ||
                target.Position.Y + target.Size.Y < source.Position.Y);

            /*return !(r2.left > r1.right ||
            r2.right < r1.left ||
            r2.top > r1.bottom ||
            r2.bottom < r1.top);*/
        }
    }
}