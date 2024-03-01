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
        private bool[] canMove = {true, true, true, true};

        public Player(int speed)
        {
            Speed = speed;
        }

        public override void Start()
        {
            _rb = Parent.GetComponent<RigidBody>();
        }

        public override void Update()
        {
            Vector2 velocity = Vector2.Zero;

            if (Input.GetMacro("Up", KeyGate.Held) && canMove[0])
            {
                velocity -= Vector2.UnitY * Speed;
            }
            if (Input.GetMacro("Down", KeyGate.Held) && canMove[1])
            {
                velocity += Vector2.UnitY * Speed;
            }
            if (Input.GetMacro("Left", KeyGate.Held) && canMove[2])
            {
                velocity -= Vector2.UnitX * Speed;
            }
            if (Input.GetMacro("Right", KeyGate.Held) && canMove[3])
            {
                velocity += Vector2.UnitX * Speed;
            }

            _rb.Velocity = velocity;
        }
    }
}