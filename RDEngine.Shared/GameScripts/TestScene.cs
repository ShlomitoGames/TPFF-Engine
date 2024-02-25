using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using RDEngine.Engine;
using RDEngine.Engine.Physics;
using RDEngine.Engine.UI;
using System.Collections.Generic;

namespace RDEngine.GameScripts
{
    public class TestScene : Scene
    {
        private Texture2D _playerTexture;
        private Texture2D _groundTexture;
        private Texture2D _koopaTexture;

        private SpriteFont _wreckside;
        private SpriteFont _testfont;

        public TestScene() : base()
        {
            
        }

        public override void Initialize()
        {
            base.Initialize();

            _playerTexture = ContentStorer.Textures["Mario"];
            _groundTexture = ContentStorer.Textures["Block"];
            _koopaTexture = ContentStorer.Textures["Koopa"];

            _wreckside = ContentStorer.Fonts["wreckside"];
            _testfont = ContentStorer.Fonts["testfont"];

            _gameObjects = new List<WorldObject>()
            {
                new WorldObject("Player", this, _playerTexture, new Vector2(3f, 4f), null, new List<GComponent>()
                {
                    new RigidBody(new Vector2(12f, 16f), new Vector2(2f, 0f), gravity: 500f, drag: 1f, mass: 1f),
                    new Player(200f, 230f)
                }),
                new WorldObject("Koopa", this, _koopaTexture, new Vector2(8f, 4f), null, new List<GComponent>()
                {
                    new RigidBody(_koopaTexture.Bounds.Size.ToVector2(), Vector2.Zero, gravity: 200f, drag: 1f, mass: 10f),
                    new Entity(100f)
                }),
                new Ground("Ground1", this, _groundTexture, new Vector2(0f, 14f), new Vector2(50f, 2f)),
                new Ground("Ground2", this, _groundTexture, new Vector2(6f, 11f), new Vector2(4f, 1f)),
                new Ground("Ground3", this, _groundTexture, new Vector2(0f, 8f), new Vector2(1f, 10f)),
                new Ground("Ground4", this, _groundTexture, new Vector2(10f, 11f), new Vector2(4f, 1f)),
                new WorldObject("Camera", this, null, Vector2.Zero, null, new List<GComponent>()
                {
                    new CameraFollow()
                    {
                        Offset = new Vector2(50, 0)
                    }
                }),
                new WorldObject("ShortCuts", this, null, Vector2.Zero,initialComponents: new List<GComponent>()
                {
                    new ShortCuts()
                })
            };
            _uiObjects = new List<UIObject>()
            {
                new TextObject("testtext", this, _wreckside, "Yo no", new Vector2(0,0), false)
                {

                    Position = new Vector2(100,100)
                },
                new TextObject("testtext2", this, _wreckside, "Yo me muevo", new Vector2(0,0), true)
                {
                    Position = new Vector2(500,500)
                },
                new TextObject("fps", this, _testfont, "0", new Vector2(RDEGame.ScreenWidth * RDEGame.ScaleFactor - 60, 30), false,
                initialComponents: new List<GComponent>(){ new FPSCounter() })
                {
                    Color = Color.LightGreen
                }
            };

            CameraPos = Vector2.Zero;
        }

        public override void Start()
        {
            base.Start();

            CameraFollow cam = FindWithTag("Camera").GetComponent<CameraFollow>();
            cam.SetTarget(FindWithTag("Player"));
            //cam.Enabled = false;
            //cam.Offset = new Vector2(10, 0);
            //CameraPos = new Vector2(100f, 0f);
        }
    }
}