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

        public Ground(string tag, Scene scene, Texture2D texture, Vector2 position, Vector2 size) : base(tag, scene, texture, position,
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

            if (_texture == null) return;

            for (int i = 0; i < _size.Y; i++)
            {
                for (int j = 0; j < _size.X; j++)
                {
                    Vector2 pos = new Vector2(j * _texture.Width, i * _texture.Height) + Position -Scene.WorldCameraPos + Vector2.One;
                    spriteBatch.Draw(_texture, pos, Color.White);
                }
            }
        }
    }
}