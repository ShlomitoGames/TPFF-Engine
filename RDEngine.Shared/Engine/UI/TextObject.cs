using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace RDEngine.Engine.UI
{
    public class TextObject : UIObject
    {
        private SpriteFont _font;
        public string Text { get; set; }

        public override Vector2 Size
        {
            get
            {
                return _font.MeasureString(Text) * Scale;
            }
            set
            {
                Scale = value / _font.MeasureString(Text);
            }
        }

        public TextObject(string tag, SpriteFont font, string text, Vector2 position, bool isWorldPos, List<GComponent> initialComponents = null, List<UIObject> children = null) : base(tag, null, position, isWorldPos, initialComponents, children)
        {
            _font = font;
            Text = text;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!Enabled) return;

            base.Draw(spriteBatch);
            
            Vector2 pos = (_isWorldPos) ? AbsolutePos - Vector2.Floor(Scene.CameraPos) + (Vector2.One * 2f * RDEGame.ScaleFactor) : AbsolutePos;
            //If it's UI in the world, scale appropiately with the ScaleFactor, with the baseline scaling being 4
            Vector2 scale = _isWorldPos ? Scale * (RDEGame.ScaleFactor / 4f) : Scale;
            spriteBatch.DrawString(_font, Text, pos - Size / 2f, Color, 0f, Vector2.Zero, scale, Effects, LayerDepth);
        }
    }
}