using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
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
        private bool _debug = false;
        private bool _dead = false;

        private SoundEffect _restart, _hardcoreRestart;

        public Player(int speed)
        {
            Speed = speed;
        }

        public override void Start()
        {
            _rb = Parent.GetComponent<RigidBody>();


            _restart = ContentStorer.SFX["Thud"];
            _hardcoreRestart = ContentStorer.SFX["SpringyThud"];
        }
        public override void Update()
        {
            Vector2 velocity = Vector2.Zero;

            if (_dead) return;

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

#if DEBUG
            _debug = Input.GetKey(Microsoft.Xna.Framework.Input.Keys.F, KeyGate.Held);
            _rb.IsTrigger = _debug;
#endif
        }

        void ICollideable.OnTriggerEnter(RigidBody intrRb)
        {
            if (_debug)
                return;

            if (intrRb.Parent.Tag == "FurnitureTrigger")
            {
                Restart();
            }
            else if (intrRb.Parent.Tag.StartsWith("OOB"))
            {
                Restart();
            }
        }

        public void Restart()
        {
            if (_dead) return;

            _dead = true;

            if (PersistentVars.Hardcore)
            {
                _hardcoreRestart.Play();
                MediaPlayer.Stop();
                Parent.Scene.FindWithTag("Fade").GetComponent<Fade>().FadeOutRestart();
            }
            else
            {
                _restart.Play();
                SceneHandler.ReloadScene();
            }
        }
    }
}