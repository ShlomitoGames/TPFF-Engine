using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RDEngine.Engine;
using RDEngine.Engine.Physics;
using System.Collections.Generic;

namespace RDEngine.GameScripts
{
    public class Ground : WorldObject
    {
        private Vector2 _size;

        public Ground(string tag, Texture2D texture, Vector2 position, Vector2 size) : base(tag, texture, position,
            initialComponents: new List<GComponent>()
            {
                new RigidBody(size * texture.Bounds.Size.ToVector2(), Vector2.Zero, mass: 100, isStatic: true)
            })
        {
            _size = size;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            if (Texture == null) return;

            for (int i = 0; i < _size.Y; i++)
            {
                for (int j = 0; j < _size.X; j++)
                {
                    Vector2 pos = new Vector2(j * Texture.Width, i * Texture.Height) + Position -Scene.WorldCameraPos + Vector2.One;
                    spriteBatch.Draw(Texture, pos, Color.White);
                }
            }
        }
    }
}