using Microsoft.Xna.Framework;
using RDEngine.Engine;
using RDEngine.Engine.Physics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace RDEngine.GameScripts
{
    internal class FollowingFurniture : Furniture
    {
        protected WorldObject _target;
        protected float _radius;
        protected float _speed;

        private bool _found, _startedMoving;
        private float _delay, _time;

        public FollowingFurniture(float radius, float speed)
        {
            _radius = radius;
            _speed = speed;
            _found = false;
            _startedMoving = false;
            _delay = 0.5f;
            _time = 0;
        }

        public override void Start()
        {
            base.Start();

            _target = Parent.Scene.FindWithTag("Player") as WorldObject;
        }

        public override void Update()
        {
            base.Update();

            Vector2 dir = Vector2.Normalize(_target.AbsolutePos - Parent.AbsolutePos);

            //RayCasts for the first solid object, looking for the player
            Collision? colPlayer = _rb.RayCast(Parent.AbsolutePos, dir, _radius * Parent.Scene.UnitSize, true);
            //RayCasts for whatever might be in the way, even if it's a trigger, to get the contact normal
            Collision? colOther = _rb.RayCast(Parent.AbsolutePos, dir, _radius * Parent.Scene.UnitSize, false);

            //Resets velocity
            _rb.Velocity = Vector2.Zero;

            bool wasFound = _found;
            _found = false;
            if (colPlayer != null)
            {
                //Sees if it has direct line of sight to the player
                if (colPlayer.Value.Rb.Parent.Tag == "Player")
                {
                    _found = true;
                    //The first frame it fount it it plays an animation and waits for a bit
                    if (!wasFound)
                    {
                        //animation
                    }
                }
            }

            if (_found)
            {
                if (!_startedMoving)
                {
                    _time += Time.DeltaTime;
                    if (_time >= _delay)
                    {
                        _time = 0;
                        _startedMoving = true;
                    }
                }
                else
                {
                    //It finished waiting so it moves towards the player stopping onlt when it's almost fully inside it
                    if (Vector2.Distance(Parent.Position, _target.Position) > 1f)
                        _rb.Velocity = dir * _speed;
                }
            }
            if (colOther != null)
            {
                //RayCasts again with a smaller radius just to see if it's going to collide with something or enter a Rug
                colOther = _rb.RayCast(Parent.AbsolutePos, -colOther.Value.ContactNormal, _rb.Size.X * 0.5f, false);
                if (colOther != null)
                {
                    if (colOther.Value.Rb.Parent.Tag != "Player")
                    {
                        //Stops it's movement along the axis of collision, so it can still move in the other
                        if (colOther.Value.ContactNormal.X != 0)
                            _rb.Velocity = new Vector2(0f, _rb.Velocity.Y);
                        else
                            _rb.Velocity = new Vector2(_rb.Velocity.X, 0f);
                    }
                }
            }
        }
    }
}
