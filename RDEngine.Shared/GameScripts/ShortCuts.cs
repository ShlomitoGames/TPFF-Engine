using Microsoft.Xna.Framework.Input;
using RDEngine.Engine;

namespace RDEngine.GameScripts
{
    public class ShortCuts : GComponent
    {
        public override void Update()
        {
            if (Input.Instance.GetKey(Keys.R, KeyGate.Down))
                SceneHandler.LoadScene(new TestScene());
            if (Input.Instance.GetKey(Keys.H, KeyGate.Down))
                ShowHitboxes = !ShowHitboxes;
        }
    }
}