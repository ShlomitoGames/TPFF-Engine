﻿using Microsoft.Xna.Framework;
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
    public class Level1End : Scene
    {
        public Level1End() : base(new Color(0x10, 0x10, 0x10, 0xff), 16)
        {
            Song = ContentStorer.Songs["Ambient"];
        }

        public override void Initialize()
        {
            base.Initialize();

            PersistentVars.CurrLevel = 1;
            PersistentVars.OnOutro = true;

            //Adds all the walls and rugs and stuff of the level
            AddGameObject
            (
                new WorldObject("Level", null, Vector2.Zero, new List<GComponent>()
                {
                    new LayoutLoader("Level1")
                })
            );

            AddGameObject
            (
                new WorldObject("Player", ContentStorer.Textures["Player1"], new Vector2(-0.5f, -20.5f), initialComponents: new List<GComponent>()
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
                new WorldObject("End", ContentStorer.WhiteSquare, new Vector2(-0.5f, -22.5f))
                {
                    Scale = new Vector2(4f, 2f),
                    LayerDepth = 0.9f,
                    Color = new Color(0x10, 0x10, 0x10)
                }
            );

            AddGameObject
            (
                new WorldObject("Door1", ContentStorer.Textures["DoorOpen"], new Vector2(-0.5f, -2f))
            );

            AddGameObject
            (
                new WorldObject("Black", ContentStorer.WhiteSquare, new Vector2(-0.5f, -0.5f))
                {
                    LayerDepth = 0.9f,
                    Scale = new Vector2(2f, 2f),
                    Color = new Color(0x10,0x10,0x10)
                }
            );

            //Room 1
            AddGameObjects(new WorldObject[]
            {
                new WorldObject("Move1", ContentStorer.Textures["ATable3x2D"], new Vector2(-12f, -7.5f), new List<GComponent>()
                {
                    new RigidBody(new Vector2(3f * UnitSize, 2f * UnitSize), Vector2.Zero, mass: 10f, isKinematic: true)
                }),
                new WorldObject("Static1", ContentStorer.Textures["Table1x2D"], new Vector2(8f, -7.5f), new List<GComponent>()
                {
                    new RigidBody(new Vector2(1f * UnitSize, 2f * UnitSize), Vector2.Zero, isStatic : true)
                }),
                new WorldObject("Move2", ContentStorer.Textures["ATable3x2D"], new Vector2(-12f, -11.5f), new List<GComponent>()
                {
                    new RigidBody(new Vector2(3f * UnitSize, 2f * UnitSize), Vector2.Zero, mass: 10f, isKinematic: true)
                }),
                new WorldObject("Static2", ContentStorer.Textures["Table1x2D"], new Vector2(8f, -11.5f), new List<GComponent>()
                {
                    new RigidBody(new Vector2(1f * UnitSize, 2f * UnitSize), Vector2.Zero, isStatic: true)
                }),
                new WorldObject("Follow1", ContentStorer.Textures["ATable1x1D"], new Vector2(-17f, -11f), new List<GComponent>()
                {
                    new RigidBody(Vector2.One * UnitSize, Vector2.Zero, isStatic: true)
                })
            });

            AddGameObjects(new GameObject[]
            {
                new WorldObject("Door1", ContentStorer.Textures["DoorOpen"], new Vector2(25.5f, -9f), new List<GComponent>()),

                new WorldObject("Door2", ContentStorer.Textures["DoorOpen"], new Vector2(-0.5f, -17f), new List<GComponent>()),

                new WorldObject("Door3", ContentStorer.Textures["DoorOpen"], new Vector2(31.5f, -5f), new List<GComponent>())
            });

            //Room 2
            AddGameObjects(new GameObject[]
            {
                new WorldObject("Move3", ContentStorer.Textures["ATable3x2D"], new Vector2(18f, -3.5f), new List<GComponent>()
                {
                    new RigidBody(new Vector2(3f * UnitSize, 2f * UnitSize), Vector2.Zero, mass: 10f, isKinematic: true)
                }),
                new WorldObject("Static3", ContentStorer.Textures["Table3x1D"], new Vector2(18f, -16f), new List<GComponent>()
                {
                    new RigidBody(new Vector2(3f * UnitSize, 1f * UnitSize), Vector2.Zero, isStatic: true)
                }),
                new WorldObject("Move4", ContentStorer.Textures["ATable3x2D"], new Vector2(21f, -3.5f), new List<GComponent>()
                {
                    new RigidBody(new Vector2(3f * UnitSize, 2f * UnitSize), Vector2.Zero, mass: 10f, isKinematic: true)
                }),
                new WorldObject("Static4", ContentStorer.Textures["Table3x1D"], new Vector2(21f, -17f), new List<GComponent>()
                {
                    new RigidBody(new Vector2(3f * UnitSize, 1f * UnitSize), Vector2.Zero, isStatic: true)
                }),
                new WorldObject("Follow2", ContentStorer.Textures["ATable2x2D"], new Vector2(10.5f, -3.5f), new List<GComponent>()
                {
                    new RigidBody(Vector2.One * UnitSize * 2, Vector2.Zero, isStatic: true)
                }),
                new WorldObject("Move5", ContentStorer.Textures["ATable2x2D"], new Vector2(28.5f, -15.5f), new List<GComponent>()
                {
                    new RigidBody(new Vector2(2f * UnitSize, 2f * UnitSize), Vector2.Zero, mass: 10f, isKinematic: true)
                }),
                new WorldObject("Move6", ContentStorer.Textures["ATable2x2D"], new Vector2(37.5f, -15.5f), new List<GComponent>()
                {
                    new RigidBody(new Vector2(2f * UnitSize, 2f * UnitSize), Vector2.Zero, mass: 10f, isKinematic: true)
                })
                {
                    Effects = SpriteEffects.FlipHorizontally
                },
                new WorldObject("Move7", ContentStorer.Textures["ATable1x2D"], new Vector2(35f, -13.5f), new List<GComponent>()
                {
                    new RigidBody(new Vector2(1f * UnitSize, 2f * UnitSize), Vector2.Zero, mass: 10f, isKinematic: true)
                }),
                new WorldObject("Move8", ContentStorer.Textures["ATable1x2D"], new Vector2(38f, -11.5f), new List<GComponent>()
                {
                    new RigidBody(new Vector2(1f * UnitSize, 2f * UnitSize), Vector2.Zero, mass: 10f, isKinematic: true)
                })
                {
                    Effects = SpriteEffects.FlipHorizontally
                },
                new WorldObject("Move9", ContentStorer.Textures["ATable1x2D"], new Vector2(35f, -9.5f), new List<GComponent>()
                {
                    new RigidBody(new Vector2(1f * UnitSize, 2f * UnitSize), Vector2.Zero, mass: 10f, isKinematic: true)
                }),
                new WorldObject("Static5", ContentStorer.Textures["Table3x1D"], new Vector2(29f, -17f), new List<GComponent>()
                {
                    new RigidBody(new Vector2(3f * UnitSize, 1f * UnitSize), Vector2.Zero, isStatic: true)
                }),
                new WorldObject("Static6", ContentStorer.Textures["Table1x3D"], new Vector2(34f, -13f), new List<GComponent>()
                {
                    new RigidBody(new Vector2(1f * UnitSize, 3f * UnitSize), Vector2.Zero, isStatic: true)
                }),
                new WorldObject("Static7", ContentStorer.Textures["Table1x3D"], new Vector2(34f, -10f), new List<GComponent>()
                {
                    new RigidBody(new Vector2(1f * UnitSize, 3f * UnitSize), Vector2.Zero, isStatic: true)
                }),
            });

            AddGameObjects(new GameObject[]
            {
                new WorldObject("TalkingTable1", ContentStorer.Textures["Table1x1D"], new Vector2(-8f, -4f), new List<GComponent>()
                {
                    new RigidBody(Vector2.One * UnitSize, Vector2.Zero, isStatic: true)
                }),
                new WorldObject("TalkingTable2", ContentStorer.Textures["Table1x1D"], new Vector2(7f, -4f), new List<GComponent>()
                {
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
                new TextObject("PauseButton", ContentStorer.Fonts["Pixel"], "[C] Pause", Vector2.Zero, false)
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
            MediaPlayer.Volume = 0.1f;

            FindWithTag("Camera").GetComponent<CameraFollow>().SetTarget(FindWithTag("Player") as WorldObject, true);
        }
    }
}
