using Microsoft.Xna.Framework;
using RDEngine.Engine;
using System;
using System.Collections.Generic;
using System.Text;
using RDEngine.Engine.Physics;
using RDEngine.Engine.Animation;
using RDEngine.Engine.UI;
using System.Diagnostics;

namespace RDEngine.GameScripts
{
    public class Scene1 : Scene
    {
        public Scene1() : base(new Color(0x10,0x10,0x10,0x255), 16) { }

        public override void Initialize()
        {
            base.Initialize();

            //Adds all the walls and rugs and stuff of the level
            AddGameObject
            (
                new WorldObject("Level", null, Vector2.Zero, new List<GComponent>()
                {
                    new LayoutLoader("Layout")
                })
            );

            AddGameObject
            (
                new WorldObject("Player", ContentStorer.WhiteSquare, new Vector2(-0.5f, -5f), initialComponents: new List<GComponent>()
                {
                    new Player(750),
                    new RigidBody(Vector2.One * UnitSize, Vector2.Zero, drag: 5f)
                })
                {
                    LayerDepth = 0.75f
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

            //Room 1
            AddGameObjects(new WorldObject[]
            {
                new WorldObject("Move1", ContentStorer.WhiteSquare, new Vector2(6f, -7.5f), new List<GComponent>()
                {
                    new Animator(new Dictionary<string, Animation>()
                    {
                        {"move", new Animation(true, new AnimLayer[]
                            {
                                new AnimLayer(new Tuple<int, float>[]
                                {
                                    new Tuple<int, float>(500, 6f),
                                    new Tuple<int, float>(300, -12.5f),
                                    new Tuple<int, float>(500, -12.5f),
                                    new Tuple<int, float>(300, 6f)
                                }, 0)
                            })
                        }
                    }, "move", floats: new float[2]),
                    new RigidBody(new Vector2(3f * UnitSize, 2f * UnitSize), Vector2.Zero, mass: 10f, isKinematic: true),
                    new MovingFurniture(true, false)
                })
                {
                    Scale = new Vector2(3f, 2f),
                    Color = Color.DarkMagenta
                },
                new WorldObject("Static1", ContentStorer.Textures["Table1x2"], new Vector2(8f, -7.5f), new List<GComponent>()
                {
                    new RigidBody(new Vector2(1f * UnitSize, 2f * UnitSize), Vector2.Zero, isStatic : true),
                    new Furniture()
                }),
                new WorldObject("Move2", ContentStorer.WhiteSquare, new Vector2(-12f, -11.5f), new List<GComponent>()
                {
                    new Animator(new Dictionary<string, Animation>()
                    {
                        {"move", new Animation(true, new AnimLayer[]
                            {
                                new AnimLayer(new Tuple<int, float>[]
                                {
                                    new Tuple<int, float>(500, -12f),
                                    new Tuple<int, float>(300, 6f),
                                    new Tuple<int, float>(500, 6f),
                                    new Tuple<int, float>(300, -12f)
                                }, 0)
                            })
                        }
                    }, "move", floats: new float[2]),
                    new RigidBody(new Vector2(3f * UnitSize, 2f * UnitSize), Vector2.Zero, mass: 10f, isKinematic: true),
                    new MovingFurniture(true, false)
                })
                {
                    Scale = new Vector2(3f, 2f),
                    Color = Color.DarkMagenta
                },
                new WorldObject("Static2", ContentStorer.Textures["Table1x2"], new Vector2(8f, -11.5f), new List<GComponent>()
                {
                    new RigidBody(new Vector2(1f * UnitSize, 2f * UnitSize), Vector2.Zero, isStatic: true),
                    new Furniture()
                }),
                new WorldObject("Follow1", ContentStorer.WhiteSquare, new Vector2(-17f, -11f), new List<GComponent>()
                {
                    new RigidBody(Vector2.One * UnitSize, Vector2.Zero, isTrigger: true),
                    new FollowingFurniture(10f, 75f)
                })
            });

            //Room 2
            AddGameObjects(new GameObject[]
            {
                new WorldObject("Move3", ContentStorer.WhiteSquare, new Vector2(18f, -14.5f), new List<GComponent>()
                {
                    new Animator(new Dictionary<string, Animation>()
                    {
                        {"move", new Animation(true, new AnimLayer[]
                            {
                                new AnimLayer(new Tuple<int, float>[]
                                {
                                    new Tuple<int, float>(500, -14.5f),
                                    new Tuple<int, float>(300, -3.5f),
                                    new Tuple<int, float>(500, -3.5f),
                                    new Tuple<int, float>(300, -14.5f)
                                }, 1)
                            })
                        }
                    }, "move", floats: new float[2]),
                    new RigidBody(new Vector2(3f * UnitSize, 2f * UnitSize), Vector2.Zero, mass: 10f, isKinematic: true),
                    new MovingFurniture(false, true)
                })
                {
                    Scale = new Vector2(3f, 2f),
                    Color = Color.DarkMagenta
                },
                new WorldObject("Static3", ContentStorer.Textures["Table3x1"], new Vector2(18f, -16f), new List<GComponent>()
                {
                    new RigidBody(new Vector2(3f * UnitSize, 1f * UnitSize), Vector2.Zero, isStatic: true),
                    new Furniture()
                }),
                new WorldObject("Move4", ContentStorer.WhiteSquare, new Vector2(21f, -3.5f), new List<GComponent>()
                {
                    new Animator(new Dictionary<string, Animation>()
                    {
                        {"move", new Animation(true, new AnimLayer[]
                            {
                                new AnimLayer(new Tuple<int, float>[]
                                {
                                    new Tuple<int, float>(500, -3.5f),
                                    new Tuple<int, float>(300, -15.5f),
                                    new Tuple<int, float>(500, -15.5f),
                                    new Tuple<int, float>(300, -3.5f)
                                }, 1)
                            })
                        }
                    }, "move", floats: new float[2]),
                    new RigidBody(new Vector2(3f * UnitSize, 2f * UnitSize), Vector2.Zero, mass: 10f, isKinematic: true),
                    new MovingFurniture(false, true)
                })
                {
                    Scale = new Vector2(3f, 2f),
                    Color = Color.DarkMagenta
                },
                new WorldObject("Static4", ContentStorer.Textures["Table3x1"], new Vector2(21f, -17f), new List<GComponent>()
                {
                    new RigidBody(new Vector2(3f * UnitSize, 1f * UnitSize), Vector2.Zero, isStatic: true),
                    new Furniture()
                }),
                new WorldObject("Follow2", ContentStorer.WhiteSquare, new Vector2(10.5f, -3.5f), new List<GComponent>()
                {
                    new RigidBody(Vector2.One * UnitSize * 2, Vector2.Zero),
                    new FollowingFurniture(10f, 50f)
                })
                {
                    Scale = Vector2.One * 2
                },
                new WorldObject("Move5", ContentStorer.WhiteSquare, new Vector2(28.5f, -15.5f), new List<GComponent>()
                {
                    new Animator(new Dictionary<string, Animation>()
                    {
                        {"move", new Animation(true, new AnimLayer[]
                            {
                                new AnimLayer(new Tuple<int, float>[]
                                {
                                    new Tuple<int, float>(1000, -15.5f),
                                    new Tuple<int, float>(600, -3.5f),
                                    new Tuple<int, float>(1000, -3.5f),
                                    new Tuple<int, float>(600, -15.5f)
                                }, 1)
                            })
                        }
                    }, "move", floats: new float[2]),
                    new RigidBody(new Vector2(2f * UnitSize, 2f * UnitSize), Vector2.Zero, mass: 10f, isKinematic: true),
                    new MovingFurniture(false, true)
                })
                {
                    Scale = new Vector2(2f, 2f),
                    Color = Color.DarkMagenta
                },
                new WorldObject("Move6", ContentStorer.WhiteSquare, new Vector2(37.5f, -15.5f), new List<GComponent>()
                {
                    new Animator(new Dictionary<string, Animation>()
                    {
                        {"move", new Animation(true, new AnimLayer[]
                            {
                                new AnimLayer(new Tuple<int, float>[]
                                {
                                    new Tuple<int, float>(1000, 37.5f),
                                    new Tuple<int, float>(600, 28.5f),
                                    new Tuple<int, float>(1000, 28.5f),
                                    new Tuple<int, float>(600, 37.5f)
                                }, 0)
                            })
                        }
                    }, "move", floats: new float[2]),
                    new RigidBody(new Vector2(2f * UnitSize, 2f * UnitSize), Vector2.Zero, mass: 10f, isKinematic: true),
                    new MovingFurniture(true, false)
                })
                {
                    Scale = new Vector2(2f, 2f),
                    Color = Color.DarkMagenta
                },
                new WorldObject("Move7", ContentStorer.WhiteSquare, new Vector2(35f, -13.5f), new List<GComponent>()
                {
                    new Animator(new Dictionary<string, Animation>()
                    {
                        {"move", new Animation(true, new AnimLayer[]
                            {
                                new AnimLayer(new Tuple<int, float>[]
                                {
                                    new Tuple<int, float>(1000, 35f),
                                    new Tuple<int, float>(600, 38f),
                                    new Tuple<int, float>(1000, 38f),
                                    new Tuple<int, float>(600, 35f)
                                }, 0)
                            })
                        }
                    }, "move", floats: new float[2]),
                    new RigidBody(new Vector2(1f * UnitSize, 2f * UnitSize), Vector2.Zero, mass: 10f, isKinematic: true),
                    new MovingFurniture(true, false)
                })
                {
                    Scale = new Vector2(1f, 2f),
                    Color = Color.DarkMagenta
                },
                new WorldObject("Move8", ContentStorer.WhiteSquare, new Vector2(38f, -11.5f), new List<GComponent>()
                {
                    new Animator(new Dictionary<string, Animation>()
                    {
                        {"move", new Animation(true, new AnimLayer[]
                            {
                                new AnimLayer(new Tuple<int, float>[]
                                {
                                    new Tuple<int, float>(1000, 38f),
                                    new Tuple<int, float>(600, 35f),
                                    new Tuple<int, float>(1000, 35f),
                                    new Tuple<int, float>(600, 38f)
                                }, 0)
                            })
                        }
                    }, "move", floats: new float[2]),
                    new RigidBody(new Vector2(1f * UnitSize, 2f * UnitSize), Vector2.Zero, mass: 10f, isKinematic: true),
                    new MovingFurniture(true, false)
                })
                {
                    Scale = new Vector2(1f, 2f),
                    Color = Color.DarkMagenta
                },
                new WorldObject("Move9", ContentStorer.WhiteSquare, new Vector2(35f, -9.5f), new List<GComponent>()
                {
                    new Animator(new Dictionary<string, Animation>()
                    {
                        {"move", new Animation(true, new AnimLayer[]
                            {
                                new AnimLayer(new Tuple<int, float>[]
                                {
                                    new Tuple<int, float>(1000, 35f),
                                    new Tuple<int, float>(600, 38f),
                                    new Tuple<int, float>(1000, 38f),
                                    new Tuple<int, float>(600, 35f)
                                }, 0)
                            })
                        }
                    }, "move", floats: new float[2]),
                    new RigidBody(new Vector2(1f * UnitSize, 2f * UnitSize), Vector2.Zero, mass: 10f, isKinematic: true),
                    new MovingFurniture(true, false)
                })
                {
                    Scale = new Vector2(1f, 2f),
                    Color = Color.DarkMagenta
                },
                new WorldObject("Static5", ContentStorer.Textures["Table3x1"], new Vector2(29f, -17f), new List<GComponent>()
                {
                    new RigidBody(new Vector2(3f * UnitSize, 1f * UnitSize), Vector2.Zero, isStatic: true),
                    new Furniture()
                }),
                new WorldObject("Static6", ContentStorer.Textures["Table1x3"], new Vector2(34f, -13f), new List<GComponent>()
                {
                    new RigidBody(new Vector2(1f * UnitSize, 3f * UnitSize), Vector2.Zero, isStatic: true),
                    new Furniture()
                }),
                new WorldObject("Static7", ContentStorer.Textures["Table1x3"], new Vector2(34f, -10f), new List<GComponent>()
                {
                    new RigidBody(new Vector2(1f * UnitSize, 3f * UnitSize), Vector2.Zero, isStatic: true),
                    new Furniture()
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

            /*AddGameObjects(new GameObject[]
            {
                new TextObject("testtext1", ContentStorer.Fonts["Coolvetica"], "Hola buenas", Vector2.Zero, false)
                {
                    TLPosition = new Vector2(10f, 10f)
                },
                new TextObject("testtext2", ContentStorer.Fonts["Pixel"], "Hola buenas", Vector2.Zero, false)
                {
                    TRPosition = new Vector2(RDEGame.UpscaledScrWidth - 10, 10f)
                },
            });*/

#if DEBUG
            AddGameObjects(new GameObject[]
            {
                new TextObject("FPS", ContentStorer.Fonts["testfont"], "0", new Vector2(RDEGame.ScreenWidth * RDEGame.ScaleFactor - 60f, 30f), false, initialComponents: new List<GComponent>()
                {
                    new FPSCounter()
                })
                {
                    Color = Color.LightGreen
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

            FindWithTag("Camera").GetComponent<CameraFollow>().SetTarget(FindWithTag("Player") as WorldObject, true);
        }
    }
}
