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

        public CameraFollow(GameObject target = null) : base()
        {
            SetTarget(target);
        }

        public override void LateUpdate()
        {
            if (Target == null)
                return;

            Vector2 desiredPosition = Target.Position + Offset;
            //Vector2 smoothedPosition = Vector2.SmoothStep(_camOrigin, desiredPosition, SmoothSpeed * Time.Instance.DeltaTime); //Not freame-rate independent
            Vector2 smoothedPosition = Vector2.Lerp(Parent.Scene.WorldCameraOrigin, desiredPosition, SmoothSpeed * Time.DeltaTime);

            Parent.Scene.WorldCameraOrigin = smoothedPosition;
        }

        public void SetTarget(GameObject target)
        {
            Target = target;
        }
    }
}