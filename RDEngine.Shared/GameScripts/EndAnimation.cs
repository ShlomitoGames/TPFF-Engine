using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using RDEngine.Engine;
using RDEngine.Engine.Animation;
using RDEngine.Engine.Physics;
using RDEngine.Engine.UI;
using RDEngine.GameScripts.Scenes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace RDEngine.GameScripts
{
    public class EndAnimation : GComponent, ICollideable
    {
        private WorldObject _player;
        private RigidBody _playerRb;
        private Player _playerScript;
        private UIObject _fadeWhite;//fades to white then transitions to black then to the end done scene.

        private Animator _anim;

        private bool _active;

        public override void Start()
        {
            _player = Parent.Scene.FindWithTag("Player") as WorldObject;
            _playerScript = _player.GetComponent<Player>();
            _playerRb = _player.GetComponent<RigidBody>();
            _active = false;

            _fadeWhite = new UIObject("Fade", ContentStorer.WhitePixel, Vector2.Zero, false)
            {
                Scale = RDEGame.UpscaledScrSize,
                CCPosition = Vector2.Zero,
                LayerDepth = 0.9f,
                Color = Color.Black,
                Enabled = false
            };

            Parent.Scene.AddGameObject(_fadeWhite);

            _anim = new Animator(new Dictionary<string, Animation>()
            {
                {
                    "cutscene", new Animation(false, new AnimLayer[]
                    {
                        new AnimLayer(new Tuple<int, float>[]
                        {
                            new Tuple<int, float>(1000, 0f),
                            new Tuple<int, float>(2000, 0f),
                            new Tuple<int, float>(100, 30f),
                            new Tuple<int, float>(2000, 0f),
                            new Tuple<int, float>(2000, 0f),
                            new Tuple<int, float>(2000, 40f),
                            new Tuple<int, float>(2000, 40f),
                            new Tuple<int, float>(100, 0f),
                        },0),
                        new AnimLayer(new Tuple<int, float>[]
                        {
                            new Tuple<int, float>(2000, 0.1f),
                            new Tuple<int, float>(9500, 0.1f),
                            new Tuple<int, float>(100, 1f)
                        },1),
                        new AnimLayer(new Tuple<int, bool>[]
                        {
                            new Tuple<int, bool>(15000, false),
                            new Tuple<int, bool>(100, true)
                        },0),
                        new AnimLayer(new Tuple<int, bool>[]
                        {
                            new Tuple<int, bool>(15000, false),
                            new Tuple<int, bool>(6000, false),
                            new Tuple<int, bool>(100, true),
                        },1),
                        new AnimLayer(new Tuple<int, SoundEffect>[]
                        {
                            new Tuple<int, SoundEffect>(15000, null),
                            new Tuple<int, SoundEffect>(500, ContentStorer.SFX["Explosion"]),
                            new Tuple<int, SoundEffect>(100, ContentStorer.SFX["Sigh"]),
                            new Tuple<int, SoundEffect>(100, ContentStorer.SFX["Sigh"]),
                            new Tuple<int, SoundEffect>(100, ContentStorer.SFX["Sigh"]),
                            new Tuple<int, SoundEffect>(100, ContentStorer.SFX["Sigh"]),
                            new Tuple<int, SoundEffect>(100, ContentStorer.SFX["Sigh"]),
                            new Tuple<int, SoundEffect>(100, ContentStorer.SFX["Sigh"])
                        },0),
                    })
                },
                {
                    "inactive", new Animation(false, new AnimLayer[0])
                }
            }, "inactive", floats: new float[2], bools: new bool[2]);

            Parent.AddComponent(_anim);
        }

        public override void Update()
        {
            if (!_active) return;

            _playerRb.Velocity = new Vector2(_anim.Floats[0], 0f);

            MediaPlayer.Volume = _anim.Floats[1];

            if (_anim.Bools[0])
            {
                _player.Texture = null;
                MediaPlayer.Stop();
                _fadeWhite.Enabled = true;
            }

            if (_anim.Bools[1])
            {
                PersistentVars.OnOutro = true;
                SceneHandler.LoadScene(new EndEnd());
            }
        }

        void ICollideable.OnTriggerEnter(RigidBody intrRb)
        {
            if (intrRb.Parent.Tag == "Player" && !_active)
            {
                _anim.SetAnimation("cutscene");
                _playerScript.Cutscene = true;
                _active = true;
            }
        }
    }
}
