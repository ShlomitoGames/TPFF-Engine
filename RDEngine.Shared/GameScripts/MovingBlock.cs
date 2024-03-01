using Microsoft.Xna.Framework;
using RDEngine.Engine;
using RDEngine.Engine.Animation;
using RDEngine.Engine.Physics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace RDEngine.GameScripts
{
    internal class MovingBlock : GComponent
    {
        private Animator _anim;
        private RigidBody _rb;

        public override void Start()
        {
            _anim = Parent.GetComponent<Animator>();
            _rb = Parent.GetComponent<RigidBody>();
        }
        public override void Update()
        {
            float pos = _anim.Floats[0] * 16;
            _rb.Position = new Vector2(pos, Parent.Position.Y);
        }
    }
}
