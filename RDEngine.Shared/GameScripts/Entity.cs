using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RDEngine.Engine;
using RDEngine.Engine.Physics;
using System;
using System.Diagnostics;

namespace RDEngine.GameScripts
{
    public class Entity : GComponent
    {
        protected RigidBody _rb;

        protected float _speed;

        public Entity(float speed) : base()
        {
            _speed = speed;
        }

        public override void Start()
        {
            base.Start();

            _rb = Parent.GetComponent<RigidBody>();
        }
    }
}