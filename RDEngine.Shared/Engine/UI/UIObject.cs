using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace RDEngine.Engine.UI
{
    public class UIObject : GameObject
    {
        //Size relative to texture size
        public Vector2 Scale { get; set; }

        //Whether its position is in the world or fixed on the screen
        protected bool _isWorldPos;

        //Real size
        public virtual Vector2 Size
        {
            get
            {
                return Texture.Bounds.Size.ToVector2() * Scale;
            }
            set
            {
                Scale = value / Texture.Bounds.Size.ToVector2();
            }
        }

        public UIObject(string tag, Scene scene, Texture2D texture, Vector2 position, bool isWorldPos, GameObject parent = null, List<GComponent> initialComponents = null) : base(tag, scene, texture, position, parent, initialComponents)
        {
            _isWorldPos = isWorldPos;
            Scale = Vector2.One;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            if (Texture == null) return;

            // UIObjects get drawn with their positions at the center, unlike WorldObjects
            //The Vector2.Floor is very important
            Vector2 pos = _isWorldPos ? Position - Vector2.Floor(Scene.CameraPos) : Position;
            spriteBatch.Draw(Texture, pos - Size / 2f, null, Color, 0f, Vector2.Zero, Scale, Effects, Layer);
        }
    }
}