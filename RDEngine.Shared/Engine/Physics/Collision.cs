using Microsoft.Xna.Framework;

namespace RDEngine.Engine.Physics
{
    public struct Collision
    {
        public Vector2 ContactPoint { get; }
        public Vector2 ContactNormal { get; }
        public RigidBody Rb { get; }

        public Collision(RigidBody rb, Vector2 contactPoint, Vector2 contactNormal)
        {
            Rb = rb;
            ContactPoint = contactPoint;
            ContactNormal = contactNormal;
        }
    }
}