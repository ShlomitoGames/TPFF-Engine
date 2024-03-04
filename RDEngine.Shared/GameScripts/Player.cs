using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RDEngine.Engine;
using RDEngine.Engine.Animation;
using RDEngine.Engine.Physics;
using System.Diagnostics;

namespace RDEngine.GameScripts
{
    public class Player : GComponent, ICollideable
    {
        public int Speed;
        private RigidBody _rb;

        public Player(int speed)
        {
            Speed = speed;
        }

        public override void Start()
        {
            _rb = Parent.GetComponent<RigidBody>();
        }
        int oldcount = 0;
        public override void Update()
        {
            Vector2 velocity = Vector2.Zero;

            if (Input.GetMacro("Up", KeyGate.Held))
            {
                velocity -= Vector2.UnitY;
            }
            if (Input.GetMacro("Down", KeyGate.Held))
            {
                velocity += Vector2.UnitY;
            }
            if (Input.GetMacro("Left", KeyGate.Held))
            {
                velocity -= Vector2.UnitX;
            }
            if (Input.GetMacro("Right", KeyGate.Held))
            {
                velocity += Vector2.UnitX;
            }

            if (velocity != Vector2.Zero)
            {
                velocity.Normalize();
                //_rb.Velocity = velocity * Speed;
                _rb.Accelerate(velocity * Speed);
            }
        }
    }
}