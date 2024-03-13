using Microsoft.Xna.Framework;
using RDEngine.Engine;
using System;
using System.Collections.Generic;
using System.Text;
using RDEngine.Engine.Physics;
using RDEngine.Engine.Animation;
using RDEngine.Engine.UI;
using System.Diagnostics;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Graphics;

namespace RDEngine.GameScripts
{
    public class Intro : Scene
    {
        public Intro() : base(new Color(0x10, 0x10, 0x10, 0xff), 16)
        {
            Song = ContentStorer.Songs["Ambient"];
        }

        public override void Initialize()
        {
            base.Initialize();

            PersistentVars.CurrLevel = 0;

            //Adds all the walls and rugs and stuff of the level
            AddGameObject
            (
                new WorldObject("Level", null, Vector2.Zero, new List<GComponent>()
                {
                    new LayoutLoader("IntroLayout")
                })
            );

            AddGameObject
            (
                new WorldObject("Player", ContentStorer.Textures["Player1"], new Vector2(-0.5f, -5f), initialComponents: new List<GComponent>()
                {
                    new Player(125),
                    new RigidBody(new Vector2(12f, 16f), new Vector2(0f, -2f), drag: 5f),
                    new Animator(new Dictionary<string, Animation>()
                    {
                        {"idle", new Animation(true, new AnimLayer[]
                            {
                                new AnimLayer(new Tuple<int, Texture2D>[]
                                {
                                    new Tuple<int, Texture2D>(200, ContentStorer.Textures["Player1"]),
                                    new Tuple<int, Texture2D>(200, ContentStorer.Textures["Player2"]),
                                    new Tuple<int, Texture2D>(200, ContentStorer.Textures["Player3"])
                                }, 0)
                            })
                        },
                        {"walk", new Animation(true, new AnimLayer[]
                            {
                                new AnimLayer(new Tuple<int, Texture2D>[]
                                {
                                    new Tuple<int, Texture2D>(200, ContentStorer.Textures["Player4"]),
                                    new Tuple<int, Texture2D>(200, ContentStorer.Textures["Player5"])
                                }, 0)
                            })
                        }
                    }, "idle", textures: new Texture2D[1])
                })
                {
                    LayerDepth = 0.75f
                }
            );

            AddGameObject
            (

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
            );

            AddGameObject
            (
                new WorldObject("End", ContentStorer.WhiteSquare, new Vector2(-0.5f, -40.5f), new List<GComponent>()
                {
                    new RigidBody(new Vector2(4f, 2f) * UnitSize, Vector2.Zero, true)
                })
                {
                    Scale = new Vector2(4f, 2f),
                    LayerDepth = 0.9f,
                    Color = new Color(0x10, 0x10, 0x10)
                }
            );

            //Dialouge Stuff
            TextObject _dialogue = new TextObject("Dialogue", ContentStorer.Fonts["Pixel"], "You should'nt be here...", Vector2.Zero, false, new List<GComponent>()
            {
                new Animator(new Dictionary<string, Animation>()
                {
                    {
                        "fade", new Animation(false, new AnimLayer[]
                        {
                            new AnimLayer(new Tuple<int, int>[]
                            {
                                new Tuple<int, int>(1000, 0),
                                new Tuple<int, int>(1000, 200),
                                new Tuple<int, int>(1000, 200),
                                new Tuple<int, int>(0, 0)
                            }, 0),
                            new AnimLayer(new Tuple<int, int>[]
                            {
                                new Tuple<int, int>(1000, 250),
                                new Tuple<int, int>(1000, 270),
                                new Tuple<int, int>(1000, 270),
                                new Tuple<int, int>(0, 290)
                            }, 1)
                        })
                    },
                    {
                        "off", new Animation(false, new AnimLayer[]
                        {
                            new AnimLayer(new Tuple<int, int>[]
                            {
                                new Tuple<int, int>(0, 0)
                            }, 0),
                            new AnimLayer(new Tuple<int, int>[]
                            {
                                new Tuple<int, int>(0, 250)
                            }, 1)
                        })
                    }
                }, "off", ints: new int[2]),
                new DialogueBox()
            })
            {
                Scale = Vector2.One * 0.75f,
                CCPosition = new Vector2(0f, 270f)
            };
            AddGameObject(_dialogue);

            Door door1 = new Door(2);
            AddGameObjects(new GameObject[]
            {
                new WorldObject("Door1", ContentStorer.Textures["Door"], new Vector2(-0.5f, -15f), new List<GComponent>()
                {
                    new RigidBody(new Vector2(2f, 1f) * UnitSize, Vector2.Zero, isStatic: true),
                    door1
                }),
            });

            AddGameObjects(new GameObject[]
            {
                new WorldObject("TalkingTable1", ContentStorer.Textures["Table1x1"], new Vector2(-4f, -14f), new List<GComponent>()
                {
                    new TalkingTable(_dialogue, "Something is deeply wrong here...", 25, door1),
                    new RigidBody(Vector2.One * UnitSize, Vector2.Zero, isStatic: true)
                }),
                new WorldObject("TalkingTable2", ContentStorer.Textures["Table1x1"], new Vector2(3f, -14f), new List<GComponent>()
                {
                    new TalkingTable(_dialogue, "Thank you for coming to our help", 25, door1),
                    new RigidBody(Vector2.One * UnitSize, Vector2.Zero, isStatic: true)
                }),

                new WorldObject("TalkingTable3", ContentStorer.Textures["Table1x1"], new Vector2(4f, -19f), new List<GComponent>()
                {
                    new TalkingTable(_dialogue, "There is... something here, something alien", 25),
                    new RigidBody(Vector2.One * UnitSize, Vector2.Zero, isStatic: true)
                }),
                new WorldObject("TalkingTable4", ContentStorer.Textures["Table1x1"], new Vector2(-5f, -19f), new List<GComponent>()
                {
                    new TalkingTable(_dialogue, "It is corrupting us, controlling us", 25),
                    new RigidBody(Vector2.One * UnitSize, Vector2.Zero, isStatic: true)
                }),

                new WorldObject("TalkingTable5", ContentStorer.Textures["Table1x1"], new Vector2(-6f, -26f), new List<GComponent>()
                {
                    new TalkingTable(_dialogue, "It is ripping us from the real world", 25),
                    new RigidBody(Vector2.One * UnitSize, Vector2.Zero, isStatic: true)
                }),
                new WorldObject("TalkingTable6", ContentStorer.Textures["Table1x1"], new Vector2(-1f, -26f), new List<GComponent>()
                {
                    new TalkingTable(_dialogue, "Do not get too close to us...", 25),
                    new RigidBody(Vector2.One * UnitSize, Vector2.Zero, isStatic: true)
                }),
                new WorldObject("TalkingTable7", ContentStorer.Textures["Table1x1"], new Vector2(4f, -26f), new List<GComponent>()
                {
                    new TalkingTable(_dialogue, "... some of us are not fully here", 25),
                    new RigidBody(Vector2.One * UnitSize, Vector2.Zero, isTrigger: true, isStatic: true)
                }),

                new WorldObject("TalkingTable8", ContentStorer.Textures["Table1x1"], new Vector2(7f, -33f), new List<GComponent>()
                {
                    new TalkingTable(_dialogue, "Get too close to out core and...", 25),
                    new RigidBody(Vector2.One * UnitSize, Vector2.Zero, isStatic: true)
                }),
                new WorldObject("TalkingTable9", ContentStorer.Textures["Table1x1"], new Vector2(2f, -33f), new List<GComponent>()
                {
                    new TalkingTable(_dialogue, "... we may corrupt you too.", 25),
                    new RigidBody(Vector2.One * UnitSize, Vector2.Zero, isStatic: true)
                })
            });

            AddGameObject
            (
                new WorldObject("Camera", null, Vector2.Zero, initialComponents: new List<GComponent>()
                {
                    new CameraFollow()
                })
            );

            UIObject pausePanel = new UIObject("PausePanel", ContentStorer.WhitePixel, Vector2.Zero, false)
            {
                Enabled = false,
                Color = new Color(0x20, 0x20, 0x20, 100),
                Scale = RDEGame.UpscaledScrSize,
                LayerDepth = 0.3f
            };
            TextObject hcText = new TextObject("HCText", ContentStorer.Fonts["Pixel"], "[D] Gamemode: Checkpoints", Vector2.Zero, false)
            {
                CCPosition = new Vector2(0f, 0f)
            };
            TextObject musicPauseText = new TextObject("MusicPauseText", ContentStorer.Fonts["Pixel"], "[S] Music: Off", Vector2.Zero, false)
            {
                CCPosition = new Vector2(0f, 60f)
            };
            TextObject fpsOptions = new TextObject("FPSOpText", ContentStorer.Fonts["Pixel"], "[A] FPS Counter: Off", Vector2.Zero, false)
            {
                CCPosition = new Vector2(0f, 120f)
            };
            TextObject pauseTitle = new TextObject("PauseTitle", ContentStorer.Fonts["PixelBig"], "Paused", Vector2.Zero, false)
            {
                Color = new Color(0xf6, 0xcd, 0x26),
                TCPosition = new Vector2(0f, 200f)
            };
            TextObject resumeText = new TextObject("ResumeText", ContentStorer.Fonts["Pixel"], "[C] Resume", Vector2.Zero, false)
            {
                Scale = Vector2.One * 0.75f,
                CCPosition = new Vector2(0f, 230f)
            };
            hcText.SetParent(pausePanel);
            pauseTitle.SetParent(pausePanel);
            resumeText.SetParent(pausePanel);
            fpsOptions.SetParent(pausePanel);
            musicPauseText.SetParent(pausePanel);

            UIObject fpsCounter = new TextObject("FPS", ContentStorer.Fonts["Arial"], "000", Vector2.Zero, false, initialComponents: new List<GComponent>()
            {
                new FPSCounter()
            })
            {
                Color = Color.LightGreen,
                TRPosition = new Vector2(-10f, 10f)
            };

            AddGameObjects(new GameObject[]
            {
                fpsCounter,
#if DEBUG
                new UIObject("CoordGrid", null, Vector2.Zero, true, new List<GComponent>()
                {
                    new GridNums()
                })
#endif
            });

            AddGameObjects(new GameObject[]
            {
                pausePanel, hcText, pauseTitle, fpsOptions, resumeText, musicPauseText
            });

            AddGameObjects(new GameObject[]
            {
                new TextObject("PauseButton", ContentStorer.Fonts["Pixel"], "[C] Options", Vector2.Zero, false)
                {
                    TLPosition = new Vector2(10f, 10f),
                    Scale = Vector2.One * 0.3f
                }
            });

            AddGameObject
            (
                new UIObject("ShortCuts", null, Vector2.Zero, false, new List<GComponent>()
                {
#if DEBUG
                    new ShortCuts(),
#endif
                    new PauseMenu(pausePanel, fpsCounter, hcText, musicPauseText, fpsOptions)
                })
            );
        }

        public override void Start()
        {
            base.Start();

            if (SceneHandler.ActiveSong != Song && PersistentVars.MusicPlaying)
                SceneHandler.PlaySong(Song, true);
            MediaPlayer.Volume = 0.05f;

            FindWithTag("Camera").GetComponent<CameraFollow>().SetTarget(FindWithTag("Player") as WorldObject, true);
        }
    }
}
