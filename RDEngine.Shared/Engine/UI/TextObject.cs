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

        public TextObject(string tag, Scene scene, SpriteFont font, string text, Vector2 position, bool isWorldPos, GameObject parent = null, List<GComponent> initialComponents = null) : base(tag, scene, null, position, isWorldPos, parent, initialComponents)
        {
            _font = font;
            Text = text;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            
            Vector2 pos = (_isWorldPos) ? Position - Vector2.Floor(Scene.CameraPos) : Position;
            spriteBatch.DrawString(_font, Text, pos - Size / 2f, Color, 0f, Vector2.Zero, Scale, Effects, Layer);
        }
    }
}