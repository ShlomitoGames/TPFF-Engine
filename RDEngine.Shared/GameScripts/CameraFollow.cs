using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RDEngine.Engine;

namespace RDEngine.GameScripts
{
    public class CameraFollow : GComponent
    {
        public GameObject Target { get; set; }

        public float SmoothSpeed = 1.5f;
        public Vector2 Offset = Vector2.Zero;
        
        private Vector2 _screenSize;

        private Vector2 _camPos
        {
            get
            {
                return Parent.Scene.CameraPos / RDEGame.ScaleFactor;
            }
            set
            {
                Parent.Scene.CameraPos = value * RDEGame.ScaleFactor;
            }
        }
        
        private Vector2 _camOrigin
        {
            get
            {
                return _camPos + _screenSize / 2;
            }
            set
            {
                Parent.Scene.CameraPos = (value - _screenSize / 2) * RDEGame.ScaleFactor;
            }
        }

        public CameraFollow(GameObject target = null) : base()
        {
            SetTarget(target);
            _screenSize = RDEGame.ScreenSize.ToVector2();
        }

        public override void LateUpdate()
        {
            Vector2 desiredPosition = Target.Origin + Offset;
            //Vector2 smoothedPosition = Vector2.SmoothStep(_camOrigin, desiredPosition, SmoothSpeed * Time.Instance.DeltaTime); //Not freame-rate independent
            Vector2 smoothedPosition = Vector2.Lerp(_camOrigin, desiredPosition, SmoothSpeed * Time.DeltaTime);

            _camOrigin = smoothedPosition;
        }

        public void SetTarget(GameObject target)
        {
            Target = target;
        }
    }
}