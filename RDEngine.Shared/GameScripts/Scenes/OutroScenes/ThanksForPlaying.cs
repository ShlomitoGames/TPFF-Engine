using Microsoft.Xna.Framework;
using RDEngine.Engine;
using RDEngine.Engine.UI;
using RDEngine.Engine.Animation;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace RDEngine.GameScripts.Scenes
{
    internal class ThanksForPlaying : Scene
    {
        private UIObject _panel;

        private Animator _anim;

        public ThanksForPlaying() : base(new Color(0x20, 0x20, 0x20, 0xff))
        {
            Song = ContentStorer.Songs["HauntedBlues"];
        }

        public override void Initialize()
        {
            base.Initialize();

            AddGameObject
            (
                new TiledTexture("BG", ContentStorer.Textures["Border"], Vector2.Zero, new Vector2(32f, 16f))
            );

            _panel = new UIObject("Panel", null, Vector2.Zero, false)
            {
                Position = new Vector2(0f, 500f)
            };

            UIObject title1 = new TextObject("title", ContentStorer.Fonts["PixelBig"], "Thanks for Playing!", Vector2.Zero, false)
            {
                Scale = Vector2.One * 0.75f,
                CCPosition = new Vector2(0f, -25f)
            };
            UIObject title2 = new TextObject("title2", ContentStorer.Fonts["Pixel"], "Made by Shlomito", Vector2.Zero, false)
            {
                Scale = Vector2.One * 1.25f,
                CCPosition = new Vector2(0f, 100f)
            };
            UIObject title3 = new TextObject("title3", ContentStorer.Fonts["Pixel"], "For the Acerola Jam 0", Vector2.Zero, false)
            {
                Scale = Vector2.One,
                CCPosition = new Vector2(0f, 200f)
            };
            UIObject title4 = new TextObject("title4", ContentStorer.Fonts["Pixel"], "All Art by: Shlomito", Vector2.Zero, false)
            {
                Scale = Vector2.One * 0.7f,
                CCPosition = new Vector2(0f, 370f)
            };
            UIObject title5 = new TextObject("title4", ContentStorer.Fonts["Pixel"], "              Music from:\n                Daniel Lucas\n                Sami Hiltunen\n                EminYILDIRIM\n                FoolBoyMedia [playing]\n                Infinita08\n(from freesoud, I couldn't have made them myself)", Vector2.Zero, false)
            {
                Scale = Vector2.One * 0.7f,
                CCPosition = new Vector2(0f, 550f)
            };
            UIObject title6 = new TextObject("title4", ContentStorer.Fonts["Pixel"], "SFX from:\n  Setuniman\n  RevEwwEr\n  EminYILDIRIM\n  axilirate\n  gabisaraceni\n  vanishedillusion\n  HorrorAudio\n  kwahmah_02", Vector2.Zero, false)
            {
                Scale = Vector2.One * 0.7f,
                CCPosition = new Vector2(0f, 880f)
            };
            UIObject title7 = new TextObject("Continue", ContentStorer.Fonts["PixelBig"], "[C] Play Again?", Vector2.Zero, false)
            {
                Scale = Vector2.One * 0.5f,
                CCPosition = new Vector2(0f, 1500f)
            };
            title1.SetParent(_panel);
            title2.SetParent(_panel);
            title3.SetParent(_panel);
            title4.SetParent(_panel);
            title5.SetParent(_panel);
            title6.SetParent(_panel);
            title7.SetParent(_panel);

            _anim = new Animator(new Dictionary<string, Animation>()
            {
                {"start", new Animation(false, new AnimLayer[]
                    {
                        new AnimLayer(new Tuple<int, int>[]
                        {
                            new Tuple<int, int>(0, 500),
                            new Tuple<int, int>(20000, 750),
                            new Tuple<int, int>(100, -1500),
                        }, 0),
                        new AnimLayer(new Tuple<int, bool>[]
                        {
                            new Tuple<int, bool>(20000, false),
                            new Tuple<int, bool>(100, true)
                        }, 0)
                    })
                }
            }, "start", ints: new int[1], bools: new bool[1]);

            AddGameObjects(new GameObject[]
            {
                _panel,
                title1,
                title2,
                title3,
                title4,
                title5,
                title6,
                title7,

                new UIObject("Animator", null, Vector2.Zero, false, new List<GComponent>()
                {
                    _anim
                }),

                new UIObject("Fade", ContentStorer.WhitePixel, Vector2.Zero, false, new List<GComponent>()
                {
                    new Fade()
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
            MediaPlayer.Volume = 0.1f;
        }

        public override void Update()
        {
            /*_title1.Color = new Color(1, 1, 1) * _anim.Ints[0];
            _title2.Color = new Color(1, 1, 1) * _anim.Ints[1];*/
            _panel.Position = new Vector2(0f, _anim.Ints[0]);

            if (_anim.Bools[0] && Input.GetKey(Microsoft.Xna.Framework.Input.Keys.C, KeyGate.Down))
            {
                Scene scene;

                scene = new Title();

                FindWithTag("Fade").GetComponent<Fade>().FadeOut(scene);
            }
#if DEBUG
            if (Input.GetKey(Microsoft.Xna.Framework.Input.Keys.R, KeyGate.Down))
                SceneHandler.ReloadScene();
#endif
        }
    }
}
