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

        internal override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            
            Vector2 pos = (_isWorldPos) ? AbsolutePos - Vector2.Floor(Scene.CameraPos) + (Vector2.One * 2f * RDEGame.ScaleFactor) : AbsolutePos;
            spriteBatch.DrawString(_font, Text, pos - Size / 2f, Color, 0f, Vector2.Zero, Scale, Effects, Layer);
        }
    }
}