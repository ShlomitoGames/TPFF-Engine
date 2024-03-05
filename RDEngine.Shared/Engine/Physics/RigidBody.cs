using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;

namespace RDEngine.Engine.Physics
{
    public class RigidBody: GComponent
    {
        public bool IsStatic, IsTrigger, IsKinematic;

        public Vector2 Offset { get; set; }
        public Vector2 Position
        {
            get
            {
                return Parent.AbsolutePos - Offset - _halfSize;
            }
            set
            {
                _positionAlter = value - Offset - _halfSize;
            }
        }

        private Vector2? _positionAlter = null;

        private Vector2 _size;
        private Vector2 _halfSize;
        public Vector2 Size
        {
            get
            {
                return _size;
            }
            set
            {
                _size = value;
                _halfSize = value / 2f;
            }
        }

        public RectF Rect
        {
            get
            {
                return new RectF(Position, Size);
            }
            set
            {
                Position = value.Position;
                Size = value.Size;
            }
        }
        private float _mass;
        public float Mass
        {
            get
            {
                return _mass;
            }
            set
            {
                if (value <= 0)
                    throw new Exception("Mass value has to be positive");
                _mass = value;
            }
        }
        public float Gravity { get; set; }
        public float Drag { get; set; }

        public Vector2 Velocity;

        private Vector2 _acceleration;

        private List<Collision> _cols, _lastCols;
        private List<RigidBody> _triggIntrs, _lastIntersects;

        public RigidBody(Vector2 size, Vector2 offset, bool isTrigger = false, bool isStatic = false, bool isKinematic = false, float gravity = 0, float drag = 0, float mass = 1): base()
        {
            Size = size;
            Offset = offset;
            IsStatic = isStatic;
            IsTrigger = isTrigger;
            IsKinematic = isKinematic;
            Gravity = gravity;
            Drag = drag; //Not frame-rate independent
            Mass = mass;

            _cols = new List<Collision>();
            _lastCols = new List<Collision>();
            _triggIntrs = new List<RigidBody>();
            _lastIntersects = new List<RigidBody>();
        }

        public override void SetParent(GameObject parent)
        {
            base.SetParent(parent);

            if (parent as WorldObject == null)
                throw new Exception("RigidBody parent must be a WorldObject");
        }

        public override void Start()
        {
            Parent.Scene.AddRb(this);
        }

        internal void UpdatePosition(float deltaTime)
        {
            Parent.AbsolutePos += Velocity * deltaTime;

            _acceleration = Vector2.Zero;

            if (_positionAlter != null)
                Velocity = Vector2.Zero;
            _positionAlter = null;
        }

        internal void UpdateVelocity(float deltaTime)
        {
            Accelerate(Vector2.UnitY * Gravity);
            
            Velocity += _acceleration * deltaTime;

            if (_positionAlter != null && deltaTime != 0)
            {
                Velocity = ((Vector2)_positionAlter - Position) / deltaTime;
            }

            //This is *not* frame-rate independent!
            float multiplier = 1 - (Drag * deltaTime);
            if (multiplier > 0)
            {
                Velocity *= multiplier;
            }
        }

        public void Accelerate(Vector2 acceleration)
        {
            _acceleration += acceleration;
        }

        internal void AddCollision(Collision collision)
        {
            _cols.Add(collision);
        }

        internal void AddIntersection(RigidBody triggerIntersection)
        {
            _triggIntrs.Add(triggerIntersection);
        }

        internal void UpdateCollisions() //Calls all OnCollision functions
        {
            var wParent = Parent as WorldObject;

            List<RigidBody> stayed = _cols.ConvertAll(c => c.Rb).Intersect(_lastCols.ConvertAll(c => c.Rb)).ToList();

            foreach (var col in _lastCols)
            {
                if (!stayed.Contains(col.Rb))
                    wParent.OnCollisionExit(col);
            }
            foreach (var col in _cols)
            {
                if (stayed.Contains(col.Rb))
                    wParent.OnCollisionStay(col);
                else
                    wParent.OnCollisionEnter(col);
            }

            _lastCols = new List<Collision>(_cols);
            _cols = new List<Collision>();
        }

        internal void UpdateTriggerIntersections()
        {
            var wParent = Parent as WorldObject;

            List<RigidBody> intrStayed = _triggIntrs.Intersect(_lastIntersects).ToList();

            foreach (var intrRb in _lastIntersects)
            {
                if (!intrStayed.Contains(intrRb))
                    wParent.OnTriggerExit(intrRb);
            }
            foreach (var intrRb in _triggIntrs)
            {
                if (intrStayed.Contains(intrRb))
                    wParent.OnTriggerStay(intrRb);
                else
                    wParent.OnTriggerEnter(intrRb);
            }

            _lastIntersects = new List<RigidBody>(_triggIntrs);
            _triggIntrs = new List<RigidBody>();
        }

        public Collision? RayCast(Vector2 origin, Vector2 direction, float maxDistance, bool skipTriggers)
        {
            Collision? col = null;
            float minDist = -1;
            foreach (var rb in Parent.Scene.Solver.RigidBodies)
            {
                if (rb == this || (rb.IsTrigger && skipTriggers)) continue;

                Vector2 cp, cn;
                float ct;
                if (PhysicsSolver.RayVsRect(origin, direction, rb.Rect, out cp, out cn, out ct, true))
                {
                    float dist = Vector2.Distance(origin, cp);

                    if (dist > maxDistance)
                        continue;

                    if (dist < minDist || minDist == -1)
                    {
                        minDist = dist;
                        col = new Collision(rb, cp, cn);
                    }
                }
            }
            return col;
        }
        public bool BoxCast(RectF box)
        {
            foreach (var rb in Parent.Scene.Solver.RigidBodies)
            {
                if (rb == this) continue;

                if (PhysicsSolver.RectIntersectsRect(box, rb.Rect))
                    return true;
            }
            return false;
        }

        public override void Draw(GraphicsDevice graphics, SpriteBatch spriteBatch)
        {
            Vector2 drawSize = Size * RDEGame.ScaleFactor;
            int borderWidth = 1;

            Color color = (IsTrigger) ? Color.Gold : Color.LightGreen;
            Vector2 offset = Position * RDEGame.ScaleFactor - Vector2.Floor(Parent.Scene.CameraPos);

            for (int y = 0; y < drawSize.Y; y++)
            {
                for (int x = 0; x < drawSize.X; x++)
                {
                    if (x < borderWidth || y < borderWidth || x >= drawSize.X - borderWidth || y >= drawSize.Y - borderWidth)
                    {
                        Vector2 drawPos = new Vector2(x, y) + offset;
                        if (drawPos.X < RDEGame.UpscaledScrWidth && drawPos.Y < RDEGame.UpscaledScrHeight)
                            spriteBatch.Draw(ContentStorer.WhitePixel, drawPos, color);
                    }
                }
            }
        }

        public override void Remove()
        {
            Parent.Scene.RemoveRb(this);
            base.Remove();
        }
    }
}