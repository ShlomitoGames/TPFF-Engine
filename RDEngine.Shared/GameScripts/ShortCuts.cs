﻿using Microsoft.Xna.Framework.Input;
using RDEngine.Engine;

namespace RDEngine.GameScripts
{
    public class ShortCuts : GComponent
    {
        public override void Update()
        {
            if (Input.GetKey(Keys.R, KeyGate.Down))
                SceneHandler.LoadScene(new Scene1());
            if (Input.GetKey(Keys.H, KeyGate.Down))
                ShowHitboxes = !ShowHitboxes;
        }
    }
}