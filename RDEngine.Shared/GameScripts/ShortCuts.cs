using Microsoft.Xna.Framework.Input;
using RDEngine.Engine;

namespace RDEngine.GameScripts
{
    public class ShortCuts : GComponent
    {
        public override void Update()
        {
            if (Input.GetKey(Keys.R, KeyGate.Down))
                SceneHandler.ReloadScene();

            if (Input.GetKey(Keys.T, KeyGate.Down))
                PersistentVars.Hardcore = !PersistentVars.Hardcore;

#if DEBUG
                if (Input.GetKey(Keys.H, KeyGate.Down))
                ShowHitboxes = !ShowHitboxes;
            if (Input.GetKey(Keys.G, KeyGate.Down))
                GridNums.ToggleTexts();
#endif
        }
    }
}