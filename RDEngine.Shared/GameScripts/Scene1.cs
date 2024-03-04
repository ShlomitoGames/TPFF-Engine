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
        public Scene1() : base(Color.DarkCyan, 16) { }

        public override void Initialize()
        {
            base.Initialize();

            AddGameObject(
                new WorldObject("Test", null, Vector2.Zero, new List<GComponent>()
                {
                    new LayoutLoader()
                })
            );

            AddGameObject(
                new WorldObject("Player", ContentStorer.WhiteSquare, Vector2.Zero, initialComponents: new List<GComponent>()
                {
                    new Player(750),
                    new RigidBody(Vector2.One * UnitSize, Vector2.Zero, drag: 5f)
                })
                {
                    LayerDepth = 0.5f
                }
            );

            /*WorldObject[] walls = new WorldObject[]
            {
                new TiledTexture("Ground1", ContentStorer.WhiteSquare, new Vector2(-7f, -2f), new Vector2(12f, 1f), new List<GComponent>()
                {
                    new RigidBody(new Vector2(12f, 1f) * UnitSize, Vector2.Zero, false, true)
                }),
                new Ground("Ground2", ContentStorer.WhiteSquare, new Vector2(21f, -2f), new Vector2(40f, 1f), new List<GComponent>()
                {
                    new RigidBody(new Vector2(40f, 1f) * UnitSize, Vector2.Zero, false, true)
                }),
                new Ground("Ground3", ContentStorer.WhiteSquare, new Vector2(-5f, -6f), new Vector2(4f, 1f), new List<GComponent>()
                {
                    new RigidBody(new Vector2(4f, 1f) * UnitSize, Vector2.Zero, false, true)
                }),

                new Ground("Wall1", ContentStorer.WhiteSquare, new Vector2(-13.5f, -9.5f), new Vector2(1f, 16f), new List<GComponent>()
                {
                    new RigidBody(new Vector2(1f, 16f) * UnitSize, Vector2.Zero, false, true)
                }),
                new Ground("Wall2", ContentStorer.WhiteSquare, new Vector2(-3.5f, -4f), new Vector2(1f, 5f), new List<GComponent>()
                {
                    new RigidBody(new Vector2(1f, 5f) * UnitSize, Vector2.Zero, false, true)
                }),
                new Ground("Ground4", ContentStorer.WhiteSquare, new Vector2(-7f, -17f), new Vector2(12f, 1f), new List<GComponent>()
                {
                    new RigidBody(new Vector2(12f, 1f) * UnitSize, Vector2.Zero, false, true)
                })
            };*/

            AddGameObjects(new WorldObject[]
            {
                new WorldObject("TestBlock", ContentStorer.WhitePixel, new Vector2(-6.5f, 3f), new List<GComponent>()
                {
                    new Animator(new Dictionary<string, Animation>()
                    {
                        {"move", new Animation(true, new AnimLayer[]
                            {
                                new AnimLayer(new Tuple<int, float>[]
                                {
                                    new Tuple<int, float>(500, -6.5f),
                                    new Tuple<int, float>(100, -2f),
                                    new Tuple<int, float>(500, -2f),
                                    new Tuple<int, float>(0, -6.5f)
                                }, 0)
                            })
                        }
                    }, "move", floats: new float[1]),
                    new RigidBody(new Vector2(2 * UnitSize, 2 * UnitSize), Vector2.Zero, mass: 10, isKinematic: true),
                    new MovingBlock()
                })
                {
                    Scale = Vector2.One * 2 * UnitSize,
                    Color = Color.Yellow
                },

                new WorldObject("Camera", null, Vector2.Zero, initialComponents: new List<GComponent>()
                {
                    new CameraFollow(),
                    new ShortCuts()
                })
            });

            UIObject[] uiObjects = new UIObject[]
            {
                new TextObject("FPS", ContentStorer.Fonts["testfont"], "0", new Vector2(RDEGame.ScreenWidth * RDEGame.ScaleFactor - 60f, 30f), false, initialComponents: new List<GComponent>()
                {
                    new FPSCounter()
                })
                {
                    Color = Color.LightGreen
                }
            };

            //_worldObjects.AddRange(walls);
            AddGameObjects(uiObjects);
        }

        public override void Start()
        {
            base.Start();

            FindWithTag("Camera").GetComponent<CameraFollow>().SetTarget(FindWithTag("Player"));
        }
    }
}
