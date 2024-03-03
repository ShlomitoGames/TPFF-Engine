using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RDEngine.Engine;
using RDEngine.Engine.Physics;
using System.Collections.Generic;
using System.Diagnostics;

namespace RDEngine.GameScripts
{
    public class Ground : WorldObject
    {
        private Vector2 _size;

        public Ground(string tag, Texture2D texture, Vector2 position, Vector2 size, List<GComponent> initialComponents = null, List<WorldObject> children = null) : base(tag, texture, position, initialComponents, children)
        {
            _size = size;
        }

        internal override void Draw(SpriteBatch spriteBatch)
        {
            Vector2 offset = AbsolutePos - Scene.WorldCameraPos + Vector2.One * 2f - ((new Vector2(Texture.Width, Texture.Height) * _size) * Scale / 2f);

            if (Texture == null) return;

            for (int i = 0; i < _size.Y; i++)
            {
                for (int j = 0; j < _size.X; j++)
                {
                    Vector2 pos = new Vector2(j * Texture.Width * Scale.X, i * Texture.Height * Scale.Y) + offset;
                    spriteBatch.Draw(Texture, Vector2.Floor(pos), null, Color, 0f, Vector2.Zero, Scale, Effects, Layer);
                }
            }
        }
    }
}