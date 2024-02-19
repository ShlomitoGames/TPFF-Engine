using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RDEngine.Engine;
using RDEngine.Engine.Physics;
using System.Diagnostics;

namespace RDEngine.GameScripts
{
    public class Player : Entity, ICollideable
    {
        private float _jumpSpeed;

        private bool _grounded;

        public Player(float speed, float jumpSpeed) : base(speed)
        {
            _jumpSpeed = jumpSpeed;
        }

        public override void Start()
        {
            base.Start();
        }

        public override void Update()
        {
            base.Update();

            if (Input.Instance.GetMacro("Right", KeyGate.Held))
                _rb.Velocity.X = _speed;
            else if (Input.Instance.GetMacro("Left", KeyGate.Held))
                _rb.Velocity.X = -_speed;
            else
                _rb.Velocity.X = 0;

            if (Input.Instance.GetMacro("Jump", KeyGate.Down) && _grounded)
            {
                //_rb.Accelerate(-7000 * Vector2.UnitY);
                _rb.Velocity.Y = -_jumpSpeed;
            }

            _rb.IsTrigger = Input.Instance.GetKey(Microsoft.Xna.Framework.Input.Keys.F, KeyGate.Held);


            GetGrounded();
        }

        private void GetGrounded()
        {
            //Collision? col = _rb.Raycast(_rb.Origin, Vector2.UnitY, _rb.Size.Y / 2);
            //Collision? col = _rb.Boxcast(new RectF(_rb.Position + (_rb.Size.Y - 1) * Vector2.UnitY, new Vector2(_rb.Size.X, 1)), Vector2.UnitY, 2);

            _grounded = _rb.BoxCast(new RectF(new Vector2(_rb.Position.X, _rb.Position.Y + _rb.Size.Y), new Vector2(_rb.Size.X, 1)));
        }
    }
}