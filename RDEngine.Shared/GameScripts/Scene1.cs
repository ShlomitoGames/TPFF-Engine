using Microsoft.Xna.Framework;
using RDEngine.Engine;
using System;
using System.Collections.Generic;
using System.Text;
using RDEngine.Engine.Physics;
using RDEngine.Engine.Animation;
using RDEngine.Engine.UI;

namespace RDEngine.GameScripts
{
    public class Scene1 : Scene
    {
        public Scene1() : base(Color.YellowGreen, 16) { }

        public override void Initialize()
        {
            base.Initialize();

            _gameObjects = new List<WorldObject>()
            {
                new Ground("Ground1", ContentStorer.WhitePixel, new Vector2(-8f, 0f), new Vector2(1f, 10f), new List<GComponent>()
                {
                    new RigidBody(new Vector2(1 * UnitSize, 10 * UnitSize), Vector2.Zero, false, true)
                })
                {
                    Scale = Vector2.One * UnitSize,
                    Color = Color.Brown
                },

                new WorldObject("Player", ContentStorer.WhitePixel, Vector2.Zero, initialComponents: new List<GComponent>()
                {
                    new Player(100),
                    new RigidBody(Vector2.One * UnitSize, Vector2.Zero)
                })
                {
                    Scale = Vector2.One * UnitSize
                },

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

                new WorldObject("TestBlock2", ContentStorer.WhitePixel, new Vector2(-1f,3f), new List<GComponent>()
                {
                    new RigidBody(new Vector2(UnitSize, UnitSize), Vector2.Zero, mass: 5, drag: 5f)
                })
                {
                    Scale = Vector2.One * UnitSize,
                    Color = Color.AliceBlue
                },

                new WorldObject("Camera", null, Vector2.Zero, initialComponents: new List<GComponent>()
                {
                    new CameraFollow(),
                    new ShortCuts()
                })
            };
            _uiObjects = new List<UIObject>()
            {
                new TextObject("FPS", ContentStorer.Fonts["testfont"], "0", new Vector2(RDEGame.ScreenWidth * RDEGame.ScaleFactor - 60f, 30f), false, initialComponents: new List<GComponent>()
                {
                    new FPSCounter()
                })
                {
                    Color = Color.LightGreen
                }
            };
        }

        public override void Start()
        {
            base.Start();

            FindWithTag("Camera").GetComponent<CameraFollow>().SetTarget(FindWithTag("Player"));
        }
    }
}
