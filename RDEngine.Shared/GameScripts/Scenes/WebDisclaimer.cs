﻿using Microsoft.Xna.Framework;
using RDEngine.Engine;
using RDEngine.Engine.UI;
using RDEngine.Engine.Animation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace RDEngine.GameScripts.Scenes
{
    internal class WebDisclaimer : Scene
    {
        private UIObject _title1, _title2, _disclaimer;

        public WebDisclaimer() : base(new Color(0x10, 0x10, 0x10, 0xff))
        {

        }

        public override void Initialize()
        {
            base.Initialize();

            _title1 = new TextObject("title1", ContentStorer.Fonts["Pixel"], "Please play the Windows version below", Vector2.Zero, false)
            {
                Scale = Vector2.One * 0.75f,
                CCPosition = new Vector2(0f, -25f)
            };
            _title2 = new TextObject("title2", ContentStorer.Fonts["Pixel"], "if you experience performance issues", Vector2.Zero, false)
            {
                Scale = Vector2.One * 0.75f,
                CCPosition = new Vector2(0f, 25f)
            };
            _disclaimer = new TextObject("disclaimer", ContentStorer.Fonts["Pixel"], "It's not my fault, I swear!", Vector2.Zero, false)
            {
                Scale = Vector2.One * 0.35f,
                BCPosition = Vector2.Zero
            };

            AddGameObjects(new GameObject[]
            {
                _title1,
                _title2,
                _disclaimer,

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
        private float _time = 0;
        public override void Update()
        {
            if (_time >= 2.5f)
                FindWithTag("Fade").GetComponent<Fade>().FadeOut(new Level1());

            _time += Time.DeltaTime;
#if DEBUG
            if (Input.GetKey(Microsoft.Xna.Framework.Input.Keys.R, KeyGate.Down))
                SceneHandler.ReloadScene();
#endif
        }
    }
}
