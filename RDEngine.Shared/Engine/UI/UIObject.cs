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

        public Vector2 TLPosition
        {
            get
            {
                return Position;
            }
            set
            {
                Position = value;
            }
        }
        public Vector2 TRPosition
        {
            get
            {
                return new Vector2(Position.X + Size.X - RDEGame.UpscaledScrWidth, Position.Y);
            }
            set
            {
                Position = new Vector2(value.X - Size.X + RDEGame.UpscaledScrWidth, value.Y);
            }
        }
        public Vector2 BLPosition
        {
            get
            {
                return new Vector2(Position.X, Position.Y + Size.Y - RDEGame.UpscaledScrHeight);
            }
            set
            {
                Position = new Vector2(value.X, value.Y - Size.Y + RDEGame.UpscaledScrHeight);
            }
        }
        public Vector2 BRPosition
        {
            get
            {
                return Position + Size - RDEGame.UpscaledScrSize;
            }
            set
            {
                Position = value - Size + RDEGame.UpscaledScrSize;
            }
        }
        public Vector2 TCPosition
        {
            get
            {
                return Position - new Vector2(RDEGame.UpscaledScrWidth * 0.5f - Size.X * 0.5f, 0f);
            }
            set
            {
                Position = value + new Vector2(RDEGame.UpscaledScrWidth * 0.5f - Size.X * 0.5f, 0f);
            }
        }
        public Vector2 BCPosition
        {
            get
            {
                return Position - new Vector2(RDEGame.UpscaledScrWidth * 0.5f - Size.X * 0.5f, RDEGame.UpscaledScrHeight - Size.Y);
            }
            set
            {
                Position = value + new Vector2(RDEGame.UpscaledScrWidth * 0.5f - Size.X * 0.5f, RDEGame.UpscaledScrHeight - Size.Y);
            }
        }
        public Vector2 CLPosition
        {
            get
            {
                return Position - new Vector2(0f, RDEGame.UpscaledScrHeight * 0.5f - Size.Y * 0.5f);
            }
            set
            {
                Position = value + new Vector2(0f, RDEGame.UpscaledScrHeight * 0.5f - Size.Y * 0.5f);
            }
        }
        public Vector2 CRPosition
        {
            get
            {
                return Position - new Vector2(RDEGame.UpscaledScrWidth - Size.X, RDEGame.UpscaledScrHeight * 0.5f - Size.Y * 0.5f);
            }
            set
            {
                Position = value + new Vector2(RDEGame.UpscaledScrWidth - Size.X, RDEGame.UpscaledScrHeight * 0.5f - Size.Y * 0.5f);
            }
        }
        public Vector2 CCPosition
        {
            get
            {
                return Position + Size * 0.5f - RDEGame.UpscaledScrSize * 0.5f;
            }
            set
            {
                Position = value - Size * 0.5f + RDEGame.UpscaledScrSize * 0.5f;
            }
        }

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
            Vector2 topLeft = AbsolutePos;
            Vector2 pos = _isWorldPos ? topLeft - Vector2.Floor(Scene.CameraPos) + (Vector2.One * 2f * RDEGame.ScaleFactor) : topLeft;
            //If it's UI in the world, scale appropiately with the ScaleFactor, with the baseline scaling being 4
            Vector2 scale = _isWorldPos ? Scale * (RDEGame.ScaleFactor / 4f) : Scale;
            spriteBatch.Draw(Texture, pos, null, Color, 0f, Vector2.Zero, scale, Effects, LayerDepth);
        }
    }
}