using Microsoft.Xna.Framework;
using RDEngine.Engine;
using RDEngine.Engine.UI;
using RDEngine.Engine.Animation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework.Media;

namespace RDEngine.GameScripts.Scenes
{
    internal class SplashScreen : Scene
    {
        private UIObject _title1, _title2, _asterisk, _disclaimer, _disclaimer2;

        private Animator _anim;

        public SplashScreen() : base(new Color(0x10,0x10,0x10,0xff))
        {
            Song = ContentStorer.Songs["Ambient"];
        }

        public override void Initialize()
        {
            base.Initialize();

            _title1 = new TextObject("title", ContentStorer.Fonts["Pixel"], "Made with", Vector2.Zero, false)
            {
                Scale = Vector2.One * 0.75f,
                CCPosition = new Vector2(0f, -25f)
            };
            _title2 = new TextObject("title2", ContentStorer.Fonts["Pixel"], "No game engine", Vector2.Zero, false)
            {
                CCPosition = new Vector2(0f, 25f)
            };
            _asterisk = new TextObject("asterisk", ContentStorer.Fonts["Pixel"], "*", Vector2.Zero, false)
            {
                Scale = Vector2.One * 0.5f,
                CCPosition = new Vector2(195f, 15f)
            };
            _disclaimer = new TextObject("disclaimer", ContentStorer.Fonts["Pixel"], "*XNI is a fork of the MonoGame framework, not a game engine.", Vector2.Zero, false)
            {
                Scale = Vector2.One * 0.35f,
                BCPosition = new Vector2(0f, -20f)
            };
            _disclaimer2 = new TextObject("disclaimer2", ContentStorer.Fonts["Pixel"], "Also technically I'm using a game engine I made myself, but I'm not using a pre-made-- you get the point.", Vector2.Zero, false)
            {
                Scale = Vector2.One * 0.35f,
                BCPosition = Vector2.Zero
            };

            _anim = new Animator(new Dictionary<string, Animation>()
            {
                {"start", new Animation(false, new AnimLayer[]
                    {
                        new AnimLayer(new Tuple<int, int>[]
                        {
                            new Tuple<int, int>(500, 0),
                            new Tuple<int, int>(0, 255)
                        }, 0),
                        new AnimLayer(new Tuple<int, int>[]
                        {
                            new Tuple<int, int>(750, 0),
                            new Tuple<int, int>(500, 0),
                            new Tuple<int, int>(0, 255),
                        }, 1),
                        new AnimLayer(new Tuple<int, int>[]
                        {
                            new Tuple<int, int>(1500, 0),
                            new Tuple<int, int>(100, 0),
                            new Tuple<int, int>(0, 100),
                        }, 2)
                    })
                }
            }, "start", ints: new int[3]);

            AddGameObjects(new GameObject[]
            {
                _title1,
                _title2,
                _asterisk,
                _disclaimer,
                _disclaimer2,

                new UIObject("Animator", null, Vector2.Zero, false, new List<GComponent>()
                {
                    _anim
                }),

                new UIObject("Fade", ContentStorer.WhitePixel, Vector2.Zero, false, new List<GComponent>()
                {
                    new Fade(false)
                })
                {
                    Scale = RDEGame.UpscaledScrSize,
                    CCPosition = Vector2.Zero,
                    LayerDepth = 0.9f,
                    Color = Color.Black
                }
            });
        }

        public override void Start()
        {
            base.Start();

            if (SceneHandler.ActiveSong != Song && PersistentVars.MusicPlaying)
                SceneHandler.PlaySong(Song, true);
            MediaPlayer.Volume = 0.05f;
        }

        private float _time = 0;
        public override void Update()
        {
            _title1.Color = new Color(1, 1, 1) * _anim.Ints[0];
            _title2.Color = new Color(1, 1, 1) * _anim.Ints[1];
            _asterisk.Color = new Color(1, 1, 1) * _anim.Ints[2];
            _disclaimer.Color = new Color(1, 1, 1) * _anim.Ints[2];
            _disclaimer2.Color = new Color(1, 1, 1) * _anim.Ints[2];

            if (_time >= 5f)
            {
                Scene scene;
#if BLAZORGL
                scene = new WebDisclaimer();
#else
                scene = new Intro();
#endif
                FindWithTag("Fade").GetComponent<Fade>().FadeOut(scene);
            }

            _time += Time.DeltaTime;
#if DEBUG
            if (Input.GetKey(Microsoft.Xna.Framework.Input.Keys.R, KeyGate.Down))
                SceneHandler.ReloadScene();
#endif
        }
    }
}
