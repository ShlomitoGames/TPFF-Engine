using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace RDEngine.Engine.UI
{
    public class UIObject : GameObject
    {
        //Whether its position is in the world or fixed on the screen
        protected bool _isWorldPos;

        //Real size
        public virtual Vector2 Size
        {
            get
            {
                return new Vector2(Texture.Width, Texture.Height) * Scale;
            }
            set
            {
                Scale = value / new Vector2(Texture.Width, Texture.Height);
            }
        }

        //public Vector2 TLPosition

        public UIObject(string tag, Texture2D texture, Vector2 position, bool isWorldPos, List<GComponent> initialComponents = null, List<UIObject> children = null) : base(tag, texture, position, initialComponents, (children != null) ? children.ConvertAll(x => x as GameObject) : null)
        {
            _isWorldPos = isWorldPos;
            Scale = Vector2.One;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!Enabled) return;

            base.Draw(spriteBatch);

            if (Texture == null) return;

            // UIObjects get drawn with their positions at the center, unlike WorldObjects
            //The Vector2.Floor is very important
            Vector2 pos = _isWorldPos ? AbsolutePos - Vector2.Floor(Scene.CameraPos) + (Vector2.One * 2f * RDEGame.ScaleFactor) : AbsolutePos;
            spriteBatch.Draw(Texture, pos - Size / 2f, null, Color, 0f, Vector2.Zero, Scale, Effects, LayerDepth);
        }
    }
}