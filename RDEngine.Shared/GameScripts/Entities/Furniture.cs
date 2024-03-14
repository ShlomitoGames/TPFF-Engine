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
    public class Furniture : GComponent
    {
        protected Animator _anim;
        protected RigidBody _rb;
        protected RigidBody _insideRb;

        public override void Start()
        {
            _anim = Parent.GetComponent<Animator>();
            _rb = Parent.GetComponent<RigidBody>();

            if (_rb.Size == Vector2.One * Parent.Scene.UnitSize)
                _insideRb = new RigidBody(_rb.Size *  0.5f, Vector2.Zero, true);
            else
                _insideRb = new RigidBody(_rb.Size * 0.75f, Vector2.Zero, true);
            WorldObject child = new WorldObject("FurnitureTrigger", null, Vector2.Zero);
            child.AddComponent(_insideRb);
            child.SetParent(Parent);
            Parent.Scene.AddGameObject(child);
        }
        public override void Update()
        {
            
        }
    }
}
