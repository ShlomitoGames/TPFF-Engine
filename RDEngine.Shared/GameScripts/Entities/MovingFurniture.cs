using Microsoft.Xna.Framework;
using RDEngine.Engine;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace RDEngine.GameScripts
{
    public class MovingFurniture : Furniture
    {
        private bool _moveX, _moveY;

        public MovingFurniture(bool moveX, bool moveY)
        {
            _moveX = moveX;
            _moveY = moveY;
        }

        public override void Start()
        {
            base.Start();

            _rb.Size -= Vector2.One;
        }

        public override void Update()
        {
            base.Update();
            float posX = (_moveX) ? (_anim.Floats[0] * Parent.Scene.UnitSize) : Parent.AbsolutePos.X;
            float posY = (_moveY) ? (_anim.Floats[1] * Parent.Scene.UnitSize) : Parent.AbsolutePos.Y;
            _rb.Position = new Vector2(posX, posY);
        }
    }
}
