using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using RDEngine.Engine;
using RDEngine.Engine.Animation;
using RDEngine.Engine.Physics;
using RDEngine.GameScripts.Scenes;
using System.Diagnostics;

namespace RDEngine.GameScripts
{
    public class Player : GComponent, ICollideable
    {
        public int Speed;
        private RigidBody _rb;
        private Animator _anim;
        private bool _debug = false;
        private bool _dead = false;

        public bool Cutscene;

        private SoundEffect _restart, _hardcoreRestart;

        public Player(int speed)
        {
            Speed = speed;
            Cutscene = false;
        }

        public override void Start()
        {
            _rb = Parent.GetComponent<RigidBody>();
            _anim = Parent.GetComponent<Animator>();


            _restart = ContentStorer.SFX["Thud"];
            _hardcoreRestart = ContentStorer.SFX["SpringyThud"];
        }
        public override void Update()
        {
            Vector2 velocity = Vector2.Zero;

            Parent.Texture = _anim.Textures[0];

            if (_dead) return;

            if (_rb.Velocity.Length() > Speed * 0.5f || (Cutscene && _rb.Velocity.Length() > 1))
                _anim.SetAnimation("walk");
            else
                _anim.SetAnimation("idle");

            if (!Cutscene)
            {
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
                    Parent.Effects = SpriteEffects.FlipHorizontally;
                }
                if (Input.GetMacro("Right", KeyGate.Held))
                {
                    velocity += Vector2.UnitX;
                    Parent.Effects = SpriteEffects.None;
                }

                if (velocity != Vector2.Zero)
                {
                    velocity.Normalize();
                    _rb.Velocity = velocity * Speed;
                }
            }

#if DEBUG
            _debug = Input.GetKey(Microsoft.Xna.Framework.Input.Keys.F, KeyGate.Held);
            _rb.IsTrigger = _debug;
#endif
        }

        void ICollideable.OnTriggerEnter(RigidBody intrRb)
        {
            if (Cutscene || _debug)
                return;

            if (intrRb.Parent.Tag == "FurnitureTrigger")
            {
                Restart();
            }
            else if (intrRb.Parent.Tag.StartsWith("OOB"))
            {
                if (!PersistentVars.OnOutro)
                    Restart();
                else
                {
                    Scene scene;
                    switch (PersistentVars.CurrLevel)
                    {
                        case 3:
                            scene = new Level2End();
                            break;
                        case 2:
                            scene = new Level1End();
                            break;
                        case 1:
                            scene = new IntroEnd();
                            break;
                        case 0:
                            scene = new ThanksForPlaying();
                            break;
                        default:
                            scene = new SplashScreen();
                            break;
                    }

                    Parent.Scene.FindWithTag("Fade").GetComponent<Fade>().FadeOut(scene);
                }
            }
            else if (intrRb.Parent.Tag == "End")
            {
                Scene scene;
                switch (PersistentVars.CurrLevel)
                {
                    case 0:
                        scene = new Level1();
                        break;
                    case 1:
                        scene = new Level2();
                        break;
                    case 2:
                        scene = new Level3();
                        break;
                    case 3:
                        scene = new End();
                        break;
                    case 4:
                        scene = new Level3End();
                        break;
                    default:
                        scene = new SplashScreen();
                        break;
                }

                Parent.Scene.FindWithTag("Fade").GetComponent<Fade>().FadeOut(scene);
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
                Parent.Scene.FindWithTag("Fade").GetComponent<Fade>().FadeOut(new Level1());
            }
            else
            {
                _restart.Play();
                SceneHandler.ReloadScene();
            }
        }
    }
}