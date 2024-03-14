using Microsoft.Xna.Framework;
using RDEngine.Engine;
using RDEngine.Engine.UI;
using RDEngine.Engine.Animation;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Media;

namespace RDEngine.GameScripts.Scenes
{
    internal class Title : Scene
    {
        private UIObject _title1, _title2, _title3;

        private Animator _anim, _anim2;

        public Title() : base(new Color(0x10, 0x10, 0x10, 0xff))
        {
            Song = ContentStorer.Songs["CreepingGhoul"];
        }

        public override void Initialize()
        {
            base.Initialize();

            AddGameObject
            (
                new WorldObject("Level", null, new Vector2(0f, 10f), new List<GComponent>()
                {
                    new LayoutLoader("ThxLayout")
                })
            );

            _title1 = new TextObject("title", ContentStorer.Fonts["PixelBig"], "The Phantom\nFurniture Frenzy", Vector2.Zero, false)
            {
                Color = new Color(0xf6, 0xcd, 0x26),
                Scale = Vector2.One * 0.75f,
                CCPosition = new Vector2(0f, -25f),
            };
            _title2 = new TextObject("title2", ContentStorer.Fonts["Pixel"], "By Shlomito", Vector2.Zero, false)
            {
                Scale = Vector2.One * 1f,
                CCPosition = new Vector2(0f, 120f)
            };
            _title3 = new TextObject("title3", ContentStorer.Fonts["Pixel"], "[C] Play", Vector2.Zero, false)
            {
                Scale = Vector2.One * 1.25f,
                CCPosition = new Vector2(0f, 250f)
            };

            _anim = new Animator(new Dictionary<string, Animation>()
            {
                {"start", new Animation(false, new AnimLayer[]
                    {
                        new AnimLayer(new Tuple<int, float>[]
                        {
                            new Tuple<int, float>(1000, 0f),
                            new Tuple<int, float>(1000, 0f),
                            new Tuple<int, float>(0, 1f)
                        }, 0),
                        new AnimLayer(new Tuple<int, int>[]
                        {
                            new Tuple<int, int>(2500, 0),
                            new Tuple<int, int>(500, 0),
                            new Tuple<int, int>(0, 255),
                        }, 0),
                        new AnimLayer(new Tuple<int, int>[]
                        {
                            new Tuple<int, int>(4500, 0),
                            new Tuple<int, int>(500, 0),
                            new Tuple<int, int>(0, 255),
                        }, 1),
                        new AnimLayer(new Tuple<int, bool>[]
                        {
                            new Tuple<int, bool>(2750, false),
                            new Tuple<int, bool>(100, true)
                        }, 0),
                        new AnimLayer(new Tuple<int, bool>[]
                        {
                            new Tuple<int, bool>(5000, false),
                            new Tuple<int, bool>(100, true)
                        }, 1),
                    })
                }
            }, "start", ints: new int[3], floats: new float[1], bools: new bool[2]);

            _anim2 = new Animator(new Dictionary<string, Animation>()
            {
                {"move", new Animation(true, new AnimLayer[]
                    {
                        new AnimLayer(new Tuple<int, int>[]
                        {
                            new Tuple<int, int>(1000, -25),
                            new Tuple<int, int>(1000, -100),
                            new Tuple<int, int>(0, -25)
                        }, 0)
                    })
                }
            }, "move", ints: new int[1])
            {
                Enabled = false
            };

            AddGameObjects(new GameObject[]
            {
                _title1,
                _title2,
                _title3,

                new UIObject("Animator", null, Vector2.Zero, false, new List<GComponent>()
                {
                    _anim, _anim2
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
            MediaPlayer.Volume = 0.2f;
        }

        public override void Update()
        {
            _title1.Color = new Color(0xf6, 0xcd, 0x26) * _anim.Floats[0];
            _title2.Color = new Color(1, 1, 1, 1) * _anim.Ints[0];
            _title3.Color = new Color(1, 1, 1, 1) * _anim.Ints[1];

            if (_anim.Bools[0])
            {
                _anim2.Enabled = true;
                _title1.CCPosition = new Vector2(0f, _anim2.Ints[0]);
            }

            if (_anim.Bools[1] && Input.GetKey(Microsoft.Xna.Framework.Input.Keys.C, KeyGate.Down))
            {
                Scene scene = new Intro();
                FindWithTag("Fade").GetComponent<Fade>().FadeOut(scene);
            }
#if DEBUG
            if (Input.GetKey(Microsoft.Xna.Framework.Input.Keys.R, KeyGate.Down))
                SceneHandler.ReloadScene();
#endif
        }
    }
}
