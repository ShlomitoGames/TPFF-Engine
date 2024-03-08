using RDEngine.Engine;
using RDEngine.Engine.Animation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace RDEngine.GameScripts
{
    internal class Fade : GComponent
    {
        private Animator _anim;

        private int _fadeDuration;

        private float _fadeOutTime;
        private bool _restart;

        public Fade()
        {
            _fadeDuration = 1000;

            _fadeOutTime = 0f;
            _restart = false;
        }

        public override void Start()
        {
            _anim = new Animator(new Dictionary<string, Animation>()
            {
                {
                    "fadein", new Animation(false, new AnimLayer[]
                    {
                        new AnimLayer(new Tuple<int, int>[]
                        {
                            new Tuple<int, int>(_fadeDuration, 255),
                            new Tuple<int, int>(0, 0)
                        }, 0)
                    })
                },
                {
                    "fadeout", new Animation(false, new AnimLayer[]
                    {
                        new AnimLayer(new Tuple<int, int>[]
                        {
                            new Tuple<int, int>(1000, 0),
                            new Tuple<int, int>(0, 255)
                        }, 0)
                    })
                }
            }, "fadein", ints: new int[1], bools: new bool[1]);

            Parent.AddComponent(_anim);
        }

        public override void Update()
        {
            Parent.Color = new Microsoft.Xna.Framework.Color(0, 0, 0, _anim.Ints[0]);

            if (_restart)
            {
                _fadeOutTime += Time.DeltaTime;

                if (_fadeOutTime >= _fadeDuration * 2 / 1000)
                {
                    SceneHandler.LoadScene(new Scene1(), true);
                }
            }
        }

        public void FadeOutRestart()
        {
            _restart = true;

            _anim.SetAnimation("fadeout");
        }
    }
}
