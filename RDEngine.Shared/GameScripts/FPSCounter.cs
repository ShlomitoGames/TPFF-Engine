using Microsoft.Xna.Framework;
using RDEngine.Engine;
using RDEngine.Engine.UI;
using System.Diagnostics;

namespace RDEngine.GameScripts
{
    public class FPSCounter: GComponent
    {
        private int _fps = 0, _totalFrames = 0;
        private float _lastFpsUpdate, _fpsUpdateInterval = 0.5f;
        private TextObject _parentText;

        public override void Start()
        {
            _parentText = Parent as TextObject;

            if (_parentText == null)
                Enabled = false;
        }

        public override void Update()
        {
            //Updates FPS
            _totalFrames++;
            if ((float)Time.GameTime.TotalGameTime.TotalSeconds > _lastFpsUpdate + _fpsUpdateInterval)
            {
                _fps = (int)(_totalFrames / _fpsUpdateInterval);
                _totalFrames = 0;
                _lastFpsUpdate = (float)Time.GameTime.TotalGameTime.TotalSeconds;
            }

            _parentText.Text = _fps.ToString();
        }
    }
}