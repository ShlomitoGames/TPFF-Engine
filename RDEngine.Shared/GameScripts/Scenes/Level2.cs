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
    public class Level2 : Scene
    {
        public Level2() : base(new Color(0x10, 0x10, 0x10, 0xff), 16) { }

        public override void Initialize()
        {
            base.Initialize();

            //Adds all the walls and rugs and stuff of the level
            AddGameObject
            (
                new WorldObject("Level", null, Vector2.Zero, new List<GComponent>()
                {
                    new LayoutLoader("Level2")
                })
            );

            AddGameObject
            (
                new WorldObject("Player", ContentStorer.Textures["Player1"], new Vector2(-0.5f, -3.5f), initialComponents: new List<GComponent>()
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
                new WorldObject("Door1", ContentStorer.Textures["Door"], new Vector2(-0.5f, -2f), new List<GComponent>()
                {
                    new RigidBody(new Vector2(2f, 1f) * UnitSize, Vector2.Zero, isStatic: true)
                })
                {
                    LayerDepth = 0.9f
                }
            );

            //Room1
            AddGameObjects(new GameObject[]
            {
                new WorldObject("Move1", ContentStorer.WhiteSquare, new Vector2(-6.5f, -6f), new List<GComponent>()
                {
                    new Animator(new Dictionary<string, Animation>()
                    {
                        {"move", new Animation(true, new AnimLayer[]
                            {
                                new AnimLayer(new Tuple<int, float>[]
                                {
                                    new Tuple<int, float>(500, -6.5f),
                                    new Tuple<int, float>(500, -2.5f),
                                    new Tuple<int, float>(1000, -2.5f),
                                    new Tuple<int, float>(800, -6.5f)
                                }, 0)
                            })
                        }
                    }, "move", floats: new float[2]),
                    new RigidBody(new Vector2(4f, 3f) * UnitSize, Vector2.Zero, mass: 10f, isKinematic: true),
                    new MovingFurniture(true, false)
                })
                {
                    Scale = new Vector2(4f, 3f),
                    Color = Color.DarkCyan
                },
                new WorldObject("Move2", ContentStorer.WhiteSquare, new Vector2(5.5f, -6f), new List<GComponent>()
                {
                    new Animator(new Dictionary<string, Animation>()
                    {
                        {"move", new Animation(true, new AnimLayer[]
                            {
                                new AnimLayer(new Tuple<int, float>[]
                                {
                                    new Tuple<int, float>(500, 5.5f),
                                    new Tuple<int, float>(500, 1.5f),
                                    new Tuple<int, float>(1000, 1.5f),
                                    new Tuple<int, float>(800, 5.5f)
                                }, 0)
                            })
                        }
                    }, "move", floats: new float[2]),
                    new RigidBody(new Vector2(4f, 3f) * UnitSize, Vector2.Zero, mass: 10f, isKinematic: true),
                    new MovingFurniture(true, false)
                })
                {
                    Scale = new Vector2(4f, 3f),
                    Color = Color.DarkCyan
                },

                new WorldObject("Move3", ContentStorer.WhiteSquare, new Vector2(-6.5f, -9f), new List<GComponent>()
                {
                    new Animator(new Dictionary<string, Animation>()
                    {
                        {"move", new Animation(true, new AnimLayer[]
                            {
                                new AnimLayer(new Tuple<int, float>[]
                                {
                                    new Tuple<int, float>(200, -6.5f),
                                    new Tuple<int, float>(500, -6.5f),
                                    new Tuple<int, float>(500, -2.5f),
                                    new Tuple<int, float>(1000, -2.5f),
                                    new Tuple<int, float>(600, -6.5f)
                                }, 0)
                            })
                        }
                    }, "move", floats: new float[2]),
                    new RigidBody(new Vector2(4f, 3f) * UnitSize, Vector2.Zero, mass: 10f, isKinematic: true),
                    new MovingFurniture(true, false)
                })
                {
                    Scale = new Vector2(4f, 3f),
                    Color = Color.DarkCyan
                },
                new WorldObject("Move4", ContentStorer.WhiteSquare, new Vector2(5.5f, -9f), new List<GComponent>()
                {
                    new Animator(new Dictionary<string, Animation>()
                    {
                        {"move", new Animation(true, new AnimLayer[]
                            {
                                new AnimLayer(new Tuple<int, float>[]
                                {
                                    new Tuple<int, float>(200, 5.5f),
                                    new Tuple<int, float>(500, 5.5f),
                                    new Tuple<int, float>(500, 1.5f),
                                    new Tuple<int, float>(1000, 1.5f),
                                    new Tuple<int, float>(600, 5.5f)
                                }, 0)
                            })
                        }
                    }, "move", floats: new float[2]),
                    new RigidBody(new Vector2(4f, 3f) * UnitSize, Vector2.Zero, mass: 10f, isKinematic: true),
                    new MovingFurniture(true, false)
                })
                {
                    Scale = new Vector2(4f, 3f),
                    Color = Color.DarkCyan
                },

                new WorldObject("Move5", ContentStorer.WhiteSquare, new Vector2(-6.5f, -12f), new List<GComponent>()
                {
                    new Animator(new Dictionary<string, Animation>()
                    {
                        {"move", new Animation(true, new AnimLayer[]
                            {
                                new AnimLayer(new Tuple<int, float>[]
                                {
                                    new Tuple<int, float>(400, -6.5f),
                                    new Tuple<int, float>(500, -6.5f),
                                    new Tuple<int, float>(500, -2.5f),
                                    new Tuple<int, float>(1000, -2.5f),
                                    new Tuple<int, float>(400, -6.5f)
                                }, 0)
                            })
                        }
                    }, "move", floats: new float[2]),
                    new RigidBody(new Vector2(4f, 3f) * UnitSize, Vector2.Zero, mass: 10f, isKinematic: true),
                    new MovingFurniture(true, false)
                })
                {
                    Scale = new Vector2(4f, 3f),
                    Color = Color.DarkCyan
                },
                new WorldObject("Move6", ContentStorer.WhiteSquare, new Vector2(5.5f, -12f), new List<GComponent>()
                {
                    new Animator(new Dictionary<string, Animation>()
                    {
                        {"move", new Animation(true, new AnimLayer[]
                            {
                                new AnimLayer(new Tuple<int, float>[]
                                {
                                    new Tuple<int, float>(400, 5.5f),
                                    new Tuple<int, float>(500, 5.5f),
                                    new Tuple<int, float>(500, 1.5f),
                                    new Tuple<int, float>(1000, 1.5f),
                                    new Tuple<int, float>(400, 5.5f)
                                }, 0)
                            })
                        }
                    }, "move", floats: new float[2]),
                    new RigidBody(new Vector2(4f, 3f) * UnitSize, Vector2.Zero, mass: 10f, isKinematic: true),
                    new MovingFurniture(true, false)
                })
                {
                    Scale = new Vector2(4f, 3f),
                    Color = Color.DarkCyan
                },

                new WorldObject("Move7", ContentStorer.WhiteSquare, new Vector2(-6.5f, -15f), new List<GComponent>()
                {
                    new Animator(new Dictionary<string, Animation>()
                    {
                        {"move", new Animation(true, new AnimLayer[]
                            {
                                new AnimLayer(new Tuple<int, float>[]
                                {
                                    new Tuple<int, float>(600, -6.5f),
                                    new Tuple<int, float>(500, -6.5f),
                                    new Tuple<int, float>(500, -2.5f),
                                    new Tuple<int, float>(1000, -2.5f),
                                    new Tuple<int, float>(200, -6.5f)
                                }, 0)
                            })
                        }
                    }, "move", floats: new float[2]),
                    new RigidBody(new Vector2(4f, 3f) * UnitSize, Vector2.Zero, mass: 10f, isKinematic: true),
                    new MovingFurniture(true, false)
                })
                {
                    Scale = new Vector2(4f, 3f),
                    Color = Color.DarkCyan
                },
                new WorldObject("Move8", ContentStorer.WhiteSquare, new Vector2(5.5f, -15f), new List<GComponent>()
                {
                    new Animator(new Dictionary<string, Animation>()
                    {
                        {"move", new Animation(true, new AnimLayer[]
                            {
                                new AnimLayer(new Tuple<int, float>[]
                                {
                                    new Tuple<int, float>(600, 5.5f),
                                    new Tuple<int, float>(500, 5.5f),
                                    new Tuple<int, float>(500, 1.5f),
                                    new Tuple<int, float>(1000, 1.5f),
                                    new Tuple<int, float>(200, 5.5f)
                                }, 0)
                            })
                        }
                    }, "move", floats: new float[2]),
                    new RigidBody(new Vector2(4f, 3f) * UnitSize, Vector2.Zero, mass: 10f, isKinematic: true),
                    new MovingFurniture(true, false)
                })
                {
                    Scale = new Vector2(4f, 3f),
                    Color = Color.DarkCyan
                },

                new WorldObject("Move9", ContentStorer.WhiteSquare, new Vector2(-6.5f, -18f), new List<GComponent>()
                {
                    new Animator(new Dictionary<string, Animation>()
                    {
                        {"move", new Animation(true, new AnimLayer[]
                            {
                                new AnimLayer(new Tuple<int, float>[]
                                {
                                    new Tuple<int, float>(800, -6.5f),
                                    new Tuple<int, float>(500, -6.5f),
                                    new Tuple<int, float>(500, -2.5f),
                                    new Tuple<int, float>(1000, -2.5f),
                                    new Tuple<int, float>(00, -6.5f)
                                }, 0)
                            })
                        }
                    }, "move", floats: new float[2]),
                    new RigidBody(new Vector2(4f, 3f) * UnitSize, Vector2.Zero, mass: 10f, isKinematic: true),
                    new MovingFurniture(true, false)
                })
                {
                    Scale = new Vector2(4f, 3f),
                    Color = Color.DarkCyan
                },
                new WorldObject("Move10", ContentStorer.WhiteSquare, new Vector2(5.5f, -18f), new List<GComponent>()
                {
                    new Animator(new Dictionary<string, Animation>()
                    {
                        {"move", new Animation(true, new AnimLayer[]
                            {
                                new AnimLayer(new Tuple<int, float>[]
                                {
                                    new Tuple<int, float>(800, 5.5f),
                                    new Tuple<int, float>(500, 5.5f),
                                    new Tuple<int, float>(500, 1.5f),
                                    new Tuple<int, float>(1000, 1.5f),
                                    new Tuple<int, float>(00, 5.5f)
                                }, 0)
                            })
                        }
                    }, "move", floats: new float[2]),
                    new RigidBody(new Vector2(4f, 3f) * UnitSize, Vector2.Zero, mass: 10f, isKinematic: true),
                    new MovingFurniture(true, false)
                })
                {
                    Scale = new Vector2(4f, 3f),
                    Color = Color.DarkCyan
                },

                new WorldObject("Static1", ContentStorer.Textures["Table1x3"], new Vector2(-9f, -6f), new List<GComponent>()
                {
                    new RigidBody(new Vector2(1f * UnitSize, 3f * UnitSize), Vector2.Zero, isStatic : true),
                    new Furniture()
                }),
                new WorldObject("Static2", ContentStorer.Textures["Table1x3"], new Vector2(-9f, -9f), new List<GComponent>()
                {
                    new RigidBody(new Vector2(1f * UnitSize, 3f * UnitSize), Vector2.Zero, isStatic : true),
                    new Furniture()
                }),
                new WorldObject("Static3", ContentStorer.Textures["Table1x3"], new Vector2(-9f, -12f), new List<GComponent>()
                {
                    new RigidBody(new Vector2(1f * UnitSize, 3f * UnitSize), Vector2.Zero, isStatic : true),
                    new Furniture()
                }),
                new WorldObject("Static4", ContentStorer.Textures["Table1x3"], new Vector2(-9f, -15f), new List<GComponent>()
                {
                    new RigidBody(new Vector2(1f * UnitSize, 3f * UnitSize), Vector2.Zero, isStatic : true),
                    new Furniture()
                }),
                new WorldObject("Static5", ContentStorer.Textures["Table1x3"], new Vector2(-9f, -18f), new List<GComponent>()
                {
                    new RigidBody(new Vector2(1f * UnitSize, 3f * UnitSize), Vector2.Zero, isStatic : true),
                    new Furniture()
                }),

                new WorldObject("Static6", ContentStorer.Textures["Table1x3"], new Vector2(8f, -6f), new List<GComponent>()
                {
                    new RigidBody(new Vector2(1f * UnitSize, 3f * UnitSize), Vector2.Zero, isStatic : true),
                    new Furniture()
                }),
                new WorldObject("Static7", ContentStorer.Textures["Table1x3"], new Vector2(8f, -9f), new List<GComponent>()
                {
                    new RigidBody(new Vector2(1f * UnitSize, 3f * UnitSize), Vector2.Zero, isStatic : true),
                    new Furniture()
                }),
                new WorldObject("Static8", ContentStorer.Textures["Table1x3"], new Vector2(8f, -12f), new List<GComponent>()
                {
                    new RigidBody(new Vector2(1f * UnitSize, 3f * UnitSize), Vector2.Zero, isStatic : true),
                    new Furniture()
                }),
                new WorldObject("Static9", ContentStorer.Textures["Table1x3"], new Vector2(8f, -15f), new List<GComponent>()
                {
                    new RigidBody(new Vector2(1f * UnitSize, 3f * UnitSize), Vector2.Zero, isStatic : true),
                    new Furniture()
                }),
                new WorldObject("Static10", ContentStorer.Textures["Table1x3"], new Vector2(8f, -18f), new List<GComponent>()
                {
                    new RigidBody(new Vector2(1f * UnitSize, 3f * UnitSize), Vector2.Zero, isStatic : true),
                    new Furniture()
                }),
            });

            Door door1 = new Door();
            Door door2 = new Door();

            AddGameObjects(new GameObject[]
            {
                new WorldObject("Door1", ContentStorer.Textures["Door"], new Vector2(-0.5f, -21f), new List<GComponent>()
                {
                    new RigidBody(new Vector2(2f, 1f) * UnitSize, Vector2.Zero, isStatic: true),
                    door1
                }),
                new WorldObject("Key1", ContentStorer.Textures["Key"], new Vector2(-7f, -16f), new List<GComponent>()
                {
                    new RigidBody(new Vector2(16f, 8f), Vector2.Zero, true),
                    new Key(door1),
                })
                {
                    LayerDepth = 0.4f
                },
                new WorldObject("Door2", ContentStorer.Textures["Door"], new Vector2(-0.5f, -23f), new List<GComponent>()
                {
                    new RigidBody(new Vector2(2f, 1f) * UnitSize, Vector2.Zero, isStatic: true),
                    door2
                }),
                new WorldObject("Key2", ContentStorer.Textures["Key"], new Vector2(6f, -16f), new List<GComponent>()
                {
                    new RigidBody(new Vector2(16f, 8f), Vector2.Zero, true),
                    new Key(door2),
                })
                {
                    LayerDepth = 0.4f
                },
            });

            //Room 2
            AddGameObjects(new GameObject[]
            {
                new WorldObject("Move11", ContentStorer.Textures["ATable1x1"], new Vector2(-3f, -24f), new List<GComponent>()
                {
                    new Animator(new Dictionary<string, Animation>()
                    {
                        {"move", new Animation(true, new AnimLayer[]
                            {
                                new AnimLayer(new Tuple<int, float>[]
                                {
                                    new Tuple<int, float>(500, -24f),
                                    new Tuple<int, float>(300, -29f),
                                    new Tuple<int, float>(500, -29f),
                                    new Tuple<int, float>(300, -24f)
                                }, 1)
                            })
                        }
                    }, "move", floats: new float[2]),
                    new RigidBody(Vector2.One * UnitSize, Vector2.Zero, mass: 10f, isKinematic: true),
                    new MovingFurniture(false, true)
                }),
                new WorldObject("Move12", ContentStorer.Textures["ATable1x1"], new Vector2(-3f, -29f), new List<GComponent>()
                {
                    new Animator(new Dictionary<string, Animation>()
                    {
                        {"move", new Animation(true, new AnimLayer[]
                            {
                                new AnimLayer(new Tuple<int, float>[]
                                {
                                    new Tuple<int, float>(500, -3f),
                                    new Tuple<int, float>(300, 2f),
                                    new Tuple<int, float>(500, 2f),
                                    new Tuple<int, float>(300, -3f)
                                }, 0)
                            })
                        }
                    }, "move", floats: new float[2]),
                    new RigidBody(Vector2.One * UnitSize, Vector2.Zero, mass: 10f, isKinematic: true),
                    new MovingFurniture(true, false)
                }),
                new WorldObject("Move13", ContentStorer.Textures["ATable1x1"], new Vector2(2f, -29f), new List<GComponent>()
                {
                    new Animator(new Dictionary<string, Animation>()
                    {
                        {"move", new Animation(true, new AnimLayer[]
                            {
                                new AnimLayer(new Tuple<int, float>[]
                                {
                                    new Tuple<int, float>(500, -29f),
                                    new Tuple<int, float>(300, -24f),
                                    new Tuple<int, float>(500, -24f),
                                    new Tuple<int, float>(300, -29f)
                                }, 1)
                            })
                        }
                    }, "move", floats: new float[2]),
                    new RigidBody(Vector2.One * UnitSize, Vector2.Zero, mass: 10f, isKinematic: true),
                    new MovingFurniture(false, true)
                }),
                new WorldObject("Move14", ContentStorer.Textures["ATable1x1"], new Vector2(2f, -24f), new List<GComponent>()
                {
                    new Animator(new Dictionary<string, Animation>()
                    {
                        {"move", new Animation(true, new AnimLayer[]
                            {
                                new AnimLayer(new Tuple<int, float>[]
                                {
                                    new Tuple<int, float>(500, 2f),
                                    new Tuple<int, float>(300, -3f),
                                    new Tuple<int, float>(500, -3f),
                                    new Tuple<int, float>(300, 2f)
                                }, 0)
                            })
                        }
                    }, "move", floats: new float[2]),
                    new RigidBody(Vector2.One * UnitSize, Vector2.Zero, mass: 10f, isKinematic: true),
                    new MovingFurniture(true, false)
                }),
            });

            AddGameObject
            (
                new WorldObject("Camera", null, Vector2.Zero, initialComponents: new List<GComponent>()
                {
                    new CameraFollow(),
                    new ShortCuts()
                })
            );

#if DEBUG
            AddGameObjects(new GameObject[]
            {
                new TextObject("FPS", ContentStorer.Fonts["Arial"], "000", Vector2.Zero, false, initialComponents: new List<GComponent>()
                {
                    new FPSCounter()
                })
                {
                    Color = Color.LightGreen,
                    TRPosition = new Vector2(-10f, 10f)
                },
                new UIObject("CoordGrid", null, Vector2.Zero, true, new List<GComponent>()
                {
                    new GridNums()
                })
            });
#endif
        }

        public override void Start()
        {
            base.Start();

            if (SceneHandler.ActiveSong != ContentStorer.Songs["MysteryLoop"])
                SceneHandler.PlaySong(ContentStorer.Songs["MysteryLoop"], true);
            MediaPlayer.Volume = 0.1f;

            FindWithTag("Camera").GetComponent<CameraFollow>().SetTarget(FindWithTag("Player") as WorldObject, true);
        }
    }
}
