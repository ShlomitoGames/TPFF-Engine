using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace RDEngine.Engine.UI
{
    public class UIElement : GameObject
    {
        public UIElement(string tag, Scene scene, Texture2D texture, Vector2 position, GameObject parent = null, List<GComponent> initialComponents = null) : base(tag, scene, texture, position, parent, initialComponents)
        {

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            if (_texture == null) return;

            spriteBatch.Draw(_texture, Vector2.Floor(Position - Scene.CameraPos + Vector2.One), Color);
        }
    }
}