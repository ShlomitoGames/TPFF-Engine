using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RDEngine.Engine;
using RDEngine.Engine.Animation;
using RDEngine.Engine.Physics;
using System.Diagnostics;

namespace RDEngine.GameScripts
{
    public class Player : Entity, ICollideable
    {
        private float _jumpSpeed;

        private bool _grounded;

        private Animator _anim;

        public Player(float speed, float jumpSpeed) : base(speed)
        {
            _jumpSpeed = jumpSpeed;
        }

        public override void Start()
        {
            base.Start();

            _anim = Parent.GetComponent<Animator>();
        }

        public override void Update()
        {
            base.Update();

            Parent.Texture = _anim.Textures[0];

            if (Input.GetMacro("Right", KeyGate.Held))
            {
                _rb.Velocity.X = _speed;
                Parent.Effects = SpriteEffects.None;
            }
            else if (Input.GetMacro("Left", KeyGate.Held))
            {
                _rb.Velocity.X = -_speed;
                Parent.Effects = SpriteEffects.FlipHorizontally;
            }
            else
                _rb.Velocity.X = 0;

            if (_grounded)
            {
                if (_rb.Velocity.X != 0)
                {
                    if (_anim.GetAnimName() != "walk")
                        _anim.SetAnimation("walk");
                }
                else
                    _anim.SetAnimation("stand");
            }
            else
            {
                _anim.SetAnimation("jump");
            }
            
            if (Input.GetMacro("Jump", KeyGate.Down) && _grounded)
            {
                _rb.Velocity.Y = -_jumpSpeed;
                ContentStorer.SFX["MarioJump"].Play();
            }

            _rb.IsTrigger = Input.GetKey(Microsoft.Xna.Framework.Input.Keys.F, KeyGate.Held);


            GetGrounded();
        }

        private void GetGrounded()
        {
            //Collision? col = _rb.Raycast(_rb.Origin, Vector2.UnitY, _rb.Size.Y / 2);
            //Collision? col = _rb.Boxcast(new RectF(_rb.Position + (_rb.Size.Y - 1) * Vector2.UnitY, new Vector2(_rb.Size.X, 1)), Vector2.UnitY, 2);

            _grounded = _rb.BoxCast(new RectF(new Vector2(_rb.Position.X + 0.05f, _rb.Position.Y + _rb.Size.Y), new Vector2(_rb.Size.X - 0.1f, 1)));
        }
    }
}