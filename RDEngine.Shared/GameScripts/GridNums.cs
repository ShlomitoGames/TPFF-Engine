﻿using RDEngine.Engine;
using RDEngine.Engine.UI;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace RDEngine.GameScripts
{
    internal class GridNums : GComponent
    {
        public static List<TextObject> texts = new List<TextObject>();

        public static void ToggleTexts()
        {
            foreach (var text in texts)
            {
                text.Enabled = !text.Enabled;
            }
        }

        public override void Start()
        {
            for (int i = -50; i < 50; i++)
            {
                for (int j = /*-150*/-50; j < /*10*/50; j++)
                {
                    TextObject text = new TextObject($"Grid[{i},{j}]", ContentStorer.Fonts["Arial"], $"[{i},{j}]", new Vector2(i, j) * RDEGame.ScaleFactor * Parent.Scene.UnitSize - new Vector2(20f,20f), true);
                    text.Scale = Vector2.One * 0.5f;
                    text.SetParent(this.Parent);
                    Parent.Scene.AddGameObject(text);
                    texts.Add(text);
                    text.Enabled = false;
                }
            }
        }
    }
}
